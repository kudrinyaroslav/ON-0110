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
    public class PTZHomeAndAuxiliaryTestSuite : Base.PTZTest
    {
        private int _homeMoveTimeout = 30;
        public PTZHomeAndAuxiliaryTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        [Test(Name = "HOME POSITION OPERATIONS (CONFIGURABLE)",
            Path = "PTZ\\Home Position operations",
            Order = "05.01.01",
            Id = "5-1-1",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService, Feature.PTZConfigurableHome, Feature.PTZAbsoluteOrRelative },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetHomePosition, Functionality.GotoHomePosition, Functionality.PtzGetStatus })]
        public void ConfigurableHome()
        {
            RunTest(() =>
            {
                Assert(Features.ContainsFeature(Feature.PTZAbsoluteOrRelative),
                       "No Absolute or Relative movement is supported",
                       "Check that Absolute or Relative movement is supported");
                
                PTZConfigurationOptions options;
                Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                
                string reason;
                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.token, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                //absolute or relative move should be supported - use generic space for position or translation
                bool absoluteMoveSupported = Features.Contains(Feature.PTZAbsolute);
                
                Space2DDescription pantiltSpace = null;
                Space1DDescription zoomSpace = null;

                // pantilt movement is used if corresponding Pan/Tilt is supported
                bool panTilt = (absoluteMoveSupported && Features.ContainsFeature(Feature.PTZAbsolutePanTilt)) ||
                               (!absoluteMoveSupported && Features.ContainsFeature(Feature.PTZRelativePanTilt));

                bool absoluteZoom = absoluteMoveSupported && Features.Contains(Feature.PTZAbsoluteZoom);
                bool relativeZoom = !absoluteMoveSupported && Features.Contains(Feature.PTZRelativeZoom);

                bool zoom = absoluteZoom || relativeZoom;

                PTZVector vector = new PTZVector();

                if (panTilt)
                {
                    pantiltSpace = absoluteMoveSupported ? options.Spaces.AbsolutePanTiltPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absolutePanTiltSpace, true) == 0) :
                        options.Spaces.RelativePanTiltTranslationSpace.FirstOrDefault(s => string.Compare(s.URI, _relativePanTiltSpace, true) == 0);

                    vector.PanTilt = new Vector2D();
                    vector.PanTilt.space = pantiltSpace.URI;
                    vector.PanTilt.x = pantiltSpace.XRange.Min;
                    vector.PanTilt.y = pantiltSpace.YRange.Min;
                }

                if (zoom)
                {
                    zoomSpace = absoluteZoom ? options.Spaces.AbsoluteZoomPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absoluteZoomSpace, true) == 0) :
                        options.Spaces.RelativeZoomTranslationSpace.FirstOrDefault(s => string.Compare(s.URI, _relativeZoomSpace, true) == 0);

                    vector.Zoom = new Vector1D();
                    vector.Zoom.space = zoomSpace.URI;
                    vector.Zoom.x = zoomSpace.XRange.Min;
                }
                if (absoluteMoveSupported)
                {
                    AbsoluteMove(profile.token, vector, null);
                }
                else
                {
                    RelativeMove(profile.token, vector, null);
                }
                RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                SetHomePosition(profile.token);

                //move to another position
                Vector2D oldPanTilt = vector.PanTilt;
                if (oldPanTilt != null)
                {
                    vector.PanTilt = new Vector2D();
                    vector.PanTilt.space = oldPanTilt.space;
                    vector.PanTilt.x = pantiltSpace.XRange.Max;
                    vector.PanTilt.y = pantiltSpace.YRange.Max;
                }
                Vector1D oldZoom = vector.Zoom;
                if (oldZoom != null)
                {
                    vector.Zoom = new Vector1D();
                    vector.Zoom.space = oldZoom.space;
                    vector.Zoom.x = zoomSpace.XRange.Max;
                }
                if (absoluteMoveSupported)
                {
                    AbsoluteMove(profile.token, vector, null);
                }
                else
                {
                    RelativeMove(profile.token, vector, null);
                }
                RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                GotoHomePosition(profile.token, null, _homeMoveTimeout);
                RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                PTZStatus status = GetPTZStatus(profile.token);
                vector.PanTilt = oldPanTilt;
                vector.Zoom = oldZoom;
                if (status.Position != null)
                {
                    CheckPTZPosition(status.Position, vector, vector);
                }
            },
            () =>
            {
                CloseVideo();
            });
        }

        [Test(Name = "HOME POSITION OPERATIONS (FIXED)",
            Path = "PTZ\\Home Position operations",
            Order = "05.01.02",
            Id = "5-1-2",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService, Feature.PTZFixedHome, Feature.PTZAbsoluteOrRelative },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetHomePosition, Functionality.GotoHomePosition, Functionality.PtzGetStatus })]
        public void FixedHome()
        {
            RunTest(() =>
            {
                Assert(Features.ContainsFeature(Feature.PTZAbsoluteOrRelative),
                       "No Absolute or Relative movement is supported",
                       "Check that Absolute or Relative movement is supported");

                PTZConfigurationOptions options;
                Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                
                string reason;
                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.token, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                GotoHomePosition(profile.token, null, _homeMoveTimeout);
                
                //absolute or relative move should be supported - use generic space for position or translation
                bool absoluteMoveSupported = Features.Contains(Feature.PTZAbsolute);

                // pantilt movement is used if correcponding Pan/Tilt is supported
                bool panTilt = (absoluteMoveSupported && Features.ContainsFeature(Feature.PTZAbsolutePanTilt)) ||
                               (!absoluteMoveSupported && Features.ContainsFeature(Feature.PTZRelativePanTilt));

                bool absoluteZoom = absoluteMoveSupported && Features.Contains(Feature.PTZAbsoluteZoom);
                bool relativeZoom = !absoluteMoveSupported && Features.Contains(Feature.PTZRelativeZoom);

                bool zoom = absoluteZoom || relativeZoom;
               
                
                PTZVector vector = new PTZVector();
                Space2DDescription pantiltSpace = null;
                Space1DDescription zoomSpace = null;

                if (panTilt)
                {
                    pantiltSpace = absoluteMoveSupported ? options.Spaces.AbsolutePanTiltPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absolutePanTiltSpace, true) == 0) :
                        options.Spaces.RelativePanTiltTranslationSpace.FirstOrDefault(s => string.Compare(s.URI, _relativePanTiltSpace, true) == 0);

                    vector.PanTilt = new Vector2D();
                    vector.PanTilt.space = pantiltSpace.URI;
                    vector.PanTilt.x = pantiltSpace.XRange.Max;
                    vector.PanTilt.y = pantiltSpace.YRange.Max;
                }

                if (zoom)
                {
                    zoomSpace = absoluteZoom ? options.Spaces.AbsoluteZoomPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absoluteZoomSpace, true) == 0) :
                        options.Spaces.RelativeZoomTranslationSpace.FirstOrDefault(s => string.Compare(s.URI, _relativeZoomSpace, true) == 0);

                    vector.Zoom = new Vector1D();
                    vector.Zoom.space = zoomSpace.URI;
                    vector.Zoom.x = zoomSpace.XRange.Max;
                }
                
                //make sure current position is different to destination
                PTZStatus status = GetPTZStatus(profile.token);
                PTZVector homePosition = status.Position;
                if ((homePosition != null) && (EqualPositions(homePosition, vector)))
                {
                    if(vector.PanTilt != null)
                    {
                        vector.PanTilt.x = 0;
                    }
                    else if(vector.Zoom != null)
                    {
                        vector.Zoom.x = 0;
                    }
                }
                if (absoluteMoveSupported)
                {
                    AbsoluteMove(profile.token, vector, null);
                }
                else
                {
                    RelativeMove(profile.token, vector, null);
                }

                SetFixedHomePosition(profile.token);

                GotoHomePosition(profile.token, null, _homeMoveTimeout);
                status = GetPTZStatus(profile.token);
                if (status.Position != null)
                {
                    CheckPTZPosition(status.Position, homePosition, homePosition);
                }
            },
            () =>
            {
                CloseVideo();
            });
        }
        
        [Test(Name = "SEND AUXILIARY COMMAND",
            Path = "PTZ\\Auxiliary operations",
            Order = "06.01.01",
            Id = "6-1-1",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService, Feature.PTZAuxiliary },
            FunctionalityUnderTest = new Functionality[] { Functionality.SendAuxiliaryCommand })]
        public void AuxiliaryCommand()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                PTZNode node = GetNode(profile.PTZConfiguration.NodeToken);

                Assert((node.AuxiliaryCommands != null) && (node.AuxiliaryCommands.Length > 0),
                    string.Format("Node [token={0}] does not contain auxiliary commands", node.token),
                    "Checking auxiliary commands list");

                foreach (string command in node.AuxiliaryCommands)
                {
                    SendAuxiliaryCommnad(profile.token, command);
                }
            });
        }
    }
}
