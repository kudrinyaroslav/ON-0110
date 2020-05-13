using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CameraWebService.PTZService10
{
    public class PtzStorage
    {
        public const string _absolutePanTiltSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace";
        public const string _absoluteZoomSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace";
        protected const string _continuousPanTiltSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace";
        public const string _continuousZoomSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace";
        public const string _relativePanTiltSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace";
        public const string _relativeZoomSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace";
        public const string _speedPanTiltSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace";
        public const string _speedZoomSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace";


        private static PtzStorage _instance;
        public static PtzStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PtzStorage();
                }
                return _instance;
            }
        }

        List<PTZ.PTZNode> _nodes;

        public List<PTZ.PTZNode> Nodes
        {
            get 
            {
                if (_nodes == null)
                {
                    _nodes = new List<PTZ.PTZNode>();

                    PTZ.PTZNode node1 = new PTZ.PTZNode() { token = "ptz0" };
                    node1.SupportedPTZSpaces = new PTZ.PTZSpaces()
                    {
                        AbsolutePanTiltPositionSpace =
                        new PTZ.Space2DDescription[]
                                                   {
                                                       new PTZ.Space2DDescription()
                                                       {
                                                           URI = _absolutePanTiltSpace, 
                                                           XRange=new PTZ.FloatRange(){ Min=0, Max=10}, 
                                                           YRange=new PTZ.FloatRange(){Min=0, Max=10}
                                                       }
                                                   },
                        ContinuousPanTiltVelocitySpace =
                        new PTZ.Space2DDescription[]
                                                   {
                                                       new PTZ.Space2DDescription()
                                                       {
                                                           URI = _continuousPanTiltSpace,
                                                           XRange=new PTZ.FloatRange(){ Min=0, Max=10}, 
                                                           YRange=new PTZ.FloatRange(){Min=0, Max=10}
                                                       }
                                                   }
                    };


                    PTZ.PTZNode node2 = new PTZ.PTZNode() { token = "ptz1" };
                    node2.AuxiliaryCommands = new string[] { "command1", "command2" };
                    node2.HomeSupported = true;

                    node2.SupportedPTZSpaces = new PTZ.PTZSpaces()
                    {
                        AbsoluteZoomPositionSpace =
                        new PTZ.Space1DDescription[]
                                                   {
                                                       new PTZ.Space1DDescription()
                                                       {
                                                           URI = _absoluteZoomSpace,
                                                           XRange=new PTZ.FloatRange(){ Min=0, Max=10}
                                                       }
                                                   },
                        RelativeZoomTranslationSpace =
                        new PTZ.Space1DDescription[]
                                                   {
                                                       new PTZ.Space1DDescription()
                                                       {
                                                           URI = _relativePanTiltSpace,
                                                           XRange=new PTZ.FloatRange(){ Min=0, Max=10}
                                                       }
                                                   }
                    };
                    
                    PTZ.PTZNode node3 = new PTZ.PTZNode() { token = "ptz3" };
                    node3.MaximumNumberOfPresets = 5;

                    _nodes.Add(node1);
                    _nodes.Add(node2);
                    _nodes.Add(node3);

                }


                return _nodes;
            }
        }

        List<PTZ.PTZConfiguration> _configurations;

        public List<PTZ.PTZConfiguration> Configurations
        {
            get
            {
                if (_configurations == null)
                {
                    _configurations = new List<PTZ.PTZConfiguration>();

                    PTZ.PTZConfiguration configuration = new PTZ.PTZConfiguration();

                    //configuration.DefaultAbsolutePantTiltPositionSpace = "";
                    //configuration.DefaultAbsoluteZoomPositionSpace = "";
                    configuration.DefaultContinuousPanTiltVelocitySpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace";
                    configuration.DefaultContinuousZoomVelocitySpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace";
                    configuration.DefaultPTZTimeout = "PT10S";
                    configuration.DefaultPTZSpeed = new PTZ.PTZSpeed()
                    {
                        PanTilt = new PTZ.Vector2D() { space = "x", x = 10, y = 20 },
                        Zoom = new PTZ.Vector1D() { space = "y", x = 5 }
                    };
                    configuration.DefaultRelativePanTiltTranslationSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace";
                    configuration.DefaultRelativeZoomTranslationSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace";
                    configuration.Name = "Configuration";
                    configuration.token = "ptz-0";
                    configuration.NodeToken = "ptz0";
                    configuration.PanTiltLimits =
                        new PTZ.PanTiltLimits()
                        {
                            Range = new PTZ.Space2DDescription()
                            {
                                URI = "http://www.tempuri.org",
                                XRange = new PTZ.FloatRange() { Max = 10, Min = 1 },
                                YRange = new PTZ.FloatRange() { Max = 20, Min = 5 }
                            }
                        };


                    configuration.ZoomLimits = new PTZ.ZoomLimits()
                    {
                        Range = new PTZ.Space1DDescription()
                        {
                            URI = "http://www.tempuri.org",
                            XRange = new PTZ.FloatRange() { Max = 10, Min = 1 }
                        }
                    };

                    _configurations.Add(configuration);
                }

                return _configurations;
            }
        
        }

    
    }

}
