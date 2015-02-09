using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdjrSimpleElevatorCommon;

namespace tdjrSimpleElevatorControl
{
    public interface ISimpleElevatorRequestQueue
    {
        /// <summary>
        /// Request to pickup passenger(s) 
        /// </summary>
        /// <param name="landingNumber">Where to pickup passengers</param>
        /// <param name="requestDirection">Passengers requsted direction of travel</param>
        void SubmitPickupRequest(Int32 landingID, DirectionOfTravel requestedDirection);

        /// <summary>
        /// Request to drop of a current passenger
        /// </summary>
        /// <param name="elevatorNumber">Elevator carrying passenger</param>
        /// <param name="landingNumber">Where to drop off passenger</param>
        void SubmitDropoffRequest(Int32 elevatorID, Int32 landingID);
    }

    
}
