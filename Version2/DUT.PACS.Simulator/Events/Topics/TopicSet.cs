using System.Collections.Generic;
using System.Xml;

namespace DUT.PACS.Simulator.Events
{
    /// <summary>
    ///  Set of topics.
    /// </summary>
    public class TopicSet
    {
        public TopicSet()
        {
            _rootTopics = new List<Topic>();
        }

        private List<Topic> _rootTopics;
        /// <summary>
        /// Root topics
        /// </summary>
        public List<Topic> Topics
        {
            get { return _rootTopics; }
        }


        #region public methods
        /// <summary>
        /// Adds topics tree to parent XML element.
        /// </summary>
        /// <param name="parentElement"></param>
        public void AddTo(XmlElement parentElement)
        {
            foreach (Topic topic in _rootTopics)
            {
                topic.Convert(parentElement);
            }
        }

        /// <summary>
        /// Adds root topic to topic set.
        /// </summary>
        /// <param name="rootTopic"></param>
        public void AddTopic(Topic rootTopic)
        {
            _rootTopics.Add(rootTopic);
        }

        #endregion
    }
}
