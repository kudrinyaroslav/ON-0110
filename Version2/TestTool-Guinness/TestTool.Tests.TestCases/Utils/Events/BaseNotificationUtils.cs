///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Xml;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Proxies.Event;

namespace TestTool.Tests.TestCases.Utils.Events
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
