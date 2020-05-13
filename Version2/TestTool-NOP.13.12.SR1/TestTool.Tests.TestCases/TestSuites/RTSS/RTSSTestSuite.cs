using System;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Proxies.Onvif;
using System.Linq;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Common.Media;
using System.ServiceModel;
using TestTool.Tests.Common.TestBase;
using System.Collections.Generic;

namespace TestTool.Tests.TestCases.TestSuites
{
    class RTSSTestSuite : MediaTest, IVideoFormEvent
    {
        protected bool VideoIsOpened = false;

        public RTSSTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        #region Common Streaming Utilities

        protected int FindNearestAudioBitrate(int current, AudioEncoding encoding, AudioEncoderConfigurationOptions options)
        {
            int bitrate = 0;
            int difference = int.MaxValue;

            foreach (AudioEncoderConfigurationOption opt in options.Options)
            {
                if (opt.Encoding == encoding && opt.BitrateList != null)
                {
                    foreach (int b in opt.BitrateList)
                    {
                        int diff = System.Math.Abs(b - current);
                        if (diff < difference)
                        {
                            difference = diff;
                            bitrate = b;
                        }
                    }
                }
            }
            return bitrate;
        }

        protected int FindNearestAudioSamplerate(int current, AudioEncoding encoding, AudioEncoderConfigurationOptions options)
        {
            int samplerate = 0;
            int difference = int.MaxValue;

            foreach (AudioEncoderConfigurationOption opt in options.Options)
            {
                if (opt.Encoding == encoding && opt.SampleRateList != null)
                {
                    foreach (int val in opt.SampleRateList)
                    {
                        int diff = System.Math.Abs(val - current);
                        if (diff < difference)
                        {
                            difference = diff;
                            samplerate = val;
                        }
                    }
                }
            }
            return samplerate;
        }

        protected void AdjustBitrateLimit(
            IntRange bitrateRange,
            VideoRateControl rateControl)
        {
            if (bitrateRange != null)
            {
                if (rateControl.BitrateLimit < bitrateRange.Min)
                {
                    rateControl.BitrateLimit = bitrateRange.Min;
                }
                if (rateControl.BitrateLimit > bitrateRange.Max)
                {
                    rateControl.BitrateLimit = bitrateRange.Max;
                }
            }
        }

        protected void AdjustVideoEncoderConfiguration(
            VideoEncoding encoding,
            VideoEncoderConfiguration config,
            VideoEncoderConfigurationOptions options)
        {
            Action<IntRange, VideoRateControl> adjustBitrateLimit = (IntRange bitrateRange, VideoRateControl rateControl) =>
            {
                if (bitrateRange != null)
                {
                    if (rateControl.BitrateLimit < bitrateRange.Min)
                    {
                        rateControl.BitrateLimit = bitrateRange.Min;
                    }
                    if (rateControl.BitrateLimit > bitrateRange.Max)
                    {
                        rateControl.BitrateLimit = bitrateRange.Max;
                    }
                }
            };

            // fix for Panasonic
            if (encoding == VideoEncoding.JPEG)
            {
                config.MPEG4 = null;
                config.H264 = null;

                // support for extensions (bitrate limits)
                if (options.Extension != null)
                {
                    if (options.Extension.JPEG != null)
                    {
                        if (config.RateControl == null)
                        {
                            config.RateControl = new VideoRateControl();
                        }
                        adjustBitrateLimit(options.Extension.JPEG.BitrateRange, config.RateControl); 
                    }
                }
            }
            if (encoding == VideoEncoding.MPEG4)
            {
                config.H264 = null;

                // support for extensions (bitrate limits)
                if (options.Extension != null)
                {
                    if (options.Extension.MPEG4 != null)
                    {
                        if (config.RateControl == null)
                        {
                            config.RateControl = new VideoRateControl();
                        }
                        adjustBitrateLimit(options.Extension.MPEG4.BitrateRange, config.RateControl); 
                    }
                }
            }
            if (encoding == VideoEncoding.H264)
            {
                config.MPEG4 = null;

                // support for extensions (bitrate limits)
                if (options.Extension != null)
                {
                    if (options.Extension.H264 != null)
                    {
                        if (config.RateControl == null)
                        {
                            config.RateControl = new VideoRateControl();
                        }
                        adjustBitrateLimit(options.Extension.H264.BitrateRange, config.RateControl); 
                    }
                }
            }
        }

        protected void AdjustJPEGVideoEncoderConfiguration(Profile profile, VideoEncoderConfigurationOptions options)
        {
            AdjustJPEGVideoEncoderConfiguration(profile.VideoEncoderConfiguration, options, true);
        }
        protected void AdjustJPEGVideoEncoderConfiguration(VideoEncoderConfiguration config, VideoEncoderConfigurationOptions options, bool updateResolution)
        {
            if (updateResolution)
            {
                Assert(null != options.JPEG, "No options for JPEG encoder are received", "Check that options for JPEG encoder are received");
                if (null != options.JPEG.ResolutionsAvailable && options.JPEG.ResolutionsAvailable.Length > 0)
                    config.Resolution = options.JPEG.ResolutionsAvailable[0];
            }
        }
        protected void AdjustMpeg4VideoEncoderConfiguration(Profile profile, VideoEncoderConfigurationOptions options)
        {
            AdjustMpeg4VideoEncoderConfiguration(profile.VideoEncoderConfiguration, options, true);
        }
        protected void AdjustMpeg4VideoEncoderConfiguration(VideoEncoderConfiguration config, VideoEncoderConfigurationOptions options, bool updateResolution)
        {
            if (config.MPEG4 == null)
            {
                config.MPEG4 = new Mpeg4Configuration();
            }

            if (options.MPEG4.Mpeg4ProfilesSupported.Contains(Mpeg4Profile.SP))
            {
                config.MPEG4.Mpeg4Profile = Mpeg4Profile.SP;
            }
            else
            {
                config.MPEG4.Mpeg4Profile = Mpeg4Profile.ASP;
            }

            if (updateResolution)
            {
                Assert(null != options.MPEG4, "No options for MPEG4 encoder are received", "Check that options for MPEG4 encoder are received");
                if (null != options.MPEG4.ResolutionsAvailable && options.MPEG4.ResolutionsAvailable.Length > 0)
                {
                    config.Resolution = options.MPEG4.ResolutionsAvailable[0];
                }
            }
            if (options.MPEG4.GovLengthRange != null)
            {
                config.MPEG4.GovLength = options.MPEG4.GovLengthRange.Min > 30
                                         ?
                                         options.MPEG4.GovLengthRange.Min
                                         :
                                         (options.MPEG4.GovLengthRange.Max < 30) ? options.MPEG4.GovLengthRange.Max : 30;
            }
            else
            {
                config.MPEG4.GovLength = 30;
            }
        }

