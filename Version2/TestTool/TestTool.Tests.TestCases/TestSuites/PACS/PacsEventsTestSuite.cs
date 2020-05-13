using System;
using System.Collections.Generic;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Channels;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.Utils;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Proxies.Event;
using System.Text;
using TestTool.Tests.Engine.Base.Definitions;
using System.Linq;
using DateTime=System.DateTime;
using TestTool.Tests.CommonUtils.SoapValidation;
using System.IO;
using System.Threading;

using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using EndpointReferenceType = TestTool.Proxies.Event.EndpointReferenceType;
using AttributedURIType = TestTool.Proxies.Event.AttributedURIType;

namespace TestTool.Tests.TestCases.TestSuites
{        
   
    [TestClass]
    partial class PacsEventsTestSuite : NotificationsTestSuite
    {
        public PacsEventsTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        protected const bool UseNotifyToGetEvents = false;

        protected const string ACNAMESPACE = OnvifService.ACCESSCONTROL;
        protected const string DCNAMESPACE = OnvifService.DOORCONTROL;

        protected const string PTNAMESPACE = "http://www.onvif.org/ver10/pacs";
        protected const string XSNAMESPACE = "http://www.w3.org/2001/XMLSchema";

        protected const string ONVIFTOPICSET = "http://www.onvif.org/ver10/topics";


        #region additional services 

        private ServiceHolder<PACSPortClient, PACSPort> _pacsServiceHolder;
        private ServiceHolder<DoorControlPortClient, DoorControlPort> _doorControlServiceHolder;
        
        protected PACSPortClient PACSPortClient
        {
            get
            {
                if (_pacsServiceHolder == null)
                {
                    InitServiceHolders();
                }

                if (_pacsServiceHolder.Client == null)
                {
                    IChannelController[] controllers = new IChannelController[]
                                                           {
                                                               new SoapValidator(AccessControlSchemaSet.GetInstance()),
                                                           };

                    InitServiceClient(_pacsServiceHolder, controllers);

                }
                return _pacsServiceHolder.Client;
            }
        }

        protected DoorControlPortClient DoorControlPortClient
        {
            get
            {
                if (_doorControlServiceHolder == null)
                {
                    InitServiceHolders();
                }

                if (_doorControlServiceHolder.Client == null)
                {
                    IChannelController[] controllers = new IChannelController[]
                                                           {
                                                               new SoapValidator(DoorControlSchemaSet.GetInstance()),
                                                           };

                    InitServiceClient(_doorControlServiceHolder, controllers);

                }
                return _doorControlServiceHolder.Client;
            }
        }

        protected new IPACSOperator Operator
        {
            get { return (IPACSOperator) base.Operator; }
        }

        
        DeviceClient _deviceClient;

