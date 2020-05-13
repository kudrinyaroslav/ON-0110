using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Proxies.WSDiscovery;
using TestTool.Tests.Common.Soap;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.Utils.Comparison;



namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public partial class ReceiverGeneralTestSuit : ReceiverTest
    {

        public ReceiverGeneralTestSuit(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH_GENERAL = "Receiver\\General";

        [Test(Name = "GET RECEIVERS",
            Order = "02.01.01",
            Id = "2-1-1",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetReceivers })]
        public void GetReceiversTest()
        {
            RunTest(() =>
            {
                Receiver[] receivers = GetReceivers();

                Assert(receivers != null,
                    "No receivers returned",
                        "Check that GetReceiversResponse has been obtained");
                ValidateFullReceiversList(receivers);
            });
        }

        [Test(Name = "GET RECEIVER",
            Order = "02.01.02",
            Id = "2-1-2",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetReceiver })]
        public void GetReceiverTest()
        {
            RunTest(() =>
            {
                Receiver[] receivers = GetReceivers();

                BeginStep("Check that receiver list is not empty");
                if (receivers == null || receivers.Length == 0)
                {
                    LogStepEvent("Receiver list is empty");
                    StepPassed();
                    return;
                }
                else
                {
                    StepPassed();
                }

                ValidateFullReceiversList(receivers);

                foreach (var item in receivers)
                {
                    var receiver = GetReceiver(item.Token);
                    Assert(receiver != null,
                        "Receiver '{0}' wasn't returned", "Check that receiver was returned");
                    CompareReceivers(item, receiver, Assert);
                }
            });
        }

        [Test(Name = "GET RECEIVER WITH INVALID TOKEN",
            Order = "02.01.03",
            Id = "2-1-3",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetReceiver })]
        public void ReceiverInvalidTokenTest()
        {
            RunTest(() =>
                {
                    var receivers = GetReceivers();
                    if (receivers == null || receivers.Length == 0)
                    {
                        var config = GetReceiverConfiguration();
                        CreateReceiver(config);
                    }
                    this.InvalidTokenTestBody<object>((s, T) => Client.GetReceiver(s), null,
                        RunStep, "Get Receiver with invalid token", null, OnvifFaults.UnknownToken);
                });
        }

        [Test(Name = "GET RECEIVER STATE",
            Order = "02.01.04",
            Id = "2-1-4",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetReceiverState })]
        public void GetReceiverStateTest()
        {
            RunTest(() =>
            {
                Receiver[] receivers = GetReceivers();

                BeginStep("Check that receiver list is not empty");
                if (receivers == null || receivers.Length == 0)
                {
                    LogStepEvent("Receiver list is empty");
                    StepPassed();
                    return;
                }
                else
                {
                    StepPassed();
                }

                ValidateFullReceiversList(receivers);

                foreach (var item in receivers)
                {
                    var receiverStateInfo = GetReceiverState(item.Token);
                    Assert(receiverStateInfo != null,
                        string.Format("Receiver '{0}' doesn't have ReceiverStateInformaion", item.Token),
                            string.Format("Check that receiver '{0}' have ReceiverStateInformaion", item.Token));
                }
            });
        }

        [Test(Name = "GET RECEIVER STATE WITH INVALID TOKEN",
            Order = "02.01.05",
            Id = "2-1-5",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetReceiverState })]
        public void ReceiverStateInvalidTokenTest()
        {
            RunTest(() =>
            {
                var receivers = GetReceivers();
                if (receivers == null || receivers.Length == 0)
                {
                    var config = GetReceiverConfiguration();
                    CreateReceiver(config);
                }
                this.InvalidTokenTestBody<object>((s, T) => Client.GetReceiverState(s), null,
                    RunStep, "Get Receiver State with invalid token", null, OnvifFaults.UnknownToken);
            });
        }

        [Test(Name = "CREATE RECEIVER",
                    Order = "02.01.06",
                    Id = "2-1-6",
                    Category = Category.RECEIVER,
                    Path = PATH_GENERAL,
                    Version = 1.0,
                    RequirementLevel = RequirementLevel.Must,
                    RequiredFeatures = new Feature[] { Feature.ReceiverService },
                    FunctionalityUnderTest = new Functionality[] { Functionality.CreateReceiver })]
        public void CreateReceiverTest()
        {
            Receiver receiverDeleted = null;
            Receiver receiver = null;
            RunTest(() =>
            {
                receiver = CreateReceiverWithValidation(false);
            }, () =>
            {
                if (receiver != null)
                    DeleteReceiver(receiver.Token);

            });
        }

        [Test(Name = "CREATE RECEIVER - PERSISTENCE",
            Order = "02.01.07",
            Id = "2-1-7",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateReceiver })]
        public void CreateReceiverPersistanceTest()
        {
            Receiver receiver = null;
            RunTest(() =>
            {
                receiver = CreateReceiverWithValidation(true);
            }, () =>
            {
                if (receiver != null)
                    DeleteReceiver(receiver.Token);
            });
        }

        [Test(Name = "CREATE RECEIVER - RECEIVERS MAX NUMBER",
            Order = "02.01.08",
            Id = "2-1-8",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateReceiver })]
        public void CreateReceiverMaxNumberTest()
        {
            List<string> tokens = new List<string>();
            RunTest(() =>
            {
                Receiver receiver = null;

                int receiverCountToCreate;
                Receiver[] receivers = GetReceiversCountToCreate(out receiverCountToCreate);

                var receiverConfiguration = GetReceiverConfiguration();

                for (int i = 1; i <= receiverCountToCreate; i++)
                {
                    receiver = CreateReceiver(receiverConfiguration);

                    Assert(receiver != null, "Receiver is not returned",
                            "Check that receiver was returned in CreateReceiverResponse");
                    tokens.Add(receiver.Token);

                    CheckReceiverConfiguration(receiver, receiverConfiguration, "CreateReceiverResponse");
                }

                RunStep(() => { receiver = Client.CreateReceiver(receiverConfiguration); },
                        "Create addition receiver", OnvifFaults.ReceiverMaxNumber, true, false);
            }, () =>
            {
                foreach (var item in tokens)
                {
                    DeleteReceiver(item);
                }
            });
        }

        [Test(Name = "DELETE RECEIVER",
                    Order = "02.01.09",
                    Id = "2-1-9",
                    Category = Category.RECEIVER,
                    Path = PATH_GENERAL,
                    Version = 1.0,
                    RequirementLevel = RequirementLevel.Must,
                    RequiredFeatures = new Feature[] { Feature.ReceiverService },
                    FunctionalityUnderTest = new Functionality[] { Functionality.DeleteReceiver })]
        public void DeleteReceiverTest()
        {
            Receiver receiver = null;
            ReceiverConfiguration config = null;
            bool isReceiverDeleted = false;
            bool isReceiverCreated = false;
            RunTest(() =>
            {
                ReceiverServiceCapabilities capabilities = GetServiceCapabilities();

                Assert(capabilities != null, "No capabilities returned", "Check that capabilities were returned");

                var receivers = GetReceivers();

                Assert(receivers != null, "No receivers returned", "Check that receivers list is not empty");

                int count = capabilities.SupportedReceivers - receivers.Length;

                if (count > 0)
                {
                    config = GetReceiverConfiguration();
                    receiver = CreateReceiver(config);
                    isReceiverCreated = true;
                    CheckReceiverConfiguration(receiver, config);
                }
                else
                {
                    receiver = receivers[0];
                }

                var token = receiver.Token;

                DeleteReceiver(token);
                isReceiverDeleted = true;

                receivers = GetReceivers();

                receiver = null;
                if (receivers != null)
                    receiver = receivers.FirstOrDefault(r => r.Token == token);

                Assert(receiver == null, "Receiver list contains receiver deleted", "Check that receiver list doesn't contain receiver deleted");

                this.InvalidTokenTestBody<object>((s, T) => Client.GetReceiver(s), null,
                        RunStep, "Try to get Receiver deleted", token, OnvifFaults.NotFound);
            }, () =>
            {
                if (!isReceiverDeleted && isReceiverCreated && receiver != null)
                {
                    DeleteReceiver(receiver.Token);
                }
            });
        }


        [Test(Name = "DELETE RECEIVER WITH INVALID TOKEN",
            Order = "02.01.10",
            Id = "2-1-10",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteReceiver })]
        public void DeleteReceiverInvalidTokenTest()
        {
            Receiver receiver = null;
            RunTest(() =>
            {
                Receiver[] receivers = null;
                receiver = CreateReceiverAnnexA2(out receivers);
                this.InvalidTokenTestBody<object>((s, T) => Client.DeleteReceiver(s), null,
                    RunStep, "Delete Receiver", null, OnvifFaults.UnknownToken);
            }, () =>
            {
                if (receiver != null)
                {
                    DeleteReceiver(receiver.Token);
                }
            });
        }



        [Test(Name = "SET RECEIVER MODE",
            Order = "02.01.11",
            Id = "2-1-11",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetReceiverMode })]
        public void SetReceiverModeTest()
        {
            ReceiverMode receiverMode = ReceiverMode.Unknown;
            bool receiverModeSpecified = false;
            string receiverToken = string.Empty;
            Receiver receiver = null;
            bool isReceiverCreated = false;
            RunTest(() =>
            {
                Receiver[] receivers = null;
                receiver = CreateReceiverAnnexA2(out receivers);
                if (receiver == null)
                {
                    receiver = receivers[0];
                }
                else
                {
                    isReceiverCreated = true;
                }

                receiverMode = receiver.Configuration.Mode;
                receiverModeSpecified = true;
                receiverToken = receiver.Token;

                ReceiverMode lastMode = ReceiverMode.Unknown;
                foreach (var mode in Enum.GetValues(typeof(ReceiverMode)))
                {
                    if ((lastMode = (ReceiverMode)mode) != ReceiverMode.Unknown)
                    {
                        SetReceiverMode(receiverToken, (ReceiverMode)mode);
                        ValidateReceiverChanging<ReceiverMode>(receiverToken, CheckReceiverMode, (ReceiverMode)mode);
                    }
                }
                receiverModeSpecified = !(lastMode == receiverMode);

            }, () =>
            {
                if (isReceiverCreated && receiver != null)
                {
                    DeleteReceiver(receiverToken);
                }
                else
                {
                    if (receiverModeSpecified)
                        SetReceiverMode(receiverToken, receiverMode);
                }
            });
        }

        [Test(Name = "SET RECEIVER MODE - PERSISTENCE",
            Order = "02.01.12",
            Id = "2-1-12",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetReceiverMode })]
        public void SetReceiverModePersistanceTest()
        {
            ReceiverMode receiverMode = ReceiverMode.Unknown;
            bool receiverModeSpecified = false;
            string receiverToken = string.Empty;
            Receiver receiver = null;
            bool isReceiverCreated = false;
            RunTest(() =>
            {
                Receiver[] receivers = null;
                receiver = CreateReceiverAnnexA2(out receivers);
                if (receiver == null)
                {
                    receiver = receivers[0];
                }
                else
                {
                    isReceiverCreated = true;
                }

                receiverMode = receiver.Configuration.Mode;

                receiverToken = receiver.Token;

                var mode = receiverMode == ReceiverMode.AutoConnect ?
                    ReceiverMode.NeverConnect : ReceiverMode.AutoConnect;

                SetReceiverMode(receiverToken, mode);
                receiverModeSpecified = true;
                //Reboot();
                SystemReboot();

                ValidateReceiverChanging<ReceiverMode>(receiverToken, CheckReceiverMode, mode);
            }, () =>
            {
                if (isReceiverCreated && receiver != null)
                {
                    DeleteReceiver(receiverToken);
                }
                else
                {
                    if (receiverModeSpecified)
                        SetReceiverMode(receiverToken, receiverMode);
                }
            });
        }

        [Test(Name = "SET RECEIVER MODE WITH INVALID TOKEN",
            Order = "02.01.13",
            Id = "2-1-13",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetReceiverMode })]
        public void SetReceiverModeInvalidTokenTest()
        {
            Receiver receiver = null;
            bool isReceiverCreated = false;
            RunTest(() =>
            {
                Receiver[] receivers = null;
                receiver = CreateReceiverAnnexA2(out receivers);
                if (receiver == null)
                {
                    receiver = receivers[0];
                }
                else
                {
                    isReceiverCreated = true;
                }

                this.InvalidTokenTestBody<ReceiverMode>((s, T) => Client.SetReceiverMode(s, T), ReceiverMode.AutoConnect,
                    RunStep, "Set Receiver Mode with invalid token", null, OnvifFaults.UnknownToken);
            }, () =>
            {
                if (isReceiverCreated && receiver != null)
                {
                    DeleteReceiver(receiver.Token);
                }
            });
        }

        [Test(Name = "CONFIGURE RECEIVER",
            Order = "02.01.20",
            Id = "2-1-20",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.ConfigureReceiver })]
        public void ConfigureReceiverTest()
        {
            Receiver receiver = null;
            bool isReceiverCreated = false;
            RunTest(() =>
                    {
                        receiver = ConfigureReceiver(false, out isReceiverCreated);
                    }, 
                    () =>
                    {
                        if (isReceiverCreated && receiver != null)
                        {
                            DeleteReceiver(receiver.Token);
                        }
                        else if (!isReceiverCreated && receiver != null)
                        {
                            ConfigureReceiver(receiver.Token, receiver.Configuration);
                        }
                    });
        }

        [Test(Name = "CONFIGURE RECEIVER - PERSISTENCE",
            Order = "02.01.21",
            Id = "2-1-21",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.ConfigureReceiver })]
        public void ConfigureReceiverPersistanceTest()
        {
            Receiver receiver = null;
            bool isReceiverCreated = false;
            RunTest(() =>
                    {
                        receiver = ConfigureReceiver(true, out isReceiverCreated);
                    }, 
                    () =>
                    {
                        if (isReceiverCreated && receiver != null)
                        {
                            DeleteReceiver(receiver.Token);
                        }
                        else if (!isReceiverCreated && receiver != null)
                        {
                            ConfigureReceiver(receiver.Token, receiver.Configuration);
                        }
                    });
        }


        [Test(Name = "CONFIGURE RECEIVER WITH INVALID TOKEN",
            Order = "02.01.19",
            Id = "2-1-19",
            Category = Category.RECEIVER,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.ConfigureReceiver })]
        public void ConfigureReceiverModeInvalidTokenTest()
        {
            Receiver receiver = null;
            bool isReceiverCreated = false;
            RunTest(() =>
            {
                Receiver[] receivers = null;
                receiver = CreateReceiverAnnexA2(out receivers);
                if (receiver == null)
                {
                    receiver = receivers[0];
                }
                else
                {
                    isReceiverCreated = true;
                }

                this.InvalidTokenTestBody<ReceiverConfiguration>((s, T) => Client.ConfigureReceiver(s, T),
                    new ReceiverConfiguration(), RunStep, "Configure Receiver with invalid token", null, OnvifFaults.UnknownToken);
            }, () =>
            {
                if (isReceiverCreated && receiver != null)
                {
                    DeleteReceiver(receiver.Token);
                }
            });
        }
    }
}
