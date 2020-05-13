using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Replay10
{
    /// <summary>
    /// Summary description for MediaService
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/replay/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ReplayBinding", Namespace = "http://www.onvif.org/ver10/replay/wsdl")]
    public class ReplayService : Replay10ServiceBinding
    {
        public void TestSuitInit()
        {

        }

        ReplayServiceTest ReplayServiceTest
        {
            get
            {
                if (Application[Base.AppVars.REPLAYSERVICE] != null)
                {
                    return (ReplayServiceTest)Application[Base.AppVars.REPLAYSERVICE];
                }
                else
                {
                    ReplayServiceTest serviceTest = new ReplayServiceTest(TestCommon);
                    Application[Base.AppVars.REPLAYSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/replay/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/replay/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/replay/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1342_SessionTimeoutRange_Empty)]
        public override Capabilities GetServiceCapabilities()
        {
            Capabilities res;
            int timeOut;
            SoapException ex;

            StepType stepType = ReplayServiceTest.GetServiceCapabilitiesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/replay/wsdl/GetReplayUri", RequestNamespace = "http://www.onvif.org/ver10/replay/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/replay/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Uri", DataType = "anyURI")]
        public override string GetReplayUri(StreamSetup StreamSetup, string RecordingToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "RecordingToken", RecordingToken);
            validation.Add(ParameterType.String, "StreamSetup/Stream", StreamSetup.Stream.ToString());
            validation.Add(ParameterType.String, "StreamSetup/Transport/Protocol", StreamSetup.Transport.Protocol.ToString());
            return (string)ExecuteGetCommand(validation, ReplayServiceTest.GetReplayUriTest);
        }
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/replay/wsdl/GetReplayConfiguration", RequestNamespace = "http://www.onvif.org/ver10/replay/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/replay/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override ReplayConfiguration GetReplayConfiguration()
        {
            ParametersValidation validation = new ParametersValidation();
            return (ReplayConfiguration)ExecuteGetCommand(validation, ReplayServiceTest.GetReplayConfigurationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/replay/wsdl/SetReplayConfiguration", RequestNamespace = "http://www.onvif.org/ver10/replay/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/replay/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_ReplaySetReplayConfigurationResponseIncorrectResponseTag)]
        public override void SetReplayConfiguration(ReplayConfiguration Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ReplayConfiguration/SessionTimeout", Configuration.SessionTimeout.ToString());
            ExecuteVoidCommand(validation, ReplayServiceTest.SetReplayConfigurationTest);
        }
    }
}
