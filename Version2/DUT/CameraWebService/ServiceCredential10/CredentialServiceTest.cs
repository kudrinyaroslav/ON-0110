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
using System.Globalization;

namespace DUT.CameraWebService.ServiceCredential10
{

    /// <summary>
    /// Class for Credential Service tests
    /// </summary>
    public class CredentialServiceTest : Base.BaseServiceTest
    {

        #region Members

        string m_setcredential_ID;
        DateTime? m_setcredential_AP_ValidFrom;
        DateTime? m_setcredential_AP_ValidTo;
        byte[] m_setcredential_CI_Value;
        DateTime? m_setcredential_ValidFrom;
        DateTime? m_setcredential_ValidTo;

        #endregion

        #region Const

        protected override string ServiceName
        {
            get
            {
                //Used in DUT to define service name for command
                return "Credential10";
            }
        }

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetServiceCapabilities = 0;
        private const int GetCredentialInfo = 1;
        private const int GetCredentialInfoList = 2;
        private const int GetCredentials = 3;
        private const int GetCredentialList = 4;
        private const int CreateCredential = 5;
        private const int ModifyCredential = 6;
        private const int DeleteCredential = 7;
        private const int GetCredentialState = 8;
        private const int EnableCredential = 9;
        private const int DisableCredential = 10;
        private const int ResetAntipassbackViolation = 11;
        private const int GetCredentialIdentifiers = 12;
        private const int SetCredentialIdentifier = 13;
        private const int DeleteCredentialIdentifier = 14;
        private const int GetCredentialAccessProfiles = 15;
        private const int SetCredentialAccessProfiles = 16;
        private const int DeleteCredentialAccessProfiles = 17;
        private const int GetSupportedFormatTypes = 18;
        private const int MaxCommands = 19;

        #endregion //Const

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public CredentialServiceTest(TestCommon testCommon)
            : base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors

