///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace TestTool.HttpTransport.Internals.Http
{
    /// <summary>
    ///  Methods for HTTP digest authentication support.
    /// </summary>
    class HttpDigest
    {
        public  const string WWWAUTHENTICATEHEADER = "WWW-Authenticate";
        private const string NONCE = "nonce";
        private const string OPAQUE = "opaque";
        private const string STALE = "stale";
        private const string ALGORITHM = "algorithm";
        private const string REALM = "realm";

        private const string MD5 = "MD5";
        private const string MD5SESS = "MD5-sess";

        private static string _a1;

        /// <summary>
        /// Creates header using challenge packet.
        /// </summary>
        /// <param name="packet">Server's challenge packet</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <param name="address">HTTP-address</param>
        /// <returns></returns>
        public static string CreateDigestAuthenticationHeader(HttpPacket packet,
            string username,
            string password, string address)
        {
            string header = string.Empty;

            if (packet.Headers.ContainsKey(WWWAUTHENTICATEHEADER))
            {
                string challenge = packet.Headers[WWWAUTHENTICATEHEADER];

                int schemeEnd = challenge.IndexOf(' ');

                // scheme - should be "Digest"
                string scheme = challenge.Substring(0, schemeEnd).Trim();

                string authParams = challenge.Substring(schemeEnd).Trim();

                string[] challengeFields = authParams.Split(',');

                Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

                foreach (string field in challengeFields)
                {
                    string[] pair = field.Split('=');
                    if (pair.Length == 2)
                    {
                        parameters.Add(Unquote(pair[0]), pair[1].Trim());
                    }
                    else
                    {
                        throw new ApplicationException(string.Format("Invalid authentication header: {0}", challenge));
                    }
                }

                // parameters are parsed
                // quotes are not trimmed!

                StringBuilder sb = new StringBuilder("Authorization: Digest ");
                sb.AppendFormat("username=\"{0}\", ", username);

                // realm should contain quotes;
                sb.AppendFormat("{0}={1}, ", REALM, parameters[REALM]);

                string alg = MD5;
                if (parameters.ContainsKey(ALGORITHM))
                {
                    alg = parameters[ALGORITHM];
                }

                sb.AppendFormat("qop=\"auth\", algorithm=\"{0}\", ", alg);
                sb.AppendFormat("uri=\"{0}\", ", address);
                sb.AppendFormat("{0}={1}, nc=00000001, ", NONCE, parameters[NONCE]);

                // cnonce
                string cnonce = GetNonce();
                sb.AppendFormat("cnonce=\"{0}\", ", cnonce);

                // opaque
                if (parameters.ContainsKey(OPAQUE))
                {
                    sb.AppendFormat("{0}={1}, ", OPAQUE, parameters[OPAQUE]);
                }

                // response

                string realm = Unquote(parameters[REALM]);
                string nonce = Unquote(parameters[NONCE]);

                /*
                 request-digest  = <"> < KD ( H(A1),     unq(nonce-value)
                                          ":" nc-value
                                          ":" unq(cnonce-value)
                                          ":" unq(qop-value)
                                          ":" H(A2)
                                  ) <">
                  
                 A1       = unq(username-value) ":" unq(realm-value) ":" passwd
                                  
                 A2       = Method ":" digest-uri-value
                 
                 */

                string a1;
                a1 = string.Format("{0}:{1}:{2}", username, realm, password);

                System.Diagnostics.Debug.WriteLine(string.Format("A1: {0}", a1));
                
                if (alg == MD5SESS)
                {
                    if (string.IsNullOrEmpty(_a1))
                    {
                        _a1 = GetMD5HashBinHex(a1);
                    }
                    a1 = string.Format("{0}:{1}:{2}", _a1, nonce, cnonce);
                }
                string a2 = string.Format("POST:{0}", address);
                
                string ha1 = GetMD5HashBinHex(a1);
                string ha2 = GetMD5HashBinHex(a2);

                System.Diagnostics.Debug.WriteLine(string.Format("HA1: {0}", ha1));
                System.Diagnostics.Debug.WriteLine(string.Format("A2: {0}", a2));
                System.Diagnostics.Debug.WriteLine(string.Format("HA2: {0}", ha2));

                string a = string.Format("{0}:{1}:00000001:{2}:auth:{3}", ha1, nonce, cnonce, ha2);

                System.Diagnostics.Debug.WriteLine(string.Format("unhashedDigest: {0}", a));

                string response = GetMD5HashBinHex(a);
                System.Diagnostics.Debug.WriteLine(string.Format("hashedDigest: {0}", response));

                sb.AppendFormat("response=\"{0}\"", response);
                header = sb.ToString();
            }

            return header;
        }

        /// <summary>
        /// Gets hex string from bytes array.
        /// </summary>
        /// <param name="bytes">Bytes array</param>
        /// <returns></returns>
        private static string GetHex(byte[] bytes)
        {
            string hex = BitConverter.ToString(bytes);
            hex = hex.Replace("-", "");
            return hex;
        }

        /// <summary>
        /// Gets hex representation of a MD5-hashed string.
        /// </summary>
        /// <param name="val">String to be hashed.</param>
        /// <returns></returns>
        private static string GetMD5HashBinHex(string val)
        {
            Encoding enc = new ASCIIEncoding();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bHA1 = md5.ComputeHash(enc.GetBytes(val));
            string HA1 = "";
            for (int i = 0; i < bHA1.Length; i++)
                HA1 += String.Format("{0:x02}", bHA1[i]);
            return HA1;
        }

        private static string GetNonce()
        {
            byte[] nonce = new byte[16];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(nonce);
            return GetHex(nonce);
        }

        static string Unquote(string value)
        {
            return value.Trim(new char[] { ' ', '"' });
        }
    }
}
