using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Common.Media;

namespace TestTool.Tests.TestCases.TestSuites.RTSS
{
    [TestClass]
    class GuaranteedNumberTestSuite : MediaConfigurationTestSuiteBase
    {
        const string PATH_VIDEO_U = "Real Time Streaming\\Video Streaming\\Unicast";
        const string PATH_VIDEO_M = "Real Time Streaming\\Video Streaming\\Multicast";

        public GuaranteedNumberTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        protected void RunStep2(Action action, string stepName, bool failTest)
        {
            BeginStep(stepName);
            try
            {
                action();
            }
            catch (Exception ex)
            {
                StepFailed(ex);
                if (failTest)
                {
                    throw ex;
                }
                return;
            }
            StepPassed();
        }

        protected void MultipleStreamTest(
            MediaConfigurationChangeLog changeLog,
            GetStreamSetup getStreamSetup
            )
        {
            //3.	ONVIF Client will invoke GetVideoSourceConfigurationsRequest message to retrieve 
            // all DUT video source configurations.
            //4.	Verify the GetVideoSourceConfigurationsResponse message from the DUT.

            VideoSourceConfiguration[] sourceConfigurations = GetVideoSourceConfigurations();
            CheckConfigurationsList(sourceConfigurations, "Video Source Configuration");

            //List<VideoSourceConfiguration> selectedConfigs = MediaTestUtils.SelectConfigurations(sourceConfigurations);

            foreach (VideoSourceConfiguration config in sourceConfigurations)
            {

                //5.	ONVIF Client will invoke GetGuaranteedNumberOfVideoEncoderInstancesRequest message 
                // (ConfigurationToken = “VSCToken1”, where “VSCToken1” is a first video source configuration 
                // token from GetVideoSourceConfigurationsResponse message) to retrieve guaranteed number of 
                // video encoder instances per first video source configuration.
                int? jpeg;
                int? mpeg;
                int? h264;
                int totalNumber = GetGuaranteedNumberOfVideoEncoderInstances(config.token, out jpeg, out h264, out mpeg);

                //6.	Verify the GetGuaranteedNumberOfVideoEncoderInstancesResponse message from the DUT.
                //7.	Create or find number of profiles equal to TotalNumber from 
                // GetGuaranteedNumberOfVideoEncoderInstancesResponse message that contains video source 
                // configuration with token “VSCToken1” and video encoder configuration (see Annex A.9).
                List<Profile> profiles = GetProfilesForMultiStreamingTest(
                    config.token, 
                    totalNumber,
                    jpeg,
                    mpeg,
                    h264,
                    changeLog);

                Assert(profiles.Count == totalNumber, 
                    "Required number of profiles could not be found or created", 
                    "Check that required number of profiles has been achieved");

                SetResourcesUsageToMimimal(changeLog, profiles);
                
                //8.	ONVIF Client invokes GetStreamUriRequest message (Profile Token, RTP-Unicast, 
                // UDP transport) to retrieve media stream URI for the first media profile from step 7.

              
                //9.	DUT sends RTSP URI and parameters defining the lifetime of the URI like 
                // ValidUntilConnect, ValidUntilReboot and Timeout in the GetStreamUriResponse message.
                //10.	ONVIF Client verifies the RTSP media stream URI provided by the DUT.
                //11.	ONVIF Client invokes RTSP DESCRIBE request.
                //12.	DUT sends 200 OK message and SDP information.
                //13.	 ONVIF Client invokes RTSP SETUP request with transport parameter as RTP/UDP.
                //14.	DUT sends 200 OK message and the media stream information.
                //15.	ONVIF Client invokes RTSP PLAY request.
                //16.	DUT sends 200 OK message and starts media streaming.
                //17.	DUT sends JPEG RTP media stream to ONVIF Client over UDP.
                //18.	DUT sends RTCP sender report to ONVIF Client.
                //19.	DUT validates the received RTP and RTCP packets, decodes and renders them.
                //20.	Repeat steps 8-20 to start video streaming for all profiles from step 7.
                //21.	ONVIF Client invokes RTSP TEARDOWN control request at the end of media streaming 
                // to terminate the RTSP session for each started stream.
                //22.	DUT sends 200 OK Response and terminates the RTSP Session.

                VideoUtils.ShowMultiple(
                    this, _semaphore.StopEvent, _username, _password, _messageTimeout,
                    getStreamSetup, _videoForm.NICIndex, profiles, GetStreamUri,
                    (a, name, failTest) => RunStep2(a, name, failTest),
                    (ex) => StepFailed(ex));

                //23.	Repeat steps 5-23 for the rest of video source configurations.
            }
        }


