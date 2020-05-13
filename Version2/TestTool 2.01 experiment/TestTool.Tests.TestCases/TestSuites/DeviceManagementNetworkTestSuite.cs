///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Linq;
using System.Text;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Device;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class DeviceManagementNetworkTestSuite : Base.DeviceManagementTest
    {
        public DeviceManagementNetworkTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Device Management\\Network";
        
        [Test(Name = "NETWORK COMMAND HOSTNAME CONFIGURATION",
            Order = "06.02.01",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
        public void HostnameInformationTest()
        {
            RunTest(() =>
            {
                HostnameInformation hostnameInformation = GetHostname();

                string stepDetails = null;
                if (hostnameInformation != null)
                {
                    stepDetails = string.Format("Hostname information: FromDHCP = {0}, Name = {1}",
                                                hostnameInformation.FromDHCP,
                                                hostnameInformation.Name == null ? "NULL" : string.Format("'{0}'", hostnameInformation.Name));
                }

                Assert(hostnameInformation != null,
                    "Hostname information not found",
                    "Check that hostname information returned from the DUT",
                    stepDetails);

                if (hostnameInformation.Name != null)
                {
                    Assert(hostnameInformation.Name.IsValidHostname(),
                           string.Format("Hostname ({0}) is invalid", hostnameInformation.Name),
                           string.Format("Validate hostname ('{0}')", hostnameInformation.Name));
                }

            });

        }

        [Test(Name = "NETWORK COMMAND SETHOSTNAME TEST",
            Order = "06.02.02",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
        public void SetHostnameTest()
        {

            RunTest<HostnameInformation>(
                (new Backup<HostnameInformation>(
                    () =>
                        {
                            HostnameInformation original = GetHostname();

                            Assert(original != null, "Cannot receive current hostname", "Check that the DUT returned current hostname");

                            return original;
                        })), 
                    () =>
                         {
                            string hostname = "Onvif-Test0-oNvif-Onv123-Onvif123-Onvif123-Onvif123-Onvif12-Onv";
                            //string hostname = "OnvifTest";

                            SetHostname(hostname);

                            HostnameInformation actual = GetHostname();

                            Assert(actual != null, "Cannot receive current hostname", "Check that the DUT returned current hostname");

                            string afterSet = actual.Name;

                            Assert(afterSet == hostname, string.Format("Hostname returned from the DUT ({0}) does not match one was set ({1})", afterSet, hostname), "Verify that hostname has been changed");

                            Assert(actual.FromDHCP == false, string.Format("FromDHCP returned from the DUT is TRUE"), "Verify that FromDHCP is false");
                             
                         }, 
                         (original) =>
                             {
                                 System.Diagnostics.Debug.WriteLine("Before SET (restore)");

                                 SetHostname(original.Name, "Restore hostname");

                                 System.Diagnostics.Debug.WriteLine("After SET (restore)");

                             });

        }

        [Test(Name = "NETWORK COMMAND SETHOSTNAME TEST ERROR CASE",
            Order = "06.02.03",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Should)]
        public void SetInvalidHostnameTest()
        {
            RunTest( () =>
                         {
                            HostnameInformation hostnameInformation = GetHostname();

                            Assert(hostnameInformation != null, 
                                "The DUD did not return current hostname information", 
                                "Check that the DUT returned current hostname information");

                            string configuredHostName = hostnameInformation.Name;
                            bool fromDHCP = hostnameInformation.FromDHCP;

                            /******** INVALID VALUE *********/
                            string hostname = "Onvif_test1";

                            RunStep( () => { Client.SetHostname(hostname);}, "Set Hostname", "Sender/InvalidArgVal/InvalidHostname", true);
                             
                            HostnameInformation actual = GetHostname();

                            Assert(actual != null, "Cannot receive current hostname", "Check that current hostname returned from the DUT");

                            string currentHostname = actual.Name;

                            string details = string.Format("Expected: {0}, actual: {1}", configuredHostName, currentHostname);

                            Assert((configuredHostName == currentHostname),
                                   "Hostname has been changed after invalid request",
                                   "Verify that hostname has not been changed",
                                   details);

                            details = string.Format("Expected: {0}, actual: {1}", fromDHCP, actual.FromDHCP);

                            Assert((fromDHCP == actual.FromDHCP),
                                   "FromDHCP has been changed after invalid request",
                                   "Verify that FromDHCP has not been changed",
                                   details);


                            /************* TOO LONG VALUE ************/

                             /* Removed accordingly to CR 33*/

                             /*
                            hostname = "VeryVeryLongHostnameVeryVeryLongHostnameVeryVeryLongHostnameVeryVeryLongHostnameVeryVeryLongHostnameVeryVeryLongHostnameVeryVeryLongHostnameVeryVeryLongHostnameVeryVeryLongHostnameVeryVeryLongHostnameVeryVeryLongHostnameVeryVeryLongHostnameVeryLongHostname";

                            fault = false;
                            reason = "The DUT did not return SOAP FAULT";

                            RunStep(() => { Client.SetHostname(hostname); }, "Set Hostname", "Sender/InvalidArgVal/InvalidHostname", true);

                            actual = GetHostname();

                            Assert(actual != null, "Cannot receive current hostname", "Check that the DUT returned current hostname information");

                            currentHostname = actual.Name;

                            details = string.Format("Expected: {0}, actual: {1}", configuredHostName, currentHostname);

                            Assert((configuredHostName == currentHostname),
                                   "Hostname has been changed after invalid request",
                                   "Verify that hostname has not been changed",
                                   details);

                            details = string.Format("Expected: {0}, actual: {1}", fromDHCP, actual.FromDHCP);

                            Assert((fromDHCP == actual.FromDHCP),
                                   "FromDHCP has been changed after invalid request",
                                   "Verify that FromDHCP has not been changed",
                                   details);         */                    
                         });

        }

        [Test(Name = "GET DNS CONFIGURATION",
            Order = "06.02.04",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
        public void GetDnsConfigurationTest()
        {
            RunTest( () =>
                         {
                            DNSInformation dnsInformation = GetDnsConfiguration();

                            Assert(dnsInformation != null, 
                                "DNSInformation not returned",
                                "Check that DUT returned DNSInformation");
                            
                            string error;
                            bool dnsInformationOk = dnsInformation.IsValidDnsInformation(false, out error);

                            Assert(dnsInformationOk, error, "Validate DNS information");
                             
                         });

        }

//#if FULL
        [Test(Name = "SET DNS CONFIGURATION - SEARCHDOMAIN",
            Order = "06.02.05",
            Path = PATH,
            Version = 1.02,
            Services = new Service[]{ Service.Device},
            RequirementLevel = RequirementLevel.Must)]
//#endif
        public void SetDnsConfigurationSearchDomainTest()
        {
            RunTest<DNSInformation>(

                new Backup<DNSInformation>( () => 
                    {
                        DNSInformation originalInformation = GetDnsConfiguration();
                        Assert(originalInformation != null, "Failed to get original DNS configuration", "Check that original DNS configuration returned from the DUT");
                        return originalInformation;
                    }),

                () =>
                    {
                        DNSInformation testInformation = new DNSInformation();
                        testInformation.SearchDomain = new string[]{"domain.name"};
                        testInformation.FromDHCP = false;

                        SetDnsConfiguration(testInformation);

                        double timeout = ((double)_operationDelay) / 1000;

                        BeginStep(string.Format("Wait {0} seconds to allow the DUT to apply settings", timeout.ToString("0.000")));
                        Sleep(_operationDelay);
                        StepPassed();

                        DNSInformation actual = GetDnsConfiguration();

                        Assert(actual != null, "Failed to get current DNS configuration", "Check that DNS configuration returned from the DUT");

                        Assert(actual.FromDHCP == false, "FromDHCP is TRUE", "Check that FromDHCP is false");

                        Assert(actual.SearchDomain != null, "No Search Domains returned from the DUT", "Check that the DUT returned Search Domains");

                        string actualDomainsDescription = DumpStringArray(actual.SearchDomain);

                        bool domainFound = actual.SearchDomain.Contains(testInformation.SearchDomain[0],
                                                                        StringComparer.InvariantCultureIgnoreCase);

                        Assert(domainFound, 
                            string.Format("SearchDomain does not equal to the value was set. "), 
                            "Validate SearchDomain value", 
                            string.Format("Expected: '{0}' should be presented, actual: {1}", testInformation.SearchDomain[0], actualDomainsDescription));


                    },

                (originalInformation) =>
                    {
                        SetDnsConfiguration(originalInformation, "Restore DNS configuration");
                    }

                );

        }
        
        [Test(Name = "SET DNS CONFIGURATION - DNSMANUAL IPV4",
            Order = "06.02.06",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
        public void SetDnsConfigurationTest()
        {
            RunTest<DNSInformation>(

                new Backup<DNSInformation>( () => 
                    {
                        DNSInformation originalInformation = GetDnsConfiguration();
                        Assert(originalInformation != null, "Failed to get original DNS configuration", "Check that original DNS configuration returned from the DUT");
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
                    actualIpDescription = DumpIPArray(actualInformation.DNSManual);

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

                    actualIpDescription = DumpIPArray(actualInformation.DNSFromDHCP);

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

                    // ToDo : remove search domain validation.
                    // The DUT may leave old Search Domain (TT Release Notes)
                    //expectedDescription = "no Search Domains";
                    //string actualDomainsDescription = DumpStringArray(actualInformation.SearchDomain);
                    //if (actualInformation.SearchDomain == null)
                    //{
                    //    bEquals = true;
                    //}
                    //else
                    //{
                    //    bEquals = (actualInformation.SearchDomain.Length == 0);
                    //}

                    //LogStepEvent(string.Format("SearchDomain: expected - {0}, actual - {1}",
                    //expectedDescription, actualDomainsDescription));

                    //bAllEquals = bAllEquals && bEquals;

                    /*********************************************************/

                    if (!bAllEquals)
                    {
                        throw new AssertException("Current DNS configuration differs from configuration was set");
                    }

                    StepPassed();
                },

                (originalInformation) => { SetDnsConfiguration(originalInformation, "Restore DNS configuration"); }
                
                );
        }

//#if FULL
        [Test(Name = "SET DNS CONFIGURATION - DNSMANUAL IPV6",
            Order = "06.02.07",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.IPv6 })]
//#endif
        public void SetDnsConfigurationDnsManualIpv6Test()
        {
            RunTest<DNSInformation>(

                new Backup<DNSInformation>(() =>
                {
                    DNSInformation originalInformation = GetDnsConfiguration();
                    Assert(originalInformation != null, "Failed to get original DNS configuration", "Check that original DNS configuration returned from the DUT");
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
                    actualIpDescription = DumpIPArray(actualInformation.DNSManual);

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

                    actualIpDescription = DumpIPArray(actualInformation.DNSFromDHCP);

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

                    //expectedDescription = "no Search Domains";
                    //string actualDomainsDescription = DumpStringArray(actualInformation.SearchDomain);
                    //if (actualInformation.SearchDomain == null)
                    //{
                    //    bEquals = true;
                    //}
                    //else
                    //{
                    //    bEquals = (actualInformation.SearchDomain.Length == 0);
                    //}

                    //LogStepEvent(string.Format("SearchDomain: expected - {0}, actual - {1}",
                    //expectedDescription, actualDomainsDescription));

                    //bAllEquals = bAllEquals && bEquals;

                    /*********************************************************/

                    if (!bAllEquals)
                    {
                        throw new AssertException("Current DNS configuration differs from configuration was set");
                    }

                    StepPassed();
                },

                (originalInformation) => { SetDnsConfiguration(originalInformation, "Restore DNS configuration"); }

                );
        }

//#if FULL
        [Test(Name = "SET DNS CONFIGURATION - FROM DHCP",
            Order = "06.02.08",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
//#endif
        public void SetDnsConfigurationDnsFromDHCPTest()
        {
            RunTest<DNSInformation>(

                new Backup<DNSInformation>(() =>
                {
                    DNSInformation originalInformation = GetDnsConfiguration();
                    string reason = null;
                    Assert((originalInformation != null) && originalInformation.IsValidDnsInformation(false, out reason), 
                        reason == null ? "Failed to get original DNS configuration" : reason, 
                        "Check that valid DNS configuration returned from the DUT");
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
                    actualIpDescription = DumpIPArray(actualInformation.DNSManual);

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

                    actualIpDescription = DumpIPArray(actualInformation.DNSFromDHCP);

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

                    /* No need to check search domains, because we don't know if domains were set via DHCP
                    expectedDescription = "no Search Domains";
                    string actualDomainsDescription = DumpStringArray(actualInformation.SearchDomain);
                    if (actualInformation.SearchDomain == null)
                    {
                        bEquals = true;
                    }
                    else
                    {
                        bEquals = (actualInformation.SearchDomain.Length == 0);
                    }

                    LogStepEvent(string.Format("SearchDomain: expected - {0}, actual - {1}",
                    expectedDescription, actualDomainsDescription));

                    bAllEquals = bAllEquals && bEquals;
                    */
                      
                    if (!bAllEquals)
                    {
                        throw new AssertException("Current DNS configuration differs from configuration was set");
                    }

                    StepPassed();
                },

                (originalInformation) => 
                { 
                    SetDnsConfiguration(originalInformation, "Restore DNS configuration"); 
                }

                );
        }
        
        [Test(Name = "SET DNS CONFIGURATION - DNS MANUAL INVALID IPV4",
            Order = "06.02.09",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Should)]
        public void SetInvalidDnsConfigurationTest()
        {
            RunTest( () =>
                         {
                            DNSInformation testInformation = new DNSInformation();
                            testInformation.FromDHCP = false;
                            testInformation.DNSManual =
                                new IPAddress[]
                                    {
                                        new IPAddress(){IPv4Address = "10.1.1", Type = IPType.IPv4}
                                    };

                            string reason = string.Empty;
                            RunStep(() =>
                                        {
                                            Client.SetDNS(testInformation.FromDHCP, testInformation.SearchDomain,
                                                          testInformation.DNSManual);
                                        },  
                                        "Set DNS configuration - negative test",
                                        "Sender/InvalidArgVal/InvalidIPv4Address", true);
                            
                            DNSInformation actualInformation = GetDnsConfiguration();

                            Assert(actualInformation != null, "Failed to get current DNS configuration", "Check that current DNS configuration returned from the DUT");

                            bool bValid = actualInformation.IsValidDnsInformation(false, out reason);
                            Assert(bValid, reason, "Validate current DNS configuration");

                            bool bFound =(actualInformation.DNSManual != null && actualInformation.DNSManual.Length > 0) &&
                                actualInformation.DNSManual.Where(IP => IP.IPv4Address == testInformation.DNSManual[0].IPv4Address).
                                    Count() > 0;

                            Assert(!bFound, "DUT set address to invalid value", "Check that current IPv4 addresses list does not containd invalid value");
                             
                         });

        }

//#if FULL
        [Test(Name = "SET DNS CONFIGURATION - DNS MANUAL INVALID IPV6",
            Order = "06.02.10",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.ConditionalShould, 
            RequiredFeatures = new Feature[]{Feature.IPv6})]
//#endif
        public void SetInvalidIpv6DnsConfigurationTest()
        {
            RunTest(() =>
            {
                DNSInformation testInformation = new DNSInformation();
                testInformation.FromDHCP = false;
                testInformation.DNSManual =
                    new IPAddress[]
                                    {
                                        new IPAddress(){IPv6Address = "FF02:1", Type = IPType.IPv6}
                                    };

                string reason = string.Empty;
                RunStep(() =>
                {
                    Client.SetDNS(testInformation.FromDHCP, testInformation.SearchDomain,
                                  testInformation.DNSManual);
                },
                            "Set DNS configuration - negative test",
                            "Sender/InvalidArgVal/InvalidIPv6Address");

                DNSInformation actualInformation = GetDnsConfiguration();

                Assert(actualInformation != null, "Failed to get current DNS configuration", "Check that current DNS configuration returned from the DUT");

                bool bValid = actualInformation.IsValidDnsInformation(false, out reason);
                Assert(bValid, reason, "Validate current DNS configuration");

                bool bFound = (actualInformation.DNSManual != null && actualInformation.DNSManual.Length > 0) &&
                    actualInformation.DNSManual.Where(IP => IP.IPv4Address == testInformation.DNSManual[0].IPv6Address).
                        Count() > 0;

                Assert(!bFound, "DUT set address to invalid value", "Check that current IPv6 addresses list does not contain invalid value");

            });

        }
        
        [Test(Name = "GET NTP CONFIGURATION",
            Order = "06.02.11",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] {Feature.NTP})]
        public void GetNtpConfigurationTest()
        {
            RunTest( () =>
                         {
                            NTPInformation ntpInformation = GetNTP();

                            Assert(ntpInformation != null, "NTP information not returned", "Check that DUT returned NTP information");

                            string reason = string.Empty;

                            bool valid = ntpInformation.IsValidNTPInformation(false, out reason);

                            Assert(valid, reason, "Validate NTP information");
                             
                         });

        }

        [Test(Name = "SET NTP CONFIGURATION - NTP MANUAL IPV4",
            Order = "06.02.12",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] {Feature.NTP})]
        public void SetNtpConfigurationTest()
        {
            RunTest<NTPInformation>(
                new Backup<NTPInformation>(() => 
                {
                    NTPInformation originalInformation = GetNTP();
                    Assert(originalInformation != null, "The DUT did not return NTP configuration", "Check that DUT returned NTP information");
                    return originalInformation;
                }),  
                new Action( () =>
                            {
                                NTPInformation testInformation = new NTPInformation();
                                testInformation.FromDHCP = false;
                                testInformation.NTPManual = new NetworkHost[]{new NetworkHost()};
                                testInformation.NTPManual[0].Type = NetworkHostType.IPv4;
                                testInformation.NTPManual[0].IPv4Address = _environmentSettings.NtpIpv4;
                                
                                SetNTP(testInformation);

                                NTPInformation currentInformation = GetNTP();
                                Assert(currentInformation != null, "The DUT did not return NTP configuration", "Check that DUT returned NTP information");
                                
                                BeginStep("Validate current NTP configuration");

                                bool bAllEquals;

                                bool bEquals = (currentInformation.FromDHCP == testInformation.FromDHCP);

                                LogStepEvent(string.Format("FromDHCP: expected - {0}, actual - {1}", testInformation.FromDHCP,
                                    currentInformation.FromDHCP));

                                bAllEquals = bEquals;

                                string actualIpDescription;
                                actualIpDescription = DumpNetworkHostArray(currentInformation.NTPManual);

                                NetworkHostType type = testInformation.NTPManual[0].Type;
                                string expectedAddress = testInformation.NTPManual[0].IPv4Address;
                                if (currentInformation.NTPManual.Where(A => A.Type == type && A.IPv4Address == expectedAddress).Count() > 0)
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

                                actualIpDescription = DumpNetworkHostArray(currentInformation.NTPFromDHCP);
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
                                    throw new AssertException("Current NTP configuration differs from configuration set in previous step");
                                }
                                
                                StepPassed();
                                    
                                }), 
                                new Action<NTPInformation>( 
                                    (originalInformation) =>
                                    {
                                        SetNTP(originalInformation, "Restore NTP configuration");
                                    })
                                );
        }

//#if FULL
        [Test(Name = "SET NTP CONFIGURATION - NTP MANUAL IPV6",
            Order = "06.02.13",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.NTP, Feature.IPv6 })]
//#endif
        public void SetNtpIpv6ConfigurationTest()
        {
            RunTest<NTPInformation>(
                new Backup<NTPInformation>(() =>
                {
                    NTPInformation originalInformation = GetNTP();
                    Assert(originalInformation != null, "The DUT did not return NTP configuration", "Check that DUT returned NTP information");
                    return originalInformation;
                }),
                new Action(() =>
                {
                    NTPInformation testInformation = new NTPInformation();
                    testInformation.FromDHCP = false;
                    testInformation.NTPManual = new NetworkHost[] { new NetworkHost() };
                    testInformation.NTPManual[0].Type = NetworkHostType.IPv6;
                    testInformation.NTPManual[0].IPv6Address = _environmentSettings.NtpIpv6;

                    SetNTP(testInformation);

                    NTPInformation currentInformation = GetNTP();
                    Assert(currentInformation != null, "The DUT did not return NTP configuration", "Check that DUT returned NTP information");

                    BeginStep("Validate current NTP configuration");

                    bool bAllEquals;

                    bool bEquals = (currentInformation.FromDHCP == testInformation.FromDHCP);

                    LogStepEvent(string.Format("FromDHCP: expected - {0}, actual - {1}", testInformation.FromDHCP,
                        currentInformation.FromDHCP));

                    bAllEquals = bEquals;

                    string actualIpDescription;
                    actualIpDescription = DumpNetworkHostArray(currentInformation.NTPManual);

                    NetworkHostType type = testInformation.NTPManual[0].Type;
                    string expectedAddress = testInformation.NTPManual[0].IPv6Address;
                    if (currentInformation.NTPManual.Where(A => A.Type == type && A.IPv6Address == expectedAddress).Count() > 0)
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

                    actualIpDescription = DumpNetworkHostArray(currentInformation.NTPFromDHCP);
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
                        throw new AssertException("Current NTP configuration differs from configuration set in previous step");
                    }

                    StepPassed();

                }),
                                new Action<NTPInformation>(
                                    (originalInformation) =>
                                    {
                                        SetNTP(originalInformation, "Restore NTP configuration");
                                    })
                                );
        }

//#if FULL
        [Test(Name = "SET NTP CONFIGURATION - FROM DHCP",
            Order = "06.02.14",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.NTP })]
//#endif
        public void SetNtpConfigurationDnsFromDHCPTest()
        {
            RunTest<NTPInformation>(
                // Backup action
                new Backup<NTPInformation>(() =>
                {
                    NTPInformation originalInformation = GetNTP();
                    string reason = null;
                    Assert((originalInformation != null) && originalInformation.IsValidNTPInformation(false, out reason),
                        reason == null ? "Failed to get original NTP configuration" : reason,
                        "Check that original NTP configuration returned from the DUT");
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
                    actualIpDescription = DumpNetworkHostArray(actualInformation.NTPManual);

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

                    actualIpDescription = DumpNetworkHostArray(actualInformation.NTPFromDHCP);

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
                (originalInformation) => { SetNTP(originalInformation, "Restore NTP configuration"); }

                );
        }
        
        [Test(Name = "SET NTP CONFIGURATION - NTP MANUAL INVALID IPV4",
            Order = "06.02.15",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.ConditionalShould,
            RequiredFeatures  = new Feature[]{Feature.NTP})]
        public void SetInvalidIpv4NtpConfigurationTest()
        {
            RunTest( () =>
                         {
                            NTPInformation testInformation = new NTPInformation();
                            testInformation.FromDHCP = false;
                            testInformation.NTPManual = new NetworkHost[] { new NetworkHost() };
                            testInformation.NTPManual[0].Type = NetworkHostType.IPv4;
                            testInformation.NTPManual[0].IPv4Address = "10.1.1";

                            RunStep(() => Client.SetNTP(testInformation.FromDHCP, testInformation.NTPManual), 
                                "Set NTP configuration - negative test",
                                "Sender/InvalidArgVal/InvalidIPv4Address", true);

                            NTPInformation currentInformation = GetNTP();
                            Assert(currentInformation != null, "The DUT did not return NTP configuration", "Check that NTP information returned from the DUT");

                            string reason;
                            bool bValidConfiguration = currentInformation.IsValidNTPInformation(false, out reason);

                            Assert(bValidConfiguration, reason, "Validate NTP configuration");

                            bool bFound = (currentInformation.NTPManual != null) &&
                                (currentInformation.NTPManual.Where(IP => IP.IPv4Address == "10.1.1").FirstOrDefault() != null);

                            Assert(!bFound, "The DUT returned invalid NTP Manual IP address", "Check if invalid address was not set");
                                          

                         });

        }

//#if FULL
        [Test(Name = "SET NTP CONFIGURATION - NTP MANUAL INVALID IPV6",
            Order = "06.02.16",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.ConditionalShould,
            RequiredFeatures = new Feature[] { Feature.NTP, Feature.IPv6 })]
//#endif
        public void SetInvalidIpv6NtpConfigurationTest()
        {
            RunTest(() =>
            {
                NTPInformation testInformation = new NTPInformation();
                testInformation.FromDHCP = false;
                testInformation.NTPManual = new NetworkHost[] { new NetworkHost() };
                testInformation.NTPManual[0].Type = NetworkHostType.IPv6;
                testInformation.NTPManual[0].IPv6Address = "FF02:1";

                RunStep(() => Client.SetNTP(testInformation.FromDHCP, testInformation.NTPManual),
                    "Set NTP configuration - negative test",
                    "Sender/InvalidArgVal/InvalidIPv6Address");

                NTPInformation currentInformation = GetNTP();
                Assert(currentInformation != null, "The DUT did not return NTP configuration", "Check that NTP information returned from the DUT");

                string reason;
                bool bValidConfiguration = currentInformation.IsValidNTPInformation(false, out reason);

                Assert(bValidConfiguration, reason, "Validate NTP configuration");

                bool bFound = (currentInformation.NTPManual != null) &&
                    (currentInformation.NTPManual.Where(IP => IP.IPv6Address == "FF02:1").FirstOrDefault() != null);

                Assert(!bFound, "The DUT returned invalid NTP Manual IP address", "Check if invalid address was not set");


            });

        }

//#if FULL
        [Test(Name = "GET NETWORK INTERFACE CONFIGURATION",
            Order = "06.02.17",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
//#endif
        public void GetNetworkInterfacesTest()
        {
            RunTest( () =>
                         {
                             NetworkInterface[] interfaces = GetNetworkInterfaces();

                             Assert(interfaces != null, 
                                 "The DUT did not return Network Interfaces", 
                                 "Check if Network Interfaces returned from the DUT");

                             // possible validate something else ?
                         });
        }

//#if FULL
        [Test(Name = "SET NETWORK INTERFACE CONFIGURATION - INVALID IPV4",
            Order = "06.02.20",
            Path = PATH,
            Version = 1.02,
            Services = new Service[]{ Service.Device},
            RequirementLevel = RequirementLevel.Should)]
//#endif
        public void SetNetworkInterfaceInvalidIPv4Test()
        {
            RunTest( () =>
                         {
                             NetworkInterface[] interfaces = GetNetworkInterfaces();

                             Assert(interfaces != null && interfaces.Length > 0,
                                 "No interfaces returned from the DUT",
                                 "Check that the DUT returned current interfaces");

                             string interfaceToken = null;
                             foreach (NetworkInterface ni in interfaces)
                             {
                                 if (ni.IPv4 != null)
                                 {
                                     interfaceToken = ni.token;
                                     break;
                                 }
                             }

                             Assert(interfaceToken != null, 
                                 "No appropriate interface found", 
                                 "Check if an interface with IPv4 configuration is presented");

                             NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
                             configuration.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
                             configuration.IPv4.Enabled = true;
                             configuration.IPv4.EnabledSpecified = true;

                             configuration.IPv4.Manual = 
                                 new PrefixedIPv4Address[] { new PrefixedIPv4Address(){Address = "10.1.1"}};

                             RunStep(() => { Client.SetNetworkInterfaces(interfaceToken, configuration); },
                                     "Set Network Interfaces - negative test", "Sender/InvalidArgVal/InvalidIPv4Address");


                             interfaces = GetNetworkInterfaces();

                             NetworkInterface modifiedInterface =
                                 interfaces.Where(NI => NI.token == interfaceToken).FirstOrDefault();

                             Assert(modifiedInterface != null,
                                    string.Format("Interfaces with token = '{0}' not found", interfaceToken),
                                    string.Format("Check if an interface with token = '{0}' is presented", interfaceToken));

                             BeginStep(string.Format("Check that interface with token '{0}' has not been changed", interfaceToken));

                             bool passed = false;
                             if (modifiedInterface.IPv4 == null )
                             {
                                 LogStepEvent("IPv4 configuration not found");
                             }
                             else
                             {
                                 if ((modifiedInterface.IPv4.Config != null) &&
                                     (modifiedInterface.IPv4.Config.Manual != null))
                                 {
                                     if (modifiedInterface.IPv4.Config.Manual.Where( A => A.Address == "10.1.1").FirstOrDefault() != null)
                                     {
                                         LogStepEvent("Invalid IP address 10.1.1 is present in the interface configuration");
                                     }
                                     else
                                     {
                                         passed = true;
                                     }
                                 }
                                 else
                                 {
                                     passed = true;
                                 }
                             }
                             if (!passed)
                             {
                                 throw new AssertException("Interface has been changed");
                             }

                             StepPassed();


                         });
        }

//#if FULL
        [Test(Name = "SET NETWORK INTERFACE CONFIGURATION - INVALID IPV6",
            Order = "06.02.21",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.ConditionalShould,
            RequiredFeatures = new Feature[] { Feature.IPv6 })]
//#endif
        public void SetNetworkInterfaceInvalidIPv6Test()
        {
            RunTest(() =>
            {
                NetworkInterface[] interfaces = GetNetworkInterfaces();

                Assert(interfaces != null && interfaces.Length > 0,
                    "No interfaces returned from the DUT",
                    "Check that the DUT returned current interfaces");

                string interfaceToken = null;
                foreach (NetworkInterface ni in interfaces)
                {
                    if (ni.IPv6 != null)
                    {
                        interfaceToken = ni.token;
                        break;
                    }
                }

                Assert(interfaceToken != null,
                    "No appropriate interface found",
                    "Check that an interface with IPv6 configuration is presented");

                NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
                configuration.IPv6 = new IPv6NetworkInterfaceSetConfiguration();
                configuration.IPv6.Enabled = true;
                configuration.IPv6.EnabledSpecified = true;

                string invalidAddress = "FF02:1";

                configuration.IPv6.Manual =
                    new PrefixedIPv6Address[] { new PrefixedIPv6Address() { Address = invalidAddress } };

                RunStep(() => { Client.SetNetworkInterfaces(interfaceToken, configuration); },
                        "Set Network Interfaces - negative test", "Sender/InvalidArgVal/InvalidIPv6Address");


                interfaces = GetNetworkInterfaces();

                Assert(interfaces != null && interfaces.Length > 0,
                    "No interfaces returned from the DUT",
                    "Check that the DUT returned current interfaces");

                NetworkInterface modifiedInterface =
                    interfaces.Where(NI => NI.token == interfaceToken).FirstOrDefault();

                Assert(modifiedInterface != null,
                       string.Format("Interfaces with token = '{0}' not found", interfaceToken),
                       string.Format("Check if an interface with token = '{0}' is presented", interfaceToken));


                BeginStep(string.Format("Check that interface with token '{0}' has not been changed", interfaceToken));

                bool passed = false;
                if (modifiedInterface.IPv6 == null)
                {
                    LogStepEvent("IPv6 configuration not found");
                }
                else
                {
                    if ( (modifiedInterface.IPv6.Config != null) &&
                         (modifiedInterface.IPv6.Config.Manual != null) &&
                         (modifiedInterface.IPv6.Config.Manual.Where(A => A.Address == invalidAddress).FirstOrDefault() != null))
                    {
                        LogStepEvent(string.Format("Invalid IP address {0} is present in the interface configuration", invalidAddress));
                    }
                    else
                    {
                        passed = true;
                    }
                }
                if (!passed)
                {
                    throw new AssertException("Interface has been changed");
                }

                StepPassed();


            });
        }

//#if FULL
        [Test(Name = "GET NETWORK PROTOCOLS CONFIGURATION",
            Order = "06.02.22",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
//#endif
        public void GetNetworkProtocolsTest()
        {
            RunTest( () =>
                         {
                             NetworkProtocol[] protocols = GetNetworkProtocols();

                             Assert(protocols != null, 
                                 "The DUT did not send Network Protocols", 
                                 "Check if network protocols returned from the DUT");

                             // ToDo: may be check also "enabled" 
                             bool rtspFound =
                                 protocols.Where(p => p.Name == NetworkProtocolType.RTSP).FirstOrDefault() != null;
                             Assert(rtspFound, "RTSP not found", "Check if RTSP is present in the list");

                             bool httpFound =
                                protocols.Where(p => p.Name == NetworkProtocolType.HTTP).FirstOrDefault() != null;
                             Assert(httpFound, "HTTP not found", "Check if HTTP is present in the list");
                         });

        }

//#if FULL
        [Test(Name = "SET NETWORK PROTOCOLS CONFIGURATION",
            Order = "06.02.23",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
//#endif
        public void SetNetworkProtocolsTest()
        {
            RunTest<NetworkProtocol[]>(
                    new Backup<NetworkProtocol[]>(() =>
                        {
                            return GetNetworkProtocols();
                        }), 
                    () =>
                        {
                            int port = 10554;
                            NetworkProtocol rtsp = new NetworkProtocol() { Enabled = true, Name = NetworkProtocolType.RTSP, Port = new int[] {port}};

                            SetNetworkProtocols(new NetworkProtocol[]{rtsp});

                            NetworkProtocol[] currentProtocols = GetNetworkProtocols();
                             
                            // validate
                            NetworkProtocol protocol =
                                currentProtocols.FirstOrDefault(
                                    p =>
                                    (p.Name == NetworkProtocolType.RTSP) && (p.Port != null) && (p.Port.Contains(port)));
                            Assert((protocol != null) && protocol.Enabled, 
                                string.Format("Protocol [Name=RTSP, Port={0}] ", 10554) + (protocol == null ? "not found" : "disabled"),
                                "Validating protocols");

                            rtsp = new NetworkProtocol() { Enabled = false, Name = NetworkProtocolType.RTSP, Port = new int[] {10554 } };

                            SetNetworkProtocols(new NetworkProtocol[] { rtsp });

                            currentProtocols = GetNetworkProtocols();

                            // validate
                            protocol = currentProtocols.FirstOrDefault(p => (p.Name == NetworkProtocolType.RTSP) );
                            Assert((protocol != null) && !protocol.Enabled,
                                "Protocol [Name=RTSP] " + (protocol == null ? "not found" : "enabled, while should be disabled"),
                                "Validating protocols");

                        },  
                    (networkProtocols) =>
                         {
                             SetNetworkProtocols(networkProtocols);   
                         });

        }




        [Test(Name = "SET NETWORK PROTOCOLS CONFIGURATION - UNSUPPORTED PROTOCOLS",
            Order = "06.02.24",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Should)]
        public void SetNetworkUnsupportedProtocolsTest()
        {
            RunTest(
                () =>
                    {
                        Capabilities capabilities = GetCapabilities(new CapabilityCategory[] {CapabilityCategory.Device});

                        Assert(capabilities != null, "Ñapabilities not found", "Check that DUT returned capabilities");

                        Assert(capabilities.Device != null, "Device capabilities not found",
                               "Check that DUT returned device capabilities");

                        bool skip = false;

                        if (capabilities.Device.Security != null)
                        {
                            skip = capabilities.Device.Security.TLS11 || capabilities.Device.Security.TLS12;
                            if (!skip )
                            {
                                if (capabilities.Device.Security.Extension != null)
                                {
                                    skip = capabilities.Device.Security.Extension.TLS10;
                                }
                                else
                                {
                                    if (capabilities.Device.Security.Any != null)
                                    {
                                        foreach (System.Xml.XmlElement child in capabilities.Device.Security.Any)
                                        {
                                            System.Xml.XmlElement element = child as System.Xml.XmlElement;
                                            if (element != null)
                                            {
                                                if (element.LocalName == "Extension" &&
                                                    element.NamespaceURI == "http://www.onvif.org/ver10/schema")
                                                {
                                                    //
                                                    foreach (System.Xml.XmlNode childNode in element.ChildNodes)
                                                    {
                                                        System.Xml.XmlElement childElement =
                                                            childNode as System.Xml.XmlElement;
                                                        if (childElement != null)
                                                        {
                                                            if (childElement.LocalName == "TLS1.0" &&
                                                                childElement.NamespaceURI == "http://www.onvif.org/ver10/schema")
                                                            {
                                                                bool tls10 = false;
                                                                bool valid = bool.TryParse(childElement.InnerText,
                                                                                           out tls10);

                                                                skip = tls10;
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (skip)
                        {
                            LogTestEvent("HTTPS supported, skip the test");
                        }
                        else
                        {
                            NetworkProtocol https = new NetworkProtocol()
                                                        {
                                                            Enabled = true,
                                                            Name = NetworkProtocolType.HTTPS,
                                                            Port = new int[] {10554}
                                                        };

                            RunStep(() => { Client.SetNetworkProtocols(new NetworkProtocol[] {https}); },
                                    "Set Network Protocols - negative test",
                                    "Sender/InvalidArgVal/ServiceNotSupported");

                            NetworkProtocol[] currentProtocols = GetNetworkProtocols();

                            // validate
                            bool httpsNotFound =
                                currentProtocols.Where(p => p.Name == NetworkProtocolType.HTTPS).FirstOrDefault() == null;
                            Assert(httpsNotFound, "Unsupported HTTPS protocol found", "Check if HTTPS is not present in the list");

                            /*****************************************/
                        }
                    });
        }

        [Test(Name = "GET NETWORK DEFAULT GATEWAY CONFIGURATION",
            Order = "06.02.25",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
        public void GetNetworkDefaultGatewayTest()
        {
            RunTest(() =>
                        {
                            NetworkGateway gateway = GetNetworkDefaultGateway();

                            Assert(gateway != null,
                                   "The DUT did not return network default gateway configuration",
                                   "Check if network default configuration returned");

                            BeginStep("Validate addresses");

                            string ipv4addresses = DumpStringArray(gateway.IPv4Address);
                            LogStepEvent(string.Format("IPv4 addresses: {0}", ipv4addresses));

                            if (gateway.IPv4Address != null)
                            {
                                foreach (string address in gateway.IPv4Address)
                                {
                                    // allow empty addresses
                                    if (!string.IsNullOrEmpty(address))
                                    {
                                        //System.Net.IPAddress.TryParse parses incorrect addresses
                                        if (!address.IsValidIPv4Address())
                                        {
                                            throw new AssertException(string.Format("Address {0} is incorrect", address));
                                        }
                                    }
                                }
                            }

                            string ipv6addresses = DumpStringArray(gateway.IPv6Address);
                            LogStepEvent(string.Format("IPv6 addresses: {0}", ipv6addresses));

                            if (gateway.IPv6Address != null)
                            {
                                foreach (string address in gateway.IPv6Address)
                                {
                                    if (!string.IsNullOrEmpty(address))
                                    {
                                        System.Net.IPAddress parsedAddress;
                                        if (!System.Net.IPAddress.TryParse(address, out parsedAddress) ||
                                            (parsedAddress.AddressFamily !=
                                             System.Net.Sockets.AddressFamily.InterNetworkV6))
                                        {
                                            throw new AssertException(string.Format("Address {0} is incorrect", address));
                                        }
                                    }
                                }
                            }
                            StepPassed();
                        });

        }

        [Test(Name = "SET NETWORK DEFAULT GATEWAY CONFIGURATION - IPV4",
            Order = "06.02.26",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Must)]
        public void SetNetworkDefaultGatewayIPv4Test()
        {
            RunTest<NetworkGateway>(
                
                new Backup<NetworkGateway>( 
                    () => 
                    {
                        NetworkGateway gateway = GetNetworkDefaultGateway();
                        Assert(gateway != null,
                            "The DUT did not return original network default gateway configuration",
                            "Check if original network default configuration returned");
                        return gateway;
                    }),  
                () =>
                    {

                        string[] addresses = new string[] {_environmentSettings.DefaultGateway};
                        
                        SetNetworkDefaultGateway(addresses, null);

                        NetworkGateway currentGateway = GetNetworkDefaultGateway();
                        
                        // validate - ToDo

                        //Assert(currentGateway.IPv6Address == null || currentGateway.IPv6Address.Length == 0,
                        //    "IPv6 addresses list is not empty", "Check if no IPv6 addresses are present");

                        Assert(currentGateway.IPv4Address != null && currentGateway.IPv4Address.Contains(addresses[0]), 
                            "IP address set in previous step not found in the default gateway configuration", 
                            string.Format("Check if IP address {0} is present in the list", addresses[0]));

                    }, 
                (gateway) =>
                    {
                        SetNetworkDefaultGateway(gateway.IPv4Address, gateway.IPv6Address);
                    });

        }


        [Test(Name = "SET NETWORK DEFAULT GATEWAY CONFIGURATION - IPV6",
            Order = "06.02.27",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.ConditionalShould,
            RequiredFeatures = new Feature[] { Feature.IPv6 })]
        public void SetNetworkDefaultGatewayIPv6Test()
        {
            RunTest<NetworkGateway>(

                new Backup<NetworkGateway>(
                    () =>
                    {
                        NetworkGateway gateway = GetNetworkDefaultGateway();
                        Assert(gateway != null,
                            "The DUT did not return original network default gateway configuration",
                            "Check if original network default configuration returned");
                        return gateway;
                    }),
                () =>
                {

                    string[] addresses = new string[] { _environmentSettings.DefaultGatewayIpv6 };

                    SetNetworkDefaultGateway(null, addresses);

                    NetworkGateway currentGateway = GetNetworkDefaultGateway();

                    // validate - ToDo 

                    //Assert(currentGateway.IPv4Address == null || currentGateway.IPv4Address.Length == 0,
                    //    "IPv4 addresses list is not empty", "Check if no IPv4 addresses are present");

                    Assert(currentGateway.IPv6Address != null && currentGateway.IPv6Address.Contains(addresses[0]),
                        "IP address set in previous step not found in the default gateway configuration",
                        string.Format("Check if IP address {0} is present in the list", addresses[0]));

                },
                (gateway) =>
                {
                    SetNetworkDefaultGateway(gateway.IPv4Address, gateway.IPv6Address);
                });

        }

        [Test(Name = "SET NETWORK DEFAULT GATEWAY CONFIGURATION - INVALID IPV4",
            Order = "06.02.28",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.Should)]
        public void SetNetworkDefaultGatewayInvalidIPv4Test()
        {
            RunTest(
                () =>
                    {

                        string[] addresses = new string[] {"10.1.1"};

                        RunStep(() => { Client.SetNetworkDefaultGateway(addresses, null); },
                                "Set Network Default Gateway - negative test",
                                "Sender/InvalidArgVal/InvalidGatewayAddress", false);

                        NetworkGateway currentGateway = GetNetworkDefaultGateway();

                        // validate - ToDo

                        if (currentGateway.IPv4Address != null)
                        {
                            Assert(!currentGateway.IPv4Address.Contains(addresses[0]),
                                   "Invalid IP address found in the default gateway configuration",
                                   string.Format("Check if IP address {0} is not present in the list", addresses[0]));
                        }
                    });

        }


        [Test(Name = "SET NETWORK DEFAULT GATEWAY CONFIGURATION - INVALID IPV6",
            Order = "06.02.29",
            Path = PATH,
            Version = 1.02,
            Services = new Service[] { Service.Device },
            RequirementLevel = RequirementLevel.ConditionalShould,
            RequiredFeatures = new Feature[] { Feature.IPv6 })]
        public void SetNetworkDefaultGatewayInvalidIPv6Test()
        {
            RunTest(
                () =>
                    {

                        string[] addresses = new string[] { "FF02:1" };

                        RunStep(() => { Client.SetNetworkDefaultGateway(null, addresses); },
                                "Set Network Default Gateway - negative test",
                                "Sender/InvalidArgVal/InvalidGatewayAddress", false);


                        NetworkGateway currentGateway = GetNetworkDefaultGateway();

                        // validate - ToDo 

                        if (currentGateway.IPv6Address != null)
                        {
                            Assert(!currentGateway.IPv6Address.Contains(addresses[0]),
                                   "Invalid IP address found in the default gateway configuration",
                                   string.Format("Check if IP address {0} is not present in the list", addresses[0]));
                        }
                    });
        }


        string DumpIPArray(IPAddress[] actual)
        {
            StringBuilder actualDescription = new StringBuilder();
            if (actual != null)
            {
                actualDescription.Append(string.Format("Array of {0} elements ", actual.Length));

                if (actual.Length > 0)
                {
                    actualDescription.Append("{");
                    bool bFirst = true;
                    foreach (IPAddress address in actual)
                    {
                        string description;
                        if (address.Type == IPType.IPv4)
                        {
                            description = string.Format("[IPv4 - '{0}']",  address.IPv4Address);
                        }
                        else
                        {
                            description = string.Format("[IPv6 - '{0}']", address.IPv6Address);
                        }
                        if (!bFirst)
                        {
                            actualDescription.Append(", ");
                        }
                        bFirst = false;
                        actualDescription.Append(description);
                    }
                    actualDescription.Append("}");
                }
            }
            else
            {
                actualDescription.Append(" NULL ");
            }

            return actualDescription.ToString();
        }

        string DumpStringArray(string[] actual)
        {
            StringBuilder actualDescription = new StringBuilder();
            if (actual == null)
            {
                actualDescription.Append("NULL");
            }
            else
            {
                actualDescription.Append(string.Format("Array of {0} elements ", actual.Length));

                if (actual.Length > 0)
                {
                    actualDescription.Append("{");
                    bool bFirst = true;
                    foreach (string domain in actual)
                    {
                        if (!bFirst)
                        {
                            actualDescription.Append(", ");
                        }
                        else
                        {
                            bFirst = false;
                        }
                        actualDescription.Append(domain);
                    }
                    actualDescription.Append("}");
                }
            }
            return actualDescription.ToString();
        }

        string DumpNetworkHostArray(NetworkHost[] actual)
        {
            StringBuilder actualDescription = new StringBuilder();
            if (actual != null)
            {
                actualDescription.Append(string.Format("Array of {0} hosts ", actual.Length));

                if (actual.Length > 0)
                {
                    actualDescription.Append("{");
                    bool bFirst = true;
                    foreach (NetworkHost address in actual)
                    {
                        string description = null;
                        switch (address.Type)
                        {
                            case NetworkHostType.DNS:
                                description = string.Format("[DNS name - '{0}']", address.DNSname);
                                break;
                            case NetworkHostType.IPv4:
                                description = string.Format("[IPv4 - {0}]", address.IPv4Address);
                                break;
                            case NetworkHostType.IPv6:
                                description = string.Format("[IPv6 - {0}]", address.IPv6Address);
                                break;
                        }
                        if (!bFirst)
                        {
                            actualDescription.Append(", ");
                        }
                        bFirst = false;
                        actualDescription.Append(description);
                    }
                    actualDescription.Append("}");
                }
            }
            else
            {
                actualDescription.Append(" NULL ");
            }

            return actualDescription.ToString();
        }

    }
}
