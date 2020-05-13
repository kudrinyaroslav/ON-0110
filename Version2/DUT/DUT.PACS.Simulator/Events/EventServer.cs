using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using DUT.PACS.Simulator.BackDoorServices;
using DUT.PACS.Simulator.Events10;
using DUT.PACS.Simulator.ServiceDoorControl10;
using System.ServiceModel;
using DUT.PACS.Simulator.ServiceAccessControl10;
using DUT.PACS.Simulator.ServiceCredential10;
using DUT.PACS.Simulator.ServiceAccessRules10;

namespace DUT.PACS.Simulator.Events
{
    /// <summary>
    /// Entity for processing subscribtions and notifications
    /// </summary>
    public class EventServer
    {
        private bool _isExternalResponseDone = false;
        private const int TIMEOUT = 10000;

        private delegate void NotifyDelegate(string consumerReference, NotificationMessageHolderType notification);

        void Notify(string consumerReference, NotificationMessageHolderType notification)
        {

            WSHttpBinding binding = new WSHttpBinding(SecurityMode.None);

            NotificationConsumerClient client = new NotificationConsumerClient(binding, new EndpointAddress(consumerReference));

            Notify notify = new Notify();

            notify.NotificationMessage = new NotificationMessageHolderType[] { notification };

            try
            {
                client.Notify(notify);
            }
            catch (Exception exc)
            {
                ExternalLogger.LogMessage(
                    string.Format("Sending notification to {0} FAILED", consumerReference),
                    ExternalLogging.MessageType.Error);
            }

        }

        #region Memebers

        /// <summary>
        /// TopicSet supported by DUT.PACS.Simulator
        /// </summary>
        XmlElement[] m_TopicSet;

        //Basic Notifications

        /// <summary>
        /// Subscription List
        /// </summary>
        Dictionary<int, EventSubsciption> m_eventSubsciptionList = new Dictionary<int, EventSubsciption>();

        /// <summary>
        /// Notification Lists for each subscribtion
        /// </summary>
        Dictionary<int, EventList> m_eventList = new Dictionary<int, EventList>();

        /// <summary>
        /// Current key for new subscribtion
        /// </summary>
        int m_currentKey = 0;

        //Services with notifications

        /// <summary>
        /// Referance to DoorControlService object
        /// </summary>
        DoorControlService m_doorControlService;

        /// <summary>
        /// Reference to PACS service
        /// </summary>
        PACSService m_pacsService;

        /// <summary>
        /// Reference to Credential service
        /// </summary>
        CredentialService m_credentialService;

        /// <summary>
        /// Reference to Access Rules service
        /// </summary>
        AccessRulesService m_accessRulesService;

        /// <summary>
        /// Reference to external logger
        /// </summary>
        private ExternalLogging.LoggingService m_externalLogger;

        XmlDocument m_TopicSetXmlDocument;

        //private List<CredentialInformation> m_credentialInfoList;

        #endregion //Members

        #region Properties

        /// <summary>
        /// Gets Event Basic Subsciption List
        /// </summary>
        public Dictionary<int, EventSubsciption> EventSubsciptionList
        {
            get { return m_eventSubsciptionList; }
        }

        /// <summary>
        /// Gets or sets referance to DoorControlService
        /// </summary>
        public DoorControlService DoorControlService
        {
            get { return m_doorControlService; }
            set { m_doorControlService = value; }
        }

        /// <summary>
        /// Gets or sets referance to PACSService
        /// </summary>
        public PACSService PACSService
        {
            get { return m_pacsService; }
            set { m_pacsService = value; }
        }

        /// <summary>
        /// Gets or sets referance to CredentialService
        /// </summary>
        public CredentialService CredentialService
        {
            get { return m_credentialService; }
            set { m_credentialService = value; }
        }

        /// <summary>
        /// Gets or sets referance to AccessRulesService
        /// </summary>
        public AccessRulesService AccessRulesService
        {
            get { return m_accessRulesService; }
            set { m_accessRulesService = value; }
        }

        public ExternalLogging.LoggingService ExternalLogger
        {
            get { return m_externalLogger; }
            set { m_externalLogger = value; }
        }

        /// <summary>
        /// Gets TopicSet supported by DUT.PACS.Simulator
        /// </summary>
        public XmlElement[] TopicSet
        {
            get { return m_TopicSet; }
        }

        public TopicSet TopicsTree
        {
            get { return PACSTopicSet.Instance; }
        }

        //public List<CredentialInformation> CredentialInfoList
        //{
        //    get
        //    {
        //        if (m_credentialInfoList == null)
        //            m_credentialInfoList = new List<CredentialInformation>();
        //        return m_credentialInfoList;
        //    }
        //    set { m_credentialInfoList = value; }
        //}


        #endregion //Properties

        #region Constructor

        /// <summary>
        /// EventServer constructor
        /// </summary>
        public EventServer()
        {
            m_TopicSet = TopicSetCreation();
        }

        #endregion //Constructor

        #region PublicMethods

        /// <summary>
        /// Adds subscribtion to subscription list.
        /// </summary>
        /// <param name="consumerReference">Consumer referance (null for Pull Point Subscriptions)</param>
        /// <param name="filter">Filter value</param>
        /// <param name="filterElement"></param>
        /// <param name="terminationTime">Termination time of subscription in UTC</param>
        /// <returns>Subscribtion ID</returns>
        public int AddSubscribtion(string consumerReference,
            FilterType filter,
            XmlElement filterElement,
            DateTime terminationTime,
            bool nullInitialTerminationTime)
        {
            RemoveExpieredSubscription();

            var item = new EventSubsciption();
            var pullPointEventList = new EventList();
            item.PullPointAsRenew = nullInitialTerminationTime;
            item.ConsumerReference = consumerReference;

            ExternalLogger.LogMessage(string.Format("Subscription requested for {0}", consumerReference), ExternalLogging.MessageType.Details);

            if (filter != null)
            {
                // look for XmlText node (skip whitespaces and other trash)
                foreach (XmlNode node in filterElement.ChildNodes)
                {
                    XmlText filterText = node as XmlText;
                    // if found
                    if (filterText != null)
                    {
                        string filterString = filterText.Value;

                        // split to single topics
                        string[] topics = filterString.Split('|');
                        foreach (string topicString in topics)
                        {
                            SubscriptionTopicFilter topic = SubscriptionTopicFilter.Create(PACSTopicSet.Instance,
                                                                                           filterText,
                                                                                           topicString.Trim());
                            if (topic != null)
                            {
                                item.AddTopic(topic);
                                pullPointEventList.AddTopic(topic);
                            }
                            else
                            {
                                string message = string.Format("Topic not found: {0}", topicString);
                                ExternalLogger.LogMessage(message,
                                                          ExternalLogging.MessageType.Warning);
                                throw FaultLib.GetSoapException(FaultType.General, message);
                            }
                        }
                    }
                    break;
                }
            }

            item.TerminationTime = terminationTime;

            m_currentKey++;

            m_eventSubsciptionList.Add(m_currentKey, item);
            m_eventList.Add(m_currentKey, pullPointEventList);

            ExternalLogger.LogMessage(string.Format("Subscription created (key={0})", m_currentKey), ExternalLogging.MessageType.Message);

            return m_currentKey;
        }

