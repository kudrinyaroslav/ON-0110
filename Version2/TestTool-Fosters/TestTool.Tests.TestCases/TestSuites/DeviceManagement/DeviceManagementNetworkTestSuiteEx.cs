using System;
using System.Linq;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites.DeviceManagement
{
    class DeviceManagementNetworkTestSuiteEx : DeviceManagementIPTestSuite
    {

        public DeviceManagementNetworkTestSuiteEx(TestLaunchParam param)
            : base(param)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Original interface</returns>
        NetworkInterface TurnOnDhcpIpv4()
        {
            BackupConnection();

            NetworkInterface[] interfaces = GetNetworkInterfaces();

            BeginStep("Check if DHCP must be turned on");

            NetworkInterface ni = null;
            foreach (NetworkInterface n in interfaces)
            {
                if (n.IPv4 == null) continue;
                if (!n.IPv4.Enabled) continue;
                if (n.IPv4.Config == null) continue;
                ni = n;
                break;
            }

            if (ni == null)
            {
                throw new AssertException("Appropriate network interface not found");
            }

            if (ni.IPv4.Config.DHCP)
            {
                LogStepEvent("DHCP is ON");
                StepPassed();
                return ni;
            }

            StepPassed();

            NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
            configuration.Enabled = true;
            configuration.EnabledSpecified = true;
            configuration.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
            configuration.IPv4.Enabled = true;
            configuration.IPv4.EnabledSpecified = true;
            configuration.IPv4.DHCP = true;
            configuration.IPv4.DHCPSpecified = true;

            SetIPConfiguration(ni.token, configuration);

            return ni;
        }

        NetworkInterface TurnOffDhcpIpv4()
        {
            BackupConnection();

            NetworkInterface[] interfaces = GetNetworkInterfaces();

            BeginStep("Check if DHCP must be turned off");

            NetworkInterface ni = null;
            foreach (NetworkInterface n in interfaces)
            {
                if (n.IPv4 == null) continue;
                //if (!n.IPv4.Enabled) continue;
                if (n.IPv4.Config == null) continue;
                ni = n;
                break;
            }

            if (ni == null)
            {
                throw new AssertException("Appropriate network interface not found");
            }

            if (!ni.IPv4.Config.DHCP)
            {
                LogStepEvent("DHCP is OFF");
                StepPassed();
                return ni;
            }
            else
            {
                if (ni.IPv4.Config.FromDHCP == null || string.IsNullOrEmpty(ni.IPv4.Config.FromDHCP.Address))
                {
                    throw new AssertException("IP address not returned from the DUT");
                }
            }
            
            StepPassed();

            NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
            configuration.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
            configuration.Enabled = true;
            configuration.EnabledSpecified = true;
            configuration.IPv4.Enabled = true;
            configuration.IPv4.EnabledSpecified = true;
            configuration.IPv4.DHCP = false;
            configuration.IPv4.DHCPSpecified = true;
            configuration.IPv4.Manual = new PrefixedIPv4Address[] { Extensions.NextAddress(ni.IPv4.Config.FromDHCP) };

            SetIPConfiguration(ni.token, configuration);

            return ni;
        }

        NetworkInterface TurnOffDhcpIpv6()
        {
            BackupConnection();

            NetworkInterface[] interfaces = GetNetworkInterfaces();

            BeginStep("Check if DHCP must be turned off");

            NetworkInterface ni = null;
            foreach (NetworkInterface n in interfaces)
            {
                if (n.IPv6 == null) continue;
                //if (!n.IPv6.Enabled) continue;
                if (n.IPv6.Config == null) continue;
                ni = n;
                break;
            }

            if (ni == null)
            {
                throw new AssertException("Appropriate network interface not found");
            }

            if (ni.IPv6.Config.DHCP == IPv6DHCPConfiguration.Off )
            {
                LogStepEvent("DHCP is OFF");
                StepPassed();
                return ni;
            }

            StepPassed();

            NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
            configuration.Enabled = true;
            configuration.EnabledSpecified = true;
            configuration.IPv6 = new IPv6NetworkInterfaceSetConfiguration();
            configuration.IPv6.Enabled = true;
            configuration.IPv6.EnabledSpecified = true;
            configuration.IPv6.DHCP = IPv6DHCPConfiguration.Off;
            configuration.IPv6.DHCPSpecified = true;

            if (ni.IPv6.Config.FromDHCP != null && ni.IPv6.Config.FromDHCP.Length > 0)
            {
                configuration.IPv6.Manual = ni.IPv6.Config.FromDHCP;
            }
            
            SetIPConfiguration(ni.token, configuration);

            return ni;
        }

        [Test(Name = "SET DNS CONFIGURATION - DNSMANUAL IPV4",
            Order = "02.01.06",
            Id = "2-1-6",
            Category = Category.DEVICE,
            Path = DeviceManagementNetworkTestSuite.PATH,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetDns },
            RequirementLevel = RequirementLevel.Must)]
        public void SetDnsConfigurationTest()
        {
            NetworkInterface ni = null;
            RunTest<DNSInformation>(

                new Backup<DNSInformation>(() =>
                {
                    DNSInformation originalInformation = GetDnsConfiguration();
                    Assert(originalInformation != null, "Failed to get original DNS configuration", "Check that original DNS configuration returned from the DUT");

                    ni = TurnOffDhcpIpv4();

                    return originalInformation;
                }),

                () =>
                {

                    DNSInformation testInformation = new DNSInformation();
                    testInformation.FromDHCP = false;
                    testInformation.DNSManual =
                        new IPAddress[]
                                    {
                                        new IPAddress(){IPv4Address = _environmentSettings.DnsIpv4, Type = IPType.IPv4}
                                    };

                    SetDnsConfiguration(testInformation);

                    double timeout = ((double)_operationDelay) / 1000;

                    BeginStep(string.Format("Wait {0} seconds to allow the DUT to apply settings", timeout.ToString("0.000")));
                    Sleep(_operationDelay);
                    StepPassed();

                    DNSInformation actualInformation = GetDnsConfiguration();

                    Assert(actualInformation != null, "Failed to get current DNS configuration", "Check that current DNS configuration returned from the DUT");

                    BeginStep("Check current DNS configuration");

                    bool bAllEquals;

                    bool bEquals = (actualInformation.FromDHCP == testInformation.FromDHCP);

                    LogStepEvent(string.Format("FromDHCP: expected - {0}, actual - {1}", testInformation.FromDHCP,
                        actualInformation.FromDHCP));

                    bAllEquals = bEquals;

                    string actualIpDescription;
                    actualIpDescription = DeviceManagementNetworkTestSuite.DumpIPArray(actualInformation.DNSManual);

                    string expectedAddress = testInformation.DNSManual[0].IPv4Address;
                    IPType expectedType = testInformation.DNSManual[0].Type;

                    if (actualInformation.DNSManual.Where(
                        A => A.Type == expectedType && A.IPv4Address == expectedAddress).Count() > 0)
                    {
                        bEquals = true;
                    }
                    else
                    {
                        bEquals = false;
                    }

                    LogStepEvent(string.Format("DNSManual: expected - {0} should be presented, actual - {1}",
                         expectedAddress, actualIpDescription));

                    bAllEquals = bAllEquals && bEquals;

                    /*************************************************/

                    actualIpDescription = DeviceManagementNetworkTestSuite.DumpIPArray(actualInformation.DNSFromDHCP);

                    if (actualInformation.DNSFromDHCP == null)
                    {
                        bEquals = true;
                    }
                    else
                    {
                        bEquals = (actualInformation.DNSFromDHCP.Length == 0);
                    }

                    string expectedDescription = "No DNSFromDHCP";

                    LogStepEvent(string.Format("DNSFromDHCP: expected - {0}, actual - {1}",
                        expectedDescription, actualIpDescription));

                    bAllEquals = bAllEquals && bEquals;

                    /*********************************************************/

                    if (!bAllEquals)
                    {
                        throw new AssertException("Current DNS configuration differs from configuration was set");
                    }

                    StepPassed();
                },

                (originalInformation) =>
                {
                    SetDnsConfiguration(originalInformation, "Restore DNS configuration");

                    // if DHCP was OFF, it has not been changed
                    if (ni.IPv4.Config.DHCP)
                    {
                        // restore network interface configuration
                        RestoreNetworkInterface(ni.token, ni);
                    }
                }
            );
        }


        [Test(Name = "SET DNS CONFIGURATION - DNSMANUAL IPV6",
            Order = "02.01.07",
            Id = "2-1-7",
            Category = Category.DEVICE,
            Path = DeviceManagementNetworkTestSuite.PATH,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetDns },
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.IPv6 })]
        public void SetDnsConfigurationDnsManualIpv6Test()
        {
            NetworkInterface ni = null;

            RunTest<DNSInformation>(

                new Backup<DNSInformation>(() =>
                {
                    DNSInformation originalInformation = GetDnsConfiguration();
                    Assert(originalInformation != null, "Failed to get original DNS configuration", "Check that original DNS configuration returned from the DUT");

                    ni = TurnOffDhcpIpv6();

                    return originalInformation;
                }),

                () =>
                {

                    DNSInformation testInformation = new DNSInformation();
                    testInformation.FromDHCP = false;
                    testInformation.DNSManual =
                        new IPAddress[]
                                    {
                                        new IPAddress(){IPv6Address = _environmentSettings.DnsIpv6, Type = IPType.IPv6}
                                    };

                    SetDnsConfiguration(testInformation);

                    double timeout = ((double)_operationDelay) / 1000;

                    BeginStep(string.Format("Wait {0} seconds to allow the DUT to apply settings", timeout.ToString("0.000")));
                    Sleep(_operationDelay);
                    StepPassed();

                    DNSInformation actualInformation = GetDnsConfiguration();

                    Assert(actualInformation != null, "Failed to get current DNS configuration", "Check that current DNS configuration returned from the DUT");

                    BeginStep("Check current DNS configuration");

                    bool bAllEquals;

                    bool bEquals = (actualInformation.FromDHCP == testInformation.FromDHCP);

                    LogStepEvent(string.Format("FromDHCP: expected - {0}, actual - {1}", testInformation.FromDHCP,
                        actualInformation.FromDHCP));

                    bAllEquals = bEquals;

                    string actualIpDescription;
                    actualIpDescription = DeviceManagementNetworkTestSuite.DumpIPArray(actualInformation.DNSManual);

                    string expectedAddress = testInformation.DNSManual[0].IPv6Address;
                    IPType expectedType = testInformation.DNSManual[0].Type;

                    if (actualInformation.DNSManual.Where(
                        A => A.Type == expectedType && A.IPv6Address == expectedAddress).Count() > 0)
                    {
                        bEquals = true;
                    }
                    else
                    {
                        bEquals = false;
                    }

                    LogStepEvent(string.Format("DNSManual: expected - {0} should be presented, actual - {1}",
                         expectedAddress, actualIpDescription));

                    bAllEquals = bAllEquals && bEquals;

                    /*************************************************/

                    actualIpDescription = DeviceManagementNetworkTestSuite.DumpIPArray(actualInformation.DNSFromDHCP);

                    if (actualInformation.DNSFromDHCP == null)
                    {
                        bEquals = true;
                    }
                    else
                    {
                        bEquals = (actualInformation.DNSFromDHCP.Length == 0);
                    }

                    string expectedDescription = "No DNSFromDHCP";

                    LogStepEvent(string.Format("DNSFromDHCP: expected - {0}, actual - {1}",
                        expectedDescription, actualIpDescription));

                    bAllEquals = bAllEquals && bEquals;

                    /*********************************************************/

                    if (!bAllEquals)
                    {
                        throw new AssertException("Current DNS configuration differs from configuration was set");
                    }

                    StepPassed();
                },

                (originalInformation) =>
                {
                    SetDnsConfiguration(originalInformation, "Restore DNS configuration");
                    if (ni.IPv6.Config.DHCP != IPv6DHCPConfiguration.Off)
                    {
                        // restore network interface configuration
                        RestoreNetworkInterface(ni.token, ni);
                    }
                }
                );
        }


        [Test(Name = "SET DNS CONFIGURATION - FROMDHCP",
            Order = "02.01.08",
            Id = "2-1-8",
            Category = Category.DEVICE,
            Path = DeviceManagementNetworkTestSuite.PATH,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetDns },
            RequirementLevel = RequirementLevel.Must)]
        public void SetDnsConfigurationDnsFromDHCPTest()
        {
            NetworkInterface ni = null;

            RunTest<DNSInformation>(

                new Backup<DNSInformation>(() =>
                {
                    DNSInformation originalInformation = GetDnsConfiguration();
                    string reason = null;
                    Assert((originalInformation != null) && originalInformation.IsValidDnsInformation(false, out reason),
                        reason == null ? "Failed to get original DNS configuration" : reason,
                        "Check that valid DNS configuration returned from the DUT");

                    ni = TurnOnDhcpIpv4();

                    return originalInformation;
                }),

                () =>
                {

                    DNSInformation testInformation = new DNSInformation();
                    testInformation.FromDHCP = true;

                    SetDnsConfiguration(testInformation);

                    double timeout = ((double)_operationDelay) / 1000;

                    BeginStep(string.Format("Wait {0} seconds to allow the DUT to interact with DHCP server", timeout.ToString("0.000")));
                    Sleep(_operationDelay);
                    StepPassed();

                    DNSInformation actualInformation = GetDnsConfiguration();

                    string reason = null;
                    Assert((actualInformation != null) && actualInformation.IsValidDnsInformation(false, out reason),
                        reason == null ? "Failed to get original DNS configuration" : reason,
                        "Check that original DNS configuration returned from the DUT");


                    Assert(actualInformation != null, "Failed to get current DNS configuration", "Check that current DNS configuration returned from the DUT");

                    BeginStep("Check current DNS configuration");

                    bool bAllEquals;

                    bool bEquals = (actualInformation.FromDHCP == testInformation.FromDHCP);

                    LogStepEvent(string.Format("FromDHCP: expected - {0}, actual - {1}", testInformation.FromDHCP,
                        actualInformation.FromDHCP));

                    bAllEquals = bEquals;

                    string actualIpDescription;
                    actualIpDescription = DeviceManagementNetworkTestSuite.DumpIPArray(actualInformation.DNSManual);

                    string expectedAddress = "No IP Address";

                    if (actualInformation.DNSManual != null && actualInformation.DNSManual.Count() > 0)
                    {
                        bEquals = false;
                    }
                    else
                    {
                        bEquals = true;
                    }

                    LogStepEvent(string.Format("DNSManual: expected - {0}, actual - {1}",
                         expectedAddress, actualIpDescription));

                    bAllEquals = bAllEquals && bEquals;

                    /*************************************************/

                    actualIpDescription = DeviceManagementNetworkTestSuite.DumpIPArray(actualInformation.DNSFromDHCP);

                    if (actualInformation.DNSFromDHCP == null)
                    {
                        bEquals = false;
                    }
                    else
                    {
                        bEquals = (actualInformation.DNSFromDHCP.Length > 0);
                    }

                    string expectedDescription = "List of DNS servers";

                    LogStepEvent(string.Format("DNSFromDHCP: expected - {0}, actual - {1}",
                        expectedDescription, actualIpDescription));

                    bAllEquals = bAllEquals && bEquals;

                    /*********************************************************/

                    if (!bAllEquals)
                    {
                        throw new AssertException("Current DNS configuration differs from configuration was set");
                    }

                    StepPassed();
                },

                (originalInformation) =>
                {
                    SetDnsConfiguration(originalInformation, "Restore DNS configuration");

                    // if DHCP was ON, it has not been changed
                    if (!ni.IPv4.Config.DHCP)
                    {
                        // restore network interface configuration
                        RestoreNetworkInterface(ni.token, ni);
                    }
                }
            );
        }


        [Test(Name = "SET DNS CONFIGURATION - DNSMANUAL INVALID IPV4",
            Order = "02.01.09",
            Id = "2-1-9",
            Category = Category.DEVICE,
            Path = DeviceManagementNetworkTestSuite.PATH,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetDns },
            RequirementLevel = RequirementLevel.Optional)]
        public void SetInvalidDnsConfigurationTest()
        {
            RunTest<NetworkInterface>(
                new Backup<NetworkInterface>(
                    () =>
                    {
                        NetworkInterface ni = TurnOffDhcpIpv4();
                        return ni;
                    }),
                () =>
                {
                    DNSInformation testInformation = new DNSInformation();
                    testInformation.FromDHCP = false;
                    testInformation.DNSManual =
                        new IPAddress[]
                                {
                                    new IPAddress() {IPv4Address = "10.1.1", Type = IPType.IPv4}
                                };

                    string reason = string.Empty;
                    RunStep(() =>
                    {
                        Client.SetDNS(testInformation.FromDHCP, testInformation.SearchDomain,
                                      testInformation.DNSManual);
                    },
                            "Set DNS configuration - negative test",
                            "Sender/InvalidArgVal/InvalidIPv4Address", true);

                    DoRequestDelay();

                    DNSInformation actualInformation = GetDnsConfiguration();

                    Assert(actualInformation != null, "Failed to get current DNS configuration",
                           "Check that current DNS configuration returned from the DUT");

                    bool bValid = actualInformation.IsValidDnsInformation(false, out reason);
                    Assert(bValid, reason, "Validate current DNS configuration");

                    bool bFound = (actualInformation.DNSManual != null && actualInformation.DNSManual.Length > 0) &&
                                  actualInformation.DNSManual.Where(
                                      IP => IP.IPv4Address == testInformation.DNSManual[0].IPv4Address).
                                      Count() > 0;

                    Assert(!bFound, "DUT set address to invalid value",
                           "Check that current IPv4 addresses list does not containd invalid value");

                },
                (ni) =>
                {
                    if (ni.IPv4.Config.DHCP)
                    {
                        RestoreNetworkInterface(ni.token, ni);
                    }
                });

        }

        [Test(Name = "SET DNS CONFIGURATION - DNSMANUAL INVALID IPV6",
            Order = "02.01.10",
            Id = "2-1-10",
            Category = Category.DEVICE,
            Path = DeviceManagementNetworkTestSuite.PATH,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetDns },
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.IPv6 })]
        public void SetInvalidIpv6DnsConfigurationTest()
        {
            // turn OFF DHCP

            RunTest<NetworkInterface>(
                new Backup<NetworkInterface>(
                    () =>
                    {
                        NetworkInterface ni = TurnOffDhcpIpv6();
                        return ni;
                    }),
                () =>
                {
                    DNSInformation testInformation = new DNSInformation();
                    testInformation.FromDHCP = false;
                    testInformation.DNSManual =
                        new IPAddress[]
                                {
                                    new IPAddress() {IPv6Address = "FF02:1", Type = IPType.IPv6}
                                };

                    string reason = string.Empty;
                    RunStep(() =>
                    {
                        Client.SetDNS(testInformation.FromDHCP, testInformation.SearchDomain,
                                      testInformation.DNSManual);
                    },
                            "Set DNS configuration - negative test",
                            "Sender/InvalidArgVal/InvalidIPv6Address",
                            true);

                    DoRequestDelay();

                    DNSInformation actualInformation = GetDnsConfiguration();

                    Assert(actualInformation != null, "Failed to get current DNS configuration",
                           "Check that current DNS configuration returned from the DUT");

                    bool bValid = actualInformation.IsValidDnsInformation(false, out reason);
                    Assert(bValid, reason, "Validate current DNS configuration");

                    bool bFound = (actualInformation.DNSManual != null && actualInformation.DNSManual.Length > 0) &&
                                  actualInformation.DNSManual.Where(
                                      IP => IP.IPv4Address == testInformation.DNSManual[0].IPv6Address).
                                      Count() > 0;

                    Assert(!bFound, "DUT set address to invalid value",
                           "Check that current IPv6 addresses list does not contain invalid value");

                },
                (ni) =>
                {
                    if (ni.IPv6.Config.DHCP != IPv6DHCPConfiguration.Off)
                    {
                        RestoreNetworkInterface(ni.token, ni);
                    }
                });

        }


        [Test(Name = "SET NTP CONFIGURATION - NTPMANUAL IPV4",
            Order = "02.01.12",
            Id = "2-1-12",
            Category = Category.DEVICE,
            Path = DeviceManagementNetworkTestSuite.PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.NTP },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetNTP })]
        public void SetNtpConfigurationTest()
        {
            NetworkInterface ni = null;
            RunTest<NTPInformation>(
                new Backup<NTPInformation>(() =>
                {
                    NTPInformation originalInformation = GetNTP();
                    Assert(originalInformation != null,
                           "The DUT did not return NTP configuration",
                           "Check that DUT returned NTP information");
                    ni = TurnOffDhcpIpv4();
                    return originalInformation;
                }),
                () =>
                {
                    NTPInformation testInformation = new NTPInformation();
                    testInformation.FromDHCP = false;
                    testInformation.NTPManual = new NetworkHost[] { new NetworkHost() };
                    testInformation.NTPManual[0].Type = NetworkHostType.IPv4;
                    testInformation.NTPManual[0].IPv4Address = _environmentSettings.NtpIpv4;

                    SetNTP(testInformation);

                    NTPInformation currentInformation = GetNTP();
                    Assert(currentInformation != null, "The DUT did not return NTP configuration",
                           "Check that DUT returned NTP information");

                    BeginStep("Validate current NTP configuration");

                    bool bAllEquals;

                    bool bEquals = (currentInformation.FromDHCP == testInformation.FromDHCP);

                    LogStepEvent(string.Format("FromDHCP: expected - {0}, actual - {1}",
                                               testInformation.FromDHCP,
                                               currentInformation.FromDHCP));

                    bAllEquals = bEquals;

                    string actualIpDescription;
                    actualIpDescription = DeviceManagementNetworkTestSuite.DumpNetworkHostArray(currentInformation.NTPManual);

                    NetworkHostType type = testInformation.NTPManual[0].Type;
                    string expectedAddress = testInformation.NTPManual[0].IPv4Address;
                    if (
                        currentInformation.NTPManual.Where(
                            A => A.Type == type && A.IPv4Address == expectedAddress).Count() > 0)
                    {
                        bEquals = true;
                    }
                    else
                    {
                        bEquals = false;
                    }

                    string expectedDescription = string.Format("{0} should be presented",
                                                               testInformation.NTPManual[0].IPv4Address);

                    LogStepEvent(string.Format("NTPManual: expected - {0}, actual - {1}",
                                               expectedDescription, actualIpDescription));

                    bAllEquals = bAllEquals && bEquals;

                    actualIpDescription = DeviceManagementNetworkTestSuite.DumpNetworkHostArray(currentInformation.NTPFromDHCP);
                    if (currentInformation.NTPFromDHCP == null)
                    {
                        bEquals = true;
                    }
                    else
                    {
                        bEquals = (currentInformation.NTPFromDHCP.Length == 0);
                    }

                    expectedDescription = "No NTPFromDHCP";

                    LogStepEvent(string.Format("NTPFromDHCP: expected - {0}, actual - {1}",
                                               expectedDescription, actualIpDescription));

                    bAllEquals = bAllEquals && bEquals;

                    if (!bAllEquals)
                    {
                        throw new AssertException(
                            "Current NTP configuration differs from configuration set in previous step");
                    }

                    StepPassed();

                },
                (originalInformation) =>
                {
                    SetNTP(originalInformation, "Restore NTP configuration");

                    if (ni.IPv4.Config.DHCP)
                    {
                        RestoreNetworkInterface(ni.token, ni);
                    }
                }
                );
        }


        [Test(Name = "SET NTP CONFIGURATION - NTPMANUAL IPV6",
            Order = "02.01.13",
            Id = "2-1-13",
            Category = Category.DEVICE,
            Path = DeviceManagementNetworkTestSuite.PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.NTP, Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetNTP })]
        public void SetNtpIpv6ConfigurationTest()
        {
            NetworkInterface ni = null;

            RunTest<NTPInformation>(
                new Backup<NTPInformation>(() =>
                {
                    NTPInformation originalInformation = GetNTP();
                    Assert(originalInformation != null,
                           "The DUT did not return NTP configuration",
                           "Check that DUT returned NTP information");

                    ni = TurnOffDhcpIpv6();

                    return originalInformation;
                }),
                () =>
                {
                    NTPInformation testInformation = new NTPInformation();
                    testInformation.FromDHCP = false;
                    testInformation.NTPManual = new NetworkHost[] { new NetworkHost() };
                    testInformation.NTPManual[0].Type = NetworkHostType.IPv6;
                    testInformation.NTPManual[0].IPv6Address = _environmentSettings.NtpIpv6;

                    SetNTP(testInformation);

                    NTPInformation currentInformation = GetNTP();
                    Assert(currentInformation != null, "The DUT did not return NTP configuration",
                           "Check that DUT returned NTP information");

                    BeginStep("Validate current NTP configuration");

                    bool bAllEquals;

                    bool bEquals = (currentInformation.FromDHCP == testInformation.FromDHCP);

                    LogStepEvent(string.Format("FromDHCP: expected - {0}, actual - {1}",
                                               testInformation.FromDHCP,
                                               currentInformation.FromDHCP));

                    bAllEquals = bEquals;

                    string actualIpDescription;
                    actualIpDescription = DeviceManagementNetworkTestSuite.DumpNetworkHostArray(currentInformation.NTPManual);

                    NetworkHostType type = testInformation.NTPManual[0].Type;
                    string expectedAddress = testInformation.NTPManual[0].IPv6Address;
                    if (
                        currentInformation.NTPManual.Where(
                            A => A.Type == type && A.IPv6Address == expectedAddress).Count() > 0)
                    {
                        bEquals = true;
                    }
                    else
                    {
                        bEquals = false;
                    }

                    string expectedDescription = string.Format("{0} should be presented",
                                                               testInformation.NTPManual[0].IPv6Address);

                    LogStepEvent(string.Format("NTPManual: expected - {0}, actual - {1}",
                                               expectedDescription, actualIpDescription));

                    bAllEquals = bAllEquals && bEquals;

                    actualIpDescription = DeviceManagementNetworkTestSuite.DumpNetworkHostArray(currentInformation.NTPFromDHCP);
                    if (currentInformation.NTPFromDHCP == null)
                    {
                        bEquals = true;
                    }
                    else
                    {
                        bEquals = (currentInformation.NTPFromDHCP.Length == 0);
                    }

                    expectedDescription = "No NTPFromDHCP";

                    LogStepEvent(string.Format("NTPFromDHCP: expected - {0}, actual - {1}",
                                               expectedDescription, actualIpDescription));

                    bAllEquals = bAllEquals && bEquals;

                    if (!bAllEquals)
                    {
                        throw new AssertException(
                            "Current NTP configuration differs from configuration set in previous step");
                    }

                    StepPassed();

                },
                (originalInformation) =>
                {
                    SetNTP(originalInformation, "Restore NTP configuration");
                    if (ni.IPv6.Config.DHCP != IPv6DHCPConfiguration.Off)
                    {
                        RestoreNetworkInterface(ni.token, ni);
                    }
                }
                );
        }


        [Test(Name = "SET NTP CONFIGURATION - FROMDHCP",
            Order = "02.01.14",
            Id = "2-1-14",
            Category = Category.DEVICE,
            Path = DeviceManagementNetworkTestSuite.PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.NTP },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetNTP })]
        public void SetNtpConfigurationDnsFromDHCPTest()
        {
            NetworkInterface ni = null;

            RunTest<NTPInformation>(
                // Backup action
                new Backup<NTPInformation>(() =>
                {
                    NTPInformation originalInformation = GetNTP();
                    string reason = null;
                    Assert((originalInformation != null) && originalInformation.IsValidNTPInformation(false, out reason),
                        reason == null ? "Failed to get original NTP configuration" : reason,
                        "Check that original NTP configuration returned from the DUT");

                    ni = TurnOnDhcpIpv4();

                    return originalInformation;
                }),
                // Main action
                () =>
                {

                    NTPInformation testInformation = new NTPInformation();
                    testInformation.FromDHCP = true;

                    SetNTP(testInformation);

                    double timeout = ((double)_operationDelay) / 1000;

                    BeginStep(string.Format("Wait {0} seconds to allow the DUT to interact with DHCP server", timeout.ToString("0.000")));
                    Sleep(_operationDelay);
                    StepPassed();

                    NTPInformation actualInformation = GetNTP();
                    string reason = null;
                    Assert((actualInformation != null) && actualInformation.IsValidNTPInformation(false, out reason),
                        reason == null ? "Failed to get current NTP configuration" : reason,
                        "Check that current NTP configuration returned from the DUT");

                    BeginStep("Check current NTP configuration");

                    bool bAllEquals;

                    bool bEquals = (actualInformation.FromDHCP == testInformation.FromDHCP);

                    LogStepEvent(string.Format("FromDHCP: expected - {0}, actual - {1}", testInformation.FromDHCP,
                        actualInformation.FromDHCP));

                    bAllEquals = bEquals;

                    string actualIpDescription;
                    actualIpDescription = DeviceManagementNetworkTestSuite.DumpNetworkHostArray(actualInformation.NTPManual);

                    string expectedAddress = "No Network Hosts";

                    if (actualInformation.NTPManual != null && actualInformation.NTPManual.Count() > 0)
                    {
                        bEquals = false;
                    }
                    else
                    {
                        bEquals = true;
                    }

                    LogStepEvent(string.Format("NTPManual: expected - {0}, actual - {1}",
                         expectedAddress, actualIpDescription));

                    bAllEquals = bAllEquals && bEquals;

                    /*************************************************/

                    actualIpDescription = DeviceManagementNetworkTestSuite.DumpNetworkHostArray(actualInformation.NTPFromDHCP);

                    if (actualInformation.NTPFromDHCP == null)
                    {
                        bEquals = false;
                    }
                    else
                    {
                        bEquals = (actualInformation.NTPFromDHCP.Length > 0);
                    }

                    string expectedDescription = "List of NTP servers";

                    LogStepEvent(string.Format("NTPFromDHCP: expected - {0}, actual - {1}",
                        expectedDescription, actualIpDescription));

                    bAllEquals = bAllEquals && bEquals;

                    /*********************************************************/

                    if (!bAllEquals)
                    {
                        throw new AssertException("Current NTP configuration differs from configuration was set");
                    }

                    StepPassed();
                },
                // Restore action
                (originalInformation) =>
                {
                    // if DHCP was ON, it has not been changed
                    if (!ni.IPv4.Config.DHCP)
                    {
                        // restore network interface configuration
                        RestoreNetworkInterface(ni.token, ni);
                    }

                    SetNTP(originalInformation, "Restore NTP configuration");
                }

                );
        }


        [Test(Name = "SET NTP CONFIGURATION - NTPMANUAL INVALID IPV4",
            Order = "02.01.15",
            Id = "2-1-15",
            Category = Category.DEVICE,
            Path = DeviceManagementNetworkTestSuite.PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.NTP },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetNTP })]
        public void SetInvalidIpv4NtpConfigurationTest()
        {
            RunTest<NetworkInterface>(
                new Backup<NetworkInterface>(
                    () =>
                        {
                            NetworkInterface ni = TurnOffDhcpIpv4();
                            return ni;
                        }),
                () =>
                    {
                        NTPInformation testInformation = new NTPInformation();
                        testInformation.FromDHCP = false;
                        testInformation.NTPManual = new NetworkHost[] {new NetworkHost()};
                        testInformation.NTPManual[0].Type = NetworkHostType.IPv4;
                        testInformation.NTPManual[0].IPv4Address = "10.1.1";

                        RunStep(() => Client.SetNTP(testInformation.FromDHCP, testInformation.NTPManual),
                                "Set NTP configuration - negative test",
                                "Sender/InvalidArgVal/InvalidIPv4Address", true);

                        DoRequestDelay();

                        NTPInformation currentInformation = GetNTP();
                        Assert(currentInformation != null, "The DUT did not return NTP configuration",
                               "Check that NTP information returned from the DUT");

                        string reason;
                        bool bValidConfiguration = currentInformation.IsValidNTPInformation(false,
                                                                                            out reason);

                        Assert(bValidConfiguration, reason, "Validate NTP configuration");

                        bool bFound = (currentInformation.NTPManual != null) &&
                                      (currentInformation.NTPManual.Where(IP => IP.IPv4Address == "10.1.1").
                                           FirstOrDefault() != null);

                        Assert(!bFound, "The DUT returned invalid NTP Manual IP address",
                               "Check if invalid address was not set");

                    },
                (ni) =>
                    {
                        if (ni.IPv4.Config.DHCP)
                        {
                            RestoreNetworkInterface(ni.token, ni);
                        }
                    });

        }


        [Test(Name = "SET NTP CONFIGURATION - NTPMANUAL INVALID IPV6",
            Order = "02.01.16",
            Id = "2-1-16",
            Category = Category.DEVICE,
            Path = DeviceManagementNetworkTestSuite.PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.NTP, Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetNTP })]
        public void SetInvalidIpv6NtpConfigurationTest()
        {
            RunTest<NetworkInterface>(
                new Backup<NetworkInterface>(
                    () =>
                        {
                            NetworkInterface ni = TurnOffDhcpIpv6();
                            return ni;
                        }),
                () =>
                    {
                        NTPInformation testInformation = new NTPInformation();
                        testInformation.FromDHCP = false;
                        testInformation.NTPManual = new NetworkHost[] {new NetworkHost()};
                        testInformation.NTPManual[0].Type = NetworkHostType.IPv6;
                        testInformation.NTPManual[0].IPv6Address = "FF02:1";

                        RunStep(() => Client.SetNTP(testInformation.FromDHCP, testInformation.NTPManual),
                                "Set NTP configuration - negative test",
                                "Sender/InvalidArgVal/InvalidIPv6Address", true);

                        DoRequestDelay();

                        NTPInformation currentInformation = GetNTP();
                        Assert(currentInformation != null, "The DUT did not return NTP configuration",
                               "Check that NTP information returned from the DUT");

                        string reason;
                        bool bValidConfiguration = currentInformation.IsValidNTPInformation(false, out reason);

                        Assert(bValidConfiguration, reason, "Validate NTP configuration");

                        bool bFound = (currentInformation.NTPManual != null) &&
                                      (currentInformation.NTPManual.Where(IP => IP.IPv6Address == "FF02:1").
                                           FirstOrDefault() != null);

                        Assert(!bFound, "The DUT returned invalid NTP Manual IP address",
                               "Check if invalid address was not set");

                    },
                (ni) =>
                    {
                        if (ni.IPv6.Config.DHCP != IPv6DHCPConfiguration.Off)
                        {
                            RestoreNetworkInterface(ni.token, ni);
                        }
                    });
        }

    }
}
