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

            FeatureNode security = new FeatureNode()
            {
                Name = "Security",
                Feature = Feature.Security,
                State = FeatureState.Undefined,
                Status = FeatureStatus.Group
            };
            featuresSet.Nodes.Add(security);


            security.Nodes.Add(new FeatureNode() { Name = "WSU", Feature = Feature.WSU });
            security.Nodes.Add(new FeatureNode() { Name = "Digest", Feature = Feature.Digest });

            FeatureNode discovery = new FeatureNode()
            {
                Name = "Discovery",
                Feature = Feature.Discovery,
                State = FeatureState.Undefined,
                Status = FeatureStatus.Group
            };
            featuresSet.Nodes.Add(discovery);

            //[14.05.2013] AKS: Added new node with two new features according ticket #224
            var types = new FeatureNode()
                        {
                            Name = "Types",
                            Feature = Feature.DiscoveryTypes,
                            State = FeatureState.Undefined,
                            Status = FeatureStatus.Group
                        };
            types.Nodes.Add(new FeatureNode() { Name = "tds:Device", Feature = Feature.DiscoveryTypesTdsDevice });
            types.Nodes.Add(new FeatureNode() { Name = "dn:NetworkVideoTransmitter", Feature = Feature.DiscoveryTypesDnNetworkVideoTransmitter });

            discovery.Nodes.Add(new FeatureNode() { Name = "BYE", Feature = Feature.BYE });
            discovery.Nodes.Add(types);

            FeatureNode deviceService = new FeatureNode()
            {
                Name = "DeviceService",
                Feature = Feature.DeviceService,
                State = FeatureState.Undefined,
                Status = FeatureStatus.Group
            };
            featuresSet.Nodes.Add(deviceService);

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
            network.Nodes.Add(new FeatureNode() { Name = "DHCPv6", Feature = Feature.DHCPv6 });
            network.Nodes.Add(new FeatureNode() { Name = "DynamicDNS", Feature = Feature.DynamicDNS });
            network.Nodes.Add(new FeatureNode() { Name = "IPFilter", Feature = Feature.IPFilter });

            {
                FeatureNode system = new FeatureNode()
                                     {
                                             Name = "System",
                                             Feature = Feature.System,
                                             State = FeatureState.Undefined,
                                             Status = FeatureStatus.Group
                                     };
                deviceService.Nodes.Add(system);

                var systemLog              = new FeatureNode() { Name = "SystemLog", Feature = Feature.SystemLogging };
                var httpSystemLog          = new FeatureNode() { Name = "HTTPSystemLog", Feature = Feature.HttpSystemLogging };
                var httpFirmwareUpgrade    = new FeatureNode() { Name = "HTTPFirmwareUpgrade", Feature = Feature.HttpFirmwareUpgrade };
                var httpSupportInformation = new FeatureNode() { Name = "HTTPSupportInformation", Feature = Feature.HttpSupportInformation };
                var httpSystemBackup       = new FeatureNode() { Name = "HTTPSystemBackup", Feature = Feature.HttpSystemBackup };

                system.Nodes.Add(systemLog);
                system.Nodes.Add(httpSystemLog);
                system.Nodes.Add(httpFirmwareUpgrade);
                system.Nodes.Add(httpSupportInformation);
                system.Nodes.Add(httpSystemBackup);
            }

            {
                var deviceServiceSecurity = new FeatureNode(){ Name = "Security", Feature = Feature.DeviceServiceSecurity, Status = FeatureStatus.Group };
                deviceService.Nodes.Add(deviceServiceSecurity);

                deviceServiceSecurity.Nodes.Add(new FeatureNode() { Name = "Default Access Policy",    Feature = Feature.DefaultAccessPolicy });
                deviceServiceSecurity.Nodes.Add(new FeatureNode() { Name = "Maximum Users",            Feature = Feature.MaxUsers });
                deviceServiceSecurity.Nodes.Add(new FeatureNode() { Name = "Remote User Handling",     Feature = Feature.RemoteUserHandling });
                deviceServiceSecurity.Nodes.Add(new FeatureNode() { Name = "Maximum Username Length",  Feature = Feature.MaximumUsernameLength });
                deviceServiceSecurity.Nodes.Add(new FeatureNode() { Name = "Maximum Password Length",  Feature = Feature.MaximumPasswordLength });
            }
            

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

            {
                var monitoringEvents = new FeatureNode() { Name = "Monitoring Events", Feature = Feature.MonitoringEvents, Status = FeatureStatus.Group};
                featuresSet.Nodes.Add(monitoringEvents);

                monitoringEvents.Nodes.Add(new FeatureNode() { Name = "Monitoring/ProcessorUsage",                              Feature = Feature.MonitoringProcessorUsageEvent });
                monitoringEvents.Nodes.Add(new FeatureNode() { Name = "Monitoring/OperatingTime/LastReset",                     Feature = Feature.MonitoringOperatingTimeLastResetEvent });
                monitoringEvents.Nodes.Add(new FeatureNode() { Name = "Monitoring/OperatingTime/LastReboot",                    Feature = Feature.MonitoringOperatingTimeLastRebootEvent });
                monitoringEvents.Nodes.Add(new FeatureNode() { Name = "Monitoring/OperatingTime/LastClockSynchronization",      Feature = Feature.MonitoringOperatingTimeLastClockSynchronizationEvent });
                monitoringEvents.Nodes.Add(new FeatureNode() { Name = "Monitoring/Backup/Last",                                 Feature = Feature.MonitoringBackupLastEvent });
                monitoringEvents.Nodes.Add(new FeatureNode() { Name = "Device/HardwareFailure/FanFailure",                      Feature = Feature.DeviceHardwareFailureFanFailureEvent });
                monitoringEvents.Nodes.Add(new FeatureNode() { Name = "DeviceHardware/FailurePower/SupplyFailure",              Feature = Feature.DeviceHardwareFailurePowerSupplyFailureEvent });
                monitoringEvents.Nodes.Add(new FeatureNode() { Name = "Device/HardwareFailure/StorageFailure",                  Feature = Feature.DeviceHardwareFailureStorageFailureEvent });
                monitoringEvents.Nodes.Add(new FeatureNode() { Name = "Device/HardwareFailure/TemperatureCritical",             Feature = Feature.DeviceHardwareFailureTemperatureCriticalEvent });
            }

            {
                FeatureNode events = new FeatureNode() { Name = "Events", Feature = Feature.EventsService };
                featuresSet.Nodes.Add(events);

                events.Nodes.Add(new FeatureNode() { Name = "Persistent Notification Storage", Feature = Feature.PersistentNotificationStorage });
                events.Nodes.Add(new FeatureNode() { Name = "WS Basic Notification", Feature = Feature.WSBasicNotification });

                var getServiceCapabilities = new FeatureNode()
                                            {
                                                Name = "EventServiceCapabilities",
                                                DisplayName = "Get Service Capabilities",
                                                Feature = Feature.EventsServiceCapabilities,
                                                State = FeatureState.Undefined,
                                                Status = FeatureStatus.Group
                                            };
                getServiceCapabilities.Nodes.Add(new FeatureNode() { Name = "MaxPullPoints", Feature = Feature.MaxPullPoints });

                events.Nodes.Add(getServiceCapabilities);
            }
            FeatureNode media = new FeatureNode() { Name = "Media", Feature = Feature.MediaService };
            featuresSet.Nodes.Add(media);

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

            {

                FeatureNode rtss = new FeatureNode()
                                       {
                                           Name = "RTSS",
                                           Feature = Feature.RTSS,
                                           State = FeatureState.Undefined,
                                           Status = FeatureStatus.Feature
                                       };
                media.Nodes.Add(rtss);

                rtss.Nodes.Add(new FeatureNode() {Name = "RTPUDP", Feature = Feature.RTPUDP});
                rtss.Nodes.Add(new FeatureNode() {Name = "RTPRTSPHTTP", Feature = Feature.RTPRTSPHTTP});
                rtss.Nodes.Add(new FeatureNode() {Name = "RTPRTSPTCP", Feature = Feature.RTPRTSPTCP});
                rtss.Nodes.Add(new FeatureNode() {Name = "RTPMulticastUDP", Feature = Feature.RTPMulticastUDP});

                media.Nodes.Add(new FeatureNode() { Name = "SnapshotURI", Feature = Feature.SnapshotUri });
            }

            {
                FeatureNode ptz = new FeatureNode() { Name = "PTZ", Feature = Feature.PTZService };
                featuresSet.Nodes.Add(ptz);

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

                ptz.Nodes.AddRange(new FeatureNode[]
                                       {
                                           ptzAbsolute, ptzRelative, ptzContinious, ptzPresets, ptzHome, ptzAuxiliary,
                                           ptzSpeed
                                       });
            }

            {
                FeatureNode io = new FeatureNode() { Name = "IO", Feature = Feature.DeviceIoService };
                featuresSet.Nodes.Add(io);
                io.Nodes.Add(new FeatureNode() { Name = "RelayOutput", Feature = Feature.RelayOutputs });
                io.Nodes.Add(new FeatureNode() { Name = "DigitalInputs", Feature = Feature.DigitalInputs });
            }

            {
                FeatureNode imaging = new FeatureNode() { Name = "Imaging", Feature = Feature.ImagingService };
                featuresSet.Nodes.Add(imaging);
                imaging.Nodes.Add(new FeatureNode() { Name = "IrCutfilter Configuration", Feature = Feature.IrCutfilterConfiguration });
            }

            {
                FeatureNode analytics = new FeatureNode() { Name = "Analytics", Feature = Feature.AnalyticsService };
                featuresSet.Nodes.Add(analytics);
            }

            {
                FeatureNode recording = new FeatureNode() { Name = "Recording", Feature = Feature.RecordingControlService };
                featuresSet.Nodes.Add(recording);
                FeatureNode dynamicRecording = new FeatureNode() { Name = "DynamicRecordings", Feature = Feature.DynamicRecordings };
                recording.Nodes.Add(dynamicRecording);

                FeatureNode dynamicTracks = new FeatureNode() { Name = "DynamicTracks", Feature = Feature.DynamicTracks };
                recording.Nodes.Add(dynamicTracks);

                FeatureNode audioRecording = new FeatureNode() { Name = "AudioRecording", Feature = Feature.AudioRecording};
                recording.Nodes.Add(audioRecording);

                recording.Nodes.Add(new FeatureNode() { Name = "RecordingOptions", Feature = Feature.RecordingOptions });

                recording.Nodes.Add(new FeatureNode() { Name = "DeleteTrackDataEvent", Feature = Feature.RecordingConfigDeleteTrackDataEvent });

                recording.Nodes.Add(new FeatureNode() { Name = "MetadataRecording", Feature = Feature.MetadataRecording });
            }

            {
                FeatureNode search = new FeatureNode() { Name = "Search", Feature = Feature.RecordingSearchService };
                featuresSet.Nodes.Add(search);
                FeatureNode metadataSearch = new FeatureNode() { Name = "MetadataSearch", Feature = Feature.MetadataSearch };
                search.Nodes.Add(metadataSearch);
                FeatureNode ptzSearch = new FeatureNode() { Name = "PTZPositionSearch", Feature = Feature.PTZPositionSearch };
                search.Nodes.Add(ptzSearch);
            }

            FeatureNode doorControl = new FeatureNode() { Name = "DoorControl", Feature = Feature.DoorControlService };
            FeatureNode doorEntityNode = CreateDoorEntityNode();
            FeatureNode eventNode = CreateDoorEventsNode();
            
            doorControl.Nodes.Add(doorEntityNode);
            doorControl.Nodes.Add(eventNode);
            featuresSet.Nodes.Add(doorControl);

            FeatureNode pacs = CreatePacsServiceNode();
            featuresSet.Nodes.Add(pacs);

            {
                FeatureNode replay = new FeatureNode() { Name = "Replay", Feature = Feature.ReplayService };
                featuresSet.Nodes.Add(replay);
                replay.Nodes.Add(new FeatureNode() { Name = "ReverseReplay", Feature = Feature.ReverseReplay });
                replay.Nodes.Add(new FeatureNode() { Name = "ReplayRTPRTSPTCP", Feature = Feature.ReplayServiceRTPRTSPTCP });
            }

            {
                FeatureNode receiver = new FeatureNode() { Name = "Receiver", Feature = Feature.ReceiverService };
                featuresSet.Nodes.Add(receiver);

            }

            featuresSet.Nodes.Add(CreateAdvancedSecurityNode());

            featuresSet.Nodes.Add(CreateCredentialNode());

            featuresSet.Nodes.Add(CreateAccessRulesNode());

            featuresSet.Nodes.Add(CreateScheduleNode());

            return featuresSet;
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
                FeatureNode doorEventNode = new FeatureNode() { Name = "DoorFaultEvent", Feature = Feature.DoorFaultEvent };
                eventNode.Nodes.Add(doorEventNode);
            }
            {
                FeatureNode doorEventNode = new FeatureNode() { Name = "DoorChangedEvent", Feature = Feature.DoorChangedEvent };
                eventNode.Nodes.Add(doorEventNode);
            }
            {
                FeatureNode doorEventNode = new FeatureNode() { Name = "DoorRemovedEvent", Feature = Feature.DoorRemovedEvent };
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
            
            //FeatureNode tamper = new FeatureNode() { Name = "Tamper", Feature = Feature.Tamper };
            //accessPointEntityNode.Nodes.Add(tamper);
            
            FeatureNode anonymousAccess = new FeatureNode() { Name = "AnonymousAccess", Feature = Feature.AnonymousAccess };
            accessPointEntityNode.Nodes.Add(anonymousAccess);

            //FeatureNode hostResponseNode = new FeatureNode() { Name = "DoorEntity", Feature = Feature.HostResponse };
            //pacs.Nodes.Add(hostResponseNode);

            FeatureNode accessPointEventsNode = CreatePacsEventsNode();
            pacs.Nodes.Add(accessPointEventsNode);

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
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedCredentialEvent", Feature = Feature.AccessDeniedCredentialEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedAnonymousEvent", Feature = Feature.AccessDeniedAnonymousEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessDeniedCredentialCredentialNotFoundCardEvent", Feature = Feature.AccessDeniedCredentialCredentialNotFoundCardEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "DuressEvent", Feature = Feature.DuressEvent };
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
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "RequestTimeoutEvent", Feature = Feature.RequestTimeoutEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessPointStateEnabledEvent", Feature = Feature.AccessPointStateEnabledEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessPointChangedEvent", Feature = Feature.AccessPointChangedEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AccessPointRemovedEvent", Feature = Feature.AccessPointRemovedEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AreaChangedEvent", Feature = Feature.AreaChangedEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }
            {
                FeatureNode accessPointEventNode = new FeatureNode() { Name = "AreaRemovedEvent", Feature = Feature.AreaRemovedEvent };
                accessPointEventsNode.Nodes.Add(accessPointEventNode);
            }

            return accessPointEventsNode;
        }

        static FeatureNode CreateAdvancedSecurityNode()
        {
            var asNode = new FeatureNode() { Name = "Advanced Security", Feature = Feature.AdvancedSecurity };

            var keyStoreNode = new FeatureNode() { Name = "Keystore features support", Feature = Feature.KeyStoreFeaturesSupport, Status = FeatureStatus.Group };

            keyStoreNode.Nodes.Add(new FeatureNode() { Name = "RSA Key Pair Generation", Feature = Feature.RSAKeyPairGeneration });
            keyStoreNode.Nodes.Add(new FeatureNode() { Name = "PKCS10 External Certification with RSA", Feature = Feature.PKCS10ExternalCertificationWithRSA });
            keyStoreNode.Nodes.Add(new FeatureNode() { Name = "Self-Signed Certificate Creation with RSA", Feature = Feature.SelfSignedCertificateCreationWithRSA });
            keyStoreNode.Nodes.Add(new FeatureNode() { Name = "Passphrase Management", Feature = Feature.PassphraseManagement });
            keyStoreNode.Nodes.Add(new FeatureNode() { Name = "PKCS#8 Container Upload", Feature = Feature.PKCS8RSAKeyPairUpload });
            keyStoreNode.Nodes.Add(new FeatureNode() { Name = "PKCS#12 Container Upload", Feature = Feature.PKCS12CertificateWithRSAPrivateKeyUpload });
            keyStoreNode.Nodes.Add(new FeatureNode() { Name = "CRLs", Feature = Feature.CRLs });
            keyStoreNode.Nodes.Add(new FeatureNode() { Name = "Certification path validation policies", Feature = Feature.CertificationPathValidationPolicies });
            keyStoreNode.Nodes.Add(new FeatureNode() { Name = "TLS WWW client auth extended key usage extension", Feature = Feature.TLSWebClientAuthExtKeyUsage });

            var tlsServerNode = new FeatureNode() { Name = "TLS features support", Feature = Feature.TLSFeaturesSupport, Status = FeatureStatus.Group };
            tlsServerNode.Nodes.Add(new FeatureNode() { Name = "TLS Server Support", Feature = Feature.TLSServerSupport });
            tlsServerNode.Nodes.Add(new FeatureNode() { Name = "TLS client authentication", Feature = Feature.TLSClientAuthentication });

            // Temprary commented according ticket #971
            //var featuresSupport802Dot1X = new FeatureNode() { Name = "802.1X features support", Feature = Feature.FeaturesSupport802Dot1X, Status = FeatureStatus.Group };
            //featuresSupport802Dot1X.Nodes.Add(new FeatureNode() { Name = "802.1X configurations", Feature = Feature.Configurations802Dot1X });

            asNode.Nodes.Add(keyStoreNode);
            asNode.Nodes.Add(tlsServerNode);
            // Temprary commented according ticket #971
            //asNode.Nodes.Add(featuresSupport802Dot1X);

            return asNode;
        }

        static FeatureNode CreateCredentialNode()
        {
            var csNode = new FeatureNode() { Name = "Credential", Feature = Feature.Credential };

            csNode.Nodes.Add(new FeatureNode() { Name = "Credential Validity", Feature = Feature.CredentialValidity });
            csNode.Nodes.Add(new FeatureNode() { Name = "Credential Access Profile Validity", Feature = Feature.CredentialAccessProfileValidity });
            csNode.Nodes.Add(new FeatureNode() { Name = "pt:Card", Feature = Feature.PtCard });
            csNode.Nodes.Add(new FeatureNode() { Name = "pt:Face", Feature = Feature.PtFace });
            csNode.Nodes.Add(new FeatureNode() { Name = "pt:Iris", Feature = Feature.PtIris });
            csNode.Nodes.Add(new FeatureNode() { Name = "pt:PIN", Feature = Feature.PtPIN });
            csNode.Nodes.Add(new FeatureNode() { Name = "pt:Vein", Feature = Feature.PtVein });
            csNode.Nodes.Add(new FeatureNode() { Name = "pt:Fingerprint", Feature = Feature.PtFingerprint });
            csNode.Nodes.Add(new FeatureNode() { Name = "Reset Antipassback Violation", Feature = Feature.ResetAntipassbackViolation });
            csNode.Nodes.Add(new FeatureNode() { Name = "Validity Supports Time Value", Feature = Feature.ValiditySupportsTimeValue });

            return csNode;
        }

        static FeatureNode CreateScheduleNode()
        {
            var sNode = new FeatureNode() { Name = "Schedule", Feature = Feature.Schedule };

            sNode.Nodes.Add(new FeatureNode() { Name = "Extended Recurrence", Feature = Feature.ExtendedRecurrence });
            sNode.Nodes.Add(new FeatureNode() { Name = "Special Days", Feature = Feature.SpecialDays });
            sNode.Nodes.Add(new FeatureNode() { Name = "State Reporting", Feature = Feature.StateReporting });

            return sNode;
        }

        static FeatureNode CreateAccessRulesNode()
        {
            var csNode = new FeatureNode() { Name = "Access Rules", Feature = Feature.AccessRulesService };

            csNode.Nodes.Add(new FeatureNode() { Name = "Multiple Schedules per Access Point", Feature = Feature.MultipleSchedulesAccessPoint });

            return csNode;
        }


    }
}
