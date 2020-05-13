///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Net;
using System.IO;

namespace TestTool.Tests.Common.NotificationConsumer
{
    /// <summary>
    /// HTTP server.
    /// </summary>
    public class HttpSoapServer : IDisposable
    {
        /// <summary>
        /// Listener
        /// </summary>
        private HttpListener _listener;
        /// <summary>
        /// Address
        /// </summary>
        private Uri _address;

        /// <summary>
        /// 
        /// </summary>
        public Action<byte[]> OnMessage;
        public Action<Exception> OnError;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address"></param>
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
            try
            {
                if (_listener != null)
                {
                    _listener.Stop();
                }
            }
            catch
            {
            }
        }
        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// Validates request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected bool ValidateRequest(HttpListenerRequest request, out string reason)
        {
            reason = null;
            if (request.HttpMethod != "POST")
            {
                reason = string.Format("Unexpected HTTP method: {0}", request.HttpMethod);
            }
            else if (!ValidateContentType(request))
            {
                reason = string.Format("Unexpected content type: {0}", request.ContentType);
            }
            else if (!Uri.Equals(request.Url, _address))
            {
                reason = string.Format("Unexpected request url: {0}", request.Url);
            }
            else
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^(/([A-Za-z0-9_])+)+/?$");
                if (!regex.IsMatch(request.RawUrl))
                {
                    reason = string.Format("Request-URI in POST line of HTTP request ('{0}') is incorrect", request.RawUrl);
                }
            }
            return reason == null;
        }

        /// <summary>
        /// Validates content type.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected bool ValidateContentType(HttpListenerRequest request)
        {
            string[] contentTypeDefinitions = request.ContentType.Split(';');
            if (contentTypeDefinitions.Length == 0)
            {
                return false;
            }

            string contentType = contentTypeDefinitions[0];
            if (StringComparer.InvariantCultureIgnoreCase.Compare(contentType, "application/soap+xml") != 0)
            {
                return false;
            }

            for (int i = 1; i < contentTypeDefinitions.Length; i++)
            {
                string[] parameters = contentTypeDefinitions[i].Trim().Split('=');
                if (parameters.Length > 1)
                {
                    if (StringComparer.InvariantCultureIgnoreCase.Compare(parameters[0], "charset") == 0)
                    {
                        if (StringComparer.InvariantCultureIgnoreCase.Compare(parameters[1], "utf-8") != 0)
                        {
                            return false;
                        }
                    }
                    else if (StringComparer.InvariantCultureIgnoreCase.Compare(parameters[0], "action") != 0)
                    {
                        // the only valid additional parameter is "action"
                        return false;
                    }
                }
            }

            return true;
        }

        protected virtual void OnMessageReceived(byte[] data)
        {
             if(OnMessage != null)
             {
                 OnMessage(data);
             }
        }
        protected void GetContextCallback(IAsyncResult ar)
        {
            try
            {
                HttpListenerContext context = null;
                HttpListener listener = (HttpListener)ar.AsyncState;
                try
                {
                    context = listener.EndGetContext(ar);
                    HttpListenerRequest request = context.Request;
                    string reason;
                        
                    if (!ValidateRequest(request, out reason))
                    {
                        OnError(new Exception(reason));
                    }
                    else
                    {
                        MemoryStream body = new MemoryStream();
                        byte[] buffer = new byte[512];
                        while (true)
                        {
                            int readByte = context.Request.InputStream.Read(buffer, 0, buffer.Length);
                            if (readByte == 0)
                            {
                                break;
                            }
                            body.Write(buffer, 0, readByte);
                        }                          
                            OnMessageReceived(body.ToArray());
                    }

                    //Send response
                    HttpListenerResponse response = context.Response;
                    response.StatusCode = 200;
                    response.ContentLength64 = 0;
                    response.Close();

                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("{0} HttpSoapServer::GetContextCallback error [{1}]",
                        DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff"), ex.Message));
                    System.Diagnostics.Trace.Flush();
                }
                listener.BeginGetContext(GetContextCallback, listener);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0} HttpSoapServer::GetContextCallback2 error [{1}]",
                    DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff"), ex.Message));
                System.Diagnostics.Trace.Flush();
            }
        }
    }
}
