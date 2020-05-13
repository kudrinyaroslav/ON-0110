using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.IO;
using System.Xml.Serialization;
using DUT.CameraWebService;
using CameraWebService;

namespace DUT.CameraWebService.Door10
{



    /// <summary>
    /// Class for Search Service tests
    /// </summary>
    public class DoorServiceTest
    {

        #region Const

        private const string ServiceName = "Door10";

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetServiceCapabilities = 0;
        private const int GetDoorState = 1;
        private const int GetDoorInfoList = 2;
        private const int AccessDoor = 3;
        private const int LockDoor = 4;
        private const int UnlockDoor = 5;
        private const int BlockDoor = 6;
        private const int LockDownDoor = 7;
        private const int LockDownReleaseDoor = 8;
        private const int LockOpenDoor = 9;
        private const int LockOpenReleaseDoor = 10;
        private const int DoubleLockDoor = 11;
        private const int MaxCommands = 12;

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
        public DoorServiceTest(TestCommon testCommon)
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

        //***************************************************************************************

        #region General

        internal StepType GetServiceCapabilitiesTest(out ServiceCapabilities target, out SoapException ex, out int Timeout)
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
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(ServiceCapabilities));
                target = (ServiceCapabilities)targetObj;

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

        #endregion //General

        //***************************************************************************************

        #region Door

        internal StepType GetDoorStateTest(out DoorState target, out SoapException ex, out int Timeout, string Token)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetDoorState";
            int tmpCommandNumber = GetDoorState;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //Token
                CommonCompare.StringCompare("RequestParameters/Token", "Token", Token, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(DoorState));
                target = (DoorState)targetObj;

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

        internal StepType GetDoorInfoListTest(out DoorInfo[] target, out SoapException ex, out int Timeout, string[] TokenList, int Limit, bool LimitSpecified, int Offset, bool OffsetSpecified)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetDoorInfoList";
            int tmpCommandNumber = GetDoorInfoList;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //TokenList
                CommonCompare.StringArrayCompare("RequestParameters/Token", "Token", TokenList, ref logMessage, ref passed, test);

                //Limit
                if (CommonCompare.Exist2("RequestParameters/Limit", "Limit", LimitSpecified, ref logMessage, ref passed, test))
                {
                    CommonCompare.IntCompare("RequestParameters/Limit", "Limit", Limit, ref logMessage, ref passed, test);
                }

                //Offset
                if (CommonCompare.Exist2("RequestParameters/Offset", "Offset", OffsetSpecified, ref logMessage, ref passed, test))
                {
                    CommonCompare.IntCompare("RequestParameters/Offset", "Offset", Offset, ref logMessage, ref passed, test);
                }

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(DoorInfo[]));
                target = (DoorInfo[])targetObj;

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

        #endregion //Door

        //***************************************************************************************

        #region DoorControl

        internal StepType AccessDoorTest(out SoapException ex, out int Timeout, string Token, bool UseExtendedTime, bool UseExtendedTimeSpecified, string AccessTime, string OpenTooLongTime, string PreAlarmTime, AccessDoorExtension Extension)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "AccessDoor";
            int tmpCommandNumber = AccessDoor;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //Token
                CommonCompare.StringCompare("RequestParameters/Token", "Token", Token, ref logMessage, ref passed, test);

                //UseExtendedTime
                if (CommonCompare.Exist2("RequestParameters/UseExtendedTime", "UseExtendedTime", UseExtendedTimeSpecified, ref logMessage, ref passed, test))
                {
                    CommonCompare.StringCompare("RequestParameters/UseExtendedTime", "UseExtendedTime", UseExtendedTime.ToString(), ref logMessage, ref passed, test);
                }

                //AccessTime
                CommonCompare.StringCompare("RequestParameters/AccessTime", "AccessTime", AccessTime, ref logMessage, ref passed, test);

                //OpenTooLongTime
                CommonCompare.StringCompare("RequestParameters/OpenTooLongTime", "OpenTooLongTime", OpenTooLongTime, ref logMessage, ref passed, test);

                //PreAlarmTime
                CommonCompare.StringCompare("RequestParameters/PreAlarmTime", "PreAlarmTime", PreAlarmTime, ref logMessage, ref passed, test);

                //Extension
                //TODO

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

        internal StepType LockDoorTest(out SoapException ex, out int Timeout, string Token)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "LockDoor";
            int tmpCommandNumber = LockDoor;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //Token
                CommonCompare.StringCompare("RequestParameters/Token", "Token", Token, ref logMessage, ref passed, test);

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

        #endregion //DoorControl

        //***************************************************************************************



        internal StepType UnlockDoorTest(out SoapException ex, out int Timeout, string Token)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "UnlockDoor";
            int tmpCommandNumber = UnlockDoor;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //Token
                CommonCompare.StringCompare("RequestParameters/Token", "Token", Token, ref logMessage, ref passed, test);

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

        internal StepType BlockDoorTest(out SoapException ex, out int Timeout, string Token)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "BlockDoor";
            int tmpCommandNumber = BlockDoor;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //Token
                CommonCompare.StringCompare("RequestParameters/Token", "Token", Token, ref logMessage, ref passed, test);

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

        internal StepType LockDownDoorTest(out SoapException ex, out int Timeout, string Token)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "LockDownDoor";
            int tmpCommandNumber = LockDownDoor;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //Token
                CommonCompare.StringCompare("RequestParameters/Token", "Token", Token, ref logMessage, ref passed, test);

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

        internal StepType LockDownReleaseDoorTest(out SoapException ex, out int Timeout, string Token)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "LockDownReleaseDoor";
            int tmpCommandNumber = LockDownReleaseDoor;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //Token
                CommonCompare.StringCompare("RequestParameters/Token", "Token", Token, ref logMessage, ref passed, test);

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

        internal StepType LockOpenDoorTest(out SoapException ex, out int Timeout, string Token)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "LockOpenDoor";
            int tmpCommandNumber = LockOpenDoor;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //Token
                CommonCompare.StringCompare("RequestParameters/Token", "Token", Token, ref logMessage, ref passed, test);

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

        internal StepType LockOpenReleaseDoorTest(out SoapException ex, out int Timeout, string Token)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "LockOpenReleaseDoor";
            int tmpCommandNumber = LockOpenReleaseDoor;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //Token
                CommonCompare.StringCompare("RequestParameters/Token", "Token", Token, ref logMessage, ref passed, test);

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

        internal StepType DoubleLockDoorTest(out SoapException ex, out int Timeout, string Token)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "DoubleLockDoor";
            int tmpCommandNumber = DoubleLockDoor;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //Token
                CommonCompare.StringCompare("RequestParameters/Token", "Token", Token, ref logMessage, ref passed, test);

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
    }
}
