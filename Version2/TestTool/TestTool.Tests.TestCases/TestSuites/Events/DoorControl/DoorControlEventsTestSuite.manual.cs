using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using FilterType = TestTool.Proxies.Event.FilterType;

namespace TestTool.Tests.TestCases.TestSuites.PACS
{
    partial class DoorControlEventsTestSuite
    {
        private const string PATH_DOORCONTROL = "Door Control\\Door Control";

        private readonly string selectedDoorMsgTemplate = "Door with token '{0}' will be used for test" + Environment.NewLine;

        private const int MESSAGEDISPLAYTIMEOUT = 10000;

        private SubscriptionHandler CurrentSubsciption;
        private void SubscribeCurrentSubsciption(Proxies.Event.FilterType filter, int actualTerminationTime)
        {
            if (null != CurrentSubsciption)
                UnsubscribeCurrentSubsciption();

            CurrentSubsciption = new SubscriptionHandler(this, UseNotifyToGetEvents, GetEventServiceAddress());
            CurrentSubsciption.Subscribe(filter, actualTerminationTime);
        }

        private void UnsubscribeCurrentSubsciption()
        {
            if (null != CurrentSubsciption)
            {
                SubscriptionHandler.Unsubscribe(CurrentSubsciption);
                CurrentSubsciption = null;
            }
        }

        protected override void Release()
        {
            base.Release();
        }

        #region TestTemplate

