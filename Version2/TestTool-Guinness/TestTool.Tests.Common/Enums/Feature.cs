///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.Common.Enums
{
    /// <summary>
    /// Features.
    /// </summary>
    public enum Feature
    {
        Discovery,
        BYE,
        Security,
        WSU,
        Digest,
        Capabilities,
        GetServices,
        GetCapabilities,
        DeviceService,
        Network,
        NTP,
        IPv6,
        ZeroConfiguration,
        DynamicDNS,
        IPFilter,
        System,
        SystemLogging,
        DeviceIO,
        DeviceIORelayOutputs,
        DeviceIORelayOutputsBistable,
        DeviceIORelayOutputsBistableOpen,
        DeviceIORelayOutputsBistableClosed,
        DeviceIORelayOutputsMonostable,
        DeviceIORelayOutputsMonostableOpen,
        DeviceIORelayOutputsMonostableClosed,
        EventsService,
        MediaService,
        Video,
        JPEG,
        H264,
        MPEG4,
        H264OrMPEG4,
        Audio,
        G711,
        G726,
        AAC,
        RTSS,
        RTPUDP,
        RTPRTSPHTTP,
        RTPRTSPTCP,
        RTPMulticastUDP,
        SnapshotUri,
        AudioOutput,
        AudioOutputG711,
        AudioOutputG726,
        AudioOutputAAC,
        PTZService,
        PTZAbsolute,
        PTZAbsolutePanTilt,
        PTZAbsoluteZoom,
        PTZRelative,
        PTZRelativePanTilt,
        PTZRelativeZoom,
        PTZAbsoluteOrRelative,
        PTZContinious,
        PTZContinuousPanTilt,
        PTZContinuousZoom,
        PTZAbsoluteOrRelativePanTilt,
        PTZAbsoluteOrRelativeZoom,
        PTZPresets,
        PTZHome,
        PTZConfigurableHome,
        PTZFixedHome,
        PTZAuxiliary,
        PTZSpeed,
        PTZSpeedPanTilt,
        PTZSpeedZoom,
        DeviceIoService,
        RelayOutputs,
        ImagingService,
        AnalyticsService
    }
}
