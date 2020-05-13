///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;

namespace TestTool.Tests.Definitions.Data
{
    public class NICListItem
    {
        public NetworkInterfaceDescription NIC { get; protected set; }
        public bool HWStyle { get; protected set; }

        public NICListItem(NetworkInterfaceDescription nic, bool hwStyle)
        {
            NIC = nic;
            HWStyle = hwStyle;
        }
        public override string ToString()
        {
            return NIC != null ? NIC.ToString(HWStyle) : string.Empty;
        }
    }
    public class NetworkInterfaceDescription
    {
        public IPAddress IP { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public NetworkInterfaceDescription(NetworkInterface adapter, IPAddress ip)
        {
            Name = adapter.Name;
            Description = adapter.Description;
            IP = ip;
        }
        public NetworkInterfaceDescription(string name, IPAddress ip)
        {
            Name = name;
            Description = name;
            IP = ip;
        }
        public string ToString(bool hwStyle)
        {
            return !IPAddress.Equals(IP, IPAddress.Any) && !IPAddress.Equals(IP, IPAddress.IPv6Any) ?
                string.Format("{0} ({1})", IP.ToString(), hwStyle ? Description : Name) :
                Name;
        }
    }
}
