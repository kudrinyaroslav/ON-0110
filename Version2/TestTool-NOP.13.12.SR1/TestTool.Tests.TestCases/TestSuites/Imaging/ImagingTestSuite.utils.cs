using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;

namespace TestTool.Tests.TestCases.TestSuites
{
    partial class ImagingTestSuite
    {

        #region Utils

        /// <summary>
        /// Gets and validates video sources
        /// </summary>
        /// <returns></returns>
        VideoSource[] GetAndValidateVideoSources()
        {
            VideoSource[] sources = GetVideoSources();

            bool empty = (sources == null) || (sources.Length == 0);
            Assert(!empty, "No video sources received from the DUT", "Check that the DUT returned video sources");

            return sources;
        }

        /// <summary>
        /// Generates imaging settings different from initially received.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="initialSettings"></param>
        /// <returns></returns>
        protected ImagingSettings20 GenerateImagingSettings(ImagingOptions20 options,
            ImagingSettings20 initialSettings)
        {
            ImagingSettings20 settings = new ImagingSettings20();

            if (options.BacklightCompensation != null)
            {
                settings.BacklightCompensation = new BacklightCompensation20();

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
                        if (options.BacklightCompensation.Level != null)
                        {
                            settings.BacklightCompensation.LevelSpecified = true;

                            settings.BacklightCompensation.Level =
                                GetSettingsValue(initialSettings.BacklightCompensation.LevelSpecified,
                                                 initialSettings.BacklightCompensation.Level,
                                                 options.BacklightCompensation.Level);
                        }
                    }
                }
                else
                {
                    BacklightCompensationMode backlightCompensationMode = options.BacklightCompensation.Mode[0];
                    settings.BacklightCompensation.Mode = backlightCompensationMode;
                    if (backlightCompensationMode == BacklightCompensationMode.ON && options.BacklightCompensation.Level != null)
                    {
                        settings.BacklightCompensation.LevelSpecified = true;
                        settings.BacklightCompensation.Level = options.BacklightCompensation.Level.Min;
                    }
                }
            }

            if (options.Brightness != null)
            {
                settings.BrightnessSpecified = true;

                settings.Brightness = GetSettingsValue(initialSettings.BrightnessSpecified,
                                                            initialSettings.Brightness,
                                                            options.Brightness);
            }
            else
            {
                settings.BrightnessSpecified = false;
            }

            if (options.ColorSaturation != null)
            {
                settings.ColorSaturationSpecified = true;

                settings.ColorSaturation = GetSettingsValue(initialSettings.ColorSaturationSpecified,
                                                                 initialSettings.ColorSaturation,
                                                                 options.ColorSaturation);
            }
            else
            {
                settings.ColorSaturationSpecified = false;
            }

            if (options.Contrast != null)
            {
                settings.ContrastSpecified = true;
                settings.Contrast = this.GetSettingsValue(initialSettings.ContrastSpecified, initialSettings.Contrast, options.Contrast);
            }
            else
            {
                settings.ContrastSpecified = false;
            }


