///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.ServiceModel;
using TestTool.GUI.Views;
using TestTool.GUI.Data;
//using Proxy = TestTool.Proxies.Device;
using TestTool.GUI.Utils;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Controller for setup tab
    /// </summary>
    class SetupController : Controller<ISetupView>
    {
        /// <summary>
        /// Class for making service calls.
        /// </summary>
        private ManagementServiceProvider _client;

        /// <summary>
        /// True when request is pending; false otherwise.
        /// </summary>
        private bool _requestPending = false;

        /// <summary>
        /// Indicates that request is pendibg.
        /// </summary>
        public override bool RequestPending
        {
            get
            {
                return _requestPending;
            }
        }
        
        /// <summary>
        /// Is raised when device information is received.
        /// </summary>
        public event ManagementServiceProvider.DeviceInformationReceived DeviceInformationReceived;

        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="view">Setup view control</param>
        public SetupController(ISetupView view)
            :base(view)
        {
        }

        /// <summary>
        /// Initializes device management service client
        /// </summary>
        /// <param name="address">Address of device management service</param>
        void InitializeClient(string address)
        {
            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            _client = new ManagementServiceProvider(address, env.Timeouts.Message);
            _client.ExceptionThrown += OnExceptionThrown;
            _client.OnDeviceInformationReceived += DeviceInformationReceived;
            _client.OperationCompleted += OnOperationCompleted;
            _client.OperationStarted += ReportOperationStarted;
            _client.FaultThrown += OnFaultThrown;

            _client.Timeout = env.Timeouts.Message;
        }
        
        /// <summary>
        /// Handles client fault thrown event
        /// </summary>
        /// <param name="stage">Stage</param>
        /// <param name="exc">Thrown fault exception</param>
        void OnFaultThrown(string stage, FaultException exc)
        {
            _requestPending = false;
            View.ShowError(exc);
            ReportOperationCompleted();
            //SwitchToState(Enums.ApplicationState.Idle);
        }

        /// <summary>
        /// Handles client exception event
        /// </summary>
        /// <param name="stage">Stage</param>
        /// <param name="exc">Exception</param>
        void OnExceptionThrown(string stage, Exception exc)
        {
            _requestPending = false;
            View.ShowError(exc);
            ReportOperationCompleted();
            //SwitchToState(Enums.ApplicationState.Idle);
        }

        /// <summary>
        /// Handles client operation completed event
        /// </summary>
        void OnOperationCompleted()
        {
            _requestPending = false;
            ReportOperationCompleted();
        }

        /// <summary>
        /// Start GetDeviceInformation operation
        /// </summary>
        public void GetDeviceInformation()
        {
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            string url = devices.ServiceAddress;
            _requestPending = true;
            ReportOperationStarted();
            InitializeClient(url);
            _client.GetDeviceInformation();
        }

        /// <summary>
        /// Updates setup tab control
        /// </summary>
        public override void UpdateView()
        {
            base.UpdateView();
            if (CurrentState == Enums.ApplicationState.Idle)
            {
                DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
                bool enable = (devices != null) && !string.IsNullOrEmpty(devices.ServiceAddress);
                View.EnableGetFromDevice(enable);
            }
        }

        /// <summary>
        /// Applies saved application context
        /// </summary>
        /// <param name="context">Saved context</param>
        public override void LoadSavedContext(SavedContext context)
        {
            SetupInfo info = context.SetupInfo;
            if (info != null)
            {
                if (info.DevInfo != null)
                {
                    View.FirmwareVersion = info.DevInfo.FirmwareVersion;
                    View.Brand = info.DevInfo.Manufacturer;
                    View.Model = info.DevInfo.Model;
                    View.Serial = info.DevInfo.SerialNumber;
                }
                //CR is lost during serialization
                View.OtherInformation = !string.IsNullOrEmpty(info.OtherInfo) ? info.OtherInfo.Replace("\n", "\r\n") : string.Empty;
                if (info.TesterInfo != null)
                {
                    View.OperatorName = info.TesterInfo.Operator;
                    View.OrganizationName = info.TesterInfo.Organization;
                    View.OrganizationAddress = !string.IsNullOrEmpty(info.TesterInfo.Address) ? info.TesterInfo.Address.Replace("\n", "\r\n") : string.Empty;
                }
            }
            bool enableGetFromDevice = (context.DiscoveryContext != null) && (!string.IsNullOrEmpty(context.DiscoveryContext.ServiceAddress));
            View.EnableGetFromDevice(enableGetFromDevice);

        }

        /// <summary>
        /// Updates application context
        /// </summary>
        public override void UpdateContext()
        {
            base.UpdateContext();
            TesterInfo testerInfo = new TesterInfo();
            testerInfo.Operator = View.OperatorName;
            testerInfo.Organization = View.OrganizationName;
            testerInfo.Address = View.OrganizationAddress;

            DeviceInfo deviceInfo = new DeviceInfo();
            deviceInfo.FirmwareVersion = View.FirmwareVersion;
            deviceInfo.Manufacturer = View.Brand;
            deviceInfo.Model = View.Model;
            deviceInfo.SerialNumber = View.Serial;

            SetupInfo setupInfo = new SetupInfo();
            setupInfo.DevInfo = deviceInfo;
            setupInfo.OtherInfo = View.OtherInformation;
            setupInfo.TesterInfo = testerInfo;

            ContextController.UpdateSetupInfo(setupInfo);
        }

        /// <summary>
        /// Returns application info
        /// </summary>
        /// <returns>Application info</returns>
        public ApplicationInfo GetApplicationInfo()
        {
            return ContextController.GetApplicationInfo();
        }

        /// <summary>
        /// Initializes component
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            View.ShowToolInfo();
        }

        /// <summary>
        /// Stops long operation
        /// </summary>
        public override void Stop()
        {
            if (_requestPending)
            {
                _client.Stop();
            }
        }
    }
}
