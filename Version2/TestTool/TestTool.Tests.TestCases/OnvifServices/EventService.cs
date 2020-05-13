using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Event;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.TestSuites.Events;
using TestTool.Tests.TestCases.Utils;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;
using DateTime = System.DateTime;
using EndpointReferenceType = TestTool.Proxies.Event.EndpointReferenceType;
using FilterType = TestTool.Proxies.Event.FilterType;
using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;

namespace TestTool.Tests.TestCases.OnvifServices
{
    public interface IEventService: IBaseOnvifService2<EventPortType, EventPortTypeClient>
    {}

    public static class EventServiceExtensions
    {
        public const string PTNAMESPACE = "http://www.onvif.org/ver10/pacs";
        public const string XSNAMESPACE = "http://www.w3.org/2001/XMLSchema";

        #region Initialization utils

        private static void InitializeGuard(this IEventService s)
        {
            if (null == s.ServiceClient.Port)
                s.Test.Assert(false,
                              "Can't connect to Event Service",
                              "Check that Event Service is accessible");
        }

        public static ServiceHolder<EventPortTypeClient, EventPortType> DefaultInitializer(this IEventService s)
        {
            var deviceService = s as IDeviceService;

            if (null == deviceService)
                s.Test.Assert(false, "The test doesn't implement Device Service", "Implementation issue");

            var client = new ServiceHolder<EventPortTypeClient, EventPortType>(features => deviceService.GetEventServiceAddress(),
                                                                               (binding, address) => new EventPortTypeClient(binding, address),
                                                                               "Event");
            if (null == client.Client)
                client.InitServiceClient(new IChannelController[] { new SoapValidator(EventsSchemasSet.GetInstance()) }, s.Test);

            return client;
        }
        #endregion

        public static EventServiceCapabilities GetServiceCapabilities(this IEventService s)
        {
            s.InitializeGuard();

            EventServiceCapabilities r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetServiceCapabilities(), "Get Service Capabilities(Event)");

            return r;
        }

        public static EndpointReferenceType CreatePullPointSubscription(this IEventService s, FilterType Filter, string InitialTerminationTime, CreatePullPointSubscriptionSubscriptionPolicy SubscriptionPolicy, ref System.Xml.XmlElement[] Any, out System.DateTime CurrentTime, out DateTime? TerminationTime)
        {
            s.InitializeGuard();

            EndpointReferenceType r = null;
            XmlElement[] localAny = Any;
            System.DateTime localCurrentTime = System.DateTime.MinValue;
            System.DateTime? localTerminationTime = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.CreatePullPointSubscription(Filter, InitialTerminationTime, SubscriptionPolicy, ref localAny, out localCurrentTime, out localTerminationTime), "Get Service Capabilities(Event)");
            CurrentTime = localCurrentTime;
            TerminationTime = localTerminationTime;

