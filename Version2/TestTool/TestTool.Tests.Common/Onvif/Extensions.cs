///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.ServiceModel;
using TestTool.Tests.Definitions.Onvif;

namespace TestTool.Tests.Common.TestBase
{
    /// <summary>
    /// Extension method for validating data received from the DUT.
    /// </summary>
    public static class Extensions
    {

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
                if (ex.Code.Name.Contains("/"))
                {
                    verificationDump = string.Format("code {0} is incorrect", ex.Code.Name);
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
                    if (nextCode.Name.Contains("/"))
                    {
                        verificationDump = string.Format("subcode {0} is incorrect", nextCode.Name);
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
