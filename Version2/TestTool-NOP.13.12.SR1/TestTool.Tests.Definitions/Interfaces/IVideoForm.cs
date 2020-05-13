///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
        string MulticastAddressAudio { get; set; }
        int    MulticastRtpPortVideo { get; set; }
        int    MulticastRtpPortAudio { get; set; }
        int    MulticastTTL          { get; set; }

        string CustomSetupFields { get; set; }
        string CustomPlayFields { get; set; }
        string CustomPauseFields { get; set; }
        bool DoSetupOnReplay { get; set; }
        bool ReplayMode { get; set; }
        int ReplayMaxDuration { get; set; }
        int ReplayPauseWait { get; set; }
        bool ReplayReverse { get; set; }

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
        string MulticastMultipleSetup { get; set; }
        bool CheckActualResolution { get; set; }
        bool CheckJPEGExtension { get; set; }

        bool Base64LineBreaks { get; set; }
        int NICIndex { get; set; }
        WaitHandle StopEvent { get; set; }
        bool DebugPage { get; set; }

        bool EVENTS { get; set; }
        string GetEvents();

        bool SYNC { get; set; }
        bool WaitForStableKey(ref double Length);
        bool WaitForSync(ref int Length);

        void OpenWindow(bool WithAudio);
        void OpenWindow(bool WithAudio, bool WithVideo);
        void CloseWindow();
        void Replay(Action<Action, Action, Action> actionControl, bool useVideo, bool useAudio, bool useMeta);

        void Reset();
    }
}
