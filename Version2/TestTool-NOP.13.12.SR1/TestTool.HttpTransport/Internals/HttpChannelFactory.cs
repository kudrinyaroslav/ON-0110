///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace TestTool.HttpTransport
{
    /// <summary>
    /// IChannelFactory implementation for custom Http.
    /// </summary>
    class HttpChannelFactory : ChannelFactoryBase<IRequestChannel>
    {
        #region Fields

        BufferManager _bufferManager;
        MessageEncoderFactory _messageEncoderFactory;
        private HttpTransportBindingElement _bindingElement;
        
        #endregion

        internal HttpChannelFactory(HttpTransportBindingElement bindingElement, BindingContext context)
            : base(context.Binding)
        {
            this._bufferManager = BufferManager.CreateBufferManager(bindingElement.MaxBufferPoolSize, int.MaxValue);

            Collection<MessageEncodingBindingElement> messageEncoderBindingElements
                = context.BindingParameters.FindAll<MessageEncodingBindingElement>();

            if(messageEncoderBindingElements.Count > 1)
            {
                throw new InvalidOperationException("More than one MessageEncodingBindingElement was found in the BindingParameters of the BindingContext");
            }
            else if (messageEncoderBindingElements.Count == 1)
            {
                this._messageEncoderFactory = messageEncoderBindingElements[0].CreateMessageEncoderFactory();
            }
            else
            {
                this._messageEncoderFactory = CustomHttpConstants.DefaultMessageEncoderFactory;
            }
            _bindingElement = bindingElement;
        }

        public BufferManager BufferManager
        {
            get
            {
                return this._bufferManager;
            }
        }

        public MessageEncoderFactory MessageEncoderFactory
        {
            get
            {
                return this._messageEncoderFactory;
            }
        }
        
        public override T GetProperty<T>()
        {
            T messageEncoderProperty = this.MessageEncoderFactory.Encoder.GetProperty<T>();
            if (messageEncoderProperty != null)
            {
                return messageEncoderProperty;
            }

            if (typeof(T) == typeof(MessageVersion))
            {
                return (T)(object)this.MessageEncoderFactory.Encoder.MessageVersion;
            }

            return base.GetProperty<T>();
        }

        protected override void OnOpen(TimeSpan timeout)
        {
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return new CompletedAsyncResult(callback, state);
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            CompletedAsyncResult.End(result);
        }

        /// <summary>
        /// Create a new Http Channel. Supports IRequestChannel.
        /// </summary>
        /// <param name="remoteAddress">The address of the remote endpoint</param>
        /// <param name="via"></param>
        /// <returns></returns>
        protected override IRequestChannel OnCreateChannel(EndpointAddress remoteAddress, Uri via)
        {
            return new HttpRequestChannel(this, 
                remoteAddress, 
                via, 
                _messageEncoderFactory.Encoder, 
                _bindingElement);
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            this._bufferManager.Clear();
        }
    }
}
