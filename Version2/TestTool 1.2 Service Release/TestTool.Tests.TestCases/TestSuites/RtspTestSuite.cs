#define USE_FIXED_SYNCLENGTH
#define USE_FIXED_SYNCTIME

using System.Linq;
using System.Collections;
using System.IO;
using System;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.Media;


namespace TestTool.Tests.TestCases.TestSuites
{
    class RtspTestSuite : MediaTest, IVideoFormEvent
    {
        const int FIXED_SYNCLENGTH_VALUE = 8;
        const int FIXED_SYNCLENGTH_TIME = 10;
        public RtspTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        private delegate bool TestVideoEncoderConfigurationOptions(VideoEncoderConfigurationOptions options);

        private delegate bool TestAudioEncoderConfigurationOptions(AudioEncoderConfigurationOptions options);

        private delegate void CopyVideoEncoderConfiguration(Profile profile, VideoEncoderConfigurationOptions options);

        private string UsedProfileToken = "";
     
        MediaUri GetVideoMediaUri(TestVideoEncoderConfigurationOptions test,
            CopyVideoEncoderConfiguration copyMethod,
            string profileRequirementsDescription,
            VideoEncoding encoding,
            TransportProtocol protocol,
            StreamType streamType,
            IPType? multicastAddressType)
        {

            Profile profile = null;
            VideoEncoderConfigurationOptions options = null;
            Profile[] profiles = GetProfiles();

            RunStep(() =>
            {
                foreach (Profile p in profiles)
                {
                    LogStepEvent(string.Format("Check if {0} profile supports {1} Video encoder configuration",
                        p.Name, profileRequirementsDescription));
                    if (p.VideoEncoderConfiguration != null)
                    {
                        options =
                            Client.GetVideoEncoderConfigurationOptions(p.VideoEncoderConfiguration.token, p.token);

                        DoRequestDelay();

                        if (test(options))
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

            profile.VideoEncoderConfiguration.Encoding = encoding;
            copyMethod(profile, options);
            // fix for Panasonic
            if (encoding == VideoEncoding.JPEG)
            {
                profile.VideoEncoderConfiguration.MPEG4 = null;
                profile.VideoEncoderConfiguration.H264 = null;

                // support for extensions (bitrate limits)
                if (options.Extension != null)
                {
                    if (options.Extension.JPEG != null)
                    {
                        if (options.Extension.JPEG.BitrateRange != null)
                        {
                            if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit < options.Extension.JPEG.BitrateRange.Min) 
                            {
                                profile.VideoEncoderConfiguration.RateControl.BitrateLimit = options.Extension.JPEG.BitrateRange.Min;
                            }
                            if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit > options.Extension.JPEG.BitrateRange.Max)
                            {
                                profile.VideoEncoderConfiguration.RateControl.BitrateLimit = options.Extension.JPEG.BitrateRange.Max;
                            }
                        }
                    }
                }
            }
            if (encoding == VideoEncoding.MPEG4)
            {
                profile.VideoEncoderConfiguration.H264 = null;

                // support for extensions (bitrate limits)
                if (options.Extension != null)
                {
                    if (options.Extension.MPEG4 != null)
                    {
                        if (options.Extension.MPEG4.BitrateRange != null)
                        {
                            if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit < options.Extension.MPEG4.BitrateRange.Min)
                            {
                                profile.VideoEncoderConfiguration.RateControl.BitrateLimit = options.Extension.MPEG4.BitrateRange.Min;
                            }
                            if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit > options.Extension.MPEG4.BitrateRange.Max)
                            {
                                profile.VideoEncoderConfiguration.RateControl.BitrateLimit = options.Extension.MPEG4.BitrateRange.Max;
                            }
                        }
                    }
                }
            }
            if (encoding == VideoEncoding.H264)
            {
                profile.VideoEncoderConfiguration.MPEG4 = null;

                // support for extensions (bitrate limits)
                if (options.Extension != null)
                {
                    if (options.Extension.H264 != null)
                    {
                        if (options.Extension.H264.BitrateRange != null)
                        {
                            if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit < options.Extension.H264.BitrateRange.Min)
                            {
                                profile.VideoEncoderConfiguration.RateControl.BitrateLimit = options.Extension.H264.BitrateRange.Min;
                            }
                            if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit > options.Extension.H264.BitrateRange.Max)
                            {
                                profile.VideoEncoderConfiguration.RateControl.BitrateLimit = options.Extension.H264.BitrateRange.Max;
                            }
                        }
                    }
                }
            }

