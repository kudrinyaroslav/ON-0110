using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.UI;
using System.Reflection;
using System.IO;
using TestTool.Tests.Definitions.Attributes;
using System.Windows.Forms;
using TestTool.Tests.Engine;
using AutomatedTesting.GUI.Data;
using TestTool.Tests.Definitions.Trace;
using AutomatedTesting.GUI.ExternalData;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Xml.Serialization;

namespace AutomatedTesting.GUI.Controllers
{
    enum LogEntryLevel
    {
        Test,
        Step,
        StepDetails
    }

    class TestController
    {
        ITestView _view;
        
        public TestController(ITestView view)
        {
            _view = view;

            _td = new TestDispatcher();

            _td.TestStarted += _td_TestStarted;
            _td.TestCompleted += _td_TestCompleted;

            _td.StepStarted += _td_StepStarted;
            _td.StepCompleted += _td_StepCompleted;

            _td.RequestSent += _td_RequestSent;
            _td.ResponseReceived += _td_ResponseReceived;

            _td.OnException += _td_OnException;
            _td.StepEvent += _td_StepEvent;
            _td.TestEvent += _td_TestEvent;

            _td.TestSuiteCompleted += new Action<TestSuiteParameters, bool>(_td_TestSuiteCompleted);

            string address = System.Configuration.ConfigurationManager.AppSettings["DutManagementAddress"];
            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            System.ServiceModel.EndpointAddress endpointAddress = new System.ServiceModel.EndpointAddress(address);
            _dutClient = new AutomatedTesting.DutManagement.DutManagementService.TestSoapClient(binding, endpointAddress);
        }

        List<TestSuite> _testSuites = new List<TestSuite>();

        public List<TestSuite> TestSuites
        {
            get { return _testSuites; }
        }

        List<DutTest> _dutTests = new List<DutTest>();

        public List<DutTest> DutTests
        {
            get { return _dutTests; }
        }


        #region LOAD

        /// <summary>
        /// Tests information
        /// </summary>
        private List<TestInfo> _testInfos;

        /// <summary>
        /// Profiles known
        /// </summary>
        private List<IProfileDefinition> _onvifProfiles;

        /// <summary>
        /// Tests list
        /// </summary>
        public List<TestInfo> TestInfos
        {
            get { return _testInfos; }
        }

        /// <summary>
        /// List of supported tests.
        /// This list is initialized when feature definition completes. Later when all results are 
        /// cleared, this list is also cleared; but if only test results are cleared, profiles tree is 
        /// coloured using this information: list of selected tests is used to define features which will be tested.
        /// </summary>
        private List<TestInfo> _testsSupported;
         
        /// <summary>
        /// Initialization.
        /// </summary>
        public void Initialize()
        {
            LoadTests();
        }

        /// <summary>
        /// Enumerates files in current directory and finds tests.
        /// </summary>
        /// <returns>List of tests loaded.</returns>
        public List<TestInfo> LoadTests()
        {
            // initialize lists
            _testInfos = new List<TestInfo>();
            _onvifProfiles = new List<IProfileDefinition>();

            Dictionary<Type, Type> controls = new Dictionary<Type, Type>();

            string location = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(location);

            // enumerate files
            foreach (string file in Directory.GetFiles(path, "*.dll"))
            {
                try
                {
                    System.Reflection.Assembly assembly = Assembly.LoadFile(file);

                    // check if this is a test assembly
                    if (assembly.GetCustomAttributes(
                        typeof(TestAssemblyAttribute),
                        false).Length > 0)
                    {
                        // Test assembly

                        // enumerate types
                        foreach (Type t in assembly.GetTypes())
                        {
                            // Load tests, if this is a test class
                            object[] attrs = t.GetCustomAttributes(typeof(TestClassAttribute), true);
                            if (attrs.Length > 0)
                            {
                                LoadTests(t);
                            }

                            // Load profiles, if this is sa profile
                            attrs = t.GetCustomAttributes(typeof(ProfileDefinitionAttribute), true);
                            if (attrs.Length > 0)
                            {
                                LoadProfile(t);
                            }
                        }
                    }
                }
                catch (Exception exc)
                {

                }
            }
            
            return _testInfos;
        }

