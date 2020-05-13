using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Proxies.Event;
using System.Xml;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using System.Threading;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.TestSuites.Events;

using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using EndpointReferenceType = TestTool.Proxies.Event.EndpointReferenceType;

namespace TestTool.Tests.TestCases.TestSuites.PACS
{
    partial class DoorControlEventsTestSuite
    {
        private const string PATH_DOORCONTROL = "Door Control\\Door Control";

        private const int MESSAGEDISPLAYTIMEOUT = 10000;

        protected override void Release()
        {
            base.Release();
        }

        #region TestTemplate

        void CommonDoorOperationEventTest(
            Func<DoorInfo, bool> doorCapabilitiesTest,
            string operationName,
            TopicInfo topic,
            ValidateMessageFunction validateMessage,
            Action<string> doorOperation,
            string stateItemName,
            string expectedState)
        {
            CommonDoorOperationEventTest(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, false, "", "", stateItemName, expectedState, true);
        }
        void CommonDoorOperationEventTestBis(
            Func<DoorInfo, bool> doorCapabilitiesTest,
            string operationName,
            TopicInfo topic,
            ValidateMessageFunction validateMessage,
            Action<string> doorOperation,
            string stateItemName,
            string expectedState)
        {
            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, false, "", "", stateItemName, expectedState, true);
        }

