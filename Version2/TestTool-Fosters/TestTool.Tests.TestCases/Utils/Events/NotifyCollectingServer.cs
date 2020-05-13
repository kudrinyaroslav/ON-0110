using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TestTool.Proxies.Event;
using Events = TestTool.Tests.Common.NotificationConsumer;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Common.Soap;

namespace TestTool.Tests.TestCases.Utils
{
    class NotifyCollectingServer : BaseNotifyServer
    {
        // should use this NIC to attach to
        public NotifyCollectingServer(NetworkInterfaceDescription Nic)
            : base(Nic)
        {
        }

        class NotificationInfo
        {
            public Notify Notify;
            public byte[] RawData;
        }

        Queue<NotificationInfo> _notifications;
        object _notificationsLock = new object();

        public Dictionary<Notify, byte[]> CollectNotifications(Action action,
            int timeout,
            int limit,
            WaitHandle startProcessing,
            WaitHandle stop)
        {
            return CollectNotifications(action, timeout, limit, null, startProcessing, stop);
        }

        /// <summary>
        /// Blocks calling thread and collects notifications. Notifications collecting will continue until
        /// - count of messages achieves <paramref name="limit"/>. If <paramref name="messageCheck"/> parameter
        /// is specified, messages will be checked before adding to the list.
        /// </summary>
        /// <param name="action">Action which should trigger notification sending</param>
        /// <param name="timeout">Timeout for waiting</param>
        /// <param name="limit">Limit of message to receive</param>
        /// <param name="messageCheck">Filter procedure</param>
        /// <param name="stop">Semaphore object</param>
        /// <returns></returns>
        public Dictionary<Notify, byte[]> CollectNotifications(Action action,
            int timeout, /* Milliseconds! */
            int limit,
            Func<NotificationMessageHolderType, bool> messageCheck,
            WaitHandle startProcessing,
            WaitHandle stop)
        {
            Dictionary<Notify, byte[]> notifications = new Dictionary<Notify,byte[]>();
            _notifications = new Queue<NotificationInfo>();

            Events.NotificationConsumer consumer = new Events.NotificationConsumer(GetNotificationUri());

            Action<Exception> errorHandling =
                new Action<Exception>(
                    (err) =>
                    {
                        RaiseDataReceived(consumer.RawData);
                        OnNotifyError(err);
                    }
                    );

            consumer.OnError += errorHandling;
            consumer.OnNotify += OnNotify;

            NotifyReceived.Reset();
            ErrorReceived.Reset();

            bool exitByStop = false;

            try
            {
                consumer.Start();
                if (action != null)
                {
                    action();
                }
                RaiseWaitStarted();

                int hndl = WaitHandle.WaitAny(new WaitHandle[] { startProcessing }, timeout);
                if (hndl == WaitHandle.WaitTimeout)
                {
                    return notifications;
                }

                int total = 0;

                while (true)
                {
                    int res = WaitHandle.WaitAny(new WaitHandle[] { NotifyReceived, ErrorReceived, stop }, timeout);
                    if (res == 0)
                    {
                        while (_notifications.Count > 0)
                        {
                            NotificationInfo info = _notifications.Dequeue();
                            Notify notify = info.Notify;
                            System.Diagnostics.Debug.WriteLine("Process Notify: " + notify.GetHashCode());

                            RaiseDataReceived(info.RawData);

                            bool add = false;
                            if (notify.NotificationMessage != null)
                            {
                                if (messageCheck == null)
                                {
                                    add = true;
                                    total += notify.NotificationMessage.Length;
                                }
                                else
                                {
                                    foreach (NotificationMessageHolderType message in notify.NotificationMessage)
                                    {
                                        System.Diagnostics.Debug.WriteLine("Check if message passes the test");
                                        if (messageCheck(message))
                                        {
                                            System.Diagnostics.Debug.WriteLine("PASSED!");
                                            add = true;
                                            total += 1;
                                        }
                                        else
                                        {
                                            System.Diagnostics.Debug.WriteLine("Message filtered out!");
                                        }
                                    }
                                }
                            }
                            if (add)
                            {
                                try
                                {
                                    System.Diagnostics.Debug.WriteLine("Add Notify object: " + notify.GetHashCode());
                                    notifications.Add(notify, info.RawData);
                                }
                                catch (Exception exc)
                                {
                                    System.Diagnostics.Debug.WriteLine("Error when adding notify [" + notify.GetHashCode() + "]");
                                    throw exc;
                                }
                            }

                            if (total >= limit)
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("All {0} messages received", limit));
                                break;
                            }
                        } // while notifications.Count > 0
                        if (total >= limit)
                        {
                            break; // exit from the second cycle
                        }
                    }
                    else if (res == 1)
                    {
                        // error - stop
                        consumer.Stop();
                        break;
                    }
                    else if (res == 2)
                    {
                        // stop event
                        exitByStop = true;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            finally
            {
                consumer.OnError -= errorHandling;
                consumer.OnNotify -= OnNotify;

                consumer.Stop();
            }

            if (exitByStop)
            {
                throw new TestTool.Tests.Definitions.Exceptions.StopEventException();
            }

            return notifications;
        }


        protected void OnNotify(SoapMessage<Notify> notify, byte[] rawData)
        {
            if (notify != null)
            {
                System.Diagnostics.Debug.WriteLine("Notify received: " + notify.Object.GetHashCode());
                Notify newNotify = notify.Object;
                NotificationInfo info = new NotificationInfo() { Notify = newNotify, RawData = rawData };
                lock (_notificationsLock)
                {
                    _notifications.Enqueue(info);
                }
            }
            NotifyReceived.Set();
        }
    }
}
