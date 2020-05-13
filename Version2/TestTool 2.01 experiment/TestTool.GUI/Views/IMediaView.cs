///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using Media = TestTool.Proxies.Media;

namespace TestTool.GUI.Views
{
    /// <summary>
    /// Media page interface.
    /// </summary>
    interface IMediaView : IView
    {
        string MediaAddress { get; set; }
        void ShowError(Exception e);
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
