using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Event =TestTool.Proxies.Event;
using TestTool.Tests.TestCases.TestSuites;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.Common.CommonUtils;

namespace TestTool.Tests.TestCases.Utils
{
    public static class EventServiceUtils
    {
        public static void ValidateSubscription(DateTime? terminationTime, DateTime currentTime,
            int requestedTerminationTime, Event.EndpointReferenceType subscription,
            AssertDelegate assert)
        {
            assert(terminationTime.HasValue, "TerminationTime is not specified",
                "Check that TerminationTime is specified", null);

            bool intervalIsWrong = false;

            if (currentTime < DateTime.Parse("1970-01-01T00:00:00"))
            {
                intervalIsWrong = true;
            }

            if (currentTime > DateTime.Parse("2070-01-01T00:00:00"))
            {
                intervalIsWrong = true;
            }

            if (terminationTime.Value < DateTime.Parse("1970-01-01T00:00:00"))
            {
                intervalIsWrong = true;
            }

            if (terminationTime.Value > DateTime.Parse("2070-01-01T00:00:00"))
            {
                intervalIsWrong = true;
            }

            assert(!intervalIsWrong, "TerminationTime or CurrentTime is out of reasonable interval (less than 1970-01-01T00:00:00 or greater than 2070-01-01T00:00:00).",
                "Check that TerminationTime and CurrentTime has reasonable values", null);

            if (requestedTerminationTime >= 0)
            {
                assert(currentTime.AddSeconds(requestedTerminationTime) <= terminationTime.Value,
                    "TerminationTime < CurrentTime + InitialTerminationTime",
                    "Validate CurrentTime and TerminationTime", null);
            }
            else
            {
                assert(currentTime < terminationTime.Value,
                    "TerminationTime <= CurrentTime",
                    "Validate CurrentTime and TerminationTime", null);
            }

            assert(subscription != null, "The DUT did not return SubscriptionReference",
                   "Check if the DUT returned SubscriptionReference", null);

            assert(subscription.Address != null && subscription.Address.Value != null,
                "SubscriptionReference does not contain address",
                   "Check if SubscriptionReference contains address", null);

            assert(subscription.Address.Value.IsValidUrl(), "URL passed in SubscriptionReference is not valid",
                   "Check that URL specified is valid", null);

        }

        public static string GetSoapPacket(string response)
        {
            string rawSoapPacket = null;

            System.IO.StringReader rdr = new StringReader(response);

            string nextLine;
            do
            {
                nextLine = rdr.ReadLine();
            } while (!string.IsNullOrEmpty(nextLine));

            rawSoapPacket = rdr.ReadToEnd();
            // fix for #976
            rawSoapPacket = BaseNotificationXmlUtils.RemoveInvalidXmlChars(rawSoapPacket);




            return rawSoapPacket;
        }


        /// <summary>
        /// Gets "raw" notification messages.
        /// </summary>
        /// <param name="notificationMessages"></param>
        /// <param name="soapRawPacket"></param>
        /// <param name="manager"></param>
        /// <param name="notification"></param>
        /// <returns></returns>
        public static Dictionary<Event.NotificationMessageHolderType, XmlElement>
            GetRawElements(
            Event.NotificationMessageHolderType[] notificationMessages,
            XmlDocument soapRawPacket,
            XmlNamespaceManager manager,
            bool notification)
        {
            Dictionary<Event.NotificationMessageHolderType, XmlElement> rawElements = new Dictionary<Event.NotificationMessageHolderType, XmlElement>();

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

            foreach (Event.NotificationMessageHolderType message in notificationMessages)
            {
                rawElements.Add(message, (XmlElement)responseNodeList[cnt]);
                cnt++;
            }

            return rawElements;
        }

        /// <summary>
        /// Create namespace manager to work with TopicSet information.
        /// </summary>
        /// <param name="soapRawPacket"></param>
        /// <returns></returns>
        public static XmlNamespaceManager CreateNamespaceManager(XmlDocument soapRawPacket)
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(soapRawPacket.NameTable);
            manager.AddNamespace("s", "http://www.w3.org/2003/05/soap-envelope");
            manager.AddNamespace("events", "http://www.onvif.org/ver10/events/wsdl");
            manager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");

