///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TestTool.Proxies.Event;
using TestTool.Tests.Common.Discovery;
using TestTool.Tests.Common.Soap;
using TestTool.Tests.Definitions.Data;
using Events_ = TestTool.Tests.Common.NotificationConsumer;
using System.Threading;
using System.Net.Sockets;
using TestTool.Tests.Definitions.Exceptions;

namespace TestTool.Tests.TestCases.Utils
{
    class NotifyServer : BaseNotifyServer
    {
        protected Notify _notify;
        
        // should use this NIC to attach to
        public NotifyServer(NetworkInterfaceDescription Nic)
            :base(Nic)
        {

        }

        public List<XmlElement> SoapHeaders { get; protected set; }
        protected byte[] _rawData;

        public byte[] RawData
        {
            get { return _rawData; }
        }

        public Notify WaitForNotify(Action action, int timeout, WaitHandle stop)
        {
            Notify notify = null;
            Events_.NotificationConsumer consumer = new Events_.NotificationConsumer(GetNotificationUri());
            NotifyReceived.Reset();
            ErrorReceived.Reset();
            consumer.OnError +=
                new Action<Exception>(
                    (err) =>
                        {
                            _rawData = consumer.RawData;
                            RaiseDataReceived(consumer.RawData);
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
                RaiseWaitStarted();

                int res = WaitHandle.WaitAny(new WaitHandle[] { NotifyReceived, ErrorReceived, stop }, timeout);
                if(res == 0)
                {
                    notify = _notify;                    
                    RaiseDataReceived(_rawData);
                    RaiseWaitFinished();
                }
                else if(res == 1)
                {
                    consumer.Stop();
                    throw _error;
                }
                else if (res == 2)
                {
                    // stop event
                    throw new StopEventException();
                }
                else
                {
                    RaiseTimeout();
                }
            }
            finally
            {
                consumer.Stop();
            }
            return notify;
        }

        protected void OnNotify(SoapMessage<Notify> notify, byte[] rawData)
        {
            if (notify != null)
            {
                System.Diagnostics.Debug.WriteLine("Notify received: " + notify.Object.GetHashCode());
                _notify = notify.Object;
                _rawData = rawData;
                SoapHeaders = notify.Header;
            }
            NotifyReceived.Set();
        }

        // second interface
        Events_.NotificationConsumer _consumer = null;
        public void StartNotify()
        {
            _consumer = new Events_.NotificationConsumer(GetNotificationUri());
            NotifyReceived.Reset();
            ErrorReceived.Reset();
            _consumer.OnError +=
                new Action<Exception>(
                    (err) =>
                    {
                        _rawData = _consumer.RawData;
                        RaiseDataReceived(_consumer.RawData);
                        OnNotifyError(err);
                    }
                    );

            _consumer.OnNotify += OnNotify;
            _consumer.Start();
        }
        public Notify WaitForNotifyOnly(Action action, int timeout, WaitHandle stop)
        {
            Notify notify = null;
            if (_consumer == null)
            {
                StartNotify();
            }
            //try
            {
                if (action != null)
                {
                    action();
                }
                RaiseWaitStarted();

                int res = WaitHandle.WaitAny(new WaitHandle[] { NotifyReceived, ErrorReceived, stop }, timeout);
                if (res == 0)
                {
                    notify = _notify;
                    RaiseDataReceived(_rawData);
                    RaiseWaitFinished();
                }
                else if (res == 1)
                {
                    //consumer.Stop();
                    throw _error;
                }
                else if (res == 2)
                {
                    // stop event
                    throw new StopEventException();
                }
                else
                {
                    RaiseTimeout();
                }
            }
            //finally
            {
                //consumer.Stop();
            }
            return notify;
        }
        public void StopNotify()
        {
            _consumer.Stop();
        }

    }
}
