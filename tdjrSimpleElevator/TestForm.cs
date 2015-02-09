using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using tdjrSimpleElevatorControl;
using tdjrSimpleElevatorCommon;

namespace tdjrSimpleElevator
{
    public partial class TestForm : Form
    {
        private SimpleElevatorControl _ElevatorControl;

        public TestForm()
        {
            InitializeComponent();
        }

        SimpleElevatorRequestQueue queue = null;

        private void CheckElevatorControl()
        {
            if (_ElevatorControl == null)
            {
                var elevatorBank = SimpleElevatorBank.CreateSimpleElevatorBank();
                this._ElevatorControl = new SimpleElevatorControl(elevatorBank);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.button1.Enabled = false;

            this.CheckElevatorControl();
            this.TestOutput.Clear();

            UpdateAllOutputs(false);

            this.queue = new SimpleElevatorRequestQueue();

            queue.SubmitPickupRequest(1, DirectionOfTravel.Ascend);
            queue.SubmitDropoffRequest(100, 6);

            this.ExecuteController(false);
            this.ExecuteController(false);

            queue.SubmitDropoffRequest(100, 3);
            queue.SubmitPickupRequest(4, DirectionOfTravel.Ascend); 
            this.ExecuteController(true);

            queue.SubmitPickupRequest(4, DirectionOfTravel.Descend);            
            queue.SubmitDropoffRequest(100, 3);

            this.ExecuteController();

            this.button1.Enabled = true;
        }

        private void ExecuteController(Boolean continueUntilDone = true)
        {
            this._ElevatorControl.Execute(queue.RequestQueue);
            UpdateAllOutputs(false);
            Thread.Sleep(1000);  //hack alert

            if (continueUntilDone == true)
            {
                while (this._ElevatorControl.RequestQueue.Count > 0)
                {
                    this._ElevatorControl.Execute(queue.RequestQueue);
                    UpdateAllOutputs(false);
                    Thread.Sleep(1000);  //hack alert
                }
            }
        }

        private void UpdateAllOutputs(Boolean clearFirst)
        {
            if (clearFirst)
                this.TestOutput.Clear();
            this.TestOutput.AppendText(this._ElevatorControl.GetCurrentElevatorStatus(100).FormattedSingle());
            this.TestOutput.AppendText(Environment.NewLine);
        
        }

    }

}
