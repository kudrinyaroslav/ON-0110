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
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Replay10
{



    /// <summary>
    /// Class for Search Service tests
    /// </summary>
    public class ReplayServiceTest : Base.BaseServiceTest
    {

        #region Const

        protected override string ServiceName { get { return "Replay10"; } }

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetServiceCapabilities = 0;
        private const int GetReplayUri = 1;
        private const int GetReplayConfiguration = 2;
        private const int SetReplayConfiguration = 3;
        private const int MaxCommands = 4;

        #endregion //Const

 
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ReplayServiceTest(TestCommon testCommon)
            :base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors


        //***************************************************************************************

        #region General

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
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Capabilities));
                target = (Capabilities)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, tmpCommandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + tmpCommandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        // to be used after refactoring
        internal Capabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<Capabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, out stepType, out ex, out Timeout);
        }

        internal string GetReplayUriTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("GetReplayUri", GetReplayUri, validationRequest, out stepType, out ex, out Timeout);
        }

        internal ReplayConfiguration GetReplayConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<ReplayConfiguration>("GetReplayConfiguration", GetReplayConfiguration, validationRequest, out stepType, out ex, out Timeout);
        }
        internal void SetReplayConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("SetReplayConfiguration", SetReplayConfiguration, validationRequest, out stepType, out ex, out Timeout);
        }
        
        #endregion //General

        //***************************************************************************************


        //***************************************************************************************





        
    }
}
