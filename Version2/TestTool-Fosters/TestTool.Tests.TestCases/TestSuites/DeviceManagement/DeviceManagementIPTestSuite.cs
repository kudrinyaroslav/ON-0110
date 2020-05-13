///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

#define USE_COLLECTION_COMPARE


using System;
using System.Linq;
using System.ServiceModel;
using TestTool.Tests.Common.Soap;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine.Base;
using TestTool.Tests.Engine.Base.Definitions;
using WSD = TestTool.Proxies.WSDiscovery;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class DeviceManagementIPTestSuite : Base.DeviceDiscoveryTest
    {
        public DeviceManagementIPTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH4 = "IP Configuration\\IPv4";
        private const string PATH6 = "IP Configuration\\IPv6";

        private string AddressBackup = "";

        #region SupportingFunctions

        #region ConfiguratingFunctions

        NetworkInterface[] BackupInterfaces()
        {
            BackupConnection();

            NetworkInterface[] interfaces = GetNetworkInterfaces();

            Assert(interfaces != null && interfaces.Length > 0,
                "No interfaces returned from the DUT",
                "Check that the DUT returned current interfaces");

            return interfaces;
        }
        protected void BackupConnection()
        {
            AddressBackup = _endpointController.Address.ToString();
            //AddressBackup = Client.Endpoint.Address.ToString();
        }
        void RestoreConnection()
        {
            _endpointController.UpdateAddress(new EndpointAddress(AddressBackup));
            //Client.Endpoint.Address = new EndpointAddress(AddressBackup);
        }
        void ReConnectTo(string ServiceAddress)
        {
            _endpointController.UpdateAddress(new EndpointAddress(ServiceAddress));
            //Client.Endpoint.Address = new EndpointAddress(ServiceAddress);
        }

        string MyV4String(PrefixedIPv4Address address)
        {
            return address.Address + " [" + address.PrefixLength + "]";
        }
        string MyV6String(PrefixedIPv6Address address)
        {
            return address.Address + " [" + address.PrefixLength + "]";
        }

        void LogConfiguration(NetworkInterfaceSetConfiguration configuration)
        {
            if ((configuration.IPv4 != null) &&
                (configuration.IPv4.EnabledSpecified) &&
                (configuration.IPv4.Enabled))
            {
                if (configuration.IPv4.DHCPSpecified)
                {
                    LogStepEvent("IPv4 DHCP = " + configuration.IPv4.DHCP);
                }
                if ((configuration.IPv4.Manual != null) &&
                    (configuration.IPv4.Manual.Count() == 1))
                {
                    LogStepEvent("IPv4 Address = " + MyV4String(configuration.IPv4.Manual[0]));
                }
            }
            if ((configuration.IPv6 != null) &&
                (configuration.IPv6.EnabledSpecified) &&
                (configuration.IPv6.Enabled))
            {
                if (configuration.IPv6.DHCPSpecified)
                {
                    LogStepEvent("IPv6 DHCP = " + configuration.IPv6.DHCP.ToString());
                }
                if (configuration.IPv6.AcceptRouterAdvertSpecified)
                {
                    LogStepEvent("IPv6 Advert = " + configuration.IPv6.AcceptRouterAdvert);
                }
                if ((configuration.IPv6.Manual != null) &&
                    (configuration.IPv6.Manual.Count() == 1))
                {
                    LogStepEvent("IPv6 Address = " + MyV6String(configuration.IPv6.Manual[0]));
                }
            }
        }

        string GuessAddress(NetworkInterfaceSetConfiguration configuration)
        {
            string SetIP = null;
            if ((configuration.IPv4 != null) &&
                (configuration.IPv4.EnabledSpecified) &&
                (configuration.IPv4.Enabled))
            {
                if ((configuration.IPv4.Manual != null) &&
                    (configuration.IPv4.Manual.Count() == 1))
                {
                    SetIP = configuration.IPv4.Manual[0].Address;
                }
            }
            if ((configuration.IPv6 != null) &&
                (configuration.IPv6.EnabledSpecified) &&
                (configuration.IPv6.Enabled))
            {
                if ((configuration.IPv6.Manual != null) &&
                    (configuration.IPv6.Manual.Count() == 1))
                {
                    SetIP = "[" + configuration.IPv6.Manual[0].Address + "]";
                }
            }
            if (string.IsNullOrEmpty(SetIP))
                return AddressBackup;

            string Postfix = "/onvif/device_service";
            {
                string uri = AddressBackup;
                int pos = uri.IndexOf(']', 8);
                if (pos > 0) // IPv6
                {
                    pos++;
                }
                else
                {
                    pos = uri.IndexOf(':', 8);
                }
                if (pos <=0)
                {
                    pos = uri.IndexOf('/', 8);
                }
                if (pos > 0)
                {
                    Postfix = uri.Substring(pos);
                }
            }
            return "http://" + SetIP + Postfix;
        }


        int FindPrefixLength(string a, string b)
        {
            int max = a.Length;
            for (int i = 0; i < max; i++)
            {
                if (a[i] != b[i]) return i;
            }
            return max;
        }

        void SetIPConfigurationInt(string interfaceToken, 
            NetworkInterfaceSetConfiguration configuration, 
            bool restore)
        {
            bool rebootNeeded = false;
            SoapMessage<WSD.HelloType> hello = null;
            try
            {
                hello = ReceiveHelloMessage(
                    false,
                    true,
                    new Action(() =>
                                   {
                                       BeginStep(restore ? "Restore network settings" : "Set network interface");
                                       if (!restore)
                                       {
                                           LogStepEvent("interface token = " + interfaceToken);
                                           LogConfiguration(configuration);
                                       }
                                       rebootNeeded = SetNetworkInterface(interfaceToken, configuration);
                                       //if (!restore)
                                       {
                                           LogStepEvent("reboot need = " + rebootNeeded);
                                       }
                                       StepPassed();

                                       if (rebootNeeded)
                                       {
                                           SystemReboot();
                                       }
                                   }));
            }
            catch (AssertException)
            {
                // Assert: something wrong with receiving message (see DeviceDiscoveryTest.ReceiveMessageInternal)
                // SetNetworkInterface and SystemReboot don't throw AssertException.
                if (!rebootNeeded)
                {
                    LogStepEvent("Warning: no Hello within timeout");
                    StepPassed();

                    if (restore)
                    {
                        RestoreConnection();
                        return;
                    }

                    // should not occur, hack
                    if (AddressBackup == null)
                    {
                        return;
                    }

                    BeginStep("No Hello - guessing right address");
                    string newAddr = GuessAddress(configuration);
                    LogStepEvent("Selecting " + newAddr);
                    ReConnectTo(newAddr);
                    StepPassed();

                    return; // no strict requirement for hello in this case
                }
                throw;
            } // catch

            if (!rebootNeeded)
            {
                if (hello == null)
                {
                    return; // no strict requirement for hello in this case
                }
            }

            // no types verification - good for 2.1
            RunNormal("Verifying Hello message", new Action(() =>
                {
                    MyAssert(
                        LogAssert(hello, "hello") &&
                        LogAssert(hello.Object, "hello content") &&
                        LogAssert(hello.Object.EndpointReference, "hello endpoint") &&
                        LogAssert(hello.Object.EndpointReference.Address, "hello endpoint address") &&
                        LogAssert(hello.Object.EndpointReference.Address.Value, "hello endpoint address value") &&
                        LogLog("Endpoint Address = " + hello.Object.EndpointReference.Address.Value) &&
                        LogAssert(hello.Object.XAddrs,"hello service address(es) value") &&
                        LogLog("Service Address(es) = " + hello.Object.XAddrs), "Hello not verified",
                        "Hello verified successfully");
                }));

            {
                BeginStep("Identifying right address");
                if (AddressBackup != null)
                    LogStepEvent("Old address " + AddressBackup);
                string[] addresses = hello.Object.XAddrs.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                string newAddr = AddressBackup;
                int count = 0;
                foreach (string s in addresses)
                {
                    int c = FindPrefixLength(s, AddressBackup);
                    if (c > count)
                    {
                        newAddr = s;
                        count = c;
                    }
                }
                LogStepEvent("Selecting " + newAddr);
                ReConnectTo(newAddr);
                StepPassed();
            }
        }

        protected void SetIPConfiguration(string interfaceToken, NetworkInterfaceSetConfiguration configuration)
        {
            SetIPConfigurationInt(interfaceToken, configuration, false);
        }
 
        protected void RestoreNetworkInterface(string interfaceToken, NetworkInterface networkInterface)
        {
            NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();

            configuration.Enabled = networkInterface.Enabled;
            configuration.EnabledSpecified = true;
            //configuration.Extension 
            if (networkInterface.IPv4 != null)
            {
                configuration.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
                configuration.IPv4.DHCP = networkInterface.IPv4.Config.DHCP;
                configuration.IPv4.DHCPSpecified = true;
                configuration.IPv4.Enabled = networkInterface.IPv4.Enabled;
                configuration.IPv4.EnabledSpecified = true;
                configuration.IPv4.Manual = networkInterface.IPv4.Config.Manual;
            }

            if (networkInterface.IPv6 != null)
            {
                configuration.IPv6 = new IPv6NetworkInterfaceSetConfiguration();

                configuration.IPv6.Enabled = networkInterface.IPv6.Enabled;
                configuration.IPv6.EnabledSpecified = true;

                if (networkInterface.IPv6.Config != null)
                {
                    configuration.IPv6.AcceptRouterAdvert = networkInterface.IPv6.Config.AcceptRouterAdvert;
                    configuration.IPv6.AcceptRouterAdvertSpecified = networkInterface.IPv6.Config.AcceptRouterAdvertSpecified;
                    //if (networkInterface.IPv6.Config.DHCP != null)
                    //{
                        configuration.IPv6.DHCP = networkInterface.IPv6.Config.DHCP;
                        configuration.IPv6.DHCPSpecified = true;
                    //}
                    configuration.IPv6.Manual = networkInterface.IPv6.Config.Manual;
                }
            }

            //configuration.Link = networkInterface.Link;

            SetIPConfigurationInt(interfaceToken, configuration, true);
        }

        #endregion

        #region AssertHelpers

        void MyAssert(bool condition, string negMessage)
        {
            if (!condition)
            {
                //_currentStep.Message = message;
                AssertException ex = new AssertException(negMessage);
                throw ex;
            }
        }
        void MyAssert(bool condition, string negMessage, string posMessage)
        {
            if (!condition)
            {
                //_currentStep.Message = message;
                AssertException ex = new AssertException(negMessage);
                throw ex;
            }
            LogStepEvent(posMessage);
        }
        bool LogAssertComp(object obj1, object obj2, string Message)
        {
            if (obj1.Equals(obj2))
            {
                LogStepEvent(Message + " are equal [" + obj1.ToString() + "]");
                return true;
            }
            else
            {
                LogStepEvent(Message + " are not equal [" + obj1.ToString() + " != " + obj2.ToString() + "]");
                return false;
            }
        }
        bool LogLog(string Message)
        {
            LogStepEvent(Message);
            return true;
        }
        bool LogAssert(bool value, string negMessage)
        {
            if (!value)
            {
                LogStepEvent(negMessage);
            }
            return value;
        }
        bool LogAssert(object obj, string negMessage)
        {
            return LogAssert(obj != null, negMessage + " undefined");
        }
        bool LogAssertLog(bool value, string Message)
        {
            LogStepEvent(string.Format(Message, value ? "" : "not "));
            return value;
        }
        bool LogAssert(bool value, string negMessage, string posMessage)
        {
            LogStepEvent(value ? posMessage : negMessage);
            return value;
        }

        bool LogAssertVal(object obj, string Name)
        {
            if (obj == null)
            {
                LogStepEvent(Name + " undefined");
                return false;
            }
            else
            {
                LogStepEvent(Name + " = " + obj.ToString());
                return true;
            }
        }

        void RunNormal(string Name, Action action)
        {
            BeginStep(Name);
            action();
            StepPassed();
        }

        bool LogAssertPrefixString(string[] Manual, string address)
        {
            int Count = Manual.Where(NI => (NI == address)).Count();
            string AdrText = null;
            foreach (string p in Manual)
            {
                if (AdrText != null)
                {
                    AdrText += ", " + p;
                }
                else
                {
                    AdrText = p;
                }
            }
            return
                LogLog("Zero Configuration address(es): " + AdrText) &
                LogLog("LinkLocal address: " + address) &
                LogAssert(
                    Count > 0,
                    "No IP address in addresses set") &&
                LogAssert(
                    Count == 1,
                    "More than one equal IP address in addresses set");
        }
        bool LogAssertPrefix4(PrefixedIPv4Address[] Manual, PrefixedIPv4Address address)
        {
            int Count = Manual.Where(NI => ((NI.PrefixLength == address.PrefixLength) && (NI.Address == address.Address))).Count();
            string AdrText = null;
            foreach (PrefixedIPv4Address p in Manual)
            {
                if (AdrText != null)
                {
                    AdrText += ", " + MyV4String(p);
                }
                else
                {
                    AdrText = MyV4String(p);
                }
            }
            return
                LogLog("Manual address(es): " + AdrText) &
                LogAssert(
                    Count > 0,
                    "No IP address in addresses set") &&
                LogAssert(
                    Count == 1,
                    "More than one equal IP address in addresses set");
        }
        bool LogAssertPrefix6(PrefixedIPv6Address[] Manual, PrefixedIPv6Address address)
        {
            int Count = Manual.Where(NI => ((NI.PrefixLength == address.PrefixLength) && (NI.Address == address.Address))).Count();
            string AdrText = null;
            foreach (PrefixedIPv6Address p in Manual)
            {
                if (AdrText != null)
                {
                    AdrText += ", " + MyV6String(p);
                }
                else
                {
                    AdrText = MyV6String(p);
                }
            }
            return
                LogLog("Manual address(es): " + AdrText) &
                LogAssert(
                    Count > 0,
                    "No IP address in addresses set") &&
                LogAssert(
                    Count == 1,
                    "More than one equal IP address in addresses set");
        }

        #endregion

        #endregion

        [Test(Name = "IPV4 STATIC IP",
            Path = PATH4,
            Order = "01.01.01",
            Id ="1-1-1",
            Category = Category.IPCONFIG, 
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[]{Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces})]
        public void IPv4StaticIPTest()
        {
            NetworkInterface[] originalSettings = null;
            string token = null;

            RunTest<NetworkInterface[]>(

            new Backup<NetworkInterface[]>(
            () =>
            {
                return originalSettings = BackupInterfaces();
            }),
            () =>
            {
                NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
                //configuration.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
                configuration.Enabled = true;
                configuration.EnabledSpecified = true;
                configuration.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
                configuration.IPv4.Enabled = true;
                configuration.IPv4.EnabledSpecified = true;
                configuration.IPv4.DHCP = false;
                configuration.IPv4.DHCPSpecified = true;

                configuration.IPv4.Manual = new PrefixedIPv4Address[] { originalSettings.GetAvailableIPv4Address(out token) };

                RunNormal("Verifying IPv4 presence", new Action(() =>
                {
                    MyAssert(
                        LogAssertVal(token, "interface"),
                        "No available interface token or they are improperly configured");
                }));

                SetIPConfiguration(token, configuration);

                NetworkInterface[] currentSettings = GetNetworkInterfaces();

                NetworkInterface modified = currentSettings.Where(NI => NI.token == token).FirstOrDefault();

                RunNormal("Verifying appliance of IPv4 static settings", new Action(() =>
                {
                    MyAssert(
                        LogLog("Check for interface token = " + token) &&
                        LogAssert(modified, "interface") &&
                        LogAssert(modified.IPv4, "interface data") &&
                        LogAssert(modified.IPv4.Enabled, "IP configuration not enabled") &&
                        LogAssert(modified.IPv4.Config, "IP configuration") &&
                        LogAssertLog(!modified.IPv4.Config.DHCP, "DHCP {0}applied") &&
                        LogAssert(modified.IPv4.Config.Manual, "IP addresses") &&
#if USE_COLLECTION_COMPARE
                        LogAssertPrefix4(modified.IPv4.Config.Manual, configuration.IPv4.Manual[0])
#else
                        LogAssert(modified.IPv4.Config.Manual.Count() == configuration.IPv4.Manual.Count(), "IP addresses count not equal") &&
                        LogAssertLog(modified.IPv4.Config.Manual[0].Address == configuration.IPv4.Manual[0].Address, "IP address {0}applied") &&
                        LogAssertLog(modified.IPv4.Config.Manual[0].PrefixLength == configuration.IPv4.Manual[0].PrefixLength, "IP prefix {0}applied")
#endif
                        , "Settings not applied", "Settings applied successfully");
                }));

            },

            // Restore configuration
            (networkInterfaces) =>
            {
                NetworkInterface networkInterface =
                    networkInterfaces.Where(NI => NI.token == token).FirstOrDefault();

                RestoreNetworkInterface(token, networkInterface);
            });
        }

        [Test(Name = "IPV4 LINK LOCAL ADDRESS",
            Path = PATH4,
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.IPCONFIG,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[]{ Feature.ZeroConfiguration },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv4LinkLocalAddress()
        {
            NetworkInterface[] originalSettings = null;
            string token = null;
            bool RestoreV6 = false;

            RunTest<NetworkInterface[]>(

            new Backup<NetworkInterface[]>(
            () =>
            {
                return originalSettings = BackupInterfaces();
            }),
            () =>
            {
                NetworkInterface ni = null;
                foreach (NetworkInterface n in originalSettings)
                {
                    if (n.IPv4 == null) continue;
                    if (!n.IPv4.Enabled) continue;
                    if (n.IPv4.Config == null) continue;
                    if (n.IPv6 != null) RestoreV6 = true;
                    token = n.token;
                    ni = n;
                    break;
                }
                RunNormal("Verifying IPv4 presence", new Action(() =>
                {
                    MyAssert(
                        LogAssertVal(token, "interface"),
                        "No available interface token");
                }));

                if (ni.Link != null)
                {
                    SetZeroConfiguration(token, false);
                }

                SetZeroConfiguration(token, true);

                /*   -- 3 -- */
                NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
                configuration.Enabled = true;
                configuration.EnabledSpecified = true;
                configuration.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
                configuration.IPv4.Enabled = true;
                configuration.IPv4.EnabledSpecified = true;
                configuration.IPv4.DHCP = false;
                configuration.IPv4.DHCPSpecified = true;
                // TODO - verify that we are not on v6 service address
                // TODO spec issue with unconditional set of V6
                if (Features.Contains(Feature.IPv6))
                {
                    configuration.IPv6 = new IPv6NetworkInterfaceSetConfiguration();
                    configuration.IPv6.Enabled = false;
                    configuration.IPv6.EnabledSpecified = true;
                }
                else
                {
                    RestoreV6 = false;
                }

                SetIPConfiguration(token, configuration);

                /*  --  4  -- */
                NetworkInterface[] currentSettings = GetNetworkInterfaces();

                /*  --  5  -- */
                NetworkInterface modified = currentSettings.Where(NI => NI.token == token).FirstOrDefault();
                RunNormal("Verifying appliance of IPv4 LinkLocal settings", new Action(() =>
                {
                    MyAssert(
                        LogLog("Check for interface token = " + token) &&
                        LogAssert(modified, "interface") &&
                        LogAssert(modified.IPv4, "interface data") &&
                        LogAssert(modified.IPv4.Enabled, "IP configuration not enabled") &&
                        LogAssert(modified.IPv4.Config, "IP configuration") &&
                        LogAssert(modified.IPv4.Config.LinkLocal, "LinkLocal") &&
                        LogLog("LinkLocal address = " + MyV4String(modified.IPv4.Config.LinkLocal))
                        , "LinkLocal settings not applied", "LinkLocal settings applied successfully");
                }));

                NetworkZeroConfiguration zeroConf = GetZeroConfiguration();
                RunNormal("Verifying appliance of IPv4 zero settings", new Action(() =>
                {
                    MyAssert(
                        LogLog("Check for interface token = " + token) &&
                        LogAssert(zeroConf, "zero configuration") &&
                        LogAssert(zeroConf.Enabled, "zero configuration not enabled") &&
                        LogAssertComp(zeroConf.InterfaceToken, token, "interface token") &&
                        LogAssert(zeroConf.Addresses, "Addresses") &&
#if USE_COLLECTION_COMPARE
                        LogAssertPrefixString(zeroConf.Addresses, modified.IPv4.Config.LinkLocal.Address)
#else
                        LogAssert(zeroConf.Addresses.Count() == 1, "Addresses count mismatched") &&
                        LogAssertComp(zeroConf.Addresses[0], modified.IPv4.Config.LinkLocal.Address, "LinkLocal and zero")
#endif
                        , "Zero settings not applied", "Zero settings applied successfully");
                }));

            },

            // Restore configuration
            (networkInterfaces) =>
            {
                SetZeroConfiguration(token, false);
                NetworkInterface original = originalSettings.Where(NI => NI.token == token).FirstOrDefault();
                if (RestoreV6)
                {
                    if (original.IPv6 == null)
                    {
                        original.IPv6 = new IPv6NetworkInterface();
                        original.IPv6.Enabled = true;
                    }
                }
                RestoreNetworkInterface(token, original);
            });
        }

        private void GoAwayDHCP(string token, PrefixedIPv4Address adr)
        {
            NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
            configuration.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
            configuration.Enabled = true;
            configuration.EnabledSpecified = true;
            configuration.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
            configuration.IPv4.Enabled = true;
            configuration.IPv4.EnabledSpecified = true;
            configuration.IPv4.DHCP = false;
            configuration.IPv4.DHCPSpecified = true;
            configuration.IPv4.Manual = new PrefixedIPv4Address[] { Extensions.NextAddress(adr) };

            SetIPConfiguration(token, configuration);
        }
        
        [Test(Name = "IPV4 DHCP",
            Path = PATH4,
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.IPCONFIG,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv4DhcpTest()
        {
            NetworkInterface[] originalSettings = null;
            string token = null;
            bool HasDHCP = false;

            RunTest<NetworkInterface[]>(

            new Backup<NetworkInterface[]>(
            () =>
            {
                return originalSettings = BackupInterfaces();
            }),
            () =>
            {
                PrefixedIPv4Address adr = null;
                foreach (NetworkInterface n in originalSettings)
                {
                    if (n.IPv4 == null) continue;
                    if (!n.IPv4.Enabled) continue;
                    if (n.IPv4.Config == null) continue;
                    token = n.token;
                    HasDHCP = n.IPv4.Config.DHCP;
                    if (n.IPv4.Config.DHCP)
                    {
                        adr = n.IPv4.Config.FromDHCP;
                    }
                    break;
                }
                RunNormal("Verifying IPv4 presence", new Action(() =>
                {
                    MyAssert(
                        LogAssertVal(token, "interface"),
                        "No available interface token");
                }));

                if (HasDHCP)
                {
                    GoAwayDHCP(token, adr);
                }

                NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
                configuration.Enabled = true;
                configuration.EnabledSpecified = true;
                configuration.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
                configuration.IPv4.Enabled = true;
                configuration.IPv4.EnabledSpecified = true;
                configuration.IPv4.DHCP = true;
                configuration.IPv4.DHCPSpecified = true;

                SetIPConfiguration(token, configuration);

                /*   -- 4 --  */
                NetworkInterface[] currentSettings = GetNetworkInterfaces();

                /*   -- 5 --  */
                // ToDo : check current settings
                NetworkInterface modified = currentSettings.Where(NI => NI.token == token).First();
                RunNormal("Verifying appliance of IPv4 static settings", new Action(() =>
                {
                    MyAssert(
                        LogLog("Check for interface token = " + token) &&
                        LogAssert(modified, "interface") &&
                        LogAssert(modified.IPv4, "interface data") &&
                        LogAssert(modified.IPv4.Enabled, "IP configuration not enabled") &&
                        LogAssert(modified.IPv4.Config, "IP configuration") &&
                        LogAssertLog(modified.IPv4.Config.DHCP, "DHCP {0}applied") &&
                        LogAssert(modified.IPv4.Config.FromDHCP, "DHCP Address") &&
                        LogLog("DHCP address = " + MyV4String(modified.IPv4.Config.FromDHCP))
                        , "DHCP settings not applied", "DHCP settings applied successfully");
                }));

            },

            // Restore configuration
            (networkInterfaces) =>
            {
                if (HasDHCP)
                {
                    BeginStep("Restore network settings");
                    LogStepEvent("Already in DHCP mode");
                    StepPassed();
                }
                else
                {
                    NetworkInterface networkInterface =
                        networkInterfaces.Where(NI => NI.token == token).FirstOrDefault();

                    RestoreNetworkInterface(token, networkInterface);
                }
            });
        }

        bool TestIPv6InterfaceByFeature(bool IPv6Feature, string interfaceToken)
        {
            RunNormal("Verifying IPv6 presence", new Action(() =>
            {
                if (!IPv6Feature)
                {
                    MyAssert(
                        !LogAssertVal(interfaceToken, "interface"),
                        "IPv6 should not be supported");
                }
                else
                {
                    MyAssert(
                        LogAssertVal(interfaceToken, "interface"),
                        "No available interface token");
                }
            }));
            if (!IPv6Feature)
            {
                EndTest(TestStatus.NotSupported);
                return true;
            }
            return false;
        }

        [Test(Name = "IPV6 STATIC IP",
            Path = PATH6,
            Order = "02.01.01",
            Id = "2-1-1",
            Category = Category.IPCONFIG,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv6StaticIPTest()
        {
            NetworkInterface[] originalSettings = null;
            string token = null;
            bool IPv6Feature = Features.Contains(Feature.IPv6);

            RunTest<NetworkInterface[]>(

            new Backup<NetworkInterface[]>(
            () =>
            {
                //if (!IPv6Feature)
                //{
                //    LogStepEvent("IPv6 not supported - negative test mode\r\n");
                //}
                return originalSettings = BackupInterfaces();
            }),
            () =>
            {
                NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
                configuration.Enabled = true;
                configuration.EnabledSpecified = true;
                configuration.IPv6 = new IPv6NetworkInterfaceSetConfiguration();
                configuration.IPv6.Enabled = true;
                configuration.IPv6.EnabledSpecified = true;
                configuration.IPv6.DHCP = IPv6DHCPConfiguration.Off;
                configuration.IPv6.DHCPSpecified = true;
                foreach (NetworkInterface n in originalSettings)
                {
                    if (n.IPv6 == null) continue;
                    //if (!n.IPv6.Enabled) continue;
                    //if (n.IPv6.Config == null) continue;
                    token = n.token;
                    configuration.IPv6.Manual = new PrefixedIPv6Address[] { new PrefixedIPv6Address() 
                        { Address = "2001:1:1:1:1:1:1:1", PrefixLength = 64 } };
                    if ((n.IPv6.Config != null) && n.IPv6.Config.AcceptRouterAdvertSpecified)
                    {
                        configuration.IPv6.AcceptRouterAdvert = n.IPv6.Config.AcceptRouterAdvert;
                        configuration.IPv6.AcceptRouterAdvertSpecified = true;
                    }
                    break;
                }
                //if (TestIPv6InterfaceByFeature(IPv6Feature, token)) return;

                SetIPConfiguration(token, configuration);

                /*   -- 4 --  */
                NetworkInterface[] currentSettings = GetNetworkInterfaces();

                /*   -- 5 --  */
                NetworkInterface modified = currentSettings.Where(NI => NI.token == token).FirstOrDefault();
                RunNormal("Verifying appliance of IPv6 static settings", new Action(() =>
                {
                    MyAssert(
                        LogLog("Check for interface token = " + token) &&
                        LogAssert(modified, "interface") &&
                        LogAssert(modified.IPv6, "interface data") &&
                        LogAssert(modified.IPv6.Enabled, "IP configuration not enabled") &&
                        LogAssert(modified.IPv6.Config, "IP configuration") &&
                        LogAssertLog(modified.IPv6.Config.DHCP == IPv6DHCPConfiguration.Off, "DHCP {0}applied") &&
                        LogAssert(modified.IPv6.Config.Manual, "IP addresses") &&
#if USE_COLLECTION_COMPARE
                        LogAssertPrefix6(modified.IPv6.Config.Manual, configuration.IPv6.Manual[0])
/*                        LogAssert(
                            modified.IPv6.Config.Manual.Where(NI => NI.Equals(configuration.IPv6.Manual[0])).Count() > 0, 
                            "No IP address in addresses set") &&
                        LogAssert(
                            modified.IPv6.Config.Manual.Where(NI => NI.Equals(configuration.IPv6.Manual[0])).Count() != 1, 
                            "More than one equal IP address in addresses set")*/
#else
                        LogAssert(modified.IPv6.Config.Manual.Count() == configuration.IPv6.Manual.Count(), "IP addresses count not equal") &&
                        LogAssertLog(modified.IPv6.Config.Manual[0].Address == configuration.IPv6.Manual[0].Address, "IP address {0}applied") &&
                        LogAssertLog(modified.IPv6.Config.Manual[0].PrefixLength == configuration.IPv6.Manual[0].PrefixLength, "IP prefix {0}applied")
#endif
                        , "Settings not applied", "Settings applied successfully");
                }));

            },

            // Restore configuration
            (networkInterfaces) =>
            {
                NetworkInterface networkInterface =
                    networkInterfaces.Where(NI => NI.token == token).FirstOrDefault();

                RestoreNetworkInterface(token, networkInterface);
            });
        }


        [Test(Name = "IPV6 STATELESS IP CONFIGURATION - ROUTER ADVERTISEMENT",
            Path = PATH6,
            Order = "02.01.02",
            Id = "2-1-2",
            Category = Category.IPCONFIG,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv6StatelessAdvIPTest()
        {
            NetworkInterface[] originalSettings = null;
            string token = null;
            bool IPv6Feature = Features.Contains(Feature.IPv6);

            RunTest<NetworkInterface[]>(

            new Backup<NetworkInterface[]>(
            () =>
            {
                //if (!IPv6Feature)
                //{
                //    LogStepEvent("IPv6 not supported - negative test mode\r\n");
                //}
                return originalSettings = BackupInterfaces();
            }),
            () =>
            {
                NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
                configuration.Enabled = true;
                configuration.EnabledSpecified = true;
                configuration.IPv6 = new IPv6NetworkInterfaceSetConfiguration();
                configuration.IPv6.Enabled = true;
                configuration.IPv6.EnabledSpecified = true;
                configuration.IPv6.AcceptRouterAdvert = true;
                configuration.IPv6.AcceptRouterAdvertSpecified = true;
                configuration.IPv6.DHCP = IPv6DHCPConfiguration.Off;
                configuration.IPv6.DHCPSpecified = true;
                foreach (NetworkInterface n in originalSettings)
                {
                    if (n.IPv6 == null) continue;
                    //if (!n.IPv6.Enabled) continue;
                    //if (n.IPv6.Config == null) continue;
                    token = n.token;
                    break;
                }
                //if (TestIPv6InterfaceByFeature(IPv6Feature, token)) return;

                SetIPConfiguration(token, configuration);

                /*   -- 4 --  */
                NetworkInterface[] currentSettings = GetNetworkInterfaces();

                /*   -- 5 --  */
                NetworkInterface modified = currentSettings.Where(NI => NI.token == token).FirstOrDefault();
                RunNormal("Verifying appliance of IPv6 advert settings", new Action(() =>
                {
                    MyAssert(
                        LogLog("Check for interface token = " + token) &&
                        LogAssert(modified, "interface") &&
                        LogAssert(modified.IPv6, "interface data") &&
                        LogAssert(modified.IPv6.Enabled, "IP configuration not enabled") &&
                        LogAssert(modified.IPv6.Config, "IP configuration") &&
                        LogAssertLog(modified.IPv6.Config.DHCP == IPv6DHCPConfiguration.Off, "DHCP {0}applied") &&
                        LogAssertLog(modified.IPv6.Config.AcceptRouterAdvert, "Advert {0}applied") &&
                        LogAssert(modified.IPv6.Config.FromRA, "IP advert addresses") &&
                        LogAssert(modified.IPv6.Config.FromRA.Count() > 0, "No IP advert addresses") &&
                        LogLog("Advert address = " + MyV6String(modified.IPv6.Config.FromRA[0]))
                        , "Settings not applied", "Settings applied successfully");
                }));

            },

            // Restore configuration
            (networkInterfaces) =>
            {
                NetworkInterface networkInterface =
                    networkInterfaces.Where(NI => NI.token == token).FirstOrDefault();

                RestoreNetworkInterface(token, networkInterface);
            });
        }


        [Test(Name = "IPV6 STATELESS IP CONFIGURATION - NEIGHBOUR DISCOVERY",
            Path = PATH6,
            Order = "02.01.03",
            Id = "2-1-3",
            Category = Category.IPCONFIG,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv6StatelessDisIPTest()
        {
            NetworkInterface[] originalSettings = null;
            string token = null;
            bool IPv6Feature = Features.Contains(Feature.IPv6);

            RunTest<NetworkInterface[]>(

            new Backup<NetworkInterface[]>(
            () =>
            {
                //if (!IPv6Feature)
                //{
                //    LogStepEvent("IPv6 not supported - negative test mode\r\n");
                //}
                return originalSettings = BackupInterfaces();
            }),
            () =>
            {
                NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
                configuration.Enabled = true;
                configuration.EnabledSpecified = true;
                configuration.IPv6 = new IPv6NetworkInterfaceSetConfiguration();
                configuration.IPv6.Enabled = true;
                configuration.IPv6.EnabledSpecified = true;
                configuration.IPv6.DHCP = IPv6DHCPConfiguration.Off;
                configuration.IPv6.DHCPSpecified = true;
                foreach (NetworkInterface n in originalSettings)
                {
                    if (n.IPv6 == null) continue;
                    //if (!n.IPv6.Enabled) continue;
                    //if (n.IPv6.Config == null) continue;
                    token = n.token;
                    if ((n.IPv6.Config != null) && n.IPv6.Config.AcceptRouterAdvertSpecified)
                    {
                        configuration.IPv6.AcceptRouterAdvert = n.IPv6.Config.AcceptRouterAdvert;
                        configuration.IPv6.AcceptRouterAdvertSpecified = true;
                    }
                    break;
                }
                //if (TestIPv6InterfaceByFeature(IPv6Feature, token)) return;

                SetIPConfiguration(token, configuration);

                /*   -- 4 --  */
                NetworkInterface[] currentSettings = GetNetworkInterfaces();

                /*   -- 5 --  */
                NetworkInterface modified = currentSettings.Where(NI => NI.token == token).FirstOrDefault();
                RunNormal("Verifying appliance of IPv6 LinkLocal settings", new Action(() =>
                {
                    MyAssert(
                        LogLog("Check for interface token = " + token) &&
                        LogAssert(modified, "interface") &&
                        LogAssert(modified.IPv6, "interface data") &&
                        LogAssert(modified.IPv6.Enabled, "IP configuration not enabled") &&
                        LogAssert(modified.IPv6.Config, "IP configuration") &&
                        LogAssertLog(modified.IPv6.Config.DHCP == IPv6DHCPConfiguration.Off, "DHCP {0}applied") &&
                        LogAssert(modified.IPv6.Config.LinkLocal, "IP LinkLocal addresses") &&
                        LogAssert(modified.IPv6.Config.LinkLocal.Count() > 0, "No IP LinkLocal addresses") &&
                        LogLog("LinkLocal address = " + MyV6String(modified.IPv6.Config.LinkLocal[0]))
                        , "Settings not applied", "Settings applied successfully");
                }));

            },

            // Restore configuration
            (networkInterfaces) =>
            {
                NetworkInterface networkInterface =
                    networkInterfaces.Where(NI => NI.token == token).FirstOrDefault();

                RestoreNetworkInterface(token, networkInterface);
            });
        }


        [Test(Name = "IPV6 STATEFUL IP CONFIGURATION",
            Path = PATH6,
            Order = "02.01.04",
            Id = "2-1-4",
            Category = Category.IPCONFIG,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void IPv6StatefullIPTest()
        {
            NetworkInterface[] originalSettings = null;
            string token = null;
            bool IPv6Feature = Features.Contains(Feature.IPv6);

            RunTest<NetworkInterface[]>(

            new Backup<NetworkInterface[]>(
            () =>
            {
                //if (!IPv6Feature)
                //{
                //    LogStepEvent("IPv6 not supported - negative test mode\r\n");
                //}
                return originalSettings = BackupInterfaces();
            }),
            () =>
            {
                NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
                configuration.Enabled = true;
                configuration.EnabledSpecified = true;
                configuration.IPv6 = new IPv6NetworkInterfaceSetConfiguration();
                configuration.IPv6.Enabled = true;
                configuration.IPv6.EnabledSpecified = true;
                configuration.IPv6.DHCP = IPv6DHCPConfiguration.Stateful;
                configuration.IPv6.DHCPSpecified = true;
                foreach (NetworkInterface n in originalSettings)
                {
                    if (n.IPv6 == null) continue;
                    //if (!n.IPv6.Enabled) continue;
                    //if (n.IPv6.Config == null) continue;
                    token = n.token;
                    if ((n.IPv6.Config != null) && n.IPv6.Config.AcceptRouterAdvertSpecified)
                    {
                        configuration.IPv6.AcceptRouterAdvert = n.IPv6.Config.AcceptRouterAdvert;
                        configuration.IPv6.AcceptRouterAdvertSpecified = true;
                    }
                    break;
                }
                //if (TestIPv6InterfaceByFeature(IPv6Feature, token)) return;

                SetIPConfiguration(token, configuration);

                /*   -- 4 --  */
                NetworkInterface[] currentSettings = GetNetworkInterfaces();

                /*   -- 5 --  */
                NetworkInterface modified = currentSettings.Where(NI => NI.token == token).FirstOrDefault();
                RunNormal("Verifying appliance of IPv6 DHCP settings", new Action(() =>
                {
                    MyAssert(
                        LogLog("Check for interface token = " + token) &&
                        LogAssert(modified, "interface") &&
                        LogAssert(modified.IPv6, "interface data") &&
                        LogAssert(modified.IPv6.Enabled, "IP configuration not enabled") &&
                        LogAssert(modified.IPv6.Config, "IP configuration") &&
                        LogAssertLog(modified.IPv6.Config.DHCP == IPv6DHCPConfiguration.Stateful, "DHCP {0}applied") &&
                        LogAssert(modified.IPv6.Config.FromDHCP, "DHCP addresses") &&
                        LogAssert(modified.IPv6.Config.FromDHCP.Count() > 0, "No DHCP addresses") &&
                        LogLog("DHCP address = " + MyV6String(modified.IPv6.Config.FromDHCP[0]))
                        , "Settings not applied", "Settings applied successfully");
                }));

            },

            // Restore configuration
            (networkInterfaces) =>
            {
                NetworkInterface networkInterface =
                    networkInterfaces.Where(NI => NI.token == token).FirstOrDefault();

                RestoreNetworkInterface(token, networkInterface);
            });
        }


///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////


        [Test(Name = "SET NETWORK INTERFACE CONFIGURATION - IPV4",
            Order = "02.01.18",
            Id = "2-1-18",
            Category = Category.DEVICE,
            Path = DeviceManagementNetworkTestSuite.PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void SetNetworkInterfacesIpv4Test()
        {
            IPv4StaticIPTest();
        }

        [Test(Name = "SET NETWORK INTERFACE CONFIGURATION - IPV6",
            Order = "02.01.19",
            Id = "2-1-19",
            Category = Category.DEVICE,
            Path = DeviceManagementNetworkTestSuite.PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.IPv6 },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetNetworkInterfaces, Functionality.SetNetworkInterfaces })]
        public void SetNetworkInterfacesIpv6Test()
        {
            IPv6StaticIPTest();
        }



    }
}
