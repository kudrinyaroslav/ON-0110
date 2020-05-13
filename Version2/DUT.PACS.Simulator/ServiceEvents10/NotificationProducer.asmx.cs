using System;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace DUT.PACS.Simulator.Events10
{
    /// <summary>
    /// Summary description for NotificationProducer
    /// </summary>
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/events/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NotificationProducer : NotificationProducerBinding
    {
        
        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Subscribe", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SubscribeResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        [RequestListenerExtension()]
        public override SubscribeResponse Subscribe([System.Xml.Serialization.XmlElementAttribute("Subscribe", Namespace = "http://docs.oasis-open.org/wsn/b-2")] Subscribe Subscribe1)
        {
            ConfStorageLoad();
            EventServerLoad();

            SubscribeResponse res = new SubscribeResponse();

            DateTime terminationTime;

            try
            {
                TimeSpan timeSpan = System.Xml.XmlConvert.ToTimeSpan(Subscribe1.InitialTerminationTime);
                terminationTime = DateTime.Now.Add(timeSpan);
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

            EventServer.AddSubscribtion(Subscribe1.ConsumerReference.Address.Value, Subscribe1.Filter, filterElement, terminationTime, false);

            res.CurrentTimeSpecified = true;
            res.CurrentTime = DateTime.Now;
            res.TerminationTimeSpecified = true;
            res.TerminationTime = terminationTime;
            res.SubscriptionReference = new EndpointReferenceType();
            res.SubscriptionReference.Address = new AttributedURIType();
            string hostAndPort = HttpContext.Current.Request.Url.Authority;
            res.SubscriptionReference.Address.Value = "http://" + hostAndPort + "/ServiceEvents10/NotificationProducer.asmx";
            res.SubscriptionReference.ReferenceParameters = new ReferenceParametersType();
            res.SubscriptionReference.ReferenceParameters.Any = new System.Xml.XmlElement[1];

            NameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            nsmgr.AddNamespace("dut", "http://dut");
            nsmgr.AddNamespace("tdc", "http://www.onvif.org/ver10/doorcontrol/wsdl");

            XmlDocument referenceParameters = new XmlDocument(nt);
            res.SubscriptionReference.ReferenceParameters.Any[0] = referenceParameters.CreateElement("dut", "id", "http://dut");
            

            EventServerSave();
            ConfStorageSave();

            throw new NotImplementedException();
        }

        public override GetCurrentMessageResponse GetCurrentMessage(GetCurrentMessage GetCurrentMessage1)
        {
            throw new NotImplementedException();
        }
    }
}
