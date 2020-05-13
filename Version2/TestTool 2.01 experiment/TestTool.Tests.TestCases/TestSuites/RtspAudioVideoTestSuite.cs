///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Enums;
using TestTool.Proxies.Media;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class RtspAudioVideoTestSuite : RtspTestSuite
    {
        private const string PATH = "Real Time Streaming\\Audio & Video Streaming";

        public RtspAudioVideoTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/ UDP)",
            Path = PATH,
            Order = "08.02.01",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures =  new Feature[]{Feature.Audio})]
        public void NvtMediaStreamingJpegG711RtpUnicastUdp()
        {
            RunTest(()=>
            {
                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );

        }

/* postponed to Phase 2
        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/ TCP)",
            Path = PATH,
            Order = "08.02.02",
            Version = 1.02,
            Interactive = true,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures =  new Feature[] { Feature.Audio,Feature.RTPTCP})]
 */ 
        public void NvtMediaStreamingJpegG711RtpUnicastTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPUnicast, TransportProtocol.TCP);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH,
            Order = "08.02.03",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio })]
        public void NvtMediaStreamingJpegG711RtpUnicastRtspHttpTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }


        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP/RTSP/TCP)",
            Path = PATH,
            Order = "08.02.04",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio, Feature.RTPRTSPTCP})]
        public void NvtMediaStreamingJpegG711RtpRtspTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }


//#if FULL
        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/UDP)",
            Path = PATH,
            Order = "08.02.05",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[]{Feature.Audio,Feature.G726})]
//#endif
        public void NvtMediaStreamingJpegG726RTPUnicastUdp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }


//#if FULL
/* postponed to Phase 2
        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/TCP)",
            Path = PATH,
            Order = "08.02.06",
            Version = 1.02,
            Interactive = true,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures =  new Feature[]{Feature.Audio,Feature.RTPTCP})]
 */ 
//#endif
        public void NvtMediaStreamingJpegG726RtpTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPUnicast, TransportProtocol.TCP);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }


//#if FULL
        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH,
            Order = "08.02.07",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] {Feature.Audio,Feature.G726})]
//#endif
        public void NvtMediaStreamingJpegG726RtpUnicastRtspHttpTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

//#if FULL
        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP/RTSP/TCP)",
            Path = PATH,
            Order = "08.02.08",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures =  new Feature[]{Feature.Audio,Feature.G726,Feature.RTPRTSPTCP})]
//#endif
        public void NvtMediaStreamingJpegG726RtpRtspTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }


        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Unicast/UDP)",
            Path = PATH,
            Order = "08.02.09",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures =  new Feature[]{Feature.Audio,Feature.AAC})]
        public void NvtMediaStreamingJpegAacRtpUnicastUdp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegAacMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }


/* postponed to Phase 2
        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Unicast/TCP)",
            Path = PATH,
            Order = "08.02.10",
            Version = 1.02,
            Interactive = true,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] {Feature.Audio, Feature.AAC, Feature.RTPTCP})]
 */ 
        public void NvtMediaStreamingJpegAacRtpTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegAacMediaUri(StreamType.RTPUnicast, TransportProtocol.TCP);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }


        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH,
            Order = "08.02.11",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] {Feature.Audio, Feature.AAC})]
        public void NvtMediaStreamingJpegAacRtpUnicastRtspHttpTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegAacMediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP/RTSP/TCP)",
            Path = PATH,
            Order = "08.02.12",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] {Feature.Audio, Feature.AAC, Feature.RTPRTSPTCP})]
        public void NvtMediaStreamingJpegAacRtpRtspTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegAacMediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }
    }
}
