using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Media210
{
    /// <summary>
    /// Summary description for Media2Service
    /// </summary>
    /// 
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver20/media/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "Media2Binding", Namespace = "http://www.onvif.org/ver20/media/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeviceEntity))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConfigurationEntity))]
    public class Media2Service : Media2Binding
    {

        //TestSuit
        Media2ServiceTest Media2ServiceTest
        {
            get
            {
                if (Application[Base.AppVars.MEDIA2SERVICE] != null)
                {
                    return (Media2ServiceTest)Application[Base.AppVars.MEDIA2SERVICE];
                }
                else
                {
                    Media2ServiceTest serviceTest = new Media2ServiceTest(TestCommon);
                    Application[Base.AppVars.MEDIA2SERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseElementName = "GetServiceCapabilitiesResponse", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesOneLine)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesNamespaceInResponseTag)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesNamespaceInBodyTag)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesCustomNamespacePrefix)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesNoNamespacePrefix)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesNullSimbol)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesIncorrectResponseTag)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesLineFeedSimbol)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesLineTabulationSimbol)]
       // [XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesCarriageReturnSimbol)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesBackspaceSimbol)]
       // [XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesIncorrectNamepace)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesNamepaceNotDeclarated)]
        public override Capabilities2 GetServiceCapabilities()
        {
            ParametersValidation validation = new ParametersValidation();
            Capabilities2 result = (Capabilities2)ExecuteGetCommand(validation, Media2ServiceTest.GetServiceCapabilitiesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/CreateProfile", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string CreateProfile(string Name, [System.Xml.Serialization.XmlElementAttribute("Configuration")] ConfigurationRef[] Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Name", Name);
            //validation.Add(ParameterType.OptionalElement, "Configuration", Configuration);
            //if (Configuration != null)
            //{
            //    validation.Add(ParameterType.String, "Configuration/Type", Configuration[0].Type.ToString());
            //    validation.Add(ParameterType.OptionalString, "Configuration/Token", Configuration[0].Token);
            //}
            string result = (string)ExecuteGetCommand(validation, Media2ServiceTest.CreateProfileTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetProfiles", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Profiles")]
        //Correct GetProfilesResponse
//        [XmlReplySubstituteExtension("<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:wsdl=\"http://www.onvif.org/ver20/media/wsdl\" xmlns:sch=\"http://www.onvif.org/ver10/schema\">\r\n" +
//"<soap:Header/>\r\n" +
//"<soap:Body>\r\n" +
//"<wsdl:GetProfilesResponse>        \r\n" +
//"<wsdl:Profiles token=\"1\" fixed=\"true\">\r\n" +
//"<wsdl:Name>name</wsdl:Name>           \r\n" +
//"<wsdl:Configurations>              \r\n" +
//"<wsdl:VideoSource token=\"VS1\">\r\n" +
//"<sch:Name>name</sch:Name>\r\n" +
//"<sch:UseCount>1</sch:UseCount>\r\n" +
//"<sch:SourceToken>1</sch:SourceToken>\r\n" +
//"<sch:Bounds x=\"1\" y=\"1\" width=\"1\" height=\"1\"/>                        \r\n" +
//"</wsdl:VideoSource>          \r\n" +
//"<wsdl:VideoEncoder token=\"2\" GovLength=\"1\" Profile=\"1\">\r\n" +
//"<sch:Name>1</sch:Name>\r\n" +
//"<sch:UseCount>1</sch:UseCount>\r\n" +
//"<sch:Encoding>H264</sch:Encoding>\r\n" +
//"<sch:Resolution>\r\n" +
//"<sch:Width>600</sch:Width>\r\n" +
//"<sch:Height>900</sch:Height>                   \r\n" +
//"</sch:Resolution>                  \r\n" +
//"<sch:Quality>1</sch:Quality>            \r\n" +
//"</wsdl:VideoEncoder>              \r\n" +
//"</wsdl:Configurations></wsdl:Profiles></wsdl:GetProfilesResponse></soap:Body></soap:Envelope>")]

        //Invalid GetProfilesResponse
