using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.Tests.TestCases.Profiles
{
    [ProfileDefinition("Profile G", "onvif://www.onvif.org/Profile/G", ProfileVersionStatus.Release)]
    public class StorageProfile : BaseProfile, IProfileDefinition
    {
        public StorageProfile()
        {
            _mandatoryScopes = new List<String>();
            _mandatoryScopes.AddRange(new [] { this.GetProfileScope() });

            InitFeatures();
            InitDiscoveryTypes();
        }

        //Profile name
        public IEnumerable<Feature> MandatoryDiscoveryTypes { get; private set; }

        //public string Name
        //{
        //    get { return ""; }
        //}

        //public ProfileVersionStatus Status 
        //{
        //    get { return ProfileVersionStatus.ReleaseCandidate; } 
        //}

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
        
        private const string SECURITY = "Security";
        private const string CAPABILITIES = "Capabilities";
        private const string RECORDINGSEARCH = "Recording Search - Media Search";
        private const string REPLAYCONTROL = "Replay Control";
        
        
        private const string ROOTPROFILECONDITIONAL = "Profile Conditional Features";

        private const string RECORDINGCONTROL = "Recording Control";
        private const string RECORDINGCONTROLDYMANICRECORDING = "Recording Control – Dynamic Recording";
        private const string RECORDINGSEARCHMETADATASEARCH = "Recording Search – Metadata Search";
        private const string RECORDINGSEARCHPTZPOSITIONSEARCH = "Recording Search – PTZ Position Search";
        

        private const string ROOTPROFILESPECIFIC = "Profile Specific Requirements";
        
        private const string PROFILEASSOURCE = "Recording Control – Using an on-board media source";
        private const string MEDIAPROFILECONFIGURATION = "Media Profile Configuration";
        private const string VIDEOSOURCECONFIGURATION = "Video Source Configuration";
        private const string VIDEOENCODERCONFIGURATION = "Video Encoder Configuration";
        private const string METADATACONFIGURATION = "Metadata Configuration";
        private const string AUDIOSOURCECONFIGURATION = "Audio Source Configuration";
        private const string AUDIOENCODERCONFIGURATION = "Audio Encoder Configuration";
        private const string AUDIODECODERCONFIGURATION = "Audio Decoder Configuration";
        private const string AUDIOOUTPUTCONFIGURATION = "Audio Output Configuration";
        private const string RECORDINGCONFIGURATION = "Recording Configuration";
        private const string RECORDINGSOURCECONFIG = "Configuration of the Recording Source";
        
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
                            //new FunctionalityItem(){Functionality = Functionality.WsSecurity, Path = GetFullPath(ROOTPROFILEMANDATORY, SECURITY), Features = new Feature[]{Feature.WSU}},
                            new FunctionalityItem(){Functionality = Functionality.DigestAuthentication, Path = GetFullPath(ROOTPROFILEMANDATORY, SECURITY), Features = new Feature[]{Feature.Digest}},

                                // capabilities
                            new FunctionalityItem(){Functionality = Functionality.GetServices, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES),Features = new Feature[]{Feature.GetServices}},
                            new FunctionalityItem(){Functionality = Functionality.GetDeviceServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices}},
                            new FunctionalityItem(){Functionality = Functionality.GetEventsServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.EventsService}},
                            new FunctionalityItem(){Functionality = Functionality.GetMediaServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.MediaOrReceiver, Feature.MediaService}},  // ?
                            //new FunctionalityItem(){Functionality = Functionality.GetPTZServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.PTZService}}, // ?
                            new FunctionalityItem(){Functionality = Functionality.GetReceiverServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.MediaOrReceiver, Feature.ReceiverService }}, // ?
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetReplayServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.ReplayService}},
                            new FunctionalityItem(){Functionality = Functionality.GetSearchServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.RecordingSearchService}},
                            //new FunctionalityItem(){Functionality = Functionality.GetDeviceIOServiceCapabilities, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices, Feature.DeviceIoService}},
                            new FunctionalityItem(){Functionality = Functionality.MaxPullPoints, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.MaxPullPoints}},
                            new FunctionalityItem(){Functionality = Functionality.GetWsdlUrl, Path = GetFullPath(ROOTPROFILEMANDATORY, CAPABILITIES), Features = new Feature[]{Feature.GetServices}},
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
                            new FunctionalityItem(){Functionality = Functionality.ReverseReplay, Path = GetFullPath(ROOTPROFILEMANDATORY, REPLAYCONTROL), Features = new Feature[]{Feature.ReverseReplay}},
                            new FunctionalityItem(){Functionality = Functionality.MediaReplay, Path = GetFullPath(ROOTPROFILEMANDATORY, REPLAYCONTROL), Features = new Feature[]{Feature.ReplayService}},
                            new FunctionalityItem(){Functionality = Functionality.GetReplayUri, Path = GetFullPath(ROOTPROFILEMANDATORY, REPLAYCONTROL), Features = new Feature[]{Feature.ReplayService}},
                            new FunctionalityItem(){Functionality = Functionality.SetReplayConfiguration, Path = GetFullPath(ROOTPROFILEMANDATORY, REPLAYCONTROL), Features = new Feature[]{Feature.ReplayService}},
                            new FunctionalityItem(){Functionality = Functionality.GetReplayConfiguration, Path = GetFullPath(ROOTPROFILEMANDATORY, REPLAYCONTROL), Features = new Feature[]{Feature.ReplayService}},
                            new FunctionalityItem(){Functionality = Functionality.RTPHeaderExtension, Path = GetFullPath(ROOTPROFILEMANDATORY, REPLAYCONTROL), Features = new Feature[]{Feature.ReplayService}},
                            new FunctionalityItem(){Functionality = Functionality.RTSPFeatureTag, Path = GetFullPath(ROOTPROFILEMANDATORY, REPLAYCONTROL), Features = new Feature[]{Feature.ReplayService}},
                           
                            // profile conditional
                                //	Recording Control - 15 points 
                            new FunctionalityItem(){Functionality = Functionality.GetRecordings, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONTROL),Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.CreateRecordingJob, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.DeleteRecordingJob, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingJobs, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.SetRecordingJobMode, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingJobState, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingOptions, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService, Feature.RecordingOptions}},
                            new FunctionalityItem(){Functionality = Functionality.RecordingJobStateChangeEvent, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.DeleteTrackDataEvent, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONTROL), Features = new Feature[]{Feature.RecordingControlService, Feature.RecordingConfigDeleteTrackDataEvent}},

                                //	Recording Control – Dynamic Recording
                            new FunctionalityItem(){Functionality = Functionality.CreateRecording, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecordings}},
                            new FunctionalityItem(){Functionality = Functionality.DeleteRecording, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecordings}},
                            new FunctionalityItem(){Functionality = Functionality.CreateRecordingEvent, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecordings}},
                            new FunctionalityItem(){Functionality = Functionality.DeleteRecordingEvent, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecordings}},
                            new FunctionalityItem(){Functionality = Functionality.CreateTrack, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecordings}},
                            new FunctionalityItem(){Functionality = Functionality.DeleteTrack, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecordings}},
                            new FunctionalityItem(){Functionality = Functionality.CreateTrackEvent, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecordings}},
                            new FunctionalityItem(){Functionality = Functionality.DeleteTrackEvent, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGCONTROLDYMANICRECORDING), Features = new Feature[]{Feature.RecordingControlService, Feature.DynamicRecordings}},
                                //	Recording Search – Metadata Search
                            new FunctionalityItem(){Functionality = Functionality.FindMetadata, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGSEARCHMETADATASEARCH), Features = new Feature[]{Feature.RecordingControlService, Feature.MetadataSearch}},
                            new FunctionalityItem(){Functionality = Functionality.GetMetadataSearchResults, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGSEARCHMETADATASEARCH), Features = new Feature[]{Feature.RecordingControlService, Feature.MetadataSearch}},
                                //	Recording Search – PTZ Position Search
                            new FunctionalityItem(){Functionality = Functionality.FindPTZPosition, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGSEARCHPTZPOSITIONSEARCH), Features = new Feature[]{Feature.RecordingControlService, Feature.PTZPositionSearch}},
                            new FunctionalityItem(){Functionality = Functionality.GetPTZPositionSearchResults, Path = GetFullPath(ROOTPROFILECONDITIONAL, RECORDINGSEARCHPTZPOSITIONSEARCH), Features = new Feature[]{Feature.RecordingControlService, Feature.PTZPositionSearch}},
                            // Profile specific requirements
                                //	Recording Control – Using a Media Profile as Source
                                    //	Media Profile Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetProfiles, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, MEDIAPROFILECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetProfile, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, MEDIAPROFILECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.CreateProfile, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, MEDIAPROFILECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.DeleteProfile, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, MEDIAPROFILECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                                    //	Video Source Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetVideoSources, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetVideoSourceConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetVideoSourceConfigurations, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.AddVideoSourceConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveVideoSourceConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.SetVideoSourceConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleVideoSourceConfigurations, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetVideoSourceConfigurationOptions, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE,VIDEOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                                    //	Video Encoder Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetVideoEncoderConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetVideoEncoderConfigurations, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.AddVideoEncoderConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveVideoEncoderConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.SetVideoEncoderConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleVideoEncoderConfigurations, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetVideoEncoderConfigurationOptions, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetGuaranteedNumberOfVideoEncoderInstances, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, VIDEOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                                    //	Metadata Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetMetadataConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetMetadataConfigurations, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.AddMetadataConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveMetadataConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.SetMetadataConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleMetadataConfigurations, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetMetadataConfigurationOptions, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, METADATACONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService}},
                                    //	Audio Source Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetAudioSources, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioSourceConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioSourceConfigurations, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.AddAudioSourceConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveAudioSourceConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.SetAudioSourceConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleAudioSourceConfigurations, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioSourceConfigurationOptions, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOSOURCECONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                                    //	Audio Encoder Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetAudioEncoderConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioEncoderConfigurations, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.AddAudioEncoderConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveAudioEncoderConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.SetAudioEncoderConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleAudioEncoderConfigurations, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioEncoderConfigurationOptions, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},

                                    //	Audio Decoder Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetAudioDecoderConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIODECODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetAudioDecoderConfigurations, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIODECODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveAudioDecoderConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIODECODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.GetCompatibleAudioDecoderConfigurations, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIODECODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},
                            new FunctionalityItem(){Functionality = Functionality.AddAudioDecoderConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOENCODERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.MediaService, Feature.Audio}},

                                    //	Audio Output Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetAudioOutputs, Path = GetFullPath(ROOTDEVICEMANDATORY, PROFILEASSOURCE, AUDIOOUTPUTCONFIGURATION), Features = new Feature[]{Feature.MediaService, Feature.Audio}},

                                //	Recording Control – Using a Receiver as Source
                                    //  Receiver Configuration
                            new FunctionalityItem(){Functionality = Functionality.GetReceivers, Path = GetFullPath(ROOTDEVICEMANDATORY, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.GetReceiver, Path = GetFullPath(ROOTDEVICEMANDATORY, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.CreateReceiver, Path = GetFullPath(ROOTDEVICEMANDATORY, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.DeleteReceiver, Path = GetFullPath(ROOTDEVICEMANDATORY, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.ConfigureReceiver, Path = GetFullPath(ROOTDEVICEMANDATORY, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.SetReceiverMode, Path = GetFullPath(ROOTDEVICEMANDATORY, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.GetReceiverState, Path = GetFullPath(ROOTDEVICEMANDATORY, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.ReceiverChangeStateEvent, Path = GetFullPath(ROOTDEVICEMANDATORY, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.ReceiverConnectionFailedEvent, Path = GetFullPath(ROOTDEVICEMANDATORY, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.ReceiverService}},
                            new FunctionalityItem(){Functionality = Functionality.MediaStreamingRtsp, Path = GetFullPath(ROOTDEVICEMANDATORY, RECEIVERASSOURCE, RECEIVERCONFIGURATION), Features = new Feature[]{ Feature.MediaOrReceiver, Feature.ReceiverService}},

                            new FunctionalityItem(){Functionality = Functionality.SetRecordingConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONFIGURATION, RECORDINGSOURCECONFIG), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONFIGURATION, RECORDINGSOURCECONFIG), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetTrackConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONFIGURATION, RECORDINGSOURCECONFIG), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.SetTrackConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONFIGURATION, RECORDINGSOURCECONFIG), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.SetRecordingJobConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONFIGURATION, RECORDINGSOURCECONFIG), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.GetRecordingJobConfiguration, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONFIGURATION, RECORDINGSOURCECONFIG), Features = new Feature[]{Feature.RecordingControlService}},
                            new FunctionalityItem(){Functionality = Functionality.RecordingConfigRecordingConfigEvent, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONFIGURATION, RECORDINGSOURCECONFIG),  Features = new Feature[]{ Feature.RecordingControlService, Feature.RecordingConfigRecordingConfigurationEvent }},
                            new FunctionalityItem(){Functionality = Functionality.RecordingConfigTrackConfigEvent, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONFIGURATION, RECORDINGSOURCECONFIG),  Features = new Feature[]{ Feature.RecordingControlService, Feature.RecordingConfigTrackConfigurationEvent }},
                            new FunctionalityItem(){Functionality = Functionality.RecordingConfigRecordingJobConfigEvent, Path = GetFullPath(ROOTDEVICEMANDATORY, RECORDINGCONFIGURATION, RECORDINGSOURCECONFIG),  Features = new Feature[]{ Feature.RecordingControlService, Feature.RecordingConfigRecordingJobConfigurationEvent }},

                                                        
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
                            new FunctionalityItem(){Functionality = Functionality.SetSystemFactoryDefault, Path = GetFullPath(ROOTDEVICEMANDATORY, SYSTEM)},
                            new FunctionalityItem(){Functionality = Functionality.Reboot, Path = GetFullPath(ROOTDEVICEMANDATORY, SYSTEM)},
                                // User handling
                            new FunctionalityItem(){Functionality = Functionality.GetUsers, Path = GetFullPath(ROOTDEVICEMANDATORY, USERHANDLING)},
                            new FunctionalityItem(){Functionality = Functionality.CreateUsers, Path = GetFullPath(ROOTDEVICEMANDATORY, USERHANDLING)},
                            new FunctionalityItem(){Functionality = Functionality.DeleteUsers, Path = GetFullPath(ROOTDEVICEMANDATORY, USERHANDLING)},
                            new FunctionalityItem(){Functionality = Functionality.SetUser, Path = GetFullPath(ROOTDEVICEMANDATORY, USERHANDLING)},
                                // Event handling
                            //new FunctionalityItem(){Functionality = Functionality.Notify, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT), Features = new Feature[]{Feature.WSBasicNotification}},
                            //new FunctionalityItem(){Functionality = Functionality.Subscribe, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT), Features = new Feature[]{Feature.WSBasicNotification}},
                            new FunctionalityItem(){Functionality = Functionality.Renew, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.Unsubscribe, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.EventsSetSynchronizationPoint, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.CreatePullPointSubscription, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.PullMessages, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.GetEventProperties, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.TopicFilter, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.MessageContentFilter, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.AtLeastTwoPullPointSubscription, Path = GetFullPath(ROOTDEVICEMANDATORY, EVENT), Features = new Feature[]{Feature.MaxPullPoints}}
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

        //public string Scope
        //{
        //    get { return STORAGE_PROFILE_SCOPE; }
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

                checkNextMandatory(Feature.GetServices, "GetServices");

                LogMandatoryFeature(sb, "Discovery");
                checkNextMandatory(Feature.DiscoveryTypesTdsDevice, "Discovery/Types/tds:Device");

                LogMandatoryFeature(sb, "Network Configuration");
                LogMandatoryFeature(sb, "System");
                LogMandatoryFeature(sb, "User Handling");
                LogMandatoryFeature(sb, "Event Handling");

                checkNextMandatory(Feature.MaxPullPoints, "EventService/GetServiceCapabilities/MaxPullPoints");
                if (profileOk)
                {
                    var v = (int)parameters["MaxPullPoints"];
                    profileOk = v >= 2;
                    sb.AppendLine(string.Format("EventService/GetServiceCapabilities/MaxPullPoints has value >= 2: {0}", profileOk ? "SUPPORTED" : "NOT SUPPORTED"));
                }

                checkNextMandatory(Feature.Digest, "HTTP Digest");

                checkNextMandatory(Feature.RecordingSearchService, "Recording Search");
                checkNextOptional(Feature.MetadataSearch, "Metadata Search");
                checkNextOptional(Feature.PTZPositionSearch, "PTZ Position Search");
                checkNextMandatory(Feature.ReplayService, "Replay Service");
                checkNextOptional(Feature.ReverseReplay, "Reverse Replay");
                checkNextMandatory(Feature.RecordingControlService, "Recording Control");

                checkNextMandatory(Feature.RecordingOptions, "Get Recording Options");

                checkNextMandatory(Feature.MediaOrReceiver, "Media or Receiver Service");

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
            _features.Add(new ProfileFeature() { Feature = Feature.PTZPositionSearch, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.RecordingOptions, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.ReverseReplay, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.ReplayService, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.RecordingControlService, State = ProfileFeatureState.Mandatory });

            _features.Add(new ProfileFeature() { Feature = Feature.DynamicRecordings, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.MetadataSearch, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.MediaOrReceiver, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.MediaService, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.ReceiverService, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.RecordingConfigRecordingConfigurationEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.RecordingConfigRecordingJobConfigurationEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.RecordingConfigTrackConfigurationEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.RecordingConfigDeleteTrackDataEvent, State = ProfileFeatureState.Optional });
        }

        void InitDiscoveryTypes()
        {
            var list = new List<Feature> {Feature.DiscoveryTypesTdsDevice};

            MandatoryDiscoveryTypes = list;
        }

    }

}
