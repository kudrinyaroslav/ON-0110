using System;
using System.Collections.Generic;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Event;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Common.CommonUtils;
using System.Xml;
using System.ServiceModel.Channels;
using TestTool.HttpTransport;
using System.ServiceModel;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.GUI.Utils
{
    class EventsServiceProvider : BaseServiceProvider<EventPortTypeClient, EventPortType>
    {

        public EventsServiceProvider(string serviceAddress, int messageTimeout) : 
            base(serviceAddress, messageTimeout)
        {
            EnableLogResponse = false;
        }

        public override EventPortTypeClient CreateClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress address)
        {
            EndpointController controller = new EndpointController(new EndpointAddress(_serviceAddress));
            controller.WsaEnabled = true;
            Binding binding1 = new HttpBinding(new IChannelController[] { this, controller, _credentialsProvider });
            
            return new EventPortTypeClient(binding1, address);
        }

        public List<EventsTopicInfo> GetTopics()
        {
            TestTool.Proxies.Event.TopicSetType topicSet = GetTopicSet();

            if (topicSet == null || topicSet.Any == null || topicSet.Any.Length == 0)
            {
                return null;
            }

            List<XmlElement> topics = new List<XmlElement>();
            foreach (XmlElement element in topicSet.Any)
            {
                FindTopics(element, topics);
            }

            List<EventsTopicInfo> topicInfos = new List<EventsTopicInfo>();
            foreach (XmlElement nextTopicElement in topics)
            {
                TopicInfo info = TopicInfo.ConstructTopicInfo(nextTopicElement);
                EventsTopicInfo nextTopicInfo = info.GetPlainInfo();
                topicInfos.Add(nextTopicInfo);
            }

            return topicInfos;
        }

        /// <summary>
        /// Gets TopicSet
        /// </summary>
        /// <returns></returns>
        protected TopicSetType GetTopicSet()
        {
            string[] response = null;

            bool fixedTopicSet = false;
            TopicSetType topicSet = null;
            string[] topicExpressionDialect = null;
            string[] messageContentFilterDialect = null;
            string[] producerPropertiesFilterDialect = null;
            string[] messageContentSchemaLocation = null;
            XmlElement[] any = null;

            if (Security == Security.None)
            {
                Action action = ConstructSecurityTolerantAction(
                        () =>
                        {
                            // query properties
                            response = Client.GetEventProperties(out fixedTopicSet,
                                                                 out topicSet,
                                                                 out topicExpressionDialect,
                                                                 out messageContentFilterDialect,
                                                                 out producerPropertiesFilterDialect,
                                                                 out messageContentSchemaLocation,
                                                                 out any);

                        }
                        );

                action();

            }
            else
            {
                response = Client.GetEventProperties(out fixedTopicSet,
                                                     out topicSet,
                                                     out topicExpressionDialect,
                                                     out messageContentFilterDialect,
                                                     out producerPropertiesFilterDialect,
                                                     out messageContentSchemaLocation,
                                                     out any);
            }

            return topicSet;
        }

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
                }
                FindTopics(child, topics);
            }
        }
    }
}
