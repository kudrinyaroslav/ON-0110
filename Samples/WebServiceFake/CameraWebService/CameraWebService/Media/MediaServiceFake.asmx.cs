using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services.Protocols;
using System.Xml;
using Media;
using System.Linq;

namespace CameraWebService
{
    public class MediaServiceFake : Media.MediaBinding
    {

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetServiceCapabilities", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetServiceCapabilitiesResponse", Namespace = "http://www.onvif.org/ver10/media/wsdl")]
        public override GetServiceCapabilitiesResponse GetServiceCapabilities([System.Xml.Serialization.XmlElementAttribute("GetServiceCapabilities", Namespace = "http://www.onvif.org/ver10/media/wsdl")] GetServiceCapabilities GetServiceCapabilities1)
        {
            GetServiceCapabilitiesResponse response = new GetServiceCapabilitiesResponse();
            response.Capabilities = new Media.Capabilities();
            response.Capabilities.ProfileCapabilities = new Media.ProfileCapabilities();
            response.Capabilities.StreamingCapabilities = new Media.StreamingCapabilities();
            response.Capabilities.StreamingCapabilities.RTP_RTSP_TCP = true;
            response.Capabilities.StreamingCapabilities.RTP_TCP = true;
            return response;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdlGetVideoSources/", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("VideoSources")]
        public override Media.VideoSource[] GetVideoSources()
        {
            return CameraWebService.MediaService.MediaStorage.Instance.VideoSources.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioSources", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AudioSources")]
        public override Media.AudioSource[] GetAudioSources()
        {
            return CameraWebService.MediaService.MediaStorage.Instance.AudioSources.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/CreateProfile", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Profile")]
        public override Media.Profile CreateProfile(string Name, string Token)
        {
            if (CameraWebService.MediaService.MediaStorage.Instance.Profiles.Count > CameraWebService.MediaService.MediaStorage.Instance.MaxNumberOfProfiles)
            {
                ReturnFault(new string[] { "Receiver", "Action", "MaxNVTProfiles" });
            }

            string token1 = Token;
            if (string.IsNullOrEmpty(Token))
            {
                token1 = Name;
            }
            Media.Profile profile = new Media.Profile() {Name =  Name, token = token1, @fixed=false, fixedSpecified = true};
            CameraWebService.MediaService.MediaStorage.Instance.Profiles.Add(profile);
            return profile;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdlGetProfile/", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Profile")]
        public override Media.Profile GetProfile(string ProfileToken)
        {
            return FindProfile(ProfileToken);
        }


        [System.Web.Services.WebMethodAttribute()]
        [SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetProfiles", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [ScriptDriven()]
        [return: System.Xml.Serialization.XmlElementAttribute("Profiles")]
        public override Media.Profile[] GetProfiles()
        {
            return CameraWebService.MediaService.MediaStorage.Instance.Profiles.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddVideoEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddVideoEncoderConfiguration(string ProfileToken, string ConfigurationToken)
        {
            Profile profile = FindProfile(ProfileToken);
            if (profile.VideoSourceConfiguration == null)
            {
                ReturnFault(new string[]{"Receiver", "ActionNotSupported"});
            }

            MediaService.MediaStorage.Instance.AddVideoEncoderConfiguration(profile,
                                                                            FindVideoEncoderConfiguration(ConfigurationToken));    
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemoveVideoEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveVideoEncoderConfiguration(string ProfileToken)
        {
            MediaService.MediaStorage.Instance.RemoveVideoEncoderConfiguration(FindProfile(ProfileToken));
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddVideoSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddVideoSourceConfiguration(string ProfileToken, string ConfigurationToken)
        {
            MediaService.MediaStorage.Instance.AddVideoSourceConfiguration(FindProfile(ProfileToken),
                                                                           FindVideoSourceConfiguration(ConfigurationToken));
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemoveVideoSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveVideoSourceConfiguration(string ProfileToken)
        {
            Profile profile = FindProfile(ProfileToken);
            if (profile.VideoEncoderConfiguration != null)
            {
                ReturnFault(new string[] { "Receiver", "ActionNotSupported" });
            }

            MediaService.MediaStorage.Instance.RemoveVideoSourceConfiguration(profile);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddAudioEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddAudioEncoderConfiguration(string ProfileToken, string ConfigurationToken)
        {
            Profile profile = FindProfile(ProfileToken);
            if (profile.AudioSourceConfiguration == null)
            {
                ReturnFault(new string[] { "Receiver", "ActionNotSupported" });
            }

            MediaService.MediaStorage.Instance.AddAudioEncoderConfiguration(profile,
                                                                           FindAudioEncoderConfiguration(ConfigurationToken));
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemoveAudioEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveAudioEncoderConfiguration(string ProfileToken)
        {
            MediaService.MediaStorage.Instance.RemoveAudioEncoderConfiguration(FindProfile(ProfileToken));
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddAudioSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddAudioSourceConfiguration(string ProfileToken, string ConfigurationToken)
        {
            MediaService.MediaStorage.Instance.AddAudioSourceConfiguration(FindProfile(ProfileToken),
                                                                           FindAudioSourceConfiguration(ConfigurationToken));
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemoveAudioSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemoveAudioSourceConfiguration(string ProfileToken)
        {
            Profile profile = FindProfile(ProfileToken);
            if (profile.AudioEncoderConfiguration != null)
            {
                ReturnFault(new string[] { "Receiver", "ActionNotSupported" });
            }

            MediaService.MediaStorage.Instance.RemoveAudioSourceConfiguration(profile);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/AddPTZConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AddPTZConfiguration(string ProfileToken, string ConfigurationToken)
        {
            
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/RemovePTZConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void RemovePTZConfiguration(string ProfileToken)
        {
            
        }

        public override void AddVideoAnalyticsConfiguration(string ProfileToken, string ConfigurationToken)
        {
            
        }

        public override void RemoveVideoAnalyticsConfiguration(string ProfileToken)
        {
            
        }

        public override void AddMetadataConfiguration(string ProfileToken, string ConfigurationToken)
        {
            
        }

        public override void RemoveMetadataConfiguration(string ProfileToken)
        {
            
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/DeleteProfile", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteProfile(string ProfileToken)
        {
            MediaService.MediaStorage.Instance.DeleteProfile(FindProfile(ProfileToken));
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetVideoSourceConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override Media.VideoSourceConfiguration[] GetVideoSourceConfigurations()
        {
            return MediaService.MediaStorage.Instance.VideoSourceConfigurations.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetVideoEncoderConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override Media.VideoEncoderConfiguration[] GetVideoEncoderConfigurations()
        {
            return MediaService.MediaStorage.Instance.VideoEncoderConfigurations.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdlGetAudioSourceConfigurations/", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override Media.AudioSourceConfiguration[] GetAudioSourceConfigurations()
        {
            return MediaService.MediaStorage.Instance.AudioSourceConfigurations.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioEncoderConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override Media.AudioEncoderConfiguration[] GetAudioEncoderConfigurations()
        {
            return MediaService.MediaStorage.Instance.AudioEncoderConfigurations.ToArray();
        }

        public override Media.VideoAnalyticsConfiguration[] GetVideoAnalyticsConfigurations()
        {
            throw new NotImplementedException();
        }

        public override Media.MetadataConfiguration[] GetMetadataConfigurations()
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetVideoSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override Media.VideoSourceConfiguration GetVideoSourceConfiguration(string ConfigurationToken)
        {
            Media.VideoSourceConfiguration configuration = FindVideoSourceConfiguration(ConfigurationToken);

            configuration.Bounds.height = configuration.Bounds.height*10;
            configuration.Bounds.width = configuration.Bounds.width * 10;
            configuration.Bounds.y = configuration.Bounds.y * 10;
            configuration.Bounds.x = configuration.Bounds.x * 10;

            return configuration;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetVideoEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override Media.VideoEncoderConfiguration GetVideoEncoderConfiguration(string ConfigurationToken)
        {
            return FindVideoEncoderConfiguration(ConfigurationToken);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override Media.AudioSourceConfiguration GetAudioSourceConfiguration(string ConfigurationToken)
        {
            return FindAudioSourceConfiguration(ConfigurationToken);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override Media.AudioEncoderConfiguration GetAudioEncoderConfiguration(string ConfigurationToken)
        {
            return FindAudioEncoderConfiguration(ConfigurationToken);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetVideoAnalyticsConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override Media.VideoAnalyticsConfiguration GetVideoAnalyticsConfiguration(string ConfigurationToken)
        {
            throw new NotImplementedException();
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetMetadataConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override Media.MetadataConfiguration GetMetadataConfiguration(string ConfigurationToken)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetCompatibleVideoEncoderConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override Media.VideoEncoderConfiguration[] GetCompatibleVideoEncoderConfigurations(string ProfileToken)
        {
            return MediaService.MediaStorage.Instance.VideoEncoderConfigurations.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetCompatibleVideoSourceConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override Media.VideoSourceConfiguration[] GetCompatibleVideoSourceConfigurations(string ProfileToken)
        {
            return MediaService.MediaStorage.Instance.VideoSourceConfigurations.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetCompatibleAudioEncoderConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override Media.AudioEncoderConfiguration[] GetCompatibleAudioEncoderConfigurations(string ProfileToken)
        {
            return MediaService.MediaStorage.Instance.AudioEncoderConfigurations.ToArray();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetCompatibleAudioSourceConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override Media.AudioSourceConfiguration[] GetCompatibleAudioSourceConfigurations(string ProfileToken)
        {
            return MediaService.MediaStorage.Instance.AudioSourceConfigurations.ToArray();
        }

        public override Media.VideoAnalyticsConfiguration[] GetCompatibleVideoAnalyticsConfigurations(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override Media.MetadataConfiguration[] GetCompatibleMetadataConfigurations(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetVideoSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetVideoSourceConfiguration(Media.VideoSourceConfiguration Configuration, bool ForcePersistence)
        {
            Media.VideoSourceConfiguration configuration = FindVideoSourceConfiguration(Configuration.token);
            configuration.Bounds = Configuration.Bounds;
            configuration.Name = Configuration.Name;
            configuration.SourceToken = Configuration.SourceToken;
        }

        [System.Web.Services.WebMethodAttribute()]
        [SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetVideoEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public override void SetVideoEncoderConfiguration(Media.VideoEncoderConfiguration Configuration, bool ForcePersistence)
        {
            Media.VideoEncoderConfiguration config =
                FindVideoEncoderConfiguration(Configuration.token);

            if (config == null)
            {
                ReturnFault(new string[]{"Sender", "InvalidArgVal", "NoConfig"});
            }

            Media.VideoResolution[] res = null;
            if (Configuration.Encoding == Media.VideoEncoding.JPEG)
            {
                res = MediaService.MediaStorage.Instance.VideoEncoderConfigurationOptions.JPEG.ResolutionsAvailable;
            }
            if (Configuration.Encoding == Media.VideoEncoding.MPEG4)
            {
                res = MediaService.MediaStorage.Instance.VideoEncoderConfigurationOptions.MPEG4.ResolutionsAvailable;
            }
            if (Configuration.Encoding == Media.VideoEncoding.H264)
            {
                res = MediaService.MediaStorage.Instance.VideoEncoderConfigurationOptions.H264.ResolutionsAvailable;
            }

            Media.VideoResolution check =
                res.Where(r => r.Height == Configuration.Resolution.Height && r.Width == Configuration.Resolution.Width)
                    .FirstOrDefault();

            if (check == null)
            {
                ReturnFault(new string[]{"Sender", "InvalidArgVal", "ConfigModify"});
            }
            
            Media.VideoEncoderConfiguration configuration = FindVideoEncoderConfiguration(Configuration.token);
            configuration.Name = Configuration.Name;
            configuration.H264 = Configuration.H264;
            configuration.MPEG4 = Configuration.MPEG4;
            configuration.Multicast = Configuration.Multicast;
            configuration.Quality = Configuration.Quality;
            configuration.Encoding = Configuration.Encoding;

            configuration.RateControl = Configuration.RateControl;
            if (Configuration.Resolution.Height > 200)
            {
                configuration.Resolution = Configuration.Resolution;
            }
            configuration.SessionTimeout = Configuration.SessionTimeout;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetAudioSourceConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetAudioSourceConfiguration(Media.AudioSourceConfiguration Configuration, bool ForcePersistence)
        {
            Media.AudioSourceConfiguration configuration = FindAudioSourceConfiguration(Configuration.token);
            configuration.Name = Configuration.Name;
            configuration.SourceToken = Configuration.SourceToken;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetAudioEncoderConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetAudioEncoderConfiguration(Media.AudioEncoderConfiguration Configuration, bool ForcePersistence)
        {
            Media.AudioEncoderConfiguration configuration = FindAudioEncoderConfiguration(Configuration.token);
            configuration.Name = Configuration.Name;
            configuration.Bitrate = Configuration.Bitrate;
            configuration.Encoding = Configuration.Encoding;
            configuration.Multicast = Configuration.Multicast;
            configuration.SampleRate = Configuration.SampleRate;
            configuration.SessionTimeout = Configuration.SessionTimeout;
        }

        public override void SetVideoAnalyticsConfiguration(Media.VideoAnalyticsConfiguration Configuration, bool ForcePersistence)
        {
            
        }

        public override void SetMetadataConfiguration(Media.MetadataConfiguration Configuration, bool ForcePersistence)
        {
            
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdlGetVideoSourceConfigurationOptions/", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override Media.VideoSourceConfigurationOptions GetVideoSourceConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            Media.VideoSourceConfigurationOptions options = new Media.VideoSourceConfigurationOptions() ;

            options.BoundsRange = 
                new Media.IntRectangleRange()
                    {
                        HeightRange = new Media.IntRange(){Min = 0, Max = 800},
                        WidthRange = new Media.IntRange(){Min = 0, Max = 600},
                        XRange = new Media.IntRange(){Min = 0, Max = 800},
                        YRange = new Media.IntRange(){Min = 0, Max = 600}
                    };

            options.VideoSourceTokensAvailable = new string[]
                                                     {
                                                         MediaService.MediaStorage.Instance.VideoSources[0].token,
                                                         MediaService.MediaStorage.Instance.VideoSources[1].token
                                                     };
            return options;
        }

        [System.Web.Services.WebMethodAttribute()]
        [SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetVideoEncoderConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override Media.VideoEncoderConfigurationOptions GetVideoEncoderConfigurationOptions(
            string ConfigurationToken, 
            string ProfileToken)
        {
            Media.VideoEncoderConfigurationOptions options = MediaService.MediaStorage.Instance.VideoEncoderConfigurationOptions;
            
            Media.VideoEncoderConfigurationOptions result = new VideoEncoderConfigurationOptions();

            if (!string.IsNullOrEmpty(ProfileToken))
            {
                Profile profile =
                    MediaService.MediaStorage.Instance.Profiles.Where(p => p.token == ProfileToken).FirstOrDefault();

                if (profile.VideoEncoderConfiguration != null)
                {
                    switch (profile.VideoEncoderConfiguration.Encoding)
                    {
                        case VideoEncoding.JPEG:
                            result.JPEG = options.JPEG;
                            break;
                        case VideoEncoding.H264:
                            result.H264 = options.H264;
                            break;
                        case VideoEncoding.MPEG4:
                            result.MPEG4 = options.MPEG4;
                            break;
                    }
                }
            }
            else
            { 
                result.JPEG = options.JPEG;
                result.H264 = options.H264;
                result.MPEG4 = options.MPEG4;
            }
            return options;        
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioSourceConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override Media.AudioSourceConfigurationOptions GetAudioSourceConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            if (!string.IsNullOrEmpty(ConfigurationToken))
            {
                FindAudioSourceConfiguration(ConfigurationToken);
            }
            if (!string.IsNullOrEmpty(ProfileToken))
            {
                FindProfile(ProfileToken);
            }

            Media.AudioSourceConfigurationOptions options = new Media.AudioSourceConfigurationOptions();

            options.InputTokensAvailable = new string[]                                                     
             {
                 MediaService.MediaStorage.Instance.AudioSourceConfigurations[0].token
             };
            options.Extension = new Media.AudioSourceOptionsExtension();

            return options;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioEncoderConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override Media.AudioEncoderConfigurationOptions 
            GetAudioEncoderConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            Profile profile = null;
            if (!string.IsNullOrEmpty(ConfigurationToken))
            {
                FindAudioEncoderConfiguration(ConfigurationToken);
            }
            if (!string.IsNullOrEmpty(ProfileToken))
            {
                profile = FindProfile(ProfileToken);
            }

            Media.AudioEncoderConfigurationOptions options = new Media.AudioEncoderConfigurationOptions();

            Media.AudioEncoderConfigurationOption opt1 = new Media.AudioEncoderConfigurationOption();
            opt1.BitrateList = new int[]{1,2,3,4};
            opt1.SampleRateList = new int[] {10, 30};
            opt1.Encoding = Media.AudioEncoding.AAC;
            
            Media.AudioEncoderConfigurationOption opt2 = new Media.AudioEncoderConfigurationOption();
            opt2.BitrateList = new int[] { 8,7,6,5 };
            opt2.Encoding = Media.AudioEncoding.G726;
            opt2.SampleRateList = new int[]{19,21};

            Media.AudioEncoderConfigurationOption opt3 = new Media.AudioEncoderConfigurationOption();
            opt3.BitrateList = new int[] { 10, 11 };
            opt3.Encoding = Media.AudioEncoding.G711;
            opt3.SampleRateList = new int[]{20, 25, 30, 50};

            if (string.IsNullOrEmpty(ConfigurationToken))
            {
                if (profile != null)
                {
                    if (profile.AudioEncoderConfiguration != null)
                    {
                        //switch (profile.AudioEncoderConfiguration.Encoding)
                        //{ 
                        //    case AudioEncoding.G711:
                        //        options.Options = new AudioEncoderConfigurationOption[] { opt3 }; break;
                        //    case AudioEncoding.G726:
                        //        options.Options = new AudioEncoderConfigurationOption[] { opt2 }; break;
                        //    case AudioEncoding.AAC:
                        //        options.Options = new AudioEncoderConfigurationOption[] { opt1 }; break;

                        //}
                        options.Options = new Media.AudioEncoderConfigurationOption[] { opt1, opt2, opt3 };

                    }
                    else
                    {
                        options.Options = new Media.AudioEncoderConfigurationOption[] { opt1, opt2, opt3 };
                    }
                }
                else 
                {
                    options.Options = new Media.AudioEncoderConfigurationOption[] { opt1, opt2, opt3 };
                }
            }
            else 
            {
                options.Options = new Media.AudioEncoderConfigurationOption[] { opt1, opt2, opt3};
            }
            return options;
        }

        public override Media.MetadataConfigurationOptions GetMetadataConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetStreamUri", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("MediaUri")]
        public override Media.MediaUri GetStreamUri(Media.StreamSetup StreamSetup, string ProfileToken)
        {
            //if (StreamSetup.Transport.Protocol == Media.TransportProtocol.UDP)
            //{
            //    SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("InvalidStreamSetup", "http://www.onvif.org/ver10/error"));
            //    SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("InvalidArgVal", "http://www.onvif.org/ver10/error"), subCode);
            //    SoapException exception = new SoapException("Invalid Argument", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
            //    throw exception;
            //}

            string token = ProfileToken.ToLower();

            if (MediaService.MediaStorage.Instance.Profiles.Where( P => P.token.ToLower() == token).FirstOrDefault() == null )
            {
                SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("NoProfile", "http://www.onvif.org/ver10/error"));
                SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("InvalidArgs", "http://www.onvif.org/ver10/error"), subCode);
                SoapException exception = new SoapException("Invalid Argument", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
                throw exception;
            }

            Media.MediaUri uri = new Media.MediaUri();
            uri.InvalidAfterConnect = false;
            uri.InvalidAfterReboot = true;
            uri.Timeout = "PT60S";
            uri.Uri = HttpContext.Current.Request.Url.Host + "/media.asp";

            return uri;
        
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/StartMulticastStreaming", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void StartMulticastStreaming(string ProfileToken)
        {
            
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/StopMulticastStreaming", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void StopMulticastStreaming(string ProfileToken)
        {
            
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetSynchronizationPoint", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetSynchronizationPoint(string ProfileToken)
        {
            
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetSnapshotUri", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("MediaUri")]
        public override Media.MediaUri GetSnapshotUri(string ProfileToken)
        {
            Media.MediaUri response = new Media.MediaUri();
            response.Uri = string.Format("http://{0}/6221295.jpg", HttpContext.Current.Request.Url.Authority);
            //response.Uri = "http://localhost/Secret/astrosoft-development/App_Themes/default/images/Logo.png";
            response.InvalidAfterConnect = false;
            response.InvalidAfterReboot = true;
            response.Timeout = "PT60S";
            return response;
        }

        [System.Web.Services.WebMethodAttribute()]
        [SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/SetAudioOutputConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public override void SetAudioOutputConfiguration(
            Media.AudioOutputConfiguration Configuration, bool ForcePersistence)
        {

            AudioOutputConfigurationOptions options = GetAudioOutputConfigurationOptions(Configuration.token, string.Empty);

            if (options.OutputLevelRange.Min > Configuration.OutputLevel ||
                options.OutputLevelRange.Max < Configuration.OutputLevel ||
                !options.OutputTokensAvailable.Contains(Configuration.OutputToken) || 
                !options.SendPrimacyOptions.Contains(Configuration.SendPrimacy))
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "ConfigModify" });
            }

            AudioOutputConfiguration config = FindAudioOutputConfiguration(Configuration.token);

            config.OutputLevel = Configuration.OutputLevel;
            config.OutputToken = Configuration.OutputToken;
            config.SendPrimacy = Configuration.SendPrimacy;
            config.Name = Configuration.Name;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioOutputConfigurations", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override Media.AudioOutputConfiguration[] GetAudioOutputConfigurations()
        {
            return MediaService.MediaStorage.Instance.AudioOutputConfigurations.ToArray();
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioOutputConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override Media.AudioOutputConfigurationOptions GetAudioOutputConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            Media.AudioOutputConfigurationOptions options = new Media.AudioOutputConfigurationOptions();

            options.OutputLevelRange = new Media.IntRange(){Min=5, Max = 15};
            options.OutputTokensAvailable = new string[]{"token"};
            options.SendPrimacyOptions = new string[]{"a","b", "c"};

            return options;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioOutputConfiguration", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override Media.AudioOutputConfiguration GetAudioOutputConfiguration(string ConfigurationToken)
        {
            return FindAudioOutputConfiguration(ConfigurationToken);
        }

        void ThrowInvalidArgVal()
        {
            ReturnFault(new string[] { "Sender", "ActionNotSupported", "InvalidArgValVal" });
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



        Media.Profile FindProfile(string ProfileToken)
        {
            Media.Profile profile = MediaService.MediaStorage.Instance.Profiles.Where(p => p.token.ToLower() == ProfileToken.ToLower()).
                    FirstOrDefault();

            if (profile == null)
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "NoProfile" });
            }

            return profile;
        }

        Media.VideoSourceConfiguration FindVideoSourceConfiguration(string configurationToken)
        {
            Media.VideoSourceConfiguration configuration = MediaService.MediaStorage.Instance.VideoSourceConfigurations.Where(p => p.token.ToLower() == configurationToken.ToLower()).
                    FirstOrDefault();

            if (configuration == null)
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "InvalidToken" });
            }

            return configuration;
        }

        Media.VideoEncoderConfiguration FindVideoEncoderConfiguration(string configurationToken)
        {
            Media.VideoEncoderConfiguration configuration = MediaService.MediaStorage.Instance.VideoEncoderConfigurations.Where(p => p.token.ToLower() == configurationToken.ToLower()).
                    FirstOrDefault();

            if (configuration == null)
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "InvalidToken" });
            }

            return configuration;
        }

        Media.AudioSourceConfiguration FindAudioSourceConfiguration(string configurationToken)
        {
            Media.AudioSourceConfiguration configuration = MediaService.MediaStorage.Instance.AudioSourceConfigurations.Where(p => p.token.ToLower() == configurationToken.ToLower()).
                    FirstOrDefault();

            if (configuration == null)
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "InvalidToken" });
            }

            return configuration;
        }

        Media.AudioEncoderConfiguration FindAudioEncoderConfiguration(string configurationToken)
        {
            Media.AudioEncoderConfiguration configuration = MediaService.MediaStorage.Instance.AudioEncoderConfigurations.Where(p => p.token.ToLower() == configurationToken.ToLower()).
                    FirstOrDefault();

            if (configuration == null)
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "InvalidToken" });
            }

            return configuration;
        }

        Media.AudioOutputConfiguration FindAudioOutputConfiguration(string configurationToken)
        {
            Media.AudioOutputConfiguration configuration = MediaService.MediaStorage.Instance.AudioOutputConfigurations.Where(p => p.token.ToLower() == configurationToken.ToLower()).
                    FirstOrDefault();

            if (configuration == null)
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "InvalidToken" });
            }

            return configuration;
        }



        public override Media.AudioOutput[] GetAudioOutputs()
        {
            throw new NotImplementedException();
        }

        public override void AddAudioOutputConfiguration(string ProfileToken, string ConfigurationToken)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAudioOutputConfiguration(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override void AddAudioDecoderConfiguration(string ProfileToken, string ConfigurationToken)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAudioDecoderConfiguration(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override Media.AudioDecoderConfiguration[] GetAudioDecoderConfigurations()
        {
            throw new NotImplementedException();
        }

        public override Media.AudioDecoderConfiguration GetAudioDecoderConfiguration(string ConfigurationToken)
        {
            throw new NotImplementedException();
        }

        public override Media.AudioOutputConfiguration[] GetCompatibleAudioOutputConfigurations(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override Media.AudioDecoderConfiguration[] GetCompatibleAudioDecoderConfigurations(string ProfileToken)
        {
            throw new NotImplementedException();
        }

        public override void SetAudioDecoderConfiguration(Media.AudioDecoderConfiguration Configuration, bool ForcePersistence)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/GetAudioDecoderConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        public override Media.AudioDecoderConfigurationOptions GetAudioDecoderConfigurationOptions(string ConfigurationToken, string ProfileToken)
        {
            AudioDecoderConfigurationOptions options = new AudioDecoderConfigurationOptions();
            options.G711DecOptions = new G711DecOptions();
            options.G711DecOptions.Bitrate = new int[]{128,256};
            options.G711DecOptions.SampleRateRange = new int[] { 128, 256 };
            options.G726DecOptions = new G726DecOptions();
            options.G726DecOptions.Bitrate = new int[] { 128, 256 };
            options.G726DecOptions.SampleRateRange = new int[] { 128, 256 };
            return options;


        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/media/wsdl/ GetGuaranteedNumberOfVideoEncoderInstances" +
            "", RequestNamespace = "http://www.onvif.org/ver10/media/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/media/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("TotalNumber")]
        public override int GetGuaranteedNumberOfVideoEncoderInstances(string ConfigurationToken, 
            out int JPEG, [System.Xml.Serialization.XmlIgnoreAttribute()] out bool JPEGSpecified, 
            out int H264, [System.Xml.Serialization.XmlIgnoreAttribute()] out bool H264Specified, 
            out int MPEG4, [System.Xml.Serialization.XmlIgnoreAttribute()] out bool MPEG4Specified)
        {
            JPEG = 1;
            JPEGSpecified = true;
            H264 = 2;
            H264Specified = true;
            MPEG4 = 0;
            MPEG4Specified = false;
            return 4;
        }

    }
}
