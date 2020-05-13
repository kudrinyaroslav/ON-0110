using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Event;
using TestTool.Proxies.Onvif;
using Onvif = TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.OnvifServices;
using TestTool.Tests.TestCases.TestSuites.Events;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;
using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using System.ServiceModel;
using TestTool.Tests.Common.TestBase;
using System.Globalization;

namespace TestTool.Tests.TestCases.TestSuites.Schedule
{
    [TestClass]
    partial class ScheduleTestSuit : BaseOnvifTest, IDeviceService, IScheduleService, IEventService
    {
        #region internal
        private const string PATH_GENERAL = "Schedule";
        private const string PATH_CAPABILITIES = PATH_GENERAL + @"\Capabilities";
        private const string PATH_SCHEDULE_INFO = PATH_GENERAL + @"\Schedule Info";
        private const string PATH_SCHEDULE = PATH_GENERAL + @"\Schedule";
        private const string PATH_SPECIAL_DAY_GROUP_INFO = PATH_GENERAL + @"\Special day group info";
        private const string PATH_SPECIAL_DAY_GROUP = PATH_GENERAL + @"\Special day group";
        private const string PATH_SCHEDULE_EVENTS = PATH_GENERAL + @"\Schedule events";
        private const string PATH_SCHEDULE_STATE = PATH_GENERAL + @"\Schedule state";
        private const string PATH_SCHEDULE_CONSISTENCY = PATH_GENERAL + @"\Consistency";


        public ScheduleTestSuit(TestLaunchParam param)
            : base(param)
        { }

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

        #region 1-1-1
        [Test(Name = "SCHEDULE SERVICE CAPABILITIES",
            Id = "1-1-1",
            Category = Category.SCHEDULE,
            Path = PATH_CAPABILITIES,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleServiceCapabilities })]
        public void ScheduleServiceCapabilitiesTest()
        {
            RunTest(() => (this as IScheduleService).GetServiceCapabilities());
        }

        #endregion

