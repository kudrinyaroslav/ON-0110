using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Proxies.Event;
using System.Xml;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.TestCases.Utils.Events;

using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using EndpointReferenceType = TestTool.Proxies.Event.EndpointReferenceType;

namespace TestTool.Tests.TestCases.TestSuites.PACS
{
    [TestClass]
    partial class DoorControlEventsTestSuite : PacsEventsTestSuite
    {
        public DoorControlEventsTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        private const string PATHDOORCONTROL = "Door Control\\Events";

        const string DOORTOKENSIMPLEITEM = "DoorToken";
        const string DOORMODESIMPLEITEM = "DoorMode";

        /// <summary>
        /// Common event test
        /// </summary>
        /// <param name="topicInfo">Topic to be tested</param>
        /// <param name="validateTopic">Method of topic validation</param>
        /// <param name="getDataForValidation">Method for getting additional data for message validation</param>
        /// <param name="validateMessageFunction">Method for message validation</param>
        void CommonEventTest(TopicInfo topicInfo,
            Action<XmlElement, TopicInfo> validateTopic,
            Func<object> getDataForValidation,
            ValidateMessageFunction validateMessageFunction)
        {
            EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = 60;

            RunTest(
            () =>
            {
                // Get topic description from the DUT.                
                XmlElement topicElement = GetTopicElement(topicInfo);

                Assert(topicElement != null,
                    string.Format("Topic {0} not supported", topicInfo.GetDescription()),
                    "Check that the event topic is supported");


                object data = getDataForValidation();

                // Validate topic
                XmlElement messageDescription = topicElement.GetMessageDescription();
                validateTopic(messageDescription, topicInfo);

                // Create filter for test

                FilterInfo filter = CreateFilter(topicInfo, messageDescription);

                // subscribe
                XmlDocument doc = new XmlDocument();
                Notify notify = null;
                subscriptionReference = ReceiveMessages(filter, timeout, doc, out notify, out subscribeStarted);

                if (notify != null)
                {
                    ValidateMessages(notify.NotificationMessage, doc, topicInfo, data, validateMessageFunction);
                }

            },
            () =>
            {
                if (subscribeStarted != System.DateTime.MaxValue)
                {
                    if (subscriptionReference != null && _subscriptionManagerClient == null)
                    {
                        CreateSubscriptionManagerClient(subscriptionReference);
                    }
                    TimeSpan diff = System.DateTime.Now - subscribeStarted;
                    int releaseTime = timeout * 1000 - (int)diff.TotalMilliseconds;
                    ReleaseSubscriptionManager(releaseTime);
                }
            });
        }


        #region Validate Topic
        

        /// <summary>
        /// Validates Door topic source (must have "DoorToken" name and be of the tdc:ReferenceToken type)
        /// </summary>
        /// <param name="messageDescription">Message description</param>
        /// <param name="topicInfo">Topic information</param>
        /// <param name="logger">Logger to add error description, if any</param>
        /// <returns></returns>
        bool ValidateDoorEventTopicSource(XmlElement messageDescription, TopicInfo topicInfo, StringBuilder logger)
        {
            return ValidatePacsEventTopicSource(messageDescription, topicInfo, logger, "DoorToken");
        }

        void ValidateDoorTopic(XmlElement messageDescription, 
            TopicInfo topicInfo,
            MessageDescription messageInfo)
        {
            ValidatePacsEntityTopic(messageDescription, topicInfo, messageInfo, "DoorToken");
        }

