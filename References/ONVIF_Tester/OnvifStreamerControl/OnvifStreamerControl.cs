/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using System.Net;
using System.Collections;
using System.IO;
using System.Threading;

namespace Onvif
{
    public partial class StreamerControl : System.Windows.Forms.UserControl
    {
        #region constants
        private enum TransportProtocol
        {
            RtspOverHttp,
            RtpOverUdp
        }

        public sealed class Codec
        {
            public const string MPEG4 = "MPEG4";
            public const string MJPEG = "MJPEG";
            public const string H264 = "H264";
        }

        public sealed class RtpPayload
        {
            public const int Pcmu = 0;
            public const int Mjpeg2435 = 26;
            public const int Mpeg4 = 96;
        }

        public sealed class ControlPort
        {
            public const int Rtsp = 554;
            public const int Http = 80;
            public const int Http_Tunnel = 8080;//TUNNEL-PATCH
        }

        private const int RtpStampsPerMillisecond = 90;  //90khz

        private const string STEP_SPACING = "     ";
        private const string STEP_MSG_SPACING = "       ";
        private const string ERROR_MSG_PREFIX = "Error - ";

        #endregion

        #region private fields

        private Brush _highlightBrush = Brushes.Aquamarine;
        private Brush _normalBrush = Brushes.White;
        private Bitmap _repaintImage;

        private RtpInterleavedTcpClient _rtspClient;
        private string _sessionCookie;
        private bool _base64EncodeRtsp;
        private int _cseq = 1;
        private string _rtpPacketFilter;
        private string _rtcpPacketFilter;
        private string _session;
        private int _interleavedVideoChannel;
        private int _sessionTimeout;
        private long _lastRtpSequence;
        private int _controlPort;
        private TransportProtocol _transportProtocol;

        private ArrayList _frame = new ArrayList();
        private bool _frameValid;
        private JpegHeaderFactory _jpegHeaderFactory = new JpegHeaderFactory();
        private string _codec;
        private string _logId;

        private bool _mouseOverPlay;
        private bool _mouseOverPause;
        private bool _playing;
        private System.Windows.Forms.Timer _keepAliveTimer;

        private RtpCallbackState _video;
        private object _packetProcessingLock = new Object();

        private string _videoUri;

        private RtspResponse RtspSetParameter_Response = null;

        #endregion

        #region public properties

        private int _rtspTimeout = 5000;
        public int RtspTimeout
        {
            get { return _rtspTimeout; }
            set { _rtspTimeout = value; }
        }

        private int _rtpReceivePort = 40000;
        public int RtpReceivePort
        {
            get { return _rtpReceivePort; }
            set { _rtpReceivePort = value; }
        }

        private string _streamUri="";
//        private string _streamUri = "rtsp://192.168.8.210/rtsp_tunnel?h26x=0&Profile=0&video_on=0"; //bosch
//        private string _streamUri = "http://192.168.8.210/rtsp_tunnel?h26x=0&Profile=0&video_on=0"; //bosch
//        private string _streamUri = "rtsp://192.168.8.74/mjpeg/media.smp"; //samsung
//        private string _streamUri = "rtsp://192.168.8.71/mpeg4/media.amp"; //axis
//        private string _streamUri = "http://98.246.78.119/axis-media/media.amp?resolution=640x480&videocodec=jpeg&audio=0&compression=20"; //axis tom

        public string StreamUri
        {
            get
            {
                return _streamUri;
            }
            set
            {
                string test = value;
                if (test.ToUpper().StartsWith("RTSP") || test.ToUpper().StartsWith("HTTP")) 
                {
                    _streamUri = value;
                }
            }
        }

        private string _userName; 
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _password;
        public string Password
        {
            get 
            {
                if (_password == null || _password.Length == 0)
                    return "";
                else
                    return "**********";
            }
            set { _password = value; }
        }

        #endregion

        #region constructors
        public StreamerControl()
        {
            InitializeComponent();

            _logId = Handle.ToString("X");

            _keepAliveTimer = new System.Windows.Forms.Timer();
        }
        #endregion

        #region public methods

        
        public bool Init(string URI)
        {
            if (_playing)
                return _playing;
            
            _playing = true;
            this.StreamUri = URI;
            try
            {
                ConnectToUri();

                _video = new RtpCallbackState();

                if ( (_controlPort == ControlPort.Http) || (_controlPort == ControlPort.Http_Tunnel))
                {
                    _transportProtocol = TransportProtocol.RtspOverHttp;
                    //DoHttpGet(); // done individually from the test tool
                    //EstablishHttpPost(); // done individually from the test tool
                    _rtspClient.OnInterleavedPacket += new RtpInterleavedTcpClient.InterleavedPacketHandler(_rtspClient_OnInterleavedPacket);
                }
                else
                {
                    _transportProtocol = TransportProtocol.RtpOverUdp;
                    InitRtp();

                }
            }
            catch (Exception ex)
            {
                _playing = false;
                Logger.Error.WriteLine("Exception in Play: " + ex.Message);
            }
            if (!_playing)
            {
                CleanupRtspResources();
                CleanupRtpResources();
            }

            return _playing;
        }

        public bool RTSP_Options(out string results, out RtspResponse response)
        {
            results = "";
            response = null;

            if (!_playing)
            {
                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Device not connected" + Environment.NewLine;
                return _playing;
            }

            try
            {
                response = RtspOptions();

                if (response.Status != 200)
                {
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NVT Did NOT send 200 OK Response, Received " + response.Status.ToString() + " status" + Environment.NewLine;
                    return false;
                }

                IDictionaryEnumerator _enumerator = response.HeaderFields.GetEnumerator();

                while (_enumerator.MoveNext())
                {
                    results += STEP_MSG_SPACING + "RTSP Option Methods = " + _enumerator.Key + " - " + _enumerator.Value + Environment.NewLine;
                }

            }
            catch (Exception ex)
            {
                _playing = false;
                Logger.Error.WriteLine("Exception in Options: " + ex.Message);
                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Exception in Options: " + ex.Message + Environment.NewLine;
                    
            }

            if (!_playing)
            {
                CleanupRtspResources();
                CleanupRtpResources();
            }

            return _playing;
        }
        
