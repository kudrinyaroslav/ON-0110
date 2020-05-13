using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.IO;
using System.Xml.Serialization;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Device10
{
    /// <summary>
    /// Class for Device Management Service tests
    /// </summary>
    public class DeviceServiceTest : Base.BaseServiceTest
    {

        #region Const

        protected override string ServiceName { get { return "Device10"; } }

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetWsdlUri = 0;
        private const int GetCapabilities = 1;
        private const int GetHostname = 2;
        private const int SetHostname = 3;
        private const int GetDNS = 4;
        private const int SetDNS = 5;
        private const int GetNTP = 6;
        private const int SetNTP = 7;
        private const int GetDeviceInformation = 8;
        private const int GetSystemDateAndTime = 9;
        private const int SetSystemDateAndTime = 10;
        private const int GetScopes = 11;
        private const int SetScopes = 12;
        private const int GetNetworkInterfaces = 13;
        private const int SetNetworkInterfaces = 14;
        private const int GetNetworkProtocols = 15;
        private const int SetNetworkProtocols = 16;
        private const int GetNetworkDefaultGateway = 17;
        private const int SetNetworkDefaultGateway = 18;
        private const int GetUsers = 19;
        private const int CreateUsers = 20;
        private const int DeleteUsers = 21;
        private const int SetUser = 22;
        private const int GetRelayOutputs = 23;
        private const int SetRelayOutputSettings = 24;
        private const int SetRelayOutputState = 25;
        private const int GetSystemLog = 26;
        private const int GetDiscoveryMode = 27;
        private const int SetDiscoveryMode = 28;
        private const int GetSystemSupportInformation = 29;
        private const int SystemReboot = 30;
        private const int SetSystemFactoryDefault = 31;
        private const int AddScopes = 32;
        private const int RemoveScopes = 33;
        private const int GetServices = 34;
        private const int GetServiceCapabilities = 35;
        private const int GetIPAddressFilter = 36;
        private const int SetIPAddressFilter = 37;
        private const int AddIPAddressFilter = 38;
        private const int RemoveIPAddressFilter = 39;
        private const int GetAccessPolicy = 40;
        private const int GetZeroConfiguration = 41;
        private const int SetZeroConfiguration = 42;
        private const int StartFirmwareUpgrade = 43;
        private const int GetSystemUris = 44;
        private const int GetRemoteUser = 45;
        private const int SetRemoteUser = 46;
        private const int StartSystemRestore = 47;
        private const int SendAuxiliaryCommand = 48;
        private const int GetDynamicDNS = 49;
        private const int SetDynamicDNS = 50;
        private const int MaxCommands = 51;

        #endregion //Const

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public DeviceServiceTest(TestCommon testCommon)
            :base(testCommon)
        {
            InitCommandsCount(MaxCommands);   
        }

        #endregion //Constructors

        #region Capabilities

        /// <summary>
        /// Test for GetWsdlUrl
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType GetWsdlUrlTest(out string target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            target = "http://testWsdlUrl";
            Timeout = 0;
            ex = null;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("GetWsdlUri");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetWsdlUri]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(string));
                target = (string)targetObj;

                TestCommon.writeToLog(test, "", true);

                Increment(m_testList.Count, GetWsdlUri);
            }
            else
            {
                Timeout = 0;
                target = null;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        /// <summary>
        /// Test for GetCapabilities
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <param name="capabilityCategory">Input information for validation</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType GetCapabilitiesTest(out Capabilities target, out SoapException ex, out int Timeout, CapabilityCategory[] capabilityCategory)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = new Capabilities();
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList;
            m_testList = TestCommon.GetStepsForCommand("Device10.GetCapabilities");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetCapabilities");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetCapabilities]];

                #region Analyze request

                if (capabilityCategory != null)
                {

                    foreach (CapabilityCategory i in capabilityCategory)
                    {
                        if (test.SelectNodes("RequestParameters[Category=\"" + i.ToString() + "\"]").Count != 1)
                        {
                            passed = false;
                            logMessage = logMessage + "Unexpected category " + i.ToString() + ". ";
                        }
                    }
                    if (test.SelectNodes("RequestParameters/Category").Count != capabilityCategory.Count())
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of categories. ";
                    }
                }
                else
                {
                    if (test.SelectNodes("RequestParameters/Category").Count != 0)
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of categories. ";
                    }
                }

                #endregion //Analyze request

                //Generate response
                int useRealAddress = 0;
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoidSpecial(test, out targetObj, out ex, out Timeout, typeof(Capabilities), out useRealAddress);
                target = (Capabilities)targetObj;

                //Get real path to service
                if (useRealAddress != 0)
                {
                    if (target.Events != null)
                    {
                       target.Events.XAddr = TestCommon.EventsUri;
                    }
                    if (target.Device != null)
                    {
                        target.Device.XAddr = TestCommon.DeviceUri;
                    }
                    if (target.PTZ != null)
                    {
                        target.PTZ.XAddr = TestCommon.PTZUri;
                    }
                    if (target.Media != null)
                    {
                        target.Media.XAddr = TestCommon.MadiaUri;
                    }
                    if (target.Imaging != null)
                    {
                        target.Imaging.XAddr = TestCommon.ImagingUri;
                    }
                    if (target.Extension!= null)
                    {
                        if (target.Extension.DeviceIO !=null)
                            target.Extension.DeviceIO.XAddr = TestCommon.DeviceIO10Uri;
                        if (target.Extension.Recording != null)
                            target.Extension.Recording.XAddr = TestCommon.RecordingUri;
                        if (target.Extension.Receiver != null)
                            target.Extension.Receiver.XAddr = TestCommon.ReceiverUri;
                        if (target.Extension.Search != null)
                            target.Extension.Search.XAddr = TestCommon.SearchUri;
                        if (target.Extension.Replay != null)
                            target.Extension.Replay.XAddr = TestCommon.ReplayUri;
                    }
 
                }
                if (useRealAddress == 2)
                {
                    if (target.Imaging != null)
                    {
                        target.Imaging.XAddr = TestCommon.Imaging20Uri;
                    }
                }

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetCapabilities);
            }
            else
            {
                throw new SoapException("NO Device10.GetCapabilities COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType GetServicesTest(out Service[] target, out SoapException ex, out int Timeout, bool IncludeCapability)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.GetServices");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetServices]];

                #region Change Auth Mode for Future Requests

                TestCommon.GrantPublicInvoke(test);

                #endregion //Change Auth Mode for Future Requests

                #region Analyze request

                //IncludeCapability
                CommonCompare.StringCompare("RequestParameters/IncludeCapability", "IncludeCapability", IncludeCapability.ToString(), ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                int useRealAddress = 0;
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoidSpecial(test, out targetObj, out ex, out Timeout, typeof(Service[]), out useRealAddress);
                target = (Service[])targetObj;

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                if (useRealAddress == 1)
                {
                    TestCommon.HttpType = ProtocolType.HTTP;
                }
                else
                {
                    TestCommon.HttpType = ProtocolType.HTTPS;
                }

                //Get real path to service
                if (useRealAddress != 0)
                {
                    foreach (Service i in target)
                    {
                        switch (i.Namespace)
                        { 
                            case "http://www.onvif.org/ver10/events/wsdl":
                                i.XAddr = TestCommon.EventsUri;
                                break;
                            case "http://www.onvif.org/ver10/device/wsdl":
                                i.XAddr = TestCommon.DeviceUri;
                                break;
                            case "http://www.onvif.org/ver10/deviceIO/wsdl":
                                i.XAddr = TestCommon.DeviceIO10Uri;
                                break;
                            case "http://www.onvif.org/ver10/imaging/wsdl":
                                i.XAddr = TestCommon.ImagingUri;
                                break;
                            case "http://www.onvif.org/ver20/imaging/wsdl":
                                i.XAddr = TestCommon.Imaging20Uri;
                                break;
                            case "http://www.onvif.org/ver10/media/wsdl":
                                i.XAddr = TestCommon.MadiaUri;
                                break;
                            case "http://www.onvif.org/ver20/ptz/wsdl":
                                i.XAddr = TestCommon.PTZUri;
                                break;
                            case "http://www.onvif.org/ver10/search/wsdl":
                                i.XAddr = TestCommon.SearchUri;
                                break;
                            case "http://www.onvif.org/ver10/recording/wsdl":
                                i.XAddr =  TestCommon.RecordingUri;
                                break;
                            case "http://www.onvif.org/v3/AccessControl/wsdl":
                                i.XAddr = TestCommon.PACSServiceUri;
                                break;                            
                            case "http://www.onvif.org/v3/DoorControl/wsdl":
                                i.XAddr = TestCommon.DoorServiceUri;
                                break;
                            case "http://www.onvif.org/ver10/accesscontrol/wsdl":
                                i.XAddr = TestCommon.PACS12ServiceUri;
                                break;                            
                            case "http://www.onvif.org/ver10/doorcontrol/wsdl":
                                i.XAddr = TestCommon.Door12ServiceUri;
                                break;
                            case "http://www.onvif.org/ver10/receiver/wsdl":
                                i.XAddr = TestCommon.ReceiverUri;
                                break;
                            case "http://www.onvif.org/ver10/replay/wsdl":
                                i.XAddr = TestCommon.ReplayUri;
                                break;
                            case "http://www.onvif.org/ver10/advancedsecurity/wsdl":
                                i.XAddr = TestCommon.AdvancedSecurityUri;
                                break;
                            case "http://www.onvif.org/ver10/accessrules/wsdl":
                                i.XAddr = TestCommon.AccessRulesUri;
                                break;
                            case "http://www.onvif.org/ver10/credential/wsdl":
                                i.XAddr = TestCommon.CredentialUri;
                                break;
                            case "http://www.onvif.org/ver10/schedule/wsdl":
                                i.XAddr = TestCommon.ScheduleUri;
                                break;
                            case "http://www.onvif.org/ver20/media/wsdl":
                                if (DUT.CameraWebService.TestSettings.switchToNewMedia2Service == true)
                                {
                                    i.XAddr = TestCommon.Media2SVCUri;
                                }
                                else
                                {
                                    //i.XAddr = TestCommon.Media2SVCUri;
                                    i.XAddr = TestCommon.Media2Uri;
                                }
                                break;
                            case "http://www.onvif.org/ver20/analytics/wsdl":
                                i.XAddr = TestCommon.AnalyticsEngineUri;
                                break;
                            case "https://www.onvif.org/ver10/provisioning/wsdl":
                                i.XAddr = TestCommon.ProvisioningUri;
                                break;
                            case "http://www.onvif.org/ver10/thermal/wsdl":
                                i.XAddr = TestCommon.ThermalUri;
                                break;

                        }

                    }
                }

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetServices);
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

        internal StepType GetServiceCapabilitiesTest(out DeviceServiceCapabilities target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.GetServiceCapabilities");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetServiceCapabilities]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(DeviceServiceCapabilities));
                target = (DeviceServiceCapabilities)targetObj;

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetServiceCapabilities);
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

        #endregion //Capabilities

        #region Network

        /// <summary>
        /// Test for GetHostname
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType GetHostnameTest(out HostnameInformation target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = new HostnameInformation();
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("GetHostname");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetHostname]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(HostnameInformation));
                target = (HostnameInformation)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetHostname);
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

        /// <summary>
        /// Test for SetHostname
        /// </summary>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType SetHostnameTest(out SoapException ex, out int Timeout, string Name)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("SetHostname");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetHostname]];

                #region Analyze request

                if (test.SelectNodes("RequestParameters/Name")[0].InnerText != Name)
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected name " + Name + ". ";
                }

                #endregion //Analyze request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetHostname);
            }
            else
            {
                res = StepType.None;
                Timeout = 0;
                ex = null;
            }
            return res;
        }

        /// <summary>
        /// Test for GetDNS
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType GetDNSTest(out DNSInformation target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = new DNSInformation();
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetDNS";
            int tmpCommandNumber = GetDNS;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            //TEMP: for backward compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand(tmpCommandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(DNSInformation));
                target = (DNSInformation)targetObj;

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

        /// <summary>
        /// Test for SetDNS
        /// </summary>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="timeOut">Output timeout for NoResponse response</param>
        /// <param name="FromDHCP">Input information for verification</param>
        /// <param name="SearchDomain">Input information for verification</param>
        /// <param name="DNSManual">Input information for verification</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType SetDNSTest(out SoapException ex, out int Timeout, bool FromDHCP, string[] SearchDomain, IPAddress[] DNSManual)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "SetDNS";
            int tmpCommandNumber = SetDNS;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            //TEMP: for backward compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand(tmpCommandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                #region Analize request

                //FromDHCP
                CommonCompare.StringCompare("RequestParameters/FromDHCP", "FromDHCP", FromDHCP.ToString(), ref logMessage, ref passed, test);

                //SearchDomain
                if (SearchDomain != null)
                {

                    foreach (string i in SearchDomain)
                    {
                        if (test.SelectNodes("RequestParameters[SearchDomain=\"" + i + "\"]").Count != 1)
                        {
                            passed = false;
                            logMessage = logMessage + "Unexpected SearchDomain " + i + ". ";
                        }
                    }
                    if (test.SelectNodes("RequestParameters/SearchDomain").Count != SearchDomain.Count())
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of SearchDomains. ";
                    }
                }
                else
                {
                    if (test.SelectNodes("RequestParameters/SearchDomain").Count != 0)
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of SearchDomains. ";
                    }
                }
                //DNSManual
                if (DNSManual != null)
                {

                    foreach (IPAddress i in DNSManual)
                    {
                        string IPstring = i.Type.ToString() + " ";
                        if (i.Type == IPType.IPv4)
                        {
                            IPstring = IPstring + i.IPv4Address;
                            if (i.IPv6Address != null)
                            {
                                passed = false;
                                logMessage = logMessage + "Unexpected IPv6 instead IPv4" + IPstring + "" + i.IPv6Address + ". ";
                            }
                        }
                        else
                        {
                            IPstring = IPstring + i.IPv6Address;
                            if (i.IPv4Address != null)
                            {
                                passed = false;
                                logMessage = logMessage + "Unexpected IPv4 instead IPv6" + IPstring + "" + i.IPv4Address + ". ";
                            }
                        }
                        if (test.SelectNodes("RequestParameters[DNSManual=\"" + IPstring + "\"]").Count != 1)
                        {
                            passed = false;
                            logMessage = logMessage + "Unexpected DNSManual " + IPstring + ". ";
                        }
                    }
                    if (test.SelectNodes("RequestParameters/DNSManual").Count != DNSManual.Count())
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of DNSManual. ";
                    }
                }
                else
                {
                    if (test.SelectNodes("RequestParameters/SearchDomain").Count != 0)
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of SearchDomains. ";
                    }
                }

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

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

        /// <summary>
        /// Test for GetDynamicDNS
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType GetDynamicDNSTest(out DynamicDNSInformation target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = new DynamicDNSInformation();
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetDynamicDNS";
            int tmpCommandNumber = GetDynamicDNS;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            //TEMP: for backward compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand(tmpCommandName);
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(DynamicDNSInformation));
                target = (DynamicDNSInformation)targetObj;

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


        /// <summary>
        /// Test for GetNTP
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType GetNTPTest(out NTPInformation target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = new NTPInformation();
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList;
            m_testList = TestCommon.GetStepsForCommand("Device10.GetNTP");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetNTP");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetNTP]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(NTPInformation));
                target = (NTPInformation)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetNTP);
            }
            else
            {
                throw new SoapException("NO Device10.GetNTP COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        /// <summary>
        /// Test for SetNTP
        /// </summary>
        /// <param name="ex">Output data for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <param name="FromDHCP">Output timeout for NoResponse response</param>
        /// <param name="NTPManual">Data for request verification</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType SetNTPTest(out SoapException ex, out int Timeout, bool FromDHCP, NetworkHost[] NTPManual)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("SetNTP");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetNTP]];

                #region Analize request

                //FromDHCP
                if (test.SelectNodes("RequestParameters/FromDHCP")[0].InnerText != FromDHCP.ToString())
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected FromDHCP " + FromDHCP.ToString() + ". ";
                }

                //NTPManual
                if (NTPManual != null)
                {
                    foreach (NetworkHost i in NTPManual)
                    {
                        string IPstring = i.Type.ToString() + " ";
                        if (i.AnyAttr != null)
                        {
                            passed = false;
                            logMessage = logMessage + "Unexpected AnyAttr. ";
                        }
                        if (i.Extension != null)
                        {
                            passed = false;
                            logMessage = logMessage + "Unexpected Extension. ";
                        }
                        switch (i.Type)
                        {
                            case NetworkHostType.IPv4:
                                {
                                    IPstring = IPstring + i.IPv4Address;
                                    if (i.IPv6Address != null)
                                    {
                                        passed = false;
                                        logMessage = logMessage + "Unexpected IPv6 instead IPv4" + IPstring + "" + i.IPv6Address + ". ";
                                    }
                                    if (i.DNSname != null)
                                    {
                                        passed = false;
                                        logMessage = logMessage + "Unexpected DNSName instead IPv4" + IPstring + "" + i.DNSname + ". ";
                                    }
                                    break;
                                }
                            case NetworkHostType.IPv6:
                                {
                                    IPstring = IPstring + i.IPv6Address;
                                    if (i.IPv4Address != null)
                                    {
                                        passed = false;
                                        logMessage = logMessage + "Unexpected IPv4 instead IPv6" + IPstring + "" + i.IPv4Address + ". ";
                                    }
                                    if (i.DNSname != null)
                                    {
                                        passed = false;
                                        logMessage = logMessage + "Unexpected DNSName instead IPv6" + IPstring + "" + i.DNSname + ". ";
                                    }
                                    break;
                                }
                            case NetworkHostType.DNS:
                                {
                                    IPstring = IPstring + i.DNSname;
                                    if (i.IPv4Address != null)
                                    {
                                        passed = false;
                                        logMessage = logMessage + "Unexpected IPv4 instead DNSName" + IPstring + "" + i.IPv4Address + ". ";
                                    }
                                    if (i.IPv6Address != null)
                                    {
                                        passed = false;
                                        logMessage = logMessage + "Unexpected IPv6 instead DNSName" + IPstring + "" + i.IPv6Address + ". ";
                                    }
                                    break;
                                }
                        }
                        if (test.SelectNodes("RequestParameters[NTPManual=\"" + IPstring + "\"]").Count != 1)
                        {
                            passed = false;
                            logMessage = logMessage + "Unexpected NTPManual " + IPstring + ". ";
                        }
                    }
                    if (test.SelectNodes("RequestParameters/NTPManual").Count != NTPManual.Count())
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of NTPManual. ";
                    }
                }
                else
                {
                    if (test.SelectNodes("RequestParameters/NTPManual").Count != 0)
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of NTPManual. ";
                    }
                }

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetNTP);
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

        /// <summary>
        /// Test for GetNetworkInterfaces
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output data for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType GetNetworkInterfacesTest(out NetworkInterface[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("GetNetworkInterfaces");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetNetworkInterfaces]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(NetworkInterface[]));
                target = (NetworkInterface[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetNetworkInterfaces);
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

        /// <summary>
        /// Test for SetNetworkInterfaces
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output data for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <param name="InterfaceToken">Input information for verification</param>
        /// <param name="NetworkInterface">Input information for verification</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType SetNetworkInterfacesTest(out bool target, out SoapException ex, out int Timeout, string InterfaceToken, NetworkInterfaceSetConfiguration NetworkInterface)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = false;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("SetNetworkInterfaces");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetNetworkInterfaces]];

                #region Analize request

                //TODO: other parameters

                //InterfaceToken
                if (test.SelectNodes("RequestParameters/InterfaceToken")[0].InnerText != InterfaceToken)
                {
                    passed = false;
                    logMessage = logMessage + "Wrong InterfaceToken: " + InterfaceToken + " instead of " + test.SelectNodes("RequestParameters/InterfaceToken")[0].InnerText + ".";
                }

                //NetworkInterface.IPv4
                if (test.SelectNodes("RequestParameters/NetworkInterface/IPv4").Count > 0)
                {
                    //Enabled
                    if (test.SelectNodes("RequestParameters/NetworkInterface/IPv4/Enabled").Count > 0)
                    {
                        if (NetworkInterface.IPv4.EnabledSpecified != true)
                        {
                            passed = false;
                            logMessage = logMessage + "There is no expected IPv4.Enabled.";
                        }
                        else
                        {
                            if (Convert.ToBoolean(test.SelectNodes("RequestParameters/NetworkInterface/IPv4/Enabled")[0].InnerText) != NetworkInterface.IPv4.Enabled)
                            {
                                passed = false;
                                logMessage = logMessage + "Wrong IPv4.Enabled: " + NetworkInterface.IPv4.Enabled.ToString() + " instead of " + test.SelectNodes("RequestParameters/NetworkInterface/IPv4/Enabled")[0].InnerText + ".";
                            }
                        }
                    }
                    else
                    {
                        if (NetworkInterface.IPv4.EnabledSpecified != false)
                        {
                            passed = false;
                            logMessage = logMessage + "There is unexpected IPv4.Enabled.";
                        }
                    }

                    //Manual
                    //    if (test.SelectNodes("RequestParameters/NetworkInterface/IPv4/Manual").Count == NetworkInterface.IPv4.Manual.Count())
                    //    {
                    //        foreach (PrefixedIPv4Address i in NetworkInterface.IPv4.Manual)
                    //        {
                    //            string requestStr = "RequestParameters/NetworkInterface/IPv4/Manual[Address=\"" + i.Address + "\"]";
                    //            if (test.SelectNodes(requestStr).Count == 0)
                    //            {
                    //                passed = false;
                    //                logMessage = logMessage + "There is unexpected IPv4.Manual " + i.Address + ".";
                    //            }
                    //            else
                    //            {
                    //                if (Convert.ToInt32(test.SelectSingleNode(requestStr).SelectSingleNode("PrefixLength").InnerText) != i.PrefixLength)
                    //                {
                    //                    passed = false;
                    //                    logMessage = logMessage + "There is wrong PrefixLength for IPv4.Manual " + i.Address + ".";
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        passed = false;
                    //        logMessage = logMessage + "Wrong number of IPv4.Manual.";
                    //    }
                    //}
                    //else
                    //{
                    //    if (NetworkInterface.IPv4 != null)
                    //    {
                    //        passed = false;
                    //        logMessage = logMessage + "There is unexpected IPv4.";
                    //    }
                    }

                    //NetworkInterface.IPv6
                    if (test.SelectNodes("RequestParameters/NetworkInterface/IPv6").Count > 0)
                    {
                        //Enabled
                        if (test.SelectNodes("RequestParameters/NetworkInterface/IPv6/Enabled").Count > 0)
                        {
                            if (NetworkInterface.IPv6.EnabledSpecified != true)
                            {
                                passed = false;
                                logMessage = logMessage + "There is no expected IPv6.Enabled.";
                            }
                            else
                            {
                                if (Convert.ToBoolean(test.SelectNodes("RequestParameters/NetworkInterface/IPv6/Enabled")[0].InnerText) != NetworkInterface.IPv6.Enabled)
                                {
                                    passed = false;
                                    logMessage = logMessage + "Wrong IPv6.Enabled: " + NetworkInterface.IPv6.Enabled.ToString() + " instead of " + test.SelectNodes("RequestParameters/NetworkInterface/IPv6/Enabled")[0].InnerText + ".";
                                }
                            }
                        }
                        else
                        {
                            if (NetworkInterface.IPv6.EnabledSpecified != false)
                            {
                                passed = false;
                                logMessage = logMessage + "There is unexpected IPv6.Enabled.";
                            }
                        }

                        //Manual
                        if (test.SelectNodes("RequestParameters/NetworkInterface/IPv6/Manual").Count == NetworkInterface.IPv6.Manual.Count())
                        {
                            foreach (PrefixedIPv6Address i in NetworkInterface.IPv6.Manual)
                            {
                                string requestStr = "RequestParameters/NetworkInterface/IPv6/Manual[Address=\"" + i.Address + "\"]";
                                if (test.SelectNodes(requestStr).Count == 0)
                                {
                                    passed = false;
                                    logMessage = logMessage + "There is unexpected IPv6.Manual " + i.Address + ".";
                                }
                                else
                                {
                                    if (Convert.ToInt32(test.SelectSingleNode(requestStr).SelectSingleNode("PrefixLength").InnerText) != i.PrefixLength)
                                    {
                                        passed = false;
                                        logMessage = logMessage + "There is wrong PrefixLength for IPv6.Manual " + i.Address + ".";
                                    }
                                }
                            }
                        }
                        else
                        {
                            passed = false;
                            logMessage = logMessage + "Wrong number of IPv6.Manual.";
                        }
                    }
                    else
                    {
                        if (NetworkInterface.IPv6 != null)
                        {
                            passed = false;
                            logMessage = logMessage + "There is unexpected IPv6.";
                        }
                    }

                #endregion //Analize request

                    //Generate response (TODO normal deserialization)
                    string typeRes = test.SelectNodes("Response")[0].InnerText;

                    switch (typeRes)
                    {
                        case "Normal":
                            {
                                res = StepType.Normal;

                                target = Convert.ToBoolean(test.SelectNodes("ResponseParameters/NeedReboot")[0].InnerText);

                                Timeout = 0;
                                ex = null;
                                break;
                            }
                        case "NoResponse":
                            {
                                res = StepType.NoResponse;
                                ex = null;
                                Timeout = TestCommon.m_timeOut;
                                break;
                            }
                        case "Fault":
                            {
                                res = StepType.Fault;
                                Timeout = 0;
                                target = false;
                                ex = TestCommon.generateExceptionObject(test);
                                break;
                            }
                        default:
                            {
                                res = StepType.None;
                                Timeout = 0;
                                target = false;
                                ex = null;
                                break;
                            }
                    }

                    //Log message
                    TestCommon.writeToLog(test, logMessage, passed);

                    Increment(m_testList.Count, SetNetworkInterfaces);
                }
                else
                {
                    res = StepType.None;
                    Timeout = 0;
                    target = false;
                    ex = null;
                    res = StepType.None;
                }
                return res;
            }
        
        /// <summary>
        /// Test for GetNetworkInterfaces
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output data for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType GetNetworkProtocolsTest(out NetworkProtocol[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("GetNetworkProtocols");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetNetworkProtocols]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(NetworkProtocol[]));
                target = (NetworkProtocol[])targetObj;

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetNetworkProtocols);
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

        /// <summary>
        /// Test for SetNetworkInterfaces
        /// </summary>
        /// <param name="ex">Output data for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <param name="NetworkProtocols"></param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType SetNetworkProtocolsTest(out SoapException ex, out int Timeout, NetworkProtocol[] NetworkProtocols)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("SetNetworkProtocols");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetNetworkProtocols]];

                #region Analize request

                //NetworkProtocol
                if (test.SelectNodes("RequestParameters/NetworkProtocol").Count == NetworkProtocols.Count())
                {
                    foreach (NetworkProtocol i in NetworkProtocols)
                    {
                        //Name
                        string requestStr = "RequestParameters/NetworkProtocol[Name=\"" + i.Name + "\"]";
                        if (test.SelectNodes(requestStr).Count == 0)
                        {
                            passed = false;
                            logMessage = logMessage + "There is unexpected NetworkProtocol " + i.Name + ".";
                        }
                        else
                        {
                            //Enable
                            if (Convert.ToBoolean(test.SelectSingleNode(requestStr).SelectSingleNode("Enabled").InnerText) != i.Enabled)
                            {
                                passed = false;
                                logMessage = logMessage + "There is wrong Enabled for NetworkProtocol " + i.Name + ".";
                            }

                            //Port
                            if (test.SelectSingleNode(requestStr).SelectNodes("Port").Count == i.Port.Count())
                            {
                                foreach (int j in i.Port)
                                {
                                    if (test.SelectSingleNode(requestStr).SelectNodes("Port[text()=" + j.ToString() + "]").Count == 0)
                                    {
                                        passed = false;
                                        logMessage = logMessage + "There is unexpected Port " + j.ToString() + " for NetworkProtocol " + i.Name + ".";
                                    }
                                }
                            }
                            else
                            {
                                passed = false;
                                logMessage = logMessage + "Wrong number of Port for NetworkProtocol " + i.Name + ".";
                            }
                        }
                    }
                }
                else
                {
                    passed = false;
                    logMessage = logMessage + "Wrong number of NetworkProtocols.";
                }

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetNetworkProtocols);
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

        /// <summary>
        /// Test for GetNetworkDefaultGateway
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output data for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType GetNetworkDefaultGatewayTest(out NetworkGateway target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("GetNetworkDefaultGateway");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetNetworkDefaultGateway]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(NetworkGateway));
                target = (NetworkGateway)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetNetworkDefaultGateway);
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

        /// <summary>
        /// Test for SetNetworkDefaultGateway
        /// </summary>
        /// <param name="ex">Output data for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <param name="IPv4Address">Data for request verification</param>
        /// <param name="IPv6Address">Data for request verification</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType SetNetworkDefaultGatewayTest(out SoapException ex, out int Timeout, string[] IPv4Address, string[] IPv6Address)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("SetNetworkDefaultGateway");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetNetworkDefaultGateway]];

                #region Analize request

                //IPv4Address
                if (IPv4Address == null)
                {
                    if (test.SelectNodes("RequestParameters/IPv4Address").Count != 0)
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of IPv4Address.";
                    }
                }
                else
                {
                    if (test.SelectNodes("RequestParameters/IPv4Address").Count == IPv4Address.Count())
                    {
                        //if (IPv4Address.Count() == 0)
                        //{
                        //    passed = false;
                        //    logMessage = logMessage + "Tag without IPv4Address values.";
                        //}
                        foreach (string i in IPv4Address)
                        {
                            if (test.SelectNodes("RequestParameters/IPv4Address[text()=\"" + i + "\"]").Count == 0)
                            {
                                passed = false;
                                logMessage = logMessage + "There is unexpected IPv4Address " + i + ".";
                            }
                        }
                    }
                    else
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of IPv4Address.";
                    }
                }

                //IPv6Address
                if (IPv6Address == null)
                {
                    if (test.SelectNodes("RequestParameters/IPv6Address").Count != 0)
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of IPv6Address.";
                    }
                }
                else
                {
                    if (test.SelectNodes("RequestParameters/IPv6Address").Count == IPv6Address.Count())
                    {
                        //if (IPv6Address.Count() == 0)
                        //{
                        //    passed = false;
                        //    logMessage = logMessage + "Tag without IPv6Address values.";
                        //}
                        foreach (string i in IPv6Address)
                        {
                            if (test.SelectNodes("RequestParameters/IPv6Address[text()=\"" + i + "\"]").Count == 0)
                            {
                                passed = false;
                                logMessage = logMessage + "There is unexpected IPv6Address " + i + ".";
                            }
                        }
                    }
                    else
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of IPv6Address.";
                    }
                }

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetNetworkDefaultGateway);
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

        /// <summary>
        /// Test for GetZeroConfiguration
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output data for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType GetZeroConfigurationTest(out NetworkZeroConfiguration target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("GetZeroConfiguration");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetZeroConfiguration]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(NetworkZeroConfiguration));
                target = (NetworkZeroConfiguration)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetZeroConfiguration);
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

                /// <summary>
        /// Test for SetZeroConfiguration
        /// </summary>
        /// <param name="ex">Output data for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <param name="IPv4Address">Data for request verification</param>
        /// <param name="IPv6Address">Data for request verification</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType SetZeroConfigurationTest(out SoapException ex, out int Timeout, string InterfaceToken, bool Enabled)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("SetZeroConfiguration");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetZeroConfiguration]];

                #region Analize request

                //InterfaceToken
                if (test.SelectNodes("RequestParameters/InterfaceToken")[0].InnerText != InterfaceToken)
                {
                    passed = false;
                    logMessage = logMessage + "Wrong InterfaceToken: " + InterfaceToken + " instead of " + test.SelectNodes("RequestParameters/InterfaceToken")[0].InnerText + ".";
                }

                if (test.SelectNodes("RequestParameters/Enabled")[0].InnerText != Enabled.ToString())
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected Enabled " + Enabled.ToString() + ". ";
                }

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetZeroConfiguration);
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
        #endregion //Network

        #region IP Filtering

        internal StepType  GetIPAddressFilterTest(out IPAddressFilter r, out SoapException ex, out int Timeout)
        {
            var res = StepType.None;

            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("GetIPAddressFilter");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetIPAddressFilter]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(IPAddressFilter));
                r = (IPAddressFilter)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetIPAddressFilter);
            }
            else
            {
                Timeout = 0;
                r = null;
                ex = null;
                res = StepType.None;
            }

            return res;
        }

        internal bool ValidateIPAddressFilter(IPAddressFilter ipAddressFilter, StringBuilder logMessage)
        {
            if (null == ipAddressFilter)
            {
                logMessage.AppendLine("Received IPAddressFilter is null");
                return false;
            }

            var passed = true;
            var prefix = "    ";
            logMessage.AppendLine("Validation of IPAddressFilter");
            logMessage.AppendLine(string.Format("{0}Type: {1}", prefix, ipAddressFilter.Type));
            if (null != ipAddressFilter.IPv4Address)
                foreach (var ip in ipAddressFilter.IPv4Address)
                {
                    logMessage.AppendLine(string.Format("{0}Validation of net address: {1}", prefix, ip.Address));
                    System.Net.IPAddress p;
                    if (!System.Net.IPAddress.TryParse(ip.Address, out p))
                    {
                        passed = false;
                        logMessage.AppendLine(string.Format("{0}{0}Wrong net address", prefix));
                    }
                    //else if (p.GetAddressBytes().Count() != ip.PrefixLength)
                    //{
                    //    logMessage.AppendLine(string.Format("{0}{0}Wrong PrefixLength of netmask {1}: expected {2}, actual {3}", 
                    //                          prefix, ip.Address, p.GetAddressBytes().Count(), ip.PrefixLength));
                    //}
                }

            return passed;
        }

        internal StepType SetIPAddressFilterTest(out SoapException ex, out int Timeout, IPAddressFilter ipAddressFilter)
        {
            var res = StepType.None;
            bool passed = true;
            var logMessage = new StringBuilder();

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("SetIPAddressFilter");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetIPAddressFilter]];

                #region Analize request

                passed = ValidateIPAddressFilter(ipAddressFilter, logMessage);
                
                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage.ToString(), passed);

                Increment(m_testList.Count, SetIPAddressFilter);
            }
            else
            {
                Timeout = 0;
                ex = null;
                res = StepType.None;
            }

            return res;
        }

        internal StepType AddIPAddressFilterTest(out SoapException ex, out int Timeout, IPAddressFilter ipAddressFilter)
        {
            var res = StepType.None;
            bool passed = true;
            var logMessage = new StringBuilder();

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("AddIPAddressFilter");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[AddIPAddressFilter]];

                #region Analize request

                passed = ValidateIPAddressFilter(ipAddressFilter, logMessage);

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage.ToString(), passed);

                Increment(m_testList.Count, AddIPAddressFilter);
            }
            else
            {
                Timeout = 0;
                ex = null;
                res = StepType.None;
            }

            return res;
        }

        internal StepType RemoveIPAddressFilterTest(out SoapException ex, out int Timeout, IPAddressFilter ipAddressFilter)
        {
            var res = StepType.None;
            bool passed = true;
            var logMessage = new StringBuilder();

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("RemoveIPAddressFilter");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[RemoveIPAddressFilter]];

                #region Analize request

                passed = ValidateIPAddressFilter(ipAddressFilter, logMessage);

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage.ToString(), passed);

                Increment(m_testList.Count, RemoveIPAddressFilter);
            }
            else
            {
                Timeout = 0;
                ex = null;
                res = StepType.None;
            }

            return res;
        }

        #endregion

        #region System

        /// <summary>
        /// Test for GetDeviceInformation
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="Model">Output data for Normal response (Model)</param>
        /// <param name="FirmwareVersion">Output data for Normal response (FirmwareVersion)</param>
        /// <param name="SerialNumber">Output data for Normal response (SerialNumber)</param>
        /// <param name="HardwareId">Output data for Normal response (HardwareId)</param>
        /// <param name="ex">Output data for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType GetDeviceInformationTest(out string target, out string Model, out string FirmwareVersion, out string SerialNumber, out string HardwareId, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            target = "";
            Model = "";
            FirmwareVersion = "";
            SerialNumber = "";
            HardwareId = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("GetDeviceInformation");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetDeviceInformation]];

                //Generate response
                string typeRes = test.SelectNodes("Response")[0].InnerText;

                switch (typeRes)
                {
                    case "Normal":
                        {
                            #region Change Auth Mode for Future Requests

                            TestCommon.GrantPublicInvoke(test);

                            #endregion //Change Auth Mode for Future Requests

                            res = StepType.Normal;
                            target = test.SelectNodes("ResponseParameters/Manufacturer")[0].InnerXml;
                            Model = test.SelectNodes("ResponseParameters/Model")[0].InnerXml;
                            FirmwareVersion = test.SelectNodes("ResponseParameters/FirmwareVersion")[0].InnerXml;
                            SerialNumber = test.SelectNodes("ResponseParameters/SerialNumber")[0].InnerXml;
                            HardwareId = test.SelectNodes("ResponseParameters/HardwareId")[0].InnerXml;

                            Timeout = 0;
                            ex = null;
                            break;
                        }
                    case "NoResponse":
                        {
                            res = StepType.NoResponse;
                            ex = null;
                            Timeout = TestCommon.m_timeOut;
                            break;
                        }
                    case "Fault":
                        {
                            res = StepType.Fault;
                            Timeout = 0;
                            target = null;
                            ex = TestCommon.generateExceptionObject(test);
                            break;
                        }
                    default:
                        {
                            res = StepType.None;
                            Timeout = 0;
                            target = null;
                            ex = null;
                            break;
                        }
                }

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetDeviceInformation);
            }
            else
            {
                res = StepType.Normal;
                Timeout = 0;
                ex = null;
                target = "DUTik";
                Model = "DUTik Model";
                FirmwareVersion = "DUTik FirmwareVersion";
                SerialNumber = "DUTik SerialNumber";
                HardwareId = "DUTik HardwareId";
            }
            return res;
        }

        /// <summary>
        /// Test for GetSystemDateAndTime
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType GetSystemDateAndTimeTest(out SystemDateTime target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;

            target = new SystemDateTime();
            Timeout = 0;
            ex = null;

            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("GetSystemDateAndTime");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetSystemDateAndTime]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(SystemDateTime));
                target = (SystemDateTime)targetObj;

                if (test.SelectNodes("ResponseParameters[@now=\"true\"]").Count != 0)
                {
                    if (target.UTCDateTime != null)
                    {
                        target.UTCDateTime.Date.Year = target.UTCDateTime.Date.Year + System.DateTime.UtcNow.Year;
                        target.UTCDateTime.Date.Month = target.UTCDateTime.Date.Month + System.DateTime.UtcNow.Month;
                        target.UTCDateTime.Date.Day = target.UTCDateTime.Date.Day + System.DateTime.UtcNow.Day;
                        target.UTCDateTime.Time.Hour = target.UTCDateTime.Time.Hour + System.DateTime.UtcNow.Hour;
                        target.UTCDateTime.Time.Minute = target.UTCDateTime.Time.Minute + System.DateTime.UtcNow.Minute;
                        target.UTCDateTime.Time.Second = target.UTCDateTime.Time.Second + System.DateTime.UtcNow.Second;
                    }
                    if (target.LocalDateTime != null)
                    {
                        target.LocalDateTime.Date.Year = target.LocalDateTime.Date.Year + System.DateTime.UtcNow.Year;
                        target.LocalDateTime.Date.Month = target.LocalDateTime.Date.Month + System.DateTime.UtcNow.Month;
                        target.LocalDateTime.Date.Day = target.LocalDateTime.Date.Day + System.DateTime.UtcNow.Day;
                        target.LocalDateTime.Time.Hour = target.LocalDateTime.Time.Hour + System.DateTime.UtcNow.Hour;
                        target.LocalDateTime.Time.Minute = target.LocalDateTime.Time.Minute + System.DateTime.UtcNow.Minute;
                        target.LocalDateTime.Time.Second = target.LocalDateTime.Time.Second + System.DateTime.UtcNow.Second;
                    }
                }

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetSystemDateAndTime);
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

        /// <summary>
        /// Test for SetSystemDateAndTime
        /// </summary>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <param name="DateTimeType">Data for request verification</param>
        /// <param name="DaylightSavings">Data for request verification</param>
        /// <param name="TimeZone">Data for request verification</param>
        /// <param name="UTCDateTime">Data for request verification</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType SetSystemDateAndTimeTest(out SoapException ex, out int Timeout, SetDateTimeType DateTimeType, bool DaylightSavings, TimeZone TimeZone, DateTime UTCDateTime)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("SetSystemDateAndTime");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetSystemDateAndTime]];

                #region Analize request

                //DateTimeType
                if (test.SelectNodes("RequestParameters/DateTimeType")[0].InnerText != DateTimeType.ToString())
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected DateTimeType " + DateTimeType.ToString() + ". ";
                }

                //DaylightSavings
                if (test.SelectNodes("RequestParameters/DaylightSavings")[0].InnerText != DaylightSavings.ToString())
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected DaylightSavings " + DaylightSavings.ToString() + ". ";
                }

                //TimeZone
                if (TimeZone != null)
                {
                    if (test.SelectNodes("RequestParameters/TimeZone").Count == 0)
                    {
                        passed = false;
                        logMessage = logMessage + "TimeZone when it isn't expected. ";
                    }
                    else
                    {
                        if (test.SelectNodes("RequestParameters/TimeZone")[0].InnerText != TimeZone.TZ)
                        {
                            passed = false;
                            logMessage = logMessage + "Unexpected TimeZone " + TimeZone.TZ + ". ";
                        }
                    }
                }
                else
                {
                    if (test.SelectNodes("RequestParameters/TimeZone").Count != 0)
                    {
                        passed = false;
                        logMessage = logMessage + "No TimeZone when it is expected. ";

                    }
                }

                //UTCDateTime
                if (UTCDateTime != null)
                {
                    if (test.SelectNodes("RequestParameters/UTCDateTime").Count == 0)
                    {
                        passed = false;
                        logMessage = logMessage + "UTCDateTime when it isn't expected. ";
                    }
                    else
                    {
                        if (test.SelectNodes("RequestParameters/UTCDateTime[@now=\"true\"]").Count == 0)
                        {
                            if (test.SelectNodes("RequestParameters/UTCDateTime/Time/Hour")[0].InnerText != UTCDateTime.Time.Hour.ToString())
                            {
                                passed = false;
                                logMessage = logMessage + "Unexpected UTCDateTime/Time/Hour " + UTCDateTime.Time.Hour.ToString() + ". ";
                            }
                            if (test.SelectNodes("RequestParameters/UTCDateTime/Time/Minute")[0].InnerText != UTCDateTime.Time.Minute.ToString())
                            {
                                passed = false;
                                logMessage = logMessage + "Unexpected UTCDateTime/Time/Minute " + UTCDateTime.Time.Minute.ToString() + ". ";
                            }
                            if (test.SelectNodes("RequestParameters/UTCDateTime/Time/Second")[0].InnerText != UTCDateTime.Time.Second.ToString())
                            {
                                passed = false;
                                logMessage = logMessage + "Unexpected UTCDateTime/Time/Second " + UTCDateTime.Time.Second.ToString() + ". ";
                            }
                            if (test.SelectNodes("RequestParameters/UTCDateTime/Date/Year")[0].InnerText != UTCDateTime.Date.Year.ToString())
                            {
                                passed = false;
                                logMessage = logMessage + "Unexpected UTCDateTime/Date/Year " + UTCDateTime.Date.Year.ToString() + ". ";
                            }
                            if (test.SelectNodes("RequestParameters/UTCDateTime/Date/Month")[0].InnerText != UTCDateTime.Date.Month.ToString())
                            {
                                passed = false;
                                logMessage = logMessage + "Unexpected UTCDateTime/Date/Month " + UTCDateTime.Date.Month.ToString() + ". ";
                            }
                            if (test.SelectNodes("RequestParameters/UTCDateTime/Date/Day")[0].InnerText != UTCDateTime.Date.Day.ToString())
                            {
                                passed = false;
                                logMessage = logMessage + "Unexpected UTCDateTime/Date/Day " + UTCDateTime.Date.Day.ToString() + ". ";
                            }
                        }
                        else
                        {
                            System.DateTime date1 = new System.DateTime(
                                UTCDateTime.Date.Year,
                                UTCDateTime.Date.Month,
                                UTCDateTime.Date.Day,
                                UTCDateTime.Time.Hour,
                                UTCDateTime.Time.Minute,
                                UTCDateTime.Time.Second);

                            System.DateTime date2 = System.DateTime.UtcNow;

                            if ((date1 - date2).Seconds > 10)
                            {
                                passed = false;
                                logMessage = logMessage + "Unexpected UTCDateTime" + date1.ToString() + ". ";
                            }
                        }
                    }
                }
                else
                {
                    if (test.SelectNodes("RequestParameters/UTCDateTime").Count != 0)
                    {
                        passed = false;
                        logMessage = logMessage + "No UTCDateTime when it is expected. ";

                    }
                }

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetSystemDateAndTime);
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

        /// <summary>
        /// Test for GetScopes
        /// </summary>
        /// <param name="target">Output data for Normal response</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType GetScopesTest(out Scope[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.GetScopes");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetScopes");
            }

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetScopes]];

                #region SerializeTMP
                //Scope[] dsr = new Scope[1];
                //dsr[0] = new Scope();
                //dsr[0].ScopeDef = ScopeDefinition.Configurable;
                //dsr[0].ScopeItem = "ertert";
                //XmlSerializer serializer = new XmlSerializer(typeof(Scope[]));
                //XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer.Serialize(textWriter, dsr);
                #endregion //SerializeTMP

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Scope[]));
                target = (Scope[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetScopes);
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

        /// <summary>
        /// Test for GetScopes
        /// </summary>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        internal StepType SetScopesTest(out SoapException ex, out int Timeout, string[] Scopes)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("SetScopes");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetScopes]];

                #region Analize request

                //Scopes

                //There is random Scopes in test
                //if (Scopes != null)
                //{
                //    foreach (string i in Scopes)
                //    {
                //        if (test.SelectNodes("RequestParameters[Scopes=\"" + i + "\"]").Count != 1)
                //        {
                //            passed = false;
                //            logMessage = logMessage + "No Scope " + i + ". ";
                //        }
                //    }

                //    if (test.SelectNodes("RequestParameters/Scopes").Count != Scopes.Count())
                //    {
                //        passed = false;
                //        logMessage = logMessage + "Wrong number of Scopes. ";
                //    }
                //}
                //else
                //{
                //    if (test.SelectNodes("RequestParameters/Scopes").Count != 0)
                //    {
                //        passed = false;
                //        logMessage = logMessage + "Wrong number of Scopes. ";
                //    }
                //}
                if (Scopes != null)
                {
                    if (Convert.ToInt32(test.SelectSingleNode("RequestParameters/ScopesNum").InnerText) != Scopes.Count())
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of Scopes. ";
                    }
                }
                else
                {
                    if (test.SelectNodes("RequestParameters/ScopesNum")[0].InnerText != "0")
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of Scopes. ";
                    }
                }

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetScopes);
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

        internal StepType GetSystemLogTest(out SystemLog target, out SoapException ex, out int Timeout, SystemLogType LogType)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = new SystemLog();
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.GetSystemLog");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetSystemLog]];

                #region Analyze request

                //LogType
                CommonCompare.StringCompare("RequestParameters/LogType", "LogType", LogType.ToString(), ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(SystemLog));
                target = (SystemLog)targetObj;


                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetSystemLog);
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

        #endregion //System


        #region Security

        internal StepType GetUsersTest(out User[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";


            //Get step list for command
            XmlNodeList m_testList;
            m_testList = TestCommon.GetStepsForCommand("Device10.GetUsers");

            //TEMP: for backword compatibility
            if (m_testList.Count == 0)
            {
                m_testList = TestCommon.GetStepsForCommand("GetUsers");
            }

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetUsers]];

                #region Change Auth Mode for Future Requests

                TestCommon.GrantPublicInvoke(test);

                #endregion //Change Auth Mode for Future Requests

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(User[]));
                target = (User[])targetObj;

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetUsers);
            }
            else
            {
                throw new SoapException("NO Device10.GetUsers COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        private void DoAddUserCredentials(XmlNode test, User[] User)
        {
            XmlNode needAddUsers = test.SelectSingleNode("AddUsers");
            if (needAddUsers != null && needAddUsers.InnerText == "true")
            {
                foreach (User user in User)
                    TestCommon.AddUserCredentials(user.Username, user.Password);
            }
        }

        internal StepType CreateUsersTest(out SoapException ex, out int Timeout, User[] User)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("CreateUsers");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[CreateUsers]];

                DoAddUserCredentials(test, User);
                #region Change Auth Mode for Future Requests

                TestCommon.GrantPublicInvoke(test);

                #endregion //Change Auth Mode for Future Requests
                #region Analize request

                //Users
                if (User == null)
                {
                    if (test.SelectNodes("RequestParameters/User").Count != 0)
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of User.";
                    }
                }
                else
                {
                    if (test.SelectNodes("RequestParameters/User").Count == User.Count())
                    {
                        foreach (User i in User)
                        {
                            //Username
                            if (test.SelectNodes("RequestParameters/User[Username=\"" + i.Username + "\"]").Count == 0)
                            {
                                passed = false;
                                logMessage = logMessage + "There is unexpected User " + i.Username + ".";
                            }
                            else
                            { 
                                //Password
                                if (test.SelectSingleNode("RequestParameters/User[Username=\"" + i.Username + "\"]/Password").InnerText != i.Password)
                                {
                                    passed = false;
                                    logMessage = logMessage + "There is wrong Password " + i.Password + " for User " + i.Username + ".";
                                }
                                //UserLevel
                                if (test.SelectSingleNode("RequestParameters/User[Username=\"" + i.Username + "\"]/UserLevel").InnerText != i.UserLevel.ToString())
                                {
                                    passed = false;
                                    logMessage = logMessage + "There is wrong UserLevel " + i.UserLevel + " for User " + i.Username + ".";
                                }
                            }
                        }
                    }
                    else
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of User.";
                    }
                }

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, CreateUsers);
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

        internal StepType DeleteUsersTest(out SoapException ex, out int Timeout, string[] Username)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("DeleteUsers");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[DeleteUsers]];

                #region Analize request

                //Users
                if (Username == null)
                {
                    if (test.SelectNodes("RequestParameters/Username").Count != 0)
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of Username.";
                    }
                }
                else
                {
                    if (test.SelectNodes("RequestParameters/Username").Count == Username.Count())
                    {
                        foreach (string i in Username)
                        {
                            //Username
                            if (test.SelectNodes("RequestParameters/Username[text()=\"" + i + "\"]").Count == 0)
                            {
                                passed = false;
                                logMessage = logMessage + "There is unexpected Username " + i + ".";
                            }
                        }
                    }
                    else
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of Username.";
                    }
                }

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, DeleteUsers);
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

        internal StepType SetUserTest(out SoapException ex, out int Timeout, User[] User)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("SetUser");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetUser]];

                DoAddUserCredentials(test, User);

                #region Analize request

                //Users
                if (User == null)
                {
                    if (test.SelectNodes("RequestParameters/User").Count != 0)
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of User.";
                    }
                }
                else
                {
                    if (test.SelectNodes("RequestParameters/User").Count == User.Count())
                    {
                        foreach (User i in User)
                        {
                            //Username
                            if (test.SelectNodes("RequestParameters/User[Username=\"" + i.Username + "\"]").Count == 0)
                            {
                                passed = false;
                                logMessage = logMessage + "There is unexpected User " + i.Username + ".";
                            }
                            else
                            {
                                //Password
                                if (test.SelectSingleNode("RequestParameters/User[Username=\"" + i.Username + "\"]/Password").InnerText != i.Password)
                                {
                                    passed = false;
                                    logMessage = logMessage + "There is wrong Password " + i.Password + " for User " + i.Username + ".";
                                }
                                //UserLevel
                                if (test.SelectSingleNode("RequestParameters/User[Username=\"" + i.Username + "\"]/UserLevel").InnerText != i.UserLevel.ToString())
                                {
                                    passed = false;
                                    logMessage = logMessage + "There is wrong UserLevel " + i.UserLevel + " for User " + i.Username + ".";
                                }
                            }
                        }
                    }
                    else
                    {
                        passed = false;
                        logMessage = logMessage + "Wrong number of User.";
                    }
                }

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetUser);
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

        #endregion //Security




        #region IO

        internal StepType GetRelayOutputsTest(out RelayOutput[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.GetRelayOutputs");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetRelayOutputs]];

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

        internal StepType SetRelayOutputSettingsTest(out SoapException ex, out int Timeout, string RelayOutputToken, RelayOutputSettings Properties)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.SetRelayOutputSettings");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetRelayOutputSettings]];

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

                //Properties
                if (Properties != null)
                { 
                    //Mode
                    CommonCompare.StringCompare("RequestParameters/Properties/Mode", "Properties.Mode", Properties.Mode.ToString(), ref logMessage, ref passed, test);
                    
                    //DelayTime
                    CommonCompare.StringCompare("RequestParameters/Properties/DelayTime", "Properties.DelayTime", Properties.DelayTime, ref logMessage, ref passed, test);

                    //IdleState
                    CommonCompare.StringCompare("RequestParameters/Properties/IdleState", "Properties.IdleState", Properties.IdleState.ToString(), ref logMessage, ref passed, test);
                }
                else
                {
                    passed = false;
                    logMessage = logMessage + "No required tag Properties.";
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
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.SetRelayOutputState");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetRelayOutputState]];

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

        #endregion //IO


        internal StepType GetDiscoveryModeTest(out DiscoveryMode target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = new DiscoveryMode();
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.GetDiscoveryMode");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetDiscoveryMode]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(DiscoveryMode));
                target = (DiscoveryMode)targetObj;

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetDiscoveryMode);
            }
            else
            {
                res = StepType.None;
                Timeout = 0;
                target = new DiscoveryMode();
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType SetDiscoveryModeTest(out SoapException ex, out int Timeout, DiscoveryMode DiscoveryMode)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.SetDiscoveryMode");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetDiscoveryMode]];

                #region Analize request

               //TODO

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetDiscoveryMode);
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

        internal StepType GetSystemSupportInformationTest(out SupportInformation target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = new SupportInformation();
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.GetSystemSupportInformation");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetSystemSupportInformation]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(SupportInformation));
                target = (SupportInformation)targetObj;

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetSystemSupportInformation);
            }
            else
            {
                res = StepType.None;
                Timeout = 0;
                target = new SupportInformation();
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType SystemRebootTest(out string target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.SystemReboot");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SystemReboot]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(string));
                target = (string)targetObj;

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SystemReboot);
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

        internal StepType SetSystemFactoryDefaultTest(out SoapException ex, out int Timeout, FactoryDefaultType FactoryDefault)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.SetSystemFactoryDefault");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetSystemFactoryDefault]];
                #region Change Auth Mode for Future Requests

                TestCommon.GrantPublicInvoke(test);

                #endregion //Change Auth Mode for Future Requests

                #region Analize request

                //TODO

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetSystemFactoryDefault);
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

        internal StepType AddScopesTest(out SoapException ex, out int Timeout, string[] ScopeItem)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.AddScopes");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[AddScopes]];

                #region Analize request

                //TODO

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, AddScopes);
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

        internal StepType RemoveScopesTest(out SoapException ex, out int Timeout, string[] ScopeItem)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.RemoveScopes");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[RemoveScopes]];

                #region Analize request

                //TODO

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, RemoveScopes);
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

        internal StepType GetAccessPolicyTest(out BinaryData target, out SoapException ex, out int Timeout)
        {
            var res = StepType.None;

            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("GetAccessPolicy");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetAccessPolicy]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(BinaryData));
                target = (BinaryData)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetAccessPolicy);
            }
            else
            {
                Timeout = 0;
                target = null;
                ex = null;
                res = StepType.None;
            }

            return res;
        }

        internal StepType StartFirmwareUpgradeTest(out string target, out SoapException ex, out int Timeout, out string UploadDelay, out string ExpectedDownTime)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";
            UploadDelay = "";
            ExpectedDownTime = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.StartFirmwareUpgrade");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[StartFirmwareUpgrade]];

                //Generate response
                object targetObj;
                int special;

                res = TestCommon.GenerateResponseStepTypeNotVoidSpecial(test, out targetObj, out ex, out Timeout, typeof(string), out special);
                target = (string)targetObj;

                TestCommon.StartFileServer(special);

                if (test.SelectNodes("ResponseParametersAdditional/UploadDelay").Count > 0)
                {
                    UploadDelay = test.SelectNodes("ResponseParametersAdditional/UploadDelay")[0].InnerXml;
                }
                if (test.SelectNodes("ResponseParametersAdditional/ExpectedDownTime").Count > 0)
                {
                    ExpectedDownTime = test.SelectNodes("ResponseParametersAdditional/ExpectedDownTime")[0].InnerXml;
                }

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, StartFirmwareUpgrade);
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

        internal StepType GetSystemUrisTest(out SystemLogUri[] target, out SoapException ex, out int Timeout, out string SupportInfoUri, out string SystemBackupUri, out GetSystemUrisResponseExtension Extension)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";
            SupportInfoUri = "";
            SystemBackupUri = "";
            Extension = null;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.GetSystemUris");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetSystemUris]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(SystemLogUri[]));
                target = (SystemLogUri[])targetObj;

                if (test.SelectNodes("ResponseParametersAdditional/SupportInfoUri").Count != 0)
                {
                    SupportInfoUri = test.SelectNodes("ResponseParametersAdditional/SupportInfoUri")[0].InnerXml;
                }

                if (test.SelectNodes("ResponseParametersAdditional/SystemBackupUri").Count != 0)
                {
                    SystemBackupUri = test.SelectNodes("ResponseParametersAdditional/SystemBackupUri")[0].InnerXml;
                }

                if (test.SelectNodes("ResponseParametersAdditional/Extension").Count != 0)
                {
                    Extension = new GetSystemUrisResponseExtension();
                    List<XmlElement> ExtensionList = new List<XmlElement>();
                    foreach (XmlNode xmlNode in test.SelectNodes("ResponseParametersAdditional/Extension")[0].ChildNodes)
                    {
                        if (xmlNode.NodeType == XmlNodeType.Element)
                        {
                            ExtensionList.Add((XmlElement)xmlNode);
                        }
                    }
                    Extension.Any = ExtensionList.ToArray();
                }
                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetSystemUris);
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

        internal StepType SetRemoteUserTest(out SoapException ex, out int Timeout, RemoteUser RemoteUser)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.SetRemoteUser");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[SetRemoteUser]];

                #region Analize request

                //TODO

                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SetRemoteUser);
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

        internal StepType GetRemoteUserTest(out RemoteUser target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.GetRemoteUser");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[GetRemoteUser]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(RemoteUser));
                target = (RemoteUser)targetObj;

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetRemoteUser);
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

        internal StepType StartSystemRestoreTest(out string target, out SoapException ex, out int Timeout, out string ExpectedDownTime)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";
            ExpectedDownTime = "";

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand("Device10.StartSystemRestore");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[StartSystemRestore]];

                //Generate response
                object targetObj;
                int special;
                res = TestCommon.GenerateResponseStepTypeNotVoidSpecial(test, out targetObj, out ex, out Timeout, typeof(string), out special);
                target = (string)targetObj;

                TestCommon.StartFileServer(special);

                ExpectedDownTime = test.SelectNodes("ResponseParametersAdditional/ExpectedDownTime")[0].InnerXml;

                string typeRes = test.SelectNodes("Response")[0].InnerText;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, StartSystemRestore);
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

        internal string SendAuxiliaryCommandTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            return GetCommand<string>("SendAuxiliaryCommand", SendAuxiliaryCommand, validationRequest, out stepType, out ex, out timeout);
        }



        internal StepType SetDynamicDNSTest(out SoapException ex, out int Timeout, DynamicDNSType Type, string Name, string TTL)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "SetDynamicDNS";
            int tmpCommandNumber = SetDNS;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            ////TEMP: for backward compatibility
            //if (m_testList.Count == 0)
            //{
            //    m_testList = TestCommon.GetStepsForCommand(tmpCommandName);
            //}

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                #region Analize request
                
                //Type 
                CommonCompare.StringCompare("RequestParameters/Type", "Type", Type.ToString(), ref logMessage, ref passed, test);
                if (Type != DynamicDNSType.ClientUpdates && Type != DynamicDNSType.NoUpdate && Type !=DynamicDNSType.ServerUpdates)
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected Type " + Type.ToString() + ". ";
                }

                //Name
                if (Name != null)
                {
                    CommonCompare.StringCompare("RequestParameters/Name", "Name", Name.ToString(), ref logMessage, ref passed, test);
                }
                //TTL
                if (TTL != null)
                {
                    CommonCompare.StringCompare("RequestParameters/TTL", "TTL", TTL.ToString(), ref logMessage, ref passed, test);
                }
                
                #endregion //Analize request

                //Generate response
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);

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
    }
}
