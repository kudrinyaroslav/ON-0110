///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.Common.Trace;
using System.Linq;
using TestTool.Tests.Engine.Data;

namespace TestTool.Tests.Engine
{

    /// <summary>
    /// Runs sequence of tests. Passes test events (step started, step completed, request 
    /// sent etc) with its own events.
    /// Pause, Resume, Stop, Halt functionality is implemented also here.
    /// </summary>
    public class TestDispatcher
    {
        /// <summary>
        /// Bunch of parameters required for test run.
        /// </summary>
        private TestSuiteParameters _parameters;

        private object[] args = null;

        /// <summary>
        /// Test currently running.
        /// </summary>
        private ITest _currentTest;

        /// <summary>
        /// Current test information.
        /// </summary>
        private TestInfo _currentTestInfo;

        public TestInfo CurrentTest
        {
            get { return _currentTestInfo; }
        }

        #region Events

        /// <summary>
        /// Raised when network settings changed.
        /// </summary>
        public event NetworkSettingsChanged NetworkSettingsChangedEvent;

        /// <summary>
        /// Raised when credentials changed.
        /// </summary>
        public event SecurityChanged SecurityChangedEvent;

        /// <summary>
        /// Is raised when a test is started.
        /// </summary>
        public event Action<TestInfo> TestStarted;

        public delegate void TestCompletedHandler(TestInfo testInfo, TestLog log);

        /// <summary>
        /// Is raised when a test is completed.
        /// </summary>
        public event TestCompletedHandler TestCompleted;
        
        /// <summary>
        /// Is raised when a step in test is started.
        /// </summary>
        public event Action<StepResult> StepStarted;
        
        /// <summary>
        /// Is raised when a step in test is completed.
        /// </summary>
        public event StepCompleted StepCompleted;

        /// <summary>
        /// Is raised when a test reports that a request is sent.
        /// </summary>
        public event Action<string> RequestSent;
        
        /// <summary>
        /// Is raised when a test reports that a response is received.
        /// </summary>
        public event Action<string> ResponseReceived;

        /// <summary>
        /// Is raised when step-level notification occurs.
        /// </summary>
        public event Action<string> StepEvent;

        /// <summary>
        /// Is raised when test-level notification occurs.
        /// </summary>
        public event Action<string> TestEvent; 

        /// <summary>
        /// IS raised when an exception occurred.
        /// </summary>
        public event Action<Exception> OnException;

        /// <summary>
        /// Is raised when all tests are completed.
        /// </summary>
        public event Action<TestSuiteParameters, bool> TestSuiteCompleted;

        /// <summary>
        /// Is raised when test is paused (accordingly to the specification, test is paused NOT in the
        /// moment when the button is pressed, but at safe point).
        /// </summary>
        public event Action Paused;

        /// <summary>
        /// Is raised when a feature is defined
        /// </summary>
        public event Action<Feature, bool> FeatureDefined;

        public event Action<Feature> FeatureDefinitionFailed;
        
        /// <summary>
        /// 
        /// </summary>
        public delegate void ProfileDefinitionCompletedHandler(IProfileDefinition profile, ProfileStatus status, string dump);

        public event ProfileDefinitionCompletedHandler ProfileDefinitionCompleted;
        
        public event Action<DeviceInformation> DeviceInformationReceived;
        
        /// <summary>
        /// Is raised when test cases for conformance testing are defined
        /// </summary>
        public event Action<ConformanceInitializationData> InitializationCompleted;

        #endregion

        #region Features

        private List<Feature> _features = new List<Feature>();
        private List<Feature> _undefinedFeatures = new List<Feature>();
        private List<Feature> _unsupportedFeatures = new List<Feature>();
        
        private List<string> _scopes = new List<string>();

        public List<Feature> Features
        {
            get { return _features; }
        }

        public List<Feature> UndefinedFeatures
        {
            get { return _undefinedFeatures; }
        }

        public List<Feature> UnsupportedFeatures
        {
            get { return _unsupportedFeatures; }
        }

        public List<string> Scopes
        {
            get { return _scopes; }
        }

        private bool _featuresDefined = false;
        public bool FeaturesDefined
        {
            get { return _featuresDefined; }
        }