        public bool RTSP_Describe(out string results, out RtspResponse response)
        {
            results = "";
            response = null;


            if (!_playing)
            {
                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Device not connected" + Environment.NewLine;
                return _playing;
            }

            try
            {
                response = RtspDescribe();

                if (response.Status != 200)
                {
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NVT Did NOT send 200 OK Response, Received " + response.Status.ToString() + " status" + Environment.NewLine;
                    return false;
                }

                results += STEP_MSG_SPACING + "RTSP Describe context = " + response.Content.Replace("\n", ", ");
                results = results.TrimEnd(new char[] { ',', ' ' });
                results += Environment.NewLine;

                IDictionaryEnumerator _enumerator = response.HeaderFields.GetEnumerator();
                while (_enumerator.MoveNext())
                {
                    results += STEP_MSG_SPACING + "RTSP Describe options = " + _enumerator.Key + " - " + _enumerator.Value + Environment.NewLine;
                }

            }
            catch (Exception ex)
            {
                _playing = false;
                Logger.Error.WriteLine("Exception in Describe: " + ex.Message);
                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Exception in Describe: " + ex.Message + Environment.NewLine;
                
            }

            if (!_playing)
            {
                CleanupRtspResources();
                CleanupRtpResources();
            }

            return _playing;
        }

        public bool RTSP_Setup(out string results, out RtspResponse response)
        {
            results = "";
            bool passed = true;
            response = null;

            if (!_playing)
            {
                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Device not connected" + Environment.NewLine;
                return _playing;
            }

            try
            {
                response = RtspSetup();

                if (response.Status != 200)
                {
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NVT Did NOT send 200 OK Response, Received " + response.Status.ToString() + " status" + Environment.NewLine;
                    return false;
                }

                // verify the Transport, Session and Timeout fields
                if (response.Session == "")
                {
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NVT Did NOT return Session ID" + Environment.NewLine;                    
                    passed &= false;
                }

                if(response.SessionTimeout == -1)
                {
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NVT Did NOT return Session Timeout" + Environment.NewLine;
                    passed &= false;
                } else
                    StartKeepAliveTimer();

                if(response.TransportFields.Count == 0)
                {
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NVT Did NOT return Session Transport information" + Environment.NewLine;
                    passed &= false;
                }

                if (passed)
                {
                    results += STEP_MSG_SPACING + "RTSP Setup Session ID = " + response.Session + Environment.NewLine;

                    results += STEP_MSG_SPACING + "RTSP Setup Timeout = " + response.SessionTimeout.ToString() + Environment.NewLine;


                    IDictionaryEnumerator _enumerator = response.TransportFields.GetEnumerator();
                    while (_enumerator.MoveNext())
                    {
                        if( (_enumerator.Value != null) && ((_enumerator.Value.ToString()) != ""))
                            results += STEP_MSG_SPACING + "RTSP Setup Transport = " + _enumerator.Key + " - " + _enumerator.Value + Environment.NewLine;
                        else
                            results += STEP_MSG_SPACING + "RTSP Setup Transport = " + _enumerator.Key + Environment.NewLine;
                    }

                }

            }
            catch (Exception ex)
            {
                _playing = false;
                Logger.Error.WriteLine("Exception in Setup: " + ex.Message);
                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Exception in Setup: " + ex.Message + Environment.NewLine;
                
            }

            if (!_playing)
            {
                CleanupRtspResources();
                CleanupRtpResources();
            }

            return _playing;
        }

        public bool RTSP_Play(out string results, out RtspResponse response)
        {

            results = "";
            response = null;

            if (!_playing)
            {
                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Device not connected" + Environment.NewLine;                   
                return _playing;
            }

            try
            {
                response = RtspPlay();

                if (response.Status != 200)
                {
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NVT Did NOT send 200 OK Response, Received " + response.Status.ToString() + " status" + Environment.NewLine;
                    return false;
                }

                results += STEP_MSG_SPACING + "RTSP Video stream playing" + Environment.NewLine;




            }
            catch (Exception ex)
            {
                _playing = false;
                Logger.Error.WriteLine("Exception in Play: " + ex.Message);
                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Exception in Play: " + ex.Message + Environment.NewLine;

            }

            if (!_playing)
            {
                CleanupRtspResources();
                CleanupRtpResources();
            }

            return _playing;

        }

        public bool RTSP_SetParameter(out string results, out RtspResponse response)
        {
            int count;
            results = "";
            response = null;

            if (!_playing)
            {
                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Device not connected" + Environment.NewLine;
                return _playing;
            }

            try
            {
                //StartKeepAliveTimer();

                count = 0;
                response = RtspSetParameter();
                if (response == null)
                {
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Response not yet received " + Environment.NewLine;
                    return false;
                }

                if (response.Status != 200)
                {
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NVT Did NOT send 200 OK Response, Received " + response.Status.ToString() + " status" + Environment.NewLine;
                    return false;
                }

                results += STEP_MSG_SPACING + "RTSP Video Set Parameter responded correctly" + Environment.NewLine;


            }
            catch (Exception ex)
            {
                _playing = false;
                Logger.Error.WriteLine("Exception in Set Parameter: " + ex.Message);
                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Exception in Set Parameter: " + ex.Message + Environment.NewLine;

            }

            if (!_playing)
            {
                CleanupRtspResources();
                CleanupRtpResources();
            }

            return _playing;

        }

