using System.Linq;
using System.Text;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Onvif;
using System.ServiceModel;
using TestTool.Tests.TestCases.Utils;

namespace TestTool.Tests.TestCases.TestSuites
{
    //[TestClass]
    public class ImagingTestSuite : ImagingTest
    {

        public ImagingTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Imaging";

        [Test(Name = "IMAGING COMMAND GETIMAGESETTINGS",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[]{Feature.Imaging})]
        public void GetImagingSettingsTest()
        {
            RunTest( 
                () =>
                    {
                        VideoSource[] sources = GetAndValidateVideoSources();

                        for (int i = 0; i < sources.Length; i++)
                        {
                            ImagingSettings20 settings = null;
                            settings = GetImagingSettings(sources[i].token);
                            
                            //if (succeeded)
                            //{
                            //    ValidateImagingSettings(sources[i].Imaging, settings);
                            //}
                        }
                    });
        }

        [Test(Name = "IMAGING COMMAND GETIMAGESETTINGS – INVALID VIDEOSOURCETOKEN",
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[]{Feature.Imaging})]
        public void GetImagingSettingsInvalidTokenTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    string[] tokens = sources.Select(s => s.token).ToArray();

                    string invalidToken = tokens.GetNonMatchingString();

                    RunStep(
                            () =>
                            {
                                ImagingSettings20 settings = Client.GetImagingSettings(invalidToken);
                            }, "Get imaging settings - negative test",
                            new ValidateTypeFault(ValidateInvalidTokenFault));

