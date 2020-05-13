using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.GUI.Data.Log
{
    [Serializable]
    public class ManagementSettings
    {
        public Timeouts Timeouts { get; set; }
        public Miscellaneous Miscellaneous { get; set; }
    }

    [Serializable]
    public class Timeouts
    {
        public int MessageTimeout { get; set; }
        public int RebootTimeout { get; set; }
        public int TimeBetweenTests { get; set; }
        public int OperationDelay { get; set; }
        public int TimeBetweenRequests { get; set; }
    }

    [Serializable]
    public class Miscellaneous
    {

        public string DnsIpv4 { get; set; }

        public string NtpIpv4 { get; set; }

        public string DefaultGatewayIpv4 { get; set; }

        public string DnsIpv6 { get; set; }

        public string NtpIpv6 { get; set; }

        public string DefaultGatewayIpv6 { get; set; }

        public string PTZNodeToken { get; set; }

        public bool UseEmbeddedPassword { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public string SecureMethod { get; set; }

        public string EventTopic { get; set; }
        public string TopicNamespaces { get; set; }
        public int SubscriptionTimeout { get; set; }

        public int RelayOutputDelayTime { get; set; }

        public string RecordingToken { get; set; }
        public int SearchTimeout { get; set; }
        public string MetadataFilter { get; set; }

        public string RetentionTime { get; set; }

    }

}