        public bool RTSP_Teardown(out string results, out RtspResponse response)
        {
            results = "";
            response = null;
            bool stepPassed = true;

            lock (_packetProcessingLock)
            {
                Debug.WriteLine("Got the pause lock");
                if (!_playing)
                {
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Device not connected" + Environment.NewLine;
                    return _playing;
                }

                _playing = false;

                _keepAliveTimer.Stop();

                CleanupRtpResources();

                try
                {
                    if (_session != null)
                    {
                        response = RtspTeardown();

                        if (response.Status != 200)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NVT Did NOT send 200 OK Response, Received " + response.Status.ToString() + " status" + Environment.NewLine;
                            return false;
                        }

                        results += STEP_MSG_SPACING + "RTSP Video is now toredown" + Environment.NewLine;

                    }
                    else
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Session not found" + Environment.NewLine;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error.WriteLine("Exception in Pause: " + ex.Message);
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Exception in Teardown: " + ex.Message + Environment.NewLine;
                    stepPassed = false;
                }

                CleanupRtspResources();
            }


            return stepPassed;
        }
        

        public bool HTTP_Get(out string results, out string response)
        {
            try
            {
                DoHttpGet(out response);
                results = STEP_MSG_SPACING + "HTTP GET - Passed" + Environment.NewLine;

                response = response.TrimEnd(new char[] { '\r', '\n' });
                response = response.Replace("\r\n", "\r\n" + STEP_MSG_SPACING);
                

                results = STEP_MSG_SPACING + "HTTP Packet received - " + response + Environment.NewLine;
                

                return true;
            }
            catch
            {
                response = null;
                results = STEP_MSG_SPACING + ERROR_MSG_PREFIX + "HTTP GET - Failed" + Environment.NewLine;
                return false;
            }

        }

        public bool HTTP_Post(out string results, out string sentPacket)
        {
            try
            {
                EstablishHttpPost(out sentPacket);
                results = STEP_MSG_SPACING + "HTTP POST - Passed" + Environment.NewLine;

                sentPacket = sentPacket.TrimEnd(new char[] { '\r', '\n' });
                sentPacket = sentPacket.Replace("\r\n", "\r\n" + STEP_MSG_SPACING);

                results = STEP_MSG_SPACING + "HTTP Packet sent - " + sentPacket + Environment.NewLine;
                return true;
            }
            catch
            {
                sentPacket = "";
                results = STEP_MSG_SPACING + ERROR_MSG_PREFIX + "HTTP POST - Failed" + Environment.NewLine;
                return false;
            }
        }

        public void Play()
        {
            if (_playing) return;

            _playing = true;
            try
            {
                ConnectToUri();

                _video = new RtpCallbackState();

                if ((_controlPort == ControlPort.Http) || (_controlPort == ControlPort.Http_Tunnel))
                {
                    _transportProtocol = TransportProtocol.RtspOverHttp;
                    DoHttpGet();
                    EstablishHttpPost();
                    _rtspClient.OnInterleavedPacket += new RtpInterleavedTcpClient.InterleavedPacketHandler(_rtspClient_OnInterleavedPacket);
                }
                else
                {
                    _transportProtocol = TransportProtocol.RtpOverUdp;
                    InitRtp();

                }
                RtspOptions();
                RtspDescribe();
                RtspSetup();
                RtspPlay();

                if (_transportProtocol == TransportProtocol.RtpOverUdp) StartKeepAliveTimer();

            }
            catch (Exception ex)
            {
                _playing = false;
                Logger.Error.WriteLine("Exception in Play: " + ex.Message);
            }

            if (!_playing)
            {
                CleanupRtspResources();
                CleanupRtpResources();
            }
        }

