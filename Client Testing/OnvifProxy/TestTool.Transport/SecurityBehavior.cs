///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Xml;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Security.Cryptography;

namespace TestTool.Transport
{
    public enum ProfileTokenPasswordType
    {
        PasswordText = 0,
        PasswordDigest
    }

    /// <summary>
    /// Adds security headers to SOAP message.
    /// </summary>
    public class SecurityBehavior : MessageHeader, IClientMessageInspector, IEndpointBehavior
    {
        private ProfileTokenPasswordType _passwordType = ProfileTokenPasswordType.PasswordDigest;
        
        public ProfileTokenPasswordType PasswordType
        {
            get { return _passwordType; }
            set { _passwordType = value; }
        }

        public override string Name
        {
            get { return "wsse:Security"; }
        }

        public override string Namespace
        {
            get { return ""; }
        }

        protected bool UseUTCTimestamp
        {
            get { return true; }
        }

        public string UserName { get; set; }

        public string Password { get; set; }
        
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {

        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                request.Headers.Add(this);
            }
            return null;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add((IClientMessageInspector)this);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }
        protected void WriteClearTextPassword(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("wsse:Password");
            writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
            writer.WriteValue(Password);
            writer.WriteEndElement(); //wsse:Password             
        }
        protected string GetNonce()
        {
            byte[] nonce = new byte[16]; // default is 16 see reference b.
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(nonce);
            return System.Convert.ToBase64String(nonce); //base64 is default encoding for nonce
        }
        protected string GetTimestamp()
        {
            DateTime time = UseUTCTimestamp ? System.DateTime.Now.ToUniversalTime() : System.DateTime.Now;
            return time.ToString("yyyy-MM-ddTHH:mm:ssZ"); //W3DTF format;
        }
        protected string GetPasswordHash(string nonce, string timestamp)
        {
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            List<byte> data = new List<byte>();
            data.AddRange(System.Convert.FromBase64String(nonce));
            data.AddRange(System.Text.Encoding.UTF8.GetBytes(timestamp + Password));
            byte[] hash = algorithm.ComputeHash(data.ToArray());
            return System.Convert.ToBase64String(hash);
        }
        protected void WritePasswordDigest(XmlDictionaryWriter writer)
        {
            string nonce = GetNonce();
            string timestamp = GetTimestamp();
            string passHash = GetPasswordHash(nonce, timestamp);
            writer.WriteStartElement("wsse:Password");
            writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest");
            writer.WriteValue(passHash);
            writer.WriteEndElement(); //wsse:Password             
            writer.WriteElementString("wsse:Nonce", nonce);
            writer.WriteElementString("wsu:Created", timestamp);
        }
        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteAttributeString("xmlns", "wsse", null, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            writer.WriteAttributeString("xmlns", "wsu", null, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            if (!string.IsNullOrEmpty(UserName))
            {
                writer.WriteStartElement("wsse:UsernameToken");
                writer.WriteElementString("wsse:Username", UserName);
                if (_passwordType == ProfileTokenPasswordType.PasswordText)
                {
                    WriteClearTextPassword(writer);
                }
                else
                {
                    WritePasswordDigest(writer);
                }
                writer.WriteEndElement(); //wsse:UsernameToken         
            }
        }
    }

}
