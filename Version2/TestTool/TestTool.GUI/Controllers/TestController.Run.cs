using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.GUI.Data;
using TestTool.GUI.Enums;
using TestTool.GUI.Utils.Profiles;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine;

namespace TestTool.GUI.Controllers
{
    partial class TestController
    {

        #region fields
        /// <summary>
        /// Quantity of test cases to be launched
        /// </summary>
        private int _testsCount;
        
        /// <summary>
        /// Object responsible for running tests.
        /// </summary>
        protected TestDispatcher _td;

        #endregion
        
        #region RUN

        /// <summary>
        /// Launches test suite.
        /// </summary>
        /// <param name="parameters">Test suite parameters.</param>
        public void Run(TestSuiteParameters parameters)
        {
            if (View.ProfilesView != null)
            {
                View.ProfilesView.ClearProfiles();
            }
            RunDiagnostic(parameters, false);
        }

        /// <summary>
        /// Launches single test.
        /// </summary>
        /// <param name="parameters">Test suite parameters.</param>
        public void RunSingle(TestSuiteParameters parameters)
        {
            if (View.ProfilesView != null)
            {
                View.ProfilesView.ClearProfiles();
            }
            RunDiagnostic(parameters, true);
        }

        /// <summary>
        /// Runs tests _as_ in conformance mode
        /// </summary>
        public void RunAll()
        {
            _td.ResetFeatures();
            Clear(true);
            if (View.TestTreeView != null)
            {
                View.TestTreeView.SelectTests(new TestInfo[0]);
            }

            TestSuiteParameters parameters = GetParameters();
            parameters.TestCases = View.TestTreeView.SelectedTests;
            parameters.DefineProfiles = true;
            RunDiagnostic(parameters, false);
        }

        /// <summary>
        /// Launches features definition process
        /// </summary>
        /// <param name="parameters"></param>
        public void DefineFeatures(TestSuiteParameters parameters)
        {
            parameters.FeatureDefinition = FeatureDefinitionMode.Define;

            RunDiagnostic(parameters, true);
        }
        
        /// <summary>
        /// Runs process in diagnostic mode (testing or feature definition).
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="single"></param>
        void RunDiagnostic(TestSuiteParameters parameters, bool single)
        {
            AddTestCases(parameters, false);
            parameters.Profiles.AddRange(_onvifProfiles);
            _runningSingle = single;
            _testsCount = parameters.TestCases.Count;
            parameters.Operator = new Utils.Operator(View.Window);
            if (_td.FeaturesDefined)
            {
                parameters.Features.AddRange(_td.Features);
                parameters.Features.AddRange(_td.UndefinedFeatures);
            }

            InternalRun(parameters, single, false);
        }

        /// <summary>
        /// Runs conformance for Conformance tab
        /// </summary>
        public void RunConformance()
        {
            _td.ResetFeatures();
            Clear(true);
            View.TestTreeView.SelectTests(new TestInfo[0]);

            TestSuiteParameters parameters = GetParameters();
            AddTestCases(parameters);
            parameters.DefineProfiles = true;
            parameters.Profiles.AddRange(_onvifProfiles);
            parameters.Conformance = true;
            _runningSingle = false;

            InternalRun(parameters, false, true);
        }

        /// <summary>
        /// Runs conformance in silent mode
        /// </summary>
        /// <param name="parameters"></param>
        public void RunConformanceSilent(TestSuiteParameters parameters)
        {
            _td.ResetFeatures();
            Clear(true);

            AddTestCases(parameters);
            parameters.Operator = new SilentProcessing.SilentOperator();
            parameters.VideoForm = View.GetVideoForm();
            parameters.DefineProfiles = true;
            parameters.Profiles.AddRange(_onvifProfiles);
            parameters.Conformance = true;
            _runningSingle = false;

            InternalRun(parameters, false, true);
        }
        
        public void AddTestCases(TestSuiteParameters parameters, bool clear = true)
        {
            if (clear) parameters.AllTestCases.Clear();

            parameters.AllTestCases.AddRange(_testInfos.OrderBy(TI => TI.ExecutionOrder).ThenBy(T => T.Category).ThenBy(t => t.Order));
        }
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

            DeviceEnvironment environment = ContextController.GetDeviceEnvironment();
            parameters.UserName = environment.Credentials.UserName;
            parameters.Password = environment.Credentials.Password;
            parameters.UseUTCTimestamp = environment.Credentials.UseUTCTimeStamp;

            parameters.MessageTimeout = environment.Timeouts.Message;
            parameters.RebootTimeout = environment.Timeouts.Reboot;
            parameters.TimeBetweenTests = environment.Timeouts.InterTests;
            parameters.OperationDelay = environment.TestSettings.OperationDelay;
            parameters.RecoveryDelay = environment.TestSettings.RecoveryDelay;
            parameters.RetentionTime = environment.TestSettings.RetentionTime;

