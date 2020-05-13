using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.TestSuites.PACS;
using TestTool.Tests.Common.TestBase;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public partial class DoorControlServiceTestSuite : DoorControlServiceTest
    {
        public DoorControlServiceTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        private const string PATH_DOORCONTROLGENERAL = "Door Control\\General";
        private const string PATH_DOORCONTROL = "Door Control\\Door Control";
        private const string PATH_CONSISTENCY = "Door Control\\Consistency";

        private const string ACNAMESPACE = OnvifService.ACCESSCONTROL;

        [Test(Name = "GET DOOR STATE",
            Order = "02.01.01",
            Id = "2-1-1",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROLGENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDoorState})]
        public void GetDoorStateTest()
        {
            RunTest(() =>
            {
                // Get valid tokens
                List<DoorInfo> infos = GetFullDoorInfoList();

                if (infos == null)
                {
                    // should be inaccessible 
                    return;
                }
                foreach (DoorInfo info in infos)
                {
                    Assert(info.Capabilities != null,
                        "Capabilities are missing in DoorInfo", 
                        "Check that Capabilities element is present");

                    string token = info.token;
                    DoorState state = GetDoorState(token);
                    ValidateDoorCapabilitiesAndState(info, state);
                }                
            });
        }

        [Test(Name = "GET DOOR STATE WITH INVALID TOKEN",
            Order = "02.01.02",
            Id = "2-1-2",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROLGENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDoorState })]
        public void GetDoorStateInvalidTokenTest()
        {
            RunTest(() =>
            {
                string token = Guid.NewGuid().ToString().Substring(0, 8);
                RunStep(() => { DoorState state = Client.GetDoorState(token); }, "Get DoorState with invalid token", OnvifFaults.NotFound, true, false);
            });
        }
        
        [Test(Name = "GET DOOR INFO",
            Order = "02.01.03",
            Id = "2-1-3",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROLGENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDoorInfo })]
        public void GetDoorInfoListTokensListTest()
        {
            RunTest(() =>
            {
                this.CommonGetListByTokenListTestBody<DoorInfo>(GetMaxLimit, GetFullDoorInfoList, GetDoorInfo,
                    D => D.token, CompareLists, "Door", "DoorInfo", RunStep, Assert);
            });
        }
        

        [Test(Name = "GET DOOR INFO WITH INVALID TOKEN",
            Order = "02.01.04",
            Id = "2-1-4",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROLGENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDoorInfo })]
        public void GetDoorInfoListInvalidTokenTest()
        {
            RunTest(() =>
            {
                List<DoorInfo> fullList = GetFullDoorInfoList();

                if (fullList == null || fullList.Count == 0)
                {
                    return;
                }

                string token = Guid.NewGuid().ToString().Substring(0, 8);
                DoorInfo[] infos = null;
                RunStep(() => { infos = Client.GetDoorInfo(new string[] { token }); }, "Get DoorInfo with invalid token");

                Assert(infos == null || infos.Length == 0,
                    "List of DoorInfo is not empty",
                    "Check that the DUT returned no DoorInfos");

                int maxLimit = GetMaxLimit();
                if (maxLimit >= 2)
                {
                    infos = GetDoorInfo(new string[] { fullList[0].token, token });

                    DoorInfo expected = fullList[0];

                    this.CheckRequestedInfo(infos, expected.token, D => D.token, "DoorInfo", Assert);
                }
                else
                {
                    LogTestEvent(string.Format("MaxLimit={0}, skip part with sending request with one correct and one incorrect tokens.", maxLimit) + Environment.NewLine);
                }
            });
        }
        
        [Test(Name = "GET DOOR INFO LIST - LIMIT",
            Order = "02.01.06",
            Id = "2-1-6",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROLGENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDoorInfoList })]
        public void GetDoorInfoListLimitTest()
        {
            RunTest(() =>
            {
                this.CommonGetListLimitTestBody<DoorInfo>(
                    GetMaxLimit,
                    GetFullDoorInfoList,
                    GetDoorInfoList,
                    D => D.token,
                    "DoorInfo",
                    Assert);

            });
        }

        [Test(Name = "GET DOOR INFO LIST - NO LIMIT",
            Order = "02.01.08",
            Id = "2-1-8",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROLGENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDoorInfoList })]
        public void GetDoorInfoListNoLimitTest()
        {
            RunTest(() =>
            {
                int maxLimit = GetMaxLimit();

                List<DoorInfo> fullList = new List<DoorInfo>();

                List<string> gotByThisMoment = new List<string>();
                string offset = null;
                while (true)
                {
                    DoorInfo[] nextPart = null;
                    offset = GetDoorInfoList(null, offset, out nextPart, string.Format("Get DoorInfo list without limit and start reference ='{0}'", offset));

                    int count = nextPart == null ? 0 : nextPart.Length;

                    Assert(count <= maxLimit,
                           string.Format("{0} DoorInfos returned when limit is {1}", count, maxLimit),
                           "Check that MaxLimit is not exceeded",
                           string.Empty);

                    if (count > 0)
                    {
                        List<string> newTokens = nextPart.Select(D => D.token).ToList();

                        this.ValidateNextPart(gotByThisMoment, newTokens, "DoorInfo", Assert);

                        gotByThisMoment.AddRange(newTokens);

                        fullList.AddRange(nextPart);
                    }

                    if (string.IsNullOrEmpty(offset))
                    {
                        break;
                    }
                }

            });
        }

        [Test(Name = "GET DOOR INFO LIST - TOO MANY ITEMS",
            Order = "02.01.10",
            Id = "2-1-10",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROLGENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDoorInfoList })]
        public void GetDoorInfoTooManyItemsTest()
        {
            RunTest(() =>
                    {
                        var fullList = GetFullDoorInfoList();

                        int maxLimit = GetMaxLimit();

                        if (fullList.Count() <= maxLimit)
                            return;

                        try
                        {
                            GetDoorInfo(fullList.Select(e => e.token).ToArray());
                        }
                        catch (FaultException e)
                        {
                            if (e.IsValidOnvifFault("Sender/InvalidArgs/TooManyItems"))
                                StepPassed();
                            else
                                throw;

                            return;
                        }

                        Assert(false, "The DUT didn't send SOAP 1.2 fault message to GetDoorInfo request");
                    });
        }

        [Test(Name = "GET DOOR INFO LIST – START REFERENCE AND LIMIT",
            Order = "02.01.07",
            Id = "2-1-7",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROLGENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDoorInfoList  })]
        public void GetFullDoorInfoListByLimitTest()
        {
            RunTest(() =>
            {
                this.CommonGetListStartReferenceLimitTestBody<DoorInfo>(
                    GetMaxLimit,
                    GetDoorInfoList,
                    D => D.token,
                    CompareDoorInfo,
                    "DoorInfo",
                    RunStep, Assert);

            });
        }
               
        [Test(Name = "GET ACCESS POINT INFO LIST AND GET DOOR INFO LIST CONSISTENCY",
            Order = "05.01.01",
            Id = "5-1-1",
            Category = Category.DOORCONTROL,
            Path = PATH_CONSISTENCY,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService, Feature.AccessControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDoorInfoList, Functionality.GetAccessPointInfoList})]
        public void AccessPointAndDoorInfoConsistencyTest()
        {
            RunTest(
                () =>
                    {
                        List<DoorInfo> doors = GetFullDoorInfoList();

                        // initialize PACSPortClient
                        Proxies.Onvif.PACSPortClient client = PACSPortClient;

                        List<AccessPointInfo> accessPoints = GetAccessPointInfoList();

                        BeginStep("Validate response received");

                        List<string> doorTokens = new List<string>();

                        foreach (AccessPointInfo info in accessPoints)
                        {
                            bool door;
                            // empty type is OK
                            if (info.EntityType == null)
                            {
                                door = true;
                            }
                            else
                            {
                                XmlQualifiedName type = info.EntityType;
                                door = (type.Namespace == OnvifService.DOORCONTROL && type.Name == "Door");
                            }
                            // it's Door
                            if (door)
                            {
                                if (!doorTokens.Contains(info.Entity))
                                {
                                    doorTokens.Add(info.Entity);
                                }
                            }
                        }

                        StepPassed();
                        
                        bool ok = true;
                        StringBuilder logger = new StringBuilder();
                        foreach (string token in doorTokens)
                        {
                            DoorInfo door = doors.Where(D => D.token == token).FirstOrDefault();
                            if (door == null)
                            {
                                ok = false;
                                logger.AppendFormat("Door with token '{0}' not found{1}", token, Environment.NewLine);
                            }
                        }

                        Assert(ok, logger.ToStringTrimNewLine(), "Check that information is consistent");
                    });

        }


        /****************************************************************/
        /*                                                              */
        /*                      DOOR OPERATIONS                         */
        /*                                                              */
        /****************************************************************/
        #region DOOR OPERATIONS - POSITIVE
        
        /*

        [Test(Name = "ACCESS DOOR",
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { })]
        public void AccessDoorTest()
        {
            GeneralDoorOperationTest(new Action<string>(s => AccessDoor(s, null, null, null, null, null)),
                                     DC => DC.Access);
        }


        [Test(Name = "BLOCK DOOR",
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { })]
        public void BlockDoorTest()
        {
            GeneralDoorOperationTest(BlockDoor, DC=>DC.Block);
        }


        [Test(Name = "DOUBLE LOCK DOOR",
            Order = "03.01.03",
            Id = "3-1-3",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { })]
        public void DoubleLockDoorTest()
        {
            GeneralDoorOperationTest(DoubleLockDoor, DC=>DC.DoubleLock);
        }
        

        [Test(Name = "LOCK DOOR",
            Order = "03.01.04",
            Id = "3-1-4",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { })]
        public void LockDoorTest()
        {
            GeneralDoorOperationTest(LockDoor, DC => DC.Lock);
        }
        

        [Test(Name = "UNLOCK DOOR",
            Order = "03.01.05",
            Id = "3-1-5",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { })]
        public void UnlockDoorTest()
        {
            GeneralDoorOperationTest(UnlockDoor, DC=>DC.Unlock);
        }

        [Test(Name = "LOCK DOWN DOOR",
            Order = "03.01.06",
            Id = "3-1-6",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { })]
        public void LockDownDoorTest()
        {
            GeneralDoorOperationTest(LockDownDoor, DC=>DC.LockDown);
        }

        [Test(Name = "LOCK DOWN RELEASE DOOR",
            Order = "03.01.07",
            Id = "3-1-7",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { })]
        public void LockDownReleaseDoorTest()
        {
            GeneralDoorOperationTest(LockDownReleaseDoor, DC=>DC.LockDown);
        }
        
        [Test(Name = "LOCK OPEN DOOR",
            Order = "03.01.08",
            Id = "3-1-8",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { })]
        public void LockOpenDoorTest()
        {
            GeneralDoorOperationTest(LockOpenDoor, DC=>DC.LockOpen);
        }
        
        [Test(Name = "LOCK OPEN RELEASE DOOR",
            Order = "03.01.09",
            Id = "3-1-9",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { })]
        public void LockOpenReleaseDoorTest()
        {
            GeneralDoorOperationTest(LockOpenReleaseDoor, DC=>DC.LockOpen);
        }
        */

        #endregion
        
        /***********************************************************************************/

        #region COMMAND NOT SUPPORTED 


        [Test(Name = "ACCESS DOOR – COMMAND NOT SUPPORTED",
            Order = "03.01.19",
            Id = "3-1-19",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.AccessDoor })]
        public void AccessDoorCommandNotSupportedTest()
        {
            RunTest(() =>
            {
                GeneralDoorOperationCommandNotSupportedTest(
                    (token) =>
                        {
                            Client.AccessDoor(token, null, null, null, null, null);
                        },
                    DC => DC.Access,
                    "Access Door (token={0})");
            });


        }

        [Test(Name = "BLOCK DOOR – COMMAND NOT SUPPORTED",
            Order = "03.01.20",
            Id = "3-1-20",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.BlockDoor })]
        public void BlockDoorCommandNotSupportedTest()
        {
            RunTest(() =>
            {
                GeneralDoorOperationCommandNotSupportedTest(Client.BlockDoor, DC => DC.Block, BLOCKDOORSTEPNAMEPATTERN);
            });
        }


        [Test(Name = "DOUBLE LOCK DOOR – COMMAND NOT SUPPORTED",
            Order = "03.01.21",
            Id = "3-1-21",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.DoubleLockDoor })]
        public void DoubleLockDoorCommandNotSupportedTest()
        {
            RunTest(() =>
            {
                GeneralDoorOperationCommandNotSupportedTest(Client.DoubleLockDoor, DC => DC.DoubleLock, DOUBLELOCKDOORSTEPNAMEPATTERN);
            });
        }


        [Test(Name = "LOCK DOOR – COMMAND NOT SUPPORTED",
            Order = "03.01.22",
            Id = "3-1-22",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDoor})]
        public void LockDoorCommandNotSupportedTest()
        {
            RunTest(() =>
            {
                GeneralDoorOperationCommandNotSupportedTest(Client.LockDoor, DC => DC.Lock, LOCKDOORSTEPNAMEPATTERN);
            });
        }


        [Test(Name = "UNLOCK DOOR – COMMAND NOT SUPPORTED",
            Order = "03.01.23",
            Id = "3-1-23",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.UnlockDoor})]
        public void UnlockDoorCommandNotSupportedTest()
        {
            RunTest(() =>
            {
                GeneralDoorOperationCommandNotSupportedTest(Client.UnlockDoor, DC => DC.Unlock, UNLOCKDOORSTEPNAMEPATTERN);
            });
        }

        [Test(Name = "LOCK DOWN DOOR – COMMAND NOT SUPPORTED",
            Order = "03.01.24",
            Id = "3-1-24",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDownDoor })]
        public void LockDownDoorCommandNotSupportedTest()
        {
            RunTest(() =>
            {
                GeneralDoorOperationCommandNotSupportedTest(Client.LockDownDoor, DC => DC.LockDown, LOCKDOWNDOORSTEPNAMEPATTERN);
            });
        }


        [Test(Name = "LOCK DOWN RELEASE DOOR – COMMAND NOT SUPPORTED",
            Order = "03.01.25",
            Id = "3-1-25",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDownReleaseDoor })]
        public void LockDownReleaseDoorCommandNotSupportedTest()
        {
            RunTest(() =>
            {
                GeneralDoorOperationCommandNotSupportedTest(Client.LockDownReleaseDoor, DC => DC.LockDown, LOCKDOWNRELEASEDOORSTEPNAMEPATTERN);
            });
        }


        [Test(Name = "LOCK OPEN DOOR – COMMAND NOT SUPPORTED",
            Order = "03.01.26",
            Id = "3-1-26",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockOpenDoor })]
        public void LockOpenDoorCommandNotSupportedTest()
        {
            RunTest(() =>
            {
                GeneralDoorOperationCommandNotSupportedTest(Client.LockOpenDoor, DC => DC.LockOpen, LOCKOPENDOORSTEPNAMEPATTERN);
            });
        }


        [Test(Name = "LOCK OPEN RELEASE DOOR – COMMAND NOT SUPPORTED",
            Order = "03.01.27",
            Id = "3-1-27",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockOpenReleaseDoor })]
        public void LockOpenReleaseDoorCommandNotSupportedTest()
        {
            RunTest(() =>
            {
                GeneralDoorOperationCommandNotSupportedTest(Client.LockOpenReleaseDoor, DC => DC.LockOpen, LOCKOPENRELEASEDOORSTEPNAMEPATTERN);
            });
        }
        
        #endregion

        /***********************************************************************************/

        #region INVALID TOKEN

        [Test(Name = "ACCESS DOOR WITH INVALID TOKEN",
            Order = "03.01.10",
            Id = "3-1-10",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.AccessDoor })]
        public void AccessDoorInvalidTokenTest()
        {
            RunTest(() =>
            {
                string token = Guid.NewGuid().ToString().Substring(0, 8);
                RunStep(() => Client.AccessDoor(token, null, null, null, null, null), 
                    string.Format("Access Door (token={0})", token), 
                    OnvifFaults.NotFound, 
                    true, 
                    false);
            }); 
        }


        [Test(Name = "BLOCK DOOR WITH INVALID TOKEN",
            Order = "03.01.11",
            Id = "3-1-11",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.BlockDoor})]
        public void BlockDoorInvalidTokenTest()
        {
            RunTest(() =>
                        {
                            GeneralDoorOperationInvalidTokenTest(Client.BlockDoor, 
                                BLOCKDOORSTEPNAMEPATTERN);
                        });
        }


        [Test(Name = "DOUBLE LOCK DOOR WITH INVALID TOKEN",
            Order = "03.01.12",
            Id = "3-1-12",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.DoubleLockDoor})]
        public void DoubleLockDoorInvalidTokenTest()
        {
            RunTest(() =>
                        {

                            GeneralDoorOperationInvalidTokenTest(Client.DoubleLockDoor,
                                DOUBLELOCKDOORSTEPNAMEPATTERN);
                        });
        }


        [Test(Name = "LOCK DOOR WITH INVALID TOKEN",
            Order = "03.01.13",
            Id = "3-1-13",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDoor })]
        public void LockDoorInvalidTokenTest()
        {
            RunTest(() =>
                        {

                            GeneralDoorOperationInvalidTokenTest(Client.LockDoor, 
                                LOCKDOORSTEPNAMEPATTERN);
                        });
        }


        [Test(Name = "UNLOCK DOOR WITH INVALID TOKEN",
            Order = "03.01.14",
            Id = "3-1-14",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.UnlockDoor})]
        public void UnlockDoorInvalidTokenTest()
        {
            RunTest(() =>
                        {
                            GeneralDoorOperationInvalidTokenTest(Client.UnlockDoor, 
                                UNLOCKDOORSTEPNAMEPATTERN);
                        });
        }

        [Test(Name = "LOCK DOWN DOOR WITH INVALID TOKEN",
            Order = "03.01.15",
            Id = "3-1-15",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[]  {Functionality.LockDownDoor })]
        public void LockDownDoorInvalidTokenTest()
        {
            RunTest(() =>
                        {
                            GeneralDoorOperationInvalidTokenTest(Client.LockDownDoor, 
                                LOCKDOWNDOORSTEPNAMEPATTERN);
                        });
        }


        [Test(Name = "LOCK DOWN RELEASE DOOR WITH INVALID TOKEN",
            Order = "03.01.16",
            Id = "3-1-16",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockDownReleaseDoor})]
        public void LockDownReleaseDoorInvalidTokenTest()
        {
            RunTest(() =>
                        {
                            GeneralDoorOperationInvalidTokenTest(Client.LockDownReleaseDoor,
                                                                 LOCKDOWNRELEASEDOORSTEPNAMEPATTERN);
                        });
        }


        [Test(Name = "LOCK OPEN DOOR WITH INVALID TOKEN",
            Order = "03.01.17",
            Id = "3-1-17",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockOpenDoor})]
        public void LockOpenDoorInvalidTokenTest()
        {
            RunTest(() =>
                        {
                            GeneralDoorOperationInvalidTokenTest(Client.LockOpenDoor, 
                                LOCKOPENDOORSTEPNAMEPATTERN);
                        });
        }


        [Test(Name = "LOCK OPEN RELEASE DOOR WITH INVALID TOKEN",
            Order = "03.01.18",
            Id = "3-1-18",
            Category = Category.DOORCONTROL,
            Path = PATH_DOORCONTROL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.DoorControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.LockOpenReleaseDoor })]
        public void LockOpenReleaseDoorInvalidTokenTest()
        {
            RunTest(() =>
                        {
                            GeneralDoorOperationInvalidTokenTest(Client.LockOpenReleaseDoor,
                                                                 LOCKOPENRELEASEDOORSTEPNAMEPATTERN);
                        });
        }

        #endregion



    }
}
