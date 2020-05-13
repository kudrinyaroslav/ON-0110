using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using System.Xml;
using TestTool.Proxies.Event;
using TestTool.Tests.TestCases.Utils;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Onvif;
using System.ServiceModel;
using System.Threading;
using TestTool.Tests.TestCases.TestSuites.Events;

using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using EndpointReferenceType = TestTool.Proxies.Event.EndpointReferenceType;

namespace TestTool.Tests.TestCases.TestSuites
{
    // External authorization tests
    partial class AccessControlEventsTestSuite
    {
        private const string PATHEXTERNALAUTHORIZATION = "Access Control\\External Authorization Events";

        const string CREDENTIALSTOKENSIMPLEITEM = "CredentialToken";
        const string CREDENTIALSHOLDERNAMESIMPLEITEM = "CredentialHolderName";
        
        #region Test Template

        void ExternalAuthorizationTest(
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest,
            TopicInfo accessRequestTopic,
            Action<XmlElement, TopicInfo> validateAccessRequestTopic,
            ValidateMessageFunction validateRequestMessageFunction,
            TopicInfo resultTopic,
            Action<XmlElement, TopicInfo> validateResultTopic,
            ValidateMessageFunction validateResultMessageFunction,
            Action<string, string> reactToRequest)
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
                            LogTestEvent("No AccessPoints with required Capabilities found, exit the test." + Environment.NewLine);
                            return;
                        }
                    }
                    else
                    {
                        accessPointsList = fullAccessPointsList;
                    }

                    // Get topic description from the DUT and check topics support  

                    ValidateTopicsSupport(accessRequestTopic, validateAccessRequestTopic, resultTopic, validateResultTopic);
                                        
                    // filter for current test
                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(new TopicInfo[] { accessRequestTopic, resultTopic});

                    Utils.NotifyServer server = null;
                    try
                    {
                        // create notification listener and subscription
                        server = new NotifyServer(_nic);
                        SetupNotifyServer(server);
                    }
                    catch (Exception e)
                    {
                        Assert(false, e.Message, "Creating subscription failed");
                    }
                    string notificationsUri = server.GetNotificationUri();
                    subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out subscribeStarted);

                    string credentialsToken = string.Empty;
                    string credentialsHolderName = string.Empty;
                    string accessPointToken = GetAccessRequestMessage(server,
                        timeout,
                        accessRequestTopic,
                        validateRequestMessageFunction,
                        fullAccessPointsList,
                        accessPointsList,
                        ref credentialsToken, 
                        ref credentialsHolderName);
                    
                    // receive events - after calling ExternalAuthorization
                    {
                        BeginStep("Start listening");
                        Notify notify = server.WaitForNotify(() => { StepPassed(); reactToRequest(accessPointToken, credentialsToken); },
                            timeout * 1000,
                            _semaphore.StopEvent);

                        RemoveHandlers(server);

                        ValidateNotificationsPacket(server.RawData);
                        
                        ValidateNotifyNotEmpty(notify);

                        if (notify.NotificationMessage.Length > 1)
                        {
                            LogTestEvent("DUT sent more than one notification. Test will be performed for token from the first notification");
                        }
                        NotificationMessageHolderType theMessage = notify.NotificationMessage[0];

                        ValidateResultMessage(notify, theMessage, server.RawData, resultTopic, validateResultMessageFunction, accessPointToken, credentialsToken, credentialsHolderName);
                    }
                },
                () =>
                {
                    Operator.HideMessage();
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });
        }

        void ExternalAuthorizationTestBis(
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest,
            TopicInfo accessRequestTopic,
            Action<XmlElement, TopicInfo> validateAccessRequestTopic,
            ValidateMessageFunction validateRequestMessageFunction,
            TopicInfo resultTopic,
            Action<XmlElement, TopicInfo> validateResultTopic,
            ValidateMessageFunction validateResultMessageFunction,
            Action<string, string> reactToRequest)
        {

            bool UseNotify = UseNotifyToGetEvents;

            SubscriptionHandler Handler = new SubscriptionHandler(this, UseNotify);
            Handler.SetPolicy(new EventsVerifyPolicy(UseNotify));

            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            int timeout = _operationDelay / 1000;

            //EndpointReferenceType subscriptionReference = null;
            //System.DateTime subscribeStarted = System.DateTime.MaxValue;

            //int timeout = 60;

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


                    // Get topic description from the DUT and check topics support  

                    ValidateTopicsSupport(accessRequestTopic, validateAccessRequestTopic, resultTopic, validateResultTopic);

                    // filter for current test
                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(new TopicInfo[] { accessRequestTopic, resultTopic });

                    // create notification listener and subscription
                    //Utils.NotifyServer server = new NotifyServer(_nic);
                    //SetupNotifyServer(server);
                    //string notificationsUri = server.GetNotificationUri();
                    //subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out subscribeStarted);
                    Handler.SetAddress(GetEventServiceAddress());
                    Handler.Subscribe(filter, actualTerminationTime);

                    string credentialsToken = string.Empty;
                    string credentialsHolderName = string.Empty;
                    string accessPointToken = GetAccessRequestMessageBis(Handler,
                        timeout,
                        accessRequestTopic,
                        validateRequestMessageFunction,
                        fullAccessPointsList,
                        accessPointsList,
                        ref credentialsToken,
                        ref credentialsHolderName);

                    // receive events - after calling ExternalAuthorization
                    {
                        Dictionary<NotificationMessageHolderType, XmlElement> Notifications = null;
                        
                        //[29.05.2013] AKS: do External authorization request before sending PullMessages command
                        reactToRequest(accessPointToken, credentialsToken);
                        Handler.GetMessages(1, () => {}, "", out Notifications);

                        /*BeginStep("Start listening");
                        Notify notify = server.WaitForNotify(() => { StepPassed(); reactToRequest(accessPointToken, credentialsToken); },
                            timeout * 1000,
                            _semaphore.StopEvent);

                        RemoveHandlers(server);*/

                        //ValidateNotificationsPacket(server.RawData);
                        Assert(Notifications.Any(),
                               string.Format("No notification messages are received{0}WARNING: may be Operation delay is too low", Environment.NewLine),
                               "Check the DUT sends any notification messages");

                        if (Notifications.Count > 1)
                        {
                            LogTestEvent("DUT sent more than one notification. Test will be performed for token from the first notification");
                        }

                        ValidateResultMessage(Notifications, resultTopic, validateResultMessageFunction, accessPointToken, credentialsToken, credentialsHolderName);
                    }
                },
                () =>
                {
                    Operator.HideMessage();
                    SubscriptionHandler.Unsubscribe(Handler);
                    //ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });
        }
        
        
        /*
        void ExternalAuthorizationTest_v2(
                Func<AccessPointInfo, bool> accessPointCapabilitiesTest,
                TopicInfo accessRequestTopic,
                Action<XmlElement, TopicInfo> validateAccessRequestTopic,
                ValidateMessageFunction validateRequestMessageFunction,
                TopicInfo resultTopic,
                Action<XmlElement, TopicInfo> validateResultTopic,
                ValidateMessageFunction validateResultMessageFunction,
                Action<string, string> reactToRequest)
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
                            LogTestEvent("No AccessPoints with required Capabilities found, exit the test." + Environment.NewLine);
                            return;
                        }
                    }
                    else
                    {
                        accessPointsList = fullAccessPointsList;
                    }

                    // Get topic description from the DUT and check topics support  

                    ValidateTopicsSupport(accessRequestTopic, validateAccessRequestTopic, resultTopic, validateResultTopic);

                    // filter for current test
                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(new TopicInfo[] { accessRequestTopic, resultTopic });

                    // create notification listener and subscription
                    Utils.NotifyServer server = new NotifyServer(_nic);
                    SetupNotifyServer(server);
                    string notificationsUri = server.GetNotificationUri();
                    subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out subscribeStarted);

                    string credentialsToken = string.Empty;
                    string credentialsHolderName = string.Empty;
                    string accessPointToken = GetAccessRequestMessage(server,
                        timeout,
                        accessRequestTopic,
                        validateRequestMessageFunction,
                        fullAccessPointsList,
                        accessPointsList,
                        ref credentialsToken,
                        ref credentialsHolderName);

                    // receive events - after calling ExternalAuthorization
                    {
                        BeginStep("Start listening");
                        Notify notify = server.WaitForNotify(() => { StepPassed(); reactToRequest(accessPointToken, credentialsToken); },
                            timeout * 1000,
                            _semaphore.StopEvent);

                        RemoveHandlers(server);

                        ValidateNotificationsPacket(server.RawData);

                        ValidateNotifyNotEmpty(notify);

                        if (notify.NotificationMessage.Length > 1)
                        {
                            LogTestEvent("DUT sent more than one notification. Test will be performed for token from the first notification");
                        }
                        NotificationMessageHolderType theMessage = notify.NotificationMessage[0];

                        ValidateResultMessage(notify, theMessage, server.RawData, resultTopic, validateResultMessageFunction, accessPointToken, credentialsToken, credentialsHolderName);
                    }
                },
                () =>
                {
                    Operator.HideMessage();
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });
        }
        */
        void ExternalAuthorizationTimeoutTest(
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest,
            TopicInfo accessRequestTopic,
            Action<XmlElement, TopicInfo> validateAccessRequestTopic,
            ValidateMessageFunction validateRequestMessageFunction,
            TopicInfo resultTopic,
            Action<XmlElement, TopicInfo> validateResultTopic,
            ValidateMessageFunction validateResultMessageFunction)
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
                            LogTestEvent("No AccessPoints with required Capabilities found, exit the test." + Environment.NewLine);
                            return;
                        }
                    }
                    else
                    {
                        accessPointsList = fullAccessPointsList;
                    }

                    // Get topic description from the DUT and check topics support  

                    ValidateTopicsSupport(accessRequestTopic, validateAccessRequestTopic,
                        resultTopic, validateResultTopic);
                    
                    // filter for current test
                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(new TopicInfo[] { accessRequestTopic, resultTopic });

                    // create notification listener and subscription
                    Utils.NotifyServer server = null;
                    try
                    {
                        server = new NotifyServer(_nic);
                        SetupNotifyServer(server);
                    }
                    catch (Exception e)
                    {
                        Assert(false, e.Message, "Creating subscription failed");
                    }

                    string notificationsUri = server.GetNotificationUri();
                    subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out subscribeStarted);

                    // get access request message
                    string credentialsToken = string.Empty;
                    string credentialsHolderName = string.Empty;
                    string accessPointToken = GetAccessRequestMessage(server, 
                        timeout, 
                        accessRequestTopic, 
                        validateRequestMessageFunction, 
                        fullAccessPointsList, 
                        accessPointsList, 
                        ref credentialsToken, 
                        ref credentialsHolderName);
                    RemoveHandlers(server);

                    // receive events 
                    {
                        EnsureNotificationProducerClientCreated();

                        LogTestEvent(string.Format("Wait for {0} event{1}", resultTopic.GetDescription(), Environment.NewLine));

                        Utils.NotifyAsyncServer asyncServer = null;
                        try
                        {
                            asyncServer = new NotifyAsyncServer(_nic);
                            SetupNotifyServer2(asyncServer);
                            asyncServer.StartCollecting(_semaphore.StopEvent);
                        }
                        catch (Exception e)
                        {
                            Assert(false, e.Message, "Creating subscription failed");
                        }

                        AutoResetEvent received = new AutoResetEvent(false);
                        asyncServer.NotificationReceived += (data) => received.Set();

                        Renew request = new Renew();
                        request.TerminationTime = "PT60S";

                        // use 50 second instead of 60
                        int subscriptionGuaranteedTimeLeft = (int)(subscribeStarted.AddSeconds(50) - System.DateTime.Now).TotalMilliseconds;
                        if (subscriptionGuaranteedTimeLeft < 0)
                        {
                            subscriptionGuaranteedTimeLeft = 0;
                        }
                        System.DateTime exitTime = System.DateTime.Now.AddMilliseconds(_messageTimeout);

                        try
                        {
                            while (true)
                            {
                                // check if we are still waiting the message
                                int waitTimeLeft =(int)(exitTime - System.DateTime.Now).TotalMilliseconds;
                                if (waitTimeLeft <= 0)
                                {
                                    break;
                                }

                                // compute time for next wait
                                int waitTime = Math.Min(subscriptionGuaranteedTimeLeft, waitTimeLeft);
                                int res = WaitHandle.WaitAny(new WaitHandle[] { received, _semaphore.StopEvent }, waitTime);

                                if (res == WaitHandle.WaitTimeout)
                                {
                                    // no notification received
                                    if (System.DateTime.Now < exitTime)
                                    {
                                        Renew(request);
                                        subscriptionGuaranteedTimeLeft = 50000;
                                        subscribeStarted = System.DateTime.Now;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else if (res == 1)
                                {
                                    asyncServer.StopCollecting();
                                    // Stop event received
                                    throw new StopEventException();
                                }
                                else
                                {
                                    // Notification received
                                    break;
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            throw;
                        }
                        finally
                        {
                            asyncServer.StopCollecting();
                            RemoveHandlers2(asyncServer);
                        }

                        Dictionary<Notify, byte[]> messages = asyncServer.Get();

                        Assert(messages.Count > 0, "No notifications received", "Check if the DUT sent notifications");

                        Notify notify = messages.Keys.First();
                        byte[] rawData = messages[notify];
                        
                        ValidateNotificationsPacket(rawData);

                        ValidateNotifyNotEmpty(notify);

                        NotificationMessageHolderType theMessage = notify.NotificationMessage[0];

                        ValidateResultMessage(notify, 
                            theMessage, 
                            rawData, 
                            resultTopic, 
                            validateResultMessageFunction, 
                            accessPointToken, 
                            credentialsToken, 
                            credentialsHolderName);
                    }
                },
                () =>
                {
                    Operator.HideMessage();
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });
        }

        void ExternalAuthorizationTimeoutTestBis(
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest,
            TopicInfo accessRequestTopic,
            Action<XmlElement, TopicInfo> validateAccessRequestTopic,
            ValidateMessageFunction validateRequestMessageFunction,
            TopicInfo resultTopic,
            Action<XmlElement, TopicInfo> validateResultTopic,
            ValidateMessageFunction validateResultMessageFunction)
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

                    // Get topic description from the DUT and check topics support  

                    ValidateTopicsSupport(accessRequestTopic, validateAccessRequestTopic,
                        resultTopic, validateResultTopic);

                    // filter for current test
                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(new TopicInfo[] { accessRequestTopic, resultTopic });

                    Handler = new SubscriptionHandler(this, UseNotify, GetEventServiceAddress());
                    Handler.Subscribe(filter, actualTerminationTime);

                    // get access request message
                    string credentialsToken = string.Empty;
                    string credentialsHolderName = string.Empty;
                    string accessPointToken = GetAccessRequestMessageBis(Handler,
                                                                         timeout,
                                                                         accessRequestTopic,
                                                                         validateRequestMessageFunction,
                                                                         fullAccessPointsList,
                                                                         accessPointsList,
                                                                         ref credentialsToken,
                                                                         ref credentialsHolderName);
                    //RemoveHandlers(server);

                    // receive events 
                    {
                        LogTestEvent(string.Format("Wait for {0} event{1}", resultTopic.GetDescription(), Environment.NewLine));

                        Dictionary<NotificationMessageHolderType, XmlElement> notifications = null;

                        var pullingCondition = new SubscriptionHandler.WaitFirstNotificationPollingCondition(timeout);

                        Assert(Handler.WaitMessages(1, pullingCondition, out notifications),
                               string.Format("No notification messages are received{0}WARNING: may be Operation delay is too low{0}", Environment.NewLine),
                               "Check the DUT sends any notification messages");

                        ValidateResultMessage(notifications,
                                              resultTopic,
                                              validateResultMessageFunction,
                                              accessPointToken,
                                              credentialsToken,
                                              credentialsHolderName);
                        }
                },
                () =>
                {
                    Operator.HideMessage();
                    SubscriptionHandler.Unsubscribe(Handler);
                });
        }

        #endregion
        
        #region Templates common steps
        
        /// <summary>
        /// Validates that both topics of interest are supported by the DUT. Validates that both topics are correct.
        /// </summary>
        /// <param name="accessRequestTopic">Topic for Request message</param>
        /// <param name="validateAccessRequestTopic">Validation method for Request message topic</param>
        /// <param name="resultTopic">Topic for resulting message</param>
        /// <param name="validateResultTopic">Validation method for Request message topic</param>
        void ValidateTopicsSupport(TopicInfo accessRequestTopic,
            Action<XmlElement, TopicInfo> validateAccessRequestTopic,
            TopicInfo resultTopic,
            Action<XmlElement, TopicInfo> validateResultTopic)
        {
            List<XmlElement> topics = GetAllTopics();

            {

                XmlElement requestTopicElement = GetTopicElement(topics, accessRequestTopic);

                Assert(requestTopicElement != null,
                    string.Format("Topic {0} not supported", accessRequestTopic.GetDescription()),
                    "Check that the event topic is supported");

                XmlElement messageDescription = requestTopicElement.GetMessageDescription();
                validateAccessRequestTopic(messageDescription, accessRequestTopic);
            }

            {
                XmlElement resultTopicElement = GetTopicElement(topics, resultTopic);

                Assert(resultTopicElement != null,
                    string.Format("Topic {0} not supported", resultTopic.GetDescription()),
                    "Check that the event topic is supported");

                XmlElement messageDescription = resultTopicElement.GetMessageDescription();
                validateResultTopic(messageDescription, resultTopic);
            }
        }

        /// <summary>
        /// Receives and validates request message
        /// </summary>
        /// <param name="server">Notification listening server</param>
        /// <param name="timeout">Waiting timeout</param>
        /// <param name="accessRequestTopic">Topic for request message</param>
        /// <param name="validateRequestMessageFunction">Validation method for request message</param>
        /// <param name="fullAccessPointsList">Full list of access points</param>
        /// <param name="accessPointsList">Filtered list of access points</param>
        /// <param name="credentialsToken">[out parameter]CredentialToken, if such SimpleItem is present 
        /// (if not present and this item is mandatory, validation will fail)</param>
        /// <param name="credentialsHolderName">[out parameter]CredentialHolderName, if 
        /// such SimpleItem is present</param>
        /// <returns></returns>
        string GetAccessRequestMessage(NotifyServer server,
            int timeout,
            TopicInfo accessRequestTopic,
            ValidateMessageFunction validateRequestMessageFunction,
            List<AccessPointInfo> fullAccessPointsList,
            List<AccessPointInfo> accessPointsList,
            ref string credentialsToken,
            ref string credentialsHolderName)
        {
            credentialsToken = string.Empty;
            string accessPointToken = null;

            string message = string.Format("{0}  event is expected!", accessRequestTopic.GetDescription());

            Action eventInitiationAction = new Action(() =>
            {
                StepPassed();
                Operator.ShowMessage(message);
            });

            // receive request event
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
                    LogTestEvent("DUT sent more than one notification. Test will be performed for token from the first notification");
                }

                NotificationMessageHolderType notification = notify.NotificationMessage[0];

                BeginStep("Validate message");

                XmlDocument doc = new XmlDocument();
                string soapRawPacket = System.Text.Encoding.UTF8.GetString(server.RawData);
                doc.LoadXml(soapRawPacket);
                XmlNamespaceManager manager = CreateNamespaceManager(doc);

                Dictionary<NotificationMessageHolderType, XmlElement> rawElements =
                    GetRawElements(notify.NotificationMessage, doc, manager, true);

                MessageCheckSettings settings = new MessageCheckSettings();
                settings.NamespaceManager = manager;
                settings.ExpectedPropertyOperation = null;
                settings.ExpectedTopic = accessRequestTopic;
                settings.RawMessageElements = rawElements;

                EntityListInfo<AccessPointInfo> info = new EntityListInfo<AccessPointInfo>();
                info.FilteredList = accessPointsList;
                info.FullList = fullAccessPointsList;
                settings.Data = info;

                StringBuilder logger = new StringBuilder();
                bool ok = true;

                ok = validateRequestMessageFunction(notification, settings, logger);

                XmlElement messageElement = notification.Message;

                if (ok)
                {
                    // if names are duplicated, OK will be false by this moment
                    Dictionary<string, string> sourceSimpleItems = messageElement.GetMessageSourceSimpleItems();
                    accessPointToken = sourceSimpleItems[ACCESSPOINTTOKENSIMPLEITEM];
                }
                if (!ok)
                {
                    throw new AssertException(logger.ToStringTrimNewLine());
                }

                StepPassed();

                // simple items must be OK by that moment
                Dictionary<string, string> dataSimpleItems = messageElement.GetMessageDataSimpleItems();

                if (dataSimpleItems.ContainsKey(CREDENTIALSTOKENSIMPLEITEM))
                {
                    credentialsToken = dataSimpleItems[CREDENTIALSTOKENSIMPLEITEM];
                }
                if (dataSimpleItems.ContainsKey(CREDENTIALSHOLDERNAMESIMPLEITEM))
                {
                    credentialsHolderName = dataSimpleItems[CREDENTIALSHOLDERNAMESIMPLEITEM];
                }
            }

            return accessPointToken;
        }

        string GetAccessRequestMessageBis(SubscriptionHandler Handler,
                                          int timeout,
                                          TopicInfo accessRequestTopic,
                                          ValidateMessageFunction validateRequestMessageFunction,
                                          List<AccessPointInfo> fullAccessPointsList,
                                          List<AccessPointInfo> accessPointsList,
                                          ref string credentialsToken,
                                          ref string credentialsHolderName)
        {
            credentialsToken = string.Empty;
            string accessPointToken = null;

            string message = string.Format("{0}  event is expected!", accessRequestTopic.GetDescription());

            // receive request event
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

                if (!rawElements.Any())
                {
                    Assert(false,
                           string.Format("No notification messages are received{0}WARNING: may be Operation delay is too low", Environment.NewLine),
                                         "Check that DUT sent any notification messages");
                }

                if (rawElements.Count > 1)
                {
                    LogTestEvent("DUT sent more than one notification. Test will be performed for token from the first notification");
                }

                NotificationMessageHolderType notification = rawElements.Keys.First();

                BeginStep("Validate message");

                MessageCheckSettings settings = new MessageCheckSettings
                    {
                        NamespaceManager = CreateNamespaceManager(rawElements.First().Value.OwnerDocument),
                        ExpectedPropertyOperation = null,
                        ExpectedTopic = accessRequestTopic,
                        RawMessageElements = rawElements
                    };

                EntityListInfo<AccessPointInfo> info = new EntityListInfo<AccessPointInfo>
                    {
                        FilteredList = accessPointsList,
                        FullList = fullAccessPointsList
                    };
                settings.Data = info;

                StringBuilder logger = new StringBuilder();
                bool ok = true;

                ok = validateRequestMessageFunction(notification, settings, logger);

                XmlElement messageElement = notification.Message;

                if (ok)
                {
                    // if names are duplicated, OK will be false by this moment
                    Dictionary<string, string> sourceSimpleItems = messageElement.GetMessageSourceSimpleItems();
                    accessPointToken = sourceSimpleItems[ACCESSPOINTTOKENSIMPLEITEM];
                }
                if (!ok)
                {
                    throw new AssertException(logger.ToStringTrimNewLine());
                }

                StepPassed();

                // simple items must be OK by that moment
                Dictionary<string, string> dataSimpleItems = messageElement.GetMessageDataSimpleItems();

                if (dataSimpleItems.ContainsKey(CREDENTIALSTOKENSIMPLEITEM))
                {
                    credentialsToken = dataSimpleItems[CREDENTIALSTOKENSIMPLEITEM];
                }
                if (dataSimpleItems.ContainsKey(CREDENTIALSHOLDERNAMESIMPLEITEM))
                {
                    credentialsHolderName = dataSimpleItems[CREDENTIALSHOLDERNAMESIMPLEITEM];
                }
            }

            return accessPointToken;
        }

        /// <summary>
        /// Validates resulting message
        /// </summary>
        /// <param name="notify">Notify packet</param>
        /// <param name="message">Selected message</param>
        /// <param name="rawData">Raw notification data</param>
        /// <param name="resultTopic">Resulting event topic</param>
        /// <param name="validateResultMessageFunction">Message validation method</param>
        /// <param name="accessPointToken">Expected access point token</param>
        /// <param name="credentialsToken">Expected credentials token</param>
        /// <param name="credentialsHolderName">Expected credentials holder name</param>
        void ValidateResultMessage(Notify notify,
            NotificationMessageHolderType message,
            byte[] rawData,
            TopicInfo resultTopic,
            ValidateMessageFunction validateResultMessageFunction,
            string accessPointToken, string credentialsToken, string credentialsHolderName)
        {
            BeginStep("Validate message");

            XmlDocument doc = new XmlDocument();
            string soapRawPacket = System.Text.Encoding.UTF8.GetString(rawData);
            doc.LoadXml(soapRawPacket);

            XmlNamespaceManager manager = CreateNamespaceManager(doc);

            Dictionary<NotificationMessageHolderType, XmlElement> rawElements =
                GetRawElements(notify.NotificationMessage, doc, manager, true);

            StringBuilder logger = new StringBuilder();
            bool ok = true;

            MessageCheckSettings settings = new MessageCheckSettings();
            settings.NamespaceManager = manager;
            settings.ExpectedPropertyOperation = null;
            settings.ExpectedTopic = resultTopic;
            settings.RawMessageElements = rawElements;
            settings.Data = accessPointToken;

            ok = validateResultMessageFunction(message, settings, logger);

            if (ok)
            {
                Dictionary<string, string> dataSimpleItems = message.Message.GetMessageDataSimpleItems();

                if (dataSimpleItems.ContainsKey(CREDENTIALSTOKENSIMPLEITEM))
                {
                    string actualCredentialsToken = dataSimpleItems[CREDENTIALSTOKENSIMPLEITEM];
                    if (credentialsToken != actualCredentialsToken)
                    {
                        ok = false;
                        logger.Append(string.Format("CredentialToken is incorrect: expected '{0}', actual '{1}'{2}", 
                            credentialsToken, actualCredentialsToken, Environment.NewLine));
                    }
                }
                if (!string.IsNullOrEmpty(credentialsHolderName))
                {
                    if (dataSimpleItems.ContainsKey(CREDENTIALSHOLDERNAMESIMPLEITEM))
                    {
                        string actualCredentialsHolderName = dataSimpleItems[CREDENTIALSHOLDERNAMESIMPLEITEM];
                        if (!string.IsNullOrEmpty(actualCredentialsHolderName))
                        {
                            if (credentialsHolderName != actualCredentialsHolderName)
                            {
                                ok = false;
                                logger.Append(
                                    string.Format("CredentialHolderName is incorrect: expected '{0}', actual '{1}'{2}",
                                    credentialsHolderName, actualCredentialsHolderName, Environment.NewLine));
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

        void ValidateResultMessage(
            Dictionary<NotificationMessageHolderType, XmlElement> rawElements,
            TopicInfo resultTopic,
            ValidateMessageFunction validateResultMessageFunction,
            string accessPointToken, string credentialsToken, string credentialsHolderName)
        {
            BeginStep("Validate message");

            StringBuilder logger = new StringBuilder();
            bool ok = true;

            NotificationMessageHolderType message = rawElements.Keys.First();
            MessageCheckSettings settings = new MessageCheckSettings();
            settings.NamespaceManager = CreateNamespaceManager(rawElements.First().Value.OwnerDocument);
            settings.ExpectedPropertyOperation = null;
            settings.ExpectedTopic = resultTopic;
            settings.RawMessageElements = rawElements;
            settings.Data = accessPointToken;

            ok = validateResultMessageFunction(message, settings, logger);

            if (ok)
            {
                Dictionary<string, string> dataSimpleItems = message.Message.GetMessageDataSimpleItems();

                if (dataSimpleItems.ContainsKey(CREDENTIALSTOKENSIMPLEITEM))
                {
                    string actualCredentialsToken = dataSimpleItems[CREDENTIALSTOKENSIMPLEITEM];
                    if (credentialsToken != actualCredentialsToken)
                    {
                        ok = false;
                        logger.Append(string.Format("CredentialToken is incorrect: expected '{0}', actual '{1}'{2}",
                            credentialsToken, actualCredentialsToken, Environment.NewLine));
                    }
                }
                if (!string.IsNullOrEmpty(credentialsHolderName))
                {
                    if (dataSimpleItems.ContainsKey(CREDENTIALSHOLDERNAMESIMPLEITEM))
                    {
                        string actualCredentialsHolderName = dataSimpleItems[CREDENTIALSHOLDERNAMESIMPLEITEM];
                        if (!string.IsNullOrEmpty(actualCredentialsHolderName))
                        {
                            if (credentialsHolderName != actualCredentialsHolderName)
                            {
                                ok = false;
                                logger.Append(
                                    string.Format("CredentialHolderName is incorrect: expected '{0}', actual '{1}'{2}",
                                    credentialsHolderName, actualCredentialsHolderName, Environment.NewLine));
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
        
        #endregion

        #region Test cases

        #region Anonymous

        [Test(Name = "ACCESS CONTROL – ACCESS GRANTED TO ANONYMOUS (EXTERNAL AUTHORIZATION)",
            Path = PATHEXTERNALAUTHORIZATION,
            Order = "11.01.01",
            Id = "11-1-1",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.ExternalAuthorization, Feature.AnonymousAccess },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ 
                                                           Functionality.TopicFilter, 
                                                           Functionality.AccessGrantedAnonymousEvent, 
                                                           Functionality.RequestAnonymousEvent })]
        public void ExternalAuthorizationAccessGrantedToAnonymousEventTest()
        {
            //Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
            //    (API) =>
            //        API.Capabilities != null &&
            //        API.Capabilities.AnonymousAccessSpecified &&
            //        API.Capabilities.AnonymousAccess &&
            //        API.Capabilities.ExternalAuthorizationSpecified &&
            //        API.Capabilities.ExternalAuthorization;

            //// tns1:AccessControl/Request/Anonymous
            //// This is empty message
            //TopicInfo requestTopicInfo = ConstructTopic(new string[] { "AccessControl", "Request", "Anonymous" });

            ////tns1:AccessControl/AccessGranted/Anonymous/External 
            //// This is empty message
            //TopicInfo resultTopicInfo = ConstructTopic(new string[] { "AccessControl", "AccessGranted", "Anonymous", "External" });

            //Action<string, string> reactToRequest =
            //    (apToken, credentialsToken) => ExternalAuthorization(apToken, null, "Test Access Granted", Decision.Granted);

            //ExternalAuthorizationTest(accessPointCapabilitiesTest,
            //                          requestTopicInfo, ValidateAccessPointEmptyTopic, ValidateAccessPointEmptyMessage,
            //                          resultTopicInfo, ValidateAccessPointEmptyTopic, ValidateAccessPointEmptyMessage, reactToRequest);
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                (API) =>
                    API.Capabilities != null &&
                    API.Capabilities.AnonymousAccessSpecified &&
                    API.Capabilities.AnonymousAccess &&
                    API.Capabilities.ExternalAuthorizationSpecified &&
                    API.Capabilities.ExternalAuthorization;

            // tns1:AccessControl/Request/Anonymous
            // This is empty message
            TopicInfo requestTopicInfo = ConstructTopic(new string[] { "AccessControl", "Request", "Anonymous" });

            //tns1:AccessControl/AccessGranted/Anonymous/External 
            // This is empty message
            TopicInfo resultTopicInfo = ConstructTopic(new string[] { "AccessControl", "AccessGranted", "Anonymous" });
            Action<XmlElement, TopicInfo> validateTopicFunction = (messageDescription, topicInfo_) => ValidateAccessPointTopicGeneral(messageDescription, topicInfo_, true, true, false, false);
            ValidateMessageFunction validateResultMessageFunction = (notification, settings, logger) => ValidateAccessPointMessageWithExternalFlag(notification, settings, logger, "true");

            Action<string, string> reactToRequest =
                (apToken, credentialsToken) => ExternalAuthorization(apToken, null, "Test Access Granted", Decision.Granted);

            ExternalAuthorizationTestBis(accessPointCapabilitiesTest,
                                         requestTopicInfo, ValidateAccessPointEmptyTopic, ValidateAccessPointEmptyMessage,
                                         resultTopicInfo, validateTopicFunction, validateResultMessageFunction, 
                                         reactToRequest);
        }
        
        [Test(Name = "ACCESS CONTROL – ACCESS DENIED TO ANONYMOUS (EXTERNAL AUTHORIZATION)",
            Path = PATHEXTERNALAUTHORIZATION,
            Order = "11.01.02",
            Id = "11-1-2",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.ExternalAuthorization, Feature.AnonymousAccess },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify, */
                Functionality.TopicFilter, Functionality.RequestAnonymousEvent, Functionality.AccessDeniedAnonymousExternalEvent })]
        public void ExternalAuthorizationAccessDeniedToAnonymousEventTest()
        {
            //Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
            //    (API) =>
            //        API.Capabilities != null &&
            //        API.Capabilities.AnonymousAccessSpecified &&
            //        API.Capabilities.AnonymousAccess &&
            //        API.Capabilities.ExternalAuthorizationSpecified &&
            //        API.Capabilities.ExternalAuthorization;

            //// tns1:AccessControl/Request/Anonymous
            //// This is empty message
            //TopicInfo requestTopicInfo = ConstructTopic(new string[] { "AccessControl", "Request", "Anonymous" });

            ////tns1:AccessControl/Denied/Anonymous/External  
            //// This is message with optional Reason simple item
            //TopicInfo resultTopicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Anonymous", "External" });

            //Action<string, string> reactToRequest =
            //    new Action<string, string>((apToken, credentialsToken) =>
            //    { ExternalAuthorization(apToken, null, "Test Access Denied", Decision.Denied); });

            //ExternalAuthorizationTest(accessPointCapabilitiesTest,
            //    requestTopicInfo, ValidateAccessPointEmptyTopic, ValidateAccessPointEmptyMessage,
            //    resultTopicInfo, ValidateAccessPointOptionalReasonTopic, ValidateAccessPointOptionalReasonMessage, reactToRequest);

            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                (API) =>
                    API.Capabilities != null &&
                    API.Capabilities.AnonymousAccessSpecified &&
                    API.Capabilities.AnonymousAccess &&
                    API.Capabilities.ExternalAuthorizationSpecified &&
                    API.Capabilities.ExternalAuthorization;

            // tns1:AccessControl/Request/Anonymous
            // This is empty message
            TopicInfo requestTopicInfo = ConstructTopic(new string[] { "AccessControl", "Request", "Anonymous" });

            //tns1:AccessControl/Denied/Anonymous/External  
            // This is message with optional Reason simple item
            TopicInfo resultTopicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Anonymous" });

            Action<string, string> reactToRequest =
                (apToken, credentialsToken) =>
                    { ExternalAuthorization(apToken, null, "Test Access Denied", Decision.Denied); };

            ExternalAuthorizationTestBis(accessPointCapabilitiesTest,
                                      requestTopicInfo, ValidateAccessPointEmptyTopic, ValidateAccessPointEmptyMessage,
                                      resultTopicInfo, ValidateAccessPointOptionalReasonTopic, ValidateAccessPointMandatoryReasonMessage, reactToRequest);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS TIMEOUT TO ANONYMOUS (EXTERNAL AUTHORIZATION)",
            Path = PATHEXTERNALAUTHORIZATION,
            Order = "11.01.03",
            Id = "11-1-3",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.ExternalAuthorization, Feature.AnonymousAccess },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ 
                Functionality.TopicFilter,Functionality.RequestAnonymousEvent, Functionality.RequestTimeoutEvent })]
        public void ExternalAuthorizationTimeoutForAnonymousEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                (API) =>
                    API.Capabilities != null &&
                    API.Capabilities.AnonymousAccessSpecified &&
                    API.Capabilities.AnonymousAccess &&
                    API.Capabilities.ExternalAuthorizationSpecified &&
                    API.Capabilities.ExternalAuthorization;

            // tns1:AccessControl/Request/Anonymous
            // This is empty message
            TopicInfo requestTopicInfo = ConstructTopic(new string[] { "AccessControl", "Request", "Anonymous" });

            // tns1:AccessControl/Request/Timeout/Anonymous Topic 
            // This is an empty message 
            TopicInfo resultTopicInfo = ConstructTopic(new string[] { "AccessControl", "Request", "Timeout" });

            ExternalAuthorizationTimeoutTestBis(accessPointCapabilitiesTest,
                                                requestTopicInfo, ValidateAccessPointEmptyTopic, ValidateAccessPointEmptyMessage,
                                                resultTopicInfo, ValidateAccessPointEmptyTopic, ValidateAccessPointEmptyMessage);

        }

        #endregion

        #region Credentials

        [Test(Name = "ACCESS CONTROL – ACCESS GRANTED WITH CREDENTIAL (EXTERNAL AUTHORIZATION)",
            Path = PATHEXTERNALAUTHORIZATION,
            Order = "11.01.04",
            Id = "11-1-4",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.ExternalAuthorization },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ 
                Functionality.TopicFilter, Functionality.RequestCredentialEvent, Functionality.AccessGrantedCredentialEvent })]
        public void ExternalAuthorizationAccessGrantedToCredentialsEventTest()
        {
            //Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
            //    new Func<AccessPointInfo, bool>(
            //        (API) =>
            //            API.Capabilities != null &&
            //            API.Capabilities.ExternalAuthorizationSpecified &&
            //            API.Capabilities.ExternalAuthorization);

            //// tns1:AccessControl/Request/Credential
            //// This is a message with credentials
            //TopicInfo requestTopicInfo = ConstructTopic(new string[] { "AccessControl", "Request", "Credential" });

            ////tns1:AccessControl/AccessGranted/Credential/External 
            //// This is a message with credentials
            //TopicInfo resultTopicInfo = ConstructTopic(new string[] { "AccessControl", "AccessGranted", "Credential", "External" });

            //Action<string, string> reactToRequest =
            //    new Action<string, string>((apToken, credentialsToken) =>
            //    { ExternalAuthorization(apToken, credentialsToken, "Test Access Granted", Decision.Granted); });

            //ExternalAuthorizationTest(accessPointCapabilitiesTest,
            //    requestTopicInfo, ValidateAccessPointCredentialsTopic, ValidateAccessPointCredentialsMessage,
            //    resultTopicInfo, ValidateAccessPointCredentialsTopic, ValidateAccessPointCredentialsMessage, reactToRequest);

            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                (API) =>
                    API.Capabilities != null &&
                    API.Capabilities.ExternalAuthorizationSpecified &&
                    API.Capabilities.ExternalAuthorization;

            // tns1:AccessControl/Request/Credential
            // This is a message with credentials
            TopicInfo requestTopicInfo = ConstructTopic(new string[] { "AccessControl", "Request", "Credential" });

            //tns1:AccessControl/AccessGranted/Credential/External 
            // This is a message with credentials
            TopicInfo resultTopicInfo = ConstructTopic(new string[] { "AccessControl", "AccessGranted", "Credential" });
            ValidateMessageFunction validateResultMessageFunction = (notification, settings, logger) => ValidateAccessPointMessageWithExternalFlag(notification, settings, logger, "true");

            Action<string, string> reactToRequest = (apToken, credentialsToken) => ExternalAuthorization(apToken, credentialsToken, "Test Access Granted", Decision.Granted);

            ExternalAuthorizationTestBis(accessPointCapabilitiesTest,
                requestTopicInfo, ValidateAccessPointCredentialsTopic, ValidateAccessPointCredentialsMessage,
                resultTopicInfo, ValidateAccessPointCredentialsTopicWithExternalFlag, validateResultMessageFunction, reactToRequest);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS DENIED WITH CREDENTIAL (EXTERNAL AUTHORIZATION)",
            Path = PATHEXTERNALAUTHORIZATION,
            Order = "11.01.05",
            Id = "11-1-5",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.ExternalAuthorization },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ 
                Functionality.TopicFilter, Functionality.RequestCredentialEvent, Functionality.AccessDeniedWithCredentialEvent })]
        public void ExternalAuthorizationAccessDeniedToCredentialsEventTest()
        {
            //Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
            //    new Func<AccessPointInfo, bool>(
            //        (API) =>
            //            API.Capabilities != null &&
            //            API.Capabilities.ExternalAuthorizationSpecified &&
            //            API.Capabilities.ExternalAuthorization);

            //// tns1:AccessControl/Request/Anonymous
            //// This is a message with credentials
            //TopicInfo requestTopicInfo = ConstructTopic(new string[] { "AccessControl", "Request", "Credential" });

            //// tns1:AccessControl/AccessGranted/Credential
            //// This is message with mandatory Reason simple item
            //TopicInfo resultTopicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Credential" });

            //Action<string, string> reactToRequest =
            //    new Action<string, string>((apToken, credentialsToken) =>
            //    { ExternalAuthorization(apToken, credentialsToken, "Test Access Denied", Decision.Denied); });

            //ExternalAuthorizationTest(accessPointCapabilitiesTest,
            //    requestTopicInfo, ValidateAccessPointCredentialsTopic, ValidateAccessPointCredentialsMessage,
            //    resultTopicInfo, ValidateAccessDeniedCredentialExternalTopic, ValidateAccessPointAccessDeniedCredentialsExternalMessage, reactToRequest);
            
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                (API) =>
                    API.Capabilities != null &&
                    API.Capabilities.ExternalAuthorizationSpecified &&
                    API.Capabilities.ExternalAuthorization;

            // tns1:AccessControl/Request/Anonymous
            // This is a message with credentials
            TopicInfo requestTopicInfo = ConstructTopic(new string[] { "AccessControl", "Request", "Credential" });

            // tns1:AccessControl/AccessGranted/Credential
            // This is message with mandatory Reason simple item
            TopicInfo resultTopicInfo = ConstructTopic(new string[] { "AccessControl", "Denied", "Credential" });

            Action<string, string> reactToRequest =
                new Action<string, string>((apToken, credentialsToken) =>
                { ExternalAuthorization(apToken, credentialsToken, "Test Access Denied", Decision.Denied); });

            ExternalAuthorizationTestBis(accessPointCapabilitiesTest,
                requestTopicInfo, ValidateAccessPointCredentialsTopic, ValidateAccessPointCredentialsMessage,
                resultTopicInfo, ValidateAccessDeniedCredentialExternalTopic, ValidateAccessPointMandatoryReasonMessage, reactToRequest);
        }

        [Test(Name = "ACCESS CONTROL – ACCESS TIMEOUT WITH CREDENTIAL (EXTERNAL AUTHORIZATION)",
            Path = PATHEXTERNALAUTHORIZATION,
            Order = "11.01.06",
            Id = "11-1-6",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService, Feature.ExternalAuthorization },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { /*Functionality.Notify,*/ 
                Functionality.TopicFilter, Functionality.RequestCredentialEvent, Functionality.RequestTimeoutEvent })]
        public void ExternalAuthorizationTimeoutForCredentialsEventTest()
        {
            Func<AccessPointInfo, bool> accessPointCapabilitiesTest =
                new Func<AccessPointInfo, bool>(
                    (API) =>
                        API.Capabilities != null &&
                        API.Capabilities.ExternalAuthorizationSpecified &&
                        API.Capabilities.ExternalAuthorization);

            // tns1:AccessControl/Request/Anonymous
            // This is a message with credentials
            TopicInfo requestTopicInfo = ConstructTopic(new string[] { "AccessControl", "Request", "Credential" });

            // tns1:AccessControl/Request/Timeout/Credential Topic 
            // This is a message with credentials only
            TopicInfo resultTopicInfo = ConstructTopic(new string[] { "AccessControl", "Request", "Timeout" });

            ExternalAuthorizationTimeoutTestBis(accessPointCapabilitiesTest,
                                                requestTopicInfo, ValidateAccessPointCredentialsTopic, ValidateAccessPointCredentialsMessage,
                                                resultTopicInfo, ValidateAccessPointCredentialsOnlyTopic, ValidateAccessPointEmptyMessage);
        }

        #endregion

        #region Negative tests

        [Test(Name = "EXTERNAL AUTHORIZATION WITH INVALID TOKEN",
            Order = "11.01.07",
            Id = "11-1-7",
            Category = Category.ACCESSCONTROL,
            Path = PATHEXTERNALAUTHORIZATION,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.ExternalAutorization })]
        public void ExternalAuthorizationInvalidTokenTest()
        {
            RunTest(() =>
            {
                PACSPortClient client = PACSPortClient;

                string token = Guid.NewGuid().ToString().Substring(0,8);
                RunStep(
                    () => { PACSPortClient.ExternalAuthorization(token, null, "Test Access Granted", Decision.Granted); },
                    "External Authorization with invalid token", 
                    OnvifFaults.NotFound, 
                    true);
            });
        }

        [Test(Name = "EXTERNAL AUTHORIZATION – COMMAND NOT SUPPORTED",
            Order = "11.01.08",
            Id = "11-1-8",
            Category = Category.ACCESSCONTROL,
            Path = PATHEXTERNALAUTHORIZATION,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.ExternalAutorization })]
        public void ExternalAuthorizationCommandNotSupportedTest()
        {
            RunTest(() =>
            {
                List<AccessPointInfo> accessPointInfos = GetAccessPointInfoList();

                List<AccessPointInfo> filtered = 
                    accessPointInfos.Where(API => 
                        API.Capabilities != null && 
                        !(API.Capabilities.ExternalAuthorizationSpecified && 
                        API.Capabilities.ExternalAuthorization)).ToList();

                foreach (AccessPointInfo info in filtered)
                {
                    RunStep(
                        () => { PACSPortClient.ExternalAuthorization(info.token, null, "Test Access Granted", Decision.Granted); },
                        "External Authorization", "Receiver/ActionNotSupported/NotSupported", true);
                }
            });
        }

        #endregion
        
        #endregion

        #region Topic Validation


        #endregion

        #region Message Validation


        #endregion

        #region Message Descriptions


        #endregion


        #region Steps

        void ExternalAuthorization(string accessPointToken, string credentialToken, string reason, Decision decision)
        {
            PACSPortClient client = PACSPortClient;
            RunStep(
                () => 
                { PACSPortClient.ExternalAuthorization(accessPointToken, credentialToken, reason, decision); }, 
                "External Authorizaton");
            DoRequestDelay();
        }


        #endregion


    }
}
