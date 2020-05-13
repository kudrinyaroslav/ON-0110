using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using CameraWebService.Events;
using System.ServiceModel;

namespace CameraWebService
{
    /// <summary>
    /// Summary description for PullPointServiceFake
    /// </summary>
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/events/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [ServiceContract]
    public class PullPointServiceFake : System.Web.Services.WebService
    {
        // Receive all SOAP headers.
        public SoapUnknownHeader[] unknownHeaders;

        private void SoapHeaderProcessing(SoapUnknownHeader[] unknownHeaders)
        {
            foreach (SoapUnknownHeader header in unknownHeaders)
            {
                header.DidUnderstand = true;
            }
        }

        public ActionHeader actionHeader = new ActionHeader();
               
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/PullMessages", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CurrentTime")]
        [SoapHeader("unknownHeaders", Direction = SoapHeaderDirection.In)]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [ScriptDriven()]
           public System.DateTime PullMessages(
            [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string Timeout, 
            int MessageLimit, 
            [System.Xml.Serialization.XmlAnyElementAttribute()] System.Xml.XmlElement[] Any, 
            out System.DateTime TerminationTime, 
            [System.Xml.Serialization.XmlElementAttribute("NotificationMessage", Namespace = "http://docs.oasis-open.org/wsn/b-2")] out NotificationMessageHolderType[] NotificationMessage)
        {
            if (Timeout == "PT24H")
            {
                SoapFaultSubCode subCode =
                    new SoapFaultSubCode(new XmlQualifiedName("ShitHappens", "http://www.onvif.org/ver10/error"));

                SoapException exception = new SoapException("Invalid Argument",
                                                            new XmlQualifiedName("Sender",
                                                                                 "http://www.w3.org/2003/05/soap-envelope"),
                                                            subCode);
                throw exception;

            }

            //if (Timeout == "PT20S")
            //{
            //    PullMessagesFaultResponseType details = new PullMessagesFaultResponseType();
            //    details.MaxMessageLimit = 5;
            //    details.MaxTimeout = "PT15S";

            //    FaultException<PullMessagesFaultResponseType> exception = new FaultException<PullMessagesFaultResponseType>(details, "InvalidArg", new FaultCode("Sender", "http://www.w3.org/2003/05/soap-envelope"));
            //    throw exception;
            //}

            string varName = "PullMessagesCounter";
            int cnt = 0;
            if (Application.AllKeys.Contains(varName))
            { 
                cnt = (int)Application[varName];
            }

            if (cnt % 2 == 0)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<tt:Message UtcTime=\"2008-10-10T12:24:57.321\" xmlns:tt=\"http://www.onvif.org/ver10/schema\"><tt:Source><tt:SimpleItem Name=\"RecordingJobToken\" Value=\"1\"/></tt:Source><tt:Data><tt:SimpleItem Name=\"State\" Value=\"Idle\" /><tt:ElementItem Name=\"Information\"><tt:RecordingJobStateInformation><tt:RecordingToken>Recording1</tt:RecordingToken><tt:State>Active</tt:State></tt:RecordingJobStateInformation></tt:ElementItem></tt:Data></tt:Message>");

                NotificationMessageHolderType notification1 = CreateMessage(doc,
                    "tns1",
                    "http://www.onvif.org/ver10/topics",
                    "RecordingConfig/JobState");

                //NotificationMessageHolderType notification2 = CreateMessage(doc, "tns1", "PropertyTopic2"); 

                NotificationMessage = new NotificationMessageHolderType[] { notification1 /*, notification2*/};
            }
            else
            {
                NotificationMessage = new NotificationMessageHolderType[] { };
            }
            cnt++;
            Application[varName] = cnt;
                            
            TerminationTime = System.DateTime.Now.AddMinutes(3);
            if (actionHeader == null)
            {
                actionHeader = new ActionHeader();
            }
            actionHeader.Value = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse";
                        
            return System.DateTime.Now.AddSeconds(10);
        }

        NotificationMessageHolderType CreateMessage(XmlDocument doc, string prefix, string ns, string topic)
        {
            NotificationMessageHolderType notification = new NotificationMessageHolderType();

            XmlElement message = doc.DocumentElement;
            notification.Message = message;
            notification.ProducerReference = new EndpointReferenceType();
            notification.ProducerReference.Address = new AttributedURIType();
            notification.ProducerReference.Address.Value = HttpContext.Current.Request.Url.AbsoluteUri;
            notification.Topic = new TopicExpressionType();
            notification.Topic.Dialect = "";

            XmlAttribute propertyOperation = doc.CreateAttribute("PropertyOperation");
            propertyOperation.Value = "Initialized";
            message.Attributes.Append(propertyOperation);


            XmlText text;
            notification.Topic.Xmlns = new System.Xml.Serialization.XmlSerializerNamespaces();
            notification.Topic.Xmlns.Add(prefix, ns);
            text = doc.CreateTextNode(string.Format("{0}:{1}", prefix, topic));

            notification.Topic.Any = new XmlNode[]{text};

            return notification;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/SetSynchronizationPoint", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [SoapHeader("unknownHeaders", Direction = SoapHeaderDirection.In)]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [HeaderHandling()]
        public void SetSynchronizationPoint()
        {
            if (actionHeader == null)
            {
                actionHeader = new ActionHeader();
            }
            actionHeader.Value = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/SetSynchronizationPointResponse";

        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Unsubscribe", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("UnsubscribeResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        [SoapHeader("unknownHeaders", Direction = SoapHeaderDirection.In)]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [HeaderHandling()]
        public UnsubscribeResponse Unsubscribe([System.Xml.Serialization.XmlElementAttribute("Unsubscribe", Namespace = "http://docs.oasis-open.org/wsn/b-2")] Unsubscribe Unsubscribe1)
        {
            if (Application["consumer"] == null)
            {
                SoapFaultSubCode subCode =
                    new SoapFaultSubCode(new XmlQualifiedName("ResourseUnknown", "http://www.onvif.org/ver10/error"));

                SoapException exception = new SoapException("Invalid Argument",
                                                            new XmlQualifiedName("Sender",
                                                                                 "http://www.w3.org/2003/05/soap-envelope"),
                                                            subCode);
                throw exception;
            }

            Application["consumer"] = null;
            UnsubscribeResponse response = new UnsubscribeResponse();

            if (actionHeader == null)
            {
                actionHeader = new ActionHeader();
            }
            actionHeader.Value = "http://docs.oasis-open.org/wsn/bw-2/SubscriptionManager/UnsubscribeResponse";


            return response;
        }
        
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Renew", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("RenewResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        [SoapHeader("unknownHeaders", Direction = SoapHeaderDirection.In)]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [ScriptDriven()]
        public RenewResponse Renew([System.Xml.Serialization.XmlElementAttribute("Renew", Namespace = "http://docs.oasis-open.org/wsn/b-2")] Renew Renew1)
        {
            if (Application["consumer"] == null)
            {
                SoapFaultSubCode subCode =
                    new SoapFaultSubCode(new XmlQualifiedName("ResourseUnknown", "http://www.onvif.org/ver10/error"));

                SoapException exception = new SoapException("Invalid Argument1",
                                                            new XmlQualifiedName("Sender",
                                                                                 "http://www.w3.org/2003/05/soap-envelope"),
                                                            subCode);
                throw exception;
            }


            RenewResponse response = new RenewResponse();

            response.CurrentTime = System.DateTime.Now;
            response.CurrentTimeSpecified = true;
            response.TerminationTime = response.CurrentTime.AddSeconds(15);
            
            return response;

        }

    }
}



