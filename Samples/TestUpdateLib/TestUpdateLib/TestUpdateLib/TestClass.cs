using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Engine.Base.TestBase;
using System;

namespace TestUpdateLib
{
    [TestClass]
    public class VeryTolerantTestSuite : BaseTest
    {
        private const string PATH = "Device Management\\System";
        private const string PATH4 = "IP Configuration\\IPv4";
        private const string PATH6 = "IP Configuration\\IPv6";

        public VeryTolerantTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        #region Common

        /// <summary>
        /// Perform common initialization/finalization. Run test action.
        /// </summary>
        /// <param name="action">Test "body"</param>
        /// <param name="cleanUpAction">Clean-up </param>
        protected void RunTest(Action action)
        {
            _halted = false;
            Exception exc = null;

            try
            {
                ResetLog();

                action();

                EndTest(TestStatus.Passed);
            }
            catch (StopEventException)
            {
                LogStepEvent("Halted");
                _halted = true;
            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
                return;

            }
        }

        /// <summary>
        /// Ends the test.
        /// </summary>
        /// <param name="status"></param>
        protected void EndTest(TestStatus status)
        {
            SetTestStatus(status);
            TestCompleted();
        }

        void DoNothing()
        {
            RunTest(
                () =>
                    {
                        LogTestEvent("THIS VERSION DOES NOTHING");
                    }
                );
        }

        #endregion

        [Test(Name = "HELLO MESSAGE",
            Path = "Device Discovery",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.DISCOVERY,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery })]
        public void HelloMessageTest()
        {
            DoNothing();
        }

