using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites.RTSS
{
    [TestClass]
    class VideoResolutionTestSuite : MediaConfigurationTestSuiteBase
    {
        const string PATH_VIDEO_U = "Real Time Streaming\\Video Streaming\\Unicast";

        public VideoResolutionTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        protected delegate VideoResolution[] GetResolutionsAvailable(VideoEncoderConfigurationOptions options);

        protected virtual void ValidateStreamSequence(bool CheckAudio, bool CheckVideo)
        {
            _videoForm.EventSink = this;
            VideoIsOpened = true;
            _videoForm.OpenWindow(CheckAudio, CheckVideo);

            _videoForm.CloseWindow();
            VideoIsOpened = false;
            _videoForm.EventSink = null;
        }

        protected void ResolutionTest(
            MediaConfigurationChangeLog changeLog,
            VideoEncoding encodingUnderTest,
            GetResolutionsAvailable getResolutionsAvailable)
        {
            //3.	ONVIF Client invokes GetVideoEncoderConfigurationsRequest message to retrieve 
            // video configuration list.
            //4.	Verify the GetVideoEncoderConfigurationsResponse message.

            VideoEncoderConfiguration[] encoderConfigurations = GetVideoEncoderConfigurations();
            CheckVideoEncoderConfigurationsList(encoderConfigurations);
            bool NoProfilesForEncoding = true;

            List<VideoEncoderConfiguration> selectedConfigs = null;
            var WhereRes = encoderConfigurations.Where(C => C.Encoding == encodingUnderTest);
            if (WhereRes != null)
            {
              selectedConfigs = WhereRes.ToList();
            }
            if (selectedConfigs == null || selectedConfigs.Count == 0)
            {
              LogTestEvent("There are no VideoEncoderConfiguration ready for selected encoder type - will try to reconfigure (if this may fail - please pre-configure before making tests)." + Environment.NewLine);
              selectedConfigs = encoderConfigurations.ToList();
            }
            selectedConfigs = MediaTestUtils.SelectConfigurations(selectedConfigs);

            var configGroups = encoderConfigurations.Where(e => !selectedConfigs.Contains(e)).Select(e => new List<VideoEncoderConfiguration>() { e }).ToList();
            configGroups.Insert(0, selectedConfigs);

            //Try to perform steps for selected profiles.
            //In case of fail for all selected profiles try to perform steps for each another profile until first success.
            foreach (var configGroup in configGroups)
            {
                foreach (VideoEncoderConfiguration encoderConfig in configGroup)
                {

                    //5.	Find or create media profile with Video Source Configuration and Video Encoder 
                    // Configuration with token VECToken1 and supporting of JPEG encoding, where VECToken1 
                    // is first video encoder configuration token from GetVideoEncoderConfigurationsResponse 
                    // message (see Annex A.8). If it is not possible skip steps 6-61 and go to the step 62.

                    Profile profile = GetProfileForSpecificConfigurationAndCodec(encoderConfig.token, encodingUnderTest, changeLog);

                    if (profile == null)
                    {
                        continue;
                    }
                    NoProfilesForEncoding = false;

                    //6.	ONVIF Client invokes GetVideoEncoderConfigurationOptionsRequest message 
                    // (ProfileToken = “Profile1”, where “Profile1” is profile token from the step 5) 
                    // to get video encoder configuration options.
                    //7.	Verify the GetVideoEncoderConfigurationOptionsResponse message from the DUT.

                    VideoEncoderConfigurationOptions options = GetVideoEncoderConfigurationOptions(null, profile.token);

                    Assert(CheckVideoSupport(options, encodingUnderTest),
                           string.Format("{0} encoding is not compatible with current configurations",
                                         GetVideoCodecName(encodingUnderTest)),
                           string.Format("Validate {0} options",
                                         GetVideoCodecName(encodingUnderTest)));

                    VideoResolution highest = null;
                    VideoResolution lowest = null;
                    VideoResolution median = null;

                    FindResolutions(getResolutionsAvailable(options), out highest, out lowest, out median);

                    //8.	ONVIF Client invokes SetVideoEncoderConfigurationRequest message 
                    // (ConfigurationToken = VECToken1, Resolution = [Width1, Height1], Encoding = JPEG, 
                    // where [Width1, Height1] is maximum resolution from the Options.JPEG.ResolutionsAvailable) 
                    // to change video encoder configuration.

                    VideoEncoderConfiguration unchanged = Utils.CopyMaker.CreateCopy(encoderConfig);

                    encoderConfig.Encoding = encodingUnderTest;
                    encoderConfig.Resolution = highest;

                    AdjustVideoEncoderConfiguration(encodingUnderTest, encoderConfig, options);
                    switch (encodingUnderTest)
                    {
                        case VideoEncoding.MPEG4:
                            AdjustMpeg4VideoEncoderConfiguration(encoderConfig, options, false);
                            break;
                        case VideoEncoding.H264:
                            AdjustH264VideoEncoderConfiguration(encoderConfig, options, false);
                            break;
                    }

                    SetVideoEncoderConfiguration(encoderConfig, false);

                    //9.	Verify the SetVideoEncoderConfigurationResponse message from the DUT.

                    changeLog.ModifiedVideoEncoderConfigurations.Add(unchanged);

                    //10.	ONVIF Client invokes GetVideoEncoderConfigurationRequest message 
                    // (ConfigurationToken = VECToken1) to get video encoder configuration.
                    //11.	Verify the GetVideoEncoderConfigurationResponse message (ConfigurationToken = 
                    // VECToken1, Resolution = [Width1, Height1], Encoding = JPEG) from the DUT. Check 
                    // that new setting for Resolution and Encoding was applied.

                    VideoEncoderConfiguration actual = GetVideoEncoderConfiguration(unchanged.token);

                    string reason = string.Empty;
                    // check encoding and resolutions
                    bool ok = ConfigurationValid(actual, encodingUnderTest, highest, out reason);
                    Assert(ok, reason, "Check that the DUT accepted values passed");

                    //12.	ONVIF Client invokes GetStreamUriRequest message (Profile Token, RTP-Unicast, 
                    // UDP transport) to retrieve media stream URI for the selected media profile.
                    //13.	DUT sends RTSP URI and parameters defining the lifetime of the URI like 
                    // ValidUntilConnect, ValidUntilReboot and Timeout in the GetStreamUriResponse message.
                    StreamSetup streamSetup = new StreamSetup();
                    streamSetup.Transport = new Transport();
                    streamSetup.Transport.Protocol = TransportProtocol.UDP;
                    streamSetup.Stream = StreamType.RTPUnicast;

                    MediaUri streamUri = GetStreamUri(streamSetup, profile.token);

                    //14.	ONVIF Client verifies the RTSP media stream URI provided by the DUT.
                    //15.	ONVIF Client invokes RTSP DESCRIBE request.
                    //16.	DUT sends 200 OK message and SDP information.
                    //17.	 ONVIF Client invokes RTSP SETUP request with transport parameter as RTP/UDP.
                    //18.	DUT sends 200 OK message and the media stream information.
                    //19.	ONVIF Client invokes RTSP PLAY request.
                    //20.	DUT sends 200 OK message and starts media streaming.
                    //21.	DUT sends JPEG RTP media stream to ONVIF Client over UDP. Verify that stream 
                    // has JPEG encoding and [Width1, Height1] resolution.
                    //22.	DUT sends RTCP sender report to ONVIF Client.
                    //23.	DUT validates the received RTP and RTCP packets, decodes and renders them.
                    //24.	ONVIF Client invokes RTSP TEARDOWN control request at the end of media 
                    // streaming to terminate the RTSP session.
                    //25.	DUT sends 200 OK Response and terminates the RTSP Session.
                    TestTool.Tests.Common.Media.VideoUtils.AdjustVideo(_videoForm, _username, _password, _messageTimeout,
                                                                       streamSetup.Transport.Protocol,
                                                                       streamSetup.Stream, streamUri, encoderConfig);
                    ValidateStreamSequence(false, true);

                    //26.	ONVIF Client invokes SetVideoEncoderConfigurationRequest message 
                    // (ConfigurationToken = VECToken1, Resolution = [Width1, Height1], Encoding = JPEG, 
                    // where [Width2, Height2] is minimum resolution from the Options.JPEG.ResolutionsAvailable) to change video encoder configuration.
                    //27.	Verify the SetVideoEncoderConfigurationResponse message from the DUT.
                    encoderConfig.Encoding = encodingUnderTest;
                    encoderConfig.Resolution = lowest;

                    AdjustVideoEncoderConfiguration(encodingUnderTest, encoderConfig, options);

                    SetVideoEncoderConfiguration(encoderConfig, false);

                    //28.	ONVIF Client invokes GetVideoEncoderConfigurationRequest message 
                    // (ConfigurationToken = VECToken1) to get video encoder configuration.
                    //29.	Verify the GetVideoEncoderConfigurationResponse message (ConfigurationToken = 
                    // VECToken1, Resolution = [Width2, Height2], Encoding = JPEG, where [Width2, Height2]) from the DUT. Check that new setting for Resolution and Encoding was applied.
                    actual = GetVideoEncoderConfiguration(unchanged.token);
                    ok = ConfigurationValid(actual, encodingUnderTest, lowest, out reason);
                    Assert(ok, reason, "Check that the DUT accepted values passed");

                    //30.	ONVIF Client invokes GetStreamUriRequest message (Profile Token, RTP-Unicast,
                    // UDP transport) to retrieve media stream URI for the selected media profile.
                    //31.	DUT sends RTSP URI and parameters defining the lifetime of the URI like ValidUntilConnect, ValidUntilReboot and Timeout in the GetStreamUriResponse message.

                    streamUri = GetStreamUri(streamSetup, profile.token);

                    //32.	ONVIF Client verifies the RTSP media stream URI provided by the DUT.
                    //33.	ONVIF Client invokes RTSP DESCRIBE request.
                    //34.	DUT sends 200 OK message and SDP information.
                    //35.	 ONVIF Client invokes RTSP SETUP request with transport parameter as RTP/UDP.
                    //36.	DUT sends 200 OK message and the media stream information.
                    //37.	ONVIF Client invokes RTSP PLAY request.
                    //38.	DUT sends 200 OK message and starts media streaming.
                    //39.	DUT sends JPEG RTP media stream to ONVIF Client over UDP. Verify that stream has JPEG encoding and [Width2, Height2] resolution.
                    //40.	DUT sends RTCP sender report to ONVIF Client.
                    //41.	DUT validates the received RTP and RTCP packets, decodes and renders them.
                    //42.	ONVIF Client invokes RTSP TEARDOWN control request at the end of media streaming to terminate the RTSP session.
                    //43.	DUT sends 200 OK Response and terminates the RTSP Session.
                    TestTool.Tests.Common.Media.VideoUtils.AdjustVideo(_videoForm, _username, _password, _messageTimeout,
                                                                       streamSetup.Transport.Protocol,
                                                                       streamSetup.Stream, streamUri, encoderConfig);
                    ValidateStreamSequence(false, true);

                    //44.	ONVIF Client invokes SetVideoEncoderConfigurationRequest message (ConfigurationToken = VECToken1, Resolution = [Width1, Height1], Encoding = JPEG, where [Width3, Height3] is middle resolution from the Options.JPEG.ResolutionsAvailable) to change video encoder configuration.
                    //45.	Verify the SetVideoEncoderConfigurationResponse message from the DUT.
                    encoderConfig.Encoding = encodingUnderTest;
                    encoderConfig.Resolution = median;

                    AdjustVideoEncoderConfiguration(encodingUnderTest, encoderConfig, options);

                    SetVideoEncoderConfiguration(encoderConfig, false);

                    //46.	ONVIF Client invokes GetVideoEncoderConfigurationRequest message (ConfigurationToken = VECToken1) to get video encoder configuration.
                    //47.	Verify the GetVideoEncoderConfigurationResponse message (ConfigurationToken = VECToken1, Resolution = [Width3, Height3], Encoding = JPEG) from the DUT. Check that new setting for Resolution and Encoding was applied.

                    actual = GetVideoEncoderConfiguration(unchanged.token);
                    ok = ConfigurationValid(actual, encodingUnderTest, median, out reason);
                    Assert(ok, reason, "Check that the DUT accepted values passed");

                    //48.	ONVIF Client invokes GetStreamUriRequest message (Profile Token, RTP-Unicast, UDP transport) to retrieve media stream URI for the selected media profile.
                    //49.	DUT sends RTSP URI and parameters defining the lifetime of the URI like ValidUntilConnect, ValidUntilReboot and Timeout in the GetStreamUriResponse message.
                    streamUri = GetStreamUri(streamSetup, profile.token);

                    //50.	ONVIF Client verifies the RTSP media stream URI provided by the DUT.
                    //51.	ONVIF Client invokes RTSP DESCRIBE request.
                    //52.	DUT sends 200 OK message and SDP information.
                    //53.	 ONVIF Client invokes RTSP SETUP request with transport parameter as RTP/UDP.
                    //54.	DUT sends 200 OK message and the media stream information.
                    //55.	ONVIF Client invokes RTSP PLAY request.
                    //56.	DUT sends 200 OK message and starts media streaming.
                    //57.	DUT sends JPEG RTP media stream to ONVIF Client over UDP. Verify that stream has JPEG encoding and [Width3, Height3] resolution.
                    //58.	DUT sends RTCP sender report to ONVIF Client.
                    //59.	DUT validates the received RTP and RTCP packets, decodes and renders them.
                    //60.	ONVIF Client invokes RTSP TEARDOWN control request at the end of media streaming to terminate the RTSP session.
                    //61.	DUT sends 200 OK Response and terminates the RTSP Session.
                    TestTool.Tests.Common.Media.VideoUtils.AdjustVideo(_videoForm, _username, _password, _messageTimeout,
                                                                       streamSetup.Transport.Protocol,
                                                                       streamSetup.Stream, streamUri, encoderConfig);
                    ValidateStreamSequence(false, true);

                    //62.	Repeat steps 5-62 for the rest Video Encoder configurations supported by the DUT with using different multicast ports and the same multicast addresses for Video Encoder Configurations.

                }

                if (!NoProfilesForEncoding)
                    break;
            }

            if (NoProfilesForEncoding)
            {
                RunStep(() =>
                {
                    throw new Exception(string.Format("No profiles for {0}", GetVideoCodecName(encodingUnderTest)));
                }, string.Format("Check if at least one profile were found ({0} not supported?)", GetVideoCodecName(encodingUnderTest)));
            }
        }


        [Test(Name = "VIDEO ENCODER CONFIGURATION – JPEG RESOLUTION",
           Path = PATH_VIDEO_U,
           Order = "01.01.46",
           Id = "1-1-46",
           Category = Category.RTSS,
           Version = 2.2,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void VideoEncoderConfigurationsJPEGResolutionTest()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
                () =>
                {
                    _videoForm.CheckActualResolution = true;
                    _videoForm.VideoCodecName = "JPEG";

                    VideoEncoding encodingUnderTest = VideoEncoding.JPEG;
                    GetResolutionsAvailable getResolutionsAvailable = (options) =>
                    {
                        return options.JPEG.ResolutionsAvailable;
                    };

                    ResolutionTest(changeLog, encodingUnderTest, 
                        (options) => { return options.JPEG.ResolutionsAvailable; });
                },
                () =>
                {
                    try
                    {
                        _videoForm.CheckActualResolution = false;
                        VideoCleanup();
                    }
                    finally
                    {
                        RestoreMediaConfiguration(changeLog);
                    }
                }
                );
        }

        [Test(Name = "VIDEO ENCODER CONFIGURATION – MPEG4 RESOLUTION",
           Path = PATH_VIDEO_U,
           Order = "01.01.47",
           Id = "1-1-47",
           Category = Category.RTSS,
           Version = 2.2,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.MPEG4 },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void VideoEncoderConfigurationsMPEG4ResolutionTest()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
                () =>
                {
                    _videoForm.CheckActualResolution = true;
                    _videoForm.VideoCodecName = "MPEG4";

                    VideoEncoding encodingUnderTest = VideoEncoding.MPEG4;
                    GetResolutionsAvailable getResolutionsAvailable = (options) =>
                    {
                        return options.MPEG4.ResolutionsAvailable;
                    };

                    ResolutionTest(changeLog, encodingUnderTest,
                        getResolutionsAvailable);
                },
                () =>
                {
                    try
                    {
                        _videoForm.CheckActualResolution = false;
                        VideoCleanup();
                    }
                    finally
                    {
                        RestoreMediaConfiguration(changeLog);
                    }
                }
                );
        }

        [Test(Name = "VIDEO ENCODER CONFIGURATION – H.264 RESOLUTION",
           Path = PATH_VIDEO_U,
           Order = "01.01.48",
           Id = "1-1-48",
           Category = Category.RTSS,
           Version = 2.2,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.H264 },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void VideoEncoderConfigurationsH264ResolutionTest()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
                () =>
                {
                    _videoForm.CheckActualResolution = true;
                    _videoForm.VideoCodecName = "H264";

                    VideoEncoding encodingUnderTest = VideoEncoding.H264;
                    GetResolutionsAvailable getResolutionsAvailable = (options) =>
                    {
                        return options.H264.ResolutionsAvailable;
                    };

                    ResolutionTest(changeLog, encodingUnderTest,
                        getResolutionsAvailable);
                },
                () =>
                {
                    try
                    {
                        _videoForm.CheckActualResolution = false;
                        VideoCleanup();
                    }
                    finally
                    {
                        RestoreMediaConfiguration(changeLog);
                    }
                }
                );
        }

        protected void AllResolutionsTest(MediaConfigurationChangeLog changeLog,
                                          VideoEncoding encodingUnderTest,
                                          GetResolutionsAvailable getResolutionsAvailable)
        {
            //3.	ONVIF Client invokes GetVideoEncoderConfigurationsRequest message to retrieve 
            // video configuration list.
            //4.	Verify the GetVideoEncoderConfigurationsResponse message.

            VideoEncoderConfiguration[] encoderConfigurations = GetVideoEncoderConfigurations();
            CheckVideoEncoderConfigurationsList(encoderConfigurations);
            bool NoProfilesForEncoding = true;

            foreach (VideoEncoderConfiguration encoderConfig in encoderConfigurations)
            {
                //5.	Find or create media profile with Video Source Configuration and 
                // Video Encoder Configuration with token VECToken1 and supporting of JPEG encoding, 
                // where VECToken1 is first video encoder configuration token from 
                // GetVideoEncoderConfigurationsResponse message (see Annex A.14). If it is not possible 
                // skip steps 6-26 and go to the step 27.
                Profile profile = GetProfileForSpecificConfigurationAndCodec(
                    encoderConfig.token, encodingUnderTest, changeLog);

                if (profile == null)
                {
                    continue;
                }
                if (NoProfilesForEncoding)
                {
                    NoProfilesForEncoding = false;
                }

                //6.	ONVIF Client invokes GetVideoEncoderConfigurationOptionsRequest message 
                // (ProfileToken = “Profile1”, where “Profile1” is profile token from the step 5) 
                // to get video encoder configuration options.
                //7.	Verify the GetVideoEncoderConfigurationOptionsResponse message from the DUT.
                VideoEncoderConfigurationOptions options = GetVideoEncoderConfigurationOptions(
                    null, profile.token);

                Assert(CheckVideoSupport(options, encodingUnderTest), 
                    string.Format("{0} encoding is not compatible with current configuration", 
                    GetVideoCodecName(encodingUnderTest)), 
                    string.Format("Validate {0} options", GetVideoCodecName(encodingUnderTest)));

                VideoResolution[] ResolutionsAvailable = getResolutionsAvailable(options);

                VideoEncoderConfiguration unchanged = Utils.CopyMaker.CreateCopy(encoderConfig);
                bool addToChangeLog = true;

                foreach (VideoResolution resolution in ResolutionsAvailable)
                {
                    //8.	ONVIF Client invokes SetVideoEncoderConfigurationRequest message 
                    // (ConfigurationToken = VECToken1, Resolution = [Width1, Height1], Encoding = JPEG, 
                    // where [Width1, Height1] is the first resolution 
                    // from the Options.JPEG.ResolutionsAvailable) to change video encoder configuration.
                    //9.	Verify the SetVideoEncoderConfigurationResponse message from the DUT.

                    encoderConfig.Encoding = encodingUnderTest;
                    encoderConfig.Resolution = resolution;

                    AdjustVideoEncoderConfiguration(encodingUnderTest, encoderConfig, options);
                    switch (encodingUnderTest)
                    {
                        case VideoEncoding.MPEG4:
                            AdjustMpeg4VideoEncoderConfiguration(encoderConfig, options, false);
                            break;
                        case VideoEncoding.H264:
                            AdjustH264VideoEncoderConfiguration(encoderConfig, options, false);
                            break;
                    }

                    SetVideoEncoderConfiguration(encoderConfig, false);

                    if (addToChangeLog)
                    {
                        changeLog.ModifiedVideoEncoderConfigurations.Add(unchanged);
                        addToChangeLog = false;
                    }


                    //10.	ONVIF Client invokes GetVideoEncoderConfigurationRequest message 
                    // (ConfigurationToken = VECToken1) to get video encoder configuration.
                    //11.	Verify the GetVideoEncoderConfigurationResponse message 
                    //(ConfigurationToken = VECToken1, Resolution = [Width1, Height1], Encoding = JPEG) 
                    // from the DUT. Check that new setting for Resolution and Encoding was applied.

                    VideoEncoderConfiguration actual = GetVideoEncoderConfiguration(unchanged.token);

                    string reason = string.Empty;
                    // check encoding and resolutions
                    bool ok = ConfigurationValid(actual, encodingUnderTest, resolution, out reason);
                    Assert(ok, reason, "Check that the DUT accepted values passed");

                    //12.	ONVIF Client invokes GetStreamUriRequest message (Profile Token, RTP-Unicast, 
                    // UDP transport) to retrieve media stream URI for the selected media profile.
                    //13.	DUT sends RTSP URI and parameters defining the lifetime of the URI like 
                    // ValidUntilConnect, ValidUntilReboot and Timeout in the GetStreamUriResponse message.

                    StreamSetup streamSetup = new StreamSetup();
                    streamSetup.Transport = new Transport();
                    streamSetup.Transport.Protocol = TransportProtocol.UDP;
                    streamSetup.Stream = StreamType.RTPUnicast;

                    MediaUri streamUri = GetStreamUri(streamSetup, profile.token);

                    //14.	ONVIF Client verifies the RTSP media stream URI provided by the DUT.
                    //15.	ONVIF Client invokes RTSP DESCRIBE request.
                    //16.	DUT sends 200 OK message and SDP information.
                    //17.	 ONVIF Client invokes RTSP SETUP request with transport parameter as RTP/UDP.
                    //18.	DUT sends 200 OK message and the media stream information.
                    //19.	ONVIF Client invokes RTSP PLAY request.
                    //20.	DUT sends 200 OK message and starts media streaming.
                    //21.	DUT sends JPEG RTP media stream to ONVIF Client over UDP. Verify that stream has JPEG encoding and [Width1, Height1] resolution.
                    //22.	DUT sends RTCP sender report to ONVIF Client.
                    //23.	DUT validates the received RTP and RTCP packets, decodes and renders them.
                    //24.	ONVIF Client invokes RTSP TEARDOWN control request at the end of media streaming to terminate the RTSP session.
                    //25.	DUT sends 200 OK Response and terminates the RTSP Session.
                    TestTool.Tests.Common.Media.VideoUtils.AdjustVideo(_videoForm, _username, 
                        _password, _messageTimeout, streamSetup.Transport.Protocol, streamSetup.Stream, 
                        streamUri, encoderConfig);
                    ValidateStreamSequence(false, true);
                }
            }

            if (NoProfilesForEncoding)
            {
                RunStep(() =>
                {
                    throw new Exception(string.Format("No profiles for {0}", GetVideoCodecName(encodingUnderTest)));
                }, string.Format("Check if at least one profile were found ({0} not supported?)", GetVideoCodecName(encodingUnderTest)));
            }
        }

        [Test(Name = "VIDEO ENCODER CONFIGURATION – JPEG RESOLUTION (ALL RESOLUTIONS)",
           Path = PATH_VIDEO_U,
           Order = "01.01.24",
           Id = "1-1-24",
           Category = Category.RTSS,
           Version = 2.3,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void AllJPEGResolutionsTest()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
                () =>
                {
                    _videoForm.CheckActualResolution = true;
                    _videoForm.VideoCodecName = "JPEG";

                    VideoEncoding encodingUnderTest = VideoEncoding.JPEG;
                    GetResolutionsAvailable getResolutionsAvailable = (options) =>
                    {
                        return options.JPEG.ResolutionsAvailable;
                    };

                    AllResolutionsTest(changeLog, encodingUnderTest, 
                        (options) => { return options.JPEG.ResolutionsAvailable; });
                },
                () =>
                {
                    try
                    {
                        _videoForm.CheckActualResolution = false;
                        VideoCleanup();
                    }
                    finally
                    {
                        RestoreMediaConfiguration(changeLog);
                    }
                }
                );
        }

        [Test(Name = "VIDEO ENCODER CONFIGURATION – MPEG4 RESOLUTION (ALL RESOLUTIONS)",
           Path = PATH_VIDEO_U,
           Order = "01.01.25",
           Id = "1-1-25",
           Category = Category.RTSS,
           Version = 2.3,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.MPEG4 },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })]
        public void AllMPEG4ResolutionsTest()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
                () =>
                {
                    _videoForm.CheckActualResolution = true;
                    _videoForm.VideoCodecName = "MPEG4";

                    VideoEncoding encodingUnderTest = VideoEncoding.MPEG4;
                    GetResolutionsAvailable getResolutionsAvailable = (options) =>
                    {
                        return options.MPEG4.ResolutionsAvailable;
                    };

                    AllResolutionsTest(changeLog, encodingUnderTest, 
                        getResolutionsAvailable);
                },
                () =>
                {
                    try
                    {
                        _videoForm.CheckActualResolution = false;
                        VideoCleanup();
                    }
                    finally
                    {
                        RestoreMediaConfiguration(changeLog);
                    }
                }
                );
        }

        [Test(Name = "VIDEO ENCODER CONFIGURATION – H.264 RESOLUTION (ALL RESOLUTIONS)",
           Path = PATH_VIDEO_U,
           Order = "01.01.26",
           Id = "1-1-26",
           Category = Category.RTSS,
           Version = 2.3,
           RequirementLevel = RequirementLevel.Optional,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.H264 },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })
        ]
        public void AllH264ResolutionsTest()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
                () =>
                {
                    _videoForm.CheckActualResolution = true;
                    _videoForm.VideoCodecName = "H264";

                    VideoEncoding encodingUnderTest = VideoEncoding.H264;
                    GetResolutionsAvailable getResolutionsAvailable = (options) =>
                    {
                        return options.H264.ResolutionsAvailable;
                    };

                    AllResolutionsTest(changeLog, encodingUnderTest, 
                        getResolutionsAvailable);
                },
                () =>
                {
                    try
                    {
                        _videoForm.CheckActualResolution = false;
                        VideoCleanup();
                    }
                    finally
                    {
                        RestoreMediaConfiguration(changeLog);
                    }
                }
                );
        }

    }
}