        DeviceClient DeviceClient
        { 
            get
            {
                if (_deviceClient == null)
                {
                    Binding binding =
                        CreateBinding(true,
                        new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                    _deviceClient = new DeviceClient(binding, new EndpointAddress(CameraAddress));
                    AddSecurityBehaviour(_deviceClient.Endpoint);
                }
                return _deviceClient;
            }

        }

        void InitServiceHolders()
        {
            // access control
            _pacsServiceHolder = new ServiceHolder<PACSPortClient, PACSPort>(
                (features) => { return DeviceClient.GetServiceAddress(OnvifService.ACCESSCONTROL); },
                (binding, address) => { return new PACSPortClient(binding, address); },
                "Access Control");

            // access control
            _doorControlServiceHolder = new ServiceHolder<DoorControlPortClient, DoorControlPort>(
                (features) => { return DeviceClient.GetServiceAddress(OnvifService.DOORCONTROL); },
                (binding, address) => { return new DoorControlPortClient(binding, address); },
                "Door Control");
        }

        void InitServiceClient(ServiceHolder serviceHolder, IEnumerable<IChannelController> controllers)
        {
            bool found = false;
            if (!serviceHolder.HasAddress)
            {
                RunStep(() =>
                {
                    serviceHolder.Retrieve(Features);
                    if (!serviceHolder.HasAddress)
                    {
                        throw new AssertException(string.Format("{0} service not found", serviceHolder.ServiceName));
                    }
                    else
                    {
                        found = true;
                        LogStepEvent(serviceHolder.Address);
                    }
                }, string.Format("Get {0} service address", serviceHolder.ServiceName),
                OnvifFaults.NoSuchService, true, true);
                DoRequestDelay();
            }

            Assert(found,
                string.Format("{0} service address not found", serviceHolder.ServiceName),
                string.Format("Check that the DUT returned {0} service address", serviceHolder.ServiceName));

            if (found)
            {
                EndpointController controller = new EndpointController(new EndpointAddress(serviceHolder.Address));

                List<IChannelController> ctrls = new List<IChannelController>();
                ctrls.Add(controller);
                ctrls.AddRange(controllers);

                Binding binding = CreateBinding(
                    false,
                    ctrls);

                serviceHolder.CreateClient(binding, AttachSecurity, SetupChannel);
            }
        }

        void CloseClients()
        {
            foreach (ServiceHolder sh in new ServiceHolder[] { _pacsServiceHolder, _doorControlServiceHolder  })
            {
                if (sh != null)
                {
                    sh.Close();
                }
            }
        }

        protected override void Release()
        {
            if (Operator != null)
            {
                Operator.HideMessage();
                Operator.HideDoorSelectionMessage();
                Operator.HideCountdownMessage();
            }
            CloseClients();
            base.Release();
        }

        #endregion
        

        #region Test Primitives

        protected List<XmlElement> GetAllTopics()
        {
            // we'll need "raw" topic information - otherwise not all namespaces are available.
            string response = string.Empty;

            Action<string> dumpAction = new Action<string>(s => response = s);

            this._trafficListener.ResponseReceived += dumpAction;

            // Get all topics.
            TestTool.Proxies.Event.TopicSetType topicSet = GetTopicSet();

            this._trafficListener.ResponseReceived -= dumpAction;

            // check that topic set is not empty
            bool notEmptyTopicSet = !(topicSet == null || topicSet.Any == null || topicSet.Any.Length == 0);
            Assert(notEmptyTopicSet, "The DUT provides no topics", "Check that topic list is not empty");


            XmlDocument soapRawResponse = BaseNotificationUtils.GetRawResponse(response);

            // find Topic elements in "raw" packet
            string topicPath;
            topicPath = "/s:Envelope/s:Body/events:GetEventPropertiesResponse/t1:TopicSet";
            XmlNamespaceManager manager = CreateNamespaceManager(soapRawResponse);
            manager.AddNamespace("t1", "http://docs.oasis-open.org/wsn/t-1");

            XmlNode topicSetNode = soapRawResponse.SelectSingleNode(topicPath, manager);
            XmlElement topicSetElement = topicSetNode as XmlElement;
            List<XmlElement> rootTopics = new List<XmlElement>();
            foreach (XmlNode node in topicSetElement.ChildNodes)
            {
                XmlElement e = node as XmlElement;
                if (e != null)
                {
                    rootTopics.Add(e);
                }
            }

            ValidateTopicsXml(rootTopics);

            // Check that the topic of interest is supported

            // select all topics 
            List<XmlElement> topics = new List<XmlElement>();
            foreach (XmlElement element in rootTopics)
            {
                FindTopics(element, topics);
            }

            return topics;
        }

        /// <summary>
        /// Creates topic element (for subscription) for topicInfo passed.
        /// To get full information about topics supported, event properties will 
        /// be requested from the DUT.
        /// </summary>
        /// <param name="topicInfo"></param>
        /// <returns></returns>
        protected XmlElement GetTopicElement(TopicInfo topicInfo)
        {
            List<XmlElement> topics = GetAllTopics();
                        
            return GetTopicElement(topics, topicInfo);
        }

        protected XmlElement GetTopicElement(IEnumerable<XmlElement> topics, TopicInfo topicInfo)
        { 
            // check if "our" topic is present
            XmlElement topicElement = null;
            foreach (XmlElement el in topics)
            {
                TopicInfo info = TopicInfo.ConstructTopicInfo(el);
                if (TopicInfo.TopicsMatch(info, topicInfo))
                {
                    topicElement = el;
                    break;
                }
            }
            return topicElement;
        }

        void ValidateTopicsXml(List<XmlElement> rootTopics)
        {
            BeginStep("Validate topics XML representation");
            StringBuilder logger = new StringBuilder();
            foreach (XmlElement element in rootTopics)
            {
                ValidateTopicXml(element, null, string.Empty, logger);
            }
            string error = logger.ToStringTrimNewLine();
            if (!string.IsNullOrEmpty(error))
            {
                throw new AssertException(error);
            }
            StepPassed();
        }

        void ValidateTopicXml(XmlElement topicElement, XmlElement parentTopic, string currentPath, StringBuilder logger)
        {             
            // check current

            if (parentTopic == null)
            {
                if (string.IsNullOrEmpty(topicElement.NamespaceURI))
                {
                    logger.AppendLine(string.Format("Empty namespace for {0} is not correct", topicElement.LocalName));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(topicElement.NamespaceURI))
                {
                    // ok, NCName
                }
                else
                {
                    // The Topic name is used as the local part of the element name, 
                    // and the element is qualified with a Namespace if and only if 
                    // it represents a root Topic from a Topic Namespace 
                    // other than the ad-hoc Topic Namespace.
                    if (parentTopic.NamespaceURI == topicElement.NamespaceURI)
                    {
                        logger.AppendLine(string.Format("Topic {0}/{1} must be represented by a non-qualified element", currentPath, topicElement.Name));
                    }                    
                }
            }

            foreach (XmlNode childNode in topicElement.ChildNodes)
            {
                XmlElement childTopic = childNode as XmlElement;
                if (childTopic != null)
                {
                    // skip MessageDescription elements
                    if (childTopic.LocalName != OnvifMessage.MESSAGEDESCRIPTION && 
                        childTopic.NamespaceURI != OnvifMessage.ONVIF  )
                    {
                        string path = string.IsNullOrEmpty(currentPath) ? topicElement.Name : string.Format("{0}/{1}", currentPath, topicElement.Name);
                        ValidateTopicXml(childTopic, topicElement, path, logger);
                    }
                }            
            }
        }


        protected void ValidateTopicXml(XmlElement topicElement)
        {
            BeginStep("Validate topic XML representation");
            StringBuilder logger = new StringBuilder();

            List<XmlElement> path = new List<XmlElement>();
            string description = string.Empty;
            XmlElement current = topicElement;
            while (current.ParentNode != null)
            {
                if (string.IsNullOrEmpty(current.NamespaceURI))
                {
                    logger.AppendLine("Namespace cannot be empty");
                    break;
                }
                path.Add(current);
                if (current.ParentNode.Name == "TopicSet" && 
                    current.ParentNode.NamespaceURI == "http://docs.oasis-open.org/wsn/t-1")
                {
                    break;
                }
                current = current.ParentNode as XmlElement;
            }

            string lastNs = current.NamespaceURI;
            for (int i = path.Count - 2; i >= 0; i--)
            { 
                XmlElement nextNode = path[i];
                if (nextNode.RepresentsTopic())
                {
                    if (nextNode.NamespaceURI == lastNs && !string.IsNullOrEmpty(nextNode.Prefix))
                    {
                        logger.AppendLine(string.Format("Non-root topics must be represented by non-qualified names ({0} is not correct)", nextNode.Name));
                        break;
                    }
                }
                lastNs = nextNode.NamespaceURI;
            }

            string error = logger.ToStringTrimNewLine();
            if (!string.IsNullOrEmpty(error))
            {
                throw new AssertException(error);
            }
            StepPassed();
        }

        /// <summary>
        /// Creates filter element for Subscribe request.
        /// </summary>
        /// <param name="topicInfo">Topic information</param>
        /// <param name="messageDescription">Message description</param>
        /// <returns></returns>
        protected FilterInfo CreateFilter(TopicInfo topicInfo, XmlElement messageDescription)
        {
            FilterInfo filter = new FilterInfo();

            filter.Filter = CreateSubscriptionFilter(topicInfo);

            filter.MessageDescription = messageDescription;

            return filter;
        }

        protected Proxies.Event.FilterType CreateSubscriptionFilter(TopicInfo topicInfo)
        {
            return CreateSubscriptionFilter(new TopicInfo[] { topicInfo });
        }

        protected Proxies.Event.FilterType CreateSubscriptionFilter(IEnumerable<TopicInfo> topicInfos)
        {
            Proxies.Event.FilterType filter = new Proxies.Event.FilterType();

            XmlDocument filterDoc = new XmlDocument();
            XmlElement filterTopicElement = filterDoc.CreateTopicElement();

            string topicPath = string.Empty;
            foreach (TopicInfo topicInfo in topicInfos)
            {
                string topicExpression = TopicInfo.CreateTopicPath(filterTopicElement, topicInfo);

                if (string.IsNullOrEmpty(topicPath))
                {
                    topicPath = topicExpression;
                }
                else
                {
                    topicPath = string.Format("{0}|{1}", topicPath, topicExpression);
                }
            }

            filterTopicElement.InnerText = topicPath;

            filter.Any = new XmlElement[] { filterTopicElement };

            return filter;
        }


        protected EndpointReferenceType CreateSubscription(Proxies.Event.FilterType filter, 
            int timeout, 
            string notificationUri,
            out DateTime subscribeStarted )
        { 
                        
            LogTestEvent(string.Format("Timeout of {0} seconds will be used{1}", timeout, Environment.NewLine));


            Proxies.Event.Subscribe subscribeRequest = new Subscribe();
            
            subscribeRequest.InitialTerminationTime = string.Format("PT{0}S", timeout);

            subscribeRequest.Filter = filter;

            subscribeRequest.ConsumerReference = new EndpointReferenceType();
            subscribeRequest.ConsumerReference.Address = new AttributedURIType();
            subscribeRequest.ConsumerReference.Address.Value = notificationUri;

            EnsureNotificationProducerClientCreated();

            subscribeStarted = DateTime.MaxValue;
                       
            SubscribeResponse subscribeResponse = null;
            RunStep(
                   () =>
                   {
                       subscribeResponse = _notificationProducerClient.Subscribe(subscribeRequest);
                   },
                   "Subscribe");

            subscribeStarted = DateTime.Now;

            ValidateSubscribeResponse(subscribeResponse, timeout);

            CreateSubscriptionManagerClient(subscribeResponse.SubscriptionReference);

            return subscribeResponse.SubscriptionReference;
        }

        #region Receive messages (one Notify)

        protected EndpointReferenceType ReceiveMessages(FilterInfo filter, int timeout, XmlDocument doc, out Notify notify, out DateTime subscribeStarted)
        {
            return ReceiveMessages(filter.Filter, timeout, null, doc, out notify, out subscribeStarted);
        }
        
        /// <summary>
        /// Creates subscription and receives messages (from one Notify call)
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <param name="doc">[output] XmlDocument to store "raw" notification packet</param>
        /// <param name="timeout">Timeout</param>
        /// <param name="subscribeStarted">Date and time when subscribe process has been started</param>
        /// <returns></returns>
        protected EndpointReferenceType ReceiveMessages(Proxies.Event.FilterType filter, 
            int timeout, 
            Action eventInitiationAction,
            XmlDocument doc, out Notify notify, out DateTime subscribeStarted)
        {
            subscribeStarted = DateTime.MaxValue;

            
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

            EndpointReferenceType subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out subscribeStarted);

            // for SetSynchronizationPoint
            Action initiationAction;
            if (eventInitiationAction != null)
            {
                initiationAction = new Action(() => { StepPassed(); eventInitiationAction(); });
            }
            else
            {
                CreatePullPointSubscriptionClient(subscriptionReference);
                initiationAction = new Action(() => { StepPassed(); SetSynchronizationPoint(); });
            }
            BeginStep("Start listening");
            notify = server.WaitForNotify(initiationAction,
                timeout * 1000,
                _semaphore.StopEvent);

            RemoveHandlers(server);

            ValidateNotificationsPacket(server.RawData);

            string dump = System.Text.Encoding.UTF8.GetString(server.RawData);
            doc.LoadXml(dump);

            ValidateNotifyNotEmpty(notify);

            return subscriptionReference;
        }

