using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace CameraClient
{
    public class TraceInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, 
            BindingParameterCollection bindingParameters) 
        { return; }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            foreach (IListener listener in _listeners)
            {
                listener.WriteLine("IClientMessageInspector.AfterReceiveReply called.", InformationType.Service);
                listener.WriteLine("Message:", InformationType.Service);
                listener.WriteLine(reply.ToString(), InformationType.Response);
            }
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            foreach (IListener listener in _listeners)
            {
                listener.WriteLine("IClientMessageInspector.BeforeSendRequest called.", InformationType.Service);
                listener.WriteLine("Message: ", InformationType.Service);
                listener.WriteLine(request.ToString(), InformationType.Request);
            }
            return null;
        }

        private List<IListener> _listeners = new List<IListener>();
        public void AttachListener(IListener listener)
        {
            _listeners.Add(listener);
        }

        public void DetachListener(IListener listener)
        {
            _listeners.Remove(listener);
        }


        #region IDispatchMessageInspector Members

        object IDispatchMessageInspector.AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            return null;
        }

        void IDispatchMessageInspector.BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            return;
        }

        #endregion
    }

    
}
