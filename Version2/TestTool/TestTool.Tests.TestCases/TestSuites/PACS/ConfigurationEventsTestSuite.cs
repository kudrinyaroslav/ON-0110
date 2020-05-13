using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Exceptions;
using System.Xml;
using TestTool.Proxies.Event;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.TestSuites.Events;

using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using EndpointReferenceType = TestTool.Proxies.Event.EndpointReferenceType;

namespace TestTool.Tests.TestCases.TestSuites.PACS
{
    [TestClass]
    class ConfigurationEventsTestSuite : PacsEventsTestSuite
    {
        private const string PATH_DOORCONFIGURATION = "Door Control\\Door Configuration";
        private const string PATH_AREACONFIGURATION = "Access Control\\Area Configuration";
        private const string PATH_ACCESSCONTROLCONFIGURATION = "Access Control\\Access Point Configuration";
        
        const string DOORTOKENSIMPLEITEM = "DoorToken";
        const string ACCESSPOINTTOKENSIMPLEITEM = "AccessPointToken";
        const string AREATOKENSIMPLEITEM = "AreaToken";

        delegate bool ValidateConfigurationMessageFunction(NotificationMessageHolderType message,
            XmlElement rawElement, XmlNamespaceManager manager, StringBuilder logger);
        
        public ConfigurationEventsTestSuite(TestLaunchParam param)
            : base(param)
        {

        }
        
        void ConfigurationEventTest(
            TopicInfo topicInfo,
            Action<XmlElement, TopicInfo> validateTopic,
            ValidateConfigurationMessageFunction validateMessageFunction,
            string sourceTokenSimpleItem,
            Action<string> validateConfigurationFunction)
        {
            EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = 60;

            RunTest(
                () =>
                {

                    // Get topic description from the DUT.                
                    XmlElement topicElement = GetTopicElement(topicInfo);

                    BeginStep("Check if the event topic is supported");
                    if (topicElement == null)
                    {
                        LogStepEvent(string.Format("Topic {0} not supported", topicInfo.GetDescription()));
                    }
                    StepPassed();

                    if (topicElement == null)
                    {
                        return;
                    }

                    XmlElement messageDescription = topicElement.GetMessageDescription();
                    validateTopic(messageDescription, topicInfo);

                    // filter for current test
                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(topicInfo);

                    string message = string.Format("{0}  event is expected!", topicInfo.GetDescription());

                    Notify notify = null;
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        subscriptionReference =
                            ReceiveMessages(filter,
                            timeout,
                            () => Operator.ShowMessage(message),
                            doc,
                            out notify,
                            out subscribeStarted);
                    }
                    finally
                    {
                        Operator.HideMessage();
                    }

                    Assert(notify.NotificationMessage.Length == 1,
                        string.Format("{0} messages received - unable to check actual configuration", notify.NotificationMessage.Length),
                        "Check that exactly one notification is received");

                    BeginStep("Validate message");

                    XmlNamespaceManager manager = CreateNamespaceManager(doc);
                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = GetRawElements(notify.NotificationMessage, doc, manager, true);

                    StringBuilder logger = new StringBuilder();
                    bool ok = true;

                    MessageCheckSettings settings = new MessageCheckSettings();
                    settings.ExpectedTopic = topicInfo;
                    settings.RawMessageElements = notifications;
                    settings.NamespaceManager = manager;

                    NotificationMessageHolderType m = notify.NotificationMessage[0];
                    ok = validateMessageFunction(m, notifications[m], manager, logger);

                    if (!ok)
                    {
                        throw new AssertException(logger.ToStringTrimNewLine());
                    }

                    StepPassed();

                    // validateMessageFunction should return false, if this simple item is missing
                    string token = m.Message.GetMessageSourceSimpleItems()[sourceTokenSimpleItem];
                    validateConfigurationFunction(token);

                },
                () =>
                {
                    Operator.HideMessage();
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });
        }

        void ConfigurationEventTestBis(
            TopicInfo topicInfo,
            Action<XmlElement, TopicInfo> validateTopic,
            ValidateConfigurationMessageFunction validateMessageFunction,
            string sourceTokenSimpleItem,
            Action<string> validateConfigurationFunction)
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

                    // Get topic description from the DUT.                
                    XmlElement topicElement = GetTopicElement(topicInfo);

                    Assert(null != topicElement,
                           string.Format("Topic {0} not supported", topicInfo.GetDescription()),
                           "Check if the event topic is supported");

                    XmlElement messageDescription = topicElement.GetMessageDescription();
                    validateTopic(messageDescription, topicInfo);

                    // filter for current test
                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(topicInfo);

