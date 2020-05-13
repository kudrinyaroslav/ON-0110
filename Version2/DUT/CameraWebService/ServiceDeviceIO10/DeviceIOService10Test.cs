using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml.Serialization;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;


namespace DUT.CameraWebService.DeviceIO10
{
    /// <summary>
    /// Class for Device Management Service tests
    /// </summary>
    public class DeviceIO10ServiceTest : Base.BaseServiceTest
    {

        protected override string ServiceName
        {
            get { return "DeviceIO10"; }
        }

        #region Const

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetRelayOutputs = 0;
        private const int SetRelayOutputSettings = 1;
        private const int SetRelayOutputState = 2;
        private const int GetVideoSources = 3;
        private const int GetServiceCapabilitiesCount = 4;
        private const int GetDigitalInputs = 5;
        private const int GetDigitalInputConfigurationOptions = 6;
        private const int SetDigitalInputConfigurations = 7;
        private const int GetRelayOutputOptions = 8;
        private const int GetAudioSources = 9;
        private const int GetAudioOutputs = 10;
        private const int MaxCommands = 11;



        #endregion //Const

        #region Members

        /// <summary>
        /// Mass with command call count
        /// </summary>
        private int[] m_commandCount = new int[MaxCommands];
        /// <summary>
        /// Test suit description
        /// </summary>
        //TestCommon m_TestCommon = null;

        #endregion //Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public DeviceIO10ServiceTest(TestCommon testCommon)
            : base(testCommon)
        {
            InitCommandsCount(MaxCommands);
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

        #region Relay

        internal StepType GetRelayOutputsTest(out RelayOutput[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("DeviceIO10.GetRelayOutputs");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[GetRelayOutputs]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(RelayOutput[]));
                target = (RelayOutput[])targetObj;

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetRelayOutputs);
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

        internal StepType SetRelayOutputSettingsTest(out SoapException ex, out int Timeout, RelayOutput RelayOutput)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("DeviceIO10.SetRelayOutputSettings");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[SetRelayOutputSettings]];

                #region Analize request

                //RelayOutput
                CommonCompare.Exist("RequestParameters/RelayOutputToken", "RelayOutput", RelayOutput, ref logMessage, ref passed, test);

                if (passed)
                {

                    //RelayOutputToken
                    if (CommonCompare.IgnoreCheck("RequestParameters/RelayOutputToken", test))
                    {
                        logMessage = logMessage + " RelayOutputToken = " + RelayOutput.token + "; ";
                    }
                    else
                    {
                        CommonCompare.StringCompare("RequestParameters/RelayOutputToken", "RelayOutputToken", RelayOutput.token, ref logMessage, ref passed, test);
                    }

                    //Properties
                    if (RelayOutput.Properties != null)
                    {
                        //Mode
                        CommonCompare.StringCompare("RequestParameters/Properties/Mode", "Properties.Mode", RelayOutput.Properties.Mode.ToString(), ref logMessage, ref passed, test);

                        //DelayTime
                        CommonCompare.StringCompare("RequestParameters/Properties/DelayTime", "Properties.DelayTime", RelayOutput.Properties.DelayTime, ref logMessage, ref passed, test);

                        //IdleState
                        CommonCompare.StringCompare("RequestParameters/Properties/IdleState", "Properties.IdleState", RelayOutput.Properties.IdleState.ToString(), ref logMessage, ref passed, test);
                    }
                    else
                    {
                        passed = false;
                        logMessage = logMessage + "No required tag Properties.";
                    }
                }

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetRelayOutputSettings);
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

        internal StepType SetRelayOutputStateTest(out SoapException ex, out int Timeout, string RelayOutputToken, RelayLogicalState LogicalState)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("DeviceIO10.SetRelayOutputState");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[SetRelayOutputState]];

                #region Analize request

                //RelayOutputToken
                if (CommonCompare.IgnoreCheck("RequestParameters/RelayOutputToken", test))
                {
                    logMessage = logMessage + " RelayOutputToken = " + RelayOutputToken + "; ";
                }
                else
                {
                    CommonCompare.StringCompare("RequestParameters/RelayOutputToken", "RelayOutputToken", RelayOutputToken, ref logMessage, ref passed, test);
                }

                //LogicalState
                CommonCompare.StringCompare("RequestParameters/LogicalState", "LogicalState", LogicalState.ToString(), ref logMessage, ref passed, test);

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetRelayOutputState);
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

        #endregion //Relay

        #region VideoOutput

        internal string[] GetVideoSourcesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string[]>("GetVideoSources", GetVideoSources, validationRequest, true, out stepType, out exc, out timeout);
        }

        #endregion //VideoOutput

        #region AudioOutput

        internal string[] GetAudioOutputsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string[]>("GetAudioOutputs", GetAudioOutputs, validationRequest, true, out stepType, out exc, out timeout);
        }

        #endregion //AudioOutput

        internal StepType GetServiceCapabilitiesTest(out Capabilities1 target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("DeviceIO10.GetServiceCapabilities");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[GetServiceCapabilitiesCount]];

                #region Serialization Temp
                //Capabilities1 dsr = new Capabilities1();
                //dsr.AudioOutputs = 2;
                //dsr.AudioSources = 2;
                //dsr.RelayOutputs = 2;
                //XmlSerializer serializer1 = new XmlSerializer(typeof(Capabilities1));
                //TextWriter textWriter = new StreamWriter("d:\\2.txt");
                //serializer1.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Capabilities1));
                target = (Capabilities1)targetObj;

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetServiceCapabilitiesCount);
            }
            else
            {
                throw new SoapException("NO DeviceIO10.GetServiceCapabilities COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal DigitalInput[] GetDigitalInputsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<DigitalInput[]>("GetDigitalInputs", GetDigitalInputs, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal DigitalInputConfigurationInputOptions GetDigitalInputConfigurationOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<DigitalInputConfigurationInputOptions>("GetDigitalInputConfigurationOptions", GetDigitalInputConfigurationOptions, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void SetDigitalInputConfigurationsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetDigitalInputConfigurations", SetDigitalInputConfigurations, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal RelayOutputOptions[] GetRelayOutputOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<RelayOutputOptions[]>("GetRelayOutputOptions", GetRelayOutputOptions, validationRequest, true, out stepType, out exc, out timeout);
        }
        internal string[] GetAudioSourcesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string[]>("GetAudioSources", GetAudioSources, validationRequest, true, out stepType, out exc, out timeout);
        }
        
    }
}
