using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace DUT.PACS.Simulator.Events
{
    /// <summary>
    /// Filter topic information
    /// </summary>
    public class SubscriptionTopicFilter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="topic">Topic or namespace</param>
        /// <param name="subtree">True, if subtree is passed</param>
        private SubscriptionTopicFilter(Topic topic, bool subtree)
        {
            Topic = topic;
            SubTree = subtree;
        }

        /// <summary>
        /// Creates SubscriptionTopicFilter taking topic set and topic filter information
        /// </summary>
        /// <param name="topicset">Set of topics.</param>
        /// <param name="topicElement">Filter element.</param>
        /// <param name="topicString">Simgle topic string</param>
        /// <returns>Information about how topic is used in filters.</returns>
        /// <remarks>Current implementation allows passing several topics in topic filter.
        /// TopicElement is used to resolve namespaces.</remarks>
        public static SubscriptionTopicFilter Create(TopicSet topicset, 
            XmlText topicElement, 
            string topicString)
        {
            Topic topic = null;

            // handle "//." sign (subtree)
            bool subtree = topicString.EndsWith("//.");
            if (subtree)
            {
                topicString = topicString.Substring(0, topicString.Length - 3);
            }

            string[] topics = topicString.Split('/');
            // go from the root to the topic of interest
            if (topics.Length > 0)
            {
                string rootTopic = topics[0];

                // root must be qualified!
                string[] segments = rootTopic.Split(':');
                if (segments.Length == 2)
                {
                    // Find root.
                    // Define namespace of the root topic.
                    string prefix = segments[0];
                    string name = segments[1];
                    string ns = topicElement.GetNamespaceOfPrefix(prefix);

                    Topic root = null;
                    foreach (Topic t in topicset.Topics)
                    {
                        if (t.Name == name && t.Namespace == ns)
                        {
                            root = t;
                            break;
                        }
                    }

                    Topic current = root;
                    string lastNs = ns;

                    // if root found
                    if (current != null)
                    {
                        // look for other parts
                        for (int i = 1; i < topics.Length; i++)
                        {
                            string nextPart = topics[i];
                            segments = nextPart.Split(':');

                            // can be qualifid or not qualified.
                            if (segments.Length == 1)
                            {
                                name = segments[0];
                                ns = null;
                            }
                            else
                            {
                                name = segments[1];
                                prefix = segments[0];
                                ns = topicElement.GetNamespaceOfPrefix(prefix);
                            }

                            // looks for the next topic in the path.
                            Topic foundChild = null;
                            foreach (Topic child in current.SubTopics)
                            {
                                if (child.Name == name && child.Namespace == ns)
                                {
                                    foundChild = child;
                                    break;
                                }
                            }

                            // if not found - break
                            if (foundChild == null)
                            {
                                break;
                            }

                            // go to the next
                            current = foundChild;
                        }
                        topic = current;
                    }
                }
            }

            if (topic == null)
            {
                return null;
            }
            return new SubscriptionTopicFilter(topic, subtree);
        }

        /// <summary>
        /// Topic
        /// </summary>
        public Topic Topic { get; private set; }
        /// <summary>
        /// True, if subtree is used.
        /// </summary>
        public bool SubTree { get; private set; }

    }
}
