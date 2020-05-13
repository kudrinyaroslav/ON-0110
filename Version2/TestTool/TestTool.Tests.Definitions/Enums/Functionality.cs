using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.Definitions.Enums
{
    /// <summary>
    /// Functionality for profile definitions
    /// </summary>
    public enum Functionality
    {
        GetCapabilities,
        GetServices,
        GetWsdlUrl,
        GetDeviceServiceCapabilities,
        GetDeviceIOServiceCapabilities,
        GetEventsServiceCapabilities,
        GetMediaServiceCapabilities,
        GetPTZServiceCapabilities,
        GetReceiverServiceCapabilities,
        GetReplayServiceCapabilities,
        GetRecordingServiceCapabilities,
        GetSearchServiceCapabilities,
        GetImagingServiceCapabilities,
        WSDiscovery,
        GetDiscoveryMode,
        SetDiscoveryMode,
        GetHostname,
        SetHostname,
        GetDns,
        SetDns,
        GetNetworkInterfaces,
        SetNetworkInterfaces,
        GetNetworkProtocols,
        SetNetworkProtocols,
        GetNetworkDefaultGateway,
        SetNetworkDefaultGateway,
        GetDeviceInformation,
        GetSystemLog,
        GetSystemURIs,
        StartSystemRestore,
        StartFirmwareUgrade,
        GetSystemDateAndTime,
        SetSystemDateAndTime,
        SetSystemFactoryDefault,
        Reboot,
        GetScopes,
        SetScopes,
        AddScopes,
        RemoveScopes,
        GetUsers,
        CreateUsers,
        DeleteUsers,
        SetUser,
        Notify,
        Subscribe,
        Renew,
        Unsubscribe,
        EventsSetSynchronizationPoint,
        CreatePullPointSubscription,
        PullMessages,
        GetEventProperties,
        TopicFilter,
        MessageContentFilter,
        GetNTP,
        SetNTP,
        GetDynamicDNS,
        SetDynamicDNS,
        GetZeroConfiguration,
        SetZeroConfiguration,
        GetIPAddressFilter,
        SetIPAddressFilter,
        AddIPAddressFilter,
        RemoveIPAddressFilter,
        GetRemoteUser,
        SetRemoteUser,
        WsSecurity, //UsernameToken Authentication
        DigestAuthentication,
        GetAccessPolicy,
        CreateProfile,
        DeleteProfile,
        GetProfiles,
        GetProfile,
        MediaSetSynchronizationPoint,
        GetStreamUri,
        MediaStreamingRtsp,
        MediaStreamingRtspJpegHeaderExtension,
        GetSnapshotUri,
        HttpGetSnapshot,
        GetVideoSources,
        GetVideoSourceConfiguration,
        GetVideoSourceConfigurations,
        AddVideoSourceConfiguration,
        RemoveVideoSourceConfiguration,
        SetVideoSourceConfiguration,
        GetCompatibleVideoSourceConfigurations,
        GetVideoSourceConfigurationOptions,
        GetVideoEncoderConfiguration,
        GetVideoEncoderConfigurations,
        AddVideoEncoderConfiguration,
        RemoveVideoEncoderConfiguration,
        SetVideoEncoderConfiguration,
        GetCompatibleVideoEncoderConfigurations,
        GetVideoEncoderConfigurationOptions,
        GetGuaranteedNumberOfVideoEncoderInstances,
        GetMetadataConfiguration,
        GetMetadataConfigurations,
        AddMetadataConfiguration,
        RemoveMetadataConfiguration,
        SetMetadataConfiguration,
        GetCompatibleMetadataConfigurations,
        GetMetadataConfigurationOptions,
        StartMulticastStreaming,
        StopMulticastStreaming,
        AddPTZConfiguration,
        RemovePTZConfiguration,
        PtzGetNodes,
        PtzGetNode,
        GetPtzConfigurations,
        GetPtzConfiguration,
        GetPtzConfigurationOptions,
        SetPtzConfiguration,
        PtzContinuousMove,
        PtzStop,
        PtzGetStatus,
        PtzSetPreset,
        PtzGetPreset,
        PtzGotoPreset,
        PtzRemovePreset,
        GotoHomePosition,
        SetHomePosition,
        PtzAbsoluteMove,
        PtzRelativeMove,
        SendAuxiliaryCommand,
        GetAudioSources,
        GetAudioSourceConfiguration,
        GetAudioSourceConfigurations,
        AddAudioSourceConfiguration,
        RemoveAudioSourceConfiguration,
        SetAudioSourceConfiguration,
        GetAudioOutputConfiguration,
        GetAudioOutputConfigurations,
        GetAudioOutputConfigurationOptions,
        GetCompatibleAudioOutputConfigurations,
        SetAudioOutputConfiguration,
        GetCompatibleAudioSourceConfigurations,
        GetAudioSourceConfigurationOptions,
        GetAudioEncoderConfiguration,
        GetAudioEncoderConfigurations,
        GetAudioDecoderConfiguration,
        GetAudioDecoderConfigurations,
        AddAudioEncoderConfiguration,
        AddAudioDecoderConfiguration,
        RemoveAudioEncoderConfiguration,
        RemoveAudioDecoderConfiguration,
        SetAudioEncoderConfiguration,
        GetCompatibleAudioEncoderConfigurations,
        GetCompatibleAudioDecoderConfigurations,
        GetAudioEncoderConfigurationOptions,
        GetAudioOutputs,
        GetRelayOutputs,
        SetRelayOutputSettings,
        SetRelayOutputState,
        // Recording search 
        GetRecordingSummary,
        GetRecordingInformation,
        GetMediaAttributes,
        FindRecordings,
        GetRecordingSearchResults,
        FindEvents,
        GetEventSearchResults,
        EndSearch,
        RecordingStateEvent, //Recording/State Event
        TrackStateEvent, //Track/State Event
        XPathDialect,
        // Replay Control  
        ReverseReplay,
        MediaReplay,
        RTPHeaderExtension,
        RTSPFeatureTag,
        GetReplayUri,
        SetReplayConfiguration,
        GetReplayConfiguration,
        //	Recording Control 
        GetRecordings,
        SetRecordingConfiguration,
        GetRecordingConfiguration,
        GetTrackConfiguration,
        SetTrackConfiguration,
        CreateRecordingJob,
        DeleteRecordingJob,
        GetRecordingJobs,
        SetRecordingJobConfiguration,
        GetRecordingJobConfiguration,
        SetRecordingJobMode,
        GetRecordingJobState,
        RecordingJobStateChangeEvent,
        RecordingConfigRecordingConfigEvent,
        RecordingConfigTrackConfigEvent,
        RecordingConfigRecordingJobConfigEvent,
        // Recording Control � Dynamic Recording 
        DeleteRecording,
        //RecordingCreationDeletionEvent, // RecordingCreation/Deletion Event
        CreateRecordingEvent,
        DeleteRecordingEvent,
        CreateRecording,
        CreateTrack,
        DeleteTrack,
        //TrackCreationDeletionEvent, // TrackCreation/Deletion Event
        CreateTrackEvent,
        DeleteTrackEvent,
        DeleteTrackDataEvent,
        GetRecordingOptions,
        //Recording Search � Metadata Search
        FindMetadata,
	    GetMetadataSearchResults,
        //Recording Search � PTZ Position Search
        FindPTZPosition,
	    GetPTZPositionSearchResults,
        // Receiver Configuration 
        GetReceivers,
        GetReceiver,
        GetReceiverState,
        CreateReceiver,
        DeleteReceiver,
        ConfigureReceiver,
        SetReceiverMode,
        SetReceiverState,
        ReceiverChangeStateEvent, // ChangeState Event
        ReceiverConnectionFailedEvent, // ConnectionFailed Event

        GetDoorControlServiceCapabilities,
        GetDoorInfo,
        GetDoorInfoList,
        GetDoorState,
        DoorModeEvent,
        DoorPhysicalStateEvent,
        DoubleLockPhysicalStateEvent,
        LockPhysicalStateEvent,
        DoorTamperEvent,
        DoorAlarmEvent,
        DoorFaultEvent,
        AccessDoor,
        LockDoor,
        UnlockDoor,
        DoubleLockDoor,
        BlockDoor,
        LockDownDoor,
        LockDownReleaseDoor,
        LockOpenDoor,
        LockOpenReleaseDoor,
        DoorChangedEvent,
        DoorRemovedEvent,
        GetUserServiceCapabilities,
        GetAccessControlServiceCapabilities,
        GetAccessPointInfo,
        GetAccessPointInfoList,
        GetAccessPointState,
        AccessPointEnabledEvent,
        GetAreaInfo,
        GetAreaInfoList,
        EnableAccessPoint,
        DisableAccessPoint,
        AccessGrantedCredentialEvent,
        AccessTakenAnonymousEvent,
        AccessTakenCredentialEvent,
        AccessNotTakenAnonymousEvent,
        AccessNotTakenCredentialEvent,
        AccessDeniedToAnonymousEvent,
        AccessDeniedCredentialCredentialNotFoundCardEvent,
        DuressEvent,
        ExternalAutorization,
        RequestAnonymousEvent,
        RequestCredentialEvent,
        AccessGrantedAnonymousEvent,
        AccessDeniedWithCredentialEvent,
        AccessDeniedAnonymousExternalEvent,
        RequestTimeoutEvent,
        AccessPointChangedEvent,
        AccessPointRemovedEvent,
        AreaChangedEvent,
        AreaRemovedEvent,
        PersistentNotificationStorage,
        GetIOServiceCapabilities,
        IOGetRelayOutputs,
        IOSetRelayOutputState,
        IOSetRelayOutputSettings,
        IOSetRelayOutputOptions,
        GetDigitalInputs,

        MaxPullPoints,
        AtLeastTwoPullPointSubscription,

        //Advanced Security Service functionality
        CreateRSAKeyPair, 
        GetKeyStatus,
        GetAllKeys,
        DeleteKey,
        CreatePKCS10CSR,
        CreateSelfSignedCertificate,
        UploadCertificate,
        GetCertificate,
        GetAllCertificates,
        DeleteCertificate,
        CreateCertificationPath,
        GetCertificationPath,
        GetAllCertificationPaths,
        DeleteCertificationPath,
        AddServerCertificateAssignment,
        ReplaceServerCertificateAssignment,
        GetAssignedServerCertificates,
        RemoveServerCertificateAssignment,
        GetAdvancedSecurityServiceCapabilities,
        UploadPassphrase,
        DeletePassphrase,
        UploadKeyPairInPKCS8,
        UploadCertificateWithPrivateKeyInPKCS12,
        GetAllPassphrases,
        AddDot1XConfiguration,
        GetAllDot1XConfigurations,
        UploadCRL,
        GetCRL,
        GetAllCRLs,
        DeleteCRL,
        CreateCertPathValidationPolicy,
        GetAllCertPathValidationPolicies,
        GetCertPathValidationPolicy,
        DeleteCertPathValidationPolicy,
        AddCertPathValidationPolicyAssignment,
        ReplaceCertPathValidationPolicyAssignment, 
        GetAssignedCertPathValidationPolicies,
        GetClientAuthenticationRequired, 
        SetClientAuthenticationRequired,

        MaxUsers,
        DefaultAccessPolicy,
    	MaximumUsernameLength,
	    MaximumPasswordLength,

        MonitoringProcessorUsageEvent,
        MonitoringOperatingTimeLastResetEvent,
        MonitoringOperatingTimeLastRebootEvent,
        MonitoringOperatingTimeLastClockSynchronizationEvent,
        MonitoringBackupLastEvent,
        DeviceHardwareFailureTemperatureCriticalEvent,
        DeviceHardwareFailureStorageFailureEvent,
        DeviceHardwareFailurePowerSupplyFailureEvent,
        DeviceHardwareFailureFanFailureEvent,

        FactoryDefaultStateIsSignaledByTheScope,
        AnonymousAccessInFactoryDefaultState,
        UserConfigurationInFactoryDefaultState,
        DynamicIPConfigurationEnabledInFactoryDefaultState,
        IPv4DHCPEnabledInFactoryDefaultState,
        IPv6StatelessAutoconfigurationEnabledInFactoryDefaultState,
        OperationalStateIsSignaledByTheScope,

        //Access Rules
        GetAccessRulesServiceCapabilities,
        GetAccessProfileInfo,
        GetAccessProfileInfoList,
        GetAccessProfiles,
        GetAccessProfileList,
        CreateAccessProfile,
        ModifyAccessProfile,
        DeleteAccessProfile,

        //Access Rules - Events
        ConfigurationAccessProfileChangedEvent,
        ConfigurationAccessProfileRemovedEvent,

        //Credential
        GetCredentialServiceCapabilities,
        GetCredentialInfo,
        GetCredentialInfoList,
        GetCredentials,
        GetCredentialList,
        CreateCredential,
        ModifyCredential,
        DeleteCredential,
        GetCredentialState,
        GetCredentialIdentifiers,
        SetCredentialIdentifier,
        DeleteCredentialIdentifier,
        GetSupportedFormatTypes,
        GetCredentialAccessProfiles,
        SetCredentialAccessProfiles,
        DeleteCredentialAccessProfiles,
        ResetAntipassbackViolation,
        EnableCredential,
        DisableCredential,

        //Credential - Events
        ConfigurationCredentialChangedEvent,
        ConfigurationCredentialRemovedEvent,
        CredentialStateEnabledEvent,
        CredentialStateApbViolationEvent,

        //Schedule
        GetScheduleServiceCapabilities,
        GetScheduleInfo,
        GetScheduleInfoList,
        GetSchedules,
        GetScheduleList,
        CreateSchedule,
        ModifySchedule,
        DeleteSchedule,
        GetSpecialDayGroupInfo,
        GetSpecialDayGroupInfoList,
        GetSpecialDayGroups,
        GetSpecialDayGroupList,
        CreateSpecialDayGroup,
        ModifySpecialDayGroup,
        DeleteSpecialDayGroup,
        GetScheduleState,

        //Schedule - Event
        ConfigurationScheduleChangedEvent,
        ConfigurationScheduleRemovedEvent,
        ConfigurationSpecialDaysChangedEvent,
        ConfigurationSpecialDaysRemovedEvent,
        ScheduleStateActiveEvent
    }

}