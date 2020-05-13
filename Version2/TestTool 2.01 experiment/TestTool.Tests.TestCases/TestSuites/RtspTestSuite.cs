#define USE_FIXED_SYNCLENGTH
#define USE_FIXED_SYNCTIME

using System.Linq;
using System.Collections;
using System.IO;
using System;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Media;
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
            StreamType streamType)
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

            SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false);

            StreamSetup streamSetup = new StreamSetup();
            streamSetup.Transport = new Transport();
            streamSetup.Transport.Protocol = protocol;
            streamSetup.Stream = streamType;

            UsedProfileToken = profile.token;
            MediaUri streamUri = GetStreamUri(streamSetup, profile.token);
            AdjustVideo(protocol, streamType, streamUri, profile.VideoEncoderConfiguration);

            return streamUri;
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
        void ValidateStreamSequence(bool CheckAudio, bool CheckSync)
        {
            _videoForm.EventSink = this;
            _videoForm.OpenWindow(CheckAudio);
            VideoIsOpened = true;

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
                DateTime ThresholdTime = DateTime.Now;
                ThresholdTime = ThresholdTime.AddSeconds(FIXED_SYNCLENGTH_TIME);
                _videoForm.WaitForSync(ref Length);
                if (DateTime.Now > ThresholdTime)
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
            TransportProtocol protocol)
        {
            Profile[] profiles = GetProfiles();

            Profile profile = null;
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

                        if (videoTest(videoOptions))
                        {
                            // Video configuration OK  - configure Audio, if needed.
                            
                            if (p.AudioEncoderConfiguration != null && p.AudioSourceConfiguration != null)
                            {
                                LogStepEvent("GetAudioEncoderConfigurationOptions");
                                AudioEncoderConfigurationOptions audioOptions =
                                    Client.GetAudioEncoderConfigurationOptions(p.AudioEncoderConfiguration.token, 
                                    p.token);

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

                                bool audioEncoderConfigurationFound = false;

                                foreach (AudioEncoderConfiguration configuration in audioEncoderConfigurations)
                                {
                                    LogStepEvent("GetAudioEncoderConfigurationOptions");
                                    AudioEncoderConfigurationOptions audioOptions =
                                        Client.GetAudioEncoderConfigurationOptions(configuration.token, p.token);

                                    if (audioTest(audioOptions))
                                    {
                                        if (p.AudioSourceConfiguration == null)
                                        {
                                            AudioSourceConfiguration[] audioSourceConfigurations = Client.GetAudioSourceConfigurations();

                                            if (audioSourceConfigurations.Length > 0)
                                            {
                                                LogStepEvent("AddAudioSourceConfiguration");
                                                Client.AddAudioSourceConfiguration(p.token, audioSourceConfigurations[0].token);
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
            VideoEncoderConfigurationOptions options = Client.GetVideoEncoderConfigurationOptions(profile.VideoEncoderConfiguration.token, profile.token);

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


            SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false);

            profile.AudioEncoderConfiguration.Encoding = audioEncoding;
            profile.AudioEncoderConfiguration.Bitrate = bitrate;
            profile.AudioEncoderConfiguration.SampleRate = sampleRate;

            SetAudioEncoderConfiguration(profile.AudioEncoderConfiguration, false);

            StreamSetup streamSetup = new StreamSetup();
            streamSetup.Transport = new Transport();
            streamSetup.Transport.Protocol = protocol;
            streamSetup.Stream = streamType;

            UsedProfileToken = profile.token;
            MediaUri streamUri = GetStreamUri(streamSetup, profile.token);
            AdjustVideo(protocol, streamType, streamUri, profile.VideoEncoderConfiguration);

            return streamUri;
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
                (profile, options) => {  },
                "JPEG", 
                VideoEncoding.JPEG, 
                protocol, 
                streamType);
        }

        protected MediaUri GetMpeg4MediaUri(StreamType streamType, TransportProtocol protocol)
        {
            return GetVideoMediaUri(
                (options) => { return (options.MPEG4 != null); },
                (profile, options) =>
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
                    },
                "MPEG4", 
                VideoEncoding.MPEG4, 
                protocol, 
                streamType);
        }

        protected MediaUri GetH264MediaUri(StreamType streamType, TransportProtocol protocol)
        {
            return GetVideoMediaUri(
                (options) => { return (options.H264 != null); },
                (profile, options) =>
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

                    },
                "H.264", 
                VideoEncoding.H264, 
                protocol, 
                streamType);
        }

        protected MediaUri GetJpegG711MediaUri(StreamType streamType, TransportProtocol protocol)
        {
            return GetAudioVideoMediaUri((options) => { return (options.JPEG != null); }, 
                "JPEG", 
                VideoEncoding.JPEG,
                options => 
                         ( (options.Options != null) 
                             && (options.Options.Where(o => o.Encoding == AudioEncoding.G711)
                             .FirstOrDefault() != null))
                ,
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
                options => ((options.Options != null)
                        && (options.Options.Where(o => o.Encoding == AudioEncoding.G726)
                        .FirstOrDefault() != null))
                ,
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
                options =>
                 ((options.Options != null)
                        && (options.Options.Where(o => o.Encoding == AudioEncoding.AAC)
                        .FirstOrDefault() != null)),
                "AAC",
                AudioEncoding.AAC,
                streamType,
                protocol);
        }

    }

}