        public void Pause()
        {
            Debug.WriteLine("Enter Pause");
            lock (_packetProcessingLock)
            {
                Debug.WriteLine("Got the pause lock");
                if (!_playing) return;

                _playing = false;

                _keepAliveTimer.Stop();

                CleanupRtpResources();

                try
                {
                    if (_session != null && _transportProtocol == TransportProtocol.RtpOverUdp)
                    {
                        RtspTeardown();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error.WriteLine("Exception in Pause: " + ex.Message);
                }

                CleanupRtspResources();
            }
            Debug.WriteLine("Exit pause");
        }


        #endregion

        #region keep alive

        private void StartKeepAliveTimer()
        {
            RtspSetParameter_Response = null;
            _keepAliveTimer.Interval = (_sessionTimeout * 5000) / 6;
            _keepAliveTimer.Tick += new EventHandler(_keepAliveTimer_Tick);
            _keepAliveTimer.Start();
        }

        void _keepAliveTimer_Tick(object sender, EventArgs e)
        {
            lock (_packetProcessingLock)
            {
                if (!_playing) return;

                try
                {
                    RtspSetParameter_Response = RtspSetParameter();
                }
                catch (Exception ex)
                {
                    Logger.Error.WriteLine("Exception sending keep alive " + ex.Message);
                }
            }
        }

        #endregion

        #region rtsp commands

        private RtspResponse RtspOptions()
        {
            string cmd = string.Format("OPTIONS {0} RTSP/1.0\r\n", _streamUri);
            string packet = cmd;
            packet += NextSequence();
            packet += "User-Agent: OnvifStreamerControl\r\n";
            packet += "\r\n";            
            string result = SendPacket(packet);

            if (result == "") throw new Exception("No response to RTSP Options");

            Debug.WriteLine(result);
            RtspResponse response = RtspResponse.Parse(result);

            if (response.Status == 401 && response.HeaderFields.ContainsKey("WWW-AUTHENTICATE")) //unauthorized
            {
                ResendWithAuthentication(ref packet, ref response);
            }

            if (response.Status != 200)
            {
                Logger.Info.WriteLine("RTSP failure in SET_PARAMETER with status code " + response.Status.ToString());
            }            

            return response;
        }

        private RtspResponse RtspPlay()
        {
            string packet = string.Format("PLAY {0} RTSP/1.0\r\n", _videoUri);
            packet += NextSequence();
            packet += "User-Agent: OnvifStreamerControl\r\n";
            packet += "Range: npt=now-\r\n";
            packet += string.Format("Session: {0}\r\n", _session);
            packet += "\r\n";

            string result = SendPacket(packet);

            if (result == "") throw new Exception("No response to RTSP Play");

            Debug.WriteLine(result);
            RtspResponse response = RtspResponse.Parse(result);

            if (response.Status == 401 && response.HeaderFields.ContainsKey("WWW-AUTHENTICATE")) //unauthorized
            {
                ResendWithAuthentication(ref packet, ref response);
            }

            if (response.Status != 200)
            {
                throw new Exception("RTSP failure in PLAY with status code " + response.Status.ToString());
            }

            return response;
        }

        private RtspResponse RtspSetup()
        {
            string cmd = string.Format("SETUP {0} RTSP/1.0\r\n", _videoUri);
            string packet = cmd;
            packet += NextSequence();
            packet += "User-Agent: OnvifStreamerControl\r\n";
            switch (_transportProtocol)
            {
                case TransportProtocol.RtspOverHttp:
                    packet += string.Format("Transport: RTP/AVP/TCP;interleaved=0-1\r\n");
                    break;
                case TransportProtocol.RtpOverUdp:
                    packet += string.Format("Transport: RTP/AVP/UDP;unicast;client_port={0}-{1}\r\n", _rtpReceivePort, _rtpReceivePort + 1);
                    break;
            }
            
            packet += "\r\n";

            string result = SendPacket(packet);

            if (result == "") throw new Exception("No response to RTSP Setup");

            Debug.WriteLine(result);
            RtspResponse response = RtspResponse.Parse(result);

            if (response.Status == 401 && response.HeaderFields.ContainsKey("WWW-AUTHENTICATE")) //unauthorized
            {
                ResendWithAuthentication(ref packet, ref response);
            }


            if (response.Status != 200)
            {
                throw new Exception("RTSP failure in SETUP with status code " + response.Status.ToString());
            }

            _session = response.Session;
            _sessionTimeout = response.SessionTimeout;

            if (response.TransportFields.ContainsKey("SERVER_PORT"))
            {
                string serverPortString = response.TransportFields["SERVER_PORT"].ToString();
                string[] serverPorts = serverPortString.Split('-');
                Uri streamUri = new Uri(_streamUri);
                _rtpPacketFilter = string.Format("{0}:{1}", streamUri.Host, serverPorts[0]);
                _rtcpPacketFilter = string.Format("{0}:{1}", streamUri.Host, serverPorts[1]);
            }
            if (response.TransportFields.ContainsKey("INTERLEAVED"))
            {
                string s = response.TransportFields["INTERLEAVED"] as string;
                if (s != null)
                {
                    string[] channelStrings = s.Split('-');
                    _interleavedVideoChannel = Convert.ToInt32(channelStrings[0]);
                }

            }

            return response;
        }

        private RtspResponse RtspDescribe()
        {
            string cmd = string.Format("DESCRIBE {0} RTSP/1.0\r\n", _streamUri);
            string packet = cmd;
            packet += NextSequence();
            packet += "User-Agent: OnvifStreamerControl\r\n";
            packet += "\r\n";

            string result = SendPacket(packet);

            if (result == "") throw new Exception("No response to RTSP Describe");

            Debug.WriteLine(result);
            RtspResponse response = RtspResponse.Parse(result);

            if (response.Status == 401 && response.HeaderFields.ContainsKey("WWW-AUTHENTICATE")) //unauthorized
            {
                ResendWithAuthentication(ref packet, ref response);
            }

            if (response.Status != 200)
            {
                throw new Exception("RTSP failure in DESCRIBE with status code " + response.Status.ToString());
            }

            _videoUri = _streamUri;  //default

            bool jpegType26Available = false;
            //verify we have JPEG type 26 available
            if (response.SdpFields.MediaDefs.Count > 0)
            {
                foreach (Onvif.RtspResponse.MediaDef media in response.SdpFields.MediaDefs)
                {
                    if (media.Media.ToLower() == "video" && media.Payload == 26)
                    {
                        jpegType26Available = true;
                        if (media.Attributes.ContainsKey("CONTROL"))
                        {
                            string controlUri = media.Attributes["CONTROL"].ToString();
                            if (controlUri.ToLower().IndexOf("rtsp") != 0)
                            {
                                _videoUri = _streamUri +"/" + controlUri;
                            }
                            else
                            {
                                _videoUri = controlUri;
                            }
                        }
                        break;
                    }
                }
            }

            if (!jpegType26Available) throw new Exception("Jpeg media descriptor not found in DESCRIBE response");

            return response;
        }

        private RtspResponse RtspTeardown()
        {

            string packet = string.Format("TEARDOWN {0} RTSP/1.0\r\n", _videoUri);
            packet += NextSequence();
            packet += "User-Agent: OnvifStreamerControl\r\n";
            packet += string.Format("Session: {0}\r\n", _session);
            packet += "\r\n";
            _session = null;

            string result = SendPacket(packet);

            if (result == "") throw new Exception("No response to RTSP Teardown");

            Debug.WriteLine(result);
            RtspResponse response = RtspResponse.Parse(result);

            if (response.Status == 401 && response.HeaderFields.ContainsKey("WWW-AUTHENTICATE")) //unauthorized
            {
                ResendWithAuthentication(ref packet, ref response);
            }

            if (response.Status != 200)
            {
                throw new Exception("RTSP failure in TEARDOWN with status code " + response.Status.ToString());
            }
            
            return response;

        }

        private RtspResponse RtspSetParameter()
        {
            string packet = string.Format("SET_PARAMETER {0} RTSP/1.0\r\n", _videoUri);
            packet += NextSequence();
            packet += "User-Agent: OnvifStreamerControl\r\n";
            packet += string.Format("Session: {0}\r\n", _session);
            packet += "\r\n";

            string result = SendPacket(packet); 

            if (result == "") throw new Exception("No response to RTSP SET_PARAMETER");

            Debug.WriteLine(result);
            RtspResponse response = RtspResponse.Parse(result);

            if (response.Status == 401 && response.HeaderFields.ContainsKey("WWW-AUTHENTICATE")) //unauthorized
            {
                ResendWithAuthentication(ref packet, ref response);
            }

            if (response.Status != 200)
            {
                Logger.Info.WriteLine("RTSP failure in SET_PARAMETER with status code " + response.Status.ToString());
            }

            return response;
        }

        #endregion

        #region rtsp operations

        private string SendPacket(string packet)
        {

            Debug.WriteLine(packet);

            if (_base64EncodeRtsp)
            {
                packet = Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(packet));
            }

            byte[] packetBytes = System.Text.UTF8Encoding.UTF8.GetBytes(packet);

            return _rtspClient.SendPacket(packetBytes);
        }

