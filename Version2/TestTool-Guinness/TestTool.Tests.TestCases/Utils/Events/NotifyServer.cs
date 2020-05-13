///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Event;
using TestTool.Tests.Common.Discovery;
using TestTool.Tests.Common.Soap;
using TestTool.Tests.Definitions.Data;
using Server = TestTool.Tests.Common.NotificationConsumer;
using System.Threading;
using System.Net.Sockets;

namespace TestTool.Tests.TestCases.Utils.Events
{
    class NotifyServer
    {
        private const int _listen_port = 8080;
        private AutoResetEvent _notifyReceived = new AutoResetEvent(false);
        private AutoResetEvent _errorReceived = new AutoResetEvent(false);
        
        private Notify _notify;
        private Exception _error;

        // should use this NIC to attach to
        public NotifyServer(NetworkInterfaceDescription Nic)
        {
            _Nic = Nic;
        }
        public string GetNotificationUri()
        {
            return _Nic.IP.AddressFamily == AddressFamily.InterNetworkV6 ? 
                string.Format("http://[{0}]:{1}/onvif_notify_server/", GetMyIP(), _listen_port) :
                string.Format("http://{0}:{1}/onvif_notify_server/", GetMyIP(), _listen_port);
        }

        public event Action WaitStarted;
        public event Action WaitFinished;
        public event Action Timeout;

        public event Action NotificationReceived;

        private byte[] _rawData;

        public byte[] RawData
        {
            get { return _rawData; }
        }

        public Notify WaitForNotify(Action action, int timeout, WaitHandle stop)
        {
            Notify notify = null;
            Server.NotificationConsumer consumer = new Server.NotificationConsumer(GetNotificationUri());
            _notifyReceived.Reset();
            _errorReceived.Reset();
            consumer.OnError +=
                new Action<Exception>(
                    (err) =>
                        {
                            _rawData = consumer.RawData;
                            NotifyDataReceived();
                            OnNotifyError(err);
                        }
                    );
           
            consumer.OnNotify += OnNotify;

            try
            {
                consumer.Start();
                if(action != null)
                {
                    action();
                }
                if (WaitStarted != null)
                {
                    WaitStarted();
                }
                int res = WaitHandle.WaitAny(new WaitHandle[] { _notifyReceived, _errorReceived, stop }, timeout);
                if(res == 0)
                {
                    notify = _notify;
                    _rawData = consumer.RawData;
                    NotifyDataReceived();
                    if (WaitFinished != null)
                    {
                        WaitFinished();
                    }
                }
                else if(res == 1)
                {
                    consumer.Stop();
                    throw _error;
                }
                else if (res == 2)
                {
                    // stop event
                }
                else
                {
                    if (Timeout != null)
                    {
                        Timeout();
                    }
                }
            }
            finally
            {
                consumer.Stop();
            }
            return notify;
        }
        protected void OnNotifyError(Exception error)
        {
            _error = error;
            _errorReceived.Set();
        }
        protected void OnNotify(SoapMessage<Notify> notify)
        {
            if (notify != null)
            {
                _notify = notify.Object;
            }
            _notifyReceived.Set();
        }

        void NotifyDataReceived()
        {
            if (NotificationReceived != null)
            {
                NotificationReceived();
            }
        }

        private string GetMyIP()
        {   // should return real listening IP
            //return "127.0.0.1";
            return  _Nic.IP.ToString();
        }
        NetworkInterfaceDescription _Nic;
    }
}
