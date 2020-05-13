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
using System.ServiceModel;
using TestTool.Tests.Common.TestBase;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class PTZHomeAndAuxiliaryTestSuite : Base.PTZTest
    {
        private int _homeMoveTimeout = 30000;
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
            LastChangedIn = "v15.06",
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
                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

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
                    // correct PanTilt values according to PanTiltLimits field
                    if (profile.PTZConfiguration.PanTiltLimits != null)
                    {
                        vector.PanTilt.x = profile.PTZConfiguration.PanTiltLimits.Range.XRange.Min;
                        vector.PanTilt.y = profile.PTZConfiguration.PanTiltLimits.Range.YRange.Min;
                    }

                    // correct Zoom value according to ZoomLimits field
                    if (profile.PTZConfiguration.ZoomLimits != null)
                    {
                        vector.Zoom.x = profile.PTZConfiguration.ZoomLimits.Range.XRange.Min;
                    }

                    AbsoluteMove(profile.token, vector, null);
                }
                else
                {
                    RelativeMove(profile.token, vector, null);
                }
                
                RunStep(() => Thread.Sleep(OperationDelay), string.Format("Waiting {0} seconds for camera to move", OperationDelay/1000));

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
                    // correct PanTilt values according to PanTiltLimits field
                    if (profile.PTZConfiguration.PanTiltLimits != null)
                    {
                        vector.PanTilt.x = profile.PTZConfiguration.PanTiltLimits.Range.XRange.Max;
                        vector.PanTilt.y = profile.PTZConfiguration.PanTiltLimits.Range.YRange.Max;
                    }

                    // correct Zoom value according to ZoomLimits field
                    if (profile.PTZConfiguration.ZoomLimits != null)
                    {
                        vector.Zoom.x = profile.PTZConfiguration.ZoomLimits.Range.XRange.Max;
                    }

                    AbsoluteMove(profile.token, vector, null);
                }
                else
                {
                    RelativeMove(profile.token, vector, null);
                }
                
                RunStep(() => Thread.Sleep(OperationDelay), string.Format("Waiting {0} seconds for camera to move", OperationDelay/1000));

                GotoHomePosition(profile.token, null, _homeMoveTimeout);
                
                RunStep(() => Thread.Sleep(OperationDelay), string.Format("Waiting {0} seconds for camera to move", OperationDelay/1000));

                PTZStatus status = GetPTZStatus(profile.token);
                vector.PanTilt = oldPanTilt;
                vector.Zoom = oldZoom;
                if (status.Position != null)
                {
                    CheckPTZPosition(status.Position, vector, vector, pantiltSpace, zoomSpace);
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
            LastChangedIn = "v15.06",
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
                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

                GotoHomePosition(profile.token, null, OperationDelay);
                
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
                    // correct PanTilt values according to PanTiltLimits field
                    if (profile.PTZConfiguration.PanTiltLimits != null)
                    {
                        vector.PanTilt.x = profile.PTZConfiguration.PanTiltLimits.Range.XRange.Max;
                        vector.PanTilt.y = profile.PTZConfiguration.PanTiltLimits.Range.YRange.Max;
                    }

                    // correct Zoom value according to ZoomLimits field
                    if (profile.PTZConfiguration.ZoomLimits != null)
                    {
                        vector.Zoom.x = profile.PTZConfiguration.ZoomLimits.Range.XRange.Max;
                    }

                    // make sure current position is different to destination
                    if ((homePosition != null) && (EqualPositions(homePosition, vector)))
                    {
                        if (vector.PanTilt != null)
                        {
                            vector.PanTilt.x = profile.PTZConfiguration.PanTiltLimits.Range.XRange.Min;
                        }
                        else if (vector.Zoom != null)
                        {
                            vector.Zoom.x = profile.PTZConfiguration.ZoomLimits.Range.XRange.Min;
                        }
                    }

                    AbsoluteMove(profile.token, vector, null);
                }
                else
                {
                    RelativeMove(profile.token, vector, null);
                }

                RunStep(() => Thread.Sleep(OperationDelay), string.Format("Waiting {0} seconds for camera to move", OperationDelay/1000));

                SetFixedHomePosition(profile.token);

                GotoHomePosition(profile.token, null, OperationDelay);
                status = GetPTZStatus(profile.token);
                if (status.Position != null)
                {
                    CheckPTZPosition(status.Position, homePosition, homePosition, pantiltSpace, zoomSpace);
                }
            },
            () =>
            {
                CloseVideo();
            });
        }


        // Heineken scope test (December 2012)
        [Test(Name = "PTZ – HOME POSITION OPERATIONS (USAGE OF FIXEDHOMEPOSITION FLAG)",
            Path = "PTZ\\Home Position operations",
            Order = "05.01.03",
            Id = "5-1-3",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService, Feature.PTZAbsoluteOrRelative },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetHomePosition })]
        public void FixedHomeAttributeUsageTest()
        {
            RunTest(() =>
            {
                //5.	ONVIF Client configures and selects a media profile as described in Annex A.1.
                Profile profile = GetPTZProfile(_ptzNodeToken);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                
                //3.	ONVIF Client invokes GetNodeRequest message (Node Token) to get PTZ node capabilities.
                //4.	Verify the GetNodeResponse message from the DUT. If GetNodeResponse message 
                // does not contains FixedHomePosition attribute skip other steps and go to the next test.

                string nodeToken = profile.PTZConfiguration.NodeToken; 
                PTZNode node = GetNode(nodeToken);
                
                //6.	ONVIF Client invokes SetHomePositionRequest message (Profile Token) to get 
                // PTZ node capabilities.
                //7.	Verify the SetHomePositionResponse message or SOAP 1.2 fault message 
                // (Action/CannotOverwriteHome or ActionNotSupported) from the DUT. Verify that 
                // SetHomePositionResponse message was relieved if FixedHomePosition = “true”. Verify that 
                // SOAP 1.2 fault message (Action/CannotOverwriteHome or ActionNotSupported) was relieved 
                // if FixedHomePosition = “false”.
                
                if (node.FixedHomePositionSpecified)
                {
                    if (node.FixedHomePosition)
                    {
                        SetFixedHomePosition(profile.token);
                    }
                    else 
                    {
                        SetHomePosition(profile.token);
                    }              
                }
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
