using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.Events;

namespace DUT.PACS.Simulator.BackDoorServices
{
    public class TopicsConverter
    {
        /// <summary>
        /// Finds Topic in TopicSet using TOpicInformation
        /// </summary>
        /// <param name="topicSet">Topic set</param>
        /// <param name="topicInfo">Topic information</param>
        /// <returns>Topic with name/namespaces the same as in topicInfo</returns>
        /// <remarks>Used for firing events via EventControlService.</remarks>
        public static Topic Find(TopicSet topicSet, TopicInformation topicInfo)
        {
            Topic topic = null;

            string topicString = topicInfo.TopicString;

            string[] topics = topicString.Split('/');
            if (topics.Length > 0)
            {
                string rootTopic = topics[0];

                string[] segments = rootTopic.Split(':');
                if (segments.Length == 2)
                {
                    string prefix = segments[0];
                    string name = segments[1];
                    string ns = topicInfo.Namespaces.Where(NS => NS.Prefix == prefix).FirstOrDefault().Namespace;

                    Topic root = null;
                    foreach (Topic t in topicSet.Topics)
                    {
                        if (t.Name == name && t.Namespace == ns)
                        {
                            root = t;
                            break;
                        }
                    }

                    Topic current = root;
                    string lastNs = ns;

                    if (current != null)
                    {
                        for (int i = 1; i < topics.Length; i++)
                        {
                            string nextPart = topics[i];
                            segments = nextPart.Split(':');

                            if (segments.Length == 1)
                            {
                                name = segments[0];
                                ns = null;
                            }
                            else
                            {
                                name = segments[1];
                                prefix = segments[0];
                                if (!string.IsNullOrEmpty(prefix))
                                {
                                    ns = topicInfo.Namespaces.Where(NS => NS.Prefix == prefix).FirstOrDefault().Namespace;
                                }
                                else
                                {
                                    ns = null;
                                }
                            }
                            Topic foundChild = null;
                            foreach (Topic child in current.SubTopics)
                            {
                                bool nsMatch = (string.IsNullOrEmpty(ns) && string.IsNullOrEmpty(child.Namespace)) || (child.Namespace == ns);
                                if (nsMatch && child.Name == name)
                                {
                                    foundChild = child;
                                    break;
                                }
                            }
                            current = foundChild;
                            if (foundChild == null)
                            {
                                break;
                            }
                        }
                        topic = current;
                    }
                }
            }

            return topic;

        }
    }
}
