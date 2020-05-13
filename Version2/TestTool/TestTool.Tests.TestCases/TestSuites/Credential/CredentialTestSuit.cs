using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.TestSuites.Events;
using TestTool.Tests.TestCases.TestSuites.Recording;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.OnvifServices;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;
using TestTool.Tests.Definitions.Interfaces;
using System.ServiceModel;
using TestTool.Proxies.Event;
using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using TestTool.Tests.Common.TestBase;

namespace TestTool.Tests.TestCases.TestSuites.Credential_
{
#if __PROFILE_A__
    [TestClass]
#else
#endif
    partial class CredentialTestSuit : BaseOnvifTest, IDeviceService, ICredentialService, IAccessRulesService, IEventService, IScheduleService, IAccessControlService
    {
        #region internal
        private const string PATH_GENERAL = "Credential";
        private const string PATH_EVENTS = PATH_GENERAL + @"\Events";
        private const string PATH_CAPABILITIES = PATH_GENERAL + @"\Capabilities";
        private const string PATH_CREDENTIAL_INFO = PATH_GENERAL + @"\Credential Info";
        private const string PATH_CREDENTIAL = PATH_GENERAL + @"\Credential";
        private const string PATH_CREDENTIAL_STATE = PATH_GENERAL + @"\Credential State";
        private const string PATH_CREDENTIAL_ACCESS_PROFILES = PATH_GENERAL + @"\Credential Access Profiles";
        private const string PATH_CREDENTIAL_RESET_ANTIPASSBACK_VIOLATIONS = PATH_GENERAL + @"\Reset Antipassback Violations";
        private const string PATH_CREDENTIAL_IDENTIFIERS = PATH_GENERAL + @"\Credential Identifiers";
        private const string PATH_CONSISTENCY = PATH_GENERAL + @"\Consistency";
        private const string PATH_CREDENTIAL_EVENTS = PATH_GENERAL + @"\Credential Events";


        public CredentialTestSuit(TestLaunchParam param)
            : base(param)
        { }

        private OnvifServiceClient<CredentialPort, CredentialPortClient> m_CredentialServiceClient;
        OnvifServiceClient<CredentialPort, CredentialPortClient> IBaseOnvifService2<CredentialPort, CredentialPortClient>.ServiceClient
        {
            get
            {
                if (!m_CredentialServiceClient.IsInitialized())
                {
                    m_CredentialServiceClient = new OnvifServiceClient<CredentialPort, CredentialPortClient>(this, "Credential", this.GetCredentialServiceAddress);
                    m_CredentialServiceClient.InitServiceClient(new[] { new SoapValidator(CredentialSchemaSet.GetInstance()) });
                }

                return m_CredentialServiceClient;
            }
        }

        private OnvifServiceClient<AccessRulesPort, AccessRulesPortClient> m_AccessRulesServiceClient;
        OnvifServiceClient<AccessRulesPort, AccessRulesPortClient> IBaseOnvifService2<AccessRulesPort, AccessRulesPortClient>.ServiceClient
        {
            get
            {
                if (!m_AccessRulesServiceClient.IsInitialized())
                {
                    m_AccessRulesServiceClient = new OnvifServiceClient<AccessRulesPort, AccessRulesPortClient>(this, "Access Rules", this.GetAccessRulesServiceAddress);
                    m_AccessRulesServiceClient.InitServiceClient(new[] { new SoapValidator(AccessRulesSchemaSet.GetInstance()) });
                }

                return m_AccessRulesServiceClient;
            }
        }

        private OnvifServiceClient<SchedulePort, SchedulePortClient> m_ScheduleServiceClient;
        OnvifServiceClient<SchedulePort, SchedulePortClient> IBaseOnvifService2<SchedulePort, SchedulePortClient>.ServiceClient
        {
            get
            {
                if (!m_ScheduleServiceClient.IsInitialized())
                {
                    m_ScheduleServiceClient = new OnvifServiceClient<SchedulePort, SchedulePortClient>(this, "Schedule", this.GetScheduleServiceAddress);
                    m_ScheduleServiceClient.InitServiceClient(new[] { new SoapValidator(ScheduleSchemaSet.GetInstance()) });
                }

                return m_ScheduleServiceClient;
            }
        }

        private OnvifServiceClient<EventPortType, EventPortTypeClient> m_EventServiceClient;
        OnvifServiceClient<EventPortType, EventPortTypeClient> IBaseOnvifService2<EventPortType, EventPortTypeClient>.ServiceClient
        {
            get
            {
                if (!m_EventServiceClient.IsInitialized())
                {
                    m_EventServiceClient = new OnvifServiceClient<EventPortType, EventPortTypeClient>(this, "Event", this.GetEventServiceAddress);
                    m_EventServiceClient.InitServiceClient(new[] { new SoapValidator(EventsSchemasSet.GetInstance()) });
                }

                return m_EventServiceClient;
            }
        }

        private OnvifServiceClient<PACSPort, PACSPortClient> m_AccessControlServiceClient;
        OnvifServiceClient<PACSPort, PACSPortClient> IBaseOnvifService2<PACSPort, PACSPortClient>.ServiceClient
        {
            get
            {
                if (!m_AccessControlServiceClient.IsInitialized())
                {
                    m_AccessControlServiceClient = new OnvifServiceClient<PACSPort, PACSPortClient>(this, "Access Control", this.GetAccessControlServiceAddress);
                    m_AccessControlServiceClient.InitServiceClient(new[] { new SoapValidator(AccessControlSchemaSet.GetInstance()) });
                }

                return m_AccessControlServiceClient;
            }
        }

        private OnvifServiceClient<Device, DeviceClient> m_DeviceServiceClient;
        public OnvifServiceClient<Device, DeviceClient> ServiceClient
        {
            get
            {
                if (!m_DeviceServiceClient.IsInitialized())
                {
                    m_DeviceServiceClient = new OnvifServiceClient<Device, DeviceClient>(this, "Device", this.GetDeviceServiceAddress);
                    m_DeviceServiceClient.InitServiceClient(new[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });
                }

                return m_DeviceServiceClient;
            }
        }

        public BaseOnvifTest Test { get { return this; } }

        #endregion

        #region 1-1-*
        [Test(Name = "CREDENTIAL SERVICE CAPABILITIES",
            Id = "1-1-1",
            Category = Category.CREDENTIAL,
            Path = PATH_CAPABILITIES,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialServiceCapabilities })]
        public void AccessRulesServiceCapabilitiesTest()
        {
            RunTest(() => (this as ICredentialService).GetServiceCapabilities());
        }

