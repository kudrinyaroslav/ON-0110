using System.Collections.Generic;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.Tests.Definitions.Features
{
    /// <summary>
    /// Hierarchycal set of all features
    /// </summary>
    public class FeaturesSet
    {
        private FeaturesSet()
        {
            Nodes = new List<FeatureNode>();
        }

        /// <summary>
        /// Root nodes
        /// </summary>
        public List<FeatureNode> Nodes { get; private set; }
        
        /// <summary>
        /// Finds node for feature specified
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public FeatureNode FindNode(Feature feature)
        {
            foreach (FeatureNode node in Nodes)
            {
                if (node.Feature== feature)
                {
                    return node;
                }
                else
                {
                    FeatureNode n = FindNode(node, feature);
                    if (n != null)
                    {
                        return n;
                    }
                }
            }
            return null;
        }

        FeatureNode FindNode(FeatureNode node, Feature feature)
        {
            foreach (FeatureNode child in node.Nodes)
            {
                if (child.Feature == feature)
                {
                    return child;
                }
                else
                {
                    FeatureNode n = FindNode(child, feature);
                    if (n != null)
                    {
                        return n;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Creates features tree. Initializes checked state; sets up mandatory features.
        /// </summary>
        /// <returns>"Canonical" features tree</returns>
        public static FeaturesSet CreateFeaturesSet()
        {
            FeaturesSet featuresSet = new FeaturesSet();

            FeatureNode security = CreateSecurityNode();
            featuresSet.Nodes.Add(security);
            
            FeatureNode discovery = CreateDiscoveryNode();
            featuresSet.Nodes.Add(discovery);
            
            FeatureNode deviceService = CreateDeviceServiceNode();
            featuresSet.Nodes.Add(deviceService);
            
            FeatureNode events = new FeatureNode() { Name = "Events", Feature = Feature.EventsService };
            featuresSet.Nodes.Add(events);
            {
                FeatureNode eventSeek = new FeatureNode() { Name = "EventSeek", Feature = Feature.EventSeek};
                events.Nodes.Add(eventSeek);
            }

            FeatureNode media = CreateMediaServiceNode();
            featuresSet.Nodes.Add(media);

            FeatureNode ptz = CreatePTZNode();
            featuresSet.Nodes.Add(ptz);


            FeatureNode io = new FeatureNode() { Name = "IO", Feature = Feature.DeviceIoService };
            featuresSet.Nodes.Add(io);
            io.Nodes.Add(new FeatureNode() { Name = "RelayOutput", Feature = Feature.RelayOutputs });
            io.Nodes.Add(new FeatureNode() { Name = "DigitalInputs", Feature = Feature.DigitalInputs });

            FeatureNode imaging = new FeatureNode() { Name = "Imaging", Feature = Feature.ImagingService };
            featuresSet.Nodes.Add(imaging);

            FeatureNode analytics = new FeatureNode() { Name = "Analytics", Feature = Feature.AnalyticsService };
            featuresSet.Nodes.Add(analytics);
            
            FeatureNode doorControl = CreateDoorControlServiceNode();
            featuresSet.Nodes.Add(doorControl);
            
            FeatureNode pacs = CreatePacsServiceNode();
            featuresSet.Nodes.Add(pacs);
            
            return featuresSet;
        }

        static FeatureNode CreateSecurityNode()
        {
            FeatureNode security = new FeatureNode()
            {
                Name = "Security",
                Feature = Feature.Security,
                State = FeatureState.Undefined,
                Status = FeatureStatus.Group
            };

            security.Nodes.Add(new FeatureNode() { Name = "WSU", Feature = Feature.WSU });
            security.Nodes.Add(new FeatureNode() { Name = "Digest", Feature = Feature.Digest });

            return security;
        }

        static FeatureNode CreateDiscoveryNode()
        {
            FeatureNode discovery = new FeatureNode()
            {
                Name = "Discovery",
                Feature = Feature.Discovery,
                State = FeatureState.Undefined,
                Status = FeatureStatus.Group
            };
            discovery.Nodes.Add(new FeatureNode() { Name = "BYE", Feature = Feature.BYE });
            return discovery;
        }

        static FeatureNode CreateDeviceServiceNode()
        {
            FeatureNode deviceService = new FeatureNode()
            {
                Name = "DeviceService",
                Feature = Feature.DeviceService,
                State = FeatureState.Undefined,
                Status = FeatureStatus.Group
            };

            FeatureNode capabilities = new FeatureNode()
            {
                Name = "Capabilities",
                Feature = Feature.Capabilities,
                State = FeatureState.Undefined,
                Status = FeatureStatus.Group
            };
            deviceService.Nodes.Add(capabilities);
            capabilities.Nodes.Add(new FeatureNode() { Name = "GetCapabilities", Feature = Feature.GetCapabilities });
            capabilities.Nodes.Add(new FeatureNode() { Name = "GetServices", Feature = Feature.GetServices });

            FeatureNode network = new FeatureNode()
            {
                Name = "Network",
                Feature = Feature.Network,
                State = FeatureState.Undefined,
                Status = FeatureStatus.Group
            };
            deviceService.Nodes.Add(network);

            network.Nodes.Add(new FeatureNode()
            {
                Name = "ZeroConfiguration",
                Feature = Feature.ZeroConfiguration
            });
            network.Nodes.Add(new FeatureNode() { Name = "NTP", Feature = Feature.NTP });
            network.Nodes.Add(new FeatureNode() { Name = "IPv6", Feature = Feature.IPv6 });
            network.Nodes.Add(new FeatureNode() { Name = "DynamicDNS", Feature = Feature.DynamicDNS });
            network.Nodes.Add(new FeatureNode() { Name = "IPFilter", Feature = Feature.IPFilter });

            FeatureNode system = new FeatureNode()
            {
                Name = "System",
                Feature = Feature.System,
                State = FeatureState.Undefined,
                Status = FeatureStatus.Group
            };

            deviceService.Nodes.Add(system);

            FeatureNode systemLog = new FeatureNode()
            {
                Name = "SystemLog",
                Feature = Feature.SystemLogging
            };

            system.Nodes.Add(systemLog);

            FeatureNode deviceIo = new FeatureNode()
            {
                Name = "DeviceIo",
                Feature = Feature.DeviceIO,
                State = FeatureState.Undefined,
                Status = FeatureStatus.Group
            };

            deviceService.Nodes.Add(deviceIo);

            FeatureNode deviceIoRelayOutputs = new FeatureNode()
            {
                Name = "DeviceIoRelayOutputs",
                Feature = Feature.DeviceIORelayOutputs
            };

            deviceIo.Nodes.Add(deviceIoRelayOutputs);

            {
                FeatureNode deviceIoRelayOutputsBistable = new FeatureNode()
                {
                    Name = "DeviceIoRelayOutputsBistable",
                    Feature = Feature.DeviceIORelayOutputsBistable

                };
                FeatureNode deviceIoRelayOutputsBistableOpen = new FeatureNode()
                {
                    Name = "DeviceIoRelayOutputsBistableOpen",
                    Feature =
                        Feature.DeviceIORelayOutputsBistableOpen
                };
                FeatureNode deviceIoRelayOutputsBistableClosed = new FeatureNode()
                {
                    Name = "DeviceIoRelayOutputsBistableClosed",
                    Feature =
                        Feature.DeviceIORelayOutputsBistableClosed
                };

                deviceIoRelayOutputsBistable.Nodes.Add(deviceIoRelayOutputsBistableOpen);
                deviceIoRelayOutputsBistable.Nodes.Add(deviceIoRelayOutputsBistableClosed);

                deviceIoRelayOutputs.Nodes.Add(deviceIoRelayOutputsBistable);
            }

            {
                FeatureNode deviceIoRelayOutputsMonostable = new FeatureNode()
                {
                    Name = "DeviceIoRelayOutputsMonostable",
                    Feature = Feature.DeviceIORelayOutputsMonostable

                };

                FeatureNode deviceIoRelayOutputsMonostableOpen = new FeatureNode()
                {
                    Name = "DeviceIoRelayOutputsMonostableOpen",
                    Feature =
                        Feature.DeviceIORelayOutputsMonostableOpen
                };
                FeatureNode deviceIoRelayOutputsMonostableClosed = new FeatureNode()
                {
                    Name = "DeviceIoRelayOutputsMonostableClosed",
                    Feature =
                        Feature.DeviceIORelayOutputsMonostableClosed
                };

                deviceIoRelayOutputsMonostable.Nodes.Add(deviceIoRelayOutputsMonostableOpen);
                deviceIoRelayOutputsMonostable.Nodes.Add(deviceIoRelayOutputsMonostableClosed);

                deviceIoRelayOutputs.Nodes.Add(deviceIoRelayOutputsMonostable);
            }

            return deviceService;       
        }

        static FeatureNode CreateMediaServiceNode()
        {
            FeatureNode media = new FeatureNode() { Name = "Media", Feature = Feature.MediaService };

            FeatureNode video = new FeatureNode()
            {
                Name = "VideoCodec",
                Feature = Feature.Video,
                State = FeatureState.Undefined,
                Status = FeatureStatus.Group
            };
            media.Nodes.Add(video);
            video.Nodes.Add(new FeatureNode() { Name = "JPEG", Feature = Feature.JPEG });
            video.Nodes.Add(new FeatureNode() { Name = "H264", Feature = Feature.H264 });
            video.Nodes.Add(new FeatureNode() { Name = "MPEG4", Feature = Feature.MPEG4 });

            FeatureNode audio = new FeatureNode() { Name = "Audio", Feature = Feature.Audio };
            media.Nodes.Add(audio);

            audio.Nodes.Add(new FeatureNode() { Name = "G711", Feature = Feature.G711 });
            audio.Nodes.Add(new FeatureNode() { Name = "G726", Feature = Feature.G726 });
            audio.Nodes.Add(new FeatureNode() { Name = "AAC", Feature = Feature.AAC });

            FeatureNode backchannel = new FeatureNode()
            {
                Name = "Backchannel",
                Feature = Feature.AudioOutput
            };
            media.Nodes.Add(backchannel);
            backchannel.Nodes.Add(new FeatureNode() { Name = "BackchannelG711", Feature = Feature.AudioOutputG711 });
            backchannel.Nodes.Add(new FeatureNode() { Name = "BackchannelG726", Feature = Feature.AudioOutputG726 });
            backchannel.Nodes.Add(new FeatureNode() { Name = "BackchannelAAC", Feature = Feature.AudioOutputAAC });



            FeatureNode rtss = new FeatureNode()
            {
                Name = "RTSS",
                Feature = Feature.RTSS,
                State = FeatureState.Undefined,
                Status = FeatureStatus.Group
            };
            media.Nodes.Add(rtss);

            rtss.Nodes.Add(new FeatureNode() { Name = "RTPUDP", Feature = Feature.RTPUDP });
            rtss.Nodes.Add(new FeatureNode() { Name = "RTPRTSPHTTP", Feature = Feature.RTPRTSPHTTP });
            rtss.Nodes.Add(new FeatureNode() { Name = "RTPRTSPTCP", Feature = Feature.RTPRTSPTCP });
            rtss.Nodes.Add(new FeatureNode() { Name = "RTPMulticastUDP", Feature = Feature.RTPMulticastUDP });
            rtss.Nodes.Add(new FeatureNode() { Name = "SnapshotURI", Feature = Feature.SnapshotUri });

            return media; 
        }

        static FeatureNode CreatePTZNode()
        {
            FeatureNode ptz = new FeatureNode() { Name = "PTZ", Feature = Feature.PTZService };

            FeatureNode ptzAbsolute = new FeatureNode() { Name = "PTZAbsolute", Feature = Feature.PTZAbsolute };
            ptzAbsolute.Nodes.Add(new FeatureNode() { Name = "PanTiltMovement", Feature = Feature.PTZAbsolutePanTilt });
            ptzAbsolute.Nodes.Add(new FeatureNode() { Name = "ZoomMovement", Feature = Feature.PTZAbsoluteZoom });
            FeatureNode ptzRelative = new FeatureNode() { Name = "PTZRelative", Feature = Feature.PTZRelative };
            ptzRelative.Nodes.Add(new FeatureNode() { Name = "PanTiltMovement", Feature = Feature.PTZRelativePanTilt });
            ptzRelative.Nodes.Add(new FeatureNode() { Name = "ZoomMovement", Feature = Feature.PTZRelativeZoom });
            FeatureNode ptzContinious = new FeatureNode() { Name = "PTZContinuous", Feature = Feature.PTZContinious };
            ptzContinious.Nodes.Add(new FeatureNode() { Name = "PanTiltSpeedConfiguration", Feature = Feature.PTZContinuousPanTilt });
            ptzContinious.Nodes.Add(new FeatureNode() { Name = "ZoomSpeedConfiguration", Feature = Feature.PTZContinuousZoom });
            FeatureNode ptzPresets = new FeatureNode() { Name = "PTZPresets", Feature = Feature.PTZPresets };
            FeatureNode ptzHome = new FeatureNode() { Name = "PTZHome", Feature = Feature.PTZHome };
            ptzHome.Nodes.Add(new FeatureNode() { Name = "ConfigurableHomePosition", Feature = Feature.PTZConfigurableHome });
            ptzHome.Nodes.Add(new FeatureNode() { Name = "FixedHomePosition", Feature = Feature.PTZFixedHome });
            FeatureNode ptzAuxiliary = new FeatureNode() { Name = "PTZAuxiliary", Feature = Feature.PTZAuxiliary };
            FeatureNode ptzSpeed = new FeatureNode() { Name = "PTZSpeed", Feature = Feature.PTZSpeed };
            ptzSpeed.Nodes.Add(new FeatureNode() { Name = "PanTiltSpeed", Feature = Feature.PTZSpeedPanTilt });
            ptzSpeed.Nodes.Add(new FeatureNode() { Name = "ZoomSpeed", Feature = Feature.PTZSpeedZoom });

            ptz.Nodes.AddRange(new FeatureNode[] { ptzAbsolute, ptzRelative, ptzContinious, ptzPresets, ptzHome, ptzAuxiliary, ptzSpeed });

            return ptz;
        }

        static FeatureNode CreateDoorControlServiceNode()
        {
            FeatureNode doorControl = new FeatureNode() { Name = "DoorControl", Feature = Feature.DoorControlService };

            FeatureNode doorEntityNode = CreateDoorEntityNode();
            doorControl.Nodes.Add(doorEntityNode);
            
            return doorControl;
        }

        static FeatureNode CreateDoorEntityNode()
        {
            FeatureNode doorEntityNode = new FeatureNode() { Name = "DoorEntity", Feature = Feature.DoorEntity };

            FeatureNode accessDoorNode = new FeatureNode() { Name = "AccessDoor", Feature = Feature.AccessDoor };
            doorEntityNode.Nodes.Add(accessDoorNode);

            FeatureNode lockDoorNode = new FeatureNode() { Name = "LockDoor", Feature = Feature.LockDoor };
            doorEntityNode.Nodes.Add(lockDoorNode);

            FeatureNode unlockDoorNode = new FeatureNode() { Name = "UnlockDoor", Feature = Feature.UnlockDoor };
            doorEntityNode.Nodes.Add(unlockDoorNode);

            FeatureNode doubleLockDoorNode = new FeatureNode() { Name = "DoubleLockDoor", Feature = Feature.DoubleLockDoor };
            doorEntityNode.Nodes.Add(doubleLockDoorNode);

            FeatureNode blockDoorNode = new FeatureNode() { Name = "BlockDoor", Feature = Feature.BlockDoor };
            doorEntityNode.Nodes.Add(blockDoorNode);

            FeatureNode lockDownDoorNode = new FeatureNode() { Name = "LockDownDoor", Feature = Feature.LockDownDoor };
            doorEntityNode.Nodes.Add(lockDownDoorNode);

            FeatureNode lockOpenDoorNode = new FeatureNode() { Name = "LockOpenDoor", Feature = Feature.LockOpenDoor };
            doorEntityNode.Nodes.Add(lockOpenDoorNode);

            FeatureNode doorMonitorNode = new FeatureNode() { Name = "DoorMonitor", Feature = Feature.DoorMonitor };
            doorEntityNode.Nodes.Add(doorMonitorNode);

            FeatureNode lockMonitorNode = new FeatureNode() { Name = "LockMonitor", Feature = Feature.LockMonitor };
            doorEntityNode.Nodes.Add(lockMonitorNode);

            FeatureNode doubleLockMonitor = new FeatureNode() { Name = "DoubleLockMonitor", Feature = Feature.DoubleLockMonitor };
            doorEntityNode.Nodes.Add(doubleLockMonitor);

            FeatureNode doorAlarmNode = new FeatureNode() { Name = "DoorAlarm", Feature = Feature.DoorAlarm };
            doorEntityNode.Nodes.Add(doorAlarmNode);

            FeatureNode doorTamperNode = new FeatureNode() { Name = "DoorTamper", Feature = Feature.DoorTamper };
            doorEntityNode.Nodes.Add(doorTamperNode);

            FeatureNode doorFaultNode = new FeatureNode() { Name = "DoorFault", Feature = Feature.DoorFault };
            doorEntityNode.Nodes.Add(doorFaultNode);

            FeatureNode eventNode = CreateDoorEventsNode();
            doorEntityNode.Nodes.Add(eventNode);

            return doorEntityNode;
        }

        static FeatureNode CreateDoorEventsNode()
        {
            FeatureNode eventNode = new FeatureNode() { Name = "DoorControlEvents", Feature = Feature.DoorControlEvents, Status = FeatureStatus.Group };

            {
                FeatureNode doorEventNode = new FeatureNode() { Name = "DoorModeEvent", Feature = Feature.DoorModeEvent };
                eventNode.Nodes.Add(doorEventNode);
            }
            {
                FeatureNode doorEventNode = new FeatureNode() { Name = "DoorPhysicalStateEvent", Feature = Feature.DoorPhysicalStateEvent };
                eventNode.Nodes.Add(doorEventNode);
            }
            {
                FeatureNode doorEventNode = new FeatureNode() { Name = "LockPhysicalStateEvent", Feature = Feature.LockPhysicalStateEvent };
                eventNode.Nodes.Add(doorEventNode);
            }            
            {
                FeatureNode doorEventNode = new FeatureNode() { Name = "DoubleLockPhysicalStateEvent", Feature = Feature.DoubleLockPhysicalStateEvent };
                eventNode.Nodes.Add(doorEventNode);
            }
            {
                FeatureNode doorEventNode = new FeatureNode() { Name = "DoorTamperEvent", Feature = Feature.DoorTamperEvent };
                eventNode.Nodes.Add(doorEventNode);
            }
            {
                FeatureNode doorEventNode = new FeatureNode() { Name = "DoorAlarmEvent", Feature = Feature.DoorAlarmEvent };
                eventNode.Nodes.Add(doorEventNode);
            }
            {
                FeatureNode doorEventNode = new FeatureNode() { Name = "DoorSetEvent", Feature = Feature.DoorSetEvent };
                eventNode.Nodes.Add(doorEventNode);
            }
            {
                FeatureNode doorEventNode = new FeatureNode() { Name = "DoorRemovedEvent", Feature = Feature.DoorRemovedEvent };
                eventNode.Nodes.Add(doorEventNode);
            }
            {
                FeatureNode doorEventNode = new FeatureNode() { Name = "DoorFaultEvent", Feature = Feature.DoorFaultEvent };
                eventNode.Nodes.Add(doorEventNode);
            }

            return eventNode;
        }

        static FeatureNode CreatePacsServiceNode()
        {
            FeatureNode pacs = new FeatureNode() { Name = "AccessControl", Feature = Feature.AccessControlService };

            FeatureNode areaEntityNode = new FeatureNode() { Name = "AreaEntity", Feature = Feature.AreaEntity };
            pacs.Nodes.Add(areaEntityNode);

            FeatureNode accessPointEntityNode = new FeatureNode() { Name = "AccessPointEntity ", Feature = Feature.AccessPointEntity };
            pacs.Nodes.Add(accessPointEntityNode);

            FeatureNode enableDisableAccessPoint = new FeatureNode() { Name = "EnableDisableAccessPoint", Feature = Feature.EnableDisableAccessPoint };
            accessPointEntityNode.Nodes.Add(enableDisableAccessPoint);
            
            FeatureNode duress = new FeatureNode() { Name = "Duress", Feature = Feature.Duress };
            accessPointEntityNode.Nodes.Add(duress);
            
            FeatureNode accessTaken = new FeatureNode() { Name = "AccessTaken", Feature = Feature.AccessTaken };
            accessPointEntityNode.Nodes.Add(accessTaken);
            
            FeatureNode exAuth = new FeatureNode() { Name = "ExternalAuthorization", Feature = Feature.ExternalAuthorization };
            accessPointEntityNode.Nodes.Add(exAuth);
            
            FeatureNode tamper = new FeatureNode() { Name = "Tamper", Feature = Feature.Tamper };
            accessPointEntityNode.Nodes.Add(tamper);
            
            FeatureNode anonymousAccess = new FeatureNode() { Name = "AnonymousAccess", Feature = Feature.AnonymousAccess };
            accessPointEntityNode.Nodes.Add(anonymousAccess);

            FeatureNode hostResponseNode = new FeatureNode() { Name = "DoorEntity", Feature = Feature.HostResponse };
            pacs.Nodes.Add(hostResponseNode);

            FeatureNode accessPointEventsNode = CreatePacsEventsNode();
            accessPointEntityNode.Nodes.Add(accessPointEventsNode);

            return pacs;
        }

        static FeatureNode CreatePacsEventsNode()
        { 
            FeatureNode accessPointEventsNode = new FeatureNode() { Name = "AccessControlEvents", Feature = Feature.AccessControlEvents, Status = FeatureStatus.Group };

            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessGrantedAnonymousEvent", Feature = Feature.AccessGrantedAnonymousEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessGrantedCredentialEvent", Feature = Feature.AccessGrantedCredentialEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessGrantedAnonymousExternalEvent", Feature = Feature.AccessGrantedAnonymousExternalEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessGrantedCredentialExternalEvent", Feature = Feature.AccessGrantedCredentialExternalEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessTakenAnonymousEvent", Feature = Feature.AccessTakenAnonymousEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessTakenCredentialEvent", Feature = Feature.AccessTakenCredentialEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessNotTakenAnonymousEvent", Feature = Feature.AccessNotTakenAnonymousEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessNotTakenCredentialEvent", Feature = Feature.AccessNotTakenCredentialEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedCredentialCredentialNotEnabledEvent", Feature = Feature.AccessDeniedCredentialCredentialNotEnabledEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedCredentialCredentialNotActiveEvent", Feature = Feature.AccessDeniedCredentialCredentialNotActiveEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedCredentialCredentialExpiredEvent", Feature = Feature.AccessDeniedCredentialCredentialExpiredEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedCredentialInvalidPINEvent", Feature = Feature.AccessDeniedCredentialInvalidPINEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedCredentialNotPermittedAtThisTimeEvent", Feature = Feature.AccessDeniedCredentialNotPermittedAtThisTimeEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedCredentialUnauthorizedEvent", Feature = Feature.AccessDeniedCredentialUnauthorizedEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedCredentialExternalEvent", Feature = Feature.AccessDeniedCredentialExternalEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedCredentialOtherEvent", Feature = Feature.AccessDeniedCredentialOtherEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedAnonymousNotPermittedAtThisTimeEvent", Feature = Feature.AccessDeniedAnonymousNotPermittedAtThisTimeEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedAnonymousUnauthorizedEvent", Feature = Feature.AccessDeniedAnonymousUnauthorizedEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedAnonymousExternalEvent", Feature = Feature.AccessDeniedAnonymousExternalEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedAnonymousOtherEvent", Feature = Feature.AccessDeniedAnonymousOtherEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedCredentialCredentialNotFoundCardEvent", Feature = Feature.AccessDeniedCredentialCredentialNotFoundCardEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "DuressAnonymousEvent", Feature = Feature.DuressAnonymousEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "DuressCredentialEvent", Feature = Feature.DuressCredentialEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "RequestAnonymousEvent", Feature = Feature.RequestAnonymousEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "RequestCredentialEvent", Feature = Feature.RequestCredentialEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "RequestTimeoutAnonymousEvent", Feature = Feature.RequestTimeoutAnonymousEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "RequestTimeoutCredentialEvent", Feature = Feature.RequestTimeoutCredentialEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessPointEnabledEvent", Feature = Feature.AccessPointEnabledEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessPointTamperingEvent", Feature = Feature.AccessPointTamperingEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AreaSetEvent", Feature = Feature.AreaSetEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AreaRemovedEvent", Feature = Feature.AreaRemovedEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessPointSetEvent", Feature = Feature.AccessPointSetEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessPointRemovedEvent", Feature = Feature.AccessPointRemovedEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            return accessPointEventsNode;
        }

    }
}
