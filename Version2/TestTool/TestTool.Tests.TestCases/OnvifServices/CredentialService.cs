using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.TestSuites.Credential_;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;

namespace TestTool.Tests.TestCases.OnvifServices
{
    public interface ICredentialService: IBaseOnvifService2<CredentialPort, CredentialPortClient>
    {}

    public static class CredentialServiceExtensions
    {
        private static void InitializeGuard(this ICredentialService s)
        {
            if (null == s.ServiceClient.Port)
                s.Test.Assert(false,
                              "Can't connect to Credential Service",
                              "Check that Credential Service is accessible");
        }
        
        public static string GetCredentialServiceAddress(this IDeviceService s, FeaturesList featureList)
        {
            return s.GetServiceAddress(OnvifService.CREDENTIAL_SERVICE);
        }

        public static CredentialServiceCapabilities GetServiceCapabilities(this ICredentialService s)
        {
            s.InitializeGuard();

            CredentialServiceCapabilities r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetServiceCapabilities(), "Get Service Capabilities(Credential)");

            return r;
        }

        public static CredentialInfo[] GetCredentialInfo(this ICredentialService s, string[] Token)
        {
            s.InitializeGuard();

            CredentialInfo[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetCredentialInfo(Token), "Get Credential Info");

            return r ?? new CredentialInfo[0];
        }

        public static string GetCredentialInfoList(this ICredentialService s, int? Limit, string StartReference, out CredentialInfo[] CredentialInfo)
        {
            s.InitializeGuard();

            string r = null;
            CredentialInfo[] localCredentialInfo = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetCredentialInfoList(Limit, StartReference, out localCredentialInfo), "Get Credential Info List");

            CredentialInfo = localCredentialInfo ?? new CredentialInfo[0];

