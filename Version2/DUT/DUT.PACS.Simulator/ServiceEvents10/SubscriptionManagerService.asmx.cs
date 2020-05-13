using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.PACS.Simulator.Common;
using DUT.PACS.Simulator.Events;

namespace DUT.PACS.Simulator.Events10
{
    /// <summary>
    /// Summary description for SubscriptionManagerService
    /// </summary>
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/events/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SubscriptionManagerService : SubscriptionManagerBinding
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

        private int SoapReferanceHeaderProcessing(SoapUnknownHeader[] unknownHeaders)
        {
            foreach (SoapUnknownHeader header in unknownHeaders)
            {
                header.DidUnderstand = true;
            }

            return Convert.ToInt32(unknownHeaders.First(header => (header.Element.LocalName == "id") && (header.Element.NamespaceURI == "http://dut")).Element.InnerText);
        }
        
        [WebMethod]
        [SoapHeader("unknownHeaders", Direction = SoapHeaderDirection.In)]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Renew", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("RenewResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        public override RenewResponse Renew([System.Xml.Serialization.XmlElementAttribute("Renew", Namespace = "http://docs.oasis-open.org/wsn/b-2")]Renew Renew1)
        {

            int subscriptionKey = SoapReferanceHeaderProcessing(unknownHeaders);
            ConfStorageLoad();
            EventServerLoad();

            DateTime terminationTime;

            try
            {
                TimeSpan timeSpan = System.Xml.XmlConvert.ToTimeSpan(Renew1.TerminationTime);
                terminationTime = DateTime.UtcNow.Add(timeSpan.Add(new TimeSpan(0, 0, 1)));
            }
            catch (Exception)
            {
                try
                {
                    terminationTime = System.Xml.XmlConvert.ToDateTime(Renew1.TerminationTime, XmlDateTimeSerializationMode.Utc);
                }
                catch (Exception)
                {
                    throw FaultLib.GetSoapException(FaultType.General, "Wrong Initial Termination Time.");
                }
            }

            EventServer.RenewSubscribtion(subscriptionKey, terminationTime);

            RenewResponse res = new RenewResponse();

            res.CurrentTimeSpecified = true;
            res.CurrentTime = DateTime.UtcNow;
            res.TerminationTime = terminationTime;

            EventServerSave();
            ConfStorageSave();

            actionHeader.actionValue = "http://docs.oasis-open.org/wsn/bw-2/SubscriptionManager/RenewResponse";

            return res;
        }

        [WebMethod]
        [SoapHeader("unknownHeaders", Direction = SoapHeaderDirection.In)]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        //[SoapHeader("actionFaultHeader", Direction = SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Unsubscribe", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("UnsubscribeResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        public override UnsubscribeResponse Unsubscribe([System.Xml.Serialization.XmlElementAttribute("Unsubscribe", Namespace = "http://docs.oasis-open.org/wsn/b-2")] Unsubscribe Unsubscribe1)
        {

            int subscriptionKey = SoapReferanceHeaderProcessing(unknownHeaders);
            ConfStorageLoad();
            EventServerLoad();

            EventServer.RemoveSubscribtion(subscriptionKey);

            UnsubscribeResponse res = new UnsubscribeResponse() ;

            EventServerSave();
            ConfStorageSave();

            actionHeader.actionValue = "http://docs.oasis-open.org/wsn/bw-2/SubscriptionManager/UnsubscribeResponse";

            return res;
        }

        [WebMethod]
        [SoapHeader("unknownHeaders", Direction = SoapHeaderDirection.In)]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        //[SoapHeader("actionFaultHeader", Direction = SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/SetSynchronizationPoint", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void SetSynchronizationPoint()
        {

            int subscriptionKey = SoapReferanceHeaderProcessing(unknownHeaders);
            ConfStorageLoad();
            EventServerLoad();

            EventServer.SynchronizationPoint(subscriptionKey);

            EventServerSave();
            ConfStorageSave();

            actionHeader.actionValue = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/SetSynchronizationPointResponse";

        }
    }
}
