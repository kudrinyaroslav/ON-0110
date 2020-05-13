/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;


namespace ONVIF_TestCases
{
    #region Exception Code
    [Serializable]
    
    public class TestCase_ExecuteException : _MessageException
    {
        public TestCase_ExecuteException(string errorMessage)
            : base(errorMessage) { }

        public TestCase_ExecuteException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }
    }


    public class TestCase_StopTest : _MessageException
    {
        public TestCase_StopTest(string errorMessage)
            : base(errorMessage) { }

        public TestCase_StopTest(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }
    }

    #endregion

    /// <summary>
    /// Test Cases used by the test application
    /// </summary>
    public class TestCases_Class
    {
        #region Form Structures

        public delegate void Video_OpenVideoWindow_CallBack();
        public delegate void Video_CloseVideoWindow_CallBack();
        public delegate void Video_SetVideoMode_CallBack(bool testMode);

        public delegate bool Video_SendPOST(out string results, out string packetData);
        public delegate bool Video_PerformGET(out string results, out string response);
        public delegate bool Video_SetCredentials_CallBack(string username, string password);

        public delegate bool RTSPInitVidStream_CallBack(string URI);
        public delegate bool RTSPOptions_CallBack(out string results, out Onvif.RtspResponse response);
        public delegate bool RTSPDescribe_CallBack(out string results, out Onvif.RtspResponse response);
        public delegate bool RTSPSetup_CallBack(out string results, out Onvif.RtspResponse response);
        public delegate bool RTSPPlay_CallBack(out string results, out Onvif.RtspResponse response);
        public delegate bool RTSPSetParameter_CallBack(out string results, out Onvif.RtspResponse response);
        public delegate bool RTSPTeardown_CallBack(out string results, out Onvif.RtspResponse response);


        public delegate void Parameter_UpdateIP(string oldIP, string newIP);

        public struct TestParameters_Type
        {
            public string UserName;
            public string Password;
            private string _Target_IP_Address;
            private string _Mulitcast_IP_Address;
            private int _Port;
            private int _TTL;
            private string _POST_URL;
            private string _MediaServiceAddress;
            public DeviceManagement.Scope[] Scopes;
            public string Temporary_String;
            public object Temporary_Object;
            public string Profile_Token;



            public int TestTimeout;
            public int RebootTime;

            // function pointers used because the ONVIF Streamer control handles the RTSP communication
            public Video_OpenVideoWindow_CallBack Video_OpenWindow;
            public Video_CloseVideoWindow_CallBack Video_CloseWindow;
            public Video_SetVideoMode_CallBack Video_SetMode;
            public Video_SetCredentials_CallBack Video_SetCredentials;

            public Video_PerformGET HTTP_GET;
            public Video_SendPOST HTTP_POST;

            public RTSPInitVidStream_CallBack RTSPInitVidStream;
            public RTSPOptions_CallBack RTSPOptions;
            public RTSPDescribe_CallBack RTSPDescribe;
            public RTSPSetup_CallBack RTSPSetup;
            public RTSPPlay_CallBack RTSPPlay;
            public RTSPSetParameter_CallBack RTSPSetParameter;
            public RTSPTeardown_CallBack RTSPTeardown;

            public Parameter_UpdateIP UpdateIPaddress;

            public Onvif.RtspResponse RTSP_Response;

            public RemoteDiscovery.EndpointReferenceType EPR;

            public TestParameters_Type(string TargetIP, string MulitcastIP, int Port, int TTL)
            {
                UserName = "";
                Password = "";
                _Target_IP_Address = TargetIP;
                _Mulitcast_IP_Address = MulitcastIP;
                _Port = Port;
                _TTL = TTL;
                _POST_URL = "";
                _MediaServiceAddress = "";
                Scopes = new DeviceManagement.Scope[0];
                Temporary_String = "";
                Temporary_Object = null;
                Profile_Token = "";

                TestTimeout = DEFAUTLT_TIMEOUT;
                RebootTime = DEFAULT_REBOOT_TIME;

                Video_OpenWindow = null;
                Video_CloseWindow = null;
                Video_SetMode = null;
                Video_SetCredentials = null;

                HTTP_GET = null;
                HTTP_POST = null;

                RTSPInitVidStream = null;
                RTSPOptions = null;
                RTSPDescribe = null;
                RTSPSetup = null;
                RTSPPlay = null;
                RTSPSetParameter = null;
                RTSPTeardown = null;

                RTSP_Response = null;

                UpdateIPaddress = null;

                EPR = null;
            }

            public string Target_IP
            {
                get
                {
                    return _Target_IP_Address;
                }
                set
                {
                    _Target_IP_Address = value;
                }
            }

            public string Multicast_IP
            {
                get
                {
                    return _Mulitcast_IP_Address;
                }
                set
                {
                    _Mulitcast_IP_Address = value;
                }
            }

            public int Port
            {
                get
                {
                    return _Port;
                }
                set
                {
                    _Port = value;
                }
            }

            public int TTL
            {
                get
                {
                    return _TTL;
                }
                set
                {
                    _TTL = value;
                }
            }

            public string URL
            {
                get { return _POST_URL; }
                set { _POST_URL = value; }
            }

            public string Media_ServiceAddress
            {
                get { return _MediaServiceAddress; }
                set { _MediaServiceAddress = value; }
            }

            public void Clear()
            {
                _MediaServiceAddress = "";
                Scopes = new DeviceManagement.Scope[0];
                Temporary_String = "";
                Temporary_Object = null;
                Profile_Token = "";

                TestTimeout = DEFAUTLT_TIMEOUT;
                RebootTime = DEFAULT_REBOOT_TIME;

                Video_OpenWindow = null;
                Video_CloseWindow = null;
                Video_SetMode = null;
                Video_SetCredentials = null;

                HTTP_GET = null;
                HTTP_POST = null;

                RTSPInitVidStream = null;
                RTSPOptions = null;
                RTSPDescribe = null;
                RTSPSetup = null;
                RTSPPlay = null;
                RTSPSetParameter = null;
                RTSPTeardown = null;

                RTSP_Response = null;

                UpdateIPaddress = null;

                EPR = null;
            }
            
        }

        #endregion

        #region Constants

        private const string TEST_GROUP_TITLE = "ONVIF Tests";
        private const string ONVIF_TEST = "ONVIF_TEST";

        public const int DEFAUTLT_TIMEOUT = 5000;
        public const int DEFAULT_REBOOT_TIME = 30000;

        #endregion

        #region TestSuites
        private const string DEVICE_DISCOVERY_TEST_SUITE = "Device Discovery Test Cases";
        private const string DEVICE_MANAGEMENT_TEST_SUITE = "Device Management Test Cases";
        private const string MEDIA_CONFIGURATION_TEST_SUITE = "Media Configuration Test Cases";
        private const string REAL_TIME_VIEWING_TEST_SUITE = "Real Time Viewing Test Cases";
        #endregion

        #region Discovery Tests
        private const string DISCOVERY_MULTICAST_HELLO = "MULTICAST NVT HELLO MESSAGE";
        private const string DISCOVERY_MULTICAST_HELLO_VALIDATE = "MULTICAST NVT HELLO MESSAGE VALIDATION";
        private const string DISCOVERY_MULTICAST_SCOPE_SEARCH = "MULTICAST NVT SEARCH BASED ON DEVICE SCOPE TYPES";
        private const string DISCOVERY_MULTICAST_SCOPE_SEARCH_OMITTED_DEVICE = "NVT SEARCH WITH OMITTED DEVICE AND SCOPE TYPES";
        private const string DISCOVERY_MULTICAST_SCOPE_SEARCH_INVALID = "NVT RESPONSE TO INVALID SEARCH REQUEST";
        private const string DISCOVERY_UNICAST_SCOPE_SEARCH = "UNICAST NVT SEARCH BASED ON DEVICE SCOPE TYPES";
        private const string DISCOVERY_UNICAST_SCOPE_SEARCH_OMITTED_DEVICE = "UNICAST NVT SEARCH WITH OMITTED DEVICE AND SCOPE TYPES";
        private const string DISCOVERY_UNICAST_SCOPE_SEARCH_INVALID = "UNICAST NVT RESPONSE TO INVALID SEARCH REQUEST";
        private const string DISCOVERY_DEVICE_SCOPES_CONFIGURATION = "NVT DEVICE SCOPES CONFIGURATION";
        private const string DISCOVERY_BYE_MESSAGE = "NVT BYE MESSAGE";
        private const string DISCOVERY_SOAP_FAULT_MESSAGE = "NVT DISCOVERY SOAP FAULT";
        #endregion

        #region Device Tests
        private const string DEVICE_WSDL_URL = "NVT WSDL URL";
        private const string DEVICE_ALL_CAPABILITIES = "NVT ALL CAPABILITIES";
        private const string DEVICE_DEVICE_CAPABILITIES = "NVT DEVICE CAPABILITIES";
        private const string DEVICE_MEDIA_CAPABILITIES = "NVT MEDIA CAPABILITIES";
        private const string DEVICE_SERVICE_CATEGORY_CAPABILITIES = "NVT SERVICE CATEGORY CAPABILITIES";
        private const string DEVICE_SOAP_FAULT_MESSAGE = "NVT DEVICE SOAP FAULT";
        private const string DEVICE_HOSTNAME_CONFIGURATION = "NVT NETWORK COMMAND HOSTNAME CONFIGURATION";
        private const string DEVICE_HOSTNAME_TEST = "NVT NETWORK COMMAND SET HOSTNAME TEST";
        private const string DEVICE_INVALID_HOSTNAME_TEST = "NVT NETWORK COMMAND SET INVALID HOSTNAME TEST";
        private const string DEVICE_DNS_CONFIGURATION = "NVT NETWORK COMMAND DNS CONFIGURATION";
        private const string DEVICE_DNS_TEST = "NVT NETWORK COMMAND SET DNS TEST";
        private const string DEVICE_INVALID_DNS_TEST = "NVT NETWORK COMMAND SET INVALID DNS TEST";
        private const string DEVICE_NTP_CONFIGURATION = "NVT NETWORK COMMAND NTP CONFIGURATION";
        private const string DEVICE_NTP_TEST = "NVT NETWORK COMMAND SET NTP TEST";
        private const string DEVICE_INVALID_IP_NTP_TEST = "NVT NETWORK COMMAND SET INVALID IP NTP TEST";
        private const string DEVICE_INVALID_NAME_NTP_TEST = "NVT NETWORK COMMAND SET INVALID NAME NTP TEST";
        private const string DEVICE_DEVICE_INFORMATION = "NVT SYSTEM COMMAND DEVICE INFORMATION";
        private const string DEVICE_SYSTEM_DATE_AND_TIME = "NVT SYSTEM COMMAND SYSTEM DATE AND TIME";
        private const string DEVICE_SYSTEM_DATE_AND_TIME_TEST = "NVT SYSTEM COMMAND SET SYSTEM DATE AND TIME TEST";
        private const string DEVICE_SYSTEM_DATE_AND_TIME_INVALID_TIMEZONE_TEST = "NVT SYSTEM COMMAND SET SYSTEM DATE AND TIME INVALID TIMEZONE TEST";
        private const string DEVICE_SYSTEM_DATE_AND_TIME_INVALID_DATE_TEST = "NVT SYSTEM COMMAND SET SYSTEM DATE AND TIME INVALID DATE TEST";
        private const string DEVICE_FACTORY_DEFAULT = "NVT SYSTEM COMMAND FACTORY DEFAULT";
        private const string DEVICE_FACTORY_DEFAULT_SOFT = "NVT SYSTEM COMMAND FACTORY DEFAULT SOFT";
        private const string DEVICE_RESET = "NVT SYSTEM COMMAND RESET";
        #endregion

        #region Media Tests
        private const string MEDIA_PROFILE_CONFIGURATION = "NVT MEDIA PROFILE CONFIGURATION";
        private const string MEDIA_DYNAMIC_MEDIA_PROFILE_CONFIGURATION = "NVT DYNAMIC MEDIA PROFILE CONFIGURATION";
        private const string MEDIA_JPEG_VIDEO_ENCODER_CONFIGURATION = "NVT JPEG VIDEO ENCODER CONFIGURATION";
        private const string MEDIA_STREAM_URI__RTP_UDP_UNICAST = "NVT MEDIA STREAM URI – RTP/UDP UNICAST TRANSPORT";
        private const string MEDIA_STREAM_URI__RTP_RTSP_HTTP = "NVT MEDIA STREAM URI – RTP/RTSP/HTTP TRANSPORT";
        private const string MEDIA_SOAP_FAULT_MESSAGE = "NVT MEDIA SOAP FAULT";
        private const string MEDIA_INVALID_TRANSPORT_SOAP_FAULT_MESSAGE = "NVT INVALID TRANSPORT SOAP FAULT MESSAGE";
        #endregion

        #region RTSP Tests
        private const string RTS_RTSP_TCP = "NVT MEDIA CONTROL – RTSP/TCP";
        private const string RTS_RTP_UDP_UNICAST = "NVT MEDIA STREAMING – RTP/UDP UNICAST TRANSPORT";
        private const string RTS_RTP_RTSP_HTTP = "NVT MEDIA STREAMING – RTP/RTSP/HTTP TRANSPORT";
        private const string RTS_RTSP_KEEPALIVE = "NVT MEDIA STREAMING – RTSP KEEPALIVE";
        #endregion

        #region Test Commands

        private const string HELLO_REQ = "Hello Request";
        private const string HELLO_RSP = "Hello Response";
        private const string GET_SCOPES_REQ = "Get Scopes Request";
        private const string GET_SCOPES_RSP = "Get Scopes Response";
        private const string SET_SCOPES_REQ = "Set Scopes Request";
        private const string SET_SCOPES_RSP = "Set Scopes Response";
        private const string ADD_SCOPES_REQ = "Add Scopes Request";
        private const string ADD_SCOPES_RSP = "Add Scopes Response";
        private const string DELETE_SCOPES_REQ = "Delete Scopes Request";
        private const string DELETE_SCOPES_RSP = "Delete Scopes Response";
        private const string PROBE = "Probe Request";
        private const string PROBE_MATCH = "Probe Match Response";
        private const string REBOOT = "Reboot Request";
        private const string REBOOT_RSP = "Reboot Response";
        private const string BYE = "Bye Message";
        
        private const string WSDL_URL_REQ = "Wsdl Url Request";
        private const string WSDL_URL_RSP = "Wsdl Url Response";
        private const string CAPABILITIES_REQ = "Capabilities Request";
        private const string CAPABILITIES_RSP = "Capabilities Response";
        private const string GET_HOSTNAME_REQ = "Get Hostname Request";
        private const string GET_HOSTNAME_RSP = "Get Hostname Response";
        private const string SET_HOSTNAME_REQ = "Set Hostname Request";
        private const string SET_HOSTNAME_RSP = "Set Hostname Response";
        private const string GET_DNS_REQ = "Get DNS Request";
        private const string GET_DNS_RSP = "Get DNS Response";
        private const string SET_DNS_REQ = "Set DNS Request";
        private const string SET_DNS_RSP = "Set DNS Response";
        private const string GET_NTP_REQ = "Get NTP Request";
        private const string GET_NTP_RSP = "Get NTP Response";
        private const string SET_NTP_REQ = "Set NTP Request";
        private const string SET_NTP_RSP = "Set NTP Response";
        private const string DEVICE_INFO_REQ = "Get Device Information Request";
        private const string DEVICE_INFO_RSP = "Get Device Information Response";
        private const string GET_DATE_TIME_REQ = "Get System Date And Time Request";
        private const string GET_DATE_TIME_RSP = "Get System Date And Time Response";
        private const string SET_DATE_TIME_REQ = "Set System Date And Time Request";
        private const string SET_DATE_TIME_RSP = "Set System Date And Time Response";
        private const string SET_FACOTORY_DEFAULT_REQ = "Set System Factory Default Request";
        private const string SET_FACOTORY_DEFAULT_RSP = "Set System Factory Default Response";
       
        private const string GET_PROFILES_REQ = "Get Profiles Request";
        private const string GET_PROFILES_RSP = "Get Profiles Response";
        private const string CREATE_PROFILES_REQ = "Create Profile Request";
        private const string CREATE_PROFILES_RSP = "Create Profile Response";
        private const string DELETE_PROFILES_REQ = "Delete Profile Request";
        private const string DELETE_PROFILES_RSP = "Delete Profile Response";
        
        private const string ADD_VID_SOURCE_REQ = "Add Video Source Configuration Request";
        private const string ADD_VID_SOURCE_RSP = "Add Video Source Configuration Response";
        private const string REMOVE_VID_SOURCE_REQ = "Remove Video Source Configuration Request";
        private const string REMOVE_VID_SOURCE_RSP = "Remove Video Source Configuration Response";

        private const string ADD_VID_ENCODER_REQ = "Add Video Encoder Configuration Request";
        private const string ADD_VID_ENCODER_RSP = "Add Video Encoder Configuration Response";
        private const string REMOVE_VID_ENCODER_REQ = "Remove Video Encoder Configuration Request";
        private const string REMOVE_VID_ENCODER_RSP = "Remove Video Encoder Configuration Response";        
        private const string GET_VID_ENCODER_REQ = "Get Video Encoder Configuration Request";
        private const string GET_VID_ENCODER_RSP = "Get Video Encoder Configuration Response";
        private const string SET_VID_ENCODER_REQ = "Set Video Encoder Configuration Request";
        private const string SET_VID_ENCODER_RSP = "Set Video Encoder Configuration Response";

        private const string GET_STREAM_URI_REQ = "Get Stream Uri Request";
        private const string GET_STREAM_URI_RSP = "Get Stream Uri Response";


        private const string RTSP_OPTIONS = "RTSP OPTIONS";
        private const string RTSP_DESCRIBE = "RTSP DESCRIBE";
        private const string RTSP_SETUP = "RTSP SETUP";
        private const string RTSP_PLAY = "RTSP PLAY";
        private const string RTSP_TEARDOWN = "RTSP TEARDOWN";
        private const string RTSP_SET_PARAMETER = "RTSP SET PARAMETER";
        private const string RTSP_200_OK = "200 OK";

        private const string HTTP_GET = "HTTP GET Request";
        private const string HTTP_POST = "HTTP POST Request";


        #endregion

        #region Soap Fault Messages

        private const string SOAP_FAULT_ = "";

        #endregion

        #region Enumerations

        /// <summary>
        /// Test actions, Perform, Skip, Pass, Fail
        /// </summary>
        public enum TestActions
        {
            NULL,
            Perform,
            Skip
            //Pass, // no longer used
            //Fail // no longer used
        }

        /// <summary>
        /// Test compliance, Optional/Mandatory
        /// </summary>
        public enum TestCompliance
        {
            NULL,
            Must,
            Must_if_Supported,
            Should,
            Should_if_Supported,
            Optional
            //Mandatory,
            //Optional

        }

        /// <summary>
        /// ONVIF test commands
        /// </summary>
        public enum Commands
        {
            CMD_HELLO_REQ,
            CMD_HELLO_RSP,
            CMD_GET_SCOPES_REQ,
            CMD_GET_SCOPES_RSP,
            CMD_SET_SCOPES_REQ,
            CMD_SET_SCOPES_RSP,
            CMD_ADD_SCOPES_REQ,
            CMD_ADD_SCOPES_RSP,
            CMD_DELETE_SCOPES_REQ,
            CMD_DELETE_SCOPES_RSP,
            CMD_PROBE,
            CMD_PROBE_MATCH,
            CMD_REBOOT,
            CMD_REBOOT_RSP,
            CMD_BYE,
            CMD_WSDL_URL_REQ,
            CMD_WSDL_URL_RSP,
            CMD_CAPABILITIES_REQ,
            CMD_CAPABILITIES_RSP,
            CMD_GET_HOSTNAME_REQ,
            CMD_GET_HOSTNAME_RSP,
            CMD_SET_HOSTNAME_REQ,
            CMD_SET_HOSTNAME_RSP,
            CMD_GET_DNS_REQ,
            CMD_GET_DNS_RSP,
            CMD_SET_DNS_REQ,
            CMD_SET_DNS_RSP,
            CMD_GET_NTP_REQ,
            CMD_GET_NTP_RSP,
            CMD_SET_NTP_REQ,
            CMD_SET_NTP_RSP,
            CMD_DEVICE_INFO_REQ,
            CMD_DEVICE_INFO_RSP,
            CMD_GET_DATE_TIME_REQ,
            CMD_GET_DATE_TIME_RSP,
            CMD_SET_DATE_TIME_REQ,
            CMD_SET_DATE_TIME_RSP,
            CMD_SET_FACOTORY_DEFAULT_REQ,
            CMD_SET_FACOTORY_DEFAULT_RSP,
            CMD_GET_PROFILES_REQ,
            CMD_GET_PROFILES_RSP,
            CMD_CREATE_PROFILES_REQ,
            CMD_CREATE_PROFILES_RSP,
            CMD_DELETE_PROFILES_REQ,
            CMD_DELETE_PROFILES_RSP,
            CMD_ADD_VID_SOURCE_REQ,
            CMD_ADD_VID_SOURCE_RSP,
            CMD_REMOVE_VID_SOURCE_REQ,
            CMD_REMOVE_VID_SOURCE_RSP,
            CMD_ADD_VID_ENCODER_REQ,
            CMD_ADD_VID_ENCODER_RSP,
            CMD_REMOVE_VID_ENCODER_REQ,
            CMD_REMOVE_VID_ENCODER_RSP,
            CMD_GET_VID_ENCODER_REQ,
            CMD_GET_VID_ENCODER_RSP,
            CMD_SET_VID_ENCODER_REQ,
            CMD_SET_VID_ENCODER_RSP,
            CMD_GET_STREAM_URI_REQ,
            CMD_GET_STREAM_URI_RSP,
            CMD_RTSP_OPTIONS,
            CMD_RTSP_DESCRIBE,
            CMD_RTSP_SETUP,
            CMD_RTSP_PLAY,
            CMD_RTSP_TEARDOWN,
            CMD_RTSP_SET_PARAMETER,
            CMD_200_OK,
            CMD_HTTP_GET,
            CMD_HTTP_POST
        }

        #endregion

        #region Structures

        private const TestActions DebugAction = TestActions.Skip;

        /// <summary>
        /// Root element of the ONVIF tests
        /// </summary>
        public struct TestGroup_Type
        {
            public string Name; // name of test

            public TestSuite_Type[] Group; // array of test groups
            int groupCount; // number of groups in this test
            private string _TestTime;
            public bool ONVIF_Conformace_Test;

            /// <summary>
            /// Initialize test group
            /// </summary>
            /// <param name="name">Name of test group</param>
            public TestGroup_Type(string name)
            {
                Name = name;
                Group = new TestSuite_Type[0];
                groupCount = 0;
                _TestTime = "";
                ONVIF_Conformace_Test = false;
            }

            /// <summary>
            /// Add a test to this test group
            /// </summary>
            /// <param name="aTest">Test to add</param>
            /// <returns>passed indicator</returns>
            public bool AddTestSuite(TestSuite_Type aTest)
            {
                
                int i;

                // check for null
                if(aTest.Equals(null))
                    return false;

                // check to make sure the test doesn't exist
                for (i = 0; i < Group.Length; i++)
                    if((Group[i].Description != null) && (Group[i].Description.Equals(aTest.Description)))
                        return false;

                if (groupCount >= Group.Length)
                {
                    
                    TestSuite_Type[] tmpTests = new TestSuite_Type[Group.Length];
                    for (i = 0; i < tmpTests.Length; i++)
                        tmpTests[i] = Group[i];

                    Group = new TestSuite_Type[groupCount + 1];

                    for (i = 0; i < tmpTests.Length; i++)
                        Group[i] = tmpTests[i];    
                }

                Group[groupCount] = aTest;
                groupCount++;

                return true;
            }

            /// <summary>
            /// Locate test group index based on name string
            /// </summary>
            /// <param name="name">Group name to locate</param>
            /// <returns>-1 if group not found, or index into Group array</returns>
            public int GetIndex(string name)
            {
                int i;

                for (i = 0; i < groupCount; i++)
                    if((Group[i].Description != null) && (Group[i].Description.Equals(name)))
                    {
                        return i;
                    }

                
                return -1;
            }

            /// <summary>
            /// Initilize test groups for rerun
            /// </summary>
            public void ReRun()
            {
                int x;

                if (groupCount == 0)
                    return;

                
                for (x = 0; x < Group.Length; x++)
                {
                    Group[x].ReTest();
                }

                _TestTime = "";
            }


            public void SetTestStartTime()
            {
                _TestTime = String.Format("Test date - {0} @ {1}\n", System.DateTime.Now.ToShortDateString(), System.DateTime.Now.ToLongTimeString());
            }

            public string TestTime
            {
                get { return _TestTime; }
            }

        }

        /// <summary>
        /// Device, Media, RTSP or other test suites
        /// </summary>
        public struct TestSuite_Type
        {
            public string Description; // test suite description
            public Test_Type[] Tests;    // array of tests
            int testCount; // number of stored tests
            public TestActions Action;  // Skip, Pass, Fail, Execute

            /// <summary>
            /// Test Suite initilization
            /// </summary>
            /// <param name="description">Description string</param>
            public TestSuite_Type(string description)
            {
                Description = description;
                Tests = new Test_Type[0];
                testCount = 0;

#if !DEBUG
                Action = TestActions.Perform;
#else
                Action = DebugAction;
#endif
            }

            /// <summary>
            /// Add a test to this test suite
            /// </summary>
            /// <param name="aTest">Test to add</param>
            /// <returns>Passed indicator</returns>
            public bool AddTest(Test_Type aTest)
            {

                int i;

                // check for null
                if (aTest.Equals(null))
                    return false;

                // check to make sure the test doesn't exist
                for (i = 0; i < Tests.Length; i++)
                    if((Tests[i].Name != null) && (Tests[i].Name.Equals(aTest.Name)) )
                        return false;

                if (testCount >= Tests.Length)
                {

                    Test_Type[] tmpTests = new Test_Type[Tests.Length];
                    for (i = 0; i < tmpTests.Length; i++)
                        tmpTests[i] = Tests[i];

                    Tests = new Test_Type[testCount + 1];

                    for (i = 0; i < tmpTests.Length; i++)
                        Tests[i] = tmpTests[i];
                }

                Tests[testCount] = aTest;
                testCount++;

                return true;
            }

            /// <summary>
            /// Retreive a test by using its test name
            /// </summary>
            /// <param name="name">Name of test to retrieve, will be null if not found</param>
            /// <param name="aTest">Storage for the test to retrieve</param>
            /// <returns>Passed indicator</returns>
            public bool GetTestSuite(string name, out Test_Type? aTest)
            {
                int i;

                for (i = 0; i < Tests.Length; i++)
                    if((Tests[i].Name != null) &&  (Tests[i].Name.Equals(name)))
                    {
                        aTest = Tests[i];
                        return true;
                    }

                aTest = null;
                return false;
            }

            /// <summary>
            /// Locate the test index of a test
            /// </summary>
            /// <param name="name">Name of test to locate</param>
            /// <returns>-1 if test not found, otherwise the indedx into the Tests array</returns>
            public int GetIndex(string name)
            {
                int i;

                for (i = 0; i < Tests.Length; i++)
                    if ((Tests[i].Name != null) && (Tests[i].Name.Equals(name)))
                    {
                        return i;
                    }


                return -1;
            }

            public void NextAction()
            {
                if (this.Action == TestActions.Perform)
                    this.Action = TestActions.Skip;
                else if (this.Action == TestActions.Skip)
                    this.Action = TestActions.Perform;
                //else if (this.Action == TestActions.Pass)
                //    this.Action = TestActions.Fail;
                //else if (this.Action == TestActions.Fail)
                //    this.Action = TestActions.Perform;
            }

            public void SetAction(TestActions newAction)
            {
                this.Action = newAction;
            }

            /// <summary>
            /// Reinitialize the tests for rerun
            /// </summary>
            public void ReTest()
            {
                int y;

                if (testCount == 0)
                    return;
                
                for (y = 0; y < Tests.Length; y++)
                {
                    Tests[y].ResetTest();
                }
            }
        }

        

        /// <summary>
        /// Test information
        /// </summary>        
        public struct Test_Type 
        {
            public object TestObject;

            private string _MessagesReceived;
            private string _MessagesSent;
            private string _TestTime;

            private string[] _XML_MessagesSent;
            private string[] _XML_MessagesReceived;
            private string[] _SoapErrors;
            private string[] _XML_Errors;
            private string _Results;

            public string Number; // test number string, used for identification
            public string Name; // test name string
            public TestActions Action;  // Skip, Pass, Fail, Execute
            public TestCompliance Compliance; // Required/Optional
            bool testPassed;  // Test passed/failed
            bool testComplete; // Test Complete
            private bool testSkipped;            
            int currentStep; // Current step number
            private bool[] stepStatus;

            
                      


            /// <summary>
            /// Test Initialization
            /// </summary>
            /// <param name="number">Test number</param>
            /// <param name="name">Test name</param>
            public Test_Type(string number, string name)
            {
                TestObject = null;

                _MessagesReceived = "";
                _MessagesSent = "";
                _TestTime = String.Format("Test date - {0} @ {1}\n", System.DateTime.Now.ToShortDateString(), System.DateTime.Now.ToLongTimeString());

                _XML_MessagesSent = new string[10];
                _XML_MessagesReceived = new string[10];
                _SoapErrors = new string[10];
                _XML_Errors = new string[10];
                _Results = "";

                Name = name;
                Number = number;                
#if !DEBUG
                Action = TestActions.Perform;
#else
                Action = DebugAction;
#endif
                Compliance = TestCompliance.Must;
                testPassed = true;
                currentStep = 0;
                testComplete = false;
                testSkipped = false; 

                stepStatus = new bool[30];
            }

            /// <summary>
            /// Test Initialization
            /// </summary>
            /// <param name="number">Test number</param>
            /// <param name="name">Test name</param>
            /// <param name="compliance">Required/Optional</param>
            public Test_Type(string number, string name, TestCompliance compliance)
            {
                TestObject = null;

                _MessagesReceived = "";
                _MessagesSent = "";
                _TestTime = String.Format("Test date - {0} @ {1}\n", System.DateTime.Now.ToShortDateString(), System.DateTime.Now.ToLongTimeString());

                _XML_MessagesSent = new string[10];
                _XML_MessagesReceived = new string[10];
                _SoapErrors = new string[10];
                _XML_Errors = new string[10];
                _Results = "";

                Name = name;
                Number = number;

                Compliance = compliance;
#if !DEBUG
                if ((Compliance == TestCompliance.Must) || (Compliance == TestCompliance.Must_if_Supported))
                    Action = TestActions.Perform;
                else
                    Action = TestActions.Skip;
#else
                Action = DebugAction;
#endif
                
                testPassed = true;
                currentStep = 0;
                testComplete = false;
                testSkipped = false; 
                stepStatus = new bool[30];
            }

            public void SetTestStartTime()
            {
                _TestTime = String.Format("Test date - {0} @ {1}\n", System.DateTime.Now.ToShortDateString(), System.DateTime.Now.ToLongTimeString());            
            }

            public string TestTime
            {
                get { return _TestTime; }
            }


            public string MessagesReceived
            {
                get
                {
                    return _MessagesReceived;
                }
                set
                {
                    _MessagesReceived = value;
                    this.XML_MessagesReceived = value;
                }
            }

            public string MessagesSent
            {
                get
                {
                    return _MessagesSent;
                }
                set
                {
                    _MessagesSent = value;
                    this.XML_MessagesSent = value;
                }
            }

            public string XML_MessagesSent
            {
                get
                {
                    if (currentStep >= _XML_MessagesSent.Length)
                    {
                        string[] tmpRes = new string[_XML_MessagesSent.Length];
                        Array.Copy(_XML_MessagesSent, tmpRes, _XML_MessagesSent.Length);
                        _XML_MessagesSent = new string[currentStep + 10];
                        Array.Copy(tmpRes, _XML_MessagesSent, tmpRes.Length);
                        _XML_MessagesSent[currentStep] = "";
                    }

                    if (_XML_MessagesSent[currentStep] == null)
                        _XML_MessagesSent[currentStep] = "";

                    return _XML_MessagesSent[currentStep];
                }

                set
                {
                    if (currentStep >= _XML_MessagesSent.Length)
                    {
                        string[] tmpRes = new string[_XML_MessagesSent.Length];
                        Array.Copy(_XML_MessagesSent, tmpRes, _XML_MessagesSent.Length);
                        _XML_MessagesSent = new string[currentStep + 10];
                        Array.Copy(tmpRes, _XML_MessagesSent, tmpRes.Length);
                       
                    }

                    if (_XML_MessagesSent[currentStep] == null)
                        _XML_MessagesSent[currentStep] = value;
                    else
                        _XML_MessagesSent[currentStep] += System.Environment.NewLine + value;
                }
            }

            public string XML_MessageSent(int index)
            {
                if (index < _XML_MessagesSent.Length)
                    if (_XML_MessagesSent[index] != null)
                        return _XML_MessagesSent[index];

                return "";
            }

            public string XML_MessagesReceived
            {
                get
                {
                    if (currentStep >= _XML_MessagesReceived.Length)
                    {
                        string[] tmpRes = new string[_XML_MessagesReceived.Length];
                        Array.Copy(_XML_MessagesReceived, tmpRes, _XML_MessagesReceived.Length);
                        _XML_MessagesReceived = new string[currentStep + 10];
                        Array.Copy(tmpRes, _XML_MessagesReceived, tmpRes.Length);
                        _XML_MessagesReceived[currentStep] = "";
                    }

                    if (_XML_MessagesReceived[currentStep] == null)
                        _XML_MessagesReceived[currentStep] = "";

                    return _XML_MessagesReceived[currentStep];
                }

                set
                {
                    if (currentStep >= _XML_MessagesReceived.Length)
                    {
                        string[] tmpRes = new string[_XML_MessagesReceived.Length];
                        Array.Copy(_XML_MessagesReceived, tmpRes, _XML_MessagesReceived.Length);
                        _XML_MessagesReceived = new string[currentStep + 10];
                        Array.Copy(tmpRes, _XML_MessagesReceived, tmpRes.Length);

                    }

                    if (_XML_MessagesReceived[currentStep] == null)
                        _XML_MessagesReceived[currentStep] = value;
                    else
                        _XML_MessagesReceived[currentStep] += System.Environment.NewLine + value;
                }
            }

            public string XML_MessageReceived(int index)
            {
                if (index < _XML_MessagesReceived.Length)
                    if (_XML_MessagesReceived[index] != null)
                        return _XML_MessagesReceived[index];

                return "";
            }

            public string SoapErrors
            {
                get
                {
                    if (currentStep >= _SoapErrors.Length)
                    {
                        string[] tmpRes = new string[_SoapErrors.Length];
                        Array.Copy(_SoapErrors, tmpRes, _SoapErrors.Length);
                        _SoapErrors = new string[currentStep + 10];
                        Array.Copy(tmpRes, _SoapErrors, tmpRes.Length);
                        _SoapErrors[currentStep] = "";
                    }

                    if (_SoapErrors[currentStep] == null)
                        _SoapErrors[currentStep] = "";

                    return _SoapErrors[currentStep];
                }

                set
                {
                    if (currentStep >= _SoapErrors.Length)
                    {
                        string[] tmpRes = new string[_SoapErrors.Length];
                        Array.Copy(_SoapErrors, tmpRes, _SoapErrors.Length);
                        _SoapErrors = new string[currentStep + 10];
                        Array.Copy(tmpRes, _SoapErrors, tmpRes.Length);

                    }

                    if ( (_SoapErrors[currentStep] == null) || (_SoapErrors[currentStep] == ""))
                        _SoapErrors[currentStep] = value;
                    else
                        _SoapErrors[currentStep] += System.Environment.NewLine + value;
                }
            }

            public string SoapError(int index)
            {
                if (index < _SoapErrors.Length)
                    if( _SoapErrors[index] != null)
                        return _SoapErrors[index];

                return "";
            }

            public string XML_Errors
            {
                get
                {
                    if (currentStep >= _XML_Errors.Length)
                    {
                        string[] tmpRes = new string[_XML_Errors.Length];
                        Array.Copy(_XML_Errors, tmpRes, _XML_Errors.Length);
                        _XML_Errors = new string[currentStep + 10];
                        Array.Copy(tmpRes, _XML_Errors, tmpRes.Length);
                        _XML_Errors[currentStep] = "";
                    }

                    if (_XML_Errors[currentStep] == null)
                        _XML_Errors[currentStep] = "";

                    return _XML_Errors[currentStep];
                }

                set
                {
                    if (currentStep >= _XML_Errors.Length)
                    {
                        string[] tmpRes = new string[_XML_Errors.Length];
                        Array.Copy(_XML_Errors, tmpRes, _XML_Errors.Length);
                        _XML_Errors = new string[currentStep + 10];
                        Array.Copy(tmpRes, _XML_Errors, tmpRes.Length);

                    }

                    if (_XML_Errors[currentStep] == null)
                        _XML_Errors[currentStep] = value;
                    else
                        _XML_Errors[currentStep] += System.Environment.NewLine + value;
                }
            }

            public string XML_Error(int index)
            {
                if (index < _XML_Errors.Length)
                    if (_XML_Errors[index] != null)
                        return _XML_Errors[index];


                return "";
            }

            public string Results
            {
                get
                {
                    return _Results;
                }

                set
                {
                    _Results = value;
                }
            }

            private void Increase_StepStatusSize()
            {
                bool[] tmp = new bool[stepStatus.Length];
                
                Array.Copy(stepStatus, tmp, tmp.Length);

                stepStatus = new bool[tmp.Length + 1];

                Array.Copy(tmp, stepStatus, tmp.Length);
            }

            public bool StepStatus()
            {
                if (currentStep > 0)
                    return stepStatus[currentStep - 1];
                else
                    return stepStatus[0];
            }

            /// <summary>
            /// Set the test action to the next setting Execute->Skip->Pass->Fail->Execute
            /// </summary>
            public void NextAction()
            {
                if (this.Action == TestActions.Perform)
                    this.Action = TestActions.Skip;
                else if (this.Action == TestActions.Skip)
                    this.Action = TestActions.Perform;
                //else if (this.Action == TestActions.Pass)
                //    this.Action = TestActions.Fail;
                //else if (this.Action == TestActions.Fail)
                //    this.Action = TestActions.Perform;
            }

            /// <summary>
            /// Set test action to specific action
            /// </summary>
            /// <param name="newAction">testActions type</param>
            public void SetAction(TestActions newAction)
            {
                this.Action = newAction;
            }

            /// <summary>
            /// Reset test
            /// </summary>
            public void ResetTest()
            {
                _MessagesReceived = "";
                _MessagesSent = "";
                _TestTime = "";

                _XML_MessagesSent = new string[10];
                _XML_MessagesReceived = new string[10];
                _SoapErrors = new string[10];
                _XML_Errors = new string[10];
                _Results = "";

                testComplete = false;
                testPassed = true;
                testSkipped = false;
                currentStep = 0;
            }

            /// <summary>
            /// Get the current test step
            /// </summary>
            public int CurrentStep
            {
                // the current step is read only
                get
                {
                    return currentStep;
                }
            }

            /// <summary>
            /// Current test step is complete, move to next step.  Assumed that the test step failed
            /// </summary>
            public void StepComplete()
            {
                if (currentStep >= stepStatus.Length)
                    Increase_StepStatusSize();

                stepStatus[currentStep] = false;

                currentStep++;
                testPassed |= false;
            }
            


            /// <summary>
            /// Current test step is complete, indicate passed/failed status, move to next step
            /// </summary>
            /// <param name="passed">Pass/Failed status of the test step</param>
            public void StepComplete(bool passed)
            {
                if (currentStep >= stepStatus.Length)
                    Increase_StepStatusSize();

                if (currentStep < stepStatus.Length) 
                    stepStatus[currentStep] = passed;

                currentStep++;
                testPassed &= passed;
#if true
                // if not in debug mode and  step failes the test fails
                if (!testPassed)
                {
                    testComplete = true;

                }
#endif
            }

            /// <summary>
            /// Get or Set test complete state
            /// </summary>
            public bool TestComplete
            {
                get
                {
                    return testComplete;
                }
                set
                {
                    testComplete = value;
                }

            }

            public bool TestSkipped
            {
                get
                {
                    return testSkipped;
                }
                set
                {
                    testSkipped = value;
                }

            }

            public void TestCompleted()
            {
                testComplete = true;
            }

            /// <summary>
            /// Get test passed value
            /// </summary>
            public bool TestPassed
            {
                // test passed is read only
                get
                {
                    return testPassed;
                }
            }

           
        }

        #endregion

        ONVIF_TestCases.OnvifTests Tester = new OnvifTests();

        /// <summary>
        /// Initilize the tester object
        /// </summary>
        public void InitTester()
        {
            Tester.InitTestMessanger();
        }

        /// <summary>
        /// Build the ONVIF Test Spec 1.0 Test
        /// </summary>
        /// <returns>ONVIF Test Spec 1.0 Test Group</returns>
        public TestGroup_Type SetupDemo()
        {
            TestGroup_Type demoTests = new TestGroup_Type(TEST_GROUP_TITLE);
            int index;

            // add the discovery tests
            demoTests.AddTestSuite(new TestSuite_Type(DEVICE_DISCOVERY_TEST_SUITE));
            index = demoTests.GetIndex(DEVICE_DISCOVERY_TEST_SUITE);
            if (index != -1)
            {                
                demoTests.Group[index].AddTest(new Test_Type("8.1.1", DISCOVERY_MULTICAST_HELLO));     // 3 min            
                demoTests.Group[index].AddTest(new Test_Type("8.1.2", DISCOVERY_MULTICAST_HELLO_VALIDATE)); // 3 min  
                demoTests.Group[index].AddTest(new Test_Type("8.1.3", DISCOVERY_MULTICAST_SCOPE_SEARCH));
                demoTests.Group[index].AddTest(new Test_Type("8.1.3.1", DISCOVERY_MULTICAST_SCOPE_SEARCH_OMITTED_DEVICE));
                demoTests.Group[index].AddTest(new Test_Type("8.1.3.2", DISCOVERY_MULTICAST_SCOPE_SEARCH_INVALID));
                demoTests.Group[index].AddTest(new Test_Type("8.1.4", DISCOVERY_UNICAST_SCOPE_SEARCH));
                demoTests.Group[index].AddTest(new Test_Type("8.1.4.1", DISCOVERY_UNICAST_SCOPE_SEARCH_OMITTED_DEVICE));
                demoTests.Group[index].AddTest(new Test_Type("8.1.4.2", DISCOVERY_UNICAST_SCOPE_SEARCH_INVALID));
                demoTests.Group[index].AddTest(new Test_Type("8.1.5", DISCOVERY_DEVICE_SCOPES_CONFIGURATION));
                demoTests.Group[index].AddTest(new Test_Type("8.1.6", DISCOVERY_BYE_MESSAGE, TestCompliance.Should));
                demoTests.Group[index].AddTest(new Test_Type("8.1.7", DISCOVERY_SOAP_FAULT_MESSAGE, TestCompliance.Optional));
            }


            // add the device tests
            demoTests.AddTestSuite(new TestSuite_Type(DEVICE_MANAGEMENT_TEST_SUITE));
            index = demoTests.GetIndex(DEVICE_MANAGEMENT_TEST_SUITE);
            if (index != -1)
            {
                demoTests.Group[index].AddTest(new Test_Type("8.2.1", DEVICE_WSDL_URL));
                demoTests.Group[index].AddTest(new Test_Type("8.2.2", DEVICE_ALL_CAPABILITIES));
                demoTests.Group[index].AddTest(new Test_Type("8.2.3", DEVICE_DEVICE_CAPABILITIES));
                demoTests.Group[index].AddTest(new Test_Type("8.2.4", DEVICE_MEDIA_CAPABILITIES));
                demoTests.Group[index].AddTest(new Test_Type("8.2.5", DEVICE_SERVICE_CATEGORY_CAPABILITIES, TestCompliance.Must_if_Supported));
                demoTests.Group[index].AddTest(new Test_Type("8.2.6", DEVICE_SOAP_FAULT_MESSAGE));
                demoTests.Group[index].AddTest(new Test_Type("8.2.7", DEVICE_HOSTNAME_CONFIGURATION));
                demoTests.Group[index].AddTest(new Test_Type("8.2.7.1", DEVICE_HOSTNAME_TEST));
                demoTests.Group[index].AddTest(new Test_Type("8.2.7.2", DEVICE_INVALID_HOSTNAME_TEST));
                demoTests.Group[index].AddTest(new Test_Type("8.2.8", DEVICE_DNS_CONFIGURATION));
                demoTests.Group[index].AddTest(new Test_Type("8.2.8.1", DEVICE_DNS_TEST));
                demoTests.Group[index].AddTest(new Test_Type("8.2.8.2", DEVICE_INVALID_DNS_TEST));
                demoTests.Group[index].AddTest(new Test_Type("8.2.9", DEVICE_NTP_CONFIGURATION, TestCompliance.Must_if_Supported));
                demoTests.Group[index].AddTest(new Test_Type("8.2.9.1", DEVICE_NTP_TEST, TestCompliance.Must_if_Supported));
                demoTests.Group[index].AddTest(new Test_Type("8.2.9.2", DEVICE_INVALID_IP_NTP_TEST, TestCompliance.Must_if_Supported));
                /*
                 *      REMOVED AS PER CR_DNS_NTP1.doc
                 *      
                 *      ITEM #2 - Remove DNS tests from NTP test cases
                 */
                //demoTests.Group[index].AddTest(new Test_Type("8.2.9.3", DEVICE_INVALID_NAME_NTP_TEST, TestCompliance.Must_if_Supported));
                demoTests.Group[index].AddTest(new Test_Type("8.2.10", DEVICE_DEVICE_INFORMATION));
                demoTests.Group[index].AddTest(new Test_Type("8.2.11", DEVICE_SYSTEM_DATE_AND_TIME));
                demoTests.Group[index].AddTest(new Test_Type("8.2.11.1", DEVICE_SYSTEM_DATE_AND_TIME_TEST));
                demoTests.Group[index].AddTest(new Test_Type("8.2.11.2", DEVICE_SYSTEM_DATE_AND_TIME_INVALID_TIMEZONE_TEST));
                demoTests.Group[index].AddTest(new Test_Type("8.2.11.3", DEVICE_SYSTEM_DATE_AND_TIME_INVALID_DATE_TEST));
                demoTests.Group[index].AddTest(new Test_Type("8.2.12", DEVICE_FACTORY_DEFAULT)); // 3 min
                demoTests.Group[index].AddTest(new Test_Type("8.2.12.1", DEVICE_FACTORY_DEFAULT_SOFT));
                demoTests.Group[index].AddTest(new Test_Type("8.2.13", DEVICE_RESET));
            }

            // add the media tests
            demoTests.AddTestSuite(new TestSuite_Type(MEDIA_CONFIGURATION_TEST_SUITE));
            index = demoTests.GetIndex(MEDIA_CONFIGURATION_TEST_SUITE);
            if (index != -1)
            {
                demoTests.Group[index].AddTest(new Test_Type("8.3.1", MEDIA_PROFILE_CONFIGURATION));
                demoTests.Group[index].AddTest(new Test_Type("8.3.2", MEDIA_DYNAMIC_MEDIA_PROFILE_CONFIGURATION));
                demoTests.Group[index].AddTest(new Test_Type("8.3.3", MEDIA_JPEG_VIDEO_ENCODER_CONFIGURATION));
                demoTests.Group[index].AddTest(new Test_Type("8.3.4", MEDIA_STREAM_URI__RTP_UDP_UNICAST));
                demoTests.Group[index].AddTest(new Test_Type("8.3.5", MEDIA_STREAM_URI__RTP_RTSP_HTTP));
                demoTests.Group[index].AddTest(new Test_Type("8.3.6", MEDIA_SOAP_FAULT_MESSAGE));
                demoTests.Group[index].AddTest(new Test_Type("8.3.6.1", MEDIA_INVALID_TRANSPORT_SOAP_FAULT_MESSAGE));
            }

            // add the RTSP tests
            demoTests.AddTestSuite(new TestSuite_Type(REAL_TIME_VIEWING_TEST_SUITE));
            index = demoTests.GetIndex(REAL_TIME_VIEWING_TEST_SUITE);
            if (index != -1)
            {
                demoTests.Group[index].AddTest(new Test_Type("8.4.1", RTS_RTSP_TCP));
                demoTests.Group[index].AddTest(new Test_Type("8.4.2", RTS_RTP_UDP_UNICAST));
                demoTests.Group[index].AddTest(new Test_Type("8.4.3", RTS_RTP_RTSP_HTTP));
                demoTests.Group[index].AddTest(new Test_Type("8.4.4", RTS_RTSP_KEEPALIVE));
            }

            demoTests.ONVIF_Conformace_Test = true;
            return demoTests;
        }

        /// <summary>
        /// When parsing the user specified tests, use the element
        /// name to identify which test is supposed to be run.
        /// </summary>
        /// <param name="elementName">The element name parsed from the user tests</param>
        /// <returns>Test tile that corresponds to the element name</returns>
        private string GetTitleFromElementString(string elementName)
        {
            switch (elementName)
            {
                case "DISCOVERY_MULTICAST_HELLO":
                    return DISCOVERY_MULTICAST_HELLO;
                case "DISCOVERY_MULTICAST_HELLO_VALIDATE":
                    return DISCOVERY_MULTICAST_HELLO_VALIDATE;
                case "DISCOVERY_MULTICAST_SCOPE_SEARCH":
                    return DISCOVERY_MULTICAST_SCOPE_SEARCH;
                case "DISCOVERY_MULTICAST_SCOPE_SEARCH_OMITTED_DEVICE":
                    return DISCOVERY_MULTICAST_SCOPE_SEARCH_OMITTED_DEVICE;
                case "DISCOVERY_MULTICAST_SCOPE_SEARCH_INVALID":
                    return DISCOVERY_MULTICAST_SCOPE_SEARCH_INVALID;
                case "DISCOVERY_UNICAST_SCOPE_SEARCH":
                    return DISCOVERY_UNICAST_SCOPE_SEARCH;
                case "DISCOVERY_UNICAST_SCOPE_SEARCH_OMITTED_DEVICE":
                    return DISCOVERY_UNICAST_SCOPE_SEARCH_OMITTED_DEVICE;
                case "DISCOVERY_UNICAST_SCOPE_SEARCH_INVALID":
                    return DISCOVERY_UNICAST_SCOPE_SEARCH_INVALID;
                case "DISCOVERY_DEVICE_SCOPES_CONFIGURATION":
                    return DISCOVERY_DEVICE_SCOPES_CONFIGURATION;
                case "DISCOVERY_BYE_MESSAGE":
                    return DISCOVERY_BYE_MESSAGE;
                case "DISCOVERY_SOAP_FAULT_MESSAGE":
                    return DISCOVERY_SOAP_FAULT_MESSAGE;

                case "DEVICE_WSDL_URL":
                    return DEVICE_WSDL_URL;
                case "DEVICE_ALL_CAPABILITIES":
                    return DEVICE_ALL_CAPABILITIES;
                case "DEVICE_DEVICE_CAPABILITIES":
                    return DEVICE_DEVICE_CAPABILITIES;
                case "DEVICE_MEDIA_CAPABILITIES":
                    return DEVICE_MEDIA_CAPABILITIES;
                case "DEVICE_SERVICE_CATEGORY_CAPABILITIES":
                    return DEVICE_SERVICE_CATEGORY_CAPABILITIES;
                case "DEVICE_SOAP_FAULT_MESSAGE":
                    return DEVICE_SOAP_FAULT_MESSAGE;
                case "DEVICE_HOSTNAME_CONFIGURATION":
                    return DEVICE_HOSTNAME_CONFIGURATION;
                case "DEVICE_HOSTNAME_TEST":
                    return DEVICE_HOSTNAME_TEST;
                case "DEVICE_INVALID_HOSTNAME_TEST":
                    return DEVICE_INVALID_HOSTNAME_TEST;
                case "DEVICE_DNS_CONFIGURATION":
                    return DEVICE_DNS_CONFIGURATION;
                case "DEVICE_DNS_TEST":
                    return DEVICE_DNS_TEST;
                case "DEVICE_INVALID_DNS_TEST":
                    return DEVICE_INVALID_DNS_TEST;
                case "DEVICE_NTP_CONFIGURATION":
                    return DEVICE_NTP_CONFIGURATION;
                case "DEVICE_NTP_TEST":
                    return DEVICE_NTP_TEST;
                case "DEVICE_INVALID_IP_NTP_TEST":
                    return DEVICE_INVALID_IP_NTP_TEST;
                case "DEVICE_INVALID_NAME_NTP_TEST":
                    return DEVICE_INVALID_NAME_NTP_TEST;
                case "DEVICE_DEVICE_INFORMATION":
                    return DEVICE_DEVICE_INFORMATION;
                case "DEVICE_SYSTEM_DATE_AND_TIME":
                    return DEVICE_SYSTEM_DATE_AND_TIME;
                case "DEVICE_SYSTEM_DATE_AND_TIME_TEST":
                    return DEVICE_SYSTEM_DATE_AND_TIME_TEST;
                case "DEVICE_SYSTEM_DATE_AND_TIME_INVALID_TIMEZONE_TEST":
                    return DEVICE_SYSTEM_DATE_AND_TIME_INVALID_TIMEZONE_TEST;
                case "DEVICE_SYSTEM_DATE_AND_TIME_INVALID_DATE_TEST":
                    return DEVICE_SYSTEM_DATE_AND_TIME_INVALID_DATE_TEST;
                case "DEVICE_FACTORY_DEFAULT":
                    return DEVICE_FACTORY_DEFAULT;
                case "DEVICE_FACTORY_DEFAULT_SOFT":
                    return DEVICE_FACTORY_DEFAULT_SOFT;
                case "DEVICE_RESET":
                    return DEVICE_RESET;

                case "MEDIA_PROFILE_CONFIGURATION":
                    return MEDIA_PROFILE_CONFIGURATION;
                case "MEDIA_DYNAMIC_MEDIA_PROFILE_CONFIGURATION":
                    return MEDIA_DYNAMIC_MEDIA_PROFILE_CONFIGURATION;
                case "MEDIA_JPEG_VIDEO_ENCODER_CONFIGURATION":
                    return MEDIA_JPEG_VIDEO_ENCODER_CONFIGURATION;
                case "MEDIA_STREAM_URI__RTP_UDP_UNICAST":
                    return MEDIA_STREAM_URI__RTP_UDP_UNICAST;
                case "MEDIA_STREAM_URI__RTP_RTSP_HTTP":
                    return MEDIA_STREAM_URI__RTP_RTSP_HTTP;
                case "MEDIA_SOAP_FAULT_MESSAGE":
                    return MEDIA_SOAP_FAULT_MESSAGE;
                case "MEDIA_INVALID_TRANSPORT_SOAP_FAULT_MESSAGE":
                    return MEDIA_INVALID_TRANSPORT_SOAP_FAULT_MESSAGE;

                case "RTS_RTSP_TCP":
                    return RTS_RTSP_TCP;
                case "RTS_RTP_UDP_UNICAST":
                    return RTS_RTP_UDP_UNICAST;
                case "RTS_RTP_RTSP_HTTP":
                    return RTS_RTP_RTSP_HTTP;
                case "RTS_RTSP_KEEPALIVE":
                    return RTS_RTSP_KEEPALIVE;
            }

            throw new TestCase_ExecuteException("Missed a test - " + elementName);

            //return "";
        }

        /// <summary>
        /// Parse XML test file and build test group
        /// </summary>
        /// <param name="Tests">Nullible test group</param>
        /// <param name="fileName">URL of the XML test file</param>
        /// <returns>passed indicator</returns>
        public bool ParseTest(out TestGroup_Type? Tests, string fileName)
        {
            int i, index;
            string testName = "";
            string groupName = "";

            TestSchema_Class.ONVIF_TEST O_Test = new TestSchema_Class.ONVIF_TEST();

            string s = "";
            

            ONVIF_TestCases.TestMessages TM = new TestMessages();

            try
            {
                using (StreamReader rdr = File.OpenText(fileName))
                {
                    s = rdr.ReadToEnd();
                }

                O_Test = (TestSchema_Class.ONVIF_TEST)TM.FromXml(s, typeof(TestSchema_Class.ONVIF_TEST));
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                Tests = null;
                return false;

            }
            


            if (O_Test.Name == null)
                O_Test.Name = TEST_GROUP_TITLE;

            TestGroup_Type tmpTests = new TestGroup_Type(O_Test.Name);
            i = 1;
            foreach(TestSchema_Class.Test schemaTest in O_Test.Test)
            {
                if ((schemaTest.Group == null) || (schemaTest.Group == ""))
                {
                    string[] tmpStringArray = (schemaTest.ItemElementName.ToString()).Split( new char[] {'_'});

                    groupName = tmpStringArray[0];

                }

                
                index = tmpTests.GetIndex(groupName);
                if (index < 0)
                {
                    tmpTests.AddTestSuite(new TestSuite_Type(groupName));
                    index = tmpTests.GetIndex(groupName);
                }

                testName = GetTitleFromElementString(schemaTest.ItemElementName.ToString());


                tmpTests.Group[index].AddTest(new Test_Type((index + 1).ToString() + "." + (tmpTests.Group[index].Tests.Length + 1).ToString(), testName));


            }

            // set the out variable to the temporary one
            Tests = tmpTests;
            return true;
        }

        /// <summary>
        /// Look at the test and return a true/false value if the test is to be skipped or run
        /// If the test is to be skipped update the results string as such.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        private bool TestRequiresAction(ref ONVIF_TestCases.TestCases_Class.Test_Type test, ref string results)
        {
            if (test.Action == TestCases_Class.TestActions.Skip)
            {
                results += OnvifTests.STEP_SPACING + "Test Marked as Skipped" + Environment.NewLine;
                results += OnvifTests.STEP_MSG_SPACING + "Skipping all test steps" + Environment.NewLine;
                results += OnvifTests.STEP_SPACING + "Test complete" + Environment.NewLine;
                results += "Test SKIPPED" + Environment.NewLine;
                test.TestComplete = true;
                test.TestSkipped = true;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Run specified test
        /// </summary>
        /// <param name="test">Test to run</param>
        /// <returns>Test results</returns>             
        public string RunTest(ref TestCases_Class.Test_Type test, ref TestParameters_Type TestParameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            string results = "";
            

            if (!TestRequiresAction(ref test, ref results))
                return results;

            // call the proper test function based on the test name
            switch (test.Name)
            {

                case DISCOVERY_MULTICAST_HELLO:
                    results += Tester.DISCOVERY_MULTICAST_HELLO(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DISCOVERY_MULTICAST_HELLO_VALIDATE:
                    results += Tester.DISCOVERY_MULTICAST_HELLO_VALIDATE(ref test, ref TestParameters, ref NetworkInterface);
                    break;
                case DISCOVERY_MULTICAST_SCOPE_SEARCH:
                    results += Tester.DISCOVERY_MULTICAST_SCOPE_SEARCH(ref test, ref TestParameters, ref NetworkInterface);
                    break;
                case DISCOVERY_MULTICAST_SCOPE_SEARCH_OMITTED_DEVICE:
                    results += Tester.DISCOVERY_MULTICAST_SCOPE_SEARCH_OMITTED_DEVICE(ref test, ref TestParameters, ref NetworkInterface);
                    break;
                case DISCOVERY_MULTICAST_SCOPE_SEARCH_INVALID:
                    results += Tester.DISCOVERY_MULTICAST_SCOPE_SEARCH_INVALID(ref test, ref TestParameters, ref NetworkInterface);
                    break;
                case DISCOVERY_UNICAST_SCOPE_SEARCH:
                    results += Tester.DISCOVERY_UNICAST_SCOPE_SEARCH(ref test, ref TestParameters, ref NetworkInterface);
                    break;
                case DISCOVERY_UNICAST_SCOPE_SEARCH_OMITTED_DEVICE:
                    results += Tester.DISCOVERY_UNICAST_SCOPE_SEARCH_OMITTED_DEVICE(ref test, ref TestParameters, ref NetworkInterface);
                    break;
                case DISCOVERY_UNICAST_SCOPE_SEARCH_INVALID:
                    results += Tester.DISCOVERY_UNICAST_SCOPE_SEARCH_INVALID(ref test, ref TestParameters, ref NetworkInterface);
                    break;
                case DISCOVERY_DEVICE_SCOPES_CONFIGURATION:
                    results += Tester.DISCOVERY_DEVICE_SCOPES_CONFIGURATION(ref test, ref TestParameters, ref NetworkInterface);
                    break;
                case DISCOVERY_BYE_MESSAGE:
                    results += Tester.DISCOVERY_BYE_MESSAGE(ref test, ref TestParameters, ref NetworkInterface);
                    break;
                case DISCOVERY_SOAP_FAULT_MESSAGE:
                    results += Tester.DISCOVERY_SOAP_FAULT_MESSAGE(ref test, ref TestParameters, ref NetworkInterface);
                    break;


                case DEVICE_WSDL_URL:
                    results += Tester.DEVICE_WSDL_URL(ref test, ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_ALL_CAPABILITIES:
                    results += Tester.DEVICE_ALL_CAPABILITIES(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_DEVICE_CAPABILITIES:
                    results += Tester.DEVICE_DEVICE_CAPABILITIES(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_MEDIA_CAPABILITIES:
                    results += Tester.DEVICE_MEDIA_CAPABILITIES(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_SERVICE_CATEGORY_CAPABILITIES:
                    results += Tester.DEVICE_SERVICE_CATEGORY_CAPABILITIES(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_SOAP_FAULT_MESSAGE:
                    results += Tester.DEVICE_SOAP_FAULT_MESSAGE(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_HOSTNAME_CONFIGURATION:
                    results += Tester.DEVICE_HOSTNAME_CONFIGURATION(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_HOSTNAME_TEST:
                    results += Tester.DEVICE_HOSTNAME_TEST(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_INVALID_HOSTNAME_TEST:
                    results += Tester.DEVICE_INVALID_HOSTNAME_TEST(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_DNS_CONFIGURATION:
                    results += Tester.DEVICE_DNS_CONFIGURATION(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_DNS_TEST:
                    results += Tester.DEVICE_DNS_TEST(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_INVALID_DNS_TEST:
                    results += Tester.DEVICE_INVALID_DNS_TEST(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_NTP_CONFIGURATION:
                    results += Tester.DEVICE_NTP_CONFIGURATION(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_NTP_TEST:
                    results += Tester.DEVICE_NTP_TEST(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_INVALID_IP_NTP_TEST:
                    results += Tester.DEVICE_INVALID_IP_NTP_TEST(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_INVALID_NAME_NTP_TEST:
                    results += Tester.DEVICE_INVALID_NAME_NTP_TEST(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_DEVICE_INFORMATION:
                    results += Tester.DEVICE_DEVICE_INFORMATION(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_SYSTEM_DATE_AND_TIME:
                    results += Tester.DEVICE_SYSTEM_DATE_AND_TIME(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_SYSTEM_DATE_AND_TIME_TEST:
                    results += Tester.DEVICE_SYSTEM_DATE_AND_TIME_TEST(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_SYSTEM_DATE_AND_TIME_INVALID_TIMEZONE_TEST:
                    results += Tester.DEVICE_SYSTEM_DATE_AND_TIME_INVALID_TIMEZONE_TEST(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_SYSTEM_DATE_AND_TIME_INVALID_DATE_TEST:
                    results += Tester.DEVICE_SYSTEM_DATE_AND_TIME_INVALID_DATE_TEST(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_FACTORY_DEFAULT:
                    results += Tester.DEVICE_FACTORY_DEFAULT(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_FACTORY_DEFAULT_SOFT:
                    results += Tester.DEVICE_FACTORY_DEFAULT_SOFT(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case DEVICE_RESET:
                    results += Tester.DEVICE_RESET(ref test,ref TestParameters, ref NetworkInterface);
                    break;


                case MEDIA_PROFILE_CONFIGURATION:
                    results += Tester.MEDIA_PROFILE_CONFIGURATION(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case MEDIA_DYNAMIC_MEDIA_PROFILE_CONFIGURATION:
                    results += Tester.MEDIA_DYNAMIC_MEDIA_PROFILE_CONFIGURATION(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case MEDIA_JPEG_VIDEO_ENCODER_CONFIGURATION:
                    results += Tester.MEDIA_JPEG_VIDEO_ENCODER_CONFIGURATION(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case MEDIA_STREAM_URI__RTP_UDP_UNICAST:
                    results += Tester.MEDIA_STREAM_URI__RTP_UDP_UNICAST(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case MEDIA_STREAM_URI__RTP_RTSP_HTTP:
                    results += Tester.MEDIA_STREAM_URI__RTP_RTSP_HTTP(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case MEDIA_SOAP_FAULT_MESSAGE:
                    results += Tester.MEDIA_SOAP_FAULT_MESSAGE(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case MEDIA_INVALID_TRANSPORT_SOAP_FAULT_MESSAGE:
                    results += Tester.MEDIA_INVALID_TRANSPORT_SOAP_FAULT_MESSAGE(ref test,ref TestParameters, ref NetworkInterface);
                    break;


                case RTS_RTSP_TCP:
                    results += Tester.REAL_TIME_STREAMING_RTSP_TCP(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case RTS_RTP_UDP_UNICAST:
                    results += Tester.REAL_TIME_STREAMING_RTP_UDP_UNICAST(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case RTS_RTP_RTSP_HTTP:
                    results += Tester.REAL_TIME_STREAMING_RTP_RTSP_HTTP(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                case RTS_RTSP_KEEPALIVE:
                    results += Tester.REAL_TIME_STREAMING_RTSP_KEEPALIVE(ref test,ref TestParameters, ref NetworkInterface);
                    break;
                

                default:
                    results += "Command - " + test.Name + " UNKNOWN";
                    test.TestComplete = true;
                    return results;
            }

            // if the test is still marked as passed then the step passed, otherwise it must have failed
            if (!test.TestComplete)
            {
                if (test.StepStatus())
                    results += ONVIF_TestCases.OnvifTests.STEP_MSG_SPACING + "Step Passed" + Environment.NewLine;
                else
                    results += ONVIF_TestCases.OnvifTests.STEP_MSG_SPACING + "Step Failed" + Environment.NewLine;
            }
            return results;
        }


    }
}
