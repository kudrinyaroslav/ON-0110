using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml.Serialization;
using DUT.CameraWebService;
using CameraWebService;

namespace DUT.CameraWebService.Imaging20
{

    /// <summary>
    /// Class for Imaging20 Service tests
    /// </summary>
    public class ImagingService20Test
    {

        #region Const

        private const string ServiceName = "Imaging20";

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetImagingSettings = 0;
        private const int SetImagingSettings = 1;
        private const int GetOptions = 2;
        private const int Move = 3;
        private const int GetMoveOptions = 4;
        private const int Stop = 5;
        private const int GetStatus = 6;
        private const int GetServiceCapabilities = 7;
        private const int MaxCommands = 8;

        #endregion //Const

        #region Members

        /// <summary>
        /// Mass with command call count
        /// </summary>
        private int[] m_commandCount = new int[MaxCommands];
        /// <summary>
        /// Test suit description
        /// </summary>
        TestCommon m_TestCommon = null;

        #endregion //Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ImagingService20Test(TestCommon testCommon)
        {
            for (int i = 0; i < m_commandCount.Length; i++)
            {
                m_commandCount[i] = 0;
            }
            m_TestCommon = testCommon;
        }

        #endregion //Constructors

        #region General

        /// <summary>
        /// Return incremented target if it is not more than maxValue. Return 0 in other case.
        /// </summary>
        /// <param name="maxValue">Max value for target</param>
        /// <param name="teaget">Incremented value</param>
        /// <returns>Changed target</returns>
        private void Increment(int maxValue, int index)
        {
            if (maxValue - 1 <= m_commandCount[index])
            {
                m_commandCount[index] = 0;
            }
            else
            {
                m_commandCount[index]++;
            }
        }

        public void ResetTestSuit()
        {
            for (int i = 0; i < m_commandCount.Length; i++)
            {
                m_commandCount[i] = 0;
            }
        }

        #endregion //General

