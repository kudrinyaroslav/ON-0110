using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using DUT.CameraWebService.Base;
using DUT.CameraWebService;

namespace DUT.CameraWebService
{
    public partial class TestSettings : System.Web.UI.Page
    {
        public static bool switchToNewMedia2Service = false;
        #region properties
        protected TestCommon TestCommon
        {
            get { return Application[AppVars.TESTCOMMON] != null ? (TestCommon)Application[AppVars.TESTCOMMON] : null; }
            set { Application[AppVars.TESTCOMMON] = value; }
        }
        protected Device10.DeviceServiceTest DeviceServiceTest
        {
            get { return Application[Base.AppVars.DEVICESERVICE] != null ? (Device10.DeviceServiceTest)Application[Base.AppVars.DEVICESERVICE] : null; }
            set { Application[Base.AppVars.DEVICESERVICE] = value; }
        }
        protected Events10.EventServiceTest EventServiceTest
        {
            get { return Application["m_EventServiceTest"] != null ? (Events10.EventServiceTest)Application["m_EventServiceTest"] : null; }
            set { Application["m_EventServiceTest"] = value; }
        }
        protected Media10.MediaServiceTest MediaServiceTest
        {
            get { return Application["m_MediaServiceTest"] != null ? (Media10.MediaServiceTest)Application["m_MediaServiceTest"] : null; }
            set { Application["m_MediaServiceTest"] = value; }
        }
        protected Imaging10.ImagingService10Test ImagingService10Test
        {
            get { return Application["m_ImagingService10Test"] != null ? (Imaging10.ImagingService10Test)Application["m_ImagingService10Test"] : null; }
            set { Application["m_ImagingService10Test"] = value; }
        }
        protected Imaging20.ImagingService20Test ImagingService20Test
        {
            get { return Application["m_ImagingService20Test"] != null ? (Imaging20.ImagingService20Test)Application["m_ImagingService20Test"] : null; }
            set { Application["m_ImagingService20Test"] = value; }
        }
        protected PTZ20.PTZServiceTest PTZServiceTest
        {
            get { return Application["m_PTZServiceTest"] != null ? (PTZ20.PTZServiceTest)Application["m_PTZServiceTest"] : null; }
            set { Application["m_PTZServiceTest"] = value; }
        }
        protected DeviceIO10.DeviceIO10ServiceTest DeviceIO10ServiceTest
        {
            get { return Application["m_DeviceIO10ServiceTest"] != null ? (DeviceIO10.DeviceIO10ServiceTest)Application["m_DeviceIO10ServiceTest"] : null; }
            set { Application["m_DeviceIO10ServiceTest"] = value; }
        }
        protected Search10.SearchServiceTest Search10ServiceTest
        {
            get { return Application["m_SearchServiceTest"] != null ? (Search10.SearchServiceTest)Application["m_SearchServiceTest"] : null; }
            set { Application["m_SearchServiceTest"] = value; }
        }
        protected Recording10.RecordingServiceTest Recording10ServiceTest
        {
            get { return Application["m_RecordingServiceTest"] != null ? (Recording10.RecordingServiceTest)Application["m_RecordingServiceTest"] : null; }
            set { Application["m_RecordingServiceTest"] = value; }
        }

        protected Receiver10.ReceiverServiceTest Receiver10ServiceTest
        {
            get { return Application[Base.AppVars.RECEIVERSERVICE] != null ? (Receiver10.ReceiverServiceTest)Application[Base.AppVars.RECEIVERSERVICE] : null; }
            set { Application[Base.AppVars.RECEIVERSERVICE] = value; }
        }

        protected Replay10.ReplayServiceTest Replay10ServiceTest
        {
            get { return Application[Base.AppVars.REPLAYSERVICE] != null ? (Replay10.ReplayServiceTest)Application[Base.AppVars.REPLAYSERVICE] : null; }
            set { Application[Base.AppVars.REPLAYSERVICE] = value; }
        }