            if (multicastAddressType.HasValue)
            {
                SetMulticastSettings(profile, true, false, multicastAddressType.Value);
            }

            SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, multicastAddressType.HasValue);

            StreamSetup streamSetup = new StreamSetup();
            streamSetup.Transport = new Transport();
            streamSetup.Transport.Protocol = protocol;
            streamSetup.Stream = streamType;

            UsedProfileToken = profile.token;
            MediaUri streamUri = GetStreamUri(streamSetup, profile.token);
            AdjustVideo(protocol, streamType, streamUri, profile.VideoEncoderConfiguration);

            return streamUri;
        }

        MediaUri GetVideoMediaUri(TestVideoEncoderConfigurationOptions test,
            CopyVideoEncoderConfiguration copyMethod,
            string profileRequirementsDescription,
            VideoEncoding encoding,
            TransportProtocol protocol,
            StreamType streamType)
        {
            return GetVideoMediaUri(test, copyMethod, profileRequirementsDescription, encoding, protocol, streamType, null);
        }
        
        //TODO remove protected attribute - temporary usage only, till GetURI will work fully OK
        protected void AdjustVideo(
            TransportProtocol protocol,
            StreamType streamType,
            MediaUri streamUri,
            VideoEncoderConfiguration conf)
        {
            VideoUtils.AdjustVideo(_videoForm, _username, _password, _messageTimeout, protocol, streamType, streamUri, conf);
        }
        public void FireBeginStep(string Name)
        {
            BeginStep(Name);
        }
        public void FireStepPassed()
        {
            StepPassed();
        }
        //void FireStepFailed()
        //{
        //    StepFailed();
        //}
        public void FireLogStepEvent(string Message)
        {
            LogStepEvent(Message);
        }

        bool VideoIsOpened = false;
        void ValidateStreamSequence(bool CheckAudio, bool CheckSync, bool CheckVideo)
        {
            _videoForm.EventSink = this;
            _videoForm.OpenWindow(CheckAudio, CheckVideo);
            VideoIsOpened = true;

            if (CheckVideo)
            Assert(_operator.GetYesNoAnswer("Do you observe video?"),
                "Operator does not observe video",
                "Video quality check (manual)");
            
            if (CheckAudio)
            Assert(_operator.GetYesNoAnswer("Do you hear audio?"),
                "Operator does not hear audio",
                "Audio quality check (manual)");

            if (CheckSync)
            {
                double LengthAv = 0;
                int Length = 0;
#if (!USE_FIXED_SYNCLENGTH)
                BeginStep("Waiting for keyframe frequency stabilization");
                if (!_videoForm.WaitForStableKey(ref LengthAv)) 
                {
                    throw new AssertException("Keyframe frequency too volatile: " + LengthAv.ToString());
                }
                LogStepEvent("Average keyframe frequency " + LengthAv.ToString());
                StepPassed();
#endif
                SetSynchronizationPoint(UsedProfileToken);

                BeginStep("Looking for out-of-order keyframe");
#if USE_FIXED_SYNCLENGTH
#if USE_FIXED_SYNCTIME
                System.DateTime ThresholdTime = System.DateTime.Now;
                ThresholdTime = ThresholdTime.AddSeconds(FIXED_SYNCLENGTH_TIME);
                _videoForm.WaitForSync(ref Length);
                if (System.DateTime.Now > ThresholdTime)
#else
                _videoForm.WaitForSync(ref Length);
                if (Length > FIXED_SYNCLENGTH_VALUE)
#endif
#else
                if (!_videoForm.WaitForSync(ref Length))
#endif
                {
                    throw new AssertException("No out-of-order keyframe");
                }

                LogStepEvent("Keyframe interval " + Length.ToString());
                StepPassed();
//                Assert(_videoForm.WaitForSync(ref Length),
//                    "No out-of-order keyframe",
//                    "Looking for out-of-order keyframe");
            }

            _videoForm.CloseWindow();
            VideoIsOpened = false;
            _videoForm.EventSink = null;
        }
        void ValidateStreamSequence(bool CheckAudio, bool CheckSync)
        {
            ValidateStreamSequence(CheckAudio, CheckSync, true);
        }
        protected void ValidateVideoSequence()
        {
            ValidateStreamSequence(false, false);
        }
        protected void ValidateAudioVideoSequence()
        {
            ValidateStreamSequence(true, false);
        }
        protected void ValidateSyncSequence()
        {
            ValidateStreamSequence(false, true);
        }
        protected void ValidateAudioSequence()
        {
            ValidateStreamSequence(true, false, false);
        }
        protected void VideoCleanup()
        {
            //try 
            {
                if (VideoIsOpened)
                {
                    _videoForm.CloseWindow();
                    VideoIsOpened = false;
                    _videoForm.EventSink = null;
                }
            }
            //catch (Exception) { }
        }


        MediaUri GetAudioVideoMediaUri(TestVideoEncoderConfigurationOptions videoTest, 
            string videoCodec,
            VideoEncoding encoding,
            TestAudioEncoderConfigurationOptions audioTest,
            string audioCodec,
            AudioEncoding audioEncoding,
            StreamType streamType, 
            TransportProtocol protocol,
            IPType? multicastAddressType)
        {
            Profile[] profiles = GetProfiles();

            Profile profile = null;
            VideoEncoderConfigurationOptions options = null;
            int bitrate = 0;
            int sampleRate = 0;

            RunStep(() =>
            {
                foreach (Profile p in profiles)
                {
                    LogStepEvent(string.Format("Check if {0} profile satisfies current needs", p.Name));

                    if (p.VideoEncoderConfiguration != null)
                    {
                        LogStepEvent("GetVideoEncoderConfigurationOptions");
                        VideoEncoderConfigurationOptions videoOptions =
                            Client.GetVideoEncoderConfigurationOptions(p.VideoEncoderConfiguration.token, p.token);
                        DoRequestDelay();

                        if (videoTest(videoOptions))
                        {
                            // Video configuration OK  - configure Audio, if needed.
                            options = videoOptions;
                            
                            if (p.AudioEncoderConfiguration != null && p.AudioSourceConfiguration != null)
                            {
                                LogStepEvent("GetAudioEncoderConfigurationOptions");
                                AudioEncoderConfigurationOptions audioOptions =
                                    Client.GetAudioEncoderConfigurationOptions(p.AudioEncoderConfiguration.token, 
                                    p.token);

                                DoRequestDelay();

                                if (audioTest(audioOptions))
                                {
                                    profile = p;
                                    LogStepEvent("OK - profile found");

                                    // find nearest bitrate and samplerate
                                    bitrate = FindNearestAudioBitrate(p.AudioEncoderConfiguration.Bitrate, audioEncoding,
                                                                      audioOptions);
                                    sampleRate = FindNearestAudioSamplerate(p.AudioEncoderConfiguration.SampleRate,
                                                                            audioEncoding, audioOptions);
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
                                    AudioEncoderConfigurationOptions audioOptions =
                                        Client.GetAudioEncoderConfigurationOptions(configuration.token, p.token);

                                    DoRequestDelay();

                                    if (audioTest(audioOptions))
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

                                        bitrate = FindNearestAudioBitrate(configuration.Bitrate, audioEncoding,
                                                                          audioOptions);
                                        sampleRate = FindNearestAudioSamplerate(configuration.SampleRate, audioEncoding,
                                                                                audioOptions);

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
                    throw new DutPropertiesException("Respective profile cannot be found or created");
            },
            string.Format("Select or create profile with {0} Video encoder configuration and {1} Audio encoder configuration", 
            videoCodec, 
            audioCodec));

            // profile found

            profile.VideoEncoderConfiguration.Encoding = encoding;

            // support for extensions (bitrate limits)

            // fix for Panasonic
            if (encoding == VideoEncoding.JPEG)
            {
                profile.VideoEncoderConfiguration.MPEG4 = null;
                profile.VideoEncoderConfiguration.H264 = null;

                // support for extensions (bitrate limits)
                if (options.Extension != null)
                {
                    if (options.Extension.JPEG != null)
                    {
                        if (options.Extension.JPEG.BitrateRange != null)
                        {
                            if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit < options.Extension.JPEG.BitrateRange.Min)
                            {
                                profile.VideoEncoderConfiguration.RateControl.BitrateLimit = options.Extension.JPEG.BitrateRange.Min;
                            }
                            if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit > options.Extension.JPEG.BitrateRange.Max)
                            {
                                profile.VideoEncoderConfiguration.RateControl.BitrateLimit = options.Extension.JPEG.BitrateRange.Max;
                            }
                        }
                    }
                }
            }
            if (encoding == VideoEncoding.MPEG4)
            {
                profile.VideoEncoderConfiguration.H264 = null;

                // support for extensions (bitrate limits)
                if (options.Extension != null)
                {
                    if (options.Extension.MPEG4 != null)
                    {
                        if (options.Extension.MPEG4.BitrateRange != null)
                        {
                            if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit < options.Extension.MPEG4.BitrateRange.Min)
                            {
                                profile.VideoEncoderConfiguration.RateControl.BitrateLimit = options.Extension.MPEG4.BitrateRange.Min;
                            }
                            if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit > options.Extension.MPEG4.BitrateRange.Max)
                            {
                                profile.VideoEncoderConfiguration.RateControl.BitrateLimit = options.Extension.MPEG4.BitrateRange.Max;
                            }
                        }
                    }
                }
            }
            if (encoding == VideoEncoding.H264)
            {
                profile.VideoEncoderConfiguration.MPEG4 = null;

                // support for extensions (bitrate limits)
                if (options.Extension != null)
                {
                    if (options.Extension.H264 != null)
                    {
                        if (options.Extension.H264.BitrateRange != null)
                        {
                            if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit < options.Extension.H264.BitrateRange.Min)
                            {
                                profile.VideoEncoderConfiguration.RateControl.BitrateLimit = options.Extension.H264.BitrateRange.Min;
                            }
                            if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit > options.Extension.H264.BitrateRange.Max)
                            {
                                profile.VideoEncoderConfiguration.RateControl.BitrateLimit = options.Extension.H264.BitrateRange.Max;
                            }
                        }
                    }
                }
            }

            if (multicastAddressType.HasValue)
            {
                SetMulticastSettings(profile, true, true, multicastAddressType.Value);
            }

            SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, multicastAddressType.HasValue);

            profile.AudioEncoderConfiguration.Encoding = audioEncoding;
            profile.AudioEncoderConfiguration.Bitrate = bitrate;
            profile.AudioEncoderConfiguration.SampleRate = sampleRate;

            SetAudioEncoderConfiguration(profile.AudioEncoderConfiguration, false, multicastAddressType.HasValue);

            StreamSetup streamSetup = new StreamSetup();
            streamSetup.Transport = new Transport();
            streamSetup.Transport.Protocol = protocol;
            streamSetup.Stream = streamType;

            UsedProfileToken = profile.token;
            MediaUri streamUri = GetStreamUri(streamSetup, profile.token);
            AdjustVideo(protocol, streamType, streamUri, profile.VideoEncoderConfiguration);

            return streamUri;
        }

        MediaUri GetAudioVideoMediaUri(TestVideoEncoderConfigurationOptions videoTest,
            string videoCodec,
            VideoEncoding encoding,
            TestAudioEncoderConfigurationOptions audioTest,
            string audioCodec,
            AudioEncoding audioEncoding,
            StreamType streamType,
            TransportProtocol protocol)
        {
            return GetAudioVideoMediaUri(
                videoTest, videoCodec, encoding, audioTest, audioCodec, audioEncoding, streamType, protocol, null);
        }

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
        
        protected MediaUri GetJpegMediaUri(StreamType streamType, TransportProtocol protocol)
        {
            return GetVideoMediaUri(
                (options) => { return (options.JPEG != null); },
                (profile, options) => { AdjustJPEGVideoEncoderConfiguration(profile, options); },
                "JPEG", 
                VideoEncoding.JPEG, 
                protocol, 
                streamType);
        }
        
        protected void AdjustJPEGVideoEncoderConfiguration(Profile profile, VideoEncoderConfigurationOptions options)
        {
            if (options.JPEG.ResolutionsAvailable.Length > 0)
            {
                profile.VideoEncoderConfiguration.Resolution = options.JPEG.ResolutionsAvailable[0];
            }
        }

        void AdjustMpeg4VideoEncoderConfiguration(Profile profile, VideoEncoderConfigurationOptions options)
        {
            if (profile.VideoEncoderConfiguration.MPEG4 == null)
            {
                profile.VideoEncoderConfiguration.MPEG4 = new Mpeg4Configuration();
            }

            profile.VideoEncoderConfiguration.MPEG4.Mpeg4Profile = Mpeg4Profile.SP;
            if (options.MPEG4.ResolutionsAvailable.Length > 0)
            {
                profile.VideoEncoderConfiguration.Resolution = options.MPEG4.ResolutionsAvailable[0];
            }

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

        protected MediaUri GetMpeg4MediaUri(StreamType streamType, TransportProtocol protocol)
        {
            return GetVideoMediaUri(
                (options) => { return (options.MPEG4 != null); },
                (profile, options) =>
                    {
                        AdjustMpeg4VideoEncoderConfiguration(profile, options);
                    },
                "MPEG4", 
                VideoEncoding.MPEG4, 
                protocol, 
                streamType);
        }

        void AdjustH264VideoEncoderConfiguration(Profile profile, VideoEncoderConfigurationOptions options)
        {
            if (profile.VideoEncoderConfiguration.H264 == null)
            {
                profile.VideoEncoderConfiguration.H264 = new H264Configuration();
            }

            profile.VideoEncoderConfiguration.H264.H264Profile = H264Profile.Baseline;
            if (options.H264.ResolutionsAvailable.Length > 0)
            {
                profile.VideoEncoderConfiguration.Resolution = options.H264.ResolutionsAvailable[0];
            }

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

        protected MediaUri GetH264MediaUri(StreamType streamType, TransportProtocol protocol)
        {
            return GetVideoMediaUri(
                (options) => { return (options.H264 != null); },
                (profile, options) =>
                    {
                        AdjustH264VideoEncoderConfiguration(profile, options);
                    },
                "H.264", 
                VideoEncoding.H264, 
                protocol, 
                streamType);
        }

        bool CheckAudioSupport(AudioEncoderConfigurationOptions options, AudioEncoding audioEncoding)
        {
            return (options.Options != null)
                   && (options.Options.Where(o => o.Encoding == audioEncoding).FirstOrDefault() != null);
        }

        protected MediaUri GetJpegG711MediaUri(StreamType streamType, TransportProtocol protocol)
        {
            return GetAudioVideoMediaUri((options) => { return (options.JPEG != null); }, 
                "JPEG", 
                VideoEncoding.JPEG,
                options => (CheckAudioSupport(options, AudioEncoding.G711)),
                "G.711",
                AudioEncoding.G711,
                streamType,
                protocol);
        }

        protected MediaUri GetJpegG726MediaUri(StreamType streamType, TransportProtocol protocol)
        {
            return GetAudioVideoMediaUri(options => (options.JPEG != null),
                "JPEG",
                VideoEncoding.JPEG,
                options => (CheckAudioSupport(options, AudioEncoding.G726)),
                "G.726",
                AudioEncoding.G726,
                streamType,
                protocol);
        }

        protected MediaUri GetJpegAacMediaUri(StreamType streamType, TransportProtocol protocol)
        {
            return GetAudioVideoMediaUri(options => (options.JPEG != null),
                "JPEG",
                VideoEncoding.JPEG,
                options => (CheckAudioSupport(options, AudioEncoding.AAC)),
                "AAC",
                AudioEncoding.AAC,
                streamType,
                protocol);
        }

        void SetMulticastSettings(Profile profile, bool toVideo, bool toAudio, IPType addressType)
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

        protected MediaUri GetJpegMediaUri(StreamType streamType, TransportProtocol protocol, IPType addressType)
        {
            return GetVideoMediaUri(
                (options) => { return (options.JPEG != null); },
                (profile, options) => { },
                "JPEG",
                VideoEncoding.JPEG,
                protocol,
                streamType,
                addressType);
        }

        protected MediaUri GetMpeg4MediaUri(StreamType streamType, TransportProtocol protocol, IPType addressType)
        {
            return GetVideoMediaUri(
                (options) => { return (options.MPEG4 != null); },
                (profile, options) =>
                {
                    AdjustMpeg4VideoEncoderConfiguration(profile, options);
                },
                "MPEG4",
                VideoEncoding.MPEG4,
                protocol,
                streamType,
                addressType);
        }

        protected MediaUri GetH264MediaUri(StreamType streamType, TransportProtocol protocol, IPType addressType)
        {
            return GetVideoMediaUri(
                (options) => { return (options.H264 != null); },
                (profile, options) =>
                {
                    AdjustH264VideoEncoderConfiguration(profile, options);
                },
                "H.264",
                VideoEncoding.H264,
                protocol,
                streamType,
                addressType);
        }

        protected MediaUri GetJpegG711MediaUri(StreamType streamType, TransportProtocol protocol, IPType addressType)
        {
            return GetAudioVideoMediaUri(
                (options) => { return (options.JPEG != null); },
                "JPEG",
                VideoEncoding.JPEG,
                options => (CheckAudioSupport(options, AudioEncoding.G711)),
                "G.711",
                AudioEncoding.G711,
                streamType,
                protocol,
                addressType);
        }

        protected MediaUri GetJpegAACMediaUri(StreamType streamType, TransportProtocol protocol, IPType addressType)
        {
            return GetAudioVideoMediaUri(
                (options) => { return (options.JPEG != null); },
                "JPEG",
                VideoEncoding.JPEG,
                options => (CheckAudioSupport(options, AudioEncoding.AAC)),
                "AAC",
                AudioEncoding.AAC,
                streamType,
                protocol,
                addressType);
        }

        protected MediaUri GetJpegG726MediaUri(StreamType streamType, TransportProtocol protocol, IPType addressType)
        {
            return GetAudioVideoMediaUri(
                (options) => { return (options.JPEG != null); },
                "JPEG",
                VideoEncoding.JPEG,
                options => (CheckAudioSupport(options, AudioEncoding.G726)),
                "G726",
                AudioEncoding.G726,
                streamType,
                protocol,
                addressType);
        }
        
        MediaUri GetAudioMediaUri(
            TestAudioEncoderConfigurationOptions audioTest,
            string audioCodec,
            AudioEncoding audioEncoding,
            StreamType streamType,
            TransportProtocol protocol,
            IPType? multicastAddressType)
        {
            Profile[] profiles = GetProfiles();

            Profile profile = null;
            AudioEncoderConfigurationOptions audioOptions = null;
            int bitrate = 0;
            int sampleRate = 0;

            RunStep(() =>
            {
                foreach (Profile p in profiles)
                {
                    if (p.AudioEncoderConfiguration != null && p.AudioSourceConfiguration != null)
                    {
                        LogStepEvent("GetAudioEncoderConfigurationOptions");
                        audioOptions =
                            Client.GetAudioEncoderConfigurationOptions(p.AudioEncoderConfiguration.token, p.token);
                        DoRequestDelay();

                        if (audioTest(audioOptions))
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
                            audioOptions =
                                Client.GetAudioEncoderConfigurationOptions(configuration.token, p.token);
                            DoRequestDelay();

                            if (audioTest(audioOptions))
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
            bitrate = FindNearestAudioBitrate(profile.AudioEncoderConfiguration.Bitrate, audioEncoding,
                                              audioOptions);
            sampleRate = FindNearestAudioSamplerate(profile.AudioEncoderConfiguration.SampleRate,
                                                    audioEncoding, audioOptions);

            if (multicastAddressType.HasValue)
            {
                SetMulticastSettings(profile, false, true, multicastAddressType.Value);
            }

            profile.AudioEncoderConfiguration.Encoding = audioEncoding;
            profile.AudioEncoderConfiguration.Bitrate = bitrate;
            profile.AudioEncoderConfiguration.SampleRate = sampleRate;

            SetAudioEncoderConfiguration(profile.AudioEncoderConfiguration, false, multicastAddressType.HasValue);

            StreamSetup streamSetup = new StreamSetup();
            streamSetup.Transport = new Transport();
            streamSetup.Transport.Protocol = protocol;
            streamSetup.Stream = streamType;

            UsedProfileToken = profile.token;
            MediaUri streamUri = GetStreamUri(streamSetup, profile.token);
            AdjustVideo(protocol, streamType, streamUri, profile.VideoEncoderConfiguration);

            return streamUri;
        }

        protected MediaUri GetG711MediaUri(StreamType streamType, TransportProtocol protocol)
        {
            return GetAudioMediaUri(
                options => (CheckAudioSupport(options, AudioEncoding.G711)),
                "G.711",
                AudioEncoding.G711,
                streamType,
                protocol,
                null);
        }

        protected MediaUri GetAACMediaUri(StreamType streamType, TransportProtocol protocol)
        {
            return GetAudioMediaUri(
                options => (CheckAudioSupport(options, AudioEncoding.AAC)),
                "AAC",
                AudioEncoding.AAC,
                streamType,
                protocol,
                null);
        }

        protected MediaUri GetG726MediaUri(StreamType streamType, TransportProtocol protocol)
        {
            return GetAudioMediaUri(
                options => (CheckAudioSupport(options, AudioEncoding.G726)),
                "G.726",
                AudioEncoding.G726,
                streamType,
                protocol,
                null);
        }
    }

}