            parameters.RepeatTests = View.Repeat;
            
            parameters.EnvironmentSettings = new Tests.Common.TestEngine.EnvironmentSettings()
            {
                DnsIpv4 = environment.EnvironmentSettings.DnsIpv4,
                NtpIpv4 = environment.EnvironmentSettings.NtpIpv4,
                DnsIpv6 = environment.EnvironmentSettings.DnsIpv6,
                NtpIpv6 = environment.EnvironmentSettings.NtpIpv6,
                DefaultGateway = environment.EnvironmentSettings.GatewayIpv4,
                DefaultGatewayIpv6 = environment.EnvironmentSettings.GatewayIpv6
            };

            parameters.PTZNodeToken = environment.TestSettings.PTZNodeToken;
            parameters.UseEmbeddedPassword = environment.TestSettings.UseEmbeddedPassword;
            parameters.Password1 = environment.TestSettings.Password1;
            parameters.Password2 = environment.TestSettings.Password2;
            parameters.SecureMethod = environment.TestSettings.SecureMethod;
            parameters.VideoSourceToken = environment.TestSettings.VideoSourceToken;
            parameters.FirmwareFilePath = environment.TestSettings.FirmwareFilePath;
            parameters.CredentialIdentifierValueFirst = environment.TestSettings.CredentialIdentifierValueFirst;
            parameters.CredentialIdentifierValueSecond = environment.TestSettings.CredentialIdentifierValueSecond;
            parameters.CredentialIdentifierValueThird = environment.TestSettings.CredentialIdentifierValueThird;
            
            parameters.EventTopic = environment.TestSettings.EventTopic;
            parameters.SubscriptionTimeout = environment.TestSettings.SubscriptionTimeout;
            parameters.TopicNamespaces = environment.TestSettings.TopicNamespaces;
            parameters.RelayOutputDelayTimeMonostable = environment.TestSettings.RelayOutputDelayTimeMonostable;

            parameters.RecordingToken = environment.TestSettings.RecordingToken;
            parameters.SearchTimeout = environment.TestSettings.SearchTimeout;
            parameters.MetadataFilter = environment.TestSettings.MetadataFilter;
            
            Dictionary<string, object> advanced = new Dictionary<string, object>();
            foreach (object o in environment.TestSettings.AdvancedSettings)
            {
                advanced.Add(o.GetType().GUID.ToString(), o);
            }
            parameters.AdvancedParameters = advanced;

            parameters.VideoForm = View.GetVideoForm();
            parameters.Operator = new Utils.Operator(View.Window);

            return parameters;
        }

        #endregion
        
        #region RUN EVENTS

        /// <summary>
        /// Handles "Paused" event.
        /// </summary>
        void _td_Paused()
        {
            View.SwitchToState(ApplicationState.TestPaused);
        }

        /// <summary>
        /// Displays feature support
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="supported"></param>
        void _td_FeatureDefined(Feature feature, bool supported)
        {
            if (View.FeaturesView != null)
            {
                View.FeaturesView.DisplayFeature(feature, supported);
            }
        }

        /// <summary>
        /// Displays "undefined" feature
        /// </summary>
        /// <param name="feature"></param>
        void _td_FeatureDefinitionFailed(Feature feature)
        {
            if (View.FeaturesView != null)
            {
                View.FeaturesView.DisplayUndefinedFeature(feature);
            }
        }

        void _td_InitializationCompleted(ConformanceInitializationData data)
        {
            _testsSupported = data.TestsSelected;
            _initializationData = data;

            if (data.Continue && View.TestTreeView != null)
            {
                View.TestTreeView.SelectTests(data.TestsSelected);
            }
            if (ConformanceInitializationCompleted != null)
            {
                ConformanceInitializationCompleted(data);
            }

            // only for test controller
            DisplayFunctionalitySupport(data.TestsSelected);
            if (View.ProfilesView != null)
            {
                View.ProfilesView.DisplayProfiles(data.SupportedProfiles, data.FailedProfiles, data.UnsupportedProfiles);
            }
        }
        
        void _td_DeviceInformationReceived(DeviceInformation data)
        {
            if (_conformance && DeviceInfoReceived != null)
            {
                DeviceInfo info = new DeviceInfo();
                info.FirmwareVersion = data.FirmwareVersion;
                info.HardwareID = data.HardwareID;
                info.Manufacturer = data.Manufacturer;
                info.Model = data.Model;
                info.SerialNumber = data.SerialNumber;

                DeviceInfoReceived(info);
            }
        }

        void _td_ProfileDefinitionCompleted(IProfileDefinition profile, ProfileStatus status, string dump)
        {
            ProfileTestInfo info = new ProfileTestInfo();
            info.Status = status;
            info.Log = dump;
            if (_profilesSupportInfo.ContainsKey(profile))
            {
                _profilesSupportInfo[profile] = info;
            }
            else
            {
                _profilesSupportInfo.Add(profile, info);
            }
        }

