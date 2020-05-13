using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using System.Xml;
using TestTool.Proxies.Event;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.TestCases.TestSuites.Events;

using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using EndpointReferenceType = TestTool.Proxies.Event.EndpointReferenceType;

namespace TestTool.Tests.TestCases.TestSuites.PACS
{
    partial class DoorControlEventsTestSuite
    {
        private const string PATH_PROPERTYEVENTS = "Door Control\\Property Events";

        public class WaitNotificationsForAllDoorsPollingCondition : SubscriptionHandler.PollingConditionBase
        {
            public WaitNotificationsForAllDoorsPollingCondition(int timeout, IEnumerable<string> waitingNotificationsFor)
                : base(timeout)
            {
                m_WaitingNotificationsFor = new HashSet<string>(waitingNotificationsFor);
            }

            public override bool StopPulling
            {
                get { return !m_WaitingNotificationsFor.Any(); }
            }

            public override string Reason
            {
                get
                {
                    if (m_WaitingNotificationsFor.Any())
                    {
                        var log = new StringBuilder();
                        log.AppendLine("Not all required notifications are received");
                        var tokens = string.Join(", ", m_WaitingNotificationsFor.Select(e => string.Format("'{0}'", e)).ToArray()).Trim(new[] { ' ', ',' });
                        if (m_WaitingNotificationsFor.Count() > 1)
                            log.AppendFormat("No notifications for the doors with tokens: {0}", tokens);
                        else
                            log.AppendFormat("No notification for the door with token: {0}", tokens);

                        return log.ToString();
                    }
                    else
                        return "Notifications for all doors are received";
                }
            }

            public override void Update(Dictionary<NotificationMessageHolderType, XmlElement> messages)
            {
                if (null != messages)
                    foreach (var msg in messages.Keys)
                    {
                        string doorToken = null;
                        if (null != msg.Message.GetMessageSourceSimpleItems()
                            && msg.Message.GetMessageSourceSimpleItems().ContainsKey("DoorToken"))
                            doorToken = msg.Message.GetMessageSourceSimpleItems()["DoorToken"];

                        if (null != doorToken)
                            m_WaitingNotificationsFor.Remove(doorToken);
                    }
            }

            private readonly HashSet<string> m_WaitingNotificationsFor;
        }


        #region Test templates

        // todo unify with accesscontrol
        NotificationMessageHolderType[] MyReceiveMessagesNotify(
            Action action,
            int timeout,
            TestTool.Proxies.Event.FilterType filter,
            XmlDocument doc
            )
        {
            NotificationMessageHolderType[] NotificationMessage;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;
            EndpointReferenceType subscriptionReference = null;
            Notify notify = null;
            try
            {
                subscriptionReference =
                    ReceiveMessages(filter,
                    timeout,
                    action,
                    doc,
                    out notify,
                    out subscribeStarted);
                NotificationMessage = notify.NotificationMessage;
            }
            finally
            {
                Operator.HideMessage();
                ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
            }
            return NotificationMessage;
        }
        //[04.03.2013] AKS: added parameter message limit and functionality to check if number of notifications in response to PullMessages command
        //exceeds messageLimit
        protected NotificationMessageHolderType[] ReceiveMessagesPullPointFirstMessage(
           Action action,
           TestTool.Proxies.Event.FilterType filter,
           int messageLimit,
           XmlDocument doc)
        {
            EndpointReferenceType subscriptionReference = null;

            int timeout = _eventSubscriptionTimeout;
            int actualTerminationTime = timeout;
            System.DateTime subscribeStarted = System.DateTime.Now;
            NotificationMessageHolderType[] NotificationMessage = null;
            System.DateTime pullingDeadline = System.DateTime.Now.AddSeconds(_operationDelay / 1000.0);
            System.DateTime TerminationExpectedTime = System.DateTime.Now.AddSeconds(timeout);
            try
            {
                actualTerminationTime = timeout;
                subscriptionReference = CreateStandardSubscription(filter, ref actualTerminationTime);

                Assert(null != subscriptionReference,
                    "Can't create pullpoint subscription",
                    "Check subscription result");
                /*if (subscriptionReference == null)
                {
                    goto ready;
                }*/
                CreatePullPointSubscriptionClient(subscriptionReference);

                string dump;
                //Operator.ShowMessage(message);
                action();
                int terminationTimeSeconds = 60;
                do
                {
                    DoRenewBeforePull(ref terminationTimeSeconds, ref TerminationExpectedTime);

                    NotificationMessage = GetMessages(subscriptionReference, false, true, false, 1, out dump);
                    if (System.DateTime.Now > pullingDeadline)
                    {
                        break;
                    }
                    if (NotificationMessage == null)
                    {
                        break;
                    }
                }
                while ((NotificationMessage != null) && (NotificationMessage.Length == 0));

                if ((NotificationMessage != null) && (NotificationMessage.Length == 0))
                {
                    NotificationMessage = null;
                }
                if (NotificationMessage != null)
                {
                    doc.LoadXml(dump);
                }
            }
            catch (Exception exc)
            {
                StepFailed(exc);
            }
            finally
            {
                Operator.HideMessage();
            }
            ready:
            try
            {
                Assert(null != NotificationMessage && 0 != NotificationMessage.Length,
                       "No valid notification messages received",
                       "Check that DUT sent valid notification messages");

                //[04.03.2013] AKS: check if number of notifications in response to PullMessages command exceeds messageLimit
                Assert(null != NotificationMessage && messageLimit >= NotificationMessage.Length,
                       "Maximum number of messages exceeded",
                       string.Format("Check that DUT sent not more than {0} message(s)", messageLimit));
            }
            finally
            {
                ReleaseSubscription(subscribeStarted, subscriptionReference, TerminationExpectedTime);
            }
            return NotificationMessage;
        }


