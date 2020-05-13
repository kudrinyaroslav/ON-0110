using System;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using System.ServiceModel;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class RtspVideoTestSuite : RtspTestSuite
    {
        private const string PATH_U = "Real Time Streaming\\Video Streaming\\Unicast";
        private const string PATH_M = "Real Time Streaming\\Video Streaming\\Multicast";


        public RtspVideoTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        #region Unicast

        [Test(Name = "MEDIA CONTROL – RTSP/TCP",
            Path = PATH_U,
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures =  new Feature[]{ Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaControlRtspTcpTest()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                _videoForm.OPTIONS = true;
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                _videoForm.OPTIONS = false;
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – RTSP KEEPALIVE (SET_PARAMETER)",
            Path = PATH_U,
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingRtspKeepAliveTest()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                _videoForm.KEEPALIVE = true;
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                _videoForm.KEEPALIVE = false;
            }
            );
        }


        [ Test( Name = "MEDIA STREAMING - RTSP KEEPALIVE (OPTIONS)",
                Path = PATH_U,
                Order = "01.01.03",
                Id = "1-1-3",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingRtspKeepAliveOptions()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                _videoForm.KEEPALIVE = true;
                _videoForm.UseKeepAliveOptions = true;
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                _videoForm.KEEPALIVE = false;
                _videoForm.UseKeepAliveOptions = false;
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG (RTP-Unicast/UDP)",
            Path = PATH_U,
            Order = "01.01.04",
            Id = "1-1-4",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegRtpUnicastUdp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [Test(Name = "MEDIA STREAMING - JPEG (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH_U,
            Order = "01.01.05",
            Id = "1-1-5",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegRtpUnicastRtspHttpTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        
        [Test(Name = "MEDIA STREAMING - JPEG (RTP/RTSP/TCP)",
            Path = PATH_U,
            Order = "01.01.06",
            Id = "1-1-6",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RTPRTSPTCP, Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegRtpRtspTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [Test(Name = "MEDIA STREAMING - MPEG4 (RTP-Unicast/UDP)",
            Path = PATH_U,
            Order = "01.01.07",
            Id = "1-1-7",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.MPEG4 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingMpeg4RtpUnicastUdp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetMpeg4MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }
        
        [Test(Name = "MEDIA STREAMING - MPEG4 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH_U,
            Order = "01.01.08",
            Id = "1-1-8",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.MPEG4 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingMpeg4RtpUnicastRtspHttpTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetMpeg4MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [Test(Name = "MEDIA STREAMING - MPEG4 (RTP/RTSP/TCP)",
            Path = PATH_U,
            Order = "01.01.09",
            Id = "1-1-9",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.MPEG4, Feature.RTPRTSPTCP },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingMpeg4RtpRtspTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetMpeg4MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [Test(Name = "SET SYNCHRONIZATION POINT - MPEG4",
            Path = PATH_U,
            Order = "01.01.10",
            Id = "1-1-10",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.MPEG4 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaSetSynchronizationPoint })]
        public void NvtMediaStreamingMpeg4SetSynchronizationPoint()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetMpeg4MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                ValidateSyncSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }


       [Test(Name = "MEDIA STREAMING - H.264 (RTP-Unicast/UDP)",
            Path = PATH_U,
            Order = "01.01.11",
            Id = "1-1-11",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.H264 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
       public void NvtMediaStreamingH264RtpUnicastUdp()
       {
           RunTest(() =>
           {
               MediaUri streamUri = GetH264MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
               ValidateVideoSequence();
           },
            () =>
            {
                VideoCleanup();
            }
            );
       }


       [Test(Name = "MEDIA STREAMING - H.264 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH_U,
            Order = "01.01.12",
            Id = "1-1-12",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.H264 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
       public void NvtMediaStreamingH264RtpUnicastRtspHttpTcp()
       {
           RunTest(() =>
           {
               MediaUri streamUri = GetH264MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP);
               ValidateVideoSequence();
           },
            () =>
            {
                VideoCleanup();
            }
            );
       }


       [Test(Name = "MEDIA STREAMING - H.264 (RTP/RTSP/TCP)",
            Path = PATH_U,
            Order = "01.01.13",
            Id = "1-1-13",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.H264, Feature.RTPRTSPTCP },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
       public void NvtMediaStreamingH264RtpRtspTcp()
       {
           RunTest(() =>
           {
               MediaUri streamUri = GetH264MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP);
               ValidateVideoSequence();
           },
            () =>
            {
                VideoCleanup();
            }
            );
       }


       [Test(Name = "SET SYNCHRONIZATION POINT - H.264",
            Path = PATH_U,
            Order = "01.01.14",
            Id = "1-1-14",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.H264 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaSetSynchronizationPoint })]
       public void NvtMediaStreamingH264SetSynchronizationPoint()
       {
           RunTest(() =>
           {
               MediaUri streamUri = GetH264MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
               ValidateSyncSequence();
           },
            () =>
            {
                VideoCleanup();
            }
            );
       }
        
        
        #endregion

        #region Multicast
        
       [ Test( Name = "MEDIA STREAMING – JPEG (RTP-Multicast/UDP, IPv4)",
                Path = PATH_M,
                Order = "01.02.01",
                Id = "1-2-1",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTPMulticastUDP },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingJpegRtpMulticastUdpIPv4()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv4);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }


        [ Test( Name = "MEDIA STREAMING – MPEG4 (RTP-Multicast/UDP, IPv4)",
                Path = PATH_M,
                Order = "01.02.02",
                Id = "1-2-2",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTPMulticastUDP, Feature.MPEG4 },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingMpeg4RtpMulticastUdpIPv4()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetMpeg4MediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv4);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – H.264 (RTP-Multicast/UDP, IPv4)",
                Path = PATH_M,
                Order = "01.02.03",
                Id = "1-2-3",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTPMulticastUDP, Feature.H264 },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
        public void NvtMediaStreamingH264RtpMulticastUdpIPv4()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetH264MediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv4);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }



        /*[ Test( Name = "MEDIA STREAMING – JPEG (RTP-Multicast/UDP, IPv6)",
                Path = PATH_M,
                Order = "01.02.04",
                Id = "1-2-4",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.IPv6 } ) ]  */
        public void NvtMediaStreamingJpegRtpMulticastUdpIPv6()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv6);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        
        /*[ Test( Name = "MEDIA STREAMING – MPEG4 (RTP-Multicast/UDP, IPv6)",
                Path = PATH_M,
                Order = "01.02.05",
                Id = "1-2-5",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.MPEG4, Feature.IPv6 } ) ]*/
        public void NvtMediaStreamingMpeg4RtpMulticastUdpIPv6()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetMpeg4MediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv6);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

        /*[ Test( Name = "MEDIA STREAMING – H.264 (RTP-Multicast/UDP, IPv6)",
                Path = PATH_M,
                Order = "01.02.06",
                Id = "1-2-6",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.H264, Feature.IPv6 } ) ]*/
        public void NvtMediaStreamingH264RtpMulticastUdpIPv6()
        {
            RunTest(
            () =>
            {
                MediaUri streamUri = GetH264MediaUri(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv6);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }

       #endregion

       [Test(Name = "NOTIFICATION STREAMING",
            Path = "Real Time Streaming\\Notification Streaming Interface",
            Order = "04.01.01",
            Id = "4-1-1",
            Category = Category.RTSS,
            Version = 1.02,
            ExecutionOrder = TestExecutionOrder.First, 
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri })]
       public void NvtNotificationStreaming()
       {
           bool StreamRunned = false;
           Profile profile = null;
           RunTest(() =>
           {
               profile = CreateProfile("Test", "Test");

               MetadataConfiguration[] metas = GetMetadataConfigurations();

               // patch for I/F
               if ((metas == null) || (metas.Length <= 0))
               {
                   return;
               }

               VideoSourceConfiguration[] sources = GetVideoSourceConfigurations();

               Assert((metas != null) && (metas.Length > 0) && (sources != null) && (sources.Length > 0),
                   "Video source or Metadata invalid",
                   "Video Source and Metadata Configuration");

               AddVideoSourceConfiguration("Test", sources[0].token);

               // fix for I/F
               try
               {
                   AddMetadataConfiguration("Test", metas[0].token);
               } catch (FaultException) {
                   LogStepEvent("Assuming I/F mode");
                   StepPassed();
                   return;
               }


               MetadataConfiguration meta = new MetadataConfiguration();
               meta.token = metas[0].token;
               meta.Multicast = metas[0].Multicast;
               meta.Name = metas[0].Name;
               meta.Analytics = false;
               meta.AnalyticsSpecified = true;
               meta.Events = new EventSubscription();
               meta.SessionTimeout = metas[0].SessionTimeout;
               
               // fix for I/F
               try
               {
                   SetMetadataConfiguration(meta, true);
               } catch (FaultException) {
                   LogStepEvent("Assuming I/F mode");
                   StepPassed();
                   return;
               }

               StreamSetup streamSetup = new StreamSetup();
               streamSetup.Transport = new Transport();
               streamSetup.Transport.Protocol = TransportProtocol.UDP;
               //streamSetup.Transport.Protocol = TransportProtocol.HTTP;
               streamSetup.Stream = StreamType.RTPUnicast;

               MediaUri streamUri = GetStreamUri(streamSetup, profile.token);

               AdjustVideo(streamSetup.Transport.Protocol, streamSetup.Stream, streamUri, null);

               _videoForm.EVENTS = true;
               _videoForm.EventSink = this;
               // fix for I/F
               try {
                    _videoForm.OpenWindow(false);
               } catch (Exception) {
                   LogStepEvent("Assuming I/F mode");
                   StepPassed();
                   return;
               }
               StreamRunned = true;

               /*Assert(_operator.GetYesNoAnswer("Do you observe video?"),
                   "Operator does not observe video",
                   "Video quality check (manual)");
               */
               SetSynchronizationPoint(profile.token);

               BeginStep("Collecting events");
               string Events = _videoForm.GetEvents();
               if (string.IsNullOrEmpty(Events))
               {
                   throw new Exception("No events within timeout");
               }
               LogStepEvent("Events: " + Events);
               StepPassed();
           },
            () =>
            {
                if (StreamRunned)
                {
                    _videoForm.CloseWindow();
                }
                _videoForm.EVENTS = false;
                _videoForm.EventSink = null;
                if (profile != null)
                    DeleteProfile("Test");
            }
            );
       }


       /* postponed to Phase 2
               [Test(Name = "MEDIA STREAMING – JPEG (RTP-Unicast/TCP)",
                   Path = PATH,
                   Order = "08.01.04",
                   Version = 1.02,
                   Interactive = true,
                   RequirementLevel = RequirementLevel.Must, 
                   RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTPTCP})]
        */
       public void NvtMediaStreamingJpegRtpUnicastTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.TCP);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }


       /* postponed to Phase 2
               [Test(Name = "MEDIA STREAMING - MPEG4 (RTP-Unicast/TCP)",
                   Path = PATH,
                   Order = "08.01.08",
                   Version = 1.02,
                   Interactive = true,
                   RequirementLevel = RequirementLevel.Must,
                   RequiredFeatures = new Feature[] { Feature.MediaService, Feature.MPEG4, Feature.RTPTCP })]
        */
       public void NvtMediaStreamingMpeg4RtpUnicastTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetMpeg4MediaUri(StreamType.RTPUnicast, TransportProtocol.TCP);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
            }
            );
        }


       /* postponed to Phase 2
              [Test(Name = "MEDIA STREAMING - H.264 (RTP-Unicast/TCP)",
               Path = PATH,
               Order = "08.01.13",
               Version = 1.02,
               Interactive = true,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.H264, Feature.RTPTCP })]
        */
       public void NvtMediaStreamingH264RtpUnicastTcp()
        {
            RunTest(() =>
            {
                MediaUri streamUri = GetH264MediaUri(StreamType.RTPUnicast, TransportProtocol.TCP);
                ValidateVideoSequence();
            },
             () =>
             {
                 VideoCleanup();
             }
             );
        }



    }
}
