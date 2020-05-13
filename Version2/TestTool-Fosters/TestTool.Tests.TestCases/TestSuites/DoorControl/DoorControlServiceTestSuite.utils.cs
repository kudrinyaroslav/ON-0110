using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.ServiceModel;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;

namespace TestTool.Tests.TestCases.TestSuites
{
    partial class DoorControlServiceTestSuite
    {
        /// <summary>
        /// Compares two DoorInfo
        /// </summary>
        /// <param name="info1">First structure</param>
        /// <param name="info2">Second structure</param>
        /// <param name="dump">Errors log</param>
        /// <returns>True, if information match, false otherwise. If information is different,
        /// edtails are appended to the log.</returns>
        bool CompareDoorInfo(DoorInfo info1, DoorInfo info2, StringBuilder dump)
        {
            bool localOk = true;

            if (info1.Description != info2.Description)
            {
                localOk = false;
                dump.AppendLine("   Description is different");
            }
            if (info1.Name != info2.Name)
            {
                localOk = false;
                dump.AppendLine("   Name is different");
            }

            // Capabilities

            if (info1.Capabilities != null && info2.Capabilities != null)
            {
                bool capabilitiesOk = true;
                StringBuilder shortDump = new StringBuilder("   DoorCapabilities are different:" + Environment.NewLine);

                Action<string, Func<DoorCapabilities, bool>> checkBoolAction =
                    new Action<string, Func<DoorCapabilities, bool>>(
                        (name, fieldSelector) =>
                        {
                            bool value1 = fieldSelector(info1.Capabilities);
                            bool value2 = fieldSelector(info2.Capabilities);
                            if (value1 != value2)
                            {
                                capabilitiesOk = false;
                                shortDump.AppendFormat("      {0} is different {1}", name, Environment.NewLine);
                            }
                        });

                checkBoolAction("Block", S => S.Block);
                checkBoolAction("DoubleLock", S => S.DoubleLock);
                checkBoolAction("Lock", S => S.Lock);
                checkBoolAction("LockDown", S => S.LockDown);
                checkBoolAction("LockOpen", S => S.LockOpen);
                checkBoolAction("MomentaryAccess", S => S.Access);
                checkBoolAction("Unlock", S => S.Unlock);

                if (!capabilitiesOk)
                {
                    localOk = false;
                    dump.AppendFormat(shortDump.ToString());
                }

            }
            else
            {
                if ( !(info1.Capabilities == null && info2.Capabilities == null))
                {
                    // error
                    dump.AppendLine("   DoorCapabilities are specified only in one structure");
                    localOk = false;
                }
            }

            return localOk;
        }

        /// <summary>
        /// Compares DoorInfo list (checks that for each token corresponding item is present in second list,
        /// checks that common elements are the same). 
        /// </summary>
        /// <param name="list1">First list</param>
        /// <param name="list2">Second list</param>
        /// <param name="description1">Description of the first list</param>
        /// <param name="description2">Description of the second lise</param>
        void CompareLists(IEnumerable<DoorInfo> list1, 
            IEnumerable<DoorInfo> list2, 
            string description1, 
            string description2)
        {
            bool ok = true;
            StringBuilder logger = new StringBuilder();

            List<string> common = new List<string>();

            ok = ArrayUtils.CompareLists<DoorInfo>(list1, list2, I => I.token, common, "DoorInfo", description1,
                                                    description2, logger);

            // for common only

            bool local = ArrayUtils.CompareListItems(list1, list2, I => I.token, common, "DoorInfo", logger, CompareDoorInfo);
            ok = ok && local;

            Assert(ok, logger.ToStringTrimNewLine(), "Check that lists are the same");

        }

