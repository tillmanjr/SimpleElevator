using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdjrSimpleElevatorCommon
{
    /// <summary>
    /// Operations an elevator may be performing
    /// </summary>
    public enum ElevatorOperation
    {
        Unkown
      , Traveling
      , DoorsOpen
      , Parked
    }

    /// <summary>
    /// Commands an elevator accepts
    /// </summary>
    public enum ElevatorCommand
    {
          Ascend
        , Descend
        , OpenDoors
        , Park
    }

    /// <summary>
    /// Directions of travel on an elevator
    /// </summary>
    public enum DirectionOfTravel
    {
          Unspecified
        , Ascend
        , Descend
    }

    /// <summary>
    /// Nature of a stop request
    /// </summary>
    public enum ElevatorStopRequestType
    {
          Unknown
        , Dropoff
        , Pickup
    }

    /// <summary>
    /// Status of a stop request as it progresses.
    /// NOTE: incomplete and to be obsoleted
    /// </summary>
    public enum ElevatorStopRequestStatus
    {
          Intitiated
        , Received
        , Evaluating
        , Queued
        , InProcess
        , Completed
    }

}
