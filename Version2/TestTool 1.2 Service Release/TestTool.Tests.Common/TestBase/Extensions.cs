///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.ServiceModel;

namespace TestTool.Tests.Common.TestBase
{
    /// <summary>
    /// Extension method for validating data received from the DUT.
    /// </summary>
    public static class Extensions
    {
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

                if (!bUrlValid)
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
            if (string.IsNullOrEmpty(hostname ))
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

        /// <summary>
        /// Check if fault subcodes sequence is the same as passed in faultCodes
        /// </summary>
        /// <param name="ex">Exception to be verified.</param>
        /// <param name="faultCodes">'/'-separated sequence of codes.</param>
        /// <returns>True, if fault exception is correct.</returns>
        public static bool IsValidOnvifFault(this FaultException ex, string faultCodes)
        {
            string result;
            return IsValidOnvifFault(ex, faultCodes, out result);
        }

        /// <summary>
        /// Check if fault subcodes sequence is the same as passed in faultCodes
        /// </summary>
        /// <param name="ex">Exception to be verified.</param>
        /// <param name="faultCodes">'/'-separated sequence of codes.</param>
        /// <param name="verificationDump">String to save verification dump (expected and actual codes).</param>
        /// <returns>True, if fault exception is correct.</returns>
        public static bool IsValidOnvifFault(this FaultException ex, string faultCodes, out string verificationDump)
        {
            if (ex == null)
            {
                verificationDump = "fault exception is null";
                return false;
            }
            else
            {
                string nameSpace = "http://www.onvif.org/ver10/error";
                string envNameSpace = "http://www.w3.org/2003/05/soap-envelope";
                
                string[] codes = faultCodes.Split('/');
                
                if (ex.Code == null)
                {
                    verificationDump = "no fault code";
                    return false;
                }
                
                if (ex.Code.Namespace.ToLower() != envNameSpace)
                {
                    verificationDump = string.Format("fault code has incorrect namespace ({0})", ex.Code.Namespace);
                    return false;
                }

                StringBuilder actual = new StringBuilder(string.Format("env:{0}", ex.Code.Name));
                FaultCode nextCode = ex.Code.SubCode;
                while (nextCode != null)
                {
                    if (nextCode.Namespace.ToLower() != nameSpace)
                    {
                        verificationDump = string.Format("subcode {0} has incorrect namespace ({1})", nextCode.Name,
                                                         nextCode.Namespace);
                        return false;
                    }
                    actual.AppendFormat("/ter:{0}", nextCode.Name);
                    nextCode = nextCode.SubCode;
                }
                
                if (codes.Length != 0)
                {
                    StringBuilder expected = new StringBuilder(string.Format("env:{0}", codes[0]));
                    for (int i = 1; i < codes.Length; i++ )
                    {
                        expected.AppendFormat("/ter:{0}", codes[i]);
                    }

                    string actualSequence = actual.ToString();
                    string expectedSequence = expected.ToString();

                    if (actualSequence != expectedSequence)
                    {
                        verificationDump = string.Format("fault subcodes sequence is incorrect. Expected: {0}, actual: {1}", expectedSequence, actualSequence);
                        return false;
                    }
                }

                verificationDump = string.Empty;
                return true;
            }
        }

    }
}
