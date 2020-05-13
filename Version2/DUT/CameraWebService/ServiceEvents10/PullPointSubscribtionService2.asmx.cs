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
    public class PullPointSubscriptionService2 : PullPointSubscriptionBinding
    {
        //TestSuit
        PullPointSubscriptionServiceTest PullPointSubscriptionServiceTest
        {
            get
            {
                if (Application[Base.AppVars.EVENTPULLPOINTSERVICE] != null)
                {
                    return (PullPointSubscriptionServiceTest)Application[Base.AppVars.EVENTPULLPOINTSERVICE];
                }
                else
                {
                    PullPointSubscriptionServiceTest serviceTest = new PullPointSubscriptionServiceTest(TestCommon);
                    Application[Base.AppVars.EVENTPULLPOINTSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }

        [WebMethod]
        [SoapHeader("unknownHeaders")]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/PullMessages", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        #region XmlReplySubstituteExtension for testing
        //TC.EVENT-6-1-1.18 (current time after termination time)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Header><a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header><soap:Body><PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\"><TerminationTime>2013-09-26T08:39:09.4852132Z</TerminationTime><CurrentTime>2013-09-26T08:38:39.4852132Z</CurrentTime><NotificationMessage xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\"><Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:EventBuffer/Begin</Topic><Message><tt:Message UtcTime=\"2013-09-26T08:13:39.4852132Z\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" /></Message></NotificationMessage></PullMessagesResponse></soap:Body></soap:Envelope>")]
        //TC.EVENT-6-1-2.18 (current time after termination time)
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Header><a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header><soap:Body><PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\"><TerminationTime>2013-09-26T08:39:09.4852132Z</TerminationTime><CurrentTime>2013-09-26T08:38:39.4852132Z</CurrentTime><NotificationMessage xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\"><Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:EventBuffer/Begin</Topic><Message><tt:Message UtcTime=\"2013-09-26T08:13:39.4852132Z\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" /></Message></NotificationMessage></PullMessagesResponse></soap:Body></soap:Envelope>")]
        #endregion //XmlReplySubstituteExtension for testing        
        [return: System.Xml.Serialization.XmlElementAttribute("CurrentTime")]
        
        //[XmlReplySubstituteExtension(ResponsesConst.ResponseTicket1307_ProfileChangedEvent)]
       // [XmlReplySubstituteExtension(ResponsesConst.ResponseForum1_PMResponse)]
        public override System.DateTime PullMessages([System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string Timeout, int MessageLimit, [System.Xml.Serialization.XmlAnyElementAttribute()] System.Xml.XmlElement[] Any, out System.DateTime TerminationTime, [System.Xml.Serialization.XmlElementAttribute("NotificationMessage", Namespace = "http://docs.oasis-open.org/wsn/b-2")] out NotificationMessageHolderType[] NotificationMessage)
        {

            SoapHeaderProcessing(unknownHeaders);
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Timeout", Timeout);
            validation.Add(ParameterType.Int, "MessageLimit", MessageLimit);
            System.DateTime currentTime = PullPointSubscriptionServiceTest.TakeCurrentTime();
            System.DateTime terminationTime = PullPointSubscriptionServiceTest.TakeTerminationTime(currentTime);
            

            NotificationMessage = (NotificationMessageHolderType[])ExecuteGetCommand(validation, PullPointSubscriptionServiceTest.PullMessagesTest);

            TerminationTime = terminationTime;


            actionHeader.actionValue = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse";

            return currentTime;
        }

        [WebMethod]
        [SoapHeader("unknownHeaders")]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Renew", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("RenewResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        public override RenewResponse Renew([System.Xml.Serialization.XmlElementAttribute("Renew", Namespace = "http://docs.oasis-open.org/wsn/b-2")]Renew Renew1)
        {
            SoapHeaderProcessing(unknownHeaders);
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.Log, "TerminationTime", Renew1.TerminationTime);
            ExecuteVoidCommand(validation, PullPointSubscriptionServiceTest.RenewTest);

            RenewResponse result = new RenewResponse();
            result.CurrentTimeSpecified = true;
            result.CurrentTime = PullPointSubscriptionServiceTest.TakeCurrentTime();
            result.TerminationTime = PullPointSubscriptionServiceTest.TakeTerminationTime(result.CurrentTime);

            actionHeader.actionValue = "http://docs.oasis-open.org/wsn/bw-2/SubscriptionManager/RenewResponse";

            return result;
        }

        [WebMethod]
        [SoapHeader("unknownHeaders")]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/SetSynchronizationPoint", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetSynchronizationPoint()
        {
            SoapHeaderProcessing(unknownHeaders);

            ParametersValidation validation = new ParametersValidation();
            ExecuteVoidCommand(validation, PullPointSubscriptionServiceTest.SetSynchronizationPointTest);

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
            ParametersValidation validation = new ParametersValidation();
            UnsubscribeResponse result = (UnsubscribeResponse)ExecuteGetCommand(validation, PullPointSubscriptionServiceTest.UnsubscribeTest);

            actionHeader.actionValue = "http://docs.oasis-open.org/wsn/bw-2/SubscriptionManager/UnsubscribeResponse";

            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [SoapHeader("unknownHeaders")]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/SeekRequest", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void Seek(System.DateTime UtcTime, bool Reverse, [System.Xml.Serialization.XmlIgnoreAttribute()] bool ReverseSpecified, [System.Xml.Serialization.XmlAnyElementAttribute()] System.Xml.XmlElement[] Any)
        {
            SoapHeaderProcessing(unknownHeaders);

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.Log, "UtcTime", UtcTime);
            validation.Add(ParameterType.OptionalBool, "Reverse", Reverse, ReverseSpecified);
            ExecuteVoidCommand(validation, PullPointSubscriptionServiceTest.SeekTest);

            actionHeader.actionValue = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/SeekResponse";

        }
    }
}