        void CommonDoorOperationEventTestBis(Func<DoorInfo, bool> doorCapabilitiesTest,
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

        void CommonDoorOperationEventTestBis(Func<DoorInfo, bool> doorCapabilitiesTest,
                                             string operationName,
                                             TopicInfo topic,
                                             ValidateMessageFunction validateMessage,
                                             Action<string> doorOperation,
                                             bool checkInitialState, string initialStateItemName, string expectedInitialState,
                                             string stateItemName,
                                             string expectedState,
                                             bool equal)
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
                actualTerminationTime = _eventSubscriptionTimeout;

            int timeout = _operationDelay / 1000;

            RunTest(() =>
                    {
                        // Get full list
                        List<DoorInfo> fullDoorsList = GetDoorInfoList();

                        // Check if there are Doors with required properties
                        var doors = fullDoorsList.Where(doorCapabilitiesTest);

                        // if there are no Doors with required capabilities, skip other steps.
                        Assert(doors.Any(),
                               "No Doors with required Capabilities found, exit the test.",
                               "Check there is appropriate door for test");

                        // Select one Door.
                        var selectedToken = string.Empty;
                        bool tryUserInteraction = true;
                        try
                        {
                            if ("Lock" == operationName)
                                selectedToken = TransferDoorToStateAllowingLockOperation(doors);
                            else if ("LockDownRelease" == operationName)
                                selectedToken = TransferDoorToLockedDownState(doors);
                            else if ("LockOpenRelease" == operationName)
                                selectedToken = TransferDoorToLockedOpenState(doors);
                            else
                                selectedToken = TransferDoorToLockedState(doors);
                        }
                        catch (TransferDoorToStateException)
                        {
                            tryUserInteraction = false;
                            selectedToken = TransferDoorToStateAllowingOperationManually(doors, operationName);
                        }
 
                        LogTestEvent(string.Format(selectedDoorMsgTemplate, selectedToken));

                        bool performOnceAgain;
                        do
                        {
                            performOnceAgain = false;

                            Proxies.Event.FilterType filter = CreateSubscriptionFilter(topic);

                            // Subscribe
                            SubscribeCurrentSubsciption(filter, actualTerminationTime);

                            Func<NotificationMessageHolderType, bool> messageCheck = (n) => CheckDoorEventSource(n, selectedToken);

                            {
                                // collect "Initialized" messaged
                                // collect messages for all door infos
                                Dictionary<NotificationMessageHolderType, XmlElement> filtered = null;

                                //Wait first notofication for selected door
                                var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout)
                                                       { Filter = e => messageCheck(e) };

                                // Create raw elements list

                                //[21.02.2013] AKS: check notification for specified door is received
                                Assert(CurrentSubsciption.WaitMessages(1, pullingCondition, out filtered),
                                       string.Format("Notification for specified door isn't received.{0}WARNING: may be Operation delay is too low", Environment.NewLine),
                                       "Check that notification for selected door is received");

                                // Validate messages
                                ValidateMessages(filtered, topic, OnvifMessage.INITIALIZED, doors, validateMessage);

                                //[04.03.2013] AKS: added conditional checking of door's initial state
                                if (checkInitialState)
                                {
                                    var msg = filtered.Last().Key.Message;
                                    Assert(null != msg,
                                           "Can't check initial state: empty message",
                                           "Check that notification with initial state present");

                                    var dataSimpleItems = msg.GetMessageDataSimpleItems();
                                    Assert(null != dataSimpleItems && dataSimpleItems.ContainsKey(initialStateItemName),
                                           (null == dataSimpleItems)
                                               ? "Can't check initial state: message without Data section"
                                               : string.Format("Can't check initial state: no SimpleItem with name '{0}' in section Data", initialStateItemName),
                                           "Validate notification with initial state");

                                    Assert(expectedInitialState == dataSimpleItems[initialStateItemName],
                                           string.Format("Expected value of SimpleItem '{0}' is '{1}'. Actual value is '{2}'",
                                                         initialStateItemName,
                                                         expectedInitialState,
                                                         dataSimpleItems[initialStateItemName]),
                                           "Check value of initial state");
                                }
                            }

                            try
                            {
                                // Perform operation
                                AutoResetEvent initializedEvent = new AutoResetEvent(false);
                                Dictionary<NotificationMessageHolderType, XmlElement> messages = null;

                                doorOperation(selectedToken);
                                initializedEvent.Set();

                                CurrentSubsciption.WaitMessages(1, new SubscriptionHandler.WaitNotificationsDuringTimeoutPollingCondition(timeout), out messages);

                                Assert(null != messages && messages.Any(),
                                       string.Format("No notification messages are received.{0}WARNING: may be Operation delay is too low", Environment.NewLine),
                                       "Check that DUT sent any notification messages");

                                Func<NotificationMessageHolderType, bool> changedMessageFilter =
                                    (n) => null != n.Message && n.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE)
                                           && OnvifMessage.CHANGED == n.Message.GetAttribute(OnvifMessage.PROPERTYOPERATIONTYPE);

                                //[04.03.2013] AKS: check that all notifications with Property Operation == 'Changed' are for selected door. Spec's step 20.
                                var  changedMessages = GetFilteredList(messages, changedMessageFilter);
                                var changedMessagesForDoorUnderTesting = GetFilteredList(changedMessages, messageCheck);

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
                                var dataSimpleItems = changedMessagesForDoorUnderTesting.Keys.Last().Message.GetMessageDataSimpleItems();

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
                            catch (FaultException)
                            {
                                if (!tryUserInteraction)
                                    throw;

                                StepPassed();

                                performOnceAgain = true;
                                tryUserInteraction = false;
                                selectedToken = TransferDoorToStateAllowingOperationManually(doors, operationName);
                            }
                        } while (performOnceAgain);
                    },
                    UnsubscribeCurrentSubsciption);
        }

        #endregion

        #region Access Door test 