        /// <summary>
        /// Validate DoorMode event topic
        /// </summary>
        /// <param name="messageDescription">Message description</param>
        /// <param name="topicInfo">Topic information (for logging purposes)</param>
        void ValidateDoorModeTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetDoorModeMessageDescription();
            ValidateDoorTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// Validates DoorMonitor topic description
        /// </summary>
        /// <param name="messageDescription">Message description</param>
        /// <param name="topicInfo">Topic information (for logging purposes)</param>
        void ValidateDoorPhysicalStateTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetDoorPhysicalStateMessageDescription();
            ValidateDoorTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// Validates Door Double Lock monitor topic description
        /// </summary>
        /// <param name="messageDescription">Message description</param>
        /// <param name="topicInfo">Topic information (for logging purposes)</param>
        void ValidateDoorDoubleLockPhysicalStateTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetDoorDoubleLockPhysicalStateMessageDescription();
            ValidateDoorTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// Validates Door Lock monitor topic description
        /// </summary>
        /// <param name="messageDescription">Message description</param>
        /// <param name="topicInfo">Topic information (for logging purposes)</param>
        void ValidateDoorLockPhysicalStateTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetDoorLockPhysicalStateMessageDescription();
            ValidateDoorTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// Validates Door Tamper topic description
        /// </summary>
        /// <param name="messageDescription">Message description</param>
        /// <param name="topicInfo">Topic information (for logging purposes)</param>
        void ValidateDoorTamperTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetDoorTamperMessageDescription();
            ValidateDoorTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// Validates Door Alarm monitor topic description
        /// </summary>
        /// <param name="messageDescription">Message description</param>
        /// <param name="topicInfo">Topic information (for logging purposes)</param>
        void ValidateDoorAlarmTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetDoorAlarmMessageDescription();
            ValidateDoorTopic(messageDescription, topicInfo, messageInfo);
        }