        protected PACS10.PACSServiceTest PACS10ServiceTest
        {
            get { return Application["m_PACSServiceTest"] != null ? (PACS10.PACSServiceTest)Application["m_PACSServiceTest"] : null; }
            set { Application["m_PACSServiceTest"] = value; }
        }
        protected Door10.DoorServiceTest Door10ServiceTest
        {
            get { return Application["m_DoorServiceTest"] != null ? (Door10.DoorServiceTest)Application["m_DoorServiceTest"] : null; }
            set { Application["m_DoorServiceTest"] = value; }
        }
        protected PACS11.PACSServiceTest PACS11ServiceTest
        {
            get { return Application["m_PACS11ServiceTest"] != null ? (PACS11.PACSServiceTest)Application["m_PACS11ServiceTest"] : null; }
            set { Application["m_PACS11ServiceTest"] = value; }
        }
        protected Door11.DoorServiceTest Door11ServiceTest
        {
            get { return Application["m_Door11ServiceTest"] != null ? (Door11.DoorServiceTest)Application["m_Door11ServiceTest"] : null; }
            set { Application["m_Door11ServiceTest"] = value; }
        }
        protected Door12.DoorServiceTest Door12ServiceTest
        {
            get { return Application[Base.AppVars.DOORSERVICE] != null ? (Door12.DoorServiceTest)Application[Base.AppVars.DOORSERVICE] : null; }
            set { Application[Base.AppVars.DOORSERVICE] = value; }
        }
        protected PACS12.PACSServiceTest PACS12ServiceTest
        {
            get { return Application[Base.AppVars.PACSSERVICE] != null ? (PACS12.PACSServiceTest)Application[Base.AppVars.PACSSERVICE] : null; }
            set { Application[Base.AppVars.PACSSERVICE] = value; }
        }
        protected AdvancedSecurity10.AdvancedSecurityServiceTest AdvancedSecurity10ServiceTest
        {
            get { return Application[Base.AppVars.ADVSECSERVICE] != null ? (AdvancedSecurity10.AdvancedSecurityServiceTest)Application[Base.AppVars.ADVSECSERVICE] : null; }
            set { Application[Base.AppVars.ADVSECSERVICE] = value; }
        }

        protected Events10.PullPointSubscriptionServiceTest PullPointSubscriptionServiceTest
        {
            get { return Application[Base.AppVars.EVENTPULLPOINTSERVICE] != null ? (Events10.PullPointSubscriptionServiceTest)Application[Base.AppVars.EVENTPULLPOINTSERVICE] : null; }
            set { Application[Base.AppVars.EVENTPULLPOINTSERVICE] = value; }
        }

        protected ServiceCredential10.CredentialServiceTest CredentialServiceTest
        {
            get { return Application[Base.AppVars.CREDENTIALSERVICE] != null ? (ServiceCredential10.CredentialServiceTest)Application[Base.AppVars.CREDENTIALSERVICE] : null; }
            set { Application[Base.AppVars.CREDENTIALSERVICE] = value; }
        }

        protected ServiceAccessRules10.AccessRulesServiceTest AccessRulesServiceTest
        {
            get { return Application[Base.AppVars.ACCESSRULESSERVICE] != null ? (ServiceAccessRules10.AccessRulesServiceTest)Application[Base.AppVars.ACCESSRULESSERVICE] : null; }
            set { Application[Base.AppVars.ACCESSRULESSERVICE] = value; }
        }
        protected ServiceSchedule10.ScheduleServiceTest ScheduleServiceTest
        {
            get { return Application[Base.AppVars.SCHEDULESERVICE] != null ? (ServiceSchedule10.ScheduleServiceTest)Application[Base.AppVars.SCHEDULESERVICE] : null; }
            set { Application[Base.AppVars.SCHEDULESERVICE] = value; }
        }

        protected Media210.Media2ServiceTest Media2ServiceTest
        {
            get { return Application[Base.AppVars.MEDIA2SERVICE] != null ? (Media210.Media2ServiceTest)Application[Base.AppVars.MEDIA2SERVICE] : null; }
            set { Application[Base.AppVars.MEDIA2SERVICE] = value; }
        }

