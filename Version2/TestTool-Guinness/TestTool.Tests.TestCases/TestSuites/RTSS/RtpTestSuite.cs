///////////////////////////////////////////////////////////////////////////
//!  @author        Ilya Gozman
////
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Common.Media;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
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
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[]{Feature.MediaService, Feature.RTPMulticastUDP},
                FunctionalityUnderTest =  new Functionality[]{Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming}) ]
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

                VideoUtils.AdjustVideo(
                    _videoForm, null, null, _messageTimeout, TransportProtocol.UDP, 
                    StreamType.RTPMulticast, null, profile.VideoEncoderConfiguration);

                RunStep(
                    () => { Client.StartMulticastStreaming(profile.token); },
                    "StartMulticastStreaming");

                DoRequestDelay();

                int fps = 1;
                if (profile.VideoEncoderConfiguration.RateControl != null)
                {
                    fps = profile.VideoEncoderConfiguration.RateControl.FrameRateLimit;
                }
                else
                {
                    if (videoOptions.JPEG.FrameRateRange != null)
                    {
                        fps = videoOptions.JPEG.FrameRateRange.Min;
                    }
                }
                _videoForm.VideoFPS = fps;
                _videoForm.MulticastAddress = profile.VideoEncoderConfiguration.Multicast.Address.IPv4Address;
                _videoForm.MulticastRtpPortVideo = profile.VideoEncoderConfiguration.Multicast.Port;
                _videoForm.MulticastTTL = profile.VideoEncoderConfiguration.Multicast.TTL;
                _videoForm.VideoCodecName = "JPEG";
                _videoForm.EventSink = this;
                _videoForm.OpenWindow(false);
                VideoIsOpened = true;

                Sleep(_operationDelay);

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