using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Common.Trace;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Engine.Base;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.Utils;
using TestTool.Proxies.Event;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class NotificationsTestSuite : EventTest
    {
        public NotificationsTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        private const string PATHBASIC = "Event Handling\\Basic Notification";
        private const string PATHPULLPOINT = "Event Handling\\Real-Time Pull-Point Notification Interface";

        [Test(Name = "BASIC NOTIFICATION INTERFACE - NOTIFY",
            Path = PATHBASIC,
            Order = "02.01.07",
            Id = "2-1-7",
            Category = Category.EVENT,
            ExecutionOrder = TestExecutionOrder.First,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.EventsSetSynchronizationPoint })]
        public void NotifyTest()
        {
            EndpointReferenceType subscriptionReference = null;
            bool subscribeStarted = false;
            int timeout = 60;

            RunTest<object>(
                new Backup<object>(() =>
                {
                    return null;
                }),
            () =>
            {
                TestTool.Proxies.Event.TopicSetType topicSet = GetTopicSet();

                if (topicSet == null || topicSet.Any == null || topicSet.Any.Length == 0)
                {
                    LogTestEvent("The DUT provides no topics. Test passed.");
                    return;
                }

                List<XmlElement> topics = new List<XmlElement>();
                foreach (XmlElement element in topicSet.Any)
                {
                    FindTopics(element, topics);
                }

                List<TopicInfo> propertyTopics = FindPropertyTopics(topics);

                bool bPropertyTopicFound = propertyTopics.Count > 0;

                Utils.NotifyServer server = new NotifyServer(_nic);
                SetupNotifyServer(server);


                if (_eventSubscriptionTimeout != 0)
                {
                    timeout = _eventSubscriptionTimeout; 
                }
                LogTestEvent(string.Format("Timeout of {0} seconds will be used{1}", timeout, Environment.NewLine));
                
                subscribeStarted = true;

                Proxies.Event.Subscribe subscribeRequest = new Subscribe();

                subscribeRequest.InitialTerminationTime = string.Format("PT{0}S", timeout);

                subscribeRequest.Filter = null;

                subscribeRequest.ConsumerReference = new EndpointReferenceType();
                subscribeRequest.ConsumerReference.Address = new AttributedURIType();
                subscribeRequest.ConsumerReference.Address.Value = server.GetNotificationUri();

                EnsureNotificationProducerClientCreated();
                
                if (!bPropertyTopicFound)
                {
                    bool bContinue =
                        _operator.GetOkCancelAnswer("No property events found. You'll have to trigger an event manually.");

                    Assert(bContinue, "Operator cancelled the test", "Check if test should be continued");
                }

                SubscribeResponse subscribeResponse = null;
                RunStep(
                       () =>
                           {
                               subscribeResponse = _notificationProducerClient.Subscribe(subscribeRequest);
                           },
                       "Subscribe",
                       new ValidateTypeFault(ValidateSubscriptionFault));

                if (subscribeResponse == null)
                {
                    return;
                }

                ValidateSubscribeResponse(subscribeResponse, timeout);

                CreateSubscriptionManagerClient(subscribeResponse.SubscriptionReference);

                CreatePullPointSubscriptionClient(subscribeResponse.SubscriptionReference);

                Action setSynchronizationPointAction = bPropertyTopicFound ? 
                    new Action( () => { StepPassed(); SetSynchronizationPoint();}) :
                    new Action(() => { StepPassed(); });

                BeginStep("Start listening");
                Notify notify = server.WaitForNotify(setSynchronizationPointAction, timeout * 1000, _semaphore.StopEvent);
                
                ValidateNotificationsPacket(server.RawData);

                XmlDocument doc = LoadNotificationsAsXml(server.RawData);

                ValidateNotifyNotEmpty(notify);

                ValidateMessages(notify.NotificationMessage,
                    doc,
                    true,
                    null,
                    topicSet);
            },
            (o) =>
            {
                if (subscribeStarted)
                {
                    if (subscriptionReference != null && _subscriptionManagerClient == null)
                    {
                        CreateSubscriptionManagerClient(subscriptionReference);
                    }
                    ReleaseSubscriptionManager(timeout*1000);

                }
            });
        }
        
        [Test(Name = "BASIC NOTIFICATION INTERFACE - NOTIFY FILTER",
            Path = PATHBASIC,
            Order = "02.01.08",
            Id = "2-1-8",
            Category = Category.EVENT,
            ExecutionOrder = TestExecutionOrder.First,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.TopicFilter, Functionality.EventsSetSynchronizationPoint })]
        public void NotifyFilterTest()
        {
            EndpointReferenceType subscriptionReference = null;
            bool subscribeStarted = false;

            int timeout = 60;

            RunTest(() =>
            {

                TestTool.Proxies.Event.TopicSetType topicSet = GetTopicSet();

                if (topicSet == null || topicSet.Any == null || topicSet.Any.Length == 0)
                {
                    LogTestEvent("The DUT provides no topics. Test passed.");
                    return;
                }

                List<XmlElement> topics = new List<XmlElement>();
                foreach (XmlElement element in topicSet.Any)
                {
                    FindTopics(element, topics);
                }

                List<TopicInfo> propertyTopics = FindPropertyTopics(topics);

                bool bPropertyTopicFound = propertyTopics.Count > 0;
                
                EventsTopicInfo plainTopicInfo = new EventsTopicInfo(){Topic = _eventTopic, NamespacesDefinition = _topicNamespaces};
                

                BeginStep("Parse topic");
                
                string stepDetails = string.Format("Topic: {0}; Namespaces: {1}",
                                                   plainTopicInfo.Topic,
                                                   plainTopicInfo.NamespacesDefinition.Replace(Environment.NewLine, " "));
                LogStepEvent(stepDetails);

                TopicInfo topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);
                if (topicInfo == null)
                {
                    throw new AssertException("Failed to parse topic information");
                }
                XmlElement topicElement = FindTopic(topicSet, topicInfo);
                if (topicElement == null)
                {
                    LogStepEvent("WARNING: topic not found");
                }
                StepPassed();
                
                FilterInfo filter = new FilterInfo();

                filter.Filter = new FilterType();

                XmlDocument filterDoc = new XmlDocument();
                XmlElement filterTopicElement = filterDoc.CreateTopicElement();

                string topicPath = TopicInfo.CreateTopicPath(filterTopicElement, topicInfo);

                filterTopicElement.InnerText = topicPath;

                filter.Filter.Any = new XmlElement[] { filterTopicElement };

                filter.Topic = topicInfo;
                if (topicElement != null)
                {
                    filter.MessageDescription = topicElement.GetMessageDescription();
                }

                Utils.NotifyServer server = new NotifyServer(_nic);

                SetupNotifyServer(server);

                if (_eventSubscriptionTimeout != 0)
                {
                    timeout = _eventSubscriptionTimeout;
                }

                LogTestEvent(string.Format("Timeout of {0} seconds will be used{1}", timeout, Environment.NewLine));
                
                subscribeStarted = true;

                Proxies.Event.Subscribe subscribeRequest = new Subscribe();

                subscribeRequest.InitialTerminationTime = string.Format("PT{0}S", timeout);

                subscribeRequest.Filter = filter.Filter;

                subscribeRequest.ConsumerReference = new EndpointReferenceType();
                subscribeRequest.ConsumerReference.Address = new AttributedURIType();
                subscribeRequest.ConsumerReference.Address.Value = server.GetNotificationUri();

                EnsureNotificationProducerClientCreated();
                
                if (!bPropertyTopicFound)
                {
                    bool bContinue =
                        _operator.GetOkCancelAnswer("No property events found. You'll have to trigger an event manually.");

                    Assert(bContinue, "Operator cancelled the test", "Check if test should be continued");
                }

                SubscribeResponse subscribeResponse = null;
                RunStep(
                       () =>
                       {
                           subscribeResponse = _notificationProducerClient.Subscribe(subscribeRequest);
                       },
                       "Subscribe",
                       new ValidateTypeFault(ValidateFilterSubscriptionFault));

                if (subscribeResponse == null)
                {
                    return;
                }

                ValidateSubscribeResponse(subscribeResponse, timeout);

                CreateSubscriptionManagerClient(subscribeResponse.SubscriptionReference);

                CreatePullPointSubscriptionClient(subscribeResponse.SubscriptionReference);

                Action setSynchronizationPointAction = bPropertyTopicFound ?
                    new Action(() => {  StepPassed(); SetSynchronizationPoint(); }) :
                    new Action(() => { StepPassed(); });

                BeginStep("Start listening");
                Notify notify = server.WaitForNotify(setSynchronizationPointAction,
                    timeout * 1000,
                    _semaphore.StopEvent);
               

                ValidateNotificationsPacket(server.RawData);

                string dump = System.Text.Encoding.UTF8.GetString(server.RawData);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dump);

                ValidateNotifyNotEmpty(notify);

                ValidateMessages(notify.NotificationMessage, 
                    doc, 
                    true, 
                    filter, 
                    topicSet);

            },
            () =>
            {
                if (subscribeStarted)
                {
                    if (subscriptionReference != null && _subscriptionManagerClient == null)
                    {
                        CreateSubscriptionManagerClient(subscriptionReference);
                    }
                    ReleaseSubscriptionManager(timeout * 1000);
                }
            });

        }
        
        [Test(Name = "REALTIME PULLPOINT SUBSCRIPTION - PULLMESSAGES",
            Path = PATHPULLPOINT,
            Order = "03.01.07",
            Id = "3-1-7",
            Category = Category.EVENT,
            ExecutionOrder = TestExecutionOrder.First,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.PullMessages, Functionality.EventsSetSynchronizationPoint })]
        public void PullMessagesTest()
        {
            EndpointReferenceType endpointReference = null;
            bool subscribeStarted = false;
            int actualTerminationTime = 60;

            RunTest(() =>
                {
                    TestTool.Proxies.Event.TopicSetType topicSet = GetTopicSet();

                    if (topicSet == null || topicSet.Any == null || topicSet.Any.Length == 0)
                    {
                        LogTestEvent("The DUT provides no topics. Test passed.");
                        return;
                    }

                    List<XmlElement> topics = new List<XmlElement>();
                    foreach (XmlElement element in topicSet.Any)
                    {
                        FindTopics(element, topics);
                    }

                    List<TopicInfo> propertyTopics = FindPropertyTopics(topics);

                    bool bPropertyTopicFound = propertyTopics.Count > 0;
                    
                    if (_eventSubscriptionTimeout != 0)
                    {
                        actualTerminationTime = _eventSubscriptionTimeout;
                    }

                    LogTestEvent(string.Format("Timeout of {0} seconds will be used{1}", actualTerminationTime, Environment.NewLine));
                    
                    if (!bPropertyTopicFound)
                    {
                        bool bContinue =
                            _operator.GetOkCancelAnswer("No property events found. You'll have to trigger an event manually.");

                        Assert(bContinue, "Operator cancelled the test", "Check if test should be continued");
                    }

                    subscribeStarted = true;

                    endpointReference = CreateStandardSubscription(null, ref actualTerminationTime);

                    if (endpointReference == null)
                    {
                        return;
                    }

                    CreatePullPointSubscriptionClient(endpointReference);

                    string dump;

                    NotificationMessageHolderType[] notificationMessages = GetMessages(endpointReference, bPropertyTopicFound, out dump);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(dump);
                    
                    ValidateMessages(notificationMessages, 
                        doc, 
                        false, 
                        null, 
                        topicSet);

                },
                () =>
                {
                    if (endpointReference != null && subscribeStarted)
                    {
                        if (endpointReference.Address != null && _subscriptionManagerClient == null)
                        {
                            CreateSubscriptionManagerClient(endpointReference);
                        }
                        ReleaseSubscriptionManager(actualTerminationTime *1000);
                    }
                });
        }

        [Test(Name = "REALTIME PULLPOINT SUBSCRIPTION - PULLMESSAGES FILTER",
            Path = PATHPULLPOINT,
            Order = "03.01.08",
            Id = "3-1-8",
            Category = Category.EVENT,
            ExecutionOrder = TestExecutionOrder.First,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.PullMessages, Functionality.TopicFilter, Functionality.EventsSetSynchronizationPoint })]
        public void PullMessagesFilterTest()
        {
            EndpointReferenceType endpointReference = null;
            int actualTerminationTime = 60;

            RunTest(() =>
                {
                    bool callSetSyncPoint = true;

                    // Find all topics
                    TestTool.Proxies.Event.TopicSetType topicSet = GetTopicSet();

                    if (topicSet == null || topicSet.Any == null || topicSet.Any.Length == 0)
                    {
                        LogTestEvent("The DUT provides no topics. Test passed.");
                        return;
                    }

                    List<XmlElement> topics = new List<XmlElement>();
                    foreach (XmlElement element in topicSet.Any)
                    {
                        FindTopics(element, topics);
                    }

                    List<TopicInfo> propertyTopics = FindPropertyTopics(topics);

                    bool bPropertyTopicFound = propertyTopics.Count > 0;
                    
                    EventsTopicInfo plainTopicInfo = new EventsTopicInfo() { Topic = _eventTopic, NamespacesDefinition = _topicNamespaces };
                    
                    BeginStep("Parse topic");

                    string stepDetails = string.Format("Topic: {0}; Namespaces: {1}",
                                                       plainTopicInfo.Topic,
                                                       plainTopicInfo.NamespacesDefinition.Replace(Environment.NewLine, ""));
                    LogStepEvent(stepDetails);

                    TopicInfo topicInfo = TopicInfo.ConstructTopicInfo(plainTopicInfo);
                    if (topicInfo == null)
                    {
                        throw new AssertException("Failed to parse topic information");
                    }
                    XmlElement topicElement = FindTopic(topicSet, topicInfo);
                    if (topicElement == null)
                    {
                        LogStepEvent("WARNING: topic not found");
                    }
                    StepPassed();

                    // Create filter
                    FilterInfo filterInfo = new FilterInfo();

                    filterInfo.Filter = new FilterType();

                    XmlDocument filterDoc = new XmlDocument();
                    XmlElement filterTopicElement = filterDoc.CreateTopicElement();

                    string topicPath = TopicInfo.CreateTopicPath(filterTopicElement, topicInfo);

                    filterTopicElement.InnerText = topicPath;

                    filterInfo.Filter.Any = new XmlElement[] { filterTopicElement };

                    filterInfo.Topic = topicInfo;

                    if (topicElement != null)
                    {
                        filterInfo.MessageDescription = topicElement.GetMessageDescription();
                    }  
                  
                    FilterType filter = filterInfo.Filter;

                    /* Create Pull Point Subscription */

                    if (_eventSubscriptionTimeout != 0)
                    {
                        actualTerminationTime = _eventSubscriptionTimeout;
                    }

                    LogTestEvent(string.Format("Timeout of {0} seconds will be used{1}", actualTerminationTime, Environment.NewLine));
                    
                    if (!bPropertyTopicFound)
                    {
                        bool bContinue =
                            _operator.GetOkCancelAnswer("No property events found. You'll have to trigger an event manually.");

                        Assert(bContinue, "Operator cancelled the test", "Check if test should be continued");
                    }

                    endpointReference =
                        CreateStandardSubscription(filter, ref actualTerminationTime);

                    if (endpointReference == null)
                    {
                        return;
                    }

                    CreatePullPointSubscriptionClient(endpointReference);

                    string dump;
                    // Pull messages
                    NotificationMessageHolderType[] notificationMessages = GetMessages(endpointReference, callSetSyncPoint, out dump);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(dump);

                    ValidateMessages(notificationMessages, 
                        doc, 
                        false, 
                        filterInfo, 
                        topicSet);
                },
                () =>
                {
                    if (endpointReference != null)
                    {
                        if (endpointReference.Address != null && _subscriptionManagerClient == null)
                        {
                            CreateSubscriptionManagerClient(endpointReference);
                        }
                        ReleaseSubscriptionManager(actualTerminationTime * 1000);
                    }
                });
        }

        #region Subscribe; Get Messages

        /// <summary>
        /// Sets up NotifyServer for 9.2.7 and 9.2.8 tests.
        /// </summary>
        /// <param name="server">Notification listener.</param>
        protected void SetupNotifyServer(Utils.NotifyServer server)
        {
            server.WaitStarted += BeginStartListeningStep;

            server.WaitFinished += StepPassed;

            server.NotificationReceived += LogNotifications;

            server.Timeout += ThrowTimeoutException;
        }

        protected void RemoveHandlers(Utils.NotifyServer server)
        {
            server.WaitStarted -= BeginStartListeningStep;
            server.WaitFinished -= StepPassed;
            server.NotificationReceived -= LogNotifications;
            server.Timeout -= ThrowTimeoutException;
        }

        void LogNotifications(byte[] data)
        { 
            string content =
                System.Text.Encoding.UTF8.GetString(data);
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                XmlDocument document = new XmlDocument();
                document.LoadXml(content);
                document.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                TextReader rdr = new StreamReader(memoryStream);
                content = rdr.ReadToEnd();
                rdr.Close();
            }
            catch (Exception exc)
            {
                // log responce "as is"
            }
            LogResponse(content);        
        }

        void BeginStartListeningStep()
        {
            BeginStep("Wait for notification");
        }

        void ThrowTimeoutException()
        {
            throw new ApplicationException("No notification received!");
        }

        /// <summary>
        /// "Standard" subscription for 9.3.4 - 9.3.7 tests.
        /// </summary>
        /// <returns></returns>
        EndpointReferenceType CreateStandardSubscription(FilterType filter,
            ref int terminationTimeSeconds)
        {
            string terminationTimeString = string.Format("PT{0}S", terminationTimeSeconds);

            XmlElement[] any = null;
            DateTime currentTime = DateTime.MinValue;
            DateTime? terminationTime = null;

            EndpointReferenceType endpointReference = null;
            EnsureEventPortTypeClientCreated();

            ValidateTypeFault validateMethod = filter == null
                                                   ? new ValidateTypeFault(ValidateSubscriptionFault)
                                                   : new ValidateTypeFault(ValidateFilterSubscriptionFault);
            RunStep( () =>
                         {
                             endpointReference = _eventPortTypeClient.CreatePullPointSubscription(
                                 filter,
                                 terminationTimeString,
                                 null,
                                 ref any,
                                 out currentTime,
                                 out terminationTime);
                         },
                         "Create Pull Point Subsciption", 
                         validateMethod
                         );
            
            if (endpointReference == null)
            {
                return null;
            }

            Assert(terminationTime.HasValue, "TerminationTime is not specified",
                   "Check that TerminationTime is specified");

            Assert(currentTime.AddSeconds(terminationTimeSeconds) <= terminationTime.Value,
                "TerminationTime < CurrentTime + InitialTerminationTime",
                "Validate times");

            Assert(endpointReference != null, "The DUT did not return SubscriptionReference",
                   "Check if the DUT returned SubscriptionReference");

            Assert(endpointReference.Address != null && endpointReference.Address.Value != null,
            "SubscriptionReference does not contain address",
                   "Check if SubscriptionReference contains address");

            Assert(endpointReference.Address.Value.IsValidUrl(), "URL passed in SubscriptionReference is not valid",
                   "Check that URL specified is valid");


            CreateSubscriptionManagerClient(endpointReference);

            return endpointReference;
        }        
        /// <summary>
        /// Delegate definition for GetMessages.
        /// </summary>
        /// <returns></returns>
        private delegate DateTime PullMessageDelegate();

        /// <summary>
        /// Getting messages for tests 9.3.7 and 9.3.8
        /// </summary>
        /// <param name="endpointReference">Endpoint reference to create dedicated client.</param>
        /// <param name="callSetSyncpoint">True, if GetSynchronizationPoint method should be called.</param>
        /// <param name="rawSoapPacket">Raw SOAP packet.</param>
        /// <returns></returns>
        NotificationMessageHolderType[] GetMessages(
            Proxies.Event.EndpointReferenceType endpointReference, 
            bool callSetSyncpoint,
            out string rawSoapPacket)
        {
            rawSoapPacket = null;

            // Create new service client to pass "local" traffic listener
            IChannelController[] controllers;
            TestTool.Tests.Common.Trace.TrafficListener trafficListener = new TrafficListener();

            EndpointAddress address = new EndpointAddress(endpointReference.Address.Value);

            EndpointController controller = new EndpointController(address);
            controller.WsaEnabled = true;

            controllers = new IChannelController[]
                              {
                                  trafficListener, 
                                  controller, 
                                  _semaphore, 
                                  _credentialsProvider,
                                  new SoapValidator(EventsSchemasSet.GetInstance())
                              };
            Binding binding = CreateBinding(controllers);
            PullPointSubscriptionClient pullPointSubscriptionClient =
                new PullPointSubscriptionClient(binding, address);
            AddSecurityBehaviour(pullPointSubscriptionClient.Endpoint);
            AttachAddressing(pullPointSubscriptionClient.Endpoint, endpointReference);

            // create delegate for PullMessages
            System.DateTime localTerminationTime = System.DateTime.MinValue;
            System.DateTime localCurrentTime = System.DateTime.MinValue;

            NotificationMessageHolderType[] notificationMessages = null;

            int messagesLimit = 2;
            int time = 20;
            string timeString = "PT20S";
            
            ManualResetEvent requestSentErrorEvent = new ManualResetEvent(false);

            PullMessageDelegate del = new PullMessageDelegate(() =>
            {
                NotificationMessageHolderType[] notificationMessageCopy = null;
                System.DateTime terminationTimeCopy = System.DateTime.MinValue;
                System.DateTime result = System.DateTime.MinValue;

                try
                {
                    result = pullPointSubscriptionClient.PullMessages(timeString,
                                                                       messagesLimit,
                                                                       null,
                                                                       out terminationTimeCopy,
                                                                       out notificationMessageCopy);
                }
                catch (System.Net.Sockets.SocketException exc)
                {
                    if (InStep)
                    {
                        StepFailed(exc);
                    }
                    requestSentErrorEvent.Set();
                }
                localTerminationTime = terminationTimeCopy;
                notificationMessages = notificationMessageCopy;
                localCurrentTime = result;

                return result;

            });

            // create delegate for SetSynchronizationPoint
            Action setSynchronizationPointAction = 
                new Action(() =>
                               {
                                   SetSynchronizationPoint();
                               });
            
            // Send PullMessages request
            BeginStep("Send PullMessages request");
            ManualResetEvent requestSentEvent = new ManualResetEvent(false);

            bool requestSentHandled = false;

            // create event handler for "RequestSent" event to 1) "close" the step (StepPassed) and 2)
            // signal that next call may be executed.
            trafficListener.RequestSent += new Action<string>((data) =>
            {
                //LogRequest handles Digest challenge
                LogRequest(data);
                
                if (!requestSentHandled)
                {
                    if (InStep)
                    {
                        string logEntry = "Waiting for PullMessagesResponse message ";
                        if (!callSetSyncpoint)
                        {
                            logEntry += "(User action required to trigger event)";
                        }
                        LogStepEvent(logEntry);
                        StepPassed();
                    }
                    requestSentEvent.Set();
                    requestSentHandled = true;
                }
            });

            // create event handler to save response
            string pullMessagesResponseData = null;
            trafficListener.ResponseReceived += new Action<string>((data) =>
            {
                pullMessagesResponseData = data;
            });

            IAsyncResult pullMessagesResult = del.BeginInvoke(null, null);
            
            // 2011/12/05 : changes from WaitOne for requestSentEvent to WaitAny
            //requestSentEvent.WaitOne();
            int hndl = System.Threading.WaitHandle.WaitAny(new WaitHandle[]
                                                               {
                                                                   requestSentEvent, 
                                                                   requestSentErrorEvent, 
                                                                   _semaphore.StopEvent
                                                               });
            if (hndl == 2)
            {
                System.Diagnostics.Debug.WriteLine("STOP - exit test");
                _semaphore.ReportStop();
                return null;
            }
            if (hndl == 1)
            {
                return null;
            }

            // call SetSynchronizationPoint after first response is sent
            IAsyncResult setSynchronizationPointResult = null;
            if (callSetSyncpoint)
            {
                setSynchronizationPointResult = setSynchronizationPointAction.BeginInvoke(null, null);
            }

            // ToDo 2011/11/24 : handle "stop" here?
            // wait until both requests are handled
            WaitHandle[] handles = 
                callSetSyncpoint ? 
                new WaitHandle[] { pullMessagesResult.AsyncWaitHandle, setSynchronizationPointResult.AsyncWaitHandle } : 
                new WaitHandle[] { pullMessagesResult.AsyncWaitHandle};
            
            WaitHandle.WaitAll(handles);

            //
            if (callSetSyncpoint)
            {
                setSynchronizationPointAction.EndInvoke(setSynchronizationPointResult);
            }

            bool retry = false;

            try
            {
                // Get response. Make it in separate step.
                BeginStep("Get PullMessages response");
                LogResponse(pullMessagesResponseData);
                DateTime dateTime = del.EndInvoke(pullMessagesResult);
                localCurrentTime = dateTime;
                StepPassed();

            }
            catch (FaultException exc)
            {
                FaultException<PullMessagesFaultResponseType> fault =
                    exc as FaultException<PullMessagesFaultResponseType>;
                if (fault != null)
                {
                    LogStepEvent(string.Format("Exception of type FaultException<PullMessagesFaultResponseType> received. Try to pull messages with new parameters"));
                    StepPassed();

                    Assert(fault.Detail != null, "Detail field is null", "Check if correct paramters are specified in fault");

                    timeString = fault.Detail.MaxTimeout;
                    time = (int)timeString.DurationToSeconds();
                    messagesLimit = fault.Detail.MaxMessageLimit;
                    retry = true;
                }
                else
                {
                    throw;
                }
            }

            if (retry)
            {
                requestSentHandled = false;
                requestSentEvent.Reset();

                BeginStep("Send PullMessages request");
                pullMessagesResult = del.BeginInvoke(null, null);
                //requestSentEvent.WaitOne();
                hndl = System.Threading.WaitHandle.WaitAny(new WaitHandle[] { requestSentEvent, _semaphore.StopEvent });
                if (hndl == 1)
                {
                    System.Diagnostics.Debug.WriteLine("STOP - exit test");
                    _semaphore.ReportStop();
                    return null;
                }
                
                if (callSetSyncpoint)
                {
                    setSynchronizationPointResult = setSynchronizationPointAction.BeginInvoke(null, null);
                }

                handles = callSetSyncpoint ?
                    new WaitHandle[] { pullMessagesResult.AsyncWaitHandle, setSynchronizationPointResult.AsyncWaitHandle } : 
                    new WaitHandle[] { pullMessagesResult.AsyncWaitHandle};
                
                WaitHandle.WaitAll(handles);

                //
                if (callSetSyncpoint)
                {
                    setSynchronizationPointAction.EndInvoke(setSynchronizationPointResult);
                }

                // Get response. Make it in separate step.
                BeginStep("Get PullMessages response");
                LogResponse(pullMessagesResponseData);
                DateTime dateTime = del.EndInvoke(pullMessagesResult);
                localCurrentTime = dateTime;
                StepPassed();
            }


            Assert(localCurrentTime < localTerminationTime,
                    "TerminationTime <= CurrentTime",
                    "Validate CurrentTime and TerminationTime");

            Assert(notificationMessages != null && notificationMessages.Length > 0,
                "No notification messages received",
                "Check that DUT sent notification messages");

            Assert(notificationMessages.Length <= messagesLimit,
                "Maximum number of messages exceeded",
                string.Format("Check that a maximum number of {0} Notification Messages is included in PullMessagesResponse", messagesLimit));

            System.IO.StringReader rdr = new StringReader(pullMessagesResponseData);

            string nextLine;
            do
            {
                nextLine = rdr.ReadLine();
            } while (!string.IsNullOrEmpty(nextLine));

            rawSoapPacket = rdr.ReadToEnd();
            rawSoapPacket = rawSoapPacket.Replace("\r\n", "");

            return notificationMessages;
        }
        
        protected void ValidateNotificationsPacket(byte[] data)
        {
            BeginStep("Validate notifications SOAP packet");
            MemoryStream stream = new MemoryStream(data);
            SoapValidator validator = new SoapValidator(EventsSchemasSet.GetInstance());
            try
            {
                validator.Validate(stream);
            }
            catch (Exception exc)
            {
                stream.Close();
                throw;
            }
            StepPassed();
        }

        protected XmlDocument LoadNotificationsAsXml(byte[] data)
        {
            string dump = System.Text.Encoding.UTF8.GetString(data);
            XmlDocument document = new XmlDocument();
            document.LoadXml(dump);
            return document;
        }

        #endregion

        #region Validate faults

        /// <summary>
        /// Validates "Invalid filter" fault.
        /// </summary>
        /// <param name="fault"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected bool ValidateFilterSubscriptionFault(FaultException fault, out string reason)
        {
            reason = ""; 

            // No fault is OK;
            if (fault == null)
            {
                return true;
            }

            if (fault is FaultException<InvalidFilterFaultType> || 
                fault is FaultException<TopicExpressionDialectUnknownFaultType> ||
                fault is FaultException<InvalidTopicExpressionFaultType> ||
                fault is FaultException<TopicNotSupportedFaultType> ||
                fault is FaultException<InvalidMessageContentExpressionFaultType> ||
                fault is FaultException<UnacceptableInitialTerminationTimeFaultType>)
            {
                reason = "Fault received is one caused by invalid filter or incorrect Termination Time. You should provide valid values.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates "Unacceptable Initial Termination Time" fault.
        /// </summary>
        /// <param name="fault"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected bool ValidateSubscriptionFault(FaultException fault, out string reason)
        {
            reason = "";

            // No fault is OK;
            if (fault == null)
            {
                return true;
            }

            if (fault is FaultException<UnacceptableInitialTerminationTimeFaultType>)
            {
                reason = "Fault received is FaultException<UnacceptableInitialTerminationTimeFaultType>. You should provide valid value.";
                return false;
            }

            return true;
        }

        #endregion

        #region Auxiliary

        /// <summary>
        /// Finds all topics beneath the element specified.
        /// </summary>
        /// <param name="element">Topic or TopicNamespace element.</param>
        /// <param name="topics">Collection to add topics to.</param>
        protected void FindTopics(XmlElement element, List<XmlElement> topics)
        {
            if (element.RepresentsTopic())
            {
                topics.Add(element);
            }

            // If not a topic - enumerate child elements.
            foreach (XmlNode node in element.ChildNodes)
            {
                XmlElement child = node as XmlElement;
                if (child == null)
                {
                    continue;
                    ;
                }
                FindTopics(child, topics);
            }
        }

        /// <summary>
        /// Finds and parses topics which represent properties.
        /// </summary>
        /// <param name="topics"></param>
        /// <returns></returns>
        protected List<TopicInfo> FindPropertyTopics(List<XmlElement> topics)
        {
            List<TopicInfo> propertyTopics = new List<TopicInfo>();
            foreach (XmlElement topicElement in topics)
            {
                XmlElement messageDescription = topicElement.GetMessageDescription();
                if (messageDescription != null)
                {
                    bool isProperty = false;
                    // check that it is a property
                    if (messageDescription.HasAttribute(OnvifMessage.ISPROPERTY))
                    {
                        isProperty = bool.Parse(messageDescription.Attributes[OnvifMessage.ISPROPERTY].Value);
                    }
                    if (isProperty)
                    {
                        System.Diagnostics.Debug.WriteLine("--> PROPERTY");
                        propertyTopics.Add(TopicInfo.ConstructTopicInfo(topicElement));
                    }
                }
            }
            return propertyTopics;
        }

        /// <summary>
        /// Create namespace manager to work with TopicSet information.
        /// </summary>
        /// <param name="soapRawPacket"></param>
        /// <returns></returns>
        protected XmlNamespaceManager CreateNamespaceManager(XmlDocument soapRawPacket)
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(soapRawPacket.NameTable);
            manager.AddNamespace("s", "http://www.w3.org/2003/05/soap-envelope");
            manager.AddNamespace("events", "http://www.onvif.org/ver10/events/wsdl");
            manager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");

            return manager;
        }

        /// <summary>
        /// Gets "raw" notification messages.
        /// </summary>
        /// <param name="notificationMessages"></param>
        /// <param name="soapRawPacket"></param>
        /// <param name="manager"></param>
        /// <param name="notification"></param>
        /// <returns></returns>
        protected Dictionary<NotificationMessageHolderType, XmlElement>
            GetRawElements(
            NotificationMessageHolderType[] notificationMessages,
            XmlDocument soapRawPacket,
            XmlNamespaceManager manager,
            bool notification)
        {
            Dictionary<NotificationMessageHolderType, XmlElement> rawElements = new Dictionary<NotificationMessageHolderType, XmlElement>();

            string messagePath;
            if (notification)
            {
                messagePath = "/s:Envelope/s:Body/b2:Notify/b2:NotificationMessage";
            }
            else
            {
                messagePath = "/s:Envelope/s:Body/events:PullMessagesResponse/b2:NotificationMessage";
            }

            XmlNodeList responseNodeList = soapRawPacket.SelectNodes(messagePath, manager);
            int cnt = 0;

            foreach (NotificationMessageHolderType message in notificationMessages)
            {
                rawElements.Add(message, (XmlElement)responseNodeList[cnt]);
                cnt++;
            }

            return rawElements;
        }
        
        /// <summary>
        /// Finds topic in the topic set.
        /// </summary>
        /// <param name="topicSet"></param>
        /// <param name="topicInfo"></param>
        /// <returns></returns>
        protected XmlElement FindTopic(TopicSetType topicSet, TopicInfo topicInfo)
        {
            if (topicSet.Any == null)
            {
                return null;
            }

            foreach (XmlElement any in topicSet.Any)
            {
                XmlElement topic = FindTopic(any, topicInfo);
                if (topic != null)
                {
                    return topic;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds topic beneath "any" element.
        /// </summary>
        /// <param name="any"></param>
        /// <param name="topicInfo"></param>
        /// <returns></returns>
        XmlElement FindTopic(XmlElement any, TopicInfo topicInfo)
        {
            List<TopicInfo> orderedTopics = new List<TopicInfo>();

            TopicInfo current = topicInfo;

            while (current != null)
            {
                orderedTopics.Add(current);
                current = current.ParentTopic;
            }

            XmlElement currentNode = any;
            if (Match(any, string.Empty, orderedTopics[orderedTopics.Count -1]))
            {
                string defaultNamespace = any.NamespaceURI;
                for (int i = orderedTopics.Count - 2; i >= 0; i--)
                {
                    currentNode = FindChildTopic(currentNode, defaultNamespace, orderedTopics[i]);
                    if (currentNode == null)
                    {
                        return null;
                    }
                    if (!string.IsNullOrEmpty(currentNode.NamespaceURI))
                    {
                        defaultNamespace = currentNode.NamespaceURI;
                    }
                }
                return currentNode;
            }
            return null;
        }

        /// <summary>
        /// Finds topic among child elements.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="topicInfo"></param>
        /// <returns></returns>
        XmlElement FindChildTopic(XmlElement topic, string defaultNamespace, TopicInfo topicInfo)
        {
            foreach (XmlNode node in topic.ChildNodes)
            {
                XmlElement element = node as XmlElement;
                if (element != null)
                {
                    if (Match(element, defaultNamespace, topicInfo))
                    {
                        return element;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if the topic represented by XML element and topic represented by TopicInfo match.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="topicInfo"></param>
        /// <returns></returns>
        bool Match(XmlElement topic, string defaultNamespace, TopicInfo topicInfo)
        {
            string topicNamespace = string.IsNullOrEmpty(topic.NamespaceURI) ? defaultNamespace : topic.NamespaceURI;
            return (StringComparer.CurrentCultureIgnoreCase.Compare(topicNamespace, topicInfo.Namespace) == 0
                    && StringComparer.CurrentCultureIgnoreCase.Compare(topic.LocalName, topicInfo.Name) == 0);

        } 

        #endregion
        
        #region Validate Messages

        protected void ValidateNotifyNotEmpty(Notify notify)
        {
            Assert(notify != null && notify.NotificationMessage != null && notify.NotificationMessage.Length > 0,
                "No notification messages received",
                "Check that DUT sent notification messages");
        }

        /// <summary>
        /// Validates messages.
        /// </summary>
        /// <param name="notificationMessages"></param>
        /// <param name="soapRawPacket">Raw packet to get access to attributes</param>
        /// <param name="notification">Used to get proper element name.</param>
        /// <param name="filter"></param>
        /// <param name="topicSet"></param>
        protected void ValidateMessages(NotificationMessageHolderType[] notificationMessages,
            XmlDocument soapRawPacket,
            bool notification,
            FilterInfo filter,
            TopicSetType topicSet)
        {
            BeginStep("Validate messages");

            /* Find raw elements */

            XmlNamespaceManager manager = CreateNamespaceManager(soapRawPacket);

            Dictionary<NotificationMessageHolderType, XmlElement> rawElements = GetRawElements(notificationMessages,
                                                                                               soapRawPacket,
                                                                                               manager,
                                                                                               notification);

            /* raw elements list initialized */

            List<XmlElement> topics = new List<XmlElement>();
            foreach (XmlElement element in topicSet.Any)
            {
                FindTopics(element, topics);
            }
            
            foreach (NotificationMessageHolderType message in notificationMessages)
            {
                string reason;
                if (!IsValidMessage(message,
                    rawElements[message],
                    manager,
                    filter,
                    topics,
                    out reason))
                {
                    throw new AssertException(reason);
                }
            }

            StepPassed();
        }

        /// <summary>
        /// Validates notification.
        /// </summary>
        /// <param name="notification">Notification</param>
        /// <param name="reason">Error description, if any</param>
        /// <returns></returns>
        protected bool IsValidMessage(NotificationMessageHolderType notification,
            XmlElement messageRawElement,
            XmlNamespaceManager manager,
            FilterInfo filter,
            List<XmlElement> topics, 
            out string reason)
        {
            if (notification.Topic == null)
            {
                reason = "Topic is null";
                return false;
            }

            XmlText text = null;
            if (notification.Topic.Any != null)
            {
                foreach (XmlNode any in notification.Topic.Any)
                {
                    XmlText current = any as XmlText;
                    if (any != null)
                    {
                        text = current;
                        break;
                    }
                }
            }

            XmlNode topicNode = messageRawElement.SelectSingleNode("b2:Topic", manager);
            
            string topic = text != null ? text.Value : "";

            TopicInfo actualTopic = TopicInfo.ExtractTopicInfo(topic, topicNode);

            TopicInfo currentTopic = actualTopic;
            while (currentTopic != null)
            {
                if (currentTopic.ParentTopic == null && string.IsNullOrEmpty(currentTopic.NamespacePrefix))
                {
                    reason = string.Format("Topic {0} is incorrect: root topic must have namespace defined", topic);
                    return false;
                }
                if (!string.IsNullOrEmpty(currentTopic.NamespacePrefix) && string.IsNullOrEmpty(currentTopic.Namespace))
                {
                    reason = string.Format("Topic {0} is incorrect: namespace prefix {1} not defined", topic, currentTopic.NamespacePrefix);
                    return false;
                }
                currentTopic = currentTopic.ParentTopic;
            }

            if (filter != null && filter.Topic != null)
            {
                // validate topic

                string expectedTopicDescription = filter.Topic.GetDescription();
                string actualTopicDescription = actualTopic.GetDescription();

                bool match = TopicInfo.TopicsMatch(actualTopic, filter.Topic);

                if (!match)
                {
                    reason = string.Format("Invalid topic. {0}Expected: {1}{0}Actual: {2}",
                        Environment.NewLine,
                        expectedTopicDescription,
                        actualTopicDescription);
                    return false;
                }
            }

            XmlElement messageTopic = null;
            foreach (XmlElement topicElement in topics)
            {
                TopicInfo info = TopicInfo.ConstructTopicInfo(topicElement);
                if (TopicInfo.TopicsMatch(info, actualTopic))
                {
                    messageTopic = topicElement;
                    break;
                }
            }

            return IsValidMessageElement(notification.Message, messageTopic, filter, out reason);
        }

        /// <summary>
        /// Validates a message against Message type defined in onvif.xsd
        /// </summary>
        /// <param name="message">Message XML element</param>
        /// <param name="filter"></param>
        /// <param name="reason">Error description, if any.</param>
        /// <returns>True, if message is valid; false otherwise</returns>
        protected bool IsValidMessageElement(XmlElement message,
            XmlElement topicElement,
            FilterInfo filter,
            out string reason)
        {
            if (message == null)
            {
                reason = "Message element not found";
                return false;
            }

            // Check that mandatory attribute is present.
            if (!message.HasAttribute(OnvifMessage.UTCTIMEATTRIBUTE))
            {
                reason = "Mandatory attribute UtcTime not found";
                return false;
            }

            // check UtcTime format
            string utcTimeValue = message.Attributes[OnvifMessage.UTCTIMEATTRIBUTE].Value;
            // xs:dateTime string

            if (!IsValidXsdDateTime(utcTimeValue))
            {
                reason = string.Format("'{0}' is not valid xs:datetime value", utcTimeValue);
                return false;
            }

            XmlElement messageDescription = null;
            if (topicElement != null)
            {
                messageDescription = topicElement.GetMessageDescription();
            }

            // if message description found - check that message is valid accordingly to the filter.
            if (messageDescription != null)
            {
                bool isProperty = false;
                // check that it is a property
                if (messageDescription.HasAttribute(OnvifMessage.ISPROPERTY))
                {
                    isProperty = bool.Parse(messageDescription.Attributes[OnvifMessage.ISPROPERTY].Value);
                }

                // if topic is Property topic
                if (isProperty)
                {
                    if (message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                    {
                        XmlAttribute propertyOperationType = message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];

                        bool match = false;
                        foreach (string allowedPropertyOperation in new string[] {OnvifMessage.INITIALIZED, OnvifMessage.CHANGED, OnvifMessage.DELETED} )
                        {
                            if (propertyOperationType.Value == allowedPropertyOperation)
                            {
                                match = true;
                                break;
                            }
                        }

                        if (!match)
                        {
                            reason = string.Format("   PropertyOperation attribute has unexpected value: {0}",
                                                   propertyOperationType.Value);
                            return false;
                        }
                    }
                    else
                    {
                        reason = "   PropertyOperation attribute not found";
                        return false;
                    }
                }
            }

            List<string> allItems = new List<string>();

            foreach (XmlNode node in message.ChildNodes)
            {
                XmlElement child = node as XmlElement;
                if (child == null)
                {
                    continue;
                }

                if (child.LocalName != OnvifMessage.SOURCE &&
                    child.LocalName != OnvifMessage.KEY &&
                    child.LocalName != OnvifMessage.DATA &&
                    child.LocalName != OnvifMessage.EXTENSION)
                {
                    reason = string.Format("Unexpected element: {0}", child.Name);
                    return false;
                }

                if (child.NamespaceURI != OnvifMessage.ONVIF)
                {
                    reason = string.Format("Element {0} is not from expected namespace", child.Name);
                    return false;
                }

                if (child.LocalName != OnvifMessage.EXTENSION)
                {
                    List<string> items = new List<string>();

                    // content should be tt:ItemList
                    foreach (XmlNode childNode in child.ChildNodes)
                    {
                        XmlElement item = childNode as XmlElement;
                        if (item == null)
                        {
                            continue;
                        }
                        switch (item.LocalName)
                        {
                            case OnvifMessage.SIMPLEITEM:
                                {
                                    if (!item.HasAttribute(OnvifMessage.NAME))
                                    {
                                        reason = string.Format("Element {0} has no mandatory 'Name' attribute",
                                                               item.Name);
                                        return false;
                                    }
                                    string name = item.Attributes[OnvifMessage.NAME].Value;
                                    if (items.Contains(name))
                                    {
                                        reason = string.Format("Name {0} is not unique", name);
                                        return false;
                                    }

                                    items.Add(name);

                                    if (!item.HasAttribute(OnvifMessage.VALUE))
                                    {
                                        reason = string.Format("Element {0} has no mandatory 'Value' attribute",
                                                               item.Name);
                                        return false;
                                    }
                                }
                                break;
                            case OnvifMessage.ELEMENTITEM:
                                {
                                    if (!item.HasAttribute(OnvifMessage.NAME))
                                    {
                                        reason = string.Format("Element {0} has no mandatory 'Name' attribute",
                                                               item.Name);
                                        return false;
                                    }

                                    string name = item.Attributes[OnvifMessage.NAME].Value;
                                    if (items.Contains(name))
                                    {
                                        reason = string.Format("Name {0} is not unique", name);
                                        return false;
                                    }

                                    items.Add(name);

                                }
                                break;
                            case OnvifMessage.ITEMLISTEXTENSION:
                                {

                                }
                                break;
                            default:
                                {
                                    reason = string.Format("Unexpected element: {0}", item.Name);
                                    return false;
                                }
                        }
                    }

                    allItems.AddRange(items);
                }
            }

            reason = string.Empty;
            return true;
        }


        #endregion
    }
}
