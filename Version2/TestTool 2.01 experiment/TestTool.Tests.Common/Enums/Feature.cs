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
        ManagementService,
        EventsService,
        MediaService,
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
        //RTPTCP,
        RTPRTSPTCP,
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
        PTZSpeedZoom
    }
}
