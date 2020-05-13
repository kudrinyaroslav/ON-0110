using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Event;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using System.Xml;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.TestCases.Utils;

namespace TestTool.Tests.TestCases.TestSuites
{
    partial class AccessControlEventsTestSuite
    {
        private const string PATHACCESSGRANTEDEVENTS = "Access Control\\Access Granted Events";
        private const string PATHACCESSTAKENEVENTS = "Access Control\\Access Taken Events";
        private const string PATHACCESSNOTTAKENEVENTS = "Access Control\\Access Not Taken Events";
        private const string PATHACCESSDENIEDEVENTS = "Access Control\\Access Denied Events";
        private const string PATHDURESSEVENTS = "Access Control\\Duress Events";

        #region Common manual test

        void ManualAccessPointEventTest(
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest,
            TopicInfo topicInfo,
            Action<XmlElement, TopicInfo> validateTopic,
            ValidateMessageFunction validateMessageFunction)
        {
            EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = 60;

            RunTest(
                () =>
                {

                    //3.	Get complete list of access points from the DUT (see Annex A.1).                    
                    List<AccessPointInfo> fullAccessPointsList = GetAccessPointInfoList();

                    //4.	Check that there is at least one Access Point with required Capabilities

                    List<AccessPointInfo> accessPointsList = null;
                    if (accessPointCapabilitiesTest != null)
                    {
                        accessPointsList = fullAccessPointsList.Where(A => accessPointCapabilitiesTest(A)).ToList();

                        if (accessPointsList.Count == 0)                        
                        {
                            LogTestEvent("No AccessPoints with required Capabilities found, exit the test" + Environment.NewLine);
                            return;
                        }
                    }
                    else
                    {
                        accessPointsList = fullAccessPointsList;
                    }

                    // Get topic description from the DUT.                
                    XmlElement topicElement = GetTopicElement(topicInfo);

                    Assert(topicElement != null,
                        string.Format("Topic {0} not supported", topicInfo.GetDescription()),
                        "Check that the event topic is supported");

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
                            new Action(() =>
                            {
                                _operator.ShowMessage(message);
                            }),
                            doc,
                            out notify,
                            out subscribeStarted);
                    }
                    finally
                    {
                        _operator.HideMessage();
                    }

                    BeginStep("Validate messages");

                    XmlNamespaceManager manager = CreateNamespaceManager(doc);
                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = GetRawElements(notify.NotificationMessage, doc, manager, true);
                    
                    StringBuilder logger = new StringBuilder();
                    bool ok = true;

                    MessageCheckSettings settings = new MessageCheckSettings();
                    settings.Data = accessPointsList;
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
                    _operator.HideMessage();
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });
        }

        void ManualAccessPointEventSequenceTest(
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest,
            TopicInfo topicUnderTest,
            Action<XmlElement, TopicInfo> validateTopic,
            MessageDescription messageInfo,
            TopicInfo initiationTopic,
            MessageDescription initiationMessageInfo)
        {
            EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = 60;

            RunTest(
                () =>
                {
                    //3.	Get complete list of access points from the DUT (see Annex A.1).                    
                    List<AccessPointInfo> fullAccessPointsList = GetAccessPointInfoList();

                    //4.	Check that there is at least one Access Point with required Capabilities

                    List<AccessPointInfo> accessPointsList = null;
                    if (accessPointCapabilitiesTest != null)
                    {
                        accessPointsList = fullAccessPointsList.Where(A => accessPointCapabilitiesTest(A)).ToList();

                        if (accessPointsList.Count == 0)
                        {
                            LogTestEvent("No AccessPoints with required Capabilities found, exit the test" + Environment.NewLine);
                            return;
                        }
                    }
                    else
                    {
                        accessPointsList = fullAccessPointsList;
                    }

                    // Get topic description from the DUT.                
                    XmlElement topicElement = GetTopicElement(topicUnderTest);

                    Assert(topicElement != null,
                        string.Format("Topic {0} not supported", topicUnderTest.GetDescription()),
                        "Check that the event topic is supported");

                    XmlElement messageDescription = topicElement.GetMessageDescription();
                    validateTopic(messageDescription, topicUnderTest);

                    // filter for current test
                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(new TopicInfo[] { topicUnderTest, initiationTopic });

                    // create notification listener and subscription
                    Utils.NotifyServer server = new NotifyServer(_nic);

                    SetupNotifyServer(server);

                    string notificationsUri = server.GetNotificationUri();

                    subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out subscribeStarted);

                    string accessPointToken = null;
                        
                    string message = string.Format("{0}  event is expected!", initiationTopic.GetDescription());

                    Action eventInitiationAction = new Action(() =>
                    {
                        StepPassed();
                        _operator.ShowMessage(message);
                    });

                    EntityListInfo<AccessPointInfo> data = new EntityListInfo<AccessPointInfo>();
                    data.FilteredList = accessPointsList;
                    data.FullList = fullAccessPointsList;

                    NotificationMessageHolderType initiationMessage = null;
                    // receive events
                    {                        
                        BeginStep("Start listening");
                        Notify notify = null;
                        try
                        {
                            notify = server.WaitForNotify(eventInitiationAction,
                                 timeout * 1000,
                                 _semaphore.StopEvent);
                        }
                        finally
                        {
                            _operator.HideMessage();
                        }

                        ValidateNotificationsPacket(server.RawData);

                        ValidateNotifyNotEmpty(notify);

                        if (notify.NotificationMessage.Length > 1)
                        {
                            LogTestEvent("DUT sent more than one notification. Test will be performed for token from the first notification" + Environment.NewLine);
                        }

                        NotificationMessageHolderType notification = notify.NotificationMessage[0];
                        initiationMessage = notification;

                        XmlElement messageElement = notification.Message;

                        // message validation
                        XmlDocument doc = new XmlDocument();
                        string soapRawPacket = System.Text.Encoding.UTF8.GetString(server.RawData);
                        doc.LoadXml(soapRawPacket);

                        NotificationMessageHolderType theMessage = notify.NotificationMessage[0];

                        BeginStep("Validate message");

                        XmlNamespaceManager manager = CreateNamespaceManager(doc);

                        Dictionary<NotificationMessageHolderType, XmlElement> rawElements = GetRawElements(notify.NotificationMessage,
                                                                                                           doc,
                                                                                                           manager,
                                                                                                           true);

                        StringBuilder logger = new StringBuilder();
                        bool ok = true;

                        MessageCheckSettings settings = new MessageCheckSettings();
                        settings.NamespaceManager = manager;
                        settings.ExpectedPropertyOperation = null;
                        settings.ExpectedTopic = initiationTopic;
                        settings.RawMessageElements = rawElements;
                        settings.Data = data;

                        ok = ValidateAccessPointMessage(theMessage, settings, logger, initiationMessageInfo);
                                                
                        if (!ok)
                        {
                            throw new AssertException(logger.ToStringTrimNewLine());
                        }
                        StepPassed();

                        // message validated

                        // if names are validated, OK will be false by this moment
                        Dictionary<string, string> sourceSimpleItems = messageElement.GetMessageSourceSimpleItems();
                        accessPointToken = sourceSimpleItems[ACCESSPOINTTOKENSIMPLEITEM];
                    }

                    // receive events - "Real" check
                    {
                        message = string.Format("{0}  event is expected!", topicUnderTest.GetDescription());
                        
                        BeginStep("Start listening");
                        Notify notify = server.WaitForNotify(eventInitiationAction,
                            timeout * 1000,
                            _semaphore.StopEvent);
                        _operator.HideMessage();

                        RemoveHandlers(server);

                        ValidateNotificationsPacket(server.RawData);

                        XmlDocument doc = new XmlDocument();
                        string soapRawPacket = System.Text.Encoding.UTF8.GetString(server.RawData);
                        doc.LoadXml(soapRawPacket);

                        ValidateNotifyNotEmpty(notify);

                        Assert(notify.NotificationMessage.Length == 1,
                            "DUT sent more than one notification",
                            "Check that exactly one notification message hes been received");

                        NotificationMessageHolderType theMessage = notify.NotificationMessage[0];

                        BeginStep("Validate message");

                        XmlNamespaceManager manager = CreateNamespaceManager(doc);

                        Dictionary<NotificationMessageHolderType, XmlElement> rawElements = GetRawElements(notify.NotificationMessage,
                                                                                                           doc,
                                                                                                           manager,
                                                                                                           true);

                        StringBuilder logger = new StringBuilder();
                        bool ok = true;
                        
                        MessageCheckSettings settings = new MessageCheckSettings();
                        settings.NamespaceManager = manager;
                        settings.ExpectedPropertyOperation = null;
                        settings.ExpectedTopic = topicUnderTest;
                        settings.RawMessageElements = rawElements;
                        settings.Data = accessPointToken;

                        // check theMessage
                        //16.	Verify received Notify message (correct value for UTC time, TopicExpression 
                        // and wsnt:Message).
                        //17.	Verify that TopicExpression is equal to tns1:AccessControl/AccessTaken/Anonymous 
                        // for the Notify message.
                        //18.	Verify that the notification contains Source.SimpleItem item with 
                        // Name="AccessPointToken" and Value is equal to AccessPointToken from 
                        // tns1:AccessControl/AccessGranted/Anonymous notification and there is Access Point 
                        // Tokens with Capabilities.AnonymousAccess = "true" and Capabilities.AccessTaken = "true" 
                        // in the complete list of access points (e.g. complete list of access points contains 
                        // Access Point with the same token and this Access Point has Capabilities.AnonymousAccess = "true" 
                        // and Capabilities.AccessTaken = "true").

                        ok = ValidateAccessPointMessage(theMessage, settings, logger, messageInfo);

                        if (ok)
                        {
                            foreach (string itemName in messageInfo.DataSimpleItems.Keys)
                            {
                                if (initiationMessageInfo.DataSimpleItems.Keys.Contains(itemName))
                                {
                                    // simple items must be OK by that moment
                                    Dictionary<string, string> initiationMessageSimpleItems =
                                        initiationMessage.Message.GetMessageDataSimpleItems();

                                    Dictionary<string, string> resultMessageSimpleItems =
                                        theMessage.Message.GetMessageDataSimpleItems();

                                    bool check = true;
                                    if (!messageInfo.DataSimpleItems[itemName].Mandatory)
                                    {
                                        check = initiationMessageSimpleItems.ContainsKey(itemName) &&
                                            resultMessageSimpleItems.ContainsKey(itemName);
                                    }

                                    if (check)
                                    {
                                        bool localOk = initiationMessageSimpleItems[itemName] == resultMessageSimpleItems[itemName];
                                        if (!localOk)
                                        {
                                            ok = false;
                                            logger.AppendLine(string.Format("'{0}' SimpleItems are different: '{1}' in first notification message, '{2}' in second",
                                                itemName, initiationMessageSimpleItems[itemName], resultMessageSimpleItems[itemName]));
                                        }
                                    }                                
                                }
                            }                        
                        }

                        if (!ok)
                        {
                            throw new AssertException(logger.ToStringTrimNewLine());
                        }

                        StepPassed();
                    }

                },
                () =>
                {
                    _operator.HideMessage();
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });
        }
        
        #endregion

        #region TEST CASES

        #region Access Granted

        [Test(Name = "ACCESS CONTROL – ACCESS GRANTED TO ANONYMOUS EVENT",
            Path = PATHACCESSGRANTEDEVENTS,
            Order = "06.01.01",
            Id = "6-1-1",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessGrantedAnonymousEvent })]
        public void AccessGrantedToAnonymousEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) => 
                        API.Capabilities != null && 
                        API.Capabilities.AnonymousAccessSpecified && 
                        API.Capabilities.AnonymousAccess);

            //tns1:AccessControl/AccessGranted/Anonymous 
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "AccessGranted", "Anonymous" });

            ManualAccessPointEventTest(accessPointCapabilitiesTest, topicInfo, ValidateAccessPointEmptyTopic, ValidateAccessPointEmptyMessage);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS GRANTED WITH CREDENTIAL EVENT",
            Path = PATHACCESSGRANTEDEVENTS,
            Order = "06.01.02",
            Id = "6-1-2",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessGrantedCredentialEvent})]
        public void AccessGrantedWithCredentialsEventTest()
        {

            //tns1:AccessControl/AccessGranted/Credential  
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "AccessGranted", "Credential" });

            ManualAccessPointEventTest(null, topicInfo, ValidateAccessPointCredentialsTopic, ValidateAccessPointCredentialsMessage);
        }

        #endregion

        #region AccessTaken

        [Test(Name = "ACCESS CONTROL – ACCESS TAKEN BY ANONYMOUS EVENT",
            Path = PATHACCESSTAKENEVENTS,
            Order = "07.01.01",
            Id = "7-1-1",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessTakenAnonymousEvent })]
        public void AccessTakenByAnonymousEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) =>
                        API.Capabilities != null &&
                        API.Capabilities.AnonymousAccessSpecified &&
                        API.Capabilities.AnonymousAccess && 
                        API.Capabilities.AccessTakenSpecified && 
                        API.Capabilities.AccessTaken);

            //tns1:AccessControl/AccessTaken/Anonymous 
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "AccessTaken", "Anonymous" });

            // tns1:AccessControl/AccessGranted/Anonymous
            TopicInfo auxilliaryEventTopicInfo = ConstructTopic(new string[] { "AccessControl", "AccessGranted", "Anonymous" });

            // message with empty Data element
            MessageDescription accessGrantedMessageInfo = new MessageDescription();
            // empty
            MessageDescription accessTakenMessageInfo = new MessageDescription();

            ManualAccessPointEventSequenceTest(accessPointCapabilitiesTest, topicInfo,
                ValidateAccessPointEmptyTopic, accessTakenMessageInfo, 
                auxilliaryEventTopicInfo, accessGrantedMessageInfo);

        }

        [Test(Name = "ACCESS CONTROL – ACCESS TAKEN WITH CREDENTIAL EVENT",
            Path = PATHACCESSTAKENEVENTS,
            Order = "07.01.02",
            Id = "7-1-2",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessTakenCredentialEvent })]
        public void AccessTakenWithCredentialsEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) =>
                        API.Capabilities != null &&
                        API.Capabilities.AccessTakenSpecified &&
                        API.Capabilities.AccessTaken);

            //tns1:AccessControl/AccessTaken/Credential 
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "AccessTaken", "Credential" });

            // tns1:AccessControl/AccessGranted/Credential
            TopicInfo auxilliaryEventTopicInfo = ConstructTopic(new string[] { "AccessControl", "AccessGranted", "Credential" });

            // credentials message
            MessageDescription accessGrantedMessageInfo = GetAccessPointCredentialsMessageDescription();
            // credentials message
            MessageDescription accessTakenMessageInfo = GetAccessPointCredentialsMessageDescription();

            ManualAccessPointEventSequenceTest(accessPointCapabilitiesTest, 
                topicInfo, 
                ValidateAccessPointCredentialsTopic, 
                accessTakenMessageInfo,
                auxilliaryEventTopicInfo, 
                accessGrantedMessageInfo);

        }

        #endregion

        #region Access Not Taken

        [Test(Name = "ACCESS CONTROL – ACCESS NOT TAKEN BY ANONYMOUS EVENT",
            Path = PATHACCESSNOTTAKENEVENTS,
            Order = "08.01.01",
            Id = "8-1-1",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessNotTakenAnonymousEvent })]
        public void AccessNotTakenByAnonymousEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) =>
                        API.Capabilities != null &&
                        API.Capabilities.AnonymousAccessSpecified &&
                        API.Capabilities.AnonymousAccess &&
                        API.Capabilities.AccessTakenSpecified &&
                        API.Capabilities.AccessTaken);

            //tns1:AccessControl/AccessNotTaken/Anonymous 
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "AccessNotTaken", "Anonymous" });

            // tns1:AccessControl/AccessGranted/Anonymous
            TopicInfo auxilliaryEventTopicInfo = ConstructTopic(new string[] { "AccessControl", "AccessGranted", "Anonymous" });

            // message with empty Data element
            MessageDescription accessGrantedMessageInfo = new MessageDescription();
            // empty
            MessageDescription accessNotTakenMessageInfo = new MessageDescription();

            ManualAccessPointEventSequenceTest(accessPointCapabilitiesTest, 
                topicInfo, ValidateAccessPointEmptyTopic, accessGrantedMessageInfo, 
                auxilliaryEventTopicInfo, accessNotTakenMessageInfo);

        }

        [Test(Name = "ACCESS CONTROL – ACCESS NOT TAKEN WITH CREDENTIAL EVENT",
            Path = PATHACCESSNOTTAKENEVENTS,
            Order = "08.01.02",
            Id = "8-1-2",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessNotTakenCredentialEvent })]
        public void AccessNotTakenWithCredentialsEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) =>
                        API.Capabilities != null &&
                        API.Capabilities.AccessTakenSpecified &&
                        API.Capabilities.AccessTaken);

            //tns1:AccessControl/AccessNotTaken/Credential 
            TopicInfo topicInfo = ConstructTopic(new string[]{"AccessControl","AccessNotTaken","Credential"});
            
            // tns1:AccessControl/AccessGranted/Credential
            TopicInfo auxilliaryEventTopicInfo = ConstructTopic(new string[]{"AccessControl", "AccessGranted", "Credential"});

            // credentials message
            MessageDescription accessGrantedMessageInfo = GetAccessPointCredentialsMessageDescription();
            // credentials message
            MessageDescription accessTakenMessageInfo = GetAccessPointCredentialsMessageDescription();

            ManualAccessPointEventSequenceTest(accessPointCapabilitiesTest,
                topicInfo,
                ValidateAccessPointCredentialsTopic,
                accessTakenMessageInfo,
                auxilliaryEventTopicInfo,
                accessGrantedMessageInfo);
        }

        #endregion

        #region Access Denied

        #region Anonymous

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED TO ANONYMOUS EVENT (NOT PERMITTED AT THIS TIME)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.01",
            Id = "9-1-1",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedAnonymousNotPermittedAtThisTimeEvent })]
        public void AccessDeniedToAnonymousNotPermittedAtThisTimeEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) =>
                        API.Capabilities != null &&
                        API.Capabilities.AnonymousAccessSpecified &&
                        API.Capabilities.AnonymousAccess);

            //tns1:AccessControl/Denied/Anonymous/NotPermittedAtThisTime 
            // message with single "Reason" simple item of type xs:string in Data
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Anonymous", "NotPermittedAtThisTime" });

            ManualAccessPointEventTest(accessPointCapabilitiesTest, 
                topicInfo, 
                ValidateAccessPointOptionalReasonTopic, ValidateAccessPointOptionalReasonMessage);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED TO ANONYMOUS EVENT (UNAUTHORIZED)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.02",
            Id = "9-1-2",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedAnonymousUnauthorizedEvent })]
        public void AccessDeniedToAnonymousNotAuthorizedEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) =>
                        API.Capabilities != null &&
                        API.Capabilities.AnonymousAccessSpecified &&
                        API.Capabilities.AnonymousAccess);

            //tns1:AccessControl/Denied/Anonymous/Unauthorized  
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Anonymous", "Unauthorized" });

            ManualAccessPointEventTest(accessPointCapabilitiesTest,
                topicInfo,
                ValidateAccessPointOptionalReasonTopic, ValidateAccessPointOptionalReasonMessage);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED TO ANONYMOUS EVENT (OTHER)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.03",
            Id = "9-1-3",
            ExecutionOrder = TestExecutionOrder.First,
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedAnonymousOtherEvent })]
        public void AccessDeniedToAnonymousOtherEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) =>
                        API.Capabilities != null &&
                        API.Capabilities.AnonymousAccessSpecified &&
                        API.Capabilities.AnonymousAccess);

            //tns1:AccessControl/Denied/Anonymous/Other 
            // "Empty" message
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Anonymous", "Other" });

            ManualAccessPointEventTest(accessPointCapabilitiesTest,
                topicInfo,
                ValidateAccessPointEmptyTopic, ValidateAccessPointEmptyMessage);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED TO ANONYMOUS EVENT",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.04",
            Id = "9-1-4",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { })]
        public void AccessDeniedToAnonymousAnyEventTest()
        {
            RunTest(
                () => 
                {
                    List<AccessPointInfo> fullAccessPointsList = GetAccessPointInfoList();

                    List<AccessPointInfo> accessPointList = 
                        fullAccessPointsList.Where(API => 
                            API.Capabilities != null && 
                            API.Capabilities.AnonymousAccessSpecified && 
                            API.Capabilities.AnonymousAccess).ToList();

                    if (accessPointList.Count > 0)
                    {
                        List<XmlElement> topics = GetAllTopics();
                        //tns1:AccessControl/Denied/Anonymous/NotPermittedAtThisTime
                        //tns1:AccessControl/Denied/Anonymous/Unauthorized 
                        //tns1:AccessControl/Denied/Anonymous/Other

                        TopicInfo topicNotPermitted = ConstructTopic(new string[] { "AccessControl", "Denied", "Anonymous", "NotPermittedAtThisTime" });
                        TopicInfo topicUnauthorized = ConstructTopic(new string[] { "AccessControl", "Denied", "Anonymous", "Unauthorized" });
                        TopicInfo topicOther = ConstructTopic(new string[] { "AccessControl", "Denied", "Anonymous", "Other" });

                        bool found = false;
                        foreach (XmlElement el in topics)
                        {
                            TopicInfo info = TopicInfo.ConstructTopicInfo(el);
                            if (TopicInfo.TopicsMatch(info, topicNotPermitted) ||
                                TopicInfo.TopicsMatch(info, topicUnauthorized) ||
                                TopicInfo.TopicsMatch(info, topicOther))
                            {
                                found = true;
                                break;
                            }
                        }

                        string message = "No topics from the following list found: " +
                            Environment.NewLine +
                            "tns1:AccessControl/Denied/Anonymous/NotPermittedAtThisTime" +
                            Environment.NewLine +
                            "tns1:AccessControl/Denied/Anonymous/Unauthorized" +
                            Environment.NewLine +
                            "tns1:AccessControl/Denied/Anonymous/Other" +
                        Environment.NewLine + "tns1=" + ONVIFTOPICSET;

                        Assert(found, message, "Check that at least one of topics of interest is supported");

                    }
                    else
                    {
                        LogTestEvent("No AccessPoints with required Capabilities found, exit the test." + Environment.NewLine);
                    }
                });

        }

        #endregion

        #region Credentials

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL EVENT (CREDENTIAL NOT ENABLED)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.05",
            Id = "9-1-5",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedCredentialCredentialNotEnabledEvent })]
        public void AccessDeniedCredentialsNotEnabledEventTest()
        {
            //tns1:AccessControl/Denied/Credential/CredentialNotEnabled 
            // message with single "Reason" simple item of type xs:string in Data
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "CredentialNotEnabled" });

            ManualAccessPointEventTest(null,
                topicInfo,
                ValidateAccessPointCredentialsReasonTopic, ValidateAccessPointCredentialsReasonMessage);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL EVENT (CREDENTIAL NOT ACTIVE)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.06",
            Id = "9-1-6",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedCredentialCredentialNotActiveEvent })]
        public void AccessDeniedWithCredentialsCredentialsNotActiveEventTest()
        {
            //tns1:AccessControl/Denied/Credential/CredentialNotActive  
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "CredentialNotActive" });

            ManualAccessPointEventTest(null, topicInfo, ValidateAccessPointCredentialsTopic, ValidateAccessPointCredentialsMessage);
        }
        
        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL EVENT (CREDENTIAL EXPIRED)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.07",
            Id = "9-1-7",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedCredentialCredentialExpiredEvent })]
        public void AccessDeniedWithCredentialsCredentialsExpiredEventTest()
        {
            //tns1:AccessControl/Denied/Credential/CredentialExpired  
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "CredentialExpired" });

            ManualAccessPointEventTest(null, topicInfo, ValidateAccessPointCredentialsTopic, ValidateAccessPointCredentialsMessage);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL EVENT (INVALID PIN)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.08",
            Id = "9-1-8",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedCredentialInvalidPINEvent })]
        public void AccessDeniedWithCredentialsInvalidPINEventTest()
        {
            //tns1:AccessControl/Denied/Credential/InvalidPIN  
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "InvalidPIN" });

            ManualAccessPointEventTest(null, topicInfo, ValidateAccessPointCredentialsTopic, ValidateAccessPointCredentialsMessage);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL EVENT (NOT PERMITTED AT THIS TIME)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.09",
            Id = "9-1-9",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedCredentialNotPermittedAtThisTimeEvent })]
        public void AccessDeniedCredentialsNotPermittedAtThisTimeEventTest()
        {
            //tns1:AccessControl/Denied/Credential/NotPermittedAtThisTime 
            // message with single "Reason" simple item of type xs:string in Data
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "NotPermittedAtThisTime" });

            ManualAccessPointEventTest(null,
                topicInfo,
                ValidateAccessPointCredentialsReasonOptionalTopic, ValidateCredentialsReasonOptionalMessage);
        }


        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL EVENT (UNAUTHORIZED)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.10",
            Id = "9-1-10",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedCredentialUnauthorizedEvent })]
        public void AccessDeniedWithCredentialsUnauthorizedEventTest()
        {
            //tns1:AccessControl/Denied/Credential/Unauthorized  
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "Unauthorized" });

            ManualAccessPointEventTest(null, topicInfo, ValidateAccessPointCredentialsTopic, ValidateAccessPointCredentialsMessage);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL EVENT (OTHER)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.11",
            Id = "9-1-11",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedCredentialOtherEvent })]
        public void AccessDeniedWithCredentialsOtherEventTest()
        {
            //tns1:AccessControl/Denied/Credential/Other   
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "Other" });

            ManualAccessPointEventTest(null, topicInfo, ValidateAccessPointCredentialOtherTopic, ValidateAccessPointAccessDeniedCredentialsOtherMessage);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL EVENT (CREDENTIAL NOT FOUND – CARD)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.12",
            Id = "9-1-12",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedCredentialCredentialNotFoundCardEvent })]
        public void AccessDeniedWithCredentialsCredentialsNotFoundCardEventTest()
        {
            //tns1:AccessControl/Denied/Credential/CredentialNotFound/Card   
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "CredentialNotFound", "Card" });

            ManualAccessPointEventTest(null, topicInfo, ValidateAccessPointCredentialsNotFoundCardTopic, ValidateAccessPointAccessDeniedCredentialsNotFoundCardMessage);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL EVENT",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.13",
            Id = "9-1-13",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { })]
        public void AccessDeniedToCredentialsAnyEventTest()
        {
            RunTest(
                () =>
                {
                    List<AccessPointInfo> fullAccessPointsList = GetAccessPointInfoList();
                    
                    List<XmlElement> topics = GetAllTopics();

                    // tns1:AccessControl/Denied/Credential/CredentialNotEnabled 
                    // tns1:AccessControl/Denied/Credential/CredentialNotActive 
                    // tns1:AccessControl/Denied/Credential/CredentialExpired 
                    // tns1:AccessControl/Denied/Credential/InvalidPIN
                    // tns1:AccessControl/Denied/Credential/NotPermittedAtThisTime
                    // tns1:AccessControl/Denied/Credential/Unauthorized
                    // tns1:AccessControl/Denied/Credential/Other
                    // tns1:AccessControl/Denied/Credential/CredentialNotFound/Card
                    List<TopicInfo> topicInfos = new List<TopicInfo>();

                    topicInfos.Add(ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "CredentialNotEnabled" }));
                    topicInfos.Add(ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "CredentialNotActive" }));
                    topicInfos.Add(ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "CredentialExpired" }));
                    topicInfos.Add(ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "InvalidPIN" }));
                    topicInfos.Add(ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "NotPermittedAtThisTime" }));
                    topicInfos.Add(ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "Unauthorized" }));
                    topicInfos.Add(ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "Other" }));
                    topicInfos.Add(ConstructTopic(new string[] { "AccessControl", "Denied", "Credential", "CredentialNotFound", "Card" }));

                    bool found = false;
                    foreach (XmlElement el in topics)
                    {
                        TopicInfo info = TopicInfo.ConstructTopicInfo(el);
                        foreach (TopicInfo inf in topicInfos)
                        {
                            if (TopicInfo.TopicsMatch(info, inf) )
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                    }

                    string message = "No topics from the following list found: " +
                        Environment.NewLine +
                        "tns1:AccessControl/Denied/Credential/CredentialNotEnabled" +
                        Environment.NewLine +
                        "tns1:AccessControl/Denied/Credential/CredentialNotActive" +
                        Environment.NewLine +
                        "tns1:AccessControl/Denied/Credential/CredentialExpired" + 
                        Environment.NewLine +
                        "tns1:AccessControl/Denied/Credential/InvalidPIN" + 
                        Environment.NewLine +
                        "tns1:AccessControl/Denied/Credential/NotPermittedAtThisTime" + 
                        Environment.NewLine +
                        "tns1:AccessControl/Denied/Credential/Unauthorized" + 
                        Environment.NewLine +
                        "tns1:AccessControl/Denied/Credential/Other" + 
                        Environment.NewLine +
                        "tns1:AccessControl/Denied/Credential/CredentialNotFound/Card" + 
                        Environment.NewLine + "tns1="+ONVIFTOPICSET;

                    Assert(found, message, "Check that at least one of topics of interest is supported");

                });

        }



        #endregion

        #endregion

        #region Duress

        [Test(Name = "ACCESS CONTROL – DURESS WITH ANONYMOUS EVENT",
            Path = PATHDURESSEVENTS,
            Order = "10.01.01",
            Id = "10-1-1",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DuressAnonymousEvent })]
        public void DuressWithAnonymousEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) =>
                        API.Capabilities != null &&
                        API.Capabilities.AnonymousAccessSpecified &&
                        API.Capabilities.AnonymousAccess && 
                        API.Capabilities.DuressSpecified &&
                        API.Capabilities.Duress);

            //tns1:AccessControl/Duress/Anonymous 
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Duress", "Anonymous" });

            ManualAccessPointEventTest(accessPointCapabilitiesTest, 
                topicInfo, 
                ValidateAccessPointOptionalReasonTopic, ValidateAccessPointOptionalReasonMessage);
        }

        [Test(Name = "ACCESS CONTROL – DURESS WITH CREDENTIAL EVENT",
            Path = PATHDURESSEVENTS,
            Order = "10.01.02",
            Id = "10-1-2",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DuressCredentialEvent })]
        public void DuressWithCredentialsEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) =>
                        API.Capabilities != null &&
                        API.Capabilities.DuressSpecified &&
                        API.Capabilities.Duress);

            //tns1:AccessControl/Duress/Credential
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Duress", "Credential" });

            ManualAccessPointEventTest(accessPointCapabilitiesTest,
                topicInfo,
                ValidateAccessPointCredentialsReasonOptionalTopic, ValidateCredentialsReasonOptionalMessage);
        }


        #endregion

        #endregion

        #region Topic Validation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageDescription"></param>
        /// <param name="topicInfo"></param>
        /// <remarks>tns1:AccessControl/AccessGranted/Anonymous		
        /// tns1:AccessControl/AccessGranted/Anonymous/External	
        /// tns1:AccessControl/AccessTaken/Anonymous		
        /// tns1:AccessControl/AccessNotTaken/Anonymous		
        /// tns1:AccessControl/Denied/Anonymous/Other		
        /// tns1:AccessControl/Request/Anonymous			
        /// tns1:AccessControl/Request/Timeout/Anonymous	
        /// </remarks>
        void ValidateAccessPointEmptyTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageDescription"></param>
        /// <param name="topicInfo"></param>
        /// <remarks>tns1:AccessControl/AccessGranted/Credential			
        /// tns1:AccessControl/AccessGranted/Credential/External		
        /// tns1:AccessControl/AccessTaken/Credential			
        /// tns1:AccessControl/AccessNotTaken/Credential			
        /// tns1:AccessControl/Denied/Credential/CredentialNotActive	
        /// tns1:AccessControl/Denied/Credential/CredentialExpired		
        /// tns1:AccessControl/Denied/Credential/InvalidPIN			
        /// tns1:AccessControl/Denied/Credential/Unauthorized		
        /// tns1:AccessControl/Request/Credential
        /// </remarks>
        void ValidateAccessPointCredentialsTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetAccessPointCredentialsMessageDescription();
            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }

        void ValidateAccessPointCredentialsOnlyTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetAccessPointCredentialsOnlyMessageDescription();
            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageDescription"></param>
        /// <param name="topicInfo"></param>
        /// <remarks>tns1:AccessControl/Denied/Credential/CredentialNotEnabled
        /// </remarks>
        void ValidateAccessPointCredentialsReasonTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetAccesPointCredentialsReasonMessageDescription();
            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageDescription"></param>
        /// <param name="topicInfo"></param>
        /// <remarks>
        /// tns1:AccessControl/Denied/Credential/NotPermittedAtThisTime
        /// tns1:AccessControl/Duress/Credential
        /// </remarks>
        void ValidateAccessPointCredentialsReasonOptionalTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetAccesPointCredentialsReasonOptionalMessageDescription();
            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageDescription"></param>
        /// <param name="topicInfo"></param>
        /// <remarks>tns1:AccessControl/Denied/Credential/Other</remarks>
        void ValidateAccessPointCredentialOtherTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetAccessDeniedCredentialsOtherMessageDescription();
            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }

        void ValidateAccessDeniedCredentialExternalTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetAccessDeniedCredentialsExternalMessageDescription();
            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageDescription"></param>
        /// <param name="topicInfo"></param>
        /// <remarks>tns1:AccessControl/Denied/Credential/CredentialNotFound/Card</remarks>
        void ValidateAccessPointCredentialsNotFoundCardTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetAccessDeniedCredentialsNotFoundCardMessageDescription();
            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageDescription"></param>
        /// <param name="topicInfo"></param>
        /// <remarks>
        /// tns1:AccessControl/Denied/Anonymous/NotPermittedAtThisTime
        /// tns1:AccessControl/Denied/Anonymous/Unauthorized
        /// tns1:AccessControl/Denied/Anonymous/External
        /// tns1:AccessControl/Duress/Anonymous
        /// </remarks>
        void ValidateAccessPointOptionalReasonTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetAccessPointOptionalReasonMessageDescription();
            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }

        #endregion

        #region Message Validation
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="settings"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        /// <remarks>tns1:AccessControl/AccessGranted/Anonymous		
        /// tns1:AccessControl/AccessGranted/Anonymous/External	
        /// tns1:AccessControl/AccessTaken/Anonymous		
        /// tns1:AccessControl/AccessNotTaken/Anonymous		
        /// tns1:AccessControl/Denied/Anonymous/Other		
        /// tns1:AccessControl/Request/Anonymous			
        /// tns1:AccessControl/Request/Timeout/Anonymous
        /// </remarks>
        bool ValidateAccessPointEmptyMessage(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger)
        {
            MessageDescription messageInfo = new MessageDescription();
            return ValidateAccessPointMessage(notification, settings, logger, messageInfo);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="settings"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        bool ValidateAccessPointCredentialsMessage(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger)
        {
            MessageDescription messageInfo = GetAccessPointCredentialsMessageDescription();
            return ValidateAccessPointMessage(notification, settings, logger, messageInfo);
        }

        bool ValidateAccessPointCredentialsOnlyMessage(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger)
        {
            MessageDescription messageInfo = GetAccessPointCredentialsOnlyMessageDescription();
            return ValidateAccessPointMessage(notification, settings, logger, messageInfo);
        }

        bool ValidateAccessPointCredentialsReasonMessage(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger)
        {
            MessageDescription messageInfo = GetAccesPointCredentialsReasonMessageDescription();
            return ValidateAccessPointMessage(notification, settings, logger, messageInfo);
        }

        bool ValidateCredentialsReasonOptionalMessage(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger)
        {
            MessageDescription messageInfo = GetAccesPointCredentialsReasonOptionalMessageDescription();
            return ValidateAccessPointMessage(notification, settings, logger, messageInfo);
        }

        bool ValidateAccessPointAccessDeniedCredentialsOtherMessage(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger)
        {
            MessageDescription messageInfo = GetAccessDeniedCredentialsOtherMessageDescription();
            return ValidateAccessPointMessage(notification, settings, logger, messageInfo);
        }

        bool ValidateAccessPointAccessDeniedCredentialsExternalMessage(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger)
        {
            MessageDescription messageInfo = GetAccessDeniedCredentialsExternalMessageDescription();
            return ValidateAccessPointMessage(notification, settings, logger, messageInfo);
        }

        bool ValidateAccessPointAccessDeniedCredentialsNotFoundCardMessage(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger)
        {
            MessageDescription messageInfo = GetAccessDeniedCredentialsNotFoundCardMessageDescription();
            return ValidateAccessPointMessage(notification, settings, logger, messageInfo);
        }

        bool ValidateAccessPointOptionalReasonMessage(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger)
        {
            MessageDescription messageInfo = GetAccessPointOptionalReasonMessageDescription();
            return ValidateAccessPointMessage(notification, settings, logger, messageInfo);
        }



        #endregion

        #region Message Descriptions

        // tns1:AccessControl/AccessGranted/Credential			
        // tns1:AccessControl/AccessGranted/Credential/External		
        // tns1:AccessControl/AccessTaken/Credential			
        // tns1:AccessControl/AccessNotTaken/Credential			
        // tns1:AccessControl/Denied/Credential/CredentialNotActive	
        // tns1:AccessControl/Denied/Credential/CredentialExpired		
        // tns1:AccessControl/Denied/Credential/InvalidPIN			
        // tns1:AccessControl/Denied/Credential/Unauthorized		
        // tns1:AccessControl/Request/Credential	
        MessageDescription GetAccessPointCredentialsMessageDescription()
        { 
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE);
            messageInfo.AddSimpleItem("CredentialHolderName", "string", XSNAMESPACE, false);
            return messageInfo;
        }

        MessageDescription GetAccessPointCredentialsOnlyMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE);
            return messageInfo;
        }

        // tns1:AccessControl/Denied/Anonymous/NotPermittedAtThisTime
        // tns1:AccessControl/Denied/Anonymous/Unauthorized
        // tns1:AccessControl/Denied/Anonymous/External
        // tns1:AccessControl/Duress/Anonymous        
        MessageDescription GetAccessPointOptionalReasonMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE, false);
            return messageInfo;
        }

        // tns1:AccessControl/Denied/Credential/CredentialNotEnabled
        MessageDescription GetAccesPointCredentialsReasonMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE);
            messageInfo.AddSimpleItem("CredentialHolderName", "string", XSNAMESPACE, false);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE, true);
            return messageInfo;
        }

        // tns1:AccessControl/Denied/Credential/NotPermittedAtThisTime
        // tns1:AccessControl/Duress/Credential
        MessageDescription GetAccesPointCredentialsReasonOptionalMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE);
            messageInfo.AddSimpleItem("CredentialHolderName", "string", XSNAMESPACE, false);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE, false);
            return messageInfo;
        }

        // tns1:AccessControl/Denied/Credential/External
        MessageDescription GetAccessDeniedCredentialsExternalMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE, false);
            messageInfo.AddSimpleItem("CredentialHolderName", "string", XSNAMESPACE, false);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE, false);
            return messageInfo;
        }

        // tns1:AccessControl/Denied/Credential/Other
        MessageDescription GetAccessDeniedCredentialsOtherMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE, false);
            messageInfo.AddSimpleItem("CredentialHolderName", "string", XSNAMESPACE, false);
            messageInfo.AddSimpleItem("Card", "string", XSNAMESPACE, false);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE, false);
            return messageInfo;
        }

        // tns1:AccessControl/Denied/Credential/CredentialNotFound/Card
        MessageDescription GetAccessDeniedCredentialsNotFoundCardMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            messageInfo.AddSimpleItem("Card", "string", XSNAMESPACE);
            return messageInfo;
        }

        #endregion


    }
}
