using System;
using System.Collections.Generic;
using TestTool.Tests.Definitions.Data;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace AutomatedTesting.GUI.Utils
{
    class NetworkUtils
    {
        public static List<NetworkInterfaceDescription> GetNetworkInterfaces()
        {
            List<NetworkInterfaceDescription> interfaces = new List<NetworkInterfaceDescription>();
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            bool hasIpv6 = false;
            bool hasIpv4 = false;
            foreach (NetworkInterface adapter in adapters)
            {
                if ((adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet) ||
                    (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                {
                    foreach (UnicastIPAddressInformation uinfo in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (uinfo.Address != null)
                        {
                            hasIpv6 |= uinfo.Address.AddressFamily == AddressFamily.InterNetworkV6;
                            hasIpv4 |= uinfo.Address.AddressFamily == AddressFamily.InterNetwork;
                        }
                        interfaces.Add(new NetworkInterfaceDescription(adapter, uinfo.Address));
                    }
                }
            }
            return interfaces;
        }

        public static System.Net.IPAddress GetIP(string hostName, bool ipv6)
        {
            System.Net.IPAddress address = null;
            if (!string.IsNullOrEmpty(hostName))
            {
                try
                {
                    if (!System.Net.IPAddress.TryParse(hostName, out address))
                    {
                        System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(hostName);
                        if (host != null)
                        {
                            foreach (System.Net.IPAddress ip in host.AddressList)
                            {
                                if (((ipv6) && (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)) ||
                                  ((!ipv6) && (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)))
                                {
                                    address = ip;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            return address;
        }

    }
}
