///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using TestTool.Tests.Common.Enums;
using System.Xml.Serialization;

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
            Features = new List<Feature>();
            TestCases = new List<string>();
        }

        /// <summary>
        /// Profile name.
        /// </summary>
        public string Name { get; set; }
        
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
        /// Operation delay.
        /// </summary>
        public int OperationDelay { get; set; }

        /// <summary>
        /// "Interactive test first" option selected by test operator.
        /// </summary>
        public bool InteractiveFirst { get; set; }
        
        /// <summary>
        /// Feautres selected by test operator.
        /// </summary>
        public List<Feature> Features { get; set; }
        /// <summary>
        /// Service selected by test operator
        /// </summary>
        public List<Service> Services { get; set; }
        /// <summary>
        /// Device type(s)
        /// </summary>
        public DeviceType DeviceType { get; set; }
        /// <summary>
        /// Test cases selected.
        /// </summary>
        public List<string> TestCases { get; set; }
        /// <summary>
        /// Test groups selected.
        /// </summary>
        public List<string> TestGroups { get; set; }
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


        [NonSerialized]
        private static Profile _defaultProfile;

        //ToDo : this is used only in one place. May be can be removed.
        /// <summary>
        /// Default profile.
        /// </summary>
        public static Profile Default
        {
            get
            {
                if (_defaultProfile == null)
                {
                    InitializeDefaultProfile();
                }
                return _defaultProfile;
            }
        }

        static void InitializeDefaultProfile()
        {
            _defaultProfile = new Profile();

            _defaultProfile.InterTests = 1000;
            _defaultProfile.Message = 5000;
            _defaultProfile.Reboot = 30000;

        }
    }
}
