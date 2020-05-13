using System;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Exceptions;
using TestTool.Proxies.Onvif;

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

        int FindNearestAudioBitrate(int current, AudioEncoding encoding, AudioEncoderConfigurationOptions options)
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

        int FindNearestAudioSamplerate(int current, AudioEncoding encoding, AudioEncoderConfigurationOptions options)
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
                        adjustBitrateLimit(options.Extension.H264.BitrateRange, config.RateControl); 
                    }
                }
            }
        }

        protected void AdjustMpeg4VideoEncoderConfiguration(Profile profile, VideoEncoderConfigurationOptions options)
        {
            if (profile.VideoEncoderConfiguration.MPEG4 == null)
            {
                profile.VideoEncoderConfiguration.MPEG4 = new Mpeg4Configuration();
            }

            profile.VideoEncoderConfiguration.MPEG4.Mpeg4Profile = Mpeg4Profile.SP;
            if (options.MPEG4.GovLengthRange != null)
            {
                profile.VideoEncoderConfiguration.MPEG4.GovLength =
                    options.MPEG4.GovLengthRange.Min > 30 ? options.MPEG4.GovLengthRange.Min : 30;
            }
            else
            {
                profile.VideoEncoderConfiguration.MPEG4.GovLength = 30;
            }
        }

        protected void AdjustH264VideoEncoderConfiguration(Profile profile, VideoEncoderConfigurationOptions options)
        {
            if (profile.VideoEncoderConfiguration.H264 == null)
            {
                profile.VideoEncoderConfiguration.H264 = new H264Configuration();
            }

            profile.VideoEncoderConfiguration.H264.H264Profile = H264Profile.Baseline;
            if (options.H264.GovLengthRange != null)
            {
                profile.VideoEncoderConfiguration.H264.GovLength =
                    options.H264.GovLengthRange.Min > 30 ? options.H264.GovLengthRange.Min : 30;
            }
            else
            {
                profile.VideoEncoderConfiguration.H264.GovLength = 30;
            }
        }

        protected void SetMulticastSettings(Profile profile, bool toVideo, bool toAudio, IPType addressType)
        {
            Action<MulticastConfiguration, int> setMulticast = (MulticastConfiguration multicast, int port) =>
            {
                multicast.TTL = 1;
                multicast.Address.Type = addressType;
                if (addressType == IPType.IPv4)
                {
                    multicast.Address.IPv6Address = null;
                    multicast.Address.IPv4Address = "239.0.0.0";
                }
                else // IPType.IPv6
                {
                    multicast.Address.IPv4Address = null;
                    multicast.Address.IPv6Address = "FFFF:0000:0000:0000:0000:0000:0000:0000";
                }
                multicast.Port = port;
            };

            if (toVideo)
            {
                if (profile.VideoEncoderConfiguration.Multicast == null)
                {
                    profile.VideoEncoderConfiguration.Multicast = new MulticastConfiguration();
                }
                setMulticast(profile.VideoEncoderConfiguration.Multicast, 1234);
                //profile.VideoEncoderConfiguration.SessionTimeout = "PT60S";
            }
            if (toAudio)
            {
                if (profile.AudioEncoderConfiguration.Multicast == null)
                {
                    profile.AudioEncoderConfiguration.Multicast = new MulticastConfiguration();
                }
                setMulticast(profile.AudioEncoderConfiguration.Multicast, 1236);
                //profile.AudioEncoderConfiguration.SessionTimeout = "PT60S";
            }
        }

        #endregion

        #region Profile Selection

        protected delegate bool TestVideoEncoderConfigurationOptions(VideoEncoderConfigurationOptions options);

        protected delegate bool TestAudioEncoderConfigurationOptions(AudioEncoderConfigurationOptions options);

        // Annex A.14
        protected Profile SelectVideoProfile(
            VideoEncoding videoEncoding,
            string profileRequirementsDescription,
            TestVideoEncoderConfigurationOptions testVideo,
            ref VideoEncoderConfigurationOptions videoOptions)
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

            profile.VideoEncoderConfiguration.Encoding = videoEncoding;
            videoOptions = videoOptionsTmp;

            AdjustVideoEncoderConfiguration(videoEncoding, profile.VideoEncoderConfiguration, videoOptions);

            return profile;
        }

        // Annex A.15
        protected Profile SelectVideoAudioProfile(
            VideoEncoding videoEncoding,
            string videoCodec,
            TestVideoEncoderConfigurationOptions testVideo,
            AudioEncoding audioEncoding,
            string audioCodec,
            TestAudioEncoderConfigurationOptions testAudio,
            ref VideoEncoderConfigurationOptions videoOptions,
            ref AudioEncoderConfigurationOptions audioOptions)
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
                                                LogStepEvent("AddAudioSourceConfiguration");
                                                Client.AddAudioSourceConfiguration(p.token, audioSourceConfigurations[0].token);
                                                DoRequestDelay();
                                            }
                                            else
                                            {
                                                throw new DutPropertiesException("Audio Source Configurations not found");
                                            }
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

            profile.VideoEncoderConfiguration.Encoding = videoEncoding;

            // find nearest bitrate and samplerate
            profile.AudioEncoderConfiguration.Bitrate = 
                FindNearestAudioBitrate(profile.AudioEncoderConfiguration.Bitrate, 
                                        audioEncoding, audioOptions);
            profile.AudioEncoderConfiguration.SampleRate = 
                FindNearestAudioSamplerate(profile.AudioEncoderConfiguration.SampleRate,
                                           audioEncoding, audioOptions);

            videoOptions = videoOptionsTmp;
            audioOptions = audioOptionsTmp;

            AdjustVideoEncoderConfiguration(videoEncoding, profile.VideoEncoderConfiguration, videoOptions);

            return profile;
        }

        // ANNEX A.X1 Media Profile Configuration for Audio Streaming
        protected Profile SelectAudioProfile(
            AudioEncoding audioEncoding,
            string audioCodec,
            TestAudioEncoderConfigurationOptions testAudio,
            ref AudioEncoderConfigurationOptions audioOptions)
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
                                        LogStepEvent("AddAudioSourceConfiguration");
                                        Client.AddAudioSourceConfiguration(p.token, audioSourceConfigurations[0].token);
                                        DoRequestDelay();
                                    }
                                    else
                                    {
                                        throw new DutPropertiesException("Audio Source Configurations not found");
                                    }
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

            // find nearest bitrate and samplerate
            profile.AudioEncoderConfiguration.Bitrate =
                FindNearestAudioBitrate(profile.AudioEncoderConfiguration.Bitrate,
                                        audioEncoding, audioOptions);
            profile.AudioEncoderConfiguration.SampleRate =
                FindNearestAudioSamplerate(profile.AudioEncoderConfiguration.SampleRate,
                                           audioEncoding, audioOptions);

            audioOptions = audioOptionsTmp;

            return profile;
        }

        #endregion

        #region IVideoFormEvent

        public void FireBeginStep(string Name)
        {
            BeginStep(Name);
        }
        public void FireStepPassed()
        {
            StepPassed();
        }
        public void FireLogStepEvent(string Message)
        {
            LogStepEvent(Message);
        }

        #endregion
    }
}