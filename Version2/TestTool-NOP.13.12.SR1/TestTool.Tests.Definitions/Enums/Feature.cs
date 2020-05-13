///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.Definitions.Enums
{
    /// <summary>
    /// Features.
    /// </summary>
    public enum Feature
    {
        Discovery,
        BYE,
        Security,
        WSU,
        Digest,
        Capabilities,
        GetServices,
        GetCapabilities,
        DeviceService,
        Network,
        NTP,
        IPv6,
        DHCPv6,
        ZeroConfiguration,
        DynamicDNS,
        IPFilter,
        System,
        SystemLogging,
        DeviceIO,
        DeviceIORelayOutputs,
        DeviceIORelayOutputsBistable,
        DeviceIORelayOutputsBistableOpen,
        DeviceIORelayOutputsBistableClosed,
        DeviceIORelayOutputsMonostable,
        DeviceIORelayOutputsMonostableOpen,
        DeviceIORelayOutputsMonostableClosed,
        EventsService,
        EventsServiceCapabilities,
        //EventSeek,
        MediaService,
        Video,
        JPEG,
        H264,
        MPEG4,
        H264OrMPEG4,
        Audio,
        G711,
        G726,
        AAC,
        RTSS,
        RTPUDP,
        RTPRTSPHTTP,
        RTPRTSPTCP,
        RTPMulticastUDP,
        SnapshotUri,
        AudioOutput,
        AudioOutputG711,
        AudioOutputG726,
        AudioOutputAAC,
        PTZService,
        PTZAbsolute,
        PTZAbsolutePanTilt,
        PTZAbsoluteZoom,
        PTZRelative,
        PTZRelativePanTilt,
        PTZRelativeZoom,
        PTZAbsoluteOrRelative,
        PTZContinious,
        PTZContinuousPanTilt,
        PTZContinuousZoom,
        PTZAbsoluteOrRelativePanTilt,
        PTZAbsoluteOrRelativeZoom,
        PTZPresets,
        PTZHome,
        PTZConfigurableHome,
        PTZFixedHome,
        PTZAuxiliary,
        PTZSpeed,
        PTZSpeedPanTilt,
        PTZSpeedZoom,
        DeviceIoService,
        RelayOutputs,
        DigitalInputs,
        ImagingService,
        AnalyticsService,
        DoorControlService,
        DoorEntity,
        AccessDoor,
        BlockDoor,
        LockDoor,
        UnlockDoor,
        DoubleLockDoor,
        LockDownDoor,
        LockOpenDoor,
        DoorMonitor,
        LockMonitor,
        DoubleLockMonitor,
        DoorAlarm,
        DoorTamper,
        DoorFault,
        DoorControlEvents,
        DoorModeEvent,
        DoorPhysicalStateEvent,
        DoubleLockPhysicalStateEvent,
        LockPhysicalStateEvent,
        DoorTamperEvent,
        DoorAlarmEvent,
        DoorChangedEvent,
        DoorRemovedEvent,
        DoorFaultEvent,
        UserService,
        AccessControlService,
        AreaEntity,
        AccessPointEntity,
        EnableDisableAccessPoint,
        AccessTaken,
        ExternalAuthorization,
        AnonymousAccess,
        Duress,
        //Tamper,
        AccessControlEvents,
        AccessGrantedAnonymousEvent,
        AccessGrantedCredentialEvent,
        //AccessGrantedAnonymousEvent,
        //AccessGrantedCredentialEvent,
        AccessTakenAnonymousEvent,
        AccessTakenCredentialEvent,
        AccessNotTakenAnonymousEvent,
        AccessNotTakenCredentialEvent,
        //AccessDeniedCredentialCredentialNotEnabledEvent,
        //AccessDeniedCredentialCredentialNotActiveEvent,
        //AccessDeniedCredentialCredentialExpiredEvent,
        //AccessDeniedCredentialInvalidPINEvent,
        //AccessDeniedCredentialNotPermittedAtThisTimeEvent,
        //AccessDeniedCredentialUnauthorizedEvent,
        //AccessDeniedWithCredentialEvent,
        //AccessDeniedCredentialOtherEvent,
        AccessDeniedCredentialEvent,
        //AccessDeniedAnonymousNotPermittedAtThisTimeEvent,
        //AccessDeniedAnonymousUnauthorizedEvent,
        //AccessDeniedAnonymousExternalEvent,
        //AccessDeniedAnonymousOtherEvent,
        AccessDeniedAnonymousEvent,
        AccessDeniedCredentialCredentialNotFoundCardEvent,
        DuressEvent,
        RequestAnonymousEvent,
        RequestCredentialEvent,
        //RequestTimeoutEvent,
        //RequestTimeoutEvent,
        RequestTimeoutEvent,
        AccessPointStateEnabledEvent,
        //AccessPointSetEvent,
        AccessPointChangedEvent,
        AccessPointRemovedEvent,
        //AccessPointEnabledEvent,
        //AccessPointTamperingEvent,
        //AreaSetEvent,
        AreaChangedEvent,
        AreaRemovedEvent,
        //HostResponse,
        RecordingSearchService,
        ReceiverService,
        ReplayService,
        ReplayServiceRTPRTSPTCP,
        ReverseReplay,
        RecordingControlService,
        RecordingConfigRecordingConfigurationEvent,
        RecordingConfigRecordingJobConfigurationEvent,
        RecordingConfigTrackConfigurationEvent,
        RecordingConfigDeleteTrackDataEvent,
        RecordingOptions,
        AudioRecording,
        MetadataRecording,
        MetadataSearch,
        PTZPositionSearch,
        //ProfileAsSource,
        ReceiverAsSource,
        MediaOrReceiver,
        DynamicRecordings,
        DynamicTracks,
        DynamicRecordingsOrDynamicTracks,
        PersistentNotificationStorage,
        WSBasicNotification,

        DiscoveryTypes,
        DiscoveryTypesDnNetworkVideoTransmitter,
        DiscoveryTypesTdsDevice,

        MaxPullPoints,

        AdvancedSecurity,
        KeyStoreFeaturesSupport,
        RSAKeyPairGeneration,
        PKCS10ExternalCertificationWithRSA,
        SelfSignedCertificateCreationWithRSA,
        TLSFeaturesSupport,
        TLSServerSupport
    }
}
