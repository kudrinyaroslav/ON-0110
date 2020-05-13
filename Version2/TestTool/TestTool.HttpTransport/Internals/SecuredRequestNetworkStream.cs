using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.HttpTransport.Internals
{
    class SecuredRequestNetworkStream: RequestNetworkStream
    {
        public SecuredRequestNetworkStream(EndpointAddress to, ICredentialsProvider credentialsProvider) : base(to)
        {
            CredentialsProvider = credentialsProvider;
        }

        protected ICredentialsProvider CredentialsProvider { get; set; }

        protected SslStream SslStream;
        protected override void InitializeNetworkStream(Socket s)
        {
            var serverCertificateValidator = new Func<object,
                                                      X509Certificate,
                                                      X509Chain,
                                                      SslPolicyErrors,
                                                      bool>
                        ((o, serverCertificate, chain, errorPolicy) =>
                            {
                                if (null != CredentialsProvider && null != CredentialsProvider.ServerCertificate)
                                    return serverCertificate.Equals(CredentialsProvider.ServerCertificate);

                                return true;
                            });

            var clientCertificateSelector = new Func<object, 
                                                     string, 
                                                     X509CertificateCollection,
                                                     X509Certificate,
                                                     string[],
                                                     X509Certificate>
                    ((o, host, availableCertificates, serverCertificate, acceptableIssuers) =>
                        {
                            if (null != CredentialsProvider && null != CredentialsProvider.ClientCertificate)
                                return CredentialsProvider.ClientCertificate;

                            return null;
                        });

            base.InitializeNetworkStream(s);
            SslStream = new SslStream(NetworkStream, false,
                                      new RemoteCertificateValidationCallback(serverCertificateValidator),
                                      new LocalCertificateSelectionCallback(clientCertificateSelector));

            SslStream.AuthenticateAsClient(_to.Uri.Host, 
                                           new X509Certificate2Collection(), 
                                           SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12,
                                           false);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            SslStream.Write(buffer, offset, count);
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int size)
        {
            return SslStream.BeginRead(buffer, offset, size, null, null);
        }

        public override int EndRead(IAsyncResult result)
        {
            return SslStream.EndRead(result);
        }
    }
}
