using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using DUT.WithLogic.Base;
using System.Net.NetworkInformation;

namespace DUT.WithLogic.Services.DeviceManagement
{
    public class ONVIFDeviceManagementConfiguration
    {
        #region Members

        #region System

        long m_timeOfset;
        string m_timeZone;
        bool m_daylightSavings;
        Proxy.SetDateTimeType m_dateTimeType;
        List<Proxy.Scope> m_ConfigurableScopes;
        Proxy.DiscoveryMode m_RemoteDiscoveryMode;
        Proxy.DiscoveryMode m_DiscoveryMode;

        #endregion //System

        #region Network

        Proxy.HostnameInformation m_HostnameInformation;
        /// <summary>
        /// TODO: get from DHCP
        /// </summary>
        string m_HostnameFromDHCP;
        Proxy.DNSInformation m_DNSInformation;
        /// <summary>
        /// TODO: get from DHCP
        /// </summary>
        List<Proxy.IPAddress> m_DNSFromDHCP;
        Proxy.NTPInformation m_NTPInformation;
        /// <summary>
        /// TODO: get from DHCP
        /// </summary>
        List<Proxy.NetworkHost> m_NTPFromDHCP;
        Proxy.DynamicDNSInformation m_DynamicDNSInformation;
        /// <summary>
        /// TODO: get from real NIC
        /// </summary>
        List<Proxy.NetworkInterface> m_NetworkInterface;

        #endregion //Network

        #region Security

        /// <summary>
        /// For loading of configuration only
        /// </summary>
        List<Proxy.User> m_notFixedUsersList;
        Dictionary<string, Proxy.User> m_notFixedUsers;

        #endregion //Security

        #endregion //Members

        #region Properties

        /// <summary>
        /// For loading of configuration only
        /// </summary>
        public List<Proxy.User> NotFixedUsersList
        {
            get { return m_notFixedUsersList; }
            set { m_notFixedUsersList = value; }
        }

