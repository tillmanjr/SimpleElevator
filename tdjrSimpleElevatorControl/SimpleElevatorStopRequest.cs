using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdjrSimpleElevatorCommon;

namespace tdjrSimpleElevatorControl
{
    public class SimpleElevatorStopRequest
    {
        public const Int32 UnspecifiedLiftID = -1;

        #region Public Properties

        /// <summary>
        /// Requested landing (floor) for request. Normally dropoff only
        /// </summary>
        public Int32 RequestedLandingID { get; set; }

        /// <summary>
        /// Direction of travel requested. Normally pickup only
        /// </summary>
        public DirectionOfTravel RequestedDirection { get; set; }

        /// <summary>
        /// If true then request may only be serviced by specified lift (elevator)
        /// </summary>
        public Boolean UseSpecifiedLift { get; set; }

        /// <summary>
        /// Lift to service this request. Normally dropoff only
        /// </summary>
        public Int32 SpecifiedLiftID { get; set; }

        /// <summary>
        /// Time stamp of request
        /// </summary>
        public DateTime RequestTimeUTC { get; private set; }

        /// <summary>
        /// Nature of request
        /// </summary>
        public ElevatorStopRequestType RequestType { get; private set; }

        /// <summary>
        /// Unique ID of request
        /// </summary>
        public Int32 RequestID {get; private set;}

        /// <summary>
        /// Status of request.
        /// WARNING: not fully implemented
        /// </summary>
        public ElevatorStopRequestStatus RequestStatus { get; set; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected SimpleElevatorStopRequest()
        {
            this.RequestTimeUTC = DateTime.UtcNow;
            this.RequestID = SimpleElevatorStopRequest.GetNewID();
            this.RequestType = ElevatorStopRequestType.Unknown;
            this.RequestStatus = ElevatorStopRequestStatus.Intitiated;
        }

        /// <summary>
        /// Public constructor, relies on protected default to set RequestID and RequestTimeUTC
        /// </summary>
        /// <param name="requestedLandingID"></param>
        /// <param name="requestedDirection"></param>
        /// <param name="useSpecifiedLift"></param>
        /// <param name="specifiedLiftID"></param>
        /// <param name="requestType"></param>
        public SimpleElevatorStopRequest(
            Int32 requestedLandingID,
            DirectionOfTravel requestedDirection,
            Boolean useSpecifiedLift,
            Int32 specifiedLiftID,
            ElevatorStopRequestType requestType
            ) : this() 
        {
            this.RequestedLandingID = requestedLandingID;
            this.RequestedDirection = requestedDirection;
            this.UseSpecifiedLift = useSpecifiedLift;
            this.SpecifiedLiftID = specifiedLiftID;
            this.RequestType = requestType;
        }

        #endregion Constructors

        #region Static helper methods

        /// <summary>
        /// Helper method to create a SimpleElevatorStopRequest for a Pickup
        /// </summary>
        /// <param name="requestedLandingID"></param>
        /// <param name="requestedDirection"></param>
        /// <returns></returns>
        public static SimpleElevatorStopRequest CreatePickupRequest(
            Int32 requestedLandingID,
            DirectionOfTravel requestedDirection
            )
        {
            return new SimpleElevatorStopRequest(
                requestedLandingID,
                requestedDirection,
                false,
                UnspecifiedLiftID,
                ElevatorStopRequestType.Pickup
                );
        }

        /// <summary>
        /// Helper method to create a SimpleElevatorStopRequest for a Dropoff
        /// </summary>
        /// <param name="requestedLandingID"></param>
        /// <param name="specifiedLiftID"></param>
        /// <returns></returns>
        public static SimpleElevatorStopRequest CreateDropOffRequest(
            Int32 requestedLandingID,
            Int32 specifiedLiftID
            )
        {
            return new SimpleElevatorStopRequest(
                requestedLandingID,
                DirectionOfTravel.Unspecified,
                true,
                specifiedLiftID,
                ElevatorStopRequestType.Dropoff
                );
        }


        #region Bonehead unique id support
        private static Int32 _CurrentSeedValue = 100;
        private static Int32 GetNewID()
        { 
            return ++_CurrentSeedValue; //  Yeah, more than a bit sketchy
        }

        #endregion Bonehead unique id support

        #endregion Static helper methods
    }
}
