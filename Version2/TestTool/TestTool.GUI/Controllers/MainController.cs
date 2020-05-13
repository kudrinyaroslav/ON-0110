///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using TestTool.GUI.Views;
using TestTool.GUI.Data;
using TestTool.Tests.Common.Transport;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Holds GUI logic for the main form. Manages other controllers where necessary.
    /// Handles controllers' events.
    /// </summary>
    class MainController : Controller<IMainView>
    {
        private const string _dataFileName = "Context.xml";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        public MainController(IMainView view)
            :base(view)
        {
            ContextController.InitGeneralContext();
        }

        #region Nested controllers

        // Typed controllers

        private ConformanceTestController _conformanceTestController;
        private DiscoveryController _discoveryController;
        private ManagementController _managementController;
        private TestController _testController;
        private DeviceController _deviceController;

        /// <summary>
        /// Untyped list of controllers for performing common operations
        /// </summary>
        private List<IController> _controllers;

        #endregion

        /// <summary>
        /// Saves references to child controllers. Adds event handlers.
        /// </summary>
        /// <param name="setupController"></param>
        /// <param name="discoveryController"></param>
        /// <param name="managementController"></param>
        /// <param name="testController"></param>
        /// <param name="deviceController"></param>
        public void SetChildControllers(ConformanceTestController setupController,
                                        DiscoveryController discoveryController,
                                        ManagementController managementController,
                                        TestController testController,
                                        DeviceController deviceController)
        {
            _conformanceTestController = setupController;
            _discoveryController = discoveryController;
            _managementController = managementController;
            _testController = testController;
            _deviceController = deviceController;

            _controllers =new List<IController>();

            _controllers.AddRange(new IController[] { setupController, discoveryController, managementController, testController,  deviceController });

            _testController.TestSuiteStarted += _testController_TestSuiteStarted;
            _testController.TestSuiteCompleted += _testController_TestSuiteCompleted;
            _testController.TestsCleared += _testController_TestsCleared;
            _testController.SettingPagesLoaded += _testController_SettingPagesLoaded;

            _managementController.ProfileApplied += _managementController_ProfileApplied;
            _managementController.OperationStarted += _managementController_OnOperationStarted;
            _managementController.OperationCompleted += _managementController_OnOperationCompleted;

            _managementController.SettingsLoaded += new System.EventHandler(_managementController_SettingsLoaded);

            _discoveryController.DiscoveryStarted += _discoveryController_DiscoveryStarted;
            _discoveryController.DiscoveryCompleted += _discoveryController_DiscoveryCompleted;

            _conformanceTestController.OperationStarted += _deviceController_OperationStarted;
            _conformanceTestController.OperationCompleted += _deviceController_OperationCompleted;

            _discoveryController.OperationStarted += _deviceController_OperationStarted;
            _discoveryController.OperationCompleted += _deviceController_OperationCompleted;

            _deviceController.OperationStarted += _deviceController_OperationStarted;
            _deviceController.OperationCompleted += _deviceController_OperationCompleted;

            _conformanceTestController.TestsRunRequested += _testController.RunConformance;
            _conformanceTestController.HaltRequested += _testController.Halt;
            _conformanceTestController.ExitRequested += _testController.Exit;
            _conformanceTestController.SettingsMissing += _conformanceTestController_SettingsMissing;

            _testController.TestStarted += _conformanceTestController.TestStarted;
            _testController.TestCompleted += _conformanceTestController.TestCompleted;
            _testController.TestSuiteCompleted += _conformanceTestController.TestSuiteCompleted;
            _testController.ConformanceInitializationCompleted += _conformanceTestController.InitializationCompleted;
            _testController.DeviceInfoReceived += _testController_DeviceInfoReceived;

            _testController.NetworkSettingsChangedEvent += _discoveryController_NetworkSettingsChanged;
            _testController.SecurityChangedEvent += DiscoveryControllerSecurityChangedEvent;
        }

        void _managementController_SettingsLoaded(object sender, System.EventArgs e)
        {
            _discoveryController.UpdateView();
            _conformanceTestController.UpdateView();
        }

        /// <summary>
        /// Shows message about missing settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _conformanceTestController_SettingsMissing(object sender, SettingsMissingEventArgs e)
        {
           View.ActivateManagementPage();
            _managementController.ShowSettings(e.Settings);
        }

        /// <summary>
        /// Notifies Management controller about known advanced settings
        /// </summary>
        /// <param name="pages"></param>
        void _testController_SettingPagesLoaded(List<Tests.Definitions.UI.SettingsTabPage> pages)
        {
            _managementController.AddSettingsPages(pages);
        }

        /// <summary>
        /// Notifies Conformance controller about device info
        /// </summary>
        /// <param name="info"></param>
        void _testController_DeviceInfoReceived(DeviceInfo info)
        {
            _conformanceTestController.DisplayDeviceInfo(info);
        }
        
        /// <summary>
        /// Clears report.
        /// </summary>
        void _testController_TestsCleared()
        {
            _conformanceTestController.Clear();
        }

        /// <summary>
        /// Applies profile where necessary.
        /// </summary>
        /// <param name="profile"></param>
        void _managementController_ProfileApplied(Profile profile)
        {
            _testController.ApplyProfile(profile);
        }

        /// <summary>
        /// Updates Discovery tab when received 'hello' message with new service address 
        /// during network settings changing
        /// </summary>
        void _discoveryController_NetworkSettingsChanged(string newServiceAddress)
        {
            _discoveryController.UpdateNetworkSettings(newServiceAddress);
        }

        private void DiscoveryControllerSecurityChangedEvent(CredentialsProvider credentialsProvider)
        {
            _discoveryController.UpdateCredentials(credentialsProvider.Username, credentialsProvider.Password);
        }

        /// <summary>
        /// Disables GUI when discovery is started.
        /// </summary>
        void _discoveryController_DiscoveryStarted()
        {
            foreach (IController controller in _controllers)
            {
                controller.SwitchToState(Enums.ApplicationState.DiscoveryRunning);
            }
            SwitchToState(Enums.ApplicationState.DiscoveryRunning);
        }

        /// <summary>
        /// Switches application to the "Idle" state
        /// </summary>
        void SwitchToIdle()
        {
            foreach (IController controller in _controllers)
            {
                controller.SwitchToState(Enums.ApplicationState.Idle);
            }
            SwitchToState(Enums.ApplicationState.Idle);
        }

        /// <summary>
        /// Unlocks application when discovery is finished.
        /// </summary>
        void _discoveryController_DiscoveryCompleted()
        {
            SwitchToIdle();
        }



        /// <summary>
        /// Unlocks application when tests are completed.
        /// </summary>
        void _testController_TestSuiteCompleted(bool bCompletedNormally)
        {
            SwitchToIdle();
        }

        /// <summary>
        /// Locks GUI when test execution is started.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="conformance"></param>
        void _testController_TestSuiteStarted(Tests.Engine.TestSuiteParameters obj, bool conformance)
        {
            foreach (IController controller in _controllers)
            {
                controller.SwitchToState(conformance ? Enums.ApplicationState.ConformanceTestRunning : Enums.ApplicationState.TestRunning);
            }
            SwitchToState(conformance ? Enums.ApplicationState.ConformanceTestRunning : Enums.ApplicationState.TestRunning);
        }
        
        /// <summary>
        /// Locks GUI when time-consuming operation is started.
        /// </summary>
        void _managementController_OnOperationStarted()
        {
            foreach (IController controller in _controllers)
            {
                controller.SwitchToState(Enums.ApplicationState.CommandRunning);
            }
            SwitchToState(Enums.ApplicationState.CommandRunning);
        }

        /// <summary>
        /// Unlock GUI when time-consuming operation is completed.
        /// </summary>
        void _managementController_OnOperationCompleted()
        {
            SwitchToIdle();
        }

        /// <summary>
        /// Locks GUI when time-consimunig operation is started.
        /// </summary>
        void _deviceController_OperationStarted()
        {
            foreach (IController controller in _controllers)
            {
                controller.SwitchToState(Enums.ApplicationState.CommandRunning);
            }
            SwitchToState(Enums.ApplicationState.CommandRunning);
        }

        /// <summary>
        /// Unlock GUI when time-consuming operation is completed.
        /// </summary>
        void _deviceController_OperationCompleted()
        {
            SwitchToIdle();
        }
        
        /// <summary>
        /// Checks if tests are being performed.
        /// </summary>
        /// <returns>True if tests are being performed.</returns>
        public bool TestIsRunning()
        {
            return _testController.Running || _conformanceTestController.Running;
        }

        /// <summary>
        /// Stops tests.
        /// </summary>
        public void StopTest()
        {
            if (_testController.Running)
            {
                _testController.Exit();
            }
            if (_conformanceTestController.Running)
            {
                _conformanceTestController.Exit();
            }
        }

        /// <summary>
        /// Checks if a time-consuming operation is being performed.
        /// </summary>
        /// <returns>True if a time-consuming operation is being performed.</returns>
        public bool RequestInProgress()
        {
            return _deviceController.RequestPending 
                || _conformanceTestController.RequestPending 
                || _discoveryController.RequestPending ;
        }

        /// <summary>
        /// Stops time-consuming operation.
        /// </summary>
        public void StopRequest()
        {
            foreach (IController controller in new IController[] {_deviceController, _conformanceTestController, _discoveryController})
            {
                if (controller.RequestPending)
                {
                    controller.Stop();
                }
            }
        }


        ///////////////////////////////////////////////////////////////////////////
        //!  @author        Ivan Vagunin
        ////

        /// <summary>
        /// Saves context data.
        /// </summary>
        public void SaveContextData()
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.Machine | IsolatedStorageScope.Application,
                                                                        new System.Security.Policy.Url("www.onvif.org/OnvifTestTool"));
            IsolatedStorageFileStream isoFile = new IsolatedStorageFileStream(_dataFileName, FileMode.Create, isoStore);
            // Create a StreamWriter using the isolated storage file
            StreamWriter writer = new StreamWriter(isoFile);
            XmlSerializer serializer = new XmlSerializer(typeof(SavedContext));
            
            SavedContext context = new SavedContext();
            _conformanceTestController.UpdateContext();
            context.SetupInfo = ContextController.GetSetupInfo();
            _discoveryController.UpdateContext();
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            if(devices != null)
            {
                context.DiscoveryContext = new SavedDiscoveryContext();
                context.DiscoveryContext.ServiceAddress = devices.ServiceAddress;
                context.DiscoveryContext.DeviceAddress = (devices.DeviceAddress != null) ? devices.DeviceAddress.ToString() : string.Empty;
                context.DiscoveryContext.SearchScopes = devices.SearchScopes.Replace(System.Environment.NewLine, " ");
                context.DiscoveryContext.ShowSearchOptions = devices.ShowSearchOptions;
                
                if ((devices.NIC != null) && (devices.NIC.IP != null))
                {
                    context.DiscoveryContext.InterfaceAddress = devices.NIC.IP.ToString();
                }
            }
            _managementController.UpdateContext();
            context.DeviceEnvironment = ContextController.GetDeviceEnvironment();
            
            _deviceController.UpdateContext();

            context.RequestsInfo = ContextController.GetRequestsInfo();
            context.MediaInfo = ContextController.GetMediaInfo();
            context.PTZInfo = ContextController.GetPTZInfo();
            context.DebugInfo = ContextController.GetDebugInfo();

            serializer.Serialize(writer, context);
            writer.Close();
        }
        
        /// <summary>
        /// Loads data saved before application closing.
        /// </summary>
        public void LoadContextData()
        {
            Stream stream = null;
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.Machine | IsolatedStorageScope.Application,
                                                                        new System.Security.Policy.Url("www.onvif.org/OnvifTestTool"));
            stream = new IsolatedStorageFileStream(_dataFileName, FileMode.Open, isoStore);
            SavedContext savedContext = null;
            // Create a StreamWriter using the isolated storage file
            StreamReader reader = new StreamReader(stream);
            XmlSerializer serializer = new XmlSerializer(typeof(SavedContext));
            savedContext = serializer.Deserialize(reader) as SavedContext;
            reader.Close();
            if (savedContext != null)
            {
                foreach (IController controller in _controllers)
                {
                    controller.LoadSavedContext(savedContext);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnPostLoadContextData()
        {
            _managementController.OnPostLoadContextData();
        }


        ///////////////////////////////////////////////////////////////////////////
        //!  @author        Anna Tarasova
        ////
        private IController _activeController;
        
        /// <summary>
        /// Activates controller when a view is activated.
        /// </summary>
        /// <param name="controller">New active controller.</param>
        public void ActivateController(IController controller)
        {
            if (_activeController != null)
            {
                _activeController.UpdateContext();
            }
            controller.UpdateView();
            controller.UpdateViewFunctions();
            _activeController = controller;
        }

        /// <summary>
        /// Starts background initialization.
        /// </summary>
        public override void Initialize()
        {
            foreach (IController controller in _controllers)
            {
                controller.Initialize();
            }
        }

        public void LoadContext()
        {
            try
            {
                LoadContextData();
            }
            catch (System.Exception exc)
            {
            }
            OnPostLoadContextData();
        }

    }
}
