using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.CameraWebService.Common;
using System.Web.Services.Protocols;
using System.Xml;


namespace DUT.CameraWebService.ServiceAnalytics20
{
    public class AnalyticsEngineServiceTest : Base.BaseServiceTest
    {
        #region Const


        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetServiceCapabilities = 0;
        private const int GetRuleOptions = 1;
        private const int GetSupportedRules = 2;
        private const int CreateRules = 3;
        private const int DeleteRules = 4;
        private const int GetRules = 5;
        private const int ModifyRules = 6;
        private const int GetSupportedAnalyticsModules = 7;
        private const int GetAnalyticsModuleOptions = 8;
        private const int MaxCommands = 9;

        #endregion //Const

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public AnalyticsEngineServiceTest(TestCommon testCommon)
            : base(testCommon)
        {
            InitCommandsCount(MaxCommands);
    }

    #endregion //Constructors

    protected override string ServiceName
        {
            get
            {
                return "Analytics20";
            }
        }
        internal Capabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<Capabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal ConfigOptions[] GetRuleOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<ConfigOptions[]>("GetRuleOptions", GetRuleOptions, validationRequest, true, out stepType, out exc, out timeout);
        }


        internal ConfigOptions[] GetAnalyticsModuleOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<ConfigOptions[]>("GetAnalyticsModuleOptions", GetAnalyticsModuleOptions, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal SupportedRules GetSupportedRulesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<SupportedRules>("GetSupportedRules", GetSupportedRules, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void CreateRulesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("CreateRules", CreateRules, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void DeleteRulesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeleteRules", DeleteRules, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal Config[] GetRulesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<Config[]>("GetRules", GetRules, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void ModifyRulesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("ModifyRules", ModifyRules, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal SupportedAnalyticsModules GetSupportedAnalyticsModulesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<SupportedAnalyticsModules>("GetSupportedAnalyticsModules", GetSupportedAnalyticsModules, validationRequest, true, out stepType, out exc, out timeout);
        }
    }


}