///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using TestTool.GUI.Views;
using TestTool.GUI.Data;
using TestTool.GUI.Utils;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Data;
using System.Text;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Engine;
using System.Collections.Generic;
using System.Linq;

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

        // 
        //
        //
        public event EventHandler<SettingsMissingEventArgs> SettingsMissing;

        /// <summary>
        /// Raises SettingsMissing events
        /// </summary>
        /// <param name="settings"></param>
        public void RaiseSettingsMissing(List<string> settings)
        {
            if (SettingsMissing != null)
            {
                SettingsMissing(this, new SettingsMissingEventArgs(){Settings = settings});
            }
        }

        #region TEST

        /// <summary>
        /// Notifies TestController that conformance testing is requested
        /// </summary>
        public event Action TestsRunRequested;
        /// <summary>
        /// Notifies TestController that Halt is requested
        /// </summary>
        public event Action HaltRequested;
        /// <summary>
        /// Notifies TestController that form is about to close
        /// </summary>
        public event Action ExitRequested;

        /// <summary>
        /// True, if tests are being executed
        /// </summary>
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
            View.EnableGenerateDoc(false);

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

        /// <summary>
        /// Number of tests selected for conformance
        /// </summary>
        private int _testsCount;
        /// <summary>
        /// Flag indicating whether tests have been selected
        /// </summary>
        private bool _testsDefined = false;
        /// <summary>
        /// Number of completed tests
        /// </summary>
        private int _completedTests;
        /// <summary>
        /// Number of failed tests
        /// </summary>
        private int _failedTests;
        
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

        /// <summary>
        /// Flag inficating that form has data from previous test run
        /// </summary>
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
                    if (log.TestStatus == Tests.Definitions.Trace.TestStatus.Failed)
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
                        View.EnableGenerateDoc(true);
                        //View.EnableGenerateDoc(!_preliminaryFailed && _failedTests == 0);
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
                View.EnableGenerateDoc(false);
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
                report.AppendFormat("   {0} SUPPORTED{1}", profile.GetProfileName(), Environment.NewLine);
            }
            foreach (var profile in data.UnsupportedProfiles)
            {
                report.AppendFormat("   {0} NOT SUPPORTED{1}", profile.GetProfileName(), Environment.NewLine);
            }
            foreach (var profile in data.FailedProfiles)
            {
                report.AppendFormat("   {0} FAILED{1}", profile.GetProfileName(), Environment.NewLine);
            }

            //If all profiles claimed to be supported are not in Release status then fail conformance.
            if (!data.SupportedProfiles.Any(e => ProfileVersionStatus.Release == e.GetProfileVersionStatus()) && !data.FailedProfiles.Any(e => ProfileVersionStatus.Release == e.GetProfileVersionStatus()))
            {
                _preliminaryFailed = true;
                report.AppendLine("No profiles in release status are supported, conformance will be FAILED");
            }
            if (data.FailedProfiles.Any())
            {
                _preliminaryFailed = true;
                report.AppendLine("Not all profiles claimed are supported, conformance will be FAILED");
            }
            if (data.UndefinedFeatures.Any())
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
            testLog.ProductName = info.DevInfo.ProductName;
            testLog.ProductTypes = info.DevInfo.ProductTypes;
            testLog.ProductTypesOther = info.DevInfo.ProductTypesOther;
            testLog.OtherInformation = info.OtherInfo;
            testLog.MemberInfo = info.MemberInfo;
            testLog.SupportInfo = info.SupportInfo;

            testLog.ManagementSettings = ContextController.GetManagementSettings();
            testLog.DeviceEnvironment = ContextController.GetDeviceEnvironment();
            testLog.Timeouts = log.Timeouts;
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
            View.ShowError(ex);
        }
        
        #endregion

        #region Context
        
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
                    View.ProductTypesAll = info.DevInfo.ProductTypesAll;
                    View.ProductTypes = info.DevInfo.ProductTypes;
                    View.ProductTypesOther = info.DevInfo.ProductTypesOther;
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

                if (info.SupportInfo != null)
                {
                    View.InternationalAddress = !string.IsNullOrEmpty(info.SupportInfo.InternationalAddress)
                                                                        ? info.SupportInfo.InternationalAddress.Replace("\n", "\r\n")
                                                                        : string.Empty;

                    View.RegionalAddress =  !string.IsNullOrEmpty(info.SupportInfo.RegionalAddress)
                                                                        ? info.SupportInfo.RegionalAddress.Replace("\n", "\r\n")
                                                                        : string.Empty;

                    View.SupportUrl = info.SupportInfo.SupportUrl;
                    View.SupportEmail = info.SupportInfo.SupportEmail;
                    View.SupportPhone = info.SupportInfo.SupportPhone;
                }

                ContextController.UpdateSetupInfo(info);
            }
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
            deviceInfo.ProductTypesAll = View.ProductTypesAll;
            deviceInfo.ProductTypes = View.ProductTypes;
            deviceInfo.ProductTypesOther = View.ProductTypesOther;

            MemberInfo memberInfo = new MemberInfo();
            memberInfo.Address = View.MemberAddress;
            memberInfo.Name = View.MemberName;

            SupportInfo supportInfo = new SupportInfo();
            supportInfo.InternationalAddress = View.InternationalAddress;
            supportInfo.RegionalAddress = View.RegionalAddress;
            supportInfo.SupportUrl = View.SupportUrl;
            supportInfo.SupportEmail = View.SupportEmail;
            supportInfo.SupportPhone = View.SupportPhone;

            SetupInfo setupInfo = new SetupInfo();
            setupInfo.DevInfo = deviceInfo;
            setupInfo.OtherInfo = View.OtherInformation;
            setupInfo.TesterInfo = testerInfo;
            setupInfo.MemberInfo = memberInfo;
            setupInfo.SupportInfo = supportInfo;

            ContextController.UpdateSetupInfo(setupInfo);
        }

        #endregion

    }
}