        /// <summary>
        /// Remuve subscribtion from subscribtion list
        /// </summary>
        /// <param name="id">Subscribtion ID</param>
        public void RemoveSubscribtion(int id)
        {
            RemoveExpieredSubscription();
            if (m_eventSubsciptionList.ContainsKey(id))
            {
                m_eventSubsciptionList.Remove(id);
                m_eventList.Remove(id);

                ExternalLogger.LogMessage(string.Format("Subscription removed (key={0})", id), ExternalLogging.MessageType.Message);

            }
            else
            {
                ExternalLogger.LogMessage(string.Format("Subscription cannot be removed: key={0} not found", id), ExternalLogging.MessageType.Error);
                throw FaultLib.GetSoapException(FaultType.General, "Unknown reference: " + id.ToString() + ".");
            }
        }

        /// <summary>
        /// Renew subscribtion with new termination time
        /// </summary>
        /// <param name="id">Subscribtion ID</param>
        /// <param name="terminationTime">New termination time in UTC</param>
        public void RenewSubscribtion(int id, DateTime terminationTime)
        {
            RemoveExpieredSubscription();
            if (m_eventSubsciptionList.ContainsKey(id))
            {
                m_eventSubsciptionList[id].TerminationTime = terminationTime;
            }
            else
            {
                throw FaultLib.GetSoapException(FaultType.General, "Unknown reference: " + id.ToString() + ".");
            }
        }

        /// <summary>
        /// Get notifications from queue
        /// </summary>
        /// <param name="id">Subscribtion ID</param>
        /// <param name="timeoutLimit">Wait for subscribtion time limit</param>
        /// <param name="messageLimit">Number of notifications to return limit</param>
        /// <returns>Return first notifications from quiue with number not more then messageLimit not later then timeoutLimit</returns>
        public NotificationMessageHolderType[] GetPullPointMessages(int id, DateTime timeoutLimit, int messageLimit)
        {
            NotificationMessageHolderType[] res;
            List<NotificationMessageHolderType> tempRes = new List<NotificationMessageHolderType>();

            RemoveExpieredSubscription();
            //while ((DateTime.UtcNow <= timeoutLimit) && (tempRes.Count < messageLimit))
            //{
            //    if (m_eventList[id].NotificationExist())
            //    {
            //        tempRes.Add(m_eventList[id].GetNotification());
            //    }
            //}



            AutoResetEvent listFull = new AutoResetEvent(false);
            EventList list = m_eventList[id];

            if (list.Count >= messageLimit)
            {
                for (int i = 0; i < messageLimit; i++)
                { tempRes.Add(list.GetNotification()); }
            }
            else
            {
                EventHandler handler = new EventHandler((sender, args) =>
                {
                    tempRes.Add(list.GetNotification());
                    if (tempRes.Count == messageLimit)
                    {
                        listFull.Set();
                    }
                });
                list.NewNotifications += handler;

                int timeout = (int)((timeoutLimit - DateTime.UtcNow).TotalMilliseconds);
                WaitHandle.WaitAny(new WaitHandle[] { listFull }, timeout);

                list.NewNotifications -= handler;
            }

            res = tempRes.ToArray();

            if (EventSubsciptionList[id].PullPointAsRenew)
            {
                EventSubsciptionList[id].TerminationTime = DateTime.UtcNow.AddSeconds(10);
            }

            return res;
        }

        int? _synchronizationPointRequester;

        /// <summary>
        /// Synchronize property events
        /// </summary>
        /// <param name="id">Subscribtion ID</param>
        public void SynchronizationPoint(int id)
        {
            ExternalLogger.LogMessage("Synchronization point requested", ExternalLogging.MessageType.Details);


            RemoveExpieredSubscription();
            if (m_eventSubsciptionList.ContainsKey(id))
            {
                _synchronizationPointRequester = id;

                //if (m_eventSubsciptionList[id].Filter == null)
                {
                    //TODO: FILTER
                    if (m_doorControlService == null)
                    {
                        m_doorControlService = new DoorControlService();
                    }
                    m_doorControlService.SynchronizationPoint();

                    if (m_pacsService == null)
                    {
                        m_pacsService = new PACSService();
                    }
                    m_pacsService.SynchronizationPoint();
                    
                }

                _synchronizationPointRequester = null;

            }
            else
            {
                ExternalLogger.LogMessage(string.Format("Key {0} not found", id), ExternalLogging.MessageType.Error);
                throw FaultLib.GetSoapException(FaultType.General, "Unknown reference: " + id.ToString() + ".");
            }

        }

        #endregion //PublicMethods

        #region PrivateMethods

        /// <summary>
        /// Removes all subscribtion that are expiered
        /// </summary>
        private void RemoveExpieredSubscription()
        {
            List<int> subscriptionsToRemove = new List<int>();

            foreach (KeyValuePair<int, EventSubsciption> eventSubsciption in EventSubsciptionList)
            {
                if (eventSubsciption.Value.TerminationTime <= DateTime.UtcNow)
                {
                    subscriptionsToRemove.Add(eventSubsciption.Key);
                }
            }
            foreach (int key in subscriptionsToRemove)
            {
                EventSubsciptionList.Remove(key);
                m_eventList.Remove(key);
            }
        }

