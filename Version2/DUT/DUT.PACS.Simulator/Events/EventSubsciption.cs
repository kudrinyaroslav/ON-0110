using System;
using System.Collections.Generic;

namespace DUT.PACS.Simulator.Events
{
    public class FilteredSubscription
    {
        private List<SubscriptionTopicFilter> _topics = new List<SubscriptionTopicFilter>();

        /// <summary>
        /// Adds topics of interest
        /// </summary>
        /// <param name="topic">Topic</param>
        public void AddTopic(SubscriptionTopicFilter topic)
        {
            _topics.Add(topic);
        }

        /// <summary>
        /// Checks if notifications for topic passed should be sent to subscribers
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public bool SubscribedTo(Topic topic)
        {
            if (_topics.Count == 0)
            {
                // No filter - subscribed to all topics
                return true;
            }
            else
            {
                foreach (SubscriptionTopicFilter t in _topics)
                {
                    if (t.SubTree)
                    {
                        // Subscribed, if topic is a descendant of topic in filter
                        Guid id = t.Topic.Id;
                        Topic current = topic;
                        while (current != null)
                        {
                            if (id == current.Id)
                            {
                                return true;
                            }
                            current = current.ParentTopic;
                        }
                    }
                    else
                    {
                        // Subscribed, if topics are the same 
                        if (t.Topic.Id == topic.Id)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
    }
    /// <summary>
    /// Events subscription
    /// </summary>
    public class EventSubsciption: FilteredSubscription
    {
        /// <summary>
        /// Consumer
        /// </summary>
        string m_consumerReference;
        /// <summary>
        /// Termination time
        /// </summary>
        DateTime m_terminationTime;
        /// <summary>
        /// PullPoint as renew
        /// </summary>
        bool m_PullPointAsRenew = false;

        public bool PullPointAsRenew
        {
            get { return m_PullPointAsRenew; }
            set { m_PullPointAsRenew = value; }
        }

        
        /// <summary>
        /// Termination time
        /// </summary>
        public DateTime TerminationTime
        {
            get { return m_terminationTime; }
            set { m_terminationTime = value; }
        }

        /// <summary>
        /// Consumer reference
        /// </summary>
        public string ConsumerReference
        {
            get { return m_consumerReference; }
            set { m_consumerReference = value; }
        }
    }
}