            return manager;
        }


        /// <summary>
        /// Checks that a string passed is valid xs:datetime string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidXsdDateTime(string value)
        {
            try
            {
                string onvif = "http://www.onvif.org/ver10/schema";
                string message = string.Format("<tt:Message UtcTime=\"{0}\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" ><//tt:Message>", value);
                string schemaString = "<xs:schema xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" elementFormDefault=\"qualified\"><xs:element name=\"Message\"><xs:complexType><xs:attribute name=\"UtcTime\" type=\"xs:dateTime\" use=\"required\"/></xs:complexType></xs:element></xs:schema>";

                XmlSchemaSet schemaSet = new XmlSchemaSet();
                XmlReader rdr = XmlReader.Create(new System.IO.StringReader(schemaString));
                schemaSet.Add(onvif, rdr);

                XmlReader reader = XmlReader.Create(new System.IO.StringReader(message));
                XmlNamespaceManager manager = new XmlNamespaceManager(reader.NameTable);

                XmlSchemaValidator validator = new XmlSchemaValidator(reader.NameTable, schemaSet, manager, XmlSchemaValidationFlags.None);
                validator.Initialize();

                XmlSchemaInfo schemaInfo = new XmlSchemaInfo();

                validator.ValidateElement("Message", onvif, schemaInfo);

                validator.ValidateAttribute("UtcTime", "", value, schemaInfo);

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        /// <summary>
        /// Validates a message against Message type defined in onvif.xsd
        /// </summary>
        /// <param name="message">Message XML element</param>
        /// <param name="filter"></param>
        /// <param name="reason">Error description, if any.</param>
        /// <returns>True, if message is valid; false otherwise</returns>
        public static bool IsValidMessageElement(XmlElement message,
            bool isProperty,
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

            if (!EventServiceUtils.IsValidXsdDateTime(utcTimeValue))
            {
                reason = string.Format("'{0}' is not valid xs:datetime value", utcTimeValue);
                return false;
            }

            // if topic is Property topic
            if (isProperty)
            {
                if (message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                {
                    XmlAttribute propertyOperationType = message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];

                    bool match = false;
                    foreach (string allowedPropertyOperation in new string[] { OnvifMessage.INITIALIZED, OnvifMessage.CHANGED, OnvifMessage.DELETED })
                    {
                        if (propertyOperationType.Value == allowedPropertyOperation)
                        {
                            match = true;
                            break;
                        }
                    }

                    if (!match)
                    {
                        reason = string.Format("PropertyOperation attribute has unexpected value: {0}",
                                               propertyOperationType.Value);
                        return false;
                    }
                }
                else
                {
                    reason = "PropertyOperation attribute not found";
                    return false;
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


        public static TopicInfo ExtractTopicInfo(Event.NotificationMessageHolderType notification, 
            XmlElement messageRawElement, XmlNamespaceManager manager, out string err)
        {
            if (notification.Topic == null)
            {
                err = "Topic is null";
                return null;
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

            TopicInfo actualTopic = TopicInfo.ExtractTopicInfoAll(topic, topicNode);

            TopicInfo currentTopic = actualTopic;
            while (currentTopic != null)
            {
                if (currentTopic.ParentTopic == null && string.IsNullOrEmpty(currentTopic.NamespacePrefix))
                {
                    err = string.Format("Topic {0} is incorrect: root topic must have namespace defined", topic);
                    return null;
                }
                if (string.IsNullOrEmpty(currentTopic.Namespace))
                {
                    err = string.Format("Topic {0} is incorrect: namespace prefix {1} not defined", topic, currentTopic.NamespacePrefix);
                    return null;
                }
                currentTopic = currentTopic.ParentTopic;
            }

            err = string.Empty;
            return actualTopic;
        }

    }
}
