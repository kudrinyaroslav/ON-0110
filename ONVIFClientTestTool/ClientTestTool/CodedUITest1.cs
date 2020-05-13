using System;
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using ClientTestTool.UIMaps.LaunchToolClasses;
using ClientTestTool.UIMaps.ConfigurationClasses;
using ClientTestTool.UIMaps.DiagnosticsClasses;
using System.Configuration;
using System.Collections.Specialized;
using ClientTestTool.UIMaps.ConformanceClasses;
using ClientTestTool.UIMaps.EventClasses;


namespace ClientTestTool
{
    /// <summary>
    /// Сводное описание для CodedUITest1
    /// </summary>
    [CodedUITest]
    public class CodedUITest1
    {
        public CodedUITest1()
        {
        }

        [TestMethod, Timeout(TestTimeout.Infinite)]
        public void CodedUITestMethod1()
        {
            TestSetList testSetList = new TestSetList();
            if (ConfigurationSettings.AppSettings["SingleFileSet"] == "true")
            {
                testSetList.testSetFilePaths = new List<string>();
                testSetList.testSetFilePaths.Add(ConfigurationSettings.AppSettings["TestSetFilePath"]);
            }
            else
            {
                testSetList = testSetList.DeSerializeData(ConfigurationSettings.AppSettings["TestSetListFilePath"]);
            }
            LogFile logFile = new LogFile();
            foreach (string filePath in testSetList.testSetFilePaths)
            {
                TestSet testSet = new TestSet();
                testSet = testSet.DeSerializeData(filePath);

                if (ConfigurationSettings.AppSettings["SingleFileSet"] == "true")
                {
                    testSetList.reportPrefix = testSet.reportPrefix;
                }
                string stepName = "";
                foreach (Test test in testSet.testList)
                {
                    try
                    {
                        LogTest logTest = new LogTest(test);

                        stepName = "LaunchONVIFClientTestTool";
                        this.LaunchTool.LaunchONVIFClientTestTool();

                        Configuration configuration = new Configuration(test);

                        stepName = "AddpcapngFiles";
                        configuration.AddpcapngFiles();

                        stepName = "StartParsing";
                        configuration.StartParsing();

                        stepName = "AddFeatureFiles";
                        configuration.AddFeatureFiles();

                        Event events = new Event();

                        stepName = "SelectEvents";
                        events.ClickSupportedEvents();
                        foreach (SupportedEvent supportedEvent in test.eventList)
                        {
                            events.CheckEvents(supportedEvent);
                        }

                        events.ClickParsingResults();

                        
                        Diagnostics diagnostics = new Diagnostics(test, filePath);

                        stepName = "RunConformanceTest";
                        diagnostics.RunConformanceTest();

                        Thread.Sleep(5000);

                        stepName = "CheckExpectedResult";
                        diagnostics.CheckExpectedResult();

                        stepName = "CheckFeaturesExpectedResult";
                        diagnostics.CheckFeaturesExpectedResult();

                        Conformance conformance = new Conformance(test, filePath);

                        stepName = "CheckProfiles";
                        conformance.CheckProfiles();

                        stepName = "CloseONVIFClientTestTool";
                        this.LaunchTool.CloseONVIFClientTestTool();

                        foreach (LogData logData in diagnostics.logData)
                        {
                            logTest.check.Add(logData);
                        }

                        foreach (LogData logDataFeatures in diagnostics.logDataFeatures)
                        {
                            logTest.checkFeatures.Add(logDataFeatures);
                        }

                        foreach (LogData logDataProfiles in conformance.logDataProfiles)
                        {
                            logTest.checkProfiles.Add(logDataProfiles);
                        }

                        logFile.test.Add(logTest);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(String.Format("Test ID: {0}, Step Name {1}. Message: {2}", test.id, stepName, e.Message), e);
                    }
                }
            }
            string logFile_failed = "";
            foreach (var test in logFile.test)
            {
                if (test.check.FindAll(C => C.result == "FAILED").Count != 0)
                {
                    logFile_failed = "_FAILED";
                }
            }

            string reportDate = DateTime.Now.ToString("yyyyMMdd_HH_mm_ss");
            logFile.SerializeData(logFile, ConfigurationSettings.AppSettings["LogFileFolderPath"] + testSetList.reportPrefix + reportDate + logFile_failed + ".xml");
            // Чтобы создать код для этого теста, выберите в контекстном меню команду "Сформировать код для кодированного теста ИП", а затем выберите один из пунктов меню.
            // Дополнительные сведения по сформированному коду см. по ссылке http://go.microsoft.com/fwlink/?LinkId=179463
        }

        #region Дополнительные атрибуты тестирования

        // При написании тестов можно использовать следующие дополнительные атрибуты:

        //TestInitialize используется для выполнения кода перед запуском каждого теста 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled;
            Playback.PlaybackSettings.ShouldSearchFailFast = false;
            Playback.PlaybackSettings.DelayBetweenActions = 500;
            Playback.PlaybackSettings.SearchTimeout = 1000;
            Playback.PlaybackSettings.WaitForReadyTimeout = 5000; //45 minuts

            // Чтобы создать код для этого теста, выберите в контекстном меню команду "Сформировать код для кодированного теста ИП", а затем выберите один из пунктов меню.
            // Дополнительные сведения по сформированному коду см. по ссылке http://go.microsoft.com/fwlink/?LinkId=179463
        }

        ////TestCleanup используется для выполнения кода после завершения каждого теста
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // Чтобы создать код для этого теста, выберите в контекстном меню команду "Сформировать код для кодированного теста ИП", а затем выберите один из пунктов меню.
        //    // Дополнительные сведения по сформированному коду см. по ссылке http://go.microsoft.com/fwlink/?LinkId=179463
        //}

        #endregion

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public LaunchTool LaunchTool
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new LaunchTool();
                }

                return this.map;
            }
        }

        private LaunchTool map;
    }
}
