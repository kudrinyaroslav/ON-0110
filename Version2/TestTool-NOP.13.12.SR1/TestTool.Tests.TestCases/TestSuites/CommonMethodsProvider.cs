using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.TestSuites;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.Tests.TestCases 
{
    /// <summary>
    /// An attempt to get rid of copying common methods (like GetProfiles from Media to Recording
    /// or GetServices from DeviceManagement to different "Capabilities" tests);
    /// </summary>
    class CommonMethodsProvider : BaseOnvifTest
    {
        private CommonMethodsProvider(TestLaunchParam param)
            :base(param)
        {
        }

        public static Service[] GetServices(BaseOnvifTest test, DeviceClient client, bool includeCapabilities)
        {
            Service[] services = null;
            RunStep(test, () => { services = client.GetServices(includeCapabilities); }, "Get Services");
            DoRequestDelay(test);
            return services;
        }

        public static Capabilities GetCapabilities(BaseOnvifTest test, DeviceClient client, CapabilityCategory[] categories)
        {
            Capabilities capabilities = null;
            RunStep(test, () => { capabilities = client.GetCapabilities(categories); }, "Get capabilities");
            DoRequestDelay(test);
            return capabilities;
        }

        #region Media service methods

        public static Profile[] GetProfiles(BaseOnvifTest test, MediaClient client)
        {
            Profile[] profiles = null;
            RunStep(test, () => { profiles = client.GetProfiles(); }, "Get Media profiles");
            DoRequestDelay(test);
            return profiles;
        }


        public static Profile GetProfile(BaseOnvifTest test, MediaClient client, string token)
        {
            string stepName = string.Format("Get Media profile '{0}'", token);
            return GetProfile(test, client, token, stepName);
        }

        public static Profile GetProfile(BaseOnvifTest test, MediaClient client, string token, string stepName)
        {
            Profile profile = null;
            RunStep(test, () => { profile = client.GetProfile(token); }, stepName);
            DoRequestDelay(test);
            return profile;
        }

        /// <summary>
        /// Retrieves lists of video encoder configurations compatible with specified profile from DUT
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <returns>Array of video encoder configurations</returns>
        public static VideoEncoderConfiguration[] GetCompatibleVideoEncoderConfigurations(
            BaseOnvifTest test, MediaClient client, string profile)
        {
            VideoEncoderConfiguration[] configs = null;
            RunStep(test, () => { configs = client.GetCompatibleVideoEncoderConfigurations(profile); },
                string.Format(Resources.StepGetCompatibleVideoEncoderConfigs_Format, profile));
            DoRequestDelay(test);
            return configs;
        }

        public static VideoSourceConfiguration[] GetCompatibleVideoSourceConfigurations(
            BaseOnvifTest test, MediaClient client, string profile)
        {
            VideoSourceConfiguration[] configs = null;
            RunStep(test, () => { configs = client.GetCompatibleVideoSourceConfigurations(profile); },
                string.Format(Resources.StepGetCompatibleVideoSourceConfigs_Format, profile));
            DoRequestDelay(test);
            return configs;
        }


        /// <summary>
        /// Adds video encoder configuration to profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <param name="configuration">Token of configuration</param>
        public static void AddVideoEncoderConfiguration(BaseOnvifTest test, MediaClient client, string profile, string configuration)
        {
            RunStep(test, () => { client.AddVideoEncoderConfiguration(profile, configuration); },
                string.Format(Resources.StepAddVideoEncoderConfig_Format, configuration, profile));
            DoRequestDelay(test);
        }

        public static void AddVideoSourceConfiguration(BaseOnvifTest test, MediaClient client, string profile, string configuration)
        {
            RunStep(test, () => { client.AddVideoSourceConfiguration(profile, configuration); },
                string.Format(Resources.StepAddVideoSourceConfig_Format, configuration, profile));
            DoRequestDelay(test);
        }

        public static void AddAudioEncoderConfiguration(BaseOnvifTest test, MediaClient client, string profile, string configuration)
        {
            RunStep(test, () => { client.AddAudioEncoderConfiguration(profile, configuration); },
                string.Format(Resources.StepAddAudioEncoderConfig_Format, configuration, profile));
            DoRequestDelay(test);
        }
        
        public static void AddAudioSourceConfiguration(BaseOnvifTest test, MediaClient client, string profile, string configuration)
        {
            RunStep(test, () => { client.AddAudioSourceConfiguration(profile, configuration); },
                string.Format(Resources.StepAddAudioSourceConfig_Format, configuration, profile));
            DoRequestDelay(test);
        }

        public static void AddMetadataConfiguration(BaseOnvifTest test, MediaClient client, string profile, string configuration)
        {
            RunStep(test, () => { client.AddMetadataConfiguration(profile, configuration); },
                string.Format(Resources.StepAddMetadataConfig_Format, configuration, profile));
            DoRequestDelay(test);
        }

        public static void AddPTZConfiguration(BaseOnvifTest test, MediaClient client, string profile, string configuration)
        {
            RunStep(test, () => { client.AddPTZConfiguration(profile, configuration); },
                string.Format(Resources.StepAddPTZConfig_Format, configuration, profile));
            DoRequestDelay(test);
        }

        public static void AddVideoAnalyticsConfiguration(BaseOnvifTest test, MediaClient client, string profile, string configuration)
        {
            RunStep(test, () => { client.AddVideoAnalyticsConfiguration(profile, configuration); },
                string.Format("Add Video Analytics Configuration {0} to profile {1}", configuration, profile));
            DoRequestDelay(test);
        }
        
        public static void SetVideoEncoderConfiguration(BaseOnvifTest test, MediaClient client, VideoEncoderConfiguration configuration, bool persistency)
        {
            RunStep(test, () => { client.SetVideoEncoderConfiguration(configuration, persistency); },
                Resources.StepSetVideoEncoderConfig_Title);
            DoRequestDelay(test);
        }

        public static void SetAudioEncoderConfiguration(BaseOnvifTest test, MediaClient client, AudioEncoderConfiguration configuration, bool persistency)
        {
            RunStep(test, () => { client.SetAudioEncoderConfiguration(configuration, persistency); },
                Resources.StepSetAudioEncoderConfig_Title);
            DoRequestDelay(test);
        }

        public static void SetMetadataConfiguration(BaseOnvifTest test, MediaClient client, MetadataConfiguration configuration, bool persistency)
        {
            RunStep(test, () => { client.SetMetadataConfiguration(configuration, persistency); },
                Resources.StepSetMetadataConfig_Title);
            DoRequestDelay(test);
        }

        /// <summary>
        /// Removes video encoder configuration from profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        public static void RemoveVideoEncoderConfiguration(BaseOnvifTest test, MediaClient client, string profile)
        {
            RunStep(test, () => { client.RemoveVideoEncoderConfiguration(profile); },
                string.Format(Resources.StepRemoveVideoEncoderConfig_Format, profile));
            DoRequestDelay(test);
        }

        public static void RemoveVideoSourceConfiguration(BaseOnvifTest test, MediaClient client, string profile)
        {
            RunStep(test, () => { client.RemoveVideoSourceConfiguration(profile); },
                string.Format(Resources.StepRemoveVideoSourceConfig_Format, profile));
            DoRequestDelay(test);
        }

        public static void RemoveAudioEncoderConfiguration(BaseOnvifTest test, MediaClient client, string profile)
        {
            RunStep(test, () => { client.RemoveAudioEncoderConfiguration(profile); },
                string.Format(Resources.StepRemoveAudioEncoderConfig_Format, profile));
            DoRequestDelay(test);
        }

        public static void RemoveAudioSourceConfiguration(BaseOnvifTest test, MediaClient client, string profile)
        {
            RunStep(test, () => { client.RemoveAudioSourceConfiguration(profile); },
                string.Format(Resources.StepRemoveAudioSourceConfig_Format, profile));
            DoRequestDelay(test);
        }


        public static void RemoveMetadataConfiguration(BaseOnvifTest test, MediaClient client, string profile)
        {
            RunStep(test, () => { client.RemoveMetadataConfiguration(profile); },
                string.Format(Resources.StepRemoveMetadataConfig_Format, profile));
            DoRequestDelay(test);
        }

        public static void RemovePTZConfiguration(BaseOnvifTest test, MediaClient client, string profile)
        {
            RunStep(test, () => { client.RemovePTZConfiguration(profile); },
                string.Format(Resources.StepRemovePTZConfig_Format, profile));
            DoRequestDelay(test);
        }

        public static void RemoveVideoAnalyticsConfiguration(BaseOnvifTest test, MediaClient client, string profile)
        {
            RunStep(test, () => { client.RemoveVideoAnalyticsConfiguration(profile); },
                string.Format("Remove Video Analytics Configuration from profile {0}", profile));
            DoRequestDelay(test);
        }


        public static VideoEncoderConfigurationOptions GetVideoEncoderConfigurationOptions(
             BaseOnvifTest test, MediaClient client, string configuration, string profile)
        {
            VideoEncoderConfigurationOptions options = null;
            RunStep(test, () => { options = client.GetVideoEncoderConfigurationOptions(configuration, profile); },
                Resources.StepGetVideoEncoderConfigOptions_Title);
            DoRequestDelay(test);
            return options;
        }

        public static void DeleteProfile(BaseOnvifTest test, MediaClient client, string profile)
        {
            RunStep(test, () => { client.DeleteProfile (profile); },
                string.Format(Resources.StepDeleteMediaProfile_Format, profile));
            DoRequestDelay(test);
        }

        public static Profile CreateProfile(BaseOnvifTest test, MediaClient client, string name, string token)
        {
            Profile profile = null;
            RunStep(test, () => { profile = client.CreateProfile(name, token); },
                string.Format(Resources.StepCreateMediaProfile_Format, name));
            DoRequestDelay(test);
            return profile;
        }

        #endregion

        #region Media configuration

        public static void RemoveAllConfigurations(BaseOnvifTest test, MediaClient client, Profile profile)
        {
            if (profile.VideoEncoderConfiguration != null)
            {
                RemoveVideoEncoderConfiguration(test, client, profile.token);
            }
            if (profile.VideoSourceConfiguration != null)
            {
                RemoveVideoSourceConfiguration(test, client, profile.token);
            }
            if (profile.AudioEncoderConfiguration != null)
            {
                RemoveAudioEncoderConfiguration(test, client, profile.token);
            }
            if (profile.VideoSourceConfiguration != null)
            {
                RemoveAudioSourceConfiguration(test, client, profile.token);
            }

            if (profile.MetadataConfiguration != null)
            {
                RemoveMetadataConfiguration(test, client, profile.token);
            }
            if (profile.PTZConfiguration != null)
            {
                RemovePTZConfiguration(test, client, profile.token);
            } 
            if (profile.VideoAnalyticsConfiguration != null)
            {
                RemoveVideoAnalyticsConfiguration(test, client, profile.token);
            }

        }

        public static void RollbackMediaConfiguration(BaseOnvifTest test, MediaClient client, MediaConfigurationChangeLog changeLog)
        {

            foreach (Profile p in changeLog.CreatedProfiles)
            {
                DeleteProfile(test, client, p.token);
            }
            foreach (Profile p in changeLog.DeletedProfiles)
            {
                RestoreDeletedProfile(test, client, p);
            }
            foreach (Profile p in changeLog.ModifiedProfiles)
            {
                ResetProfile(test, client, p);
            }

            foreach (VideoEncoderConfiguration config in changeLog.ModifiedVideoEncoderConfigurations)
            {
                SetVideoEncoderConfiguration(test, client, config, true);
            }
            foreach (AudioEncoderConfiguration config in changeLog.ModifiedAudioEncoderConfigurations)
            {
                SetAudioEncoderConfiguration(test, client, config, false);
            }
            foreach (MetadataConfiguration config in changeLog.ModifiedMetadataConfigurations)
            {
                SetMetadataConfiguration(test, client, config, false);
            }        
        }
        
        protected static void ResetProfile(BaseOnvifTest test, MediaClient client, Profile profile)
        {
            LogTestEvent(test, string.Format("Restore profile '{0}' used for test{1}", profile.token, Environment.NewLine));

            Profile actual = GetProfile(test, client, profile.token, "Get actual profile");

            if (profile.VideoEncoderConfiguration == null)
            {
                if (actual.VideoEncoderConfiguration != null)
                {
                    RemoveVideoEncoderConfiguration(test, client, profile.token);
                }
            }

            if (profile.AudioEncoderConfiguration == null)
            {
                if (actual.AudioEncoderConfiguration != null)
                {
                    RemoveAudioEncoderConfiguration(test, client, profile.token);
                }
            }

            if (profile.VideoSourceConfiguration != null)
            {
                if (actual.VideoSourceConfiguration == null ||
                    actual.VideoSourceConfiguration.token != profile.VideoSourceConfiguration.token)
                {
                    AddVideoSourceConfiguration(test, client, profile.token, profile.VideoSourceConfiguration.token);
                }
            }
            else
            {
                if (actual.VideoSourceConfiguration != null)
                {
                    RemoveVideoSourceConfiguration(test, client, profile.token);
                }
            }

            if (profile.AudioSourceConfiguration != null)
            {
                if (actual.AudioSourceConfiguration == null ||
                    actual.AudioSourceConfiguration.token != profile.AudioSourceConfiguration.token)
                {
                    AddAudioSourceConfiguration(test, client, profile.token, profile.AudioSourceConfiguration.token);
                }
            }
            else
            {
                if (actual.AudioSourceConfiguration != null)
                {
                    RemoveAudioSourceConfiguration(test, client, profile.token);
                }
            }

            if (profile.VideoEncoderConfiguration != null)
            {
                if (actual.VideoEncoderConfiguration == null ||
                    actual.VideoEncoderConfiguration.token != profile.VideoEncoderConfiguration.token)
                {
                    AddVideoEncoderConfiguration(test, client, profile.token, profile.VideoEncoderConfiguration.token);
                }
            }

            if (profile.AudioEncoderConfiguration != null)
            {
                if (actual.AudioEncoderConfiguration == null ||
                    actual.AudioEncoderConfiguration.token != profile.AudioEncoderConfiguration.token)
                {
                    AddAudioEncoderConfiguration(test, client, profile.token, profile.AudioEncoderConfiguration.token);
                }
            }

        }

        protected static void RestoreDeletedProfile(BaseOnvifTest test, MediaClient client, Profile profile)
        {
            LogTestEvent(test, string.Format("Restore profile '{0}' deleted during the test{1}", profile.token, Environment.NewLine));

            Profile actual = CreateProfile(test, client, profile.Name, profile.token);

            if (profile.VideoSourceConfiguration != null)
            {
                AddVideoSourceConfiguration(test, client, profile.token, profile.VideoSourceConfiguration.token);
            }
            if (profile.AudioSourceConfiguration != null)
            {
                AddAudioSourceConfiguration(test, client, profile.token, profile.AudioSourceConfiguration.token);
            }
            if (profile.VideoEncoderConfiguration != null)
            {
                AddVideoEncoderConfiguration(test, client, profile.token, profile.VideoEncoderConfiguration.token);
            }
            if (profile.AudioEncoderConfiguration != null)
            {
                AddAudioEncoderConfiguration(test, client, profile.token, profile.AudioEncoderConfiguration.token);
            }
            if (profile.MetadataConfiguration != null)
            {
                AddMetadataConfiguration(test, client, profile.token, profile.MetadataConfiguration.token);
            }
            if (profile.PTZConfiguration != null)
            {
                AddPTZConfiguration(test, client, profile.token, profile.PTZConfiguration.token);
            }
            if (profile.VideoAnalyticsConfiguration != null)
            {
                AddVideoAnalyticsConfiguration(test, client, profile.token, profile.VideoAnalyticsConfiguration.token);
            }
        }
        
        #endregion

        #region PTZ Service methods

        public static PTZConfiguration[] GetPtzConfigurations(BaseOnvifTest test, PTZClient client)
        {
            PTZConfiguration[] result = null;
            RunStep(test, () => { result = client.GetConfigurations(); }, "Get PTZ configurations");
            DoRequestDelay(test);
            return result;
        }

        #endregion

        #region Receiver service methods

        public static Receiver GetReceiver(BaseOnvifTest test, ReceiverPortClient client, string receiverToken)
        {
            Receiver receiver = null;
            RunStep(test, () => { receiver = client.GetReceiver(receiverToken); },
                string.Format("Get Receiver {0}", receiverToken));
            DoRequestDelay(test);
            return receiver;
        }

        public static Receiver[] GetReceivers(BaseOnvifTest test, ReceiverPortClient client)
        {
            Receiver[] items = null;
            RunStep(test, () => { items = client.GetReceivers(); }, "Get Receivers");
            DoRequestDelay(test);
            return items;
        }

        public static void SetReceiverMode(BaseOnvifTest test, ReceiverPortClient client, string receiverToken, ReceiverMode mode)
        {
            RunStep(test, () => { client.SetReceiverMode(receiverToken, mode); },
                string.Format("Set Receiver Mode (receiverToken = '{0}') to '{1}'", receiverToken, mode));
            DoRequestDelay(test);
        }        
        
        public static void DeleteReceiver(BaseOnvifTest test, ReceiverPortClient client, string receiverToken)
        {
            RunStep(test, () => client.DeleteReceiver(receiverToken),
                string.Format("Delete Receiver (token = '{0}')", receiverToken));
            DoRequestDelay(test);
        }

        public static ReceiverServiceCapabilities GetReceiverServiceCapabilities(BaseOnvifTest test, ReceiverPortClient client)
        {
            ReceiverServiceCapabilities capabilities = null;
            RunStep(test, () => { capabilities = client.GetServiceCapabilities(); }, "Get Receiver Service Capabilities");
            DoRequestDelay(test);
            return capabilities;
        }

        public static ReceiverStateInformation GetReceiverState(BaseOnvifTest test, ReceiverPortClient client,  string receiverToken)
        {
            ReceiverStateInformation receiverSI = null;
            RunStep(test, () => { receiverSI = client.GetReceiverState(receiverToken); },
                string.Format("Get Receiver {0} State", receiverToken));
            DoRequestDelay(test );
            return receiverSI;
        }

        public static void ConfigureReceiver(BaseOnvifTest test, ReceiverPortClient client, string receiverToken, ReceiverConfiguration config)
        {
            RunStep(test, () => client.ConfigureReceiver(receiverToken, config),
                string.Format("Configure Receiver (token = '{0}')", receiverToken));
            DoRequestDelay(test);
        }

        public static Receiver CreateReceiver(BaseOnvifTest test, ReceiverPortClient client, ReceiverConfiguration config)
        {
            Receiver receiver = null;
            RunStep(test, () => { receiver = client.CreateReceiver(config); }, "Create Receiver");
            DoRequestDelay(test);
            return receiver;
        }

        #endregion

        #region RecordingPortClient methods

        public static GetRecordingsResponseItem[] GetRecordings(BaseOnvifTest test, RecordingPortClient client)
        {
            GetRecordingsResponseItem[] recordingsResponseItems = null;
            RunStep(test, 
                () => { recordingsResponseItems = client.GetRecordings(); },
                "Get recordings");
            DoRequestDelay(test);
            return recordingsResponseItems;
        }

        #endregion


    }
}
