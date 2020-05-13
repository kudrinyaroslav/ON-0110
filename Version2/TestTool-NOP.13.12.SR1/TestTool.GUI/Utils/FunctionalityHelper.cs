using System.Collections.Generic;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Utility methods for "Functionality" test property.
    /// </summary>
    class FunctionalityHelper
    {
        /// <summary>
        /// Gets display name
        /// </summary>
        /// <param name="functionality"></param>
        /// <returns></returns>
        public static string GetDisplayName(Functionality functionality)
        {
            if (_functionalityInfos == null)
            {
                InitDisplayNames();
            }

            if (_functionalityInfos.ContainsKey(functionality))
            {
                var funcInfo = _functionalityInfos[functionality];
                var funcName = string.IsNullOrEmpty(funcInfo.Name) ? functionality.ToString() : funcInfo.Name;

                if (string.IsNullOrEmpty(funcInfo.ServiceName))
                    return funcName;

                return string.Format("{0} ({1})", funcName, funcInfo.ServiceName);
            }
            else
            {
                return functionality.ToString();
            }
        }

        class FunctionalityInfo
        {
            public string Name { get; set;  }
            public string ServiceName { get; set; }
        }

        private static Dictionary<Functionality, FunctionalityInfo> _functionalityInfos;

        private static string MediaServiceName = "Media";
        private static string AccessControlServiceName = "Access Control";
        private static string DoorControlServiceName = "Door Control";
        private static string DeviceServiceName = "Device";
        private static string DeviceIOServiceName = "Device IO";
        private static string EventServiceName = "Event";
        private static string PTZServiceName = "PTZ";
        private static string RecordingControlServiceName = "Recording Control";
        private static string ReplayServiceName = "Replay";
        private static string RecordingSearchServiceName = "Recording Search";
        private static string ReceiverServiceName = "Receiver";
        private static string IOServiceName = "IO";
        /// <summary>
        /// Initializes disply names where "ToString" is not OK.
        /// </summary>
        static void InitDisplayNames()
        {
            _functionalityInfos = new Dictionary<Functionality, FunctionalityInfo>();

            _functionalityInfos.Add(Functionality.GetServices, new FunctionalityInfo() { Name = "GetServices", ServiceName = DeviceServiceName });

            _functionalityInfos.Add(Functionality.GetDeviceServiceCapabilities, new FunctionalityInfo() { Name = "GetServiceCapabilities", ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.GetDeviceIOServiceCapabilities, new FunctionalityInfo() { Name = "GetServiceCapabilities", ServiceName = DeviceIOServiceName });
            _functionalityInfos.Add(Functionality.GetEventsServiceCapabilities, new FunctionalityInfo() { Name = "GetServiceCapabilities", ServiceName = EventServiceName });
            _functionalityInfos.Add(Functionality.GetMediaServiceCapabilities, new FunctionalityInfo() { Name = "GetServiceCapabilities", ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetPTZServiceCapabilities, new FunctionalityInfo() { Name = "GetServiceCapabilities", ServiceName = PTZServiceName });
            _functionalityInfos.Add(Functionality.GetRecordingServiceCapabilities, new FunctionalityInfo() { Name = "GetServiceCapabilities", ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.GetReplayServiceCapabilities, new FunctionalityInfo() { Name = "GetServiceCapabilities", ServiceName = ReplayServiceName });
            _functionalityInfos.Add(Functionality.GetSearchServiceCapabilities, new FunctionalityInfo() { Name = "GetServiceCapabilities", ServiceName = RecordingSearchServiceName });
            _functionalityInfos.Add(Functionality.GetReceiverServiceCapabilities, new FunctionalityInfo() { Name = "GetServiceCapabilities)", ServiceName = ReceiverServiceName });
            _functionalityInfos.Add(Functionality.GetWsdlUrl, new FunctionalityInfo() { Name = "GetWsdlUrl", ServiceName = DeviceServiceName });

            _functionalityInfos.Add(Functionality.WsSecurity, new FunctionalityInfo() { Name = "WS-UsernameToken Authentication" });
            _functionalityInfos.Add(Functionality.DigestAuthentication, new FunctionalityInfo() { Name = "HTTP Digest Authentication" });
            _functionalityInfos.Add(Functionality.MediaSetSynchronizationPoint, new FunctionalityInfo() { Name = "SetSynchronizationPoint" });
            _functionalityInfos.Add(Functionality.MediaStreamingRtspJpegHeaderExtension, new FunctionalityInfo() { Name = "Media Streaming RTSP  (JPEG RTP Header Extention)" });
            _functionalityInfos.Add(Functionality.GetPtzConfigurations, new FunctionalityInfo() { Name = "GetConfigurations" });
            _functionalityInfos.Add(Functionality.GetPtzConfiguration, new FunctionalityInfo() { Name = "GetConfiguration" });
            _functionalityInfos.Add(Functionality.GetPtzConfigurationOptions, new FunctionalityInfo() { Name = "GetConfigurationOptions" });
            _functionalityInfos.Add(Functionality.AddPTZConfiguration, new FunctionalityInfo() { Name = "AddPTZConfiguration" });
            _functionalityInfos.Add(Functionality.RemovePTZConfiguration, new FunctionalityInfo() { Name = "RemovePTZConfiguration" });
            _functionalityInfos.Add(Functionality.SetPtzConfiguration, new FunctionalityInfo() { Name = "SetConfiguration" });
            _functionalityInfos.Add(Functionality.PtzStop, new FunctionalityInfo() { Name = "Stop" });
            _functionalityInfos.Add(Functionality.PtzGetStatus, new FunctionalityInfo() { Name = "GetStatus" });
            _functionalityInfos.Add(Functionality.PtzAbsoluteMove, new FunctionalityInfo() { Name = "AbsoluteMove" });
            _functionalityInfos.Add(Functionality.PtzRelativeMove, new FunctionalityInfo() { Name = "RelativeMove" });
            _functionalityInfos.Add(Functionality.PtzGetNodes, new FunctionalityInfo() { Name = "GetNodes" });
            _functionalityInfos.Add(Functionality.PtzGetNode, new FunctionalityInfo() { Name = "GetNode" });
            _functionalityInfos.Add(Functionality.PtzContinuousMove, new FunctionalityInfo() { Name = "ContinuousMove" });
            _functionalityInfos.Add(Functionality.PtzSetPreset, new FunctionalityInfo() { Name = "SetPreset" });
            _functionalityInfos.Add(Functionality.PtzGetPreset, new FunctionalityInfo() { Name = "GetPreset" });
            _functionalityInfos.Add(Functionality.PtzGotoPreset, new FunctionalityInfo() { Name = "GotoPreset" });
            _functionalityInfos.Add(Functionality.PtzRemovePreset, new FunctionalityInfo() { Name = "RemovePreset" });

            _functionalityInfos.Add(Functionality.XPathDialect, new FunctionalityInfo() { Name = "XPath Dialect" });
            _functionalityInfos.Add(Functionality.RecordingStateEvent, new FunctionalityInfo() { Name = "tns1:RecordingHistory/Recording/State" });
            _functionalityInfos.Add(Functionality.TrackStateEvent, new FunctionalityInfo() { Name = "tns1:RecordingHistory/Track/State" });

            _functionalityInfos.Add(Functionality.CreateRecording, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.DeleteRecording, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.CreateRecordingEvent, new FunctionalityInfo() { Name = "tns1:RecordingConfig/CreateRecording" });
            _functionalityInfos.Add(Functionality.DeleteRecordingEvent, new FunctionalityInfo() { Name = "tns1:RecordingConfig/DeleteRecording" });
            _functionalityInfos.Add(Functionality.CreateTrack, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.DeleteTrack, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.CreateTrackEvent, new FunctionalityInfo() { Name = "tns1:RecordingConfig/CreateTrack" });
            _functionalityInfos.Add(Functionality.DeleteTrackEvent, new FunctionalityInfo() { Name = "tns1:RecordingConfig/DeleteTrack" });

            _functionalityInfos.Add(Functionality.ReverseReplay, new FunctionalityInfo() { Name = "Reverse Replay" });
            _functionalityInfos.Add(Functionality.MediaReplay, new FunctionalityInfo() { Name = "Media Replay" });
            _functionalityInfos.Add(Functionality.RTSPFeatureTag, new FunctionalityInfo() { Name = "\"onvif-replay\" RTSP feature tag" });

            _functionalityInfos.Add(Functionality.GetRecordings, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.CreateRecordingJob, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.DeleteRecordingJob, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.GetRecordingJobs, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.GetRecordingJobState, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.SetRecordingJobMode, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.GetRecordingOptions, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.RecordingJobStateChangeEvent, new FunctionalityInfo() { Name = "tns1:RecordingConfig/JobState" });
            _functionalityInfos.Add(Functionality.DeleteTrackDataEvent, new FunctionalityInfo() { Name = "tns1:RecordingConfig/DeleteTrackData" });

            _functionalityInfos.Add(Functionality.SetRecordingConfiguration, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.GetRecordingConfiguration, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.SetTrackConfiguration, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.GetTrackConfiguration, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.SetRecordingJobConfiguration, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.GetRecordingJobConfiguration, new FunctionalityInfo() { ServiceName = RecordingControlServiceName });
            _functionalityInfos.Add(Functionality.RecordingConfigRecordingConfigEvent, new FunctionalityInfo() { Name = "tns1:RecordingConfig/RecordingConfiguration" });
            _functionalityInfos.Add(Functionality.RecordingConfigTrackConfigEvent, new FunctionalityInfo() { Name = "tns1:RecordingConfig/TrackConfiguration" });
            _functionalityInfos.Add(Functionality.RecordingConfigRecordingJobConfigEvent, new FunctionalityInfo() { Name = "tns1:RecordingConfig/RecordingJobConfiguration" });

            // template for copy-paste
            _functionalityInfos.Add(Functionality.DoorModeEvent, new FunctionalityInfo() { Name = "tns1:Door/State/DoorMode" });
            _functionalityInfos.Add(Functionality.DoorPhysicalStateEvent, new FunctionalityInfo() { Name = "tns1:Door/State/DoorPhysicalState" });
            _functionalityInfos.Add(Functionality.DoubleLockPhysicalStateEvent, new FunctionalityInfo() { Name = "tns1:Door/State/LockPhysicalState" });
            _functionalityInfos.Add(Functionality.LockPhysicalStateEvent, new FunctionalityInfo() { Name = "tns1:Door/State/DoubleLockPhysicalState" });
            _functionalityInfos.Add(Functionality.DoorTamperEvent, new FunctionalityInfo() { Name = "tns1:Door/State/DoorTamper" });
            _functionalityInfos.Add(Functionality.DoorAlarmEvent, new FunctionalityInfo() { Name = "tns1:Door/State/DoorAlarm" });
            _functionalityInfos.Add(Functionality.DoorFaultEvent, new FunctionalityInfo() { Name = "tns1:Door/State/DoorFault" });
            _functionalityInfos.Add(Functionality.DoorChangedEvent, new FunctionalityInfo() { Name = "tns1:Configuration/Door/Changed" });
            _functionalityInfos.Add(Functionality.DoorRemovedEvent, new FunctionalityInfo() { Name = "tns1:Configuration/Door/Removed" });
            _functionalityInfos.Add(Functionality.AccessGrantedAnonymousEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/AccessGranted/Anonymous" });
            _functionalityInfos.Add(Functionality.AccessGrantedCredentialEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/AccessGranted/Credential" });
            _functionalityInfos.Add(Functionality.AccessTakenAnonymousEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/AccessTaken/Anonymous" });
            _functionalityInfos.Add(Functionality.AccessTakenCredentialEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/AccessTaken/Credential" });
            _functionalityInfos.Add(Functionality.AccessNotTakenAnonymousEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/AccessNotTaken/Anonymous" });
            _functionalityInfos.Add(Functionality.AccessNotTakenCredentialEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/AccessNotTaken/Credential" });
            _functionalityInfos.Add(Functionality.AccessDeniedWithCredentialEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/Denied/Credential" });
            _functionalityInfos.Add(Functionality.AccessDeniedToAnonymousEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/Denied/Anonymous" });
            _functionalityInfos.Add(Functionality.AccessDeniedCredentialCredentialNotFoundCardEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/Denied/CredentialNotFound/Card" });
            _functionalityInfos.Add(Functionality.DuressEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/Duress" });
            _functionalityInfos.Add(Functionality.RequestAnonymousEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/Request/Anonymous" });
            _functionalityInfos.Add(Functionality.RequestCredentialEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/Request/Credential" });
            _functionalityInfos.Add(Functionality.RequestTimeoutEvent, new FunctionalityInfo() { Name = "tns1:AccessControl/Request/Timeout" });

            _functionalityInfos.Add(Functionality.GetAccessControlServiceCapabilities, new FunctionalityInfo() { Name = "GetServiceCapabilities", ServiceName = AccessControlServiceName });
            _functionalityInfos.Add(Functionality.GetAccessPointInfoList, new FunctionalityInfo() { Name = "GetAccessPointInfoList", ServiceName = AccessControlServiceName });
            _functionalityInfos.Add(Functionality.GetAccessPointInfo, new FunctionalityInfo() { Name = "GetAccessPointInfo", ServiceName = AccessControlServiceName });
            _functionalityInfos.Add(Functionality.GetAreaInfoList, new FunctionalityInfo() { Name = "GetAreaInfoList", ServiceName = AccessControlServiceName });
            _functionalityInfos.Add(Functionality.GetAreaInfo, new FunctionalityInfo() { Name = "GetAreaInfo", ServiceName = AccessControlServiceName });
            _functionalityInfos.Add(Functionality.GetAccessPointState, new FunctionalityInfo() { Name = "GetAccessPointState", ServiceName = AccessControlServiceName });

            _functionalityInfos.Add(Functionality.EnableAccessPoint, new FunctionalityInfo() { Name = "EnableAccessPoint", ServiceName = AccessControlServiceName });
            _functionalityInfos.Add(Functionality.DisableAccessPoint, new FunctionalityInfo() { Name = "DisableAccessPoint", ServiceName = AccessControlServiceName });

            _functionalityInfos.Add(Functionality.ExternalAutorization, new FunctionalityInfo() { Name = "ExternalAutorization", ServiceName = AccessControlServiceName });

            _functionalityInfos.Add(Functionality.AccessPointChangedEvent, new FunctionalityInfo() { Name = "tns1:Configuration/AccessPoint/Changed" });
            _functionalityInfos.Add(Functionality.AccessPointRemovedEvent, new FunctionalityInfo() { Name = "tns1:Configuration/AccessPoint/Removed" });
            _functionalityInfos.Add(Functionality.AccessPointEnabledEvent, new FunctionalityInfo() { Name = "tns1:AccessPoint/State/Enabled" });
            _functionalityInfos.Add(Functionality.AreaChangedEvent, new FunctionalityInfo() { Name = "tns1:Configuration/Area/Changed" });
            _functionalityInfos.Add(Functionality.AreaRemovedEvent, new FunctionalityInfo() { Name = "tns1:Configuration/Area/Removed" });

            _functionalityInfos.Add(Functionality.GetDoorControlServiceCapabilities, new FunctionalityInfo() { Name = "GetServiceCapabilities", ServiceName = DoorControlServiceName });
            _functionalityInfos.Add(Functionality.GetDoorInfoList, new FunctionalityInfo() { Name = "GetDoorInfoList", ServiceName = DoorControlServiceName });
            _functionalityInfos.Add(Functionality.GetDoorInfo, new FunctionalityInfo() { Name = "GetDoorInfo", ServiceName = DoorControlServiceName });
            _functionalityInfos.Add(Functionality.GetDoorState, new FunctionalityInfo() { Name = "GetDoorState", ServiceName = DoorControlServiceName });

            _functionalityInfos.Add(Functionality.AccessDoor, new FunctionalityInfo() { Name = "AccessDoor", ServiceName = DoorControlServiceName });
            _functionalityInfos.Add(Functionality.LockDoor, new FunctionalityInfo() { Name = "LockDoor", ServiceName = DoorControlServiceName });
            _functionalityInfos.Add(Functionality.UnlockDoor, new FunctionalityInfo() { Name = "UnlockDoor", ServiceName = DoorControlServiceName });
            _functionalityInfos.Add(Functionality.DoubleLockDoor, new FunctionalityInfo() { Name = "DoubleLockDoor", ServiceName = DoorControlServiceName });
            _functionalityInfos.Add(Functionality.BlockDoor, new FunctionalityInfo() { Name = "BlockDoor", ServiceName = DoorControlServiceName });
            _functionalityInfos.Add(Functionality.LockDownDoor, new FunctionalityInfo() { Name = "LockDownDoor", ServiceName = DoorControlServiceName });
            _functionalityInfos.Add(Functionality.LockDownReleaseDoor, new FunctionalityInfo() { Name = "LockDownReleaseDoor", ServiceName = DoorControlServiceName });
            _functionalityInfos.Add(Functionality.LockOpenDoor, new FunctionalityInfo() { Name = "LockOpenDoor", ServiceName = DoorControlServiceName });
            _functionalityInfos.Add(Functionality.LockOpenReleaseDoor, new FunctionalityInfo() { Name = "LockOpenReleaseDoor", ServiceName = DoorControlServiceName });

            _functionalityInfos.Add(Functionality.PersistentNotificationStorage, new FunctionalityInfo() { Name = "Seek", ServiceName = EventServiceName });

            _functionalityInfos.Add(Functionality.Renew, new FunctionalityInfo() { ServiceName = EventServiceName });
            _functionalityInfos.Add(Functionality.Unsubscribe, new FunctionalityInfo() { ServiceName = EventServiceName });
            _functionalityInfos.Add(Functionality.EventsSetSynchronizationPoint, new FunctionalityInfo() { Name = "SetSynchronizationPoint", ServiceName = EventServiceName });
            _functionalityInfos.Add(Functionality.CreatePullPointSubscription, new FunctionalityInfo() { ServiceName = EventServiceName });
            _functionalityInfos.Add(Functionality.PullMessages, new FunctionalityInfo() { ServiceName = EventServiceName });
            _functionalityInfos.Add(Functionality.GetEventProperties, new FunctionalityInfo() { ServiceName = EventServiceName });
            _functionalityInfos.Add(Functionality.TopicFilter, new FunctionalityInfo() { ServiceName = EventServiceName });
            
            _functionalityInfos.Add(Functionality.GetIOServiceCapabilities, new FunctionalityInfo() { Name = "GetServiceCapabilities", ServiceName = IOServiceName });
            _functionalityInfos.Add(Functionality.IOGetRelayOutputs, new FunctionalityInfo() { Name = "GetRelayOutputs" });
            _functionalityInfos.Add(Functionality.IOSetRelayOutputOptions, new FunctionalityInfo() { Name = "SetRelayOutputOptions" });
            _functionalityInfos.Add(Functionality.IOSetRelayOutputSettings, new FunctionalityInfo() { Name = "SetRelayOutputSettings" });
            _functionalityInfos.Add(Functionality.IOSetRelayOutputState, new FunctionalityInfo() { Name = "SetRelayOutputState" });


            _functionalityInfos.Add(Functionality.MessageContentFilter, new FunctionalityInfo() { Name = "Message Content Filter" });
            _functionalityInfos.Add(Functionality.MaxPullPoints, new FunctionalityInfo() { Name = "MaxPullPoints capability" });

            _functionalityInfos.Add(Functionality.GetIPAddressFilter, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.SetIPAddressFilter, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.AddIPAddressFilter, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.RemoveIPAddressFilter, new FunctionalityInfo() { ServiceName = DeviceServiceName });

            _functionalityInfos.Add(Functionality.WSDiscovery, new FunctionalityInfo() { Name = "WS-Discovery" });
            _functionalityInfos.Add(Functionality.GetDiscoveryMode, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.SetDiscoveryMode, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.GetScopes, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.SetScopes, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.AddScopes, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.RemoveScopes, new FunctionalityInfo() { ServiceName = DeviceServiceName });

            _functionalityInfos.Add(Functionality.GetHostname, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.SetHostname, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.GetDns, new FunctionalityInfo() { Name = "GetDNS", ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.SetDns, new FunctionalityInfo() { Name = "SetDNS", ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.GetNetworkInterfaces, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.SetNetworkInterfaces, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.GetNetworkProtocols, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.SetNetworkProtocols, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.GetNetworkDefaultGateway, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.SetNetworkDefaultGateway, new FunctionalityInfo() { ServiceName = DeviceServiceName });

            _functionalityInfos.Add(Functionality.GetDeviceInformation, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.GetSystemDateAndTime, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.SetSystemDateAndTime, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.SetSystemFactoryDefault, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.Reboot, new FunctionalityInfo() { ServiceName = DeviceServiceName });

            _functionalityInfos.Add(Functionality.GetUsers, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.CreateUsers, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.DeleteUsers, new FunctionalityInfo() { ServiceName = DeviceServiceName });
            _functionalityInfos.Add(Functionality.SetUser, new FunctionalityInfo() { ServiceName = DeviceServiceName });

            _functionalityInfos.Add(Functionality.FindRecordings, new FunctionalityInfo() { ServiceName = RecordingSearchServiceName });
            _functionalityInfos.Add(Functionality.GetRecordingSearchResults, new FunctionalityInfo() { ServiceName = RecordingSearchServiceName });
            _functionalityInfos.Add(Functionality.EndSearch, new FunctionalityInfo() { ServiceName = RecordingSearchServiceName });
            _functionalityInfos.Add(Functionality.FindEvents, new FunctionalityInfo() { ServiceName = RecordingSearchServiceName });
            _functionalityInfos.Add(Functionality.GetEventSearchResults, new FunctionalityInfo() { ServiceName = RecordingSearchServiceName });
            _functionalityInfos.Add(Functionality.GetRecordingSummary, new FunctionalityInfo() { ServiceName = RecordingSearchServiceName });
            _functionalityInfos.Add(Functionality.GetRecordingInformation, new FunctionalityInfo() { ServiceName = RecordingSearchServiceName });
            _functionalityInfos.Add(Functionality.GetMediaAttributes, new FunctionalityInfo() { ServiceName = RecordingSearchServiceName });

            _functionalityInfos.Add(Functionality.FindMetadata, new FunctionalityInfo() { ServiceName = RecordingSearchServiceName });
            _functionalityInfos.Add(Functionality.GetMetadataSearchResults, new FunctionalityInfo() { ServiceName = RecordingSearchServiceName });

            _functionalityInfos.Add(Functionality.FindPTZPosition, new FunctionalityInfo() { ServiceName = RecordingSearchServiceName });
            _functionalityInfos.Add(Functionality.GetPTZPositionSearchResults, new FunctionalityInfo() { ServiceName = RecordingSearchServiceName });

            _functionalityInfos.Add(Functionality.GetStreamUri, new FunctionalityInfo() { ServiceName = ReplayServiceName });
            _functionalityInfos.Add(Functionality.SetReplayConfiguration, new FunctionalityInfo() { ServiceName = ReplayServiceName });
            _functionalityInfos.Add(Functionality.GetReplayConfiguration, new FunctionalityInfo() { ServiceName = ReplayServiceName });

            _functionalityInfos.Add(Functionality.GetProfiles, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetProfile, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.CreateProfile, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.DeleteProfile, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetVideoSources, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetVideoSourceConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetVideoSourceConfigurations, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.AddVideoSourceConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.RemoveVideoSourceConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.SetVideoSourceConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetCompatibleVideoSourceConfigurations, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetVideoSourceConfigurationOptions, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetVideoEncoderConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetVideoEncoderConfigurations, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.AddVideoEncoderConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.RemoveVideoEncoderConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.SetVideoEncoderConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetCompatibleVideoEncoderConfigurations, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetVideoEncoderConfigurationOptions, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetGuaranteedNumberOfVideoEncoderInstances, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetMetadataConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetMetadataConfigurations, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.AddMetadataConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.RemoveMetadataConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.SetMetadataConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetCompatibleMetadataConfigurations, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetMetadataConfigurationOptions, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetAudioSources, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetAudioSourceConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetAudioSourceConfigurations, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.AddAudioSourceConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.RemoveAudioSourceConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.SetAudioSourceConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetCompatibleAudioSourceConfigurations, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetAudioSourceConfigurationOptions, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetAudioEncoderConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetAudioEncoderConfigurations, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.AddAudioEncoderConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.RemoveAudioEncoderConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.SetAudioEncoderConfiguration, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetCompatibleAudioEncoderConfigurations, new FunctionalityInfo() { ServiceName = MediaServiceName });
            _functionalityInfos.Add(Functionality.GetAudioEncoderConfigurationOptions, new FunctionalityInfo() { ServiceName = MediaServiceName });

            _functionalityInfos.Add(Functionality.GetReceivers, new FunctionalityInfo() { ServiceName = ReceiverServiceName });
            _functionalityInfos.Add(Functionality.GetReceiver, new FunctionalityInfo() { ServiceName = ReceiverServiceName });
            _functionalityInfos.Add(Functionality.CreateReceiver, new FunctionalityInfo() { ServiceName = ReceiverServiceName });
            _functionalityInfos.Add(Functionality.DeleteReceiver, new FunctionalityInfo() { ServiceName = ReceiverServiceName });
            _functionalityInfos.Add(Functionality.ConfigureReceiver, new FunctionalityInfo() { ServiceName = ReceiverServiceName });
            _functionalityInfos.Add(Functionality.SetReceiverMode, new FunctionalityInfo() { ServiceName = ReceiverServiceName });
            _functionalityInfos.Add(Functionality.GetReceiverState, new FunctionalityInfo() { ServiceName = ReceiverServiceName });
            _functionalityInfos.Add(Functionality.ReceiverChangeStateEvent, new FunctionalityInfo() { Name = "tns1:Receiver/ChangeState" });
            _functionalityInfos.Add(Functionality.ReceiverConnectionFailedEvent, new FunctionalityInfo() { Name = "tns1:Receiver/ConnectionFailed" });
            _functionalityInfos.Add(Functionality.MediaStreamingRtsp, new FunctionalityInfo() { Name = "Media Streaming using RTSP " });

        }
    }
}
