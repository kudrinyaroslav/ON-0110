using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;

namespace DUT.CameraWebService.PTZ20
{
    /// <summary>
    /// Summary description for PtzService
    /// </summary>
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver20/ptz/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PtzService : PTZBinding
    {

        //TestSuit
        TestCommon m_TestCommon = null;
        PTZServiceTest m_PTZServiceTest = null;

        public void TestSuitInit()
        {
            if (Application["m_TestCommon"] != null)
            {
                m_TestCommon = (TestCommon)Application["m_TestCommon"];
            }
            else
            {
                m_TestCommon = new TestCommon();
                m_TestCommon.LoadTestSuit();
                Application["m_TestCommon"] = m_TestCommon;
            }

            if (Application["m_PTZServiceTest"] != null)
            {
                m_PTZServiceTest = (PTZServiceTest)Application["m_PTZServiceTest"];
            }
            else
            {
                m_PTZServiceTest = new PTZServiceTest(m_TestCommon);
                Application["m_PTZServiceTest"] = m_PTZServiceTest;
            }
        }

        private void StepTypeProcessing(StepType stepType, SoapException ex, int timeOut)
        {
            switch (stepType)
            {
                case StepType.Normal:
                    {
                        break;
                    }
                case StepType.Fault:
                    {
                        throw ex;
                    }
                case StepType.NoResponse:
                    {
                        System.Threading.Thread.Sleep(timeOut);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetNodes", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PTZNode")]
        public override PTZNode[] GetNodes()
        {
            TestSuitInit();
            PTZNode[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.GetNodesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetNode", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PTZNode")]
        public override PTZNode GetNode(string NodeToken)
        {
            TestSuitInit();
            PTZNode res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.GetNodeTest(out res, out ex, out timeOut, NodeToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetConfiguration", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PTZConfiguration")]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/event/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\"><soap:Body><tptz:GetConfigurationResponse><tptz:PTZConfiguration token=\"eptz\"><tt:Name>eptzName</tt:Name><tt:UseCount>0</tt:UseCount><tt:NodeToken>eptzNodeToken1</tt:NodeToken><tt:DefaultAbsolutePantTiltPositionSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:DefaultAbsolutePantTiltPositionSpace><tt:DefaultAbsoluteZoomPositionSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:DefaultAbsoluteZoomPositionSpace><tt:DefaultRelativePanTiltTranslationSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</tt:DefaultRelativePanTiltTranslationSpace><tt:DefaultRelativeZoomTranslationSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</tt:DefaultRelativeZoomTranslationSpace><tt:DefaultPTZSpeed><tt:PanTilt x=\"0.100\" y=\"0.100\"/><tt:Zoom x=\"1.000\"/></tt:DefaultPTZSpeed><tt:DefaultContinuousPanTiltVelocitySpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</tt:DefaultContinuousPanTiltVelocitySpace><tt:DefaultContinuousZoomVelocitySpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</tt:DefaultContinuousZoomVelocitySpace><tt:DefaultPTZTimeout>PT5S</tt:DefaultPTZTimeout><tt:PanTiltLimits><tt:Range><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:URI><tt:XRange><tt:Min>-INF</tt:Min><tt:Max>INF</tt:Max></tt:XRange><tt:YRange><tt:Min>-INF</tt:Min><tt:Max>INF</tt:Max></tt:YRange></tt:Range></tt:PanTiltLimits><tt:ZoomLimits><tt:Range><tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:URI><tt:XRange><tt:Min>-INF</tt:Min><tt:Max>INF</tt:Max></tt:XRange></tt:Range></tt:ZoomLimits></tptz:PTZConfiguration></tptz:GetConfigurationResponse></soap:Body></soap:Envelope>")]
        public override PTZConfiguration GetConfiguration(string PTZConfigurationToken)
        {
            TestSuitInit();
            PTZConfiguration res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.GetConfigurationTest(out res, out ex, out timeOut, PTZConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetConfigurations", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PTZConfiguration")]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/event/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\"><soap:Body><tptz:GetConfigurationsResponse><tptz:PTZConfiguration token=\"eptz\"><tt:Name>eptzName</tt:Name><tt:UseCount>0</tt:UseCount><tt:NodeToken>eptzNodeToken1</tt:NodeToken><tt:DefaultAbsolutePantTiltPositionSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:DefaultAbsolutePantTiltPositionSpace><tt:DefaultAbsoluteZoomPositionSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:DefaultAbsoluteZoomPositionSpace><tt:DefaultRelativePanTiltTranslationSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</tt:DefaultRelativePanTiltTranslationSpace><tt:DefaultRelativeZoomTranslationSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</tt:DefaultRelativeZoomTranslationSpace><tt:DefaultPTZSpeed><tt:PanTilt x=\"0.100\" y=\"0.100\"/><tt:Zoom x=\"1.000\"/></tt:DefaultPTZSpeed><tt:DefaultContinuousPanTiltVelocitySpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</tt:DefaultContinuousPanTiltVelocitySpace><tt:DefaultContinuousZoomVelocitySpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</tt:DefaultContinuousZoomVelocitySpace><tt:DefaultPTZTimeout>PT5S</tt:DefaultPTZTimeout><tt:PanTiltLimits><tt:Range><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:URI><tt:XRange><tt:Min>-INF</tt:Min><tt:Max>INF</tt:Max></tt:XRange><tt:YRange><tt:Min>-INF</tt:Min><tt:Max>INF</tt:Max></tt:YRange></tt:Range></tt:PanTiltLimits><tt:ZoomLimits><tt:Range><tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:URI><tt:XRange><tt:Min>-INF</tt:Min><tt:Max>INF</tt:Max></tt:XRange></tt:Range></tt:ZoomLimits></tptz:PTZConfiguration></tptz:GetConfigurationsResponse></soap:Body></soap:Envelope>")]
        public override PTZConfiguration[] GetConfigurations()
        {
            TestSuitInit();
            PTZConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.GetConfigurationsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/SetConfiguration", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetConfiguration(PTZConfiguration PTZConfiguration, bool ForcePersistence)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.SetConfigurationTest(out ex, out timeOut, PTZConfiguration, ForcePersistence);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        #region XmlReplySubstituteExtension for testing
        //a. Test Suite: "PTZ(PTZ-3-1-4)TestSuit.xml", Test Case: "TC.PTZ-3-1-4.01"
        //a. Test Group Name: "PTZ-3-1-4 PTZ CONTINUOUS MOVE"
        //a.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetConfigurationOptionsResponse xmlns=\"http://www.onvif.org/ver20/ptz/wsdl\">      <PTZConfigurationOptions>        <Spaces xmlns=\"http://www.onvif.org/ver10/schema\">          <AbsolutePanTiltPositionSpace>            <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>            <YRange>              <Min>-1</Min>              <Max>1</Max>            </YRange>          </AbsolutePanTiltPositionSpace>          <AbsoluteZoomPositionSpace>            <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</URI>            <XRange>              <Min>0</Min>              <Max>1</Max>            </XRange>          </AbsoluteZoomPositionSpace>          <RelativePanTiltTranslationSpace>            <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>            <YRange>              <Min>-1</Min>              <Max>1</Max>            </YRange>          </RelativePanTiltTranslationSpace>          <RelativeZoomTranslationSpace>            <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>          </RelativeZoomTranslationSpace>          <ContinuousPanTiltVelocitySpace>            <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>            <YRange>              <Min>-1</Min>              <Max>1</Max>            </YRange>          </ContinuousPanTiltVelocitySpace>          <ContinuousZoomVelocitySpace>            <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>          </ContinuousZoomVelocitySpace>          <PanTiltSpeedSpace>            <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>          </PanTiltSpeedSpace>          <ZoomSpeedSpace>            <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace</URI>            <XRange>              <Min>4.3</Min>              <Max>4.3</Max>            </XRange>          </ZoomSpeedSpace>        </Spaces>        <PTZTimeout xmlns=\"http://www.onvif.org/ver10/schema\">          <Min>PT0S</Min>          <Max>PT73S</Max>        </PTZTimeout>      </PTZConfigurationOptions>    </GetConfigurationOptionsResponse>  </soap:Body></soap:Envelope>")]
        //a.1. TC.PTZ-3-1-4.03: "Change tags order (in main response GetConfigurationOptions)" (set tag PTZConfigurationOptions\Spaces\AbsolutePanTiltPositionSpace\<YRange> in front of PTZConfigurationOptions\Spaces\AbsolutePanTiltPositionSpace\<XRange> tag)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetConfigurationOptionsResponse xmlns=\"http://www.onvif.org/ver20/ptz/wsdl\">      <PTZConfigurationOptions>        <Spaces xmlns=\"http://www.onvif.org/ver10/schema\">          <AbsolutePanTiltPositionSpace>            <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</URI>     <YRange>              <Min>-1</Min>              <Max>1</Max>            </YRange>       <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>                      </AbsolutePanTiltPositionSpace>          <AbsoluteZoomPositionSpace>            <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</URI>            <XRange>              <Min>0</Min>              <Max>1</Max>            </XRange>          </AbsoluteZoomPositionSpace>          <RelativePanTiltTranslationSpace>            <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>            <YRange>              <Min>-1</Min>              <Max>1</Max>            </YRange>          </RelativePanTiltTranslationSpace>          <RelativeZoomTranslationSpace>            <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>          </RelativeZoomTranslationSpace>          <ContinuousPanTiltVelocitySpace>            <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>            <YRange>              <Min>-1</Min>              <Max>1</Max>            </YRange>          </ContinuousPanTiltVelocitySpace>          <ContinuousZoomVelocitySpace>            <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>          </ContinuousZoomVelocitySpace>          <PanTiltSpeedSpace>            <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>          </PanTiltSpeedSpace>          <ZoomSpeedSpace>            <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace</URI>            <XRange>              <Min>4.3</Min>              <Max>4.3</Max>            </XRange>          </ZoomSpeedSpace>        </Spaces>        <PTZTimeout xmlns=\"http://www.onvif.org/ver10/schema\">          <Min>PT0S</Min>          <Max>PT73S</Max>        </PTZTimeout>      </PTZConfigurationOptions>    </GetConfigurationOptionsResponse>  </soap:Body></soap:Envelope>")]
        //a.2. TC.PTZ-3-1-4.06: "Delete child tag (in main response GetConfigurationOptions)" (delete tag PTZConfigurationOptions\<PTZTimeout>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetConfigurationOptionsResponse xmlns=\"http://www.onvif.org/ver20/ptz/wsdl\">      <PTZConfigurationOptions>        <Spaces xmlns=\"http://www.onvif.org/ver10/schema\">          <AbsolutePanTiltPositionSpace>            <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>            <YRange>              <Min>-1</Min>              <Max>1</Max>            </YRange>          </AbsolutePanTiltPositionSpace>          <AbsoluteZoomPositionSpace>            <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</URI>            <XRange>              <Min>0</Min>              <Max>1</Max>            </XRange>          </AbsoluteZoomPositionSpace>          <RelativePanTiltTranslationSpace>            <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>            <YRange>              <Min>-1</Min>              <Max>1</Max>            </YRange>          </RelativePanTiltTranslationSpace>          <RelativeZoomTranslationSpace>            <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>          </RelativeZoomTranslationSpace>          <ContinuousPanTiltVelocitySpace>            <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>            <YRange>              <Min>-1</Min>              <Max>1</Max>            </YRange>          </ContinuousPanTiltVelocitySpace>          <ContinuousZoomVelocitySpace>            <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>          </ContinuousZoomVelocitySpace>          <PanTiltSpeedSpace>            <URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace</URI>            <XRange>              <Min>-1</Min>              <Max>1</Max>            </XRange>          </PanTiltSpeedSpace>          <ZoomSpeedSpace>            <URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace</URI>            <XRange>              <Min>4.3</Min>              <Max>4.3</Max>            </XRange>          </ZoomSpeedSpace>        </Spaces>             </PTZConfigurationOptions>    </GetConfigurationOptionsResponse>  </soap:Body></soap:Envelope>")]        
        #endregion //XmlReplySubstituteExtension for testing         
        [return: System.Xml.Serialization.XmlElementAttribute("PTZConfigurationOptions")]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/event/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\"><soap:Body><tptz:GetConfigurationOptionsResponse><tptz:PTZConfigurationOptions><tt:Spaces><tt:AbsolutePanTiltPositionSpace><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:URI><tt:XRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange><tt:YRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:YRange></tt:AbsolutePanTiltPositionSpace><tt:AbsolutePanTiltPositionSpace><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/DigitalPositionSpace</tt:URI><tt:XRange><tt:Min>0.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange><tt:YRange><tt:Min>0.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:YRange></tt:AbsolutePanTiltPositionSpace><tt:AbsoluteZoomPositionSpace><tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:URI><tt:XRange><tt:Min>0.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange></tt:AbsoluteZoomPositionSpace><tt:RelativePanTiltTranslationSpace><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</tt:URI><tt:XRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange><tt:YRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:YRange></tt:RelativePanTiltTranslationSpace><tt:RelativeZoomTranslationSpace><tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</tt:URI><tt:XRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange></tt:RelativeZoomTranslationSpace><tt:ContinuousPanTiltVelocitySpace><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</tt:URI><tt:XRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange><tt:YRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:YRange></tt:ContinuousPanTiltVelocitySpace><tt:ContinuousZoomVelocitySpace><tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</tt:URI><tt:XRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange></tt:ContinuousZoomVelocitySpace><tt:PanTiltSpeedSpace><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace</tt:URI><tt:XRange><tt:Min>0.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange></tt:PanTiltSpeedSpace><tt:ZoomSpeedSpace><tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace</tt:URI><tt:XRange><tt:Min>0.000000</tt:Min><tt:Max>INF</tt:Max></tt:XRange></tt:ZoomSpeedSpace></tt:Spaces><tt:PTZTimeout><tt:Min>PT1S</tt:Min><tt:Max>PT5S</tt:Max></tt:PTZTimeout></tptz:PTZConfigurationOptions></tptz:GetConfigurationOptionsResponse></soap:Body></soap:Envelope>")]
        public override PTZConfigurationOptions GetConfigurationOptions(string ConfigurationToken)
        {
            TestSuitInit();
            PTZConfigurationOptions res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.GetConfigurationOptionsTest(out res, out ex, out timeOut, ConfigurationToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/SendAuxiliaryCommand", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AuxiliaryResponse")]
        public override string SendAuxiliaryCommand(string ProfileToken, string AuxiliaryData)
        {
            TestSuitInit();
            string res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.SendAuxiliaryCommandTest(out res, out ex, out timeOut, ProfileToken, AuxiliaryData);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetPresets", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Preset")]
        public override PTZPreset[] GetPresets(string ProfileToken)
        {
            TestSuitInit();
            PTZPreset[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.GetPresetsTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/SetPreset", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetPreset(string ProfileToken, string PresetName, ref string PresetToken)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.SetPresetTest(out ex, out timeOut, ProfileToken, PresetName, ref PresetToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }
        
        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/RemovePreset", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemovePreset(string ProfileToken, string PresetToken)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.RemovePresetTest(out ex, out timeOut, ProfileToken, PresetToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GotoPreset", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void GotoPreset(string ProfileToken, string PresetToken, PTZSpeed Speed)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.GotoPresetTest(out ex, out timeOut, ProfileToken, PresetToken, Speed);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GotoHomePosition", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void GotoHomePosition(string ProfileToken, PTZSpeed Speed)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.GotoHomePositionTest(out ex, out timeOut, ProfileToken, Speed);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/SetHomePosition", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetHomePosition(string ProfileToken)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.SetHomePositionTest(out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/ContinuousMove", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]

        #region XmlReplySubstituteExtension for testing
        //a. Test Suite: "PTZ(PTZ-3-1-4)TestSuit.xml", Test Case: "TC.PTZ-3-1-4.01"
        //a. Test Group Name: "PTZ-3-1-4 PTZ CONTINUOUS MOVE"
        //a.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <ContinuousMoveResponse xmlns=\"http://www.onvif.org/ver20/ptz/wsdl\" />  </soap:Body></soap:Envelope>")]        
        //a.1. TC.PTZ-3-1-4.08: "Delete namespace (in main response ContinuousMove)" (delete namespace from tag <ContinuousMove>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <ContinuousMoveResponse/>  </soap:Body></soap:Envelope>")]
        #endregion //XmlReplySubstituteExtension for testing
        public override void ContinuousMove(string ProfileToken, PTZSpeed Velocity, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string Timeout)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.ContinuousMoveTest(out ex, out timeOut, ProfileToken, Velocity, Timeout);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/RelativeMove", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RelativeMove(string ProfileToken, PTZVector Translation, PTZSpeed Speed)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.RelativeMoveTest(out ex, out timeOut, ProfileToken, Translation, Speed);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetStatus", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        #region XmlReplySubstituteExtension for testing
        //a. Test Suite: "PTZ(PTZ-3-1-4)TestSuit.xml", Test Case: "TC.PTZ-3-1-4.01"
        //a. Test Group Name: "PTZ-3-1-4 PTZ CONTINUOUS MOVE"
        //a.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetStatusResponse xmlns=\"http://www.onvif.org/ver20/ptz/wsdl\">      <PTZStatus>        <Position xmlns=\"http://www.onvif.org/ver10/schema\">          <PanTilt x=\"0\" y=\"0\" space=\"http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace\" />          <Zoom x=\"1\" space=\"http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace\" />        </Position>        <MoveStatus xmlns=\"http://www.onvif.org/ver10/schema\">          <PanTilt>IDLE</PanTilt>          <Zoom>IDLE</Zoom>        </MoveStatus>        <UtcTime xmlns=\"http://www.onvif.org/ver10/schema\">1900-01-01T01:01:01+03:00</UtcTime>      </PTZStatus>    </GetStatusResponse>  </soap:Body></soap:Envelope>")]
        //a.1. TC.PTZ-3-1-4.02: "Delete namespace (in main response GetStatus)" (delete namespace from tag PTZStatus\<Position>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetStatusResponse xmlns=\"http://www.onvif.org/ver20/ptz/wsdl\">      <PTZStatus>        <Position>          <PanTilt x=\"0\" y=\"0\" space=\"http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace\" />          <Zoom x=\"1\" space=\"http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace\" />        </Position>        <MoveStatus xmlns=\"http://www.onvif.org/ver10/schema\">          <PanTilt>IDLE</PanTilt>          <Zoom>IDLE</Zoom>        </MoveStatus>        <UtcTime xmlns=\"http://www.onvif.org/ver10/schema\">1900-01-01T01:01:01+03:00</UtcTime>      </PTZStatus>    </GetStatusResponse>  </soap:Body></soap:Envelope>")] 
        //a.2. TC.PTZ-3-1-4.04: "Change tag name (in main response GetStatus)" (change name of tag PTZStatus\Position\<Zoom> to <ZoomQQQ>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetStatusResponse xmlns=\"http://www.onvif.org/ver20/ptz/wsdl\">      <PTZStatus>        <Position xmlns=\"http://www.onvif.org/ver10/schema\">          <PanTilt x=\"0\" y=\"0\" space=\"http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace\" />          <ZoomQQQ x=\"1\" space=\"http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace\" />        </Position>        <MoveStatus xmlns=\"http://www.onvif.org/ver10/schema\">          <PanTilt>IDLE</PanTilt>          <Zoom>IDLE</Zoom>        </MoveStatus>        <UtcTime xmlns=\"http://www.onvif.org/ver10/schema\">1900-01-01T01:01:01+03:00</UtcTime>      </PTZStatus>    </GetStatusResponse>  </soap:Body></soap:Envelope>")]
        //a.3. TC.PTZ-3-1-4.05: "Add '\n' after tags (in main response GetStatus)" (add '\n' after all (or some) tags)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetStatusResponse xmlns=\"http://www.onvif.org/ver20/ptz/wsdl\">      <PTZStatus>        <Position xmlns=\"http://www.onvif.org/ver10/schema\">          <PanTilt x=\"0\" y=\"0\" space=\"http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace\" />          <Zoom x=\"1\" space=\"http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace\" />        </Position>        <MoveStatus xmlns=\"http://www.onvif.org/ver10/schema\">          <PanTilt>IDLE</PanTilt>          <Zoom>IDLE</Zoom>        </MoveStatus>        <UtcTime xmlns=\"http://www.onvif.org/ver10/schema\">1900-01-01T01:01:01+03:00</UtcTime>  \n    </PTZStatus>\n\n\n    </GetStatusResponse> \n </soap:Body>\n\n</soap:Envelope>")]
        #endregion //XmlReplySubstituteExtension for testing       
        [return: System.Xml.Serialization.XmlElementAttribute("PTZStatus")]
        public override PTZStatus GetStatus(string ProfileToken)
        {
            TestSuitInit();
            PTZStatus res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.GetStatusTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/AbsoluteMove", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AbsoluteMove(string ProfileToken, PTZVector Position, PTZSpeed Speed)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.AbsoluteMoveTest(out ex, out timeOut, ProfileToken, Position, Speed);
            StepTypeProcessing(stepType, ex, timeOut);
        }
        
        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/Stop", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void Stop(string ProfileToken, bool PanTilt, [System.Xml.Serialization.XmlIgnoreAttribute()] bool PanTiltSpecified, bool Zoom, [System.Xml.Serialization.XmlIgnoreAttribute()] bool ZoomSpecified)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.StopTest(out ex, out timeOut, ProfileToken, PanTilt, PanTiltSpecified, Zoom, ZoomSpecified);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
       //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_PTZCapabilitiesIncorrectResponseTag)] 
        public override Capabilities GetServiceCapabilities()
        {
            TestSuitInit();
            Capabilities res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.GetServiceCapabilitiesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        public override PresetTour[] GetPresetTours(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override PresetTour GetPresetTour(string ProfileToken, string PresetTourToken)
        {
            throw new NotImplementedException();
        }

        public override PTZPresetTourOptions GetPresetTourOptions(string ProfileToken, string PresetTourToken)
        {
            throw new NotImplementedException();
        }

        public override string CreatePresetTour(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override void ModifyPresetTour(string ProfileToken, PresetTour PresetTour)
        {
            throw new NotImplementedException();
        }

        public override void OperatePresetTour(string ProfileToken, string PresetTourToken, PTZPresetTourOperation Operation)
        {
            throw new NotImplementedException();
        }

        public override void RemovePresetTour(string ProfileToken, string PresetTourToken)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetCompatibleConfigurations", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PTZConfiguration")]
        public override PTZConfiguration[] GetCompatibleConfigurations(string ProfileToken)
        {
            TestSuitInit();
            PTZConfiguration[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PTZServiceTest.GetCompatibleConfigurationsTest(out res, out ex, out timeOut, ProfileToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }
    }
}
