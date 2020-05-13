using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace HttpServer
{
    public class HttpSoapServer : IDisposable
    {
        private HttpListener _listener;
        private Uri _address;

        public Action<byte[]> OnMessage;
        public Action<Exception> OnError;

        public HttpSoapServer(string address)
        {
            _listener = new HttpListener();
            _address = new Uri(address);
            _listener.Prefixes.Add(address);
        }
        public void Start()
        {
            _listener.Start();
            _listener.BeginGetContext(GetContextCallback, _listener);

        }
        public void Stop()
        {
            if (_listener != null)
            {
                _listener.Stop();
            }
        }
        public void Dispose()
        {
            Stop();
        }
        protected bool ValidateRequest(HttpListenerRequest request, out string reason)
        {
            reason = null;
            if (request.HttpMethod != "POST")
            {
                reason = string.Format("Unexpected HTTP method: {0}", request.HttpMethod);
            }
            else if (request.ContentType != "application/soap+xml; charset=utf-8")
            {
                reason = string.Format("Unexpected content type: {0}", request.ContentType);
            }
            else if (!Uri.Equals(request.Url, _address))
            {
                reason = string.Format("Unexpected request url: {0}", request.Url);
            }
            return reason == null;
        }
        protected void GetContextCallback(IAsyncResult ar)
        {
            HttpListenerContext context = null;
            HttpListener listener = (HttpListener)ar.AsyncState;
            try
            {
                context = listener.EndGetContext(ar);
                HttpListenerResponse response = context.Response;
                

                HttpListenerRequest request = context.Request;
                string reason;
                if (!ValidateRequest(request, out reason))
                {
                    OnError(new Exception(reason));
                }
                else if(OnMessage != null)
                {
                    byte[] body = new byte[request.ContentLength64];
                    context.Request.InputStream.Read(body, 0, body.Length);
                    OnMessage(body);
                }

                response.StatusCode = (int)HttpStatusCode.OK;
                //response.ContentLength64 = -1;
                response.ContentLength64 = 0;
                //Send response
                response.Close();

            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(string.Format("{0} HttpSoapServer::GetContextCallback error [{1}]",
                    DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff"), ex.Message));
                Trace.Flush();
            }
            listener.BeginGetContext(GetContextCallback, listener);
        }
    }
}
