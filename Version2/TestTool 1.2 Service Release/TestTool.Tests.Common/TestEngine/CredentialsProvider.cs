using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.HttpTransport.ChannelControllers;

namespace TestTool.Tests.Common.TestEngine
{
    public class CredentialsProvider : ICredentialsProvider
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
