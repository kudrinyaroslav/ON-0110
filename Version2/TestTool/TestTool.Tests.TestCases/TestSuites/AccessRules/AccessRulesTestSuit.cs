using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Event;
using TestTool.Proxies.Onvif;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.Utils.Events;
using System.ServiceModel.Channels;
using TestTool.Tests.Definitions.Interfaces;
using System.ServiceModel;
using TestTool.Proxies.Event;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.TestCases.OnvifServices;
using TestTool.Tests.TestCases.TestSuites.Events;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;
using TestTool.Tests.Common.TestBase;

namespace TestTool.Tests.TestCases.TestSuites.AccessRules
{
#if __PROFILE_A__
    [TestClass]
#else
#endif
    partial class AccessRulesTestSuit : BaseOnvifTest, IDeviceService, IAccessRulesService, IEventService, IAccessControlService, IScheduleService
    {
        private const string PATH_GENERAL = "Access Rules";
        private const string PATH_CAPABILITIES = PATH_GENERAL + @"\Capabilities";
        private const string PATH_EVENTS = PATH_GENERAL + @"\Events";
        private const string PATH_ACCESS_PROFILE_INFO = PATH_GENERAL + @"\Access Profile Info";
        private const string PATH_ACCESS_PROFILE = PATH_GENERAL + @"\Access Profile";
        private const string PATH_CONSISTENCY = PATH_GENERAL + @"\Consistency";

        public AccessRulesTestSuit(TestLaunchParam param)
            : base(param)
        { }

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
        public BaseOnvifTest Test { get { return this; } }

        #region 1-1-*
        [Test(Name = "ACCESS RULES SERVICE CAPABILITIES",
            Id = "1-1-1",
            Category = Category.ACCESS_RULES,
            Path = PATH_CAPABILITIES,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessRulesServiceCapabilities })]
        public void AccessRulesServiceCapabilitiesTest()
        {
            RunTest(() =>
                    {
                        (this as IAccessRulesService).GetServiceCapabilities();
                    });
        }

