using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Events10
{
    /// <summary>
    /// Summary description for PullpointSubscriptionService
    /// </summary>
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/events/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PullpointSubscriptionService : PullPointSubscriptionBinding
    {
        //TestSuit
        TestCommon m_TestCommon = null;
        EventServiceTest m_EventServiceTest = null;
        // Receive all SOAP headers.
        public SoapUnknownHeader[] unknownHeaders;
        public ActionHeader actionHeader = new ActionHeader();

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

            if (Application["m_EventServiceTest"] != null)
            {
                m_EventServiceTest = (EventServiceTest)Application["m_EventServiceTest"];
            }
            else
            {
                m_EventServiceTest = new EventServiceTest(m_TestCommon);
                Application["m_EventServiceTest"] = m_EventServiceTest;
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

        private void TimeoutProcessing(bool timeOutSpec, int timeOut)
        {
            if (timeOutSpec)
            {
                System.Threading.Thread.Sleep(timeOut);
            }
                        
        }

        private void SoapHeaderProcessing(SoapUnknownHeader[] unknownHeaders)
        {
            foreach (SoapUnknownHeader header in unknownHeaders)
            {
                header.DidUnderstand = true;
            }
        }

        [WebMethod]
        [SoapHeader("unknownHeaders")]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/PullMessages", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CurrentTime")]
        #region XmlReplySubstituteExtension for test cases
        //Test Suite: "ON-0110\Version2\DUT\TS_DoorControl\Property Events\DOORCONTROL-6-1-1_Logic.xml"
        //Test Case: "TC.DOORCONTROL-6-1-1.205"
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"> <soap:Header> <a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action> </soap:Header> <soap:Body> <PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\"> <CurrentTime1>2013-04-25T16:36:24.7705676+04:00</CurrentTime1> <TerminationTime>2013-04-25T16:37:19.7705676+04:00</TerminationTime> <NotificationMessage xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\"> <Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:Door/State/DoorMode</Topic> <ProducerReference> <Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://localhost:3246/Events/PullPointServiceFake.asmx?param=value</Address> </ProducerReference> <Message> <tt:Message UtcTime=\"2012-12-01T07:18:40.321\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" PropertyOperation=\"Initialized\"> <tt:Source> <tt:SimpleItem Name=\"DoorToken\" Value=\"tokenDoorPoint1\" /> </tt:Source> <tt:Data> <tt:SimpleItem Name=\"State\" Value=\"Locked\" /> </tt:Data> </tt:Message> </Message> </NotificationMessage> </PullMessagesResponse> </soap:Body></soap:Envelope>")]
        //QUICK_INSTALL-3-1-1 prefix is defined in the ENVELOP
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket605_PMEnv)]

        //QUICK_INSTALL-3-1-1 prefix is defined in the Body
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket605_PMBody)]

        //QUICK_INSTALL-3-1-1 prefix is defined in the PullMessages response
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket605_PMResponse)]

        //QUICK_INSTALL-3-1-1 prefix is defined in the NotificationMessage
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket605_PMNotificationMessage)]
        
        //QUICK_INSTALL-3-1-1 prefix is defined in the Topic
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket605_PMTopic)]

        //QUICK_INSTALL-3-1-1 no prefix is defined
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket605_PMnoPrefix)]

        //QUICK_INSTALL-3-1-1 prefix tns1 is redefined within PM
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket605_PMredefined_tns1)]
        #endregion //XmlReplySubstituteExtension for test cases
        #region XmlReplySubstituteExtension for bugs
        //Bug 10501
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" +
        //"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\"><soap:Header><wsa:Action>http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</wsa:Action>" + "\r\n" +
        //"</soap:Header>" + "\r\n" +
        //"<soap:Body><tev:PullMessagesResponse><tev:CurrentTime>2012-12-16T16:36:31Z</tev:CurrentTime>" + "\r\n" +
        //"<tev:TerminationTime>2012-12-16T16:36:51Z</tev:TerminationTime>" + "\r\n" +
        //"<wsnt:NotificationMessage><wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:MediaControl/VideoSourceConfiguration</wsnt:Topic>" + "\r\n" +
        //"<wsnt:Message><tt:Message UtcTime=\"2012-12-16T16:36:31Z\" PropertyOperation=\"Initialized\"><tt:Source><tt:SimpleItem Name=\"VideoSourceConfigurationToken\" Value=\"VideoSourceToken\"/>" + "\r\n" +
        //"</tt:Source>" + "\r\n" +
        //"<tt:Data><tt:ElementItem Name=\"Config\"><trt:Configuration token=\"VideoSourceToken\"><tt:Name>VideoSourceConfig</tt:Name>" + "\r\n" +
        //"<tt:UseCount>0</tt:UseCount>" + "\r\n" +
        //"<tt:SourceToken>VideoSource_1</tt:SourceToken>" + "\r\n" +
        //"<tt:Bounds x=\"0\" y=\"0\" width=\"2560\" height=\"1920\"></tt:Bounds>" + "\r\n" +
        //"</trt:Configuration>" + "\r\n" +
        //"</tt:ElementItem>" + "\r\n" +
        //"</tt:Data>" + "\r\n" +
        //"</tt:Message>" + "\r\n" +
        //"</wsnt:Message>" + "\r\n" +
        //"</wsnt:NotificationMessage>" + "\r\n" +
        //"<wsnt:NotificationMessage><wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:MediaControl/VideoEncoderConfiguration</wsnt:Topic>" + "\r\n" +
        //"<wsnt:Message><tt:Message UtcTime=\"2012-12-16T16:36:31Z\" PropertyOperation=\"Initialized\"><tt:Source><tt:SimpleItem Name=\"VideoEncoderConfigurationToken\" Value=\"VideoEncoderToken_1\"/>" + "\r\n" +
        //"</tt:Source>" + "\r\n" +
        //"<tt:Data><tt:ElementItem Name=\"Config\"><trt:Configuration token=\"VideoEncoderToken_1\"><tt:Name>VideoEncoder_1</tt:Name>" + "\r\n" +
        //"<tt:UseCount>0</tt:UseCount>" + "\r\n" +
        //"<tt:Encoding>H264</tt:Encoding>" + "\r\n" +
        //"<tt:Resolution><tt:Width>1920</tt:Width>" + "\r\n" +
        //"<tt:Height>1080</tt:Height>" + "\r\n" +
        //"</tt:Resolution>" + "\r\n" +
        //"<tt:Quality>1.000000</tt:Quality>" + "\r\n" +
        //"<tt:RateControl><tt:FrameRateLimit>0</tt:FrameRateLimit>" + "\r\n" +
        //"<tt:EncodingInterval>25</tt:EncodingInterval>" + "\r\n" +
        //"<tt:BitrateLimit>8192</tt:BitrateLimit>" + "\r\n" +
        //"</tt:RateControl>" + "\r\n" +
        //"<tt:H264><tt:GovLength>25</tt:GovLength>" + "\r\n" +
        //"<tt:H264Profile>Baseline</tt:H264Profile>" + "\r\n" +
        //"</tt:H264>" + "\r\n" +
        //"<tt:Multicast><tt:Address><tt:Type>IPv4</tt:Type>" + "\r\n" +
        //"<tt:IPv4Address >224.1.2.3</tt:IPv4Address >" + "\r\n" +
        //"</tt:Address>" + "\r\n" +
        //"<tt:Port>8600</tt:Port>" + "\r\n" +
        //"<tt:TTL>1</tt:TTL>" + "\r\n" +
        //"<tt:AutoStart>true</tt:AutoStart>" + "\r\n" +
        //"</tt:Multicast>" + "\r\n" +
        //"<tt:SessionTimeout>PT10S</tt:SessionTimeout>" + "\r\n" +
        //"</trt:Configuration>" + "\r\n" +
        //"</tt:ElementItem>" + "\r\n" +
        //"</tt:Data>" + "\r\n" +
        //"</tt:Message>" + "\r\n" +
        //"</wsnt:Message>" + "\r\n" +
        //"</wsnt:NotificationMessage>" + "\r\n" +
        //"</tev:PullMessagesResponse>" + "\r\n" +
        //"</soap:Body>" + "\r\n" +
        //"</soap:Envelope>" + "\r\n")]
        //Ticket #416
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket416)]
        #endregion //XmlReplySubstituteExtension for bugs

        #region XmlReplySubstituteExtension for bugs
        //QuickInstall-3-1-1 PullMessages response