        protected Provisioning10.ProvisioningServiceTest ProvisioningServiceTest
        {
            get { return Application[Base.AppVars.PROVISIONINGSERVICE] != null ? (Provisioning10.ProvisioningServiceTest)Application[Base.AppVars.PROVISIONINGSERVICE] : null; }
            set { Application[Base.AppVars.PROVISIONINGSERVICE] = value; }
        }

        protected Thermal10.ThermalServiceTest ThermalServiceTest
        {
            get { return Application[Base.AppVars.THERMALSERVICE] != null ? (Thermal10.ThermalServiceTest)Application[Base.AppVars.THERMALSERVICE] : null; }
            set { Application[Base.AppVars.THERMALSERVICE] = value; }
        }

        protected Media2SVC.Media2SVCServiceTest Media2SVCServiceTest
        {
            get { return Application[Base.AppVars.MEDIA2SVCSERVICE] != null ? (Media2SVC.Media2SVCServiceTest)Application[Base.AppVars.MEDIA2SVCSERVICE] : null; }
            set { Application[Base.AppVars.MEDIA2SVCSERVICE] = value; }
        }

        protected ServiceAnalytics20.AnalyticsEngineServiceTest AnalyticsEngineServiceTest
        {
            get { return Application[Base.AppVars.ANALYTSERVICE] != null ? (ServiceAnalytics20.AnalyticsEngineServiceTest)Application[Base.AppVars.ANALYTSERVICE] : null; }
            set { Application[Base.AppVars.ANALYTSERVICE] = value; }
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void ShowError(Exception e)
        {
            LabelError.Visible = true;
            LabelError.Text = "Error: " + e.Message;
        }
        protected void OnMenuClick(object sender, CommandEventArgs e)
        {
            mvMain.ActiveViewIndex = (e.CommandArgument as string) == "Main" ? 1 : 0;
        }
        protected void OnLoadTestSuite(object sender, CommandEventArgs e)
        {
            LabelError.Visible = false;
            try
            {
                if (!string.IsNullOrEmpty(TestSuiteFile.FileName))
                {
                    TestCommon = new TestCommon();
                    //TestCommon.testID = TextTestCase.Text;
                    TestCommon.Title = TestSuiteFile.FileName;
                    TestCommon.LoadTestSuit(TestSuiteFile.FileContent);

                    DeviceServiceTest = new Device10.DeviceServiceTest(TestCommon);
                    DeviceIO10ServiceTest = new DeviceIO10.DeviceIO10ServiceTest(TestCommon);
                    EventServiceTest = new Events10.EventServiceTest(TestCommon);
                    MediaServiceTest = new Media10.MediaServiceTest(TestCommon);
                    ImagingService10Test = new Imaging10.ImagingService10Test(TestCommon);
                    ImagingService20Test = new Imaging20.ImagingService20Test(TestCommon);
                    PTZServiceTest = new PTZ20.PTZServiceTest(TestCommon);
                    Search10ServiceTest = new Search10.SearchServiceTest(TestCommon);
                    Recording10ServiceTest = new Recording10.RecordingServiceTest(TestCommon);
                    Replay10ServiceTest = new Replay10.ReplayServiceTest(TestCommon);
                    Receiver10ServiceTest = new DUT.CameraWebService.Receiver10.ReceiverServiceTest(TestCommon);
                    PACS10ServiceTest = new PACS10.PACSServiceTest(TestCommon);
                    Door10ServiceTest = new Door10.DoorServiceTest(TestCommon);
                    PACS11ServiceTest = new PACS11.PACSServiceTest(TestCommon);
                    Door11ServiceTest = new Door11.DoorServiceTest(TestCommon);
                    Door12ServiceTest = new DUT.CameraWebService.Door12.DoorServiceTest(TestCommon);
                    PACS12ServiceTest = new PACS12.PACSServiceTest(TestCommon);
                    AdvancedSecurity10ServiceTest = new AdvancedSecurity10.AdvancedSecurityServiceTest(TestCommon);
                    PullPointSubscriptionServiceTest = new Events10.PullPointSubscriptionServiceTest(TestCommon);
                    CredentialServiceTest = new ServiceCredential10.CredentialServiceTest(TestCommon);
                    AccessRulesServiceTest = new ServiceAccessRules10.AccessRulesServiceTest(TestCommon);
                    ScheduleServiceTest = new ServiceSchedule10.ScheduleServiceTest(TestCommon);
                    Media2ServiceTest = new Media210.Media2ServiceTest(TestCommon);
                    Media2SVCServiceTest = new Media2SVC.Media2SVCServiceTest(TestCommon);
                    AnalyticsEngineServiceTest = new ServiceAnalytics20.AnalyticsEngineServiceTest(TestCommon);
                    ProvisioningServiceTest = new Provisioning10.ProvisioningServiceTest(TestCommon);
                    ThermalServiceTest = new Thermal10.ThermalServiceTest(TestCommon);

                    Reset();

                    //List<string> tests = GetTests(TestCommon.m_testSuitXML);
                    ListTestCases.DataSource = TestCommon.TestList2;
                    ListTestCases.DataBind();
                }
            }
            catch (System.Exception ex)
            {
                ShowError(ex);
                TestCommon = null;
            }
            TestCommon.testResult = "";
            TestCommon.testSummaryResult = true;
            TestCommon.testStepsCompleted = false;
            TestCommon.previousStepNumber = 0;
        }
        protected void OnLoadTestCase(object sender, CommandEventArgs e)
        {
            if(TestCommon != null)
            {
                TestCommon.testID = ListTestCases.Text;
                LabelTestName.Text = TestCommon.GetCurrentTestName();
                LabelTestDescription.Text = TestCommon.GetCurrentTestDescription().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br/>");
                LabelExpectedResult.Text = TestCommon.GetCurrentExpectedResult().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br/>");
                Reset();
            }
        }
        protected void Reset()
        {
            if (DeviceServiceTest != null)
            {
                DeviceServiceTest.ResetTestSuit();
            }

            if (Search10ServiceTest != null)
            {
                Search10ServiceTest.ResetTestSuit();
            }

            if (Recording10ServiceTest != null)
            {
                Recording10ServiceTest.ResetTestSuit();
            }

            if (Receiver10ServiceTest != null)
            {
                Receiver10ServiceTest.ResetTestSuit();
            }

            if (Receiver10ServiceTest != null)
            {
                Receiver10ServiceTest.ResetTestSuit();
            }

            if (Door12ServiceTest != null)
            {
                Door12ServiceTest.ResetTestSuit();
            }

            if (PACS12ServiceTest != null)
            {
                PACS12ServiceTest.ResetTestSuit();
            }

            if (Replay10ServiceTest != null)
            {
                Replay10ServiceTest.ResetTestSuit();
            }

            if (EventServiceTest != null)
            {
                EventServiceTest.ResetTestSuit();
            }
            if (MediaServiceTest != null)
            {
                MediaServiceTest.ResetTestSuit();
            }
            if (PTZServiceTest != null)
            {
                PTZServiceTest.ResetTestSuit();
            }
            if (ImagingService10Test != null)
            {
                ImagingService10Test.ResetTestSuit();
            }
            if (ImagingService20Test != null)
            {
                ImagingService20Test.ResetTestSuit();
            }
            if (DeviceIO10ServiceTest != null)
            {
                DeviceIO10ServiceTest.ResetTestSuit();
            }
            if (PACS10ServiceTest != null)
            {
                PACS10ServiceTest.ResetTestSuit();
            }
            if (Door10ServiceTest != null)
            {
                Door10ServiceTest.ResetTestSuit();
            }
            if (PACS11ServiceTest != null)
            {
                PACS11ServiceTest.ResetTestSuit();
            }
            if (Door11ServiceTest != null)
            {
                Door11ServiceTest.ResetTestSuit();
            }
            if (AdvancedSecurity10ServiceTest != null)
            {
                AdvancedSecurity10ServiceTest.ResetTestSuit();
            }
            if (PullPointSubscriptionServiceTest != null)
            {
                PullPointSubscriptionServiceTest.ResetTestSuit();
            }
            if (CredentialServiceTest != null)
            {
                CredentialServiceTest.ResetTestSuit();
            }
            if (AccessRulesServiceTest != null)
            {
                AccessRulesServiceTest.ResetTestSuit();
            }
            if (ScheduleServiceTest != null)
            {
                ScheduleServiceTest.ResetTestSuit();
            }
            if (Media2ServiceTest != null)
            {
                Media2ServiceTest.ResetTestSuit();
            }
            if (Media2SVCServiceTest != null)
            {
                Media2SVCServiceTest.ResetTestSuit();
            }
            if (AnalyticsEngineServiceTest != null)
            {
                AnalyticsEngineServiceTest.ResetTestSuit();
            }
            if (ProvisioningServiceTest != null)
            {
                ProvisioningServiceTest.ResetTestSuit();
            }
            if (ThermalServiceTest != null)
            {
                ThermalServiceTest.ResetTestSuit();
            }

            TestCommon.testResult = "";
            TestCommon.testSummaryResult = true;
            TestCommon.testStepsCompleted = false;
            TestCommon.previousStepNumber = 0;
        }
        protected void OnReset(object sender, CommandEventArgs e)
        {
            try
            {
                Reset();
            }
            catch (System.Exception ex)
            {
                ShowError(ex);
            }
        }
        protected List<string> GetTests(XmlDocument testSuite)
        {
            List<string> tests = new List<string>();
            XmlNodeList nodes = testSuite.SelectNodes("/TestSuit/Test");
            foreach (XmlNode node in nodes)
            {
                tests.Add(node.Attributes["ID"].Value);
            }
            return tests;
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if(TestCommon != null)
            {
                LabelTestSuite.Text = TestCommon.Title;
                LabelTestCase.Text = string.IsNullOrEmpty(TestCommon.testID) ? "Not loaded" : TestCommon.testID;
            }
            else
            {
                LabelTestSuite.Text = "Not loaded";
                LabelTestCase.Text = "Not loaded";
            }
        }

        protected void OnGetCurrentResult(object sender, CommandEventArgs e)
        {
            if (TestCommon != null)
            {
                if (TestCommon.testSummaryResult)
                {
                    LabelCurrentResult.Text = "Summary result: PASSED <br/><br/>";
                }
                else
                {
                    LabelCurrentResult.Text = "Summary result: <font color=\"red\"><b>FAILED</b></font> <br/>(At least one step was failed or there was wrong step sequence.)<br/><br/>";
                }
                LabelCurrentResult.Text += TestCommon.testResult.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br/>").Replace("FAILED","<font color=\"red\"><b>FAILED</font></b>");
            }
        }

