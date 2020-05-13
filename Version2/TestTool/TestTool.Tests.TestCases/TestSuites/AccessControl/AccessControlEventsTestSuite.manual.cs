using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TestTool.Proxies.Event;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using System.Xml;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.TestCases.Utils;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Proxies.Event;
using System.ServiceModel;
using TestTool.Tests.TestCases.TestSuites.Events;

using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using EndpointReferenceType = TestTool.Proxies.Event.EndpointReferenceType;

namespace TestTool.Tests.TestCases.TestSuites
{
    partial class AccessControlEventsTestSuite
    {
        private const string PATHACCESSGRANTEDEVENTS = "Access Control\\Access Granted Events";
        private const string PATHACCESSTAKENEVENTS = "Access Control\\Access Taken Events";
        private const string PATHACCESSNOTTAKENEVENTS = "Access Control\\Access Not Taken Events";
        private const string PATHACCESSDENIEDEVENTS = "Access Control\\Access Denied Events";
        private const string PATHDURESSEVENTS = "Access Control\\Duress Events";

        private readonly string[] allowedReasons =
                {
                    "CredentialNotEnabled",
                    "CredentialNotActive",
                    "CredentialExpired",
                    "InvalidPIN",
                    "NotPermittedAtThisTime",
                    "Unauthorized",
                    "Other"
                };


    #region Common manual test
        
        NotificationMessageHolderType[] MyReceiveMessagesNotify(
            string message,
            int timeout,
            TestTool.Proxies.Event.FilterType filter,
            XmlDocument doc
            )
        {
            NotificationMessageHolderType[] NotificationMessage;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;
            EndpointReferenceType subscriptionReference = null;
            Notify notify = null;
            Action action = null;
            if (string.IsNullOrEmpty(message))
            {
                action = new Action(() => { });
            }
            else
            {
                action = new Action(() =>
                    {
                        Operator.ShowMessage(message);
                    });
            }

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
        /*
        NotificationMessageHolderType[] MyReceiveMessagesPullPoint(
            string message,
            int timeout,
            TestTool.Proxies.Event.FilterType filter,
            XmlDocument doc
            )
        {
            EndpointReferenceType subscriptionReference = null;
            int actualTerminationTime = timeout;
            System.DateTime subscribeStarted = System.DateTime.Now;
            NotificationMessageHolderType[] NotificationMessage = null;

            try
            {

                subscriptionReference = CreateStandardSubscription(filter, ref actualTerminationTime);

                if (subscriptionReference == null)
                {
                    return null;
                }

                CreatePullPointSubscriptionClient(subscriptionReference);

                string dump;
                // todo - put dump to doc!!!
                Operator.ShowMessage(message);
                NotificationMessage = GetMessages(subscriptionReference, false, true, true, 1, out dump);
                doc.LoadXml(dump);
            }
            finally
            {
                Operator.HideMessage();
                ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
            }
            return NotificationMessage;
        }*/

        //[04.03.2013] AKS: added parameter message limit and functionality to check if number of notifications in response to PullMessages command
        //exceeds messageLimit
        protected NotificationMessageHolderType[] ReceiveMessagesPullPointFirstMessage(string message,
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
                if (!string.IsNullOrEmpty(message))
                {
                    Operator.ShowMessage(message);
                }
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

        void ManualAccessPointEventTest(
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest,
            TopicInfo topicInfo,
            Action<XmlElement, TopicInfo> validateTopic,
            ValidateMessageFunction validateMessageFunction)
        {
            //EndpointReferenceType subscriptionReference = null;
            //System.DateTime subscribeStarted = System.DateTime.MaxValue;

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
#if false
                    Notify notify = null;
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        subscriptionReference =
                            ReceiveMessages(filter,
                            timeout,
                            new Action(() =>
                            {
                                Operator.ShowMessage(message);
                            }),
                            doc,
                            out notify,
                            out subscribeStarted);
                    }
                    finally
                    {
                        Operator.HideMessage();
                        ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                    }

                    BeginStep("Validate messages");

                    XmlNamespaceManager manager = CreateNamespaceManager(doc);
                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = GetRawElements(notify.NotificationMessage, doc, manager, true);
#else
                    bool UseNotify = UseNotifyToGetEvents;

                    XmlDocument doc = new XmlDocument();
                    NotificationMessageHolderType[] NotificationMessage = null;
                    if (UseNotify)
                    {
                        NotificationMessage = MyReceiveMessagesNotify(
                            message,
                            timeout,
                            filter,
                            doc);
                    }
                    else
                    {
                        NotificationMessage = ReceiveMessagesPullPointFirstMessage(message, filter, 1, doc);
                    }

                    BeginStep("Validate messages");
                    XmlNamespaceManager manager = CreateNamespaceManager(doc);
                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = GetRawElements(NotificationMessage, doc, manager, UseNotify);

#endif
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
                    //Operator.HideMessage();
                    //ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });
        }

        void ManualAccessPointEventTestBis(
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest,
            TopicInfo topicInfo,
            Action<XmlElement, TopicInfo> validateTopic,
            ValidateMessageFunction validateMessageFunction)
        {
            int timeout = _operationDelay / 1000;
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }

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
                    }
                    else
                    {
                        accessPointsList = fullAccessPointsList;
                    }

                    Assert(accessPointsList.Any(),
                           "There is no appropriate Access Points",
                           "Check there is appropriate Access Point");

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

