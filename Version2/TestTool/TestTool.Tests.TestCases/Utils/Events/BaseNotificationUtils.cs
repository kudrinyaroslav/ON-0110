///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Xml;
using TestTool.Proxies.Event;
using TestTool.Tests.Common.CommonUtils;
using System.Collections.Generic;

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
            return element.GetChildElement(OnvifMessage.MESSAGEDESCRIPTION, OnvifMessage.ONVIF);
        }

        /// <summary>
        /// Find "Message" element in "NotificationHolder" element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static XmlElement GetMessageContentElement(this XmlElement element)
        {
            XmlElement message = element.GetChildElement("Message", BaseNotification.WSNT);
            return message != null ? message.GetChildElement("Message", OnvifMessage.ONVIF) : null;
        }

        /// <summary>
        /// Finds "Source" element in Message
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static XmlElement GetMessageSource(this XmlElement element)
        {
            return element.GetChildElement(OnvifMessage.SOURCE, OnvifMessage.ONVIF);
        }

        /// <summary>
        /// Finds "Data" element in Message
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static XmlElement GetMessageData(this XmlElement element)
        {
            return element.GetChildElement(OnvifMessage.DATA, OnvifMessage.ONVIF);
        }

        /// <summary>
        /// Finds child element with specified name and namespace
        /// </summary>
        /// <param name="element"></param>
        /// <param name="localName"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        static XmlElement GetChildElement(this XmlElement element, string localName, string ns)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                XmlElement child = node as XmlElement;
                if (child == null)
                {
                    continue;
                }
                if (child.LocalName == localName && child.NamespaceURI == ns)
                {
                    return child;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets simple items from Source element.
        /// </summary>
        /// <param name="message">"Source" element</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetMessageSourceSimpleItems(this XmlElement message, out bool success, out string error)
        {
            XmlElement sourceElement = message.GetMessageSource();
            if (sourceElement != null)
            {
                return GetMessageElementSimpleItems(sourceElement, out success, out error);
            }
            else
            {
                success = false;
                error = "Source element is missing";
                return null;
            }
        }

        /// <summary>
        /// Gets simple items from "Data" element
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetMessageDataSimpleItems(this XmlElement message, out bool success, out string error)
        {
            XmlElement dataElement = message.GetMessageData();
            if (dataElement != null)
            {
                return GetMessageElementSimpleItems(dataElement, out success, out error);
            }
            else
            {
                success = false;
                error = "Data element is missing";
                return null;
            }
        }

        public static Dictionary<string, string> GetMessageElementSimpleItems(this XmlElement message, out bool success, out string error)
        {
            error = string.Empty;
            success = true;

            Dictionary<string, string> items = new Dictionary<string, string>();
            foreach (XmlNode node in message.ChildNodes)
            {
                XmlElement element = node as XmlElement;
                if (element == null)
                {
                    continue;
                }

                if (element.LocalName == "SimpleItem" && element.NamespaceURI == OnvifMessage.ONVIF)
                {
                    string name = element.GetAttribute(OnvifMessage.NAME);
                    if (string.IsNullOrEmpty(name))
                    {
                        success = false;
                        error = "'Name' attribute is missing";
                        return null;
                    }
                    if (items.ContainsKey(name))
                    {
                        success = false;
                        error = string.Format("SimpleItem name '{0}' is not unique", name);
                        return null;
                    }
                    else
                    {
                        items.Add(name, element.GetAttribute(OnvifMessage.VALUE));
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <remarks>No check that message names are unique. Use only after validation</remarks>
        public static Dictionary<string, string> GetMessageElementSimpleItems(this XmlElement message)
        {
            Dictionary<string, string> items = new Dictionary<string, string>();

            foreach (XmlNode node in message.ChildNodes)
            {
                XmlElement element = node as XmlElement;
                if (element == null)
                {
                    continue;
                }

                if (element.LocalName == "SimpleItem" && element.NamespaceURI == OnvifMessage.ONVIF)
                {
                    items.Add(element.GetAttribute(OnvifMessage.NAME), element.GetAttribute(OnvifMessage.VALUE));
                }
            }

            return items;
        }

        /// <summary>
        /// Gets simple items from Source element.
        /// </summary>
        /// <param name="message">"Source" element</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetMessageSourceSimpleItems(this XmlElement message)
        {
            return GetMessageSimpleItems(OnvifMessage.SOURCE, message);
        }

        /// <summary>
        /// Gets simple items from "Data" element
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetMessageDataSimpleItems(this XmlElement message)
        {
            return GetMessageSimpleItems(OnvifMessage.DATA, message);
        }

        /// <summary>
        /// Gets simple items from specified message part
        /// </summary>
        /// <param name="part">Part name</param>
        /// <param name="message">Part element</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetMessageSimpleItems(string part, XmlElement message)
        {
            Dictionary<string, string> items = new Dictionary<string, string>();

            XmlNamespaceManager manager = new XmlNamespaceManager(message.OwnerDocument.NameTable);

            string prefix = manager.LookupNamespace(OnvifMessage.ONVIF);
            if (string.IsNullOrEmpty(prefix))
            {
                prefix = OnvifMessage.ONVIFPREFIX;
                manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);
            }

            string path;
            path = string.Format("/{0}:{1}/{0}:SimpleItem", prefix, part);
            XmlNodeList itemNodesList = message.SelectNodes(path, manager);

            foreach (XmlNode node in itemNodesList)
            {
                XmlElement element = node as XmlElement;
                if (element == null)
                {
                    continue;
                }
                // handle wrong elements!
                items.Add(element.GetAttribute(OnvifMessage.NAME), element.GetAttribute(OnvifMessage.VALUE));
            }

            return items;
        }

        public static List<XmlElement> GetMessageElementItems(string part, XmlElement message)
        {
            List<XmlElement> elementItems = new List<XmlElement>();

            XmlNamespaceManager manager = new XmlNamespaceManager(message.OwnerDocument.NameTable);

            string prefix = manager.LookupNamespace(OnvifMessage.ONVIF);
            if (string.IsNullOrEmpty(prefix))
            {
                prefix = OnvifMessage.ONVIFPREFIX;
                manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);
            }

            string path;
            path = string.Format("/{0}:{1}/{0}:ElementItem", prefix, part);
            XmlNodeList itemNodesList = message.SelectNodes(path, manager);

            foreach (XmlNode node in itemNodesList)
            {
                XmlElement element = node as XmlElement;
                if (element == null)
                {
                    continue;
                }
                if (element.NamespaceURI != OnvifMessage.ONVIF || element.LocalName != "SimpleItem")
                {
                    elementItems.Add(element);
                }
            }


            return elementItems;
        }
        public static Dictionary<string, XmlElement> GetMessageSourceSimpleItemDescriptions(this XmlElement message)
        {
            return GetMessageSimpleItemDescriptions(OnvifMessage.SOURCE, message);
        }

        public static Dictionary<string, XmlElement> GetMessageDataSimpleItemDescriptions(this XmlElement message)
        {
            return GetMessageSimpleItemDescriptions(OnvifMessage.DATA, message);
        }


        public static Dictionary<string, XmlElement> GetMessageSourceSimpleItemDescriptions(this XmlElement message, out bool success, out string err)
        {
            return GetMessageSimpleItemDescriptions(OnvifMessage.SOURCE, message, out success, out err);
        }

        public static Dictionary<string, XmlElement> GetMessageDataSimpleItemDescriptions(this XmlElement message, out bool success, out string err)
        {
            return GetMessageSimpleItemDescriptions(OnvifMessage.DATA, message, out success, out err);
        }


        /// <summary>
        /// Gest simple item description from message description
        /// </summary>
        /// <param name="part"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        static Dictionary<string, XmlElement> GetMessageSimpleItemDescriptions(string part, XmlElement message)
        {
            Dictionary<string, XmlElement> items = new Dictionary<string, XmlElement>();

            XmlNamespaceManager manager = new XmlNamespaceManager(message.OwnerDocument.NameTable);
            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            string path;
            path = string.Format("tt:{0}/tt:SimpleItemDescription", part);
            XmlNodeList itemNodesList = message.SelectNodes(path, manager);
            
            foreach (XmlNode node in itemNodesList)
            {
                XmlElement element = node as XmlElement;
                if (element == null)
                {
                    continue;
                }
                items.Add(element.GetAttribute(OnvifMessage.NAME), element);
            }

            return items;
        }
        
        static Dictionary<string, XmlElement> GetMessageSimpleItemDescriptions(string part, XmlElement message, out bool success, out string err)
        {
            success = true;
            err = null;
            Dictionary<string, XmlElement> items = new Dictionary<string, XmlElement>();

            XmlNamespaceManager manager = new XmlNamespaceManager(message.OwnerDocument.NameTable);
            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            string path;
            path = string.Format("tt:{0}/tt:SimpleItemDescription", part);
            XmlNodeList itemNodesList = message.SelectNodes(path, manager);

            foreach (XmlNode node in itemNodesList)
            {
                XmlElement element = node as XmlElement;
                if (element == null)
                {
                    continue;
                }
                string name = element.GetAttribute(OnvifMessage.NAME);
                if (string.IsNullOrEmpty(name))
                {
                    success = false;
                    err = "'Name' attribute is missing";
                    return null;
                }
                if (items.ContainsKey(name))
                {
                    success = false;
                    err = string.Format("SimpleItem name '{0}' is not unique", name);
                    return null;
                }
                else
                {
                    items.Add(name, element);
                }
            }

            return items;
        }
        /// <summary>
        /// Validates QName
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="expectedLocal"></param>
        /// <param name="expectedNamespace"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsCorrectQName(this XmlAttribute attr, 
                                          string expectedLocal, 
                                          string expectedNamespace, 
                                          XmlElement element,
                                          ref string error)
        { return IsCorrectQName(attr, expectedLocal, expectedNamespace, element.GetNamespaceOfPrefix, ref error); }

        public static bool IsCorrectQName(this XmlAttribute attr,
                                          string expectedLocal,
                                          string expectedNamespace,
                                          IXmlNamespaceResolver namespaceResolver, 
                                          ref string error)
        { return IsCorrectQName(attr, expectedLocal, expectedNamespace, namespaceResolver.LookupNamespace, ref error); }

        internal static bool IsCorrectQName(this XmlAttribute attr,
                                            string expectedLocal,
                                            string expectedNamespace,
                                            Func<string, string> namespaceResolver,
                                            ref string error)
        {
            bool ok = true;

            string name = attr.Value;

            string[] parts = attr.Value.Split(':');
            if (parts.Length == 2)
            {
                string ns = namespaceResolver(parts[0]);
                if (string.IsNullOrEmpty(ns))
                {
                    ok = false;
                    error = string.Format("prefix '{0}' is not defined", parts[0]);
                }
                else
                {
                    if (ns != expectedNamespace || parts[1] != expectedLocal)
                    {
                        error = string.Format("expected '{0}' from namespace '{1}', actual '{2}' from namespace '{3}'", expectedLocal, expectedNamespace, parts[1], ns);
                        ok = false;
                    }
                }
            }
            else
            {
                error = string.Format("'{0}' is not a correct Qualified Name", name);
                ok = false;
            }
            return ok;
        }

        public static XmlDocument GetRawResponse(string response)
        {
            return BaseNotificationXmlUtils.GetRawResponse(response);
        }
    }
}
