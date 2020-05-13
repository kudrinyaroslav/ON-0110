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

namespace DUT.CameraWebService.Media10
{



    /// <summary>
    /// Class for Device Management Service tests
    /// </summary>
    public class MediaServiceTest : Base.BaseServiceTest
    {

        #region Const

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetProfiles = 0;
        private const int GetSnapshotUri = 1;
        private const int GetVideoEncoderConfigurations = 2;
        private const int GetVideoEncoderConfigurationOptions = 3;
        private const int SetVideoEncoderConfiguration = 4;
        private const int GetVideoEncoderConfiguration = 5;
        private const int GetVideoSources = 6;
        private const int GetCompatibleVideoSourceConfigurations = 7;
        private const int GetVideoSourceConfigurations = 8;
        private const int GetVideoSourceConfigurationOptions = 9;
        private const int CreateProfile = 10;
        private const int GetAudioEncoderConfigurationOptions = 11;
        private const int SetAudioEncoderConfiguration = 12;
        private const int RemovePTZConfiguration = 14;
        private const int AddPTZConfiguration = 15;
        private const int GetProfile = 16;
        private const int DeleteProfile = 17;
        private const int AddVideoEncoderConfiguration = 18;
        private const int RemoveVideoEncoderConfiguration = 19;
        private const int AddVideoSourceConfiguration = 20;
        private const int RemoveVideoSourceConfiguration = 21;
        private const int AddMetadataConfiguration = 22;
        private const int RemoveMetadataConfiguration = 23;
        private const int GetVideoSourceConfiguration = 24;
        private const int SetVideoSourceConfiguration = 25;
        private const int GetCompatibleVideoEncoderConfigurations = 26;
        private const int GetGuaranteedNumberOfVideoEncoderInstances = 27;
        private const int GetAudioSourceConfigurations = 28;
        private const int GetAudioSourceConfiguration = 29;
        private const int GetAudioSourceConfigurationOptions = 30;
        private const int GetAudioSources = 31;
        private const int AddAudioSourceConfiguration = 32;
        private const int RemoveAudioSourceConfiguration = 33;
        private const int SetAudioSourceConfiguration = 34;
        private const int GetAudioEncoderConfigurations = 35;
        private const int AddAudioEncoderConfiguration = 36;
        private const int RemoveAudioEncoderConfiguration = 37;
        private const int GetMetadataConfigurations = 38;
        private const int GetAudioEncoderConfiguration = 39;
        private const int GetMetadataConfiguration = 40;
        private const int GetCompatibleAudioEncoderConfigurations = 41;
        private const int GetCompatibleAudioSourceConfigurations = 42;
        private const int AddVideoAnalyticsConfiguration = 43;
        private const int GetServiceCapabilitiesCount = 44;
        private const int GetAudioOutputConfigurations = 45;
        private const int GetAudioOutputConfiguration = 46;
        private const int SetAudioOutputConfiguration = 47;
        private const int GetAudioOutputConfigurationOptions = 48;
        private const int GetAudioDecoderConfigurationOptions = 49;
        private const int GetAudioOutputs = 50;
        private const int GetCompatibleMetadataConfigurations = 51;
        private const int SetMetadataConfiguration = 52;
        private const int GetMetadataConfigurationOptions = 53;
        private const int GetStreamUri = 54;
        private const int StartMulticastStreaming = 55;
        private const int SetSynchronizationPoint = 56;
        private const int StopMulticastStreaming = 57;
        private const int AddAudioOutputConfiguration = 58;
        private const int RemoveAudioOutputConfiguration = 59;
        private const int RemoveAudioDecoderConfiguration = 60;
        private const int AddAudioDecoderConfiguration = 61;
        private const int GetAudioDecoderConfiguration = 62;
        private const int GetCompatibleAudioOutputConfigurations = 63;
        private const int GetCompatibleAudioDecoderConfigurations = 64;
        private const int GetAudioDecoderConfigurations = 65;
        private const int SetAudioDecoderConfiguration = 66;
        private const int RemoveVideoAnalyticsConfiguration = 67;
        private const int GetOSDs = 68;
        private const int GetOSD = 69;
        private const int GetOSDOptions = 70;
        private const int SetOSD = 71;
        private const int CreateOSD = 72;
        private const int DeleteOSD = 73;
        private const int MaxCommands = 74;

        #endregion //Const

        #region Members

        /// <summary>
        /// Mass with command call count
        /// </summary>
        //private int[] CommandCount = new int[MaxCommands];

        private Dictionary<string, object> m_PSU = new Dictionary<string, object>();

        protected override string ServiceName
        {
            get { return "Media10"; }
        }

        #endregion //Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public MediaServiceTest(TestCommon testCommon)
            : base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors

        //***************************************************************************************

        #region Other

