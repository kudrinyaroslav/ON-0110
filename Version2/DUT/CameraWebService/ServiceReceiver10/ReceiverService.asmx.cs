using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Receiver10
{
    /// <summary>
    /// Summary description for ReceiverService
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/receiver/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ReceiverBinding", Namespace = "http://www.onvif.org/ver10/receiver/wsdl")]
    public class ReceiverService : ReceiverBinding
    {
        ReceiverServiceTest ReceiverServiceTest
        {
            get 
            { 
                if (Application[Base.AppVars.RECEIVERSERVICE] != null)
                {
                    return (ReceiverServiceTest)Application[Base.AppVars.RECEIVERSERVICE];
                }
                else
                {
                    ReceiverServiceTest receiverServiceTest = new ReceiverServiceTest(TestCommon);
                    Application[Base.AppVars.RECEIVERSERVICE] = receiverServiceTest;
                    return receiverServiceTest;
                }            
            }
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_ReceiverCapabilitiesIncorrectResponseTag)]
        public override Capabilities GetServiceCapabilities()
        {
            Capabilities result = (Capabilities)ExecuteGetCommand(null, ReceiverServiceTest.GetServiceCapabilitiesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/GetReceivers", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Receivers")]
        public override Receiver[] GetReceivers()
        {
            Receiver[] result = (Receiver[])ExecuteGetCommand(null, ReceiverServiceTest.GetReceiversTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/GetReceiver", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Receiver")]
        public override Receiver GetReceiver(string ReceiverToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ReceiverToken", ReceiverToken);
            Receiver result = (Receiver)ExecuteGetCommand(validation, ReceiverServiceTest.GetReceiverTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/CreateReceiver", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Receiver")]
        public override Receiver CreateReceiver(ReceiverConfiguration Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            // ToDo: Configuration
            Receiver result = (Receiver)ExecuteGetCommand(validation, ReceiverServiceTest.CreateReceiverTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/DeleteReceiver", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteReceiver(string ReceiverToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ReceiverToken", ReceiverToken);
            ExecuteVoidCommand(validation, ReceiverServiceTest.DeleteReceiverTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/ConfigureReceiver", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ConfigureReceiver(string ReceiverToken, ReceiverConfiguration Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ReceiverToken", ReceiverToken);
            // ToDo: Configuration
            ExecuteVoidCommand(validation, ReceiverServiceTest.ConfigureReceiverTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/SetReceiverMode", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetReceiverMode(string ReceiverToken, ReceiverMode Mode)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ReceiverToken", ReceiverToken);
            validation.Add(ParameterType.String, "Mode", Mode.ToString());
            ExecuteVoidCommand(validation, ReceiverServiceTest.SetReceiverModeTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/receiver/wsdl/GetReceiverState", RequestNamespace = "http://www.onvif.org/ver10/receiver/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/receiver/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ReceiverState")]
        public override ReceiverStateInformation GetReceiverState(string ReceiverToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ReceiverToken", ReceiverToken);

            ReceiverStateInformation result = (ReceiverStateInformation)ExecuteGetCommand(validation, ReceiverServiceTest.GetReceiverStateTest);
            return result;
        }
    }
}
