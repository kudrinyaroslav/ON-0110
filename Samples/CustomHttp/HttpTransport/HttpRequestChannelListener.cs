using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CustomHttpTransport
{
    class HttpRequestChannelListener : ChannelListenerBase, 
        IDefaultCommunicationTimeouts, 
        IChannelListener<IReplyChannel>, 
        IChannelListener, 
        ICommunicationObject
    {
        private Uri _uri;
        
        public HttpRequestChannelListener(TransportBindingElement bindingElement, BindingContext context)
        {
            _uri = context.ListenUriBaseAddress;
        }

        #region IDefaultCommunicationTimeouts Members

        TimeSpan IDefaultCommunicationTimeouts.CloseTimeout
        {
            get { throw new NotImplementedException(); }
        }

        TimeSpan IDefaultCommunicationTimeouts.OpenTimeout
        {
            get { throw new NotImplementedException(); }
        }

        TimeSpan IDefaultCommunicationTimeouts.ReceiveTimeout
        {
            get { throw new NotImplementedException(); }
        }

        TimeSpan IDefaultCommunicationTimeouts.SendTimeout
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IChannelListener<IReplyChannel> Members

        public IReplyChannel AcceptChannel(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public IReplyChannel AcceptChannel()
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginAcceptChannel(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IReplyChannel EndAcceptChannel(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IChannelListener Members

        IAsyncResult IChannelListener.BeginWaitForChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        bool IChannelListener.EndWaitForChannel(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        T IChannelListener.GetProperty<T>()
        {
            throw new NotImplementedException();
        }

        Uri IChannelListener.Uri
        {
            get { return _uri; }
        }

        bool IChannelListener.WaitForChannel(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICommunicationObject Members

        void ICommunicationObject.Abort()
        {
            throw new NotImplementedException();
        }

        IAsyncResult ICommunicationObject.BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        IAsyncResult ICommunicationObject.BeginClose(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        IAsyncResult ICommunicationObject.BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        IAsyncResult ICommunicationObject.BeginOpen(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        void ICommunicationObject.Close(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        void ICommunicationObject.Close()
        {
            throw new NotImplementedException();
        }

        event EventHandler ICommunicationObject.Closed
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler ICommunicationObject.Closing
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        void ICommunicationObject.EndClose(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        void ICommunicationObject.EndOpen(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        event EventHandler ICommunicationObject.Faulted
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        void ICommunicationObject.Open(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        void ICommunicationObject.Open()
        {
            throw new NotImplementedException();
        }

        event EventHandler ICommunicationObject.Opened
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler ICommunicationObject.Opening
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        CommunicationState ICommunicationObject.State
        {
            get { throw new NotImplementedException(); }
        }



        #endregion

        protected override IAsyncResult OnBeginWaitForChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override bool OnEndWaitForChannel(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override bool OnWaitForChannel(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        protected override void OnAbort()
        {
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override void OnClose(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public override Uri Uri 
        { 
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
