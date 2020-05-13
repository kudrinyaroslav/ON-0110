using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using System.Xml;
using TestTool.Proxies.Event;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Data;

namespace TestTool.Tests.TestCases.TestSuites.PACS
{
    partial class DoorControlEventsTestSuite
    {
        private const string PATH_PROPERTYEVENTS = "Door Control\\Property Events";

        #region Test templates

        void CommonDoorPropertyEventTest(Func<DoorInfo, bool> doorCapabilitiesTest,
            TopicInfo topicInfo,
            Action<XmlElement, TopicInfo> validateTopic,
            ValidateMessageFunction validateMessageFunction,
            Func<DoorState, string> stateValueSelector,
            string stateSimpleItemName)
        {
            EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = 60;

            RunTest(
                () =>
                {
                    //3.	Get complete list of doors from the DUT (see Annex A.1).
                    //4.	Check that there is at least one Door with [DOOR CAPABILITIES TEST]= “true”. 
                    // Otherwise skip other steps and go to the next test.
                    List<DoorInfo> fullList = GetDoorInfoList();

                    List<DoorInfo> doors = fullList.Where(D => doorCapabilitiesTest(D)).ToList();

                    if (doors.Count == 0)
                    {
                        LogTestEvent("No Doors with required Capabilities found, exit the test" + Environment.NewLine);
                        return;
                    }

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

                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();

                    //11.	ONVIF Client will invoke SubscribeRequest message with tns1:DoorControl/DoorMode Topic as Filter and an InitialTerminationTime of 60s to ensure that the SubscriptionManager is deleted after one minute.
                    //12.	Verify that the DUT sends a SubscribeResponse message.
                    //13.	Verify that DUT sends Notify message(s)
                    //14.	Verify received Notify messages (correct value for UTC time, TopicExpression and wsnt:Message).
                    //15.	Verify that TopicExpression is equal to tns1:DoorControl/DoorMode for all received Notify messages.
                    //16.	Verify that each notification contains Source.SimpleItem item with Name="DoorToken" and Value is equal to one of existing Door Tokens (e.g. complete list of doors contains Door with the same token). Verify that there are Notification messages for each Door.
                    //17.	Verify that each notification contains Data.SimpleItem item with Name="DoorMode" and Value with type is equal to tdc:DoorMode.
                    //18.	Verify that Notify PropertyOperation="Initialized".
                    
                    subscriptionReference =
                        ReceiveMessages(filter.Filter,
                        timeout,
                        new Action(() => { }),
                        fullList.Count,
                        notifications,
                        out subscribeStarted);
                    
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
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });       
        }

