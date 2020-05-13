using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites.DeviceManagement
{
    [TestClass]
    class DeviceManagementIPFilteringTestSuit: Base.DeviceManagementTest
    {
        public DeviceManagementIPFilteringTestSuit(TestLaunchParam param): base(param)
        {}

        private const string PATH = "Device Management\\IP Filtering";

        [Test(Name = "GET IP ADDRESS FILTER",
            Order = "07.01.01",
            Id = "7-1-1",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.0,
            RequiredFeatures =  new Feature[]{ Feature.IPFilter },
            FunctionalityUnderTest = new Functionality[]{ Functionality.GetIPAddressFilter },
            RequirementLevel = RequirementLevel.Must)]
        public void GetIPAddressFilterTest()
        {
            RunTest(() => GetIPAddressFilter());
        }

        [Test(Name = "SET IP ADDRESS FILTER - IPv4",
            Order = "07.01.02",
            Id = "7-1-2",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.IPFilter },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetIPAddressFilter },
            RequirementLevel = RequirementLevel.Must)]
        public void SetIPAddressFilterIPv4Test()
        {
            IPAddressFilter originalConfig = null;
            bool restoreSettings = false;
            RunTest(() =>
                    {
                        originalConfig = GetIPAddressFilter();

                        var netAddress = NotClientAddress();
                        var uploadedConfig = new IPAddressFilter() { Type = IPAddressFilterType.Deny, IPv4Address = new[] { netAddress } };
                        SetIPAddressFilter(uploadedConfig);
                        restoreSettings = true;

                        var updatedConfig = GetIPAddressFilter();

                        CompareIPAddressFilters(uploadedConfig, updatedConfig);

                        netAddress = ClientAddress();
                        uploadedConfig = new IPAddressFilter() { Type = IPAddressFilterType.Allow, IPv4Address = new[] { netAddress } };
                        SetIPAddressFilter(uploadedConfig);

                        updatedConfig = GetIPAddressFilter();

                        CompareIPAddressFilters(uploadedConfig, updatedConfig);
                    },
                    () =>
                    {
                        if (restoreSettings) SetIPAddressFilter(originalConfig);
                    });
        }

        [Test(Name = "ADD IP ADDRESS FILTER - IPv4",
            Order = "07.01.03",
            Id = "7-1-3",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.IPFilter },
            FunctionalityUnderTest = new Functionality[] { Functionality.AddIPAddressFilter },
            RequirementLevel = RequirementLevel.Must)]
        public void AddIPAddressFilterIPv4Test()
        {
            IPAddressFilter originalConfig = null;
            bool restoreSettings = false;
            RunTest(() =>
                    {
                        originalConfig = GetIPAddressFilter();

                        SetIPAddressFilter(new IPAddressFilter() { Type = IPAddressFilterType.Deny });
                        restoreSettings = true;

                        var netAddress = NotClientAddress();
                        var uploadedConfig = new IPAddressFilter() { Type = IPAddressFilterType.Deny, IPv4Address = new[] { netAddress } };
                        AddIPAddressFilter(uploadedConfig);

                        var updatedConfig = GetIPAddressFilter();

                        CompareIPAddressFilters(uploadedConfig, updatedConfig);
                    },
                    () =>
                    {
                        if (restoreSettings) SetIPAddressFilter(originalConfig);
                    });
        }

        [Test(Name = "REMOVE IP ADDRESS FILTER - IPv4",
            Order = "07.01.04",
            Id = "7-1-4",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.0,
            RequiredFeatures = new Feature[] { Feature.IPFilter },
            FunctionalityUnderTest = new Functionality[] { Functionality.RemoveIPAddressFilter },
            RequirementLevel = RequirementLevel.Must)]
        public void RemoveIPAddressFilterIPv4Test()
        {
            IPAddressFilter originalConfig = null;
            bool restoreSettings = false;
            RunTest(() =>
                    {
                        originalConfig = GetIPAddressFilter();

                        var netAddress = NotClientAddress();
                        var newConfig = new IPAddressFilter()
                            {
                                Type = IPAddressFilterType.Deny,
                                IPv4Address = new[] {netAddress}
                            };
                        SetIPAddressFilter(newConfig);
                        restoreSettings = true;

                        CompareIPAddressFilters(newConfig, GetIPAddressFilter());

                        RemoveIPAddressFilter(new IPAddressFilter() { Type = IPAddressFilterType.Allow, IPv4Address = new[] { netAddress } });

                        var referenceConfig = new IPAddressFilter() { Type = IPAddressFilterType.Deny };
                        CompareIPAddressFilters(referenceConfig, GetIPAddressFilter());
                    },
                    () =>
                    {
                        if (restoreSettings) SetIPAddressFilter(originalConfig);
                    });
        }

        #region Service Commands

        private IPAddressFilter GetIPAddressFilter()
        {
            IPAddressFilter r = null;
            RunStep(() => r = Client.GetIPAddressFilter(), "Get IP Address Filter");

            RunStep(() =>
                    {
                        bool passed = true;

                        if (null != r.IPv4Address)
                            foreach (var ip in r.IPv4Address)
                            { passed = passed && ValidateIPAddress(ip); }


                        if (null != r.IPv6Address)
                            foreach (var ip in r.IPv6Address)
                            { passed = passed && ValidateIPAddress(ip); }
                        
                        if (!passed)
                            throw new FormatException("There is one or more invalid IP ranges in filter");
                    },
                    "Validate received IP Address Filter");

            return r;
        }

        private void SetIPAddressFilter(IPAddressFilter filter)
        {
            RunStep(() => Client.SetIPAddressFilter(filter), "Set IP Address Filter");
        }
        
        private void AddIPAddressFilter(IPAddressFilter filter)
        {
            RunStep(() => Client.AddIPAddressFilter(filter), "Add IP Address Filter");
        }

        private void RemoveIPAddressFilter(IPAddressFilter filter)
        {
            RunStep(() => Client.RemoveIPAddressFilter(filter), "Remove IP Address Filter");
        }

        #endregion

        #region Utils

        private void CompareIPAddressFilters(IPAddressFilter uploaded, IPAddressFilter received)
        {
            const string logPrefix = "    ";
            var log = new StringBuilder();
            bool flag = true;

            if (uploaded.Type != received.Type)
            {
                flag = false;
                log.AppendLine(string.Format("{0}Field 'Type' has unexpected value: expected '{1}', actually '{2}'", logPrefix, uploaded.Type, received.Type));
            }

            var uEmptyv4 = null == uploaded.IPv4Address || !uploaded.IPv4Address.Any();
            var rEmptyv4 = null == received.IPv4Address || !received.IPv4Address.Any();
            if (uEmptyv4 != rEmptyv4)
            {
                flag = false;
                if (uEmptyv4)
                    log.AppendLine(string.Format("{0}Field 'IPv4Address' is empty in uploaded IPAddressFilter and isn't empty in received one.", logPrefix));
                else
                    log.AppendLine(string.Format("{0}Field 'IPv4Address' is empty in received IPAddressFilter and isn't empty in uploaded one.", logPrefix));
            }
            else if (!uEmptyv4)
            {
                if (uploaded.IPv4Address.Count() != received.IPv4Address.Count())
                {
                    flag = false;
                    log.AppendLine(string.Format("{0}Field 'IPv4Address' in received IPAddressFilter has different count of items than in uploaded one.", logPrefix));
                }
                else
                {
                    foreach (var ripa in received.IPv4Address)
                    {
                        var contains = uploaded.IPv4Address.Any(e => e.Address == ripa.Address && e.PrefixLength == ripa.PrefixLength);
                        if (!contains)
                        {
                            flag = false;
                            log.AppendLine(string.Format("{0}Field 'IPv4Address' in received IPAddressFilter contains item than is not present in uploaded one: Address '{1}', PrefixLength '{2}'", 
                                           logPrefix, ripa.Address, ripa.PrefixLength));
                            break;
                        }
                    }
                }
            }

            //var uEmptyv6 = null == uploaded.IPv6Address || !uploaded.IPv6Address.Any();
            //var rEmptyv6 = null == received.IPv6Address || !received.IPv6Address.Any();
            //if (uEmptyv6 != rEmptyv6)
            //{
            //    flag = false;
            //    if (uEmptyv6)
            //        log.AppendLine(string.Format("{0}Field 'IPv6Address' is empty in uploaded IPAddressFilter and isn't empty in received one.", logPrefix));
            //    else
            //        log.AppendLine(string.Format("{0}Field 'IPv6Address' is empty in received IPAddressFilter and isn't empty in uploaded one.", logPrefix));
            //}
            //else if (!uEmptyv6)
            //{
            //    if (uploaded.IPv6Address.Count() != received.IPv6Address.Count())
            //    {
            //        flag = false;
            //        log.AppendLine(string.Format("{0}Field 'IPv6Address' in received IPAddressFilter has different count of items than in uploaded one.", logPrefix));
            //    }
            //    else
            //    {
            //        foreach (var ripa in received.IPv6Address)
            //        {
            //            var contains = uploaded.IPv6Address.Any(e => e.Address == ripa.Address && e.PrefixLength == ripa.PrefixLength);
            //            if (!contains)
            //            {
            //                flag = false;
            //                log.AppendLine(string.Format("{0}Field 'IPv6Address' in received IPAddressFilter contains item than is not present in uploaded one: Address '{1}', PrefixLength '{2}'",
            //                               logPrefix, ripa.Address, ripa.PrefixLength));
            //                break;
            //            }
            //        }
            //    }
            //}

            Assert(flag,
                   string.Format("Received IPAddressFilter is different from uploaded one:{0}{1}", Environment.NewLine, log),
                   "Compare uploaded and received IPAddressFilters");
        }

        private PrefixedIPv4Address ClientAddress()
        {
            return new PrefixedIPv4Address() { Address = _nic.IP.ToString(), PrefixLength = 32 };
        }

        private PrefixedIPv4Address NotClientAddress()
        {
            var addr = _nic.IP.GetAddressBytes();
            addr[3] += 1;
            addr[3] %= 255;
            if (0 == addr[3]) addr[3] = 1;

            return new PrefixedIPv4Address() { Address = new System.Net.IPAddress(addr).ToString(), PrefixLength = 32 };
        }

        private bool ValidateIPAddressCore(string ipString, int specifiedPrefixLength, int bitsSize)
        {
            string ipVersion = 128 == bitsSize ? "6" : "4";
            string logPrefix = Environment.NewLine + "    ";

            var log = new StringBuilder(string.Format("Validation of IPv{0} range '{1}/{2}': ", ipVersion, ipString, specifiedPrefixLength));
            try
            {

                System.Net.IPAddress ip;
                if (!System.Net.IPAddress.TryParse(ipString, out ip) || 32 == bitsSize && ip.ToString() != ipString)
                {
                    log.Append(string.Format("{0}Wrong net address", logPrefix));
                    return false;
                }
                else if (32 == bitsSize && ip.AddressFamily != AddressFamily.InterNetwork)
                {
                    log.Append(string.Format("{0}Specified range isn't valid IPv4 range", logPrefix));
                    return false;
                }
                else if (128 == bitsSize && ip.AddressFamily != AddressFamily.InterNetworkV6)
                {
                    log.Append(string.Format("{0}Specified range isn't valid IPv6 range", logPrefix));
                    return false;
                }
                else
                {
                    var bytesSize = bitsSize / 8;
                    var addressBytes = ip.GetAddressBytes();
                    if (bytesSize != addressBytes.Count())
                    {
                        log.Append(string.Format("{0}Specified range isn't valid IPv{1} range", logPrefix, ipVersion));
                        return false;
                    }
                    else
                    {
                        if (0 > specifiedPrefixLength || specifiedPrefixLength > bitsSize)
                        {
                            log.Append(string.Format("{0}Prefix length should be in range from 0 to {1}", logPrefix, bitsSize));
                            return false;
                        }
                        else
                        {
                            for (int i = 0; i < bitsSize - specifiedPrefixLength; i++)
                            {
                                if (0 != (addressBytes[addressBytes.Count() - 1 - i / 8] & (1 << (i % 8))))
                                {
                                    log.Append(string.Format("{0}Specified range has not enough zero bits in tail", logPrefix));
                                    return false;
                                }
                            }
                        }
                    }
                }

                log.Append("OK");

                return true;
            }
            finally 
            {
                LogStepEvent(log.ToString());
            }
        }

        private bool ValidateIPAddress(PrefixedIPv4Address ip)
        {
            return ValidateIPAddressCore(ip.Address, ip.PrefixLength, 32);
        }

        private bool ValidateIPAddress(PrefixedIPv6Address ip)
        {
            return ValidateIPAddressCore(ip.Address, ip.PrefixLength, 128);
        }

        #endregion
    }
}
