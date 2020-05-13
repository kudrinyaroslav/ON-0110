/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Onvif
{
    public class DigestAuthentication
    {

        private static string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }


        public static string CreateDigestAuthentication(string authRequestParamString, string userName, string password, string url, string rtspCommand)
        {
            string realm = null;
            string nonce = null;

            //RTSP/1.0 401 Unauthorized\r\nCSeq: 1\r\nDate: Mon Dec  1 02:29:26 2008 GMT\r\nWWW-Authenticate: Digest realm=\"NET-i\", nonce=\"00000000000000000000000049824C0F\"\r\n\r\n
            authRequestParamString = authRequestParamString.Replace("Digest ", "");
            authRequestParamString = authRequestParamString.Replace("DIGEST ", "");
//            authRequestParamString = authRequestParamString.Replace(" ", "");
            string[] authRequestParams = authRequestParamString.Split(',');
            foreach (string authRequestParam in authRequestParams)
            {
                string trimAuthRequestParam = authRequestParam.Trim();
                string[] valuePair = trimAuthRequestParam.Split('=');
                switch (valuePair[0].ToUpper())
                {
                    case "REALM":
                        realm = valuePair[1];
                        realm = realm.Replace("\"", "");
                        break;
                    case "NONCE":
                        nonce = valuePair[1];
                        nonce = nonce.Replace("\"", "");
                        break;
                }
            }

            if (realm == null || nonce == null)
            {
                throw new Exception(string.Format("Unable to parse authentication params: {0}", authRequestParamString));
            }

            //Authorization: Digest username="root", realm="NET-i", nonce="0000000000000000000000007CB7AA10", uri="rtsp://192.168.8.74", response="0f37c9f032cd84a154cb36c338ecde98"

            string A1 = string.Format("{0}:{1}:{2}", userName, realm, password);
            string HA1 = GetMD5Hash(A1);
            string A2 = string.Format("{0}:{1}", rtspCommand, url);
            string HA2 = GetMD5Hash(A2);

            string responseInput = string.Format("{0}:{1}:{2}", HA1, nonce, HA2);
            string md5Response = GetMD5Hash(responseInput);//"0f37c9f032cd84a154cb36c338ecde98";

            return string.Format("Authorization: Digest username=\"{0}\", realm=\"{1}\", nonce=\"{2}\", uri=\"{3}\", response=\"{4}\"\r\n",
                userName,
                realm,
                nonce,
                url,
                md5Response);
        }

    }
}