        /// <summary>
        /// Creates Topic Set for DUT.PACS.Simulator
        /// </summary>
        /// <returns>Topic Set</returns>

        private XmlElement[] TopicSetCreation()
        {
            NameTable nt = new NameTable();
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(nt);
            namespaceManager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            namespaceManager.AddNamespace("tt", "http://www.onvif.org/ver10/schema");
            namespaceManager.AddNamespace("tdc", "http://www.onvif.org/ver10/doorcontrol/wsdl");
            namespaceManager.AddNamespace("wstop", "http://docs.oasis-open.org/wsn/t-1");
            namespaceManager.AddNamespace("tns1", "http://www.onvif.org/ver10/topics");

            //DoorControl
            m_TopicSetXmlDocument = new XmlDocument(nt);
            XmlElement root = m_TopicSetXmlDocument.CreateElement("root");
            PACSTopicSet.Instance.AddTo(root);

            XmlElement[] res = new XmlElement[root.ChildNodes.Count];
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                res[i] = root.ChildNodes[i] as XmlElement;
            }
            return res;
        }

        /// <summary>
        /// Generates notification message (common case)
        /// </summary>
        /// <param name="topicValue"></param>
        /// <param name="sourceItems"></param>
        /// <param name="dataItems"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private NotificationMessageHolderType GenerateNotificationMessage(string topicValue,
            Dictionary<string, string> sourceItems,
            Dictionary<string, string> dataItems,
            string operation)
        {
            NotificationMessageHolderType res = new NotificationMessageHolderType();

            NameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            nsmgr.AddNamespace("tt", "http://www.onvif.org/ver10/schema");
            nsmgr.AddNamespace("tdc", "http://www.onvif.org/ver10/doorcontrol/wsdl");
            nsmgr.AddNamespace("wsnt", "http://docs.oasis-open.org/wsn/b-2");

            XmlDocument xmlDocument = new XmlDocument(nt);

            res.Topic = new TopicExpressionType();
            res.Topic.Dialect = "http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";
            res.Topic.Any = new XmlNode[1];
            res.Topic.Any[0] = xmlDocument.CreateTextNode(topicValue);
            res.Topic.Xmlns = new System.Xml.Serialization.XmlSerializerNamespaces();
            res.Topic.Xmlns.Add("tns1", "http://www.onvif.org/ver10/topics");

            //Message
            res.Message = xmlDocument.CreateElement("tt", "Message", "http://www.onvif.org/ver10/schema");

            //Message[UtcTime]
            XmlAttribute utcTime = xmlDocument.CreateAttribute("UtcTime");
            utcTime.Value = System.Xml.XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc);
            res.Message.Attributes.Append(utcTime);

            //Message[PropertyOperation]
            if (operation != null)
            {
                XmlAttribute propertyOperation = xmlDocument.CreateAttribute("PropertyOperation");
                propertyOperation.Value = operation;
                res.Message.Attributes.Append(propertyOperation);
            }

            //Message/Source
            XmlElement source = xmlDocument.CreateElement("tt", "Source", "http://www.onvif.org/ver10/schema");
            res.Message.AppendChild(source);

            foreach (string sourceName in sourceItems.Keys)
            {
                string sourceValue = sourceItems[sourceName];

                //Message/Source/SimpleItem
                XmlElement simpleItem = xmlDocument.CreateElement("tt", "SimpleItem", "http://www.onvif.org/ver10/schema");
                source.AppendChild(simpleItem);

                //Message/Source/SimpleItem[Name]
                XmlAttribute name = xmlDocument.CreateAttribute("Name");
                name.Value = sourceName;
                simpleItem.Attributes.Append(name);

                //Message/Source/SimpleItem[Value]
                XmlAttribute value = xmlDocument.CreateAttribute("Value");
                value.Value = sourceValue;
                simpleItem.Attributes.Append(value);
            }

            //Message/Data
            if (dataItems != null)
            {
                XmlElement data = xmlDocument.CreateElement("tt", "Data", "http://www.onvif.org/ver10/schema");
                res.Message.AppendChild(data);

                foreach (string dataName in dataItems.Keys)
                {
                    string dataValue = dataItems[dataName];

                    //Message/Data/SimpleItem
                    XmlElement simpleItem = xmlDocument.CreateElement("tt", "SimpleItem", "http://www.onvif.org/ver10/schema");
                    data.AppendChild(simpleItem);

                    //Message/Data/SimpleItem[Name]
                    XmlAttribute name = xmlDocument.CreateAttribute("Name");
                    name.Value = dataName;
                    simpleItem.Attributes.Append(name);

                    //Message/Data/SimpleItem[Value]
                    XmlAttribute value = xmlDocument.CreateAttribute("Value");
                    value.Value = dataValue;
                    simpleItem.Attributes.Append(value);
                }
            }

            return res;
        }

        /// <summary>
        /// Generate Notification Message
        /// </summary>
        /// <param name="topicValue">Notification Message topic</param>
        /// <param name="sourceName">Notification Message source name</param>
        /// <param name="sourceValue">Notification Message source value</param>
        /// <param name="dataName">Notification Message data name</param>
        /// <param name="dataValue">Notification Message data value</param>
        /// <param name="operation">Notification Message operation</param>
        /// <returns>Notification Message</returns>
        private NotificationMessageHolderType GenerateNotificationMessage(string topicValue,
            string sourceName,
            string sourceValue,
            string dataName,
            string dataValue,
            string operation)
        {
            Dictionary<string, string> source = new Dictionary<string, string>();
            source.Add(sourceName, sourceValue);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(dataName, dataValue);

            return GenerateNotificationMessage(topicValue, source, data, operation);
        }

        private NotificationMessageHolderType GenerateDoorNotificationMessage(string topicValue,
            DoorPropertyEventArgs args)
        {
            return GenerateNotificationMessage(topicValue,
                "DoorToken",
                args.DoorToken,
                args.PropertyName,
                args.PropertyValue,
                args.PropertyOperation);
        }



        private NotificationMessageHolderType GenerateNotificationMessage(string topicValue,
            PropertyEventArgs args)
        {
            return GenerateNotificationMessage(topicValue,
                args.GetSource(),
                args.GetData(),
                args.PropertyOperation);
        }

        private NotificationMessageHolderType GenerateNotificationMessage(string topicValue,
            NotPropertyEventArgs args)
        {
            return GenerateNotificationMessage(topicValue,
                args.GetSource(),
                args.GetData(),
                null);
        }


        #endregion //PrivateMethods

        #region EventHandlers

        void NotifyAll(NotificationMessageHolderType notificationMessageHolder, Topic topic)
        {
            RemoveExpieredSubscription();

            string message = string.Format("Notification is being sent [Topic= {0}]", topic.GetDescription());
            ExternalLogger.LogMessage(message, ExternalLogging.MessageType.Message);

            foreach (KeyValuePair<int, EventSubsciption> eventSubsciption in m_eventSubsciptionList)
            {
                if (_synchronizationPointRequester.HasValue)
                {
                    if (_synchronizationPointRequester.GetValueOrDefault() != eventSubsciption.Key)
                    {
                        continue;    
                    }
                }

                if (eventSubsciption.Value.SubscribedTo(topic))
                {
                    m_eventList[eventSubsciption.Key].AddNotification(notificationMessageHolder);

                    if (eventSubsciption.Value.ConsumerReference != null)
                    {
                        message = string.Format("Notify {0}", eventSubsciption.Value.ConsumerReference);
                        ExternalLogger.LogMessage(message, ExternalLogging.MessageType.Details);

                        NotifyDelegate func = new NotifyDelegate(Notify);
                        func.BeginInvoke(eventSubsciption.Value.ConsumerReference, m_eventList[eventSubsciption.Key].GetNotification(), null, null);
                    }
                }
            }
        }

        #region DoorModeEvents

        /// <summary>
        /// Declare an event of delegate type EventHandler of DoorModePropertyEventArgs
        /// </summary>
        public event EventHandler<DoorModePropertyEventArgs> DoorModePropertyEvent;

        /// <summary>
        /// Catch a Door Mode Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Mode State</param>
        public void DoorModeEvent(DoorControlService doorControlService, string propertyOperation, string doorToken, DoorMode currentState)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<DoorModePropertyEventArgs> temp = DoorModePropertyEvent;
            if (temp != null)
            {
                temp(this, new DoorModePropertyEventArgs(doorControlService, DateTime.UtcNow, propertyOperation, doorToken, currentState));
            }
        }

        /// <summary>
        /// Add Door Mode Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="doorModePropertyEventArgs">Event arguments</param>
        public void DoorModeEventHandler(object src, DoorModePropertyEventArgs doorModePropertyEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateDoorNotificationMessage("tns1:Door/State/DoorMode",
                                                doorModePropertyEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.DoorModeTopic);

            System.Diagnostics.Debug.WriteLine(doorModePropertyEventArgs.DoorToken + ": state " + doorModePropertyEventArgs.CurrentState.ToString());
        }

        #endregion //DoorModeEvents

        #region DoorPhysicalStateEvents

        /// <summary>
        /// Declare an event of delegate type EventHandler of DoorPhysicalStatePropertyEventArgs
        /// </summary>
        public event EventHandler<DoorPhysicalStatePropertyEventArgs> DoorPhysicalStatePropertyEvent;

        /// <summary>
        /// Catch a Door PhysicalState Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door PhysicalState</param>
        public void DoorPhysicalStateEvent(DoorControlService doorControlService, string propertyOperation, string doorToken, DoorPhysicalState currentState)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<DoorPhysicalStatePropertyEventArgs> temp = DoorPhysicalStatePropertyEvent;
            if (temp != null)
            {
                temp(this, new DoorPhysicalStatePropertyEventArgs(doorControlService, DateTime.UtcNow, propertyOperation, doorToken, currentState));
            }
        }

        /// <summary>
        /// Add Door Monitor Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="doorPhysicalStatePropertyEventArgs">Event arguments</param>
        public void DoorPhysicalStateEventHandler(object src, DoorPhysicalStatePropertyEventArgs doorPhysicalStatePropertyEventArgs)
        {
            NotificationMessageHolderType notificationMessageHolderType =
                GenerateDoorNotificationMessage("tns1:Door/State/DoorPhysicalState",
                                                doorPhysicalStatePropertyEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.DoorPhysicalState);

            System.Diagnostics.Debug.WriteLine(doorPhysicalStatePropertyEventArgs.DoorToken + ": state " + doorPhysicalStatePropertyEventArgs.CurrentState.ToString());
        }

        #endregion //DoorPhysicalStateEvents

        #region DoubleLockPhysicalStateEvents

        /// <summary>
        /// Declare an event of delegate type EventHandler of DoorModePropertyEventArgs
        /// </summary>
        public event EventHandler<DoubleLockPhysicalStatePropertyEventArgs> DoubleLockPhysicalStatePropertyEvent;

        /// <summary>
        /// Catch a Door PhysicalState Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door PhysicalState</param>
        public void DoubleLockPhysicalStateEvent(DoorControlService doorControlService,
            string propertyOperation,
            string doorToken,
            LockPhysicalState currentState)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<DoubleLockPhysicalStatePropertyEventArgs> temp = DoubleLockPhysicalStatePropertyEvent;
            if (temp != null)
            {
                temp(this, new DoubleLockPhysicalStatePropertyEventArgs(doorControlService, DateTime.UtcNow, propertyOperation, doorToken, currentState));
            }
        }

        /// <summary>
        /// Add Door Monitor Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="doubleLockPhysicalStatePropertyEventArgs">Event arguments</param>
        public void DoubleLockPhysicalStatePropertyEventHandler(object src,
            DoubleLockPhysicalStatePropertyEventArgs doubleLockPhysicalStatePropertyEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateDoorNotificationMessage("tns1:Door/State/DoubleLockPhysicalState",
                                                doubleLockPhysicalStatePropertyEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.DoubleLockPhysicalState);

            System.Diagnostics.Debug.WriteLine(doubleLockPhysicalStatePropertyEventArgs.DoorToken + ": state " + doubleLockPhysicalStatePropertyEventArgs.DoubleLockPhysicalState.ToString());
        }

        #endregion //DoorDoubleLockMonitorEvents

        #region LockPhysicalStateEvents

        /// <summary>
        /// Declare an event of delegate type EventHandler of DoorModePropertyEventArgs
        /// </summary>
        public event EventHandler<LockPhysicalStatePropertyEventArgs> LockPhysicalStatePropertyEvent;

        /// <summary>
        /// Catch a Door PhysicalState Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door PhysicalState</param>
        public void LockPhysicalStateEvent(DoorControlService doorControlService,
            string propertyOperation,
            string doorToken,
            LockPhysicalState currentState)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<LockPhysicalStatePropertyEventArgs> temp = LockPhysicalStatePropertyEvent;
            if (temp != null)
            {
                temp(this, new LockPhysicalStatePropertyEventArgs(doorControlService, DateTime.UtcNow, propertyOperation, doorToken, currentState));
            }
        }

        /// <summary>
        /// Add Door Monitor Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="lockPhysicalStatePropertyEventArgs">Event arguments</param>
        public void LockPhysicalStatePropertyEventHandler(object src,
            LockPhysicalStatePropertyEventArgs lockPhysicalStatePropertyEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateDoorNotificationMessage("tns1:Door/State/LockPhysicalState",
                                                lockPhysicalStatePropertyEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.LockPhysicalState);

            System.Diagnostics.Debug.WriteLine(lockPhysicalStatePropertyEventArgs.DoorToken + ": state " + lockPhysicalStatePropertyEventArgs.LockPhysicalState.ToString());
        }

        #endregion //DoorDoubleLockMonitorEvents

        #region DoorTamperEvents

        /// <summary>
        /// Declare an event of delegate type EventHandler of DoorModePropertyEventArgs
        /// </summary>
        public event EventHandler<DoorTamperPropertyEventArgs> DoorTamperPropertyEvent;

        /// <summary>
        /// Catch a Door Tamper Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Monitor State</param>
        public void DoorTamperMonitorEvent(DoorControlService doorControlService,
            string propertyOperation,
            string doorToken,
            DoorTamperState currentState)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<DoorTamperPropertyEventArgs> temp = DoorTamperPropertyEvent;
            if (temp != null)
            {
                temp(this, new DoorTamperPropertyEventArgs(doorControlService, DateTime.UtcNow, propertyOperation, doorToken, currentState));
            }
        }

        /// <summary>
        /// Add Door Monitor Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="doorTamperPropertyEventArgs">Event arguments</param>
        public void DoorTamperPropertyEventHandler(object src,
            DoorTamperPropertyEventArgs doorTamperPropertyEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateDoorNotificationMessage("tns1:Door/State/DoorTamper",
                                                doorTamperPropertyEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.DoorTamper);

            System.Diagnostics.Debug.WriteLine(doorTamperPropertyEventArgs.DoorToken + ": state " + doorTamperPropertyEventArgs.DoorTamperState.ToString());
        }

        #endregion //DoorTamperEvents

        #region DoorAlarmEvents

        /// <summary>
        /// Declare an event of delegate type EventHandler of DoorAlarmPropertyEventArgs
        /// </summary>
        public event EventHandler<DoorAlarmPropertyEventArgs> DoorAlarmPropertyEvent;

        /// <summary>
        /// Catch a Door Tamper Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Monitor State</param>
        public void DoorAlarmMonitorEvent(DoorControlService doorControlService,
            string propertyOperation,
            string doorToken,
            DoorAlarmState currentState)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<DoorAlarmPropertyEventArgs> temp = DoorAlarmPropertyEvent;
            if (temp != null)
            {
                temp(this, new DoorAlarmPropertyEventArgs(doorControlService, DateTime.UtcNow, propertyOperation, doorToken, currentState));
            }
        }

        /// <summary>
        /// Add Door Monitor Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="doorAlarmPropertyEventArgs">Event arguments</param>
        public void DoorAlarmPropertyEventHandler(object src,
            DoorAlarmPropertyEventArgs doorAlarmPropertyEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateDoorNotificationMessage("tns1:Door/State/DoorAlarm",
                                                doorAlarmPropertyEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.DoorAlarm);

            System.Diagnostics.Debug.WriteLine(doorAlarmPropertyEventArgs.DoorToken + ": state " + doorAlarmPropertyEventArgs.DoorAlarmState.ToString());
        }

        #endregion //DoorAlarmEvents


        #region DoorFaultEvents

        /// <summary>
        /// Declare an event of delegate type EventHandler of DoorAlarmPropertyEventArgs
        /// </summary>
        public event EventHandler<DoorFaultPropertyEventArgs> DoorFaultPropertyEvent;

        /// <summary>
        /// Catch a Door Tamper Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Monitor State</param>
        public void DoorFaultEvent(DoorControlService doorControlService,
            string propertyOperation,
            string doorToken,
            DoorFaultState currentState)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<DoorFaultPropertyEventArgs> temp = DoorFaultPropertyEvent;
            if (temp != null)
            {
                temp(this, new DoorFaultPropertyEventArgs(doorControlService, DateTime.UtcNow, propertyOperation, doorToken, currentState));
            }
        }

        /// <summary>
        /// Add Door Fault Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="doorAlarmPropertyEventArgs">Event arguments</param>
        public void DoorFaultPropertyEventHandler(object src,
            DoorFaultPropertyEventArgs doorFaultPropertyEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateDoorNotificationMessage("tns1:Door/State/DoorFault",
                                                doorFaultPropertyEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.DoorFault);

            System.Diagnostics.Debug.WriteLine(doorFaultPropertyEventArgs.DoorToken + ": state " + doorFaultPropertyEventArgs.DoorFaultState.ToString());
        }

        #endregion //DoorAlarmEvents


        #region AccessControl

        #region AccessPoint

        #region EnabledEvents
        /// <summary>
        /// Declare an event of delegate type EventHandler of TamperingEvent
        /// </summary>
        public event EventHandler<EnabledPropertyEventArgs> AccessPointEnabledPropertyEvent;

        /// <summary>
        /// Catch a EnabledEvent Event
        /// </summary>
        /// <param name="pacsService">Access Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="accessController">Access Controlles Token</param>
        /// <param name="active">Current  State</param>
        /// <param name="reason">Reason</param>
        public void AccessPointEnabledEvent(PACSService pacsService,
            string propertyOperation,
            string accessController,
            bool active,
            string reason)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<EnabledPropertyEventArgs> temp = AccessPointEnabledPropertyEvent;
            if (temp != null)
            {
                temp(this, new EnabledPropertyEventArgs(pacsService, DateTime.UtcNow, propertyOperation, accessController) { Enabled = active, Reason = reason });
            }
        }



        /// <summary>
        /// Add Enabled Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="enabledPropertyEventArgs"> Event arguments </param>
        public void AccessPointEnabledPropertyEventHandler(object src,
            EnabledPropertyEventArgs enabledPropertyEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateNotificationMessage("tns1:AccessPoint/State/Enabled",
                                                enabledPropertyEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.AccessControlEnabled);

            System.Diagnostics.Debug.WriteLine(enabledPropertyEventArgs.AccessPointToken + ": state " + enabledPropertyEventArgs.Enabled.ToString());
        }
        #endregion //EnabledEvents

        #region TamperingEvents
        /// <summary>
        /// Declare an event of delegate type EventHandler of TamperingEvent
        /// </summary>
        public event EventHandler<TamperingPropertyEventArgs> AccessPointTamperingPropertyEvent;

        /// <summary>
        /// Catch a TamperingEvent Event
        /// </summary>
        /// <param name="pacsService">Access Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="accessController">Access Controlles Token</param>
        /// <param name="active">Current  State</param>
        /// <param name="reason">Reason</param>
        public void AccessPointTamperingEvent(PACSService pacsService,
            string propertyOperation,
            string accessController,
            bool active,
            string reason)
        {
            //Tampering event is removed
            // Copy to a temporary variable to be thread-safe.
            //EventHandler<TamperingPropertyEventArgs> temp = AccessPointTamperingPropertyEvent;
            //if (temp != null)
            //{
            //    temp(this, new TamperingPropertyEventArgs(pacsService, DateTime.UtcNow, propertyOperation, accessController) { Active = active, Reason = reason });
            //}
        }

        /// <summary>
        /// Add Tampering Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="tamperingPropertyEventArgs">Event arguments</param>
        public void AccessPointTamperingPropertyEventHandler(object src,
            TamperingPropertyEventArgs tamperingPropertyEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateNotificationMessage("tns1:AccessPoint/State/Tampering",
                                                tamperingPropertyEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.AccessControlTampering);

            System.Diagnostics.Debug.WriteLine(tamperingPropertyEventArgs.AccessPointToken + ": state " + tamperingPropertyEventArgs.Active.ToString());
        }
        #endregion //TamperingEvents

        #endregion //AccessPoint

        //    #region Denied
        //    public event EventHandler<AccessDeniedCredentialExternalEventArgs> AccessDeniedCredentialExternalEvent;

        //    public void DeniedCredentialExternalEvent(PACSService pacsService,
        //        string propertyOperation,
        //        string accessController,
        //        string credentialToken,
        //        string credentialHolderName,
        //        string reason)
        //    {
        //        // Copy to a temporary variable to be thread-safe.
        //        EventHandler<AccessDeniedCredentialExternalEventArgs> temp = AccessDeniedCredentialExternalEvent;
        //        if (temp != null)
        //        {
        //            temp(this, 
        //                new AccessDeniedCredentialExternalEventArgs(pacsService, DateTime.UtcNow, propertyOperation, accessController)
        //                    {
        //                        CredentialToken=credentialToken, 
        //                        CredentialHolderName = credentialHolderName,
        //                        Reason = reason
        //                    });
        //        }
        //    }

        //    public void DeniedCredentialExternalEventHandler(object src,
        //AccessDeniedCredentialExternalEventArgs deniedCredentialExternalEventArgs)
        //    {

        //        NotificationMessageHolderType notificationMessageHolderType =
        //            GenerateNotificationMessage("tns1:AccessControl/Denied/Credential/External",
        //                                            deniedCredentialExternalEventArgs);

        //        NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.DeniedCredentialExternal);

        //        System.Diagnostics.Debug.WriteLine(deniedCredentialExternalEventArgs.AccessPointToken + ": reason " + deniedCredentialExternalEventArgs.Reason);
        //    }
        //    #endregion //Denied


        #region External
        public event EventHandler<AccessControlExternalPropertyEventArgs> AccessControlExternalPropertyEvent;

        public void AccessControlExternalEvent(PACSService pacsService,
            string propertyOperation,
            string accessController,
            string credentialToken,
            string credentialHolderName,
            string reason,
            Decision decision,
            Requester requester)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<AccessControlExternalPropertyEventArgs> temp = AccessControlExternalPropertyEvent;
            if (temp != null)
            {
                temp(this,
                    new AccessControlExternalPropertyEventArgs(pacsService, DateTime.UtcNow,
                        null, accessController, decision, requester)
                    {
                        CredentialToken = credentialToken,
                        CredentialHolderName = credentialHolderName,
                        Reason = reason,
                    });
            }
        }

        public void AccessControlExternalEventHandler(object src, AccessControlExternalPropertyEventArgs externalEventArgs)
        {

            string access = externalEventArgs.Decision == Decision.Granted
                                ? "AccessGranted"
                                : "Denied";
            string requester = externalEventArgs.Requester == Requester.Anonymous
                                   ? "Anonymous"
                                   : "Credential";
            NotificationMessageHolderType notificationMessageHolderType =
                GenerateNotificationMessage(string.Format("tns1:AccessControl/{0}/{1}", access, requester), externalEventArgs);
            Topic topic = null;
            if (externalEventArgs.Requester == Requester.Anonymous)
            {
                topic = externalEventArgs.Decision == Decision.Granted
                            ? PACSTopicSet.Instance.AccessGrantedAnonymous
                            : PACSTopicSet.Instance.DeniedAnonymous;
            }
            else
            {
                topic = externalEventArgs.Decision == Decision.Granted
                            ? PACSTopicSet.Instance.AccessGrantedCredential
                            : PACSTopicSet.Instance.DeniedCredential;
            }
            NotifyAll(notificationMessageHolderType, topic);
            _isExternalResponseDone = true;
            //System.Diagnostics.Debug.WriteLine(credentialExternalEventArgs.AccessPointToken + ": reason " + deniedCredentialExternalEventArgs.Reason);
        }
        #endregion

        #region RequestTimout

        public event EventHandler<RequestTimeoutEventArgs> RequestTimeoutPropertyEvent;

        public void TimeoutEvent(EventControlService eventService, Topic topic, List<SimpleItem> source, List<SimpleItem> data)
        {
            if (topic.ParentTopic != null && topic.ParentTopic.Name == "Request")
            {
                if (topic.ParentTopic.ParentTopic != null && topic.ParentTopic.ParentTopic.Name == "AccessControl")
                {
                    var accessPointItem = source.FirstOrDefault(s => s.Name == "AccessPointToken");
                    string accessPoint = null;
                    if (accessPointItem != null)
                        accessPoint = accessPointItem.Value;
                    string credentialToken = null;
                    var credentialTokenItem = data.FirstOrDefault(s => s.Name == "CredentialToken");
                    if (credentialTokenItem != null)
                        credentialToken = credentialTokenItem.Value;

                    Requester requester = Requester.Credential;
                    if (string.IsNullOrEmpty(credentialToken))
                        requester = Requester.Anonymous;
                    RequestTimeoutEvent(eventService, "Timeout", accessPoint, credentialToken, requester);
                }
            }
        }

        public void RequestTimeoutEvent(EventControlService eventControlService,
            string propertyOperation,
            string accessController,
            string credentialToken,
            Requester requester)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<RequestTimeoutEventArgs> temp = RequestTimeoutPropertyEvent;
            if (temp != null)
            {
                temp(this,
                    new RequestTimeoutEventArgs(eventControlService, DateTime.UtcNow,
                        propertyOperation, accessController, requester)
                    {
                        CredentialToken = credentialToken,
                    });
            }
        }

        public void RequestTimeoutEventHandler(object src,
            RequestTimeoutEventArgs requestTimeoutEventArgs)
        {
            Timer timer = new Timer(RequestTimeoutEventHandler_OnTimout,
                requestTimeoutEventArgs, TIMEOUT, -1);
        }

        public void RequestTimeoutEventHandler_OnTimout(object evenArgs)
        {
            if (!_isExternalResponseDone)
            {
                RequestTimeoutEventArgs requestTimeoutEventArgs = (RequestTimeoutEventArgs)evenArgs;
                string requester = requestTimeoutEventArgs.Requester == Requester.Anonymous
                                    ? "Anonymous"
                                    : "Credential";
                NotificationMessageHolderType notificationMessageHolderType =
                    GenerateNotificationMessage(string.Format("tns1:AccessControl/Request/Timeout/{0}", requester),
                                                    requestTimeoutEventArgs);
                Topic topic = requestTimeoutEventArgs.Requester == Requester.Anonymous
                                  ? PACSTopicSet.Instance.RequestTimeoutAnonymous
                                  : PACSTopicSet.Instance.RequestTimeoutCredential;
                NotifyAll(notificationMessageHolderType, topic);
            }
        }
        #endregion

        #endregion //AccessControl

        #region Credential

        #region Credential Enable

        /// <summary>
        /// Declare an event of delegate type EventHandler of TamperingEvent
        /// </summary>
        public event EventHandler<CredentiaEnabledPropertyEventArgs> CredentialEnabledPropertyEvent;

        public void CredentialEnabledEvent(CredentialService credentialService,
            string propertyOperation,
            string credentialToken,
            bool active,
            string reason)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<CredentiaEnabledPropertyEventArgs> temp = CredentialEnabledPropertyEvent;
            if (temp != null)
            {
                temp(this, new CredentiaEnabledPropertyEventArgs(credentialService, DateTime.UtcNow, propertyOperation, credentialToken) { Enabled = active, Reason = reason });
            }
        }

        /// <summary>
        /// Add Enabled Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="enabledPropertyEventArgs"> Event arguments </param>
        public void CredentialEnabledPropertyEventHandler(object src,
            EnabledPropertyEventArgs enabledPropertyEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateNotificationMessage("tns1:Credential/State/Enabled",
                                                enabledPropertyEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.AccessControlEnabled);

            System.Diagnostics.Debug.WriteLine(enabledPropertyEventArgs.AccessPointToken + ": state " + enabledPropertyEventArgs.Enabled.ToString());
        }

        #endregion


        #endregion

        #region AccessRules

        #region CreateAccessProfile

        /// <summary>
        /// Declare an event of delegate type EventHandler of ConfigurationAccessProfileChangedArgs
        /// </summary>
        public event EventHandler<ConfigurationAccessProfileChangedEventArgs> ConfigurationAccessProfileChangedPropertyEvent;

        /// <summary>
        /// Catch a Door Tamper Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Monitor State</param>
        public void ConfigurationAccessProfileChangedEvent(AccessRulesService accessRulesService,
            string accessProfileToken)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<ConfigurationAccessProfileChangedEventArgs> temp = ConfigurationAccessProfileChangedPropertyEvent;
            if (temp != null)
            {
                temp(this, new ConfigurationAccessProfileChangedEventArgs(accessRulesService, DateTime.UtcNow, accessProfileToken));
            }
        }

        /// <summary>
        /// Add Door Monitor Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="doorAlarmPropertyEventArgs">Event arguments</param>
        public void ConfigurationAccessProfileChangedEventHandler(object src,
            ConfigurationAccessProfileChangedEventArgs configurationAccessProfileChangedEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateNotificationMessage("tns1:Configuration/AccessProfile/Changed",
                                                configurationAccessProfileChangedEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.ConfigurationAccessProfileChanged);

            System.Diagnostics.Debug.WriteLine(configurationAccessProfileChangedEventArgs.AccessProfileToken + " changed or created");
        }


        #endregion

        #region CreateCredential

        /// <summary>
        /// Declare an event of delegate type EventHandler of ConfigurationCredentialChangedArgs
        /// </summary>
        public event EventHandler<ConfigurationCredentialChangedEventArgs> ConfigurationCredentialChangedPropertyEvent;

        /// <summary>
        /// Catch a Door Tamper Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Monitor State</param>
        public void ConfigurationCredentialChangedEvent(CredentialService credentialService,
            string credentialToken)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<ConfigurationCredentialChangedEventArgs> temp = ConfigurationCredentialChangedPropertyEvent;
            if (temp != null)
            {
                temp(this, new ConfigurationCredentialChangedEventArgs(credentialService, DateTime.UtcNow, credentialToken));
            }
        }

        /// <summary>
        /// Add Door Monitor Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="doorAlarmPropertyEventArgs">Event arguments</param>
        public void ConfigurationCredentialChangedEventHandler(object src,
            ConfigurationCredentialChangedEventArgs configurationCredentialChangedEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateNotificationMessage("tns1:Configuration/Credential/Changed",
                                                configurationCredentialChangedEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.ConfigurationCredentialChanged);

            System.Diagnostics.Debug.WriteLine(configurationCredentialChangedEventArgs.CredentialToken + " changed or created");
        }


        #endregion

        #region DeleteCredential

        /// <summary>
        /// Declare an event of delegate type EventHandler of ConfigurationCredentialRemovedArgs
        /// </summary>
        public event EventHandler<ConfigurationCredentialRemovedEventArgs> ConfigurationCredentialRemovedPropertyEvent;

        /// <summary>
        /// Catch a Door Tamper Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Monitor State</param>
        public void ConfigurationCredentialRemovedEvent(CredentialService credentialService,
            string credentialToken)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<ConfigurationCredentialRemovedEventArgs> temp = ConfigurationCredentialRemovedPropertyEvent;
            if (temp != null)
            {
                temp(this, new ConfigurationCredentialRemovedEventArgs(credentialService, DateTime.UtcNow, credentialToken));
            }
        }

        /// <summary>
        /// Add Door Monitor Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="doorAlarmPropertyEventArgs">Event arguments</param>
        public void ConfigurationCredentialRemovedEventHandler(object src,
            ConfigurationCredentialRemovedEventArgs configurationCredentialRemovedEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateNotificationMessage("tns1:Configuration/Credential/Removed",
                                                configurationCredentialRemovedEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.ConfigurationCredentialRemoved);

            System.Diagnostics.Debug.WriteLine(configurationCredentialRemovedEventArgs.CredentialToken + " enabled");
        }

        #endregion

        #region DisableCredential & EnableCredential

        /// <summary>
        /// Declare an event of delegate type EventHandler of ConfigurationCredentialRemovedArgs
        /// </summary>
        public event EventHandler<ConfigurationCredentialStateChangeEventArgs> ConfigurationCredentialEnabledDisabledPropertyEvent;

        /// <summary>
        /// Catch a Door Tamper Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Monitor State</param>
        public void ConfigurationCredentialEnableDisableEvent(CredentialService credentialService,
            string credentialToken)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<ConfigurationCredentialStateChangeEventArgs> temp = ConfigurationCredentialEnabledDisabledPropertyEvent;
            if (temp != null)
            {
                temp(this, new ConfigurationCredentialStateChangeEventArgs(credentialService, DateTime.UtcNow, credentialToken));
            }
        }

        /// <summary>
        /// Add Door Monitor Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="doorAlarmPropertyEventArgs">Event arguments</param>
        public void ConfigurationCredentialEnabledDisabledEventHandler(object src,
            ConfigurationCredentialStateChangeEventArgs configurationCredentialRemovedEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateNotificationMessage("tns1:Configuration/CredentialState/Changed",
                                                configurationCredentialRemovedEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.ConfigurationCredentialStateChanged);

            System.Diagnostics.Debug.WriteLine(configurationCredentialRemovedEventArgs.CredentialToken + " enabled");
        }

        #endregion

        #region Antipassback

        /// <summary>
        /// Declare an event of delegate type EventHandler of ConfigurationCredentialRemovedArgs
        /// </summary>
        public event EventHandler<ConfigurationCredentialStateChangeEventArgs> evConfigurationCredentialAntipassbackEvent;

        /// <summary>
        /// Catch a Door Tamper Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Monitor State</param>
        public void ConfigurationCredentialAntipassbackEvent(CredentialService credentialService,
            string credentialToken)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<ConfigurationCredentialStateChangeEventArgs> temp = evConfigurationCredentialAntipassbackEvent;
            if (temp != null)
            {
                temp(this, new ConfigurationCredentialStateChangeEventArgs(credentialService, DateTime.UtcNow, credentialToken));
            }
        }

        /// <summary>
        /// Add Door Monitor Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="doorAlarmPropertyEventArgs">Event arguments</param>
        public void ConfigurationCredentialAntipassbackEventHandler(object src,
            ConfigurationCredentialStateChangeEventArgs configurationCredentialAntipassbackEventArgs)  
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateNotificationMessage("tns1:Configuration/CredentialState/Changed",
                                                configurationCredentialAntipassbackEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.ConfigurationCredentialChanged);

            System.Diagnostics.Debug.WriteLine(configurationCredentialAntipassbackEventArgs.CredentialToken + " Antipassback done");
        }

        #endregion

        #region Credential Identifier

        /// <summary>
        /// Declare an event of delegate type EventHandler of ConfigurationCredentialRemovedArgs
        /// </summary>
        public event EventHandler<ConfigurationCredentialStateChangeEventArgs> evConfigurationCredentialCredentialIdentifierEvent;

        /// <summary>
        /// Catch a Door Tamper Event
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Monitor State</param>
        public void ConfigurationCredentialIdentifierEvent(CredentialService credentialService,
            string credentialToken)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<ConfigurationCredentialStateChangeEventArgs> temp = evConfigurationCredentialCredentialIdentifierEvent;
            if (temp != null)
            {
                temp(this, new ConfigurationCredentialStateChangeEventArgs(credentialService, DateTime.UtcNow, credentialToken));
            }
        }

        /// <summary>
        /// Add Door Monitor Events to corresponding subscribtions
        /// </summary>
        /// <param name="src">Source of event</param>
        /// <param name="doorAlarmPropertyEventArgs">Event arguments</param>
        public void ConfigurationCredentialCredentialIdentifierEventHandler(object src,
            ConfigurationCredentialStateChangeEventArgs configurationCredentialCredentialIdentifierEventArgs)
        {

            NotificationMessageHolderType notificationMessageHolderType =
                GenerateNotificationMessage("tns1:Configuration/Credential/Changed",
                                                configurationCredentialCredentialIdentifierEventArgs);

            NotifyAll(notificationMessageHolderType, PACSTopicSet.Instance.ConfigurationCredentialChanged);

            System.Diagnostics.Debug.WriteLine(configurationCredentialCredentialIdentifierEventArgs.CredentialToken + " Credential Identifier set");
        }

        #endregion
        #endregion


        #endregion //EventHandlers

        public void TransmitMessage(NotificationMessageHolderType notificationMessageHolder, Topic topic)
        {
            NotifyAll(notificationMessageHolder, topic);
        }
    }





}
