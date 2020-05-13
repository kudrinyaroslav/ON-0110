﻿using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class ReplayServicePlaybackAudioTestSuite : ReplayServicePlaybackTestSuite
    {
        public ReplayServicePlaybackAudioTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Replay\\Playback Control\\Audio Streaming";

        #region Playback Tests

        [Test(Name = "PLAYBACK AUDIO STREAMING – MEDIA CONTROL",
                Order = "03.02.01",
                Id = "3-2-1",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.AudioRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAudioStreamingControlMessagesTest()
        {
            RunTest(() =>
            {
                RecordingInformation recInfo = GetRecordingParameters();

                AdjustVideo(recInfo.RecordingToken, TransportProtocol.UDP);

                _handleLogMessage = (message) =>
                {
                    if (HadleMessagePacket(message))
                    {
                        return true;
                    }
                    return false;
                };

                _videoForm.OPTIONS = true;

                ReplayAudioSequence(
                    REQUIRE_ONVIF_REPLAY + CRLF,
                    MakeFieldsSet(new string[] { REQUIRE_ONVIF_REPLAY, RangeClock(recInfo.EarliestRecording) }),
                    "",
                    (actionPlay, actionPause, actionTeardown) =>
                    {
                        actionPlay();
                    });
            },
            () =>
            {
                Cleanup();
            });
        }

        [ Test( Name = "PLAYBACK AUDIO STREAMING – RTP-Unicast/UDP",
                Order = "03.02.02",
                Id = "3-2-2",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.AudioRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAudioStreamingRtpUnicastUdpTest()
        {
            SimpleAudioPlaybackTest(false, TransportProtocol.UDP);
        }

        [ Test( Name = "PLAYBACK AUDIO STREAMING – RTP-Unicast/RTSP/HTTP/TCP",
                Order = "03.02.03",
                Id = "3-2-3",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.AudioRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAudioStreamingRtpUnicastHttpTest()
        {
            SimpleAudioPlaybackTest(false, TransportProtocol.HTTP);
        }

        [ Test( Name = "PLAYBACK AUDIO STREAMING – RTP/RTSP/TCP",
                Order = "03.02.04",
                Id = "3-2-4",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.AudioRecording, Feature.ReplayServiceRTPRTSPTCP },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAudioStreamingRtpRtspTcpTest()
        {
            SimpleAudioPlaybackTest(false, TransportProtocol.RTSP);
        }

        /*[ Test( Name = "REVERSE PLAYBACK AUDIO STREAMING – RTP-Unicast/UDP",
                Order = "03.02.05",
                Id = "3-2-5",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay, Feature.AudioRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay } ) ]*/
        public void ReversePlayabckAudioStreamingRtpUnicastUdpTest()
        {
            SimpleAudioPlaybackTest(true, TransportProtocol.UDP);
        }

        /*[ Test( Name = "REVERSE PLAYBACK AUDIO STREAMING – RTP-Unicast/RTSP/HTTP/TCP",
                Order = "03.02.06",
                Id = "3-2-6",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay, Feature.AudioRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay } ) ]*/
        public void ReversePlayabckAudioStreamingRtpUnicastHttpTest()
        {
            SimpleAudioPlaybackTest(true, TransportProtocol.HTTP);
        }

        /*[ Test( Name = "REVERSE PLAYBACK AUDIO STREAMING – RTP/RTSP/TCP",
                Order = "03.02.07",
                Id = "3-2-7",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay, Feature.AudioRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay } ) ]*/
        public void ReversePlayabckAudioStreamingRtpRtspTcpTest()
        {
            SimpleAudioPlaybackTest(true, TransportProtocol.RTSP);
        }

        #endregion

        #region Pause Tests

        [ Test( Name = "PLAYBACK AUDIO STREAMING – PAUSE WITHOUT RANGE",
                Order = "03.02.14",
                Id = "3-2-14",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.AudioRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAudioStreamingPauseNoRangeTest()
        {
            ReplayPauseTest(false, false, true, false);
        }

        [ Test( Name = "PLAYBACK AUDIO STREAMING – PAUSE WITH RANGE",
                Order = "03.02.15",
                Id = "3-2-15",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.AudioRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAudioStreamingPauseWithRangeTest()
        {
            ReplayPauseTest(true, false, true, false);
        }

        #endregion

        [Test(Name = "PLAYBACK AUDIO STREAMING – PLAY WITH RANGE",
                Order = "03.02.10",
                Id = "3-2-10",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.AudioRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAudioStreamingStopOfPlayingTest()
        {
            ReplayStopOfPlayingTest(false, true, false);
        }
        
        [ Test( Name = "PLAYBACK AUDIO STREAMING – RATECONTROL",
                Order = "03.02.11",
                Id = "3-2-11",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.AudioRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAudioStreamingRateControlTest()
        {
            ReplayRateControlTest(false, true, false);
        }

        [ Test( Name = "PLAYBACK AUDIO STREAMING – IMMEDIATE HEADER",
                Order = "03.02.12",
                Id = "3-2-12",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.AudioRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAudioStreamingImmediateHeaderTest()
        {
            ReplayImmediateHeaderTest(false, true, false);
        }

        [ Test( Name = "PLAYBACK AUDIO STREAMING – SEEK",
                Order = "03.02.13",
                Id = "3-2-13",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.AudioRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAudioStreamingSeekTest()
        {
            ReplaySeekTest(false, true, false);
        }
    }
}