        void CommonDoorPropertyEventTestBis(Func<DoorInfo, bool> doorCapabilitiesTest,
            TopicInfo topicInfo,
            Action<XmlElement, TopicInfo> validateTopic,
            ValidateMessageFunction validateMessageFunction,
            Func<DoorState, string> stateValueSelector,
            string stateSimpleItemName)
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay / 1000;

            RunTest(
                () =>
                {
                    //3.	Get complete list of doors from the DUT (see Annex A.1).
                    //4.	Check that there is at least one Door with [DOOR CAPABILITIES TEST]= “true”. 
                    // Otherwise skip other steps and go to the next test.
                    List<DoorInfo> fullList = GetDoorInfoList();

                    List<DoorInfo> doors = fullList.Where(D => doorCapabilitiesTest(D)).ToList();

                    Assert(doors.Any(),
                           "No Doors with required Capabilities found, exit the test.",
                           "Check there is appropriate door for test");

                    //5.	ONVIF Client will invoke GetEventPropertiesRequest message to retrieve 
                    // all events supported by the DUT.
                    //6.	Verify the GetEventPropertiesResponse message from the DUT.
                    //7.	Check if there is an event with Topic [TOPIC]. If there is no event with 
                    // such Topic skip other steps, fail the test and go to the next test.
                    XmlElement topicElement = GetTopicElement(topicInfo);

                    // if only one topic should be checked
                    //ValidateTopicXml(topicElement); 

                    Assert(topicElement != null,
                           string.Format("Topic {0} not supported", topicInfo.GetDescription()),
                           "Check that the event topic is supported");

                    //8.	Check that this event is a Property event (MessageDescription.IsProperty="true").
                    //9.	Check that this event contains Source.SimpleItemDescription item with 
                    // Name="DoorToken" and Type="pt:ReferenceToken".
                    //10.	Check that this event contains Data.SimpleItemDescription item with Name=[NAME] 
                    // and Type=[TYPE].
                    XmlElement messageDescription = topicElement.GetMessageDescription();
                    validateTopic(messageDescription, topicInfo);

                    FilterInfo filter = CreateFilter(topicInfo, messageDescription);


                    //11.	ONVIF Client will invoke SubscribeRequest message with tns1:DoorControl/DoorMode Topic as Filter and an InitialTerminationTime of 60s to ensure that the SubscriptionManager is deleted after one minute.
                    //12.	Verify that the DUT sends a SubscribeResponse message.
                    //13.	Verify that DUT sends Notify message(s)
                    //14.	Verify received Notify messages (correct value for UTC time, TopicExpression and wsnt:Message).
                    //15.	Verify that TopicExpression is equal to tns1:DoorControl/DoorMode for all received Notify messages.
                    //16.	Verify that each notification contains Source.SimpleItem item with Name="DoorToken" and Value is equal to one of existing Door Tokens (e.g. complete list of doors contains Door with the same token). Verify that there are Notification messages for each Door.
                    //17.	Verify that each notification contains Data.SimpleItem item with Name="DoorMode" and Value with type is equal to tdc:DoorMode.
                    //18.	Verify that Notify PropertyOperation="Initialized".
                    bool UseNotify = UseNotifyToGetEvents;

                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = null;
                    CurrentSubsciption = null;
                    bool Timeout = false;
                    try
                    {
//                        Handler = new SubscriptionHandler(this, UseNotify);
//                        Handler.SetPolicy(new EventsVerifyPolicy(UseNotify));
//                        EnsureServiceAddressNotEmpty();
//                        Handler.SetAddress(_eventServiceAddress);
//                        Handler.Subscribe(filter.Filter, actualTerminationTime);
//                        int MessagesCount = 0;
//
//                        Timeout = !Handler.WaitMessages(
//                            timeout, 
//                            (n) => { return ++MessagesCount >= doors.Count; },
//                            SubscriptionHandler.WaitCondition.WC_AllExit,
//                            out notifications);
                        CurrentSubsciption = new SubscriptionHandler(this, UseNotify, GetEventServiceAddress());
                        CurrentSubsciption.Subscribe(filter.Filter, actualTerminationTime);

                        var pullingCondition = new WaitNotificationsForAllDoorsPollingCondition(timeout, doors.Select(e => e.token));

                        Timeout = !CurrentSubsciption.WaitMessages(1, pullingCondition, out notifications);
                    }
                    finally
                    {
                        UnsubscribeCurrentSubsciption();
                    }
                    Assert(null != notifications && notifications.Any(),
                           string.Format("No notification messages are received.{0}WARNING: may be Operation delay is too low", Environment.NewLine),
                           "Check that DUT sent any notification messages");

                    if (Timeout && notifications.Count() < doors.Count())
                    {
                        LogTestEvent(string.Format("Not enough notification messages are received:{0}"
                                                   + "received {1} notifications while there exist {2} doors.{0}"
                                                   + "WARNING: may be Operation delay is too low{0}", 
                                                   Environment.NewLine, notifications.Count(), doors.Count()));
                    }

                    EntityListInfo<DoorInfo> data = new EntityListInfo<DoorInfo>();
                    data.FullList = fullList;
                    data.FilteredList = doors;
                    ValidateMessages(notifications, topicInfo, OnvifMessage.INITIALIZED, data, validateMessageFunction);

                    Dictionary<string, NotificationMessageHolderType> doorsMessages = new Dictionary<string, NotificationMessageHolderType>();
                    ValidateMessagesSet(notifications.Keys, doors, doorsMessages);
                     
                    //19.	ONVIF Client will invoke GetDoorStateRequest message for each Door 
                    // with corresponding tokens.
                    //20.	Verify the GetDoorStateResponse messages from the DUT. Verify that 
                    // Data.SimpleItem item with Name=[SIMPLEITEMNAME] from Notification message has the 
                    // same value with [ELEMENT SELECTION] elements from corresponding GetDoorStateResponse 
                    // messages for each Door.
                    foreach (string doorToken in doorsMessages.Keys)
                    {
                        DoorState state = GetDoorState(doorToken);

                        string expectedState = stateValueSelector(state);

                        XmlElement dataElement = doorsMessages[doorToken].Message.GetMessageData();

                        Dictionary<string, string> dataSimpleItems = dataElement.GetMessageElementSimpleItems();

                        string notificationState = dataSimpleItems[stateSimpleItemName];

                        Assert(expectedState == notificationState,
                            string.Format("State is different ({0} in GetDoorStateResponse, {1} in Notification)", expectedState, notificationState),
                            "Check that state is the same in Notification and in GetDoorStateResponse");
                    }
                },
                () =>
                {
                });       
        }

