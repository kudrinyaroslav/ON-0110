///////////////////////////////////////////////////////////////////////////
//!  @author        Ekaterina Nefedova
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
using WSD = TestTool.Proxies.WSDiscovery;

using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.Utils
{
    public class NetworkConfiguration : Base.DeviceDiscoveryTest
    {
        BaseTest Test;

        //DeviceClient Client;

        public NetworkConfiguration(TestLaunchParam param, BaseTest test)
            : base(param)
        {
            Test = test;

            base.Initialize(test);

            Test.SetNesting(this);

            //OnStepStarted += new Action<StepResult>(test_OnStepStarted);
            //test.OnStepStarted += new Action<StepResult>(Test.OnStepStarted);
        }

        //void test_OnStepStarted(StepResult obj)
        //{
            //if (Test.OnStepStarted != null)
            //{
            //    Test.OnStepStarted(obj);
            //}
        //}

        private string AddressBackup = "";
        bool BackupIPv6 = false;

        #region SupportingFunctions

        #region ConfiguratingFunctions

        protected NetworkInterface[] BackupInterfaces()
        {
            BackupConnection();

            NetworkInterface[] interfaces = GetNetworkInterfaces(Test);

            Test.Assert(interfaces != null && interfaces.Length > 0,
                "No interfaces returned from the DUT",
                "Check that the DUT returned current interfaces");

            return interfaces;
        }
        protected void BackupConnection()
        {
            AddressBackup = _endpointController.Address.ToString();
            BackupIPv6 = AddressBackup.LastIndexOf('[') >= 0;
            //AddressBackup = Client.Endpoint.Address.ToString();
        }
        void RestoreConnection(string addressBackup)
        {
            _endpointController.UpdateAddress(new EndpointAddress(addressBackup));
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
                    Test.LogStepEvent("IPv4 DHCP = " + configuration.IPv4.DHCP);
                }
                if ((configuration.IPv4.Manual != null) &&
                    (configuration.IPv4.Manual.Count() == 1))
                {
                    if (null != configuration.IPv4.Manual.First())
                        Test.LogStepEvent("IPv4 Address = " + MyV4String(configuration.IPv4.Manual.First()));
                    else
                        Test.LogStepEvent("IPv4 Address = {Warning: empty address}");
                }
            }
            if ((configuration.IPv6 != null) &&
                (configuration.IPv6.EnabledSpecified) &&
                (configuration.IPv6.Enabled))
            {
                if (configuration.IPv6.DHCPSpecified)
                {
                    Test.LogStepEvent("IPv6 DHCP = " + configuration.IPv6.DHCP.ToString());
                }
                if (configuration.IPv6.AcceptRouterAdvertSpecified)
                {
                    Test.LogStepEvent("IPv6 Advert = " + configuration.IPv6.AcceptRouterAdvert);
                }
                if ((configuration.IPv6.Manual != null) &&
                    (configuration.IPv6.Manual.Count() == 1))
                {
                    if (null != configuration.IPv6.Manual.First())
                        Test.LogStepEvent("IPv6 Address = " + MyV6String(configuration.IPv6.Manual.First()));
                    else
                        Test.LogStepEvent("IPv6 Address = {Warning: empty address}");
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
                if (!BackupIPv6)
                {
                    return AddressBackup;
                }
                if ((configuration.IPv6.Manual != null) &&
                    (configuration.IPv6.Manual.Count() == 1))
                {
                    SetIP = "[" + configuration.IPv6.Manual[0].Address + "]";
                }
            }
            if (string.IsNullOrEmpty(SetIP))
            {
                return AddressBackup;
            }

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
                if (pos <= 0)
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
                        Test.BeginStep(restore ? "Restore network settings" : "Set network interface");
                        if (!restore)
                        {
                            Test.LogStepEvent("interface token = " + interfaceToken);
                            LogConfiguration(configuration);
                        }
                        rebootNeeded = SetNetworkInterface(interfaceToken, configuration);
                        //if (!restore)
                        {
                            Test.LogStepEvent("reboot need = " + rebootNeeded);
                        }
                        Test.StepPassed();

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
                    Test.LogStepEvent("Warning: no Hello within timeout");
                    StepPassed();

                    if (restore)
                    {
                        RaiseNetworkSettingsChangedEvent(AddressBackup);
                        RestoreConnection(AddressBackup);
                        return;
                    }

                    // should not occur, hack
                    if (AddressBackup == null)
                    {
                        return;
                    }

                    BeginStep("No Hello - guessing right address");
                    string newServiceAddr = GuessAddress(configuration);
                    Test.LogStepEvent("Selecting " + newServiceAddr);

                    // raise event that network settings changed
                    RaiseNetworkSettingsChangedEvent(newServiceAddr);

                    ReConnectTo(newServiceAddr);
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
                    LogAssert(hello.Object.XAddrs, "hello service address(es) value") &&
                    LogLog("Service Address(es) = " + hello.Object.XAddrs), "Hello not verified",
                    "Hello verified successfully");
            }));

            {
                BeginStep("Identifying right address");
                if (AddressBackup != null)
                    Test.LogStepEvent("Old address " + AddressBackup);
                string[] addresses = hello.Object.XAddrs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string newServiceAddr = AddressBackup;
                int count = 0;
                foreach (string s in addresses)
                {
                    int c = FindPrefixLength(s, AddressBackup);
                    if (c > count)
                    {
                        newServiceAddr = s;
                        count = c;
                    }
                }
                Test.LogStepEvent("Selecting " + newServiceAddr);

                // raise event that network settings changed
                RaiseNetworkSettingsChangedEvent(newServiceAddr);

                ReConnectTo(newServiceAddr);
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

        // to be unified with SetIPConfigurationInt
        void SetZeroConfigurationBis(string interfaceToken, bool enabled)
        {
            SoapMessage<WSD.HelloType> hello = null;
            try
            {
                hello = ReceiveHelloMessage(
                    false,
                    true,
                    new Action(() =>
                    {
                        SetZeroConfiguration(interfaceToken, enabled);
                    }));
            }
            catch (AssertException)
            {
                Test.LogStepEvent("Warning: no Hello within timeout");
                StepPassed();
                return; // no strict requirement for hello in this case
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
                    LogAssert(hello.Object.XAddrs, "hello service address(es) value") &&
                    LogLog("Service Address(es) = " + hello.Object.XAddrs), "Hello not verified",
                    "Hello verified successfully");
            }));

            {
                BeginStep("Identifying right address");
                if (AddressBackup != null)
                    Test.LogStepEvent("Old address " + AddressBackup);
                string[] addresses = hello.Object.XAddrs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string newServiceAddr = AddressBackup;
                int count = 0;
                foreach (string s in addresses)
                {
                    int c = FindPrefixLength(s, AddressBackup);
                    if (c > count)
                    {
                        newServiceAddr = s;
                        count = c;
                    }
                }
                Test.LogStepEvent("Selecting " + newServiceAddr);
                RaiseNetworkSettingsChangedEvent(newServiceAddr);
                ReConnectTo(newServiceAddr);
                StepPassed();
            }
        }

        void ExtractIPfromServiceAddress(string newServiceAddress, out string IP)
        {
            IP = null;

            // IPv6
            if (newServiceAddress.Contains("["))
            {
                int startPos = newServiceAddress.IndexOf("[") + 1;
                int endPos = newServiceAddress.IndexOf("]");

                if (endPos > startPos)
                    IP = newServiceAddress.Substring(startPos, endPos - startPos);
            }
            // IPv4
            else
            {
                int startPos = newServiceAddress.IndexOf("//") + 2;

                //if service address contains ":port"
                int colonPos = -1;
                if ((colonPos = newServiceAddress.IndexOf(":", startPos)) != -1)
                    IP = newServiceAddress.Substring(startPos, colonPos - startPos);
                //if service address doesn't contain ":port"
                else
                {
                    int slashPos = newServiceAddress.IndexOf("/", startPos);
                    IP = newServiceAddress.Substring(startPos, slashPos - startPos);
                }
            }
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
            Test.LogStepEvent(posMessage);
        }
        bool LogAssertComp(object obj1, object obj2, string Message)
        {
            if (obj1.Equals(obj2))
            {
                Test.LogStepEvent(Message + " are equal [" + obj1.ToString() + "]");
                return true;
            }
            else
            {
                Test.LogStepEvent(Message + " are not equal [" + obj1.ToString() + " != " + obj2.ToString() + "]");
                return false;
            }
        }
        bool LogLog(string Message)
        {
            Test.LogStepEvent(Message);
            return true;
        }
        bool LogAssert(bool value, string negMessage)
        {
            if (!value)
            {
                Test.LogStepEvent(negMessage);
            }
            return value;
        }
        bool LogAssert(object obj, string negMessage)
        {
            return LogAssert(obj != null, negMessage + " undefined");
        }
        bool LogAssertLog(bool value, string Message)
        {
            Test.LogStepEvent(string.Format(Message, value ? "" : "not "));
            return value;
        }
        bool LogAssert(bool value, string negMessage, string posMessage)
        {
            Test.LogStepEvent(value ? posMessage : negMessage);
            return value;
        }

        bool LogAssertVal(object obj, string Name)
        {
            if (obj == null)
            {
                Test.LogStepEvent(Name + " undefined");
                return false;
            }
            else
            {
                Test.LogStepEvent(Name + " = " + obj.ToString());
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

        void SelectIPv6(ref string oldServiceAddr, ref string newServiceAddr, string IPv6)
        {
            Test.BeginStep("Switching to IPv6 address");
            if (AddressBackup != null)
            {
                Test.LogStepEvent("Old address " + AddressBackup);
                oldServiceAddr = AddressBackup;
            }

            newServiceAddr = "http://" + "[" + IPv6 + "]" + "/onvif/device_service";
            Test.LogStepEvent("Selecting " + newServiceAddr);

            Test.RaiseNetworkSettingsChangedEvent(newServiceAddr);
            ReConnectTo(newServiceAddr);
            Test.StepPassed();
        }

        void EnableIPv6(string intToken, ref string oldServiceAddr, 
            ref string newServiceAddr, out bool restoreEnabled)
        {
            NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
            configuration.IPv6 = new IPv6NetworkInterfaceSetConfiguration();
            configuration.IPv6.Enabled = true;
            configuration.IPv6.EnabledSpecified = true;

            SetIPConfiguration(intToken, configuration);

            // verify if IPv6 settings were applied
            NetworkInterface[] currentSettings = GetNetworkInterfaces();
            NetworkInterface modified = currentSettings.Where(NI => NI.token == intToken).FirstOrDefault();

            Test.Assert(modified.IPv6 != null && modified.IPv6.Enabled,
                "IPv6 settings were not applied",
                "Verifying appliance of IPv6 settings");

            restoreEnabled = true;

            if (modified.IPv6.Config.DHCP == IPv6DHCPConfiguration.Off)
            {
                // Manual tag exists
                if (modified.IPv6.Config.Manual != null &&
                    modified.IPv6.Config.Manual.Length != 0 &&
                    !string.IsNullOrEmpty(modified.IPv6.Config.Manual[0].Address))
                {
                    string IPv6 = modified.IPv6.Config.Manual[0].Address;
                    SelectIPv6(ref oldServiceAddr, ref newServiceAddr, IPv6);
                }
                // LinkLocal tag exists
                else if (modified.IPv6.Config.LinkLocal != null &&
                    modified.IPv6.Config.LinkLocal.Length != 0 &&
                    !string.IsNullOrEmpty(modified.IPv6.Config.LinkLocal[0].Address))
                {
                    string IPv6 = modified.IPv6.Config.LinkLocal[0].Address;
                    SelectIPv6(ref oldServiceAddr, ref newServiceAddr, IPv6);
                }
                else
                {
                    Test.Assert(false, "Manual or LinkLocal IPv6 addresses were not found", 
                        "Searching for Manual or LinkLocal IPv6 address");
                }
            }
            else
            {
                // FromDHCP tag exists
                if (modified.IPv6.Config.FromDHCP != null &&
                    modified.IPv6.Config.FromDHCP.Length != 0 &&
                    !string.IsNullOrEmpty(modified.IPv6.Config.FromDHCP[0].Address))
                {
                    string IPv6 = modified.IPv6.Config.FromDHCP[0].Address;
                    SelectIPv6(ref oldServiceAddr, ref newServiceAddr, IPv6);
                }
                // LinkLocal tag exists
                else if (modified.IPv6.Config.LinkLocal != null &&
                    modified.IPv6.Config.LinkLocal.Length != 0 &&
                    !string.IsNullOrEmpty(modified.IPv6.Config.LinkLocal[0].Address))
                {
                    string IPv6 = modified.IPv6.Config.LinkLocal[0].Address;
                    SelectIPv6(ref oldServiceAddr, ref newServiceAddr, IPv6);
                }
                else
                {
                    Test.Assert(false, "FromDHCP or LinkLocal IPv6 addresses were not found",
                        "Searching for Manual or LinkLocal IPv6 address");
                }
            }
        }

        // A.23 - Turn on IPv6
        public NetworkInterface TurnOnIPv6(out string newServiceAddr, out string oldServiceAddr, 
            out bool restoreEnabled)
        {
            newServiceAddr = null;
            oldServiceAddr = null;
            restoreEnabled = false;

            bool IPv6Feature = Features.Contains(Feature.IPv6);
            if (!IPv6Feature)
                Test.Assert(IPv6Feature, "IPv6 not supported", "Check if IPv6 is supported");

            // backup interfaces
            NetworkInterface[] interfaces = BackupInterfaces();
            NetworkInterface selectedInterface = null;

            // select current network interface to search for IPv6 address in it
            string intToken = null;
            foreach (var item in interfaces)
            {
                if (BackupIPv6)
                {
                    // search in Manual tag
                    if (item.Enabled &&
                        item.IPv6 != null && item.IPv6.Config != null &&
                        item.IPv6.Config.Manual != null && item.IPv6.Config.Manual.Length != 0 &&
                        !string.IsNullOrEmpty(item.IPv6.Config.Manual[0].Address))
                    {
                        string ipAddress = null;
                        ExtractIPfromServiceAddress(AddressBackup, out ipAddress);

                        if (item.IPv6.Config.Manual[0].Address == ipAddress)
                        {
                            intToken = item.token;
                            selectedInterface = item;
                            break;
                        }
                    }
                    // search in LinkLocal tag
                    else if (item.Enabled &&
                                item.IPv6 != null && item.IPv6.Config != null &&
                                item.IPv6.Config.LinkLocal != null && item.IPv6.Config.LinkLocal.Length != 0 &&
                                !string.IsNullOrEmpty(item.IPv6.Config.LinkLocal[0].Address))
                    {
                        string ipAddress = null;
                        ExtractIPfromServiceAddress(AddressBackup, out ipAddress);

                        if (item.IPv6.Config.LinkLocal[0].Address == ipAddress)
                        {
                            intToken = item.token;
                            selectedInterface = item;
                            break;
                        }
                    }
                }
                else
                {
                    // search in Manual tag
                    if (item.Enabled && 
                        item.IPv4 != null && item.IPv4.Config != null &&
                        item.IPv4.Config.Manual != null && item.IPv4.Config.Manual.Length != 0 && 
                        !string.IsNullOrEmpty(item.IPv4.Config.Manual[0].Address))
                    {
                        string ipAddress = null;
                        ExtractIPfromServiceAddress(AddressBackup, out ipAddress);

                        if (item.IPv4.Config.Manual[0].Address == ipAddress)
                        {
                            intToken = item.token;
                            selectedInterface = item;
                            break;
                        }
                    }
                    // search in LinkLocal tag
                    else if (item.Enabled &&
                       item.IPv4 != null && item.IPv4.Config != null &&
                       item.IPv4.Config.LinkLocal != null &&
                       !string.IsNullOrEmpty(item.IPv4.Config.LinkLocal.Address))
                    {
                        string ipAddress = null;
                        ExtractIPfromServiceAddress(AddressBackup, out ipAddress);

                        if (item.IPv4.Config.LinkLocal.Address == ipAddress)
                        {
                            intToken = item.token;
                            selectedInterface = item;
                            break;
                        }
                    }
                }
            }

            Test.Assert(!string.IsNullOrEmpty(intToken), 
                "Current network interface not found", 
                "Search for current network interface");

            // select IPv6
            if (BackupIPv6)
            {
                // do nothing, use current IPv6 and current network interface
                // nothing needed to be restored
                return selectedInterface;
            }
            else if (!BackupIPv6)
            {
                if (selectedInterface.IPv6 != null)
                {
                    if (selectedInterface.IPv6.Enabled)
                    {
                        if (selectedInterface.IPv6.Config.DHCP == IPv6DHCPConfiguration.Off)
                        {
                            // Manual tag exists
                            if (selectedInterface.IPv6.Config.Manual != null &&
                                selectedInterface.IPv6.Config.Manual.Length != 0 &&
                                !string.IsNullOrEmpty(selectedInterface.IPv6.Config.Manual[0].Address))
                            {
                                string IPv6 = selectedInterface.IPv6.Config.Manual[0].Address;
                                SelectIPv6(ref oldServiceAddr, ref newServiceAddr, IPv6); 
                            }
                            // LinkLocal tag exists
                            else if (selectedInterface.IPv6.Config.LinkLocal != null &&
                                selectedInterface.IPv6.Config.LinkLocal.Length != 0 &&
                                !string.IsNullOrEmpty(selectedInterface.IPv6.Config.LinkLocal[0].Address))
                            {
                                string IPv6 = selectedInterface.IPv6.Config.LinkLocal[0].Address;
                                SelectIPv6(ref oldServiceAddr, ref newServiceAddr, IPv6); 
                            }
                            else
                            {
                                Test.Assert(false, "Manual or LinkLocal IPv6 addresses were not found", 
                                    "Searching for Manual or LinkLocal IPv6 address");
                            }
                        }
                        else
                        {
                            // FromDHCP tag exists
                            if (selectedInterface.IPv6.Config.FromDHCP != null &&
                                selectedInterface.IPv6.Config.FromDHCP.Length != 0 &&
                                !string.IsNullOrEmpty(selectedInterface.IPv6.Config.FromDHCP[0].Address))
                            {
                                string IPv6 = selectedInterface.IPv6.Config.FromDHCP[0].Address;
                                SelectIPv6(ref oldServiceAddr, ref newServiceAddr, IPv6);
                            }
                            // LinkLocal tag exists
                            else if (selectedInterface.IPv6.Config.LinkLocal != null &&
                                selectedInterface.IPv6.Config.LinkLocal.Length != 0 &&
                                !string.IsNullOrEmpty(selectedInterface.IPv6.Config.LinkLocal[0].Address))
                            {
                                string IPv6 = selectedInterface.IPv6.Config.LinkLocal[0].Address;
                                SelectIPv6(ref oldServiceAddr, ref newServiceAddr, IPv6);
                            }
                            else
                            {
                                Test.Assert(false, "FromDHCP or LinkLocal IPv6 addresses were not found",
                                    "Searching for Manual or LinkLocal IPv6 address");
                            }
                        }
                    }
                    else
                    { 
                        // setup IPv6 interface as enabled
                        EnableIPv6(intToken, ref oldServiceAddr, ref newServiceAddr, out restoreEnabled/*, ref restoreDHCP, out dhcp*/);
                    }
                }
                else
                {
                    // setup IPv6 interface as enabled
                    EnableIPv6(intToken, ref oldServiceAddr, ref newServiceAddr, out restoreEnabled/*, ref restoreDHCP, out dhcp*/);
                }
            }

            return selectedInterface;
        }

        // A.24 - Restore network settings
        public void RestoreNetworkSettings(string oldServiceAddr, string newServiceAddr, 
            bool restoreEnabled, string intToken)
        {
            if (!string.IsNullOrEmpty(oldServiceAddr) &&
                !string.IsNullOrEmpty(newServiceAddr))
            {
                Test.BeginStep("Switching to previous IP address");

                Test.LogStepEvent("Old address " + newServiceAddr);
                Test.LogStepEvent("Selecting " + oldServiceAddr);

                Test.RaiseNetworkSettingsChangedEvent(oldServiceAddr);
                ReConnectTo(oldServiceAddr);

                Test.StepPassed();
            }

            if (restoreEnabled)
            {
                NetworkInterfaceSetConfiguration configuration = new NetworkInterfaceSetConfiguration();
                configuration.IPv6 = new IPv6NetworkInterfaceSetConfiguration();
                configuration.IPv6.Enabled = false;
                configuration.IPv6.EnabledSpecified = true;

                SetIPConfigurationInt(intToken, configuration, true);
            }
        }

    }
}
