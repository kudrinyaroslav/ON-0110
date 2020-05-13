using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml.Serialization;
using DUT.CameraWebService;

namespace DUT.CameraWebService.PTZ20
{
    public class PTZServiceTest
    {
        #region Const

        private const string ServiceName = "PTZ20";

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetNodes = 0;
        private const int GetNode = 1;
        private const int GetConfigurations = 2;
        private const int GetConfiguration = 3;
        private const int GetConfigurationOptions = 4;
        private const int SetConfiguration = 5;
        private const int GetStatus = 6;
        private const int AbsoluteMove = 7;
        private const int SendAuxiliaryCommand = 8;
        private const int RelativeMove = 9;
        private const int ContinuousMove = 10;
        private const int Stop = 11;
        private const int SetPreset = 12;
        private const int GetPresets = 13;
        private const int RemovePreset = 14;
        private const int SetHomePosition = 15;
        private const int GotoHomePosition = 16;
        private const int GotoPreset = 17;
        private const int GetServiceCapabilities = 18;
        private const int GetCompatibleConfigurations = 19;
        private const int MaxCommands = 20;

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
        public PTZServiceTest(TestCommon testCommon)
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

        /// <summary>
        /// Reset test
        /// </summary>
        public void ResetTestSuit()
        {
            for (int i = 0; i < m_commandCount.Length; i++)
            {
                m_commandCount[i] = 0;
            }
        }

        #endregion //General

        #region PTZNode