        #endregion

        #region Receive message (several Notify)

        protected EndpointReferenceType ReceiveMessages(TestTool.Proxies.Event.FilterType filter,
            int timeout, 
            Action eventInitiationAction,
            int limit, 
            Dictionary<NotificationMessageHolderType, XmlElement> messages, out DateTime subscribeStarted)
        {
            return ReceiveMessages(filter, timeout, eventInitiationAction, limit, null, messages, out subscribeStarted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="timeout">Subscription timeout</param>
        /// <param name="operationTimeout"></param>
        /// <param name="eventInitiationAction"></param>
        /// <param name="limit"></param>
        /// <param name="messageCheck"></param>
        /// <param name="messages"></param>
        /// <param name="subscribeStarted"></param>
        /// <returns></returns>
        protected EndpointReferenceType ReceiveMessages(TestTool.Proxies.Event.FilterType filter, 
            int timeout, 
            Action eventInitiationAction,
            int limit,
            Func<NotificationMessageHolderType, bool> messageCheck,
            Dictionary<NotificationMessageHolderType, XmlElement> messages, out DateTime subscribeStarted)
        {
            subscribeStarted = DateTime.MaxValue;

            Utils.NotifyCollectingServer server = null;
            try
            {
                // Create and setup Notification server
                server = new NotifyCollectingServer(_nic);

                SetupNotifyServer2(server);
            }
            catch (Exception e)
            {
                Assert(false, e.Message, "Creating subscription failed");
            }

            string notificationsUri = server.GetNotificationUri();

            DateTime localSubscribeStarted = DateTime.MaxValue;

            EndpointReferenceType subscriptionReference = null; 

            // for SetSynchronizationPoint
            //CreatePullPointSubscriptionClient(subscribeResponse.SubscriptionReference);

            AutoResetEvent subscribedEvent = new AutoResetEvent(false);
            Action setSynchronizationPointAction = 
                new Action(
                    () => 
                    { 
                        StepPassed(); 
                        subscriptionReference = CreateSubscription(filter, timeout, notificationsUri, out localSubscribeStarted);
                        eventInitiationAction(); 
                        subscribedEvent.Set(); 
                    });

            BeginStep("Start listening");

            Dictionary<Notify, byte[]> notifications = server.CollectNotifications(setSynchronizationPointAction,
                _operationDelay, limit, messageCheck, subscribedEvent,
                _semaphore.StopEvent);

            subscribeStarted = localSubscribeStarted;
            RemoveHandlers2(server);

            Operator.HideMessage();

            foreach (Notify notify in notifications.Keys)
            {
                ValidateNotificationsPacket(notifications[notify]);

                string soap = Encoding.UTF8.GetString(notifications[notify]);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(soap);
                
                XmlNamespaceManager manager = CreateNamespaceManager(doc);
                Dictionary<NotificationMessageHolderType, XmlElement> nextPart =
                    GetRawElements(notify.NotificationMessage, doc, manager, true);

                foreach (NotificationMessageHolderType message in nextPart.Keys)
                {
                    bool add = true;
                    if (messageCheck != null)
                    {
                        add = messageCheck(message);
                    }
                    if (add)
                    {
                        messages.Add(message, nextPart[message]);
                    }
                }
            }

            return subscriptionReference;
        }

        #endregion

        protected Dictionary<NotificationMessageHolderType, XmlElement>
            GetRawElements(Dictionary<Notify, byte[]> notifications)
        {
            Dictionary<NotificationMessageHolderType, XmlElement> messages = new Dictionary<NotificationMessageHolderType, XmlElement>();
            foreach (Notify notify in notifications.Keys)
            {
                ValidateNotificationsPacket(notifications[notify]);

                string soap = Encoding.UTF8.GetString(notifications[notify]);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(soap);

                XmlNamespaceManager manager = CreateNamespaceManager(xmlDoc);
                Dictionary<NotificationMessageHolderType, XmlElement> nextPart =
                    GetRawElements(notify.NotificationMessage, xmlDoc, manager, true);

                foreach (NotificationMessageHolderType m in nextPart.Keys)
                {
                    messages.Add(m, nextPart[m]);
                }
            }

            return messages;
        }


        protected Dictionary<NotificationMessageHolderType, XmlElement> GetFilteredList(Dictionary<NotificationMessageHolderType, XmlElement> messages,
                                                                                        Func<NotificationMessageHolderType, bool> messageCheck)
        {
            return messages.Where(e => messageCheck(e.Key)).ToDictionary(e => e.Key, e => e.Value);
        }


        /// <summary>
        /// Sets up notify server for receiving several notifications
        /// </summary>
        /// <param name="server"></param>
        protected void SetupNotifyServer2(Utils.BaseNotifyServer server)
        {
            server.NotificationReceived += LogNotificationStep;
        }

        protected void RemoveHandlers2(Utils.BaseNotifyServer server)
        {
            server.NotificationReceived -= LogNotificationStep;
        }

        protected void LogNotificationStep(byte[] rawData)
        { 
            string content =
                System.Text.Encoding.UTF8.GetString(rawData);
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
            BeginStep("Receive notifications");
            LogResponse(content);
            StepPassed();        
        }

        #region Subscription

        // deprecated
        protected void ReleaseSubscription(System.DateTime subscribeStarted,
            EndpointReferenceType subscriptionReference,
            int timeout)
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
        }

        protected void ReleaseSubscription(System.DateTime subscribeStarted,
            EndpointReferenceType subscriptionReference,
            System.DateTime subscribeTerminationExpected)
        {
            if (subscribeStarted != System.DateTime.MaxValue)
            {
                if (subscriptionReference != null && _subscriptionManagerClient == null)
                {
                    CreateSubscriptionManagerClient(subscriptionReference);
                }
                TimeSpan diff = subscribeTerminationExpected - System.DateTime.Now;
                int releaseTime = (int)diff.TotalMilliseconds;
                ReleaseSubscriptionManager(releaseTime);
            }
        }

        #endregion


        #region Topic construction

        protected TopicInfo ConstructTopic(IEnumerable<string> sequence)
        {
            TopicInfo current = null;

            foreach (string topic in sequence)
            {
                TopicInfo last = current;
                current = new TopicInfo();
                current.ParentTopic = last;
                current.Name = topic;
                if (last == null)
                {
                    current.Namespace = ONVIFTOPICSET;
                    current.NamespacePrefix = "tns1";
                }
            }
            return current;
        }

        #endregion

        #endregion

        #region ValidateTopic

        protected bool ValidatePacsEventTopicSource(XmlElement messageDescription,
            TopicInfo topicInfo, StringBuilder logger, string sourceSimpleItemName)
        {
            bool ok = true;
            XmlElement sourceElement = messageDescription.GetMessageSource();

            if (sourceElement == null)
            {
                logger.AppendLine("Source element is missing");
                ok = false;
            }
            else
            {
                string err;
                bool success;
                Dictionary<string, XmlElement> sourceSimpleItems = messageDescription.GetMessageSourceSimpleItemDescriptions(out success, out err);
                if (success)
                {
                    bool localOk = ValidateSimpleItemDescription(sourceSimpleItems, "Source", sourceSimpleItemName, "ReferenceToken", PTNAMESPACE, true, logger);
                    ok = ok && localOk;
                }
                else 
                {
                    logger.AppendLine("Source element is incorrect: " + err);
                    ok = false;
                }
            }
            return ok;
        }

        /// <summary>
        /// Validates simple item description in Topic
        /// </summary>
        /// <param name="sourceSimpleItems">All simple items</param>
        /// <param name="part">Message description part (source or data)</param>
        /// <param name="itemName">Item name</param>
        /// <param name="itemType">Expected item type</param>
        /// <param name="typeNamespace">Namespace of the type</param>
        /// <param name="logger">Logger object to store error description</param>
        /// <returns>True, if item descipriton is present and correct; false otherwise</returns>
        protected bool ValidateSimpleItemDescription(Dictionary<string, XmlElement> sourceSimpleItems, 
                                                     string part,
                                                     string itemName, 
                                                     string itemType, 
                                                     string typeNamespace, 
                                                     bool mandatory, 
                                                     StringBuilder logger)
        {
            bool ok = true;

            if (sourceSimpleItems.ContainsKey(itemName))
            {
                XmlElement item = sourceSimpleItems[itemName];
                XmlAttribute type = item.Attributes[OnvifMessage.TYPE];
                if (type == null)
                {
                    ok = false;
                    logger.AppendFormat("'Type' attribute is missing for '{0}' simple item{1}", itemName, Environment.NewLine);
                }
                else
                {
                    string error = string.Empty;
                    if (!type.IsCorrectQName(itemType, typeNamespace, item, ref error))
                    {
                        ok = false;
                        logger.AppendFormat("'Type' attribute is incorrect for '{0}' simple item: {1}{2}", itemName, error, Environment.NewLine);
                    }
                }
            }
            else
            {
                if (mandatory)
                {
                    logger.AppendFormat("'{0}' SimpleItemDescription is missing in {1}{2}", itemName, part,
                                        Environment.NewLine);
                    ok = false;
                }
            }

            return ok;
        }
                        
        /// <summary>
        /// Common topic validation 
        /// </summary>
        /// <param name="messageDescription">Message description</param>
        /// <param name="topicInfo">Topic information (for logging purposes)</param>
        protected void ValidateTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            ValidateTopic(messageDescription, topicInfo, true);
        }
        /// <summary>
        /// Common topic validation 
        /// </summary>
        /// <param name="messageDescription">Message description</param>
        /// <param name="topicInfo">Topic information (for logging purposes)</param>
        protected void ValidateTopic(XmlElement messageDescription, TopicInfo topicInfo, bool property)
        { 
            Assert(messageDescription != null, "Message description is missing", "Check that message contains message description");

            {
                bool isProperty = false;
                // check that it is a property
                if (messageDescription.HasAttribute(OnvifMessage.ISPROPERTY))
                {
                    isProperty = XmlConvert.ToBoolean(messageDescription.Attributes[OnvifMessage.ISPROPERTY].Value);
                }

                if (property)
                {
                    Assert(isProperty, string.Format("Event with topic {0} is not a property event", topicInfo.GetDescription()), "Check that the event is correct");
                }
                else
                {
                    Assert(!isProperty, string.Format("Event with topic {0} is a property event", topicInfo.GetDescription()), "Check that the event is correct");
                }
            }        
        }

