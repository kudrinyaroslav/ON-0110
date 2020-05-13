using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using TestTool.HttpTransport.Interfaces;
using TestTool.HttpTransport.Internals;

namespace TestTool.HttpTransport
{
    class HttpsTransportBindingElement: HttpTransportBindingElement
    {
        public HttpsTransportBindingElement(IEnumerable<IChannelController> controllers) : base(controllers)
        {
            Console.WriteLine("!");
        }

        protected HttpsTransportBindingElement(HttpsTransportBindingElement other): this(other.Controllers)
        {}

        public override string Scheme
        {
            get { return CustomHttpConstants.HTTPSScheme; }
        }

        public override BindingElement Clone()
        {
            return new HttpsTransportBindingElement(this);
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return (IChannelFactory<TChannel>)new HttpsChannelFactory(this, context);
        }
    }
}
