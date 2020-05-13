///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Linq;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class PTZMoveOperationsTestSuite : Base.PTZTest
    {
        public PTZMoveOperationsTestSuite(TestLaunchParam param)
            : base(param)
        {
        }
        
        [Test(Name = "PTZ ABSOLUTE MOVE",
            Path = "PTZ\\Move Operation",
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService, Feature.PTZAbsolute },
            FunctionalityUnderTest = new Functionality[] { Functionality.PtzAbsoluteMove, Functionality.PtzGetStatus })]
        public void AbsoluteMoveTest()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                string reason = null;

                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                Assert(options.Spaces.AbsolutePanTiltPositionSpace != null || options.Spaces.AbsoluteZoomPositionSpace != null,
                    "Node does not support Absolute move",
                    "Check if Absolute move is supported");

                //select pantilt and zoom spaces
                Space2DDescription pantiltSpace = null;
                Space1DDescription zoomSpace = null;

                //set selected spaces as default if they are not (we need it to get position in these spaces)
                BeginStep("Check if configuration needs to be updated");
                bool updateNeeded = false;

                string dump = string.Empty;

                if (Features.Contains(Feature.PTZAbsolutePanTilt))
                {
                    pantiltSpace = options.Spaces.AbsolutePanTiltPositionSpace[0];
                    bool update = false;
                    if (string.IsNullOrEmpty(profile.PTZConfiguration.DefaultAbsolutePantTiltPositionSpace))
                    {
                        update = true;
                    }
                    else
                    {
                        update =
                            (!EqualSpaces(pantiltSpace.URI,
                                          profile.PTZConfiguration.DefaultAbsolutePantTiltPositionSpace));
                    }
                    if (update)
                    {
                        updateNeeded = true;
                        dump += string.Format("DefaultAbsolutePantTiltPositionSpace={0}", pantiltSpace.URI);
                        profile.PTZConfiguration.DefaultAbsolutePantTiltPositionSpace = pantiltSpace.URI;
                    }
                }
                
                if(Features.Contains(Feature.PTZAbsoluteZoom))
                {
                    zoomSpace = options.Spaces.AbsoluteZoomPositionSpace[0];
                    bool update = false;
                    if (string.IsNullOrEmpty(profile.PTZConfiguration.DefaultAbsoluteZoomPositionSpace))
                    {
                        update = true;
                    }
                    else
                    {
                        update = !EqualSpaces(zoomSpace.URI, profile.PTZConfiguration.DefaultAbsoluteZoomPositionSpace);
                    }
                    if (update)
                    {
                        updateNeeded = true;

                        string localdump =
                            string.Format(
                                "DefaultAbsoluteZoomPositionSpace={0}",
                                zoomSpace.URI);
                        if (string.IsNullOrEmpty(dump))
                        {
                            dump = localdump;
                        }
                        else
                        {
                            dump += string.Format(", {0}", localdump);
                        }
                        profile.PTZConfiguration.DefaultAbsoluteZoomPositionSpace = zoomSpace.URI;
                    }
                }

                StepPassed();
                
                if (updateNeeded)
                {
                    SetConfiguration(profile.PTZConfiguration, false, dump);    
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
                    CheckPTZPosition(status.Position, pantilt, zoom, pantiltSpace, zoomSpace);
                }
            });
        }

        [Test(Name = "SOAP FAULT MESSAGE",
            Path = "PTZ\\Move Operation",
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService, Feature.PTZAbsolute })]
        public void AbsoluteMoveFaultTest()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Profile profile = GetPTZProfile(_ptzNodeToken, out options);
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
            Order = "03.01.03",
            Id = "3-1-3",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService, Feature.PTZRelative },
            FunctionalityUnderTest = new Functionality[] { Functionality.PtzRelativeMove, Functionality.PtzGetStatus })]
        public void RelativeMoveTest()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                string reason = null;

                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                Assert(options.Spaces.RelativePanTiltTranslationSpace != null || options.Spaces.RelativeZoomTranslationSpace != null,
                    "Node does not support Relative move", 
                    "Check if Relative move is supported");

                //Set default spaces to generic, if they are not. We need it to get position in generic spaces
                bool changed = false;
                string dump = string.Empty;

                BeginStep("Check if configuration needs to be updated");
                if (Features.Contains(Feature.PTZRelativePanTilt) && 
                    (string.IsNullOrEmpty(profile.PTZConfiguration.DefaultRelativePanTiltTranslationSpace) || 
                    !EqualSpaces(_relativePanTiltSpace, profile.PTZConfiguration.DefaultRelativePanTiltTranslationSpace)))
                {
                    profile.PTZConfiguration.DefaultRelativePanTiltTranslationSpace = _relativePanTiltSpace;
                    changed = true;
                    dump = string.Format("DefaultRelativePanTiltTranslationSpace={0}",
                                         profile.PTZConfiguration.DefaultRelativePanTiltTranslationSpace);
                }
                if (Features.Contains(Feature.PTZRelativeZoom) && 
                    (string.IsNullOrEmpty(profile.PTZConfiguration.DefaultRelativeZoomTranslationSpace) || 
                    !EqualSpaces(_relativeZoomSpace, profile.PTZConfiguration.DefaultRelativeZoomTranslationSpace)))
                {
                    profile.PTZConfiguration.DefaultRelativeZoomTranslationSpace = _relativeZoomSpace;
                    changed = true;
                    if (!string.IsNullOrEmpty(dump))
                    {
                        dump += ", ";
                    }
                    dump += string.Format("DefaultRelativeZoomTranslationSpace={0}", profile.PTZConfiguration.DefaultRelativeZoomTranslationSpace);

                }
                StepPassed();

                if(changed)
                {
                    SetConfiguration(profile.PTZConfiguration, 
                        false,
                        dump);
                }

                PTZStatus status = GetPTZStatus(profile.token);
                PTZVector oldPosition = status.Position;
                PTZVector pantilt = null;
                PTZVector zoom = null;

                Space1DDescription zoomOptions = null;
                if (Features.Contains(Feature.PTZRelativeZoom))
                {
                    zoomOptions =
                        options.Spaces.RelativeZoomTranslationSpace.FirstOrDefault(
                            o => string.Compare(o.URI, _relativeZoomSpace, true) == 0);
                }

                Space2DDescription pantiltOptions = null;
                if (Features.Contains(Feature.PTZRelativePanTilt))
                {
                    pantiltOptions =
                        options.Spaces.RelativePanTiltTranslationSpace.FirstOrDefault(
                            o => string.Compare(o.URI, _relativePanTiltSpace, true) == 0);
                }

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
                    CheckPTZPosition(status.Position, pantilt, zoom, pantiltOptions, zoomOptions);
                }
            });
        }

        [Test(Name = "PTZ CONTINUOUS MOVE",
            Path = "PTZ\\Move Operation",
            Order = "03.01.04",
            Id = "3-1-4",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.PtzContinuousMove })]
        public void ContinuousMoveTest()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                string reason = null;
                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                Assert(options.Spaces.ContinuousPanTiltVelocitySpace != null || options.Spaces.ContinuousZoomVelocitySpace!= null,
                    "Node does not support Continuous move",
                    "Check if Continuous move is supported");


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
                if (Features.ContainsFeature(Feature.PTZContinuousPanTilt))
                {
                    PTZSpeed velocity = new PTZSpeed();
                    velocity.PanTilt = new Vector2D();
                    velocity.PanTilt.space = options.Spaces.ContinuousPanTiltVelocitySpace[0].URI;
                    velocity.PanTilt.x = options.Spaces.ContinuousPanTiltVelocitySpace[0].XRange.Max;
                    velocity.PanTilt.y = options.Spaces.ContinuousPanTiltVelocitySpace[0].YRange.Max;
                    CheckContinuousMove(profile.token, velocity, timeout, options);
                }

                if (Features.Contains(Feature.PTZContinuousZoom))
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
            Order = "03.01.05",
            Id = "3-1-5",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.PtzContinuousMove, Functionality.PtzStop })]
        public void ContinuousMoveStopTest()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                string reason = null;

                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                //check pan\tilt movement
                if (options.Spaces.ContinuousPanTiltVelocitySpace != null && options.Spaces.ContinuousPanTiltVelocitySpace.Length > 0)
                {
                    PTZSpeed velocity = new PTZSpeed();
                    velocity.PanTilt = new Vector2D();
                    velocity.PanTilt.space = options.Spaces.ContinuousPanTiltVelocitySpace[0].URI;
                    velocity.PanTilt.x = options.Spaces.ContinuousPanTiltVelocitySpace[0].XRange.Max;
                    velocity.PanTilt.y = options.Spaces.ContinuousPanTiltVelocitySpace[0].YRange.Max;
                    CheckContinuousMove(profile.token, velocity, null, options);
                }

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