        public void ResetFeatures()
        {
            _features.Clear();
            _undefinedFeatures.Clear();
            _unsupportedFeatures.Clear();
            _scopes.Clear();
            _supportedProfiles.Clear();
            _unsupportedProfiles.Clear();
            _failedProfiles.Clear();
           
            _featuresDefined = false;
        }

        public void RequestFeatures(TestSuiteParameters parameters)
        {
            parameters.TestCases.Clear();
            parameters.TestCases.Add(FeaturesDefinitionProcess.This);
            Run(parameters);
        }

        private DeviceInformation _deviceInformation;
        public DeviceInformation DeviceInformation
        {
            get { return _deviceInformation; }
        }

        #endregion


        #region Profiles

        private List<IProfileDefinition> _supportedProfiles = new List<IProfileDefinition>();
        private List<IProfileDefinition> _unsupportedProfiles = new List<IProfileDefinition>();
        private List<IProfileDefinition> _failedProfiles = new List<IProfileDefinition>();

        public int MaxPullPoints { get; protected set; }
        public int MaximumNumberOfKeys { get; protected set; }
        public int MaximumNumberOfCertificates { get; protected set; }

        #endregion

        #region Run/Pause/Stop

        ///
        /// The following situations are considered:
        ///  - "Pause" is clicked when a test is running - pause/resume/halt etc is handled at the test level.
        ///  - "Pause" is clicked when test initialization is being performed, or in the pause between tests - 
        ///    than pause is handled at the dispatcher level.
        /// 
        ///  "Stop" is always handled at the dispatcher level ("Stop" button stops execution at the end of test).
        /// 
        /// 
        /// 
        /// 

        /// <summary>
        /// Event to resume test running.
        /// </summary>
        ManualResetEvent _resumeEvent = new ManualResetEvent(false);
        
        /// <summary>
        /// Event to stop test execution immediately in the pause between tests or in the pause initiated
        /// by clicking UI button.
        /// </summary>
        ManualResetEvent _haltEvent = new ManualResetEvent(false);

        /// <summary>
        /// Flag indicating whether no tests should be performed after current is completed.
        /// </summary>
        private bool _stop;

        private object _pauseSync = new object();

        /// <summary>
        /// True if a pause is being handled at the dispatcher level.
        /// </summary>
        private bool _dispatcherLevelPause;

        /// <summary>
        /// True if Halt button is pressed when no test is running.
        /// </summary>
        private bool _delayedHalt;

        /// <summary>
        /// True if shutdown is in progress.
        /// </summary>
        private bool _shutDownInProgress = false;

        /// <summary>
        /// Current state.
        /// </summary>
        private TestState _state;
        
        /// <summary>
        /// Current state.
        /// </summary>
        public TestState State
        {
            get { return _state; }
        }

        /// <summary>
        /// Sets the flag indicating that no more tests should be run.
        /// </summary>
        public void Stop()
        {
            lock (_pauseSync)
            {
                _stop = true;
            }
        }
        
        /// <summary>
        /// Pause current test (between steps) or handle pause at the dispatcher level.
        /// </summary>
        public void Pause()
        {
            _state = TestState.Paused;
            if (_currentTest != null)
            {
                System.Diagnostics.Debug.WriteLine("Pause running test");
                _currentTest.Pause();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Pause Test dispatcher");
                if (Paused != null)
                {
                    Paused();
                }
                lock (_pauseSync)
                {
                    _dispatcherLevelPause = true;
                }
            }
        }

        /// <summary>
        /// Resume current test or test dispatcher.
        /// </summary>
        public void Resume()
        {
            _state = TestState.Running;
            bool dispatcherLevelPause;
            lock (_pauseSync)
            {
                dispatcherLevelPause = _dispatcherLevelPause;
            }

            if (dispatcherLevelPause)
            {
                System.Diagnostics.Debug.WriteLine("Dispatcher pause - set Resume event");
                _resumeEvent.Set();
            }
            else
            {
                if (_currentTest != null)
                    _currentTest.Resume();
            }
        }

