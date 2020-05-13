///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using TestTool.Tests.Common.Discovery;

namespace TestTool.GUI.Views
{
    interface IDiscoveryView : IView
    {
        NetworkInterfaceDescription NIC { get; }
        string NICAddress { set; }
        List<DeviceDiscoveryData> Devices { get; }
        DeviceDiscoveryData Current { get; }
        string ServiceAddress { get; set; }
        IPAddress DeviceAddress { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        void ShowError(Exception e);
        void UpdateFormTitle();
        void UpdateButtons();
    }
}
