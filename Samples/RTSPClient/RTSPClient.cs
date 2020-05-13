using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace RTSPClient
{
    public enum TransportProtocol
    {
        RtspOverHttp,
        RtpOverUdp,
        RtpOverTcp
    }
    public class StreamingClient : IDisposable
    {
        private UdpClient _rtpClient;
        private UdpClient _rtcpClient;
        private TcpClient _tcpClient = new TcpClient();
        private IPAddress _hostIp;
        private Uri _uriHost;
        private byte[] _incomingBuffer = new byte[8000];
        private string _incomingPacket;
        private int _incomingDataLen = 0;
        private AutoResetEvent _dataReceived = new AutoResetEvent(false);
        private int _cseq;
        private string _session;
        private string _videoUri;
        private System.Windows.Forms.Timer _sessionWatchdog = new System.Windows.Forms.Timer();
        private object _clientLock = new object();


        public TransportProtocol Transport { get; set; }
        public int RtpReceivePort { get; set; }

        public StreamingClient()
        {
            Transport = TransportProtocol.RtpOverUdp;
            RtpReceivePort = 1234;
            _sessionWatchdog.Tick += new EventHandler(OnSessionWatchdogTick);
        }
        public void Connect(Uri uri)
        {
            _cseq = 1;
            lock (_clientLock)
            {
                int port = uri.Port != -1 ? uri.Port : 554;
                _tcpClient = new TcpClient();
                _tcpClient.Client.ReceiveBufferSize = 128 * 1024;
                _tcpClient.Connect(uri.Host, port);
                _uriHost = uri;
                _hostIp = IPAddress.Parse(_uriHost.Host);
                StartListening();
            }
        }
        public void Disconnect()
        {
            lock (_clientLock)
            {
                if (_tcpClient.Connected)
                {
                    _tcpClient.Close();
                }
            }
        }
        public string Play()
        {
            string res = string.Empty;
            string options = SendCommand(BuildCommand("OPTIONS"));
            if(options.Contains("DESCRIBE"))
            {
                string describe = SendCommand(BuildCommand("DESCRIBE"));
                RtspResponse responseDescribe = RtspResponse.Parse(describe);
                ParseDescribeResponse(describe);
                
                string transport = GetTranspotParam();
                string setup = SendCommand(BuildCommand("SETUP", _videoUri, transport));

                RtspResponse response = RtspResponse.Parse(setup);
                if (response.Status != 200)
                {
                    throw new Exception("RTSP failure in SETUP with status code " + response.Status.ToString());
                }

                _session = response.Session;
                if (response.TransportFields.ContainsKey("SERVER_PORT"))
                {
                    string serverPortString = response.TransportFields["SERVER_PORT"].ToString();
                    string[] serverPorts = serverPortString.Split('-');
                    res = string.Format("rtp://{0}:{1}@192.168.10.79:{2}", _uriHost.Host, serverPorts[0], RtpReceivePort);
                    //res = string.Format("rtp://192.168.10.79:{2}", _uriHost.Host, serverPorts[0], RtpReceivePort);
                }
                int sessionTimeout = response.SessionTimeout;
                if(sessionTimeout != -1)
                {
                    _sessionWatchdog.Stop();
                    _sessionWatchdog.Interval = (sessionTimeout * 5000) / 6;
                    _sessionWatchdog.Start();
                }
                //InitRtp();
                string playParam = string.Format("Session: {0}\r\nRange:npt=0.000-\r\n\r\n", _session);
                string play = SendCommand(BuildCommand("PLAY", _uriHost.ToString(), playParam));
            }
            return res;
        }
        private void InitRtp()
        {
            int rtpVideoReceivePort = (RtpReceivePort + 1) & 0xFFFE;


            while (true)
            {
                try
                {
                   _rtpClient = new UdpClient(rtpVideoReceivePort);
                    _rtcpClient = new UdpClient(rtpVideoReceivePort + 1);
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode == (int)SocketError.AddressAlreadyInUse)
                    {
                        rtpVideoReceivePort += 2;
                        continue;
                    }
                    throw ex;
                }
                break;
            }

            RtpReceivePort = rtpVideoReceivePort;

            _rtpClient.Client.ReceiveBufferSize = 300 * 1024;
            _rtcpClient.Client.ReceiveBufferSize = 1 * 1024;

            _rtpClient.BeginReceive(new AsyncCallback(OnRtpData), null);
            _rtcpClient.BeginReceive(new AsyncCallback(OnRtcpData), null);
        }
        protected void OnRtcpData(IAsyncResult asyncResult)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.None, 0);
            _rtcpClient.EndReceive(asyncResult, ref endPoint);
            _rtcpClient.BeginReceive(new AsyncCallback(OnRtcpData), null);
        }
        protected void OnRtpData(IAsyncResult asyncResult)
        {
            IPEndPoint endPoint = new IPEndPoint(_hostIp, _uriHost.Port);
            _rtpClient.EndReceive(asyncResult, ref endPoint);
            _rtpClient.BeginReceive(new AsyncCallback(OnRtpData), null);
        }
        protected void OnSessionWatchdogTick(object sender, EventArgs e)
        {
            string session = string.Format("Session: {0}\r\n\r\n", _session);
            string stop = SendCommand(BuildCommand("SET_PARAMETER", _uriHost.ToString(), session));
        }
        public void Stop()
        {
            _sessionWatchdog.Stop();
            string session = string.Format("Session: {0}\r\n\r\n", _session);
            string stop = SendCommand(BuildCommand("TEARDOWN", _uriHost.ToString(), session));
        }
        protected string GetTranspotParam()
        {
            string res = string.Empty;
            switch (Transport)
            {
                case TransportProtocol.RtspOverHttp:
                    res = "Transport: RTP/AVP/TCP;interleaved=0-1\r\n";
                    break;
                case TransportProtocol.RtpOverUdp:
                    res = string.Format("Transport: RTP/AVP;unicast;client_port={0}-{1}\r\n", RtpReceivePort, RtpReceivePort + 1);
                    break;
                case TransportProtocol.RtpOverTcp:
                    res = string.Format("Transport: RTP/AVP/TCP;unicast;client_port={0}-{1}\r\n", RtpReceivePort, RtpReceivePort + 1);
                    break;
            }
            return res;
        }
        protected void ParseDescribeResponse(string message)
        {
            int sessionStart = message.IndexOf("s=");
            if(sessionStart != -1)
            {
                int sessionStop = message.IndexOf("\r", sessionStart);
                _session = message.Substring(sessionStart + 2, sessionStop - sessionStart - 2);
            }
            int indexA = message.IndexOf("a=");
            if(indexA != -1)
            {
                int indexControl = message.IndexOf("control:", indexA);
                if(indexControl != -1)
                {
                    int controlStop = message.IndexOf("\r", indexControl);
                    string video = message.Substring(indexControl + 8, controlStop - indexControl - 8);
                    _videoUri = _uriHost.ToString() + video;
                }
            }
        }
        protected string BuildCommand(string command, string uri, string param)
        {
            string commandHead = BuildCommand(command, uri);
            return commandHead.Substring(0, commandHead.Length - 2) + param + "\r\n";
        }
        protected string BuildCommand(string command, string uri)
        {
            string res = string.Format("{0} {1} RTSP/1.0\r\nCSeq: {2}\r\nUser-Agent: TestStreamerControl\r\n",
                command,
                uri,
                _cseq++);
            return res + "\r\n";
        }
        protected string BuildCommand(string command)
        {
            string res = string.Format("{0} {1} RTSP/1.0\r\nCSeq: {2}\r\nUser-Agent: TestStreamerControl\r\n\r\n", 
                command, 
                _uriHost,
                _cseq++);
            return res;
        }
        protected string SendCommand(string command)
        {
            lock (_clientLock)
            {
                byte[] packetBytes = System.Text.UTF8Encoding.UTF8.GetBytes(command);
                _tcpClient.Client.Send(packetBytes);

                StartListening();
            }
            string res = string.Empty;
            if (WaitHandle.WaitAll(new WaitHandle[] { _dataReceived }, 4000))
            {
                res = _incomingPacket;
            }
            return res;
        }
        protected void StartListening()
        {
            _incomingDataLen = 0;

            _tcpClient.Client.BeginReceive(
                _incomingBuffer,
                _incomingDataLen,
                /*1500 TUNNEL-PATCH*/8000,
                SocketFlags.None,
                new AsyncCallback(OnReceiving),
                null);
        }
        protected void OnReceiving(IAsyncResult asyncResult)
        {
            lock(_clientLock)
            {
                if (_tcpClient.Client != null)
                {
                    _incomingDataLen = _tcpClient.Client.EndReceive(asyncResult);

                    if (_incomingDataLen > 0)
                    {
                        _incomingPacket = System.Text.UTF8Encoding.UTF8.GetString(_incomingBuffer, 0, _incomingDataLen);
                        _dataReceived.Set();
                    }

                    StartListening();
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if(_tcpClient != null)
            {
                Disconnect();
            }
        }

        #endregion
    }
}
