using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.HttpTransport
{
    public class HttpsBinding: HttpBinding
    {
        /// <summary>
        /// Creates binding with controllers added.
        /// </summary>
        /// <param name="elements"></param>
        public HttpsBinding(IEnumerable<IChannelController> elements): base(elements)
        {}
        
        public override string Scheme { get { return "https"; } }

        /// <summary>
        ///  "Our" transport layer
        /// </summary>
        TransportBindingElement _transport;
        public override TransportBindingElement Transport
        {
            get
            {
                if (null == _transport)
                    _transport = new HttpsTransportBindingElement(Controllers.Where(c => c is ITransportController));

                return _transport;
            }
        }
    }
}
