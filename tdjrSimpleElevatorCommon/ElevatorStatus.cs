using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdjrSimpleElevatorCommon
{
    public class ElevatorStatus
    {
        /// <summary>
        /// Current or most recent landing
        /// </summary>
        public Int32 ElevatorID { get; private set; }

        /// <summary>
        /// Current operation
        /// </summary>
        public ElevatorOperation CurrentOperation { get; private set; }

        /// <summary>
        /// Currect direction of travel 
        /// </summary>
        public DirectionOfTravel CurrentDirection { get; private set; }

        /// <summary>
        /// Current or most recent landing
        /// </summary>
        public Int32 LandingID { get; private set; }

        public ElevatorStatus(
            Int32 elevatorID,
            ElevatorOperation currentOperation,
            DirectionOfTravel currentDirection,
            Int32 landingID
            )
        {
            this.ElevatorID = elevatorID;
            this.CurrentOperation = currentOperation;
            this.CurrentDirection = currentDirection;
            this.LandingID = landingID;
        }
    }
}
