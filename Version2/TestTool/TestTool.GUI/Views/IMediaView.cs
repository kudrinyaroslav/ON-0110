///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Media = TestTool.Proxies.Onvif;

namespace TestTool.GUI.Views
{
    interface IMediaView : IView
    {
        string MediaAddress { get; set; }
        void EnableControls(bool enable);
        void DisplayLog(string logEntry);
        void SetProfiles(Media.Profile[] profiles);
        void SetVideoSourceConfigs(Media.VideoSourceConfiguration[] configs);
        void SetVideoEncoderConfigs(Media.VideoEncoderConfiguration[] configs);
        void SetVideoEncoderConfigOptions(Media.VideoEncoderConfigurationOptions options);
        void SetAudioSourceConfigs(Media.AudioSourceConfiguration[] configs);
        void SetAudioEncoderConfigs(Media.AudioEncoderConfiguration[] configs);
        void SetAudioEncoderConfigOptions(Media.AudioEncoderConfigurationOptions options);
        void ShowVideo(Media.MediaUri uri, Media.VideoEncoderConfiguration encoder, Media.AudioEncoderConfiguration audio);
    }
}
