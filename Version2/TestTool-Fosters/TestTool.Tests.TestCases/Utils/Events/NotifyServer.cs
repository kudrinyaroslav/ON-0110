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
using Events = TestTool.Tests.Common.NotificationConsumer;
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
        
        protected byte[] _rawData;

        public byte[] RawData
        {
            get { return _rawData; }
        }

        public Notify WaitForNotify(Action action, int timeout, WaitHandle stop)
        {
            Notify notify = null;
            Events.NotificationConsumer consumer = new Events.NotificationConsumer(GetNotificationUri());
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
            }
            NotifyReceived.Set();
        }
        


    }
}