                    DoRequestDelay();
                });
        }

        [Test(Name = "IMAGING COMMAND GETOPTIONS",
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void GetImagingOptionsTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        ImagingOptions20 options = null;
                        options = GetOptions(sources[i].token);

                        Assert(options != null, "No imaging options received", "Check if the DUT sent imaging options");

                        ValidateImagingOptions(options);
                    }
                });
        }
        
        [Test(Name = "IMAGING COMMAND GETOPTIONS – INVALID VIDEOSOURCETOKEN",
            Order = "01.01.04",
            Id = "1-1-4",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void GetOptionsInvalidTokenTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    string[] tokens = sources.Select(s => s.token).ToArray();

                    string invalidToken = tokens.GetNonMatchingString();

                    RunStep(
                            () =>
                            {
                                ImagingOptions20 options = Client.GetOptions(invalidToken);
                            }, "Get options - negative test",
                            new ValidateTypeFault(ValidateInvalidTokenFault));

                    DoRequestDelay();

                });
        }

        /*
        [Test(Name = "IMAGING COMMAND SETIMAGINGSETTINGS AND GETOPTIONS SUPPORT",
            Order = "01.01.05",
            Id = "1-1-5",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void SetImagingSettingsGetOptionsSupportTest()
        {
            string token = string.Empty;
            ImagingSettings20 settingsToRestore = null;

            RunTest(
                () =>
                    {
                        VideoSource[] sources = GetAndValidateVideoSources();

                        for (int i = 0; i < sources.Length; i++)
                        {
                            ImagingOptions20 options = null;
                            bool ok = RunStepHandleNotSupported(() =>
                                                                    {
                                                                        options = Client.GetOptions(sources[i].token);
                                                                    }, "Get options");

                            if (!ok)
                            {
                                ImagingSettings20 settings = GetImagingSettings(sources[i].token);
                                RunStepHandleNotSupported(
                                    () =>
                                        {
                                            Client.SetImagingSettings(sources[i].token, settings, false);
                                        }, 
                                    "Set imaging Settings",
                                    new ValidateTypeFault(
                                        (FaultException fault, out string reason) =>
                                            {
                                                bool faultOk = false;
                                                faultOk = fault.IsValidOnvifFault("Sender/InvalidArgVal/NoImagingForSource",
                                                                             out reason);
                                                return faultOk;
                                            }),
                                    new ValidateNoFault((out string reason) =>
                                                            {
                                                                settingsToRestore = settings;
                                                                token = sources[i].token;
                                                                reason =
                                                                    "SetImagingSettings succeeded, while GetOptions failed";
                                                                return false;
                                                            }));
                            }
                        }
                    },
                    () =>
                        {
                            if (settingsToRestore != null)
                            {
                                SetImagingSettings(token, settingsToRestore, false);
                            }
                        });
        }
        */

        [Test(Name = "IMAGING COMMAND SETIMAGESETTINGS",
            Order = "01.01.06",
            Id = "1-1-6",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void SetImagingSettingsTest()
        {
            ImagingSettings20 backup = null;
            string token = string.Empty;

            RunTest(
                () =>
                    {
                        VideoSource[] sources = GetAndValidateVideoSources();

                        for (int i = 0; i < sources.Length; i++)
                        {
                            ImagingOptions20 options = GetOptions(sources[i].token);

                            if (options == null)
                            {
                                continue;
                            }

                            ValidateImagingOptions(options);

                            ImagingSettings20 initialSettings = GetImagingSettings(sources[i].token);

                            ImagingSettings20 settings = GenerateImagingSettings(options, initialSettings);

                            SetImagingSettings(sources[i].token, settings, false);

                            ImagingSettings20 actualSettings = GetImagingSettings(sources[i].token);

                            // compare settings
                            string dump;
                            bool bEqual = ImagingSettingsEqual(settings, actualSettings, out dump);
                            if (!bEqual)
                            {
                                // save for restoring
                                // if equal, restoring will be done after passing Assert
                                backup = initialSettings;
                                token = sources[i].token;
                            }
                            Assert(bEqual,
                                   string.Format("Expected and actual settings are different.\n{0}",
                                                 dump), "Compare settings");

                            SetImagingSettings(sources[i].token, initialSettings, false, "Restore imaging settings");

                        }
                    }, 
                    () =>
                        {
                            if (backup != null)
                            {
                                SetImagingSettings(token, backup, false, "Restore imaging settings");
                            }
                        });
        }

        [Test(Name = "IMAGING COMMAND SETIMAGESETTINGS – INVALID VIDEOSOURCETOKEN",
            Order = "01.01.07",
            Id = "1-1-7",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void SetImagingSettingsInvalidTokenTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    string[] tokens = sources.Select(s => s.token).ToArray();

                    string invalidToken = tokens.GetNonMatchingString();

                    RunStep(
                            () =>
                            {
                                ImagingSettings20 settings = new ImagingSettings20();
                                Client.SetImagingSettings(invalidToken, settings, false);
                            }, 
                            "Set imaging settings - negative test",
                            new ValidateTypeFault(ValidateInvalidTokenFault));

                    DoRequestDelay();

                });
        }
        
        [Test(Name = "IMAGING COMMAND SETIMAGESETTINGS – INVALID SETTINGS",
            Order = "01.01.08",
            Id = "1-1-8",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void SetImagingSettingsInvalidSettingsTest()
        {
            ImagingSettings20 backup = null;
            string token = string.Empty;
            
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        ImagingOptions20 options = GetOptions(sources[i].token);

                        Assert(options != null, "No imaging options received", "Check if the DUT sent imaging options");

                        ImagingSettings20 initialSettings = GetImagingSettings(sources[i].token);
                        
                        Assert(initialSettings != null, "No initial settings received", "Check if the DUT sent imaging settings");
                        
                        ValidateImagingOptions(options);
                        ImagingSettings20 settings = GenerateInvalidImagingSettings(options);

                        RunStep(
                            () =>
                            {
                                Client.SetImagingSettings(sources[i].token, settings, false);
                            }, "Set imaging settings", 
                                new ValidateTypeFault( 
                                    (FaultException fault, out string reason) 
                                        =>
                                        {
                                            if (fault == null)
                                            {
                                                reason = "No fault received";
                                                return false;
                                            }

                                            // SettingsInvalid 
                                            return fault.IsValidOnvifFault("Sender/InvalidArgVal/SettingsInvalid", out reason);
                                        }));

                        DoRequestDelay();

                        ImagingSettings20 actualSettings = GetImagingSettings(sources[i].token);

                        Assert(actualSettings != null, "No current settings received", "Check if the DUT sent imaging settings");

                        string dump;
                        bool bEqual = ImagingSettingsEqual(initialSettings, actualSettings, out dump);
                        if (!bEqual)
                        {
                            // save for restoring
                            backup = initialSettings;
                            token = sources[i].token;
                        }
                        Assert(bEqual,
                               string.Format("Expected and actual settings are different.\n{0}",
                                             dump), "Check that settings have not been changed");


                    }
                },
                () =>
                {
                    if (backup != null)
                    {
                        SetImagingSettings(token, backup, false, "Restore imaging settings");
                    }
                });
        }

        [Test(Name = "IMAGING COMMAND GETMOVEOPTIONS",
            Order = "01.01.09",
            Id = "1-1-9",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void GetMoveOptionsTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        MoveOptions20 options = null;
                        options = GetMoveOptions(sources[i].token);

                        if (options != null)
                        {
                            ValidateMoveOptions(options);
                        }
                    }
                });
        }

        [Test(Name = "IMAGING COMMAND GETMOVEOPTIONS – INVALID VIDEOSOURCETOKEN",
            Order = "01.01.10",
            Id = "1-1-10",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void GetMoveOptionsInvalidTokenTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    string[] tokens = sources.Select(s => s.token).ToArray();

                    string invalidToken = tokens.GetNonMatchingString();

                    RunStep(
                            () =>
                            {
                                MoveOptions20 options = Client.GetMoveOptions(invalidToken);
                            }, "Get options - negative test",
                            new ValidateTypeFault(ValidateInvalidTokenFault));

                    DoRequestDelay();

                });
        }
        
        [Test(Name = "IMAGING COMMAND ABSOLUTE MOVE",
            Order = "01.01.11",
            Id = "1-1-11",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void ImagingCommandAbsoluteMoveTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        MoveOptions20 options = null;
                        options = GetMoveOptions(sources[i].token);

                        if (options != null)
                        {
                            ValidateMoveOptions(options);

                            bool absoluteSupported;

                            BeginStep(string.Format("Check if Absolute Move is supported for video source '{0}'", sources[i].token));
                            absoluteSupported = (options.Absolute != null);
                            if (absoluteSupported)
                            {
                                LogStepEvent("Supported");
                            }
                            else
                            {
                                LogStepEvent("Not supported");
                            }
                            StepPassed();

                            if (absoluteSupported)
                            {
                                float position = (2.0F * options.Absolute.Position.Min / 3.0F + options.Absolute.Position.Max /3.0F);
                                FocusMove focus = new FocusMove();
                                focus.Absolute = new AbsoluteFocus();
                                focus.Absolute.Position = position;

                                Move(sources[i].token, focus);

                                BeginStep(string.Format("Check if Absolute Move Speed is supported for video source '{0}'", sources[i].token));
                                bool supported = (options.Absolute.Speed != null);
                                if (supported)
                                {
                                    LogStepEvent("Supported");
                                }
                                else
                                {
                                    LogStepEvent("Not supported");
                                }
                                StepPassed();

                                if (supported)
                                {
                                    position = position = ( options.Absolute.Position.Min / 3.0F + 2.0F *options.Absolute.Position.Max / 3.0F);
                                    focus.Absolute.Position = position;

                                    focus.Absolute.SpeedSpecified = true;
                                    focus.Absolute.Speed = options.Absolute.Speed.Average();

                                    Move(sources[i].token, focus);

                                }

                            }
                        }
                    }

                });
        }

        [Test(Name = "IMAGING COMMAND ABSOLUTE MOVE – INVALID SETTINGS",
            Order = "01.01.12",
            Id = "1-1-12",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void ImagingCommandAbsoluteMoveInvalidSettingsTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        MoveOptions20 options = null;
                        options = GetMoveOptions(sources[i].token);

                        if (options != null)
                        {
                            ValidateMoveOptions(options);

                            bool absoluteSupported;

                            BeginStep(string.Format("Check if Absolute Move is supported for video source '{0}'", sources[i].token));
                            absoluteSupported = (options.Absolute != null);
                            if (absoluteSupported)
                            {
                                LogStepEvent("Supported");
                            }
                            else
                            {
                                LogStepEvent("Not supported");
                            }
                            StepPassed();

                            if (absoluteSupported)
                            {
                                float position = options.Absolute.Position.Max + 1;
                                FocusMove focus = new FocusMove();
                                focus.Absolute = new AbsoluteFocus();
                                focus.Absolute.Position = position;

                                RunStep( () => { Client.Move(sources[i].token, focus); }, 
                                    "Move - negative test (invalid Position)",
                                    new ValidateTypeFault( ValidateInvalidMoveFault));

                                DoRequestDelay();
                                
                                BeginStep(string.Format("Check if Absolute Move Speed is supported for video source '{0}'", sources[i].token));
                                bool supported = (options.Absolute.Speed != null);
                                if (supported)
                                {
                                    LogStepEvent("Supported");
                                }
                                else
                                {
                                    LogStepEvent("Not supported");
                                }
                                StepPassed();

                                if (supported)
                                {
                                    position = options.Absolute.Position.Average();
                                    focus.Absolute.Position = position;

                                    focus.Absolute.SpeedSpecified = true;
                                    focus.Absolute.Speed = options.Absolute.Speed.Max + 1;

                                    RunStep(() => { Client.Move(sources[i].token, focus); },
                                        "Move - negative test (invalid Speed)",
                                        new ValidateTypeFault(ValidateInvalidMoveFault));

                                    DoRequestDelay();

                                }
                            }
                        }
                    }
                });
        }

        [Test(Name = "IMAGING COMMAND RELATIVE MOVE",
            Order = "01.01.13",
            Id = "1-1-13",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void ImagingCommandRelativeMoveTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        MoveOptions20 options = null;
                        options = GetMoveOptions(sources[i].token);

                        if (options != null)
                        {
                            ValidateMoveOptions(options);

                            bool relativeSupported;

                            BeginStep(string.Format("Check if Relative Move is supported for video source '{0}'", sources[i].token));
                            relativeSupported = (options.Relative != null);
                            if (relativeSupported)
                            {
                                LogStepEvent("Supported");
                            }
                            else
                            {
                                LogStepEvent("Not supported");
                            }
                            StepPassed();

                            if (relativeSupported)
                            {
                                float distance = options.Relative.Distance.Max/2;
                                FocusMove focus = new FocusMove();
                                focus.Relative = new RelativeFocus();
                                focus.Relative.Distance = distance;

                                Move(sources[i].token, focus);
                                
                                bool speedSupported;

                                BeginStep(string.Format("Check if Relative Move Speed is supported for video source '{0}'", sources[i].token));
                                speedSupported = (options.Relative.Speed != null);
                                if (speedSupported)
                                {
                                    LogStepEvent("Supported");
                                }
                                else
                                {
                                    LogStepEvent("Not supported");
                                }
                                StepPassed();

                                if (speedSupported)
                                {
                                    focus.Relative.Distance = options.Relative.Distance.Min/2;
                                    focus.Relative.SpeedSpecified = true;
                                    focus.Relative.Speed = options.Relative.Speed.Average();

                                    Move(sources[i].token, focus);
                                }
                            }
                        }
                    }
                });
        }
        
        [Test(Name = "IMAGING COMMAND RELATIVE MOVE – INVALID SETTINGS",
            Order = "01.01.14",
            Id = "1-1-14",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void ImagingCommandRelativeMoveInvalidSettingsTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        MoveOptions20 options = null;
                        options = GetMoveOptions(sources[i].token);

                        if (options != null)
                        {
                            ValidateMoveOptions(options);

                            bool relativeSupported;

                            BeginStep(string.Format("Check if Relative Move is supported for video source '{0}'", sources[i].token));
                            relativeSupported = (options.Relative != null);
                            if (relativeSupported)
                            {
                                LogStepEvent("Supported");
                            }
                            else
                            {
                                LogStepEvent("Not supported");
                            }
                            StepPassed();

                            if (relativeSupported)
                            {
                                float distance = options.Relative.Distance.Max + 1;
                                FocusMove focus = new FocusMove();
                                focus.Relative = new RelativeFocus();
                                focus.Relative.Distance = distance;

                                RunStep(() => { Client.Move(sources[i].token, focus); },
                                    "Move - negative test (invalid Distance)",
                                    new ValidateTypeFault(ValidateInvalidMoveFault));

                                DoRequestDelay();

                                bool speedSupported;

                                BeginStep(string.Format("Check if Relative Move Speed is supported for video source '{0}'", sources[i].token));
                                speedSupported = (options.Relative.Speed != null);
                                if (speedSupported)
                                {
                                    LogStepEvent("Supported");
                                }
                                else
                                {
                                    LogStepEvent("Not supported");
                                }
                                StepPassed();

                                if (speedSupported)
                                {
                                    focus.Relative.Distance = options.Relative.Distance.Average();
                                    focus.Relative.SpeedSpecified = true;
                                    focus.Relative.Speed = options.Relative.Speed.Max + 1;

                                    RunStep(() => { Client.Move(sources[i].token, focus); },
                                        "Move - negative test (invalid Speed)",
                                        new ValidateTypeFault(ValidateInvalidMoveFault));

                                    DoRequestDelay();
                                }
                            }
                        }
                    }
                });
        }
        
        [Test(Name = "IMAGING COMMAND CONTINUOUS MOVE",
            Order = "01.01.15",
            Id = "1-1-15",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void ImagingCommandContinuousMoveTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        MoveOptions20 options = null;
                        options = GetMoveOptions(sources[i].token);

                        if (options != null)
                        {
                            ValidateMoveOptions(options);

                            bool continiousSupported;

                            BeginStep(string.Format("Check if Continuous Move is supported for video source '{0}'", sources[i].token));
                            bool continuousSupported = (options.Continuous != null);
                            if (continuousSupported)
                            {
                                LogStepEvent("Supported");
                            }
                            else
                            {
                                LogStepEvent("Not supported");
                            }
                            StepPassed();

                            if (continuousSupported)
                            {
                                float speed = options.Continuous.Speed.Average();
                                FocusMove focus = new FocusMove();
                                focus.Continuous = new ContinuousFocus();
                                focus.Continuous.Speed = speed;

                                Move(sources[i].token, focus);

                                bool stopSucceeded;
                                Stop(sources[i].token, out stopSucceeded);

                                if (!stopSucceeded)
                                {
                                    double timeout = ((double)_operationDelay) / 1000;
                                    BeginStep(string.Format("Wait {0} seconds to allow the DUT to move focus", timeout.ToString("0.000")));
                                    Sleep(_operationDelay);
                                    StepPassed();
                                }

                            }
                        }
                    }
                });
        }

        [Test(Name = "IMAGING COMMAND CONTINUOUS MOVE – INVALID SETTINGS",
            Order = "01.01.16",
            Id = "1-1-16",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void ImagingCommandContinuousMoveInvalidSettingsTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        MoveOptions20 options = null;
                        options = GetMoveOptions(sources[i].token);

                        if (options != null)
                        {
                            ValidateMoveOptions(options);

                            bool continiousSupported;

                            BeginStep(string.Format("Check if Continuous Move is supported for video source '{0}'", sources[i].token));
                            bool continuousSupported = (options.Continuous != null);
                            if (continuousSupported)
                            {
                                LogStepEvent("Supported");
                            }
                            else
                            {
                                LogStepEvent("Not supported");
                            }
                            StepPassed();

                            if (continuousSupported)
                            {
                                float speed = options.Continuous.Speed.Max + 1;
                                FocusMove focus = new FocusMove();
                                focus.Continuous = new ContinuousFocus();
                                focus.Continuous.Speed = speed;

                                RunStep(() => { Client.Move(sources[i].token, focus); },
                                    "Move - negative test (invalid Speed)",
                                    new ValidateTypeFault(ValidateInvalidMoveFault));
                                DoRequestDelay();
                            }
                        }
                    }
                });
        }

        [Test(Name = "IMAGING COMMAND MOVE – INVALID VIDEOSOURCETOKEN",
            Order = "01.01.17",
            Id = "1-1-17",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void ImagingCommandMoveInvalidSourceTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    string[] tokens = sources.Select(s => s.token).ToArray();

                    string invalidToken = tokens.GetNonMatchingString();

                    RunStep(
                            () =>
                            {
                                FocusMove focus = new FocusMove();
                                focus.Continuous = new ContinuousFocus();
                                focus.Continuous.Speed = 1.0F;

                                Client.Move(invalidToken, focus);

                            }, 
                            "Move - negative test",
                            new ValidateTypeFault(ValidateInvalidTokenFault));

                    DoRequestDelay();
                });
        }

        [Test(Name = "IMAGING COMMAND MOVE – UNSUPPORTED MOVE",
            Order = "01.01.18",
            Id = "1-1-18",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void ImagingCommandMoveUnsupportedMoveTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        MoveOptions20 options = null;
                        options = GetMoveOptions(sources[i].token);

                        if (options != null)
                        {
                            ValidateMoveOptions(options);

                            // absolute

                            bool absoluteSupported;

                            BeginStep(string.Format("Check if Absolute Move is supported for video source '{0}'", sources[i].token));
                            absoluteSupported = (options.Absolute != null);
                            if (absoluteSupported)
                            {
                                LogStepEvent("Supported");
                            }
                            else
                            {
                                LogStepEvent("Not supported");
                            }
                            StepPassed();

                            if (!absoluteSupported)
                            {
                                FocusMove focus = new FocusMove();
                                focus.Absolute = new AbsoluteFocus();
                                focus.Absolute.Position = 1.0F;

                                RunStep(() => { Client.Move(sources[i].token, focus); },
                                    "Move - negative test (absolute not supported)",
                                    new ValidateTypeFault(ValidateInvalidMoveFault));

                                DoRequestDelay();
                            }

                            // relative
                            
                            bool relativeSupported;

                            BeginStep(string.Format("Check if Relative Move is supported for video source '{0}'", sources[i].token));
                            relativeSupported = (options.Relative != null);
                            if (relativeSupported)
                            {
                                LogStepEvent("Supported");
                            }
                            else
                            {
                                LogStepEvent("Not supported");
                            }
                            StepPassed();

                            if (!relativeSupported)
                            {
                                FocusMove focus = new FocusMove();
                                focus.Relative = new RelativeFocus();
                                focus.Relative.Distance = 1.0F;

                                RunStep(() => { Client.Move(sources[i].token, focus); },
                                        "Move - negative test (relative not supported)",
                                        new ValidateTypeFault(ValidateInvalidMoveFault));
                                DoRequestDelay();
                            }
                            

                            // continuous
                            bool continiousSupported;

                            BeginStep(string.Format("Check if Continuous Move is supported for video source '{0}'", sources[i].token));
                            bool continuousSupported = (options.Continuous != null);
                            if (continuousSupported)
                            {
                                LogStepEvent("Supported");
                            }
                            else
                            {
                                LogStepEvent("Not supported");
                            }
                            StepPassed();

                            if (!continuousSupported)
                            {
                                FocusMove focus = new FocusMove();
                                focus.Continuous = new ContinuousFocus();
                                focus.Continuous.Speed = 1.0F;

                                RunStep(() => { Client.Move(sources[i].token, focus); },
                                    "Move - negative test (continuous not supported)",
                                    new ValidateTypeFault(ValidateInvalidMoveFault));
                                DoRequestDelay();
                            }
                        }
                    }
                });
        }

        [Test(Name = "IMAGING COMMAND GETSTATUS",
            Order = "01.01.19",
            Id = "1-1-19",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void GetImagingStatusTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        ImagingStatus20 status = null;
                        bool succeeded = false;
                        status = GetImagingStatus(sources[i].token, out succeeded);

                        if (succeeded)
                        {
                            ValidateImagingStatus(status);
                        }

                    }
                });
        }

        [Test(Name = "IMAGING COMMAND GETSTATUS – INVALID VIDEOSOURCETOKEN",
            Order = "01.01.20",
            Id = "1-1-20",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void GetImagingStatusInvalidTokenTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    string[] tokens = sources.Select(s => s.token).ToArray();

                    string invalidToken = tokens.GetNonMatchingString();

                    RunStepHandleNotSupported(
                            () =>
                            {
                                ImagingStatus20 status = Client.GetStatus(invalidToken);

                            },
                            "GetStatus - negative test",
                            new ValidateTypeFault(
                                (FaultException fault, out string reason) =>
                                {
                                    bool ok = fault.IsValidOnvifFault("Sender/InvalidArgVal/NoSource", out reason);
                                    if (!ok)
                                    {
                                        LogStepEvent("WARNING: fault received is not Sender/InvalidArgVal/NoSource");
                                    }
                                    return true;
                                }),
                            new ValidateNoFault(HandleNoExpectedFault));
                    DoRequestDelay();

                });
        }
        
        [Test(Name = "IMAGING COMMAND STOP",
            Order = "01.01.21",
            Id = "1-1-21",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void StopTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        bool succeeded;
                        Stop(sources[i].token, out succeeded);
                    }
                });
        }

        [Test(Name = "IMAGING COMMAND STOP – INVALID VIDEOSOURCETOKEN",
            Order = "01.01.22",
            Id = "1-1-22",
            Category = Category.IMAGING,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Imaging })]
        public void StopInvalidTokenTest()
        {
            RunTest(
                () =>
                {
                    VideoSource[] sources = GetAndValidateVideoSources();

                    string[] tokens = sources.Select(s => s.token).ToArray();

                    string invalidToken = tokens.GetNonMatchingString();

                    RunStepHandleNotSupported(
                            () =>
                            {
                                Client.Stop(invalidToken);

                            },
                            "Stop - negative test",
                            new ValidateTypeFault(
                                (FaultException fault, out string reason) =>
                                {
                                    bool ok = fault.IsValidOnvifFault("Sender/InvalidArgVal/NoSource", out reason);
                                    if (!ok)
                                    {
                                        LogStepEvent("WARNING: fault received is not Sender/InvalidArgVal/NoSource fault.");
                                    }
                                    return true;
                                }),
                            new ValidateNoFault(HandleNoExpectedFault));

                    DoRequestDelay();

                });
        }
        
        #region Utils

        VideoSource[] GetAndValidateVideoSources()
        {
            VideoSource[] sources = GetVideoSources();

            bool empty = (sources == null) || (sources.Length == 0);
            Assert(!empty, "No video sources received from the DUT", "Check that the DUT returned video sources");

            return sources;
        }

        protected ImagingSettings20 GenerateImagingSettings(ImagingOptions20 options, 
            ImagingSettings20 initialSettings)
        {
            ImagingSettings20 settings = new ImagingSettings20();
            
            settings.BacklightCompensation = new BacklightCompensation20();

            if (options.BacklightCompensation != null)
            {
                if (initialSettings.BacklightCompensation != null)
                {
                    BacklightCompensationMode backlightCompensationMode = initialSettings.BacklightCompensation.Mode;

                    foreach (BacklightCompensationMode mode in options.BacklightCompensation.Mode)
                    {
                        if (mode != initialSettings.BacklightCompensation.Mode)
                        {
                            backlightCompensationMode = mode;
                            break;
                        }
                    }

                    settings.BacklightCompensation.Mode = backlightCompensationMode;
                    if (backlightCompensationMode == BacklightCompensationMode.ON)
                    {
                        settings.BacklightCompensation.LevelSpecified = true;
                        if (initialSettings.BacklightCompensation.LevelSpecified)
                        {
                            settings.BacklightCompensation.Level = (initialSettings.BacklightCompensation.Level ==
                                                                    options.BacklightCompensation.Level.Max)
                                                                       ? options.BacklightCompensation.Level.Max
                                                                       : options.BacklightCompensation.Level.Min;
                        }
                        else
                        {
                            settings.BacklightCompensation.Level = options.BacklightCompensation.Level.Min;
                        }
                    }
                }
                else
                {
                    BacklightCompensationMode backlightCompensationMode = options.BacklightCompensation.Mode[0];
                    settings.BacklightCompensation.Mode = backlightCompensationMode;
                    if (backlightCompensationMode == BacklightCompensationMode.ON)
                    {
                        settings.BacklightCompensation.LevelSpecified = true;
                        settings.BacklightCompensation.Level = options.BacklightCompensation.Level.Min;
                    }
                }
            }

            if (options.Brightness != null)
            {
                settings.BrightnessSpecified = true;
                if (initialSettings.BrightnessSpecified)
                {
                    settings.Brightness = initialSettings.Brightness == options.Brightness.Max
                                              ? options.Brightness.Min
                                              : options.Brightness.Max;
                }
                else
                {
                    settings.Brightness = options.Brightness.Min;
                }
            }
            else
            {
                settings.BrightnessSpecified = false;
            }

            if (options.ColorSaturation != null)
            {
                settings.ColorSaturationSpecified = true;
                if (initialSettings.ColorSaturationSpecified)
                {
                    settings.ColorSaturation = 
                        options.ColorSaturation.Max == initialSettings.ColorSaturation ? 
                        options.ColorSaturation.Min : options.ColorSaturation.Max;
                }
                else
                {
                    settings.ColorSaturation = options.ColorSaturation.Min;
                }
            }
            else
            {
                settings.ColorSaturationSpecified = false;
            }
            
            if (options.Contrast != null)
            {
                settings.ContrastSpecified = true;
                if (initialSettings.ContrastSpecified)
                {
                    settings.Contrast = options.Contrast.Min != initialSettings.Contrast
                                            ? options.Contrast.Min
                                            : options.Contrast.Max;
                }
                else
                {
                    settings.Contrast = options.Contrast.Min;
                }
            }
            else
            {
                settings.ContrastSpecified = false;
            }


            if (options.Exposure != null)
            {
                settings.Exposure = new Exposure20();
                ExposureMode mode = options.Exposure.Mode[0]; // first

                // or different;
                if (settings.Exposure != null)
                {
                    mode = initialSettings.Exposure.Mode;
                    foreach (ExposureMode m in options.Exposure.Mode)
                    {
                        if (mode != m)
                        {
                            mode = m;
                            break;
                        }
                    }
                }

                settings.Exposure.Mode = mode;

                if (initialSettings.Exposure != null)
                {
                    if (mode == ExposureMode.AUTO)
                    {
                        settings.Exposure.PrioritySpecified = true;
                        ExposurePriority priority = options.Exposure.Priority[0];
                        if (initialSettings.Exposure.PrioritySpecified)
                        {
                            foreach (ExposurePriority p in options.Exposure.Priority)
                            {
                                if (p != initialSettings.Exposure.Priority)
                                {
                                    priority = p;
                                    break;
                                }
                            }
                        }
                        settings.Exposure.Priority = priority;


                        settings.Exposure.MinExposureTimeSpecified = true;
                        if (initialSettings.Exposure.MinExposureTimeSpecified)
                        {
                            settings.Exposure.MinExposureTime = options.Exposure.MinExposureTime.Min !=
                                                                initialSettings.Exposure.MinExposureTime
                                                                    ? options.Exposure.MinExposureTime.Min
                                                                    : options.Exposure.MinExposureTime.Max;
                        }
                        else
                        {
                            settings.Exposure.MinExposureTime = options.Exposure.MinExposureTime.Min;
                        }

                        settings.Exposure.MaxExposureTimeSpecified = true;
                        if (initialSettings.Exposure.MaxExposureTimeSpecified)
                        {
                            settings.Exposure.MaxExposureTime = initialSettings.Exposure.MaxExposureTime ==
                                                                options.Exposure.MaxExposureTime.Max
                                                                    ? options.Exposure.MaxExposureTime.Min
                                                                    : options.Exposure.MaxExposureTime.Max;
                        }
                        else
                        {
                            settings.Exposure.MaxExposureTime = options.Exposure.MaxExposureTime.Max;
                        }
                        settings.Exposure.MaxGainSpecified = true;
                        if (!initialSettings.Exposure.MaxGainSpecified)
                        {
                            settings.Exposure.MaxGain = initialSettings.Exposure.MaxGain ==
                                                                options.Exposure.MaxGain.Max
                                                                    ? options.Exposure.MaxGain.Min
                                                                    : options.Exposure.MaxGain.Max;
                        }
                        else
                        {
                            settings.Exposure.MaxGain = options.Exposure.MaxGain.Max;
                        }

                        settings.Exposure.MinGainSpecified = true;
                        if (initialSettings.Exposure.MinGainSpecified)
                        {
                            settings.Exposure.MinGain = initialSettings.Exposure.MinGain ==
                                                                options.Exposure.MinGain.Max
                                                                    ? options.Exposure.MinGain.Min
                                                                    : options.Exposure.MinGain.Max;
                        }
                        else
                        {
                            settings.Exposure.MinGain = options.Exposure.MinGain.Max;
                        }


                        settings.Exposure.MinIrisSpecified = true;
                        if (initialSettings.Exposure.MinIrisSpecified)
                        {
                            settings.Exposure.MinIris = initialSettings.Exposure.MinIris ==
                                                                options.Exposure.MinIris.Max
                                                                    ? options.Exposure.MinIris.Min
                                                                    : options.Exposure.MinIris.Max;
                        }
                        else
                        {
                            settings.Exposure.MinIris = options.Exposure.MinIris.Max;
                        }


                        settings.Exposure.MaxIrisSpecified = true;
                        if (initialSettings.Exposure.MaxIrisSpecified)
                        {
                            settings.Exposure.MaxIris = initialSettings.Exposure.MaxIris ==
                                                                options.Exposure.MaxIris.Max
                                                                    ? options.Exposure.MaxIris.Min
                                                                    : options.Exposure.MaxIris.Max;
                        }
                        else
                        {
                            settings.Exposure.MaxIris = options.Exposure.MaxIris.Max;
                        }
                    }

                    if (mode == ExposureMode.MANUAL)
                    {
                        settings.Exposure.ExposureTimeSpecified = true;
                        if (initialSettings.Exposure.ExposureTimeSpecified)
                        {
                            settings.Exposure.ExposureTime = initialSettings.Exposure.ExposureTime ==
                                                                options.Exposure.ExposureTime.Max
                                                                    ? options.Exposure.ExposureTime.Min
                                                                    : options.Exposure.ExposureTime.Max;
                        }
                        else
                        {
                            settings.Exposure.ExposureTime = options.Exposure.ExposureTime.Max;
                        }

                        settings.Exposure.GainSpecified = true;
                        if (initialSettings.Exposure.GainSpecified)
                        {
                            settings.Exposure.Gain = initialSettings.Exposure.Gain ==
                                                                options.Exposure.Gain.Max
                                                                    ? options.Exposure.Gain.Min
                                                                    : options.Exposure.Gain.Max;
                        }
                        else
                        {
                            settings.Exposure.Gain = options.Exposure.Gain.Max;
                        }

                        settings.Exposure.IrisSpecified = true;
                        if (initialSettings.Exposure.IrisSpecified)
                        {
                            settings.Exposure.Gain = initialSettings.Exposure.Gain ==
                                                                options.Exposure.Gain.Max
                                                                    ? options.Exposure.Gain.Min
                                                                    : options.Exposure.Gain.Max;
                        }
                        else
                        {
                            settings.Exposure.Gain = options.Exposure.Gain.Max;
                        }
                    }
                }
                else
                {
                    // initial settings not specified 
                    if (mode == ExposureMode.AUTO)
                    {
                        settings.Exposure.PrioritySpecified = true;
                        settings.Exposure.Priority = options.Exposure.Priority[0];
                        settings.Exposure.MinExposureTimeSpecified = true;
                        settings.Exposure.MinExposureTime = options.Exposure.MinExposureTime.Min;
                        settings.Exposure.MaxExposureTimeSpecified = true;
                        settings.Exposure.MaxExposureTime = options.Exposure.MaxExposureTime.Max;
                        settings.Exposure.MaxGainSpecified = true;
                        settings.Exposure.MaxGain = options.Exposure.MaxGain.Max;
                        settings.Exposure.MinGainSpecified = true;
                        settings.Exposure.MinGain = options.Exposure.MinGain.Min;
                        settings.Exposure.MinIrisSpecified = true;
                        settings.Exposure.MinIris = options.Exposure.MinIris.Min;
                        settings.Exposure.MaxIrisSpecified = true;
                        settings.Exposure.MaxIris = options.Exposure.MaxIris.Max;
                    }

                    if (mode == ExposureMode.MANUAL)
                    {
                        settings.Exposure.ExposureTimeSpecified = true;
                        settings.Exposure.ExposureTime = options.Exposure.ExposureTime.Min;
                        settings.Exposure.GainSpecified = true;
                        settings.Exposure.Gain = options.Exposure.Gain.Min;
                        settings.Exposure.IrisSpecified = true;
                        settings.Exposure.Iris = options.Exposure.Iris.Min;
                    }                    
                }

            }

            if (options.Focus != null)
            {
                settings.Focus = new FocusConfiguration20();
                if (initialSettings.Focus != null)
                {
                    AutoFocusMode mode = options.Focus.AutoFocusModes[0];

                    foreach (AutoFocusMode m in options.Focus.AutoFocusModes)
                    {
                        if (m != initialSettings.Focus.AutoFocusMode)
                        {
                            mode = m;
                            break;
                        }
                    }

                    settings.Focus.AutoFocusMode = mode;

                    if (settings.Focus.AutoFocusMode == AutoFocusMode.AUTO)
                    {
                        settings.Focus.FarLimitSpecified = true;
                        if (initialSettings.Focus.FarLimitSpecified)
                        {
                            settings.Focus.FarLimit = initialSettings.Focus.FarLimit ==
                                                                options.Focus.FarLimit.Max
                                                                    ? options.Focus.FarLimit.Min
                                                                    : options.Focus.FarLimit.Max;
                        }
                        else
                        {
                            settings.Focus.FarLimit = options.Focus.FarLimit.Max;
                        }

                        settings.Focus.NearLimitSpecified = true;
                        if (initialSettings.Focus.NearLimitSpecified)
                        {
                            settings.Focus.NearLimit = initialSettings.Focus.NearLimit ==
                                                                options.Focus.NearLimit.Max
                                                                    ? options.Focus.NearLimit.Min
                                                                    : options.Focus.NearLimit.Max;
                        }
                        else
                        {
                            settings.Focus.NearLimit = options.Focus.NearLimit.Max;
                        }
                    }

                    if (settings.Focus.AutoFocusMode == AutoFocusMode.MANUAL)
                    {
                        settings.Focus.DefaultSpeedSpecified = true;
                        if (initialSettings.Focus.DefaultSpeedSpecified)
                        {
                            settings.Focus.DefaultSpeed = initialSettings.Focus.DefaultSpeed ==
                                                                options.Focus.DefaultSpeed.Max
                                                                    ? options.Focus.DefaultSpeed.Min
                                                                    : options.Focus.DefaultSpeed.Max;
                        }
                        else
                        {
                            settings.Focus.DefaultSpeed = options.Focus.DefaultSpeed.Max;
                        }
                    }
                }
                else
                {
                    settings.Focus.AutoFocusMode = options.Focus.AutoFocusModes[0];

                    if (settings.Focus.AutoFocusMode == AutoFocusMode.AUTO)
                    {
                        settings.Focus.FarLimitSpecified = true;
                        settings.Focus.FarLimit = options.Focus.FarLimit.Min;
                        settings.Focus.NearLimitSpecified = true;
                        settings.Focus.NearLimit = options.Focus.NearLimit.Min;
                    }
                    if (settings.Focus.AutoFocusMode == AutoFocusMode.MANUAL)
                    {
                        settings.Focus.DefaultSpeedSpecified = true;
                        settings.Focus.DefaultSpeed = options.Focus.DefaultSpeed.Min;
                    }
                }
            }

            if (options.IrCutFilterModes != null && options.IrCutFilterModes.Length > 0)
            {
                settings.IrCutFilterSpecified = true;
                IrCutFilterMode mode = options.IrCutFilterModes[0];
                if (initialSettings.IrCutFilterSpecified)
                {
                    foreach (IrCutFilterMode m in options.IrCutFilterModes)
                    {
                        if (m != initialSettings.IrCutFilter)
                        {
                            mode = m;
                            break;
                        }
                    }
                }
                settings.IrCutFilter = mode;
            }

            if (options.Sharpness != null)
            {
                settings.SharpnessSpecified = true;
                if (initialSettings.SharpnessSpecified)
                {
                    settings.Sharpness = initialSettings.Sharpness == options.Sharpness.Min ? options.Sharpness.Max : options.Sharpness.Min;
                }
                else
                {
                    settings.Sharpness = options.Sharpness.Min;
                }
            }
            else
            {
                settings.SharpnessSpecified = false;
            }

            if (options.WhiteBalance != null)
            {
                settings.WhiteBalance = new WhiteBalance20();

                if (initialSettings.WhiteBalance != null)
                {
                    WhiteBalanceMode mode = options.WhiteBalance.Mode[0];
                    foreach (WhiteBalanceMode m in options.WhiteBalance.Mode)
                    {
                        if (m != initialSettings.WhiteBalance.Mode)
                        {
                            mode = m;
                            break;
                        }
                    }

                    settings.WhiteBalance.Mode = mode;

                    if (settings.WhiteBalance.Mode == WhiteBalanceMode.MANUAL)
                    {
                        settings.WhiteBalance.CbGainSpecified = true;
                        if (initialSettings.WhiteBalance.CbGainSpecified)
                        {
                            settings.WhiteBalance.CbGain =
                                initialSettings.WhiteBalance.CbGain == options.WhiteBalance.YbGain.Min
                                    ? options.WhiteBalance.YbGain.Max
                                    : options.WhiteBalance.YbGain.Min;
                        }
                        else
                        {
                            settings.WhiteBalance.CbGain = options.WhiteBalance.YbGain.Max;
                        }


                        settings.WhiteBalance.CrGainSpecified = true;
                        if (initialSettings.WhiteBalance.CrGainSpecified)
                        {
                            settings.WhiteBalance.CrGain =
                                initialSettings.WhiteBalance.CrGain == options.WhiteBalance.YrGain.Min
                                    ? options.WhiteBalance.YrGain.Max
                                    : options.WhiteBalance.YrGain.Min;
                        }
                        else
                        {
                            settings.WhiteBalance.CrGain = options.WhiteBalance.YrGain.Max;
                        }
                    }
                }
                else
                {
                    settings.WhiteBalance.Mode = options.WhiteBalance.Mode[0];

                    if (settings.WhiteBalance.Mode == WhiteBalanceMode.MANUAL)
                    {
                        settings.WhiteBalance.CbGainSpecified = true;
                        settings.WhiteBalance.CbGain = options.WhiteBalance.YbGain.Min;

                        settings.WhiteBalance.CrGainSpecified = true;
                        settings.WhiteBalance.CrGain = options.WhiteBalance.YrGain.Min;
                    }
                }
            }

            if (options.WideDynamicRange != null)
            {
                settings.WideDynamicRange = new WideDynamicRange20();

                if (initialSettings.WideDynamicRange != null)
                {
                    WideDynamicMode mode = options.WideDynamicRange.Mode[0];
                    foreach (WideDynamicMode m in options.WideDynamicRange.Mode)
                    {
                        if (m != initialSettings.WideDynamicRange.Mode)
                        {
                            mode = m;
                            break;
                        }
                    }

                    settings.WideDynamicRange.Mode = mode;
                    if (settings.WideDynamicRange.Mode == WideDynamicMode.ON)
                    {
                        settings.WideDynamicRange.LevelSpecified = true;
                        if (initialSettings.WideDynamicRange.LevelSpecified)
                        {
                            settings.WideDynamicRange.Level = initialSettings.WideDynamicRange.Level ==
                                                              options.WideDynamicRange.Level.Min
                                                                  ? options.WideDynamicRange.Level.Max
                                                                  : options.WideDynamicRange.Level.Min;
                        }
                        else
                        {
                            settings.WideDynamicRange.Level = options.WideDynamicRange.Level.Min;
                        }
                    }
                }
                else
                {
                    settings.WideDynamicRange.Mode = options.WideDynamicRange.Mode[0];
                    if (settings.WideDynamicRange.Mode == WideDynamicMode.ON)
                    {
                        settings.WideDynamicRange.LevelSpecified = true;
                        settings.WideDynamicRange.Level = options.WideDynamicRange.Level.Min;
                    }
                }
            }

            return settings;
        }
        
        protected ImagingSettings20 GenerateInvalidImagingSettings(ImagingOptions20 options)
        {
            ImagingSettings20 settings = new ImagingSettings20();

            settings.BacklightCompensation = new BacklightCompensation20();

            if (options.BacklightCompensation != null)
            {
                BacklightCompensationMode backlightCompensationMode = options.BacklightCompensation.Mode[0];
                settings.BacklightCompensation.Mode = backlightCompensationMode;
                if (backlightCompensationMode == BacklightCompensationMode.ON)
                {
                    settings.BacklightCompensation.LevelSpecified = true;
                    settings.BacklightCompensation.Level = options.BacklightCompensation.Level.Max + 1;
                }
            }

            if (options.Brightness != null)
            {
                settings.Brightness = options.Brightness.Max + 1;
            }
            settings.BrightnessSpecified = (options.Brightness != null);


            if (options.ColorSaturation != null)
            {
                settings.ColorSaturation = options.ColorSaturation.Max + 1;
            }
            settings.ColorSaturationSpecified = (options.ColorSaturation != null);


            if (options.Contrast != null)
            {
                settings.Contrast = options.Contrast.Max + 1;
            }
            settings.ContrastSpecified = (options.Contrast != null);


            if (options.Exposure != null)
            {
                settings.Exposure = new Exposure20();
                ExposureMode mode = options.Exposure.Mode[0];
                settings.Exposure.Mode = mode;

                if (mode == ExposureMode.AUTO)
                {
                    settings.Exposure.PrioritySpecified = true;
                    settings.Exposure.Priority = options.Exposure.Priority[0];
                    settings.Exposure.MinExposureTimeSpecified = true;
                    settings.Exposure.MinExposureTime = options.Exposure.MinExposureTime.Min-1;
                    settings.Exposure.MaxExposureTimeSpecified = true;
                    settings.Exposure.MaxExposureTime = options.Exposure.MaxExposureTime.Max + 1;
                    settings.Exposure.MaxGainSpecified = true;
                    settings.Exposure.MaxGain = options.Exposure.MaxGain.Max + 1;
                    settings.Exposure.MinGainSpecified = true;
                    settings.Exposure.MinGain = options.Exposure.MinGain.Min-1;
                    settings.Exposure.MinIrisSpecified = true;
                    settings.Exposure.MinIris = options.Exposure.MinIris.Min-1;
                    settings.Exposure.MaxIrisSpecified = true;
                    settings.Exposure.MaxIris = options.Exposure.MaxIris.Max + 1;
                }

                if (mode == ExposureMode.MANUAL)
                {
                    settings.Exposure.ExposureTimeSpecified = true;
                    settings.Exposure.ExposureTime = options.Exposure.ExposureTime.Max + 1;
                    settings.Exposure.GainSpecified = true;
                    settings.Exposure.Gain = options.Exposure.Gain.Max + 1;
                    settings.Exposure.IrisSpecified = true;
                    settings.Exposure.Iris = options.Exposure.Iris.Max + 1;
                }
            }

            if (options.Focus != null)
            {
                settings.Focus = new FocusConfiguration20();
                settings.Focus.AutoFocusMode = options.Focus.AutoFocusModes[0];

                if (settings.Focus.AutoFocusMode == AutoFocusMode.AUTO)
                {
                    settings.Focus.FarLimitSpecified = true;
                    settings.Focus.FarLimit = options.Focus.FarLimit.Max + 1;
                    settings.Focus.NearLimitSpecified = true;
                    settings.Focus.NearLimit = options.Focus.NearLimit.Max + 1;
                }
                if (settings.Focus.AutoFocusMode == AutoFocusMode.MANUAL)
                {
                    settings.Focus.DefaultSpeedSpecified = true;
                    settings.Focus.DefaultSpeed = options.Focus.DefaultSpeed.Max + 1;
                }
            }


            if (options.IrCutFilterModes != null && options.IrCutFilterModes.Length > 0)
            {
                settings.IrCutFilterSpecified = true;
                settings.IrCutFilter = options.IrCutFilterModes[0];
            }

            if (options.Sharpness != null)
            {
                settings.SharpnessSpecified = true;
                settings.Sharpness = options.Sharpness.Max + 1;
            }


            if (options.WhiteBalance != null)
            {
                settings.WhiteBalance = new WhiteBalance20();

                if (options.WhiteBalance.Mode != null && options.WhiteBalance.Mode.Length > 0)
                {
                    settings.WhiteBalance.Mode = options.WhiteBalance.Mode[0];

                    if (settings.WhiteBalance.Mode == WhiteBalanceMode.MANUAL)
                    {
                        if (options.WhiteBalance.YbGain != null)
                        {
                            settings.WhiteBalance.CbGainSpecified = true;
                            settings.WhiteBalance.CbGain = options.WhiteBalance.YbGain.Max + 1;
                        }

                        if (options.WhiteBalance.YrGain != null)
                        {
                            settings.WhiteBalance.CrGainSpecified = true;
                            settings.WhiteBalance.CrGain = options.WhiteBalance.YrGain.Max + 1;
                        }
                    }
                }
            }


            if (options.WideDynamicRange != null)
            {
                settings.WideDynamicRange = new WideDynamicRange20();

                if (options.WideDynamicRange.Mode != null && options.WideDynamicRange.Mode.Length > 0)
                {
                    settings.WideDynamicRange.Mode = options.WideDynamicRange.Mode[0];
                    if (settings.WideDynamicRange.Mode == WideDynamicMode.ON)
                    {
                        if (options.WideDynamicRange.Level != null)
                        {
                            settings.WideDynamicRange.LevelSpecified = true;
                            settings.WideDynamicRange.Level = options.WideDynamicRange.Level.Max + 1;
                        }
                    }
                }
            }


            
            return settings;
        }

        #endregion

        #region Validate structures

        protected void ValidateImagingSettings(ImagingSettings mediaSettings, 
            ImagingSettings20 imagingSettings)
        {
            bool bEqual = true;
            StringBuilder sb = new StringBuilder();

            if (mediaSettings != null && imagingSettings != null)
            {

                if (!(mediaSettings.BacklightCompensation == null && imagingSettings.BacklightCompensation == null))
                {
                    if (mediaSettings.BacklightCompensation != null && imagingSettings.BacklightCompensation != null)
                    {

                        if (mediaSettings.BacklightCompensation.Mode == BacklightCompensationMode.OFF &&
                            imagingSettings.BacklightCompensation.Mode == BacklightCompensationMode.OFF)
                        {
                        }
                        else
                        {
                            if (mediaSettings.BacklightCompensation.Mode == BacklightCompensationMode.ON &&
                                imagingSettings.BacklightCompensation.Mode == BacklightCompensationMode.ON)
                            {
                                if (mediaSettings.BacklightCompensation.Level !=
                                    imagingSettings.BacklightCompensation.Level)
                                {
                                    bEqual = false;
                                    sb.AppendLine("BacklightCompensation.Level is different");
                                }
                            }
                            else
                            {
                                bEqual = false;
                                sb.AppendLine("BacklightCompensation.Mode is different");
                            }
                        }
                    }
                    else
                    {
                        bEqual = false;
                        sb.AppendLine(string.Format("BacklightCompensation is NULL for settings got from {0} service",
                                                    mediaSettings.BacklightCompensation == null ? "Media" : "Imaging"));
                    }
                }

                // to be continued...

            }
            else
            {
                if (!(mediaSettings == null && imagingSettings == null))
                {
                    bEqual = false;
                    sb.AppendLine(string.Format("Imaging settings not returned from {0} service", mediaSettings == null ? "Media" : "Imaging"));
                }
            }
            string reason = sb.ToStringTrimNewLine();
            Assert(bEqual, reason, "Compare settings got via media service and via imaging service");
        }

        protected void ValidateImagingOptions(ImagingOptions20 options)
        {
            bool ok = true;
            bool currentOk = true;
            StringBuilder sb = new StringBuilder();

            // BacklightCompensation
            if (options.BacklightCompensation != null)
            {
                bool off = options.BacklightCompensation.Mode != null &&
                           options.BacklightCompensation.Mode.Length == 1 &&
                           options.BacklightCompensation.Mode[0] == BacklightCompensationMode.OFF;

                ok = ok && ValidateFloatRange(options.BacklightCompensation.Level,
                                              "BacklightCompensation level",
                                              !off,
                                              sb,
                                              "No Level options for BacklightCompensation",
                                              "Level defined for BacklightCompensation while Mode is OFF");
            }
            //else
            //{
                //ok = false;
                //sb.AppendLine("No Backlight Compensation options");
            //}
            
            // Brightness
            if (options.Brightness != null)
            {
                if (options.Brightness.Min > options.Brightness.Max)
                {
                    ok = false;
                    sb.AppendLine("Range is invalid for Brightness");
                }
            }            
            //else
            //{
            //    ok = false;
            //    sb.AppendLine("No Brightness options");
            //}



            // ColorSaturation
            if (options.ColorSaturation != null)
            {
                currentOk = ValidateFloatRange(options.ColorSaturation, "ColorSaturation", sb);
                ok = ok && currentOk;
            }

            // Contrast
            if (options.Contrast != null)
            {
                currentOk = ValidateFloatRange(options.Contrast, "Contrast", sb);
                ok = ok && currentOk;

            }

            // Exposure
            if (options.Exposure != null)
            {
                bool onlyAuto = false;
                bool onlyManual = false;

                if (options.Exposure.Mode != null && options.Exposure.Mode.Length > 0)
                {

                    if (options.Exposure.Mode.Length == 1)
                    {
                        onlyAuto = options.Exposure.Mode[0] == ExposureMode.AUTO;
                        onlyManual = !onlyAuto;
                    }

                    currentOk = ValidateFloatRange(options.Exposure.ExposureTime,
                                       "Exposure Time", 
                                       !onlyAuto, 
                                       sb, 
                                       "No Exposure Time options", 
                                       "Exposure Time options found while the only Mode is Auto");

                    ok = ok && currentOk;
                    
                    currentOk = ValidateFloatRange(options.Exposure.Gain,
                                       "Gain",
                                       !onlyAuto,
                                       sb,
                                       "No Gain options",
                                       "Gain options found while the only Mode is Auto");
                    ok = ok && currentOk;
                    
                    currentOk =  ValidateFloatRange(options.Exposure.Iris,
                                       "Iris",
                                       !onlyAuto,
                                       sb,
                                       "No Iris options",
                                       "Iris options found while the only Mode is Auto");
                    ok = ok && currentOk;
                    
                    currentOk = ValidateFloatRange(options.Exposure.MaxExposureTime,
                                       "MaxExposureTime",
                                       !onlyManual,
                                       sb,
                                       "No MaxExposureTime options",
                                       "MaxExposureTime options found while the only Mode is Manual");
                    ok = ok && currentOk;


                    currentOk = ValidateFloatRange(options.Exposure.MinExposureTime,
                                       "MinExposureTime",
                                       !onlyManual,
                                       sb,
                                       "No MinExposureTime options",
                                       "MinExposureTime options found while the only Mode is Manual");
                    ok = ok && currentOk;

                    currentOk = ValidateFloatRange(options.Exposure.MaxGain,
                                       "MaxGain",
                                       !onlyManual,
                                       sb,
                                       "No MaxGain options",
                                       "MaxGain options found while the only Mode is Manual");
                    ok = ok && currentOk;

                    currentOk = ValidateFloatRange(options.Exposure.MinGain,
                                       "MinGain",
                                       !onlyManual,
                                       sb,
                                       "No MinGain options",
                                       "MinGain options found while the only Mode is Manual");
                    ok = ok && currentOk;

                    currentOk = ValidateFloatRange(options.Exposure.MaxIris,
                                       "MaxIris",
                                       !onlyManual,
                                       sb,
                                       "No MaxIris options",
                                       "MaxIris options found while the only Mode is Manual");
                    ok = ok && currentOk;

                    currentOk = ValidateFloatRange(options.Exposure.MinIris,
                                       "MinIris",
                                       !onlyManual,
                                       sb,
                                       "No MinIris options",
                                       "MinIris options found while the only Mode is Manual");
                    ok = ok && currentOk;

                    if (onlyManual && (options.Exposure.Priority != null && options.Exposure.Priority.Length > 0))
                    {
                        ok = false;
                        sb.AppendLine("Exposure priority settings are not empty while the only Mode supported is manual");
                    }
                }
                else
                {
                    ok = false;
                    sb.AppendLine("Exposure modes not specified");
                }
            }
            //else
            //{
            //    ok = false;
            //    sb.AppendLine("No Exposure options");
            //}

            if (options.Focus != null)
            {

                bool onlyAuto = false;
                bool onlyManual = false;

                if (options.Focus.AutoFocusModes.Length == 1)
                {
                    onlyAuto = options.Focus.AutoFocusModes[0] == AutoFocusMode.AUTO;
                    onlyManual = !onlyAuto;
                }

                currentOk = ValidateFloatRange(options.Focus.DefaultSpeed,
                                   "DefaultSpeed",
                                   !onlyAuto,
                                   sb,
                                   "No DefaultSpeed options",
                                   "DefaultSpeed options found while the only Mode is Auto");
                ok = ok && currentOk;

                currentOk = ValidateFloatRange(options.Focus.NearLimit,
                                   "NearLimit",
                                   !onlyManual,
                                   sb,
                                   "No NearLimit options",
                                   "NearLimit options found while the only Mode is Manual");
                ok = ok && currentOk;

                currentOk = ValidateFloatRange(options.Focus.FarLimit,
                                   "FarLimit",
                                   !onlyManual,
                                   sb,
                                   "No FarLimit options",
                                   "FarLimit options found while the only Mode is Manual");
                ok = ok && currentOk;

            }
            //{
            //    ok = false;
            //    sb.AppendLine("No Focus options");
            //}

            if (options.Sharpness != null)
            {
                currentOk = ValidateFloatRange(options.Sharpness, "Sharpness", sb);
                ok = ok && currentOk;

            }

            if (options.WideDynamicRange != null)
            {
                bool onlyOff = options.WideDynamicRange.Mode != null &&
                options.WideDynamicRange.Mode.Length == 1 &&
                options.WideDynamicRange.Mode[0] == WideDynamicMode.OFF;
                
                currentOk = ValidateFloatRange(options.WideDynamicRange.Level,
                                               "WideDynamicRange",
                                               !onlyOff,
                                               sb,
                                               "No Level options for WideDynamicRange",
                                               "Level options for WideDynamicRange found while the only Mode is Off");
                ok = ok && currentOk;

            }
            //else
            //{
            //    ok = false;
            //    sb.AppendLine("No WideDynamicRange options");
            //}

            if (options.WhiteBalance != null)
            {
                bool onlyAuto = options.WhiteBalance.Mode != null &&
                                options.WhiteBalance.Mode.Length == 1 &&
                                options.WhiteBalance.Mode[0] == WhiteBalanceMode.AUTO;
                
                currentOk = ValidateFloatRange(options.WhiteBalance.YrGain,
                                   "WhiteBalance.YrGain",
                                   !onlyAuto,
                                   sb,
                                   "No YrGain options for WhiteBalance",
                                   "WhiteBalance.YrGain options found while the only Mode is Auto");
                ok = ok && currentOk;

                currentOk = ValidateFloatRange(options.WhiteBalance.YbGain,
                   "WhiteBalance.YbGain",
                   !onlyAuto,
                   sb,
                   "No YbGain options for WhiteBalance",
                   "WhiteBalance.YbGain options found while the only Mode is Auto");
                ok = ok && currentOk;
                
            }
            //else
            //{
            //    ok = false;
            //    sb.AppendLine("No WhiteBalance options");
            //}

            string dump = sb.ToStringTrimNewLine();

            Assert(ok, dump, "Validate options structure");
        }

        protected void ValidateMoveOptions(MoveOptions20 options)
        {
            bool ok = true;
            bool currentOk = true;
            StringBuilder sb = new StringBuilder();

            if (options.Absolute != null)
            {
                currentOk = ValidateFloatRange(options.Absolute.Position, "Absolute positios", sb);
                ok = ok && currentOk;

                currentOk = ValidateFloatRange(options.Absolute.Speed, "Absolute speed", sb);
                ok = ok && currentOk;

            }
            if (options.Relative != null)
            {
                currentOk = ValidateFloatRange(options.Relative.Distance, "Relative distance", sb);
                ok = ok && currentOk;

                currentOk = ValidateFloatRange(options.Relative.Speed, "Relative speed", sb);
                ok = ok && currentOk;
            }

            if (options.Continuous != null)
            {
                currentOk = ValidateFloatRange(options.Continuous.Speed, "Continuous speed", sb);
                ok = ok && currentOk;
            }
            
            string reason = sb.ToString();
            if (reason.Length > 0)
            {
                reason = reason.Remove(sb.Length - 2, 2);
            }
            Assert(ok, reason, "Validate Move options");
        }

        protected void ValidateImagingStatus(ImagingStatus20 status)
        {
            
        }

        protected bool ImagingSettingsEqual(ImagingSettings20 expectedSettings, ImagingSettings20 actualSettings, out string dump)
        {
            bool bOk = true;
            StringBuilder sb = new StringBuilder();
            
            /*             
             //  should the DUT clear all "Any" elements ?           
              
            if (expectedSettings.AnyAttr != null && actualSettings.AnyAttr != null)
            {
                

            }
            else
            {
                if (! (expectedSettings.AnyAttr == null && actualSettings.AnyAttr == null) )
                {
                    sb.AppendLine(string.Format("AnyAttr field: expected - {0}, actual - {1}", 
                        expectedSettings.AnyAttr == null ? "NULL" : "NOT NULL",
                        actualSettings.AnyAttr == null ? "NULL" : "NOT NULL"));
                }
            }
            */

            // BacklightCompensation 
            if (actualSettings.BacklightCompensation != null && expectedSettings.BacklightCompensation != null)
            {
                if (actualSettings.BacklightCompensation.Mode != expectedSettings.BacklightCompensation.Mode)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Backlight compensation mode: expected - {0}, actual - {1}",
                                                expectedSettings.BacklightCompensation.Mode,
                                                actualSettings.BacklightCompensation.Mode));
                }

                if (actualSettings.BacklightCompensation.LevelSpecified != expectedSettings.BacklightCompensation.LevelSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Backlight compensation level specified: expected - {0}, actual - {1}",
                                                expectedSettings.BacklightCompensation.LevelSpecified,
                                                actualSettings.BacklightCompensation.LevelSpecified));
                }

                if (actualSettings.BacklightCompensation.LevelSpecified && expectedSettings.BacklightCompensation.LevelSpecified)
                {
                    if (actualSettings.BacklightCompensation.Level != expectedSettings.BacklightCompensation.Level)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Backlight compensation level: expected - {0}, actual - {1}",
                                                    expectedSettings.BacklightCompensation.Level,
                                                    actualSettings.BacklightCompensation.Level));
                    }
                }
            }
            else
            {
                // Both nulls are OK (excluding tha fact that WE don't send null)
                if (!(actualSettings.BacklightCompensation == null && expectedSettings.BacklightCompensation == null))
                {
                    sb.AppendLine(string.Format("BacklightCompensation: {0}",
                                                CreateNullsDescription(expectedSettings.BacklightCompensation,
                                                                       actualSettings.BacklightCompensation)));
                    bOk = false;
                }
            }

            // Brightness
            if (actualSettings.BrightnessSpecified != expectedSettings.BrightnessSpecified)
            {
                bOk = false;
                sb.AppendLine(string.Format("Brightness specified: expected - {0}, actual - {1}",
                                            expectedSettings.BrightnessSpecified,
                                            actualSettings.BrightnessSpecified));
            }

            if (actualSettings.BrightnessSpecified && expectedSettings.BrightnessSpecified)
            {
                if (actualSettings.Brightness != expectedSettings.Brightness)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Brightness: expected - {0}, actual - {1}",
                                                expectedSettings.Brightness,
                                                actualSettings.Brightness));
                }
            }


            // Color saturation
            if (actualSettings.ColorSaturationSpecified != expectedSettings.ColorSaturationSpecified)
            {
                bOk = false;
                sb.AppendLine(string.Format("Color saturation specified: expected - {0}, actual - {1}",
                                            expectedSettings.ColorSaturationSpecified,
                                            actualSettings.ColorSaturationSpecified));
            }

            if (actualSettings.ColorSaturationSpecified && expectedSettings.ColorSaturationSpecified)
            {
                if (actualSettings.ColorSaturation != expectedSettings.ColorSaturation)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Color saturation: expected - {0}, actual - {1}",
                                                expectedSettings.ColorSaturation,
                                                actualSettings.ColorSaturation));
                }
            }

            // Contrast

            if (actualSettings.ContrastSpecified != expectedSettings.ContrastSpecified)
            {
                bOk = false;
                sb.AppendLine(string.Format("Contrast specified: expected - {0}, actual - {1}",
                                            expectedSettings.ContrastSpecified,
                                            actualSettings.ContrastSpecified));
            }

            if (actualSettings.ContrastSpecified && expectedSettings.ContrastSpecified)
            {
                if (actualSettings.Contrast != expectedSettings.Contrast)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Contrast: expected - {0}, actual - {1}",
                                                expectedSettings.Contrast,
                                                actualSettings.Contrast));
                }
            }


            // Exposure
            if (actualSettings.Exposure != null && expectedSettings.Exposure != null)
            {
                Exposure20 expected = expectedSettings.Exposure;
                Exposure20 actual = actualSettings.Exposure;

                if (expected.Mode != actual.Mode)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Exposure mode: expected - {0}, actual - {1}",
                                                expected.Mode, actual.Mode));
                }

                // Exposure time

                if (expected.ExposureTimeSpecified != actual.ExposureTimeSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Exposure time specified: expected - {0}, actual - {1}",
                                                expected.ExposureTimeSpecified, actual.ExposureTimeSpecified));
                }

                if (expected.ExposureTimeSpecified && actual.ExposureTimeSpecified)
                {
                    if (expected.ExposureTime != actual.ExposureTime)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Exposure time: expected - {0}, actual - {1}",
                                                    expected.ExposureTime, actual.ExposureTime));
                    }
                }

                // Gain

                if (expected.GainSpecified != actual.GainSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Gain specified: expected - {0}, actual - {1}",
                                                expected.GainSpecified, actual.GainSpecified));
                }

                if (expected.GainSpecified && actual.GainSpecified)
                {
                    if (expected.Gain != actual.Gain)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Gain: expected - {0}, actual - {1}",
                                                    expected.Gain, actual.Gain));
                    }
                }

                // Iris

                if (expected.IrisSpecified != actual.IrisSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Iris specified: expected - {0}, actual - {1}",
                                                expected.IrisSpecified, actual.IrisSpecified));
                }

                if (expected.IrisSpecified && actual.IrisSpecified)
                {
                    if (expected.Iris != actual.Iris)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Iris: expected - {0}, actual - {1}",
                                                    expected.Iris, actual.Iris));
                    }
                }

                // MinExposureTime

                if (expected.MinExposureTimeSpecified != actual.MinExposureTimeSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Min Exposure Time specified: expected - {0}, actual - {1}",
                                                expected.MinExposureTimeSpecified, actual.MinExposureTimeSpecified));
                }

                if (expected.MinExposureTimeSpecified && actual.MinExposureTimeSpecified)
                {
                    if (expected.MinExposureTime != actual.MinExposureTime)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Min Exposure Time: expected - {0}, actual - {1}",
                                                    expected.MinExposureTime, actual.MinExposureTime));
                    }
                }
                
                // MaxExposureTime

                if (expected.MaxExposureTimeSpecified != actual.MaxExposureTimeSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Max Exposure Time specified: expected - {0}, actual - {1}",
                                                expected.MaxExposureTimeSpecified, actual.MaxExposureTimeSpecified));
                }

                if (expected.MaxExposureTimeSpecified && actual.MaxExposureTimeSpecified)
                {
                    if (expected.MaxExposureTime != actual.MaxExposureTime)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Max Exposure Time: expected - {0}, actual - {1}",
                                                    expected.MaxExposureTime, actual.MaxExposureTime));
                    }
                }

                // MinGain

                if (expected.MinGainSpecified != actual.MinGainSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Min Gain specified: expected - {0}, actual - {1}",
                                                expected.MinGainSpecified, actual.MinGainSpecified));
                }

                if (expected.MinGainSpecified && actual.MinGainSpecified)
                {
                    if (expected.MinGain != actual.MinGain)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Min Gain: expected - {0}, actual - {1}",
                                                    expected.MinGain, actual.MinGain));
                    }
                }

                // MaxGain

                if (expected.MaxGainSpecified != actual.MaxGainSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Max Gain specified: expected - {0}, actual - {1}",
                                                expected.MaxGainSpecified, actual.MaxGainSpecified));
                }

                if (expected.MaxGainSpecified && actual.MaxGainSpecified)
                {
                    if (expected.MaxGain != actual.MaxGain)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Max Gain: expected - {0}, actual - {1}",
                                                    expected.MaxGain, actual.MaxGain));
                    }
                }

                // MinIris
                
                if (expected.MinIrisSpecified != actual.MinIrisSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Min Iris specified: expected - {0}, actual - {1}",
                                                expected.MinIrisSpecified, actual.MinIrisSpecified));
                }

                if (expected.MinIrisSpecified && actual.MinIrisSpecified)
                {
                    if (expected.MinIris != actual.MinIris)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Min Iris: expected - {0}, actual - {1}",
                                                    expected.MinIris, actual.MinIris));
                    }
                }

                // MaxIris

                if (expected.MaxIrisSpecified != actual.MaxIrisSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Max Iris specified: expected - {0}, actual - {1}",
                                                expected.MaxIrisSpecified, actual.MaxIrisSpecified));
                }

                if (expected.MaxIrisSpecified && actual.MaxIrisSpecified)
                {
                    if (expected.MaxIris != actual.MaxIris)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Max Iris: expected - {0}, actual - {1}",
                                                    expected.MaxIris, actual.MaxIris));
                    }
                }

                // Priority

                if (expected.PrioritySpecified != actual.PrioritySpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Priority specified: expected - {0}, actual - {1}",
                                                expected.PrioritySpecified, actual.PrioritySpecified));
                }

                if (expected.PrioritySpecified && actual.PrioritySpecified)
                {
                    if (expected.Priority != actual.Priority)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Priority: expected - {0}, actual - {1}",
                                                    expected.Priority, actual.Priority));
                    }
                }


                // Window

                if (actual.Window != null && expected.Window != null)
                {
                    Rectangle actualWindow = actual.Window;
                    Rectangle expectedWindow = expected.Window;

                    sb.AppendLine("Exposure window: ");

                    // bottom 
                    if (expectedWindow.bottomSpecified != actualWindow.bottomSpecified)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Bottom specified: expected - {0}, actual - {1}",
                                                    expectedWindow.bottomSpecified, actualWindow.bottomSpecified));
                    }

                    if (expectedWindow.bottomSpecified && actualWindow.bottomSpecified)
                    {
                        if (expectedWindow.bottom != actualWindow.bottom)
                        {
                            bOk = false;
                            sb.AppendLine(string.Format("Bottom: expected - {0}, actual - {1}",
                                                        expectedWindow.bottom, actualWindow.bottom));
                        }
                    }

                    // top
                    if (expectedWindow.topSpecified != actualWindow.topSpecified)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Top specified: expected - {0}, actual - {1}",
                                                    expectedWindow.topSpecified, actualWindow.topSpecified));
                    }

                    if (expectedWindow.topSpecified && actualWindow.topSpecified)
                    {
                        if (expectedWindow.top != actualWindow.top)
                        {
                            bOk = false;
                            sb.AppendLine(string.Format("Top: expected - {0}, actual - {1}",
                                                        expectedWindow.top, actualWindow.top));
                        }
                    }

                    // left

                    if (expectedWindow.leftSpecified != actualWindow.leftSpecified)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Left specified: expected - {0}, actual - {1}",
                                                    expectedWindow.leftSpecified, actualWindow.leftSpecified));
                    }

                    if (expectedWindow.leftSpecified && actualWindow.leftSpecified)
                    {
                        if (expectedWindow.left != actualWindow.left)
                        {
                            bOk = false;
                            sb.AppendLine(string.Format("Left: expected - {0}, actual - {1}",
                                                        expectedWindow.left, actualWindow.left));
                        }
                    }


                    // right

                    if (expectedWindow.rightSpecified != actualWindow.rightSpecified)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Right specified: expected - {0}, actual - {1}",
                                                    expectedWindow.rightSpecified, actualWindow.rightSpecified));
                    }

                    if (expectedWindow.rightSpecified && actualWindow.rightSpecified)
                    {
                        if (expectedWindow.right != actualWindow.right)
                        {
                            bOk = false;
                            sb.AppendLine(string.Format("Right: expected - {0}, actual - {1}",
                                                        expectedWindow.right, actualWindow.right));
                        }
                    }

                }
                else
                {
                    // Both nulls are OK (excluding tha fact that WE don't send null)
                    if (!(actual.Window == null && expected.Window == null))
                    {
                        sb.AppendLine(string.Format("Exposure Window: {0}",
                                                    CreateNullsDescription(expected.Window,
                                                                           actual.Window)));
                        bOk = false;
                    }
                }


            }
            else
            {
                // Both nulls are OK (excluding the fact that WE don't send null)
                if (!(actualSettings.Exposure == null && expectedSettings.Exposure == null))
                {
                    sb.AppendLine(string.Format("Exposure: {0}",
                                                CreateNullsDescription(expectedSettings.Exposure,
                                                                       actualSettings.Exposure)));
                    bOk = false;
                }
            }


            // Extension
            //if (actualSettings.Extension != null && expectedSettings.Extension != null)
            //{
            //    // ToDo: Any field - need to compare ?
            //}
            //else
            //{
            //    // Both nulls are OK (excluding tha fact that WE don't send null)
            //    if (!(actualSettings.Extension == null && expectedSettings.Extension == null))
            //    {
            //        sb.AppendLine(string.Format("Exposure: {0}",
            //                                    CreateNullsDescription(expectedSettings.Extension,
            //                                                           actualSettings.Extension)));
            //        bOk = false;
            //    }
            //}

            // Focus
            if (actualSettings.Focus != null && expectedSettings.Focus != null)
            {
                // ToDo: need to compare Any ?

                FocusConfiguration20 actualFocus = actualSettings.Focus;
                FocusConfiguration20 expectedFocus = expectedSettings.Focus;

                // Auto Focus Mode
                if (actualFocus.AutoFocusMode != expectedFocus.AutoFocusMode)
                {
                    sb.AppendLine(string.Format("AutoFocus mode: expected - {0}, actual - {1}",
                                                expectedFocus.AutoFocusMode, actualFocus.AutoFocusMode));
                }

                // Default Speed
                if (actualFocus.DefaultSpeedSpecified != expectedFocus.DefaultSpeedSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Default Speed specified: expected - {0}, actual - {1}",
                                                expectedFocus.DefaultSpeedSpecified, actualFocus.DefaultSpeedSpecified));
                }

                if (actualFocus.DefaultSpeedSpecified && expectedFocus.DefaultSpeedSpecified)
                {
                    if (expectedFocus.DefaultSpeed != actualFocus.DefaultSpeed)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Default speed: expected - {0}, actual - {1}",
                                                    expectedFocus.DefaultSpeed, actualFocus.DefaultSpeed));
                    }
                }

                // Extension - only Any [ToDo]
                
                // FarLimit, FarLimitSpecified
                if (actualFocus.FarLimitSpecified != expectedFocus.FarLimitSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Far Limit specified: expected - {0}, actual - {1}",
                                                expectedFocus.FarLimitSpecified, actualFocus.FarLimitSpecified));
                }

                if (actualFocus.FarLimitSpecified && expectedFocus.FarLimitSpecified)
                {
                    if (expectedFocus.FarLimit != actualFocus.FarLimit)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Far Limit: expected - {0}, actual - {1}",
                                                    expectedFocus.FarLimit, actualFocus.FarLimit));
                    }
                }
                
                // NearLimit, NearLimitSpecified

                if (actualFocus.NearLimitSpecified != expectedFocus.NearLimitSpecified)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Near Limit specified: expected - {0}, actual - {1}",
                                                expectedFocus.NearLimitSpecified, actualFocus.NearLimitSpecified));
                }

                if (actualFocus.NearLimitSpecified && expectedFocus.NearLimitSpecified)
                {
                    if (expectedFocus.NearLimit != actualFocus.NearLimit)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Near Limit: expected - {0}, actual - {1}",
                                                    expectedFocus.NearLimit, actualFocus.NearLimit));
                    }
                }
            }
            else
            {
                // Both nulls are OK (excluding tha fact that WE don't send null)
                if (!(actualSettings.Focus == null && expectedSettings.Focus == null))
                {
                    sb.AppendLine(string.Format("Focus: {0}",
                                                CreateNullsDescription(expectedSettings.Focus,
                                                                       actualSettings.Focus)));
                    bOk = false;
                }
            }


            // WhiteBalance
            if (actualSettings.WhiteBalance != null && expectedSettings.WhiteBalance != null)
            {
                WhiteBalance20 expectedWhiteBalance = expectedSettings.WhiteBalance;
                WhiteBalance20 actualWhiteBalance = actualSettings.WhiteBalance;

                if (expectedWhiteBalance.Mode != actualWhiteBalance.Mode)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("White balance mode: expected - {0}, actual - {1}",
                                                expectedWhiteBalance.Mode, actualWhiteBalance.Mode));
                }

                if (expectedWhiteBalance.Mode == WhiteBalanceMode.MANUAL && 
                    actualWhiteBalance.Mode == WhiteBalanceMode.MANUAL )
                {
                    if (expectedWhiteBalance.CbGainSpecified != actualWhiteBalance.CbGainSpecified)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("White balance CbGain specified: expected - {0}, actual - {1}",
                                                    expectedWhiteBalance.CbGainSpecified, actualWhiteBalance.CbGainSpecified));
                    }
                    if (actualWhiteBalance.CbGainSpecified && expectedWhiteBalance.CbGainSpecified)
                    {
                        if (expectedWhiteBalance.CbGain != actualWhiteBalance.CbGain)
                        {
                            bOk = false;
                            sb.AppendLine(string.Format("White balance CbGain: expected - {0}, actual - {1}",
                                                        expectedWhiteBalance.CbGain, actualWhiteBalance.CbGain));
                        }
                    }


                    if (expectedWhiteBalance.CrGainSpecified != actualWhiteBalance.CrGainSpecified)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("White balance CrGain specified: expected - {0}, actual - {1}",
                                                    expectedWhiteBalance.CrGainSpecified, actualWhiteBalance.CrGainSpecified));
                    }
                    if (actualWhiteBalance.CrGainSpecified && expectedWhiteBalance.CrGainSpecified)
                    {
                        if (expectedWhiteBalance.CrGain != actualWhiteBalance.CrGain)
                        {
                            bOk = false;
                            sb.AppendLine(string.Format("White balance CrGain: expected - {0}, actual - {1}",
                                                        expectedWhiteBalance.CrGain, actualWhiteBalance.CrGain));
                        }
                    }
                }
            }
            else
            {
                // Both nulls are OK (excluding tha fact that WE don't send null)
                if (!(actualSettings.WhiteBalance == null && expectedSettings.WhiteBalance == null))
                {
                    sb.AppendLine(string.Format("WhiteBalance: {0}",
                                                CreateNullsDescription(expectedSettings.WhiteBalance,
                                                                       actualSettings.WhiteBalance)));
                    bOk = false;
                }
            }

            // WideDynamicRange
            if (actualSettings.WideDynamicRange != null && expectedSettings.WideDynamicRange != null)
            {
                if (actualSettings.WideDynamicRange.Mode != expectedSettings.WideDynamicRange.Mode)
                {
                    bOk = false;
                    sb.AppendLine(string.Format("Wide Dynamic Range: expected - {0}, actual - {1}",
                                                expectedSettings.WideDynamicRange.Mode, actualSettings.WideDynamicRange.Mode));
                }

                if (actualSettings.WideDynamicRange.Mode == WideDynamicMode.ON && 
                    actualSettings.WideDynamicRange.Mode == WideDynamicMode.ON)
                {

                    if (expectedSettings.WideDynamicRange.LevelSpecified != actualSettings.WideDynamicRange.LevelSpecified)
                    {
                        bOk = false;
                        sb.AppendLine(string.Format("Wide Dynamic Range level specified: expected - {0}, actual - {1}",
                                                    expectedSettings.WideDynamicRange.LevelSpecified, actualSettings.WideDynamicRange.LevelSpecified));
                    }
                    if (actualSettings.WideDynamicRange.LevelSpecified && expectedSettings.WideDynamicRange.LevelSpecified)
                    {
                        if (expectedSettings.WideDynamicRange.Level != actualSettings.WideDynamicRange.Level)
                        {
                            bOk = false;
                            sb.AppendLine(string.Format("Wide Dynamic Range level : expected - {0}, actual - {1}",
                                                        expectedSettings.WideDynamicRange.Level, actualSettings.WideDynamicRange.Level));
                        }
                    }
                }
            }
            else
            {
                // Both nulls are OK (excluding tha fact that WE don't send null)
                if (!(actualSettings.WideDynamicRange == null && expectedSettings.WideDynamicRange == null))
                {
                    sb.AppendLine(string.Format("WideDynamicRange: {0}",
                                                CreateNullsDescription(expectedSettings.WideDynamicRange,
                                                                       actualSettings.WideDynamicRange)));
                    bOk = false;
                }
            }

            dump = sb.ToStringTrimNewLine();

            return bOk;
        }

        string CreateNullsDescription(object object1, object object2)
        {
            return string.Format("expected - {0}, actual - {1}",
                                 object1 == null ? "NULL" : "NOT NULL",
                                 object2 == null ? "NULL" : "NOT NULL");
        }

        bool ValidateFloatRange(FloatRange range, string settingsName, StringBuilder dumpOutput)
        {
            bool ok = true;
            if (range != null)
            {
                ok = ValidateRangeBoudary(range, settingsName, dumpOutput);
            }
            //else
            //{
            //    ok = false;
            //    dumpOutput.AppendLine(string.Format("No {0} options", settingsName));
            //}
            return ok;
        }

        bool ValidateFloatRange(FloatRange range, 
            string settingsName, 
            bool required,
            StringBuilder dumpOutput,
            string optionsRequired, 
            string optionsNotAllowed)
        {
            bool ok = true;
            if (range == null)
            {
                if (required)
                {
                    ok = false;
                    dumpOutput.AppendLine(optionsRequired);
                }
            }
            else
            {
                if (!required)
                {
                    ok = false;
                    dumpOutput.AppendLine(optionsNotAllowed);
                }
                else
                {
                    ok = ValidateRangeBoudary(range, settingsName, dumpOutput);
                }
            }
            return ok;
        }

        bool ValidateRangeBoudary(FloatRange range, string settingsName, StringBuilder dumpOutput)
        {
            if (range.Min > range.Max)
            {
                dumpOutput.AppendLine(string.Format("Range is invalid for {0}", settingsName));
            }
            return (range.Min <= range.Max);
        }

        #endregion
    
        bool ValidateInvalidMoveFault(FaultException fault, out string reason)
        {
            reason = fault == null ? "No SOAP fault received"  : string.Empty;
            return (fault != null);
        }

        bool ValidateInvalidTokenFault(FaultException fault, out string reason)
        {
            if (fault == null)
            {
                reason = "No SOAP fault received";
                return false;
            }

            bool ok = fault.IsValidOnvifFault("Sender/InvalidArgVal/NoSource", out reason);
            if (!ok)
            {
                LogStepEvent("WARNING: fault received is not Sender/InvalidArgVal/NoSource fault.");
            }
            reason = string.Empty;
            return true;
        }
    }

}