                    string message = string.Format("{0}  event is expected!", topicInfo.GetDescription());
                    bool UseNotify = UseNotifyToGetEvents;

                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = null;
                    SubscriptionHandler Handler = null;
                    try
                    {
                        Handler = new SubscriptionHandler(this, UseNotify, GetEventServiceAddress());
                        Handler.Subscribe(filter, actualTerminationTime);

                        Operator.ShowMessage(message);

                        var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                        Handler.WaitMessages(1, pullingCondition, out notifications);
                    }
                    finally
                    {
                        Operator.HideMessage();
                        SubscriptionHandler.Unsubscribe(Handler);
                    }
                    Assert(null != notifications && notifications.Any(),
                           string.Format("No notification messages are received.{0}WARNING: may be Operation delay is too low", Environment.NewLine),
                           "Check that DUT sent any notification messages");
                    Assert(notifications.Count == 1,
                           string.Format("{0} messages received - unable to check actual configuration", notifications.Count),
                           "Check that exactly one notification is received");

                    BeginStep("Validate message");

                    XmlNamespaceManager manager = CreateNamespaceManager(notifications.First().Value.OwnerDocument);
                    StringBuilder logger = new StringBuilder();
                    bool ok = true;

                    MessageCheckSettings settings = new MessageCheckSettings();
                    settings.ExpectedTopic = topicInfo;
                    settings.RawMessageElements = notifications;
                    settings.NamespaceManager = manager;

                    NotificationMessageHolderType m = notifications.Keys.First();
                    ok = validateMessageFunction(m, notifications[m], manager, logger);

                    if (!ok)
                    {
                        throw new AssertException(logger.ToStringTrimNewLine());
                    }

                    StepPassed();

                    // validateMessageFunction should return false, if this simple item is missing
                    string token = m.Message.GetMessageSourceSimpleItems()[sourceTokenSimpleItem];
                    validateConfigurationFunction(token);

                },
                () =>
                {
                });
        }
        #region configuration tests