        internal StepType GetNodesTest(out PTZNode[] target, out System.Web.Services.Protocols.SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetNodes";
            int tmpCommandNumber = GetNodes;


            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            //TEMP: for backward compatibility
            if (m_testList.Count == 0)
            {
                m_testList = m_TestCommon.GetStepsForCommand(tmpCommandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(PTZNode[]));
                target = (PTZNode[])targetObj;

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

        internal StepType GetNodeTest(out PTZNode target, out SoapException ex, out int Timeout, string NodeToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetNode";
            int tmpCommandNumber = GetNode;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            //TEMP: for backward compatibility
            if (m_testList.Count == 0)
            {
                m_testList = m_TestCommon.GetStepsForCommand(tmpCommandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //NodeToken
                CommonCompare.StringCompare("RequestParameters/NodeToken", "NodeToken", NodeToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(PTZNode));
                target = (PTZNode)targetObj;

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

        #endregion //PTZNode

        #region PTZConfiguration

        internal StepType GetConfigurationsTest(out PTZConfiguration[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetConfigurations";
            int tmpCommandNumber = GetConfigurations;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            //TEMP: for backward compatibility
            if (m_testList.Count == 0)
            {
                m_testList = m_TestCommon.GetStepsForCommand(tmpCommandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(PTZConfiguration[]));
                target = (PTZConfiguration[])targetObj;

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

        internal StepType GetConfigurationTest(out PTZConfiguration target, out SoapException ex, out int Timeout, string PTZConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetConfiguration";
            int tmpCommandNumber = GetConfiguration;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            //TEMP: for backward compatibility
            if (m_testList.Count == 0)
            {
                m_testList = m_TestCommon.GetStepsForCommand(tmpCommandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //PTZConfigurationToken
                CommonCompare.StringCompare("RequestParameters/PTZConfigurationToken", "PTZConfigurationToken", PTZConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(PTZConfiguration));
                target = (PTZConfiguration)targetObj;

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

        internal StepType GetConfigurationOptionsTest(out PTZConfigurationOptions target, out SoapException ex, out int Timeout, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetConfigurationOptions";
            int tmpCommandNumber = GetConfigurationOptions;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            //TEMP: for backward compatibility
            if (m_testList.Count == 0)
            {
                m_testList = m_TestCommon.GetStepsForCommand(tmpCommandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(PTZConfigurationOptions));
                target = (PTZConfigurationOptions)targetObj;

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

        internal StepType SetConfigurationTest(out SoapException ex, out int Timeout, PTZConfiguration PTZConfiguration, bool ForcePersistence)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "SetConfiguration";
            int tmpCommandNumber = SetConfiguration;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            //TEMP: for backward compatibility
            if (m_testList.Count == 0)
            {
                m_testList = m_TestCommon.GetStepsForCommand(tmpCommandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //ForcePersistence
                CommonCompare.StringCompare("RequestParameters/ForcePersistence", "ForcePersistence", ForcePersistence.ToString(), ref logMessage, ref passed, test);

                //PTZConfiguration
                if (PTZConfiguration != null)
                {
                    //token
                    CommonCompare.StringCompare("RequestParameters/PTZConfiguration/@token", "token", PTZConfiguration.token, ref logMessage, ref passed, test);
                    //Name
                    CommonCompare.StringCompare("RequestParameters/PTZConfiguration/Name", "Name", PTZConfiguration.Name, ref logMessage, ref passed, test);
                    //NodeToken
                    CommonCompare.StringCompare("RequestParameters/PTZConfiguration/NodeToken", "NodeToken", PTZConfiguration.NodeToken, ref logMessage, ref passed, test);
                    //DefaultAbsolutePantTiltPositionSpace
                    CommonCompare.StringCompare("RequestParameters/PTZConfiguration/DefaultAbsolutePantTiltPositionSpace", "DefaultAbsolutePantTiltPositionSpace", PTZConfiguration.DefaultAbsolutePantTiltPositionSpace, ref logMessage, ref passed, test);
                    //DefaultAbsoluteZoomPositionSpace
                    CommonCompare.StringCompare("RequestParameters/PTZConfiguration/DefaultAbsoluteZoomPositionSpace", "DefaultAbsoluteZoomPositionSpace", PTZConfiguration.DefaultAbsoluteZoomPositionSpace, ref logMessage, ref passed, test);
                    //DefaultRelativePanTiltTranslationSpace
                    CommonCompare.StringCompare("RequestParameters/PTZConfiguration/DefaultRelativePanTiltTranslationSpace", "DefaultRelativePanTiltTranslationSpace", PTZConfiguration.DefaultRelativePanTiltTranslationSpace, ref logMessage, ref passed, test);
                    //DefaultRelativeZoomTranslationSpace
                    CommonCompare.StringCompare("RequestParameters/PTZConfiguration/DefaultRelativeZoomTranslationSpace", "DefaultRelativeZoomTranslationSpace", PTZConfiguration.DefaultRelativeZoomTranslationSpace, ref logMessage, ref passed, test);
                    //DefaultContinuousPanTiltVelocitySpace
                    CommonCompare.StringCompare("RequestParameters/PTZConfiguration/DefaultContinuousPanTiltVelocitySpace", "DefaultContinuousPanTiltVelocitySpace", PTZConfiguration.DefaultContinuousPanTiltVelocitySpace, ref logMessage, ref passed, test);
                    //DefaultContinuousZoomVelocitySpace
                    CommonCompare.StringCompare("RequestParameters/PTZConfiguration/DefaultContinuousZoomVelocitySpace", "DefaultContinuousZoomVelocitySpace", PTZConfiguration.DefaultContinuousZoomVelocitySpace, ref logMessage, ref passed, test);
                    //DefaultPTZTimeout
                    CommonCompare.StringCompare("RequestParameters/PTZConfiguration/DefaultPTZTimeout", "DefaultPTZTimeout", PTZConfiguration.DefaultPTZTimeout, ref logMessage, ref passed, test);
                    //PanTiltLimits
                    if (CommonCompare.Exist("RequestParameters/PTZConfiguration/PanTiltLimits", "PanTiltLimits", (object)(PTZConfiguration.PanTiltLimits), ref logMessage, ref passed, test))
                    {
                        //PTZConfiguration.PanTiltLimits.Range
                        if (CommonCompare.Exist("RequestParameters/PTZConfiguration/PanTiltLimits/Range", "PanTiltLimits.Range", (object)(PTZConfiguration.PanTiltLimits.Range), ref logMessage, ref passed, test))
                        {
                            //PTZConfiguration.PanTiltLimits.Range.URI
                            CommonCompare.StringCompare("RequestParameters/PTZConfiguration/PanTiltLimits/Range/URI", "PanTiltLimits.Range.URI", PTZConfiguration.PanTiltLimits.Range.URI, ref logMessage, ref passed, test);

                            //PTZConfiguration.PanTiltLimits.Range.XRange
                            if (CommonCompare.Exist("RequestParameters/PTZConfiguration/PanTiltLimits/Range/XRange", "PanTiltLimits.Range.XRange", (object)(PTZConfiguration.PanTiltLimits.Range.XRange), ref logMessage, ref passed, test))
                            {
                                //PTZConfiguration.PanTiltLimits.Range.XRange.Min
                                CommonCompare.FloatCompare("RequestParameters/PTZConfiguration/PanTiltLimits/Range/XRange/Min", "PanTiltLimits.Range.XRange.Min", PTZConfiguration.PanTiltLimits.Range.XRange.Min, ref logMessage, ref passed, test);

                                //PTZConfiguration.PanTiltLimits.Range.XRange.Max
                                CommonCompare.FloatCompare("RequestParameters/PTZConfiguration/PanTiltLimits/Range/XRange/Max", "PanTiltLimits.Range.XRange.Max", PTZConfiguration.PanTiltLimits.Range.XRange.Max, ref logMessage, ref passed, test);
                            }

                            //PTZConfiguration.PanTiltLimits.Range.YRange
                            if (CommonCompare.Exist("RequestParameters/PTZConfiguration/PanTiltLimits/Range/YRange", "PanTiltLimits.Range.YRange", (object)(PTZConfiguration.PanTiltLimits.Range.YRange), ref logMessage, ref passed, test))
                            {
                                //PTZConfiguration.PanTiltLimits.Range.YRange.Min
                                CommonCompare.FloatCompare("RequestParameters/PTZConfiguration/PanTiltLimits/Range/YRange/Min", "PanTiltLimits.Range.YRange.Min", PTZConfiguration.PanTiltLimits.Range.YRange.Min, ref logMessage, ref passed, test);

                                //PTZConfiguration.PanTiltLimits.Range.YRange.Max
                                CommonCompare.FloatCompare("RequestParameters/PTZConfiguration/PanTiltLimits/Range/YRange/Max", "PanTiltLimits.Range.YRange.Max", PTZConfiguration.PanTiltLimits.Range.YRange.Max, ref logMessage, ref passed, test);
                            }

                        }

                    }
                    
                    //ZoomLimits
                    if (CommonCompare.Exist("RequestParameters/PTZConfiguration/ZoomLimits", "ZoomLimits", (object)(PTZConfiguration.ZoomLimits), ref logMessage, ref passed, test))
                    {
                        //PTZConfiguration.ZoomLimits.Range.URI
                        CommonCompare.StringCompare("RequestParameters/PTZConfiguration/ZoomLimits/Range/URI", "ZoomLimits.Range.URI", PTZConfiguration.ZoomLimits.Range.URI, ref logMessage, ref passed, test);

                        //PTZConfiguration.ZoomLimits.Range.XRange
                        if (CommonCompare.Exist("RequestParameters/PTZConfiguration/ZoomLimits/Range/XRange", "ZoomLimits.Range.XRange", (object)(PTZConfiguration.ZoomLimits.Range.XRange), ref logMessage, ref passed, test))
                        {
                            //PTZConfiguration.ZoomLimits.Range.XRange.Min
                            CommonCompare.FloatCompare("RequestParameters/PTZConfiguration/ZoomLimits/Range/XRange/Min", "ZoomLimits.Range.XRange.Min", PTZConfiguration.ZoomLimits.Range.XRange.Min, ref logMessage, ref passed, test);

                            //PTZConfiguration.ZoomLimits.Range.XRange.Max
                            CommonCompare.FloatCompare("RequestParameters/PTZConfiguration/ZoomLimits/Range/XRange/Max", "ZoomLimits.Range.XRange.Max", PTZConfiguration.ZoomLimits.Range.XRange.Max, ref logMessage, ref passed, test);
                        }
                    }

                    //DefaultPTZSpeed
                    if (CommonCompare.Exist("RequestParameters/PTZConfiguration/DefaultPTZSpeed", "DefaultPTZSpeed", (object)(PTZConfiguration.DefaultPTZSpeed), ref logMessage, ref passed, test))
                    {
                        //DefaultPTZSpeed.PanTilt
                        if (CommonCompare.Exist("RequestParameters/PTZConfiguration/DefaultPTZSpeed/PanTilt", "DefaultPTZSpeed.PanTilt", (object)(PTZConfiguration.DefaultPTZSpeed.PanTilt), ref logMessage, ref passed, test))
                        {
                            //DefaultPTZSpeed.PanTilt.space
                            CommonCompare.StringCompare("RequestParameters/PTZConfiguration/DefaultPTZSpeed/PanTilt/space", "DefaultPTZSpeed.PanTilt.space", PTZConfiguration.DefaultPTZSpeed.PanTilt.space, ref logMessage, ref passed, test);
                            //DefaultPTZSpeed.PanTilt.x
                            CommonCompare.FloatCompare("RequestParameters/PTZConfiguration/DefaultPTZSpeed/PanTilt/x", "DefaultPTZSpeed.PanTilt.x", PTZConfiguration.DefaultPTZSpeed.PanTilt.x, ref logMessage, ref passed, test);
                            //DefaultPTZSpeed.PanTilt.y
                            CommonCompare.FloatCompare("RequestParameters/PTZConfiguration/DefaultPTZSpeed/PanTilt/y", "DefaultPTZSpeed.PanTilt.y", PTZConfiguration.DefaultPTZSpeed.PanTilt.y, ref logMessage, ref passed, test);
                        }

                        //DefaultPTZSpeed.Zoom
                        if (CommonCompare.Exist("RequestParameters/PTZConfiguration/DefaultPTZSpeed/Zoom", "DefaultPTZSpeed.PanTilt", (object)(PTZConfiguration.DefaultPTZSpeed.PanTilt), ref logMessage, ref passed, test))
                        {
                            //DefaultPTZSpeed.Zoom.space
                            CommonCompare.StringCompare("RequestParameters/PTZConfiguration/DefaultPTZSpeed/Zoom/space", "DefaultPTZSpeed.Zoom.space", PTZConfiguration.DefaultPTZSpeed.Zoom.space, ref logMessage, ref passed, test);
                            //DefaultPTZSpeed.Zoom.x
                            CommonCompare.FloatCompare("RequestParameters/PTZConfiguration/DefaultPTZSpeed/Zoom/x", "DefaultPTZSpeed.Zoom.x", PTZConfiguration.DefaultPTZSpeed.Zoom.x, ref logMessage, ref passed, test);
                        }
                    }
                }
                else 
                {
                    passed = false;
                    logMessage = logMessage + "No  PTZConfiguration. ";
                }

                #endregion //Analyze request

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

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

        #endregion //PTZConfiguration

        #region PTZMove

        internal StepType GetStatusTest(out PTZStatus target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, "GetStatus"));

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[GetStatus]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(PTZStatus));
                target = (PTZStatus)targetObj;

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

        internal StepType AbsoluteMoveTest(out SoapException ex, out int Timeout, string ProfileToken, PTZVector Position, PTZSpeed Speed)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, "AbsoluteMove"));

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[AbsoluteMove]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //Position
                CommonCompare.PTZPositionCompare("RequestParameters/Position", "Position", Position, ref logMessage, ref passed, test);

                //Speed
                CommonCompare.PTZSpeedCompare("RequestParameters/Speed", "Speed", Speed, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, AbsoluteMove);
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

        #endregion //PTZMove

        #region PTZAux

        internal StepType SendAuxiliaryCommandTest(out string target, out SoapException ex, out int Timeout, string ProfileToken, string AuxiliaryData)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("SendAuxiliaryCommand");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[SendAuxiliaryCommand]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //AuxiliaryData
                CommonCompare.StringCompare("RequestParameters/AuxiliaryData", "AuxiliaryData", AuxiliaryData, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(string));
                target = (string)targetObj;

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SendAuxiliaryCommand);
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

        #endregion //PTZAux

        internal StepType RelativeMoveTest(out SoapException ex, out int Timeout, string ProfileToken, PTZVector Translation, PTZSpeed Speed)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, "RelativeMove"));

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[RelativeMove]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //Translation
                CommonCompare.PTZPositionCompare("RequestParameters/Translation", "Translation", Translation, ref logMessage, ref passed, test);

                //Speed
                CommonCompare.PTZSpeedCompare("RequestParameters/Speed", "Speed", Speed, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, RelativeMove);
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

        internal StepType ContinuousMoveTest(out SoapException ex, out int Timeout, string ProfileToken, PTZSpeed Velocity, string verTimeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, "ContinuousMove"));

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[ContinuousMove]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //Velocity
                CommonCompare.PTZSpeedCompare("RequestParameters/Velocity", "Velocity", Velocity, ref logMessage, ref passed, test);

                //verTimeout
                CommonCompare.StringCompare("RequestParameters/Timeout", "Timeout", verTimeout, ref logMessage, ref passed, test);
                
                #endregion //Analyze request

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, ContinuousMove);
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

        internal StepType StopTest(out SoapException ex, out int Timeout, string ProfileToken, bool PanTilt, bool PanTiltSpecified, bool Zoom, bool ZoomSpecified)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, "Stop"));

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[Stop]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //PanTilt
                //TestCommon.NotReqBoolCompare("RequestParameters/PanTilt", "PanTilt", PanTilt, PanTiltSpecified, ref logMessage, ref passed, test);

                //Zoom
                //TestCommon.NotReqBoolCompare("RequestParameters/Zoom", "Zoom", Zoom, ZoomSpecified, ref logMessage, ref passed, test);

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

        internal StepType SetPresetTest(out SoapException ex, out int Timeout, string ProfileToken, string PresetName, ref string PresetToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, "SetPreset"));

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[SetPreset]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //PresetName
                CommonCompare.StringCompare("RequestParameters/PresetName", "PresetName", PresetName, ref logMessage, ref passed, test);

                //PresetToken
                CommonCompare.StringCompare("RequestParameters/PresetToken", "PresetToken", PresetToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(string));
                PresetToken = (string)targetObj;

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetPreset);
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

        internal StepType GetPresetsTest(out PTZPreset[] target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, "GetPresets"));

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[GetPresets]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(PTZPreset[]));
                target = (PTZPreset[])targetObj;

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetPresets);
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

