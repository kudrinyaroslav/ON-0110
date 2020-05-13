using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using TestTool.HttpTransport.Internals;

namespace TestTool.HttpTransport
{
    class HttpsRequestChannel: HttpRequestChannel
    {
        internal HttpsRequestChannel(HttpChannelFactory factory, 
                                     EndpointAddress to, 
                                     Uri via,
                                     MessageEncoder encoder, 
                                     HttpsTransportBindingElement bindingElement): base(factory, to, via, encoder, bindingElement)
        {}
    
        private SecuredRequestNetworkStream _networkStream;
        protected override RequestNetworkStream NetworkStream 
        {
            get
            {
                if (null == _networkStream)
                    _networkStream = new SecuredRequestNetworkStream(_to, CredentialsProvider);

                return _networkStream;
            }
        }
    }
}
