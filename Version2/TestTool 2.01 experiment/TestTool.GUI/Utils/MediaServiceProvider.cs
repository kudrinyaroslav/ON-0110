///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using TestTool.Proxies.Media;
using System.ServiceModel.Channels;
using System.ServiceModel;
using TestTool.Tests.Common.TestEngine;
using TestTool.HttpTransport;
using DateTime = TestTool.Proxies.Device.DateTime;

namespace TestTool.GUI.Utils
{
    class MediaServiceProvider : BaseServiceProvider<MediaClient, Media>
    {
        public const string TestMediaProfileName = "TestMediaProfile";
        public delegate void VideoSourceConfigurationsReceived(VideoSourceConfiguration[] configs);
        public event VideoSourceConfigurationsReceived OnVideoSourceConfigurationsReceived;

        public delegate void AudioSourceConfigurationsReceived(AudioSourceConfiguration[] configs);
        public event AudioSourceConfigurationsReceived OnAudioSourceConfigurationsReceived;

        public delegate void VideoEncoderConfigurationReceived(VideoEncoderConfiguration[] configs);
        public event VideoEncoderConfigurationReceived OnVideoEncoderConfigurationReceived;

        public delegate void AudioEncoderConfigurationReceived(AudioEncoderConfiguration[] configs);
        public event AudioEncoderConfigurationReceived OnAudioEncoderConfigurationReceived;

        public delegate void VideoEncoderConfigOptionsReceived(VideoEncoderConfigurationOptions options);
        public event VideoEncoderConfigOptionsReceived OnVideoEncoderConfigOptionsReceived;

        public delegate void AudioEncoderConfigOptionsReceived(AudioEncoderConfigurationOptions options);
        public event AudioEncoderConfigOptionsReceived OnAudioEncoderConfigOptionsReceived;

        public delegate void MediaUriReceived(MediaUri uri, VideoEncoderConfiguration encoder, AudioEncoderConfiguration audio);
        public event MediaUriReceived OnMediaUriReceived;

        public delegate void ProfilesReceived(Profile[] profiles);
        public event ProfilesReceived OnProfilesReceived;