        /// <summary>
        /// Handles "NetworkSettingsChangedEvent" event.
        /// </summary>
        /// <param name="newServiceAddress">New service address.</param>
        void _td_NetworkSettingsChanged(string newServiceAddress)
        {
            if (NetworkSettingsChangedEvent != null)
            {
                NetworkSettingsChangedEvent(newServiceAddress);
            }
        }

        private void TdOnSecurityChangedEvent(CredentialsProvider credentialsProvider)
        {
            if (null != SecurityChangedEvent)
                SecurityChangedEvent(credentialsProvider);
        }
        /// <summary>
        /// Handles "TestSuiteCompleted" event.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="bCompletedNormally">Indicates whether test suite has not been halted.</param>
        void _td_TestSuiteCompleted(TestSuiteParameters parameters, bool bCompletedNormally)
        {
            if (bCompletedNormally && !_conformance)
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
            
            View.BeginTest(testInfo);
            if (_scrollingEnabled)
            {
                View.TestResultView.BeginTest();
            }

            _plainTextLog = new StringBuilder();
            _shortTestLog = new StringBuilder();
            _warningsLog = new List<string>();
            _testSteps = new TestResult();
            _testSteps.Log = new Tests.Definitions.Trace.TestLog();
            
            System.Diagnostics.Debug.WriteLine("Write test header: " + testInfo.Name);
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

            bool overwriteResult = /*(_runningSingle || View.Repeat) &&*/ _testResults.ContainsKey(testInfo);

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
                    testResultLine = "PROCESS COMPLETED";
                }
            }
            WriteLog(testResultLine, LogEntryLevel.Test);

            testResult.PlainTextLog = _plainTextLog.ToString();
            testResult.ShortTextLog = _shortTestLog.ToString();
            if (_warningsLog.Count > 0)
            {
                testResult.Warnings = new List<string>();
                testResult.Warnings.AddRange(_warningsLog);
            }

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
                View.EndTest(_testResults[testInfo]);
            }
            else
            {
                _featureDefinitionLog = testResult;
                View.EndTest(_featureDefinitionLog);
            }
            
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

        #endregion
        
        #region START/STOP/STATE

        /// <summary>
        /// Launches tests.
        /// </summary>
        /// <param name="parameters">Parameters passed from the View.</param>
        /// <param name="single">True if "Run current" button is in use. If "Run current" button has 
        /// been clicked, results collected by this moment will not be cleared.</param>
        /// <param name="conformance"></param>
        protected void InternalRun(TestSuiteParameters parameters, bool single, bool conformance)
        {
            _running = true;
            EnableScrolling(true); 

            if (!single)
            {
                _testResults.Clear();
                _testExecutionTime = DateTime.Now;
            }

            if (parameters.FeatureDefinition == FeatureDefinitionMode.Define)
            {
                _testResults.Clear();
            }
            _conformance = conformance;
            _runningSingle = single;

            ReportTestSuiteStarted(parameters, conformance);

            if (parameters.FeatureDefinition == FeatureDefinitionMode.Define)
            {
                _td.RequestFeatures(parameters);
            }
            else
            {
                _td.Run(parameters);
            }

        }
        
        void CompleteTestSuite(TestSuiteParameters parameters, bool bCompletedNormally)
        {
            if (bCompletedNormally)
            {
                if (parameters.DefineProfiles)
                {
                    Dictionary<TestInfo, TestStatus> results =
                        new Dictionary<TestInfo, TestStatus>();
                    foreach (TestInfo info in _testResults.Keys)
                    {
                        results.Add(info, _testResults[info].Log.TestStatus);
                    }

                    if (View.ProfilesView != null)
                    {

                        Dictionary<Functionality, bool> functionalities = ProfilesSupportInfo.CheckTestResults(results);
                        View.ProfilesView.DisplaySupportedFunctionality(functionalities);
                    }

                    bool failed = _testResults.Values.Where(TR => TR.Log.TestStatus == TestStatus.Failed).Count() > 0;
                    //

                    List<IProfileDefinition> failedProfiles = new List<IProfileDefinition>();
                    List<IProfileDefinition> supportedProfiles = new List<IProfileDefinition>();

                    if (_initializationData.FailedProfiles.Count != 0 && !failed)
                    {
                        failedProfiles.AddRange(_initializationData.FailedProfiles);
                    }
                    else if (failed)
                    {
                        failedProfiles.AddRange(_initializationData.FailedProfiles);
                        failedProfiles.AddRange(_initializationData.SupportedProfiles);
                    }
                    else
                    {
                        supportedProfiles.AddRange(_initializationData.SupportedProfiles);
                    }

                    if (View.ProfilesView != null)
                    {
                        View.ProfilesView.DisplayProfiles(supportedProfiles, failedProfiles,
                                                          _initializationData.UnsupportedProfiles);
                    }
                }

                UpdateTestLog();
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

    }
}
