///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using TestTool.Tests.Common.Enums;
using TestTool.Proxies.Onvif;

namespace TestTool.GUI.Views
{
    interface IManagementView : IView
    {
        void DisplayProfiles(List<Data.Profile> profiles);
        //void InitFeatures();

        //string UserName { get; set; }
        //string Password { get; set; }
        int MessageTimeout { get; set; }
        int RebootTimeout { get; set; }
        int TimeBetweenTests { get; set; }

        string NtpIpv4 { get; set; }
        string DnsIpv4 { get; set; }
        string NtpIpv6 { get; set; }
        string DnsIpv6 { get; set; }
        string GatewayIpv4 { get; set; }
        string GatewayIpv6 { get; set; }

        //bool UtcTimestamp { get; set; }
        bool UseEmbeddedPassword { get; set; }
        string Password1 { get; set; }
        string Password2 { get; set; }
        int OperationDelay { get; set; }
        int RecoveryDelay { get; set; }

        string PTZNodeToken { get; set; }

        List<Feature> Features { get; set; }

        Data.Profile CurrentProfile { get; }

        void SetPTZNodes(PTZNode[] nodes);
        void ShowError(Exception e);
    }
}