        void CommonDoorPropertyEventStateChangeTestBis(Func<DoorInfo, bool> doorCapabilitiesTest,
                                                       TopicInfo topicInfo,
                                                       ValidateMessageFunction validateMessageFunction)
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay / 1000;

            RunTest(
                () =>
                {

                    //3.	Get complete list of doors from the DUT (see Annex A.1).
                    List<DoorInfo> fullDoorInfosList = GetDoorInfoList();

                    //4.	Check that there is at least one Door with Capabilities.[CAPABILITIES FOR THE TEST]= “true”. 
                    // Otherwise skip other steps and go to the next test.
                    List<DoorInfo> doorsList = null;
                    if (doorCapabilitiesTest != null)
                        doorsList = fullDoorInfosList.Where(A => doorCapabilitiesTest(A)).ToList();
                    else
                        doorsList = fullDoorInfosList;

                    Assert(doorsList.Any(),
                           "No Doors with required Capabilities found, exit the test.",
                           "Check there is appropriate door for test");

                    //5.	ONVIF Client will select one random Door (token = Token1) with 
                    // Capabilities.DoorMonitor= “true”.
                    // ToDo: may be change it to really random selection
                    DoorInfo selectedDoor = doorsList[0];

                    // filter for current test
                    Proxies.Event.FilterType filter = CreateSubscriptionFilter(topicInfo);

                    //6.	ONVIF Client will invoke SubscribeRequest message with tns1:DoorControl/DoorPhysicalState Topic as Filter and an InitialTerminationTime of 60s to ensure that the SubscriptionManager is deleted after one minute.
                    //7.	Verify that the DUT sends a SubscribeResponse message.
                    //8.	Test Operator will invoke change of DoorPhysicalState property for Door with token = Token1.
                    //9.	Verify that DUT sends Notify message.
                    string message = string.Format("{0} event is expected \r\n for the Door with token={{0}}", 
                                                   topicInfo.GetDescription());
                    
                    DoorSelectionData data = new DoorSelectionData
                                             {
                                                 SelectedToken = selectedDoor.token,
                                                 MessageTemplate = message,
                                                 Doors = doorsList.Select(D => new DoorSelectionData.DoorShortInfo() {Token = D.token, Name = D.Name}).ToList()
                                             };

                    string Message = string.Format("{0} event is expected \r\n for the Door with token = '{1}'",
                                                   topicInfo.GetDescription(), selectedDoor.token);

                    bool UseNotify = UseNotifyToGetEvents;

                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = null;
                    CurrentSubsciption = null;
                    try
                    {
                        CurrentSubsciption = new SubscriptionHandler(this, UseNotify, GetEventServiceAddress());
                        CurrentSubsciption.Subscribe(filter, actualTerminationTime);

                        Operator.ShowMessage(Message);
//                        Handler.WaitMessages(
//                            timeout,
//                            (n) => CheckMessagePropertyOperation(n, OnvifMessage.CHANGED),
//                            SubscriptionHandler.WaitCondition.WC_ALL,
//                            out notifications);
                        var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                                               { Filter = msg => CheckMessagePropertyOperation(msg, OnvifMessage.CHANGED) };
                        CurrentSubsciption.WaitMessages(1, pullingCondition, out notifications);
                    }
                    finally
                    {
                        Operator.HideMessage();
                        UnsubscribeCurrentSubsciption();
                    }

                    Assert(null != notifications && notifications.Any(),
                           string.Format("Message with PropertyOperation='Changed' for the door with token='{0}' has not been received.{1}WARNING: may be Operation delay is too low", 
                                         data.SelectedToken, Environment.NewLine),
                           "Check that the message for selected door has been received");


                    //10.	Verify received Notify messages (correct value for UTC time, TopicExpression and 
                    // wsnt:Message).
                    //11.	Verify that TopicExpression is equal to [TOPIC] for 
                    // received Notify message.
                    //12.	Verify that notification contains Source.SimpleItem item with Name="DoorToken" 
                    // and Value= “Token1”.
                    //13.	Verify that notification contains Data.SimpleItem item with Name=[ITEMNAME] and 
                    // Value with type is equal to [TYPE].

                    BeginStep("Validate messages");

                    XmlNamespaceManager manager = CreateNamespaceManager(notifications.First().Value.OwnerDocument);

                    StringBuilder logger = new StringBuilder();
                    bool ok = true;

                    MessageCheckSettings settings = new MessageCheckSettings();
                    settings.Data = data.SelectedToken;
                    settings.ExpectedTopic = topicInfo;
                    settings.RawMessageElements = notifications;
                    settings.NamespaceManager = manager;

                    foreach (NotificationMessageHolderType m in notifications.Keys)
                    {
                        bool local = validateMessageFunction(m, settings, logger);
                        ok = ok && local;
                    }
                    if (!ok)
                    {
                        throw new AssertException(logger.ToStringTrimNewLine());
                    }

                    StepPassed();

                },
                () =>
                {
                    Operator.HideMessage();
                    //Operator.HideDoorSelectionMessage();
                });
        }

        #endregion

        #region Tests 
        [Test(Name = "DOOR CONTROL – DOOR MODE EVENT",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.01",
            Id = "6-1-1",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoorModeEvent })]
        public void DoorControlDoorModeEventTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest = D => true;
            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            // Topic validation method
            Action<XmlElement, TopicInfo> validateTopic = ValidateDoorModeTopic;
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorModeMessage;
            // State selector
            Func<DoorState, string> stateValueSelector = DS => DS.DoorMode.ToString();
            // simple item name
            string stateSimpleItemName = "State";

            CommonDoorPropertyEventTestBis(doorCapabilitiesTest,
                topicInfo,
                validateTopic,
                validateMessageFunction,
                stateValueSelector,
                stateSimpleItemName);

        }
        [Test(Name = "DOOR CONTROL – DOOR PHYSICAL STATE EVENT",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.02",
            Id = "6-1-2",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoorMonitor },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoorPhysicalStateEvent })]
        public void DoorControlDoorPhysicalStateEventTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest = 
                D => D.Capabilities != null && 
                    D.Capabilities.DoorMonitorSpecified && 
                    D.Capabilities.DoorMonitor;

            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "DoorPhysicalState" });
            // Topic validation method
            Action<XmlElement, TopicInfo> validateTopic = ValidateDoorPhysicalStateTopic;
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorPhysicalStateMessage;
            // State selector
            Func<DoorState, string> stateValueSelector = 
                DS => DS.DoorPhysicalStateSpecified ? DS.DoorPhysicalState.ToString() : "NOT SPECIFIED";
            // simple item name
            string stateSimpleItemName = "State";

            CommonDoorPropertyEventTestBis(doorCapabilitiesTest,
                topicInfo,
                validateTopic,
                validateMessageFunction,
                stateValueSelector,
                stateSimpleItemName);

        }

        [Test(Name = "DOOR CONTROL – DOOR PHYSICAL STATE EVENT STATE CHANGE",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.03",
            Id = "6-1-3",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoorMonitor },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoorPhysicalStateEvent })]
        public void DoorControlDoorPhysicalStateEventStateChangeTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                D => D.Capabilities != null &&
                    D.Capabilities.DoorMonitorSpecified &&
                    D.Capabilities.DoorMonitor;

            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "DoorPhysicalState" });
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorPhysicalStateMessage;

            CommonDoorPropertyEventStateChangeTestBis(doorCapabilitiesTest,
                topicInfo,
                validateMessageFunction);
        }
        
        [Test(Name = "DOOR CONTROL – DOUBLE LOCK PHYSICAL STATE EVENT",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.04",
            Id = "6-1-4",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoubleLockMonitor },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoubleLockPhysicalStateEvent })]
        public void DoorControlDoubleLockPhysicalStateEventTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                D => D.Capabilities != null &&
                    D.Capabilities.DoubleLockMonitorSpecified &&
                    D.Capabilities.DoubleLockMonitor;

            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "DoubleLockPhysicalState" });
            // Topic validation method
            Action<XmlElement, TopicInfo> validateTopic = ValidateDoorDoubleLockPhysicalStateTopic;
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorDoubleLockMessage;
            // State selector
            Func<DoorState, string> stateValueSelector =
                DS => DS.DoubleLockPhysicalStateSpecified ? DS.DoubleLockPhysicalState.ToString() : "NOT SPECIFIED";
            // simple item name
            string stateSimpleItemName = "State";

            CommonDoorPropertyEventTestBis(doorCapabilitiesTest,
                topicInfo,
                validateTopic,
                validateMessageFunction,
                stateValueSelector,
                stateSimpleItemName);

        }

        [Test(Name = "DOOR CONTROL – DOUBLE LOCK PHYSICAL STATE EVENT STATE CHANGE",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.05",
            Id = "6-1-5",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoubleLockMonitor },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoubleLockPhysicalStateEvent })]
        public void DoorControlDoubleLockPhysicalStateEventStateChangeTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                D => D.Capabilities != null &&
                    D.Capabilities.DoubleLockMonitorSpecified &&
                    D.Capabilities.DoubleLockMonitor;

            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "DoubleLockPhysicalState" });
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorDoubleLockMessage;

            CommonDoorPropertyEventStateChangeTestBis(doorCapabilitiesTest,
                topicInfo,
                validateMessageFunction);
        }
        
        [Test(Name = "DOOR CONTROL – LOCK PHYSICAL STATE EVENT",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.06",
            Id = "6-1-6",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.LockMonitor },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.LockPhysicalStateEvent })]
        public void DoorControlLockPhysicalStateEventTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                D => D.Capabilities != null &&
                    D.Capabilities.LockMonitorSpecified &&
                    D.Capabilities.LockMonitor;

            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "LockPhysicalState" });
            // Topic validation method
            Action<XmlElement, TopicInfo> validateTopic = ValidateDoorLockPhysicalStateTopic;
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorLockPhysicalStateMessage; ;
            // State selector
            Func<DoorState, string> stateValueSelector =
                DS => DS.LockPhysicalStateSpecified ? DS.LockPhysicalState.ToString() : "NOT SPECIFIED";
            // simple item name
            string stateSimpleItemName = "State";

            CommonDoorPropertyEventTestBis(doorCapabilitiesTest,
                topicInfo,
                validateTopic,
                validateMessageFunction,
                stateValueSelector,
                stateSimpleItemName);

        }


        [Test(Name = "DOOR CONTROL – LOCK PHYSICAL STATE EVENT CHANGE STATE",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.07",
            Id = "6-1-7",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.LockMonitor },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.LockPhysicalStateEvent })]
        public void DoorControlLockPhysicalStateEventChangeStateTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                D => D.Capabilities != null &&
                    D.Capabilities.LockMonitorSpecified &&
                    D.Capabilities.LockMonitor;

            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "LockPhysicalState" });
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorLockPhysicalStateMessage; ;
            // State selector

            CommonDoorPropertyEventStateChangeTestBis(doorCapabilitiesTest,
                topicInfo,
                validateMessageFunction);
        }

        [Test(Name = "DOOR CONTROL – TAMPER EVENT",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.08",
            Id = "6-1-8",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoorTamper },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoorTamperEvent })]
        public void DoorControlDoorTamperEventTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                D => D.Capabilities != null &&
                    D.Capabilities.TamperSpecified &&
                    D.Capabilities.Tamper;

            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "DoorTamper" });
            // Topic validation method
            Action<XmlElement, TopicInfo> validateTopic = ValidateDoorTamperTopic;
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorTamperMessage;
            // State selector
            Func<DoorState, string> stateValueSelector = DS => DS.Tamper != null ? DS.Tamper.State.ToString() : "Not specified";
            // simple item name
            string stateSimpleItemName = "State";

            CommonDoorPropertyEventTestBis(doorCapabilitiesTest,
                topicInfo,
                validateTopic,
                validateMessageFunction,
                stateValueSelector,
                stateSimpleItemName);

        }

        [Test(Name = "DOOR CONTROL – TAMPER EVENT STATE CHANGE",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.09",
            Id = "6-1-9",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoorTamper },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoorTamperEvent })]
        public void DoorControlDoorTamperEventStateChangeTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                D => D.Capabilities != null &&
                    D.Capabilities.TamperSpecified &&
                    D.Capabilities.Tamper;

            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "DoorTamper" });
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorTamperMessage;
            
            CommonDoorPropertyEventStateChangeTestBis(doorCapabilitiesTest,
                topicInfo,
                validateMessageFunction);
        }
        
        [Test(Name = "DOOR CONTROL – ALARM EVENT",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.10",
            Id = "6-1-10",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoorAlarm },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoorAlarmEvent })]
        public void DoorControlDoorAlarmEventTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                D => D.Capabilities != null &&
                    D.Capabilities.AlarmSpecified  &&
                    D.Capabilities.Alarm;

            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "DoorAlarm" });
            // Topic validation method
            Action<XmlElement, TopicInfo> validateTopic = ValidateDoorAlarmTopic ;
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorAlarmMessage;
            // State selector
            Func<DoorState, string> stateValueSelector = DS => DS.AlarmSpecified ? DS.Alarm.ToString() : "NOT SPECIFIED";
            // simple item name
            string stateSimpleItemName = "State";

            CommonDoorPropertyEventTestBis(doorCapabilitiesTest,
                topicInfo,
                validateTopic,
                validateMessageFunction,
                stateValueSelector,
                stateSimpleItemName);
        }

        [Test(Name = "DOOR CONTROL – ALARM EVENT STATE CHANGE",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.11",
            Id = "6-1-11",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoorAlarm },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoorAlarmEvent })]
        public void DoorControlDoorAlarmEventStateChangeTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                D => D.Capabilities != null &&
                    D.Capabilities.AlarmSpecified &&
                    D.Capabilities.Alarm;

            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "DoorAlarm" });
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorAlarmMessage;

            CommonDoorPropertyEventStateChangeTestBis(doorCapabilitiesTest,
                topicInfo,
                validateMessageFunction);
        }
        
       [Test(Name = "DOOR CONTROL – FAULT EVENT",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.12",
            Id = "6-1-12",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoorFault },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoorFaultEvent })]
        public void DoorControlDoorFaultEventTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                D => D.Capabilities != null &&
                    D.Capabilities.FaultSpecified &&
                    D.Capabilities.Fault;

            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "DoorFault" });
            // Topic validation method
            Action<XmlElement, TopicInfo> validateTopic = ValidateDoorFaultTopic;
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorFaultMessage;
            // State selector
            Func<DoorState, string> stateValueSelector = DS => DS.Fault != null ? DS.Fault.State.ToString() : "NOT SPECIFIED";
            // simple item name
            string stateSimpleItemName = "State";

            CommonDoorPropertyEventTestBis(doorCapabilitiesTest,
                topicInfo,
                validateTopic,
                validateMessageFunction,
                stateValueSelector,
                stateSimpleItemName);
        }

        [Test(Name = "DOOR CONTROL – FAULT EVENT STATE CHANGE",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.13",
            Id = "6-1-13",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoorFault },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoorFaultEvent })]
        public void DoorControlDoorFaultEventStateChangeTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                D => D.Capabilities != null &&
                    D.Capabilities.FaultSpecified &&
                    D.Capabilities.Fault;

            // Topic for current test
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "State", "DoorFault" });
            // Message validation method
            ValidateMessageFunction validateMessageFunction = ValidateDoorFaultMessage;

            CommonDoorPropertyEventStateChangeTestBis(doorCapabilitiesTest,
                topicInfo,
                validateMessageFunction);
        }


        
        #endregion


        #region Validation

        void ValidateMessagesSet(IEnumerable<NotificationMessageHolderType> messages,
            IEnumerable<DoorInfo> doors,
            Dictionary<string, NotificationMessageHolderType> doorsMessages)
        {
            ValidateMessagesSet(messages,
                doors,
                D => D.token,
                "Door",
                DOORTOKENSIMPLEITEM,
                doorsMessages);

        }


        #endregion

        #region Steps

        protected DoorState GetDoorState(string token)
        {
            DoorControlPortClient client = DoorControlPortClient;

            DoorState state = null;
            RunStep(
                () => { state = client.GetDoorState(token); },
                string.Format("Get Door state [token='{0}']", token));
            DoRequestDelay();
            return state;
        }


        #endregion


    }
}
