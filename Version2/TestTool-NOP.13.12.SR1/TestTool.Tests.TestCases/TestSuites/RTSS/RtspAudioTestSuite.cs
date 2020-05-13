///////////////////////////////////////////////////////////////////////////
//!  @author        Ilya Gozman
////
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Exceptions;
using System.ServiceModel;
using TestTool.Tests.Common.TestBase;

using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.TestCases.Utils;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class RtspAudioTestSuite : RtspTestSuite
    {
        private const string PATH_U = "Real Time Streaming\\Audio Streaming\\Unicast";
        private const string PATH_M = "Real Time Streaming\\Audio Streaming\\Multicast";

        NetworkConfiguration NetConfig;

        public RtspAudioTestSuite(TestLaunchParam param)
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

        [ Test( Name = "MEDIA STREAMING – G.711 (RTP-Unicast/UDP)",
                Path = PATH_U,
                Order = "02.01.19",
                Id = "2-1-19",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingG711RtpUnicastUdp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                MediaUri streamUri = GetG711MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – G.711 (RTP-Unicast/RTSP/HTTP/TCP)",
                Path = PATH_U,
                Order = "02.01.20",
                Id = "2-1-20",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingG711RtpUnicastRtspHttpTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                MediaUri streamUri = GetG711MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – G.711 (RTP/RTSP/TCP)",
                Path = PATH_U,
                Order = "02.01.21",
                Id = "2-1-21",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPRTSPTCP },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingG711RtpRtspTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                MediaUri streamUri = GetG711MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – AAC (RTP-Unicast/UDP)",
                Path = PATH_U,
                Order = "02.01.25",
                Id = "2-1-25",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingAACRtpUnicastUdp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
                () =>
                {
                    MediaUri streamUri = GetAACMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                    ValidateAudioSequence();
                },
                () =>
                {
                    VideoCleanup();
                    RestoreMediaConfiguration(changeLog);
                }
            );
        }

        [ Test( Name = "MEDIA STREAMING – AAC (RTP-Unicast/RTSP/HTTP/TCP)",
                Path = PATH_U,
                Order = "02.01.26",
                Id = "2-1-26",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingAACRtpUnicastRtspHttpTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                MediaUri streamUri = GetAACMediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – AAC (RTP/RTSP/TCP)",
                Path = PATH_U,
                Order = "02.01.27",
                Id = "2-1-27",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPRTSPTCP, Feature.AAC },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingAACRtpRtspTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
                () =>
                {
                    MediaUri streamUri = GetAACMediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                    ValidateAudioSequence();
                },
                () =>
                {
                    VideoCleanup();
                    RestoreMediaConfiguration(changeLog);
                }
            );
        }    

        [ Test( Name = "MEDIA STREAMING – G.726 (RTP-Unicast/UDP)",
                Path = PATH_U,
                Order = "02.01.22",
                Id = "2-1-22",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726 },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingG726RtpUnicastUdp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                MediaUri streamUri = GetG726MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – G.726 (RTP-Unicast/RTSP/HTTP/TCP)",
                Path = PATH_U,
                Order = "02.01.23",
                Id = "2-1-23",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726 },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingG726RtpUnicastRtspHttpTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                MediaUri streamUri = GetG726MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "MEDIA STREAMING – G.726 (RTP/RTSP/TCP)",
                Path = PATH_U,
                Order = "02.01.24",
                Id = "2-1-24",
                Category = Category.RTSS,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPRTSPTCP, Feature.G726 },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void NvtMediaStreamingG726RtpRtspTcp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                MediaUri streamUri = GetG726MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateAudioSequence();
            },
            () =>
            {
                VideoCleanup();
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        void StreamingSetup(ref Profile profile, AudioEncoderConfigurationOptions audioOptions, 
                            AudioEncoding encoding, TransportProtocol protocol,
                            StreamType streamType, IPType? multicastAddressType, 
                            MediaConfigurationChangeLog changeLog)
        {

            {
                AudioEncoderConfiguration config = Utils.CopyMaker.CreateCopy(profile.AudioEncoderConfiguration);
                changeLog.TrackModifiedConfiguration(config);
            }

            // find nearest bitrate and sample rate
            int bitrate = FindNearestAudioBitrate(profile.AudioEncoderConfiguration.Bitrate,
                                                  encoding, audioOptions);
            int sampleRate = FindNearestAudioSamplerate(profile.AudioEncoderConfiguration.SampleRate,
                                                        encoding, audioOptions);

            if (multicastAddressType.HasValue)
            {
                SetMulticastSettings(profile, false, true, multicastAddressType.Value);
            }

            profile.AudioEncoderConfiguration.Encoding = encoding;
            profile.AudioEncoderConfiguration.Bitrate = bitrate;
            profile.AudioEncoderConfiguration.SampleRate = sampleRate;

            // audio encoder config setup
            SetAudioEncoderConfiguration(profile.AudioEncoderConfiguration, false, multicastAddressType.HasValue);

            // streaming setup
            StreamSetup streamSetup = new StreamSetup();
            streamSetup.Transport = new Transport();
            streamSetup.Transport.Protocol = protocol;
            streamSetup.Stream = streamType;

            UsedProfileToken = profile.token;
            MediaUri streamUri = GetStreamUri(streamSetup, profile.token);
            Assert(streamUri != null, "Stream URI is empty", "Validating of stream URI");


            AdjustVideo(protocol, streamType, streamUri, null);
        }

        [Test(Name = "AUDIO STREAMING – G.711 (RTP-Unicast/UDP)",
              Path = PATH_U,
              Order = "02.01.28",
              Id = "2-1-28",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG711RtpUnicastUdp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                Profile profile = null;
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G711, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G711, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions, 
                               AudioEncoding.G711, TransportProtocol.UDP, 
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "AUDIO STREAMING – G.711 (RTP-Unicast/RTSP/HTTP/TCP)",
              Path = PATH_U,
              Order = "02.01.29",
              Id = "2-1-29",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG711RtpUnicastHttp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                Profile profile = null;
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G711, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G711, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions, 
                               AudioEncoding.G711, TransportProtocol.HTTP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile removing
                RestoreMediaConfiguration(changeLog);
                VideoCleanup();
            }
            );
        }

        [Test(Name = "AUDIO STREAMING – G.711 (RTP/RTSP/TCP)",
              Path = PATH_U,
              Order = "02.01.30",
              Id = "2-1-30",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPRTSPTCP },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG711RtpUnicastRtsp()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G711, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G711, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G711, TransportProtocol.RTSP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "AUDIO STREAMING – G.726 (RTP-Unicast/UDP)",
              Path = PATH_U,
              Order = "02.01.31",
              Id = "2-1-31",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG726RtpUnicastUdp()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G726, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G726, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G726, TransportProtocol.UDP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "AUDIO STREAMING – G.726 (RTP-Unicast/RTSP/HTTP/TCP)",
              Path = PATH_U,
              Order = "02.01.32",
              Id = "2-1-32",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG726RtpUnicastHttp()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G726, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G726, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G726, TransportProtocol.HTTP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "AUDIO STREAMING – G.726 (RTP/RTSP/TCP)",
              Path = PATH_U,
              Order = "02.01.33",
              Id = "2-1-33",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726, Feature.RTPRTSPTCP },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG726RtpUnicastRtsp()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G726, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G726, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G726, TransportProtocol.RTSP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "AUDIO STREAMING – AAC (RTP-Unicast/UDP)",
              Path = PATH_U,
              Order = "02.01.34",
              Id = "2-1-34",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingAACRtpUnicastUdp()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.AAC, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.AAC, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.AAC, TransportProtocol.UDP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "AUDIO STREAMING – AAC (RTP-Unicast/RTSP/HTTP/TCP)",
              Path = PATH_U,
              Order = "02.01.35",
              Id = "2-1-35",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingAACRtpUnicastHttp()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.AAC, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.AAC, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.AAC, TransportProtocol.HTTP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "AUDIO STREAMING – AAC (RTP/RTSP/TCP)",
              Path = PATH_U,
              Order = "02.01.36",
              Id = "2-1-36",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC, Feature.RTPRTSPTCP },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingAACRtpUnicastRtsp()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.AAC, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.AAC, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.AAC, TransportProtocol.RTSP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – G.711 (RTP-Multicast/UDP, IPv4)",
              Path = PATH_M,
              Order = "02.02.11",
              Id = "2-2-11",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG711RtpMulticastIPv4()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G711, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G711, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G711, TransportProtocol.UDP,
                               StreamType.RTPMulticast, IPType.IPv4, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – G.711 (RTP-Multicast/UDP, IPv6)",
              Path = PATH_M,
              Order = "02.02.12",
              Id = "2-2-12",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG711RtpMulticastIPv6()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G711, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G711, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G711, TransportProtocol.UDP,
                               StreamType.RTPMulticast, IPType.IPv6, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – G.726 (RTP-Multicast/UDP, IPv4)",
              Path = PATH_M,
              Order = "02.02.13",
              Id = "2-2-13",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP, Feature.G726 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG726RtpMulticastIPv4()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G726, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G726, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G726, TransportProtocol.UDP,
                               StreamType.RTPMulticast, IPType.IPv4, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – G.726 (RTP-Multicast/UDP, IPv6)",
              Path = PATH_M,
              Order = "02.02.14",
              Id = "2-2-14",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP, Feature.G726, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG726RtpMulticastIPv6()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G726, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G726, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G726, TransportProtocol.UDP,
                               StreamType.RTPMulticast, IPType.IPv6, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – AAC (RTP-Multicast/UDP, IPv4)",
              Path = PATH_M,
              Order = "02.02.15",
              Id = "2-2-15",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP, Feature.AAC },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingAACRtpMulticastIPv4()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.AAC, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.AAC, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.AAC, TransportProtocol.UDP,
                               StreamType.RTPMulticast, IPType.IPv4, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – AAC (RTP-Multicast/UDP, IPv6)",
              Path = PATH_M,
              Order = "02.02.16",
              Id = "2-2-16",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP, Feature.AAC, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingAACRtpMulticastIPv6()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
            () =>
            {
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.AAC, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.AAC, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.AAC, TransportProtocol.UDP,
                               StreamType.RTPMulticast, IPType.IPv6, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();
            }
            );
        }

        [Test(Name = "MEDIA STREAMING – G.711 (RTP-Unicast/UDP, IPv6)",
                Path = PATH_U,
                Order = "02.01.37",
                Id = "2-1-37",
                Category = Category.RTSS,
                Version = 1.0,
                RequirementLevel = RequirementLevel.Optional,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.IPv6 },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void G711StreamingUnicastUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetG711MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateAudioSequence();
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

        [Test(Name = "MEDIA STREAMING – G.711 (RTP-Unicast/RTSP/HTTP/TCP, IPv6)",
               Path = PATH_U,
               Order = "02.01.38",
               Id = "2-1-38",
               Category = Category.RTSS,
               Version = 1.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.IPv6 },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void G711StreamingUnicastRtspHttpTcpUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetG711MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateAudioSequence();
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

        [Test(Name = "MEDIA STREAMING – G.711 (RTP/RTSP/TCP, IPv6)",
               Path = PATH_U,
               Order = "02.01.39",
               Id = "2-1-39",
               Category = Category.RTSS,
               Version = 1.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPRTSPTCP, Feature.IPv6 },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void G711StreamingUnicastRtpRtspTcpUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetG711MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateAudioSequence();
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

        [Test(Name = "MEDIA STREAMING – G.726 (RTP-Unicast/UDP, IPv6)",
               Path = PATH_U,
               Order = "02.01.40",
               Id = "2-1-40",
               Category = Category.RTSS,
               Version = 1.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726, Feature.IPv6 },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void G726StreamingUnicastUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetG726MediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                ValidateAudioSequence();
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

        [Test(Name = "MEDIA STREAMING – G.726 (RTP-Unicast/RTSP/HTTP/TCP, IPv6)",
               Path = PATH_U,
               Order = "02.01.41",
               Id = "2-1-41",
               Category = Category.RTSS,
               Version = 1.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726, Feature.IPv6 },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void G726StreamingUnicastRtspHttpTcpUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetG726MediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateAudioSequence();
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

        [Test(Name = "MEDIA STREAMING – G.726 (RTP/RTSP/TCP, IPv6)",
               Path = PATH_U,
               Order = "02.01.42",
               Id = "2-1-42",
               Category = Category.RTSS,
               Version = 1.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPRTSPTCP, Feature.G726, Feature.IPv6 },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void G726StreamingUnicastRtpRtspTcpUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetG726MediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                ValidateAudioSequence();
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

        [Test(Name = "MEDIA STREAMING – AAC (RTP-Unicast/UDP, IPv6)",
               Path = PATH_U,
               Order = "02.01.43",
               Id = "2-1-43",
               Category = Category.RTSS,
               Version = 1.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC, Feature.IPv6 },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void AACStreamingUnicastUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
                () =>
                {
                    // A.23 - turn on IPv6
                    netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                    if (!string.IsNullOrEmpty(newServiceAddr))
                    {
                        ReConnectTo(newServiceAddr);
                        base.Initialize();
                    }

                    MediaUri streamUri = GetAACMediaUri(StreamType.RTPUnicast, TransportProtocol.UDP, changeLog);
                    ValidateAudioSequence();
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

        [Test(Name = "MEDIA STREAMING – AAC (RTP-Unicast/RTSP/HTTP/TCP, IPv6)",
               Path = PATH_U,
               Order = "02.01.44",
               Id = "2-1-44",
               Category = Category.RTSS,
               Version = 1.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC, Feature.IPv6 },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void AACStreamingUnicastRtspHttpTcpUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                MediaUri streamUri = GetAACMediaUri(StreamType.RTPUnicast, TransportProtocol.HTTP, changeLog);
                ValidateAudioSequence();
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

        [Test(Name = "MEDIA STREAMING – AAC (RTP/RTSP/TCP, IPv6)",
               Path = PATH_U,
               Order = "02.01.45",
               Id = "2-1-45",
               Category = Category.RTSS,
               Version = 1.0,
               RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPRTSPTCP, Feature.AAC, Feature.IPv6 },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })]
        public void AACStreamingUnicastRtpRtspTcpUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
                () =>
                {
                    // A.23 - turn on IPv6
                    netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                    if (!string.IsNullOrEmpty(newServiceAddr))
                    {
                        ReConnectTo(newServiceAddr);
                        base.Initialize();
                    }

                    MediaUri streamUri = GetAACMediaUri(StreamType.RTPUnicast, TransportProtocol.RTSP, changeLog);
                    ValidateAudioSequence();
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

        [Test(Name = "AUDIO STREAMING – G.711 (RTP-Unicast/UDP, IPv6)",
              Path = PATH_U,
              Order = "02.01.46",
              Id = "2-1-46",
              Category = Category.RTSS,
              Version = 1.0,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
            ]
        public void AudioStreamingG711RtpUnicastUdpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                Profile profile = null;
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G711, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G711, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G711, TransportProtocol.UDP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();

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

        [Test(Name = "AUDIO STREAMING – G.711 (RTP-Unicast/RTSP/HTTP/TCP, IPv6)",
              Path = PATH_U,
              Order = "02.01.47",
              Id = "2-1-47",
              Category = Category.RTSS,
              Version = 1.0,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG711RtpUnicastHttpIPv6()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                Profile profile = null;
                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G711, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G711, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G711, TransportProtocol.HTTP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile removing
                RestoreMediaConfiguration(changeLog);
                VideoCleanup();

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

        [Test(Name = "AUDIO STREAMING – G.711 (RTP/RTSP/TCP, IPv6)",
              Path = PATH_U,
              Order = "02.01.48",
              Id = "2-1-48",
              Category = Category.RTSS,
              Version = 1.0,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPRTSPTCP, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG711RtpUnicastRtspIPv6()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G711, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G711, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G711, TransportProtocol.RTSP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();

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

        [Test(Name = "AUDIO STREAMING – G.726 (RTP-Unicast/UDP, IPv6)",
              Path = PATH_U,
              Order = "02.01.49",
              Id = "2-1-49",
              Category = Category.RTSS,
              Version = 1.0,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG726RtpUnicastUdpIPv6()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G726, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G726, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G726, TransportProtocol.UDP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();

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

        [Test(Name = "AUDIO STREAMING – G.726 (RTP-Unicast/RTSP/HTTP/TCP, IPv6)",
              Path = PATH_U,
              Order = "02.01.50",
              Id = "2-1-50",
              Category = Category.RTSS,
              Version = 1.0,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG726RtpUnicastHttpIPv6()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G726, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G726, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G726, TransportProtocol.HTTP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();

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

        [Test(Name = "AUDIO STREAMING – G.726 (RTP/RTSP/TCP, IPv6)",
              Path = PATH_U,
              Order = "02.01.51",
              Id = "2-1-51",
              Category = Category.RTSS,
              Version = 1.0,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.G726, Feature.RTPRTSPTCP, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingG726RtpUnicastRtspIPv6()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.G726, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.G726, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.G726, TransportProtocol.RTSP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();

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

        [Test(Name = "AUDIO STREAMING – AAC (RTP-Unicast/UDP, IPv6)",
              Path = PATH_U,
              Order = "02.01.52",
              Id = "2-1-52",
              Category = Category.RTSS,
              Version = 1.0,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingAACRtpUnicastUdpIPv6()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.AAC, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.AAC, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.AAC, TransportProtocol.UDP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();

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

        [Test(Name = "AUDIO STREAMING – AAC (RTP-Unicast/RTSP/HTTP/TCP, IPv6)",
              Path = PATH_U,
              Order = "02.01.53",
              Id = "2-1-53",
              Category = Category.RTSS,
              Version = 1.0,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingAACRtpUnicastHttpIPv6()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.AAC, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.AAC, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.AAC, TransportProtocol.HTTP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();

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

        [Test(Name = "AUDIO STREAMING – AAC (RTP/RTSP/TCP, IPv6)",
              Path = PATH_U,
              Order = "02.01.54",
              Id = "2-1-54",
              Category = Category.RTSS,
              Version = 1.0,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.AAC, Feature.RTPRTSPTCP, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.GetStreamUri })
        ]
        public void AudioStreamingAACRtpUnicastRtspIPv6()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            NetworkInterface netInterface = null;
            string newServiceAddr = null;
            string oldServiceAddr = null;
            bool restoreEnabled = false;

            RunTest(
            () =>
            {
                // A.23 - turn on IPv6
                netInterface = NetConfig.TurnOnIPv6(out newServiceAddr, out oldServiceAddr, out restoreEnabled);

                if (!string.IsNullOrEmpty(newServiceAddr))
                {
                    ReConnectTo(newServiceAddr);
                    base.Initialize();
                }

                //profile creating
                if (CreateOrSelectAudioProfile(ref profile, AudioEncoding.AAC, changeLog) == false)
                {
                    // test should be passed if there is no appropriate ready profiles found
                    // or we can't create new profile
                    return;
                }

                // adding AudioSource and AudioEncoder configurations with support of necessary audio codec
                AudioEncoderConfigurationOptions audioOptions = null;
                SelectAudioSourceEncoderConfigs(ref profile, AudioEncoding.AAC, ref audioOptions);

                // stream setup
                StreamingSetup(ref profile, audioOptions,
                               AudioEncoding.AAC, TransportProtocol.RTSP,
                               StreamType.RTPUnicast, null, changeLog);

                ValidateAudioSequence();
            },
            () =>
            {
                // profile restoring
                RestoreMediaConfiguration(changeLog);

                VideoCleanup();

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