        /// <summary>
        /// Stops tests execution immediately, or stops test dispatcher.
        /// </summary>
        /// <remarks>There sill can be some delay between pressing "Halt" and real stop, but all 
        /// potentially long actions - waiting for DUT answer or pause between tests - are 
        /// interrupted immediately.</remarks>
        public void Halt()
        {
            _stop = true;
            if (_currentTest != null)
            {
                System.Diagnostics.Debug.WriteLine("TestDispatcher::Halt - stop running test");
                _currentTest.Halt();
            }
            //else
            {
                _haltEvent.Set();
                lock (_pauseSync)
                {
                    _delayedHalt = true;
                }
            }
        }

        /// <summary>
        /// Shutdown. Current test is stopped, and no events are raised after Shutdown.
        /// </summary>
        public void Shutdown()
        {
            System.Diagnostics.Debug.WriteLine("Shutdown");
            _shutDownInProgress = true;
            Halt();
        }

        #endregion

        /// <summary>
        /// Runs tests passed in separate thread.
        /// </summary>
        /// <param name="parameters"></param>
        public void Run(TestSuiteParameters parameters)
        {
            _parameters = parameters;
            lock (_pauseSync)
            {
                _stop = false;
                _delayedHalt = false;
                _dispatcherLevelPause = false;
            }

            _state = TestState.Running;

            Thread thread = new Thread(RunTests);
            thread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            thread.Start();
        }
        
