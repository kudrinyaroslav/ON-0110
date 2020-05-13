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
    //[TestClass]
    class DeviceIoServiceTestSuite : Base.IoTest
    {
        /// <summary>
        /// Relay output delay.
        /// </summary>
        protected int _relayOutputDelayTimeMonostable;
        protected int _relayOutputDelayTimeBistable;


        public DeviceIoServiceTestSuite(TestLaunchParam param)
            : base(param)
        {
            _relayOutputDelayTimeMonostable = param.RelayOutputDelayTimeMonostable;
            _relayOutputDelayTimeBistable = param.RelayOutputDelayTimeMonostable;

        }

        private const string PATH = "I/O";
        
        [Test(Name = "IO COMMAND GETRELAYOUTPUTS",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.DEVICEIO,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RelayOutputs },
            FunctionalityUnderTest = new Functionality[]{Functionality.GetRelayOutputs})]
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
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.DEVICEIO,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RelayOutputs },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRelayOutputs, Functionality.GetCapabilities })]
        public void RelayOutputsCountTest()
        {
            RunTest(
                () =>
                {
                    Capabilities capabilities = GetCapabilities(new[] { CapabilityCategory.All });

                    Assert(capabilities != null, "Ñapabilities not found", "Check that DUT returned capabilities");

                    bool found = capabilities.Extension != null;
                    if (found)
                    {
                        found = capabilities.Extension.DeviceIO != null;
                    }

                    Assert(found, "I/O capabilities not found", "Check that IO capabilities returned");
                    
                    RelayOutput[] outputs = GetRelayOutputs();

                    Assert(outputs != null, "No relay outputs received via GetRelayOutputs",
                           "Check that the DUT sent relay outputs information");

                    Assert(outputs.Length == capabilities.Extension.DeviceIO.RelayOutputs,
                           "Count of relay output actually received differs from count specified in GetCapabilities response",
                           "Check that count of relay outputs is the same");
                });
        }
        
        [Test(Name = "IO COMMAND SETRELAYOUTPUTSETTINGS",
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.DEVICEIO,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RelayOutputs },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputSettings })]
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
                        testTime= string.Format("PT{0}S", _relayOutputDelayTimeMonostable) ;

                        ChangeRelayOutputProperties(output.token, testTime, RelayIdleState.open, RelayMode.Bistable);

                        testTime = string.Format("PT{0}S", _relayOutputDelayTimeBistable);

                        ChangeRelayOutputProperties(output.token, testTime, RelayIdleState.closed, RelayMode.Monostable);
                    }

                });

        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSETTINGS – INVALID TOKEN",
            Order = "01.01.04",
            Id = "1-1-4",
            Category = Category.DEVICEIO,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RelayOutputs },
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
                    testSettings.DelayTime = "PT10S";
                    testSettings.IdleState = RelayIdleState.open;
                    testSettings.Mode = RelayMode.Bistable;

                    RelayOutput testOutput = new RelayOutput();
                    testOutput.token = token;
                    testOutput.Properties = testSettings;

                    RunStep(
                        () => { Client.SetRelayOutputSettings(testOutput); },
                        "Set relay output settings - negative test",
                        "Sender/InvalidArgVal/RelayToken",
                        true);
                    DoRequestDelay();

                });

        }


        [Test(Name = "IO COMMAND SETRELAYOUTPUTSTATE – BISTABLE MODE (OPENED IDLE STATE)",
            Order = "01.01.05",
            Id = "1-1-5",
            Category = Category.DEVICEIO,
            Path = PATH,
            Version = 2.0,
            Interactive = true,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RelayOutputs },
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

                    string testTime = string.Format("PT{0}S", _relayOutputDelayTimeMonostable);

                    token = outputs[0].token;

                    RelayOutputSettings testSettings = new RelayOutputSettings();
                    testSettings.DelayTime = testTime;
                    testSettings.IdleState = RelayIdleState.open;
                    testSettings.Mode = RelayMode.Bistable;

                    RelayOutput testOutput = new RelayOutput();
                    testOutput.token = token;
                    testOutput.Properties = testSettings;

                    SetRelayOutputSettings(testOutput);
                    initialSettings = outputs[0].Properties;

                    SetRelayOutputState(token, RelayLogicalState.active);

                    // get answer
                    CheckRelayOutputClosed(token);

                    SetRelayOutputState(token, RelayLogicalState.inactive);

                    // get answer
                    CheckRelayOutputOpen(token);

                },
                () =>
                {
                    if (initialSettings != null)
                    {
                        RelayOutput output = new RelayOutput();
                        output.token = token;
                        output.Properties = initialSettings;
                        SetRelayOutputSettings(output, "Restore output settings");
                    }
                });
        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSTATE – BISTABLE MODE (CLOSED IDLE STATE)",
            Order = "01.01.06",
            Id = "1-1-6",
            Category = Category.DEVICEIO,
            Path = PATH,
            Version = 2.0,
            Interactive = true,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RelayOutputs },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputState })]
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

                    RelayOutput output = new RelayOutput();
                    output.token = token;
                    output.Properties = testSettings;
                    
                    SetRelayOutputSettings(output);
                    initialSettings = outputs[0].Properties;

                    SetRelayOutputState(token, RelayLogicalState.active);

                    // get answer
                    CheckRelayOutputOpen(token);

                    SetRelayOutputState(token, RelayLogicalState.inactive);

                    // get answer
                    CheckRelayOutputClosed(token);

                },
                () =>
                {
                    if (initialSettings != null)
                    {
                        RelayOutput output = new RelayOutput();
                        output.token = token;
                        output.Properties = initialSettings;

                        SetRelayOutputSettings(output, "Restore output settings");
                    }
                });
        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSTATE –  MONOSTABLE MODE (OPENED IDLE STATE)",
            Order = "01.01.07",
            Id = "1-1-7",
            Category = Category.DEVICEIO,
            Path = PATH,
            Version = 2.0,
            Interactive = true,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RelayOutputs },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputState })]
        public void SetRelayOutputMonostableOpenIdleStateTest()
        {
            RelayOutputSettings initialSettings = null;
            string token = string.Empty;

            RunTest(
                () =>
                {
                    RelayOutput[] outputs = GetRelayOutputs();

                    Assert(outputs != null && outputs.Length > 0, "No relay outputs received via GetRelayOutputs", "Check that the DUT sent relay outputs information");

                    int testTime = 60;
                    bool ok = _operator.GetDelayTime("Enter Delay Time value for test", ref testTime);

                    Assert(ok, "Operator cancelled the test", "Get test parameter from operator");

                    token = outputs[0].token;

                    RelayOutputSettings testSettings = new RelayOutputSettings();
                    testSettings.DelayTime = string.Format("PT{0}S", testTime);
                    testSettings.IdleState = RelayIdleState.open;
                    testSettings.Mode = RelayMode.Monostable;

                    RelayOutput output = new RelayOutput();
                    output.token = token;
                    output.Properties = testSettings;
                    
                    SetRelayOutputSettings(output);
                    initialSettings = outputs[0].Properties;

                    SetRelayOutputState(token, RelayLogicalState.active);

                    System.DateTime setTime = System.DateTime.Now;

                    // get answer

                    CheckRelayOutputClosed(token);

                    System.DateTime answerTime = System.DateTime.Now;

                    // wait
                    int diff = testTime - (int)((answerTime - setTime).TotalSeconds);
                    BeginStep("Check if timeout expired");
                    if (diff > 0)
                    {
                        LogStepEvent(string.Format("Wait {0} seconds more", diff));
                        Sleep(diff * 1000);
                    }
                    StepPassed();

                    // get answer
                    CheckRelayOutputOpen(token);
                },
                () =>
                {
                    if (initialSettings != null)
                    {
                        RelayOutput output = new RelayOutput();
                        output.token = token;
                        output.Properties = initialSettings;

                        SetRelayOutputSettings(output, "Restore output settings");
                    }
                });
        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSTATE –  MONOSTABLE MODE (CLOSED IDLE STATE)",
            Order = "01.01.08",
            Id = "1-1-8",
            Category = Category.DEVICEIO,
            Path = PATH,
            Version = 2.0,
            Interactive = true,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RelayOutputs },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputState })]
        public void SetRelayOutputMonostableClosedIdleStateTest()
        {
            RelayOutputSettings initialSettings = null;
            string token = string.Empty;

            RunTest(
                () =>
                {
                    RelayOutput[] outputs = GetRelayOutputs();

                    Assert(outputs != null && outputs.Length > 0, "No relay outputs received via GetRelayOutputs", "Check that the DUT sent relay outputs information");

                    int testTime = 60;
                    bool ok = _operator.GetDelayTime("Enter Delay Time value for test", ref testTime);

                    Assert(ok, "Operator cancelled the test", "Get test parameter from operator");

                    token = outputs[0].token;

                    RelayOutputSettings testSettings = new RelayOutputSettings();
                    testSettings.DelayTime = string.Format("PT{0}S", testTime);
                    testSettings.IdleState = RelayIdleState.closed;
                    testSettings.Mode = RelayMode.Monostable;

                    RelayOutput output = new RelayOutput();
                    output.token = token;
                    output.Properties = testSettings;

                    SetRelayOutputSettings(output);
                    initialSettings = outputs[0].Properties;

                    SetRelayOutputState(token, RelayLogicalState.active);

                    System.DateTime setTime = System.DateTime.Now;

                    // get answer
                    CheckRelayOutputOpen(token);

                    System.DateTime answerTime = System.DateTime.Now;

                    // wait
                    int diff = testTime - (int)((answerTime - setTime).TotalSeconds);
                    BeginStep("Check if timeout expired");
                    if (diff > 0)
                    {
                        LogStepEvent(string.Format("Wait {0} seconds more", diff));
                        Sleep(diff * 1000);
                    }
                    StepPassed();

                    // get answer

                    CheckRelayOutputClosed(token);
                },
                () =>
                {
                    if (initialSettings != null)
                    {
                        RelayOutput output = new RelayOutput();
                        output.token = token;
                        output.Properties = initialSettings;

                        SetRelayOutputSettings(output, "Restore output settings");
                    }
                });
        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSTATE – MONOSTABLE MODE (INACTIVE BEFORE DELAYTIME EXPIRED)",
            Order = "01.01.09",
            Id = "1-1-9",
            Category = Category.DEVICEIO,
            Path = PATH,
            Version = 2.0,
            Interactive = true,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RelayOutputs },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputState })]
        public void SetRelayOutputMonostableOpenIdleStateTimeoutTest()
        {
            RelayOutputSettings initialSettings = null;
            string token = string.Empty;

            RunTest(
                () =>
                {
                    RelayOutput[] outputs = GetRelayOutputs();

                    Assert(outputs != null && outputs.Length > 0, "No relay outputs received via GetRelayOutputs", "Check that the DUT sent relay outputs information");

                    int testTime = 60;
                    bool ok = _operator.GetDelayTime("Enter Delay Time value for test", ref testTime);

                    Assert(ok, "Operator cancelled the test", "Get test parameter from operator");

                    token = outputs[0].token;

                    RelayOutputSettings testSettings = new RelayOutputSettings();
                    testSettings.DelayTime = string.Format("PT{0}S", testTime);
                    testSettings.IdleState = RelayIdleState.open;
                    testSettings.Mode = RelayMode.Monostable;

                    RelayOutput output = new RelayOutput();
                    output.token = token;
                    output.Properties = testSettings;
                    
                    SetRelayOutputSettings(output);
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

                    // get answer
                    CheckRelayOutputOpen(token, timeLeft);

                    timeLeft = (int)((expireTime - System.DateTime.Now).TotalSeconds);

                    // Wait
                    BeginStep("Check if timeout expired");
                    if (timeLeft > 0)
                    {
                        LogStepEvent(string.Format("Wait {0} seconds more", timeLeft));
                        Sleep(timeLeft * 1000);
                    }
                    StepPassed();
                    // second part

                    testSettings.IdleState = RelayIdleState.closed;

                    output = new RelayOutput();
                    output.token = token;
                    output.Properties = testSettings;

                    SetRelayOutputSettings(output);
                    initialSettings = outputs[0].Properties;

                    dtSend = System.DateTime.Now;
                    SetRelayOutputState(token, RelayLogicalState.active);
                    dtReceived = System.DateTime.Now;

                    delay = (dtSend - dtReceived).TotalMilliseconds;
                    dtSet = dtSend.AddMilliseconds(delay / 2);
                    expireTime = dtSet.AddSeconds(testTime);

                    Assert(expireTime > System.DateTime.Now, "Timeout expired already",
                        "Check if timeout has not expired");

                    SetRelayOutputState(token, RelayLogicalState.inactive);

                    Assert(expireTime > System.DateTime.Now, "Timeout expired already",
                        "Check if timeout has not expired");

                    timeLeft = (int)((expireTime - System.DateTime.Now).TotalSeconds);

                    // get answer
                    CheckRelayOutputClosed(token, timeLeft);

                    timeLeft = (int)((expireTime - System.DateTime.Now).TotalSeconds);

                    // Wait
                    BeginStep("Check if timeout expired");
                    if (timeLeft > 0)
                    {
                        LogStepEvent(string.Format("Wait {0} seconds more", timeLeft));
                        Sleep(timeLeft * 1000);
                    }
                    StepPassed();

                },
                () =>
                {
                    if (initialSettings != null)
                    {
                        RelayOutput output = new RelayOutput();
                        output.token = token;
                        output.Properties = initialSettings;

                        SetRelayOutputSettings(output, "Restore output settings");
                    }
                });
        }

        [Test(Name = "IO COMMAND SETRELAYOUTPUTSTATE – INVALID TOKEN",
            Order = "01.01.10",
            Id = "1-1-10",
            Category = Category.DEVICEIO,
            Path = PATH,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RelayOutputs },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRelayOutputState })]
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

            RelayOutput output = new RelayOutput();
            output.token = token;
            output.Properties = testSettings;

            SetRelayOutputSettings(output);

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
            bool warning = false;
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
                if (expected.Mode == RelayMode.Bistable)
                {
                    warning = true;
                    dump.AppendLine(string.Format("WARNING: Delay time is different. Expected: {0}, actual: {1}",
                                                  expected.DelayTime,
                                                  actual.DelayTime));
                }
                else
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
                reason = dump.ToString(0, dump.Length - 2);

            }

            if (!ok)
            {
                Assert(ok, reason, "Compare expected and actual relay output properties");
            }
            else
            {
                if (warning)
                {
                    Assert(true, string.Empty, "Compare expected and actual relay output properties", reason);
                }
                else
                {
                    Assert(true, string.Empty, "Compare expected and actual relay output properties");
                }
            }

        }


        void CheckRelayOutputOpen(string token)
        {
            CheckRelayOutputOpen(token, -1);
        }

        void CheckRelayOutputOpen(string token, int timeout)
        {
            string question = timeout > 0 ?
                string.Format("Is the \"{0}\" relay open?{1}(Please, check in {2} seconds)", token, System.Environment.NewLine, timeout) :
                string.Format("Is the \"{0}\" relay open?", token);

            Assert(new CheckCondition(() =>
            {
                return _operator.GetYesNoAnswer(question);
            }),
                "Relay is closed",
                "Check that relay is open");

        }

        void CheckRelayOutputClosed(string token)
        {
            CheckRelayOutputClosed(token, -1);
        }

        void CheckRelayOutputClosed(string token, int timeout)
        {
            string question = timeout > 0 ?
                string.Format("Is the \"{0}\" relay closed?{1}(Please, check in {2} seconds)", token, System.Environment.NewLine, timeout) :
                string.Format("Is the \"{0}\" relay closed?", token);

            Assert(new CheckCondition(() => { return _operator.GetYesNoAnswer(question); }),
                "Relay is open",
                "Check that relay is closed");
        }
        

    }
}
