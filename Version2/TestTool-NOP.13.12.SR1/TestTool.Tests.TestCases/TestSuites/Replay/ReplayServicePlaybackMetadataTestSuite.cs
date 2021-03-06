﻿using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class ReplayServicePlaybackMetadataTestSuite : ReplayServicePlaybackTestSuite
    {
        public ReplayServicePlaybackMetadataTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Replay\\Playback Control\\Metadata Streaming";

        #region Playback Tests

        [Test(Name = "PLAYBACK METADATA STREAMING – MEDIA CONTROL",
                Order = "03.03.01",
                Id = "3-3-1",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.MetadataRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckMetadataStreamingControlMessagesTest()
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

                ReplayMetadataSequence(
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

        [ Test( Name = "PLAYBACK METADATA STREAMING – RTP-Unicast/UDP",
                Order = "03.03.02",
                Id = "3-3-2",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.MetadataRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckMetadataStreamingRtpUnicastUdpTest()
        {
            SimpleMetaPlaybackTest(false, TransportProtocol.UDP);
        }

        [ Test( Name = "PLAYBACK METADATA STREAMING – RTP-Unicast/RTSP/HTTP/TCP",
                Order = "03.03.03",
                Id = "3-3-3",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.MetadataRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckMetadataStreamingRtpUnicastHttpTest()
        {
            SimpleMetaPlaybackTest(false, TransportProtocol.HTTP);
        }

        [ Test( Name = "PLAYBACK METADATA STREAMING – RTP/RTSP/TCP",
                Order = "03.03.04",
                Id = "3-3-4",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.MetadataRecording, Feature.ReplayServiceRTPRTSPTCP },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckMetadataStreamingRtpRtspTcpTest()
        {
            SimpleMetaPlaybackTest(false, TransportProtocol.RTSP);
        }

        [ Test( Name = "REVERSE PLAYBACK METADATA STREAMING – RTP-Unicast/UDP",
                Order = "03.03.05",
                Id = "3-3-5",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay, Feature.MetadataRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.ReverseReplay, Functionality.MediaReplay })]
        public void ReversePlayabckMetadataStreamingRtpUnicastUdpTest()
        {
            SimpleMetaPlaybackTest(true, TransportProtocol.UDP);
        }

        [ Test( Name = "REVERSE PLAYBACK METADATA STREAMING – RTP-Unicast/RTSP/HTTP/TCP",
                Order = "03.03.06",
                Id = "3-3-6",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay, Feature.MetadataRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.ReverseReplay, Functionality.MediaReplay })]
        public void ReversePlayabckMetadataStreamingRtpUnicastHttpTest()
        {
            SimpleMetaPlaybackTest(true, TransportProtocol.HTTP);
        }

        [ Test( Name = "REVERSE PLAYBACK METADATA STREAMING – RTP/RTSP/TCP",
                Order = "03.03.07",
                Id = "3-3-7",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay, Feature.MetadataRecording, Feature.ReplayServiceRTPRTSPTCP },
                FunctionalityUnderTest = new Functionality[] { Functionality.ReverseReplay, Functionality.MediaReplay })]
        public void ReversePlayabckMetadataStreamingRtpRtspTcpTest()
        {
            SimpleMetaPlaybackTest(true, TransportProtocol.RTSP);
        }

        #endregion

        #region Pause Tests

        [ Test( Name = "PLAYBACK METADATA STREAMING – PAUSE WITHOUT RANGE",
                Order = "03.03.14",
                Id = "3-3-14",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.MetadataRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckMetadataStreamingPauseNoRangeTest()
        {
            ReplayPauseTest(false, false, false, true);
        }

        [ Test( Name = "PLAYBACK METADATA STREAMING – PAUSE WITH RANGE",
                Order = "03.03.15",
                Id = "3-3-15",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.MetadataRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckMetadataStreamingPauseWithRangeTest()
        {
            ReplayPauseTest(true, false, false, true);
        }

        #endregion

        [Test(Name = "PLAYBACK METADATA STREAMING – PLAY WITH RANGE",
                Order = "03.03.10",
                Id = "3-3-10",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.MetadataRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckMetadataStreamingStopOfPlayingTest()
        {
            ReplayStopOfPlayingTest(false, false, true);
        }
        
        [ Test( Name = "PLAYBACK METADATA STREAMING – RATECONTROL",
                Order = "03.03.11",
                Id = "3-3-11",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.MetadataRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckMetadataStreamingRateControlTest()
        {
            ReplayRateControlTest(false, false, true);
        }

        [ Test( Name = "PLAYBACK METADATA STREAMING – IMMEDIATE HEADER",
                Order = "03.03.12",
                Id = "3-3-12",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.MetadataRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckMetadataStreamingImmediateHeaderTest()
        {
            ReplayImmediateHeaderTest(false, false, true);
        }

        [ Test( Name = "PLAYBACK METADATA STREAMING – SEEK",
                Order = "03.03.13",
                Id = "3-3-13",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.MetadataRecording },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckMetadataStreamingSeekTest()
        {
            ReplaySeekTest(false, false, true);
        }
    }
}
