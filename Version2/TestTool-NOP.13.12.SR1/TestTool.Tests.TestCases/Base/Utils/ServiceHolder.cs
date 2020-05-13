using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.Base
{
    delegate string ServiceAddressRetrievalMethod(FeaturesList feature);

    internal delegate void SetupSecurity(ServiceEndpoint endpoint);

    internal delegate void SetupChannel(IClientChannel channel);

    internal delegate T CreateClient<T>(Binding binding, EndpointAddress address);

    internal class ServiceHolder
    {
        public string Address { get; protected set; }

        public string ServiceName { get; protected set; }
        
        protected ServiceAddressRetrievalMethod RetrievalMethod { get; set; }

        public void Retrieve(FeaturesList features)
        {
            Address = RetrievalMethod(features);
        }

        public bool HasAddress
        {
            get { return !string.IsNullOrEmpty(Address); }
        }

        public virtual void CreateClient(Binding binding, SetupSecurity securitySetup, SetupChannel channelSetup)
        {
        }

        public virtual void Close()
        {

        }

    }

    internal class ServiceHolder<T, PortType> : ServiceHolder
        where PortType : class 
        where T : ClientBase<PortType>, PortType
    {
        public ServiceHolder(ServiceAddressRetrievalMethod retrievalMethod, 
            CreateClient<T> initializationMethod,
            string serviceName)
        {
            RetrievalMethod = retrievalMethod;
            ServiceName = serviceName;
            InitializationMethod = initializationMethod;  
        }

        public T Client { get; private set; }
        
        public override void Close()
        {
            if (Client != null && Client.State == CommunicationState.Opened)
            {
                Client.Close();
            }
        }

        
        private CreateClient<T> InitializationMethod { get; set; }
        
        public override void CreateClient(Binding binding, SetupSecurity securitySetup, SetupChannel channelSetup)
        {
            Client = InitializationMethod(binding, new EndpointAddress(Address));
            securitySetup(Client.Endpoint);
            channelSetup(Client.InnerChannel);
        }
    }
}