        /// <summary>
        /// Checks that door subset is selected as expected
        /// </summary>
        /// <param name="actual">List actually received</param>
        /// <param name="expected">Expected list</param>
        void ValidateSubset(IEnumerable<DoorInfo> actual,
            IEnumerable<DoorInfo> expected)
        {
            bool ok = true;
            StringBuilder logger = new StringBuilder();

            List<string> common = new List<string>();

            /****************/

            foreach (DoorInfo info in expected)
            {
                string token = info.token;
                DoorInfo[] foundItems =
                    actual.Where(I => I.token == token).ToArray();

                if (foundItems.Length == 0)
                {
                    logger.AppendFormat(
                            "DoorInfo with token '{0}' not found {1}",
                            token, Environment.NewLine);
                    ok = false;
                }
                else
                {
                    common.Add(token);
                }
            }

            foreach (DoorInfo info in actual)
            {
                string token = info.token ;
                DoorInfo[] foundItems =
                    expected.Where(I => I.token == token).ToArray();

                if (foundItems.Length == 0)
                {
                    logger.AppendFormat(
                            "DoorInfo with token '{0}' not expected {1}",
                            info.token, Environment.NewLine);
                    ok = false;
                }
            }

            /****************/

            // for common only

            bool local = ArrayUtils.CompareListItems(actual, expected, I => I.token, common, "DoorInfo", logger, CompareDoorInfo);
            ok = ok && local;

            Assert(ok, logger.ToStringTrimNewLine(), "Check that lists are the same");

        }

        /// <summary>
        /// Gets access point XML elements from "raw" SOAP packet
        /// </summary>
        /// <param name="responses">SOAP packet</param>
        /// <returns>Dictionary of XML elements representing AccessPoints</returns>
        Dictionary<string, XmlElement> GetRawAccessPointElements(List<string> responses)
        {
            Dictionary<string, XmlElement> elements = new Dictionary<string, XmlElement>();

            string path = "/s:Envelope/s:Body/tac:GetAccessPointInfoListResponse/tac:AccessPointInfoList/tac:AccessPointInfo";
            
            foreach (string response in responses)
            {
                XmlDocument doc = BaseNotificationUtils.GetRawResponse(response);
                XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
                manager.AddNamespace("tac", ACNAMESPACE);
                manager.AddNamespace("s", "http://www.w3.org/2003/05/soap-envelope");

                XmlNodeList nodes = doc.SelectNodes(path, manager);
                foreach (XmlNode node in nodes)
                {
                    XmlElement element = node as XmlElement;
                    if (element == null)
                    {
                        continue;
                    }

                    string name = element.Attributes["token"].Value;

                    if (elements.ContainsKey(name))
                    {
                        throw new AssertException(string.Format("Token '{0}' is not unique", name));
                    }

                    elements.Add(name, element);
                }
            }
            return elements;
        }

        /// <summary>
        /// Gets access point info list from the DUT
        /// </summary>
        /// <returns></returns>
        protected List<AccessPointInfo> GetAccessPointInfoList()
        {
            Proxies.Onvif.PACSPortClient client = PACSPortClient;

            AccessControlServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = client.GetServiceCapabilities(); }, "Get AccessControl service capabilities");
            
            DoRequestDelay();
            
            Assert(capabilities != null, "No Capabilities returned", "Check that the DUT returned service capabilities");

            PACS.GetListMethod<AccessPointInfo> getList =
                new PACS.GetListMethod<AccessPointInfo>(
                    (int? limit, string offset, out AccessPointInfo[] list) =>
                    {
                        string newOffset = null;
                        AccessPointInfo[] infos = null;
                        RunStep(() => { newOffset = client.GetAccessPointInfoList(limit, offset, out infos); }, "Get AccessPointInfo list");
                        list = infos;
                        return newOffset;

                    }); 

            int maxLimit = capabilities.MaxLimit > 0 ? (int)capabilities.MaxLimit : 1;
            List<AccessPointInfo> fullList = PACS.Extensions.GetFullList(getList, maxLimit , "AccessPointInfo", Assert);

            Assert(fullList.Count > 0,
                "No Access Points returned",
                "Check that the list of AccessPoint is not empty");