        bool CheckChanged(NotificationMessageHolderType n)
        {
            return null != n.Message
                   && n.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE)
                   && OnvifMessage.CHANGED == n.Message.GetAttribute(OnvifMessage.PROPERTYOPERATIONTYPE);
        }

        bool CheckAccessed(NotificationMessageHolderType n)
        {
            XmlElement dataElement = n.Message.GetMessageData();
            Dictionary<string, string> dataSimpleItems = dataElement.GetMessageElementSimpleItems();
            return dataSimpleItems["State"] == "Accessed";

        }
        void CommonDoorOperationEventTestBis(
            Func<DoorInfo, bool> doorCapabilitiesTest,
            string operationName,
            TopicInfo topic,
            ValidateMessageFunction validateMessage,
            Action<string> doorOperation,
            bool checkInitialState, string initialStateItemName, string expectedInitialState,
            string stateItemName,
            string expectedState,
            bool equal)
        {
            bool UseNotify = UseNotifyToGetEvents;

            SubscriptionHandler Handler = null;

            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay / 1000;

            RunTest(
                () =>
                {
                    // Get full list
                    List<DoorInfo> fullDoorsList = GetDoorInfoList();

                    // Check if there are Doors with required properties
                    List<DoorInfo> doorInfos = fullDoorsList.Where(doorCapabilitiesTest).ToList();

                    // if there are no Doors with required capabilities, skip other steps.
                    Assert(doorInfos.Any(),
                           "No Doors with required Capabilities found, exit the test.",
                           "Check there is appropriate door for test");

                    // Select one Door.
                    DoorInfo selectedDoor = doorInfos[0];
                    string selectedToken = selectedDoor.token;
                    List<DoorSelectionData.DoorShortInfo> tokens = doorInfos.Select(D => new DoorSelectionData.DoorShortInfo() { Token = D.token, Name = D.Name }).ToList();

                    string message = string.Format("Put the Door with token '{{0}}' into state which allows '{0}' operation.{1}Note: Operation Delay in Management tab is used as a time-out for this action.", 
                                                   operationName, Environment.NewLine);

                    DoorSelectionData data = new DoorSelectionData();
                    data.Doors = tokens;
                    data.SelectedToken = selectedToken;
                    data.MessageTemplate = message;

                    WaitHandle formHandle = Operator.ShowCountdownMessage(_messageTimeout, data);
                    try
                    {
                        Sleep(_messageTimeout, new WaitHandle[] { formHandle });
                    }
                    finally
                    {
                        Operator.HideCountdownMessage();
                    }

                    selectedToken = data.SelectedToken;
                    LogTestEvent(string.Format("Door with token '{0}' will be used for test{1}", selectedToken, Environment.NewLine));

                    Proxies.Event.FilterType filter = CreateSubscriptionFilter(topic);

                    // Subscribe
                    Handler = new SubscriptionHandler(this, UseNotify, GetEventServiceAddress());
                    Handler.Subscribe(filter, actualTerminationTime);

                    Func<NotificationMessageHolderType, bool> messageCheck = (n) => CheckDoorEventSource(n, selectedToken);

                    {
                        // collect "Initialized" messaged
                        // collect messages for all door infos
                        Dictionary<NotificationMessageHolderType, XmlElement> filtered = null;

                        //Wait first notofication for selected door
                        var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout) { Filter = e => messageCheck(e) };

                        // Create raw elements list

                        //[21.02.2013] AKS: check notification for specified door is received
                        Assert(Handler.WaitMessages(1, pullingCondition, out filtered),
                               string.Format("Notification for specified door isn't received.{0}WARNING: may be Operation delay is too low", Environment.NewLine),
                               "Check that notification for selected door is received");

                        // Validate messages
                        ValidateMessages(filtered, topic, OnvifMessage.INITIALIZED, doorInfos, validateMessage);

                        //[04.03.2013] AKS: added conditional checking of door's initial state
                        if (checkInitialState)
                        {
                            var msg = filtered.Last().Key.Message;
                            Assert(null != msg,
                                   "Can't check initial state: empty message",
                                   "Check that notification with initial state present");

                            var dataSimpleItems = msg.GetMessageDataSimpleItems();
                            Assert(null != dataSimpleItems && dataSimpleItems.ContainsKey(initialStateItemName),
                                   (null == dataSimpleItems) ? "Can't check initial state: message without Data section"
                                                               :
                                                               string.Format("Can't check initial state: no SimpleItem with name '{0}' in section Data", initialStateItemName),
                                   "Validate notification with initial state");

                            Assert(expectedInitialState == dataSimpleItems[initialStateItemName],
                                   string.Format("Expected value of SimpleItem '{0}' is '{1}'. Actual value is '{2}'", initialStateItemName,
                                                                                                                       expectedInitialState,
                                                                                                                       dataSimpleItems[initialStateItemName]),
                                   "Check value of initial state");
                        }
                    }

                    {
                        // Perform operation
                        AutoResetEvent initializedEvent = new AutoResetEvent(false);
                        Dictionary<NotificationMessageHolderType, XmlElement> messages = null;
                        
                        doorOperation(selectedToken); initializedEvent.Set();

                        Handler.WaitMessages(1, new SubscriptionHandler.WaitNotificationsDuringTimeoutPollingCondition(timeout), out messages);

                        Assert(null != messages && message.Any(),
                               string.Format("No notification messages are received.{0}WARNING: may be Operation delay is too low", Environment.NewLine),
                               "Check that DUT sent any notification messages");

                        Func<NotificationMessageHolderType, bool> changedMessageFilter =
                            (n) => null != n.Message
                                   && n.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE)
                                   && OnvifMessage.CHANGED == n.Message.GetAttribute(OnvifMessage.PROPERTYOPERATIONTYPE);

                        //[04.03.2013] AKS: check that all notifications with Property Operation == 'Changed' are for selected door. Spec's step 20.
                        Dictionary<NotificationMessageHolderType, XmlElement> changedMessages = GetFilteredList(messages, changedMessageFilter);
                        Dictionary<NotificationMessageHolderType, XmlElement> changedMessagesForDoorUnderTesting = GetFilteredList(changedMessages, messageCheck);

                        Assert(changedMessages.Any(),
                               "Received no event's notifications with Property Operation = 'Changed'",
                               "Check for Property Operation = 'Changed'");

                        Assert(changedMessagesForDoorUnderTesting.Any(),
                               "Received no event's notifications with Property Operation = 'Changed' for selected door",
                               "Check that event's notifications with Property Operation = 'Changed' for selected door are received");

                        Assert(changedMessages.Count() == changedMessagesForDoorUnderTesting.Count(),
                               "Received event's notification with Property Operation = 'Changed' for door that isn't under testing",
                               "Check that no event's notifications with Property Operation = 'Changed' for other doors are received");

                        // validate state changed message
                        ValidateMessages(changedMessages, topic, OnvifMessage.CHANGED, selectedToken, validateMessage);

                        BeginStep("Validate current State");

                        //[20.02.2013] AKS: According to test spec, step 18 we should check value of stateItemName in the last message
                        //for specified door, not in the first.
                        Dictionary<string, string> dataSimpleItems = changedMessagesForDoorUnderTesting.Keys.Last().Message.GetMessageDataSimpleItems();

                        if (equal)
                        {
                            if (dataSimpleItems[stateItemName] != expectedState)
                            {
                                throw new AssertException(string.Format("{0} is incorrect: expected {1}, actual {2}", stateItemName, expectedState, dataSimpleItems[stateItemName]));
                            }
                        }
                        else
                        {
                            if (dataSimpleItems[stateItemName] == expectedState)
                            {
                                throw new AssertException(string.Format("{0} is incorrect: must be different from {1}", stateItemName, dataSimpleItems[stateItemName]));
                            }
                        }

                        StepPassed();
                    }
                },
                () =>
                {
                    SubscriptionHandler.Unsubscribe(Handler);
                });
        }
        void CommonDoorOperationEventTest(
            Func<DoorInfo, bool> doorCapabilitiesTest,
            string operationName,
            TopicInfo topic,
            ValidateMessageFunction validateMessage,
            Action<string> doorOperation,
            bool checkInitialState, string initialStateItemName, string expectedInitialState,
            string stateItemName,
            string expectedState, 
            bool equal)
        {
            EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = _eventSubscriptionTimeout;
            bool UseNotify = UseNotifyToGetEvents;
            System.DateTime TerminationExpectedTime = System.DateTime.Now;

            RunTest(
                () =>
                {
                    // Get full list
                    List<DoorInfo> fullDoorsList = GetDoorInfoList();

                    // Check if there are Doors with required properties
                    List<DoorInfo> doorInfos = fullDoorsList.Where(doorCapabilitiesTest).ToList();

                    // if there are no Doors with required capabilities, skip other steps.
                    if (doorInfos.Count == 0)
                    {
                        LogTestEvent("No Doors with required Capabilities found, exit the test.");
                        return;
                    }

                    // Select one Door.
                    DoorInfo selectedDoor = doorInfos[0];
                    string selectedToken = selectedDoor.token;
                    List<DoorSelectionData.DoorShortInfo> tokens = 
                        doorInfos.Select(
                        D => new DoorSelectionData.DoorShortInfo(){ Token = D.token, Name=D.Name}).ToList();

                    string message = string.Format("Put the Door with token '{{0}}' into state which allows '{0}' operation{1}Note: Operation Delay in Management tab is used as a time-out for this action.", 
                                                   operationName, Environment.NewLine);

                    DoorSelectionData data = new DoorSelectionData();
                    data.Doors = tokens;
                    data.SelectedToken = selectedToken;
                    data.MessageTemplate = message;

                    WaitHandle formHandle = Operator.ShowCountdownMessage(_messageTimeout, data);
                    try
                    {
                        Sleep(_messageTimeout, new WaitHandle[] { formHandle });
                    }
                    finally
                    {
                        Operator.HideCountdownMessage();
                    }

                    selectedToken = data.SelectedToken;
                    LogTestEvent(string.Format("Door with token '{0}' will be used for test{1}", selectedToken, Environment.NewLine));

                    EnsureEventPortTypeClientCreated();

                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(topic);

                    // Subscribe
                    Utils.NotifyCollectingServer eventServer = null;
                    string notificationsUri = null;
                    
                    TerminationExpectedTime = System.DateTime.Now;
                    
                    if (UseNotify)
                    {
                        try
                        {
                            eventServer = new TestTool.Tests.TestCases.Utils.NotifyCollectingServer(_nic);
                            SetupNotifyServer2(eventServer);
                            notificationsUri = eventServer.GetNotificationUri();
                        }
                        catch (Exception e)
                        {
                            Assert(false, e.Message, "Creating subscription failed");
                        }
                    }


                    System.DateTime localSubscribeStarted = System.DateTime.MaxValue; 
                    

                    //Func<NotificationMessageHolderType, bool> messageCheck = (n) => CheckDoorEventSource(n, selectedToken);
                    Func<NotificationMessageHolderType, bool> messageCheck = 
                        (n) => CheckDoorEventSource(n, selectedToken);

                    {
                        AutoResetEvent subscribedEvent = new AutoResetEvent(false);
                        // collect "Initialized" messaged


                        // collect messages for all door infos
                        Dictionary<NotificationMessageHolderType, XmlElement> messages = null;
                        //[20.02.2013] AKS: message limit as constant; changed message limit from 1 to 2 according to test spec, step 8.
                        //[28.02.2013] AKS: spec changed, changed message limit from 2 to 1 according to test spec, step 8.
                        int messageLimit = 1;
                        subscribeStarted = System.DateTime.Now;
                        if (UseNotify)
                        {
                            Action setSynchronizationPointAction =
                                () =>
                                    {
                                        StepPassed();
                                        subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out localSubscribeStarted);
                                        subscribedEvent.Set();
                                    };
                            BeginStep("Start listening");
                            Dictionary<Notify, byte[]> notifications = null;
                            notifications = eventServer.CollectNotifications(setSynchronizationPointAction, _operationDelay, messageLimit, messageCheck, subscribedEvent, _semaphore.StopEvent);
                            subscribeStarted = localSubscribeStarted;

                            Assert(notifications.Count > 0,
                                string.Format("No notifications were received for Door with token='{0}'", selectedToken),
                                "Check that the DUT sent expected notification(s)");

                            messages = GetRawElements(notifications);
                        } 
                        else 
                        {
                            int actualTerminationTime = timeout;
                            subscriptionReference = CreateStandardSubscription(filter, ref actualTerminationTime);
                            TerminationExpectedTime = System.DateTime.Now.AddSeconds(actualTerminationTime);
                            
                            Assert(null != subscriptionReference, 
                                "Can't create pullpoint subscription",
                                "Check subscription result");

                            CreatePullPointSubscriptionClient(subscriptionReference);
                            localSubscribeStarted = System.DateTime.Now;
                            subscribeStarted = localSubscribeStarted;

                            subscribedEvent.Set();

                            //NotificationMessageHolderType[] NotificationMessage = GetMessages(subscriptionReference, false, false, messageLimit, out dump);
                            //var doc = new XmlDocument();
                            //NotificationMessageHolderType[] NotificationMessage = WaitSpecifiedMessage(subscriptionReference, doc, messageCheck);
                            //XmlNamespaceManager manager = CreateNamespaceManager(doc);
                            //messages = GetRawElements(NotificationMessage, doc, manager, UseNotify);
                            //[21.02.2013] AKS: wait for specified message during operation delay time
                            messages = WaitSpecifiedMessage(subscriptionReference, messageCheck, messageLimit, ref TerminationExpectedTime);
                        }


                        // Create raw elements list
                        Dictionary<NotificationMessageHolderType, XmlElement> filtered = GetFilteredList(messages, messageCheck);

                        //[21.02.2013] AKS: check notification for specified door is received
                        Assert(filtered.Any(), 
                               "Notification for specified door isn't received",
                               "Check that notification for selected door is received");

                        // Validate messages
                        ValidateMessages(filtered, topic, OnvifMessage.INITIALIZED, doorInfos, validateMessage);

                        //[04.03.2013] AKS: added conditional checking of door's initial state
                        if (checkInitialState)
                        {
                            var msg = filtered.Last().Key.Message;
                            Assert(null != msg, 
                                   "Can't check initial state: empty message",
                                   "Check that notification with initial state present");
                            
                            var dataSimpleItems = msg.GetMessageDataSimpleItems();
                            Assert(null != dataSimpleItems && dataSimpleItems.ContainsKey(initialStateItemName), 
                                   (null == dataSimpleItems) ? "Can't check initial state: message without Data section" 
                                                               : 
                                                               string.Format("Can't check initial state: no SimpleItem with name '{0}' in section Data", initialStateItemName),
                                   "Validate notification with initial state");

                            Assert(expectedInitialState == dataSimpleItems[initialStateItemName],
                                   string.Format("Expected value of SimpleItem '{0}' is '{1}'. Actual value is '{2}'", initialStateItemName, 
                                                                                                                       expectedInitialState,
                                                                                                                       dataSimpleItems[initialStateItemName]),
                                   "Check value of initial state"
                                   );
                        }
                    }

                    {
                        // Perform operation
                        AutoResetEvent initializedEvent = new AutoResetEvent(false);
                        Dictionary<NotificationMessageHolderType, XmlElement> messages = null;
                        //[20.02.2013] AKS: message limit as constant.
                        const int messageLimit = 1;
                        if (UseNotify)
                        {
                            Action initiationAction = () => { doorOperation(selectedToken); initializedEvent.Set(); };

                            // Check state change message

                            Dictionary<Notify, byte[]> notifications = null;
                            notifications = eventServer.CollectNotifications(initiationAction, _operationDelay, messageLimit, messageCheck, initializedEvent, _semaphore.StopEvent);

                            Assert(notifications.Count > 0,
                                string.Format("No notifications were received for Door with token='{0}'", selectedToken),
                                "Check that the DUT sent notification(s)");

                            messages = GetRawElements(notifications);
                        }
                        else
                        {
                            doorOperation(selectedToken); initializedEvent.Set(); 

                            //string dump;
                            //XmlDocument doc = new XmlDocument();
                            //NotificationMessageHolderType[] NotificationMessage = WaitAllMessages(subscriptionReference, doc, messageLimit);
                            //XmlNamespaceManager manager = CreateNamespaceManager(doc);
                            //messages = GetRawElements(NotificationMessage, doc, manager, UseNotify);
                            //[21.02.2013] AKS: wait for all message during operation delay time
                            messages = WaitAllMessages(subscriptionReference, messageLimit, ref TerminationExpectedTime);
                        }

                        Func<NotificationMessageHolderType, bool> changedMessageFilter =
                            (n) => null != n.Message
                                   && n.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE)
                                   && OnvifMessage.CHANGED == n.Message.GetAttribute(OnvifMessage.PROPERTYOPERATIONTYPE);
                        
                        //[04.03.2013] AKS: check that all notifications with Property Operation == 'Changed' are for selected door. Spec's step 20.
                        Dictionary<NotificationMessageHolderType, XmlElement> changedMessages = GetFilteredList(messages, changedMessageFilter);
                        Dictionary<NotificationMessageHolderType, XmlElement> changedMessagesForDoorUnderTesting = GetFilteredList(changedMessages, messageCheck);

                        Assert(changedMessages.Any(), 
                               "Received no event's notifications with Property Operation = 'Changed'",
                               "Check for Property Operation = 'Changed'");

                        Assert(changedMessagesForDoorUnderTesting.Any(), 
                               "Received no event's notifications with Property Operation = 'Changed' for selected door",
                               "Check that event's notifications with Property Operation = 'Changed' for selected door are received");

                        Assert(changedMessages.Count() == changedMessagesForDoorUnderTesting.Count(), 
                               "Received event's notification with Property Operation = 'Changed' for door that isn't under testing",
                               "Check that no event's notifications with Property Operation = 'Changed' for other doors are received");

                        // validate state changed message
                        ValidateMessages(changedMessages, topic, OnvifMessage.CHANGED, selectedToken, validateMessage);

                        BeginStep("Validate current State");

                        //[20.02.2013] AKS: According to test spec, step 18 we should check value of stateItemName in the last message
                        //for specified door, not in the first.
                        Dictionary<string, string> dataSimpleItems = changedMessagesForDoorUnderTesting.Keys.Last().Message.GetMessageDataSimpleItems();

                        if (equal)
                        {
                            if (dataSimpleItems[stateItemName] != expectedState)
                            {
                                throw new AssertException(string.Format("{0} is incorrect: expected {1}, actual {2}", stateItemName, expectedState, dataSimpleItems[stateItemName]));
                            }
                        }
                        else
                        {
                            if (dataSimpleItems[stateItemName] == expectedState)
                            {
                                throw new AssertException(string.Format("{0} is incorrect: must be different from {1}", stateItemName, dataSimpleItems[stateItemName]));
                            }
                        }

                        StepPassed();
                    }
                    if (UseNotify)
                    {
                        RemoveHandlers2(eventServer);
                    }
                },
                () => 
                {
                    Operator.HideMessage();
                    ReleaseSubscription(subscribeStarted, subscriptionReference, TerminationExpectedTime);
                });
        }


        #endregion

        #region Access Door test 

        // temporary disable before pullpoint ready