        void ValidateDoorFaultTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetDoorFaultMessageDescription();
            ValidateDoorTopic(messageDescription, topicInfo, messageInfo);
        }

        #endregion


        #region DOOR EVENTS

        bool ValidateDoorMessage(NotificationMessageHolderType notification,
                                 MessageCheckSettings settings,
                                 StringBuilder logger,
                                 MessageDescription messageInfo)
        {
            XmlElement messageElement = notification.Message;
            XmlElement messageRawElement = settings.RawMessageElements[notification];
            TopicInfo topicInfo = settings.ExpectedTopic;
            XmlNamespaceManager manager = settings.NamespaceManager;

            // Init
            StringBuilder dump = new StringBuilder();
            bool ok = true;

            ok = ValidateMessageCommonElements(notification, messageRawElement, topicInfo, settings.ExpectedPropertyOperation, manager, dump);

            if (messageElement != null)
            {
                // check message source and data 

                // source

                bool localOk = ValidateDoorEventSource(messageElement, manager, settings.Data, dump);
                ok = ok && localOk;

                XmlElement messageInnerElement = messageRawElement.GetMessageContentElement();
                XmlElement dataElement = messageInnerElement.GetMessageData();
                localOk = ValidateMessageDataSimpleItems(dataElement, messageInfo, dump);
                ok = ok && localOk;
            }

            if (!ok)
            {
                logger.Append(dump.ToString());
            }
            return ok;
        }

        bool ValidateDoorEventSource(XmlElement messageElement,
                                     XmlNamespaceManager manager,
                                     object data,
                                     StringBuilder logger)
        {
            var doors = data as IEnumerable<DoorInfo>;
            string token = data as string;
            var entityInfo = data as EntityListInfo<DoorInfo>;

            bool ok = true;

            XmlElement sourceElement = messageElement.GetMessageSource();
            if (sourceElement == null)
            {
                logger.AppendLine("   Message Source element is missing");
                ok = false;
            }
            else
            {
                bool success;
                string err;

                Dictionary<string, string> sourceSimpleItems = messageElement.GetMessageSourceSimpleItems(out success, out err);
                if (success)
                {
                    if (sourceSimpleItems.ContainsKey(DOORTOKENSIMPLEITEM))
                    {
                        string value = sourceSimpleItems[DOORTOKENSIMPLEITEM];
                        // check value
                        StringBuilder error = new StringBuilder();
                        if (doors != null)
                        {
                            DoorInfo found = doors.FirstOrDefault(I => I.token == value);
                            if (found == null)
                            {
                                ok = false;
                                logger.Append(string.Format("   Door  with token '{0}' not found", value));
                            }
                        }
                        else if (entityInfo != null)
                        {
                            DoorInfo found = entityInfo.FullList.FirstOrDefault(I => I.token == value);
                            if (found == null)
                            {
                                ok = false;
                                logger.Append(string.Format("   Door  with token '{0}' not found", value));
                            }
                            else
                            {
                                found = entityInfo.FilteredList.FirstOrDefault(I => I.token == value);
                                if (found == null)
                                {
                                    ok = false;
                                    logger.Append(string.Format("   Door  with token '{0}' does not have required capabilities", value));
                                }
                            }
                        }
                        else
                        {
                            if (value != token)
                            {
                                ok = false;
                                logger.Append(string.Format("   Token is incorrect. Expected '{0}', actual '{1}'", token, value));
                            }

                        }
                    }
                    else
                    {
                        logger.AppendLine("   'DoorToken' SimpleItem is missing in Source");
                        ok = false;
                    }
                }
                else
                {
                    logger.AppendLine("   " + err);
                    ok = false;
                }
            }

            return ok;
        }               

        bool ValidateDoorModeMessage(NotificationMessageHolderType notification, MessageCheckSettings settings, StringBuilder logger)
        {
            MessageDescription messageInfo = GetDoorModeMessageDescription();

            return ValidateDoorMessage(notification, settings, logger, messageInfo);
        }

        void ValidateDoorIsInMode(NotificationMessageHolderType notification, DoorMode expectedMode)
        {
            ValidateDoorIsInMode(notification, expectedMode.ToString());
        }

        void ValidateDoorIsInMode(NotificationMessageHolderType notification, string expectedMode)
        {
            ValidateDoorModeSimpleItem(notification,
                                       (s) => s == expectedMode,
                                       (s) => string.Format("Expected: '{0}', actual: '{1}'", expectedMode, s));
        }

        void ValidateDoorIsNotInMode(NotificationMessageHolderType notification, DoorMode notExpectedMode)
        {
            ValidateDoorModeSimpleItem(notification,
                                       (s) => s != notExpectedMode.ToString(),
                                       (s) => string.Format("Expected: not '{0}', actual: '{1}'", notExpectedMode, s));
        }

        DoorMode? GetDoorStateFromNotification(NotificationMessageHolderType notification)
        {
            XmlElement dataElement = notification.Message.GetMessageData();

            Dictionary<string, string> dataSimpleItems = dataElement.GetMessageElementSimpleItems();

            if (dataSimpleItems.ContainsKey("State"))
            {
                DoorMode state;
                if (DoorMode.TryParse(dataSimpleItems["State"], out state))
                    return state;

                return null;
            }

            return null;
        }

        void ValidateDoorModeSimpleItem(NotificationMessageHolderType notification, 
                                        Func<string, bool> expectedModePredicate,
                                        Func<string, string> msgPredicate = null)
        {
            var assertHeader = "Validate 'State' simple item value";
            BeginStep(assertHeader);

            var state = GetDoorStateFromNotification(notification);

            if (state.HasValue)
            {
                var msg = "'State' simple item value is incorrect.";
                if (null != msgPredicate)
                {
                    var additionalMsg = msgPredicate(state.Value.ToString());
                    if (!string.IsNullOrEmpty(additionalMsg))
                        msg = string.Format("{0}{1}{2}", msg, Environment.NewLine, additionalMsg);
                }
                if (expectedModePredicate(state.Value.ToString()))
                    LogStepEvent("Ok");
                else
                {
                    //LogStepEvent(msg);
                    throw new AssertException(msg);
                }
            }
            else
            {
                var msg = "No 'State' field in received notification";
                //LogStepEvent(msg);
                throw new AssertException(msg);
            }

            StepPassed();
        }

        // DoorMonitor

        bool ValidateDoorPhysicalStateMessage(NotificationMessageHolderType notification, MessageCheckSettings settings, StringBuilder logger)
        {
            MessageDescription messageInfo = GetDoorPhysicalStateMessageDescription();
            return ValidateDoorMessage(notification, settings, logger, messageInfo);
        }

        // DOOR DOUBLE LOCK MONITOR 

        bool ValidateDoorDoubleLockMessage(NotificationMessageHolderType notification, MessageCheckSettings settings, StringBuilder logger)
        {
            MessageDescription messageInfo = GetDoorDoubleLockPhysicalStateMessageDescription();
            return ValidateDoorMessage(notification, settings, logger, messageInfo);
        }

        // DOOR LOCK MONITOR 

        bool ValidateDoorLockPhysicalStateMessage(NotificationMessageHolderType notification, MessageCheckSettings settings, StringBuilder logger)
        {
            MessageDescription messageInfo = GetDoorLockPhysicalStateMessageDescription();
            return ValidateDoorMessage(notification, settings, logger, messageInfo);
        }

        // DOOR TAMPER 

        bool ValidateDoorTamperMessage(NotificationMessageHolderType notification, MessageCheckSettings settings, StringBuilder logger)
        {
            MessageDescription messageInfo = GetDoorTamperMessageDescription();
            return ValidateDoorMessage(notification, settings, logger, messageInfo);
        }

        // DOOR ALARM 

        bool ValidateDoorAlarmMessage(NotificationMessageHolderType notification, MessageCheckSettings settings, StringBuilder logger)
        {
            MessageDescription messageInfo = GetDoorAlarmMessageDescription();
            return ValidateDoorMessage(notification, settings, logger, messageInfo);
        }


        bool ValidateDoorFaultMessage(NotificationMessageHolderType notification, MessageCheckSettings settings, StringBuilder logger)
        {
            MessageDescription messageInfo = GetDoorFaultMessageDescription();
            return ValidateDoorMessage(notification, settings, logger, messageInfo);
        }

        #endregion

        #region Messages descriptions

        MessageDescription GetDoorModeMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = true;
            messageInfo.AddSimpleItem("State", "DoorMode", DCNAMESPACE, true);

            return messageInfo;
        }

        MessageDescription GetDoorPhysicalStateMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = true;
            messageInfo.AddSimpleItem("State", "DoorPhysicalState", DCNAMESPACE, true);

            return messageInfo;
        }

        MessageDescription GetDoorLockPhysicalStateMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = true;
            messageInfo.AddSimpleItem("State", "LockPhysicalState", DCNAMESPACE, true);

            return messageInfo;
        }

        MessageDescription GetDoorDoubleLockPhysicalStateMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = true;
            //[25.02.2013] AKS: LockPhysicalState -> DoubleLockPhysicalState, is it a copy-paste mistake?
            messageInfo.AddSimpleItem("State", "LockPhysicalState", DCNAMESPACE, true);
            //messageInfo.AddSimpleItem("State", "DoubleLockPhysicalState", DCNAMESPACE, true);

            return messageInfo;
        }

        MessageDescription GetDoorTamperMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = true;
            messageInfo.AddSimpleItem("State", "DoorTamperState", DCNAMESPACE, true);

            return messageInfo;
        }

        MessageDescription GetDoorAlarmMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = true;
            messageInfo.AddSimpleItem("State", "DoorAlarmState", DCNAMESPACE, true);

            return messageInfo;
        }

        MessageDescription GetDoorFaultMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = true;
            messageInfo.AddSimpleItem("State", "DoorFaultState", DCNAMESPACE, true);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE, false);

            return messageInfo;
        }

        #endregion

        #region Steps


        protected List<DoorInfo> GetDoorInfoList()
        {
            DoorControlPortClient client = DoorControlPortClient;

            PACS.GetListMethod<DoorInfo> getList =
                new PACS.GetListMethod<DoorInfo>(
                    (int? limit, string offset, out DoorInfo[] list) =>
                    {
                        string newOffset = null;
                        DoorInfo[] infos = null;
                        RunStep(() => { newOffset = client.GetDoorInfoList(limit, offset, out infos); }, "Get DoorInfo list");
                        list = infos;
                        return newOffset;

                    });

            List<DoorInfo> fullList = PACS.Extensions.GetFullList(getList, null, "DoorInfo", Assert);

            Assert(fullList.Count > 0,
                "No Doors returned",
                "Check that the list of Doors is not empty");

            return fullList;

        }


        #endregion

    }

}
