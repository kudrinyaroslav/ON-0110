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
using System.Threading;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Data;

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
            CommonDoorOperationEventTest(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, stateItemName, expectedState, true);
        }
        
        void CommonDoorOperationEventTest(
            Func<DoorInfo, bool> doorCapabilitiesTest,
            string operationName,
            TopicInfo topic,
            ValidateMessageFunction validateMessage,
            Action<string> doorOperation,
            string stateItemName,
            string expectedState, 
            bool equal)
        {
            EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = _eventSubscriptionTimeout;

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

                    string message =
                        string.Format("Put the Door with token '{{0}}' into state which allows '{0}' operation",
                        operationName);

                    DoorSelectionData data = new DoorSelectionData();
                    data.Doors = tokens;
                    data.SelectedToken = selectedToken;
                    data.MessageTemplate = message;

                    WaitHandle formHandle = _operator.ShowCountdownMessage(_messageTimeout, data);
                    try
                    {
                        Sleep(_messageTimeout, new WaitHandle[] { formHandle });
                    }
                    finally
                    {
                        _operator.HideCountdownMessage();
                    }

                    selectedToken = data.SelectedToken;
                    LogTestEvent(string.Format("Door with token '{0}' will be used for test{1}", selectedToken, Environment.NewLine));

                    EnsureEventPortTypeClientCreated();

                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(topic);

                    // Subscribe
                    Utils.NotifyCollectingServer eventServer = new TestTool.Tests.TestCases.Utils.NotifyCollectingServer(_nic);

                    SetupNotifyServer2(eventServer);

                    string notificationsUri = eventServer.GetNotificationUri();

                    System.DateTime localSubscribeStarted = System.DateTime.MaxValue; 
                    

                    Func<NotificationMessageHolderType, bool> messageCheck = (n) => CheckDoorEventSource(n, selectedToken);

                    {
                        AutoResetEvent subscribedEvent = new AutoResetEvent(false);
                        // collect "Initialized" messaged
                        Action setSynchronizationPointAction = 
                            new Action(
                                () => 
                                { 
                                    StepPassed(); 
                                    subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out localSubscribeStarted);
                                    subscribedEvent.Set();
                                });
                        
                        BeginStep("Start listening");

                        // collect messages for all door infos
                        Dictionary<Notify, byte[]> notifications = eventServer.CollectNotifications(setSynchronizationPointAction,
                            _operationDelay, 1, messageCheck, subscribedEvent,
                            _semaphore.StopEvent);

                        subscribeStarted = localSubscribeStarted;

                        Assert(notifications.Count > 0,
                            string.Format("No notifications were received for Door with token='{0}'", selectedToken),
                            "Check that the DUT sent expected notification(s)");

                        // Create raw elements list
                        Dictionary<NotificationMessageHolderType, XmlElement> messages = GetRawElements(notifications);
                        Dictionary<NotificationMessageHolderType, XmlElement> filtered = GetFilteredList(messages, messageCheck);

                        // Validate messages
                        ValidateMessages(filtered, topic, OnvifMessage.INITIALIZED, doorInfos, validateMessage);
                    }

                    {
                        // Perform operation
                        AutoResetEvent initializedEvent = new AutoResetEvent(false);
                        Action initiationAction = new Action(() => { doorOperation(selectedToken); initializedEvent.Set(); });

                        // Check state change message

                        Dictionary<Notify, byte[]> notifications = eventServer.CollectNotifications(initiationAction,
                            _operationDelay, 1, messageCheck, initializedEvent,
                            _semaphore.StopEvent);

                        Assert(notifications.Count > 0,
                            string.Format("No notifications were received for Door with token='{0}'", selectedToken),
                            "Check that the DUT sent notification(s)");

                        // Create raw elements list
                        Dictionary<NotificationMessageHolderType, XmlElement> messages = GetRawElements(notifications);
                        Dictionary<NotificationMessageHolderType, XmlElement> filtered = GetFilteredList(messages, messageCheck);

                        // validate state changed message
                        ValidateMessages(filtered, topic, OnvifMessage.CHANGED, selectedToken, validateMessage);

                        BeginStep("Validate current State");

                        Dictionary<string, string> dataSimpleItems = messages.Keys.First().Message.GetMessageDataSimpleItems();

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
                    RemoveHandlers2(eventServer);
                },
                () => 
                {
                    _operator.HideMessage();
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });
        }


        #endregion

        #region Access Door test 

        [Test(Name = "ACCESS DOOR",
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

                    WaitHandle formHandle = _operator.ShowCountdownMessage(_messageTimeout, data);
                    try
                    {
                        Sleep(_messageTimeout, new WaitHandle[] { formHandle });
                    }
                    finally
                    {
                        _operator.HideCountdownMessage();
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
                    _operator.HideCountdownMessage();
                    if (eventServer != null)
                    {
                        eventServer.StopCollecting();
                    }
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
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

        [Test(Name = "BLOCK DOOR",
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.BlockDoor, Functionality.DoorModeEvent })]
        public void BlockDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest = 
                new Func<DoorInfo, bool>( D => D.Capabilities != null && D.Capabilities.BlockSpecified && D.Capabilities.Block);

            string operationName = "Block";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = BlockDoor;

            CommonDoorOperationEventTest(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "Blocked");

        }

        [Test(Name = "DOUBLE LOCK DOOR",
            Order = "03.01.03",
            Id = "3-1-3",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.DoubleLockDoor, Functionality.DoorModeEvent })]
        public void DoubleLockDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.DoubleLockSpecified && D.Capabilities.DoubleLock);

            string operationName = "Double Lock";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = DoubleLockDoor;

            CommonDoorOperationEventTest(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "DoubleLocked");

        }

        [Test(Name = "LOCK DOOR",
            Order = "03.01.04",
            Id = "3-1-4",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDoor, Functionality.DoorModeEvent })]
        public void LockDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.LockSpecified && D.Capabilities.Lock);

            string operationName = "Lock";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockDoor;

            CommonDoorOperationEventTest(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "Locked");
        }

        [Test(Name = "UNLOCK DOOR",
            Order = "03.01.05",
            Id = "3-1-5",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.UnlockDoor, Functionality.DoorModeEvent })]
        public void UnlockDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.UnlockSpecified && D.Capabilities.Unlock);

            string operationName = "Unlock";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = UnlockDoor;

            CommonDoorOperationEventTest(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "Unlocked");
        }
        
        [Test(Name = "LOCK DOWN DOOR",
            Order = "03.01.06",
            Id = "3-1-6",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDownDoor, Functionality.DoorModeEvent })]
        public void LockDownDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.LockDownSpecified && D.Capabilities.LockDown);

            string operationName = "LockDown";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockDownDoor;

            CommonDoorOperationEventTest(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "LockedDown");
        }


        [Test(Name = "LOCK DOWN RELEASE DOOR",
            Order = "03.01.07",
            Id = "3-1-7",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDownReleaseDoor, Functionality.DoorModeEvent })]
        public void LockDownReleaseDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.LockDownSpecified && D.Capabilities.LockDown);

            string operationName = "LockDownRelease";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockDownReleaseDoor;

            CommonDoorOperationEventTest(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "LockedDown", false);
        }

        [Test(Name = "LOCK OPEN DOOR",
            Order = "03.01.08",
            Id = "3-1-8",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockOpenDoor, Functionality.DoorModeEvent })]
        public void LockOpenDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.LockOpenSpecified && D.Capabilities.LockOpen);

            string operationName = "LockOpen";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockOpenDoor;

            CommonDoorOperationEventTest(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "LockedOpen");
        }


        [Test(Name = "LOCK OPEN RELEASE DOOR",
            Order = "03.01.09",
            Id = "3-1-9",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockOpenReleaseDoor, Functionality.DoorModeEvent })]
        public void LockOpenReleaseDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest =
                new Func<DoorInfo, bool>(D => D.Capabilities != null && D.Capabilities.LockOpenSpecified && D.Capabilities.LockOpen);

            string operationName = "LockOpenRelease";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockOpenReleaseDoor;

            CommonDoorOperationEventTest(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "LockedOpen", false);
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

            bool ok = false;

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
                            ok = true;
                        }
                        else
                        {
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