//        [XmlReplySubstituteExtension("<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:wsdl=\"http://www.onvif.org/ver20/media/wsdl\" xmlns:sch=\"http://www.onvif.org/ver10/schema\">\r\n" +
//"<soap:Header/>\r\n" +
//"<soap:Body>\r\n" +
//"<wsdl:GetProfilesResponse>        \r\n" +
//"<wsdl:Profiles fixed=\"true\">\r\n" +
//"<wsdl:Name>name</wsdl:Name>           \r\n" +
//"<wsdl:Configurations>              \r\n" +
//"<wsdl:VideoSource token=\"VS1\">\r\n" +
//"<sch:Name>name</sch:Name>\r\n" +
//"<sch:UseCount>1</sch:UseCount>\r\n" +
//"<sch:SourceToken>1</sch:SourceToken>\r\n" +
//"<sch:Bounds x=\"1\" y=\"1\" width=\"1\" height=\"1\"/>                        \r\n" +
//"</wsdl:VideoSource>          \r\n" +
//"<wsdl:VideoEncoder token=\"2\" GovLength=\"1\" Profile=\"1\">\r\n" +
//"<sch:Name>1</sch:Name>\r\n" +
//"<sch:UseCount>1</sch:UseCount>\r\n" +
//"<sch:Encoding>H264</sch:Encoding>\r\n" +
//"<sch:Resolution>\r\n" +
//"<sch:Width>600</sch:Width>\r\n" +
//"<sch:Height>900</sch:Height>                   \r\n" +
//"</sch:Resolution>                  \r\n" +
//"<sch:Quality>1</sch:Quality>            \r\n" +
//"</wsdl:VideoEncoder>              \r\n" +
//"</wsdl:Configurations></wsdl:Profiles></wsdl:GetProfilesResponse></soap:Body></soap:Envelope>")]
        
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1616_GetProfilesResponse)]
       // [XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.AxisGetMedia2ProfilesResponse)]
            public override MediaProfile[] GetProfiles(string Token, [System.Xml.Serialization.XmlElementAttribute("Type")] string[] Type)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "Token", Token);
            validation.Add(ParameterType.OptionalElement, "Type ", Type);
            MediaProfile[] result = (MediaProfile[])ExecuteGetCommand(validation, Media2ServiceTest.GetProfilesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/AddConfiguration", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddConfiguration(string ProfileToken, string Name, [System.Xml.Serialization.XmlElementAttribute("Configuration")] ConfigurationRef[] Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ProfileToken",  ProfileToken);
            validation.Add(ParameterType.String, "Name",  Name);
            validation.Add(ParameterType.OptionalElement, "Configuration",  Configuration);
            if ( Configuration != null)
            {
                validation.Add(ParameterType.String, "Configuration/Type",  Configuration[0].Type.ToString());
                validation.Add(ParameterType.OptionalString, "Configuration/Token",  Configuration[0].Token);
            }
            ExecuteVoidCommand(validation, Media2ServiceTest.AddConfigurationTest);
        }
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/RemoveConfiguration", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_RemoveConfigurationResponse)]
        public override void RemoveConfiguration(string ProfileToken, [System.Xml.Serialization.XmlElementAttribute("Configuration")] ConfigurationRef[] Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ProfileToken",  ProfileToken);
            validation.Add(ParameterType.String, "Configuration/Type",  Configuration[0].Type);
            validation.Add(ParameterType.OptionalString, "Configuration/Token",  Configuration[0].Token);
            ExecuteVoidCommand(validation, Media2ServiceTest.RemoveConfigurationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/DeleteProfile", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteProfile(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, Media2ServiceTest.DeleteProfileTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetVideoSourceConfigurations", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override VideoSourceConfiguration[] GetVideoSourceConfigurations(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);
            VideoSourceConfiguration[] result = (VideoSourceConfiguration[])ExecuteGetCommand(validation, Media2ServiceTest.GetVideoSourceConfigurationsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetVideoEncoderConfigurations", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override VideoEncoder2Configuration[] GetVideoEncoderConfigurations(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);
            VideoEncoder2Configuration[] result = (VideoEncoder2Configuration[])ExecuteGetCommand(validation, Media2ServiceTest.GetVideoEncoderConfigurationsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdlGetAudioSourceConfigurations/", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override AudioSourceConfiguration[] GetAudioSourceConfigurations(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);
            AudioSourceConfiguration[] result = (AudioSourceConfiguration[])ExecuteGetCommand(validation, Media2ServiceTest.GetAudioSourceConfigurationsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetAudioEncoderConfigurations", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override AudioEncoder2Configuration[] GetAudioEncoderConfigurations(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);
            AudioEncoder2Configuration[] result = (AudioEncoder2Configuration[])ExecuteGetCommand(validation, Media2ServiceTest.GetAudioEncoderConfigurationsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetAnalyticsConfigurations", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.Ticket1730_GetAnalyticsConfigurationsResponse)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.WhiteSpace_GetAnalyticsConfigurationsResponse)]
        public override VideoAnalyticsConfiguration[] GetAnalyticsConfigurations(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);
            VideoAnalyticsConfiguration[] result = (VideoAnalyticsConfiguration[])ExecuteGetCommand(validation, Media2ServiceTest.GetAnalyticsConfigurationsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetMetadataConfigurations", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override MetadataConfiguration[] GetMetadataConfigurations(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);
            MetadataConfiguration[] result = (MetadataConfiguration[])ExecuteGetCommand(validation, Media2ServiceTest.GetMetadataConfigurationsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetAudioOutputConfigurations", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override AudioOutputConfiguration[] GetAudioOutputConfigurations(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);
            AudioOutputConfiguration[] result = (AudioOutputConfiguration[])ExecuteGetCommand(validation, Media2ServiceTest.GetAudioOutputConfigurationsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetAudioDecoderConfigurations", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override AudioDecoderConfiguration[] GetAudioDecoderConfigurations(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);
            AudioDecoderConfiguration[] result = (AudioDecoderConfiguration[])ExecuteGetCommand(validation, Media2ServiceTest.GetAudioDecoderConfigurationsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/SetVideoSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetVideoSourceConfiguration(VideoSourceConfiguration Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSourceConfiguration/@token", Configuration.token);
            validation.Add(ParameterType.String, "VideoSourceConfiguration/Name", Configuration.Name);
            validation.Add(ParameterType.String, "VideoSourceConfiguration/SourceToken", Configuration.SourceToken);
            validation.Add(ParameterType.String, "VideoSourceConfiguration/Bounds", Configuration.Bounds.ToString());
            if ( Configuration.Bounds != null)
            {
                validation.Add(ParameterType.Int, "VideoSourceConfiguration/Bounds/@x", Configuration.Bounds.x);
                validation.Add(ParameterType.Int, "VideoSourceConfiguration/Bounds/@y", Configuration.Bounds.y);
                validation.Add(ParameterType.Int, "VideoSourceConfiguration/Bounds/@width", Configuration.Bounds.width);
                validation.Add(ParameterType.Int, "VideoSourceConfiguration/Bounds/@height", Configuration.Bounds.height);
            }
            validation.Add(ParameterType.OptionalElement, "VideoSourceConfiguration/Extension", Configuration.Extension);
            if ( Configuration.Extension != null)
            {
                validation.Add(ParameterType.OptionalElement, "VideoSourceConfiguration/Extension/Rotate", Configuration.Extension.Rotate);
                if ( Configuration.Extension.Rotate != null)
                {
                    validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Rotate/Mode", Configuration.Extension.Rotate.Mode.ToString());
                    validation.Add(ParameterType.OptionalInt, "VideoSourceConfiguration/Extension/Rotate/Degree", Configuration.Extension.Rotate.Degree);
                    validation.Add(ParameterType.OptionalElement, "VideoSourceConfiguration/Extension/Rotate/Extension", Configuration.Extension.Rotate.Extension);
                }
                validation.Add(ParameterType.OptionalElement, "VideoSourceConfiguration/Extension/Extension", Configuration.Extension.Extension);
                if ( Configuration.Extension.Extension != null)
                {
                    validation.Add(ParameterType.OptionalElement, "VideoSourceConfiguration/Extension/Extension/LensDescription", Configuration.Extension.Extension.LensDescription);
                    if ( Configuration.Extension.Extension.LensDescription != null)
                    {
                        validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Extension/LensDescription/FocalLength", Configuration.Extension.Extension.LensDescription[0].FocalLength.ToString());
                        if ( Configuration.Extension.Extension.LensDescription[0].Offset != null)
                        {
                            validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Extension/LensDescription/Offset/@x", Configuration.Extension.Extension.LensDescription[0].Offset.x.ToString());
                            validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Extension/LensDescription/Offset/@y", Configuration.Extension.Extension.LensDescription[0].Offset.x.ToString());
                        }
                        if ( Configuration.Extension.Extension.LensDescription[0].Projection != null)
                        {
                            validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Extension/LensDescription/Projection/Angle", Configuration.Extension.Extension.LensDescription[0].Projection[0].Angle.ToString());
                            validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Extension/LensDescription/Projection/Radius", Configuration.Extension.Extension.LensDescription[0].Projection[0].Radius.ToString());
                            validation.Add(ParameterType.OptionalString, "VideoSourceConfiguration/Extension/Extension/LensDescription/Projection/Radius", Configuration.Extension.Extension.LensDescription[0].Projection[0].Transmittance.ToString());
                        }
                        validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Extension/LensDescription/XFactor", Configuration.Extension.Extension.LensDescription[0].XFactor.ToString());
                    }
                }
            }
            ExecuteVoidCommand(validation, Media2ServiceTest.SetVideoSourceConfigurationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/SetVideoEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetVideoEncoderConfiguration(VideoEncoder2Configuration Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoEncoder2Configuration/@token", Configuration.token);
            validation.Add(ParameterType.String, "VideoEncoder2Configuration/Name", Configuration.Name);
            //validation.Add(ParameterType.OptionalElement, "VideoEncoder2Configuration/@GovLength", Configuration.GovLength);
            if (Configuration.GovLengthSpecified)
            {
                validation.Add(ParameterType.Int, "VideoEncoder2Configuration/@GovLength", Configuration.GovLength);
            }
            validation.Add(ParameterType.String, "VideoEncoder2Configuration/@Profile", Configuration.Profile);
            validation.Add(ParameterType.String, "VideoEncoder2Configuration/Encoding", Configuration.Encoding);
            validation.Add(ParameterType.Int, "VideoEncoder2Configuration/Resolution/Width", Configuration.Resolution.Width);
            validation.Add(ParameterType.Int, "VideoEncoder2Configuration/Resolution/Height", Configuration.Resolution.Height);
            validation.Add(ParameterType.OptionalElement, "VideoEncoder2Configuration/RateControl", Configuration.RateControl);
            if (Configuration.RateControl != null)
            {
                if (Configuration.RateControl.ConstantBitRateSpecified)
                {
                    validation.Add(ParameterType.String, "VideoEncoder2Configuration/RateControl/ConstantBitRateSpecified", Configuration.RateControl.ConstantBitRate.ToString());
                }
                validation.Add(ParameterType.String, "VideoEncoder2Configuration/RateControl/FrameRateLimit", Configuration.RateControl.FrameRateLimit.ToString());
                validation.Add(ParameterType.Int, "VideoEncoder2Configuration/RateControl/BitrateLimit", Configuration.RateControl.BitrateLimit);
            }
            validation.Add(ParameterType.OptionalElement, "VideoEncoder2Configuration/Multicast", Configuration.Multicast);
            if (Configuration.Multicast != null)
            {
                //validation.Add(ParameterType.String, "VideoEncoder2Configuration/Multicast/Address", Configuration.Multicast.Address.ToString());
                if (Configuration.Multicast.Address != null)
                {
                    validation.Add(ParameterType.String, "VideoEncoder2Configuration/Multicast/Address/Type", Configuration.Multicast.Address.Type.ToString());
                    validation.Add(ParameterType.OptionalString, "VideoEncoder2Configuration/Multicast/Address/IPv4Address", Configuration.Multicast.Address.IPv4Address.ToString());
                    //validation.Add(ParameterType.OptionalString, "VideoEncoder2Configuration/Multicast/Address/IPv6Address", Configuration.Multicast.Address.IPv6Address.ToString());
                }
                validation.Add(ParameterType.Int, "VideoEncoder2Configuration/Multicast/Port", Configuration.Multicast.Port);
                validation.Add(ParameterType.Int, "VideoEncoder2Configuration/Multicast/TTL", Configuration.Multicast.TTL);
                validation.Add(ParameterType.String, "VideoEncoder2Configuration/Multicast/AutoStart", Configuration.Multicast.AutoStart.ToString());
            }

            validation.Add(ParameterType.String, "VideoEncoder2Configuration/Quality", Configuration.Quality.ToString());
            ExecuteVoidCommand(validation, Media2ServiceTest.SetVideoEncoderConfigurationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/SetAudioSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetAudioSourceConfiguration(AudioSourceConfiguration Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "AudioSourceConfiguration/@token", Configuration.token);
            validation.Add(ParameterType.String, "AudioSourceConfiguration/Name", Configuration.Name);
            validation.Add(ParameterType.String, "AudioSourceConfiguration/SourceToken", Configuration.SourceToken);

            ExecuteVoidCommand(validation, Media2ServiceTest.SetAudioSourceConfigurationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/SetAudioEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetAudioEncoderConfiguration(AudioEncoder2Configuration Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "AudioEncoder2Configuration/@token", Configuration.token);
            validation.Add(ParameterType.String, "AudioEncoder2Configuration/Name", Configuration.Name);
            validation.Add(ParameterType.String, "AudioEncoder2Configuration/Encoding", Configuration.Encoding);
            validation.Add(ParameterType.OptionalElement, "AudioEncoder2Configuration/Multicast", Configuration.Multicast);
            if (Configuration.Multicast != null)
            {
                validation.Add(ParameterType.String, "AudioEncoder2Configuration/Multicast/Address", Configuration.Multicast.Address.ToString());
                if (Configuration.Multicast.Address != null)
                {
                    validation.Add(ParameterType.String, "AudioEncoder2Configuration/Multicast/Address/Type", Configuration.Multicast.Address.Type.ToString());
                    validation.Add(ParameterType.OptionalString, "AudioEncoder2Configuration/Multicast/Address/IPv4Address", Configuration.Multicast.Address.IPv4Address);
                    validation.Add(ParameterType.OptionalString, "AudioEncoder2Configuration/Multicast/Address/IPv6Address", Configuration.Multicast.Address.IPv6Address);
                }
                validation.Add(ParameterType.Int, "AudioEncoder2Configuration/Multicast/Port", Configuration.Multicast.Port);
                validation.Add(ParameterType.Int, "AudioEncoder2Configuration/Multicast/TTL", Configuration.Multicast.TTL);
                validation.Add(ParameterType.String, "AudioEncoder2Configuration/Multicast/AutoStart", Configuration.Multicast.AutoStart.ToString());
            }
            validation.Add(ParameterType.Int, "AudioEncoder2Configuration/Bitrate", Configuration.Bitrate);
            validation.Add(ParameterType.Int, "AudioEncoder2Configuration/SampleRate", Configuration.SampleRate);

            ExecuteVoidCommand(validation, Media2ServiceTest.SetAudioEncoderConfigurationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/SetMetadataConfiguration", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetMetadataConfiguration(MetadataConfiguration Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "MetadataConfiguration/@token", Configuration.token);
            validation.Add(ParameterType.String, "MetadataConfiguration/Name", Configuration.Name);
            validation.Add(ParameterType.String, "MetadataConfiguration/@CompressionType", Configuration.CompressionType);
            
            if (Configuration.PTZStatus != null)
            {
                validation.Add(ParameterType.OptionalBool, "MetadataConfiguration/PTZStatus/@Status", Configuration.PTZStatus.Status);
                validation.Add(ParameterType.OptionalBool, "MetadataConfiguration/PTZStatus/@Position", Configuration.PTZStatus.Position);
            }
            validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/Events", Configuration.Events);
            if (Configuration.Events != null)
            {
                validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/Events/Filter", Configuration.Events.Filter);
                validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/Events/SubscriptionPolicy", Configuration.Events.SubscriptionPolicy);
            }
            validation.Add(ParameterType.OptionalBool, "MetadataConfiguration/@Analytics", Configuration.Analytics);
            if (Configuration.Multicast != null)
            {
                if (Configuration.Multicast.Address != null)
                {
                    validation.Add(ParameterType.String, "MetadataConfiguration/Multicast/Address/Type", Configuration.Multicast.Address.Type.ToString());
                    if (Configuration.Multicast.Address.Type == IPType.IPv4)
                    {
                        validation.Add(ParameterType.OptionalString, "MetadataConfiguration/Multicast/Address/IPv4Address", Configuration.Multicast.Address.IPv4Address.ToString());
                    }
                    if (Configuration.Multicast.Address.Type == IPType.IPv6)
                    {
                        validation.Add(ParameterType.OptionalString, "MetadataConfiguration/Multicast/Address/IPv6Address", Configuration.Multicast.Address.IPv6Address.ToString());
                    }
                }
                validation.Add(ParameterType.Int, "MetadataConfiguration/Multicast/Port", Configuration.Multicast.Port);
                validation.Add(ParameterType.Int, "MetadataConfiguration/Multicast/TTL", Configuration.Multicast.TTL);
                validation.Add(ParameterType.String, "MetadataConfiguration/Multicast/AutoStart", Configuration.Multicast.AutoStart.ToString().ToLower());
            }
            validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/AnalyticsEngineConfiguration", Configuration.AnalyticsEngineConfiguration);
            if (Configuration.AnalyticsEngineConfiguration != null)
            {
                validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule", Configuration.AnalyticsEngineConfiguration.AnalyticsModule);
                if (Configuration.AnalyticsEngineConfiguration.AnalyticsModule != null)
                {
                    validation.Add(ParameterType.String, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Name", Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Name);
                    validation.Add(ParameterType.String, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Type", Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Type.ToString());
                    validation.Add(ParameterType.String, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters", Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.ToString());
                    if (Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters != null)
                    {
                        validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters/SimpleItem", Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.SimpleItem);
                        if (Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.SimpleItem != null)
                        {
                            validation.Add(ParameterType.String, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters/SimpleItem/Name", Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.SimpleItem[0].Name);
                            validation.Add(ParameterType.String, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters/SimpleItem/Value", Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.SimpleItem[0].Value.ToString());
                        }
                        validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters/ElementItem", Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.ElementItem);
                        if (Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.ElementItem != null)
                        {
                            validation.Add(ParameterType.String, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters/ElementItem/Name", Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.ElementItem[0].Name);
                        }
                        if (Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.Extension != null)
                        {
                            //should be different type then string
                            //validation.Add(ParameterType.OptionalString, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters/Extension", Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.Extension.ToString());
                        }
                    }
                }
                if (Configuration.AnalyticsEngineConfiguration.Extension != null)
                {
                    //validation.Add(ParameterType.OptionalString, "MetadataConfiguration/AnalyticsEngineConfiguration/Extension", Configuration.AnalyticsEngineConfiguration.Extension.ToString());
                }
            }
            if (Configuration.Extension != null)
            {
                //validation.Add(ParameterType.OptionalString, "MetadataConfiguration/Extension", Configuration.Extension.ToString());
            }
            ExecuteVoidCommand(validation, Media2ServiceTest.SetMetadataConfigurationTest);
           
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/SetAudioOutputConfiguration", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetAudioOutputConfiguration(AudioOutputConfiguration Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "AudioOutputConfiguration/@token", Configuration.token);
            validation.Add(ParameterType.String, "AudioOutputConfiguration/Name", Configuration.Name);
            //validation.Add(ParameterType.Int, "AudioOutputConfiguration/UseCount", Configuration.UseCount);
            validation.Add(ParameterType.String, "AudioOutputConfiguration/OutputToken", Configuration.OutputToken);
            validation.Add(ParameterType.OptionalString, "AudioOutputConfiguration/SendPrimacy", Configuration.SendPrimacy.ToString());
            if (Configuration.SendPrimacy != null)
            {
                //The following modes for the Send-Primacy are defined:
                //www.onvif.org/ver20/HalfDuplex/Server
                //www.onvif.org/ver20/HalfDuplex/Client
                //www.onvif.org/ver20/HalfDuplex/Auto
            }
            validation.Add(ParameterType.Int, "AudioOutputConfiguration/OutputLevel", Configuration.OutputLevel);
            ExecuteVoidCommand(validation, Media2ServiceTest.SetAudioOutputConfigurationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/SetAudioDecoderConfiguration", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetAudioDecoderConfiguration(AudioDecoderConfiguration Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "AudioDecoderConfiguration/@token", Configuration.token);
            validation.Add(ParameterType.String, "AudioDecoderConfiguration/Name", Configuration.Name);
            ExecuteVoidCommand(validation, Media2ServiceTest.SetAudioDecoderConfigurationTest);

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdlGetVideoSourceConfigurationOptions/", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override VideoSourceConfigurationOptions GetVideoSourceConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);
            VideoSourceConfigurationOptions result = (VideoSourceConfigurationOptions)ExecuteGetCommand(validation, Media2ServiceTest.GetVideoSourceConfigurationOptionsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetVideoEncoderConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1228_FrameRatesSupported_Empty)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1228_GovLengthRange_Empty)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1228_ConstantBitRateSupported_Empty)]
        public override VideoEncoder2ConfigurationOptions[] GetVideoEncoderConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);
            VideoEncoder2ConfigurationOptions[] result = (VideoEncoder2ConfigurationOptions[])ExecuteGetCommand(validation, Media2ServiceTest.GetVideoEncoderConfigurationOptionsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetAudioSourceConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override AudioSourceConfigurationOptions GetAudioSourceConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);

            AudioSourceConfigurationOptions result = (AudioSourceConfigurationOptions)ExecuteGetCommand(validation, Media2ServiceTest.GetAudioSourceConfigurationOptionsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetAudioEncoderConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override AudioEncoder2ConfigurationOptions[] GetAudioEncoderConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);

            AudioEncoder2ConfigurationOptions[] result = (AudioEncoder2ConfigurationOptions[])ExecuteGetCommand(validation, Media2ServiceTest.GetAudioEncoderConfigurationOptionsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetMetadataConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override MetadataConfigurationOptions GetMetadataConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);

            MetadataConfigurationOptions result = (MetadataConfigurationOptions)ExecuteGetCommand(validation, Media2ServiceTest.GetMetadataConfigurationOptionsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetAudioOutputConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override AudioOutputConfigurationOptions GetAudioOutputConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);

            AudioOutputConfigurationOptions result = (AudioOutputConfigurationOptions)ExecuteGetCommand(validation, Media2ServiceTest.GetAudioOutputConfigurationOptionsTest);

            return result;

        }
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetAudioDecoderConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override AudioEncoder2ConfigurationOptions[] GetAudioDecoderConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", ProfileToken);

            AudioEncoder2ConfigurationOptions [] result = (AudioEncoder2ConfigurationOptions[])ExecuteGetCommand(validation, Media2ServiceTest.GetAudioDecoderConfigurationOptionsTest);

            return result;

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetVideoEncoderInstances", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Info")]
        public override EncoderInstanceInfo GetVideoEncoderInstances(string ConfigurationToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);

            EncoderInstanceInfo result = (EncoderInstanceInfo)ExecuteGetCommand(validation, Media2ServiceTest.GetVideoEncoderInstancesTest);

            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetStreamUri", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Uri", DataType = "anyURI")]
        public override string GetStreamUri(string Protocol, string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Protocol", Protocol); //there wasn't these validations
            validation.Add(ParameterType.String, "ProfileToken", ProfileToken);//

            string result = (string)ExecuteGetCommand(validation, Media2ServiceTest.GetStreamUriTest);

            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/StartMulticastStreaming", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void StartMulticastStreaming(string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ProfileToken", ProfileToken);

            ExecuteVoidCommand(validation, Media2ServiceTest.StartMulticastStreamingTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/StopMulticastStreaming", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void StopMulticastStreaming(string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ProfileToken", ProfileToken);

            ExecuteVoidCommand(validation, Media2ServiceTest.StopMulticastStreamingTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/SetSynchronizationPoint", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetSynchronizationPoint(string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ProfileToken", ProfileToken);

            ExecuteVoidCommand(validation, Media2ServiceTest.SetSynchronizationPointTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetSnapshotUri", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Uri", DataType = "anyURI")]
        public override string GetSnapshotUri(string ProfileToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ProfileToken", ProfileToken);

            string result = (string)ExecuteGetCommand(validation, Media2ServiceTest.GetSnapshotUriTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetVideoSourceModes", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("VideoSourceModes")]
        public override VideoSourceMode[] GetVideoSourceModes(string VideoSourceToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "VideoSourceToken", VideoSourceToken);

            VideoSourceMode [] result = (VideoSourceMode[])ExecuteGetCommand(validation, Media2ServiceTest.GetVideoSourceModesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/SetVideoSourceMode", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Reboot")]
        public override bool SetVideoSourceMode(string VideoSourceToken, string VideoSourceModeToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSourceToken", VideoSourceToken);
            validation.Add(ParameterType.String, "VideoSourceModeToken", VideoSourceModeToken);

            bool result = (bool)ExecuteGetCommand(validation, (ParametersValidation validationRequest, out StepType stepType, out System.Web.Services.Protocols.SoapException exc, out int timeout)
                                                       => Media2ServiceTest.SetVideoSourceModeTest(validationRequest, out stepType,  out exc,  out timeout));
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetOSDs", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("OSDs")]
        public override OSDConfiguration[] GetOSDs(string OSDToken, string ConfigurationToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "OSDToken", OSDToken);
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            OSDConfiguration[] result = (OSDConfiguration[])ExecuteGetCommand(validation, Media2ServiceTest.GetOSDsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetOSDOptions", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("OSDOptions")]
        public override OSDConfigurationOptions GetOSDOptions(string ConfigurationToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", ConfigurationToken);
            OSDConfigurationOptions result = (OSDConfigurationOptions)ExecuteGetCommand(validation, Media2ServiceTest.GetOSDOptionsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/SetOSD", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetOSD(OSDConfiguration OSD)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "OSDConfiguration/@token", OSD.token);
            validation.Add(ParameterType.String, "OSDConfiguration/VideoSourceConfigurationToken", OSD.VideoSourceConfigurationToken.Value);
            validation.Add(ParameterType.String, "OSDConfiguration/Type", OSD.Type.ToString());
            validation.Add(ParameterType.String, "OSDConfiguration/Position/Type", OSD.Position.Type);
            validation.Add(ParameterType.OptionalElement, "OSDConfiguration/Position/Pos", OSD.Position.Pos);
            if (OSD.Position.Pos != null)
            {
                validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/Position/Pos/@x", OSD.Position.Pos.xSpecified);
                if (OSD.Position.Pos.xSpecified)
                {
                    validation.Add(ParameterType.OptionalString, "OSDConfiguration/Position/Pos/@x", OSD.Position.Pos.x.ToString());
                }

                validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/Position/Pos/@y", OSD.Position.Pos.ySpecified);
                if (OSD.Position.Pos.ySpecified)
                {
                    validation.Add(ParameterType.OptionalString, "OSDConfiguration/Position/Pos/@y", OSD.Position.Pos.y.ToString());
                }
            }
            validation.Add(ParameterType.OptionalElement, "OSDConfiguration/TextString", OSD.TextString);
            if (OSD.TextString != null)
            {
                validation.Add(ParameterType.String, "OSDConfiguration/TextString/Type", OSD.TextString.Type);
                validation.Add(ParameterType.OptionalString, "OSDConfiguration/TextString/DateFormat", OSD.TextString.DateFormat);
                validation.Add(ParameterType.OptionalString, "OSDConfiguration/TextString/TimeFormat", OSD.TextString.TimeFormat);
                validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/TextString/FontSize", OSD.TextString.FontSizeSpecified);
                if (OSD.TextString.FontSizeSpecified)
                {
                    validation.Add(ParameterType.Int, "OSDConfiguration/TextString/FontSize", OSD.TextString.FontSize);
                }
                validation.Add(ParameterType.OptionalElement, "OSDConfiguration/TextString/FontColor", OSD.TextString.FontColor);
                if (OSD.TextString.FontColor != null)
                {
                    validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/TextString/FontColor/@Transparent", OSD.TextString.FontColor.TransparentSpecified);
                    if (OSD.TextString.FontColor.TransparentSpecified)
                    {
                        validation.Add(ParameterType.Int, "OSDConfiguration/TextString/FontColor/@Transparent", OSD.TextString.FontColor.Transparent);
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@X", OSD.TextString.FontColor.Color.X.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@Y", OSD.TextString.FontColor.Color.Y.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@Z", OSD.TextString.FontColor.Color.Z.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@Colorspace", OSD.TextString.FontColor.Color.Colorspace);
                    }
                }
                validation.Add(ParameterType.OptionalElement, "OSDConfiguration/TextString/BackgroundColor", OSD.TextString.BackgroundColor);
                if (OSD.TextString.BackgroundColor != null)
                {
                    validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/TextString/BackgroundColor/@Transparent", OSD.TextString.BackgroundColor.TransparentSpecified);
                    if (OSD.TextString.BackgroundColor.TransparentSpecified)
                    {
                        validation.Add(ParameterType.Int, "OSDConfiguration/TextString/BackgroundColor/@Transparent", OSD.TextString.BackgroundColor.Transparent);
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@X", OSD.TextString.BackgroundColor.Color.X.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@Y", OSD.TextString.BackgroundColor.Color.Y.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@Z", OSD.TextString.BackgroundColor.Color.Z.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@Colorspace", OSD.TextString.BackgroundColor.Color.Colorspace);
                    }
                }
                validation.Add(ParameterType.OptionalString, "OSDConfiguration/TextString/PlainText", OSD.TextString.PlainText);
            }
            validation.Add(ParameterType.OptionalElement, "OSDConfiguration/Image", OSD.Image);
            if (OSD.Image != null)
            {
                validation.Add(ParameterType.String, "OSDConfiguration/Image/ImgPath", OSD.Image.ImgPath);
            }

            ExecuteVoidCommand(validation, Media2ServiceTest.SetOSDTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/CreateOSD", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("OSDToken")]
        public override string CreateOSD(OSDConfiguration OSD)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "OSDConfiguration/@token", OSD.token);
            validation.Add(ParameterType.String, "OSDConfiguration/VideoSourceConfigurationToken", OSD.VideoSourceConfigurationToken.Value);
            validation.Add(ParameterType.String, "OSDConfiguration/Type", OSD.Type.ToString());
            validation.Add(ParameterType.String, "OSDConfiguration/Position/Type", OSD.Position.Type);
            validation.Add(ParameterType.OptionalElement, "OSDConfiguration/Position/Pos", OSD.Position.Pos);
            if (OSD.Position.Pos != null)
            {
                validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/Position/Pos/@x", OSD.Position.Pos.xSpecified);
                if (OSD.Position.Pos.xSpecified)
                {
                    validation.Add(ParameterType.OptionalString, "OSDConfiguration/Position/Pos/@x", OSD.Position.Pos.x.ToString());
                }

                validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/Position/Pos/@y", OSD.Position.Pos.ySpecified);
                if (OSD.Position.Pos.ySpecified)
                {
                    validation.Add(ParameterType.OptionalString, "OSDConfiguration/Position/Pos/@y", OSD.Position.Pos.y.ToString());
                }
            }
            validation.Add(ParameterType.OptionalElement, "OSDConfiguration/TextString", OSD.TextString);
            if (OSD.TextString != null)
            {
                validation.Add(ParameterType.String, "OSDConfiguration/TextString/Type", OSD.TextString.Type);               
                validation.Add(ParameterType.OptionalString, "OSDConfiguration/TextString/DateFormat", OSD.TextString.DateFormat);
                validation.Add(ParameterType.OptionalString, "OSDConfiguration/TextString/TimeFormat", OSD.TextString.TimeFormat);
                validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/TextString/FontSize", OSD.TextString.FontSizeSpecified);
                if (OSD.TextString.FontSizeSpecified)
                {
                    validation.Add(ParameterType.Int, "OSDConfiguration/TextString/FontSize", OSD.TextString.FontSize);
                }
                validation.Add(ParameterType.OptionalElement, "OSDConfiguration/TextString/FontColor", OSD.TextString.FontColor);
                if (OSD.TextString.FontColor != null)
                {
                    validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/TextString/FontColor/@Transparent", OSD.TextString.FontColor.TransparentSpecified);
                    if (OSD.TextString.FontColor.TransparentSpecified)
                    {
                        validation.Add(ParameterType.Int, "OSDConfiguration/TextString/FontColor/@Transparent", OSD.TextString.FontColor.Transparent);
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@X", OSD.TextString.FontColor.Color.X.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@Y", OSD.TextString.FontColor.Color.Y.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@Z", OSD.TextString.FontColor.Color.Z.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@Colorspace", OSD.TextString.FontColor.Color.Colorspace);
                    }
                }
                validation.Add(ParameterType.OptionalElement, "OSDConfiguration/TextString/BackgroundColor", OSD.TextString.BackgroundColor);
                if (OSD.TextString.BackgroundColor != null)
                {
                    validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/TextString/BackgroundColor/@Transparent", OSD.TextString.BackgroundColor.TransparentSpecified);
                    if (OSD.TextString.BackgroundColor.TransparentSpecified)
                    {
                        validation.Add(ParameterType.Int, "OSDConfiguration/TextString/BackgroundColor/@Transparent", OSD.TextString.BackgroundColor.Transparent);
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@X", OSD.TextString.BackgroundColor.Color.X.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@Y", OSD.TextString.BackgroundColor.Color.Y.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@Z", OSD.TextString.BackgroundColor.Color.Z.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@Colorspace", OSD.TextString.BackgroundColor.Color.Colorspace);
                    }
                }
                validation.Add(ParameterType.OptionalString, "OSDConfiguration/TextString/PlainText", OSD.TextString.PlainText);
            }
            validation.Add(ParameterType.OptionalElement, "OSDConfiguration/Image", OSD.Image);
            if (OSD.Image != null)
            {
                validation.Add(ParameterType.String, "OSDConfiguration/Image/ImgPath", OSD.Image.ImgPath);
            }
            string result = (string)ExecuteGetCommand(validation, Media2ServiceTest.CreateOSDTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/DeleteOSD", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseElementName = "DeleteOSDResponse", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteOSD(string OSDToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "OSDToken", OSDToken);
            ExecuteVoidCommand(validation, Media2ServiceTest.DeleteOSDTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetMasks", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Masks")]
        public override Mask[] GetMasks(string Token, string ConfigurationToken)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetMaskOptions", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override MaskOptions GetMaskOptions(string ConfigurationToken)
        {
            throw new NotImplementedException();
        }
       
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/SetMask", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetMask(Mask Mask)
        {
            throw new NotImplementedException();
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/CreateMask", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string CreateMask(Mask Mask)
        {
            throw new NotImplementedException();
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/DeleteMask", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteMask(string Token)
        {
            throw new NotImplementedException();
        }
    }
}