        private void SendPacketNoResponse(string packet)
        {

            Debug.WriteLine(packet);
            byte[] packetBytes = System.Text.UTF8Encoding.UTF8.GetBytes(packet);

            _rtspClient.SendPacketNoResponse(packetBytes);
        }

        private void CleanupRtspResources()
        {
            try
            {
                if (_rtspClient != null)
                {
                    _rtspClient.Close();
                    _rtspClient = null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error.WriteLine("Exception in CleanupRtspResources: " + ex.Message);
            }
        }

        private void ConnectToUri()
        {
            Uri streamUri = new Uri(_streamUri);

            _rtspClient = new RtpInterleavedTcpClient();

            switch (_streamUri.ToUpper().Substring(0, 4))
            {
                case "RTSP":
                    _controlPort = ControlPort.Rtsp;
                    break;
                case "HTTP":
                    _controlPort = ControlPort.Http;
                    break;
                default:
                    throw new NotSupportedException("Uri protocol not supported");
            }

            try
            {
                if (streamUri.Port != -1)
                    _controlPort = streamUri.Port;

            }
            catch { }

            if (!_rtspClient.Connect(streamUri.Host, _controlPort))
            {
                throw new TimeoutException("Unable to connect to " + streamUri.Host);
            }
        }

       
        private string NextSequence()
        {
            return string.Format("CSeq: {0}\r\n", _cseq++);
        }

        #endregion

        #region rtp resources
        private void InitRtp()
        {
            _frameValid = true;
            _lastRtpSequence = -1;
            _sessionTimeout = 60; //default
            _rtpPacketFilter = null;
            _rtcpPacketFilter = null;
                     



            int rtpVideoReceivePort = (_rtpReceivePort + 1) & 0xFFFE;


            while (true)
            {
                try
                {
                    _video.RtpClient = new UdpClient(rtpVideoReceivePort);
                    _video.RtcpClient = new UdpClient(rtpVideoReceivePort + 1);
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode == (int)SocketError.AddressAlreadyInUse)
                    {
                        rtpVideoReceivePort += 2;
                        continue;
                    }

                    throw;
                }
                break;
            }

            _rtpReceivePort = rtpVideoReceivePort;

            _video.RtpClient.Client.ReceiveBufferSize = 300 * 1024;
            _video.RtcpClient.Client.ReceiveBufferSize = 1 * 1024;

            _video.RtpClient.BeginReceive(new AsyncCallback(OnRtpData), _video);
            _video.RtcpClient.BeginReceive(new AsyncCallback(OnRtcpData), _video);
        }

