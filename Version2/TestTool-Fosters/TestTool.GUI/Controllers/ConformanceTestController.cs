///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using TestTool.GUI.Views;
using TestTool.GUI.Data;
using TestTool.GUI.Utils;
using TestTool.Tests.Definitions.Data;
using System.Text;
using TestTool.Tests.Engine;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Controller for setup tab
    /// </summary>
    class ConformanceTestController : Controller<IConformanceTestView>
    {
        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="view">Setup view control</param>
        public ConformanceTestController(IConformanceTestView view)
            :base(view)
        {
        }

        /// <summary>
        /// Is raised when device information is received.
        /// </summary>
        public event ManagementServiceProvider.DeviceInformationReceived DeviceInformationReceived;
        
        /*
        // To be removed
        #region GetFromDevice

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
        /// Stops long operation
        /// </summary>
        public override void Stop()
        {
            if (_requestPending)
            {
                _client.Stop();
            }
        }

        #endregion
        */

        #region TEST

        public event Action TestsRunRequested;
        public event Action HaltRequested;
        public event Action ExitRequested;

        private bool _testsRunning;

        /// <summary>
        /// Runs tests as in conformance mode
        /// </summary>
        public void RunAll()
        {
            _completedTests = 0;
            _testsDefined = false;
            _testsCount = 0;
            _failedTests = 0;
            _empty = false;
            View.ClearLog();
            View.EnableSaveReport(false);
            if (TestsRunRequested != null)
            {
                _testsRunning = true;
                TestsRunRequested();
            }
        }

        /// <summary>
        /// Stops tests execution immediately.
        /// </summary>
        public void Halt()
        {
            if (HaltRequested != null)
            {
                HaltRequested();
            }
        }

        /// <summary>
        /// Stops tests execution without raising additional events.
        /// </summary>
        public void Exit()
        {
            if (ExitRequested != null)
            {
                ExitRequested();
            }
        }

        private int _testsCount;
        private bool _testsDefined = false;
        private int _completedTests;
        private int _failedTests;

        private ConformanceInitializationData _initializationData;
        
        /// <summary>
        /// Show initialization information (tests count, preliminary testing result)
        /// </summary>
        /// <param name="data"></param>
        public void InitializationCompleted(ConformanceInitializationData data)
        {
            if (_testsRunning)
            {
                _testsDefined = true;
                _testsCount = data.TestsSelected.Count;
                View.DefineTestsCount(data.TestsSelected.Count);

                _initializationData = data;

                ReportFeatureDefinitionResult(data);
            }
        }
        
        /// <summary>
        /// Handles "TestStarted" event.
        /// </summary>
        /// <param name="testInfo">Test description.</param>
        public void TestStarted(TestInfo testInfo)
        {
            if (_testsRunning)
            {
                View.BeginTest(testInfo);
            }
            else
            {
                Clear();
            }
        }

        private bool _empty;
        
        /// <summary>
        /// Handles "TestCompleted" event.
        /// </summary>
        /// <param name="testInfo">Test description.</param>
        /// <param name="log">Test results.</param>
        public void TestCompleted(TestInfo testInfo, Tests.Definitions.Trace.TestLog log)
        {
            if (_testsRunning)
            {
                bool isTest = testInfo.ProcessType == ProcessType.Test;
                if (isTest)
                {
                    _completedTests++;
                    if (log.TestStatus == TestTool.Tests.Definitions.Trace.TestStatus.Failed)
                    {
                        _failedTests++;
                    } 
                    View.EndTest(testInfo.Id, testInfo.Name, log.TestStatus);
                    View.ReportProgress(_completedTests, _testsCount, _failedTests);
                }
                else
                {
                    View.EndFeatureDefinition(log.TestStatus);
                }
            }
        }

        /// <summary>
        /// Handles "TestSuiteCompleted" event.
        /// </summary>
        /// <param name="bCompletedNormally">Indicates whether test suite has not been halted.</param>
        public void TestSuiteCompleted(bool bCompletedNormally)
        {
            if (_testsRunning)
            {
                _testsRunning = false;
                if (_testsDefined)
                {
                    _empty = false;
                    View.ReportTestSuiteCompleted(!_preliminaryFailed, 
                        _completedTests - _failedTests,
                        _failedTests, 
                        bCompletedNormally);
                    if (bCompletedNormally)
                    {
                        View.EnableSaveReport(true);
                    }                
                }
                else
                {
                    View.ReportFeatureDefinitionCompleted(bCompletedNormally);
                }
            }
        }
        
        /// <summary>
        /// Indicates that tests are being performed.
        /// </summary>
        public bool Running
        {
            get { return _testsRunning; }

        }

        /// <summary>
        /// Clears all information.
        /// </summary>
        public void Clear()
        {
            if (!_empty && !_testsRunning)
            {
                View.EnableSaveReport(false);
                View.ClearInfo();
                View.ClearLog();
                _empty = true;
            }
        }

        /// <summary>
        /// Displays device information
        /// </summary>
        /// <param name="info"></param>
        public void DisplayDeviceInfo(DeviceInfo info)
        {
            SetupInfo setupInfo = ContextController.GetSetupInfo();

            if (DeviceInformationReceived != null)
            {
                DeviceInformationReceived(info.Manufacturer, info.Model, info.FirmwareVersion, info.SerialNumber,
                                          info.HardwareID);
            }
            setupInfo.DevInfo = info;
        }

        private bool _preliminaryFailed = false;

        private DeviceInformation _conformanceTestingInfo;

        /// <summary>
        /// Makes preliminary estimation.
        /// </summary>
        /// <param name="data"></param>
        public void ReportFeatureDefinitionResult(ConformanceInitializationData data)
        {
            _conformanceTestingInfo = data.DeviceInformation;
            _preliminaryFailed = false;

            StringBuilder report= new StringBuilder();

            report.AppendFormat("Feature definition process {0} ({1} supported, {2} unsupported, {3} undefined)",
                data.UndefinedFeatures.Count > 0 ? "FAILED" : "PASSED",
                data.SupportedFeatures.Count, data.UnsupportedFeatures.Count, data.UndefinedFeatures.Count);

            report.AppendLine(Environment.NewLine + "Profile Support Preliminary Check:");
            foreach (var profile in data.SupportedProfiles)
            {
                report.AppendFormat("   {0} SUPPORTED{1}", profile.Name, Environment.NewLine );
            }
            foreach (var profile in data.UnsupportedProfiles)
            {
                report.AppendFormat("   {0} NOT SUPPORTED{1}", profile.Name, Environment.NewLine);
            }
            foreach (var profile in data.FailedProfiles)
            {
                report.AppendFormat("   {0} FAILED{1}", profile.Name, Environment.NewLine);
            }

            if (data.SupportedProfiles.Count == 0 && data.FailedProfiles.Count == 0)
            {
                _preliminaryFailed = true;
                report.AppendLine("No profiles supported, conformance will be FAILED");
            }
            if (data.FailedProfiles.Count > 0)
            {
                _preliminaryFailed = true;
                report.AppendLine("Not all profiles claimed are supported, conformance will be FAILED");
            }
            if (data.UndefinedFeatures.Count > 0)
            {
                _preliminaryFailed = true;
                report.AppendLine("Not all features were defined, conformance will be FAILED");
            }

            View.Log(report.ToString());
        }

        #endregion

        #region Report

        /// <summary>
        /// Creates test report
        /// </summary>
        /// <param name="fileName"></param>
        public void CreateReport(string fileName)
        {
            CreateReport(fileName, new PdfReportGenerator());
        }

        /// <summary>
        /// Creates declaration of conformance
        /// </summary>
        /// <param name="fileName"></param>
        public void CreateDoCReport(string fileName)
        {
            CreateReport(fileName, new DoCGenerator());
        }

        /// <summary>
        /// Creates DataSheet report
        /// </summary>
        /// <param name="fileName"></param>
        public void CreateDatasheetReport(string fileName)
        {
            CreateReport(fileName, new DatasheetReportGenerator());
        }

        /// <summary>
        /// Creates report
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="reportGenerator"></param>
        private void CreateReport(string fileName, IReportGenerator reportGenerator)
        {
            UpdateContext();

            Data.TestLogFull testLog = GetFullTestData();

            reportGenerator.OnException += reportGenerator_OnException;
            reportGenerator.OnReportSaved += reportGenerator_OnReportSaved;

            reportGenerator.CreateReport(fileName, testLog);

            reportGenerator.OnException -= reportGenerator_OnException;
            reportGenerator.OnReportSaved -= reportGenerator_OnReportSaved;
        }

        /// <summary>
        /// Colllects log data.
        /// </summary>
        /// <returns></returns>
        private Data.TestLogFull GetFullTestData()
        {
            Data.TestLogFull testLog = new TestLogFull();
            Data.TestLog log = ContextController.GetTestLog();
            testLog.TestResults = log.TestResults;
            testLog.Features = log.Features;
            testLog.InitializationData = log.InitializationData;
            testLog.FeaturesDefinitionLog = log.FeaturesDefinitionLog;

            testLog.DeviceInformation = _conformanceTestingInfo;
            
            if (log.TestExecutionTime != DateTime.MinValue)
            {
                testLog.TestExecutionTime = log.TestExecutionTime;
            }
            else
            {
                testLog.TestExecutionTime = DateTime.Now;
            }

            SetupInfo info = ContextController.GetSetupInfo();
            testLog.TesterInfo = info.TesterInfo;
            testLog.Application = ContextController.GetApplicationInfo();
            testLog.DeviceInfo = info.DevInfo;
            testLog.OtherInformation = info.OtherInfo;
            testLog.MemberInfo = info.MemberInfo;

            testLog.DeviceEnvironment = ContextController.GetDeviceEnvironment();
            return testLog;
        }

        /// <summary>
        /// Handles operation competion.
        /// </summary>
        void reportGenerator_OnReportSaved()
        {
            View.ReportDocumentCreationCompleted();
        }

        /// <summary>
        /// Handles exception in view generation.
        /// </summary>
        /// <param name="ex">Exception data.</param>
        void reportGenerator_OnException(Exception ex)
        {
            View.ReportException(ex);
        }
        
        #endregion

        #region Context

        /// <summary>
        /// Updates setup tab control
        /// </summary>
        public override void UpdateView()
        {
            base.UpdateView();
            //if (CurrentState == Enums.ApplicationState.Idle)
            //{
            //    DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            //    bool enable = (devices != null) && !string.IsNullOrEmpty(devices.ServiceAddress);
            //    View.EnableGetFromDevice(enable);
            //}
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
                    View.Brand = info.DevInfo.Manufacturer;
                    View.Model = info.DevInfo.Model;
                    View.OnvifProductName = info.DevInfo.ProductName;
                }
                //CR is lost during serialization
                View.OtherInformation = !string.IsNullOrEmpty(info.OtherInfo) ? info.OtherInfo.Replace("\n", "\r\n") : string.Empty;
                if (info.TesterInfo != null)
                {
                    View.OperatorName = info.TesterInfo.Operator;
                    View.OrganizationName = info.TesterInfo.Organization;
                    View.OrganizationAddress = !string.IsNullOrEmpty(info.TesterInfo.Address) ? info.TesterInfo.Address.Replace("\n", "\r\n") : string.Empty;
                }
                if (info.MemberInfo != null)
                {
                    View.MemberName = info.MemberInfo.Name;
                    View.MemberAddress = info.MemberInfo.Address;
                }
            }
            //bool enableGetFromDevice = (context.DiscoveryContext != null) && (!string.IsNullOrEmpty(context.DiscoveryContext.ServiceAddress));
            //View.EnableGetFromDevice(enableGetFromDevice);

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
            deviceInfo.Manufacturer = View.Brand;
            deviceInfo.Model = View.Model;
            deviceInfo.ProductName = View.OnvifProductName;
            MemberInfo memberInfo = new MemberInfo();
            memberInfo.Address = View.MemberAddress;
            memberInfo.Name = View.MemberName;

            SetupInfo setupInfo = new SetupInfo();
            setupInfo.DevInfo = deviceInfo;
            setupInfo.OtherInfo = View.OtherInformation;
            setupInfo.TesterInfo = testerInfo;
            setupInfo.MemberInfo = memberInfo;

            ContextController.UpdateSetupInfo(setupInfo);
        }

        #endregion

    }
}
