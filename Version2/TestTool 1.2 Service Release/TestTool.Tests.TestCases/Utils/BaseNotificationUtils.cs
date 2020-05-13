///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Xml;

namespace TestTool.Tests.TestCases
{
    /// <summary>
    /// Extension methods to simplify creation on XML-elements
    /// </summary>
    static class BaseNotificationUtils
    {
        /// <summary>
        /// Creates topic element for Filter (with Dialect attribute).
        /// </summary>
        /// <param name="doc">Document to be used.</param>
        /// <returns>XML Element for topic filter information</returns>
        public static XmlElement CreateTopicElement(this XmlDocument doc)
        {
            XmlElement topicElement = doc.CreateElement(BaseNotification.WSNTPREFIX, BaseNotification.TOPICEXPRESSION, BaseNotification.WSNT);

            XmlAttribute dialectAttribute = doc.CreateAttribute(BaseNotification.DIALECT);
            dialectAttribute.Value = BaseNotification.CONCRETESETDIALECT;

            topicElement.Attributes.Append(dialectAttribute);
            return topicElement;
        }

        /// <summary>
        /// Creates message content filter element for filter (with Dialect attribute).
        /// </summary>
        /// <param name="doc">Document to be used.</param>
        /// <returns>XML element for message content filter information.</returns>
        public static XmlElement CreateContentFilterElement(this XmlDocument doc)
        {
            XmlElement contentFilterElement =
                doc.CreateElement(BaseNotification.WSNTPREFIX, BaseNotification.MESSAGECONTENT, BaseNotification.WSNT);

            XmlAttribute itemFilterDialectAttribute = doc.CreateAttribute(BaseNotification.DIALECT);
            itemFilterDialectAttribute.Value = BaseNotification.ITEMFILTERDIALECT;
            contentFilterElement.Attributes.Append(itemFilterDialectAttribute);

            return contentFilterElement;
        }

        /// <summary>
        /// Checks if XmlElement represents a topic in TopicSet
        /// </summary>
        /// <param name="element">Element to be checked</param>
        /// <returns>True, if element represents a topic; false otherwise</returns>
        public static bool RepresentsTopic(this XmlElement element)
        {
            if (element.HasAttribute(BaseNotification.TOPIC, BaseNotification.T1))
            {
                XmlAttribute topicAttribute = element.Attributes[BaseNotification.TOPIC, BaseNotification.T1];

                if (string.Compare(topicAttribute.Value, "true", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds MEssageDescription element within Topic element child nodes.
        /// </summary>
        /// <param name="element">Topic element</param>
        /// <returns>MEssageDescription elemtn or null if not found </returns>
        public static XmlElement GetMessageDescription(this XmlElement element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                XmlElement child = node as XmlElement;
                if (child == null)
                {
                    continue;
                    ;
                }
                if (child.LocalName == OnvifMessage.MESSAGEDESCRIPTION && child.NamespaceURI == OnvifMessage.ONVIF)
                {
                    return child;
                }
            }
            return null;
        }

    }
}