        /// <summary>
        /// Method for running tests in the background.
        /// </summary>
        void RunTests()
        {
            TrafficListener.ResetStatistics();
            System.Diagnostics.Debug.WriteLine("RunTests started");
            //_haltEvent.Reset();
            // types description for test class constructor
            Type[] types = new Type[] { typeof(TestLaunchParam) };

            // parameters as defined in TestEngine.
            TestLaunchParam param = new TestLaunchParam();
            param.FirmwareFilePath = _parameters.FirmwareFilePath;
            param.CredentialIdentifierValueFirst = _parameters.CredentialIdentifierValueFirst;
            param.CredentialIdentifierValueSecond = _parameters.CredentialIdentifierValueSecond;
            param.CredentialIdentifierValueThird = _parameters.CredentialIdentifierValueThird;
            param.ServiceAddress = _parameters.Address;
            param.CameraIp = _parameters.CameraIP;
            param.CameraUUID = _parameters.CameraUUID;
            param.NIC = _parameters.NetworkInterfaceController;
            param.MessageTimeout = _parameters.MessageTimeout;
            param.RebootTimeout = _parameters.RebootTimeout;
            param.UserName = _parameters.UserName;
            param.Password = _parameters.Password;
            param.UseUTCTimestamp = _parameters.UseUTCTimestamp;
            param.Operator = _parameters.Operator;
            param.VideoForm = _parameters.VideoForm;
            param.EnvironmentSettings = _parameters.EnvironmentSettings;
            param.PTZNodeToken = _parameters.PTZNodeToken;
            param.UseEmbeddedPassword = _parameters.UseEmbeddedPassword;
            param.Password1 = _parameters.Password1;
            param.Password2 = _parameters.Password2;
            param.OperationDelay = _parameters.OperationDelay;
            param.RecoveryDelay = _parameters.RecoveryDelay;
            param.SecureMethod = _parameters.SecureMethod;
            param.SubscriptionTimeout = _parameters.SubscriptionTimeout;
            param.EventTopic = _parameters.EventTopic;
            param.TopicNamespaces = _parameters.TopicNamespaces;

            param.RelayOutputDelayTimeMonostable = _parameters.RelayOutputDelayTimeMonostable;
            param.RecordingToken = _parameters.RecordingToken;
            param.SearchTimeout = _parameters.SearchTimeout;
            param.MetadataFilter = _parameters.MetadataFilter;
            param.RetentionTime = _parameters.RetentionTime;

            param.AdvancedPrameters = _parameters.AdvancedParameters;

            // parameters for constructor.
            args = new object[] { param };

            param.DeclaredScopes.AddRange(_scopes);

            ExecutableTestList processes = _parameters.TestCases;
            bool featuresDefined = _featuresDefined;
            switch (_parameters.FeatureDefinition)
            {
                case FeatureDefinitionMode.Default:
                    param.Features.AddRange(_features);
                    break;
                case FeatureDefinitionMode.AssumeSupported:
                    featuresDefined = true;
                    param.FeatureDefinitionMode = Base.Definitions.FeatureDefinitionMode.AllSupported;
                    break;
                case FeatureDefinitionMode.AssumeNotSupported:
                    param.FeatureDefinitionMode = Base.Definitions.FeatureDefinitionMode.AllNotSupported;
                    featuresDefined = true;
                    break;
            }

            if (!featuresDefined)
            {
                if (!processes.Contains(FeaturesDefinitionProcess.This))
                    processes.Add(FeaturesDefinitionProcess.This);

                System.Diagnostics.Debug.WriteLine("SECURITY: NONE");
                param.Security = Security.None; 
            }
            else
            {
                if (processes.Contains(FeaturesDefinitionProcess.This))
                    processes.Remove(FeaturesDefinitionProcess.This);

                // if "Assume all supported", Digest also is assumed to be supported !!!
                if (_features.Contains(Feature.Digest) || _parameters.FeatureDefinition == Tests.Engine.FeatureDefinitionMode.AssumeSupported)
                {
                    System.Diagnostics.Debug.WriteLine("SECURITY: DIGEST");
                    param.Security = Security.Digest; 
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("SECURITY: WS-USERNAME");
                    param.Security = Security.WS; 
                }
                //
                // we HAVE features => Controller is notified already about profile.
                // so, if we just run some tests - there is no reason to colour profiles tree
                // or select tests in test tree.
                //
                bool defineTests = (processes.Count == 1 && processes.Contains(FeaturesDefinitionProcess.This)) && _parameters.FeatureDefinition != FeatureDefinitionMode.Define;
                if (defineTests)
                {
                    List<Feature> features = new List<Feature>();
                    features.AddRange(_features);
                    features.AddRange(_undefinedFeatures);

                    // define test cases via features detected
                    List<TestInfo> tests = ConformanceLogicHandler.GetTestsByFeatures(_parameters.AllTestCases, features, _parameters.Conformance);
                    // no tests in list, not a feature definition process - add tests to list
                    _parameters.TestCases.AddRange(tests);
                    // notify anyway (?)
                    // - really, notify if not notified previously...

                    ReportInitializationCompleted(tests, defineTests);
                }
            }
//            if (_parameters.TestCases.Count > 0)
//            {
//                // if this is a request to run tests, not only feature definition
//                if (!(_parameters.TestCases.Count == 1 && _parameters.TestCases.First().ProcessType == ProcessType.FeatureDefinition ))
//                {
//                    processes.AddRange(_parameters.TestCases);
//                }
//            }

            var bCompletedNormally = RunTestExecutionLoop(processes, types, param);

            // all tests are performed.
            _state = TestState.Idle;

            if (TestSuiteCompleted != null && !_shutDownInProgress)
            {
                TestSuiteCompleted(_parameters, bCompletedNormally);
            }
            System.Diagnostics.Debug.WriteLine("RunTests stopped");
/*            
            int Max;
            int Median;
            int Mean;
            TrafficListener.GetAggregated(out Max, out Median, out Mean);
            System.Diagnostics.Debug.WriteLine(string.Format("RunTests stopped with Max={0}, Median={1} and Mean={2}", Max, Median, Mean));
*/ 
        }

