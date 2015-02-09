using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdjrSimpleElevatorCommon;

namespace tdjrSimpleElevatorControl
{
    /// <summary>
    /// Simple class to queue SimpleElevatorStopRequests until wich time they can be passed off to a SimpleElevatorController
    /// </summary>
    public class SimpleElevatorRequestQueue : ISimpleElevatorRequestQueue
    {
        #region Public Properties

        public Queue<SimpleElevatorStopRequest> RequestQueue { get; private set; }

        #endregion Public Properties

        #region ISimpleElevatorRequestQueue Implementation

        /// <summary>
        /// Accept a Pickup Request and queue it
        /// </summary>
        /// <param name="landingID"></param>
        /// <param name="requestedDirection"></param>
        public void SubmitPickupRequest(int landingID, DirectionOfTravel requestedDirection)
        {
            SimpleElevatorStopRequest request = SimpleElevatorStopRequest.CreatePickupRequest(landingID, requestedDirection);
            this.EnqueueRequest(request);
        }

        /// <summary>
        /// Accept a Dropoff Request and queue it
        /// </summary>
        /// <param name="elevatorID"></param>
        /// <param name="landingID"></param>
        public void SubmitDropoffRequest(int elevatorID, int landingID)
        {
            SimpleElevatorStopRequest request = SimpleElevatorStopRequest.CreateDropOffRequest(landingID, elevatorID);
            this.EnqueueRequest(request);
        }

        #endregion ISimpleElevatorRequestQueue Implementation

        #region Private Methods

        /// <summary>
        /// Helper to queue the request
        /// </summary>
        /// <param name="request"></param>
        private void EnqueueRequest(SimpleElevatorStopRequest request)
        {
            if (this.RequestQueue == null)
                this.RequestQueue = new Queue<SimpleElevatorStopRequest>();
            this.RequestQueue.Enqueue(request);
        }

        #endregion Private Methods
    }


    public static class SimpleElevatorRequestQueueExtensions
    {
        /// <summary>
        /// Queues items from another queue. NOTE: dequeues all items in the fromQueue
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fromQueue">Enqueue items from this queue</param>
        public static void EnqueueFrom(
            this Queue<SimpleElevatorStopRequest> self,
            Queue<SimpleElevatorStopRequest> fromQueue) 
        {
            if (fromQueue.Count > 0)
            {
                do
                {
                    self.Enqueue(fromQueue.Dequeue());
                } while (fromQueue.Count != 0);
            }
        }
    }
}
