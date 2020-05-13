///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.TestCases.Utils;
using System.ServiceModel;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class RtspAudioVideoTestSuite : RtspTestSuite
    {
        private const string PATH_U = "Real Time Streaming\\Audio & Video Streaming\\Unicast";

        private const string PATH_M = "Real Time Streaming\\Audio & Video Streaming\\Multicast";

        NetworkConfiguration NetConfig;
        
        public RtspAudioVideoTestSuite(TestLaunchParam param)
            : base(param)
        {
            NetConfig = new NetworkConfiguration(param, this);
        }

        /// <summary>
        /// Ends the test.
        /// </summary>
        /// <param name="status"></param>
        public override void EndTest(TestStatus status)
        {
            base.EndTest(status);
            NetConfig.Release(this);
        }

        public void ReConnectTo(string ServiceAddress)
        {
            _cameraAddress = ServiceAddress;
            _endpointController.UpdateAddress(new EndpointAddress(ServiceAddress));
        }

        #region Unicast

        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/UDP)",
            Path = PATH_U,
            Order = "03.01.10",
            Id = "3-1-10",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG711RtpUnicastUdp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(()=>
            {
                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }
        
        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH_U,
            Order = "03.01.11",
            Id = "3-1-11",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG711RtpUnicastRtspHttpTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP/RTSP/TCP)",
            Path = PATH_U,
            Order = "03.01.12",
            Id = "3-1-12",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPRTSPTCP },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG711RtpRtspTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/UDP)",
            Path = PATH_U,
            Order = "03.01.13",
            Id = "3-1-13",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG726RTPUnicastUdp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH_U,
            Order = "03.01.14",
            Id = "3-1-14",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG726RtpUnicastRtspHttpTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }


        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP/RTSP/TCP)",
            Path = PATH_U,
            Order = "03.01.15",
            Id = "3-1-15",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726, Feature.RTPRTSPTCP },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG726RtpRtspTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
                {
                    MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                    ValidateAudioVideoSequence();
                },
                () =>
                {
                    VideoCleanup();
                    RestoreMediaConfiguration(changeLog);
                }
            );
        }
        
        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Unicast/UDP)",
            Path = PATH_U,
            Order = "03.01.16",
            Id = "3-1-16",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegAacRtpUnicastUdp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegAacMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH_U,
            Order = "03.01.17",
            Id = "3-1-17",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegAacRtpUnicastRtspHttpTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegAacMediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP/RTSP/TCP)",
            Path = PATH_U,
            Order = "03.01.18",
            Id = "3-1-18",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC, Feature.RTPRTSPTCP },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegAacRtpRtspTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegAacMediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }
        
        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/UDP, IPv6)",
	        Path = PATH_U,
	        Order = "03.01.19",
	        Id = "3-1-19",
	        Category = Category.RTSS,
	        Version = 1.0,
	        RequirementLevel = RequirementLevel.Optional,
	        RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.IPv6 },
	        FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void JpegG711StreamingRtpUnicastUdpIPv6()
        {
	        MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

	        RunTest(()=>
	        {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

		        MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
		        ValidateAudioVideoSequence();
	        },
	        () =>
	        {
		        VideoCleanup();
		        RestoreMediaConfiguration(changeLog);

                // A.24 - network settings restore
                if (!string.IsNullOrEmpty(newServiceAddr) &&
                    !string.IsNullOrEmpty(oldServiceAddr))
                {
                    NetConfig.RestoreNetworkSettings(oldServiceAddr, newServiceAddr, restoreEnabled, netInterface.token);
                    //ReConnectTo(oldServiceAddr);
                    //base.Initialize();
                }
	        }
	        );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Unicast/RTSP/HTTP/TCP, IPv6)",
            Path = PATH_U,
            Order = "03.01.20",
            Id = "3-1-20",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void JpegG711StreamingRtpUnicastRtspHttpTcpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(() =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);

                // A.24 - network settings restore
                if (!string.IsNullOrEmpty(newServiceAddr) &&
                    !string.IsNullOrEmpty(oldServiceAddr))
                {
                    NetConfig.RestoreNetworkSettings(oldServiceAddr, newServiceAddr, restoreEnabled, netInterface.token);
                    //ReConnectTo(oldServiceAddr);
                    //base.Initialize();
                }
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.711 (RTP/RTSP/TCP, IPv6)",
            Path = PATH_U,
            Order = "03.01.21",
            Id = "3-1-21",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPRTSPTCP, Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void JpegG711StreamingRtpRtspTcpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(() =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);

                // A.24 - network settings restore
                if (!string.IsNullOrEmpty(newServiceAddr) &&
                    !string.IsNullOrEmpty(oldServiceAddr))
                {
                    NetConfig.RestoreNetworkSettings(oldServiceAddr, newServiceAddr, restoreEnabled, netInterface.token);
                    //ReConnectTo(oldServiceAddr);
                    //base.Initialize();
                }
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/UDP, IPv6)",
            Path = PATH_U,
            Order = "03.01.22",
            Id = "3-1-22",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726, Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void JpegG726StreamingRTPUnicastUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(() =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);

                // A.24 - network settings restore
                if (!string.IsNullOrEmpty(newServiceAddr) &&
                    !string.IsNullOrEmpty(oldServiceAddr))
                {
                    NetConfig.RestoreNetworkSettings(oldServiceAddr, newServiceAddr, restoreEnabled, netInterface.token);
                    //ReConnectTo(oldServiceAddr);
                    //base.Initialize();
                }
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/RTSP/HTTP/TCP, IPv6)",
            Path = PATH_U,
            Order = "03.01.23",
            Id = "3-1-23",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726, Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void JpegG726StreamingRtpUnicastRtspHttpTcpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(() =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);

                // A.24 - network settings restore
                if (!string.IsNullOrEmpty(newServiceAddr) &&
                    !string.IsNullOrEmpty(oldServiceAddr))
                {
                    NetConfig.RestoreNetworkSettings(oldServiceAddr, newServiceAddr, restoreEnabled, netInterface.token);
                    //ReConnectTo(oldServiceAddr);
                    //base.Initialize();
                }
            }
            );
        }


        [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP/RTSP/TCP, IPv6)",
            Path = PATH_U,
            Order = "03.01.24",
            Id = "3-1-24",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726, Feature.RTPRTSPTCP, Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void JpegG726StreamingRtpRtspTcpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(() =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateAudioVideoSequence();
            },
                () =>
                {
                    VideoCleanup();
                    RestoreMediaConfiguration(changeLog);

                    // A.24 - network settings restore
                    if (!string.IsNullOrEmpty(newServiceAddr) &&
                        !string.IsNullOrEmpty(oldServiceAddr))
                    {
                        NetConfig.RestoreNetworkSettings(oldServiceAddr, newServiceAddr, restoreEnabled, netInterface.token);
                        //ReConnectTo(oldServiceAddr);
                        //base.Initialize();
                    }
                }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Unicast/UDP, IPv6)",
            Path = PATH_U,
            Order = "03.01.25",
            Id = "3-1-25",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC, Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void JpegAacStreamingRtpUnicastUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(() =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetJpegAacMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);

                // A.24 - network settings restore
                if (!string.IsNullOrEmpty(newServiceAddr) &&
                    !string.IsNullOrEmpty(oldServiceAddr))
                {
                    NetConfig.RestoreNetworkSettings(oldServiceAddr, newServiceAddr, restoreEnabled, netInterface.token);
                    //ReConnectTo(oldServiceAddr);
                    //base.Initialize();
                }
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Unicast/RTSP/HTTP/TCP, IPv6)",
            Path = PATH_U,
            Order = "03.01.26",
            Id = "3-1-26",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC, Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void JpegAacStreamingRtpUnicastRtspHttpTcpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(() =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetJpegAacMediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);

                // A.24 - network settings restore
                if (!string.IsNullOrEmpty(newServiceAddr) &&
                    !string.IsNullOrEmpty(oldServiceAddr))
                {
                    NetConfig.RestoreNetworkSettings(oldServiceAddr, newServiceAddr, restoreEnabled, netInterface.token);
                    //ReConnectTo(oldServiceAddr);
                    //base.Initialize();
                }
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP/RTSP/TCP, IPv6)",
            Path = PATH_U,
            Order = "03.01.27",
            Id = "3-1-27",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC, Feature.RTPRTSPTCP, Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void JpegAacStreamingRtpRtspTcpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(() =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetJpegAacMediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);

                // A.24 - network settings restore
                if (!string.IsNullOrEmpty(newServiceAddr) &&
                    !string.IsNullOrEmpty(oldServiceAddr))
                {
                    NetConfig.RestoreNetworkSettings(oldServiceAddr, newServiceAddr, restoreEnabled, netInterface.token);
                    //ReConnectTo(oldServiceAddr);
                    //base.Initialize();
                }
            }
            );
        }

        #endregion

        #region Multicast

        
        [ Test( Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Multicast/UDP, IPv4)",
                Path = PATH_M,
                Order = "03.02.016",
                Id = "3-2-16",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegG711RtpMulticastUdpIPv4()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv4, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – JPEG/G.711 (RTP-Multicast/UDP, IPv6)",
                Path = PATH_M,
                Order = "03.02.17",
                Id = "3-2-17",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.IPv6, Feature.RTPMulticastUDP },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingJpegG711RtpMulticastUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv6, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Multicast/UDP, IPv4)",
                Path = PATH_M,
                Order = "03.02.18",
                Id = "3-2-18",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726, Feature.RTPMulticastUDP },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingJpegG726RtpMulticastUdpIPv4()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv4, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Multicast/UDP, IPv6)",
                Path = PATH_M,
                Order = "03.02.19",
                Id = "3-2-19",
                Category = Category.RTSS,
                Version = 2.0,
                Interactive = true,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726, Feature.IPv6, Feature.RTPMulticastUDP },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingJpegG726RtpMulticastUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv6, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        
        [ Test( Name = "MEDIA STREAMING – JPEG/AAC (RTP-Multicast/UDP, IPv4)",
                Path = PATH_M,
                Order = "03.02.20",
                Id = "3-2-20",
                Category = Category.RTSS,
                Version = 2.0,
                Interactive = true,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC, Feature.RTPMulticastUDP },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingJpegAACRtpMulticastUdpIPv4()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegAACMediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv4, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – JPEG/AAC (RTP-Multicast/UDP, IPv6)",
                Path = PATH_M,
                Order = "03.02.21",
                Id = "3-2-21",
                Category = Category.RTSS,
                Version = 2.0,
                Interactive = true,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC, Feature.IPv6, Feature.RTPMulticastUDP },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingJpegAACRtpMulticastUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegAACMediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv6, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
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
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures =  new Feature[] { Feature.MediaService, Feature.Audio,Feature.RTPTCP})]
 */
        public void NvtMediaStreamingJpegG711RtpUnicastTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG711MediaUri(StreamType.RTPUnicast, TransportProtocol.TCP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        /* postponed to Phase 2
                [Test(Name = "MEDIA STREAMING – JPEG/G.726 (RTP-Unicast/TCP)",
                    Path = PATH,
                    Order = "08.02.06",
                    Version = 1.02,
                    Interactive = true,
                    RequirementLevel = RequirementLevel.Must,
                    RequiredFeatures =  new Feature[]{ Feature.MediaService, Feature.Audio,Feature.RTPTCP})]
         */
        public void NvtMediaStreamingJpegG726RtpTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegG726MediaUri(StreamType.RTPUnicast, TransportProtocol.TCP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        /* postponed to Phase 2
                [Test(Name = "MEDIA STREAMING – JPEG/AAC (RTP-Unicast/TCP)",
                    Path = PATH,
                    Order = "08.02.10",
                    Version = 1.02,
                    Interactive = true,
                    RequirementLevel = RequirementLevel.Must,
                    RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio, Feature.AAC, Feature.RTPTCP})]
         */
        public void NvtMediaStreamingJpegAacRtpTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegAacMediaUri(StreamType.RTPUnicast, TransportProtocol.TCP, changeLog);
                ValidateAudioVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }
    }
}
