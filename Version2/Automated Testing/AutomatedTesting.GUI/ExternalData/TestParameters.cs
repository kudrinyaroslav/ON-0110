using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Common.TestEngine;
using System.IO;
using System.Xml.Serialization;

namespace AutomatedTesting.GUI.ExternalData
{
    public class TestParameters
    {
        public TestParameters()
        {
        }

        /// <summary>
        /// Device management service address
        /// </summary>
        public string DeviceServiceAddress { get;  set;   }        
        public string DeviceIP { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        #region IP addresses

        public string Address { get; set; }
        
        public string DnsIpv4 { get; set; }

        public string NtpIpv4 { get; set; }

        public string DnsIpv6 { get; set; }

        public string NtpIpv6 { get; set; }

        public string DefaultGatewayIpv4 { get; set; }

        public string DefaultGatewayIpv6 { get; set; }

        #endregion

        #region Timeouts
        /// <summary>
        /// Message timeout
        /// </summary>
        public int? MessageTimeout { get; set; }
        /// <summary>
        /// Reboot timeout
        /// </summary>
        public int? RebootTimeout { get; set; }
        /// <summary>
        /// Time between tests if last test failed.
        /// </summary>
        public int? TimeBetweenTests { get; set; }
        public int? OperationDelay { get; set; }
        public int? RecoveryDelay { get; set; }

        #endregion

        #region TestParameters

        public string PTZNodeToken { get; set; }        
        public string VideoSourceToken { get; set; }

        public bool? UseEmbeddedPassword { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public string SecureMethod { get; set; }
        
        public string EventTopic { get; set; }
        public string TopicNamespaces { get; set; }
        public int? SubscriptionTimeout { get; set; }

        public int? RelayOutputDelayTime { get; set; }

        public string RecordingToken { get; set; }
        public int? SearchTimeout { get; set; }
        public string MetadataFilter { get; set; }

        #endregion
        
        public static TestParameters Load(string fileName)
        {
            if (File.Exists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TestParameters));
                Stream stream = File.OpenRead(fileName);
                TestParameters parameters = (TestParameters)(serializer.Deserialize(stream));
                stream.Close();
                return parameters;
            }
            return null;
        }

        [NonSerialized]
        public const string ROOT = "TestParameters";

    }

}
