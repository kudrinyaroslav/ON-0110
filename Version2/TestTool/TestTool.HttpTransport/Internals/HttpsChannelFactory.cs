using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Channels;

namespace TestTool.HttpTransport.Internals
{
    class HttpsChannelFactory: HttpChannelFactory
    {
        internal HttpsChannelFactory(HttpsTransportBindingElement bindingElement, BindingContext context): base(bindingElement, context)
        {}

        /// <summary>
        /// Create a new Http Channel. Supports IRequestChannel.
        /// </summary>
        /// <param name="remoteAddress">The address of the remote endpoint</param>
        /// <param name="via"></param>
        /// <returns></returns>
        protected override IRequestChannel OnCreateChannel(EndpointAddress remoteAddress, Uri via)
        {
            return new HttpsRequestChannel(this, 
                                           remoteAddress, 
                                           via, 
                                           _messageEncoderFactory.Encoder, 
                                           _bindingElement as HttpsTransportBindingElement);
        }    }
}
