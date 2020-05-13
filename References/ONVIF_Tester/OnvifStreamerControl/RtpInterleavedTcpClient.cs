/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;


namespace Onvif
{
    public class RtpInterleavedTcpClient
    {
        enum InterleavedDataState
        {
            StartPacket,
            CompletePacket
        }

        byte[] _inboundData;
        int _inboundDataOffset;
        TcpClient _tcpClient;
        TcpClient _tcpSendClient;
        AsyncCallback _inboundDataCallback;
        object _callbackSync = new object();
        Queue _resultStringList = new Queue();
        ManualResetEvent _resultStringReady = new ManualResetEvent(false);
        private int _tcpTimeOut = 5000;
        public string LocalEndpoint;

        public delegate void InterleavedPacketHandler(int channel, byte[] packet);

        public event InterleavedPacketHandler OnInterleavedPacket;

        public RtpInterleavedTcpClient()
        {
        }

        public void SendPacketNoResponse(byte[] packet)
        {
            if (_tcpSendClient != null)
            {
                _tcpSendClient.Client.Send(packet);
            }
            else
            {
                _tcpClient.Client.Send(packet);
            }
        }

        public string ReadNextString()
        {
            lock (_resultStringList.SyncRoot)
            {
                if (_resultStringList.Count > 0) return (string)_resultStringList.Dequeue();
                _resultStringReady.Reset();
            }

            if (_resultStringReady.WaitOne(_tcpTimeOut, false))
            {
                lock (_resultStringList.SyncRoot)
                {
                    if (_resultStringList.Count > 0) return (string)_resultStringList.Dequeue();
                }
            }

            return null;
        }

        public string SendPacket(byte[] packet)
        {
            lock (_resultStringList.SyncRoot)
            {
                _resultStringList.Clear();
                _resultStringReady.Reset();
            }

            SendPacketNoResponse(packet);

            string result = ReadNextString();

            if(result == null) result = "";

            return result;

        }

