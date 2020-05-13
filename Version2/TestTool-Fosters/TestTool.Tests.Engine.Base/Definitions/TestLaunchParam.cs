///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Net;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.Tests.Engine.Base.Definitions
{
    /// <summary>
    /// Parameters for launching separate tests.
    /// </summary>
    public class TestLaunchParam : BaseTestParam
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

        #region Timeouts

        /// <summary>
        /// Reboot timeout
        /// </summary>
        public int RebootTimeout { get; set; }
        /// <summary>
        /// Message timeout
        /// </summary>
        public int MessageTimeout { get; set; }

        #endregion

        /// <summary>
        /// Authentication type
        /// </summary>
        private HttpTransport.Interfaces.Security _security;

        /// <summary>
        /// Authentication type
        /// </summary>
        public Security Security
        {
            get { return _security; }
            set { _security = value; }
        }

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
        /// Videou source for PTZ tests
        /// </summary>
        public string VideoSourceToken { get; set; }

        /// <summary>
        /// Interface for getting operator's answers
        /// </summary>
        public IOperator Operator { get; set; }
        /// <summary>
        /// Video form.
        /// </summary>
        public IVideoForm VideoForm { get; set; }

        private List<Feature> _features = new List<Feature>();
        /// <summary>
        /// Features list
        /// </summary>
        public List<Feature> Features
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
        /// <summary>
        /// Delay for time-consuming operations
        /// </summary>
        public int OperationDelay { get; set; }
        /// <summary>
        /// Delay after sending request to allow the DUT to get ready to next requests.
        /// </summary>
        public int RecoveryDelay { get; set; }
        /// <summary>
        /// Secure method for security tests
        /// </summary>
        public string SecureMethod { get; set; }
        /// <summary>
        /// Event topic for subscription tests.
        /// </summary>
        public string EventTopic { get; set; }
        /// <summary>
        /// Topic namespaces definition
        /// </summary>
        public string TopicNamespaces { get; set; }
        /// <summary>
        /// Subscription timeout
        /// </summary>
        public int SubscriptionTimeout { get; set; }
        /// <summary>
        /// Delay for relay output tests.
        /// </summary>
        public int RelayOutputDelayTimeMonostable { get; set; }

    }
}
