using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using PTZ;

namespace CameraWebService
{
    /// <summary>
    /// Summary description for PtzService
    /// </summary>
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver20/ptz/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PtzService : PTZ.PTZBinding
    {

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetNodes", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PTZNode")]
        [ScriptDriven]
        public override PTZ.PTZNode[] GetNodes()
        {
            PTZ.PTZNode[] res = PTZService10.PtzStorage.Instance.Nodes.ToArray();
            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetNode", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PTZNode")]
        public override PTZ.PTZNode GetNode(string NodeToken)
        {
            PTZ.PTZNode res = PTZService10.PtzStorage.Instance.Nodes.Where(N => N.token == NodeToken).FirstOrDefault();
                      
            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetConfiguration", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PTZConfiguration")]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/event/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\"><soap:Body><tptz:GetConfigurationResponse><tptz:PTZConfiguration token=\"eptz\"><tt:Name>eptzName</tt:Name><tt:UseCount>0</tt:UseCount><tt:NodeToken>eptzNodeToken1</tt:NodeToken><tt:DefaultAbsolutePantTiltPositionSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:DefaultAbsolutePantTiltPositionSpace><tt:DefaultAbsoluteZoomPositionSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:DefaultAbsoluteZoomPositionSpace><tt:DefaultRelativePanTiltTranslationSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</tt:DefaultRelativePanTiltTranslationSpace><tt:DefaultRelativeZoomTranslationSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</tt:DefaultRelativeZoomTranslationSpace><tt:DefaultPTZSpeed><tt:PanTilt x=\"0.100\" y=\"0.100\"/><tt:Zoom x=\"1.000\"/></tt:DefaultPTZSpeed><tt:DefaultContinuousPanTiltVelocitySpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</tt:DefaultContinuousPanTiltVelocitySpace><tt:DefaultContinuousZoomVelocitySpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</tt:DefaultContinuousZoomVelocitySpace><tt:DefaultPTZTimeout>PT5S</tt:DefaultPTZTimeout><tt:PanTiltLimits><tt:Range><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:URI><tt:XRange><tt:Min>-INF</tt:Min><tt:Max>INF</tt:Max></tt:XRange><tt:YRange><tt:Min>-INF</tt:Min><tt:Max>INF</tt:Max></tt:YRange></tt:Range></tt:PanTiltLimits><tt:ZoomLimits><tt:Range><tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:URI><tt:XRange><tt:Min>-INF</tt:Min><tt:Max>INF</tt:Max></tt:XRange></tt:Range></tt:ZoomLimits></tptz:PTZConfiguration></tptz:GetConfigurationResponse></soap:Body></soap:Envelope>")]
        public override PTZ.PTZConfiguration GetConfiguration(string PTZConfigurationToken)
        {
            PTZ.PTZConfiguration configuration = PTZService10.PtzStorage.Instance.Configurations.Where(C => C.token == PTZConfigurationToken).FirstOrDefault();

            return configuration;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetConfigurations", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PTZConfiguration")]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/event/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\"><soap:Body><tptz:GetConfigurationsResponse><tptz:PTZConfiguration token=\"eptz\"><tt:Name>eptzName</tt:Name><tt:UseCount>0</tt:UseCount><tt:NodeToken>eptzNodeToken1</tt:NodeToken><tt:DefaultAbsolutePantTiltPositionSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:DefaultAbsolutePantTiltPositionSpace><tt:DefaultAbsoluteZoomPositionSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:DefaultAbsoluteZoomPositionSpace><tt:DefaultRelativePanTiltTranslationSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</tt:DefaultRelativePanTiltTranslationSpace><tt:DefaultRelativeZoomTranslationSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</tt:DefaultRelativeZoomTranslationSpace><tt:DefaultPTZSpeed><tt:PanTilt x=\"0.100\" y=\"0.100\"/><tt:Zoom x=\"1.000\"/></tt:DefaultPTZSpeed><tt:DefaultContinuousPanTiltVelocitySpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</tt:DefaultContinuousPanTiltVelocitySpace><tt:DefaultContinuousZoomVelocitySpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</tt:DefaultContinuousZoomVelocitySpace><tt:DefaultPTZTimeout>PT5S</tt:DefaultPTZTimeout><tt:PanTiltLimits><tt:Range><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:URI><tt:XRange><tt:Min>-INF</tt:Min><tt:Max>INF</tt:Max></tt:XRange><tt:YRange><tt:Min>-INF</tt:Min><tt:Max>INF</tt:Max></tt:YRange></tt:Range></tt:PanTiltLimits><tt:ZoomLimits><tt:Range><tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:URI><tt:XRange><tt:Min>-INF</tt:Min><tt:Max>INF</tt:Max></tt:XRange></tt:Range></tt:ZoomLimits></tptz:PTZConfiguration></tptz:GetConfigurationsResponse></soap:Body></soap:Envelope>")]
        public override PTZ.PTZConfiguration[] GetConfigurations()
        {
            PTZ.PTZConfiguration[] res = PTZService10.PtzStorage.Instance.Configurations.ToArray();
            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/SetConfiguration", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetConfiguration(PTZ.PTZConfiguration PTZConfiguration, bool ForcePersistence)
        {
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PTZConfigurationOptions")]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/event/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\"><soap:Body><tptz:GetConfigurationOptionsResponse><tptz:PTZConfigurationOptions><tt:Spaces><tt:AbsolutePanTiltPositionSpace><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:URI><tt:XRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange><tt:YRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:YRange></tt:AbsolutePanTiltPositionSpace><tt:AbsolutePanTiltPositionSpace><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/DigitalPositionSpace</tt:URI><tt:XRange><tt:Min>0.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange><tt:YRange><tt:Min>0.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:YRange></tt:AbsolutePanTiltPositionSpace><tt:AbsoluteZoomPositionSpace><tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:URI><tt:XRange><tt:Min>0.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange></tt:AbsoluteZoomPositionSpace><tt:RelativePanTiltTranslationSpace><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</tt:URI><tt:XRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange><tt:YRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:YRange></tt:RelativePanTiltTranslationSpace><tt:RelativeZoomTranslationSpace><tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</tt:URI><tt:XRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange></tt:RelativeZoomTranslationSpace><tt:ContinuousPanTiltVelocitySpace><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</tt:URI><tt:XRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange><tt:YRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:YRange></tt:ContinuousPanTiltVelocitySpace><tt:ContinuousZoomVelocitySpace><tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</tt:URI><tt:XRange><tt:Min>-1.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange></tt:ContinuousZoomVelocitySpace><tt:PanTiltSpeedSpace><tt:URI>http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace</tt:URI><tt:XRange><tt:Min>0.000000</tt:Min><tt:Max>1.000000</tt:Max></tt:XRange></tt:PanTiltSpeedSpace><tt:ZoomSpeedSpace><tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace</tt:URI><tt:XRange><tt:Min>0.000000</tt:Min><tt:Max>INF</tt:Max></tt:XRange></tt:ZoomSpeedSpace></tt:Spaces><tt:PTZTimeout><tt:Min>PT1S</tt:Min><tt:Max>PT5S</tt:Max></tt:PTZTimeout></tptz:PTZConfigurationOptions></tptz:GetConfigurationOptionsResponse></soap:Body></soap:Envelope>")]
        public override PTZ.PTZConfigurationOptions GetConfigurationOptions(string ConfigurationToken)
        {

            /*
       //ONVIF 13.8.1.1 Generic Pan/Tilt Position Space
        protected const string _absolutePanTiltSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace";
        //ONVIF 13.8.1.2 Generic Zoom Position Space
        protected const string _absoluteZoomSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace";
        //ONVIF 13.8.3.1 Generic Pan/Tilt Velocity Space
        protected const string _continuousPanTiltSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace";
        //ONVIF 13.8.3.2 Generic Zoom Velocity Space
        protected const string _continuousZoomSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace";
        //ONVIF 13.8.3.1 Generic Pan/Tilt Velocity Space
        protected const string _relativePanTiltSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace";
        //ONVIF 13.8.3.2 Generic Zoom Velocity Space
        protected const string _relativeZoomSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace";
        //ONVIF 13.8.4.1 Generic Pan/Tilt Speed Space
        protected const string _speedPanTiltSpace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace";
        //ONVIF 13.8.4.2 Generic Zoom Speed Space
        protected const string _speedZoomSpace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace";
             */

            PTZ.PTZConfigurationOptions res = new PTZ.PTZConfigurationOptions();
            res.PTZTimeout = new PTZ.DurationRange(){Min="PT10S", Max="PT20S"};
            res.Spaces = new PTZ.PTZSpaces();
            res.Spaces.AbsolutePanTiltPositionSpace = new PTZ.Space2DDescription[]
                    {
                        new PTZ.Space2DDescription() { URI = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace", XRange = new PTZ.FloatRange() { Min = 1.0F, Max = 5F }, YRange = new PTZ.FloatRange() { Min = 1.0F, Max = 5F }  }
                    };
            res.Spaces.AbsoluteZoomPositionSpace = 
                new PTZ.Space1DDescription[]
                    {
                        new PTZ.Space1DDescription() { URI = "http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace", XRange = new PTZ.FloatRange() { Min = 1.0F, Max = 5F } }
                    };
            res.Spaces.ContinuousPanTiltVelocitySpace = new PTZ.Space2DDescription[]
                    {
                        new PTZ.Space2DDescription() { URI = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace", XRange = new PTZ.FloatRange() { Min = 1.0F, Max = 5F }, YRange = new PTZ.FloatRange() { Min = 1.0F, Max = 5F } }
                    };
            res.Spaces.ContinuousZoomVelocitySpace = 
                new PTZ.Space1DDescription[]
                    {
                        new PTZ.Space1DDescription() { URI = "http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace", XRange = new PTZ.FloatRange() { Min = 1.0F, Max = 5F } }
                    };
            res.Spaces.Extension = new PTZ.PTZSpacesExtension();
            res.Spaces.PanTiltSpeedSpace = 
                new PTZ.Space1DDescription[]
                    {
                        new PTZ.Space1DDescription(){ URI = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace", XRange = new PTZ.FloatRange() {Min = 1.0F, Max=5F}}
                    };
            res.Spaces.RelativePanTiltTranslationSpace = 
                new PTZ.Space2DDescription[]
                    {
                        new PTZ.Space2DDescription() { URI = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace", XRange = new PTZ.FloatRange() { Min = 1.0F, Max = 5F }, YRange = new PTZ.FloatRange() { Min = 1.0F, Max = 5F }  }
                    };
            res.Spaces.ZoomSpeedSpace = 
                new PTZ.Space1DDescription[]
                    {
                        new PTZ.Space1DDescription() { URI = "http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace", XRange = new PTZ.FloatRange() { Min = 1.0F, Max = 5F } }
                    }; 


            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/SendAuxiliaryCommand", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AuxiliaryResponse")]
        public override string SendAuxiliaryCommand(string ProfileToken, string AuxiliaryData)
        {
            string res = null;

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetPresets", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Preset")]
        public override PTZ.PTZPreset[] GetPresets(string ProfileToken)
        {
            PTZ.PTZPreset[] res = null;

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/SetPreset", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetPreset(string ProfileToken, string PresetName, ref string PresetToken)
        {
        }
        
        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/RemovePreset", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemovePreset(string ProfileToken, string PresetToken)
        {
        }

        public override void GotoPreset(string ProfileToken, string PresetToken, PTZ.PTZSpeed Speed)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GotoHomePosition", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void GotoHomePosition(string ProfileToken, PTZ.PTZSpeed Speed)
        {
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/SetHomePosition", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetHomePosition(string ProfileToken)
        {
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/ContinuousMove", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ContinuousMove(string ProfileToken, PTZ.PTZSpeed Velocity, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string Timeout)
        {
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/RelativeMove", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RelativeMove(string ProfileToken, PTZ.PTZVector Translation, PTZ.PTZSpeed Speed)
        {
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetStatus", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("PTZStatus")]
        public override PTZ.PTZStatus GetStatus(string ProfileToken)
        {
            PTZ.PTZStatus res = new  PTZ.PTZStatus();
            res.MoveStatus = new PTZMoveStatus();
            res.MoveStatus.PanTiltSpecified = false;
            res.MoveStatus.ZoomSpecified = false;
            res.Position = new PTZVector(){PanTilt = new Vector2D(), Zoom = new Vector1D()};
            res.UtcTime = System.DateTime.UtcNow;

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/AbsoluteMove", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AbsoluteMove(string ProfileToken, PTZ.PTZVector Position, PTZ.PTZSpeed Speed)
        {
        }
        
        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/Stop", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void Stop(string ProfileToken, bool PanTilt, [System.Xml.Serialization.XmlIgnoreAttribute()] bool PanTiltSpecified, bool Zoom, [System.Xml.Serialization.XmlIgnoreAttribute()] bool ZoomSpecified)
        {
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/ptz/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver20/ptz/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/ptz/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override PTZ.Capabilities GetServiceCapabilities()
        {
            return new PTZ.Capabilities(){EFlip = true, EFlipSpecified = true, ReverseSpecified = true, Reverse = true};
        }
    }
}
