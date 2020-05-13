using System.ServiceModel.Description;

namespace CameraClient
{
    public class TraceBehavior : IEndpointBehavior
    {
        public TraceBehavior()
        {
        }

        private IListener _listener;
        public TraceBehavior(IListener listener)
        {
            _listener = listener;
        }

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            var inspector = new TraceInspector();
            if (_listener != null)
            {
                inspector.AttachListener(_listener);
            }
            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            var inspector = new TraceInspector();
            if (_listener != null)
            {
                inspector.AttachListener(_listener);
            }
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}
