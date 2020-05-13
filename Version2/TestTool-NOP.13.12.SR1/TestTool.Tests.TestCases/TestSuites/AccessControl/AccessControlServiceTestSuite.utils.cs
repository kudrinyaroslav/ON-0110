using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using TestTool.Proxies.Onvif;
using System.Linq;

namespace TestTool.Tests.TestCases.TestSuites
{
    partial class AccessControlServiceTestSuite
    {
        /// <summary>
        /// Compares two access points
        /// </summary>
        /// <param name="info1">First AccessPoint</param>
        /// <param name="info2">Second AccessPoint</param>
        /// <param name="dump">Buffer to add error description, if any</param>
        /// <returns>True if structures hold the same information</returns>
        bool CompareAccessPoints(AccessPointInfo info1, AccessPointInfo info2, StringBuilder dump)
        {
            bool localOk = true;

            Action<string, Func<AccessPointInfo, string>> checkStringAction =
            new Action<string, Func<AccessPointInfo, string>>(
                (name, fieldSelector) =>
                {
                    string value1 = fieldSelector(info1);
                    string value2 = fieldSelector(info2);
                    if (value1 != value2)
                    {
                        localOk = false;
                        dump.AppendFormat("   {0} is different {1}", name, Environment.NewLine);
                    }
                });

            checkStringAction("AreaFrom", S => S.AreaFrom);
            checkStringAction("AreaTo", S => S.AreaTo);
            checkStringAction("Description", S => S.Description);
            checkStringAction("Entity", S => S.Entity);
            checkStringAction("Name", S => S.Name);

            if (info1.EntityType != null && info2.EntityType != null)
            {
                checkStringAction("Type.Name", S => S.EntityType.Name);
                checkStringAction("Type.Namespace", S => S.EntityType.Namespace);
            }
            else 
            {
                if (info1.EntityType != null || info2.EntityType != null)
                {
                    localOk = false;
                    dump.AppendFormat("   Type is specified only for one of entities");
                }
            }

            //if (info1.Enabled != info2.Enabled)
            //{
            //    localOk = false;
            //    dump.AppendFormat("   Enabled is different {0}", Environment.NewLine);
            //}

            if (info1.Capabilities != null && info2.Capabilities!= null)
            {
                if (info1.Capabilities.DisableAccessPoint != info2.Capabilities.DisableAccessPoint)
                {
                    localOk = false;
                    dump.AppendFormat("   Capabilities.DisableAccessPoint is different {0}", Environment.NewLine);
                }
            }
            else
            {
                if (!(info1.Capabilities == null && info2.Capabilities == null))
                {
                    localOk = false;
                    dump.AppendFormat("   Capabilities are specified only for one structure{0}", Environment.NewLine);
                }
            }


            return localOk;

        }

        /*
        /// <summary>
        /// Compares two access controllers
        /// </summary>
        /// <param name="info1">First AccessControllerInfo</param>
        /// <param name="info2">Second AccessControllerInfo</param>
        /// <param name="dump">Buffer to add error description, if any</param>
        /// <returns>True if structures hold the same information</returns>
        bool CompareAccessControllers(AccessControllerInfo info1, AccessControllerInfo info2, StringBuilder dump)
        {
            bool localOk = true;

            if (info1.Description != info2.Description)
            {
                localOk = false;
                dump.AppendLine("   Description is different");
            }
            if (info1.Name  != info2.Name)
            {
                localOk = false;
                dump.AppendLine("   Name is different");
            }

            // compare AccessPoints lists

            bool list1Empty = info1.AccessPointList == null || info1.AccessPointList.Length == 0;
            bool list2Empty = info1.AccessPointList == null || info1.AccessPointList.Length == 0;

            // bth empty are OK
            if (!(list1Empty && list2Empty))
            {
                if (!list1Empty && !list2Empty)
                {
                    bool listsOk = true;

                    foreach (string item in info1.AccessPointList)
                    {
                        if (!info2.AccessPointList.Contains(item))
                        {
                            listsOk = false;
                            break;
                        }
                    }
                    if (listsOk)
                    {
                        foreach (string item in info2.AccessPointList)
                        {
                            if (!info1.AccessPointList.Contains(item))
                            {
                                listsOk = false;
                                break;
                            }
                        }
                    }
                    if (!listsOk)
                    {
                        localOk = false;
                        dump.AppendLine("   AccessPointList is different");
                    }
                }
                else
                {
                    localOk = false;
                    dump.AppendLine("   AccessPointList is missing in one entry");
                }
            }
            
            return localOk;
       
        }
        */
        /// <summary>
        /// Compares two area infos
        /// </summary>
        /// <param name="info1">First AreaInfo</param>
        /// <param name="info2">Second AreaInfo</param>
        /// <param name="dump">Buffer to add error description, if any</param>
        /// <returns>True if structures hold the same information</returns>
        bool CompareAreaInfo(AreaInfo info1, AreaInfo info2, StringBuilder dump)
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

            return localOk;
        }
        
