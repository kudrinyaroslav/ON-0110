using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using TestTool.Transport;

namespace TestTool.Device
{
    class ServiceProxy
    {
        public ServiceProxy(string endpointAddress, string deviceServiceAddress)
        {
            _httpPacketHelper = new HttpPacketHelper();

            _endpointAddress = endpointAddress;
            _uri = new Uri(deviceServiceAddress);

        }

        private string _endpointAddress;
        private Uri _uri;
        private HttpSoapServer _server;
        private TcpClient _client;
        private HttpPacketHelper _httpPacketHelper;

        public event NetworkEvent DataTransmitted;

        public void Start()
        {
            _server = new HttpSoapServer(_endpointAddress);
            _server.ProcessMessage += ProcessMessage;
            _server.Start();
        }

        public void Stop()
        {
            _server.Stop();
            _server.ProcessMessage -= ProcessMessage;
            _server = null;
        }

        public bool Started
        {
            get { return _server != null; }
        }

        WebResponse ProcessMessage(WebRequest request)
        {
            try
            {
                Stream input = request.Stream;

                MemoryStream body = new MemoryStream();
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int readByte = input.Read(buffer, 0, buffer.Length);
                    if (readByte == 0)
                    {
                        break;
                    }
                    body.Write(buffer, 0, readByte);
                }
                // RECEIVED DATA
                RaiseNetworkEvent(NetworkEventType.ClientRequest, body.GetBuffer());

                /*********************************************************************************/

                StringBuilder headers = new StringBuilder();
                int size = (int)body.Length;
                headers.Append("POST " + _uri.LocalPath + " HTTP/1.1\r\n");
                headers.Append("Content-Type: application/soap+xml; charset=utf-8\r\n");
                headers.AppendLine("Host: " + _uri.Host);
                headers.Append("Content-Length: " + size.ToString() + "\r\n");
                headers.Append("\r\n");

                byte[] headerBytes = Encoding.UTF8.GetBytes(headers.ToString());

                string requestForDevice = GetRequestForDevice(Encoding.UTF8.GetString(body.GetBuffer()));

                _client = new TcpClient(_uri.Host, _uri.Port);

                _client.GetStream().Write(headerBytes, 0, headerBytes.Length);
                _client.GetStream().Write(Encoding.UTF8.GetBytes(requestForDevice), 0, requestForDevice.Length);

                // the same message now...
                RaiseNetworkEvent(NetworkEventType.DutRequest, body.GetBuffer());

                /*********************************************************************************/

                MemoryStream responseStream = new MemoryStream();

                byte offset = 0;
                string statusHeader = null;
                string response = string.Empty;

                int readTimeout = 60;
                DateTime end = DateTime.Now.AddSeconds(readTimeout);
                while (true)
                {
                    int readByte = _client.GetStream().Read(buffer, offset, 1024);

                    if (readByte != 0)
                    {
                        responseStream.Write(buffer, 0, readByte);
                    }                
                    
                    response = _httpPacketHelper.GetBody(responseStream, out statusHeader);
                    if (!string.IsNullOrEmpty(response))
                    {
                        break;
                    }
                    
                    // Timeout
                    if (DateTime.Now > end)
                    {
                        break;
                    }
                }

                int bodyOffset = 0;
                while (response[bodyOffset] != '<' && bodyOffset < response.Length)
                {
                    bodyOffset++;
                }
                response = response.Substring(bodyOffset);

                RaiseNetworkEvent(NetworkEventType.DutResponse, Encoding.UTF8.GetBytes(response));
                _client.Close();

                /*********************************************************************************/

                int receivedBytes = 0;

                string responseForClient = GetResponseForClient(response);
                receivedBytes = responseForClient.Length;

                RaiseNetworkEvent(NetworkEventType.ClientResponse, Encoding.UTF8.GetBytes(responseForClient));

                /*********************************************************************************/

                WebResponse resp = new WebResponse();
                resp.ReceivedBytes = receivedBytes;
                resp.Stream.Write(Encoding.UTF8.GetBytes(responseForClient), 0, receivedBytes);

                string[] statusParts = statusHeader.Split(' ');
                if (statusParts.Length > 1)
                {
                    resp.StatusCode = int.Parse(statusParts[1]);
                }
                return resp;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                if (_client != null)
                {
                    _client.Close();
                }
            }

        }

        private void RaiseNetworkEvent(NetworkEventType type, byte[] data)
        {
            if (DataTransmitted != null)
            {
                DataTransmitted(new NetworkEventData(type, data));
            }
        }
    

        protected virtual string GetRequestForDevice(string request)
        {
            return request;
        }

        protected virtual string GetResponseForClient(string response)
        {
            return response;
        }

    }

}
