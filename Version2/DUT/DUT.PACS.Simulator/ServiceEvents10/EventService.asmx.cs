using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace DUT.PACS.Simulator.Events10
{
    // Define a SOAP header by deriving from the SoapHeader class.

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
    [System.Xml.Serialization.XmlRootAttribute("Action", Namespace = "http://www.w3.org/2005/08/addressing", IsNullable = false)]
    public class ActionHeader : SoapHeader
    {
        public ActionHeader()
        {
            actionValue = "http://docs.oasis-open.org/wsrf/fault";
            xmlns = new System.Xml.Serialization.XmlSerializerNamespaces();
            xmlns.Add("a", "http://www.w3.org/2005/08/addressing");
            MustUnderstand = true;
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string actionValue;

        private System.Xml.Serialization.XmlSerializerNamespaces xmlns;

        [System.Xml.Serialization.XmlNamespaceDeclarations]
        public System.Xml.Serialization.XmlSerializerNamespaces Xmlns
        {
            get { return xmlns; }
            set { xmlns = value; }
        }
    }


    /// <summary>
    /// Summary description for EventService
    /// </summary>
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/events/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class EventService : EventBinding
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
        

        [WebMethod]
        [SoapHeader("unknownHeaders", Direction=SoapHeaderDirection.In)]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        //[SoapHeader("actionFaultHeader", Direction = SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/CreatePullPointSubscription", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SubscriptionReference")]
        [RequestListenerExtension]
        public override EndpointReferenceType CreatePullPointSubscription(FilterType Filter, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string InitialTerminationTime, CreatePullPointSubscriptionSubscriptionPolicy SubscriptionPolicy, [System.Xml.Serialization.XmlAnyElementAttribute()] ref System.Xml.XmlElement[] Any, [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")] out System.DateTime CurrentTime, [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2", IsNullable = true)] out System.Nullable<System.DateTime> TerminationTime)
        {
            SoapHeaderProcessing(unknownHeaders);
            ConfStorageLoad();
            EventServerLoad();

            EndpointReferenceType res = new EndpointReferenceType();

            DateTime terminationTime;
            bool nullInitialTerminationTime = false;

            if (InitialTerminationTime == null)
            {
                InitialTerminationTime = "PT10S";
                nullInitialTerminationTime = true;
            }

            try
            {
                TimeSpan timeSpan = System.Xml.XmlConvert.ToTimeSpan(InitialTerminationTime);
                terminationTime = DateTime.UtcNow.Add(timeSpan.Add(new TimeSpan(0, 0, 1)));
            }
            catch (Exception)
            {
                try
                {
                    terminationTime = System.Xml.XmlConvert.ToDateTime(InitialTerminationTime, XmlDateTimeSerializationMode.Utc);
                }
                catch (Exception)
                {
                    throw FaultLib.GetSoapException(FaultType.General, "Wrong Initial Termination Time.");
                }
            }

            string rawRequest = RequestListener.Take();
            XmlElement filterElement = Utils.GetFilterElements(rawRequest, true);

            int subscriptionKey = EventServer.AddSubscribtion(null, Filter, filterElement, terminationTime, nullInitialTerminationTime);

            CurrentTime = DateTime.UtcNow;
            TerminationTime = terminationTime;

            res.ReferenceParameters = new ReferenceParametersType();
            res.Address = new AttributedURIType();
            string hostAndPort = HttpContext.Current.Request.Url.Authority;
            res.Address.Value = "http://" + hostAndPort + "/ServiceEvents10/PullpointSubscriptionService.asmx";
            res.ReferenceParameters.Any = new System.Xml.XmlElement[1];

            NameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            nsmgr.AddNamespace("dut", "http://dut");
            nsmgr.AddNamespace("tdc", "http://www.onvif.org/ver10/doorcontrol/wsdl");

            XmlDocument referenceParameters = new XmlDocument(nt);
            res.ReferenceParameters.Any[0] = referenceParameters.CreateElement("dut", "id", "http://dut");
            res.ReferenceParameters.Any[0].InnerXml = subscriptionKey.ToString();



            EventServerSave();
            ConfStorageSave();

            actionHeader.actionValue = "http://www.onvif.org/ver10/events/wsdl/EventPortType/CreatePullPointSubscriptionResponse";
            EventServer.SynchronizationPoint(subscriptionKey);
            return res;
        }



        [WebMethod]
        [SoapHeader("unknownHeaders", Direction = SoapHeaderDirection.In)]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(Action = "http://www.onvif.org/ver10/events/wsdl/GetEventProperties", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("TopicNamespaceLocation", DataType = "anyURI")]
        public override string[] GetEventProperties([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://docs.oasis-open.org/wsn/b-2")] out bool FixedTopicSet, [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://docs.oasis-open.org/wsn/t-1")] out TopicSetType TopicSet, [System.Xml.Serialization.XmlElementAttribute("TopicExpressionDialect", Namespace = "http://docs.oasis-open.org/wsn/b-2", DataType = "anyURI")] out string[] TopicExpressionDialect, [System.Xml.Serialization.XmlElementAttribute("MessageContentFilterDialect", DataType = "anyURI")] out string[] MessageContentFilterDialect, [System.Xml.Serialization.XmlElementAttribute("ProducerPropertiesFilterDialect", DataType = "anyURI")] out string[] ProducerPropertiesFilterDialect, [System.Xml.Serialization.XmlElementAttribute("MessageContentSchemaLocation", DataType = "anyURI")] out string[] MessageContentSchemaLocation, [System.Xml.Serialization.XmlAnyElementAttribute()] out System.Xml.XmlElement[] Any)
        {
            SoapHeaderProcessing(unknownHeaders);
            ConfStorageLoad();
            EventServerLoad();

            string[] res = new string[1];

            res[0] = @"http://www.onvif.org/onvif/ver10/topics/topicns.xml";
            FixedTopicSet = true;
            TopicSet = new TopicSetType();
            TopicExpressionDialect = new string[2];
            TopicExpressionDialect[0] = @"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";
            TopicExpressionDialect[1] = @"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete";
            MessageContentFilterDialect = new string[1];
            MessageContentFilterDialect[0] = @"http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter";
            MessageContentSchemaLocation = new string[1];
            MessageContentSchemaLocation[0] = @"http://www.onvif.org/ver10/schema/onvif.xsd";
            ProducerPropertiesFilterDialect = null;
            Any = null;

            TopicSet.Any = EventServer.TopicSet;

            EventServerSave();
            ConfStorageSave();

            actionHeader.actionValue = "http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse";

            return res;

        }

        [WebMethod]
        [SoapHeader("unknownHeaders", Direction = SoapHeaderDirection.In)]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Subscribe", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SubscribeResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        [RequestListenerExtension()]
        public override SubscribeResponse Subscribe([System.Xml.Serialization.XmlElementAttribute("Subscribe", Namespace = "http://docs.oasis-open.org/wsn/b-2")] Subscribe Subscribe1)
        {
            SoapHeaderProcessing(unknownHeaders);
            ConfStorageLoad();
            EventServerLoad();
            
            SubscribeResponse res = new SubscribeResponse();

            DateTime terminationTime;

            try
            {
                TimeSpan timeSpan = System.Xml.XmlConvert.ToTimeSpan(Subscribe1.InitialTerminationTime);
                terminationTime = DateTime.UtcNow.Add(timeSpan.Add(new TimeSpan(0,0,1)));
            }
            catch (Exception)
            {
                try
                {
                    terminationTime = System.Xml.XmlConvert.ToDateTime(Subscribe1.InitialTerminationTime, XmlDateTimeSerializationMode.Utc);
                }
                catch (Exception)
                {
                    throw FaultLib.GetSoapException(FaultType.General, "Wrong Initial Termination Time.");
                }
            }

            string rawRequest = RequestListener.Take();
            XmlElement filterElement = Utils.GetFilterElements(rawRequest);

            int subscriptionKey = EventServer.AddSubscribtion(Subscribe1.ConsumerReference.Address.Value, 
                Subscribe1.Filter, 
                filterElement, 
                terminationTime, false);

            

            res.CurrentTimeSpecified = true;
            res.CurrentTime = DateTime.UtcNow;
            res.TerminationTimeSpecified = true;
            res.TerminationTime = terminationTime;
            res.SubscriptionReference = new EndpointReferenceType();
            res.SubscriptionReference.Address = new AttributedURIType();
            string hostAndPort = HttpContext.Current.Request.Url.Authority;
            res.SubscriptionReference.Address.Value = "http://" + hostAndPort + "/ServiceEvents10/SubscriptionManagerService.asmx";
            res.SubscriptionReference.ReferenceParameters = new ReferenceParametersType();
            res.SubscriptionReference.ReferenceParameters.Any = new System.Xml.XmlElement[1];

            NameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            nsmgr.AddNamespace("dut", "http://dut");
            nsmgr.AddNamespace("tdc", "http://www.onvif.org/ver10/doorcontrol/wsdl");

            XmlDocument referenceParameters = new XmlDocument(nt);
            res.SubscriptionReference.ReferenceParameters.Any[0] = referenceParameters.CreateElement("dut", "id", "http://dut");
            res.SubscriptionReference.ReferenceParameters.Any[0].InnerXml = subscriptionKey.ToString();



            EventServerSave();
            ConfStorageSave();

            actionHeader.actionValue = "http://docs.oasis-open.org/wsn/bw-2/NotificationProducer/SubscribeResponse";
            EventServer.SynchronizationPoint(subscriptionKey);
            return res;
        }

        
    }
}
