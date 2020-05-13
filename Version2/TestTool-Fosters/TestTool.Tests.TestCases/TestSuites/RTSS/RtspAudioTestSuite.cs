///////////////////////////////////////////////////////////////////////////
//!  @author        Ilya Gozman
////
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class RtspAudioTestSuite : RtspTestSuite
    {
        private const string PATH_U = "Real Time Streaming\\Audio Streaming\\Unicast";

        public RtspAudioTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        [ Test( Name = "MEDIA STREAMING – G.711 (RTP-Unicast/UDP)",
                Path = PATH_U,
                Order = "02.01.01",
                Id = "2-1-1",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
                FunctionalityUnderTest = new Functionality[]{Functionality.GetStreamUri}) ]
        public void NvtMediaStreamingG711RtpUnicastUdp()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetG711MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – G.711 (RTP-Unicast/RTSP/HTTP/TCP)",
                Path = PATH_U,
                Order = "02.01.02",
                Id = "2-1-2",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingG711RtpUnicastRtspHttpTcp()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetG711MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – G.711 (RTP/RTSP/TCP)",
                Path = PATH_U,
                Order = "02.01.03",
                Id = "2-1-3",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.RTPRTSPTCP },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingG711RtpRtspTcp()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetG711MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – AAC (RTP-Unicast/UDP)",
                Path = PATH_U,
                Order = "02.01.07",
                Id = "2-1-7",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.AAC },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingAACRtpUnicastUdp()
        {
            RunTest(
                () =>
                {
                    MediaUri streamUri = GetAACMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                    ValidateAudioSequence();
                },
                () =>
                {
                    VideoCleanup();
                }
            );
        }

        [ Test( Name = "MEDIA STREAMING – AAC (RTP-Unicast/RTSP/HTTP/TCP)",
                Path = PATH_U,
                Order = "02.01.08",
                Id = "2-1-8",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.AAC },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingAACRtpUnicastRtspHttpTcp()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetAACMediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – AAC (RTP/RTSP/TCP)",
                Path = PATH_U,
                Order = "02.01.09",
                Id = "2-1-9",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.RTPRTSPTCP, Feature.AAC },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingAACRtpRtspTcp()
        {
            RunTest(
                () =>
                {
                    MediaUri streamUri = GetAACMediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP);
                    ValidateAudioSequence();
                },
                () =>
                {
                    VideoCleanup();
                }
            );
        }    

        [ Test( Name = "MEDIA STREAMING – G.726 (RTP-Unicast/UDP)",
                Path = PATH_U,
                Order = "02.01.04",
                Id = "2-1-4",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.G726 },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingG726RtpUnicastUdp()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetG726MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – G.726 (RTP-Unicast/RTSP/HTTP/TCP)",
                Path = PATH_U,
                Order = "02.01.05",
                Id = "2-1-5",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.G726 },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingG726RtpUnicastRtspHttpTcp()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetG726MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – G.726 (RTP/RTSP/TCP)",
                Path = PATH_U,
                Order = "02.01.06",
                Id = "2-1-6",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.RTPRTSPTCP, Feature.G726 },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingG726RtpRtspTcp()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetG726MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }
    }
}