                    bool UseNotify = UseNotifyToGetEvents;
                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = null;
                    SubscriptionHandler Handler = null;
                    // next code is ugly
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

                    Assert(1 == notifications.Count(),
                           !notifications.Any()
                           ?
                           string.Format("The DUT has sent a PullMessagesResponse that contains no notification message.{0}WARNING: may be Operation delay is too low", Environment.NewLine)
                           :
                           "The DUT has sent a PullMessagesResponse that contains more than one notification message.",
                           "Verify that the DUT sends a PullMessagesResponse that contains one notification message.");

                    BeginStep("Validate messages");
                    StringBuilder logger = new StringBuilder();
                    bool ok = true;

                    MessageCheckSettings settings = new MessageCheckSettings
                        {
                            Data = accessPointsList,
                            ExpectedTopic = topicInfo,
                            RawMessageElements = notifications
                        };
                    //settings.NamespaceManager = manager;

                    foreach (NotificationMessageHolderType m in notifications.Keys)
                    {
                        settings.NamespaceManager = CreateNamespaceManager(notifications[m].OwnerDocument);
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
                });
        }

        void ManualAccessPointEventTest_v2Bis(
                Func<AccessPointInfo, bool> accessPointCapabilitiesTest,
                TopicInfo topicInfo,
                Action<XmlElement, TopicInfo> validateTopic,
                ValidateNotificationMessageFunction validateMessageFunction)
        {
            int timeout = _operationDelay / 1000;
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }

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
                    }
                    else
                    {
                        accessPointsList = fullAccessPointsList;
                    }

                    Assert(accessPointsList.Any(),
                           "There is no appropriate Access Points",
                           "Check there is appropriate Access Point");

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
                    bool UseNotify = UseNotifyToGetEvents;


                    Dictionary<NotificationMessageHolderType, XmlElement> Notifications = null;
                    SubscriptionHandler Handler = null;
                    // next code is ugly
                    try
                    {
                        Handler = new SubscriptionHandler(this, UseNotify, GetEventServiceAddress());
                        Handler.Subscribe(filter, actualTerminationTime);

                        Operator.ShowMessage(message);
                        var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                        Handler.WaitMessages(1, pullingCondition, out Notifications);
                    }
                    finally
                    {
                        Operator.HideMessage();
                        SubscriptionHandler.Unsubscribe(Handler);
                    }
                    Assert(null != Notifications && Notifications.Any(),
                           string.Format("No notification messages are received.{0}WARNING: may be Operation delay is too low", Environment.NewLine),
                           "Check that DUT sent any notification messages");

                    BeginStep("Validate messages");

                    StringBuilder logger = new StringBuilder();
                    bool ok = true;

                    MessageCheckSettings settings = new MessageCheckSettings
                        {
                            Data = accessPointsList,
                            ExpectedTopic = topicInfo,
                            RawMessageElements = Notifications
                        };
                    //settings.NamespaceManager = manager;

                    foreach (NotificationMessageHolderType m in Notifications.Keys)
                    {
                        settings.NamespaceManager = CreateNamespaceManager(Notifications[m].OwnerDocument);
                        bool local = validateMessageFunction(topicElement, m, settings, logger);
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
                });
        }
        void ManualAccessPointEventTest_v2(
                Func<AccessPointInfo, bool> accessPointCapabilitiesTest,
                TopicInfo topicInfo,
                Action<XmlElement, TopicInfo> validateTopic,
                ValidateNotificationMessageFunction validateMessageFunction)
        {
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
                    bool UseNotify = UseNotifyToGetEvents;

                    XmlDocument doc = new XmlDocument();
                    NotificationMessageHolderType[] NotificationMessage = null;
                    if (UseNotify)
                    {
                        NotificationMessage = MyReceiveMessagesNotify(
                            message,
                            timeout,
                            filter,
                            doc);
                    }
                    else
                    {
                        NotificationMessage = ReceiveMessagesPullPointFirstMessage(message, filter, 1, doc);
                    }

                    BeginStep("Validate messages");
                    XmlNamespaceManager manager = CreateNamespaceManager(doc);
                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = GetRawElements(NotificationMessage, doc, manager, UseNotify);

                    StringBuilder logger = new StringBuilder();
                    bool ok = true;

                    MessageCheckSettings settings = new MessageCheckSettings();
                    settings.Data = accessPointsList;
                    settings.ExpectedTopic = topicInfo;
                    settings.RawMessageElements = notifications;
                    settings.NamespaceManager = manager;

                    foreach (NotificationMessageHolderType m in notifications.Keys)
                    {
                        bool local = validateMessageFunction(topicElement, m, settings, logger);
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
            System.DateTime TerminationExpectedTime = System.DateTime.Now;
            bool UseNotify = UseNotifyToGetEvents;

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
                    Utils.NotifyServer server = null;
                    string notificationsUri = null;

                    if (UseNotify)
                    {
                        try
                        {
                            server = new NotifyServer(_nic);
                            SetupNotifyServer(server);
                        }
                        catch(Exception e)
                        {
                            Assert(false, e.Message, "Creating subscription failed"); 
                        }
                        notificationsUri = server.GetNotificationUri();
                    };

                    string accessPointToken = null;
                        
                    string message = string.Format("{0}  event is expected!", initiationTopic.GetDescription());

                    Action eventInitiationAction = new Action(() =>
                    {
                        StepPassed();
                        Operator.ShowMessage(message);
                    });

                    EntityListInfo<AccessPointInfo> data = new EntityListInfo<AccessPointInfo>();
                    data.FilteredList = accessPointsList;
                    data.FullList = fullAccessPointsList;

                    NotificationMessageHolderType initiationMessage = null;

                    TerminationExpectedTime = System.DateTime.Now;
                    subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out subscribeStarted);
                    int messageLimit = 1;
                    // receive events
                    {
                        Dictionary<NotificationMessageHolderType, XmlElement> rawElements = null;
                        if (UseNotify)
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
                                Operator.HideMessage();
                            }

                            ValidateNotificationsPacket(server.RawData);

                            ValidateNotifyNotEmpty(notify);

                            if (notify.NotificationMessage.Length > 1)
                            {
                                LogTestEvent("DUT sent more than one notification. Test will be performed for token from the first notification" + Environment.NewLine);
                            }

                            NotificationMessageHolderType notification = notify.NotificationMessage[0];
                            initiationMessage = notification;

                            //XmlElement messageElement = notification.Message;

                            // message validation
                            XmlDocument doc = new XmlDocument();
                            string soapRawPacket = System.Text.Encoding.UTF8.GetString(server.RawData);
                            doc.LoadXml(soapRawPacket);

                            NotificationMessageHolderType theMessage = notify.NotificationMessage[0];

                            BeginStep("Validate message");

                            XmlNamespaceManager manager = CreateNamespaceManager(doc);

                            rawElements = GetRawElements(notify.NotificationMessage,
                               doc,
                               manager,
                               true);
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
                            subscribeStarted = System.DateTime.Now;

                            rawElements = WaitAllMessages(subscriptionReference, messageLimit, ref TerminationExpectedTime);
                        }
                        StringBuilder logger = new StringBuilder();
                        bool ok = true;

                        NotificationMessageHolderType Message = rawElements.First().Key;
                        MessageCheckSettings settings = new MessageCheckSettings();
                        settings.NamespaceManager = CreateNamespaceManager(rawElements[Message].OwnerDocument);
                        //settings.NamespaceManager = manager;
                        settings.ExpectedPropertyOperation = null;
                        settings.ExpectedTopic = initiationTopic;
                        settings.RawMessageElements = rawElements;
                        settings.Data = data;

                        ok = ValidateAccessPointMessage(Message, settings, logger, initiationMessageInfo);
                                                
                        if (!ok)
                        {
                            throw new AssertException(logger.ToStringTrimNewLine());
                        }
                        StepPassed();

                        // message validated

                        // if names are validated, OK will be false by this moment
                        Dictionary<string, string> sourceSimpleItems = Message.Message.GetMessageSourceSimpleItems();
                        accessPointToken = sourceSimpleItems[ACCESSPOINTTOKENSIMPLEITEM];
                    }

                    // receive events - "Real" check
                    {
                        Dictionary<NotificationMessageHolderType, XmlElement> rawElements = null;
                        if (UseNotify)
                        {
                            message = string.Format("{0}  event is expected!", topicUnderTest.GetDescription());

                            BeginStep("Start listening");
                            Notify notify = server.WaitForNotify(eventInitiationAction,
                                timeout * 1000,
                                _semaphore.StopEvent);
                            Operator.HideMessage();

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

                            rawElements = GetRawElements(notify.NotificationMessage,
                               doc,
                               manager,
                               true);
                        }
                        else
                        {
                            rawElements = WaitAllMessages(subscriptionReference, messageLimit, ref TerminationExpectedTime);
                        }
                        StringBuilder logger = new StringBuilder();
                        bool ok = true;

                        NotificationMessageHolderType Message = rawElements.First().Key;
                        MessageCheckSettings settings = new MessageCheckSettings();
                        settings.NamespaceManager = CreateNamespaceManager(rawElements[Message].OwnerDocument);
//                        settings.NamespaceManager = manager;
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

                        ok = ValidateAccessPointMessage(Message, settings, logger, messageInfo);

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
                                        Message.Message.GetMessageDataSimpleItems();

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
                    Operator.HideMessage();
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });
        }

        void ManualAccessPointEventSequenceTestBis(
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest,
            TopicInfo topicUnderTest,
            Action<XmlElement, TopicInfo> validateTopic,
            MessageDescription messageInfo,
            TopicInfo initiationTopic,
            MessageDescription initiationMessageInfo)
        {
            bool UseNotify = UseNotifyToGetEvents;

            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay / 1000;
            SubscriptionHandler Handler = null;

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
                    }
                    else
                    {
                        accessPointsList = fullAccessPointsList;
                    }

                    Assert(accessPointsList.Any(),
                           "There is no appropriate Access Points",
                           "Check there is appropriate Access Point");

                    // Get topic description from the DUT.                
                    XmlElement topicElement = GetTopicElement(topicUnderTest);

                    Assert(topicElement != null,
                        string.Format("Topic {0} not supported", topicUnderTest.GetDescription()),
                        "Check that the event topic is supported");

                    XmlElement messageDescription = topicElement.GetMessageDescription();
                    validateTopic(messageDescription, topicUnderTest);

                    // filter for current test
                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(new TopicInfo[] { topicUnderTest, initiationTopic });

                    string accessPointToken = null;

                    string message = string.Format("{0}  event is expected!", initiationTopic.GetDescription());

                    EntityListInfo<AccessPointInfo> data = new EntityListInfo<AccessPointInfo>();
                    data.FilteredList = accessPointsList;
                    data.FullList = fullAccessPointsList;

                    NotificationMessageHolderType initiationMessage = null;

                    int messageLimit = 1;

                    Handler = new SubscriptionHandler(this, UseNotify, GetEventServiceAddress());
                    Handler.Subscribe(filter, actualTerminationTime);

                    // receive events
                    {
                        Dictionary<NotificationMessageHolderType, XmlElement> rawElements = null;
                        try
                        {
                            Operator.ShowMessage(message);
                            var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                            Handler.WaitMessages(1, pullingCondition, out rawElements);
                        }
                        finally
                        {
                            Operator.HideMessage();
                        }

                        Assert(null != rawElements && rawElements.Any(),
                               string.Format("No notification messages are received.{0}WARNING: may be Operation delay is too low", Environment.NewLine),
                               "Check that DUT sent any notification messages");

                        BeginStep("Validate message");

                        StringBuilder logger = new StringBuilder();
                        bool ok = true;

                        NotificationMessageHolderType Message = rawElements.First().Key;
                        initiationMessage = Message;
                        MessageCheckSettings settings = new MessageCheckSettings
                            {
                                NamespaceManager = CreateNamespaceManager(rawElements[Message].OwnerDocument),
                                ExpectedPropertyOperation = null,
                                ExpectedTopic = initiationTopic,
                                RawMessageElements = rawElements,
                                Data = data
                            };
                        //settings.NamespaceManager = manager;

                        ok = ValidateAccessPointMessage(Message, settings, logger, initiationMessageInfo);

                        if (!ok)
                        {
                            throw new AssertException(logger.ToStringTrimNewLine());
                        }
                        StepPassed();

                        // message validated

                        // if names are validated, OK will be false by this moment
                        Dictionary<string, string> sourceSimpleItems = Message.Message.GetMessageSourceSimpleItems();
                        accessPointToken = sourceSimpleItems[ACCESSPOINTTOKENSIMPLEITEM];
                    }

                    // receive events - "Real" check
                    {
                        message = string.Format("{0}  event is expected!", topicUnderTest.GetDescription());
                        Dictionary<NotificationMessageHolderType, XmlElement> rawElements = null;
                        try
                        {
                            Operator.ShowMessage(message);
                            var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);
                            Handler.WaitMessages(1, pullingCondition, out rawElements);
                        }
                        finally
                        {
                            Operator.HideMessage();
                        }

                        Assert(null != rawElements && rawElements.Any(),
                               string.Format("No notification messages are received.{0}WARNING: may be Operation delay is too low", Environment.NewLine),
                               "Check that DUT sent any notification messages");


                        BeginStep("Validate message");
                        StringBuilder logger = new StringBuilder();
                        bool ok = true;

                        NotificationMessageHolderType Message = rawElements.First().Key;
                        MessageCheckSettings settings = new MessageCheckSettings();
                        settings.NamespaceManager = CreateNamespaceManager(rawElements[Message].OwnerDocument);
                        //                        settings.NamespaceManager = manager;
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

                        ok = ValidateAccessPointMessage(Message, settings, logger, messageInfo);

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
                                        Message.Message.GetMessageDataSimpleItems();

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
                    Operator.HideMessage();
                    SubscriptionHandler.Unsubscribe(Handler);
                });
        }
    #endregion

        #region TEST CASES

        #region Access Granted
