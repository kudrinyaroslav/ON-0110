using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using CameraWebService.Notification;

namespace CameraWebService.Events
{

    /// <summary>
    /// Summary description for EventsServiceFake
    /// </summary>
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/events/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class EventsServiceFake : WebService
    {
        public ActionHeader actionHeader;
        public WsaToHeader wsaToHeader;
        public RelatesToHeader relatesToHeader;
        public ReplyToHeader replyToHeader;
        public MessageIdHeader messageIdHeader;

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/GetEventProperties", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("TopicNamespaceLocation", DataType = "anyURI")]
        [HeaderHandling()]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.InOut)]
        [SoapHeader("wsaToHeader", Direction = SoapHeaderDirection.InOut)]
        [SoapHeader("relatesToHeader", Direction = SoapHeaderDirection.InOut)]
        [SoapHeader("messageIdHeader", Direction = SoapHeaderDirection.In)]
        [SoapHeader("replyToHeader", Direction = SoapHeaderDirection.In)]
        public string[] GetEventProperties(
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")] out bool FixedTopicSet, 
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://docs.oasis-open.org/wsn/t-1")] out Events.TopicSetType TopicSet,
            [System.Xml.Serialization.XmlElementAttribute("TopicExpressionDialect", Namespace = "http://docs.oasis-open.org/wsn/b-2", DataType = "anyURI")] out string[] TopicExpressionDialect, 
            [System.Xml.Serialization.XmlElementAttribute("MessageContentFilterDialect", DataType = "anyURI")] out string[] MessageContentFilterDialect, 
            [System.Xml.Serialization.XmlElementAttribute("ProducerPropertiesFilterDialect", DataType = "anyURI")] out string[] ProducerPropertiesFilterDialect, 
            [System.Xml.Serialization.XmlElementAttribute("MessageContentSchemaLocation", DataType = "anyURI")] out string[] MessageContentSchemaLocation, 
            [System.Xml.Serialization.XmlAnyElementAttribute()] out System.Xml.XmlElement[] Any)
        {

            if (actionHeader == null)
            {
                actionHeader = new ActionHeader();
            }
            actionHeader.Value = "http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse";

            
            if (messageIdHeader != null)
            {
                relatesToHeader = new RelatesToHeader();
                relatesToHeader.Value = messageIdHeader.Value;
                //relatesToHeader.Value = new System.Xml.UniqueId(Guid.Empty).ToString();
                //relatesToHeader.Value = new System.Xml.UniqueId(Guid.NewGuid()).ToString();
                //relatesToHeader.Value = "Not a GUID at all";
            }

            if (replyToHeader != null)
            {
                wsaToHeader = new WsaToHeader();
                wsaToHeader.Value = replyToHeader.Value;
            }

            /*
            SoapFaultSubCode subCode =
                new SoapFaultSubCode(new XmlQualifiedName("ResourseUnknown1", "http://www.onvif.org/ver10/error"));

            SoapException exception = new SoapException("Invalid Argument",
                                                        new XmlQualifiedName("Sender",
                                                                             "http://www.w3.org/2003/05/soap-envelope"),
                                                        subCode);
            throw exception;

            FixedTopicSet = true;
            TopicSet = new TopicSetType();
            TopicSet.Any = new XmlElement[1];
            TopicExpressionDialect = null;
            MessageContentFilterDialect = null;
            MessageContentSchemaLocation = null;
            ProducerPropertiesFilterDialect = null;
            Any = null;
            return null;
            */

            FixedTopicSet = true;
            TopicSet = new Events.TopicSetType();

            string concreteTopic = "http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete";
            string concreteSetTopic = "http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";

            TopicExpressionDialect = new string[] { concreteTopic, concreteSetTopic };

            string mandatoryDialect = "http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter";

            MessageContentFilterDialect = new string[] { mandatoryDialect };
            ProducerPropertiesFilterDialect = new string[] { "ProducerPropertiesFilterDialect" };
            MessageContentSchemaLocation = new string[] {"MessageContentSchemaLocation"};
            
            XmlDocument doc = new XmlDocument();

            List<XmlElement> elements = new List<XmlElement>();
            foreach (string dialect in TopicExpressionDialect)
            {
                XmlElement element = doc.CreateElement("wsnt", "TopicExpressionDialect",
                                                       "http://docs.oasis-open.org/wsn/b-2");
                element.InnerText = dialect;
                elements.Add(element);

            }

            Any = elements.ToArray();

            TopicSet = new TopicSetType();

            XmlDocument doc1 = new XmlDocument();
            doc1.LoadXml("<tns1:RuleEngine xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" wstop:topic=\"true\"> <tns1:LineDetector wstop:topic=\"true\"><tns1:Crossed wstop:topic=\"true\"> <tt:MessageDescription IsProperty=\"true\"><tt:Source> <tt:SimpleItem Name=\"VideoSourceConfigurationToken\" Type=\"tt:ReferenceToken\"/><tt:SimpleItem Name=\"VideoAnalyticsConfigurationToken\" Type=\"tt:ReferenceToken\"/></tt:Source> <tt:Data> <tt:SimpleItem Name=\"ObjectId\" Type=\"tt:ObjectRefType\"/></tt:Data> </tt:MessageDescription> </tns1:Crossed> </tns1:LineDetector> </tns1:RuleEngine>");

            XmlElement topic1 = doc.CreateElement("tns1:RuleEngine", "http://www.onvif.org/ver10/topics");
            //XmlAttribute attr1 = doc.CreateAttribute("wstop:topic", "http://docs.oasis-open.org/wsn/t-1");
            //attr1.Value = "true";
            //topic1.Attributes.Append(attr1);

            XmlElement topic2 = doc.CreateElement("tns1:LineDetector", "http://www.onvif.org/ver10/topics");
            XmlAttribute attr2 = doc.CreateAttribute("wstop:topic", "http://docs.oasis-open.org/wsn/t-1");
            attr2.Value = "true";
            topic2.Attributes.Append(attr2);

            topic1.AppendChild(topic2);

            TopicSet.Any = new XmlElement[] { topic1, doc1.DocumentElement};

            return new string[] { "http://www.onvif.org/onvif/ver10/topics/topicns.xml" };
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Subscribe", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [HeaderHandling()]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.InOut)]
        [ScriptDriven]
        [return: System.Xml.Serialization.XmlElementAttribute("SubscribeResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        public SubscribeResponse Subscribe([System.Xml.Serialization.XmlElementAttribute("Subscribe", Namespace = "http://docs.oasis-open.org/wsn/b-2")] Subscribe Subscribe1)
        {

            //SoapFaultSubCode subCode =
            //    new SoapFaultSubCode(new XmlQualifiedName("ResourseUnknown1", "http://www.onvif.org/ver10/error"));

            //SoapException exception = new SoapException("Invalid Argument",
            //                                            new XmlQualifiedName("Sender",
            //                                                                 "http://www.w3.org/2003/05/soap-envelope"),
            //                                            subCode);
            //throw exception;

            Application["consumer"] = Subscribe1.ConsumerReference;

            SubscribeResponse response = new SubscribeResponse();

            response.CurrentTimeSpecified = true;
            response.CurrentTime = System.DateTime.Now;
            response.TerminationTimeSpecified = true;
            response.TerminationTime = response.CurrentTime.AddSeconds(300);

            response.SubscriptionReference = new Events.EndpointReferenceType();
            response.SubscriptionReference.Address = new Events.AttributedURIType();
            response.SubscriptionReference.Address.Value = HttpContext.Current.Request.Url.AbsoluteUri + "?param=value";

            


            if (actionHeader == null)
            {
                actionHeader = new ActionHeader();
            }
            actionHeader.Value = "http://docs.oasis-open.org/wsn/bw-2/NotificationProducer/SubscribeResponse";



            return response;
        }


        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Renew", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("RenewResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        [ScriptDriven]
        [HeaderHandling()]
        public RenewResponse Renew([System.Xml.Serialization.XmlElementAttribute("Renew", Namespace = "http://docs.oasis-open.org/wsn/b-2")] Renew Renew1)
        {
            if (Application["consumer"] == null)
            {
                SoapFaultSubCode subCode =
                    new SoapFaultSubCode(new XmlQualifiedName("ResourseUnknown1", "http://www.onvif.org/ver10/error"));

                SoapException exception = new SoapException("Invalid Argument",
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

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Unsubscribe", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("UnsubscribeResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        [HeaderHandling()]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.InOut)]
        [ScriptDriven]
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

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(
            "http://www.onvif.org/ver10/events/wsdl/SetSynchronizationPoint", 
            RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", 
            ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", 
            Use = System.Web.Services.Description.SoapBindingUse.Literal, 
            ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [HeaderHandling()]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.InOut)]
        public void SetSynchronizationPoint()
        {
            System.Threading.Thread thread = new Thread(new ThreadStart(DoNotify));
            thread.Start();

            if (actionHeader == null)
            {
                actionHeader = new ActionHeader();
            }
            actionHeader.Value = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/SetSynchronizationPointResponse";

        }

        void DoNotify()
        {
            Thread.Sleep(5000);

            Events.EndpointReferenceType reference = (Events.EndpointReferenceType)Application["consumer"];

            Notification.NotificationProvider provider = new NotificationProvider();

            //string address = "http://192.168.31.142:8080//onvif_notify_server/";
            string address = reference.Address.Value;
            provider.Notify(address);
        }

        void DoNotify1()
        {

            Events.EndpointReferenceType reference = (Events.EndpointReferenceType)Application["consumer"];

            NotificationConsumerProxy.NotificationConsumerClient client = new
                NotificationConsumerProxy.NotificationConsumerClient(new WSHttpBinding(), new EndpointAddress(reference.Address.Value));

            NotificationConsumerProxy.Notify notify = new NotificationConsumerProxy.Notify();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<tt:Message UtcTime=\"2008-10-10T12:24:57.321\" xmlns:tt=\"http://www.onvif.org/ver10/schema\"><tt:Source><tt:SimpleItem Name=\"VideoSourceConfigurationToken\" Value=\"1\"/><tt:SimpleItem Name=\"VideoAnalyticsConfigurationToken\" Value=\"2\"/><tt:SimpleItem Value=\"MyImportantFence1\" Name=\"Rule\"/></tt:Source><tt:Data><tt:SimpleItem Name=\"ObjectId\" Value=\"15\" /></tt:Data></tt:Message>");

            NotificationMessageHolderType notification1 = new NotificationMessageHolderType();

            XmlElement message1 = doc.DocumentElement;

            notification1.Message = message1;

            notification1.ProducerReference = new EndpointReferenceType();
            notification1.ProducerReference.Address = new AttributedURIType();
            notification1.ProducerReference.Address.Value = HttpContext.Current.Request.Url.AbsoluteUri;

            notification1.Topic = new TopicExpressionType();
            notification1.Topic.Dialect = "";

            notify.NotificationMessage = new NotificationMessageHolderType[] { notification1 };

            client.Notify(notify);

        }
        
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/CreatePullPointSubscription", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SubscriptionReference")]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.InOut)]
        [HeaderHandling()]
        [ScriptDriven]
        public EndpointReferenceType CreatePullPointSubscription(Events.FilterType Filter, 
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string InitialTerminationTime, 
            CreatePullPointSubscriptionSubscriptionPolicy SubscriptionPolicy, 
            [System.Xml.Serialization.XmlAnyElementAttribute()] ref System.Xml.XmlElement[] Any, 
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")] out System.DateTime CurrentTime, [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2", IsNullable = true)] out System.Nullable<System.DateTime> TerminationTime)
        {
            //SoapFaultSubCode subCode =
            //    new SoapFaultSubCode(new XmlQualifiedName("ResourseUnknown1", "http://www.onvif.org/ver10/error"));

            //SoapException exception = new SoapException("Invalid Argument",
            //                                            new XmlQualifiedName("Sender",
            //                                                                 "http://www.w3.org/2003/05/soap-envelope"),
            //                                            subCode);
            //throw exception;


            Application["consumer"] = "http://127.0.0.1:8080";

            Events.EndpointReferenceType endpointReferenceType = new Events.EndpointReferenceType();
            
            endpointReferenceType.Address = new Events.AttributedURIType();
            endpointReferenceType.Address.Value = string.Format("http://{0}/Events/PullPointServiceFake.asmx?param=value", HttpContext.Current.Request.Url.Authority);
            //endpointReferenceType.Address.Value = string.Format("http://{0}/Events/PullPointWcfService.svc?param=value", HttpContext.Current.Request.Url.Host);

            CurrentTime = System.DateTime.Now;
            TerminationTime = CurrentTime.AddSeconds(60);

            if (actionHeader == null)
            {
                actionHeader = new ActionHeader();
            }
            actionHeader.Value = "http://www.onvif.org/ver10/events/wsdl/EventPortType/CreatePullPointSubscriptionResponse";

            return endpointReferenceType;
        }


        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/EventPortType/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [HeaderHandling()]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.InOut)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public  Capabilities GetServiceCapabilities()
        {
            if (actionHeader == null)
            {
                actionHeader = new ActionHeader();
            }

            // Set the value of the SoapHeader returned
            // to the client.

            actionHeader.Value = "Some strange invalid action";

            Capabilities capabilities = new Capabilities();

            capabilities.WSPausableSubscriptionManagerInterfaceSupport = true;
            capabilities.WSPausableSubscriptionManagerInterfaceSupportSpecified = true;
            capabilities.WSPullPointSupport = true;
            capabilities.WSPullPointSupportSpecified = true;

            return capabilities;
        }

        void InitHeader(WsaTextHeader header, string value)
        {
            if (header == null)
            {
                header = new ActionHeader(); 
            }
            header.Value = value;
        }

    }
}

