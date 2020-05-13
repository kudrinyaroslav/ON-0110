///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using TestTool.HttpTransport.Interfaces;

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

        internal class DigestAuthenticationParameters
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Address { get; set; }
            public HttpPacket Challenge { get; set; }
        }

        /// <summary>
        /// Creates header using challenge packet.
        /// </summary>
        /// <returns></returns>
        public static string CreateDigestAuthenticationHeader(
            DigestAuthenticationParameters authenticationParameters, 
            DigestTestingSettings testingSettings,
            ref string nonceBack,
            ref int nonceCounter)
        {
            string header = string.Empty;

            string username = authenticationParameters.UserName;
            string password = authenticationParameters.Password;
            string address = authenticationParameters.Address;
            HttpPacket packet = authenticationParameters.Challenge;

            if (packet.Headers.ContainsKey(WWWAUTHENTICATEHEADER))
            {
                string challenge = packet.Headers[WWWAUTHENTICATEHEADER];

                int schemeEnd = challenge.IndexOf(' ');

                // scheme - should be "Digest"
                string scheme = challenge.Substring(0, schemeEnd).Trim();

                string authParams = challenge.Substring(schemeEnd).Trim();

                List<int> commas = new List<int>();
                bool inQuote = false;
                for (int i = 0; i < authParams.Length; i++)
                {
                    char nextChar = authParams[i];
                    if (nextChar == '"')
                    {
                        inQuote = !inQuote;
                    }
                    else
                    {
                        if (nextChar == ',' && !inQuote)
                        {
                            commas.Add(i);
                        }
                    }
                }
                commas.Add(authParams.Length);

                Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

                int lastStart = 0;
                foreach (int comma in commas)
                {
                    string field = authParams.Substring(lastStart, (comma - lastStart));
                    string[] pair = field.Split('=');
                    var equalPos = field.IndexOf('=');
                    var paramName = field.Substring(0, equalPos);
                    var paramValue = field.Substring(equalPos + 1);
                    if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(paramValue))
                    {
                        parameters.Add(Unquote(paramName), paramValue.Trim());
                    }
                    else
                    {
                        throw new ApplicationException(string.Format("Invalid authentication header: {0}", challenge));
                    }

                    lastStart = comma + 1;
                }


                // parameters are parsed
                // quotes are not trimmed!

                if (parameters[NONCE] != nonceBack)
                {
                    nonceBack = parameters[NONCE];
                    nonceCounter = 1;
                }
                else
                {
                    nonceCounter++;
                }
                string nonceCounterString = string.Format("{0:x08}", nonceCounter);

                StringBuilder sb = new StringBuilder("Authorization: Digest ");
                if (testingSettings == null || !testingSettings.UserNameMissing)
                {
                    sb.AppendFormat("username=\"{0}\", ", username);
                }

                if (testingSettings == null || !testingSettings.RealmMissing)
                {
                    // realm should contain quotes;
                    sb.AppendFormat("{0}={1}, ", REALM, parameters[REALM]);
                }

                string alg = MD5;
                if (parameters.ContainsKey(ALGORITHM))
                {
                    alg = parameters[ALGORITHM];
                }

                sb.AppendFormat("qop=\"auth\", algorithm=\"{0}\", ", alg);
                if (testingSettings == null || !testingSettings.UriMissing)
                {
                    sb.AppendFormat("uri=\"{0}\", ", address);
                }
                if (testingSettings == null || !testingSettings.NonceMissing)
                {
                    sb.AppendFormat("{0}={1}, nc={2}, ", NONCE, parameters[NONCE], nonceCounterString);
                }

                // cnonce
                string cnonce = GetNonce();
                sb.AppendFormat("cnonce=\"{0}\", ", cnonce);

                // opaque
                if (parameters.ContainsKey(OPAQUE))
                {
                    sb.AppendFormat("{0}={1}, ", OPAQUE, parameters[OPAQUE]);
                }

                // response
                if (testingSettings == null || !testingSettings.ResponseMissing)
                {
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

                    string a = string.Format("{0}:{1}:{4}:{2}:auth:{3}", ha1, nonce, cnonce, ha2, nonceCounterString);

                    System.Diagnostics.Debug.WriteLine(string.Format("unhashedDigest: {0}", a));

                    string response = GetMD5HashBinHex(a);
                    System.Diagnostics.Debug.WriteLine(string.Format("hashedDigest: {0}", response));

                    sb.AppendFormat("response=\"{0}\"", response);
                }
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

    //Known Issue
    //Workaround solution: https://connect.microsoft.com/VisualStudio/feedback/details/571052/digest-authentication-does-not-send-the-full-uri-path-in-the-uri-parameter
    public class DigestAuthFixer
    {
        private string _host;
        private string _user;
        private string _password;
        private string _realm;
        private string _nonce;
        private string _qop;
        private string _cnonce;
        private string _opaque;
        private DateTime _cnonceDate;
        private int _nc;

        public DigestAuthFixer(string user, string password)
        {
            // TODO: Complete member initialization
            _user = user;
            _password = password;
        }

        private static string CalculateMd5Hash(string input)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = MD5.Create().ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        private static string GrabHeaderVar(string varName, string header)
        {
            var regHeader = new Regex(string.Format(@"{0}=""([^""]*)""", varName));
            var matchHeader = regHeader.Match(header);
            if (matchHeader.Success)
                return matchHeader.Groups[1].Value;
            throw new ApplicationException(string.Format("Header {0} not found", varName));
        }

        // http://en.wikipedia.org/wiki/Digest_access_authentication
        private string GetDigestHeader(string dir, string method = WebRequestMethods.Http.Get)
        {
            _nc = _nc + 1;

            var ha1 = CalculateMd5Hash(string.Format("{0}:{1}:{2}", _user, _realm, _password));
            var ha2 = CalculateMd5Hash(string.Format("{0}:{1}", method, dir));
            var digestResponse = CalculateMd5Hash(string.Format("{0}:{1}:{2:00000000}:{3}:{4}:{5}", ha1, _nonce, _nc, _cnonce, _qop, ha2));

            var r = string.Format("Digest username=\"{0}\", realm=\"{1}\", nonce=\"{2}\", uri=\"{3}\", algorithm=\"MD5\", response=\"{4}\", qop=\"{5}\", nc={6:00000000}, cnonce=\"{7}\"",
                                  _user, _realm, _nonce, dir, digestResponse, _qop, _nc, _cnonce);

            if (null != _opaque)
                r = r + string.Format(", opaque=\"{0}\"", _opaque);

            return r;
        }

        public HttpWebResponse GrabResponse(string requestStr, string contentType = null, byte[] data = null, int timeout = 100000)
        {
            var uri = new Uri(requestStr);

            var request = (HttpWebRequest)WebRequest.Create(uri);

            var method = string.IsNullOrEmpty(contentType) ? WebRequestMethods.Http.Get : WebRequestMethods.Http.Post;
            // If we've got a recent Auth header, re-use it!
            if (!string.IsNullOrEmpty(_cnonce) && DateTime.Now.Subtract(_cnonceDate).TotalHours < 1.0)
            {
                request.Headers.Add("Authorization", GetDigestHeader(uri.PathAndQuery, method));
            }

            Action<HttpWebRequest> processPOSTRequest = (httpWebRequest) =>
                                                        {
                                                            request.Timeout = timeout;

                                                            if (!string.IsNullOrEmpty(contentType))
                                                            {
                                                                httpWebRequest.Method = "POST";
                                                                httpWebRequest.ContentType = contentType;

                                                                httpWebRequest.ContentLength = (null != data ? data.Count() : 0);
                                                                if (null != data && data.Any())
                                                                {
                                                                    var requestStream = httpWebRequest.GetRequestStream();
                                                                    requestStream.Write(data, 0, data.Count());
                                                                    requestStream.Close();
                                                                }
                                                            }
                                                       };

            processPOSTRequest(request);

            HttpWebResponse response;
            try
            {
                
                response = (HttpWebResponse)request.GetResponse(); 
            }
            catch (WebException ex)
            {
                // Try to fix a 401 exception by adding a Authorization header
                if (ex.Response == null || ((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.Unauthorized)
                    throw;

                var wwwAuthenticateHeader = ex.Response.Headers["WWW-Authenticate"];
                _realm = GrabHeaderVar("realm", wwwAuthenticateHeader);
                _nonce = GrabHeaderVar("nonce", wwwAuthenticateHeader);
                _qop = GrabHeaderVar("qop", wwwAuthenticateHeader);

                try
                { _opaque = GrabHeaderVar("opaque", wwwAuthenticateHeader); }
                catch (Exception)
                { _opaque = null; }

                _nc = 0;
                _cnonce = new Random().Next(123400, 9999999).ToString();
                _cnonceDate = DateTime.Now;

                var request2 = (HttpWebRequest)WebRequest.Create(uri);
                request2.Headers.Add("Authorization", GetDigestHeader(uri.PathAndQuery, method));
                processPOSTRequest(request2);
                response = (HttpWebResponse)request2.GetResponse();
            }

            return response;
        }
    }
}
