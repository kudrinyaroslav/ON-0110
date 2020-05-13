using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DUT.WithLogic.Proxy;
using DUT.WithLogic.Base;
using DUT.WithLogic.Engine;

namespace DUT.WithLogic.Services.Media2
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/device/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "DeviceBinding", Namespace = "http://www.onvif.org/ver10/device/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeviceEntity))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtensibleDocumented))]
    public class ONVIFMedia2 : Media2Binding
    {

        protected ONVIFMedia2Configuration ONVIFMedia2Configuration
        {
            get
            {
                if (Application[AppVars.ONVIFMEDIA2CONFIGURATION] != null)
                {
                    return (ONVIFMedia2Configuration)Application[AppVars.ONVIFMEDIA2CONFIGURATION];
                }
                else
                {
                    ONVIFMedia2Configuration onvifMedia2Configuration = ONVIFMedia2Configuration.Load();
                    Application[AppVars.ONVIFMEDIA2CONFIGURATION] = onvifMedia2Configuration;
                    return onvifMedia2Configuration;
                }
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        public void SaveConfiguration()
        {
            ONVIFMedia2Configuration.Serialize();
        }

        [System.Web.Services.WebMethodAttribute()]
        public void SaveCapabilities()
        {
            ONVIFMedia2Capabilities.Serialize();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseElementName = "GetServiceCapabilitiesResponse", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override Capabilities2 GetServiceCapabilities()
        {
            return ONVIFConfiguration.ONVIFMedia2Capabilities.Capabilities;
        }

        public override string CreateProfile(string Name, ConfigurationRef[] Configuration)
        {
            throw new NotImplementedException();
        }

        public override MediaProfile[] GetProfiles(string Token, string[] Type)
        {
            throw new NotImplementedException();
        }

        public override void AddConfiguration(string ProfileToken, string Name, ConfigurationRef[] Configuration)
        {
            throw new NotImplementedException();
        }

        public override void RemoveConfiguration(string ProfileToken, ConfigurationRef[] Configuration)
        {
            throw new NotImplementedException();
        }

        public override void DeleteProfile(string Token)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetVideoSourceConfigurations", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override VideoSourceConfiguration[] GetVideoSourceConfigurations(string ConfigurationToken, string ProfileToken)
        {
            if ((ConfigurationToken == null) && (ProfileToken == null))
            {
                return ONVIFMedia2Configuration.VideoSourceConfigurationList.ToArray();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override VideoEncoder2Configuration[] GetVideoEncoderConfigurations(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override AudioSourceConfiguration[] GetAudioSourceConfigurations(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override AudioEncoder2Configuration[] GetAudioEncoderConfigurations(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override VideoAnalyticsConfiguration[] GetAnalyticsConfigurations(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override MetadataConfiguration[] GetMetadataConfigurations(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override AudioOutputConfiguration[] GetAudioOutputConfigurations(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override AudioDecoderConfiguration[] GetAudioDecoderConfigurations(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override void SetVideoSourceConfiguration(VideoSourceConfiguration Configuration)
        {
            throw new NotImplementedException();
        }

        public override void SetVideoEncoderConfiguration(VideoEncoder2Configuration Configuration)
        {
            throw new NotImplementedException();
        }

        public override void SetAudioSourceConfiguration(AudioSourceConfiguration Configuration)
        {
            throw new NotImplementedException();
        }

        public override void SetAudioEncoderConfiguration(AudioEncoder2Configuration Configuration)
        {
            throw new NotImplementedException();
        }

        public override void SetMetadataConfiguration(MetadataConfiguration Configuration)
        {
            throw new NotImplementedException();
        }

        public override void SetAudioOutputConfiguration(AudioOutputConfiguration Configuration)
        {
            throw new NotImplementedException();
        }

        public override void SetAudioDecoderConfiguration(AudioDecoderConfiguration Configuration)
        {
            throw new NotImplementedException();
        }

        public override VideoSourceConfigurationOptions GetVideoSourceConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override VideoEncoder2ConfigurationOptions[] GetVideoEncoderConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override AudioSourceConfigurationOptions GetAudioSourceConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override AudioEncoder2ConfigurationOptions[] GetAudioEncoderConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override MetadataConfigurationOptions GetMetadataConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override AudioOutputConfigurationOptions GetAudioOutputConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override AudioEncoder2ConfigurationOptions[] GetAudioDecoderConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override EncoderInstanceInfo GetVideoEncoderInstances(string ConfigurationToken)
        {
            throw new NotImplementedException();
        }

        public override string GetStreamUri(string Protocol, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override void StartMulticastStreaming(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override void StopMulticastStreaming(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override void SetSynchronizationPoint(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override string GetSnapshotUri(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override VideoSourceMode[] GetVideoSourceModes(string VideoSourceToken)
        {
            throw new NotImplementedException();
        }

        public override bool SetVideoSourceMode(string VideoSourceToken, string VideoSourceModeToken)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetOSDs", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("OSDs")]
        public override OSDConfiguration[] GetOSDs(string OSDToken, string ConfigurationToken)
        {
            if ((OSDToken == null) && (ConfigurationToken == null))
            {
                return ONVIFMedia2Configuration.OSDConfigurationList.ToArray();
            }

            if ((OSDToken != null) && (ConfigurationToken != null))
            {
                throw ONVIFFault.GetGeneralException_InvalidArgVal("Both OSDToken and ConfigurationToken are not allowed.");
            }

            if ((OSDToken != null) && (ConfigurationToken == null))
            {
                if (ONVIFMedia2Configuration.OSDConfigurationList.Any(C => C.token == OSDToken))
                {
                    return (ONVIFMedia2Configuration.OSDConfigurationList.FindAll(C => C.token == OSDToken)).ToArray();
                }
                else
                {
                    throw ONVIFFault.GetMedia2Exception_InvalidArgVal_NoConfig(OSDToken);
                }
            }

            if ((OSDToken == null) && (ConfigurationToken != null))
            {
                if (ONVIFMedia2Configuration.VideoSourceConfigurationList.Any(C => C.token == ConfigurationToken))
                {
                    return (ONVIFMedia2Configuration.OSDConfigurationList.FindAll(C => C.VideoSourceConfigurationToken.Value == ConfigurationToken)).ToArray();
                }
                else
                {
                    throw ONVIFFault.GetMedia2Exception_InvalidArgVal_NoConfig(ConfigurationToken);
                }
            }

            return null;

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/GetOSDOptions", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("OSDOptions")]
        public override OSDConfigurationOptions GetOSDOptions(string ConfigurationToken)
        {
            if (ONVIFMedia2Configuration.VideoSourceConfigurationList.Any(C => C.token == ConfigurationToken))
            {
                return (ONVIFMedia2Configuration.OSDOptionsList.Find(C => C.VideoSourceConfigurationToken1 == ConfigurationToken)).OSDConfigurationOptions;
            }
            else
            {
                throw ONVIFFault.GetMedia2Exception_InvalidArgVal_NoConfig(ConfigurationToken);
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/SetOSD", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseElementName = "SetOSDResponse", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetOSD(OSDConfiguration OSD)
        {
            OSDConfigurationOptions OSDOpt = this.GetOSDOptions(OSD.VideoSourceConfigurationToken.Value);
            var OSDList = ONVIFMedia2Configuration.OSDConfigurationList.FindAll(C => (C.VideoSourceConfigurationToken.Value == OSD.VideoSourceConfigurationToken.Value) && (C.token != OSD.token));

            ONVIFMedia2Configuration.OSDParametersCheck(OSD, OSDOpt, OSDList);

            ONVIFMedia2Configuration.OSDConfigurationList.RemoveAll(C => C.token == OSD.token);
            ONVIFMedia2Configuration.OSDConfigurationList.Add(OSD);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/CreateOSD", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("OSDToken")]
        public override string CreateOSD(OSDConfiguration OSD)
        {
            OSDConfigurationOptions OSDOpt = this.GetOSDOptions(OSD.VideoSourceConfigurationToken.Value);
            var OSDList = ONVIFMedia2Configuration.OSDConfigurationList.FindAll(C => C.VideoSourceConfigurationToken.Value == OSD.VideoSourceConfigurationToken.Value);

            ONVIFMedia2Configuration.OSDParametersCheck(OSD, OSDOpt, OSDList);


            while (ONVIFMedia2Configuration.OSDConfigurationList.Any(C => C.token == OSD.token))
            {
                OSD.token = HelperCommon.RandomStr();
            }

            ONVIFMedia2Configuration.OSDConfigurationList.Add(OSD);

            return OSD.token;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver20/media/wsdl/DeleteOSD", RequestNamespace = "http://www.onvif.org/ver20/media/wsdl", ResponseElementName = "DeleteOSDResponse", ResponseNamespace = "http://www.onvif.org/ver20/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteOSD(string OSDToken)
        {
            if (ONVIFMedia2Configuration.OSDConfigurationList.Any(C => C.token == OSDToken))
            {
                ONVIFMedia2Configuration.OSDConfigurationList.RemoveAll(C => C.token == OSDToken);
            }
            else
            {
                throw ONVIFFault.GetMedia2Exception_InvalidArgVal_NoConfig(OSDToken);
            }
        }
    }
}
