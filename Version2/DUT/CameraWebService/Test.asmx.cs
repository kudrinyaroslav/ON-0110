using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace DUT.CameraWebService
{
    /// <summary>
    /// Summary description for Test
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Test : System.Web.Services.WebService
    {
        //TestSuit
        TestCommon m_TestCommon = null;
        Device10.DeviceServiceTest m_DeviceServiceTest = null;
        DeviceIO10.DeviceIO10ServiceTest m_DeviceIO10ServiceTest = null;
        Events10.EventServiceTest m_EventServiceTest = null;
        Media10.MediaServiceTest m_MediaServiceTest = null;
        Imaging10.ImagingService10Test m_ImagingService10Test = null;
        Imaging20.ImagingService20Test m_ImagingService20Test = null;
        PTZ20.PTZServiceTest m_PTZServiceTest = null;
        Receiver10.ReceiverServiceTest m_ReceiverServiceTest = null;
        Recording10.RecordingServiceTest m_RecordingServiceTest = null;
        Replay10.ReplayServiceTest m_ReplayServiceTest = null;
        Search10.SearchServiceTest m_SearchServiceTest = null;
        Door12.DoorServiceTest m_DoorServiceTest = null;
        PACS12.PACSServiceTest  m_PacsServiceTest = null;
        AdvancedSecurity10.AdvancedSecurityServiceTest m_AdvancedSecurityServiceTest = null;
        Events10.PullPointSubscriptionServiceTest m_PullPointSubscriptionService2Test = null;
        ServiceCredential10.CredentialServiceTest m_CredentialServiceTest = null;
        ServiceAccessRules10.AccessRulesServiceTest m_AccessRulesServiceTest = null;
        ServiceSchedule10.ScheduleServiceTest m_ScheduleServiceTest = null;
        Media210.Media2ServiceTest m_Media2ServiceTest = null;
        Media2SVC.Media2SVCServiceTest m_Media2SVCServiceTest = null;
        ServiceAnalytics20.AnalyticsEngineServiceTest m_AnalyticsEngineServiceTest = null;
        Provisioning10.ProvisioningServiceTest m_ProvisioningServiceTest = null;
        Thermal10.ThermalServiceTest m_ThermalServiceTest = null;


        private void TestInit()
        {
            if (Application["m_TestCommon"] != null)
            {
                m_TestCommon = (TestCommon)Application["m_TestCommon"];
            }
            else
            {
                m_TestCommon = new TestCommon();
                m_TestCommon.LoadTestSuit();
                Application["m_TestCommon"] = m_TestCommon;
            }

            if (Application["m_DeviceServiceTest"] != null)
            {
                m_DeviceServiceTest = (Device10.DeviceServiceTest)Application["m_DeviceServiceTest"];
            }
            else
            {
                m_DeviceServiceTest = new Device10.DeviceServiceTest(m_TestCommon);
                Application["m_DeviceServiceTest"] = m_DeviceServiceTest;
            }

            if (Application[Base.AppVars.REPLAYSERVICE] != null)
            {
                m_ReplayServiceTest = (Replay10.ReplayServiceTest)Application[Base.AppVars.REPLAYSERVICE];
            }
            else
            {
                m_ReplayServiceTest = new Replay10.ReplayServiceTest(m_TestCommon);
                Application[Base.AppVars.REPLAYSERVICE] = m_ReplayServiceTest;
            }

            if (Application["m_DeviceIO10ServiceTest"] != null)
            {
                m_DeviceIO10ServiceTest = (DeviceIO10.DeviceIO10ServiceTest)Application["m_DeviceIO10ServiceTest"];
            }
            else
            {
                m_DeviceIO10ServiceTest = new DeviceIO10.DeviceIO10ServiceTest(m_TestCommon);
                Application["m_DeviceIO10ServiceTest"] = m_DeviceIO10ServiceTest;
            }

            if (Application["m_EventServiceTest"] != null)
            {
                m_EventServiceTest = (Events10.EventServiceTest)Application["m_EventServiceTest"];
            }
            else
            {
                m_EventServiceTest = new Events10.EventServiceTest(m_TestCommon);
                Application["m_EventServiceTest"] = m_EventServiceTest;
            }

            if (Application["m_MediaServiceTest"] != null)
            {
                m_MediaServiceTest = (Media10.MediaServiceTest)Application["m_MediaServiceTest"];
            }
            else
            {
                m_MediaServiceTest = new Media10.MediaServiceTest(m_TestCommon);
                Application["m_MediaServiceTest"] = m_MediaServiceTest;
            }

            if (Application["m_ImagingService10Test"] != null)
            {
                m_ImagingService10Test = (Imaging10.ImagingService10Test)Application["m_ImagingService10Test"];
            }
            else
            {
                m_ImagingService10Test = new Imaging10.ImagingService10Test(m_TestCommon);
                Application["m_ImagingService10Test"] = m_ImagingService10Test;
            }

            if (Application["m_ImagingService20Test"] != null)
            {
                m_ImagingService20Test = (Imaging20.ImagingService20Test)Application["m_ImagingService20Test"];
            }
            else
            {
                m_ImagingService20Test = new Imaging20.ImagingService20Test(m_TestCommon);
                Application["m_ImagingService20Test"] = m_ImagingService20Test;
            }

            if (Application["m_PTZServiceTest"] != null)
            {
                m_PTZServiceTest = (PTZ20.PTZServiceTest)Application["m_PTZServiceTest"];
            }
            else
            {
                m_PTZServiceTest = new PTZ20.PTZServiceTest(m_TestCommon);
                Application["m_PTZServiceTest"] = m_PTZServiceTest;
            }
            if (Application["m_PTZServiceTest"] != null)
            {
                m_PTZServiceTest = (PTZ20.PTZServiceTest)Application["m_PTZServiceTest"];
            }
            else
            {
                m_PTZServiceTest = new PTZ20.PTZServiceTest(m_TestCommon);
                Application["m_PTZServiceTest"] = m_PTZServiceTest;
            }
            if (Application["m_RecordingServiceTest"] != null)
            {
                m_RecordingServiceTest = (Recording10.RecordingServiceTest)Application["m_RecordingServiceTest"];
            }
            else
            {
                m_RecordingServiceTest = new Recording10.RecordingServiceTest(m_TestCommon);
                Application["m_RecordingServiceTest"] = m_RecordingServiceTest;
            }
            if (Application["m_ReceiverServiceTest"] != null)
            {
                m_ReceiverServiceTest = (Receiver10.ReceiverServiceTest)Application["m_ReceiverServiceTest"];
            }
            else
            {
                m_ReceiverServiceTest = new Receiver10.ReceiverServiceTest(m_TestCommon);
                Application["m_ReceiverServiceTest"] = m_ReceiverServiceTest;
            }
            if (Application["m_SearchServiceTest"] != null)
            {
                m_SearchServiceTest = (Search10.SearchServiceTest)Application["m_SearchServiceTest"];
            }
            else
            {
                m_SearchServiceTest = new Search10.SearchServiceTest(m_TestCommon);
                Application["m_SearchServiceTest"] = m_SearchServiceTest;
            }

            if (Application[Base.AppVars.DOORSERVICE] != null)
            {
                m_DoorServiceTest = (Door12.DoorServiceTest)Application[Base.AppVars.DOORSERVICE];
            }
            else
            {
                m_DoorServiceTest = new Door12.DoorServiceTest(m_TestCommon);
                Application[Base.AppVars.DOORSERVICE] = m_DoorServiceTest;
            }

            if (Application[Base.AppVars.PACSSERVICE] != null)
            {
                m_PacsServiceTest = (PACS12.PACSServiceTest)Application[Base.AppVars.PACSSERVICE];
            }
            else
            {
                m_PacsServiceTest = new PACS12.PACSServiceTest(m_TestCommon);
                Application[Base.AppVars.PACSSERVICE] = m_PacsServiceTest;
            }

            if (Application[Base.AppVars.ADVSECSERVICE] != null)
            {
                m_AdvancedSecurityServiceTest = (AdvancedSecurity10.AdvancedSecurityServiceTest)Application[Base.AppVars.ADVSECSERVICE];
            }
            else
            {
                m_AdvancedSecurityServiceTest = new AdvancedSecurity10.AdvancedSecurityServiceTest(m_TestCommon);
                Application[Base.AppVars.ADVSECSERVICE] = m_AdvancedSecurityServiceTest;
            }
            if (Application[Base.AppVars.MEDIA2SERVICE] != null)
            {
                m_Media2ServiceTest = (Media210.Media2ServiceTest)Application[Base.AppVars.MEDIA2SERVICE];
            }
            else
            {
                m_Media2ServiceTest = new Media210.Media2ServiceTest(m_TestCommon);
                Application[Base.AppVars.MEDIA2SERVICE] = m_Media2ServiceTest;
            }

            if (Application[Base.AppVars.EVENTPULLPOINTSERVICE] != null)
            {
                m_PullPointSubscriptionService2Test = (Events10.PullPointSubscriptionServiceTest)Application[Base.AppVars.EVENTPULLPOINTSERVICE];
            }
            else
            {
                m_PullPointSubscriptionService2Test = new Events10.PullPointSubscriptionServiceTest(m_TestCommon);
                Application[Base.AppVars.EVENTPULLPOINTSERVICE] = m_PullPointSubscriptionService2Test;
            }

            if (Application[Base.AppVars.CREDENTIALSERVICE] != null)
            {
                m_CredentialServiceTest = (ServiceCredential10.CredentialServiceTest)Application[Base.AppVars.CREDENTIALSERVICE];
            }
            else
            {
                m_CredentialServiceTest = new ServiceCredential10.CredentialServiceTest(m_TestCommon);
                Application[Base.AppVars.CREDENTIALSERVICE] = m_CredentialServiceTest;
            }

            if (Application[Base.AppVars.ACCESSRULESSERVICE] != null)
            {
                m_AccessRulesServiceTest = (ServiceAccessRules10.AccessRulesServiceTest)Application[Base.AppVars.ACCESSRULESSERVICE];
            }
            else
            {
                m_AccessRulesServiceTest = new ServiceAccessRules10.AccessRulesServiceTest(m_TestCommon);
                Application[Base.AppVars.ACCESSRULESSERVICE] = m_AccessRulesServiceTest;
            }
            if (Application[Base.AppVars.SCHEDULESERVICE] != null)
            {
                m_ScheduleServiceTest = (ServiceSchedule10.ScheduleServiceTest)Application[Base.AppVars.SCHEDULESERVICE];
            }
            else
            {
                m_ScheduleServiceTest = new ServiceSchedule10.ScheduleServiceTest(m_TestCommon);
                Application[Base.AppVars.SCHEDULESERVICE] = m_ScheduleServiceTest;
            }
            if (Application[Base.AppVars.MEDIA2SVCSERVICE] != null)
            {
                m_Media2SVCServiceTest = (Media2SVC.Media2SVCServiceTest)Application[Base.AppVars.MEDIA2SVCSERVICE];
            }
            else
            {
                m_Media2SVCServiceTest = new Media2SVC.Media2SVCServiceTest(m_TestCommon);
                Application[Base.AppVars.MEDIA2SVCSERVICE] = m_Media2SVCServiceTest;
            }
            if (Application[Base.AppVars.ANALYTSERVICE] != null)
            {
                m_AnalyticsEngineServiceTest = (ServiceAnalytics20.AnalyticsEngineServiceTest)Application[Base.AppVars.ANALYTSERVICE];
            }
            else
            {
                m_AnalyticsEngineServiceTest = new ServiceAnalytics20.AnalyticsEngineServiceTest(m_TestCommon);
                Application[Base.AppVars.ANALYTSERVICE] = m_AnalyticsEngineServiceTest;
            }
            if (Application[Base.AppVars.PROVISIONINGSERVICE] != null)
            {
                m_ProvisioningServiceTest = (Provisioning10.ProvisioningServiceTest)Application[Base.AppVars.PROVISIONINGSERVICE];
            }
            else
            {
                m_ProvisioningServiceTest = new Provisioning10.ProvisioningServiceTest(m_TestCommon);
                Application[Base.AppVars.PROVISIONINGSERVICE] = m_ProvisioningServiceTest;
            }
            if (Application[Base.AppVars.THERMALSERVICE] != null)
            {
                m_ThermalServiceTest = (Thermal10.ThermalServiceTest)Application[Base.AppVars.THERMALSERVICE];
            }
            else
            {
                m_ThermalServiceTest = new Thermal10.ThermalServiceTest(m_TestCommon);
                Application[Base.AppVars.THERMALSERVICE] = m_ThermalServiceTest;
            }
        }

        [WebMethod]
        public string GetInitialPoint()
        {
            m_TestCommon = new TestCommon();
            Application["m_TestCommon"] = m_TestCommon;
            m_TestCommon.LoadTestSuit();

            m_DeviceServiceTest = new Device10.DeviceServiceTest(m_TestCommon);
            Application["m_DeviceServiceTest"] = m_DeviceServiceTest;

            m_DeviceIO10ServiceTest = new DeviceIO10.DeviceIO10ServiceTest(m_TestCommon);
            Application["m_DeviceIO10ServiceTest"] = m_DeviceIO10ServiceTest;
            
            m_EventServiceTest = new Events10.EventServiceTest(m_TestCommon);
            Application["m_EventServiceTest"] = m_EventServiceTest;
            
            m_MediaServiceTest = new Media10.MediaServiceTest(m_TestCommon);
            Application["m_MediaServiceTest"] = m_MediaServiceTest;

            m_ImagingService10Test = new Imaging10.ImagingService10Test(m_TestCommon);
            Application["m_ImagingService10Test"] = m_ImagingService10Test;

            m_ImagingService20Test = new Imaging20.ImagingService20Test(m_TestCommon);
            Application["m_ImagingService20Test"] = m_ImagingService20Test;

            m_PTZServiceTest = new PTZ20.PTZServiceTest(m_TestCommon);
            Application["m_PTZServiceTest"] = m_PTZServiceTest;

            m_RecordingServiceTest = new Recording10.RecordingServiceTest(m_TestCommon);
            Application["m_RecordingServiceTest"] = m_RecordingServiceTest;

            m_ReceiverServiceTest = new Receiver10.ReceiverServiceTest(m_TestCommon);
            Application["m_ReceiverServiceTest"] = m_ReceiverServiceTest;

            m_SearchServiceTest = new Search10.SearchServiceTest(m_TestCommon);
            Application["m_SearchServiceTest"] = m_SearchServiceTest;

            m_ReplayServiceTest = new Replay10.ReplayServiceTest(m_TestCommon);
            Application[Base.AppVars.REPLAYSERVICE] = m_ReplayServiceTest;

            m_AdvancedSecurityServiceTest = new AdvancedSecurity10.AdvancedSecurityServiceTest(m_TestCommon);
            Application[Base.AppVars.ADVSECSERVICE] = m_AdvancedSecurityServiceTest;

            m_Media2ServiceTest = new Media210.Media2ServiceTest(m_TestCommon);
            Application[Base.AppVars.MEDIA2SERVICE] = m_Media2ServiceTest;

            m_DoorServiceTest = new Door12.DoorServiceTest(m_TestCommon);
            Application[Base.AppVars.DOORSERVICE] = m_DoorServiceTest;

            m_PacsServiceTest = new PACS12.PACSServiceTest(m_TestCommon);
            Application[Base.AppVars.PACSSERVICE] = m_PacsServiceTest;

            m_PullPointSubscriptionService2Test = new Events10.PullPointSubscriptionServiceTest(m_TestCommon);
            Application[Base.AppVars.EVENTPULLPOINTSERVICE] = m_PullPointSubscriptionService2Test;

            m_AccessRulesServiceTest = new ServiceAccessRules10.AccessRulesServiceTest(m_TestCommon);
            Application[Base.AppVars.ACCESSRULESSERVICE] = m_AccessRulesServiceTest;

            m_CredentialServiceTest = new ServiceCredential10.CredentialServiceTest(m_TestCommon);
            Application[Base.AppVars.CREDENTIALSERVICE] = m_CredentialServiceTest;

            m_ScheduleServiceTest = new ServiceSchedule10.ScheduleServiceTest(m_TestCommon);
            Application[Base.AppVars.SCHEDULESERVICE] = m_ScheduleServiceTest;

            m_AnalyticsEngineServiceTest = new ServiceAnalytics20.AnalyticsEngineServiceTest(m_TestCommon);
            Application[Base.AppVars.ANALYTSERVICE] = m_AnalyticsEngineServiceTest;

            m_ProvisioningServiceTest = new Provisioning10.ProvisioningServiceTest(m_TestCommon);
            Application[Base.AppVars.PROVISIONINGSERVICE] = m_ProvisioningServiceTest;

            m_ThermalServiceTest = new Thermal10.ThermalServiceTest(m_TestCommon);
            Application[Base.AppVars.THERMALSERVICE] = m_ThermalServiceTest;

            return m_TestCommon.DeviceUri;
        }

        [WebMethod]
        public void LoadTestSuit(string PathToTestSuit)
        {
            m_TestCommon = new TestCommon();
            Application["m_TestCommon"] = m_TestCommon;
            TestCommon.pathToTestSuit = PathToTestSuit;
            TestCommon.testID = "";
            m_TestCommon.LoadTestSuit();

            m_DeviceServiceTest = new Device10.DeviceServiceTest(m_TestCommon);
            Application["m_DeviceServiceTest"] = m_DeviceServiceTest;
            m_DeviceIO10ServiceTest = new DeviceIO10.DeviceIO10ServiceTest(m_TestCommon);
            Application["m_DeviceIO10ServiceTest"] = m_DeviceIO10ServiceTest;
            m_EventServiceTest = new Events10.EventServiceTest(m_TestCommon);
            Application["m_EventServiceTest"] = m_EventServiceTest;
            m_MediaServiceTest = new Media10.MediaServiceTest(m_TestCommon);
            Application["m_MediaServiceTest"] = m_MediaServiceTest;
            m_ImagingService10Test = new Imaging10.ImagingService10Test(m_TestCommon);
            Application["m_ImagingService10Test"] = m_ImagingService10Test;
            m_ImagingService20Test = new Imaging20.ImagingService20Test(m_TestCommon);
            Application["m_ImagingService20Test"] = m_ImagingService20Test;
            m_PTZServiceTest = new PTZ20.PTZServiceTest(m_TestCommon);
            Application["m_PTZServiceTest"] = m_PTZServiceTest;
            m_RecordingServiceTest = new Recording10.RecordingServiceTest(m_TestCommon);
            Application["m_RecordingServiceTest"] = m_RecordingServiceTest;

            m_ReceiverServiceTest = new Receiver10.ReceiverServiceTest(m_TestCommon);
            Application["m_ReceiverServiceTest"] = m_ReceiverServiceTest;

            m_SearchServiceTest = new Search10.SearchServiceTest(m_TestCommon);
            Application["m_SearchServiceTest"] = m_SearchServiceTest;

            m_DoorServiceTest  = new Door12.DoorServiceTest(m_TestCommon);
            Application[Base.AppVars.DOORSERVICE] = m_DoorServiceTest;
            
            m_PacsServiceTest = new PACS12.PACSServiceTest(m_TestCommon);
            Application[Base.AppVars.PACSSERVICE] = m_PacsServiceTest;

            m_ReplayServiceTest = new Replay10.ReplayServiceTest(m_TestCommon);
            Application[Base.AppVars.REPLAYSERVICE] = m_ReplayServiceTest;

            m_AdvancedSecurityServiceTest = new AdvancedSecurity10.AdvancedSecurityServiceTest(m_TestCommon);
            Application[Base.AppVars.ADVSECSERVICE] = m_AdvancedSecurityServiceTest;

            m_Media2ServiceTest = new Media210.Media2ServiceTest(m_TestCommon);
            Application[Base.AppVars.MEDIA2SERVICE] = m_Media2ServiceTest;

            m_PullPointSubscriptionService2Test = new Events10.PullPointSubscriptionServiceTest(m_TestCommon);
            Application[Base.AppVars.EVENTPULLPOINTSERVICE] = m_PullPointSubscriptionService2Test;

            m_AccessRulesServiceTest = new ServiceAccessRules10.AccessRulesServiceTest(m_TestCommon);
            Application[Base.AppVars.ACCESSRULESSERVICE] = m_AccessRulesServiceTest;

            m_CredentialServiceTest = new ServiceCredential10.CredentialServiceTest(m_TestCommon);
            Application[Base.AppVars.CREDENTIALSERVICE] = m_CredentialServiceTest;

            m_ScheduleServiceTest = new ServiceSchedule10.ScheduleServiceTest(m_TestCommon);
            Application[Base.AppVars.SCHEDULESERVICE] = m_ScheduleServiceTest;

            m_Media2SVCServiceTest = new Media2SVC.Media2SVCServiceTest(m_TestCommon);
            Application[Base.AppVars.MEDIA2SVCSERVICE] = m_Media2SVCServiceTest;

            m_AnalyticsEngineServiceTest = new ServiceAnalytics20.AnalyticsEngineServiceTest(m_TestCommon);
            Application[Base.AppVars.ANALYTSERVICE] = m_AnalyticsEngineServiceTest;

            m_ProvisioningServiceTest = new Provisioning10.ProvisioningServiceTest(m_TestCommon);
            Application[Base.AppVars.PROVISIONINGSERVICE] = m_ProvisioningServiceTest;

            m_ThermalServiceTest = new Thermal10.ThermalServiceTest(m_TestCommon);
            Application[Base.AppVars.THERMALSERVICE] = m_ThermalServiceTest;
        }

        [WebMethod]
        public void SelectTestCase(string testID)
        {
            TestInit();
            ResetTestSuit();
            TestCommon.testID = testID;
        }

        [WebMethod]
        public void ResetTestSuit()
        {
            TestInit();
            TestCommon.testResult = string.Empty;
            TestCommon.testSummaryResult = true;
            TestCommon.previousStepNumber = 0;
            TestCommon.testStepsCompleted = false;
            m_DeviceServiceTest.ResetTestSuit();
            m_DeviceIO10ServiceTest.ResetTestSuit();
            m_EventServiceTest.ResetTestSuit();
            m_MediaServiceTest.ResetTestSuit();
            m_ImagingService10Test.ResetTestSuit();
            m_ImagingService20Test.ResetTestSuit();
            m_PTZServiceTest.ResetTestSuit();
            m_ReceiverServiceTest.ResetTestSuit();
            m_RecordingServiceTest.ResetTestSuit();
            m_SearchServiceTest.ResetTestSuit();
            m_DoorServiceTest.ResetTestSuit();
            m_PacsServiceTest.ResetTestSuit();
            m_ReplayServiceTest.ResetTestSuit();
            m_AdvancedSecurityServiceTest.ResetTestSuit();
            m_PullPointSubscriptionService2Test.ResetTestSuit();
            m_CredentialServiceTest.ResetTestSuit();
            m_AccessRulesServiceTest.ResetTestSuit();
            m_ScheduleServiceTest.ResetTestSuit();
            m_Media2ServiceTest.ResetTestSuit();
            m_Media2SVCServiceTest.ResetTestSuit();
            m_AnalyticsEngineServiceTest.ResetTestSuit();
            m_ProvisioningServiceTest.ResetTestSuit();
            m_ThermalServiceTest.ResetTestSuit();
        }

        [WebMethod]
        public string[] GetTestList()
        {
            TestInit();
            return m_TestCommon.TestList;
        }

        [WebMethod]
        public string GetTestResult()
        {
            return TestCommon.testResult;
        }

        [WebMethod]
        public bool GetTestSummaryResult()
        {
            TestInit();
            if (m_TestCommon == null)
            {
                return false;
            }
            else
            {
                return TestCommon.testStepsCompleted&&TestCommon.testSummaryResult;
            }
        }

        [WebMethod]
        public string GetONVIFTestExpectedResult()
        {
            TestInit();
            if (m_TestCommon == null)
            {
                return "NONE";
            }
            else
            {
                return m_TestCommon.GetONVIFTestExpectedResult();
            }
        }

        [WebMethod]
        public string GetTestDescription()
        {
            TestInit();
            if (m_TestCommon == null)
            {
                return "None";
            }
            else
            {
                return m_TestCommon.GetCurrentTestDescription();
            }
        }

        [WebMethod]
        public string GetTestName()
        {
            TestInit();
            if (m_TestCommon == null)
            {
                return "None";
            }
            else
            {
                return m_TestCommon.GetCurrentTestName();
            }
        }

        [WebMethod]
        public string GetTestExpectedResult()
        {
            TestInit();
            if (m_TestCommon == null)
            {
                return "None";
            }
            else
            {
                return m_TestCommon.GetCurrentExpectedResult();
            }
        }

    }
}
