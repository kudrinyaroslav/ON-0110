// ----------------------------------------------------------------------------
// Copyright (C) 2003-2005 Microsoft Corporation, All rights reserved.
// ----------------------------------------------------------------------------

#region using
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Globalization;
using CustomHttpTransport;
#endregion

namespace Microsoft.ServiceModel.Samples
{
    /// <summary>
    /// IChannelFactory implementation for custom Http.
    /// </summary>
    class CustomHttpChannelFactory : ChannelFactoryBase<IRequestChannel>
    {
        #region member_variables
        BufferManager bufferManager;
        MessageEncoderFactory messageEncoderFactory;
        private CustomHttpTransportBindingElement _bindingElement;
        #endregion

        internal CustomHttpChannelFactory(CustomHttpTransportBindingElement bindingElement, BindingContext context)
            : base(context.Binding)
        {
            this.bufferManager = BufferManager.CreateBufferManager(bindingElement.MaxBufferPoolSize, int.MaxValue);

            Collection<MessageEncodingBindingElement> messageEncoderBindingElements
                = context.BindingParameters.FindAll<MessageEncodingBindingElement>();

            if(messageEncoderBindingElements.Count > 1)
            {
                throw new InvalidOperationException("More than one MessageEncodingBindingElement was found in the BindingParameters of the BindingContext");
            }
            else if (messageEncoderBindingElements.Count == 1)
            {
                this.messageEncoderFactory = messageEncoderBindingElements[0].CreateMessageEncoderFactory();
            }
            else
            {
                this.messageEncoderFactory = CustomHttpConstants.DefaultMessageEncoderFactory;
            }
            _bindingElement = bindingElement;
        }

        public BufferManager BufferManager
        {
            get
            {
                return this.bufferManager;
            }
        }

        public MessageEncoderFactory MessageEncoderFactory
        {
            get
            {
                return this.messageEncoderFactory;
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
        /// Create a new Udp Channel. Supports IOutputChannel.
        /// </summary>
        /// <typeparam name="TChannel">The type of Channel to create (e.g. IOutputChannel)</typeparam>
        /// <param name="remoteAddress">The address of the remote endpoint</param>
        /// <returns></returns>
        protected override IRequestChannel OnCreateChannel(EndpointAddress remoteAddress, Uri via)
        {
            return new CustomHttpRequestChannel(this, 
                remoteAddress, 
                via, 
                true, 
                messageEncoderFactory.Encoder, 
                _bindingElement);
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            this.bufferManager.Clear();
        }
    }
}
