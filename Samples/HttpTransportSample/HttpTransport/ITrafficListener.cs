using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpTransport
{
    public interface ITrafficListener : IChannelController
    {
        void LogRequest(string data);
        void LogResponse(string data);

    }
}
