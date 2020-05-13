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
using System.Threading;
namespace TestTool.Tests.TestCases.TestSuites
{
//#if FULL
    [TestClass]
//#endif
    public class PTZHomeAndAuxiliaryTestSuite: Base.PTZTest
    {
        private int _homeMoveTimeout = 30;
        public PTZHomeAndAuxiliaryTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        [Test(Name = "HOME POSITION OPERATIONS (CONFIGURABLE)",
            Path = "PTZ\\Home Position operations",
            Order = "10.05.01",
            Version = 1.02,
            Interactive = true,
            Services = new Service[] { Service.Device, Service.PTZ, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZConfigurableHome, Feature.PTZAbsoluteOrRelative })]
        public void ConfigurableHome()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Media.Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                
                string reason;
                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.token, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                //absolute or relative move should be supported - use generic space for position or translation
                bool absoulteMoveSupported = Features.Contains(Feature.PTZAbsolute);
                PTZVector vector = new PTZVector();

                Space2DDescription pantiltSpace = null;
                Space1DDescription zoomSpace = null;

                if (Features.Contains(Feature.PTZAbsolutePanTilt) || Features.Contains(Feature.PTZRelativePanTilt))
                {
                    pantiltSpace = absoulteMoveSupported ? options.Spaces.AbsolutePanTiltPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absolutePanTiltSpace, true) == 0) :
                        options.Spaces.RelativePanTiltTranslationSpace.FirstOrDefault(s => string.Compare(s.URI, _relativePanTiltSpace, true) == 0);

                    vector.PanTilt = new Vector2D();
                    vector.PanTilt.space = pantiltSpace.URI;
                    vector.PanTilt.x = pantiltSpace.XRange.Min;
                    vector.PanTilt.y = pantiltSpace.YRange.Min;
                }
                if (Features.Contains(Feature.PTZAbsoluteZoom) || Features.Contains(Feature.PTZRelativeZoom))
                {
                    zoomSpace = absoulteMoveSupported ? options.Spaces.AbsoluteZoomPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absoluteZoomSpace, true) == 0) :
                        options.Spaces.RelativeZoomTranslationSpace.FirstOrDefault(s => string.Compare(s.URI, _relativeZoomSpace, true) == 0);

                    vector.Zoom = new Vector1D();
                    vector.Zoom.space = zoomSpace.URI;
                    vector.Zoom.x = zoomSpace.XRange.Min;
                }

                if (absoulteMoveSupported)
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
                if (absoulteMoveSupported)
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
                else
                {
                    OpenVideo();

                    Assert(_operator.GetYesNoAnswer(string.Format("Is camera in position [{0}]?", PositionToString(vector))),
                        "Camera is in wrong position",
                        "Camera position check (manual)");
                    CloseVideo();
                }
            },
            () =>
            {
                CloseVideo();
            });
        }

        [Test(Name = "HOME POSITION OPERATIONS (FIXED)",
            Interactive = true,
            Path = "PTZ\\Home Position operations",
            Order = "10.05.02",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZFixedHome, Feature.PTZAbsoluteOrRelative })]
        public void FixedHome()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Media.Profile profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                
                string reason;
                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.token, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                GotoHomePosition(profile.token, null, _homeMoveTimeout);

                //absolute or relative move should be supported - use generic space for position or translation
                bool absoulteMoveSupported = Features.Contains(Feature.PTZAbsolute);
                PTZVector vector = new PTZVector();
                Space2DDescription pantiltSpace = null;
                Space1DDescription zoomSpace = null;

                if (Features.Contains(Feature.PTZAbsolutePanTilt) || Features.Contains(Feature.PTZRelativePanTilt))
                {
                    pantiltSpace = absoulteMoveSupported ? options.Spaces.AbsolutePanTiltPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absolutePanTiltSpace, true) == 0) :
                        options.Spaces.RelativePanTiltTranslationSpace.FirstOrDefault(s => string.Compare(s.URI, _relativePanTiltSpace, true) == 0);

                    vector.PanTilt = new Vector2D();
                    vector.PanTilt.space = pantiltSpace.URI;
                    vector.PanTilt.x = pantiltSpace.XRange.Max;
                    vector.PanTilt.y = pantiltSpace.YRange.Max;
                }
                if (Features.Contains(Feature.PTZAbsoluteZoom) || Features.Contains(Feature.PTZRelativeZoom))
                {
                    zoomSpace = absoulteMoveSupported ? options.Spaces.AbsoluteZoomPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absoluteZoomSpace, true) == 0) :
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
                if (absoulteMoveSupported)
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
                else
                {
                    OpenVideo();
                    Assert(_operator.GetYesNoAnswer(string.Format("Is camera in position [{0}]?", PositionToString(homePosition))),
                        "Camera is in wrong position",
                        "Camera position check (manual)");
                    CloseVideo();
                }
            },
            () =>
            {
                CloseVideo();
            });
        }
        
        [Test(Name = "SEND AUXILIARY COMMAND",
            Path = "PTZ\\Auxiliary operations",
            Order = "10.06.01",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZAuxiliary })]
        public void AuxiliaryCommand()
        {
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                Media.Profile profile = GetPTZProfile(_ptzNodeToken, out options);
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
