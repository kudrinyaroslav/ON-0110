using System;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using System.ServiceModel;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Common.TestEngine;
using System.Collections.Generic;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Common.Media;
using System.Threading;

using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.TestCases.Utils;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class RtspVideoTestSuite : RtspTestSuite
    {
        private const string PATH_U = "Real Time Streaming\\Video Streaming\\Unicast";
        private const string PATH_M = "Real Time Streaming\\Video Streaming\\Multicast";

        NetworkConfiguration NetConfig;

        public RtspVideoTestSuite(TestLaunchParam param)
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
            _endpointController.UpdateAddress(new EndpointAddress(ServiceAddress));
        }

        #region Unicast

        [Test(Name = "MEDIA CONTROL – RTSP/TCP",
            Path = PATH_U,
            Order = "01.01.31",
            Id = "1-1-31",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp  })]
        public void NvtMediaControlRtspTcpTest()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                _videoForm.OPTIONS = true;
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – RTSP KEEPALIVE (SET_PARAMETER)",
            Path = PATH_U,
            Order = "01.01.32",
            Id = "1-1-32",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingRtspKeepAliveTest()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                _videoForm.KEEPALIVE = true;
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }


        [ Test( Name = "MEDIA STREAMING - RTSP KEEPALIVE (OPTIONS)",
                Path = PATH_U,
                Order = "01.01.33",
                Id = "1-1-33",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                LastChangedIn = "v14.12",
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingRtspKeepAliveOptions()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                _videoForm.KEEPALIVE = true;
                _videoForm.UseKeepAliveOptions = true;
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – JPEG (RTP-Unicast/UDP)",
            Path = PATH_U,
            Order = "01.01.34",
            Id = "1-1-34",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingJpegRtpUnicastUdp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri2(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "MEDIA STREAMING - JPEG (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH_U,
            Order = "01.01.35",
            Id = "1-1-35",
            Category = Category.RTSS,
            Version = 1.02,
            //RequirementLevel = RequirementLevel.Optional,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingJpegRtpUnicastRtspHttpTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri2(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        
        [Test(Name = "MEDIA STREAMING - JPEG (RTP/RTSP/TCP)",
            Path = PATH_U,
            Order = "01.01.36",
            Id = "1-1-36",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RTPRTSPTCP, Feature.MediaService, Feature.RTSS },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingJpegRtpRtspTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri2(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "MEDIA STREAMING - MPEG4 (RTP-Unicast/UDP)",
            Path = PATH_U,
            Order = "01.01.37",
            Id = "1-1-37",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.MPEG4 },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingMpeg4RtpUnicastUdp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetMpeg4MediaUri2(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }
        
        [Test(Name = "MEDIA STREAMING - MPEG4 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH_U,
            Order = "01.01.38",
            Id = "1-1-38",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.MPEG4 },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingMpeg4RtpUnicastRtspHttpTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetMpeg4MediaUri2(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "MEDIA STREAMING - MPEG4 (RTP/RTSP/TCP)",
            Path = PATH_U,
            Order = "01.01.39",
            Id = "1-1-39",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.MPEG4, Feature.RTPRTSPTCP },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingMpeg4RtpRtspTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetMpeg4MediaUri2(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "SET SYNCHRONIZATION POINT - MPEG4",
            Path = PATH_U,
            Order = "01.01.40",
            Id = "1-1-40",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.MPEG4 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaSetSynchronizationPoint, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingMpeg4SetSynchronizationPoint()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetMpeg4MediaUri2(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateSyncSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "MEDIA STREAMING - H.264 (RTP-Unicast/UDP)",
             Path = PATH_U,
             Order = "01.01.41",
             Id = "1-1-41",
             Category = Category.RTSS,
             Version = 1.02,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.H264 },
             LastChangedIn = "v15.06",
             FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingH264RtpUnicastUdp()
        {
          MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

          RunTest(() =>
          {
            MediaUri streamUri = GetH264MediaUri2(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
            ValidateStreamSequence2(false, false, true);
            //ValidateSyncSequence();
          },
           () =>
           {
             VideoCleanup2();
             RestoreMediaConfiguration(changeLog);
           }
           );
        }

       [Test(Name = "MEDIA STREAMING - H.264 (RTP-Unicast/RTSP/HTTP/TCP)",
            Path = PATH_U,
            Order = "01.01.42",
            Id = "1-1-42",
            Category = Category.RTSS,
            Version = 1.02,
            //RequirementLevel = RequirementLevel.Optional,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.H264 },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
       public void NvtMediaStreamingH264RtpUnicastRtspHttpTcp()
       {
           MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

           RunTest(() =>
           {
               MediaUri streamUri = GetH264MediaUri2(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
               ValidateStreamSequence2(false, false, true);
               //ValidateVideoSequence();
           },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
       }


       [Test(Name = "MEDIA STREAMING - H.264 (RTP/RTSP/TCP)",
            Path = PATH_U,
            Order = "01.01.43",
            Id = "1-1-43",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.H264, Feature.RTPRTSPTCP },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
       public void NvtMediaStreamingH264RtpRtspTcp()
       {
           MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

           RunTest(() =>
           {
               MediaUri streamUri = GetH264MediaUri2(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
               ValidateStreamSequence2(false, false, true);
               //ValidateVideoSequence();
           },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
       }


       [Test(Name = "SET SYNCHRONIZATION POINT - H.264",
            Path = PATH_U,
            Order = "01.01.44",
            Id = "1-1-44",
            Category = Category.RTSS,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.H264 },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaSetSynchronizationPoint, Functionality.MediaStreamingRtsp })]
       public void NvtMediaStreamingH264SetSynchronizationPoint()
       {
           MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

           RunTest(() =>
           {
               MediaUri streamUri = GetH264MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
               ValidateSyncSequence();
           },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
       }
        
        
        #endregion

        #region Multicast
        
       [ Test( Name = "MEDIA STREAMING – JPEG (RTP-Multicast/UDP, IPv4)",
                Path = PATH_M,
                Order = "01.02.13",
                Id = "1-2-13",
                Category = Category.RTSS,
                Version = 2.0,
                //RequirementLevel = RequirementLevel.Optional,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP },
                LastChangedIn = "v15.06",
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingJpegRtpMulticastUdpIPv4()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
              MediaUri streamUri = GetJpegMediaUri2(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv4, changeLog);
              ValidateStreamSequence2(false, false, true);
              //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }


        [ Test( Name = "MEDIA STREAMING – MPEG4 (RTP-Multicast/UDP, IPv4)",
                Path = PATH_M,
                Order = "01.02.14",
                Id = "1-2-14",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP, Feature.MPEG4 },
                LastChangedIn = "v15.06",
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingMpeg4RtpMulticastUdpIPv4()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                MediaUri streamUri = GetMpeg4MediaUri2(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv4, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – H.264 (RTP-Multicast/UDP, IPv4)",
                Path = PATH_M,
                Order = "01.02.15",
                Id = "1-2-15",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP, Feature.H264 },
                LastChangedIn = "v15.06",
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingH264RtpMulticastUdpIPv4()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                MediaUri streamUri = GetH264MediaUri2(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv4, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – JPEG (RTP-Multicast/UDP, IPv6)",
                Path = PATH_M,
                Order = "01.02.16",
                Id = "1-2-16",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP, Feature.IPv6 },
                LastChangedIn = "v15.06",
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingJpegRtpMulticastUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                MediaUri streamUri = GetJpegMediaUri2(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv6, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        
        [ Test( Name = "MEDIA STREAMING – MPEG4 (RTP-Multicast/UDP, IPv6)",
                Path = PATH_M,
                Order = "01.02.17",
                Id = "1-2-17",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP, Feature.MPEG4, Feature.IPv6 },
                LastChangedIn = "v15.06",
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingMpeg4RtpMulticastUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                MediaUri streamUri = GetMpeg4MediaUri2(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv6, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – H.264 (RTP-Multicast/UDP, IPv6)",
                Path = PATH_M,
                Order = "01.02.18",
                Id = "1-2-18",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP, Feature.H264, Feature.IPv6 },
                LastChangedIn = "v15.06",
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingH264RtpMulticastUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                MediaUri streamUri = GetH264MediaUri2(StreamType.RTPMulticast, TransportProtocol.UDP, IPType.IPv6, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

       #endregion

       [Test(Name = "NOTIFICATION STREAMING",
            Path = "Real Time Streaming\\Notification Streaming Interface",
            Order = "04.01.03",
            Id = "4-1-3",
            Category = Category.RTSS,
            Version = 1.02,
            ExecutionOrder = TestExecutionOrder.First, 
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp  })]
       public void NvtNotificationStreaming()
       {
           MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
           bool StreamRunned = false;
           Profile deletedProfile = null;
           Profile createdProfile = null;
           Profile modifiedProfile = null;
           Profile profile = null;
           RunTest(() =>
           {
               profile = CreateProfileByAnnex3("Test", null, out deletedProfile, out createdProfile, out modifiedProfile);
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

               AddVideoSourceConfiguration(profile.token, sources[0].token);

               // fix for I/F
               try
               {
                   AddMetadataConfiguration(profile.token, metas[0].token);
               } catch (FaultException) {
                   LogStepEvent("Assuming I/F mode");
                   StepPassed();
                   return;
               }

               MetadataConfiguration configCopy = Utils.CopyMaker.CreateCopy(metas[0]);
               changeLog.TrackModifiedConfiguration(configCopy);

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
                   _videoForm.OpenWindow(false, false);
               } catch (Exception) {
                   LogStepEvent("Assuming I/F mode");
                   StepPassed();
                   return;
               }
               StreamRunned = true;
               
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
                _videoForm.EventSink = null;

                RestoreMediaConfiguration(changeLog);
                RestoreProfileByAnnex3(deletedProfile, createdProfile, modifiedProfile);
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
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.TCP, changeLog);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
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
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetMpeg4MediaUri(StreamType.RTPUnicast, TransportProtocol.TCP, changeLog);
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
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
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetH264MediaUri(StreamType.RTPUnicast, TransportProtocol.TCP, changeLog);
                ValidateVideoSequence();
            },
             () =>
             {
                 VideoCleanup();
                 RestoreMediaConfiguration(changeLog);
             }
             );
        }


        [ Test( Name = "MEDIA STREAMING – RTP-Unicast/RTSP/HTTP/TCP (LINE BREAKS IN BASE64 ENCODING)",
                Path = PATH_U,
                Order = "01.01.45",
                Id = "1-1-45",
                Category = Category.RTSS,
                Version = 1.02,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void NvtMediaStreamingRtpUnicastRtspHttpTcpLineBreaks()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                MediaUri streamUri = GetJpegMediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                _videoForm.Base64LineBreaks = true;
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        class StreamParameters
        {
            public Profile profile = null;
            public StreamSetup streamSetup = null;
            public MediaUri streamUri = null;
            public string username;
            public string password;
            public int messageTimeout;
            public int duration;
            public WaitHandle waitHandle = new AutoResetEvent(false);
        }

        [Test(Name = "MEDIA STREAMING – JPEG (VALIDATING RTP HEADER EXTENSION)",
            Path = PATH_U,
            Order = "01.01.53",
            Id = "1-1-53",
            Category = Category.RTSS,
            Version = 2.2,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
            FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtspJpegHeaderExtension })]
        public void NvtMediaStreamingJpegRtpUnicastUdpExtension()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            RunTest(() =>
            {
                VideoEncoderConfigurationOptions videoOptions = null;
                VideoEncoding videoEncoding = VideoEncoding.JPEG;

                Profile profile = GetProfileForSpecificCodec(VideoEncoding.JPEG, out videoOptions);
                Assert(profile != null,
                    "Profile with JPEG Video encoder configuration not found",
                    "Check if required profile found");

                if (profile.VideoEncoderConfiguration.Encoding == VideoEncoding.JPEG)
                {
                    videoOptions = GetVideoEncoderConfigurationOptions(null, profile.token);
                }
                
                RunStep(() =>
                {
                    VideoResolution resolutionMax = videoOptions.JPEG.ResolutionsAvailable[0];
                    foreach (VideoResolution resolution in videoOptions.JPEG.ResolutionsAvailable)
                    {
                        if (resolution.Width > resolutionMax.Width || resolution.Height > resolutionMax.Height)
                        {
                            resolutionMax = resolution;
                        }
                    }
                    {
                        VideoEncoderConfiguration config = Utils.CopyMaker.CreateCopy(profile.VideoEncoderConfiguration);
                        changeLog.TrackModifiedConfiguration(config);
                    }                    
                    profile.VideoEncoderConfiguration.Resolution = resolutionMax;
                    LogStepEvent(string.Format("Selected resolution: {0}x{1}", resolutionMax.Width, resolutionMax.Height));
                }, "Select high resolution");


                SetVideoEncoding(profile.VideoEncoderConfiguration, videoEncoding, videoOptions);
                SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, false);

                StreamSetup streamSetup = new StreamSetup();
                streamSetup.Transport = new Transport();
                streamSetup.Transport.Protocol = TransportProtocol.UDP;
                streamSetup.Stream = StreamType.RTPUnicast;

                MediaUri streamUri = GetStreamUri(streamSetup, profile.token);
                AdjustVideo(streamSetup.Transport.Protocol, streamSetup.Stream, streamUri, profile.VideoEncoderConfiguration);

                _videoForm.CheckJPEGExtension = true;
                ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

       [Test(Name = "MEDIA STREAMING – JPEG (RTP-Unicast/UDP, IPv6)",
            Path = PATH_U,
            Order = "01.01.54",
            Id = "1-1-54",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.IPv6 },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void JpegStreamingUnicastUdpIPv6()
        {
           MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
           NetworkInterface netInterface = null;
           string newServiceAddr = null;
           string oldServiceAddr = null;
           bool restoreEnabled = false;

            RunTest(() =>
            {
                // A.1 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }
                
                MediaUri streamUri = GetJpegMediaUri2(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
                RestoreMediaConfiguration(changeLog);

                // A.2 - network settings restore
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

       [Test(Name = "MEDIA STREAMING - JPEG (RTP-Unicast/RTSP/HTTP/TCP, IPv6)",
            Path = PATH_U,
            Order = "01.01.55",
            Id = "1-1-55",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.IPv6 },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
       public void JpegStreamingUnicastRtspHttpTcpUdpIPv6()
       {
           MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
           NetworkInterface netInterface = null;
           string newServiceAddr = null;
           string oldServiceAddr = null;
           bool restoreEnabled = false;

           RunTest(() =>
           {
               // A.1 - turn on IPv6
               netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

               if (!string.IsNullOrEmpty(newServiceAddr))
               {
                   ReConnectTo(newServiceAddr);
                   base.Initialize();
               }
                
               MediaUri streamUri = GetJpegMediaUri2(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
               ValidateStreamSequence2(false, false, true);
               //ValidateVideoSequence();
           },
           () =>
           {
               VideoCleanup2();
               RestoreMediaConfiguration(changeLog);

               // A.2 - network settings restore
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

        [Test(Name = "MEDIA STREAMING – JPEG (RTP/RTSP/TCP, IPv6)",
            Path = PATH_U,
            Order = "01.01.56",
            Id = "1-1-56",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.RTPRTSPTCP, Feature.MediaService, Feature.RTSS, Feature.IPv6 },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void JpegStreamingUnicastRtpRtspTcpUdpIPv6()
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

                MediaUri streamUri = GetJpegMediaUri2(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
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

        [Test(Name = "MEDIA STREAMING – MPEG4 (RTP-Unicast/UDP, IPv6)",
            Path = PATH_U,
            Order = "01.01.57",
            Id = "1-1-57",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.MPEG4, Feature.IPv6 },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void Mpeg4StreamingUnicastUdpIPv6()
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

                MediaUri streamUri = GetMpeg4MediaUri2(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
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

        [Test(Name = "MEDIA STREAMING - MPEG4 (RTP-Unicast/RTSP/HTTP/TCP, IPv6)",
                Path = PATH_U,
                Order = "01.01.58",
                Id = "1-1-58",
                Category = Category.RTSS,
                Version = 1.0,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.MPEG4, Feature.IPv6 },
                LastChangedIn = "v15.06",
                FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void Mpeg4StreamingUnicastRtspHttpTcpUdpIPv6()
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

                MediaUri streamUri = GetMpeg4MediaUri2(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateStreamSequence2(false, false, true);
              //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
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

        [Test(Name = "MEDIA STREAMING - MPEG4 (RTP/RTSP/TCP, IPv6)",
            Path = PATH_U,
            Order = "01.01.59",
            Id = "1-1-59",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.MPEG4, Feature.RTPRTSPTCP, Feature.IPv6 },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void Mpeg4StreamingUnicastRtpRtspTcpUdpIPv6()
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

                MediaUri streamUri = GetMpeg4MediaUri2(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
            () =>
            {
                VideoCleanup2();
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

        [Test(Name = "MEDIA STREAMING - H.264 (RTP-Unicast/UDP, IPv6)",
            Path = PATH_U,
            Order = "01.01.60",
            Id = "1-1-60",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.H264, Feature.IPv6 },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void H264StreamingUnicastUdpIPv6()
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

                MediaUri streamUri = GetH264MediaUri2(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
             () =>
             {
                 VideoCleanup2();
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

        [Test(Name = "MEDIA STREAMING - H.264 (RTP-Unicast/RTSP/HTTP/TCP, IPv6)",
            Path = PATH_U,
            Order = "01.01.61",
            Id = "1-1-61",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.H264, Feature.IPv6 },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void H264StreamingUnicastRtspHttpTcpUdpIPv6()
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

                MediaUri streamUri = GetH264MediaUri2(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
             () =>
             {
                 VideoCleanup2();
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


        [Test(Name = "MEDIA STREAMING - H.264 (RTP/RTSP/TCP, IPv6)",
            Path = PATH_U,
            Order = "01.01.62",
            Id = "1-1-62",
            Category = Category.RTSS,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.H264, Feature.RTPRTSPTCP, Feature.IPv6 },
            LastChangedIn = "v15.06",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetStreamUri, Functionality.MediaStreamingRtsp })]
        public void H264StreamingUnicastRtpRtspTcpUdpIPv6()
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

                MediaUri streamUri = GetH264MediaUri2(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateStreamSequence2(false, false, true);
                //ValidateVideoSequence();
            },
             () =>
             {
                 VideoCleanup2();
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

    }
}