        protected void ValidatePacsEntityTopic(XmlElement messageDescription,
                                               TopicInfo topicInfo,
                                               MessageDescription messageInfo, 
                                               string sourceSimpleItemName)
        {
            ValidateTopic(messageDescription, topicInfo, messageInfo.IsProperty);

            bool ok = true;
            StringBuilder logger = new StringBuilder();

            // check MessageDescription

            logger.AppendLine(string.Format("Validating topic {0}... ", topicInfo.GetDescription()));

            bool localOk = ValidatePacsEventTopicSource(messageDescription, topicInfo, logger, sourceSimpleItemName);
            ok = ok && localOk;

            // 10.	Check that this event contains Data.SimpleItemDescription item with Name="DoorMonitor" and Type=" tdc:DoorMonitorStateType ".

            if (messageInfo.DataSimpleItems.Count > 0)
            {
                XmlElement dataElement = messageDescription.GetMessageData();
                if (dataElement == null)
                {
                    ok = false;
                    logger.AppendLine("Message Data element is missing");
                }
                else
                {
                    string err;
                    bool success;
                    
                    Dictionary<string, XmlElement> dataSimpleItems = messageDescription.GetMessageDataSimpleItemDescriptions(out success, out err);

                    if (success)
                    {
                        foreach (var itemNameDescriptionPair in messageInfo.DataSimpleItems)
                        {
                            var itemName = itemNameDescriptionPair.Key;
                            var itemDescription = itemNameDescriptionPair.Value;
                            localOk = ValidateSimpleItemDescription(dataSimpleItems, "Data", itemName, itemDescription.Type, itemDescription.Namespace, true, logger);
                            ok = ok && localOk;
                        }
                    }
                    else
                    {
                        ok = false;
                        logger.AppendLine("Data element is incorrect: " + err);
                    }
                }
            }

            Assert(ok, logger.ToStringTrimNewLine(), "Check that Topic is correct");

        }


