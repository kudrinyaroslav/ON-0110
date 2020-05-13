/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSXML2;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;


namespace ONVIF_NetworkInterface
{
    #region Exception Code
    [Serializable]
    public class _MessageException : Exception
    {
        public string ErrorMessage
        {
            get
            {
                return base.Message.ToString();
            }
        }

        public _MessageException(string errorMessage)
            : base(errorMessage) { }

        public _MessageException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }
    }

    public class NetworkInterface_SendException : _MessageException
    {
        public NetworkInterface_SendException(string errorMessage)
            : base(errorMessage) { }

        public NetworkInterface_SendException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }
    }

    public class NetworkInterface_ReceiveException : _MessageException
    {
        public NetworkInterface_ReceiveException(string errorMessage)
            : base(errorMessage) { }

        public NetworkInterface_ReceiveException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }

    }

    public class NetworkInterface_TimeoutException : _MessageException
    {
        public NetworkInterface_TimeoutException(string errorMessage)
            : base(errorMessage) { }

        public NetworkInterface_TimeoutException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }

    }

    public class NetworkInterface_InvalidPerameterException : _MessageException
    {
        public NetworkInterface_InvalidPerameterException(string errorMessage)
            : base(errorMessage) { }

        public NetworkInterface_InvalidPerameterException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }

    }

    public class NetworkInterface_NotConnected : _MessageException
    {
        public NetworkInterface_NotConnected(string errorMessage)
            : base(errorMessage) { }

        public NetworkInterface_NotConnected(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }

    }

    public class NetworkInterface_POSTException : _MessageException
    {
        public NetworkInterface_POSTException(string errorMessage)
            : base(errorMessage) { }

        public NetworkInterface_POSTException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }
    }

    public class NetworkInterface_AbortRequest : _MessageException
    {
        public NetworkInterface_AbortRequest(string errorMessage)
            : base(errorMessage) { }

        public NetworkInterface_AbortRequest(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }
    }
       
    #endregion


    public class NetworkInterface_Class
    {
        private const int DEFAULT_TIMEOUT = 5000;

        #region structures
        private struct UdpState
        {
            public UdpClient u;
            public IPEndPoint e;
        }

        private struct GET_State
        {
            public HttpWebRequest get;
        }

        private struct SocketState
        {
            public Socket s;
        }

        private struct PortEndPoint
        {
            public IPAddress Address;
            public int Port;

            public PortEndPoint(IPAddress address, int port)
            {
                this.Address = address;
                this.Port = port;
            }
        }
        #endregion
        
        private UdpClient UDP;
        private IPEndPoint RemoteEndPoint;
        private IPEndPoint EndPoint_Multicast;
        private bool UDP_Connected = false;

        private IPEndPoint messageReceived_EP = null;
        private bool messageReceived = false;
        private bool messageSent = false;
        
        private byte[] messageBytes;

        private int message_SendTimeout = DEFAULT_TIMEOUT;
        private int message_ReceiveTimeout = DEFAULT_TIMEOUT;

        
        private PortEndPoint RemoteConnection = new PortEndPoint(IPAddress.Any, 0);
      
        
        /// <summary>
        /// Open Mulicast port, no IP filtering
        /// </summary>
        /// <param name="Multicast_Address"></param>
        /// <param name="Port"></param>
        /// <param name="TTL"></param>
        public void UDP_ConnectAnyMulticast(string Multicast_Address, int Port, short TTL)
        {
            UDP_ConnectMulticast(Multicast_Address, IPAddress.Any.ToString(), Port, TTL);

        }

        /// <summary>
        /// Open Multicast port.  Setup filtering based on IP
        /// </summary>
        /// <param name="Multicast_Address"></param>
        /// <param name="Target_IP_Address"></param>
        /// <param name="Port"></param>
        /// <param name="TTL"></param>
        public void UDP_ConnectMulticast(string Multicast_Address, string Target_IP_Address, int Port, short TTL)
        {
            IPAddress IP = IPAddress.Parse(Target_IP_Address);
            IPAddress IP_Multicast = IPAddress.Parse(Multicast_Address);

            if (UDP_Connected || (UDP != null)) // if already connected close
                UDP_Close();


            //System.Windows.Forms.MessageBox.Show("UDP", "test", MessageBoxButtons.OK);
            UDP = new UdpClient(Port);

            //System.Windows.Forms.MessageBox.Show("EndPoint", "test", MessageBoxButtons.OK);
            RemoteEndPoint = new IPEndPoint(IPAddress.Parse(Target_IP_Address), Port);

            //System.Windows.Forms.MessageBox.Show("EndPoint_Multicast", "test", MessageBoxButtons.OK);
            EndPoint_Multicast = new IPEndPoint(IPAddress.Parse(Multicast_Address), Port);



            UDP.Ttl = TTL;
            //UDP.JoinMulticastGroup(IP_Multicast, IPAddress.Parse("192.168.1.100"));
            if (IsMulticast(IP_Multicast.ToString()))
                UDP.JoinMulticastGroup(IP_Multicast);
            else
                throw new NetworkInterface_InvalidPerameterException("Multicast IP address invalid");


            // don't listen to its own muilticast though
            //UDP.MulticastLoopback = false;

            //UDP.Connect(EndPoint);
            UDP_Connected = true;
        }

        /// <summary>
        /// UDP connect.
        /// </summary>
        /// <param name="Target_IP_Address"></param>
        /// <param name="Port"></param>
        /// <param name="TTL"></param>
        public void UDP_Connect(string Target_IP_Address, int Port, short TTL)
        {
            IPAddress IP = IPAddress.Parse(Target_IP_Address);

            if (UDP_Connected || (UDP != null)) // if already connected close
                UDP_Close();

            
            RemoteEndPoint = new IPEndPoint(IPAddress.Parse(Target_IP_Address), Port);
            UDP = new UdpClient();

            UDP.Ttl = TTL;

            //UDP.Connect(EndPoint);
            UDP_Connected = true;
        }

        /// <summary>
        /// Send UDP message on open connection
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns></returns>
        public int UDP_Send(string message)
        {
            if (!UDP_Connected)
                throw new NetworkInterface_NotConnected("UDP Client not connected");

            byte[] sendBytes = Encoding.ASCII.GetBytes(message);

            if (SendMessages(RemoteEndPoint, UDP, sendBytes) != true)
                throw new NetworkInterface_SendException("Unable to send to - " + RemoteEndPoint.Address.ToString());

            return sendBytes.Length;
        }
        
        /// <summary>
        /// Send message on Muliticast connection, and listen for response.
        /// </summary>
        /// <param name="timeout_ms">Reveive timeout</param>
        /// <param name="message">Message to send</param>
        /// <returns>Message string received</returns>
        public string UDP_SendMulticast(int timeout_ms, string message, string targetIP)
        {
            Random UDP_DELAY = new Random((int)(System.DateTime.Now.Ticks));
            bool runAgain = true;
            int totalTimeout = timeout_ms;
            System.DateTime endTime = System.DateTime.Now.AddMilliseconds(timeout_ms);


            if (!UDP_Connected)
                throw new NetworkInterface_NotConnected("UDP Client not connected");

            IPEndPoint receivedFrom;
            message_ReceiveTimeout = totalTimeout;

            byte[] sendBytes = Encoding.ASCII.GetBytes(message);
 

            // do not use the asychronus call since this is a mulitcast we arn't expecing anything to listen            
            UDP.Send(sendBytes, sendBytes.Length, EndPoint_Multicast);

            // According to the Soap-over-UDP documentation the message needs to be repeated
            // 4 times, with a Minimum delay of 50 ms, max of 250
            System.Threading.Thread.Sleep(50 + UDP_DELAY.Next(200));

            // send again
            UDP.Send(sendBytes, sendBytes.Length, EndPoint_Multicast);

            byte[] data;
            string returnString;
            do
            {
                //runAgain = false;

                data = ReceiveMessages(RemoteEndPoint, UDP, out receivedFrom);
                returnString = Encoding.ASCII.GetString(data);

                if (receivedFrom.Address.ToString() == IPAddress.Any.ToString())
                    break;

                if (receivedFrom.Address.ToString() == targetIP)
                    break;

                totalTimeout = (endTime - System.DateTime.Now).Milliseconds + ((endTime - System.DateTime.Now).Seconds * 1000) + ((endTime - System.DateTime.Now).Minutes * 60000);

                if (totalTimeout <= 0)
                    break;

                message_ReceiveTimeout = totalTimeout;

            } while (runAgain);

            UDP_Close(); // close the connection

            return returnString;
        }

        /// <summary>
        /// Send message to opened Multicast connection
        /// </summary>
        /// <param name="message"></param>
        public void SendMulticast(string message)
        {
            if (!UDP_Connected)
                throw new NetworkInterface_NotConnected("UDP Client not connected");

            //AnyEndPt = new IPEndPoint(IPAddress.Any, EndPoint.Port);

            byte[] sendBytes = Encoding.ASCII.GetBytes(message);

            // drop the multicast group if not null
            //if (EndPoint != null)
            //    UDP.DropMulticastGroup(EndPoint.Address);

            // do not use the asychronus call since this is a mulitcast we arn't expecing anything to listen    
            if (RemoteEndPoint.Address.ToString() == IPAddress.Any.ToString())
                UDP.Send(sendBytes, sendBytes.Length, EndPoint_Multicast);
            else
                UDP.Send(sendBytes, sendBytes.Length, RemoteEndPoint);

            // now join back up, if needed
            //if (EndPoint != null)
            //    UDP.JoinMulticastGroup(EndPoint.Address);
                 
        }

        /// <summary>
        /// UDP listen, filter on specified IP address
        /// </summary>
        /// <param name="timeout_ms">Timeout in miliseconds</param>
        /// <param name="IP_Target">IP address of target</param>
        /// <returns>Message received</returns>
        public string UDP_Listen(int timeout_ms, string IP_Target)
        {
            IPEndPoint receivedFrom;
            bool messageRecieved = false;
            string returnMessage = "";

            while (!messageRecieved)
            {
                returnMessage = UDP_Listen(timeout_ms, out receivedFrom);

                if (IP_Target == IPAddress.Any.ToString())
                    break;

                if (IP_Target == receivedFrom.Address.ToString())
                    break;
            }

            return returnMessage;
        }

        /// <summary>
        /// Listen on the UDP connection, return the first response without filtering
        /// </summary>
        /// <param name="timeout_ms">Timeout in miliseconds</param>
        /// <param name="receivedFrom">Transimitter endpoint</param>
        /// <returns>Message received</returns>
        public string UDP_Listen(int timeout_ms, out IPEndPoint receivedFrom)
        {
            message_ReceiveTimeout = timeout_ms;

            byte[] receiveBytes = ReceiveMessages(RemoteEndPoint, UDP, out receivedFrom);
            
            string message = Encoding.ASCII.GetString(receiveBytes);

            return message;
        }

        /// <summary>
        /// Close the UDP connection
        /// </summary>
        public void UDP_Close()
        {
            if(UDP != null)
                UDP.Close();
            //UDP = null;
            //EndPoint_Multicast = null;
            this.RemoteEndPoint = null;
            UDP_Connected = false;
        }



        //public string POST_Message2(string sendURL, string message, string user, string pwd)
        //{

        //    //XMLHTTP xh = new XMLHTTP();
        //    XMLHTTP XML_HTTP = new XMLHTTP();

        //    XML_HTTP.open("POST", sendURL, false, user, pwd);

        //    XML_HTTP.setRequestHeader("Content-Type", "text/xml; charset=utf-8");
        //    XML_HTTP.setRequestHeader("SOAPAction", "");

        //    try{
        //    XML_HTTP.send(message);
        //    }
        //    catch (Exception e)
        //    {               
        //        //return e.Message;
        //        string errorText = e.Message.TrimEnd(new char[] { '\n', '\r' });
        //        throw new NetworkInterface_POSTException(errorText);
        //    }

        //    return XML_HTTP.responseText;
        //}



        public string POST_Message(int timeout_ms, string sendURL, string message, string user, string pwd)
        {
            
            MSXML2.ServerXMLHTTP xh = new ServerXMLHTTP();
            //MSXML2.ServerXMLHTTP xh = new ServerXMLHTTP();

            // set the timeouts
            xh.setTimeouts(timeout_ms, timeout_ms, timeout_ms, timeout_ms);

            Console.WriteLine("Sending request...");
            xh.open("POST", sendURL, false, user, pwd);

            //xh.setRequestHeader("Content-Type", "text/xml; charset=utf-8");
            xh.setRequestHeader("Content-Type", "application/soap+xml; charset=utf-8");  // SOAP Version 1.2 Part 2: Adjuncts (Second Edition)
            

            xh.setOption(SERVERXMLHTTP_OPTION.SXH_OPTION_SELECT_CLIENT_SSL_CERT, "");
                        

            try
            {
                xh.send(message);
            }
            catch (Exception e)
            {
                string errorText = e.Message.TrimEnd(new char[] { '\n', '\r' });
                throw new NetworkInterface_POSTException(errorText);
            }

            if (xh.status == 401)
            {
                throw new NetworkInterface_POSTException("Error - " + xh.status.ToString() + " Username or Password not correct" + xh.statusText);
            }

            return xh.responseText;
        }


        private bool IsMulticast(string IP_Address)
        {
            char[] IP_Split = {'.'};
            string[] IP_Parts = IP_Address.Split(IP_Split);
            byte[] AddressBytes = new byte[] { byte.Parse(IP_Parts[0]),
                                               byte.Parse(IP_Parts[1]),
                                               byte.Parse(IP_Parts[2]),
                                               byte.Parse(IP_Parts[3]) };

            // valid multicast is 224.0.0.0 to 239.255.255.255

            if ((AddressBytes[0] > 239) || (AddressBytes[0] < 224))
                return false;

            return true;
        }


       



        #region Asynchronos Message Calls
        private void ReceiveCallback(IAsyncResult ar)
        {

            UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

            IPEndPoint Origional_E = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

            try
            {
                while (!messageReceived)
                {
                    Byte[] receiveBytes = u.EndReceive(ar, ref e);
                    messageBytes = receiveBytes;
                    messageReceived_EP = e;
                    messageReceived = true;
                }
            }
            catch (Exception err) 
            {
                Console.WriteLine(err.Message);

            }
            
        }
              
        private byte[] ReceiveMessages(IPEndPoint e, UdpClient u, out IPEndPoint receivedFromEndPoint)
        {
            int waitCount = 0;
            UdpState s = new UdpState();
                      

            s.e = e;
            s.u = u;

            messageBytes = new byte[0];

            messageReceived = false;
            messageReceived_EP = null;
            u.BeginReceive(new AsyncCallback(ReceiveCallback), s);

            // Do some work while we wait for a message. For this example,
            // we'll just sleep
            while (!messageReceived)
            {
                Thread.Sleep(100);
                if (waitCount++ > (message_ReceiveTimeout / 100))
                {
                    UDP_Close();
                    throw new NetworkInterface_TimeoutException("Receive message timeout");
                    

                }

                if (Thread.CurrentThread.ThreadState == ThreadState.AbortRequested)
                {
                    UDP_Close();
                    throw new NetworkInterface_AbortRequest("Abort requested");
                }

            }
            // reset the timeout for the next message
            message_ReceiveTimeout = DEFAULT_TIMEOUT;

            receivedFromEndPoint = messageReceived_EP;
            return messageBytes;
        }

        
        private void SendCallback(IAsyncResult ar)
        {

            UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

            try
            {
                u.EndSend(ar);
                messageSent = true;
            }
            catch { }
        }

        private bool SendMessages(IPEndPoint e, UdpClient u, byte[] message)
        {
            int waitCount = 0;
            UdpState s = new UdpState();
            s.e = e;
            s.u = u;

            u.Connect(e);

            messageSent = false;
            u.BeginSend(message, message.Length, new AsyncCallback(SendCallback), s);

            while (!messageSent)
            {
                Thread.Sleep(100);
                if (waitCount++ > (message_SendTimeout / 100))
                {
                    UDP_Close();
                    throw new NetworkInterface_TimeoutException("Send message timeout");
                }

                if (Thread.CurrentThread.ThreadState == ThreadState.AbortRequested)
                {
                    UDP_Close();
                    throw new NetworkInterface_AbortRequest("Abort requested");
                }
                    
            }

            // reset the timeout for the next message
            message_SendTimeout = DEFAULT_TIMEOUT;

            return true;
        }


        /*
        private void SocketReceiveCallback(IAsyncResult ar)
        {

            Socket soc = (Socket)((SocketState)(ar.AsyncState)).s;
            IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
            EndPoint endpt = (EndPoint)remoteEp;

            try
            {
                while (!messageReceived)
                {
                    SocketReceivedCount = soc.EndReceiveFrom(ar, ref endpt);
                    messageReceived = true;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);

            }

        }

        private byte[] Socket_ReceiveMessages(SocketFlags flags, out IPEndPoint receivedFromEndPoint)
        {
            int waitCount = 0;
            SocketState ss = new SocketState();
            IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
            EndPoint endpt = (EndPoint)remoteEp;

            ss.s = theSocket;

            SocketReceivedCount = 0;
            messageBytes = new byte[0x1000];

            messageReceived = false;
            //messageReceived_EP = null;
            theSocket.BeginReceiveFrom(messageBytes, 0, messageBytes.Length, flags, ref endpt, new AsyncCallback(SocketReceiveCallback), ss);

            //u.BeginReceive(new AsyncCallback(ReceiveCallback), s);

            // Do some work while we wait for a message. For this example,
            // we'll just sleep
            while (!messageReceived)
            {
                Thread.Sleep(100);
                if (waitCount++ > (message_ReceiveTimeout / 100))
                {
                    Disconnect();
                    throw new NetworkInterface_TimeoutException("Receive message timeout");


                }

                if (Thread.CurrentThread.ThreadState == ThreadState.AbortRequested)
                {
                    Disconnect();
                    throw new NetworkInterface_AbortRequest("Abort requested");
                }

            }
            

            receivedFromEndPoint = (IPEndPoint)endpt;
            return messageBytes;
        }


        private void Socket_SendCallback(IAsyncResult ar)
        {

            Socket soc = (Socket)((SocketState)(ar.AsyncState)).s;

            try
            {
                soc.EndSend(ar);
                messageSent = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);            
            }
        }

        private void Socket_SendToCallback(IAsyncResult ar)
        {

            Socket soc = (Socket)((SocketState)(ar.AsyncState)).s;

            try
            {
                soc.EndSendTo(ar);
                messageSent = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private bool Socket_SendMessages(byte[] message, SocketFlags flags)
        {
            int waitCount = 0;
            SocketState ss = new SocketState();
            IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
            EndPoint endpt = (EndPoint)remoteEp;

            ss.s = theSocket;

            if (!theSocket.Connected && (RemoteConnection.Address != IPAddress.Any))
            {
                //set the target IP 
                IPEndPoint RemoteIPEndPoint = new IPEndPoint(RemoteConnection.Address, RemoteConnection.Port);
                EndPoint rEndPoint = (EndPoint)RemoteIPEndPoint;
                
                //do asynchronous send 
                theSocket.BeginSendTo(message, 0, message.Length, flags, rEndPoint, new AsyncCallback(Socket_SendToCallback), ss);
            } else
                theSocket.BeginSend(message, 0, message.Length, flags, new AsyncCallback(Socket_SendCallback), ss);
            
            while (!messageSent)
            {
                Thread.Sleep(100);
                if (waitCount++ > (message_SendTimeout / 100))
                {
                    Disconnect();
                    throw new NetworkInterface_TimeoutException("Send message timeout");
                }

                if (Thread.CurrentThread.ThreadState == ThreadState.AbortRequested)
                {
                    Disconnect();
                    throw new NetworkInterface_AbortRequest("Abort requested");
                }

            }

            // reset the timeout for the next message
            message_SendTimeout = DEFAULT_TIMEOUT;

            return true;
        }
*/


        #endregion
    }

    

}