        //***************************************************************************************
        #region Helpers
        void CredentialFromCreateModifyRemember(ParametersValidation validationRequest, string credentialToken = "") // for CreateSchedule set m_schedule_token = scheduleToken (ValidationRules parameterName string is empty for this case)
        {
            if (validationRequest.ValidationRules.Any(rule => rule.ParameterName == "Credential.ValidFrom"))
            {
                m_setcredential_ValidFrom = (DateTime)validationRequest.ValidationRules.First(rule => rule.ParameterName == "Credential.ValidFrom").Value;
            }
            else
            {
                m_setcredential_ValidFrom = null;
            }

            if (validationRequest.ValidationRules.Any(rule => rule.ParameterName == "Credential.ValidTo"))
            {
                m_setcredential_ValidTo = (DateTime)validationRequest.ValidationRules.First(rule => rule.ParameterName == "Credential.ValidTo").Value;
            }
            else
            {
                m_setcredential_ValidTo = null;
            }

            if (validationRequest.ValidationRules.Any(rule => rule.ParameterName == "Credential.CredentialAccessProfile0.ValidFrom"))
            {
                m_setcredential_AP_ValidFrom = (DateTime)validationRequest.ValidationRules.First(rule => rule.ParameterName == "Credential.CredentialAccessProfile0.ValidFrom").Value;
            }
            else
            {
                m_setcredential_AP_ValidFrom = null;
            }

            if (validationRequest.ValidationRules.Any(rule => rule.ParameterName == "Credential.CredentialAccessProfile0.ValidTo"))
            {
                m_setcredential_AP_ValidTo = (DateTime)validationRequest.ValidationRules.First(rule => rule.ParameterName == "Credential.CredentialAccessProfile0.ValidTo").Value;
            }
            else
            {
                m_setcredential_AP_ValidTo = null;
            }

            if (validationRequest.ValidationRules.Any(rule => rule.ParameterName == "Credential.CredentialIdentifier0.Value"))
            {
                m_setcredential_CI_Value = (byte[])validationRequest.ValidationRules.First(rule => rule.ParameterName == "Credential.CredentialIdentifier0.Value").Value;
            }
            else
            {
                m_setcredential_CI_Value = null;
            }

            m_setcredential_ID = string.IsNullOrEmpty(credentialToken) ? (string)validationRequest.ValidationRules.First(rule => rule.ParameterName == "Credential.token").Value : credentialToken;

        }
        void CredentialFromCreateModifyUpdate(int special, ref Credential[] res)
        {
            if (special != 0)
            {
                var credential = res.First(rule => rule.token == m_setcredential_ID);
                #region TimeValueValidity supported
                if (special == 1) //Cr and AcPr validity and CI Value
                {
                    if (m_setcredential_ValidFrom != null)
                    {
                        credential.ValidFrom = (DateTime)m_setcredential_ValidFrom;
                    }
                    if (m_setcredential_ValidTo != null)
                    {
                        credential.ValidTo = (DateTime)m_setcredential_ValidTo;
                    }
                    if (m_setcredential_AP_ValidFrom != null)
                    {
                        credential.CredentialAccessProfile[0].ValidFrom = (DateTime)m_setcredential_AP_ValidFrom;
                    }
                    if (m_setcredential_AP_ValidTo != null)
                    {
                        credential.CredentialAccessProfile[0].ValidTo = (DateTime)m_setcredential_AP_ValidTo;
                    }
                    if (m_setcredential_CI_Value != null)
                    {
                        credential.CredentialIdentifier[0].Value = m_setcredential_CI_Value;
                    }
                }
                if (special == 2) //Only Cr validity
                {
                    if (m_setcredential_ValidFrom != null)
                    {
                        credential.ValidFrom = (DateTime)m_setcredential_ValidFrom;
                    }
                    if (m_setcredential_ValidTo != null)
                    {
                        credential.ValidTo = (DateTime)m_setcredential_ValidTo;
                    }
                }
                
                if (special == 4) //Only Cr ValidFrom
                {

                    if (m_setcredential_ValidFrom != null)
                    {
                        credential.ValidFrom = (DateTime)m_setcredential_ValidFrom;
                    }
                }
                if (special == 5) //all Cr and AcPr only ValidFrom
                {
                    if (m_setcredential_ValidFrom != null)
                    {
                        credential.ValidFrom = (DateTime)m_setcredential_ValidFrom;
                    }
                    if (m_setcredential_ValidTo != null)
                    {
                        credential.ValidTo = (DateTime)m_setcredential_ValidTo;
                    }
                    if (m_setcredential_AP_ValidFrom != null)
                    {
                        credential.CredentialAccessProfile[0].ValidFrom = (DateTime)m_setcredential_AP_ValidFrom;
                    }
                }
                if (special == 6) //add ms to valid values
                {
                    if (m_setcredential_ValidFrom != null)
                    {
                        credential.ValidFrom = (DateTime)m_setcredential_ValidFrom.Value.AddMilliseconds(001);
                    }
                    if (m_setcredential_ValidTo != null)
                    {
                        credential.ValidTo = (DateTime)m_setcredential_ValidTo.Value.AddMilliseconds(001);
                    }
                    if (m_setcredential_AP_ValidFrom != null)
                    {
                        //credential.CredentialAccessProfile[0].ValidFrom = (DateTime)m_setcredential_AP_ValidFrom.Value.AddMilliseconds(001);
                        credential.CredentialAccessProfile[0].ValidFrom = (DateTime)m_setcredential_AP_ValidFrom;
                    }
                    if (m_setcredential_AP_ValidTo != null)
                    {
                        credential.CredentialAccessProfile[0].ValidTo = (DateTime)m_setcredential_AP_ValidTo.Value.AddMilliseconds(001);
                    }
                    if (m_setcredential_CI_Value != null)
                    {
                        credential.CredentialIdentifier[0].Value = m_setcredential_CI_Value;
                    }
                }
                 if (special == 7) //add sec to valid values
                {
                    if (m_setcredential_ValidFrom != null)
                    {
                        credential.ValidFrom = (DateTime)m_setcredential_ValidFrom.Value.AddSeconds(1);
                    }
                    if (m_setcredential_ValidTo != null)
                    {
                        credential.ValidTo = (DateTime)m_setcredential_ValidTo.Value.AddSeconds(1);
                    }
                    if (m_setcredential_AP_ValidFrom != null)
                    {
                        credential.CredentialAccessProfile[0].ValidFrom = (DateTime)m_setcredential_AP_ValidFrom.Value.AddSeconds(1);
                    }
                    if (m_setcredential_AP_ValidTo != null)
                    {
                        credential.CredentialAccessProfile[0].ValidTo = (DateTime)m_setcredential_AP_ValidTo.Value.AddSeconds(1);
                    }
                    if (m_setcredential_CI_Value != null)
                    {
                        credential.CredentialIdentifier[0].Value = m_setcredential_CI_Value;
                    }
                }

                 if (special == 8) //timezone
                 {
                     if (m_setcredential_ValidFrom != null)
                     {
                         credential.ValidFrom = (DateTime)m_setcredential_ValidFrom;                         
                        
                         credential.ValidFrom = DateTime.SpecifyKind(credential.ValidFrom.ToUniversalTime(), DateTimeKind.Utc);                      
                                                  
                     }
                     if (m_setcredential_ValidTo != null)
                     {
                         credential.ValidTo = (DateTime)m_setcredential_ValidTo;
                         
                         credential.ValidTo = DateTime.SpecifyKind(credential.ValidTo.ToUniversalTime(), DateTimeKind.Utc);
                     }
                     if (m_setcredential_AP_ValidFrom != null)
                     {
                         credential.CredentialAccessProfile[0].ValidFrom = (DateTime)m_setcredential_AP_ValidFrom;
                         
                         credential.CredentialAccessProfile[0].ValidFrom = DateTime.SpecifyKind(credential.CredentialAccessProfile[0].ValidFrom.ToUniversalTime(), DateTimeKind.Utc);
                     }
                     if (m_setcredential_AP_ValidTo != null)
                     {
                         credential.CredentialAccessProfile[0].ValidTo = (DateTime)m_setcredential_AP_ValidTo;
                         
                         credential.CredentialAccessProfile[0].ValidTo = DateTime.SpecifyKind(credential.CredentialAccessProfile[0].ValidTo.ToUniversalTime(), DateTimeKind.Utc);
                     }
                     if (m_setcredential_CI_Value != null)
                     {
                         credential.CredentialIdentifier[0].Value = m_setcredential_CI_Value;
                     }
                 }

                 if (special == 9) //timezone, incorrect credential.ValidFrom value
                 {
                     if (m_setcredential_ValidFrom != null)
                     {
                         credential.ValidFrom = (DateTime)m_setcredential_ValidFrom;

                         credential.ValidFrom = DateTime.SpecifyKind(credential.ValidFrom.ToUniversalTime().AddMinutes(1), DateTimeKind.Utc);

                     }
                     if (m_setcredential_ValidTo != null)
                     {
                         credential.ValidTo = (DateTime)m_setcredential_ValidTo;

                         credential.ValidTo = DateTime.SpecifyKind(credential.ValidTo.ToUniversalTime(), DateTimeKind.Utc);
                     }
                     if (m_setcredential_AP_ValidFrom != null)
                     {
                         credential.CredentialAccessProfile[0].ValidFrom = (DateTime)m_setcredential_AP_ValidFrom;

                         credential.CredentialAccessProfile[0].ValidFrom = DateTime.SpecifyKind(credential.CredentialAccessProfile[0].ValidFrom.ToUniversalTime(), DateTimeKind.Utc);
                     }
                     if (m_setcredential_AP_ValidTo != null)
                     {
                         credential.CredentialAccessProfile[0].ValidTo = (DateTime)m_setcredential_AP_ValidTo;

                         credential.CredentialAccessProfile[0].ValidTo = DateTime.SpecifyKind(credential.CredentialAccessProfile[0].ValidTo.ToUniversalTime(), DateTimeKind.Utc);
                     }
                     if (m_setcredential_CI_Value != null)
                     {
                         credential.CredentialIdentifier[0].Value = m_setcredential_CI_Value;
                     }
                 }

                #endregion
                #region TimeValueValidity not supported
                if (special == 10) //Cr and AcPr validity
                {
                    if (m_setcredential_ValidFrom != null)
                    {
                        credential.ValidFrom = (DateTime)m_setcredential_ValidFrom;
                        credential.ValidFrom = credential.ValidFrom.AddMilliseconds(1.0);
                    }
                    if (m_setcredential_ValidTo != null)
                    {
                        credential.ValidTo = (DateTime)m_setcredential_ValidTo;
                        credential.ValidTo = credential.ValidTo.AddMilliseconds(1.0);
                    }
                    if (m_setcredential_AP_ValidFrom != null)
                    {
                        credential.CredentialAccessProfile[0].ValidFrom = (DateTime)m_setcredential_AP_ValidFrom;
                        credential.CredentialAccessProfile[0].ValidFrom = credential.CredentialAccessProfile[0].ValidFrom.AddMilliseconds(1.0);
                    }
                    if (m_setcredential_AP_ValidTo != null)
                    {
                        credential.CredentialAccessProfile[0].ValidTo = (DateTime)m_setcredential_AP_ValidTo;
                        credential.CredentialAccessProfile[0].ValidTo = credential.CredentialAccessProfile[0].ValidTo.AddMilliseconds(1.0);
                    }
                }
                if (special == 20) //Only Cr validity
                {
                    if (m_setcredential_ValidFrom != null)
                    {
                        credential.ValidFrom = (DateTime)m_setcredential_ValidFrom;
                        credential.ValidFrom = credential.ValidFrom.AddMilliseconds(1.0);
                    }
                    if (m_setcredential_ValidTo != null)
                    {
                        credential.ValidTo = (DateTime)m_setcredential_ValidTo;
                        credential.ValidTo = credential.ValidTo.AddMilliseconds(1.0);
                    }
                }

                if (special == 40) //Only Cr ValidFrom
                {

                    if (m_setcredential_ValidFrom != null)
                    {
                        credential.ValidFrom = (DateTime)m_setcredential_ValidFrom;
                        credential.ValidFrom = credential.ValidFrom.AddMilliseconds(1.0);
                    }
                }
                if (special == 50) //all Cr and AcPr only ValidFrom
                {
                    if (m_setcredential_ValidFrom != null)
                    {
                        credential.ValidFrom = (DateTime)m_setcredential_ValidFrom;
                        credential.ValidFrom = credential.ValidFrom.AddMilliseconds(1.0);
                    }
                    if (m_setcredential_ValidTo != null)
                    {
                        credential.ValidTo = (DateTime)m_setcredential_ValidTo;
                        credential.ValidTo = credential.ValidTo.AddMilliseconds(1.0);
                    }
                    if (m_setcredential_AP_ValidFrom != null)
                    {
                        credential.CredentialAccessProfile[0].ValidFrom = (DateTime)m_setcredential_AP_ValidFrom;
                        credential.CredentialAccessProfile[0].ValidFrom = credential.CredentialAccessProfile[0].ValidFrom.AddMilliseconds(1.0);
                    }
                }
                #endregion
            }
        }
        