        void SetResourcesUsageToMimimal(MediaConfigurationChangeLog changeLog, IEnumerable<Profile> profiles)
        {
            List<VideoEncoderConfiguration> modifiedConfigurations = new List<VideoEncoderConfiguration>();
            // set resolutions and FPS to minimal values
            foreach (Profile profile in profiles)
            {
                // actually we change configurations
                // so we must track modified configurations and don't apply changes more than once
                VideoEncoderConfiguration AlreadyModifiedVec = null;
                AlreadyModifiedVec = modifiedConfigurations.Find(vec => null != profile.VideoEncoderConfiguration && vec.token == profile.VideoEncoderConfiguration.token);
                if (AlreadyModifiedVec != null)
                {
                    profile.VideoEncoderConfiguration = AlreadyModifiedVec;
                    continue;
                }

                if (null != profile.VideoEncoderConfiguration)
                {
                    VideoEncoderConfigurationOptions options = GetVideoEncoderConfigurationOptions(profile.VideoEncoderConfiguration.token, null);
    #if true
                    if (OptimizeVEC(changeLog, profile.VideoEncoderConfiguration, options))
                    {
                      //SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false);
                      SetVideoEncoderConfiguration(Utils.CopyMaker.CreateCopy(profile.VideoEncoderConfiguration), false);
                      modifiedConfigurations.Add(profile.VideoEncoderConfiguration);
                    }
    #else
                    VideoResolution[] resolutionsAvailable = null;
                    IntRange fpsRange = null;
                    switch (profile.VideoEncoderConfiguration.Encoding)
                    { 
                        case VideoEncoding.JPEG:
                            if (options.JPEG != null)
                            {
                                resolutionsAvailable = options.JPEG.ResolutionsAvailable;
                                fpsRange = options.JPEG.FrameRateRange;                            
                            }
                            break;
                        case VideoEncoding.H264:
                            if (options.H264 != null)
                            {
                                resolutionsAvailable = options.H264.ResolutionsAvailable;
                                fpsRange = options.H264.FrameRateRange;
                            }
                            break;
                        case VideoEncoding.MPEG4:
                            if (options.MPEG4 != null)
                            {
                                resolutionsAvailable = options.MPEG4.ResolutionsAvailable;
                                fpsRange = options.MPEG4.FrameRateRange;
                            }
                            break;
                    }

                    VideoResolution minimalResolution = null;
                    bool updateResolution = false;
                    if (resolutionsAvailable != null)
                    {
                        VideoResolution currentResolution = profile.VideoEncoderConfiguration.Resolution;
                        foreach (VideoResolution resolution in resolutionsAvailable)
                        {
                            if (minimalResolution == null)
                            {
                                minimalResolution = resolution;
                            }
                            else
                            {
                                if (minimalResolution.Height * minimalResolution.Width > resolution.Height * resolution.Width)
                                {
                                    minimalResolution = resolution;
                                }
                            }
                        }
                        updateResolution = (minimalResolution.Width * minimalResolution.Height < currentResolution.Width * currentResolution.Height);
                    }

                    bool updateFps = false;
                    if (fpsRange != null)
                    {
                        if (profile.VideoEncoderConfiguration.RateControl != null)
                        {
                            if (profile.VideoEncoderConfiguration.RateControl.FrameRateLimit > fpsRange.Min)
                            {
                                updateFps = true;
                            }
                        }
                        else
                        {
                            updateFps = true;
                        }
                    }

                    if (updateResolution || updateFps)
                    {
                        VideoEncoderConfiguration backup = Utils.CopyMaker.CreateCopy(profile.VideoEncoderConfiguration);
                        changeLog.TrackModifiedConfiguration(backup);
                        if (updateResolution)
                        {
                            profile.VideoEncoderConfiguration.Resolution = minimalResolution;
                        }
                        if (updateFps)
                        {
                            if (profile.VideoEncoderConfiguration.RateControl == null)
                            {
                                profile.VideoEncoderConfiguration.RateControl = new VideoRateControl();
                            }
                            profile.VideoEncoderConfiguration.RateControl.FrameRateLimit = fpsRange.Min;
                        }
                        SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false);
                        modifiedConfigurations.Add(profile.VideoEncoderConfiguration);
                    }
    #endif
                }
            }
        }