        internal StepType GetImagingSettingsTest(out ImagingSettings20 target, out SoapException ex, out int Timeout, string VideoSourceToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging20.GetImagingSettings");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[GetImagingSettings]];

                #region Serialization Temp
                //Media.VideoEncoderConfigurationOptions dsr = new Media.VideoEncoderConfigurationOptions();
                //dsr.JPEG = new Media.JpegOptions();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.VideoEncoderConfigurationOptions));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //VideoSourceToken
                CommonCompare.StringCompare("RequestParameters/VideoSourceToken", "VideoSourceToken", VideoSourceToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(ImagingSettings20));
                target = (ImagingSettings20)targetObj;

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetImagingSettings);
            }
            else
            {
                res = StepType.None;
                Timeout = 0;
                target = null;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType SetImagingSettingsTest(out SoapException ex, out int Timeout, string VideoSourceToken, ImagingSettings20 ImagingSettings, bool ForcePersistence, bool ForcePersistenceSpecified)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging20.SetImagingSettings");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[SetImagingSettings]];

                #region Analyze request

                //VideoSourceToken
                CommonCompare.StringCompare("RequestParameters/VideoSourceToken", "VideoSourceToken", VideoSourceToken, ref logMessage, ref passed, test);

                //ImagingSettings
                if (CommonCompare.Exist("RequestParameters/ImagingSettings", "ImagingSettings", ImagingSettings, ref logMessage, ref passed, test))
                {
                    //BacklightCompensation
                    if (CommonCompare.Exist("RequestParameters/ImagingSettings/BacklightCompensation", "BacklightCompensation", ImagingSettings.BacklightCompensation, ref logMessage, ref passed, test))
                    {
                        //Mode
                        CommonCompare.StringCompare("RequestParameters/ImagingSettings/BacklightCompensation/Mode", "Mode", ImagingSettings.BacklightCompensation.Mode.ToString(), ref logMessage, ref passed, test);
                        if (ImagingSettings.BacklightCompensation.Mode == BacklightCompensationMode.ON)
                        {
                            //Level
                            CommonCompare.FloatCompare("RequestParameters/ImagingSettings/BacklightCompensation/Level", "Level", ImagingSettings.BacklightCompensation.Level, ref logMessage, ref passed, test);
                        }
                    }
                    //Brightness
                    if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Brightness", "Brightness", ImagingSettings.BrightnessSpecified, ref logMessage, ref passed, test))
                    {
                        CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Brightness", "Brightness", ImagingSettings.Brightness, ref logMessage, ref passed, test);
                    }
                    //ColorSaturation
                    if (CommonCompare.Exist2("RequestParameters/ImagingSettings/ColorSaturation", "ColorSaturation", ImagingSettings.ColorSaturationSpecified, ref logMessage, ref passed, test))
                    {
                        CommonCompare.FloatCompare("RequestParameters/ImagingSettings/ColorSaturation", "ColorSaturation", ImagingSettings.ColorSaturation, ref logMessage, ref passed, test);
                    }
                    //Contrast
                    if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Contrast", "Contrast", ImagingSettings.ContrastSpecified, ref logMessage, ref passed, test))
                    {
                        CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Contrast", "Contrast", ImagingSettings.Contrast, ref logMessage, ref passed, test);
                    }
                    //Exposure
                    if (CommonCompare.Exist("RequestParameters/ImagingSettings/Exposure", "Exposure", ImagingSettings.Exposure, ref logMessage, ref passed, test))
                    {
                        //Mode
                        CommonCompare.StringCompare("RequestParameters/ImagingSettings/Exposure/Mode", "Mode", ImagingSettings.Exposure.Mode.ToString(), ref logMessage, ref passed, test);
                        if (ImagingSettings.Exposure.Mode == ExposureMode.AUTO)
                        {
                            //Priority
                            if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Exposure/Priority", "Priority", ImagingSettings.Exposure.PrioritySpecified, ref logMessage, ref passed, test))
                            {
                                CommonCompare.StringCompare("RequestParameters/ImagingSettings/Exposure/Priority", "Priority", ImagingSettings.Exposure.Priority.ToString(), ref logMessage, ref passed, test);
                            }
                            //MinExposureTime
                            if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Exposure/MinExposureTime", "MinExposureTime", ImagingSettings.Exposure.MinExposureTimeSpecified, ref logMessage, ref passed, test))
                            {
                                CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Exposure/MinExposureTime", "MinExposureTime", ImagingSettings.Exposure.MinExposureTime, ref logMessage, ref passed, test);
                            }
                            //MaxExposureTime
                            if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Exposure/MaxExposureTime", "MaxExposureTime", ImagingSettings.Exposure.MaxExposureTimeSpecified, ref logMessage, ref passed, test))
                            {
                                CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Exposure/MaxExposureTime", "MaxExposureTime", ImagingSettings.Exposure.MaxExposureTime, ref logMessage, ref passed, test);
                            }
                            //MinGain
                            if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Exposure/MinGain", "MinGain", ImagingSettings.Exposure.MinGainSpecified, ref logMessage, ref passed, test))
                            {
                                CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Exposure/MinGain", "MinGain", ImagingSettings.Exposure.MinGain, ref logMessage, ref passed, test);
                            }
                            //MaxGain
                            if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Exposure/MaxGain", "MaxGain", ImagingSettings.Exposure.MaxGainSpecified, ref logMessage, ref passed, test))
                            {
                                CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Exposure/MaxGain", "MaxGain", ImagingSettings.Exposure.MaxGain, ref logMessage, ref passed, test);
                            }
                            //MinIris
                            if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Exposure/MinIris", "MinIris", ImagingSettings.Exposure.MinIrisSpecified, ref logMessage, ref passed, test))
                            {
                                CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Exposure/MinIris", "MinIris", ImagingSettings.Exposure.MinIris, ref logMessage, ref passed, test);
                            }
                            //MaxIris
                            if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Exposure/MaxIris", "MaxIris", ImagingSettings.Exposure.MaxIrisSpecified, ref logMessage, ref passed, test))
                            {
                                CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Exposure/MaxIris", "MaxIris", ImagingSettings.Exposure.MaxIris, ref logMessage, ref passed, test);
                            }
                        }
                        else if (ImagingSettings.Exposure.Mode == ExposureMode.MANUAL)
                        {
                            //ExposureTime
                            if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Exposure/ExposureTime", "ExposureTime", ImagingSettings.Exposure.ExposureTimeSpecified, ref logMessage, ref passed, test))
                            {
                                CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Exposure/ExposureTime", "ExposureTime", ImagingSettings.Exposure.ExposureTime, ref logMessage, ref passed, test);
                            }
                            //Gain
                            if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Exposure/Gain", "Gain", ImagingSettings.Exposure.GainSpecified, ref logMessage, ref passed, test))
                            {
                                CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Exposure/Gain", "Gain", ImagingSettings.Exposure.Gain, ref logMessage, ref passed, test);
                            }
                            //Iris
                            if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Exposure/Iris", "Iris", ImagingSettings.Exposure.IrisSpecified, ref logMessage, ref passed, test))
                            {
                                CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Exposure/Iris", "Iris", ImagingSettings.Exposure.Iris, ref logMessage, ref passed, test);
                            }
                        }
                    }
                    //Focus
                    if (CommonCompare.Exist("RequestParameters/ImagingSettings/Focus", "Focus", ImagingSettings.Focus, ref logMessage, ref passed, test))
                    {
                        //AutoFocusMode
                        CommonCompare.StringCompare("RequestParameters/ImagingSettings/Focus/AutoFocusMode", "AutoFocusMode", ImagingSettings.Focus.AutoFocusMode.ToString(), ref logMessage, ref passed, test);
                        if (ImagingSettings.Focus.AutoFocusMode == AutoFocusMode.MANUAL)
                        {
                            //DefaultSpeed
                            CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Focus/DefaultSpeed", "DefaultSpeed", ImagingSettings.Focus.DefaultSpeed, ref logMessage, ref passed, test);
                        }
                        else if (ImagingSettings.Focus.AutoFocusMode == AutoFocusMode.AUTO)
                        {
                            //NearLimit
                            CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Focus/NearLimit", "NearLimit", ImagingSettings.Focus.NearLimit, ref logMessage, ref passed, test);
                            //FarLimit
                            CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Focus/FarLimit", "FarLimit", ImagingSettings.Focus.FarLimit, ref logMessage, ref passed, test);
                        }
                    }
                    //IrCutFilter
                    if (CommonCompare.Exist2("RequestParameters/ImagingSettings/IrCutFilter", "IrCutFilter", ImagingSettings.IrCutFilterSpecified, ref logMessage, ref passed, test))
                    {
                        CommonCompare.StringCompare("RequestParameters/ImagingSettings/IrCutFilter", "IrCutFilter", ImagingSettings.IrCutFilter.ToString(), ref logMessage, ref passed, test);
                    }
                    //Sharpness
                    if (CommonCompare.Exist2("RequestParameters/ImagingSettings/Sharpness", "Sharpness", ImagingSettings.SharpnessSpecified, ref logMessage, ref passed, test))
                    {
                        CommonCompare.FloatCompare("RequestParameters/ImagingSettings/Sharpness", "Sharpness", ImagingSettings.Sharpness, ref logMessage, ref passed, test);
                    }
                    //WideDynamicRange
                    if (CommonCompare.Exist("RequestParameters/ImagingSettings/WideDynamicRange", "WideDynamicRange", ImagingSettings.WideDynamicRange, ref logMessage, ref passed, test))
                    {
                        //Mode
                        CommonCompare.StringCompare("RequestParameters/ImagingSettings/WideDynamicRange/Mode", "Mode", ImagingSettings.WideDynamicRange.Mode.ToString(), ref logMessage, ref passed, test);
                        if (ImagingSettings.WideDynamicRange.Mode == WideDynamicMode.ON)
                        {
                            //Level
                            CommonCompare.FloatCompare("RequestParameters/ImagingSettings/WideDynamicRange/Level", "Level", ImagingSettings.WideDynamicRange.Level, ref logMessage, ref passed, test);
                        }
                    }
                    //WhiteBalance
                    if (CommonCompare.Exist("RequestParameters/ImagingSettings/WhiteBalance", "WhiteBalance", ImagingSettings.WhiteBalance, ref logMessage, ref passed, test))
                    {
                        //Mode
                        CommonCompare.StringCompare("RequestParameters/ImagingSettings/WhiteBalance/Mode", "Mode", ImagingSettings.WhiteBalance.Mode.ToString(), ref logMessage, ref passed, test);
                        if (ImagingSettings.WhiteBalance.Mode == WhiteBalanceMode.MANUAL)
                        {
                            //CrGain
                            CommonCompare.FloatCompare("RequestParameters/ImagingSettings/WhiteBalance/CrGain", "CrGain", ImagingSettings.WhiteBalance.CrGain, ref logMessage, ref passed, test);
                            //CbGain
                            CommonCompare.FloatCompare("RequestParameters/ImagingSettings/WhiteBalance/CbGain", "CbGain", ImagingSettings.WhiteBalance.CbGain, ref logMessage, ref passed, test);
                        }
                    }
                }

                //ForcePersistence
                CommonCompare.Exist2("RequestParameters/ForcePersistence", "ForcePersistence", ForcePersistenceSpecified, ref logMessage, ref passed, test);
                if (passed)
                {
                    CommonCompare.StringCompare("RequestParameters/ForcePersistence", "ForcePersistence", ForcePersistence.ToString(), ref logMessage, ref passed, test);
                }

                #endregion //Analyze request

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetImagingSettings);
            }
            else
            {
                res = StepType.None;
                Timeout = 0;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType GetOptionsTest(out ImagingOptions20 target, out SoapException ex, out int Timeout, string VideoSourceToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging20.GetOptions");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[GetOptions]];

                #region Serialization Temp
                //Media.VideoEncoderConfigurationOptions dsr = new Media.VideoEncoderConfigurationOptions();
                //dsr.JPEG = new Media.JpegOptions();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.VideoEncoderConfigurationOptions));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //VideoSourceToken
                CommonCompare.StringCompare("RequestParameters/VideoSourceToken", "VideoSourceToken", VideoSourceToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(ImagingOptions20));
                target = (ImagingOptions20)targetObj;

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetOptions);
            }
            else
            {
                res = StepType.None;
                Timeout = 0;
                target = null;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType MoveTest(out SoapException ex, out int Timeout, string VideoSourceToken, FocusMove Focus)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging20.Move");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[Move]];

                #region Analyze request

                //VideoSourceToken
                CommonCompare.StringCompare("RequestParameters/VideoSourceToken", "VideoSourceToken", VideoSourceToken, ref logMessage, ref passed, test);

                //Focus
                if (Focus != null)
                {
                    //Absolute
                    if (Focus.Absolute != null)
                    {
                        CommonCompare.Exist("RequestParameters/Focus/Absolute", "Focus.Absolute", Focus.Absolute, ref logMessage, ref passed, test);
                        if (passed)
                        {
                            //Position
                            CommonCompare.FloatCompare("RequestParameters/Focus/Absolute/Position", "Focus.Absolute.Position", Focus.Absolute.Position, ref logMessage, ref passed, test);

                            //Speed
                            if (Focus.Absolute.SpeedSpecified)
                            {
                                CommonCompare.Exist2("RequestParameters/Focus/Absolute/Speed", "Focus.Absolute.Speed", Focus.Absolute.SpeedSpecified, ref logMessage, ref passed, test);
                                if (passed)
                                {
                                    CommonCompare.FloatCompare("RequestParameters/Focus/Absolute/Speed", "Focus.Absolute.Speed", Focus.Absolute.Speed, ref logMessage, ref passed, test);
                                }
                            }
                            else
                            {
                                CommonCompare.Exist2("RequestParameters/Focus/Absolute/Speed", "Focus.Absolute.Speed", Focus.Absolute.SpeedSpecified, ref logMessage, ref passed, test);
                            }
                        }
                    }
                    else
                    {
                        CommonCompare.Exist("RequestParameters/Focus/Absolute", "Focus.Absolute", Focus.Absolute, ref logMessage, ref passed, test);
                    }
                    //Relative
                    if (Focus.Relative != null)
                    {
                        CommonCompare.Exist("RequestParameters/Focus/Relative", "Focus.Relative", Focus.Relative, ref logMessage, ref passed, test);
                        if (passed)
                        {
                            //Distance
                            CommonCompare.FloatCompare("RequestParameters/Focus/Relative/Distance", "Focus.Relative.Distance", Focus.Relative.Distance, ref logMessage, ref passed, test);

                            //Speed
                            if (Focus.Relative.SpeedSpecified)
                            {
                                CommonCompare.Exist2("RequestParameters/Focus/Relative/Speed", "Focus.Relative.Speed", Focus.Relative.SpeedSpecified, ref logMessage, ref passed, test);
                                if (passed)
                                {
                                    CommonCompare.FloatCompare("RequestParameters/Focus/Relative/Speed", "Focus.Relative.Speed", Focus.Relative.Speed, ref logMessage, ref passed, test);
                                }
                            }
                            else
                            {
                                CommonCompare.Exist2("RequestParameters/Focus/Relative/Speed", "Focus.Relative.Speed", Focus.Relative.SpeedSpecified, ref logMessage, ref passed, test);
                            }
                        }
                    }
                    else
                    {
                        CommonCompare.Exist("RequestParameters/Focus/Relative", "Focus.Relative", Focus.Relative, ref logMessage, ref passed, test);
                    }

                    //Continuous
                    if (Focus.Continuous != null)
                    {
                        CommonCompare.Exist("RequestParameters/Focus/Continuous", "Focus.Continuous", Focus.Continuous, ref logMessage, ref passed, test);
                        if (passed)
                        {
                            //Speed
                            CommonCompare.FloatCompare("RequestParameters/Focus/Continuous/Speed", "Focus.Continuous.Speed", Focus.Continuous.Speed, ref logMessage, ref passed, test);
                        }
                    }
                    else
                    {
                        CommonCompare.Exist("RequestParameters/Focus/Continuous", "Focus.Continuous", Focus.Continuous, ref logMessage, ref passed, test);
                    }
                }
                else
                {
                    passed = false;
                    logMessage = logMessage + "No required tag Focus.";
                }


                #endregion //Analyze request

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, Move);
            }
            else
            {
                res = StepType.None;
                Timeout = 0;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType GetMoveOptionsTest(out MoveOptions20 target, out SoapException ex, out int Timeout, string VideoSourceToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging20.GetMoveOptions");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[GetMoveOptions]];

                #region Serialization Temp
                //Media.VideoEncoderConfigurationOptions dsr = new Media.VideoEncoderConfigurationOptions();
                //dsr.JPEG = new Media.JpegOptions();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.VideoEncoderConfigurationOptions));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //VideoSourceToken
                CommonCompare.StringCompare("RequestParameters/VideoSourceToken", "VideoSourceToken", VideoSourceToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(MoveOptions20));
                target = (MoveOptions20)targetObj;

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetMoveOptions);
            }
            else
            {
                res = StepType.None;
                Timeout = 0;
                target = null;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType StopTest(out SoapException ex, out int Timeout, string VideoSourceToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging20.Stop");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[Stop]];

                #region Analyze request

                //VideoSourceToken
                CommonCompare.StringCompare("RequestParameters/VideoSourceToken", "VideoSourceToken", VideoSourceToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, Stop);
            }
            else
            {
                res = StepType.None;
                Timeout = 0;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType GetStatusTest(out ImagingStatus20 target, out SoapException ex, out int Timeout, string VideoSourceToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging20.GetStatus");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[GetStatus]];

                #region Serialization Temp
                //Imaging10.ImagingStatus dsr = new Imaging10.ImagingStatus();
                //dsr.FocusStatus = new Imaging10.FocusStatus();
                //dsr.FocusStatus.Position = 12;
                //dsr.FocusStatus.MoveStatus = Imaging10.MoveStatus.IDLE;
                //dsr.FocusStatus.Error = "Test";
                //XmlSerializer serializer = new XmlSerializer(typeof(Imaging10.ImagingStatus));
                ////XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //VideoSourceToken
                CommonCompare.StringCompare("RequestParameters/VideoSourceToken", "VideoSourceToken", VideoSourceToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(ImagingStatus20));
                target = (ImagingStatus20)targetObj;

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetStatus);
            }
            else
            {
                res = StepType.None;
                Timeout = 0;
                target = null;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType GetServiceCapabilitiesTest(out Capabilities target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetServiceCapabilities";
            int tmpCommandNumber = GetServiceCapabilities;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Capabilities));
                target = (Capabilities)targetObj;

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, tmpCommandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + tmpCommandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }
    }
}
