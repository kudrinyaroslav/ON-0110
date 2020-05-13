using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SMC.Events
{
    public static class Utils
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
        /// Adds namespace prefix to an element, if not present currently.
        /// </summary>
        /// <param name="element">Element where namespace declaration should be added</param>
        /// <param name="ns">Namespace</param>
        /// <param name="defaultPrefix">Desired prefix. If cannot be used, other prefix will be generated and returned.</param>
        /// <returns>Namespace prefix</returns>
        public static string AddNamespacePrefix(this XmlElement element, string ns, string defaultPrefix)
        {
            // if already exists
            string prefix = element.GetPrefixOfNamespace(ns);
            if (!string.IsNullOrEmpty(prefix))
            {
                return prefix;
            }

            // if OK to add as default
            if (!string.IsNullOrEmpty(defaultPrefix))
            {
                if (!element.HasAttribute(string.Format("xmlns:{0}", prefix)))
                {
                    XmlAttribute localNamespaceAttribute = element.OwnerDocument.CreateAttribute(string.Format("xmlns:{0}", defaultPrefix));
                    localNamespaceAttribute.Value = ns;
                    element.Attributes.Append(localNamespaceAttribute);
                    return defaultPrefix;
                }
            }

            int i = 1;
            while (true)
            {
                prefix = string.Format("tns{0}", i);
                if (!element.HasAttribute(string.Format("xmlns:{0}", prefix)))
                {
                    break;
                }
                i++;
            }

            XmlAttribute namespaceAttribute = element.OwnerDocument.CreateAttribute(string.Format("xmlns:{0}", prefix));
            namespaceAttribute.Value = ns;
            element.Attributes.Append(namespaceAttribute);

            return prefix;
        }
    }
}
