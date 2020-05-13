///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading;
using System.ServiceModel;
using TestTool.HttpTransport;
using TestTool.Tests.Common.SoapValidation;
using TestTool.Tests.Common.TestBase;
using TestTool.Proxies.Onvif;
using TestTool.Tests.TestCases.Utils.SoapValidation.SchemasSets;
using Device = TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Discovery;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestEngine;
using Media = TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.Media;
using TestTool.Tests.TestCases.Utils.SoapValidation;

namespace TestTool.Tests.TestCases.Base
{
    public class PTZTest : BaseServiceTest<PTZ, PTZClient>
    {
        //ONVIF 13.8.1.1 Generic Pan/Tilt Position Space
        protected const string _absolutePanTiltSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace";
        //ONVIF 13.8.1.2 Generic Zoom Position Space
        protected const string _absoluteZoomSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace";
        //ONVIF 13.8.3.1 Generic Pan/Tilt Velocity Space
        protected const string _continuousPanTiltSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace";
        //ONVIF 13.8.3.2 Generic Zoom Velocity Space
        protected const string _continuousZoomSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace";
        //ONVIF 13.8.3.1 Generic Pan/Tilt Velocity Space
        protected const string _relativePanTiltSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace";
        //ONVIF 13.8.3.2 Generic Zoom Velocity Space
        protected const string _relativeZoomSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace";
        //ONVIF 13.8.4.1 Generic Pan/Tilt Speed Space
        protected const string _speedPanTiltSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace";
        //ONVIF 13.8.4.2 Generic Zoom Speed Space
        protected const string _speedZoomSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace";

        private string _mediaAddress;

        public PTZTest(TestLaunchParam param)
            : base(param)
        {
        }

        /// <summary>
        /// Returns address of specified service
        /// </summary>
        /// <param name="category">Service category</param>
        /// <returns>Service address</returns>
        protected string GetServiceAddress(Device.CapabilityCategory category)
        {
            string address = string.Empty;
            RunStep(() =>
            {
                HttpTransport.HttpBinding binding =
                    (HttpTransport.HttpBinding)CreateBinding(true,
                    new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                Device.DeviceClient device = new Device.DeviceClient(binding, new EndpointAddress(_cameraAddress));

                AttachSecurity(device.Endpoint);
                SetupChannel(device.InnerChannel);

                Device.Capabilities capabilities = device.GetCapabilities(new Device.CapabilityCategory[] { category });

                if ((category == Device.CapabilityCategory.PTZ) && (capabilities.PTZ != null))
                {
                    address = capabilities.PTZ.XAddr;
                }
                else if ((category == Device.CapabilityCategory.Media) && (capabilities.Media != null))
                {
                    address = capabilities.Media.XAddr;
                }
                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException(category == Device.CapabilityCategory.Media ?
                        Resources.ErrorNoMediaAddress_Text :
                        Resources.ErrorNoPTZAddress_Text);
                }
            }, category == Device.CapabilityCategory.Media ? Resources.StepGetMediaAddress_Title : Resources.StepGetPTZAddress_Title);
            DoRequestDelay();
            return address;
        }
        /// <summary>
        /// Creates instance of media client
        /// </summary>
        /// <returns>Instance of media client</returns>
        protected override PTZClient CreateClient()
        {
            string address = GetServiceAddress(Device.CapabilityCategory.PTZ);

            BeginStep("Connect to PTZ service");
            LogStepEvent(string.Format("PTZ service address: {0}", address));
            if (!address.IsValidUrl())
            {
                throw new AssertException("PTZ service address is invalid");
            }
            Binding binding = CreateBinding(false,
                    new IChannelController[] { new SoapValidator(PtzSchemasSet.GetInstance()) });
            PTZClient client = new PTZClient(binding, new EndpointAddress(address));
            StepPassed();
            return client;
        }
        protected Media.Profile GetPTZProfile(PTZNode node, out PTZConfigurationOptions options)
        {
            return GetPTZProfile(node != null ? node.token : null, out options);
        }
        protected Media.Profile GetPTZProfile(string nodeToken, out PTZConfigurationOptions options)
        {
            Media.Profile res = null;
            options = null;
            _mediaAddress = GetServiceAddress(Device.CapabilityCategory.Media);
            if (!string.IsNullOrEmpty(_mediaAddress))
            {
                BeginStep("Connect to media service");
                LogStepEvent(string.Format("Media service address: {0}", _mediaAddress));
                if (!_mediaAddress.IsValidUrl())
                {
                    throw new AssertException("Media service address is invalid");
                }
                Binding binding = CreateBinding(
                    false, 
                    new IChannelController[]{new SoapValidator(MediaSchemasSet.GetInstance())});
                Media.MediaClient media = new Media.MediaClient(binding, new EndpointAddress(_mediaAddress));
                if ((Client == null) || (!EndpointAddress.Equals(Client.Endpoint, media.Endpoint)))
                {
                    AttachSecurity(media.Endpoint);
                }
                StepPassed();

                Media.Profile[] profiles = null;
                RunStep(() =>
                {
                    profiles = media.GetProfiles();
                    foreach (Media.Profile profile in profiles)
                    {
                        if ((profile.PTZConfiguration != null) &&
                            (string.IsNullOrEmpty(nodeToken) || (profile.PTZConfiguration.NodeToken == nodeToken)))
                        {
                            res = profile; break;
                        }
                    }
                },
                !string.IsNullOrEmpty(nodeToken) ?
                string.Format("Searching media profile with PTZ configuration for node [token = {0}]", nodeToken) :
                string.Format("Searching media profile with PTZ configuration"));

                DoRequestDelay();

                if ((res == null) && (profiles != null) && (profiles.Length > 0))
                {
                    //add ptz configuration to profile, if not found
                    PTZConfiguration[] configurations = GetConfigurations();
                    Assert((configurations != null) && (configurations.Length > 0), Resources.ErrorNoPTZConfig_Text, Resources.StepValidateGetConfigurations_Title);
                    RunStep(() =>
                    {
                        Media.Profile profile = profiles[0];
                        if (profile.PTZConfiguration != null)
                        {
                            //remove previous configuration
                            LogStepEvent(string.Format("Removing PTZ configuration from profile [token = {0}]", profile.token));
                            media.RemovePTZConfiguration(profile.token);
                        }
                        PTZConfiguration config = configurations.FirstOrDefault(c => string.IsNullOrEmpty(nodeToken) || (c.NodeToken == nodeToken));
                        if (config != null)
                        {
                            LogStepEvent(string.Format("Adding configuration [token = {0}] to profile [token = {1}]", config, profile.token));
                            media.AddPTZConfiguration(profile.token, configurations[0].token);
                            profile.PTZConfiguration = MediaTest.ToMediaPTZConfiguration(config);
                            res = profile;
                        }
                        else
                        {
                            throw new AssertException(string.Format("No PTZ configuration found for node [token = {0}]", nodeToken));
                        }
                    }, string.Format("Add PTZ Configuration"));
                    DoRequestDelay();
                }
                if((res != null)&&(res.PTZConfiguration != null))
                {
                    options = GetConfigurationOptions(res.PTZConfiguration.token);
                }
            }
            return res;
        }

