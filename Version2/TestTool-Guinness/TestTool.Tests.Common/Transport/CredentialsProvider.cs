using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.Tests.Common.Transport
{
    public class CredentialsProvider : ICredentialsProvider
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Security Security { get; set; }
    }
}
