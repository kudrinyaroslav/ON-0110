///////////////////////////////////////////////////////////////////////////
//!  @author        Ilya Gozman
////
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Common.Media;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using System.Linq;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class RtpTestSuite : RTSSTestSuite
    {
        private const string PATH = "Real Time Streaming\\Start and Stop Multicast Streaming";

        public RtpTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        [ Test( Name = "START AND STOP MULTICAST STREAMING – JPEG (IPv4)",
                Path = PATH,
                Order = "05.01.07",
                Id = "5-1-7",
                Category = Category.RTSS,
                Version = 2.0,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP },
                FunctionalityUnderTest =  new Functionality[]{Functionality.MediaStreamingRtsp, Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming}) ]
        public void NvtStartAndStopMulticastStreamingJpegIPv4()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            Profile profile = null;
            RunTest(() =>
            {
                StartMulticastStreaming2(out profile, VideoEncoding.JPEG, null, changeLog);
            },
            () =>
            {
                StopMulticastStreaming2(profile);
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "START AND STOP MULTICAST STREAMING – G.711 (IPv4)",
                Path = PATH,
                Order = "05.01.08",
                Id = "5-1-8",
                Category = Category.RTSS,
                Version = 2.0,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP, Feature.Audio },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming })]
        public void NvtStartAndStopMulticastStreamingG711IPv4()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                StartMulticastStreaming2(out profile, null, AudioEncoding.G711, changeLog);
            },
            () =>
            {
                StopMulticastStreaming2(profile);
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [ Test( Name = "START AND STOP MULTICAST STREAMING – JPEG/G.711 (IPv4)",
                Path = PATH,
                Order = "05.01.09",
                Id = "5-1-9",
                Category = Category.RTSS,
                Version = 2.0,
                LastChangedIn = "v15.06",
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP, Feature.Audio },
                FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming })]
        public void NvtStartAndStopMulticastStreamingJpegG711IPv4()
        {
            Profile profile = null;
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(() =>
            {
                StartMulticastStreaming2(out profile, VideoEncoding.JPEG, AudioEncoding.G711, changeLog);
            },
            () =>
            {
                StopMulticastStreaming2(profile);
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "START AND STOP MULTICAST STREAMING – G.711 (IPv4, ONLY AUDIO PROFILE)",
              Path = PATH,
              Order = "05.01.10",
              Id = "5-1-10",
              Category = Category.RTSS,
              Version = 2.2,
              LastChangedIn = "v15.06",
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP, Feature.Audio },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming })
        ]
        public void StartAndStopMulticastStreamingG711IPv4AudioOnly()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            Profile profile = null;
            bool multicastStarted = false;
            RunTest(() =>
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

                // streaming
                AudioMulticastStreamingSetup2(ref profile, audioOptions, AudioEncoding.G711,
                    TransportProtocol.UDP, StreamType.RTPMulticast, IPType.IPv4, out multicastStarted, changeLog);
            },
            () =>
            {
                if (multicastStarted)
                {
                    StopMulticastStreaming2(profile);
                }
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "START AND STOP MULTICAST STREAMING – G.726 (IPv4, ONLY AUDIO PROFILE)",
              Path = PATH,
              Order = "05.01.11",
              Id = "5-1-11",
              Category = Category.RTSS,
              Version = 2.2,
              LastChangedIn = "v15.06",
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP, Feature.G726 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming })
        ]
        public void StartAndStopMulticastStreamingG726IPv4AudioOnly()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            Profile profile = null;
            bool multicastStarted = false;

            RunTest(() =>
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

                // streaming
                AudioMulticastStreamingSetup2(ref profile, audioOptions, AudioEncoding.G726,
                    TransportProtocol.UDP, StreamType.RTPMulticast, IPType.IPv4, out multicastStarted, changeLog);
            },
            () =>
            {
                if (multicastStarted)
                {
                    StopMulticastStreaming2(profile);
                } 
                RestoreMediaConfiguration(changeLog);
            }
            );
        }

        [Test(Name = "START AND STOP MULTICAST STREAMING – AAC (IPv4, ONLY AUDIO PROFILE)",
              Path = PATH,
              Order = "05.01.12",
              Id = "5-1-12",
              Category = Category.RTSS,
              Version = 2.2,
              LastChangedIn = "v15.06",
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP, Feature.AAC },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming })
        ]
        public void StartAndStopMulticastStreamingAACIPv4AudioOnly()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            Profile profile = null;
            bool multicastStarted = false;

            RunTest(() =>
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

                // streaming
                AudioMulticastStreamingSetup2(ref profile, audioOptions, AudioEncoding.AAC,
                    TransportProtocol.UDP, StreamType.RTPMulticast, IPType.IPv4, out multicastStarted, changeLog);
            },
            () =>
            {
                if (multicastStarted)
                {
                    StopMulticastStreaming2(profile);
                }
                RestoreMediaConfiguration(changeLog);
            }
            );
        }
    }
}