using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml.Serialization;
using System.Net;
using Digest.Samples;
using CameraWebService.FileServer;

namespace DUT.CameraWebService
{
    /// <summary>
    /// Define possible types of test
    /// </summary>
    public enum StepType
    {
        Normal,
        NoResponse,
        Fault,
        None
    }

    public class NotificationRec
    {
        public System.DateTime Time = System.DateTime.UtcNow;
        public string Address = "";
        public string Request = "";
        public int Timeout = 5000;
    }

    /// <summary>
    /// Define possible fault namespace error
    /// </summary>
    public enum FaultNamespaceErrorType
    {
        None,
        SubCode,
        SubSubCode
    }

    /// <summary>
    /// Define possible protocols
    /// </summary>
    public enum ProtocolType
    {
        HTTP,
        HTTPS,
    }

    public class TestCommon
    {
        #region Const

        /// <summary>
        /// Relative path to test suit (from CodeBase)
        /// </summary>
        public static string pathToTestSuit;
        /// <summary>
        /// Test ID (load all tests if empty)
        /// </summary>
        public static string testID = null;
        /// <summary>
        /// Test Result Log
        /// </summary>
        public static string testResult = "NONE";
        /// <summary>
        /// Test Summary
        /// </summary>
        public static bool testSummaryResult = true;
        /// <summary>
        /// HTTPS
        /// </summary>
        public static string protocolHTTPS = "https";
        /// <summary>
        /// HTTP
        /// </summary>
        public static string protocolHTTP = "http";
        /// <summary>
        /// Test Summary
        /// </summary>
        public static bool testStepsCompleted = false;
        /// <summary>
        /// Previous Step Number
        /// </summary>
        public static int previousStepNumber = 0;
        /// <summary>
        /// Path to log file
        /// </summary>
        public static string pathToLog = "c:\\ONVIFTestLog.txt";

        public string LastNotificationAddress = "";

        public static string Title { get; set; }

        #endregion //Const

        #region Members

        /// <summary>
        /// XML document with test scecification
        /// </summary>
        public XmlDocument m_testSuitXML = null;
        /// <summary>
        /// Complex Test Flag
        /// </summary>
        public bool m_complexTest = false;
        /// <summary>
        /// Timeout for no response emulation
        /// </summary>
        public int m_timeOut = 10000;

        public int m_stepCount = 0;

        //public string m_HostAndPort = null;

        private ProtocolType httpType = ProtocolType.HTTP;

        public ProtocolType HttpType
        {
            get { return httpType; }
            set { httpType = value; }
        }

        public string HttpTypeString
        {
            get {
                if (httpType == ProtocolType.HTTPS)
                { return "https"; }
                else
                { return "http"; }
            }
        }

        public string EventsUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceEvents10/EventService.asmx"; }
        }

