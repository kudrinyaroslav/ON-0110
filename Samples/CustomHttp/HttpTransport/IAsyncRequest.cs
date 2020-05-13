using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;

namespace CustomHttpTransport
{
    interface IAsyncRequest : IAsyncResult
    {
        // Methods
        void BeginSendRequest(Message message, TimeSpan timeout);
        Message End();
    }


 

}
