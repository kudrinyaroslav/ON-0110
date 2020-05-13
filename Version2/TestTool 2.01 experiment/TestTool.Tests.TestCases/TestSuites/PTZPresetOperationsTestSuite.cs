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
    public class PTZPresetOperationsTestSuite : Base.PTZTest
    {
        public PTZPresetOperationsTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        [Test(Name = "SET AND GET PRESET",
            Path = "PTZ\\Preset operations",
            Order = "10.04.01",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZPresets, Feature.PTZAbsoluteOrRelative })]
        public void SetGetPreset()
        {
            string presetToken = null;
            Media.Profile profile = null;
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                profile = GetPTZProfile(_ptzNodeToken, out options);
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
                    vector.PanTilt.x = pantiltSpace.XRange.Min / 2;
                    vector.PanTilt.y = pantiltSpace.YRange.Min / 2;
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


                string presetName = "Test";
                presetToken = SetPreset(profile.token, presetName, null);
                PTZPreset[] presets = GetPresets(profile.token);
                CheckPreset(presets, presetToken, presetName, vector);

                //move to another position
                if (vector.PanTilt != null)
                {
                    vector.PanTilt.x = pantiltSpace.XRange.Max / 2;
                    vector.PanTilt.y = pantiltSpace.YRange.Max / 2;
                }
                if (vector.Zoom != null)
                {
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

                SetPreset(profile.token, presetName, presetToken);
                presets = GetPresets(profile.token);

                if (!absoulteMoveSupported && (vector.PanTilt != null))
                {
                    //in case of relative move consider camera should return to 0,0
                    vector.PanTilt.x = 0;
                    vector.PanTilt.y = 0;
                }
                CheckPreset(presets, presetToken, presetName, vector);

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
            Interactive = true,
            Path = "PTZ\\Preset operations",
            Order = "10.04.02",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZPresets, Feature.PTZAbsoluteOrRelative })]
        public void GotoPreset()
        {
            string presetToken = null;
            Media.Profile profile = null;
            RunTest(() =>
            {
                PTZConfigurationOptions options;
                profile = GetPTZProfile(_ptzNodeToken, out options);
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
                if (absoulteMoveSupported)
                {
                    AbsoluteMove(profile.token, vector, null);
                }
                else
                {
                    RelativeMove(profile.token, vector, null);
                }
                RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                GotoPreset(profile.token, presetToken, null);
                RunStep(() => { Thread.Sleep(10000); }, string.Format("Waiting 10 seconds for camera to move"));

                PTZStatus status = GetPTZStatus(profile.token);
                vector.PanTilt = oldPanTilt;
                vector.Zoom = oldZoom;
                if(status.Position != null)
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
            Order = "10.04.03",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.PTZ, Service.Media },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ, Feature.PTZPresets })]
        public void RemovePreset()
        {
            string presetToken = null;
            Media.Profile profile = null;
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
                CheckPreset(presets, presetToken, presetName, null);

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
