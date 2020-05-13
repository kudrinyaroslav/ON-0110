using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ServiceModel;
using System.Web.Services.Protocols;
using System.Xml;
using System.Threading;

namespace CameraWebService.Receiver
{
    /// <summary>
    /// Summary description for ReceiverService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ReceiverService : ReceiverBinding
    {

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override Capabilities GetServiceCapabilities()
        {
            return new Capabilities()
                       {
                           MaximumRTSPURILength = 256,
                           MaximumRTSPURILengthSpecified = true,
                           RTP_Multicast = true,
                           RTP_MulticastSpecified = true,
                           RTP_RTSP_TCP = true,
                           RTP_RTSP_TCPSpecified = true,
                           SupportedReceivers = 15
                       };
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/GetReceivers", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Receivers")]
        public override Receiver[] GetReceivers()
        {
            return Search.SearchStorage.Instance.Receivers.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/GetReceiver", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Receiver")]
        public override Receiver GetReceiver(string ReceiverToken)
        {
            Receiver receiver = Search.SearchStorage.Instance.Receivers.Where(r => r.Token == ReceiverToken).FirstOrDefault();
            if (receiver == null)
                ReturnFault(new string[] {"Sender", "InvalidArgVal", "UnknownToken"});
            
            return receiver;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/CreateReceiver", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Receiver")]
        public override Receiver CreateReceiver(ReceiverConfiguration Configuration)
        {
            int MaxNumberOfReceivers = GetServiceCapabilities().SupportedReceivers;
            Receiver receiver = null;
            if (Search.SearchStorage.Instance.Receivers.Count < MaxNumberOfReceivers)
            {
                receiver = new Receiver()
                {
                    Token = string.Format("receiver{0}", Search.SearchStorage.Instance.Receivers.Count + 1),
                    Configuration = Configuration
                };
                Search.SearchStorage.Instance.Receivers.Add(receiver);
            }
            else
            {
                ReturnFault(new string[] { "Receiver", "Action", "MaxReceivers" });
            }
            return receiver;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/DeleteReceiver", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteReceiver(string ReceiverToken)
        {
            Receiver receiver = Search.SearchStorage.Instance.Receivers.Where(r=>r.Token==ReceiverToken).FirstOrDefault();
            if (receiver != default(Receiver))
            {
                Search.SearchStorage.Instance.Receivers.Remove(receiver);
            }
            else
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "UnknownToken" });
            }
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/ConfigureReceiver", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ConfigureReceiver(string ReceiverToken, ReceiverConfiguration Configuration)
        {
            Receiver receiver = Search.SearchStorage.Instance.Receivers.Where(r => r.Token == ReceiverToken).FirstOrDefault();
            if (receiver != default(Receiver))
            {                
                receiver.Configuration = Configuration;

                switch (Configuration.Mode)
                { 
                    case ReceiverMode.AlwaysConnect:
                        NotifyReceiverStateChange(ReceiverToken, "Connected");
                        break;
                    case ReceiverMode.NeverConnect:
                        NotifyReceiverStateChange(ReceiverToken, "NotConnected");
                        break;
                }
            }
            else
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "UnknownToken" });
            }
        }

        class NotifyParameters
        {
            public string Address { get; set; }
            public string Request { get; set; }
        }

        void NotifyReceiverStateChange(string receiverToken, string state)
        {
            if (!Application.AllKeys.Contains("consumer"))
            {
                return;
            }

            NotifyParameters parameters = new NotifyParameters();

            Events.EndpointReferenceType reference = (Events.EndpointReferenceType)Application["consumer"];
            string address = reference.Address.Value;
            parameters.Address = address;

            string requestPattern = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> " +
                                "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tet=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tt=\"http://www.onvif.org/ver10/schema\"> " +
                                "<SOAP-ENV:Header>" +
                                "<wsa:Action>urn:#Notify</wsa:Action>" +
                                "<wsa:ReplyTo><wsa:Address>http://example.com/business/client1</wsa:Address></wsa:ReplyTo>" +
                                "<wsa:MessageID>http://example.com/6B29FC40-CA47-1067-B31D-00DD010662DA  http://example.com/6B29FC40-CA47-1067-B31D-00DD010662DA</wsa:MessageID>" +
                                "</SOAP-ENV:Header>" +
                                "<SOAP-ENV:Body>" +
                                "<wsnt:Notify>" +
                                "<wsnt:NotificationMessage>" +
                                "<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">" +
                                "tns1:Receiver/ChangeState" +
                                "</wsnt:Topic>" +
                                "<wsnt:Message>" +
                                "<tt:Message UtcTime=\"{0}\">" +
                                "<tt:Source>" +
                                "<tt:SimpleItem Name=\"ReceiverToken\" Value=\"{1}\"/>" +
                                "</tt:Source>" +
                                "<tt:Data>" +
                                "<tt:SimpleItem Name=\"NewState\" Value=\"{2}\"/>" +
                                "</tt:Data>" +
                                "</tt:Message>" +
                                "</wsnt:Message>" +
                                "</wsnt:NotificationMessage>" +
                                "<tet:CurrentTime>{3}</tet:CurrentTime>" +
                                "<tet:TerminationTime>{4}</tet:TerminationTime>" +
                                "</wsnt:Notify>" +
                                "</SOAP-ENV:Body>" +
                                "</SOAP-ENV:Envelope>"; 

            System.DateTime currentTime = System.DateTime.Now;
            string notificationTime = System.Xml.XmlConvert.ToString(currentTime.AddSeconds(-1), XmlDateTimeSerializationMode.Utc);
            string serverCurrentTime = System.Xml.XmlConvert.ToString(currentTime, XmlDateTimeSerializationMode.Utc);
            string terminationTime = System.Xml.XmlConvert.ToString(currentTime.AddSeconds(20), XmlDateTimeSerializationMode.Utc);

            string request = string.Format(requestPattern, notificationTime, receiverToken, state, serverCurrentTime, terminationTime);
            parameters.Request = request;
            System.Threading.Thread thread = new Thread( new ParameterizedThreadStart(DoNotify));
            thread.Start(parameters);
        }

        void DoNotify(object parameters)
        {
            Thread.Sleep(5000);

            NotifyParameters param = (NotifyParameters)parameters;
            CameraWebService.Notification.NotificationProvider provider = new CameraWebService.Notification.NotificationProvider();
            provider.Notify(param.Address, param.Request);
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/SetReceiverMode", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetReceiverMode(string ReceiverToken, ReceiverMode Mode)
        {
            Receiver receiver = Search.SearchStorage.Instance.Receivers.Where(r => r.Token == ReceiverToken).FirstOrDefault();
            if (receiver != default(Receiver))
            {
                receiver.Configuration.Mode = Mode;
            }
            else
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "UnknownToken" });
            }
            
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/GetReceiverState", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ReceiverState")]
        public override ReceiverStateInformation GetReceiverState(string ReceiverToken)
        {
            if (!Search.SearchStorage.Instance.ReceiverStateInformationDictionary.ContainsKey(ReceiverToken))
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "UnknownToken" });
            ReceiverStateInformation receiverState = Search.SearchStorage.Instance.ReceiverStateInformationDictionary[ReceiverToken];
            return receiverState;
        }

        void ReturnFault(string[] codes)
        {
            SoapFaultSubCode subCode = null;
            for (int i = codes.Length - 1; i > 0; i--)
            {
                SoapFaultSubCode currentSubCode = new SoapFaultSubCode(new XmlQualifiedName(codes[i], "http://www.onvif.org/ver10/error"), subCode);
                subCode = currentSubCode;
            }
            throw new SoapException("Error", new XmlQualifiedName(codes[0], "http://www.w3.org/2003/05/soap-envelope"), subCode);
        }
    }
}