        internal StepType RemovePresetTest(out SoapException ex, out int Timeout, string ProfileToken, string PresetToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, "RemovePreset"));

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[RemovePreset]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //PresetToken
                CommonCompare.StringCompare("RequestParameters/PresetToken", "PresetToken", PresetToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, RemovePreset);
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

        internal StepType SetHomePositionTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, "SetHomePosition"));

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[SetHomePosition]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetHomePosition);
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

        internal StepType GotoHomePositionTest(out SoapException ex, out int Timeout, string ProfileToken, PTZSpeed Speed)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, "GotoHomePosition"));

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[GotoHomePosition]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //Speed
                CommonCompare.PTZSpeedCompare("RequestParameters/Speed", "Speed", Speed, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GotoHomePosition);
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

        internal StepType GotoPresetTest(out SoapException ex, out int Timeout, string ProfileToken, string PresetToken, PTZSpeed Speed)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, "GotoPreset"));

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[GotoPreset]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //PresetToken
                CommonCompare.StringCompare("RequestParameters/PresetToken", "PresetToken", PresetToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GotoPreset);
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
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, tmpCommandName));

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

        internal StepType GetCompatibleConfigurationsTest(out PTZConfiguration[] target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(string.Format("{0}.{1}", ServiceName, "GetCompatibleConfigurations"));

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[GetCompatibleConfigurations]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(PTZConfiguration[]));
                target = (PTZConfiguration[])targetObj;

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetCompatibleConfigurations);
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