        #endregion

        #region Validate Messages
        
        protected bool ValidateDataItem(Dictionary<string, string> dataSimpleItems,
            string itemName, string itemType, string schemaNamespace, bool mandatory,
            BaseSchemaSet schemasSet,
            StringBuilder dump)
        {
            bool ok = true;

            if (dataSimpleItems.ContainsKey(itemName))
            {
                string value = dataSimpleItems[itemName];
                // check value
                SchemaTypeValidator validator = new SchemaTypeValidator(schemasSet);
                ok = validator.IsValidTypeValue(value, itemType, schemaNamespace);

                if (!ok)
                {
                    dump.AppendFormat("   '{0}' SimpleItem value ({1}) is incorrect {2}", itemName, value, Environment.NewLine);
                }
            }
            else
            {
                if (mandatory)
                {
                    dump.AppendFormat("   '{0}' SimpleItem is missing in Data{1}", itemName, Environment.NewLine);
                    ok = false;
                }
            }

            return ok;
        }

        /// <summary>
        /// Validates messages.
        /// </summary>
        /// <param name="notificationMessages"></param>
        /// <param name="soapRawPacket">Raw packet to get access to attributes</param>
        /// <param name="topic"> </param>
        /// <param name="data"></param> 
        /// <param name="validateMessageFunction"></param>
        protected void ValidateMessages(NotificationMessageHolderType[] notificationMessages,
            XmlDocument soapRawPacket, 
            TopicInfo topic,
            object data,
            ValidateMessageFunction validateMessageFunction)
        {
            BeginStep("Validate messages");

            /* Find raw elements */

            XmlNamespaceManager manager = CreateNamespaceManager(soapRawPacket);

            Dictionary<NotificationMessageHolderType, XmlElement> rawElements = GetRawElements(notificationMessages,
                                                                                               soapRawPacket,
                                                                                               manager,
                                                                                               true);

            /* raw elements list initialized */

            StringBuilder logger = new StringBuilder();
            bool ok = true;

            MessageCheckSettings settings = new MessageCheckSettings();
            settings.NamespaceManager = manager;
            settings.ExpectedTopic = topic;
            settings.RawMessageElements = rawElements;
            settings.Data = data;

            foreach (NotificationMessageHolderType message in notificationMessages)
            {
                bool local = validateMessageFunction(message, settings, logger);
                ok = ok && local;
            }

            if (!ok)
            {
                throw new AssertException(logger.ToStringTrimNewLine());
            }

            StepPassed();
        }


