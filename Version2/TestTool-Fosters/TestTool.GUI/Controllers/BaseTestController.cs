using System;
using System.Collections.Generic;
using System.Text;
using TestTool.GUI.Views;
using TestTool.GUI.Data;
using TestTool.Tests.Common.TestEngine;
using TestTool.GUI.Enums;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Base operations for controller performing testing.
    /// </summary>
    /// <typeparam name="T">View types</typeparam>
    /// <remarks>This base class has been added when possibility of having TWO controllers which perform testing 
    /// was considered. Now all testing operations are performed by only one controller, but "pure" testing operations 
    /// were left here. </remarks>
    class BaseTestController<T> : Controller<T>
        where T : IView
    {
        /// <summary>
        /// Performs initialization.
        /// </summary>
        /// <param name="view">View.</param>
        public BaseTestController(T view)
            :base(view)
        {
        }

        #region Properties

        private bool _conformance;

        /// <summary>
        /// True, if conformance testing is being performed; false when tests are performed in Diagnostic mode.
        /// </summary>
        protected bool Conformance
        {
            get { return _conformance; }
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

        /// <summary>
        /// Indicates that single test is launched
        /// </summary>
        private bool _runningSingle;

        #endregion

        /// <summary>
        /// Tests execution start time (for report)
        /// </summary>
        protected DateTime _testExecutionTime;

        #region EVENTS

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

        #endregion

        #region RESULTS

        /// <summary>
        /// Test results.
        /// </summary>
        protected Dictionary<TestInfo, TestResult> _testResults;

        /// <summary>
        ///  Feature definition log.
        /// </summary>
        protected TestResult _featureDefinitionLog;

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
        /// Object responsible for running tests.
        /// </summary>
        protected TestDispatcher _td;
        
        /// <summary>
        /// Current test's full log.
        /// </summary>
        private StringBuilder _plainTextLog;

        /// <summary>
        /// Current test's short log (without step events).
        /// </summary>
        private StringBuilder _shortTestLog;

        #endregion

        /// <summary>
        /// Subscribers to dispatcher's events.
        /// </summary>
        protected void InitTestEnvironment()
        {
            _td = new TestDispatcher();
            _td.TestStarted += _td_TestStarted;
            _td.TestCompleted += _td_TestCompleted;
            _td.TestSuiteCompleted += _td_TestSuiteCompleted;
            _td.StepStarted += _td_StepStarted;
            _td.StepCompleted += _td_StepCompleted;
            _td.RequestSent += _td_RequestSent;
            _td.ResponseReceived += _td_ResponseReceived;
            _td.OnException += _td_OnException;
            _td.StepEvent += _td_StepEvent;
            _td.TestEvent += _td_TestEvent;
            
            _testResults = new Dictionary<TestInfo, TestResult>();
        }

        #region START/STOP/STATE

        /// <summary>
        /// Launches tests.
        /// </summary>
        /// <param name="parameters">Parameters passed from the View.</param>
        /// <param name="single">True if "Run current" button is in use. If "Run current" button has 
        /// been clicked, results collected by this moment will not be cleared.</param>
        protected void InternalRun(TestSuiteParameters parameters, bool single, bool conformance)
        {
            _running = true;
            
            if (!single)
            {
                _testResults.Clear();
                _testExecutionTime = DateTime.Now;
            }

            if (parameters.FeatureDefinition)
            {
                _testResults.Clear();
            }
            _conformance = conformance;
            _runningSingle = single;

            ReportTestSuiteStarted(parameters, conformance);

            if (parameters.FeatureDefinition)
            {
                _td.RequestFeatures(parameters);
            }
            else
            {
                _td.Run(parameters);
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
        /// Collects parameters for launching current test/current test group/all selected tests
        /// </summary>
        /// <returns></returns>
        public TestSuiteParameters GetParameters()
        {
            TestSuiteParameters parameters = new TestSuiteParameters();
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            parameters.Address = devices.ServiceAddress;
            parameters.CameraIP = devices.DeviceAddress;
            parameters.NetworkInterfaceController = devices.NIC;
            if ((devices.Current != null) && (devices.Current.ByDiscovery != null))
            {
                parameters.CameraUUID = devices.Current.ByDiscovery.UUID;
            }

            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            parameters.UserName = env.Credentials.UserName;
            parameters.Password = env.Credentials.Password;
            parameters.UseUTCTimestamp = env.Credentials.UseUTCTimeStamp;
            
            DeviceEnvironment environment = ContextController.GetDeviceEnvironment();
            parameters.MessageTimeout = environment.Timeouts.Message;
            parameters.RebootTimeout = environment.Timeouts.Reboot;
            parameters.TimeBetweenTests = environment.Timeouts.InterTests;
            parameters.PTZNodeToken = environment.TestSettings.PTZNodeToken;
            
            parameters.EnvironmentSettings = new Tests.Common.TestEngine.EnvironmentSettings()
            {
                DnsIpv4 = env.EnvironmentSettings.DnsIpv4,
                NtpIpv4 = env.EnvironmentSettings.NtpIpv4,
                DnsIpv6 = env.EnvironmentSettings.DnsIpv6,
                NtpIpv6 = env.EnvironmentSettings.NtpIpv6,
                DefaultGateway = env.EnvironmentSettings.GatewayIpv4,
                DefaultGatewayIpv6 = env.EnvironmentSettings.GatewayIpv6
            };

            parameters.UseEmbeddedPassword = environment.TestSettings.UseEmbeddedPassword;
            parameters.Password1 = environment.TestSettings.Password1;
            parameters.Password2 = environment.TestSettings.Password2;
            parameters.SecureMethod = environment.TestSettings.SecureMethod;
            parameters.VideoSourceToken = environment.TestSettings.VideoSourceToken;

            parameters.OperationDelay = environment.TestSettings.OperationDelay;
            parameters.RecoveryDelay = environment.TestSettings.RecoveryDelay;

            parameters.EventTopic = environment.TestSettings.EventTopic;
            parameters.SubscriptionTimeout = environment.TestSettings.SubscriptionTimeout;
            parameters.TopicNamespaces = environment.TestSettings.TopicNamespaces;
            parameters.RelayOutputDelayTimeMonostable = environment.TestSettings.RelayOutputDelayTimeMonostable;

            Dictionary<string, object> advanced = new Dictionary<string, object>();
            foreach (object o in environment.TestSettings.AdvancedSettings)
            {
                advanced.Add(o.GetType().GUID.ToString(), o);
            }
            parameters.AdvancedPrameters = advanced;

            return parameters;
        }

        /// <summary>
        /// Handles "TestSuiteCompleted" event.
        /// </summary>
        /// <param name="bCompletedNormally">Indicates whether test suite has not been halted.</param>
        void _td_TestSuiteCompleted(TestSuiteParameters parameters, bool bCompletedNormally)
        {
            if (bCompletedNormally && !Conformance)
            {
                ReportTestSuiteCompleted();
            }

            CompleteTestSuite(parameters, bCompletedNormally);

            if (TestSuiteCompleted != null)
            {
                TestSuiteCompleted(bCompletedNormally);
            }
            _running = false;
        }


        /// <summary>
        /// Handles "TestStarted" event.
        /// </summary>
        /// <param name="testInfo">Test description.</param>
        void _td_TestStarted(TestInfo testInfo)
        {
            DisplayTestStart(testInfo);

            _plainTextLog = new StringBuilder();
            _shortTestLog = new StringBuilder();

            WriteLog(testInfo.Name, LogEntryLevel.Test);
            WriteLog(string.Empty, LogEntryLevel.Test); 
            
            if (TestStarted != null)
            {
                TestStarted(testInfo);
            }
        } 

        /// <summary>
        /// Handles "TestCompleted" event.
        /// </summary>
        /// <param name="testInfo">Test description.</param>
        /// <param name="log">Test results.</param>
        void _td_TestCompleted(TestInfo testInfo, Tests.Definitions.Trace.TestLog log)
        {
            bool isTest = testInfo.ProcessType == ProcessType.Test;

            bool overwriteResult = _runningSingle && _testResults.ContainsKey(testInfo);

            TestResult testResult = new TestResult();
            testResult.Log = log;
            testResult.TestInfo = testInfo;
            if (!string.IsNullOrEmpty(log.ErrorMessage))
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
                if (isTest)
                {
                    testResultLine = string.Format("TEST {0}", testResult.Log.TestStatus.ToString().ToUpper());
                }
                else
                {
                    //testResultLine = string.Format("PROCESS {0}", testResult.Log.TestStatus == TestStatus.Passed ? "COMPLETED OK" : "FAILED");
                    testResultLine = "PROCESS COMPLETED";
                }
            }
            WriteLog(testResultLine, LogEntryLevel.Test);

            testResult.PlainTextLog = _plainTextLog.ToString();
            testResult.ShortTextLog = _shortTestLog.ToString();

            if (isTest)
            {
                if (overwriteResult)
                {
                    _testResults[testInfo] = testResult;
                }
                else
                {
                    _testResults.Add(testInfo, testResult);
                }
            }
            else
            {
                _featureDefinitionLog = testResult;
            }
            CompleteTest(testInfo, log);

            if (TestCompleted != null)
            {
                TestCompleted(testInfo, log);
            }
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

            DisplayStepResult(result);
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
        /// Handles inner exception occurred when running tests.
        /// </summary>
        /// <param name="exc">Exception data.</param>
        void _td_OnException(Exception exc)
        {
            StringBuilder dump = new StringBuilder("Inner exception: " + Environment.NewLine);
            Exception current = exc;
            string offset = string.Empty;
            while (current != null)
            {
                dump.AppendFormat("{0}{1}{2}", offset, current.Message, Environment.NewLine);
                current = current.InnerException;
                offset += "   ";
            }
            
            string message = dump.ToString();
            WriteLog(message, LogEntryLevel.Test);
        }
        
        /// <summary>
        /// Raises TestSuiteStarted event if handler is not empty
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="conformance"></param>
        protected void ReportTestSuiteStarted(TestSuiteParameters parameters, bool conformance)
        {
            if (TestSuiteStarted != null)
            {
                TestSuiteStarted(parameters, conformance);
            }
        }
 
        /// <summary>
        /// Creates log entry and appends to log where it is necessary
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="level"></param>
        protected void WriteLog(string entry, LogEntryLevel level)
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

            DisplayLogLine(line);
        }


        /// <summary>
        /// Displays log line using concrete View
        /// </summary>
        /// <param name="line"></param>
        protected virtual void DisplayLogLine(string line)
        {

        }

        /// <summary>
        /// Displays step result
        /// </summary>
        /// <param name="result"></param>
        protected virtual  void DisplayStepResult(StepResult result)
        {

        }

        /// <summary>
        /// Notifies user that test is started
        /// </summary>
        /// <param name="testInfo"></param>
        protected virtual void DisplayTestStart(TestInfo testInfo)
        {

        }

        /// <summary>
        /// Completes test
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="log"></param>
        protected virtual void CompleteTest(TestInfo testInfo, Tests.Definitions.Trace.TestLog log)
        {

        }

        protected virtual void CompleteTestSuite(TestSuiteParameters parameters, bool bCompletedNormally)
        {

        }

        /// <summary>
        /// Reports that testsuite is completed
        /// </summary>
        protected virtual void ReportTestSuiteCompleted()
        {

        }
    }
}
