﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using TestTool.GUI.Data;

namespace TestTool.GUI.Controllers
{
    class ManagementController : Controller<Views.IManagementView>
    {
        public ManagementController(Views.IManagementView view)
            :base(view)
        {

        }

        /// <summary>
        /// Returns list of available Ethernet network interfaces
        /// </summary>
        /// <returns>List of network interfaces</returns>
        public List<NetworkInterfaceDescription> GetNetworkInterfaces()
        {
            List<NetworkInterfaceDescription> interfaces = new List<NetworkInterfaceDescription>();
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if ((adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet) ||
                    (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                {
                    foreach (UnicastIPAddressInformation uinfo in adapter.GetIPProperties().UnicastAddresses)
                    {
                        //if (uinfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            interfaces.Add(new NetworkInterfaceDescription(adapter, uinfo.Address));
                        }
                    }
                }
            }
            interfaces = interfaces.OrderByDescending(I => I.IP.AddressFamily == AddressFamily.InterNetwork).ToList();
            return interfaces;
        }

        public void SwitchToWorkingMode()
        {
            View.SwitchToWorkingMode();
        }

        public void SwitchToIdleMode()
        {
            View.SwitchToIdleMode();
        }

        public override void UpdateContext()
        {
            Context context = Context.Instance;
            context.Environment.BaseAddress = View.BaseAddress;
            context.Environment.DeviceAddress = View.DutAddress;
            context.Credentials.Username = View.Username;
            context.Credentials.Password = View.Password;
        }
    }
}
