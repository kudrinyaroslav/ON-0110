///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.UI;

namespace TestTool.GUI.Views
{
    interface IManagementView : IView
    {
        void DisplayProfiles(List<Data.Profile> profiles);

        Data.Profile CurrentProfile { get; }
        
        bool OpenFileForEditing { get; set; }

        ITestSettingsView SettingsView { get; }
    }

    /// <summary>
    /// Interface for direct access to Settings control.
    /// </summary>
    interface ITestSettingsView: IView
    {
        void AddSettingsPages(List<SettingsTabPage> pages);
        List<object> AdvancedSettings { get; set; }
        
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
        int RecoveryDelay { get; set; }
        int SearchTimeout { get; set; }
        string MetadataFilter { get; set; }

        string PTZNodeToken { get; set; }
        string VideoSourceToken { get; set; }
        string SecureMethod { get; set; }
        int SubscriptionTimeout { get; set; }
        string EventTopic { get; set; }
        string TopicNamespaces { get; set; }
        int RelayOutputDelayTimeMonostable { get; set; }
        string RecordingToken { get; set; }

        string RetentionTime { get; set; }

        void SetPTZNodes(PTZNode[] nodes);
        void SetEventsTopic(List<EventsTopicInfo> topics);
        void SetVideoSources(VideoSource[] sources);

        void ShowMissingSettings(string name);
        void SetRecordings(IEnumerable<string> recordings);
    }

}
