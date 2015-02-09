using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdjrSimpleElevatorCommon;

namespace tdjrSimpleElevatorControl
{
    public class SimpleElevatorControl
    {
        #region Constructors 

        public SimpleElevatorControl(SimpleElevatorBank elevatorBank)
        {
            this.ElevatorBank = elevatorBank;
            this.RequestQueue = new Queue<SimpleElevatorStopRequest>();
            this.ElevatorCommands = new Dictionary<Int32, ElevatorCommand>();
        }

        #endregion Constructors

        private Dictionary<Int32, ElevatorCommand > ElevatorCommands {get; set;}

        #region Public Properties

        /// <summary>
        /// Queue for requests. An actual Queue is overkill for out simplified one elevator version
        /// </summary>
        public Queue<SimpleElevatorStopRequest> RequestQueue { get; private set; }

        /// <summary>
        /// A bank of elevators. For now it only has one
        /// </summary>
        public SimpleElevatorBank ElevatorBank { get; private set; }

        #endregion Public Properties

        /// <summary>
        /// Get the ElevatorStatus of the current Elevator. 
        /// NOTE: needs to extend to support banks of more than one elevator
        /// </summary>
        /// <param name="elevatorID"></param>
        /// <returns></returns>
        public tdjrSimpleElevatorCommon.ElevatorStatus GetCurrentElevatorStatus(int elevatorID)
        {
            SimpleElevator elevator = this.GetElevator(elevatorID);
            if (elevator != null) {
                return elevator.GetCurrentStatus();
            } else
                return null;
        }

        /// <summary>
        /// Sequence the Controller through its states
        /// </summary>
        /// <param name="queuedRequests"></param>
        public void Execute( Queue<SimpleElevatorStopRequest> queuedRequests ) {
            if (queuedRequests != null &&
                queuedRequests.Any())
                this.RequestQueue.EnqueueFrom(queuedRequests);

            this.ProcessRequestQueue();
            this.ProcessCommands();
        }

        /// <summary>
        /// Process all queued requests to generate commands to be issued this cycle
        /// </summary>
        private void ProcessRequestQueue() { 
            // is there a Request queue and does it have anything
            if (this.RequestQueue != null && this.RequestQueue.Peek() != null)
            {
                // copy of elevator IDs. If a command is setup for an elevator it is removed from this list
                List<Int32> elevatorIDs = ElevatorBank.Elevators.Select(x => x.ElevatorID).ToList();

                // copy the current queue to a list. This list will be manipulated then the queue will be rebuilt from it
                List<SimpleElevatorStopRequest> requestList = this.RequestQueue.ToList();

                foreach (var req in requestList) { req.RequestStatus = ElevatorStopRequestStatus.Evaluating; }

                // can we handle any without moving any lifts
                List<Int32> currentLandings = this.ElevatorBank.GetCurrentLandings();

                // where are the lifts currently?
                // shortcut here since there is only one elevator for this scenario
                Int32 currentLanding = currentLandings.FirstOrDefault();

                // get requests for current landings
                List<SimpleElevatorStopRequest> immediates = new List<SimpleElevatorStopRequest>();
                immediates.AddRange(requestList.Where(x => x.RequestedLandingID == currentLanding));

                if (immediates.Any() == true)
                {
                    IEnumerable<SimpleElevator> cmdOpenDoorsElevators = ElevatorBank.Elevators.Where(x => x.CurrentLanding == currentLanding).Distinct();

                    // cheat for single elevator assumption
                    Int32 openElevatorID = cmdOpenDoorsElevators.FirstOrDefault().ElevatorID;

                    this.ElevatorCommands.Add(openElevatorID, ElevatorCommand.OpenDoors);

                    requestList.RemoveAll(x => immediates.Exists(i => i.RequestID == x.RequestID));

                    elevatorIDs.Remove(openElevatorID);
                }

                if (elevatorIDs.Any())
                {
                    // single elevator cheat
                    SimpleElevator elevatorCHEAT = ElevatorBank.Elevators.FirstOrDefault();

                    // seperate queued requests by type
                    List<SimpleElevatorStopRequest> dropOffs = requestList.Where(x => x.RequestType == ElevatorStopRequestType.Dropoff).ToList();
                    List<SimpleElevatorStopRequest> pickUps = requestList.Where(x => x.RequestType == ElevatorStopRequestType.Pickup).ToList();

                    DirectionOfTravel direction = this.ResolveBestDirection(
                        elevatorCHEAT.CurrentLanding,
                        elevatorCHEAT.CurrentDirection,
                        dropOffs.Select(x => x.RequestedLandingID),
                        pickUps.Select(x => x.RequestedLandingID)
                        );


                    switch (direction)
                    {
                        case DirectionOfTravel.Unspecified:
                            break;

                        case DirectionOfTravel.Ascend:
                            this.ElevatorCommands.Add(elevatorCHEAT.ElevatorID, ElevatorCommand.Ascend);
                            elevatorIDs.Remove(elevatorCHEAT.ElevatorID);
                            break;

                        case DirectionOfTravel.Descend:
                            this.ElevatorCommands.Add(elevatorCHEAT.ElevatorID, ElevatorCommand.Descend);
                            elevatorIDs.Remove(elevatorCHEAT.ElevatorID);
                            break;
                    }

                }

                this.RebuidRequestQueue(requestList);
            }
        }

