///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Environment settings (NTP, DNS, Gateway)
    /// </summary>
    public class EnvironmentSettings
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EnvironmentSettings()
        {
            NtpIpv4 = "10.1.1.1";
            DnsIpv4 = "10.1.1.1";
            GatewayIpv4 = "10.1.1.1";
            NtpIpv6 = "2001:1:1:1:1:1:1:1";
            DnsIpv6 = "2001:1:1:1:1:1:1:1";
            GatewayIpv6 = "2001:1:1:1:1:1:1:1";
        }

        /// <summary>
        /// NTP server address used for test (IPv4)
        /// </summary>
        public string NtpIpv4 { get; set; }
        /// <summary>
        /// DNS server address used for test (IPv4)
        /// </summary>
        public string DnsIpv4 { get; set; }
        /// <summary>
        /// Gateway address used or test (IPv4)
        /// </summary>
        public string GatewayIpv4 { get; set; }
        /// <summary>
        /// NTP server address used for test (IPv6)
        /// </summary>
        public string NtpIpv6 { get; set; }
        /// <summary>
        /// DNS server address used for test (IPv6)
        /// </summary>
        public string DnsIpv6 { get; set; }
        /// <summary>
        /// Gateway address used or test (IPv6)
        /// </summary>
        public string GatewayIpv6 { get; set; }

    }
}