        public bool ConnectSendChannel(string address, int port)
        {
            _tcpSendClient = new TcpClient();

            try
            {
                IAsyncResult asyncResult = _tcpSendClient.BeginConnect(address, port, null, null);

                if (!asyncResult.AsyncWaitHandle.WaitOne(_tcpTimeOut, false))
                {
                    _tcpSendClient.Close();
                    _tcpSendClient = null;
                    return false;
                }

                _tcpSendClient.EndConnect(asyncResult);

            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Connected
        {
            get
            {
                if (_tcpClient == null) return false;

                return _tcpClient.Connected;
            }
        }

        public bool Connect(string address, int port)
        {
            _tcpClient = new TcpClient();
            _tcpClient.Client.ReceiveBufferSize = 128 * 1024;
//            _inboundData = new byte[1500];
            _inboundData = new byte[8000]; //TUNNEL-PATCH
            _inboundDataOffset = 0;
            _inboundDataCallback = new AsyncCallback(OnInboundData);

            try
            {
                IAsyncResult asyncResult = _tcpClient.BeginConnect(address, port, null, null);

                if (!asyncResult.AsyncWaitHandle.WaitOne(_tcpTimeOut, false))
                {
                    _tcpClient.Close();
                    _tcpClient = null;
                    return false;
                }

                _tcpClient.EndConnect(asyncResult);

                IPEndPoint endpoint = (IPEndPoint)_tcpClient.Client.LocalEndPoint; 
                LocalEndpoint = endpoint.Address.ToString();
        



                _tcpClient.Client.BeginReceive(_inboundData, _inboundDataOffset, /*1500 TUNNEL-PATCH*/8000, SocketFlags.None, _inboundDataCallback, null);
            }
            catch
            {
                return false;
            }

            return true;
        }


        public void Close()
        {
            lock (_callbackSync)
            {
                if (_tcpClient != null)
                {
                    _tcpClient.Close();
                    _tcpClient = null;
                }
                if (_tcpSendClient != null)
                {
                    _tcpSendClient.Close();
                    _tcpSendClient = null;
                }
            }
        }

        private void OnInboundData(IAsyncResult asyncResult)
        {
            int bytesRead;

            lock (_callbackSync)
            {
                if (_tcpClient == null) return;

                try
                {
                    bytesRead = _tcpClient.Client.EndReceive(asyncResult);
//                    Debug.WriteLine(string.Format("On inbound data of size {0}", bytesRead));
                }

                catch (SocketException ex)
                {
                    Logger.Error.WriteLine("Exception in interleaved TCP endrecieve " + ex.ToString());
                    _tcpClient.Close();
                    _tcpClient = null;
                    return;
                }
            }


            if (bytesRead > 0)
            {
                _inboundDataOffset += bytesRead;

                ProcessInboundData();  //this could end up closing the client if the send back to the client hits a closed socket so we need to check for null below

            }

            lock (_callbackSync)
            {
                if (_tcpClient == null)
                {
                    return;
                }

                try
                {
                    _tcpClient.Client.BeginReceive(_inboundData, _inboundDataOffset, /*1500 TUNNEL-PATCH*/8000 - _inboundDataOffset, SocketFlags.None, _inboundDataCallback, null);
                }
                catch (SocketException ex)
                {
                    _tcpClient.Close();
                    _tcpClient = null;
                    _resultStringReady.Set();
                    Logger.Error.WriteLine("Exception in interleaved TCP begin recieve " + ex.ToString());
                    return;
                }
            }
        }

        private void ProcessInboundData()
        {
            try
            {
                if (_inboundData[0] != 36)
                {
                    string resultString = System.Text.UTF8Encoding.UTF8.GetString(_inboundData, 0, _inboundDataOffset);
                    lock (_resultStringList.SyncRoot)
                    {
                        _resultStringList.Enqueue(resultString);
                    }
                    _resultStringReady.Set();
                    int endPosition = resultString.Length;
                    if (_inboundDataOffset > endPosition)
                    {
                        int bytesLeft = _inboundDataOffset - endPosition;
                        byte[] leftOver = new byte[bytesLeft];
                        Array.Copy(_inboundData, endPosition, leftOver, 0, bytesLeft);
                        Array.Copy(leftOver, _inboundData, bytesLeft);
                        _inboundDataOffset = bytesLeft;
                    }
                    else
                    {
                        _inboundDataOffset = 0;
                    }

                    if (_inboundDataOffset > 4) ProcessInboundData();
                }
                else
                {
                    byte iChannel = _inboundData[1];
                    int interleavedPacketSize = _inboundData[2] * 256 + _inboundData[3];
                    int endPosition = interleavedPacketSize + 4;

                    if (endPosition > _inboundData.Length) //the packet size is bigger than our buffer so something is corrupt
                    {
                        Logger.Error.WriteLine("RtpInterleavedTcpClient processInboundData encountered interleaved packet larger than buffer " + interleavedPacketSize);
                        _inboundDataOffset = 0;
                        return;
                    }

                    if (_inboundDataOffset >= endPosition)
                    {
                        //we have whole packet
                        byte[] iPacket = new byte[interleavedPacketSize];
                        Array.Copy(_inboundData, 4, iPacket, 0, interleavedPacketSize);
                        if (OnInterleavedPacket != null) OnInterleavedPacket(iChannel, iPacket);

                        if (_inboundDataOffset > endPosition)
                        {
                            int bytesLeft = _inboundDataOffset - endPosition;
                            byte[] leftOver = new byte[bytesLeft];
                            Array.Copy(_inboundData, endPosition, leftOver, 0, bytesLeft);
                            Array.Copy(leftOver, _inboundData, bytesLeft);
                            _inboundDataOffset = bytesLeft;
                        }
                        else
                        {
                            _inboundDataOffset = 0;
                        }

                        if (_inboundDataOffset > 4) ProcessInboundData();
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error.WriteLine("Exception processing inbound data on RtpInterleavedTcpClient " + ex.Message);
                _inboundDataOffset = 0;
            }

            
        }
    }
}
