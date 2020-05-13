///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System.Linq;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Onvif;
using System.Threading;
namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class PTZPredefinedSpacesTestSuite : Base.PTZTest
    {
        public PTZPredefinedSpacesTestSuite(TestLaunchParam param)
            : base(param)
        {
        }
        
        [Test(Name = "GENERIC PAN/TILT POSITION SPACE",
            Path = "PTZ\\Predefined PTZ spaces\\Absolute Position Spaces",
            Order = "07.01.01",
            Id = "7-1-1",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZAbsolutePanTilt })]
        public void GenericPanTiltPosition()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();
                foreach (PTZNode node in nodes)
                {
                    string reason = null;
                    Assert(ValidatePTZNode(node, out reason), reason, Resources.StepValidatePTZNode_Title);

                    Space2DDescription[] spaces = node.SupportedPTZSpaces.AbsolutePanTiltPositionSpace;
                    Space2DDescription generic = spaces != null ?
                        spaces.FirstOrDefault(space => string.Compare(space.URI, _absolutePanTiltSpace) == 0) :
                        null;

                    Assert((generic != null) && (generic.XRange != null) && (generic.YRange != null),
                        "Node does not support generic absolute pan/tilt position space or space ranges are missing",
                        "Validating generic absolute pan/tilt position space");

                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    //check max ranges
                    PTZVector vector = new PTZVector();
                    vector.PanTilt = new Vector2D();
                    vector.PanTilt.space = generic.URI;
                    vector.PanTilt.x = generic.XRange.Max;
                    vector.PanTilt.y = generic.YRange.Max;

                    AbsoluteMove(profile.token, vector, null);

                    //check min ranges
                    vector.PanTilt.x = generic.XRange.Min;
                    vector.PanTilt.y = generic.YRange.Min;

                    AbsoluteMove(profile.token, vector, null);
                }
            });
        }

        [Test(Name = "GENERIC ZOOM POSITION SPACE",
            Path = "PTZ\\Predefined PTZ spaces\\Absolute Position Spaces",
            Order = "07.01.02",
            Id = "7-1-2",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZAbsoluteZoom })]
        public void GenericZoomPosition()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();
                foreach (PTZNode node in nodes)
                {
                    string reason = null;
                    Assert(ValidatePTZNode(node, out reason), reason, Resources.StepValidatePTZNode_Title);
                    
                    Space1DDescription[] spaces = node.SupportedPTZSpaces.AbsoluteZoomPositionSpace;
                    Space1DDescription generic = spaces != null ?
                        spaces.FirstOrDefault(space => string.Compare(space.URI, _absoluteZoomSpace) == 0) :
                        null;

                    Assert((generic != null) && (generic.XRange != null),
                        "Node does not support generic absolute zoom position space or space ranges are missing",
                        "Validating generic absolute zoom position space");

                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    //check max ranges
                    PTZVector vector = new PTZVector();
                    vector.Zoom = new Vector1D();
                    vector.Zoom.space = generic.URI;
                    vector.Zoom.x = generic.XRange.Max;

                    AbsoluteMove(profile.token, vector, null);

                    //check min ranges
                    vector.Zoom.x = generic.XRange.Min;

                    AbsoluteMove(profile.token, vector, null);
                }
            });
        }

        [Test(Name = "GENERIC PAN/TILT TRANSLATION SPACE",
            Path = "PTZ\\Predefined PTZ spaces\\Relative Translation Spaces",
            Order = "07.02.01",
            Id = "7-2-1",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZRelativePanTilt })]
        public void GenericPanTiltTranslation()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();
                foreach (PTZNode node in nodes)
                {
                    string reason = null;
                    Assert(ValidatePTZNode(node, out reason), reason, Resources.StepValidatePTZNode_Title);

                    Space2DDescription[] spaces = node.SupportedPTZSpaces.RelativePanTiltTranslationSpace;
                    Space2DDescription generic = spaces != null ?
                        spaces.FirstOrDefault(space => string.Compare(space.URI, _relativePanTiltSpace) == 0) :
                        null;

                    Assert((generic != null) && (generic.XRange != null) && (generic.YRange != null),
                        "Node does not support generic relative pan/tilt translation space or space ranges are missing",
                        "Validating generic relative pan/tilt translation space");

                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    //PTZNode node = GetNode(profile.PTZConfiguration.NodeToken);

                    //check max ranges
                    PTZVector vector = new PTZVector();
                    vector.PanTilt = new Vector2D();
                    vector.PanTilt.space = generic.URI;
                    vector.PanTilt.x = generic.XRange.Max;
                    vector.PanTilt.y = generic.YRange.Max;

                    RelativeMove(profile.token, vector, null);

                    //check min ranges
                    vector.PanTilt.x = generic.XRange.Min;
                    vector.PanTilt.y = generic.YRange.Min;

                    RelativeMove(profile.token, vector, null);
                }
            });
        }

        [Test(Name = "GENERIC ZOOM TRANSLATION SPACE",
            Path = "PTZ\\Predefined PTZ spaces\\Relative Translation Spaces",
            Order = "07.02.02",
            Id = "7-2-2",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZRelativeZoom })]
        public void GenericZoomTranslation()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();
                foreach (PTZNode node in nodes)
                {
                    string reason = null;
                    Assert(ValidatePTZNode(node, out reason), reason, Resources.StepValidatePTZNode_Title);

                    Space1DDescription[] spaces = node.SupportedPTZSpaces.RelativeZoomTranslationSpace;
                    Space1DDescription generic = spaces != null ?
                        spaces.FirstOrDefault(space => string.Compare(space.URI, _relativeZoomSpace) == 0) :
                        null;

                    Assert((generic != null) && (generic.XRange != null),
                        "Node does not support generic relative zoom translation space or space ranges are missing",
                        "Validating generic relative zoom translation space");

                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    //check max ranges
                    PTZVector vector = new PTZVector();
                    vector.Zoom = new Vector1D();
                    vector.Zoom.space = generic.URI;
                    vector.Zoom.x = generic.XRange.Max;

                    RelativeMove(profile.token, vector, null);

                    //check min ranges
                    vector.Zoom.x = generic.XRange.Min;

                    RelativeMove(profile.token, vector, null);
                }
            });
        }

        [Test(Name = "GENERIC PAN/TILT VELOCITY SPACE",
            Path = "PTZ\\Predefined PTZ spaces\\Continuous Velocity Spaces",
            Order = "07.03.01",
            Id = "7-3-1",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZContiniousPanTilt })]
        public void GenericPanTiltVelocity()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();
                foreach (PTZNode node in nodes)
                {
                    string reason = null;
                    Assert(ValidatePTZNode(node, out reason), reason, Resources.StepValidatePTZNode_Title);

                    Space2DDescription[] spaces = node.SupportedPTZSpaces.ContinuousPanTiltVelocitySpace;
                    Space2DDescription generic = spaces != null ?
                        spaces.FirstOrDefault(space => string.Compare(space.URI, _continuousPanTiltSpace) == 0) :
                        null;

                    Assert((generic != null) && (generic.XRange != null) && (generic.YRange != null),
                        "Node does not support generic continuous pan/tilt velocity space or space ranges are missing",
                        "Validating generic continuous pan/tilt velocity space");

                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    //PTZNode node = GetNode(profile.PTZConfiguration.NodeToken);

                    //check max ranges
                    PTZSpeed velocity = new PTZSpeed();
                    velocity.PanTilt = new Vector2D();
                    velocity.PanTilt.space = generic.URI;
                    velocity.PanTilt.x = generic.XRange.Max;
                    velocity.PanTilt.y = generic.YRange.Max;

                    ContinuousMove(profile.token, velocity, null);
                    RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                    //check min ranges
                    velocity.PanTilt.x = generic.XRange.Min;
                    velocity.PanTilt.y = generic.YRange.Min;

                    ContinuousMove(profile.token, velocity, null);
                    RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                    Stop(profile.token, true, false);
                }
            });
        }

        [Test(Name = "GENERIC ZOOM VELOCITY SPACE",
            Path = "PTZ\\Predefined PTZ spaces\\Continuous Velocity Spaces",
            Order = "07.03.02",
            Id = "7-3-2",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZContiniousZoom })]
        public void GenericZoomVelocity()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();
                foreach (PTZNode node in nodes)
                {
                    string reason = null;
                    Assert(ValidatePTZNode(node, out reason), reason, Resources.StepValidatePTZNode_Title);

                    Space1DDescription[] spaces = node.SupportedPTZSpaces.ContinuousZoomVelocitySpace;
                    Space1DDescription generic = spaces != null ?
                        spaces.FirstOrDefault(space => string.Compare(space.URI, _continuousZoomSpace) == 0) :
                        null;

                    Assert((generic != null) && (generic.XRange != null),
                        "Node does not support generic continuous zoom velocity space or space ranges are missing",
                        "Validating generic continuous zoom velocity space");

                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    //PTZNode node = GetNode(profile.PTZConfiguration.NodeToken);

                    //check max ranges
                    PTZSpeed velocity = new PTZSpeed();
                    velocity.Zoom = new Vector1D();
                    velocity.Zoom.space = generic.URI;
                    velocity.Zoom.x = generic.XRange.Max;

                    ContinuousMove(profile.token, velocity, null);
                    RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                    //check min ranges
                    velocity.Zoom.x = generic.XRange.Min;

                    ContinuousMove(profile.token, velocity, null);
                    RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                    Stop(profile.token, false, true);
                }
            });
        }

        [Test(Name = "GENERIC PAN/TILT SPEED SPACE",
             Path = "PTZ\\Predefined PTZ spaces\\Speed Spaces",
            Order = "07.04.01",
            Id = "7-4-1",
            Category = Category.PTZ,
             Version = 1.02,
             RequirementLevel = RequirementLevel.ConditionalMust,
             RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZSpeedPanTilt })]
        public void GenericPanTiltSpeed()
        {
            RunTest(() =>
            {
                //Assert(Features.ContainsFeature(Feature.PTZAbsoluteOrRelativePanTilt),
                //   "No Absolute or Relative Pan/Tilt movement is supported",
                //   "Check that Absolute or Relative Pan/Tilt movement is supported");
                
                PTZNode[] nodes = GetNodes();
                foreach (PTZNode node in nodes)
                {
                    string reason = null;
                    Assert(ValidatePTZNode(node, out reason), reason, Resources.StepValidatePTZNode_Title);

                    Space1DDescription[] spaces = node.SupportedPTZSpaces.PanTiltSpeedSpace;
                    Space1DDescription generic = spaces != null ?
                        spaces.FirstOrDefault(space => string.Compare(space.URI, _speedPanTiltSpace) == 0) :
                        null;

                    Assert((generic != null) && (generic.XRange != null),
                        "Node does not support generic pan/tilt speed space or space ranges are missing",
                        "Validating generic pan/tilt speed space");

                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                    
                    Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.token, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                    //check max ranges
                    bool absoulteMoveSupported = Features.Contains(Feature.PTZAbsolutePanTilt);
                    PTZVector vector = new PTZVector();
                    vector.PanTilt = new Vector2D();
                    Space2DDescription pantiltSpace = absoulteMoveSupported ? options.Spaces.AbsolutePanTiltPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absolutePanTiltSpace, true) == 0) :
                        options.Spaces.RelativePanTiltTranslationSpace.FirstOrDefault(s => string.Compare(s.URI, _relativePanTiltSpace, true) == 0);

                    //absolute or relative move should be supported - use generic space for position or translation
                    vector.PanTilt.space = pantiltSpace.URI;
                    vector.PanTilt.x = pantiltSpace.XRange.Min;
                    vector.PanTilt.y = pantiltSpace.YRange.Min;

                    PTZSpeed speed = new PTZSpeed();
                    speed.PanTilt = new Vector2D();
                    speed.PanTilt.space = generic.URI;
                    speed.PanTilt.x = generic.XRange.Max;
                    speed.PanTilt.y = generic.XRange.Max;
                    if (absoulteMoveSupported)
                    {
                        AbsoluteMove(profile.token, vector, speed);
                    }
                    else
                    {
                        RelativeMove(profile.token, vector, speed);
                    }


                    //check min ranges
                    speed.PanTilt.x = generic.XRange.Min;
                    speed.PanTilt.y = generic.XRange.Min;
                    if (absoulteMoveSupported)
                    {
                        AbsoluteMove(profile.token, vector, speed);
                    }
                    else
                    {
                        RelativeMove(profile.token, vector, speed);
                    }
                }
            });
        }
        
        [Test(Name = "GENERIC ZOOM SPEED SPACE",
            Path = "PTZ\\Predefined PTZ spaces\\Speed Spaces",
            Order = "07.04.02",
            Id = "7-4-2",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZSpeedZoom })]
        public void GenericZoomSpeed()
        {
            RunTest(() =>
            {
                //Assert(Features.ContainsFeature(Feature.PTZAbsoluteOrRelativeZoom),
                //   "No Absolute or Relative Zoom movement is supported",
                //   "Check that Absolute or Relative Zoom movement is supported");
                
                PTZNode[] nodes = GetNodes();
                foreach (PTZNode node in nodes)
                {
                    string reason = null;
                    Assert(ValidatePTZNode(node, out reason), reason, Resources.StepValidatePTZNode_Title);

                    Space1DDescription[] spaces = node.SupportedPTZSpaces.ZoomSpeedSpace;
                    Space1DDescription generic = spaces != null ?
                        spaces.FirstOrDefault(space => string.Compare(space.URI, _speedZoomSpace) == 0) :
                        null;

                    Assert((generic != null) && (generic.XRange != null),
                        "Node does not support generic zoom speed space or space ranges are missing",
                        "Validating generic zoom speed space");

                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                    
                    Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.token, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                    //check max ranges
                    bool absoulteMoveSupported = Features.Contains(Feature.PTZAbsoluteZoom);
                    PTZVector vector = new PTZVector();
                    vector.Zoom = new Vector1D();
                    //absolute or relative move should be supported - use generic space for position or translation
                    Space1DDescription zoomSpace = absoulteMoveSupported ? options.Spaces.AbsoluteZoomPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absoluteZoomSpace, true) == 0) :
                        options.Spaces.RelativeZoomTranslationSpace.FirstOrDefault(s => string.Compare(s.URI, _relativeZoomSpace, true) == 0);

                    vector.Zoom.space = zoomSpace.URI;
                    vector.Zoom.x = zoomSpace.XRange.Min;

                    PTZSpeed speed = new PTZSpeed();
                    speed.Zoom = new Vector1D();
                    speed.Zoom.space = generic.URI;
                    speed.Zoom.x = generic.XRange.Max;

                    if (absoulteMoveSupported)
                    {
                        AbsoluteMove(profile.token, vector, speed);
                    }
                    else
                    {
                        RelativeMove(profile.token, vector, speed);
                    }

                    //check min ranges
                    speed.Zoom.x = generic.XRange.Min;
                    if (absoulteMoveSupported)
                    {
                        AbsoluteMove(profile.token, vector, speed);
                    }
                    else
                    {
                        RelativeMove(profile.token, vector, speed);
                    }
                }
            });
        }
    }
}
