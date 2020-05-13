///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Text;
using TestTool.GUI.Views;
using System.IO;
using TestTool.Tests.Definitions.Data;
using TestTool.GUI.Data;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Engine;
using System.Xml.Serialization;
using TestTool.Tests.Common.Trace;
using TestTool.Tests.Engine.Base.TestBase;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// GUI logic for the Test tab.
    /// </summary>
    partial class TestController : Controller<ITestView>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view">View.</param>
        public TestController(ITestView view)
            :base(view)
        {

            _profilesSupportInfo = new Dictionary<IProfileDefinition, ProfileTestInfo>();
            _testResults = new Dictionary<TestInfo, TestResult>();

            _td = new TestDispatcher();

            _td.NetworkSettingsChangedEvent += _td_NetworkSettingsChanged;
            _td.SecurityChangedEvent += TdOnSecurityChangedEvent;

            _td.TestStarted += _td_TestStarted;
            _td.TestCompleted += _td_TestCompleted;
           
            _td.StepStarted += _td_StepStarted;
            _td.StepCompleted += _td_StepCompleted;
            
            _td.RequestSent += _td_RequestSent;
            _td.ResponseReceived += _td_ResponseReceived;
            
            _td.OnException += _td_OnException;
            _td.StepEvent += _td_StepEvent;
            _td.TestEvent += _td_TestEvent;
            _td.Paused += _td_Paused;
            _td.FeatureDefined += _td_FeatureDefined;
            _td.FeatureDefinitionFailed += _td_FeatureDefinitionFailed;
            _td.InitializationCompleted += _td_InitializationCompleted;
            _td.DeviceInformationReceived += _td_DeviceInformationReceived;
            _td.ProfileDefinitionCompleted += _td_ProfileDefinitionCompleted;
            _td.TestSuiteCompleted += _td_TestSuiteCompleted;
        }

        #region Events
        
        /// <summary>
        /// Is raised when conformance initialization is completed (features are defined, tests are selected)
        /// </summary>
        public event Action<ConformanceInitializationData> ConformanceInitializationCompleted;

        /// <summary>
        /// Is raised when user clicks "Clear" button.
        /// </summary>
        public event Action TestsCleared;
        
        /// <summary>
        /// Raised when device information is received for conformance testing
        /// </summary>
        public event Action<DeviceInfo> DeviceInfoReceived;
        
        /// <summary>
        /// Is raised when a test suite is started.
        /// </summary>
        public event Action<TestSuiteParameters, bool> TestSuiteStarted;

        /// <summary>
        /// Indicates that all tests are done or testing is stopped.
        /// </summary>
        public event Action<bool> TestSuiteCompleted;

        /// <summary>
        /// Is raised when a test is started.
        /// </summary>
        public event Action<TestInfo> TestStarted;

        /// <summary>
        /// Is raised when a test is completed.
        /// </summary>
        public event Action<TestInfo, Tests.Definitions.Trace.TestLog> TestCompleted;

        /// <summary>
        /// Raised when network settings are changed.
        /// </summary>
        public event NetworkSettingsChanged NetworkSettingsChangedEvent;
        public event SecurityChanged     SecurityChangedEvent;
        
        #endregion
        
        #region Properties

        /// <summary>
        /// Indicates that conformance process has been started
        /// </summary>
        private bool _conformance;

        public bool Conformance
        {
            get { return _conformance;  }
        }

        /// <summary>
        /// Indicates that tests are being performed.
        /// </summary>
        protected bool _running;

        /// <summary>
        /// Indicates that tests are being performed.
        /// </summary>
        public bool Running
        {
            get { return _running; }
        }

        public bool Single
        {
            get { return _runningSingle; }
        }
        
        /// <summary>
        /// True, if scrolling results is enabled
        /// </summary>
        private bool _scrollingEnabled = true;

        /// <summary>
        /// True, if scrolling results is enabled
        /// </summary>
        public bool ScrollingEnabled
        {
            get { return _scrollingEnabled; }
        }

        /// <summary>
        /// Enables/disables scrolling
        /// </summary>
        /// <param name="enable">true, if scrolling should be enabled</param>
        public void EnableScrolling(bool enable)
        {
            _scrollingEnabled = enable;
        }

        /// <summary>
        /// Indicates that single test is launched
        /// </summary>
        private bool _runningSingle;

        /// <summary>
        /// Tests execution start time (for report)
        /// </summary>
        protected DateTime _testExecutionTime;

        #endregion

        #region Results
        
        /// <summary>
        /// Test results.
        /// </summary>
        protected Dictionary<TestInfo, TestResult> _testResults;

        /// <summary>
        ///  Feature definition log.
        /// </summary>
        protected TestResult _featureDefinitionLog;

        /// <summary>
        /// Initialization data (features support, tests selection)
        /// </summary>
        private ConformanceInitializationData _initializationData;

        /// <summary>
        /// Gets test results. 
        /// </summary>
        /// <param name="testInfo">Test information.</param>
        /// <returns>Test result if test has been executed; otherwise null.</returns>
        public TestResult GetTestResult(TestInfo testInfo)
        {
            if (_testResults.ContainsKey(testInfo))
            {
                return _testResults[testInfo];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns feature definition log.
        /// </summary>
        /// <returns></returns>
        public TestResult GetFeaturesDefinitionLog()
        {
            return _featureDefinitionLog;
        }

        /// <summary>
        /// Current test's full log.
        /// </summary>
        private StringBuilder _plainTextLog;

        /// <summary>
        /// Current test's short log (without step events).
        /// </summary>
        private StringBuilder _shortTestLog;

        /// <summary>
        /// Current test's warnings log.
        /// </summary>
        private List<string> _warningsLog;
        
        /// <summary>
        /// Test results obtained so far
        /// </summary>
        private TestResult _testSteps;
        
        /// <summary>
        /// Current test
        /// </summary>
        public TestInfo CurrentTest
        {
            get { return _td.CurrentTest; }
        }

        /// <summary>
        /// Current log
        /// </summary>
        /// <returns></returns>
        public TestResult GetCurrentLog()
        {
            _testSteps.PlainTextLog = _plainTextLog.ToString();
            return _testSteps;
        }

        /// <summary>
        /// Clears test results.
        /// </summary>
        /// <param name="bAllResults">True if results at other tabs also should be cleared.</param>
        public void Clear(bool bAllResults)
        {
            _testResults.Clear();
            View.ClearTestResults();

            if (bAllResults)
            {
                ClearFeatures();
                _testsSupported = null;
                _profilesSupportInfo.Clear();
            }
            else
            {
                // Don't clear features definition info.
                // Leave also profile support information.
                if (_testsSupported != null)
                {
                    DisplayFunctionalitySupport(_testsSupported);
                }                
            }
            if (TestsCleared != null)
            {
                TestsCleared();
            }
        }

        /// <summary>
        /// True if test results are available (for "Save" button)
        /// </summary>
        /// <returns></returns>
        public bool HasTestResults()
        {
            return _testResults.Count > 0;
        }

        /// <summary>
        /// True if warnings in test results are available (for "Save Warnings" button)
        /// </summary>
        /// <returns></returns>
        public bool HasWarningsInTests()
        {
            bool flag = false;

            if (_testResults.Values.Count > 0)
            {
                foreach (var value in _testResults.Values)
                {
                    if (value.Warnings != null && value.Warnings.Count > 0)
                    {
                        flag = true;
                        break;
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// True if warnings in features log are available (for "Save Warnings" button)
        /// </summary>
        /// <returns></returns>
        public bool HasWarningsInFeaturesLog()
        {
            bool flag = false;

            if (_featureDefinitionLog != null && _featureDefinitionLog.Warnings != null && 
                _featureDefinitionLog.Warnings.Count > 0)
                flag = true;

            return flag;
        }

        public bool FeaturesDefined
        {
            get { return _td.FeaturesDefined; }
        }

        /// <summary>
        /// Clears feature support information
        /// </summary>
        public void ClearFeatures()
        {
            _featureDefinitionLog = null;
            _td.ResetFeatures();
            if (View.FeaturesView != null)
            {
                View.FeaturesView.Clear();
            }
        }

        /// <summary>
        /// Returns profiles information
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public ProfileTestInfo GetProfileInformation(IProfileDefinition profile)
        {
            if (_profilesSupportInfo.ContainsKey(profile))
            {
                return _profilesSupportInfo[profile];
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Device information
        /// </summary>
        public DeviceInformation DeviceInformation
        {
            get
            {
                return _initializationData != null ? _initializationData.DeviceInformation : null;
            }
        }

        #endregion

        #region View/Context

        /// <summary>
        /// Updates view functions.
        /// </summary>
        public override void UpdateViewFunctions()
        {
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            bool bHasAddress = devices != null && !string.IsNullOrEmpty(devices.ServiceAddress);
            View.EnableTestRun(bHasAddress);
        }

        /// <summary>
        /// Updates context.
        /// </summary>
        public override void UpdateContext()
        {
            TestOptions options = new TestOptions();

            if (View.TestTreeView != null)
            {
                var selectedTests = View.TestTreeView.SelectedTests;
                foreach (TestInfo info in selectedTests)
                {
                    options.Tests.Add(string.Format("{0}|{1}", info.Category, info.Order));
                }

                options.Groups.AddRange(View.TestTreeView.SelectedGroups);
            }
            options.Repeat = View.Repeat;
            ContextController.UpdateTestOptions(options);
        }
        
        /// <summary>
        /// Updates test log in "context".
        /// </summary>
        public void UpdateTestLog()
        {
            TestLog log = new TestLog();
            log.TestResults = _testResults;
            log.Features = _td.Features;
            log.TestExecutionTime = _testExecutionTime;
            log.InitializationData = _initializationData;
            log.FeaturesDefinitionLog = _featureDefinitionLog;

            log.Timeouts = new RealTimeouts();
            TrafficListener.GetAggregated(out log.Timeouts.Maximum, out log.Timeouts.Median, out log.Timeouts.Average);
            System.Diagnostics.Debug.WriteLine(string.Format("RunTests stopped with Max={0}, Median={1} and Mean={2}",
                log.Timeouts.Maximum,
                log.Timeouts.Median,
                log.Timeouts.Average));

            ContextController.UpdateTestLog(log);
        }

        #endregion

        #region Profile

        /// <summary>
        /// Applies profile passed.
        /// </summary>
        /// <param name="profile">Profile data.</param>
        public void ApplyProfile(Profile profile)
        {
            // tests + test groups
            if (View.TestTreeView != null)
            {
                View.TestTreeView.ApplyProfileOptions(profile);
            }
        }
        
        #endregion

        #region Save Debug report

        /// <summary>
        /// Save selected results
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="results"></param>
        public void Save(string fileName, List<TestResult> results)
        {
            SaveInternal(fileName, results);
        }

        /// <summary>
        /// Save selected results
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="results"></param>
        public void Save(string fileName, List<TestResultWithWarnings> results)
        {
            SaveWarningsInternal(fileName, results);
        }

        /// <summary>
        /// Save single test
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="results"></param>
        public void Save(string fileName, TestResult results)
        {
            SaveInternal(fileName, results);
        }

        /// <summary>
        /// Common "Save" procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="results"></param>
        void SaveInternal<T>(string fileName, T results)
        {
            StreamWriter writer = null;
            try
            {
                DebugReport<T> report = new DebugReport<T>();
                report.ExecutionTime = DateTime.Now;

                DeviceInfo info = new DeviceInfo();
                if (_initializationData != null && _initializationData.DeviceInformation != null)
                {
                    info.FirmwareVersion = _initializationData.DeviceInformation.FirmwareVersion;
                    info.HardwareID = _initializationData.DeviceInformation.HardwareID;
                    info.Manufacturer = _initializationData.DeviceInformation.Manufacturer;
                    info.Model = _initializationData.DeviceInformation.Model;
                    info.SerialNumber = _initializationData.DeviceInformation.SerialNumber;
                }
                report.DeviceInfo = info;

                report.Results = results;

                {                    
                    Data.Log.ManagementSettings settings = ContextController.GetManagementSettings();
                    report.ManagementSettings = settings;
                }
                writer = new StreamWriter(fileName);
                XmlSerializer s = new XmlSerializer(typeof(DebugReport<T>));
                s.Serialize(writer, report);
            }
            catch (Exception exc)
            {
                View.ShowError(exc);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// "Save Warnings" procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="results"></param>
        void SaveWarningsInternal<T>(string fileName, T results)
        {
            StreamWriter writer = null;
            try
            {
                WarningsReport<T> report = new WarningsReport<T>();
                report.ExecutionTime = DateTime.Now;

                report.Results = results;

                writer = new StreamWriter(fileName);
                XmlSerializer s = new XmlSerializer(typeof(WarningsReport<T>));
                s.Serialize(writer, report);
            }
            catch (Exception exc)
            {
                View.ShowError(exc);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        #endregion
        
    }
}