        /// <summary>
        /// Gets PTZ nodes from DUT
        /// </summary>
        /// <returns>PTZ nodes</returns>
        protected PTZNode[] GetNodes()
        {
            PTZNode[] nodes = null;
            RunStep(() => { nodes = Client.GetNodes(); }, "Getting PTZ nodes");
            DoRequestDelay();
            return nodes;
        }

        /// <summary>
        /// Gets PTZ node from DUT
        /// </summary>
        /// <param name="token">Node token</param>
        /// <returns>PTZ node</returns>
        protected PTZNode GetNode(string token)
        {
            PTZNode node = null;
            RunStep(() => { node = Client.GetNode(token); }, string.Format("Getting PTZ node [token={0}]", token));
            DoRequestDelay();
            return node;
        }

        /// <summary>
        /// Tries to get PTZ node with invalid token, checks that fault is returned
        /// </summary>
        /// <param name="token">Invalid token</param>
        protected void GetInvalidNode(string token)
        {
            RunStep(() => { Client.GetNode(token); }, string.Format("Getting PTZ node [token={0}]", token), "Sender/InvalidArgVal/NoEntity", true);
            DoRequestDelay();
        }

        /// <summary>
        /// Tries to set PTZ configuration with invalid token, checks that fault is returned
        /// </summary>
        /// <param name="token">Invalid token</param>
        protected void SetInvalidConfiguration(string token)
        {
            PTZConfiguration configuration = new PTZConfiguration();
            configuration.token = token;
            configuration.Name = "TestConfig";
            configuration.NodeToken = "0";
            configuration.DefaultAbsolutePantTiltPositionSpace = "www.onvif.org";
            configuration.DefaultAbsoluteZoomPositionSpace = "www.onvif.org";
            configuration.DefaultRelativePanTiltTranslationSpace = "www.onvif.org";
            configuration.DefaultRelativeZoomTranslationSpace = "www.onvif.org";
            configuration.DefaultContinuousPanTiltVelocitySpace = "www.onvif.org";
            configuration.DefaultContinuousZoomVelocitySpace = "www.onvif.org";
            configuration.DefaultPTZTimeout = "PT10S";

            RunStep(() => { Client.SetConfiguration(configuration, false); },
                string.Format("Setting PTZ configuration [token={0}] - negative test", token), "Sender/InvalidArgVal/NoConfig", true);
       
            DoRequestDelay();
        }

