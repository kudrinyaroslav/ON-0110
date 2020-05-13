///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.GUI.Utils;
using TestTool.GUI.Views;
using TestTool.GUI.Data;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Controller for the Device -> Management tab.
    /// </summary>
    class DeviceManagementController : Controller<IDeviceManagementView>
    {
        private ManagementServiceProvider _client;

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="view">View to interract with user.</param>
        public DeviceManagementController(IDeviceManagementView view)
            :base(view)
        {
        }

        /// <summary>
        /// Initializes service client, if it's possibly.
        /// </summary>
        /// <returns>True if client has been initialized successfully.</returns>
        bool InitializeClient()
        {
            string serviceAddress = string.Empty;
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            if (devices.Current != null)
            {
                serviceAddress = devices.ServiceAddress;
            }

            try
            {
                new Uri(serviceAddress);
            }
            catch (Exception)
            {
                View.ReportError("Device service address is invalid!");
                return false;
            }

            try
            {
                DeviceEnvironment env = ContextController.GetDeviceEnvironment();

                _client = new ManagementServiceProvider(serviceAddress, env.Timeouts.Message);
                _client.ExceptionThrown += _client_ExceptionThrown;
                _client.OnDeviceInformationReceived += _client_OnDeviceInformationReceived;
                _client.OperationCompleted += _client_OperationCompleted;
                _client.OperationStarted += _client_OperationStarted;
                _client.ResponseReceived += _client_ResponseReceived;

                return true;
            }
            catch (Exception exc)
            {
                View.ReportError(string.Format("Error occurred: {0}", exc.Message));
                return false;
            }

        }

        /// <summary>
        /// Displays response.
        /// </summary>
        /// <param name="response">Response received.</param>
        void _client_ResponseReceived(string response)
        {
            View.DisplayLog(response);
        }

        /// <summary>
        /// Informs other controllers that a long operation is started.
        /// </summary>
        void _client_OperationStarted()
        {
            ReportOperationStarted();
        }

        /// <summary>
        /// Informs other controllers that a long operation is completed.
        /// </summary>
        void _client_OperationCompleted()
        {
            _idle = true;
            ReportOperationCompleted();
        }

        /// <summary>
        /// Displays device information, when it is received.
        /// </summary>
        /// <param name="manufacturer">Manufacturer.</param>
        /// <param name="model">Model.</param>
        /// <param name="firmwareVersion">Firmware version.</param>
        /// <param name="serial">Serial number.</param>
        /// <param name="hardwareId">Hardware ID.</param>
        void _client_OnDeviceInformationReceived(string manufacturer, string model, string firmwareVersion, string serial, string hardwareId)
        {
            View.DisplayDeviceInformation(manufacturer, model, firmwareVersion, serial, hardwareId);
        }

        /// <summary>
        /// Handles exception thrown in client operations.
        /// </summary>
        /// <param name="stage">String description of action being performed.</param>
        /// <param name="exc">Exception data.</param>
        void _client_ExceptionThrown(string stage, Exception exc)
        {
            _idle = true;
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            string message;
            if (!string.IsNullOrEmpty(stage))
            {
                message = string.Format("{0}{1}{2}", stage, Environment.NewLine, exc.Message);
            }
            else
            {
                message = exc.Message;
            }
            View.DisplayLog(message.Replace("\n", Environment.NewLine));
        }

        /// <summary>
        /// Updates view.
        /// </summary>
        public override void UpdateView()
        {
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            if (devices != null)
            {
                View.DeviceManagementUrl = devices.ServiceAddress;
                View.IP = devices.DeviceAddress != null ? devices.DeviceAddress.ToString() : string.Empty;
                if (_client != null && _client.Address != devices.ServiceAddress) 
                { 
                    View.Clear();
                }
            }
            else
            {
                View.DeviceManagementUrl = string.Empty;
                View.IP = string.Empty;
                View.Clear();
            }
            DeviceEnvironment environment = ContextController.GetDeviceEnvironment();
            if (_client != null)
            {
                _client.Timeout = environment.Timeouts.Message;
            }

        }

        private bool _idle = true;

        /// <summary>
        /// Updates view controls availability.
        /// </summary>
        public override void UpdateViewFunctions()
        {
            View.EnableFunctions(DeviceNotEmpty() && _idle && (CurrentState == Enums.ApplicationState.Idle));
        }

        /// <summary>
        /// Checks if device address is not empty.
        /// </summary>
        /// <returns>True if device is selected and address is not empty.</returns>
        public bool DeviceNotEmpty()
        {
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            bool bHasAddress = devices != null && !string.IsNullOrEmpty(devices.ServiceAddress);
            return bHasAddress; 
        }

        /// <summary>
        /// Stops time-consuming operation.
        /// </summary>
        public override void Stop()
        {
            _client.Stop();
        }

        /// <summary>
        /// Checks if time-consuming operation is being performed.
        /// </summary>
        public override bool RequestPending
        {
            get { return !_idle; }
        }

        /// <summary>
        /// Gets device information.
        /// </summary>
        public void GetDeviceInformation()
        {
            if (InitializeClient())
            {
                _idle = false;
                _client.GetDeviceInformation();
            }
        }

        /// <summary>
        /// Gets hostname information.
        /// </summary>
        public void GetHostname()
        {
            if (InitializeClient())
            {
                _idle = false;
                _client.GetHostname();
            }
        }

        /// <summary>
        /// Gets network interfaces.
        /// </summary>
        public  void GetNetworkInterfaces()
        {
            if (InitializeClient())
            {
                _idle = false;
                _client.GetInterfaces();
            }
        }

        /// <summary>
        /// Gets device scopes.
        /// </summary>
        public void GetScopes()
        {
            if (InitializeClient())
            {
                _idle = false;
                _client.GetScopes();
            }
        }

        /// <summary>
        /// Resets the devive.
        /// </summary>
        public void HardReset()
        {
            if (InitializeClient())
            {
                _idle = false;
                _client.HardReset();
            }
        }

        /// <summary>
        /// Reboots the device.
        /// </summary>
        public void Reboot()
        {
            if (InitializeClient())
            {
                _idle = false;
                _client.Reboot();
            }
        }

        /// <summary>
        /// Sets current time.
        /// </summary>
        public void SyncTime()
        {
            if (InitializeClient())
            {
                _idle = false;
                _client.SyncTime();
            }
        }

        /// <summary>
        /// Sets IP address.
        /// </summary>
        /// <param name="token">Interface token.</param>
        /// <param name="ipAddress">IP address.</param>
        /// <param name="prefixLength">Address prefix length.</param>
        public void SetIPAddress(string token, string ipAddress, int prefixLength)
        {
            if (InitializeClient())
            {
                _idle = false;
                _client.SetIPAddress(token, ipAddress, prefixLength);
            }
        }

        /// <summary>
        /// Sets default gateway.
        /// </summary>
        /// <param name="gateway">Gateway.</param>
        public void SetGateway(string gateway)
        {
            if (InitializeClient())
            {
                _idle = false;
                _client.SetGateway(gateway);
            }
        }
    }

}