            return r;
        }

        public static Credential[] GetCredentials(this ICredentialService s, string[] Token)
        {
            s.InitializeGuard();

            Credential[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetCredentials(Token), "Get Credentials");

            return r ?? new Credential[0];
        }

        public static string GetCredentialList(this ICredentialService s, int? Limit, string StartReference, out Credential[] Credential)
        {
            s.InitializeGuard();

            string r = null;
            Credential[] localCredential = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetCredentialList(Limit, StartReference, out localCredential), "Get Credential List");

            Credential = localCredential ?? new Credential[0];

            return r;
        }

        public static string CreateCredential(this ICredentialService s, Credential Credential, CredentialState State)
        {
            s.InitializeGuard();

            string r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.CreateCredential(Credential, State), "Create Credential");

            return r;
        }

        public static void ModifyCredential(this ICredentialService s, Credential Credential)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.ModifyCredential(Credential), "Modify Credential");
        }

        public static void DeleteCredential(this ICredentialService s, string Token)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteCredential(Token), "Delete Credential");
        }

        public static CredentialState GetCredentialState(this ICredentialService s, string Token)
        {
            s.InitializeGuard();

            CredentialState r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetCredentialState(Token), "Get CredentialState");

            return r;
        }

        public static void EnableCredential(this ICredentialService s, string Token, string Reason)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.EnableCredential(Token, Reason), "Enable Credential");
        }

        public static void DisableCredential(this ICredentialService s, string Token, string Reason)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DisableCredential(Token, Reason), "Disable Credential");
        }

        public static void ResetAntipassbackViolation(this ICredentialService s, string CredentialToken)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.ResetAntipassbackViolation(CredentialToken), "Reset Antipassback Violation");
        }

        public static CredentialIdentifier[] GetCredentialIdentifiers(this ICredentialService s, string CredentialToken)
        {
            s.InitializeGuard();

            CredentialIdentifier[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetCredentialIdentifiers(CredentialToken), "Get CredentialIdentifiers");

            return r ?? new CredentialIdentifier[0];
        }

        public static void SetCredentialIdentifier(this ICredentialService s, string CredentialToken, CredentialIdentifier CredentialIdentifier)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetCredentialIdentifier(CredentialToken, CredentialIdentifier), "Set CredentialIdentifier");
        }

        public static void DeleteCredentialIdentifier(this ICredentialService s, string CredentialToken, string CredentialIdentifierTypeName)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteCredentialIdentifier(CredentialToken, CredentialIdentifierTypeName), "Delete CredentialIdentifier");
        }

        public static CredentialAccessProfile[] GetCredentialAccessProfiles(this ICredentialService s, string CredentialToken)
        {
            s.InitializeGuard();

            CredentialAccessProfile[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetCredentialAccessProfiles(CredentialToken), "Get CredentialAccess Profiles");

            return r ?? new CredentialAccessProfile[0];
        }

        public static void SetCredentialAccessProfiles(this ICredentialService s, string CredentialToken, CredentialAccessProfile[] CredentialAccessProfile)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.SetCredentialAccessProfiles(CredentialToken, CredentialAccessProfile), "Set CredentialAccess Profiles");
        }

        public static void DeleteCredentialAccessProfiles(this ICredentialService s, string CredentialToken, string[] AccessProfileToken)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteCredentialAccessProfiles(CredentialToken, AccessProfileToken), "Delete CredentialAccess Profiles");
        }

        public static CredentialIdentifierFormatTypeInfo[] GetSupportedFormatTypes(this ICredentialService s, string CredentialIdentifierTypeName)
        {
            s.InitializeGuard();

            CredentialIdentifierFormatTypeInfo[] r = null;
            
            s.Test.RunStep(() => r = s.ServiceClient.Port.GetSupportedFormatTypes(CredentialIdentifierTypeName), "Get Supported Format Types");

            return r ?? new CredentialIdentifierFormatTypeInfo[0];
        }

        public static CredentialServiceCapabilities ExtractCredentialCapabilities(this ICredentialService s, Service service)
        {
            return s.ExtractCapabilities<CredentialServiceCapabilities, CredentialPort, CredentialPortClient>(service, "Credential");
        }

        public static List<CredentialInfo> GetFullCredentialInfoListA1(this ICredentialService s)
        {
            var r = new List<CredentialInfo>();

            string nextReference = null;
            do
            {
                CredentialInfo[] dst = null;
                nextReference = s.GetCredentialInfoList(null, nextReference, out dst);
                r.AddRange(dst);
            } while (!string.IsNullOrEmpty(nextReference) && !s.Test.StopRequested());

            return r;
        }

        public static List<Credential> GetFullCredentialListA3(this ICredentialService s)
        {
            var r = new List<Credential>();

            string nextReference = null;
            do
            {
                Credential[] dst = null;
                nextReference = s.GetCredentialList(null, nextReference, out dst);
                r.AddRange(dst);
            } while (!string.IsNullOrEmpty(nextReference) && !s.Test.StopRequested());

            return r;
        }

        public struct CredentialAndState
        {
            public Credential credential;
            public CredentialState state;
        }

        // Annex A.7: Helper procedure to check free storage for additional credential
        public static CredentialAndState[] CheckFreeStorageForCredentialA7(this ICredentialService s, List<Credential> lstCredential)
        {
            CredentialAndState[] arrCrSt = new CredentialAndState[0];

            //1. ONVIF Client gets the service capabilities (out cap) by following the procedure mentioned in Annex A.2.
            var cap = s.GetServiceCapabilities();

            //3. If number of items of credentialCompleteList less than cap.MaxCredential, skip other steps.
            if (lstCredential.Count < cap.MaxCredentials) return arrCrSt;

            //4. If number of items at credentialCompleteList equal to cap.MaxCredentials, execute the following steps:
            if (lstCredential.Count == cap.MaxCredentials)
            {
                //4.1. ONVIF client invokes GetCredentials with parameters
                Credential[] arrCr = s.GetCredentials(new string[] { lstCredential[0].token });

                Array.Resize<CredentialAndState>(ref arrCrSt, 1);
                arrCrSt[0].credential = arrCr[0];
                arrCrSt[0].state = s.GetCredentialState(arrCr[0].token);

                //4.3. ONVIF Client deletes the Credential (in credentialCompleteList[0].token) by following the procedure mentioned in Annex A.6
                s.DeleteCredential(arrCr[0].token);
            }
            else
                s.Test.Assert(false, "No free storage space for credential", "Check free storage for credentials");

            return arrCrSt;
        }

        // Annex A.7: Helper procedure to check free storage for additional credential
        public static void RestoreCredentialsA10(this ICredentialService s, CredentialAndState credentialAndState)
        {
            credentialAndState.credential.token = "";
            s.CreateCredential(credentialAndState.credential, credentialAndState.state);
        }

        public static void RestoreCredentialsA10(this ICredentialService s, IEnumerable<CredentialAndState> credentials)
        {
            foreach (var credentialAndState in credentials)
            { s.RestoreCredentialsA10(credentialAndState); }
        }
        

        public static string CreateCredentialA11(this ICredentialService s, out Credential credential, out CredentialState state, bool antipassbackViolated = false)
        {
            var serviceCapabilities = s.GetServiceCapabilities();

            var value = s.GetCredentialIdentifierTypeAndValueA15(serviceCapabilities.SupportedIdentifierType);

            credential = new Credential
                         {
                             token = "",
                             Description = "Test Description",
                             CredentialHolderReference = "TestUser",
                             CredentialIdentifier = new[] { new CredentialIdentifier { Type = new CredentialIdentifierType { Name = value.TypeName, FormatType = value.FormatType }, Value = value.Value, ExemptedFromAuthentication = false } },
                             CredentialAccessProfile = null, //new[] { new CredentialAccessProfile { AccessProfileToken = null, ValidFromSpecified = false, ValidToSpecified = false } },
                             Extension = null,
                             ValidFromSpecified = false,
                             ValidToSpecified = false
                         };

            AntipassbackState antipassbackState = null;

            if (serviceCapabilities.ResetAntipassbackSupported)
                antipassbackState = new AntipassbackState() { AntipassbackViolated = antipassbackViolated };

            state = new CredentialState
                    {
                        Enabled = true,
                        Reason = "Test Reason",
                        AntipassbackState = antipassbackState
                    };

            return s.CreateCredential(credential, state);
        }

        public static string CreateCredentialA11(this ICredentialService s, bool antipassbackViolated = false)
        {
            Credential credential = null;
            CredentialState state = null;

            return s.CreateCredentialA11(out credential, out state, antipassbackViolated);
        }

        public static string CreateCredentialA11(this ICredentialService s, out string credentialTypeName, bool antipassbackViolated = false)
        {
            Credential credential = null;
            CredentialState state = null;

            string credentialToken = s.CreateCredentialA11(out credential, out state, antipassbackViolated);

            credentialTypeName = credential.CredentialIdentifier != null && credential.CredentialIdentifier.Length > 0 ? credential.CredentialIdentifier.First().Type.Name : null;

            return credentialToken;
        }

        public static string[] SupportedCredentialIdentifierFormatTypesA14(this ICredentialService s)
        {
            return new []
            {
                "WIEGAND26",
                "WIEGAND37",
                "WIEGAND37_FACILITY",
                "FACILITY16_CARD32",
                "FACILITY32_CARD32",
                "FASC_N",
                "FASC_N_BCD",
                "FASC_N_LARGE",
                "FASC_N_LARGE_BCD",
                "GSA75",
                "GUID",
                "CHUID",
                "CHUID_FULL",
                //"CBEFF_A",
                //"CBEFF_B",
                //"CBEFF_C",
                "USER_PASSWORD",
                "SIMPLE_NUMBER16",
                "SIMPLE_NUMBER32",
                "SIMPLE_NUMBER56",
                "SIMPLE_ALPHA_NUMERIC",
                "ABA_TRACK2"
            };
        }

        public static CredentialIdentifierValue GetCredentialIdentifierTypeAndValueA15(this ICredentialService s, IEnumerable<string> identifierTypeList)
        {
            foreach (var typeID in identifierTypeList)
            {
                var supportedTypes = s.GetSupportedFormatTypes(typeID);

                s.Test.Assert(supportedTypes.Any(), 
                              string.Format("GetSupportedFormatTypes command returned no supported format types for CredentialIdentifierType with ID = '{0}' that is declared as supported", typeID), 
                              "Check GetSupportedFormatTypes returned at least one CredentialIdentifierFormatTypeInfo for specified CredentialIdentifierType");

                CredentialIdentifierValue r = null;
                s.Test.RunStep(() =>
                               {
                                   foreach (var typeInfo in supportedTypes)
                                   {
                                       if (s.SupportedCredentialIdentifierFormatTypesA14().Contains(typeInfo.FormatType))
                                           r = new CredentialIdentifierValue(typeID,
                                                                             typeInfo,
                                                                             CredentialIdentifierValueFactory.Create(typeInfo.FormatType, "TestValue"));
                                   }
                               }, 
                               "Create CredentialIdentifier value");

                return r;
            }

            //Check Management tab contains value in custom format
            var flag = null != s.Test.CredentialIdentifierValueFirst || null != s.Test.CredentialIdentifierValueThird;
            s.Test.Assert(flag, 
                          "'Management' tab contains no value of CredentialIdentifier in custom format", 
                          "Check 'Management' tab contains value of CredentialIdentifier in custom format");

            if (null != s.Test.CredentialIdentifierValueFirst && identifierTypeList.Contains(s.Test.CredentialIdentifierValueFirst.TypeName))
                return s.Test.CredentialIdentifierValueFirst;

            if (null != s.Test.CredentialIdentifierValueThird && identifierTypeList.Contains(s.Test.CredentialIdentifierValueThird.TypeName))
                return s.Test.CredentialIdentifierValueThird;

            s.Test.Assert(false, 
                          "'Management' tab contains no value of CredentialIdentifier in allowable custom format", 
                          "Check 'Management' tab contains value of CredentialIdentifier in allowable custom format");

            return null;
        }

        public static List<string> GetCredentialIdentifierTypesSupportedAtleastTwoFormatTypesA16(this ICredentialService s)
        {
            var r = new List<string>();

            var identifierTypeList = s.GetServiceCapabilities().SupportedIdentifierType;

            foreach (var typeID in identifierTypeList)
            {
                var supportedTypes = s.GetSupportedFormatTypes(typeID);

                s.Test.Assert(supportedTypes.Any(), 
                              string.Format("GetSupportedFormatTypes command returned no supported format types for CredentialIdentifierType with ID = '{0}' that is declared as supported", typeID), 
                              "Check GetSupportedFormatTypes returned at least one CredentialIdentifierFormatTypeInfo for specified CredentialIdentifierType");

                if (supportedTypes.Count() >= 2)
                    r.Add(typeID);
            }

            return r;
        }

        public static void GetCredentialIdentifierTypeAndValueForTypeSupportedAtleastTwoFormatTypesA17(this ICredentialService s, IEnumerable<string> identifierTypeList,
                                                                                                       out CredentialIdentifierValue valueFirst, out CredentialIdentifierValue valueSecond)
        {
            valueFirst = null;
            valueSecond = null;

            var firstGUIValue  = s.Test.CredentialIdentifierValueFirst;
            var secondGUIValue = s.Test.CredentialIdentifierValueSecond;

            foreach (var typeID in identifierTypeList)
            {
                var supportedTypes = s.GetSupportedFormatTypes(typeID);

                s.Test.Assert(supportedTypes.Any(), 
                              string.Format("GetSupportedFormatTypes command returned no supported format types for CredentialIdentifierType with ID = '{0}' that is declared as supported", typeID), 
                              "Check GetSupportedFormatTypes returned at least one CredentialIdentifierFormatTypeInfo for specified CredentialIdentifierType");

                var customFormats = new List<string>();
                if (null != firstGUIValue && firstGUIValue.TypeName == typeID)
                    customFormats.Add(firstGUIValue.FormatType);
                if (null != secondGUIValue && secondGUIValue.FormatType == typeID)
                    customFormats.Add(secondGUIValue.FormatType);
                var possibleFormatTypes = s.SupportedCredentialIdentifierFormatTypesA14().ToList();
                possibleFormatTypes.AddRange(customFormats);

                var odttSupportedTypes = supportedTypes.Where(e => possibleFormatTypes.Contains(e.FormatType));
                if (odttSupportedTypes.Count() >= 2)
                {
                    CredentialIdentifierValue valueFirstLocal = null;
                    CredentialIdentifierValue valueSecondLocal = null;
                    s.Test.RunStep(() =>
                                   {
                                        foreach (var typeInfo in odttSupportedTypes)
                                        {
                                            if (null == valueFirstLocal)
                                            {
                                                if (customFormats.Contains(typeInfo.FormatType))
                                                {
                                                    if (null != firstGUIValue && firstGUIValue.FormatType == typeInfo.FormatType)
                                                        valueFirstLocal = firstGUIValue;
                                                    else 
                                                        valueFirstLocal = secondGUIValue;
                                                }
                                                else
                                                    valueFirstLocal = new CredentialIdentifierValue(typeID, typeInfo, CredentialIdentifierValueFactory.Create(typeInfo.FormatType, "TestValue1"));
                                            }
                                            else if (null == valueSecondLocal)
                                            {
                                                if (customFormats.Contains(typeInfo.FormatType))
                                                {
                                                    if (null != firstGUIValue && firstGUIValue.FormatType == typeInfo.FormatType && firstGUIValue != valueFirstLocal)
                                                        valueSecondLocal = firstGUIValue;
                                                    else if (null != secondGUIValue && secondGUIValue.FormatType == typeInfo.FormatType)
                                                        valueSecondLocal = secondGUIValue;
                                                }
                                                else
                                                    valueSecondLocal = new CredentialIdentifierValue(typeID, typeInfo, CredentialIdentifierValueFactory.Create(typeInfo.FormatType, "TestValue2"));
                                            }
                                            else 
                                                break;
                                        }
                                   }, 
                                   "Create CredentialIdentifier values");

                    valueFirst = valueFirstLocal;
                    valueSecond = valueSecondLocal;

                    return;
                }
            }

            //Check Management tab contains value in custom format

            if (null != firstGUIValue && null != secondGUIValue)
                s.Test.Assert(firstGUIValue.FormatType != secondGUIValue.FormatType, 
                              "'Management' tab contains no two values of CredentialIdentifier of the same custom type but in different Format types", 
                              "Check 'Management' tab contains two values of CredentialIdentifier of the same custom type");
            else
                s.Test.Assert(false, 
                              "'Management' tab contains no two values of CredentialIdentifier of the same custom type", 
                              "Check 'Management' tab contains two values of CredentialIdentifier of the same custom type");

            valueFirst  = firstGUIValue;
            valueSecond = secondGUIValue;
        }

        public static string CreateCredentialWithTwoCredentialIdentifierItemsA18(this ICredentialService s, bool antipassbackViolated, CredentialServiceCapabilities serviceCapabilities,
                                                                                 out CredentialIdentifier credentialIdentifierFirst, out CredentialIdentifier credentialIdentifierSecond)
        {
            credentialIdentifierFirst  = null;
            credentialIdentifierSecond = null;

            if (serviceCapabilities == null)
            {
                serviceCapabilities = s.GetServiceCapabilities();
            }

            var valueFirst  = s.GetCredentialIdentifierTypeAndValueA15(serviceCapabilities.SupportedIdentifierType);
            var valueSecond = s.GetCredentialIdentifierTypeAndValueA15(serviceCapabilities.SupportedIdentifierType.Where(e => e != valueFirst.TypeName));

            //7. ONVIF client invokes CreateCredential with parameters by following the procedure mentioned in Annex A.11.
            var credential = new Credential
                         {
                             token = "",
                             Description = "Test Description",
                             CredentialHolderReference = "TestUser",
                             CredentialIdentifier = new[] { credentialIdentifierFirst = new CredentialIdentifier 
                                                                { 
                                                                    Type = new CredentialIdentifierType { Name = valueFirst.TypeName, FormatType = valueFirst.FormatType }, 
                                                                    Value = valueFirst.Value, 
                                                                    ExemptedFromAuthentication = false 
                                                                },
                                                            credentialIdentifierSecond = new CredentialIdentifier 
                                                                { 
                                                                    Type = new CredentialIdentifierType { Name = valueSecond.TypeName, FormatType = valueSecond.FormatType }, 
                                                                    Value = valueSecond.Value, 
                                                                    ExemptedFromAuthentication = false 
                                                                } 
                                                            },
                             CredentialAccessProfile = null,
                             Extension = null,
                             ValidFromSpecified = false,
                             ValidToSpecified = false
                         };

            AntipassbackState antipassbackState = null;

            if (serviceCapabilities.ResetAntipassbackSupported)
                antipassbackState = new AntipassbackState() { AntipassbackViolated = antipassbackViolated };

            var state = new CredentialState
                    {
                        Enabled = true,
                        Reason = "Test Reason",
                        AntipassbackState = antipassbackState
                    };

            return s.CreateCredential(credential, state);
        }
    }
}
