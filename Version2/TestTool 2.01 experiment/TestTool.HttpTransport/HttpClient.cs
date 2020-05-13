///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.HttpTransport.Exceptions;
using TestTool.HttpTransport.Internals.Http;
using System.Threading;
using System.IO;

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

        public HttpClient(string address, int timeout)
            : this (address, timeout, null)
        {
        }

        public HttpClient(string address, int timeout, IExecutionController controller)
        {
            _address = new Uri(address);
            _timeout = timeout;
            _executionController = controller;
        }
        
        public string SendSoapMessage(string request)
        {
            // create request bytes
            byte[] soapBytes = Encoding.UTF8.GetBytes(request);

            // create headers
            byte[] httpHeaders = HttpHelper.CreateHttpHeaders(soapBytes.Length ,
                                                   _address.AbsolutePath,
                                                   _address.Host);

            //whole message with headers
            byte[] bytes = new byte[httpHeaders.Length + soapBytes.Length];

            Array.Copy(httpHeaders, bytes, httpHeaders.Length);

            Array.Copy(soapBytes, 0, bytes, httpHeaders.Length, soapBytes.Length);

#if DEBUG
            //string str = Encoding.UTF8.GetString(bytes);
            //System.Diagnostics.Debug.WriteLine(string.Format("SEND: {0}", str));
#endif
            
            RequestNetworkStream _networkStream = new RequestNetworkStream(new EndpointAddress(_address.OriginalString));
            _networkStream.Connect();

            _networkStream.Write(bytes, 0, bytes.Length);

            MemoryStream responseStream = new MemoryStream();
            byte[] responseBuffer = new byte[2048];
            int readTimeout = _timeout;
            int bytesCount = 0;
            bool bContinue = false;
            HttpPacket header = null;

            do
            {
                DateTime startTime = DateTime.Now;

                IAsyncResult result = _networkStream.BeginRead(responseBuffer, 0, responseBuffer.Length);

                // wait for bytes receied, stop event or timeout
                WaitHandle[] handles;
                if (_executionController != null && _executionController.StopEvent != null)
                {
                    handles = new WaitHandle[] { result.AsyncWaitHandle, _executionController.StopEvent };
                }
                else
                {
                    handles = new WaitHandle[] { result.AsyncWaitHandle };
                }
                int handle = System.Threading.WaitHandle.WaitAny(handles, readTimeout);

                if (handle == WaitHandle.WaitTimeout)
                {
                    _networkStream.Close();
                    throw new IOException("The HTTP request has exceeded the allotted timeout");
                }

                if (handle == 1)
                {
                    System.Diagnostics.Debug.WriteLine("Stop event");
                    _networkStream.Close();
                    _executionController.ReportStop();
                }

                bytesCount = _networkStream.EndRead(result);

                DateTime endTime = DateTime.Now;

                System.Diagnostics.Debug.WriteLine(string.Format("--- Bytes received: {0}", bytesCount));

                responseStream.Write(responseBuffer, 0, bytesCount);

                // timeout for next part of answer
                readTimeout -= (int)((endTime - startTime).TotalMilliseconds);

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

                if (readTimeout < 0)
                {
                    _networkStream.Close();
                    throw new IOException("The HTTP request has exceeded the allotted timeout");
                }
            }
            while (_networkStream.DataAvailable || bContinue);

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


    }
}
