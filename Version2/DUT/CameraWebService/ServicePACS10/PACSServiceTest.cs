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

namespace DUT.CameraWebService.PACS10
{



    /// <summary>
    /// Class for Search Service tests
    /// </summary>
    public class PACSServiceTest
    {

        #region Const

        private const string ServiceName = "PACS10";

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetServiceCapabilities = 0;
        private const int GetAreaInfoList = 1;
        private const int GetAccessPointInfoList = 2;
        private const int GetAccessControllerInfoList = 3;
        private const int MaxCommands = 4;

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
        public PACSServiceTest(TestCommon testCommon)
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

        #region Area


        internal StepType GetAreaInfoListTest(out AreaInfo[] target, out SoapException ex, out int Timeout, string[] TokenList)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetAreaInfoList";
            int tmpCommandNumber = GetAreaInfoList;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //TokenList
                CommonCompare.StringArrayCompare("RequestParameters/Token", "Token", TokenList, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AreaInfo[]));
                target = (AreaInfo[])targetObj;

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

        #endregion //Area

        //***************************************************************************************

        #region Access Point

        internal StepType GetAccessPointInfoListTest(out AccessPointInfo[] target, out SoapException ex, out int Timeout, string[] TokenList)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetAccessPointInfoList";
            int tmpCommandNumber = GetAccessPointInfoList;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Serialization Temp
                //AccessPointInfo[] dsr = new AccessPointInfo[1];
                //dsr[0] = new AccessPointInfo();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].Entity = "Device";
                //dsr[0].Enabled = true;
                //dsr[0].Type = "not:Device";
                //XmlSerializer serializer = new XmlSerializer(typeof(AccessPointInfo[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("d:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //TokenList
                CommonCompare.StringArrayCompare("RequestParameters/Token", "Token", TokenList, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AccessPointInfo[]));
                target = (AccessPointInfo[])targetObj;

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

        #endregion //Access Point

        //***************************************************************************************

        #region Access Point

        internal StepType GetAccessControllerInfoListTest(out AccessControllerInfo[] target, out SoapException ex, out int Timeout, string[] TokenList)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetAccessControllerInfoList";
            int tmpCommandNumber = GetAccessControllerInfoList;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                #region Analyze request

                //TokenList
                CommonCompare.StringArrayCompare("RequestParameters/Token", "Token", TokenList, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AccessControllerInfo[]));
                target = (AccessControllerInfo[])targetObj;

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

        #endregion //Access Point
    }
}
