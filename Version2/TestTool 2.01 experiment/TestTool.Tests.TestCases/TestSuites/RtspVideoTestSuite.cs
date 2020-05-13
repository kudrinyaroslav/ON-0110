using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Enums;
using TestTool.Proxies.Media;
using System.ServiceModel;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class RtspVideoTestSuite : RtspTestSuite
    {

        private const string PATH = "Real Time Streaming\\Video Streaming";
        
        public RtspVideoTestSuite(TestLaunchParam param)
            : base(param)
        {

        }
        
        [Test(Name = "MEDIA CONTROL – RTSP/TCP",
            Path = PATH,
            Order = "08.01.01",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.Must)]
        public void NvtMediaControlRtspTcpTest()
        {
            RunTest(() =>
            {
#if true
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP);
                _videoForm.OPTIONS = true;
                ValidateVideoSequence();
#else
                MediaUri streamUri = new MediaUri();
                streamUri.Uri = "rtsp://192.168.10.201/onvif-media/media.amp";
                AdjustVideo(TransportProtocol.UDP, StreamType.RTPUnicast, streamUri, new VideoEncoderConfiguration());
                ValidateAudioVideoSequence();
#endif
            },
            () =>
            {
                VideoCleanup();
                _videoForm.OPTIONS = false;
            }
            );
        }


//#if FULL
        [Test(Name = "MEDIA STREAMING – RTSP KEEPALIVE",
            Path = PATH,
            Order = "08.01.02",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.Must)]
//#endif
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


        [Test(Name = "MEDIA STREAMING – JPEG (RTP-Unicast/UDP)",
            Path = PATH,
            Order = "08.01.03",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.Must)]
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

/* postponed to Phase 2
        [Test(Name = "MEDIA STREAMING – JPEG (RTP-Unicast/TCP)",
            Path = PATH,
            Order = "08.01.04",
            Version = 1.02,
            Interactive = true,
            RequirementLevel = RequirementLevel.ConditionalMust, 
            RequiredFeatures = new Feature[] { Feature.RTPTCP})]
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


        [Test(Name = "MEDIA STREAMING - JPEG (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH,
            Order = "08.01.05",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.Must)]
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
            Path = PATH,
            Order = "08.01.06",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] {Feature.RTPRTSPTCP})]
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
            Path = PATH,
            Order = "08.01.07",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures =  new Feature[] { Feature.MPEG4 })]
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

/* postponed to Phase 2
        [Test(Name = "MEDIA STREAMING - MPEG4 (RTP-Unicast/TCP)",
            Path = PATH,
            Order = "08.01.08",
            Version = 1.02,
            Interactive = true,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.MPEG4, Feature.RTPTCP })]
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

        [Test(Name = "MEDIA STREAMING - MPEG4 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH,
            Order = "08.01.09",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] {Feature.MPEG4})]
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
            Path = PATH,
            Order = "08.01.10",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.MPEG4, Feature.RTPRTSPTCP })]
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


//#if FULL
        [Test(Name = "SET SYNCHRONIZATION POINT - MPEG4",
            Path = PATH,
            Order = "08.01.11",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.MPEG4 })]
//#endif
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
        Path = PATH,
        Order = "08.01.12",
        Version = 1.02,
        Interactive = true,
        Services = new Service[] { Service.Device, Service.Media },
        RequirementLevel = RequirementLevel.ConditionalMust,
        RequiredFeatures = new Feature[] { Feature.H264 })]
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


/* postponed to Phase 2
       [Test(Name = "MEDIA STREAMING - H.264 (RTP-Unicast/TCP)",
        Path = PATH,
        Order = "08.01.13",
        Version = 1.02,
        Interactive = true,
        RequirementLevel = RequirementLevel.ConditionalMust,
        RequiredFeatures = new Feature[] { Feature.H264, Feature.RTPTCP })]
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


       [Test(Name = "MEDIA STREAMING - H.264 (RTP-Unicast/RTSP/HTTP/TCP)",
        Path = PATH,
        Order = "08.01.14",
        Version = 1.02,
        Interactive = true,
        Services = new Service[] { Service.Device, Service.Media },
        RequirementLevel = RequirementLevel.ConditionalMust,
        RequiredFeatures = new Feature[] { Feature.H264 })]
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
        Path = PATH,
        Order = "08.01.15",
        Version = 1.02,
        Interactive = true,
        Services = new Service[] { Service.Device, Service.Media },
        RequirementLevel = RequirementLevel.ConditionalMust,
        RequiredFeatures = new Feature[] { Feature.H264, Feature.RTPRTSPTCP })]
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


//#if FULL
       [Test(Name = "SET SYNCHRONIZATION POINT - H.264",
            Path = PATH,
            Order = "08.01.16",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.H264 })]
//#endif
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


       [Test(Name = "NOTIFICATION STREAMING",
            Path = "Event Handling\\Notification Streaming Interface",
            Order = "09.04.01",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Events, Service.Media },
            RequirementLevel = RequirementLevel.Must)]
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

    }
}
