///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Settings to be saved.
    /// </summary>
    [Serializable]
    public class Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Profile()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="profileName">Profile name.</param>
        public Profile(string profileName)
        {
            Name = profileName;
            TestCases = new List<string>();
        }

        /// <summary>
        /// Profile name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// True if UTS timestamp should be used.
        /// </summary>
        public bool UtcTimeStamp { get; set; }

        /// <summary>
        /// Reboot timeout.
        /// </summary>
        public int Reboot { get; set; }
        /// <summary>
        /// Message timeout.
        /// </summary>
        public int Message { get; set; }
        /// <summary>
        /// Time between tests.
        /// </summary>
        public int InterTests { get; set; }
        /// <summary>
        /// Operation delay.
        /// </summary>
        public int OperationDelay { get; set; }
        /// <summary>
        /// Time between requests
        /// </summary>
        public int RecoveryDelay { get; set; }
        /// <summary>
        /// NTP server address (IPv4).
        /// </summary>
        public string NtpIpv4 { get; set; }
        /// <summary>
        /// DNS address (IPv4).
        /// </summary>
        public string DnsIpv4 { get; set; }
        /// <summary>
        /// Gateway server address (IPv4).
        /// </summary>
        public string GatewayIpv4 { get; set; }

        /// <summary>
        /// NTP server address (IPv6).
        /// </summary>
        public string NtpIpv6 { get; set; }
        /// <summary>
        /// DNS address (IPv6).
        /// </summary>
        public string DnsIpv6 { get; set; }
        /// <summary>
        /// Gateway address (IPv6).
        /// </summary>
        public string GatewayIpv6 { get; set; }

        /// <summary>
        /// "Use embedded passwords" option selected by test operator.
        /// </summary>
        public bool UseEmbeddedPassword { get; set; }
        /// <summary>
        /// First password.
        /// </summary>
        public string Password1 { get; set; }
        /// <summary>
        /// Second password.
        /// </summary>
        public string Password2 { get; set; }


        /// <summary>
        /// PTZ node
        /// </summary>
        public string PTZNodeToken { get; set; }
        /// <summary>
        /// Video source
        /// </summary>
        public string VideoSourceToken { get; set; }

        public string SecureMethod { get; set; }

        public int SubscriptionTimeout { get; set; }

        public string EventTopic { get; set; }

        public string TopicNamespaces { get; set; }

        public int RelayOutputDelayTime { get; set; }

        public List<XmlElement> AdvancedSettings { get; set; }

        /// <summary>
        /// Test cases selected.
        /// </summary>
        public List<string> TestCases { get; set; }
        /// <summary>
        /// Test groups selected.
        /// </summary>
        public List<string> TestGroups { get; set; }

        public string FirmwareFilePath { get; set; }

        public CredentialIdentifierValue CredentialIdentifierValueFirst { get; set; }
        public CredentialIdentifierValue CredentialIdentifierValueSecond { get; set; }
        public CredentialIdentifierValue CredentialIdentifierValueThird { get; set; }
    }

    /// <summary>
    /// Profiles set.
    /// </summary>
    [Serializable]
    [XmlRoot("Settings")]
    public class ProfilesSet
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProfilesSet()
        {
            Profiles = new List<Profile>();
        }

        /// <summary>
        /// Profiles defined.
        /// </summary>
        [XmlArrayAttribute("Profiles")]
        public List<Profile> Profiles { get; set; }

        /// <summary>
        /// Adds profile.
        /// </summary>
        /// <param name="profile">Profile to be added.</param>
        public void Add(Profile profile)
        {
            Profiles.Add(profile);
        }
    }
}