        internal StepType GetServiceCapabilitiesTest(out Capabilities target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetServiceCapabilities");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetServiceCapabilitiesCount]];

                #region Serialization Temp
                //Media.Profile[] dsr = new Media.Profile[1];
                //dsr[0] = new Media.Profile();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].PTZConfiguration = new Media.PTZConfiguration();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.Profile[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //GetServiceCapabilities1
                //WrongSchema

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Capabilities));
                target = (Capabilities)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetServiceCapabilitiesCount);
            }
            else
            {
                throw new SoapException("NO Media10.GetServiceCapabilities COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        #endregion //Other

        //***************************************************************************************

        #region Profiles

        internal StepType GetProfilesTest(out Profile[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetProfiles");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetProfiles");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetProfiles]];

                #region Serialization Temp
                //Media.Profile[] dsr = new Media.Profile[1];
                //dsr[0] = new Media.Profile();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].PTZConfiguration = new Media.PTZConfiguration();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.Profile[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Profile[]));
                target = (Profile[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetProfiles);
            }
            else
            {
                throw new SoapException("NO Media10.GetProfiles COMMAND IN SCRIPT", SoapException.ServerFaultCode); 
            }
            return res;
        }

        internal StepType GetProfileTest(out Profile target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetProfile");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetProfile]];

                #region Serialization Temp
                //Media.Profile[] dsr = new Media.Profile[1];
                //dsr[0] = new Media.Profile();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].PTZConfiguration = new Media.PTZConfiguration();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.Profile[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Profile));
                target = (Profile)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetProfile);
            }
            else
            {
                throw new SoapException("NO Media10.GetProfile COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType DeleteProfileTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.DeleteProfile");

            //TEMP: for backward compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("DeleteProfile");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[DeleteProfile]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, DeleteProfile);
            }
            else
            {
                throw new SoapException("NO Media10.DeleteProfile COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal Profile CreateProfileTest(Common.ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special = 0;

            var result = GetCommand<Profile>("CreateProfile", CreateProfile, validationRequest, true, out stepType, out exc, out timeout, out special);

            if (stepType == StepType.Normal)
            {
                switch (special)
                {
                    case 0:
                        result.Name = (string)validationRequest.ValidationRules.Find(rule => rule.ParameterName == "Name").Value;
                        break;
                    case 1:
                        //result from the script
                        break;
                }
            }
            return result;
        }

        //internal StepType CreateProfileTest(out Profile target, out SoapException ex, out int Timeout, string Name, string Token)
        //{
        //    StepType res = StepType.None;
        //    Timeout = 0;
        //    ex = null;
        //    bool passed = true;
        //    string logMessage = "";

        //    //Get step list for command
        //    XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.CreateProfile");

        //    //TEMP: for backword compatibility
        //    if (m_testList.Count == 0)
        //    {
        //        m_testList = TestCommon.GetStepsForCommand("CreateProfile");
        //    }

        //    if (m_testList.Count != 0)
        //    {
        //        //Get current step
        //        XmlNode test = m_testList[CommandCount[CreateProfile]];

        //        #region Serialization Temp
        //        //Media.Profile[] dsr = new Media.Profile[1];
        //        //dsr[0] = new Media.Profile();
        //        //dsr[0].token = "test";
        //        //dsr[0].Name = "name";
        //        //dsr[0].PTZConfiguration = new Media.PTZConfiguration();
        //        //XmlSerializer serializer = new XmlSerializer(typeof(Media.Profile[]));
        //        //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
        //        //TextWriter textWriter = new StreamWriter("c:\\2.txt");
        //        //serializer.Serialize(textWriter, dsr);
        //        #endregion //Serialization Temp

        //        #region Analyze request

        //        //Name
        //        CommonCompare.StringCompare("RequestParameters/Name", "Name", Name, ref logMessage, ref passed, test);

        //        //Token
        //        CommonCompare.StringCompare("RequestParameters/Token", "Token", Token, ref logMessage, ref passed, test);


        //        #endregion //Analyze request

        //        //Generate response
        //        object targetObj;
        //        res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Profile));
        //        target = (Profile)targetObj;

        //        //Log message
        //        TestCommon.writeToLog(test, logMessage, passed);

        //        Increment(m_testList.Count, CreateProfile);
        //    }
        //    else
        //    {
        //        throw new SoapException("NO Media10.CreateProfile COMMAND IN SCRIPT", SoapException.ServerFaultCode); 
        //    }
        //    return res;
        //}

        #endregion //Profiles

        //***************************************************************************************

        #region AudioEncoder

        internal StepType GetAudioEncoderConfigurationOptionsTest(out AudioEncoderConfigurationOptions target, out SoapException ex, out int Timeout, string ConfigurationToken, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetAudioEncoderConfigurationOptions");

            //TEMP
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetAudioEncoderConfigurationOptions");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAudioEncoderConfigurationOptions]];

                #region Serialization Temp
                //Media.VideoEncoderConfigurationOptions dsr = new Media.VideoEncoderConfigurationOptions();
                //dsr.JPEG = new Media.JpegOptions();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.VideoEncoderConfigurationOptions));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioEncoderConfigurationOptions));
                target = (AudioEncoderConfigurationOptions)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAudioEncoderConfigurationOptions);
            }
            else
            {
                throw new SoapException("NO Media10.GetAudioEncoderConfigurationOptions COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType SetAudioEncoderConfigurationTest(out SoapException ex, out int Timeout, AudioEncoderConfiguration Configuration, bool ForcePersistence)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.SetAudioEncoderConfiguration");

            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("SetAudioEncoderConfiguration");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetAudioEncoderConfiguration]];

                #region Analyze request

                //Configuration
                if (CommonCompare.Exist("RequestParameters/Configuration", "Configuration", Configuration, ref logMessage, ref passed, test))
                {
                    //Name
                    CommonCompare.StringCompare("RequestParameters/Configuration/Name", "Name", Configuration.Name, ref logMessage, ref passed, test);

                    //UseCount
                    CommonCompare.IntCompare("RequestParameters/Configuration/UseCount", "UseCount", Configuration.UseCount, ref logMessage, ref passed, test);

                    //token
                    CommonCompare.StringCompare("RequestParameters/Configuration/@token", "token", Configuration.token, ref logMessage, ref passed, test);

                    //Encoding
                    CommonCompare.StringCompare("RequestParameters/Configuration/Encoding", "Encoding", Configuration.Encoding.ToString(), ref logMessage, ref passed, test);

                    //Bitrate
                    CommonCompare.FloatCompare("RequestParameters/Configuration/Bitrate", "Bitrate", Configuration.Bitrate, ref logMessage, ref passed, test);

                    //SampleRate
                    CommonCompare.FloatCompare("RequestParameters/Configuration/SampleRate", "SampleRate", Configuration.SampleRate, ref logMessage, ref passed, test);

                    //Multicast
                    if (CommonCompare.Exist("RequestParameters/Configuration/Multicast", "Multicast", Configuration.Multicast, ref logMessage, ref passed, test))
                    {
                        //Address
                        if (CommonCompare.Exist("RequestParameters/Configuration/Multicast/Address", "Address", Configuration.Multicast.Address, ref logMessage, ref passed, test))
                        {
                            //Type
                            CommonCompare.StringCompare("RequestParameters/Configuration/Multicast/Address/Type", "Type", Configuration.Multicast.Address.Type.ToString(), ref logMessage, ref passed, test);

                            //IPv4Address
                            CommonCompare.StringCompare("RequestParameters/Configuration/Multicast/Address/IPv4Address", "IPv4Address", Configuration.Multicast.Address.IPv4Address, ref logMessage, ref passed, test);

                            //IPv6Address
                            CommonCompare.StringCompare("RequestParameters/Configuration/Multicast/Address/IPv6Address", "IPv6Address", Configuration.Multicast.Address.IPv6Address, ref logMessage, ref passed, test);
                        }

                        //Port
                        CommonCompare.IntCompare("RequestParameters/Configuration/Multicast/Port", "Port", Configuration.Multicast.Port, ref logMessage, ref passed, test);

                        //TTL
                        CommonCompare.IntCompare("RequestParameters/Configuration/Multicast/TTL", "TTL", Configuration.Multicast.TTL, ref logMessage, ref passed, test);

                        //AutoStart
                        CommonCompare.StringCompare("RequestParameters/Configuration/Multicast/AutoStart", "AutoStart", Configuration.Multicast.AutoStart.ToString(), ref logMessage, ref passed, test);
                    }

                    //SessionTimeout
                    CommonCompare.StringCompare("RequestParameters/Configuration/SessionTimeout", "SessionTimeout", Configuration.SessionTimeout.ToString(), ref logMessage, ref passed, test);

                }
                //ForcePersistence
                CommonCompare.StringCompare("RequestParameters/ForcePersistence", "ForcePersistence", ForcePersistence.ToString(), ref logMessage, ref passed, test);
                

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Remember request
                if (m_PSU.ContainsKey("AEC.Multicast.Address.IPv4Address"))
                {
                    m_PSU["AEC.Multicast.Address.IPv4Address"] = Configuration.Multicast.Address.IPv4Address;
                }
                else
                {
                    m_PSU.Add("AEC.Multicast.Address.IPv4Address", Configuration.Multicast.Address.IPv4Address);
                }

                if (m_PSU.ContainsKey("AEC.Multicast.Port"))
                {
                    m_PSU["AEC.Multicast.Port"] = Configuration.Multicast.Port;
                }
                else
                {
                    m_PSU.Add("AEC.Multicast.Port", Configuration.Multicast.Port);
                }

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetAudioEncoderConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.SetAudioEncoderConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetAudioEncoderConfigurationsTest(out AudioEncoderConfiguration[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetAudioEncoderConfigurations");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAudioEncoderConfigurations]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioEncoderConfiguration[]));
                target = (AudioEncoderConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAudioEncoderConfigurations);
            }
            else
            {
                throw new SoapException("NO Media10.GetAudioEncoderConfigurations COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType AddAudioEncoderConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.AddAudioEncoderConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[AddAudioEncoderConfiguration]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, AddAudioEncoderConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.AddAudioEncoderConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;

        }

        internal StepType RemoveAudioEncoderConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.RemoveAudioEncoderConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[RemoveAudioEncoderConfiguration]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, RemoveAudioEncoderConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.RemoveAudioEncoderConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetAudioEncoderConfigurationTest(out AudioEncoderConfiguration target, out SoapException ex, out int Timeout, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetAudioEncoderConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAudioEncoderConfiguration]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioEncoderConfiguration));
                target = (AudioEncoderConfiguration)targetObj;

                //Remember request
                XmlNode temp;

                temp = test.SelectSingleNode(".//AEC.Multicast.Address.IPv4Address");
                if ((temp != null) && (temp.InnerText == "1"))
                {
                    if (m_PSU.ContainsKey("AEC.Multicast.Address.IPv4Address"))
                    {
                        target.Multicast.Address.IPv4Address = (string)(m_PSU["AEC.Multicast.Address.IPv4Address"]);
                    }
                }

                temp = test.SelectSingleNode(".//AEC.Multicast.Port");
                if ((temp != null) && (temp.InnerText == "1"))
                {
                    if (m_PSU.ContainsKey("AEC.Multicast.Port"))
                    {
                        target.Multicast.Port = (int)(m_PSU["AEC.Multicast.Port"]);
                    }
                }

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAudioEncoderConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.GetAudioEncoderConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;

        }

        internal StepType GetCompatibleAudioEncoderConfigurationsTest(out AudioEncoderConfiguration[] target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetCompatibleAudioEncoderConfigurations");

            //TEMP: for backward compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetCompatibleAudioEncoderConfigurations");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetCompatibleAudioEncoderConfigurations]];

                #region Serialization Temp
                //Media.Profile[] dsr = new Media.Profile[1];
                //dsr[0] = new Media.Profile();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].PTZConfiguration = new Media.PTZConfiguration();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.Profile[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioEncoderConfiguration[]));
                target = (AudioEncoderConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetCompatibleAudioEncoderConfigurations);
            }
            else
            {
                throw new SoapException("NO Media10.GetCompatibleAudioEncoderConfigurations COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }


        #endregion //AudioEncoder

        //***************************************************************************************

        #region AudioSource

        internal StepType GetAudioSourceConfigurationsTest(out AudioSourceConfiguration[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetAudioSourceConfigurations");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAudioSourceConfigurations]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioSourceConfiguration[]));
                target = (AudioSourceConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAudioSourceConfigurations);
            }
            else
            {
                throw new SoapException("NO Media10.GetAudioSourceConfigurations COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetAudioSourceConfigurationTest(out AudioSourceConfiguration target, out SoapException ex, out int Timeout, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetAudioSourceConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAudioSourceConfiguration]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioSourceConfiguration));
                target = (AudioSourceConfiguration)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAudioSourceConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.GetAudioSourceConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetAudioSourceConfigurationOptionsTest(out AudioSourceConfigurationOptions target, out SoapException ex, out int Timeout, string ConfigurationToken, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetAudioSourceConfigurationOptions");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAudioSourceConfigurationOptions]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioSourceConfigurationOptions));
                target = (AudioSourceConfigurationOptions)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAudioSourceConfigurationOptions);
            }
            else
            {
                throw new SoapException("NO Media10.GetAudioSourceConfigurationOptions COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetAudioSourcesTest(out AudioSource[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetAudioSources");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAudioSources]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioSource[]));
                target = (AudioSource[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAudioSources);
            }
            else
            {
                throw new SoapException("NO Media10.GetAudioSources COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType AddAudioSourceConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.AddAudioSourceConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[AddAudioSourceConfiguration]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, AddAudioSourceConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.AddAudioSourceConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType RemoveAudioSourceConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.RemoveAudioSourceConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[RemoveAudioSourceConfiguration]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, RemoveAudioSourceConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.RemoveAudioSourceConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType SetAudioSourceConfigurationTest(out SoapException ex, out int Timeout, AudioSourceConfiguration Configuration, bool ForcePersistence)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.SetAudioSourceConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetAudioSourceConfiguration]];

                #region Analyze request

                //Configuration
                if (CommonCompare.Exist("RequestParameters/Configuration", "Configuration", Configuration, ref logMessage, ref passed, test))
                {
                    //Name
                    CommonCompare.StringCompare("RequestParameters/Configuration/Name", "Name", Configuration.Name, ref logMessage, ref passed, test);

                    //UseCount
                    CommonCompare.IntCompare("RequestParameters/Configuration/UseCount", "UseCount", Configuration.UseCount, ref logMessage, ref passed, test);

                    //SourceToken
                    CommonCompare.StringCompare("RequestParameters/Configuration/SourceToken", "SourceToken", Configuration.SourceToken, ref logMessage, ref passed, test);

                    //token
                    CommonCompare.StringCompare("RequestParameters/Configuration/token", "token", Configuration.token, ref logMessage, ref passed, test);

                }

                //ForcePersistence
                CommonCompare.StringCompare("RequestParameters/ForcePersistence", "ForcePersistence", ForcePersistence.ToString(), ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetAudioSourceConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.SetAudioSourceConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetCompatibleAudioSourceConfigurationsTest(out AudioSourceConfiguration[] target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetCompatibleAudioSourceConfigurations");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetCompatibleAudioSourceConfigurations]];

                #region Serialization Temp
                //Media.Profile[] dsr = new Media.Profile[1];
                //dsr[0] = new Media.Profile();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].PTZConfiguration = new Media.PTZConfiguration();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.Profile[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioSourceConfiguration[]));
                target = (AudioSourceConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetCompatibleAudioSourceConfigurations);
            }
            else
            {
                throw new SoapException("NO Media10.GetCompatibleAudioSourceConfigurations COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }



        #endregion //AudioSource

        //***************************************************************************************

        #region VideoEncoder

        internal StepType GetVideoEncoderConfigurationsTest(out VideoEncoderConfiguration[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetVideoEncoderConfigurations");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetVideoEncoderConfigurations");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetVideoEncoderConfigurations]];

                #region Serialization Temp
                //Media.VideoEncoderConfiguration[] dsr = new Media.VideoEncoderConfiguration[1];
                //dsr[0] = new Media.VideoEncoderConfiguration();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].Encoding = Media.VideoEncoding.H264;
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.VideoEncoderConfiguration[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(VideoEncoderConfiguration[]));
                target = (VideoEncoderConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetVideoEncoderConfigurations);
            }
            else
            {
                throw new SoapException("NO Media10.GetVideoEncoderConfigurations COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetVideoEncoderConfigurationOptionsTest(out VideoEncoderConfigurationOptions target, out SoapException ex, out int Timeout, string ConfigurationToken, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetVideoEncoderConfigurationOptions");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetVideoEncoderConfigurationOptions");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetVideoEncoderConfigurationOptions]];

                #region Serialization Temp
                //Media.VideoEncoderConfigurationOptions dsr = new Media.VideoEncoderConfigurationOptions();
                //dsr.JPEG = new Media.JpegOptions();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.VideoEncoderConfigurationOptions));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(VideoEncoderConfigurationOptions));
                target = (VideoEncoderConfigurationOptions)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetVideoEncoderConfigurationOptions);
            }
            else
            {
                throw new SoapException("NO Media10.GetVideoEncoderConfigurationOptions COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType SetVideoEncoderConfigurationTest(out SoapException ex, out int Timeout, VideoEncoderConfiguration Configuration, bool ForcePersistence)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.SetVideoEncoderConfiguration");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("SetVideoEncoderConfiguration");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetVideoEncoderConfiguration]];

                #region Analyze request

                //Configuration
                if (CommonCompare.Exist("RequestParameters/Configuration", "Configuration", Configuration, ref logMessage, ref passed, test))
                {
                    //Name
                    CommonCompare.StringCompare("RequestParameters/Configuration/Name", "Name", Configuration.Name, ref logMessage, ref passed, test);

                    //UseCount
                    CommonCompare.IntCompare("RequestParameters/Configuration/UseCount", "UseCount", Configuration.UseCount, ref logMessage, ref passed, test);

                    //token
                    CommonCompare.StringCompare("RequestParameters/Configuration/@token", "token", Configuration.token, ref logMessage, ref passed, test);

                    //Encoding
                    CommonCompare.StringCompare("RequestParameters/Configuration/Encoding", "Encoding", Configuration.Encoding.ToString(), ref logMessage, ref passed, test);

                    //Resolution
                    if (CommonCompare.Exist("RequestParameters/Configuration/Resolution", "Resolution", Configuration.Resolution, ref logMessage, ref passed, test))
                    {
                        //Width
                        CommonCompare.IntCompare("RequestParameters/Configuration/Resolution/Width", "Width", Configuration.Resolution.Width, ref logMessage, ref passed, test);

                        //Height
                        CommonCompare.IntCompare("RequestParameters/Configuration/Resolution/Height", "Height", Configuration.Resolution.Height, ref logMessage, ref passed, test);
                    }

                    //Quality
                    CommonCompare.FloatCompare("RequestParameters/Configuration/Quality", "Quality", Configuration.Quality, ref logMessage, ref passed, test);

                    //RateControl
                    if (CommonCompare.Exist("RequestParameters/Configuration/RateControl", "RateControl", Configuration.RateControl, ref logMessage, ref passed, test))
                    {
                        //FrameRateLimit
                        CommonCompare.IntCompare("RequestParameters/Configuration/RateControl/FrameRateLimit", "FrameRateLimit", Configuration.RateControl.FrameRateLimit, ref logMessage, ref passed, test);

                        //EncodingInterval
                        CommonCompare.IntCompare("RequestParameters/Configuration/RateControl/EncodingInterval", "EncodingInterval", Configuration.RateControl.EncodingInterval, ref logMessage, ref passed, test);

                        //BitrateLimit
                        CommonCompare.IntCompare("RequestParameters/Configuration/RateControl/BitrateLimit", "BitrateLimit", Configuration.RateControl.BitrateLimit, ref logMessage, ref passed, test);
                    };

                    //MPEG4
                    if (CommonCompare.Exist("RequestParameters/Configuration/MPEG4", "MPEG4", Configuration.MPEG4, ref logMessage, ref passed, test))
                    {
                        //GovLength
                        CommonCompare.IntCompare("RequestParameters/Configuration/MPEG4/GovLength", "GovLength", Configuration.MPEG4.GovLength, ref logMessage, ref passed, test);

                        //Mpeg4Profile
                        CommonCompare.StringCompare("RequestParameters/Configuration/MPEG4/Mpeg4Profile", "Mpeg4Profile", Configuration.MPEG4.Mpeg4Profile.ToString(), ref logMessage, ref passed, test);
                    };

                    //H264
                    if (CommonCompare.Exist("RequestParameters/Configuration/H264", "H264", Configuration.H264, ref logMessage, ref passed, test))
                    {
                        //GovLength
                        CommonCompare.IntCompare("RequestParameters/Configuration/H264/GovLength", "GovLength", Configuration.H264.GovLength, ref logMessage, ref passed, test);

                        //H264Profile
                        CommonCompare.StringCompare("RequestParameters/Configuration/H264/H264Profile", "H264Profile", Configuration.H264.H264Profile.ToString(), ref logMessage, ref passed, test);
                    };

                    //Multicast
                    if (CommonCompare.Exist("RequestParameters/Configuration/Multicast", "Multicast", Configuration.Multicast, ref logMessage, ref passed, test))
                    {
                        //Address
                        if (CommonCompare.Exist("RequestParameters/Configuration/Multicast/Address", "Address", Configuration.Multicast.Address, ref logMessage, ref passed, test))
                        {
                            //Type
                            CommonCompare.StringCompare("RequestParameters/Configuration/Multicast/Address/Type", "Type", Configuration.Multicast.Address.Type.ToString(), ref logMessage, ref passed, test);

                            //IPv4Address
                            CommonCompare.StringCompare("RequestParameters/Configuration/Multicast/Address/IPv4Address", "IPv4Address", Configuration.Multicast.Address.IPv4Address, ref logMessage, ref passed, test);

                            //IPv6Address
                            CommonCompare.StringCompare("RequestParameters/Configuration/Multicast/Address/IPv6Address", "IPv6Address", Configuration.Multicast.Address.IPv6Address, ref logMessage, ref passed, test);
                        }

                        //Port
                        CommonCompare.IntCompare("RequestParameters/Configuration/Multicast/Port", "Port", Configuration.Multicast.Port, ref logMessage, ref passed, test);

                        //TTL
                        CommonCompare.IntCompare("RequestParameters/Configuration/Multicast/TTL", "TTL", Configuration.Multicast.TTL, ref logMessage, ref passed, test);

                        //AutoStart
                        CommonCompare.StringCompare("RequestParameters/Configuration/Multicast/AutoStart", "AutoStart", Configuration.Multicast.AutoStart.ToString(), ref logMessage, ref passed, test);
                    }

                    //SessionTimeout
                    CommonCompare.StringCompare("RequestParameters/Configuration/SessionTimeout", "SessionTimeout", Configuration.SessionTimeout.ToString(), ref logMessage, ref passed, test);

                }
                //ForcePersistence
                CommonCompare.StringCompare("RequestParameters/ForcePersistence", "ForcePersistence", ForcePersistence.ToString(), ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Remember request
                if (m_PSU.ContainsKey("VEC.Multicast.Address.IPv4Address"))
                {
                    m_PSU["VEC.Multicast.Address.IPv4Address"] = Configuration.Multicast.Address.IPv4Address;
                }
                else
                {
                    m_PSU.Add("VEC.Multicast.Address.IPv4Address", Configuration.Multicast.Address.IPv4Address);
                }

                if (m_PSU.ContainsKey("VEC.Multicast.Port"))
                {
                    m_PSU["VEC.Multicast.Port"] = Configuration.Multicast.Port;
                }
                else
                {
                    m_PSU.Add("VEC.Multicast.Port", Configuration.Multicast.Port);
                }
                
                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetVideoEncoderConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.SetVideoEncoderConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetVideoEncoderConfigurationTest(out VideoEncoderConfiguration target, out SoapException ex, out int Timeout, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetVideoEncoderConfiguration");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetVideoEncoderConfiguration");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetVideoEncoderConfiguration]];

                #region Serialization Temp
                //Media.VideoEncoderConfiguration[] dsr = new Media.VideoEncoderConfiguration[1];
                //dsr[0] = new Media.VideoEncoderConfiguration();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].Encoding = Media.VideoEncoding.H264;
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.VideoEncoderConfiguration[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(VideoEncoderConfiguration));
                target = (VideoEncoderConfiguration)targetObj;

                
                //Remember request
                XmlNode temp;

                temp = test.SelectSingleNode(".//VEC.Multicast.Address.IPv4Address");
                if ((temp != null)&&(temp.InnerText == "1"))
                {
                    if (m_PSU.ContainsKey("VEC.Multicast.Address.IPv4Address"))
                    {
                        target.Multicast.Address.IPv4Address = (string)(m_PSU["VEC.Multicast.Address.IPv4Address"]);
                    }
                }

                temp = test.SelectSingleNode(".//VEC.Multicast.Port");
                if ((temp != null) && (temp.InnerText == "1"))
                {
                    if (m_PSU.ContainsKey("VEC.Multicast.Port"))
                    {
                        target.Multicast.Port = (int)(m_PSU["VEC.Multicast.Port"]);
                    }
                }

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetVideoEncoderConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.GetVideoEncoderConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType AddVideoEncoderConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.AddVideoEncoderConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[AddVideoEncoderConfiguration]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, AddVideoEncoderConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.AddVideoEncoderConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType RemoveVideoEncoderConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.RemoveVideoEncoderConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[RemoveVideoEncoderConfiguration]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, RemoveVideoEncoderConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.RemoveVideoEncoderConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetCompatibleVideoEncoderConfigurationsTest(out VideoEncoderConfiguration[] target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetCompatibleVideoEncoderConfigurations");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetCompatibleVideoEncoderConfigurations]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(VideoEncoderConfiguration[]));
                target = (VideoEncoderConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetCompatibleVideoEncoderConfigurations);
            }
            else
            {
                throw new SoapException("NO Media10.GetCompatibleVideoEncoderConfigurations COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetGuaranteedNumberOfVideoEncoderInstancesTest(out int target, out SoapException ex, out int Timeout, string ConfigurationToken, out int JPEG, out bool JPEGSpecified, out int H264, out bool H264Specified, out int MPEG4, out bool MPEG4Specified)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetGuaranteedNumberOfVideoEncoderInstances");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetGuaranteedNumberOfVideoEncoderInstances]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //TotalNumber
                target = Convert.ToInt32(test.SelectSingleNode("ResponseParameters/TotalNumber").InnerText);

                //JPEG
                if (test.SelectSingleNode("ResponseParameters/JPEG") != null)
                {
                    JPEGSpecified = true;
                    JPEG = Convert.ToInt32(test.SelectSingleNode("ResponseParameters/JPEG").InnerText);
                }
                else
                {
                    JPEGSpecified = false;
                    JPEG = 0;
                }

                //H264
                if (test.SelectSingleNode("ResponseParameters/H264") != null)
                {
                    H264Specified = true;
                    H264 = Convert.ToInt32(test.SelectSingleNode("ResponseParameters/H264").InnerText);
                }
                else
                {
                    H264Specified = false;
                    H264 = 0;
                }

                //MPEG4
                if (test.SelectSingleNode("ResponseParameters/MPEG4") != null)
                {
                    MPEG4Specified = true;
                    MPEG4 = Convert.ToInt32(test.SelectSingleNode("ResponseParameters/MPEG4").InnerText);
                }
                else
                {
                    MPEG4Specified = false;
                    MPEG4 = 0;
                }

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetGuaranteedNumberOfVideoEncoderInstances);
            }
            else
            {
                throw new SoapException("NO Media10.GetGuaranteedNumberOfVideoEncoderInstances COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        
        #endregion //VideoEncoder

        //***************************************************************************************

        #region VideoSource

        internal StepType GetVideoSourcesTest(out VideoSource[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList;
            m_testList = TestCommon.GetStepsForCommand("Media10.GetVideoSources");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetVideoSources");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetVideoSources]];

                #region Serialization Temp
                //Media.Profile[] dsr = new Media.Profile[1];
                //dsr[0] = new Media.Profile();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].PTZConfiguration = new Media.PTZConfiguration();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.Profile[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(VideoSource[]));
                target = (VideoSource[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetVideoSources);
            }
            else
            {
                throw new SoapException("NO Media10.GetVideoSources COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetCompatibleVideoSourceConfigurationsTest(out VideoSourceConfiguration[] target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetCompatibleVideoSourceConfigurations");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetCompatibleVideoSourceConfigurations");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetCompatibleVideoSourceConfigurations]];

                #region Serialization Temp
                //Media.Profile[] dsr = new Media.Profile[1];
                //dsr[0] = new Media.Profile();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].PTZConfiguration = new Media.PTZConfiguration();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.Profile[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(VideoSourceConfiguration[]));
                target = (VideoSourceConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetCompatibleVideoSourceConfigurations);
            }
            else
            {
                throw new SoapException("NO Media10.GetCompatibleVideoSourceConfigurations COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetVideoSourceConfigurationsTest(out VideoSourceConfiguration[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetVideoSourceConfigurations");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetVideoSourceConfigurations");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetVideoSourceConfigurations]];

                #region Serialization Temp
                //Media.VideoEncoderConfiguration[] dsr = new Media.VideoEncoderConfiguration[1];
                //dsr[0] = new Media.VideoEncoderConfiguration();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].Encoding = Media.VideoEncoding.H264;
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.VideoEncoderConfiguration[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(VideoSourceConfiguration[]));
                target = (VideoSourceConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetVideoSourceConfigurations);
            }
            else
            {
                throw new SoapException("NO Media10.GetVideoSourceConfigurations COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetVideoSourceConfigurationOptionsTest(out VideoSourceConfigurationOptions target, out SoapException ex, out int Timeout, string ConfigurationToken, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetVideoSourceConfigurationOptions");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetVideoSourceConfigurationOptions");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetVideoSourceConfigurationOptions]];

                #region Serialization Temp
                //Media.VideoEncoderConfiguration[] dsr = new Media.VideoEncoderConfiguration[1];
                //dsr[0] = new Media.VideoEncoderConfiguration();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].Encoding = Media.VideoEncoding.H264;
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.VideoEncoderConfiguration[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(VideoSourceConfigurationOptions));
                target = (VideoSourceConfigurationOptions)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetVideoSourceConfigurationOptions);
            }
            else
            {
                throw new SoapException("NO Media10.GetVideoSourceConfigurationOptions COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType AddVideoSourceConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.AddVideoSourceConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[AddVideoSourceConfiguration]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, AddVideoSourceConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.AddVideoSourceConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType RemoveVideoSourceConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.RemoveVideoSourceConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[RemoveVideoSourceConfiguration]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, RemoveVideoSourceConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.RemoveVideoSourceConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetVideoSourceConfigurationTest(out VideoSourceConfiguration target, out SoapException ex, out int Timeout, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetVideoSourceConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetVideoSourceConfiguration]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(VideoSourceConfiguration));
                target = (VideoSourceConfiguration)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetVideoSourceConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.GetVideoSourceConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType SetVideoSourceConfigurationTest(out SoapException ex, out int Timeout, VideoSourceConfiguration Configuration, bool ForcePersistence)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.SetVideoSourceConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetVideoSourceConfiguration]];

                #region Analyze request

                //Configuration
                if (CommonCompare.Exist("RequestParameters/Configuration", "Configuration", Configuration, ref logMessage, ref passed, test))
                {
                    //Name
                    CommonCompare.StringCompare("RequestParameters/Configuration/Name", "Name", Configuration.Name, ref logMessage, ref passed, test);

                    //UseCount
                    CommonCompare.IntCompare("RequestParameters/Configuration/UseCount", "UseCount", Configuration.UseCount, ref logMessage, ref passed, test);

                    //SourceToken
                    CommonCompare.StringCompare("RequestParameters/Configuration/SourceToken", "SourceToken", Configuration.SourceToken, ref logMessage, ref passed, test);

                    //token
                    CommonCompare.StringCompare("RequestParameters/Configuration/@token", "token", Configuration.token, ref logMessage, ref passed, test);

                    //Bounds
                    if (CommonCompare.Exist("RequestParameters/Configuration/Bounds", "Bounds", Configuration.Bounds, ref logMessage, ref passed, test))
                    {
                        //x
                        CommonCompare.IntCompare("RequestParameters/Configuration/Bounds/@x", "x", Configuration.Bounds.x, ref logMessage, ref passed, test);

                        //y
                        CommonCompare.IntCompare("RequestParameters/Configuration/Bounds/@y", "y", Configuration.Bounds.y, ref logMessage, ref passed, test);

                        //width
                        CommonCompare.IntCompare("RequestParameters/Configuration/Bounds/@width", "width", Configuration.Bounds.width, ref logMessage, ref passed, test);

                        //height
                        CommonCompare.IntCompare("RequestParameters/Configuration/Bounds/@height", "height", Configuration.Bounds.height, ref logMessage, ref passed, test);
                    };


                }

                //ForcePersistence
                CommonCompare.StringCompare("RequestParameters/ForcePersistence", "ForcePersistence", ForcePersistence.ToString(), ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetVideoSourceConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.SetVideoSourceConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }


        #endregion //VideoSource

        //***************************************************************************************

        #region Metadata

        internal StepType AddMetadataConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.AddMetadataConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[AddMetadataConfiguration]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, AddMetadataConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.AddMetadataConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType RemoveMetadataConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.RemoveMetadataConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[RemoveMetadataConfiguration]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, RemoveMetadataConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.RemoveMetadataConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetMetadataConfigurationsTest(out MetadataConfiguration[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetMetadataConfigurations");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetMetadataConfigurations]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(MetadataConfiguration[]));
                target = (MetadataConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetMetadataConfigurations);
            }
            else
            {
                throw new SoapException("NO Media10.GetMetadataConfigurations COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetMetadataConfigurationTest(out MetadataConfiguration target, out SoapException ex, out int Timeout, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetMetadataConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetMetadataConfiguration]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(MetadataConfiguration));
                target = (MetadataConfiguration)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetMetadataConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.GetMetadataConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        #endregion //Metadata

        //***************************************************************************************

        #region PTZ

        internal StepType RemovePTZConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.RemovePTZConfiguration");

            //TEMP
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("RemovePTZConfiguration");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[RemovePTZConfiguration]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, RemovePTZConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.RemovePTZConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType AddPTZConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.AddPTZConfiguration");

            //TEMP
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("AddPTZConfiguration");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[AddPTZConfiguration]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, AddPTZConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.AddPTZConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        #endregion //PTZ

        //***************************************************************************************
        #region VideoAnalytics

        internal StepType RemoveVideoAnalyticsConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.RemoveVideoAnalyticsConfiguration");

            //TEMP
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("RemoveVideoAnalyticsConfiguration");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[RemoveVideoAnalyticsConfiguration]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, RemoveVideoAnalyticsConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.RemoveVideoAnalyticsConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType AddVideoAnalyticsConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.AddVideoAnalyticsConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[AddVideoAnalyticsConfiguration]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, AddVideoAnalyticsConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.AddVideoAnalyticsConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        #endregion //VideoAnalytics

        //***************************************************************************************

        internal StepType GetSnapshotUriTest(out MediaUri target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetSnapshotUri");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetSnapshotUri");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetSnapshotUri]];

                #region Serialization Temp
                //Media.GetSnapshotUriResponse dsr = new Media.GetSnapshotUriResponse();
                //dsr.MediaUri = new Media.MediaUri();
                //dsr.MediaUri.Timeout = "P60T";
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.GetSnapshotUriResponse));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(MediaUri));
                target = (MediaUri)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetSnapshotUri);
            }
            else
            {
                throw new SoapException("NO Media10.GetSnapshotUri COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        
        internal StepType GetAudioOutputConfigurationsTest(out AudioOutputConfiguration[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetAudioOutputConfigurations");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAudioOutputConfigurations]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioOutputConfiguration[]));
                target = (AudioOutputConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAudioOutputConfigurations);
            }
            else
            {
                throw new SoapException("NO Media10.GetAudioOutputConfigurations COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetAudioOutputConfigurationTest(out AudioOutputConfiguration target, out SoapException ex, out int Timeout, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetAudioOutputConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAudioOutputConfiguration]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioOutputConfiguration));
                target = (AudioOutputConfiguration)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAudioOutputConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.GetAudioOutputConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType SetAudioOutputConfigurationTest(out SoapException ex, out int Timeout, AudioOutputConfiguration Configuration, bool ForcePersistence)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.SetAudioOutputConfiguration");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetAudioOutputConfiguration]];

                #region Analyze request

                //Configuration
                if (CommonCompare.Exist("RequestParameters/Configuration", "Configuration", Configuration, ref logMessage, ref passed, test))
                {
                    //Name
                    CommonCompare.StringCompare("RequestParameters/Configuration/Name", "Name", Configuration.Name, ref logMessage, ref passed, test);

                    //UseCount
                    CommonCompare.IntCompare("RequestParameters/Configuration/UseCount", "UseCount", Configuration.UseCount, ref logMessage, ref passed, test);

                    //SourceToken
                    CommonCompare.StringCompare("RequestParameters/Configuration/OutputToken", "OutputToken", Configuration.OutputToken, ref logMessage, ref passed, test);

                    //token
                    CommonCompare.StringCompare("RequestParameters/Configuration/token", "token", Configuration.token, ref logMessage, ref passed, test);
                    //SendPrimacy
                    CommonCompare.StringCompare("RequestParameters/Configuration/SendPrimacy", "SendPrimacy", Configuration.SendPrimacy, ref logMessage, ref passed, test);
                }

                //ForcePersistence
                CommonCompare.StringCompare("RequestParameters/ForcePersistence", "ForcePersistence", ForcePersistence.ToString(), ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetAudioOutputConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.SetAudioOutputConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetAudioOutputConfigurationOptionsTest(out AudioOutputConfigurationOptions target, out SoapException ex, out int Timeout, string ConfigurationToken, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetAudioOutputConfigurationOptions");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAudioOutputConfigurationOptions]];

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioOutputConfigurationOptions));
                target = (AudioOutputConfigurationOptions)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAudioOutputConfigurationOptions);
            }
            else
            {
                throw new SoapException("NO Media10.GetAudioOutputConfigurationOptions COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetAudioDecoderConfigurationOptionsTest(out AudioDecoderConfigurationOptions target, out SoapException ex, out int Timeout, string ConfigurationToken, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetAudioDecoderConfigurationOptions");

            //TEMP
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetAudioDecoderConfigurationOptions");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAudioDecoderConfigurationOptions]];

                #region Serialization Temp
                //Media.VideoEncoderConfigurationOptions dsr = new Media.VideoEncoderConfigurationOptions();
                //dsr.JPEG = new Media.JpegOptions();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.VideoEncoderConfigurationOptions));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioDecoderConfigurationOptions));
                target = (AudioDecoderConfigurationOptions)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAudioDecoderConfigurationOptions);
            }
            else
            {
                throw new SoapException("NO Media10.GetAudioDecoderConfigurationOptions COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetAudioOutputsTest(out AudioOutput[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetAudioOutputs");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAudioOutputs]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioOutput[]));
                target = (AudioOutput[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAudioOutputs);
            }
            else
            {
                throw new SoapException("NO Media10.GetAudioOutputs COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetCompatibleMetadataConfigurationsTest(out MetadataConfiguration[] target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetCompatibleMetadataConfigurations");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetCompatibleMetadataConfigurations]];

                #region Serialization Temp
                //Media.Profile[] dsr = new Media.Profile[1];
                //dsr[0] = new Media.Profile();
                //dsr[0].token = "test";
                //dsr[0].Name = "name";
                //dsr[0].PTZConfiguration = new Media.PTZConfiguration();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.Profile[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(MetadataConfiguration[]));
                target = (MetadataConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetCompatibleMetadataConfigurations);
            }
            else
            {
                throw new SoapException("NO Media10.GetCompatibleMetadataConfigurations COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType SetMetadataConfigurationTest(out SoapException ex, out int Timeout, MetadataConfiguration Configuration, bool ForcePersistence)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.SetMetadataConfiguration");

            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("SetMetadataConfiguration");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetMetadataConfiguration]];

                #region Analyze request

                //Configuration
                if (CommonCompare.Exist("RequestParameters/Configuration", "Configuration", Configuration, ref logMessage, ref passed, test))
                {
                    //Name
                    CommonCompare.StringCompare("RequestParameters/Configuration/Name", "Name", Configuration.Name, ref logMessage, ref passed, test);

                    //UseCount
                    CommonCompare.IntCompare("RequestParameters/Configuration/UseCount", "UseCount", Configuration.UseCount, ref logMessage, ref passed, test);

                    //token
                    CommonCompare.StringCompare("RequestParameters/Configuration/@token", "token", Configuration.token, ref logMessage, ref passed, test);

                    ////Encoding
                    //CommonCompare.StringCompare("RequestParameters/Configuration/Encoding", "Encoding", Configuration.Encoding.ToString(), ref logMessage, ref passed, test);

                    ////Bitrate
                    //CommonCompare.FloatCompare("RequestParameters/Configuration/Bitrate", "Bitrate", Configuration.Bitrate, ref logMessage, ref passed, test);

                    ////SampleRate
                    //CommonCompare.FloatCompare("RequestParameters/Configuration/SampleRate", "SampleRate", Configuration.SampleRate, ref logMessage, ref passed, test);

                    //Multicast
                    if (CommonCompare.Exist("RequestParameters/Configuration/Multicast", "Multicast", Configuration.Multicast, ref logMessage, ref passed, test))
                    {
                        //Address
                        if (CommonCompare.Exist("RequestParameters/Configuration/Multicast/Address", "Address", Configuration.Multicast.Address, ref logMessage, ref passed, test))
                        {
                            //Type
                            CommonCompare.StringCompare("RequestParameters/Configuration/Multicast/Address/Type", "Type", Configuration.Multicast.Address.Type.ToString(), ref logMessage, ref passed, test);

                            //IPv4Address
                            CommonCompare.StringCompare("RequestParameters/Configuration/Multicast/Address/IPv4Address", "IPv4Address", Configuration.Multicast.Address.IPv4Address, ref logMessage, ref passed, test);

                            //IPv6Address
                            CommonCompare.StringCompare("RequestParameters/Configuration/Multicast/Address/IPv6Address", "IPv6Address", Configuration.Multicast.Address.IPv6Address, ref logMessage, ref passed, test);
                        }

                        //Port
                        CommonCompare.IntCompare("RequestParameters/Configuration/Multicast/Port", "Port", Configuration.Multicast.Port, ref logMessage, ref passed, test);

                        //TTL
                        CommonCompare.IntCompare("RequestParameters/Configuration/Multicast/TTL", "TTL", Configuration.Multicast.TTL, ref logMessage, ref passed, test);

                        //AutoStart
                        CommonCompare.StringCompare("RequestParameters/Configuration/Multicast/AutoStart", "AutoStart", Configuration.Multicast.AutoStart.ToString(), ref logMessage, ref passed, test);
                    }

                    //SessionTimeout
                    CommonCompare.StringCompare("RequestParameters/Configuration/SessionTimeout", "SessionTimeout", Configuration.SessionTimeout.ToString(), ref logMessage, ref passed, test);

                }
                //ForcePersistence
                CommonCompare.StringCompare("RequestParameters/ForcePersistence", "ForcePersistence", ForcePersistence.ToString(), ref logMessage, ref passed, test);


                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetMetadataConfiguration);
            }
            else
            {
                throw new SoapException("NO Media10.SetMetadataConfiguration COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetMetadataConfigurationOptionsTest(out MetadataConfigurationOptions target, out SoapException ex, out int Timeout, string ConfigurationToken, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetMetadataConfigurationOptions");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetMetadataConfigurationOptions");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetMetadataConfigurationOptions]];

                #region Serialization Temp
                //Media.VideoEncoderConfigurationOptions dsr = new Media.VideoEncoderConfigurationOptions();
                //dsr.JPEG = new Media.JpegOptions();
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.VideoEncoderConfigurationOptions));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(MetadataConfigurationOptions));
                target = (MetadataConfigurationOptions)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetMetadataConfigurationOptions);
            }
            else
            {
                throw new SoapException("NO Media10.GetMetadataConfigurationOptions COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetStreamUriTest(out MediaUri target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.GetStreamUri");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetStreamUri");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetStreamUri]];

                #region Serialization Temp
                //Media.GetSnapshotUriResponse dsr = new Media.GetSnapshotUriResponse();
                //dsr.MediaUri = new Media.MediaUri();
                //dsr.MediaUri.Timeout = "P60T";
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.GetSnapshotUriResponse));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(MediaUri));
                target = (MediaUri)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetStreamUri);
            }
            else
            {
                throw new SoapException("NO Media10.GetStreamUri COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType StartMulticastStreamingTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.StartMulticastStreaming");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[StartMulticastStreaming]];

                #region Analyze request

                //Configuration
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);
         
                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, StartMulticastStreaming);
            }
            else
            {
                throw new SoapException("NO Media10.StartMulticastStreaming COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType SetSynchronizationPointTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.SetSynchronizationPoint");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetSynchronizationPoint]];

                #region Analyze request

                //Configuration
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetSynchronizationPoint);
            }
            else
            {
                throw new SoapException("NO Media10.SetSynchronizationPoint COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType StopMulticastStreamingTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.StopMulticastStreaming");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[StopMulticastStreaming]];

                #region Analyze request

                //Configuration
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, StopMulticastStreaming);
            }
            else
            {
                throw new SoapException("NO Media10.StopMulticastStreaming COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }



        internal StepType AddAudioOutputConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "AddAudioOutputConfiguration";
            int commandId = AddAudioOutputConfiguration;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType RemoveAudioOutputConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "RemoveAudioOutputConfiguration";
            int commandId = RemoveAudioOutputConfiguration;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType AddAudioDecoderConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "AddAudioDecoderConfiguration";
            int commandId = AddAudioDecoderConfiguration;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                //ConfigurationToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType RemoveAudioDecoderConfigurationTest(out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "RemoveAudioDecoderConfiguration";
            int commandId = RemoveAudioDecoderConfiguration;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetAudioDecoderConfigurationTest(out AudioDecoderConfiguration target, out SoapException ex, out int Timeout, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "GetAudioDecoderConfiguration";
            int commandId = GetAudioDecoderConfiguration;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand(commandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Serialization Temp
                //Media.GetSnapshotUriResponse dsr = new Media.GetSnapshotUriResponse();
                //dsr.MediaUri = new Media.MediaUri();
                //dsr.MediaUri.Timeout = "P60T";
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.GetSnapshotUriResponse));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioDecoderConfiguration));
                target = (AudioDecoderConfiguration)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetCompatibleAudioOutputConfigurationsTest(out AudioOutputConfiguration[] target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "GetCompatibleAudioOutputConfigurations";
            int commandId = GetCompatibleAudioOutputConfigurations;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand(commandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Serialization Temp
                //Media.GetSnapshotUriResponse dsr = new Media.GetSnapshotUriResponse();
                //dsr.MediaUri = new Media.MediaUri();
                //dsr.MediaUri.Timeout = "P60T";
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.GetSnapshotUriResponse));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioOutputConfiguration[]));
                target = (AudioOutputConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetCompatibleAudioDecoderConfigurationsTest(out AudioDecoderConfiguration[] target, out SoapException ex, out int Timeout, string ProfileToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "GetCompatibleAudioDecoderConfigurations";
            int commandId = GetCompatibleAudioDecoderConfigurations;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand(commandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Serialization Temp
                //Media.GetSnapshotUriResponse dsr = new Media.GetSnapshotUriResponse();
                //dsr.MediaUri = new Media.MediaUri();
                //dsr.MediaUri.Timeout = "P60T";
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.GetSnapshotUriResponse));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ProfileToken", "ProfileToken", ProfileToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioDecoderConfiguration[]));
                target = (AudioDecoderConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetAudioDecoderConfigurationsTest(out AudioDecoderConfiguration[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "GetAudioDecoderConfigurations";
            int commandId = GetAudioDecoderConfigurations;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand(commandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Serialization Temp
                //Media.GetSnapshotUriResponse dsr = new Media.GetSnapshotUriResponse();
                //dsr.MediaUri = new Media.MediaUri();
                //dsr.MediaUri.Timeout = "P60T";
                //XmlSerializer serializer = new XmlSerializer(typeof(Media.GetSnapshotUriResponse));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                #region Analyze request


                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(AudioDecoderConfiguration[]));
                target = (AudioDecoderConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType SetAudioDecoderConfigurationTest(out SoapException ex, out int Timeout, AudioDecoderConfiguration Configuration, bool ForcePersistence)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "SetAudioDecoderConfiguration";
            int commandId = SetAudioDecoderConfiguration;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Analyze request

                //TODO

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        public StepType GetOSDsTest(out OSDConfiguration[] target, out SoapException ex, out int Timeout, string ConfigurationToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "GetOSDs";
            int commandId = GetOSDs;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(OSDConfiguration[]));
                target = (OSDConfiguration[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        public StepType GetOSDTest(out OSDConfiguration target, out SoapException ex, out int Timeout, string OSDToken, ref XmlElement[] Any)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "GetOSD";
            int commandId = GetOSD;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/OSDToken", "OSDToken", OSDToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(OSDConfiguration));
                target = (OSDConfiguration)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        public StepType GetOSDOptionsTest(out OSDConfigurationOptions target, out SoapException ex, out int Timeout, string ConfigurationToken, ref XmlElement[] Any)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "GetOSDOptions";
            int commandId = GetOSDOptions;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Analyze request

                //ProfileToken
                CommonCompare.StringCompare("RequestParameters/ConfigurationToken", "ConfigurationToken", ConfigurationToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(OSDConfigurationOptions));
                target = (OSDConfigurationOptions)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        public StepType SetOSDTest(out SoapException ex, out int Timeout, OSDConfiguration OSD, ref XmlElement[] Any)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "SetOSD";
            int commandId = SetOSD;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Analyze request

                //TODO

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        public StepType CreateOSDTest(out string target, out SoapException ex, out int Timeout, OSDConfiguration OSD, ref XmlElement[] any)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Media10.CreateOSD");

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[CreateOSD]];

                #region Analyze request

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(string));
                target = (string)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetStreamUri);
            }
            else
            {
                throw new SoapException("NO Media10.GetStreamUri COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        public StepType DeleteOSDTest(out SoapException ex, out int Timeout, string OSDToken, ref XmlElement[] any)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            string commandName = "DeleteOSD";
            int commandId = DeleteOSD;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + commandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[commandId]];

                #region Analyze request

                //TODO

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, commandId);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + commandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }
    }
}
