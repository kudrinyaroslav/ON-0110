///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using TestTool.Tests.Common.Enums;
using TestTool.GUI.Data;
using TestTool.Proxies.PTZ;

namespace TestTool.GUI.Views
{
    /// <summary>
    /// Management page interface.
    /// </summary>
    interface IManagementView : IView
    {
        void DisplayProfiles(List<Profile> profiles);
        Profile CurrentProfile { get; }

        int MessageTimeout { get; set; }
        int RebootTimeout { get; set; }
        int TimeBetweenTests { get; set; }

        string NtpIpv4 { get; set; }
        string DnsIpv4 { get; set; }
        string NtpIpv6 { get; set; }
        string DnsIpv6 { get; set; }
        string GatewayIpv4 { get; set; }
        string GatewayIpv6 { get; set; }

        bool UseEmbeddedPassword { get; set; }
        string Password1 { get; set; }
        string Password2 { get; set; }
        int OperationDelay { get; set; }

        string PTZNodeToken { get; set; }
        void SetPTZNodes(PTZNode[] nodes);
        
        void EnableDeviceTypes(bool bEnable);
        DeviceType DeviceTypes { get; set; }
        void SelectFeatures(List<Service> services, List<Feature> features);
        List<Feature> Features { get; }
        List<Service> Services { get; }
        
        void ShowError(Exception e);

    }
}
