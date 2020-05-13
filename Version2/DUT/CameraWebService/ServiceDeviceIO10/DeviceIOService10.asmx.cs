using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.DeviceIO10
{
    /// <summary>
    /// Summary description for DeviceIOService10
    /// </summary>
    [WebService(Namespace = "http://www.onvif.org/ver10/deviceio/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DeviceIOService10 : DeviceIOBinding
    {

        //TestSuit
        DeviceIO10ServiceTest DeviceIO10ServiceTest
        {
            get
            {
                if (Application[Base.AppVars.DEVICEIOSERVICE] != null)
                {
                    return (DeviceIO10ServiceTest)Application[Base.AppVars.DEVICEIOSERVICE];
                }
                else
                {
                    DeviceIO10ServiceTest serviceTest = new DeviceIO10ServiceTest(TestCommon);
                    Application[Base.AppVars.DEVICEIOSERVICE] = serviceTest;
                    return serviceTest;
                }
                    }
                    }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/GetVideoSources", RequestNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
       // [XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_DeviceIO_GetVideoSourcesResponseIncorrectTag)]
        public override string[] GetVideoSources()
        {
            ParametersValidation validation = new ParametersValidation();
            string[] result = (string[])ExecuteGetCommand(validation, DeviceIO10ServiceTest.GetVideoSourcesTest);
            return result;
        }

        public override VideoOutput[] GetVideoOutputs()
        {
            throw new NotImplementedException();
        }

        public override VideoSourceConfiguration GetVideoSourceConfiguration(string VideoSourceToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override VideoOutputConfiguration GetVideoOutputConfiguration(string VideoOutputToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override AudioSourceConfiguration GetAudioSourceConfiguration(string AudioSourceToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override AudioOutputConfiguration GetAudioOutputConfiguration(string AudioOutputToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override void SetVideoSourceConfiguration(VideoSourceConfiguration Configuration, bool ForcePersistence, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override void SetVideoOutputConfiguration(VideoOutputConfiguration Configuration, bool ForcePersistence, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override void SetAudioSourceConfiguration(AudioSourceConfiguration Configuration, bool ForcePersistence, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override void SetAudioOutputConfiguration(AudioOutputConfiguration Configuration, bool ForcePersistence, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override VideoSourceConfigurationOptions GetVideoSourceConfigurationOptions(string VideoSourceToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override VideoOutputConfigurationOptions GetVideoOutputConfigurationOptions(string VideoOutputToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override AudioSourceConfigurationOptions GetAudioSourceConfigurationOptions(string AudioSourceToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        public override AudioOutputConfigurationOptions GetAudioOutputConfigurationOptions(string AudioOutputToken, ref System.Xml.XmlElement[] Any)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/GetRelayOutputs", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RelayOutputs")]
//        for bug 10845:
//        [XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:xmime=\"http://tempuri.org/xmime.xsd\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsrfbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsrfr=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:aa=\"http://www.axis.com/vapix/ws/action1\" xmlns:aev=\"http://www.axis.com/vapix/ws/event1\" xmlns:tan1=\"http://www.onvif.org/ver20/analytics/wsdl/RuleEngineBinding\" xmlns:tan2=\"http://www.onvif.org/ver20/analytics/wsdl/AnalyticsEngineBinding\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tev1=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:tev2=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tev3=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev4=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnsaxis=\"http://www.axis.com/2009/event/topics\"><SOAP-ENV:Header> </SOAP-ENV:Header>  <SOAP-ENV:Body>    <tds:GetRelayOutputsResponse>    </tds:GetRelayOutputsResponse>  </SOAP-ENV:Body></SOAP-ENV:Envelope>")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_DeviceIO_GetRelayOutputsResponseIncorrectResponseTag)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_DeviceIO_GetRelayOutputsResponseIncorrectNamespace)]
        public override RelayOutput[] GetRelayOutputs()
        {
            RelayOutput[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceIO10ServiceTest.GetRelayOutputsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/SetRelayOutputSettings", RequestNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRelayOutputSettings(RelayOutput RelayOutput)
        {
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceIO10ServiceTest.SetRelayOutputSettingsTest(out ex, out timeOut, RelayOutput);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/SetRelayOutputState", RequestNamespace = "http://www.onvif.org/ver10/device/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/device/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRelayOutputState(string RelayOutputToken, RelayLogicalState LogicalState)
        {
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceIO10ServiceTest.SetRelayOutputStateTest(out ex, out timeOut, RelayOutputToken, LogicalState);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override Capabilities1 GetServiceCapabilities()
        {
            Capabilities1 res;
            int timeOut;
            SoapException ex;

            StepType stepType = DeviceIO10ServiceTest.GetServiceCapabilitiesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/GetRelayOutputOptions", RequestNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RelayOutputOptions")]
        public override RelayOutputOptions[] GetRelayOutputOptions(string RelayOutputToken)
        {
            ParametersValidation validation = new ParametersValidation();
            RelayOutputOptions[] result = (RelayOutputOptions[])ExecuteGetCommand(validation, DeviceIO10ServiceTest.GetRelayOutputOptionsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/GetDigitalInputs", RequestNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DigitalInputs")]
        public override DigitalInput[] GetDigitalInputs()
        {
            ParametersValidation validation = new ParametersValidation();
            DigitalInput[] result = (DigitalInput[])ExecuteGetCommand(validation, DeviceIO10ServiceTest.GetDigitalInputsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/GetDigitalInputConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DigitalInputOptions")]
        public override DigitalInputConfigurationInputOptions GetDigitalInputConfigurationOptions(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "Token", Token);
            DigitalInputConfigurationInputOptions result = (DigitalInputConfigurationInputOptions)ExecuteGetCommand(validation, DeviceIO10ServiceTest.GetDigitalInputConfigurationOptionsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/SetDigitalInputConfigurations", RequestNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetDigitalInputConfigurations(DigitalInput[] DigitalInputs)
        {
            ParametersValidation validation = new ParametersValidation();
            //validation.Add(ParameterType.String, "DigitalInput/@token", DigitalInputs[0].token);
            //validation.Add(ParameterType.String, "IdleState", DigitalInputs);
            ExecuteVoidCommand(validation, DeviceIO10ServiceTest.SetDigitalInputConfigurationsTest);
        }

        public override SerialPort[] GetSerialPorts()
        {
            throw new NotImplementedException();
        }

        public override SerialPortConfiguration GetSerialPortConfiguration(string SerialPortToken)
        {
            throw new NotImplementedException();
        }

        public override void SetSerialPortConfiguration(SerialPortConfiguration SerialPortConfiguration, bool ForcePersistance)
        {
            throw new NotImplementedException();
        }

        public override SerialPortConfigurationOptions GetSerialPortConfigurationOptions(string SerialPortToken)
        {
            throw new NotImplementedException();
        }

        public override void SendReceiveSerialCommand(ref SerialData SerialData, string TimeOut, string DataLength, string Delimiter)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/GetAudioSources", RequestNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string[] GetAudioSources()
        {
            ParametersValidation validation = new ParametersValidation();
            string[] result = (string[])ExecuteGetCommand(validation, DeviceIO10ServiceTest.GetAudioSourcesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/deviceio/wsdl/GetAudioOutputs", RequestNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/deviceIO/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string[] GetAudioOutputs()
        {
            ParametersValidation validation = new ParametersValidation();
            string[] result = (string[])ExecuteGetCommand(validation, DeviceIO10ServiceTest.GetAudioOutputsTest);
            return result;
        }
    }
}
