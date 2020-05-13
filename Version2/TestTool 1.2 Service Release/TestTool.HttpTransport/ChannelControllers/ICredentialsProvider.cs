using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.HttpTransport.ChannelControllers
{
    public interface ICredentialsProvider : ITransportController
    {
        string Username { get; }
        string Password { get; }
    }
}
