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

            SecureMethod = "GetUsers";
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

        public int SearchTimeout { get; set; }

        public string SecureMethod { get; set; }
        
        public int SubscriptionTimeout { get; set; }
        public string EventTopic { get; set; }
        public string TopicNamespaces { get; set; }

        public int RelayOutputDelayTimeMonostable { get; set; }

        public List<XmlElement> RawAdvancedSettings { get; set; }

        [XmlIgnore]
        public List<object> AdvancedSettings { get; set; }

    }
}
