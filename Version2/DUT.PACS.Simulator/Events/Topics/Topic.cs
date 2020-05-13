using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace DUT.PACS.Simulator.Events
{
    /// <summary>
    /// Topic representation
    /// </summary>
    public class Topic
    {
        /// <summary>
        /// Name (local)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id for topic comparison in the code.
        /// </summary>
        private Guid _id = Guid.NewGuid();
        
        [XmlIgnore]
        public Guid Id
        {
            get { return _id; }
        }

        private string _namespace;
 
        /// <summary>
        /// Namespace
        /// </summary>
        public string Namespace
        {
            get
            {
                string ns = _namespace;
                return ns;
            }
            set 
            {
                _namespace = value;
            }
        }

        private string _prefix;
        /// <summary>
        /// Prefix (default for serialization)
        /// </summary>
        public string Prefix
        {
            get
            {
                string prefix = _prefix;
                return prefix;
            }
            set 
            {
                _prefix = value;
            }
        }
        
        /// <summary>
        /// Flag indicating whether it's topic or namespace
        /// </summary>
        public bool IsTopic { get; set; }
        
        [XmlIgnore]
        public Topic ParentTopic { get; set; }

        List<Topic> _subTopics = new List<Topic>();
        /// <summary>
        /// Child topics
        /// </summary>
        public List<Topic> SubTopics
        {
            get { return _subTopics; }
        }

        private MessageDescription _messageDescription;
        /// <summary>
        /// Message  description
        /// </summary>
        public MessageDescription MessageDescription
        {
            get { return _messageDescription; }
            set { _messageDescription = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Topic()
            : this(string.Empty, string.Empty, string.Empty)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ns"></param>
        /// <param name="prefix"></param>
        public Topic(string name, string ns, string prefix)
            : this(name, ns, prefix, false)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ns"></param>
        /// <param name="prefix"></param>
        /// <param name="isTopic"></param>
        public Topic(string name, string ns, string prefix, bool isTopic)
        {
            Name = name;
            Namespace = ns;
            Prefix = prefix;
            IsTopic = isTopic;
            if (isTopic)
            {
                MessageDescription = new MessageDescription();
            }
        }

        /// <summary>
        /// Adds child topics
        /// </summary>
        /// <param name="topics">List of topics to add as childers</param>
        void Add(IEnumerable<Topic> topics)
        {
            foreach(Topic topic in topics)
            {
                Add(topic);
            }
        }

        /// <summary>
        /// Adds child topics
        /// </summary>
        /// <param name="child">Topis to add</param>
        public void Add(params Topic[] child)
        {
            Add(new List<Topic>(child)); 
        }

        /// <summary>
        /// Adds single child
        /// </summary>
        /// <param name="child"></param>
        public void Add(Topic child)
        {
            if (child.ParentTopic != null)
            {
                child.ParentTopic._subTopics.Remove(child);
            }
            child.ParentTopic = this;
            _subTopics.Add(child);
        }

        /// <summary>
        /// Gets description (for logging purposes).
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            // ordered list of topics;
            List<Topic> orderedTopics = new List<Topic>();

            Topic currentTopic = this;
            while (currentTopic != null)
            {
                orderedTopics.Add(currentTopic);
                currentTopic = currentTopic.ParentTopic;
            }

            // namespaces prefixes cache;
            Dictionary<string, string> namespacePrefixes = new Dictionary<string, string>();

            foreach (Topic topicInfo in orderedTopics)
            {
                if (!string.IsNullOrEmpty(topicInfo.Namespace))
                {
                    if (!namespacePrefixes.ContainsKey(topicInfo.Namespace))
                    {
                        namespacePrefixes.Add(topicInfo.Namespace, topicInfo.Prefix);
                    }
                }
            }

            string topicPath = null;
            topicPath = orderedTopics[orderedTopics.Count - 1].QName;
            string lastNs = orderedTopics[orderedTopics.Count - 1].Namespace;

            for (int i = orderedTopics.Count - 2; i >= 0; i--)
            {
                Topic nextTopic = orderedTopics[i];

                string topicName = string.Empty;
                if (lastNs == nextTopic.Namespace)
                {
                    topicName = nextTopic.Name;
                }
                else
                {
                    topicName = nextTopic.QName;
                }
                lastNs = nextTopic.Namespace;
                topicPath = string.Format("{0}/{1}", topicPath, topicName);
            }
#if true
            return topicPath;
#else
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
#endif
        }

        /// <summary>
        /// Qualified name
        /// </summary>
        private string QName
        {
            get { return string.IsNullOrEmpty(Prefix) ? Name : string.Format("{0}:{1}", Prefix, Name); }
        }

        /// <summary>
        /// Adds topic XML element to parent element.
        /// </summary>
        /// <param name="parentElement"></param>
        /// <returns></returns>
        public XmlElement Convert(XmlElement parentElement)
        {
            XmlDocument owner = parentElement.OwnerDocument;

            XmlElement topicElement = null;

            bool declareNamespaces = true;
            if (string.IsNullOrEmpty(this.Namespace))
            {
                declareNamespaces = false;
            }
            else
            {
                if (this.ParentTopic != null )
                {
                    if (this.ParentTopic.Namespace == _namespace)
                    {
                        declareNamespaces = false;
                    }
                }
            }
            if (declareNamespaces)
            {
                topicElement = owner.CreateElement(Prefix, Name, Namespace);
            }
            else
            {
                topicElement = owner.CreateElement(Name, _namespace);
            }
            parentElement.AppendChild(topicElement);
            if (IsTopic)
            {
                XmlAttribute topicAttribute = owner.CreateAttribute("wstop", "topic", "http://docs.oasis-open.org/wsn/t-1");
                topicAttribute.Value = "true";
                topicElement.Attributes.Append(topicAttribute);
                if (MessageDescription != null)
                {
                    topicElement.AppendChild(CreateMessageDescription(owner, topicElement, MessageDescription));
                }
            }

            foreach (Topic child in SubTopics)
            {
                topicElement.AppendChild(child.Convert(topicElement));
            }
            return topicElement;
        }
        

        /// <summary>
        /// Creates message description
        /// </summary>
        /// <param name="owner">Owner document</param>
        /// <param name="topicElement">Topic element for getting namespaces</param>
        /// <param name="message">Message description</param>
        /// <returns>Message element</returns>
        private XmlElement CreateMessageDescription(XmlDocument owner, 
            XmlElement topicElement,
            MessageDescription message)
        {
            XmlElement messageDescription;
            XmlAttribute isPropertyAttribute;
            XmlElement source;
            XmlElement data;

            messageDescription = owner.CreateElement("tt", "MessageDescription", "http://www.onvif.org/ver10/schema");

            isPropertyAttribute = owner.CreateAttribute("IsProperty");
            isPropertyAttribute.Value = message.IsProperty.ToString().ToLower();
            messageDescription.Attributes.Append(isPropertyAttribute);

            if (message.SourceItems.Count > 0)
            {

                source = owner.CreateElement("tt", "Source", "http://www.onvif.org/ver10/schema");
                messageDescription.AppendChild(source);

                foreach (SimpleItemDescription item in message.SourceItems)
                {
                    XmlElement simpleItemDescription = owner.CreateElement("tt", "SimpleItemDescription",
                                                                "http://www.onvif.org/ver10/schema");
                    source.AppendChild(simpleItemDescription);

                    XmlAttribute name = owner.CreateAttribute("Name");
                    name.Value = item.Name;
                    simpleItemDescription.Attributes.Append(name);

                    XmlAttribute type = owner.CreateAttribute("Type");

                    string prefix = messageDescription.GetPrefixOfNamespace(item.Type.Namespace);
                    if (string.IsNullOrEmpty(prefix))
                    {
                        prefix =  GetPrefixOfNamespace(topicElement, item.Type.Namespace);
                    }
                    type.Value = string.Format("{0}:{1}", prefix, item.Type.Name);
                    simpleItemDescription.Attributes.Append(type);
                }
            }
            if (message.DataItems.Count > 0)
            {
                //MessageDescription/Data
                data = owner.CreateElement("tt", "Data", "http://www.onvif.org/ver10/schema");
                messageDescription.AppendChild(data);
                
                foreach (SimpleItemDescription item in message.DataItems)
                {

                    XmlElement simpleItemDescription = owner.CreateElement("tt", "SimpleItemDescription",
                                                                   "http://www.onvif.org/ver10/schema");
                    data.AppendChild(simpleItemDescription);

                    XmlAttribute name = owner.CreateAttribute("Name");
                    name.Value = item.Name;
                    simpleItemDescription.Attributes.Append(name);

                    XmlAttribute type = owner.CreateAttribute("Type");
                    string prefix = messageDescription.GetPrefixOfNamespace(item.Type.Namespace);
                    if (string.IsNullOrEmpty(prefix))
                    {
                        prefix = GetPrefixOfNamespace(topicElement, item.Type.Namespace);
                    }
                    type.Value = string.Format("{0}:{1}", prefix, item.Type.Name);
                    simpleItemDescription.Attributes.Append(type);
                }
            }

            return messageDescription;
        }

        /// <summary>
        /// Gets prefix of namespace. Adds namespace declaration, if needed.
        /// </summary>
        /// <param name="topicElement"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        string GetPrefixOfNamespace(XmlElement topicElement, string ns)
        {
            string prefix = topicElement.GetPrefixOfNamespace(ns);

            if (string.IsNullOrEmpty(prefix))
            {
                int i = 1;
                while (true)
                {
                    prefix = string.Format("tns{0}", i);
                    string nsUri = topicElement.GetNamespaceOfPrefix(prefix);
                    if (string.IsNullOrEmpty(nsUri))
                    {
                        XmlAttribute nsDeclaration = topicElement.OwnerDocument.CreateAttribute(string.Format("xmlns:{0}", prefix) );
                        nsDeclaration.Value = ns;
                        topicElement.Attributes.Append(nsDeclaration);
                        break;
                    }
                    i++;
                }
            }

            return prefix;
        }

    }

    /// <summary>
    /// Message simple item description (name and TYPE, not value)
    /// </summary>
    public class SimpleItemDescription
    {
        /// <summary>
        /// Item name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Item type (qualified)
        /// </summary>
        public XmlQualifiedName Type { get; set; }

        public SimpleItemDescription()
        {

        }

        public SimpleItemDescription(string name, string type, string typeNamespace)
        {
            Name = name;
            Type = new XmlQualifiedName(type, typeNamespace);
        }
    }

    /// <summary>
    /// Message description
    /// </summary>
    public class MessageDescription
    {
        public bool IsProperty { get; set; }

        private List<SimpleItemDescription> _sourceItems = new List<SimpleItemDescription>();
        
        /// <summary>
        /// Source items
        /// </summary>
        public List<SimpleItemDescription> SourceItems
        {
            get { return _sourceItems; }
        }

        private List<SimpleItemDescription> _dataItems = new List<SimpleItemDescription>();

        /// <summary>
        /// Data items
        /// </summary>
        public List<SimpleItemDescription> DataItems
        {
            get { return _dataItems; }
        }

    }



}
