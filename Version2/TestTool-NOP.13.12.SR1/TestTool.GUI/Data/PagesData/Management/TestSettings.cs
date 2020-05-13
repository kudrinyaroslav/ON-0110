///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Values which can be defined by operator.
    /// </summary>
    public class TestSettings
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TestSettings()
        {
            UseEmbeddedPassword = true;
            OperationDelay = 5000;

            Password1 = "OnvifTest123";
            Password2 = "OnvifTest321";

            RelayOutputDelayTimeMonostable = 20;
            SubscriptionTimeout = 60;

            SecureMethod = "GetUsers";

            SearchTimeout = 10;

            RetentionTime = "P1D";
        }

        /// <summary>
        /// PTZ node
        /// </summary>
        public string PTZNodeToken { get; set; }
        /// <summary>
        /// Video source
        /// </summary>
        public string VideoSourceToken { get; set; }
        /// <summary>
        /// Recording
        /// </summary>
        public string RecordingToken { get; set; }
        /// <summary>
        /// Filter for Metadata search tests
        /// </summary>
        public string MetadataFilter { get; set; }
        /// <summary>
        /// Flag indicating whether default passwords can be used.
        /// </summary>
        public bool UseEmbeddedPassword { get; set; }
        /// <summary>
        /// Password for first operation
        /// </summary>
        public string Password1 { get; set; }
        /// <summary>
        /// Password for second operation
        /// </summary>
        public string Password2 { get; set; }
        /// <summary>
        /// Operation delay
        /// </summary>
        public int OperationDelay { get; set; }
        /// <summary>
        /// Recovery delay
        /// </summary>
        public int RecoveryDelay { get; set; }
        /// <summary>
        /// Timeout for search operations
        /// </summary>
        public int SearchTimeout { get; set; }
        /// <summary>
        /// Secure method for security test
        /// </summary>
        public string SecureMethod { get; set; }
        /// <summary>
        /// Subscription timeout
        /// </summary>
        public int SubscriptionTimeout { get; set; }
        /// <summary>
        /// Event topic
        /// </summary>
        public string EventTopic { get; set; }
        /// <summary>
        /// Topic namespaces definition
        /// </summary>
        public string TopicNamespaces { get; set; }
        /// <summary>
        /// Relay output delay time
        /// </summary>
        public int RelayOutputDelayTimeMonostable { get; set; }
        /// <summary>
        /// Retention time for recording creation
        /// </summary>
        public string RetentionTime { get; set; }

        /// <summary>
        /// Additional settings (currently not in use)
        /// </summary>
        public List<XmlElement> RawAdvancedSettings { get; set; }

        [XmlIgnore]
        public List<object> AdvancedSettings { get; set; }

    }
}