        protected void AdjustH264VideoEncoderConfiguration(Profile profile, VideoEncoderConfigurationOptions options)
        {
            AdjustH264VideoEncoderConfiguration(profile.VideoEncoderConfiguration, options, true);
        }

        protected void AdjustH264VideoEncoderConfiguration(VideoEncoderConfiguration config, VideoEncoderConfigurationOptions options, bool updateResolution)
        {
            if (config.H264 == null)
            {
                config.H264 = new H264Configuration();
            }

            H264Profile[] profiles = options.H264.H264ProfilesSupported;
            // The parameter of H264Profile is set the highest value that DUT supports as the order is High/Extended/Main/Baseline.
            H264Profile encoderProfile = H264Profile.High;

            // if high not supported...
            if (!profiles.Contains(H264Profile.High))
            {
                if (profiles.Contains(H264Profile.Extended))
                {
                    encoderProfile = H264Profile.Extended;
                }
                else
                {
                    // if Main supported
                    if (profiles.Contains(H264Profile.Main))
                    {
                        encoderProfile = H264Profile.Main;
                    }
                    else
                    {
                        // if Extended not supported.
                        encoderProfile = H264Profile.Baseline;
                    }
                }
            }

            config.H264.H264Profile = encoderProfile;

            if (updateResolution)
            {
                Assert(null != options.H264, "No options for H264 encoder are received", "Check that options for H264 encoder are received");
                if (null != options.H264.ResolutionsAvailable && options.H264.ResolutionsAvailable.Length > 0)
                {
                    config.Resolution = options.H264.ResolutionsAvailable[0];
                }
            }
            if (options.H264.GovLengthRange != null)
            {
                config.H264.GovLength = options.H264.GovLengthRange.Min > 30
                                        ?
                                        options.H264.GovLengthRange.Min
                                        :
                                        (options.H264.GovLengthRange.Max < 30) ? options.H264.GovLengthRange.Max : 30;
            }
            else
            {
                config.H264.GovLength = 30;
            }
        }



        protected string OffsetDefaultMulticastAddress(IPType addressType, int offset)
        {
            string defaultIPv4Base = "239.0.0.";
            string defaultIPv6Base = "FF15:0000:0000:0000:0000:0000:8000:";
            return (addressType == IPType.IPv4
                    ? defaultIPv4Base + ((int)Math.Min(255, Math.Max(0, offset))).ToString()
                    : defaultIPv6Base + ((int)Math.Min(0xFFFF, Math.Max(0, offset))).ToString("X4"));
        }

        protected void SetMulticastSettings(Profile profile, bool toVideo, bool toAudio, IPType addressType)
        {
            SetMulticastSettings(profile, toVideo, toAudio, addressType, 0, 0);
        }

        protected void SetMulticastSettings(Profile profile, bool toVideo, bool toAudio, IPType addressType, int addrOffset, int portOffset)
        {
            Action<MulticastConfiguration, int> setMulticast = (MulticastConfiguration multicast, int port) =>
            {
                multicast.TTL = 1;
                multicast.Address.Type = addressType;
                if (addressType == IPType.IPv4)
                {
                    multicast.Address.IPv6Address = null;
                    multicast.Address.IPv4Address = OffsetDefaultMulticastAddress(addressType, addrOffset);
                }
                else // IPType.IPv6
                {
                    multicast.Address.IPv4Address = null;
                    multicast.Address.IPv6Address = OffsetDefaultMulticastAddress(addressType, addrOffset);
                }
                multicast.Port = port;
            };

            if (toVideo)
            {
                if (profile.VideoEncoderConfiguration.Multicast == null)
                {
                    profile.VideoEncoderConfiguration.Multicast = new MulticastConfiguration();
                }
                setMulticast(profile.VideoEncoderConfiguration.Multicast, 1234 + 4 * portOffset);
            }
            if (toAudio)
            {
                if (profile.AudioEncoderConfiguration.Multicast == null)
                {
                    profile.AudioEncoderConfiguration.Multicast = new MulticastConfiguration();
                }
                setMulticast(profile.AudioEncoderConfiguration.Multicast, 1236 + 4 * portOffset);
            }
        }

        #endregion

        #region Profile Selection

        protected delegate bool TestVideoEncoderConfigurationOptions(VideoEncoderConfigurationOptions options);

        protected delegate bool TestAudioEncoderConfigurationOptions(AudioEncoderConfigurationOptions options);

