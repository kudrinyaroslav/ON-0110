using System.Collections.Generic;
using TestTool.Tests.Common.Enums;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Set of all features
    /// </summary>
    class FeaturesSet
    {
        public FeaturesSet()
        {
            Nodes = new List<FeatureNode>();
        }

        public List<FeatureNode> Nodes { get; private set; }
        
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
        /// <returns></returns>
        public static FeaturesSet CreateFeaturesSet()
        {
            FeaturesSet featuresSet = new FeaturesSet();
            
            FeatureNode security = new FeatureNode()
            {
                Name = "Security",
                Feature = Feature.Security,
                Status = FeatureState.Group
            };
            featuresSet.Nodes.Add(security);

            security.Nodes.Add(new FeatureNode() { Name = "WSU", Feature = Feature.WSU });
            security.Nodes.Add(new FeatureNode() { Name = "Digest", Feature = Feature.Digest });

            FeatureNode discovery = new FeatureNode()
            {
                Name = "Discovery",
                Feature = Feature.Discovery,
                Status = FeatureState.Group
            };
            featuresSet.Nodes.Add(discovery);

            discovery.Nodes.Add(new FeatureNode() { Name = "BYE", Feature = Feature.BYE });
            
            /******************************************/
            
            FeatureNode deviceService = new FeatureNode()
            {
                Name = "DeviceService",
                Feature = Feature.DeviceService,
                Status = FeatureState.Group
            };
            featuresSet.Nodes.Add(deviceService);

            FeatureNode capabilities = new FeatureNode() { Name = "Capabilities", Feature = Feature.Capabilities, Status = FeatureState.Group };
            deviceService.Nodes.Add(capabilities);
            capabilities.Nodes.Add(new FeatureNode() { Name = "GetCapabilities", Feature = Feature.GetCapabilities });
            capabilities.Nodes.Add(new FeatureNode() { Name = "GetServices", Feature = Feature.GetServices });

            FeatureNode network = new FeatureNode()
            {
                Name = "Network",
                Feature = Feature.Network,
                Status = FeatureState.Group
            };
            deviceService.Nodes.Add(network);

            network.Nodes.Add(new FeatureNode()
            {
                Name = "ZeroConfiguration",
                Feature = Feature.ZeroConfiguration
            });
            network.Nodes.Add(new FeatureNode() { Name = "NTP", Feature = Feature.NTP });
            network.Nodes.Add(new FeatureNode() { Name = "IPv6", Feature = Feature.IPv6 });

            FeatureNode system = new FeatureNode()
            {
                Name = "System",
                Feature = Feature.System,
                Status = FeatureState.Group
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
                Status = FeatureState.Group
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

            FeatureNode events = new FeatureNode() { Name = "Events", Feature = Feature.EventsService };
            featuresSet.Nodes.Add(events);

            FeatureNode media = new FeatureNode() { Name = "Media", Feature = Feature.MediaService };
            featuresSet.Nodes.Add(media);

            FeatureNode video = new FeatureNode() { Name = "VideoCodec", Feature = Feature.Video, Status = FeatureState.Group };
            media.Nodes.Add(video);
            video.Nodes.Add(new FeatureNode() { Name = "JPEG", Feature = Feature.JPEG });
            video.Nodes.Add(new FeatureNode() { Name = "H264",  Feature = Feature.H264 });
            video.Nodes.Add(new FeatureNode() { Name = "MPEG4",  Feature = Feature.MPEG4 });

            FeatureNode audio = new FeatureNode() { Name = "Audio", Feature = Feature.Audio };
            media.Nodes.Add(audio);

            audio.Nodes.Add(new FeatureNode() { Name = "G711",  Feature = Feature.G711 });
            audio.Nodes.Add(new FeatureNode() { Name = "G726",  Feature = Feature.G726 });
            audio.Nodes.Add(new FeatureNode() { Name = "AAC", Feature = Feature.AAC });

            FeatureNode backchannel = new FeatureNode()
            {
                Name = GetDisplayName(Feature.Backchannel),
                Feature = Feature.Backchannel
            };
            media.Nodes.Add(backchannel);
            backchannel.Nodes.Add(new FeatureNode() { Name = "BackchannelG711", Feature = Feature.BackchannelG711 });
            backchannel.Nodes.Add(new FeatureNode() { Name = "BackchannelG726", Feature = Feature.BackchannelG726 });
            backchannel.Nodes.Add(new FeatureNode() { Name = "BackchannelAAC", Feature = Feature.BackchannelAAC });



            FeatureNode rtss = new FeatureNode() { Name = "RTSS", Feature = Feature.RTSS, Status = FeatureState.Group };
            media.Nodes.Add(rtss);

            rtss.Nodes.Add(new FeatureNode() { Name = "RTPUDP", Feature = Feature.RTPUDP });
            rtss.Nodes.Add(new FeatureNode() { Name = "RTPRTSPHTTP", Feature = Feature.RTPRTSPHTTP });
            rtss.Nodes.Add(new FeatureNode() { Name = "RTPRTSPTCP", Feature = Feature.RTPRTSPTCP });
            rtss.Nodes.Add(new FeatureNode() { Name = "RTPMulticastUDP", Feature = Feature.RTPMulticastUDP });
            rtss.Nodes.Add(new FeatureNode() { Name = "SnapshotURI", Feature = Feature.SnapshotUri });

            FeatureNode ptz = new FeatureNode() { Name = "PTZ", Feature = Feature.PTZService };
            featuresSet.Nodes.Add(ptz);

            FeatureNode ptzAbsolute = new FeatureNode() { Name = "PTZAbsolute", Feature = Feature.PTZAbsolute };
            ptzAbsolute.Nodes.Add(new FeatureNode() { Name = "PanTiltMovement", Feature = Feature.PTZAbsolutePanTilt });
            ptzAbsolute.Nodes.Add(new FeatureNode() { Name = "ZoomMovement", Feature = Feature.PTZAbsoluteZoom });
            FeatureNode ptzRelative = new FeatureNode() { Name = "PTZRelative", Feature = Feature.PTZRelative };
            ptzRelative.Nodes.Add(new FeatureNode() { Name = "PanTiltMovement", Feature = Feature.PTZRelativePanTilt });
            ptzRelative.Nodes.Add(new FeatureNode() { Name = "ZoomMovement",  Feature = Feature.PTZRelativeZoom });
            FeatureNode ptzContinious = new FeatureNode() { Name = "PTZContinuous", Feature = Feature.PTZContinious };
            ptzContinious.Nodes.Add(new FeatureNode() { Name = "PanTiltSpeedConfiguration", Feature = Feature.PTZContinuousPanTilt });
            ptzContinious.Nodes.Add(new FeatureNode() { Name = "ZoomSpeedConfiguration", Feature = Feature.PTZContinuousZoom });
            FeatureNode ptzPresets = new FeatureNode() { Name = "PTZPresets", Feature = Feature.PTZPresets };
            FeatureNode ptzHome = new FeatureNode() { Name = "PTZHome", Feature = Feature.PTZHome };
            ptzHome.Nodes.Add(new FeatureNode() { Name = "ConfigurableHomePosition",  Feature = Feature.PTZConfigurableHome });
            ptzHome.Nodes.Add(new FeatureNode() { Name = "FixedHomePosition",  Feature = Feature.PTZFixedHome });
            FeatureNode ptzAuxiliary = new FeatureNode() { Name = "PTZAuxiliary", Feature = Feature.PTZAuxiliary };
            FeatureNode ptzSpeed = new FeatureNode() { Name = "PTZSpeed", Feature = Feature.PTZSpeed };
            ptzSpeed.Nodes.Add(new FeatureNode() { Name = "PanTiltSpeed", Feature = Feature.PTZSpeedPanTilt });
            ptzSpeed.Nodes.Add(new FeatureNode() { Name = "ZoomSpeed", Feature = Feature.PTZSpeedZoom });

            ptz.Nodes.AddRange(new FeatureNode[] { ptzAbsolute, ptzRelative, ptzContinious, ptzPresets, ptzHome, ptzAuxiliary, ptzSpeed });

            FeatureNode io = new FeatureNode() { Name = "IO", Feature = Feature.DeviceIoService };
            featuresSet.Nodes.Add(io);
            io.Nodes.Add(new FeatureNode() { Name = "RelayOutput", Feature = Feature.RelayOutputs });

            FeatureNode imaging = new FeatureNode() { Name = "Imaging", Feature = Feature.ImagingService };
            featuresSet.Nodes.Add(imaging);

            FeatureNode analytics = new FeatureNode() { Name = "Analytics", Feature = Feature.AnalyticsService };
            featuresSet.Nodes.Add(analytics);

            return featuresSet;
        }



    }
}
