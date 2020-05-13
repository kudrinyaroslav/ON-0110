using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;

namespace CameraWebService.Servers
{
    public class HTTPSServer: IDisposable
    {
        public const int Port = 12346;

        private readonly TcpListener _listener;
        
        private EventWaitHandle _timeoutHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private EventWaitHandle _serverStopHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private bool _forceStop = false;

        private bool m_Run = false;
        private static HTTPSServer _instance;
        public static HTTPSServer getInstance(bool renew = false)
        {
            if (null == _instance || renew)
            {
                if (null != _instance)
                    _instance.Stop();
                _instance = new HTTPSServer();
            }

            return _instance;
        }

 
        public HTTPSServer()
        {
            _listener = new TcpListener(IPAddress.Any, Port);
            _listener.Start();
        }
 
        public void Run(X509Certificate serverCertificate, int serverHelloTimeout)
        {
            if (!m_Run)
            {
                m_Run = true;
                ThreadPool.QueueUserWorkItem((_o) =>
                    {
                        Console.WriteLine("HTTPS Server running...");

                        try
                        {
                            _serverStopHandle.Reset();
                            while (!_forceStop)
                            {
                                if (_listener.Pending())
                                {
                                    ThreadPool.QueueUserWorkItem((o) =>
                                        {
                                            var client = o as TcpClient;

                                            var clientCertificateValidator = new Func
                                                    <object, X509Certificate, X509Chain, SslPolicyErrors, bool>
                                                    ((obj, clientCertificate, chain, errorPolicy) => true);

                                            var serverCertificateSelector = new Func
                                                    <object, string, X509CertificateCollection, X509Certificate,
                                                            string[],
                                                            X509Certificate>
                                                    ((obj,
                                                      host,
                                                      availableCertificates,
                                                      clientCertificate,
                                                      acceptableIssuers) =>
                                                        {
                                                            _timeoutHandle.WaitOne(serverHelloTimeout);
                                                            if (_forceStop)
                                                                throw new Exception("Forced stop");

                                                            return serverCertificate;
                                                        });

                                            var sslStream = new SslStream(client.GetStream(),
                                                                          false,
                                                                          new RemoteCertificateValidationCallback(
                                                                                  clientCertificateValidator),
                                                                          new LocalCertificateSelectionCallback(
                                                                                  serverCertificateSelector),
                                                                          EncryptionPolicy.RequireEncryption);


                                            try
                                            {
                                                sslStream.AuthenticateAsServer(serverCertificate);
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.ToString());
                                            }
                                            finally
                                            {
                                                sslStream.Close();
                                                client.Close();
                                            }
                                        },
                                        _listener.AcceptTcpClient());
                                }
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            _serverStopHandle.Set();
                        }
                    });
                            
            }
        }
 
        public void Stop()
        {
            _forceStop = true;
            _timeoutHandle.Set();

            _serverStopHandle.WaitOne(10000);
            _listener.Stop();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}