        // Annex A.14
        protected Profile SelectVideoProfile(
            string profileRequirementsDescription,
            TestVideoEncoderConfigurationOptions testVideo,
            out VideoEncoderConfigurationOptions videoOptions)
        {
            Profile[] profiles = GetProfiles();
            Profile profile = null;
            VideoEncoderConfigurationOptions videoOptionsTmp = null;

            RunStep(() =>
            {
                foreach (Profile p in profiles)
                {
                    LogStepEvent(string.Format("Check if {0} profile supports {1} Video encoder configuration",
                        p.Name, profileRequirementsDescription));
                    if (p.VideoEncoderConfiguration != null)
                    {
                        videoOptionsTmp =
                            Client.GetVideoEncoderConfigurationOptions(p.VideoEncoderConfiguration.token, p.token);

                        DoRequestDelay();

                        if (testVideo(videoOptionsTmp))
                        {
                            profile = p;
                            break;
                        }
                    }
                }
            },
            string.Format("Select profile with {0} Video encoder configuration", profileRequirementsDescription));

            Assert(profile != null,
                string.Format("Profile with {0} Video encoder configuration not found", profileRequirementsDescription),
                "Check if required profile found");

            videoOptions = videoOptionsTmp;

            return profile;
        }
        protected Profile SelectVideoProfile(
            VideoEncoding videoEncoding,
            out VideoEncoderConfigurationOptions videoOptions)
        {
            return SelectVideoProfile(
                GetVideoCodecName(videoEncoding),
                options => (CheckVideoSupport(options, videoEncoding)), 
                out videoOptions);
        }

        // Annex A.15
        protected Profile SelectVideoAudioProfile(
            string videoCodec,
            TestVideoEncoderConfigurationOptions testVideo,
            string audioCodec,
            TestAudioEncoderConfigurationOptions testAudio,
            out VideoEncoderConfigurationOptions videoOptions,
            out AudioEncoderConfigurationOptions audioOptions,
            MediaConfigurationChangeLog changeLog)
        {
            Profile[] profiles = GetProfiles();
            Profile profile = null;
            VideoEncoderConfigurationOptions videoOptionsTmp = null;
            AudioEncoderConfigurationOptions audioOptionsTmp = null;


            RunStep(() =>
            {
                foreach (Profile p in profiles)
                {
                    LogStepEvent(string.Format("Check if {0} profile satisfies current needs", p.Name));

                    if (p.VideoEncoderConfiguration != null)
                    {
                        LogStepEvent("GetVideoEncoderConfigurationOptions");
                        videoOptionsTmp =
                            Client.GetVideoEncoderConfigurationOptions(p.VideoEncoderConfiguration.token, p.token);

                        DoRequestDelay();

                        if (testVideo(videoOptionsTmp))
                        {
                            // Video configuration OK  - configure Audio, if needed.

                            if (p.AudioEncoderConfiguration != null && p.AudioSourceConfiguration != null)
                            {
                                LogStepEvent("GetAudioEncoderConfigurationOptions");
                                audioOptionsTmp =
                                    Client.GetAudioEncoderConfigurationOptions(p.AudioEncoderConfiguration.token, p.token);

                                DoRequestDelay();

                                if (testAudio(audioOptionsTmp))
                                {
                                    profile = p;
                                    LogStepEvent("OK - profile found");
                                    break;
                                }
                            }
                            else
                            {
                                LogStepEvent("GetAudioEncoderConfigurations");
                                AudioEncoderConfiguration[] audioEncoderConfigurations =
                                    Client.GetAudioEncoderConfigurations();

                                DoRequestDelay();

                                bool audioEncoderConfigurationFound = false;

                                foreach (AudioEncoderConfiguration configuration in audioEncoderConfigurations)
                                {
                                    LogStepEvent("GetAudioEncoderConfigurationOptions");
                                    audioOptionsTmp =
                                        Client.GetAudioEncoderConfigurationOptions(configuration.token, p.token);

                                    DoRequestDelay();

                                    if (testAudio(audioOptionsTmp))
                                    {
                                        if (p.AudioSourceConfiguration == null)
                                        {
                                            AudioSourceConfiguration[] audioSourceConfigurations = Client.GetAudioSourceConfigurations();

                                            DoRequestDelay();

                                            if (audioSourceConfigurations.Length > 0)
                                            {
                                                {
                                                    Profile copy = Utils.CopyMaker.CreateCopy(p);
                                                    changeLog.TrackModifiedProfile(copy);
                                                }
                                                LogStepEvent("AddAudioSourceConfiguration");
                                                Client.AddAudioSourceConfiguration(p.token, audioSourceConfigurations[0].token);
                                                DoRequestDelay();
                                            }
                                            else
                                            {
                                                throw new DutPropertiesException("Audio Source Configurations not found");
                                            }
                                        }

                                        {
                                            Profile copy = Utils.CopyMaker.CreateCopy(p);
                                            changeLog.TrackModifiedProfile(copy);
                                        }
                                        LogStepEvent("AddAudioEncoderConfiguration");
                                        Client.AddAudioEncoderConfiguration(p.token, configuration.token);
                                        DoRequestDelay();

                                        p.AudioEncoderConfiguration = configuration;

                                        profile = p;

                                        LogStepEvent(string.Format("Add Audio configuration to the {0} profile - OK", profile.Name));

                                        audioEncoderConfigurationFound = true;
                                        break;
                                    }
                                }

                                if (!audioEncoderConfigurationFound)
                                {
                                    throw new DutPropertiesException("Audio Encoder Configuration with required properties not found");
                                }
                            }
                        }
                    }
                }

                if (profile == null)
                {
                    throw new DutPropertiesException("Respective profile cannot be found or created");
                }
            },
            string.Format("Select or create profile with {0} Video encoder configuration and {1} Audio encoder configuration",
                          videoCodec, audioCodec));

            videoOptions = videoOptionsTmp;
            audioOptions = audioOptionsTmp;

            return profile;
        }