        protected void ValidateMessages(Dictionary<NotificationMessageHolderType, XmlElement> notifications,
            TopicInfo topic,
            object data,
            ValidateMessageFunction validateMessageFunction)
        {
            ValidateMessages(notifications, topic, null, data, validateMessageFunction);
        }

        protected class EntityListInfo<T>
        {
            public List<T> FullList { get;set; }
            public List<T> FilteredList { get; set; }
        }

        /// <summary>
        /// Validates messages.
        /// </summary>
        /// <param name="notificationMessages"></param>
        /// <param name="soapRawPacket">Raw packet to get access to attributes</param>
        /// <param name="topic"> </param>
        /// <param name="data"></param> 
        /// <param name="validateMessageFunction"></param>
        protected void ValidateMessages(Dictionary<NotificationMessageHolderType, XmlElement> notifications,
                                        TopicInfo topic,
                                        string propertyOperation,
                                        object data,
                                        ValidateMessageFunction validateMessageFunction)
        {
            BeginStep("Validate messages");

            Dictionary<XmlDocument, XmlNamespaceManager> managers = new Dictionary<XmlDocument, XmlNamespaceManager>();
            foreach (NotificationMessageHolderType message in notifications.Keys)
            {
                XmlDocument doc = notifications[message].OwnerDocument;
                if (!managers.ContainsKey(doc))
                {
                    XmlNamespaceManager manager = CreateNamespaceManager(doc);
                    managers.Add(doc, manager);
                }
            }

            StringBuilder logger = new StringBuilder();
            bool ok = true;

            MessageCheckSettings settings = new MessageCheckSettings();
            settings.ExpectedTopic = topic;
            settings.RawMessageElements = notifications;
            settings.Data = data;
            settings.ExpectedPropertyOperation = propertyOperation;

            foreach (NotificationMessageHolderType message in notifications.Keys)
            {
                XmlDocument doc = notifications[message].OwnerDocument;
                settings.NamespaceManager = managers[doc];
                bool local = validateMessageFunction(message, settings, logger);
                ok = ok && local;
            }

            if (!ok)
            {
                throw new AssertException(logger.ToStringTrimNewLine());
            }

            StepPassed();
        }


        protected bool ValidateMessageCommonElements(NotificationMessageHolderType notification,
            XmlElement messageRawElement,
            TopicInfo topicInfo,
            XmlNamespaceManager manager,
            StringBuilder logger)
        {
            return ValidateMessageCommonElements(notification, messageRawElement, topicInfo, null, manager, logger);
        }
        /// <summary>
        /// Validates common elements
        /// </summary>
        /// <param name="notification">Notification</param>
        /// <param name="messageRawElement">Raw notification XML element</param>
        /// <param name="topicInfo">Topic information</param>
        /// <param name="manager">Namespaces manager</param>
        /// <param name="logger">Logger to store error description</param>
        /// <returns></returns>
        protected bool ValidateMessageCommonElements(NotificationMessageHolderType notification,
            XmlElement messageRawElement,
            TopicInfo topicInfo,
            string propertyOperation,
            XmlNamespaceManager manager,
            StringBuilder logger)
        {
            bool ok = true;
            StringBuilder dump = new StringBuilder();

            XmlElement messageElement = notification.Message;
           
            if (messageElement == null)
            {
                dump.AppendLine("Notification without Message element found");
                ok = false;
            }
            else
            {
                // Check that mandatory attribute is present.
                if (!messageElement.HasAttribute(OnvifMessage.UTCTIMEATTRIBUTE))
                {
                    dump.AppendFormat("Mandatory attribute UtcTime not found for a notification");
                    ok = false;
                }
                else
                {
                    string utcTime = messageElement.Attributes[OnvifMessage.UTCTIMEATTRIBUTE].Value;

                    dump.AppendFormat("Message with UTC time = {0} is incorrect: {1}", utcTime, Environment.NewLine);

                    try
                    {
                        System.DateTime timestamp = XmlConvert.ToDateTime(utcTime);
                    }
                    catch (Exception exc)
                    {
                        dump.AppendFormat("   UTC time '{0}' is incorrect: {1}", utcTime, Environment.NewLine);
                        ok = false;
                    }
                }

                // Check topic

                if (notification.Topic == null)
                {
                    dump.AppendLine("   Topic is null");
                    ok = false;
                }
                else
                {
                    // validate topic
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

                    TopicInfo actualTopic = TopicInfo.ExtractTopicInfoPACS(topic, topicNode);

                    TopicInfo currentTopic = actualTopic;
                    while (currentTopic != null)
                    {
                        if (currentTopic.ParentTopic == null && string.IsNullOrEmpty(currentTopic.NamespacePrefix))
                        {
                            dump.AppendFormat("   Topic {0} is incorrect: root topic must have namespace defined{1}", topic, Environment.NewLine);
                            ok = false;
                        }
                        if (!string.IsNullOrEmpty(currentTopic.NamespacePrefix) && string.IsNullOrEmpty(currentTopic.Namespace))
                        {
                            dump.AppendFormat("   Topic {0} is incorrect: namespace prefix {1} not defined{2}", topic, currentTopic.NamespacePrefix, Environment.NewLine);
                            ok = false;
                        }
                        currentTopic = currentTopic.ParentTopic;
                    }

                    if (topicInfo != null)
                    {
                        // validate topic

                        string expectedTopicDescription = topicInfo.GetDescription();
                        string actualTopicDescription = actualTopic.GetDescription();

                        bool match = TopicInfo.TopicsMatch(actualTopic, topicInfo);

                        if (!match)
                        {
                            dump.AppendFormat("   Invalid topic. {0}   Expected: {1}{0}   Actual: {2}{0}",
                                Environment.NewLine,
                                expectedTopicDescription,
                                actualTopicDescription);
                            ok = false;
                        }
                    }
                }

                if (propertyOperation != null)
                {
                    if (messageElement.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                    {
                        XmlAttribute propertyOperationType = messageElement.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];

                        if (propertyOperationType.Value != propertyOperation)
                        {
                            dump.AppendFormat("   Invalid Property Operation. {0}   Expected: {1}{0}   Actual: {2}{0}",
                                Environment.NewLine,
                                propertyOperation,
                                propertyOperationType.Value);

                            ok = false;
                        }
                    }
                    else
                    {
                        dump.Append(string.Format("PropertyOperation attribute not found{0}", Environment.NewLine));
                        ok = false;
                    }                
                }

            }

            //if (!ok)
            {
                logger.Append(dump.ToString());
            }

            return ok;
        }
        