/*
 * 
 Possible replacement for PullMessages 
  
 [XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" +
                "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\"><soap:Header><wsa:Action>http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</wsa:Action>" + "\r\n" +
                "</soap:Header>" + "\r\n" +
                "<soap:Body><tev:PullMessagesResponse><tev:CurrentTime>2012-12-16T16:36:31Z</tev:CurrentTime>" + "\r\n" +
                "<tev:TerminationTime>2012-12-16T16:36:51Z</tev:TerminationTime>" + "\r\n" +
                "<wsnt:NotificationMessage><wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:MediaControl/VideoSourceConfiguration</wsnt:Topic>" + "\r\n" +
                "<wsnt:Message><tt:Message UtcTime=\"2012-12-16T16:36:31Z\" PropertyOperation=\"Initialized\"><tt:Source><tt:SimpleItem Name=\"VideoSourceConfigurationToken\" Value=\"VideoSourceToken\"/>" + "\r\n" +
                "</tt:Source>" + "\r\n" +
                "<tt:Data><tt:ElementItem Name=\"Config\"><trt:Configuration token=\"VideoSourceToken\"><tt:Name>VideoSourceConfig</tt:Name>" + "\r\n" +
                "<tt:UseCount>0</tt:UseCount>" + "\r\n" +
                "<tt:SourceToken>VideoSource_1</tt:SourceToken>" + "\r\n" +
                "<tt:Bounds x=\"0\" y=\"0\" width=\"2560\" height=\"1920\"></tt:Bounds>" + "\r\n" +
                "</trt:Configuration>" + "\r\n" +
                "</tt:ElementItem>" + "\r\n" +
                "</tt:Data>" + "\r\n" +
                "</tt:Message>" + "\r\n" +
                "</wsnt:Message>" + "\r\n" +
                "</wsnt:NotificationMessage>" + "\r\n" +
                "<wsnt:NotificationMessage><wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:MediaControl/VideoEncoderConfiguration</wsnt:Topic>" + "\r\n" +
                "<wsnt:Message><tt:Message UtcTime=\"2012-12-16T16:36:31Z\" PropertyOperation=\"Initialized\"><tt:Source><tt:SimpleItem Name=\"VideoEncoderConfigurationToken\" Value=\"VideoEncoderToken_1\"/>" + "\r\n" +
                "</tt:Source>" + "\r\n" +
                "<tt:Data><tt:ElementItem Name=\"Config\"><trt:Configuration token=\"VideoEncoderToken_1\"><tt:Name>VideoEncoder_1</tt:Name>" + "\r\n" +
                "<tt:UseCount>0</tt:UseCount>" + "\r\n" +
                "<tt:Encoding>H264</tt:Encoding>" + "\r\n" +
                "<tt:Resolution><tt:Width>1920</tt:Width>" + "\r\n" +
                "<tt:Height>1080</tt:Height>" + "\r\n" +
                "</tt:Resolution>" + "\r\n" +
                "<tt:Quality>1.000000</tt:Quality>" + "\r\n" +
                "<tt:RateControl><tt:FrameRateLimit>0</tt:FrameRateLimit>" + "\r\n" +
                "<tt:EncodingInterval>25</tt:EncodingInterval>" + "\r\n" +
                "<tt:BitrateLimit>8192</tt:BitrateLimit>" + "\r\n" +
                "</tt:RateControl>" + "\r\n" +
                "<tt:H264><tt:GovLength>25</tt:GovLength>" + "\r\n" +
                "<tt:H264Profile>Baseline</tt:H264Profile>" + "\r\n" +
                "</tt:H264>" + "\r\n" +
                "<tt:Multicast><tt:Address><tt:Type>IPv4</tt:Type>" + "\r\n" +
                "<tt:IPv4Address >224.1.2.3</tt:IPv4Address >" + "\r\n" +
                "</tt:Address>" + "\r\n" +
                "<tt:Port>8600</tt:Port>" + "\r\n" +
                "<tt:TTL>1</tt:TTL>" + "\r\n" +
                "<tt:AutoStart>true</tt:AutoStart>" + "\r\n" +
                "</tt:Multicast>" + "\r\n" +
                "<tt:SessionTimeout>PT10S</tt:SessionTimeout>" + "\r\n" +
                "</trt:Configuration>" + "\r\n" +
                "</tt:ElementItem>" + "\r\n" +
                "</tt:Data>" + "\r\n" +
                "</tt:Message>" + "\r\n" +
                "</wsnt:Message>" + "\r\n" +
                "</wsnt:NotificationMessage>" + "\r\n" +
                "</tev:PullMessagesResponse>" + "\r\n" +
                "</soap:Body>" + "\r\n" +
                "</soap:Envelope>" + "\r\n")]*/