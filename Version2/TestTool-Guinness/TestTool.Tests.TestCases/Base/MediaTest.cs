///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Xml;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Exceptions;
using System.Text;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.Base
{
    public class MediaTest : BaseServiceTest<Media, MediaClient>
    {
        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="param">Test params</param>
        public MediaTest(TestLaunchParam param)
            : base(param)
        {
        }

        protected override void Release()
        {
            if (_ptzClient != null)
            {
                _ptzClient.Close();
            }
            base.Release();
        }

        /// <summary>
        /// Creates instance of media client
        /// </summary>
        /// <returns>Instance of media client</returns>
        protected override MediaClient CreateClient()
        {
            string address = GetMediaServiceAddress();
            
            BeginStep("Connect to Media service");
            LogStepEvent(string.Format("Media service address: {0}", address));
            if (!address.IsValidUrl())
            {
                throw new AssertException("Media service address is invalid");
            }
            Binding binding = CreateBinding(false, 
                    new IChannelController[]{new SoapValidator(MediaSchemasSet.GetInstance())});
            MediaClient client = new MediaClient(binding, new EndpointAddress(address));
            StepPassed();
            return client;
        }

        private PTZClient _ptzClient;

        private PTZClient PtzClient
        {
            get
            {
                if (_ptzClient == null)
                {
                    string address = string.Empty;

                    RunStep(() =>
                    {
                        Binding binding =
                            CreateBinding(true,
                            new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                        TestTool.Proxies.Onvif.DeviceClient device = new TestTool.Proxies.Onvif.DeviceClient(binding, new EndpointAddress(_cameraAddress));

                        AttachSecurity(device.Endpoint);
                        SetupChannel(device.InnerChannel);

                        address = device.GetPtzServiceAddress(Features);
                            
                        device.Close();

                        if (string.IsNullOrEmpty(address))
                        {
                            throw new AssertException(Resources.ErrorNoPTZAddress_Text);
                        }
                    }, Resources.StepGetPTZAddress_Title);
                    DoRequestDelay();

                    BeginStep("Connect to PTZ service");
                    LogStepEvent(string.Format("PTZ service address: {0}", address));
                    if (!address.IsValidUrl())
                    {
                        throw new AssertException("PTZ service address is invalid");
                    }
                    Binding ptzBinding = CreateBinding(false,
                            new IChannelController[] { new SoapValidator(PtzSchemasSet.GetInstance()) });
                    _ptzClient = new TestTool.Proxies.Onvif.PTZClient(ptzBinding, new EndpointAddress(address));
                    AttachSecurity(_ptzClient.Endpoint);
                    SetupChannel(_ptzClient.InnerChannel);

                    StepPassed();

                }
                return _ptzClient;
            }

        }

        /// <summary>
        /// Returns DUT's media service address
        /// </summary>
        /// <returns>Media service url</returns>
        protected string GetMediaServiceAddress()
        {
            string address = string.Empty;
            RunStep(() =>
            {
                Binding binding =
                    CreateBinding(false,
                    new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                DeviceClient device = new DeviceClient(binding, new EndpointAddress(_cameraAddress));

                AttachSecurity(device.Endpoint);
                SetupChannel(device.InnerChannel);

                address = device.GetMediaServiceAddress(Features);

                device.Close();

                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException(Resources.ErrorNoMediaAddress_Text);
                }

            }, Resources.StepGetMediaAddress_Title);
            DoRequestDelay();
            return address;
        }

        /// <summary>
        /// Gets list of media profiles form DUT
        /// </summary>
        /// <returns>Array of profiles</returns>
        protected Profile[] GetProfiles()
        {
            Profile[] profiles = null;
            RunStep(() => { profiles = Client.GetProfiles(); }, Resources.StepGetMediaProfiles_Title);
            DoRequestDelay();
            return profiles;
        }

        /// <summary>
        /// Validates list of media profiles
        /// </summary>
        /// <param name="profiles">Profiles to be validated</param>
        /// <param name="reason">Reason why list of profiles is invalid, null if list is valid</param>
        /// <param name="validProfile">instance of profile containing both video source and encoder configurations</param>
        /// <returns>true, if list of profiles is valid</returns>
        protected bool ValidateProfiles(Profile[] profiles, out string reason, out Profile validProfile)
        {
            bool res = (profiles != null) && (profiles.Length > 0);
            reason = null;
            validProfile = null;
            if(res)
            {
                
                //DUT must provide at least one media profile with video source and video encoder
                foreach (Profile profile in profiles)
                {
                    if(!profile.fixedSpecified)
                    {
                        reason = string.Format("Profile [token={0}] has no \"fixed\" attribute", profile.token);
                        res = false;
                        break;
                    }
                    if ((profile.VideoEncoderConfiguration != null) && (profile.VideoSourceConfiguration != null) && (validProfile == null))
                    {
                        validProfile = profile;
                    }
                }
                if(res)
                {
                    res = validProfile != null;
                    if (!res)
                    {
                        reason = Resources.ErrorNoValidMediaProfile_Text;
                    }
                }
            }
            else
            {
                reason = Resources.ErrorNoMediaProfiles_Text;
            }
            return res;
        }

        /// <summary>
        /// Creates new media profile
        /// </summary>
        /// <param name="name">Name of new profile</param>
        /// <param name="token">Token of new profile</param>
        /// <returns>Created profile</returns>
        protected Profile CreateProfile(string name, string token)
        {
            Profile profile = null;
            RunStep(() => { 
                profile = Client.CreateProfile(name, token); },
                string.Format(Resources.StepCreateMediaProfile_Format, name));
            DoRequestDelay();
            return profile;
        }

        /// <summary>
        /// Checks if profiles contains any configuration instances
        /// </summary>
        /// <param name="profile">Profile to be checked</param>
        /// <param name="reason">Reason why profile is invalid</param>
        /// <returns>true, if profile is valid and empty</returns>
        protected bool IsEmptyProfile(Profile profile, out string reason)
        {
            reason = null;
            bool res = profile != null;
            if(res)
            {
                res = (profile.AudioEncoderConfiguration == null) && (profile.AudioSourceConfiguration == null) &&
                    (profile.MetadataConfiguration == null) && (profile.PTZConfiguration == null) &&
                    (profile.VideoAnalyticsConfiguration == null) && (profile.VideoEncoderConfiguration == null) &&
                    (profile.VideoSourceConfiguration == null);
                if(!res)
                {
                    reason = Resources.ErrorNewProfileNotEmpty_Text;
                }
            }
            else
            {
                reason = Resources.ErrorNoMediaProfiles_Text;
            }
            return res;
        }

        /// <summary>
        /// Adds video source configuration to profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <param name="configuration">Token of configuration</param>
        protected void AddVideoSourceConfiguration(string profile, string configuration)
        {
            RunStep(() => { Client.AddVideoSourceConfiguration(profile, configuration); },
                string.Format(Resources.StepAddVideoSourceConfig_Format, configuration, profile));
            
            DoRequestDelay();
        }

        /// <summary>
        /// Adds audio source configuration to profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <param name="configuration">Token of configuration</param>
        protected void AddAudioSourceConfiguration(string profile, string configuration)
        {
            RunStep(() => { Client.AddAudioSourceConfiguration(profile, configuration); }, 
                string.Format(Resources.StepAddAudioSourceConfig_Format, configuration, profile));

            DoRequestDelay();
        }

        /// <summary>
        /// Adds video encoder configuration to profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <param name="configuration">Token of configuration</param>
        protected void AddVideoEncoderConfiguration(string profile, string configuration)
        {
            RunStep(() => { Client.AddVideoEncoderConfiguration(profile, configuration); },
                string.Format(Resources.StepAddVideoEncoderConfig_Format, configuration, profile));

            DoRequestDelay();
        }

        /// <summary>
        /// Adds PTZ configuration to profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <param name="configuration">Token of configuration</param>
        protected void AddPTZConfiguration(string profile, string configuration)
        {
            RunStep(() => { Client.AddPTZConfiguration(profile, configuration); },
                string.Format(Resources.StepAddPTZConfig_Format, configuration, profile));

            DoRequestDelay();
        }

        /// <summary>
        /// Adds audio encoder configuration to profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <param name="configuration">Token of configuration</param>
        protected void AddAudioEncoderConfiguration(string profile, string configuration)
        {
            RunStep(() => { Client.AddAudioEncoderConfiguration(profile, configuration); },
                string.Format(Resources.StepAddAudioEncoderConfig_Format, configuration, profile));
            
            DoRequestDelay();
        }

        /// <summary>
        /// Adds metadata configuration to profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <param name="configuration">Token of configuration</param>
        protected void AddMetadataConfiguration(string profile, string configuration)
        {
            RunStep(() => { Client.AddMetadataConfiguration(profile, configuration); },
                string.Format(Resources.StepAddMetadataConfig_Format, configuration, profile));

            DoRequestDelay();
        }

        /// <summary>
        /// Removes video source configuration from profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        protected void RemoveVideoSourceConfiguration(string profile)
        {
            RunStep(() => { Client.RemoveVideoSourceConfiguration(profile); }, 
                string.Format(Resources.StepRemoveVideoSourceConfig_Format, profile));

            DoRequestDelay();
        }

        /// <summary>
        /// Removes audio source configuration from profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        protected void RemoveAudioSourceConfiguration(string profile)
        {
            RunStep(() => { Client.RemoveAudioSourceConfiguration(profile); },
                string.Format(Resources.StepRemoveAudioSourceConfig_Format, profile));

            DoRequestDelay();
        }

        /// <summary>
        /// Removes video encoder configuration from profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        protected void RemoveVideoEncoderConfiguration(string profile)
        {
            RunStep(() => { Client.RemoveVideoEncoderConfiguration(profile); },
                string.Format(Resources.StepRemoveVideoEncoderConfig_Format, profile));

            DoRequestDelay();
        }

        /// <summary>
        /// Removes audio encoder configuration from profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        protected void RemoveAudioEncoderConfiguration(string profile)
        {
            RunStep(() => { Client.RemoveAudioEncoderConfiguration(profile); },
                string.Format(Resources.StepRemoveAudioEncoderConfig_Format, profile));

            DoRequestDelay();
        }

        /// <summary>
        /// Removes metadata configuration from profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        protected void RemoveMetadataConfiguration(string profile)
        {
            RunStep(() => { Client.RemoveMetadataConfiguration(profile); },
                string.Format(Resources.StepRemoveMetadataConfig_Format, profile));

            DoRequestDelay();
        }

        /// <summary>
        /// Removes PTZ configuration from profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        protected void RemovePTZConfiguration(string profile)
        {
            RunStep(() => { Client.RemovePTZConfiguration(profile); }, 
                string.Format(Resources.StepRemovePTZConfig_Format, profile));

            DoRequestDelay();
        }

        /// <summary>
        /// Returns media profile from DUT
        /// </summary>
        /// <param name="token">Token of profile</param>
        /// <returns>Media profile</returns>
        protected Profile GetProfile(string token)
        {
            Profile profile = null;
            RunStep(() => { profile = Client.GetProfile(token); }, Resources.StepGetMediaProfile_Title);
            DoRequestDelay();
            return profile;
        }

        /// <summary>
        /// Performs negative get profile test step
        /// </summary>
        /// <param name="token">Token of profile</param>
        protected void GetInvalidProfile(string token)
        {
            RunStep(() => { 
                Client.GetProfile(token); }, 
                string.Format(Resources.StepGetMediaProfileNegative_Format, token),
                "Sender/InvalidArgVal/NoProfile", 
                true);

            DoRequestDelay();
        }

        /// <summary>
        /// Validates media profile 
        /// </summary>
        /// <param name="profile">Profile to be validated</param>
        /// <param name="videoSourceConfig">Token of expected video source configuration</param>
        /// <param name="videoEncoderConfig">Token of expected video encoder configuration</param>
        /// <param name="reason">Reason why profile is invalid, null profile is invalid</param>
        /// <returns>true, if profile is valid</returns>
        protected bool ValidateProfile(Profile profile, string videoSourceConfig, string videoEncoderConfig, out string reason)
        {
            reason = null;
            if(profile == null)
            {
                reason = Resources.ErrorNoMediaProfiles_Text; 
                return false;
            }
            if (profile.VideoSourceConfiguration == null)
            {
                reason = Resources.ErrorNoVideoSourceConfig_Text;
                return false;
            }
            if (profile.VideoEncoderConfiguration == null)
            {
                reason = Resources.ErrorNoVideoEncoderConfig_Text;
                return false;
            }
            if (profile.VideoSourceConfiguration.token != videoSourceConfig)
            {
                reason = string.Format(Resources.ErrorWrongVideoSourceConfig_Format, videoSourceConfig, profile.VideoSourceConfiguration.token);
                return false;
            }
            if (profile.VideoEncoderConfiguration.token != videoEncoderConfig)
            {
                reason = string.Format(Resources.ErrorWrongVideoEncoderConfig_Format, videoEncoderConfig, profile.VideoEncoderConfiguration.token);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Deletes media profile from DUT
        /// </summary>
        /// <param name="token">Token of profile to be deleted</param>
        protected void DeleteProfile(string token)
        {
            RunStep(() => { Client.DeleteProfile(token); }, 
                string.Format(Resources.StepDeleteMediaProfile_Format, token));

            DoRequestDelay();
        }

        /// <summary>
        /// Retrieves lists of video sources form DUT
        /// </summary>
        /// <returns>Array of video sources</returns>
        protected VideoSource[] GetVideoSources()
        {
            VideoSource[] sources = null;
            RunStep(() => { sources = Client.GetVideoSources(); }, Resources.StepGetVideoSources_Title);
            DoRequestDelay(); 
            return sources;
        }

        /// <summary>
        /// Retrieves lists of audio sources form DUT
        /// </summary>
        /// <returns>Array of audio sources</returns>
        protected AudioSource[] GetAudioSources()
        {
            AudioSource[] sources = null;
            RunStep(() => { sources = Client.GetAudioSources(); }, Resources.StepGetAudioSources_Title);
            DoRequestDelay();
            return sources;
        }

        /// <summary>
        /// Validates list of video sources
        /// </summary>
        /// <param name="sources">Array of sources to be validated</param>
        /// <param name="reason">Reason why list of sources is invalid</param>
        /// <returns>true, is list of sources is valid</returns>
        protected bool ValidateVideoSources(VideoSource[] sources, out string reason)
        {
            reason = null;
            if((sources == null)||(sources.Length < 1))
            {
                reason = Resources.ErrorNoVideoSources_Text;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates list of audio sources
        /// </summary>
        /// <param name="sources">Array of sources to be validated</param>
        /// <param name="reason">Reason why list of sources is invalid</param>
        /// <returns>true, is list of sources is valid</returns>
        protected bool ValidateAudioSources(AudioSource[] sources, out string reason)
        {
            reason = null;
            if ((sources == null) || (sources.Length < 1))
            {
                reason = Resources.ErrorNoAudioSources_Text;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Retrieves lists of video source configurations compatible with specified profile from DUT
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <returns>Array of video source configurations</returns>
        protected VideoSourceConfiguration[] GetCompatibleVideoSourceConfigurations(string profile)
        {
            VideoSourceConfiguration[] configs = null;
            RunStep(() => { configs = Client.GetCompatibleVideoSourceConfigurations(profile); },
                string.Format(Resources.StepGetCompatibleVideoSourceConfigs_Format, profile));
            DoRequestDelay();
            
            return configs;
        }

        /// <summary>
        /// Retrieves lists of video source configurations from DUT
        /// </summary>
        /// <returns>Array of video source configurations</returns>
        protected VideoSourceConfiguration[] GetVideoSourceConfigurations()
        {
            VideoSourceConfiguration[] configs = null;
            RunStep(() => { configs = Client.GetVideoSourceConfigurations(); }, Resources.StepGetVideoSourceConfigs_Title);
            DoRequestDelay();
            return configs;
        }

        /// <summary>
        /// Retrieves lists of audio source configurations from DUT
        /// </summary>
        /// <returns>Array of audio source configurations</returns>
        protected AudioSourceConfiguration[] GetAudioSourceConfigurations()
        {
            AudioSourceConfiguration[] configs = null;
            RunStep(() => { configs = Client.GetAudioSourceConfigurations(); }, Resources.StepGetAudioSourceConfigs_Title);
            DoRequestDelay();
            return configs;
        }

        /// <summary>
        /// Retrieves lists of metadata configurations from DUT
        /// </summary>
        /// <returns>Array of metadata configurations</returns>
        protected MetadataConfiguration[] GetMetadataConfigurations()
        {
            MetadataConfiguration[] configs = null;
            RunStep(() => { configs = Client.GetMetadataConfigurations(); }, Resources.StepGetMetadataConfigs_Title);
            DoRequestDelay();
            return configs;
        }

        /// <summary>
        /// Retrieves lists of PTZ configurations from DUT
        /// </summary>
        /// <returns>Array of PTZ configurations</returns>
        protected PTZConfiguration[] GetPTZConfigurations()
        {
            string ptzAddress = string.Empty;
            RunStep(() =>
            {
                Binding binding =
                    CreateBinding(false,
                    new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                DeviceClient device = new DeviceClient(binding, new EndpointAddress(_cameraAddress));
                AttachSecurity(device.Endpoint);
                SetupChannel(device.InnerChannel);

                ptzAddress = device.GetPtzServiceAddress(Features);
                
                device.Close();

                if(string.IsNullOrEmpty(ptzAddress))
                {
                    throw new AssertException(Resources.ErrorNoPTZAddress_Text);
                }
                else
                {
                    LogStepEvent("PTZ service address: " + ptzAddress);
                }
            }, Resources.StepGetPTZAddress_Title);
            DoRequestDelay();

            PTZConfiguration[] configs = null;
            RunStep(() =>
            {
                Binding binding = CreateBinding(false,
                    new IChannelController[] { new SoapValidator(PtzSchemasSet.GetInstance()) });

                PTZClient ptz = new PTZClient(binding, new EndpointAddress(ptzAddress));
                if((Client == null)||(!EndpointAddress.Equals(Client.Endpoint, ptz.Endpoint)))
                {
                    AttachSecurity(ptz.Endpoint);
                }
                configs = ptz.GetConfigurations();

                ptz.Close();

                if((configs == null)||(configs.Length == 0))
                {
                    throw new AssertException(Resources.ErrorNoPTZConfigurations_Text);
                }

            }, Resources.StepGetPTZConfigurations_Title);
            DoRequestDelay();

            return configs;
        }

        /// <summary>
        /// Retrieves lists of video encoder configurations compatible with specified profile from DUT
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <returns>Array of video encoder configurations</returns>
        protected VideoEncoderConfiguration[] GetCompatibleVideoEncoderConfigurations(string profile)
        {
            VideoEncoderConfiguration[] configs = null;
            RunStep(() => { configs = Client.GetCompatibleVideoEncoderConfigurations(profile); }, 
                string.Format(Resources.StepGetCompatibleVideoEncoderConfigs_Format, profile));
            DoRequestDelay();
            return configs;
        }

        /// <summary>
        /// Retrieves lists of audio encoder configurations compatible with specified profile from DUT
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <returns>Array of audio encoder configurations</returns>
        protected AudioEncoderConfiguration[] GetCompatibleAudioEncoderConfigurations(string profile)
        {
            AudioEncoderConfiguration[] configs = null;
            RunStep(() => { configs = Client.GetCompatibleAudioEncoderConfigurations(profile); }, 
                string.Format(Resources.StepGetCompatibleAudioEncoderConfigs_Format, profile));
            DoRequestDelay();
            return configs;
        }

        /// <summary>
        /// Retrieves lists of audio source configurations compatible with specified profile from DUT
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <returns>Array of audio source configurations</returns>
        protected AudioSourceConfiguration[] GetCompatibleAudioSourceConfigurations(string profile)
        {
            AudioSourceConfiguration[] configs = null;
            RunStep(() => { configs = Client.GetCompatibleAudioSourceConfigurations(profile); }, 
                string.Format(Resources.StepGetCompatibleAudioSourceConfigs_Format, profile));
            DoRequestDelay();
            return configs;
        }

        /// <summary>
        /// Retrieves lists of metadata configurations compatible with specified profile from DUT
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <returns>Array of metadata configurations</returns>
        protected MetadataConfiguration[] GetCompatibleMetadataConfigurations(string profile)
        {
            MetadataConfiguration[] configs = null;
            RunStep(() => { configs = Client.GetCompatibleMetadataConfigurations(profile); }, 
                string.Format(Resources.StepGetCompatibleMetadataConfigs_Format, profile));
            DoRequestDelay();
            return configs;
        }

        /// <summary>
        /// Retrieves lists of video encoder configurations from DUT
        /// </summary>
        /// <returns>Array of video encoder configurations</returns>
        protected VideoEncoderConfiguration[] GetVideoEncoderConfigurations()
        {
            VideoEncoderConfiguration[] configs = null;
            RunStep(() => { configs = Client.GetVideoEncoderConfigurations(); }, Resources.StepGetEncoderSourceConfigs_Title);
            DoRequestDelay();
            return configs;
        }

        /// <summary>
        /// Retrieves lists of audio encoder configurations from DUT
        /// </summary>
        /// <returns>Array of audio encoder configurations</returns>
        protected AudioEncoderConfiguration[] GetAudioEncoderConfigurations()
        {
            AudioEncoderConfiguration[] configs = null;
            RunStep(() => { configs = Client.GetAudioEncoderConfigurations(); }, Resources.StepGetAudioEncoderConfigs_Title);
            DoRequestDelay();
            return configs;
        }

        /// <summary>
        /// Returns video source configuration from DUT
        /// </summary>
        /// <param name="token">Token of configuration</param>
        /// <returns>Video source configuration</returns>
        protected VideoSourceConfiguration GetVideoSourceConfiguration(string token)
        {
            VideoSourceConfiguration config = null;
            RunStep(() => { config = Client.GetVideoSourceConfiguration(token); }, Resources.StepGetVideoSourceConfig_Title);
            DoRequestDelay();
            return config;
        }

        /// <summary>
        /// Returns audio source configuration from DUT
        /// </summary>
        /// <param name="token">Token of configuration</param>
        /// <returns>Audio source configuration</returns>
        protected AudioSourceConfiguration GetAudioSourceConfiguration(string token)
        {
            AudioSourceConfiguration config = null;
            RunStep(() => { config = Client.GetAudioSourceConfiguration(token); }, Resources.StepGetAudioSourceConfig_Title);
            DoRequestDelay();
            return config;
        }

        /// <summary>
        /// Returns video encoder configuration from DUT
        /// </summary>
        /// <param name="token">Token of configuration</param>
        /// <returns>Video encoder configuration</returns>
        protected VideoEncoderConfiguration GetVideoEncoderConfiguration(string token)
        {
            VideoEncoderConfiguration config = null;
            RunStep(() => { config = Client.GetVideoEncoderConfiguration(token); }, Resources.StepGetVideoEncoderConfig_Title);
            DoRequestDelay();
            return config;
        }

        /// <summary>
        /// Returns video encoder configuration from DUT
        /// </summary>
        /// <param name="token">Token of configuration</param>
        /// <returns>Video encoder configuration</returns>
        protected AudioEncoderConfiguration GetAudioEncoderConfiguration(string token)
        {
            AudioEncoderConfiguration config = null;
            RunStep(() => { config = Client.GetAudioEncoderConfiguration(token); }, Resources.StepGetAudioEncoderConfig_Title);
            DoRequestDelay();
            return config;
        }

        /// <summary>
        /// Gets audio encoder configuration options for given configuration token or profile token.
        /// </summary>
        /// <param name="configurationToken">Configuration token.</param>
        /// <param name="profileToken">Profile token.</param>
        /// <returns>Audio encoder configuration options</returns>
        protected AudioEncoderConfigurationOptions GetAudioEncoderConfigurationOptions(string configurationToken, string profileToken)
        {
            return GetAudioEncoderConfigurationOptions(configurationToken, profileToken,
                                                     "Get audio encoder configuration options");
        }

        /// <summary>
        /// Gets audio encoder configuration options for given configuration token or profile token.
        /// </summary>
        /// <param name="configurationToken">Configuration token.</param>
        /// <param name="profileToken">Profile token.</param>
        /// <param name="stepName">Step name to be used.</param>
        /// <returns>Audio encoder configuration options</returns>
        protected AudioEncoderConfigurationOptions GetAudioEncoderConfigurationOptions(string configurationToken, string profileToken, string stepName)
        {
            AudioEncoderConfigurationOptions options = null;
            RunStep(() => { options = Client.GetAudioEncoderConfigurationOptions(configurationToken, profileToken); }, stepName);
            DoRequestDelay();
            return options;
        }

        /// <summary>
        /// Returns metadata configuration from DUT
        /// </summary>
        /// <param name="token">Token of configuration</param>
        /// <returns>Metadata configuration</returns>
        protected MetadataConfiguration GetMetadataConfiguration(string token)
        {
            MetadataConfiguration config = null;
            RunStep(() => { config = Client.GetMetadataConfiguration(token); }, Resources.StepGetMetadataConfig_Title);
            DoRequestDelay();
            return config;
        }

        /// <summary>
        /// Validate array of video source configurations
        /// </summary>
        /// <param name="configs">Array of video source configurations</param>
        /// <param name="reason">Reason why array of video source configurations is invalid</param>
        /// <returns>true, if list of configurations is valid</returns>
        protected bool ValidateVideoSourceConfigs(VideoSourceConfiguration[] configs, out string reason)
        {
            reason = null;
            if ((configs == null) || (configs.Length < 1))
            {
                reason = Resources.ErrorNoVideoSourceConfigs_Text;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate array of metadata configurations
        /// </summary>
        /// <param name="configs">Array of metadata configurations</param>
        /// <param name="reason">Reason why array of metadata configurations is invalid</param>
        /// <returns>true, if list of configurations is valid</returns>
        protected bool ValidateMetadataConfigs(MetadataConfiguration[] configs, out string reason)
        {
            reason = null;
            if ((configs == null) || (configs.Length < 1))
            {
                reason = Resources.ErrorNoMetadataConfigs_Text;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate array of metadata configurations
        /// </summary>
        /// <param name="configs">Array of metadata configurations</param>
        /// <param name="reason">Reason why array of metadata configurations is invalid</param>
        /// <returns>true, if list of configurations is valid</returns>
        protected bool ValidateAudioSourceConfigs(AudioSourceConfiguration[] configs, out string reason)
        {
            reason = null;
            if ((configs == null) || (configs.Length < 1))
            {
                reason = Resources.ErrorNoAudioSourceConfigs_Text;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate array of video encoder configurations
        /// </summary>
        /// <param name="configs">Array of video encoder configurations</param>
        /// <param name="reason">Reason why array of video encoder configurations is invalid</param>
        /// <returns>true, if list of configurations is valid</returns>
        protected bool ValidateVideoEncoderConfigs(VideoEncoderConfiguration[] configs, out string reason)
        {
            reason = null;
            if ((configs == null) || (configs.Length < 1))
            {
                reason = Resources.ErrorNoVideoEncoderConfigs_Text;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate array of audio encoder configurations
        /// </summary>
        /// <param name="configs">Array of audio encoder configurations</param>
        /// <param name="reason">Reason why array of audio encoder configurations is invalid</param>
        /// <returns>true, if list of configurations is valid</returns>        
        protected bool ValidateAudioEncoderConfigs(AudioEncoderConfiguration[] configs, out string reason)
        {
            reason = null;
            if ((configs == null) || (configs.Length < 1))
            {
                reason = Resources.ErrorNoAudioEncoderConfigs_Text;
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Returns video source configuration options for specified profile and configuration
        /// </summary>
        /// <param name="profile">token of profile, can be null</param>
        /// <param name="config">token of configuration</param>
        /// <returns>Configuration options</returns>
        protected VideoSourceConfigurationOptions GetVideoSourceConfigurationOptions(string profile, string config)
        {
            VideoSourceConfigurationOptions options = null;
            RunStep(() => { options = Client.GetVideoSourceConfigurationOptions(config, profile);}, 
                string.Format(Resources.StepGetVideoSourceConfigOptions_Format, config));
            DoRequestDelay();
            return options;
        }

        /// <summary>
        /// Returns audio source configuration options for specified profile and configuration
        /// </summary>
        /// <param name="profile">token of profile, can be null</param>
        /// <param name="config">token of configuration</param>
        /// <returns>Configuration options</returns>
        protected AudioSourceConfigurationOptions GetAudioSourceConfigurationOptions(string profile, string config)
        {
            return GetAudioSourceConfigurationOptions(
                profile, 
                config,
                string.Format(Resources.StepGetAudioSourceConfigOptions_Format, config));
        }

        /// <summary>
        /// Returns audio source configuration options for specified profile and configuration
        /// </summary>
        /// <param name="profile">token of profile, can be null</param>
        /// <param name="config">token of configuration</param>
        /// <returns>Configuration options</returns>
        protected AudioSourceConfigurationOptions GetAudioSourceConfigurationOptions(string profile, string config, string stepName)
        {
            AudioSourceConfigurationOptions options = null;
            RunStep(() => { options = Client.GetAudioSourceConfigurationOptions(config, profile); },
                stepName);
            DoRequestDelay();
            return options;
        }

        /// <summary>
        /// Returns metadata configuration options for specified profile and configuration
        /// </summary>
        /// <param name="profile">token of profile, can be null</param>
        /// <param name="config">token of configuration</param>
        /// <returns>Configuration options</returns>
        protected MetadataConfigurationOptions GetMetadataConfigurationOptions(string profile, string config)
        {
            MetadataConfigurationOptions options = null;
            RunStep(() => { options = Client.GetMetadataConfigurationOptions(config, profile);},
                string.Format(Resources.StepGetMetadataConfigOptions_Format, config));
            DoRequestDelay();
            return options;
        }

        /// <summary>
        /// Searches for video encoder configuration and configuration options with specified encoding in list of configurations
        /// </summary>
        /// <param name="configs">List of configurations</param>
        /// <param name="encoding">Encoding to be searched</param>
        /// <param name="options">Options for found configuration</param>
        /// <returns>Video encoder configuration</returns>
        protected VideoEncoderConfiguration GetVideoEncoderConfiguration(VideoEncoderConfiguration[] configs, VideoEncoding encoding, out VideoEncoderConfigurationOptions options)
        {
            VideoEncoderConfiguration encoderConfig = null;
            options = null;
            VideoEncoderConfigurationOptions encoderOptions = null;
            RunStep(() =>
            {
                foreach (VideoEncoderConfiguration config in configs)
                {
                    LogStepEvent(string.Format(Resources.StepGetVideoEncoderConfigOptions_Format, config.token));
                    encoderOptions = Client.GetVideoEncoderConfigurationOptions(config.token, null);
                    if (encoderOptions == null)
                    {
                        throw new AssertException("No valid VideoEncoderConfigurationOptions");
                    }
                    if (((encoding == VideoEncoding.JPEG) && (encoderOptions.JPEG != null)) ||
                        ((encoding == VideoEncoding.H264) && (encoderOptions.H264 != null))||
                        ((encoding == VideoEncoding.MPEG4) && (encoderOptions.MPEG4 != null)))
                    {
                        encoderConfig = config;
                        break;
                    }

                    // make pause here - to avoid extra pauses
                    DoRequestDelay();
                }
                if (encoderConfig == null)
                {
                    string error = Resources.ErrorNoJpegVideoEncoderConfig_Text;
                    if (encoding == VideoEncoding.H264)
                    {
                        error = Resources.ErrorNoH264VideoEncoderConfig_Text;
                    }
                    else if (encoding == VideoEncoding.MPEG4)
                    {
                        error = Resources.ErrorNoMPEG4VideoEncoderConfig_Text;
                    }
                    throw new AssertException(error);
                }
                else
                {
                    if ((encoding == VideoEncoding.JPEG) && ((encoderOptions.JPEG.ResolutionsAvailable == null) || (encoderOptions.JPEG.ResolutionsAvailable.Length == 0)))
                    {
                        new AssertException(Resources.ErrorJpegVideoEncoderConfigOptions_Text);
                    }
                    else if ((encoding == VideoEncoding.MPEG4) && ((encoderOptions.MPEG4.ResolutionsAvailable == null) || (encoderOptions.MPEG4.ResolutionsAvailable.Length == 0)))
                    {
                        new AssertException(Resources.ErrorMpeg4VideoEncoderConfigOptions_Text);
                    }
                    else if ((encoding == VideoEncoding.H264) && ((encoderOptions.H264.ResolutionsAvailable == null) || (encoderOptions.H264.ResolutionsAvailable.Length == 0)))
                    {
                        new AssertException(Resources.ErrorH264VideoEncoderConfigOptions_Text);
                    }
                }
            }, Resources.StepGetVideoEncoderConfigOptions_Title);
            options = encoderOptions;
            return encoderConfig;
        }

        /// <summary>
        /// Searches for audio encoder configuration and configuration options with specified encoding in list of configurations
        /// </summary>
        /// <param name="configs">List of configurations</param>
        /// <param name="encoding">Encoding to be searched</param>
        /// <param name="options">Options for found configuration</param>
        /// <returns>Audio encoder configuration</returns>
        protected AudioEncoderConfiguration GetAudioEncoderConfiguration(AudioEncoderConfiguration[] configs, AudioEncoding encoding, out AudioEncoderConfigurationOption options)
        {
            AudioEncoderConfiguration encoderConfig = null;
            options = null;
            AudioEncoderConfigurationOption encoderOption = null;
            RunStep(() =>
            {
                foreach (AudioEncoderConfiguration config in configs)
                {
                    LogStepEvent(string.Format(Resources.StepGetAudioEncoderConfigOptions_Format, config.token));
                    AudioEncoderConfigurationOptions encoderOptions = Client.GetAudioEncoderConfigurationOptions(config.token, null);
                    if (encoderOptions == null)
                    {
                        throw new AssertException("No valid AudioEncoderConfigurationOptions");
                    }

                    DoRequestDelay();

                    if(encoderOptions.Options != null)
                    {
                        foreach (AudioEncoderConfigurationOption option in encoderOptions.Options)
                        {
                            if(option.Encoding == encoding)
                            {
                                encoderOption = option;
                                encoderConfig = config;
                                break;
                            }
                        }
                        if(encoderConfig != null)
                        {
                            break;
                        }
                    }
                }
                if (encoderConfig == null)
                {
                    string error = Resources.ErrorNoG711AudioEncoderConfig_Text;
                    if (encoding == AudioEncoding.G726)
                    {
                        error = Resources.ErrorNoG726AudioEncoderConfig_Text;
                    }
                    else if (encoding == AudioEncoding.AAC)
                    {
                        error = Resources.ErrorNoAACAudioEncoderConfig_Text;
                    }
                    throw new AssertException(error);
                }
                else
                {
                    if((encoderOption.SampleRateList == null)||(encoderOption.SampleRateList.Length == 0)||
                       (encoderOption.BitrateList == null) || (encoderOption.BitrateList.Length == 0))
                    {
                        throw new AssertException(Resources.ErrorInvalidAudioEncoderConfigOptions_Text);
                    }
                }
            }, Resources.StepGetAudioEncoderConfigOptions_Title);
            options = encoderOption;
            return encoderConfig;
        }

        /// <summary>
        /// Performs negative test step, setting invalid video source configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="persistence">Force persistence flag</param>
        /// <param name="details">details about invalid parameters in configuration</param>
        protected void SetInvalidVideoSourceConfiguration(VideoSourceConfiguration config, bool persistence, string details)
        {
            RunStep(() => {
                if (!string.IsNullOrEmpty(details))
                {
                    LogStepEvent(details);
                }
                Client.SetVideoSourceConfiguration(config, persistence); 
            }, Resources.StepSetVideoSourceInvalidConfig_Title, "Sender/InvalidArgVal/ConfigModify", true);

            DoRequestDelay();
        }

        /// <summary>
        /// Performs negative test step, setting invalid video encoder configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="persistence">Force persistence flag</param>
        /// <param name="details">details about invalid parameters in configuration</param>
        protected void SetInvalidVideoEncoderConfiguration(VideoEncoderConfiguration config, bool persistence, string details)
        {
            RunStep(() => {
                if (!string.IsNullOrEmpty(details))
                {
                    LogStepEvent(details);
                } 
                Client.SetVideoEncoderConfiguration(config, persistence);
            }, Resources.StepSetVideoEncoderInvalidConfig_Title, "Sender/InvalidArgVal/ConfigModify", true);

            DoRequestDelay();
        }

        /// <summary>
        /// Performs negative test step, setting invalid audio encoder configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="persistence">Force persistence flag</param>
        /// <param name="details">details about invalid parameters in configuration</param>
        protected void SetInvalidAudioEncoderConfiguration(AudioEncoderConfiguration config, bool persistence, string details)
        {
            RunStep(() => {
                if (!string.IsNullOrEmpty(details))
                {
                    LogStepEvent(details);
                }
                Client.SetAudioEncoderConfiguration(config, persistence);
            }, Resources.StepSetAudioEncoderInvalidConfig_Title, "Sender/InvalidArgVal/ConfigModify", true);

            DoRequestDelay();
        }

        /// <summary>
        /// Performs negative test step, setting invalid audio source configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="persistence">Force persistence flag</param>
        /// <param name="details">details about invalid parameters in configuration</param>
        protected void SetInvalidAudioSourceConfiguration(AudioSourceConfiguration config, bool persistence, string details)
        {
            RunStep(() => {
                if (!string.IsNullOrEmpty(details))
                {
                    LogStepEvent(details);
                }
                Client.SetAudioSourceConfiguration(config, persistence);
            }, Resources.StepSetAudioSourceInvalidConfig_Title, "Sender/InvalidArgVal/ConfigModify", true);

            DoRequestDelay();
        }

        /// <summary>
        /// Performs negative test step, setting invalid metadata configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="persistence">Force persistence flag</param>
        /// <param name="details">details about invalid parameters in configuration</param>
        protected void SetInvalidMetadataConfiguration(MetadataConfiguration config, bool persistence, string details)
        {
            RunStep(() => {
                if (!string.IsNullOrEmpty(details))
                {
                    LogStepEvent(details);
                }
                Client.SetMetadataConfiguration(config, persistence);
            }, Resources.StepSetMetadataInvalidConfig_Title, "Sender/InvalidArgVal/ConfigModify", true);

            DoRequestDelay();
        }

        /// <summary>
        /// Performs negative test step, setting invalid metadata configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="persistence">Force persistence flag</param>
        /// <param name="details">details about invalid parameters in configuration</param>
        protected void SetVideoSourceConfiguration(VideoSourceConfiguration config, bool persistence)
        {
            RunStep(() => { Client.SetVideoSourceConfiguration(config, persistence); }, Resources.StepSetVideoSourceConfig_Title);
            DoRequestDelay();
        }

        /// <summary>
        /// Sets audio source configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="persistence">Force persistence flag</param>
        protected void SetAudioSourceConfiguration(AudioSourceConfiguration config, bool persistence)
        {
            RunStep(() => { Client.SetAudioSourceConfiguration(config, persistence); }, Resources.StepSetAudioSourceConfig_Title);
            DoRequestDelay();
        }

        /// <summary>
        /// Displays Multicast configuration information.
        /// </summary>
        /// <param name="config">Configuration.</param>
        /// <param name="sessionTimeout">Session timeout.</param>
        void DumpMulticastConfiguration(MulticastConfiguration config, string sessionTimeout)
        {
            LogStepEvent("Multicast address type = " + (config.Address.Type == IPType.IPv4 ? "IPv4" : "IPv6"));
            if (config.Address.Type == IPType.IPv4)
            {
                LogStepEvent("Multicast address = " + config.Address.IPv4Address);
            }
            else
            {
                LogStepEvent("Multicast address = " + config.Address.IPv6Address);
            }
            LogStepEvent("Multicast port = " + config.Port);
            LogStepEvent("Multicast TTL = " + config.TTL);
            LogStepEvent("Session timeout = " + sessionTimeout);
        }

        /// <summary>
        /// Prints fields of video encoder configuration to log
        /// </summary>
        /// <param name="config">Configuraion to be logged</param>
        /// <param name="multicast">Log multicast configuration</param>
        void DumpVideoEncoderConfiguration(VideoEncoderConfiguration config, bool multicast)
        {
            LogStepEvent("Configuration token = " + config.token);
            LogStepEvent("Configuration name = " + config.Name);
            LogStepEvent("Configuration encoding = " + config.Encoding.ToString());
            if (config.RateControl != null)
            {
                LogStepEvent("Frame rate = " + config.RateControl.FrameRateLimit);
            }
            if (config.Resolution != null)
            {
                LogStepEvent("Width = " + config.Resolution.Width);
                LogStepEvent("Height = " + config.Resolution.Height);
            }
            if ((config.Encoding == VideoEncoding.MPEG4) && (config.MPEG4 != null))
            {
                LogStepEvent("MPEG4 len = " + config.MPEG4.GovLength);
                LogStepEvent("MPEG4 Profile = " + config.MPEG4.Mpeg4Profile.ToString());
            }
            if ((config.Encoding == VideoEncoding.H264) && (config.H264 != null))
            {
                LogStepEvent("H.264 len = " + config.H264.GovLength);
                LogStepEvent("H.264 Profile = " + config.H264.H264Profile.ToString());
            }
            if (multicast)
            {
                DumpMulticastConfiguration(config.Multicast, config.SessionTimeout);
            }
        }

        /// <summary>
        /// Sets video encoder configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="persistence">Force persistence flag</param>
        /// <param name="multicast">Print fields of multicast configuration</param>
        protected void SetVideoEncoderConfiguration(VideoEncoderConfiguration config, 
            bool persistence, 
            bool multicast, 
            string stepName)
        {
            //config.RateControl.FrameRateLimit = 25;
            if ((config.Encoding == VideoEncoding.MPEG4) && (config.MPEG4 != null))
            {
                if (config.MPEG4.GovLength > 32)
                {
                    config.MPEG4.GovLength = 32;
                }
            }
            if ((config.Encoding == VideoEncoding.H264) && (config.H264 != null))
            {
                if (config.H264.GovLength > 32)
                {
                    config.H264.GovLength = 32;
                }
            }
            RunStep(() =>
            {
                DumpVideoEncoderConfiguration(config, multicast);
                Client.SetVideoEncoderConfiguration(config, persistence); 
            }, stepName);

            DoRequestDelay();
        }

       protected void SetVideoEncoderConfiguration(VideoEncoderConfiguration config, 
            bool persistence, 
            bool multicast)
        {
            SetVideoEncoderConfiguration(config, persistence, multicast, Resources.StepSetVideoEncoderConfig_Title);
        }

        /// <summary>
        /// Sets video encoder configuration.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="persistence"></param>
        protected void SetVideoEncoderConfiguration(VideoEncoderConfiguration config, bool persistence)
        {
            SetVideoEncoderConfiguration(config, persistence, false);
        }

        /// <summary>
        /// Displays audio encoder configuration information.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="multicast"></param>
        void DumpAudioEncoderConfiguration(AudioEncoderConfiguration config, bool multicast)
        {
            LogStepEvent("Configuration token = " + config.token);
            LogStepEvent("Configuration name = " + config.Name);
            LogStepEvent("Configuration encoding = " + config.Encoding.ToString());
            LogStepEvent("Bitrate = " + config.Bitrate);
            LogStepEvent("SampleRate = " + config.SampleRate);
            if (multicast)
            {
                DumpMulticastConfiguration(config.Multicast, config.SessionTimeout);
            }
        }

        /// <summary>
        /// Sets audio encoder configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="persistence">Force persistence flag</param>
        /// <param name="multicast">Print fields of multicast configuration</param>
        protected void SetAudioEncoderConfiguration(AudioEncoderConfiguration config, bool persistence, bool multicast)
        {
            RunStep(() =>
            {
                DumpAudioEncoderConfiguration(config, multicast);
                Client.SetAudioEncoderConfiguration(config, persistence);
            }, Resources.StepSetAudioEncoderConfig_Title);

            DoRequestDelay();
        }

        /// <summary>
        /// Sets audio encoder configuration.
        /// </summary>
        /// <param name="config">Configuration.</param>
        /// <param name="persistence">Persistence</param>
        protected void SetAudioEncoderConfiguration(AudioEncoderConfiguration config, bool persistence)
        {
            SetAudioEncoderConfiguration(config, persistence, false);
        }

        /// <summary>
        /// Sets metadata configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="persistence">Force persistence flag</param>
        protected void SetMetadataConfiguration(MetadataConfiguration config, bool persistence)
        {
            RunStep(() => { Client.SetMetadataConfiguration(config, persistence); }, Resources.StepSetMetadataConfig_Title);
            DoRequestDelay();
        }

        /// <summary>
        /// Sets metadata configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="persistence">Force persistence flag</param>
        protected void SetSynchronizationPoint(string ProfileToken)
        {
            RunStep(() => { Client.SetSynchronizationPoint(ProfileToken); }, "SetSynchronizationPoint");
            DoRequestDelay();
        }

        /// <summary>
        /// Compares video source configurations
        /// </summary>
        /// <param name="a">Configuration to be compared</param>
        /// <param name="b">Configuration to be compared to</param>
        /// <param name="reason">Reason why configurations are not equal</param>
        /// <returns>true, if configurations are equal</returns>
        protected bool EqualConfigurations(VideoSourceConfiguration a, VideoSourceConfiguration b, out string reason)
        {
            reason = null;
            if (a == b)
            {
                reason = null;
                return true;
            }
            if ((a == null) || (b == null))
            {
                reason = Resources.ErrorOneConfigIsEmpty_Text;
                return false;
            }
            if (a.SourceToken != b.SourceToken)
            {
                reason = "configurations have different source tokens";
                return false;
            } 
            if (a.Name != b.Name)
            {
                reason = "configurations have different names";
                return false;
            } 
            if (a.token != b.token)
            {
                reason = "configurations have different tokens";
                return false;
            } 
            if (a.Bounds.height != b.Bounds.height)
            {
                reason = "configurations have different bounds";
                return false;
            } 
            if (a.Bounds.width != b.Bounds.width)
            {
                reason = "configurations have different bounds";
                return false;
            } 
            if (a.Bounds.x != b.Bounds.x)
            {
                reason = "configurations have different bounds";
                return false;
            } 
            if (a.Bounds.y != b.Bounds.y)
            {
                reason = "configurations have different bounds";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Compares video resolutions
        /// </summary>
        /// <param name="a">Resolution to be compared</param>
        /// <param name="b">Resolution to be compared to</param>
        /// <returns>true, if resolutions are equal</returns>
        protected bool EqualVideoResolution(VideoResolution a, VideoResolution b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            return ((a.Height == b.Height) && (a.Width == b.Width));
        }

        /// <summary>
        /// Compares video rates
        /// </summary>
        /// <param name="a">Rate to be compared</param>
        /// <param name="b">Rate to be compared to</param>
        /// <returns>true, if rates are equal</returns>
        protected bool EqualVideoRateControl(VideoRateControl a, VideoRateControl b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            //this is according to agreement with ONVIF WG - only one parameter must match
            return (a.BitrateLimit == b.BitrateLimit)||(a.EncodingInterval == b.EncodingInterval)||(a.FrameRateLimit == b.FrameRateLimit);
        }

        /// <summary>
        /// Compares Mpeg4 configurations
        /// </summary>
        /// <param name="a">Configuration to be compared</param>
        /// <param name="b">Configuration to be compared to</param>
        /// <returns>true, if configurations are equal</returns>
        protected bool EqualMpeg4Configuration(Mpeg4Configuration a, Mpeg4Configuration b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            //this is according to agreement with ONVIF WG - Gov length can differ
            return /*(a.GovLength == b.GovLength)&&*/(a.Mpeg4Profile == b.Mpeg4Profile);
        }

        /// <summary>
        /// Compares H.264 configurations
        /// </summary>
        /// <param name="a">Configuration to be compared</param>
        /// <param name="b">Configuration to be compared to</param>
        /// <returns>true, if configurations are equal</returns>
        protected bool EqualH264Configuration(H264Configuration a, H264Configuration b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            //this is according to agreement with ONVIF WG - Gov length can differ
            return /*(a.GovLength == b.GovLength)&&*/(a.H264Profile == b.H264Profile);
        }

        /// <summary>
        /// Compares IP addresses
        /// </summary>
        /// <param name="a">Address to be compared</param>
        /// <param name="b">Address to be compared to</param>
        /// <returns>true, if addresses are equal</returns>
        protected bool EqualIPAddresses(IPAddress a, IPAddress b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            return (a.IPv4Address == b.IPv4Address) && (a.IPv6Address == a.IPv6Address) && (a.Type == b.Type);
        }
        
        /// <summary>
        /// Compares multicast configurations
        /// </summary>
        /// <param name="a">Configuration to be compared</param>
        /// <param name="b">Configuration to be compared to</param>
        /// <returns>true, if configurations are equal</returns>
        protected bool EqualMulticastConfiguration(MulticastConfiguration a, MulticastConfiguration b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }

            return EqualIPAddresses(a.Address, b.Address) && (a.AutoStart == b.AutoStart) && (a.Port == b.Port) && (a.TTL == b.TTL);
        }

        /// <summary>
        /// Compares video encoder configurations
        /// </summary>
        /// <param name="a">Configuration to be compared</param>
        /// <param name="b">Configuration to be compared to</param>
        /// <param name="reason">Reason why configurations are not equal</param>
        /// <returns>true, if configurations are equal</returns>
        protected bool EqualConfigurations(VideoEncoderConfiguration a, VideoEncoderConfiguration b, out string reason)
        {
            reason = null;
            if (a == b)
            {
                reason = null;
                return true;
            }
            if ((a == null) || (b == null))
            {
                reason = Resources.ErrorOneConfigIsEmpty_Text;
                return false;
            }
            if (a.Name != b.Name)
            {
                reason = Resources.ConfigsDiffNames_Text;
                return false;
            }
            if (a.token != b.token)
            {
                reason = Resources.ConfigsDiffTokens_Text;
                return false;
            }
            if (a.Encoding != b.Encoding)
            {
                reason = Resources.ConfigsDiffEncodings_Text;
                return false;
            }
            /*if (a.Quality != b.Quality)
            {
                reason = Resources.ConfigsDiffQualities_Text;
                return false;
            }*/
            /*if (a.SessionTimeout != b.SessionTimeout)
            {
                reason = Resources.ConfigsDiffSessionTimeouts_Text; 
                return false;
            }*/
            if ((a.Encoding == VideoEncoding.H264) && !EqualH264Configuration(a.H264, b.H264))
            {
                reason = Resources.ConfigsDiffH264Configs_Text;
                return false;
            }
            if ((a.Encoding == VideoEncoding.MPEG4) && !EqualMpeg4Configuration(a.MPEG4, b.MPEG4))
            {
                reason = Resources.ConfigsDiffMPEG4Configs_Text;
                return false;
            }
            //this is according to agreement with ONVIF WG - should not check multicast configuration
            /*if (!EqualMulticastConfiguration(a.Multicast, b.Multicast))
            {
                reason = Resources.ConfigsDiffMulticastConfigs_Text;
                return false;
            }*/
            //this is according to agreement with ONVIF WG - one of this parameters can differ
            /*if (!EqualVideoRateControl(a.RateControl, b.RateControl) && !EqualVideoResolution(a.Resolution, b.Resolution))
            {
                reason = Resources.ConfigsDiffVideoRateControls_Text + " " + Resources.ConfigsDiffResolutions_Text;
                return false;
            }*/
            if ((a.Quality != b.Quality) && !EqualVideoRateControl(a.RateControl, b.RateControl) && !EqualVideoResolution(a.Resolution, b.Resolution))
            {
                reason = Resources.ConfigsDiffQualities_Text + " " + 
                         Resources.ConfigsDiffVideoRateControls_Text + " " + 
                         Resources.ConfigsDiffResolutions_Text;
                return false;
            }
            return true;
        }

        protected bool ConfigurationValid(VideoEncoderConfiguration configuration, 
            VideoEncoding encoding, 
            VideoResolution resolution, 
            out string reason)
        {
            bool ok = true;
            StringBuilder dump = new StringBuilder(); 

            if (configuration.Encoding != encoding)
            {
                ok = false;
                dump.AppendFormat("Encoding incorrect: expected {0}, actual {1}{2}", 
                    encoding, configuration.Encoding, System.Environment.NewLine);
            }

            if (configuration.Resolution == null)
            {
                ok = false;
                dump.AppendLine("Resolution is missing");
            }
            else
            {
                if ((configuration.Resolution.Height != resolution.Height) || 
                    (configuration.Resolution.Width != resolution.Width))
                {
                    ok = false;
                    dump.AppendFormat("Resolution incorrect. Expected: width={0}, height={1}, actual: width={2}, height={3}{4}", 
                        resolution.Width, resolution.Height, configuration.Resolution.Width, configuration.Resolution.Height,
                        System.Environment.NewLine);
                }
            }

            reason = dump.ToStringTrimNewLine();
            return ok;
        }

        /// <summary>
        /// Compares arrays of xml elements
        /// </summary>
        /// <param name="a">Array to be compared</param>
        /// <param name="b">Array to be compared to</param>
        /// <returns>true, if arrays are equal</returns>
        protected bool EqualExtensions(XmlElement[] a, XmlElement[] b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            if(a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if(a[i].OuterXml != b[i].OuterXml)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares audio source configurations
        /// </summary>
        /// <param name="a">Configuration to be compared</param>
        /// <param name="b">Configuration to be compared to</param>
        /// <param name="reason">Reason why configurations are not equal</param>
        /// <returns>true, if configurations are equal</returns>
        protected bool EqualConfigurations(AudioSourceConfiguration a, AudioSourceConfiguration b, out string reason)
        {
            reason = null;
            if (a == b)
            {
                reason = null;
                return true;
            }
            if ((a == null) || (b == null))
            {
                reason = Resources.ErrorOneConfigIsEmpty_Text;
                return false;
            }
            if (a.Name != b.Name)
            {
                reason = Resources.ConfigsDiffNames_Text;
                return false;
            }
            if (a.token != b.token)
            {
                reason = Resources.ConfigsDiffTokens_Text;
                return false;
            }
            if (a.SourceToken != b.SourceToken)
            {
                reason = Resources.ConfigsDiffSourceTokens_Text;
                return false;
            }
            if (!EqualExtensions(a.Any, b.Any))
            {
                reason = Resources.ConfigsDiffExtensions_Text;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Compares audio encoder configurations
        /// </summary>
        /// <param name="a">Configuration to be compared</param>
        /// <param name="b">Configuration to be compared to</param>
        /// <param name="reason">Reason why configurations are not equal</param>
        /// <returns>true, if configurations are equal</returns>
        protected bool EqualConfigurations(AudioEncoderConfiguration a, AudioEncoderConfiguration b, out string reason)
        {
            reason = null;
            if (a == b)
            {
                reason = null;
                return true;
            }
            if ((a == null) || (b == null))
            {
                reason = Resources.ErrorOneConfigIsEmpty_Text;
                return false;
            }
            if (a.Name != b.Name)
            {
                reason = Resources.ConfigsDiffNames_Text;
                return false;
            } 
            if (a.token != b.token)
            {
                reason = Resources.ConfigsDiffTokens_Text;
                return false;
            }
            /*if (a.SessionTimeout != b.SessionTimeout)
            {
                reason = Resources.ConfigsDiffSessionTimeouts_Text;
                return false;
            }*/
            if (a.Encoding != b.Encoding)
            {
                reason = Resources.ConfigsDiffEncodings_Text;
                return false;
            }
            //this is according to agreement with ONVIF WG - should not check multicast configuration
            /*if (!EqualMulticastConfiguration(a.Multicast, b.Multicast))
            {
                reason = Resources.ConfigsDiffMulticastConfigs_Text;
                return false;
            }*/
            if (a.SampleRate != b.SampleRate)
            {
                reason = Resources.ConfigsDiffSampleRates_Text;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Compares subscription policies
        /// </summary>
        /// <param name="a">Policy to be compared</param>
        /// <param name="b">Policy to be compared to</param>
        /// <returns>true, if policies are equal</returns>
        protected bool EqualSubscriptionPolicies(EventSubscriptionSubscriptionPolicy a, EventSubscriptionSubscriptionPolicy b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            return EqualExtensions(a.Any, b.Any);
        }

        /// <summary>
        /// Compares filter types
        /// </summary>
        /// <param name="a">Type to be compared</param>
        /// <param name="b">Type to be compared to</param>
        /// <returns>true, if policies are equal</returns>
        protected bool EqualFilterTypes(FilterType a, FilterType b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            return EqualExtensions(a.Any, b.Any);
        }

        /// <summary>
        /// Compares event subscriptions
        /// </summary>
        /// <param name="a">Subscription to be compared</param>
        /// <param name="b">Subscription to be compared to</param>
        /// <returns>true, if subscriptions are equal</returns>
        protected bool EqualEventSubscriptions(EventSubscription a, EventSubscription b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            return EqualFilterTypes(a.Filter, b.Filter) && EqualSubscriptionPolicies(a.SubscriptionPolicy, b.SubscriptionPolicy);
        }

        /// <summary>
        /// Compares PTZ filters
        /// </summary>
        /// <param name="a">Filter to be compared</param>
        /// <param name="b">Filter to be compared to</param>
        /// <returns>true, if filters are equal</returns>
        protected bool EqualPTZFilters(PTZFilter a, PTZFilter b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            return (a.Position == b.Position)&&(a.Status == b.Status);
        }

        /// <summary>
        /// Compares metadata configurations
        /// </summary>
        /// <param name="a">Configuration to be compared</param>
        /// <param name="b">Configuration to be compared to</param>
        /// <param name="reason">Reason why configurations are not equal</param>
        /// <returns>true, if configurations are equal</returns>
        protected bool EqualConfigurations(MetadataConfiguration a, MetadataConfiguration b, out string reason)
        {
            reason = null;
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                reason = Resources.ErrorOneConfigIsEmpty_Text; 
                return false;
            }
            if (a.Name != b.Name)
            {
                reason = Resources.ConfigsDiffNames_Text;
                return false;
            }
            if (a.token != b.token) 
            {
                reason = Resources.ConfigsDiffTokens_Text;
                return false;
            }
            /*if (a.SessionTimeout != b.SessionTimeout)
            {
                reason = Resources.ConfigsDiffSessionTimeouts_Text;
                return false;
            }*/
            if (a.Analytics != b.Analytics)
            {
                reason = Resources.ConfigsDiffAnalytics_Text;
                return false;
            }
            if (a.AnalyticsSpecified != b.AnalyticsSpecified)
            {
                reason = Resources.ConfigsDiffAnalyticsSpecifieds_Text;
                return false;
            }
            /*if (!EqualMulticastConfiguration(a.Multicast, b.Multicast))
            {
                reason = Resources.ConfigsDiffMulticastConfigs_Text;
                return false;
            }*/
            if (!EqualEventSubscriptions(a.Events, b.Events))
            {
                reason = Resources.ConfigsDiffEventSubscriptions_Text;
                return false;
            }
            if (!EqualPTZFilters(a.PTZStatus, b.PTZStatus))
            {
                reason = Resources.ConfigsDiffPTZStatuses_Text;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates video encoder configuration options
        /// </summary>
        /// <param name="options">Options to be validated</param>
        /// <param name="reason">Reason why options are invalid</param>
        /// <returns>true, if options are valid</returns>
        protected bool ValidateVideoEncoderConfigOptions(VideoEncoderConfigurationOptions options, out string reason)
        {
            reason = null;
            if((options.JPEG == null)||
                (options.JPEG.EncodingIntervalRange == null)||
                (options.JPEG.FrameRateRange == null)||
                (options.JPEG.ResolutionsAvailable == null)||
                (options.JPEG.ResolutionsAvailable.Length == 0))
            {
                reason = Resources.ErrorNoValidJpegOptions_Text;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns media stream uri from DUT
        /// </summary>
        /// <param name="streamSetup">Stream step parameters</param>
        /// <param name="profileToken">Token of media profile</param>
        /// <returns>Media stream uri</returns>
        protected MediaUri GetStreamUri(StreamSetup streamSetup, string profileToken)
        {
            MediaUri uri = null;
            RunStep(() =>
            {
                uri = Client.GetStreamUri(streamSetup, profileToken); 
                LogStepEvent("Stream URI = " + uri.Uri);
            }, "Get Stream URI");
            DoRequestDelay();
            return uri;
        }

        /// <summary>
        /// Returns snapshot uri from DUT
        /// </summary>
        /// <param name="getSnapshotUri">Get snapshot parameters</param>
        /// <returns>Snapshot uri</returns>
        protected MediaUri GetSnapshotUri(string profileToken)
        {
            MediaUri response = null;
            RunStep(() =>
            {
                response = Client.GetSnapshotUri(profileToken);
            },
            "Get snapshot URI");
            DoRequestDelay();
            return response;
        }

        /// <summary>
        /// Returns video encoder configuration options for specified profile and configuration
        /// </summary>
        /// <param name="profileToken">token of profile, can be null</param>
        /// <param name="configurationToken">token of configuration</param>
        /// <returns>Configuration options</returns>
        protected VideoEncoderConfigurationOptions GetVideoEncoderConfigurationOptions(string configurationToken, 
            string profileToken)
        {
            VideoEncoderConfigurationOptions options = null;
            RunStep(() =>
            {
                options = Client.GetVideoEncoderConfigurationOptions(configurationToken, profileToken);
            },
            "Get video encoder configuration options");
            DoRequestDelay();
            return options;
        }
        /// <summary>
        /// Returns guaranteed number of video encoder instances for specified configuration
        /// </summary>
        /// <param name="configurationToken">token of configuration</param>
        /// <param name="JPEG"></param>
        /// <param name="MPEG4"></param>
        /// <param name="H264"></param>
        protected int GetGuaranteedNumberOfVideoEncoderInstances(
            string configurationToken,
            out int JPEG, 
            out int H264, 
            out int MPEG4)
        {
            int totalNumber = -1;
            int jpeg = -1;
            int h264 = -1;
            int mpeg4 = -1;
            RunStep(() =>
            {
                totalNumber = Client.GetGuaranteedNumberOfVideoEncoderInstances(out jpeg, out h264, out mpeg4, configurationToken);
            },
            "Getting guaranteed number of video encoder instances");
            JPEG = jpeg;
            H264 = h264;
            MPEG4 = mpeg4;

            DoRequestDelay();
            return totalNumber;
        }
        
        /// <summary>
        /// Gets audio output configurations.
        /// </summary>
        /// <returns></returns>
        protected AudioOutputConfiguration[] GetAudioOutputConfigurations()
        {
            return GetAudioOutputConfigurations("Get audio output configurations");
        }

        /// <summary>
        /// Gets audio outputs configurations.
        /// </summary>
        /// <param name="stepName"></param>
        /// <returns></returns>
        protected AudioOutputConfiguration[] GetAudioOutputConfigurations(string stepName)
        {
            AudioOutputConfiguration[] audioOutputConfigurations = null;
            RunStep(() => { audioOutputConfigurations = Client.GetAudioOutputConfigurations() ; }, stepName);
            DoRequestDelay();
            return audioOutputConfigurations;
        }

        /// <summary>
        /// Gets single audio output configuration.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected AudioOutputConfiguration GetAudioOutputConfiguration(string token)
        {
            return GetAudioOutputConfiguration(token, "Get audio output configuration");
        }

        /// <summary>
        /// Gets single audio output configuration.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="stepName"></param>
        /// <returns></returns>
        protected AudioOutputConfiguration GetAudioOutputConfiguration (string token, string stepName)
        {
            AudioOutputConfiguration audioOutputConfiguration = null;
            RunStep(() => { audioOutputConfiguration = Client.GetAudioOutputConfiguration(token); }, stepName);
            DoRequestDelay();
            return audioOutputConfiguration;
        }

        /// <summary>
        /// Gets audio output configuration options.
        /// </summary>
        /// <param name="configurationToken"></param>
        /// <param name="profileToken"></param>
        /// <returns></returns>
        protected AudioOutputConfigurationOptions GetAudioOutputConfigurationOptions(string configurationToken,
            string profileToken)
        {
            return GetAudioOutputConfigurationOptions(configurationToken, profileToken,
                                                      "Get audio output configuration options");
        }

        /// <summary>
        /// Gets audio output configuration options.
        /// </summary>
        /// <param name="configurationToken"></param>
        /// <param name="profileToken"></param>
        /// <param name="stepName"></param>
        /// <returns></returns>
        protected AudioOutputConfigurationOptions GetAudioOutputConfigurationOptions(string configurationToken, 
            string profileToken, 
            string stepName)
        {
            AudioOutputConfigurationOptions options = null;
            RunStep(() => { options = Client.GetAudioOutputConfigurationOptions(configurationToken, profileToken); }, stepName);
            DoRequestDelay();
            return options;
        }

        /// <summary>
        /// Sets audio output configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="forcePersistance"></param>
        protected void SetAudioOutputConfiguration(AudioOutputConfiguration configuration,
            bool forcePersistance)
        {
            SetAudioOutputConfiguration(configuration, forcePersistance, "Set audio output configuration");
        }

        protected void SetAudioOutputConfiguration(AudioOutputConfiguration configuration, 
            bool forcePersistance, 
            string stepName)
        {
            RunStep(() => { Client.SetAudioOutputConfiguration(configuration, forcePersistance);}, stepName);
            DoRequestDelay();
        }

        protected void AddVideoAnalyticsConfiguration(string profile, string configuration)
        {
            RunStep(() => { Client.AddVideoAnalyticsConfiguration(profile, configuration); },
                string.Format("Adding video analytics configuration [token = '{0}'] to profile [token = '{1}']", configuration, profile));
            DoRequestDelay();
        }

        protected Profile GetProfile(string profileToken, string stepName)
        {
            Profile profile = null;
            RunStep(() => { profile = Client.GetProfile(profileToken); }, stepName);
            DoRequestDelay();
            return profile;
        }

        protected TestTool.Proxies.Onvif.PTZConfiguration[] GetPtzConfigurations()
        {
            TestTool.Proxies.Onvif.PTZClient client = PtzClient;
            TestTool.Proxies.Onvif.PTZConfiguration[] result = null;
            RunStep(() => { result = client.GetConfigurations(); }, "Get PTZ configurations");
            DoRequestDelay();
            return result;

        }

    }
}