//        [XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
//"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
//"<soap:Header>\r\n" +
//"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header>\r\n" +
//"<soap:Body>\r\n" +
//"<PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
//"<CurrentTime>2014-09-26T12:37:17.0536482Z</CurrentTime>\r\n" +
//"<TerminationTime>2014-09-26T12:37:57.0536482Z</TerminationTime>\r\n" +
//"<NotificationMessage xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
//"<Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:Monitoring/ProcessorUsage</Topic>\r\n" +
//"<ProducerReference>\r\n" +
//"<Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://localhost:3246/Events/PullPointServiceFake.asmx?param=value</Address></ProducerReference>\r\n" +
//"<Message>\r\n" +
//"<tt:Message UtcTime=\"2012-12-01T07:18:40.321\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" PropertyOperation=\"Initialized\">\r\n" +
//"<tt:Source>\r\n" +
//"<tt:SimpleItem Name=\"Token\" Value=\"Token1\" /></tt:Source>\r\n" +
//"<tt:Data>\r\n" +
        //"<tt:SimpleItem Name=\"Value\" Value=\"0.5\" /></tt:Data></tt:Message></Message></NotificationMessage></PullMessagesResponse></soap:Body></soap:Envelope>")]
        #endregion
        #region XmlReplySubstituteExtension for tickets
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.PullMessage_Hikvision_001)]
       // [XmlReplySubstituteExtension(ResponsesConst.ResponseForum1_PMResponse)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Event_PullMessagesResponseIncorrectResponseTag)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Event_PullMessagesResponseIncorrectNamespace)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1614_PullMessagesResponse_prefix_defined_in_Topic)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1614_PanasonicPullMessagesResponse_MotionAlarmEvent)]        
        #endregion
        public override System.DateTime PullMessages([System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string Timeout, int MessageLimit, [System.Xml.Serialization.XmlAnyElementAttribute()] System.Xml.XmlElement[] Any, out System.DateTime TerminationTime, [System.Xml.Serialization.XmlElementAttribute("NotificationMessage", Namespace = "http://docs.oasis-open.org/wsn/b-2")] out NotificationMessageHolderType[] NotificationMessage)
        {
            SoapHeaderProcessing(unknownHeaders);
            TestSuitInit();
            System.DateTime res;
            int timeOut;
            bool timeOutSpec;
            SoapException ex;

            StepType stepType = m_EventServiceTest.PMSPullMessagesTest(out res, out NotificationMessage, out TerminationTime, out ex, out timeOut, out timeOutSpec, Timeout, MessageLimit, Any);
            TimeoutProcessing(timeOutSpec, timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            actionHeader.actionValue = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse";


            return res;
        }

        [WebMethod]
        [SoapHeader("unknownHeaders")]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Renew", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("RenewResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        public override RenewResponse Renew([System.Xml.Serialization.XmlElementAttribute("Renew", Namespace = "http://docs.oasis-open.org/wsn/b-2")]Renew Renew1)
        {

            SoapHeaderProcessing(unknownHeaders);
            TestSuitInit();
            RenewResponse res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_EventServiceTest.SMSRenewTest(out res, out ex, out timeOut, Renew1);
            StepTypeProcessing(stepType, ex, timeOut);

            actionHeader.actionValue = "http://docs.oasis-open.org/wsn/bw-2/SubscriptionManager/RenewResponse";

            return res;
        }

        [WebMethod]
        [SoapHeader("unknownHeaders")]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/SetSynchronizationPoint", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetSynchronizationPoint()
        {
            SoapHeaderProcessing(unknownHeaders);
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_EventServiceTest.PMSSetSynchronizationPointTest(out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            actionHeader.actionValue = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/SetSynchronizationPointResponse";

        }

        [WebMethod]
        [SoapHeader("unknownHeaders")]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Unsubscribe", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("UnsubscribeResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        public UnsubscribeResponse Unsubscribe([System.Xml.Serialization.XmlElementAttribute("Unsubscribe", Namespace = "http://docs.oasis-open.org/wsn/b-2")] Unsubscribe Unsubscribe1)
        {
            SoapHeaderProcessing(unknownHeaders);
            TestSuitInit();
            UnsubscribeResponse res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_EventServiceTest.SMSUnsubscribeTest(out res, out ex, out timeOut, Unsubscribe1);
            StepTypeProcessing(stepType, ex, timeOut);

            actionHeader.actionValue = "http://docs.oasis-open.org/wsn/bw-2/SubscriptionManager/UnsubscribeResponse";

            return res;
        }

        public override void Seek(DateTime UtcTime, bool Reverse, bool ReverseSpecified, XmlElement[] Any)
        {
            throw new NotImplementedException();
        }
    }
}