        private bool RunTestExecutionLoop(ExecutableTestList processes, Type[] types, TestLaunchParam param)
        {
            bool bCompletedNormally = false;
            try
            {
                // Go through the list of tests...
                //for (int i = 0; i < processes.Count; i++)
                bool breakFlag = true;
                do
                {
                    breakFlag = true;
                    processes.StartExecution();
                    IEnumerator<TestInfo> curr = processes.GetUpdatableEnumerator();
                    var processNextFlag = curr.MoveNext();
                    for (; processNextFlag;)
                    {
                        //TestInfo testInfo = processes[i];
                        var testInfo = curr.Current;
                        _currentTestInfo = testInfo;

                        /// "Stop" requested or "Halt" has not been handled at test level.
                        if (_stop)
                        {
                            System.Diagnostics.Debug.WriteLine("TEST SEQUENCE STOPPED");
                            bCompletedNormally = false;
                            break;
                        }

                        try
                        {
                            // Check if a pause should be done.
                            // If a pause should be done at this point and "Halt" is clicked 
                            // during the pause = don't execute next test.
                            bool dispatcherLevelPause;
                            lock (_pauseSync)
                            {
                                dispatcherLevelPause = _dispatcherLevelPause;
                            }

                            if (dispatcherLevelPause)
                            {
                                // Sleep() returns TRUE if pause has been ended by clicking "Resume" and FALSE if "Halt"
                                // button has been clicked.
                                bool bContinue = Sleep();
                                if (!bContinue)
                                {
                                    System.Diagnostics.Debug.WriteLine("TEST SEQUENCE PAUSED");
                                    // Tests execution halted
                                    bCompletedNormally = false;
                                    break;
                                }
                            }

                            System.Diagnostics.Debug.WriteLine("TEST SEQUENCE before TestStarted");
                            // Report that a test is started
                            if (TestStarted != null)
                            {
                                TestStarted(testInfo);
                            }
                            System.Diagnostics.Debug.WriteLine("TEST SEQUENCE TestStarted");

                            _currentTest = null;
                            BaseTest test = InitCurrentTest(testInfo, types, args);
                            System.Diagnostics.Debug.WriteLine("TEST SEQUENCE InitCurrentTest");

                            // check if tests execution should be stopped
                            bool halt;
                            lock (_pauseSync)
                            {
                                halt = _delayedHalt || _stop;
                            }

                            if (halt)
                            {
                                bCompletedNormally = false;
                                System.Diagnostics.Debug.WriteLine("Handle HALT after test started");
                                break;
                            }

                            lock (_pauseSync)
                            {
                                dispatcherLevelPause = _dispatcherLevelPause;
                            }

                            if (dispatcherLevelPause)
                            {
                                // WAIT
                                bool bContinue = Sleep();
                                if (!bContinue)
                                {
                                    System.Diagnostics.Debug.WriteLine("TEST SEQUENCE PAUSED2");
                                    bCompletedNormally = false;
                                    break;
                                }
                            }

                            try
                            {
                                // start current test.
                                // (really it means that _currentTest.EntryPoint method is executed synchronously)
                                System.Diagnostics.Debug.WriteLine("TEST SEQUENCE before Start");
                                if (_currentTest == null)
                                {
                                    System.Diagnostics.Debug.WriteLine("TEST SEQUENCE Invalid Start entry point");
                                }
                                else
                                {
                                    _currentTest.Start();
                                }
                                System.Diagnostics.Debug.WriteLine("TEST SEQUENCE after Start");

                                //
                                // Feature definition process ended
                                //
                                if (testInfo.ProcessType == ProcessType.FeatureDefinition)
                                {
                                    if (test.Halted)
                                    {
                                        System.Diagnostics.Debug.WriteLine("TEST SEQUENCE HALTED");
                                        break;
                                    }
                                    else
                                    {
                                        ProfilesSupportTest pst = new ProfilesSupportTest();
                                        pst.ProfileDefinitionCompleted += pst_ProfileDefinitionCompleted;

                                        var parameters = new Dictionary<string, object>();
                                        parameters.Add("MaxPullPoints", MaxPullPoints);
                                        parameters.Add("MaximumNumberOfKeys", MaximumNumberOfKeys);
                                        parameters.Add("MaximumNumberOfCertificates", MaximumNumberOfCertificates);

                                        pst.CheckProfiles(_parameters.Profiles, _features, _scopes, parameters);

                                        //
                                        // if we run only one/several defined test (not Feature definition process, not 0 tests to run), 
                                        // we'll have to display profiles support
                                        // And if profiles support have been displayed already (features defined), there is no need
                                        // to notify TestController at all
                                        //

                                        bool defineTests = (processes.Count == 1 &&
                                                            processes.Contains(FeaturesDefinitionProcess.This)) &&
                                                           _parameters.FeatureDefinition != FeatureDefinitionMode.Define;
                                        if (!_featuresDefined || defineTests)
                                        {
                                            InitConformanceTesting(param);

                                            _featuresDefined = true;

                                            List<TestInfo> tests =
                                                ConformanceLogicHandler.GetTestsByFeatures(_parameters.AllTestCases,
                                                                                           param.Features,
                                                                                           _parameters.Conformance);
                                            if (defineTests)
                                            {
                                                // define test cases via features detected
                                                processes.AddRange(tests);
                                            }
                                            ReportInitializationCompleted(tests, defineTests);
                                        }
                                    }
                                }
                            }
                            finally
                            {
                                // pause between tests. If "Halt" is clicked during the pause - exit tests execution.
                                //if (current != processes.Count)
                                if (processNextFlag = curr.MoveNext())
                                {
                                    _currentTest = null;

                                    int hndl = WaitHandle.WaitAny(new WaitHandle[] {_haltEvent},
                                                                  _parameters.TimeBetweenTests);

                                    _haltEvent.Reset();
                                    lock (_pauseSync)
                                    {
                                        halt = _delayedHalt || _stop;
                                    }
                                    if (hndl == 0 || halt)
                                    {
                                        System.Diagnostics.Debug.WriteLine("TEST SEQUENCE HALTED2");
                                        bCompletedNormally = false;
                                        processNextFlag = false;
                                    }
                                }
                                else
                                {
                                    if (_parameters.RepeatTests)
                                    {
                                        breakFlag = false;
                                    }
                                    bCompletedNormally = true;
                                }
                            }
                        }
                        catch (System.Reflection.TargetException exc)
                        {
                            System.Diagnostics.Debug.WriteLine("TEST SEQUENCE TargetException1");
                            _currentTest.ExitTest(exc);
                            ReportException(exc);
                            if (testInfo.ProcessType == ProcessType.FeatureDefinition)
                            {
                                System.Diagnostics.Debug.WriteLine("TEST SEQUENCE TargetException2");
                                break;
                            }
                        }
                        catch (System.ArgumentException exc)
                        {
                            System.Diagnostics.Debug.WriteLine("TEST SEQUENCE ArgumentException");
                            _currentTest.ExitTest(exc);
                            ReportException(exc);
                        }
                        catch (System.Reflection.TargetParameterCountException exc)
                        {
                            System.Diagnostics.Debug.WriteLine("TEST SEQUENCE TargetParameterCountException");
                            _currentTest.ExitTest(exc);
                            ReportException(exc);
                        }
                        catch (System.MethodAccessException exc)
                        {
                            System.Diagnostics.Debug.WriteLine("TEST SEQUENCE MethodAccessException");
                            _currentTest.ExitTest(exc);
                            ReportException(exc);
                        }
                        catch (System.InvalidOperationException exc)
                        {
                            System.Diagnostics.Debug.WriteLine("TEST SEQUENCE InvalidOperationException");
                            _currentTest.ExitTest(exc);
                            ReportException(exc);
                        }
                        catch (Exception exc)
                        {
                            System.Diagnostics.Debug.WriteLine("TEST SEQUENCE Exception");
                            ReportException(exc);
                        }
                    }
                    processes.EndExecution();
                } while (_parameters.RepeatTests && !breakFlag);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine("TEST SEQUENCE wrap Exception");
            }
            finally
            {
                //If halt command was received and handled before start of waiting on _haltEvent.
                _haltEvent.Reset();
            }

            return bCompletedNormally;
        }

