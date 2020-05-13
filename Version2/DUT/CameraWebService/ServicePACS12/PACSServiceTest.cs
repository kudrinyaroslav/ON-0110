using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.IO;
using System.Xml.Serialization;
using DUT.CameraWebService;
using CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.PACS12
{

    /// <summary>
    /// Class for Search Service tests
    /// </summary>
    public class PACSServiceTest : Base.BaseServiceTest
    {

        #region Const

        protected override string ServiceName
        { 
            get 
            { 
                return "PACS12"; 
            } 
        }

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetServiceCapabilities = 0;
        private const int GetAreaInfoListByTokenList = 1;
        private const int EnableAccessPoint = 2;
        private const int DisableAccessPoint = 3;
        private const int GetAreaInfo = 4;
        private const int GetAreaInfoList = 5;
        private const int GetAccessPointInfo = 6;
        private const int GetAccessPointInfoList = 7;
        private const int GetAccessPointState = 8;
        private const int ExternalAuthorization = 9;
        private const int CreateAccessPoint = 10;
        private const int SetAccessPoint = 11;
        private const int ModifyAccessPoint = 12;
        private const int DeleteAccessPoint = 13;        
        private const int MaxCommands = 14;
       
        #endregion //Const
     
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public PACSServiceTest(TestCommon testCommon)
            :base(testCommon)
        {                        
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors

        //***************************************************************************************

     
        #region General

        internal ServiceCapabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<ServiceCapabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //General
                
        //***************************************************************************************

        #region Area
        
        internal string GetAreaInfoListTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("GetAreaInfoList", GetAreaInfoList, validationRequest, out stepType, out ex, out Timeout);
        }

        internal AreaInfo[] TakeAreaInfoList()
        {
            return TakeSpecialParameter<AreaInfo[]>("GetAreaInfoList", GetAreaInfoList, "ArrayOfAreaInfo");
        }

        internal AreaInfo[] GetAreaInfoTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<AreaInfo[]>("GetAreaInfo", GetAreaInfo, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //Area

        //***************************************************************************************

        #region Access Point

        internal string GetAccessPointInfoListTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("GetAccessPointInfoList", GetAccessPointInfoList, validationRequest, out stepType, out ex, out Timeout);
        }

        internal AccessPointInfo[] TakeAccesssPointInfoList()
        {
            return TakeSpecialParameter<AccessPointInfo[]>("GetAccessPointInfoList", GetAccessPointInfoList, "ArrayOfAccessPointInfo");
        }

        internal AccessPointInfo[] GetAccessPointInfoTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<AccessPointInfo[]>("GetAccessPointInfo", GetAccessPointInfo, validationRequest, out stepType, out ex, out Timeout);
        }
                
        internal void EnableAccessPointTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("EnableAccessPoint", EnableAccessPoint, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void DisableAccessPointTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DisableAccessPoint", DisableAccessPoint, validationRequest, out stepType, out ex, out Timeout);
        }

        internal AccessPointState GetAccessPointStateTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<AccessPointState>("GetAccessPointState", GetAccessPointState, validationRequest, out stepType, out ex, out Timeout);
        }

        internal string CreateAccessPointTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {           
            return GetCommand<string>("CreateAccessPoint", CreateAccessPoint, validationRequest, true, out stepType, out exc, out timeout);

        }

        internal void SetAccessPointTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("SetAccessPoint", SetAccessPoint, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void ModifyAccessPointTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("ModifyAccessPoint", ModifyAccessPoint, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void DeleteAccessPointTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DeleteAccessPoint", DeleteAccessPoint, validationRequest, out stepType, out ex, out Timeout);
        }
        #endregion //Access Point

        //***************************************************************************************


        internal void ExternalAuthorizationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("ExternalAuthorization", ExternalAuthorization, validationRequest, true, out stepType, out ex, out Timeout);
        }

        

    }
}