        [Test(Name = "HELLO MESSAGE VALIDATION",
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.DISCOVERY,
            Path = "Device Discovery",
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery })]
        public void HelloMessageValidationTest()
        {
            DoNothing();
        }

        [Test(Name = "SEARCH BASED ON DEVICE SCOPE TYPES",
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.DISCOVERY,
            Path = "Device Discovery",
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery })]
        public void SearchDeviceScopeTypesTest()
        {
            DoNothing();
        }

        [Test(Name = "SEARCH WITH OMITTED DEVICE AND SCOPE TYPES",
            Path = "Device Discovery",
            Order = "01.01.04",
            Id = "1-1-4",
            Category = Category.DISCOVERY,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery })]
        public void SearchOmittedDeviceTest()
        {
            DoNothing();
        }

        [Test(Name = "RESPONSE TO INVALID SEARCH REQUEST",
            Path = "Device Discovery",
            Order = "01.01.05",
            Id = "1-1-5",
            Category = Category.DISCOVERY,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery })]
        public void ResponseInvalidSearchTest()
        {
            DoNothing();
        }

        [Test(Name = "SEARCH USING UNICAST PROBE MESSAGE",
            Path = "Device Discovery",
            Order = "01.01.06",
            Id = "1-1-6",
            Category = Category.DISCOVERY,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery })]
        public void UnicastProbeMessageTest()
        {
            DoNothing();
        }

        [Test(Name = "DEVICE SCOPES CONFIGURATION",
            Path = "Device Discovery",
            Order = "01.01.07",
            Id = "1-1-7",
            Category = Category.DISCOVERY,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[]
                                         {
                                             Functionality.GetScopes, 
                                             Functionality.AddScopes, 
                                             Functionality.RemoveScopes, 
                                             Functionality.SetScopes
                                         })]
        public void DeviceScopesConfigurationTest()
        {
            DoNothing();
        }

        [Test(Name = "BYE MESSAGE",
            Path = "Device Discovery",
            Order = "01.01.08",
            Id = "1-1-8",
            Category = Category.DISCOVERY,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.BYE },
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery })]
        public void ByeMessageTest()
        {
            DoNothing();
        }

        [Test(Name = "DISCOVERY MODE CONFIGURATION",
            Path = "Device Discovery",
            Version = 2.1,
            Order = "01.01.09",
            Id = "1-1-9",
            Category = Category.DISCOVERY,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery, Functionality.GetDiscoveryMode, Functionality.SetDiscoveryMode })]
        public void DiscoveryModeConfigurationTest()
        {
            DoNothing();
        }

        [Test(Name = "SOAP FAULT MESSAGE",
            Path = "Device Discovery",
            Order = "01.01.10",
            Id = "1-1-10",
            Category = Category.DISCOVERY,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional)]
        public void SoapFaultMessageTest()
        {
            DoNothing();
        }


        [Test(Name = "IPV4 STATIC IP",
            Path = PATH4,
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.IPCONFIG,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv4StaticIPTest()
        {
            DoNothing();
        }

        [Test(Name = "IPV4 LINK LOCAL ADDRESS",
            Path = PATH4,
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.IPCONFIG,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ZeroConfiguration },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv4LinkLocalAddress()
        {
            DoNothing();
        }

        [Test(Name = "IPV4 DHCP",
            Path = PATH4,
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.IPCONFIG,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv4DhcpTest()
        {
            DoNothing();
        }


        [Test(Name = "IPV6 STATIC IP",
            Path = PATH6,
            Order = "02.01.01",
            Id = "2-1-1",
            Category = Category.IPCONFIG,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv6StaticIPTest()
        {
            DoNothing();
        }
        
        [Test(Name = "IPV6 STATELESS IP CONFIGURATION - ROUTER ADVERTISEMENT",
            Path = PATH6,
            Order = "02.01.02",
            Id = "2-1-2",
            Category = Category.IPCONFIG,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv6StatelessAdvIPTest()
        {
            DoNothing();
        }
        
        [Test(Name = "IPV6 STATELESS IP CONFIGURATION - NEIGHBOUR DISCOVERY",
            Path = PATH6,
            Order = "02.01.03",
            Id = "2-1-3",
            Category = Category.IPCONFIG,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv6StatelessDisIPTest()
        {
            DoNothing();
        }

        [Test(Name = "IPV6 STATEFUL IP CONFIGURATION",
            Path = PATH6,
            Order = "02.01.04",
            Id = "2-1-4",
            Category = Category.IPCONFIG,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv6StatefullIPTest()
        {
            DoNothing();
        }


        [Test(Name = "DISCOVERY - NAMESPACES (DEFAULT NAMESPASES FOR EACH TAG)",
            Order = "02.01.01",
            Id = "2-1-1",
            Category = Category.DISCOVERY,
            Path = "Device Discovery",
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery })]
        public void DiscoveryNamespacesDefaultNamespacesForEachTagTest()
        {
            DoNothing();
        }

        [Test(Name = "DISCOVERY - NAMESPACES (DEFAULT NAMESPASES FOR PARENT TAG)",
            Order = "02.01.02",
            Id = "2-1-2",
            Category = Category.DISCOVERY,
            Path = "Device Discovery",
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery })]
        public void DiscoveryNamespacesDefaultNamespacesForParentTagTest()
        {
            DoNothing();
        }

        [Test(Name = "DISCOVERY - NAMESPACES (NOT STANDARD PREFIXES)",
            Order = "02.01.03",
            Id = "2-1-3",
            Category = Category.DISCOVERY,
            Path = "Device Discovery",
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery })]
        public void DiscoveryNamespacesNotStandardPrefixesTest()
        {
            DoNothing();
        }

        [Test(Name = "DISCOVERY - NAMESPACES (DIFFERENT PREFIXES FOR THE SAME NAMESPACE)",
            Order = "02.01.04",
            Id = "2-1-4",
            Category = Category.DISCOVERY,
            Path = "Device Discovery",
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery })]
        public void DiscoveryNamespacesDifferentPrefixesForTheSameNamespaceTest()
        {
            DoNothing();
        }

        [Test(Name = "DISCOVERY - NAMESPACES (THE SAME PREFIX FOR DIFFERENT NAMESPACES)",
            Order = "02.01.05",
            Id = "2-1-5",
            Category = Category.DISCOVERY,
            Path = "Device Discovery",
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.WSDiscovery })]
        public void DiscoveryNamespacesTheSamePrefixForDifferentNamespacesTest()
        {
            DoNothing();
        }
        [Test(Name = "SYSTEM COMMAND FACTORY DEFAULT SOFT",
            Path = PATH,
            Order = "03.01.07",
            Id = "3-1-7",
            Category = Category.DEVICE,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.Last,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetSystemFactoryDefaults })]
        public void FactoryDefaultSoftTest()
        {
            DoNothing();
        }

        [Test(Name = "SYSTEM COMMAND REBOOT",
            Path = PATH,
            Order = "03.01.08",
            Id = "3-1-8",
            Category = Category.DEVICE,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Reboot})]
        public void SystemCommandRebootTest()
        {
            DoNothing();
        }



        [Test(Name = "SET NETWORK INTERFACE CONFIGURATION - IPV4",
            Order = "02.01.18",
            Id = "2-1-18",
            Category = Category.DEVICE,
            Path = "Device Management\\Network",
            Version = 5.0,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void SetNetworkInterfacesIpv4Test()
        {
            DoNothing();
        }

        [Test(Name = "SET DNS CONFIGURATION - FROMDHCP",
            Order = "02.01.08",
            Id = "2-1-8",
            Category = Category.DEVICE,
            Path = "Device Management\\Network",
            Version = 5.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetDns },
            RequirementLevel = RequirementLevel.Must)]
        public void SetNetworkInterfacesDnsTest()
        {
            DoNothing();
        }

                
        [Test(Name = "SET NTP CONFIGURATION - FROMDHCP",
            Order = "02.01.14",
            Id = "2-1-14",
            Category = Category.DEVICE,
            Path = "Device Management\\Network",
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.NTP },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetNTP })]
        public void SetNtpConfigurationDnsFromDHCPTest()
        {
            DoNothing();
        }


        [Test(Name = "SET NETWORK INTERFACE CONFIGURATION - IPV6",
            Order = "02.01.19",
            Id = "2-1-19",
            Category = Category.DEVICE,
            Path = "Device Management\\Network",
            Version = 2.5,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void SetNetworkInterfacesIpv6Test()
        {
            DoNothing();
        }



    }

}