        private void CleanupRtpResources()
        {
            try
            {
                Debug.WriteLine("Cleaning up rtp");
                if (_video != null)
                {
                    if (_video.RtpClient != null) _video.RtpClient.Close();
                    if (_video.RtcpClient != null) _video.RtcpClient.Close();
                    _video = null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error.WriteLine("Exception in CleanupRtp: " + ex.Message);
            }

        }


        #endregion

        #region rtp processing

        private void _rtspClient_OnInterleavedPacket(int channel, byte[] packet)
        {
            lock (_packetProcessingLock)
            {
                if (!_playing) return;

                try
                {
                    if (channel == _interleavedVideoChannel) ProcessRtpPacket(ref packet, _video);
                    else if (channel == _interleavedVideoChannel + 1) ProcessRtcpPacket(ref packet, _video);
                }
                catch (Exception ex)
                {
                    Logger.Error.WriteLine("Exception in OnInterleavedPacket Callback " + ex.Message);
                }
            }

        }

        private void OnRtpData(IAsyncResult result)
        {
            lock (_packetProcessingLock)
            {
                if (!_playing) return;

                try
                {

                    RtpCallbackState state = (RtpCallbackState)result.AsyncState;

                    System.Net.IPEndPoint sentFrom = new IPEndPoint(IPAddress.Any, 0);

                    byte[] packet = state.RtpClient.EndReceive(result, ref sentFrom);

                    if (_rtpPacketFilter != null)  //do not process the packet until we have a valid filter set
                    {
                        string[] filterParts = _rtpPacketFilter.Split(':');

                        if (filterParts.Length == 2 && filterParts[0] == sentFrom.Address.ToString() && filterParts[1] == sentFrom.Port.ToString())
                        {
                            ProcessRtpPacket(ref packet, state);
                        }
                    }

                    state.RtpClient.BeginReceive(new AsyncCallback(OnRtpData), state);
                }
                catch (Exception ex)
                {
                    Logger.Error.WriteLine("Exception in OnRtpData: " + ex.Message);
                }
            }
        }
        private void ProcessRtcpPacket(ref byte[] packet, RtpCallbackState state)
        {
            Debug.WriteLine("Processing Rtcp Packet");
        }

        private void ProcessRtpPacket(ref byte[] packet, RtpCallbackState state)
        {
            if (packet.Length < 12) return;

            bool extension = (packet[0] & 0x10) == 0x10;	//00010000
            byte sources = (byte)(packet[0] & 0x0F); //00001111

            bool eof = (packet[1] & 0x80) == 0x80;	//10000000
            int payloadType = (packet[1] & 0x7F);			//01111111
            ushort sequence = (ushort)(packet[2] << 8 | packet[3]);
            long tl = (packet[4] << 24) | (packet[5] << 16) | (packet[6] << 8) | packet[7];
            uint timestamp = (uint)tl;

            long ssrc64 = (packet[8] << 24) | (packet[9] << 16) | (packet[10] << 8) | packet[11];
            int ssrc = (int)ssrc64;

            //EXTN-PATCH byte headerSize = (byte)(12 + sources * 4);
            ushort headerSize = (ushort)(12 + sources * 4);
            if (extension)
            {
                int extensionSize = (packet[headerSize + 2] << 8) + packet[headerSize + 3];
                headerSize += 4;
                //EXTN-PATCH headerSize += (byte)(extensionSize * 4);
                headerSize += (ushort)(extensionSize * 4);
            }

            if (headerSize != 12)
            {
                Logger.Info.WriteOnce(string.Format("OnvifStreamerControl[{0}] long rtp header detected\n", _logId));
            }

            if (state.RtpStats.SSRC != ssrc)
            {
                state.RtpStats.InitSequence(ssrc, sequence);
                state.LastTransit = Environment.TickCount * RtpStampsPerMillisecond - timestamp;
                _lastRtpSequence = sequence;
            }
            else
            {
                if (sequence == _lastRtpSequence) return; //duplicate packet
                _lastRtpSequence = sequence;
            }

            long transit = Environment.TickCount * RtpStampsPerMillisecond - timestamp;
            long d = transit - state.LastTransit;
            state.LastTransit = transit;
            if (d < 0) d = -d;
            state.TransitJitter += (.0625) * ((double)d - state.TransitJitter);

            state.RtpStats.UpdateSequence(sequence);

            switch (payloadType)
            {
                case RtpPayload.Mjpeg2435:
                    ProcessJpeg2435Packet(packet, eof, sequence, timestamp, ssrc, headerSize);
                    _codec = Codec.MJPEG;
                    break;
                default:
                    Logger.Info.WriteOnce(string.Format("RtpPlayer[{0}] wrong payload type in rtp header detected {1}\n", _logId, payloadType));
                    // wrong type of payload.  Get the next packet.
                    return;
            }
        }

        private void ProcessJpeg2435Packet(byte[] packet, bool eof, ushort sequence, uint timestamp, int ssrc, /*EXTN-PATCH byte*/ ushort headerSize)
        {
            ushort pos = headerSize;
            byte typeSpecific = packet[pos++];
            int fragmentOffset = (packet[pos++] << 16) | (packet[pos++] << 8) | (packet[pos++]);
            byte pixelFormatType = (byte)(packet[pos] & 0x3F);
            _jpegHeaderFactory.LuminanceVerticalSampling = pixelFormatType + 1;
            bool restartMarkers = (packet[pos++] & 0x40) > 0;
            byte qField = packet[pos++];
            _jpegHeaderFactory.Width = packet[pos++] * 8;
            _jpegHeaderFactory.Height = packet[pos++] * 8;

            if (restartMarkers)
            {
                uint restartBits = BitConverter.ToUInt32(packet, pos);
                //todo: support the restart bits in RTP JPEG
                pos += 4;
            }
            if (fragmentOffset == 0 && (qField & 0xF0) > 0)
            {
                byte mbz = packet[pos++];
                byte precisionFlags = packet[pos++];
                ushort qTableLength = (ushort)((packet[pos++] << 8) | (packet[pos++]));
                int endPos = pos + qTableLength;


                byte tableNumber = 0;
                while (pos < endPos)
                {

                    int tableSize = ((precisionFlags & (1 << tableNumber)) > 0) ? 128 : 64;
                    byte[] qTable = new byte[tableSize];
                    Array.Copy(packet, pos, qTable, 0, tableSize);

                    _jpegHeaderFactory.SetQuantTable(tableNumber, qTable);
                    tableNumber++;
                    pos += (ushort)tableSize;
                }

            }

            ProcessVideoPacket(packet, eof, sequence, timestamp, ssrc, pos);
        }

        #endregion

        #region Http operations
        private void EstablishHttpPost()
        {
            string packet;
            EstablishHttpPost(out packet);
        }
        private void EstablishHttpPost(out string packet)
        {
            Uri streamUri = new Uri(_streamUri);
            string cmd = string.Format("POST {0} HTTP/1.0\r\n", streamUri.PathAndQuery);
            packet = cmd;
            packet += "User-Agent: OnvifStreamerControl\r\n";
            packet += string.Format("x-sessioncookie: {0}\r\n",_sessionCookie);
            packet += "Content-Type: application/x-rtsp-tunnelled\r\n";
            packet += "Pragma: no-cache\r\n";
            packet += "Cache-Control: no-cache\r\n";
            packet += "Content-Length: 32767\r\n";
            DateTime expires = DateTime.Now;
            expires.AddDays(1);
            packet += string.Format("Expires: {0}\r\n", expires.ToUniversalTime().ToString("R"));
            packet += "\r\n";

            SendPacketNoResponse(packet);

            _base64EncodeRtsp = true;
        }

        private void DoHttpGet()
        {
            
            string result;
            DoHttpGet(out result);
        }

        private void DoHttpGet(out string result)
        {
            _base64EncodeRtsp = false;
            Uri streamUri = new Uri(_streamUri);
            string cmd = string.Format("GET {0} HTTP/1.0\r\n", streamUri.PathAndQuery);
            string packet = cmd;
            packet += "User-Agent: OnvifStreamerControl\r\n";

            _sessionCookie = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            packet += string.Format("x-sessioncookie: {0}\r\n",_sessionCookie);

            packet += "Accept: application/x-rtsp-tunnelled\r\n";
            packet += "Pragma: no-cache\r\n";
            packet += "Cache-Control: no-cache\r\n";
            packet += "Connection: Keep-Alive\r\n";
            packet += "\r\n";

            result = SendPacket(packet);

            if (result == "") throw new Exception("No response to HTTP Get");

            Debug.WriteLine(result);
            RtspResponse response = RtspResponse.Parse(result);

            if (response.Status == 401 && response.HeaderFields.ContainsKey("WWW-AUTHENTICATE")) //unauthorized
            {
                result += Environment.NewLine + ResendWithAuthentication(ref packet, ref response);
            }

            if (response.Status != 200)
            {
                throw new Exception("RTSP failure in Http GET with status code " + response.Status.ToString());
            }

            _rtspClient.ConnectSendChannel(streamUri.Host, _controlPort);
        }

        #endregion

        #region Authentication Functions
        private string ResendWithAuthentication(ref string packet, ref RtspResponse response)
        {
            int insertPoint = packet.IndexOf('\n');
            string cmd = packet.Substring(0, insertPoint + 1);
            string headers = packet.Substring(insertPoint + 1);
            string result = "";

            string auth = GetAuthentication(cmd, response.HeaderFields["WWW-AUTHENTICATE"].ToString());
            if (auth.Length > 0)
            {
                packet = cmd + auth + headers;

                result = SendPacket(packet);

                if (!_rtspClient.Connected)
                {
                    ConnectToUri();
                    result = SendPacket(packet);
                }                

                if (result == "") throw new Exception("No response to HTTP Get");

                Debug.WriteLine(result);
                response = RtspResponse.Parse(result);
            }

            return result;
        }

        private string GetAuthentication(string cmd, string authenticateParams)
        {
            if (authenticateParams.ToLower().IndexOf("digest") == 0) //digest authentication
            {
                int firstSpace = cmd.IndexOf(' ');
                string verb = cmd.Substring(0, firstSpace);
                return DigestAuthentication.CreateDigestAuthentication(
                    authenticateParams,
                    _userName,
                    _password,
                    _streamUri,
                    verb);
            }
            else if (authenticateParams.ToLower().IndexOf("basic") == 0)  //basic authentication
            {
                string credential = string.Format("{0}:{1}", _userName, _password);
                credential = Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(credential));
                return string.Format("Authorization: Basic {0}\r\n", credential);
            }

            return "";
        }
        #endregion

