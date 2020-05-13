///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TestTool.GUI.Views;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestEngine;
using System.IO;
using TestTool.Tests.Common.Trace;
using TestTool.GUI.Data;
using TestTool.GUI.Enums;
using TestLog=TestTool.GUI.Data.TestLog;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// GUI logic for the Test tab.
    /// </summary>
    class TestController : Controller<ITestView>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view">View.</param>
        public TestController(ITestView view)
            :base(view)
        {
            _td = new TestDispatcher();
            _td.TestStarted += _td_TestStarted;
            _td.TestCompleted += _td_TestCompleted;
            _td.StepStarted += _td_StepStarted;
            _td.StepCompleted += _td_StepCompleted;
            _td.RequestSent += _td_RequestSent;
            _td.ResponseReceived += _td_ResponseReceived;
            _td.TestSuiteCompleted += _td_TestSuiteCompleted;
            _td.OnException += _td_OnException;
            _td.StepEvent += _td_StepEvent;
            _td.TestEvent += _td_TestEvent;
            _td.Paused += _td_Paused;
            _testResults = new Dictionary<TestInfo, TestResult>();
        }

        
        private bool _running;

        /// <summary>
        /// Indicates that tests are being performed.
        /// </summary>
        public bool Running
        {
            get { return _running; }

        }

        private bool _runningSingle;

        private int _testsCount;
        
        /// <summary>
        /// Test results.
        /// </summary>
        private Dictionary<TestInfo, TestResult> _testResults;

        /// <summary>
        /// Is raised when a test suite is started.
        /// </summary>
        public event Action<TestSuiteParameters> TestSuiteStarted;

        /// <summary>
        /// Is raised when a test suite is completed.
        /// </summary>
        public event Action TestSuiteCompleted;

        /// <summary>
        /// Is raised when some event occurs in the test.
        /// </summary>
        public event Action<string> TestEvent;

        /// <summary>
        /// Is raised when user clicks "Clear" button.
        /// </summary>
        public event Action TestsCleared;

        /// <summary>
        /// Is raised after tests are loaded.
        /// </summary>
        public event Action<List<TestInfo>> TestsLoaded;

        /// <summary>
        /// Updates view functions.
        /// </summary>
        public override void UpdateViewFunctions()
        {
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            bool bHasAddress = devices != null && !string.IsNullOrEmpty(devices.ServiceAddress);
            View.EnableTestRun(bHasAddress);
        }

        #region LOAD

        private List<TestInfo> _testInfos;

        /// <summary>
        /// Initialization.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
           LoadTests();
        }

        /// <summary>
        /// Enumerates files in current directory and finds tests.
        /// </summary>
        /// <returns>List of tests loaded.</returns>
        public List<TestInfo> LoadTests()
        {
            _testInfos = new List<TestInfo>();

            string location = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(location);

            foreach (string file in Directory.GetFiles(path, "*.dll"))
            {
                try
                {
                    System.Reflection.Assembly assembly = Assembly.LoadFile(file);
                    if (assembly.GetCustomAttributes(
                        typeof(TestAssemblyAttribute),
                        false).Length > 0)
                    {
                        // Test assembly

                        foreach (Type t in assembly.GetTypes())
                        {
                            object[] attrs = t.GetCustomAttributes(typeof(TestClassAttribute), true);
                            if (attrs.Length > 0)
                            {
                                if (t.IsSubclassOf(typeof(Tests.Common.TestBase.BaseTest)))
                                {
                                    foreach (MethodInfo mi in t.GetMethods())
                                    {
                                        object[] testAttributes = mi.GetCustomAttributes(typeof(TestAttribute), true);

                                        if (testAttributes.Length > 0)
                                        {
                                            TestAttribute attribute = (TestAttribute)testAttributes[0];
                                            
                                            TestInfo existing =
                                                _testInfos.Where(ti => ti.Order == attribute.Order).FirstOrDefault();

                                            if (existing != null)
                                            {
                                                System.Diagnostics.Debug.WriteLine(string.Format("One more test with order {0} found {1}", attribute.Order, attribute.Name));

                                                if (existing.Version > attribute.Version)
                                                {
                                                    System.Diagnostics.Debug.WriteLine("Leave test already loaded");
                                                    continue;
                                                }
                                                else
                                                {
                                                    System.Diagnostics.Debug.WriteLine("Reload newer test");
                                                    _testInfos.Remove(existing);
                                                }
                                            }

                                            TestInfo testInfo = new TestInfo();
                                            testInfo.Method = mi;
                                            testInfo.Name = attribute.Name;
                                            testInfo.Group = attribute.Path;
                                            testInfo.Order = attribute.Order;
                                            testInfo.Version = attribute.Version;
                                            testInfo.Interactive = attribute.Interactive;
                                            testInfo.RequirementLevel = attribute.RequirementLevel;
                                            testInfo.RequiredFeatures.AddRange(attribute.RequiredFeatures);
                                            testInfo.Services.AddRange(attribute.Services);

                                            _testInfos.Add(testInfo);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {

                }
            }

            View.DisplayTests(_testInfos);

            if (TestsLoaded != null)
            {
                TestsLoaded(_testInfos);
            }
            return _testInfos;
        }

        #endregion

        #region RUN

        /// <summary>
        /// Object responsible for running tests.
        /// </summary>
        private TestDispatcher _td;

        /// <summary>
        /// Current test's full log.
        /// </summary>
        private StringBuilder _plainTextLog;

        /// <summary>
        /// Current test's short log (without step events).
        /// </summary>
        private StringBuilder _shortTestLog;

        /// <summary>
        /// Launches test suite.
        /// </summary>
        /// <param name="parameters">Test suite parameters.</param>
        public void Run(TestSuiteParameters parameters)
        {
            InternalRun(parameters, false);
        }
        
        /// <summary>
        /// Launches single test.
        /// </summary>
        /// <param name="parameters">Test suite parameters.</param>
        public void RunSingle(TestSuiteParameters parameters)
        {
            InternalRun(parameters, true);
        }

        private DateTime _testExecutionTime;

        /// <summary>
        /// Launches tests.
        /// </summary>
        /// <param name="parameters">Parameters passed from the View.</param>
        /// <param name="single">True if "Run current" button is in use. If "Run current" button has 
        /// been clicked, results collected by this moment will not be cleared.</param>
        void InternalRun(TestSuiteParameters parameters, bool single)
        {
            _running = true;
            _runningSingle = single;
            _testsCount = parameters.TestCases.Count;

            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            parameters.UserName = env.Credentials.UserName;
            parameters.Password = env.Credentials.Password;
            parameters.UseUTCTimestamp = env.Credentials.UseUTCTimeStamp;
            parameters.Operator = new Utils.Operator(View.Window);
            parameters.VideoForm = View.GetVideoForm();

            parameters.EnvironmentSettings = new Tests.Common.TestEngine.EnvironmentSettings()
                                                 {
                                                     DnsIpv4 = env.EnvironmentSettings.DnsIpv4, 
                                                     NtpIpv4 = env.EnvironmentSettings.NtpIpv4,
                                                     DnsIpv6 = env.EnvironmentSettings.DnsIpv6,
                                                     NtpIpv6 = env.EnvironmentSettings.NtpIpv6,
                                                     DefaultGateway = env.EnvironmentSettings.GatewayIpv4,
                                                     DefaultGatewayIpv6 = env.EnvironmentSettings.GatewayIpv6
                                                 };

            if (!single)
            {
                _testResults.Clear();
                _testExecutionTime = DateTime.Now;
            }

            if (TestSuiteStarted != null)
            {
                TestSuiteStarted(parameters);
            }
            
            _td.Run(parameters);
        }

        /// <summary>
        /// Clears test results.
        /// </summary>
        /// <param name="bAllResults">True if results at other tabs also should be cleared.</param>
        public void Clear(bool bAllResults)
        {
            _testResults.Clear();
            if (TestsCleared != null && bAllResults)
            {
                TestsCleared();
            }
        }

        /// <summary>
        /// Handles "TestStarted" event.
        /// </summary>
        /// <param name="testInfo">Test description.</param>
        void _td_TestStarted(TestInfo testInfo)
        {
            _plainTextLog = new StringBuilder();
            _shortTestLog = new StringBuilder();
            View.BeginTest(testInfo);
            WriteLog(testInfo.Name, LogEntryLevel.Test);
            WriteLog(string.Empty, LogEntryLevel.Test);

        }        

        /// <summary>
        /// Handles "TestConpleted" event.
        /// </summary>
        /// <param name="testInfo">Test description.</param>
        /// <param name="log">Test results.</param>
        void _td_TestCompleted(TestInfo testInfo, Tests.Common.Trace.TestLog log)
        {

            TestResult testResult = new TestResult();
            testResult.Log = log;
            testResult.TestInfo = testInfo;
            if ( !string.IsNullOrEmpty(log.ErrorMessage))
            {
                string errorMessage = string.Format("   Error: {0}{1}", log.ErrorMessage, Environment.NewLine);
                WriteLog(errorMessage, LogEntryLevel.Test);
            }

            string testResultLine = string.Empty;

            if (testResult.Log.TestStatus == TestStatus.NotSupported)
            {
                testResultLine = "DEVICE FEATURE NOT SUPPORTED BY THE NVT";
            }
            else
            {
                testResultLine = string.Format("TEST {0}", testResult.Log.TestStatus.ToString().ToUpper());
            }
            WriteLog(testResultLine, LogEntryLevel.Test);
            ReportTestEvent("------------------------------------");

            testResult.PlainTextLog = _plainTextLog.ToString();
            testResult.ShortTextLog = _shortTestLog.ToString();

            if (_runningSingle && _testResults.ContainsKey(testInfo))
            {
                _testResults[testInfo] = testResult;
            }
            else
            {
                _testResults.Add(testInfo, testResult);
            }
            View.EndTest(testResult);
        }

        /// <summary>
        /// Handles "Paused" event.
        /// </summary>
        void _td_Paused()
        {
            View.SwitchToState(ApplicationState.TestPaused);
        }

        /// <summary>
        /// Handles "StepStarted" event.
        /// </summary>
        /// <param name="result">Step result (at this stage only with Name initialized).</param>
        void _td_StepStarted(StepResult result)
        {
            string stepHeader = string.Format("STEP {0} - {1}", result.Number, result.StepName);
            WriteLog(stepHeader, LogEntryLevel.Step);
        }
        
        /// <summary>
        /// Handles "StepCompleted" event.
        /// </summary>
        /// <param name="result"></param>
        void _td_StepCompleted(StepResult result)
        {
            System.ServiceModel.FaultException fault = result.Exception as System.ServiceModel.FaultException;
            bool bFault = false;
            if (fault != null)
            {
                bFault = true;
            }

            string stepResult = string.Format("STEP {0}", result.Status.ToString().ToUpper());
            
            if (result.Status == StepStatus.Failed && !bFault)
            {
                WriteLog(result.Message, LogEntryLevel.StepDetails);
            }
            WriteLog(stepResult, LogEntryLevel.Step);
            WriteLog(string.Empty, LogEntryLevel.Test);

            View.DisplayStepResult(result);

        }

        /// <summary>
        /// Handles "RequestSent" event.
        /// </summary>
        /// <param name="request">Request data.</param>
        void _td_RequestSent(string request)
        {
            string logEntry = "Transmit done";
            WriteLog(logEntry, LogEntryLevel.StepDetails);
        }

        /// <summary>
        /// Handles "ResponseReceived" event.
        /// </summary>
        /// <param name="obj">Response data.</param>
        void _td_ResponseReceived(string obj)
        {
            string logEntry = "Receive done";
            WriteLog(logEntry, LogEntryLevel.StepDetails);
        }
        
        /// <summary>
        /// Handles step events.
        /// </summary>
        /// <param name="obj">Step event description.</param>
        void _td_StepEvent(string obj)
        {
            WriteLog(obj, LogEntryLevel.StepDetails);
        }

        /// <summary>
        /// Handles test event 
        /// </summary>
        /// <param name="obj">Test event description.</param>
        void _td_TestEvent(string obj)
        {
            WriteLog(obj, LogEntryLevel.Step);
        }

        /// <summary>
        /// Handles "TestSuiteCompleted" event.
        /// </summary>
        /// <param name="bCompletedNormally">Indicates whether test suite has not been halted.</param>
        void _td_TestSuiteCompleted(bool bCompletedNormally)
        {
            if (TestSuiteCompleted != null)
            {
                TestSuiteCompleted();
            }
            if ((_testsCount != 1 || !_runningSingle) && bCompletedNormally)
            {
                View.ReportTestSuiteCompleted();
            }
            _running = false;
        }

        /// <summary>
        /// Handles inner exception occurred when running tests.
        /// </summary>
        /// <param name="exc">Exception data.</param>
        void _td_OnException(Exception exc)
        {
            string message = string.Format("Inner exception: {0}{1}", exc.Message, Environment.NewLine);
            WriteLog(message, LogEntryLevel.Test);
        }

        /// <summary>
        /// Writes log where necessary.
        /// </summary>
        /// <param name="entry">Log entry.</param>
        /// <param name="level">Log entry additional data (at which level the event occurred.)</param>
        void WriteLog(string entry, LogEntryLevel level)
        {
            string indent = string.Empty;
            switch (level)
            {
                case LogEntryLevel.Step:
                    indent = "   ";
                    break;
                case LogEntryLevel.StepDetails:
                    indent = "      ";
                    break;
            }

            string offset = Environment.NewLine + indent;
            string line = string.Format("{0}{1}", indent, entry.Replace(Environment.NewLine, offset));

            _plainTextLog.AppendLine(line);
            if (level == LogEntryLevel.Test || level == LogEntryLevel.Step)
            {
                _shortTestLog.AppendLine(line);
            }
            View.WriteLine(line);
            ReportTestEvent(line);
        }

        /// <summary>
        /// Raises "TestEvent" event.
        /// </summary>
        /// <param name="entry">Test event description.</param>
        void ReportTestEvent(string entry)
        {
            if (TestEvent != null)
            {
                TestEvent(entry);
            }
        }

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
        /// Pauses tests execution.
        /// </summary>
        public void Pause()
        {
            switch (_td.State)
            {
                case TestState.Running:
                    {
                        _td.Pause();
                    }
                    break;
                case TestState.Paused:
                    {
                        _td.Resume();
                        View.SwitchToState(ApplicationState.TestRunning);
                    }
                    break;
                case TestState.Idle:
                    {
                        System.Diagnostics.Debug.WriteLine("Pause button pressed when test is not running!");
                    }
                    break;
            }
        }

        /// <summary>
        /// Stops tests execution at the end of current tests.
        /// </summary>
        public override void Stop()
        {
            _td.Stop();
        }

        /// <summary>
        /// Stops tests execution immediately.
        /// </summary>
        public void Halt()
        {
            _td.Halt();
        }

        /// <summary>
        /// Stops tests execution without raising additional events.
        /// </summary>
        public void Exit()
        {
            _td.Shutdown();
        }

        /// <summary>
        /// TestDispatcher state.
        /// </summary>
        public TestState State
        {
            get { return _td.State; }
        }

        #endregion

        /// <summary>
        /// Updates context.
        /// </summary>
        public override void UpdateContext()
        {
            TestOptions options = new TestOptions();

            options.InteractiveFirst = View.InteractiveFirst;
            
            List<TestInfo> selectedTests = View.SelectedTests;
            foreach (TestInfo info in selectedTests)
            {
                options.Tests.Add(info.Order);
            }

            options.Groups.AddRange(View.SelectedGroups);

            ContextController.UpdateTestOptions(options);

            TestLog log = new TestLog();
            log.TestResults = _testResults;
            log.Tests = _testInfos;
            log.TestExecutionTime = _testExecutionTime;
            
            ContextController.UpdateTestLog(log);

        }

        /// <summary>
        /// Applies profile passed.
        /// </summary>
        /// <param name="profile">Profile data.</param>
        public void ApplyProfile(Profile profile)
        {
            // InteractiveFirst button + tests + test groups
            View.ApplyProfileOptions(profile);
        }
    
        /// <summary>
        /// Selects tests depending on features selected.
        /// </summary>
        /// <param name="features">Features</param>
        public void SelectFeatureDependentTests(IEnumerable<Feature> features)
        {
            // "Must" and "Should" tests are selected when corresponding services are selected. 
            // "Optional" tests don't change their state. 
            // "Conditional Must", "Conditional Should" become checked or unchecked  
            // accordingly to features selection.
            //
            
            List<TestInfo> tests = new List<TestInfo>();

            DeviceEnvironment environment = ContextController.GetDeviceEnvironment();

            foreach (TestInfo testInfo in _testInfos)
            {
                bool add = true;

                switch (testInfo.RequirementLevel)
                {
                    case RequirementLevel.ConditionalMust:
                    case RequirementLevel.ConditionalShould:
                        {
                            add = Utils.FeaturesHelper.AllFeaturesSelected(testInfo);
                        }
                        break;
                    case RequirementLevel.Optional:
                        add = false;
                        break;
                }
                if (add)
                {
                    tests.Add(testInfo);
                }
            }

            View.SelectFeatureDependentTests(tests);
        }

        /// <summary>
        /// Enters/exits certification mode.
        /// </summary>
        /// <param name="bOn"></param>
        public void SetCertificationMode(bool bOn)
        {
            //
            // If certification mode is ON, "Must", "Should", "Optional" tests are selected.
            // "Conditional must", "Condiitonal should" are selected or unselected accordingly to
            // features selection.
            // Only "Should", "ConditionalShould" and "Optional" tests can be checked or unchecked 
            // by the user.
            //
            //
            //
            //

            if (bOn)
            {
                
            }
            View.SetCertificationMode(bOn);
        }
    }
}
