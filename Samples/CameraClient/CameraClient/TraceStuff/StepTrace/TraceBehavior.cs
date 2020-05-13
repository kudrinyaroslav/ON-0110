using System.ServiceModel.Description;


namespace CameraClient.TestTrace
{
    class TraceBehavior : IEndpointBehavior
    {
        public TraceBehavior()
        {
        }

        public event RequestReady RequestReady;

        public event ResponseReceived ResponseReceived;
        
        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            var inspector = new StepInspector();
            inspector.ResponseReceived += inspector_ResponseReceived;
            inspector.RequestReady += inspector_RequestReady;
            clientRuntime.MessageInspectors.Add(inspector);
        }

        void inspector_RequestReady(System.ServiceModel.Channels.Message request)
        {
            if (RequestReady != null)
            {
                RequestReady(request);
            }              
        }

        void inspector_ResponseReceived(System.ServiceModel.Channels.Message response)
        {
            if (ResponseReceived != null)
            {
                ResponseReceived(response);
            }            
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }
        
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion   
    }
}
