using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class ReplayServicePlaybackAllTestSuite : ReplayServicePlaybackTestSuite
    {
        public ReplayServicePlaybackAllTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Replay\\Playback Control\\Video, Audio and Metadata Streaming";

        #region Playback Tests

        [Test(Name = "PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – MEDIA CONTROL",
                Order = "03.04.01",
                Id = "3-4-1",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAllStreamingControlMessagesTest()
        {
#if true
          bool metadataSupported = Features.ContainsFeature(Feature.MetadataRecording);
          bool audioSupported = Features.ContainsFeature(Feature.AudioRecording);
          MediaPlaybackTest2(TransportProtocol.UDP, true, audioSupported, metadataSupported);
#else
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

                ReplayAllSequence(
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
#endif
        }

        [ Test( Name = "PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – RTP-Unicast/UDP",
                Order = "03.04.02",
                Id = "3-4-2",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAllStreamingRtpUnicastUdpTest()
        {
          SimpleAllPlaybackTest2(false, TransportProtocol.UDP);
        }

        [ Test( Name = "PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – RTP-Unicast/RTSP/HTTP/TCP",
                Order = "03.04.03",
                Id = "3-4-3",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAllStreamingRtpUnicastHttpTest()
        {
            SimpleAllPlaybackTest2(false, TransportProtocol.HTTP);
        }

        [ Test( Name = "PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – RTP/RTSP/TCP",
                Order = "03.04.04",
                Id = "3-4-4",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReplayServiceRTPRTSPTCP },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAllStreamingRtpRtspTcpTest()
        {
            SimpleAllPlaybackTest2(false, TransportProtocol.RTSP);
        }

        /*[ Test( Name = "REVERSE PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – RTP-Unicast/UDP",
                Order = "03.04.05",
                Id = "3-4-5",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay },
                FunctionalityUnderTest = new Functionality[] { } ) ]*/
        public void ReversePlayabckAllStreamingRtpUnicastUdpTest()
        {
            SimpleAllPlaybackTest2(false, TransportProtocol.UDP);
        }

        /*[ Test( Name = "REVERSE PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – RTP-Unicast/RTSP/HTTP/TCP",
                Order = "03.04.06",
                Id = "3-4-6",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay },
                FunctionalityUnderTest = new Functionality[] { } ) ]*/
        public void ReversePlayabckAllStreamingRtpUnicastHttpTest()
        {
            SimpleAllPlaybackTest2(false, TransportProtocol.HTTP);
        }

        /*[ Test( Name = "REVERSE PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – RTP/RTSP/TCP",
                Order = "03.04.07",
                Id = "3-4-7",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay },
                FunctionalityUnderTest = new Functionality[] { } ) ]*/
        public void ReversePlayabckAllStreamingRtpRtspTcpTest()
        {
            SimpleAllPlaybackTest2(false, TransportProtocol.RTSP);
        }

        #endregion

        #region Pause Tests

        [ Test( Name = "PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – PAUSE WITHOUT RANGE",
                Order = "03.04.14",
                Id = "3-4-14",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAllStreamingPauseNoRangeTest()
        {
            bool metadataSupported = Features.ContainsFeature(Feature.MetadataRecording);
            bool audioSupported = Features.ContainsFeature(Feature.AudioRecording);

            ReplayPauseTest2(false, true, audioSupported, metadataSupported);
        }

        [ Test( Name = "PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – PAUSE WITH RANGE",
                Order = "03.04.15",
                Id = "3-4-15",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAllStreamingPauseWithRangeTest()
        {
            bool metadataSupported = Features.ContainsFeature(Feature.MetadataRecording);
            bool audioSupported = Features.ContainsFeature(Feature.AudioRecording);

            ReplayPauseTest2(true, true, audioSupported, metadataSupported);
        }

        #endregion

        [Test(Name = "PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – PLAY WITH RANGE",
                Order = "03.04.10",
                Id = "3-4-10",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAllStreamingStopOfPlayingTest()
        {
            bool metadataSupported = Features.ContainsFeature(Feature.MetadataRecording);
            bool audioSupported = Features.ContainsFeature(Feature.AudioRecording);

            ReplayStopOfPlayingTest2(true, audioSupported, metadataSupported);
        }
        
        [ Test( Name = "PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – RATECONTROL",
                Order = "03.04.11",
                Id = "3-4-11",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAllStreamingRateControlTest()
        {
            bool metadataSupported = Features.ContainsFeature(Feature.MetadataRecording);
            bool audioSupported = Features.ContainsFeature(Feature.AudioRecording);

            ReplayRateControlTest2(true, audioSupported, metadataSupported);
        }

        [ Test( Name = "PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – IMMEDIATE HEADER",
                Order = "03.04.12",
                Id = "3-4-12",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAllStreamingImmediateHeaderTest()
        {
            bool metadataSupported = Features.ContainsFeature(Feature.MetadataRecording);
            bool audioSupported = Features.ContainsFeature(Feature.AudioRecording);

            ReplayImmediateHeaderTest2(true, audioSupported, metadataSupported);
        }

        [ Test( Name = "PLAYBACK VIDEO, AUDIO AND METADATA STREAMING – SEEK",
                Id = "3-4-16",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckAllStreamingSeekTest()
        {
            bool metadataSupported = Features.ContainsFeature(Feature.MetadataRecording);
            bool audioSupported = Features.ContainsFeature(Feature.AudioRecording);

            ReplaySeekTest2(true, audioSupported, metadataSupported);
        }

    }
}