        #region video processing
        private void ProcessVideoPacket(byte[] packet, bool eof, ushort sequence, uint timestamp, int ssrc, ushort headerSize)
        {
            if (_frame.Count == 0)
            {
                _video.LastSequence = sequence;
            }
            else
            {
                ushort sequenceDiff = (ushort)(sequence - _video.LastSequence);
                if (sequenceDiff != 1)
                {
                    _frameValid = false;
                    Logger.Info.WriteLine(string.Format("Video sequence break detected {0} to {1}\n", _video.LastSequence, sequence));
                }

                _video.LastSequence = sequence;
            }

            packet[0] = (byte)(headerSize & 0xFF);  //just set a pointer to the start of the frame data
            packet[1] = (byte)((headerSize & 0xFF00) >> 8);

            _frame.Add(packet);

            if (eof) ProcessFrame(timestamp);
        }

        private void ProcessFrame(uint timestamp)
        {
            if (_frameValid)
            {
                switch (_codec)
                {
                    case Codec.MJPEG:
                        ProcessJpegFrame(timestamp);
                        break;
                }
            }

            _frame.Clear();
            _frameValid = true;

        }

        private void ProcessJpegFrame(uint timestamp)
        {
            int frameSize = 0;

            //build frame

            foreach (byte[] packet in _frame)
            {
                int headerSize = (packet[0] | (packet[1] << 8));
                if (packet.Length <= headerSize)
                {
                    _frameValid = false;
                    break;
                }

                frameSize += packet.Length - headerSize;
            }

            if (_frameValid && frameSize > 0)
            {


                byte[] frame = new byte[frameSize];
                int pos = 0;
                foreach (byte[] packet in _frame)
                {
                    int headerSize = (packet[0] | (packet[1] << 8));

                    Array.Copy(packet, headerSize, frame, pos, packet.Length - headerSize);
                    pos += packet.Length - headerSize;
                }

                byte[] headers = _jpegHeaderFactory.GetHeaders();


                byte[] fixedFrame = new byte[headers.Length + frame.Length + 2];
                Array.Copy(headers, fixedFrame, headers.Length);
                Array.Copy(frame, 0, fixedFrame, headers.Length, frame.Length);
                fixedFrame[fixedFrame.Length - 2] = 0xff;
                fixedFrame[fixedFrame.Length - 1] = 0xD9;

                frame = fixedFrame;

                DecodeJpegFrame(frame);
            }
        }

        private void DecodeJpegFrame(byte[] frame)
        {
            Bitmap bm = null;

            Debug.WriteLine("Decode jpeg frame with size " + frame.Length.ToString());                

            try
            {
                MemoryStream ioStream = new MemoryStream(frame);
                bm = new Bitmap(ioStream);
            }
            catch (Exception ex)
            {
                Logger.Error.WriteLine("Exception in DecodeJpegFrame " + ex.Message);
            }

            if (bm != null)
            {
                try
                {
                    BeginInvoke(new DisplayJpegFrameDelegate(DisplayJpegFrame), bm);
                }
                catch (System.ObjectDisposedException ex)
                {
                    Debug.WriteLine("Ignoring object disposed exception in invoke");
                }
                catch (Exception ex)
                {
                    Logger.Error.WriteOnce("Exception in invoke JPEG display: " + ex.Message);
                }
            }
        }