            if (options.Exposure != null)
            {
                settings.Exposure = new Exposure20();

                ExposureMode mode = ExposureMode.AUTO;

                if (options.Exposure.Mode != null)
                {
                    mode = options.Exposure.Mode[0]; // first
                    // or different;
                    if (initialSettings.Exposure != null)
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
                }
                else
                {
                    if (initialSettings.Exposure != null)
                    {
                        mode = initialSettings.Exposure.Mode;
                    }
                }

                settings.Exposure.Mode = mode;

                if (initialSettings.Exposure != null)
                {
                    if (mode == ExposureMode.AUTO)
                    {
                        if (options.Exposure.Priority != null)
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
                        }

                        if (options.Exposure.MinExposureTime != null)
                        {
                            settings.Exposure.MinExposureTimeSpecified = true;

                            settings.Exposure.MinExposureTime =
                                this.GetSettingsValue(initialSettings.Exposure.MinExposureTimeSpecified,
                                                      initialSettings.Exposure.MinExposureTime,
                                                      options.Exposure.MinExposureTime);

                        }

                        if (options.Exposure.MaxExposureTime != null)
                        {
                            settings.Exposure.MaxExposureTimeSpecified = true;
                            settings.Exposure.MaxExposureTime =
                                this.GetSettingsValue(initialSettings.Exposure.MaxExposureTimeSpecified,
                                                      initialSettings.Exposure.MaxExposureTime,
                                                      options.Exposure.MaxExposureTime);
                        }

                        if (options.Exposure.MaxGain != null)
                        {
                            settings.Exposure.MaxGainSpecified = true;
                            settings.Exposure.MaxGain = this.GetSettingsValue(
                                initialSettings.Exposure.MaxGainSpecified, initialSettings.Exposure.MaxGain,
                                options.Exposure.MaxGain);
                        }

                        if (options.Exposure.MinGain != null)
                        {
                            settings.Exposure.MinGainSpecified = true;
                            settings.Exposure.MinGain = this.GetSettingsValue(
                                initialSettings.Exposure.MinGainSpecified, initialSettings.Exposure.MinGain,
                                options.Exposure.MinGain);
                        }

                        if (options.Exposure.MinIris != null)
                        {
                            settings.Exposure.MinIrisSpecified = true;
                            settings.Exposure.MinIris = this.GetSettingsValue(
                                initialSettings.Exposure.MinIrisSpecified, initialSettings.Exposure.MinIris,
                                options.Exposure.MinIris);
                        }

                        if (options.Exposure.MaxIris != null)
                        {
                            settings.Exposure.MaxIrisSpecified = true;
                            settings.Exposure.MaxIris = this.GetSettingsValue(
                                initialSettings.Exposure.MaxIrisSpecified, initialSettings.Exposure.MaxIris,
                                options.Exposure.MaxIris);
                        }
                    }

                    if (mode == ExposureMode.MANUAL)
                    {
                        if (options.Exposure.ExposureTime != null)
                        {
                            settings.Exposure.ExposureTimeSpecified = true;
                            settings.Exposure.ExposureTime = this.GetSettingsValue(initialSettings.Exposure.ExposureTimeSpecified, initialSettings.Exposure.ExposureTime, options.Exposure.ExposureTime);
                        }

                        if (options.Exposure.Gain != null)
                        {
                            settings.Exposure.GainSpecified = true;
                            settings.Exposure.Gain = this.GetSettingsValue(initialSettings.Exposure.GainSpecified, initialSettings.Exposure.Gain, options.Exposure.Gain);
                        }

                        if (options.Exposure.Iris != null)
                        {
                            settings.Exposure.IrisSpecified = true;
                            settings.Exposure.Iris = this.GetSettingsValue(initialSettings.Exposure.IrisSpecified, initialSettings.Exposure.Iris, options.Exposure.Iris);
                        }
                    }
                }
                else
                {
                    // initial settings not specified 
                    if (mode == ExposureMode.AUTO)
                    {
                        if (options.Exposure != null)
                        {
                            if (options.Exposure.Priority != null)
                            {
                                settings.Exposure.PrioritySpecified = true;
                                settings.Exposure.Priority = options.Exposure.Priority[0];
                            }
                            if (options.Exposure.MinExposureTime != null)
                            {
                                settings.Exposure.MinExposureTimeSpecified = true;
                                settings.Exposure.MinExposureTime = options.Exposure.MinExposureTime.Min;
                            }
                            if (options.Exposure.MaxExposureTime != null)
                            {
                                settings.Exposure.MaxExposureTimeSpecified = true;
                                settings.Exposure.MaxExposureTime = options.Exposure.MaxExposureTime.Max;
                            }
                            if (options.Exposure.MaxGain != null)
                            {
                                settings.Exposure.MaxGainSpecified = true;
                                settings.Exposure.MaxGain = options.Exposure.MaxGain.Max;
                            }
                            if (options.Exposure.MinGain != null)
                            {
                                settings.Exposure.MinGainSpecified = true;
                                settings.Exposure.MinGain = options.Exposure.MinGain.Min;
                            }
                            if (options.Exposure.MinIris != null)
                            {
                                settings.Exposure.MinIrisSpecified = true;
                                settings.Exposure.MinIris = options.Exposure.MinIris.Min;
                            }
                            if (options.Exposure.MaxIris != null)
                            {
                                settings.Exposure.MaxIrisSpecified = true;
                                settings.Exposure.MaxIris = options.Exposure.MaxIris.Max;
                            }
                        }
                    }

                    if (mode == ExposureMode.MANUAL)
                    {
                        if (options.Exposure != null)
                        {
                            if (options.Exposure.ExposureTime != null)
                            {
                                settings.Exposure.ExposureTimeSpecified = true;
                                settings.Exposure.ExposureTime = options.Exposure.ExposureTime.Min;
                            }

                            if (options.Exposure.Gain != null)
                            {
                                settings.Exposure.GainSpecified = true;
                                settings.Exposure.Gain = options.Exposure.Gain.Min;
                            }
                            if (options.Exposure.Iris != null)
                            {
                                settings.Exposure.IrisSpecified = true;
                                settings.Exposure.Iris = options.Exposure.Iris.Min;
                            }
                        }
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
                        if (options.Focus.FarLimit != null)
                        {
                            settings.Focus.FarLimitSpecified = true;
                            settings.Focus.FarLimit = GetSettingsValue(initialSettings.Focus.FarLimitSpecified,
                                                                       initialSettings.Focus.FarLimit,
                                                                       options.Focus.FarLimit);
                        }

                        if (options.Focus.NearLimit != null)
                        {
                            settings.Focus.NearLimitSpecified = true;
                            settings.Focus.NearLimit = GetSettingsValue(initialSettings.Focus.NearLimitSpecified,
                                                                        initialSettings.Focus.NearLimit,
                                                                        options.Focus.NearLimit);
                        }
                    }

                    if (settings.Focus.AutoFocusMode == AutoFocusMode.MANUAL)
                    {
                        if (options.Focus.DefaultSpeed != null)
                        {
                            settings.Focus.DefaultSpeedSpecified = true;
                            settings.Focus.DefaultSpeed = GetSettingsValue(initialSettings.Focus.DefaultSpeedSpecified,
                                                                           initialSettings.Focus.DefaultSpeed,
                                                                           options.Focus.DefaultSpeed);
                        }
                    }
                }
                else
                {
                    settings.Focus.AutoFocusMode = options.Focus.AutoFocusModes[0];

                    if (settings.Focus.AutoFocusMode == AutoFocusMode.AUTO)
                    {
                        if (options.Focus.FarLimit != null)
                        {
                            settings.Focus.FarLimitSpecified = true;
                            settings.Focus.FarLimit = options.Focus.FarLimit.Min;
                        }
                        if (options.Focus.NearLimit != null)
                        {
                            settings.Focus.NearLimitSpecified = true;
                            settings.Focus.NearLimit = options.Focus.NearLimit.Min;
                        }
                    }
                    if (settings.Focus.AutoFocusMode == AutoFocusMode.MANUAL)
                    {
                        if (options.Focus.DefaultSpeed != null)
                        {
                            settings.Focus.DefaultSpeedSpecified = true;
                            settings.Focus.DefaultSpeed = options.Focus.DefaultSpeed.Min;
                        }
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
                settings.Sharpness = GetSettingsValue(initialSettings.SharpnessSpecified, initialSettings.Sharpness, options.Sharpness);
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
                        if (options.WhiteBalance.YbGain != null)
                        {
                            settings.WhiteBalance.CbGainSpecified = true;
                            settings.WhiteBalance.CbGain = GetSettingsValue(
                                initialSettings.WhiteBalance.CbGainSpecified, initialSettings.WhiteBalance.CbGain,
                                options.WhiteBalance.YbGain);
                        }

                        if (options.WhiteBalance.YrGain != null)
                        {
                            settings.WhiteBalance.CrGainSpecified = true;
                            settings.WhiteBalance.CrGain = GetSettingsValue(initialSettings.WhiteBalance.CrGainSpecified, initialSettings.WhiteBalance.CrGain, options.WhiteBalance.YrGain);
                        }
                    }
                }
                else
                {
                    settings.WhiteBalance.Mode = options.WhiteBalance.Mode[0];

                    if (settings.WhiteBalance.Mode == WhiteBalanceMode.MANUAL)
                    {
                        if (options.WhiteBalance.YbGain != null)
                        {
                            settings.WhiteBalance.CbGainSpecified = true;
                            settings.WhiteBalance.CbGain = options.WhiteBalance.YbGain.Min;
                        }

                        if (options.WhiteBalance.YrGain != null)
                        {
                            settings.WhiteBalance.CrGainSpecified = true;
                            settings.WhiteBalance.CrGain = options.WhiteBalance.YrGain.Min;
                        }
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
                        if (options.WideDynamicRange.Level != null)
                        {
                            settings.WideDynamicRange.LevelSpecified = true;
                            settings.WideDynamicRange.Level =
                                GetSettingsValue(initialSettings.WideDynamicRange.LevelSpecified, initialSettings.WideDynamicRange.Level, options.WideDynamicRange.Level);
                        }
                    }
                }
                else
                {
                    settings.WideDynamicRange.Mode = options.WideDynamicRange.Mode[0];
                    if (settings.WideDynamicRange.Mode == WideDynamicMode.ON)
                    {
                        if (options.WideDynamicRange.Level != null)
                        {
                            settings.WideDynamicRange.LevelSpecified = true;
                            settings.WideDynamicRange.Level = options.WideDynamicRange.Level.Min;
                        }
                    }
                }
            }

            return settings;
        }

        /// <summary>
        /// Gets from the range value which differs from initially specified.
        /// </summary>
        /// <param name="valueSpecified"></param>
        /// <param name="value"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        protected float GetSettingsValue(bool valueSpecified, float value, FloatRange range)
        {
            if (range == null)
            {
                return 0;
            }
            else
            {
                if (valueSpecified)
                {
                    return range.Min == value ? range.Max : range.Min;
                }
                else
                {
                    return range.Min;
                }
            }
        }

        /// <summary>
        /// Generates invalid settings from options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
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
                    bool specified = options.Exposure.Priority != null && options.Exposure.Priority.Length > 0;
                    settings.Exposure.PrioritySpecified = specified;
                    if (options.Exposure.Priority != null)
                    {
                        settings.Exposure.Priority = options.Exposure.Priority[0];
                    }
                    
                    specified = options.Exposure.MinExposureTime != null;
                    settings.Exposure.MinExposureTimeSpecified = specified;
                    if (specified)
                    {
                        settings.Exposure.MinExposureTime = options.Exposure.MinExposureTime.Min - 1;
                    }

                    specified = options.Exposure.MaxExposureTime != null;
                    settings.Exposure.MaxExposureTimeSpecified = specified;
                    if (specified)
                    {
                        settings.Exposure.MaxExposureTime = options.Exposure.MaxExposureTime.Max + 1;
                    }

                    specified = options.Exposure.MaxGain != null;
                    settings.Exposure.MaxGainSpecified = specified;
                    if (specified)
                    {
                        settings.Exposure.MaxGain = options.Exposure.MaxGain.Max + 1;
                    }

                    specified = options.Exposure.MinGain != null;
                    settings.Exposure.MinGainSpecified = specified;
                    if (specified)
                    {
                        settings.Exposure.MinGain = options.Exposure.MinGain.Min - 1;
                    }

                    specified = options.Exposure.MinIris != null;
                    settings.Exposure.MinIrisSpecified = specified;
                    if (specified)
                    {
                        settings.Exposure.MinIris = options.Exposure.MinIris.Min - 1;
                    }

                    specified = options.Exposure.MaxIris != null;
                    settings.Exposure.MaxIrisSpecified = specified;
                    if (specified)
                    {
                        settings.Exposure.MaxIris = options.Exposure.MaxIris.Max + 1;
                    }
                }

                if (mode == ExposureMode.MANUAL)
                {
                    bool specified = options.Exposure.ExposureTime != null;
                    
                    settings.Exposure.ExposureTimeSpecified = specified;
                    if (specified)
                    {
                        settings.Exposure.ExposureTime = options.Exposure.ExposureTime.Max + 1;
                    }

                    specified = options.Exposure.Gain != null;
                    settings.Exposure.GainSpecified = specified;
                    if (specified)
                    {
                        settings.Exposure.Gain = options.Exposure.Gain.Max + 1;
                    }

                    specified = options.Exposure.Iris != null;
                    settings.Exposure.IrisSpecified = specified;
                    if (specified)
                    {
                        settings.Exposure.Iris = options.Exposure.Iris.Max + 1;
                    }
                }
            }

            if (options.Focus != null)
            {
                settings.Focus = new FocusConfiguration20();
                settings.Focus.AutoFocusMode = options.Focus.AutoFocusModes[0];

                bool specified;

                if (settings.Focus.AutoFocusMode == AutoFocusMode.AUTO)
                {
                    specified = options.Focus.FarLimit != null;
                    settings.Focus.FarLimitSpecified = specified;
                    if (specified)
                    {
                        settings.Focus.FarLimit = options.Focus.FarLimit.Max + 1;
                    }

                    specified = options.Focus.NearLimit != null;
                    settings.Focus.NearLimitSpecified = specified;
                    if (specified)
                    {
                        settings.Focus.NearLimit = options.Focus.NearLimit.Max + 1;
                    }
                }
                if (settings.Focus.AutoFocusMode == AutoFocusMode.MANUAL  )
                {
                    specified = options.Focus.DefaultSpeed != null;
                    settings.Focus.DefaultSpeedSpecified = specified;
                    if (specified)
                    {
                        settings.Focus.DefaultSpeed = options.Focus.DefaultSpeed.Max + 1;
                    }
                }
            }

            if (options.IrCutFilterModes != null && options.IrCutFilterModes.Length > 0)
            {
                settings.IrCutFilterSpecified = true;
                settings.IrCutFilter = options.IrCutFilterModes[0];
            }

            if (options.Sharpness != null)
            {
                settings.SharpnessSpecified = options.Sharpness != null;
                if (settings.SharpnessSpecified)
                {
                    settings.Sharpness = options.Sharpness.Max + 1;
                }
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

        /// <summary>
        /// Compares settings received from Media services\ and from Imaging service.
        /// </summary>
        /// <param name="mediaSettings"></param>
        /// <param name="imagingSettings"></param>
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

        /// <summary>
        /// Validates options.
        /// </summary>
        /// <param name="options"></param>
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
                                              off,
                                              sb,
                                              "Level defined for BacklightCompensation while Mode is OFF");
            }

            // Brightness
            if (options.Brightness != null)
            {
                if (options.Brightness.Min > options.Brightness.Max)
                {
                    ok = false;
                    sb.AppendLine("Range is invalid for Brightness");
                }
            }


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
                                       onlyAuto,
                                       sb,
                                       "Exposure Time options found while the only Mode is Auto");

                    ok = ok && currentOk;

                    currentOk = ValidateFloatRange(options.Exposure.Gain,
                                       "Gain",
                                       onlyAuto,
                                       sb,
                                       "Gain options found while the only Mode is Auto");
                    ok = ok && currentOk;

                    currentOk = ValidateFloatRange(options.Exposure.Iris,
                                       "Iris",
                                       onlyAuto,
                                       sb,
                                       "Iris options found while the only Mode is Auto");
                    ok = ok && currentOk;

                    currentOk = ValidateFloatRange(options.Exposure.MaxExposureTime,
                                       "MaxExposureTime",
                                       onlyManual,
                                       sb,
                                       "MaxExposureTime options found while the only Mode is Manual");
                    ok = ok && currentOk;


                    currentOk = ValidateFloatRange(options.Exposure.MinExposureTime,
                                       "MinExposureTime",
                                       onlyManual,
                                       sb,
                                       "MinExposureTime options found while the only Mode is Manual");
                    ok = ok && currentOk;

                    currentOk = ValidateFloatRange(options.Exposure.MaxGain,
                                       "MaxGain",
                                       onlyManual,
                                       sb,
                                       "MaxGain options found while the only Mode is Manual");
                    ok = ok && currentOk;

                    currentOk = ValidateFloatRange(options.Exposure.MinGain,
                                       "MinGain",
                                       onlyManual,
                                       sb,
                                       "MinGain options found while the only Mode is Manual");
                    ok = ok && currentOk;

                    currentOk = ValidateFloatRange(options.Exposure.MaxIris,
                                       "MaxIris",
                                       onlyManual,
                                       sb,
                                       "MaxIris options found while the only Mode is Manual");
                    ok = ok && currentOk;

                    currentOk = ValidateFloatRange(options.Exposure.MinIris,
                                       "MinIris",
                                       onlyManual,
                                       sb,
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
                                   onlyAuto,
                                   sb,
                                   "DefaultSpeed options found while the only Mode is Auto");
                ok = ok && currentOk;

                currentOk = ValidateFloatRange(options.Focus.NearLimit,
                                   "NearLimit",
                                   onlyManual,
                                   sb,
                                   "NearLimit options found while the only Mode is Manual");
                ok = ok && currentOk;

                currentOk = ValidateFloatRange(options.Focus.FarLimit,
                                   "FarLimit",
                                   onlyManual,
                                   sb,
                                   "FarLimit options found while the only Mode is Manual");
                ok = ok && currentOk;

            }

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
                                               onlyOff,
                                               sb,
                                               "Level options for WideDynamicRange found while the only Mode is Off");
                ok = ok && currentOk;

            }

            if (options.WhiteBalance != null)
            {
                bool onlyAuto = options.WhiteBalance.Mode != null &&
                                options.WhiteBalance.Mode.Length == 1 &&
                                options.WhiteBalance.Mode[0] == WhiteBalanceMode.AUTO;

                currentOk = ValidateFloatRange(options.WhiteBalance.YrGain,
                                   "WhiteBalance.YrGain",
                                   onlyAuto,
                                   sb,
                                   "WhiteBalance.YrGain options found while the only Mode is Auto");
                ok = ok && currentOk;

                currentOk = ValidateFloatRange(options.WhiteBalance.YbGain,
                   "WhiteBalance.YbGain",
                   onlyAuto,
                   sb,
                   "WhiteBalance.YbGain options found while the only Mode is Auto");
                ok = ok && currentOk;

            }

            string dump = sb.ToStringTrimNewLine();

            Assert(ok, dump, "Validate options structure");
        }

        /// <summary>
        /// Validates Move options
        /// </summary>
        /// <param name="options"></param>
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

        /// <summary>
        /// Validates imaging status
        /// </summary>
        /// <param name="status"></param>
        protected void ValidateImagingStatus(ImagingStatus20 status)
        {

        }

        /// <summary>
        /// Compares settings.
        /// </summary>
        /// <param name="expectedSettings">Expected value</param>
        /// <param name="actualSettings">Actual value.</param>
        /// <param name="changed">True if there was an attempt to change settings</param>
        /// <param name="dump"></param>
        /// <returns></returns>
        protected bool ImagingSettingsEqual(ImagingSettings20 expectedSettings,
            ImagingSettings20 actualSettings,
            bool changed,
            out string dump)
        {
            bool bOk = true;
            bool current = true;
            StringBuilder sb = new StringBuilder();

            // if there was an attempt to change settings and BacklightCompensation was not sent, don't check
            if (!changed || expectedSettings.BacklightCompensation != null)
            {
 
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

                    if (!changed || expectedSettings.BacklightCompensation.LevelSpecified)
                    {
                        current = ValidateFloatProperty<BacklightCompensation20>(
                            expectedSettings.BacklightCompensation,
                            actualSettings.BacklightCompensation,
                            (B) => B.LevelSpecified,
                            (B) => B.Level,
                            "Backlight compensation level",
                            sb);
                        bOk = bOk && current;
                    }
                }
                else
                {
                    if (!changed)
                    {
                        // Both nulls are OK (excluding tha fact that WE don't send null)
                        if (
                            !(actualSettings.BacklightCompensation == null &&
                              expectedSettings.BacklightCompensation == null))
                        {
                            sb.AppendLine(string.Format("BacklightCompensation: {0}",
                                                        CreateNullsDescription(expectedSettings.BacklightCompensation,
                                                                               actualSettings.BacklightCompensation)));
                            bOk = false;
                        }
                    }
                }
            }



            if (!changed || expectedSettings.BrightnessSpecified)
            {
                // Brightness
                current = ValidateFloatProperty<ImagingSettings20>(
                    expectedSettings,
                    actualSettings,
                    (IS) => IS.BrightnessSpecified,
                    (IS) => IS.Brightness,
                    "Brightness",
                    sb);
                bOk = bOk && current;
            }


            if (!changed || expectedSettings.ColorSaturationSpecified)
            {
                // Color saturation

                current = ValidateFloatProperty<ImagingSettings20>(
                    expectedSettings,
                    actualSettings,
                    (IS) => IS.ColorSaturationSpecified,
                    (IS) => IS.ColorSaturation,
                    "Color saturation",
                    sb);
                bOk = bOk && current;
            }


            if (!changed || expectedSettings.ContrastSpecified)
            {
                // Contrast
                current = ValidateFloatProperty<ImagingSettings20>(
                    expectedSettings,
                    actualSettings,
                    (IS) => IS.ContrastSpecified,
                    (IS) => IS.Contrast,
                    "Contrast",
                    sb);
                bOk = bOk && current;
            }


            if (!changed || expectedSettings.Exposure != null)
            {
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

                    if (!changed || expected.ExposureTimeSpecified)
                    {
                        current = ValidateFloatProperty<Exposure20>(
                            expected,
                            actual,
                            (E) => E.ExposureTimeSpecified,
                            (E) => E.ExposureTime,
                            "Exposure time",
                            sb);
                        bOk = bOk && current;

                    }


                    if (!changed || expected.GainSpecified)
                    {
                        // Gain
                        current = ValidateFloatProperty<Exposure20>(
                            expected,
                            actual,
                            (E) => E.GainSpecified,
                            (E) => E.Gain,
                            "Gain",
                            sb);
                        bOk = bOk && current;
                    }

                    if (!changed || expected.IrisSpecified)
                    {
                        // Iris
                        current = ValidateFloatProperty<Exposure20>(
                            expected,
                            actual,
                            (E) => E.IrisSpecified,
                            (E) => E.Iris,
                            "Iris",
                            sb);
                        bOk = bOk && current;
                    }

                    // MinExposureTime

                    if (!changed || expected.MinExposureTimeSpecified)
                    {
                        current = ValidateFloatProperty<Exposure20>(
                            expected,
                            actual,
                            (E) => E.MinExposureTimeSpecified,
                            (E) => E.MinExposureTime,
                            "Min Exposure Time",
                            sb);
                    }

                    bOk = bOk && current;

                    // MaxExposureTime

                    if (!changed || expected.MaxExposureTimeSpecified)
                    {
                        current = ValidateFloatProperty<Exposure20>(
                            expected,
                            actual,
                            (E) => E.MaxExposureTimeSpecified,
                            (E) => E.MaxExposureTime,
                            "Max Exposure Time",
                            sb);

                        bOk = bOk && current;
                    }

                    // MinGain

                    if (!changed || expected.MinGainSpecified)
                    {
                        current = ValidateFloatProperty<Exposure20>(
                            expected,
                            actual,
                            (E) => E.MinGainSpecified,
                            (E) => E.MinGain,
                            "Min Gain",
                            sb);
                        bOk = bOk && current;
                    }

                    // MaxGain

                    if (!changed || expected.MaxGainSpecified)
                    {

                        current = ValidateFloatProperty<Exposure20>(
                            expected,
                            actual,
                            (E) => E.MaxGainSpecified,
                            (E) => E.MaxGain,
                            "Max Gain",
                            sb);

                        bOk = bOk && current;
                    }


                    // MinIris
                    if (!changed || expected.MinIrisSpecified)
                    {
                        current = ValidateFloatProperty<Exposure20>(
                            expected,
                            actual,
                            (E) => E.MinIrisSpecified,
                            (E) => E.MinIris,
                            "Min Iris",
                            sb);

                        bOk = bOk && current;
                    }



                    // MaxIris

                    if (!changed || expected.MaxIrisSpecified)
                    {
                        current = ValidateFloatProperty<Exposure20>(
                            expected,
                            actual,
                            (E) => E.MaxIrisSpecified,
                            (E) => E.MaxIris,
                            "Max Iris",
                            sb);

                        bOk = bOk && current;
                    }

                    // Priority

                    if (!changed || expected.PrioritySpecified)
                    {
                        current = ValidateValueProperty<Exposure20, ExposurePriority>(
                            expected,
                            actual,
                            (E) => E.PrioritySpecified,
                            (E) => E.Priority,
                            (P1, P2) => (P1 == P2),
                            "Priority specified",
                            sb);

                        bOk = bOk && current;
                    }


                    // Window

                    if (actual.Window != null && expected.Window != null)
                    {
                        Rectangle actualWindow = actual.Window;
                        Rectangle expectedWindow = expected.Window;

                        sb.AppendLine("Exposure window: ");

                        // bottom 

                        if (!changed || expectedWindow.bottomSpecified)
                        {
                            current = ValidateFloatProperty<Rectangle>(
                                expectedWindow,
                                actualWindow,
                                (W) => W.bottomSpecified,
                                (W) => W.bottom,
                                "Bottom",
                                sb);

                            bOk = bOk && current;
                        }

                        // top

                        if (!changed || expectedWindow.topSpecified)
                        {
                            current = ValidateFloatProperty<Rectangle>(
                                expectedWindow,
                                actualWindow,
                                (W) => W.topSpecified,
                                (W) => W.top,
                                "Top",
                                sb);

                            bOk = bOk && current;
                        }

                        // left

                        if (!changed || expectedWindow.leftSpecified)
                        {
                            current = ValidateFloatProperty<Rectangle>(
                                expectedWindow,
                                actualWindow,
                                (W) => W.leftSpecified,
                                (W) => W.left,
                                "Left",
                                sb);

                            bOk = bOk && current;
                        }

                        // right
                        if (!changed || expectedWindow.rightSpecified)
                        {
                            current = ValidateFloatProperty<Rectangle>(
                                expectedWindow,
                                actualWindow,
                                (W) => W.rightSpecified,
                                (W) => W.right,
                                "Right",
                                sb);

                            bOk = bOk && current;
                        }
                    }
                    else
                    {
                        if (!changed)
                        {
                            if (!(actual.Window == null && expected.Window == null))
                            {
                                sb.AppendLine(string.Format("Exposure Window: {0}",
                                                            CreateNullsDescription(expected.Window,
                                                                                   actual.Window)));
                                bOk = false;
                            }
                        }
                        // Both nulls are OK (excluding tha fact that WE don't send null)
                    }
                }
                else
                {
                    if (!changed)
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
                }

            }


            // Focus
            if (actualSettings.Focus != null && expectedSettings.Focus != null)
            {

                FocusConfiguration20 actualFocus = actualSettings.Focus;
                FocusConfiguration20 expectedFocus = expectedSettings.Focus;

                // Auto Focus Mode
                if (actualFocus.AutoFocusMode != expectedFocus.AutoFocusMode)
                {
                    sb.AppendLine(string.Format("AutoFocus mode: expected - {0}, actual - {1}",
                                                expectedFocus.AutoFocusMode, actualFocus.AutoFocusMode));
                }

                // Default Speed

                if (!changed || expectedFocus.DefaultSpeedSpecified)
                {
                    current = ValidateFloatProperty<FocusConfiguration20>(
                        expectedFocus,
                        actualFocus,
                        (F) => F.DefaultSpeedSpecified,
                        (F) => F.DefaultSpeed,
                        "Default Speed",
                        sb);
                    bOk = bOk && current;
                }




                // FarLimit, FarLimitSpecified


                if (!changed || expectedFocus.FarLimitSpecified)
                {
                    current = ValidateFloatProperty<FocusConfiguration20>(
                        expectedFocus,
                        actualFocus,
                        (F) => F.FarLimitSpecified,
                        (F) => F.FarLimit,
                        "Far Limit",
                        sb);
                    bOk = bOk && current;
                }

                // NearLimit, NearLimitSpecified

                if (!changed || expectedFocus.NearLimitSpecified)
                {
                    current = ValidateFloatProperty<FocusConfiguration20>(
                        expectedFocus,
                        actualFocus,
                        (F) => F.NearLimitSpecified,
                        (F) => F.NearLimit,
                        "Near Limit",
                        sb);
                    bOk = bOk && current;
                }
            }
            else
            {
                if (!changed)
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
            }

            // IrCutFilter
            current = ValidateValueProperty<ImagingSettings20, IrCutFilterMode>(
                expectedSettings,
                actualSettings,
                (S) => S.IrCutFilterSpecified,
                (S) => S.IrCutFilter,
                (v1, v2) => v1 == v2,
                "IrCutFilter",
                sb);
            
            bOk = bOk && current;
            
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
                    actualWhiteBalance.Mode == WhiteBalanceMode.MANUAL)
                {


                    if (!changed || expectedWhiteBalance.CbGainSpecified)
                    {
                        current = ValidateFloatProperty<WhiteBalance20>(
                            expectedWhiteBalance,
                            actualWhiteBalance,
                            (W) => W.CbGainSpecified,
                            (W) => W.CbGain,
                            "White balance CbGain",
                            sb);
                        bOk = bOk && current;
                    }


                    if (!changed || expectedWhiteBalance.CrGainSpecified)
                    {
                        current = ValidateFloatProperty<WhiteBalance20>(
                            expectedWhiteBalance,
                            actualWhiteBalance,
                            (W) => W.CrGainSpecified,
                            (W) => W.CrGain,
                            "White balance CrGain",
                            sb);
                        bOk = bOk && current;
                    }
                }
            }
            else
            {
                if (!changed)
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

                    if (!changed || expectedSettings.WideDynamicRange.LevelSpecified)
                    {
                        current = ValidateFloatProperty<WideDynamicRange20>(
                            expectedSettings.WideDynamicRange,
                            actualSettings.WideDynamicRange,
                            (S) => S.LevelSpecified,
                            (S) => S.Level,
                            "Wide Dynamic Range level",
                            sb);
                        bOk = bOk && current;

                    }
                }
            }
            else
            {
                if (!changed)
                {
                    // Both nulls are OK (excluding the fact that WE don't send null)
                    if (!(actualSettings.WideDynamicRange == null && expectedSettings.WideDynamicRange == null))
                    {
                        sb.AppendLine(string.Format("WideDynamicRange: {0}",
                                                    CreateNullsDescription(expectedSettings.WideDynamicRange,
                                                                           actualSettings.WideDynamicRange)));
                        bOk = false;
                    }
                }
            }

            dump = sb.ToStringTrimNewLine();

            return bOk;
        }

        /// <summary>
        /// Creates description of two values from the point of view "null - not null"
        /// </summary>
        /// <param name="object1">Expected value</param>
        /// <param name="object2">Actual value</param>
        /// <returns></returns>
        string CreateNullsDescription(object object1, object object2)
        {
            return string.Format("expected - {0}, actual - {1}",
                                 object1 == null ? "NULL" : "NOT NULL",
                                 object2 == null ? "NULL" : "NOT NULL");
        }

        /// <summary>
        /// Validate float range
        /// </summary>
        /// <param name="range">Range</param>
        /// <param name="settingsName">Name of the structure.</param>
        /// <param name="dumpOutput"></param>
        /// <returns></returns>
        bool ValidateFloatRange(FloatRange range, string settingsName, StringBuilder dumpOutput)
        {
            bool ok = true;
            if (range != null)
            {
                ok = ValidateRangeBoudary(range, settingsName, dumpOutput);
            }
            return ok;
        }

        /// <summary>
        /// Validates float range which can be disabled
        /// </summary>
        /// <param name="range">Range</param>
        /// <param name="settingsName">Name of the structure.</param>
        /// <param name="disabled">True if no such settings should exist</param>
        /// <param name="dumpOutput">Log</param>
        /// <param name="optionsNotAllowed"></param>
        /// <returns></returns>
        bool ValidateFloatRange(FloatRange range,
            string settingsName,
            bool disabled,
            StringBuilder dumpOutput,
            string optionsNotAllowed)
        {
            bool ok = true;
            if (range != null)
            {
                if (disabled)
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

        /// <summary>
        /// Validate range bounds
        /// </summary>
        /// <param name="range">Range</param>
        /// <param name="settingsName">Name of the structure.</param>
        /// <param name="dumpOutput">Log</param>
        /// <returns></returns>
        bool ValidateRangeBoudary(FloatRange range, string settingsName, StringBuilder dumpOutput)
        {
            if (range.Min > range.Max)
            {
                dumpOutput.AppendLine(string.Format("Range is invalid for {0}", settingsName));
            }
            return (range.Min <= range.Max);
        }

        /// <summary>
        /// Validates float property
        /// </summary>
        /// <typeparam name="T">Structure type</typeparam>
        /// <param name="expected">Excpected structure</param>
        /// <param name="actual">Actual structure</param>
        /// <param name="specifiedSelector">Selector for "Specified" property</param>
        /// <param name="valueSelector">Selector for value property</param>
        /// <param name="name">Name of the property</param>
        /// <param name="sb">Log</param>
        /// <returns></returns>
        bool ValidateFloatProperty<T>(T expected, T actual,
            Func<T, bool> specifiedSelector,
            Func<T, float> valueSelector,
            string name,
            StringBuilder sb)
        {
            Func<float, string> toString =
                (val) =>
                {
                    return float.IsPositiveInfinity(val)
                               ? "Positive infinity"
                               : float.IsNegativeInfinity(val) ? "Negative infinity" : val.ToString();
                };

            return ValidateValueProperty<T, float>
                (expected,
                 actual,
                 specifiedSelector,
                 valueSelector,
                 (F1, F2) => (F1 == F2),
                 name,
                 sb,
                 toString);
        }

        /// <summary>
        /// Validates typed property.
        /// </summary>
        /// <typeparam name="T">Type of the structure</typeparam>
        /// <typeparam name="T1">Type of the field</typeparam>
        /// <param name="expected">Expected valie</param>
        /// <param name="actual">Actual value </param>
        /// <param name="specifiedSelector">Specified selector</param>
        /// <param name="valueSelector">Value selector</param>
        /// <param name="comparison">Comparer</param>
        /// <param name="name">Field name</param>
        /// <param name="sb">Log</param>
        /// <returns></returns>
        bool ValidateValueProperty<T, T1>(T expected, T actual,
            Func<T, bool> specifiedSelector,
            Func<T, T1> valueSelector,
            Func<T1, T1, bool> comparison,
            string name,
            StringBuilder sb)
        {
            return ValidateValueProperty<T, T1>
                (expected,
                 actual,
                 specifiedSelector,
                 valueSelector,
                 comparison,
                 name,
                 sb,
                 (V1) => V1.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type of the structure</typeparam>
        /// <typeparam name="T1">Type of the field</typeparam>
        /// <param name="expected">Expected valie</param>
        /// <param name="actual">Actual value </param>
        /// <param name="specifiedSelector">Specified selector</param>
        /// <param name="valueSelector">Value selector</param>
        /// <param name="comparison">Comparer</param>
        /// <param name="name">Field name</param>
        /// <param name="sb">Log</param>
        /// <param name="toString">Method to get string from value.</param>
        /// <returns></returns>
        bool ValidateValueProperty<T, T1>(T expected, T actual,
            Func<T, bool> specifiedSelector,
            Func<T, T1> valueSelector,
            Func<T1, T1, bool> comparison,
            string name,
            StringBuilder sb,
            Func<T1, string> toString)
        {
            bool bOk = true;

            bool expectedSpecified = specifiedSelector(expected);
            bool actualSpecified = specifiedSelector(actual);

            if (expectedSpecified != actualSpecified)
            {
                bOk = false;
                sb.AppendLine(string.Format("{0} specified: expected - {1}, actual - {2}",
                    name, expectedSpecified, actualSpecified));
            }


            if (expectedSpecified && actualSpecified)
            {
                T1 expectedVal = valueSelector(expected);
                T1 actualVal = valueSelector(actual);

                if (!comparison(expectedVal, actualVal))
                {
                    bOk = false;
                    sb.AppendLine(string.Format("{0}: expected - {1}, actual - {2}",
                        name, toString(expectedVal), toString(actualVal)));
                }
            }

            return bOk;
        }

        #endregion
    

    }
}
