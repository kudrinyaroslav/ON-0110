using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;

namespace CustomHttpTransport
{
    class HttpChannelAsyncRequest : IAsyncRequest
    {
        public HttpChannelAsyncRequest(IRequestChannel channel, AsyncCallback callback, object state)
        {

        }

        #region IAsyncRequest Members

        public void BeginSendRequest(Message message, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public Message End()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAsyncResult Members

        public object AsyncState
        {
            get { throw new NotImplementedException(); }
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public bool CompletedSynchronously
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsCompleted
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }

}
