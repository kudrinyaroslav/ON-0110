using System;
using TestTool.Device;
using TestTool.GUI.Data;

namespace TestTool.GUI.Controllers
{
    class TestController : Controller<Views.ITestView>
    {

        private Simulator _simulator;

        public TestController(Views.ITestView view)
            :base(view)
        {
            _simulator = new Simulator();

            _simulator.DataTransmitted += new NetworkEvent(_simulator_DataTransmitted);
            _simulator.Started += new SimulatorStartedEvent(_simulator_Started);
            _simulator.StartFailed += new SimulatorEvent(_simulator_StartFailed);
        }

        public void Start()
        {
            try
            {
                SimulatorStartParameters parameters = new SimulatorStartParameters();
                parameters.IPAddress = Context.Instance.Environment.BaseAddress;
                parameters.DeviceAddress = Context.Instance.Environment.DeviceAddress;
                parameters.Username = Context.Instance.Credentials.Username;
                parameters.Password = Context.Instance.Credentials.Password;

                _simulator.Start(parameters);
            }
            catch (Exception exc)
            {
                View.ReportError(string.Format("Unable to start services: {0}", exc.Message));
            }

        }


        void _simulator_Started()
        {
            View.SwitchToWorkingMode();
            if (Started != null)
            {
                Started();
            }            
        }
        
        void _simulator_StartFailed(string message)
        {
            View.ReportError(string.Format("Unable to start services: {0}", message));
        }        
        
        public void Stop()
        {
            try
            {
                _simulator.Stop();

                View.SwitchToIdleMode();
                if (Stopped != null)
                {
                    Stopped();
                }
            }
            catch (Exception exc)
            {
                View.ReportError(string.Format("Unable to stop services: {0}", exc.Message));
            }
        }

        public event SimulatorStartedEvent Started;
        public event SimulatorStoppedEvent Stopped;

        void _simulator_DataTransmitted(NetworkEventData parameters)
        {
            View.DisplayNetworkEvent(parameters);
        }
        

    }
}