        [Test(Name = "GET SERVICES AND GET CREDENTIAL SERVICE CAPABILITIES CONSISTENCY",
            Id = "1-1-2",
            Category = Category.CREDENTIAL,
            Path = PATH_CAPABILITIES,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialServiceCapabilities, Functionality.GetServices })]
        public void GetServicesAndGetAccessRulesServiceCapabilitiesConsistencyTest()
        {
            RunTest(() =>
                    {
                        var services = this.GetServices(true);
                        var credentialService = services.FindService(OnvifService.CREDENTIAL_SERVICE);

                        Assert(null != credentialService,
                               "The DUT didn't return Credential service",
                               "Check Credential service is supported");

                        var capabilities = this.ExtractCredentialCapabilities(credentialService);

                        var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                        var logger = new StringBuilder();
                        Assert(equalCredentialCapabilities(serviceCapabilities, capabilities, logger),
                               logger.ToStringTrimNewLine(),
                               "Check CredentialServiceCapabilities consistency");
                    });
        }
        #endregion//1-1-*

        #region 2-1-*
        [Test(Name = "GET CREDENTIAL INFO",
            Id = "2-1-1",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialInfo })]
        public void GetCredentialInfoTest()
        {
            RunTest(() =>
                    {
                        var credentialInfoList = this.GetFullCredentialInfoListA1();

                        if (!credentialInfoList.Any())
                            return;

                        var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                        var tokenList = credentialInfoList.Take((int)serviceCapabilities.MaxLimit).Select(e => e.token);
                        var accessProfileInfoList1 = this.GetCredentialInfo(tokenList.ToArray());

                        var logger = new StringBuilder();
                        Assert(validateListFromGetCredentialInfo(accessProfileInfoList1, tokenList, logger),
                               logger.ToStringTrimNewLine(),
                               "Checking consistency of received CredentialInfo item's lists");

                        foreach (var credentialInfo in credentialInfoList)
                        {
                            var apil = this.GetCredentialInfo(new[] { credentialInfo.token });

                            var msg = string.Empty;
                            var n = apil.Count();
                            if (1 != n)
                            {
                                msg = string.Format("The DUT returned {0} CredentialInfo item{1} though the single item for token '{2}' is expected",
                                                    0 == n ? "no" : "several",
                                                    0 == n ? "" : "s",
                                                    credentialInfo.token);
                            }
                            else
                                msg = string.Format("The DUT returned no CredentialInfo item for token '{0}'", credentialInfo.token);

                            Assert(1 == apil.Count() && apil.First().token == credentialInfo.token,
                                   msg,
                                   "Checking that requested Credential item is received");

                            logger = new StringBuilder();
                            Assert(equalCredentialInfo(credentialInfo, apil.First(), logger),
                                   logger.ToStringTrimNewLine(),
                                   "Checking received CredentialInfo item");
                        }
                    });
        }

        [Test(Name = "GET CREDENTIAL INFO LIST - LIMIT",
            Id = "2-1-2",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialInfoList })]
        public void GetCredentialInfoLimitTest()
        {
            RunTest(() =>
                    {
                        var stepTitle = "Checking received list of CredentialInfo items";
                        var msgHeader = "Received list of CredentialInfo items contains {0} items but expected no more than {1}";

                        var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                        CredentialInfo[] credentialInfoList1;
                        this.GetCredentialInfoList(1, null, out credentialInfoList1);

                        Assert(credentialInfoList1.Count() <= 1,
                               string.Format(msgHeader, credentialInfoList1.Count(), 1),
                               stepTitle);

                        if (1 == serviceCapabilities.MaxLimit)
                            return;


                        CredentialInfo[] credentialInfoList2;
                        this.GetCredentialInfoList((int)serviceCapabilities.MaxLimit, null, out credentialInfoList2);

                        Assert(credentialInfoList2.Count() <= serviceCapabilities.MaxLimit,
                               string.Format(msgHeader, credentialInfoList2.Count(), serviceCapabilities.MaxLimit),
                               stepTitle);

                        if (2 == serviceCapabilities.MaxLimit)
                            return;


                        CredentialInfo[] credentialInfoList3;
                        this.GetCredentialInfoList((int)serviceCapabilities.MaxLimit - 1, null, out credentialInfoList3);

                        Assert(credentialInfoList3.Count() <= serviceCapabilities.MaxLimit - 1,
                               string.Format(msgHeader, credentialInfoList3.Count(), serviceCapabilities.MaxLimit - 1),
                               stepTitle);
                    });
        }

        [Test(Name = "GET CREDENTIAL INFO LIST - START REFERENCE AND LIMIT",
            Id = "2-1-3",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialInfoList })]
        public void GetCredentialInfoStartReferenceAndLimitTest()
        {
            RunTest(() =>
                    {
                        var stepTitle = "Checking two complete lists of CredentialInfo items";
                        var msgHeader = "Received complete list of CredentialInfo items with Limit = '{0}' is not the same as the received one with Limit = '{1}'";

                        var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                        var listFirst = receiveAndValidateCredentialInfoList((int)serviceCapabilities.MaxLimit);

                        if (1 == serviceCapabilities.MaxLimit)
                            return;


                        var listSecond = receiveAndValidateCredentialInfoList(1);

                        Assert(equalCredentialInfoLists(listFirst, listSecond),
                               string.Format(msgHeader, serviceCapabilities.MaxLimit, 1),
                               stepTitle);

                        if (2 == serviceCapabilities.MaxLimit)
                            return;


                        var listThird = receiveAndValidateCredentialInfoList((int)serviceCapabilities.MaxLimit - 1);

                        Assert(equalCredentialInfoLists(listFirst, listThird),
                               string.Format(msgHeader, serviceCapabilities.MaxLimit, serviceCapabilities.MaxLimit - 1),
                               stepTitle);
                    });
        }

        [Test(Name = "GET CREDENTIAL INFO LIST - NO LIMIT",
            Id = "2-1-4",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            RequiredFeatures = new Feature[] { Feature.Credential },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialInfoList })]
        public void GetCredentialInfoNoLimitTest()
        {
            RunTest(() =>
                    {
                        var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                        var list = receiveAndValidateCredentialInfoList((int)serviceCapabilities.MaxLimit, true);

                        Assert(list.Count() <= serviceCapabilities.MaxCredentials,
                               string.Format("The received full list of CredentialInfo items contains {0} items though the expected number is not more than {1}", list.Count(), serviceCapabilities.MaxCredentials),
                               "Checking complete list of CredentialInfo items");
                    });
        }

        [Test(Name = "GET CREDENTIAL INFO WITH INVALID TOKEN",
            Id = "2-1-5",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialInfo })]
        public void GetCredentialInfoWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullCredentialInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                CredentialInfo[] infos = null;
                infos = this.GetCredentialInfo(new string[] { invalidToken });

                Assert(infos == null || infos.Length == 0,
                    "List of CredentialInfo is not empty",
                    "Check that the DUT returned no CredentialInfo");

                if (fullList == null || fullList.Count == 0)
                {
                    return;
                }
                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;
                if (maxLimit >= 2)
                {
                    infos = this.GetCredentialInfo(new string[] { fullList[0].token, invalidToken });

                    var expected = fullList[0];

                    this.CheckRequestedInfo(infos, expected.token, "Credential Info", D => D.token);
                }
                else
                {
                    LogTestEvent(string.Format("MaxLimit={0}, skip part with sending request with one correct and one incorrect tokens.", maxLimit) + Environment.NewLine);
                }
            });
        }
        [Test(Name = "GET CREDENTIAL INFO - TOO MANY ITEMS",
            Id = "2-1-6",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialInfo })]
        public void GetCredentialInfoTooManyItemsTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullCredentialInfoListA1();

                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;

                if (fullList == null || fullList.Count <= maxLimit)
                    return;

                string faultCode = "Sender/InvalidArgs/TooManyItems";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    fullList.RemoveRange((int)maxLimit, fullList.Count - (int)maxLimit - 1);
                    this.GetCredentialInfo(fullList.Select(e => e.token).ToArray());
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        throw;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
            });
        }
        #endregion//2-1-*

        #region 3-1-*

        #region 3-1-1 GET CREDENTIALS
        [Test(Name = "GET CREDENTIALS",
            Id = "3-1-1",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentials })]
        public void GetAccessProfilesTest()
        {
            RunTest(() =>
                    {
                        var credentialList = this.GetFullCredentialListA3();

                        if (!credentialList.Any())
                            return;

                        var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                        var tokenList = credentialList.Take((int)serviceCapabilities.MaxLimit).Select(e => e.token);
                        var credentialsList1 = this.GetCredentials(tokenList.ToArray());

                        var logger = new StringBuilder();
                        Assert(validateListFromGetCredentials(credentialsList1, tokenList, logger),
                               logger.ToStringTrimNewLine(),
                               "Checking consistency of received Credential item's lists");

                        foreach (var credential in credentialList)
                        {
                            var apl = this.GetCredentials(new[] { credential.token });

                            var msg = string.Empty;
                            var n = apl.Count();
                            if (1 != n)
                            {
                                msg = string.Format("The DUT returned {0} Credential item{1} though the single item for token '{2}' is expected",
                                                    0 == n ? "no" : "several",
                                                    0 == n ? "" : "s",
                                                    credential.token);
                            }
                            else
                                msg = string.Format("The DUT returned no CredentialInfo item for token '{0}'", credential.token);

                            Assert(1 == apl.Count() && apl.First().token == credential.token,
                                   msg,
                                   "Checking that requested Credential item is received");


                            logger = new StringBuilder();
                            Assert(equalCredential(credential, apl.First(), logger, "GetCredentialList", "GetCredentials"),
                                   logger.ToStringTrimNewLine(),
                                   "Checking received Credential item");
                        }
                    });
        }
        #endregion

        #region 3-1-2 GET CREDENTIAL LIST - LIMIT
        [Test(Name = "GET CREDENTIAL LIST - LIMIT",
            Id = "3-1-2",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialList })]
        public void GetAccessProfileListLimitTest()
        {
            RunTest(() =>
                    {
                        var stepTitle = "Checking received list of Credential items";
                        var msgHeader = "Received list of Credential items contains {0} items but expected no more than {1}";

                        var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                        Credential[] credentialList1;
                        this.GetCredentialList(1, null, out credentialList1);

                        Assert(credentialList1.Count() <= 1,
                               string.Format(msgHeader, credentialList1.Count(), 1),
                               stepTitle);

                        if (1 == serviceCapabilities.MaxLimit)
                            return;


                        Credential[] credentialList2;
                        this.GetCredentialList((int)serviceCapabilities.MaxLimit, null, out credentialList2);

                        Assert(credentialList2.Count() <= serviceCapabilities.MaxLimit,
                               string.Format(msgHeader, credentialList2.Count(), serviceCapabilities.MaxLimit),
                               stepTitle);

                        if (2 == serviceCapabilities.MaxLimit)
                            return;


                        Credential[] credentialList3;
                        this.GetCredentialList((int)serviceCapabilities.MaxLimit - 1, null, out credentialList3);

                        Assert(credentialList3.Count() <= serviceCapabilities.MaxLimit - 1,
                               string.Format(msgHeader, credentialList3.Count(), serviceCapabilities.MaxLimit - 1),
                               stepTitle);
                    });
        }
        #endregion

        #region 3-1-3 GET CREDENTIAL LIST - START REFERENCE AND LIMIT
        [Test(Name = "GET CREDENTIAL LIST - START REFERENCE AND LIMIT",
            Id = "3-1-3",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialList })]
        public void GetAccessProfileStartReferenceAndLimitTest()
        {
            RunTest(() =>
                    {
                        var stepTitle = "Checking two complete lists of Credential items";
                        var msgHeader = "Received complete list of Credential items with Limit = '{0}' is not the same as the received one with Limit = '{1}'";
                        var logger = new StringBuilder();

                        var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                        var listFirst = receiveAndValidateCredentialList((int)serviceCapabilities.MaxLimit);

                        if (1 == serviceCapabilities.MaxLimit)
                        {
                            logger = new StringBuilder();
                            Assert(validateListFromGetCredentialAndGetCredentialInfoConsistency(this.GetFullCredentialInfoListA1(), listFirst, logger),
                                   logger.ToStringTrimNewLine(),
                                   "Checking consistency of received CredentialInfo and Credential lists");
                            return;
                        }

                        var listSecond = receiveAndValidateCredentialList(1);

                        Assert(equalCredentialInfoLists(listFirst, listSecond),
                               string.Format(msgHeader, serviceCapabilities.MaxLimit, 1),
                               stepTitle);

                        if (2 == serviceCapabilities.MaxLimit)
                        {
                            logger = new StringBuilder();
                            Assert(validateListFromGetCredentialAndGetCredentialInfoConsistency(this.GetFullCredentialInfoListA1(), listSecond, logger),
                                   logger.ToStringTrimNewLine(),
                                   "Checking consistency of received CredentialInfo and Credential lists");
                            return;
                        }

                        var listThird = receiveAndValidateCredentialList((int)serviceCapabilities.MaxLimit - 1);

                        Assert(equalCredentialInfoLists(listFirst, listThird),
                               string.Format(msgHeader, serviceCapabilities.MaxLimit, serviceCapabilities.MaxLimit - 1),
                               stepTitle);

                        logger = new StringBuilder();
                        Assert(validateListFromGetCredentialAndGetCredentialInfoConsistency(this.GetFullCredentialInfoListA1(), listThird, logger),
                               logger.ToStringTrimNewLine(),
                               "Checking consistency of received CredentialInfo and Credential lists");
                    });
        }
        #endregion

        #region 3-1-4 GET CREDENTIAL LIST - NO LIMIT
        [Test(Name = "GET CREDENTIAL LIST - NO LIMIT",
            Id = "3-1-4",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialList })]
        public void GetAccessProfileNoLimitTest()
        {
            RunTest(() =>
                    {
                        var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                        var list = receiveAndValidateCredentialList((int)serviceCapabilities.MaxLimit, true);

                        var fullAccessProfileInfoList = this.GetFullCredentialInfoListA1();

                        var logger = new StringBuilder();
                        Assert(validateListFromGetCredentialAndGetCredentialInfoConsistency(fullAccessProfileInfoList, list, logger),
                               logger.ToStringTrimNewLine(),
                               "Checking consistency of received CredentialInfo and Credential lists");
                    });
        }
        #endregion

        #region internal 3-1-5 CreateCredentialUniTest() test function
        internal void CreateCredentialUniTest(bool bEnabled)
        {
            string credentialToken = null;
            SubscriptionHandler subscription = null;
            CredentialServiceExtensions.CredentialAndState[] arrCredToRestore = new CredentialServiceExtensions.CredentialAndState[0];
            var logger = new StringBuilder();

            RunTest(() =>
            {
                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                //3 retrieves a complete list of credentials (out credentialCompleteList1) by following the procedure mentioned in Annex A.3
                var credentialCompleteList = this.GetFullCredentialListA3();

                //4. ONVIF Client checks free storage for additional Credential (in credentialCompleteList1, out credentialToRestore) by following the procedure mentioned in Annex A.7.
                arrCredToRestore = this.CheckFreeStorageForCredentialA7(credentialCompleteList);

                //5. ONVIF Client retrieves a complete list of access profile (out accessProfileCompleteList) by following the procedure mentioned in Annex A.5.
                List<AccessProfile> arrAccessProfile = this.GetFullAccessProfilesListA3();

                var value = this.GetCredentialIdentifierTypeAndValueA15(serviceCapabilities.SupportedIdentifierType);

                //6. ONVIF Client invokes CreatePullPointSubscription with parameters Filter.TopicExpression := "tns1:Configuration/Credential/Changed"
                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/Credential/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                //7. The DUT responds with a CreatePullPointSubscriptionResponse 
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                //8. ONVIF client invokes CreateCredential with parameters
                var cred = new Credential
                {
                    token = "",
                    Description = "Test Description",
                    CredentialHolderReference = "TestUser",
                    CredentialIdentifier = new[] 
                                           { 
                                               new CredentialIdentifier 
                                               { 
                                                   Type = new CredentialIdentifierType 
                                                          { Name = value.TypeName, FormatType = value.FormatType }, 
                                                   ExemptedFromAuthentication = false,
                                                   Value = value.Value
                                               } 
                                           },
                    CredentialAccessProfile = arrAccessProfile.Any() ? new[] { new CredentialAccessProfile { AccessProfileToken = arrAccessProfile.First().token, ValidFromSpecified = false, ValidToSpecified = false } } : null,
                    Extension = null,
                    ValidFromSpecified = false,
                    ValidToSpecified = false
                };

                AntipassbackState credapbst = null;

                if (serviceCapabilities.ResetAntipassbackSupported)
                    credapbst = new AntipassbackState { AntipassbackViolated = false };

                var credst = new CredentialState
                {
                    Enabled = bEnabled,
                    Reason = "Test Reason",
                    AntipassbackState = credapbst
                };


                credentialToken = this.CreateCredential(cred, credst);
                Assert(!string.IsNullOrEmpty(credentialToken),
                       string.Format("CreateCredential returned {0}", null == credentialToken ? "no token" : "empty token"),
                       "Check token returned by CreateCredential");
                cred.token = credentialToken;

                // 11. Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Credential.CredentialTokenValidation.xq", new List<Credential>() { cred });

                // 12. If timeout1 timeout expires for step 11 without Notification with CredentialToken source simple item equal to credentialToken, FAIL the test and go to the step 21.
                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that CreateCredential notification is received");

                logger.Clear();
                Assert(ValidateNotifications(notifications, topicInfo, credentialToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");

                // 13. ONVIF Client retrieves a credential (in credentialToken, out credentialList) by following the procedure mentioned in Annex A.8.
                var arrCredRetr = this.GetCredentials(new[] { credentialToken });
                ValidateConsistency(cred, arrCredRetr.FirstOrDefault(), "CreateCredential", "GetCredentials");

                var arrCredInfoRetr = this.GetCredentialInfo(new[] { credentialToken });
                ValidateConsistency(cred, arrCredInfoRetr.FirstOrDefault(), "CreateCredential", "GetCredentialInfo");

                var fullCredentialInfoList = this.GetFullCredentialInfoListA1();
                ValidateConsistency(cred, fullCredentialInfoList, "CreateCredential", "GetCredentialInfoList");

                var fullCredentialList = this.GetFullCredentialListA3();
                ValidateConsistency(cred, fullCredentialList, "CreateCredential", "GetCredentialList");

                // 15. ONVIF Client retrieves a credential info (in credentialToken, out credentialInfoList) by following the procedure mentioned in Annex A.9.
                var crs = this.GetCredentialState(credentialToken);
                ValidateConsistency(serviceCapabilities, credst, crs, "CreateCredential", "GetCredentialState");
            },
            () =>
            {
                // 21. ONVIF Client deletes the Credential (in credentialToken) by following the procedure mentioned in Annex A.6 to restore DUT configuration.
                if (!string.IsNullOrEmpty(credentialToken))
                    this.DeleteCredential(credentialToken);

                this.RestoreCredentialsA10(arrCredToRestore);

                // 23. ONVIF Client sends an Unsubscribe to the subscription endpoint s.
                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);
            });
        }

        #endregion

        #region 3-1-5 CREATE CREDENTIAL (ENABLED)
        [Test(Name = "CREATE CREDENTIAL (ENABLED)",
            Id = "3-1-5",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            RequiredFeatures = new Feature[] { Feature.Credential, Feature.AccessRulesService },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateCredential, Functionality.ConfigurationCredentialChangedEvent })]
        public void CreateCredentialEnabledTest()
        {
            CreateCredentialUniTest(true);
        }
        #endregion

        #region 3-1-6 CREATE CREDENTIAL (DISABLED)
        [Test(Name = "CREATE CREDENTIAL (DISABLED)",
            Id = "3-1-6",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential, Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateCredential, Functionality.ConfigurationCredentialChangedEvent })]
        public void CreateCredentialDisabledTest()
        {
            CreateCredentialUniTest(false);
        }
        #endregion

        #region 3-1-7 MODIFY CREDENTIAL
        [Test(Name = "MODIFY CREDENTIAL",
            Id = "3-1-7",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential, Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.ModifyCredential, Functionality.ConfigurationCredentialChangedEvent })]
        public void ModifyCredentialTest()
        {
            string credentialToken = null;
            SubscriptionHandler subscription = null;
            var arrCredToRestore = new CredentialServiceExtensions.CredentialAndState[0];
            var logger = new StringBuilder();

            RunTest(() =>
            {
                //3. retrieves a complete list of credentials (out credentialCompleteList1) by following the procedure mentioned in Annex A.3
                List<Credential> credentialCompleteList = this.GetFullCredentialListA3();

                //4. ONVIF Client checks free storage for additional Credential (in credentialCompleteList1, out credentialToRestore) by following the procedure mentioned in Annex A.7.
                arrCredToRestore = this.CheckFreeStorageForCredentialA7(credentialCompleteList);

                //5. ONVIF Client retrieves a complete list of access profile (out accessProfileCompleteList) by following the procedure mentioned in Annex A.5.
                List<AccessProfile> arrAccessProfile = this.GetFullAccessProfilesListA3();

                //6. ONVIF client invokes CreateCredential with parameters
                Credential cred;
                CredentialState credst;
                credentialToken = this.CreateCredentialA11(out cred, out credst);
                cred.token = credentialToken;

                //7. Client invokes CreatePullPointSubscription with parameters: Filter.TopicExpression := "tns1:Configuration/Credential/Changed"
                subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/Credential/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                //8. The DUT responds with a CreatePullPointSubscriptionResponse 
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                //9. ONVIF client invokes ModifyCredential with parameters
                cred.token = credentialToken;
                cred.Description = "Test Description 2";
                cred.CredentialHolderReference = "TestUser 2";
                cred.CredentialAccessProfile = arrAccessProfile.Any() ? new[] { new CredentialAccessProfile { AccessProfileToken = arrAccessProfile.First().token } } : null;

                //(this as ICredentialService).ModifyCredential(strCreateCredentialResponseToken, cred);
                this.ModifyCredential(cred);

                // 11. Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Credential.CredentialTokenValidation.xq", new List<Credential>() { cred });

                // 12. If timeout1 timeout expires for step 11 without Notification with CredentialToken source simple item equal to credentialToken, FAIL the test and go to the step 21.
                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();

                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that ModifyCredential notification is received");

                logger.Clear();
                Assert(ValidateNotifications(notifications, topicInfo, credentialToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");

                // 13. ONVIF Client retrieves a credential (in credentialToken, out credentialList) by following the procedure mentioned in Annex A.8.
                var arrCredRetr = this.GetCredentials(new[] { credentialToken });
                ValidateConsistency(cred, arrCredRetr.FirstOrDefault(), "ModifyCredential", "GetCredentials");

                var arrCredInfoRetr = this.GetCredentialInfo(new[] { credentialToken });
                // 14. If credentialList[0] item does not have equal field values to values from step 8, FAIL the test and go Step 21.
                ValidateConsistency(cred, arrCredInfoRetr.FirstOrDefault(), "ModifyCredential", "GetCredentialInfo");

                var fullCredentialInfoList = this.GetFullCredentialInfoListA1();
                ValidateConsistency(cred, fullCredentialInfoList, "ModifyCredential", "GetCredentialInfoList");

                var fullCredentialList = this.GetFullCredentialListA3();
                ValidateConsistency(cred, fullCredentialList, "ModifyCredential", "GetCredentialList");
            },
            () =>
            {
                // 21. ONVIF Client deletes the Credential (in credentialToken) by following the procedure mentioned in Annex A.6 to restore DUT configuration.
                if (!string.IsNullOrEmpty(credentialToken))
                    this.DeleteCredential(credentialToken);

                this.RestoreCredentialsA10(arrCredToRestore);

                // 23. ONVIF Client sends an Unsubscribe to the subscription endpoint s.
                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);
            });
        }
        #endregion

        #region 3-1-8 DELETE CREDENTIAL
        [Test(Name = "DELETE CREDENTIAL",
            Id = "3-1-8",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCredential, Functionality.ConfigurationCredentialRemovedEvent })]
        public void DeleteCredentialTest()
        {
            bool bDeletedExists = false;
            string credentialToken = null;
            Events.SubscriptionHandler subscription = null;
            CredentialServiceExtensions.CredentialAndState[] arrCredToRestore = new CredentialServiceExtensions.CredentialAndState[0];
            StringBuilder logger = new StringBuilder();

            RunTest(() =>
            {
                //3. retrieves a complete list of credentials (out credentialCompleteList1) by following the procedure mentioned in Annex A.3
                List<Credential> credentialCompleteList = this.GetFullCredentialListA3();

                //4. ONVIF Client checks free storage for additional Credential (in credentialCompleteList1, out credentialToRestore) by following the procedure mentioned in Annex A.7.
                arrCredToRestore = this.CheckFreeStorageForCredentialA7(credentialCompleteList);

                //6. ONVIF Client creates credential (in false, out credentialToken) by following the procedure mentioned in Annex A.11.
                Credential cred;
                CredentialState credst;
                credentialToken = this.CreateCredentialA11(out cred, out credst, false);
                cred.token = credentialToken;

                //7. Client invokes CreatePullPointSubscription with parameters: Filter.TopicExpression := "tns1:Configuration/Credential/Removed"
                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/Credential/Removed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                //8. The DUT responds with a CreatePullPointSubscriptionResponse 
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                //10.	ONVIF Client invokes DeleteCredential with parameters 	Token := credentialToken
                this.DeleteCredential(credentialToken);

                //11. Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new Events.SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Credential.CredentialTokenValidation.xq", new List<Credential>() { cred });

                var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Waiting for notification");

                // 12.	Until timeout1 timeout expires repeat the following steps
                Assert(ValidateNotifications(notifications, topicInfo, credentialToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");


                // 14. ONVIF Client retrieves a credential (in credentialToken, out credentialList) by following the procedure mentioned in Annex A.8.
                var credentials = this.GetCredentials(new string[] { credentialToken });
                // 15.	If credentialList is not empty, FAIL the test and go Step 22.
                Assert(!credentials.Any(),
                       "Previosly removed creditial exists and didn't removed!",
                       "Verifying removed creditial can be retrieved");

                // 16. ONVIF Client retrieves a credential info (in credentialToken, out credentialInfoList) by following the procedure mentioned in Annex A.9.
                var credentialInfos = this.GetCredentialInfo(new string[] { credentialToken });
                // 17. If credentialInfoList is not empty, FAIL the test and go Step 22.
                Assert(!credentialInfos.Any(),
                       "Previosly removed creditial info exists and didn't removed!",
                       "Verifying removed creditial info can be retrieved");

                //18. ONVIF Client retrieves a complete list of credentials (out credentialInfoCompleteList) by following the procedure mentioned in Annex A.1.
                var credentialInfoList = this.GetFullCredentialInfoListA1();
                Assert(!credentialInfoList.Any(e => e.token == credentialToken),
                       "GetCredentialInfoList retrives deleted credential info for token = \"" + credentialToken + "\"",
                       "Verifying list of credential info");

                // 20. ONVIF Client retrieves a complete list of credentials (out credentialCompleteList2) by following the procedure mentioned in Annex A.3.
                var credentialList = this.GetFullCredentialListA3(); // credentialCompleteList
                Assert(!credentialList.Any(e => e.token == credentialToken),
                       "GetCredentialList retrives deleted credential for token = \"" + credentialToken + "\"",
                       "Verifying list of credentials");

            },
            () =>
            {
                // 22. If there was credential deleted at Step 4, restore it (in credentialToRestore, stateToReastore) by following the procedure mentioned in Annex A.10 to restore DUT configuration.
                this.RestoreCredentialsA10(arrCredToRestore);

                // 23. ONVIF Client sends an Unsubscribe to the subscription endpoint s.
                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);
            });
        }
        #endregion

        [Test(Name = "GET CREDENTIALS WITH INVALID TOKEN",
            Id = "3-1-9",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentials })]
        public void GetCredentialsWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullCredentialInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                Credential[] credentials = null;
                credentials = this.GetCredentials(new string[] { invalidToken });

                Assert(credentials == null || credentials.Length == 0,
                    "List of Credentials is not empty",
                    "Check that the DUT returned no Credentials");

                if (fullList == null || fullList.Count == 0)
                {
                    return;
                }
                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;
                if (maxLimit >= 2)
                {
                    credentials = this.GetCredentials(new string[] { fullList[0].token, invalidToken });

                    var expected = fullList[0];

                    this.CheckRequestedInfo(credentials, expected.token, "Credentials", D => D.token);
                }
                else
                {
                    LogTestEvent(string.Format("MaxLimit={0}, skip part with sending request with one correct and one incorrect tokens.", maxLimit) + Environment.NewLine);
                }
            });
        }

        [Test(Name = "GET CREDENTIALS - TOO MANY ITEMS",
            Id = "3-1-10",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentials })]
        public void GetCredentialsTooManyItemsTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullCredentialListA3();

                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;

                if (fullList == null || fullList.Count <= maxLimit)
                    return;

                string faultCode = "Sender/InvalidArgs/TooManyItems";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    fullList.RemoveRange((int)maxLimit, fullList.Count - (int)maxLimit - 1);
                    this.GetCredentials(fullList.Select(e => e.token).ToArray());
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        throw;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
            });
        }

        [Test(Name = "CREATE CREDENTIAL - NOT EMPTY CREDENTIAL TOKEN",
            Id = "3-1-11",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateCredential })]
        public void CreateCredentialNotEmptyCredentialTokenTest()
        {
            RunTest(() =>
            {

                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();
                var value = this.GetCredentialIdentifierTypeAndValueA15(serviceCapabilities.SupportedIdentifierType);
                var newCredential = new Credential()
                {
                    token = "CredentialToken",
                    Description = "Test Description",
                    CredentialHolderReference = "TestUser",
                    CredentialIdentifier = new[] {
                        new CredentialIdentifier {
                            Type = new CredentialIdentifierType { Name = value.TypeName, FormatType = value.FormatType },
                            ExemptedFromAuthentication = false,
                            Value = value.Value
                        }
                    },
                };

                AntipassbackState antipassbackState = null;

                if (serviceCapabilities.ResetAntipassbackSupported)
                    antipassbackState = new AntipassbackState() { AntipassbackViolated = false };

                var newCredentialState = new CredentialState()
                {
                    Enabled = true,
                    Reason = "Test Reason",
                    AntipassbackState = antipassbackState
                };

                string faultCode = "Sender/InvalidArgs";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    var credentialToken = this.CreateCredential(newCredential, newCredentialState);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        throw;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            });
        }

        [Test(Name = "MODIFY CREDENTIAL WITH INVALID TOKEN",
            Id = "3-1-12",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.ModifyCredential })]
        public void ModifyCredentialWithInvalidTokenTest()
        {
            RunTest(() =>
            {

                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();
                var fullList = this.GetFullCredentialInfoListA1();
                var value = this.GetCredentialIdentifierTypeAndValueA15(serviceCapabilities.SupportedIdentifierType);

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                var invalidCredential = new Credential()
                {
                    token = invalidToken,
                    Description = "Test Description",
                    CredentialHolderReference = "TestUser",
                    CredentialIdentifier = new[] {
                        new CredentialIdentifier {
                            Type = new CredentialIdentifierType { Name = value.TypeName, FormatType = value.FormatType },
                            ExemptedFromAuthentication = false,
                            Value = value.Value
                        }
                    },
                };

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.ModifyCredential(invalidCredential);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            });
        }

        [Test(Name = "DELETE CREDENTIAL WITH INVALID TOKEN",
            Id = "3-1-13",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCredential })]
        public void DeleterCredentialWithInvalidTokenTest()
        {
            RunTest(() =>
            {

                var fullList = this.GetFullCredentialInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.DeleteCredential(invalidToken);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            });
        }

        [Test(Name = "CREATE CREDENTIAL - VALIDITY VALUES",
            Id = "3-1-14",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential, Feature.CredentialValidityOrCredentialAccessProfileValidity, Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateCredential })]
        public void CreateCredentialValidityValuesTest()
        {
            string credentialToken = "";
            RunTest(() =>
            {

                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();
                var fullAccessProfileList = this.GetFullAccessProfilesListA3();

                var value = this.GetCredentialIdentifierTypeAndValueA15(serviceCapabilities.SupportedIdentifierType);

                System.DateTime credentialValidFrom = System.DateTime.Now;
                System.DateTime credentialValidTo = credentialValidFrom.AddYears(1);

                System.DateTime accessProfileValidFrom = System.DateTime.Now.AddHours(24);
                System.DateTime accessProfileValidTo = accessProfileValidFrom.AddYears(1);

                var newCredential = new Credential()
                {
                    token = "",
                    Description = "Test Description",
                    CredentialHolderReference = "TestUser",
                    CredentialIdentifier = new[] {
                        new CredentialIdentifier {
                            Type = new CredentialIdentifierType { Name = value.TypeName, FormatType = value.FormatType },
                            ExemptedFromAuthentication = false,
                            Value = value.Value,
                        }
                    },
                };

                if (serviceCapabilities.CredentialValiditySupported)
                {
                    newCredential.ValidFrom = credentialValidFrom;
                    newCredential.ValidTo = credentialValidTo;
                    newCredential.ValidFromSpecified = true;
                    newCredential.ValidToSpecified = true;
                }
                if (fullAccessProfileList.Count > 0 && serviceCapabilities.CredentialAccessProfileValiditySupported)
                {
                    newCredential.CredentialAccessProfile = new CredentialAccessProfile[] {
                            new CredentialAccessProfile() {
                                AccessProfileToken = fullAccessProfileList[0].token,
                                ValidFrom = accessProfileValidFrom,
                                ValidTo = accessProfileValidTo,
                                ValidFromSpecified = true,
                                ValidToSpecified = true
                            }
                        };
                }

                AntipassbackState antipassbackState = null;

                if (serviceCapabilities.ResetAntipassbackSupported)
                    antipassbackState = new AntipassbackState() { AntipassbackViolated = false };

                var newCredentialState = new CredentialState()
                {
                    Enabled = false,
                    Reason = "Test Reason",
                    AntipassbackState = antipassbackState
                };

                credentialToken = this.CreateCredential(newCredential, newCredentialState);

                // 9. ONVIF Client retrieves a credential (in credentialToken, out credentialList) by following the procedure mentioned in Annex A.8.
                var credentials = this.GetCredentials(new string[] { credentialToken });
                // 10. ONVIF Client retrieves a credential info (in credentialToken, out credentialInfoList) by following the procedure mentioned in Annex A.9.
                var credentialInfos = this.GetCredentialInfo(new string[] { credentialToken });
                if (serviceCapabilities.CredentialValiditySupported)
                {
                    ValidateTimeValueConsistency(credentials[0].ValidFrom, credentialValidFrom, "GetCredentials", "Credential", "ValidFrom", serviceCapabilities.ValiditySupportsTimeValue);
                    ValidateTimeValueConsistency(credentials[0].ValidTo, credentialValidTo, "GetCredentials", "Credential", "ValidTo", serviceCapabilities.ValiditySupportsTimeValue);

                    if (credentials[0].CredentialAccessProfile != null)
                    {
                        ValidateTimeValueConsistency(credentials[0].CredentialAccessProfile[0].ValidFrom, accessProfileValidFrom, "GetCredentials", "Credential.CredentialAccessProfile", "ValidFrom", serviceCapabilities.ValiditySupportsTimeValue);
                        ValidateTimeValueConsistency(credentials[0].CredentialAccessProfile[0].ValidTo, accessProfileValidTo, "GetCredentials", "Credential.CredentialAccessProfile", "ValidTo", serviceCapabilities.ValiditySupportsTimeValue);
                    }

                    ValidateTimeValueConsistency(credentialInfos[0].ValidFrom, credentialValidFrom, "GetCredentialInfo", "CredentialInfo", "ValidFrom", serviceCapabilities.ValiditySupportsTimeValue);
                    ValidateTimeValueConsistency(credentialInfos[0].ValidTo, credentialValidTo, "GetCredentialInfo", "CredentialInfo", "ValidTo", serviceCapabilities.ValiditySupportsTimeValue);
                }
            },
            () =>
            {
                if (!string.IsNullOrEmpty(credentialToken))
                    this.DeleteCredential(credentialToken);
            });

        }

        [Test(Name = "MODIFY CREDENTIAL - VALIDITY VALUES",
            Id = "3-1-15",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential, Feature.CredentialValidityOrCredentialAccessProfileValidity, Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.ModifyCredential })]
        public void ModifyCredentialValidityValuesTest()
        {
            string credentialToken = "";
            RunTest(() =>
            {

                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();
                var fullAccessProfileList = this.GetFullAccessProfilesListA3();

                var value = this.GetCredentialIdentifierTypeAndValueA15(serviceCapabilities.SupportedIdentifierType);

                System.DateTime credentialValidFrom1 = System.DateTime.Now;
                System.DateTime credentialValidTo1 = credentialValidFrom1.AddYears(1);

                System.DateTime accessProfileValidFrom1 = System.DateTime.Now.AddHours(24);
                System.DateTime accessProfileValidTo1 = accessProfileValidFrom1.AddYears(1);

                var newCredential = new Credential()
                {
                    token = "",
                    Description = "Test Description",
                    CredentialHolderReference = "TestUser",
                    CredentialIdentifier = new[] {
                        new CredentialIdentifier {
                            Type = new CredentialIdentifierType { Name = value.TypeName, FormatType = value.FormatType },
                            ExemptedFromAuthentication = false,
                            Value = value.Value,
                        }
                    },
                };

                if (serviceCapabilities.CredentialValiditySupported)
                {
                    newCredential.ValidFrom = credentialValidFrom1;
                    newCredential.ValidTo = credentialValidTo1;
                    newCredential.ValidFromSpecified = true;
                    newCredential.ValidToSpecified = true;
                }
                if (fullAccessProfileList.Count > 0 && serviceCapabilities.CredentialAccessProfileValiditySupported)
                {
                    newCredential.CredentialAccessProfile = new CredentialAccessProfile[] {
                            new CredentialAccessProfile() {
                                AccessProfileToken = fullAccessProfileList[0].token,
                                ValidFrom = accessProfileValidFrom1,
                                ValidTo = accessProfileValidTo1,
                                ValidFromSpecified = true,
                                ValidToSpecified = true
                            }
                        };
                }

                AntipassbackState antipassbackState = null;

                if (serviceCapabilities.ResetAntipassbackSupported)
                    antipassbackState = new AntipassbackState() { AntipassbackViolated = false };

                var newCredentialState = new CredentialState()
                {
                    Enabled = false,
                    Reason = "Test Reason",
                    AntipassbackState = antipassbackState
                };

                credentialToken = this.CreateCredential(newCredential, newCredentialState);
                newCredential.token = credentialToken;

                System.DateTime credentialValidFrom2 = credentialValidFrom1.AddDays(1).AddHours(1);
                System.DateTime credentialValidTo2 = credentialValidTo1.AddDays(1).AddHours(1);

                System.DateTime accessProfileValidFrom2 = accessProfileValidFrom1.AddDays(1).AddHours(1);
                System.DateTime accessProfileValidTo2 = accessProfileValidTo1.AddDays(1).AddHours(1);

                if (serviceCapabilities.CredentialValiditySupported)
                {
                    newCredential.ValidFrom = credentialValidFrom2;
                    newCredential.ValidTo = credentialValidTo2;
                    newCredential.ValidFromSpecified = true;
                    newCredential.ValidToSpecified = true;
                }
                if (fullAccessProfileList.Count > 0 && serviceCapabilities.CredentialAccessProfileValiditySupported)
                {
                    newCredential.CredentialAccessProfile = new CredentialAccessProfile[] {
                            new CredentialAccessProfile() {
                                AccessProfileToken = fullAccessProfileList[0].token,
                                ValidFrom = accessProfileValidFrom2,
                                ValidTo = accessProfileValidTo2,
                                ValidFromSpecified = true,
                                ValidToSpecified = true
                            }
                        };
                }

                this.ModifyCredential(newCredential);

                // 12. ONVIF Client retrieves a credential (in credentialToken, out credentialList) by following the procedure mentioned in Annex A.8.
                var credentials = this.GetCredentials(new string[] { credentialToken });
                // 13. ONVIF Client retrieves a credential info (in credentialToken, out credentialInfoList) by following the procedure mentioned in Annex A.9.
                var credentialInfos = this.GetCredentialInfo(new string[] { credentialToken });
                if (serviceCapabilities.CredentialValiditySupported)
                {
                    ValidateTimeValueConsistency(credentials[0].ValidFrom, credentialValidFrom2, "GetCredentials", "Credential", "ValidFrom", serviceCapabilities.ValiditySupportsTimeValue);
                    ValidateTimeValueConsistency(credentials[0].ValidTo, credentialValidTo2, "GetCredentials", "Credential", "ValidTo", serviceCapabilities.ValiditySupportsTimeValue);

                    if (credentials[0].CredentialAccessProfile != null)
                    {
                        ValidateTimeValueConsistency(credentials[0].CredentialAccessProfile[0].ValidFrom, accessProfileValidFrom2, "GetCredentials", "Credential.CredentialAccessProfile", "ValidFrom", serviceCapabilities.ValiditySupportsTimeValue);
                        ValidateTimeValueConsistency(credentials[0].CredentialAccessProfile[0].ValidTo, accessProfileValidTo2, "GetCredentials", "Credential.CredentialAccessProfile", "ValidTo", serviceCapabilities.ValiditySupportsTimeValue);
                    }

                    ValidateTimeValueConsistency(credentialInfos[0].ValidFrom, credentialValidFrom2, "GetCredentialInfo", "CredentialInfo", "ValidFrom", serviceCapabilities.ValiditySupportsTimeValue);
                    ValidateTimeValueConsistency(credentialInfos[0].ValidTo, credentialValidTo2, "GetCredentialInfo", "CredentialInfo", "ValidTo", serviceCapabilities.ValiditySupportsTimeValue);
                }

            },
            () =>
            {
                if (!string.IsNullOrEmpty(credentialToken))
                    this.DeleteCredential(credentialToken);
            });

        }

        #endregion

        #region 4-1-*

        #region 4-1-1 GET CREDENTIAL STATE
        [Test(Name = "GET CREDENTIAL STATE",
            Id = "4-1-1",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_STATE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialState })]
        public void ChangeCredentialStateTest()
        {
            RunTest(() =>
            {
                // 3. ONVIF Client gets the service capabilities (out cap) by following the procedure mentioned in Annex A.2.
                CredentialServiceCapabilities serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                // 4. ONVIF Client retrieves a complete list of credentials (out credentialInfoCompleteList) by following the procedure mentioned in Annex A.1.
                List<CredentialInfo> credentialInfoCompleteList = this.GetFullCredentialInfoListA1();

                // 5. If credentialInfoCompleteList is empty, skip other steps.
                if (!credentialInfoCompleteList.Any())
                {
                    LogTestEvent("The DUT returned no Credential items. Test passed.");
                    LogTestEvent("");

                    return;
                }

                // 6. For each CredentialInfo.token token from credentialInfoCompleteList repeat the following steps:
                foreach (CredentialInfo cri in credentialInfoCompleteList)
                {
                    CredentialState srs = (this as ICredentialService).GetCredentialState(cri.token);
                    
                    // 6.3. If cap.ResetAntipassbackSupported is equal to true and credentialState does not contain AntipassbackState element, FAIL the test.
                    if (serviceCapabilities.ResetAntipassbackSupported)
                        Assert(srs.AntipassbackState != null, "The DUT supports Reset Antipassback (capability ResetAntipassbackSupported = true), but CredentialState doesn't contain AntipassbackState.", string.Format("Checking that CredentialState contains AntipassbackState (CredentialID = {0})", cri.token));
                }
            },
            () =>
            {
            });
        }
        #endregion

        #region 4-1-2 CHANGE CREDENTIAL STATE
        [Test(Name = "CHANGE CREDENTIAL STATE",
            Id = "4-1-2",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_STATE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialState, Functionality.CredentialStateEnabledEvent, Functionality.EnableCredential, Functionality.DisableCredential })]
        public void GetCredentialStateTest()
        {
            string credentialToken = null;
            Events.SubscriptionHandler subscription = null;
            CredentialServiceExtensions.CredentialAndState[] arrCredToRestore = new CredentialServiceExtensions.CredentialAndState[0];
            StringBuilder logger = new StringBuilder();

            RunTest(() =>
            {
                //3. ONVIF Client retrieves a complete list of credentials (out credentialCompleteList1) by following the procedure mentioned in Annex A.3.
                List<Credential> credentialCompleteList1 = this.GetFullCredentialListA3();

                //4. ONVIF Client checks free storage for additional Credential (in credentialCompleteList1, out credentialToRestore, stateToReastore) by following the procedure mentioned in Annex A.7.
                arrCredToRestore = this.CheckFreeStorageForCredentialA7(credentialCompleteList1);

                //5. ONVIF Client creates credential (in false, out credentialToken) by following the procedure mentioned in Annex A.11.
                Credential cred;
                CredentialState credst;
                credentialToken = this.CreateCredentialA11(out cred, out credst);

                //6. ONVIF client invokes GetCredentialState with parameters
                CredentialState stCreated = this.GetCredentialState(credentialToken);

                Assert(stCreated.Enabled,
                       "The new Credential was created with CredentialState.Enabled = 'true' but GetCredentialState command returned CredentialState item with Enabled = 'false'",
                       "Check CreateCredential and GetCredentialState consistency");

                //8. Client invokes CreatePullPointSubscription with parameters: Filter.TopicExpression := "tns1:Configuration/Credential/Changed"
                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var topics = new List<XmlElement>();
                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Credential/State/Enabled", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                //8. The DUT responds with a CreatePullPointSubscriptionResponse 
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                //10.	ONVIF Client invokes DeleteCredential with parameters 	Token := credentialToken
                var expectedReason = "Test Reason";
                this.DisableCredential(credentialToken, expectedReason);

                Func<NotificationMessageHolderType, bool, string, bool> filterBase =
                    (msg, state, reason)
                       =>
                       messageFilterBase(msg, topicInfo, null, credentialToken,
                                        (log) =>
                                        {
                                            var invalidFlag = false;
                                            var data = msg.Message.GetMessageDataSimpleItems();
                                            if (!data.ContainsKey("State") || !data.ContainsKey("Reason") || !data.ContainsKey("ClientUpdated"))
                                            {
                                                if (!data.ContainsKey("State"))
                                                    log.AppendLine("Received notification has no Data/SimpleItem with Name = 'State'");

                                                if (!data.ContainsKey("Reason"))
                                                    log.AppendLine("Received notification has no Data/SimpleItem with Name = 'Reason'");

                                                if (!data.ContainsKey("ClientUpdated"))
                                                    log.AppendLine("Received notification has no Data/SimpleItem with Name = 'ClientUpdated'");

                                                invalidFlag = true;
                                            }
                                            else
                                            {
                                                var stateStr = data["State"];
                                                var reasonStr = data["Reason"];
                                                var clientUpdatedStr = data["ClientUpdated"];

                                                var receivedState = false;
                                                if (!stateStr.tryConvertXmlBoolean(out receivedState, (value) => log.AppendLine(string.Format("Received notification has Data/SimpleItem with Name = 'State' and Value = '{0}' but this value is not of type xs:boolean", value))))
                                                    invalidFlag = true;

                                                if (receivedState != state)
                                                {
                                                    invalidFlag = true;
                                                    log.AppendLine(string.Format("Received notification has Source/SimpleItem with Name = 'State' and Value = '{0}' but notification with Value = '{1}' is expected", stateStr, XmlConvert.ToString(state)));
                                                }

                                                if (!clientUpdatedStr.tryConvertXmlBoolean((value) => log.AppendLine(string.Format("Received notification has Data/SimpleItem with Name = 'ClientUpdated' and Value = '{0}' but this value is not of type xs:boolean", value))))
                                                    invalidFlag = true;

                                                if (reasonStr != reason)
                                                {
                                                    invalidFlag = true;
                                                    log.AppendLine(string.Format("Received notification has Source/SimpleItem with Name = 'Reason' and Value = '{0}' but notification with Value = '{1}' is expected", reasonStr, expectedReason));
                                                }
                                            }

                                            return invalidFlag;
                                        });

                //11. Until timeout1 timeout expires repeat the following steps
                // Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = msg => filterBase(msg, false, expectedReason) };

                // 12.	Until timeout1 timeout expires ...
                var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Waiting for notification");

                // 13. ONVIF client invokes GetCredentialState with parameters
                CredentialState credStateRetr = this.GetCredentialState(credentialToken);

                // 15. If credentialState1.Enabled equal to credentialState2.Enabled, FAIL the test ang go to step 22.
                Assert(stCreated.Enabled != credStateRetr.Enabled,
                       "Credential state didn't change!",
                       "Verifying changes in credential state");

                // Until timeout1 timeout expires repeat the following steps
                pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = msg => filterBase(msg, true, expectedReason) };

                this.EnableCredential(credentialToken, expectedReason);

                // Until timeout1 timeout expires ...
                notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Waiting for notification");

                // 19. ONVIF client invokes GetCredentialState with parameters
                CredentialState credStateRetr2 = this.GetCredentialState(credentialToken);

                Assert(credStateRetr.Enabled != credStateRetr2.Enabled,
                       "Credential state didn't change!",
                       "Verifying changes in credential state");
            },
            () =>
            {
                // 22. ONVIF Client deletes the Credential (in credentialToken) by following the procedure mentioned in Annex A.6 to restore DUT configuration.
                if (credentialToken != null)
                {
                    this.DeleteCredential(credentialToken);
                }

                this.RestoreCredentialsA10(arrCredToRestore);

                Events.SubscriptionHandler.Unsubscribe(subscription);
            });
        }
        #endregion

        #region 4-1-3 GET CREDENTIAL STATE WITH INVALID TOKEN
        [Test(Name = "GET CREDENTIAL STATE WITH INVALID TOKEN",
            Id = "4-1-3",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_STATE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialState })]
        public void GetCredentialStateWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullCredentialInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                CredentialState state = null;

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    state = this.GetCredentialState(invalidToken);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            });
        }
        #endregion

        #region 4-1-4 ENABLE CREDENTIAL WITH INVALID TOKEN
        [Test(Name = "ENABLE CREDENTIAL WITH INVALID TOKEN",
           Id = "4-1-4",
           Category = Category.CREDENTIAL,
           Path = PATH_CREDENTIAL_STATE,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.Credential },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.EnableCredential })]
        public void EnableCredentialWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullCredentialInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.EnableCredential(invalidToken, "");
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            });
        }
        #endregion

        #region 4-1-5 DISABLE CREDENTIAL WITH INVALID TOKEN
        [Test(Name = "DISABLE CREDENTIAL WITH INVALID TOKEN",
           Id = "4-1-5",
           Category = Category.CREDENTIAL,
           Path = PATH_CREDENTIAL_STATE,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.Credential },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.DisableCredential })]
        public void DisableCredentialWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullCredentialInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.DisableCredential(invalidToken, "");
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            });
        }
        #endregion

        #endregion

        #region 5-1-1 GET CREDENTIAL IDENTIFIERS
        [Test(Name = "GET CREDENTIAL IDENTIFIER",
            Id = "5-1-1",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_IDENTIFIERS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialIdentifiers })]
        public void GetCredentialIdentifiers()
        {
            StringBuilder logger = new StringBuilder();

            RunTest(() =>
            {
                //3. ONVIF Client gets the service capabilities (out cap) by following the procedure mentioned in Annex A.2.
                CredentialServiceCapabilities serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                //4. ONVIF Client retrieves a complete list of credentials (out credentialCompleteList) by following the procedure mentioned in Annex A.3.
                List<Credential> credentialCompleteList = this.GetFullCredentialListA3();

                //5. If credentialCompleteList is empty, skip other steps.
                if (!credentialCompleteList.Any())
                {
                    LogStepEvent("Warning: the DUT returned the empty Credential list");
                    LogStepEvent("");
                    return;
                }

                //6. For each Credential.token token from credentialCompleteList repeat the following steps:
                foreach (Credential cr in credentialCompleteList)
                {
                    // 6.1. ONVIF client invokes GetCredentialIdentifiers with parameters
                    CredentialIdentifier[] arrCrId = (this as ICredentialService).GetCredentialIdentifiers(cr.token);

                    // 6.3. If credentialIdentifierList contains at least two credential identifier items with equal TypeName, FAIL the test and skip other steps.
                    Dictionary<string, CredentialIdentifier> dicCrId = new Dictionary<string, CredentialIdentifier>();
                    foreach (CredentialIdentifier crid in arrCrId)
                    {
                        if (!dicCrId.ContainsKey(crid.Type.Name))
                            dicCrId.Add(crid.Type.Name, crid);
                        else
                            Assert(false, "Credential identifier items type names in the list are not unique!", "Checking credential identifier items type names");
                    }

                    // 6.4. If credentialIdentifierList contains at least one credential identifier item with TypeName other than listed in cap.SupportedIdentifierTypes, FAIL the test and skip other steps.
                    if (true)
                    {
                        Dictionary<string, string> dicSuppTypes = new Dictionary<string, string>();
                        foreach (string stn in serviceCapabilities.SupportedIdentifierType)
                        {
                            if (!dicSuppTypes.ContainsKey(stn))
                                dicSuppTypes.Add(stn, stn);
                        }

                        foreach (CredentialIdentifier crid in arrCrId)
                        {
                            if (!dicSuppTypes.ContainsKey(crid.Type.Name))
                                Assert(false, "Credential identifier items type names should be listed in supported types!", "Checking credential identifier items type names");
                        }
                    }

                    // 6.5. If credentialIdentifierList does not contain all credential identifiers from credentialCompleteList[token = token].CredentialIdentifierList, FAIL the test and skip other steps.
                    if (true)
                    {
                        bool bContainAll = true;
                        foreach (CredentialIdentifier cri in cr.CredentialIdentifier ?? new CredentialIdentifier[0])
                        {
                            if (!dicCrId.ContainsKey(cri.Type.Name))
                            {
                                bContainAll = false;
                                break;
                            }
                        }
                        Assert(bContainAll, "Credential identifier does not contain all credential identifiers from complete list!", "Checking credential identifiers lists");
                    }

                    // 6.6. If credentialIdentifierList contains credential identifiers other than credential identifiers from credentialCompleteList[token = token].CredentialIdentifierList, FAIL the test and skip other steps.
                    if (true)
                    {
                        Dictionary<string, CredentialIdentifier> dicCrIdFull = new Dictionary<string, CredentialIdentifier>();
                        foreach (CredentialIdentifier crid in cr.CredentialIdentifier ?? new CredentialIdentifier[0])
                        {
                            if (!dicCrIdFull.ContainsKey(crid.Type.Name))
                                dicCrIdFull.Add(crid.Type.Name, crid);
                        }

                        foreach (CredentialIdentifier cri in arrCrId)
                        {
                            if (!dicCrIdFull.ContainsKey(cri.Type.Name))
                            {
                                Assert(false, "Credential identifier list contains extra credential identifiers!", "Checking credential identifiers list for extra identifiers");
                            }
                        }
                    }

                    // 6.7. For each credential identifier credentialIdentifier from credentialIdentifierList repeat the following steps:
                    foreach (CredentialIdentifier crid in arrCrId)
                    {
                        // 6.7.1. If credentialIdentifier item does not have equal field values to credentialCompleteList[token = token].CredentialIdentifierList [TypeName = credentialIdentifier.TypeName] item, FAIL the test and skip other steps.
                        foreach (CredentialIdentifier iditem in cr.CredentialIdentifier)
                        {
                            if (iditem.Type.Name == crid.Type.Name)
                            {
                                logger.Clear();
                                logger.AppendLine("Credential identifier item does not have equal field values in credential complete list:");
                                Assert(equalCredentialIdentifier(iditem, crid, logger),
                                       logger.ToStringTrimNewLine(),
                                       "Checking credential identifier item fields");
                            }
                        }
                    }
                }
            });
        }
        #endregion

        #region 5-1-2 SET CREDENTIAL IDENTIFIER – ADDING NEW TYPE
        [Test(Name = "SET CREDENTIAL IDENTIFIER – ADDING NEW TYPE",
            Id = "5-1-2",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_IDENTIFIERS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.SetCredentialIdentifier, Functionality.ConfigurationCredentialChangedEvent })]
        public void SetCredentialIdentifierAddingNewTypeTest()
        {
            string credentialToken = null;
            Events.SubscriptionHandler subscription = null;
            var arrCredToRestore = new CredentialServiceExtensions.CredentialAndState[0];

            RunTest(() =>
            {
                //3. ONVIF Client gets the service capabilities (out cap) by following the procedure mentioned in Annex A.2.
                CredentialServiceCapabilities serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                if (serviceCapabilities.SupportedIdentifierType.Count() < 2)
                {
                    LogStepEvent("WARNING: the DUT supports less than 2 types of CredentialIdentifier. Test passed.");
                    return;
                }

                //4. ONVIF Client retrieves a complete list of credentials (out credentialCompleteList) by following the procedure mentioned in Annex A.3.
                List<Credential> credentialCompleteList = this.GetFullCredentialListA3();

                //5. ONVIF Client checks free storage for additional Credential (in credentialCompleteList1, out credentialToRestore) by following the procedure mentioned in Annex A.7.
                arrCredToRestore = this.CheckFreeStorageForCredentialA7(credentialCompleteList);

                //7. ONVIF client invokes CreateCredential with parameters by following the procedure mentioned in Annex A.11.
                Credential cred;
                CredentialState credst;
                credentialToken = this.CreateCredentialA11(out cred, out credst);
                cred.token = credentialToken;

                var value = this.GetCredentialIdentifierTypeAndValueA15(serviceCapabilities.SupportedIdentifierType.Where(e => e != cred.CredentialIdentifier.First().Type.Name));

                //8. Client invokes CreatePullPointSubscription with parameters: Filter.TopicExpression := "tns1:Configuration/Credential/Changed"
                subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/Credential/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                //9. The DUT responds with a CreatePullPointSubscriptionResponse 
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                // 10. ONVIF client invokes SetCredentialIdentifier with parameters
                var crid = new CredentialIdentifier
                           {
                               Type = new CredentialIdentifierType
                                      {
                                          Name = value.TypeName,
                                          FormatType = value.FormatType
                                      },
                               ExemptedFromAuthentication = false,
                               Value = value.Value
                           };

                this.SetCredentialIdentifier(credentialToken, crid);

                //12. Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Credential.CredentialTokenValidation.xq", new List<Credential>() { cred });

                // 13.	Until timeout1 timeout expires repeat the following steps
                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that SetCredentialIdentifier notification is received");

                var logger = new StringBuilder();
                Assert(ValidateNotifications(notifications, topicInfo, credentialToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");

                // 14. ONVIF client invokes GetCredentialIdentifiers with parameters
                var credentialIDs = this.GetCredentialIdentifiers(credentialToken);

                // 16. If credentialIdentifierList contains more or less than one CredentialIdentifier item, FAIL the test and go to step 30.
                Assert(2 == credentialIDs.Count(),
                       string.Format("GetCredentialIdentifiers should return exactly two items of type CredentialIdentifier but actually it returned {0} items", credentialIDs.Count()),
                       "Checking amount of CredentialIdentifier items returned by GetCredentialIdentifiers");

                logger.Clear();
                ValidateConsistency(cred.CredentialIdentifier.First(), credentialIDs, "CreateCredential", "GetCredentialIdentifiers");

                logger.Clear();
                ValidateConsistency(crid, credentialIDs, "SetCredentialIdentifier", "GetCredentialIdentifiers");
            },
            () =>
            {
                // 29. ONVIF Client deletes the Credential (in credentialToken) by following the procedure mentioned in Annex A.6 to restore DUT configuration.
                if (!string.IsNullOrEmpty(credentialToken))
                    this.DeleteCredential(credentialToken);

                // 30. Restoring 
                this.RestoreCredentialsA10(arrCredToRestore);

                // 31. ONVIF Client sends an Unsubscribe to the subscription endpoint s.
                if (null != subscription)
                    SubscriptionHandler.Unsubscribe(subscription);
            });
        }
        #endregion

        #region 5-1-3 SET CREDENTIAL IDENTIFIER – REPLACE OF THE SAME TYPE
        [Test(Name = "SET CREDENTIAL IDENTIFIER – REPLACE OF THE SAME TYPE",
            Id = "5-1-3",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_IDENTIFIERS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.SetCredentialIdentifier, Functionality.ConfigurationCredentialChangedEvent })]
        public void SetCredentialIdentifierReplaceOfTheSameType()
        {
            string credentialToken = null;
            Events.SubscriptionHandler subscription = null;
            var arrCredToRestore = new CredentialServiceExtensions.CredentialAndState[0];

            RunTest(() =>
            {
                //3. ONVIF Client gets the service capabilities (out cap) by following the procedure mentioned in Annex A.2.
                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                //4. ONVIF Client retrieves a complete list of credentials (out credentialCompleteList) by following the procedure mentioned in Annex A.3.
                List<Credential> credentialCompleteList = this.GetFullCredentialListA3();

                //5. ONVIF Client checks free storage for additional Credential (in credentialCompleteList1, out credentialToRestore) by following the procedure mentioned in Annex A.7.
                arrCredToRestore = this.CheckFreeStorageForCredentialA7(credentialCompleteList);

                var typeNameList = this.GetCredentialIdentifierTypesSupportedAtleastTwoFormatTypesA16();
                if (!typeNameList.Any())
                {
                    LogStepEvent("WARNING: the DUT don't support types of CredentialIdentifier having at least two format types. Test passed.");
                    return;
                }

                CredentialIdentifierValue valueFirst;
                CredentialIdentifierValue valueSecond;
                this.GetCredentialIdentifierTypeAndValueForTypeSupportedAtleastTwoFormatTypesA17(typeNameList, out valueFirst, out valueSecond);

                //7. ONVIF client invokes CreateCredential with parameters by following the procedure mentioned in Annex A.11.
                var credential = new Credential
                             {
                                 token = "",
                                 Description = "Test Description",
                                 CredentialHolderReference = "TestUser",
                                 CredentialIdentifier = new[] { new CredentialIdentifier 
                                                                { 
                                                                    Type = new CredentialIdentifierType { Name = valueFirst.TypeName, FormatType = valueFirst.FormatType }, 
                                                                    Value = valueFirst.Value, 
                                                                    ExemptedFromAuthentication = false 
                                                                } 
                                                              },
                                 CredentialAccessProfile = null,
                                 Extension = null,
                                 ValidFromSpecified = false,
                                 ValidToSpecified = false
                             };

                AntipassbackState antipassbackState = null;

                if (serviceCapabilities.ResetAntipassbackSupported)
                    antipassbackState = new AntipassbackState() { AntipassbackViolated = false };

                var state = new CredentialState
                        {
                            Enabled = true,
                            Reason = "Test Reason",
                            AntipassbackState = antipassbackState
                        };

                credentialToken = this.CreateCredential(credential, state);
                credential.token = credentialToken;


                //8. Client invokes CreatePullPointSubscription with parameters: Filter.TopicExpression := "tns1:Configuration/Credential/Changed"
                subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/Credential/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                //9. The DUT responds with a CreatePullPointSubscriptionResponse 
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                // 10. ONVIF client invokes SetCredentialIdentifier with parameters
                var crid = new CredentialIdentifier
                           {
                               Type = new CredentialIdentifierType
                                      {
                                          Name = valueSecond.TypeName,
                                          FormatType = valueSecond.FormatType
                                      },
                               ExemptedFromAuthentication = false,
                               Value = valueSecond.Value
                           };

                this.SetCredentialIdentifier(credentialToken, crid);

                //12. Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Credential.CredentialTokenValidation.xq", new List<Credential>() { credential });

                // 13.	Until timeout1 timeout expires repeat the following steps
                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that SetCredentialIdentifier notification is received");

                var logger = new StringBuilder();
                Assert(ValidateNotifications(notifications, topicInfo, credentialToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");

                // 14. ONVIF client invokes GetCredentialIdentifiers with parameters
                var credentialIDs = this.GetCredentialIdentifiers(credentialToken);

                // 16. If credentialIdentifierList contains more or less than one CredentialIdentifier item, FAIL the test and go to step 30.
                Assert(1 == credentialIDs.Count(),
                       string.Format("GetCredentialIdentifiers should return exactly 1 items of type CredentialIdentifier but actually it returned {0} items", credentialIDs.Count()),
                       "Check amount of CredentialIdentifier items returned by GetCredentialIdentifiers");

                ValidateConsistency(crid, credentialIDs, "SetCredentialIdentifier", "GetCredentialIdentifiers");
            },
            () =>
            {
                // 29. ONVIF Client deletes the Credential (in credentialToken) by following the procedure mentioned in Annex A.6 to restore DUT configuration.
                if (!string.IsNullOrEmpty(credentialToken))
                    this.DeleteCredential(credentialToken);

                // 30. Restoring 
                this.RestoreCredentialsA10(arrCredToRestore);

                // 31. ONVIF Client sends an Unsubscribe to the subscription endpoint s.
                if (null != subscription)
                    SubscriptionHandler.Unsubscribe(subscription);
            });
        }
        #endregion

        #region 5-1-4 DELETE CREDENTIAL IDENTIFIER
        [Test(Name = "DELETE CREDENTIAL IDENTIFIER",
            Id = "5-1-4",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_IDENTIFIERS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCredentialIdentifier, Functionality.ConfigurationCredentialChangedEvent })]
        public void DeleteCredentialIdentifier()
        {
            string credentialToken = null;
            Events.SubscriptionHandler subscription = null;
            var arrCredToRestore = new CredentialServiceExtensions.CredentialAndState[0];

            RunTest(() =>
            {
                //3. ONVIF Client gets the service capabilities (out cap) by following the procedure mentioned in Annex A.2.
                CredentialServiceCapabilities serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();
                if (serviceCapabilities.SupportedIdentifierType.Count() < 2)
                {
                    LogStepEvent("WARNING: the DUT supports less than 2 CredentialIdentifier types. Test passed.");
                    return;
                }

                //4. ONVIF Client retrieves a complete list of credentials (out credentialCompleteList) by following the procedure mentioned in Annex A.3.
                List<Credential> credentialCompleteList = this.GetFullCredentialListA3();

                //5. ONVIF Client checks free storage for additional Credential (in credentialCompleteList1, out credentialToRestore) by following the procedure mentioned in Annex A.7.
                arrCredToRestore = this.CheckFreeStorageForCredentialA7(credentialCompleteList);


                // 7. ONVIF Client creates credential with two Credential identifier items (out typeName1), (out typeName2), with corresponding Format Types (out formatType1), (out formatType2) and with corresponding values (out value1), (out value2), with antipass back state equal to false (in false), and with credential token (out credentialToken) by following the procedure mentioned in Annex A.18.
                CredentialIdentifier credentialIdentifierFirst;
                CredentialIdentifier credentialIdentifierSecond;
                credentialToken = this.CreateCredentialWithTwoCredentialIdentifierItemsA18(false, serviceCapabilities, out credentialIdentifierFirst, out credentialIdentifierSecond);

                //10. Client invokes CreatePullPointSubscription with parameters: Filter.TopicExpression := "tns1:Configuration/Credential/Changed"
                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/Credential/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                //11. The DUT responds with a CreatePullPointSubscriptionResponse 
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                //12. ONVIF client invokes DeleteCredentialIdentifier with parameters
                this.DeleteCredentialIdentifier(credentialToken, credentialIdentifierFirst.Type.Name);

                //14. Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new Events.SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Credential.CredentialTokenValidation.xq", new List<Credential>() { new Credential() { token = credentialToken, Description = "" } });

                // 15.	Until timeout1 timeout expires repeat the following steps
                var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that SetCredentialIdentifier notification is received");

                var logger = new StringBuilder();
                Assert(ValidateNotifications(notifications, topicInfo, credentialToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");

                // 16.	ONVIF client invokes GetCredentialIdentifiers with parameters
                var credentialIDs = this.GetCredentialIdentifiers(credentialToken);

                // 18. If credentialIdentifierList contains one or more items, FAIL the test and go to step 19.
                Assert(1 == credentialIDs.Count(),
                       string.Format("GetCredentialIdentifiers should return exactly 1 items of type CredentialIdentifier but actually it returned {0} items", credentialIDs.Count()),
                       "Check amount of CredentialIdentifier items returned by GetCredentialIdentifiers");

                ValidateConsistency(credentialIdentifierSecond, credentialIDs, "CreateCredential", "GetCredentialIdentifiers");
            },
            () =>
            {
                // 29. ONVIF Client deletes the Credential (in credentialToken) by following the procedure mentioned in Annex A.6 to restore DUT configuration.
                if (credentialToken != null)
                    this.DeleteCredential(credentialToken);

                // 30. Restoring 
                this.RestoreCredentialsA10(arrCredToRestore);

                // 31. ONVIF Client sends an Unsubscribe to the subscription endpoint s.
                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);
            });
        }
        #endregion

        #region 5-1-5 GET SUPPORTED FORMAT TYPES
        [Test(Name = "GET SUPPORTED FORMAT TYPES",
            Id = "5-1-5",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_IDENTIFIERS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSupportedFormatTypes })]
        public void GetSupportedFormatTypeTest()
        {
            RunTest(() =>
            {
                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                var formatTypes = new Dictionary<string, IEnumerable<CredentialIdentifierFormatTypeInfo>>();
                foreach (var typeName in serviceCapabilities.SupportedIdentifierType)
                {
                    //The scheme is mandated at least one element to be present
                    var supportedFormatTypes = this.GetSupportedFormatTypes(typeName);

                    Assert(supportedFormatTypes.Any(),
                           string.Format("GetSupportedFormatTypes command returned no supported format types for CredentialIdentifierType with ID = '{0}' that is declared as supported", typeName),
                           "Check GetSupportedFormatTypes returned at least one CredentialIdentifierFormatTypeInfo for specified CredentialIdentifierType");

                    Assert(supportedFormatTypes.Select(e => e.FormatType).Distinct().Count() == supportedFormatTypes.Count(),
                           "The DUT returned CredentialIdentifierFormatTypeInfo items with the same name",
                           "Checking all CredentialIdentifierFormatTypeInfo items are distinct");

                    //var unknownFormatTypes = supportedFormatTypes.Where(e => !SupportedCredentialIdentifierFormatTypesA14().Contains(e.FormatType));
                    //Assert(!unknownFormatTypes.Any(),
                    //       string.Format("The DUT returned CredentialIdentifierFormatTypeInfo item(s) with unknown FormatType: {0}", string.Join(" ", unknownFormatTypes.Select(e => e.FormatType))),
                    //       "Checking all CredentialIdentifierFormatTypeInfo items are known types");

                    formatTypes[typeName] = supportedFormatTypes;
                }

                var credentialCompleteList = this.GetFullCredentialListA3();

                if (!credentialCompleteList.Any())
                    return;

                foreach (var credential in credentialCompleteList)
                {
                    var credentialIdentifierList = this.GetCredentialIdentifiers(credential.token);

                    foreach (var credentialIdentifier in credentialIdentifierList.Where(e => null != e.Type && !string.IsNullOrEmpty(e.Type.Name)))
                    {
                        var flag = formatTypes.ContainsKey(credentialIdentifier.Type.Name) && formatTypes[credentialIdentifier.Type.Name].Any(e => credentialIdentifier.Type.FormatType == e.FormatType);
                        var msg = string.Empty;
                        if (!flag)
                        {
                            if (formatTypes.ContainsKey(credentialIdentifier.Type.Name))
                                msg = string.Format("CredentialIdentifier item with Type.Name = '{0}' has FormatType = '{1}' that wasn't present in GetSupportedFormatTypes response", credentialIdentifier.Type.Name, credentialIdentifier.Type.FormatType);
                            else
                                msg = string.Format("Type.Name = '{0}' isn't specified in CredentialServiceCapabilities.SupportedIdentifierType field", credentialIdentifier.Type.Name);
                        }

                        Assert(flag, msg, "Check GetCredentialIdentifiers and GetSupportedFormatTypes consistency");
                    }
                }
            });
        }
        #endregion

        #region 5-1-6 GET CREDENTIAL IDENTIFIERS WITH INVALID TOKEN
        [Test(Name = "GET CREDENTIAL IDENTIFIERS WITH INVALID TOKEN",
           Id = "5-1-6",
           Category = Category.CREDENTIAL,
           Path = PATH_CREDENTIAL_IDENTIFIERS,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.Credential },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialIdentifiers })]
        public void GetCredentialIdentifiersWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullCredentialInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.GetCredentialIdentifiers(invalidToken);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            });
        }
        #endregion

        #region 5-1-7 SET CREDENTIAL IDENTIFIER WITH INVALID TOKEN
        [Test(Name = "SET CREDENTIAL IDENTIFIER WITH INVALID TOKEN",
           Id = "5-1-7",
           Category = Category.CREDENTIAL,
           Path = PATH_CREDENTIAL_IDENTIFIERS,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.Credential },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.SetCredentialIdentifier })]
        public void SetCredentialIdentifierWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullCredentialInfoListA1();

                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();
                var value = this.GetCredentialIdentifierTypeAndValueA15(serviceCapabilities.SupportedIdentifierType);

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                CredentialIdentifier credentialIdentifier = new CredentialIdentifier()
                {
                    Type = new CredentialIdentifierType { Name = value.TypeName, FormatType = value.FormatType },
                    ExemptedFromAuthentication = false,
                    Value = value.Value
                };

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.SetCredentialIdentifier(invalidToken, credentialIdentifier);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            });
        }
        #endregion

        #region 5-1-8 DELETE CREDENTIAL IDENTIFIER WITH INVALID CREDENTIAL TOKEN
        [Test(Name = "DELETE CREDENTIAL IDENTIFIER WITH INVALID CREDENTIAL TOKEN",
           Id = "5-1-8",
           Category = Category.CREDENTIAL,
           Path = PATH_CREDENTIAL_IDENTIFIERS,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.Credential },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCredentialIdentifier })]
        public void DeleteCredentialIdentifierWithInvalidTokenTest()
        {
            RunTest(() =>
            {

                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                var fullList = this.GetFullCredentialInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.DeleteCredentialIdentifier(invalidToken, serviceCapabilities.SupportedIdentifierType[0]);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            });
        }
        #endregion

        #region 5-1-9 DELETE CREDENTIAL IDENTIFIER WITH INVALID IDENTIFIER TYPE
        [Test(Name = "DELETE CREDENTIAL IDENTIFIER WITH INVALID IDENTIFIER TYPE",
           Id = "5-1-9",
           Category = Category.CREDENTIAL,
           Path = PATH_CREDENTIAL_IDENTIFIERS,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.Credential },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCredentialIdentifier })]
        public void DeleteCredentialIdentifierWithInvalidIdentifierTypeTest()
        {
            string credentialToken = "";
            RunTest(() =>
            {

                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                if (serviceCapabilities.SupportedIdentifierType.Length < 2)
                {
                    LogStepEvent("WARNING: the DUT supports less than 2 CredentialIdentifier types. Test passed.");
                    return;
                }
                CredentialIdentifier credentialIdentifierFirst;
                CredentialIdentifier credentialIdentifierSecond;
                credentialToken = this.CreateCredentialWithTwoCredentialIdentifierItemsA18(false, serviceCapabilities, out credentialIdentifierFirst, out credentialIdentifierSecond);

                string invalidIdentifierType = serviceCapabilities.SupportedIdentifierType.ToList().GetNonMatchingString();

                this.DeleteCredentialIdentifier(credentialToken, invalidIdentifierType);

                var credentialIdentifierList = this.GetCredentialIdentifiers(credentialToken);

                Assert(credentialIdentifierList.Length == 2,
                    credentialIdentifierList.Length < 2 ?
                    string.Format("The DUT returns Get Credential Identifiers supports less than 2 Credential Identifier types") :
                    string.Format("The DUT returns Get Credential Identifiers supports more than 2 Credential Identifier types"),
                    "Checking received GetCredentialIdentifiers list");

                Assert(credentialIdentifierList.Select(e => e.Type.Name).Contains(credentialIdentifierFirst.Type.Name),
                    string.Format("The DUT returns Get Credential Identifiers where Credential Identifier does not contains type name = {0}", credentialIdentifierFirst.Type.Name),
                    "Checking received GetCredentialIdentifiers list");

                Assert(credentialIdentifierList.Select(e => e.Type.Name).Contains(credentialIdentifierSecond.Type.Name),
                    string.Format("The DUT returns Get Credential Identifiers where Credential Identifier does not contains type name = {0}", credentialIdentifierSecond.Type.Name),
                    "Checking received GetCredentialIdentifiers list");

            },
            () =>
            {
                if (!string.IsNullOrEmpty(credentialToken))
                {
                    this.DeleteCredential(credentialToken);
                }
            });
        }
        #endregion

        #region 5-1-10 DELETE CREDENTIAL IDENTIFIER - MIN IDENTIFIERS PER CREDENTIAL
        [Test(Name = "DELETE CREDENTIAL IDENTIFIER - MIN IDENTIFIERS PER CREDENTIAL",
           Id = "5-1-10",
           Category = Category.CREDENTIAL,
           Path = PATH_CREDENTIAL_IDENTIFIERS,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.Credential },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCredentialIdentifier })]
        public void DeleteCredentialIdentifierMinIdentifiersPerCredentialTest()
        {
            string credentialToken = "";
            RunTest(() =>
            {
                string credentialTypeName = "";
                credentialToken = this.CreateCredentialA11(out credentialTypeName);

                string faultCode = "Receiver/ConstraintViolated/MinIdentifiersPerCredential";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.DeleteCredentialIdentifier(credentialToken, credentialTypeName);
                }
                catch (FaultException e)
                {

                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            },
            () =>
            {
                if (!string.IsNullOrEmpty(credentialToken))
                {
                    this.DeleteCredential(credentialToken);
                }
            });
        }
        #endregion

        #region 6-1-*

        [Test(Name = "GET CREDENTIAL ACCESS PROFILES",
            Id = "6-1-1",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_ACCESS_PROFILES,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialAccessProfiles })]
        public void GetCredentialAccessProfilesTest()
        {
            var logger = new StringBuilder();

            RunTest(() =>
            {
                // 3. ONVIF Client gets the service capabilities (out cap) by following the procedure mentioned in Annex A.2.
                CredentialServiceCapabilities serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                // 4. ONVIF Client retrieves a complete list of credentials (out credentialCompleteList) by following the procedure mentioned in Annex A.3.
                List<Credential> credentialCompleteList = this.GetFullCredentialListA3();

                // 5. If credentialCompleteList is empty, skip other steps.
                if (!credentialCompleteList.Any())
                {
                    LogStepEvent("WARNING: The DUT returned no Credential items. Test passed.");
                    LogStepEvent("");

                    return;
                }

                // 6. For each Credential.token token from credentialCompleteList repeat the following steps:
                foreach (Credential cr in credentialCompleteList)
                {
                    // 6.1. ONVIF client invokes GetCredentialAccessProfiles with parameters
                    var credentialAccessProfiles = this.GetCredentialAccessProfiles(cr.token);

                    // 6.4. If credentialAccessProfileList contains more AccessProfileInfo items than cap.MaxAccessProfilesPerCredential, FAIL the test and skip other steps.
                    Assert(credentialAccessProfiles.Count() <= serviceCapabilities.MaxAccessProfilesPerCredential,
                           "CredentialAccessProfile list contains more items than MaxAccessProfilesPerCredential",
                           "Checking size of CredentialAccessProfile list");

                    logger.Clear();
                    logger.AppendLine(singleTab + "CredentialAccessProfile lists are inconsistent.");
                    Assert(equalCredentialAccessProfileLists(cr.CredentialAccessProfile, credentialAccessProfiles, logger, "GetCredentialList", "GetCredentialAccessProfiles"),
                           logger.ToStringTrimNewLine(),
                           "Checking CredentialAccessProfile lists");
                }
            });
        }

        [Test(Name = "SET CREDENTIAL ACCESS PROFILES - ADDING NEW ACCESS PROFILE",
            Id = "6-1-2",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_ACCESS_PROFILES,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential, Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.SetCredentialAccessProfiles, Functionality.ConfigurationCredentialChangedEvent })]
        public void SetCredentialAccessProfilesAddingNewAccessProfileTest()
        {
            var credentialToken = string.Empty;
            var accessProfileToken = string.Empty;
            Events.SubscriptionHandler subscription = null;
            CredentialServiceExtensions.CredentialAndState? credentialToRestore = null;

            RunTest(() =>
            {
                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();
                var fullAccessProfileList = this.GetFullAccessProfilesListA3();

                if (!fullAccessProfileList.Any())
                {
                    AccessProfile o;
                    accessProfileToken = this.CreateAccessProfileA11(out o, new Proxies.Onvif.Schedule[0], new AccessPointInfo[0]);
                    fullAccessProfileList.Add(o);
                }

                var credentialCompleteList = this.GetFullCredentialListA3();

                credentialToRestore = this.CheckFreeStorageForCredentialA7(credentialCompleteList).FirstOrDefault();

                credentialToken = this.CreateCredentialA11();

                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/Credential/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                var newCredentialAccessProfile = new CredentialAccessProfile()
                                                 {
                                                     AccessProfileToken = fullAccessProfileList.First().token,
                                                     ValidFromSpecified = serviceCapabilities.CredentialAccessProfileValiditySupported,
                                                     ValidFrom = System.DateTime.Now,
                                                     ValidToSpecified = serviceCapabilities.CredentialAccessProfileValiditySupported,
                                                     ValidTo = System.DateTime.Now.AddYears(1)
                                                 };
                this.SetCredentialAccessProfiles(credentialToken, new[] { newCredentialAccessProfile });

                var pollingCondition = new Events.SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Credential.CredentialTokenValidation.xq", new List<Credential>() { new Credential() { token = credentialToken, Description = "" } });

                var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Waiting for notification");

                var logger = new StringBuilder();
                Assert(ValidateNotifications(notifications, topicInfo, credentialToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");

                var credentialAccessProfilesList = this.GetCredentialAccessProfiles(credentialToken);
                Assert(1 == credentialAccessProfilesList.Count(),
                       string.Format("Received CredentialAccessProfile list contains {0} item. Expected: 1", credentialAccessProfilesList.Any() ? credentialAccessProfilesList.Count().ToString() : "no"),
                       "Checking received CredentialAccessProfile list");
                Assert(fullAccessProfileList.First().token == credentialAccessProfilesList.First().AccessProfileToken,
                       string.Format("Received CredentialAccessProfile token has unexpected value {0}. Expected: {1}", credentialAccessProfilesList.First().AccessProfileToken, fullAccessProfileList.First().token),
                       "Checking received CredentialAccessProfile token");

                logger.Clear();
                if (serviceCapabilities.CredentialAccessProfileValiditySupported && !serviceCapabilities.ValiditySupportsTimeValue)
                {
                    Assert(equalCredentialAccessProfileTimeValueUnsupported(credentialAccessProfilesList.First(), newCredentialAccessProfile, logger, "GetCredentialAccessProfiles", "SetCredentialAccessProfiles"),
                           logger.ToStringTrimNewLine(),
                           "Checking received CredentialAccessProfile");
                }
                else
                {
                    Assert(equalCredentialAccessProfile(credentialAccessProfilesList.First(), newCredentialAccessProfile, logger, "GetCredentialAccessProfiles", "SetCredentialAccessProfiles"),
                           logger.ToStringTrimNewLine(),
                           "Checking received CredentialAccessProfile");
                }
            },
            () =>
            {
                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(credentialToken))
                        this.DeleteCredential(credentialToken);
                });

                this.AllowFaultStep(() =>
                {
                    if (credentialToRestore.HasValue && credentialToRestore.Value.credential != null)
                        this.RestoreCredentialsA10(credentialToRestore.Value);
                });

                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(accessProfileToken))
                        this.DeleteAccessProfile(accessProfileToken);
                });

                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);

                this.FinishRestoreSettings();
            });
        }

        [Test(Name = "SET CREDENTIAL ACCESS PROFILES - UPDATING ACCESS PROFILE",
            Id = "6-1-3",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_ACCESS_PROFILES,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential, Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.SetCredentialAccessProfiles, Functionality.ConfigurationCredentialChangedEvent })]
        public void SetCredentialAccessProfilesUpdatingAccessProfileTest()
        {
            var credentialToken = string.Empty;
            var accessProfileToken = string.Empty;
            Events.SubscriptionHandler subscription = null;
            CredentialServiceExtensions.CredentialAndState? credentialToRestore = null;

            RunTest(() =>
            {
                var serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();
                var fullAccessProfileList = this.GetFullAccessProfilesListA3();
                if (!fullAccessProfileList.Any())
                {
                    AccessProfile o;
                    accessProfileToken = this.CreateAccessProfileA11(out o, new Proxies.Onvif.Schedule[0], new AccessPointInfo[0]);
                    fullAccessProfileList.Add(o);
                }


                var credentialCompleteList = this.GetFullCredentialListA3();

                credentialToRestore = this.CheckFreeStorageForCredentialA7(credentialCompleteList).FirstOrDefault();

                credentialToken = this.CreateCredentialA11();

                System.DateTime validFromDateTime1 = System.DateTime.Now;
                System.DateTime validToDateTime1 = validFromDateTime1.AddYears(1);

                var newCredentialAccessProfile = new CredentialAccessProfile()
                                                 {
                                                     AccessProfileToken = fullAccessProfileList.First().token,
                                                     ValidFromSpecified = serviceCapabilities.CredentialAccessProfileValiditySupported,
                                                     ValidFrom = validFromDateTime1,
                                                     ValidToSpecified = serviceCapabilities.CredentialAccessProfileValiditySupported,
                                                     ValidTo = validToDateTime1
                                                 };
                this.SetCredentialAccessProfiles(credentialToken, new[] { newCredentialAccessProfile });

                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/Credential/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                newCredentialAccessProfile = new CredentialAccessProfile()
                                             {
                                                 AccessProfileToken = fullAccessProfileList.First().token,
                                                 ValidFromSpecified = serviceCapabilities.CredentialAccessProfileValiditySupported,
                                                 ValidFrom = validFromDateTime1.AddDays(1).AddHours(1),
                                                 ValidToSpecified = serviceCapabilities.CredentialAccessProfileValiditySupported,
                                                 ValidTo = validToDateTime1.AddDays(1).AddHours(1)
                                             };
                this.SetCredentialAccessProfiles(credentialToken, new[] { newCredentialAccessProfile });

                var pollingCondition = new Events.SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Credential.CredentialTokenValidation.xq", new List<Credential>() { new Credential() { token = credentialToken, Description = "" } });

                var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Waiting for notification");

                var logger = new StringBuilder();
                Assert(ValidateNotifications(notifications, topicInfo, credentialToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");


                var credentialAccessProfilesList = this.GetCredentialAccessProfiles(credentialToken);
                Assert(1 == credentialAccessProfilesList.Count(),
                       string.Format("Received CredentialAccessProfile list contains {0} item. Expected: 1", credentialAccessProfilesList.Any() ? credentialAccessProfilesList.Count().ToString() : "no"),
                       "Checking received CredentialAccessProfile list");

                Assert(fullAccessProfileList.First().token == credentialAccessProfilesList.First().AccessProfileToken,
                       string.Format("Received CredentialAccessProfile token has unexpected value {0}. Expected: {1}", credentialAccessProfilesList.First().AccessProfileToken, fullAccessProfileList.First().token),
                       "Checking received CredentialAccessProfile token");


                logger.Clear();
                if (serviceCapabilities.CredentialAccessProfileValiditySupported && !serviceCapabilities.ValiditySupportsTimeValue)
                {
                    Assert(equalCredentialAccessProfileTimeValueUnsupported(credentialAccessProfilesList.First(), newCredentialAccessProfile, logger, "GetCredentialAccessProfiles", "SetCredentialAccessProfiles"),
                           logger.ToStringTrimNewLine(),
                           "Checking received CredentialAccessProfile");
                }
                else
                {
                    Assert(equalCredentialAccessProfile(credentialAccessProfilesList.First(), newCredentialAccessProfile, logger, "GetCredentialAccessProfiles", "SetCredentialAccessProfiles"),
                           logger.ToStringTrimNewLine(),
                           "Checking received CredentialAccessProfile");
                }
            },
            () =>
            {

                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(credentialToken))
                        this.DeleteCredential(credentialToken);
                });

                this.AllowFaultStep(() =>
                {
                    if (credentialToRestore.HasValue && credentialToRestore.Value.credential != null)
                        this.RestoreCredentialsA10(credentialToRestore.Value);
                });

                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(accessProfileToken))
                        this.DeleteAccessProfile(accessProfileToken);
                });

                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);

                this.FinishRestoreSettings();

            });
        }

        [Test(Name = "DELETE CREDENTIAL ACCESS PROFILES",
            Id = "6-1-4",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_ACCESS_PROFILES,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential, Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCredentialIdentifier, Functionality.ConfigurationCredentialChangedEvent })]
        public void DeleteCredentialAccessProfilesTest()
        {
            var credentialToken = string.Empty;
            var accessProfileToken = string.Empty;
            Events.SubscriptionHandler subscription = null;
            CredentialServiceExtensions.CredentialAndState? credentialToRestore = null;

            RunTest(() =>
            {
                var fullAccessProfileList = this.GetFullAccessProfilesListA3();
                if (!fullAccessProfileList.Any())
                {
                    AccessProfile o;
                    accessProfileToken = this.CreateAccessProfileA11(out o, new Proxies.Onvif.Schedule[0], new AccessPointInfo[0]);
                    fullAccessProfileList.Add(o);
                }


                var credentialCompleteList = this.GetFullCredentialListA3();

                credentialToRestore = this.CheckFreeStorageForCredentialA7(credentialCompleteList).FirstOrDefault();

                credentialToken = this.CreateCredentialA11();

                var newAccessProfileToken = fullAccessProfileList.First().token;
                var newCredentialAccessProfile = new CredentialAccessProfile()
                                                 {
                                                     AccessProfileToken = newAccessProfileToken,
                                                     ValidFromSpecified = false,
                                                     ValidToSpecified = false,
                                                 };
                this.SetCredentialAccessProfiles(credentialToken, new[] { newCredentialAccessProfile });

                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/Credential/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                this.DeleteCredentialAccessProfiles(credentialToken, new[] { newAccessProfileToken });

                var pollingCondition = new Events.SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Credential.CredentialTokenValidation.xq", new List<Credential>() { new Credential() { token = credentialToken, Description = "" } });

                var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Waiting for notification");

                var logger = new StringBuilder();
                Assert(ValidateNotifications(notifications, topicInfo, credentialToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");

                var credentialAccessProfilesList = this.GetCredentialAccessProfiles(credentialToken);
                Assert(!credentialAccessProfilesList.Any(),
                       string.Format("Received CredentialAccessProfile list contains {0} item(s). Expected empty list.", credentialAccessProfilesList.Count()),
                       "Checking received CredentialAccessProfile list");
            },
            () =>
            {
                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(credentialToken))
                        this.DeleteCredential(credentialToken);
                });

                this.AllowFaultStep(() =>
                {
                    if (credentialToRestore.HasValue && credentialToRestore.Value.credential != null)
                        this.RestoreCredentialsA10(credentialToRestore.Value);
                });

                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(accessProfileToken))
                        this.DeleteAccessProfile(accessProfileToken);
                });

                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);

                this.FinishRestoreSettings();

            });
        }

        #region 6-1-5 GET CREDENTIAL ACCESS PROFILES WITH INVALID TOKEN
        [Test(Name = "GET CREDENTIAL ACCESS PROFILES WITH INVALID TOKEN",
           Id = "6-1-5",
           Category = Category.CREDENTIAL,
           Path = PATH_CREDENTIAL_ACCESS_PROFILES,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.Credential },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentialAccessProfiles })]
        public void GetCredentialAccessProfilesWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullCredentialInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.GetCredentialAccessProfiles(invalidToken);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            });
        }
        #endregion

        #region 6-1-6 SET CREDENTIAL ACCESS PROFILES WITH INVALID CREDENTIAL TOKEN
        [Test(Name = "SET CREDENTIAL ACCESS PROFILES WITH INVALID CREDENTIAL TOKEN",
           Id = "6-1-6",
           Category = Category.CREDENTIAL,
           Path = PATH_CREDENTIAL_ACCESS_PROFILES,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.Credential, Feature.AccessRulesService },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.SetCredentialAccessProfiles })]
        public void SetCredentialAccessProfilesWithInvalidCredentialTokenTest()
        {
            string newAccessProfileToken = "";
            RunTest(() =>
            {
                // 3.	ONVIF Client retrieves a complete list of access profiles (out accessProfileCompleteList) by following the procedure mentioned in Annex A.5.
                List<AccessProfile> fullAccessProfileList = this.GetFullAccessProfilesListA3();

                string accessProfileToken = fullAccessProfileList.Select(e => e.token).GetNonMatchingString();

                if (fullAccessProfileList == null || !fullAccessProfileList.Any())
                {
                    AccessProfile newAccessProfile;
                    newAccessProfileToken = this.CreateAccessProfileA11(out newAccessProfile, new Proxies.Onvif.Schedule[0], new AccessPointInfo[0]);
                    fullAccessProfileList = fullAccessProfileList ?? new List<AccessProfile>();
                    fullAccessProfileList.Add(newAccessProfile);
                }

                var fullCredentialList = this.GetFullCredentialInfoListA1();

                string credentialToken = fullCredentialList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.SetCredentialAccessProfiles(credentialToken, new CredentialAccessProfile[] { new CredentialAccessProfile() { AccessProfileToken = fullAccessProfileList[0].token } });
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            },
            () =>
            {
                if (!string.IsNullOrEmpty(newAccessProfileToken))
                {
                    this.DeleteAccessProfile(newAccessProfileToken);
                }
            });
        }
        #endregion

        #region 6-1-7 DELETE CREDENTIAL ACCESS PROFILES WITH INVALID CREDENTIAL TOKEN
        [Test(Name = "DELETE CREDENTIAL ACCESS PROFILES WITH INVALID CREDENTIAL TOKEN",
           Id = "6-1-7",
           Category = Category.CREDENTIAL,
           Path = PATH_CREDENTIAL_ACCESS_PROFILES,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.Credential, Feature.AccessRulesService },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.DeleteCredentialAccessProfiles })]
        public void DeleteCredentialAccessProfilesWithInvalidCredentialTokenTest()
        {
            string newAccessProfileToken = "";
            RunTest(() =>
            {
                // 3.	ONVIF Client retrieves a complete list of access profiles (out accessProfileCompleteList) by following the procedure mentioned in Annex A.5.
                List<AccessProfile> fullAccessProfileList = this.GetFullAccessProfilesListA3();

                string accessProfileToken = fullAccessProfileList.Select(e => e.token).GetNonMatchingString();

                if (fullAccessProfileList == null || !fullAccessProfileList.Any())
                {
                    AccessProfile newAccessProfile;
                    newAccessProfileToken = this.CreateAccessProfileA11(out newAccessProfile, new Proxies.Onvif.Schedule[0], new AccessPointInfo[0]);
                    fullAccessProfileList = fullAccessProfileList ?? new List<AccessProfile>();
                    fullAccessProfileList.Add(newAccessProfile);
                }

                var fullCredentialList = this.GetFullCredentialInfoListA1();

                string credentialToken = fullCredentialList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.DeleteCredentialAccessProfiles(credentialToken, new string[] { fullAccessProfileList[0].token });
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            },
            () =>
            {
                if (!string.IsNullOrEmpty(newAccessProfileToken))
                {
                    this.DeleteAccessProfile(newAccessProfileToken);
                }
            });
        }
        #endregion

        #endregion

        #region 7-1-*

        #region 7-1-1 RESET ANTIPASSBACK VIOLATIONS
        [Test(Name = "RESET ANTIPASSBACK VIOLATIONS",
            Id = "7-1-1",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_RESET_ANTIPASSBACK_VIOLATIONS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential, Feature.ResetAntipassbackViolation },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.ResetAntipassbackViolation, Functionality.CredentialStateApbViolationEvent })]
        public void ResetAntipassbackTest()
        {
            string credentialToken = null;
            Events.SubscriptionHandler subscription = null;
            CredentialServiceExtensions.CredentialAndState[] arrCredToRestore = new CredentialServiceExtensions.CredentialAndState[0];
            StringBuilder logger = new StringBuilder();

            RunTest(() =>
            {
                //3. ONVIF Client gets the service capabilities (out cap) by following the procedure mentioned in Annex A.2.
                CredentialServiceCapabilities serviceCapabilities = (this as ICredentialService).GetServiceCapabilities();

                //4. If cap.ResetAntipassbackViolationsSupported is equal to false, FAIL the test and skip other steps.
                Assert(serviceCapabilities.ResetAntipassbackSupported, "ResetAntipassbackSupported should be supported", "Verifying ResetAntipassbackSupported value");

                //5. ONVIF Client retrieves a complete list of credentials (out credentialCompleteList) by following the procedure mentioned in Annex A.3.
                List<Credential> credentialCompleteList = this.GetFullCredentialListA3();

                //6. ONVIF Client checks free storage for additional Credential (in credentialCompleteList1, out credentialToRestore) by following the procedure mentioned in Annex A.7.
                arrCredToRestore = this.CheckFreeStorageForCredentialA7(credentialCompleteList);

                //7. ONVIF client invokes CreateCredential with parameters by following the procedure mentioned in Annex A.11.
                Credential cred;
                CredentialState credst;
                credentialToken = this.CreateCredentialA11(out cred, out credst, true);

                // 8. ONVIF client retrives credential state (in credentialToken, out credentialState) by following the procedure mentioned in Annex A.13.
                CredentialState crState = this.GetCredentialState(credentialToken);
                // 9. If credentialState does not contain AntipassbackState elemet, FAIL the test and go to step 20.
                Assert(crState.AntipassbackState != null, "AntipassbackState flag shall not be skipped.", "Check that AntipassbackState specified");
                // 10. If credentialState.AntipassbackState.AntipassbackViolated equal to false, FAIL the test and go to step 20.
                Assert(crState.AntipassbackState.AntipassbackViolated != false, "ResetAntipassbackViolation flag shouldn't be 'false'", "Checking ResetAntipassbackViolation is 'true'");

                //10. Client invokes CreatePullPointSubscription with parameters: Filter.TopicExpression := "tns1:Configuration/Credential/Changed"
                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var topics = new List<XmlElement>();
                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Credential/State/ApbViolation", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);
                var eventNode = GetTopicElement(topics, topicInfo);

                const int messageLimit = 1;
                var filter = CreateFilter(topicInfo, eventNode);

                //11. The DUT responds with a CreatePullPointSubscriptionResponse 
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter.Filter, -1);

                // ONVIF Client invokes ResetAntipassbackViolation with parameters 	Token := credentialToken
                this.ResetAntipassbackViolation(credentialToken);

                Func<NotificationMessageHolderType, bool, bool> filterBase =
                    (msg, expectedApbViolation)
                       =>
                       messageFilterBase(msg, topicInfo, null, credentialToken,
                                        (log) =>
                                        {
                                            var invalidFlag = false;
                                            var data = msg.Message.GetMessageDataSimpleItems();
                                            if (!data.ContainsKey("ApbViolation") || !data.ContainsKey("ClientUpdated"))
                                            {
                                                if (!data.ContainsKey("ApbViolation"))
                                                    log.AppendLine("Received notification has no Data/SimpleItem with Name = 'ApbViolation'");

                                                if (!data.ContainsKey("ClientUpdated"))
                                                    log.AppendLine("Received notification has no Data/SimpleItem with Name = 'ClientUpdated'");

                                                invalidFlag = true;
                                            }
                                            else
                                            {
                                                var apbViolationStr = data["ApbViolation"];
                                                var clientUpdatedStr = data["ClientUpdated"];

                                                var receivedState = false;
                                                if (!apbViolationStr.tryConvertXmlBoolean(out receivedState, (value) => log.AppendLine(string.Format("Received notification has Data/SimpleItem with Name = 'ApbViolation' and Value = '{0}' but this value is not of type xs:boolean", value))))
                                                    invalidFlag = true;

                                                if (receivedState != expectedApbViolation)
                                                {
                                                    invalidFlag = true;
                                                    log.AppendLine(string.Format("Received notification has Source/SimpleItem with Name = 'ApbViolation' and Value = '{0}' but notification with Value = '{1}' is expected", apbViolationStr, XmlConvert.ToString(expectedApbViolation)));
                                                }

                                                if (!clientUpdatedStr.tryConvertXmlBoolean((value) => log.AppendLine(string.Format("Received notification has Data/SimpleItem with Name = 'ClientUpdated' and Value = '{0}' but this value is not of type xs:boolean", value))))
                                                    invalidFlag = true;
                                            }

                                            return invalidFlag;
                                        });

                // Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = msg => filterBase(msg, false) };


                // 17.	Until timeout1 timeout expires repeat the following steps
                var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that ResetAntipassbackViolation notification is received");

                logger.Clear();
                Assert(ValidateNotifications(notifications, topicInfo, credentialToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");


                // 18. ONVIF Client retrieves a credential state 
                CredentialState credRetrState = this.GetCredentialState(credentialToken);
                // 18. If credentialState does not contain AntipassbackState elemet, FAIL the test and go to step 20.
                Assert(credRetrState.AntipassbackState != null, "AntipassbackState flag shall not be skipped.", "Check that AntipassbackState specified");
                // 19. If credentialState.AntipassbackState.AntipassbackViolated equal to true, FAIL the test and go to step 20.
                Assert(credRetrState.AntipassbackState.AntipassbackViolated != true, "ResetAntipassbackViolation flag should be 'false'", "Checking ResetAntipassbackViolation is not 'true'");
            },
            () =>
            {
                // 21. ONVIF Client deletes the Credential (in credentialToken) by following the procedure mentioned in Annex A.6 to restore DUT configuration.
                if (credentialToken != null)
                    this.DeleteCredential(credentialToken);

                this.RestoreCredentialsA10(arrCredToRestore);

                // 23. ONVIF Client sends an Unsubscribe to the subscription endpoint s.
                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);
            });
        }
        #endregion


        #region 7-1-2 RESET ANTIPASSBACK VIOLATIONS WITH INVALID TOKEN
        [Test(Name = "RESET ANTIPASSBACK VIOLATIONS WITH INVALID TOKEN",
           Id = "7-1-2",
           Category = Category.CREDENTIAL,
           Path = PATH_CREDENTIAL_RESET_ANTIPASSBACK_VIOLATIONS,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.Credential, Feature.ResetAntipassbackViolation },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.ResetAntipassbackViolation })]
        public void ResetAntipassbackViolationsWithInvalidTokenTest()
        {
            RunTest(() =>
            {

                var fullList = this.GetFullCredentialInfoListA1();

                string credentialToken = fullList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.ResetAntipassbackViolation(credentialToken);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                        return;
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            });
        }
        #endregion

        #endregion

        #region 8-1-*

        #region 8-1-1
        //[Test(Name = "STATE CHANGE EVENT",
        //    Id = "8-1-1",
        //    Category = Category.CREDENTIAL,
        //    Path = PATH_CREDENTIAL_EVENTS,
        //    Version = 1.0,
        //    RequirementLevel = RequirementLevel.Optional,
        //    //RequiredFeatures = new Feature[] { Feature.ReceiverService },
        //    LastChangedIn = "v15.06",
        //    FunctionalityUnderTest = new Functionality[] { Functionality.GetEventProperties })]
        //public void StateChangedEventTest()
        //{
        //    RunTest(() =>
        //            {
        //                var topicSet = this.GetTopicSet();

        //                var topics = new List<XmlElement>();
        //                foreach (XmlElement element in topicSet.Any)
        //                { FindTopics(element, topics); }

        //                const string eventTopic = "tns1:Configuration/CredentialState/Changed";

        //                var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
        //                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

        //                var eventNode = GetTopicElement(topics, topicInfo);
        //                Assert(null != eventNode,
        //                       string.Format("Event with topic {0} is not supported", eventTopic),
        //                       string.Format("Check that event with topic {0} is present", eventTopic));


        //                var eventDescription = new CredentialEventDescription() { isProperty = false };
        //                eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "CredentialToken", "ReferenceToken", EventServiceExtensions.PTNAMESPACE));

        //                var logger = new StringBuilder();
        //                Assert(checkEventDescription(eventNode, eventDescription, eventNode, logger),
        //                       logger.ToString(),
        //                       string.Format("Checking description of event with topic {0}", eventTopic));
        //            });
        //}
        #endregion

        #region 8-1-2
        [Test(Name = "CONFIGURATION CREDENTIAL CHANGED EVENT",
            Id = "8-1-2",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_EVENTS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventProperties, Functionality.ConfigurationCredentialChangedEvent })]
        public void ConfigurationCredentialChangedEventTest()
        {
            RunTest(() =>
                    {
                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Configuration/Credential/Changed";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Check that event with topic {0} is present", eventTopic));


                        var eventDescription = new CredentialEventDescription() { isProperty = false };
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "CredentialToken", "ReferenceToken", EventServiceExtensions.PTNAMESPACE));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, eventNode, logger),
                               logger.ToStringTrimNewLine(),
                               string.Format("Checking description of event with topic {0}", eventTopic));
                    });
        }
        #endregion

        #region 8-1-3
        [Test(Name = "CONFIGURATION CREDENTIAL REMOVED EVENT",
            Id = "8-1-3",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_EVENTS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventProperties, Functionality.ConfigurationCredentialRemovedEvent })]
        public void ConfigurationCredentialRemovedEventTest()
        {
            RunTest(() =>
                    {
                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Configuration/Credential/Removed";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Check that event with topic {0} is present", eventTopic));


                        var eventDescription = new CredentialEventDescription() { isProperty = false };
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "CredentialToken", "ReferenceToken", EventServiceExtensions.PTNAMESPACE));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, eventNode, logger),
                               logger.ToStringTrimNewLine(),
                               string.Format("Checking description of event with topic {0}", eventTopic));
                    });
        }
        #endregion

        #region 8-1-4
        [Test(Name = "CREDENTIAL STATE ENABLED EVENT",
            Id = "8-1-4",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_EVENTS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventProperties, Functionality.CredentialStateEnabledEvent })]
        public void CredentialStateEnabledEventTest()
        {
            RunTest(() =>
                    {
                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Credential/State/Enabled";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Check that event with topic {0} is present", eventTopic));


                        var eventDescription = new CredentialEventDescription() { isProperty = false };
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "CredentialToken", "ReferenceToken", EventServiceExtensions.PTNAMESPACE));
                        eventDescription.addItemDescription(new EventItemDescription("Data/SimpleItemDescription", "State", "boolean", "http://www.w3.org/2001/XMLSchema"));
                        eventDescription.addItemDescription(new EventItemDescription("Data/SimpleItemDescription", "Reason", "string", "http://www.w3.org/2001/XMLSchema"));
                        eventDescription.addItemDescription(new EventItemDescription("Data/SimpleItemDescription", "ClientUpdated", "boolean", "http://www.w3.org/2001/XMLSchema"));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, eventNode, logger),
                               logger.ToStringTrimNewLine(),
                               string.Format("Checking description of event with topic {0}", eventTopic));
                    });
        }
        #endregion

        #region 8-1-5
        [Test(Name = "CREDENTIAL STATE ANTIPASSBACK VIOLATION EVENT",
            Id = "8-1-5",
            Category = Category.CREDENTIAL,
            Path = PATH_CREDENTIAL_EVENTS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventProperties, Functionality.CredentialStateApbViolationEvent })]
        public void CredentialStateAntipassbackViolationEventTest()
        {
            SubscriptionHandler subscription = null;
            RunTest(() =>
                    {
                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var topicSet = this.GetTopicSet();

                        var topics = new List<XmlElement>();
                        foreach (XmlElement element in topicSet.Any)
                        { FindTopics(element, topics); }

                        const string eventTopic = "tns1:Credential/State/ApbViolation";

                        var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        var eventNode = GetTopicElement(topics, topicInfo);
                        Assert(null != eventNode,
                               string.Format("Event with topic {0} is not supported", eventTopic),
                               string.Format("Check that event with topic {0} is present", eventTopic));


                        var eventDescription = new CredentialEventDescription() { isProperty = false };
                        eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "CredentialToken", "ReferenceToken", EventServiceExtensions.PTNAMESPACE));
                        eventDescription.addItemDescription(new EventItemDescription("Data/SimpleItemDescription", "ApbViolation", "boolean", "http://www.w3.org/2001/XMLSchema"));
                        eventDescription.addItemDescription(new EventItemDescription("Data/SimpleItemDescription", "ClientUpdated", "boolean", "http://www.w3.org/2001/XMLSchema"));

                        var logger = new StringBuilder();
                        Assert(checkEventDescription(eventNode, eventDescription, eventNode, logger),
                               logger.ToStringTrimNewLine(),
                               string.Format("Checking description of event with topic {0}", eventTopic));
                    });
        }
        #endregion

        #endregion

        #region 9-1-1
        [Test(Name = "GET CREDENTIAL AND GET ACCESS PROFILE INFO LIST CONSISTENCY",
            Id = "9-1-1",
            Category = Category.CREDENTIAL,
            Path = PATH_CONSISTENCY,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Credential, Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCredentials, Functionality.GetAccessProfileInfo })]
        public void GetCredentialAndGetAccessProfileInfoListConsistencyTest()
        {
            RunTest(() =>
            {
                var fullCredentialList = this.GetFullCredentialListA3();
                var fullAccessProfileInfoList = this.GetFullAccessProfilesInfoListA1();

                var logger = new StringBuilder();

                var flag = true;
                foreach (var credential in fullCredentialList)
                {
                    var internalFlag = true;
                    var internalLogger = new StringBuilder();
                    internalLogger.AppendLine(string.Format("Credential item with token = '{0}' received via GetCredentialList is inconsistent with full AccessPointInfo list received via GetAccessProfileInfoList:",
                                                            credential.token));
                    if (null != credential.CredentialAccessProfile && credential.CredentialAccessProfile.Any())
                        foreach (var credentialAccessProfile in credential.CredentialAccessProfile)
                        {
                            if (!fullAccessProfileInfoList.Any(api => api.token == credentialAccessProfile.AccessProfileToken))
                            {
                                internalFlag = false;
                                internalLogger.AppendLine(string.Format("{0}CredentialAccessProfile item with AccessProfileToken = '{1}' doesn't have corresponding AccessPointInfo item", singleTab, credentialAccessProfile.AccessProfileToken));
                            }
                        }

                    if (!internalFlag)
                    {
                        logger.AppendLine(internalLogger.ToStringTrimNewLine());
                        flag = false;
                    }
                }


                Assert(flag,
                       logger.ToStringTrimNewLine(),
                       "Checking consistency of received complete Access Profile Info list and complete Credential list");
            });
        }
        #endregion
    }
}
