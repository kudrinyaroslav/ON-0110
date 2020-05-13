using System.Xml;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace FakeClient
{
    public class SecurityBehavior : MessageHeader, IClientMessageInspector, IEndpointBehavior
    {
        private string m_sUserName;
        private string m_sPassword;
        public override string Name
        {
            get { return "wsse:Security"; }
        }

        public override string Namespace
        {
            get { return ""; }
        }

        public string UserName
        {
            get { return m_sUserName; }
            set { m_sUserName = value; }
        }

        public string Password
        {
            get { return m_sPassword; }
            set { m_sPassword = value; }
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {

        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            request.Headers.Add(this);
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

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteAttributeString("xmlns", "wsse", null, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            writer.WriteStartElement("wsse:UsernameToken");
            writer.WriteElementString("wsse:Username", m_sUserName);
            writer.WriteStartElement("wsse:Password");
            writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
            writer.WriteValue(m_sPassword);
            writer.WriteEndElement(); //wsse:Password             
            writer.WriteEndElement(); //wsse:UsernameToken         
        }
    }
}
