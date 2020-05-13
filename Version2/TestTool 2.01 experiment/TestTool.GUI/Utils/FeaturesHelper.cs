///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using TestTool.Tests.Common.Enums;
using TestTool.GUI.Data;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Contains methods for handling Features logic.
    /// </summary>
    static class FeaturesHelper
    {

        /// <summary>
        /// Types of feature realization
        /// </summary>
        public enum FeatureRealizationType
        {
            Supported,
            Implemented
        }

        /// <summary>
        /// Adds selected feature to a list of features for test log. Uses default display name.
        /// </summary>
        /// <param name="features">List of features.</param>
        /// <param name="log">Certification log.</param>
        /// <param name="feature">Feature</param>
        public static void AddIfSelected(this List<string> features, TestLogFull log, Feature feature)
        {
            AddIfSelected(features, log, feature, GetDisplayName(feature));
        }

        /// <summary>
        /// Adds selected feature to a list of features for test log. Uses default display name.
        /// </summary>
        /// <param name="features">List of features.</param>
        /// <param name="log">Certification log.</param>
        /// <param name="feature">Feature</param>
        /// <param name="displayName">Description to be used (it can be different from feature display name
        /// e.g. for nodes in PTZ subtree.)</param>
        public static void AddIfSelected(this List<string> features, TestLogFull log, Feature feature, string displayName)
        {
            if (log.DeviceEnvironment.Features.Contains(feature))
            {
                features.Add(displayName);
            }
        }

        /// <summary>
        /// Creates list of selected features display names. Skips features which are under 
        /// unselected parent features. 
        /// </summary>
        /// <param name="log">Tests execution information.</param>
        /// <returns>List of features selected for tests execution.</returns>
        public static List<string> SelectedFeatures(TestLogFull log)
        {
            List<string> features = new List<string>();

            features.AddIfSelected(log, Feature.NTP);
            features.AddIfSelected(log, Feature.IPv6);

            features.AddIfSelected(log, Feature.JPEG);
            features.AddIfSelected(log, Feature.H264, GetDisplayName(Feature.H264));
            features.AddIfSelected(log, Feature.MPEG4);
            // add only if top-level feature is selected
            if (log.DeviceEnvironment.Features.Contains(Feature.Audio))
            {
                features.AddIfSelected(log, Feature.G711);
                features.AddIfSelected(log, Feature.G726);
                features.AddIfSelected(log, Feature.AAC);
            }
            
            features.AddIfSelected(log, Feature.RTPUDP);
            features.AddIfSelected(log, Feature.RTPRTSPHTTP);
            features.AddIfSelected(log, Feature.RTPRTSPTCP);

            // add only if top-level feature is selected
            if (log.DeviceEnvironment.Features.Contains(Feature.PTZ))
            {
                //
                // here low-level features are cleared if top-level feature is cleared, so 
                //  "PTZAbsoluteZoom" does not depend on "Absolute" node selection
                // 
                features.AddIfSelected(log, Feature.PTZAbsolutePanTilt, "Absolute move - Pan/Tilt movement");
                features.AddIfSelected(log, Feature.PTZAbsoluteZoom, "Absolute move - Zoom movement");
                features.AddIfSelected(log, Feature.PTZRelativePanTilt, "Relative move - Pan/Tilt movement");
                features.AddIfSelected(log, Feature.PTZRelativeZoom, "Relative move - Zoom movement");
                features.AddIfSelected(log, Feature.PTZContiniousPanTilt,
                                       "Continous move - Pan/Tilt speed configuration");
                features.AddIfSelected(log, Feature.PTZContiniousZoom, "Continous move - Zoom speed configuration");
                features.AddIfSelected(log, Feature.PTZPresets, "Presets");
                features.AddIfSelected(log, Feature.PTZConfigurableHome, "Configurable home position");
                features.AddIfSelected(log, Feature.PTZFixedHome, "Fixed home position");
                features.AddIfSelected(log, Feature.PTZAuxiliary, "Auxiliary operation");
            }

            return features;

        }
    
        /// <summary>
        /// Creates features tree. Initializes checked state; sets up mandatory features.
        /// </summary>
        /// <returns></returns>
        public static FeaturesSet CreateFeaturesSet()
        {
            FeaturesSet featuresSet = new FeaturesSet();

            //
            // MANAGEMENT
            //
            FeatureNode management = new FeatureNode()
                                         {
                                             Name = "Management",
                                             DisplayName = "Device Management Service",
                                             Feature = Feature.ManagementService,
                                             State = FeatureState.Optional,
                                             Enabled = true
                                         };

            featuresSet.Nodes.Add(management);
            management.Nodes.Add(new FeatureNode() { Name = "NTP", DisplayName = GetDisplayName(Feature.NTP), Feature = Feature.NTP });
            management.Nodes.Add(new FeatureNode() { Name = "IPv6", DisplayName = GetDisplayName(Feature.IPv6), Feature = Feature.IPv6 });
            
            //
            // EVENTS
            //
            FeatureNode events = new FeatureNode()
                                     {
                                         Name = "Events", 
                                         DisplayName = "Events Service", 
                                         Feature = Feature.EventsService, 
                                         State = FeatureState.Optional,
                                         Enabled = true
                                     };
            featuresSet.Nodes.Add(events);

            //
            // MEDIA
            //

            FeatureNode media = new FeatureNode()
                                    {
                                        Name = "Media", 
                                        DisplayName = "Media Service", 
                                        Feature = Feature.MediaService,
                                        State =  FeatureState.Optional,
                                        Enabled = true
                                    };
            featuresSet.Nodes.Add(media);
            
            FeatureNode video = new FeatureNode() { Name = "VideoCodec", DisplayName = "Video codec", Feature = Feature.Video, Mandatory = true};
            media.Nodes.Add(video);
            video.Nodes.Add(new FeatureNode() { Name = "JPEG", DisplayName = "JPEG", Feature = Feature.JPEG, Mandatory = true });
            video.Nodes.Add(new FeatureNode() { Name = "H264", DisplayName = GetDisplayName(Feature.H264), Feature = Feature.H264 });
            video.Nodes.Add(new FeatureNode() { Name = "MPEG4", DisplayName = GetDisplayName(Feature.MPEG4), Feature = Feature.MPEG4 });

            FeatureNode audio = new FeatureNode() { Name = "Audio", DisplayName = "Audio", Feature = Feature.Audio };
            media.Nodes.Add(audio);

            audio.Nodes.Add(new FeatureNode() { Name = "G711", DisplayName = "G.711", Feature = Feature.G711, Mandatory = true });
            audio.Nodes.Add(new FeatureNode() { Name = "G726", DisplayName = GetDisplayName(Feature.G726), Feature = Feature.G726 });
            audio.Nodes.Add(new FeatureNode() { Name = "AAC", DisplayName = "AAC", Feature = Feature.AAC });

            FeatureNode rtss = new FeatureNode() { Name = "RTSS", DisplayName = "Real-time Streaming Setup", Feature = Feature.RTSS, Mandatory = true };
            media.Nodes.Add(rtss);

            rtss.Nodes.Add(new FeatureNode() { Name = "RTPUDP", DisplayName = "RTP/UDP", Feature = Feature.RTPUDP, Mandatory = true });
            rtss.Nodes.Add(new FeatureNode() { Name = "RTPRTSPHTTP", DisplayName = "RTP/RTSP/HTTP", Feature = Feature.RTPRTSPHTTP, Mandatory = true });
            rtss.Nodes.Add(new FeatureNode() { Name = "RTPRTSPTCP", DisplayName = GetDisplayName(Feature.RTPRTSPTCP), Feature = Feature.RTPRTSPTCP });

            //
            // PTZ
            //

            FeatureNode ptz = new FeatureNode()
                                  {
                                      Name = "PTZ", 
                                      DisplayName = GetDisplayName(Feature.PTZ), 
                                      Feature = Feature.PTZ, 
                                      State = FeatureState.Optional,
                                      Enabled = true
                                  };
            featuresSet.Nodes.Add(ptz);

            FeatureNode ptzAbsolute = new FeatureNode() { Name = "PTZAbsolute", DisplayName = "Absolute move", Feature = Feature.PTZAbsolute };
            ptzAbsolute.Nodes.Add(new FeatureNode() { Name = "PanTiltMovement", DisplayName = "Pan/Tilt movement", Feature = Feature.PTZAbsolutePanTilt });
            ptzAbsolute.Nodes.Add(new FeatureNode() { Name = "ZoomMovement", DisplayName = "Zoom movement", Feature = Feature.PTZAbsoluteZoom });

            FeatureNode ptzRelative = new FeatureNode() { Name = "PTZRelative", DisplayName = "Relative move", Feature = Feature.PTZRelative };
            ptzRelative.Nodes.Add(new FeatureNode() { Name = "PanTiltMovement", DisplayName = "Pan/Tilt movement", Feature = Feature.PTZRelativePanTilt });
            ptzRelative.Nodes.Add(new FeatureNode() { Name = "ZoomMovement", DisplayName = "Zoom movement", Feature = Feature.PTZRelativeZoom });

            FeatureNode ptzContinious = new FeatureNode() { Name = "PTZContinious", DisplayName = "Continous move", Feature = Feature.PTZContinious, Mandatory = true };
            ptzContinious.Nodes.Add(new FeatureNode() { Name = "PanTiltSpeedConfiguration", DisplayName = "Pan/Tilt speed configuration", Feature = Feature.PTZContiniousPanTilt });
            ptzContinious.Nodes.Add(new FeatureNode() { Name = "ZoomSpeedConfiguration", DisplayName = "Zoom speed configuration", Feature = Feature.PTZContiniousZoom });

            FeatureNode ptzPresets = new FeatureNode() { Name = "PTZPresets", DisplayName = "Presets", Feature = Feature.PTZPresets };

            FeatureNode ptzHome = new FeatureNode() { Name = "PTZHome", DisplayName = "Home position", Feature = Feature.PTZHome };
            ptzHome.Nodes.Add(new FeatureNode() { Name = "ConfigurableHomePosition", DisplayName = "Configurable home position", Feature = Feature.PTZConfigurableHome });
            ptzHome.Nodes.Add(new FeatureNode() { Name = "FixedHomePosition", DisplayName = "Fixed home position", Feature = Feature.PTZFixedHome });
            
            FeatureNode ptzAuxiliary = new FeatureNode() { Name = "PTZAuxiliary", DisplayName = "Auxiliary operation", Feature = Feature.PTZAuxiliary };

            FeatureNode ptzSpeed = new FeatureNode() { Name = "PTZSpeed", DisplayName = "Speed", Feature = Feature.PTZSpeed };
            ptzSpeed.Nodes.Add(new FeatureNode() { Name = "PanTiltSpeed", DisplayName = "Speed for pan/tilt", Feature = Feature.PTZSpeedPanTilt });
            ptzSpeed.Nodes.Add(new FeatureNode() { Name = "ZoomSpeed", DisplayName = "Speed for zoom", Feature = Feature.PTZSpeedZoom });

            ptz.Nodes.AddRange(new FeatureNode[] { ptzAbsolute, ptzRelative, ptzContinious, ptzPresets, ptzHome, ptzAuxiliary, ptzSpeed });

            //foreach (FeatureNode node in featuresSet.Nodes)
            //{
            //    SetupFeatureNode(null, node);
            //}

            return featuresSet;
        }

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
        /// Initializes display names.
        /// </summary>
        static void InitDisplayNames()
        {
            _displayNames = new Dictionary<Feature, string>();

            _displayNames.Add(Feature.H264, "H.264-Baseline");
            _displayNames.Add(Feature.MPEG4, "MPEG4-SP");
            _displayNames.Add(Feature.G726, "G.726");
            _displayNames.Add(Feature.G711, "G.711");

            _displayNames.Add(Feature.RTSS, "RTSS");
            _displayNames.Add(Feature.RTPUDP, "RTP/UDP");
            _displayNames.Add(Feature.RTPRTSPHTTP, "RTP/RTSP/HTTP");
            _displayNames.Add(Feature.RTPRTSPTCP, "RTP/RTSP/TCP");
            
            _displayNames.Add(Feature.PTZAbsoluteOrRelative, "AbsoluteMove OR RelativeMove");
            
            _displayNames.Add(Feature.PTZAbsolutePanTilt, "Absolute move - Pan/Tilt movement");
            _displayNames.Add(Feature.PTZAbsoluteZoom, "Absolute move - Zoom movement");
            _displayNames.Add(Feature.PTZRelativePanTilt, "Relative move - Pan/Tilt movement");
            _displayNames.Add(Feature.PTZRelativeZoom, "Relative move - Zoom movement");
            _displayNames.Add(Feature.PTZContiniousPanTilt,
                                   "Continous move - Pan/Tilt speed configuration");
            _displayNames.Add(Feature.PTZContiniousZoom, "Continous move - Zoom speed configuration");
            _displayNames.Add(Feature.PTZPresets, "Presets");
            _displayNames.Add(Feature.PTZConfigurableHome, "Configurable home position");
            _displayNames.Add(Feature.PTZFixedHome, "Fixed home position");
            _displayNames.Add(Feature.PTZAuxiliary, "Auxiliary operation");

            _displayNames.Add(Feature.PTZAbsolute, "PTZ Absolute");
            _displayNames.Add(Feature.PTZRelative, "PTZ Relative");

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
        /// Feature realization types for features.
        /// </summary>
        private static Dictionary<Feature, FeatureRealizationType> _featureRealization;
        
        /// <summary>
        /// Initializes features realization.
        /// </summary>
        static void InitFeatureRealization()
        {
            _featureRealization = new Dictionary<Feature, FeatureRealizationType>();

            _featureRealization.Add(Feature.NTP, FeatureRealizationType.Supported);
            _featureRealization.Add(Feature.Audio, FeatureRealizationType.Supported);
            _featureRealization.Add(Feature.PTZ, FeatureRealizationType.Supported);

            _featureRealization.Add(Feature.IPv6, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.AAC, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.G726, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.H264, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.MPEG4, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.RTPRTSPTCP, FeatureRealizationType.Implemented); 
            _featureRealization.Add(Feature.PTZAbsolute, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZAbsolutePanTilt, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZAbsoluteZoom, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZContiniousPanTilt, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZContiniousZoom, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZRelative, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZRelativePanTilt, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZRelativeZoom, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZSpeedPanTilt, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZSpeedZoom, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZPresets, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZHome, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZConfigurableHome, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZFixedHome, FeatureRealizationType.Implemented);
            _featureRealization.Add(Feature.PTZAuxiliary, FeatureRealizationType.Implemented);

            _featureRealization.Add(Feature.PTZAbsoluteOrRelative, FeatureRealizationType.Implemented);

        }

        /// <summary>
        /// Feature realization type (implemented or supported)
        /// </summary>
        /// <param name="feature">Feature to define realization type.</param>
        /// <returns>Realization type.</returns>
        public static FeatureRealizationType FeatureRealization(Feature feature)
        {
            if (_featureRealization == null)
            {
                InitFeatureRealization();
            }
            return _featureRealization[feature];
        }

        /// <summary>
        /// Checks if all features required for test are selected.
        /// </summary>
        /// <param name="testInfo">Test information.</param>
        /// <returns>True if all required features are selected.</returns>
        public static bool AllFeaturesSelected(Tests.Common.TestEngine.TestInfo testInfo)
        {
            DeviceEnvironment environment = Controllers.ContextController.GetDeviceEnvironment();
            List<Tests.Common.Enums.Feature> features = environment.Features;

            foreach (Service service in testInfo.Services)
            {
                if (!environment.Services.Contains(service))
                {
                    return false;
                }
            }

            if (testInfo.RequirementLevel == Tests.Common.Enums.RequirementLevel.ConditionalMust ||
                testInfo.RequirementLevel == Tests.Common.Enums.RequirementLevel.ConditionalShould)
            {
                foreach (Tests.Common.Enums.Feature feature in testInfo.RequiredFeatures)
                {
                    if (feature == Feature.PTZAbsoluteOrRelative)
                    {
                        if (!features.Contains(Feature.PTZAbsolute) 
                            && !features.Contains(Feature.PTZRelative))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!features.Contains(feature))
                        {
                            return false;
                        }                        
                    }

                }
            }
            return true;
        }
        
        /// <summary>
        /// Sets up feature node and child nodes (disables if parent is not selected or enables)
        /// </summary>
        /// <param name="parent">Parent node.</param>
        /// <param name="node">Child node.</param>
        /// <remarks>This method is called only from CreateFeaturesSet and from this method recursively.
        /// So, all root nodes (services) become "Optional" (unselected) and all nodes beneath become disabled.
        /// </remarks>
        //static void SetupFeatureNode(FeatureNode parent, FeatureNode node)
        //{
        //    if (parent == null)
        //    {
        //        node.Enabled = true;
        //    }
        //    else
        //    {
        //        node.Enabled = parent.Checked && parent.Enabled;
        //    }
        //    foreach (FeatureNode child in node.Nodes)
        //    {
        //        SetupFeatureNode(node, child);
        //    }
        //}
    
    }

}
