///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.ServiceModel.Channels;

namespace TestTool.HttpTransport
{
    class HttpRequestChannelListener : ChannelListenerBase, 
        IChannelListener<IReplyChannel>
    {
        private Uri _uri;
        
        public HttpRequestChannelListener(TransportBindingElement bindingElement, BindingContext context)
        {
            _uri = context.ListenUriBaseAddress;
        }

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
