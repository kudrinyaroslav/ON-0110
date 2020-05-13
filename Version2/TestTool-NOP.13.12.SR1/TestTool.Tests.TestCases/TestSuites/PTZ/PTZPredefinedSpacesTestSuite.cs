///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System.Linq;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using System.Threading;
using TestTool.Tests.Engine.Base.Definitions;

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
            Order = "07.01.03",
            Id = "7-1-3",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[]{Functionality.PtzAbsoluteMove})]
        public void GenericPanTiltPosition()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();

                Profile[] profiles = GetProfiles();

                foreach (PTZNode node in nodes)
                {
                    // select node that supports AbsolutePanTiltPositionSpace
                    Space2DDescription[] absolutePanTiltSpaces = node.SupportedPTZSpaces.AbsolutePanTiltPositionSpace;
                    if (absolutePanTiltSpaces != null)
                    {
                        LogTestEvent(string.Format("Node (token = {0}) supports AbsolutePanTilt move{1}", node.token, System.Environment.NewLine));
                    }
                    else
                    {
                        // if current node doesn't support AbsolutePanTiltPositionSpace 
                        // then check next node
                        continue;
                    }

                    // verify that AbsolutePanTiltPositionSpace contains Position Generic Pan/Tilt Space
                    Space2DDescription positionGenericPanTiltSpace = 
                        absolutePanTiltSpaces.FirstOrDefault(space => space.URI == _absolutePanTiltSpace);

                    Assert(positionGenericPanTiltSpace != null,
                        "AbsolutePanTiltPositionSpace element doesn't contain mandatory space", 
                        "Verifying of Position Generic Pan/Tilt Space presence");

                    // verify that allowed range is specified
                    Assert(positionGenericPanTiltSpace.XRange.Max >= positionGenericPanTiltSpace.XRange.Min && 
                           positionGenericPanTiltSpace.YRange.Max >= positionGenericPanTiltSpace.YRange.Min,
                        "Incorrect space range",
                        "Verifying of space range");

                    // configure and select a media profile 
                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(profiles, node.token, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    PTZVector vector = new PTZVector();
                    vector.PanTilt = new Vector2D();

                    // check min ranges
                    vector.PanTilt.space = positionGenericPanTiltSpace.URI;
                    vector.PanTilt.x = positionGenericPanTiltSpace.XRange.Min;
                    vector.PanTilt.y = positionGenericPanTiltSpace.YRange.Min;

                    AbsoluteMove(profile.token, vector, null);

                    // check max ranges
                    vector.PanTilt.space = positionGenericPanTiltSpace.URI;
                    vector.PanTilt.x = positionGenericPanTiltSpace.XRange.Max;
                    vector.PanTilt.y = positionGenericPanTiltSpace.YRange.Max;

                    AbsoluteMove(profile.token, vector, null);
               }
            });
        }

        [Test(Name = "GENERIC ZOOM POSITION SPACE",
            Path = "PTZ\\Predefined PTZ spaces\\Absolute Position Spaces",
            Order = "07.01.04",
            Id = "7-1-4",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService},
            FunctionalityUnderTest = new Functionality[]{Functionality.PtzAbsoluteMove})]
        public void GenericZoomPosition()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();

                foreach (PTZNode node in nodes)
                {
                    // select node that supports AbsoluteZoomPositionSpace
                    Space1DDescription[] absoluteZoomSpaces = node.SupportedPTZSpaces.AbsoluteZoomPositionSpace;
                    if (absoluteZoomSpaces != null)
                    {
                        LogTestEvent(string.Format("Node (token = {0}) supports AbsoluteZoom move{1}", node.token, System.Environment.NewLine));
                    }
                    else
                    {
                        // if current node doesn't support AbsoluteZoomPositionSpace 
                        // then check next node
                        continue;
                    }

                    // verify that AbsoluteZoomPositionSpace contains Position Generic Zoom Space
                    Space1DDescription positionGenericZoomSpace =
                        absoluteZoomSpaces.FirstOrDefault(space => space.URI == _absoluteZoomSpace);

                    Assert(positionGenericZoomSpace != null,
                        "AbsoluteZoomPositionSpace element doesn't contain mandatory space",
                        "Verifying of Position Generic Zoom Space presence");

                    // verify that allowed range is specified
                    Assert(positionGenericZoomSpace.XRange.Max >= positionGenericZoomSpace.XRange.Min,
                        "Incorrect space range",
                        "Verifying of space range");

                    // configure and select a media profile 
                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    PTZVector vector = new PTZVector();
                    vector.Zoom = new Vector1D();

                    // check max ranges
                    vector.Zoom.space = positionGenericZoomSpace.URI;
                    vector.Zoom.x = positionGenericZoomSpace.XRange.Max;

                    AbsoluteMove(profile.token, vector, null);

                    // check min ranges
                    vector.Zoom.space = positionGenericZoomSpace.URI;
                    vector.Zoom.x = positionGenericZoomSpace.XRange.Min;

                    AbsoluteMove(profile.token, vector, null);
                }
            });
        }

        [Test(Name = "GENERIC PAN/TILT TRANSLATION SPACE",
            Path = "PTZ\\Predefined PTZ spaces\\Relative Translation Spaces",
            Order = "07.02.03",
            Id = "7-2-3",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[]{Functionality.PtzRelativeMove})]
        public void GenericPanTiltTranslation()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();

                foreach (PTZNode node in nodes)
                {
                    // select node that supports RelativePanTiltPositionSpace
                    Space2DDescription[] relativePanTiltSpaces = node.SupportedPTZSpaces.RelativePanTiltTranslationSpace;
                    if (relativePanTiltSpaces != null)
                    {
                        LogTestEvent(string.Format("Node (token = {0}) supports RelativePanTilt move{1}", node.token, System.Environment.NewLine));
                    }
                    else
                    {
                        // if current node doesn't support RelativePanTiltPositionSpace 
                        // then check next node
                        continue;
                    }

                    // verify that RelativePanTiltPositionSpace contains Translation Generic Pan/Tilt Space
                    Space2DDescription translationGenericPanTiltSpace =
                        relativePanTiltSpaces.FirstOrDefault(space => space.URI == _relativePanTiltSpace);

                    Assert(translationGenericPanTiltSpace != null,
                        "RelativePanTiltPositionSpace element doesn't contain mandatory space",
                        "Verifying of Translation Generic Pan/Tilt Space presence");

                    // verify that allowed range is specified
                    Assert(translationGenericPanTiltSpace.XRange.Max >= translationGenericPanTiltSpace.XRange.Min &&
                           translationGenericPanTiltSpace.YRange.Max >= translationGenericPanTiltSpace.YRange.Min,
                        "Incorrect space range",
                        "Verifying of space range");

                    // configure and select a media profile 
                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    PTZVector vector = new PTZVector();
                    vector.PanTilt = new Vector2D();

                    // check min ranges
                    vector.PanTilt.space = translationGenericPanTiltSpace.URI;
                    vector.PanTilt.x = translationGenericPanTiltSpace.XRange.Min;
                    vector.PanTilt.y = translationGenericPanTiltSpace.YRange.Min;

                    RelativeMove(profile.token, vector, null);

                    // check max ranges
                    vector.PanTilt.space = translationGenericPanTiltSpace.URI;
                    vector.PanTilt.x = translationGenericPanTiltSpace.XRange.Max;
                    vector.PanTilt.y = translationGenericPanTiltSpace.YRange.Max;

                    RelativeMove(profile.token, vector, null);
                }
            });
        }

        [Test(Name = "GENERIC ZOOM TRANSLATION SPACE",
            Path = "PTZ\\Predefined PTZ spaces\\Relative Translation Spaces",
            Order = "07.02.04",
            Id = "7-2-4",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService },
            FunctionalityUnderTest = new Functionality[] { Functionality.PtzRelativeMove })]
        public void GenericZoomTranslation()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();

                foreach (PTZNode node in nodes)
                {
                    // select node that supports RelativeZoomPositionSpace
                    Space1DDescription[] relativeZoomSpaces = node.SupportedPTZSpaces.RelativeZoomTranslationSpace;
                    if (relativeZoomSpaces != null)
                    {
                        LogTestEvent(string.Format("Node (token = {0}) supports RelativeZoom move{1}", node.token, System.Environment.NewLine));
                    }
                    else
                    {
                        // if current node doesn't support RelativeZoomPositionSpace 
                        // then check next node
                        continue;
                    }

                    // verify that RelativeZoomPositionSpace contains Translation Generic Zoom Space
                    Space1DDescription translationGenericZoomSpace =
                        relativeZoomSpaces.FirstOrDefault(space => space.URI == _relativeZoomSpace);

                    Assert(translationGenericZoomSpace != null,
                        "RelativeZoomPositionSpace element doesn't contain mandatory space",
                        "Verifying of Translation Generic Zoom Space presence");

                    // verify that allowed range is specified
                    Assert(translationGenericZoomSpace.XRange.Max >= translationGenericZoomSpace.XRange.Min,
                        "Incorrect space range",
                        "Verifying of space range");

                    // configure and select a media profile 
                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    PTZVector vector = new PTZVector();
                    vector.Zoom = new Vector1D();

                    // check max ranges
                    vector.Zoom.space = translationGenericZoomSpace.URI;
                    vector.Zoom.x = translationGenericZoomSpace.XRange.Max;

                    RelativeMove(profile.token, vector, null);

                    // check min ranges
                    vector.Zoom.space = translationGenericZoomSpace.URI;
                    vector.Zoom.x = translationGenericZoomSpace.XRange.Min;

                    RelativeMove(profile.token, vector, null);
                }
            });
        }

        [Test(Name = "GENERIC PAN/TILT VELOCITY SPACE",
            Path = "PTZ\\Predefined PTZ spaces\\Continuous Velocity Spaces",
            Order = "07.03.03",
            Id = "7-3-3",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService},
            FunctionalityUnderTest = new Functionality[] { Functionality.PtzContinuousMove })]
        public void GenericPanTiltVelocityBis()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();
                int Count = 0;
                foreach (PTZNode node in nodes)
                {
                    // select node that supports ContinuousPanTiltPositionSpace
                    Space2DDescription[] spaces = node.SupportedPTZSpaces.ContinuousPanTiltVelocitySpace;
                    if (spaces != null)
                    {
                        LogTestEvent(string.Format("Node (token = {0}) supports ContinuousPanTilt move{1}", node.token, System.Environment.NewLine));
                    }
                    else
                    {
                        // if current node doesn't support ContinuousPanTiltVelocitySpace 
                        // then check next node
                        continue;
                    }
                    Count++;

                    // verify that ContinuousPanTiltVelocitySpace contains Velocity Generic Pan/Tilt Space
                    Space2DDescription generic =
                        spaces.FirstOrDefault(space => space.URI == _continuousPanTiltSpace);

                    Assert((generic != null) && (generic.XRange != null) && (generic.YRange != null),
                        "ContinuousPanTiltVelocitySpace element doesn't contain mandatory space",
                        "Verifying of Velocity Generic Pan/Tilt Space presence");

                    // verify that allowed range is specified
                    Assert(generic.XRange.Max >= generic.XRange.Min &&
                           generic.YRange.Max >= generic.YRange.Min,
                        "Incorrect space range",
                        "Verifying of space range");

                    // configure and select a media profile 
                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    PTZSpeed velocity = new PTZSpeed();
                    velocity.PanTilt = new Vector2D();

                    // check min ranges
                    velocity.PanTilt.space = generic.URI;
                    velocity.PanTilt.x = generic.XRange.Min;
                    velocity.PanTilt.y = generic.YRange.Min;

                    ContinuousMove(profile.token, velocity, null);
                    RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                    // check max ranges
                    velocity.PanTilt.space = generic.URI;
                    velocity.PanTilt.x = generic.XRange.Max;
                    velocity.PanTilt.y = generic.YRange.Max;

                    ContinuousMove(profile.token, velocity, null);
                    RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                    Stop(profile.token, true, false);
                }
                //Assert(Count > 0,
                //    "Continuous is a must, but no nodes support it",
                //    "Verifying ContinuousPanTilt support nodes count");

            });
        }

        [Test(Name = "GENERIC ZOOM VELOCITY SPACE",
            Path = "PTZ\\Predefined PTZ spaces\\Continuous Velocity Spaces",
            Order = "07.03.04",
            Id = "7-3-4",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService},
            FunctionalityUnderTest = new Functionality[] { Functionality.PtzContinuousMove })]
        public void GenericZoomVelocityBis()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();
                int Count = 0;
                foreach (PTZNode node in nodes)
                {
                    // select node that supports ContinuousZoomPositionSpace
                    Space1DDescription[] spaces = node.SupportedPTZSpaces.ContinuousZoomVelocitySpace;
                    if (spaces != null)
                    {
                        LogTestEvent(string.Format("Node (token = {0}) supports ContinuousZoom move{1}", node.token, System.Environment.NewLine));
                    }
                    else
                    {
                        // if current node doesn't support RelativeZoomPositionSpace 
                        // then check next node
                        continue;
                    }
                    Count++;

                    // verify that ContinuousZoomPositionSpace contains Velocity Generic Zoom Space
                    Space1DDescription generic =
                        spaces.FirstOrDefault(space => space.URI == _continuousZoomSpace);

                    Assert((generic != null) && (generic.XRange != null),
                        "ContinuousZoomVelocitySpace element doesn't contain mandatory space",
                        "Verifying of Continuous Generic Zoom Space presence");

                    // verify that allowed range is specified
                    Assert(generic.XRange.Max >= generic.XRange.Min,
                        "Incorrect space range",
                        "Verifying of space range");

                    // configure and select a media profile 
                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    PTZSpeed velocity = new PTZSpeed();
                    velocity.Zoom = new Vector1D();
                    velocity.Zoom.space = generic.URI;

                    // check max ranges
                    velocity.Zoom.x = generic.XRange.Max;

                    ContinuousMove(profile.token, velocity, null);
                    RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                    // check min ranges
                    velocity.Zoom.x = generic.XRange.Min;

                    ContinuousMove(profile.token, velocity, null);
                    RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                    Stop(profile.token, false, true);
                }
                //Assert(Count > 0,
                //    "Continuous is a must, but no nodes support it",
                //    "Verifying ContinuousZoom support nodes count");

            });
        }

        [Test(Name = "GENERIC PAN/TILT SPEED SPACE",
             Path = "PTZ\\Predefined PTZ spaces\\Speed Spaces",
             Order = "07.04.03",
             Id = "7-4-3",
             Category = Category.PTZ,
             Version = 1.02,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.PTZService })]
        public void GenericPanTiltSpeedBis()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();
                foreach (PTZNode node in nodes)
                {
                    // select node that supports SpeedPanTiltPositionSpace
                    Space1DDescription[] spaces = node.SupportedPTZSpaces.PanTiltSpeedSpace;
                    if (spaces != null)
                    {
                        LogTestEvent(string.Format("Node (token = {0}) supports Speed for PanTilt move{1}", node.token, System.Environment.NewLine));
                    }
                    else
                    {
                        // if current node doesn't support SpeedPanTiltPositionSpace 
                        // then check next node
                        continue;
                    }

                    // verify that SpeedPanTiltPositionSpace contains Speed Generic Zoom Space
                    Space1DDescription generic =
                        spaces.FirstOrDefault(space => space.URI == _speedPanTiltSpace);

                    Assert((generic != null) && (generic.XRange != null),
                        "SpeedPanTiltSpace element doesn't contain mandatory space",
                        "Verifying of Speed Generic Pan/Tilt Space presence");

                    // verify that allowed range is specified
                    Assert(generic.XRange.Max >= generic.XRange.Min,
                        "Incorrect space range",
                        "Verifying of space range");

                    // configure and select a media profile 
                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    // check min range
                    PTZVector vector = new PTZVector();
                    vector.PanTilt = new Vector2D();

                    PTZSpeed speed = new PTZSpeed();
                    speed.PanTilt = new Vector2D();

                    Space2DDescription pantiltSpace = null;

                    Assert(!(null == options.Spaces.AbsolutePanTiltPositionSpace && null == options.Spaces.RelativePanTiltTranslationSpace),
                           "There are no options for Absolute command nor Relative move command in the selected PTZ configuration",
                           "Check there are options for Absolute command or Relative move command in selected PTZ configuration");

                    bool Absolute = true;
                    try
                    {
                        pantiltSpace = options.Spaces.AbsolutePanTiltPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absolutePanTiltSpace, true) == 0);

                        vector.PanTilt.space = pantiltSpace.URI;
                        vector.PanTilt.x = pantiltSpace.XRange.Min;
                        vector.PanTilt.y = pantiltSpace.YRange.Min;

                        speed.PanTilt.space = generic.URI;
                        speed.PanTilt.x = generic.XRange.Min;
                        speed.PanTilt.y = generic.XRange.Min;

                        AbsoluteMove(profile.token, vector, speed);
                    }
                    catch (System.Exception)
                    {
                        Absolute = false;
                        pantiltSpace = options.Spaces.RelativePanTiltTranslationSpace.FirstOrDefault(s => string.Compare(s.URI, _relativePanTiltSpace, true) == 0);

                        vector.PanTilt.space = pantiltSpace.URI;
                        vector.PanTilt.x = pantiltSpace.XRange.Min;
                        vector.PanTilt.y = pantiltSpace.YRange.Min;

                        RelativeMove(profile.token, vector, speed);
                    }

                    // check max range
                    vector.PanTilt.x = pantiltSpace.XRange.Max;
                    vector.PanTilt.y = pantiltSpace.YRange.Max;
                    speed.PanTilt.x = generic.XRange.Max;
                    speed.PanTilt.y = generic.XRange.Max;

                    if (Absolute)
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
            Order = "07.04.04",
            Id = "7-4-4",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService})]
        public void GenericZoomSpeedBis()
        {
            RunTest(() =>
            {
                PTZNode[] nodes = GetNodes();
                foreach (PTZNode node in nodes)
                {
                    // select node that supports SpeedZoomSpace
                    Space1DDescription[] spaces = node.SupportedPTZSpaces.ZoomSpeedSpace;
                    if (spaces != null)
                    {
                        LogTestEvent(string.Format("Node (token = {0}) supports Speed for Zoom move{1}", node.token, System.Environment.NewLine));
                    }
                    else
                    {
                        // if current node doesn't support SpeedZoomSpace 
                        // then check next node
                        continue;
                    }

                    // verify that SpeedZoomSpace contains Speed Generic Zoom Space
                    Space1DDescription generic =
                        spaces.FirstOrDefault(space => space.URI == _speedZoomSpace);

                    Assert((generic != null) && (generic.XRange != null),
                        "SpeedZoomSpace element doesn't contain mandatory space",
                        "Verifying of Speed Generic Zoom Space presence");

                    // verify that allowed range is specified
                    Assert(generic.XRange.Max >= generic.XRange.Min,
                        "Incorrect space range",
                        "Verifying of space range");

                    // configure and select a media profile 
                    PTZConfigurationOptions options;
                    Profile profile = GetPTZProfile(node, out options);
                    Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                    Assert(!(null == options.Spaces.AbsoluteZoomPositionSpace && null == options.Spaces.RelativeZoomTranslationSpace),
                           "There are no options for Absolute command nor Relative move command in the selected PTZ configuration",
                           "Check there are options for Absolute command or Relative move command in selected PTZ configuration");
                    // check max range
                    PTZVector vector = new PTZVector();
                    vector.Zoom = new Vector1D();
                    PTZSpeed speed = new PTZSpeed();
                    speed.Zoom = new Vector1D();

                    Space1DDescription zoomSpace = null;

                    bool Absolute = true;
                    try
                    {
                        //absolute or relative move should be supported - use generic space for position or translation
                        zoomSpace = options.Spaces.AbsoluteZoomPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absoluteZoomSpace, true) == 0);
                        vector.Zoom.space = zoomSpace.URI;
                        vector.Zoom.x = zoomSpace.XRange.Max;

                        speed.Zoom.space = generic.URI;
                        speed.Zoom.x = generic.XRange.Max;

                        AbsoluteMove(profile.token, vector, speed);
                    }
                    catch (System.Exception)
                    {
                        Absolute = false;
                        zoomSpace = options.Spaces.RelativeZoomTranslationSpace.FirstOrDefault(s => string.Compare(s.URI, _relativeZoomSpace, true) == 0);
                        vector.Zoom.space = zoomSpace.URI;
                        vector.Zoom.x = zoomSpace.XRange.Max;
                        RelativeMove(profile.token, vector, speed);
                    }

                    // check min range
                    vector.Zoom.x = zoomSpace.XRange.Min;
                    speed.Zoom.x = generic.XRange.Min;

                    if (Absolute)
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
