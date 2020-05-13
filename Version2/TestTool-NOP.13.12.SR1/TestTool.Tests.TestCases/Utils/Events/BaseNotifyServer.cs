using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using TestTool.HttpTransport.Interfaces.Exceptions;
using TestTool.Tests.Definitions.Data;

namespace TestTool.Tests.TestCases.Utils
{
    /// <summary>
    /// Base class for receiving notifications
    /// </summary>
    class BaseNotifyServer
    {
        static int _listen_port_start = 8080;

        private const int _listen_port_default = 8080;
        private const int _listen_port_default_max = 10000;
        private int _listen_port = _listen_port_default;
        /// <summary>
        /// Is raised when notification is received
        /// </summary>
        private AutoResetEvent _notifyReceived = new AutoResetEvent(false);
        /// <summary>
        /// Is raised when an error occurs
        /// </summary>
        private AutoResetEvent _errorReceived = new AutoResetEvent(false);
                
        protected AutoResetEvent NotifyReceived
        {
            get { return _notifyReceived; }
        }
        protected AutoResetEvent ErrorReceived
        {
            get { return _errorReceived; }
        }

        public event Action WaitStarted;
        public event Action WaitFinished;
        public event Action Timeout;
        public event Action<byte[]> NotificationReceived;

        public class SubscribtionException: Exception
        {
            public SubscribtionException(string msg): base(msg) {}
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Nic"></param>
        public BaseNotifyServer(NetworkInterfaceDescription Nic)
        {
            _Nic = Nic;
            ChoosePort();
        }

        void ChoosePort()
        {
            //[04.06.2013] AKS: try to choose a port starting not from 8080 but from the port coming next to the one used last
            _listen_port = _listen_port_start++;
            for (int i = 0; i <= (_listen_port_default_max - _listen_port_default); i++)
            {
                try
                {
                    TcpListener l = new TcpListener(_Nic.IP, _listen_port);
                    l.Start();
                    l.Stop();
                    Thread.Sleep(300);
                    return;
                }
                catch (Exception exc)
                {};
                _listen_port_start++;
                _listen_port++;
            }

            _listen_port_start = _listen_port_default;
            //_listen_port = 12345;
            throw new SubscribtionException("Can't create subscription: can't open port to wait for notifications");
        }
        
        /// <summary>
        /// Returns listening URI for subscription
        /// </summary>
        /// <returns></returns>
        public string GetNotificationUri()
        {
            return _Nic.IP.AddressFamily == AddressFamily.InterNetworkV6 ?
                string.Format("http://[{0}]:{1}/onvif_notify_server/", GetMyIP(), _listen_port) :
                string.Format("http://{0}:{1}/onvif_notify_server/", GetMyIP(), _listen_port);
        }

        /// <summary>
        /// Currently used IP
        /// </summary>
        /// <returns></returns>
        private string GetMyIP()
        {   
            // return real listening IP
            return _Nic.IP.ToString();
        }

        /// <summary>
        /// NIC selected for testing
        /// </summary>
        NetworkInterfaceDescription _Nic;

        protected Exception _error;

        /// <summary>
        /// Handles error occurence
        /// </summary>
        /// <param name="error"></param>
        protected void OnNotifyError(Exception error)
        {
            _error = error;
            ErrorReceived.Set();
        }

        /// <summary>
        /// Raises "WaitStarted" event
        /// </summary>
        protected void RaiseWaitStarted()
        {
            if (WaitStarted != null)
            {
                WaitStarted();
            }
        }

        /// <summary>
        /// Raises "WaitFinished" event
        /// </summary>
        protected void RaiseWaitFinished()
        {
            if (WaitFinished != null)
            {
                WaitFinished();
            }
        }

        /// <summary>
        /// Raises "NotificationReceived" event (passing raw data)
        /// </summary>
        /// <param name="data"></param>
        protected void RaiseDataReceived(byte[] data)
        {
            if (NotificationReceived != null)
            {
                NotificationReceived(data);
            }
        }

        /// <summary>
        /// Raises "Timeout" event
        /// </summary>
        protected void RaiseTimeout()
        {
            if (Timeout != null)
            {
                Timeout();
            }
        }
    }
}