/*
        [Test(Name = "ACCESS CONTROL – ACCESS GRANTED TO ANONYMOUS EVENT",
            Path = PATHACCESSGRANTEDEVENTS,
            Order = "06.01.00",
            Id = "6-1-0",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessGrantedAnonymousEvent })]
        public void AccessGrantedToAnonymousEventTestBis()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) => 
                        API.Capabilities != null && 
                        API.Capabilities.AnonymousAccessSpecified && 
                        API.Capabilities.AnonymousAccess);

            //tns1:AccessControl/AccessGranted/Anonymous 
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "AccessGranted", "Anonymous" });

            Action<XmlElement, TopicInfo> validateTopicFunction = 
                (element, description) =>
                    ValidateAccessPointTopicGeneral(element, description, true, false, false, false);

            ValidateNotificationMessageFunction validateMessage =
                (element, notification, settings, logger) =>
                    ValidateAccessPointMessageGeneral(new MessageDescription(), element, notification, settings, logger, true, "false", false);


            //ManualAccessPointEventTest(accessPointCapabilitiesTest, topicInfo, ValidateAccessPointEmptyTopic, ValidateAccessPointEmptyMessage);
            ManualAccessPointEventTest_v2Bis(accessPointCapabilitiesTest, topicInfo, validateTopicFunction, validateMessage);
        }*/

        [Test(Name = "ACCESS CONTROL – ACCESS GRANTED TO ANONYMOUS EVENT",
            Path = PATHACCESSGRANTEDEVENTS,
            Order = "06.01.01",
            Id = "6-1-1",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.AnonymousAccess },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.AccessGrantedAnonymousEvent })]
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

            Action<XmlElement, TopicInfo> validateTopicFunction =
                (element, description) =>
                    ValidateAccessPointTopicGeneral(element, description, true, false, false, false);

            ValidateNotificationMessageFunction validateMessage =
                (element, notification, settings, logger) =>
                    ValidateAccessPointMessageGeneral(new MessageDescription(), element, notification, settings, logger, true, "false", false);


            //ManualAccessPointEventTest(accessPointCapabilitiesTest, topicInfo, ValidateAccessPointEmptyTopic, ValidateAccessPointEmptyMessage);
            ManualAccessPointEventTest_v2Bis(accessPointCapabilitiesTest, topicInfo, validateTopicFunction, validateMessage);
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
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.AccessGrantedCredentialEvent})]
        public void AccessGrantedWithCredentialsEventTest()
        {

            //tns1:AccessControl/AccessGranted/Credential  
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "AccessGranted", "Credential" });
            Action<XmlElement, TopicInfo> validateTopic = (element, info) => ValidateAccessPointCredentialsTopicGeneral(element, info, true, false, false, false);

            ManualAccessPointEventTest_v2Bis(null, topicInfo, validateTopic, ValidateAccessPointCredentialsMessage_v2);
        }

        #endregion

        #region AccessTaken
