using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdjrSimpleElevatorControl
{
    /// <summary>
    /// a collection of Elevators (Lifts) serving the same floors.
    /// For now there's only one
    /// </summary>
    public class SimpleElevatorBank
    {
        #region Public Properties

        /// <summary>
        /// a collection of Elevators
        /// </summary>
        public List<SimpleElevator> Elevators { get; private set; }

        /// <summary>
        /// Helper method to get an elevator by id
        /// </summary>
        /// <param name="elevatorID"></param>
        /// <returns></returns>
        public SimpleElevator GetElevator(Int32 elevatorID)
        {
            return this.Elevators
                   .Where(x => x.ElevatorID == elevatorID)
                   .FirstOrDefault();       
        }

        #endregion Public Properties

        #region Constructors

        public SimpleElevatorBank(List<SimpleElevator> elevators)
        {
            this.Elevators = elevators;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Which landings (floors) currently have elevators
        /// </summary>
        /// <returns></returns>
        public List<Int32> GetCurrentLandings()
        {
            return this.Elevators
                .Select(x => x.CurrentLanding)
                .ToList();
        }

        #endregion Public Methods

        #region Static Helper Methods

        public static SimpleElevatorBank CreateSimpleElevatorBank() {
            List<SimpleElevator> elevators = new List<SimpleElevator>();
            elevators.Add(
                SimpleElevator.CreateSimpleElevator()
                );

            return new SimpleElevatorBank(elevators);
        }

        #endregion Static Helper Methods

    }
}