            return fullList;
        }

        void ValidateDoorCapabilitiesAndState(DoorInfo info, DoorState state)
        {
            bool ok = true;
            StringBuilder logger = new StringBuilder();
            DoorCapabilities capabilities = info.Capabilities;

            //6.	Check that if DoorInfo.Capabilities.DoorMonitor = "true", than 
            // DoorState.DoorPhysicalState is present. Otherwise if DoorInfo.Capabilities.DoorMonitor = "false", 
            // than check that DoorState.DoorPhysicalState is skipped.

            bool local = CheckCapabilityAndStateField(info.token, 
                capabilities, 
                C => C.DoorMonitorSpecified && C.DoorMonitor, 
                "DoorMonitor", 
                state, 
                S => S.DoorPhysicalStateSpecified, 
                "DoorPhysicalState", 
                logger);
            ok = ok && local;
            //7.	Check that if DoorInfo.Capabilities.LockMonitor = "true", than DoorState.LockPhysicalState 
            // is present. Otherwise if DoorInfo.Capabilities.LockMonitor = "false", than check that 
            // DoorState.LockPhysicalState is skipped.
            local = CheckCapabilityAndStateField(info.token,
                capabilities,
                C => C.LockMonitorSpecified && C.LockMonitor,
                "LockMonitor",
                state,
                S => S.LockPhysicalStateSpecified,
                "LockPhysicalState",
                logger);
            ok = ok && local;

            //8.	Check that if DoorInfo.Capabilities.DoubleLockMonitor = "true", 
            // than DoorState.DoubleLockPhysicalState is present. Otherwise if DoorInfo.Capabilities.DoubleLockMonitor = "false", 
            // than check that DoorState.DoubleLockPhysicalState is skipped.
            local = CheckCapabilityAndStateField(info.token,
                capabilities,
                C => C.DoubleLockMonitorSpecified && C.DoubleLockMonitor,
                "DoubleLockMonitor",
                state,
                S => S.DoubleLockPhysicalStateSpecified,
                "DoubleLockPhysicalState",
                logger);
            ok = ok && local;

            //9.	Check that if DoorInfo.Capabilities.Alarm = "true", than DoorState.Alarm is present. Otherwise 
            // if DoorInfo.Capabilities.Alarm = "false", than check that DoorState.Alarm is skipped.
            local = CheckCapabilityAndStateField(info.token,
                capabilities,
                C => C.AlarmSpecified && C.Alarm,
                "Alarm",
                state,
                S => S.AlarmSpecified,
                "Alarm",
                logger);
            ok = ok && local;
           
            //10.	Check that if DoorInfo.Capabilities.Tamper = "true", than DoorState.Tamper is present. Otherwise 
            // if DoorInfo.Capabilities.Tamper = "false", than check that DoorState.Tamper is skipped.

            local = CheckCapabilityAndStateField(info.token,
                capabilities,
                C => C.TamperSpecified && C.Tamper,
                "Tamper",
                state,
                S => S.Tamper != null,
                "Tamper",
                logger);
            ok = ok && local;

            local = CheckCapabilityAndStateField(info.token,
                capabilities,
                C => C.FaultSpecified && C.Fault,
                "Fault",
                state,
                S => S.Fault != null ,
                "Fault",
                logger);
            ok = ok && local;

            Assert(ok, logger.ToStringTrimNewLine(), "Validate DoorState and DoorCapabilities");
        }

        bool CheckCapabilityAndStateField(string token, 
            DoorCapabilities capabilities, Func<DoorCapabilities, bool> capabilitySelector, string capabilityField,
            DoorState state, Func<DoorState, bool> stateSelector, string stateField, StringBuilder logger)
        {
            bool ok = true;
            
            if (capabilitySelector(capabilities))
            {
                if (!stateSelector(state))
                {
                    ok = false;
                    logger.AppendLine(string.Format("'{0}' is missing in DoorState, while '{1}' capability is present", stateField, capabilityField));
                }
            }
            else
            {
                if (stateSelector(state))
                {
                    ok = false;
                    logger.AppendLine(string.Format("'{0}' is present in DoorState, while '{1}' capability is absent", stateField, capabilityField));
                }
            }

            return ok;
        }

        #region DoorOperations

        /// <summary>
        /// Common door operation positive test (for all door whilch support operation perform operation)
        /// </summary>
        /// <param name="action">Operation</param>
        /// <param name="checkAction">Method for checking support</param>
        void GeneralDoorOperationTest(Action<string> action, Func<DoorCapabilities, bool> checkAction)
        {
            RunTest(() =>
            {
                List<DoorInfo> infos = GetFullDoorInfoList();
                bool testPerformed = false;

                foreach (DoorInfo info in infos)
                {
                    if (checkAction(info.Capabilities))
                    {
                        testPerformed = true;
                        action(info.token);
                    }
                }

                if (!testPerformed)
                {
                    LogTestEvent(string.Format("No doors support this command, real testing not performed{0}", Environment.NewLine));
                }
            });
        }

        /// <summary>
        /// Common "Door operation - INVALID TOKEN" test
        /// </summary>
        /// <param name="action">Action to be tested</param>
        /// <param name="stepNamePattern">Step name pattern</param>
        void GeneralDoorOperationInvalidTokenTest(Action<string> action,
            string stepNamePattern)
        {

            string token = Guid.NewGuid().ToString().Substring(0, 8);

            RunStep(() => action(token), string.Format(stepNamePattern, token), OnvifFaults.NotFound, true, false);
            
        }

        /// <summary>
        /// Check that fault is not null
        /// </summary>
        /// <param name="exc">Exception to be validated</param>
        /// <param name="reason">(out) error description</param>
        /// <returns>True if fault is not null, false otherwise</returns>
        bool CheckNotSupportedFault(FaultException exc, out string reason)
        {
            reason = null;
            if (exc == null)
            {
                reason = "No SOAP fault returned"; 
            }
            return exc != null;
        }

        /// <summary>
        /// Common "NOT SUPPORTED" door operation test
        /// </summary>
        /// <param name="action">Action to be tested</param>
        /// <param name="checkAction">Action to understand if operation is supported</param>
        /// <param name="stepNamePattern">Step name pattern</param>
        void GeneralDoorOperationCommandNotSupportedTest(Action<string> action,
            Func<DoorCapabilities, bool> checkAction,
            string stepNamePattern)
        {
            List<DoorInfo> infos = GetFullDoorInfoList();

            bool testPerformed = false;
            foreach (DoorInfo info in infos)
            {
                if (!checkAction(info.Capabilities))
                {
                    testPerformed = true;

                    string token =info.token;
                    string stepName = string.Format(stepNamePattern, token);
                    RunStep( () => action(token), stepName, CheckNotSupportedFault);
                }
            }

            if (!testPerformed)
            {
                LogTestEvent(string.Format("All doors support this command, real testing not performed{0}", Environment.NewLine));
            }
        }


        List<DoorInfo> GetFullDoorInfoList()
        {
            return GetFullDoorInfoList(null); 
        }

        List<DoorInfo> GetFullDoorInfoList(int? maxLimit)
        {
            PACS.GetListMethod<DoorInfo> getList =
                new PACS.GetListMethod<DoorInfo>(
                    (int? limit, string offset, out DoorInfo[] list) =>
                    {
                        string newOffset = null;
                        DoorInfo[] infos = null;
                        RunStep(() => { newOffset = Client.GetDoorInfoList(limit, offset, out infos); }, "Get DoorInfo list");
                        list = infos;
                        return newOffset;

                    }); 

            List<DoorInfo> fullList = PACS.Extensions.GetFullList(getList, maxLimit, "DoorInfo", Assert);

            Assert(fullList.Count > 0,
                "No Doors returned",
                "Check that the list of Doors is not empty");

            return fullList;
        }

        int GetMaxLimit()
        {
            DoorControlServiceCapabilities capabilities = GetServiceCapabilities();

            Assert(capabilities != null, "No Capabilities returned", "Check that the DUT returned service capabilities");

            return capabilities.MaxLimit > 0 ? (int)capabilities.MaxLimit : 1;
        }

        #endregion
    }
}