/*
        [Test(Name = "ACCESS CONTROL – ACCESS TAKEN BY ANONYMOUS EVENT",
            Path = PATHACCESSTAKENEVENTS,
            Order = "07.01.001",
            Id = "7-1-01",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessTakenAnonymousEvent })]
        public void AccessTakenByAnonymousEventTestBis()
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

            ManualAccessPointEventSequenceTestBis(accessPointCapabilitiesTest, topicInfo,
                                               ValidateAccessPointEmptyTopic, accessTakenMessageInfo, 
                                               auxilliaryEventTopicInfo, accessGrantedMessageInfo);

        }*/
        [Test(Name = "ACCESS CONTROL – ACCESS TAKEN BY ANONYMOUS EVENT",
            Path = PATHACCESSTAKENEVENTS,
            Order = "07.01.01",
            Id = "7-1-1",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.AnonymousAccess, Feature.AccessTaken },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.AccessTakenAnonymousEvent })]
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

            ManualAccessPointEventSequenceTestBis(accessPointCapabilitiesTest, topicInfo,
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
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.AccessTaken },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.AccessTakenCredentialEvent })]
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

            ManualAccessPointEventSequenceTestBis(accessPointCapabilitiesTest, 
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
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.AnonymousAccess, Feature.AccessTaken },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.AccessNotTakenAnonymousEvent })]
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

            ManualAccessPointEventSequenceTestBis(accessPointCapabilitiesTest, 
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
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.AccessTaken },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.AccessNotTakenCredentialEvent })]
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

            ManualAccessPointEventSequenceTestBis(accessPointCapabilitiesTest,
                topicInfo,
                ValidateAccessPointCredentialsTopic,
                accessTakenMessageInfo,
                auxilliaryEventTopicInfo,
                accessGrantedMessageInfo);
        }

        #endregion

        #region Access Denied

        #region Anonymous

        //[Test(Name = "ACCESS CONTROL – ACCESS DENIED TO ANONYMOUS EVENT (NOT PERMITTED AT THIS TIME)",
        //    Path = PATHACCESSDENIEDEVENTS,
        //    Order = "09.01.01",
        //    Id = "9-1-1",
        //    Category = Category.ACCESSCONTROL,
        //    Version = 2.1,
        //    ExecutionOrder = TestExecutionOrder.First,
        //    RequiredFeatures = new Feature[] { Feature.AccessControlService },
        //    RequirementLevel = RequirementLevel.Must,
        //    FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedAnonymousNotPermittedAtThisTimeEvent })]
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

        //[Test(Name = "ACCESS CONTROL – ACCESS DENIED TO ANONYMOUS EVENT (UNAUTHORIZED)",
        //    Path = PATHACCESSDENIEDEVENTS,
        //    Order = "09.01.02",
        //    Id = "9-1-2",
        //    Category = Category.ACCESSCONTROL,
        //    Version = 2.1,
        //    ExecutionOrder = TestExecutionOrder.First,
        //    RequiredFeatures = new Feature[] { Feature.AccessControlService },
        //    RequirementLevel = RequirementLevel.Must,
        //    FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedAnonymousUnauthorizedEvent })]
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

        //[Test(Name = "ACCESS CONTROL – ACCESS DENIED TO ANONYMOUS EVENT (OTHER)",
        //    Path = PATHACCESSDENIEDEVENTS,
        //    Order = "09.01.03",
        //    Id = "9-1-3",
        //    ExecutionOrder = TestExecutionOrder.First,
        //    Category = Category.ACCESSCONTROL,
        //    Version = 2.1,
        //    RequiredFeatures = new Feature[] { Feature.AccessControlService },
        //    RequirementLevel = RequirementLevel.Must,
        //    FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.AccessDeniedAnonymousOtherEvent })]
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

        //[Test(Name = "ACCESS CONTROL – ACCESS DENIED TO ANONYMOUS EVENT",
        //    Path = PATHACCESSDENIEDEVENTS,
        //    Order = "09.01.04",
        //    Id = "9-1-4",
        //    Category = Category.ACCESSCONTROL,
        //    Version = 2.1,
        //    RequiredFeatures = new Feature[] { Feature.AccessControlService },
        //    RequirementLevel = RequirementLevel.Must,
        //    FunctionalityUnderTest = new Functionality[] { })]
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

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED TO ANONYMOUS EVENT",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.01",
            Id = "9-1-1",
            ExecutionOrder = TestExecutionOrder.First,
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.AnonymousAccess },
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.TopicFilter, Functionality.AccessDeniedToAnonymousEvent })]
        public void AccessDeniedToAnonymousEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) =>
                        API.Capabilities != null &&
                        API.Capabilities.AnonymousAccessSpecified &&
                        API.Capabilities.AnonymousAccess);

            //tns1:AccessControl/Denied/Anonymous/Other 
            // "Empty" message
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Anonymous" });

            //Optional "External" field and mandatory "Reason" field
            Action<XmlElement, TopicInfo> validateTopic = (element, info) => ValidateAccessPointTopicGeneral(element, info, true, false, true, true);
            ValidateNotificationMessageFunction validateMessage =
                (element, notification, settings, logger) =>
                    ValidateAccessPointMessageGeneral(GetAccessDeniedAnonymousMessageDescription(), element, notification, settings, logger, true, "false", true);

            //ManualAccessPointEventTest_v2(accessPointCapabilitiesTest,
            //    topicInfo,
            //    validateTopic, ValidateAccessPointEmptyMessage);
            ManualAccessPointEventTest_v2Bis(accessPointCapabilitiesTest, topicInfo, validateTopic, validateMessage);
        }

        #endregion

        #region Credentials

        //+
        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL EVENT",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.02",
            Id = "9-1-2",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.TopicFilter, Functionality.AccessDeniedWithCredentialEvent })]
        public void AccessDeniedWithCredentialEventTest()
        {
            //tns1:AccessControl/Denied/Credential/Other   
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Credential" });

            ValidateNotificationMessageFunction validateMessage =
                (element, notification, settings, logger) => 
                    ValidateAccessPointMessageGeneral(GetAccessDeniedCredentialMessageDescription(), element, notification, settings, logger, true, "false", true);

            ManualAccessPointEventTest_v2Bis(null, topicInfo, ValidateAccessPointCredentialEventTopic, validateMessage);
        }
