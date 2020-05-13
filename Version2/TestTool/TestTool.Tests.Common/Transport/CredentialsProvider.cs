using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.Tests.Common.Transport
{
    public class CredentialsProvider : ICredentialsProvider
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Security Security { get; set; }
        public DigestTestingSettings DigestTestingSettings { get; set; }
        public bool HTTPS { get; set; }
        public X509Certificate2 ClientCertificate { get; set; }
        public X509Certificate  ServerCertificate { get; set; }
    }
}
