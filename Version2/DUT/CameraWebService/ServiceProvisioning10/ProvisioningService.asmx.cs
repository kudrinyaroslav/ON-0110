using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Provisioning10
{
    /// <summary>
    /// Summary description for ProvisioningService
    /// </summary>

    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.18020")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/provisioning/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ProvisioningBinding", Namespace = "http://www.onvif.org/ver10/provisioning/wsdl")]
    public class ProvisioningService : ProvisioningServiceBinding
    {
        //TestSuit
        ProvisioningServiceTest ProvisioningServiceTest
        {
            get
            {
                if (Application[Base.AppVars.PROVISIONINGSERVICE] != null)
                {
                    return (ProvisioningServiceTest)Application[Base.AppVars.PROVISIONINGSERVICE];
                }
                else
                {
                    ProvisioningServiceTest serviceTest = new ProvisioningServiceTest(TestCommon);
                    Application[Base.AppVars.PROVISIONINGSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/provisioning/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override Capabilities GetServiceCapabilities()
        {
            ParametersValidation validation = new ParametersValidation();
            Capabilities result = (Capabilities)ExecuteGetCommand(validation, ProvisioningServiceTest.GetServiceCapabilitiesTest);
            return result;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/provisioning/wsdl/PanMove", RequestNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void PanMove(string VideoSource, PanDirection Direction, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string Timeout)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSource", VideoSource);
            validation.Add(ParameterType.OptionalString, "Timeout", Timeout);
            validation.Add(ParameterType.String, "Direction", Direction.ToString());
            ExecuteVoidCommand(validation, ProvisioningServiceTest.PanMoveTest);
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/provisioning/wsdl/TiltMove", RequestNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void TiltMove(string VideoSource, TiltDirection Direction, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string Timeout)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSource", VideoSource);
            validation.Add(ParameterType.OptionalString, "Timeout", Timeout);
            validation.Add(ParameterType.String, "Direction", Direction.ToString());
            ExecuteVoidCommand(validation, ProvisioningServiceTest.TiltMoveTest);
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/provisioning/wsdl/ZoomMove", RequestNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]        
        public override void ZoomMove(string VideoSource, ZoomDirection Direction, string Timeout)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSource", VideoSource);
            validation.Add(ParameterType.OptionalString, "Timeout", Timeout);
            validation.Add(ParameterType.String, "Direction", Direction.ToString());
            ExecuteVoidCommand(validation, ProvisioningServiceTest.ZoomMoveTest);
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/provisioning/wsdl/RollMove", RequestNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RollMove(string VideoSource, RollDirection Direction, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string Timeout)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSource", VideoSource);
            validation.Add(ParameterType.OptionalString, "Timeout", Timeout);
            validation.Add(ParameterType.String, "Direction", Direction.ToString());
            ExecuteVoidCommand(validation, ProvisioningServiceTest.RollMoveTest);
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/provisioning/wsdl/FocusMove", RequestNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void FocusMove(string VideoSource, FocusDirection Direction, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string Timeout)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSource", VideoSource);
            validation.Add(ParameterType.OptionalString, "Timeout", Timeout);
            validation.Add(ParameterType.String, "Direction", Direction.ToString());
            ExecuteVoidCommand(validation, ProvisioningServiceTest.FocusMoveTest);
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/provisioning/wsdl/Stop", RequestNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void Stop(string VideoSource)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSource", VideoSource);
            ExecuteVoidCommand(validation, ProvisioningServiceTest.StopTest);
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/provisioning/wsdl/Usage", RequestNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/provisioning/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Usage")]
        public override Usage GetUsage(string VideoSource)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSource", VideoSource);
            Usage result = (Usage)ExecuteGetCommand(validation, ProvisioningServiceTest.GetUsageTest);
            return result;
        }
    }
}