/*
        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL EVENT (CREDENTIAL NOT FOUND – CARD)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.012",
            Id = "9-1-012",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.TopicFilter, Functionality.AccessDeniedCredentialCredentialNotFoundCardEvent })]
        public void AccessDeniedWithCredentialsCredentialsNotFoundCardEventTestBis()
        {
            //tns1:AccessControl/Denied/Credential/CredentialNotFound/Card   
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "CredentialNotFound", "Card" });

            ManualAccessPointEventTestBis(null, topicInfo, ValidateAccessPointCredentialsNotFoundCardTopic, ValidateAccessPointAccessDeniedCredentialsNotFoundCardMessage);
        }*/
        //+
        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL EVENT (CREDENTIAL NOT FOUND – CARD)",
            Path = PATHACCESSDENIEDEVENTS,
            Order = "09.01.03",
            Id = "9-1-3",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.AccessDeniedCredentialCredentialNotFoundCardEvent },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.CreatePullPointSubscription, Functionality.PullMessages, Functionality.TopicFilter, Functionality.AccessDeniedCredentialCredentialNotFoundCardEvent })]
        public void AccessDeniedWithCredentialsCredentialsNotFoundCardEventTest()
        {
            //tns1:AccessControl/Denied/Credential/CredentialNotFound/Card   
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "CredentialNotFound", "Card" });

            ManualAccessPointEventTestBis(null, topicInfo, ValidateAccessPointCredentialsNotFoundCardTopic, ValidateAccessPointAccessDeniedCredentialsNotFoundCardMessage);
        }

        #endregion

        #endregion

        #region Duress

        //+
        /*[Test(Name = "ACCESS CONTROL – DURESS WITH ANONYMOUS EVENT",
            Path = PATHDURESSEVENTS,
            Order = "10.01.01",
            Id = "10-1-1",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DuressAnonymousEvent })]
        */public void DuressWithAnonymousEventTest()
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

        //+
        //[Test(Name = "ACCESS CONTROL – DURESS WITH CREDENTIAL EVENT",
        //    Path = PATHDURESSEVENTS,
        //    Order = "10.01.02",
        //    Id = "10-1-2",
        //    Category = Category.ACCESSCONTROL,
        //    Version = 2.1,
        //    ExecutionOrder = TestExecutionOrder.First,
        //    RequiredFeatures = new Feature[] { Feature.AccessControlService },
        //    RequirementLevel = RequirementLevel.Must,
        //    FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.DuressCredentialEvent })]
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

        [Test(Name = "ACCESS CONTROL – DURESS",
            Path = PATHDURESSEVENTS,
            Order = "10.01.02",
            Id = "10-1-2",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.Duress },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ Functionality.TopicFilter, Functionality.DuressEvent })]
        public void DuressEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                (API) =>
                API.Capabilities != null &&
                API.Capabilities.DuressSpecified &&
                API.Capabilities.Duress;

            //tns1:AccessControl/Duress
            TopicInfo topicInfo = ConstructTopic(new string[] { "AccessControl", "Duress" });

            ManualAccessPointEventTestBis(accessPointCapabilitiesTest,
                topicInfo,
                ValidateAccessPointDuressTopic, ValidateDuressNotificationMessage);
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

        void ValidateAccessPointTopicGeneral(XmlElement messageDescription, TopicInfo topicInfo, 
                                             bool checkExternalField, bool mandatoryExternalField,
                                             bool checkReasonField, bool mandatoryReasonField)
        {
            MessageDescription messageInfo = new MessageDescription {IsProperty = false};
            if (checkExternalField) 
                messageInfo.AddSimpleItem("External", "boolean", XSNAMESPACE, mandatoryExternalField);
            if (checkReasonField)
                messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE, mandatoryReasonField);
            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }

        void ValidateAccessPointCredentialsTopicGeneral(XmlElement messageDescription, TopicInfo topicInfo,
                                                       bool checkExternalField, bool mandatoryExternalField,
                                                       bool checkReasonField, bool mandatoryReasonField)
        {
            MessageDescription messageInfo = new MessageDescription { IsProperty = false };
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE);
            messageInfo.AddSimpleItem("CredentialHolderName", "string", XSNAMESPACE, false);

            if (checkExternalField)
                messageInfo.AddSimpleItem("External", "boolean", XSNAMESPACE, mandatoryExternalField);
            if (checkReasonField)
                messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE, mandatoryReasonField);
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

        void ValidateAccessPointCredentialsTopicWithExternalFlag(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetAccessPointCredentialsWithExternalFlagMessageDescription();
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
        void ValidateAccessPointCredentialEventTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetAccessDeniedCredentialMessageDescription();
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
        /// </remarks>
        void ValidateAccessPointOptionalReasonTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = GetAccessPointOptionalReasonMessageDescription();
            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageDescription"></param>
        /// <param name="topicInfo"></param>
        /// <remarks>
        /// tns1:AccessControl/Duress
        /// </remarks>
        void ValidateAccessPointDuressTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            var messageInfo = new MessageDescription {IsProperty = false};
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE, false);
            messageInfo.AddSimpleItem("CredentialHolderName", "string", XSNAMESPACE, false);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE);

            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }


        #endregion

        #region Message Validation

        bool validateExternalFlag(XmlElement topicElement, NotificationMessageHolderType notification, StringBuilder logger,
                                  string expectedExternalFlagValue)
        {
            bool messageDescriptionHasExternalFlag = false;
            if (null != topicElement.GetMessageDescription()
                && null != topicElement.GetMessageDescription().GetMessageDataSimpleItemDescriptions())
                messageDescriptionHasExternalFlag = topicElement.GetMessageDescription().GetMessageDataSimpleItemDescriptions().ContainsKey("External");

            //No DataSimpleItems in notification message -> no external flag -> valid
            if (null == notification.Message || null == notification.Message.GetMessageData() ||
                null == notification.Message.GetMessageData().GetMessageDataSimpleItems())
                return true;

            var notificationFields = notification.Message.GetMessageData().GetMessageDataSimpleItems();
            if (messageDescriptionHasExternalFlag)
            {
                if (!notificationFields.ContainsKey("External"))
                    return true;

                if (!notificationFields.Contains(new KeyValuePair<string, string>("External", expectedExternalFlagValue)))
                {
                    logger.Append(string.Format("'External' flag in notification is expected with value '{0}'.\nActual value is '{1}'.", expectedExternalFlagValue, notificationFields["External"]));
                    return false;
                }

                return true;
            }
            else
            {
                if (notificationFields.ContainsKey("External"))
                {
                    logger.Append("Event description doesn't contain External flag, but notification does.");
                    return false;
                }
                else
                    return true;
            }
        }
                
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

        bool ValidateAccessPointMessageGeneral(MessageDescription messageTemplate,
                                               XmlElement topicElement,
                                               NotificationMessageHolderType notification,
                                               MessageCheckSettings settings,
                                               StringBuilder logger,
                                               bool checkExternalFieldValue, string expectedExternalFieldValue,
                                               bool checkReasonFieldValue)
        {
            if (!ValidateAccessPointMessage(notification, settings, logger, messageTemplate))
                return false;

            if (checkExternalFieldValue && !validateExternalFlag(topicElement, notification, logger, expectedExternalFieldValue))
                return false;

            if (checkReasonFieldValue)
            {
                var messageDataSimpleItems = (null == notification.Message
                                              ? null
                                              : notification.Message.GetMessageDataSimpleItems());
                if (null != messageDataSimpleItems && messageDataSimpleItems.ContainsKey("Reason"))
                {
                    var actualReason = notification.Message.GetMessageDataSimpleItems()["Reason"];
                    if (!allowedReasons.Contains(actualReason))
                    {
                        logger.Append(
                            string.Format(
                                "SimpleItem 'Reason' exists with actual value '{0}'. Expected one of the following values: '{1}'",
                                actualReason, string.Join(", ", allowedReasons)));
                        return false;
                    }
                }
                else
                {
                    if (null == messageDataSimpleItems) logger.Append("Notification message doesn't contain data simple items");
                    else logger.Append("Notification message doesn't contain data simple item with name 'Reason'");

                    return false;
                }
            }

            return true;
        }

        bool ValidateAccessPointMessageWithExternalFlag(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger,
            string requestedExternalFieldValue)
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.AddSimpleItem("External", "boolean", XSNAMESPACE, true);
            if (!ValidateAccessPointMessage(notification, settings, logger, messageInfo))
                return false;

            var externalFieldValue = notification.Message.GetMessageDataSimpleItems()["External"];
            if (requestedExternalFieldValue != externalFieldValue)
            {
                logger.Append(string.Format("SimpleItem 'External' exists with actual value '{0}'. Expected: '{1}'", externalFieldValue, requestedExternalFieldValue));
                return false;
            }

            return true;
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

        bool ValidateAccessPointCredentialsMessage_v2(XmlElement topicElement,
                                                      NotificationMessageHolderType notification,
                                                      MessageCheckSettings settings,
                                                      StringBuilder logger)
        {
            MessageDescription messageInfo = GetAccessPointCredentialsMessageDescription();
            if (!ValidateAccessPointMessage(notification, settings, logger, messageInfo))
                return false;

            return validateExternalFlag(topicElement, notification, logger, "false");
        }

        bool ValidateAccessPointCredentialsMessageWithMandatoryExternalFlag(NotificationMessageHolderType notification,
                                                                            MessageCheckSettings settings,
                                                                            StringBuilder logger)
        {
            MessageDescription messageInfo = new MessageDescription { IsProperty = false };
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE);
            messageInfo.AddSimpleItem("CredentialHolderName", "string", XSNAMESPACE, false);
            messageInfo.AddSimpleItem("External", "boolean", XSNAMESPACE, true);

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
            MessageDescription messageInfo = GetAccessDeniedCredentialMessageDescription();
            return ValidateAccessPointMessage(notification, settings, logger, messageInfo);
        }

        //bool ValidateAccessPointAccessDeniedCredentialEvent(XmlElement topicElement,
        //                                                    NotificationMessageHolderType notification,
        //                                                    MessageCheckSettings settings,
        //                                                    StringBuilder logger)
        //{
        //    MessageDescription messageInfo = GetAccessDeniedCredentialMessageDescription();
        //    if (!ValidateAccessPointMessage(notification, settings, logger, messageInfo))
        //        return false;

        //    var actualReason = notification.Message.GetMessageDataSimpleItems()["Reason"];
        //    if (!allowedReasons.Contains(actualReason))
        //    {
        //        logger.Append(string.Format("SimpleItem 'Reason' exists with actual value '{0}'. Expected one of the following values: '{1}'", actualReason, string.Join(", ", allowedReasons)));
        //        return false;
        //    }

        //    return validateExternalFlag(topicElement, notification, logger, "false");
        //}

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

        bool ValidateAccessPointMandatoryReasonMessage(NotificationMessageHolderType notification,
                                                       MessageCheckSettings settings,
                                                       StringBuilder logger)
        {
            MessageDescription messageInfo = new MessageDescription()
                {
                    IsProperty = false
                };
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE, true);
            messageInfo.AddSimpleItem("External", "boolean", XSNAMESPACE, true);
            if (!ValidateAccessPointMessage(notification, settings, logger, messageInfo))
                return false;

            var actualReason = notification.Message.GetMessageDataSimpleItems()["Reason"];
            if (!allowedReasons.Contains(actualReason))
            {
                logger.Append(string.Format("SimpleItem 'Reason' exists with actual value '{0}'. Expected one of the following values: '{1}'", actualReason, string.Join(", ", allowedReasons)));
                return false;
            }

            var externalFieldValue = notification.Message.GetMessageDataSimpleItems()["External"];
            if ("true" != externalFieldValue)
            {
                logger.Append(string.Format("SimpleItem 'External' exists with actual value '{0}'. Expected: 'true'", externalFieldValue));
                return false;
            }

            return true;
        }

        bool ValidateDuressNotificationMessage(NotificationMessageHolderType notification,
                                               MessageCheckSettings settings,
                                               StringBuilder logger)
        {
            var messageInfo = new MessageDescription {IsProperty = false};
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE, false);
            messageInfo.AddSimpleItem("CredentialHolderName", "string", XSNAMESPACE, false);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE);

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

        MessageDescription GetAccessPointCredentialsWithExternalFlagMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE);
            messageInfo.AddSimpleItem("CredentialHolderName", "string", XSNAMESPACE, false);
            messageInfo.AddSimpleItem("External", "boolean", XSNAMESPACE);
            return messageInfo;
        }

        MessageDescription GetAccessPointCredentialsOnlyMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            //messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE);
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

        // tns1:AccessControl/Denied/Credential
        MessageDescription GetAccessDeniedCredentialsExternalMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE, true);
            messageInfo.AddSimpleItem("CredentialHolderName", "string", XSNAMESPACE, false);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE, true);
            messageInfo.AddSimpleItem("External", "boolean", XSNAMESPACE, true);
            return messageInfo;
        }

        MessageDescription GetAccessDeniedAnonymousMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE, true);
            messageInfo.AddSimpleItem("External", "boolean", XSNAMESPACE, false);
            return messageInfo;
        }

        // tns1:AccessControl/Denied/Credential/Other
        MessageDescription GetAccessDeniedCredentialMessageDescription()
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = false;
            messageInfo.AddSimpleItem("CredentialToken", "ReferenceToken", PTNAMESPACE);
            messageInfo.AddSimpleItem("CredentialHolderName", "string", XSNAMESPACE, false);
            messageInfo.AddSimpleItem("External", "boolean", XSNAMESPACE, false);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE);
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
