﻿///////////////////////////////////////////////////////////////////////////
//!  @authors        Anna Tarasova, Ivan Vagunin
////
using System;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using Net = System.Net;
using TestTool.Proxies.Onvif;
using System.Xml;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace TestTool.Tests.TestCases
{
    /// <summary>
    /// Class holding extension validation methods.
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Hostname regular expression
        /// </summary>
        private static Regex _hostNamePattern =  new Regex("^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\\-]*[a-zA-Z0-9])\\.)*([A-Za-z]|[A-Za-z][A-Za-z0-9\\-]*[A-Za-z0-9])$");
        
        /// <summary>
        /// IP 4 regular expression
        /// </summary>
        private static Regex _ipv4Pattern = new Regex(@"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");

        /// <summary>
        /// Checks if string passed represents is valid hostname.
        /// </summary>
        /// <param name="hostname"></param>
        /// <returns></returns>
        private static bool IsValidHostName(string hostname)
        {
            return _hostNamePattern.IsMatch(hostname);
        }
        
        /// <summary>
        /// Checks if array passed contains only valid hostname information.
        /// </summary>
        /// <param name="hosts">Hosts array.</param>
        /// <param name="ipAddressMandatory">True, if IP address cannot be omitted.</param>
        /// <param name="reason">Failure reason.</param>
        /// <returns></returns>
        private static bool AreValidHosts(IEnumerable<NetworkHost> hosts, bool ipAddressMandatory, out string reason)
        {
            reason = string.Empty;
            foreach (NetworkHost host in hosts)
            {
                Net.IPAddress parsedAddress;
                if(host.Type == NetworkHostType.IPv4)
                {
                    if (string.IsNullOrEmpty(host.IPv4Address))
                    {
                        if (ipAddressMandatory)
                        {
                            reason = "IPv4 address is null or empty";
                            return false;
                        }
                    }
                    else
                    {
                        if (!Net.IPAddress.TryParse(host.IPv4Address, out parsedAddress) ||
                            (parsedAddress.AddressFamily != Net.Sockets.AddressFamily.InterNetwork))
                        {
                            reason = "Incorrect IPv4 address";
                            return false;
                        }                        
                    }
                }
                else if(host.Type == NetworkHostType.IPv6)
                {
                    if (string.IsNullOrEmpty(host.IPv6Address))
                    {
                        if (ipAddressMandatory)
                        {
                            reason = "IPv6 address is null or empty";
                            return false;
                        }
                    }
                    else
                    {
                        if (!Net.IPAddress.TryParse(host.IPv6Address, out parsedAddress) ||
                            (parsedAddress.AddressFamily != Net.Sockets.AddressFamily.InterNetworkV6))
                        {
                            reason = "Incorrect IPv6 address";
                            return false;
                        }
                    }
                }
                else if(host.Type == NetworkHostType.DNS)
                {
                    if(!IsValidHostName(host.DNSname))
                    {
                        reason = "Invalid host name";
                        return false;
                    }
                }
            }
            return true;
        }
        
        /// <summary>
        /// Checks if string passed represents a valid IPv4 address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsValidIPv4Address(this string address)
        {
            return _ipv4Pattern.IsMatch(address);
        }
       
        /// <summary>
        /// Checks if structure passed represents a valid IP address.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mandatory">True if address cannot be omitted.</param>
        /// <param name="reason">Failure reason.</param>
        /// <returns></returns>
        public static bool IsValidIPAddress(IPAddress address, bool mandatory, out string reason)
        {
            reason = null;
            if (address.Type == IPType.IPv4)
            {
                if (string.IsNullOrEmpty(address.IPv4Address))
                {
                    if (mandatory)
                    {
                        reason = string.Format("Empty IPv4 address");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                //System.Net.IPAddress.TryParse parses incorrect addresses
                if (!IsValidIPv4Address(address.IPv4Address))
                {
                    reason = string.Format("Incorrect IPv4 address ({0})", address.IPv4Address);
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(address.IPv6Address))
                {
                    if (mandatory)
                    {
                        reason = string.Format("Empty IPv6 address");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                Net.IPAddress parsedAddress = null;
                if (!Net.IPAddress.TryParse(address.IPv6Address, out parsedAddress) ||
                    (parsedAddress.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6))
                {
                    reason = string.Format("Incorrect IPv6 address ({0})", address.IPv6Address);
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// Checks if all addresses in an annay are valid.
        /// </summary>
        /// <param name="addresses">Array of addresses.</param>
        /// <param name="mandatory">True, if address cannot be omitted.</param>
        /// <param name="reason">Failure reason.</param>
        /// <returns></returns>
        private static bool AreValidAddresses(IEnumerable<IPAddress> addresses, bool mandatory, out string reason)
        {
            reason = string.Empty;
            foreach (IPAddress address in addresses)
            {
                if(!IsValidIPAddress(address, mandatory, out reason))
                {
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// Checks if DNS information is valid.
        /// </summary>
        /// <param name="dnsInformation">DNS information.</param>
        /// <param name="ipAddressMandatory">True, if IP address cannot be omitted.</param>
        /// <param name="reason">Failure reason.</param>
        /// <returns></returns>
        public static bool IsValidDnsInformation(this DNSInformation dnsInformation, bool ipAddressMandatory, out string reason)
        {
            reason = string.Empty;
            if (dnsInformation.FromDHCP)
            {
                if ((dnsInformation.DNSManual != null) && 
                    (dnsInformation.DNSManual.Length > 0))
                {
                    reason = "DNSManual is not empty, while FromDHCP is true";
                    return false;
                }
                if ((dnsInformation.DNSFromDHCP != null) && 
                    !AreValidAddresses(dnsInformation.DNSFromDHCP, ipAddressMandatory, out reason))
                {
                    return false;
                }
            }
            else
            {
                if ((dnsInformation.DNSFromDHCP != null) && 
                    (dnsInformation.DNSFromDHCP.Length > 0))
                {
                    reason = "DNSFromDHCP is not empty, while FromDHCP is false";
                    return false;
                }
                if ((dnsInformation.DNSManual != null) && 
                    !AreValidAddresses(dnsInformation.DNSManual, ipAddressMandatory, out reason))
                {
                    return false;
                }
            }
            if(dnsInformation.SearchDomain != null)
            {
                foreach (string hostname in dnsInformation.SearchDomain)
                {
                    if(!IsValidHostName(hostname))
                    {
                        reason = string.Format("Invalid host name ({0})", hostname);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if NTP information is valid.
        /// </summary>
        /// <param name="ntpInformation">NTP information.</param>
        /// <param name="ipAddressesMandatory">True, if IP address cannot be omitted.</param>
        /// <param name="reason">Failure reason.</param>
        /// <returns></returns>
        public static bool IsValidNTPInformation(this NTPInformation ntpInformation, bool ipAddressesMandatory, out string reason)
        {
            reason = string.Empty;
            if(ntpInformation.FromDHCP)
            {
                if ((ntpInformation.NTPManual != null) && (ntpInformation.NTPManual.Length > 0))
                {
                    reason = "NTPManual is not empty, while FromDHCP is true";
                    return false;
                }
                if ((ntpInformation.NTPFromDHCP != null) && 
                    !AreValidHosts(ntpInformation.NTPFromDHCP, ipAddressesMandatory, out reason))
                {
                    return false;
                }
            }
            else
            {
                if ((ntpInformation.NTPFromDHCP != null) && (ntpInformation.NTPFromDHCP.Length > 0))
                {
                    reason = "NTPFromDHCP is not empty, while FromDHCP is false";
                    return false;
                }
                if ((ntpInformation.NTPManual != null) && !AreValidHosts(ntpInformation.NTPManual, ipAddressesMandatory, out reason))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if data contains valid JPEG image.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsJpeg(this byte[] data)
        {
            try
            {
                MemoryStream ioStream = new MemoryStream(data);
                Bitmap bm = new Bitmap(ioStream);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
 
        [DllImport("iphlpapi.dll", ExactSpelling = true)] static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

        private static bool PeekARP(string address)
        {
            System.Net.IPAddress adr = System.Net.IPAddress.Parse(address);
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            return SendARP((int)adr.Address, 0, macAddr, ref macAddrLen) != 0;
        }
        private static bool PeekARP(System.Net.IPAddress adr)
        {
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            int Ret = SendARP((int)adr.Address, 0, macAddr, ref macAddrLen);
            System.Diagnostics.Trace.WriteLine(string.Format("Peeking {0} as {1}", adr.ToString(), Ret));
            return Ret == 0;
        }

        public static PrefixedIPv4Address NextAddress(PrefixedIPv4Address Orig)
        {
            System.Net.IPAddress adr = null;
            System.Diagnostics.Trace.WriteLine("Parsing address" + Orig.Address);
            if (!System.Net.IPAddress.TryParse(Orig.Address, out adr))
            {
                System.Diagnostics.Trace.WriteLine("Failed with parsing");
                return Orig;
            }
            long from = (adr.Address >> 24) & 0xff;
            long i;
            // do ARP scanning, after current
            for (i = from+1; i <= 254; i++)
            {
                adr.Address = (adr.Address & 0xffffff) | (i << 24);
                if (!PeekARP(adr)) break;
            }
            if (i >= 254)
            {
                // do ARP scanning, before current
                for (i = 1; i < from; i++)
                {
                    adr.Address = (adr.Address & 0xffffff) | (i << 24);
                    if (!PeekARP(adr)) break;
                }
                // nothing found - just guess
                if (i >= from)
                {
                    i = from + 1;
                    if (i >= 254) i = 1;
                    adr.Address = (adr.Address & 0xffffff) | (i << 24);
                }
            }
            return new PrefixedIPv4Address() { Address = adr.ToString(), PrefixLength = Orig.PrefixLength };
        }

        public static PrefixedIPv4Address GetAvailableIPv4Address(this NetworkInterface[] interfaces, out string interfaceToken)
        {
            PrefixedIPv4Address address = null;
            interfaceToken = null;

            foreach (NetworkInterface n in interfaces)
            {
                if (n.IPv4 == null) continue;
                if (!n.IPv4.Enabled) continue;
                if (n.IPv4.Config == null) continue;
                PrefixedIPv4Address adr = null;
                if (n.IPv4.Config.DHCP)
                {
                    adr = n.IPv4.Config.FromDHCP;
                }
                else
                {
                    if (n.IPv4.Config.Manual.Length > 0)
                        adr = n.IPv4.Config.Manual[0];
                }
                if (adr == null) continue;
                // TODO - introduce correct address modification
                string a = adr.Address;
                if (a.Length < 2) continue;

                address = NextAddress(adr);
                interfaceToken = n.token;
                break;
            }

            return address;
        }
        
        /// <summary>
        /// Creates string which does not match any of strings passed (for negative tests)
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static string GetNonMatchingString(this IEnumerable<string> strings)
        {
            StringBuilder result = new StringBuilder();
            int i = 0;
            foreach (string str in strings)
            {
                char c = 'X';
                if (i < str.Length)
                {
                    c = str[i];

                    if (( (c >= 'a') && (c < 'z')) || ((c >= 'A') && ( c < 'Z')))
                    {
                        c++;
                    }
                    else
                    {
                        c = 'a';
                    }
                }
                i++;
                result.Append(c);
            }
            result.Append("X");
            return result.ToString();
        }
        
        /// <summary>
        /// Finds average value.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public static float Average(this FloatRange range)
        {
            if (float.IsInfinity(range.Min) && float.IsInfinity(range.Max))
            {
                return 0;
            }
            else
            {
                if (float.IsNegativeInfinity(range.Min))
                {
                    return range.Max - 1;
                }
                if (float.IsPositiveInfinity(range.Max))
                {
                    return range.Min + 1;
                }
            }
            return (range.Min + range.Max)/2;
        }

        /// <summary>
        /// Counts average value.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public static int Average(this IntRange range)
        {
            return (range.Min + range.Max) / 2;
        }

        /// <summary>
        /// Checks if int value is within the range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Contains(this IntRange range, int value)
        {
            return (value >= range.Min && value <= range.Max);
        }

        /// <summary>
        /// Checks if float value is in range.
        /// </summary>
        /// <param name="range">Range.</param>
        /// <param name="value">Value to be checked.</param>
        /// <returns></returns>
        public static bool Contains(this IntRange range, float value)
        {
            return (value >= range.Min && value <= range.Max);
        }
        
        /// <summary>
        /// Checks if resilution is containd in the list.
        /// </summary>
        /// <param name="available"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool ContainsResolution(this IEnumerable<VideoResolution> available, VideoResolution target)
        {
            if (available == null || target == null)
            {
                return false;
            }
            else
            {
                return available.FirstOrDefault(VR => VR.Height == target.Height && VR.Width == target.Width) != null;
            }
        }

        /// <summary>
        /// Converts String Builder to string. Trims NewLine symbol if exists at the end.
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static string ToStringTrimNewLine(this StringBuilder sb)
        {
            string dump = string.Empty;
            dump = sb.ToString();
            if (dump.EndsWith(Environment.NewLine))
            {                
                dump = sb.ToString(0, sb.Length - 2);
            }
            return dump;
        }
        
        /// <summary>
        /// Converts Duration to seconds
        /// </summary>
        /// <param name="duration">XS-duration string</param>
        /// <returns>Number of seconds</returns>
        public static double DurationToSeconds(this string duration)
        {
            double seconds = 0;

            if (!(duration.StartsWith("PT") || duration.StartsWith("-PT")))
            {
                seconds = double.NaN;
            }
            else
            {
                int i = 0;
                while (i < duration.Length && duration[i] != 'T')
                {
                    i++;
                }
                i++;
                while (i < duration.Length)
                {
                    string currentValue = string.Empty;
                    bool hasPoint = false;
                    while (i < duration.Length - 1 && (char.IsDigit(duration[i]) || duration[i] == '.'))
                    {
                        currentValue += duration[i];
                        if (duration[i] == '.')
                        {
                            hasPoint = true;
                        }
                        i++;
                    }

                    string separator = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    double val = double.Parse(currentValue.Replace(".", separator));

                    if (i < duration.Length)
                    {

                        if (hasPoint && duration[i] != 'S')
                        {
                            continue;
                        }

                        switch (duration[i])
                        {
                            case 'H':
                                {
                                    seconds += 3600 * val;
                                }
                                break;
                            case 'M':
                                {
                                    seconds += 60 * val;
                                }
                                break;
                            case 'S':
                                {
                                    seconds += val;
                                }
                                break;

                        }
                    }
                    i++;
                }

                if (duration[0] == '-')
                {
                    seconds = -seconds;
                }
            }


            return seconds;
        }

        /// <summary>
        /// Checks if DateTime structure represents valid date and time.
        /// </summary>
        /// <param name="serverDateTime">Date and time gone from the DUT</param>
        /// <param name="fieldName">Field name (for generation failure reason)</param>
        /// <param name="reason">Failure reason.</param>
        /// <returns></returns>
        public static bool IsValidDateTime(this Proxies.Onvif.DateTime serverDateTime, string fieldName, out string reason)
        {
            bool bDateTimeValid = true;

            reason = string.Empty;

            if (serverDateTime.Date == null)
            {
                reason = "Date is null";
            }
            else if (serverDateTime.Time == null)
            {
                reason = "Time is null";
            }
            else
            {
                try
                {
                    System.DateTime dt = new System.DateTime(serverDateTime.Date.Year,
                                                             serverDateTime.Date.Month,
                                                             serverDateTime.Date.Day,
                                                             serverDateTime.Time.Hour,
                                                             serverDateTime.Time.Minute,
                                                             serverDateTime.Time.Second);
                }
                catch (Exception)
                {
                    bDateTimeValid = false;
                    reason =
                        string.Format(
                            "{0} is invalid (Year: {1}, Month: {2}, Day: {3}, Hour: {4}, Minute: {5}, Seconds: {6})",
                            fieldName,
                            serverDateTime.Date.Year, serverDateTime.Date.Month, serverDateTime.Date.Day,
                            serverDateTime.Time.Hour, serverDateTime.Time.Minute, serverDateTime.Time.Second);

                }
            }
            return bDateTimeValid;
        }

        /// <summary>
        /// Validates URL. If an instance of Uri cannot be created from the string passed, the second 
        /// attempt is made with "http://" prepended. If both attempts failed, uri passed is treated 
        /// as invalid.
        /// </summary>
        /// <param name="url">String to be verified</param>
        /// <returns>True, if string passed is valid URL.</returns>
        public static bool IsValidUrl(this string url)
        {
            bool bUrlValid = true;
            if (url == null)
            {
                bUrlValid = false;
            }
            else
            {
                try
                {
                    Uri uri = new Uri(url);
                }
                catch (Exception)
                {
                    bUrlValid = false;
                }

                if (!bUrlValid && !(url.StartsWith("http:")))
                {
                    string urlEx = string.Format("http://{0}", url);
                    try
                    {
                        Uri uri = new Uri(urlEx);
                        bUrlValid = true;
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            return bUrlValid;
        }

        /// <summary>
        /// Validates hostname.
        /// </summary>
        /// <param name="hostname">String to be validated.</param>
        /// <returns>True if hostname is valid.</returns>
        public static bool IsValidHostname(this string hostname)
        {
            bool valid = true;
            if (string.IsNullOrEmpty(hostname))
            {
                valid = true;
            }
            else
            {
                if (hostname.Length > 63)
                {
                    valid = false;
                }
                else
                {
                    string hostnameRegex = @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$";
                    System.Text.RegularExpressions.Regex regex = new Regex(hostnameRegex);
                    valid = regex.IsMatch(hostname);

                }

            }

            return valid;
        }

        /// <summary>
        /// Checks if fault is valid.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool IsValidOnvifFault(this FaultException ex)
        {
            if (ex == null)
            {
                return false;
            }
            else
            {
                return true;
                //string nameSpace = "http://www.onvif.org/ver10/error";
                //bool found = false;
                //FaultCode subCode = ex.Code.SubCode;
                //while (subCode != null && !found)
                //{
                //    found = (subCode.Namespace == nameSpace);
                //    subCode = subCode.SubCode;
                //}
                //return found;
            }
        }

    }
}