        /// <summary>
        /// Process (dispatch) the commands generated from processing the request gueue
        /// </summary>
        private void ProcessCommands()
        { 
            if (this.ElevatorCommands.Any())
            {
                foreach( var cmd in this.ElevatorCommands)
                {
                    SimpleElevator elevator = this.ElevatorBank.GetElevator(cmd.Key);
                    elevator.PerformCommand(cmd.Value);
                }
                this.ElevatorCommands.Clear();
            }
        }

        /// <summary>
        /// Which way should the elevator be sent
        /// </summary>
        /// <param name="currentLanding"></param>
        /// <param name="currentDirection"></param>
        /// <param name="dropoffLandings"></param>
        /// <param name="pickupLandings"></param>
        /// <returns></returns>
        private DirectionOfTravel ResolveBestDirection(
            Int32 currentLanding,
            DirectionOfTravel currentDirection,
            IEnumerable<Int32> dropoffLandings,
            IEnumerable<Int32> pickupLandings)
        {
            switch (currentDirection)
            {
                case DirectionOfTravel.Unspecified:
                    break;

                case DirectionOfTravel.Ascend:
                    if (dropoffLandings.Where(x => x > currentLanding).Any() ||
                         pickupLandings.Where(x => x > currentLanding).Any())
                        return DirectionOfTravel.Ascend;

                    break;

                case DirectionOfTravel.Descend:
                    if (dropoffLandings.Where(x => x < currentLanding).Any() ||
                         pickupLandings.Where(x => x < currentLanding).Any())
                        return DirectionOfTravel.Descend;
                    break;

            }
            if (dropoffLandings.Any())
            {
                IEnumerable<Int32> dropoffs = dropoffLandings.Where(x => x > currentLanding);
                if (dropoffs.Any())
                    return DirectionOfTravel.Ascend;

                dropoffs = dropoffLandings.Where(x => x < currentLanding);
                if (dropoffs.Any())
                    return DirectionOfTravel.Descend;

            }
            else if (pickupLandings.Any())
            {
                IEnumerable<Int32> pickups = pickupLandings.Where(x => x > currentLanding);
                if (pickups.Any())
                    return DirectionOfTravel.Ascend;

                pickups = pickupLandings.Where(x => x > currentLanding);
                if (pickups.Any())
                    return DirectionOfTravel.Ascend;
            }
            return DirectionOfTravel.Unspecified;
        }


        /// <summary>
        /// Helper method to get a SimpleElevator by ID
        /// </summary>
        /// <param name="elevatorID"></param>
        /// <returns></returns>
        private SimpleElevator GetElevator(Int32 elevatorID) {
            return this.ElevatorBank.GetElevator(elevatorID);
        }

        /// <summary>
        /// Rebuild the RequestQueue from a list of SimpleElevatorStopRequests
        /// </summary>
        /// <param name="requestList"></param>
        private void RebuidRequestQueue(List<SimpleElevatorStopRequest> requestList)
        {
            this.RequestQueue.Clear();
            foreach (var request in requestList.OrderBy(x => x.RequestTimeUTC))
            {
                this.EnqueueRequest(request);
            }
        }

        /// <summary>
        /// Add a request to the queue
        /// </summary>
        /// <param name="request"></param>
        private void EnqueueRequest(SimpleElevatorStopRequest request)
        {
            if (this.RequestQueue == null)
                this.RequestQueue = new Queue<SimpleElevatorStopRequest>();
            this.RequestQueue.Enqueue(request);
        }
    }

}
