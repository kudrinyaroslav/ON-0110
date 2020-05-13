///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.PTZ;
using Media = TestTool.Proxies.Media;

namespace TestTool.Tests.TestCases.TestSuites
{
//#if FULL
    [TestClass]
//#endif
    public class PTZMoveOperationsTestSuite : Base.PTZTest
    {
        public PTZMoveOperationsTestSuite(TestLaunchParam param)
            : base(param)
        {
        }
        
        [Test(Name = "PTZ ABSOLUTE MOVE",
            Path = "PTZ\\Move Operation",
            Order = "10.03.01",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZAbsolute })]
        public void AbsoluteMoveTest()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Media.Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                string reason = null;

                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                //select pantilt and zoom spaces
                Space2DDescription pantiltSpace = null;
                Space1DDescription zoomSpace = null;
                if (Features.Contains(Feature.PTZAbsolutePanTilt))
                {
                    pantiltSpace = options.Spaces.AbsolutePanTiltPositionSpace[0];
                }
                if(Features.Contains(Feature.PTZAbsoluteZoom))
                {
                    zoomSpace = options.Spaces.AbsoluteZoomPositionSpace[0];
                }

                //set selected spaces as default if they are not (we need it to get position in these spaces)
                if (((pantiltSpace != null) && !EqualSpaces(pantiltSpace.URI, profile.PTZConfiguration.DefaultAbsolutePantTiltPositionSpace))||
                    ((zoomSpace != null) && !EqualSpaces(zoomSpace.URI, profile.PTZConfiguration.DefaultAbsoluteZoomPositionSpace)))
                {
                    profile.PTZConfiguration.DefaultAbsolutePantTiltPositionSpace = pantiltSpace != null ? pantiltSpace.URI : null;
                    profile.PTZConfiguration.DefaultAbsoluteZoomPositionSpace = zoomSpace != null ? zoomSpace.URI : null;
                    SetConfiguration(profile.PTZConfiguration, false, string.Format("DefaultAbsolutePantTiltPositionSpace={0}, DefaultAbsoluteZoomPositionSpace={1}", profile.PTZConfiguration.DefaultAbsolutePantTiltPositionSpace, profile.PTZConfiguration.DefaultAbsoluteZoomPositionSpace));    
                }

                PTZStatus status = GetPTZStatus(profile.token);
                PTZVector pantilt = null;
                PTZVector zoom = null;
                if (pantiltSpace != null)
                {
                    PTZSpeed speed = null;
                    if ((options.Spaces.PanTiltSpeedSpace != null) && (options.Spaces.PanTiltSpeedSpace.Length > 0))
                    {
                        Space1DDescription space = options.Spaces.PanTiltSpeedSpace[0];
                        speed = new PTZSpeed();
                        speed.PanTilt = new Vector2D();
                        speed.PanTilt.space = space.URI;
                        speed.PanTilt.x = space.XRange.Max;
                        speed.PanTilt.y = space.XRange.Max;
                    }
                    pantilt = new PTZVector();
                    pantilt.PanTilt = new Vector2D();
                    pantilt.PanTilt.space = pantiltSpace.URI;
                    pantilt.PanTilt.x = pantiltSpace.XRange.Max;
                    pantilt.PanTilt.y = pantiltSpace.YRange.Max;
                    AbsoluteMove(profile.token, pantilt, speed);
                }
                if (zoomSpace != null)
                {
                    PTZSpeed speed = null;
                    if ((options.Spaces.ZoomSpeedSpace != null) && (options.Spaces.ZoomSpeedSpace.Length > 0))
                    {
                        Space1DDescription space = options.Spaces.ZoomSpeedSpace[0];
                        speed = new PTZSpeed();
                        speed.Zoom = new Vector1D();
                        speed.Zoom.space = space.URI;
                        speed.Zoom.x = space.XRange.Max;
                    }
                    zoom = new PTZVector();
                    zoom.Zoom = new Vector1D();
                    zoom.Zoom.space = zoomSpace.URI;
                    zoom.Zoom.x = zoomSpace.XRange.Max;
                    AbsoluteMove(profile.token, zoom, speed);
                }

