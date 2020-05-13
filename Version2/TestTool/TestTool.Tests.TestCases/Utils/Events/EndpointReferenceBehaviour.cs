///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Xml;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using TestTool.Proxies.Event;

namespace TestTool.Tests.TestCases.Utils.Events
{
    /// <summary>
    /// Adds "reference parameters" headers.
    /// </summary>
    class EndpointReferenceBehaviour : IClientMessageInspector, IEndpointBehavior
    {
        public EndpointReferenceBehaviour(EndpointReferenceType endpointReference)
        {
            _endpointReference = endpointReference;
        }
        
        private EndpointReferenceType _endpointReference;

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {

        }

        /// <summary>
        /// Adds headers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public object BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
        {
            if (_endpointReference != null && _endpointReference.ReferenceParameters != null) 
            {
                if (_endpointReference.ReferenceParameters.Any != null)
                {
                    foreach (XmlElement element in _endpointReference.ReferenceParameters.Any)
                    {
                        request.Headers.Add(new SubscriptionReferenceHeader(element));
                    }
                }

            }
            return null;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(this);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }
    }

    /// <summary>
    /// Message header representing WSA reference parameter.
    /// </summary>
    class SubscriptionReferenceHeader : MessageHeader
    {
        private readonly XmlElement _element;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element">Element passed in ReferenceParameters field.</param>
        public SubscriptionReferenceHeader(XmlElement element)
        {
            _element = element;
        }

        /// <summary>
        /// Writes header
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="messageVersion"></param>
        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteAttributeString("IsReferenceParameter", "http://www.w3.org/2005/08/addressing", "true");
            foreach (XmlAttribute attr in _element.Attributes)
            {
                writer.WriteAttributeString(attr.LocalName, attr.NamespaceURI, attr.Value);
            }
            writer.WriteValue(_element.InnerXml);
        }

        /// <summary>
        /// Name of the header.
        /// </summary>
        public override string Name
        {
            get { return _element.LocalName; }
        }

        /// <summary>
        /// Namespace of the header.
        /// </summary>
        public override string Namespace
        {
            get { return _element.NamespaceURI; }
        }
    }
}