        void CredentialInfoFromCreateModifyUpdate(int special, ref CredentialInfo[] res)
        {
            if (special != 0)
            {
                var credentialInfo = res.First(rule => rule.token == m_setcredential_ID);

                #region TimeValueValidity supported
                if (special == 1)
                {
                    if (m_setcredential_ValidFrom != null)
                    {
                        credentialInfo.ValidFrom = (DateTime)m_setcredential_ValidFrom;
                    }
                    if (m_setcredential_ValidTo != null)
                    {
                        credentialInfo.ValidTo = (DateTime)m_setcredential_ValidTo;
                    }
                }
                if (special == 2) //Cr.ValidTo fails
                {
                    if (m_setcredential_ValidFrom != null)
                    {
                        credentialInfo.ValidFrom = (DateTime)m_setcredential_ValidFrom;
                    }
                }

                if (special == 6) //add ms to valid values
                {
                    if (m_setcredential_ValidFrom != null)
                    {
                        credentialInfo.ValidFrom = (DateTime)m_setcredential_ValidFrom.Value.AddMilliseconds(1);
                    }
                    if (m_setcredential_ValidTo != null)
                    {
                        credentialInfo.ValidTo = (DateTime)m_setcredential_ValidTo.Value.AddMilliseconds(1);
                    }
                }
               if (special == 7) //add sec to valid values
                    {
                        if (m_setcredential_ValidFrom != null)
                        {
                            credentialInfo.ValidFrom = (DateTime)m_setcredential_ValidFrom.Value.AddSeconds(1);
                        }
                        if (m_setcredential_ValidTo != null)
                        {
                            credentialInfo.ValidTo = (DateTime)m_setcredential_ValidTo.Value.AddSeconds(1);
                        }
                    }

               if (special == 8) //timezone
               {
                   if (m_setcredential_ValidFrom != null)
                   {
                       credentialInfo.ValidFrom = (DateTime)m_setcredential_ValidFrom;

                       credentialInfo.ValidFrom = DateTime.SpecifyKind(credentialInfo.ValidFrom.ToUniversalTime(), DateTimeKind.Utc);

                   }
                   if (m_setcredential_ValidTo != null)
                   {
                       credentialInfo.ValidTo = (DateTime)m_setcredential_ValidTo;

                       credentialInfo.ValidTo = DateTime.SpecifyKind(credentialInfo.ValidTo.ToUniversalTime(), DateTimeKind.Utc);
                   }                   
               }
                #endregion
                    #region TimeValueValidity not supported
                    if (special == 10)
                    {
                        if (m_setcredential_ValidFrom != null)
                        {
                            credentialInfo.ValidFrom = (DateTime)m_setcredential_ValidFrom;
                            credentialInfo.ValidFrom = credentialInfo.ValidFrom.AddMilliseconds(1.0);
                        }
                        if (m_setcredential_ValidTo != null)
                        {
                            credentialInfo.ValidTo = (DateTime)m_setcredential_ValidTo;
                            credentialInfo.ValidTo = credentialInfo.ValidTo.AddMilliseconds(1.0);
                        }
                    }
                    if (special == 20) //Cr.ValidTo fails
                    {
                        if (m_setcredential_ValidFrom != null)
                        {
                            credentialInfo.ValidFrom = (DateTime)m_setcredential_ValidFrom;
                            credentialInfo.ValidFrom = credentialInfo.ValidFrom.AddMilliseconds(1.0);
                        }
                    }
                    #endregion
                }
            }
        
