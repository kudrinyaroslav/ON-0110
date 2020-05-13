///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using Media = TestTool.Proxies.Onvif;

namespace TestTool.GUI.Views
{
    interface IPtzView : IView
    {
        void DisplayLog(string log);
        string PTZAddress { get; set; }
        string MediaAddress { get; set; }
        void EnableControls(bool enable);
        void ShowVideo(Media.MediaUri uri, Media.VideoEncoderConfiguration encoder);
        void SetProfiles(Media.Profile[] profiles);
        void OnPTZConfigurationAdded(string profile, string config);
    }
}
