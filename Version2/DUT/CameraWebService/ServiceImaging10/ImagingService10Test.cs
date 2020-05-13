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

namespace DUT.CameraWebService.Imaging10
{



    /// <summary>
    /// Class for Device Management Service tests
    /// </summary>
    public class ImagingService10Test
    {

        #region Const

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
        private const int MaxCommands = 7;

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
        public ImagingService10Test(TestCommon testCommon)
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

        internal StepType GetImagingSettingsTest(out Imaging10.ImagingSettings target, out SoapException ex, out int Timeout, string VideoSourceToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging10.GetImagingSettings");

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
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Imaging10.ImagingSettings));
                target = (Imaging10.ImagingSettings)targetObj;

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

        internal StepType SetImagingSettingsTest(out SoapException ex, out int Timeout, string VideoSourceToken, Imaging10.ImagingSettings ImagingSettings, bool ForcePersistence, bool ForcePersistenceSpecified)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("SetImagingSettings");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[SetImagingSettings]];

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

        internal StepType GetOptionsTest(out Imaging10.ImagingOptions target, out SoapException ex, out int Timeout, string VideoSourceToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging10.GetOptions");

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
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Imaging10.ImagingOptions));
                target = (Imaging10.ImagingOptions)targetObj;

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

        internal StepType MoveTest(out SoapException ex, out int Timeout, string VideoSourceToken, Imaging10.FocusMove Focus)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging10.Move");

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

        internal StepType GetMoveOptionsTest(out Imaging10.MoveOptions target, out SoapException ex, out int Timeout, string VideoSourceToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging10.GetMoveOptions");

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
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Imaging10.MoveOptions));
                target = (Imaging10.MoveOptions)targetObj;

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
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging10.Stop");

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

        internal StepType GetStatusTest(out Imaging10.ImagingStatus target, out SoapException ex, out int Timeout, string VideoSourceToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Imaging10.GetStatus");

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
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Imaging10.ImagingStatus));
                target = (Imaging10.ImagingStatus)targetObj;

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
    }
}