        #endregion
        #region General

        internal ServiceCapabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            return GetCommand<ServiceCapabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, out stepType, out ex, out timeout);
        }

        internal CredentialIdentifierFormatTypeInfo[] GetSupportedFormatTypesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            return GetCommand<CredentialIdentifierFormatTypeInfo[]>("GetSupportedFormatTypes", GetSupportedFormatTypes, validationRequest, out stepType, out ex, out timeout);
        }

        #endregion //General

        #region CredentialInfo

        internal CredentialInfo[] GetCredentialInfoTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            int special;
            var res = GetCommand<CredentialInfo[]>("GetCredentialInfo", GetCredentialInfo, validationRequest, out stepType, out ex, out timeout, out special);
            CredentialInfoFromCreateModifyUpdate(special, ref res);
            return res;
        }

        internal CredentialInfo[] TakeCredentialInfoList()
        {
            return TakeSpecialParameter<CredentialInfo[]>("GetCredentialInfoList", GetCredentialInfoList, "ArrayOfCredentialInfo");
        }

        internal string GetCredentialInfoListTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("GetCredentialInfoList", GetCredentialInfoList, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //CredentialInfo

        #region Credential

        internal Credential[] GetCredentialsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            int special;
            var res = GetCommand<Credential[]>("GetCredentials", GetCredentials, validationRequest, out stepType, out ex, out timeout, out special);
            CredentialFromCreateModifyUpdate(special, ref res);
            return res;
        }

        internal Credential[] TakeCredentialList()
        {
            string special;
            var res = TakeSpecialParameter<Credential[]>("GetCredentialList", GetCredentialList, "ArrayOfCredential", "special", out special);
            if (special != "")
            {
                CredentialFromCreateModifyUpdate(Convert.ToInt32(special), ref res);
            }
            return res;
        }

        internal string GetCredentialListTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("GetCredentialList", GetCredentialList, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //Credential

        #region Credential Create/Modify/Delete

        internal void DeleteCredentialTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DeleteCredential", DeleteCredential, validationRequest, out stepType, out ex, out Timeout);
        }

        internal string CreateCredentialTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            var res = GetCommand<string>("CreateCredential", CreateCredential, validationRequest, out stepType, out ex, out Timeout);
            CredentialFromCreateModifyRemember(validationRequest, res);
            return res;
        }

        internal void ModifyCredentialTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("ModifyCredential", ModifyCredential, validationRequest, out stepType, out ex, out Timeout);
            CredentialFromCreateModifyRemember(validationRequest);
        }

        #endregion //Credential Create/Modify/Delete

        #region Credential State

        internal CredentialState GetCredentialStateTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            return GetCommand<CredentialState>("GetCredentialState", GetCredentialState, validationRequest, out stepType, out ex, out timeout);
        }

        internal void EnableCredentialTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("EnableCredential", EnableCredential, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void DisableCredentialTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DisableCredential", DisableCredential, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void ResetAntipassbackViolationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("ResetAntipassbackViolation", ResetAntipassbackViolation, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //Credential State

        #region Credential Identifiers

        internal CredentialIdentifier[] GetCredentialIdentifiersTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            return GetCommand<CredentialIdentifier[]>("GetCredentialIdentifiers", GetCredentialIdentifiers, validationRequest, out stepType, out ex, out timeout);
        }

        internal void SetCredentialIdentifierTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("SetCredentialIdentifier", SetCredentialIdentifier, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void DeleteCredentialIdentifierTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DeleteCredentialIdentifier", DeleteCredentialIdentifier, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //Credential Identifiers

        #region Credential Access Profile

        internal CredentialAccessProfile[] GetCredentialAccessProfilesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            int special;

            var res = GetCommand<CredentialAccessProfile[]>("GetCredentialAccessProfiles", GetCredentialAccessProfiles, validationRequest, out stepType, out ex, out timeout, out special);

            #region TimaValueValidity supported
            if (special == 1) //ValidFrom and ValidTo
            { 
                 var credentialAccessProfile = res.First(rule => rule.AccessProfileToken == m_setcredential_ID);
                 
                 if (m_setcredential_AP_ValidFrom != null)
                 {
                     credentialAccessProfile.ValidFromSpecified = true;
                     credentialAccessProfile.ValidFrom = (DateTime)m_setcredential_AP_ValidFrom;
                 }
                 else
                 {
                     credentialAccessProfile.ValidFromSpecified = false;
                 }

                 if (m_setcredential_AP_ValidTo != null)
                 {
                     credentialAccessProfile.ValidToSpecified = true;
                     credentialAccessProfile.ValidTo = (DateTime)m_setcredential_AP_ValidTo;
                 }
                 else
                 {
                     credentialAccessProfile.ValidToSpecified = false;
                 }
            }
            if (special == 2) //only ValidFrom
            {
                var credentialAccessProfile = res.First(rule => rule.AccessProfileToken == m_setcredential_ID);

                if (m_setcredential_AP_ValidFrom != null)
                {
                    credentialAccessProfile.ValidFromSpecified = true;
                    credentialAccessProfile.ValidFrom = (DateTime)m_setcredential_AP_ValidFrom;
                }
                else
                {
                    credentialAccessProfile.ValidFromSpecified = false;
                }
            }
            #endregion
            #region TimeValueValidity not supported
            if (special == 10) //ValidFrom and ValidTo
            {
                var credentialAccessProfile = res.First(rule => rule.AccessProfileToken == m_setcredential_ID);

                if (m_setcredential_AP_ValidFrom != null)
                {
                    credentialAccessProfile.ValidFromSpecified = true;
                    credentialAccessProfile.ValidFrom = (DateTime)m_setcredential_AP_ValidFrom;
                    credentialAccessProfile.ValidFrom = credentialAccessProfile.ValidFrom.AddMilliseconds(1.0);
                }
                else
                {
                    credentialAccessProfile.ValidFromSpecified = false;
                }

                if (m_setcredential_AP_ValidTo != null)
                {
                    credentialAccessProfile.ValidToSpecified = true;
                    credentialAccessProfile.ValidTo = (DateTime)m_setcredential_AP_ValidTo;
                    credentialAccessProfile.ValidTo = credentialAccessProfile.ValidTo.AddMilliseconds(1.0);
                }
                else
                {
                    credentialAccessProfile.ValidToSpecified = false;
                }
            }
            if (special == 20) //only ValidFrom
            {
                var credentialAccessProfile = res.First(rule => rule.AccessProfileToken == m_setcredential_ID);

                if (m_setcredential_AP_ValidFrom != null)
                {
                    credentialAccessProfile.ValidFromSpecified = true;
                    credentialAccessProfile.ValidFrom = (DateTime)m_setcredential_AP_ValidFrom;
                    credentialAccessProfile.ValidFrom = credentialAccessProfile.ValidFrom.AddMilliseconds(1.0);
                }
                else
                {
                    credentialAccessProfile.ValidFromSpecified = false;
                }
            }
            #endregion

            return res;
        }

        internal void DeleteCredentialAccessProfilesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DeleteCredentialAccessProfiles", DeleteCredentialAccessProfiles, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void SetCredentialAccessProfilesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            int special;

            VoidCommand("SetCredentialAccessProfiles", SetCredentialAccessProfiles, validationRequest, out stepType, out ex, out Timeout, out special);

            if (validationRequest.ValidationRules.Any(rule => rule.ParameterName == "CredentialAccessProfile0/ValidFrom"))
            {
                m_setcredential_ValidFrom = (DateTime)validationRequest.ValidationRules.First(rule => rule.ParameterName == "CredentialAccessProfile0/ValidFrom").Value;
            }
            else
            {
                m_setcredential_AP_ValidFrom = null;
            }
            if (validationRequest.ValidationRules.Any(rule => rule.ParameterName == "CredentialAccessProfile[0].ValidTo"))
            {
                m_setcredential_ValidTo = (DateTime)validationRequest.ValidationRules.First(rule => rule.ParameterName == "CredentialAccessProfile0/ValidTo").Value;
            }
            else
            {
                m_setcredential_AP_ValidTo = null;
            }
            m_setcredential_ID = (string)validationRequest.ValidationRules.First(rule => rule.ParameterName == "CredentialAccessProfile0/AccessProfileToken").Value;

        }

        #endregion Credential Access Profile



    }
}