        [System.Xml.Serialization.XmlIgnore()]
        public List<Proxy.NetworkInterface> NetworkInterface
        {
            get
            {
                if (m_NetworkInterface == null)
                {
                    m_NetworkInterface = new List<Proxy.NetworkInterface>();

                    System.Net.NetworkInformation.NetworkInterface[] adapters = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                    foreach (System.Net.NetworkInformation.NetworkInterface adapter in adapters)
                    {
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            Proxy.NetworkInterface ni = new Proxy.NetworkInterface();
                            ni.token = adapter.Id;
                            ni.Enabled = (adapter.OperationalStatus == OperationalStatus.Up);
                            ni.Info = new Proxy.NetworkInterfaceInfo();
                            ni.Info.Name = adapter.Name;
                            ni.Info.HwAddress = adapter.GetPhysicalAddress().ToString();

                            //IPv4
                            if (adapter.GetIPProperties().GetIPv4Properties() != null)
                            {
                                ni.Info.MTUSpecified = true;
                                ni.Info.MTU = adapter.GetIPProperties().GetIPv4Properties().Mtu;
                                foreach (var unicastAddresses in adapter.GetIPProperties().UnicastAddresses)
                                {
                                    switch (unicastAddresses.Address.AddressFamily)
                                    {
                                        case System.Net.Sockets.AddressFamily.InterNetwork:
                                            if (ni.IPv4 == null)
                                            {
                                                ni.IPv4 = new Proxy.IPv4NetworkInterface();
                                                ni.IPv4.Enabled = true;
                                                ni.IPv4.Config = new Proxy.IPv4Configuration();
                                                ni.IPv4.Config.DHCP = adapter.GetIPProperties().GetIPv4Properties().IsDhcpEnabled;
                                                if (ni.IPv4.Config.DHCP)
                                                {
                                                    ni.IPv4.Config.FromDHCP = new Proxy.PrefixedIPv4Address();
                                                    ni.IPv4.Config.FromDHCP.Address = unicastAddresses.Address.ToString();
                                                    //TODO: get from unicastAddresses.IPv4Mask.Address
                                                    ni.IPv4.Config.FromDHCP.PrefixLength = 16;
                                                }
                                                else
                                                {
                                                    ni.IPv4.Config.Manual = new Proxy.PrefixedIPv4Address[1];
                                                    ni.IPv4.Config.Manual[0] = new Proxy.PrefixedIPv4Address();
                                                    ni.IPv4.Config.Manual[0].Address = unicastAddresses.Address.ToString();
                                                    //TODO: get from unicastAddresses.IPv4Mask.Address
                                                    ni.IPv4.Config.Manual[0].PrefixLength = 16;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                ni.Info.MTUSpecified = false;
                            }


                            //IPv6
                            if (adapter.GetIPProperties().GetIPv6Properties() != null)
                            {
                                ni.Info.MTUSpecified = true;
                                ni.Info.MTU = adapter.GetIPProperties().GetIPv6Properties().Mtu;
                                foreach (var unicastAddresses in adapter.GetIPProperties().UnicastAddresses)
                                {
                                    switch (unicastAddresses.Address.AddressFamily)
                                    {
                                        case System.Net.Sockets.AddressFamily.InterNetwork:
                                            if (ni.IPv6 == null)
                                            {
                                                ni.IPv6 = new Proxy.IPv6NetworkInterface();
                                                ni.IPv6.Enabled = true;
                                                ni.IPv6.Config = new Proxy.IPv6Configuration();
                                                //TODO: get from DHCP
                                                ni.IPv6.Config.DHCP = Proxy.IPv6DHCPConfiguration.Off;
                                                if (ni.IPv6.Config.DHCP == Proxy.IPv6DHCPConfiguration.Off)
                                                {
                                                    //TODO
                                                }
                                                else
                                                {
                                                    ni.IPv6.Config.Manual = new Proxy.PrefixedIPv6Address[1];
                                                    ni.IPv6.Config.Manual[0] = new Proxy.PrefixedIPv6Address();
                                                    ni.IPv6.Config.Manual[0].Address = unicastAddresses.Address.ToString();
                                                    //TODO: get from unicastAddresses.IPv4Mask.Address
                                                    ni.IPv6.Config.Manual[0].PrefixLength = 16;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                ni.Info.MTUSpecified = false;
                            }

                            m_NetworkInterface.Add(ni);
                        }
                    }
                }

                return m_NetworkInterface;
            }
            set { m_NetworkInterface = value; }
        }

        public Proxy.DynamicDNSInformation DynamicDNSInformation
        {
            get { return m_DynamicDNSInformation; }
            set { m_DynamicDNSInformation = value; }
        }

        public List<Proxy.NetworkHost> NTPFromDHCP
        {
            get { return m_NTPFromDHCP; }
            set { m_NTPFromDHCP = value; }
        }


        public Proxy.NTPInformation NTPInformation
        {
            get { return m_NTPInformation; }
            set { m_NTPInformation = value; }
        }

        public List<Proxy.IPAddress> DNSFromDHCP
        {
            get { return m_DNSFromDHCP; }
            set { m_DNSFromDHCP = value; }
        }

        public Proxy.DNSInformation DNSInformation
        {
            get { return m_DNSInformation; }
            set { m_DNSInformation = value; }
        }

        public string HostnameFromDHCP
        {
            get { return m_HostnameFromDHCP; }
            set { m_HostnameFromDHCP = value; }
        }

        public Proxy.HostnameInformation HostnameInformation
        {
            get { return m_HostnameInformation; }
            set { m_HostnameInformation = value; }
        }

        public Proxy.DiscoveryMode RemoteDiscoveryMode
        {
            get { return m_RemoteDiscoveryMode; }
            set { m_RemoteDiscoveryMode = value; }
        }

        public Proxy.DiscoveryMode DiscoveryMode
        {
            get { return m_DiscoveryMode; }
            set { m_DiscoveryMode = value; }
        }

        public List<Proxy.Scope> ConfigurableScopes
        {
            get { return m_ConfigurableScopes; }
            set { m_ConfigurableScopes = value; }
        }

        public Proxy.SetDateTimeType DateTimeType
        {
            get { return m_dateTimeType; }
            set { m_dateTimeType = value; }
        }

        public bool DaylightSavings
        {
            get { return m_daylightSavings; }
            set { m_daylightSavings = value; }
        }

        public string TimeZone
        {
            get { return m_timeZone; }
            set { m_timeZone = value; }
        }

        public long TimeOfset
        {
            get { return m_timeOfset; }
            set { m_timeOfset = value; }
        }

        [System.Xml.Serialization.XmlIgnore()]
        public Dictionary<string, Proxy.User> NotFixedUsers
        {
            get {
                if ((m_notFixedUsers == null) && (m_notFixedUsersList != null))
                {
                    m_notFixedUsers = m_notFixedUsersList.ToDictionary(C => C.Username);
                }
                return m_notFixedUsers; 
            }
            set { m_notFixedUsers = value; }
        }


        #endregion //Properties

        static public Proxy.Scope CreateConfigurableScope(string scopeValue)
        {
            Proxy.Scope res = new Proxy.Scope();

            res.ScopeDef = Proxy.ScopeDefinition.Configurable;
            res.ScopeItem = scopeValue;

            return res;
        }

        private static ONVIFDeviceManagementConfiguration Load(string configurationPath)
        {
            using (XmlReader reader = XmlReader.Create(Engine.ONVIFServiceList.FullUri(configurationPath)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ONVIFDeviceManagementConfiguration));
                return (ONVIFDeviceManagementConfiguration)serializer.Deserialize(reader);
            }

        }

        public static void Serialize()
        {
            ONVIFDeviceManagementConfiguration tmp = new ONVIFDeviceManagementConfiguration();

            //DiscoveryMode
            tmp.DiscoveryMode = Proxy.DiscoveryMode.Discoverable;
            tmp.RemoteDiscoveryMode = Proxy.DiscoveryMode.NonDiscoverable;

            //Configurable Scopes
            tmp.ConfigurableScopes = new List<Proxy.Scope>();
            
            Proxy.Scope scope = new Proxy.Scope();
            scope.ScopeDef = Proxy.ScopeDefinition.Configurable;
            scope.ScopeItem = "onvif://www.onvif.org/Location/Novgorod";
            tmp.ConfigurableScopes.Add(scope);

            //DateTime
            tmp.DateTimeType = Proxy.SetDateTimeType.Manual;
            tmp.DaylightSavings = true;
            tmp.TimeOfset = 0;
            tmp.TimeZone = "UTC0:0";

            //DNSFromDHCP
            tmp.DNSFromDHCP = new List<Proxy.IPAddress>();

            Proxy.IPAddress DNSFromDHCP = new Proxy.IPAddress();
            DNSFromDHCP.Type = Proxy.IPType.IPv4;
            DNSFromDHCP.IPv4Address = "192.168.10.4";

            tmp.DNSFromDHCP.Add(DNSFromDHCP);

            //DNSInformation
            tmp.DNSInformation = new Proxy.DNSInformation();
            tmp.DNSInformation.FromDHCP = false;

            //DynamicDNSInformation
            tmp.DynamicDNSInformation = new Proxy.DynamicDNSInformation();

            //Hostname
            tmp.HostnameFromDHCP = "hostnamefromDHCP";
            tmp.HostnameInformation = new Proxy.HostnameInformation();
            tmp.HostnameInformation.Name = "duthostname";
            tmp.HostnameInformation.FromDHCP = false;

            //NetworkInterface
            tmp.NetworkInterface = new List<Proxy.NetworkInterface>();

            //NotFixedUsers
            tmp.NotFixedUsersList = new List<Proxy.User>();

            Proxy.User User = new Proxy.User();
            User.Username = "user";
            User.UserLevel = Proxy.UserLevel.User;
            User.Password = "2";

            tmp.NotFixedUsersList.Add(User);

            
            //NTP
            tmp.NTPInformation = new Proxy.NTPInformation();
            tmp.NTPInformation.FromDHCP = false;
            tmp.NTPInformation.NTPManual = new Proxy.NetworkHost[1];
            tmp.NTPInformation.NTPManual[0] = new Proxy.NetworkHost();
            tmp.NTPInformation.NTPManual[0].Type = Proxy.NetworkHostType.IPv4;
            tmp.NTPInformation.NTPManual[0].IPv4Address = "192.168.10.3";

            tmp.NTPFromDHCP = new List<Proxy.NetworkHost>();
            tmp.NTPFromDHCP.Add(tmp.NTPInformation.NTPManual[0]);

            using (XmlWriter writer = XmlWriter.Create(@"D:\2.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ONVIFDeviceManagementConfiguration));
                serializer.Serialize(writer, tmp);
            }
        }

        public static ONVIFDeviceManagementConfiguration Load()
        {
            return Load(Base.AppPaths.PATH_DEVICEMANAGEMENTCONFIGURATION);
        }



        public static ONVIFDeviceManagementConfiguration HardReset()
        {
            return Load(Base.AppPaths.PATH_DEVICEMANAGEMENTCONFIGURATION_FACTORYDEFAULTS);
        }

        public void SetNetworkInterface(string InterfaceToken, Proxy.NetworkInterfaceSetConfiguration NetworkInterfaceNew)
        {
            Proxy.NetworkInterface temp = NetworkInterface.Find(C => C.token == InterfaceToken);

            if (NetworkInterfaceNew.AnyAttr != null)
            {
                temp.AnyAttr = NetworkInterfaceNew.AnyAttr;
            }

            if (NetworkInterfaceNew.EnabledSpecified)
            {
                temp.Enabled = NetworkInterfaceNew.Enabled;
            }

            if (NetworkInterfaceNew.Extension != null)
            {
                if (temp.Extension == null)
                {
                    temp.Extension = new Proxy.NetworkInterfaceExtension();
                }

                if (NetworkInterfaceNew.Extension.Any != null)
                {
                    temp.Extension.Any = NetworkInterfaceNew.Extension.Any;
                }

                if (NetworkInterfaceNew.Extension.Dot11 != null)
                {
                    temp.Extension.Dot11 = NetworkInterfaceNew.Extension.Dot11;
                }

                if (NetworkInterfaceNew.Extension.Dot3 != null)
                {
                    temp.Extension.Dot3 = NetworkInterfaceNew.Extension.Dot3;
                }

                if (NetworkInterfaceNew.Extension.Extension != null)
                {
                    if (temp.Extension.Extension == null)
                    {
                        temp.Extension.Extension = new Proxy.NetworkInterfaceExtension2();
                    }

                    if (NetworkInterfaceNew.Extension.Extension.Any != null)
                    {
                        temp.Extension.Extension.Any = NetworkInterfaceNew.Extension.Extension.Any;
                    }

                }
            }

            if (NetworkInterfaceNew.IPv4 != null)
            {
                if (temp.IPv4 == null)
                {
                    temp.IPv4 = new Proxy.IPv4NetworkInterface();
                }

                if (NetworkInterfaceNew.IPv4.EnabledSpecified)
                {
                    temp.IPv4.Enabled = NetworkInterfaceNew.IPv4.Enabled;
                }

                if (NetworkInterfaceNew.IPv4.DHCPSpecified)
                {
                    if (temp.IPv4.Config == null)
                    {
                        temp.IPv4.Config = new Proxy.IPv4Configuration();
                    }

                    temp.IPv4.Config.DHCP = NetworkInterfaceNew.IPv4.DHCP;
                }

                if (NetworkInterfaceNew.IPv4.Manual != null)
                {
                    if (temp.IPv4.Config == null)
                    {
                        temp.IPv4.Config = new Proxy.IPv4Configuration();
                    }

                    temp.IPv4.Config.Manual = NetworkInterfaceNew.IPv4.Manual;
                }

            }

            if (NetworkInterfaceNew.IPv6 != null)
            {
                if (temp.IPv6 == null)
                {
                    temp.IPv6 = new Proxy.IPv6NetworkInterface();
                }

                if (NetworkInterfaceNew.IPv6.EnabledSpecified)
                {
                    temp.IPv6.Enabled = NetworkInterfaceNew.IPv6.Enabled;
                }

                if (NetworkInterfaceNew.IPv6.DHCPSpecified)
                {
                    if (temp.IPv6.Config == null)
                    {
                        temp.IPv6.Config = new Proxy.IPv6Configuration();
                    }

                    temp.IPv6.Config.DHCP = NetworkInterfaceNew.IPv6.DHCP;
                }

                if (NetworkInterfaceNew.IPv6.Manual != null)
                {
                    if (temp.IPv6.Config == null)
                    {
                        temp.IPv6.Config = new Proxy.IPv6Configuration();
                    }

                    temp.IPv6.Config.Manual = NetworkInterfaceNew.IPv6.Manual;
                }

                if (NetworkInterfaceNew.Link != null)
                {
                    if (temp.Link == null)
                    {
                        temp.Link = new Proxy.NetworkInterfaceLink();
                    }

                    temp.Link.AdminSettings = NetworkInterfaceNew.Link;
                }

                if (NetworkInterfaceNew.MTUSpecified)
                {
                    if (temp.Info != null)
                    {
                        temp.Info = new Proxy.NetworkInterfaceInfo();
                    }

                    temp.Info.MTUSpecified = NetworkInterfaceNew.MTUSpecified;
                    temp.Info.MTU = NetworkInterfaceNew.MTU;
                }
            }
        }

    }
}