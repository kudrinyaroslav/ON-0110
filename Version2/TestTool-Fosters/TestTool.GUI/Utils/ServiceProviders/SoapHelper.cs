///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Security.Cryptography;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Provides methods to add security headers to a SOAP packet.
    /// </summary>
    class SoapHelper
    {
        private const string SOAPENVELOPE = "http://www.w3.org/2003/05/soap-envelope";
        
        private const string PREFIX = "s";
        private const string HEADER = "Header";
        private const string ENVELOPE = "Envelope";

        private const string WSSEPREFIX = "wsse";
        private const string WSSE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        private const string WSUPREFIX = "wsu";
        private const string WSU = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        private const string SECURITY = "Security";
        private const string USERNAMETOKEN = "UsernameToken";
        private const string USERNAME = "Username";
        private const string PASSWORD = "Password";
        private const string NONCE = "Nonce";
        private const string CREATED = "Created";
        private const string TYPE = "Type";
        private const string PASSWORDTYPE =
            "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest";

        /// <summary>
        /// Adds security headers to a request, if request is a valid SOAP packet. 
        /// </summary>
        /// <param name="request">Request. If it is not a valid SOAP packet, <paramref name="request"/> is 
        /// returned as a method result. </param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <param name="utcTimeStamp">Flag indicating whether UTC timestamp should be used.</param>
        /// <returns>SOAP packet represented by <paramref name="request"/>, with Security 
        /// header added, if a <paramref name="request"/> represents valid SOAP packet. If <paramref name="request"/> 
        /// is not a valid SOAP packet, <paramref name="request"/> is returned. </returns>
        public string ApplySecurity(string request, string username, string password, bool utcTimeStamp)
        {
            return ApplySecurity(request, username, password, utcTimeStamp, false);
        }

        /// <summary>
        /// Adds security headers to a request, if request is a valid SOAP packet. 
        /// </summary>
        /// <param name="request">Request. If it is not a valid SOAP packet, <paramref name="request"/> is 
        /// returned as a method result. </param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <param name="utcTimeStamp">Flag indicating whether UTC timestamp should be used.</param>
        /// <param name="forceUpdate">True, if existing header should be overwritten.</param>
        /// <returns>SOAP packet represented by <paramref name="request"/>, with Security 
        /// header added, if a <paramref name="request"/> represents valid SOAP packet. If <paramref name="request"/> 
        /// is not a valid SOAP packet, <paramref name="request"/> is returned. </returns>
        public string ApplySecurity(string request, string username, string password, bool utcTimeStamp, bool forceUpdate)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(request);
            }
            catch (Exception)
            {
                return request;
            }

            XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace(PREFIX, SOAPENVELOPE);
            manager.AddNamespace(WSSEPREFIX, WSSE);

            XmlNode securityElement = doc.SelectSingleNode(string.Format("/{0}:{1}/{0}:{2}/{3}:{4}", PREFIX, ENVELOPE, HEADER, WSSEPREFIX, SECURITY), manager);
            if (securityElement != null && !forceUpdate)
            {
                return request;
            }
            
            XmlNode headerNode = doc.SelectSingleNode(string.Format("/{0}:{1}/{0}:{2}", PREFIX, ENVELOPE, HEADER), manager);

            if (headerNode!=null && forceUpdate)
            {
                headerNode.ParentNode.RemoveChild(headerNode);
                headerNode = null;
            }

            if (headerNode == null)
            {
                XmlNode envelopeNode = doc.SelectSingleNode(string.Format("/{0}:{1}", PREFIX, ENVELOPE), manager);
                if (envelopeNode == null)
                {
                    return request;
                }

                headerNode = doc.CreateElement(PREFIX, HEADER, SOAPENVELOPE);
                if (envelopeNode.FirstChild != null)
                {
                    envelopeNode.InsertBefore(headerNode, envelopeNode.FirstChild);
                }
                else
                {
                    envelopeNode.AppendChild(headerNode);
                }
            }

            // header node found or added

            XmlElement securityNode = doc.CreateElement(WSSEPREFIX, SECURITY, WSSE);

            XmlAttribute wsseAttribute = doc.CreateAttribute(string.Format("xmlns:{0}", WSSEPREFIX));
            wsseAttribute.Value = WSSE;
            securityNode.Attributes.Append(wsseAttribute);

            XmlAttribute wsuAttribute = doc.CreateAttribute(string.Format("xmlns:{0}", WSUPREFIX));
            wsuAttribute.Value = WSU;
            securityNode.Attributes.Append(wsuAttribute);

            XmlElement usernameTokenElement = doc.CreateElement(WSSEPREFIX, USERNAMETOKEN, WSSE);
            securityNode.AppendChild(usernameTokenElement);

            XmlElement usernameElement = doc.CreateElement(WSSEPREFIX, USERNAME, WSSE);
            usernameElement.InnerText = username;
            usernameTokenElement.AppendChild(usernameElement);

            XmlElement passwordElement = doc.CreateElement(WSSEPREFIX, PASSWORD, WSSE);
            // password element value - ?
            XmlAttribute passwordTypeAttribute = doc.CreateAttribute(TYPE);
            passwordTypeAttribute.Value = PASSWORDTYPE;
            passwordElement.Attributes.Append(passwordTypeAttribute);
            string nonce = GetNonce();
            string timestamp = GetTimestamp(utcTimeStamp); 
            string passwordHash = GetPasswordHash(password, nonce, timestamp);
            passwordElement.InnerText = passwordHash;
            usernameTokenElement.AppendChild(passwordElement);

            XmlElement nonceElement = doc.CreateElement(WSSEPREFIX, NONCE, WSSE);
            nonceElement.InnerText = nonce;
            usernameTokenElement.AppendChild(nonceElement);

            XmlElement createdElement = doc.CreateElement(WSUPREFIX, CREATED, WSU);
            createdElement.InnerText = timestamp;
            usernameTokenElement.AppendChild(createdElement);

            headerNode.AppendChild(securityNode);
            
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);

            int offset = 0;
            byte[] bytes = stream.GetBuffer();
            while (bytes[offset] != (byte)'<')
            {
                offset++;
            }

            stream.Close();

            return Encoding.UTF8.GetString(bytes, offset, bytes.Length - offset);
        }
        
        public string RemoveSecurity(string request)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(request);
            }
            catch (Exception)
            {
                return request;
            }

            XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace(PREFIX, SOAPENVELOPE);
            manager.AddNamespace(WSSEPREFIX, WSSE);

            XmlNode securityElement = doc.SelectSingleNode(string.Format("/{0}:{1}/{0}:{2}/{3}:{4}", PREFIX, ENVELOPE, HEADER, WSSEPREFIX, SECURITY), manager);
            XmlNode headerNode = doc.SelectSingleNode(string.Format("/{0}:{1}/{0}:{2}", PREFIX, ENVELOPE, HEADER), manager);
            if (securityElement != null && headerNode != null)
            {
                headerNode.RemoveChild(securityElement);
            }
            
            if (headerNode != null && headerNode.ChildNodes.Count == 0)
            {
                headerNode.ParentNode.RemoveChild(headerNode);
            }

            MemoryStream stream = new MemoryStream();
            doc.Save(stream);

            int offset = 0;
            byte[] bytes = stream.GetBuffer();
            while (bytes[offset] != (byte)'<')
            {
                offset++;
            }

            stream.Close();

            return Encoding.UTF8.GetString(bytes, offset, bytes.Length - offset);
        }

        protected string GetNonce()
        {
            byte[] nonce = new byte[16]; // default is 16 see reference b.
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(nonce);
            return System.Convert.ToBase64String(nonce); //base64 is default encoding for nonce
        }
        
        protected string GetTimestamp(bool useUtcTimestamp)
        {
            DateTime time = useUtcTimestamp ? System.DateTime.Now.ToUniversalTime() : System.DateTime.Now;
            return time.ToString("yyyy-MM-ddTHH:mm:ssZ"); //W3DTF format;
        }
        
        protected string GetPasswordHash(string password, string nonce, string timestamp)
        {
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            List<byte> data = new List<byte>();
            data.AddRange(System.Convert.FromBase64String(nonce));
            data.AddRange(System.Text.Encoding.UTF8.GetBytes(timestamp + password));
            byte[] hash = algorithm.ComputeHash(data.ToArray());
            return System.Convert.ToBase64String(hash);
        }
    }
}