        void pst_ProfileDefinitionCompleted(IProfileDefinition profile, ProfileStatus status, string dump)
        {
            switch (status)
            {
                case ProfileStatus.Failed:
                    _failedProfiles.Add(profile);
                    break;
                case ProfileStatus.NotSupported:
                    _unsupportedProfiles.Add(profile);
                    break;
                case ProfileStatus.Supported:
                    _supportedProfiles.Add(profile);
                    break;
            }

            if (ProfileDefinitionCompleted != null)
            {
                ProfileDefinitionCompleted(profile, status, dump);
            }
        }


        /// <summary>
        /// Subscribe to tests events.
        /// </summary>
        /// <param name="test">Test to be subscribed.</param>
        void SubscribeToTestEvents(BaseTest test)
        {
            test.OnTestCompleted += new TestCompleted(test_OnTestCompleted);
            test.OnStepCompleted += new StepCompleted(test_OnStepCompleted);
            test.OnStepStarted += new Action<StepResult>(test_OnStepStarted);
            test.OnRequestSent += new Action<string>(test_OnRequestSent);
            test.OnResponseReceived += new Action<string>(test_OnResponseReceived);
            test.OnStepEvent += new Action<string>(test_OnStepEvent);
            test.OnTestEvent += new Action<string>(test_OnTestEvent);
            test.Paused += new Action(test_Paused);
            test.NetworkSettingsChangedEvent += test_OnNetworkSettingsChanged;
            test.SecurityChangedEvent += TestOnSecurityChangedEvent;
        }

