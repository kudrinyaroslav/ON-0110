using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.Tests.TestCases.Profiles
{
    [ProfileDefinition]
    public class StorageProfile : BaseProfile, IProfileDefinition
    {
        // Profile scope
        public const string STORAGE_PROFILE_SCOPE = "onvif://www.onvif.org/Profile/G";

        public StorageProfile()
        {
            _mandatoryScopes = new List<String>();
            _mandatoryScopes.AddRange(new String[]
                                   {
                                       STORAGE_PROFILE_SCOPE
                                   });

            InitFeatures();
        }

        //Profile name
        public string Name
        {
            get { return "Profile G"; }
        }

        #region Features

        private List<String> _mandatoryScopes;

        #endregion

        #region Functionality
        
        private const string ROOTDEVICEMANDATORY = "Device Mandatory Features";

        private const string DISCOVERY = "Discovery";
        private const string NETWORKCONFIGURATION = "Network Configuration";
        private const string SYSTEM = "System";
        private const string USERHANDLING = "User Handling";
        private const string EVENT = "Event handling";
        

        private const string ROOTPROFILEMANDATORY = "Profile Mandatory Features";
        
        private const string SECURITY = "User Authentication";
        private const string CAPABILITIES = "Capabilities";
        private const string RECORDINGSEARCH = "Recording Search";
        private const string REPLAYCONTROL = "Replay Contorl";
        
        
        private const string ROOTPROFILECONDITIONAL = "Profile Conditional Features";

        private const string RECORDINGCONTROL = "Recording Control";
        private const string RECORDINGCONTROLDYMANICRECORDING = "Recording Control – Dynamic Recording";
        private const string RECORDINGSEARCHMETADATASEARCH = "Recording Search – Metadata Search";
        private const string RECORDINGSEARCHPTZPOSITIONSEARCH = "Recording Search – PTZ Position Search";
        

        private const string ROOTPROFILESPECIFIC = "Profile Specific Requirements";
        
        private const string PROFILEASSOURCE = "Recording Control – Using a Media Profile as Source";
        private const string MEDIAPROFILECONFIGURATION = "Media Profile Configuration";
        private const string VIDEOSOURCECONFIGURATION = "Video Source Configuration";
        private const string VIDEOENCODERCONFIGURATION = "Video Encoder Configuration";
        private const string METADATACONFIGURATION = "Metadata Configuration";
        private const string AUDIOSOURCECONFIGURATION = "Audio Source Configuration";
        private const string AUDIOENCODERCONFIGURATION = "Audio Encoder Configuration";
        
        private const string RECEIVERASSOURCE = "Recording Control – Using a Receiver as Source";
        private const string MEDIASTREAMING = "Media Streaming";
        private const string RECEIVERCONFIGURATION = "Receiver Configuration";
        

        protected List<FunctionalityItem> LoadProfileFunctionalities()
        {
            if (_profileFunctionalities == null)
            {
                _profileFunctionalities = new List<FunctionalityItem>();

                _profileFunctionalities.AddRange(
                    new FunctionalityItem[]
                        {
                            // Profile mandatory
                                // security
                            new FunctionalityItem(){Functionality = Functionality.WsSecurity, Path = GetFullPath(ROOTPROFILEMANDATORY, SECURITY), Features = new Feature[]{Feature.WSU}},
                            new FunctionalityItem(){Functionality = Functionality.DigestAuthentication, Path = GetFullPath(ROOTPROFILEMANDATORY, SECURITY), Features = new Feature[]{Feature.Digest}},

                                // capabilities
                            new FunctionalityItem(){Functionality = Functionality.GetServices, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES),Features = new Feature[]{Feature.GetServices}},
                            new FunctionalityItem(){Functionality = Functionality.GetDeviceServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices}},
                            new FunctionalityItem(){Functionality = Functionality.GetEventsServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.EventsService}},
                            new FunctionalityItem(){Functionality = Functionality.GetMediaServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.MediaService}}, /* ? */
                            new FunctionalityItem(){Functionality = Functionality.GetPTZServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.PTZService}}, /* ? */
                            new FunctionalityItem(){Functionality = Functionality.GetReceiverServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.ReceiverService }}, /* ? */
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetReplayServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.ReplayService}},
                            new FunctionalityItem(){Functionality = Functionality.GetSearchServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.RecordingSearchService}},
                                // recording search
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingSummary, Path = GetFullPath(ROOTPROFILEMANDATORY, RECORDINGSEARCH),  Features = new Feature[]{Feature.RecordingSearchService}},
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingInformation, Path = GetFullPath(ROOTPROFILEMANDATORY, RECORDINGSEARCH),  Features = new Feature[]{Feature.RecordingSearchService}},
                            new FunctionalityItem(){Functionality = Functionality.GetMediaAttributes, Path = GetFullPath(ROOTPROFILEMANDATORY, RECORDINGSEARCH),  Features = new Feature[]{Feature.RecordingSearchService}},
                            new FunctionalityItem(){Functionality = Functionality.FindRecordings, Path = GetFullPath(ROOTPROFILEMANDATORY, RECORDINGSEARCH), Features = new Feature[]{Feature.RecordingSearchService}},
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingSearchResults, Path = GetFullPath(ROOTPROFILEMANDATORY, RECORDINGSEARCH), Features = new Feature[]{Feature.RecordingSearchService}},
                            new FunctionalityItem(){Functionality = Functionality.FindEvents, Path = GetFullPath(ROOTPROFILEMANDATORY, RECORDINGSEARCH), Features = new Feature[]{Feature.RecordingSearchService}},
                            new FunctionalityItem(){Functionality = Functionality.GetEventSearchResults, Path = GetFullPath(ROOTPROFILEMANDATORY, RECORDINGSEARCH), Features = new Feature[]{Feature.RecordingSearchService}},
                            new FunctionalityItem(){Functionality = Functionality.EndSearch, Path = GetFullPath(ROOTPROFILEMANDATORY, RECORDINGSEARCH), Features = new Feature[]{Feature.RecordingSearchService}},
                            new FunctionalityItem(){Functionality = Functionality.RecordingStateEvent, Path = GetFullPath(ROOTPROFILEMANDATORY, RECORDINGSEARCH), Features = new Feature[]{Feature.RecordingSearchService}},
                            new FunctionalityItem(){Functionality = Functionality.TrackStateEvent, Path = GetFullPath(ROOTPROFILEMANDATORY, RECORDINGSEARCH), Features = new Feature[]{Feature.RecordingSearchService}},
                            new FunctionalityItem(){Functionality = Functionality.XPathDialect, Path = GetFullPath(ROOTPROFILEMANDATORY, RECORDINGSEARCH), Features = new Feature[]{Feature.RecordingSearchService}},
                                // replay control
                            new FunctionalityItem(){Functionality = Functionality.ReverseReplay, Path = GetFullPath(ROOTPROFILEMANDATORY, REPLAYCONTROL), Features = new Feature[]{Feature.ReplayService}},
                            new FunctionalityItem(){Functionality = Functionality.ReplayUsingRTSP, Path = GetFullPath(ROOTPROFILEMANDATORY, REPLAYCONTROL), Features = new Feature[]{Feature.ReplayService}},
                            new FunctionalityItem(){Functionality = Functionality.GetReplayUri, Path = GetFullPath(ROOTPROFILEMANDATORY, REPLAYCONTROL), Features = new Feature[]{Feature.ReplayService}},
                            new FunctionalityItem(){Functionality = Functionality.SetReplayConfiguration, Path = GetFullPath(ROOTPROFILEMANDATORY, REPLAYCONTROL), Features = new Feature[]{Feature.ReplayService}},
                            new FunctionalityItem(){Functionality = Functionality.GetReplayConfiguration, Path = GetFullPath(ROOTPROFILEMANDATORY, REPLAYCONTROL), Features = new Feature[]{Feature.ReplayService}},
                           
                            // profile conditional
                                //	Recording Control /* 15 points */
                            new FunctionalityItem(){Functionality = Functionality.GetRecordings, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL),Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.SetRecordingConfiguration, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingConfiguration, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetTrackConfiguration, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.SetTrackConfiguration, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.CreateRecordingJob, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.DeleteRecordingJob, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingJobs, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.SetRecordingJobConfiguration, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingJobConfiguration, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.SetRecordingJobMode, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingJobState, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.RecordingJobStateChangeEvent, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.ConfigurationChangeEvent, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL),  Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.DataDeletionEvent, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                                //	Recording Control – Dynamic Recording
                            new FunctionalityItem(){Functionality = Functionality.DeleteRecording, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecording}},
                            new FunctionalityItem(){Functionality = Functionality.RecordingCreationDeletionEvent, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecording}},
                            new FunctionalityItem(){Functionality = Functionality.CreateTrack, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecording}},
                            new FunctionalityItem(){Functionality = Functionality.DeleteTrack, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecording}},
                            new FunctionalityItem(){Functionality = Functionality.TrackCreationDeletionEvent, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecording}},
                                //	Recording Search – Metadata Search
                            new FunctionalityItem(){Functionality = Functionality.FindMetadata, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGSEARCHMETADATASEARCH), Features = new Feature[]{Feature.RecordingControlService, Feature.MetadataSearch}},
                            new FunctionalityItem(){Functionality = Functionality.GetMetadataSearchResults, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGSEARCHMETADATASEARCH), Features = new Feature[]{Feature.RecordingControlService, Feature.MetadataSearch}},
                                //	Recording Search – PTZ Position Search
                            new FunctionalityItem(){Functionality = Functionality.FindPTZPosition, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGSEARCHPTZPOSITIONSEARCH), Features = new Feature[]{Feature.RecordingControlService, Feature.PTZPositionSearch}},
                            new FunctionalityItem(){Functionality = Functionality.GetPTZPositionSearchResults, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGSEARCHPTZPOSITIONSEARCH), Features = new Feature[]{Feature.RecordingControlService, Feature.PTZPositionSearch}},
                            // Profile specific requirements
                                //	Recording Control – Using a Media Profile as Source
                                    //	Media Profile Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetProfiles, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, MEDIAPROFILECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.ProfileAsSource}},
                            new FunctionalityItem(){Functionality = Functionality.GetProfile, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, MEDIAPROFILECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.ProfileAsSource}},
                            new FunctionalityItem(){Functionality = Functionality.CreateProfile, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, MEDIAPROFILECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.ProfileAsSource}},
                            new FunctionalityItem(){Functionality = Functionality.DeleteProfile, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, MEDIAPROFILECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.ProfileAsSource}},
                                    //	Video Source Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetVideoSources, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.ProfileAsSource}},
                            new FunctionalityItem(){Functionality = Functionality.GetVideoSourceConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.ProfileAsSource}},
                            new FunctionalityItem(){Functionality = Functionality.GetVideoSourceConfigurations, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.ProfileAsSource}},
                            new FunctionalityItem(){Functionality = Functionality.AddVideoSourceConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.ProfileAsSource}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveVideoSourceConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.ProfileAsSource}},
                            new FunctionalityItem(){Functionality = Functionality.SetVideoSourceConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.ProfileAsSource}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleVideoSourceConfigurations, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.ProfileAsSource}},
                            new FunctionalityItem(){Functionality = Functionality.GetVideoSourceConfigurationOptions, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.ProfileAsSource}},
                                    //	Video Encoder Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetVideoEncoderConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetVideoEncoderConfigurations, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.AddVideoEncoderConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveVideoEncoderConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.SetVideoEncoderConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleVideoEncoderConfigurations, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetVideoEncoderConfigurationOptions, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetGuaranteedNumberOfVideoEncoderInstances, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                                    //	Metadata Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetMetadataConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetMetadataConfigurations, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.AddMetadataConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveMetadataConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.SetMetadataConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleMetadataConfigurations, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetMetadataConfigurationOptions, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                                    //	Audio Source Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetAudioSources, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioSourceConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioSourceConfigurations, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.AddAudioSourceConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveAudioSourceConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.SetAudioSourceConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleAudioSourceConfigurations, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioSourceConfigurationOptions, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                                    //	Audio Encoder Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetAudioEncoderConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioEncoderConfigurations, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.AddAudioEncoderConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveAudioEncoderConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.SetAudioEncoderConfiguration, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleAudioEncoderConfigurations, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioEncoderConfigurationOptions, Path = GetFullPath(ROOTPROFILESPECIFIC, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{Feature.MediaService}},

                                //	Recording Control – Using a Receiver as Source
                                    //  Receiver Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetReceivers, Path = GetFullPath(ROOTPROFILESPECIFIC, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.GetReceiver, Path = GetFullPath(ROOTPROFILESPECIFIC, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.CreateReceiver, Path = GetFullPath(ROOTPROFILESPECIFIC, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.DeleteReceiver, Path = GetFullPath(ROOTPROFILESPECIFIC, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.ConfigureReceiver, Path = GetFullPath(ROOTPROFILESPECIFIC, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.SetReceiverMode, Path = GetFullPath(ROOTPROFILESPECIFIC, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.SetReceiverState, Path = GetFullPath(ROOTPROFILESPECIFIC, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.ChangeStateEvent, Path = GetFullPath(ROOTPROFILESPECIFIC, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.ConnectionFailedEvent, Path = GetFullPath(ROOTPROFILESPECIFIC, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{Feature.ReceiverService}},
                                    //	Media Streaming
                            new FunctionalityItem(){Functionality = Functionality.MediaStreamingRtsp, Path = GetFullPath(ROOTPROFILESPECIFIC, RECEIVERASSOURCE, MEDIASTREAMING), Features = new Feature[]{Feature.MediaService}},
                                                        
                            // Device mandatory
                                //Discovery
                            new FunctionalityItem(){Functionality = Functionality.WSDiscovery, Path = GetFullPath(ROOTDEVICEMANDATORY, DISCOVERY)},
                            new FunctionalityItem(){Functionality = Functionality.GetDiscoveryMode, Path = GetFullPath(ROOTDEVICEMANDATORY, DISCOVERY)},
                            new FunctionalityItem(){Functionality = Functionality.SetDiscoveryMode, Path = GetFullPath(ROOTDEVICEMANDATORY, DISCOVERY)},
                            new FunctionalityItem(){Functionality = Functionality.GetScopes, Path = GetFullPath(ROOTDEVICEMANDATORY, DISCOVERY)},
                            new FunctionalityItem(){Functionality = Functionality.SetScopes, Path = GetFullPath(ROOTDEVICEMANDATORY, DISCOVERY)},
                            new FunctionalityItem(){Functionality = Functionality.AddScopes, Path = GetFullPath(ROOTDEVICEMANDATORY, DISCOVERY)},
                            new FunctionalityItem(){Functionality = Functionality.RemoveScopes, Path = GetFullPath(ROOTDEVICEMANDATORY, DISCOVERY)},
                                // Network
                            new FunctionalityItem(){Functionality = Functionality.GetHostname, Path = GetFullPath(ROOTDEVICEMANDATORY, NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.SetHostname, Path = GetFullPath(ROOTDEVICEMANDATORY, NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetDns, Path = GetFullPath(ROOTDEVICEMANDATORY, NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.SetDns, Path = GetFullPath(ROOTDEVICEMANDATORY, NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetNetworkInterfaces, Path = GetFullPath(ROOTDEVICEMANDATORY, NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.SetNetworkInterfaces, Path = GetFullPath(ROOTDEVICEMANDATORY, NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetNetworkProtocols, Path = GetFullPath(ROOTDEVICEMANDATORY, NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.SetNetworkProtocols, Path = GetFullPath(ROOTDEVICEMANDATORY, NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetNetworkDefaultGateway, Path = GetFullPath(ROOTDEVICEMANDATORY, NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.SetNetworkDefaultGateway, Path = GetFullPath(ROOTDEVICEMANDATORY, NETWORKCONFIGURATION)},
                                // System
                            new FunctionalityItem(){Functionality = Functionality.GetDeviceInformation, Path = GetFullPath(ROOTDEVICEMANDATORY, SYSTEM)},
                            new FunctionalityItem(){Functionality = Functionality.GetSystemDateAndTime, Path = GetFullPath(ROOTDEVICEMANDATORY, SYSTEM)},
                            new FunctionalityItem(){Functionality = Functionality.SetSystemDateAndTime, Path = GetFullPath(ROOTDEVICEMANDATORY, SYSTEM)},
                            new FunctionalityItem(){Functionality = Functionality.SetSystemFactoryDefaults, Path = GetFullPath(ROOTDEVICEMANDATORY, SYSTEM)},
                            new FunctionalityItem(){Functionality = Functionality.Reboot, Path = GetFullPath(ROOTDEVICEMANDATORY, SYSTEM)},
                                // User handling
                            new FunctionalityItem(){Functionality = Functionality.GetUsers, Path = GetFullPath(ROOTDEVICEMANDATORY, USERHANDLING)},
                            new FunctionalityItem(){Functionality = Functionality.CreateUsers, Path = GetFullPath(ROOTDEVICEMANDATORY, USERHANDLING)},
                            new FunctionalityItem(){Functionality = Functionality.DeleteUsers, Path = GetFullPath(ROOTDEVICEMANDATORY, USERHANDLING)},
                            new FunctionalityItem(){Functionality = Functionality.SetUser, Path = GetFullPath(ROOTDEVICEMANDATORY, USERHANDLING)},
                                // Event handling
                            new FunctionalityItem(){Functionality = Functionality.Notify, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.Subscribe, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.Renew, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.Unsubscribe, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.EventsSetSynchronizationPoint, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.CreatePullPointSubscription, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.PullMessages, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.GetEventProperties, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.TopicFilter, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.MessageContentFilter, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)}

                        }
                    );
            }

            return _profileFunctionalities;
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

        public string Scope
        {
            get { return STORAGE_PROFILE_SCOPE; }
        }

        public ProfileStatus Check(out string reason, IEnumerable<Feature> features, IEnumerable<string> scopes)
        {
            reason = string.Empty;
            
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
                        supported = features.Contains(feature);
                        LogMandatory(sb, displayName, supported);
                        profileOk = profileOk && supported;
                    });

                Action<Feature, string> checkNextOptional = new Action<Feature, string>(
                    (feature, displayName) =>
                    {
                        supported = features.Contains(feature);
                        LogOptional(sb, displayName, supported);
                    });

                checkNextMandatory(Feature.GetServices, "GetServices");

                checkNextMandatory(Feature.Discovery, "Discovery");
                LogMandatoryFeature(sb, "Network Configuration");
                LogMandatoryFeature(sb, "System");
                LogMandatoryFeature(sb, "User Handling");
                LogMandatoryFeature(sb, "Event Handling");

                checkNextMandatory(Feature.RecordingSearchService, "Recording Search");
                checkNextOptional(Feature.ReverseReplay, "Reverse Replay");

                checkNextMandatory(Feature.ReplayService, "Replay Service");
                checkNextMandatory(Feature.RecordingControlService, "Recording Control");

                checkNextOptional(Feature.DynamicRecording, "Dynamic Recording");
                checkNextOptional(Feature.MetadataSearch, "Metadata Search");
                checkNextOptional(Feature.PTZPositionSearch, "PTZ Position Search");

                checkNextOptional(Feature.WSU, "WS-UsernameToken");
                checkNextMandatory(Feature.Digest, "Digest");


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

            _features.Add(new ProfileFeature() { Feature = Feature.GetServices, State = ProfileFeatureState.ProfileMandatory });

            _features.Add(new ProfileFeature() { Feature = Feature.Discovery, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.Network, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.System, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.Security, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.EventsService, State = ProfileFeatureState.Mandatory });
            
            _features.Add(new ProfileFeature() { Feature = Feature.WSU, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.Digest, State = ProfileFeatureState.ProfileMandatory});

            _features.Add(new ProfileFeature() { Feature = Feature.RecordingSearchService, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.ReverseReplay, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.ReplayService, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.RecordingControlService, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.DynamicRecording, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.MetadataSearch, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.PTZPositionSearch, State = ProfileFeatureState.Optional });
            
        }

    }

}
