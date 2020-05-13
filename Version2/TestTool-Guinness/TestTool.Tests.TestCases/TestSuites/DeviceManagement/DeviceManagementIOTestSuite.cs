///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using System.Text;
using System.Linq;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class DeviceManagementIOTestSuite: Base.DeviceManagementTest
    {
        public DeviceManagementIOTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        private const string PATH = "Device Management\\I/O";

        [Test(Name = "IO COMMAND GETRELAYOUTPUTS",
            Order = "05.01.01",
            Id = "5-1-1",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DeviceIORelayOutputs },
            FunctionalityUnderTest =  new Functionality[]{Functionality.GetRelayOutputs})]
        public void GetRelayOutputsTest()
        {
            RunTest(() =>
            {
                RelayOutput[] outputs = GetRelayOutputs();

                Assert(outputs != null && outputs.Length > 0, "No relay outputs received via GetRelayOutputs",
                    "Check that the DUT sent relay outputs information");
                
                if (outputs != null)
                {
                    List<string> tokens = new List<string>();
                    List<string> duplicates = new List<string>();
                    foreach (RelayOutput output in outputs)
                    {
                        if (!tokens.Contains(output.token))
                        {
                            tokens.Add(output.token);
                        }
                        else
                        {
                            if (!duplicates.Contains(output.token))
                            {
                                duplicates.Add(output.token);
                            }
                        }
                    }

                    if (duplicates.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder("The following tokens are not unique: ");
                        bool first = true;
                        foreach (string token in duplicates)
                        {
                            sb.Append(first ? token : string.Format(", {0}", token));
                            first = false;
                        }

                        Assert(false, sb.ToString(), "Validate relay outputs");

                    }


                }
            });
        }

        [Test(Name = "RELAY OUTPUTS COUNT IN GETRELAYOUTPUTS AND GETCAPABILITIES",
            Order = "05.01.02",
            Id = "5-1-2",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest =  new Functionality[]{Functionality.GetRelayOutputs, Functionality.GetCapabilities},
            RequiredFeatures = new Feature[] { Feature.DeviceIORelayOutputs, Feature.GetCapabilities })]
        public void RelayOutputsCountTest()
        {
            RunTest( 
                () =>
                    {
                        Capabilities capabilities = GetCapabilities(new[] { CapabilityCategory.Device });

                        Assert(capabilities != null, "Сapabilities not found", "Check that DUT returned capabilities");

                        Assert(capabilities.Device != null, "Device capabilities not found", "Check that DUT returned device capabilities");

                        Assert(capabilities.Device.IO != null, "I/O capabilities not found", "Check that IO capabilities returned");

                        if (capabilities.Device.IO.RelayOutputsSpecified)
                        {

                            RelayOutput[] outputs = GetRelayOutputs();

                            Assert(outputs != null, "No relay outputs received via GetRelayOutputs",
                                   "Check that the DUT sent relay outputs information");

                            Assert(outputs.Length == capabilities.Device.IO.RelayOutputs,
                                   "Count of relay output actually received differs from count specified in GetCapabilities response",
                                   "Check that count of relay outputs is the same");
                        }
                        else
                        {
                            LogTestEvent("WARNING: relay outputs not specified in capabilities response" + System.Environment.NewLine);
                        }

                    });
        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSETTINGS",
            Order = "05.01.03",
            Id = "5-1-3",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DeviceIORelayOutputs },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRelayOutputs, Functionality.SetRelayOutputSettings })]
        public void SetRelayOutputTest()
        {
            RunTest(
                () =>
                    {
                        RelayOutput[] outputs = GetRelayOutputs();

                        Assert(outputs != null, "No relay outputs received via GetRelayOutputs", "Check that the DUT sent relay outputs information");

                        string testTime = string.Empty;

                        foreach (RelayOutput output in outputs)
                        {
                            testTime = string.Format("PT{0}S", _relayOutputDelayTimeBistable);

                            if (Features.Contains(Feature.DeviceIORelayOutputsBistableOpen))
                            {
                                ChangeRelayOutputProperties(output.token, testTime, 
                                    RelayIdleState.open, RelayMode.Bistable);
                            }

                            if (Features.Contains(Feature.DeviceIORelayOutputsBistableClosed))
                            {
                                ChangeRelayOutputProperties(output.token, testTime,
                                    RelayIdleState.closed, RelayMode.Bistable);
                            }

                            testTime = string.Format("PT{0}S", _relayOutputDelayTimeMonostable);

                            if (Features.Contains(Feature.DeviceIORelayOutputsMonostableOpen))
                            {
                                ChangeRelayOutputProperties(output.token, testTime,
                                    RelayIdleState.open , RelayMode.Monostable);
                            }

                            if (Features.Contains(Feature.DeviceIORelayOutputsMonostableClosed))
                            {
                                ChangeRelayOutputProperties(output.token, testTime, 
                                    RelayIdleState.closed, RelayMode.Monostable);
                            }
                        }

                    });

        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSETTINGS – INVALID TOKEN",
            Order = "05.01.04",
            Id = "5-1-4",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DeviceIORelayOutputs },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputSettings })]
        public void SetRelayOutputInvalidTokenTest()
        {
            RunTest(
                () =>
                {
                    RelayOutput[] outputs = GetRelayOutputs();

                    Assert(outputs != null, "No relay outputs received via GetRelayOutputs", "Check that the DUT sent relay outputs information");

                    List<string> tokens = new List<string>();

                    foreach (RelayOutput output in outputs)
                    {
                        tokens.Add(output.token);
                    }

                    string token = tokens.GetNonMatchingString();

                    RelayOutputSettings testSettings = new RelayOutputSettings();
                    testSettings.DelayTime =  string.Format("PT{0}S", _relayOutputDelayTimeBistable);
                    testSettings.IdleState = RelayIdleState.open;
                    testSettings.Mode = RelayMode.Bistable;

                    RunStep(
                        () => { Client.SetRelayOutputSettings(token, testSettings);},
                        "Set relay output settings - negative test", 
                        "Sender/InvalidArgVal/RelayToken", 
                        true);

                    DoRequestDelay();

                });

        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSTATE – BISTABLE MODE (OPENED IDLE STATE)",
            Order = "05.01.05",
            Id = "5-1-5",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DeviceIORelayOutputs, Feature.DeviceIORelayOutputsBistableOpen },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputState })]
        public void SetRelayOutputOpenIdleStateTest()
        {
            RelayOutputSettings initialSettings = null;
            string token = string.Empty;

            RunTest(
                () =>
                {
                    RelayOutput[] outputs = GetRelayOutputs();

                    Assert(outputs != null && outputs.Length > 0, "No relay outputs received via GetRelayOutputs", "Check that the DUT sent relay outputs information");

                    string testTime = string.Format("PT{0}S", _relayOutputDelayTimeBistable);

                    token = outputs[0].token;

                    RelayOutputSettings testSettings = new RelayOutputSettings();
                    testSettings.DelayTime = testTime;
                    testSettings.IdleState = RelayIdleState.open;
                    testSettings.Mode = RelayMode.Bistable;

                    SetRelayOutputSettings(token, testSettings);
                    initialSettings = outputs[0].Properties;

                    SetRelayOutputState(token, RelayLogicalState.active);
                    
                    SetRelayOutputState(token, RelayLogicalState.inactive);

                }, 
                ()=>
                    {
                        if (initialSettings != null)
                        {
                            SetRelayOutputSettings(token, initialSettings, "Restore output settings");
                        }
                    });
        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSTATE – BISTABLE MODE (CLOSED IDLE STATE)",
            Order = "05.01.06",
            Id = "5-1-6",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DeviceIORelayOutputs, Feature.DeviceIORelayOutputsBistableClosed },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputState})]
        public void SetRelayOutputClosedIdleStateTest()
        {
            RelayOutputSettings initialSettings = null;
            string token = string.Empty;

            RunTest(
                () =>
                {
                    RelayOutput[] outputs = GetRelayOutputs();

                    Assert(outputs != null && outputs.Length > 0, "No relay outputs received via GetRelayOutputs", "Check that the DUT sent relay outputs information");

                    string testTime = string.Format("PT{0}S", _relayOutputDelayTimeBistable);

                    token = outputs[0].token;

                    RelayOutputSettings testSettings = new RelayOutputSettings();
                    testSettings.DelayTime = testTime;
                    testSettings.IdleState = RelayIdleState.closed;
                    testSettings.Mode = RelayMode.Bistable;

                    SetRelayOutputSettings(token, testSettings);
                    initialSettings = outputs[0].Properties;

                    SetRelayOutputState(token, RelayLogicalState.active);

                    SetRelayOutputState(token, RelayLogicalState.inactive);

                },
                () =>
                {
                    if (initialSettings != null)
                    {
                        SetRelayOutputSettings(token, initialSettings, "Restore output settings");
                    }
                });
        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSTATE – MONOSTABLE MODE (OPENED IDLE STATE)",
            Order = "05.01.07",
            Id = "5-1-7",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DeviceIORelayOutputs, Feature.DeviceIORelayOutputsMonostableOpen },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputState})]
        public void SetRelayOutputMonostableOpenIdleStateTest()
        {
            RelayOutputSettings initialSettings = null;
            string token = string.Empty;

            RunTest(
                () =>
                {
                    RelayOutput[] outputs = GetRelayOutputs();

                    Assert(outputs != null && outputs.Length>0, "No relay outputs received via GetRelayOutputs", "Check that the DUT sent relay outputs information");

                    string testTime = string.Format("PT{0}S", _relayOutputDelayTimeMonostable);

                    token = outputs[0].token;
                    
                    RelayOutputSettings testSettings = new RelayOutputSettings();
                    testSettings.DelayTime = testTime;
                    testSettings.IdleState = RelayIdleState.open;
                    testSettings.Mode = RelayMode.Monostable;

                    SetRelayOutputSettings(token, testSettings);
                    initialSettings = outputs[0].Properties;

                    SetRelayOutputState(token, RelayLogicalState.active);
                    
                    // wait
                    BeginStep(string.Format("Wait {0} seconds", _relayOutputDelayTimeMonostable));
                    Sleep(_relayOutputDelayTimeMonostable * 1000);
                    StepPassed();
                },
                () =>
                {
                    if (initialSettings != null)
                    {
                        SetRelayOutputSettings(token, initialSettings, "Restore output settings");
                    }
                });
        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSTATE – MONOSTABLE MODE (CLOSED IDLE STATE)",
            Order = "05.01.08",
            Id = "5-1-8",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DeviceIORelayOutputs, Feature.DeviceIORelayOutputsMonostableClosed },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputState})]
        public void SetRelayOutputMonostableClosedIdleStateTest()
        {
            RelayOutputSettings initialSettings = null;
            string token = string.Empty;

            RunTest(
                () =>
                {
                    RelayOutput[] outputs = GetRelayOutputs();

                    Assert(outputs != null && outputs.Length>0, "No relay outputs received via GetRelayOutputs", "Check that the DUT sent relay outputs information");

                    int testTime = _relayOutputDelayTimeMonostable;

                    token = outputs[0].token;

                    RelayOutputSettings testSettings = new RelayOutputSettings();
                    testSettings.DelayTime = string.Format("PT{0}S", testTime);
                    testSettings.IdleState = RelayIdleState.closed;
                    testSettings.Mode = RelayMode.Monostable;

                    SetRelayOutputSettings(token, testSettings);
                    initialSettings = outputs[0].Properties;

                    SetRelayOutputState(token, RelayLogicalState.active);

                    // wait
                    BeginStep(string.Format("Wait {0} seconds", _relayOutputDelayTimeMonostable));
                    Sleep(_relayOutputDelayTimeMonostable * 1000);
                    StepPassed();

                },
                () =>
                {
                    if (initialSettings != null)
                    {
                        SetRelayOutputSettings(token, initialSettings, "Restore output settings");
                    }
                });
        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSTATE – MONOSTABLE MODE (INACTIVE BEFORE DELAYTIME EXPIRED)",
            Order = "05.01.09",
            Id = "5-1-9",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DeviceIORelayOutputs, Feature.DeviceIORelayOutputsMonostable },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputState})]
        public void SetRelayOutputMonostableOpenIdleStateTimeoutTest()
        {
            RelayOutputSettings initialSettings = null;
            string token = string.Empty;

            RunTest(
                () =>
                {
                    RelayOutput[] outputs = GetRelayOutputs();

                    Assert(outputs != null && outputs.Length>0, "No relay outputs received via GetRelayOutputs", "Check that the DUT sent relay outputs information");

                    int testTime = _relayOutputDelayTimeMonostable; 
                    token = outputs[0].token;

                    RelayOutputSettings testSettings = new RelayOutputSettings();
                    testSettings.DelayTime = string.Format("PT{0}S", testTime);
                    
                    if (Features.Contains(Feature.DeviceIORelayOutputsMonostableOpen))
                    {
                        testSettings.IdleState = RelayIdleState.open;
                        testSettings.Mode = RelayMode.Monostable;

                        SetRelayOutputSettings(token, testSettings);
                        initialSettings = outputs[0].Properties;

                        System.DateTime dtSend = System.DateTime.Now;

                        SetRelayOutputState(token, RelayLogicalState.active);
                    
                        System.DateTime dtReceived = System.DateTime.Now;

                        double delay = (dtSend - dtReceived).TotalMilliseconds;

                        System.DateTime dtSet = dtSend.AddMilliseconds(delay/2);
                        System.DateTime expireTime = dtSet.AddSeconds(testTime);

                        Assert(expireTime > System.DateTime.Now, "Timeout expired already",
                               "Check if timeout has not expired");

                        SetRelayOutputState(token, RelayLogicalState.inactive);

                        Assert(expireTime > System.DateTime.Now, "Timeout expired already",
                           "Check if timeout has not expired");

                        int timeLeft = (int)((expireTime - System.DateTime.Now).TotalSeconds);
                        // Wait
                        BeginStep("Check if timeout expired");
                        if (timeLeft > 0)
                        {
                            LogStepEvent(string.Format("Wait {0} seconds more", timeLeft));
                            Sleep(timeLeft * 1000);
                        }
                        StepPassed();               
                    
                    }
                    


                    // second part
                    if (Features.Contains(Feature.DeviceIORelayOutputsMonostableClosed))
                    {
                        testSettings.IdleState = RelayIdleState.closed;

                        SetRelayOutputSettings(token, testSettings);
                        initialSettings = outputs[0].Properties;

                        System.DateTime dtSend = System.DateTime.Now;
                        SetRelayOutputState(token, RelayLogicalState.active);
                        System.DateTime dtReceived = System.DateTime.Now;

                        double delay = (dtSend - dtReceived).TotalMilliseconds;
                        System.DateTime dtSet = dtSend.AddMilliseconds(delay / 2);
                        System.DateTime expireTime = dtSet.AddSeconds(testTime);

                        Assert(expireTime > System.DateTime.Now, "Timeout expired already",
                            "Check if timeout has not expired");

                        SetRelayOutputState(token, RelayLogicalState.inactive);

                        Assert(expireTime > System.DateTime.Now, "Timeout expired already",
                            "Check if timeout has not expired");

                        int timeLeft = (int)((expireTime - System.DateTime.Now).TotalSeconds);

                        // Wait
                        BeginStep("Check if timeout expired");
                        if (timeLeft > 0)
                        {
                            LogStepEvent(string.Format("Wait {0} seconds more", timeLeft));
                            Sleep(timeLeft * 1000);
                        }
                        StepPassed();
                    }
                    
                },
                () =>
                {
                    if (initialSettings != null)
                    {
                        SetRelayOutputSettings(token, initialSettings, "Restore output settings");
                    }
                });
        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSTATE – INVALID TOKEN",
            Order = "05.01.10",
            Id = "5-1-10",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.DeviceIORelayOutputs },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputState})]
        public void SetRelayOutputStateInvalidTokenTest()
        {
            RunTest(
                () =>
                {
                    RelayOutput[] outputs = GetRelayOutputs();

                    Assert(outputs != null, "No relay outputs received via GetRelayOutputs", "Check that the DUT sent relay outputs information");

                    List<string> tokens = new List<string>();

                    foreach (RelayOutput output in outputs)
                    {
                        tokens.Add(output.token);
                    }

                    string token = tokens.GetNonMatchingString();
                    
                    RunStep(
                        () => { Client.SetRelayOutputState(token, RelayLogicalState.active); },
                        "Set relay output settings - negative test", 
                        "Sender/InvalidArgVal/RelayToken", 
                        true);

                    DoRequestDelay();

                });
        }
        


        void ChangeRelayOutputProperties(string token, string delayTime, RelayIdleState idleState, RelayMode mode)
        {
            RelayOutputSettings testSettings = new RelayOutputSettings();
            testSettings.DelayTime = delayTime;
            testSettings.IdleState = idleState;
            testSettings.Mode = mode;

            SetRelayOutputSettings(token, testSettings);

            RelayOutput[] actualOutputs = GetRelayOutputs();

            Assert(actualOutputs != null, "No relay outputs received via GetRelayOutputs", "Check that the DUT sent relay outputs information");

            RelayOutput actualOutput =
                actualOutputs.Where(o => o.token == token).FirstOrDefault();

            Assert(actualOutput != null,
                string.Format("Relay output with token {0} not found", token),
                "Find current output settings");

            ValidateRelayOutputSettings(testSettings, actualOutput.Properties);

        }

        void ValidateRelayOutputSettings(RelayOutputSettings expected, RelayOutputSettings actual)
        {
            bool ok = true;
            StringBuilder dump = new StringBuilder();

            if (expected.Mode != actual.Mode)
            {
                ok = false;
                dump.AppendLine(string.Format("Mode is different. Expected: {0}, actual: {1}",
                    expected.Mode,
                    actual.Mode));
            }

            if (expected.IdleState != actual.IdleState)
            {
                ok = false;
                dump.AppendLine(string.Format("Idle state is different. Expected: {0}, actual: {1}",
                    expected.IdleState,
                    actual.IdleState));
            }

            if (expected.DelayTime != actual.DelayTime)
            {
                if (expected.Mode == RelayMode.Monostable)
                {
                    ok = false;
                    dump.AppendLine(string.Format("Delay time is different. Expected: {0}, actual: {1}",
                              expected.DelayTime,
                              actual.DelayTime));

                }
            }
            
            string reason = string.Empty;
            if (dump.Length > 0)
            {
                reason = dump.ToStringTrimNewLine();

            }
            Assert(ok, reason, "Compare expected and actual relay output properties");
        }
   
    }
}
