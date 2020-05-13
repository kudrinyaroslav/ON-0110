///////////////////////////////////////////////////////////////////////////
//!  @author        Ilya Gozman
////
using System.Linq;
using System.Text;
using TestTool.Tests.CommonUtils.XmlTransformation;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class NamespacesDeviceManagementTestSuite : Base.DeviceManagementTest
    {
        public NamespacesDeviceManagementTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Device Management\\Namespace Handling";

        [ Test( Name = "DEVICE MANAGEMENT - NAMESPACES (DEFAULT NAMESPACES FOR EACH TAG)",
                Path = PATH,
                Order = "06.01.01",
                Id = "6-1-1",
                Category = Category.DEVICE,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void DeviceManagementNamespacesDefaultNamespacesForEachTagTest()
        {
            SetDnsConfigurationTestWithTransofrmation(XmlTransformation.EachTag);
        }

        [ Test( Name = "DEVICE MANAGEMENT - NAMESPACES (DEFAULT NAMESPACES FOR PARENT TAG)",
                Path = PATH,
                Order = "06.01.02",
                Id = "6-1-2",
                Category = Category.DEVICE,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void DeviceManagementNamespacesDefaultNamespacesForParentTagTest()
        {
            SetDnsConfigurationTestWithTransofrmation(XmlTransformation.ParentTag);
        }

        [ Test( Name = "DEVICE MANAGEMENT - NAMESPACES (NOT STANDARD PREFIXES)",
                Path = PATH,
                Order = "06.01.03",
                Id = "6-1-3",
                Category = Category.DEVICE,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void DeviceManagementNamespacesNotStandardPrefixesTest()
        {
            SetDnsConfigurationTestWithTransofrmation(XmlTransformation.NotStandardPrefixes);
        }

        [ Test( Name = "DEVICE MANAGEMENT - NAMESPACES (DIFFERENT PREFIXES FOR THE SAME NAMESPACE)",
                Path = PATH,
                Order = "06.01.04",
                Id = "6-1-4",
                Category = Category.DEVICE,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void DeviceManagementNamespacesDifferentPrefixesForTheSameNamespaceTest()
        {
            SetDnsConfigurationTestWithTransofrmation(XmlTransformation.DifferentPrefixes);
        }

        [ Test( Name = "DEVICE MANAGEMENT - NAMESPACES (THE SAME PREFIX FOR DIFFERENT NAMESPACES)",
                Path = PATH,
                Order = "06.01.05",
                Id = "6-1-5",
                Category = Category.DEVICE,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void DeviceManagementNamespacesTheSamePrefixForDifferentNamespacesTest()
        {
            SetDnsConfigurationTestWithTransofrmation(XmlTransformation.SamePrefixes);
        }

        void SetDnsConfigurationTestWithTransofrmation(XmlTransformation transformation)
        {
            RunTest<DNSInformation>(
            new Backup<DNSInformation>(
            () =>
            {
                XmlNamespacesTransformer transformer = new XmlNamespacesTransformer(transformation);
                SetBreakingBehaviour(transformer);

                return GetOriginalDnsConfiguration();
            }),
            () =>
            {
                SetDnsConfiguration();
            },
            (originalInformation) =>
            {
                RestoreOriginalDnsConfiguration(originalInformation);

                ResetBreakingBehaviour();
            });
        }

        #region SetDNS

        DNSInformation GetOriginalDnsConfiguration()
        {
            DNSInformation originalInformation = GetDnsConfiguration();
            Assert(originalInformation != null, "Failed to get original DNS configuration", "Check that original DNS configuration returned from the DUT");
            return originalInformation;
        }

        void RestoreOriginalDnsConfiguration(DNSInformation originalInformation)
        {
            SetDnsConfiguration(originalInformation, "Restore DNS configuration");
        }

        void SetDnsConfiguration()
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
                            description = string.Format("[IPv4 - '{0}']", address.IPv4Address);
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

        #endregion
    }
}