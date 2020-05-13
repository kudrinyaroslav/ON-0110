///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.IO;

namespace HttpTransport
{
    /// <summary>
    /// Provides functionality for sending raw SOAP packets.
    /// </summary>
    public class HttpClient
    {
        private Uri _address;

        private int _timeout;

        public HttpClient(string address, int timeout)
        {
            _address = new Uri(address);
            _timeout = timeout;
        }
        
        public void SendSoapMessage(string request)
        {
            // create request bytes
            byte[] soapBytes = Encoding.UTF8.GetBytes(request);

            // create headers
            byte[] httpHeaders = CreateHttpHeaders(soapBytes.Length ,
                                                   _address.AbsolutePath,
                                                   _address.Host);

            //whole message with headers
            byte[] bytes = new byte[httpHeaders.Length + soapBytes.Length];

            Array.Copy(httpHeaders, bytes, httpHeaders.Length);

            Array.Copy(soapBytes, 0, bytes, httpHeaders.Length, soapBytes.Length);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(_address.Host, _address.Port);
            System.Net.Sockets.NetworkStream _networkStream = new NetworkStream(socket);
            

            _networkStream.Write(bytes, 0, bytes.Length);
            _networkStream.Close();
            
        }

        public static byte[] CreateHttpHeaders(long size, string path, string address)
        {
            // Create HTTP headers and add content
            StringBuilder httpRequest = new StringBuilder();
            httpRequest.AppendFormat(STATUSLINEPATTERN, path);
            httpRequest.AppendFormat(HOSTLINEPATTERN, address);
            httpRequest.AppendFormat("{0}: {1}; {2}={3}\r\n", CONTENTTYPE, APPLICATIONSOAPXML, CHARSET, "utf-8");
            httpRequest.Append(CONTENTLENGTH + ": " + (size).ToString() + "\r\n");
            httpRequest.Append("\r\n");

            // Convert HTTP request to byte array to send
            return (Encoding.UTF8.GetBytes(httpRequest.ToString()));
        }

        public const string CONTENTLENGTH = "Content-Length";
        public const string CONTENTTYPE = "Content-Type";
        private const string APPLICATIONSOAPXML = "application/soap+xml";
        private const string CHARSET = "charset";
        private const string UTF8 = "utf-8";

        private const string STATUSLINEPATTERN = "POST {0} HTTP/1.1\r\n";
        private const string HOSTLINEPATTERN = "Host: {0}\r\n";

    }
}