        protected bool ValidateMessageDataSimpleItems(XmlElement dataElement,
            MessageDescription messageInfo, StringBuilder dump)
        {
            bool ok = true;

            if (messageInfo.DataSimpleItems.Where(SI => SI.Value.Mandatory).Count() > 0)
            {
                if (dataElement == null)
                {
                    dump.AppendLine("   Message Data element is missing");
                    ok = false;
                }
                else
                {
                    bool success;
                    string error;
                    Dictionary<string, string> dataSimpleItems = dataElement.GetMessageElementSimpleItems(out success, out error);
                    if (!success)
                    {
                        ok = false;
                        dump.AppendLine(error);
                    }
                    else
                    {
                        foreach (string itemName in messageInfo.DataSimpleItems.Keys)
                        {
                            SimpleItemDescription itemInfo = messageInfo.DataSimpleItems[itemName];
                            bool localOk = ValidateDataItem(dataSimpleItems,
                                itemInfo.Name,
                                itemInfo.Type,
                                itemInfo.Namespace,
                                itemInfo.Mandatory,
                                GetSchemaSet(itemInfo.Namespace),
                                dump);

                            ok = ok && localOk;
                        }
                    }
                }
            }
            return ok;
        }


        #endregion 
        
        protected void ValidateMessagesSet<T>(IEnumerable<NotificationMessageHolderType> messages,
            IEnumerable<T> entities,
            Func<T, string> tokenSelector,
            string entityName,
            string sourceSimpleItemName,
            Dictionary<string, NotificationMessageHolderType> messagesDictionary)
        {
            {
                BeginStep(string.Format("Check that messages for all {0}s are present", entityName));
                bool ok = true;
                StringBuilder logger = new StringBuilder();
                List<string> foundTokens = new List<string>();

                foreach (NotificationMessageHolderType message in messages)
                {
                    XmlElement sourceElement = message.Message.GetMessageSource();

                    Dictionary<string, string> sourceSimpleItems = sourceElement.GetMessageSourceSimpleItems();

                    // if ValidateMessages succeeds, sourceSimpleItemName is present
                    string entityToken = sourceSimpleItems[sourceSimpleItemName];
                    if (foundTokens.Contains(entityToken))
                    {
                        ok = false;
                        logger.AppendLine(string.Format("Notification for token='{0}' was sent more than once", entityToken, Environment.NewLine));
                    }
                    else
                    {
                        foundTokens.Add(entityToken);
                        messagesDictionary.Add(entityToken, message);
                    }
                }

                foreach (T info in entities)
                {
                    string token = tokenSelector(info);
                    if (!foundTokens.Contains(token))
                    {
                        ok = false;
                        logger.AppendLine(string.Format("Notification for token='{0}' was not sent", token, Environment.NewLine));
                    }
                }

                if (!ok)
                {
                    throw new AssertException(logger.ToStringTrimNewLine());
                }
                StepPassed();
            }
        }

        protected bool CheckMessagePropertyOperation(NotificationMessageHolderType message, string propertyOperation)
        {
            BeginStep("Check if the message should be considered");

            bool ok = false;
            XmlElement messageElement = message.Message;

            if (messageElement != null)
            {
                if (messageElement.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                {
                    XmlAttribute propertyOperationType = messageElement.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];

                    if (propertyOperationType.Value == propertyOperation)
                    {
                        ok = true;
                    }
                    else
                    {
                        LogStepEvent("Message filtered out");
                    }
                }
                else
                {
                    LogStepEvent("No PropertyOperation attribute, message filtered out");
                }
            }
            StepPassed();
            return ok;
        }

        BaseSchemaSet GetSchemaSet(string schemaNamespace)
        {
            BaseSchemaSet schemasSet = null;

            //
            // DoorControl messages need DCNAMESPACE schema (enumerations are in use)
            // AccessControl use mostly "ReferenceToken" (can be found in TypesSchemaSet) 
            // or types from xsd schema

            switch (schemaNamespace)
            {
                case XSNAMESPACE:
                    schemasSet = XmlSchemaSet.GetInstance();
                    break;
                case DCNAMESPACE:
                    schemasSet = DoorControlSchemaSet.GetInstance();
                    break;
                default:
                    schemasSet = TypesSchemaSet.GetInstance();
                    break;
            }
            return schemasSet;
        }

