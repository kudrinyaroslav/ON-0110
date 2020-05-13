using System;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using TestTool.Crypto;
using TestTool.Tests.Definitions.Exceptions;

namespace TestTool.Tests.TestCases.TestSuites.AdvancedSecurity
{
    partial class AdvancedSecurityTestSuit
    {

        #region TLS Utils

        private TcpClient establishTCPConnection(int portNumber)
        {
            TcpClient client = null;
            string msg = "";
            if (-1 != portNumber)
                try
                {
                    client = new TcpClient(_cameraIp.ToString(), portNumber);
                }
                catch (Exception e)
                {
                    msg = e.Message;
                    client = null;
                }
            else
                msg = "Invalid port number";

            Assert(null != client,
                   string.Format("Can't establish connection with the DUT over TCP/IP: {0}", msg),
                   "Checking connection with the DUT over TCP/IP is established");

            return client;
        }

        private void performTLSHandshake(NetworkStream stream,
                                         X509CertificateBase clientCertificate,
                                         byte[] expectedServerCertificateBinary)
        {
            var serverCertificateValidator = new Func<object,
                    X509Certificate,
                    X509Chain,
                    SslPolicyErrors,
                    bool>
                    ((o, serverCertificate, chain, errorPolicy) =>
                        {
                            if (null == expectedServerCertificateBinary)
                            {
                                LogStepEvent("No server certificate validation. Accept any one as trusted.");
                                return true;
                            }

                            bool valid = true;

                            byte[] receivedRaw = serverCertificate.GetRawCertData();
                            byte[] expectedRaw = expectedServerCertificateBinary;
                            if (receivedRaw.Count() != expectedRaw.Count())
                                valid = false;

                            if (valid)
                                for (int i = 0; i < receivedRaw.Count(); i++)
                                    if (receivedRaw[i] != expectedRaw[i])
                                    {
                                        valid = false;
                                        break;
                                    }

                            LogStepEvent(string.Format("Checking whether server certificate is as expected... {0}",
                                                       valid ? "Ok" : "Failed"));
                            return valid;
                        });

            var clientCertificateSelector = new Func<object, string, X509CertificateCollection,
                    X509Certificate,
                    string[],
                    X509Certificate>
                    ((o, host, availableCertificates, serverCertificate, acceptableIssuers) =>
                        {
                            if (null != clientCertificate)
                                return new X509Certificate(clientCertificate.GetEncoded());
                            return null;
                        });

            var sslStream = new SslStream(stream,
                                          false,
                                          new RemoteCertificateValidationCallback(serverCertificateValidator),
                                          new LocalCertificateSelectionCallback(clientCertificateSelector),
                                          EncryptionPolicy.RequireEncryption);
            sslStream.ReadTimeout = sslStream.WriteTimeout = OperationDelay;
            
            //var sslStream = new SslStream(stream, false, new RemoteCertificateValidationCallback(serverCertificateValidator));

            var certificate = new X509CertificateBC(new MemoryStream(expectedServerCertificateBinary));

            string targetHost = null == expectedServerCertificateBinary ? _cameraIp.ToString() : certificate.SubjectDN;

            try
            {
                BeginStep("Basic TLS Handshake procedure");
                sslStream.AuthenticateAsClient(targetHost,
                                               new X509CertificateCollection(),
                                               SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12,
                                               false);
                StepPassed();
            }
            catch (Exception e)
            {
                //StepFailed(e);
                throw new AssertException(e.Message, e.InnerException);
            }
        }

        #endregion
    }
}