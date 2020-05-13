///////////////////////////////////////////////////////////////////////////
//!  @author        Ilya Gozman
////
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.Media;
using TestTool.Proxies.Onvif;

namespace TestTool.Tests.TestCases.TestSuites
{
    //[TestClass]
    class RtpTestSuite : RTSSTestSuite
    {
        private const string PATH = "Real Time Streaming\\Start and Stop Multicast Streaming";

        public RtpTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        [ Test( Name = "START AND STOP MULTICAST STREAMING – JPEG (IPv4)",
                Path = PATH,
                Order = "05.01.01",
                Id = "5-1-1",
                Category = Category.RTSS,
                Version = 2.0,
                Interactive = true,
                RequirementLevel = RequirementLevel.Should ) ]
        public void NvtStartAndStopMulticastStreamingJpegIPv4()
        {
            Profile profile = null;
            RunTest(() =>
            {
                VideoEncoderConfigurationOptions videoOptions = null;
                profile = SelectVideoProfile(
                              VideoEncoding.JPEG, "JPEG",
                              (options) => { return (options.JPEG != null); },
                              ref videoOptions);

                SetMulticastSettings(profile, true, false, IPType.IPv4);

                SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, true);

                bool needResetAudio = false;
                RunStep(
                    () => 
                    { 
                        needResetAudio = (profile.AudioEncoderConfiguration != null);
                        if (needResetAudio)
                        {
                            LogStepEvent("AudioEncoderConfiguration exists");
                        }
                        else
                        {
                            LogStepEvent("No AudioEncoderConfiguration");
                        }
                    },
                    "Check AudioEncoderConfiguration");

                if (needResetAudio)
                {
                    profile.AudioEncoderConfiguration.Multicast.Address.Type = IPType.IPv4;
                    profile.AudioEncoderConfiguration.Multicast.Address.IPv4Address = "0.0.0.0";
                    SetAudioEncoderConfiguration(profile.AudioEncoderConfiguration, false, true);
                }

                bool needResetMeta = false;
                RunStep(
                    () =>
                    {
                        needResetMeta = (profile.MetadataConfiguration != null);
                        if (needResetMeta)
                        {
                            LogStepEvent("MetadataConfiguration exists");
                        }
                        else
                        {
                            LogStepEvent("No MetadataConfiguration");
                        }
                    },
                    "Check MetadataConfiguration");

                if (needResetMeta)
                {
                    profile.MetadataConfiguration.Multicast.Address.Type = IPType.IPv4;
                    profile.MetadataConfiguration.Multicast.Address.IPv4Address = "0.0.0.0";
                    SetMetadataConfiguration(profile.MetadataConfiguration, false);
                }

                VideoUtils.AdjustVideo(
                    _videoForm, null, null, _messageTimeout, TransportProtocol.UDP, 
                    StreamType.RTPMulticast, null, profile.VideoEncoderConfiguration);

                RunStep(
                    () => { Client.StartMulticastStreaming(profile.token); },
                    "StartMulticastStreaming");

                DoRequestDelay();

                _videoForm.VideoFPS = profile.VideoEncoderConfiguration.RateControl.FrameRateLimit;
                _videoForm.MulticastAddress = profile.VideoEncoderConfiguration.Multicast.Address.IPv4Address;
                _videoForm.MulticastRtpPortVideo = profile.VideoEncoderConfiguration.Multicast.Port;
                _videoForm.MulticastTTL = profile.VideoEncoderConfiguration.Multicast.TTL;
                _videoForm.VideoCodecName = "JPEG";
                _videoForm.EventSink = this;
                _videoForm.OpenWindow(false);
                VideoIsOpened = true;

                Assert(_operator.GetYesNoAnswer("Do you observe video?"),
                    "Operator does not observe video",
                    "Video quality check (manual)");

                VideoIsOpened = false;
                _videoForm.CloseWindow();
                _videoForm.EventSink = null;
            },
            () =>
            {
                if (profile != null)
                {
                    RunStep(
                        () => { Client.StopMulticastStreaming(profile.token); },
                        "StopMulticastStreaming");

                    DoRequestDelay();
                }
                if (VideoIsOpened)
                {
                    VideoIsOpened = false;
                    _videoForm.CloseWindow();
                    _videoForm.EventSink = null;
                }
            }
            );
        }
    }
}