        // ANNEX A.X1 Media Profile Configuration for Audio Streaming
        protected Profile SelectAudioProfile(
            string audioCodec,
            TestAudioEncoderConfigurationOptions testAudio,
            out AudioEncoderConfigurationOptions audioOptions,
            MediaConfigurationChangeLog changeLog)
        {
            Profile[] profiles = GetProfiles();
            Profile profile = null;
            AudioEncoderConfigurationOptions audioOptionsTmp = null;

            RunStep(() =>
            {
                foreach (Profile p in profiles)
                {
                    if (p.AudioEncoderConfiguration != null && p.AudioSourceConfiguration != null)
                    {
                        LogStepEvent("GetAudioEncoderConfigurationOptions");
                        audioOptionsTmp =
                            Client.GetAudioEncoderConfigurationOptions(p.AudioEncoderConfiguration.token, p.token);

                        DoRequestDelay();

                        if (testAudio(audioOptionsTmp))
                        {
                            profile = p;
                            LogStepEvent("OK - profile found");
                            break;
                        }
                    }
                    else
                    {
                        LogStepEvent("GetAudioEncoderConfigurations");
                        AudioEncoderConfiguration[] audioEncoderConfigurations =
                            Client.GetAudioEncoderConfigurations();

                        DoRequestDelay();

                        bool audioEncoderConfigurationFound = false;

                        foreach (AudioEncoderConfiguration configuration in audioEncoderConfigurations)
                        {
                            LogStepEvent("GetAudioEncoderConfigurationOptions");
                            audioOptionsTmp =
                                Client.GetAudioEncoderConfigurationOptions(configuration.token, p.token);

                            DoRequestDelay();

                            if (testAudio(audioOptionsTmp))
                            {
                                if (p.AudioSourceConfiguration == null)
                                {
                                    AudioSourceConfiguration[] audioSourceConfigurations = Client.GetAudioSourceConfigurations();

                                    DoRequestDelay();

                                    if (audioSourceConfigurations.Length > 0)
                                    {
                                        {
                                            Profile copy = Utils.CopyMaker.CreateCopy(p);
                                            changeLog.TrackModifiedProfile(copy);                                        
                                        }

                                        LogStepEvent("AddAudioSourceConfiguration");
                                        Client.AddAudioSourceConfiguration(p.token, audioSourceConfigurations[0].token);
                                        DoRequestDelay();
                                    }
                                    else
                                    {
                                        throw new DutPropertiesException("Audio Source Configurations not found");
                                    }
                                }

                                {
                                    Profile copy = Utils.CopyMaker.CreateCopy(p);
                                    changeLog.TrackModifiedProfile(copy);
                                }

                                LogStepEvent("AddAudioEncoderConfiguration");
                                Client.AddAudioEncoderConfiguration(p.token, configuration.token);
                                DoRequestDelay();

                                p.AudioEncoderConfiguration = configuration;

                                profile = p;

                                LogStepEvent(string.Format("Add Audio configuration to the {0} profile - OK", profile.Name));

                                audioEncoderConfigurationFound = true;
                                break;
                            }
                        }

                        if (!audioEncoderConfigurationFound)
                        {
                            throw new DutPropertiesException("Audio Encoder Configuration with required properties not found");
                        }
                    }
                }

                if (profile == null)
                {
                    throw new DutPropertiesException("Respective profile cannot be found or created");
                }
            },
            string.Format("Select or create profile with {0} Audio encoder configuration", audioCodec));

            audioOptions = audioOptionsTmp;

            return profile;
        }


        /// <summary>
        /// Selects profile which a) has VideoEncoderConfiguration with encoding specified 
        /// or b) can be modified to use encoding specified
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected Profile GetProfileForSpecificCodec(VideoEncoding encoding,
            out VideoEncoderConfigurationOptions options)
        {
            Profile profile = null;
            options = null;

            Profile[] profiles = GetProfiles();
            Assert(profiles != null && profiles.Length > 0,
                "The DUT return no profiles", "Check if the DUT returned any profiles");

            foreach (Profile p in profiles)
            {
                if (p.VideoEncoderConfiguration != null && p.VideoEncoderConfiguration.Encoding == encoding)
                {
                    profile = p;
                    break;
                }
            }

            if (profile == null)
            {
                foreach (Profile p in profiles)
                {
                    VideoEncoderConfigurationOptions opt = GetVideoEncoderConfigurationOptions(null, p.token);

                    bool canBeConfigured = OptionsAllowEncoding(opt, encoding);

                    if (canBeConfigured)
                    {
                        options = opt;
                        profile = p;
                        break;
                    }
                }
            }

            return profile;
        }

        protected bool OptionsAllowEncoding(VideoEncoderConfigurationOptions opt, VideoEncoding encoding)
        {
            return (encoding == VideoEncoding.JPEG && opt.JPEG != null) ||
                        (encoding == VideoEncoding.H264 && opt.H264 != null) ||
                        (encoding == VideoEncoding.MPEG4 && opt.MPEG4 != null);

        }

        #endregion

        #region IVideoFormEvent

        List<string> _stepMessages = new List<string>();

