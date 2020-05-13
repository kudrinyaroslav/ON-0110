using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TestTool.HttpTransport.Interfaces
{
    /// <summary>
    /// Security modes
    /// </summary>
    public enum Security
    {
        /// <summary>
        /// No security allowed. HTTP 401 not handled
        /// </summary>
        None,
        /// <summary>
        /// Digest security. HTTP 401 status is handled
        /// </summary>
        Digest,
        /// <summary>
        /// Digest testing
        /// </summary>
        DigestTesting,
        /// <summary>
        /// WS-Username Token security
        /// </summary>
        WS
    }

    /// <summary>
    /// Settings for Digest testing 
    /// </summary>
    public class DigestTestingSettings
    {
        /// <summary>
        /// Username missing in header
        /// </summary>
        public bool UserNameMissing { get; set; }
        /// <summary>
        /// Realm missing in header
        /// </summary>
        public bool RealmMissing { get; set; }
        /// <summary>
        /// Nonce missing in header
        /// </summary>
        public bool NonceMissing { get; set; }
        /// <summary>
        /// URI missing
        /// </summary>
        public bool UriMissing { get; set; }
        /// <summary>
        /// Response missing
        /// </summary>
        public bool ResponseMissing { get; set; }
    }

    /// <summary>
    /// Provides credentials information
    /// </summary>
    public interface ICredentialsProvider : ITransportController
    {
        string Username { get; }
        string Password { get; }
        Security Security { get; }
        DigestTestingSettings DigestTestingSettings { get; }
        bool HTTPS { get; set; }
        X509Certificate2 ClientCertificate { get; set; }
        X509Certificate  ServerCertificate { get; set; }
    }
}