        public delegate void PTZConfigurationAdded(string profile, string config);
        public event PTZConfigurationAdded OnPTZConfigurationAdded;

        
        public MediaServiceProvider(string serviceAddress, int messageTimeout) : 
            base(serviceAddress, messageTimeout)
        {
            EnableLogResponse = true;
        }
        public override MediaClient CreateClient(Binding binding, EndpointAddress address)
        {
            return new MediaClient(binding, address);
        }
        public void GetVideoSourceConfigurations()
        {
            RunInBackground(new Action(() =>
            {
                VideoSourceConfiguration[] configs = Client.GetVideoSourceConfigurations();
                if (OnVideoSourceConfigurationsReceived != null)
                {
                    OnVideoSourceConfigurationsReceived(configs);
                }
            }));
        }
        public void GetAudioSourceConfigurations()
        {
            RunInBackground(new Action(() =>
            {
                AudioSourceConfiguration[] configs = Client.GetAudioSourceConfigurations();
                if (OnAudioSourceConfigurationsReceived != null)
                {
                    OnAudioSourceConfigurationsReceived(configs);
                }
            }));
        }
        public void GetVideoEncoderConfigurations()
        {
            RunInBackground(new Action(() =>
            {
                VideoEncoderConfiguration[] configs = Client.GetVideoEncoderConfigurations();
                if (OnVideoEncoderConfigurationReceived != null)
                {
                    OnVideoEncoderConfigurationReceived(configs);
                }
            }));
        }
        public void GetAudioEncoderConfigurations()
        {
            RunInBackground(new Action(() =>
            {
                AudioEncoderConfiguration[] configs = Client.GetAudioEncoderConfigurations();
                if (OnAudioEncoderConfigurationReceived != null)
                {
                    OnAudioEncoderConfigurationReceived(configs);
                }
            }));
        }
        public void GetVideoEncoderConfigOptions(string config)
        {
            RunInBackground(new Action(() =>
            {
                VideoEncoderConfigurationOptions options = Client.GetVideoEncoderConfigurationOptions(config, null);
                if (OnVideoEncoderConfigOptionsReceived != null)
                {
                    OnVideoEncoderConfigOptionsReceived(options);
                }
            }));
        }
        public void GetAudioEncoderConfigOptions(string config)
        {
            RunInBackground(new Action(() =>
            {
                AudioEncoderConfigurationOptions options = Client.GetAudioEncoderConfigurationOptions(config, null);
                if (OnAudioEncoderConfigOptionsReceived != null)
                {
                    OnAudioEncoderConfigOptionsReceived(options);
                }
            }));
        }
        protected Profile CreateProfile(string profileName)
        {
            return Client.CreateProfile(profileName, null);
        }
        protected Profile GetVideoProfile()
        {
            Profile videoProfile = null;
            Profile[] profiles = Client.GetProfiles();
            foreach (Profile profile in profiles)
            {
                if ((profile.VideoSourceConfiguration != null) && (profile.VideoEncoderConfiguration != null))
                {
                    videoProfile = profile;
                    break;
                }
            }
            return videoProfile;
        }
        protected void ConfigureProfile(
            Profile profile,
            VideoSourceConfiguration videoSourceConfig,
            VideoEncoderConfiguration videoEncoderConfig,
            AudioSourceConfiguration audioSourceConfig,
            AudioEncoderConfiguration audioEncoderConfig)
        {
            if ((profile.VideoSourceConfiguration != null) && (profile.VideoSourceConfiguration.token != videoSourceConfig.token))
            {
                Client.RemoveVideoSourceConfiguration(profile.token);
                profile.VideoSourceConfiguration = null;
            }
            if (profile.VideoSourceConfiguration == null)
            {
                Client.AddVideoSourceConfiguration(profile.token, videoSourceConfig.token);
            }

            if ((profile.VideoEncoderConfiguration != null) && (profile.VideoEncoderConfiguration.token != videoEncoderConfig.token))
            {
                Client.RemoveVideoEncoderConfiguration(profile.token);
                profile.VideoEncoderConfiguration = null;
            }
            if (profile.VideoEncoderConfiguration == null)
            {
                Client.AddVideoEncoderConfiguration(profile.token, videoEncoderConfig.token);
            }
            //encoder configuration can be modified - update it
            Client.SetVideoEncoderConfiguration(videoEncoderConfig, false);

            if ((profile.AudioSourceConfiguration != null) &&
                ((audioSourceConfig == null)||
                (profile.AudioSourceConfiguration.token != audioSourceConfig.token)))
            {
                Client.RemoveAudioSourceConfiguration(profile.token);
                profile.AudioSourceConfiguration = null;
            }
            if ((profile.AudioSourceConfiguration == null) && (audioSourceConfig != null))
            {
                Client.AddAudioSourceConfiguration(profile.token, audioSourceConfig.token);
            }

            if ((profile.AudioEncoderConfiguration != null) &&
                ((audioEncoderConfig == null)||
                (profile.AudioEncoderConfiguration.token != audioEncoderConfig.token)))
            {
                Client.RemoveAudioEncoderConfiguration(profile.token);
                profile.AudioEncoderConfiguration = null;
            }
            if(audioEncoderConfig != null)
            {
                if ((profile.AudioEncoderConfiguration == null) && (audioEncoderConfig != null))
                {
                    Client.AddAudioEncoderConfiguration(profile.token, audioEncoderConfig.token);
                }
                //encoder configuration can be modified - update it
                Client.SetAudioEncoderConfiguration(audioEncoderConfig, false);
            }
        }
        public void GetMediaUri(
            Profile profile,
            VideoSourceConfiguration videoSourceConfig,
            VideoEncoderConfiguration videoEncoderConfig,
            AudioSourceConfiguration audioSourceConfig,
            AudioEncoderConfiguration audioEncoderConfig,
            TransportProtocol protocol)
        {
            RunInBackground(new Action(() =>
            {
                if(profile == null)
                {
                    profile = CreateProfile(TestMediaProfileName);
                }
                ConfigureProfile(profile, videoSourceConfig, videoEncoderConfig, audioSourceConfig, audioEncoderConfig);

                StreamSetup streamSetup = new StreamSetup();
                streamSetup.Transport = new Transport();
                streamSetup.Transport.Protocol = protocol;
                streamSetup.Stream = StreamType.RTPUnicast;

                MediaUri streamUri = Client.GetStreamUri(streamSetup, profile.token);
                if(OnMediaUriReceived != null)
                {
                    OnMediaUriReceived(streamUri, videoEncoderConfig, audioEncoderConfig);
                }
            }));
        }

        public void GetMediaUri(
            Profile profile,
            TransportProtocol protocol)
        {
            RunInBackground(new Action(() =>
            {
                StreamSetup streamSetup = new StreamSetup();
                streamSetup.Transport = new Transport();
                streamSetup.Transport.Protocol = protocol;
                streamSetup.Stream = StreamType.RTPUnicast;

                MediaUri streamUri = Client.GetStreamUri(streamSetup, profile.token);
                if (OnMediaUriReceived != null)
                {
                    OnMediaUriReceived(streamUri, profile.VideoEncoderConfiguration, profile.AudioEncoderConfiguration);
                }
            }));
        }

        public void GetMediaUri()
        {
            RunInBackground(new Action(() =>
            {
                Profile profile = GetVideoProfile();
                if(profile == null)
                {
                    throw new Exception("Profile with video source encoder configuration not found");
                }

                StreamSetup streamSetup = new StreamSetup();
                streamSetup.Transport = new Transport();
                streamSetup.Transport.Protocol = TransportProtocol.UDP;
                streamSetup.Stream = StreamType.RTPUnicast;

                MediaUri streamUri = Client.GetStreamUri(streamSetup, profile.token);
                if (OnMediaUriReceived != null)
                {
                    OnMediaUriReceived(streamUri, profile.VideoEncoderConfiguration, null);
                }
            }));
        }
        public void GetProfiles()
        {
            RunInBackground(new Action(() =>
            {
                Profile[] profiles = Client.GetProfiles();
                if (OnProfilesReceived != null)
                {
                    OnProfilesReceived(profiles);
                }
            }));
        }
        public void AddPTZConfiguration(string profile, string configuration)
        {
            RunInBackground(new Action(() =>
            {
                Client.AddPTZConfiguration(profile, configuration);
                if(OnPTZConfigurationAdded != null)
                {
                    OnPTZConfigurationAdded(profile, configuration);
                }
            }));
        }
    }
}
