using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DUT.CameraWebService.Common;
using System.Xml.Serialization;
using System.IO;

namespace DUT.CameraWebService.Thermal10
{
    /// <summary>
    /// Summary description for ThermalService
    /// </summary>

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.18020")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/thermal/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ThermalBinding", Namespace = "http://www.onvif.org/ver10/thermal/wsdl")]
    public class ThermalService : ThermalSeviceBinding
    {
        //TestSuit
        ThermalServiceTest ThermalServiceTest
        {
            get
            {
                if (Application[Base.AppVars.THERMALSERVICE] != null)
                {
                    return (ThermalServiceTest)Application[Base.AppVars.THERMALSERVICE];
                }
                else
                {
                    ThermalServiceTest serviceTest = new ThermalServiceTest(TestCommon);
                    Application[Base.AppVars.THERMALSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/thermal/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/thermal/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/thermal/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Media2CapabilitiesNamepaceNotDeclarated)]
        public override Capabilities GetServiceCapabilities()
        {
            ParametersValidation validation = new ParametersValidation();
            Capabilities result = (Capabilities)ExecuteGetCommand(validation, ThermalServiceTest.GetServiceCapabilitiesTest);
            return result;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/thermal/wsdl/GetConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/thermal/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/thermal/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ConfigurationOptions")]
        public override ConfigurationOptions GetConfigurationOptions(string VideoSourceToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSourceToken", VideoSourceToken);
            ConfigurationOptions result = (ConfigurationOptions)ExecuteGetCommand(validation, ThermalServiceTest.GetConfigurationOptionsTest);
            return result;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/thermal/wsdl/GetConfiguration", RequestNamespace = "http://www.onvif.org/ver10/thermal/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/thermal/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override Configuration GetConfiguration(string VideoSourceToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSourceToken", VideoSourceToken);
            Configuration result = (Configuration)ExecuteGetCommand(validation, ThermalServiceTest.GetConfigurationTest);
            return result;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/thermal/wsdl/GetConfigurations", RequestNamespace = "http://www.onvif.org/ver10/thermal/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/thermal/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configurations")]
        public override Configurations[] GetConfigurations()
        {
            #region Serialization Temp

            Configurations[] dsr = new Configurations[1];
            //dsr[0] = new Configurations();
            //dsr[0].token = "test";
            //dsr[0].Configuration = new Configuration();
            //dsr[0].Configuration.ColorPalette = new ColorPalette();
            //dsr[0].Configuration.Cooler = new Cooler();
            //XmlSerializer serializer1 = new XmlSerializer(typeof(Configurations[]));
            //TextWriter textWriter = new StreamWriter("d:\\2.txt");
            //serializer1.Serialize(textWriter, dsr);
            #endregion //Serialization Temp

            ParametersValidation validation = new ParametersValidation();
            Configurations[] result = (Configurations[])ExecuteGetCommand(validation, ThermalServiceTest.GetConfigurationsTest);
            return result;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/thermal/wsdl/SetConfiguration", RequestNamespace = "http://www.onvif.org/ver10/thermal/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/thermal/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetConfiguration(string VideoSourceToken, Configuration Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSourceToken", VideoSourceToken);

            validation.Add(ParameterType.String, "Configuration/ColorPalette/@token", Configuration != null && Configuration.ColorPalette != null ? Configuration.ColorPalette.token : null);
            validation.Add(ParameterType.String, "Configuration/ColorPalette/@Type", Configuration != null && Configuration.ColorPalette != null ? Configuration.ColorPalette.Type : null);
            validation.Add(ParameterType.String, "Configuration/ColorPalette/Name", Configuration != null && Configuration.ColorPalette != null ? Configuration.ColorPalette.Name : null);

            validation.Add(ParameterType.String, "Configuration/Polarity", Configuration != null ? Configuration.Polarity.ToString() : null);

            if (Configuration != null && Configuration.NUCTable != null)
            {
                validation.Add(ParameterType.String, "Configuration/NUCTable/@token", Configuration.NUCTable.token);
                if (Configuration.NUCTable.LowTemperatureSpecified)
                    validation.Add(ParameterType.Float, "Configuration/NUCTable/@LowTemperature", Configuration.NUCTable.LowTemperature);
                if (Configuration.NUCTable.HighTemperatureSpecified)
                    validation.Add(ParameterType.Float, "Configuration/NUCTable/@HighTemperature", Configuration.NUCTable.HighTemperature);
                validation.Add(ParameterType.String, "Configuration/NUCTable/HighTemperature/Name", Configuration.NUCTable.Name); 
            }

            if (Configuration != null && Configuration.Cooler != null)
            {
                validation.Add(ParameterType.OptionalBool, "Configuration/Cooler/Enabled", Configuration.Cooler.Enabled);
                if(Configuration.Cooler.RunTimeSpecified)
                    validation.Add(ParameterType.OptionalFloat, "Configuration/Cooler/RunTime", Configuration.Cooler.RunTime); 
            }

            ExecuteVoidCommand(validation, ThermalServiceTest.SetConfigurationTest);
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/thermal/wsdl/GetRadiometryConfigurationOptions", RequestNamespace = "http://www.onvif.org/ver10/thermal/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/thermal/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ConfigurationOptions")]
        public override RadiometryConfigurationOptions GetRadiometryConfigurationOptions(string VideoSourceToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSourceToken", VideoSourceToken);
            RadiometryConfigurationOptions result = (RadiometryConfigurationOptions)ExecuteGetCommand(validation, ThermalServiceTest.GetRadiometryConfigurationOptionsTest);
            return result;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/thermal/wsdl/GetRadiometryConfiguration", RequestNamespace = "http://www.onvif.org/ver10/thermal/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/thermal/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public override RadiometryConfiguration GetRadiometryConfiguration(string VideoSourceToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSourceToken", VideoSourceToken);
            RadiometryConfiguration result = (RadiometryConfiguration)ExecuteGetCommand(validation, ThermalServiceTest.GetRadiometryConfigurationTest);
            return result;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/thermal/wsdl/SetRadiometryConfiguration", RequestNamespace = "http://www.onvif.org/ver10/thermal/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/thermal/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRadiometryConfiguration(string VideoSourceToken, RadiometryConfiguration Configuration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "VideoSourceToken", VideoSourceToken);

            if (Configuration != null && Configuration.RadiometryGlobalParameters != null)
            {
                validation.Add(ParameterType.Float, "Configuration/RadiometryGlobalParameters/ReflectedAmbientTemperature", Configuration.RadiometryGlobalParameters.ReflectedAmbientTemperature);
                validation.Add(ParameterType.Float, "Configuration/RadiometryGlobalParameters/Emissivity", Configuration.RadiometryGlobalParameters.Emissivity);
                validation.Add(ParameterType.Float, "Configuration/RadiometryGlobalParameters/DistanceToObject", Configuration.RadiometryGlobalParameters.DistanceToObject);
                if (Configuration.RadiometryGlobalParameters.RelativeHumiditySpecified)
                    validation.Add(ParameterType.Float, "Configuration/RadiometryGlobalParameters/DistanceToObject", Configuration.RadiometryGlobalParameters.RelativeHumidity);
                if (Configuration.RadiometryGlobalParameters.AtmosphericTemperatureSpecified)
                    validation.Add(ParameterType.Float, "Configuration/RadiometryGlobalParameters/AtmosphericTemperature", Configuration.RadiometryGlobalParameters.AtmosphericTemperature);
                if (Configuration.RadiometryGlobalParameters.AtmosphericTransmittanceSpecified)
                    validation.Add(ParameterType.Float, "Configuration/RadiometryGlobalParameters/AtmosphericTransmittance", Configuration.RadiometryGlobalParameters.AtmosphericTransmittance);
                if (Configuration.RadiometryGlobalParameters.ExtOpticsTemperatureSpecified)
                    validation.Add(ParameterType.Float, "Configuration/RadiometryGlobalParameters/ExtOpticsTemperature", Configuration.RadiometryGlobalParameters.ExtOpticsTemperature);
                if (Configuration.RadiometryGlobalParameters.ExtOpticsTransmittanceSpecified)
                    validation.Add(ParameterType.Float, "Configuration/RadiometryGlobalParameters/ExtOpticsTransmittance", Configuration.RadiometryGlobalParameters.ExtOpticsTransmittance);
            }

            ExecuteVoidCommand(validation, ThermalServiceTest.SetRadiometryConfigurationTest);
        }
    }
}
