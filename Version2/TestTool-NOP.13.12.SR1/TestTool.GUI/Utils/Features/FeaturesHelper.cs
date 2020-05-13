///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Features;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Contains methods for handling Features logic.
    /// </summary>
    static class FeaturesHelper
    {
        /// <summary>
        /// Display names (for features where display name differs from Feature.ToString() )
        /// </summary>
        private static Dictionary<Feature, string> _displayNames;

        /// <summary>
        /// Accessor to display names.
        /// </summary>
        static Dictionary<Feature, string> DisplayNames
        {
            get
            {
                if (_displayNames == null)
                {
                    InitDisplayNames();
                }
                return _displayNames;
            }
        }

        /// <summary>
        /// Initializes display names (for test tooltips)
        /// </summary>
        static void InitDisplayNames()
        {
            _displayNames = new Dictionary<Feature, string>();

            _displayNames.Add(Feature.DeviceService, "Device Service");
            _displayNames.Add(Feature.ImagingService, "Imaging Service");
            _displayNames.Add(Feature.AnalyticsService, "Analytics Service");
            _displayNames.Add(Feature.MediaService, "Media Service");
            _displayNames.Add(Feature.EventsService, "Events Service");
            _displayNames.Add(Feature.PTZService, "PTZ Service");
            _displayNames.Add(Feature.DeviceIoService, "Device IO Service");
            _displayNames.Add(Feature.ReceiverService, "Receiver Service");

            _displayNames.Add(Feature.RecordingControlService, "Recording Control Service");
            _displayNames.Add(Feature.RecordingSearchService, "Recording Search Service");
            _displayNames.Add(Feature.ReplayService, "Replay Service");

            _displayNames.Add(Feature.GetCapabilities, "GetCapabilities");
            
            _displayNames.Add(Feature.SystemLogging, "System Logging");
            _displayNames.Add(Feature.BYE, "BYE Message");

            _displayNames.Add(Feature.WSU, "WS-UsernameToken");
            
            _displayNames.Add(Feature.IPv6, "IPv6");
            _displayNames.Add(Feature.ZeroConfiguration, "Zero Configuration");
            _displayNames.Add(Feature.DynamicDNS, "Dynamic DNS");
            _displayNames.Add(Feature.IPFilter, "IP Filter");

            _displayNames.Add(Feature.H264, "H.264");
            _displayNames.Add(Feature.MPEG4, "MPEG4");
            _displayNames.Add(Feature.SnapshotUri, "Snapshot URI");
           
            _displayNames.Add(Feature.G726, "G.726");
            _displayNames.Add(Feature.G711, "G.711");

            _displayNames.Add(Feature.RTSS, "RTSS");
            _displayNames.Add(Feature.RTPUDP, "RTP/UDP");
            _displayNames.Add(Feature.RTPRTSPHTTP, "RTP/RTSP/HTTP");
            _displayNames.Add(Feature.RTPRTSPTCP, "RTP/RTSP/TCP");
            _displayNames.Add(Feature.RTPMulticastUDP, "RTP-Multicast/UDP");

            _displayNames.Add(Feature.PTZAbsoluteOrRelative, "Absolute Movement OR Relative Movement");

            _displayNames.Add(Feature.PTZAbsoluteOrRelativePanTilt, "Absolute OR Relative Pan/Tilt Movement");
            _displayNames.Add(Feature.PTZAbsoluteOrRelativeZoom, "Absolute OR Relative Zoom Movement");

            _displayNames.Add(Feature.PTZAbsolutePanTilt, "Absolute Movement - Pan/Tilt Movement");
            _displayNames.Add(Feature.PTZAbsoluteZoom, "Absolute Movement - Zoom Movement");
            _displayNames.Add(Feature.PTZRelativePanTilt, "Relative Movement - Pan/Tilt Movement");
            _displayNames.Add(Feature.PTZRelativeZoom, "Relative Movement - Zoom Movement");
            _displayNames.Add(Feature.PTZContinuousPanTilt,
                                   "Continous Movement - Pan/Tilt Movement");
            _displayNames.Add(Feature.PTZContinuousZoom, "Continous Movement - Zoom Movement");
            _displayNames.Add(Feature.PTZPresets, "Presets");
            _displayNames.Add(Feature.PTZConfigurableHome, "Configurable home position");
            _displayNames.Add(Feature.PTZFixedHome, "Fixed home position");
            _displayNames.Add(Feature.PTZAuxiliary, "Auxiliary operation");

            _displayNames.Add(Feature.PTZAbsolute, "PTZ Absolute");
            _displayNames.Add(Feature.PTZRelative, "PTZ Relative");
            
            _displayNames.Add(Feature.DeviceIO, "I/O");
            _displayNames.Add(Feature.DeviceIORelayOutputs, "Relay outputs");

            _displayNames.Add(Feature.DeviceIORelayOutputsBistable, "Bistable");
            _displayNames.Add(Feature.DeviceIORelayOutputsBistableOpen, "Bistable Mode with Open Idle State");
            _displayNames.Add(Feature.DeviceIORelayOutputsBistableClosed, "Bistable Mode with Closed Idle State");
            _displayNames.Add(Feature.DeviceIORelayOutputsMonostable, "Monostable");
            _displayNames.Add(Feature.DeviceIORelayOutputsMonostableOpen, "Monostable Mode with Open Idle State");
            _displayNames.Add(Feature.DeviceIORelayOutputsMonostableClosed, "Monostable Mode with Closed Idle State");
            
            _displayNames.Add(Feature.RelayOutputs, "Relay outputs");

            _displayNames.Add(Feature.AudioOutput, "Audio Output");
            _displayNames.Add(Feature.AudioOutputAAC, "AAC");
            _displayNames.Add(Feature.AudioOutputG711, "G.711");
            _displayNames.Add(Feature.AudioOutputG726, "G.726");

            _displayNames.Add(Feature.ReverseReplay, "Reverse Replay");
            _displayNames.Add(Feature.DynamicRecording, "Dynamic Recording");
            _displayNames.Add(Feature.MetadataSearch, "Metadata Search");
            _displayNames.Add(Feature.PTZPositionSearch, "PTZ Position Search");

            _displayNames.Add(Feature.MediaOrReceiver, "Media Or Receiver");
            _displayNames.Add(Feature.AudioRecording , "Audio Recording");
            _displayNames.Add(Feature.MetadataRecording, "Metadata Recording");
        }

        /// <summary>
        /// Feature display name. For most features it's just feature.ToString()
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public static string GetDisplayName(Feature feature)
        {
            if (DisplayNames.ContainsKey(feature))
            {
                return DisplayNames[feature];
            }
            else
            {
                return feature.ToString();
            }
        }
        
        /// <summary>
        /// Initializes display names for features tree.
        /// </summary>
        /// <param name="featuresSet"></param>
        public static void Translate(FeaturesSet featuresSet)
        {
            foreach (FeatureNode node in featuresSet.Nodes)
            {
                Translate(node);
            }
        }

        /// <summary>
        /// Initializes display name.
        /// </summary>
        /// <param name="node"></param>
        static void Translate(FeatureNode node)
        {
            node.DisplayName = GetTreeDisplayName(node.Feature);
            foreach (FeatureNode child in node.Nodes)
            {
                Translate(child);
            }
        }

        /// <summary>
        /// Gets display name for Tree. If not overridden, "common" display name is used.
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        static string GetTreeDisplayName(Feature feature)
        {
            string name = string.Empty;

            switch (feature)
            {
                case Feature.RTSS:
                    name = "Real-time Streaming Setup";
                    break;
                case Feature.PTZAbsolute:
                    name = "Absolute move";
                    break;
                case Feature.PTZAbsolutePanTilt:
                    name = "Pan/Tilt movement";
                    break;
                case Feature.PTZAbsoluteZoom:
                    name = "Zoom movement";
                    break;
                case Feature.PTZRelative:
                    name = "Relative move";
                    break;
                case Feature.PTZRelativePanTilt:
                    name = "Pan/Tilt movement";
                    break;
                case  Feature.PTZRelativeZoom:
                    name = "Zoom movement";
                    break;
                case Feature.PTZContinious:
                    name = "Continuous move";
                    break;
                case Feature.PTZContinuousPanTilt:
                    name = "Pan/Tilt movement";
                    break;
                case Feature.PTZContinuousZoom:
                    name = "Zoom movement";
                    break;
                case Feature.PTZPresets:
                    name = "Presets";
                    break;
                case Feature.PTZHome :
                    name = "Home position";
                    break;
                case Feature.PTZConfigurableHome:
                    name = "Configurable";
                    break;
                case Feature.PTZFixedHome:
                    name = "Fixed";
                    break;
                case Feature.PTZAuxiliary :
                    name = "Auxiliary operations";
                    break;
                case Feature.PTZSpeed:
                    name = "Speed";
                    break;
                case Feature.PTZSpeedPanTilt:
                    name = "Speed for Pan/Tilt";
                    break;
                case Feature.PTZSpeedZoom:
                    name = "Speed for Zoom";
                    break;
                case Feature.DeviceIORelayOutputsBistableOpen: 
                case Feature.DeviceIORelayOutputsMonostableOpen:
                    name = "Open";
                    break;
                case Feature.DeviceIORelayOutputsBistableClosed: 
                case Feature.DeviceIORelayOutputsMonostableClosed:
                    name = "Closed";
                    break;
            }
            
            if (string.IsNullOrEmpty(name))
            {
                name = GetDisplayName(feature);
            }

            return name;
        }

    }

}