        BaseTest InitCurrentTest(TestInfo testInfo, Type[] types, object[] args)
        {
            // find constructor.
            Type t = testInfo.Method.DeclaringType;
            System.Reflection.ConstructorInfo ci =
                t.GetConstructor(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
                null,
                System.Reflection.CallingConventions.HasThis,
                types,
                null);

            // Create object
            object itObject = ci.Invoke(args);
            BaseTest test = (BaseTest)itObject;
            _currentTest = test;

            // subscribe to test events.
            SubscribeToTestEvents(test);

            if (testInfo.ProcessType == ProcessType.FeatureDefinition)
            {
                FeaturesDefinitionProcess fdp = (FeaturesDefinitionProcess)test;
                fdp.FeatureDefined += new Action<Feature, bool>(fdp_FeatureDefined);
                fdp.FeatureDefinitionFailed += new Action<Feature>(fdp_FeatureDefinitionFailed);
                fdp.ScopeDefined += new Action<string>(fdp_ScopeDefined);
                fdp.DeviceInformationReceived += new Action<DeviceInformation>(fdp_DeviceInformationReceived);
            }

            _currentTest.EntryPoint = testInfo.Method;
            return test;
        }
        
        void InitConformanceTesting(TestLaunchParam param)
        {
            param.Features.Clear();
            param.Features.AddRange(_features);
            param.Features.AddRange(_undefinedFeatures);

            if (_features.Contains(Feature.Digest))
            {
                param.Security = Security.Digest;
            }
            else
            {
                param.Security = Security.WS;
            }
        }

        /// <summary>
        /// Perform pause at dispatcher level.
        /// </summary>
        /// <returns>True, if pause was ended by clicking "Resume"; false, if "Halt" 
        /// has been clicked.</returns>
        bool Sleep()
        {
            System.Diagnostics.Debug.WriteLine("Dispatcher level pause - wait resume event;");
            _state = TestState.Paused;

            if (Paused != null)
            {
                Paused();
            }

            /// Wait Resume or Halt.
            int handle = WaitHandle.WaitAny(new WaitHandle[] { _resumeEvent, _haltEvent });
            if (handle == 0)
            {
                _resumeEvent.Reset();
                System.Diagnostics.Debug.WriteLine("Dispatcher level pause - resume");
                lock (_pauseSync)
                {
                    _dispatcherLevelPause = false;
                }
            }
            return (handle == 0);
        }

        void ReportException(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            System.Diagnostics.Debug.WriteLine(string.Format("Shutdown: {0}", _shutDownInProgress));
            if (OnException != null && !_shutDownInProgress)
            {
                OnException(ex);
            }
        }

        void ReportInitializationCompleted(IEnumerable<TestInfo> tests, bool toBeContinued)
        {
            if (InitializationCompleted != null)
            {
                ConformanceInitializationData data = new ConformanceInitializationData();
                data.DeviceInformation = _deviceInformation;
                data.TestsSelected.AddRange(tests);

                data.FailedProfiles.AddRange(_failedProfiles);
                data.SupportedProfiles.AddRange(_supportedProfiles);
                data.UnsupportedProfiles.AddRange(_unsupportedProfiles);

                data.SupportedFeatures.AddRange(_features);
                data.UnsupportedFeatures.AddRange(_unsupportedFeatures);
                data.UndefinedFeatures.AddRange(_undefinedFeatures);
                data.Scopes.AddRange(_scopes);

                data.Continue = toBeContinued;

                InitializationCompleted(data);
            }
        }

        #region TestEvents

        void test_OnNetworkSettingsChanged(string newServiceAddress)
        {
            if (NetworkSettingsChangedEvent != null && !_shutDownInProgress)
            {
                NetworkSettingsChangedEvent(newServiceAddress);
                if (args != null && args.Length != 0)
                {
                    if (((TestLaunchParam)args[0]).ServiceAddress != null)
                        ((TestLaunchParam)args[0]).ServiceAddress = newServiceAddress;
                }
            }
        }

