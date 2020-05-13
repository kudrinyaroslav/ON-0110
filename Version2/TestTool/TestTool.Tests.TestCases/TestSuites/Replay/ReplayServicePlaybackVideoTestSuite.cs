using System;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Enums;
using DateTime=System.DateTime;
using TestTool.Tests.Common.Media;
using TestTool.Tests.Common.Transport;
using System.ServiceModel;
using System.ServiceModel.Channels;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Interfaces;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class ReplayServicePlaybackVideoTestSuite : ReplayServicePlaybackTestSuite
    {
        public ReplayServicePlaybackVideoTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Replay\\Playback Control\\Video Streaming";

        #region Playback Tests

        [Test(Name = "PLAYBACK VIDEO STREAMING - MEDIA CONTROL",
                Order = "03.01.01",
                Id = "3-1-1",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay } ) ]
        public void PlayabckVideoStreamingControlMessagesTest()
        {
#if true
#if true
          MediaPlaybackTest2(TransportProtocol.UDP, true, false, false);
#else
          RunTest(() =>
          {
            RecordingInformation recInfo = GetRecordingParameters();

            AdjustGeneral2(recInfo.RecordingToken, TransportProtocol.UDP);

            _handleLogMessage = (message) =>
            {
              if (HadleMessagePacket(message))
              {
                return true;
              }
              return false;
            };

            //_videoForm.OPTIONS = true;

            ReplaySequence2(
                REQUIRE_ONVIF_REPLAY + CRLF,
                MakeFieldsSet(new string[] { REQUIRE_ONVIF_REPLAY, RangeClock(recInfo.EarliestRecording) }),
                "",
                (actionPlay, actionPause, actionTeardown) =>
                {
                  actionPlay();
                },
                true, false, false);
          },
          () =>
          {
            Cleanup2();
          });
#endif
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

                ReplayVideoSequence(
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

        [ Test( Name = "PLAYBACK VIDEO STREAMING – RTP-Unicast/UDP",
                Order = "03.01.02",
                Id = "3-1-2",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckVideoStreamingRtpUnicastUdpTest()
        {
          SimpleVideoPlaybackTest2(false, TransportProtocol.UDP);
        }

        [ Test( Name = "PLAYBACK VIDEO STREAMING – RTP-Unicast/RTSP/HTTP/TCP",
                Order = "03.01.03",
                Id = "3-1-3",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckVideoStreamingRtpUnicastHttpTest()
        {
          SimpleVideoPlaybackTest2(false, TransportProtocol.HTTP);
        }

        [ Test( Name = "PLAYBACK VIDEO STREAMING – RTP/RTSP/TCP",
                Order = "03.01.04",
                Id = "3-1-4",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReplayServiceRTPRTSPTCP },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckVideoStreamingRtpRtspTcpTest()
        {
          SimpleVideoPlaybackTest2(false, TransportProtocol.RTSP);
        }

        [ Test( Name = "REVERSE PLAYBACK VIDEO STREAMING – RTP-Unicast/UDP",
                Order = "03.01.05",
                Id = "3-1-5",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay, Functionality.ReverseReplay })]
        public void ReversePlayabckVideoStreamingRtpUnicastUdpTest()
        {
          SimpleVideoPlaybackTest2(true, TransportProtocol.UDP);
        }

        [ Test( Name = "REVERSE PLAYBACK VIDEO STREAMING – RTP-Unicast/RTSP/HTTP/TCP",
                Order = "03.01.06",
                Id = "3-1-6",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay, Functionality.ReverseReplay })]
        public void ReversePlayabckVideoStreamingRtpUnicastHttpTest()
        {
          SimpleVideoPlaybackTest2(true, TransportProtocol.HTTP);
        }

        [ Test( Name = "REVERSE PLAYBACK VIDEO STREAMING – RTP/RTSP/TCP",
                Order = "03.01.07",
                Id = "3-1-7",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService, Feature.ReverseReplay, Feature.ReplayServiceRTPRTSPTCP },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay, Functionality.ReverseReplay })]
        public void Reverse()
        {
          SimpleVideoPlaybackTest2(true, TransportProtocol.RTSP);
        }

        #endregion

        #region Pause Tests

        [ Test( Name = "PLAYBACK VIDEO STREAMING – PAUSE WITHOUT RANGE",
                Order = "03.01.15",
                Id = "3-1-15",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckVideoStreamingPauseNoRangeTest()
        {
            ReplayPauseTest2(false, true, false, false);
        }

        [ Test( Name = "PLAYBACK VIDEO STREAMING – PAUSE WITH RANGE",
                Order = "03.01.16",
                Id = "3-1-16",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckVideoStreamingPauseWithRangeTest()
        {
            ReplayPauseTest2(true, true, false, false);
        }

        #endregion

        [Test(Name = "PLAYBACK VIDEO STREAMING – PLAY WITH RANGE",
                Order = "03.01.10",
                Id = "3-1-10",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckVideoStreamingStopOfPlayingTest()
        {
            ReplayStopOfPlayingTest2(true, false, false);
        }

        [Test(Name = "PLAYBACK VIDEO STREAMING - I-FRAMES",
                Order = "03.01.11",
                Id = "3-1-11",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckVideoStreamingIFramesTest()
        {
            ReplayIFramesTest2(true, false, false);
        }
        
        [ Test( Name = "PLAYBACK VIDEO STREAMING – RATECONTROL",
                Order = "03.01.12",
                Id = "3-1-12",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckVideoStreamingRateControlTest()
        {
            ReplayRateControlTest2(true, false, false);
        }

        [ Test( Name = "PLAYBACK VIDEO STREAMING – IMMEDIATE HEADER",
                Order = "03.01.13",
                Id = "3-1-13",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckVideoStreamingImmediateHeaderTest()
        {
            ReplayImmediateHeaderTest2(true, false, false);
        }

        [ Test( Name = "PLAYBACK VIDEO STREAMING – SEEK",
                Id = "3-1-17",
                Category = Category.REPLAY,
                Path = PATH,
                Version = 2.1,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.ReplayService },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaReplay })]
        public void PlayabckVideoStreamingSeekTest()
        {
            ReplaySeekTest2(true, false, false);
        }
    }
}
