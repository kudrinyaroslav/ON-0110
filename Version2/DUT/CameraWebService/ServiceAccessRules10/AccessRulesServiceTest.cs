using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.IO;
using System.Xml.Serialization;
using DUT.CameraWebService;
using CameraWebService;
using DUT.CameraWebService.Common;


namespace DUT.CameraWebService.ServiceAccessRules10
{

    /// <summary>
    /// Class for Access Rules Service tests
    /// </summary>
    public class AccessRulesServiceTest : Base.BaseServiceTest
    {

        #region Const

        protected override string ServiceName
        {
            get
            {
                //Used in DUT to define service name for command
                return "AccessRules10";
            }
        }

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetServiceCapabilities = 0;
        private const int GetAccessProfileInfo = 1;
        private const int GetAccessProfileInfoList = 2;
        private const int GetAccessProfiles = 3;
        private const int GetAccessProfileList = 4;
        private const int CreateAccessProfile = 5;
        private const int ModifyAccessProfile = 6;
        private const int DeleteAccessProfile = 7;
        private const int MaxCommands = 8;

        #endregion //Const

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public AccessRulesServiceTest(TestCommon testCommon)
            : base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors

        //***************************************************************************************

        #region General

        internal ServiceCapabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            return GetCommand<ServiceCapabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, out stepType, out ex, out timeout);
        }

        #endregion //General

        #region AccessProfileInfo

        internal AccessProfileInfo[] GetAccessProfileInfoTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            return GetCommand<AccessProfileInfo[]>("GetAccessProfileInfo", GetAccessProfileInfo, validationRequest, out stepType, out ex, out timeout);
        }

        internal AccessProfileInfo[] TakeAccessProfileInfoList()
        {
            return TakeSpecialParameter<AccessProfileInfo[]>("GetAccessProfileInfoList", GetAccessProfileInfoList, "ArrayOfAccessProfileInfo");
        }

        internal string GetAccessProfileInfoListTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("GetAccessProfileInfoList", GetAccessProfileInfoList, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //AccessProfileInfo

        #region AccessProfile

        internal AccessProfile[] GetAccessProfilesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            return GetCommand<AccessProfile[]>("GetAccessProfiles", GetAccessProfiles, validationRequest, out stepType, out ex, out timeout);
        }

        internal AccessProfile[] TakeAccessProfileList()
        {
            return TakeSpecialParameter<AccessProfile[]>("GetAccessProfileList", GetAccessProfileList, "ArrayOfAccessProfile");
        }

        internal string GetAccessProfileListTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("GetAccessProfileList", GetAccessProfileList, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //AccessProfile

        #region AccessProfile Create/Modify/Delete

        internal void DeleteAccessProfileTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DeleteAccessProfile", DeleteAccessProfile, validationRequest, out stepType, out ex, out Timeout);
        }

        internal string CreateAccessProfileTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("CreateAccessProfile", CreateAccessProfile, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void ModifyAccessProfileTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("ModifyAccessProfile", ModifyAccessProfile, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //AccessProfile Create/Modify/Delete



    }
}
