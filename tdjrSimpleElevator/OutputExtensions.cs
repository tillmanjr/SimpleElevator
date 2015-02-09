using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdjrSimpleElevatorControl;
using tdjrSimpleElevatorCommon;

namespace tdjrSimpleElevator
{
    /// <summary>
    /// Collection of extension methods to provide formatted text representations
    /// </summary>
    public static class OutputExtensions
    {
        public static String Formatted(this ElevatorStatus self)
        {
            StringBuilder result = new StringBuilder();
            
            result.AppendLine("Lift Status");
            result.AppendLine(String.Format("     Lift ID : {0}", self.ElevatorID));
            result.AppendLine(String.Format("     Current Landing : {0}", self.LandingID));
            result.AppendLine(String.Format("     Current Direction : {0}", self.CurrentDirection.ToString()));
            result.AppendLine(String.Format("     Current Operation : {0}", self.CurrentOperation.ToString()) );

            return result.ToString(); ;
        }

        public static String FormattedSingle(this ElevatorStatus self)
        {
            return String.Format("{0}\t{1}\t{2}\t{3}", self.ElevatorID, self.LandingID, self.CurrentDirection.ToString(), self.CurrentOperation.ToString() );
        }

        public static String Formatted(this SimpleElevatorStopRequest self)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("Stop Request");
            result.AppendLine(String.Format("     ID : {0}", self.RequestID));
            result.AppendLine(String.Format("     Landing : {0}", self.RequestedLandingID));
            result.AppendLine(String.Format("     Direction : {0}", self.RequestedDirection.ToString()));
            result.AppendLine(String.Format("     Lift ID : {0}", self.SpecifiedLiftID));
            result.AppendLine(String.Format("     Type : {0}", self.RequestType.ToString()));
            result.AppendLine(String.Format("     Status : {0}", self.RequestStatus.ToString()));

            return result.ToString(); ;
        }

    }
}
