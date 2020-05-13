using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DUT.CameraWebService.Common;
using System.Web.Services.Protocols;

namespace DUT.CameraWebService.Media2SVC
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Media2" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Media2.svc or Media2.svc.cs at the Solution Explorer and start debugging.
    [DispatchByBodyElementServiceBehaviorAttribute]
    public class Media2SVCService : Base.BaseDutService, Media2Binding
    {
        //TestSuit
        Media2SVCServiceTest Media2SVCServiceTest
        {
            get
            {
                if (Application[Base.AppVars.MEDIA2SVCSERVICE] != null)
                {
                    return (Media2SVCServiceTest)Application[Base.AppVars.MEDIA2SVCSERVICE];
                }
                else
                {
                    Media2SVCServiceTest serviceTest = new Media2SVCServiceTest(TestCommon);
                    Application[Base.AppVars.MEDIA2SVCSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }

        public void DoWork()
        {
        }

        public GetServiceCapabilitiesResponse GetServiceCapabilities(GetServiceCapabilitiesRequest request)
        {
            GetServiceCapabilitiesResponse result = new GetServiceCapabilitiesResponse();

            ParametersValidation validation = new ParametersValidation();
            
            result.Capabilities = (Capabilities2)ExecuteGetCommand(validation, Media2SVCServiceTest.GetServiceCapabilitiesTest);
            
            return result;
        }

        public CreateProfileResponse CreateProfile(CreateProfileRequest request)
        {
            CreateProfileResponse result = new CreateProfileResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Name", request.Name);
            //if (request.Configuration != null)
            //{
            //    validation.Add(ParameterType.String, "Configuration/Type", request.Configuration[0].Type.ToString());
            //    validation.Add(ParameterType.OptionalString, "Configuration/Token", request.Configuration[0].Token);
            //}
            
            result.Token = (string)ExecuteGetCommand(validation, Media2SVCServiceTest.CreateProfileTest);

            return result;
        }

        public GetProfilesResponse GetProfiles(GetProfilesRequest request)
        {
            GetProfilesResponse result = new GetProfilesResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "Token", request.Token);
            if (request.Type != null || request.Type.Count() > 0)
            {
                validation.Add(ParameterType.StringArray, "Type", request.Type);
            }

            result.Profiles = (MediaProfile[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetProfilesTest);

            return result;
        }

        public AddConfigurationResponse AddConfiguration(AddConfigurationRequest request)
        {
            AddConfigurationResponse result = new AddConfigurationResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ProfileToken", request.ProfileToken);
            validation.Add(ParameterType.String, "Name", request.Name);
            validation.Add(ParameterType.OptionalElement, "Configuration", request.Configuration);
            if (request.Configuration != null)
            {
                validation.Add(ParameterType.String, "Configuration/Type", request.Configuration[0].Type.ToString());
                validation.Add(ParameterType.OptionalString, "Configuration/Token", request.Configuration[0].Token);
            }
            
            ExecuteVoidCommand(validation, Media2SVCServiceTest.AddConfigurationTest);

            return result;
        }

        public RemoveConfigurationResponse RemoveConfiguration(RemoveConfigurationRequest request)
        {
            RemoveConfigurationResponse result = new RemoveConfigurationResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ProfileToken", request.ProfileToken);
            validation.Add(ParameterType.String, "Configuration/Type", request.Configuration[0].Type);
            validation.Add(ParameterType.OptionalString, "Configuration/Token", request.Configuration[0].Token);

            ExecuteVoidCommand(validation, Media2SVCServiceTest.RemoveConfigurationTest);

            return result;
        }

        public DeleteProfileResponse DeleteProfile(DeleteProfileRequest request)
        {
            DeleteProfileResponse result = new DeleteProfileResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", request.Token);

            ExecuteVoidCommand(validation, Media2SVCServiceTest.DeleteProfileTest);

            return result;
        }

        public GetVideoSourceConfigurationsResponse GetVideoSourceConfigurations(GetVideoSourceConfigurationsRequest request)
        {
            GetVideoSourceConfigurationsResponse result = new GetVideoSourceConfigurationsResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);

            result.Configurations = (VideoSourceConfiguration[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetVideoSourceConfigurationsTest);
            
            return result;
        }

        public GetVideoEncoderConfigurationsResponse GetVideoEncoderConfigurations(GetVideoEncoderConfigurationsRequest request)
        {
            GetVideoEncoderConfigurationsResponse result = new GetVideoEncoderConfigurationsResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);

            result.Configurations = (VideoEncoder2Configuration[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetVideoEncoderConfigurationsTest);
            
            return result;
        }

        public GetAudioSourceConfigurationsResponse GetAudioSourceConfigurations(GetAudioSourceConfigurationsRequest request)
        {
            GetAudioSourceConfigurationsResponse result = new GetAudioSourceConfigurationsResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);
            result.Configurations = (AudioSourceConfiguration[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetAudioSourceConfigurationsTest);

            return result;
        }

        public GetAudioEncoderConfigurationsResponse GetAudioEncoderConfigurations(GetAudioEncoderConfigurationsRequest request)
        {
            GetAudioEncoderConfigurationsResponse result = new GetAudioEncoderConfigurationsResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);

            result.Configurations = (AudioEncoder2Configuration[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetAudioEncoderConfigurationsTest);
            return result;
        }

        public GetAnalyticsConfigurationsResponse GetAnalyticsConfigurations(GetAnalyticsConfigurationsRequest request)
        {
            GetAnalyticsConfigurationsResponse result = new GetAnalyticsConfigurationsResponse();
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);
            result.Configurations = (VideoAnalyticsConfiguration[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetAnalyticsConfigurationsTest);

            return result;
        }

        public GetMetadataConfigurationsResponse GetMetadataConfigurations(GetMetadataConfigurationsRequest request)
        {
            GetMetadataConfigurationsResponse result = new GetMetadataConfigurationsResponse();
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);

            result.Configurations = (MetadataConfiguration[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetMetadataConfigurationsTest);
            return result;
        }

        public GetAudioOutputConfigurationsResponse GetAudioOutputConfigurations(GetAudioOutputConfigurationsRequest request)
        {
            GetAudioOutputConfigurationsResponse result = new GetAudioOutputConfigurationsResponse();
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);

            result.Configurations = (AudioOutputConfiguration[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetAudioOutputConfigurationsTest);
            return result;
        }

        public GetAudioDecoderConfigurationsResponse GetAudioDecoderConfigurations(GetAudioDecoderConfigurationsRequest request)
        {
            GetAudioDecoderConfigurationsResponse result = new GetAudioDecoderConfigurationsResponse();
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);

            result.Configurations = (AudioDecoderConfiguration[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetAudioDecoderConfigurationsTest);
            return result;
        }

        public SetVideoSourceConfigurationResponse SetVideoSourceConfiguration(SetVideoSourceConfigurationRequest request)
        {
            SetVideoSourceConfigurationResponse result = new SetVideoSourceConfigurationResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSourceConfiguration/@token", request.Configuration.token);
            validation.Add(ParameterType.String, "VideoSourceConfiguration/Name", request.Configuration.Name);
            validation.Add(ParameterType.String, "VideoSourceConfiguration/SourceToken", request.Configuration.SourceToken);
            validation.Add(ParameterType.String, "VideoSourceConfiguration/Bounds", request.Configuration.Bounds.ToString());
            if (request.Configuration.Bounds != null)
            {
                validation.Add(ParameterType.Int, "VideoSourceConfiguration/Bounds/@x", request.Configuration.Bounds.x);
                validation.Add(ParameterType.Int, "VideoSourceConfiguration/Bounds/@y", request.Configuration.Bounds.y);
                validation.Add(ParameterType.Int, "VideoSourceConfiguration/Bounds/@width", request.Configuration.Bounds.width);
                validation.Add(ParameterType.Int, "VideoSourceConfiguration/Bounds/@height", request.Configuration.Bounds.height);
            }
            validation.Add(ParameterType.OptionalElement, "VideoSourceConfiguration/Extension", request.Configuration.Extension);
            if(request.Configuration.Extension != null)
            {
                validation.Add(ParameterType.OptionalElement, "VideoSourceConfiguration/Extension/Rotate", request.Configuration.Extension.Rotate);
                if (request.Configuration.Extension.Rotate != null)
                {
                    validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Rotate/Mode", request.Configuration.Extension.Rotate.Mode.ToString());
                    validation.Add(ParameterType.OptionalInt, "VideoSourceConfiguration/Extension/Rotate/Degree", request.Configuration.Extension.Rotate.Degree);
                    validation.Add(ParameterType.OptionalElement, "VideoSourceConfiguration/Extension/Rotate/Extension", request.Configuration.Extension.Rotate.Extension);
                }
                validation.Add(ParameterType.OptionalElement, "VideoSourceConfiguration/Extension/Extension", request.Configuration.Extension.Extension);
                if (request.Configuration.Extension.Extension != null)
                {
                    validation.Add(ParameterType.OptionalElement, "VideoSourceConfiguration/Extension/Extension/LensDescription", request.Configuration.Extension.Extension.LensDescription);
                    if (request.Configuration.Extension.Extension.LensDescription != null)
                    {
                        validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Extension/LensDescription/FocalLength", request.Configuration.Extension.Extension.LensDescription[0].FocalLength.ToString());
                        if (request.Configuration.Extension.Extension.LensDescription[0].Offset != null)
                        {
                            validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Extension/LensDescription/Offset/@x", request.Configuration.Extension.Extension.LensDescription[0].Offset.x.ToString());
                            validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Extension/LensDescription/Offset/@y", request.Configuration.Extension.Extension.LensDescription[0].Offset.x.ToString());
                        }
                        if (request.Configuration.Extension.Extension.LensDescription[0].Projection != null)
                        {
                            validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Extension/LensDescription/Projection/Angle", request.Configuration.Extension.Extension.LensDescription[0].Projection[0].Angle.ToString());
                            validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Extension/LensDescription/Projection/Radius", request.Configuration.Extension.Extension.LensDescription[0].Projection[0].Radius.ToString());
                            validation.Add(ParameterType.OptionalString, "VideoSourceConfiguration/Extension/Extension/LensDescription/Projection/Radius", request.Configuration.Extension.Extension.LensDescription[0].Projection[0].Transmittance.ToString());
                        }
                        validation.Add(ParameterType.String, "VideoSourceConfiguration/Extension/Extension/LensDescription/XFactor", request.Configuration.Extension.Extension.LensDescription[0].XFactor.ToString());
                    }
                }
            }

            ExecuteVoidCommand(validation, Media2SVCServiceTest.SetVideoSourceConfigurationTest);

            return result;
        }

        public SetVideoSourceConfigurationResponse SetVideoEncoderConfiguration(SetVideoEncoderConfigurationRequest request)
        {
            SetVideoSourceConfigurationResponse result = new SetVideoSourceConfigurationResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoEncoder2Configuration/@token", request.Configuration.token);
            validation.Add(ParameterType.String, "VideoEncoder2Configuration/Name", request.Configuration.Name);
            //validation.Add(ParameterType.OptionalElement, "VideoEncoder2Configuration/@GovLength", request.Configuration.GovLength);
            if (request.Configuration.GovLengthSpecified)
            {
                validation.Add(ParameterType.Int, "VideoEncoder2Configuration/@GovLength", request.Configuration.GovLength);
            }
            validation.Add(ParameterType.String, "VideoEncoder2Configuration/@Profile", request.Configuration.Profile);
            validation.Add(ParameterType.String, "VideoEncoder2Configuration/Encoding", request.Configuration.Encoding);
            validation.Add(ParameterType.Int, "VideoEncoder2Configuration/Resolution/Width", request.Configuration.Resolution.Width);
            validation.Add(ParameterType.Int, "VideoEncoder2Configuration/Resolution/Height", request.Configuration.Resolution.Height);
            validation.Add(ParameterType.OptionalElement, "VideoEncoder2Configuration/RateControl", request.Configuration.RateControl);
            if (request.Configuration.RateControl!= null)
            {
                if (request.Configuration.RateControl.ConstantBitRateSpecified)
                {
                    validation.Add(ParameterType.String, "VideoEncoder2Configuration/RateControl/ConstantBitRateSpecified", request.Configuration.RateControl.ConstantBitRate.ToString());
                }
                validation.Add(ParameterType.String, "VideoEncoder2Configuration/RateControl/FrameRateLimit", request.Configuration.RateControl.FrameRateLimit.ToString());
                validation.Add(ParameterType.Int, "VideoEncoder2Configuration/RateControl/BitrateLimit", request.Configuration.RateControl.BitrateLimit);
            }
            validation.Add(ParameterType.OptionalElement, "VideoEncoder2Configuration/Multicast", request.Configuration.Multicast);
            if (request.Configuration.Multicast != null)
            {
                //validation.Add(ParameterType.String, "VideoEncoder2Configuration/Multicast/Address", request.Configuration.Multicast.Address.ToString());
                if (request.Configuration.Multicast.Address != null)
                {
                    validation.Add(ParameterType.String, "VideoEncoder2Configuration/Multicast/Address/Type", request.Configuration.Multicast.Address.Type.ToString());
                    validation.Add(ParameterType.OptionalString, "VideoEncoder2Configuration/Multicast/Address/IPv4Address", request.Configuration.Multicast.Address.IPv4Address.ToString());
                    //validation.Add(ParameterType.OptionalString, "VideoEncoder2Configuration/Multicast/Address/IPv6Address", request.Configuration.Multicast.Address.IPv6Address.ToString());
                }
                validation.Add(ParameterType.Int, "VideoEncoder2Configuration/Multicast/Port", request.Configuration.Multicast.Port);
                validation.Add(ParameterType.Int, "VideoEncoder2Configuration/Multicast/TTL", request.Configuration.Multicast.TTL);
                validation.Add(ParameterType.String, "VideoEncoder2Configuration/Multicast/AutoStart", request.Configuration.Multicast.AutoStart.ToString());
            }

            validation.Add(ParameterType.String, "VideoEncoder2Configuration/Quality", request.Configuration.Quality.ToString());

            ExecuteVoidCommand(validation, Media2SVCServiceTest.SetVideoEncoderConfigurationTest);

            return result;
        }

        public SetVideoSourceConfigurationResponse SetAudioSourceConfiguration(SetAudioSourceConfigurationRequest request)
        {
            SetVideoSourceConfigurationResponse result = new SetVideoSourceConfigurationResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "AudioSourceConfiguration/@token", request.Configuration.token);
            validation.Add(ParameterType.String, "AudioSourceConfiguration/Name", request.Configuration.Name);
            validation.Add(ParameterType.String, "AudioSourceConfiguration/SourceToken", request.Configuration.SourceToken);

            ExecuteVoidCommand(validation, Media2SVCServiceTest.SetAudioSourceConfigurationTest);

            return result;
        }

        public SetVideoSourceConfigurationResponse SetAudioEncoderConfiguration(SetAudioEncoderConfigurationRequest request)
        {
            SetVideoSourceConfigurationResponse result = new SetVideoSourceConfigurationResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "AudioEncoder2Configuration/@token", request.Configuration.token);
            validation.Add(ParameterType.String, "AudioEncoder2Configuration/Name", request.Configuration.Name);
            validation.Add(ParameterType.String, "AudioEncoder2Configuration/Encoding", request.Configuration.Encoding);
            validation.Add(ParameterType.OptionalElement, "AudioEncoder2Configuration/Multicast", request.Configuration.Multicast);
            if (request.Configuration.Multicast != null)
            {
                validation.Add(ParameterType.String, "AudioEncoder2Configuration/Multicast/Address", request.Configuration.Multicast.Address.ToString());
                if (request.Configuration.Multicast.Address != null)
                {
                    validation.Add(ParameterType.String, "AudioEncoder2Configuration/Multicast/Address/Type", request.Configuration.Multicast.Address.Type.ToString());
                    validation.Add(ParameterType.OptionalString, "AudioEncoder2Configuration/Multicast/Address/IPv4Address", request.Configuration.Multicast.Address.IPv4Address);
                    validation.Add(ParameterType.OptionalString, "AudioEncoder2Configuration/Multicast/Address/IPv6Address", request.Configuration.Multicast.Address.IPv6Address);
                }
                validation.Add(ParameterType.Int, "AudioEncoder2Configuration/Multicast/Port", request.Configuration.Multicast.Port);
                validation.Add(ParameterType.Int, "AudioEncoder2Configuration/Multicast/TTL", request.Configuration.Multicast.TTL);
                validation.Add(ParameterType.String, "AudioEncoder2Configuration/Multicast/AutoStart", request.Configuration.Multicast.AutoStart.ToString());
            }
            validation.Add(ParameterType.Int, "AudioEncoder2Configuration/Bitrate", request.Configuration.Bitrate);
            validation.Add(ParameterType.Int, "AudioEncoder2Configuration/SampleRate", request.Configuration.SampleRate);

            ExecuteVoidCommand(validation, Media2SVCServiceTest.SetAudioEncoderConfigurationTest);

            return result;
        }

        public SetVideoSourceConfigurationResponse SetMetadataConfiguration(SetMetadataConfigurationRequest request)
        {
            SetVideoSourceConfigurationResponse result = new SetVideoSourceConfigurationResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "MetadataConfiguration/@token", request.Configuration.token);
            validation.Add(ParameterType.String, "MetadataConfiguration/Name", request.Configuration.Name);
            validation.Add(ParameterType.String, "MetadataConfiguration/@CompressionType", request.Configuration.CompressionType);
            validation.Add(ParameterType.String, "MetadataConfiguration/PTZStatus", request.Configuration.PTZStatus);
            if (request.Configuration.PTZStatus != null)
            {
                validation.Add(ParameterType.String, "MetadataConfiguration/PTZStatus/Status", request.Configuration.PTZStatus.Status.ToString());
                validation.Add(ParameterType.String, "MetadataConfiguration/PTZStatus/Position", request.Configuration.PTZStatus.Position.ToString());
            }
            validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/Events", request.Configuration.Events);
            if (request.Configuration.Events != null)
            {
                validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/Events/Filter", request.Configuration.Events.Filter);
                validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/Events/SubscriptionPolicy", request.Configuration.Events.SubscriptionPolicy);
            }
            validation.Add(ParameterType.String, "MetadataConfiguration/Analytics", request.Configuration.Analytics.ToString());
            validation.Add(ParameterType.String, "MetadataConfiguration/Multicast", request.Configuration.Multicast.ToString());
            if (request.Configuration.Multicast != null)
            {
                validation.Add(ParameterType.String, "MetadataConfiguration/Multicast/Address", request.Configuration.Multicast.Address.ToString());
                if (request.Configuration.Multicast.Address != null)
                {
                    validation.Add(ParameterType.String, "MetadataConfiguration/Multicast/Address/Type", request.Configuration.Multicast.Address.Type.ToString());
                    validation.Add(ParameterType.OptionalString, "MetadataConfiguration/Multicast/Address/IPv4Address", request.Configuration.Multicast.Address.IPv4Address.ToString());
                    validation.Add(ParameterType.OptionalString, "MetadataConfiguration/Multicast/Address/IPv6Address", request.Configuration.Multicast.Address.IPv6Address.ToString());
                }
                validation.Add(ParameterType.Int, "MetadataConfiguration/Multicast/Port", request.Configuration.Multicast.Port);
                validation.Add(ParameterType.Int, "MetadataConfiguration/Multicast/TTL", request.Configuration.Multicast.TTL);
                validation.Add(ParameterType.String, "MetadataConfiguration/Multicast/AutoStart", request.Configuration.Multicast.AutoStart.ToString());
            }
            validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/AnalyticsEngineConfiguration", request.Configuration.AnalyticsEngineConfiguration);
            if (request.Configuration.AnalyticsEngineConfiguration != null)
            {
                validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule", request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule);
                if (request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule != null)
                {
                    validation.Add(ParameterType.String, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Name", request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Name);
                    validation.Add(ParameterType.String, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Type", request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Type.ToString());
                    validation.Add(ParameterType.String, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters", request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.ToString());
                    if (request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters !=null)
                    {
                        validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters/SimpleItem", request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.SimpleItem);
                        if (request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.SimpleItem !=null)
                        {
                            validation.Add(ParameterType.String, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters/SimpleItem/Name", request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.SimpleItem[0].Name);
                            validation.Add(ParameterType.String, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters/SimpleItem/Value", request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.SimpleItem[0].Value.ToString());
                        }
                        validation.Add(ParameterType.OptionalElement, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters/ElementItem",request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.ElementItem);
                        if (request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.ElementItem !=null)
                        {
                            validation.Add(ParameterType.String, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters/ElementItem/Name", request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.ElementItem[0].Name);
                        }
                        validation.Add(ParameterType.OptionalString, "MetadataConfiguration/AnalyticsEngineConfiguration/AnalyticsModule/Parameters/Extension", request.Configuration.AnalyticsEngineConfiguration.AnalyticsModule[0].Parameters.Extension.ToString());
                    }
                }
                validation.Add(ParameterType.OptionalString, "MetadataConfiguration/AnalyticsEngineConfiguration/Extension", request.Configuration.AnalyticsEngineConfiguration.Extension.ToString());
            } 
            validation.Add(ParameterType.OptionalString, "MetadataConfiguration/Extension", request.Configuration.Extension.ToString());

            ExecuteVoidCommand(validation, Media2SVCServiceTest.SetMetadataConfigurationTest);

            return result;
        }

        public SetVideoSourceConfigurationResponse SetAudioOutputConfiguration(SetAudioOutputConfigurationRequest request)
        {
            SetVideoSourceConfigurationResponse result = new SetVideoSourceConfigurationResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "AudioOutputConfiguration/@token", request.Configuration.token);
            validation.Add(ParameterType.String, "AudioOutputConfiguration/Name", request.Configuration.Name);
            //validation.Add(ParameterType.Int, "AudioOutputConfiguration/UseCount", request.Configuration.UseCount);
            validation.Add(ParameterType.String, "AudioOutputConfiguration/OutputToken", request.Configuration.OutputToken);
            validation.Add(ParameterType.OptionalElement, "AudioOutputConfiguration/SendPrimacy", request.Configuration.SendPrimacy);
            if (request.Configuration.SendPrimacy != null)
            {
                //The following modes for the Send-Primacy are defined:
                //www.onvif.org/ver20/HalfDuplex/Server
                //www.onvif.org/ver20/HalfDuplex/Client
                //www.onvif.org/ver20/HalfDuplex/Auto
            }
            validation.Add(ParameterType.Int, "AudioOutputConfiguration/OutputLevel", request.Configuration.OutputLevel);
            ExecuteVoidCommand(validation, Media2SVCServiceTest.SetAudioOutputConfigurationTest);

            return result;
        }

        public SetVideoSourceConfigurationResponse SetAudioDecoderConfiguration(SetAudioDecoderConfigurationRequest request)
        {
            SetVideoSourceConfigurationResponse result = new SetVideoSourceConfigurationResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "AudioDecoderConfiguration/@token", request.Configuration.token);
            validation.Add(ParameterType.String, "AudioDecoderConfiguration/Name", request.Configuration.Name);
            ExecuteVoidCommand(validation, Media2SVCServiceTest.SetAudioDecoderConfigurationTest);

            return result;
        }

        public GetVideoSourceConfigurationOptionsResponse GetVideoSourceConfigurationOptions(GetVideoSourceConfigurationOptionsRequest request)
        {
            GetVideoSourceConfigurationOptionsResponse result = new GetVideoSourceConfigurationOptionsResponse();
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);

            result.Options = (VideoSourceConfigurationOptions)ExecuteGetCommand(validation, Media2SVCServiceTest.GetVideoSourceConfigurationOptionsTest);
            return result;
        }

        public GetVideoEncoderConfigurationOptionsResponse GetVideoEncoderConfigurationOptions(GetVideoEncoderConfigurationOptionsRequest request)
        {
            GetVideoEncoderConfigurationOptionsResponse result = new GetVideoEncoderConfigurationOptionsResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);
  
            result.Options = (VideoEncoder2ConfigurationOptions[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetVideoEncoderConfigurationOptionsTest);
            
            return result;
        }

        public GetAudioSourceConfigurationOptionsResponse GetAudioSourceConfigurationOptions(GetAudioSourceConfigurationOptionsRequest request)
        {
            GetAudioSourceConfigurationOptionsResponse result = new GetAudioSourceConfigurationOptionsResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);

            result.Options = (AudioSourceConfigurationOptions)ExecuteGetCommand(validation, Media2SVCServiceTest.GetAudioSourceConfigurationOptionsTest);

            return result;
        }

        public GetAudioEncoderConfigurationOptionsResponse GetAudioEncoderConfigurationOptions(GetAudioEncoderConfigurationOptionsRequest request)
        {
            GetAudioEncoderConfigurationOptionsResponse result = new GetAudioEncoderConfigurationOptionsResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);

            result.Options = (AudioEncoder2ConfigurationOptions[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetAudioEncoderConfigurationOptionsTest);
            
            return result;
        }

        public GetMetadataConfigurationOptionsResponse GetMetadataConfigurationOptions(GetMetadataConfigurationOptionsRequest request)
        {
            GetMetadataConfigurationOptionsResponse result = new GetMetadataConfigurationOptionsResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);

            result.Options = (MetadataConfigurationOptions)ExecuteGetCommand(validation, Media2SVCServiceTest.GetMetadataConfigurationOptionsTest);

            return result;
        }

        public GetAudioOutputConfigurationOptionsResponse GetAudioOutputConfigurationOptions(GetAudioOutputConfigurationOptionsRequest request)
        {
            GetAudioOutputConfigurationOptionsResponse result = new GetAudioOutputConfigurationOptionsResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);

            result.Options = (AudioOutputConfigurationOptions)ExecuteGetCommand(validation, Media2SVCServiceTest.GetAudioOutputConfigurationOptionsTest);

            return result;
        }

        public GetAudioDecoderConfigurationOptionsResponse GetAudioDecoderConfigurationOptions(GetAudioDecoderConfigurationOptionsRequest request)
        {
            GetAudioDecoderConfigurationOptionsResponse result = new GetAudioDecoderConfigurationOptionsResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);
            validation.Add(ParameterType.OptionalString, "ProfileToken", request.ProfileToken);

            result.Options = (AudioEncoder2ConfigurationOptions[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetAudioDecoderConfigurationOptionsTest);

            return result;
        }

        public GetVideoEncoderInstancesResponse GetVideoEncoderInstances(GetVideoEncoderInstancesRequest request)
        {
            GetVideoEncoderInstancesResponse result = new GetVideoEncoderInstancesResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);

            result.Info = (EncoderInstanceInfo)ExecuteGetCommand(validation, Media2SVCServiceTest.GetVideoEncoderInstancesTest);

            return result;
        }

        public GetStreamUriResponse GetStreamUri(GetStreamUriRequest request)
        {
            GetStreamUriResponse result = new GetStreamUriResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Protocol", request.Protocol); //there wasn't these validations
            validation.Add(ParameterType.String, "ProfileToken", request.ProfileToken);//
            
            result.Uri = (string)ExecuteGetCommand(validation, Media2SVCServiceTest.GetStreamUriTest);

            return result;
        }

        public StartMulticastStreamingResponse StartMulticastStreaming(StartMulticastStreamingRequest request)
        {
            StartMulticastStreamingResponse result = new StartMulticastStreamingResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ProfileToken", request.ProfileToken);

            ExecuteVoidCommand(validation, Media2SVCServiceTest.StartMulticastStreamingTest);

            return result;
        }

        public StartMulticastStreamingResponse StopMulticastStreaming(StopMulticastStreamingRequest request)
        {
            StartMulticastStreamingResponse result = new StartMulticastStreamingResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ProfileToken", request.ProfileToken);

            ExecuteVoidCommand(validation, Media2SVCServiceTest.StopMulticastStreamingTest);
            return result;
        }

        public SetSynchronizationPointResponse SetSynchronizationPoint(SetSynchronizationPointRequest request)
        {
            SetSynchronizationPointResponse result = new SetSynchronizationPointResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ProfileToken", request.ProfileToken);

            ExecuteVoidCommand(validation, Media2SVCServiceTest.SetSynchronizationPointTest);
            return result;
        }

        public GetSnapshotUriResponse GetSnapshotUri(GetSnapshotUriRequest request)
        {
            GetSnapshotUriResponse result = new GetSnapshotUriResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "ProfileToken", request.ProfileToken);

            result.Uri = (string)ExecuteGetCommand(validation, Media2SVCServiceTest.GetSnapshotUriTest);
            return result;
        }

        public GetVideoSourceModesResponse GetVideoSourceModes(GetVideoSourceModesRequest request)
        {
            GetVideoSourceModesResponse result = new GetVideoSourceModesResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "VideoSourceToken", request.VideoSourceToken);

            result.VideoSourceModes = (VideoSourceMode[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetVideoSourceModesTest);

            return result;
        }

        public SetVideoSourceModeResponse SetVideoSourceMode(SetVideoSourceModeRequest request)
        {
            SetVideoSourceModeResponse result = new SetVideoSourceModeResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSourceToken", request.VideoSourceToken);
            validation.Add(ParameterType.String, "VideoSourceModeToken", request.VideoSourceModeToken);

            result.Reboot = (bool)ExecuteGetCommand(validation, (ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
                                                       => Media2SVCServiceTest.SetVideoSourceModeTest(validationRequest, out stepType, out exc, out timeout));
            return result;
        }

        public GetOSDsResponse GetOSDs(GetOSDsRequest request)
        {
            GetOSDsResponse result = new GetOSDsResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "OSDToken", request.OSDToken);
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);

            result.OSDs = (OSDConfiguration[])ExecuteGetCommand(validation, Media2SVCServiceTest.GetOSDsTest);

            return result;
        }

        public GetOSDOptionsResponse GetOSDOptions(GetOSDOptionsRequest request)
        {
            GetOSDOptionsResponse result = new GetOSDOptionsResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.OptionalString, "ConfigurationToken", request.ConfigurationToken);

            result.OSDOptions = (OSDConfigurationOptions)ExecuteGetCommand(validation, Media2SVCServiceTest.GetOSDOptionsTest);

            return result;
        }

        public SetVideoSourceConfigurationResponse SetOSD(SetOSDRequest request)
        {
            SetVideoSourceConfigurationResponse result = new SetVideoSourceConfigurationResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "OSDConfiguration/@token", request.OSD.token);
            validation.Add(ParameterType.String, "OSDConfiguration/VideoSourceConfigurationToken", request.OSD.VideoSourceConfigurationToken.Value);
            validation.Add(ParameterType.String, "OSDConfiguration/Type", request.OSD.Type.ToString());
            validation.Add(ParameterType.String, "OSDConfiguration/Position/Type", request.OSD.Position.Type);
            validation.Add(ParameterType.OptionalElement, "OSDConfiguration/Position/Pos", request.OSD.Position.Pos);
            if (request.OSD.Position.Pos != null)
            {
                validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/Position/Pos/@x", request.OSD.Position.Pos.xSpecified);
                if (request.OSD.Position.Pos.xSpecified)
                {
                    validation.Add(ParameterType.OptionalString, "OSDConfiguration/Position/Pos/@x", request.OSD.Position.Pos.x.ToString());
                }

                validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/Position/Pos/@y", request.OSD.Position.Pos.ySpecified);
                if (request.OSD.Position.Pos.ySpecified)
                {
                    validation.Add(ParameterType.OptionalString, "OSDConfiguration/Position/Pos/@y", request.OSD.Position.Pos.y.ToString());
                }
            }
            validation.Add(ParameterType.OptionalElement, "OSDConfiguration/TextString", request.OSD.TextString);
            if (request.OSD.TextString != null)
            {
                validation.Add(ParameterType.String, "OSDConfiguration/TextString/Type", request.OSD.TextString.Type);
                validation.Add(ParameterType.OptionalString, "OSDConfiguration/TextString/DateFormat", request.OSD.TextString.DateFormat);
                validation.Add(ParameterType.OptionalString, "OSDConfiguration/TextString/TimeFormat", request.OSD.TextString.TimeFormat);
                validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/TextString/FontSize", request.OSD.TextString.FontSizeSpecified);
                if (request.OSD.TextString.FontSizeSpecified)
                {
                    validation.Add(ParameterType.Int, "OSDConfiguration/TextString/FontSize", request.OSD.TextString.FontSize);
                }
                validation.Add(ParameterType.OptionalElement, "OSDConfiguration/TextString/FontColor", request.OSD.TextString.FontColor);
                if (request.OSD.TextString.FontColor != null)
                {
                    validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/TextString/FontColor/@Transparent", request.OSD.TextString.FontColor.TransparentSpecified);
                    if (request.OSD.TextString.FontColor.TransparentSpecified)
                    {
                        validation.Add(ParameterType.Int, "OSDConfiguration/TextString/FontColor/@Transparent", request.OSD.TextString.FontColor.Transparent);
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@X", request.OSD.TextString.FontColor.Color.X.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@Y", request.OSD.TextString.FontColor.Color.Y.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@Z", request.OSD.TextString.FontColor.Color.Z.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@Colorspace", request.OSD.TextString.FontColor.Color.Colorspace);
                    }
                }
                validation.Add(ParameterType.OptionalElement, "OSDConfiguration/TextString/BackgroundColor", request.OSD.TextString.BackgroundColor);
                if (request.OSD.TextString.BackgroundColor != null)
                {
                    validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/TextString/BackgroundColor/@Transparent", request.OSD.TextString.BackgroundColor.TransparentSpecified);
                    if (request.OSD.TextString.BackgroundColor.TransparentSpecified)
                    {
                        validation.Add(ParameterType.Int, "OSDConfiguration/TextString/BackgroundColor/@Transparent", request.OSD.TextString.BackgroundColor.Transparent);
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@X", request.OSD.TextString.BackgroundColor.Color.X.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@Y", request.OSD.TextString.BackgroundColor.Color.Y.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@Z", request.OSD.TextString.BackgroundColor.Color.Z.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@Colorspace", request.OSD.TextString.BackgroundColor.Color.Colorspace);
                    }
                }
                validation.Add(ParameterType.OptionalString, "OSDConfiguration/TextString/PlainText", request.OSD.TextString.PlainText);
            }
            validation.Add(ParameterType.OptionalElement, "OSDConfiguration/Image", request.OSD.Image);
            if (request.OSD.Image != null)
            {
                validation.Add(ParameterType.String, "OSDConfiguration/Image/ImgPath", request.OSD.Image.ImgPath);
            }

            ExecuteVoidCommand(validation, Media2SVCServiceTest.SetOSDTest);

            return result;

        }

        public CreateOSDResponse CreateOSD(CreateOSDRequest request)
        {
            CreateOSDResponse result = new CreateOSDResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "OSDConfiguration/@token", request.OSD.token);
            validation.Add(ParameterType.String, "OSDConfiguration/VideoSourceConfigurationToken", request.OSD.VideoSourceConfigurationToken.Value);
            validation.Add(ParameterType.String, "OSDConfiguration/Type", request.OSD.Type.ToString());
            validation.Add(ParameterType.String, "OSDConfiguration/Position/Type", request.OSD.Position.Type);
            validation.Add(ParameterType.OptionalElement, "OSDConfiguration/Position/Pos", request.OSD.Position.Pos);
            if (request.OSD.Position.Pos != null)
            {
                validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/Position/Pos/@x", request.OSD.Position.Pos.xSpecified);
                if (request.OSD.Position.Pos.xSpecified)
                {
                    validation.Add(ParameterType.OptionalString, "OSDConfiguration/Position/Pos/@x", request.OSD.Position.Pos.x.ToString());
                }

                validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/Position/Pos/@y", request.OSD.Position.Pos.ySpecified);
                if (request.OSD.Position.Pos.ySpecified)
                {
                    validation.Add(ParameterType.OptionalString, "OSDConfiguration/Position/Pos/@y", request.OSD.Position.Pos.y.ToString());
                }
            }
            validation.Add(ParameterType.OptionalElement, "OSDConfiguration/TextString", request.OSD.TextString);
            if (request.OSD.TextString != null)
            {
                validation.Add(ParameterType.String, "OSDConfiguration/TextString/Type", request.OSD.TextString.Type);
                validation.Add(ParameterType.OptionalString, "OSDConfiguration/TextString/DateFormat", request.OSD.TextString.DateFormat);
                validation.Add(ParameterType.OptionalString, "OSDConfiguration/TextString/TimeFormat", request.OSD.TextString.TimeFormat);
                validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/TextString/FontSize", request.OSD.TextString.FontSizeSpecified);
                if (request.OSD.TextString.FontSizeSpecified)
                {
                    validation.Add(ParameterType.Int, "OSDConfiguration/TextString/FontSize", request.OSD.TextString.FontSize);
                }
                validation.Add(ParameterType.OptionalElement, "OSDConfiguration/TextString/FontColor", request.OSD.TextString.FontColor);
                if (request.OSD.TextString.FontColor != null)
                {
                    validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/TextString/FontColor/@Transparent", request.OSD.TextString.FontColor.TransparentSpecified);
                    if (request.OSD.TextString.FontColor.TransparentSpecified)
                    {
                        validation.Add(ParameterType.Int, "OSDConfiguration/TextString/FontColor/@Transparent", request.OSD.TextString.FontColor.Transparent);
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@X", request.OSD.TextString.FontColor.Color.X.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@Y", request.OSD.TextString.FontColor.Color.Y.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@Z", request.OSD.TextString.FontColor.Color.Z.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/FontColor/Color/@Colorspace", request.OSD.TextString.FontColor.Color.Colorspace);
                    }
                }
                validation.Add(ParameterType.OptionalElement, "OSDConfiguration/TextString/BackgroundColor", request.OSD.TextString.BackgroundColor);
                if (request.OSD.TextString.BackgroundColor != null)
                {
                    validation.Add(ParameterType.OptionalElementBoolFlag, "OSDConfiguration/TextString/BackgroundColor/@Transparent", request.OSD.TextString.BackgroundColor.TransparentSpecified);
                    if (request.OSD.TextString.BackgroundColor.TransparentSpecified)
                    {
                        validation.Add(ParameterType.Int, "OSDConfiguration/TextString/BackgroundColor/@Transparent", request.OSD.TextString.BackgroundColor.Transparent);
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@X", request.OSD.TextString.BackgroundColor.Color.X.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@Y", request.OSD.TextString.BackgroundColor.Color.Y.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@Z", request.OSD.TextString.BackgroundColor.Color.Z.ToString());
                        validation.Add(ParameterType.String, "OSDConfiguration/TextString/BackgroundColor/Color/@Colorspace", request.OSD.TextString.BackgroundColor.Color.Colorspace);
                    }
                }
                validation.Add(ParameterType.OptionalString, "OSDConfiguration/TextString/PlainText", request.OSD.TextString.PlainText);
            }
            validation.Add(ParameterType.OptionalElement, "OSDConfiguration/Image", request.OSD.Image);
            if (request.OSD.Image != null)
            {
                validation.Add(ParameterType.String, "OSDConfiguration/Image/ImgPath", request.OSD.Image.ImgPath);
            }
            result.OSDToken = (string)ExecuteGetCommand(validation, Media2SVCServiceTest.CreateOSDTest);

            return result;
        }

        public SetVideoSourceConfigurationResponse DeleteOSD(DeleteOSDRequest request)
        {
            SetVideoSourceConfigurationResponse result = new SetVideoSourceConfigurationResponse();

            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "OSDToken", request.OSDToken);

            ExecuteVoidCommand(validation, Media2SVCServiceTest.DeleteOSDTest);

            return result;
        }
    }
}