/*
[XmlReplySubstituteExtension("    <soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\"> "+
"<soap:Header><wsa:Action>http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</wsa:Action></soap:Header>" + 
"<soap:Body>"+
"<tev:GetEventPropertiesResponse>"+
  "<tev:TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</tev:TopicNamespaceLocation>"+
  "<wsnt:FixedTopicSet>true</wsnt:FixedTopicSet>"+
  "<wstop:TopicSet>"+
    "<tns1:MediaControl wstop:topic=\"true\">"+
      "<tns1:VideoSourceConfiguration wstop:topic=\"true\">"+
        "<tt:MessageDescription IsProperty=\"true\">"+
          "<tt:Source><tt:SimpleItemDescription Name=\"VideoSourceConfigurationToken\" Type=\"tt:ReferenceToken\"/></tt:Source>"+
          "<tt:Data><tt:SimpleItemDescription Name=\"Config\" Type=\"tt:VideoSourceConfiguration\"/></tt:Data>"+
        "</tt:MessageDescription>"+
      "</tns1:VideoSourceConfiguration>"+
      "<tns1:VideoEncoderConfiguration wstop:topic=\"true\">"+
        "<tt:MessageDescription IsProperty=\"true\">"+
          "<tt:Source>"+
            "<tt:SimpleItemDescription Name=\"VideoEncoderConfigurationToken\" Type=\"tt:ReferenceToken\"/>"+
          "</tt:Source>"+
          "<tt:Data>"+
            "<tt:SimpleItemDescription Name=\"Config\" Type=\"tt:VideoEncoderConfiguration\"/>"+
          "</tt:Data>"+
        "</tt:MessageDescription>"+
      "</tns1:VideoEncoderConfiguration>"+
    "</tns1:MediaControl>"+
    "<tns1:VideoSource wstop:topic=\"true\">\r\n"+
      "<tnshik:SignalLoss wstop:topic=\"true\">"+
        "<tt:MessageDescription>"+
          "<tt:Source>"+
            "<tt:SimpleItemDescription Name=\"VideoSourceToken\" Type=\"tt:ReferenceToken\"/>"+
          "</tt:Source>"+
          "<tt:Data>"+
            "<tt:SimpleItemDescription Name=\"State\" Type=\"xs:boolean\"/>"+
          "</tt:Data>"+
        "</tt:MessageDescription>"+
      "</tnshik:SignalLoss>"+
      "<tnshik:SignalStandardMismatch wstop:topic=\"true\">"+
        "<tt:MessageDescription>"+
          "<tt:Source>"+
            "<tt:SimpleItemDescription Name=\"VideoSourceToken\" Type=\"tt:ReferenceToken\"/>"+
          "</tt:Source>"+
          "<tt:Data>"+
            "<tt:SimpleItemDescription Name=\"State\" Type=\"xs:boolean\"/>"+
          "</tt:Data>"+
        "</tt:MessageDescription>"+
      "</tnshik:SignalStandardMismatch>"+
    "</tns1:VideoSource>"+
    "<tns1:UserAlarm wstop:topic=\"true\">"+
      "<tnshik:IllegalAccess wstop:topic=\"true\"/>"+
    "</tns1:UserAlarm>"+
  "</wstop:TopicSet>"+
  "<wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect>"+
  "<wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect>"+
  "<tev:MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</tev:MessageContentFilterDialect>"+
  "<tev:MessageContentSchemaLocation>http://www.onvif.org/onvif/ver10/schema/onvif.xsd</tev:MessageContentSchemaLocation>" +
"</tev:GetEventPropertiesResponse>"+
"</soap:Body>"+
"</soap:Envelope>  ")]
 
 
 */