        [Test(Name = "GET SERVICES AND GET ACCESS RULES SERVICE CAPABILITIES CONSISTENCY",
            Id = "1-1-2",
            Category = Category.ACCESS_RULES,
            Path = PATH_CAPABILITIES,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessRulesServiceCapabilities, Functionality.GetServices })]
        public void GetServicesAndGetAccessRulesServiceCapabilitiesConsistencyTest()
        {
            RunTest(() =>
                    {
                        var services = this.GetServices(true);
                        var accessRulesService = services.FindService(OnvifService.ACCESS_RULES_SERVICE);

                        Assert(null != accessRulesService,
                               "The DUT didn't return Access Rules service",
                               "Check Access Rules service is supported");

                        var capabilities = this.ExtractAccessRulesCapabilities(accessRulesService);

                        var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();

                        var logger = new StringBuilder();
                        Assert(equalAccessRulesCapabilities(serviceCapabilities, capabilities, logger),
                               logger.ToStringTrimNewLine(),
                               "Check AccessRulesServiceCapabilities consistency");
                    });
        }
        #endregion//1-1-*

        #region 2-1-*
        [Test(Name = "GET ACCESS PROFILE INFO",
            Id = "2-1-1",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessProfileInfo })]
        public void GetAccessProfileInfoTest()
        {
            RunTest(() =>
                    {
                        var accessProfileInfoList = this.GetFullAccessProfilesInfoListA1();

                        if (!accessProfileInfoList.Any())
                            return;

                        var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();

                        var tokenList = accessProfileInfoList.Take((int)serviceCapabilities.MaxLimit).Select(e => e.token);
                        var accessProfileInfoList1 = this.GetAccessProfileInfo(tokenList.ToArray());

                        var logger = new StringBuilder();
                        Assert(validateListFromGetAccessProfileInfo(accessProfileInfoList1, tokenList, logger),
                               logger.ToStringTrimNewLine(),
                               "Checking consistency of received Access ProfileInfo item's lists");

                        foreach (var accessProfileInfo in accessProfileInfoList)
                        {
                            var apil = this.GetAccessProfileInfo(new[] { accessProfileInfo.token });

                            var msg = string.Empty;
                            var n = apil.Count();
                            if (1 != n)
                            {
                                msg = string.Format("The DUT returned {0} Access ProfileInfo item{1} though the single item for token '{2}' is expected",
                                                    0 == n ? "no" : "several",
                                                    0 == n ? "" : "s",
                                                    accessProfileInfo.token);
                            }
                            else
                                msg = string.Format("The DUT returned no Access ProfileInfo item for token '{0}'", accessProfileInfo.token);

                            Assert(1 == apil.Count() && apil.First().token == accessProfileInfo.token,
                                   msg,
                                   "Checking that requested Access ProfileInfo item is received");

                            ValidateConsistency(accessProfileInfo, apil.First(), "GetAccessProfileInfoList", "GetAccessProfileInfo");
                        }
                    });
        }

        [Test(Name = "GET ACCESS PROFILE INFO LIST - LIMIT",
            Id = "2-1-2",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessProfileInfoList })]
        public void GetAccessProfileInfoLimitTest()
        {
            RunTest(() =>
                    {
                        var stepTitle = "Checking received list of Access ProfileInfo items";
                        var msgHeader = "Received list of Access ProfileInfo items contains {0} items but expected no more than {1}";

                        var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();

                        AccessProfileInfo[] accessProfileInfoList1;
                        this.GetAccessProfileInfoList(1, null, out accessProfileInfoList1);

                        Assert(accessProfileInfoList1.Count() <= 1,
                               string.Format(msgHeader, accessProfileInfoList1.Count(), 1),
                               stepTitle);

                        if (1 == serviceCapabilities.MaxLimit)
                            return;


                        AccessProfileInfo[] accessProfileInfoList2;
                        this.GetAccessProfileInfoList((int)serviceCapabilities.MaxLimit, null, out accessProfileInfoList2);

                        Assert(accessProfileInfoList2.Count() <= serviceCapabilities.MaxLimit,
                               string.Format(msgHeader, accessProfileInfoList2.Count(), serviceCapabilities.MaxLimit),
                               stepTitle);

                        if (2 == serviceCapabilities.MaxLimit)
                            return;


                        AccessProfileInfo[] accessProfileInfoList3;
                        this.GetAccessProfileInfoList((int)serviceCapabilities.MaxLimit - 1, null, out accessProfileInfoList3);

                        Assert(accessProfileInfoList3.Count() <= serviceCapabilities.MaxLimit - 1,
                               string.Format(msgHeader, accessProfileInfoList3.Count(), serviceCapabilities.MaxLimit - 1),
                               stepTitle);
                    });
        }

        [Test(Name = "GET ACCESS PROFILE INFO LIST - START REFERENCE AND LIMIT",
            Id = "2-1-3",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessProfileInfoList })]
        public void GetAccessProfileInfoStartReferenceAndLimitTest()
        {
            RunTest(() =>
                    {
                        var stepTitle = "Checking two complete lists of Access ProfileInfo items";
                        var msgHeader = "Received complete list of Access ProfileInfo items with Limit = '{0}' is not the same as the received one with Limit = '{1}'";

                        var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();

                        var listFirst = receiveAndValidateAccessProfileInfoList((int)serviceCapabilities.MaxLimit);

                        if (1 == serviceCapabilities.MaxLimit)
                            return;


                        var listSecond = receiveAndValidateAccessProfileInfoList(1);

                        Assert(equalAccessProfileInfoLists(listFirst, listSecond),
                               string.Format(msgHeader, serviceCapabilities.MaxLimit, 1),
                               stepTitle);

                        if (2 == serviceCapabilities.MaxLimit)
                            return;


                        var listThird = receiveAndValidateAccessProfileInfoList((int)serviceCapabilities.MaxLimit - 1);

                        Assert(equalAccessProfileInfoLists(listFirst, listThird),
                               string.Format(msgHeader, serviceCapabilities.MaxLimit, serviceCapabilities.MaxLimit - 1),
                               stepTitle);
                    });
        }

        [Test(Name = "GET ACCESS PROFILE INFO LIST - NO LIMIT",
            Id = "2-1-4",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessProfileInfoList })]
        public void GetAccessProfileInfoNoLimitTest()
        {
            RunTest(() =>
                    {
                        var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();

                        var list = receiveAndValidateAccessProfileInfoList((int)serviceCapabilities.MaxLimit, true);

                        Assert(list.Count() <= serviceCapabilities.MaxAccessProfiles,
                               string.Format("The received full list of Access ProfileInfo items contains {0} items though the expected number is not more than {1}", list.Count(), serviceCapabilities.MaxAccessProfiles),
                               "Checking complete list of Access ProfileInfo items");
                    });
        }

        [Test(Name = "GET ACCESS PROFILE INFO WITH INVALID TOKEN",
            Id = "2-1-5",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessProfileInfo })]
        public void GetAccessProfileInfoWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullAccessProfilesInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                AccessProfileInfo[] infos = null;
                infos = this.GetAccessProfileInfo(new string[] { invalidToken });

                Assert(infos == null || infos.Length == 0,
                    "List of AccessProfileInfo is not empty",
                    "Check that the DUT returned no AccessProfileInfos");

                if (fullList == null || fullList.Count == 0)
                {
                    return;
                }
                var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;
                if (maxLimit >= 2)
                {
                    infos = this.GetAccessProfileInfo(new string[] { fullList[0].token, invalidToken });

                    var expected = fullList[0];
                    this.CheckRequestedInfo(infos, expected.token, "AccessProfileInfo", D => D.token);
                }
                else
                {
                    LogTestEvent(string.Format("MaxLimit={0}, skip part with sending request with one correct and one incorrect tokens.", maxLimit) + Environment.NewLine);
                }
            });
        }
        [Test(Name = "GET ACCESS PROFILE INFO - TOO MANY ITEMS",
            Id = "2-1-6",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessProfileInfo })]
        public void GetAccessProfileInfoTooManyItemsTest()
        {
            RunTest(() =>
                    {
                        var fullList = this.GetFullAccessProfilesInfoListA1();

                        var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();
                        var maxLimit = serviceCapabilities.MaxLimit;

                        if (fullList == null || fullList.Count <= maxLimit)
                            return;

                        string faultCode = "Sender/InvalidArgs/TooManyItems";
                        string faultExpectedSequence = ConvertToFaultCode(faultCode);
                        try
                        {
                            fullList.RemoveRange((int)maxLimit, fullList.Count - (int)maxLimit - 1);
                            this.GetAccessProfileInfo(fullList.Select(e => e.token).ToArray());
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
        [Test(Name = "GET ACCESS PROFILES",
            Id = "3-1-1",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessProfiles })]
        public void GetAccessProfilesTest()
        {
            RunTest(() =>
                    {
                        var accessProfileList = this.GetFullAccessProfilesListA3();

                        if (!accessProfileList.Any())
                            return;

                        var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();

                        var tokenList = accessProfileList.Take((int)serviceCapabilities.MaxLimit).Select(e => e.token);
                        var accessProfileList1 = this.GetAccessProfiles(tokenList.ToArray());

                        var logger = new StringBuilder();
                        Assert(validateListFromGetAccessProfiles(accessProfileList1, tokenList, logger),
                               logger.ToStringTrimNewLine(),
                               "Checking consistency of received Access Profile item's lists");

                        foreach (var accessProfile in accessProfileList)
                        {
                            var apl = this.GetAccessProfiles(new[] { accessProfile.token });

                            var msg = string.Empty;
                            var n = apl.Count();
                            if (1 != n)
                            {
                                msg = string.Format("The DUT returned {0} Access Profile item{1} though the single item for token '{2}' is expected",
                                                    0 == n ? "no" : "several",
                                                    0 == n ? "" : "s",
                                                    accessProfile.token);
                            }
                            else
                                msg = string.Format("The DUT returned no Access ProfileInfo item for token '{0}'", accessProfile.token);

                            Assert(1 == apl.Count() && apl.First().token == accessProfile.token,
                                   msg,
                                   "Checking that requested Access ProfileInfo item is received");


                            ValidateConsistency(accessProfile, apl.First(), "GetAccessProfileList", "GetAccessProfiles");
                        }
                    });
        }

        [Test(Name = "GET ACCESS PROFILE LIST - LIMIT",
            Id = "3-1-2",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessProfileList })]
        public void GetAccessProfileListLimitTest()
        {
            RunTest(() =>
                    {
                        var stepTitle = "Checking received list of Access Profile items";
                        var msgHeader = "Received list of Access Profile items contains {0} items but expected no more than {1}";

                        var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();

                        AccessProfile[] accessProfileList1;
                        this.GetAccessProfileList(1, null, out accessProfileList1);

                        Assert(accessProfileList1.Count() <= 1,
                               string.Format(msgHeader, accessProfileList1.Count(), 1),
                               stepTitle);

                        if (1 == serviceCapabilities.MaxLimit)
                            return;


                        AccessProfile[] accessProfileList2;
                        this.GetAccessProfileList((int)serviceCapabilities.MaxLimit, null, out accessProfileList2);

                        Assert(accessProfileList2.Count() <= serviceCapabilities.MaxLimit,
                               string.Format(msgHeader, accessProfileList2.Count(), serviceCapabilities.MaxLimit),
                               stepTitle);

                        if (2 == serviceCapabilities.MaxLimit)
                            return;


                        AccessProfile[] accessProfileList3;
                        this.GetAccessProfileList((int)serviceCapabilities.MaxLimit - 1, null, out accessProfileList3);

                        Assert(accessProfileList3.Count() <= serviceCapabilities.MaxLimit - 1,
                               string.Format(msgHeader, accessProfileList3.Count(), serviceCapabilities.MaxLimit - 1),
                               stepTitle);
                    });
        }

        [Test(Name = "GET ACCESS PROFILE LIST - START REFERENCE AND LIMIT",
            Id = "3-1-3",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessProfileList })]
        public void GetAccessProfileStartReferenceAndLimitTest()
        {
            RunTest(() =>
                    {
                        var stepTitle = "Checking two complete lists of Access Profile items";
                        var msgHeader = "Received complete list of Access Profile items with Limit = '{0}' is not the same as the received one with Limit = '{1}'";
                        StringBuilder logger = new StringBuilder();

                        var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();

                        var listFirst = receiveAndValidateAccessProfileList((int)serviceCapabilities.MaxLimit);

                        if (1 == serviceCapabilities.MaxLimit)
                        {
                            logger = new StringBuilder();
                            Assert(validateListFromGetAccessProfileAndGetAccessProfilesConsistency(this.GetFullAccessProfilesInfoListA1(), listFirst, logger),
                                   logger.ToStringTrimNewLine(),
                                   "Checking consistency of received Access ProfileInfo and Access Profile lists");
                            return;
                        }

                        var listSecond = receiveAndValidateAccessProfileList(1);

                        Assert(equalAccessProfileInfoLists(listFirst, listSecond),
                               string.Format(msgHeader, serviceCapabilities.MaxLimit, 1),
                               stepTitle);

                        if (2 == serviceCapabilities.MaxLimit)
                        {
                            logger = new StringBuilder();
                            Assert(validateListFromGetAccessProfileAndGetAccessProfilesConsistency(this.GetFullAccessProfilesInfoListA1(), listSecond, logger),
                                   logger.ToStringTrimNewLine(),
                                   "Checking consistency of received Access ProfileInfo and Access Profile lists");
                            return;
                        }

                        var listThird = receiveAndValidateAccessProfileList((int)serviceCapabilities.MaxLimit - 1);

                        Assert(equalAccessProfileInfoLists(listFirst, listThird),
                               string.Format(msgHeader, serviceCapabilities.MaxLimit, serviceCapabilities.MaxLimit - 1),
                               stepTitle);

                        logger = new StringBuilder();
                        Assert(validateListFromGetAccessProfileAndGetAccessProfilesConsistency(this.GetFullAccessProfilesInfoListA1(), listThird, logger),
                               logger.ToStringTrimNewLine(),
                               "Checking consistency of received Access ProfileInfo and Access Profile lists");
                    });
        }

        [Test(Name = "GET ACCESS PROFILE LIST - NO LIMIT",
            Id = "3-1-4",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessProfileList })]
        public void GetAccessProfileNoLimitTest()
        {
            RunTest(() =>
                    {
                        var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();

                        var list = receiveAndValidateAccessProfileList((int)serviceCapabilities.MaxLimit, true);

                        var fullAccessProfileInfoList = this.GetFullAccessProfilesInfoListA1();

                        var logger = new StringBuilder();
                        Assert(validateListFromGetAccessProfileAndGetAccessProfilesConsistency(fullAccessProfileInfoList, list, logger),
                               logger.ToStringTrimNewLine(),
                               "Checking consistency of received Access ProfileInfo and Access Profile lists");
                    });
        }

        [Test(Name = "CREATE ACCESS PROFILE",
            Id = "3-1-5",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService, Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateAccessProfile, Functionality.ConfigurationAccessProfileChangedEvent })]
        public void CreateAccessProfileTest()
        {
            AccessProfile restoreProfile = null;
            SubscriptionHandler subscription = null;
            var accessProfileToken = string.Empty;
            RunTest(() =>
                    {
                        var fullAccessProfileList = this.GetFullAccessProfilesListA3();

                        restoreProfile = this.CheckFreeStorageForAdditionalAccessProfileA7(fullAccessProfileList);
                        if (null != restoreProfile) fullAccessProfileList = fullAccessProfileList.Where(e => e.token != restoreProfile.token).ToList();

                        var fullSchedulesList = this.GetFullScheduleListA3();

                        var services = this.GetServices(false);
                        var fullAccessPointList = this.IsAccessControlSupported(services) ? this.GetFullAccessPointInfoListA1() : new List<AccessPointInfo>();

                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/AccessProfile/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        const int messageLimit = 1;
                        var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter, -1);

                        AccessPolicy accessPolicy = null;
                        if (fullAccessPointList.Any() && fullSchedulesList.Any())
                            accessPolicy = new AccessPolicy()
                                           {
                                               ScheduleToken = fullSchedulesList.First().token,
                                               Entity = fullAccessPointList.First().token,
                                               EntityType = null,
                                               Extension = null
                                           };

                        var newAccessProfile = new AccessProfile()
                                               {
                                                   token = "",
                                                   Name = "Test Access Profile",
                                                   Description = "Test Description",
                                                   AccessPolicy = (null != accessPolicy ? new[] { accessPolicy } : null),
                                                   Extension = null
                                               };

                        accessProfileToken = this.CreateAccessProfile(newAccessProfile);
                        Assert(!fullAccessProfileList.Any(e => e.token == accessProfileToken),
                               string.Format("CreateAccessProfile response contains invalid Access Profile token: '{0}'. The Access Profile item with the same token already exists.", accessProfileToken),
                               "Checking CreateAccessProfile response");
                        newAccessProfile.token = accessProfileToken;

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                        pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "AccessProfile.AccessProfileTokenValidation.xq", new List<AccessProfile>() { newAccessProfile });

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Waiting for notification");

                        var logger = new StringBuilder();
                        Assert(ValidateNotifications(notifications, topicInfo, accessProfileToken, logger),
                               logger.ToStringTrimNewLine(),
                               "Validation of received notification");

                        var receivedAP = this.GetAccessProfiles(new[] { accessProfileToken }).FirstOrDefault(e => e.token == accessProfileToken);
                        ValidateConsistency(newAccessProfile, receivedAP, "CreateAccessProfile", "GetAccessProfiles");

                        var receivedAPI = this.GetAccessProfileInfo(new[] { accessProfileToken }).FirstOrDefault(e => e.token == accessProfileToken);
                        ValidateConsistency(newAccessProfile, receivedAPI, "CreateAccessProfile", "GetAccessProfileInfo");

                        receivedAPI = this.GetFullAccessProfilesInfoListA1().FirstOrDefault(e => e.token == accessProfileToken);
                        ValidateConsistency(newAccessProfile, receivedAPI, "CreateAccessProfile", "GetAccessProfileInfoList");

                        receivedAP = this.GetFullAccessProfilesListA3().FirstOrDefault(e => e.token == accessProfileToken);
                        ValidateConsistency(newAccessProfile, receivedAP, "CreateAccessProfile", "GetAccessProfileList");
                    },
                    () =>
                    {
                        if (!string.IsNullOrEmpty(accessProfileToken))
                            this.DeleteAccessProfile(accessProfileToken);

                        if (null != restoreProfile)
                        {
                            restoreProfile.token = "";
                            this.CreateAccessProfile(restoreProfile);
                        }

                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }

        [Test(Name = "MODIFY ACCESS PROFILE",
            Id = "3-1-7",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService, Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.ModifyAccessProfile, Functionality.ConfigurationAccessProfileChangedEvent })]
        public void ModifyAccessProfileTest()
        {
            AccessProfile restoreProfile = null;
            SubscriptionHandler subscription = null;
            var accessProfileToken = string.Empty;
            RunTest(() =>
                    {
                        // 3. ONVIF Client retrieves a complete list of access profiles (out accessProfileCompleteList1) by following the procedure mentioned in Annex A.3.
                        var fullAccessProfileList = this.GetFullAccessProfilesListA3();

                        // 4. ONVIF Client checks free storage for additional Access Profile (in accessProfileCompleteList1, out accessProfileToRestore) by following the procedure mentioned in Annex A.7
                        restoreProfile = this.CheckFreeStorageForAdditionalAccessProfileA7(fullAccessProfileList);

                        // 5. ONVIF Client retrieves a complete list of schedules (out scheduleCompleteList) by following the procedure mentioned in Annex A.4.
                        var fullSchedulesList = this.GetFullScheduleListA3();

                        // 6. ONVIF Client retrieves a complete list of access point information (out accessPointInfoCompleteList) by following the procedure mentioned in AnnexA.5.
                        var services = this.GetServices(false);
                        var fullAccessPointList = this.IsAccessControlSupported(services) ? this.GetFullAccessPointInfoListA1() : new List<AccessPointInfo>();


                        // 7. ONVIF client invokes CreateAccessProfile with parameters
                        AccessProfile newAccessProfile;
                        accessProfileToken = this.CreateAccessProfileA11(out newAccessProfile, fullSchedulesList, fullAccessPointList, "");   // Oksana Tyushkina

                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/AccessProfile/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        const int messageLimit = 1;
                        var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                        // 9. ONVIF Client invokes CreatePullPointSubscription with parameters
                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter, -1);

                        AccessPolicy accessPolicy = null;
                        if (fullAccessPointList.Any() && fullSchedulesList.Any())
                            accessPolicy = new AccessPolicy()
                                           {
                                               ScheduleToken = fullSchedulesList.First().token,
                                               Entity = fullAccessPointList.First().token,
                                               EntityType = null,
                                               Extension = null
                                           };

                        var modifiedAccessProfile = new AccessProfile()
                                                    {
                                                        token = accessProfileToken,
                                                        Name = "Test Access Profile 2",
                                                        Description = "Test Description 2",
                                                        Extension = null,
                                                        AccessPolicy = (null != accessPolicy ? new[] { accessPolicy } : null)
                                                    };

                        //this.ModifyAccessProfile(accessProfileToken, modifiedAccessProfile);
                        this.ModifyAccessProfile(modifiedAccessProfile);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                        pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "AccessProfile.AccessProfileTokenValidation.xq", new List<AccessProfile>() { newAccessProfile });

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Waiting for notification");

                        var logger = new StringBuilder();
                        Assert(ValidateNotifications(notifications, topicInfo, accessProfileToken, logger),
                               logger.ToStringTrimNewLine(),
                               "Validation of received notification");

                        ValidateConsistency(modifiedAccessProfile, this.GetAccessProfiles(new[] { accessProfileToken }), "ModifyAccessProfile", "GetAccessProfiles");
                    },
                    () =>
                    {
                        if (!string.IsNullOrEmpty(accessProfileToken))
                            this.DeleteAccessProfile(accessProfileToken);

                        if (null != restoreProfile)
                        {
                            restoreProfile.token = "";
                            this.CreateAccessProfile(restoreProfile);
                        }

                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }

        [Test(Name = "DELETE ACCESS PROFILE",
            Id = "3-1-8",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService, Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteAccessProfile, Functionality.ConfigurationAccessProfileRemovedEvent })]
        public void DeleteAccessProfileTest()
        {
            AccessProfile restoreProfile = null;
            SubscriptionHandler subscription = null;
            var accessProfileToken = string.Empty;
            var flagRemoveAccessProfileToken = false;
            RunTest(() =>
                    {
                        var initialFullAccessProfileList = this.GetFullAccessProfilesListA3();
                        restoreProfile = this.CheckFreeStorageForAdditionalAccessProfileA7(initialFullAccessProfileList);
                        if (null != restoreProfile) initialFullAccessProfileList = initialFullAccessProfileList.Where(e => e.token != restoreProfile.token).ToList();

                        var fullSchedulesList = this.GetFullScheduleListA3();

                        var services = this.GetServices(false);
                        var fullAccessPointList = this.IsAccessControlSupported(services) ? this.GetFullAccessPointInfoListA1() : new List<AccessPointInfo>();

                        AccessProfile newAccessProfile;
                        accessProfileToken = this.CreateAccessProfileA11(out newAccessProfile, fullSchedulesList, fullAccessPointList, "");
                        newAccessProfile.token = accessProfileToken;
                        flagRemoveAccessProfileToken = true;


                        subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                        var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/AccessProfile/Removed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                        var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                        const int messageLimit = 1;
                        var filter = EventServiceExtensions.CreateSubscriptionFilter(topicInfo);

                        var timeout = _operationDelay / 1000;
                        subscription.Subscribe(filter, -1);

                        flagRemoveAccessProfileToken = false;
                        this.DeleteAccessProfile(accessProfileToken);

                        var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                        pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "AccessProfile.AccessProfileTokenValidation.xq", new List<AccessProfile>() { newAccessProfile });

                        var notifications = new Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement>();
                        Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                               pollingCondition.Reason,
                               "Waiting for notification");

                        var logger = new StringBuilder();
                        Assert(ValidateNotifications(notifications, topicInfo, accessProfileToken, logger),
                               logger.ToStringTrimNewLine(),
                               "Validation of received notification");

                        var receivedAccessProfiles = this.GetAccessProfiles(new[] { accessProfileToken });
                        Assert(!receivedAccessProfiles.Any(),
                               string.Format("GetAccessProfiles returned {0} Access Profile items, but expected empty list.", receivedAccessProfiles.Count()),
                               "Checking GetAccessProfiles command");

                        var receivedAccessProfileInfos = this.GetAccessProfileInfo(new[] { accessProfileToken });
                        Assert(!receivedAccessProfileInfos.Any(),
                               string.Format("GetAccessProfiles returned {0} Access Profile Info items, but expected empty list.", receivedAccessProfileInfos.Count()),
                               "Checking GetAccessProfileInfo command");

                        Assert(!this.GetFullAccessProfilesInfoListA1().Any(e => e.token == accessProfileToken),
                               string.Format("GetAccessProfileInfoList returned Access Profile Info item with token = '{0}' that had been deleted earlier", accessProfileToken),
                               "Checking GetAccessProfileInfoList command");

                        var receivedAccessProfilesList = this.GetFullAccessProfilesListA3();
                        Assert(equalAccessProfileInfoLists(initialFullAccessProfileList, receivedAccessProfilesList),
                               "The received complete Access Profile List is different from the one at the start of the test",
                               "Checking received complete Access Profile List");
                    },
                    () =>
                    {
                        if (flagRemoveAccessProfileToken)
                            this.DeleteAccessProfile(accessProfileToken);

                        if (null != restoreProfile)
                        {
                            restoreProfile.token = "";
                            this.CreateAccessProfile(restoreProfile);
                        }

                        SubscriptionHandler.Unsubscribe(subscription);
                    });
        }
        #endregion

        [Test(Name = "GET ACCESS PROFILES WITH INVALID TOKEN",
            Id = "3-1-9",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessProfiles })]
        public void GetAccessProfilesWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullAccessProfilesInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                AccessProfile[] accessProfiles = null;
                accessProfiles = this.GetAccessProfiles(new string[] { invalidToken });

                Assert(accessProfiles == null || accessProfiles.Length == 0,
                    "List of AccessProfiles is not empty",
                    "Check that the DUT returned no AccessProfiles");

                if (fullList == null || fullList.Count == 0)
                {
                    return;
                }
                var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;
                if (maxLimit >= 2)
                {
                    accessProfiles = this.GetAccessProfiles(new string[] { fullList[0].token, invalidToken });

                    var expected = fullList[0];
                    this.CheckRequestedInfo(accessProfiles, expected.token, "AccessProfiles", D => D.token);
                }
                else
                {
                    LogTestEvent(string.Format("MaxLimit={0}, skip part with sending request with one correct and one incorrect tokens.", maxLimit) + Environment.NewLine);
                }
            });
        }

        [Test(Name = "GET ACCESS PROFILES - TOO MANY ITEMS",
            Id = "3-1-10",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessProfiles })]
        public void GetAccessProfilesTooManyItemsTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullAccessProfilesListA3();

                var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;

                if (fullList == null || fullList.Count <= maxLimit)
                    return;

                string faultCode = "Sender/InvalidArgs/TooManyItems";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    fullList.RemoveRange((int)maxLimit, fullList.Count - (int)maxLimit - 1);
                    this.GetAccessProfiles(fullList.Select(e => e.token).ToArray());
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

        [Test(Name = "CREATE ACCESS PROFILE - NOT EMPTY ACCESS PROFILE TOKEN",
            Id = "3-1-11",
            Category = Category.ACCESS_RULES,
            Path = PATH_ACCESS_PROFILE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateAccessProfile })]
        public void CreateAccessProfileNotEmptyAccessProfileTokenTest()
        {
            RunTest(() =>
            {

                var newAccessProfile = new AccessProfile()
                {
                    token = "AccessProfileToken",
                    Name = "Test Access Profile",
                    Description = "Test Description",
                    AccessPolicy = null,
                    Extension = null
                };

                string faultCode = "Sender/InvalidArgs";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    var accessProfileToken = this.CreateAccessProfile(newAccessProfile);
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

        #region 3-1-12 CREATE ACCESS PROFILE - MULTIPLE SCHEDULES NOT SUPPORTED
        [Test(Name = "CREATE ACCESS PROFILE - MULTIPLE SCHEDULES NOT SUPPORTED",
           Id = "3-1-12",
           Category = Category.ACCESS_RULES,
           Path = PATH_ACCESS_PROFILE,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.AccessRulesService, Feature.Schedule },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.CreateAccessProfile })]
        public void CreateAccessProfileMultipleSchedulesNotSupportedTest()
        {
            RunTest(() =>
            {

                var serviceCapabilities = (this as IAccessRulesService).GetServiceCapabilities();
                if (serviceCapabilities.MultipleSchedulesPerAccessPointSupported)
                {
                    LogStepEvent("MultipleSchedulesPerAccessPointSupported capability is supported, other steps will be passed");
                    return;
                }

                var fullSchedulesList = this.GetFullScheduleListA3();
                if (fullSchedulesList == null || fullSchedulesList.Count < 2)
                {
                    LogStepEvent("Get Schedule List returns less than 2 items, other steps will be passed");
                    return;
                }
                List<AccessPointInfo> fullAccessPointList = null;
                var services = this.GetServices(false);
                if (this.IsAccessControlSupported(services))
                {
                    fullAccessPointList = this.GetFullAccessPointInfoListA1();
                }
                if (fullAccessPointList == null || !fullAccessPointList.Any())
                {
                    LogStepEvent("Get Access Point List returns empty list, other steps will be passed");
                    return;
                }

                AccessProfile newAccessProfile = new AccessProfile()
                {
                    token = "",
                    Name = "Test Access Profile",
                    Description = "Test Description",
                    AccessPolicy = new AccessPolicy[] {
                        new AccessPolicy() {
                            ScheduleToken = fullSchedulesList[0].token,
                            Entity = fullAccessPointList[0].token,
                        },
                        new AccessPolicy () {
                            ScheduleToken = fullSchedulesList[1].token,
                            Entity = fullAccessPointList[1].token
                        }
                    }
                };

                string faultCode = "Sender/CapabilityViolated/MultipleSchedulesPerAccessPointSupported";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.CreateAccessProfile(newAccessProfile);
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

        #region 3-1-13 MODIFY ACCESS PROFILE WITH INVALID TOKEN
        [Test(Name = "MODIFY ACCESS PROFILE WITH INVALID TOKEN",
           Id = "3-1-13",
           Category = Category.ACCESS_RULES,
           Path = PATH_ACCESS_PROFILE,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.AccessRulesService },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.ModifyAccessProfile })]
        public void ModifyAccessProfileWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullAccessProfilesInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                var invalidAccessProfile = new AccessProfile()
                {
                    token = invalidToken,
                    Name = "Test Access Profile",
                    Description = "Test Description",
                };

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.ModifyAccessProfile(invalidAccessProfile);
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

        #region 3-1-14 DELETE ACCESS PROFILE WITH INVALID TOKEN
        [Test(Name = "DELETE ACCESS PROFILE WITH INVALID TOKEN",
           Id = "3-1-14",
           Category = Category.ACCESS_RULES,
           Path = PATH_ACCESS_PROFILE,
           Version = 1.0,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.AccessRulesService },
           LastChangedIn = "v15.06",
           FunctionalityUnderTest = new Functionality[] { Functionality.DeleteAccessProfile })]
        public void DeleteAccessProfileWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullAccessProfilesInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.DeleteAccessProfile(invalidToken);
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

        #region 4-3-*
        /*
        [Test(Name = "CREATE ACCESS PROFILE",
            Id = "4-3-5",
            Category = Category.ACCESS_RULES,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            //RequiredFeatures = new Feature[] { Feature.KeyManagement },
            FunctionalityUnderTest = new Functionality[] { })]
        public void CreateAccessProfileTest()
        {
            RunTest(() =>
            {
                string sNextRef = null;

                // 3. ONVIF Client retrieves a complete list of access profiles (out accessProfileCompleteList) by following the procedure mentioned in Annex A.3.
                List<AccessProfile> lstAccessProfile1 = (this as IAccessRulesService).GetFullAccessProfilesListA3();

                // 4. ONVIF Client checks free storage for additional Access Profile (in accessProfileCompleteList1, out accessProfileToRestore) by following the procedure mentioned in Annex A.7.
                AccessProfile accProfRestore = this.CheckFreeStorageForAdditionalAccessProfileA7(lstAccessProfile1);

                // 4. 5.	ONVIF Client retrieves a complete list of schedules (out scheduleCompleteList) by following the procedure mentioned in Annex A.4.
                //@@@@ this.GetScheduleList() - not implemented
            });
        }
        */
        #endregion

        // helper function for GetAccessProfilesChangedEventTest()
        internal void FillTopicList(Dictionary<string, XmlElement> dicTopics, string sNamePrefix, XmlElement xp)
        {
            foreach (XmlElement xn in xp)
            {
                string sKey = sNamePrefix + "/" + xn.Name;
                if (!dicTopics.ContainsKey(sKey))
                {
                    dicTopics.Add(sKey, xn);
                    if (xn.HasChildNodes)
                        FillTopicList(dicTopics, sKey, xn);
                }
            }
        }

        #region Access Rules event description

        internal class EventItemDescription
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Namespace { get; set; }
            public bool Mandatory { get; set; }

            public EventItemDescription()
                : this(null, null, null, null)
            { }

            public EventItemDescription(string path, string name, string type, string ns)
                : this(path, name, type, ns, true)
            { }

            public EventItemDescription(string path, string name, string type, string ns, bool mandatory)
            {
                Path = path;
                Name = name;
                Namespace = ns;
                Type = type;
                Mandatory = mandatory;
            }
        }

        internal class AccessRulesEventDescription
        {
            public List<EventItemDescription> itemDescriptions { get; private set; }

            public AccessRulesEventDescription()
            { itemDescriptions = new List<EventItemDescription>(); }

            public void addItemDescription(EventItemDescription description) { itemDescriptions.Add(description); }

            public bool isProperty { get; set; }
        }

        #endregion

        #region Validating utils
        bool checkEventDescription(XmlElement eventNode,
                                   AccessRulesEventDescription eventDescription,
                                   IXmlNamespaceResolver namespaceResolver,
                                   StringBuilder logger)
        {
            return checkEventDescription(eventNode, eventDescription, namespaceResolver.LookupNamespace, logger);
        }

        bool checkEventDescription(XmlElement eventNode,
                                   AccessRulesEventDescription eventDescription,
                                   XmlElement namespaceResolver,
                                   StringBuilder logger)
        {
            return checkEventDescription(eventNode, eventDescription, namespaceResolver.GetNamespaceOfPrefix, logger);
        }

        bool checkEventDescription(XmlElement eventNode, AccessRulesEventDescription eventDescription, Func<string, string> namespaceResolver, StringBuilder logger)
        {
            var descriptionNode = eventNode.GetMessageDescription();
            if (null == descriptionNode)
            {
                logger.AppendLine("MessageDescription element is absent.");
                return false;
            }

            var manager = new XmlNamespaceManager(eventNode.OwnerDocument.NameTable);
            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            var isPropertyAttribute = descriptionNode.Attributes[OnvifMessage.ISPROPERTY];
            if (!eventDescription.isProperty)
            {
                if (null == isPropertyAttribute)
                    return true;
            }
            else
            {
                if (null == isPropertyAttribute)
                {
                    logger.AppendLine("The 'IsProperty' attribute is absent.");
                    return false;
                }
            }
            var isPropertyValueXmlString = XmlConvert.ToString(eventDescription.isProperty);
            if (isPropertyAttribute.Value != isPropertyValueXmlString)
            {
                logger.AppendLine(
                        string.Format("The 'IsProperty' attribute is incorrect. Expected: {0}. Actual: {1}",
                                      isPropertyValueXmlString,
                                      isPropertyAttribute.Value));
                return false;
            }

            bool flag = true;
            foreach (var itemDescription in eventDescription.itemDescriptions)
            {
                var path = itemDescription.Path.Split('/').Select(e => "tt:" + e).Aggregate("", (s, s1) => s + s1 + "/").Trim('/');
                var nodes = descriptionNode.SelectNodes(path, manager).OfType<XmlElement>();
                var itemNode = nodes.FirstOrDefault(e => null != e.Attributes[OnvifMessage.NAME]
                                                         && e.Attributes[OnvifMessage.NAME].Value == itemDescription.Name);
                if (null == itemNode)
                {
                    if (itemDescription.Mandatory)
                    {
                        flag = false;
                        logger.AppendLine(string.Format("Mandatory element {0} of type '{1}' is absent", itemDescription.Name, itemDescription.Type));
                    }
                }
                else
                {
                    XmlAttribute type = itemNode.Attributes[OnvifMessage.TYPE];
                    if (type == null)
                    {
                        flag = false;
                        logger.AppendLine(string.Format("'Type' attribute is missing for '{0}' simple item", itemDescription.Name));
                    }
                    else
                    {
                        string error = string.Empty;
                        if (!type.IsCorrectQName(itemDescription.Type, itemDescription.Namespace, namespaceResolver, ref error))
                        {
                            flag = false;
                            logger.AppendLine(string.Format("'Type' attribute is incorrect for '{0}' simple item: {1}", itemDescription.Name, error));
                        }
                    }
                }
            }

            return flag;
        }
        #endregion

        #region 5-1-*
        [Test(Name = "ACCESS PROFILE CHANGED EVENT",
            Id = "5-1-1",
            Category = Category.ACCESS_RULES,
            Path = PATH_EVENTS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventProperties, Functionality.ConfigurationAccessProfileChangedEvent })]
        public void GetAccessProfilesChangedEventTest()
        {
            RunTest(() =>
            {
                var topicSet = this.GetTopicSet();

                var topics = new List<XmlElement>();
                foreach (XmlElement element in topicSet.Any)
                { FindTopics(element, topics); }

                var eventTopic = "tns1:Configuration/AccessProfile/Changed";

                var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                var eventNode = GetTopicElement(topics, topicInfo);
                Assert(null != eventNode,
                       string.Format("Event with topic {0} is not supported", eventTopic),
                       string.Format("Check that event with topic {0} is present", eventTopic));

                var eventDescription = new AccessRulesTestSuit.AccessRulesEventDescription() { isProperty = false };
                eventDescription.addItemDescription(new AccessRulesTestSuit.EventItemDescription("Source/SimpleItemDescription", "AccessProfileToken", "ReferenceToken", EventServiceExtensions.PTNAMESPACE));

                var logger = new StringBuilder();
                Assert(checkEventDescription(eventNode, eventDescription, eventNode, logger), logger.ToString(), "Checking AccessProfileToken type");
            });
        }

        [Test(Name = "ACCESS PROFILE REMOVED EVENT",
            Id = "5-1-2",
            Category = Category.ACCESS_RULES,
            Path = PATH_EVENTS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventProperties, Functionality.ConfigurationAccessProfileRemovedEvent })]
        public void GetAccessProfilesRemovedEventTest()
        {
            RunTest(() =>
            {
                var topicSet = this.GetTopicSet();

                var topics = new List<XmlElement>();
                foreach (XmlElement element in topicSet.Any)
                { FindTopics(element, topics); }

                var eventTopic = "tns1:Configuration/AccessProfile/Removed";

                var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                var eventNode = GetTopicElement(topics, topicInfo);
                Assert(null != eventNode,
                       string.Format("Event with topic {0} is not supported", eventTopic),
                       string.Format("Check that event with topic {0} is present", eventTopic));

                var eventDescription = new AccessRulesTestSuit.AccessRulesEventDescription() { isProperty = false };
                eventDescription.addItemDescription(new AccessRulesTestSuit.EventItemDescription("Source/SimpleItemDescription", "AccessProfileToken", "ReferenceToken", EventServiceExtensions.PTNAMESPACE));

                var logger = new StringBuilder();
                Assert(checkEventDescription(eventNode, eventDescription, eventNode, logger), logger.ToString(), "Checking AccessProfileToken type");
            });
        }

        #endregion

        #region 6-1-*
        [Test(Name = "ACCESS POLICIES AND ACCESS POINT CONSISTENCY",
            Id = "6-1-1",
            Category = Category.ACCESS_RULES,
            Path = PATH_CONSISTENCY,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessRulesService, Feature.AccessControlService },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessPointInfoList, Functionality.GetAccessProfileList })]
        public void AccessPoliciesAndAccessPointConsistencyTest()
        {
            RunTest(() =>
            {
                // 3. ONVIF Client retrieves a complete list of access profiles (out accessProfileCompleteList) by following the procedure mentioned in Annex A.3.
                var fullAccessProfile = this.GetFullAccessProfilesListA3();

                // 4.	ONVIF Client retrieves a complete list of access point information (out accessPointInfoCompleteList) by following the procedure mentioned in AnnexA.5.
                var services = this.GetServices(false);
                Assert(this.IsAccessControlSupported(services),
                    "AccessControlService does not support",
                    "Checking that AccessControl Service is supported");

                var fullAccessPointInfo = this.GetFullAccessPointInfoListA1();

                // 5. For each access policy accessProfileCompleteList.accessPolicy repeat the following steps:
                //    5.1. If accessProfileCompleteList.AccessPolicy[0].Entity item have different value from any of accessPointInfoCompleteList.token[0] items, FAIL the test and skip other steps.
                bool flag = true;
                var logger = new StringBuilder();
                foreach (var accessProfile in fullAccessProfile)
                {
                    if (null != accessProfile.AccessPolicy && accessProfile.AccessPolicy.Any())
                    {
                        var policies = accessProfile.AccessPolicy.Where(p => null == p.EntityType || "AccessPoint" == p.EntityType.Name && OnvifService.ACCESSCONTROL == p.EntityType.Namespace);

                        foreach (var policy in policies)
                        {
                            if (!fullAccessPointInfo.Any(e => e.token == policy.Entity))
                            {
                                flag = false;
                                logger.AppendLine(string.Format("The AccessProfile item with token = '{0}' received via GetAccessProfileList has no corresponding AccessPointInfo item received via GetAccessPointInfoList", accessProfile.token));
                            }
                        }
                    }
                }

                Assert(flag,
                       logger.ToStringTrimNewLine(),
                       "Checking consistency of received Access Profile and Access ProfileInfo lists");

            });
        }

        #endregion
    }
}
