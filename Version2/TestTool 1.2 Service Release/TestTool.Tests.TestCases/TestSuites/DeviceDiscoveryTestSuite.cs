///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.ServiceModel;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Discovery;
using WSD = TestTool.Proxies.WSDiscovery;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class DeviceDiscoveryTestSuite : Base.DeviceDiscoveryTest
    {
        public DeviceDiscoveryTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        [Test(Name = "HELLO MESSAGE",
            Path = "Device Discovery",
            Order="01.01.01",
            Id = "1-1-1",
            Category = Category.DISCOVERY,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must)]
        public void HelloMessageTest()
        {
            RunTest(
                () =>
                {
                    Reboot();
                    ReceiveHelloMessage(true, true, null);
                }
            );
        }

        [Test(Name = "HELLO MESSAGE VALIDATION",
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.DISCOVERY,
            Path = "Device Discovery",
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must)]
        public void HelloMessageValidationTest()
        {
            RunTest(
                () =>
                {
                    Reboot();
                    SoapMessage<WSD.HelloType> hello = ReceiveHelloMessage(true, true, null);
                    string reason = null;
                    Assert(ValidateHelloMessage(hello, null, out reason), reason, "Validating hello message");
                }
            );
        }
        [Test(Name = "SEARCH BASED ON DEVICE SCOPE TYPES",
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.DISCOVERY,
            Path = "Device Discovery",
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must)]
        public void SearchDeviceScopeTypesTest()
        {
            RunTest(() =>
            {
                TestTool.Proxies.Onvif.Scope[] scopes = GetScopes();
                string missingScope = DiscoveryUtils.GetMissingMandatoryScope(scopes);
                Assert(string.IsNullOrEmpty(missingScope), 
                    string.Format(Resources.ErrorMissingMandatoryScope_Format, missingScope),
                    "Validating device scopes");

                SoapMessage<WSD.ProbeMatchesType> probeMatch = ProbeDeviceStep(true, DiscoveryUtils.GetManadatoryScopes(), null);

                string reason = null;
                Assert(ValidateProbeMatchMessage(probeMatch, out reason), reason, Resources.StepValidateProbeMatch_Title);
            });
        }

        [Test(Name = "SEARCH WITH OMITTED DEVICE AND SCOPE TYPES",
            Path = "Device Discovery",
            Order = "01.01.04",
            Id = "1-1-4",
            Category = Category.DISCOVERY,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must)]
        public void SearchOmittedDeviceTest()
        {
            RunTest(() =>
            {
                SoapMessage<WSD.ProbeMatchesType> probeMatch = ProbeDeviceStep(true, null, null);

                string reason = null;
                Assert(ValidateProbeMatchMessage(probeMatch, out reason), reason, Resources.StepValidateProbeMatch_Title);
            });
        }

        [Test(Name = "RESPONSE TO INVALID SEARCH REQUEST",
            Path = "Device Discovery",
            Order = "01.01.05",
            Id = "1-1-5",
            Category = Category.DISCOVERY,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must)]
        public void ResponseInvalidSearchTest()
        {
            RunTest(() =>
            {
                InvalidProbeDeviceStep(true, new string[] { "InvalidScope1", "InvalidScope1" });
            });
        }

        [Test(Name = "SEARCH USING UNICAST PROBE MESSAGE",
            Path = "Device Discovery",
            Order = "01.01.06",
            Id = "1-1-6",
            Category = Category.DISCOVERY,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional)]
        public void UnicastProbeMessageTest()
        {
            RunTest(() =>
            {
                //5.3
                Scope[] scopes = GetScopes();
                string missingScope = DiscoveryUtils.GetMissingMandatoryScope(scopes);
                Assert(string.IsNullOrEmpty(missingScope),
                    string.Format(Resources.ErrorMissingMandatoryScope_Format, missingScope),
                    "Validating device scopes");
                SoapMessage<WSD.ProbeMatchesType> probeMatch = ProbeDeviceStep(false, DiscoveryUtils.GetManadatoryScopes(), null);
                string reason = null;
                Assert(ValidateProbeMatchMessage(probeMatch, out reason), reason, Resources.StepValidateProbeMatch_Title);
                //5.4
                probeMatch = ProbeDeviceStep(false, null, null);
                Assert(ValidateProbeMatchMessage(probeMatch, out reason), reason, Resources.StepValidateProbeMatch_Title);
                //5.5
                InvalidProbeDeviceStep(false, new string[] { "InvalidScope1", "InvalidScope1" });
            });
        }

        [Test(Name = "DEVICE SCOPES CONFIGURATION",
            Path = "Device Discovery",
            Order = "01.01.07",
            Id = "1-1-7",
            Category = Category.DISCOVERY,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceScopesConfigurationTest()
        {
            bool scopesAdded = false;
            string[] newScopes = null;

            RunTest<string[]>(
                new Backup<string[]>(() => { return null; }),
                () =>
                {
                    //get and validate fixed scopes
                    Scope[] scopes = GetScopes();

                    string missingScope = DiscoveryUtils.GetMissingMandatoryScope(scopes);
                    Assert(string.IsNullOrEmpty(missingScope),
                        string.Format(Resources.ErrorMissingMandatoryScope_Format, missingScope),
                        "Validating device scopes");
                
                    //set fixed scopes fault validation
                    SetFixedScopes(scopes);

                    // fix for 10767
                    Sleep(2000);

                    //add new scopes
                    //begin wait before sending AddScopes, because Hello can be sent by NVT before response to AddScopes
                    newScopes = new string[] { "onvif://www.onvif.org/" + Guid.NewGuid().ToString(), "onvif://www.onvif.org/" + Guid.NewGuid().ToString() };
                    bool canAddScopes = true;
                    SoapMessage<WSD.HelloType> hello = null;
                    try
                    {
                        hello = ReceiveHelloMessage(
                            true,
                            true,
                            () => { 
                                AddScopes(newScopes); 
                                scopesAdded = true; 
                            });
                    }
                    catch (FaultException fault)
                    {
                        if (fault.IsValidOnvifFault("Receiver/Action/TooManyScopes"))
                        {
                            StepPassed();
                            canAddScopes = false;
                        }
                        else
                        {
                            throw;
                        }
                    }
                    //According to CR26, if device cannot add scopes, consider test as passed.
                    if(canAddScopes)
                    {
                        string reason;
                        Assert(ValidateHelloMessage(hello, newScopes, out reason), reason, "Hello message validation");

                        //probe new scopes
                        SoapMessage<WSD.ProbeMatchesType> probeMatch = ProbeDeviceStep(true, newScopes, null);

                        Assert(ValidateProbeMatchMessage(probeMatch, out reason), reason, Resources.StepValidateProbeMatch_Title);

                        //delete create scopes
                        hello = ReceiveHelloMessage(
                            true,
                            true,
                            () =>
                            {
                                RemoveScopes(newScopes);
                                scopesAdded = false;
                            });

                        Assert(ValidateHelloMessage(hello, null, out reason), reason, "Hello message validation");

                        //probe deleted scopes
                        InvalidProbeDeviceStep(true, newScopes);
                    }
                },
                (param) =>
                {
                    if (scopesAdded && (newScopes != null))
                    {
                        RemoveScopes(newScopes);
                    }
                });
        }

        [Test(Name = "BYE MESSAGE",
            Path = "Device Discovery",
            Order = "01.01.08",
            Id = "1-1-8",
            Category = Category.DISCOVERY,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Should)]
        public void ByeMessageTest()
        {
            RunTest(
                () =>
                {
                    ReceiveByeMessage(true, true, () => { Reboot(); });
                    WaitDeviceReboot();
                });
        }
        [Test(Name = "DISCOVERY MODE CONFIGURATION",
            Path = "Device Discovery",
            Version = 1.02,
            Order = "01.01.09",
            Id = "1-1-9",
            Category = Category.DISCOVERY,
            RequirementLevel = RequirementLevel.Must)]
        public void DiscoveryModeConfigurationTest()
        {
            RunTest(
                () =>
                {
                    DiscoveryMode mode = GetDiscoveryMode();
                    Assert(mode == DiscoveryMode.Discoverable, "Discovery invalid mode discovery mode returned.", "Check current DiscoveryMode");
                    // Set NonDiscoverable
                    SetDiscoveryMode(DiscoveryMode.NonDiscoverable);

                    mode = GetDiscoveryMode();
                    Assert(mode == DiscoveryMode.NonDiscoverable, "Discovery invalid mode discovery mode returned.", "Check current DiscoveryMode");

                    //probe device while it is non-discoverable
                    InvalidProbeDeviceStep(true, null);

                    //reboot and verify no bye or hello
                    ReceiveByeOrHelloMessage(
                        true, 
                        true, 
                        () => { Reboot(); },
                        (p) => 
                        {
                            if(p != null)
                            {
                                string reason = string.Format("NVT sent {0} while NVT is NonDiscoverable",
                                    p.Object is WSD.HelloType ? "Hello" : "Bye");
                                throw new AssertException(reason);
                            }
                        });
                    SetDiscoveryMode(DiscoveryMode.Discoverable);
                });
        }

        [Test(Name = "SOAP FAULT MESSAGE",
            Path = "Device Discovery",
            Order = "01.01.10",
            Id = "1-1-10",
            Category = Category.DISCOVERY,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional)]
        public void SoapFaultMessageTest()
        {
            RunTest(() =>
            {
                RunStep(() =>
                {
                    try
                    {
                        SoapMessage<WSD.ProbeMatchesType> response = ProbeDevice(
                            false, DiscoveryUtils.GetManadatoryScopes(), "InvalidMatchRule");
                        throw new AssertException(response != null ? "NVT responded to invalid probe message" :
                        "NVT did not respond");
                    }
                    catch (SoapFaultException ex)
                    {
                        string reason;
                        if(!DiscoveryUtils.IsCorrectSoapFault(
                            ex.Fault, 
                            "Sender",
                            SoapBuilder.SoapEnvelopeUri,
                            "MatchingRuleNotSupported",
                            DiscoveryUtils.WS_DISCOVER_NS,
                            out reason))
                        {
                            throw new AssertException(reason);
                        }
                    }
                }, Resources.StepInvalidProbe_Title);
            });
        }
    }
}
