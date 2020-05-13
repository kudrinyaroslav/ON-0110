///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.Common.TestEngine
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

        string Address { get; set; }
        string User { get; set; }
        string Password { get; set; }
        int HTTPPort { get; set; }
        bool TCP { get; set; }
        //bool Multicast { get; set; }
        int Timeout { get; set; }
        bool OPTIONS { get; set; }
        bool KEEPALIVE { get; set; }


        bool EVENTS { get; set; }
        string GetEvents();

        bool SYNC { get; set; }
        bool WaitForStableKey(ref double Length);
        bool WaitForSync(ref int Length);

        void OpenWindow(bool WithAudio);
        void CloseWindow();
    }
}