        #region 1-1-2
        [Test(Name = "GET SERVICES AND GET SCHEDULE SERVICE CAPABILITIES CONSISTENCY",
            Id = "1-1-2",
            Category = Category.SCHEDULE,
            Path = PATH_CAPABILITIES,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleServiceCapabilities, Functionality.GetServices })]
        public void GetServicesAndGetScheduleServiceCapabilitiesConsistencyTest()
        {
            RunTest(() =>
                    {
                        var services = this.GetServices(true);
                        var scheduleService = services.FindService(OnvifService.SCHEDULE_SERVICE);

                        Assert(null != scheduleService,
                               "The DUT didn't return Schedule service",
                               "Check Schedule service is supported");

                        var capabilities = this.ExtractScheduleCapabilities(scheduleService);

                        var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                        var logger = new StringBuilder();
                        Assert(equalScheduleCapabilities(serviceCapabilities, capabilities, logger),
                               logger.ToStringTrimNewLine(),
                               "Check ScheduleServiceCapabilities consistency");
                    });
        }
        #endregion
        #endregion

        #region 2-1-*

        #region 2-1-1

        [Test(Name = "GET SCHEDULE INFO",
            Id = "2-1-1",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleInfo })]
        public void GetScheduleInfoTest()
        {
            RunTest(() =>
            {
                var scheduleInfoList = this.GetFullScheduleInfoListA1();

                if (!scheduleInfoList.Any())
                    return;

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var tokenList = scheduleInfoList.Take((int)serviceCapabilities.MaxLimit).Select(e => e.token);
                var scheduleInfoList1 = this.GetScheduleInfo(tokenList.ToArray());

                var logger = new StringBuilder();
                Assert(validateListFromGetScheduleInfo(scheduleInfoList1, tokenList, logger),
                       logger.ToStringTrimNewLine(),
                       "Checking consistency of received ScheduleInfo item's lists");

                foreach (var scheduleInfo in scheduleInfoList)
                {
                    var apil = this.GetScheduleInfo(new[] { scheduleInfo.token });

                    var msg = string.Empty;
                    var n = apil.Count();
                    if (1 != n)
                    {
                        msg = string.Format("The DUT returned {0} ScheduleInfo item{1} though the single item for token '{2}' is expected",
                                            0 == n ? "no" : "several",
                                            0 == n ? "" : "s",
                                            scheduleInfo.token);
                    }
                    else
                        msg = string.Format("The DUT returned no ScheduleInfo item for token '{0}'", scheduleInfo.token);

                    Assert(1 == apil.Count() && apil.First().token == scheduleInfo.token,
                           msg,
                           "Checking that requested Schedule item is received");

                    logger = new StringBuilder();
                    Assert(equalScheduleInfo(scheduleInfo, apil.First(), logger),
                           logger.ToStringTrimNewLine(),
                           "Checking received ScheduleInfo item");
                }
            });
        }

        #endregion

        #region 2-1-2

        [Test(Name = "GET SCHEDULE INFO LIST - LIMIT",
            Id = "2-1-2",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleInfoList })]
        public void GetScheduleInfoLimitTest()
        {
            RunTest(() =>
            {
                var stepTitle = "Checking received list of ScheduleInfo items";
                var msgHeader = "Received list of ScheduleInfo items contains {0} items but expected not more than {1}";

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                ScheduleInfo[] scheduleInfoList1;
                this.GetScheduleInfoList(1, null, out scheduleInfoList1);

                Assert(scheduleInfoList1.Count() <= 1,
                       string.Format(msgHeader, scheduleInfoList1.Count(), 1),
                       stepTitle);

                if (1 == serviceCapabilities.MaxLimit)
                    return;


                ScheduleInfo[] scheduleInfoList2;
                this.GetScheduleInfoList((int)serviceCapabilities.MaxLimit, null, out scheduleInfoList2);

                Assert(scheduleInfoList2.Count() <= serviceCapabilities.MaxLimit,
                       string.Format(msgHeader, scheduleInfoList2.Count(), serviceCapabilities.MaxLimit),
                       stepTitle);

                if (2 == serviceCapabilities.MaxLimit)
                    return;


                ScheduleInfo[] scheduleInfoList3;
                this.GetScheduleInfoList((int)serviceCapabilities.MaxLimit - 1, null, out scheduleInfoList3);

                Assert(scheduleInfoList3.Count() <= serviceCapabilities.MaxLimit - 1,
                       string.Format(msgHeader, scheduleInfoList3.Count(), serviceCapabilities.MaxLimit - 1),
                       stepTitle);
            });
        }

        #endregion

        #region 2-1-3
        [Test(Name = "GET SCHEDULE INFO LIST - START REFERENCE AND LIMIT",
            Id = "2-1-3",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleInfoList })]
        public void GetScheduleInfoStartReferenceAndLimitTest()
        {
            RunTest(() =>
            {
                var stepTitle = "Checking two complete lists of ScheduleInfo items";
                var msgHeader = "Received complete list of ScheduleInfo items with Limit = '{0}' is not the same as the received one with Limit = '{1}'";

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var listFirst = receiveAndValidateScheduleInfoList((int)serviceCapabilities.MaxLimit);

                if (1 == serviceCapabilities.MaxLimit)
                    return;


                var listSecond = receiveAndValidateScheduleInfoList(1);

                Assert(equalScheduleInfoLists(listFirst, listSecond),
                       string.Format(msgHeader, serviceCapabilities.MaxLimit, 1),
                       stepTitle);

                if (2 == serviceCapabilities.MaxLimit)
                    return;


                var listThird = receiveAndValidateScheduleInfoList((int)serviceCapabilities.MaxLimit - 1);

                Assert(equalScheduleInfoLists(listFirst, listThird),
                       string.Format(msgHeader, serviceCapabilities.MaxLimit, serviceCapabilities.MaxLimit - 1),
                       stepTitle);
            });
        }
        #endregion

        #region 2-1-4
        [Test(Name = "GET SCHEDULE INFO LIST - NO LIMIT",
            Id = "2-1-4",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            RequiredFeatures = new Feature[] { Feature.Schedule },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleInfoList })]
        public void GetScheduleInfoNoLimitTest()
        {
            RunTest(() =>
            {
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var list = receiveAndValidateScheduleInfoList((int)serviceCapabilities.MaxLimit, true);

                Assert(list.Count() <= serviceCapabilities.MaxSchedules,
                       string.Format("The received full list of ScheduleInfo items contains {0} items though the expected number is not more than {1}", list.Count(), serviceCapabilities.MaxSchedules),
                       "Checking complete list of ScheduleInfo items");
            });
        }
        #endregion

        #region 2-1-5 GET SCHEDULE INFO WITH INVALID TOKEN
        [Test(Name = "GET SCHEDULE INFO WITH INVALID TOKEN",
            Id = "2-1-5",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleInfo })]
        public void GetScheduleInfoWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullScheduleInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                ScheduleInfo[] infos = null;
                infos = this.GetScheduleInfo(new string[] { invalidToken });

                Assert(infos == null || infos.Length == 0,
                    "List of ScheduleInfo is not empty",
                    "Check that the DUT returned no ScheduleInfo");

                if (fullList == null || fullList.Count == 0)
                {
                    return;
                }
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;
                if (maxLimit >= 2)
                {
                    infos = this.GetScheduleInfo(new string[] { fullList[0].token, invalidToken });

                    var expected = fullList[0];

                    this.CheckRequestedInfo(infos, expected.token, "Schedule Info", D => D.token);
                }
                else
                {
                    LogTestEvent(string.Format("MaxLimit={0}, skip part with sending request with one correct and one incorrect tokens.", maxLimit) + Environment.NewLine);
                }
            });
        }
        #endregion

        #region 2-1-6 GET SCHEDULE INFO - TOO MANY ITEMS
        [Test(Name = "GET SCHEDULE INFO - TOO MANY ITEMS",
            Id = "2-1-6",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleInfo })]
        public void GetScheduleInfoTooManyItemsTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullScheduleInfoListA1();

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;

                if (fullList == null || fullList.Count <= maxLimit)
                    return;

                string faultCode = "Sender/InvalidArgs/TooManyItems";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    fullList.RemoveRange((int)maxLimit, fullList.Count - (int)maxLimit - 1);
                    this.GetScheduleInfo(fullList.Select(e => e.token).ToArray());
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
        #endregion

        #endregion

        #region 3-1-*

        #region 3-1-1 GET SCHEDULES
        [Test(Name = "GET SCHEDULES",
            Id = "3-1-1",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSchedules })]
        public void GetSchedulesTest()
        {
            RunTest(() =>
            {
                var schedulesList = this.GetFullScheduleListA3();

                if (!schedulesList.Any())
                    return;

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var tokenList = schedulesList.Take((int)serviceCapabilities.MaxLimit).Select(e => e.token);
                var schedulesList1 = this.GetSchedules(tokenList.ToArray());

                var logger = new StringBuilder();
                Assert(validateListFromGetSchedules(schedulesList1, tokenList, logger),
                       logger.ToStringTrimNewLine(),
                       "Checking consistency of received Schedule item's lists");

                foreach (var schedule in schedulesList)
                {
                    var apl = this.GetSchedules(new[] { schedule.token });

                    var msg = string.Empty;
                    var n = apl.Count();
                    if (1 != n)
                    {
                        msg = string.Format("The DUT returned {0} Schedule item{1} though the single item for token '{2}' is expected",
                                            0 == n ? "no" : "several",
                                            0 == n ? "" : "s",
                                            schedule.token);
                    }
                    else
                        msg = string.Format("The DUT returned no Schedule item for token '{0}'", schedule.token);

                    Assert(1 == apl.Count() && apl.First().token == schedule.token,
                           msg,
                           "Checking that requested Schedule item is received");


                    logger = new StringBuilder();
                    Assert(equalSchedule(schedule, apl.First(), logger, "GetScheduleList", "GetSchedules"),
                           logger.ToStringTrimNewLine(),
                           "Checking received Schedule item");
                }
            });
        }
        #endregion

        #region 3-1-2 GET SCHEDULE LIST - LIMIT
        [Test(Name = "GET SCHEDULE LIST - LIMIT",
            Id = "3-1-2",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleList })]
        public void GetScheduleListLimitTest()
        {
            RunTest(() =>
            {
                var stepTitle = "Checking received list of Schedule items";
                var msgHeader = "Received list of Schedule items contains {0} items but expected not more than {1}";

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                Onvif.Schedule[] scheduleList1;
                this.GetScheduleList(1, null, out scheduleList1);

                Assert(scheduleList1.Count() <= 1,
                       string.Format(msgHeader, scheduleList1.Count(), 1),
                       stepTitle);

                if (1 == serviceCapabilities.MaxLimit)
                    return;


                Onvif.Schedule[] scheduleList2;
                this.GetScheduleList((int)serviceCapabilities.MaxLimit, null, out scheduleList2);

                Assert(scheduleList2.Count() <= serviceCapabilities.MaxLimit,
                       string.Format(msgHeader, scheduleList2.Count(), serviceCapabilities.MaxLimit),
                       stepTitle);

                if (2 == serviceCapabilities.MaxLimit)
                    return;


                Onvif.Schedule[] scheduleList3;
                this.GetScheduleList((int)serviceCapabilities.MaxLimit - 1, null, out scheduleList3);

                Assert(scheduleList3.Count() <= serviceCapabilities.MaxLimit - 1,
                       string.Format(msgHeader, scheduleList3.Count(), serviceCapabilities.MaxLimit - 1),
                       stepTitle);
            });
        }
        #endregion

        #region 3-1-3 GET SCHEDULE LIST - START REFERENCE AND LIMIT
        [Test(Name = "GET SCHEDULE LIST - START REFERENCE AND LIMIT",
            Id = "3-1-3",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleList })]
        public void GetScheduleStartReferenceAndLimitTest()
        {
            RunTest(() =>
            {
                var stepTitle = "Checking two complete lists of Schedule items";
                var msgHeader = "Received complete list of Schedule items with Limit = '{0}' is not the same as the received one with Limit = '{1}'";
                var logger = new StringBuilder();

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var listFirst = receiveAndValidateScheduleList((int)serviceCapabilities.MaxLimit);

                if (1 == serviceCapabilities.MaxLimit)
                {
                    logger = new StringBuilder();
                    Assert(validateListFromGetScheduleAndGetScheduleInfoConsistency(this.GetFullScheduleInfoListA1(), listFirst, logger),
                           logger.ToStringTrimNewLine(),
                           "Checking consistency of received ScheduleInfo and Schedule lists");
                    return;
                }

                var listSecond = receiveAndValidateScheduleList(1);

                Assert(equalScheduleInfoLists(listFirst, listSecond),
                       string.Format(msgHeader, serviceCapabilities.MaxLimit, 1),
                       stepTitle);

                if (2 == serviceCapabilities.MaxLimit)
                {
                    logger = new StringBuilder();
                    Assert(validateListFromGetScheduleAndGetScheduleInfoConsistency(this.GetFullScheduleInfoListA1(), listSecond, logger),
                           logger.ToStringTrimNewLine(),
                           "Checking consistency of received ScheduleInfo and Schedule lists");
                    return;
                }

                var listThird = receiveAndValidateScheduleList((int)serviceCapabilities.MaxLimit - 1);

                Assert(equalScheduleInfoLists(listFirst, listThird),
                       string.Format(msgHeader, serviceCapabilities.MaxLimit, serviceCapabilities.MaxLimit - 1),
                       stepTitle);

                logger = new StringBuilder();
                Assert(validateListFromGetScheduleAndGetScheduleInfoConsistency(this.GetFullScheduleInfoListA1(), listThird, logger),
                       logger.ToStringTrimNewLine(),
                       "Checking consistency of received ScheduleInfo and Schedule lists");
            });
        }
        #endregion

        #region 3-1-4 GET SCHEDULE LIST - NO LIMIT
        [Test(Name = "GET SCHEDULE LIST - NO LIMIT",
            Id = "3-1-4",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleList })]
        public void GetScheduleListNoLimitTest()
        {
            RunTest(() =>
            {
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var list = receiveAndValidateScheduleList((int)serviceCapabilities.MaxLimit, true);

                var fullScheduleInfoList = this.GetFullScheduleInfoListA1();

                var logger = new StringBuilder();
                Assert(validateListFromGetScheduleAndGetScheduleInfoConsistency(fullScheduleInfoList, list, logger),
                       logger.ToStringTrimNewLine(),
                       "Checking consistency of received ScheduleInfo and Schedule lists");
            });
        }
        #endregion

        #region 3-1-5
        [Test(Name = "CREATE SCHEDULE",
        Id = "3-1-5",
        Category = Category.SCHEDULE,
        Path = PATH_SCHEDULE,
        Version = 1.0,
        RequirementLevel = RequirementLevel.Optional,
        LastChangedIn = "v15.06",
        RequiredFeatures = new Feature[] { Feature.Schedule },
        FunctionalityUnderTest = new Functionality[] { Functionality.CreateSchedule, Functionality.ConfigurationScheduleChangedEvent })]
        public void CreateScheduleTest()
        {
            string scheduleToken = null;
            string specialDayGroupToken = null;
            SubscriptionHandler subscription = null;
            bool isSpecialDaysSupported = true;

            var logger = new StringBuilder();

            RunTest(() =>
            {
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var scheduleiCalendarValue = this.ScheduleiCalendarGenerationA7(serviceCapabilities.ExtendedRecurrenceSupported);

                // 4. ONVIF Client retrieves an initial complete list of schedules (out scheduleCompleteList1) by following the procedure mentioned in Annex A.3
                var scheduleCompleteList = this.GetFullScheduleListA3();

                // 5. If cap.SpecialDaysSupported is equal to true, ONVIF Client creates SpecialDayGroup (out specialDayGroupToken) by following the procedure mentioned in Annex A.8


                if (serviceCapabilities.SpecialDaysSupported)
                {
                    isSpecialDaysSupported = true;
                    SpecialDayGroup specialDayGroup;
                    specialDayGroupToken = this.CreateSpecialDayGroupA8(out specialDayGroup);
                }

                // 7.	ONVIF Client invokes CreatePullPointSubscription with parameters Filter.TopicExpression := "tns1:Configuration/Schedule/Changed"

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/Schedule/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                // 8. The DUT responds with a CreatePullPointSubscriptionResponse message with parameters
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                // 9. ONVIF client invokes CreateSchedule with parameters

                var schedule = new Onvif.Schedule
                {
                    token = "",
                    Name = "Test Name",
                    Description = "Test Description",
                    Standard = scheduleiCalendarValue,
                };
                if (serviceCapabilities.SpecialDaysSupported)
                {
                    System.DateTime fromDate = new System.DateTime(1, 1, 1, 22, 0, 0);
                    schedule.SpecialDays = new SpecialDaysSchedule[] {
                        new SpecialDaysSchedule
                        {
                            GroupToken = specialDayGroupToken,
                            TimeRange = new TimePeriod[]
                            {
                                new TimePeriod
                                {
                                    From = fromDate,
                                    Until = fromDate.AddHours(1),
                                    UntilSpecified = true
                                }
                            }
                        }
                    };
                }


                scheduleToken = this.CreateSchedule(schedule);
                Assert(!string.IsNullOrEmpty(scheduleToken),
                       string.Format("CreateSchedule returned {0}", null == scheduleToken ? "no token" : "empty token"),
                       "Check token returned by CreateSchedule");
                schedule.token = scheduleToken;

                // 11. Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Schedule.TokenValidation.xq", new List<Onvif.Schedule>() { schedule });

                // 12. If timeout1 timeout expires for step 11 without Notification with ScheduleToken source simple item equal to scheduleToken, FAIL the test and go to the step 22
                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that CreateSchedule notification is received");

                logger.Clear();
                Assert(ValidateNotifications(notifications, topicInfo, "ScheduleToken", scheduleToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");

                // 13.	ONVIF Client retrieves a schedule (in scheduleToken, out scheduleList) by following the procedure mentioned in Annex A.9
                var arrScheduleRetrieves = this.GetSchedules(new[] { scheduleToken });
                ValidateConsistency(schedule, arrScheduleRetrieves.FirstOrDefault(), "CreateSchedule", "GetSchedules");

                var arrScheduleInfoRetrieves = this.GetScheduleInfo(new[] { scheduleToken });
                ValidateConsistency(schedule, arrScheduleInfoRetrieves.FirstOrDefault(), "CreateSchedule", "GetScheduleInfo");

                var fullScheduleInfoList = this.GetFullScheduleInfoListA1();
                ValidateConsistency(schedule, fullScheduleInfoList, "CreateSchedule", "GetScheduleInfoList");

                var fullScheduleList = this.GetFullScheduleListA3();
                ValidateConsistency(schedule, fullScheduleList, "CreateSchedule", "GetScheduleList");

                logger.Clear();
                Assert(validateListFromGetFullScheduleList(fullScheduleList, scheduleCompleteList.Select(e => e.token), logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received full schedule list");
            },
            () =>
            {
                // 22.	ONVIF Client deletes the Schedule (in scheduleToken) by following the procedure mentioned in Annex A.11 to restore DUT configuration
                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(scheduleToken))
                        this.DeleteSchedule(scheduleToken);
                });

                this.AllowFaultStep(() =>
                {
                    if (isSpecialDaysSupported && !string.IsNullOrEmpty(specialDayGroupToken))
                        this.DeleteSpecialDayGroupA12(specialDayGroupToken);
                });

                // 24.	ONVIF Client sends an Unsubscribe to the subscription endpoint s.
                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);

                this.FinishRestoreSettings();
            });
        }

        #endregion

        #region 3-1-6
        [Test(Name = "MODIFY SCHEDULE",
            Id = "3-1-6",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.ModifySchedule, Functionality.ConfigurationScheduleChangedEvent })]
        public void ModifyScheduleTest()
        {
            string scheduleToken = null;
            string specialDayGroupToken = "";
            SubscriptionHandler subscription = null;
            var logger = new StringBuilder();
            bool isSpecialDaysSupported = false;

            RunTest(() =>
            {

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var scheduleiCalendarValue = this.ScheduleiCalendarGenerationA7(serviceCapabilities.ExtendedRecurrenceSupported);

                // 5. If cap.SpecialDaysSupported is equal to true, ONVIF Client creates SpecialDayGroup (out specialDayGroupToken) by following the procedure mentioned in Annex A.8
                if (serviceCapabilities.SpecialDaysSupported)
                {
                    isSpecialDaysSupported = true;
                    SpecialDayGroup specialDayGroup;
                    specialDayGroupToken = this.CreateSpecialDayGroupA8(out specialDayGroup);
                }

                Onvif.Schedule schedule;
                scheduleToken = this.CreateScheduleA13(scheduleiCalendarValue, isSpecialDaysSupported, out schedule, specialDayGroupToken);
                Assert(!string.IsNullOrEmpty(scheduleToken),
                   string.Format("CreateSchedule returned {0}", null == scheduleToken ? "no token" : "empty token"),
                   "Check token returned by CreateSchedule");
                schedule.token = scheduleToken;

                // 8.	ONVIF Client invokes CreatePullPointSubscription with parameters Filter.TopicExpression := “tns1:Configuration/Schedule/Changed”

                subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/Schedule/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                // 9.	The DUT responds with a CreatePullPointSubscriptionResponse message with parameters
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                // 10.	ONVIF client invokes ModifySchedule with parameters
                scheduleiCalendarValue = this.ScheduleiCalendarGenerationA7(serviceCapabilities.ExtendedRecurrenceSupported, 1);
                schedule.token = scheduleToken;
                schedule.Name = "Test Name2";
                schedule.Description = "Test Description2";
                schedule.Standard = scheduleiCalendarValue;
                schedule.SpecialDays = null;

                this.ModifySchedule(schedule);

                // 12.	Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Schedule.TokenValidation.xq", new List<Onvif.Schedule>() { schedule });

                // 13.	If timeout1 timeout expires for step 12 without Notification with ScheduleToken source simple item equal to scheduleToken, FAIL the test and go to the step 23
                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that ModifySchedule notification is received");

                logger.Clear();
                Assert(ValidateNotifications(notifications, topicInfo, "ScheduleToken", scheduleToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");

                // 14.	ONVIF Client retrieves a schedule (in scheduleToken, out scheduleList) by following the procedure mentioned in Annex A.9
                var arrScheduleRetrieves = this.GetSchedules(new[] { scheduleToken });
                ValidateConsistency(schedule, arrScheduleRetrieves.FirstOrDefault(), "ModifySchedule", "GetSchedules");

                var arrScheduleInfoRetrieves = this.GetScheduleInfo(new[] { scheduleToken });
                ValidateConsistency(schedule, arrScheduleInfoRetrieves.FirstOrDefault(), "ModifySchedule", "GetScheduleInfo");

                var fullScheduleInfoList = this.GetFullScheduleInfoListA1();
                ValidateConsistency(schedule, fullScheduleInfoList, "ModifySchedule", "GetScheduleInfoList");

                var fullScheduleList = this.GetFullScheduleListA3();
                ValidateConsistency(schedule, fullScheduleList, "ModifySchedule", "GetScheduleList");

            },
            () =>
            {
                // 22.	ONVIF Client deletes the Schedule (in scheduleToken) by following the procedure mentioned in Annex A.11 to restore DUT configuration
                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(scheduleToken))
                        this.DeleteSchedule(scheduleToken);
                });

                this.AllowFaultStep(() =>
                {
                    if (isSpecialDaysSupported && !string.IsNullOrEmpty(specialDayGroupToken))
                        this.DeleteSpecialDayGroupA12(specialDayGroupToken);
                });

                // 24.	ONVIF Client sends an Unsubscribe to the subscription endpoint s.
                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);

                this.FinishRestoreSettings();

            });
        }
        #endregion

        #region 3-1-7 DELETE SCHEDULE
        [Test(Name = "DELETE SCHEDULE",
            Id = "3-1-7",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteSchedule, Functionality.ConfigurationScheduleRemovedEvent })]
        public void DeleteScheduleTest()
        {
            string scheduleToken = null;
            SubscriptionHandler subscription = null;
            bool isScheduleCreatedAndNotDelete = false;

            var logger = new StringBuilder();

            RunTest(() =>
            {
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                // 3.	ONVIF Client retrieves a complete list of schedules (out scheduleCompleteList1) by following the procedure mentioned in Annex A.3
                var scheduleCompleteList = this.GetFullScheduleListA3();

                var scheduleiCalendarValue = this.ScheduleiCalendarGenerationA7(serviceCapabilities.ExtendedRecurrenceSupported);

                Onvif.Schedule schedule;
                scheduleToken = this.CreateScheduleA13(scheduleiCalendarValue, false, out schedule);

                isScheduleCreatedAndNotDelete = true;

                Assert(!string.IsNullOrEmpty(scheduleToken),
                   string.Format("CreateSchedule returned {0}", null == scheduleToken ? "no token" : "empty token"),
                   "Check token returned by CreateSchedule");
                schedule.token = scheduleToken;

                // 6.	ONVIF Client invokes CreatePullPointSubscription with parameters Filter.TopicExpression := "tns1:Configuration/Schedule/Removed"
                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/Schedule/Removed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                // 7.	The DUT responds with a CreatePullPointSubscriptionResponse message with parameters
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                // 8.	ONVIF Client invokes DeleteSchedule with parameters Token := scheduleToken
                this.DeleteSchedule(scheduleToken);

                isScheduleCreatedAndNotDelete = false;

                // 10.	Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Schedule.TokenValidation.xq", new List<Onvif.Schedule>() { schedule });

                // 11.	If timeout1 timeout expires for step 10 without Notification with ScheduleToken source simple item equal to scheduleToken, FAIL the test and go to the step 21
                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Waiting for notification");

                Assert(ValidateNotifications(notifications, topicInfo, "ScheduleToken", scheduleToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");


                // 12.	ONVIF Client retrieves a schedule (in scheduleToken, out scheduleList) by following the procedure mentioned in Annex A.9
                var schedules = this.GetSchedules(new string[] { scheduleToken });
                // 13.	If scheduleList is not empty, FAIL the test and go step 21
                ValidateDeleteConsistency(schedules, scheduleToken, "GetSchedules", "Verifying removed schedule cannot be retrieved");

                // 14.	ONVIF Client retrieves a schedule info (in scheduleToken, out scheduleInfoList) by following the procedure mentioned in Annex A.10
                var scheduleInfos = this.GetScheduleInfo(new string[] { scheduleToken });
                // 15.	If scheduleInfoList is not empty, FAIL the test and go step 21
                ValidateDeleteConsistency(scheduleInfos, scheduleToken, "GetScheduleInfo", "Verifying removed schedule info cannot be retrieved");

                // 16.	ONVIF Client retrieves a complete schedule information list (out scheduleInfoCompleteList) by following the procedure mentioned in Annex A.1
                var scheduleInfoList = this.GetFullScheduleInfoListA1();

                ValidateDeleteConsistency(scheduleInfoList, scheduleToken, "GetScheduleInfoList", "Verifying list of schedule info", true);

                // 18.	ONVIF Client retrieves a complete list of schedules (out scheduleCompleteList2) by following the procedure mentioned in Annex A.3
                var scheduleList = this.GetFullScheduleListA3(); // scheduleCompleteList

                ValidateDeleteConsistency(scheduleList, scheduleToken, "GetScheduleList", "Verifying list of schedules", true);

                logger.Clear();
                Assert(validateListFromGetFullScheduleList(scheduleList, scheduleCompleteList.Select(e => e.token), logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received full schedule list");
            },
            () =>
            {
                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(scheduleToken) && isScheduleCreatedAndNotDelete)
                        this.DeleteSchedule(scheduleToken);
                });

                // 21.	ONVIF Client sends an Unsubscribe to the subscription endpoint s.
                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);

                this.FinishRestoreSettings();

            });
        }
        #endregion

        #region 3-1-8 GET SCHEDULES WITH INVALID TOKEN
        [Test(Name = "GET SCHEDULES WITH INVALID TOKEN",
            Id = "3-1-8",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSchedules })]
        public void GetSchedulesWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullScheduleInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                Onvif.Schedule[] schedules = null;
                schedules = this.GetSchedules(new string[] { invalidToken });

                Assert(schedules == null || schedules.Length == 0,
                    "List of Schedules is not empty",
                    "Check that the DUT returned no Schedules");

                if (fullList == null || fullList.Count == 0)
                {
                    return;
                }
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;
                if (maxLimit >= 2)
                {
                    schedules = this.GetSchedules(new string[] { fullList[0].token, invalidToken });

                    var expected = fullList[0];

                    this.CheckRequestedInfo(schedules, expected.token, "Schedules", D => D.token);
                }
                else
                {
                    LogTestEvent(string.Format("MaxLimit={0}, skip part with sending request with one correct and one incorrect tokens.", maxLimit) + Environment.NewLine);
                }
            });
        }
        #endregion

        #region 3-1-9 GET SCHEDULE - TOO MANY ITEMS
        [Test(Name = "GET SCHEDULE - TOO MANY ITEMS",
            Id = "3-1-9",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSchedules })]
        public void GetScheduleTooManyItemsTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetFullScheduleListA3();

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;

                if (fullList == null || fullList.Count <= maxLimit)
                    return;

                string faultCode = "Sender/InvalidArgs/TooManyItems";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    fullList.RemoveRange((int)maxLimit, fullList.Count - (int)maxLimit - 1);
                    this.GetSchedules(fullList.Select(e => e.token).ToArray());
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
        #endregion

        #region 3-1-10 CREATE SCHEDULE - NOT EMPTY SCHEDULE TOKEN
        [Test(Name = "CREATE SCHEDULE - NOT EMPTY SCHEDULE TOKEN",
            Id = "3-1-10",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateSchedule })]
        public void CreateScheduleNotEmptyScheduleTokenTest()
        {
            RunTest(() =>
            {

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                var scheduleICalendarValue = this.ScheduleiCalendarGenerationA7(serviceCapabilities.ExtendedRecurrenceSupported);
                var newSchedule = new Onvif.Schedule()
                {
                    token = "ScheduleToken",
                    Description = "Test Description",
                    Name = "Test Name",
                    Standard = scheduleICalendarValue
                };

                string faultCode = "Sender/InvalidArgs";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    var scheduleToken = this.CreateSchedule(newSchedule);
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

        #endregion

        #region 3-1-11 CREATE SCHEDULE - TOO MANY TIME PERIODS PER DAY
        [Test(Name = "CREATE SCHEDULE - TOO MANY TIME PERIODS PER DAY",
            Id = "3-1-11",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateSchedule })]
        public void CreateScheduleTooManyTimePeriodsPerDayTest()
        {
            string scheduleToken = string.Empty;
            RunTest(() =>
            {
                Onvif.Schedule newSchedule = null;
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                uint numberPeriodsPerDay = 0;
                string standardValue = "";
                numberPeriodsPerDay = serviceCapabilities.MaxTimePeriodsPerDay;
                if (numberPeriodsPerDay != 1)
                {
                    standardValue = this.HelperScheduleiCalTimePeriodsGenerationA17(numberPeriodsPerDay, serviceCapabilities.ExtendedRecurrenceSupported);
                    newSchedule = new Onvif.Schedule()
                    {
                        token = "",
                        Description = "Test Description",
                        Name = "Test Name",
                        Standard = standardValue
                    };
                    scheduleToken = this.CreateSchedule(newSchedule);
                    this.DeleteSchedule(scheduleToken);
                    scheduleToken = string.Empty;
                }

                numberPeriodsPerDay++;
                standardValue = this.HelperScheduleiCalTimePeriodsGenerationA17(numberPeriodsPerDay, serviceCapabilities.ExtendedRecurrenceSupported);

                newSchedule = new Onvif.Schedule()
                {
                    token = "",
                    Description = "Test Description",
                    Name = "Test Name",
                    Standard = standardValue
                };

                string faultCode = "Sender/CapabilityViolated/MaxTimePeriodsPerDay";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    scheduleToken = this.CreateSchedule(newSchedule);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            },
            () =>
            {
                if (!string.IsNullOrEmpty(scheduleToken))
                    this.DeleteSchedule(scheduleToken);
            });
        }

        #endregion

        #region 3-1-12 CREATE SCHEDULE - INVALID TIME RANGE INTERVAL
        [Test(Name = "CREATE SCHEDULE - INVALID TIME RANGE INTERVAL",
            Id = "3-1-12",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateSchedule })]
        public void CreateScheduleInvalidTimeRangeIntervalTest()
        {
            string scheduleToken = string.Empty;
            string specialDayGroupToken = string.Empty;
            RunTest(() =>
            {
                Onvif.Schedule newSchedule = null;
                SpecialDayGroup newSpecialDayGroup = null;
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                specialDayGroupToken = this.CreateSpecialDayGroupA8(out newSpecialDayGroup);

                var standartICalendarValue = this.ScheduleiCalendarGenerationA7(serviceCapabilities.ExtendedRecurrenceSupported);

                System.DateTime fromDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 10, 0, 0);
                System.DateTime untilDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 9, 0, 0);

                newSchedule = new Onvif.Schedule()
                {
                    token = "",
                    Description = "Test Description",
                    Name = "Test Name",
                    Standard = standartICalendarValue,
                    SpecialDays = new SpecialDaysSchedule[] {
                        new SpecialDaysSchedule() {
                            GroupToken = specialDayGroupToken,
                            TimeRange = new TimePeriod[] {
                                new TimePeriod() {
                                    From = fromDate,
                                    Until = untilDate,
                                    UntilSpecified = true
                                }
                            }
                        }
                    }
                };

                string faultCode = "Sender/InvalidArgVal/ReferenceNotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    scheduleToken = this.CreateSchedule(newSchedule);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            },
            () =>
            {
                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(scheduleToken))
                        this.DeleteSchedule(scheduleToken);
                });

                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(specialDayGroupToken))
                        this.DeleteSpecialDayGroup(specialDayGroupToken);
                });

                this.FinishRestoreSettings();
            });
        }

        #endregion

        #region 3-1-13 MODIFY SCHEDULE WITH INVALID TOKEN
        [Test(Name = "MODIFY SCHEDULE WITH INVALID TOKEN",
            Id = "3-1-13",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.ModifySchedule })]
        public void ModifyScheduleWithInvalidTokenTest()
        {
            RunTest(() =>
            {

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                var fullList = this.GetFullScheduleInfoListA1();
                var scheduleICalendarValue = this.ScheduleiCalendarGenerationA7(serviceCapabilities.ExtendedRecurrenceSupported);

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                var invalidSchedule = new Onvif.Schedule()
                {
                    token = invalidToken,
                    Name = "Test Name",
                    Description = "Test Description",
                    Standard = scheduleICalendarValue
                };

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.ModifySchedule(invalidSchedule);
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

        #region 3-1-14 MODIFY SCHEDULE - TOO MANY TIME PERIODS PER DAY
        [Test(Name = "MODIFY SCHEDULE - TOO MANY TIME PERIODS PER DAY",
            Id = "3-1-14",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.ModifySchedule })]
        public void ModifyScheduleTooManyTimePeriodsPerDayTest()
        {
            string scheduleToken = string.Empty;
            RunTest(() =>
            {
                Onvif.Schedule newSchedule = null;
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                uint numberPeriodsPerDay = 0;
                string standardValue = "";
                string scheduleICalendarValue = this.ScheduleiCalendarGenerationA7(serviceCapabilities.ExtendedRecurrenceSupported);
                scheduleToken = this.CreateScheduleA13(scheduleICalendarValue, false, out newSchedule);

                numberPeriodsPerDay = serviceCapabilities.MaxTimePeriodsPerDay;
                if (numberPeriodsPerDay != 1)
                {
                    standardValue = this.HelperScheduleiCalTimePeriodsGenerationA17(numberPeriodsPerDay, serviceCapabilities.ExtendedRecurrenceSupported);
                    newSchedule = new Onvif.Schedule()
                    {
                        token = scheduleToken,
                        Description = "Test Description",
                        Name = "Test Name",
                        Standard = standardValue
                    };
                    this.ModifySchedule(newSchedule);
                }

                numberPeriodsPerDay++;
                standardValue = this.HelperScheduleiCalTimePeriodsGenerationA17(numberPeriodsPerDay, serviceCapabilities.ExtendedRecurrenceSupported);

                newSchedule = new Onvif.Schedule()
                {
                    token = scheduleToken,
                    Description = "Test Description",
                    Name = "Test Name",
                    Standard = standardValue
                };

                string faultCode = "Sender/CapabilityViolated/MaxTimePeriodsPerDay";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.ModifySchedule(newSchedule);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            },
            () =>
            {
                if (!string.IsNullOrEmpty(scheduleToken))
                    this.DeleteSchedule(scheduleToken);
            });
        }

        #endregion

        #region 3-1-15 MODIFY SCHEDULE - INVALID TIME RANGE INTERVAL
        [Test(Name = "MODIFY SCHEDULE - INVALID TIME RANGE INTERVAL",
            Id = "3-1-15",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.ModifySchedule })]
        public void ModifyScheduleInvalidTimeRangeIntervalTest()
        {
            string scheduleToken = string.Empty;
            string specialDayGroupToken = string.Empty;
            RunTest(() =>
            {
                Onvif.Schedule newSchedule = null;
                SpecialDayGroup newSpecialDayGroup = null;
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                specialDayGroupToken = this.CreateSpecialDayGroupA8(out newSpecialDayGroup);

                var standartICalendarValue = this.ScheduleiCalendarGenerationA7(serviceCapabilities.ExtendedRecurrenceSupported);

                scheduleToken = this.CreateScheduleA13(standartICalendarValue, true, out newSchedule, specialDayGroupToken);

                System.DateTime fromDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 10, 0, 0);
                System.DateTime untilDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 9, 0, 0);

                newSchedule = new Onvif.Schedule()
                {
                    token = scheduleToken,
                    Description = "Test Description",
                    Name = "Test Name",
                    Standard = standartICalendarValue,
                    SpecialDays = new SpecialDaysSchedule[] {
                        new SpecialDaysSchedule() {
                            GroupToken = specialDayGroupToken,
                            TimeRange = new TimePeriod[] {
                                new TimePeriod() {
                                    From = fromDate,
                                    Until = untilDate,
                                    UntilSpecified = true
                                }
                            }
                        }
                    }
                };

                string faultCode = "Sender/InvalidArgVal/ReferenceNotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.ModifySchedule(newSchedule);
                }
                catch (FaultException e)
                {
                    if (e.IsValidOnvifFault(faultCode))
                        StepPassed();
                    else
                    {
                        LogStepEvent(string.Format("WARNING: The DUT send wrong SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));
                        StepPassed();
                    }

                    return;
                }

                Assert(false, string.Format("The DUT didn't send SOAP 1.2 fault message. The {0} SOAP 1.2 fault message is expected", faultExpectedSequence));

            },
            () =>
            {
                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(scheduleToken))
                        this.DeleteSchedule(scheduleToken);
                });

                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(specialDayGroupToken))
                        this.DeleteSpecialDayGroup(specialDayGroupToken);
                });

                this.FinishRestoreSettings();
            });
        }

        #endregion

        #region 3-1-16 DELETE SCHEDULE WITH INVALID TOKEN
        [Test(Name = "DELETE SCHEDULE WITH INVALID TOKEN",
            Id = "3-1-16",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteSchedule })]
        public void DeleteScheduleWithInvalidTokenTest()
        {
            RunTest(() =>
            {

                var fullList = this.GetFullScheduleInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.DeleteSchedule(invalidToken);
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

        #region 4-1-*

        #region 4-1-1 GET SPECIAL DAY GROUP INFO
        [Test(Name = "GET SPECIAL DAY GROUP INFO",
            Id = "4-1-1",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSpecialDayGroupInfo })]
        public void GetSpecialDayGroupInfoTest()
        {
            RunTest(() =>
            {
                var specialDayGroupInfoList = this.GetSpecialDayGroupInfoListA4();

                if (!specialDayGroupInfoList.Any())
                    return;

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var tokenList = specialDayGroupInfoList.Take((int)serviceCapabilities.MaxLimit).Select(e => e.token);
                var specialDayGroupInfoList1 = this.GetSpecialDayGroupInfo(tokenList.ToArray());

                var logger = new StringBuilder();
                Assert(validateListFromGetSpecialDayGroupInfo(specialDayGroupInfoList1, tokenList, logger),
                       logger.ToStringTrimNewLine(),
                       "Checking consistency of received Special Day Group Info item's lists");

                foreach (var specialDayGroupInfo in specialDayGroupInfoList)
                {
                    var apl = this.GetSpecialDayGroupInfo(new[] { specialDayGroupInfo.token });

                    var msg = string.Empty;
                    var n = apl.Count();
                    if (1 != n)
                    {
                        msg = string.Format("The DUT returned {0} Special Day Group Info item{1} though the single item for token '{2}' is expected",
                                            0 == n ? "no" : "several",
                                            0 == n ? "" : "s",
                                            specialDayGroupInfo.token);
                    }
                    else
                        msg = string.Format("The DUT returned no SpecialDayGroupInfo item for token '{0}'", specialDayGroupInfo.token);

                    Assert(1 == apl.Count() && apl.First().token == specialDayGroupInfo.token,
                           msg,
                           "Checking that requested Special Day Group Info item is received");


                    logger = new StringBuilder();
                    Assert(equalSpecialDayGroupInfo(specialDayGroupInfo, apl.First(), logger, "GetSpecialDayGroupInfoList", "GetSpecialDayGroupInfo"),
                           logger.ToStringTrimNewLine(),
                           "Checking received Special Day Group Info item");
                }
            });
        }
        #endregion

        #region 4-1-2

        [Test(Name = "GET SPECIAL DAY GROUP INFO LIST - LIMIT",
            Id = "4-1-2",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSpecialDayGroupInfoList })]
        public void GetSpecialDayGroupInfoLimitTest()
        {
            RunTest(() =>
            {
                var stepTitle = "Checking received list of SpecialDayGroupInfo items";
                var msgHeader = "Received list of SpecialDayGroupInfo items contains {0} items but expected not more than {1}";

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                SpecialDayGroupInfo[] specialDayGroupInfoList1;
                this.GetSpecialDayGroupInfoList(1, null, out specialDayGroupInfoList1);

                Assert(specialDayGroupInfoList1.Count() <= 1,
                       string.Format(msgHeader, specialDayGroupInfoList1.Count(), 1),
                       stepTitle);

                if (1 == serviceCapabilities.MaxLimit)
                    return;


                SpecialDayGroupInfo[] specialDayGroupInfoList2;
                this.GetSpecialDayGroupInfoList((int)serviceCapabilities.MaxLimit, null, out specialDayGroupInfoList2);

                Assert(specialDayGroupInfoList2.Count() <= serviceCapabilities.MaxLimit,
                       string.Format(msgHeader, specialDayGroupInfoList2.Count(), serviceCapabilities.MaxLimit),
                       stepTitle);

                if (2 == serviceCapabilities.MaxLimit)
                    return;


                SpecialDayGroupInfo[] specialDayGroupInfoList3;
                this.GetSpecialDayGroupInfoList((int)serviceCapabilities.MaxLimit - 1, null, out specialDayGroupInfoList3);

                Assert(specialDayGroupInfoList3.Count() <= serviceCapabilities.MaxLimit - 1,
                       string.Format(msgHeader, specialDayGroupInfoList3.Count(), serviceCapabilities.MaxLimit - 1),
                       stepTitle);
            });
        }

        #endregion

        #region 4-1-3
        [Test(Name = "GET SPECIAL DAY GROUP INFO - START REFERENCE AND LIMIT",
            Id = "4-1-3",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSpecialDayGroupInfoList })]
        public void GetSpecialDayGroupInfoStartReferenceAndLimitTest()
        {
            RunTest(() =>
            {
                var stepTitle = "Checking two complete lists of SpecialDayGroupInfo items";
                var msgHeader = "Received complete list of SpecialDayGroupInfo items with Limit = '{0}' is not the same as the received one with Limit = '{1}'";

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var listFirst = receiveAndValidateSpecialDayGroupInfoList((int)serviceCapabilities.MaxLimit);

                if (1 == serviceCapabilities.MaxLimit)
                    return;


                var listSecond = receiveAndValidateSpecialDayGroupInfoList(1);

                Assert(equalSpecialDayGroupInfoLists(listFirst, listSecond),
                       string.Format(msgHeader, serviceCapabilities.MaxLimit, 1),
                       stepTitle);

                if (2 == serviceCapabilities.MaxLimit)
                    return;


                var listThird = receiveAndValidateSpecialDayGroupInfoList((int)serviceCapabilities.MaxLimit - 1);

                Assert(equalSpecialDayGroupInfoLists(listFirst, listThird),
                       string.Format(msgHeader, serviceCapabilities.MaxLimit, serviceCapabilities.MaxLimit - 1),
                       stepTitle);
            });
        }
        #endregion

        #region 4-1-4
        [Test(Name = "GET SPECIAL DAY GROUP INFO LIST - NO LIMIT",
            Id = "4-1-4",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v15.06",
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSpecialDayGroupInfoList })]
        public void GetSpecialDayGroupInfoNoLimitTest()
        {
            RunTest(() =>
            {
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var list = receiveAndValidateSpecialDayGroupInfoList((int)serviceCapabilities.MaxLimit, true);

                Assert(list.Count() <= serviceCapabilities.MaxSpecialDayGroups,
                       string.Format("The received full list of SpecialDayGroupInfo items contains {0} items though the expected number is not more than {1}", list.Count(), serviceCapabilities.MaxSchedules),
                       "Checking complete list of SpecialDayGroupInfo items");
            });
        }
        #endregion

        #region 4-1-5 GET SPECIAL DAY GROUP INFO WITH INVALID TOKEN
        [Test(Name = "GET SPECIAL DAY GROUP INFO WITH INVALID TOKEN",
            Id = "4-1-5",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSpecialDayGroupInfo })]
        public void GetSpecialDayGroupInfoWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetSpecialDayGroupInfoListA4();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                SpecialDayGroupInfo[] infos = null;
                infos = this.GetSpecialDayGroupInfo(new string[] { invalidToken });

                Assert(infos == null || infos.Length == 0,
                    "List of SpecialDayGroupInfo is not empty",
                    "Check that the DUT returned no SpecialDayGroupInfo");

                if (fullList == null || fullList.Count == 0)
                {
                    return;
                }
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;
                if (maxLimit >= 2)
                {
                    infos = this.GetSpecialDayGroupInfo(new string[] { fullList[0].token, invalidToken });

                    var expected = fullList[0];

                    this.CheckRequestedInfo(infos, expected.token, "Special Day Group Info", D => D.token);
                }
                else
                {
                    LogTestEvent(string.Format("MaxLimit={0}, skip part with sending request with one correct and one incorrect tokens.", maxLimit) + Environment.NewLine);
                }
            });
        }
        #endregion

        #region 4-1-6 GET SPECIAL DAY GROUP INFO - TOO MANY ITEMS
        [Test(Name = "GET SPECIAL DAY GROUP INFO - TOO MANY ITEMS",
            Id = "4-1-6",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP_INFO,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSpecialDayGroupInfo })]
        public void GetSpecialDayGroupInfoTooManyItemsTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetSpecialDayGroupInfoListA4();

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;

                if (fullList == null || fullList.Count <= maxLimit)
                    return;

                string faultCode = "Sender/InvalidArgs/TooManyItems";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    fullList.RemoveRange((int)maxLimit, fullList.Count - (int)maxLimit - 1);
                    this.GetSpecialDayGroupInfo(fullList.Select(e => e.token).ToArray());
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
        #endregion

        #endregion

        #region 5-1-*

        #region 5-1-1
        [Test(Name = "GET SPECIAL DAY GROUPS",
            Id = "5-1-1",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSpecialDayGroups })]
        public void GetSpecialDayGroupsTest()
        {
            RunTest(() =>
            {
                var specialDayGroupList = this.GetSpecialDayGroupListA5();

                if (!specialDayGroupList.Any())
                    return;

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var tokenList = specialDayGroupList.Take((int)serviceCapabilities.MaxLimit).Select(e => e.token);
                var specialDayGroupList1 = this.GetSpecialDayGroups(tokenList.ToArray());

                var logger = new StringBuilder();
                Assert(validateListFromGetSpecialDayGroups(specialDayGroupList1, tokenList, logger),
                       logger.ToStringTrimNewLine(),
                       "Checking consistency of received Special Day Group item's lists");

                foreach (var specialDayGroup in specialDayGroupList)
                {
                    var apl = this.GetSpecialDayGroups(new[] { specialDayGroup.token });

                    var msg = string.Empty;
                    var n = apl.Count();
                    if (1 != n)
                    {
                        msg = string.Format("The DUT returned {0} Special Day Group item{1} though the single item for token '{2}' is expected",
                                            0 == n ? "no" : "several",
                                            0 == n ? "" : "s",
                                            specialDayGroup.token);
                    }
                    else
                        msg = string.Format("The DUT returned no Special Day Group item for token '{0}'", specialDayGroup.token);

                    Assert(1 == apl.Count() && apl.First().token == specialDayGroup.token,
                           msg,
                           "Checking that requested Special Day Group item is received");


                    logger = new StringBuilder();
                    Assert(equalSpecialDayGroup(specialDayGroup, apl.First(), logger, "GetSpecialDayGroupList", "GetSpecialDayGroups"),
                           logger.ToStringTrimNewLine(),
                           "Checking received Special Day Group item");
                }
            });
        }
        #endregion

        #region 5-1-2
        [Test(Name = "GET SPECIAL DAY GROUP LIST - LIMIT",
            Id = "5-1-2",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSpecialDayGroupList })]
        public void GetSpecialDayGroupListLimitTest()
        {
            RunTest(() =>
            {
                var stepTitle = "Checking received list of Special Day Group items";
                var msgHeader = "Received list of Special Day Group items contains {0} items but expected not more than {1}";

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                SpecialDayGroup[] specialDayGroupList1;
                this.GetSpecialDayGroupList(1, null, out specialDayGroupList1);

                Assert(specialDayGroupList1.Count() <= 1,
                       string.Format(msgHeader, specialDayGroupList1.Count(), 1),
                       stepTitle);

                if (1 == serviceCapabilities.MaxLimit)
                    return;


                SpecialDayGroup[] specialDayGroupList2;
                this.GetSpecialDayGroupList((int)serviceCapabilities.MaxLimit, null, out specialDayGroupList2);

                Assert(specialDayGroupList2.Count() <= serviceCapabilities.MaxLimit,
                       string.Format(msgHeader, specialDayGroupList2.Count(), serviceCapabilities.MaxLimit),
                       stepTitle);

                if (2 == serviceCapabilities.MaxLimit)
                    return;


                SpecialDayGroup[] specialDayGroupList3;
                this.GetSpecialDayGroupList((int)serviceCapabilities.MaxLimit - 1, null, out specialDayGroupList3);

                Assert(specialDayGroupList3.Count() <= serviceCapabilities.MaxLimit - 1,
                       string.Format(msgHeader, specialDayGroupList3.Count(), serviceCapabilities.MaxLimit - 1),
                       stepTitle);
            });
        }
        #endregion

        #region 5-1-3
        [Test(Name = "GET SPECIAL DAY GROUP LIST - START REFERENCE AND LIMIT",
            Id = "5-1-3",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSpecialDayGroupList })]
        public void GetSpecialDayGroupStartReferenceAndLimitTest()
        {
            RunTest(() =>
            {
                var stepTitle = "Checking two complete lists of Special Day Group items";
                var msgHeader = "Received complete list of Special Day Group items with Limit = '{0}' is not the same as the received one with Limit = '{1}'";
                var logger = new StringBuilder();

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var listFirst = receiveAndValidateSpecialDayGroupList((int)serviceCapabilities.MaxLimit);

                if (1 == serviceCapabilities.MaxLimit)
                {
                    logger = new StringBuilder();
                    Assert(validateListFromGetSpecialDayGroupAndGetSpecialDayGroupInfoConsistency(this.GetSpecialDayGroupInfoListA4(), listFirst, logger),
                           logger.ToStringTrimNewLine(),
                           "Checking consistency of received SpecialDayGroupInfo and SpecialDayGroup lists");
                    return;
                }

                var listSecond = receiveAndValidateSpecialDayGroupList(1);

                Assert(equalSpecialDayGroupInfoLists(listFirst, listSecond),
                       string.Format(msgHeader, serviceCapabilities.MaxLimit, 1),
                       stepTitle);

                if (2 == serviceCapabilities.MaxLimit)
                {
                    logger = new StringBuilder();
                    Assert(validateListFromGetSpecialDayGroupAndGetSpecialDayGroupInfoConsistency(this.GetSpecialDayGroupInfoListA4(), listSecond, logger),
                           logger.ToStringTrimNewLine(),
                           "Checking consistency of received SpecialDayGroupInfo and SpecialDayGroup lists");
                    return;
                }

                var listThird = receiveAndValidateSpecialDayGroupList((int)serviceCapabilities.MaxLimit - 1);

                Assert(equalSpecialDayGroupInfoLists(listFirst, listThird),
                       string.Format(msgHeader, serviceCapabilities.MaxLimit, serviceCapabilities.MaxLimit - 1),
                       stepTitle);

                logger = new StringBuilder();
                Assert(validateListFromGetSpecialDayGroupAndGetSpecialDayGroupInfoConsistency(this.GetSpecialDayGroupInfoListA4(), listThird, logger),
                       logger.ToStringTrimNewLine(),
                       "Checking consistency of received SpecialDayGroupInfo and SpecialDayGroup lists");
            });
        }
        #endregion

        #region 5-1-4
        [Test(Name = "GET SPECIAL DAY GROUP LIST - NO LIMIT",
            Id = "5-1-4",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSpecialDayGroupList })]
        public void GetSpecialDayGroupListNoLimitTest()
        {
            RunTest(() =>
            {
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var list = receiveAndValidateSpecialDayGroupList((int)serviceCapabilities.MaxLimit, true);

                var fullSpecialDayGroupInfoList = this.GetSpecialDayGroupInfoListA4();

                var logger = new StringBuilder();
                Assert(validateListFromGetSpecialDayGroupAndGetSpecialDayGroupInfoConsistency(fullSpecialDayGroupInfoList, list, logger),
                       logger.ToStringTrimNewLine(),
                       "Checking consistency of received SpecialDayGroupInfo and SpecialDayGroup lists");
            });
        }
        #endregion

        #region 5-1-5
        [Test(Name = "CREATE SPECIAL DAY GROUP",
        Id = "5-1-5",
        Category = Category.SCHEDULE,
        Path = PATH_SPECIAL_DAY_GROUP,
        Version = 1.0,
        RequirementLevel = RequirementLevel.Optional,
        LastChangedIn = "v15.06",
        RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
        FunctionalityUnderTest = new Functionality[] { Functionality.CreateSpecialDayGroup, Functionality.ConfigurationSpecialDaysChangedEvent })]
        public void CreateSpecialDayGroupTest()
        {
            string specialDayGroupToken = null;
            SubscriptionHandler subscription = null;

            var logger = new StringBuilder();

            RunTest(() =>
            {
                // 3.	ONVIF Client retrieves an initial complete list of SpecialDaysGroup (out specialDayGroupsCompleteList1) by following the procedure mentioned in Annex A.5
                var specialDayGroupCompleteList = this.GetSpecialDayGroupListA5();

                // 4.	ONVIF Client generates Unique Identifier value for UID field in iCalendar (out uid) by following the procedure mentioned in Annex A.6
                Guid specialDayGroupUid = this.UIDiCalendarGenerationA6();
                System.DateTime startDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 0, 0, 0);
                System.DateTime endDate = new System.DateTime(System.DateTime.Now.AddDays(1).Year, System.DateTime.Now.AddDays(1).Month, System.DateTime.Now.AddDays(1).Day, 0, 0, 0);

                var specialDayGroupDays = GetSpecialDaysiCalendarValue(startDate, endDate, specialDayGroupUid);

                // 6.	ONVIF Client invokes CreatePullPointSubscription with parameters Filter.TopicExpression := "Configuration/SpecialDays/Changed"

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/SpecialDays/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                // 7.	The DUT responds with a CreatePullPointSubscriptionResponse message with parameters
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                // 8.	ONVIF client invokes CreateSpecialDayGroup with parameters

                var specialDayGroup = new SpecialDayGroup
                {
                    token = "",
                    Name = "Test SpecialDayGroup Name",
                    Description = "Test SpecialDayGroup Description",
                    Days = specialDayGroupDays,
                };

                specialDayGroupToken = this.CreateSpecialDayGroup(specialDayGroup);

                Assert(!string.IsNullOrEmpty(specialDayGroupToken),
                       string.Format("CreateSpecialDayGroup returned {0}", null == specialDayGroupToken ? "no token" : "empty token"),
                       "Check token returned by CreateSpecialDayGroup");
                specialDayGroup.token = specialDayGroupToken;

                // 10.	Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Schedule.SpecialDayTokenValidation.xq", new List<SpecialDayGroup>() { specialDayGroup });

                // 11.	If timeout1 timeout expires for step 10 without Notification with SpecialDaysToken source simple item equal to specialDayGroupToken, FAIL the test and go to the step 21
                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that CreateSpecialDayGroup notification is received");

                logger.Clear();
                Assert(ValidateNotifications(notifications, topicInfo, "SpecialDaysToken", specialDayGroupToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");

                // 12.	ONVIF Client retrieves a SpecialDayGroup (in specialDayGroupToken, out specialDayGroupList) by following the procedure mentioned in Annex A.14

                var arrSpecialDayGroupRetrieves = this.GetSpecialDayGroups(new[] { specialDayGroupToken });
                ValidateConsistency(specialDayGroup, arrSpecialDayGroupRetrieves.FirstOrDefault(), "CreateSpecialDayGroup", "GetSpecialDayGroups");

                var arrSpecialDayGroupInfoRetrieves = this.GetSpecialDayGroupInfo(new[] { specialDayGroupToken });
                ValidateConsistency(specialDayGroup, arrSpecialDayGroupInfoRetrieves.FirstOrDefault(), "CreateSpecialDayGroup", "GetSpecialDayGroupInfo");

                var fullSpecialDayGroupInfoInfoList = this.GetSpecialDayGroupInfoListA4();
                ValidateConsistency(specialDayGroup, fullSpecialDayGroupInfoInfoList, "CreateSpecialDayGroup", "GetSpecialDayGroupInfoList");

                var fullSpecialDayGroupList = this.GetSpecialDayGroupListA5();
                ValidateConsistency(specialDayGroup, fullSpecialDayGroupList, "CreateSpecialDayGroup", "GetSpecialDayGroupList");

                logger.Clear();
                Assert(validateListFromGetFullSpecialDaysList(fullSpecialDayGroupList, specialDayGroupCompleteList.Select(e => e.token), logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received full special day group list");

            },
            () =>
            {

                this.AllowFaultStep(() =>
                {
                    // 21.	ONVIF Client deletes the SpecialDayGroup (in specialDayGroupToken) by following the procedure mentioned in Annex A.12 to restore DUT configuration
                    if (!string.IsNullOrEmpty(specialDayGroupToken))
                        this.DeleteSpecialDayGroup(specialDayGroupToken);
                });

                // 22.	ONVIF Client sends an Unsubscribe to the subscription endpoint s
                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);

                this.FinishRestoreSettings();

            });
        }

        #endregion

        #region 5-1-6
        [Test(Name = "MODIFY SPECIAL DAY GROUP",
        Id = "5-1-6",
        Category = Category.SCHEDULE,
        Path = PATH_SPECIAL_DAY_GROUP,
        Version = 1.0,
        RequirementLevel = RequirementLevel.Optional,
        LastChangedIn = "v15.06",
        RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
        FunctionalityUnderTest = new Functionality[] { Functionality.ModifySpecialDayGroup, Functionality.ConfigurationSpecialDaysChangedEvent })]
        public void ModifySpecialDayGroupTest()
        {
            string specialDayGroupToken = null;
            SubscriptionHandler subscription = null;

            var logger = new StringBuilder();

            RunTest(() =>
            {
                // 3.	ONVIF Client creats SpecialDayGroup (out specialDayGroupToken) with iCalendar value of Days field (out days) by following the procedure mentioned in Annex A.8
                SpecialDayGroup specialDayGroup;
                specialDayGroupToken = this.CreateSpecialDayGroupA8(out specialDayGroup);

                Assert(!string.IsNullOrEmpty(specialDayGroupToken),
                       string.Format("CreateSpecialDayGroup returned {0}", null == specialDayGroupToken ? "no token" : "empty token"),
                       "Check token returned by CreateSpecialDayGroup");
                specialDayGroup.token = specialDayGroupToken;

                // 4.	Set the following: days := days with changed <day> value in DTSTART field (increase day value in one day) and with changed <day> value in DTEND field (increase day value in one day)
                System.DateTime startDate = new System.DateTime(System.DateTime.Now.AddDays(1).Year, System.DateTime.Now.AddDays(1).Month, System.DateTime.Now.AddDays(1).Day, 0, 0, 0);
                System.DateTime endDate = new System.DateTime(System.DateTime.Now.AddDays(1).Year, System.DateTime.Now.AddDays(1).Month, System.DateTime.Now.AddDays(1).Day, 0, 0, 0);
                Guid specialDayGroupUid = this.UIDiCalendarGenerationA6();

                specialDayGroup.Days = GetSpecialDaysiCalendarValue(startDate, endDate, specialDayGroupUid);

                // 5.	ONVIF Client invokes CreatePullPointSubscription with parameters Filter.TopicExpression := "tns1:Configuration/SpecialDays/Changed"

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/SpecialDays/Changed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                // 6.	The DUT responds with a CreatePullPointSubscriptionResponse message with parameters
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                // 7.	ONVIF client invokes ModifySpecialDayGroup with parameters

                specialDayGroup.Name = "Test SpecialDayGroup Name2";
                specialDayGroup.Description = "Test SpecialDayGroup Description2";

                startDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 0, 0, 0);
                endDate = new System.DateTime(System.DateTime.Now.AddDays(2).Year, System.DateTime.Now.AddDays(2).Month, System.DateTime.Now.AddDays(2).Day, 0, 0, 0);

                specialDayGroup.Days = GetSpecialDaysiCalendarValue(startDate, endDate, specialDayGroupUid);

                this.ModifySpecialDayGroup(specialDayGroup);

                // 9.	Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Schedule.SpecialDayTokenValidation.xq", new List<SpecialDayGroup>() { specialDayGroup });

                // 10.	If timeout1 timeout expires for step 9 without Notification with SpecialDaysToken source simple item equal to specialDayGroupToken, FAIL the test and go to the step 19
                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that ModifySpecialDayGroup notification is received");

                logger.Clear();
                Assert(ValidateNotifications(notifications, topicInfo, "SpecialDaysToken", specialDayGroupToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");

                // 11.	ONVIF Client retrieves a SpecialDayGroup (in specialDayGroupToken, out specialDayGroupList) by following the procedure mentioned in Annex A.14
                var arrSpecialDayGroupRetrieves = this.GetSpecialDayGroups(new[] { specialDayGroupToken });
                ValidateConsistency(specialDayGroup, arrSpecialDayGroupRetrieves.FirstOrDefault(), "ModifySpecialDayGroup", "GetSpecialDayGroups");

                var arrSpecialDayGroupInfoRetrieves = this.GetSpecialDayGroupInfo(new[] { specialDayGroupToken });
                ValidateConsistency(specialDayGroup, arrSpecialDayGroupInfoRetrieves.FirstOrDefault(), "ModifySpecialDayGroup", "GetSpecialDayGroupInfo");

                var fullSpecialDayGroupInfoInfoList = this.GetSpecialDayGroupInfoListA4();
                ValidateConsistency(specialDayGroup, fullSpecialDayGroupInfoInfoList, "ModifySpecialDayGroup", "GetSpecialDayGroupInfoList");

                var fullSpecialDayGroupList = this.GetSpecialDayGroupListA5();
                ValidateConsistency(specialDayGroup, fullSpecialDayGroupList, "ModifySpecialDayGroup", "GetSpecialDayGroupList");

            },
            () =>
            {
                this.AllowFaultStep(() =>
                {
                    // 19.	ONVIF Client deletes the SpecialDayGroup (in specialDayGroupToken) by following the procedure mentioned in Annex A.12 to restore DUT configuration
                    if (!string.IsNullOrEmpty(specialDayGroupToken))
                        this.DeleteSpecialDayGroup(specialDayGroupToken);
                });

                // 20.	ONVIF Client sends an Unsubscribe to the subscription endpoint s
                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);

                this.FinishRestoreSettings();

            });
        }

        #endregion

        #region 5-1-7 DELETE SPECIAL DAY GROUP
        [Test(Name = "DELETE SPECIAL DAY GROUP",
            Id = "5-1-7",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteSpecialDayGroup, Functionality.ConfigurationSpecialDaysRemovedEvent })]
        public void DeleteSpecialDayGroupTest()
        {
            string specialDayGroupToken = null;
            SubscriptionHandler subscription = null;

            var logger = new StringBuilder();

            RunTest(() =>
            {
                // 3.	ONVIF Client retrieves an initial complete list of SpecialDaysGroup (out specialDayGroupsCompleteList1) by following the procedure mentioned in Annex A.5
                var specialDayGroupCompleteList = this.GetSpecialDayGroupListA5();

                SpecialDayGroup specialDayGroup;
                specialDayGroupToken = this.CreateSpecialDayGroupA8(out specialDayGroup);
                Assert(!string.IsNullOrEmpty(specialDayGroupToken),
                       string.Format("CreateSpecialDayGroup returned {0}", null == specialDayGroupToken ? "no token" : "empty token"),
                       "Check token returned by CreateSpecialDayGroup");
                specialDayGroup.token = specialDayGroupToken;

                // 5.	ONVIF Client invokes CreatePullPointSubscription with parameters Filter.TopicExpression := "tns1:Configuration/SpecialDays/Removed"
                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Configuration/SpecialDays/Removed", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                // 6.	The DUT responds with a CreatePullPointSubscriptionResponse message with parameters
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                // 7.	ONVIF Client invokes DeleteSpecialDayGroup with parameters Token := specialDayGroupToken
                this.DeleteSpecialDayGroup(specialDayGroupToken);

                // 9.	Until timeout1 timeout expires repeat the following steps
                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, null, "Schedule.SpecialDayTokenValidation.xq", new List<SpecialDayGroup>() { specialDayGroup });

                // 10.	If timeout1 timeout expires for step 9 without Notification with SpecialDaysToken source simple item equal to specialDayGroupToken, FAIL the test and go to the step 20
                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(messageLimit, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Waiting for notification");

                Assert(ValidateNotifications(notifications, topicInfo, "SpecialDaysToken", specialDayGroupToken, logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received notification");


                // 11.	ONVIF Client retrieves a SpecialDayGroup (in specialDayGroupToken, out specialDayGroupList) by following the procedure mentioned in Annex A.14
                var specialDays = this.GetSpecialDayGroups(new string[] { specialDayGroupToken });
                // 12.	If specialDayGroupList is not empty, FAIL the test and go step 20
                ValidateDeleteConsistency(specialDays, specialDayGroupToken, "GetSpecialDayGroups", "Verifying removed special day group cannot be retrieved");

                // 13.	ONVIF Client retrieves a SpecialDayGroup info (in specialDayGroupToken, out specialDayGroupInfoList) by following the procedure mentioned in Annex A.15
                var specialDayGroupInfos = this.GetSpecialDayGroupInfo(new string[] { specialDayGroupToken });
                // 14.	If specialDayGroupInfoList is not empty, FAIL the test and go step 20
                ValidateDeleteConsistency(specialDayGroupInfos, specialDayGroupToken, "GetSpecialDayGroupInfo", "Verifying removed special day group info info cannot be retrieved");

                // 15.	ONVIF Client retrieves a complete SpecialDayGroup information list (out specialDayGroupInfoCompleteList) by following the procedure mentioned in Annex A.4
                var specialDayGroupInfoList = this.GetSpecialDayGroupInfoListA4();
                ValidateDeleteConsistency(specialDayGroupInfoList, specialDayGroupToken, "GetSpecialDayGroupInfoList", "Verifying list of special day group info", true);

                // 17.	ONVIF Client retrieves a complete list of SpecialDayGroups (out specialDayGroupCompleteList2) by following the procedure mentioned in Annex A.5
                var specialDayGroupList = this.GetSpecialDayGroupListA5(); // specialDayGroupCompleteList
                ValidateDeleteConsistency(specialDayGroupList, specialDayGroupToken, "GetSpecialDayGroupList", "Verifying list of special day groups", true);

                logger.Clear();
                Assert(validateListFromGetFullSpecialDaysList(specialDayGroupList, specialDayGroupCompleteList.Select(e => e.token), logger),
                       logger.ToStringTrimNewLine(),
                       "Validation of received full special day group list");
            },
            () =>
            {
                // 20.	ONVIF Client sends an Unsubscribe to the subscription endpoint s
                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);
            });
        }
        #endregion

        #region 5-1-8 GET SPECIAL DAY GROUPS WITH INVALID TOKEN
        [Test(Name = "GET SPECIAL DAY GROUPS WITH INVALID TOKEN",
            Id = "5-1-8",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSpecialDayGroups })]
        public void GetSpecialDayGroupsWithInvalidTokenTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetSpecialDayGroupInfoListA4();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();
                SpecialDayGroup[] infos = null;
                infos = this.GetSpecialDayGroups(new string[] { invalidToken });

                Assert(infos == null || infos.Length == 0,
                    "List of SpecialDayGroups is not empty",
                    "Check that the DUT returned no SpecialDayGroups");

                if (fullList == null || fullList.Count == 0)
                {
                    return;
                }
                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;
                if (maxLimit >= 2)
                {
                    infos = this.GetSpecialDayGroups(new string[] { fullList[0].token, invalidToken });

                    var expected = fullList[0];

                    this.CheckRequestedInfo(infos, expected.token, "Special Day Groups", D => D.token);
                }
                else
                {
                    LogTestEvent(string.Format("MaxLimit={0}, skip part with sending request with one correct and one incorrect tokens.", maxLimit) + Environment.NewLine);
                }
            });
        }
        #endregion

        #region 5-1-9 GET SPECIAL DAY GROUPS - TOO MANY ITEMS
        [Test(Name = "GET SPECIAL DAY GROUPS - TOO MANY ITEMS",
            Id = "5-1-9",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSpecialDayGroups })]
        public void GetSpecialDayGroupsTooManyItemsTest()
        {
            RunTest(() =>
            {
                var fullList = this.GetSpecialDayGroupInfoListA4();

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                var maxLimit = serviceCapabilities.MaxLimit;

                if (fullList == null || fullList.Count <= maxLimit)
                    return;

                string faultCode = "Sender/InvalidArgs/TooManyItems";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    fullList.RemoveRange((int)maxLimit, fullList.Count - (int)maxLimit - 1);
                    this.GetSpecialDayGroups(fullList.Select(e => e.token).ToArray());
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
        #endregion

        #region 5-1-10 CREATE SPECIAL DAY GROUP - NOT EMPTY SPECIAL DAY GROUP TOKEN
        [Test(Name = "CREATE SPECIAL DAY GROUP - NOT EMPTY SPECIAL DAY GROUP TOKEN",
            Id = "5-1-10",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateSpecialDayGroup })]
        public void CreateSpecialDayGrounNotEmptySpecialDayGroupTokenTest()
        {
            RunTest(() =>
            {

                var uid = this.UIDiCalendarGenerationA6();

                System.DateTime startDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 0, 0, 0);
                System.DateTime endDate = new System.DateTime(System.DateTime.Now.AddDays(1).Year, System.DateTime.Now.AddDays(1).Month, System.DateTime.Now.AddDays(1).Day, 0, 0, 0);

                var specialDayGroupDays = GetSpecialDaysiCalendarValue(startDate, endDate, uid);
                var newSpecialDayGroup = new SpecialDayGroup()
                {
                    token = "SpecialDayGroupToken",
                    Name = "Test SpecialDayGroup Name",
                    Description = "Test SpecialDayGroup Description",
                    Days = specialDayGroupDays
                };

                string faultCode = "Sender/InvalidArgs";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    var scheduleToken = this.CreateSpecialDayGroup(newSpecialDayGroup);
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

        #endregion

        #region 5-1-11 MODIFY SPECIAL DAY GROUP WITH INVALID TOKEN
        [Test(Name = "MODIFY SPECIAL DAY GROUP WITH INVALID TOKEN",
            Id = "5-1-11",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.ModifySpecialDayGroup })]
        public void ModifySpecialDayGroupWithInvalidTokenTest()
        {
            RunTest(() =>
            {

                var fullList = this.GetSpecialDayGroupInfoListA4();

                var uid = this.UIDiCalendarGenerationA6();

                System.DateTime startDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 0, 0, 0);
                System.DateTime endDate = new System.DateTime(System.DateTime.Now.AddDays(1).Year, System.DateTime.Now.AddDays(1).Month, System.DateTime.Now.AddDays(1).Day, 0, 0, 0);

                var specialDayGroupDays = GetSpecialDaysiCalendarValue(startDate, endDate, uid);

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();

                var modifySpecialDayGroup = new SpecialDayGroup()
                {
                    token = invalidToken,
                    Name = "Test SpecialDayGroup Name",
                    Description = "Test SpecialDayGroup Description",
                    Days = specialDayGroupDays
                };

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.ModifySpecialDayGroup(modifySpecialDayGroup);
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

        #region 5-1-12 DELETE SPECIAL DAY GROUP WITH INVALID TOKEN
        [Test(Name = "DELETE SPECIAL DAY GROUP WITH INVALID TOKEN",
            Id = "5-1-12",
            Category = Category.SCHEDULE,
            Path = PATH_SPECIAL_DAY_GROUP,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteSpecialDayGroup })]
        public void DeleteSpecialDayGroupWithInvalidTokenTest()
        {
            RunTest(() =>
            {

                var fullList = this.GetSpecialDayGroupInfoListA4();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.DeleteSpecialDayGroup(invalidToken);
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

        #region 6-1-*

        #region 6-1-1
        [Test(Name = "GET SCHEDULE STATE",
            Id = "6-1-1",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_STATE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.StateReporting },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleState })]
        public void ConfigurationGetScheduleStateTest()
        {
            RunTest(() =>
            {

                var scheduleCompleteList = this.GetFullScheduleListA3();
                if (scheduleCompleteList != null && scheduleCompleteList.Count > 0)
                {
                    foreach (var schedule in scheduleCompleteList)
                    {
                        var scheduleState = this.GetScheduleState(schedule.token);
                        Assert(scheduleState != null,
                            string.Format("There is ScheduleState for token {0} is null", schedule.token),
                            "Checking that ScheduleState not null");

                        ValidateConsistency(scheduleState, schedule);
                    }

                }
            });
        }

        #endregion

        #region 6-1-2 CHANGE SCHEDULE STATE - CHANGE STANDARD
        [Test(Name = "CHANGE SCHEDULE STATE - CHANGE STANDARD",
            Id = "6-1-2",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_STATE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.StateReporting },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleState })]
        public void ChangeScheduleStateChangeStandartTest()
        {
            string scheduleToken = string.Empty;
            SubscriptionHandler subscription = null;
            RunTest(() =>
            {

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();

                var uidICalendar = this.UIDiCalendarGenerationA6();
                int delta = _messageTimeout / 1000 > 10 ? _messageTimeout / 1000 : 10;
                string scheduleiCalendarValue = "";

                CultureInfo culture = new CultureInfo("en-US");
                string[] weekDayNames = culture.DateTimeFormat.ShortestDayNames.Select(e => e.ToUpper()).ToArray();

                if (serviceCapabilities.ExtendedRecurrenceSupported)
                {
                    System.DateTime startDate = System.DateTime.Now.AddSeconds(delta);
                    System.DateTime endDate = new System.DateTime(System.DateTime.Now.AddDays(7).Year, System.DateTime.Now.AddDays(7).Month, System.DateTime.Now.AddDays(7).Day, 18, 0, 0);
                    scheduleiCalendarValue = GetSpecialDaysiCalendarValue(startDate, endDate, uidICalendar, "Test summary", "FREQ=DAILY");
                }
                else
                {
                    System.DateTime startDate = new System.DateTime(1970, System.DateTime.Now.AddSeconds(delta).Month, System.DateTime.Now.AddSeconds(delta).Day, System.DateTime.Now.AddSeconds(delta).Hour, System.DateTime.Now.AddSeconds(delta).Minute, System.DateTime.Now.AddSeconds(delta).Second);
                    System.DateTime endDate = new System.DateTime(1970, System.DateTime.Now.AddDays(7).Month, System.DateTime.Now.AddDays(7).Day, 18, 0, 0);
                    string rrule = string.Format("FREQ=WEEKLY;BYDAY={0},{1}", weekDayNames[(int)System.DateTime.Now.DayOfWeek], weekDayNames[(int)System.DateTime.Now.AddDays(1).DayOfWeek]);
                    scheduleiCalendarValue = GetSpecialDaysiCalendarValue(startDate, endDate, uidICalendar, "Test summary", rrule);
                }

                Onvif.Schedule schedule = null;
                scheduleToken = this.CreateScheduleA13(scheduleiCalendarValue, false, out schedule);
                schedule.token = scheduleToken;

                subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Schedule/State/Active", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                // 9.	The DUT responds with a CreatePullPointSubscriptionResponse message with parameters
                var timeout = _operationDelay / 1000 > delta + 5 ? _operationDelay / 1000 : delta + 5;
                subscription.Subscribe(filter, timeout);

                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.CHANGED, "ScheduleStateChangeEvent.xq", "ScheduleToken", schedule.token, true, false, schedule);

                // 11.	If timeout1 timeout expires for step 10 without Notification with ScheduleToken source simple item equal to scheduleToken, FAIL the test and go to the step 21
                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(1, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that ScheduleState Active notification is received");

                var scheduleState = this.GetScheduleState(scheduleToken);
                Assert(scheduleState.Active,
                    "Received GetScheduleState.Active is false. Expected: true",
                    "Validation of GetScheduleState.Active response");

                Assert(!scheduleState.SpecialDay,
                    "Received GetScheduleState.SpecialDay is true. Expected: false",
                    "Validation of GetScheduleState.SpecialDay response");

            },
            () =>
            {
                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(scheduleToken))
                        this.DeleteSchedule(scheduleToken);
                });

                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);

                this.FinishRestoreSettings();
            });
        }

        #endregion

        #region 6-1-3 CHANGE SCHEDULE STATE - CHANGE SPECIAL DAYS
        [Test(Name = "CHANGE SCHEDULE STATE - CHANGE SPECIAL DAYS",
            Id = "6-1-3",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_STATE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.StateReporting, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleState })]
        public void ChangeScheduleStateChangeSpecialDaysTest()
        {
            string scheduleToken = string.Empty;
            string specialDayToken = string.Empty;
            SubscriptionHandler subscription = null;

            RunTest(() =>
            {

                var serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                bool specialDayState = false;

                var days1 = this.HelperSpecialDayGroupiCalendarGenerationA16(specialDayState);

                SpecialDayGroup specialDayGroup = null;
                specialDayToken = this.CreateSpecialDayGroupA8(out specialDayGroup, days1);
                specialDayGroup.token = specialDayToken;

                var standartICalendar = this.ScheduleiCalendarGenerationA7(serviceCapabilities.ExtendedRecurrenceSupported);

                Onvif.Schedule schedule = null;

                scheduleToken = this.CreateScheduleA13(standartICalendar, true, out schedule, specialDayToken);
                schedule.token = scheduleToken;

                specialDayState = true;

                var days2 = this.HelperSpecialDayGroupiCalendarGenerationA16(specialDayState);

                subscription = new SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };

                var plainTopicInfo = new EventsTopicInfo() { Topic = "tns1:Schedule/State/Active", NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };
                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                const int messageLimit = 1;
                var filter = this.CreateSubscriptionFilter(topicInfo);

                // 9.	The DUT responds with a CreatePullPointSubscriptionResponse message with parameters
                var timeout = _operationDelay / 1000;
                subscription.Subscribe(filter, -1);

                specialDayGroup.Name = "Test SpecialDayGroup Name";
                specialDayGroup.Description = "Test SpecialDayGroup Description";
                specialDayGroup.Days = days2;

                this.ModifySpecialDayGroup(specialDayGroup);

                var pollingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.CHANGED, "ScheduleStateChangeEvent.xq", "ScheduleToken", schedule.token, true, true, schedule);

                // 11.	If timeout1 timeout expires for step 10 without Notification with ScheduleToken source simple item equal to scheduleToken, FAIL the test and go to the step 21
                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(1, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that ScheduleState Active notification is received");

                var scheduleState = this.GetScheduleState(scheduleToken);

                Assert(scheduleState.Active,
                    "Received GetScheduleState.Active is false. Expected: true",
                    "Validation of GetScheduleState.Active response");

                Assert(scheduleState.SpecialDay,
                    "Received GetScheduleState.SpecialDay is false. Expected: true",
                    "Validation of GetScheduleState.SpecialDay response");

            },
            () =>
            {
                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(scheduleToken))
                        this.DeleteSchedule(scheduleToken);
                });

                this.AllowFaultStep(() =>
                {
                    if (!string.IsNullOrEmpty(specialDayToken))
                        this.DeleteSpecialDayGroupA12(specialDayToken);
                });

                if (null != subscription)
                    Events.SubscriptionHandler.Unsubscribe(subscription);

                this.FinishRestoreSettings();
            });
        }

        #endregion

        #region 6-1-4 GET SCHEDULE STATE WITH INVALID TOKEN
        [Test(Name = "GET SCHEDULE STATE WITH INVALID TOKEN",
            Id = "6-1-4",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_STATE,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.StateReporting },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleState })]
        public void DeleteScheduleStateWithInvalidTokenTest()
        {
            RunTest(() =>
            {

                var fullList = this.GetFullScheduleInfoListA1();

                string invalidToken = fullList.Select(e => e.token).GetNonMatchingString();

                string faultCode = "Sender/InvalidArgVal/NotFound";
                string faultExpectedSequence = ConvertToFaultCode(faultCode);

                try
                {
                    this.GetScheduleState(invalidToken);
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

        #region 7-1-*

        #region 7-1-1
        [Test(Name = "SCHEDULE STATE ACTIVE EVENT (PROPERTY EVENT)",
            Id = "7-1-1",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_EVENTS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.StateReporting },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventProperties, Functionality.ScheduleStateActiveEvent })]
        public void ConfigurationScheduleStateActiveEventPropertyEventTest()
        {
            ScheduleServiceCapabilities serviceCapabilities = null;
            SubscriptionHandler subscription = null;
            string specialDayGroupToken = "";
            string scheduleToken = "";
            bool isSpecialDaysSupported = false;
            RunTest(() =>
            {
                var topicSet = this.GetTopicSet();

                var topics = new List<XmlElement>();
                foreach (XmlElement element in topicSet.Any)
                { FindTopics(element, topics); }

                const string eventTopic = "tns1:Schedule/State/Active";

                var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };

                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                var eventNode = GetTopicElement(topics, topicInfo);
                Assert(null != eventNode,
                       string.Format("Event with topic {0} is not supported", eventTopic),
                       string.Format("Check that event with topic {0} is present", eventTopic));


                var eventDescription = new ScheduleEventDescription() { isProperty = true };
                eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "ScheduleToken", "ReferenceToken", EventServiceExtensions.PTNAMESPACE));
                eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "Name", "string", EventServiceExtensions.XSNAMESPACE));
                eventDescription.addItemDescription(new EventItemDescription("Data/SimpleItemDescription", "Active", "boolean", EventServiceExtensions.XSNAMESPACE));
                eventDescription.addItemDescription(new EventItemDescription("Data/SimpleItemDescription", "SpecialDay", "boolean", EventServiceExtensions.XSNAMESPACE));

                var logger = new StringBuilder();
                Assert(checkEventDescription(eventNode, eventDescription, eventNode, logger),
                       logger.ToStringTrimNewLine(),
                       string.Format("Checking description of event with topic {0}", eventTopic));

                var scheduleCompleteList = this.GetFullScheduleListA3();

                if (scheduleCompleteList != null && scheduleCompleteList.Count == 0)
                {
                    Onvif.Schedule schedule;
                    SpecialDayGroup specialDayGroup;
                    serviceCapabilities = (this as IScheduleService).GetServiceCapabilities();
                    if (serviceCapabilities.SpecialDaysSupported)
                    {
                        isSpecialDaysSupported = true;
                        specialDayGroupToken = this.CreateSpecialDayGroupA8(out specialDayGroup);
                    }
                    var scheduleiCalendarValue = this.ScheduleiCalendarGenerationA7(serviceCapabilities.ExtendedRecurrenceSupported);
                    scheduleToken = this.CreateScheduleA13(scheduleiCalendarValue, serviceCapabilities.SpecialDaysSupported, out schedule, specialDayGroupToken);
                    if (!string.IsNullOrEmpty(scheduleToken))
                    {
                        schedule.token = scheduleToken;
                        scheduleCompleteList.Add(schedule);
                    }
                }

                var filter = this.CreateSubscriptionFilter(topicInfo);

                subscription = new Events.SubscriptionHandler(this, false, this.GetEventServiceAddress()) { PullMessagesRequestTimeout = "PT60S" };
                // 8. The DUT responds with a CreatePullPointSubscriptionResponse message with parameters
                var timeout = _operationDelay / 100;
                subscription.Subscribe(filter, -1);

                var pollingCondition = new WaitNotificationsForAllSchedulesPollingCondition(timeout, scheduleCompleteList.Select(e => e.token), topicInfo);
                pollingCondition.Filter = (message) => messageFilterBase(message, topicInfo, OnvifMessage.INITIALIZED, "ScheduleStateActiveEvent.xq", scheduleCompleteList);

                // 12. If timeout1 timeout expires for step 11 without Notification with ScheduleToken source simple item equal to scheduleToken, FAIL the test and go to the step 22

                var notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Assert(subscription.WaitMessages(1, pollingCondition, out notifications),
                       pollingCondition.Reason,
                       "Checking that ScheduleState Active notification is received");

            },
            () =>
            {
                if (!string.IsNullOrEmpty(scheduleToken))
                {
                    this.AllowFaultStep(() =>
                    {
                        if (!string.IsNullOrEmpty(scheduleToken))
                            this.DeleteSchedule(scheduleToken);
                    });

                    this.AllowFaultStep(() =>
                    {
                        if (isSpecialDaysSupported && !string.IsNullOrEmpty(specialDayGroupToken))
                            this.DeleteSpecialDayGroupA12(specialDayGroupToken);
                    });
                }

                // 24.	ONVIF Client sends an Unsubscribe to the subscription endpoint s.
                this.AllowFaultStep(() =>
                {
                    if (null != subscription)
                        Events.SubscriptionHandler.Unsubscribe(subscription);
                });

                this.FinishRestoreSettings();
            });
        }

        #endregion

        #region 7-1-2
        [Test(Name = "SCHEDULE CHANGED EVENT",
            Id = "7-1-2",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_EVENTS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventProperties, Functionality.ConfigurationScheduleChangedEvent })]
        public void ConfigurationScheduleChangedEventTest()
        {
            RunTest(() =>
            {
                var topicSet = this.GetTopicSet();

                var topics = new List<XmlElement>();
                foreach (XmlElement element in topicSet.Any)
                { FindTopics(element, topics); }

                const string eventTopic = "tns1:Configuration/Schedule/Changed";

                var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };

                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                var eventNode = GetTopicElement(topics, topicInfo);
                Assert(null != eventNode,
                       string.Format("Event with topic {0} is not supported", eventTopic),
                       string.Format("Check that event with topic {0} is present", eventTopic));


                var eventDescription = new ScheduleEventDescription() { isProperty = false };
                eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "ScheduleToken", "ReferenceToken", EventServiceExtensions.PTNAMESPACE));

                var logger = new StringBuilder();
                Assert(checkEventDescription(eventNode, eventDescription, eventNode, logger),
                       logger.ToStringTrimNewLine(),
                       string.Format("Checking description of event with topic {0}", eventTopic));


            });
        }

        #endregion

        #region 7-1-3
        [Test(Name = "SCHEDULE REMOVED EVENT",
            Id = "7-1-3",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_EVENTS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventProperties, Functionality.ConfigurationScheduleRemovedEvent })]
        public void ConfigurationScheduleRemovedEventTest()
        {
            RunTest(() =>
            {
                var topicSet = this.GetTopicSet();

                var topics = new List<XmlElement>();
                foreach (XmlElement element in topicSet.Any)
                { FindTopics(element, topics); }

                const string eventTopic = "tns1:Configuration/Schedule/Removed";

                var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };

                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                var eventNode = GetTopicElement(topics, topicInfo);
                Assert(null != eventNode,
                       string.Format("Event with topic {0} is not supported", eventTopic),
                       string.Format("Check that event with topic {0} is present", eventTopic));


                var eventDescription = new ScheduleEventDescription() { isProperty = false };
                eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "ScheduleToken", "ReferenceToken", EventServiceExtensions.PTNAMESPACE));

                var logger = new StringBuilder();
                Assert(checkEventDescription(eventNode, eventDescription, eventNode, logger),
                       logger.ToStringTrimNewLine(),
                       string.Format("Checking description of event with topic {0}", eventTopic));


            });
        }

        #endregion

        #region 7-1-4
        [Test(Name = "SPECIAL DAYS CHANGED EVENT",
            Id = "7-1-4",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_EVENTS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventProperties, Functionality.ConfigurationSpecialDaysChangedEvent })]
        public void ConfigurationSpecialDaysChangedEventTest()
        {
            RunTest(() =>
            {
                var topicSet = this.GetTopicSet();

                var topics = new List<XmlElement>();
                foreach (XmlElement element in topicSet.Any)
                { FindTopics(element, topics); }

                const string eventTopic = "tns1:Configuration/SpecialDays/Changed";

                var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };

                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                var eventNode = GetTopicElement(topics, topicInfo);
                Assert(null != eventNode,
                       string.Format("Event with topic {0} is not supported", eventTopic),
                       string.Format("Check that event with topic {0} is present", eventTopic));


                var eventDescription = new ScheduleEventDescription() { isProperty = false };
                eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "SpecialDaysToken", "ReferenceToken", EventServiceExtensions.PTNAMESPACE));

                var logger = new StringBuilder();
                Assert(checkEventDescription(eventNode, eventDescription, eventNode, logger),
                       logger.ToStringTrimNewLine(),
                       string.Format("Checking description of event with topic {0}", eventTopic));


            });
        }

        #endregion

        #region 7-1-5
        [Test(Name = "SPECIAL DAYS REMOVED EVENT",
            Id = "7-1-5",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_EVENTS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetEventProperties, Functionality.ConfigurationSpecialDaysRemovedEvent })]
        public void ConfigurationSpecialDaysRemovedEventTest()
        {
            RunTest(() =>
            {
                var topicSet = this.GetTopicSet();

                var topics = new List<XmlElement>();
                foreach (XmlElement element in topicSet.Any)
                { FindTopics(element, topics); }

                const string eventTopic = "tns1:Configuration/SpecialDays/Removed";

                var plainTopicInfo = new EventsTopicInfo() { Topic = eventTopic, NamespacesDefinition = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE) };

                var topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);

                var eventNode = GetTopicElement(topics, topicInfo);
                Assert(null != eventNode,
                       string.Format("Event with topic {0} is not supported", eventTopic),
                       string.Format("Check that event with topic {0} is present", eventTopic));


                var eventDescription = new ScheduleEventDescription() { isProperty = false };
                eventDescription.addItemDescription(new EventItemDescription("Source/SimpleItemDescription", "SpecialDaysToken", "ReferenceToken", EventServiceExtensions.PTNAMESPACE));

                var logger = new StringBuilder();
                Assert(checkEventDescription(eventNode, eventDescription, eventNode, logger),
                       logger.ToStringTrimNewLine(),
                       string.Format("Checking description of event with topic {0}", eventTopic));


            });
        }

        #endregion

        #endregion

        #region 8-1-*

        #region 8-1-1 GET SCHEDULE AND GET SPECIAL DAY GROUP INFO LIST CONSISTENCY
        [Test(Name = "GET SCHEDULE AND GET SPECIAL DAY GROUP INFO LIST CONSISTENCY",
            Id = "8-1-1",
            Category = Category.SCHEDULE,
            Path = PATH_SCHEDULE_CONSISTENCY,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.Schedule, Feature.SpecialDays },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetScheduleList, Functionality.GetSpecialDayGroupInfoList })]
        public void GetScheduleAndGetSpecialDayGroupInfoListConsistencyTest()
        {
            RunTest(() =>
            {
                var fullScheduleList = this.GetFullScheduleListA3();
                var fullSpecialDayGroupInfoList = this.GetSpecialDayGroupInfoListA4().Select(e => e.token);

                foreach (var specialDayGroupToken in fullScheduleList.SelectMany(e => e.SpecialDays).Select(e => e.GroupToken))
                {
                    Assert(fullSpecialDayGroupInfoList.Contains(specialDayGroupToken),
                        string.Format("Special Day Group Info List does not contain SpecialDayGroup item with token = '{0}'", specialDayGroupToken),
                        "Checking consistency of returned Schedule list and Special Day Group Info List");
                }
            });
        }

        #endregion

        #endregion

        //#region Message validation

        //bool ValidateScheduleMessage(NotificationMessageHolderType notification,
        //    MessageCheckSettings settings,
        //    StringBuilder logger,
        //    MessageDescription messageInfo)
        //{
        //    XmlElement messageElement = notification.Message;
        //    XmlElement messageRawElement = settings.RawMessageElements[notification];
        //    TopicInfo topicInfo = settings.ExpectedTopic;
        //    XmlNamespaceManager manager = settings.NamespaceManager;

        //    // Init
        //    StringBuilder dump = new StringBuilder();
        //    bool ok = true;

        //    ok = ValidateMessageCommonElements(
        //        notification, messageRawElement, topicInfo,
        //        settings.ExpectedPropertyOperation, manager, dump);

        //    if (messageElement != null)
        //    {
        //        // check message source and data 

        //        // source
        //        bool localOk = ValidateScheduleEventSource(messageElement, manager, settings.Data, dump);
        //        ok = ok && localOk;

        //        XmlElement messageInnerElement = messageRawElement.GetMessageContentElement();
        //        XmlElement dataElement = messageInnerElement.GetMessageData();
        //        localOk = ValidateMessageDataSimpleItems(dataElement, messageInfo, dump);
        //        ok = ok && localOk;
        //    }

        //    if (!ok)
        //    {
        //        logger.Append(dump.ToString());
        //    }
        //    return ok;
        //}

        //bool ValidateScheduleEventSource(XmlElement messageElement,
        //    XmlNamespaceManager manager,
        //    object data,
        //    StringBuilder logger)
        //{
        //    bool ok = true;

        //    List<Onvif.Schedule> infos = data as List<Onvif.Schedule>;
        //    string token = data as string;
        //    EntityListInfo<Onvif.Schedule> entityInfo = data as EntityListInfo<Onvif.Schedule>;

        //    XmlElement sourceElement = messageElement.GetMessageSource();
        //    if (sourceElement == null)
        //    {
        //        logger.AppendLine("   Message Source element is missing");
        //        ok = false;
        //    }
        //    else
        //    {
        //        bool success = false;
        //        string err;

        //        Dictionary<string, string> sourceSimpleItems = messageElement.GetMessageSourceSimpleItems(out success, out err);
        //        if (!success)
        //        {
        //            ok = false;
        //            logger.AppendLine("   " + err);
        //        }
        //        else
        //        {
        //            if (sourceSimpleItems.ContainsKey("ScheduleToken"))
        //            {
        //                string value = sourceSimpleItems["ScheduleToken"];
        //                // check value
        //                StringBuilder error = new StringBuilder();

        //                if (infos != null)
        //                {
        //                    ScheduleInfo found = infos.Where(I => I.token == value).FirstOrDefault();
        //                    if (found == null)
        //                    {
        //                        ok = false;
        //                        logger.Append(string.Format("   Schedule with token '{0}' not found", value));
        //                    }
        //                }
        //                else if (entityInfo != null)
        //                {
        //                    ScheduleInfo found = entityInfo.FullList.Where(I => I.token == value).FirstOrDefault();
        //                    if (found == null)
        //                    {
        //                        ok = false;
        //                        logger.Append(string.Format("   Schedule with token '{0}' not found", value));
        //                    }
        //                    else
        //                    {
        //                        found = entityInfo.FilteredList.Where(I => I.token == value).FirstOrDefault();
        //                        if (found == null)
        //                        {
        //                            ok = false;
        //                            logger.Append(string.Format("   Schedule with token '{0}' does not have required capabilities", value));
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (value != token)
        //                    {
        //                        ok = false;
        //                        logger.Append(string.Format("   Token is incorrect. Expected '{0}', actual '{1}'", token, value));
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                logger.AppendLine("   'Schedule' SimpleItem is missing in Source");
        //                ok = false;
        //            }
        //        }
        //    }

        //    return ok;
        //}


        //public class WaitNotificationsForAllSchedulePollingCondition : SubscriptionHandler.PollingConditionBase
        //{
        //    public WaitNotificationsForAllSchedulePollingCondition(int timeout, IEnumerable<string> waitingNotificationsFor)
        //        : base(timeout)
        //    {
        //        m_WaitingNotificationsFor = new HashSet<string>(waitingNotificationsFor);
        //    }

        //    public override bool StopPulling
        //    {
        //        get { return !m_WaitingNotificationsFor.Any(); }
        //    }

        //    public override string Reason
        //    {
        //        get
        //        {
        //            if (m_WaitingNotificationsFor.Any())
        //            {
        //                var log = new StringBuilder();
        //                log.AppendLine("Not all required notifications are received");
        //                var tokens = string.Join(", ", m_WaitingNotificationsFor.Select(e => string.Format("'{0}'", e)).ToArray()).Trim(new[] { ' ', ',' });
        //                if (m_WaitingNotificationsFor.Count() > 1)
        //                    log.AppendFormat("No notifications for access points with tokens: {0}", tokens);
        //                else
        //                    log.AppendFormat("No notification for access point with token: {0}", tokens);

        //                return log.ToString();
        //            }
        //            else
        //                return "Notifications for all access points are received";
        //        }
        //    }

        //    public override void Update(Dictionary<NotificationMessageHolderType, XmlElement> messages)
        //    {
        //        if (null != messages)
        //            foreach (var msg in messages.Keys)
        //            {
        //                string accessPointToken = null;
        //                if (null != msg.Message.GetMessageSourceSimpleItems()
        //                    && msg.Message.GetMessageSourceSimpleItems().ContainsKey("AccessPointToken"))
        //                    accessPointToken = msg.Message.GetMessageSourceSimpleItems()["AccessPointToken"];

        //                if (null != accessPointToken)
        //                    m_WaitingNotificationsFor.Remove(accessPointToken);
        //            }
        //    }

        //    private readonly HashSet<string> m_WaitingNotificationsFor;
        //}

        //bool ValidateAccessPointEnabledMessage(NotificationMessageHolderType notification,
        //    MessageCheckSettings settings,
        //    StringBuilder logger)
        //{
        //    MessageDescription messageInfo = new MessageDescription();
        //    messageInfo.IsProperty = true;
        //    messageInfo.AddSimpleItem("Active", "boolean", XSNAMESPACE);
        //    messageInfo.AddSimpleItem("SpecialDays", "boolean", XSNAMESPACE);
        //    //[11.06.2013] AKS: fix for 19522
        //    //messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE);

        //    return ValidateScheduleMessage(notification, settings, logger, messageInfo);
        //}

        //#endregion
    }
}