                status = GetPTZStatus(profile.token);
                if(status.Position != null)
                {
                    CheckPTZPosition(status.Position, pantilt, zoom);
                }
            });
        }

        [Test(Name = "SOAP FAULT MESSAGE",
            Path = "PTZ\\Move Operation",
            Order = "10.03.02",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalShould,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZAbsolute })]
        public void AbsoluteMoveFaultTest()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Media.Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                string reason = null;
                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                if (Features.Contains(Feature.PTZAbsolutePanTilt))
                {
                    PTZVector pantilt = new PTZVector();
                    pantilt.PanTilt = new Vector2D();
                    pantilt.PanTilt.space = options.Spaces.AbsolutePanTiltPositionSpace[0].URI;
                    pantilt.PanTilt.x = options.Spaces.AbsolutePanTiltPositionSpace[0].XRange.Max + 1;
                    pantilt.PanTilt.y = options.Spaces.AbsolutePanTiltPositionSpace[0].YRange.Max + 1;
                    AbsoluteInvalidMove(profile.token, pantilt);
                }
                else if (Features.Contains(Feature.PTZAbsoluteZoom))
                {
                    PTZVector zoom = new PTZVector();
                    zoom.Zoom = new Vector1D();
                    zoom.Zoom.space = options.Spaces.AbsoluteZoomPositionSpace[0].URI;
                    zoom.Zoom.x = options.Spaces.AbsoluteZoomPositionSpace[0].XRange.Max + 1;
                    AbsoluteInvalidMove(profile.token, zoom);
                }
            });
        }

        [Test(Name = "PTZ RELATIVE MOVE",
            Path = "PTZ\\Move Operation",
            Order = "10.03.03",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZRelative })]
        public void RelativeMoveTest()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Media.Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                string reason = null;

                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                //Set default spaces to generic, if they are not. We need it to get position in generic spaces
                bool changed = false;
                if (Features.Contains(Feature.PTZRelativePanTilt) && !EqualSpaces(_absolutePanTiltSpace, profile.PTZConfiguration.DefaultAbsolutePantTiltPositionSpace))
                {
                    profile.PTZConfiguration.DefaultAbsolutePantTiltPositionSpace = _absolutePanTiltSpace;
                    changed = true;
                }
                if (Features.Contains(Feature.PTZRelativeZoom) && !EqualSpaces(_absoluteZoomSpace, profile.PTZConfiguration.DefaultAbsoluteZoomPositionSpace))
                {
                    profile.PTZConfiguration.DefaultAbsoluteZoomPositionSpace = _absoluteZoomSpace;
                    changed = true;
                }
                if(changed)
                {
                    SetConfiguration(profile.PTZConfiguration, false, string.Format("DefaultAbsolutePantTiltPositionSpace={0}, DefaultAbsoluteZoomPositionSpace={1}", profile.PTZConfiguration.DefaultAbsolutePantTiltPositionSpace, profile.PTZConfiguration.DefaultAbsoluteZoomPositionSpace));
                }

                PTZStatus status = GetPTZStatus(profile.token);
                PTZVector oldPosition = status.Position;
                PTZVector pantilt = null;
                PTZVector zoom = null;

                Space1DDescription zoomOptions = options.Spaces.RelativeZoomTranslationSpace.FirstOrDefault(o => string.Compare(o.URI, _relativeZoomSpace, true) == 0);
                Space2DDescription pantiltOptions = options.Spaces.RelativePanTiltTranslationSpace.FirstOrDefault(o => string.Compare(o.URI, _relativePanTiltSpace, true) == 0);

                if (Features.Contains(Feature.PTZRelativePanTilt))
                {
                    PTZSpeed speed = null;
                    if ((options.Spaces.PanTiltSpeedSpace != null) && (options.Spaces.PanTiltSpeedSpace.Length > 0))
                    {
                        Space1DDescription space = options.Spaces.PanTiltSpeedSpace[0];
                        speed = new PTZSpeed();
                        speed.PanTilt = new Vector2D();
                        speed.PanTilt.space = space.URI;
                        speed.PanTilt.x = space.XRange.Max;
                        speed.PanTilt.y = space.XRange.Max;
                    }
                    pantilt = new PTZVector();
                    pantilt.PanTilt = new Vector2D();
                    //use generic space, so we can calculate new position
                    pantilt.PanTilt.space = pantiltOptions.URI; //options.Spaces.RelativePanTiltTranslationSpace[0].URI;
                    pantilt.PanTilt.x = pantiltOptions.XRange.Max;// options.Spaces.RelativePanTiltTranslationSpace[0].XRange.Max;
                    pantilt.PanTilt.y = pantiltOptions.YRange.Max;// options.Spaces.RelativePanTiltTranslationSpace[0].YRange.Max;
                    RelativeMove(profile.token, pantilt, speed);
                }
                if (Features.Contains(Feature.PTZRelativeZoom))
                {
                    PTZSpeed speed = null;
                    if ((options.Spaces.ZoomSpeedSpace != null) && (options.Spaces.ZoomSpeedSpace.Length > 0))
                    {
                        Space1DDescription space = options.Spaces.ZoomSpeedSpace[0];
                        speed = new PTZSpeed();
                        speed.Zoom = new Vector1D();
                        speed.Zoom.space = space.URI;
                        speed.Zoom.x = space.XRange.Max;
                    }
                    zoom = new PTZVector();
                    zoom.Zoom = new Vector1D();
                    zoom.Zoom.space = zoomOptions.URI;
                    zoom.Zoom.x = zoomOptions.XRange.Max;
                    RelativeMove(profile.token, zoom, speed);
                }

                status = GetPTZStatus(profile.token);
                if((status.Position != null)&&(oldPosition != null))
                {
                    //translate position
                    if((oldPosition.PanTilt != null)&&(pantilt != null))
                    {
                        pantilt.PanTilt.x = pantilt.PanTilt.x + oldPosition.PanTilt.x > 1 ? 1 : pantilt.PanTilt.x + oldPosition.PanTilt.x;
                        pantilt.PanTilt.y = pantilt.PanTilt.y + oldPosition.PanTilt.y > 1 ? 1 : pantilt.PanTilt.y + oldPosition.PanTilt.y;
                    }
                    if((oldPosition.Zoom != null)&&(zoom != null))
                    {
                        zoom.Zoom.x = zoom.Zoom.x + oldPosition.Zoom.x > 1 ? 1 : zoom.Zoom.x + oldPosition.Zoom.x;
                    }

                    //change relative spaces to absolute spaces, because status returns position in absolute space
                    if(pantilt != null)
                    {
                        pantilt.PanTilt.space = _absolutePanTiltSpace;
                    }
                    if(zoom != null)
                    {
                        zoom.Zoom.space = _absoluteZoomSpace;
                    }
                    CheckPTZPosition(status.Position, pantilt, zoom);
                }
            });
        }

        [Test(Name = "PTZ CONTINUOUS MOVE",
            Path = "PTZ\\Move Operation",
            Order = "10.03.04",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ })]
        public void ContinuousMoveTest()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Media.Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                string reason = null;
                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                string timeout = "PT60S";//60 seconds by default
                //check ranges
                TimeSpan min = System.Xml.XmlConvert.ToTimeSpan(options.PTZTimeout.Min);
                TimeSpan max = System.Xml.XmlConvert.ToTimeSpan(options.PTZTimeout.Max);
                if (min.TotalMinutes > 1)
                {
                    timeout = options.PTZTimeout.Min;
                }
                else if (max.TotalMinutes < 1)
                {
                    timeout = options.PTZTimeout.Max;
                }

                //check pan\tilt movement
                PTZSpeed velocity = new PTZSpeed();
                velocity.PanTilt = new Vector2D();
                velocity.PanTilt.space = options.Spaces.ContinuousPanTiltVelocitySpace[0].URI;
                velocity.PanTilt.x = options.Spaces.ContinuousPanTiltVelocitySpace[0].XRange.Max;
                velocity.PanTilt.y = options.Spaces.ContinuousPanTiltVelocitySpace[0].YRange.Max;
                CheckContinuousMove(profile.token, velocity, timeout, options);

                if (Features.Contains(Feature.PTZContiniousZoom))
                {
                    //check zoom movement if supported
                    PTZSpeed zoomVelocity = new PTZSpeed();
                    zoomVelocity.Zoom = new Vector1D();
                    zoomVelocity.Zoom = new Vector1D();
                    zoomVelocity.Zoom.space = options.Spaces.ContinuousZoomVelocitySpace[0].URI;
                    zoomVelocity.Zoom.x = options.Spaces.ContinuousZoomVelocitySpace[0].XRange.Max;
                    CheckContinuousMove(profile.token, zoomVelocity, timeout, options);
                }
            });
        }

        [Test(Name = "PTZ CONTINUOUS MOVE & STOP",
            Path = "PTZ\\Move Operation",
            Order = "10.03.05",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ })]
        public void ContinuousMoveStopTest()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Media.Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                string reason = null;
                //TODO add validation
                //Assert(ValidatePTZConfiguration(profile.PTZConfiguration, out reason), reason, Resources.StepValidatePTZConfig_Title);

                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                //check pan\tilt movement
                PTZSpeed velocity = new PTZSpeed();
                velocity.PanTilt = new Vector2D();
                velocity.PanTilt.space = options.Spaces.ContinuousPanTiltVelocitySpace[0].URI;
                velocity.PanTilt.x = options.Spaces.ContinuousPanTiltVelocitySpace[0].XRange.Max;
                velocity.PanTilt.y = options.Spaces.ContinuousPanTiltVelocitySpace[0].YRange.Max;
                CheckContinuousMove(profile.token, velocity, null, options);

                if ((options.Spaces.ContinuousZoomVelocitySpace != null) && (options.Spaces.ContinuousZoomVelocitySpace.Length > 0))
                {
                    //check zoom movement if supported
                    PTZSpeed zoomVelocity = new PTZSpeed();
                    zoomVelocity.Zoom = new Vector1D();
                    zoomVelocity.Zoom.space = options.Spaces.ContinuousZoomVelocitySpace[0].URI;
                    zoomVelocity.Zoom.x = options.Spaces.ContinuousZoomVelocitySpace[0].XRange.Max;
                    CheckContinuousMove(profile.token, zoomVelocity, null, options);
                }
            });
        }
       
    }
}
