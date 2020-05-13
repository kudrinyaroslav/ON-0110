///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Enums;
using TestTool.Proxies.Onvif;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class RtspAudioVideoTestSuite : RtspTestSuite
    {
        private const string PATH_U = "Real Time Streaming\\Audio & Video Streaming\\Unicast";

        private const string PATH_M = "Real Time Streaming\\Audio & Video Streaming\\Multicast";
        
        public RtspAudioVideoTestSuite(TestLaunchParam param)
            : base(param)
        {

        }


        #region Unicast

        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/UDP)",
            Path = PATH_U,
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.RTSS,
            Version = 1.02,
            Interactive = true,
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
        
        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH_U,
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.RTSS,
            Version = 1.02,
            Interactive = true,
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
            Path = PATH_U,
            Order = "03.01.03",
            Id = "3-1-3",
            Category = Category.RTSS,
            Version = 1.02,
            Interactive = true,
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

        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/UDP)",
            Path = PATH_U,
            Order = "03.01.04",
            Id = "3-1-4",
            Category = Category.RTSS,
            Version = 1.02,
            Interactive = true,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio, Feature.G726 })]
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

        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH_U,
            Order = "03.01.05",
            Id = "3-1-5",
            Category = Category.RTSS,
            Version = 1.02,
            Interactive = true,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio, Feature.G726 })]
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


        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP/RTSP/TCP)",
            Path = PATH_U,
            Order = "03.01.06",
            Id = "3-1-6",
            Category = Category.RTSS,
            Version = 1.02,
            Interactive = true,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures =  new Feature[]{Feature.Audio,Feature.G726,Feature.RTPRTSPTCP})]
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
            Path = PATH_U,
            Order = "03.01.07",
            Id = "3-1-7",
            Category = Category.RTSS,
            Version = 1.02,
            Interactive = true,
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

        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH_U,
            Order = "03.01.08",
            Id = "3-1-8",
            Category = Category.RTSS,
            Version = 1.02,
            Interactive = true,
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
            Path = PATH_U,
            Order = "03.01.09",
            Id = "3-1-9",
            Category = Category.RTSS,
            Version = 1.02,
            Interactive = true,
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
        
        #endregion


        #region Multicast
        
        /*[ Test( Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Multicast/UDP, IPv4)",
                Path = PATH,
                Order = "04.01.04",
                Id = "4-1-4",
                Category = Category.RTSS,
                Version = 2.0,
                Interactive = true,
                RequirementLevel = RequirementLevel.ConditionalShould,
                RequiredFeatures = new Feature[] { Feature.Audio } ) ]*/
        public void NvtMediaStreamingJpegG711RtpMulticastUdpIPv4()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv4);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        /*[ Test( Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Multicast/UDP, IPv6)",
                Path = PATH,
                Order = "04.01.05",
                Id = "4-1-5",
                Category = Category.RTSS,
                Version = 2.0,
                Interactive = true,
                RequirementLevel = RequirementLevel.ConditionalShould,
                RequiredFeatures = new Feature[] { Feature.Audio, Feature.IPv6 } ) ]*/
        public void NvtMediaStreamingJpegG711RtpMulticastUdpIPv6()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv6);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        /*[ Test( Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Multicast/UDP, IPv4)",
                Path = PATH,
                Order = "04.01.09",
                Id = "4-1-9",
                Category = Category.RTSS,
                Version = 2.0,
                Interactive = true,
                RequirementLevel = RequirementLevel.ConditionalShould,
                RequiredFeatures = new Feature[] { Feature.Audio, Feature.G726 } ) ]*/
        public void NvtMediaStreamingJpegG726RtpMulticastUdpIPv4()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv4);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        /*[ Test( Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Multicast/UDP, IPv6)",
                Path = PATH,
                Order = "04.01.10",
                Id = "4-1-10",
                Category = Category.RTSS,
                Version = 2.0,
                Interactive = true,
                RequirementLevel = RequirementLevel.ConditionalShould,
                RequiredFeatures = new Feature[] { Feature.Audio, Feature.G726, Feature.IPv6 } )]*/
        public void NvtMediaStreamingJpegG726RtpMulticastUdpIPv6()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv6);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }
        
        /*[ Test( Name = "MEDIA STREAMING – JPEG/AAC (RTP-Multicast/UDP, IPv4)",
                Path = PATH,
                Order = "04.01.14",
                Id = "4-1-14",
                Category = Category.RTSS,
                Version = 2.0,
                Interactive = true,
                RequirementLevel = RequirementLevel.ConditionalShould,
                RequiredFeatures = new Feature[] { Feature.Audio, Feature.AAC } ) ]*/
        public void NvtMediaStreamingJpegAACRtpMulticastUdpIPv4()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegAACMediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv4);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        /*[ Test( Name = "MEDIA STREAMING – JPEG/AAC (RTP-Multicast/UDP, IPv6)",
                Path = PATH,
                Order = "04.01.15",
                Id = "4-1-15",
                Category = Category.RTSS,
                Version = 2.0,
                Interactive = true,
                RequirementLevel = RequirementLevel.ConditionalShould,
                RequiredFeatures = new Feature[] { Feature.Audio, Feature.AAC, Feature.IPv6 } ) ]*/
        public void NvtMediaStreamingJpegAACRtpMulticastUdpIPv6()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegAACMediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv6);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        #endregion
        


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
        
/* postponed to Phase 2
        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/TCP)",
            Path = PATH,
            Order = "08.02.06",
            Version = 1.02,
            Interactive = true,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures =  new Feature[]{Feature.Audio,Feature.RTPTCP})]
 */ 
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
    }
}