        [Test(Name = "MEDIA STREAMING – GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES (RTP-Unicast/UDP) (ALL VIDEO SOURCE CONFIGURATIONS)",
           Path = PATH_VIDEO_U,
           Order = "01.01.49",
           Id = "1-1-49",
           Category = Category.RTSS,
           Version = 2.2,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, })]
        public void GuarenteedNumberOfVideoEncoderInstancesUdp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            RunTest(
                () =>
                {
                    MultipleStreamTest(changeLog, (ref Profile profile) =>
                    {
                        StreamSetup streamSetup = new StreamSetup();
                        streamSetup.Transport = new Transport();
                        streamSetup.Transport.Protocol = TransportProtocol.UDP;
                        streamSetup.Stream = StreamType.RTPUnicast;
                        return streamSetup;
                    });
                },
                () =>
                {
                    RestoreMediaConfiguration(changeLog);
                }
                );

        }

        [Test(Name = "MEDIA STREAMING – GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES (RTP-Multicast/UDP) (ALL VIDEO SOURCE CONFIGURATIONS)",
           Path = PATH_VIDEO_M,
           Order = "01.02.23",
           Id = "1-2-23",
           Category = Category.RTSS,
           Version = 2.2,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, })]
        public void GuarenteedNumberOfVideoEncoderInstancesMulticast()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            RunTest(
                () =>
                {
                    // list of used VEC tokens for RTP-Multicast streaming
                    List<string> MulticastTokenList = new List<string>();

                    // list of used VSC tokens
                    List<string> VscTokenList = new List<string>();

                    // lists of used ip addresses and ports for RTP-Multicast streaming
                    List<int> usedMulticastPorts = new List<int>();
                    List<string> usedMulticastIPs = new List<string>();

                    MultipleStreamTest(changeLog, (ref Profile profile) =>
                    {
                        if (!VscTokenList.Contains(profile.VideoSourceConfiguration.token))
                        {
                            VscTokenList.Add(profile.VideoSourceConfiguration.token);

                            // we should clear this for every new Video Source Configuration
                            if (0 != MulticastTokenList.Count)
                            {
                                MulticastTokenList.Clear();
                            }
                        }

                        StreamSetup streamSetup = new StreamSetup();
                        streamSetup.Transport = new Transport();
                        streamSetup.Transport.Protocol = TransportProtocol.UDP;
                        streamSetup.Stream = StreamType.RTPMulticast;

                        if (MulticastTokenList.Contains(profile.VideoEncoderConfiguration.token))
                        {
                            profile = null;
                        }
                        else
                        {
                            MulticastTokenList.Add(profile.VideoEncoderConfiguration.token);

                            string addressVideo = "";
                            int portVideo = 0;

                            string addressAudio = "";
                            int portAudio = 0;

                            string addressMetadata = "";
                            int portMetadata = 0;

                            if (profile.VideoEncoderConfiguration != null)
                            {
                                addressVideo = GetMulticastAddress3(usedMulticastIPs);
                                usedMulticastIPs.Add(addressVideo);
                                portVideo = GetMulticastPort2(usedMulticastPorts);
                                usedMulticastPorts.Add(portVideo);

                                VideoEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(profile.VideoEncoderConfiguration);
                                changeLog.TrackModifiedConfiguration(configCopy);
                                SetMulticast(profile.VideoEncoderConfiguration.Multicast, IPType.IPv4, addressVideo, portVideo);
                                SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, true);
                            }

                            if (profile.AudioEncoderConfiguration != null)
                            {
                                addressAudio = GetMulticastAddress3(usedMulticastIPs);
                                usedMulticastIPs.Add(addressAudio);
                                portAudio = GetMulticastPort2(usedMulticastPorts);
                                usedMulticastPorts.Add(portAudio);
                            }

                            if (profile.MetadataConfiguration != null)
                            {
                                addressMetadata = GetMulticastAddress3(usedMulticastIPs);
                                usedMulticastIPs.Add(addressMetadata);
                                portMetadata = GetMulticastPort2(usedMulticastPorts);
                                usedMulticastPorts.Add(portMetadata);
                            }

                            SetMulticastSettings(profile, IPType.IPv4, changeLog,
                                addressAudio, portAudio,
                                addressVideo, portVideo,
                                addressMetadata, portMetadata);
                        }

                        return streamSetup;
                    });
                },
                () =>
                {
                    RestoreMediaConfiguration(changeLog);
                }
                );

        }

        [Test(Name = "MEDIA STREAMING – GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES (RTP-Unicast/RTSP/HTTP/TCP) (ALL VIDEO SOURCE CONFIGURATIONS)",
           Path = PATH_VIDEO_U,
           Order = "01.01.50",
           Id = "1-1-50",
           Category = Category.RTSS,
           Version = 2.2,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, })]
        public void GuarenteedNumberOfVideoEncoderInstancesHttp()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            RunTest(
                () =>
                {
                    MultipleStreamTest(changeLog, (ref Profile profile) =>
                    {
                        StreamSetup streamSetup = new StreamSetup();
                        streamSetup.Transport = new Transport();
                        streamSetup.Transport.Protocol = TransportProtocol.HTTP;
                        streamSetup.Stream = StreamType.RTPUnicast;
                        return streamSetup;
                    });
                },
                () =>
                {
                    RestoreMediaConfiguration(changeLog);
                }
                );

        }

        [Test(Name = "MEDIA STREAMING – GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES (RTP/RTSP/TCP) (ALL VIDEO SOURCE CONFIGURATIONS)",
           Path = PATH_VIDEO_U,
           Order = "01.01.51",
           Id = "1-1-51",
           Category = Category.RTSS,
           Version = 2.2,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPRTSPTCP },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, })]
        public void GuarenteedNumberOfVideoEncoderInstancesTCP()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            RunTest(
                () =>
                {
                    MultipleStreamTest(changeLog, (ref Profile profile) =>
                    {
                        StreamSetup streamSetup = new StreamSetup();
                        streamSetup.Transport = new Transport();
                        streamSetup.Transport.Protocol = TransportProtocol.RTSP;
                        streamSetup.Stream = StreamType.RTPUnicast;
                        return streamSetup;
                    });
                },
                () =>
                {
                    RestoreMediaConfiguration(changeLog);
                }
                );

        }

        [Test(Name = "MEDIA STREAMING – GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES (MIX OF TRANSPORT TYPES) (ALL VIDEO SOURCE CONFIGURATIONS)",
           Path = PATH_VIDEO_U,
           Order = "01.01.52",
           Id = "1-1-52",
           Category = Category.RTSS,
           Version = 2.2,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, })]
        public void GuarenteedNumberOfVideoEncoderInstancesMix()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            
            // 4 StreamSetup types:
            // - RTPUDP - must be supported
            // - RTPRTSPHTTP - must be supported
            // - RTPRTSPTCP - optional
            // - RTPMulticastUDP - optional

            List<StreamSetup> supportedSetups = new List<StreamSetup>();

            StreamSetup streamSetup1 = new StreamSetup();
            streamSetup1.Transport = new Transport();
            streamSetup1.Transport.Protocol = TransportProtocol.UDP;
            streamSetup1.Stream = StreamType.RTPUnicast;
            supportedSetups.Add(streamSetup1);

            StreamSetup streamSetup2 = new StreamSetup();
            streamSetup2.Transport = new Transport();
            streamSetup2.Transport.Protocol = TransportProtocol.HTTP;
            streamSetup2.Stream = StreamType.RTPUnicast;
            supportedSetups.Add(streamSetup2);

            bool TCP = Features.Contains(Feature.RTPRTSPTCP);
            StreamSetup streamSetup3 = new StreamSetup();
            streamSetup3.Transport = new Transport();
            if (TCP)
            {
                streamSetup3.Transport.Protocol = TransportProtocol.RTSP;
                streamSetup3.Stream = StreamType.RTPUnicast;
                supportedSetups.Add(streamSetup3);
            }

            bool Multicast = Features.Contains(Feature.RTPMulticastUDP);
            StreamSetup streamSetup4 = new StreamSetup();
            streamSetup4.Transport = new Transport();
            if (Multicast)
            {
                streamSetup4.Transport.Protocol = TransportProtocol.UDP;
                streamSetup4.Stream = StreamType.RTPMulticast;
                supportedSetups.Add(streamSetup4);
            }

            // list of used VEC tokens for RTP-Multicast streaming
            List<string> MulticastTokenList = new List<string>();

            // list of used VSC tokens
            List<string> VscTokenList = new List<string>();

            // lists of used ip addresses and ports for RTP-Multicast streaming
            List<int> usedMulticastPorts = new List<int>();
            List<string> usedMulticastIPs = new List<string>();

            RunTest(
                () =>
                {
                    int step = 0;

                    MultipleStreamTest(changeLog, (ref Profile profile) =>
                    {
                        if (!VscTokenList.Contains(profile.VideoSourceConfiguration.token))
                        {
                            VscTokenList.Add(profile.VideoSourceConfiguration.token);

                            // we should clear this for every new Video Source Configuration
                            if (0 != MulticastTokenList.Count)
                            {
                                MulticastTokenList.Clear();
                            }
                        }

                        int idx = step % supportedSetups.Count;
                        StreamSetup current = supportedSetups[idx];

                        // setup multicast
                        if (current.Stream == StreamType.RTPMulticast)
                        {
                            if (MulticastTokenList.Contains(profile.VideoEncoderConfiguration.token))
                            {
                                idx = (++step) % supportedSetups.Count;
                                current = supportedSetups[idx];
                            }
                            else
                            {
                                MulticastTokenList.Add(profile.VideoEncoderConfiguration.token);

                                string addressVideo = "";
                                int portVideo = 0;

                                string addressAudio = "";
                                int portAudio = 0;

                                string addressMetadata = "";
                                int portMetadata = 0;

                                if (profile.VideoEncoderConfiguration != null)
                                {
                                    addressVideo = GetMulticastAddress3(usedMulticastIPs);
                                    usedMulticastIPs.Add(addressVideo);
                                    portVideo = GetMulticastPort2(usedMulticastPorts);
                                    usedMulticastPorts.Add(portVideo);

                                    VideoEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(profile.VideoEncoderConfiguration);
                                    changeLog.TrackModifiedConfiguration(configCopy);
                                    SetMulticast(profile.VideoEncoderConfiguration.Multicast, IPType.IPv4, addressVideo, portVideo);
                                    SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, true);
                                }

                                if (profile.AudioEncoderConfiguration != null)
                                {
                                    addressAudio = GetMulticastAddress3(usedMulticastIPs);
                                    usedMulticastIPs.Add(addressAudio);
                                    portAudio = GetMulticastPort2(usedMulticastPorts);
                                    usedMulticastPorts.Add(portAudio);
                                }

                                if (profile.MetadataConfiguration != null)
                                {
                                    addressMetadata = GetMulticastAddress3(usedMulticastIPs);
                                    usedMulticastIPs.Add(addressMetadata);
                                    portMetadata = GetMulticastPort2(usedMulticastPorts);
                                    usedMulticastPorts.Add(portMetadata);
                                }

                                SetMulticastSettings(profile, IPType.IPv4, changeLog,
                                    addressAudio, portAudio,
                                    addressVideo, portVideo,
                                    addressMetadata, portMetadata);
                            }
                        }

                        ++step;
                        return current;
                    });
                },
                () =>
                {
                    RestoreMediaConfiguration(changeLog);
                }
                );

        }

        protected void MultipleStreamTestReduced(
            MediaConfigurationChangeLog changeLog,
            GetStreamSetup getStreamSetup
            )
        {
            //3.	ONVIF Client will invoke GetVideoSourceConfigurationsRequest message to retrieve 
            // all DUT video source configurations.
            //4.	Verify the GetVideoSourceConfigurationsResponse message from the DUT.

            VideoSourceConfiguration[] sourceConfigurations = GetVideoSourceConfigurations();
            CheckConfigurationsList(sourceConfigurations, "Video Source Configuration");

            List<VideoSourceConfiguration> selectedConfigs = MediaTestUtils.SelectConfigurations(sourceConfigurations);

            foreach (VideoSourceConfiguration config in selectedConfigs)
            {

                //5.	ONVIF Client will invoke GetGuaranteedNumberOfVideoEncoderInstancesRequest message 
                // (ConfigurationToken = “VSCToken1”, where “VSCToken1” is a first video source configuration 
                // token from GetVideoSourceConfigurationsResponse message) to retrieve guaranteed number of 
                // video encoder instances per first video source configuration.
                int? jpeg;
                int? mpeg;
                int? h264;
                int totalNumber = GetGuaranteedNumberOfVideoEncoderInstances(config.token, out jpeg, out h264, out mpeg);

                //6.	Verify the GetGuaranteedNumberOfVideoEncoderInstancesResponse message from the DUT.
                //7.	Create or find number of profiles equal to TotalNumber from 
                // GetGuaranteedNumberOfVideoEncoderInstancesResponse message that contains video source 
                // configuration with token “VSCToken1” and video encoder configuration (see Annex A.9).
                List<Profile> profiles = GetProfilesForMultiStreamingTest(
                    config.token, 
                    totalNumber, 
                    jpeg,
                    mpeg,
                    h264,
                    changeLog);

                Assert(profiles.Count == totalNumber,
                    "Required number of profiles could not be found or created",
                    "Check that required number of profiles has been achieved");

                SetResourcesUsageToMimimal(changeLog, profiles);

                //8.	ONVIF Client invokes GetStreamUriRequest message (Profile Token, RTP-Unicast, 
                // UDP transport) to retrieve media stream URI for the first media profile from step 7.


                //9.	DUT sends RTSP URI and parameters defining the lifetime of the URI like 
                // ValidUntilConnect, ValidUntilReboot and Timeout in the GetStreamUriResponse message.
                //10.	ONVIF Client verifies the RTSP media stream URI provided by the DUT.
                //11.	ONVIF Client invokes RTSP DESCRIBE request.
                //12.	DUT sends 200 OK message and SDP information.
                //13.	 ONVIF Client invokes RTSP SETUP request with transport parameter as RTP/UDP.
                //14.	DUT sends 200 OK message and the media stream information.
                //15.	ONVIF Client invokes RTSP PLAY request.
                //16.	DUT sends 200 OK message and starts media streaming.
                //17.	DUT sends JPEG RTP media stream to ONVIF Client over UDP.
                //18.	DUT sends RTCP sender report to ONVIF Client.
                //19.	DUT validates the received RTP and RTCP packets, decodes and renders them.
                //20.	Repeat steps 8-20 to start video streaming for all profiles from step 7.
                //21.	ONVIF Client invokes RTSP TEARDOWN control request at the end of media streaming 
                // to terminate the RTSP session for each started stream.
                //22.	DUT sends 200 OK Response and terminates the RTSP Session.

                VideoUtils.ShowMultiple(
                    this, _semaphore.StopEvent, _username, _password, _messageTimeout,
                    getStreamSetup, _videoForm.NICIndex, profiles, GetStreamUri,
                    (a, name, failTest) => RunStep2(a, name, failTest),
                    (ex) => StepFailed(ex));

                //23.	Repeat steps 5-23 for the rest of video source configurations.
            }
        }

        [Test(Name = "MEDIA STREAMING – GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES (RTP-Unicast/UDP)",
           Path = PATH_VIDEO_U,
           Order = "01.01.27",
           Id = "1-1-27",
           Category = Category.RTSS,
           Version = 2.3,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, })]
        public void GuarenteedNumberOfVideoEncoderInstancesUdpReduced()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            RunTest(
                () =>
                {
                    MultipleStreamTestReduced(changeLog, (ref Profile profile) =>
                    {
                        StreamSetup streamSetup = new StreamSetup();
                        streamSetup.Transport = new Transport();
                        streamSetup.Transport.Protocol = TransportProtocol.UDP;
                        streamSetup.Stream = StreamType.RTPUnicast;
                        return streamSetup;
                    });
                },
                () =>
                {
                    RestoreMediaConfiguration(changeLog);
                }
                );

        }

        [Test(Name = "MEDIA STREAMING – GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES (RTP-Unicast/RTSP/HTTP/TCP)",
           Path = PATH_VIDEO_U,
           Order = "01.01.28",
           Id = "1-1-28",
           Category = Category.RTSS,
           Version = 2.3,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, })]
        public void GuarenteedNumberOfVideoEncoderInstancesHttpReduced()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            RunTest(
                () =>
                {
                    MultipleStreamTestReduced(changeLog, (ref Profile profile) =>
                    {
                        StreamSetup streamSetup = new StreamSetup();
                        streamSetup.Transport = new Transport();
                        streamSetup.Transport.Protocol = TransportProtocol.HTTP;
                        streamSetup.Stream = StreamType.RTPUnicast;
                        return streamSetup;
                    });
                },
                () =>
                {
                    RestoreMediaConfiguration(changeLog);
                }
                );

        }

        [Test(Name = "MEDIA STREAMING – GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES (RTP/RTSP/TCP)",
           Path = PATH_VIDEO_U,
           Order = "01.01.29",
           Id = "1-1-29",
           Category = Category.RTSS,
           Version = 2.3,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPRTSPTCP },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, })]
        public void GuarenteedNumberOfVideoEncoderInstancesTCPReduced()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            RunTest(
                () =>
                {
                    MultipleStreamTestReduced(changeLog, (ref Profile profile) =>
                    {
                        StreamSetup streamSetup = new StreamSetup();
                        streamSetup.Transport = new Transport();
                        streamSetup.Transport.Protocol = TransportProtocol.RTSP;
                        streamSetup.Stream = StreamType.RTPUnicast;
                        return streamSetup;
                    });
                },
                () =>
                {
                    RestoreMediaConfiguration(changeLog);
                }
                );

        }

        [Test(Name = "MEDIA STREAMING – GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES (MIX OF TRANSPORT TYPES)",
           Path = PATH_VIDEO_U,
           Order = "01.01.30",
           Id = "1-1-30",
           Category = Category.RTSS,
           Version = 2.3,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, })]
        public void GuarenteedNumberOfVideoEncoderInstancesMixReduced()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            // 4 StreamSetup types:
            // - RTPUDP - must be supported
            // - RTPRTSPHTTP - must be supported
            // - RTPRTSPTCP - optional
            // - RTPMulticastUDP - optional

            List<StreamSetup> supportedSetups = new List<StreamSetup>();

            StreamSetup streamSetup1 = new StreamSetup();
            streamSetup1.Transport = new Transport();
            streamSetup1.Transport.Protocol = TransportProtocol.UDP;
            streamSetup1.Stream = StreamType.RTPUnicast;
            supportedSetups.Add(streamSetup1);

            StreamSetup streamSetup2 = new StreamSetup();
            streamSetup2.Transport = new Transport();
            streamSetup2.Transport.Protocol = TransportProtocol.HTTP;
            streamSetup2.Stream = StreamType.RTPUnicast;
            supportedSetups.Add(streamSetup2);

            bool TCP = Features.Contains(Feature.RTPRTSPTCP);
            StreamSetup streamSetup3 = new StreamSetup();
            streamSetup3.Transport = new Transport();
            if (TCP)
            {
                streamSetup3.Transport.Protocol = TransportProtocol.RTSP;
                streamSetup3.Stream = StreamType.RTPUnicast;
                supportedSetups.Add(streamSetup3);
            }

            bool Multicast = Features.Contains(Feature.RTPMulticastUDP);
            StreamSetup streamSetup4 = new StreamSetup();
            streamSetup4.Transport = new Transport();
            if (Multicast)
            {
                streamSetup4.Transport.Protocol = TransportProtocol.UDP;
                streamSetup4.Stream = StreamType.RTPMulticast;
                supportedSetups.Add(streamSetup4);
            }

            // list of used VEC tokens for RTP-Multicast streaming
            List<string> MulticastTokenList = new List<string>();

            // list of used VSC tokens
            List<string> VscTokenList = new List<string>();

            // lists of used ip addresses and ports for RTP-Multicast streaming
            List<int> usedMulticastPorts = new List<int>();
            List<string> usedMulticastIPs = new List<string>();

            RunTest(
                () =>
                {
                    int step = 0;

                    MultipleStreamTestReduced(changeLog, (ref Profile profile) =>
                    {
                        if (!VscTokenList.Contains(profile.VideoSourceConfiguration.token))
                        {
                            VscTokenList.Add(profile.VideoSourceConfiguration.token);

                            // we should clear this for every new Video Source Configuration
                            if (0 != MulticastTokenList.Count)
                            {
                                MulticastTokenList.Clear();
                            }
                        }

                        int idx = step % supportedSetups.Count;
                        StreamSetup current = supportedSetups[idx];

                        // setup multicast
                        if (current.Stream == StreamType.RTPMulticast)
                        {
                            if (MulticastTokenList.Contains(profile.VideoEncoderConfiguration.token))
                            {
                                idx = (++step) % supportedSetups.Count;
                                current = supportedSetups[idx];
                            }
                            else
                            {
                                MulticastTokenList.Add(profile.VideoEncoderConfiguration.token);

                                string addressVideo = "";
                                int portVideo = 0;

                                string addressAudio = "";
                                int portAudio = 0;

                                string addressMetadata = "";
                                int portMetadata = 0;

                                if (profile.VideoEncoderConfiguration != null)
                                {
                                    addressVideo = GetMulticastAddress3(usedMulticastIPs);
                                    usedMulticastIPs.Add(addressVideo);
                                    portVideo = GetMulticastPort2(usedMulticastPorts);
                                    usedMulticastPorts.Add(portVideo);

                                    VideoEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(profile.VideoEncoderConfiguration);
                                    changeLog.TrackModifiedConfiguration(configCopy);
                                    SetMulticast(profile.VideoEncoderConfiguration.Multicast, IPType.IPv4, addressVideo, portVideo);
                                    SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, true);
                                }

                                if (profile.AudioEncoderConfiguration != null)
                                {
                                    addressAudio = GetMulticastAddress3(usedMulticastIPs);
                                    usedMulticastIPs.Add(addressAudio);
                                    portAudio = GetMulticastPort2(usedMulticastPorts);
                                    usedMulticastPorts.Add(portAudio);
                                }

                                if (profile.MetadataConfiguration != null)
                                {
                                    addressMetadata = GetMulticastAddress3(usedMulticastIPs);
                                    usedMulticastIPs.Add(addressMetadata);
                                    portMetadata = GetMulticastPort2(usedMulticastPorts);
                                    usedMulticastPorts.Add(portMetadata);
                                }

                                SetMulticastSettings(profile, IPType.IPv4, changeLog,
                                    addressAudio, portAudio,
                                    addressVideo, portVideo,
                                    addressMetadata, portMetadata);
                            }
                        }

                        ++step;
                        return current;
                    });
                },
                () =>
                {
                    RestoreMediaConfiguration(changeLog);
                }
                );
        }

        [Test(Name = "MEDIA STREAMING – GUARANTEED NUMBER OF VIDEO ENCODER INSTANCES (RTP-Multicast/UDP)",
           Path = PATH_VIDEO_M,
           Order = "01.02.12",
           Id = "1-2-12",
           Category = Category.RTSS,
           Version = 2.3,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, })]
        public void GuarenteedNumberOfVideoEncoderInstancesMulticastReduced()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            RunTest(
                () =>
                {
                    // list of used VEC tokens for RTP-Multicast streaming
                    List<string> MulticastTokenList = new List<string>();

                    // list of used VSC tokens
                    List<string> VscTokenList = new List<string>();

                    // lists of used ip addresses and ports for RTP-Multicast streaming
                    List<int> usedMulticastPorts = new List<int>();
                    List<string> usedMulticastIPs = new List<string>();

                    MultipleStreamTestReduced(changeLog, (ref Profile profile) =>
                    {
                        if (!VscTokenList.Contains(profile.VideoSourceConfiguration.token))
                        {
                            VscTokenList.Add(profile.VideoSourceConfiguration.token);

                            // we should clear this for every new Video Source Configuration
                            if (0 != MulticastTokenList.Count)
                            {
                                MulticastTokenList.Clear();
                            }
                        }

                        StreamSetup streamSetup = new StreamSetup();
                        streamSetup.Transport = new Transport();
                        streamSetup.Transport.Protocol = TransportProtocol.UDP;
                        streamSetup.Stream = StreamType.RTPMulticast;

                        if (MulticastTokenList.Contains(profile.VideoEncoderConfiguration.token))
                        {
                            profile = null;
                        }
                        else
                        {
                            MulticastTokenList.Add(profile.VideoEncoderConfiguration.token);

                            string addressVideo = "";
                            int portVideo = 0;

                            string addressAudio = "";
                            int portAudio = 0;

                            string addressMetadata = "";
                            int portMetadata = 0;

                            if (profile.VideoEncoderConfiguration != null)
                            {
                                addressVideo = GetMulticastAddress3(usedMulticastIPs);
                                usedMulticastIPs.Add(addressVideo);
                                portVideo = GetMulticastPort2(usedMulticastPorts);
                                usedMulticastPorts.Add(portVideo);

                                VideoEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(profile.VideoEncoderConfiguration);
                                changeLog.TrackModifiedConfiguration(configCopy);
                                SetMulticast(profile.VideoEncoderConfiguration.Multicast, IPType.IPv4, addressVideo, portVideo);
                                SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, true);
                            }

                            if (profile.AudioEncoderConfiguration != null)
                            {
                                addressAudio = GetMulticastAddress3(usedMulticastIPs);
                                usedMulticastIPs.Add(addressAudio);
                                portAudio = GetMulticastPort2(usedMulticastPorts);
                                usedMulticastPorts.Add(portAudio);
                            }

                            if (profile.MetadataConfiguration != null)
                            {
                                addressMetadata = GetMulticastAddress3(usedMulticastIPs);
                                usedMulticastIPs.Add(addressMetadata);
                                portMetadata = GetMulticastPort2(usedMulticastPorts);
                                usedMulticastPorts.Add(portMetadata);
                            }

                            SetMulticastSettings(profile, IPType.IPv4, changeLog,
                                addressAudio, portAudio,
                                addressVideo, portVideo,
                                addressMetadata, portMetadata);
                        }

                        return streamSetup;
                    });
                },
                () =>
                {
                    RestoreMediaConfiguration(changeLog);
                }
                );
        }
    }

}
