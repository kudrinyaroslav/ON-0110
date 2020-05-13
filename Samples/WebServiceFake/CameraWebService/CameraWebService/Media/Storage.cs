using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Media;

namespace CameraWebService.MediaService
{
    public class MediaStorage
    {
        private static MediaStorage _instance;
        public static MediaStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MediaStorage();
                }
                return _instance;
            }
        }
        
        private List<Media.VideoSource> _videoSources;
        public List<Media.VideoSource>  VideoSources
        {
            get
            {
                if (_videoSources == null)
                {
                    List<Media.VideoSource> lst = new List<Media.VideoSource>();

                    for (int i = 1; i < 5; i++)
                    {

                        Media.VideoSource source = new Media.VideoSource();
                        
                        source.token = "source" + i;
                        source.Resolution = new Media.VideoResolution();
                        source.Resolution.Height = 480;
                        source.Resolution.Width = 640;

                        source.Imaging = new Media.ImagingSettings();
                        source.Imaging.BacklightCompensation = new Media.BacklightCompensation();
                        source.Imaging.BacklightCompensation.Mode = Media.BacklightCompensationMode.OFF;

                        lst.Add(source);

                    }
                    _videoSources = lst;
                }

                return _videoSources;
            }
        }
        
        private List<Media.AudioSource> _audioSources;
        public List<Media.AudioSource> AudioSources
        {
            get
            {
                if (_audioSources == null)
                {
                    List<Media.AudioSource> lst = new List<Media.AudioSource>();

                    for (int i = 1; i < 5; i++)
                    {

                        Media.AudioSource source = new Media.AudioSource();

                        source.token = "source" + i;
                        lst.Add(source);

                    }
                    _audioSources = lst;
                }

                return _audioSources;
            }
        }

        private List<Media.VideoSourceConfiguration> _videoSourceConfigurations;
        public List<Media.VideoSourceConfiguration> VideoSourceConfigurations
        {
            get
            {
                if (_videoSourceConfigurations == null)
                {
                    _videoSourceConfigurations = new List<Media.VideoSourceConfiguration>();

                    Media.VideoSourceConfiguration vsc1 = new Media.VideoSourceConfiguration();
                    vsc1.SourceToken = VideoSources[0].token;
                    vsc1.token = "vsc1";
                    vsc1.Name = "VSC1";
                    vsc1.UseCount = 0;
                    vsc1.Bounds = new Media.IntRectangle() { height = 320, width = 480, x = 240, y = 0 };
                    _videoSourceConfigurations.Add(vsc1);


                    Media.VideoSourceConfiguration vsc2 = new Media.VideoSourceConfiguration();
                    vsc2.SourceToken = VideoSources[1].token;
                    vsc2.token = "vsc2";
                    vsc2.Name = "VSC2";
                    vsc2.UseCount = 0;
                    vsc2.Bounds = new Media.IntRectangle() { height = 320, width = 480, x = 240, y = 0 };
                    _videoSourceConfigurations.Add(vsc2);
                }

                return _videoSourceConfigurations;
            }
        }

        private List<Media.VideoEncoderConfiguration> _videoEncoderConfigurations;
        public List<Media.VideoEncoderConfiguration> VideoEncoderConfigurations
        {
            get
            {
                if (_videoEncoderConfigurations == null)
                {
                    _videoEncoderConfigurations = new List<Media.VideoEncoderConfiguration>();

                    Media.VideoEncoderConfiguration vec1 = new Media.VideoEncoderConfiguration();
                    vec1.token = "jpeg";
                    vec1.Name = "JPEG";
                    vec1.Encoding = VideoEncoding.JPEG;
                    vec1.Resolution = new Media.VideoResolution() { Height = 480, Width = 640 };
                    vec1.Multicast = new Media.MulticastConfiguration();
                    vec1.Multicast.Address = new Media.IPAddress(){IPv6Address = "192.168.3.211", Type = Media.IPType.IPv6};
                    vec1.RateControl = new Media.VideoRateControl();
                    vec1.RateControl.FrameRateLimit = 10;
                    vec1.RateControl.BitrateLimit = 5;
                    vec1.SessionTimeout = "PT180S";
                    vec1.Quality = 200;
                    vec1.UseCount = 0;
                    vec1.RateControl.EncodingInterval = 100;
                    _videoEncoderConfigurations.Add(vec1);


                    Media.VideoEncoderConfiguration vec2 = new Media.VideoEncoderConfiguration();
                    vec2.token = "H264";
                    vec2.Name = "H264";
                    vec2.Encoding = VideoEncoding.H264;
                    vec2.Resolution = new Media.VideoResolution() { Height = 480, Width = 640 };
                    vec2.Multicast = new Media.MulticastConfiguration();
                    vec2.Multicast.Address = new Media.IPAddress() { IPv6Address = "192.168.3.211", Type = Media.IPType.IPv6 };
                    vec2.RateControl = new Media.VideoRateControl();
                    vec2.RateControl.FrameRateLimit = 10;
                    vec2.RateControl.BitrateLimit = 5;
                    vec2.SessionTimeout = "PT180S";
                    vec2.Quality = 200;
                    vec2.UseCount = 0;
                    vec2.RateControl.EncodingInterval = 100;
                    _videoEncoderConfigurations.Add(vec2);
                    
                }

                return _videoEncoderConfigurations;
            }
        }

        private Media.VideoEncoderConfigurationOptions _videoEncoderConfigurationOptions;
        public Media.VideoEncoderConfigurationOptions VideoEncoderConfigurationOptions
        {
            get
            {
                if (_videoEncoderConfigurationOptions == null)
                {
                    _videoEncoderConfigurationOptions = new VideoEncoderConfigurationOptions();

                    _videoEncoderConfigurationOptions.JPEG = new JpegOptions();
                    _videoEncoderConfigurationOptions.JPEG.ResolutionsAvailable = 
                        new Media.VideoResolution[]
                            {
                                new Media.VideoResolution() { Height = 480, Width = 640 },
                                new Media.VideoResolution() { Height = 240, Width = 320 },
                                new Media.VideoResolution() { Height = 120, Width = 160 },
                                new Media.VideoResolution() { Height = 720, Width = 1280 }
                            };

                    _videoEncoderConfigurationOptions.QualityRange = new Media.IntRange() { Min = 100, Max = 300 };
                    _videoEncoderConfigurationOptions.JPEG.EncodingIntervalRange = new Media.IntRange() { Min = 100, Max = 200 };
                    _videoEncoderConfigurationOptions.JPEG.FrameRateRange = new Media.IntRange() { Min = 10, Max = 20 };


                    _videoEncoderConfigurationOptions.MPEG4 = new Media.Mpeg4Options();
                    _videoEncoderConfigurationOptions.MPEG4.GovLengthRange = new Media.IntRange();
                    _videoEncoderConfigurationOptions.MPEG4.GovLengthRange.Min = 25;
                    _videoEncoderConfigurationOptions.MPEG4.GovLengthRange.Max = 40;
                    _videoEncoderConfigurationOptions.MPEG4.EncodingIntervalRange = new Media.IntRange() { Min = 100, Max = 200 };
                    _videoEncoderConfigurationOptions.MPEG4.FrameRateRange = new Media.IntRange() { Min = 10, Max = 20 };
                    _videoEncoderConfigurationOptions.MPEG4.ResolutionsAvailable =
                        new Media.VideoResolution[]
                                    {
                                        new Media.VideoResolution() {Height = 120, Width = 160},
                                        new Media.VideoResolution() {Height = 480, Width = 640}
                                    };
                    _videoEncoderConfigurationOptions.MPEG4.Mpeg4ProfilesSupported = new Media.Mpeg4Profile[] { Media.Mpeg4Profile.ASP, Media.Mpeg4Profile.SP };
                    _videoEncoderConfigurationOptions.H264 = new Media.H264Options();
                    _videoEncoderConfigurationOptions.H264.GovLengthRange = new Media.IntRange();
                    _videoEncoderConfigurationOptions.H264.GovLengthRange.Min = 25;
                    _videoEncoderConfigurationOptions.H264.GovLengthRange.Max = 40;
                    _videoEncoderConfigurationOptions.H264.EncodingIntervalRange = new Media.IntRange() { Min = 100, Max = 200 };
                    _videoEncoderConfigurationOptions.H264.FrameRateRange = new Media.IntRange() { Min = 10, Max = 20 };
                    _videoEncoderConfigurationOptions.H264.ResolutionsAvailable =
                        new Media.VideoResolution[]
                                    {
                                        new Media.VideoResolution() {Height = 120, Width = 160},
                                        new Media.VideoResolution() {Height = 480, Width = 640}
                                    };
                    _videoEncoderConfigurationOptions.H264.H264ProfilesSupported = new Media.H264Profile[] { Media.H264Profile.Baseline, Media.H264Profile.High };



                }

                return _videoEncoderConfigurationOptions;
            }
        }
        
        private List<Media.AudioSourceConfiguration> _audioSourceConfigurations;
        public List<Media.AudioSourceConfiguration> AudioSourceConfigurations
        {
            get
            {
                if (_audioSourceConfigurations == null)
                {
                    _audioSourceConfigurations = new List<Media.AudioSourceConfiguration>();

                    Media.AudioSourceConfiguration vsc1 = new Media.AudioSourceConfiguration();
                    vsc1.SourceToken = AudioSources[0].token;
                    vsc1.token = "asc1";
                    vsc1.Name = "ASC1";
                    _audioSourceConfigurations.Add(vsc1);


                    Media.AudioSourceConfiguration vsc2 = new Media.AudioSourceConfiguration();
                    vsc2.SourceToken = AudioSources[1].token;
                    vsc2.token = "asc2";
                    vsc2.Name = "ASC2";
                    _audioSourceConfigurations.Add(vsc2);

                }

                return _audioSourceConfigurations;
            }
        }

        private List<Media.AudioEncoderConfiguration> _audioEncoderConfigurations;
        public List<Media.AudioEncoderConfiguration> AudioEncoderConfigurations
        {
            get
            {
                if (_audioEncoderConfigurations == null)
                {
                    _audioEncoderConfigurations = new List<Media.AudioEncoderConfiguration>();

                    Media.AudioEncoderConfiguration vec1 = new Media.AudioEncoderConfiguration();
                    vec1.token = "G726";
                    vec1.Name = "G726";
                    vec1.Encoding = Media.AudioEncoding.G726;
                    vec1.Bitrate = 10;
                    vec1.SampleRate = 20;
                    vec1.Multicast = new Media.MulticastConfiguration();
                    vec1.Multicast.Address = new Media.IPAddress() { IPv4Address = "192.168.3.211", Type = Media.IPType.IPv4 };
                    vec1.SessionTimeout = "PT180S";
                    _audioEncoderConfigurations.Add(vec1);

                    Media.AudioEncoderConfiguration vec2 = new Media.AudioEncoderConfiguration();
                    vec2.token = "aec2";
                    vec2.Name = "AEC2";
                    vec2.Encoding = Media.AudioEncoding.AAC;
                    vec2.Bitrate = 10;
                    vec2.SampleRate = 20;
                    vec2.Multicast = new Media.MulticastConfiguration();
                    vec2.Multicast.Address = new Media.IPAddress() { IPv4Address = "192.168.3.211", Type = Media.IPType.IPv4 };
                    vec2.SessionTimeout = "PT180S";
                    _audioEncoderConfigurations.Add(vec2);

                }

                return _audioEncoderConfigurations;
            }
        }


        private List<Media.AudioOutputConfiguration> _audioOutputConfigurations;
        public List<Media.AudioOutputConfiguration> AudioOutputConfigurations
        {
            get
            {
                if (_audioOutputConfigurations == null)
                {
                    _audioOutputConfigurations = new List<Media.AudioOutputConfiguration>();

                    Media.AudioOutputConfiguration configuration = new Media.AudioOutputConfiguration();
                    configuration.OutputLevel = 10;
                    configuration.OutputToken = "token";
                    configuration.SendPrimacy = "a";
                    configuration.token = "token";
                    configuration.Name = "Configuration";

                    _audioOutputConfigurations.Add(configuration);
                }

                return _audioOutputConfigurations;
            }
        }



        #region Add...

        public void AddVideoEncoderConfiguration(Media.Profile profile, Media.VideoEncoderConfiguration configuration)
        {
            configuration.UseCount = configuration.UseCount + 1;
            if (profile.VideoEncoderConfiguration != null)
            {
                profile.VideoEncoderConfiguration.UseCount = profile.VideoEncoderConfiguration.UseCount - 1;
            }
            profile.VideoEncoderConfiguration = configuration;
        }

        public void AddVideoSourceConfiguration(Media.Profile profile, Media.VideoSourceConfiguration configuration)
        {
            configuration.UseCount = configuration.UseCount + 1;
            if (profile.VideoSourceConfiguration != null)
            {
                profile.VideoSourceConfiguration.UseCount = profile.VideoSourceConfiguration.UseCount - 1;
            }
            profile.VideoSourceConfiguration = configuration;
        }

        public void AddAudioEncoderConfiguration(Media.Profile profile, Media.AudioEncoderConfiguration configuration)
        {
            configuration.UseCount = configuration.UseCount + 1;
            if (profile.AudioEncoderConfiguration != null)
            {
                profile.AudioEncoderConfiguration.UseCount = profile.AudioEncoderConfiguration.UseCount - 1;
            }
            profile.AudioEncoderConfiguration = configuration;
        }

        public void AddAudioSourceConfiguration(Media.Profile profile, Media.AudioSourceConfiguration configuration)
        {
            configuration.UseCount = configuration.UseCount + 1;
            if (profile.AudioSourceConfiguration != null)
            {
                profile.AudioSourceConfiguration.UseCount = profile.AudioSourceConfiguration.UseCount - 1;
            }
            profile.AudioSourceConfiguration = configuration;
        }


        public void RemoveVideoEncoderConfiguration(Media.Profile profile)
        {
            if (profile.VideoEncoderConfiguration != null)
            {
                profile.VideoEncoderConfiguration.UseCount = profile.VideoEncoderConfiguration.UseCount - 1;
                profile.VideoEncoderConfiguration = null;
            }
        }

        public void RemoveVideoSourceConfiguration(Media.Profile profile)
        {
            if (profile.VideoSourceConfiguration != null)
            {
                profile.VideoSourceConfiguration.UseCount = profile.VideoSourceConfiguration.UseCount - 1;
                profile.VideoSourceConfiguration = null;
            }
        }

        public void RemoveAudioEncoderConfiguration(Media.Profile profile)
        {
            if (profile.AudioEncoderConfiguration != null)
            {
                profile.AudioEncoderConfiguration.UseCount = profile.AudioEncoderConfiguration.UseCount - 1;
                profile.AudioEncoderConfiguration = null;
            }
        }

        public void RemoveAudioSourceConfiguration(Media.Profile profile)
        {
            if (profile.AudioSourceConfiguration != null)
            {
                profile.AudioSourceConfiguration.UseCount = profile.AudioSourceConfiguration.UseCount - 1;
                profile.AudioSourceConfiguration = null;
            }
        }

        #endregion



        private List<Media.Profile> _profiles;
        public List<Media.Profile> Profiles
        {
            get
            {
                if (_profiles == null)
                {
                    _profiles = new List<Media.Profile>();

                    Media.Profile p1 = new Media.Profile();
                    p1.Name = "Profile1";
                    p1.token = "profile1";
                    p1.fixedSpecified = true;
                    p1.@fixed = false;

                    AddVideoSourceConfiguration(p1, VideoSourceConfigurations[0]);
                    AddVideoEncoderConfiguration(p1, VideoEncoderConfigurations[1]);
                    AddAudioSourceConfiguration(p1, AudioSourceConfigurations[0]);
                    AddAudioEncoderConfiguration(p1, AudioEncoderConfigurations[0]);

                    _profiles.Add(p1);
                    

                    Media.Profile p2 = new Media.Profile();
                    p2.Name = "Profile2";
                    p2.token = "profile2";
                    p2.fixedSpecified = true;
                    p2.@fixed = false;

                    AddVideoSourceConfiguration(p2, VideoSourceConfigurations[0]);
                    AddVideoEncoderConfiguration(p2, VideoEncoderConfigurations[0]);
                    _profiles.Add(p2);

                    Media.Profile p3 = new Media.Profile();
                    p3.Name = "Profile3";
                    p3.token = "profile3";
                    p3.fixedSpecified = true;
                    p3.@fixed = false;

                    //AddVideoSourceConfiguration(p3, VideoSourceConfigurations[0]);
                    //AddVideoEncoderConfiguration(p3, VideoEncoderConfigurations[1]);

                    _profiles.Add(p3);
                    
                    Media.Profile p4 = new Media.Profile();
                    p4.Name = "Audio";
                    p4.token = "Audio";
                    p4.fixedSpecified = true;
                    p4.@fixed = false;

                    AddAudioSourceConfiguration(p4, AudioSourceConfigurations[0]);
                    AddAudioEncoderConfiguration(p4, AudioEncoderConfigurations[0]);

                    _profiles.Add(p4);

                    //foreach (Profile p in _profiles)
                    //{
                    //    p.@fixed = true;
                    //}  

                }
                return _profiles;
            }

        }

        public int MaxNumberOfProfiles = 10;

        public void DeleteProfile(Media.Profile profile)
        {
            if (profile.VideoEncoderConfiguration != null)
            {
                profile.VideoEncoderConfiguration.UseCount = profile.VideoEncoderConfiguration.UseCount - 1;
            }
            if (profile.VideoSourceConfiguration != null)
            {
                profile.VideoSourceConfiguration.UseCount = profile.VideoSourceConfiguration.UseCount - 1;
            }
            if (profile.AudioEncoderConfiguration != null)
            {
                profile.AudioEncoderConfiguration.UseCount = profile.AudioEncoderConfiguration.UseCount - 1;
            }
            if (profile.AudioSourceConfiguration != null)
            {
                profile.AudioSourceConfiguration.UseCount = profile.AudioSourceConfiguration.UseCount - 1;
            }
            
            _profiles.Remove(profile);

        }
    }
}