        protected bool CheckPTZSpaces(object[] spaces, string genericSpace, Feature feature, out string reason)
        {
            reason = null;
            bool res = !Features.Contains(feature); //no need to check if not supported
            if (!res)
            {
                if (spaces != null)
                {
                    foreach (object space in spaces)
                    {
                        string uri = string.Empty;
                        if (space is Space2DDescription)
                        {
                            uri = (space as Space2DDescription).URI;
                        }
                        else if (space is Space1DDescription)
                        {
                            uri = (space as Space1DDescription).URI;
                        }
                        if (string.Compare(genericSpace, uri, true) == 0)
                        {
                            res = true;
                            break;
                        }
                    }
                }
            }
            if (!res)
            {
                reason = string.Format("Space [{0}] not found.", genericSpace);
            }
            return res;
        }
        protected bool ValidatePTZNode(PTZNode node, out string reason)
        {
            reason = null;

            if (string.IsNullOrEmpty(node.token))
            {
                reason = string.Format("PTZ node does not contain token");
                return false;
            }
            if (node.SupportedPTZSpaces == null)
            {
                reason = string.Format("PTZ node {0} does not contain SupportedPTZSpaces element", node.token);
                return false;
            }
            bool selectedNode = string.IsNullOrEmpty(_ptzNodeToken) || (_ptzNodeToken == node.token);

            if (!CheckPTZSpaces(node.SupportedPTZSpaces.AbsolutePanTiltPositionSpace, _absolutePanTiltSpace, Feature.PTZAbsolutePanTilt, out reason) && selectedNode ||
               !CheckPTZSpaces(node.SupportedPTZSpaces.AbsoluteZoomPositionSpace, _absoluteZoomSpace, Feature.PTZAbsoluteZoom, out reason) && selectedNode ||
               !CheckPTZSpaces(node.SupportedPTZSpaces.ContinuousPanTiltVelocitySpace, _continuousPanTiltSpace, Feature.PTZContiniousPanTilt, out reason) || //this is mandatory for all nodes
               !CheckPTZSpaces(node.SupportedPTZSpaces.ContinuousZoomVelocitySpace, _continuousZoomSpace, Feature.PTZContiniousZoom, out reason) && selectedNode ||
               !CheckPTZSpaces(node.SupportedPTZSpaces.RelativePanTiltTranslationSpace, _relativePanTiltSpace, Feature.PTZRelativePanTilt, out reason) && selectedNode ||
               !CheckPTZSpaces(node.SupportedPTZSpaces.RelativeZoomTranslationSpace, _relativeZoomSpace, Feature.PTZRelativeZoom, out reason) && selectedNode ||
               !CheckPTZSpaces(node.SupportedPTZSpaces.PanTiltSpeedSpace, _speedPanTiltSpace, Feature.PTZSpeedPanTilt, out reason) && selectedNode ||
               !CheckPTZSpaces(node.SupportedPTZSpaces.ZoomSpeedSpace, _speedZoomSpace, Feature.PTZSpeedZoom, out reason) && selectedNode)
            {
                reason = string.Format("PTZ node {0} does not contain mandatory spaces. {1}", node.token, reason);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets PTZ configurations from DUT
        /// </summary>
        /// <returns>PTZ configurations</returns>
        protected PTZConfiguration[] GetConfigurations()
        {
            PTZConfiguration[] configurations = null;
            RunStep(() => { configurations = Client.GetConfigurations(); }, "Get PTZ configurations");
            DoRequestDelay();
            return configurations;
        }

        /// <summary>
        /// Gets PTZ configuration from DUT
        /// </summary>
        /// <param name="token">Configuration token</param>
        /// <returns>PTZ configuration</returns>
        protected PTZConfiguration GetConfiguration(string token)
        {
            PTZConfiguration configuration = null;
            RunStep(() => { configuration = Client.GetConfiguration(token); }, "Get PTZ configuration");
            DoRequestDelay();
            return configuration;
        }

        /// <summary>
        /// Checks if PTZ configuration is valid
        /// </summary>
        /// <param name="configuration">Configuration to be validated</param>
        /// <param name="reason">Reason, why configuration is invalid</param>
        /// <returns>True if configuration is valid</returns>
        protected bool ValidatePTZConfiguration(PTZConfiguration configuration, out string reason)
        {
            reason = null;

            if (string.IsNullOrEmpty(configuration.NodeToken))
            {
                reason = string.Format("PTZ configuration {0} does not contain NodeToken element", configuration.token);
                return false;
            }

            if (string.IsNullOrEmpty(configuration.DefaultAbsolutePantTiltPositionSpace) &&
                string.IsNullOrEmpty(configuration.DefaultAbsoluteZoomPositionSpace) &&
                string.IsNullOrEmpty(configuration.DefaultRelativePanTiltTranslationSpace) &&
                string.IsNullOrEmpty(configuration.DefaultRelativeZoomTranslationSpace) &&
                string.IsNullOrEmpty(configuration.DefaultContinuousPanTiltVelocitySpace) &&
                string.IsNullOrEmpty(configuration.DefaultContinuousZoomVelocitySpace) &&
                (configuration.DefaultPTZSpeed == null) &&
                string.IsNullOrEmpty(configuration.DefaultPTZTimeout) &&
                (configuration.PanTiltLimits == null) &&
                (configuration.ZoomLimits == null))
            {
                reason = string.Format("PTZ configuration {0} does not contain parameters", configuration.token);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets configuration options for specified configuration
        /// </summary>
        /// <param name="token">Configuration token</param>
        /// <returns>Configuration options</returns>
        protected PTZConfigurationOptions GetConfigurationOptions(string token)
        {
            PTZConfigurationOptions options = null;
            RunStep(() => { options = Client.GetConfigurationOptions(token); }, "Getting PTZ configuration options");
            DoRequestDelay();
            return options;
        }

        /// <summary>
        /// Validates PTZ configuration options
        /// </summary>
        /// <param name="options">Options to be validated</param>
        /// <param name="reason">Reason why options are invalif</param>
        /// <returns>True, if options are valid</returns>
        protected bool ValidatePTZConfigurationOptions(PTZConfigurationOptions options, string nodeToken, out string reason)
        {
            reason = null;
            if (options == null)
            {
                reason = string.Format("PTZ configuration options are empty");
                return false;
            }
            if (options.Spaces == null)
            {
                reason = string.Format("PTZ configuration options do not contain Spaces element");
                return false;
            }
            bool selectedNode = string.IsNullOrEmpty(_ptzNodeToken) || (_ptzNodeToken == nodeToken);
            if (!CheckPTZSpaces(options.Spaces.AbsolutePanTiltPositionSpace, _absolutePanTiltSpace, Feature.PTZAbsolutePanTilt, out reason) && selectedNode ||
               !CheckPTZSpaces(options.Spaces.AbsoluteZoomPositionSpace, _absoluteZoomSpace, Feature.PTZAbsoluteZoom, out reason) && selectedNode ||
               !CheckPTZSpaces(options.Spaces.ContinuousPanTiltVelocitySpace, _continuousPanTiltSpace, Feature.PTZContiniousPanTilt, out reason) ||
               !CheckPTZSpaces(options.Spaces.ContinuousZoomVelocitySpace, _continuousZoomSpace, Feature.PTZContiniousZoom, out reason) && selectedNode ||
               !CheckPTZSpaces(options.Spaces.RelativePanTiltTranslationSpace, _relativePanTiltSpace, Feature.PTZRelativePanTilt, out reason) && selectedNode ||
               !CheckPTZSpaces(options.Spaces.RelativeZoomTranslationSpace, _relativeZoomSpace, Feature.PTZRelativeZoom, out reason) && selectedNode ||
               !CheckPTZSpaces(options.Spaces.PanTiltSpeedSpace, _speedPanTiltSpace, Feature.PTZSpeedPanTilt, out reason) && selectedNode ||
               !CheckPTZSpaces(options.Spaces.ZoomSpeedSpace, _speedZoomSpace, Feature.PTZSpeedZoom, out reason) && selectedNode)
            {
                reason = string.Format("PTZ configuration options do not contain mandatory spaces. {0}", reason);
                return false;
            }
            //check PTZ timeout
            if (options.PTZTimeout == null)
            {
                reason = "PTZ configuration options do not contain timeout";
                return false;
            }
            bool minParsed = false;
            try
            {
                TimeSpan min = System.Xml.XmlConvert.ToTimeSpan(options.PTZTimeout.Min);
                minParsed = true;
                TimeSpan max = System.Xml.XmlConvert.ToTimeSpan(options.PTZTimeout.Max);
                if (max < min)
                {
                    reason = string.Format("PTZ configuration options contains invalid timeout range. Max value ({0}) is less than min value ({1})",
                        options.PTZTimeout.Max,
                        options.PTZTimeout.Min);
                    return false;
                }

            }
            catch
            {
                //incorrect timeout format;
                reason = string.Format("PTZ configuration options timeout value {0} is incorrect. The value must be compliant with xml schema duration format.",
                    minParsed ? options.PTZTimeout.Max : options.PTZTimeout.Min);
                return false;
            }
            return true;
        }

        /*
        /// <summary>
        /// Converts Media.PTZConfiguration class to PTZ.PTZConfiguration class
        /// </summary>
        /// <param name="config">Object to be converted</param>
        /// <returns>Converted object</returns>
        protected PTZConfiguration FromMediaPTZConfiguration(Media.PTZConfiguration config)
        {
            PTZConfiguration newConfig = new PTZConfiguration();
            newConfig.AnyAttr = config.AnyAttr;
            newConfig.DefaultAbsolutePantTiltPositionSpace = config.DefaultAbsolutePantTiltPositionSpace;
            newConfig.DefaultAbsoluteZoomPositionSpace = config.DefaultAbsoluteZoomPositionSpace;
            newConfig.DefaultContinuousPanTiltVelocitySpace = config.DefaultContinuousPanTiltVelocitySpace;
            newConfig.DefaultContinuousZoomVelocitySpace = config.DefaultContinuousZoomVelocitySpace;
            if (config.DefaultPTZSpeed != null)
            {
                newConfig.DefaultPTZSpeed = new PTZSpeed();
                if (config.DefaultPTZSpeed.PanTilt != null)
                {
                    newConfig.DefaultPTZSpeed.PanTilt = new Vector2D();
                    newConfig.DefaultPTZSpeed.PanTilt.x = config.DefaultPTZSpeed.PanTilt.x;
                    newConfig.DefaultPTZSpeed.PanTilt.y = config.DefaultPTZSpeed.PanTilt.y;
                }
                if (config.DefaultPTZSpeed.Zoom != null)
                {
                    newConfig.DefaultPTZSpeed.Zoom = new Vector1D();
                    newConfig.DefaultPTZSpeed.Zoom.x = config.DefaultPTZSpeed.Zoom.x;
                }
            }
            newConfig.DefaultPTZTimeout = config.DefaultPTZTimeout;
            newConfig.DefaultRelativePanTiltTranslationSpace = config.DefaultRelativePanTiltTranslationSpace;
            newConfig.DefaultRelativeZoomTranslationSpace = config.DefaultRelativeZoomTranslationSpace;
            if (config.Extension != null)
            {
                newConfig.Extension = new PTZConfigurationExtension();
                newConfig.Extension.Any = config.Extension.Any;
            }
            newConfig.NodeToken = config.NodeToken;
            if (config.PanTiltLimits != null)
            {
                newConfig.PanTiltLimits = new PanTiltLimits();
                if (config.PanTiltLimits.Range != null)
                {
                    newConfig.PanTiltLimits.Range = new Space2DDescription();
                    newConfig.PanTiltLimits.Range.URI = config.PanTiltLimits.Range.URI;
                    if (config.PanTiltLimits.Range.XRange != null)
                    {
                        newConfig.PanTiltLimits.Range.XRange = new FloatRange();
                        newConfig.PanTiltLimits.Range.XRange.Max = config.PanTiltLimits.Range.XRange.Max;
                        newConfig.PanTiltLimits.Range.XRange.Min = config.PanTiltLimits.Range.XRange.Min;
                    }
                    if (config.PanTiltLimits.Range.YRange != null)
                    {
                        newConfig.PanTiltLimits.Range.YRange = new FloatRange();
                        newConfig.PanTiltLimits.Range.YRange.Max = config.PanTiltLimits.Range.YRange.Max;
                        newConfig.PanTiltLimits.Range.YRange.Min = config.PanTiltLimits.Range.YRange.Min;
                    }
                }
            }
            newConfig.token = config.token;
            newConfig.UseCount = config.UseCount;
            newConfig.Name = config.Name;
            if (config.ZoomLimits != null)
            {
                newConfig.ZoomLimits = new ZoomLimits();
                if (config.ZoomLimits.Range != null)
                {
                    newConfig.ZoomLimits.Range = new Space1DDescription();
                    newConfig.ZoomLimits.Range.URI = config.ZoomLimits.Range.URI;
                    if (config.ZoomLimits.Range.XRange != null)
                    {
                        newConfig.ZoomLimits.Range.XRange = new FloatRange();
                        newConfig.ZoomLimits.Range.XRange.Max = config.ZoomLimits.Range.XRange.Max;
                        newConfig.ZoomLimits.Range.XRange.Min = config.ZoomLimits.Range.XRange.Min;
                    }
                }
            }
            return newConfig;
        }
        */

        /// <summary>
        /// Sets PTZ configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="forcePersistence">If true, configuration persists after device reboot</param>
        protected void SetConfiguration(PTZConfiguration config, bool forcePersistence, string details)
        {
            RunStep(() => { Client.SetConfiguration(config, forcePersistence); }, string.Format("Setting PTZ configuration [token = {0}]. {1}", config.token, details));
            DoRequestDelay();
        }

        /*
        /// <summary>
        /// Sets PTZ configuration
        /// </summary>
        /// <param name="config">Configuration to be set</param>
        /// <param name="forcePersistence">If true, configuration persists after device reboot</param>
        protected void SetConfiguration(Media.PTZConfiguration config, bool forcePersistence, string details)
        {
            RunStep(() => { Client.SetConfiguration(FromMediaPTZConfiguration(config), forcePersistence); }, string.Format("Setting PTZ configuration [token = {0}]. {1}", config.token, details));
            DoRequestDelay();
        }
         */

        /// <summary>
        /// Returns timeout value, different from current, but inside valid range
        /// </summary>
        /// <param name="current">Current value</param>
        /// <param name="options">Configuration options</param>
        /// <returns>New timeout value</returns>
        protected string GetModifiedTimeout(string current, PTZConfigurationOptions options)
        {
            TimeSpan spanCurrent = !string.IsNullOrEmpty(current) ? System.Xml.XmlConvert.ToTimeSpan(current) : TimeSpan.FromSeconds(0);
            TimeSpan min = System.Xml.XmlConvert.ToTimeSpan(options.PTZTimeout.Min);
            TimeSpan max = System.Xml.XmlConvert.ToTimeSpan(options.PTZTimeout.Max);

            return (spanCurrent == max) ? options.PTZTimeout.Min : options.PTZTimeout.Max;
        }
        #region compare functions
        protected bool EqualVector1D(Vector1D vecA, Vector1D vecB)
        {
            if ((vecA == null) && (vecB == null))
            {
                return true;
            }
            if ((vecA == null) || (vecB == null))
            {
                return false;
            }
            return (string.Compare(vecA.space, vecB.space, true) == 0) && (vecA.x == vecB.x);
        }
        protected bool EqualVector2D(Vector2D vecA, Vector2D vecB)
        {
            if ((vecA == null) && (vecB == null))
            {
                return true;
            }
            if ((vecA == null) || (vecB == null))
            {
                return false;
            }
            return (string.Compare(vecA.space, vecB.space, true) == 0) && (vecA.x == vecB.x) && (vecA.y == vecB.y);
        }
        protected bool EqualPTZSpeed(PTZSpeed speedA, PTZSpeed speedB)
        {
            if ((speedA == null) && (speedB == null))
            {
                return true;
            }
            if ((speedA == null) || (speedB == null))
            {
                return false;
            }
            return EqualVector2D(speedA.PanTilt, speedB.PanTilt) && EqualVector1D(speedA.Zoom, speedB.Zoom);
        }
        protected bool EqualFloatRange(FloatRange rangeA, FloatRange rangeB)
        {
            if ((rangeA == null) && (rangeB == null))
            {
                return true;
            }
            if ((rangeA == null) || (rangeB == null))
            {
                return false;
            }
            return (rangeA.Max == rangeB.Max) && (rangeA.Min == rangeB.Min);
        }
        protected bool EqualSpace2DDescription(Space2DDescription spaceA, Space2DDescription spaceB)
        {
            if ((spaceA == null) && (spaceB == null))
            {
                return true;
            }
            if ((spaceA == null) || (spaceB == null))
            {
                return false;
            }
            return (string.Compare(spaceA.URI, spaceB.URI, true) == 0) &&
                EqualFloatRange(spaceA.XRange, spaceB.XRange) &&
                EqualFloatRange(spaceA.YRange, spaceB.YRange);
        }
        protected bool EqualPanTiltLimits(PanTiltLimits limitsA, PanTiltLimits limitsB)
        {
            if ((limitsA == null) && (limitsB == null))
            {
                return true;
            }
            if ((limitsA == null) || (limitsB == null))
            {
                return false;
            }
            return EqualSpace2DDescription(limitsA.Range, limitsB.Range);
        }
        protected bool EqualSpace1DDescription(Space1DDescription spaceA, Space1DDescription spaceB)
        {
            if ((spaceA == null) && (spaceB == null))
            {
                return true;
            }
            if ((spaceA == null) || (spaceB == null))
            {
                return false;
            }
            return (string.Compare(spaceA.URI, spaceB.URI, true) == 0) &&
                EqualFloatRange(spaceA.XRange, spaceB.XRange);
        }
        protected bool EqualZoomLimits(ZoomLimits zoomA, ZoomLimits zoomB)
        {
            if ((zoomA == null) && (zoomB == null))
            {
                return true;
            }
            if ((zoomA == null) || (zoomB == null))
            {
                return false;
            }
            return EqualSpace1DDescription(zoomA.Range, zoomB.Range);
        }

        /// <summary>
        /// Checks if PTZ configurations are equal
        /// </summary>
        /// <param name="configA">Configuration to be compared</param>
        /// <param name="configB">Configuration to be compared to</param>
        /// <param name="reason">Reason why configurations are not equal</param>
        /// <returns>True, if configurations are equal</returns>
        protected bool EqualConfigs(PTZConfiguration configA, PTZConfiguration configB, out string reason)
        {
            reason = null;
            if ((configA == null) && (configB == null))
            {
                return true;
            }
            if ((configA == null) || (configB == null))
            {
                reason = Resources.ErrorOneConfigIsEmpty_Text;
                return false;
            }
            if (string.Compare(configA.token, configB.token, true) != 0)
            {
                reason = "Configurations have different tokens";
                return false;
            }
            if (string.Compare(configA.NodeToken, configB.NodeToken, true) != 0)
            {
                reason = "Configurations have different node tokens";
                return false;
            }
            if (string.Compare(configA.DefaultAbsolutePantTiltPositionSpace, configB.DefaultAbsolutePantTiltPositionSpace, true) != 0)
            {
                reason = "Configurations have different default absolute pant/tilt position spaces";
                return false;
            }
            if (string.Compare(configA.DefaultAbsoluteZoomPositionSpace, configB.DefaultAbsoluteZoomPositionSpace, true) != 0)
            {
                reason = "Configurations have different default absolute zoom position spaces";
                return false;
            }
            if (string.Compare(configA.DefaultRelativePanTiltTranslationSpace, configB.DefaultRelativePanTiltTranslationSpace, true) != 0)
            {
                reason = "Configurations have different default relative pant/tilt translation spaces";
                return false;
            }
            if (string.Compare(configA.DefaultRelativeZoomTranslationSpace, configB.DefaultRelativeZoomTranslationSpace, true) != 0)
            {
                reason = "Configurations have different default relative zoom translation spaces";
                return false;
            }
            if (string.Compare(configA.DefaultContinuousPanTiltVelocitySpace, configB.DefaultContinuousPanTiltVelocitySpace, true) != 0)
            {
                reason = "Configurations have different default continuous pant/tilt velocity spaces";
                return false;
            }
            if (string.Compare(configA.DefaultContinuousZoomVelocitySpace, configB.DefaultContinuousZoomVelocitySpace, true) != 0)
            {
                reason = "Configurations have different default continuous zoom velocity spaces";
                return false;
            }
            if (string.Compare(configA.DefaultPTZTimeout, configB.DefaultPTZTimeout, true) != 0)
            {
                if (!string.IsNullOrEmpty(configA.DefaultPTZTimeout) && !string.IsNullOrEmpty(configB.DefaultPTZTimeout))
                {
                    double timeout1 = configA.DefaultPTZTimeout.DurationToSeconds();
                    double timeout2 = configB.DefaultPTZTimeout.DurationToSeconds();

                    if (!double.IsNaN(timeout1) && !double.IsNaN(timeout2))
                    {
                        if (timeout1 != timeout2)
                        {
                            reason = "Configurations have different PTZ timeouts";
                            return false;
                        }
                    }
                }
                else
                {
                    if (! (string.IsNullOrEmpty(configA.DefaultPTZTimeout) && string.IsNullOrEmpty(configB.DefaultPTZTimeout) ))
                    {
                        reason = "Configurations have different PTZ timeouts";
                        return false;
                    }
                }
            }
            if (!EqualPTZSpeed(configA.DefaultPTZSpeed, configB.DefaultPTZSpeed))
            {
                reason = "Configurations have different PTZ speeds";
                return false;
            }
            if (!EqualPanTiltLimits(configA.PanTiltLimits, configB.PanTiltLimits))
            {
                reason = "Configurations have different pan/tilt limits";
                return false;
            }
            if (!EqualZoomLimits(configA.ZoomLimits, configB.ZoomLimits))
            {
                reason = "Configurations have different zoom limits";
                return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Starts camera continuous move
        /// </summary>
        /// <param name="profile">Profile token</param>
        /// <param name="velocity">Velocity</param>
        /// <param name="timeout">Move timeout</param>
        protected void ContinuousMove(string profile, PTZSpeed velocity, string timeout)
        {
            RunStep(() => { Client.ContinuousMove(profile, velocity, timeout); }, "Continuous move start");
            DoRequestDelay();
        }

        /// <summary>
        /// Gets PTZ status of specified profile
        /// </summary>
        /// <param name="profile">Profile token</param>
        /// <returns>PTZ status</returns>
        protected PTZStatus GetPTZStatus(string profile)
        {
            PTZStatus status = null;
            RunStep(() => { status = Client.GetStatus(profile); }, "Getting PTZ status");
            DoRequestDelay();
            return status;
        }

        /// <summary>
        /// Checks if PTZ status is one of expected
        /// </summary>
        /// <param name="status">Status</param>
        /// <param name="expected">Expected status list</param>
        /// <returns>true if status is expected</returns>
        protected bool ValidatePanTiltStatus(PTZStatus status, MoveStatus[] expectedPanTilt, MoveStatus[] expectedZoom)
        {
            bool res = true;//if status is not available - do not check
            if ((status.MoveStatus != null))
            {
                if ((status.MoveStatus.PanTiltSpecified) && (expectedPanTilt != null))
                {
                    res = expectedPanTilt.Contains(status.MoveStatus.PanTilt);
                }
                if ((status.MoveStatus.ZoomSpecified) && (expectedZoom != null))
                {
                    res = res && expectedZoom.Contains(status.MoveStatus.Zoom);
                }
            }
            return res;
        }

        /// <summary>
        /// Stops PTZ movement
        /// </summary>
        /// <param name="profile">Profile token</param>
        /// <param name="stopPanTilt">If true, stops pan/tilt movement</param>
        /// <param name="stopZoom">If true, stops zoom movement</param>
        protected void Stop(string profile, bool stopPanTilt, bool stopZoom)
        {
            RunStep(() => { Client.Stop(profile, stopPanTilt, stopZoom); }, "Stop PTZ movement");
            DoRequestDelay();
        }
        /// <summary>
        /// Move camera to specified position
        /// </summary>
        /// <param name="profile">Profile token</param>
        /// <param name="vector">Position</param>
        /// <param name="speed">Speed</param>
        protected void AbsoluteMove(string profile, PTZVector vector, PTZSpeed speed)
        {
            string stepName = "Moving";
            if (vector.PanTilt != null)
            {
                stepName += string.Format(" pan/tilt to ({0}, {1}) space={2}", vector.PanTilt.x, vector.PanTilt.y, vector.PanTilt.space);
            }
            if (vector.Zoom != null)
            {
                stepName += string.Format(" zoom to ({0}) space={1}", vector.Zoom.x, vector.Zoom.space);
            }
            RunStep(() => { Client.AbsoluteMove(profile, vector, speed); }, stepName);
            DoRequestDelay();
        }

        /// <summary>
        /// Tries to move camera to invalid position, checks that fault is returned
        /// </summary>
        /// <param name="token">Profile token</param>
        /// <param name="position">Invalid position</param>
        protected void AbsoluteInvalidMove(string token, PTZVector position)
        {
            string stepName = "Moving";
            if (position.PanTilt != null)
            {
                stepName += string.Format(" pan/tilt to ({0}, {1}) space={2}", position.PanTilt.x, position.PanTilt.y, position.PanTilt.space);
            }
            if (position.Zoom != null)
            {
                stepName += string.Format(" zoom to ({0}) space={1}", position.Zoom.x, position.Zoom.space);
            }
            RunStep(() => { Client.AbsoluteMove(token, position, null); }, stepName, "Sender/InvalidArgVal/InvalidPosition", true);
            DoRequestDelay();
        }

        /// <summary>
        /// Move camera to specified position
        /// </summary>
        /// <param name="profile">Profile token</param>
        /// <param name="vector">Translation</param>
        /// <param name="speed">Speed</param>
        protected void RelativeMove(string profile, PTZVector vector, PTZSpeed speed)
        {
            string stepName = "Moving relative";
            if (vector.PanTilt != null)
            {
                stepName += string.Format(" pan/tilt to ({0}, {1}) space={2}", vector.PanTilt.x, vector.PanTilt.y, vector.PanTilt.space);
            }
            if (vector.Zoom != null)
            {
                stepName += string.Format(" zoom to ({0}) space={1}", vector.Zoom.x, vector.Zoom.space);
            }
            RunStep(() => { Client.RelativeMove(profile, vector, speed); }, stepName);
            DoRequestDelay();
        }

        /// <summary>
        /// Performs test steps to check PTZ continuous movement
        /// </summary>
        /// <param name="profile">Profile token</param>
        /// <param name="velocity">Movement velocity</param>
        /// <param name="timeout">Movement timeout</param>
        /// <param name="options">Configuration options</param>
        protected void CheckContinuousMove(string profile, PTZSpeed velocity, string timeout, PTZConfigurationOptions options)
        {
            ContinuousMove(profile, velocity, timeout);

            //CR54-55
            /* PTZStatus status = GetPTZStatus(profile);
            MoveStatus[] expected = new MoveStatus[] { MoveStatus.MOVING, MoveStatus.UNKNOWN };
            Assert(ValidatePanTiltStatus(status, velocity.PanTilt != null ? expected : null, velocity.Zoom != null ? expected : null),
                "Returned PTZ status is not MOVING or UNKNOWN",
               Resources.StepValidatePTZStatus);*/

            if (!string.IsNullOrEmpty(timeout))
            {
                TimeSpan spanTimeout = System.Xml.XmlConvert.ToTimeSpan(timeout);
                RunStep(() => { Thread.Sleep(spanTimeout); }, string.Format("Waiting {0} seconds for move to complete", spanTimeout.TotalSeconds));
            }
            else
            {
                RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));
                Stop(profile, velocity.PanTilt != null, velocity.Zoom != null);
            }
            RunStep(() => { Thread.Sleep(_operationDelay); }, string.Format("Waiting {0} seconds for camera to stop", _operationDelay / 1000));
            DoRequestDelay();

            PTZStatus status = GetPTZStatus(profile);
            MoveStatus[] expected = new MoveStatus[] { MoveStatus.IDLE, MoveStatus.UNKNOWN };
            Assert(ValidatePanTiltStatus(status, velocity.PanTilt != null ? expected : null, velocity.Zoom != null ? expected : null),
                "Returned PTZ status is not IDLE or UNKNOWN",
                Resources.StepValidatePTZStatus);
        }
        protected bool EqualSpaces(string space1, string space2)
        {
            return DiscoveryUtils.CompareUri(space1, space2);
        }
        /// <summary>
        /// Check if current position is as expected
        /// </summary>
        /// <param name="position">Current position</param>
        /// <param name="pantilt">Expected pan/tilt position</param>
        /// <param name="zoom">Expected zoom position</param>
        /// <returns>true if position is expected</returns>
        protected void CheckPTZPosition(PTZVector position, PTZVector pantilt, PTZVector zoom)
        {
            RunStep(() =>
            {
                if ((position.PanTilt != null) && (pantilt != null) && (pantilt.PanTilt != null))
                {
                    LogStepEvent(string.Format("Current pan/tilt position ({0},{1}) space={2}", position.PanTilt.x, position.PanTilt.y, position.PanTilt.space));

                    // dont' compare spaces
                    //if (!string.IsNullOrEmpty(position.PanTilt.space) && !string.IsNullOrEmpty(pantilt.PanTilt.space))
                    //{
                    //    if (!EqualSpaces(position.PanTilt.space, pantilt.PanTilt.space))
                    //    {
                    //        throw new AssertException(
                    //            string.Format("Pan/tilt position space is incorrect. Should be {0}",
                    //                          pantilt.PanTilt.space));
                    //    }
                    //}
                    //else
                    //{
                    //    if (!(string.IsNullOrEmpty(position.PanTilt.space) && string.IsNullOrEmpty(pantilt.PanTilt.space)))
                    //    {
                    //        throw new AssertException(
                    //            string.Format("Pan/tilt position space is incorrect. Should be {0}",
                    //                          pantilt.PanTilt.space));
                    //    }
                    //}
                    if ((position.PanTilt.x != pantilt.PanTilt.x) ||
                        (position.PanTilt.y != pantilt.PanTilt.y))
                    {
                        LogStepEvent(string.Format("Warning: Actual pan/tilt position differs from expected ({0},{1})", pantilt.PanTilt.x, pantilt.PanTilt.y));
                    }
                }
                if ((position.Zoom != null) && (zoom != null) && (zoom.Zoom != null))
                {
                    LogStepEvent(string.Format("Current zoom position ({0}) space={1}", position.Zoom.x, position.Zoom.space));
                    
                    // don't compare spaces
                    //if ( ! (string.IsNullOrEmpty(position.Zoom.space)) && !(string.IsNullOrEmpty(zoom.Zoom.space)))
                    //{
                    //    if (!EqualSpaces(position.Zoom.space, zoom.Zoom.space))
                    //    {
                    //        throw new AssertException(string.Format("Zoom position space is incorrect. Should be {0}", zoom.Zoom.space));
                    //    }
                    //}
                    //else
                    //{
                    //    if (!( (string.IsNullOrEmpty(position.Zoom.space)) && (string.IsNullOrEmpty(zoom.Zoom.space))))
                    //    {
                    //        throw new AssertException(string.Format("Zoom position space is incorrect. Should be {0}", zoom.Zoom.space));
                    //    }
                    //}
                    if (position.Zoom.x != zoom.Zoom.x)
                    {
                        LogStepEvent(string.Format("Warning: Actual zoom position differs from expected ({0})", zoom.Zoom.x));
                    }
                }
            }, "Checking current pan/tilt and zoom position");
        }

        /// <summary>
        /// Sets new preset
        /// </summary>
        /// <param name="profile">Profile token</param>
        /// <param name="name">Preset name</param>
        /// <returns>Preset token</returns>
        protected string SetPreset(string profile, string name, string token)
        {
            RunStep(() =>
            {
                Client.SetPreset(profile, name, ref token);
                if (string.IsNullOrEmpty(token))
                {
                    throw new AssertException("Empty preset token returned");
                }
            }, string.Format("Setting preset {0} for profile [token={1}]",
            !string.IsNullOrEmpty(name) ? string.Format("[name={0}]", name) : string.Format("[token={0}]", token), profile));
            DoRequestDelay();

            return token;
        }

        /// <summary>
        /// Returns PTZ presets for specified profile
        /// </summary>
        /// <param name="profile">Profile token</param>
        /// <returns>Profiles</returns>
        protected PTZPreset[] GetPresets(string profile)
        {
            PTZPreset[] presets = null;
            RunStep(() => { presets = Client.GetPresets(profile); }, string.Format("Getting presets for profile [token={0}]", profile));
            DoRequestDelay();
            return presets;
        }

        /// <summary>
        /// Removes specified preset
        /// </summary>
        /// <param name="profile">Profile token</param>
        /// <param name="preset">Preset token</param>
        protected void RemovePreset(string profile, string preset)
        {
            RunStep(() => { Client.RemovePreset(profile, preset); }, string.Format("Removing preset [token={0}] from profile [token={1}]", preset, profile));
            DoRequestDelay();
        }

        /// <summary>
        /// Compares 1d vectors
        /// </summary>
        /// <param name="a">Vector to be compared</param>
        /// <param name="b">Vector to be compared</param>
        /// <returns>True, if vectors are equal</returns>
        protected bool EqualVectors(Vector1D a, Vector1D b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            return (a.x == b.x) && DiscoveryUtils.CompareUri(a.space, b.space);
        }

        /// <summary>
        /// Compares 2d vectors
        /// </summary>
        /// <param name="a">Vector to be compared</param>
        /// <param name="b">Vector to be compared</param>
        /// <returns>True, if vectors are equal</returns>
        protected bool EqualVectors(Vector2D a, Vector2D b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            return (a.x == b.x) && (a.y == b.y) && DiscoveryUtils.CompareUri(a.space, b.space);
        }

        /// <summary>
        /// Compares PTZ vectors
        /// </summary>
        /// <param name="a">Vector to be compared</param>
        /// <param name="b">Vector to be compared</param>
        /// <returns>True, if vectors are equal</returns>
        protected bool EqualPositions(PTZVector a, PTZVector b)
        {
            if (a == b)
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            return EqualVectors(a.PanTilt, b.PanTilt) && EqualVectors(a.Zoom, b.Zoom);
        }

        /// <summary>
        /// Converts PTZ position to string
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>String representing position</returns>
        protected string PositionToString(PTZVector position)
        {
            string res = string.Empty;
            if (position != null)
            {
                if (position.PanTilt != null)
                {
                    res += string.Format("pan/tilt=({0},{1}) space={2}", position.PanTilt.x, position.PanTilt.y, position.PanTilt.space);
                }
                if (position.Zoom != null)
                {
                    if (!string.IsNullOrEmpty(res))
                    {
                        res += " ";
                    }
                    res += string.Format("zoom=({0}) space={1}", position.Zoom.x, position.Zoom.space);
                }
            }
            return res;
        }

        /// <summary>
        /// Checks that specified preset exists and contains valid params
        /// </summary>
        /// <param name="presets">List of presets</param>
        /// <param name="presetToken">Preset token</param>
        /// <param name="name">Preset name</param>
        /// <param name="vector">Preset position</param>
        protected void CheckPreset(PTZPreset[] presets, string presetToken, string name, PTZVector position)
        {
            PTZPreset preset = null;
            RunStep(() =>
            {
                if ((presets == null) || (presets.Length == 0))
                {
                    throw new AssertException("No presets return from NVT");
                }
                preset = presets.FirstOrDefault(p => p.token == presetToken);
                if (preset == null)
                {
                    throw new AssertException(string.Format("Preset [token={0}] not found"));
                }
                if (preset.Name != name)
                {
                    throw new AssertException(
                        string.Format("Preset [token={0}] has wrong name. Expected [{1}] - actual [{2}]",
                        presetToken, name, preset.Name));

                }
            }, string.Format("Searching for preset [token={0}]", presetToken));

            if ((preset.PTZPosition != null) && (position != null))
            {
                //convert generic relative spaces to absolute, because position in preset should be in absolute space
                if ((preset.PTZPosition.PanTilt != null) && (preset.PTZPosition.PanTilt.space == _relativePanTiltSpace))
                {
                    preset.PTZPosition.PanTilt.space = _absolutePanTiltSpace;
                }
                if ((preset.PTZPosition.Zoom != null) && (preset.PTZPosition.Zoom.space == _relativeZoomSpace))
                {
                    preset.PTZPosition.Zoom.space = _absoluteZoomSpace;
                }

                CheckPTZPosition(preset.PTZPosition, position, position);
            }
        }

        /// <summary>
        /// Checks that specified preset does not exist
        /// </summary>
        /// <param name="presets">List of presets</param>
        /// <param name="presetToken">Preset token</param>
        /// <param name="name">Preset name</param>
        /// <param name="vector">Preset position</param>
        protected void CheckNoPreset(PTZPreset[] presets, string presetToken)
        {
            RunStep(() =>
            {
                if ((presets != null) && (presets.Length > 0))
                {
                    PTZPreset preset = presets.FirstOrDefault(p => p.token == presetToken);
                    if (preset != null)
                    {
                        throw new AssertException(string.Format("Preset [token={0}] found", presetToken));
                    }
                }
            }, string.Format("Searching for preset [token={0}]", presetToken));
        }

        /// <summary>
        /// Move camera to specified preset
        /// </summary>
        /// <param name="profile">Profile token</param>
        /// <param name="preset">Preset token</param>
        /// <param name="speed">Speed</param>
        protected void GotoPreset(string profile, string preset, PTZSpeed speed)
        {
            RunStep(() => { Client.GotoPreset(profile, preset, speed); },
                string.Format("Going to preset [token={0}] in profile [token={1}]", preset, profile));
            DoRequestDelay();
        }

        /// <summary>
        /// Sets home position for specified profile
        /// </summary>
        /// <param name="profile">Profile token</param>
        protected void SetHomePosition(string profile)
        {
            RunStep(() => { Client.SetHomePosition(profile); },
               string.Format("Setting home position for profile [token={0}]", profile));

            DoRequestDelay();
        }

        /// <summary>
        /// Moves camera to home position for specified profile
        /// </summary>
        /// <param name="profile">Profile token</param>
        protected void GotoHomePosition(string profile, PTZSpeed speed, int delay)
        {
            RunStep(() =>
            {
                Client.GotoHomePosition(profile, speed);
                if (delay > 0)
                {
                    LogStepEvent(string.Format("Waiting for {0} seconds for camera to come to home position.", delay));
                    Thread.Sleep(TimeSpan.FromSeconds(delay));
                }
            },
            string.Format("Going to home position for profile [token={0}]", profile));

            DoRequestDelay();
        }

        /// <summary>
        /// Tries to set fixed home position of specified profile and check that fault returned
        /// </summary>
        /// <param name="profile">Profile token</param>
        protected void SetFixedHomePosition(string profile)
        {
            RunStep(() => { Client.SetHomePosition(profile); },
               string.Format("Setting home position for profile [token={0}] - negative test", profile),
               "Receiver/Action/CannotOverwriteHome",
               true);

            DoRequestDelay();
        }

        protected string SendAuxiliaryCommnad(string profile, string command)
        {
            string response = null;
            RunStep(() =>
            {
                response = Client.SendAuxiliaryCommand(profile, command);
                LogStepEvent(string.Format("Response: [{0}]", response));
            },
                string.Format("Sending command [0] for profile [token={1}]", command, profile)
            );

            DoRequestDelay();
            return response;
        }

        protected void OpenVideo()
        {
            RunStep(() =>
            {
                try
                {
                    Binding binding = CreateBinding(
                        false,
                        new IChannelController[] { new SoapValidator(MediaSchemasSet.GetInstance()) });

                    Media.MediaClient media = new Media.MediaClient(binding, new EndpointAddress(_mediaAddress));
                    if ((Client == null) || (!EndpointAddress.Equals(Client.Endpoint, media.Endpoint)))
                    {
                        AttachSecurity(media.Endpoint);
                    }

                    List<string> tokens = new List<string>();
                    LogStepEvent("Getting video sources");
                    Media.VideoSource[] sources = media.GetVideoSources();
                    if (sources != null)
                    {
                        foreach (Media.VideoSource source in sources)
                        {
                            tokens.Add(source.token);
                        }
                    }
                    string token = _operator.GetVideoConfigurationToken(tokens);

                    Media.Profile profile = null;
                    LogStepEvent("Getting media profiles");
                    Media.Profile[] profiles = media.GetProfiles();
                    if (string.IsNullOrEmpty(token))
                    {
                        profile = profiles.FirstOrDefault((p) =>
                            (p.VideoSourceConfiguration != null) &&
                            (p.VideoEncoderConfiguration != null));
                    }
                    else
                    {
                        profile = profiles.FirstOrDefault((p) =>
                            (p.VideoSourceConfiguration != null) &&
                            (p.VideoEncoderConfiguration != null) &&
                            (p.VideoSourceConfiguration.SourceToken == token));
                    }
                    if (profile == null)
                    {
                        throw new AssertException(string.IsNullOrEmpty(token) ?
                            "No media profile with video source configuration and video encoder configuration" :
                            string.Format("No media profile with video source configuration [SourceToken={0}] and video encoder configuration", token));
                    }

                    LogStepEvent("Getting media stream URI");
                    Media.StreamSetup setup = new Media.StreamSetup();
                    setup.Transport = new Media.Transport();
                    setup.Transport.Protocol = Media.TransportProtocol.UDP;
                    setup.Stream = Media.StreamType.RTPUnicast;
                    Media.MediaUri uri = media.GetStreamUri(setup, profile.token);
                    VideoUtils.AdjustVideo(
                        _videoForm,
                        _username,
                        _password,
                        _messageTimeout,
                        Media.TransportProtocol.UDP,
                        Media.StreamType.RTPUnicast,
                        uri,
                        profile.VideoEncoderConfiguration);

                    _videoForm.OpenWindow(false);
                }
                catch (Exception e)
                {
                    LogStepEvent("Error: " + e.Message);
                }
            }, "Opening video stream");
            DoRequestDelay();
        }
        protected void CloseVideo()
        {
            _videoForm.CloseWindow();
        }
    }
}