        public void FireBeginStep(string Name)
        {
            _stepMessages.Clear();
            BeginStep(Name);
        }
        public void FireStepPassed()
        {
            StepPassed();
        }
        public void FireLogStepEvent(string Message)
        {
            bool rtspCommand = false;

            if (Message.StartsWith("DESCRIBE_REQUEST"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "DESCRIBE_REQUEST=".Length);
                FillRtspRequest(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("DESCRIBE_RESPONSE"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "DESCRIBE_RESPONSE=".Length);
                FillRtspResponse(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("SETUP_REQUEST"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "SETUP_REQUEST=".Length);
                FillRtspRequest(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("SETUP_RESPONSE"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "SETUP_RESPONSE=".Length);
                FillRtspResponse(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("PLAY_REQUEST"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "PLAY_REQUEST=".Length);
                FillRtspRequest(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("PLAY_RESPONSE"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "PLAY_RESPONSE=".Length);
                FillRtspResponse(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("TEARDOWN_REQUEST"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "TEARDOWN_REQUEST=".Length);
                FillRtspRequest(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("TEARDOWN_RESPONSE"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "TEARDOWN_RESPONSE=".Length);
                FillRtspResponse(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("PAUSE_REQUEST"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "PAUSE_REQUEST=".Length);
                FillRtspRequest(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("PAUSE_RESPONSE"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "PAUSE_RESPONSE=".Length);
                FillRtspResponse(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("OPTIONS_REQUEST"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "OPTIONS_REQUEST=".Length);
                FillRtspRequest(Message);

                rtspCommand = true;
            }
            else if (Message.StartsWith("OPTIONS_RESPONSE"))
            {
                Message = Message.Replace("!!", "\r\n");
                Message = Message.Remove(0, "OPTIONS_RESPONSE=".Length);
                FillRtspResponse(Message);

                rtspCommand = true;
            }

            if (!_stepMessages.Contains(Message))
            {
                _stepMessages.Add(Message);

                if (rtspCommand) return;

                LogStepEvent(Message);
            }           
        }

        #endregion


        protected virtual void ValidateStreamSequence(bool CheckAudio, bool CheckVideo)
        {
            _videoForm.EventSink = this;
            VideoIsOpened = true;
            _videoForm.OpenWindow(CheckAudio, CheckVideo);

            Sleep(_operationDelay);

            _videoForm.CloseWindow();
            VideoIsOpened = false;
            _videoForm.EventSink = null; 
        }

        protected virtual void VideoCleanup()
        {
            if (VideoIsOpened)
            {
                _videoForm.CloseWindow();
                VideoIsOpened = false;
                _videoForm.EventSink = null;
            }
        }

        protected string GetAudioCodecName(AudioEncoding audioEncoding)
        {
            switch (audioEncoding)
            {
                case AudioEncoding.AAC: return "AAC";
                case AudioEncoding.G711: return "G.711";
                case AudioEncoding.G726: return "G.726";
            }
            return null;
        }

        protected string GetAudioCodecFormatName(AudioEncoding audioEncoding)
        {
            switch (audioEncoding)
            {
                case AudioEncoding.AAC: return "MPEG4-GENERIC";
                case AudioEncoding.G711: return "PCMU";
                case AudioEncoding.G726: return "G726";
            } 
            return null;
        }

        protected string GetVideoCodecName(VideoEncoding videoEncoding)
        {
            switch (videoEncoding)
            {
                case VideoEncoding.H264: return "H264";
                case VideoEncoding.JPEG: return "JPEG";
                case VideoEncoding.MPEG4: return "MPEG4";
            }
            return null;
        }

        protected string GetVideoCodecFormatName(VideoEncoding videoEncoding)
        {
            switch (videoEncoding)
            {
                case VideoEncoding.H264: return "H264";
                case VideoEncoding.JPEG: return "JPEG";
                case VideoEncoding.MPEG4: return "MP4V-ES";
            }
            return null;
        }

        protected bool CheckAudioSupport(AudioEncoderConfigurationOptions options, AudioEncoding audioEncoding)
        {
            return (options.Options != null) 
                    && (options.Options.Where(o => o.Encoding == audioEncoding).FirstOrDefault() != null);
        }

        protected bool CheckVideoSupport(VideoEncoderConfigurationOptions options, VideoEncoding videoEncoding)
        {
            switch (videoEncoding)
            {
                case VideoEncoding.H264: return options.H264 != null;
                case VideoEncoding.JPEG: return options.JPEG != null;
                case VideoEncoding.MPEG4: return options.MPEG4 != null;
            }
            return false;
        }

        protected void SetAudioEncoding(AudioEncoderConfiguration config, AudioEncoding encoding, AudioEncoderConfigurationOptions options)
        {
            config.Encoding = encoding;
            // find nearest bitrate and samplerate
            config.Bitrate = FindNearestAudioBitrate(config.Bitrate, encoding, options);
            config.SampleRate = FindNearestAudioSamplerate(config.SampleRate, encoding, options);
        }

        protected void SetVideoEncoding(VideoEncoderConfiguration config, VideoEncoding encoding, VideoEncoderConfigurationOptions options)
        {
            config.Encoding = encoding;
            AdjustVideoEncoderConfiguration(encoding, config, options);
        }

        protected void StartMulticastStreaming(
            out Profile profile,
            VideoEncoding? videoEncoding,
            AudioEncoding? audioEncoding,
            MediaConfigurationChangeLog changeLog)
        {
            bool useVideo = videoEncoding.HasValue;
            bool useAudio = audioEncoding.HasValue;

            VideoEncoderConfigurationOptions videoOptions = null;
            AudioEncoderConfigurationOptions audioOptions = null;

            if (useVideo && !useAudio)
            {
                profile = SelectVideoProfile(
                    GetVideoCodecName(videoEncoding.Value),
                    options => (CheckVideoSupport(options, videoEncoding.Value)),
                    out videoOptions);
            }
            else if (!useVideo && useAudio)
            {
                profile = SelectAudioProfile(
                    GetAudioCodecName(audioEncoding.Value),
                    options => (CheckAudioSupport(options, audioEncoding.Value)),
                    out audioOptions, changeLog);
            }
            else
            {
                profile = SelectVideoAudioProfile(
                    GetVideoCodecName(videoEncoding.Value),
                    options => (CheckVideoSupport(options, videoEncoding.Value)),
                    GetAudioCodecName(audioEncoding.Value),
                    options => (CheckAudioSupport(options, audioEncoding.Value)),
                    out videoOptions,
                    out audioOptions, changeLog);
            }

            { 
            
            }

            if (useVideo)
            { 
                // save Video Encoder configuration
                VideoEncoderConfiguration vecCopy = Utils.CopyMaker.CreateCopy(profile.VideoEncoderConfiguration);
                changeLog.TrackModifiedConfiguration(vecCopy);
            }
            if (useAudio)
            {
                // Save Audio encoder configuration
                AudioEncoderConfiguration aecCopy = Utils.CopyMaker.CreateCopy(profile.AudioEncoderConfiguration);
                changeLog.TrackModifiedConfiguration(aecCopy);
            }
            
            SetMulticastSettings(profile, useVideo, useAudio, IPType.IPv4);

            if (useVideo)
            {
                SetVideoEncoding(profile.VideoEncoderConfiguration, videoEncoding.Value, videoOptions);
                SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, true);
            }
            if (useAudio)
            {
                SetAudioEncoding(profile.AudioEncoderConfiguration, audioEncoding.Value, audioOptions);
                SetAudioEncoderConfiguration(profile.AudioEncoderConfiguration, false, true);
            }

            VideoUtils.AdjustVideo(
                _videoForm, null, null, _messageTimeout, TransportProtocol.UDP,
                StreamType.RTPMulticast, null, profile.VideoEncoderConfiguration);

            string token = profile.token;
            RunStep(
                () => { Client.StartMulticastStreaming(token); },
                "StartMulticastStreaming");

            DoRequestDelay();

            if (useVideo)
            {
                int fps = 1;
                if (profile.VideoEncoderConfiguration.RateControl != null)
                {
                    fps = profile.VideoEncoderConfiguration.RateControl.FrameRateLimit;
                }
                else
                {

                    if (videoOptions.JPEG.FrameRateRange != null)
                    {
                        fps = videoOptions.JPEG.FrameRateRange.Min;
                    }
                }
                _videoForm.VideoFPS = fps;
                _videoForm.MulticastRtpPortVideo = profile.VideoEncoderConfiguration.Multicast.Port;
                _videoForm.VideoCodecName = GetVideoCodecFormatName(videoEncoding.Value);
                _videoForm.MulticastAddress = profile.VideoEncoderConfiguration.Multicast.Address.IPv4Address;
                _videoForm.MulticastTTL = profile.VideoEncoderConfiguration.Multicast.TTL;
            }
            if (useAudio)
            {
                _videoForm.MulticastRtpPortAudio = profile.AudioEncoderConfiguration.Multicast.Port;
                _videoForm.AudioCodecName = GetAudioCodecFormatName(audioEncoding.Value);
                _videoForm.MulticastAddress = profile.AudioEncoderConfiguration.Multicast.Address.IPv4Address;
                _videoForm.MulticastTTL = profile.AudioEncoderConfiguration.Multicast.TTL;
            }


            ValidateStreamSequence(useAudio, useVideo);
        }

        protected void StopMulticastStreaming(Profile profile)
        {
            if (profile != null)
            {
                RunStep(
                    () => { Client.StopMulticastStreaming(profile.token); },
                    "StopMulticastStreaming");

                DoRequestDelay();
            }
            VideoCleanup();
        }


        protected bool CreateOrSelectAudioProfile(ref Profile profile, AudioEncoding encoding, MediaConfigurationChangeLog changeLog)
        {
            // profile creating
            string profileName = "TestProfileX";
            bool notFixedProfileFound = false;

            while (profile == null)
            {
                BeginStep(string.Format(Resources.StepCreateMediaProfile_Format, profileName));
                try
                {
                    profile = Client.CreateProfile(profileName, null);
                    DoRequestDelay();
                    StepPassed();
                    changeLog.CreatedProfiles.Add(profile);
                    return true;
                }
                catch (FaultException exc)
                {
                    LogFault(exc);
                    string err;
                    if (exc.IsValidOnvifFault("Receiver/Action/MaxNVTProfiles", out err))
                    {
                        LogStepEvent("The maximum number of  supported profiles has been reached.");
                    }
                    StepPassed();
                }

                // if MaxNVTProfiles was returned then A2
                if (profile == null)
                {
                    Profile[] profiles = GetProfiles();
                    Assert(profiles != null && profiles.Length > 0,
                           Resources.ErrorNoMediaProfiles_Text,
                           Resources.StepValidatingProfiles_Title);

                    foreach (Profile p in profiles)
                    {
                        if (p.@fixed == false)
                        {
                            notFixedProfileFound = true;
                            DeleteProfile(p.token);
                            changeLog.TrackDeletedProfile(p);
                            break;
                        }
                    }

                    if (notFixedProfileFound == false)
                    {
                        //AudioEncoderConfigurationOptions options = null;
                        //foreach (Profile p in profiles)
                        //{
                        //    if (p.AudioSourceConfiguration != null && p.AudioEncoderConfiguration != null &&
                        //        p.VideoSourceConfiguration == null && p.VideoEncoderConfiguration == null &&
                        //        p.VideoAnalyticsConfiguration == null && p.MetadataConfiguration == null)
                        //    {
                        //        options = GetAudioEncoderConfigurationOptions(null, p.token);
                        //        // checking for audio codec support
                        //        if (CheckAudioSupport(options, encoding))
                        //        {
                        //            LogTestEvent(string.Format("Profile with {0} support found - OK", GetAudioCodecName(encoding)));
                        //            profile = p;
                        //            fixedProfileFound = true;
                        //            changeLog.TrackModifiedProfile(profile);
                        //            return true;
                        //        }
                        //    }
                        //}

                        //// no existing profile with audio codec support
                        //if (notFixedProfileFound == false && fixedProfileFound == false)
                        //{
                        //    LogTestEvent(string.Format("Profile with {0} support cannot be found or created", GetAudioCodecName(encoding)));
                        //    // test should be passed if there is no appropriate ready profiles
                        //    // or we can't create new profile
                        //    return false;
                        //}

                        // If there are no profiles with fixed=”false” remove all configurations from one 
                        // fixed profile and use this profile for test. If there are no profiles skip other 
                        // steps and fail test.
                        Profile firstProfile = profiles[0];
                        {
                            Profile backup = Utils.CopyMaker.CreateCopy(firstProfile);
                            changeLog.TrackModifiedProfile(backup);
                        }

                        if (firstProfile.AudioEncoderConfiguration != null)
                        {
                            RemoveAudioEncoderConfiguration(firstProfile.token);
                        }
                        if (firstProfile.AudioSourceConfiguration != null)
                        {
                            RemoveAudioSourceConfiguration(firstProfile.token);
                        }
                        if (firstProfile.PTZConfiguration != null)
                        {
                            RemovePTZConfiguration(firstProfile.token);
                        }                        
                        if (firstProfile.VideoEncoderConfiguration != null)
                        {
                            RemoveVideoEncoderConfiguration(firstProfile.token);
                        }
                        if (firstProfile.VideoSourceConfiguration != null)
                        {
                            RemoveVideoSourceConfiguration(firstProfile.token);
                        }
                        if (firstProfile.MetadataConfiguration != null)
                        {
                            RemoveMetadataConfiguration(firstProfile.token);
                        }
                        firstProfile.AudioEncoderConfiguration = null;
                        firstProfile.AudioSourceConfiguration = null;
                        firstProfile.MetadataConfiguration = null;
                        firstProfile.PTZConfiguration = null;
                        firstProfile.VideoEncoderConfiguration = null;
                        firstProfile.VideoSourceConfiguration = null;
                        profile = firstProfile;
                        return true;
                    }
                }
            }

            return false;
        }

        protected void SelectAudioSourceEncoderConfigs(ref Profile profile, AudioEncoding encoding,
                                                       ref AudioEncoderConfigurationOptions audioOptions)
        {
            string reason;
            AudioEncoderConfiguration encoderConfig = null;

            if (profile.AudioSourceConfiguration == null && profile.AudioEncoderConfiguration == null)
            {
                // audio source config getting
                AudioSourceConfiguration[] sourceConfigs = GetAudioSourceConfigurations();
                Assert(ValidateAudioSourceConfigs(sourceConfigs, out reason), reason, Resources.StepValidatingAudioSources_Title);

                // searching for compatible audio encoder config which supports necessary audio codec
                foreach (AudioSourceConfiguration sourceConfig in sourceConfigs)
                {
                    AddAudioSourceConfiguration(profile.token, sourceConfig.token);
                    profile.AudioSourceConfiguration = sourceConfig;

                    AudioEncoderConfiguration[] compatibleConfigs = GetCompatibleAudioEncoderConfigurations(profile.token);
                    Assert(ValidateAudioEncoderConfigs(compatibleConfigs, out reason), reason, Resources.StepValidatingAudioEncoders_Title);

                    AudioEncoderConfigurationOptions options = null;
                    string profileToken = profile.token;
                    RunStep(() =>
                    {
                        foreach (AudioEncoderConfiguration compatibleConfig in compatibleConfigs)
                        {
                            // audio encoder options getting
                            LogStepEvent("GetAudioEncoderConfigurationOptions");
                            options = Client.GetAudioEncoderConfigurationOptions(compatibleConfig.token, profileToken);
                            if (options == null)
                            {
                                throw new AssertException(Resources.ErrorInvalidAudioEncoderConfigOptions_Text);
                            }
                            DoRequestDelay();

                            // checking for audio codec support
                            if (CheckAudioSupport(options, encoding))
                            {
                                encoderConfig = compatibleConfig;
                                LogStepEvent("Audio encoder configuration found - OK");
                                break;
                            }
                        }
                    },
                    string.Format("Select audio encoder configuration with {0} audio codec support", GetAudioCodecName(encoding)));

                    audioOptions = options;

                    // if config which audio codec support exists for current audio source and profile
                    if (encoderConfig != null)
                    {
                        // found audio encoder config adding
                        AddAudioEncoderConfiguration(profile.token, encoderConfig.token);
                        profile.AudioEncoderConfiguration = encoderConfig;
                        break;
                    }
                }

                // if there is no any config which support necessary audio codec for all available audio sources
                if (encoderConfig == null)
                {
                    Assert((encoderConfig != null), string.Format("No audio encoder with {0} encoding support", GetAudioCodecName(encoding)),
                        string.Format("Searching for audio encoder configuration with {0} encoding support", GetAudioCodecName(encoding)));
                }
            }
        }

        protected void AudioMulticastStreamingSetup(
            ref Profile profile, AudioEncoderConfigurationOptions audioOptions,
            AudioEncoding encoding, TransportProtocol protocol,
            StreamType streamType, IPType? multicastAddressType, 
            out bool started, 
            MediaConfigurationChangeLog changeLog)
        {
            started = false;
            // Track as modified. 
            {
                AudioEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(profile.AudioEncoderConfiguration);
                changeLog.TrackModifiedConfiguration(configCopy);
            }

            if (audioOptions == null)
            {
                audioOptions = GetAudioEncoderConfigurationOptions(
                    profile.AudioEncoderConfiguration.token, profile.token);
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
            VideoUtils.AdjustVideo(
                _videoForm, null, null, _messageTimeout, protocol, streamType, null, null);

            string token = profile.token;
            RunStep(
                () => { Client.StartMulticastStreaming(token); },
                "StartMulticastStreaming");

            started = true;
            DoRequestDelay();

            _videoForm.MulticastRtpPortAudio = profile.AudioEncoderConfiguration.Multicast.Port;
            _videoForm.AudioCodecName = GetAudioCodecFormatName(encoding);
            _videoForm.MulticastAddressAudio = profile.AudioEncoderConfiguration.Multicast.Address.IPv4Address;
            _videoForm.MulticastTTL = profile.AudioEncoderConfiguration.Multicast.TTL;

            // validate audio sequence
            ValidateStreamSequence(true, false);    
        }

        protected void AudioProfileRemoving(ref Profile profile)
        {
            // profile removing
            if (profile != null && profile.@fixed == false)
            {
                if (profile.AudioEncoderConfiguration != null)
                {
                    RemoveAudioEncoderConfiguration(profile.token);
                }

                if (profile.AudioSourceConfiguration != null)
                {
                    RemoveAudioSourceConfiguration(profile.token);
                }

                DeleteProfile(profile.token);
                profile = null;
            }
        }


        #region UNDO CHANGES

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeLog"></param>
        /// <remarks>ToDo: switch to using methods from CommonMethodsProvider</remarks>
        public void RestoreMediaConfiguration(MediaConfigurationChangeLog changeLog)
        {
            foreach (Profile p in changeLog.CreatedProfiles)
            {
                DeleteProfile(p.token);
            }
            foreach (Profile p in changeLog.DeletedProfiles)
            {
                RestoreDeletedProfile(p);
            }
            foreach (Profile p in changeLog.ModifiedProfiles)
            {
                ResetProfile(p);
            }

            foreach (VideoEncoderConfiguration config in changeLog.ModifiedVideoEncoderConfigurations)
            {
                SetVideoEncoderConfiguration(config, false);
            }
            foreach (AudioEncoderConfiguration config in changeLog.ModifiedAudioEncoderConfigurations)
            {
                SetAudioEncoderConfiguration(config, false);
            }
            foreach (MetadataConfiguration config in changeLog.ModifiedMetadataConfigurations)
            {
                SetMetadataConfiguration(config, false);
            }
        }

        /// <summary>
        /// Copied from MediaServiceTestSuite.Utils.cs
        /// ToDo : check if classes tree can be updated to avoid duplicating code
        /// </summary>
        /// <param name="profile"></param>
        /// <remarks>ToDo: switch to using methods from CommonMethodsProvider</remarks>
        protected void ResetProfile(Profile profile)
        {
            LogTestEvent(string.Format("Restore profile '{0}' used for test{1}", profile.token, Environment.NewLine));

            Profile actual = GetProfile(profile.token, "Get actual profile");

            if (profile.VideoEncoderConfiguration == null)
            {
                if (actual.VideoEncoderConfiguration != null)
                {
                    RemoveVideoEncoderConfiguration(profile.token);
                }
            }

            if (profile.AudioEncoderConfiguration == null)
            {
                if (actual.AudioEncoderConfiguration != null)
                {
                    RemoveAudioEncoderConfiguration(profile.token);
                }
            }

            if (profile.VideoSourceConfiguration != null)
            {
                if (actual.VideoSourceConfiguration == null ||
                    actual.VideoSourceConfiguration.token != profile.VideoSourceConfiguration.token)
                {
                    AddVideoSourceConfiguration(profile.token, profile.VideoSourceConfiguration.token);
                }
            }
            else
            {
                if (actual.VideoSourceConfiguration != null)
                {
                    RemoveVideoSourceConfiguration(profile.token);
                }
            }

            if (profile.AudioSourceConfiguration != null)
            {
                if (actual.AudioSourceConfiguration == null ||
                    actual.AudioSourceConfiguration.token != profile.AudioSourceConfiguration.token)
                {
                    AddAudioSourceConfiguration(profile.token, profile.AudioSourceConfiguration.token);
                }
            }
            else
            {
                if (actual.AudioSourceConfiguration != null)
                {
                    RemoveAudioSourceConfiguration(profile.token);
                }
            }

            if (profile.VideoEncoderConfiguration != null)
            {
                if (actual.VideoEncoderConfiguration == null ||
                    actual.VideoEncoderConfiguration.token != profile.VideoEncoderConfiguration.token)
                {
                    AddVideoEncoderConfiguration(profile.token, profile.VideoEncoderConfiguration.token);
                }
            }

            if (profile.AudioEncoderConfiguration != null)
            {
                if (actual.AudioEncoderConfiguration == null ||
                    actual.AudioEncoderConfiguration.token != profile.AudioEncoderConfiguration.token)
                {
                    AddAudioEncoderConfiguration(profile.token, profile.AudioEncoderConfiguration.token);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        /// <remarks>ToDo: switch to using methods from CommonMethodsProvider</remarks>
        protected void RestoreDeletedProfile(Profile profile)
        {
            LogTestEvent(string.Format("Restore profile '{0}' deleted during the test{1}", profile.token, Environment.NewLine));

            Profile actual = CreateProfile(profile.Name, profile.token);

            if (profile.VideoSourceConfiguration != null)
            {
                AddVideoSourceConfiguration(profile.token, profile.VideoSourceConfiguration.token);
            }
            if (profile.AudioSourceConfiguration != null)
            {
                AddAudioSourceConfiguration(profile.token, profile.AudioSourceConfiguration.token);
            }
            if (profile.VideoEncoderConfiguration != null)
            {
                AddVideoEncoderConfiguration(profile.token, profile.VideoEncoderConfiguration.token);
            }
            if (profile.AudioEncoderConfiguration != null)
            {
                AddAudioEncoderConfiguration(profile.token, profile.AudioEncoderConfiguration.token);
            }
            if (profile.MetadataConfiguration != null)
            {
                AddMetadataConfiguration(profile.token, profile.MetadataConfiguration.token);
            }
            if (profile.PTZConfiguration != null)
            {
                AddPTZConfiguration(profile.token, profile.PTZConfiguration.token);
            }
            if (profile.VideoAnalyticsConfiguration != null)
            {
                AddVideoAnalyticsConfiguration(profile.token, profile.VideoAnalyticsConfiguration.token);
            }
        }


        #endregion
    }
}