        protected void OnClearAuth(object sender, CommandEventArgs e)
        {
            if (TestCommon != null)
            {
                TestCommon.ClearAuthList();
            }
        }

        protected void OnClearCredentials(object sender, CommandEventArgs e)
        {
            if (TestCommon != null)
            {
                TestCommon.ClearAllUserCredentials();
            }
        }
        
        protected void OnClearTestList(object sender, CommandEventArgs e)
        {
            LabelCurrentResult.Text = "";
        }

        protected void OnGetTestList(object sender, CommandEventArgs e)
        {
            if (TestCommon != null)
            {
                foreach (string i in TestCommon.TestList)
                {
                    LabelCurrentResult.Text += i.Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>";
                }
                CheckListExcelCreator excelList = new CheckListExcelCreator(TicketID.Text);//create check list report
                TestReportCreated.Text = excelList.FileRedyForWriting ? 
                    TicketID.Text + excelList.CreateReport(TestCommon.TestList) : "Cannot create report, " + excelList.errorMessage; 
            }

        }    
        protected void OnSwitchMedia2Service(object sender, EventArgs e)
        {
            switchToNewMedia2Service = !switchToNewMedia2Service;
            if (switchToNewMedia2Service == true)
            {
                LabelSwitchMedia2Service.Text = "New Media2SVC Service is enabled";
            }
            else
            {
                LabelSwitchMedia2Service.Text = "Old Media2 Service is enabled";
            }
        }
    }
}
