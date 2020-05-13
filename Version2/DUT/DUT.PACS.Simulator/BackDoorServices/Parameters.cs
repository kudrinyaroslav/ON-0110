using System.Collections.Generic;
using DUT.PACS.Simulator.Events;

namespace DUT.PACS.Simulator.BackDoorServices
{
    /// <summary>
    /// Notification message SimpleItem representation.
    /// </summary>
    public class SimpleItem
    {
        public string Name { get; set; }

        public string Value { get; set; }
    
    }

    /// <summary>
    /// Namespace description
    /// </summary>
    public class NamespaceDescription
    {
        public string Prefix { get; set; }
        public string Namespace { get; set; }

    }

    /// <summary>
    /// Topic information
    /// </summary>
    public class TopicInformation
    {
        public TopicInformation()
        {
            Namespaces = new List<NamespaceDescription>();
        }

        /// <summary>
        /// Namespace definitions
        /// </summary>
        public List<NamespaceDescription> Namespaces { get;  set; }

        /// <summary>
        /// Topic string (like aaa:XXX/YYY/bbb:ZZZ)
        /// </summary>
        public string TopicString { get; set; }

        /// <summary>
        /// Topic string (like aaa:XXX/YYY/bbb:ZZZ)
        /// </summary>
        /// <remarks>Namespaces definitions for parent topic are not presented.</remarks>
        public string ParentTopicString { get; set; }

        /// <summary>
        /// Flag inditcating whether it's topic or topics namespace
        /// </summary>
        public bool IsTopic { get; set; }

        public bool IsProperty { get; set; }

        private List<SimpleItemDescription> _sourceItems = new List<SimpleItemDescription>();
        /// <summary>
        /// Message source items.
        /// </summary>
        public List<SimpleItemDescription> SourceItems
        {
            get { return _sourceItems; }
            set { _sourceItems = value;}
        }

        private List<SimpleItemDescription> _dataItems = new List<SimpleItemDescription>();
        
        /// <summary>
        /// Message data items
        /// </summary>
        public List<SimpleItemDescription> DataItems
        {
            get { return _dataItems; }
            set { _dataItems = value; }
        }
    }

}
