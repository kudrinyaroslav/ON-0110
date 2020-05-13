using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TestTool.Proxies.Onvif;

namespace TestTool.Tests.Common.CommonUtils
{
    public static class Extensions
    {

        /// <summary>
        /// Gets namespace prefix. If namespace is not defined, adds definition. 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static string GetNamespacePrefix(this XmlElement element, string ns)
        {
            return GetNamespacePrefix(element, ns, string.Empty);
        }

        /// <summary>
        /// Gets namespace prefix. If namespace is not defined, adds definition. If definition should be
        /// added, an attepmt to use prefix passed is made.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="ns"></param>
        /// <param name="defaultPrefix"></param>
        /// <returns></returns>
        /// <remarks>Namespace definition is added like normal attribute.</remarks>
        public static string GetNamespacePrefix(this XmlElement element, string ns, string defaultPrefix)
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
        /// Finds service bt namespace.
        /// </summary>
        /// <param name="services">List of all services</param>
        /// <param name="ns">Namespace</param>
        /// <returns>Service entry for newest service with the namespace specified</returns>
        public static Service FindService(this IEnumerable<Service> services, string ns)
        {
            if (services == null)
            {
                return null;
            }
            else
            {
                return services.
                OrderByDescending(s => s.Version == null ? 0 : s.Version.Major).
                ThenByDescending(s => s.Version == null ? 0 : s.Version.Minor).
                FirstOrDefault(s => s.Namespace == ns);
            }
        }
    }
}
