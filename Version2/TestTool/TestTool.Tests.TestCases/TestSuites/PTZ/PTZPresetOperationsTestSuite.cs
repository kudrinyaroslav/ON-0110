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
    public class PTZPresetOperationsTestSuite : Base.PTZTest
    {
        public PTZPresetOperationsTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        [Test(Name = "SET AND GET PRESET",
            Path = "PTZ\\Preset operations",
            Order = "04.01.04",
            Id = "4-1-4",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            LastChangedIn = "v15.06",
            RequiredFeatures = new Feature[] { Feature.PTZService, Feature.PTZPresets, Feature.PTZAbsoluteOrRelative },
            FunctionalityUnderTest = new Functionality[] { Functionality.PtzGetPreset, Functionality.PtzSetPreset })]
        public void SetGetPreset()
        {
            string presetToken = null;
            Profile profile = null;
            RunTest(() =>
            {
                Assert(Features.ContainsFeature(Feature.PTZAbsoluteOrRelative), 
                       "No Absolute or Relative movement is supported", 
                       "Check that Absolute or Relative movement is supported");

                PTZConfigurationOptions options;
                profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);
                string reason;
                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);
                //absolute or relative move should be supported - use generic space for position or translation
                bool absoluteMoveSupported = Features.Contains(Feature.PTZAbsolute);
                
                PTZVector vector = new PTZVector();

                Space2DDescription pantiltSpace = null;
                Space1DDescription zoomSpace = null;

                // pantilt movement is used if corresponding Pan/Tilt is supported
                bool absolutePanTilt = absoluteMoveSupported && Features.Contains(Feature.PTZAbsolutePanTilt);
                bool relativePanTilt = !absoluteMoveSupported && Features.Contains(Feature.PTZRelativePanTilt);

                bool panTilt = absolutePanTilt || relativePanTilt;

                bool absoluteZoom = absoluteMoveSupported && Features.Contains(Feature.PTZAbsoluteZoom);
                bool relativeZoom = !absoluteMoveSupported && Features.Contains(Feature.PTZRelativeZoom);

                bool zoom = absoluteZoom || relativeZoom;

                if (panTilt)
                {
                    pantiltSpace = absoluteMoveSupported ? options.Spaces.AbsolutePanTiltPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absolutePanTiltSpace, true) == 0) :
                        options.Spaces.RelativePanTiltTranslationSpace.FirstOrDefault(s => string.Compare(s.URI, _relativePanTiltSpace, true) == 0);
                    vector.PanTilt = new Vector2D();
                    vector.PanTilt.space = pantiltSpace.URI;
                    vector.PanTilt.x = pantiltSpace.XRange.Min + (pantiltSpace.XRange.Max - pantiltSpace.XRange.Min) / 3;
                    vector.PanTilt.y = pantiltSpace.YRange.Min + (pantiltSpace.YRange.Max - pantiltSpace.YRange.Min) / 3;
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
                        var range = profile.PTZConfiguration.PanTiltLimits.Range;
                        vector.PanTilt.x = range.XRange.Min + (range.XRange.Max - range.XRange.Min) / 3;
                        vector.PanTilt.y = range.YRange.Min + (range.YRange.Max - range.YRange.Min) / 3;
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


                string presetName = "Test";
                presetToken = SetPreset(profile.token, presetName, null);
                PTZPreset[] presets = GetPresets(profile.token);
                CheckPreset(presets, presetToken, presetName, vector, pantiltSpace, zoomSpace, absolutePanTilt, absoluteZoom);

                //move to another position
                if (vector.PanTilt != null)
                {
                    vector.PanTilt.x = pantiltSpace.XRange.Max - (pantiltSpace.XRange.Max - pantiltSpace.XRange.Min) / 3;
                    vector.PanTilt.y = pantiltSpace.YRange.Max - (pantiltSpace.YRange.Max - pantiltSpace.YRange.Min) / 3;
                }
                if (vector.Zoom != null)
                {
                    vector.Zoom.x = zoomSpace.XRange.Max;
                }
                if (absoluteMoveSupported)
                {
                    // correct PanTilt values according to PanTiltLimits field
                    if (profile.PTZConfiguration.PanTiltLimits != null)
                    {
                        var range = profile.PTZConfiguration.PanTiltLimits.Range;
                        vector.PanTilt.x = range.XRange.Max - (range.XRange.Max - range.XRange.Min) / 3;
                        vector.PanTilt.y = range.YRange.Max - (range.YRange.Max - range.YRange.Min) / 3;
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

                SetPreset(profile.token, presetName, presetToken);
                presets = GetPresets(profile.token);

                if (!absoluteMoveSupported && (vector.PanTilt != null))
                {
                    //in case of relative move consider camera should return to 0,0
                    vector.PanTilt.x = 0;
                    vector.PanTilt.y = 0;
                }
                CheckPreset(presets, presetToken, presetName, vector, pantiltSpace, zoomSpace, absolutePanTilt, absoluteZoom);

                RemovePreset(profile.token, presetToken);
                presetToken = null;
            },
            () =>
            {
                if ((presetToken != null) && (profile != null))
                {
                    RemovePreset(profile.token, presetToken);
                }
            });
        }

        [Test(Name = "GOTO PRESET",
            Path = "PTZ\\Preset operations",
            Order = "04.01.05",
            Id = "4-1-5",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            LastChangedIn = "v15.06",
            RequiredFeatures = new Feature[] { Feature.PTZService, Feature.PTZPresets, Feature.PTZAbsoluteOrRelative },
            FunctionalityUnderTest = new Functionality[] { Functionality.PtzGotoPreset, Functionality.PtzGetStatus })]
        public void GotoPreset()
        {
            string presetToken = null;
            Profile profile = null;
            RunTest(() =>
            {
                Assert(Features.ContainsFeature(Feature.PTZAbsoluteOrRelative),
                       "No Absolute or Relative movement is supported",
                       "Check that Absolute or Relative movement is supported");

                PTZConfigurationOptions options;
                profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                string reason;
                Assert(ValidatePTZConfigurationOptions(options, profile.PTZConfiguration.NodeToken, out reason), reason, Resources.StepValidatePTZConfigOptions_Title);

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
                    vector.PanTilt.x = pantiltSpace.XRange.Min;
                    vector.PanTilt.y = pantiltSpace.YRange.Min;
                }
                if (zoom)
                {
                    zoomSpace = absoluteMoveSupported ? options.Spaces.AbsoluteZoomPositionSpace.FirstOrDefault(s => string.Compare(s.URI, _absoluteZoomSpace, true) == 0) :
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

                string presetName = "Test";
                presetToken = SetPreset(profile.token, presetName, null);

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
                      if (profile.PTZConfiguration.ZoomLimits.Range != null &&
                          profile.PTZConfiguration.ZoomLimits.Range.URI != null &&
                          vector.Zoom.space != null &&
                          profile.PTZConfiguration.ZoomLimits.Range.URI == vector.Zoom.space)
                      {
                        vector.Zoom.x = profile.PTZConfiguration.ZoomLimits.Range.XRange.Max;
                      }
                    }

                    AbsoluteMove(profile.token, vector, null);
                }
                else
                {
                    RelativeMove(profile.token, vector, null);
                }
                RunStep(() => Thread.Sleep(OperationDelay), string.Format("Waiting {0} seconds for camera to move", OperationDelay/1000));

                GotoPreset(profile.token, presetToken, null);
                RunStep(() => Thread.Sleep(OperationDelay), string.Format("Waiting {0} seconds for camera to move", OperationDelay/1000));

                PTZStatus status = GetPTZStatus(profile.token);
                vector.PanTilt = oldPanTilt;
                vector.Zoom = oldZoom;
                if(status.Position != null)
                {
                    CheckPTZPosition(status.Position, vector, vector, pantiltSpace, zoomSpace);
                }

                RemovePreset(profile.token, presetToken);
                presetToken = null;
            },
            () =>
            {
                CloseVideo();

                if ((presetToken != null) && (profile != null))
                {
                    RemovePreset(profile.token, presetToken);
                }
            });
        }

        [Test(Name = "REMOVE PRESET",
            Path = "PTZ\\Preset operations",
            Order = "04.01.06",
            Id = "4-1-6",
            Category = Category.PTZ,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.PTZService, Feature.PTZPresets },
            FunctionalityUnderTest = new Functionality[]{Functionality.PtzRemovePreset})]
        public void RemovePreset()
        {
            string presetToken = null;
            Profile profile = null;
            bool presetAdded = false;
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                profile = GetPTZProfile(_ptzNodeToken, out options);
                Assert((profile != null) && (profile.PTZConfiguration != null), Resources.ErrorNoPTZProfile, Resources.StepValidatePTZProfile);

                string presetName = "Test";
                presetToken = SetPreset(profile.token, presetName, null);
                presetAdded = true;
                PTZPreset[] presets = GetPresets(profile.token);
                CheckPreset(presets, presetToken, presetName, null, null, null, false, false);

                RemovePreset(profile.token, presetToken);
                presetAdded = false;
                presets = GetPresets(profile.token);
                CheckNoPreset(presets, presetToken);
            },
            () =>
            {
                if (presetAdded)
                {
                    RemovePreset(profile.token, presetToken);
                }
            });
        }
    }
}
