using System;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTool.Proxies.Onvif;
using TestTool.Tests.TestCases.TestSuites.Events;
using TestTool.Tests.TestCases.Utils;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.OnvifServices;
using TestTool.Tests.TestCases.TestSuites.AccessRules;
using TestTool.Tests.Common.CommonUtils;
using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;


namespace TestTool.Tests.TestCases.TestSuites.Credential_
{
    partial class CredentialTestSuit
    {
        protected const string singleTab = "    ";
        protected const string doubleTab = singleTab + singleTab;

        #region equal utils

        public static bool equalCredentialCapabilities(CredentialServiceCapabilities fromGetServiceCapabilities,
                                                       CredentialServiceCapabilities fromGetServices,
                                                       StringBuilder logger)
        {
            bool flag = true;

            const string msgHeader = "Value of '{0}' field is inconsistent. GetServiceCapabilities: '{1}'. GetServices: '{2}'";

            if (fromGetServiceCapabilities.MaxLimit != fromGetServices.MaxLimit)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MaxLimit", fromGetServiceCapabilities.MaxLimit, fromGetServices.MaxLimit));
            }

            if (fromGetServiceCapabilities.CredentialValiditySupported != fromGetServices.CredentialValiditySupported)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "CredentialValiditySupported", fromGetServiceCapabilities.CredentialValiditySupported, fromGetServices.CredentialValiditySupported));
            }

            if (fromGetServiceCapabilities.CredentialAccessProfileValiditySupported != fromGetServices.CredentialAccessProfileValiditySupported)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "CredentialAccessProfileValiditySupported", fromGetServiceCapabilities.CredentialAccessProfileValiditySupported, fromGetServices.CredentialAccessProfileValiditySupported));
            }

            if (fromGetServiceCapabilities.MaxCredentials != fromGetServices.MaxCredentials)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MaxCredentials", fromGetServiceCapabilities.MaxCredentials, fromGetServices.MaxCredentials));
            }

            if (fromGetServiceCapabilities.MaxAccessProfilesPerCredential != fromGetServices.MaxAccessProfilesPerCredential)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MaxAccessProfilesPerCredential", fromGetServiceCapabilities.MaxAccessProfilesPerCredential, fromGetServices.MaxAccessProfilesPerCredential));
            }

            if (fromGetServiceCapabilities.ResetAntipassbackSupported != fromGetServices.ResetAntipassbackSupported)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "ResetAntipassbackSupported", fromGetServiceCapabilities.ResetAntipassbackSupported, fromGetServices.ResetAntipassbackSupported));
            }

            if (fromGetServiceCapabilities.ValiditySupportsTimeValue != fromGetServices.ValiditySupportsTimeValue)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "ValiditySupportsTimeValue", fromGetServiceCapabilities.ValiditySupportsTimeValue, fromGetServices.ValiditySupportsTimeValue));
            }

            //Two lists contains the same elements
            if (!fromGetServiceCapabilities.SupportedIdentifierType.OrderBy(e => e).SequenceEqual(fromGetServices.SupportedIdentifierType.OrderBy(e => e)))
            {
                flag = false;
                logger.AppendLine("Value of 'SupportedIdentifierType' list is inconsistent.");
            }

            return flag;
        }

        
        public static bool validateListFromGetCredentialInfo(IEnumerable<CredentialInfo> fromGetCredentialInfo,
                                                             IEnumerable<string> tokens,
                                                             StringBuilder logger)
        {
            var notRequested = fromGetCredentialInfo.Where(e => !tokens.Any(t => t == e.token));

            if (notRequested.Any())
            {
                const string msgHeader = "The list of CredentialInfo items received through GetCredentialInfo contains not requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notRequested.Aggregate("", (s, e) => s + ", " + e.token).Trim(',', ' ')));

                return false;
            }

            var duplicates = fromGetCredentialInfo.Where(e => fromGetCredentialInfo.Count(e1 => e1.token == e.token) >= 2).Select(e => e.token).Distinct();

            if (duplicates.Any())
            {
                const string msgHeader = "The list of CredentialInfo items received through GetCredentialInfo contains items with the following tokens more than once: {0}";
                logger.AppendLine(string.Format(msgHeader, duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            var notReceived = tokens.Where(t => !fromGetCredentialInfo.Any(e => e.token == t));

            if (notReceived.Any())
            {
                const string msgHeader = "The list of CredentialInfo items received through GetCredentialInfo doesn't contain requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notReceived.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            return true;
        }

        public static bool validateListFromGetCredentials(IEnumerable<Credential> fromGetCredentials,
                                                          IEnumerable<string> tokens,
                                                          StringBuilder logger)
        {
            var notRequested = fromGetCredentials.Where(e => !tokens.Any(t => t == e.token));

            if (notRequested.Any())
            {
                const string msgHeader = "The list of Credential items received through GetCredentials contains not requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notRequested.Aggregate("", (s, e) => s + string.Format(", '{0}'", e.token)).Trim(',', ' ')));

                return false;
            }

            var duplicates = fromGetCredentials.Where(e => fromGetCredentials.Count(e1 => e1.token == e.token) >= 2).Select(e => e.token).Distinct();

            if (duplicates.Any())
            {
                const string msgHeader = "The list of Credential items received through GetCredentials contains items with the following tokens more than once: {0}";
                logger.AppendLine(string.Format(msgHeader, duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            var notReceived = tokens.Where(t => !fromGetCredentials.Any(e => e.token == t));

            if (notReceived.Any())
            {
                const string msgHeader = "The list of Credential items received through GetCredentials doesn't contain requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notReceived.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            return true;
        }

        public bool equalCredentialInfoLists(IEnumerable<CredentialInfo> credentialInfoFirst,
                                             IEnumerable<CredentialInfo> credentialInfoSecond)
        {
            return credentialInfoFirst.Select(e => e.token).OrderBy(e => e).SequenceEqual(credentialInfoSecond.Select(e => e.token).OrderBy(e => e));
        }

        //public static bool equalCredentialIdentifierValue(CredentialIdentifierValue credentialIdentifierValueFirst,
        //                                                  CredentialIdentifierValue credentialIdentifierValueSecond,
        //                                                  StringBuilder logger,
        //                                                  string headerFirst = "GetCredentialInfoList",
        //                                                  string headerSecond = "GetCredentialInfo")
        //{
        //    var flag = true;

        //    var msgHeader = string.Format("Value of '{{0}}' field is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'", headerFirst, headerSecond);

        //    if (!credentialIdentifierValueFirst.Value.SequenceEqual(credentialIdentifierValueSecond.Value))
        //    {
        //        flag = false;
        //        logger.AppendLine(string.Format(msgHeader, 
        //                                        "Value", 
        //                                        Convert.ToBase64String(credentialIdentifierValueFirst.Value), 
        //                                        Convert.ToBase64String(credentialIdentifierValueSecond.Value)));
        //    }

        //    return flag;
        //}

        //public static bool equalCredentialIdentifierValueLists(IEnumerable<CredentialIdentifierValue> credentialIdentifierValueListFirst,
        //                                                       IEnumerable<CredentialIdentifierValue> credentialIdentifierValueListSecond,
        //                                                       StringBuilder logger,
        //                                                       string headerFirst = "GetCredentialInfoList",
        //                                                       string headerSecond = "GetCredentialInfo")
        //{
        //    if (null == credentialIdentifierValueListFirst)
        //        credentialIdentifierValueListFirst = new CredentialIdentifierValue[0];
        //    if (null == credentialIdentifierValueListSecond)
        //        credentialIdentifierValueListSecond = new CredentialIdentifierValue[0];

        //    if (credentialIdentifierValueListFirst.Count() != credentialIdentifierValueListSecond.Count())
        //    {
        //        logger.AppendLine(singleTab + "CredentialIdentifierValue lists has different number of items");
        //        return false;
        //    }

        //    var flag = true;

        //    var internalLogger = new StringBuilder();
        //    internalLogger.AppendLine(singleTab + "The sequence of CredentialIdentifierValue items is inconsistent:");

        //    var arrayFirst  = credentialIdentifierValueListFirst.ToArray();
        //    var arraySecond = credentialIdentifierValueListSecond.ToArray();

        //    for (int i = 0; i < arrayFirst.Count(); i++)
        //    {
        //        var l = new StringBuilder(string.Format("{0}CredentialIdentifierValue items at position {1} are different.", doubleTab, i + 1));
        //        if (!equalCredentialIdentifierValue(arrayFirst[i], arraySecond[i], l, headerFirst, headerSecond))
        //        {
        //            internalLogger.Append(l);
        //            flag = false;
        //        }
        //    }

        //    if (!flag)
        //        logger.AppendLine(internalLogger.ToStringTrimNewLine());

        //    return flag;
        //}


        public static bool equalCredentialIdentifier(CredentialIdentifier credentialIdentifierFirst,
                                                     CredentialIdentifier credentialIdentifierSecond,
                                                     StringBuilder logger,
                                                     string headerFirst = "GetCredentialInfoList",
                                                     string headerSecond = "GetCredentialInfo")
        {
            bool flag = true;

            var msgHeader  = string.Format("{{0}}Value of '{{1}}' field is inconsistent. {0}: '{{2}}'. {1}: '{{3}}'", headerFirst, headerSecond);

            if (credentialIdentifierFirst.Type.Name != credentialIdentifierSecond.Type.Name)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, singleTab, "Type.Name", credentialIdentifierFirst.Type.Name, credentialIdentifierSecond.Type.Name));
            }

            if (credentialIdentifierFirst.Type.FormatType != credentialIdentifierSecond.Type.FormatType)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, singleTab, "Type.FormatType", credentialIdentifierFirst.Type.FormatType, credentialIdentifierSecond.Type.FormatType));
            }

            if (credentialIdentifierFirst.ExemptedFromAuthentication != credentialIdentifierSecond.ExemptedFromAuthentication)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, singleTab, "ExemptedFromAuthentication", credentialIdentifierFirst.ExemptedFromAuthentication, credentialIdentifierSecond.ExemptedFromAuthentication));
            }

            if (!credentialIdentifierFirst.Value.SequenceEqual(credentialIdentifierSecond.Value))
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, singleTab, "Value", BitConverter.ToString(credentialIdentifierFirst.Value).Replace("-", ""), BitConverter.ToString(credentialIdentifierSecond.Value).Replace("-", "")));
            }
            //var internalLogger = new StringBuilder();
            //if (!equalCredentialIdentifierValueLists(credentialIdentifierFirst.CredentialIdentifierValue, credentialIdentifierSecond.CredentialIdentifierValue, internalLogger))
            //{
            //    flag = false;
            //    logger.AppendLine(internalLogger.ToStringTrimNewLine());
            //}

            return flag;
        }

        public static bool equalCredentialIdentifierLists(IEnumerable<CredentialIdentifier> fromGetCredentialInfoList,
                                                          IEnumerable<CredentialIdentifier> fromGetCredentialInfo,
                                                          StringBuilder logger,
                                                          string headerFirst = "GetCredentialInfoList",
                                                          string headerSecond = "GetCredentialInfo")
        {
            if (null == fromGetCredentialInfoList)
                fromGetCredentialInfoList = new CredentialIdentifier[0];

            if (null == fromGetCredentialInfo)
                fromGetCredentialInfo = new CredentialIdentifier[0];

            if (fromGetCredentialInfoList.Count() != fromGetCredentialInfo.Count())
            {
                logger.AppendLine(doubleTab + "CredentialIdentifier lists has different number of items");
                return false;
            }
            
            var flag = true;

            foreach (var credentialIdentifier in fromGetCredentialInfoList)
            {
                var twin = fromGetCredentialInfo.FirstOrDefault(e => e.Type.Name == credentialIdentifier.Type.Name);

                if (null == twin)
                {
                    logger.AppendLine(string.Format("{0}There is no corresponding CredentialIdentifier item for item with Type = '{1}'", doubleTab, credentialIdentifier.Type.Name));
                    flag = false;
                }
                else
                {
                    var l = new StringBuilder();
                    l.AppendLine(string.Format("{0}The CredentialIdentifier items with Type = '{1}' are inconsistent.", doubleTab, credentialIdentifier.Type.Name));
                    if (!equalCredentialIdentifier(credentialIdentifier, twin, l, headerFirst, headerSecond))
                    {
                        flag = false;
                        logger.AppendLine(l.ToStringTrimNewLine());
                    }
                }
            }

            return flag;
        }

        public static bool equalCredentialAccessProfile(CredentialAccessProfile credentialAccessProfileFirst,
                                                        CredentialAccessProfile credentialAccessProfileSecond,
                                                        StringBuilder logger,
                                                        string headerFirst = "GetCredentialInfoList",
                                                        string headerSecond = "GetCredentialInfo")
        {
            bool flag = true;

            string msgHeader  = string.Format("{{0}}Value of '{{1}}' field is inconsistent. {0}: '{{2}}'. {1}: '{{3}}'", headerFirst, headerSecond);
            string msgHeader1 = string.Format("{{0}}The field '{{1}}' is inconsistent. {0}: '{{2}}'. {1}: '{{3}}'", headerFirst, headerSecond);

            if (credentialAccessProfileFirst.AccessProfileToken != credentialAccessProfileSecond.AccessProfileToken)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, singleTab, "AccessProfileToken", credentialAccessProfileFirst.AccessProfileToken, credentialAccessProfileSecond.AccessProfileToken));
            }

            if (credentialAccessProfileFirst.ValidFromSpecified != credentialAccessProfileSecond.ValidFromSpecified)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader1, singleTab, "ValidFrom", 
                                                credentialAccessProfileFirst.ValidFromSpecified ? "present" : "absent",
                                                credentialAccessProfileSecond.ValidFromSpecified ? "present" : "absent"));
            }
            else if (credentialAccessProfileFirst.ValidFromSpecified && credentialAccessProfileFirst.ValidFrom != credentialAccessProfileSecond.ValidFrom)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, singleTab, "ValidFrom", credentialAccessProfileFirst.ValidFrom.ToString("dd.MM.yyyy hh:mm:ss:fffffff"), credentialAccessProfileSecond.ValidFrom.ToString("dd.MM.yyyy hh:mm:ss:fffffff")));
            }

            if (credentialAccessProfileFirst.ValidToSpecified != credentialAccessProfileSecond.ValidToSpecified)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader1, singleTab, "ValidTo", 
                                                credentialAccessProfileFirst.ValidToSpecified ? "present" : "absent",
                                                credentialAccessProfileSecond.ValidToSpecified ? "present" : "absent"));
            }
            else if (credentialAccessProfileFirst.ValidToSpecified && credentialAccessProfileFirst.ValidTo != credentialAccessProfileSecond.ValidTo)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, singleTab, "ValidTo", credentialAccessProfileFirst.ValidTo.ToString("dd.MM.yyyy hh:mm:ss:fffffff"), credentialAccessProfileSecond.ValidTo.ToString("dd.MM.yyyy hh:mm:ss:fffffff")));
            }

            return flag;
        }

        public static bool equalCredentialAccessProfileTimeValueUnsupported(CredentialAccessProfile credentialAccessProfileFirst,
                                                        CredentialAccessProfile credentialAccessProfileSecond,
                                                        StringBuilder logger,
                                                        string headerFirst = "GetCredentialInfoList",
                                                        string headerSecond = "GetCredentialInfo")
        {
            bool flag = true;

            string msgHeader = string.Format("{{0}}Value of '{{1}}' field is inconsistent. {0}: '{{2}}'. {1}: '{{3}}'", headerFirst, headerSecond);
            string msgHeader1 = string.Format("{{0}}The field '{{1}}' is inconsistent. {0}: '{{2}}'. {1}: '{{3}}'", headerFirst, headerSecond);

            if (credentialAccessProfileFirst.AccessProfileToken != credentialAccessProfileSecond.AccessProfileToken)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, singleTab, "AccessProfileToken", credentialAccessProfileFirst.AccessProfileToken, credentialAccessProfileSecond.AccessProfileToken));
            }

            if (credentialAccessProfileFirst.ValidFromSpecified != credentialAccessProfileSecond.ValidFromSpecified)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader1, singleTab, "ValidFrom",
                                                credentialAccessProfileFirst.ValidFromSpecified ? "present" : "absent",
                                                credentialAccessProfileSecond.ValidFromSpecified ? "present" : "absent"));
            }
            else if (credentialAccessProfileFirst.ValidFromSpecified && credentialAccessProfileFirst.ValidFrom.ToShortDateString() != credentialAccessProfileSecond.ValidFrom.ToShortDateString())
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, singleTab, "ValidFrom", credentialAccessProfileFirst.ValidFrom.ToShortDateString(), credentialAccessProfileSecond.ValidFrom.ToShortDateString()));
            }

            if (credentialAccessProfileFirst.ValidToSpecified != credentialAccessProfileSecond.ValidToSpecified)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader1, singleTab, "ValidTo",
                                                credentialAccessProfileFirst.ValidToSpecified ? "present" : "absent",
                                                credentialAccessProfileSecond.ValidToSpecified ? "present" : "absent"));
            }
            else if (credentialAccessProfileFirst.ValidToSpecified && credentialAccessProfileFirst.ValidTo.ToShortDateString() != credentialAccessProfileSecond.ValidTo.ToShortDateString())
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, singleTab, "ValidTo", credentialAccessProfileFirst.ValidTo.ToShortDateString(), credentialAccessProfileSecond.ValidTo.ToShortDateString()));
            }

            return flag;
        }

        public static bool equalCredentialAccessProfileLists(IEnumerable<CredentialAccessProfile> fromGetCredentialInfoList,
                                                             IEnumerable<CredentialAccessProfile> fromGetCredentialInfo,
                                                             StringBuilder logger,
                                                             string headerFirst = "GetCredentialInfoList",
                                                             string headerSecond = "GetCredentialInfo")
        {
            if (null == fromGetCredentialInfoList)
                fromGetCredentialInfoList = new CredentialAccessProfile[0];

            if (null == fromGetCredentialInfo)
                fromGetCredentialInfo = new CredentialAccessProfile[0];

            if (fromGetCredentialInfoList.Count() != fromGetCredentialInfo.Count())
            {
                logger.AppendLine(doubleTab + "CredentialAccessProfile lists has different number of items");
                return false;
            }

            var fromGetCredentialInfoListTokens = fromGetCredentialInfoList.Select(e => e.AccessProfileToken);
            if (fromGetCredentialInfoListTokens.Count() != fromGetCredentialInfoListTokens.Distinct().Count())
            {
                logger.AppendLine(string.Format("{0}CredentialAccessProfile list received via {1} contains items with non-unique 'AccessProfileToken' field", doubleTab, headerFirst));
                return false;
            }
            
            var flag = true;

            foreach (var accessProfileInfo in fromGetCredentialInfoList)
            {
                var twins = fromGetCredentialInfo.Where(e => e.AccessProfileToken == accessProfileInfo.AccessProfileToken);
                if (!twins.Any())
                {
                    flag = false;
                    logger.AppendLine(string.Format("{0}There is no corresponding CredentialAccessProfile item for item with Token = '{1}'",
                                                    doubleTab,
                                                    accessProfileInfo.AccessProfileToken));
                }
                else
                {
                    if (twins.Count() >= 2)
                    {
                        logger.AppendLine(string.Format("{0}There are many corresponding CredentialAccessProfile items for item with Token = '{1}' while only one is expected",
                                                        doubleTab,
                                                        accessProfileInfo.AccessProfileToken));
                        flag = false;
                    }
                    else
                    {
                        var twin = twins.First();
                        var l = new StringBuilder();
                        l.AppendLine(string.Format("{0}The AccessProfileInfo items with Token = '{1}' are inconsistent.",
                                                   doubleTab,
                                                   accessProfileInfo.AccessProfileToken));

                        if (!equalCredentialAccessProfile(accessProfileInfo, twin, l, headerFirst, headerSecond))
                        {
                            flag = false;
                            logger.AppendLine(l.ToStringTrimNewLine());
                        }
                    }
                }
            }

            return flag;
        }

        public static bool equalCredentialInfo(CredentialInfo fromGetCredentialInfoList,
                                               CredentialInfo fromGetCredentialInfo,
                                               StringBuilder logger,
                                               string headerFirst = "GetCredentialInfoList",
                                               string headerSecond = "GetCredentialInfo")
        {
            bool flag = true;

            var msgHeader  = string.Format("Value of '{{0}}' field is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'", headerFirst, headerSecond);
            var msgHeader1  = string.Format("The field '{{0}}' is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'", headerFirst, headerSecond);

            if (fromGetCredentialInfoList.token != fromGetCredentialInfo.token)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "token", fromGetCredentialInfoList.token, fromGetCredentialInfo.token));
            }

            if (fromGetCredentialInfoList.CredentialHolderReference != fromGetCredentialInfo.CredentialHolderReference)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "CredentialHolderReference", fromGetCredentialInfoList.CredentialHolderReference, fromGetCredentialInfo.CredentialHolderReference));
            }

            if (fromGetCredentialInfoList.Description != fromGetCredentialInfo.Description)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Description", fromGetCredentialInfoList.Description, fromGetCredentialInfo.Description));
            }

            if (fromGetCredentialInfoList.ValidFromSpecified != fromGetCredentialInfo.ValidFromSpecified)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader1, "ValidFrom", 
                                                fromGetCredentialInfoList.ValidFromSpecified ? "present" : "absent",
                                                fromGetCredentialInfo.ValidFromSpecified ? "present" : "absent"));
            }
            else if (fromGetCredentialInfoList.ValidFromSpecified && fromGetCredentialInfoList.ValidFrom != fromGetCredentialInfo.ValidFrom)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "ValidFrom", fromGetCredentialInfoList.ValidFrom, fromGetCredentialInfo.ValidFrom));
            }

            if (fromGetCredentialInfoList.ValidToSpecified != fromGetCredentialInfo.ValidToSpecified)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader1, "ValidTo", 
                                                fromGetCredentialInfoList.ValidToSpecified ? "present" : "absent", 
                                                fromGetCredentialInfo.ValidToSpecified ? "present" : "absent"));
            }
            else if (fromGetCredentialInfoList.ValidToSpecified && fromGetCredentialInfoList.ValidTo != fromGetCredentialInfo.ValidTo)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "ValidTo", fromGetCredentialInfoList.ValidTo, fromGetCredentialInfo.ValidTo));
            }

            return flag;
        }

        public static bool equalCredential(Credential fromGetCredentialList,
                                           Credential fromGetCredentials,
                                           StringBuilder logger,
                                           string headerFirst = "GetCredentialList",
                                           string headerSecond = "GetCredentialList")
        {
            bool flag = true;

            var msgHeader  = string.Format("{0}Value of '{{0}}' field is inconsistent. {1}: '{{1}}'. {2}: '{{2}}'", singleTab, headerFirst, headerSecond);
            var msgHeader1  = string.Format("{0}The field '{{0}}' is inconsistent. {1}: '{{1}}'. {2}: '{{2}}'", singleTab, headerFirst, headerSecond);

            if (fromGetCredentialList.token != fromGetCredentials.token)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "token", fromGetCredentialList.token, fromGetCredentials.token));
            }

            if (fromGetCredentialList.CredentialHolderReference != fromGetCredentials.CredentialHolderReference)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "CredentialHolderReference", fromGetCredentialList.CredentialHolderReference, fromGetCredentials.CredentialHolderReference));
            }

            if (fromGetCredentialList.Description != fromGetCredentials.Description)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Description", fromGetCredentialList.Description, fromGetCredentials.Description));
            }

            if (fromGetCredentialList.ValidFromSpecified != fromGetCredentials.ValidFromSpecified)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader1, "ValidFrom", 
                                                fromGetCredentialList.ValidFromSpecified ? "present" : "absent",
                                                fromGetCredentials.ValidFromSpecified ? "present" : "absent"));
            }
            else if (fromGetCredentialList.ValidFromSpecified && fromGetCredentialList.ValidFrom != fromGetCredentials.ValidFrom)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "ValidFrom", fromGetCredentialList.ValidFrom, fromGetCredentials.ValidFrom));
            }

            if (fromGetCredentialList.ValidToSpecified != fromGetCredentials.ValidToSpecified)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader1, "ValidTo", 
                                                fromGetCredentialList.ValidToSpecified ? "present" : "absent", 
                                                fromGetCredentials.ValidToSpecified ? "present" : "absent"));
            }
            else if (fromGetCredentialList.ValidToSpecified && fromGetCredentialList.ValidTo != fromGetCredentials.ValidTo)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "ValidTo", fromGetCredentialList.ValidTo, fromGetCredentials.ValidTo));
            }

            var internalLogger = new StringBuilder();
            internalLogger.AppendLine(singleTab + "CredentialIdentifier lists are inconsistent.");
            if (!equalCredentialIdentifierLists(fromGetCredentialList.CredentialIdentifier, fromGetCredentials.CredentialIdentifier, internalLogger, headerFirst, headerSecond))
            {
                flag = false;
                logger.AppendLine(internalLogger.ToStringTrimNewLine());
            }

            internalLogger.Clear();
            internalLogger.AppendLine(singleTab + "CredentialAccessProfile lists are inconsistent.");
            if (!equalCredentialAccessProfileLists(fromGetCredentialList.CredentialAccessProfile, fromGetCredentials.CredentialAccessProfile, internalLogger, headerFirst, headerSecond))
            {
                flag = false;
                logger.AppendLine(internalLogger.ToStringTrimNewLine());
            }

            return flag;
        }

        public static bool equalCredentialState(bool resetAntipassbackSupported,
                                                CredentialState fromCreateCredential,
                                                CredentialState fromGetCredentials,
                                                StringBuilder logger,
                                                string headerFirst = "CreateCredential",
                                                string headerSecond = "GetCredentialState")
        {
            bool flag = true;

            var msgHeader  = string.Format("Value of '{{0}}' field is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'", headerFirst, headerSecond);
            var msgHeader1  = string.Format("The field '{{0}}' is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'", headerFirst, headerSecond);

            if (fromCreateCredential.Enabled != fromGetCredentials.Enabled)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Enabled", fromCreateCredential.Enabled, fromGetCredentials.Enabled));
            }

            if (fromCreateCredential.Reason != fromGetCredentials.Reason)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Reason", fromCreateCredential.Reason, fromGetCredentials.Reason));
            }
            if (resetAntipassbackSupported)
            {
                if (fromGetCredentials.AntipassbackState != null)
                {
                    if (fromCreateCredential.AntipassbackState.AntipassbackViolated != fromGetCredentials.AntipassbackState.AntipassbackViolated)
                    {
                        flag = false;
                        logger.AppendLine(string.Format(msgHeader, "AntipassbackState.AntipassbackViolated", fromCreateCredential.AntipassbackState.AntipassbackViolated, fromGetCredentials.AntipassbackState.AntipassbackViolated));
                    }
                }
                else
                {
                    flag = false;
                    logger.AppendLine("The DUT supports Reset Antipassback (capability ResetAntipassbackSupported = true), but CredentialState doesn't contain AntipassbackState.");
                }
            }
            
            return flag;
        }

        public static bool equalCredentialLists(IEnumerable<Credential> arrFromGetCredentialList,
                                                IEnumerable<Credential> arrFromGetCredentials,
                                                StringBuilder logger,
                                                string headerFirst = "GetCredentialList",
                                                string headerSecond = "GetCredentialList")
        {
            if (null == arrFromGetCredentialList)
                arrFromGetCredentialList = new Credential[0];

            if (null == arrFromGetCredentials)
                arrFromGetCredentials = new Credential[0];

            if (arrFromGetCredentialList.Count() != arrFromGetCredentials.Count())
            {
                logger.AppendLine("Credential lists has different number of items");
                return false;
            }

            var flag = true;

            foreach (var credential in arrFromGetCredentialList)
            {
                var twin = arrFromGetCredentials.FirstOrDefault(e => e.token == credential.token);

                if (null == twin)
                {
                    logger.AppendLine(string.Format("There is no corresponding Credential item for item with Token = '{0}'", credential.token));
                    flag = false;
                }
                else
                {
                    var l = new StringBuilder();
                    l.AppendLine(string.Format("The Credential items with Token = '{0}' are inconsistent.", credential.token));
                    if (!equalCredential(credential, twin, l, headerFirst, headerSecond))
                    {
                        flag = false;
                        logger.AppendLine(l.ToStringTrimNewLine());
                    }
                }
            }
            return flag;
        }

        #endregion

        public List<CredentialInfo> receiveAndValidateCredentialInfoList(int limit, bool skipLimitInRequest = false)
        {
            var stepTitle = "Checking received list of CredentialInfo items";
            var msgHeader = "The received list of CredentialInfo items contains {0} items though the expected number is not more than {1}";

            string nextStartRef = null;
            var fullCredentialInfoList = new List<CredentialInfo>();

            do
            {
                CredentialInfo[] credentialInfoList1;
                nextStartRef = this.GetCredentialInfoList(skipLimitInRequest ? null : new int?(limit), nextStartRef, out credentialInfoList1);

                Assert(credentialInfoList1.Count() <= limit,
                       string.Format(msgHeader, credentialInfoList1.Count(), limit),
                       stepTitle);

                fullCredentialInfoList.AddRange(credentialInfoList1);

            } while (!string.IsNullOrEmpty(nextStartRef) && !StopRequested());

            var token = fullCredentialInfoList.Select(e => e.token);
            var duplicates = token.Where(t => fullCredentialInfoList.Count(e => e.token == t) >= 2).Distinct();

            Assert(!duplicates.Any(),
                   string.Format("The complete list of CredentialInfo items received through GetCredentialInfoList contains items with the following tokens more than once: {0}", 
                                 duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')),
                   "Checking complete list of CredentialInfo items");

            return fullCredentialInfoList;
        }

        public List<Credential> receiveAndValidateCredentialList(int limit, bool skipLimitInRequest = false)
        {
            var stepTitle = "Checking received list of Credential items";
            var msgHeader = "The received list of Credential items contains {0} items though the expected number is not more than {1}";

            string nextStartRef = null;
            var fullCredentialList = new List<Credential>();

            do
            {
                Credential[] credentialList1;
                nextStartRef = this.GetCredentialList(skipLimitInRequest ? null : new int?(limit), nextStartRef, out credentialList1);

                Assert(credentialList1.Count() <= limit,
                       string.Format(msgHeader, credentialList1.Count(), limit),
                       stepTitle);

                fullCredentialList.AddRange(credentialList1);

            } while (!string.IsNullOrEmpty(nextStartRef) && !StopRequested());

            var token = fullCredentialList.Select(e => e.token);
            var duplicates = token.Where(t => fullCredentialList.Count(e => e.token == t) >= 2).Distinct();

            Assert(!duplicates.Any(),
                   string.Format("The complete list of Credential items received through GetCredentialList contains items with the following tokens more than once: {0}", 
                                 duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')),
                   "Checking complete list of Credential items");

            return fullCredentialList;
        }

        public static bool validateListFromGetCredentialAndGetCredentialInfoConsistency(IEnumerable<CredentialInfo> fromGetCredentialInfo,
                                                                                        IEnumerable<Credential> fromGetCredentials,
                                                                                        StringBuilder logger)
        {
            if (!fromGetCredentialInfo.Select(e => e.token).OrderBy(e => e).SequenceEqual(fromGetCredentials.Select(e => e.token).OrderBy(e => e)))
            {
                logger.AppendLine("The DUT returned list of Credential items inconsistent with list of CredentialInfo items");

                return false;
            }

            bool flag = true;
            foreach (var credentialInfo in fromGetCredentialInfo)
            {
                var n = fromGetCredentials.Count(e => e.token == credentialInfo.token);
                if (1 != n)
                {
                    logger.AppendLine(string.Format("The DUT returned {0} Credential item{1} for CredentialInfo item with token '{2}'",
                                                    0 == n ? "no" : "several",
                                                    0 == n ? "" : "s",
                                                    credentialInfo.token));

                    flag = false;
                }
                else
                {
                    var ap = fromGetCredentials.FirstOrDefault(e => e.token == credentialInfo.token);

                    var internalLogger = new StringBuilder();
                    if (!equalCredentialInfo(credentialInfo, ap, internalLogger))
                    {
                        flag = false;
                        logger.AppendLine(string.Format("CredentialInfo item with token '{0}' is inconsistent with Credential item for the same token", credentialInfo.token));
                        logger.Append(internalLogger);
                    }
                }
            }

            return flag;
        }

        #region Validate consistency
        public void ValidateConsistency(CredentialIdentifier initial,
                                        CredentialIdentifier received,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            logger.AppendLine(string.Format("The CredentialIdentifier item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != received && equalCredentialIdentifier(initial, received, logger, commandInitial, commandReceive),
                   null == received ? string.Format("The {0} command returned no CredentialIdentifier item", commandReceive) : logger.ToStringTrimNewLine(),
                   "Checking received CredentialIdentifier item");
        }

        public void ValidateConsistency(CredentialIdentifier initial,
                                        IEnumerable<CredentialIdentifier> receivedList,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            var twin = receivedList.FirstOrDefault(e => e.Type.Name == initial.Type.Name);
            logger.AppendLine(string.Format("The CredentialIdentifier item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != twin && equalCredentialIdentifier(initial, twin, logger, commandInitial, commandReceive),
                   null == twin ? string.Format("Received CredentialIdentifier list doesn't contain item with Type.Name = '{0}'", initial.Type.Name) : logger.ToStringTrimNewLine(),
                   "Checking received CredentialIdentifier item");
        }

        public void ValidateConsistency(Credential initial,
                                        Credential received,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            logger.AppendLine(string.Format("The Credential item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != received && equalCredential(initial, received, logger, commandInitial, commandReceive),
                   null == received ? string.Format("The {0} command returned no Credential item", commandReceive) : logger.ToStringTrimNewLine(),
                   "Checking received Credential item");
        }

        public void ValidateConsistency(Credential initial,
                                        IEnumerable<Credential> receivedList,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            var twin = receivedList.FirstOrDefault(e => e.token == initial.token);
            logger.AppendLine(string.Format("The Credential item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != twin && equalCredential(initial, twin, logger, commandInitial, commandReceive),
                   null == twin ? string.Format("Received Credential list doesn't contain item with token = '{0}'", initial.token) : logger.ToStringTrimNewLine(),
                   "Checking received Credential item");
        }

        public void ValidateConsistency(CredentialInfo initial,
                                        CredentialInfo received,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            logger.AppendLine(string.Format("The CredentialInfo item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != received && equalCredentialInfo(initial, received, logger, commandInitial, commandReceive),
                   null == received ? string.Format("The {0} command returned no CredentialInfo item", commandReceive) : logger.ToStringTrimNewLine(),
                   "Checking received CredentialInfo item");
        }

        public void ValidateConsistency(CredentialInfo initial,
                                        IEnumerable<CredentialInfo> receivedList,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            var twin = receivedList.FirstOrDefault(e => e.token == initial.token);
            logger.AppendLine(string.Format("The CredentialInfo item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != twin && equalCredentialInfo(initial, twin, logger, commandInitial, commandReceive),
                   null == twin ? string.Format("Received CredentialInfo list doesn't contain item with token = '{0}'", initial.token) : logger.ToStringTrimNewLine(),
                   "Checking received CredentialInfo item");
        }

        public void ValidateConsistency(CredentialServiceCapabilities serviceCapabilities,
                                        CredentialState initial,
                                        CredentialState received,
                                        string commandInitial,
                                        string commandReceive = "GetCredentialState")
        {
            var logger = new StringBuilder();
            logger.AppendLine(string.Format("The CredentialState item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != received && equalCredentialState(serviceCapabilities.ResetAntipassbackSupported, initial, received, logger, commandInitial, commandReceive),
                   null == received ? string.Format("The {0} command returned no CredentialState item", commandReceive) : logger.ToStringTrimNewLine(),
                   "Checking received CredentialState item");
        }
        public void ValidateTimeValueConsistency(System.DateTime timeValue1, System.DateTime timeValue2,
                                                 string command, string element, string fieldName, bool isValiditySupportsTimeValue)
        {
            if (isValiditySupportsTimeValue)
            {
                Assert(timeValue1 == timeValue2,
                                string.Format("The field {0}.{1} received in {2}Response has unexpected value {3}. Expected: {4}", element, fieldName, command, timeValue1.ToString("dd.MM.yyyy hh:mm:ss:fffffff"), timeValue2.ToString("dd.MM.yyyy hh:mm:ss:fffffff")),
                                string.Format("Checking received {0}.{1}", element, fieldName));
            }
            else
            {
                Assert(timeValue1.ToShortDateString() == timeValue2.ToShortDateString(),
                                string.Format("The field {0}.{1} received in {2}Response has unexpected data component value {3}. Expected: {4}", element, fieldName, command, timeValue1.ToShortDateString(), timeValue2.ToShortDateString()),
                                string.Format("Checking received {0}.{1} data component", element, fieldName));
            }
        }
        #endregion

        protected void FindTopics(XmlElement element, List<XmlElement> topics)
        {
            if (element.RepresentsTopic())
            {
                topics.Add(element);
            }

            // If not a topic - enumerate child elements.
            foreach (XmlNode node in element.ChildNodes)
            {
                XmlElement child = node as XmlElement;
                if (child == null)
                {
                    continue;
                }
                FindTopics(child, topics);
            }
        }

        protected XmlElement GetTopicElement(IEnumerable<XmlElement> topics, TopicInfo topicInfo)
        {
            // check if "our" topic is present
            XmlElement topicElement = null;
            foreach (XmlElement el in topics)
            {
                TopicInfo info = TopicInfo.ConstructTopicInfo(el);
                if (TopicInfo.TopicsMatch(info, topicInfo))
                {
                    topicElement = el;
                    break;
                }
            }
            return topicElement;
        }

        #region Filter utils
        bool messageFilterBase(Proxies.Event.NotificationMessageHolderType message,
                               TopicInfo topicInfo,
                               string expectedPropertyOperation,
                               string validationScript,
                               IEnumerable<Credential> credentialList)
        {
            if (!string.IsNullOrEmpty(expectedPropertyOperation))
            {
                if (!message.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                    return true;

                XmlAttribute propertyOperationType = message.Message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];
                if (expectedPropertyOperation != propertyOperationType.Value)
                    return false;
            }

            var variables = new Dictionary<string, string>();
            variables.Add("credentialList", string.Format(@"{{{0}}}", string.Join(",", credentialList.Select(s => string.Format(@"""{0}"" : ""{1}""", s.token, s.Description)))));


            var logger = new StringBuilder();
            string validationScriptPath = string.Format("TestTool.Tests.TestCases.TestSuites.Credential.Scripts.{0}", validationScript);
            Assert(ValidationEngine.GetInstance().Validate(message,
                                                           topicInfo,
                                                           validationScriptPath,
                                                           logger,
                                                           variables),
                   logger.ToStringTrimNewLine(),
                   "Validate received notification(s)");

            return true;
        }

        bool messageFilterBase (NotificationMessageHolderType msg, TopicInfo topicInfo, string  expectedPropertyOperation, string expectedCredentialToken, Func<StringBuilder, bool> action)
        {
            var invalidFlag = false;
            var log = new StringBuilder();
            try
            {
                if (!EventServiceExtensions.NotificationTopicMatch(msg, topicInfo))
                {
                    log.AppendLine(string.Format("Received notification has unexpected topic '{0}' while the expected one should have with topic '{1}'", 
                                                 EventServiceExtensions.GetTopicInfo(msg).GetDescription(), topicInfo.GetDescription()));
                    invalidFlag = true;
                }

                if (null != expectedPropertyOperation)
                {
                    if (!msg.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                    {
                        log.AppendLine("Received notification has no 'PropertyOperation' field");
                        invalidFlag = true;
                    }
                    else
                    {
                        XmlAttribute propertyOperationType = msg.Message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];
                        //Skip non-changed messages
                        if (propertyOperationType.Value != expectedPropertyOperation)
                        {
                            log.AppendLine(string.Format("Received notification has 'PropertyOperation' field with value = '{0}' but value = '{1}' is expected", propertyOperationType.Value, expectedPropertyOperation));
                            invalidFlag = true;
                        }
                    }
                }

                var source = msg.Message.GetMessageSourceSimpleItems();
                if (!source.ContainsKey("CredentialToken"))
                {
                    log.AppendLine("Received notification has no Source/SimpleItem with Name = 'CredentialToken'");
                    invalidFlag = true;
                }
                else
                {
                    var credentialToken = source["CredentialToken"];
                    if (credentialToken != expectedCredentialToken)
                    {
                        log.AppendLine(string.Format("Received notification has Source/SimpleItem with Name = 'CredentialToken' and Value = '{0}' but notification for Credential item with token = '{1}' is expected", credentialToken, expectedCredentialToken));
                        invalidFlag = true;
                    }
                }

                if (null != action && action(log))
                    invalidFlag = true;

                return true;
            }
            finally
            {
                Assert(!invalidFlag, log.ToStringTrimNewLine(), "Validation of received notification");
            }
        }


        protected FilterInfo CreateFilter(TopicInfo topicInfo, XmlElement messageDescription)
        {
            FilterInfo filter = new FilterInfo();

            filter.Filter = CreateSubscriptionFilter(topicInfo);

            filter.MessageDescription = messageDescription;

            return filter;
        }

        protected Proxies.Event.FilterType CreateSubscriptionFilter(TopicInfo topicInfo)
        {
            return CreateSubscriptionFilter(new TopicInfo[] { topicInfo });
        }

        protected Proxies.Event.FilterType CreateSubscriptionFilter(IEnumerable<TopicInfo> topicInfos)
        {
            Proxies.Event.FilterType filter = new Proxies.Event.FilterType();

            XmlDocument filterDoc = new XmlDocument();
            XmlElement filterTopicElement = filterDoc.CreateTopicElement();

            string topicPath = string.Empty;
            foreach (TopicInfo topicInfo in topicInfos)
            {
                string topicExpression = TopicInfo.CreateTopicPath(filterTopicElement, topicInfo);

                if (string.IsNullOrEmpty(topicPath))
                {
                    topicPath = topicExpression;
                }
                else
                {
                    topicPath = string.Format("{0}|{1}", topicPath, topicExpression);
                }
            }

            filterTopicElement.InnerText = topicPath;

            filter.Any = new XmlElement[] { filterTopicElement };

            return filter;
        }
        #endregion

        #region Notification validation
        protected bool ValidateNotifications(Dictionary<Proxies.Event.NotificationMessageHolderType, XmlElement> messages, TopicInfo topic, string credentialToken, StringBuilder logger)
        {
            var filtered = messages.Where(pair => EventServiceExtensions.NotificationTopicMatch(pair.Key, pair.Value, topic)).Select(k => k.Key);

            if (!filtered.Any())
            {
                logger.AppendLine(string.Format("There is no required notification with topic '{0}'", topic.GetDescription()));
                return false;
            }

            foreach (var msg in filtered)
            {
                var source = msg.Message.GetMessageSourceSimpleItems();

                if (source.Any(s => s.Key == "CredentialToken" && s.Value == credentialToken))
                    return true;
            }

            logger.AppendLine(string.Format("There is no notification containing 'Source/SimpleItem' element with Name = 'CredentialToken' and Value = '{0}'", credentialToken));
            return false;
        }
        #endregion

        #region Credential event description

        internal class EventItemDescription
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Namespace { get; set; }
            public bool Mandatory { get; set; }

            public EventItemDescription()
                : this(null, null, null, null)
            { }

            public EventItemDescription(string path, string name, string type, string ns)
                : this(path, name, type, ns, true)
            { }

            public EventItemDescription(string path, string name, string type, string ns, bool mandatory)
            {
                Path = path;
                Name = name;
                Namespace = ns;
                Type = type;
                Mandatory = mandatory;
            }
        }

        internal class CredentialEventDescription
        {
            public List<EventItemDescription> itemDescriptions { get; private set; }

            public CredentialEventDescription()
            { itemDescriptions = new List<EventItemDescription>(); }

            public void addItemDescription(EventItemDescription description) { itemDescriptions.Add(description); }

            public bool isProperty { get; set; }
        }

        #endregion
        #region Validating utils
        bool checkEventDescription(XmlElement eventNode,
                                   CredentialEventDescription eventDescription,
                                   IXmlNamespaceResolver namespaceResolver,
                                   StringBuilder logger)
        {
            return checkEventDescription(eventNode, eventDescription, namespaceResolver.LookupNamespace, logger);
        }

        bool checkEventDescription(XmlElement eventNode,
                                   CredentialEventDescription eventDescription,
                                   XmlElement namespaceResolver,
                                   StringBuilder logger)
        {
            return checkEventDescription(eventNode, eventDescription, namespaceResolver.GetNamespaceOfPrefix, logger);
        }

        bool checkEventDescription(XmlElement eventNode, CredentialEventDescription eventDescription, Func<string, string> namespaceResolver, StringBuilder logger)
        {
            var descriptionNode = eventNode.GetMessageDescription();
            if (null == descriptionNode)
            {
                logger.AppendLine("MessageDescription element is absent.");
                return false;
            }

            var manager = new XmlNamespaceManager(eventNode.OwnerDocument.NameTable);
            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            var isPropertyAttribute = descriptionNode.Attributes[OnvifMessage.ISPROPERTY];
            if (null == isPropertyAttribute)
            {
                //IsProperty == false in description
                if (eventDescription.isProperty)
                {
                    logger.AppendLine("The 'IsProperty' attribute is absent but expected with value 'true'");
                    return false;
                }
            }
            else if (XmlConvert.ToBoolean(isPropertyAttribute.Value) != eventDescription.isProperty)
            {
                logger.AppendLine(string.Format("The value of 'IsProperty' attribute is incorrect. Expected: {0}. Actual: {1}",
                                                eventDescription.isProperty,
                                                isPropertyAttribute.Value));
                return false;
            }

            bool flag = true;
            foreach (var itemDescription in eventDescription.itemDescriptions)
            {
                var path = itemDescription.Path.Split('/').Select(e => "tt:" + e).Aggregate("", (s, s1) => s + s1 + "/").Trim('/');
                var nodes = descriptionNode.SelectNodes(path, manager).OfType<XmlElement>();
                var itemNode = nodes.FirstOrDefault(e => null != e.Attributes[OnvifMessage.NAME]
                                                         && e.Attributes[OnvifMessage.NAME].Value == itemDescription.Name);
                if (null == itemNode)
                {
                    if (itemDescription.Mandatory)
                    {
                        flag = false;
                        logger.AppendLine(string.Format("Mandatory element {0} of type '{1}' is absent", itemDescription.Name, itemDescription.Type));
                    }
                }
                else
                {
                    XmlAttribute type = itemNode.Attributes[OnvifMessage.TYPE];
                    if (type == null)
                    {
                        flag = false;
                        logger.AppendLine(string.Format("'Type' attribute is missing for '{0}' simple item", itemDescription.Name));
                    }
                    else
                    {
                        string error = string.Empty;
                        if (!type.IsCorrectQName(itemDescription.Type, itemDescription.Namespace, namespaceResolver, ref error))
                        {
                            flag = false;
                            logger.AppendLine(string.Format("'Type' attribute is incorrect for '{0}' simple item: {1}", itemDescription.Name, error));
                        }
                    }
                }
            }

            return flag;
        }
        #endregion

        #region Polling conditions
        public class WaitNotificationsForAllCredentialsPollingCondition: SubscriptionHandler.PollingConditionBase
        {
            public WaitNotificationsForAllCredentialsPollingCondition(int timeout, IEnumerable<string> waitingNotificationsFor): base(timeout)
            {
                m_WaitingNotificationsFor = new HashSet<string>(waitingNotificationsFor);
            }

            public override bool StopPulling
            {
                get { return !m_WaitingNotificationsFor.Any(); }
            }

            public override string Reason
            {
                get
                {
                    if (m_WaitingNotificationsFor.Any())
                    {
                        var log = new StringBuilder();
                        log.AppendLine("Not all required notifications are received");
                        var tokens = string.Join(", ", m_WaitingNotificationsFor.Select(e => string.Format("'{0}'", e)).ToArray()).Trim(new[] { ' ', ',' });
                        if (m_WaitingNotificationsFor.Count() > 1)
                            log.AppendFormat("    No notifications for Credentials with tokens: {0}", tokens);
                        else
                            log.AppendFormat("    No notification for Credential with token: {0}", tokens);

                        return log.ToString();
                    }
                    else
                        return "Notifications for Credentials are received";
                }
            }

            public override void Update(Dictionary<NotificationMessageHolderType, XmlElement> messages)
            {
                if (null != messages)
                    foreach (var msg in messages.Keys)
                    {
                        string credentialToken = null;
                        if (null != msg.Message.GetMessageSourceSimpleItems()
                            && msg.Message.GetMessageSourceSimpleItems().ContainsKey("CredentialToken"))
                            credentialToken = msg.Message.GetMessageSourceSimpleItems()["CredentialToken"];

                        if (null != credentialToken)
                            m_WaitingNotificationsFor.Remove(credentialToken);
                    }
            }

            public HashSet<string> WaitingNotificationsFor
            {
                get { return m_WaitingNotificationsFor; }
            }

            private readonly HashSet<string> m_WaitingNotificationsFor;
        }

        #endregion

        public void CheckRequestedInfo<T>(IEnumerable<T> list,
            string token, string itemName, Func<T, string> tokenSelector)
        {
            string error = string.Empty;

            int count = 0;
            if (list != null)
            {
                count = list.Count();
            }

            if (count == 0)
            {
                error = string.Format("No {0} information returned", itemName);
            }
            else
            {
                if (count > 1)
                {
                    error = "More than one entry returned";
                }
                else
                {
                    T item = list.First();
                    if (tokenSelector(item) != token)
                    {
                        error = "Entry for other token returned";
                    }
                }
            }
            Assert(string.IsNullOrEmpty(error), error, "Check response");

        }
    }
}
