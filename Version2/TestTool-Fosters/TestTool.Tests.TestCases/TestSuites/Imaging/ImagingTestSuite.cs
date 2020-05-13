using System.Linq;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Proxies.Onvif;
using System.ServiceModel;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public partial class ImagingTestSuite : ImagingTest
    {

        public ImagingTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH_SETTINGS = "Imaging\\Imaging Settings";
        private const string PATH_FOCUSMOVE = "Imaging\\Focus Move";

        #region Settings

        [Test(Name = "IMAGING COMMAND GETIMAGINGSETTINGS",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.IMAGING,
            Path = PATH_SETTINGS,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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

        [Test(Name = "IMAGING COMMAND GETIMAGINGSETTINGS – INVALID VIDEOSOURCETOKEN",
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.IMAGING,
            Path = PATH_SETTINGS,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[]{Feature.ImagingService})]
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
            Path = PATH_SETTINGS,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Path = PATH_SETTINGS,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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

        [Test(Name = "IMAGING COMMAND SETIMAGINGSETTINGS",
            Order = "01.01.06",
            Id = "1-1-6",
            Category = Category.IMAGING,
            Path = PATH_SETTINGS,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
                            bool bEqual = ImagingSettingsEqual(settings, actualSettings, true, out dump);
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

        [Test(Name = "IMAGING COMMAND SETIMAGINGSETTINGS – INVALID VIDEOSOURCETOKEN",
            Order = "01.01.07",
            Id = "1-1-7",
            Category = Category.IMAGING,
            Path = PATH_SETTINGS,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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

        [Test(Name = "IMAGING COMMAND SETIMAGINGSETTINGS – INVALID SETTINGS",
            Order = "01.01.08",
            Id = "1-1-8",
            Category = Category.IMAGING,
            Path = PATH_SETTINGS,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
                        bool bEqual = ImagingSettingsEqual(initialSettings, actualSettings, false, out dump);
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

#endregion

        #region Move

        [Test(Name = "IMAGING COMMAND GETMOVEOPTIONS",
            Order = "02.01.01",
            Id = "2-1-1",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.02",
            Id = "2-1-2",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.03",
            Id = "2-1-3",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.04",
            Id = "2-1-4",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.05",
            Id = "2-1-5",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.06",
            Id = "2-1-6",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.07",
            Id = "2-1-7",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.08",
            Id = "2-1-8",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.09",
            Id = "2-1-9",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.10",
            Id = "2-1-10",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.11",
            Id = "2-1-11",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.12",
            Id = "2-1-12",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.13",
            Id = "2-1-13",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
            Order = "02.01.14",
            Id = "2-1-14",
            Category = Category.IMAGING,
            Path = PATH_FOCUSMOVE,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
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
