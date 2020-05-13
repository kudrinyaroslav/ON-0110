///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.HttpTransport.Interfaces.Exceptions;
using TestTool.HttpTransport.Internals.Http;
using System.Threading;
using System.IO;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.HttpTransport
{
    /// <summary>
    /// Provides functionality for sending raw SOAP packets.
    /// </summary>
    public class HttpClient
    {
        private Uri _address;

        private int _timeout;

        private IExecutionController _executionController;

        private ICredentialsProvider _credentialsProvider;

        private RequestNetworkStream _networkStream;

        /// <summary>
        /// authentication challenge, if once received
        /// </summary>
        private HttpPacket _digestAuthChallenge = null;

        private string _nonceBack;
        private int _nonceCounter;
        public HttpClient(string address, int timeout)
            : this (address, timeout, null, null)
        {
        }

        public HttpClient(string address, 
            int timeout, 
            IExecutionController controller, 
            ICredentialsProvider credentialsProvider )
        {
            _nonceBack = "";
            _nonceCounter = 0;

            _address = new Uri(address);
            _timeout = timeout;
            _executionController = controller;
            _credentialsProvider = credentialsProvider;
        }
        
        public string SendSoapMessage(string request)
        {
            byte[] bytes = CreateMessageBytes(request);

            _networkStream = new RequestNetworkStream(new EndpointAddress(_address.OriginalString));
            _networkStream.Connect();

            _networkStream.Write(bytes, 0, bytes.Length);

            MemoryStream responseStream = new MemoryStream();
            int readTimeout = _timeout;
            HttpPacket header = null;
            
            do
            {
                responseStream = new MemoryStream();
                GetResponse(responseStream, out header, readTimeout);

                if (header.StatusCode == 100)
                {
                    //System.Diagnostics.Debug.WriteLine("100-Continue received");
                }

            } while (header.StatusCode == 100);

            _networkStream.Close();

            bool digestEnabled = false;
            if (_credentialsProvider != null)
            {
                digestEnabled = _credentialsProvider.Security == Security.Digest;
            }
            if (header.StatusCode == 401)
            {
                if (digestEnabled)
                {
                    System.Diagnostics.Debug.WriteLine("HTTP 401 received");
                    _networkStream.Close();

                    _networkStream.EnsureOpen(new EndpointAddress(_address.OriginalString));

                    // Send request once more.
                    _digestAuthChallenge = header;

                    // uses _digestAuthChallenge (and _currentMessageBytes), so behaviour will be different
                    // (_digestAuthChallenge might be not null by the first attempt, but if we get status 401,
                    // it will be updated)
                    byte[] messageBytes = CreateMessageBytes(request);
                    _networkStream.Write(messageBytes, 0, messageBytes.Length);

                    //System.Diagnostics.Debug.WriteLine(string.Format("After ResendMessage: socket: {0}", _networkStream.Connected));

                    do
                    {
                        responseStream = new MemoryStream();
                        GetResponse(responseStream, out header, readTimeout);
                    } while (header.StatusCode == 100);

                    if (header.StatusCode == 401)
                    {
                        _networkStream.Close();
                        throw new AccessDeniedException("Digest authentication FAILED (HTTP status 401 received)");
                    }
                }
                else
                {
                    _networkStream.Close();
                    throw new AccessDeniedException("Access denied (HTTP status 401 received)");
                }
            }

            _networkStream.Close();
            
            int count = (int)responseStream.Length - header.BodyOffset;

            if (header.ContentLength < count)
            {
                throw new HttpProtocolException(
                    string.Format("An error occurred while receiving packet. Expected length: {0}, received: {1}",
                                  header.ContentLength, count));
            }

            // parse response and notify listeners
            string response = HttpHelper.GetFormattedMessage(responseStream.GetBuffer(), header.BodyOffset);
            
            responseStream.Close();
            return response;

        }

        byte[] CreateMessageBytes(string request)
        {
            // create request bytes
            byte[] soapBytes = Encoding.UTF8.GetBytes(request);

            string username = string.Empty;
            string password = string.Empty;
            if (_credentialsProvider != null && _digestAuthChallenge != null)
            {
                username = _credentialsProvider.Username;
                password = _credentialsProvider.Password;
            }

            // create headers
            byte[] httpHeaders = HttpHelper.CreateHttpHeaders(soapBytes.Length,
                                                   _address.AbsolutePath,
                                                   _address.Host,
                                                   _digestAuthChallenge, 
                                                   username, 
                                                   password, 
                                                   null, ref _nonceBack, ref _nonceCounter);

            //whole message with headers
            byte[] bytes = new byte[httpHeaders.Length + soapBytes.Length];

            Array.Copy(httpHeaders, bytes, httpHeaders.Length);

            Array.Copy(soapBytes, 0, bytes, httpHeaders.Length, soapBytes.Length);

            return bytes;
        }
        

        void GetResponse(MemoryStream responseStream, out HttpPacket header, int readTimeout)
        {
            int bytes = 0;
            byte[] responseBuffer = new byte[2048];
            bool bContinue = false;

            int readTimeoutLocal = readTimeout;

            DateTime startTime = DateTime.Now;

            // read response
            do
            {
                IAsyncResult result = _networkStream.BeginRead(responseBuffer, 0, responseBuffer.Length);

                // wait for bytes received, stop event or timeout
                WaitHandle[] handles;
                if (_executionController != null && _executionController.StopEvent != null)
                {
                    handles = new WaitHandle[] { result.AsyncWaitHandle, _executionController.StopEvent };
                }
                else
                {
                    handles = new WaitHandle[] { result.AsyncWaitHandle };
                }
                int handle = System.Threading.WaitHandle.WaitAny(handles, readTimeoutLocal);

                if (handle == WaitHandle.WaitTimeout)
                {
                    _networkStream.Close();
                    throw new Interfaces.Exceptions.TimeoutException("The HTTP request has exceeded the allotted timeout");
                }

                if (handle == 1)
                {
                    //Stop event
                    _networkStream.Close();
                    _executionController.ReportStop();
                }

                bytes = _networkStream.EndRead(result);

                DateTime endTime = DateTime.Now;

                //System.Diagnostics.Debug.WriteLine(string.Format("--- Bytes received: {0}", bytes));

                responseStream.Write(responseBuffer, 0, bytes);

                // timeout for next part of answer
                int ms = (int)((endTime - startTime).TotalMilliseconds);

                // parse response received by this moment.
                try
                {
                    bContinue = HttpHelper.ContinueReading(responseStream, out header);
                }
                catch (Exception exc)
                {
                    _networkStream.Close();
                    // clean resources somehow ?
                    throw new Exception("An error occurred while parsing HTTP packet", exc);
                }

                readTimeoutLocal = readTimeout - ms;
                //System.Diagnostics.Debug.WriteLine("Timeout: " + readTimeoutLocal);
                if (readTimeoutLocal < 0)
                {
                    _networkStream.Close();
                    throw new IOException("The HTTP request has exceeded the allotted timeout");
                }

                //System.Diagnostics.Debug.WriteLine(string.Format("DataAvailable: {0}, continue: {1} ", _networkStream.DataAvailable, bContinue));
            }
            while (_networkStream.DataAvailable || bContinue);

        }

    }
}
