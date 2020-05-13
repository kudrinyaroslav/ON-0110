///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel.Channels;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.HttpTransport
{
    /// <summary>
    /// Http Binding Element.  
    /// Used to configure and construct Http ChannelFactories and ChannelListeners.
    /// </summary>
    public class HttpTransportBindingElement: TransportBindingElement // to signal that we're a transport
    {
        private List<IChannelController> _controllers;
        public HttpTransportBindingElement(IEnumerable<IChannelController> controllers)
        {
            _controllers = new List<IChannelController>();
            if (controllers != null)
            {
                _controllers.AddRange(controllers);
            }
        }

        protected HttpTransportBindingElement(HttpTransportBindingElement other): base(other)
        {
            _controllers = other._controllers;
        }

        public List<IChannelController> Controllers
        {
            get { return _controllers; }
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return (IChannelFactory<TChannel>)(object)new HttpChannelFactory(this, context);
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (!this.CanBuildChannelListener<TChannel>(context))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Unsupported channel type: {0}.", typeof(TChannel).Name));
            }

            return (IChannelListener<TChannel>)(object)new HttpTransport.HttpRequestChannelListener(this, context);
        }

        /// <summary>
        /// Used by higher layers to determine what types of channel factories this
        /// binding element supports. Which in this case is just IRequestChannel.
        /// </summary>
        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            return (typeof(TChannel) == typeof(IRequestChannel));
        }

        /// <summary>
        /// Used by higher layers to determine what types of channel listeners this
        /// binding element supports. Which in this case is just IReplyChannel.
        /// </summary>
        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            return (typeof(TChannel) == typeof(IReplyChannel));
        }

        public override string Scheme
        {
            get { return CustomHttpConstants.HTTPScheme; }
        }

        public override BindingElement Clone()
        {
            return new HttpTransportBindingElement(this);
        }

        public override T GetProperty<T>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return context.GetInnerProperty<T>();
        }
    }
}
