///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using TestTool.GUI.Views;
using TestTool.GUI.Data;

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

        private SetupController _setupController;
        private DiscoveryController _discoveryController;
        private ManagementController _managementController;
        private TestController _testController;
        private ReportController _reportController;
        private DeviceController _deviceController;
        private RequestsController _requestsController;

        private List<IController> _controllers;

        #endregion

        /// <summary>
        /// Saves references to child controllers. Adds event handlers.
        /// </summary>
        /// <param name="setupController"></param>
        /// <param name="discoveryController"></param>
        /// <param name="managementController"></param>
        /// <param name="testController"></param>
        /// <param name="reportController"></param>
        /// <param name="deviceController"></param>
        /// <param name="requestsController"></param>
        public void SetChildControllers(SetupController setupController,
                                        DiscoveryController discoveryController,
                                        ManagementController managementController,
                                        TestController testController,
                                        ReportController reportController,
                                        DeviceController deviceController,
                                        RequestsController requestsController)
        {
            _setupController = setupController;
            _discoveryController = discoveryController;
            _managementController = managementController;
            _testController = testController;
            _reportController = reportController;
            _deviceController = deviceController;
            _requestsController = requestsController;

            _controllers =new List<IController>();

            _controllers.AddRange(new IController[] { setupController, discoveryController, managementController, testController, reportController, deviceController, requestsController });

            _testController.TestSuiteStarted += _testController_TestSuiteStarted;
            _testController.TestSuiteCompleted += _testController_TestSuiteCompleted;
            _testController.TestEvent += _testController_TestEvent;
            _testController.TestsCleared += _testController_TestsCleared;

            _managementController.ProfileApplied += _managementController_ProfileApplied;
            _managementController.FeaturesApplied += _managementController_FeaturesApplied;
            _managementController.OnCertificationMode += _managementController_OnCertificationMode;
            _managementController.OperationStarted += _managementController_OnOperationStarted;
            _managementController.OperationCompleted += _managementController_OnOperationCompleted;

            _discoveryController.DiscoveryStarted += _discoveryController_DiscoveryStarted;
            _discoveryController.DiscoveryCompleted += _discoveryController_DiscoveryCompleted;

            _setupController.OperationStarted += _deviceController_OperationStarted;
            _setupController.OperationCompleted += _deviceController_OperationCompleted;

            _discoveryController.OperationStarted += _deviceController_OperationStarted;
            _discoveryController.OperationCompleted += _deviceController_OperationCompleted;

            _deviceController.OperationStarted += _deviceController_OperationStarted;
            _deviceController.OperationCompleted += _deviceController_OperationCompleted;

            _requestsController.OperationStarted += _deviceController_OperationStarted;
            _requestsController.OperationCompleted += _deviceController_OperationCompleted;

        }
        
        /// <summary>
        /// Clears report.
        /// </summary>
        void _testController_TestsCleared()
        {
            _reportController.Clear();
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
        /// Applies features selection.
        /// </summary>
        /// <param name="features"></param>
        void _managementController_FeaturesApplied(List<Tests.Common.Enums.Feature> features)
        {
            _testController.SelectFeatureDependentTests(features);
        }

        /// <summary>
        /// Sets certification mode where necessary.
        /// </summary>
        /// <param name="bOn"></param>
        void _managementController_OnCertificationMode(bool bOn)
        {
            _testController.SetCertificationMode(bOn);
            _reportController.SetCertificationMode(bOn);
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
        /// Passes test event to report controller.
        /// </summary>
        /// <param name="entry"></param>
        void _testController_TestEvent(string entry)
        {
            _reportController.LogEvent(entry);
        }

        /// <summary>
        /// Unlocks application when tests are completed.
        /// </summary>
        void _testController_TestSuiteCompleted()
        {
            SwitchToIdle();
        }

        /// <summary>
        /// Locks GUI when test execution is started.
        /// </summary>
        /// <param name="obj"></param>
        void _testController_TestSuiteStarted(Tests.Common.TestEngine.TestSuiteParameters obj)
        {
            foreach (IController controller in _controllers)
            {
                controller.SwitchToState(Enums.ApplicationState.TestRunning);
            }
            SwitchToState(Enums.ApplicationState.TestRunning);
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
            return _testController.Running;
        }

        /// <summary>
        /// Stops tests.
        /// </summary>
        public void StopTest()
        {
            _testController.Exit();
        }

        /// <summary>
        /// Checks if a time-consuming operation is being performed.
        /// </summary>
        /// <returns>True if a time-consuming operation is being performed.</returns>
        public bool RequestInProgress()
        {
            return _deviceController.RequestPending 
                || _setupController.RequestPending 
                || _discoveryController.RequestPending 
                || _requestsController.RequestPending;
        }

        /// <summary>
        /// Stops time-consuming operation.
        /// </summary>
        public void StopRequest()
        {
            foreach (IController controller in new IController[] {_deviceController, _setupController, _discoveryController, _requestsController})
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
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetMachineStoreForAssembly();
            IsolatedStorageFileStream isoFile = new IsolatedStorageFileStream(_dataFileName, FileMode.Create, isoStore);
            // Create a StreamWriter using the isolated storage file
            StreamWriter writer = new StreamWriter(isoFile);
            XmlSerializer serializer = new XmlSerializer(typeof(SavedContext));
            
            SavedContext context = new SavedContext();
            _setupController.UpdateContext();
            context.SetupInfo = ContextController.GetSetupInfo();
            _discoveryController.UpdateContext();
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            if(devices != null)
            {
                context.DiscoveryContext = new SavedDiscoveryContext();
                context.DiscoveryContext.ServiceAddress = devices.ServiceAddress;
                context.DiscoveryContext.DeviceAddress = (devices.DeviceAddress != null) ? devices.DeviceAddress.ToString() : string.Empty;
                if ((devices.NIC != null) && (devices.NIC.IP != null))
                {
                    context.DiscoveryContext.InterfaceAddress = devices.NIC.IP.ToString();
                }
            }
            _reportController.UpdateContext();
            context.ReportInfo = ContextController.GetReportInfo();
            _managementController.UpdateContext();
            context.DeviceEnvironment = ContextController.GetDeviceEnvironment();
            _requestsController.UpdateContext();
            context.RequestsInfo = ContextController.GetRequestsInfo();
            _deviceController.MediaController.UpdateContext();
            context.MediaInfo = ContextController.GetMediaInfo();
            _deviceController.PTZController.UpdateContext();
            context.PTZInfo = ContextController.GetPTZInfo();

            serializer.Serialize(writer, context);
            writer.Close();
        }
        
        /// <summary>
        /// Loads data saved before application closing.
        /// </summary>
        public void LoadContextData()
        {
            Stream stream = null;
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetMachineStoreForAssembly();
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
    }
}