        /// <summary>
        /// Compares two lists of AreaInfo (that tokens set match and that 
        /// elements with the same tokens hold the same information)
        /// </summary>
        /// <param name="list1">First list</param>
        /// <param name="list2">Second list</param>
        /// <param name="description1">First list description</param>
        /// <param name="description2">Second list description</param>
        /// <remarks>Is a test step</remarks>
        void CompareLists(IEnumerable<AreaInfo> list1,
            IEnumerable<AreaInfo> list2,
            string description1,
            string description2)
        {
            bool ok = true;
            StringBuilder logger = new StringBuilder();

            List<string> common = new List<string>();

            ok = ArrayUtils.CompareLists<AreaInfo>(list1, list2, I => I.token, common, "AreaInfo", description1,
                                                    description2, logger);

            // for common only

            bool local = ArrayUtils.CompareListItems(list1, list2, I => I.token, common, "AreaInfo", logger, CompareAreaInfo);
            ok = ok && local;

            Assert(ok, logger.ToStringTrimNewLine(), "Check that lists are the same");

        }

        /// <summary>
        /// Compares two lists of AccessPointInfo (that tokens set match and that 
        /// elements with the same tokens hold the same information)
        /// </summary>
        /// <param name="list1">First list</param>
        /// <param name="list2">Second list</param>
        /// <param name="description1">First list description</param>
        /// <param name="description2">Second list description</param>
        /// <remarks>Is a test step</remarks>
        void CompareLists(IEnumerable<AccessPointInfo> list1,
            IEnumerable<AccessPointInfo> list2,
            string description1,
            string description2)
        {

            bool ok = true;
            StringBuilder logger = new StringBuilder();

            List<string> common = new List<string>();

            ok = ArrayUtils.CompareLists<AccessPointInfo>(list1, list2, I => I.token, common, "AccessPoint", description1,
                                        description2, logger);

            // for common only

            bool local = ArrayUtils.CompareListItems(list1, list2, API => API.token, common, "AccessPoint", logger, CompareAccessPoints);
            
            ok = ok && local;

            Assert(ok, logger.ToStringTrimNewLine(), "Check that lists are the same");
        }

        /// <summary>
        /// Checks that tokens of some structures in list are present in list of all tokens
        /// </summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <param name="list">List of items</param>
        /// <param name="tokens">List of tokens</param>
        /// <param name="tokenSelector">Function to get token of an item</param>
        /// <param name="itemName">Item name</param>
        /// <param name="listDescription"></param>
        /// <remarks>Is a test step</remarks>
        private void CheckTokensInclusion<T>(IEnumerable<T> list, 
            IEnumerable<string> tokens,
            Func<T, string> tokenSelector, string itemName, string listDescription)
        {
            bool ok = true;
            StringBuilder logger = new StringBuilder();

            foreach (string token in tokens)
            {
                T foundItem =
                    list.Where(I => tokenSelector(I) == token).FirstOrDefault();

                if (foundItem == null)
                {
                    logger.AppendFormat(
                            "{0} with token '{1}' not found {2}",
                            itemName, token, Environment.NewLine);
                    ok = false;
                }
            }
            
            Assert(ok, logger.ToStringTrimNewLine(), string.Format("Check that results for all tokens are present in {0}", listDescription));
        }
        
        List<AccessPointInfo> GetFullAccessPointInfoList()
        {
            return GetFullAccessPointInfoList(null); 
        }
        
        List<AccessPointInfo> GetFullAccessPointInfoList(int? maxLimit)
        {
            PACS.GetListMethod<AccessPointInfo> getList =
                new PACS.GetListMethod<AccessPointInfo>(
                    (int? limit, string offset, out AccessPointInfo[] list) =>
                    {
                        string newOffset = null;
                        AccessPointInfo[] infos = null;
                        RunStep(() => { newOffset = Client.GetAccessPointInfoList(limit, offset, out infos); }, "Get AccessPointInfo list");
                        list = infos;
                        return newOffset;

                    }); 
            
            List<AccessPointInfo> fullList = PACS.Extensions.GetFullList(getList, maxLimit, "AccessPointInfo", Assert);

            Assert(fullList.Count > 0,
                "No Access Points returned",
                "Check that the list of AccessPoint is not empty");

            return fullList;
        }

        List<AreaInfo> GetFullAreaInfoList()
        {
            return GetFullAreaInfoList(null);
        }
          
        List<AreaInfo> GetFullAreaInfoList(int? maxLimit)
        {
            PACS.GetListMethod<AreaInfo> getList =
                new PACS.GetListMethod<AreaInfo>(
                    (int? limit, string offset, out AreaInfo[] list) =>
                    {
                        string newOffset = null;
                        AreaInfo[] infos = null;
                        RunStep(() => { newOffset = Client.GetAreaInfoList(limit, offset, out infos); }, "Get AreaInfo list");
                        list = infos;
                        return newOffset;

                    }); 

            return PACS.Extensions.GetFullList(getList, maxLimit, "AreaInfo", Assert);
        }

        int GetMaxLimit()
        {
            AccessControlServiceCapabilities capabilities = GetServiceCapabilities();

            Assert(capabilities != null, "No Capabilities returned", "Check that the DUT returned service capabilities");

            return capabilities.MaxLimit > 0 ? (int)capabilities.MaxLimit : 1;
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

    }
}
