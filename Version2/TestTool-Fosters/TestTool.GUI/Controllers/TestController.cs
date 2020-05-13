///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.Tests.Definitions.Attributes;
using System.IO;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Trace;
using TestTool.GUI.Data;
using TestTool.GUI.Enums;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.UI;
using TestTool.Tests.Engine;
using System.Xml.Serialization;
using TestTool.GUI.Utils.Profiles;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// GUI logic for the Test tab.
    /// </summary>
    class TestController : BaseTestController<ITestView>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view">View.</param>
        public TestController(ITestView view)
            :base(view)
        {
            InitTestEnvironment();

            _profilesSupportInfo = new Dictionary<IProfileDefinition, ProfileTestInfo>();
            
            // TC specific
            _td.Paused += _td_Paused;
            _td.FeatureDefined += _td_FeatureDefined;
            _td.FeatureDefinitionFailed += _td_FeatureDefinitionFailed;
            _td.InitializationCompleted += _td_InitializationCompleted;
            _td.DeviceInformationReceived += _td_DeviceInformationReceived;
            _td.ProfileDefinitionCompleted += _td_ProfileDefinitionCompleted;
        }

        
        /// <summary>
        /// Indicate sthat single test is launched
        /// </summary>
        private bool _runningSingle;

        /// <summary>
        /// Quantity of test cases to be launched
        /// </summary>
        private int _testsCount;
        
        /// <summary>
        /// Is raised when some event occurs in the test.
        /// </summary>
        //public event Action<string> TestEvent;
        
        #region Events
        
        /// <summary>
        /// 
        /// </summary>
        public event Action<ConformanceInitializationData> ConformanceInitializationCompleted;

        /// <summary>
        /// Is raised when user clicks "Clear" button.
        /// </summary>
        public event Action TestsCleared;

        /// <summary>
        /// Is raised after tests are loaded.
        /// </summary>
        public event Action<List<TestInfo>> TestsLoaded;

        public event Action<DeviceInfo> DeviceInfoReceived;

        #endregion
        
        #region LOAD

        /// <summary>
        /// Tests information
        /// </summary>
        private List<TestInfo> _testInfos;

        public List<TestInfo> TestInfos
        {
            get { return _testInfos; }
        }

        private List<TestInfo> _testsSupported;

        private List<IProfileDefinition> _onvifProfiles;

        private Dictionary<Guid, SettingsTabPage> _controls;

        private List<Type> _settingsTypes;

        private Dictionary<IProfileDefinition, ProfileTestInfo> _profilesSupportInfo;

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
            _onvifProfiles = new List<IProfileDefinition>();

            _controls = new Dictionary<Guid, SettingsTabPage>();
            _settingsTypes = new List<Type>();

            Dictionary<Type, Type> controls = new Dictionary<Type, Type>();


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
                                LoadTests(t);
                            }

                            attrs = t.GetCustomAttributes(typeof(ProfileDefinitionAttribute), true);
                            if (attrs.Length > 0)
                            {
                                LoadProfile(t);
                            }

                            attrs = t.GetCustomAttributes(typeof(SettingsControlAttribute), true);
                            if (attrs.Length > 0)
                            {
                                SettingsControlAttribute attr = (SettingsControlAttribute)attrs[0];
                                if (!controls.ContainsKey(attr.ParametersType))
                                {
                                    controls.Add(attr.ParametersType, t);
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {

                }
            }


            foreach (Type type in _settingsTypes)
            {
                Type ctrlType = controls[type];

                object ctrl = Activator.CreateInstance(ctrlType);
                SettingsTabPage ctl = (SettingsTabPage)ctrl;
                ctl.Dock = DockStyle.Fill;
                
                _controls.Add(type.GUID, (SettingsTabPage)ctrl);
            }

            if (SettingPagesLoaded != null)
            {
                SettingPagesLoaded(_controls.Values.ToList());
            }

            View.DisplayTests(_testInfos);

            View.DisplayProfiles(_onvifProfiles.OrderBy( P => P.Name));

            if (TestsLoaded != null)
            {
                TestsLoaded(_testInfos);
            }
            return _testInfos;
        }

        public event Action<List<SettingsTabPage>> SettingPagesLoaded; 

        void LoadTests(Type t)
        {
            if (t.GetInterfaces().Contains(typeof(ITest)))
            {
                foreach (MethodInfo mi in t.GetMethods())
                {
                    object[] testAttributes = mi.GetCustomAttributes(typeof(TestAttribute), true);

                    if (testAttributes.Length > 0)
                    {
                        TestAttribute attribute = (TestAttribute)testAttributes[0];

                        TestInfo existing =
                            _testInfos.Where(ti => ti.Id == attribute.Id && ti.Category == attribute.Category).FirstOrDefault();

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
                        testInfo.ExecutionOrder = attribute.ExecutionOrder;
                        testInfo.Id = attribute.Id;
                        testInfo.Category = attribute.Category;
                        testInfo.Version = attribute.Version;
                        testInfo.RequirementLevel = attribute.RequirementLevel;
                        testInfo.RequiredFeatures.AddRange(attribute.RequiredFeatures);
                        testInfo.FunctionalityUnderTest.AddRange(attribute.FunctionalityUnderTest);

                        _testInfos.Add(testInfo);

                        if (attribute.ParametersTypes != null)
                        {
                            foreach (Type type in attribute.ParametersTypes)
                            {
                                if (!_settingsTypes.Contains(type))
                                {
                                    _settingsTypes.Add(type);
                                }
                            }
                        }
                    }
                }
            }
        }

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
        /// Launches test suite.
        /// </summary>
        /// <param name="parameters">Test suite parameters.</param>
        public void Run(TestSuiteParameters parameters)
        {
            View.ClearProfiles();
            RunDiagnostic(parameters, false);
        }
        
        /// <summary>
        /// Launches single test.
        /// </summary>
        /// <param name="parameters">Test suite parameters.</param>
        public void RunSingle(TestSuiteParameters parameters)
        {
            View.ClearProfiles();
            RunDiagnostic(parameters, true);
        }

        /// <summary>
        /// Runs tests as in conformance mode
        /// </summary>
        public void RunAll()
        {
            _td.ResetFeatures();
            Clear(true);
            View.SelectTests(new TestInfo[0]);

            TestSuiteParameters parameters = GetParameters();
            parameters.DefineProfiles = true;
            RunDiagnostic(parameters, false);
        }

        /// <summary>
        /// Launches features definition process
        /// </summary>
        /// <param name="parameters"></param>
        public void DefineFeatures(TestSuiteParameters parameters)
        {
            RunDiagnostic(parameters, true, true);
        } 
       
        void RunDiagnostic(TestSuiteParameters parameters, bool single)
        {
            RunDiagnostic(parameters, single, false);
        }

        void RunDiagnostic(TestSuiteParameters parameters, bool single, bool features)
        {
            parameters.AllTestCases.AddRange(_testInfos.OrderBy(TI => TI.ExecutionOrder).ThenBy(T => T.Category).ThenBy(t => t.Order));
            parameters.Profiles.AddRange(_onvifProfiles);
            parameters.Operator = new Utils.Operator(View.Window);
            parameters.VideoForm = View.GetVideoForm();
            _runningSingle = single;
            _testsCount = parameters.TestCases.Count;
            parameters.Operator = new Utils.Operator(View.Window);
            parameters.FeatureDefinition = features;
            if (_td.FeaturesDefined)
            {
                parameters.Features.AddRange(_td.Features);
                parameters.Features.AddRange(_td.UndefinedFeatures);
            }

            InternalRun(parameters, single, false);
        }
        
        public void RunConformance()
        {
            _td.ResetFeatures();
            Clear(true);
            View.SelectTests(new TestInfo[0]);

            TestSuiteParameters parameters = GetParameters();
            parameters.AllTestCases.Clear();
            parameters.AllTestCases.AddRange(_testInfos.OrderBy(TI => TI.ExecutionOrder).ThenBy(T => T.Category).ThenBy(t => t.Order));
            parameters.Operator = new Utils.Operator(View.Window);
            parameters.VideoForm = View.GetVideoForm();
            parameters.DefineProfiles = true;
            parameters.Profiles.AddRange(_onvifProfiles);
            parameters.Conformance = true;
            _runningSingle = false;

            InternalRun(parameters, false, true);
        }
        
        #endregion

        #region Results

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

        public bool HasTestResults()
        {
            return _testResults.Count > 0;
        }

        public void ClearFeatures()
        {
            _featureDefinitionLog = null;
            _td.ResetFeatures();
            View.ClearFeatures();
        }

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

        #endregion
        
        #region RUN EVENTS

        /// <summary>
        /// Handles "Paused" event.
        /// </summary>
        void _td_Paused()
        {
            View.SwitchToState(ApplicationState.TestPaused);
        }
        
        void _td_FeatureDefined(Feature feature, bool supported)
        {
            View.DisplayFeature(feature, supported);
        }


        void _td_FeatureDefinitionFailed(Feature feature)
        {
            View.DisplayUndefinedFeature(feature);
        }

        private ConformanceInitializationData _initializationData;
        void _td_InitializationCompleted(ConformanceInitializationData data)
        {
            _testsSupported = data.TestsSelected;
            _initializationData = data;

            if (data.Continue)
            {
                View.SelectTests(data.TestsSelected);
            }
            if (ConformanceInitializationCompleted != null)
            {
                ConformanceInitializationCompleted(data);
            }

            // only for test controller
            DisplayFunctionalitySupport(data.TestsSelected);
            View.DisplayProfiles(data.SupportedProfiles, data.FailedProfiles, data.UnsupportedProfiles);
            
        }

        void _td_DeviceInformationReceived(DeviceInformation data)
        {
            if (Conformance && DeviceInfoReceived != null)
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
            _profilesSupportInfo.Add(profile, info);
        }

        void DisplayFunctionalitySupport(List<TestInfo> tests)
        {
            List<Feature> features = new List<Feature>(_td.Features);
            features.AddRange(_td.UndefinedFeatures);
            ProfilesSupportInfo info = ProfilesSupportInfo.LoadPreliminary(_onvifProfiles, _testInfos, tests, features);
            View.DisplayFunctionalityWithoutTestsInSuite(info.FunctionalityWithoutTests);
            View.DisplayFunctionalityToBeTested(info.FunctionalityUnderTests);
            View.DisplayFailedByFeaturesFunctionality(info.MandatoryFunctionalityWithoutFeatures);
            View.DisplaySkippedByFeaturesFunctionality(info.OptionalFunctionalityUnderSkippedTests);
            
            DisplayProfileScopes();
        }

        protected override void CompleteTestSuite(TestSuiteParameters parameters, bool bCompletedNormally)
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

                    Dictionary<Functionality, bool> functionalities = ProfilesSupportInfo.CheckTestResults(results);
                    View.DisplaySupportedFunctionality(functionalities);

                    bool failed = _testResults.Values.Where(TR => TR.Log.TestStatus == TestStatus.Failed).Count() > 0;
                    //

                    List<IProfileDefinition> failedProfiles = new List<IProfileDefinition>();
                    List<IProfileDefinition> supportedProfiles = new List<IProfileDefinition>();

                    if (failed)
                    {
                        failedProfiles.AddRange(_initializationData.FailedProfiles);
                        failedProfiles.AddRange(_initializationData.SupportedProfiles);
                    }
                    else
                    {
                        supportedProfiles.AddRange(_initializationData.SupportedProfiles);
                    }

                    View.DisplayProfiles(supportedProfiles, failedProfiles, _initializationData.UnsupportedProfiles);
                }

                UpdateTestLog();
            }
        }

        void DisplayProfileScopes()
        {
            List<String> scopes = new List<String>();
            foreach (IProfileDefinition profile in _onvifProfiles)
            {
                foreach (string f in profile.MandatoryScopes)
                {
                    if (!scopes.Contains(f))
                    {
                        scopes.Add(f);
                    }
                }
            }
            foreach (string f in scopes)
            {
                View.DisplayScope(f, _td.Scopes.Contains(f));
            }
        }

        protected override void DisplayLogLine(string line)
        {
            View.WriteLine(line);
        }

        protected override void DisplayStepResult(StepResult result)
        {
            View.DisplayStepResult(result);
        }

        protected override void CompleteTest(TestInfo testInfo, Tests.Definitions.Trace.TestLog log)
        {
            bool isTest = testInfo.ProcessType == ProcessType.Test;

            if (isTest)
            {
                View.EndTest(_testResults[testInfo]);
            }
            else
            {
                View.EndTest(_featureDefinitionLog);
            }
        }

        protected override void DisplayTestStart(TestInfo testInfo)
        {
            View.BeginTest(testInfo);
        }

        protected override void ReportTestSuiteCompleted()
        {
            if (_testsCount > 1 || !_runningSingle)
            {
                View.ReportTestSuiteCompleted();
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

            //options.InteractiveFirst = View.InteractiveFirst;
            
            List<TestInfo> selectedTests = View.SelectedTests;
            foreach (TestInfo info in selectedTests)
            {
                options.Tests.Add(info.Order);
            }

            options.Groups.AddRange(View.SelectedGroups);

            ContextController.UpdateTestOptions(options);
        }

        void UpdateTestLog()
        {
            TestTool.GUI.Data.TestLog log = new TestTool.GUI.Data.TestLog();
            log.TestResults = _testResults;
            log.Features = _td.Features;
            log.TestExecutionTime = _testExecutionTime;
            log.InitializationData = _initializationData;
            log.FeaturesDefinitionLog = _featureDefinitionLog;
            ContextController.UpdateTestLog(log);
        }

        #endregion

        #region Profile, mode

        /// <summary>
        /// Applies profile passed.
        /// </summary>
        /// <param name="profile">Profile data.</param>
        public void ApplyProfile(Profile profile)
        {
            // tests + test groups
            View.ApplyProfileOptions(profile);
        }
        
        #endregion

        #region Save Debug report

        public void Save(string fileName, List<TestResult> results)
        {
            SaveInternal<List<TestResult>>(fileName, results);
        }

        public void Save(string fileName, TestResult results)
        {
            SaveInternal<TestResult>(fileName, results);
        }

        void SaveInternal<T>(string fileName, T results)
        {
            StreamWriter writer = null;
            try
            {
                DebugReport<T> report = new DebugReport<T>();
                report.ExecutionTime = DateTime.Now;
                report.DeviceInfo = ContextController.GetSetupInfo().DevInfo;

                report.Results = results;

                writer = new StreamWriter(fileName);
                XmlSerializer s = new XmlSerializer(typeof(DebugReport<T>));
                s.Serialize(writer, report);
            }
            catch (Exception exc)
            {
                View.ReportError("An error occurred: " + exc.Message);
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
