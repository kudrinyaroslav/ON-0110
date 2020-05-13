///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Definitions.Devices.Definitions.FeatureList
{
  /// <summary>
  /// DTT Feature mapper
  /// </summary> 
  public static class FeatureMapper
  {
    public static Feature[] GetTranslatedFeatures(String dttFeature)
    {
      if (mFeatures.Value.ContainsKey(dttFeature))
        return mFeatures.Value[dttFeature];

      return new[]
      {
        Feature.Unknown
      };
    }

    #region Dictionary

    private static readonly Lazy<Dictionary<String, Feature[]>> mFeatures = new Lazy<Dictionary<string, Feature[]>>(() => new Dictionary<String, Feature[]>
    {
      {
        "WSU",                                new [] { Feature.UsernameToken }
      },                                      
      {                                       
        "Digest",                             new [] { Feature.HTTPDigest }
      },                                      
      {                                       
        "GetServices",                        new [] { Feature.GetServices }
      },                                      
      {                                       
        "GetCapabilities",                    new [] { Feature.GetCapabilities }
      },	                                    
      {                                       
        "WSBasicNotification",                new [] { Feature.WSBaseNotification }
      },                                      
      {                                       
        "RTPRTSPTCP",                         new [] { Feature.RTSPStreaming }
      },                                      
      {                                       
        "RTPUDP",                             new [] { Feature.UDP }
      },                                      
      {                                       
        "RTPRTSPHTTP",                        new [] { Feature.HTTP }
      },                                      
      {                                       
        "JPEG",                               new [] { Feature.MJPEGStreaming }
      },                                      
      {                                       
        "MPEG4",                              new [] { Feature.MPEG4Streaming }
      },                                      
      {                                       
        "H264",                               new [] { Feature.H264Streaming }
      },                                      
      {                                       
        "RTPMulticastUDP",                    new [] { Feature.RTSPMulticast }
      },                                      
      {                                       
        "MediaService",                       new [] { Feature.GetProfiles, 
          Feature.GetStreamURI,
          Feature.SOAPMulticast, 
          Feature.GetVideoEncoderConfigurations,
          Feature.GetVideoEncoderConfiguration,
          Feature.ModifyVideoEncoderConfiguration,
          Feature.ListMediaProfiles,
          Feature.GetMediaProfile,
          Feature.CreateMediaProfile,
          Feature.GetVideoSourceConfiguration,
          Feature.GetVideoSourceConfigurations,
          Feature.ModifyVideoSourceConfiguration,
          Feature.AddVideoSourceConfiguration }
      },                                      
      {                                       
        "RecordingSearchService",             new [] { Feature.RecordingSearch,
          Feature.RecordingSummary }
      },                                      
      {                                       
        "RecordingControlService",            new [] { Feature.RecordingInformation,
          Feature.MediaAttributes }
      },                                      
      {                                       
        "EventsService",                      new [] { Feature.MetadataStreaming,
          Feature.PullPoint,
          Feature.EventSearch,
          Feature.EventSearchFilter }
      },                                      
      {                                       
        "AccessPointEntity",                  new [] { Feature.ListingOfAccessPoints }
      },                                      
      {                                       
        "DoorEntity",                         new [] { Feature.ListingOfDoors }
      },                                      
      {                                       
        "AreaEntity",                         new [] { Feature.ListingOfAreas }
      },                                      
      {                                       
        "AccessPointStateEnabledEvent",       new [] { Feature.StateOfAccessPoints }
      },                                      
      {                                       
        "DoorPhysicalStateEvent",             new [] { Feature.StateOfDoors }
      },                                      
      {                                       
        "AccessDoor",                         new [] { Feature.AccessDoor }
      },                                      
      {                                       
        "LockDoor",                           new [] { Feature.LockDoor}
      },                                      
      {                                       
        "UnlockDoor",                         new [] { Feature.UnlockDoor }
      },                                      
      {                                       
        "DoubleLockDoor",                     new [] { Feature.DoubleLockDoor }
      },                                      
      {                                       
        "BlockDoor",                          new [] { Feature.BlockDoor }
      },                                      
      {                                       
        "LockDownDoor",                       new [] { Feature.LockDownDoor }
      },                                      
      {                                       
        "LockOpenDoor",                       new [] { Feature.LockOpenDoor }
      },                                      
      {                                       
        "EnableDisableAccessPoint",           new [] { Feature.DisableEnableAccessPoint }
      },                                      
      {                                       
        "Discovery",                          new [] { Feature.WSDiscovery }
      },                                      
      {                                       
        "Network",                            new [] { Feature.GetNetworkInterfaces,
          Feature.SetNetworkInterfaces,
          Feature.GetNetworkDefaultGateway,
          Feature.SetNetworkDefaultGateway }
      },                                      
      {                                       
        "System",                             new [] { Feature.GetDeviceInformation }
      },                                      
      {                                       
        "UserService",                        new [] { Feature.GetUsers,
          Feature.SetUser, 
          Feature.CreateUsers,
          Feature.DeleteUsers  }
      },
      {
        "DeviceIORelayOutputsBistable",       new [] { Feature.SetRelayOutputBistable }
      },
      {
        "DeviceIORelayOutputsMonostable",     new [] { Feature.SetRelayOutputMonostable }
      },
      {
        "NTP",                                new [] { Feature.GetNTP, Feature.SetNTP  }
      },
      {
        "DynamicDNS",                         new [] { Feature.GetDynamicDnsSettings, Feature.SetDynamicDnsSettings  }
      },
      {
        "ZeroConfiguration",                  new [] { Feature.GetZeroConfiguration, Feature.SetZeroConfiguration }
      },
      {
        "IPFilter",                           new [] { Feature.GetIpAddressFilter,
          Feature.SetIpV4AddressFilter,
          Feature.SetIpV6AddressFilter,
          Feature.AddIpV4AddressFilter,
          Feature.AddIpV6AddressFilter,
          Feature.RemoveIpV4AddressFilter,
          Feature.RemoveIpV6AddressFilter }
      },
      {                                       
        "PersistentNotificationStorage",      new [] { Feature.Seek }
      }, 
      {                                       
        "ExternalAuthorization",              new [] { Feature.ReceiveAuthRequest,
          Feature.SendAuthDecision,
          Feature.RetrieveNotificationsAboutAccessDecisions }
      }, 
      {                                       
        "ReplayService",                      new [] { Feature.GetReplayUri, 
          Feature.MJPEGReplayRecording,
          Feature.MPEG4ReplayRecording, 
          Feature.H264ReplayRecording,
          Feature.RTSPSessionTimeoutConfiguration  }
      }, 
      {                                       
        "ReverseReplay",                      new [] { Feature.ReverseReplay }
      }, 
      {                                       
        "PTZService",                         new [] { Feature.GetNode, Feature.GetNodes, Feature.AddPtzConfiguration, Feature.Stop }
      }, 
      {                                       
        "PTZContinuousPanTilt",               new [] { Feature.ContinuousMovePanTilt }
      }, 
      {                                       
        "PTZContinuousZoom",                  new [] { Feature.ContinuousMoveZoom }
      }, 
      {                                       
        "PTZAbsolutePanTilt",                 new [] { Feature.AbsoluteMovePanTilt }
      }, 
      {                                       
        "PTZAbsoluteZoom",                    new [] { Feature.AbsoluteZoom }
      }, 
      {                                       
        "PTZRelativePanTilt",                 new [] { Feature.PtzRelativeMovePanTilt }
      }, 
      {                                       
        "PTZRelativeZoom",                    new [] { Feature.PtzRelativeMoveZoom }
      }, 
      {                                       
        "PTZPresets",                         new [] { Feature.PtzGotoPreset, Feature.PtzGetPresets }
      }, 
      {                                       
        "PTZHome",                            new [] { Feature.PtzGotoHomePosition }
      },                                      
      {                                       
        "PTZAuxiliary",                       new [] { Feature.PtzSendAuxiliaryCommand }
      },
      {
        "Audio",                              new [] { Feature.AudioStreamingConfigureMediaProfile }
      },
      {                                       
        "G711",                               new [] { Feature.AudioStreamingG711 }
      }, 
      {                                       
        "G726",                               new [] { Feature.AudioStreamingG726 }
      }, 
      {                                       
        "AAC",                                new [] { Feature.AudioStreamingAAC }
      }, 
    });
      

    #endregion
  }
}
