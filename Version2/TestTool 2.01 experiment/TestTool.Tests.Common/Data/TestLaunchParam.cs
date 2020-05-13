///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Net;
using TestTool.Tests.Common.Discovery;

namespace TestTool.Tests.Common.TestEngine
{
    /// <summary>
    /// Parameters for launching separate tests.
    /// </summary>
    public class TestLaunchParam
    {
        /// <summary>
        /// Camera IP address
        /// </summary>
        public IPAddress CameraIp { get; set; }
        /// <summary>
        /// Camera UUID
        /// </summary>
        public string CameraUUID { get; set; }
        /// <summary>
        ///Device service address
        /// </summary>
        public string ServiceAddress { get; set; }
        /// <summary>
        /// Network interface selected
        /// </summary>
        public NetworkInterfaceDescription NIC { get; set; }
        /// <summary>
        /// Reboot timeout
        /// </summary>
        public int RebootTimeout { get; set; }
        /// <summary>
        /// Message timeout
        /// </summary>
        public int MessageTimeout { get; set; }
        /// <summary>
        /// Username used for operation where authentication is required
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Password used for operation where authentication is required
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Format of timestamp where authentication is required
        /// </summary>
        public bool UseUTCTimestamp { get; set; }
        /// <summary>
        /// Environment-depended value used in tests (DNS server etc)
        /// </summary>
        public EnvironmentSettings EnvironmentSettings { get; set; }
        /// <summary>
        /// Environment-depended value used in tests (DNS server etc)
        /// </summary>
        public string PTZNodeToken { get; set; }
        /// <summary>
        /// Interface for getting operator's answers
        /// </summary>
        public IOperator Operator { get; set; }
        /// <summary>
        /// Video form.
        /// </summary>
        public IVideoForm VideoForm { get; set; }

        private List<Enums.Feature> _features = new List<Enums.Feature>();
        /// <summary>
        /// Features list
        /// </summary>
        public List<Enums.Feature> Features
        {
            get { return _features; }
        }
        /// <summary>
        /// True, if hard-coded passwords should be used 
        /// </summary>
        public bool UseEmbeddedPassword { get; set; }
        /// <summary>
        /// First user-provided password
        /// </summary>
        public string Password1 { get; set; }
        /// <summary>
        /// Second user-provided password
        /// </summary>
        public string Password2 { get; set; }

        public int OperationDelay { get; set; }

    }
}
