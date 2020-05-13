///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.GUI.Enums;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Enums;

namespace ClientTestTool.Tests.Engine.Enums
{
  /// <summary>
  /// Features
  /// </summary>
  public enum Feature
  {
    /// <summary>
    /// Unknown Feature
    /// </summary>
    Unknown,

    #region Core

    #region Security

    [Feature(Type = FeatureType.Parent)]
    Security,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Mandatory)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Optional)]
    [Feature(Type = FeatureType.Child, ParentFeature = Security)]
    [EnumDescription("Username Token")]
    UsernameToken,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Mandatory)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Mandatory)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = Security)]
    [EnumDescription("Http Digest")]
    HTTPDigest,

    #endregion

    #region Capabilities

    [Feature(Type = FeatureType.Parent)]
    Capabilities,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Mandatory)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Mandatory)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = Capabilities)]
    [EnumDescription("Get Services")]
    GetServices,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Mandatory)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Optional)]
    [Feature(Type = FeatureType.Child, ParentFeature = Capabilities)]
    [EnumDescription("Get Capabilities")]
    GetCapabilities,

    #endregion

    #region Event Handling

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Event Handling")]
    EventHandling,

    [Profile(Profile = Profile.S, RequirementLevel  = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel  = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel  = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = EventHandling)]
    [EnumDescription("Pull Point")]
    PullPoint,

    [Profile(Profile = Profile.S, RequirementLevel  = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel  = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel  = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = EventHandling)]
    [EnumDescription("Base Notification")]
    WSBaseNotification,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = EventHandling)]
    [EnumDescription("Metadata Streaming")]
    MetadataStreaming,

    #endregion

    #region Discovery

    [Feature(Type = FeatureType.Parent)]
    Discovery,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = Discovery)]
    [EnumDescription("WS-Discovery")]
    WSDiscovery,

    #endregion

    #region Network Configuration

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Network Configuration")]
    NetworkConfiguration,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = NetworkConfiguration)]
    [EnumDescription("Get Network Interfaces")]
    GetNetworkInterfaces,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = NetworkConfiguration)]
    [EnumDescription("Set Network Interfaces")]
    SetNetworkInterfaces,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = NetworkConfiguration)]
    [EnumDescription("Get Network Default Gateway")]
    GetNetworkDefaultGateway,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = NetworkConfiguration)]
    [EnumDescription("Set Network Default Gateway")]
    SetNetworkDefaultGateway,

    #endregion

    #region System

    [Feature(Type = FeatureType.Parent)]
    System,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = System)]
    [EnumDescription("Get Device Information")]
    GetDeviceInformation,

    #endregion

    #region UserHandling

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("User Handling")]
    UserHandling,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = UserHandling)]
    [EnumDescription("Create Users")]
    CreateUsers,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = UserHandling)]
    [EnumDescription("Get Users")]
    GetUsers,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = UserHandling)]
    [EnumDescription("Set User")]
    SetUser,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = UserHandling)]
    [EnumDescription("Delete Users")]
    DeleteUsers,

    #endregion 

    #region Relay Outputs

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Relay Outputs")]
    RelayOutputs,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Optional)]
    [Feature(Type = FeatureType.Child, ParentFeature = RelayOutputs)]
    [EnumDescription("Get Relay Outputs")]
    GetRelayOutputs,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Optional)]
    [Feature(Type = FeatureType.Child, ParentFeature = RelayOutputs)]
    [EnumDescription("Set Relay Output State")]
    SetRelayOutputState,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Optional)]
    [Feature(Type = FeatureType.Child, ParentFeature = RelayOutputs)]
    [EnumDescription("Set Relay Output Settings Bistable Mode")]
    SetRelayOutputBistable,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Optional)]
    [Feature(Type = FeatureType.Child, ParentFeature = RelayOutputs)]
    [EnumDescription("Set Relay Output Settings Monostable Mode")]
    SetRelayOutputMonostable,

    #endregion

    #region NTP

    [Feature(Type = FeatureType.Parent)]
    NTP,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Optional)]
    [Feature(Type = FeatureType.Child, ParentFeature = NTP)]
    [EnumDescription("Get NTP")]
    GetNTP,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Optional)]
    [Feature(Type = FeatureType.Child, ParentFeature = NTP)]
    [EnumDescription("Set NTP")]
    SetNTP,

    #endregion

    #region Dynamic DNS

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Dynamic DNS")]
    DynamicDns,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Optional)]
    [Feature(Type = FeatureType.Child, ParentFeature = DynamicDns)]
    [EnumDescription("Get Dynamic DNS")]
    GetDynamicDnsSettings,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Optional)]
    [Feature(Type = FeatureType.Child, ParentFeature = DynamicDns)]
    [EnumDescription("Set Dynamic DNS")]
    SetDynamicDnsSettings,

    #endregion

    #region Zero Configurations

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Zero Configuration")]
    ZeroConfiguration,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Optional)]
    [Feature(Type = FeatureType.Child, ParentFeature = ZeroConfiguration)]
    [EnumDescription("Get Zero Configuration")]
    GetZeroConfiguration,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Optional)]
    [Feature(Type = FeatureType.Child, ParentFeature = ZeroConfiguration)]
    [EnumDescription("Get Zero Configuration")]
    SetZeroConfiguration,

    #endregion

    #region IP Address Filtering

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("IP Address Filtering")]
    IPAddressFiltering,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = IPAddressFiltering)]
    [EnumDescription("Get Ip Address Filter")]
    GetIpAddressFilter,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = IPAddressFiltering)]
    [EnumDescription("Set IPv4 Address Filter")]
    SetIpV4AddressFilter,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = IPAddressFiltering)]
    [EnumDescription("Set IPv6 Address Filter")]
    SetIpV6AddressFilter,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = IPAddressFiltering)]
    [EnumDescription("Add IPv4 Address Filter")]
    AddIpV4AddressFilter,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = IPAddressFiltering)]
    [EnumDescription("Add IPv6 Address Filter")]
    AddIpV6AddressFilter,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = IPAddressFiltering)]
    [EnumDescription("Remove IPv4 Address Filter")]
    RemoveIpV4AddressFilter,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = IPAddressFiltering)]
    [EnumDescription("Remove IPv6 Address Filter")]
    RemoveIpV6AddressFilter,

    #endregion 

    #region Persistent Notification Storage Retrieval

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Persistent Notification Storage Retrieval")]
    PersistentNotificationStorageRetrieval,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Optional)]
    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PersistentNotificationStorageRetrieval)]
    Seek,

    #endregion

    #endregion

    #region Profile S

    #region Media Streaming

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Media Streaming")]
    MediaStreaming,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    //[Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    //[Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Mandatory)] //TODO
    [Feature(Type = FeatureType.Child, ParentFeature = MediaStreaming)]
    [EnumDescription("Get Profiles")]
    GetProfiles,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaStreaming)]
    [EnumDescription("Get Stream URI")]
    GetStreamURI,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaStreaming)]
    [EnumDescription("Streaming Over RTSP")]
    RTSPStreaming,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaStreaming)]
    [EnumDescription("Streaming Over UDP")]
    UDP,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaStreaming)]
    [EnumDescription("Streaming Over HTTP")]
    HTTP,

    #endregion

    #region Video Streaming

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Video Streaming")]
    VideoStreaming,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = VideoStreaming)]
    [EnumDescription("MJPEG Video Streaming")]
    MJPEGStreaming,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = VideoStreaming)]
    [EnumDescription("MPEG4 Video Streaming")]
    MPEG4Streaming,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = VideoStreaming)]
    [EnumDescription("H264 Video Streaming")]
    H264Streaming,

    #endregion

    #region Multicast Streaming

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Multicast Streaming")]
    MulticastStreaming,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = MulticastStreaming)]
    [EnumDescription("Multicast Streaming Using RTSP")]
    RTSPMulticast,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = MulticastStreaming)]
    [EnumDescription("Multicast Streaming Using SOAP")]
    SOAPMulticast,

    #endregion

    #region Video Encoder Configurations

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Video Encoder Configurations")]
    VideoEncoderConfigurations,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = VideoEncoderConfigurations)]
    [EnumDescription("List Video Encoder Configurations")]
    GetVideoEncoderConfigurations,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = VideoEncoderConfigurations)]
    [EnumDescription("Get Specific Video Encoder Configuration")]
    GetVideoEncoderConfiguration,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = VideoEncoderConfigurations)]
    [EnumDescription("Modify Video Encoder Configuration")]
    ModifyVideoEncoderConfiguration,

    #endregion

    #region Media Profile Configurations

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Media Profile Configurations")]
    MediaProfileConfigurations,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaProfileConfigurations)]
    [EnumDescription("List Available Media Profiles")]
    ListMediaProfiles,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaProfileConfigurations)]
    [EnumDescription("Get Specific Media Profile")]
    GetMediaProfile,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaProfileConfigurations)]
    [EnumDescription("Create a Media Profile")]
    CreateMediaProfile,

    #endregion

    #region Video Source Configurations

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Video Source Configurations")]
    VideoSourceConfigurations,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = VideoSourceConfigurations)]
    [EnumDescription("Get Specific Video Source Configuration")]
    GetVideoSourceConfiguration,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = VideoSourceConfigurations)]
    [EnumDescription("List Video Source Configurations")]
    GetVideoSourceConfigurations,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = VideoSourceConfigurations)]
    [EnumDescription("Modify Video Source Configuration")]
    ModifyVideoSourceConfiguration,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = VideoSourceConfigurations)]
    [EnumDescription("Add Video Source Configuration")]
    AddVideoSourceConfiguration,

    #endregion

    #region PTZ Listing

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("PTZ Listing")]
    PtzListing,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzListing)]
    [EnumDescription("Get Nodes")]
    GetNodes,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzListing)]
    [EnumDescription("Get Node")]
    GetNode,

    #endregion // PTZ Listing

    #region PTZ Configuration

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("PTZ Configuration")]
    PtzConfiguration,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzConfiguration)]
    [EnumDescription("Add PTZ Configuration")]
    AddPtzConfiguration,

    #endregion // PTZ Configuration

    #region PTZ Continious Positioning

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("PTZ Continuous Positioning")]
    PtzContinuousPositioning,
      
    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzContinuousPositioning)]
    [EnumDescription("PTZ Continuous Move PAN/TILT")]
    ContinuousMovePanTilt,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzContinuousPositioning)]
    [EnumDescription("Continuous Move Zoom")]
    ContinuousMoveZoom,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzContinuousPositioning)]
    [EnumDescription("Stop")]
    Stop,

    #endregion //PTZ Continious Positioning

    #region PTZ Absolute Positioning
    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("PTZ Absolute Positioning")]
    PtzAbsolutePositioning,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzAbsolutePositioning)]
    [EnumDescription("AbsoluteMove PanTilt")]
    AbsoluteMovePanTilt,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzAbsolutePositioning)]
    [EnumDescription("AbsoluteMove Zoom")]
    AbsoluteZoom,

    #endregion

    #region PTZ Relative Positioning
    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("PTZ Relative Positioning")]
    PtzRelativePositioning,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzRelativePositioning)]
    [EnumDescription("RelativeMove PanTilt")]
    PtzRelativeMovePanTilt,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzRelativePositioning)]
    [EnumDescription("RelativeMove Zoom")]
    PtzRelativeMoveZoom,

    #endregion

    #region PTZ Presets

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("PTZ Presets")]
    PtzPresets,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzPresets)]
    [EnumDescription("GetPresets")]
    PtzGetPresets,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzPresets)]
    [EnumDescription("GotoPreset")]
    PtzGotoPreset,

    #endregion

    #region PTZ Home Position

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("PTZ Home Position")]
    PtzHomePosition,
 
    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzHomePosition)]
    [EnumDescription("GotoHomePosition")]
    PtzGotoHomePosition,

    #endregion

    #region PTZ Home Position

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("PTZ Auxiliary Command")]
    PtzAuxiliaryCommand,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = PtzAuxiliaryCommand)]
    [EnumDescription("SendAuxiliaryCommand")]
    PtzSendAuxiliaryCommand,

    #endregion

    #region Audio Streaming

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Audio Streaming")]
    AudioStreaming,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = AudioStreaming)]
    [EnumDescription("Audio Streaming - Configure Media Profile")]
    AudioStreamingConfigureMediaProfile,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = AudioStreaming)]
    [EnumDescription("Audio Streaming - G.711")]
    AudioStreamingG711,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = AudioStreaming)]
    [EnumDescription("Audio Streaming - G.726")]
    AudioStreamingG726,

    [Profile(Profile = Profile.S, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = AudioStreaming)]
    [EnumDescription("Audio Streaming - AAC")]
    AudioStreamingAAC,

    #endregion

    #endregion

    #region Profile G

    #region Media Search

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Media Search")]
    MediaSearch,

    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaSearch)]
    [EnumDescription("Recording Search")]
    RecordingSearch,

    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaSearch)]
    [EnumDescription("Event Search")]
    EventSearch,

    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaSearch)]
    [EnumDescription("Recording Summary")]
    RecordingSummary,

    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaSearch)]
    [EnumDescription("Recording Information")]
    RecordingInformation,

    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaSearch)]
    [EnumDescription("Media Attributes")]
    MediaAttributes,

    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = MediaSearch)]
    [EnumDescription("Event Search Filter")]
    EventSearchFilter,

    #endregion

    #region Replay Recordings

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Replay Control")]
    ReplayControl,

    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = ReplayControl)]
    [EnumDescription("Get Replay Uri")]
    GetReplayUri,

    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = ReplayControl)]
    [EnumDescription("MJPEG Replay Recording")]
    MJPEGReplayRecording,

    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = ReplayControl)]
    [EnumDescription("MPEG4 Replay Recording")]
    MPEG4ReplayRecording,

    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = ReplayControl)]
    [EnumDescription("H264 Replay Recording")]
    H264ReplayRecording,

    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = ReplayControl)]
    [EnumDescription("Reverse Replay")]
    ReverseReplay,

    [Profile(Profile = Profile.G, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = ReplayControl)]
    [EnumDescription("RTSP Session Timeout Configuration")]
    RTSPSessionTimeoutConfiguration,

    #endregion

    #endregion

    #region Profile C

    #region System Component Information

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("System Component Information")]
    SystemComponentInformation,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = SystemComponentInformation)]
    [EnumDescription("Listing of Access Points")]
    ListingOfAccessPoints,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = SystemComponentInformation)]
    [EnumDescription("Listing of Doors")]
    ListingOfDoors,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = SystemComponentInformation)]
    [EnumDescription("Listing of Areas")]
    ListingOfAreas,

    #endregion

    #region System Component State

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("System Component State")]
    SystemComponentState,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = SystemComponentState)]
    [EnumDescription("State of Access Points")]
    StateOfAccessPoints,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = SystemComponentState)]
    [EnumDescription("State of Doors")]
    StateOfDoors,

    #endregion

    #region Door Control

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Door Control")]
    DoorControl,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = DoorControl)]
    [EnumDescription("Access Door")]
    AccessDoor,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = DoorControl)]
    [EnumDescription("Lock Door")]
    LockDoor,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Mandatory)]
    [Feature(Type = FeatureType.Child, ParentFeature = DoorControl)]
    [EnumDescription("Unlock Door")]
    UnlockDoor,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = DoorControl)]
    [EnumDescription("Double Lock Door")]
    DoubleLockDoor,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = DoorControl)]
    [EnumDescription("Block Door")]
    BlockDoor,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = DoorControl)]
    [EnumDescription("Lock Down Door")]
    LockDownDoor,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = DoorControl)]
    [EnumDescription("Lock Open Door")]
    LockOpenDoor,

    #endregion

    #region Access Points Control

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("Access Points Control")]
    AccessPointControl,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = AccessPointControl)]
    [EnumDescription("Disable Enable Access Point")]
    DisableEnableAccessPoint,

    #endregion

    #region External Authorization

    [Feature(Type = FeatureType.Parent)]
    [EnumDescription("External Authorization")]
    ExternalAuthorization,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = ExternalAuthorization)]
    [EnumDescription("Receive Authorization Request")]
    ReceiveAuthRequest,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = ExternalAuthorization)]
    [EnumDescription("Send Authorization Desicion")]
    SendAuthDecision,

    [Profile(Profile = Profile.C, RequirementLevel = RequirementLevel.Conditional)]
    [Feature(Type = FeatureType.Child, ParentFeature = ExternalAuthorization)]
    [EnumDescription("Retrieve Notifications about Access Decisions")]
    RetrieveNotificationsAboutAccessDecisions,

    #endregion

    #endregion
  }
}