using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;

namespace DUT.CameraWebService.Imaging20
{
    /// <summary>
    /// Summary description for ImagingService20
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.18020")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver20/imaging/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ImagingBinding", Namespace = "http://www.onvif.org/ver20/imaging/wsdl")]
    public class ImagingService20 : Imaging20ServiceBinding
    {
        //TestSuit
        TestCommon m_TestCommon = null;
        ImagingService20Test m_ImagingService20Test = null;

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

            if (Application["m_ImagingService20Test"] != null)
            {
                m_ImagingService20Test = (ImagingService20Test)Application["m_ImagingService20Test"];
            }
            else
            {
                m_ImagingService20Test = new ImagingService20Test(m_TestCommon);
                Application["m_ImagingService20Test"] = m_ImagingService20Test;
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
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/GetImagingSettings", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        #region XmlReplySubstituteExtension for testing
        //a. Test Suite: "IMAGING-1-1-1 TestSuit.xml", Test Case: "TC.IMAGING-1-1-1.05"
        //a. Test Group Name: "IMAGING-1-1-1 IMAGING COMMAND GETIMAGESETTINGS"
        //a.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetImagingSettingsResponse xmlns=\"http://www.onvif.org/ver20/imaging/wsdl\">      <ImagingSettings>        <BacklightCompensation xmlns=\"http://www.onvif.org/ver10/schema\">          <Mode>OFF</Mode>          <Level>1</Level>        </BacklightCompensation>        <Brightness xmlns=\"http://www.onvif.org/ver10/schema\">1</Brightness>        <ColorSaturation xmlns=\"http://www.onvif.org/ver10/schema\">1</ColorSaturation>        <Contrast xmlns=\"http://www.onvif.org/ver10/schema\">1</Contrast>        <Exposure xmlns=\"http://www.onvif.org/ver10/schema\">          <Mode>AUTO</Mode>          <Priority>LowNoise</Priority>          <Window bottom=\"1\" top=\"1\" right=\"1\" left=\"1\" />          <MinExposureTime>1</MinExposureTime>          <MaxExposureTime>1</MaxExposureTime>          <MinGain>1</MinGain>          <MaxGain>1</MaxGain>          <MinIris>1</MinIris>          <MaxIris>1</MaxIris>          <ExposureTime>1</ExposureTime>          <Gain>1</Gain>          <Iris>1</Iris>        </Exposure>        <Focus xmlns=\"http://www.onvif.org/ver10/schema\">          <AutoFocusMode>AUTO</AutoFocusMode>          <DefaultSpeed>1</DefaultSpeed>          <NearLimit>1</NearLimit>          <FarLimit>1</FarLimit>          <Extension />        </Focus>        <IrCutFilter xmlns=\"http://www.onvif.org/ver10/schema\">ON</IrCutFilter>        <Sharpness xmlns=\"http://www.onvif.org/ver10/schema\">1</Sharpness>        <WideDynamicRange xmlns=\"http://www.onvif.org/ver10/schema\">          <Mode>OFF</Mode>          <Level>1</Level>        </WideDynamicRange>        <WhiteBalance xmlns=\"http://www.onvif.org/ver10/schema\">          <Mode>AUTO</Mode>          <CrGain>1</CrGain>          <CbGain>1</CbGain>          <Extension />        </WhiteBalance>        <Extension xmlns=\"http://www.onvif.org/ver10/schema\" />      </ImagingSettings>    </GetImagingSettingsResponse>  </soap:Body></soap:Envelope>")]
        //a.1. TC.IMAGING-1-1-1.05: "Incorrect response for 6th request: for 'GetImagingSettings' command" (rename tag <ImagingSettings>)
//bug 10767 "[IMAGING-1-1-1] No validation for 'GetImagingSettingsResponse'"
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <GetImagingSettingsResponse xmlns=\"http://www.onvif.org/ver20/imaging/wsdl\">      <INCORRECT_TAG_ImagingSettings>        <BacklightCompensation xmlns=\"http://www.onvif.org/ver10/schema\">          <Mode>OFF</Mode>          <Level>1</Level>        </BacklightCompensation>        <Brightness xmlns=\"http://www.onvif.org/ver10/schema\">1</Brightness>        <ColorSaturation xmlns=\"http://www.onvif.org/ver10/schema\">1</ColorSaturation>        <Contrast xmlns=\"http://www.onvif.org/ver10/schema\">1</Contrast>        <Exposure xmlns=\"http://www.onvif.org/ver10/schema\">          <Mode>AUTO</Mode>          <Priority>LowNoise</Priority>          <Window bottom=\"1\" top=\"1\" right=\"1\" left=\"1\" />          <MinExposureTime>1</MinExposureTime>          <MaxExposureTime>1</MaxExposureTime>          <MinGain>1</MinGain>          <MaxGain>1</MaxGain>          <MinIris>1</MinIris>          <MaxIris>1</MaxIris>          <ExposureTime>1</ExposureTime>          <Gain>1</Gain>          <Iris>1</Iris>        </Exposure>        <Focus xmlns=\"http://www.onvif.org/ver10/schema\">          <AutoFocusMode>AUTO</AutoFocusMode>          <DefaultSpeed>1</DefaultSpeed>          <NearLimit>1</NearLimit>          <FarLimit>1</FarLimit>          <Extension />        </Focus>        <IrCutFilter xmlns=\"http://www.onvif.org/ver10/schema\">ON</IrCutFilter>        <Sharpness xmlns=\"http://www.onvif.org/ver10/schema\">1</Sharpness>        <WideDynamicRange xmlns=\"http://www.onvif.org/ver10/schema\">          <Mode>OFF</Mode>          <Level>1</Level>        </WideDynamicRange>        <WhiteBalance xmlns=\"http://www.onvif.org/ver10/schema\">          <Mode>AUTO</Mode>          <CrGain>1</CrGain>          <CbGain>1</CbGain>          <Extension />        </WhiteBalance>        <Extension xmlns=\"http://www.onvif.org/ver10/schema\" />      </INCORRECT_TAG_ImagingSettings>    </GetImagingSettingsResponse>  </soap:Body></soap:Envelope>")]

        //b. Test Suite: "IMAGING-1-1-1 TestSuit.xml", Test Case: "TC.IMAGING-1-1-1.12"
        //   Test Suite: "IMAGING-1-1-2 TestSuit.xml", Test Case: "TC.IMAGING-1-1-2.02"
        //b. Test Group Name: "IMAGING-1-1-1 IMAGING COMMAND GETIMAGESETTINGS"
        //   Test Group Name: "IMAGING-1-1-2 IMAGING COMMAND GETIMAGESETTINGS – INVALID VIDEOSOURCETOKEN"
        //b.0. CORRECT XML RESPONSE
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <soap:Fault>      <soap:Code>        <soap:Value>soap:Receiver</soap:Value>        <soap:Subcode>          <soap:Value xmlns:q0=\"http://www.onvif.org/ver10/error\">q0:ActionNotSupported</soap:Value>        </soap:Subcode>      </soap:Code>      <soap:Reason>        <soap:Text xml:lang=\"en\">System.Web.Services.Protocols.SoapException: MESSAGE   at DUT.CameraWebService.Imaging20.ImagingService20.StepTypeProcessing(StepType stepType, SoapException ex, Int32 timeOut) in D:\Documents\Testing\ONVIF\Version2\DUT\CameraWebService\ServiceImaging20\ImagingService20.asmx.cs:line 61   at DUT.CameraWebService.Imaging20.ImagingService20.GetImagingSettings(String VideoSourceToken) in D:\Documents\Testing\ONVIF\Version2\DUT\CameraWebService\ServiceImaging20\ImagingService20.asmx.cs:line 97</soap:Text>      </soap:Reason>      <soap:Node>http://www.w3.org/2003/05/soap-envelope/node/ultimateReceiver</soap:Node>      <soap:Role>http://www.w3.org/2003/05/soap-envelope/node/ultimateReceiver</soap:Role>      <soap:Detail />    </soap:Fault>  </soap:Body></soap:Envelope>")]
        //b.1. TC.IMAGING-1-1-1.12: "Fault response for 6th request: for 'GetImagingSettings' command incorrect fault response" (delete tag Fault\<Code>)
        //     TC.IMAGING-1-1-2.02: "Fault response for 6th request: for 'GetImagingSettings' command incorrect fault response" (delete tag Fault\<Code>)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <soap:Body>    <soap:Fault>      <soap:Reason>        <soap:Text xml:lang=\"en\">System.Web.Services.Protocols.SoapException: MESSAGE   at DUT.CameraWebService.Imaging20.ImagingService20.StepTypeProcessing(StepType stepType, SoapException ex, Int32 timeOut) in D:\\Documents\\Testing\\ONVIF\\Version2\\DUT\\CameraWebService\\ServiceImaging20\\ImagingService20.asmx.cs:line 61   at DUT.CameraWebService.Imaging20.ImagingService20.GetImagingSettings(String VideoSourceToken) in D:\\Documents\\Testing\\ONVIF\\Version2\\DUT\\CameraWebService\\ServiceImaging20\\ImagingService20.asmx.cs:line 97</soap:Text>      </soap:Reason>      <soap:Node>http://www.w3.org/2003/05/soap-envelope/node/ultimateReceiver</soap:Node>      <soap:Role>http://www.w3.org/2003/05/soap-envelope/node/ultimateReceiver</soap:Role>      <soap:Detail />    </soap:Fault>  </soap:Body></soap:Envelope>")]
        #endregion //XmlReplySubstituteExtension for testing        
        [return: System.Xml.Serialization.XmlElementAttribute("ImagingSettings")]
        public override ImagingSettings20 GetImagingSettings(string VideoSourceToken)
        {
            TestSuitInit();
            ImagingSettings20 res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService20Test.GetImagingSettingsTest(out res, out ex, out timeOut, VideoSourceToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/SetImagingSettings", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetImagingSettings(string VideoSourceToken, ImagingSettings20 ImagingSettings, bool ForcePersistence, [System.Xml.Serialization.XmlIgnoreAttribute()] bool ForcePersistenceSpecified)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService20Test.SetImagingSettingsTest(out ex, out timeOut, VideoSourceToken, ImagingSettings, ForcePersistence, ForcePersistenceSpecified);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/GetOptions", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        #region XmlReplySubstituteExtension for testing
        //Wrong enumeration
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetOptionsResponse xmlns=\"http://www.onvif.org/ver20/imaging/wsdl\">      <ImagingOptions p2:any_Attr=\"anySimpleType\" xmlns:p2=\"http://www.onvif.org/ver20/imaging/wsdl\"><BacklightCompensation xmlns=\"http://www.onvif.org/ver10/schema\"><Mode>OFF</Mode><Mode>ON</Mode><Level><Min>1</Min><Max>1</Max></Level></BacklightCompensation><Brightness xmlns=\"http://www.onvif.org/ver10/schema\"><Min>1</Min><Max>1</Max></Brightness><ColorSaturation xmlns=\"http://www.onvif.org/ver10/schema\"><Min>1</Min><Max>1</Max></ColorSaturation><Contrast xmlns=\"http://www.onvif.org/ver10/schema\"><Min>1</Min><Max>1</Max></Contrast><Exposure xmlns=\"http://www.onvif.org/ver10/schema\"><Mode>AUTO</Mode><Mode>MANUAL</Mode><Priority>LowNoise</Priority><Priority>FrameRate</Priority><Priority>LowNoise</Priority><MinExposureTime><Min>1</Min><Max>1</Max></MinExposureTime><MaxExposureTime><Min>1</Min><Max>1</Max></MaxExposureTime><MinGain><Min>1</Min><Max>1</Max></MinGain><MaxGain><Min>1</Min><Max>1</Max></MaxGain><MinIris><Min>1</Min><Max>1</Max></MinIris><MaxIris><Min>1</Min><Max>1</Max></MaxIris><ExposureTime><Min>1</Min><Max>1</Max></ExposureTime><Gain><Min>1</Min><Max>1</Max></Gain><Iris><Min>1</Min><Max>1</Max></Iris></Exposure><Focus xmlns=\"http://www.onvif.org/ver10/schema\"><AutoFocusModes>AUTO</AutoFocusModes><AutoFocusModes>MANUAL</AutoFocusModes><DefaultSpeed><Min>1</Min><Max>1</Max></DefaultSpeed><NearLimit><Min>1</Min><Max>1</Max></NearLimit><FarLimit><Min>1</Min><Max>1</Max></FarLimit><Extension /></Focus><IrCutFilterModes xmlns=\"http://www.onvif.org/ver10/schema\">ON</IrCutFilterModes><IrCutFilterModes xmlns=\"http://www.onvif.org/ver10/schema\">OZZ</IrCutFilterModes><IrCutFilterModes xmlns=\"http://www.onvif.org/ver10/schema\">OFF</IrCutFilterModes><IrCutFilterModes xmlns=\"http://www.onvif.org/ver10/schema\">AUTO</IrCutFilterModes><Sharpness xmlns=\"http://www.onvif.org/ver10/schema\"><Min>1</Min><Max>1</Max></Sharpness><WideDynamicRange xmlns=\"http://www.onvif.org/ver10/schema\"><Mode>OFF</Mode><Mode>ON</Mode><Level><Min>1</Min><Max>1</Max></Level></WideDynamicRange><WhiteBalance xmlns=\"http://www.onvif.org/ver10/schema\"><Mode>AUTO</Mode><Mode>MANUAL</Mode><YrGain><Min>1</Min><Max>1</Max></YrGain><YbGain><Min>1</Min><Max>1</Max></YbGain><Extension /></WhiteBalance><Extension xmlns=\"http://www.onvif.org/ver10/schema\" />      </ImagingOptions>    </GetOptionsResponse>  </soap:Body></soap:Envelope>")]
        #endregion //XmlReplySubstituteExtension for testing  
        [return: System.Xml.Serialization.XmlElementAttribute("ImagingOptions")]
        public override ImagingOptions20 GetOptions(string VideoSourceToken)
        {
            TestSuitInit();
            ImagingOptions20 res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService20Test.GetOptionsTest(out res, out ex, out timeOut, VideoSourceToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/Move", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void Move(string VideoSourceToken, FocusMove Focus)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService20Test.MoveTest(out ex, out timeOut, VideoSourceToken, Focus);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/GetMoveOptions", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("MoveOptions")]
        public override MoveOptions20 GetMoveOptions(string VideoSourceToken)
        {
            TestSuitInit();
            MoveOptions20 res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService20Test.GetMoveOptionsTest(out res, out ex, out timeOut, VideoSourceToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/FocusStop", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void Stop(string VideoSourceToken)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService20Test.StopTest(out ex, out timeOut, VideoSourceToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/GetStatus", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Status")]
        public override ImagingStatus20 GetStatus(string VideoSourceToken)
        {
            TestSuitInit();
            ImagingStatus20 res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService20Test.GetStatusTest(out res, out ex, out timeOut, VideoSourceToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/imaging/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver20/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_ImagingCapabilitiesIncorrectResponseTag)]
        public override Capabilities GetServiceCapabilities()
        {
            TestSuitInit();
            Capabilities res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService20Test.GetServiceCapabilitiesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        public override ImagingPreset[] GetPresets(string VideoSourceToken)
        {
            throw new NotImplementedException();
        }

        public override ImagingPreset GetCurrentPreset(string VideoSourceToken)
        {
            throw new NotImplementedException();
        }

        public override void SetCurrentPreset(string VideoSourceToken, string PresetToken)
        {
            throw new NotImplementedException();
        }
    }
}