        [Test(Name = "ACCESS DOOR",
            Id = "3-1-28",
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
                actualTerminationTime = _eventSubscriptionTimeout;
            int timeout = _operationDelay / 1000;

            RunTest(() =>
                    {

                        // 3.	Get complete list of doors from the DUT (see Annex A.1).
                        var fullDoorsList = GetDoorInfoList();

                        //4.	ONVIF Client selects one door from the complete list of doors at step 
                        // 3 with DoorInfo.Capabilities.Access equal to true. If there is no such Door, 
                        // skip other steps and go to the next test.
                        var doors = fullDoorsList.Where(D => D.Capabilities != null && D.Capabilities.AccessSpecified && D.Capabilities.Access);

                        Assert(doors.Any(),
                               "No Doors with required Capabilities found, exit the test.",
                               "Check there is appropriate door for test");

                        //5.	Test Operator puts Door in state when AccessDoor command will be accepted 
                        // with state change.
                        // ToDo : discuss with customer if there are better methods to select Door 
                        // which will be convenient for operator
                        var doorToken = string.Empty;
                        bool tryUserInteraction = true;
                        try
                        { doorToken = TransferDoorToLockedState(doors); }
                        catch (TransferDoorToStateException)
                        {
                            tryUserInteraction = false;
                            doorToken = TransferDoorToStateAllowingOperationManually(doors, "AccessDoor");
                        }

                        LogTestEvent(string.Format(selectedDoorMsgTemplate, doorToken));

                        TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
                        TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(topic);

                        SubscriptionHandler.PollingConditionBase.MessageFilter messageCheck = (n) =>  CheckDoorEventSource(n, doorToken);
                        SubscriptionHandler.PollingConditionBase.MessageFilter messageCheck2 = (n) => (CheckDoorEventSource(n, doorToken) && CheckChanged(n) && CheckAccessed(n));

                        string initialMode = string.Empty;
                        bool performOnceAgain;
                        do
                        {
                            performOnceAgain = false;
                            //6.	ONVIF Client will invoke SubscribeRequest message with tns1:DoorControl/DoorMode 
                            // Topic as Filter and an InitialTerminationTime of 60s to ensure that the SubscriptionManager 
                            // is deleted after one minute.
                            //7.	Verify that the DUT sends a SubscribeResponse message.
                            //subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out subscribeStarted);
                            SubscribeCurrentSubsciption(filter, actualTerminationTime);

                            LogTestEvent(string.Format("Wait until message for Door with token='{0}' is received", doorToken) + Environment.NewLine);

                            try
                            {
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
                                    var condition = new WaitNotificationsForAllDoorsPollingCondition(timeout, new[] {doorToken})
                                                    { Filter = messageCheck };

                                    Assert(CurrentSubsciption.WaitMessages(1, condition, out initialMessages),
                                           string.Format("{0}.{1}WARNING: may be Operation delay is too low", condition.Reason, Environment.NewLine),
                                           "Check that the message for selected door has been received");

                                    // validate
                                    ValidateMessages(initialMessages, topic, OnvifMessage.INITIALIZED, doorToken, ValidateDoorModeMessage);

                                    NotificationMessageHolderType receivedMessage = initialMessages.Keys.First();

                                    XmlElement dataElement = receivedMessage.Message.GetMessageData();

                                    var dataSimpleItems = dataElement.GetMessageElementSimpleItems();

                                    initialMode = dataSimpleItems["State"];
                                }

                                //14.	ONVIF Client will invoke AccessDoorRequest message (Token = [selected Door token]) 
                                // to change door state.
                                //15.	Verify the AccessDoorResponse message from the DUT.
                                AccessDoor(doorToken, null, null, null, null, null);
                                tryUserInteraction = false;
                            }
                            catch (FaultException)
                            {
                                if (!tryUserInteraction)
                                    throw;

                                StepPassed();

                                performOnceAgain = true;
                                tryUserInteraction = false;
                                doorToken = TransferDoorToStateAllowingOperationManually(doors, "AccessDoor");
                            }
                        } while (performOnceAgain);


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
                            var condition = new WaitNotificationsForAllDoorsPollingCondition(timeout, new []{ doorToken }) { Filter = messageCheck2 };

                            Assert(CurrentSubsciption.WaitMessages(1, condition, out doorModeChangedMessages),
                                   string.Format("{0}.{1}WARNING: may be Operation delay is too low", condition.Reason, Environment.NewLine),
                                   "Check that the message for selected door has been received");

                            // validate

                            ValidateMessages(doorModeChangedMessages, topic, doorToken, ValidateDoorModeMessage);

                            NotificationMessageHolderType receivedMessage = doorModeChangedMessages.Keys.First();
                            ValidateDoorIsInMode(receivedMessage, DoorMode.Accessed);
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
                            CurrentSubsciption.WaitMessages(1, condition, out messages);

                            //var doorModeChangedMessages = messages.Where(e => messageCheck(e.Key)).ToDictionary(e => e.Key, e => e.Value);
                            Assert(messages.Any(),
                                   string.Format("Message for the door with token = '{0}' has not been received.{1}WARNING: may be Operation delay is too low", doorToken, Environment.NewLine),
                                   "Check that the message for selected door has been received");

                            // validate
                            ValidateMessages(messages, topic, doorToken, ValidateDoorModeMessage);

                            NotificationMessageHolderType receivedMessage = messages.Keys.Last();
                            ValidateDoorIsInMode(receivedMessage, initialMode);
                        }
                    },
                    () =>
                    {
                        Operator.HideCountdownMessage();
                        UnsubscribeCurrentSubsciption();
                    });
        }

        #endregion

        #region Door operations tests

        [Test(Name = "BLOCK DOOR",
            Id = "3-1-29",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.BlockDoor,  },
            FunctionalityUnderTest = new Functionality[] { Functionality.BlockDoor, Functionality.DoorModeEvent })]
        public void BlockDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest = D => D.Capabilities != null && D.Capabilities.BlockSpecified && D.Capabilities.Block;

            string operationName = "Block";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = BlockDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "Blocked");

        }
        [Test(Name = "DOUBLE LOCK DOOR",
            Id = "3-1-30",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoubleLockDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.DoubleLockDoor, Functionality.DoorModeEvent })]
        public void DoubleLockDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest = D => D.Capabilities != null && D.Capabilities.DoubleLockSpecified && D.Capabilities.DoubleLock;

            string operationName = "Double Lock";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = DoubleLockDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "DoubleLocked");

        }

        [Test(Name = "LOCK DOOR",
            Id = "3-1-31",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.LockDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDoor, Functionality.DoorModeEvent })]
        public void LockDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest = D => D.Capabilities != null && D.Capabilities.LockSpecified && D.Capabilities.Lock;

            string operationName = "Lock";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "Locked");
        }

        [Test(Name = "UNLOCK DOOR",
            Id = "3-1-32",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.UnlockDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.UnlockDoor, Functionality.DoorModeEvent })]
        public void UnlockDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest = D => D.Capabilities != null && D.Capabilities.UnlockSpecified && D.Capabilities.Unlock;

            string operationName = "Unlock";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = UnlockDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "Unlocked");
        }
        
        [Test(Name = "LOCK DOWN DOOR",
            Id = "3-1-35",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.LockDownDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDownDoor, Functionality.DoorModeEvent })]
        public void LockDownDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest = D => D.Capabilities != null && D.Capabilities.LockDownSpecified && D.Capabilities.LockDown;

            string operationName = "LockDown";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockDownDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "LockedDown");
        }


        [Test(Name = "LOCK DOWN RELEASE DOOR",
            Id = "3-1-36",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.LockDownDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDownReleaseDoor, Functionality.DoorModeEvent })]
        public void LockDownReleaseDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest = (D => D.Capabilities != null && D.Capabilities.LockDownSpecified && D.Capabilities.LockDown);

            string operationName = "LockDownRelease";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockDownReleaseDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation,
                                            true, "State", "LockedDown",
                                            "State", "LockedDown", false);
        }

        [Test(Name = "LOCK OPEN DOOR",
            Id = "3-1-33",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.LockOpenDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockOpenDoor, Functionality.DoorModeEvent })]
        public void LockOpenDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest = D => D.Capabilities != null && D.Capabilities.LockOpenSpecified && D.Capabilities.LockOpen;

            string operationName = "LockOpen";
            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            ValidateMessageFunction validateMessage = ValidateDoorModeMessage;

            Action<string> doorOperation = LockOpenDoor;

            CommonDoorOperationEventTestBis(doorCapabilitiesTest, operationName, topic, validateMessage, doorOperation, "State", "LockedOpen");
        }


        [Test(Name = "LOCK OPEN RELEASE DOOR",
            Id = "3-1-34",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.LockOpenDoor },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockOpenReleaseDoor, Functionality.DoorModeEvent })]
        public void LockOpenReleaseDoorTest()
        {
            Func<DoorInfo, bool> doorCapabilitiesTest = D => D.Capabilities != null && D.Capabilities.LockOpenSpecified && D.Capabilities.LockOpen;

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

        internal class TransferDoorToStateException: Exception
        {}

        string TransferDoorToStateManually(string doorToken, IEnumerable<DoorInfo> doors, string message)
        {
            var tokens = doors.Select(D => new DoorSelectionData.DoorShortInfo() { Token = D.token, Name = D.Name });

            var data = new DoorSelectionData
                       {
                           Doors = tokens,
                           SelectedToken = doorToken,
                           MessageTemplate = message
                       };

            WaitHandle formHandle = Operator.ShowCountdownMessage(_messageTimeout, data);
            try
            {
                Sleep(_messageTimeout, new WaitHandle[] { formHandle });
            }
            finally
            {
                Operator.HideCountdownMessage();
            }

            if (doorToken != data.SelectedToken)
                LogStepEvent(string.Format(selectedDoorMsgTemplate, data.SelectedToken));

            return data.SelectedToken;
        }

        string TransferDoorToStateAllowingOperationManually(IEnumerable<DoorInfo> doors, string operationName)
        {
            var doorToken = doors.First().token;
            string message = "Put the Door with token '{0}' into state which" + string.Format(" allows '{0}' operation.", operationName)
                           + string.Format("{0}Note: Operation Delay in Management tab is used as a time-out for this action.", Environment.NewLine);

            return TransferDoorToStateManually(doorToken, doors, message);
        }

        Dictionary<NotificationMessageHolderType, XmlElement> WaitForChangedMessage(string doorToken, TopicInfo topic, int timeout)
        {
            SubscriptionHandler.PollingConditionBase.MessageFilter messageCheck = (n) => CheckDoorEventSource(n, doorToken) && CheckChanged(n);

            Dictionary<NotificationMessageHolderType, XmlElement> messages = null;
            var condition = new SubscriptionHandler.WaitNotificationsDuringTimeoutPollingCondition(timeout) { Filter = messageCheck };
            CurrentSubsciption.WaitMessages(1, condition, out messages);

            Assert(messages.Any(),
                   string.Format("Message for the door with token = '{0}' has not been received.{1}WARNING: may be Operation delay is too low",
                                 doorToken, Environment.NewLine),
                   "Check that the message for selected door has been received");

            // validate
            ValidateMessages(messages, topic, doorToken, ValidateDoorModeMessage);

            return messages;
        }

        Dictionary<NotificationMessageHolderType, XmlElement> WaitForDoorModeChangedMessage(string doorToken)
        {
            int timeout = _operationDelay / 1000;

            TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
            return WaitForChangedMessage(doorToken, topic, timeout);
        }

        class DoorModeTransferAction
        {
            public DoorModeTransferAction(Func<DoorMode, bool> alreadyInStatePredicate,
                                           Action<DoorState> transferAction,
                                           Action<DoorMode, NotificationMessageHolderType> expectedStatePredicate = null)
            {
                AlreadyInStatePredicate = alreadyInStatePredicate;
                TransferAction = transferAction;
                ExpectedStatePredicate = expectedStatePredicate;
            }

            public readonly Func<DoorMode, bool> AlreadyInStatePredicate;
            public readonly Action<DoorState> TransferAction;
            public readonly Action<DoorMode, NotificationMessageHolderType> ExpectedStatePredicate;
        }

        string TransferDoorToState(string doorToken, IEnumerable<DoorModeTransferAction> transferSequence)
        {
            if (!transferSequence.Any())
                return doorToken;

            try
            {
                var state = GetDoorState(doorToken);

                TopicInfo topic = ConstructTopic(new string[] { "Door", "State", "DoorMode" });
                FilterType filter = CreateSubscriptionFilter(topic);

                int actualTerminationTime = 60;
                if (_eventSubscriptionTimeout != 0)
                    actualTerminationTime = _eventSubscriptionTimeout;

                bool subscribed = false;
                DoorMode? currentDoorMode = state.DoorMode;
                //foreach (var doorStateTransferAction in transferSequence)
                //{
                //    if (!doorStateTransferAction.AlreadyInStatePredicate(currentDoorMode.Value))
                //    {
                //        if (!subscribed)
                //        {
                //            SubscribeCurrentSubsciption(filter, actualTerminationTime);
                //            subscribed = true;
                //            Sleep(3000);
                //        }

                //        try
                //        { doorStateTransferAction.TransferAction(state); }
                //        catch(FaultException)
                //        {
                //            StepPassed();
                //            throw new TransferDoorToStateException();
                //        }

                //        var messages = WaitForDoorModeChangedMessage(doorToken);

                //        var receivedMessage = messages.Keys.Last();
                //        if (null != doorStateTransferAction.ExpectedStatePredicate)
                //            doorStateTransferAction.ExpectedStatePredicate(currentDoorMode.Value, receivedMessage);
                //        currentDoorMode = GetDoorStateFromNotification(receivedMessage);
                //    }
                //}

                var transferSequenceArray = transferSequence.ToArray();

                var startAction = Array.FindLastIndex(transferSequenceArray, e => e.AlreadyInStatePredicate(currentDoorMode.Value));

                for (int currAction = -1 != startAction ? startAction : 0; currAction < transferSequenceArray.Count(); currAction++)
                {
                    var doorStateTransferAction = transferSequenceArray[currAction];
                    if (!doorStateTransferAction.AlreadyInStatePredicate(currentDoorMode.Value))
                    {
                        if (!subscribed)
                        {
                            SubscribeCurrentSubsciption(filter, actualTerminationTime);
                            subscribed = true;
                            Sleep(3000);
                        }

                        try
                        { doorStateTransferAction.TransferAction(state); }
                        catch (FaultException)
                        {
                            StepPassed();
                            throw new TransferDoorToStateException();
                        }

                        var messages = WaitForDoorModeChangedMessage(doorToken);

                        var receivedMessage = messages.Keys.Last();
                        if (null != doorStateTransferAction.ExpectedStatePredicate)
                            doorStateTransferAction.ExpectedStatePredicate(currentDoorMode.Value, receivedMessage);
                        currentDoorMode = GetDoorStateFromNotification(receivedMessage);
                    }
                }

                return doorToken;
            }
            catch (FaultException)
            {
                StepPassed();
                throw new TransferDoorToStateException();
            }
        }

        void ManualTransferIfNotInState(NotificationMessageHolderType msg, DoorMode doorMode)
        {
            try
            { ValidateDoorIsInMode(msg, doorMode); }
            catch (AssertException)
            {
                StepPassed();
                throw new TransferDoorToStateException();
            }           
        }

        void ManualTransferIfInState(NotificationMessageHolderType msg, DoorMode doorMode)
        {
            try
            { ValidateDoorIsNotInMode(msg, doorMode); }
            catch (AssertException)
            {
                StepPassed();
                throw new TransferDoorToStateException();
            }
        }

        List<DoorModeTransferAction> TransferSequenceToLockedState(string doorToken)
        {
            var transferSequence = new List<DoorModeTransferAction>();

            transferSequence.Add(new DoorModeTransferAction((doorMode) => DoorMode.Locked == doorMode,
                                                            (doorMode) =>
                                                            {
                                                                if (DoorMode.Unlocked == doorMode.DoorMode || DoorMode.Accessed == doorMode.DoorMode ||
                                                                    DoorMode.DoubleLocked == doorMode.DoorMode || DoorMode.Blocked == doorMode.DoorMode)
                                                                    LockDoor(doorToken);
                                                                else if (DoorMode.LockedDown == doorMode.DoorMode)
                                                                    LockDownReleaseDoor(doorToken);
                                                                else if (DoorMode.LockedOpen == doorMode.DoorMode)
                                                                    LockOpenReleaseDoor(doorToken);
                                                                else
                                                                    throw new TransferDoorToStateException();
                                                            },
                                                            (doorModeBefore, msg) =>
                                                            {
                                                                if (doorModeBefore == DoorMode.LockedOpen)
                                                                    ManualTransferIfNotInState(msg, DoorMode.Unlocked);
                                                                else
                                                                {
                                                                    if (doorModeBefore == DoorMode.LockedDown)
                                                                        ManualTransferIfNotInState(msg, DoorMode.Locked);

                                                                    ValidateDoorIsInMode(msg, DoorMode.Locked);
                                                                }
                                                            }));

            transferSequence.Add(new DoorModeTransferAction((doorMode) => DoorMode.Locked == doorMode,
                                                            (doorMode) => LockDoor(doorToken),
                                                            (doorModeBefore, msg) => ValidateDoorIsInMode(msg, DoorMode.Locked)));

            return transferSequence;
        }

        string TransferDoorToLockedState(IEnumerable<DoorInfo> doors)
        {
            var lockDoors = doors.Where(d => null != d.Capabilities && d.Capabilities.LockSpecified && d.Capabilities.Lock);
            if (lockDoors.Any())
            {
                var doorToken = lockDoors.First().token;
                return TransferDoorToState(doorToken, TransferSequenceToLockedState(doorToken));
            }

            throw new TransferDoorToStateException();
        }

        string TransferDoorToStateAllowingLockOperation(IEnumerable<DoorInfo> doors)
        {
            var doorToken = doors.First().token;
            var transferSequence = new List<DoorModeTransferAction>();

            //At this moment we don't know if there is a need to perform LockDownRelease.
            //So don't skip second step initially.
            bool skipSecondStep = false;
            transferSequence.Add(new DoorModeTransferAction((doorMode) => 
                                                            {
                                                                return skipSecondStep = DoorMode.Blocked      == doorMode ||
                                                                                        DoorMode.Unlocked     == doorMode ||
                                                                                        DoorMode.DoubleLocked == doorMode ||
                                                                                        DoorMode.Accessed     == doorMode;
                                                            },
                                                            (doorState) =>
                                                            {
                                                                skipSecondStep = DoorMode.LockedDown != doorState.DoorMode;
                                                                if (DoorMode.Locked == doorState.DoorMode)
                                                                    UnlockDoor(doorToken);
                                                                else if (DoorMode.LockedOpen == doorState.DoorMode)
                                                                    LockOpenReleaseDoor(doorToken);
                                                                else if (DoorMode.LockedDown == doorState.DoorMode)
                                                                    LockDownReleaseDoor(doorToken);
                                                                else
                                                                    throw new TransferDoorToStateException();
                                                            },
                                                            (doorModeBefore, msg) =>
                                                            {
                                                                if (DoorMode.LockedDown == doorModeBefore)
                                                                    ManualTransferIfNotInState(msg, DoorMode.Locked);
                                                                else if (DoorMode.LockedOpen == doorModeBefore)
                                                                    ManualTransferIfInState(msg, DoorMode.Locked);
                                                                else
                                                                    ValidateDoorIsNotInMode(msg, DoorMode.Locked);
                                                            }));

            transferSequence.Add(new DoorModeTransferAction((doorMode) => skipSecondStep,
                                                            (doorState) => UnlockDoor(doorToken),
                                                            (doorModeBefore, msg) => ValidateDoorIsNotInMode(msg, DoorMode.Locked)));

            return TransferDoorToState(doorToken, transferSequence);
        }

        string TransferDoorToLockedDownState(IEnumerable<DoorInfo> doors)
        {
            var doorToken = doors.First().token;
            var transferSequence = TransferSequenceToLockedState(doorToken);

            transferSequence.Add(new DoorModeTransferAction((doorMode) => DoorMode.LockedDown == doorMode,
                                                            (doorMode) => LockDownDoor(doorToken),
                                                            (doorModeBefore, msg) => ValidateDoorIsInMode(msg, DoorMode.LockedDown)));

            return TransferDoorToState(doorToken, transferSequence);
        }

        string TransferDoorToLockedOpenState(IEnumerable<DoorInfo> doors)
        {
            var doorToken = doors.First().token;
            var transferSequence = TransferSequenceToLockedState(doorToken);
            transferSequence.Add(new DoorModeTransferAction((doorMode) => DoorMode.LockedOpen == doorMode,
                                                            (doorMode) => LockOpenDoor(doorToken),
                                                            (doorModeBefore, msg) => ValidateDoorIsInMode(msg, DoorMode.LockedOpen)));

            return TransferDoorToState(doorToken, transferSequence);
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
