using System;
using System.Linq;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace DUT.PACS.Simulator.Events10
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

        // Receive all SOAP headers.
        public SoapUnknownHeader[] unknownHeaders;

        private void SoapHeaderProcessing(SoapUnknownHeader[] unknownHeaders)
        {
            foreach (SoapUnknownHeader header in unknownHeaders)
            {
                header.DidUnderstand = true;
            }
        }

        private int SoapReferanceHeaderProcessing(SoapUnknownHeader[] unknownHeaders)
        {
            foreach (SoapUnknownHeader header in unknownHeaders)
            {
                header.DidUnderstand = true;
            }

            return Convert.ToInt32(unknownHeaders.First(header => (header.Element.LocalName == "id") && (header.Element.NamespaceURI == "http://dut")).Element.InnerText);
        }
        
        public ActionHeader actionHeader = new ActionHeader();
        
        [WebMethod]
        [SoapHeader("unknownHeaders", Direction = SoapHeaderDirection.In)]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/PullMessages", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("CurrentTime")]
        public override System.DateTime PullMessages([System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string Timeout, int MessageLimit, [System.Xml.Serialization.XmlAnyElementAttribute()] System.Xml.XmlElement[] Any, out System.DateTime TerminationTime, [System.Xml.Serialization.XmlElementAttribute("NotificationMessage", Namespace = "http://docs.oasis-open.org/wsn/b-2")] out NotificationMessageHolderType[] NotificationMessage)
        {

            int subscriptionKey = SoapReferanceHeaderProcessing(unknownHeaders);
            ConfStorageLoad();
            EventServerLoad();

            DateTime timeout;

            try
            {
                TimeSpan timeSpan = System.Xml.XmlConvert.ToTimeSpan(Timeout);
                timeout = DateTime.UtcNow.Add(timeSpan.Subtract(new TimeSpan(0, 0, 1)));
            }
            catch (Exception)
            {
                throw FaultLib.GetSoapException(FaultType.General, "Wrong Timeout.");
            }


            NotificationMessage = EventServer.GetPullPointMessages(subscriptionKey, timeout, MessageLimit);

            TerminationTime = EventServer.EventSubsciptionList[subscriptionKey].TerminationTime;

            DateTime res = DateTime.UtcNow;


            EventServerSave();
            ConfStorageSave();

            actionHeader.actionValue = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse";

            return res;
        }

        [WebMethod]
        [SoapHeader("unknownHeaders", Direction = SoapHeaderDirection.In)]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/SetSynchronizationPoint", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetSynchronizationPoint()
        {

            int subscriptionKey = SoapReferanceHeaderProcessing(unknownHeaders);
            ConfStorageLoad();
            EventServerLoad();

            EventServer.SynchronizationPoint(subscriptionKey);

            EventServerSave();
            ConfStorageSave();

            actionHeader.actionValue = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/SetSynchronizationPointResponse";

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
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Unsubscribe", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("UnsubscribeResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        public UnsubscribeResponse Unsubscribe([System.Xml.Serialization.XmlElementAttribute("Unsubscribe", Namespace = "http://docs.oasis-open.org/wsn/b-2")] Unsubscribe Unsubscribe1)
        {

            int subscriptionKey = SoapReferanceHeaderProcessing(unknownHeaders);
            ConfStorageLoad();
            EventServerLoad();

            EventServer.RemoveSubscribtion(subscriptionKey);

            UnsubscribeResponse res = new UnsubscribeResponse();

            EventServerSave();
            ConfStorageSave();

            actionHeader.actionValue = "http://docs.oasis-open.org/wsn/bw-2/SubscriptionManager/UnsubscribeResponse";

            return res;
        }
    }
}
