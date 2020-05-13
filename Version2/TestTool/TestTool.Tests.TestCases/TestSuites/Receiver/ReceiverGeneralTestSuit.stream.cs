using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.TestCases.Base;
using System.Xml;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Proxies.Event;
using System.IO;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.TestCases.Utils.SoapValidation;
using TestTool.Tests.CommonUtils.SoapValidation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using TestTool.Tests.TestCases.Utils;
using System.Reflection;
using System.Threading;
using Event = TestTool.Proxies.Event;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Common.Trace;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class ReceiverEventsGeneralTestSuit : NotificationsTestSuite
    {
        private const string PATH_GENERAL = "Receiver\\General";

        private const string msgNoNotification = "Expected notification has not been received within predefined timeout \"Operation Delay\".";
        private readonly string msgNoNotificationHeaderFormat = msgNoNotification
                                                                + Environment.NewLine
                                                                + "The details of the expected notification are the following: {0}";

        private readonly string msgBeforePullMessagesHeaderFormat = "Send PullMessages requests until an event with {0} is received" + Environment.NewLine;

        public ReceiverEventsGeneralTestSuit(TestLaunchParam param) : base(param)
        {}

        #region tests for receiving stream

        private const string ONVIFTOPICSET = "http://www.onvif.org/ver10/topics";

        private const string RECEIVERTOKENSIMPLEITEM = "ReceiverToken";
        private const string RECEIVERSTATESIMPLEITEM = "NewState";

        RTSPSimulator _simulator = null;

        [Test(Name = "CONFIGURE RECEIVER - (RTP-Unicast/UDP)",
            Order = "02.01.16",
            Id = "2-1-16",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 2.1,
            //RequirementLevel = RequirementLevel.Optional,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.ConfigureReceiver, Functionality.ReceiverChangeStateEvent })]
        public void ConfigureReceiverRtpUnicastUdpTest()
        {
            ConfigureReceiverTest(StreamType.RTPUnicast, TransportProtocol.UDP);
        }

        [Test(Name = "CONFIGURE RECEIVER – (RTP/RTSP/TCP)",
            Order = "02.01.17",
            Id = "2-1-17",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 2.1,
            //RequirementLevel = RequirementLevel.Optional,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.ConfigureReceiver, Functionality.ReceiverChangeStateEvent })]
        public void ConfigureReceiverRtpRtspTcpUdpTest()
        {
            ConfigureReceiverTest(StreamType.RTPUnicast, TransportProtocol.RTSP);
        }

        [Test(Name = "CONFIGURE RECEIVER – INVALID MEDIA URI",
            Order = "02.01.18",
            Id = "2-1-18",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.ConfigureReceiver, Functionality.ReceiverConnectionFailedEvent })]
        public void ConfigureReceiverInvalidUriTest()
        {
            Receiver receiver = null;
            ReceiverConfiguration configuration = null;
            string token = string.Empty;

            Proxies.Event.EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = 60;
            
            RunTest(
                () => 
                    {
                        //3.	ONVIF Client will retrieve complete receivers list from the DUT and 
                        // create a new Receiver if the list is empty (see Annex A.2).
                        ReceiverConfiguration config = new ReceiverConfiguration();
                        config.Mode = ReceiverMode.NeverConnect;
                        //config.MediaUri = "http://localhost/Valid/URI";

                        string ip = _nic.IP.ToString();
                        if (_nic.IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        {
                            int idx = ip.IndexOf('%');
                            if (idx > 0)
                            {
                                ip = ip.Substring(0, idx);
                            }
                            ip = string.Format("[{0}]", ip);
                        }

                        config.MediaUri = string.Format("http://{0}:1234/Valid/URI",ip);
                        config.StreamSetup = new StreamSetup();
                        config.StreamSetup.Transport = new Transport();
                        config.StreamSetup.Stream = StreamType.RTPUnicast;
                        config.StreamSetup.Transport.Protocol = TransportProtocol.UDP;

                        Receiver[] receivers = GetReceivers();

                        if (receivers == null || receivers.Length == 0)
                        {
                            //create new one
                            receiver = CreateReceiver(config);
                            token = receiver.Token;
                            // 3.	ONVIF Client will retrieve complete receivers list from the DUT and create a new Receiver if the list is empty (see Annex A.2).
                            // ToDo: check that receiver has been created properly
                            CheckReceiverConfiguration(receiver, config);
                        }
                        else
                        {
                            receiver = receivers[0];
                            token = receiver.Token;
                            configuration = receiver.Configuration;
                        }


                        TopicInfo topicInfo = new TopicInfo();
                        topicInfo.ParentTopic = new TopicInfo();
                        topicInfo.ParentTopic.NamespacePrefix = "tns1";
                        topicInfo.ParentTopic.Name = "Receiver";
                        topicInfo.ParentTopic.Namespace = ONVIFTOPICSET;
                        topicInfo.Name = "ConnectionFailed";
                        topicInfo.Namespace = ONVIFTOPICSET;

                        // create filter from topic
                        Proxies.Event.FilterType filter = null;
                        {
                            filter = new Proxies.Event.FilterType();

                            XmlDocument filterDoc = new XmlDocument();
                            XmlElement filterTopicElement = filterDoc.CreateTopicElement();

                            string topicPath = TopicInfo.CreateTopicPath(filterTopicElement, topicInfo);

                            filterTopicElement.InnerText = topicPath;

                            filter.Any = new XmlElement[] { filterTopicElement };
                        }
                        
                        //4.	ONVIF Client will invoke SubscribeRequest message with tns1:Receiver/ConnectionFailed 
                        // Topic as Filter and an InitialTerminationTime=Time1 to check Receiver state.
                        //5.	Verify that the DUT sends a SubscribeResponse message.
                        //6.	ONVIF Client will invoke ConfigureReceiverRequest message (ReceiverToken as 
                        // Token of the first Receiver in the GetReceiversResponse message, 
                        // Configuration.Mode=”AlwaysConnect”, Configuration.MediaUri as invalid stream_uri of 
                        // RTSP Simulator, Configuration.StreamSetup.Stream=”RTP-Unicast”, 
                        // StreamSetup.Transport.Tunnel.Protocol=“UDP”, no StreamSetup.Transport.Tunnel.Tunnel) 
                        // to configure the receiver to receive media from RTSP Simulator.                        
                        //7.	Verify ConfigureReceiverResponse message from the DUT.
                        //8.	Verify that DUT sends Notify message(s) with TopicExpression=tns1:Receiver/ConnectionFailed.
                        //9.	Verify that there is Notify message with Source.SimpleItem  
                        // item with Name="ReceiverToken” and Value is equal to “ReceiverToken1”.

                        {
                            subscriptionReference = CreateSubscription(filter, 60);
                            subscribeStarted = System.DateTime.Now;

                            config.Mode = ReceiverMode.AlwaysConnect;
                            config.MediaUri = string.Format("http://{0}:1234/NoRtspHere", ip);
                            ConfigureReceiver(receiver.Token, config);
                            System.DateTime T1 = System.DateTime.UtcNow;

                            var eventDetails = string.Format("'ReceiverToken' Simple Item with value='{0}'", token);
                            LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                            Func<Event.NotificationMessageHolderType, bool> eventCheck = (m) => { return CheckConnectionFailedMessage(m, token); };

                            var notifications = GetMessages(subscriptionReference, T1, eventCheck);

                            // Check that notifications have been received
                            Assert(notifications.Any(),
                                   string.Format(msgNoNotificationHeaderFormat, eventDetails),
                                   "Check that the message with requested source has been received so far");


                            Proxies.Event.NotificationMessageHolderType message = notifications.Keys.First();

                            // initialize "raw" elements
                            XmlNamespaceManager manager = EventServiceUtils.CreateNamespaceManager(notifications[message].OwnerDocument);

                            // validate messages
                            ValidateReceiverConnectionFailedMessages(notifications, topicInfo, manager, receiver.Token, ReceiverMode.AlwaysConnect);
                        }
                                                
                        //10.	ONVIF Client will invoke GetReceiverRequest message with 
                        // ReceiverToken=ReceiverToken1.
                        //11.	Verify GetReceiverResponse message from the DUT. Check that GetReceiverResponse 
                        // message contains the same parameters values as were changed in ConfigureReceiverRequest 
                        // message.
                        Receiver actualReceiver = GetReceiver(token);
                        CheckReceiverConfiguration(actualReceiver, config);

                        //12.	ONVIF Client will invoke GetReceiverStateRequest message to check Receiver State.
                        //13.	Verify GetReceiverStateResponse message from the DUT. Check that 
                        // ReceiverState.State=” NotConnected”.

                        ReceiverStateInformation state = GetReceiverState(token);
                        Assert(state.State == ReceiverState.NotConnected,
                            string.Format("The state is {0}", state.State),
                            "Check that Receiver state is set to 'NotConnected'");
                    }, 
                    () =>
                    {
                        if (subscribeStarted != System.DateTime.MaxValue)
                        {
                            if (subscriptionReference != null)
                            {
                                ReleaseSubscription(subscriptionReference.Address.Value, subscribeStarted, timeout);
                            }
                        }

                        if (configuration != null)
                        {
                            ConfigureReceiver(token, configuration);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(token))
                            {
                                DeleteReceiver(token);
                            }
                        }
                    }
                );

        }

        void ConfigureReceiverTest(StreamType type, TransportProtocol protocol)
        {
            Receiver receiver = null;
            ReceiverConfiguration configuration = null;
            string token = string.Empty;

            Proxies.Event.EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = 60;

            RunTest(() =>
            {
                _simulator = new RTSPSimulator(_nic.IP.ToString(), Sleep, _messageTimeout);

                string streamName = AddStream();
                Assert(_simulator.StartRTSP(), "Unable to start RTSP simulator!", "Starting RTSP simulator...");

                ReceiverConfiguration config = new ReceiverConfiguration();
                config.Mode = ReceiverMode.NeverConnect;
                //config.MediaUri = "http://localhost/Valid/URI";

                config.MediaUri = _simulator.GetUrl(RTSPSimulator.Codecs.JPEG, streamName);
                config.StreamSetup = new StreamSetup();
                config.StreamSetup.Transport = new Transport();
                config.StreamSetup.Stream = type;
                config.StreamSetup.Transport.Protocol = TransportProtocol.UDP;               

                Receiver[] receivers = GetReceivers();

                if (receivers == null || receivers.Length == 0)
                {
                    //create new one
                    receiver = CreateReceiver(config);
                    token = receiver.Token;
                    // 3.	ONVIF Client will retrieve complete receivers list from the DUT and create a new Receiver if the list is empty (see Annex A.2).
                    CheckReceiverConfiguration(receiver, config);
                }
                else
                {
                    receiver = receivers[0];
                    configuration = receiver.Configuration;
                    token = receiver.Token;
                }
                
                // tns1:Receiver/ChangeState 
                TopicInfo topicInfo = new TopicInfo();
                topicInfo.ParentTopic = new TopicInfo();
                topicInfo.ParentTopic.NamespacePrefix = "tns1";
                topicInfo.ParentTopic.Name = "Receiver";
                topicInfo.ParentTopic.Namespace = ONVIFTOPICSET;
                topicInfo.Name = "ChangeState";
                topicInfo.Namespace = ONVIFTOPICSET;

                // create filter from topic
                Proxies.Event.FilterType filter = null;
                {
                    filter = new Proxies.Event.FilterType();

                    XmlDocument filterDoc = new XmlDocument();
                    XmlElement filterTopicElement = filterDoc.CreateTopicElement();

                    string topicPath = TopicInfo.CreateTopicPath(filterTopicElement, topicInfo);

                    filterTopicElement.InnerText = topicPath;

                    filter.Any = new XmlElement[] { filterTopicElement };
                }

                // Get and validate notifications
                {
                    subscriptionReference = CreateSubscription(filter, 60);
                    subscribeStarted = System.DateTime.Now;

                    config.Mode = ReceiverMode.AlwaysConnect;
                    config.StreamSetup.Transport.Protocol = protocol;                

                    ConfigureReceiver(receiver.Token, config);
                    System.DateTime T1 = System.DateTime.UtcNow;

                    var eventDetails = string.Format("'ReceiverToken' Simple Item with value='{0}' and 'NewState' Simple Item with value='{1}'", token, "Connected");
                    LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                    Func<Event.NotificationMessageHolderType, bool> eventCheck = (m) => { return CheckReceiverChangedMessage(m, token, ReceiverState.Connected ); };

                    var notifications = GetMessages(subscriptionReference, T1, eventCheck);

                    // Check that notifications have been received
                    Assert(notifications.Any(),
                           string.Format(msgNoNotificationHeaderFormat, eventDetails),
                           "Check that the message with requested source has been received so far");


                    Proxies.Event.NotificationMessageHolderType message = notifications.Keys.First();

                    XmlNamespaceManager manager = EventServiceUtils.CreateNamespaceManager(notifications[message].OwnerDocument);

                    // validate messages

                    ValidateReceiverStateChangeMessages(notifications, topicInfo, manager, receiver.Token, ReceiverMode.AlwaysConnect);
                }

                {
                    //19.	ONVIF Client will invoke GetReceiverRequest message with ReceiverToken=ReceiverToken1.
                    //20.	Verify GetReceiverResponse message from the DUT. Check that GetReceiverResponse message contains the same parameters values as were changed in ConfigureReceiverRequest message.
                    //21.	ONVIF Client will invoke GetReceiverStateRequest message to check Receiver State.
                    //22.	Verify GetReceiverStateResponse message from the DUT. Check that ReceiverState.State=”Connected”.


                    // Get Receiver
                    Receiver actualReceiver = GetReceiver(token);
                    CheckReceiverConfiguration(actualReceiver, config);
                    // Get Receiver State

                    ReceiverStateInformation state = GetReceiverState(token);
                    Assert(state.State == ReceiverState.Connected,
                           string.Format("The state is {0}", state.State),                        
                           "Check that Receiver state is set to 'Connected'");

                }

                {
                    SetReceiverMode(receiver.Token, ReceiverMode.NeverConnect);
                    
                    System.DateTime T1 = System.DateTime.UtcNow;

                    var eventDetails = string.Format("'ReceiverToken' Simple Item with value='{0}' and 'NewState' Simple Item with value='{1}'", token, "NotConnected");
                    LogTestEvent(string.Format(msgBeforePullMessagesHeaderFormat, eventDetails));

                    Func<Event.NotificationMessageHolderType, bool> eventCheck = (m) => { return CheckReceiverChangedMessage(m, token, ReceiverState.NotConnected); };

                    var notifications = GetMessages(subscriptionReference, T1, eventCheck);

                    // Check that notifications have been received
                    Assert(notifications.Any(),
                           string.Format(msgNoNotificationHeaderFormat, eventDetails),
                           "Check that the message with requested topic has been received so far");
                    
                    Proxies.Event.NotificationMessageHolderType message = notifications.Keys.First();

                    XmlNamespaceManager manager = EventServiceUtils.CreateNamespaceManager(notifications[message].OwnerDocument);
                    
                    // validate messages

                    ValidateReceiverStateChangeMessages(notifications, topicInfo, manager, receiver.Token, ReceiverMode.NeverConnect);
                }
                
                { 
                    //29.	ONVIF Client will invoke GetReceiverStateRequest message to check Receiver State.
                    //30.	Verify GetReceiverStateResponse message from the DUT. Check that ReceiverState.State=”NotConnected”.
                    ReceiverStateInformation state = GetReceiverState(token);
                    Assert(state.State == ReceiverState.NotConnected,
                        string.Format("The state is {0}", state.State),
                        "Check that Receiver state is set to 'NotConnected'");                
                }

            }, () =>
            {

                if (!string.IsNullOrEmpty(token))
                {
                    if (configuration != null)
                    {
                        ConfigureReceiver(token, configuration);
                    }
                    else
                    {
                        DeleteReceiver(token);
                    }
                }

                if (_simulator != null)
                {
                    _simulator.StopRTSP();
                    _simulator = null;
                }

                if (subscribeStarted != System.DateTime.MaxValue)
                {
                    if (subscriptionReference != null)
                    {
                        ReleaseSubscription(subscriptionReference.Address.Value, subscribeStarted, timeout);                     
                    }
                }

            });        
        }
        
        private void CheckReceiverConfiguration(Receiver receiver, ReceiverConfiguration config)
        {
            var receiverConfiguration = receiver.Configuration;
            bool ok = true;
            StringBuilder logger = new StringBuilder();

            bool local = CompareParameteres(receiverConfiguration.Mode, 
                config.Mode, (param1, param2) => param1 == param2, logger, "Mode");
            ok &= local;
            local = CompareParameteres(receiverConfiguration.MediaUri, 
                config.MediaUri,(param1, param2) => param1 == param2, logger, "MediaUri");
            ok &= local;
            local = CompareParameteres(receiverConfiguration.StreamSetup.Stream, 
                config.StreamSetup.Stream, (param1, param2) => param1 == param2, logger, "Stream");
            ok &= local;
            local = CompareParameteres(receiverConfiguration.StreamSetup.Transport.Protocol,
                config.StreamSetup.Transport.Protocol, (param1, param2) => param1 == param2, logger,
                "Protocol");
            ok &= local;
            Assert(ok, logger.ToStringTrimNewLine(),
                    "Check receiver configuration");
        }

        private bool CompareParameteres<T>(T param1, T param2, Func<T, T, bool> compareOp,
            StringBuilder logger, string paramName)
        {
            bool ok = compareOp(param1, param2);
            if (!ok)
            {
                logger.AppendFormat("{0} is {1} but must be {2}{3}", paramName, 
                    param1, param2, Environment.NewLine);
            }
            return ok;
        }

        bool CheckReceiverChangedMessage(Event.NotificationMessageHolderType message, string receiverToken, ReceiverState state)
        {
            Dictionary<string, string> sourceSimpleItems =
                BaseNotificationUtils.GetMessageSimpleItems(OnvifMessage.SOURCE, message.Message);

            if (!sourceSimpleItems.ContainsKey(RECEIVERTOKENSIMPLEITEM))
            {
                return false;
            }

            if (sourceSimpleItems[RECEIVERTOKENSIMPLEITEM] != receiverToken)
            {
                return false;
            }

            Dictionary<string, string> dataSimpleItems = BaseNotificationUtils.GetMessageSimpleItems(OnvifMessage.DATA, message.Message);

            if (!dataSimpleItems.ContainsKey(RECEIVERSTATESIMPLEITEM))
            {
                return false;
            }
            if (dataSimpleItems[RECEIVERSTATESIMPLEITEM] != state.ToString())
            {
                return false;
            }
            return true;
        }

        bool CheckConnectionFailedMessage(Event.NotificationMessageHolderType message, string receiverToken)
        {
            Dictionary<string, string> sourceSimpleItems =
                BaseNotificationUtils.GetMessageSimpleItems(OnvifMessage.SOURCE, message.Message);

            if (!sourceSimpleItems.ContainsKey(RECEIVERTOKENSIMPLEITEM))
            {
                return false;
            }

            if (sourceSimpleItems[RECEIVERTOKENSIMPLEITEM] != receiverToken)
            {
                return false;
            }

            return true;
        }


        #endregion

        #region Simulator

        protected override void Release()
        {
            if (_simulator != null)
            {
                StopSimulator(_simulator);
            }
            base.Release();
        }

        void StartSimulator(RTSPSimulator sim)
        {
            RunStep(() => { sim.StartRTSP(); }, "Start simulator");
        }

        void StopSimulator(RTSPSimulator sim)
        {
            RunStep(() => { sim.StopRTSP(); }, "Stop simulator");
        }

        string AddStream()
        {
            string streamName = "JPEGStream";
                _simulator.Add(RTSPSimulator.Codecs.JPEG,
                    System.IO.Path.Combine(GetCurrentDirectory(), "Streams\\Jpeg\\video_480x360_fps30-%04d.jpeg"),
                    streamName);
            return streamName;
        }

        string GetCurrentDirectory()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(location);
            return path;
        }

        #endregion

        #region Notifications

        /// <summary>
        /// Validates common elements
        /// </summary>
        /// <param name="notification">Notification</param>
        /// <param name="messageRawElement">Raw notification XML element</param>
        /// <param name="topicInfo">Topic information</param>
        /// <param name="manager">Namespaces manager</param>
        /// <param name="logger">Logger to store error description</param>
        /// <returns></returns>
        bool ValidateMessageCommonElements(Proxies.Event.NotificationMessageHolderType notification,
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

                    TopicInfo actualTopic = TopicInfo.ExtractTopicInfoAll(topic, topicNode);

                    TopicInfo currentTopic = actualTopic;
                    while (currentTopic != null)
                    {
                        if (currentTopic.ParentTopic == null && string.IsNullOrEmpty(currentTopic.NamespacePrefix))
                        {
                            dump.AppendFormat("   Topic {0} is incorrect: root topic must have namespace defined{1}", topic, Environment.NewLine);
                            ok = false;
                        }
                        if (string.IsNullOrEmpty(currentTopic.Namespace))
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

        bool ValidateReceiverMessageSource(XmlElement messageElement, string receiverToken, StringBuilder logger)
        {
            bool ok = true;
            Dictionary<string, string> sourceSimpleItems =
                BaseNotificationUtils.GetMessageSimpleItems(OnvifMessage.SOURCE, messageElement);

            if (sourceSimpleItems.ContainsKey(RECEIVERTOKENSIMPLEITEM))
            {
                string token = sourceSimpleItems[RECEIVERTOKENSIMPLEITEM];
                if (token != receiverToken)
                {
                    ok = false;
                    logger.AppendLine(string.Format("ReceiverToken SimpleItem is incorrect: expected {0}, actual {1}", receiverToken, token));
                }
            }
            else
            {
                ok = false;
                logger.AppendLine("ReceiverToken SimpleItem (in Source) not present");
            }
            return ok;
        }

        void ValidateReceiverStateChangeMessages(Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement> notifications,
                                                 TopicInfo topic,
                                                 XmlNamespaceManager manager,
                                                 string receiverToken,
                                                 ReceiverMode expectedMode)
        {
            bool ok = true;
            StringBuilder logger = new StringBuilder();

            bool found;
            foreach (Proxies.Event.NotificationMessageHolderType message in notifications.Keys)
            {
                XmlElement rawElement = notifications[message];

                // validate topic & message structure
                ok = ValidateMessageCommonElements(message, rawElement, topic, null, manager, logger);

                if (ok)
                {
                    // check state
                    // if expected - set "found" to true

                    ok = ValidateReceiverMessageSource(message.Message, receiverToken, logger);

                    Dictionary<string, string> dataSimpleItems = BaseNotificationUtils.GetMessageSimpleItems(OnvifMessage.DATA, message.Message);

                    if (dataSimpleItems.ContainsKey(RECEIVERSTATESIMPLEITEM))
                    {
                        string expectedState = expectedMode == ReceiverMode.AlwaysConnect ? "Connected" : "NotConnected";
                        string state = dataSimpleItems[RECEIVERSTATESIMPLEITEM];
                        if (state != expectedState)
                        {
                            ok = false;
                            logger.AppendLine(string.Format("NewState SimpleItem is incorrect: expected {0}, actual {1}", expectedState, state));
                        }
                    }
                    else
                    {
                        ok = false;
                        logger.AppendLine("NewState SimpleItem (in Data) not present");
                    }

                }
            }          

            Assert(ok, logger.ToStringTrimNewLine(), "Validate notifications received");
        }
        
        void ValidateReceiverConnectionFailedMessages(Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement> notifications,
            TopicInfo topic,
            XmlNamespaceManager manager,
            string receiverToken,
            ReceiverMode expectedMode)
        {
            bool ok = true;
            StringBuilder logger = new StringBuilder();

            foreach (Proxies.Event.NotificationMessageHolderType message in notifications.Keys)
            {
                XmlElement rawElement = notifications[message];

                // validate topic & message structure
                ok = ValidateMessageCommonElements(message, rawElement, topic, null, manager, logger);

                if (ok)
                {
                    ok = ValidateReceiverMessageSource(message.Message, receiverToken, logger);                    
                }
            }

            Assert(ok, logger.ToStringTrimNewLine(), "Validate notifications received");
        }


        TestTool.Proxies.Event.EndpointReferenceType CreateSubscription(Proxies.Event.FilterType filter,
            int terminationTimeSeconds)
        {
            string terminationTimeString = string.Format("PT{0}S", terminationTimeSeconds);

            XmlElement[] any = null;
            System.DateTime currentTime = System.DateTime.MinValue;
            System.DateTime? terminationTime = null;

            TestTool.Proxies.Event.EndpointReferenceType endpointReference = null;
            EnsureEventPortTypeClientCreated();

            RunStep(() =>
            {
                endpointReference = _eventPortTypeClient.CreatePullPointSubscription(
                    filter,
                    terminationTimeString,
                    null,
                    ref any,
                    out currentTime,
                    out terminationTime);
            },
            "Create Pull Point Subscription");
            
            Utils.EventServiceUtils.ValidateSubscription(terminationTime,
                currentTime,
                terminationTimeSeconds,
                endpointReference,
                Assert);

            return endpointReference;
        }

        /// <summary>
        /// Delegate definition for GetMessages.
        /// </summary>
        /// <returns></returns>
        private delegate System.DateTime PullMessageDelegate(ref System.DateTime localCurrentTime,
            ref System.DateTime localTerminationTime);

        private class PullMessagesData
        {
            public Event.NotificationMessageHolderType[] NotificationMessages;
            public string PullMessagesResponseData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="operationTime">UTC time</param>
        /// <param name="exitCheck"></param>
        /// <returns></returns>
        protected Dictionary<Event.NotificationMessageHolderType, XmlElement> GetMessages(
            Event.EndpointReferenceType subscription, System.DateTime operationTime,
            Func<Event.NotificationMessageHolderType, bool> exitCheck)
        {
            Event.PullPointSubscriptionClient pullPointClient = null;

            try
            {
                // Create new service client to pass "local" traffic listener
                IChannelController[] controllers;

                EndpointAddress address = new EndpointAddress(subscription.Address.Value);
                EndpointController controller = new EndpointController(address);
                WsaController wsaController = new WsaController();

                controllers = new IChannelController[]
                              {
                                  _trafficListener, 
                                  controller, 
                                  wsaController,
                                  _semaphore, 
                                  _credentialsProvider,
                                  new SoapValidator(EventsSchemasSet.GetInstance())
                              };
                Binding binding = CreateBinding(controllers);

                pullPointClient = new TestTool.Proxies.Event.PullPointSubscriptionClient(binding, new EndpointAddress(subscription.Address.Value));

                AttachSecurity(pullPointClient.Endpoint);
                SetupChannel(pullPointClient.InnerChannel);

                // from spec
                int messagesLimit = 1;
                string timeString = "PT20S";

                // Total list of notifications
                Dictionary<Event.NotificationMessageHolderType, XmlElement> totalMessagesList =
                    new Dictionary<TestTool.Proxies.Event.NotificationMessageHolderType, XmlElement>();


                PullMessagesData pullMessagesData = null;

                AutoResetEvent requestSentErrorEvent = new AutoResetEvent(false);

                // initialize delegate
                PullMessageDelegate del =
                    new PullMessageDelegate(
                        (ref System.DateTime localCurrentTime, ref System.DateTime localTerminationTime) =>
                        {
                            Event.NotificationMessageHolderType[] notificationMessageCopy = null;
                            System.DateTime terminationTimeCopy = System.DateTime.MinValue;
                            System.DateTime result = System.DateTime.MinValue;

                            try
                            {
                                result = pullPointClient.PullMessages(timeString,
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
                            localTerminationTime = terminationTimeCopy.ToUniversalTime();
                            pullMessagesData.NotificationMessages = notificationMessageCopy;
                            localCurrentTime = result.ToUniversalTime();

                            return localCurrentTime;
                        });

               
                // create event handler to save response
                _trafficListener.ResponseReceived += new Action<string>((data) =>
                {
                    pullMessagesData.PullMessagesResponseData = data;
                });

                System.DateTime eventLastTime = operationTime.AddSeconds(_operationDelay/1000).ToUniversalTime();

                while (true)
                {
                    pullMessagesData = new PullMessagesData();
                    System.DateTime dutCurrentTime = GetMessages(messagesLimit, pullMessagesData, del);

                    
                    BeginStep("Check that more PullMessages requests are needed");
                    if (pullMessagesData.NotificationMessages.Length == 0)
                    {
                        if (dutCurrentTime > eventLastTime)
                        {
                            LogStepEvent("Allowed interval for event generation is expired, stop getting notifications");
                            StepPassed();
                            break;
                        }
                    }
                    else
                    {
                        TestTool.Proxies.Event.NotificationMessageHolderType message = pullMessagesData.NotificationMessages[0];

                        string utcTimeValue = message.Message.Attributes[OnvifMessage.UTCTIMEATTRIBUTE].Value;
                        // xs:dateTime string

                        System.DateTime messageTime = XmlConvert.ToDateTime(utcTimeValue, XmlDateTimeSerializationMode.Utc);

                        if (messageTime > eventLastTime)
                        {
                            LogStepEvent("Last message received is out of interval of interest, stop getting messages");
                            StepPassed();
                            break;
                        }

                        string rawSoapPacket = Utils.EventServiceUtils.GetSoapPacket(pullMessagesData.PullMessagesResponseData);

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(rawSoapPacket);

                        XmlNamespaceManager manager = EventServiceUtils.CreateNamespaceManager(doc);

                        Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement> rawElements =
                            EventServiceUtils.GetRawElements(pullMessagesData.NotificationMessages,
                            doc, manager, false);


                        if (exitCheck != null)
                        {
                            bool messageFound = exitCheck(message);
                            if (messageFound)
                            {
                                LogStepEvent("Expected message found, stop getting results with PullMessages");
                                
                                foreach (TestTool.Proxies.Event.NotificationMessageHolderType m in pullMessagesData.NotificationMessages)
                                {
                                    totalMessagesList.Add(m, rawElements[m]);
                                }

                                StepPassed();
                                break;
                            }
                        }
                        else
                        {
                            foreach (TestTool.Proxies.Event.NotificationMessageHolderType m in pullMessagesData.NotificationMessages)
                            {
                                totalMessagesList.Add(m, rawElements[m]);
                            }
                        }
                    }

                    StepPassed();
                }

                return totalMessagesList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                pullPointClient.Close();
            }
        }

        private System.DateTime GetMessages(int messagesLimit,
            PullMessagesData pullMessagesData,
            PullMessageDelegate del)
        {
            // declare parameters
            System.DateTime localTerminationTime = System.DateTime.MinValue;
            System.DateTime localCurrentTime = System.DateTime.MinValue;

            //
            // Send PullMessages request
            BeginStep("PullMessages");
            System.DateTime dateTime = del.Invoke(ref localCurrentTime, ref localTerminationTime);
            localCurrentTime = dateTime;
            StepPassed();
            //                

            Assert(localCurrentTime < localTerminationTime,
                "TerminationTime <= CurrentTime",
                "Validate CurrentTime and TerminationTime", null);

            Assert(pullMessagesData.NotificationMessages.Length <= messagesLimit,
                "Maximum number of messages exceeded",
                string.Format("Check that a maximum number of {0} Notification Messages is included in PullMessagesResponse", messagesLimit),
                null);

            return localCurrentTime;
        }

        void ReleaseSubscription(string subscription, System.DateTime subscribed, int subscriptionTimeout)
        {
            int timeout = 0;

            if (subscribed != System.DateTime.MinValue)
            {
                System.DateTime now = System.DateTime.Now;
                double seconds = (now - subscribed).TotalSeconds;
                if (seconds <= subscriptionTimeout)
                {
                    // need to unsubscribe or release
                    timeout = (int)(subscriptionTimeout - seconds);

                    Binding binding = CreateEventServiceBinding(subscription);
                    SubscriptionManagerClient client = new SubscriptionManagerClient(binding, new EndpointAddress(subscription));

                    LogTestEvent("Delete Subscription Manager" + Environment.NewLine);

                    bool unsubscribeByRequest = false;
                    try
                    {
                        RunStep(() => { client.Unsubscribe(new Unsubscribe()); }, "Unsubscribe");
                        unsubscribeByRequest = true;
                    }
                    catch (FaultException exc)
                    {
                        LogFault(exc);
                        LogStepEvent("Failed to unsubscribe through request.");
                        StepPassed();
                    }
                    catch (System.Net.Sockets.SocketException exc)
                    {
                        LogStepEvent(string.Format("Failed to unsubscribe through request. Error received: {0}", exc.Message));
                        StepPassed();
                    }
                    catch (Exception exc)
                    {
                        LogStepEvent(string.Format("Failed to unsubscribe through request. Error received: {0}", exc.Message));
                        StepPassed();
                    }
                    finally
                    {
                        client.Close();
                    }

                    if (!unsubscribeByRequest)
                    {
                        RunStep(() => { Sleep(timeout*1000); }, "Wait until Subscription Manager is deleted by timeout");
                    }

                }
            }
        }


        #endregion

        #region Receiver client and methods

        private DeviceClient _deviceClient;

        protected DeviceClient DeviceClient
        {
            get
            {
                if (_deviceClient == null)
                {
                    Binding binding =
                        CreateBinding(false,
                        new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                    _deviceClient = new DeviceClient(binding, new EndpointAddress(CameraAddress));

                    AttachSecurity(_deviceClient.Endpoint);
                    SetupChannel(_deviceClient.InnerChannel);

                }

                return _deviceClient;
            }
        }

        ReceiverPortClient _receiverClient = null;

        protected ReceiverPortClient ReceiverClient
        {
            get
            {
                if (_receiverClient == null)
                {
                    BeginStep("Connect to Receiver service");
                    string serviceAddress = DeviceClient.GetReceiverServiceAddress(Features);

                    LogStepEvent(string.Format("Receiver service address: {0}", serviceAddress));
                    if (string.IsNullOrEmpty(serviceAddress))
                    {
                        throw new AssertException("Receiver service not supported");
                    }
                    else
                    {
                        if (!serviceAddress.IsValidUrl())
                        {
                            throw new AssertException("Receiver service address is invalid");
                        }
                    }

                    Binding binding = CreateBinding(false,
                        new IChannelController[] { new SoapValidator(ReceiverSchemasSet.GetInstance()) });
                    _receiverClient = new ReceiverPortClient(binding, new EndpointAddress(serviceAddress));

                    AttachSecurity(_receiverClient.Endpoint);
                    SetupChannel(_receiverClient.InnerChannel);

                    StepPassed();
                }


                return _receiverClient;
            }
        }
        
        protected Receiver[] GetReceivers()
        {
            ReceiverPortClient client = ReceiverClient;
            return CommonMethodsProvider.GetReceivers(this, client);
        }

        protected Receiver GetReceiver(string token)
        {
            ReceiverPortClient client = ReceiverClient;
            return CommonMethodsProvider.GetReceiver(this, client, token);
        }

        protected ReceiverStateInformation GetReceiverState(string token)
        {
            ReceiverPortClient client = ReceiverClient;
            return CommonMethodsProvider.GetReceiverState(this, client, token);
        }

        protected void ConfigureReceiver(string receiverToken, ReceiverConfiguration config)
        {
            ReceiverPortClient client = ReceiverClient;
            CommonMethodsProvider.ConfigureReceiver(this, client, receiverToken, config);
        }

        protected Receiver CreateReceiver(ReceiverConfiguration config)
        {
            ReceiverPortClient client = ReceiverClient;
            return CommonMethodsProvider.CreateReceiver(this, client, config);
        }

        protected void SetReceiverMode(string receiverToken, ReceiverMode mode)
        {
            ReceiverPortClient client = ReceiverClient;
            CommonMethodsProvider.SetReceiverMode(this, client, receiverToken, mode);
        }

        protected void DeleteReceiver(string receiverToken)
        {
            ReceiverPortClient client = ReceiverClient;
            CommonMethodsProvider.DeleteReceiver(this, client, receiverToken);
        }

        #endregion 
    }
}