        void CommonDoorPropertyEventStateChangeTest(Func<DoorInfo, bool> doorCapabilitiesTest,
            TopicInfo topicInfo,
            ValidateMessageFunction validateMessageFunction)
        {
            EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = 60;

            RunTest(
                () =>
                {

                    //3.	Get complete list of doors from the DUT (see Annex A.1).
                    List<DoorInfo> fullDoorInfosList = GetDoorInfoList();

                    //4.	Check that there is at least one Door with Capabilities.[CAPABILITIES FOR THE TEST]= “true”. 
                    // Otherwise skip other steps and go to the next test.
                    List<DoorInfo> doorsList = null;
                    if (doorCapabilitiesTest != null)
                    {
                        doorsList = fullDoorInfosList.Where(A => doorCapabilitiesTest(A)).ToList();

                        if (doorsList.Count == 0)
                        {
                            LogTestEvent("No Doors with required Capabilities found, exit the test" + Environment.NewLine);
                            return;
                        }
                    }
                    else
                    {
                        doorsList = fullDoorInfosList;
                    }

                    //5.	ONVIF Client will select one random Door (token = Token1) with 
                    // Capabilities.DoorMonitor= “true”.
                    // ToDo: may be change it to really random selection
                    DoorInfo selectedDoor= doorsList[0];

                    // filter for current test
                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(topicInfo);

                    //6.	ONVIF Client will invoke SubscribeRequest message with tns1:DoorControl/DoorPhysicalState Topic as Filter and an InitialTerminationTime of 60s to ensure that the SubscriptionManager is deleted after one minute.
                    //7.	Verify that the DUT sends a SubscribeResponse message.
                    //8.	Test Operator will invoke change of DoorPhysicalState property for Door with token = Token1.
                    //9.	Verify that DUT sends Notify message.
                    string message = 
                        string.Format("{0} event is expected \r\n for the Door with token={{0}}", 
                        topicInfo.GetDescription());
                    
                    DoorSelectionData data = new DoorSelectionData();
                    data.SelectedToken = selectedDoor.token;
                    data.MessageTemplate = message;
                    data.Doors = doorsList.Select(
                        D => new DoorSelectionData.DoorShortInfo() { Token = D.token, Name = D.Name }).ToList();

                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();

                    try
                    {
                        subscriptionReference =
                            ReceiveMessages(filter,
                            timeout,
                            new Action(() =>
                            {
                                _operator.ShowDoorSelectionMessage(data);
                            }),
                            1,
                            (n) => CheckMessagePropertyOperation(n, OnvifMessage.CHANGED),
                            notifications,
                            out subscribeStarted);
                    }
                    finally
                    {
                        _operator.HideDoorSelectionMessage();
                    }
                    Assert(notifications.Count > 0,
                        string.Format("Message with PropertyOperation='Changed' for the door with token='{0}' has not been received", data.SelectedToken),
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
                    _operator.HideDoorSelectionMessage();
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
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
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DoorModeEvent })]
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

            CommonDoorPropertyEventTest(doorCapabilitiesTest, 
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
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DoorPhysicalStateEvent })]
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

            CommonDoorPropertyEventTest(doorCapabilitiesTest,
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
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DoorPhysicalStateEvent })]
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

            CommonDoorPropertyEventStateChangeTest(doorCapabilitiesTest,
                topicInfo,
                validateMessageFunction);
        }
        
        [Test(Name = "DOOR CONTROL – DOUBLE LOCK PHYSICAL STATE EVENT",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.04",
            Id = "6-1-4",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DoubleLockPhysicalStateEvent })]
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

            CommonDoorPropertyEventTest(doorCapabilitiesTest,
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
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DoubleLockPhysicalStateEvent })]
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

            CommonDoorPropertyEventStateChangeTest(doorCapabilitiesTest,
                topicInfo,
                validateMessageFunction);
        }
        
        [Test(Name = "DOOR CONTROL – LOCK PHYSICAL STATE EVENT",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.06",
            Id = "6-1-6",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.LockPhysicalStateEvent })]
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

            CommonDoorPropertyEventTest(doorCapabilitiesTest,
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
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.LockPhysicalStateEvent })]
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

            CommonDoorPropertyEventStateChangeTest(doorCapabilitiesTest,
                topicInfo,
                validateMessageFunction);
        }

        [Test(Name = "DOOR CONTROL – TAMPER EVENT",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.08",
            Id = "6-1-8",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DoorTamperEvent })]
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

            CommonDoorPropertyEventTest(doorCapabilitiesTest,
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
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DoorTamperEvent })]
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
            
            CommonDoorPropertyEventStateChangeTest(doorCapabilitiesTest,
                topicInfo,
                validateMessageFunction);
        }
        
        [Test(Name = "DOOR CONTROL – ALARM EVENT",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.10",
            Id = "6-1-10",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DoorAlarmEvent })]
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

            CommonDoorPropertyEventTest(doorCapabilitiesTest,
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
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DoorAlarmEvent })]
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

            CommonDoorPropertyEventStateChangeTest(doorCapabilitiesTest,
                topicInfo,
                validateMessageFunction);
        }
        
       [Test(Name = "DOOR CONTROL – FAULT EVENT",
            Path = PATH_PROPERTYEVENTS,
            Order = "06.01.12",
            Id = "6-1-12",
            Category = Category.DOORCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DoorFaultEvent })]
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

            CommonDoorPropertyEventTest(doorCapabilitiesTest,
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
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DoorFaultEvent })]
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

            CommonDoorPropertyEventStateChangeTest(doorCapabilitiesTest,
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
