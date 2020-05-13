using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.HttpTransport.ChannelControllers
{
    public enum Security
    {
        None,
        Digest,
        WS
    }

    public interface ICredentialsProvider : ITransportController
    {
        string Username { get; }
        string Password { get; }
        Security Security { get; }
    }
}
