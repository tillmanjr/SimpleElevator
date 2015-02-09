using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdjrSimpleElevatorCommon;

namespace tdjrSimpleElevatorControl
{
    /// <summary>
    /// An elevator (lift)
    /// </summary>
    public class SimpleElevator
    {
        #region Public Properties

        /// <summary>
        /// Unique ID
        /// </summary>
        public Int32 ElevatorID { get; private set; }

        /// <summary>
        ///  Landing (floor) where the elevator is located or most recently left
        /// </summary>
        public Int32 CurrentLanding { get; private set; }

        /// <summary>
        /// List of the Landings (floors) the elevator can access
        /// </summary>
        public List<Int32> AccessibleLandings { get; set; }

        /// <summary>
        /// what operation is the elevator currently performing
        /// </summary>
        public ElevatorOperation CurrentOperation { get; private set; }

        /// <summary>
        /// Which direction is the elevator traveling or if the doors are open what was its last direction of travel
        /// </summary>
        public DirectionOfTravel CurrentDirection { get; private set; }

        #endregion Public Properties


        #region Public Methods

        /// <summary>
        /// Status of elevator
        /// </summary>
        /// <returns></returns>
        public ElevatorStatus GetCurrentStatus() {
            return new ElevatorStatus(
                this.ElevatorID,
                this.CurrentOperation,
                this.CurrentDirection,
                this.CurrentLanding
                );
        }

        /// <summary>
        /// Command it to do something
        /// </summary>
        /// <param name="command"></param>
        public void PerformCommand(ElevatorCommand command) { 
            switch (command)
            {
                case ElevatorCommand.Ascend:
                    this.PerformAscendOperation();
                    break;

                case ElevatorCommand.Descend:
                    this.PerformDescendOperation();
                    break;

                case ElevatorCommand.OpenDoors:
                    this.PerformOpenDoorsOperation();
                    break;

                case ElevatorCommand.Park:
                    this.PerformParkOperation();
                    break;
            }
        
        }

        #endregion Public Methods


        #region Private Methods

        #region Elevator Operation methods

        /// <summary>
        /// Ascend one floor
        /// </summary>
        private void PerformAscendOperation()
        {
            Int32 maxLanding = this.AccessibleLandings.Max();
            if (this.CurrentLanding < maxLanding)
            {
                Int32 nextLanding = this.AccessibleLandings
                                    .SkipWhile(x => x <= this.CurrentLanding)
                                    .Take(1)
                                    .FirstOrDefault();
                
                this.UpdateElevatorState(
                    nextLanding, 
                    ElevatorOperation.Traveling, 
                    DirectionOfTravel.Ascend);  
            }
        }

        /// <summary>
        /// Descend one floor
        /// </summary>
        private void PerformDescendOperation()
        {
            Int32 minLanding = this.AccessibleLandings.Min();
            if (this.CurrentLanding > minLanding)
            {
                Int32 prevLanding = -1;
                foreach (Int32 landing in this.AccessibleLandings)
                {
                    if (landing == this.CurrentLanding) {
                        break; 
                    }

                    prevLanding = landing;
                }

                this.UpdateElevatorState(
                    prevLanding,
                    ElevatorOperation.Traveling,
                    DirectionOfTravel.Descend);
            }
        }

        /// <summary>
        /// Open doors on current landing
        /// </summary>
        private void PerformOpenDoorsOperation()
        {
            this.UpdateElevatorState(
                    this.CurrentLanding,
                    ElevatorOperation.DoorsOpen,
                    this.CurrentDirection);
        }

        /// <summary>
        /// Park 
        /// </summary>
        private void PerformParkOperation()
        {
            this.UpdateElevatorState(
                    this.CurrentLanding,
                    ElevatorOperation.Parked,
                    DirectionOfTravel.Unspecified);
        }

        /// <summary>
        /// Utility to update common elevator attributes, used after performing a command
        /// </summary>
        /// <param name="currentLanding"></param>
        /// <param name="currentOperation"></param>
        /// <param name="currentDirection"></param>
        private void UpdateElevatorState(
            Int32 currentLanding,
            ElevatorOperation currentOperation,
            DirectionOfTravel currentDirection
            )
        { 
            // grab mutex, token, whatever

            this.CurrentLanding = currentLanding;
            this.CurrentOperation = currentOperation;
            this.CurrentDirection = currentDirection;

            // release mutex, token, whatever
        }

        #endregion Elevator Operation methods


        #region Property Accessors/Mutators

        /// <summary>
        /// Mutator for AccessibleLandings. Assures landings are sorted ascending
        /// </summary>
        /// <param name="accessibleLandings"></param>
        private void SetAccessibleLandings(List<Int32> accessibleLandings)
        {
            this.AccessibleLandings = accessibleLandings.OrderBy(x => x).ToList();
        }

        #endregion Property Accessors/Mutators

        #endregion Private Methods

        /// <summary>
        /// Convenience method for creating a test elevator
        /// </summary>
        /// <returns></returns>
        public static SimpleElevator CreateSimpleElevator() {
            SimpleElevator result = new SimpleElevator();
            result.ElevatorID = 100;
            result.CurrentLanding = 1;
            result.AccessibleLandings = new List<Int32>(6) { 1, 2, 3, 4, 5, 6 };
            result.CurrentDirection = DirectionOfTravel.Unspecified;
            result.CurrentOperation = ElevatorOperation.Parked;

            return result;
        }

    }
}