        /// <summary>
        /// Loads tests from the type specified
        /// </summary>
        /// <param name="t"></param>
        void LoadTests(Type t)
        {
            // if this is a test class,
            if (t.GetInterfaces().Contains(typeof(ITest)))
            {
                // enumerate methods.
                foreach (MethodInfo mi in t.GetMethods())
                {
                    object[] testAttributes = mi.GetCustomAttributes(typeof(TestAttribute), true);

                    // if this is a test method,
                    if (testAttributes.Length > 0)
                    {
                        // get test information.
                        TestAttribute attribute = (TestAttribute)testAttributes[0];

                        // check if a test needs to be updated
                        TestInfo existing =
                            _testInfos.Where(ti => ti.Id == attribute.Id && ti.Category == attribute.Category).FirstOrDefault();

                        if (existing != null)
                        {
                            System.Diagnostics.Debug.WriteLine(string.Format("--- One more test with order {0} found {1}", attribute.Order, attribute.Name));

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

                        // add test info
                        TestInfo testInfo = new TestInfo();
                        testInfo.Method = mi;
                        testInfo.Name = attribute.Name;
                        testInfo.Group = attribute.Path;
                        testInfo.Order = attribute.Order;
                        testInfo.ExecutionOrder = attribute.ExecutionOrder;
                        testInfo.Id = attribute.Id;
                        testInfo.Category = attribute.Category;
                        testInfo.Version = attribute.Version;
                        testInfo.RequirementLevel = attribute.RequirementLevel;
                        testInfo.RequiredFeatures.AddRange(attribute.RequiredFeatures);
                        testInfo.FunctionalityUnderTest.AddRange(attribute.FunctionalityUnderTest);

                        _testInfos.Add(testInfo);

                    }
                }
            }
        }

        /// <summary>
        /// Loads profile from the type
        /// </summary>
        /// <param name="t"></param>
        void LoadProfile(Type t)
        {
            if (t.GetInterfaces().Contains(typeof(IProfileDefinition)))
            {
                IProfileDefinition profile = (IProfileDefinition)Activator.CreateInstance(t, new object[0]);
                _onvifProfiles.Add(profile);
            }
        }



        #endregion

        #region RUN

        /// <summary>
        /// Object responsible for running tests.
        /// </summary>
        protected TestDispatcher _td;
        
        void _td_TestSuiteCompleted(TestSuiteParameters parameters, bool completedNormally)
        {
            _view.EndTesting();
            
        }
        
        public bool FeaturesDefined
        {
            get { return _td.FeaturesDefined; }
        }

        
        public void RunTests(Task task)
        {
            _view.BeginTesting();
            TestSuiteParameters parameters = GetTestSuiteParameters(task);

            _currentTask = new List<TestCaseSettings>();

            if (task.FeatureDefnitionSettings != null)
            {
                _td.ResetFeatures();
                _currentTask.Add(task.FeatureDefnitionSettings);
            }

            bool testsExist = false;

            foreach (TestCaseSettings tc in task.Tests)
            {
                TestInfo ti = _testInfos.Where(TI => TI.Category == tc.Category && TI.Id == tc.TestID).FirstOrDefault();
                if (ti != null)
                {
                    parameters.TestCases.Add(ti);
                    _currentTask.Add(tc);
                    testsExist = true;
                }      
            }

            if (testsExist)
            {
                parameters.FeatureDefinition = FeatureDefinitionMode.Default;            
            }
            else 
            {
                parameters.FeatureDefinition = FeatureDefinitionMode.Define;
            }

            _currentTestIdx = 0;
            EnterNextTask();
            // setup DUT

            _td.Run(parameters);
        }
        
        TestSuiteParameters GetTestSuiteParameters(Task task)
        {
            TestParameters source = task.Parameters;

            TestSuiteParameters parameters = new TestSuiteParameters();
            parameters.Address = source.DeviceServiceAddress;
            parameters.AllTestCases.AddRange(_testInfos);
            parameters.Conformance = false;
            parameters.DefineProfiles = false;
            parameters.EnvironmentSettings = new TestTool.Tests.Common.TestEngine.EnvironmentSettings();
            parameters.EnvironmentSettings.DefaultGateway = source.DefaultGatewayIpv4;
            parameters.EnvironmentSettings.DefaultGatewayIpv6 = source.DefaultGatewayIpv6;
            parameters.EnvironmentSettings.DnsIpv4 = source.DnsIpv4;
            parameters.EnvironmentSettings.DnsIpv6 = source.DnsIpv6;
            parameters.EnvironmentSettings.NtpIpv4 = source.NtpIpv4;
            parameters.EnvironmentSettings.NtpIpv6 = source.NtpIpv6;
            parameters.EventTopic = source.EventTopic;
            parameters.FeatureDefinition = FeatureDefinitionMode.Default;
            parameters.MessageTimeout = source.MessageTimeout.Value;
            parameters.MetadataFilter = source.MetadataFilter;
            parameters.OperationDelay = source.OperationDelay.Value;
            parameters.Password = source.Password;
            parameters.Password1 = source.Password1;
            parameters.Password2 = source.Password2;
            parameters.PTZNodeToken = source.PTZNodeToken;
            parameters.RebootTimeout = source.RebootTimeout.Value;
            parameters.RecordingToken = source.RecordingToken;
            parameters.RecoveryDelay = source.RecoveryDelay.Value;
            parameters.RelayOutputDelayTimeMonostable = source.RelayOutputDelayTime.Value;
            parameters.SearchTimeout = source.SearchTimeout.Value;
            parameters.SecureMethod = source.SecureMethod;
            parameters.SubscriptionTimeout = source.SubscriptionTimeout.Value;
            parameters.TimeBetweenTests = source.TimeBetweenTests.Value;
            parameters.TopicNamespaces = source.TopicNamespaces;
            parameters.UseEmbeddedPassword = source.UseEmbeddedPassword.Value;
            parameters.UserName = source.UserName;
            parameters.UseUTCTimestamp = true;
            parameters.VideoSourceToken = source.VideoSourceToken;

            if (!string.IsNullOrEmpty(source.Address))
            {
                // get all "own" addresses;

                if (_networkInterfaces == null)
                {
                    _networkInterfaces = Utils.NetworkUtils.GetNetworkInterfaces();
                }

                List<NetworkInterfaceDescription> nics = _networkInterfaces;

                // select required address (compare strings)
                foreach (NetworkInterfaceDescription nic in nics)
                {
                    if (nic.IP.ToString() == source.Address)
                    {
                        parameters.NetworkInterfaceController = nic;
                        break;
                    }
                }

                if (parameters.NetworkInterfaceController != null)
                {
                    // define device IP
                    bool ipv6 = (parameters.NetworkInterfaceController.IP != null) &&
                        (parameters.NetworkInterfaceController.IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
                    parameters.CameraIP = Utils.NetworkUtils.GetIP(source.DeviceIP, ipv6);
                }

            }  

            return parameters;
        }

        public void Stop()
        {
            if (_td.State == TestState.Running)
            {
                _td.Stop();
            }
        }

        List<NetworkInterfaceDescription> _networkInterfaces;


        #endregion

        #region Save results 

        StringBuilder _plainTextLog;
        TestResult _testSteps;

        string _currentTC;

        Dictionary<string, TestResult> _testResults = new Dictionary<string, TestResult>();

        #endregion

        #region DUT setting

        AutomatedTesting.DutManagement.DutManagementService.TestSoapClient _dutClient;

        List<TestCaseSettings> _currentTask;

        int _currentTestIdx;

        void EnterNextTask()
        {
            if (_currentTestIdx < _currentTask.Count)
            {            
                // setup DUT !

                TestCaseSettings settings = _currentTask[_currentTestIdx];

                _currentTC = settings.TestCaseID;

                _dutClient.LoadTestSuit(settings.FileName);

                _dutClient.SelectTestCase(settings.TestCaseID);

                _dutClient.ResetTestSuit();

                _view.BeginTest(null, _currentTC);
            }
        }

        #endregion

        #region TD events

        /// <summary>
        /// Handles "TestStarted" event.
        /// </summary>
        /// <param name="testInfo">Test description.</param>
        void _td_TestStarted(TestInfo testInfo)
        {
            _view.BeginTest(testInfo, _currentTC);

            _plainTextLog = new StringBuilder();

            _testSteps = new TestResult();
            _testSteps.Log = new TestTool.Tests.Definitions.Trace.TestLog();

            System.Diagnostics.Debug.WriteLine("Write test header: " + testInfo.Name);
            WriteLog(testInfo.Name, LogEntryLevel.Test);
            WriteLog(string.Empty, LogEntryLevel.Test);

        }

        /// <summary>
        /// Handles "TestCompleted" event.
        /// </summary>
        /// <param name="testInfo">Test description.</param>
        /// <param name="log">Test results.</param>
        void _td_TestCompleted(TestInfo testInfo, TestTool.Tests.Definitions.Trace.TestLog log)
        {
            string dutLog = _dutClient.GetTestResult().Replace("\n", Environment.NewLine);
            string dutONVIFTestExpectedResult = _dutClient.GetONVIFTestExpectedResult();

            bool isTest = testInfo.ProcessType == ProcessType.Test;

            TestResult testResult = new TestResult();
            testResult.Log = log;
            testResult.DutLog = dutLog;
            testResult.TestInfo = testInfo;
            testResult.TestResultDUT = _dutClient.GetTestSummaryResult();
            testResult.TestName = _dutClient.GetTestName().Replace("\n", Environment.NewLine);
            testResult.TestDescription = _dutClient.GetTestDescription().Replace("\n", Environment.NewLine);
            testResult.TestResultInnerException = false;
            testResult.TestExpectedResult = _dutClient.GetTestExpectedResult().Replace("\n", Environment.NewLine);
            if (dutONVIFTestExpectedResult == "NONE")
            {
                testResult.TestResultONVIFTestUnderfined = true;
                testResult.TestResultONVIFTest = false;
            }
            else
            {
                testResult.TestResultONVIFTestUnderfined = false;
                if (dutONVIFTestExpectedResult == testResult.Log.TestStatus.ToString().ToUpper())
                {
                    testResult.TestResultONVIFTest = true;
                }
                else
                {
                    testResult.TestResultONVIFTest = false;
                }
            }

            testResult.TestFinalResult = "Result from DUT: " + testResult.TestResultDUT.ToString() + Environment.NewLine;
            testResult.TestFinalResult = testResult.TestFinalResult + "Inner Exception: " + testResult.TestResultInnerException.ToString() + Environment.NewLine;
            testResult.TestFinalResult = testResult.TestFinalResult + "Expected ONVIF Test Result is not Specified: " + testResult.TestResultONVIFTestUnderfined.ToString() + Environment.NewLine;
            testResult.TestFinalResult = testResult.TestFinalResult + "Expected ONVIF Test Result: " + testResult.TestResultONVIFTest.ToString() + Environment.NewLine;
            testResult.TestFinalResult = testResult.TestFinalResult + Environment.NewLine;
            bool finalResult = testResult.TestResultDUT && !testResult.TestResultInnerException && (testResult.TestResultONVIFTest || testResult.TestResultONVIFTestUnderfined);
            testResult.TestFinalResult = testResult.TestFinalResult + "Final Result: " + finalResult.ToString() + Environment.NewLine;

            if (!string.IsNullOrEmpty(log.ErrorMessage))
            {
                string errorMessage = string.Format("   Error: {0}{1}", log.ErrorMessage, Environment.NewLine);
                WriteLog(errorMessage, LogEntryLevel.Test);
            }

            string testResultLine = string.Empty;
            testResultLine = string.Format("TEST {0}", testResult.Log.TestStatus.ToString().ToUpper());

            WriteLog(testResultLine, LogEntryLevel.Test);

            testResult.PlainTextLog = _plainTextLog.ToString();
                                   
            _testResults[_currentTC] = testResult;

            TestCaseStatus status;
            // toDo: define status
            if (finalResult)
            {
                if (testResult.TestResultONVIFTestUnderfined)
                {
                    status = TestCaseStatus.Yellow;
                }
                else
                {
                    status = TestCaseStatus.Green;
                }
            }
            else
            {
                status = TestCaseStatus.Red;
            }
            testResult.Status = status;
            _view.DisplayTestResult(_currentTC, status);

            // move to next
            
            _currentTestIdx++;
            EnterNextTask();

        
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

            _testSteps.Log.Steps.Add(result);

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
        /// Creates log entry and appends to log where it is necessary
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="level"></param>
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

            DisplayLogLine(line);
        }

        /// <summary>
        /// Adds step
        /// </summary>
        /// <param name="result"></param>
        void DisplayStepResult(StepResult result)
        {
            _view.DisplayStepResult(result);
        }


        /// <summary>
        /// Udates test log
        /// </summary>
        /// <param name="line"></param>
        void DisplayLogLine(string line)
        {
            _view.WriteLine(line);
        }

        #endregion


        /// <summary>
        /// Gets test results. 
        /// </summary>
        /// <param name="testInfo">Test information.</param>
        /// <returns>Test result if test has been executed; otherwise null.</returns>
        public TestResult GetTestResult(string testCaseId)
        {
            if (_testResults.ContainsKey(testCaseId))
            {
                return _testResults[testCaseId];
            }
            else
            {
                return null;
            }
        }


        public void ExportFailedTests(string fileName)
        {
            TestSuite export = new TestSuite();
            export.Tests = new List<Test>();

            List<string> failedCasese = 
                _testResults.Where(res => res.Value.Status == TestCaseStatus.Red).Select(res => res.Key).ToList();

            foreach (TestSuite ts in _testSuites)
            {
                foreach (Test test in ts.Tests)
                {
                    Test copy = new Test()
                    {
                        Category = test.Category,
                        DefaultFileName = test.DefaultFileName,
                        TestID = test.TestID,
                        TestCases = new List<TestCase>()
                    };

                    foreach (TestCase testCase in test.TestCases)
                    {
                        if (failedCasese.Contains(testCase.TestCaseID))
                        {
                            copy.TestCases.Add(testCase);
                        }
                    }

                    if (copy.TestCases.Count > 0)
                    {
                        export.Tests.Add(copy);
                    }
                }
            }

            foreach (DutTest ts in _dutTests)
            {
                Test copy = new Test()
                {
                    Category = ts.Category,
                    DefaultFileName = ts.FileName,
                    TestID = ts.TestID,
                    TestCases = new List<TestCase>()
                };

                foreach (DutTestCase testCase in ts.Tests)
                {
                    if (failedCasese.Contains(testCase.TestCaseID))
                    {
                        TestCase tcCopy = new TestCase();
                        tcCopy.FileName = ts.FileName;
                        tcCopy.TestCaseID = testCase.TestCaseID;
                        copy.TestCases.Add(tcCopy);
                    }
                }

                if (copy.TestCases.Count > 0)
                {
                    export.Tests.Add(copy);
                }
            }

            XmlSerializer ser = new XmlSerializer(typeof(TestSuite));
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            FileStream fs = File.OpenWrite(fileName);
            ser.Serialize(fs, export);
            fs.Close();
        }
    }
}