        public string DeviceUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceDevice10/DeviceServiceFake.asmx"; }
        }

        public string DeviceIO10Uri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceDeviceIO10/DeviceIOService10.asmx"; }
        }

        public string MadiaUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceMedia10/MediaService.asmx"; }
        }

        public string ImagingUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceImaging10/ImagingService10.asmx"; }
        }

        public string Imaging20Uri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceImaging20/ImagingService20.asmx"; }
        }

        public string PTZUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServicePTZ20/PtzService.asmx"; }
        }

        public string SearchUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceSearch10/SearchService.asmx"; }
        }

        public string RecordingUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceRecording10/RecordingService.asmx"; }
        }

        public string SubscriptionManagerServiceUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceEvents10/SubscriptionManagerService.asmx"; }
        }

        public string PullpointSubscriptionServiceUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceEvents10/PullpointSubscriptionService.asmx"; }
        }

        public string PullpointSubscriptionService2Uri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceEvents10/PullPointSubscribtionService2.asmx"; }
        }

        public string PACSServiceUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServicePACS10/PACSService.asmx"; }
        }

        public string DoorServiceUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceDoor10/DoorService.asmx"; }
        }

        public string PACS11ServiceUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServicePACS11/PACSService.asmx"; }
        }

        public string PACS12ServiceUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServicePACS12/PACSService.asmx"; }
        }

        public string Door11ServiceUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceDoor11/DoorService.asmx"; }
        }

        public string Door12ServiceUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceDoor12/DoorService.asmx"; }
        }

        public string ReceiverUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceReceiver10/ReceiverService.asmx"; }
        }

        public string ReplayUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceReplay10/ReplayService.asmx"; }
        }

        public string AdvancedSecurityUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceAdvancedSecurity10/AdvancedSecurityService.asmx"; }
        }

        public string Media2Uri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceMedia210/Media2Service.asmx"; }
        }

        public string Media2SVCUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceMedia2SVC/Media2Service.svc"; }
        }

        public string AccessRulesUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceAccessRules10/AccessRulesService.asmx"; }
        }

        public string CredentialUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceCredential10/CredentialService.asmx"; }
        }

        public string ScheduleUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceSchedule10/ServiceSchedule.asmx"; }
        }

        public string PCS10Binary1Uri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/Common/csr.bin"; }
        }

        public string PCS10Binary2Uri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/Common/csr1024.bin"; }
        }

        public string PCS10Binary3Uri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/Common/csr1024InvalidSignature.bin"; }
        }

        public string PCS10Binary4Uri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/Common/csr2048.bin"; }
        }

        public string PCS10Binary5Uri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/Common/csr3072.bin"; }
        }

        public string TLSCertificate1Uri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/Common/fido2.pfx"; }
        }

        public string PullPointSubscribtionHelpUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/Help/EventServiceHelp.txt"; }
        }

        public string AnalyticsEngineHelpUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/Help/AnalyticsServiceHelp.txt"; }
        }

        public string AnalyticsEngineUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceAnalytics20/AnalyticsEngineService.asmx"; }
        }

        public string ProvisioningUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceProvisioning10/ProvisioningService.asmx"; }
        }

        public string ThermalUri
        {
            get { return HttpTypeString + "://" + HttpContext.Current.Request.Url.Authority + "/ServiceThermal10/ThermalService.asmx"; }
        }

        public string[] TestList
        {
            get
            {
                XmlNodeList list;
                string[] str;
                if (m_complexTest)
                {
                    str = new string[1];
                    str[0] = m_testSuitXML.SelectSingleNode("/ComplexTest/@id").Value;
                }
                else
                {
                    if (m_testSuitXML != null)
                    {
                        list = m_testSuitXML.SelectNodes("/TestSuit/Test");
                        str = new string[list.Count*3+2];
                        int j = 0;
                        foreach (XmlNode i in list)
                        {
                            str[j] = i.Attributes["ID"].Value + " " + i.SelectSingleNode("Name").InnerXml;
                            j++;
                        }
                        str[j] = "";
                        j++;
                        foreach (XmlNode i in list)
                        {
                            str[j] = i.Attributes["ID"].Value;
                            j++;
                        }
                        str[j] = "";
                        j++;
                        foreach (XmlNode i in list)
                        {
                            str[j] = i.SelectSingleNode("Name").InnerXml;
                            j++;
                        }
                    }
                    else
                    {
                        str = null;
                    }
                }

                return str;
            }
        }

        public List<string> TestList2
        {
            get
            {
                XmlNodeList list;
                List<string> str = new List<string>();
                if (m_complexTest)
                {
                    str.Add(m_testSuitXML.SelectSingleNode("/ComplexTest/@id").Value);
                }
                else
                {
                    list = m_testSuitXML.SelectNodes("/TestSuit/Test");
                    int j = 0;
                    foreach (XmlNode i in list)
                    {
                        str.Add(i.Attributes["ID"].Value);
                        j++;
                    }
                }

                return str;
            }
        }

        #endregion //Members

        public static byte[] ReadBinary(string url)
        {
            byte[] result = null;

            WebRequest request = (WebRequest)WebRequest.Create(url);
            WebResponse response = (WebResponse)request.GetResponse();
            Stream stm = response.GetResponseStream();
            BinaryReader br = new BinaryReader(stm);

            result = br.ReadBytes((int)response.ContentLength);

            return result;
        }

        /// <summary>
        /// Load test suit from file and initialize general options
        /// </summary>
        /// <remarks>05.10.12: this method is really used.</remarks>
        public void LoadTestSuit(Stream stream)
        {
            XmlReader reader = null;
            try
            {
                
                reader = XmlReader.Create(stream);
                m_testSuitXML = new XmlDocument();
                m_testSuitXML.Load(reader);

                //If complex test
                if (m_testSuitXML.SelectSingleNode("/ComplexTest") != null)
                {
                    //Initialization of timeout
                    m_timeOut = Convert.ToInt32(m_testSuitXML.SelectNodes("/ComplexTest/Parameters/TimeOut")[0].InnerText);

                    //
                    XmlNodeList complexTest = m_testSuitXML.SelectNodes("/ComplexTest/Include");
                    XmlDocument tmp = new XmlDocument();
                    //Get location to test file
                    string location = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                    location = System.IO.Path.GetDirectoryName(location);
                    string pathToTestSuit;
                    XmlNode tmpNode;
                    for (int i = 0; i < complexTest.Count; i++)
                    {
                        pathToTestSuit = System.IO.Path.Combine(location, complexTest[i].Attributes.GetNamedItem("file").Value);
                        reader = XmlReader.Create(pathToTestSuit);
                        tmp.Load(reader);
                        tmpNode = m_testSuitXML.ImportNode(tmp.SelectSingleNode("/TestSuit/Test[@ID=\"" + complexTest[i].Attributes.GetNamedItem("testId").Value + "\"]"), true);
                        m_testSuitXML.DocumentElement.AppendChild(tmpNode);
                    }
                    m_complexTest = true;

                }
                else
                {
                    //Initialization of timeout
                    m_timeOut = Convert.ToInt32(m_testSuitXML.SelectNodes("/TestSuit/Parameters/TimeOut")[0].InnerText);
                    m_complexTest = false;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        /// <summary>
        /// Load test suit from file and initialize general options
        /// </summary>
        public void LoadTestSuit()
        {
            XmlReader reader = null;
            string result = null;
            try
            {
                //Get location to test file
                string location = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                location = System.IO.Path.GetDirectoryName(location);
                location = System.IO.Path.Combine(location, TestCommon.pathToTestSuit);

                //Load test file
                reader = XmlReader.Create(location);
                m_testSuitXML = new XmlDocument();
                m_testSuitXML.Load(reader);

                //If complex test
                if (m_testSuitXML.SelectSingleNode("/ComplexTest") != null)
                {
                    //Initialization of timeout
                    m_timeOut = Convert.ToInt32(m_testSuitXML.SelectNodes("/ComplexTest/Parameters/TimeOut")[0].InnerText);

                    //
                    XmlNodeList complexTest = m_testSuitXML.SelectNodes("/ComplexTest/Include");
                    XmlDocument tmp = new XmlDocument();
                    //Get location to test file
                    string location2 = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                    location2 = System.IO.Path.GetDirectoryName(location2);
                    string pathToTestSuit;
                    XmlNode tmpNode;
                    for (int i = 0; i < complexTest.Count; i++)
                    {
                        pathToTestSuit = System.IO.Path.Combine(location2, complexTest[i].Attributes.GetNamedItem("file").Value);
                        reader = XmlReader.Create(pathToTestSuit);
                        tmp.Load(reader);
                        tmpNode = m_testSuitXML.ImportNode(tmp.SelectSingleNode("/TestSuit/Test[@ID=\"" + complexTest[i].Attributes.GetNamedItem("testId").Value + "\"]"), true);
                        m_testSuitXML.DocumentElement.AppendChild(tmpNode);
                    }
                    m_complexTest = true;

                }
                else
                {
                    //Initialization of timeout
                    m_timeOut = Convert.ToInt32(m_testSuitXML.SelectNodes("/TestSuit/Parameters/TimeOut")[0].InnerText);
                    m_complexTest = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                result = ex.Message;
            }
            finally
            {
                if (reader != null) reader.Close();
            }

        }

        /// <summary>
        /// Write test step result to log
        /// </summary>
        /// <param name="testDescription">Part of test suit specification (contains one step)</param>
        /// <param name="message">Test result message</param>
        public void writeToLog(XmlNode testDescription, string messageIn, bool passed)
        {
            string message = messageIn;

            string resultString;

            if (passed)
            {
                resultString = "PASSED;";
            }
            else
            {
                testSummaryResult = false;
                resultString = "FAILED;";
            }

            int stepsCount = testDescription.ParentNode.SelectNodes("Step").Count;

            testResult += "Test ID " + testDescription.ParentNode.Attributes["ID"].InnerText + ";";
            if (testDescription.Attributes["id"].InnerText != (previousStepNumber + 1).ToString())
            {
                testSummaryResult = false;
            }

            if (testStepsCompleted)
            {
                testSummaryResult = false;
            }

            if (stepsCount == previousStepNumber + 1)
            {
                testStepsCompleted = true;
            }

            previousStepNumber++;

            testResult += "STEP " + testDescription.Attributes["id"].InnerText + "/" + stepsCount.ToString() + ";";
            //SW.Write(testDescription.SelectSingleNode("Command").InnerText + ";");
            testResult += resultString + testDescription.SelectSingleNode("Command").InnerText + ";";
            //SW.WriteLine(message);
            testResult += message + "\n";

            //SW.Close();
        }

        public void writeToLogInfo(string messageIn)
        {
            string message = messageIn;

            testResult += "INFO " + messageIn + "\n";

            //SW.Close();
        }

        /// <summary>
        /// SOAP exception generation with specified code
        /// </summary>
        /// <param name="testDescription">Part of test suit specification (contains one step)</param>
        /// <returns>Generated exception</returns>
        public SoapException generateExceptionObject(XmlNode testDescription)
        {
            SoapException res = null;
            System.Xml.XmlQualifiedName code = null;
            XmlQualifiedName xmlQualifiedNameSubCode = null;
            XmlQualifiedName xmlQualifiedNameSubSubCode = null;
            SoapFaultSubCode subCode = null;
            SoapFaultSubCode subSubCode = null;
            string subCodeNamespace = null;
            string message = "MESSAGE";
            string actor = "http://www.w3.org/2003/05/soap-envelope/node/ultimateReceiver";
            string role = "http://www.w3.org/2003/05/soap-envelope/node/ultimateReceiver";
            string lang = "en";
            XmlNode details = null;


            //Get fault code
            if (testDescription.SelectNodes("Code")[0].InnerText == "Sender")
            {
                code = SoapException.ClientFaultCode;
            }
            else
            {
                code = SoapException.ServerFaultCode;
            }

            //Set Details
            if (testDescription.SelectNodes("Details").Count != 0)
            {
                details = testDescription.SelectSingleNode("Details").FirstChild;
                if (testDescription.SelectSingleNode("Details").Attributes.GetNamedItem("InvalidFilterFaultTimestamp") != null)
                {
                    XmlNameTable xmlNameTable = testDescription.OwnerDocument.NameTable;
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlNameTable);
                    xmlNamespaceManager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");
                    xmlNamespaceManager.AddNamespace("bf", "http://docs.oasis-open.org/wsrf/bf-2");
                    details.SelectSingleNode("b2:InvalidFilterFault/bf:Timestamp", xmlNamespaceManager).InnerText = System.DateTime.UtcNow.ToString("o");
                }
                if (testDescription.SelectSingleNode("Details").Attributes.GetNamedItem("InvalidMessageContentExpressionFaultTimestamp") != null)
                {
                    XmlNameTable xmlNameTable = testDescription.OwnerDocument.NameTable;
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlNameTable);
                    xmlNamespaceManager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");
                    xmlNamespaceManager.AddNamespace("bf", "http://docs.oasis-open.org/wsrf/bf-2");
                    details.SelectSingleNode("b2:InvalidMessageContentExpressionFault/bf:Timestamp", xmlNamespaceManager).InnerText = System.DateTime.UtcNow.ToString("o");
                }
                if (testDescription.SelectSingleNode("Details").Attributes.GetNamedItem("TopicNotSupportedFaultTimestamp") != null)
                {
                    XmlNameTable xmlNameTable = testDescription.OwnerDocument.NameTable;
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlNameTable);
                    xmlNamespaceManager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");
                    xmlNamespaceManager.AddNamespace("bf", "http://docs.oasis-open.org/wsrf/bf-2");
                    details.SelectSingleNode("b2:TopicNotSupportedFault/bf:Timestamp", xmlNamespaceManager).InnerText = System.DateTime.UtcNow.ToString("o");
                }
                if (testDescription.SelectSingleNode("Details").Attributes.GetNamedItem("Timestamp") != null)
                {
                    XmlNameTable xmlNameTable = testDescription.OwnerDocument.NameTable;
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlNameTable);
                    xmlNamespaceManager.AddNamespace("wsrf-bf", "http://docs.oasis-open.org/wsrf/bf-2");
                    xmlNamespaceManager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");
                    details.SelectSingleNode("*/wsrf-bf:Timestamp", xmlNamespaceManager).InnerText = System.DateTime.UtcNow.ToString("o");
                }
                if (testDescription.SelectSingleNode("Details").Attributes.GetNamedItem("MinimumTime") != null)
                {
                    XmlNameTable xmlNameTable = testDescription.OwnerDocument.NameTable;
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlNameTable);
                    xmlNamespaceManager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");
                    if (testDescription.SelectSingleNode("Details").Attributes.GetNamedItem("MinimumTime").InnerText == "now+120")
                    {
                        details.SelectSingleNode("*/b2:MinimumTime", xmlNamespaceManager).InnerText = (System.DateTime.UtcNow.AddSeconds(120)).ToString("o");
                    }
                    else
                    {
                        details.SelectSingleNode("*/b2:MinimumTime", xmlNamespaceManager).InnerText = (System.DateTime.UtcNow.AddSeconds(30)).ToString("o");
                    }
                }
                if (testDescription.SelectSingleNode("Details").Attributes.GetNamedItem("MaximumTime") != null)
                {
                    XmlNameTable xmlNameTable = testDescription.OwnerDocument.NameTable;
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlNameTable);
                    xmlNamespaceManager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");
                    details.SelectSingleNode("*/b2:MaximumTime", xmlNamespaceManager).InnerText = (System.DateTime.UtcNow.AddSeconds(60)).ToString("o");
                }
            }

            switch (testDescription.SelectNodes("Subcode").Count)
            {
                case 0: //Only code
                    //res = new SoapException(message, code);
                    break;
                case 1: //Code and subcode
                    if (testDescription.SelectNodes("Subcode")[0].Attributes["Namespace"] != null)
                    {
                        subCodeNamespace = testDescription.SelectNodes("Subcode")[0].Attributes["Namespace"].InnerText;
                    }
                    else
                    {
                        subCodeNamespace = "http://www.onvif.org/ver10/error";
                    }
                    xmlQualifiedNameSubCode = new XmlQualifiedName(testDescription.SelectNodes("Subcode")[0].InnerText, subCodeNamespace);
                    subCode = new SoapFaultSubCode(xmlQualifiedNameSubCode);
                    //res = new SoapException(message, code, subCode);
                    break;
                case 2: //Code, subcode, and subsubcode
                    if (testDescription.SelectNodes("Subcode")[0].Attributes["Namespace"] != null)
                    {
                        subCodeNamespace = testDescription.SelectNodes("Subcode")[0].Attributes["Namespace"].InnerText;
                    }
                    else
                    {
                        subCodeNamespace = "http://www.onvif.org/ver10/error";
                    }
                    xmlQualifiedNameSubCode = new XmlQualifiedName(testDescription.SelectNodes("Subcode")[0].InnerText, "http://www.onvif.org/ver10/error");
                    if (testDescription.SelectNodes("Subcode")[0].Attributes["Namespace"] != null)
                    {
                        subCodeNamespace = testDescription.SelectNodes("Subcode")[1].Attributes["Namespace"].InnerText;
                    }
                    else
                    {
                        subCodeNamespace = "http://www.onvif.org/ver10/error";
                    }
                    xmlQualifiedNameSubSubCode = new XmlQualifiedName(testDescription.SelectNodes("Subcode")[1].InnerText, subCodeNamespace);

                    subSubCode = new SoapFaultSubCode(xmlQualifiedNameSubSubCode);
                    subCode = new SoapFaultSubCode(xmlQualifiedNameSubCode, subSubCode);
                    //res = new SoapException(message, code, subCode);
                    break;
            }
            res = new SoapException(message, code, actor, role, lang, details, subCode, null);

            return res;
        }

        /// <summary>
        /// Return step list for command
        /// </summary>
        /// <param name="CommandName">Command name for step selection</param>
        /// <returns>List of steps for CommandName</returns>
        public XmlNodeList GetStepsForCommand(string CommandName)
        {
            XmlNodeList result;
            if (m_complexTest)
            {
                result = m_testSuitXML.SelectNodes("/ComplexTest/Test/Step[Command = \"" + CommandName + "\"]");
            }
            else
            {
                if (TestCommon.testID == "")
                {
                    result = m_testSuitXML.SelectNodes("/TestSuit/Test[@Enabled=\"true\"]/Step[Command = \"" + CommandName + "\"]");
                }
                else
                {
                    result = m_testSuitXML.SelectNodes("/TestSuit/Test[@ID=\"" + TestCommon.testID + "\"]/Step[Command = \"" + CommandName + "\"]");
                }
            }
            return result;
        }

        /// <summary>
        /// Generate final response for void response
        /// </summary>
        /// <param name="test">Test step description</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType GenerateResponseStepTypeVoid(XmlNode test, out SoapException ex, out int Timeout)
        {

            StepType res;
            string typeRes = test.SelectNodes("Response")[0].InnerText;

            switch (typeRes)
            {
                case "Normal":
                    {
                        res = StepType.Normal;
                        Timeout = 0;
                        ex = null;
                        break;
                    }
                case "NoResponse":
                    {
                        res = StepType.NoResponse;
                        ex = null;
                        Timeout = m_timeOut;
                        break;
                    }
                case "Fault":
                    {
                        res = StepType.Fault;
                        Timeout = 0;
                        ex = generateExceptionObject(test);
                        break;
                    }
                default:
                    {
                        res = StepType.None;
                        Timeout = 0;
                        ex = null;
                        break;
                    }
            }

            return res;
        }

        /// <summary>
        /// Generate final response for void response
        /// </summary>
        /// <param name="test">Test step description</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType GenerateResponseStepTypeVoid(XmlNode test, out SoapException ex, out int Timeout, out int special)
        {

            StepType res;
            string typeRes = test.SelectNodes("Response")[0].InnerText;

            special = 0;

            switch (typeRes)
            {
                case "Normal":
                    {
                        res = StepType.Normal;

                        if (test.SelectNodes("ResponseParameters/@special").Count != 0)
                        {
                            special = Convert.ToInt32(test.SelectSingleNode("ResponseParameters/@special").InnerText);
                        }
                        else
                        {
                            special = 0;
                        }

                        Timeout = 0;
                        ex = null;
                        break;
                    }
                case "NoResponse":
                    {
                        res = StepType.NoResponse;
                        ex = null;
                        Timeout = m_timeOut;
                        break;
                    }
                case "Fault":
                    {
                        res = StepType.Fault;
                        Timeout = 0;
                        ex = generateExceptionObject(test);
                        break;
                    }
                default:
                    {
                        res = StepType.None;
                        Timeout = 0;
                        ex = null;
                        break;
                    }
            }

            return res;
        }

        /// <summary>
        /// Generate final response for typed response
        /// </summary>
        /// <param name="test">Test step description</param>
        /// <param name="target">Output timeout for Normal response</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <param name="type">Type of output for normal resonse</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType GenerateResponseStepTypeNotVoidTimeout(XmlNode test, out object target, out SoapException ex, out int Timeout, out bool TimeoutSpecify, Type type)
        {
            StepType res = StepType.None;
            target = null;

            string typeRes = test.SelectNodes("Response")[0].InnerText;

            switch (typeRes)
            {
                case "Normal":
                    {
                        res = StepType.Normal;

                        XmlSerializer serializer = new XmlSerializer(type);
                        XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                        target = serializer.Deserialize(sr);

                        Timeout = 0;
                        ex = null;
                        break;
                    }
                case "NoResponse":
                    {
                        res = StepType.NoResponse;
                        ex = null;
                        Timeout = m_timeOut;
                        break;
                    }
                case "Fault":
                    {
                        res = StepType.Fault;
                        Timeout = 0;
                        target = null;
                        ex = generateExceptionObject(test);
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

            if (test.SelectSingleNode("Timeout") != null)
            {
                TimeoutSpecify = true;
                Timeout = Convert.ToInt32(test.SelectNodes("Timeout")[0].InnerText);
            }
            else
            {
                TimeoutSpecify = false;
            }
             
            

            return res;
        }

        /// <summary>
        /// Generate final response for typed response
        /// </summary>
        /// <param name="test">Test step description</param>
        /// <param name="target">Output timeout for Normal response</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <param name="type">Type of output for normal resonse</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType GenerateResponseStepTypeNotVoid(XmlNode test, out object target, out SoapException ex, out int Timeout, Type type)
        {
            StepType res = StepType.None;
            target = null;

            XmlNodeList xmlLst = test.SelectNodes("Response");
            string typeRes = xmlLst[0].InnerText;

            switch (typeRes)
            {
                case "Normal":
                    {
                        res = StepType.Normal;

                        XmlSerializer serializer = new XmlSerializer(type);
                        var responseNode = test.SelectNodes("ResponseParameters");
                        if (0 != responseNode.Count && !string.IsNullOrEmpty(responseNode[0].InnerXml))
                        {
                            XmlReader sr = XmlReader.Create(new StringReader(responseNode[0].InnerXml));
                            target = serializer.Deserialize(sr);
                        }
                        else
                            target = null;

                        Timeout = 0;
                        ex = null;
                        break;
                    }
                case "NoResponse":
                    {
                        res = StepType.NoResponse;
                        ex = null;
                        Timeout = m_timeOut;
                        break;
                    }
                case "Fault":
                    {
                        res = StepType.Fault;
                        Timeout = 0;
                        target = null;
                        ex = generateExceptionObject(test);
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


            return res;
        }

        /// <summary>
        /// Generate final response for typed response in the case of specific XPath
        /// </summary>
        /// <param name="test">Test step description</param>
        /// <param name="target">Output timeout for Normal response</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <param name="type">Type of output for normal resonse</param>
        /// <param name="AdditionalName">Additional name for response data to find data for deserialization</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType GenerateResponseStepTypeNotVoidSpecificXPath(XmlNode test, out object target, out SoapException ex, out int Timeout, Type type, string AdditionalName)
        {
            StepType res = StepType.None;
            target = null;

            string typeRes = test.SelectNodes("Response")[0].InnerText;

            switch (typeRes)
            {
                case "Normal":
                    {
                        res = StepType.Normal;

                        if (test.SelectSingleNode("ResponseParameters[@name=\"" + AdditionalName + "\"]") != null)
                        {
                            XmlSerializer serializer = new XmlSerializer(type);
                            XmlReader sr = XmlReader.Create(new StringReader(test.SelectSingleNode("ResponseParameters[@name=\"" + AdditionalName + "\"]").InnerXml));
                            target = serializer.Deserialize(sr);
                        }
                        else
                        {
                            target = null;
                        }

                        Timeout = 0;
                        ex = null;
                        break;
                    }
                case "NoResponse":
                    {
                        res = StepType.NoResponse;
                        ex = null;
                        if (test.SelectSingleNode("ResponseParameters[@name=\"" + AdditionalName + "\"]") != null)
                        {
                            XmlSerializer serializer = new XmlSerializer(type);
                            XmlReader sr = XmlReader.Create(new StringReader(test.SelectSingleNode("ResponseParameters[@name=\"" + AdditionalName + "\"]").InnerXml));
                            target = serializer.Deserialize(sr);
                        }
                        else
                        {
                            target = null;
                        }
                        Timeout = m_timeOut;
                        break;
                    }
                case "Fault":
                    {
                        res = StepType.Fault;
                        Timeout = 0;
                        target = null;
                        ex = generateExceptionObject(test);
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
            return res;
        }

        /// <summary>
        /// Generate final response for typed response whith possibility to set special case flag
        /// </summary>
        /// <param name="test">Test step description</param>
        /// <param name="target">Output timeout for Normal response</param>
        /// <param name="ex">Output exception for Fault response</param>
        /// <param name="Timeout">Output timeout for NoResponse response</param>
        /// <param name="type">Type of output for normal resonse</param>
        /// <returns>Step type (Normal, Fault, NoResponse)</returns>
        public StepType GenerateResponseStepTypeNotVoidSpecial(XmlNode test, out object target, out SoapException ex, out int Timeout, Type type, out int special)
        {
            StepType res = StepType.None;
            target = null;
            special = 0;

            string typeRes = test.SelectNodes("Response")[0].InnerText;

            switch (typeRes)
            {
                case "Normal":
                    {
                        res = StepType.Normal;

                        XmlSerializer serializer = new XmlSerializer(type);
                        XmlReader sr = XmlReader.Create(new StringReader(test.SelectNodes("ResponseParameters")[0].InnerXml));
                        target = serializer.Deserialize(sr);

                        if (test.SelectNodes("ResponseParameters/@special").Count != 0)
                        {
                            special = Convert.ToInt32(test.SelectSingleNode("ResponseParameters/@special").InnerText);
                        }
                        else
                        {
                            special = 0;
                        }

                        Timeout = 0;
                        ex = null;
                        break;
                    }
                case "NoResponse":
                    {
                        res = StepType.NoResponse;
                        ex = null;
                        Timeout = m_timeOut;
                        break;
                    }
                case "Fault":
                    {
                        res = StepType.Fault;
                        Timeout = 0;
                        target = null;
                        ex = generateExceptionObject(test);
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
            return res;
        }

        #region Change Auth Mode for Future Requests

        /// <summary>
        /// Testing for tags, allowing public authentication
        /// </summary>
        internal void GrantPublicInvoke(string AUserName, string ACommandName)
        {
            if (AUserName.ToUpper() == "Anonymous".ToUpper())
            {
                DigestAuthenticationModule.AddLocalPublicMethods(ACommandName);
            }
            else if (AUserName.ToUpper() == "Administrator".ToUpper())
            {
                DigestAuthenticationModule.RemoveLocalPublicMethods(ACommandName);
            }
        }

        internal void GrantPublicInvoke(XmlNode ANode)
        {
            XmlNode caNode = ANode.SelectSingleNode("ChangeAuth");
            if (caNode != null)
            {

                XmlNodeList lNode = caNode.SelectNodes("Command");
                foreach (XmlNode cNode in lNode)
                {
                    if (cNode != null && cNode.Attributes.Count > 0)
                    {
                        XmlAttribute uAttribute = cNode.Attributes[0];
                        GrantPublicInvoke(uAttribute.InnerText, cNode.InnerText);
                    }
                }
            }
        }

        public void ClearAuthList()
        {
            DigestAuthenticationModule.ClearLocalPublicMethods();
        }

        #endregion //Change Auth Mode for Future Requests

        #region Add user credentials for Future Requests

        /// <summary>
        /// </summary>
        public void AddUserCredentials(string userName, string password)
        {
            DigestAuthenticationModule.AddUserCredentials(userName, password);
        }

        public void ClearAllUserCredentials()
        {
            DigestAuthenticationModule.ClearAllUserCredentials();
        }

        #endregion //Add user credentials for Future Requests

        private delegate void NotificationAsync(NotificationRec notification);

        private void SendNotificationAsync(NotificationRec notification)
        {
            try
            {
                System.Threading.Thread.Sleep(5000);

                NotificationProvider notificationProvider = new NotificationProvider();
                notificationProvider.Notify(notification.Address, notification.Request);
            }
            catch (Exception) { }
        }

        internal void SendNotification(XmlNode test, ref string logMessage, ref bool passed, string lastNotificationAddress)
        {
            if (test.SelectSingleNode("Notification") != null)
            {
                NotificationRec rec = new NotificationRec();
                if (test.SelectSingleNode("Notification/@address") != null)
                {
                    rec.Address = test.SelectSingleNode("Notification/@address").Value;
                }
                else
                {
                    rec.Address = lastNotificationAddress;
                }
                rec.Request = test.SelectSingleNode("Notification").InnerText;

                NotificationAsync func = new NotificationAsync(SendNotificationAsync);
                func.BeginInvoke(rec, null, null);
            }
        }

        private void SendNotificationsAsync(List<NotificationRec> notifications)
        {
            try
            {
                List<NotificationRec> queue = notifications.OrderBy(R => R.Timeout).ToList();
                
                NotificationProvider notificationProvider = new NotificationProvider();
                int lastTimeout = 0;
                foreach (NotificationRec notification in queue)
                {
                    if (notification.Timeout - lastTimeout > 0)
                    {
                        System.Threading.Thread.Sleep(notification.Timeout - lastTimeout);
                    }
                    notificationProvider.Notify(notification.Address, notification.Request);
                    lastTimeout = notification.Timeout;
                }
            }
            catch (Exception) { }
        }

        internal void SendNotifications(XmlNode test, ref string logMessage, ref bool passed, string lastNotificationAddress)
        {
            XmlNodeList notificaitonNodes = test.SelectNodes("Notification");

            if (notificaitonNodes.Count > 0 && !string.IsNullOrEmpty(LastNotificationAddress))
            {
                List<NotificationRec> notifications = new List<NotificationRec>();

                foreach (XmlNode node in notificaitonNodes)
                { 
                    NotificationRec rec = new NotificationRec();
                    if (node.Attributes["address"] != null)
                    {
                        rec.Address = node.Attributes["address"].Value;
                    }
                    else
                    {
                        rec.Address = lastNotificationAddress;
                    }
                    int timeout = 2000;
                    if (node.Attributes["timeout"] != null)
                    {
                        timeout = int.Parse(node.Attributes["timeout"].Value);
                    }

                    rec.Request = node.InnerText;
                    rec.Timeout = timeout;

                    notifications.Add(rec);
                }

                Action<List<NotificationRec>> func = new Action<List<NotificationRec>>(SendNotificationsAsync);
                func.BeginInvoke(notifications, null, null);
            }
        }

        public string GetCurrentTestName()
        {
            if (m_complexTest)
            {
                return m_testSuitXML.SelectSingleNode("/ComplexTest/Parameters/Name").InnerText;
            }
            else
            {
                if (TestCommon.testID == "")
                {
                    return "";
                }
                else
                {
                    return m_testSuitXML.SelectSingleNode("/TestSuit/Test[@ID=\"" + TestCommon.testID + "\"]/Name").InnerText;
                }
            }
        }

        public string GetONVIFTestExpectedResult()
        {
            if (m_complexTest)
            {
                return "NONE";
            }
            else
            {
                if (TestCommon.testID == "")
                {
                    return "";
                }
                else
                {
                    XmlNode res = m_testSuitXML.SelectSingleNode("/TestSuit/Test[@ID=\"" + TestCommon.testID + "\"]/ExpectedResult/ONVIFTestResult");
                    if (res != null)
                    {
                        return m_testSuitXML.SelectSingleNode("/TestSuit/Test[@ID=\"" + TestCommon.testID + "\"]/ExpectedResult/ONVIFTestResult").InnerText;
                    }
                    else
                    {
                        return "NONE";
                    }
                }
            }
        }

        public string GetCurrentTestDescription()
        {
            if (m_complexTest)
            {
                return m_testSuitXML.SelectSingleNode("/ComplexTest/Parameters/Description").InnerText;
            }
            else
            {
                if (TestCommon.testID == "")
                {
                    return "";
                }
                else
                {
                    return m_testSuitXML.SelectSingleNode("/TestSuit/Test[@ID=\"" + TestCommon.testID + "\"]/Description").InnerText;
                }
            }
        }
        public string GetCurrentExpectedResult()
        {
            if (m_complexTest)
            {
                return m_testSuitXML.SelectSingleNode("/ComplexTest/Parameters/ExpectedResult").InnerText;
            }
            else
            {
                if (TestCommon.testID == "")
                {
                    return "";
                }
                else
                {
                    if (m_testSuitXML.SelectSingleNode("/TestSuit/Test[@ID=\"" + TestCommon.testID + "\"]/ExpectedResult") != null)
                    {
                        return m_testSuitXML.SelectSingleNode("/TestSuit/Test[@ID=\"" + TestCommon.testID + "\"]/ExpectedResult").InnerText;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
        }

        public void StartFileServer(int special)
        {
            switch (special)
            {
                case 200:
                    FileServer.getInstance().StatusCode = System.Net.HttpStatusCode.OK;
                    FileServer.getInstance().StatusCodeNext = System.Net.HttpStatusCode.OK;
                    break;
                case 415:
                    FileServer.getInstance().StatusCode = System.Net.HttpStatusCode.UnsupportedMediaType;
                    FileServer.getInstance().StatusCodeNext = System.Net.HttpStatusCode.UnsupportedMediaType;
                    break;
                case 401:
                    FileServer.getInstance().StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    FileServer.getInstance().StatusCodeNext = System.Net.HttpStatusCode.Unauthorized;
                    break;
                case 500:
                    FileServer.getInstance().StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    FileServer.getInstance().StatusCodeNext = System.Net.HttpStatusCode.InternalServerError;
                    break;
                case 401415:
                    FileServer.getInstance().StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    FileServer.getInstance().StatusCodeNext = System.Net.HttpStatusCode.UnsupportedMediaType;
                    break;
                case 401200:
                    FileServer.getInstance().StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    FileServer.getInstance().StatusCodeNext = System.Net.HttpStatusCode.OK;
                    break;
                default:
                    FileServer.getInstance().StatusCode = System.Net.HttpStatusCode.OK;
                    FileServer.getInstance().StatusCodeNext = System.Net.HttpStatusCode.OK;
                    break;
            }


            FileServer.getInstance().Run();
        }

    }
}