            return r;
        }

        public static string[] GetEventProperties(this IEventService s, out bool FixedTopicSet, out TopicSetType TopicSet, out string[] TopicExpressionDialect, out string[] MessageContentFilterDialect, out string[] ProducerPropertiesFilterDialect, out string[] MessageContentSchemaLocation, out System.Xml.XmlElement[] Any)
        {
            s.InitializeGuard();

            string[] response = null;

            bool fixedTopicSetCopy = false;
            TopicSetType topicSetCopy = null;
            string[] topicExpressionDialectCopy = null;
            string[] messageContentFilterDialectCopy = null;
            string[] producerPropertiesFilterDialectCopy = null;
            string[] messageContentSchemaLocationCopy = null;
            XmlElement[] anyCopy = null;

            s.Test.RunStep(() =>
                           {
                               response = s.ServiceClient.Port.GetEventProperties(out fixedTopicSetCopy,
                                                                    out topicSetCopy,
                                                                    out topicExpressionDialectCopy,
                                                                    out messageContentFilterDialectCopy,
                                                                    out producerPropertiesFilterDialectCopy,
                                                                    out messageContentSchemaLocationCopy,
                                                                    out anyCopy);
                           },
                           "Get Event Properties");

            FixedTopicSet = fixedTopicSetCopy;
            TopicSet = topicSetCopy;
            TopicExpressionDialect = topicExpressionDialectCopy;
            MessageContentFilterDialect = messageContentFilterDialectCopy;
            ProducerPropertiesFilterDialect = producerPropertiesFilterDialectCopy;
            MessageContentSchemaLocation = messageContentSchemaLocationCopy;
            Any = anyCopy;

            return response;
        }

        public static TopicSetType GetTopicSet(this IEventService s)
        {
            s.InitializeGuard();

            bool fixedTopicSet = false;
            TopicSetType topicSet = null;
            string[] topicExpressionDialect = null;
            string[] messageContentFilterDialect = null;
            string[] producerPropertiesFilterDialect = null;
            string[] messageContentSchemaLocation = null;
            XmlElement[] any = null;

            // query properties
            s.GetEventProperties(out fixedTopicSet,
                                 out topicSet,
                                 out topicExpressionDialect,
                                 out messageContentFilterDialect,
                                 out producerPropertiesFilterDialect,
                                 out messageContentSchemaLocation,
                                 out any);

            return topicSet;
        }
        
        //public static string GetEventServiceAddress(this IEventService s)
        //{
        //    s.InitializeGuard();

        //    string address = string.Empty;
            
        //    s.Test.RunStep(() =>
        //                   {
        //                      var binding = s.Test.CreateBinding(true, new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });
                               
        //                      var device = new DeviceClient(binding, new EndpointAddress(s.Test.CameraAddress));
        //                      s.Test.AttachSecurity(device.Endpoint);

        //                      address = device.GetEventServiceAddress(s.Test.Features);

        //                      device.Close();

        //                      if (string.IsNullOrEmpty(address))
        //                          throw new DutPropertiesException("Event service not found");

        //                      Uri uri;
        //                      if (!Uri.TryCreate(address, UriKind.Absolute, out uri))
        //                          throw new AssertException(string.Format("Event service address [{0}] is invalid", address));
        //                   },
        //                   "Get Event service address");
        //    s.Test.DoRequestDelay();

        //    return address;
        //}

        #region Filter utils
        public static FilterType CreateSubscriptionFilter(TopicInfo topicInfo)
        {
            return CreateSubscriptionFilter(new TopicInfo[] { topicInfo });
        }

        public static  FilterType CreateSubscriptionFilter(IEnumerable<TopicInfo> topicInfos)
        {
            FilterType filter = new Proxies.Event.FilterType();

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

        public static TopicInfo GetTopicInfo(NotificationMessageHolderType msg, XmlElement rawMsg)
        {
            XmlNamespaceManager manager = EventsMainHelper.CreateNamespaceManager(rawMsg.OwnerDocument);

            XmlText text = null;
            if (msg.Topic.Any != null)
            {
                foreach (XmlNode any in msg.Topic.Any)
                {
                    var current = any as XmlText;
                    if (any != null)
                    {
                        text = current;
                        break;
                    }
                }
            }

            XmlNode topicNode = rawMsg.SelectSingleNode("b2:Topic", manager);

            string topic = text != null ? text.Value : "";

            return TopicInfo.ExtractTopicInfoPACS(topic, topicNode);
        }

        public static TopicInfo GetTopicInfo(NotificationMessageHolderType msg)
        {
            var rawMsg = msg.Topic.Any.First().OwnerDocument;
            XmlNamespaceManager manager = EventsMainHelper.CreateNamespaceManager(rawMsg);

            XmlText text = null;
            if (msg.Topic.Any != null)
            {
                foreach (XmlNode any in msg.Topic.Any)
                {
                    var current = any as XmlText;
                    if (any != null)
                    {
                        text = current;
                        break;
                    }
                }
            }

            string topic = text != null ? text.Value : "";

            return TopicInfo.ExtractTopicInfoPACS(topic, msg.Topic);
        }

        public static bool NotificationTopicMatch(NotificationMessageHolderType msg, XmlElement rawMsg, TopicInfo topic)
        {
            return TopicInfo.TopicsMatch(GetTopicInfo(msg, rawMsg), topic);
        }

        public static bool NotificationTopicMatch(NotificationMessageHolderType msg, TopicInfo topic)
        {
            return TopicInfo.TopicsMatch(GetTopicInfo(msg), topic);
        }
        #endregion
    }
}
