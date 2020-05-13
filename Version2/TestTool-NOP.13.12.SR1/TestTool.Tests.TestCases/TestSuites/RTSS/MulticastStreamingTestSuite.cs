using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Media;

namespace TestTool.Tests.TestCases.TestSuites.RTSS
{
    [TestClass]
    class MulticastStreamingTestSuite : MediaConfigurationTestSuiteBase
    {
        public MulticastStreamingTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        const string PATH_AUDIO_M = "Real Time Streaming\\Audio Streaming\\Multicast";
        const string PATH_VIDEO_M = "Real Time Streaming\\Video Streaming\\Multicast";
        const string PATH_AUDIO_VIDEO_M = "Real Time Streaming\\Audio & Video Streaming\\Multicast";
        
        [Test(Name = "AUDIO ENCODER CONFIGURATION – MULTICAST PORT (IPv4)",
               Path = PATH_AUDIO_M,
               Order = "02.02.09",
               Id = "2-2-9",
               Category = Category.RTSS,
               Version = 2.2,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming })]
        public void AudioEncoderConfigurationMulticastPortIpv4Test()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            List<int> usedMulticastPorts = new List<int>();
            List<string> usedMulticastIPs = new List<string>();
            Profile prof = null;

            RunTest(
                () =>
                {
                    //3.	ONVIF Client invokes GetAudioEncoderConfigurationsRequest message to 
                    // retrieve audio configuration list.
                    //4.	Verify the GetAudioEncoderConfigurationsResponse message.

                    AudioEncoderConfiguration[] encoderConfigurations = GetAudioEncoderConfigurations();
                    CheckAudioEncoderConfigurationsList(encoderConfigurations);

                    // Use the same address for all configurations
                    string multicastAddress = GetMulticastAddress2(usedMulticastIPs);
                    usedMulticastIPs.Add(multicastAddress);

                    //List<AudioEncoderConfiguration> selectedConfigs = MediaTestUtils.SelectConfigurations(encoderConfigurations);

                    foreach (AudioEncoderConfiguration aec in encoderConfigurations)
                    {
                        AudioEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(aec);

                        //5.	ONVIF Client invokes SetAudioEncoderConfigurationRequest message 
                        //(ConfigurationToken = AECToken1, Multicast.Address = [“IPv4”, “multicastAddress1”], 
                        // Multicast.Port = “port1”, where AECToken1 is first audio encoder configuration 
                        // token from GetAudioEncoderConfigurationsResponse message) 
                        // to change multicast port and address for audio encoder configuration.

                        if (aec.Multicast == null)
                        {
                            aec.Multicast = new MulticastConfiguration();
                        }

                        // Use different ports
                        int multicastPort = GetMulticastPort2(usedMulticastPorts);
                        usedMulticastPorts.Add(multicastPort);

                        SetMulticast(aec.Multicast, IPType.IPv4, multicastAddress, multicastPort);

                        SetAudioEncoderConfiguration(aec, false, true);
                        changeLog.TrackModifiedConfiguration(configCopy);
                        
                        //6.	Verify the SetAudioEncoderConfigurationResponse message from the DUT.
                        
                        // the only error possible is fault - in this case we will no go here.

                        //7.	ONVIF Client invokes GetAudioEncoderConfigurationRequest message 
                        // (ConfigurationToken = AECToken1) to get audio encoder configuration.
                        AudioEncoderConfiguration actual = GetAudioEncoderConfiguration(aec.token);

                        //8.	Verify the GetAudioEncoderConfigurationResponse message 
                        // (ConfigurationToken = Token1, Multicast.Address = [“IPv4”, “multicastAddress1”], 
                        // Multicast.Port = “port1”) from the DUT. Check that new setting for Multicast.Port 
                        // and Multicast.Address was applied.
                        CheckMulticastSettings(aec.Multicast, actual.Multicast);
                       
                        //9.	Repeat steps 5-8 for the rest Audio Encoder configurations supported by the 
                        // DUT with using different multicast ports and the same multicast addresses for 
                        // Audio Encoder Configurations.                  
                    }

                    bool streamingTested = false;
                    foreach (AudioEncoderConfiguration aec in encoderConfigurations)
                    {
                        //10.	Find or create media profile with Audio Source Configuration and Audio Encoder 
                        // Configuration with token AECToken1 (see Annex A.5). If it is not possible skip steps 
                        // 11-18  and go to the step 19.

                        Profile profile = GetProfileWithAudioEncoderConfiguration(aec.token, changeLog);
                        prof = profile;
                        // "impossible" can mean FAIL or "don't test for this AEC" - ToDo...

                        // FAIL. Other solutions are 'continue' (do for other AECs) or 'break' 
                        // (goto step 20, as in specification)
                        if (profile == null)
                        {
                            continue;
                        }

                        // to check "It is not possible to find or create profile for all Audio 
                        // Encoder Configurations" FAIL criteria
                        streamingTested = true;

                        //11.	Configure multicast settings for other entities from profile if required 
                        // (see Annex A.6).
                        string addressAudio = aec.Multicast.Address.IPv4Address;
                        int portAudio = aec.Multicast.Port;

                        string addressVideo = "";
                        int portVideo = 0;
                        string addressMetadata = "";
                        int portMetadata = 0;

                        if (profile.VideoEncoderConfiguration != null)
                        {
                            addressVideo = GetMulticastAddress2(usedMulticastIPs);
                            usedMulticastIPs.Add(addressVideo);
                            portVideo = GetMulticastPort2(usedMulticastPorts);
                            usedMulticastPorts.Add(portVideo);
                        }

                        if (profile.MetadataConfiguration != null)
                        {
                            addressMetadata = GetMulticastAddress2(usedMulticastIPs);
                            usedMulticastIPs.Add(addressMetadata);
                            portMetadata = GetMulticastPort2(usedMulticastPorts);
                            usedMulticastPorts.Add(portMetadata);
                        }


                        SetMulticastSettings(profile, IPType.IPv4, changeLog,
                            addressAudio, portAudio,
                            addressVideo, portVideo,
                            addressMetadata, portMetadata);
                        
                        //12.	ONVIF Client invokes StartMulticastStreamingRequest message 
                        // (ProfileToken = [profile token from the step 10]) to start multicast streaming from specified port.

                        // streaming setup
                        VideoUtils.AdjustVideo(
                            _videoForm, null, null, _messageTimeout, 
                            TransportProtocol.UDP, StreamType.RTPMulticast, null, null);

                        string token = profile.token;
                        RunStep(
                            () => { Client.StartMulticastStreaming(token); },
                            "StartMulticastStreaming");

                        DoRequestDelay();    
 
                        //13.	Verify the StartMulticastStreamingResponse from the DUT.
                        //14.	The DUT sends audio RTP multicast media stream to multicast IPv4 address over 
                        // UDP. 
                        //15.	ONVIF Client validates the received RTP and RTCP packets, decodes and renders 
                        // them.
                        //16.	ONVIF Client validates that specified multicast address and port are used.

                        _videoForm.MulticastRtpPortAudio = aec.Multicast.Port;
                        _videoForm.MulticastAddressAudio = aec.Multicast.Address.IPv4Address;
                        _videoForm.MulticastTTL = aec.Multicast.TTL;
                        _videoForm.AudioCodecName = GetAudioCodecFormatName(aec.Encoding);

                        ValidateStreamSequence(true, false);

                        //17.	ONVIF Client invokes StopMulticastStreamingRequest message 
                        // (ProfileToken = [profile token from the step 10]) to stop multicast streaming from specified port.

                        RunStep(
                            () => { Client.StopMulticastStreaming(token); },
                            "StopMulticastStreaming");

                        DoRequestDelay();

                        VideoCleanup();

                        //18.	Verify the StopMulticastStreamingResponse from the DUT.
                        //19.	Repeat steps 10-19 for the rest Audio Encoder configurations supported by the 
                        // DUT.

                    }

                    if (!streamingTested)
                    {
                        Assert(false, 
                            "For all encoder configurations it has been impossible to setup or create profile", 
                            "Check if testing has been performed at least for one configuration");
                    }
                },
                () => 
                {
                    StopMulticastStreaming(prof);

                    //20.	Restore Audio Encoder Configurations settings.
                    RestoreMediaConfiguration(changeLog);
                });
        
        }

        [Test(Name = "AUDIO ENCODER CONFIGURATION – MULTICAST ADDRESS (IPv4)",
               Path = PATH_AUDIO_M,
               Order = "02.02.10",
               Id = "2-2-10",
               Category = Category.RTSS,
               Version = 2.2,
               RequirementLevel = RequirementLevel.Must,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming })]
        public void AudioEncoderConfigurationMulticastAddressPIv4Test()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            List<int> usedMulticastPorts = new List<int>();
            List<string> usedMulticastIPs = new List<string>();
            Profile prof = null;

            RunTest(
                () =>
                {

                    //3.	ONVIF Client invokes GetAudioEncoderConfigurationsRequest message to retrieve 
                    // audio configuration list.
                    //4.	Verify the GetAudioEncoderConfigurationsResponse message.

                    AudioEncoderConfiguration[] encoderConfigurations = GetAudioEncoderConfigurations();
                    CheckAudioEncoderConfigurationsList(encoderConfigurations);
                        
                    int multicastPort = GetMulticastPort2(usedMulticastPorts);
                    usedMulticastPorts.Add(multicastPort);

                    //List<AudioEncoderConfiguration> selectedConfigs = MediaTestUtils.SelectConfigurations(encoderConfigurations);

                    foreach (AudioEncoderConfiguration aec in encoderConfigurations)
                    {
                        AudioEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(aec);

                        if (aec.Multicast == null)
                        {
                            aec.Multicast = new MulticastConfiguration();
                        }

                        string multicastAddress = GetMulticastAddress2(usedMulticastIPs);
                        usedMulticastIPs.Add(multicastAddress);

                        SetMulticast(aec.Multicast, IPType.IPv4, multicastAddress, multicastPort);
                        
                        //5.	ONVIF Client invokes SetAudioEncoderConfigurationRequest message 
                        // (ConfigurationToken = AECToken1, Multicast.Address = [“IPv4”, “multicastAddress1”], 
                        // Multicast.Port = “port1”, where AECToken1 is first audio encoder configuration token 
                        // from GetAudioEncoderConfigurationsResponse message) to change multicast port and 
                        // address for audio encoder configuration.

                        SetAudioEncoderConfiguration(aec, false, true);
                        //6.	Verify the SetAudioEncoderConfigurationResponse message from the DUT.

                        changeLog.TrackModifiedConfiguration(configCopy);

                        //7.	ONVIF Client invokes GetAudioEncoderConfigurationRequest message 
                        // (ConfigurationToken = AECToken1) to get audio encoder configuration.
                        //8.	Verify the GetAudioEncoderConfigurationResponse message (ConfigurationToken = Token1, 
                        // Multicast.Address = [“IPv4”, “multicastAddress1”], Multicast.Port = “port1”) from the DUT. 
                        // Check that new setting for Multicast.Port and Multicast.Address was applied.
                        AudioEncoderConfiguration actual = GetAudioEncoderConfiguration(aec.token);
                        CheckMulticastSettings(aec.Multicast, actual.Multicast);

                        //9.	Repeat steps 5-8 for the rest Audio Encoder configurations supported by the DUT with 
                        // using different multicast address and the same multicast port for Audio Encoder Configurations.

                    }


                    bool streamingTested = false;
                    foreach (AudioEncoderConfiguration aec in encoderConfigurations)
                    {

                        //10.	Find or create media profile with Audio Source Configuration and Audio Encoder 
                        // Configuration with token AECToken1 (see Annex A.5). If it is not possible skip steps 11-18 
                        // and go to the step 19.
                        Profile profile = GetProfileWithAudioEncoderConfiguration(aec.token, changeLog);
                        prof = profile;

                        // go to next AEC
                        if (profile == null)
                        {
                            continue;
                        }

                        // to check "It is not possible to find or create profile for all Audio 
                        // Encoder Configurations" FAIL criteria
                        streamingTested = true;
                        
                        //11.	Configure multicast settings for other entities from profile if required (see Annex A.6).
                        string addressAudio = aec.Multicast.Address.IPv4Address;
                        int portAudio = aec.Multicast.Port;

                        string addressVideo = "";
                        int portVideo = 0;
                        string addressMetadata = "";
                        int portMetadata = 0;

                        if (profile.VideoEncoderConfiguration != null)
                        {
                            addressVideo = GetMulticastAddress2(usedMulticastIPs);
                            usedMulticastIPs.Add(addressVideo);
                            portVideo = GetMulticastPort2(usedMulticastPorts);
                            usedMulticastPorts.Add(portVideo);
                        }

                        if (profile.MetadataConfiguration != null)
                        {
                            addressMetadata = GetMulticastAddress2(usedMulticastIPs);
                            usedMulticastIPs.Add(addressMetadata);
                            portMetadata = GetMulticastPort2(usedMulticastPorts);
                            usedMulticastPorts.Add(portMetadata);
                        }


                        SetMulticastSettings(profile, IPType.IPv4, changeLog,
                            addressAudio, portAudio,
                            addressVideo, portVideo,
                            addressMetadata, portMetadata);

                        //12.	ONVIF Client invokes StartMulticastStreamingRequest message 
                        // (ProfileToken = [profile token from the step 10]) to start multicast streaming from specified port.

                        // streaming setup
                        VideoUtils.AdjustVideo(
                            _videoForm, null, null, _messageTimeout,
                            TransportProtocol.UDP, StreamType.RTPMulticast, null, null);

                        string token = profile.token;
                        RunStep(
                            () => { Client.StartMulticastStreaming(token); },
                            "StartMulticastStreaming");

                        DoRequestDelay();  

                        //13.	Verify the StartMulticastStreamingResponse from the DUT.
                        //14.	The DUT sends audio RTP multicast media stream to multicast IPv4 address over UDP. 
                        //15.	ONVIF Client validates the received RTP and RTCP packets, decodes and renders them.
                        //16.	ONVIF Client validates that specified multicast address and port are used.

                        _videoForm.MulticastRtpPortAudio = aec.Multicast.Port;
                        _videoForm.MulticastAddressAudio = aec.Multicast.Address.IPv4Address;
                        _videoForm.MulticastTTL = aec.Multicast.TTL;
                        _videoForm.AudioCodecName = GetAudioCodecFormatName(aec.Encoding);

                        ValidateStreamSequence(true, false);

                        //17.	ONVIF Client invokes StopMulticastStreamingRequest message 
                        // (ProfileToken = [profile token from the step 10]) to stop multicast streaming 
                        // from specified port.

                        RunStep(
                            () => { Client.StopMulticastStreaming(token); },
                            "StopMulticastStreaming");

                        DoRequestDelay();

                        VideoCleanup();

                        //18.	Verify the StopMulticastStreamingResponse from the DUT.                    
                        
                        //19.	Repeat steps 10-18 for the rest Audio Encoder configurations supported by the DUT.
                    }

                    if (!streamingTested)
                    {
                        Assert(false,
                            "For all encoder configurations it has been impossible to setup or create profile",
                            "Check if testing has been performed at least for one configuration");
                    }
                },
                () => 
                {
                    StopMulticastStreaming(prof);

                    //20.	Restore Audio Encoder Configurations settings.
                    RestoreMediaConfiguration(changeLog);
                });

        }
        

        [Test(Name = "VIDEO ENCODER CONFIGURATION – MULTICAST PORT (IPv4)",
               Path = PATH_VIDEO_M,
               Order = "01.02.19",
               Id = "1-2-19",
               Category = Category.RTSS,
               Version = 2.2,
               RequirementLevel = RequirementLevel.Must, // [AR] wush 136
               //RequirementLevel = RequirementLevel.Optional,
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming })]
        public void VideoEncoderConfigurationMulticastPortIpv4Test()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            List<int> usedMulticastPorts = new List<int>();
            List<string> usedMulticastIPs = new List<string>();
            Profile prof = null;

            RunTest(
                () =>
                {

                    //3.	ONVIF Client invokes GetVideoEncoderConfigurationsRequest message to 
                    // retrieve video configuration list.
                    //4.	Verify the GetVideoEncoderConfigurationsResponse message.
                   VideoEncoderConfiguration[] encoderConfigurations = GetVideoEncoderConfigurations();
                   CheckVideoEncoderConfigurationsList(encoderConfigurations);

                    // Use the same address for all configurations
                    string multicastAddress = GetMulticastAddress2(usedMulticastIPs);
                    usedMulticastIPs.Add(multicastAddress);

                    //List<VideoEncoderConfiguration> selectedConfigs = MediaTestUtils.SelectConfigurations(encoderConfigurations);

                    foreach (VideoEncoderConfiguration vec in encoderConfigurations)
                    {
                        VideoEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(vec);


                        if (vec.Multicast == null)
                        {
                            vec.Multicast = new MulticastConfiguration();
                        }

                        // Use different port
                        int multicastPort = GetMulticastPort2(usedMulticastPorts);
                        usedMulticastPorts.Add(multicastPort);

                        SetMulticast(vec.Multicast, IPType.IPv4, multicastAddress, multicastPort);

                        //5.	ONVIF Client invokes SetVideoEncoderConfigurationRequest message 
                        // (ConfigurationToken = VECToken1, Multicast.Address = [“IPv4”, “multicastAddress1”], 
                        // Multicast.Port = “port1”, where VECToken1 is first video encoder configuration 
                        // token from GetVideoEncoderConfigurationsResponse message) to change multicast port 
                        // and address for video encoder configuration.
                        //6.	Verify the SetVideoEncoderConfigurationResponse message from the DUT.

                        SetVideoEncoderConfiguration(vec, false, true);
                        
                        changeLog.ModifiedVideoEncoderConfigurations.Add(configCopy);

                        //7.	ONVIF Client invokes GetVideoEncoderConfigurationRequest message 
                        // (ConfigurationToken = VECToken1) to get video encoder configuration.
                        //8.	Verify the GetVideoEncoderConfigurationResponse message (ConfigurationToken = Token1, 
                        // Multicast.Address = [“IPv4”, “multicastAddress1”], Multicast.Port = “port1”) from the 
                        // DUT. Check that new setting for Multicast.Port and Multicast.Address was applied.
                        VideoEncoderConfiguration actual = GetVideoEncoderConfiguration(vec.token);

                        CheckMulticastSettings(vec.Multicast, actual.Multicast);
                        //9.	Repeat steps 5-8 for the rest Video Encoder configurations supported by the 
                        // DUT with using different multicast ports and the same multicast addresses for Video 
                        // Encoder Configurations.                  
                    }                  
                                      
                    
                    bool streamingTested = false;
                    foreach (VideoEncoderConfiguration vec in encoderConfigurations)
                    {

                        //10.	Find or create media profile with Video Source Configuration and Video Encoder 
                        // Configuration with token VECToken1 (see Annex A.7). If it is not possible skip steps 
                        // 11-18 and go to the step 19.
                        Profile profile = GetProfileWithVideoEncoderConfiguration(vec.token, changeLog);
                        prof = profile;
                        if (profile == null)
                        {
                            continue;
                        }

                        streamingTested = true;

                        //11.	Configure multicast settings for other entities from profile if required 
                        // (see Annex A.6).
                        string addressVideo = vec.Multicast.Address.IPv4Address;
                        int portVideo = vec.Multicast.Port;

                        string addressAudio = "";
                        int portAudio = 0;
                        string addressMetadata = "";
                        int portMetadata = 0;

                        if (profile.AudioEncoderConfiguration != null)
                        {
                            addressAudio = GetMulticastAddress2(usedMulticastIPs);
                            usedMulticastIPs.Add(addressAudio);
                            portAudio = GetMulticastPort2(usedMulticastPorts);
                            usedMulticastPorts.Add(portAudio);
                        }

                        if (profile.MetadataConfiguration != null)
                        {
                            addressMetadata = GetMulticastAddress2(usedMulticastIPs);
                            usedMulticastIPs.Add(addressMetadata);
                            portMetadata = GetMulticastPort2(usedMulticastPorts);
                            usedMulticastPorts.Add(portMetadata);
                        }

                        SetMulticastSettings(profile, IPType.IPv4, changeLog,
                            addressAudio, portAudio,
                            addressVideo, portVideo,
                            addressMetadata, portMetadata);
                                                
                        //12.	ONVIF Client invokes StartMulticastStreamingRequest message (ProfileToken = 
                        // [profile token from the step 10]) to start multicast streaming from specified port.

                        // streaming setup
                        VideoUtils.AdjustVideo(
                            _videoForm, null, null, _messageTimeout,
                            TransportProtocol.UDP, StreamType.RTPMulticast, null, vec);

                        string token = profile.token;
                        RunStep(() => 
                        {
                            LogStepEvent(string.Format(
                                "Start streaming to {0}:{1} (video encoder configuration settings), {2}:{3} (profile settings)",
                                vec.Multicast.Address.IPv4Address,
                                vec.Multicast.Port,
                                profile.VideoEncoderConfiguration.Multicast.Address.IPv4Address,
                                profile.VideoEncoderConfiguration.Multicast.Port));
                            Client.StartMulticastStreaming(token); 
                        }, "StartMulticastStreaming");

                        DoRequestDelay();  


                        //13.	Verify the StartMulticastStreamingResponse from the DUT.
                        //14.	The DUT sends video RTP multicast media stream to multicast IPv4 address over UDP. 
                        //15.	ONVIF Client validates the received RTP and RTCP packets, decodes and renders them.
                        //16.	ONVIF Client validates that specified multicast address and port are used.

                        int fps = 1;
                        if (vec.RateControl != null)
                        {
                            fps = vec.RateControl.FrameRateLimit;
                        }

                        _videoForm.VideoFPS = fps;
                        _videoForm.MulticastRtpPortVideo = vec.Multicast.Port;
                        _videoForm.VideoCodecName = GetVideoCodecFormatName(vec.Encoding);
                        _videoForm.MulticastAddress = vec.Multicast.Address.IPv4Address;
                        _videoForm.MulticastTTL = vec.Multicast.TTL;

                        ValidateStreamSequence(false, true);

                        //17.	ONVIF Client invokes StopMulticastStreamingRequest message (ConfigurationToken 
                        // ProfileToken = [profile token from the step 10]) to stop multicast streaming from 
                        // specified port.

                        RunStep(
                            () => { Client.StopMulticastStreaming(token); },
                            "StopMulticastStreaming");

                        DoRequestDelay();

                        VideoCleanup();

                        //18.	Verify the StopMulticastStreamingResponse from the DUT.
                        //19.	Repeat steps 10-19 for the rest Video Encoder configurations supported by the DUT.

                    }

                    if (!streamingTested)
                    {
                        // check FAIL criteria
                        Assert(false,
                            "For all encoder configurations it has been impossible to setup or create profile",
                            "Check if testing has been performed at least for one configuration");
                    }
                },
                () =>
                {
                    StopMulticastStreaming(prof);

                    //20.	Restore Video Encoder Configurations settings.
                    RestoreMediaConfiguration(changeLog);
                });

        }
        

        [Test(Name = "VIDEO ENCODER CONFIGURATION – MULTICAST ADDRESS (IPv4)",
               Path = PATH_VIDEO_M,
               Order = "01.02.20",
               Id = "1-2-20",
               Category = Category.RTSS,
               Version = 2.2,
               RequirementLevel = RequirementLevel.Must,// wush 136
               RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP },
               FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming })]
        public void VideoEncoderConfigurationMulticastAddressIpv4Test()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            List<int> usedMulticastPorts = new List<int>();
            List<string> usedMulticastIPs = new List<string>();
            Profile prof = null;

            RunTest(
                () =>
                {

                    //3.	ONVIF Client invokes GetVideoEncoderConfigurationsRequest message to 
                    // retrieve video configuration list.
                    //4.	Verify the GetVideoEncoderConfigurationsResponse message.
                    VideoEncoderConfiguration[] encoderConfigurations = GetVideoEncoderConfigurations();
                    CheckVideoEncoderConfigurationsList(encoderConfigurations);

                    // Use the same port for all configurations                        
                    int multicastPort = GetMulticastPort2(usedMulticastPorts);
                    usedMulticastPorts.Add(multicastPort);

                    //List<VideoEncoderConfiguration> selectedConfigs = MediaTestUtils.SelectConfigurations(encoderConfigurations);

                    foreach (VideoEncoderConfiguration vec in encoderConfigurations)
                    {
                        VideoEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(vec);
                        
                        if (vec.Multicast == null)
                        {
                            vec.Multicast = new MulticastConfiguration();
                        }

                        // Use different addresses
                        string multicastAddress = GetMulticastAddress2(usedMulticastIPs);
                        usedMulticastIPs.Add(multicastAddress);

                        SetMulticast(vec.Multicast, IPType.IPv4, multicastAddress, multicastPort);
                        //5.	ONVIF Client invokes SetVideoEncoderConfigurationRequest message 
                        // (ConfigurationToken = VECToken1, Multicast.Address = [“IPv4”, “multicastAddress1”], 
                        // Multicast.Port = “port1”, where VECToken1 is first video encoder configuration 
                        // token from GetVideoEncoderConfigurationsResponse message) to change multicast port and address for video encoder configuration.
                        //6.	Verify the SetVideoEncoderConfigurationResponse message from the DUT.

                        SetVideoEncoderConfiguration(vec, false, true);

                        changeLog.ModifiedVideoEncoderConfigurations.Add(configCopy);

                        //7.	ONVIF Client invokes GetVideoEncoderConfigurationRequest message (ConfigurationToken = VECToken1) to get video encoder configuration.
                        //8.	Verify the GetVideoEncoderConfigurationResponse message (ConfigurationToken = Token1, Multicast.Address = [“IPv4”, “multicastAddress1”], Multicast.Port = “port1”) from the DUT. Check that new setting for Multicast.Port and Multicast.Address was applied.
                        VideoEncoderConfiguration actual = GetVideoEncoderConfiguration(vec.token);

                        CheckMulticastSettings(vec.Multicast, actual.Multicast);
                        //9.	Repeat steps 5-8 for the rest Video Encoder configurations supported by the DUT with using different multicast ports and the same multicast addresses for Video Encoder Configurations.
                    }

                    bool streamingTested = false;
                    foreach (VideoEncoderConfiguration vec in encoderConfigurations)
                    {

                        //10.	Find or create media profile with Video Source Configuration and Video Encoder Configuration with token VECToken1 (see Annex A.7). If it is not possible skip steps 11-19 and go to the step 20.
                        Profile profile = GetProfileWithVideoEncoderConfiguration(vec.token, changeLog);
                        prof = profile;
                        if (profile == null)
                        {
                            continue;
                        }

                        streamingTested = true;

                        //11.	Configure multicast settings for other entities from profile if required (see Annex A.6).
                        string addressVideo = vec.Multicast.Address.IPv4Address;
                        int portVideo = vec.Multicast.Port;

                        string addressAudio = "";
                        int portAudio = 0;
                        string addressMetadata = "";
                        int portMetadata = 0;

                        if (profile.AudioEncoderConfiguration != null)
                        {
                            addressAudio = GetMulticastAddress2(usedMulticastIPs);
                            usedMulticastIPs.Add(addressAudio);
                            portAudio = GetMulticastPort2(usedMulticastPorts);
                            usedMulticastPorts.Add(portAudio);
                        }

                        if (profile.MetadataConfiguration != null)
                        {
                            addressMetadata = GetMulticastAddress2(usedMulticastIPs);
                            usedMulticastIPs.Add(addressMetadata);
                            portMetadata = GetMulticastPort2(usedMulticastPorts);
                            usedMulticastPorts.Add(portMetadata);
                        }

                        SetMulticastSettings(profile, IPType.IPv4, changeLog,
                            addressAudio, portAudio,
                            addressVideo, portVideo,
                            addressMetadata, portMetadata);

                        //12.	ONVIF Client invokes StartMulticastStreamingRequest message (ProfileToken = [profile token from the step 10]) to start multicast streaming from specified port.

                        // streaming setup
                        VideoUtils.AdjustVideo(
                            _videoForm, null, null, _messageTimeout,
                            TransportProtocol.UDP, StreamType.RTPMulticast, null, vec);

                        string token = profile.token;
                        RunStep(() =>
                        {
                            LogStepEvent(string.Format(
                                "Start streaming to {0}:{1} (video encoder configuration settings), {2}:{3} (profile settings)",
                                vec.Multicast.Address.IPv4Address,
                                vec.Multicast.Port,
                                profile.VideoEncoderConfiguration.Multicast.Address.IPv4Address,
                                profile.VideoEncoderConfiguration.Multicast.Port));
                            Client.StartMulticastStreaming(token);
                        }, "StartMulticastStreaming");

                        DoRequestDelay();  

                        //13.	Verify the StartMulticastStreamingResponse from the DUT.
                        //14.	The DUT sends video RTP multicast media stream to multicast IPv4 address over UDP. 
                        //15.	ONVIF Client validates the received RTP and RTCP packets, decodes and renders them.
                        //16.	ONVIF Client validates that specified multicast address and port are used.

                        int fps = 1;
                        if (vec.RateControl != null)
                        {
                            fps = vec.RateControl.FrameRateLimit;
                        }

                        _videoForm.VideoFPS = fps;
                        _videoForm.MulticastRtpPortVideo = vec.Multicast.Port;
                        _videoForm.VideoCodecName = GetVideoCodecFormatName(vec.Encoding);
                        _videoForm.MulticastAddress = vec.Multicast.Address.IPv4Address;
                        _videoForm.MulticastTTL = vec.Multicast.TTL;

                        ValidateStreamSequence(false, true);  

                        //17.	ONVIF Client invokes StopMulticastStreamingRequest message (ConfigurationToken ProfileToken = [profile token from the step 10]) to stop multicast streaming from specified port.

                        RunStep(
                            () => { Client.StopMulticastStreaming(token); },
                            "StopMulticastStreaming");

                        DoRequestDelay();

                        VideoCleanup();

                        //18.	Verify the StopMulticastStreamingResponse from the DUT.
                        //19.	Repeat steps 10-19 for the rest Video Encoder configurations supported by the DUT.

                    }

                    if (!streamingTested)
                    {
                        // check FAIL criteria
                        Assert(false,
                            "For all encoder configurations it has been impossible to setup or create profile",
                            "Check if testing has been performed at least for one configuration");
                    }
                },
                () =>
                {
                    StopMulticastStreaming(prof);

                    //20.	Restore Video Encoder Configurations settings.
                    RestoreMediaConfiguration(changeLog);
                });

        }

        [Test(Name = "VIDEO ENCODER CONFIGURATION – MULTICAST ADDRESS AND PORT IN RTSP SETUP (IPv4)",
              Path = PATH_VIDEO_M,
              Order = "01.02.21",
              Id = "1-2-21",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })
        ]
        public void VideoEncoderConfigurationMulticastAddressAndPortInRTSPSetupIpv4Test()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
                () =>
                {
                    // 3. ONVIF Client selects a media profile with JPEG video encoding support 
                    // by the following procedure mentioned in Annex A.13.

                    VideoEncoderConfigurationOptions options = null;

                    Profile profile = SelectVideoProfile(VideoEncoding.JPEG, out options);

                    //
                    // Pre-requisite requires, that profile with JPEG encoder must exist at the DUT, so 
                    // GetProfileForSpecificCodec will return "ready to use" JPEG profile
                    //

                    if (profile.VideoEncoderConfiguration == null)
                    {
                        Assert(false,
                            "According to pre-requisite, profile with JPEG encoder should exist at the DUT",
                            "No ready to use profile can be found");
                    }

                    {
                        VideoEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(profile.VideoEncoderConfiguration);
                        changeLog.TrackModifiedConfiguration(configCopy);
                    }

                    // 4. ONVIF Client invokes SetVideoEncoderConfigurationRequest message 
                    // (Encoding = “JPEG”, Resolution = [“Width”, “Height”], Quality = q1, 
                    // Multicast.Address = [“IPv4”, “multicastAddress1”], Multicast.Port = “port1”, TTL = “ttl1”, 
                    // Session Timeout = t1 and force persistence = false) to set JPEG encoding and Multicast settings.

                    // 5. Verify the SetVideoEncoderConfigurationResponse message from the DUT.

                    int addrOffset = 0;
                    int portOffset = 0;
                    SetMulticastSettings(profile, true, false, IPType.IPv4, addrOffset++, portOffset++);

                    SetVideoEncoding(profile.VideoEncoderConfiguration, VideoEncoding.JPEG, options);

                    SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, true);

                    // 6. ONVIF Client invokes GetStreamUriRequest message 
                    // (ProfileToken, Stream = “RTP-Multicast”, Transport.Protocol = “UDP”) 
                    // to retrieve media stream URI for the selected media profile.

                    // 7. The DUT sends the GetStreamUriResponse message with RTSP URI and parameters 
                    // defining the lifetime of the URI like ValidUntilConnect, ValidUntilReboot and Timeout.

                    StreamSetup streamSetup = new StreamSetup();
                    streamSetup.Transport = new Transport();
                    streamSetup.Transport.Protocol = TransportProtocol.UDP;
                    streamSetup.Stream = StreamType.RTPMulticast;

                    MediaUri streamUri = GetStreamUri(streamSetup, profile.token);

                    //8. ONVIF Client verifies the RTSP media stream URI provided by the DUT.
                    //9. ONVIF Client invokes RTSP DESCRIBE request.
                    //10. The DUT sends 200 OK message and SDP information.
                    //11. ONVIF Client invokes RTSP SETUP request with transport parameter RTP-Multicast/UDP and destination= multicastAddress1, port=port1-port2.
                    //12. The DUT sends 200 OK message and the media stream information. Verify that destination= multicastAddress1, port=port1-port2 was received.
                    //13. ONVIF Client invokes RTSP PLAY request.
                    //14. The DUT sends 200 OK message and starts media streaming.
                    //15. The DUT sends JPEG RTP multicast media stream to multicast IPv4 address over UDP.
                    //16. Verify that specified multicast port and address are used for streaming.
                    //17. The DUT sends RTCP sender report to ONVIF Client.
                    //18. ONVIF Client validates the received RTP and RTCP packets, decodes and renders them.
                    //19. ONVIF Client invokes RTSP TEARDOWN control request at the end of media streaming to terminate the RTSP session.
                    //20. The DUT sends 200 OK Response and terminates the RTSP Session.

                    VideoUtils.AdjustVideo(
                        _videoForm, _username, _password, _messageTimeout, streamSetup.Transport.Protocol,
                        streamSetup.Stream, streamUri, profile.VideoEncoderConfiguration);

                    int port = profile.VideoEncoderConfiguration.Multicast.Port;

                    // get multicast settings from VEC for SETUP
                    _videoForm.MulticastMultipleSetup = string.Format(
                        "{0};{1};",
                        profile.VideoEncoderConfiguration.Multicast.Address.IPv4Address,
                        profile.VideoEncoderConfiguration.Multicast.Port);
                    ValidateStreamSequence(false, true);
                    VideoCleanup();
                },
                () =>
                {
                    RestoreMediaConfiguration(changeLog);
                }
                );
        }

        [Test(Name = "VIDEO ENCODER CONFIGURATION – MULTICAST ADDRESS AND PORT IN RTSP SETUP (IPv6)",
              Path = PATH_VIDEO_M,
              Order = "01.02.22",
              Id = "1-2-22",
              Category = Category.RTSS,
              Version = 2.2,
              RequirementLevel = RequirementLevel.Optional,
              RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP, Feature.IPv6 },
              FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp })
        ]
        public void VideoEncoderConfigurationMulticastAddressAndPortInRTSPSetupIpv6Test()
        {
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();

            RunTest(
                () =>
                {
                    // 3. ONVIF Client selects a media profile with JPEG video encoding support 
                    // by the following procedure mentioned in Annex A.13.

                    VideoEncoderConfigurationOptions options = null;

                    Profile profile = SelectVideoProfile(VideoEncoding.JPEG, out options);

                    //
                    // Pre-requisite requires, that profile with JPEG encoder must exist at the DUT, so 
                    // GetProfileForSpecificCodec will return "ready to use" JPEG profile
                    //

                    if (profile.VideoEncoderConfiguration == null)
                    {
                        Assert(false,
                            "According to pre-requisite, profile with JPEG encoder should exist at the DUT",
                            "No ready to use profile can be found");
                    }

                    {
                        VideoEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(profile.VideoEncoderConfiguration);
                        changeLog.TrackModifiedConfiguration(configCopy);
                    }

                    // 4. ONVIF Client invokes SetVideoEncoderConfigurationRequest message 
                    // (Encoding = “JPEG”, Resolution = [“Width”, “Height”], Quality = q1, 
                    // Multicast.Address = [“IPv6”, “multicastAddress1”], Multicast.Port = “port1”, 
                    // TTL = “ttl1”, Session Timeout = t1 and force persistence = false) 
                    // to set JPEG encoding and Multicast settings.
                    // 5. Verify the SetVideoEncoderConfigurationResponse message from the DUT.

                    int addrOffset = 0;
                    int portOffset = 0;
                    SetMulticastSettings(profile, true, false, IPType.IPv6, addrOffset++, portOffset++);

                    SetVideoEncoding(profile.VideoEncoderConfiguration, VideoEncoding.JPEG, options);

                    SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, true);

                    // 6. ONVIF Client invokes GetStreamUriRequest message 
                    // (ProfileToken, Stream = “RTP-Multicast”, Transport.Protocol = “UDP”) to retrieve 
                    // media stream URI for the selected media profile.
                    //7. The DUT sends the GetStreamUriResponse message with RTSP URI and parameters 
                    // defining the lifetime of the URI like ValidUntilConnect, ValidUntilReboot and Timeout.

                    StreamSetup streamSetup = new StreamSetup();
                    streamSetup.Transport = new Transport();
                    streamSetup.Transport.Protocol = TransportProtocol.UDP;
                    streamSetup.Stream = StreamType.RTPMulticast;

                    MediaUri streamUri = GetStreamUri(streamSetup, profile.token);

                    //8. ONVIF Client verifies the RTSP media stream URI provided by the DUT.
                    //9. ONVIF Client invokes RTSP DESCRIBE request.
                    //10. The DUT sends 200 OK message and SDP information.
                    //11. ONVIF Client invokes RTSP SETUP request with transport parameter RTP-Multicast/UDP and destination= multicastAddress1, port=port1-port2.
                    //12. The DUT sends 200 OK message and the media stream information. Verify that destination= multicastAddress1, port=port1-port2 was received.
                    //13. ONVIF Client invokes RTSP PLAY request.
                    //14. The DUT sends 200 OK message and starts media streaming.
                    //15. The DUT sends JPEG RTP multicast media stream to multicast IPv4 address over UDP.
                    //16. Verify that specified multicast port and address are used for streaming.
                    //17. The DUT sends RTCP sender report to ONVIF Client.
                    //18. ONVIF Client validates the received RTP and RTCP packets, decodes and renders them.
                    //19. ONVIF Client invokes RTSP TEARDOWN control request at the end of media streaming to terminate the RTSP session.
                    //20. The DUT sends 200 OK Response and terminates the RTSP Session.

                    VideoUtils.AdjustVideo(
                        _videoForm, _username, _password, _messageTimeout, streamSetup.Transport.Protocol,
                        streamSetup.Stream, streamUri, profile.VideoEncoderConfiguration);

                    int port = profile.VideoEncoderConfiguration.Multicast.Port;

                    _videoForm.MulticastMultipleSetup = string.Format(
                        "{0};{1};",
                        profile.VideoEncoderConfiguration.Multicast.Address.IPv6Address,
                        profile.VideoEncoderConfiguration.Multicast.Port);
                    ValidateStreamSequence(false, true);
                    VideoCleanup();
                },
                () =>
                {
                    RestoreMediaConfiguration(changeLog);
                }
                );
        }

        [Test(Name = "VIDEO AND AUDIO ENCODER CONFIGURATION – DIFFERENT PORTS",
           Path = PATH_AUDIO_VIDEO_M,
           Order = "03.02.22",
           Id = "3-2-22",
           Category = Category.RTSS,
           Version = 2.2,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming })]
        public void VideoAndAudioEncoderConfigurationDifferentPortsTest()
        {
            VideoAndAudioMulticastSettingsTest(false, true);
        }

        [Test(Name = "VIDEO AND AUDIO ENCODER CONFIGURATION – DIFFERENT ADDRESS",
           Path = PATH_AUDIO_VIDEO_M,
           Order = "03.02.23",
           Id = "3-2-23",
           Category = Category.RTSS,
           Version = 2.2,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.Audio, Feature.RTPMulticastUDP },
           FunctionalityUnderTest = new Functionality[] { Functionality.MediaStreamingRtsp, Functionality.StartMulticastStreaming, Functionality.StopMulticastStreaming })]
        public void VideoAndAudioEncoderConfigurationDifferentAddressTest()
        {
            VideoAndAudioMulticastSettingsTest(true, false);
        }

        void VideoAndAudioMulticastSettingsTest(bool addressDifferent, bool portDifferent)
        { 
            MediaConfigurationChangeLog changeLog = new MediaConfigurationChangeLog();
            List<int> usedMulticastPorts = new List<int>();
            List<string> usedMulticastIPs = new List<string>();
            Profile prof = null;
            bool isMulticastStopped = false;
            bool isMulticastStarted = false;

            RunTest(
                () =>
                {

                    //3.	ONVIF Client selects a media profile with JPEG and G.711 encoding support 
                    // (see Annex A.15).

                    VideoEncoderConfigurationOptions videoOptions = null;
                    AudioEncoderConfigurationOptions audioOptions = null;
                    Profile profile = GetProfileWithAudioAndVideoSupport(VideoEncoding.JPEG,
                        AudioEncoding.G711,
                        changeLog,
                        out videoOptions,
                        out audioOptions);

                    prof = profile;

                    string err;
                    Assert(ValidateVideoEncoderConfigOptions(videoOptions, out err),
                        err, "Validate videoEncoderConfigurationOptions");
                                                            
                    AudioEncoderConfigurationOption g711Option = 
                        audioOptions.Options.Where( o => o.Encoding == AudioEncoding.G711).FirstOrDefault();
                    
                    ValidateAudioEncoderConfigurationOption(g711Option);

                    string multicastAddress1 = GetMulticastAddress2(usedMulticastIPs);
                    usedMulticastIPs.Add(multicastAddress1);

                    string multicastAddress2 = addressDifferent ? GetMulticastAddress2(usedMulticastIPs) : multicastAddress1;
                    if (multicastAddress2 != multicastAddress1)
                        usedMulticastIPs.Add(multicastAddress2);

                    int multicastPort1 = GetMulticastPort2(usedMulticastPorts);
                    usedMulticastPorts.Add(multicastPort1);

                    int multicastPort2 = portDifferent ? GetMulticastPort2(usedMulticastPorts): multicastPort1 ;
                    if (multicastPort2 != multicastPort1)
                        usedMulticastPorts.Add(multicastPort2);

                    int ttl = 1;

                    //4.	ONVIF Client invokes SetVideoEncoderConfigurationRequest message 
                    // (Encoding = “JPEG”, Resolution = [“Width”, “Height”], Quality = q1, 
                    // Multicast.Address = [“IPv4”, “multicastAddress1”], Multicast.Port = “port1”, 
                    // TTL = “ttl1”, Session Timeout = t1 and force persistence = false) to set JPEG 
                    // encoding and Multicast settings.

                    VideoEncoderConfiguration vec = profile.VideoEncoderConfiguration;
                    VideoEncoderConfiguration vecCopy = Utils.CopyMaker.CreateCopy(vec);

                    vec.Encoding = VideoEncoding.JPEG;
                    vec.H264 = null;
                    vec.MPEG4 = null;
                    vec.Resolution = videoOptions.JPEG.ResolutionsAvailable.First();
                    vec.Quality = videoOptions.QualityRange.Average();

                    if (vec.RateControl == null && (videoOptions.JPEG.EncodingIntervalRange != null
                        || videoOptions.JPEG.FrameRateRange != null ||
                        (videoOptions.Extension != null && videoOptions.Extension.JPEG != null && videoOptions.Extension.JPEG.BitrateRange != null)))
                    {
                        vec.RateControl = new VideoRateControl();
                    }
                                        
                    if (videoOptions.JPEG.EncodingIntervalRange != null)
                    {
                        vec.RateControl.EncodingInterval = videoOptions.JPEG.EncodingIntervalRange.Average();
                    }
                    if (videoOptions.JPEG.FrameRateRange != null)
                    {
                        vec.RateControl.FrameRateLimit = videoOptions.JPEG.FrameRateRange.Average();                    
                    }
                    if (videoOptions.Extension != null && videoOptions.Extension.JPEG != null && videoOptions.Extension.JPEG.BitrateRange != null)
                    {
                        vec.RateControl.BitrateLimit = videoOptions.Extension.JPEG.BitrateRange.Average();
                    }

                    // TODO: review multicast setup!!!
                    if (vec.Multicast == null)
                    {
                        vec.Multicast = new MulticastConfiguration();
                    }
                    if (vec.Multicast.Address == null)
                    {
                        vec.Multicast.Address = new IPAddress();
                    }
                    vec.Multicast.Address.Type = IPType.IPv4;
                    vec.Multicast.Address.IPv4Address = multicastAddress1;
                    vec.Multicast.Address.IPv6Address = null;
                    vec.Multicast.Port = multicastPort1;
                    vec.Multicast.TTL = ttl;
                    
                    //5.	Verify the SetVideoEncoderConfigurationResponse message from the DUT.
                    SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, true);
                    changeLog.ModifiedVideoEncoderConfigurations.Add(vecCopy);

                    //6.	ONVIF Client invokes SetAudioEncoderConfigurationRequest message 
                    // (Encoding = “G711”, Bitrate = r1, SampleRate = r2, Multicast.Address = 
                    // [“IPv4”, “multicastAddress1”], Multicast.Port = “port2”, TTL = “ttl1”, 
                    // Session Timeout = t1 and force persistence = false) to set G.711 encoding and 
                    // Multicast settings.

                    AudioEncoderConfiguration aec = profile.AudioEncoderConfiguration;
                    AudioEncoderConfiguration aecCopy = Utils.CopyMaker.CreateCopy(aec);
                                        
                    aec.Encoding = AudioEncoding.G711;
                    aec.Bitrate = g711Option.BitrateList.First();
                    aec.SampleRate = g711Option.SampleRateList.First();

                    if (aec.Multicast == null)
                    {
                        aec.Multicast = new MulticastConfiguration();
                    }
                    if (aec.Multicast.Address == null)
                    {
                        aec.Multicast.Address = new IPAddress();
                    }
                    aec.Multicast.Address.Type = IPType.IPv4;
                    aec.Multicast.Address.IPv4Address = multicastAddress2;
                    aec.Multicast.Address.IPv6Address = null;
                    aec.Multicast.Port = multicastPort2;
                    aec.Multicast.TTL = ttl;

                    SetAudioEncoderConfiguration(aec, false, true);
                    //7.	Verify the SetAudioEncoderConfigurationResponse message from the DUT.
                    changeLog.TrackModifiedConfiguration(aecCopy);

                    //8.	ONVIF Client invokes GetVideoEncoderConfigurationRequest message to get audio 
                    // encoder configuration.
                    //9.	Verify the GetVideoEncoderConfigurationResponse message (Multicast.Address = 
                    // [“IPv4”, “multicastAddress1”], Multicast.Port = “port1”) from the DUT. Check that 
                    // new setting for Multicast.Port and Multicast.Address was applied.

                    VideoEncoderConfiguration actualVec = GetVideoEncoderConfiguration(vec.token);
                    CheckMulticastSettings(vec.Multicast, actualVec.Multicast);

                    //10.	ONVIF Client invokes GetAudioEncoderConfigurationRequest message 
                    // (ConfigurationToken = AECToken1) to get audio encoder configuration.
                    //11.	Verify the GetAudioEncoderConfigurationResponse message (Multicast.Address = 
                    // [“IPv4”, “multicastAddress1”], Multicast.Port = “port1”) from the DUT. Check that 
                    // new setting for Multicast.Port and Multicast.Address was applied.
                    AudioEncoderConfiguration actualAec = GetAudioEncoderConfiguration(aec.token);
                    CheckMulticastSettings(aec.Multicast, actualAec.Multicast);

                    //12.	ONVIF Client will invoke StartMulticastStreamingRequest message (ProfileToken) 
                    // to start multicast streaming.

                    // streaming setup
                    VideoUtils.AdjustVideo(
                        _videoForm, null, null, _messageTimeout,
                        TransportProtocol.UDP, StreamType.RTPMulticast, null, vec);

                    string token = profile.token;
                    RunStep(
                        () => { Client.StartMulticastStreaming(token); },
                        "StartMulticastStreaming");

                    DoRequestDelay();

                    isMulticastStarted = true;

                    //13.	Verify the StartMulticastStreamingResponse message from the DUT.
                    //14.	The DUT sends JPEG/G.711 RTP multicast media stream to multicast IPv4 address 
                    // over UDP.
                    //15.	The DUT sends RTCP sender report to ONVIF Client.
                    //16.	ONVIF Client validates the received RTP and RTCP packets, decodes and renders 
                    // them.
                    //17.	ONVIF Client validates that specified multicast address and port are used

                    int fps = 1;
                    if (vec.RateControl != null)
                    {
                        fps = vec.RateControl.FrameRateLimit;
                    }

                    _videoForm.VideoFPS = fps;
                    _videoForm.MulticastRtpPortVideo = vec.Multicast.Port;
                    _videoForm.VideoCodecName = GetVideoCodecFormatName(vec.Encoding);
                    _videoForm.MulticastAddress = vec.Multicast.Address.IPv4Address;
                    _videoForm.MulticastTTL = vec.Multicast.TTL;

                    _videoForm.MulticastRtpPortAudio = aec.Multicast.Port;
                    _videoForm.MulticastAddressAudio = aec.Multicast.Address.IPv4Address;
                    _videoForm.MulticastTTL = aec.Multicast.TTL;
                    _videoForm.AudioCodecName = GetAudioCodecFormatName(aec.Encoding);

                    _videoForm.EventSink = this;
                    _videoForm.OpenWindow(true, true);
                    VideoIsOpened = true;

                    Sleep(_operationDelay);

                    VideoIsOpened = false;
                    _videoForm.CloseWindow();
                    _videoForm.EventSink = null;   

                    //18.	ONVIF Client will invoke StopMulticastStreamingRequest message (ProfileToken) to 
                    // stop multicast streaming

                    StopMulticastStreaming(prof);
                    isMulticastStopped = true;

                    //19.	Verify the StoptMulticastStreamingResponse message from the DUT.
                    //20.	Verify that multicast stream is stopped by the DUT.                  
                    

                }, 
                () => 
                {
                    if (isMulticastStarted && !isMulticastStopped)
                    {
                        StopMulticastStreaming(prof);
                    }


                    RestoreMediaConfiguration(changeLog);
                });
                
        
        }

    }
}
