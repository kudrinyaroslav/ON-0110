using System;
using System.Collections.Generic;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Attributes;
using System.Text;
using System.Linq;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.Tests.TestCases.Profiles
{


    [ProfileDefinition("Profile S", "onvif://www.onvif.org/Profile/Streaming", ProfileVersionStatus.Release)]
    public class BasicStreamingProfile : BaseProfile, IProfileDefinition
    {
        public BasicStreamingProfile()
        {
            _mandatoryScopes = new List<String>();
            _mandatoryScopes.AddRange(new [] { this.GetProfileScope() });
            InitFeatures();
            InitDiscoveryTypes();
        }

        public IEnumerable<Feature> MandatoryDiscoveryTypes { get; private set; }

        //public string Name
        //{
        //    get { return "Profile S"; }
        //}
        
        //public ProfileVersionStatus Status
        //{
        //    get { return ProfileVersionStatus.Release; }
        //}

        #region Features

        private List<String> _mandatoryScopes;

        #endregion

        #region Functionality

        private static string GetProfileFeaturesPath(string path)
        {
            return GetFullPath("Profile Features", path);
        }

        private static string GetCoreFullPath(string path)
        {
            return GetFullPath("Device Mandatory Features", path);
        }

        private const string DISCOVERY = "Discovery";
        private const string NETWORKCONFIGURATION = "Network Configuration";
        private const string SYSTEM = "System";
        private const string USERHANDLING = "User Handling";
        private const string EVENT = "Event handling";
        private const string MEDIAPROFILECONFIGURATION = "Media Profile Configuration";
        private const string VIDEOSOURCECONFIGURATION = "Video Source Configuration";
        private const string METADATACONFIGURATION = "Metadata Configuration";

        private const string SECURITY = "User Authentication";
        private const string MEDIASTREAMING = "Media Streaming";
        private const string VIDEOENCODERCONFIGURATION = "Video Encoder Configuration";
        private const string MEDIASTREAMINGMULTICAST = "Media Streaming - Multicast";
        private const string PTZ = "PTZ";
        private const string PTZABSOLUTE = "PTZ – Absolute Positioning";
        private const string PTZRELATIVE = "PTZ – Relative Positioning";
        private const string PTZPRESETS = "PTZ - Presets";
        private const string PTZHOME = "PTZ - Home Position";
        private const string PTZAUXILLIARY = "PTZ - Auxiliary Commands";
        private const string AUDIOSTREAMING = "Audio Streaming";
        private const string RELAYOUTPUTS = "Relay Outputs";
        private const string CAPABILITIES = "Capabilities";
        private const string NTP = "NTP";
        private const string DYNAMICIP = "Dynamic DNS";
        private const string ZEROCONFIGURATION = "Zero Configuration";
        private const string IPFILTERING = "IP Address Filtering";

        protected List<FunctionalityItem> LoadProfileFunctionalities()
        {
            if (_profileFunctionalities == null)
            {
                _profileFunctionalities = new List<FunctionalityItem>();

                _profileFunctionalities.AddRange(
                    new FunctionalityItem[]
                        {
                            // Media Streaming
                            new FunctionalityItem(){Functionality = Functionality.GetStreamUri, Path = GetProfileFeaturesPath(MEDIASTREAMING), Features = new Feature[]{Feature.MediaService, Feature.RTSS}},
                            new FunctionalityItem(){Functionality = Functionality.MediaSetSynchronizationPoint, Path = GetProfileFeaturesPath(MEDIASTREAMING), Features = new Feature[]{Feature.MediaService, Feature.RTSS, Feature.H264OrMPEG4}},
                            new FunctionalityItem(){Functionality = Functionality.MediaStreamingRtsp, Path = GetProfileFeaturesPath(MEDIASTREAMING), Features = new Feature[]{Feature.MediaService, Feature.RTSS}},
                            new FunctionalityItem(){Functionality = Functionality.MediaStreamingRtspJpegHeaderExtension, Path = GetProfileFeaturesPath(MEDIASTREAMING), Features = new Feature[]{Feature.MediaService, Feature.RTSS}},
                            // Video Encoder Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetVideoEncoderConfiguration, Path = GetProfileFeaturesPath(VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetVideoEncoderConfigurations, Path = GetProfileFeaturesPath(VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.AddVideoEncoderConfiguration, Path = GetProfileFeaturesPath(VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveVideoEncoderConfiguration, Path = GetProfileFeaturesPath(VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.SetVideoEncoderConfiguration, Path = GetProfileFeaturesPath(VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleVideoEncoderConfigurations, Path = GetProfileFeaturesPath(VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetVideoEncoderConfigurationOptions, Path = GetProfileFeaturesPath(VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetGuaranteedNumberOfVideoEncoderInstances, Path = GetProfileFeaturesPath(VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            //User Authentication
                            new FunctionalityItem(){Functionality = Functionality.WsSecurity, Path = GetProfileFeaturesPath(SECURITY), Features = new Feature[]{Feature.WSU}},
                            new FunctionalityItem(){Functionality = Functionality.DigestAuthentication, Path = GetProfileFeaturesPath(SECURITY), Features = new Feature[]{Feature.Digest}},
                            // Capabilities
                            new FunctionalityItem(){Functionality = Functionality.GetCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[] {Feature.GetCapabilities}},
                            new FunctionalityItem(){Functionality = Functionality.GetWsdlUrl, Path = GetProfileFeaturesPath(CAPABILITIES)},
                            // PTZ
                            new FunctionalityItem(){Functionality = Functionality.AddPTZConfiguration, Path = GetProfileFeaturesPath(PTZ), Features = new Feature[]{Feature.MediaService, Feature.PTZService}},
                            new FunctionalityItem(){Functionality = Functionality.RemovePTZConfiguration, Path = GetProfileFeaturesPath(PTZ), Features = new Feature[]{Feature.MediaService, Feature.PTZService}},
                            new FunctionalityItem(){Functionality = Functionality.PtzGetNodes, Path = GetProfileFeaturesPath(PTZ), Features = new Feature[]{Feature.PTZService}},
                            new FunctionalityItem(){Functionality = Functionality.PtzGetNode, Path = GetProfileFeaturesPath(PTZ), Features = new Feature[]{Feature.PTZService}},
                            new FunctionalityItem(){Functionality = Functionality.GetPtzConfigurations, Path = GetProfileFeaturesPath(PTZ), Features = new Feature[]{Feature.PTZService}},
                            new FunctionalityItem(){Functionality = Functionality.GetPtzConfiguration, Path = GetProfileFeaturesPath(PTZ), Features = new Feature[]{Feature.PTZService}},
                            new FunctionalityItem(){Functionality = Functionality.GetPtzConfigurationOptions, Path = GetProfileFeaturesPath(PTZ), Features = new Feature[]{Feature.PTZService}},
                            new FunctionalityItem(){Functionality = Functionality.SetPtzConfiguration, Path = GetProfileFeaturesPath(PTZ), Features = new Feature[]{Feature.PTZService}},
                            new FunctionalityItem(){Functionality = Functionality.PtzContinuousMove, Path = GetProfileFeaturesPath(PTZ), Features = new Feature[]{Feature.PTZService}},
                            new FunctionalityItem(){Functionality = Functionality.PtzStop, Path = GetProfileFeaturesPath(PTZ), Features = new Feature[]{Feature.PTZService}},
                            new FunctionalityItem(){Functionality = Functionality.PtzGetStatus, Path = GetProfileFeaturesPath(PTZ), Features = new Feature[]{Feature.PTZService}},
                            // PTZ Absolute
                            new FunctionalityItem(){Functionality = Functionality.PtzAbsoluteMove, Path = GetProfileFeaturesPath(PTZABSOLUTE), Features = new Feature[]{Feature.PTZService, Feature.PTZAbsolute}},
                            // PTZ Relative
                            new FunctionalityItem(){Functionality = Functionality.PtzRelativeMove, Path = GetProfileFeaturesPath(PTZRELATIVE), Features = new Feature[]{Feature.PTZService, Feature.PTZRelative}},
                            // PTZ - Presets
                            new FunctionalityItem(){Functionality = Functionality.PtzSetPreset, Path = GetProfileFeaturesPath(PTZPRESETS), Features = new Feature[]{Feature.PTZService, Feature.PTZPresets}},
                            new FunctionalityItem(){Functionality = Functionality.PtzGetPreset, Path = GetProfileFeaturesPath(PTZPRESETS), Features = new Feature[]{Feature.PTZService, Feature.PTZPresets}},
                            new FunctionalityItem(){Functionality = Functionality.PtzGotoPreset, Path = GetProfileFeaturesPath(PTZPRESETS), Features = new Feature[]{Feature.PTZService, Feature.PTZPresets}},
                            new FunctionalityItem(){Functionality = Functionality.PtzRemovePreset, Path = GetProfileFeaturesPath(PTZPRESETS), Features = new Feature[]{Feature.PTZService, Feature.PTZPresets}},
                            // PTZ- HomePosition
                            new FunctionalityItem(){Functionality = Functionality.GotoHomePosition, Path = GetProfileFeaturesPath(PTZHOME), Features = new Feature[]{Feature.PTZService, Feature.PTZHome}},
                            new FunctionalityItem(){Functionality = Functionality.SetHomePosition, Path = GetProfileFeaturesPath(PTZHOME), Features = new Feature[]{Feature.PTZService, Feature.PTZHome, Feature.PTZConfigurableHome}},
                            // PTZ Auxilliary
                            new FunctionalityItem(){Functionality = Functionality.SendAuxiliaryCommand, Path = GetProfileFeaturesPath(PTZAUXILLIARY), Features = new Feature[]{Feature.PTZService, Feature.PTZAuxiliary}},
                            // Audio Streaming
                            new FunctionalityItem(){Functionality = Functionality.GetAudioSources, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioSourceConfiguration, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioSourceConfigurations, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.AddAudioSourceConfiguration, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveAudioSourceConfiguration, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.SetAudioSourceConfiguration, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleAudioSourceConfigurations, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioSourceConfigurationOptions, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioEncoderConfiguration, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioEncoderConfigurations, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.AddAudioEncoderConfiguration, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveAudioEncoderConfiguration, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.SetAudioEncoderConfiguration, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleAudioEncoderConfigurations, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioEncoderConfigurationOptions, Path = GetProfileFeaturesPath(AUDIOSTREAMING), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            // Multicast
                            new FunctionalityItem(){Functionality = Functionality.StartMulticastStreaming, Path = GetProfileFeaturesPath(MEDIASTREAMINGMULTICAST), Features = new Feature[]{Feature.MediaService, Feature.RTPMulticastUDP}},
                            new FunctionalityItem(){Functionality = Functionality.StopMulticastStreaming, Path = GetProfileFeaturesPath(MEDIASTREAMINGMULTICAST), Features = new Feature[]{Feature.MediaService, Feature.RTPMulticastUDP}},
                            // Relay outputs
                            new FunctionalityItem(){Functionality = Functionality.GetRelayOutputs, Path = GetProfileFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.DeviceIORelayOutputs}},
                            new FunctionalityItem(){Functionality = Functionality.SetRelayOutputSettings, Path = GetProfileFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.DeviceIORelayOutputs}},
                            new FunctionalityItem(){Functionality = Functionality.SetRelayOutputState, Path = GetProfileFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.DeviceIORelayOutputs}},
                            // NTP
                            new FunctionalityItem(){Functionality = Functionality.GetNTP, Path = GetProfileFeaturesPath(NTP), Features = new Feature[]{Feature.NTP}},
                            new FunctionalityItem(){Functionality = Functionality.SetNTP, Path = GetProfileFeaturesPath(NTP), Features = new Feature[]{Feature.NTP}},
                            // Dynamic DNS
                            new FunctionalityItem(){Functionality = Functionality.GetDynamicDNS, Path = GetProfileFeaturesPath(DYNAMICIP), Features = new Feature[]{Feature.DynamicDNS}},
                            new FunctionalityItem(){Functionality = Functionality.SetDynamicDNS, Path = GetProfileFeaturesPath(DYNAMICIP), Features = new Feature[]{Feature.DynamicDNS}},
                            // Zero Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetZeroConfiguration, Path = GetProfileFeaturesPath(ZEROCONFIGURATION), Features = new Feature[]{Feature.ZeroConfiguration}},
                            new FunctionalityItem(){Functionality = Functionality.SetZeroConfiguration, Path = GetProfileFeaturesPath(ZEROCONFIGURATION), Features = new Feature[]{Feature.ZeroConfiguration}},
                            // IP Address filtering
                            new FunctionalityItem(){Functionality = Functionality.GetIPAddressFilter, Path = GetProfileFeaturesPath(IPFILTERING), Features = new Feature[]{Feature.IPFilter}},
                            new FunctionalityItem(){Functionality = Functionality.SetIPAddressFilter, Path = GetProfileFeaturesPath(IPFILTERING), Features = new Feature[]{Feature.IPFilter}},
                            new FunctionalityItem(){Functionality = Functionality.AddIPAddressFilter, Path = GetProfileFeaturesPath(IPFILTERING), Features = new Feature[]{Feature.IPFilter}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveIPAddressFilter, Path = GetProfileFeaturesPath(IPFILTERING), Features = new Feature[]{Feature.IPFilter}},

                        }
                    );
                _profileFunctionalities.AddRange(LoadCoreFunctionalities());
            }

            return _profileFunctionalities;
        }


        private static List<FunctionalityItem> LoadCoreFunctionalities()
        {

            List<FunctionalityItem> coreFunctionalities =
                new List<FunctionalityItem>();

            coreFunctionalities.AddRange(
                new FunctionalityItem[]
                    {
                        //Discovery
                        new FunctionalityItem(){Functionality = Functionality.WSDiscovery, Path = GetCoreFullPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.GetDiscoveryMode, Path = GetCoreFullPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.SetDiscoveryMode, Path = GetCoreFullPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.GetScopes, Path = GetCoreFullPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.SetScopes, Path = GetCoreFullPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.AddScopes, Path = GetCoreFullPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.RemoveScopes, Path = GetCoreFullPath(DISCOVERY)},
                        // Network
                        new FunctionalityItem(){Functionality = Functionality.GetHostname, Path = GetCoreFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.SetHostname, Path = GetCoreFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.GetDns, Path = GetCoreFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.SetDns, Path = GetCoreFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.GetNetworkInterfaces, Path = GetCoreFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.SetNetworkInterfaces, Path = GetCoreFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.GetNetworkProtocols, Path = GetCoreFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.SetNetworkProtocols, Path = GetCoreFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.GetNetworkDefaultGateway, Path = GetCoreFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.SetNetworkDefaultGateway, Path = GetCoreFullPath(NETWORKCONFIGURATION)},
                        // System
                        new FunctionalityItem(){Functionality = Functionality.GetDeviceInformation, Path = GetCoreFullPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.GetSystemDateAndTime, Path = GetCoreFullPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.SetSystemDateAndTime, Path = GetCoreFullPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.SetSystemFactoryDefault, Path = GetCoreFullPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.Reboot, Path = GetCoreFullPath(SYSTEM)},
                        // User handling
                        new FunctionalityItem(){Functionality = Functionality.GetUsers, Path = GetCoreFullPath(USERHANDLING)},
                        new FunctionalityItem(){Functionality = Functionality.CreateUsers, Path = GetCoreFullPath(USERHANDLING)},
                        new FunctionalityItem(){Functionality = Functionality.DeleteUsers, Path = GetCoreFullPath(USERHANDLING)},
                        new FunctionalityItem(){Functionality = Functionality.SetUser, Path = GetCoreFullPath(USERHANDLING)},
                        // Event handling
                        new FunctionalityItem(){Functionality = Functionality.Notify, Path = GetCoreFullPath(EVENT), Features = new Feature[]{ Feature.WSBasicNotification, }},
                        new FunctionalityItem(){Functionality = Functionality.Subscribe, Path = GetCoreFullPath(EVENT), Features = new Feature[]{ Feature.WSBasicNotification, }},
                        new FunctionalityItem(){Functionality = Functionality.Renew, Path = GetCoreFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.Unsubscribe, Path = GetCoreFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.EventsSetSynchronizationPoint, Path = GetCoreFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.CreatePullPointSubscription, Path = GetCoreFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.PullMessages, Path = GetCoreFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.GetEventProperties, Path = GetCoreFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.TopicFilter, Path = GetCoreFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.MessageContentFilter, Path = GetCoreFullPath(EVENT)},
                        //Media Profile Configuration
                        new FunctionalityItem(){Functionality = Functionality.GetProfiles, Path = GetCoreFullPath(MEDIAPROFILECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.GetProfile, Path = GetCoreFullPath(MEDIAPROFILECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.CreateProfile, Path = GetCoreFullPath(MEDIAPROFILECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.DeleteProfile, Path = GetCoreFullPath(MEDIAPROFILECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        // Video Source Configuration
                        new FunctionalityItem(){Functionality = Functionality.GetVideoSources, Path = GetCoreFullPath(VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.GetVideoSourceConfiguration, Path = GetCoreFullPath(VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.GetVideoSourceConfigurations, Path = GetCoreFullPath(VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.AddVideoSourceConfiguration, Path = GetCoreFullPath(VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.RemoveVideoSourceConfiguration, Path = GetCoreFullPath(VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.SetVideoSourceConfiguration, Path = GetCoreFullPath(VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.GetCompatibleVideoSourceConfigurations, Path = GetCoreFullPath(VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.GetVideoSourceConfigurationOptions, Path = GetCoreFullPath(VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        // Metadata Configuration
                        new FunctionalityItem(){Functionality = Functionality.GetMetadataConfiguration, Path = GetCoreFullPath(METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.GetMetadataConfigurations, Path = GetCoreFullPath(METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.AddMetadataConfiguration, Path = GetCoreFullPath(METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.RemoveMetadataConfiguration, Path = GetCoreFullPath(METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.SetMetadataConfiguration, Path = GetCoreFullPath(METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.GetCompatibleMetadataConfigurations, Path = GetCoreFullPath(METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                        new FunctionalityItem(){Functionality = Functionality.GetMetadataConfigurationOptions, Path = GetCoreFullPath(METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}}

                    }
                );

            return coreFunctionalities;
        }


        #endregion

        #region IProfileDefinition Members

        public IEnumerable<String> MandatoryScopes
        {
            get
            {
                return _mandatoryScopes;
            }
        }

        public IEnumerable<FunctionalityItem> Functionalities
        {
            get
            {
                if (_profileFunctionalities == null)
                {
                    LoadProfileFunctionalities();
                }
                return _profileFunctionalities;
            }
        }

        //public string Scope
        //{
        //    get { return STREAMING_PROFILE_SCOPE; }
        //}

        public ProfileStatus Check(out string reason, IEnumerable<Feature> features, IEnumerable<string> scopes, Dictionary<string, object> parameters)
        {
            reason = string.Empty;

            var Name = this.GetProfileName();
            var Scope = this.GetProfileScope();

            StringBuilder sb = new StringBuilder();
            ProfileStatus status = ProfileStatus.NotSupported;

            sb.AppendLine(string.Format("Check profile support for {0}", Name));

            bool scopePresent = scopes.Contains(Scope);
            sb.AppendLine(string.Format("Scope {0}: \t\t{1}", Scope, scopePresent ? "PRESENT" : "NOT PRESENT"));

            if (!scopePresent)
            {
                sb.AppendFormat("Profile not supported");
            }
            else
            {
                bool profileOk = true;
                bool supported;

                Action<Feature, string> checkNextMandatory = new Action<Feature, string>(
                    (feature, displayName) =>
                    {
                        supported = features.ContainsFeature(feature);
                        LogMandatory(sb, displayName, supported);
                        profileOk = profileOk && supported;
                    });

                Action<Feature, string> checkNextOptional = new Action<Feature, string>(
                    (feature, displayName) =>
                    {
                        supported = features.Contains(feature);
                        LogOptional(sb, displayName, supported);
                    });

                checkNextMandatory(Feature.GetCapabilities, "GetCapabilities");

                LogMandatoryFeature(sb, "Discovery");
                LogMandatoryFeature(sb, "Network Configuration");
                LogMandatoryFeature(sb, "System");
                LogMandatoryFeature(sb, "User Handling");
                LogMandatoryFeature(sb, "Event Handling");
                
                checkNextMandatory(Feature.DiscoveryTypesDnNetworkVideoTransmitter, "dn:NetworkVideoTransmitter");
                checkNextMandatory(Feature.WSBasicNotification, "WS Basic Notification");

                checkNextOptional(Feature.NTP, "NTP");
                checkNextOptional(Feature.DynamicDNS, "Dynamic DNS");
                checkNextOptional(Feature.ZeroConfiguration, "Zero Configuration");
                checkNextOptional(Feature.IPFilter, "IP Address Filtering");

                checkNextMandatory(Feature.WSU, "WS-UsernameToken Authentication");
                checkNextOptional(Feature.Digest, "HTTP Digest Authentication");

                checkNextMandatory(Feature.MediaService, "Media Profile Configuration");
                checkNextMandatory(Feature.RTSS, "Media Streaming");
                checkNextMandatory(Feature.MediaService, "Video Source Configuration");
                checkNextMandatory(Feature.MediaService, "Video Encoder Configuration");
                checkNextMandatory(Feature.MediaService, "Metadata Configuration");
                checkNextOptional(Feature.RTPMulticastUDP, "RTP-Multicast/UDP");

                checkNextOptional(Feature.PTZService, "PTZ");
                checkNextOptional(Feature.PTZPresets, "PTZ – Presets");
                checkNextOptional(Feature.PTZHome, "PTZ – Home Position");
                checkNextOptional(Feature.PTZAbsolute, "PTZ – Absolute Positioning");
                checkNextOptional(Feature.PTZRelative, "PTZ – Relative Positioning");
                checkNextOptional(Feature.PTZAuxiliary, "PTZ – Auxiliary Commands");
                checkNextOptional(Feature.Audio, "Audio Streaming");
                checkNextOptional(Feature.DeviceIORelayOutputs, "Relay Outputs");

                if (profileOk)
                {
                    status = ProfileStatus.Supported;
                }
                else
                {
                    status = ProfileStatus.Failed;
                }
            }
            reason = sb.ToString();
            return status;
        }


        #endregion

        void InitFeatures()
        {
            _features = new List<ProfileFeature>();

            _features.Add(new ProfileFeature() { Feature = Feature.Discovery, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.DiscoveryTypesDnNetworkVideoTransmitter, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.Network, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.System, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.Security, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.EventsService, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.WSBasicNotification, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.NTP, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.DynamicDNS, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.ZeroConfiguration, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.IPFilter, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.WSU, State = ProfileFeatureState.ProfileMandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.Digest, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.MediaService, State = ProfileFeatureState.ProfileMandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.RTSS, State = ProfileFeatureState.ProfileMandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.RTPMulticastUDP, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.PTZService, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.PTZPresets, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.PTZHome, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.PTZAbsolute, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.PTZRelative, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.PTZAuxiliary, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.Audio, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.DeviceIORelayOutputs, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.H264OrMPEG4, State = ProfileFeatureState.Optional });
        }

        void InitDiscoveryTypes()
        {
            var list = new List<Feature> { Feature.DiscoveryTypesDnNetworkVideoTransmitter };

            MandatoryDiscoveryTypes = list;
        }
    }

}
