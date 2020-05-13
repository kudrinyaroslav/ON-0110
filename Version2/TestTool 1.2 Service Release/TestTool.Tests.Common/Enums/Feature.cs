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
    /// Features to be taken into account. Mandatory features are not taken into account.
    /// </summary>
    public enum Feature
    {
        None,
        NTP,
        IPv6,
        Video,
        JPEG,
        H264,
        MPEG4,
        Audio,
        G711,
        G726,
        AAC,
        RTSS,
        RTPUDP,
        RTPRTSPHTTP,
        RTPRTSPTCP,
        RTPMulticastUDP,
        BiDirectional,
        BiDirectionalG711,
        BiDirectionalG726,
        BiDirectionalAAC,
        PTZ,
        PTZAbsolute,
        PTZAbsolutePanTilt,
        PTZAbsoluteZoom,
        PTZRelative,
        PTZRelativePanTilt,
        PTZRelativeZoom,
        PTZAbsoluteOrRelative,
        PTZContinious,
        PTZContiniousPanTilt,
        PTZContiniousZoom,
        PTZPresets,
        PTZHome,
        PTZConfigurableHome,
        PTZFixedHome,
        PTZAuxiliary,
        PTZSpeed,
        PTZSpeedPanTilt,
        PTZSpeedZoom,
        Imaging,
        IO,
        RelayOutputs,
        Backchannel
    }
}
