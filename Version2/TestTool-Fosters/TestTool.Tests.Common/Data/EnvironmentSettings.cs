///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.Tests.Common.TestEngine
{
    /// <summary>
    /// Environment-dependent values to be used during the test.
    /// </summary>
    public class EnvironmentSettings
    {
        public string DnsIpv4 { get; set; }
        public string NtpIpv4 { get; set; }
        public string DnsIpv6 { get; set; }
        public string NtpIpv6 { get; set; }
        public string DefaultGateway { get; set; }
        public string DefaultGatewayIpv6 { get; set; }
    }
}
