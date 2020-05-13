using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
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
            Order = "01.01.10",
            Id = "1-1-10",
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
            Order = "01.01.11",
            Id = "1-1-11",
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

        //[Test(Name = "IMAGING COMMAND SETIMAGINGSETTINGS",
        //    Order = "01.01.06",
        //    Id = "1-1-6",
        //    Category = Category.IMAGING,
        //    Path = PATH_SETTINGS,
        //    Version = 2.0,
        //    RequirementLevel = RequirementLevel.Must,
        //    RequiredFeatures = new Feature[] { Feature.ImagingService })]
        //public void SetImagingSettingsTest()
        //{
        //    ImagingSettings20 backup = null;
        //    string token = string.Empty;

        //    RunTest(
        //        () =>
        //            {
        //                VideoSource[] sources = GetAndValidateVideoSources();

        //                for (int i = 0; i < sources.Length; i++)
        //                {
        //                    ImagingOptions20 options = GetOptions(sources[i].token);

        //                    if (options == null)
        //                    {
        //                        continue;
        //                    }

        //                    ValidateImagingOptions(options);

        //                    ImagingSettings20 initialSettings = GetImagingSettings(sources[i].token);

        //                    ImagingSettings20 settings = GenerateImagingSettings(options, initialSettings);

        //                    SetImagingSettings(sources[i].token, settings, false);

        //                    ImagingSettings20 actualSettings = GetImagingSettings(sources[i].token);

        //                    // compare settings
        //                    string dump;
        //                    bool bEqual = ImagingSettingsEqual(settings, actualSettings, true, out dump);
        //                    if (!bEqual)
        //                    {
        //                        // save for restoring
        //                        // if equal, restoring will be done after passing Assert
        //                        backup = initialSettings;
        //                        token = sources[i].token;
        //                    }
        //                    Assert(bEqual,
        //                           string.Format("Expected and actual settings are different.\n{0}",
        //                                         dump), "Compare settings");

        //                    SetImagingSettings(sources[i].token, initialSettings, false, "Restore imaging settings");

        //                }
        //            }, 
        //            () =>
        //                {
        //                    if (backup != null)
        //                    {
        //                        SetImagingSettings(token, backup, false, "Restore imaging settings");
        //                    }
        //                });
        //}

        internal void ValidateImagingSettings20Property<T>(ImagingSettings20 expected, ImagingSettings20 actual,
                                                           Func<ImagingSettings20, bool> specifiedSelector,
                                                           Func<ImagingSettings20, T> valueSelector,
                                                           Func<T, T, bool> comparator,
                                                           string name)
        {
            var log = new StringBuilder();
            bool flag = ValidateValueProperty(expected, actual, specifiedSelector, valueSelector, comparator, name, log);

            Assert(flag, log.ToString().TrimEnd(), string.Format("Check setting '{0}' is applied", name));
        }

        [Test(Name = "IMAGING COMMAND SETIMAGINGSETTINGS",
            Order = "01.01.09",
            Id = "1-1-9",
            Category = Category.IMAGING,
            Path = PATH_SETTINGS,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ImagingService })]
        public void SetImagingSettingsTest()
        {
            ImagingSettings20 backup = null;
            string token = string.Empty;
            bool restoreSettings = false;

            RunTest(() =>
                    {
                        var chooseNewFloatParameterValue = new Func<float, float, float, float>(
                            (s, e, v) =>
                            {
                                if (Math.Abs(v - s) > Math.Abs(e - v)) return s;
                                else return e;
                            });

                        foreach (var videoSource in GetAndValidateVideoSources())
                        {
                            var setAndGetNewSettings = new Func<ImagingSettings20, ImagingSettings20>(
                                (s) =>
                                {
                                    SetImagingSettings(videoSource.token, s, false);

                                    return GetImagingSettings(videoSource.token);
                                });

                            Func<ImagingSettings20, ImagingSettings20> copyImagingOptions = 
                                (o) =>
                                    {
                                        XmlSerializer serializer = new XmlSerializer(typeof(ImagingSettings20));
                                        var stream = new MemoryStream();
                                        serializer.Serialize(stream, o);
                                        stream.Flush();
                                        stream.Seek(0, SeekOrigin.Begin);
                                        return (ImagingSettings20)serializer.Deserialize(stream);
                                    };

                            //Step 3.
                            ImagingOptions20 options = GetOptions(videoSource.token);

                            if (null == options)
                            {
                                continue;
                            }

                            //Step 4.
                            ValidateImagingOptions(options);

                            //Step 5.
                            ImagingSettings20 initialSettings = GetImagingSettings(videoSource.token);

                            if (null == initialSettings)
                                continue;

                            ImagingSettings20 currentSettings = copyImagingOptions(initialSettings);
                            //backup = GetImagingSettings(videoSource.token);
                            backup = copyImagingOptions(initialSettings);
                            token = videoSource.token;
                            restoreSettings = true;

                            //Step 6. TO DO

                            //Step 7.
                            if (null != options.BacklightCompensation)
                            {
                                //Step 8.
                                if (options.BacklightCompensation.Mode.Count() == 2)
                                {
                                    if (null == currentSettings.BacklightCompensation)
                                        currentSettings.BacklightCompensation = new BacklightCompensation20() { Mode = BacklightCompensationMode.ON };
                                    else
                                        currentSettings.BacklightCompensation.Mode = (initialSettings.BacklightCompensation.Mode == BacklightCompensationMode.ON)
                                                                                      ? BacklightCompensationMode.OFF : BacklightCompensationMode.ON;

                                    ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                      s => null != s.BacklightCompensation,
                                                                      s => s.BacklightCompensation.Mode,
                                                                      (l, r) => l == r,
                                                                      "Backlight Compensation/Mode");
                                }

                                //Step 13.
                                if (null != options.BacklightCompensation.Level &&
                                    (options.BacklightCompensation.Level.Max > options.BacklightCompensation.Level.Min
                                     && options.BacklightCompensation.Mode.Contains(BacklightCompensationMode.ON)))
                                {
                                    if (null == currentSettings.BacklightCompensation)
                                        currentSettings.BacklightCompensation = new BacklightCompensation20();

                                    currentSettings.BacklightCompensation.Mode = BacklightCompensationMode.ON;
                                    currentSettings.BacklightCompensation.LevelSpecified = true;
                                    currentSettings.BacklightCompensation.Level = chooseNewFloatParameterValue(options.BacklightCompensation.Level.Min,
                                                                                                               options.BacklightCompensation.Level.Max,
                                                                                                               currentSettings.BacklightCompensation.Level);

                                    ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                      s => null != s.BacklightCompensation && s.BacklightCompensation.LevelSpecified,
                                                                      s => s.BacklightCompensation.Level,
                                                                      (l, r) => l == r,
                                                                      "Backlight Compensation/Level");
                                }
                            }

                            foreach (var optionName in new[] { "Brightness", "ColorSaturation", "Contrast", "Sharpness" })
                            {
                                var option = (FloatRange)options.GetType().InvokeMember(optionName, BindingFlags.GetProperty, null, options, null);
                                if (null != option && option.Max > option.Min)
                                {
                                    var currentValue = (float)initialSettings.GetType().InvokeMember(optionName, BindingFlags.GetProperty, null, initialSettings, null);
                                    var newValue = chooseNewFloatParameterValue(option.Min, option.Max, currentValue);

                                    currentSettings.GetType().InvokeMember(optionName + "Specified", BindingFlags.SetProperty, null, currentSettings, new object[] { true });

                                    currentSettings.GetType().InvokeMember(optionName, BindingFlags.SetProperty, null, currentSettings, new object[] { newValue });

                                    ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                      s => (bool)s.GetType().GetProperty(optionName + "Specified").GetValue(s, null),
                                                                      s => (float)s.GetType().GetProperty(optionName).GetValue(s, null),
                                                                      (l, r) => l == r,
                                                                      optionName);
                                }
                            }

                            //Step 33.
                            if (null != options.Exposure)
                            {
                                if (null == currentSettings.Exposure)
                                    currentSettings.Exposure = new Exposure20();

                                if (2 == options.Exposure.Mode.Count())
                                {
                                    currentSettings.Exposure.Mode = (initialSettings.Exposure.Mode == ExposureMode.AUTO) ? ExposureMode.MANUAL : ExposureMode.AUTO;

                                    ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                      s => null != s.Exposure,
                                                                      s => s.Exposure.Mode,
                                                                      (l, r) => l == r,
                                                                      "Exposure/Mode");
                                }

                                if (options.Exposure.Mode.Contains(ExposureMode.AUTO))
                                {
                                    //Step 40.
                                    var priority = options.Exposure.Priority;
                                    if (null != priority && priority.Max() > priority.Min())
                                    {
                                        currentSettings.Exposure.Mode = ExposureMode.AUTO;
                                        currentSettings.Exposure.PrioritySpecified = true;
                                        currentSettings.Exposure.Priority = (initialSettings.Exposure.Priority ==
                                                                             ExposurePriority.FrameRate)
                                                                                ? ExposurePriority.LowNoise
                                                                                : ExposurePriority.FrameRate;

                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.Exposure && s.Exposure.PrioritySpecified,
                                                                          s => s.Exposure.Priority,
                                                                          (l, r) => l == r,
                                                                          "Exposure/Priority");

                                    }

                                    //Step 45.
                                    if (null != options.Exposure.MinExposureTime && options.Exposure.MinExposureTime.Max > options.Exposure.MinExposureTime.Min)
                                    {
                                        currentSettings.Exposure.Mode = ExposureMode.AUTO;
                                        currentSettings.Exposure.MinExposureTimeSpecified = true;
                                        currentSettings.Exposure.MinExposureTime = chooseNewFloatParameterValue(options.Exposure.MinExposureTime.Max,
                                                                                                                options.Exposure.MinExposureTime.Min,
                                                                                                                currentSettings.Exposure.MinExposureTime);

                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.Exposure && s.Exposure.MinExposureTimeSpecified,
                                                                          s => s.Exposure.MinExposureTime,
                                                                          (l, r) => l == r,
                                                                          "Exposure/Min Exposure Time");
                                    }

                                    //Step 50.
                                    if (null != options.Exposure.MaxExposureTime && options.Exposure.MaxExposureTime.Max > options.Exposure.MaxExposureTime.Min)
                                    {
                                        currentSettings.Exposure.Mode = ExposureMode.AUTO;
                                        currentSettings.Exposure.MaxExposureTimeSpecified = true;
                                        currentSettings.Exposure.MaxExposureTime = chooseNewFloatParameterValue(options.Exposure.MaxExposureTime.Max,
                                                                                                                options.Exposure.MaxExposureTime.Min,
                                                                                                                currentSettings.Exposure.MaxExposureTime);

                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.Exposure && s.Exposure.MaxExposureTimeSpecified,
                                                                          s => s.Exposure.MaxExposureTime,
                                                                          (l, r) => l == r,
                                                                          "Exposure/Max Exposure Time");

                                    }

                                    //Step 55.
                                    if (null != options.Exposure.MinGain && options.Exposure.MinGain.Max > options.Exposure.MinGain.Min)
                                    {
                                        currentSettings.Exposure.Mode = ExposureMode.AUTO;
                                        currentSettings.Exposure.MinGainSpecified = true;
                                        currentSettings.Exposure.MinGain = chooseNewFloatParameterValue(options.Exposure.MinGain.Max,
                                                                                                        options.Exposure.MinGain.Min,
                                                                                                        currentSettings.Exposure.MinGain);

                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.Exposure && s.Exposure.MinGainSpecified,
                                                                          s => s.Exposure.MinGain,
                                                                          (l, r) => l == r,
                                                                          "Exposure/Min Gain");

                                    }

                                    //Step 60.
                                    if (null != options.Exposure.MaxGain && options.Exposure.MaxGain.Max > options.Exposure.MaxGain.Min)
                                    {
                                        currentSettings.Exposure.Mode = ExposureMode.AUTO;
                                        currentSettings.Exposure.MaxGainSpecified = true;
                                        currentSettings.Exposure.MaxGain = chooseNewFloatParameterValue(options.Exposure.MaxGain.Max,
                                                                                                        options.Exposure.MaxGain.Min,
                                                                                                        currentSettings.Exposure.MaxGain);

                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.Exposure && s.Exposure.MaxGainSpecified,
                                                                          s => s.Exposure.MaxGain,
                                                                          (l, r) => l == r,
                                                                          "Exposure/Max Gain");
                                    }

                                    //Step 65.
                                    if (null != options.Exposure.MinIris && options.Exposure.MinIris.Max > options.Exposure.MinIris.Min)
                                    {
                                        currentSettings.Exposure.Mode = ExposureMode.AUTO;
                                        currentSettings.Exposure.MinIrisSpecified = true;
                                        currentSettings.Exposure.MinIris = chooseNewFloatParameterValue(options.Exposure.MinIris.Max,
                                                                                                        options.Exposure.MinIris.Min,
                                                                                                        currentSettings.Exposure.MinIris);

                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.Exposure && s.Exposure.MinIrisSpecified,
                                                                          s => s.Exposure.MinIris,
                                                                          (l, r) => l == r,
                                                                          "Exposure/Min Iris");
                                    }

                                    //Step 70.
                                    if (null != options.Exposure.MaxIris && options.Exposure.MaxIris.Max > options.Exposure.MaxIris.Min)
                                    {
                                        currentSettings.Exposure.Mode = ExposureMode.AUTO;
                                        currentSettings.Exposure.MaxIrisSpecified = true;
                                        currentSettings.Exposure.MaxIris = chooseNewFloatParameterValue(options.Exposure.MaxIris.Max,
                                                                                                        options.Exposure.MaxIris.Min,
                                                                                                        currentSettings.Exposure.MaxIris);

                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.Exposure && s.Exposure.MaxIrisSpecified,
                                                                          s => s.Exposure.MaxIris,
                                                                          (l, r) => l == r,
                                                                          "Exposure/Max Iris");
                                    }
                                }

                                //Step 75.
                                if (options.Exposure.Mode.Contains(ExposureMode.MANUAL))
                                {
                                    //Step 76.
                                    if (null != options.Exposure.ExposureTime && options.Exposure.ExposureTime.Max > options.Exposure.ExposureTime.Min)
                                    {
                                        currentSettings.Exposure.Mode = ExposureMode.MANUAL;
                                        currentSettings.Exposure.ExposureTimeSpecified = true;
                                        currentSettings.Exposure.ExposureTime = chooseNewFloatParameterValue(options.Exposure.ExposureTime.Max,
                                                                                                             options.Exposure.ExposureTime.Min,
                                                                                                             currentSettings.Exposure.ExposureTime);

                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.Exposure && s.Exposure.ExposureTimeSpecified,
                                                                          s => s.Exposure.ExposureTime,
                                                                          (l, r) => l == r,
                                                                          "Exposure/Exposure Time");
                                    }

                                    //Step 81.
                                    if (null != options.Exposure.Gain && options.Exposure.Gain.Max > options.Exposure.Gain.Min)
                                    {
                                        currentSettings.Exposure.Mode = ExposureMode.MANUAL;
                                        currentSettings.Exposure.GainSpecified = true;
                                        currentSettings.Exposure.Gain = chooseNewFloatParameterValue(options.Exposure.Gain.Max,
                                                                                                     options.Exposure.Gain.Min,
                                                                                                     currentSettings.Exposure.Gain);

                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.Exposure && s.Exposure.GainSpecified,
                                                                          s => s.Exposure.Gain,
                                                                          (l, r) => l == r,
                                                                          "Exposure/Gain");
                                    }

                                    //Step 86.
                                    if (null != options.Exposure.Iris && options.Exposure.Iris.Max > options.Exposure.Iris.Min)
                                    {
                                        currentSettings.Exposure.Mode = ExposureMode.MANUAL;
                                        currentSettings.Exposure.IrisSpecified = true;
                                        currentSettings.Exposure.Iris = chooseNewFloatParameterValue(options.Exposure.Iris.Max,
                                                                                                     options.Exposure.Iris.Min,
                                                                                                     currentSettings.Exposure.Iris);

                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.Exposure && s.Exposure.IrisSpecified,
                                                                          s => s.Exposure.Iris,
                                                                          (l, r) => l == r,
                                                                          "Exposure/Iris");
                                    }
                                }
                            }

                            //Step 91.
                            if (null != options.Focus)
                            {
                                if (null == currentSettings.Focus)
                                    currentSettings.Focus = new FocusConfiguration20();

                                var focus = options.Focus;
                                if (2 <= focus.AutoFocusModes.Distinct().Count())
                                {
                                    currentSettings.Focus.AutoFocusMode = (AutoFocusMode.AUTO == currentSettings.Focus.AutoFocusMode) ? AutoFocusMode.MANUAL : AutoFocusMode.AUTO;

                                    ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                      s => null != s.Focus,
                                                                      s => s.Focus.AutoFocusMode,
                                                                      (l, r) => l == r,
                                                                      "Focus/AutoFocusMode");
                                }

                                if (focus.AutoFocusModes.Contains(AutoFocusMode.AUTO))
                                {
                                    if (null != focus.NearLimit && focus.NearLimit.Max > focus.NearLimit.Min)
                                    {
                                        currentSettings.Focus.AutoFocusMode = AutoFocusMode.AUTO;
                                        currentSettings.Focus.NearLimitSpecified = true;
                                        currentSettings.Focus.NearLimit = chooseNewFloatParameterValue(focus.NearLimit.Max,
                                                                                                       focus.NearLimit.Min,
                                                                                                       currentSettings.Focus.NearLimit);

                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.Focus && s.Focus.NearLimitSpecified,
                                                                          s => s.Focus.NearLimit,
                                                                          (l, r) => l == r,
                                                                          "Focus/Near Limit");
                                    }

                                    if (null != focus.FarLimit && focus.FarLimit.Max > focus.FarLimit.Min)
                                    {
                                        currentSettings.Focus.AutoFocusMode = AutoFocusMode.AUTO;
                                        currentSettings.Focus.FarLimitSpecified = true;
                                        currentSettings.Focus.FarLimit = chooseNewFloatParameterValue(focus.FarLimit.Max,
                                                                                                      focus.FarLimit.Min,
                                                                                                      currentSettings.Focus.FarLimit);

                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.Focus && s.Focus.FarLimitSpecified,
                                                                          s => s.Focus.FarLimit,
                                                                          (l, r) => l == r,
                                                                          "Focus/Far Limit");

                                    }
                                }

                                if (focus.AutoFocusModes.Contains(AutoFocusMode.MANUAL)
                                    && (null != focus.DefaultSpeed && focus.DefaultSpeed.Max > focus.DefaultSpeed.Min))
                                {
                                    currentSettings.Focus.AutoFocusMode = AutoFocusMode.MANUAL;
                                    currentSettings.Focus.DefaultSpeedSpecified = true;
                                    currentSettings.Focus.DefaultSpeed = chooseNewFloatParameterValue(focus.DefaultSpeed.Max, 
                                                                                                      focus.DefaultSpeed.Min,
                                                                                                      currentSettings.Focus.DefaultSpeed);

                                    ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                      s => null != s.Focus && s.Focus.DefaultSpeedSpecified,
                                                                      s => s.Focus.DefaultSpeed,
                                                                      (l, r) => l == r,
                                                                      "Focus/Default Speed");

                                }
                            }

                            //Step 113.
                            if (null != options.IrCutFilterModes && options.IrCutFilterModes.Distinct().Count() >= 2)
                            {
                                currentSettings.IrCutFilterSpecified = true;
                                currentSettings.IrCutFilter = options.IrCutFilterModes.First(e => e != currentSettings.IrCutFilter);
                            

                                ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                  s => s.IrCutFilterSpecified,
                                                                  s => s.IrCutFilter,
                                                                  (l, r) => l == r,
                                                                  "IrCutFilter/Mode");

                            }

                            //Step 123.
                            if (null != options.WhiteBalance)
                            {
                                if (null == currentSettings.WhiteBalance)
                                    currentSettings.WhiteBalance = new WhiteBalance20();

                                var whiteBalance = options.WhiteBalance;
                                if (null != whiteBalance.Mode && whiteBalance.Mode.Distinct().Count() >= 2)
                                {
                                    currentSettings.WhiteBalance.Mode = (WhiteBalanceMode.AUTO == currentSettings.WhiteBalance.Mode) ? WhiteBalanceMode.MANUAL : WhiteBalanceMode.AUTO;

                                    ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                      s => null != s.WhiteBalance,
                                                                      s => s.WhiteBalance.Mode,
                                                                      (l, r) => l == r,
                                                                      "WhiteBalance/Mode");

                                }

                                if (null != whiteBalance.Mode && whiteBalance.Mode.Contains(WhiteBalanceMode.MANUAL))
                                {
                                    if (null != whiteBalance.YrGain && whiteBalance.YrGain.Max > whiteBalance.YrGain.Min)
                                    {
                                        currentSettings.WhiteBalance.Mode = WhiteBalanceMode.MANUAL;
                                        currentSettings.WhiteBalance.CrGainSpecified = true;
                                        currentSettings.WhiteBalance.CrGain = chooseNewFloatParameterValue(whiteBalance.YrGain.Max,
                                                                                                           whiteBalance.YrGain.Min,
                                                                                                           currentSettings.WhiteBalance.CrGain);


                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.WhiteBalance && s.WhiteBalance.CrGainSpecified,
                                                                          s => s.WhiteBalance.CrGain,
                                                                          (l, r) => l == r,
                                                                          "WhiteBalance/CrGain");

                                    }

                                    if (null != whiteBalance.YbGain && whiteBalance.YbGain.Max > whiteBalance.YbGain.Min)
                                    {
                                        currentSettings.WhiteBalance.Mode = WhiteBalanceMode.MANUAL;
                                        currentSettings.WhiteBalance.CbGainSpecified = true;
                                        currentSettings.WhiteBalance.CbGain = chooseNewFloatParameterValue(whiteBalance.YbGain.Max,
                                                                                                           whiteBalance.YbGain.Min,
                                                                                                           currentSettings.WhiteBalance.CbGain);


                                        ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                          s => null != s.WhiteBalance && s.WhiteBalance.CbGainSpecified,
                                                                          s => s.WhiteBalance.CbGain,
                                                                          (l, r) => l == r,
                                                                          "WhiteBalance/CbGain");

                                    }
                                }
                            }

                            //Step 140.
                            if (null != options.WideDynamicRange)
                            {
                                if (null == currentSettings.WideDynamicRange)
                                    currentSettings.WideDynamicRange = new WideDynamicRange20();

                                var wideDynamicRange = options.WideDynamicRange;
                                if (null != wideDynamicRange.Mode && 2 == wideDynamicRange.Mode.Count())
                                {
                                    currentSettings.WideDynamicRange.Mode = (WideDynamicMode.ON == currentSettings.WideDynamicRange.Mode) ? WideDynamicMode.OFF : WideDynamicMode.ON;

                                    ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                      s => null != s.WideDynamicRange,
                                                                      s => s.WideDynamicRange.Mode,
                                                                      (l, r) => l == r,
                                                                      "WideDynamicRange/Mode");
                                }

                                if (null != wideDynamicRange.Level 
                                    && (wideDynamicRange.Level.Max > wideDynamicRange.Level.Min && wideDynamicRange.Mode.Contains(WideDynamicMode.ON)))
                                {
                                    currentSettings.WideDynamicRange.Mode = WideDynamicMode.ON;
                                    currentSettings.WideDynamicRange.LevelSpecified = true;
                                    currentSettings.WideDynamicRange.Level = chooseNewFloatParameterValue(wideDynamicRange.Level.Max,
                                                                                                          wideDynamicRange.Level.Min,
                                                                                                          currentSettings.WideDynamicRange.Level);


                                    ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                      s => null != s.WideDynamicRange && s.WideDynamicRange.LevelSpecified,
                                                                      s => s.WideDynamicRange.Level,
                                                                      (l, r) => l == r,
                                                                      "WideDynamicRange/Level");
                                }
                            }

                            //Step 151.
                            if (null != options.Extension && null != options.Extension.ImageStabilization)
                            {
                                if (null == currentSettings.Extension)
                                    currentSettings.Extension = new ImagingSettingsExtension20
                                                                { ImageStabilization = new ImageStabilization() };
                                else if (null == currentSettings.Extension)
                                    currentSettings.Extension.ImageStabilization = new ImageStabilization();

                                var imageStabilization = options.Extension.ImageStabilization;
                                if (null != imageStabilization.Mode && imageStabilization.Mode.Distinct().Count() >= 2)
                                {
                                    currentSettings.Extension.ImageStabilization.Mode = imageStabilization.Mode.First(e => e != currentSettings.Extension.ImageStabilization.Mode);

                                    ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                      s => null != s.Extension && null != s.Extension.ImageStabilization,
                                                                      s => s.Extension.ImageStabilization.Mode,
                                                                      (l, r) => l == r,
                                                                      "ImageStabilization/Mode");
                                }

                                if (null != imageStabilization.Mode &&
                                    imageStabilization.Mode.Contains(ImageStabilizationMode.ON)
                                    && null != imageStabilization.Level &&
                                    imageStabilization.Level.Max > imageStabilization.Level.Min)
                                {
                                    currentSettings.Extension.ImageStabilization.Mode = ImageStabilizationMode.ON;
                                    currentSettings.Extension.ImageStabilization.LevelSpecified = true;
                                    currentSettings.Extension.ImageStabilization.Level = chooseNewFloatParameterValue(imageStabilization.Level.Max,
                                                                                                                      imageStabilization.Level.Min,
                                                                                                                      currentSettings.Extension.ImageStabilization.Level);


                                    ValidateImagingSettings20Property(currentSettings, setAndGetNewSettings(currentSettings),
                                                                      s => null != s.Extension && null != s.Extension.ImageStabilization && s.Extension.ImageStabilization.LevelSpecified,
                                                                      s => s.Extension.ImageStabilization.Level,
                                                                      (l, r) => l == r,
                                                                      "ImageStabilization/Level");
                                }
                            }

                            restoreSettings = false;
                            SetImagingSettings(videoSource.token, initialSettings, false, "Restore imaging settings");
                        }
                    },
                    () =>
                    {
                        if (backup != null && restoreSettings)
                        {
                            SetImagingSettings(token, backup, false, "Restore imaging settings");
                        }
                    });
        }

        [Test(Name = "IMAGING COMMAND SETIMAGINGSETTINGS – INVALID VIDEOSOURCETOKEN",
            Order = "01.01.12",
            Id = "1-1-12",
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
            Order = "02.01.15",
            Id = "2-1-15",
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
            Order = "02.01.16",
            Id = "2-1-16",
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
            Order = "02.01.17",
            Id = "2-1-17",
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
            Order = "02.01.18",
            Id = "2-1-18",
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
