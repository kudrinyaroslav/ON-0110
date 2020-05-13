using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Data;
using TestTool.Proxies.Event;
using System.Threading;
using Events = TestTool.Tests.Common.NotificationConsumer;
using TestTool.Tests.Common.Soap;
using System.Xml;

namespace TestTool.Tests.TestCases.Utils
{
    /// <summary>
    /// Class for receiving notifications in asynchronous mode
    /// </summary>
    class NotifyAsyncServer : BaseNotifyServer
    {
        public NotifyAsyncServer(NetworkInterfaceDescription Nic)
            :base(Nic)
        {

        }

        Dictionary<Notify, byte[]> _notifications;
        object _notificationsLock = new object();
        private AutoResetEvent _collectingStopped = new AutoResetEvent(false);

        Events.NotificationConsumer _consumer;

        public void StartCollecting(WaitHandle stop)
        {
            lock (_notificationsLock)
            {
                _notifications = new Dictionary<Notify, byte[]>();
            }

            _consumer = new Events.NotificationConsumer(GetNotificationUri());

            _consumer.OnError +=
                new Action<Exception>(
                    (err) =>
                    {
                        RaiseDataReceived(_consumer.RawData);
                        OnNotifyError(err);
                    }
                    );

            _consumer.OnNotify += 
                new Action<SoapMessage<Notify>, byte[]>( (n, raw) => AddMessage(n, raw)); 

            NotifyReceived.Reset();
            ErrorReceived.Reset();

            try
            {
                _consumer.Start();
                RaiseWaitStarted();            
            }
            finally
            {
            }

            if (_error != null)
            {
                throw _error;
            }
        }

        void AddMessage(SoapMessage<Notify> notify, byte[] rawData)
        {
            System.Diagnostics.Debug.WriteLine("Notification received!");
            if (notify != null)
            {
                Notify newNotify = notify.Object;
                lock (_notificationsLock)
                {
                    _notifications.Add(newNotify, rawData);
                }
            }
            NotifyReceived.Set();
            RaiseDataReceived(rawData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout">Timeout for single "WaitAny" operation</param>
        /// <param name="stopHandle"></param>
        /// <returns></returns>
        public Dictionary<Notify, byte[]> Peek(int timeout, WaitHandle stopHandle)
        {
            System.Diagnostics.Debug.WriteLine("NotifyAsyncServer::Peek");

            Dictionary<Notify, byte[]> result = new Dictionary<Notify, byte[]>();
            lock (_notificationsLock)
            {
                if (_notifications.Count > 0)
                {
                    MoveNotifications(result);
                    System.Diagnostics.Debug.WriteLine(string.Format("Return immediately {0} notifications", result.Count));
                    return result;
                }               
            }

            WaitHandle[] handles = new WaitHandle[] { NotifyReceived, ErrorReceived, stopHandle };
            int res = WaitHandle.WaitAny(handles, timeout);

            try
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Result of WaitAny: {0}", res));
                if (res == 0)
                {
                    System.Diagnostics.Debug.WriteLine("   Return notifications list");
                    MoveNotifications(result);
                }
                else if (res == 1)
                {
                    System.Diagnostics.Debug.WriteLine("   Throw error");
                    throw _error;
                }
                else if (res == 2)
                {
                    System.Diagnostics.Debug.WriteLine("   Stop event - throw exception");
                    throw new Definitions.Exceptions.StopEventException();
                    // stop event
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("   Timeout");
                    RaiseTimeout();
                }
            }
            catch(Exception)
            {                   
                _consumer.Stop();
                throw;
            }
            return result;
        }

        public Dictionary<Notify, byte[]> Get()
        {
            return _notifications;
        }

        void MoveNotifications(Dictionary<Notify, byte[]> target)
        {
            foreach (Notify notify in _notifications.Keys)
            {
                target.Add(notify, _notifications[notify]);
            }
            lock (_notificationsLock)
            {
                _notifications.Clear();
            }        
        }

        public void StopCollecting()
        {
            _collectingStopped.Set();
            _consumer.Stop();      
        }
        
        public void ResetNotifications()
        {
            lock (_notificationsLock)
            {
                _notifications.Clear();
            }
        }

    }
}