        protected void DoRenewBeforePull(ref int terminationTimeSeconds, ref System.DateTime TerminationExpectedTime)
        {
            if (System.DateTime.Now.AddSeconds(58) > TerminationExpectedTime)
            { // renew required
            repeatRenew:

                Renew renew = new Renew();
                renew.TerminationTime = string.Format("PT{0}S", terminationTimeSeconds);
                RenewResponse Resp = null;
                try
                {
                    Resp = Renew(renew);
                }
                catch (FaultException<UnacceptableTerminationTimeFaultType> exc)
                {
                    LogTestEvent("Possible exception - trying to select Renew time and repeat");
                    StepPassed();
                    DateTime minimumTime = GetDateTimeFromFault(exc,
                        "MinimumTime",
                        "http://docs.oasis-open.org/wsn/b-2");

                    Assert(minimumTime != DateTime.MinValue,
                        "Fault details or MinimumTime not found",
                        "Check if MinimumTime is specified");

                    int diffSeconds = Convert.ToInt32((minimumTime - System.DateTime.Now).TotalSeconds + 0.9);
                    if (diffSeconds < 60)
                    {   // just for case, any big value can be there
                        if (terminationTimeSeconds >= 90)
                        {
                            terminationTimeSeconds += 30;
                        }
                        else
                        {
                            terminationTimeSeconds = 90;
                        }
                    }
                    else
                    {
                        terminationTimeSeconds = diffSeconds;
                    }
                    if (terminationTimeSeconds > 300)
                    {
                        LogTestEvent("Can't find Renew time within 300 seconds - terminating test\r\n");
                        throw;
                    }
                    goto repeatRenew;
                }
                if (!Resp.TerminationTime.HasValue)
                {
                    TerminationExpectedTime = System.DateTime.Now.AddSeconds(58);
                }
                else
                {
                    TerminationExpectedTime = Resp.TerminationTime.Value;
                }
            }
        }

        /// <summary>
        /// Wait for specified message on created pullpoint subscription during operationDelayTime
        /// </summary>
        /// <param name="subscriptionReference">Created pullpoint subscription</param>
        /// <param name="messageChecker">Predicate to check if it is a message we are waiting for</param>
        /// <param name="messageLimit">Message limit per count of notifications in the single pullpoint message</param>
        /// <returns></returns>
        protected Dictionary<NotificationMessageHolderType, XmlElement> WaitSpecifiedMessage(EndpointReferenceType subscriptionReference,
                                                                                             Func<NotificationMessageHolderType, bool> messageChecker,
                                                                                             int messageLimit,
                                                                                             ref System.DateTime TerminationExpectedTime)
        {
            var NotificationMessages = new Dictionary<NotificationMessageHolderType, XmlElement>();
            System.DateTime pullingDeadline = System.DateTime.Now.AddSeconds(_operationDelay / 1000.0);
            //try
            {
                string dump;
                NotificationMessageHolderType[] currentNotificationMessages = null;
                int terminationTimeSeconds = 60;

                do
                {
                    DoRenewBeforePull(ref terminationTimeSeconds, ref TerminationExpectedTime);
                    currentNotificationMessages = GetMessages(subscriptionReference, false, false, false, 1, out dump);

                    if (currentNotificationMessages == null)
                        break;

                    Assert(currentNotificationMessages.Count() <= messageLimit, 
                        "Maximum number of messages exceeded",
                        string.Format("Check that DUT sent not more than {0} message(s)", messageLimit));
                    
                    if (DateTime.Now > pullingDeadline)
                        break;
                }
                while ((currentNotificationMessages != null) && !currentNotificationMessages.Any(messageChecker));

                if ((currentNotificationMessages != null) && !currentNotificationMessages.Any(messageChecker))
                {
                    currentNotificationMessages = null;
                }

                if (currentNotificationMessages != null)
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(dump);
                    XmlNamespaceManager manager = CreateNamespaceManager(doc);
                    NotificationMessages = GetRawElements(currentNotificationMessages, doc, manager, UseNotifyToGetEvents);
                }

            }
            //catch (Exception exc)
            //{
            //    StepFailed(exc);
            //}

            return NotificationMessages;
        }

        /// <summary>
        /// Wait all messages on created pullpoint subscription during operationDelayTime
        /// </summary>
        /// <param name="subscriptionReference">Created pullpoint subscription</param>
        /// <param name="messageLimit">Message limit per count of notifications in the single pullpoint message</param>
        /// <returns></returns>
        protected Dictionary<NotificationMessageHolderType, XmlElement> WaitAllMessages(EndpointReferenceType subscriptionReference,
                                                                                        int messageLimit,
                                                                                        ref System.DateTime TerminationExpectedTime)
        {
            var NotificationMessages = new Dictionary<NotificationMessageHolderType, XmlElement>();
            System.DateTime pullingDeadline = System.DateTime.Now.AddSeconds(_operationDelay / 1000.0);
            //try
            {
                NotificationMessageHolderType[] currentNotificationMessages;
                int terminationTimeSeconds = 60;
                do
                {
                    DoRenewBeforePull(ref terminationTimeSeconds, ref TerminationExpectedTime);
                    string dump;
                    currentNotificationMessages = GetMessages(subscriptionReference, false, false, false, 1, out dump);
                    if (currentNotificationMessages == null)
                    {
                        break;
                    }
                    Assert(currentNotificationMessages.Count() <= messageLimit, 
                        "Maximum number of messages exceeded",
                        string.Format("Check that DUT sent not more than {0} message(s)", messageLimit));

                    //PullMessage request is made before timeout expiration,
                    //PullMessage response is recieved after timeout expiration.
                    //Received messages should be processed.
                    if (currentNotificationMessages.Any())
                    {
                        var doc = new XmlDocument();
                        doc.LoadXml(dump);
                        XmlNamespaceManager manager = CreateNamespaceManager(doc);
                        var messages = GetRawElements(currentNotificationMessages, doc, manager, UseNotifyToGetEvents);


                        foreach (var e in messages)
                        { NotificationMessages.Add(e.Key, e.Value); }

                        if (DateTime.Now > pullingDeadline)
                        {
                            break;
                        }
                    } 
                    else
                    {
                        if (DateTime.Now > pullingDeadline)
                        {
                            break;
                        }
                    }
                }
                while (true);
            }
            //catch (Exception exc)
            //{
            //    StepFailed(exc);
            //}

            return NotificationMessages;
        }


    }
}
