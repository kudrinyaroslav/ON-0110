///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.Tests.Common.CommonUtils
{

    /// <summary>
    /// Topic information
    /// </summary>
    public class TopicInfo
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Namespace 
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Namespace prefix, used in topic definition
        /// </summary>
        public string NamespacePrefix { get; set; }

        /// <summary>
        /// Parent topic
        /// </summary>
        public TopicInfo ParentTopic { get; set; }

        /// <summary>
        /// "Full" name (QName or name)
        /// </summary>
        private string QName
        {
            get { return string.IsNullOrEmpty(NamespacePrefix) ? Name : string.Format("{0}:{1}", NamespacePrefix, Name); }
        }

        /// <summary>
        /// Extracts TopicInfo from topic string and node with namespaces definitions.
        /// </summary>
        /// <param name="topicString">Topic string (like ns1:xxx/ns2:yyy/zzz )</param>
        /// <param name="topicNode">XmlElement with namespaces definitions.</param>
        /// <returns></returns>
        public static TopicInfo ExtractTopicInfo(string topicString, XmlNode topicNode)
        {
            TopicInfo currentTopicInfo = null;

            string[] topicParts = topicString.Split('/');
            for (int i = 0; i < topicParts.Length; i++)
            {
                TopicInfo lastTopic = currentTopicInfo;

                string currentTopic = topicParts[i];
                string[] currentTopicParts = currentTopic.Split(':');

                string topicName = string.Empty;
                string topicNamespace = string.Empty;
                string namespacePrefix = string.Empty;

                if (currentTopicParts.Length == 1)
                {
                    topicName = currentTopicParts[0];
                    /*if (lastTopic != null)
                    {
                        topicNamespace = lastTopic.Namespace;
                    }*/
                }
                else
                {
                    topicName = currentTopicParts[1];
                    topicNamespace = topicNode.GetNamespaceOfPrefix(currentTopicParts[0]);
                    namespacePrefix = currentTopicParts[0];
                }

                currentTopicInfo = new TopicInfo() { Name = topicName, Namespace = topicNamespace, NamespacePrefix = namespacePrefix, ParentTopic = lastTopic };
            }

            return currentTopicInfo;
        }
        
        /// <summary>
        /// Constructs TopicInfo from XmlElement (received in GetEventProperties)
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static TopicInfo ConstructTopicInfo(XmlElement element)
        {
            List<XmlElement> orderedTopics = new List<XmlElement>();
            orderedTopics.Add(element);
            XmlNode parent = element.ParentNode;

            while (parent != null && ! (parent.LocalName == "TopicSet" && parent.NamespaceURI == BaseNotification.T1 ))
            {
                orderedTopics.Add(parent as XmlElement);
                parent = parent.ParentNode;
            }

            TopicInfo current = null;
            Dictionary<string, string> prefixes = new Dictionary<string, string>();
            string lastNamespace = string.Empty;

            for (int i = orderedTopics.Count-1; i>=0; i--)
            {
                XmlElement currentElement = orderedTopics[i];

                string topicNs = /*string.IsNullOrEmpty(currentElement.NamespaceURI)
                                     ? lastNamespace
                                     : */currentElement.NamespaceURI;
                string namespacePrefix = currentElement.GetPrefixOfNamespace(currentElement.NamespaceURI);
                if (!string.IsNullOrEmpty(namespacePrefix))
                {
                    if (prefixes.ContainsKey(namespacePrefix) && topicNs != prefixes[namespacePrefix])
                    {
                        string pattern = "tns{0}";
                        int j = 1;
                        while (true)
                        {
                            string prefix = string.Format(pattern, j);
                            if (!prefixes.ContainsKey(prefix))
                            {
                                namespacePrefix = prefix;
                                break;
                            }
                            j++;
                        }
                    }
                    if (!prefixes.ContainsKey(namespacePrefix))
                    {
                        prefixes.Add(namespacePrefix, topicNs);
                    }
                }

                TopicInfo topicInfo = new TopicInfo()
                                          {
                                              Name = currentElement.LocalName,
                                              Namespace = topicNs,
                                              NamespacePrefix = namespacePrefix
                                          };

                topicInfo.ParentTopic = current;
                lastNamespace = topicNs;
                current = topicInfo;
            };
            
            return current;
        }        

        /// <summary>
        /// Constructs TopicInfo using information entered by user. 
        /// </summary>
        /// <param name="plainInfo"></param>
        /// <returns></returns>
        public static TopicInfo ConstructTopicInfo(EventsTopicInfo plainInfo)
        {
            Dictionary<string, string> namespaces = new Dictionary<string, string>();
            string[] definitions = plainInfo.NamespacesDefinition.Replace('"' + Environment.NewLine, "\" ").Split(' ');
            foreach (string definition in definitions)
            {
                if (!string.IsNullOrEmpty(definition))
                {
                    string[] parts = definition.Split('=');
                    namespaces.Add(parts[0], parts[1].Replace("\"", "").Replace(Environment.NewLine, "").Trim());
                }
            }

            TopicInfo currentTopicInfo = null;

            string[] topicParts = plainInfo.Topic.Split('/');
            for (int i = 0; i < topicParts.Length; i++)
            {
                TopicInfo lastTopic = currentTopicInfo;

                string currentTopic = topicParts[i];
                string[] currentTopicParts = currentTopic.Split(':');

                string topicName = string.Empty;
                string topicNamespace = string.Empty;
                string namespaceaPrefix = string.Empty;

                if (currentTopicParts.Length == 1)
                {
                    topicName = currentTopicParts[0];
                    /*if (lastTopic != null)
                    {
                        topicNamespace = lastTopic.Namespace;
                    }*/
                }
                else
                {
                    topicName = currentTopicParts[1];
                    if (!namespaces.ContainsKey(currentTopicParts[0]))
                    {
                        throw new ApplicationException(string.Format("Prefix {0} not defined", currentTopicParts[0]));
                    }
                    topicNamespace = namespaces[currentTopicParts[0]];
                    namespaceaPrefix = currentTopicParts[0];
                }

                currentTopicInfo = new TopicInfo() { Name = topicName, Namespace = topicNamespace, ParentTopic = lastTopic, NamespacePrefix = namespaceaPrefix};
            }

            return currentTopicInfo;

        }        
        
        /// <summary>
        /// Checks if two topics are the same.
        /// </summary>
        /// <param name="actualTopic"></param>
        /// <param name="expectedTopic"></param>
        /// <returns></returns>
        public static bool TopicsMatch(TopicInfo actualTopic, TopicInfo expectedTopic)
        {
            bool match = false;

            TopicInfo currentFilterTopic = expectedTopic;
            TopicInfo currentActualTopic = actualTopic;
            while (true)
            {
                // both at the end;
                if (currentActualTopic == null && currentFilterTopic == null)
                {
                    match = true;
                    break;
                }

                // only one at the end
                // we don't accept child topics
                if (currentActualTopic == null || currentFilterTopic == null)
                {
                    break;
                }

                bool namespacesMatch = true;
                if (string.IsNullOrEmpty(currentActualTopic.Namespace) && 
                    string.IsNullOrEmpty(currentFilterTopic.Namespace))
                {
                    namespacesMatch = true;
                }
                else 
                { 
                    namespacesMatch = currentActualTopic.Namespace == currentFilterTopic.Namespace;
                }

                match = (namespacesMatch && currentActualTopic.Name == currentFilterTopic.Name);

                if (!match)
                {
                    break;
                }

                currentFilterTopic = currentFilterTopic.ParentTopic;
                currentActualTopic = currentActualTopic.ParentTopic;
            }

            return match;
        }
        
        /// <summary>
        /// Gets "plain" info (topic string and namespaces definitions)
        /// </summary>
        /// <returns></returns>
        public EventsTopicInfo GetPlainInfo()
        {
            // ordered list of topics;
            List<TopicInfo> orderedTopics = new List<TopicInfo>();

            TopicInfo currentTopic = this;
            while (currentTopic != null)
            {
                orderedTopics.Add(currentTopic);
                currentTopic = currentTopic.ParentTopic;
            }

            // namespaces prefixes cache;
            Dictionary<string, string> namespacePrefixes = new Dictionary<string, string>();

            foreach (TopicInfo topicInfo in orderedTopics)
            {
                if (!string.IsNullOrEmpty(topicInfo.Namespace))
                {
                    if (!namespacePrefixes.ContainsKey(topicInfo.Namespace))
                    {
                        string namespacePrefix = topicInfo.NamespacePrefix ;
                        namespacePrefixes.Add(topicInfo.Namespace, namespacePrefix);
                    }
                }
            }

            string topicPath = orderedTopics[orderedTopics.Count - 1].QName;

            for (int i = orderedTopics.Count - 2; i >= 0; i--)
            {
                TopicInfo nextTopic = orderedTopics[i];

                topicPath = string.Format("{0}/{1}",
                    topicPath, nextTopic.QName);
            }

            StringBuilder sb = new StringBuilder();
            foreach (string nameSpace in namespacePrefixes.Keys)
            {
                sb.AppendLine(string.Format("{0}=\"{1}\"", namespacePrefixes[nameSpace], nameSpace));
            }

            string namespacesDefinition = sb.ToString();

            return new EventsTopicInfo() {Topic = topicPath, NamespacesDefinition = namespacesDefinition};
        }
        
        /// <summary>
        /// Gets description (for logging purposes).
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            // ordered list of topics;
            List<TopicInfo> orderedTopics = new List<TopicInfo>();
            
            TopicInfo currentTopic = this;
            while (currentTopic != null)
            {
                orderedTopics.Add(currentTopic);
                currentTopic = currentTopic.ParentTopic;
            }

            // namespaces prefixes cache;
            Dictionary<string, string> namespacePrefixes = new Dictionary<string, string>();

            foreach (TopicInfo topicInfo in orderedTopics)
            {
                if (!string.IsNullOrEmpty(topicInfo.Namespace) && !string.IsNullOrEmpty(topicInfo.NamespacePrefix) )
                {
                    if (!namespacePrefixes.ContainsKey(topicInfo.Namespace))
                    {
                        namespacePrefixes.Add(topicInfo.Namespace, topicInfo.NamespacePrefix);
                    }
                }
            }

            string topicPath = null;
            topicPath = orderedTopics[orderedTopics.Count - 1].QName;
            
            for (int i = orderedTopics.Count-2; i >= 0 ;i--)
            {
                TopicInfo nextTopic = orderedTopics[i];

                topicPath = string.Format("{0}/{1}", 
                    topicPath, nextTopic.QName);
            }

            StringBuilder sb = new StringBuilder();
            bool bFirst = true;
            foreach (string nameSpace in namespacePrefixes.Keys)
            {
                string entry = string.Format("  xmlns:{0}={1}", namespacePrefixes[nameSpace], nameSpace);
                if (bFirst)
                {
                    sb.Append(entry);
                    bFirst = false;
                }
                else
                {
                    sb.Append(Environment.NewLine);
                    sb.Append(entry);
                }
            }

            string namespacesDefinition = sb.ToString();

            if (!string.IsNullOrEmpty(namespacesDefinition))
            {
                return string.Format("{0}, where {1}{2}", topicPath, Environment.NewLine, namespacesDefinition);
            }
            else
            {
                return topicPath;
            }
        }
        
        /// <summary>
        /// Creates topic string (like ns1:xxx/ns2:yyy/zzz) to be used during subscription creation.
        /// </summary>
        /// <param name="topicElement">Element in filter</param>
        /// <param name="topicInfo">Topic information</param>
        /// <returns></returns>
        public static string CreateTopicPath(XmlElement topicElement, TopicInfo topicInfo)
        {
            List<TopicInfo> orderedTopics = new List<TopicInfo>();
            TopicInfo currentTopic = topicInfo;
            while (currentTopic != null)
            {
                orderedTopics.Add(currentTopic);
                currentTopic = currentTopic.ParentTopic;
            }

            TopicInfo current = orderedTopics[orderedTopics.Count-1];
            string prefix = string.Empty;
            if (!string.IsNullOrEmpty(current.Namespace))
            {
                prefix = topicElement.GetNamespacePrefix(current.Namespace, current.NamespacePrefix);
            }

            string topicPath = string.IsNullOrEmpty(prefix) ? current.Name : string.Format("{0}:{1}", prefix, current.Name);
            string lastNamespace = current.Namespace;

            for (int i = orderedTopics.Count-2; i>=0; i--)
            {
                current = orderedTopics[i];
                string currentNamespace = string.IsNullOrEmpty(current.Namespace) ? lastNamespace : current.Namespace;

                prefix = topicElement.GetNamespacePrefix(currentNamespace, current.NamespacePrefix);

                string currentTopicName = string.IsNullOrEmpty(current.NamespacePrefix)
                                   ? current.Name
                                   : string.Format("{0}:{1}", prefix, current.Name);
                
                lastNamespace = currentNamespace;
                
                topicPath = string.Format("{0}/{1}", topicPath, currentTopicName);
            }
            
            return topicPath;
        }

        /// <summary>
        /// Gets topic from "leaf" to root.
        /// </summary>
        /// <param name="topicInfo"></param>
        /// <returns></returns>
        static List<TopicInfo> GetOrderedTopic(TopicInfo topicInfo)
        {
            List<TopicInfo> orderedTopics = new List<TopicInfo>();
            TopicInfo currentTopic = topicInfo;
            while (currentTopic != null)
            {
                orderedTopics.Add(currentTopic);
                currentTopic = currentTopic.ParentTopic;
            }
            return orderedTopics;
        }

    }

}