/*        [Test(Name = "DOOR CONTROL – ADD OR CHANGE DOOR EVENT",
            Order = "07.01.00",
            Id = "7-1-0",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONFIGURATION,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DoorChangedEvent })]
        public void DoorSetTestBis()
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "Changed" });
            ConfigurationEventTestBis(topicInfo, ValidateDoorConfigurationTopic, ValidateDoorSetMessage, DOORTOKENSIMPLEITEM, (token) => ValidateDoorExistance(token, true));
        }*/

        [Test(Name = "DOOR CONTROL – ADD OR CHANGE DOOR EVENT",
            Order = "07.01.01",
            Id = "7-1-1",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONFIGURATION,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoorChangedEvent },
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoorChangedEvent })]
        public void DoorSetTest()
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Configuration", "Door", "Changed" });
            //TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "Changed" });
            ConfigurationEventTestBis(topicInfo, ValidateDoorConfigurationTopic, ValidateDoorSetMessage, DOORTOKENSIMPLEITEM, (token) => ValidateDoorExistance(token, true));
        }

        [Test(Name = "DOOR CONTROL – REMOVE DOOR EVENT",
            Order = "07.01.02",
            Id = "7-1-2",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONFIGURATION,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.DoorRemovedEvent },
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DoorRemovedEvent })]
        public void DoorRemovedTest()
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Configuration", "Door", "Removed" });
            //TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "Removed" });
            ConfigurationEventTestBis(topicInfo, ValidateDoorConfigurationTopic, ValidateDoorRemovedMessage, DOORTOKENSIMPLEITEM, (token) => ValidateDoorExistance(token, false));
        }

        [Test(Name = "ACCESS CONTROL – ADD OR CHANGE ACCESS POINT EVENT",
            Order = "12.01.01",
            Id = "12-1-1",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROLCONFIGURATION,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.AccessPointChangedEvent },
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.AccessPointChangedEvent})]
        public void AccessPointSetTest()
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Configuration", "AccessPoint", "Changed" });
            //TopicInfo topicInfo = ConstructTopic(new string[] { "AccessPoint", "Changed" });
            ConfigurationEventTestBis(topicInfo, ValidateAccessPointConfigurationTopic, ValidateAccessPointSetMessage, ACCESSPOINTTOKENSIMPLEITEM, (token) => ValidateAccessPointExistance(token, true));
        }

        [Test(Name = "ACCESS CONTROL – REMOVE ACCESS POINT EVENT",
            Order = "12.01.02",
            Id = "12-1-2",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROLCONFIGURATION,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.AccessPointRemovedEvent },
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.AccessPointRemovedEvent})]
        public void AccessPointRemovedTest()
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Configuration", "AccessPoint", "Removed" });
            //TopicInfo topicInfo = ConstructTopic(new string[] { "AccessPoint", "Removed" });
            ConfigurationEventTestBis(topicInfo, ValidateAccessPointConfigurationTopic, ValidateAccessPointRemovedMessage, ACCESSPOINTTOKENSIMPLEITEM, (token) => ValidateAccessPointExistance(token, false));
        }
        
        [Test(Name = "ACCESS CONTROL – ADD OR CHANGE AREA EVENT",
            Order = "13.01.01",
            Id = "13-1-1",
            Category = Category.ACCESSCONTROL,
            Path = PATH_AREACONFIGURATION,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.AreaChangedEvent },
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.AreaChangedEvent})]
        public void AreaSetTest()
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Configuration", "Area", "Changed" });
            //TopicInfo topicInfo = ConstructTopic(new string[] { "Area", "Changed" });
            ConfigurationEventTestBis(topicInfo, ValidateAreaConfigurationTopic, ValidateAreaSetMessage, AREATOKENSIMPLEITEM, (token) => ValidateAreaExistance(token, true));
        }

        [Test(Name = "ACCESS CONTROL – REMOVE AREA EVENT",
            Order = "13.01.02",
            Id = "13-1-2",
            Category = Category.ACCESSCONTROL,
            Path = PATH_AREACONFIGURATION,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.AreaRemovedEvent },
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.AreaRemovedEvent})]
        public void AreaRemovedTest()
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Configuration", "Area", "Removed" });
            //TopicInfo topicInfo = ConstructTopic(new string[] { "Area", "Removed" });
            ConfigurationEventTestBis(topicInfo, ValidateAreaConfigurationTopic, ValidateAreaRemovedMessage, AREATOKENSIMPLEITEM, (token) => ValidateAreaExistance(token, false));
        }
        

        #region validation

        void ValidateDoorConfigurationTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            ValidatePacsEntityTopic(messageDescription, topicInfo, messageInfo, DOORTOKENSIMPLEITEM);
        }

        void ValidateAreaConfigurationTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            ValidatePacsEntityTopic(messageDescription, topicInfo, messageInfo, AREATOKENSIMPLEITEM);
        }

        void ValidateAccessPointConfigurationTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            ValidatePacsEntityTopic(messageDescription, topicInfo, messageInfo, ACCESSPOINTTOKENSIMPLEITEM);
        }

        bool ValidateDoorSetMessage(NotificationMessageHolderType message,
            XmlElement rawElement,
            XmlNamespaceManager manager,
            StringBuilder logger)
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Configuration", "Door", "Changed" });
            //TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "Changed" });
            return ValidateSetMessage(topicInfo, DOORTOKENSIMPLEITEM, message, rawElement, manager, logger);
        }

        bool ValidateDoorRemovedMessage(NotificationMessageHolderType message,
            XmlElement rawElement,
            XmlNamespaceManager manager,
            StringBuilder logger)
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Configuration", "Door", "Removed" });
            //TopicInfo topicInfo = ConstructTopic(new string[] { "Door", "Removed" });
            return ValidateRemovedMessage(topicInfo, DOORTOKENSIMPLEITEM, message, rawElement, manager, logger);
        }

        bool ValidateAreaSetMessage(NotificationMessageHolderType message,
            XmlElement rawElement,
            XmlNamespaceManager manager,
            StringBuilder logger)
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Configuration", "Area", "Changed" });
            //TopicInfo topicInfo = ConstructTopic(new string[] { "Area", "Changed" });
            return ValidateSetMessage(topicInfo, AREATOKENSIMPLEITEM, message, rawElement, manager, logger);
        }

        bool ValidateAreaRemovedMessage(NotificationMessageHolderType message,
            XmlElement rawElement,
            XmlNamespaceManager manager,
            StringBuilder logger)
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Configuration", "Area", "Removed" });
            //TopicInfo topicInfo = ConstructTopic(new string[] { "Area", "Removed" });
            return ValidateRemovedMessage(topicInfo, AREATOKENSIMPLEITEM, message, rawElement, manager, logger);
        }
        
        bool ValidateAccessPointSetMessage(NotificationMessageHolderType message,
            XmlElement rawElement,
            XmlNamespaceManager manager,
            StringBuilder logger)
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Configuration", "AccessPoint", "Changed" });
            //TopicInfo topicInfo = ConstructTopic(new string[] { "AccessPoint", "Changed" });
            return ValidateSetMessage(topicInfo, ACCESSPOINTTOKENSIMPLEITEM, message, rawElement, manager, logger);
        }

        bool ValidateAccessPointRemovedMessage(NotificationMessageHolderType message,
            XmlElement rawElement,
            XmlNamespaceManager manager,
            StringBuilder logger)
        {
            TopicInfo topicInfo = ConstructTopic(new string[] { "Configuration", "AccessPoint", "Removed" });
            //TopicInfo topicInfo = ConstructTopic(new string[] { "AccessPoint", "Removed" });
            return ValidateRemovedMessage(topicInfo, ACCESSPOINTTOKENSIMPLEITEM, message, rawElement, manager, logger);
        }

        bool ValidateSetMessage(TopicInfo topicInfo, 
            string sourceSimpleItem, 
            NotificationMessageHolderType message,
            XmlElement rawElement,
            XmlNamespaceManager manager,
            StringBuilder logger)
        {
            return ValidateConfigurationMessage(message, rawElement, manager, topicInfo, sourceSimpleItem, logger);
        }

        bool ValidateRemovedMessage(TopicInfo topicInfo,
            string sourceSimpleItem, 
            NotificationMessageHolderType message,
            XmlElement rawElement,
            XmlNamespaceManager manager,
            StringBuilder logger)
        {
            return ValidateConfigurationMessage(message, rawElement, manager, topicInfo, sourceSimpleItem, logger);
        }

        bool ValidateConfigurationMessage(NotificationMessageHolderType message,
            XmlElement rawElement,
            XmlNamespaceManager manager,
            TopicInfo topicInfo, string sourceItemName,
            StringBuilder logger)
        {
            XmlElement messageElement = message.Message;

            StringBuilder dump = new StringBuilder();
            bool ok = true;

            ok = ValidateMessageCommonElements(message, rawElement, topicInfo, null, manager, dump);

            if (messageElement != null)
            {
                // check message source [presence only]
                XmlElement sourceElement = messageElement.GetMessageSource();
                if (sourceElement == null)
                {
                    logger.AppendLine("   Message Source element is missing");
                    ok = false;
                }
                else
                {
                    bool success = false;
                    string err;
                    Dictionary<string, string> sourceSimpleItems = messageElement.GetMessageSourceSimpleItems(out success, out err);
                    if (success)
                    {
                        if (!sourceSimpleItems.ContainsKey(sourceItemName))
                        {
                            logger.AppendLine(string.Format("   '{0}' SimpleItem is missing in Source", sourceItemName));
                            ok = false;
                        }
                    }
                    else
                    {
                        logger.AppendLine(string.Format("   {0}", err));
                        ok = false;
                    }
                }
            }

            if (!ok)
            {
                logger.Append(dump.ToString());
            }
            return ok;
        }

        #endregion

        #region existance

        void ValidateDoorExistance(string token, bool exists)
        {
            ValidateExistance<DoorInfo>("DoorInfo", GetDoorInfo, D => D.token, token, exists);
        }

        void ValidateAreaExistance(string token, bool exists)
        {
            ValidateExistance<AreaInfo>("AreaInfo", GetAreaInfo, A => A.token, token, exists);
        }

        void ValidateAccessPointExistance(string token, bool exists)
        {
            ValidateExistance<AccessPointInfo>("AccessPointInfo", GetAccessPointInfo, A => A.token, token, exists);
        }

        void ValidateExistance<T>(string entityName, 
            Func<string[], string, T[]> infoSelector, 
            Func<T, string> tokenSelector,
            string token, bool exists)
        {
            T[] infos = infoSelector(
                new string[] { token }, string.Format("Get {0} for token='{1}'",entityName, token));
            
            if (exists)
            {
                if (infos == null)
                {
                    Assert(false,
                        "No valid entities returned",
                        "Check that exactly one entry returned");
                }

                int length = infos.Length;
                Assert(length == 1,
                    string.Format("{0} entries returned for specified token", length),
                    "Check that exactly one entry returned");

                string actualToken = tokenSelector(infos[0]);
                Assert(actualToken == token, string.Format("DUT returned info for other token ('{0}')", actualToken),
                    "Check that information for specified token returned");
            }
            else
            {
                Assert(infos == null || infos.Length == 0, "Not empty list returned", "Check that the list is empty");
            }
        }

        #endregion

        protected DoorInfo[] GetDoorInfo(string[] tokensList, string stepName)
        {
            DoorControlPortClient client = DoorControlPortClient;
            DoorInfo[] info = null;
            RunStep(() => { info = client.GetDoorInfo(tokensList); }, stepName);
            DoRequestDelay();
            return info;
        }

        protected AreaInfo[] GetAreaInfo(string[] tokensList, string stepName)
        {
            PACSPortClient client = PACSPortClient;
            AreaInfo[] info = null;
            RunStep(() => { info = client.GetAreaInfo(tokensList); }, stepName);
            DoRequestDelay();
            return info;
        }

        protected AccessPointInfo[] GetAccessPointInfo(string[] tokensList, string stepName)
        {
            PACSPortClient client = PACSPortClient;
            AccessPointInfo[] info = null;
            RunStep(() => { info = client.GetAccessPointInfo(tokensList); }, stepName);
            DoRequestDelay();
            return info;
        }

        #endregion


    }
}
