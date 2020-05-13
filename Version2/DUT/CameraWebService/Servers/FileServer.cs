using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace CameraWebService.FileServer
{
    public class FileServer: IDisposable
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        private object m_StatusCodeLock = new object();
        private object m_StatusCodeNextLock = new object();
        private HttpStatusCode m_StatusCode;
        private HttpStatusCode m_StatusCodeNext;

        public HttpStatusCode StatusCodeNext
        {
            get
            {
                lock (m_StatusCodeNextLock)
                { return m_StatusCodeNext; }
            }
            set
            {
                lock (m_StatusCodeNextLock)
                { m_StatusCodeNext = value; }
            }
        }
        public HttpStatusCode StatusCode
        {
            get
            {
                lock (m_StatusCodeLock)
                { return m_StatusCode; }
            }
            set
            {
                lock (m_StatusCodeLock)
                { m_StatusCode = value; }
            }
        }

        private bool m_Run = false;
        private static FileServer _instance;
        public static FileServer getInstance()
        {
            if (null == _instance)
                _instance = new FileServer((request) => { return string.Empty; });

            return _instance;
        }

 
        public FileServer(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
 
            // URI prefixes are required, for example 
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");
 
            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");
 
            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);
 
            _responderMethod = method;

            StatusCode = HttpStatusCode.OK;

            _listener.Start();
        }
 
        public FileServer(Func<HttpListenerRequest, string> method, params string[] prefixes): this(prefixes, method) { }

        public FileServer(Func<HttpListenerRequest, string> method): this(new [] { "http://localhost:12345/fakefileupload/" }, method) { }
 
        public void Run()
        {
            if (!m_Run)
            {
                m_Run = true;
                ThreadPool.QueueUserWorkItem((o) =>
                    {
                        Console.WriteLine("FileServer running...");
                        try
                        {
                            while (_listener.IsListening)
                            {
                                ThreadPool.QueueUserWorkItem((c) =>
                                    {
                                        var ctx = c as HttpListenerContext;
                                        try
                                        {
                                            string rstr = _responderMethod(ctx.Request);
                                            byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                            ctx.Response.StatusCode = (int)StatusCode;
                                            StatusCode = HttpStatusCode.UnsupportedMediaType;
                                            ctx.Response.ContentLength64 = buf.Length;
                                            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                                        }
                                        catch
                                        {
                                        } // suppress any exceptions
                                        finally
                                        {
                                            // always close the stream
                                            ctx.Response.OutputStream.Close();
                                        }
                                    },
                                    _listener.GetContext());
                            }
                        }
                        catch
                        {
                        } // suppress any exceptions
                    });
            }
        }
 
        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}    