/*        [Test(Name = "ACCESS DOOR",
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.AccessDoor, Functionality.DoorModeEvent })]
        public void AccessDoorTest()
        {
            EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = _eventSubscriptionTimeout;

            Utils.NotifyAsyncServer eventServer = null;
            RunTest(
                () =>
                {

                    // 3.	Get complete list of doors from the DUT (see Annex A.1).
                    List<DoorInfo> fullDoorsList = GetDoorInfoList();

                    //4.	ONVIF Client selects one door from the complete list of doors at step 
                    // 3 with DoorInfo.Capabilities.Access equal to true. If there is no such Door, 
                    // skip other steps and go to the next test.
                    List<DoorInfo> doorInfos = fullDoorsList.Where(
                        D => D.Capabilities != null &&
                            D.Capabilities.AccessSpecified &&
                            D.Capabilities.Access).ToList();

                    if (doorInfos.Count == 0)
                    {
                        LogTestEvent("No Doors with required Capabilities found, exit the test.");
                        return;
                    }

                    //5.	Test Operator puts Door in state when AccessDoor command will be accepted 
                    // with state change.
                    // ToDo : discuss with customer if there are better methods to select Door 
                    // which will be convenient for operator
                    List<DoorSelectionData.DoorShortInfo> tokens =
                        doorInfos.Select(
                        D => new DoorSelectionData.DoorShortInfo() { Token = D.token, Name = D.Name }).ToList();
                    string selectedToken = tokens.First().Token;

                    string message ="Put the Door with token '{0}' into state which allows 'Access' operation";

                    Definitions.Data.DoorSelectionData data = new TestTool.Tests.Definitions.Data.DoorSelectionData();
                    data.Doors = tokens;
                    data.SelectedToken = selectedToken;
                    data.MessageTemplate = message;

                    WaitHandle formHandle = Operator.ShowCountdownMessage(_messageTimeout, data);
                    try
                    {
                        Sleep(_messageTimeout, new WaitHandle[] { formHandle });
                    }
                    finally
                    {
                        Operator.HideCountdownMessage();
                    }

                    selectedToken = data.SelectedToken;
                    LogTestEvent(string.Format("Door with token '{0}' will be used for test{1}", selectedToken, Environment.NewLine));

                    TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(topic);

                    Func<NotificationMessageHolderType, bool> messageCheck = (n) => CheckDoorEventSource(n, selectedToken);

                    // Subscribe
                    eventServer = new TestTool.Tests.TestCases.Utils.NotifyAsyncServer(_nic);

                    //SetupNotifyServer2(eventServer);

                    string notificationsUri = eventServer.GetNotificationUri();

                    BeginStep("Begin listening");
                    eventServer.StartCollecting(_semaphore.StopEvent);
                    StepPassed();

                    //6.	ONVIF Client will invoke SubscribeRequest message with tns1:DoorControl/DoorMode 
                    // Topic as Filter and an InitialTerminationTime of 60s to ensure that the SubscriptionManager 
                    // is deleted after one minute.
                    //7.	Verify that the DUT sends a SubscribeResponse message.
                    subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out subscribeStarted);

                    LogTestEvent(string.Format("Wait until message for Door with token='{0}' is received", selectedToken)+Environment.NewLine);

                    System.DateTime messageTimeLimit = System.DateTime.Now.AddMilliseconds(_operationDelay);

                    string initialMode = string.Empty;
 
                    //8.	Verify that DUT sends Notify message for selected Door with current state 
                    // (Notifications for other Doors will be ignored for this test case).
                    //9.	Verify received Notify message (correct value for UTC time, TopicExpression and 
                    // wsnt:Message).
                    //10.	Verify that PropertyOperation="Initialized".
                    //11.	Verify that TopicExpression is equal to tns1:DoorControl/DoorMode.
                    //12.	Verify that notification contains Source.SimpleItem item with Name="DoorToken" 
                    // and Value is equal to selected Door Token.
                    //13.	Verify that notification contains Data.SimpleItem item with Name="DoorMode" 
                    // and Value with type is equal to tdc:DoorMode.
                    {
                        System.Diagnostics.Debug.WriteLine("Wait for messages with PropertyOperation=INITIALIZED"); 

                        Dictionary<NotificationMessageHolderType, XmlElement> initialMessages =
                            PeekMessages(eventServer, _messageTimeout,
                            messageTimeLimit, fullDoorsList.Count, messageCheck);
                        
                        Assert(initialMessages.Count > 0,
                            string.Format("Message for the door with token='{0}' has not been received", selectedToken ),
                            "Check that the message for selected door has been received");

                        // validate
                        ValidateMessages(initialMessages, topic, OnvifMessage.INITIALIZED, selectedToken, ValidateDoorModeMessage);

                        NotificationMessageHolderType receivedMessage = initialMessages.Keys.First();

                        XmlElement dataElement = receivedMessage.Message.GetMessageData();

                        Dictionary<string, string> dataSimpleItems = dataElement.GetMessageElementSimpleItems();

                        initialMode = dataSimpleItems["State"];
                    }

                    //14.	ONVIF Client will invoke AccessDoorRequest message (Token = [selected Door token]) 
                    // to change door state.
                    //15.	Verify the AccessDoorResponse message from the DUT.
                    AccessDoor(selectedToken, null, null, null, null, null);

                    messageTimeLimit = System.DateTime.Now.AddSeconds(_operationDelay/1000);

                    LogTestEvent("Wait until message with PropertyOperation=\"Changed\" is received" + Environment.NewLine);

                    //16.	Verify that DUT sends Notify message for selected Door with current state 
                    // (Notifications for other Doors will be ignored for this test case).
                    //17.	Verify received Notify message (correct value for UTC time, TopicExpression and 
                    // wsnt:Message).
                    //18.	Verify that PropertyOperation="Changed".
                    //19.	Verify that TopicExpression is equal to tns1:DoorControl/DoorMode.
                    //20.	Verify that notification contains Source.SimpleItem item with Name="DoorToken" 
                    // and Value is equal to selected Door Token.
                    //21.	Verify that notification contains Data.SimpleItem item with Name="DoorMode" 
                    // and Value equal to “Accessed”.
                    {
                        System.Diagnostics.Debug.WriteLine("Wait for first 'CHANGED' message"); 

                        Dictionary<NotificationMessageHolderType, XmlElement> doorModeChangedMessages =
                            PeekMessages(eventServer, _messageTimeout,
                            messageTimeLimit, 1, messageCheck);

                        Assert(doorModeChangedMessages.Count > 0,
                            string.Format("Message for the door with token='{0}' has not been received", selectedToken),
                            "Check that the message for selected door has been received");

                        // validate

                        ValidateMessages(doorModeChangedMessages, topic, selectedToken, ValidateDoorModeMessage);
                    
                        NotificationMessageHolderType receivedMessage = doorModeChangedMessages.Keys.First();
                        ValidateDoorModeSimpleItem(receivedMessage, DoorMode.Accessed);
                    }

                    LogTestEvent("Wait until message with PropertyOperation=\"Changed\" is received" + Environment.NewLine);

                    //22.	ONVIF Client will wait for next Notification for selected Door.
                    //23.	Verify that DUT sends Notify message for selected Door with current state 
                    // (Notifications for other Doors will be ignored for this test case).
                    //24.	Verify received Notify message (correct value for UTC time, TopicExpression 
                    // and wsnt:Message).
                    //25.	Verify that PropertyOperation="Changed".
                    //26.	Verify that TopicExpression is equal to tns1:DoorControl/DoorMode.
                    //27.	Verify that notification contains Source.SimpleItem item with Name="DoorToken" 
                    // and Value is equal to selected Door Token.
                    //28.	Verify that notification contains Data.SimpleItem item with Name="DoorMode" and 
                    // Value equal to previous state (previous state was received with Notification from step 8).
                    {
                        System.Diagnostics.Debug.WriteLine("Wait for second 'CHANGED' message");

                        messageTimeLimit = System.DateTime.Now.AddSeconds(_messageTimeout / 1000);

                        // ToDo: should we wait for "_operationDelay" ?
                        Dictionary<NotificationMessageHolderType, XmlElement> doorModeChangedMessages =
                            PeekMessages(eventServer, _messageTimeout,
                            messageTimeLimit, 1, messageCheck);

                        Assert(doorModeChangedMessages.Count > 0,
                            string.Format("Message for the door with token='{0}' has not been received", selectedToken),
                            "Check that the message for selected door has been received");

                        // validate
                        ValidateMessages(doorModeChangedMessages, topic, selectedToken, ValidateDoorModeMessage);

                        NotificationMessageHolderType receivedMessage = doorModeChangedMessages.Keys.First();
                        ValidateDoorModeSimpleItem(receivedMessage, initialMode);
                    }

                    eventServer.StopCollecting();
                },
                () =>
                {
                    Operator.HideCountdownMessage();
                    if (eventServer != null)
                    {
                        eventServer.StopCollecting();
                    }
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });
        }*/
        [Test(Name = "ACCESS DOOR",
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.AccessDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.AccessDoor, Functionality.DoorModeEvent })]
        public void AccessDoorTestBis()
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay / 1000;

            bool UseNotify = UseNotifyToGetEvents;
            SubscriptionHandler Handler = new SubscriptionHandler(this, UseNotify);
            Handler.SetPolicy(new EventsVerifyPolicy(UseNotify));

            RunTest(
                () =>
                {

                    // 3.	Get complete list of doors from the DUT (see Annex A.1).
                    List<DoorInfo> fullDoorsList = GetDoorInfoList();

                    //4.	ONVIF Client selects one door from the complete list of doors at step 
                    // 3 with DoorInfo.Capabilities.Access equal to true. If there is no such Door, 
                    // skip other steps and go to the next test.
                    List<DoorInfo> doorInfos = fullDoorsList.Where(
                        D => D.Capabilities != null &&
                            D.Capabilities.AccessSpecified &&
                            D.Capabilities.Access).ToList();

                    Assert(doorInfos.Any(),
                           "No Doors with required Capabilities found, exit the test.",
                           "Check there is appropriate door for test");

                    //5.	Test Operator puts Door in state when AccessDoor command will be accepted 
                    // with state change.
                    // ToDo : discuss with customer if there are better methods to select Door 
                    // which will be convenient for operator
                    List<DoorSelectionData.DoorShortInfo> tokens =
                        doorInfos.Select(
                        D => new DoorSelectionData.DoorShortInfo() { Token = D.token, Name = D.Name }).ToList();
                    string selectedToken = tokens.First().Token;

                    string message = "Put the Door with token '{0}' into state which allows 'Access' operation"
                                   + string.Format("{0}Note: Operation Delay in Management tab is used as a time-out for this action.", Environment.NewLine);

                    Definitions.Data.DoorSelectionData data = new TestTool.Tests.Definitions.Data.DoorSelectionData();
                    data.Doors = tokens;
                    data.SelectedToken = selectedToken;
                    data.MessageTemplate = message;

                    WaitHandle formHandle = Operator.ShowCountdownMessage(_messageTimeout, data);
                    try
                    {
                        Sleep(_messageTimeout, new WaitHandle[] { formHandle });
                    }
                    finally
                    {
                        Operator.HideCountdownMessage();
                    }

                    selectedToken = data.SelectedToken;
                    LogTestEvent(string.Format("Door with token '{0}' will be used for test{1}", selectedToken, Environment.NewLine));

                    TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(topic);

                    SubscriptionHandler.PollingConditionBase.MessageFilter messageCheck = (n) => CheckDoorEventSource(n, selectedToken);
                    SubscriptionHandler.PollingConditionBase.MessageFilter messageCheck2 = (n) => (CheckDoorEventSource(n, selectedToken) && CheckChanged(n) && CheckAccessed(n));


                    //6.	ONVIF Client will invoke SubscribeRequest message with tns1:DoorControl/DoorMode 
                    // Topic as Filter and an InitialTerminationTime of 60s to ensure that the SubscriptionManager 
                    // is deleted after one minute.
                    //7.	Verify that the DUT sends a SubscribeResponse message.
                    //subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out subscribeStarted);
                    Handler.SetAddress(GetEventServiceAddress());
                    Handler.Subscribe(filter, actualTerminationTime);

                    LogTestEvent(string.Format("Wait until message for Door with token='{0}' is received", selectedToken) + Environment.NewLine);


                    string initialMode = string.Empty;

                    //8.	Verify that DUT sends Notify message for selected Door with current state 
                    // (Notifications for other Doors will be ignored for this test case).
                    //9.	Verify received Notify message (correct value for UTC time, TopicExpression and 
                    // wsnt:Message).
                    //10.	Verify that PropertyOperation="Initialized".
                    //11.	Verify that TopicExpression is equal to tns1:DoorControl/DoorMode.
                    //12.	Verify that notification contains Source.SimpleItem item with Name="DoorToken" 
                    // and Value is equal to selected Door Token.
                    //13.	Verify that notification contains Data.SimpleItem item with Name="DoorMode" 
                    // and Value with type is equal to tdc:DoorMode.
                    {
                        System.Diagnostics.Debug.WriteLine("Wait for messages with PropertyOperation=INITIALIZED");

                        Dictionary<NotificationMessageHolderType, XmlElement> initialMessages = null;

//                        Handler.WaitMessages(
//                            timeout,
//                            messageCheck,
//                            SubscriptionHandler.WaitCondition.WC_ALL,
//                            out initialMessages);
                        var condition = new WaitNotificationsForAllDoorsPollingCondition(timeout, new []{ selectedToken }) { Filter = messageCheck };

                        Assert(Handler.WaitMessages(1, condition, out initialMessages),
                               string.Format("{0}.{1}WARNING: may be Operation delay is too low", condition.Reason, Environment.NewLine),
                               "Check that the message for selected door has been received");

                        // validate
                        ValidateMessages(initialMessages, topic, OnvifMessage.INITIALIZED, selectedToken, ValidateDoorModeMessage);

                        NotificationMessageHolderType receivedMessage = initialMessages.Keys.First();

                        XmlElement dataElement = receivedMessage.Message.GetMessageData();

                        Dictionary<string, string> dataSimpleItems = dataElement.GetMessageElementSimpleItems();

                        initialMode = dataSimpleItems["State"];
                    }

                    //14.	ONVIF Client will invoke AccessDoorRequest message (Token = [selected Door token]) 
                    // to change door state.
                    //15.	Verify the AccessDoorResponse message from the DUT.
                    AccessDoor(selectedToken, null, null, null, null, null);

                    LogTestEvent("Wait until message with PropertyOperation=\"Changed\" is received" + Environment.NewLine);

                    //16.	Verify that DUT sends Notify message for selected Door with current state 
                    // (Notifications for other Doors will be ignored for this test case).
                    //17.	Verify received Notify message (correct value for UTC time, TopicExpression and 
                    // wsnt:Message).
                    //18.	Verify that PropertyOperation="Changed".
                    //19.	Verify that TopicExpression is equal to tns1:DoorControl/DoorMode.
                    //20.	Verify that notification contains Source.SimpleItem item with Name="DoorToken" 
                    // and Value is equal to selected Door Token.
                    //21.	Verify that notification contains Data.SimpleItem item with Name="DoorMode" 
                    // and Value equal to “Accessed”.
                    {
                        System.Diagnostics.Debug.WriteLine("Wait for first 'CHANGED' message");

                        Dictionary<NotificationMessageHolderType, XmlElement> doorModeChangedMessages = null;
//                        Handler.WaitMessages(
//                            timeout,
//                            messageCheck2,
//                            //SubscriptionHandler.WaitCondition.WC_TimeAndFilterLast,
//                            SubscriptionHandler.WaitCondition.WC_ALL,
//                            out doorModeChangedMessages);
                        var condition = new WaitNotificationsForAllDoorsPollingCondition(timeout, new []{ selectedToken }) { Filter = messageCheck2 };

                        Assert(Handler.WaitMessages(1, condition, out doorModeChangedMessages),
                               string.Format("{0}.{1}WARNING: may be Operation delay is too low", condition.Reason, Environment.NewLine),
                               "Check that the message for selected door has been received");

                        // validate

                        ValidateMessages(doorModeChangedMessages, topic, selectedToken, ValidateDoorModeMessage);

                        NotificationMessageHolderType receivedMessage = doorModeChangedMessages.Keys.First();
                        ValidateDoorModeSimpleItem(receivedMessage, DoorMode.Accessed);
                    }

                    LogTestEvent("Wait until message with PropertyOperation=\"Changed\" is received" + Environment.NewLine);

                    //22.	ONVIF Client will wait for next Notification for selected Door.
                    //23.	Verify that DUT sends Notify message for selected Door with current state 
                    // (Notifications for other Doors will be ignored for this test case).
                    //24.	Verify received Notify message (correct value for UTC time, TopicExpression 
                    // and wsnt:Message).
                    //25.	Verify that PropertyOperation="Changed".
                    //26.	Verify that TopicExpression is equal to tns1:DoorControl/DoorMode.
                    //27.	Verify that notification contains Source.SimpleItem item with Name="DoorToken" 
                    // and Value is equal to selected Door Token.
                    //28.	Verify that notification contains Data.SimpleItem item with Name="DoorMode" and 
                    // Value equal to previous state (previous state was received with Notification from step 8).
                    {
                        System.Diagnostics.Debug.WriteLine("Wait for second 'CHANGED' message");

                        // ToDo: should we wait for "_operationDelay" ?
                        Dictionary<NotificationMessageHolderType, XmlElement> messages = null;
//                        Handler.WaitMessages(timeout,
//                                             null,
//                                             SubscriptionHandler.WaitCondition.WC_Timeout,
//                                             out messages);
                        var condition = new SubscriptionHandler.WaitNotificationsDuringTimeoutPollingCondition(timeout) { Filter = messageCheck };
                        Handler.WaitMessages(1, condition, out messages);

                        //var doorModeChangedMessages = messages.Where(e => messageCheck(e.Key)).ToDictionary(e => e.Key, e => e.Value);
                        Assert(messages.Any(),
                               string.Format("Message for the door with token = '{0}' has not been received.{1}WARNING: may be Operation delay is too low", selectedToken, Environment.NewLine),
                               "Check that the message for selected door has been received");

                        // validate
                        ValidateMessages(messages, topic, selectedToken, ValidateDoorModeMessage);

                        NotificationMessageHolderType receivedMessage = messages.Keys.Last();
                        ValidateDoorModeSimpleItem(receivedMessage, initialMode);
                    }
                },
                () =>
                {
                    Operator.HideCountdownMessage();
                    SubscriptionHandler.Unsubscribe(Handler);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventServer"></param>
        /// <param name="timeout">Timeout for single "Collect" </param>
        /// <param name="messageTimeLimit">Time for breaking the cycle</param>
        /// <param name="totalLimit"></param>
        /// <param name="messageCheck"></param>
        /// <returns></returns>
        Dictionary<NotificationMessageHolderType, XmlElement>
            PeekMessages(Utils.NotifyAsyncServer eventServer, 
            int timeout, System.DateTime messageTimeLimit,
            int totalLimit,
            Func<NotificationMessageHolderType, bool> messageCheck)
        {
            Dictionary<NotificationMessageHolderType, XmlElement> result = new Dictionary<NotificationMessageHolderType, XmlElement>();
            int messagesCount = 0;
            while (true)
            {
                Dictionary<Notify, byte[]> notifications =
                    eventServer.Peek(timeout, _semaphore.StopEvent);

                foreach (Notify notify in notifications.Keys)
                {
                    LogNotificationStep(notifications[notify]);
                    if (notify.NotificationMessage != null)
                    {
                        messagesCount += notify.NotificationMessage.Length;
                    }
                    if (messagesCount>=totalLimit)
                    {
                        break;
                    }
                }
                                
                Dictionary<NotificationMessageHolderType, XmlElement> messages = 
                    GetRawElements(notifications);
                Dictionary<NotificationMessageHolderType, XmlElement> filtered = 
                    GetFilteredList(messages, messageCheck);

                foreach (NotificationMessageHolderType key in filtered.Keys)
                {
                    result.Add(key, filtered[key]);
                }

                if ((messagesCount >= totalLimit) || (messageTimeLimit < System.DateTime.Now))
                {
                    break;
                }
            }
                    
            return result;
        }

        #endregion

        #region Door operations tests
/*
        [Test(Name = "BLOCK DOOR",
            Order = "03.01.00",
            Id = "3-1-0",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.BlockDoor, Functionality.DoorModeEvent })]
        public void BlockDoorTestBis()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest = 
                new Func<DoorInfo, bool>( D => D.Capabilities != null && D.Capabilities.BlockSpecified && D.Capabilities.Block);

            string operationName = "Block";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = BlockDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "Blocked");

        }
*/
        [Test(Name = "BLOCK DOOR",
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.BlockDoor,  },
            FunctionalityUnderTest = new Functionality[] { Functionality.BlockDoor, Functionality.DoorModeEvent })]
        public void BlockDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.BlockSpecified && D.Capabilities.Block);

            string operationName = "Block";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = BlockDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "Blocked");

        }
        [Test(Name = "DOUBLE LOCK DOOR",
            Order = "03.01.03",
            Id = "3-1-3",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoubleLockDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.DoubleLockDoor, Functionality.DoorModeEvent })]
        public void DoubleLockDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.DoubleLockSpecified && D.Capabilities.DoubleLock);

            string operationName = "Double Lock";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = DoubleLockDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "DoubleLocked");

        }

        [Test(Name = "LOCK DOOR",
            Order = "03.01.04",
            Id = "3-1-4",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.LockDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDoor, Functionality.DoorModeEvent })]
        public void LockDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.LockSpecified && D.Capabilities.Lock);

            string operationName = "Lock";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "Locked");
        }

        [Test(Name = "UNLOCK DOOR",
            Order = "03.01.05",
            Id = "3-1-5",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.UnlockDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.UnlockDoor, Functionality.DoorModeEvent })]
        public void UnlockDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.UnlockSpecified && D.Capabilities.Unlock);

            string operationName = "Unlock";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = UnlockDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "Unlocked");
        }
        
        [Test(Name = "LOCK DOWN DOOR",
            Order = "03.01.06",
            Id = "3-1-6",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.LockDownDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDownDoor, Functionality.DoorModeEvent })]
        public void LockDownDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.LockDownSpecified && D.Capabilities.LockDown);

            string operationName = "LockDown";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockDownDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "LockedDown");
        }


        [Test(Name = "LOCK DOWN RELEASE DOOR",
            Order = "03.01.07",
            Id = "3-1-7",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.LockDownDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDownReleaseDoor, Functionality.DoorModeEvent })]
        public void LockDownReleaseDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.LockDownSpecified && D.Capabilities.LockDown);

            string operationName = "LockDownRelease";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockDownReleaseDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation,
                                         true, "State", "LockedDown",
                                         "State", "LockedDown", false);
        }

        [Test(Name = "LOCK OPEN DOOR",
            Order = "03.01.08",
            Id = "3-1-8",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.LockOpenDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockOpenDoor, Functionality.DoorModeEvent })]
        public void LockOpenDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.LockOpenSpecified && D.Capabilities.LockOpen);

            string operationName = "LockOpen";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockOpenDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "LockedOpen");
        }


        [Test(Name = "LOCK OPEN RELEASE DOOR",
            Order = "03.01.09",
            Id = "3-1-9",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.LockOpenDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockOpenReleaseDoor, Functionality.DoorModeEvent })]
        public void LockOpenReleaseDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.LockOpenSpecified && D.Capabilities.LockOpen);

            string operationName = "LockOpenRelease";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockOpenReleaseDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation,
                                         true, "State", "LockedOpen",
                                         "State", "LockedOpen", false);
        }

        #endregion

        
        #region DoorOperations

        protected void BlockDoor(string token)
        {
            DoorControlPortClient client = DoorControlPortClient;
            RunStep(() => { client.BlockDoor(token); }, string.Format("Block door [token='{0}']", token));
            DoRequestDelay();
        }
        
        protected void LockDoor(string token)
        {
            DoorControlPortClient client = DoorControlPortClient;
            RunStep(() => { client.LockDoor(token); }, string.Format("Lock door [token='{0}']", token));
            DoRequestDelay();
        }

        protected void LockDownDoor(string token)
        {
            DoorControlPortClient client = DoorControlPortClient;
            RunStep(() => { client.LockDownDoor(token); }, string.Format("LockDown door [token='{0}']", token));
            DoRequestDelay();
        }

        protected void LockDownReleaseDoor(string token)
        {
            DoorControlPortClient client = DoorControlPortClient;
            RunStep(() => { client.LockDownReleaseDoor(token); }, string.Format("LockDownRelease door [token='{0}']", token));
            DoRequestDelay();
        }

        protected void LockOpenReleaseDoor(string token)
        {
            DoorControlPortClient client = DoorControlPortClient;
            RunStep(() => { client.LockOpenReleaseDoor(token); }, string.Format("LockOpenRelease door [token='{0}']", token));
            DoRequestDelay();
        }
        
        protected void LockOpenDoor(string token)
        {
            DoorControlPortClient client = DoorControlPortClient;
            RunStep(() => { client.LockOpenDoor(token); }, string.Format("LockOpen door [token='{0}']", token));
            DoRequestDelay();
        }

        protected void UnlockDoor(string token)
        {
            DoorControlPortClient client = DoorControlPortClient;
            RunStep(() => { client.UnlockDoor(token); }, string.Format("Unlock door [token='{0}']", token));
            DoRequestDelay();
        }


        protected void DoubleLockDoor(string token)
        {
            DoorControlPortClient client = DoorControlPortClient;
            RunStep(() => { client.DoubleLockDoor(token); }, string.Format("DoubleLock door [token='{0}']", token));
            DoRequestDelay();
        }

        protected void AccessDoor(string token,
            bool? useExtendedTime,
            string accessTime,
            string openTooLongTime,
            string preAlarmTime,
            AccessDoorExtension extension)
        {
            DoorControlPortClient client = DoorControlPortClient;
            string stepName = string.Format("Access Door (token={0})", token);
            RunStep(() => { client.AccessDoor(token, useExtendedTime, accessTime, openTooLongTime, preAlarmTime, extension); }, stepName);
            DoRequestDelay();
        }

        #endregion
       
        #region Message filtering

        bool CheckDoorEventSource(NotificationMessageHolderType message, string token)
        {
            BeginStep("Check if notification should be considered");

            bool ok = true;

            XmlElement messageElement = message.Message;

            if (messageElement != null)
            {
                bool success;
                string err;
                Dictionary<string, string> sourceSimpleItems = messageElement.GetMessageSourceSimpleItems(out success, out err);
                if (success) // don't check invalid message at all
                {
                    if (sourceSimpleItems.ContainsKey(DOORTOKENSIMPLEITEM))
                    {
                        if (sourceSimpleItems[DOORTOKENSIMPLEITEM] == token)
                        {
                            LogStepEvent(string.Format("Ok, message is for selected token: {0}", token));
                            ok = true;
                        }
                        else
                        {
                            ok = false;
                            LogStepEvent("Message is for other token, skip this notification");
                        }
                    }
                    else
                    {
                        LogStepEvent(string.Format("Notification cannot be checked: no '{0}' SimpleItem" , DOORMODESIMPLEITEM));
                    }
                }
                else
                {
                    LogStepEvent("Notification cannot be checked: " + err);
                }
            }

            StepPassed();
            return ok;
        }

        #endregion

    }
}
