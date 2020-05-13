using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;

namespace TestTool.Transport.Security
{
    public class SecurityBehavior : IEndpointBehavior
    {
        private ILogger _logger;
        public SecurityBehavior(ILogger logger)
        {
            _logger = logger;
        }

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new SecurityMessageInspector(_logger));
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}
