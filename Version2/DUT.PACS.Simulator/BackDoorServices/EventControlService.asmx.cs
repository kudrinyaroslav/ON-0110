using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using DUT.PACS.Simulator.Events;
using DUT.PACS.Simulator.Events10;
using System.Xml;
using DUT.PACS.Simulator.Common;

namespace DUT.PACS.Simulator.BackDoorServices
{
    /// <summary>
    /// Summary description for EventControlService
    /// </summary>
    [WebService(Namespace = "http://www.onvif.org/simulator/events")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class EventControlService : BaseDutService
    {
        /// <summary>
        /// Send topics information in convenient form. 
        /// </summary>
        /// <returns>Plain set of topics as list of TopicInformation.</returns>
        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(Action = "http://www.onvif.org/simulator/events/GetTopics", RequestNamespace = "http://www.onvif.org/simulator/events", ResponseNamespace = "http://www.onvif.org/simulator/events", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public List<TopicInformation> GetTopics()
        {
            List<TopicInformation> topics = new List<TopicInformation>();

            foreach (Topic topic in PACSTopicSet.Instance.Topics)
            {
                AddTopics(topics, topic);
            }

            return topics;
        }

        /// <summary>
        /// Fires event.
        /// </summary>
        /// <param name="topicInfo">Topic information</param>
        /// <param name="messageTime">Message time. If DateTime.MinValue is sent, current time is used.</param>
        /// <param name="source">Source simple items.</param>
        /// <param name="data">Data simple items.</param>
        [WebMethod]
        public void FireEvent(TopicInformation topicInfo, DateTime messageTime, string propertyOperation, List<SimpleItem> source, List<SimpleItem> data)
        {
            // find topic

            DUT.PACS.Simulator.Events.Topic topic = TopicsConverter.Find(
                DUT.PACS.Simulator.Events.PACSTopicSet.Instance, topicInfo);

            // create NotificationMessage

            NotificationMessageHolderType message = GenerateNotificationMessage(topicInfo, messageTime, propertyOperation, source, data);

            EventServer.TransmitMessage(message, topic);

            EventServer.TimeoutEvent(this, topic, source, data);
        }

        /// <summary>
        /// Creates notification message.
        /// </summary>
        /// <param name="topicInfo">Topic information.</param>
        /// <param name="dateTime">Message time.</param>
        /// <param name="sourceItems">Source simple items.</param>
        /// <param name="dataItems">Data simple items.</param>
        /// <returns>Norification message for sending.</returns>
        private NotificationMessageHolderType GenerateNotificationMessage(TopicInformation topicInfo,
            DateTime dateTime,
            string propertyOperation,
            List<SimpleItem> sourceItems,
            List<SimpleItem> dataItems)
        {
            NotificationMessageHolderType res = new NotificationMessageHolderType();

            NameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            nsmgr.AddNamespace("tt", "http://www.onvif.org/ver10/schema");
            nsmgr.AddNamespace("wsnt", "http://docs.oasis-open.org/wsn/b-2");

            XmlDocument xmlDocument = new XmlDocument(nt);

            res.Topic = new TopicExpressionType();
            res.Topic.Dialect = "http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";
            res.Topic.Any = new XmlNode[1];
            res.Topic.Any[0] = xmlDocument.CreateTextNode(topicInfo.TopicString);
            res.Topic.Xmlns = new System.Xml.Serialization.XmlSerializerNamespaces();

            foreach (NamespaceDescription description in topicInfo.Namespaces)
            {
                res.Topic.Xmlns.Add(description.Prefix, description.Namespace);
            }

            //Message
            res.Message = xmlDocument.CreateElement("tt", "Message", "http://www.onvif.org/ver10/schema");

            DateTime time = System.DateTime.UtcNow;
            if (dateTime != DateTime.MinValue)
            {
                time = dateTime;
            }

            if (propertyOperation != null)
            {
                XmlAttribute operation = xmlDocument.CreateAttribute("PropertyOperation");
                operation.Value = propertyOperation;
                res.Message.Attributes.Append(operation);
            }

            //Message[UtcTime]
            XmlAttribute utcTime = xmlDocument.CreateAttribute("UtcTime");
            utcTime.Value = System.Xml.XmlConvert.ToString(time, XmlDateTimeSerializationMode.Utc);
            res.Message.Attributes.Append(utcTime);


            //Message/Source
            XmlElement source = xmlDocument.CreateElement("tt", "Source", "http://www.onvif.org/ver10/schema");
            res.Message.AppendChild(source);

            foreach (SimpleItem item in sourceItems)
            {

                //Message/Source/SimpleItem
                XmlElement simpleItem = xmlDocument.CreateElement("tt", "SimpleItem", "http://www.onvif.org/ver10/schema");
                source.AppendChild(simpleItem);

                //Message/Source/SimpleItem[Name]
                XmlAttribute name = xmlDocument.CreateAttribute("Name");
                name.Value = item.Name;
                simpleItem.Attributes.Append(name);

                //Message/Source/SimpleItem[Value]
                XmlAttribute value = xmlDocument.CreateAttribute("Value");
                value.Value = item.Value;
                simpleItem.Attributes.Append(value);
            }

            //Message/Data
            XmlElement data = xmlDocument.CreateElement("tt", "Data", "http://www.onvif.org/ver10/schema");
            res.Message.AppendChild(data);

            foreach (SimpleItem item in dataItems)
            {
                //Message/Data/SimpleItem
                XmlElement simpleItem = xmlDocument.CreateElement("tt", "SimpleItem", "http://www.onvif.org/ver10/schema");
                data.AppendChild(simpleItem);

                //Message/Data/SimpleItem[Name]
                XmlAttribute name = xmlDocument.CreateAttribute("Name");
                name.Value = item.Name;
                simpleItem.Attributes.Append(name);

                //Message/Data/SimpleItem[Value]
                XmlAttribute value = xmlDocument.CreateAttribute("Value");
                value.Value = item.Value;
                simpleItem.Attributes.Append(value);
            }

            return res;
        }


        /// <summary>
        /// Adds child topic to list.
        /// </summary>
        /// <param name="topics">Plain list of topics.</param>
        /// <param name="topic">Topic with child topics.</param>
        private void AddTopics(List<TopicInformation> topics, Topic topic)
        {
            TopicInformation info = new TopicInformation();

            List<Topic> line = new List<Topic>();
            Topic current = topic;
            do
            {
                line.Add(current);
                current = current.ParentTopic;
            } while (current != null);

            string topicsString = string.Empty;
            string lastNs = string.Empty;
            for (int i = line.Count - 1; i >= 0; i--)
            {
                current = line[i];

                if (!string.IsNullOrEmpty(current.Namespace))
                {
                    topicsString += string.Format("{0}:{1}", current.Prefix, current.Name);
                    info.Namespaces.Add(new NamespaceDescription() { Namespace = current.Namespace, Prefix = current.Prefix });
                }
                else
                {
                    topicsString += (current.Name);
                }

                if (i == 1)
                {
                    info.ParentTopicString = topicsString;
                }
                if (i > 0)
                {
                    topicsString += "/";
                }
                lastNs = current.Namespace;
            }

            info.TopicString = topicsString;
            if (topic.MessageDescription != null)
            {
                info.SourceItems = topic.MessageDescription.SourceItems;
                info.DataItems = topic.MessageDescription.DataItems;
                info.IsProperty = topic.IsTopic && topic.MessageDescription.IsProperty;
            }
            info.IsTopic = topic.IsTopic;
            topics.Add(info);

            foreach (Topic child in topic.SubTopics)
            {
                AddTopics(topics, child);
            }

        }
    }
}
