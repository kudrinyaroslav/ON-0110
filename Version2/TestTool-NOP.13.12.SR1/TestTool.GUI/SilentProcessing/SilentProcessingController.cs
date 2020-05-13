using System;
using System.IO;
using System.Linq;
using System.Threading;
using TestTool.GUI.Controllers;
using TestTool.GUI.Data;
using TestTool.GUI.Utils;
using TestTool.Tests.Engine;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TestTool.GUI
{
    class SilentProcessingController
    {
        private SilentTestView _view;
        public SilentProcessingController(CompactProcessingForm view)
        {
            _view = new SilentTestView(view);
            view.Closing += (view_Closing);

            ContextController.InitGeneralContext();
        }

        private TestController _controller;

        void view_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_controller != null && _controller.Running)
            {
                e.Cancel = true;
                System.Windows.Forms.MessageBox.Show("Testing is running, unable to close the form.");
            }
        }

        private Dictionary<string, string> _options;

        void UpdateDeviceContext(TestSuiteParameters parameters)
        {
            DeviceEnvironment environment = ContextController.GetDeviceEnvironment(); 

            environment.Timeouts = new Timeouts();
            environment.Timeouts.InterTests = parameters.TimeBetweenTests;
            environment.Timeouts.Message = parameters.MessageTimeout;
            environment.Timeouts.Reboot = parameters.RebootTimeout;

            environment.EnvironmentSettings = new EnvironmentSettings();
            environment.EnvironmentSettings.DnsIpv4 = parameters.EnvironmentSettings.DnsIpv4;
            environment.EnvironmentSettings.NtpIpv4 = parameters.EnvironmentSettings.NtpIpv4;
            environment.EnvironmentSettings.DnsIpv6 = parameters.EnvironmentSettings.DnsIpv6;
            environment.EnvironmentSettings.NtpIpv6 = parameters.EnvironmentSettings.NtpIpv6;
            environment.EnvironmentSettings.GatewayIpv4 = parameters.EnvironmentSettings.DefaultGateway;
            environment.EnvironmentSettings.GatewayIpv6 = parameters.EnvironmentSettings.DefaultGatewayIpv6;

            environment.TestSettings = new TestSettings();
            environment.TestSettings.PTZNodeToken = parameters.PTZNodeToken;

            environment.TestSettings.UseEmbeddedPassword = parameters.UseEmbeddedPassword;
            environment.TestSettings.Password1 = parameters.Password1;
            environment.TestSettings.Password2 = parameters.Password2;
            environment.TestSettings.OperationDelay = parameters.OperationDelay;
            environment.TestSettings.RecoveryDelay = parameters.RecoveryDelay;
            environment.TestSettings.VideoSourceToken = parameters.VideoSourceToken;

            environment.TestSettings.SecureMethod = parameters.SecureMethod;
            environment.TestSettings.SubscriptionTimeout = parameters.SubscriptionTimeout;
            environment.TestSettings.EventTopic = parameters.EventTopic;
            environment.TestSettings.TopicNamespaces = parameters.TopicNamespaces;

            environment.TestSettings.RelayOutputDelayTimeMonostable = parameters.RelayOutputDelayTimeMonostable;

            environment.TestSettings.RecordingToken = parameters.RecordingToken;
            environment.TestSettings.SearchTimeout = parameters.SearchTimeout;
            environment.TestSettings.MetadataFilter = parameters.MetadataFilter;

            environment.TestSettings.RetentionTime = parameters.RetentionTime;
        }

        public void Run(string[] args)
        {
            if (args.Contains(CommandLineArgs.PARAMETERSFILE))
            {
                LoadOptions(args);

                SerializableTestingParameters parameters = LoadParameters();
                if (parameters == null)
                {
                    return;
                }

                // get test suite parameters;
                // get operator's etc. data
                // get info about files where to save data;
                
                Controllers.TestController controller = new TestController(_view);
                _controller = controller;

                AutoResetEvent completed = new AutoResetEvent(false);

                bool completedOk = true;
                controller.TestSuiteCompleted +=
                    (normally) =>
                                         {
                                             completedOk = normally;
                                             completed.Set();
                                         };

                controller.ConformanceInitializationCompleted += controller_ConformanceInitializationCompleted;

                controller.LoadTests();
                
                TestSuiteParameters testSuiteParameters = parameters.GetTestSuiteParameters();
                
                testSuiteParameters.AdvancedParameters = new Dictionary<string, object>();
                if (parameters.Advanced != null)
                {
                    List<object> advanced = AdvancedParametersUtils.Deserialize(parameters.Advanced, controller.AdvancedSettingsTypes);
                    foreach (object p in advanced)
                    {
                        testSuiteParameters.AdvancedParameters.Add(p.GetType().GUID.ToString(), p);
                    }
                }

                UpdateDeviceContext(testSuiteParameters);

                controller.RunConformanceSilent(testSuiteParameters);
                completed.WaitOne();

                if (!completedOk)
                {
                    return;
                }

                try
                {
                    // Save documents

                    Output output = parameters.Output;

                    // Get directory 
                    string folder = string.Empty;

                    if (output != null)
                    {
                        folder = output.Directory;
                    }
                    if (string.IsNullOrEmpty(folder))
                    {
                        folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        folder = Path.Combine(folder, "ONVIF Device Test Tool");
                    }
                    string path = folder;
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    // Create nested folder if necessary
                    if (output != null && output.CreateNestedFolder)
                    {
                        string name = DateTime.Now.ToString("yyyy-MM-dd hh-mm");
                        path = Path.Combine(folder, name);
                        Directory.CreateDirectory(path);
                    }

                    TestLogFull log = GetTestLog(testSuiteParameters, parameters, controller);

                    // Save Report

                    string reportFileName = null;
                    if (output != null)
                    {
                        reportFileName = output.Report;
                    }
                    if (string.IsNullOrEmpty(reportFileName))
                    {
                        if (log.DeviceInformation != null)
                        {
                            reportFileName = string.Format("{0} - report.pdf", log.ProductName);
                        }
                    }

                    if (string.IsNullOrEmpty(reportFileName))
                    {
                        reportFileName = "report.pdf";
                    }

                    reportFileName = Path.Combine(path, reportFileName);
                    Utils.PdfReportGenerator reportGenerator = new PdfReportGenerator();
                    reportGenerator.CreateReport(reportFileName, log);

                    // Save DoC

                    string docFileName = null;
                    if (output != null)
                    {
                        docFileName = output.DeclarationOfConformance;
                    }
                    if (string.IsNullOrEmpty(docFileName))
                    {
                        if (log.DeviceInformation != null)
                        {
                            docFileName = string.Format("{0} - DoC.pdf", log.ProductName);
                        }
                    }

                    if (string.IsNullOrEmpty(docFileName))
                    {
                        docFileName = "DoC.pdf";
                    }

                    docFileName = Path.Combine(path, docFileName);
                    Utils.DoCGenerator docGenerator = new DoCGenerator();
                    docGenerator.CreateReport(docFileName, log);
                    
                    // Save log, if required
                    if (output != null)
                    {
                        if (!string.IsNullOrEmpty(output.TestLog))
                        {
                            string fileName = Path.Combine(path, output.TestLog);

                            List<TestResult> results = new List<TestResult>();
                            foreach (TestTool.Tests.Definitions.Data.TestInfo ti in controller.TestInfos)
                            {
                                TestResult tr = controller.GetTestResult(ti);
                                if (tr != null)
                                {
                                    results.Add(tr);
                                }
                            }
                            controller.Save(fileName, results);
                        }

                        if (!string.IsNullOrEmpty(output.FeatureDefinitionLog))
                        {
                            string fileName = Path.Combine(path, output.FeatureDefinitionLog);
                            TestResult tr = new TestResult();
                            tr.Log = _controller.GetFeaturesDefinitionLog().Log;
                            tr.PlainTextLog = _controller.GetFeaturesDefinitionLog().PlainTextLog;
                            controller.Save(fileName, tr);
                        }
                    }
                }
                catch (Exception exc)
                {
                    SaveErrorLog(string.Format("Failed to create documents: {0}", exc.Message));
                }
            }
        }

        void controller_ConformanceInitializationCompleted(ConformanceInitializationData data)
        {
            _view.DefineTestCount(data.TestsSelected.Count);
        }

        private bool LoadOptions(string[] args)
        {
            _options = new Dictionary<string, string>();
            for (int i = 0; i< args.Length; i++)
            {
                if (CommandLineArgs.OPTIONS.Contains(args[i]))
                {
                    if (i<args.Length-1)
                    {
                        _options.Add(args[i], args[i+1]);
                    }
                    i++;
                }
            }
            return true;
        }

        private string _errorLog;
        void SaveErrorLog(string entry)
        {
            if (string.IsNullOrEmpty(_errorLog))
            {
                string myAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string testToolLogs = Path.Combine(myAppData, "ONVIF\\ONVIF Device Test Tool");
                if (!Directory.Exists(testToolLogs))
                {
                    Directory.CreateDirectory(testToolLogs);
                }

                string errorLog = string.Format("ErrorLog {0}.txt", DateTime.Now.ToString("yyyy-MM-dd hh-mm"));
                _errorLog = Path.Combine(testToolLogs, errorLog);
            }
            StreamWriter sw = File.AppendText(_errorLog);
            sw.WriteLine(entry);
            sw.Close();

        }

        private SerializableTestingParameters LoadParameters()
        {
            SerializableTestingParameters userParameters = null;

            if (_options.ContainsKey(CommandLineArgs.PARAMETERSFILE))
            {
                string parametersFile = _options[CommandLineArgs.PARAMETERSFILE];

                if (!File.Exists(parametersFile))
                {
                    SaveErrorLog(string.Format("File not found: {0}", parametersFile));
                    return null;
                }
                
                XmlSerializer serializer = new XmlSerializer(typeof(SerializableTestingParameters));
                FileStream stream = null;
                try
                {
                    stream = new FileStream(parametersFile, FileMode.Open);
                    userParameters = (SerializableTestingParameters) serializer.Deserialize(stream);
                }
                catch (Exception exc)
                {
                    SaveErrorLog(string.Format("Parameters loading failed: {0}", exc.Message));
                    return null;
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }

            }
            return userParameters;
        }
        
        private TestLogFull GetTestLog(TestSuiteParameters testSuiteParameters,
            SerializableTestingParameters parameters, 
            TestController controller)
        {
            controller.UpdateTestLog();

            TestLogFull testLog = new TestLogFull();
            Data.TestLog log = ContextController.GetTestLog();
            testLog.TestResults = log.TestResults;
            testLog.Features = log.Features;
            testLog.InitializationData = log.InitializationData;
            testLog.FeaturesDefinitionLog = log.FeaturesDefinitionLog;

            testLog.DeviceInformation = controller.DeviceInformation;

            if (log.TestExecutionTime != DateTime.MinValue)
            {
                testLog.TestExecutionTime = log.TestExecutionTime;
            }
            else
            {
                testLog.TestExecutionTime = DateTime.Now;
            }

            testLog.Application = new ApplicationInfo();

            testLog.DeviceEnvironment = new DeviceEnvironment();
            testLog.DeviceEnvironment.Credentials = new Credentials();
            testLog.DeviceEnvironment.Credentials.UserName = testSuiteParameters.UserName;
            // password not used for REPORTS
            testLog.DeviceEnvironment.TestSettings = new TestSettings();
            // only two values are user for log
            testLog.DeviceEnvironment.TestSettings.OperationDelay = testSuiteParameters.OperationDelay;
            testLog.DeviceEnvironment.TestSettings.RecoveryDelay = testSuiteParameters.RecoveryDelay;
            testLog.DeviceEnvironment.Timeouts.InterTests = testSuiteParameters.TimeBetweenTests;
            testLog.DeviceEnvironment.Timeouts.Message = testSuiteParameters.MessageTimeout;
            testLog.DeviceEnvironment.Timeouts.Reboot = testSuiteParameters.RebootTimeout;
            
            if (parameters.Device != null)
            {
                testLog.ProductName = parameters.Device.Model;
            }

            if (parameters.SessionInfo != null)
            {
                testLog.TesterInfo = parameters.SessionInfo.TesterInfo;
                testLog.OtherInformation = parameters.SessionInfo.OtherInformation;
                testLog.MemberInfo = parameters.SessionInfo.MemberInfo;
            }

            return testLog;
        }

    }
}
