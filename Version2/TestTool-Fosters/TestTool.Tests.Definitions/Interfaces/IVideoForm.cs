﻿///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.Definitions.Interfaces
{
    public interface IVideoFormEvent
    {
        void FireBeginStep(string Name);
        void FireStepPassed();
        //void FireStepFailed();
        void FireLogStepEvent(string Message);
    }

    public interface IVideoForm
    {
        IVideoFormEvent EventSink { set; }
        // HACK begin
        int VideoWidth { get; set; }
        int VideoHeight { get; set; }
        // HACK end

        // parameters for receiving RTP stream without RTSP
        bool   RTSP                  { get; set; }
        int    VideoFPS              { get; set; }
        string VideoCodecName        { get; set; }
        string AudioCodecName        { get; set; }
        string MulticastAddress      { get; set; }
        int    MulticastRtpPortVideo { get; set; }
        int    MulticastRtpPortAudio { get; set; }
        int    MulticastTTL          { get; set; }

        string Address { get; set; }
        string User { get; set; }
        string Password { get; set; }
        int HTTPPort { get; set; }
        bool TCP { get; set; }
        bool Multicast { get; set; }
        bool UseVideo { get; set; }
        int Timeout { get; set; }
        bool OPTIONS { get; set; }
        bool KEEPALIVE { get; set; }
        bool UseKeepAliveOptions { get; set; }


        bool EVENTS { get; set; }
        string GetEvents();

        bool SYNC { get; set; }
        bool WaitForStableKey(ref double Length);
        bool WaitForSync(ref int Length);

        void OpenWindow(bool WithAudio);
        void OpenWindow(bool WithAudio, bool WithVideo);
        void CloseWindow();
    }
}