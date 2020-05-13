using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.TestSuites.PACS;
using TestTool.Tests.Definitions.Onvif;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public partial class AccessControlServiceTestSuite : AccessControlTest
    {
        public AccessControlServiceTestSuite(TestLaunchParam param)
            : base(param)
        {

        }


        private const string PATH_ACCESSCONTROL_AREA = "Access Control\\Area";
        private const string PATH_ACCESSCONTROL_ACCESSPOINT = "Access Control\\Access Point";
        private const string PATH_ACCESSCONTROL_ACCESSCONTROLLER = "Access Control\\Access Controller";

        private const string PATH_ACCESSCONTROL_CONSISTENCY = "Access Control\\Consistency";


        [Test(Name = "GET ACCESS POINT INFO",
            Order = "02.01.01",
            Id = "2-1-1",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_ACCESSPOINT,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessPointInfo })]
        public void GetAccessPointInfoListTest()
        {
            CommonGetListTest<AccessPointInfo>(GetFullAccessPointInfoList, GetAccessPointInfo, 
                CompareLists, API => API.token, "AccessPoint", "AccessPointInfo");
        }

        [Test(Name = "GET ACCESS POINT INFO WITH INVALID TOKEN",
            Order = "02.01.02",
            Id = "2-1-2",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_ACCESSPOINT,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessPointInfo })]
        public void GetAccessPointInfoInfoListInvalidTokenTest()
        {
            RunTest(() =>
            {
                List<AccessPointInfo> fullList = GetFullAccessPointInfoList();

                if (fullList == null || fullList.Count == 0)
                {
                    // should be inaccessible (GetFullAccessPointInfoList checks that list is not empty)
                    return;
                }
                
                string token = Guid.NewGuid().ToString().Substring(0, 8);
                AccessPointInfo[] infos = null;
                RunStep(() => { infos = Client.GetAccessPointInfo(new string[] { token }); }, "Get AccessPointInfo with invalid token");
                Assert(infos == null || infos.Length == 0,
                    "List of AccessPointInfo is not empty",
                    "Check that the DUT returned no AccessPointInfos");

                int maxLimit = GetMaxLimit();
                if (maxLimit >= 2)
                {
                    infos = GetAccessPointInfo(new string[] { fullList[0].token, token });

                    AccessPointInfo expected = fullList[0];

                    this.CheckRequestedInfo(infos, expected.token, D => D.token, "AccessPointInfo", Assert);
                }
                else
                {
                    LogTestEvent(string.Format("MaxLimit={0}, skip part with sending request with one correct and one incorrect tokens.", maxLimit) + Environment.NewLine);
                }
            });
        }
        
        [Test(Name = "GET ACCESS POINT INFO LIST - LIMIT",
            Order = "02.01.04",
            Id = "2-1-4",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_ACCESSPOINT,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessPointInfoList })]
        public void GetAccessPointInfoListLimitTest()
        {
            RunTest(
                () =>
                {
                    this.CommonGetListLimitTestBody<AccessPointInfo>(
                        GetMaxLimit,
                        GetFullAccessPointInfoList,
                        GetAccessPointInfoList,
                        AP => AP.token,
                        "Access Point",
                        Assert);
                });
        }

        [Test(Name = "GET ACCESS POINT INFO LIST - START REFERENCE AND LIMIT",
            Order = "02.01.05",
            Id = "2-1-5",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_ACCESSPOINT,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessPointInfoList })]
        public void GetAccessPointInfoListOffsetLimitTest()
        {
            RunTest(
                () =>
                {
                    this.CommonGetListStartReferenceLimitTestBody<AccessPointInfo>(
                        GetMaxLimit,
                        GetAccessPointInfoList,
                        AP => AP.token,
                        CompareAccessPoints, "Access Point",
                        RunStep, Assert);
                });
        }
                
        [Test(Name = "ENABLE/DISABLE ACCESS POINT",
            Order = "02.01.08",
            Id = "2-1-8",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_ACCESSPOINT,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.EnableAccessPoint, Functionality.DisableAccessPoint })]
        public void EnableDisableAccessPointTest()
        {
            RunTest(()=>
            {
                List<AccessPointInfo> aps = GetFullAccessPointInfoList();
                
                Action<string> disableAccessPoint = 
                    new Action<string>(
                        (t) =>
                            {
                                DisableAccessPoint(t);

                                AccessPointState actual = GetAccessPointState(t);
                               
                                Assert(actual.Enabled == false, 
                                    "AccessPoint is enabled", 
                                    string.Format("Check that AccessPoint '{0}' is disabled", t));
                            });

                foreach (AccessPointInfo info in aps)
                {
                    Assert(info.Capabilities != null, "Capabilities field is null",
                           string.Format("Check that capabilities are present for AccessPoint '{0}'", info.token));

                    if (info.Capabilities.DisableAccessPoint)
                    {
                        string token = info.token;

                        AccessPointState state = GetAccessPointState(token);

                        bool enabled = state.Enabled;

                        bool initiallyEnabled = enabled;

                        if (initiallyEnabled)
                        {
                            disableAccessPoint(token);
                        }

                        EnableAccessPoint(token);

                        AccessPointState actualState = GetAccessPointState(token);

                        Assert(actualState.Enabled,
                            "AccessPoint is disabled",
                            string.Format("Check that AccessPoint '{0}' is enabled", token));
                        
                        if (!initiallyEnabled)
                        {
                            disableAccessPoint(token);
                        }
                    }
                }
            });
        }
        

        [Test(Name = "ENABLE/DISABLE ACCESS POINT - COMMAND NOT SUPPORTED",
            Order = "02.01.09",
            Id = "2-1-9",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_ACCESSPOINT,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.EnableAccessPoint, Functionality.DisableAccessPoint})]
        public void EnableDisableAccessPointCommandNotSupportedTest()
        {
            RunTest(() =>
            {
                List<AccessPointInfo> aps = GetFullAccessPointInfoList();
                
                foreach (AccessPointInfo info in aps)
                {
                    Assert(info.Capabilities != null, "Capabilities field is null",
                           string.Format("Check that capabilities are present for AccessPoint '{0}'", info.token));

                    if (!info.Capabilities.DisableAccessPoint)
                    {
                        string token = info.token;

                        RunStep(
                            () => { Client.EnableAccessPoint(token); },
                            string.Format("Enable AccessPoint with token='{0}'", token),
                            CheckNotSupportedFault);
                   
                        RunStep(
                            () => { Client.DisableAccessPoint(token); },
                            string.Format("Disable AccessPoint with token='{0}'", token),
                            CheckNotSupportedFault);
                    }
                    else
                    {
                        LogTestEvent("DisableAccessPoint capability supported, skip this AccessPoint" + Environment.NewLine);
                    }
                }
            });
        }


        [Test(Name = "ENABLE ACCESS POINT WITH INVALID TOKEN",
            Order = "02.01.10",
            Id = "2-1-10",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_ACCESSPOINT,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.EnableAccessPoint })]
        public void EnableAccessPointInvalidTokenTest()
        {
            RunTest(() =>
            {
                string token = Guid.NewGuid().ToString().Substring(0, 8);
                RunStep(() => { Client.EnableAccessPoint(token); },
                    string.Format("Enable AccessPoint with token='{0}'", token), OnvifFaults.NotFound, true, false);
            
            });
        }

        [Test(Name = "DISABLE ACCESS POINT WITH INVALID TOKEN",
            Order = "02.01.11",
            Id = "2-1-11",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_ACCESSPOINT,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.DisableAccessPoint })]
        public void DisableAccessPointInvalidTokenTest()
        {
            RunTest(() =>
            {
                string token = Guid.NewGuid().ToString().Substring(0, 8);
                RunStep(() => { Client.DisableAccessPoint(token); },
                    string.Format("Disable AccessPoint with token='{0}'", token), OnvifFaults.NotFound, true, false);

            });
        }


        [Test(Name = "GET ACCESS POINT STATE",
            Order = "02.01.12",
            Id = "2-1-12",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_ACCESSPOINT,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessPointState})]
        public void GetAccessPointStateTest()
        {
            RunTest(() =>
            {
                List<AccessPointInfo> aps = GetFullAccessPointInfoList();

                foreach (AccessPointInfo info in aps)
                {
                    AccessPointState state = GetAccessPointState(info.token);

                    if (info.Capabilities != null && info.Capabilities.DisableAccessPoint == false)
                    {
                        Assert(state.Enabled, 
                            "State.Enabled is false", 
                            "Check that State.Enabled is true when DisableAccessPoint capability is not supported");
                    
                    }
                }
            });
        }

        [Test(Name = "GET ACCESS POINT STATE WITH INVALID TOKEN",
            Order = "02.01.13",
            Id = "2-1-13",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_ACCESSPOINT,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessPointState })]
        public void GetAccessPointStateInvalidTokenTest()
        {
            RunTest(() =>
            {
                this.InvalidTokenTestBody<AccessPointState>(Client.GetAccessPointState, RunStep, "AccessPointState", OnvifFaults.NotFound);
            });
        }

        [Test(Name = "GET ACCESS POINT INFO - TOO MANY ITEMS",
            Order = "02.01.15",
            Id = "2-1-15",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_ACCESSPOINT,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessPointInfo})]
        public void GetAccessPointInfoListTooManyItemsTest()
        {
            RunTest(
                () =>
                {
                    List<AccessPointInfo> fullList = GetFullAccessPointInfoList();

                    int maxLimit = GetMaxLimit();
                    
                    if (fullList.Count > maxLimit)
                    {
                        List<string> tokens = SelectTokens(fullList, A => A.token, maxLimit + 1);

                        RunStep(() => { Client.GetAccessPointInfo(tokens.ToArray()); },
                            "Get AccessPointInfo - too many tokens",
                            "Sender/InvalidArgs/TooManyItems");
                    }
                });
        }


        [Test(Name = "GET ACCESS POINT INFO LIST - NO LIMIT",
            Order = "02.01.14",
            Id = "2-1-14",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_ACCESSPOINT,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAccessPointInfoList})]
        public void GetAccessPointInfoListNoLimitTest()
        {
            RunTest(
                () =>
                {
                    this.CommonGetListNoLimitTestBody<AccessPointInfo>(
                            GetMaxLimit,
                            GetAccessPointInfoList,
                            AP => AP.token,
                            "Access Point",
                            Assert);

                });
        }

        /***************************************************************************/
        /*                                  AREA                                   */
        /***************************************************************************/

        [Test(Name = "GET AREA INFO",
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_AREA,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAreaInfo })]
        public void GetAreaInfoListTest()
        {
            CommonGetListTest<AreaInfo>(GetFullAreaInfoList, GetAreaInfo, 
                CompareLists, AI => AI.token, "Area", "AreaInfo");
        }

        [Test(Name = "GET AREA INFO WITH INVALID TOKEN",
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_AREA,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAreaInfo })]
        public void GetAreaInfoInfoListInvalidTokenTest()
        {
            RunTest(() =>
            {
                List<AreaInfo> fullList = GetFullAreaInfoList(); 


                string token = Guid.NewGuid().ToString().Substring(0, 8);
                AreaInfo[] infos = null;
                RunStep(() => { infos = Client.GetAreaInfo(new string[] { token }); }, "Get AreaInfo with invalid token");

                Assert(infos == null || infos.Length == 0,
                    "List of AreaInfo is not empty",
                    "Check that the DUT returned no AreaInfos");

                if (fullList == null || fullList.Count == 0)
                {
                    LogTestEvent("No Areas found, unable to send request with one correct and one incorrect tokens." + Environment.NewLine);
                }
                else
                {
                    int maxLimit = GetMaxLimit();
                    if (maxLimit >= 2)
                    {
                        infos = GetAreaInfo(new string[] { fullList[0].token, token });

                        AreaInfo expected = fullList[0];

                        this.CheckRequestedInfo(infos, expected.token, D => D.token, "AreaInfo", Assert);
                    }
                    else
                    {
                        LogTestEvent(string.Format("MaxLimit={0}, skip part with sending request with one correct and one incorrect tokens.", maxLimit) + Environment.NewLine);
                    }
                }
            });
        }

        [Test(Name = "GET AREA INFO LIST - LIMIT",
            Order = "03.01.04",
            Id = "3-1-4",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_AREA,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAreaInfoList })]
        public void GetAreaInfoListLimitTest()
        {
            RunTest(
                () =>
                {
                    this.CommonGetListLimitTestBody<AreaInfo>(
                        GetMaxLimit,
                        GetFullAreaInfoList,
                        GetAreaInfoList,
                        A => A.token,
                        "Area",
                        Assert);
                });
        }


        [Test(Name = "GET AREA INFO LIST - START REFERENCE AND LIMIT",
            Order = "03.01.05",
            Id = "3-1-5",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_AREA,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAreaInfoList })]
        public void GetAreaInfoListOffsetLimitTest()
        {
            RunTest(
                () =>
                {
                    this.CommonGetListStartReferenceLimitTestBody<AreaInfo>(
                        GetMaxLimit,
                        GetAreaInfoList,
                        A => A.token,
                        CompareAreaInfo,
                        "Area",
                        RunStep, Assert);
                });
        }

        [Test(Name = "GET AREA INFO LIST - NO LIMIT",
            Order = "03.01.10",
            Id = "3-1-10",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_AREA,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAreaInfoList })]
        public void GetAreaInfoListNoLimitTest()
        {
            RunTest(
                () =>
                {
                    this.CommonGetListNoLimitTestBody<AreaInfo>(
                            GetMaxLimit,
                            GetAreaInfoList,
                            A => A.token,
                            "Area",
                            Assert);

                });
        }

        [Test(Name = "GET AREA INFO - TOO MANY ITEMS",
            Order = "03.01.09",
            Id = "3-1-9",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_AREA,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAreaInfo })]
        public void GetAreaInfoTooManyItemsTest()
        {
            RunTest(
                () =>
                {
                    int maxLimit = GetMaxLimit();

                    List<AreaInfo> fullList = GetFullAreaInfoList();

                    if (fullList.Count > maxLimit)
                    {
                        List<string> tokens = SelectTokens(fullList, A => A.token, maxLimit + 1);

                        RunStep(() => { Client.GetAreaInfo(tokens.ToArray()); },
                            "Get AreaInfo - too many tokens",
                            "Sender/InvalidArgs/TooManyItems");
                    }
                });
        }

        List<string> SelectTokens<T>(List<T> list, Func<T, string> tokenSelector, int count)
        {
            List<T> listCopy = new List<T>(list);
            List<string> tokens = new List<string>();

            for (int i = 0; i <= count; i++)
            {
                Random rnd = new Random();
                int idx = rnd.Next(0, listCopy.Count - 1);
                tokens.Add(tokenSelector(listCopy[idx]));
                listCopy.RemoveAt(idx);
                if (listCopy.Count == 0)
                {
                    break;
                }
            }
            return tokens;
        }

        /***************************************************************************/

        [Test(Name = "GET AREA INFO LIST AND GET ACCESS POINT INFO LIST CONSISTENCY",
            Order = "04.01.01",
            Id = "4-1-1",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_CONSISTENCY,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAreaInfoList, Functionality.GetAccessPointInfoList})]
        public void GetAccessPointInfoAndAreaInfoListConsistencyTest()
        {
            RunTest(
                () =>
                    {
                        List<AccessPointInfo> aps = GetFullAccessPointInfoList();

                        List<AreaInfo> areas = GetFullAreaInfoList();

                        //ValidateAccessPointInfoList(aps);

                        List<string> areasMentioned = new List<string>();
                        {
                            foreach (AccessPointInfo ap in aps)
                            {
                                if (!string.IsNullOrEmpty(ap.AreaFrom) && !areasMentioned.Contains(ap.AreaFrom))
                                {
                                    areasMentioned.Add(ap.AreaFrom);
                                }
                                if (!string.IsNullOrEmpty(ap.AreaTo) && !areasMentioned.Contains(ap.AreaTo))
                                {
                                    areasMentioned.Add(ap.AreaTo);
                                }
                            }
                        }

                        StringBuilder dump = new StringBuilder();
                        List<string> invalidTokens = new List<string>();
                        bool ok = true;

                        foreach (string area in areasMentioned)
                        {
                            if (!invalidTokens.Contains(area))
                            {
                                if (areas.FirstOrDefault(A => A.token == area) == null)
                                {
                                    ok = false;
                                    dump.AppendFormat("Area with token '{0}' not found {1}", area,
                                                      Environment.NewLine);
                                    invalidTokens.Add(area);
                                }
                            }

                        }

                        Assert(ok, dump.ToStringTrimNewLine(),
                               "Check AreaFrom and AreaTo fields for each AccessPointInfo");
                    }
                );

        }

        [Test(Name = "GET ACCESS POINT INFO LIST AND SERVICE CAPABILITIES CONSISTENCY",
            Order = "04.01.02",
            Id = "4-1-2",
            Category = Category.ACCESSCONTROL,
            Path = PATH_ACCESSCONTROL_CONSISTENCY,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { })]
        public void GetAccessPointInfoAndServiceCapabilitiesConsistencyTest()
        {
            RunTest(
                () =>
                {
                    List<AccessPointInfo> aps = GetFullAccessPointInfoList();

                    AccessControlServiceCapabilities capabilities = GetServiceCapabilities();

                    //Assert(capabilities.)
                }
                );
        }

        #region Common tests
        
        /// <summary>
        /// Common "GET XXX INFO" test
        /// </summary>
        /// <typeparam name="T">Type of XXX</typeparam>
        /// <param name="getFullList"></param>
        /// <param name="getInfo">Command under test</param>
        /// <param name="compareLists">Function to compare two lists of XXX</param>
        /// <param name="tokenSelector">Function to retrieve token from XXX</param>
        /// <param name="itemName">Name of XXX</param>
        /// <param name="itemInfoName">Name of structure holding XXX info</param>
        void CommonGetListTest<T>(
            Func<List<T>> getFullList,
            Func<string[], T[]> getInfo, 
            Action<IEnumerable<T>, IEnumerable<T>, string, string> compareLists,
            Func<T, string>  tokenSelector,
            string itemName, 
            string itemInfoName)
        {
            RunTest(() =>
            {
                this.CommonGetListByTokenListTestBody(GetMaxLimit, getFullList, getInfo, 
                    tokenSelector, compareLists,  
                    itemName, itemInfoName, 
                    RunStep, Assert);
                
            });
        }


        void CommonGetListDuplicatedTokenTest<T>(Func<string[], T[]> getList,
            Func<T, string> tokenSelector)
        {
            RunTest(() =>
            {
                // Get valid tokens
                T[] infos = getList(null);

                if (infos == null)
                {
                    return;
                }

                string token = tokenSelector(infos[0]);
                T[] infosList = getList(new string[] { token, token });

            });
        }

        #endregion
    }
}