        private delegate void DisplayJpegFrameDelegate(Bitmap bm);

        private void DisplayJpegFrame(Bitmap bm)
        {
            try
            {
                using (Graphics g = Graphics.FromHwnd(Handle))
                {
                    g.DrawImage(bm, g.VisibleClipBounds);
                    DrawButtons(g);
                }

                if (_repaintImage != null) _repaintImage.Dispose();
                _repaintImage = bm;

            }
            catch (Exception ex)
            {
                Logger.Error.WriteLine("Exception doing graphics render " + ex.ToString());
            }

            bm = null;

        }

        #endregion

        #region UI operations
        private void DrawButtons(Graphics g)
        {
            Rectangle clientRect = this.ClientRectangle;
            Rectangle playRect = new Rectangle(clientRect.Width / 2 - 32 - 3, clientRect.Bottom - 32 - 6, 32, 32);
            Rectangle pauseRect = new Rectangle(clientRect.Width / 2 + 3, clientRect.Bottom - 32 - 6, 32, 32);

            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            g.DrawRectangle(Pens.White, playRect);
            g.DrawRectangle(Pens.White, pauseRect);

            Point[] playButtonVertices = new Point[3];
            playButtonVertices[0]= new Point(playRect.Left + playRect.Width/3,playRect.Top + playRect.Height/3);
            playButtonVertices[1]= new Point(playRect.Left + playRect.Width/3,playRect.Top + 2*playRect.Height/3);
            playButtonVertices[2]= new Point(playRect.Left + 3*playRect.Width/4,playRect.Top + playRect.Height/2);

            g.FillPolygon(_mouseOverPlay ? _highlightBrush : _normalBrush, playButtonVertices);

            g.FillRectangle(_mouseOverPause ? _highlightBrush : _normalBrush, new Rectangle(pauseRect.Left + 10, pauseRect.Top + 8, 5, 16));
            g.FillRectangle(_mouseOverPause ? _highlightBrush : _normalBrush, new Rectangle(pauseRect.Right - 15, pauseRect.Top + 8, 5, 16));
        }

        #endregion

        #region rtcp processing

        private void OnRtcpData(IAsyncResult result)
        {            
            lock (_packetProcessingLock)
            {
                Debug.WriteLine("Enter OnRtcpData");
                if (!_playing) return;

                try
                {

                    RtpCallbackState state = (RtpCallbackState)result.AsyncState;

                    System.Net.IPEndPoint sentFrom = new IPEndPoint(IPAddress.Any, 0);

                    byte[] packet = state.RtcpClient.EndReceive(result, ref sentFrom);

                    if (_rtcpPacketFilter != null)  //do not process the packet until we have a valid filter set
                    {
                        string[] filterParts = _rtcpPacketFilter.Split(':');

                        if (filterParts.Length == 2 && filterParts[0] == sentFrom.Address.ToString() && filterParts[1] == sentFrom.Port.ToString())
                        {
                            ProcessRtcpPacket(ref packet, state);
                        }
                    }

                    state.RtpClient.BeginReceive(new AsyncCallback(OnRtcpData), state);
                }
                catch (Exception ex)
                {
                    Logger.Error.WriteLine("Exception in OnRtpData: " + ex.Message);
                }

                Debug.WriteLine("Exit OnRtcpData");
            }
        }

        #endregion

        #region protected overrides

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            if (_repaintImage != null)
            {
                e.Graphics.DrawImage(_repaintImage, e.Graphics.VisibleClipBounds);
            }
            else
            {
                base.OnPaintBackground(e);
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            DrawButtons(e.Graphics);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)  components.Dispose();

                if (_playing) Pause();
            }
            base.Dispose(disposing);
        }

        #endregion       

        #region mouseevents

        private void OnvifStreamerControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Rectangle clientRect = this.ClientRectangle;
            Rectangle playRect = new Rectangle(clientRect.Width / 2 - 32 - 3, clientRect.Bottom - 32 - 6, 32, 32);
            Rectangle pauseRect = new Rectangle(clientRect.Width / 2 + 3, clientRect.Bottom - 32 - 6, 32, 32);

            if (playRect.Contains(e.Location)) Play();
            else if (pauseRect.Contains(e.Location)) Pause();
        }

        private void OnvifStreamerControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Rectangle clientRect = this.ClientRectangle;
            Rectangle playRect = new Rectangle(clientRect.Width / 2 - 32 - 3, clientRect.Bottom - 32 - 6, 32, 32);
            Rectangle pauseRect = new Rectangle(clientRect.Width / 2 + 3, clientRect.Bottom - 32 - 6, 32, 32);

            bool mouseOverPlay = playRect.Contains(e.Location);
            bool mouseOverPause = pauseRect.Contains(e.Location);
            bool redrawButtons = false;
            if (mouseOverPlay != _mouseOverPlay)
            {
                _mouseOverPlay = mouseOverPlay;
                redrawButtons = true;
            }
            if (mouseOverPause != _mouseOverPause)
            {
                _mouseOverPause = mouseOverPause;
                redrawButtons = true;
            }

            if (redrawButtons)
            {
                Graphics g = Graphics.FromHwnd(Handle);
                DrawButtons(g);
                g.Dispose();
            }

        }

        private void OnvifStreamerControl_MouseLeave(object sender, EventArgs e)
        {
            bool mouseOverPlay = false;
            bool mouseOverPause = false;
            bool redrawButtons = false;
            if (mouseOverPlay != _mouseOverPlay)
            {
                _mouseOverPlay = mouseOverPlay;
                redrawButtons = true;
            }
            if (mouseOverPause != _mouseOverPause)
            {
                _mouseOverPause = mouseOverPause;
                redrawButtons = true;
            }

            if (redrawButtons)
            {
                Graphics g = Graphics.FromHwnd(Handle);
                DrawButtons(g);
                g.Dispose();
            }
        }
        #endregion
    }
}