        private void TestOnSecurityChangedEvent(CredentialsProvider credentialsProvider)
        {
            if (null != SecurityChangedEvent)
            {
                SecurityChangedEvent(credentialsProvider);
                if (args != null && args.Length != 0)
                {
                    ((TestLaunchParam)args[0]).UserName = credentialsProvider.Username;
                    ((TestLaunchParam)args[0]).Password = credentialsProvider.Password;
                }
            }
        }

        void test_OnTestCompleted(TestLog log)
        {
            if (TestCompleted != null && !_shutDownInProgress)
            {
                TestCompleted(_currentTestInfo, log);
            }
        }

        void test_OnStepStarted(StepResult obj)
        {
            if (StepStarted != null && !_shutDownInProgress)
            {
                StepStarted(obj);
            }
        }

        void test_OnStepCompleted(StepResult result)
        {
            if (StepCompleted != null && !_shutDownInProgress)
            {
                StepCompleted(result);
            }
        }
        
        void test_OnRequestSent(string obj)
        {
            if (RequestSent != null && !_shutDownInProgress)
            {
                RequestSent(obj);
            }
        }
        
        void test_OnResponseReceived(string obj)
        {
            if (ResponseReceived != null && !_shutDownInProgress)
            {
                ResponseReceived(obj);
            }
        }

        void test_OnStepEvent(string obj)
        {
            if (StepEvent != null && !_shutDownInProgress)
            {
                StepEvent(obj);
            }
        }
        
        void test_OnTestEvent(string obj)
        {
            if (TestEvent != null && !_shutDownInProgress)
            {
                TestEvent(obj);
            }
        }
        
        void fdp_FeatureDefined(Feature feature, bool supported)
        {
            if (_undefinedFeatures.Contains(feature))
            {
                _undefinedFeatures.Remove(feature);
            }
            if (supported)
            {
                if (!_features.Contains(feature))
                {
                    _features.Add(feature);
                }
                if (_unsupportedFeatures.Contains(feature))
                {
                    _unsupportedFeatures.Remove(feature);
                }

                //[20.05.2013] AKS: store value of Capabilities.MaxPullPoints to use it in profile check
                if (Feature.MaxPullPoints == feature)
                    MaxPullPoints = ((FeaturesDefinitionProcess)_currentTest).MaxPullPoints;
                if (Feature.MaximumNumberOfKeys == feature)
                    MaximumNumberOfKeys = ((FeaturesDefinitionProcess)_currentTest).MaximumNumberOfKeys;
                if (Feature.MaximumNumberOfCertificates == feature)
                    MaximumNumberOfCertificates = ((FeaturesDefinitionProcess)_currentTest).MaximumNumberOfCertificates;
            }
            else
            {
                if (!_unsupportedFeatures.Contains(feature))
                {
                    _unsupportedFeatures.Add(feature);
                }
                if (_features.Contains(feature))
                {
                    _features.Remove(feature);
                }
            }
            if (FeatureDefined != null)
            {
                FeatureDefined(feature, supported);
            }
        }

        void fdp_FeatureDefinitionFailed(Feature feature)
        {
            if (!_undefinedFeatures.Contains(feature))
            {
                _undefinedFeatures.Add(feature);
            }
            if (_features.Contains(feature))
            {
                _features.Remove(feature);
            }
            if (_unsupportedFeatures.Contains(feature))
            {
                _unsupportedFeatures.Remove(feature);
            }
            if (FeatureDefinitionFailed != null)
            {
                FeatureDefinitionFailed(feature);
            }
        }

        void fdp_ScopeDefined(string scope)
        {
            _scopes.Add(scope);
            //if (ScopeDefined != null)
            //{
            //    ScopeDefined(scope);
            //}
        }
        
        void fdp_DeviceInformationReceived(DeviceInformation data)
        {
            _deviceInformation = data;
            if (DeviceInformationReceived != null)
            {
                DeviceInformationReceived(data);
            }
        }

        /// <summary>
        /// Notify event handlers about pause.
        /// </summary>
        void test_Paused()
        {
            if (Paused != null)
            {
                Paused();
            }
        }
        
        #endregion
    }
}
