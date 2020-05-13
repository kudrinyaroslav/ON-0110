using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.CameraWebService.Common;
using System.Web.Services.Protocols;


namespace DUT.CameraWebService.Thermal10
{
    public class ThermalServiceTest : Base.BaseServiceTest 
    {
        protected override string ServiceName
        {
            get { return "ThermalService"; }
        }

        #region Const

        /// <summary>
        /// Constant for Command index
        /// </summary>

        private const int GetServiceCapabilities = 0;
        private const int GetConfigurationOptions = 1;
        private const int GetConfiguration = 2;
        private const int GetConfigurations = 3;
        private const int SetConfiguration = 4;
        private const int GetRadiometryConfigurationOptions = 5;
        private const int GetRadiometryConfiguration = 6;
        private const int SetRadiometryConfiguration = 7;

        private const int MaxCommands = 8;

        #endregion //Const

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ThermalServiceTest(TestCommon testCommon)
            : base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors

        internal Capabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<Capabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal ConfigurationOptions GetConfigurationOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<ConfigurationOptions>("GetConfigurationOptions", GetConfigurationOptions, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal Configuration GetConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<Configuration>("GetConfiguration", GetConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal Configurations[] GetConfigurationsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<Configurations[]>("GetConfigurations", GetConfigurations, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void SetConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetConfiguration", SetConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal RadiometryConfigurationOptions GetRadiometryConfigurationOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<RadiometryConfigurationOptions>("GetRadiometryConfigurationOptions", GetRadiometryConfigurationOptions, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal RadiometryConfiguration GetRadiometryConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<RadiometryConfiguration>("GetRadiometryConfiguration", GetRadiometryConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void SetRadiometryConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetRadiometryConfiguration", SetRadiometryConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }
    }
}