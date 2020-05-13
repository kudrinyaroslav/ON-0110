using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace CameraClient.TestTrace
{
    public delegate void RequestReady(Message request);

    public delegate void ResponseReceived(Message response);

    class StepInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        public void AddBindingParameters(ServiceEndpoint endpoint,
                            BindingParameterCollection bindingParameters)
        { return; }

        public event RequestReady RequestReady;

        public event ResponseReceived ResponseReceived;

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (ResponseReceived != null)
            {
                ResponseReceived(reply);
            }
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (RequestReady != null)
            {
                RequestReady(request);
            }           
            return null;
        }
        

        #region IDispatchMessageInspector Members

        object IDispatchMessageInspector.AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            return null;
        }

        void IDispatchMessageInspector.BeforeSendReply(ref Message reply, object correlationState)
        {
            return;
        }

        #endregion
    }
}
