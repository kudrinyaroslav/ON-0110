using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace DUT.PACS.Simulator.ExternalLogging
{
    /// <summary>
    /// Base subscription holder as it's needed for BaseLoggingService (the only operation 
    /// it needs is closing channel)
    /// </summary>
    public abstract class BaseSubscriptionHolder
    {
        /// <summary>
        /// Channel
        /// </summary>
        public abstract ICommunicationObject Channel { get; }
    }

    /// <summary>
    /// Template for services which hold a queue of notiifcations and send them to subscribers. 
    /// </summary>
    /// <typeparam name="TSubscriptionHolder">Subscription holder type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    public abstract class BaseLoggingService<TSubscriptionHolder, TRequest> : IDisposable
        where TSubscriptionHolder: BaseSubscriptionHolder
    {
                
        /// <summary>
        /// Subscribers
        /// </summary>
        private Dictionary<Guid, TSubscriptionHolder> _subscribers;
        /// <summary>
        /// Messages to send
        /// </summary>
        Queue<TRequest> _requests = new Queue<TRequest>();
        /// <summary>
        /// New clients to send welcome message, if supported 
        /// </summary>
        Queue<TSubscriptionHolder> _newClients = new Queue<TSubscriptionHolder>();
        
        /// <summary>
        /// Set when new message appears
        /// </summary>
        AutoResetEvent _messagesAppeared = new AutoResetEvent(false);
        /// <summary>
        /// Set when exit is requested
        /// </summary>
        AutoResetEvent _stopEvent = new AutoResetEvent(false);
        /// <summary>
        /// Set when new subscription is added.
        /// </summary>
        AutoResetEvent _newSubscriberEvent = new AutoResetEvent(false);
        
        /// <summary>
        /// Queue guard
        /// </summary>
        object _lock = new object();

        /// <summary>
        /// Subscribers list.
        /// </summary>
        protected Dictionary<Guid, TSubscriptionHolder> Subscribers
        {
            get { return _subscribers; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseLoggingService()
        {
            _subscribers = new Dictionary<Guid, TSubscriptionHolder>();

            System.Threading.Thread workingThread = new Thread(WorkingThread);
            workingThread.Start();
        }

        /// <summary>
        /// Adds subscription
        /// </summary>
        /// <param name="subscriptionHolder">Subscription holder</param>
        /// <returns>Subscription ID for further operations</returns>
        public Guid Subscribe(TSubscriptionHolder subscriptionHolder)
        {
            Guid guid = Guid.NewGuid();

            subscriptionHolder.Channel.Closed += new EventHandler(
            (sender, e) =>
                RemoveSubscriber(guid));

            lock (_lock)
            {
                _subscribers.Add(guid, subscriptionHolder);
                _newClients.Enqueue(subscriptionHolder);
            }
            _newSubscriberEvent.Set();

            return guid;
        }

        /// <summary>
        /// Removes subscriber
        /// </summary>
        /// <param name="guid"></param>
        public void Unsubscribe(Guid guid)
        {
            if (_subscribers.ContainsKey(guid))
            {
                ICommunicationObject client = _subscribers[guid].Channel;
                client.Close();
                RemoveSubscriber(guid);
            }
        }

        /// <summary>
        /// Adds request to queue.
        /// </summary>
        public void SendMessage(TRequest request)
        {
            lock (_lock)
            {
                _requests.Enqueue(request);
            }
            _messagesAppeared.Set();
        }
        
        /// <summary>
        /// Stops working
        /// </summary>
        public void Stop()
        {
            _stopEvent.Set();
        }        
        
        /// <summary>
        /// Working thread function
        /// </summary>
        void WorkingThread()
        {
            while (true)
            {
                string hashCode = System.Threading.Thread.CurrentThread.GetHashCode().ToString();
                string lockHashCode = typeof (TRequest).Name;

                int handle = WaitHandle.WaitAny(new WaitHandle[] {_stopEvent, _messagesAppeared, _newSubscriberEvent});
                if (handle == 0)
                {
                    break;
                }
                switch (handle)
                {
                    case 1:
                        {
                            Queue<TRequest> queueCopy = new Queue<TRequest>();
                            lock (_lock)
                            {
                                while (_requests.Count > 0)
                                {
                                    TRequest request = _requests.Dequeue();
                                    queueCopy.Enqueue(request);
                                }
                            }
                            while (queueCopy.Count > 0)
                            {
                                TRequest request = queueCopy.Dequeue();
                                SendNotifications(request);
                            }
                        }
                        break;
                    case 2:
                        {
                            Queue<TSubscriptionHolder> queueCopy = new Queue<TSubscriptionHolder>();

                            lock (_lock)
                            {
                                while (_newClients.Count > 0)
                                {
                                    TSubscriptionHolder client = _newClients.Dequeue();
                                    queueCopy.Enqueue(client);
                                }
                            }
                            while(queueCopy.Count > 0)
                            {
                                TSubscriptionHolder client = queueCopy.Dequeue();
                                SendWelcomeMessage(client);
                            }
                        }
                        break;
                }
            }
            CloseConnections();
        }


        /// <summary>
        /// Removes subscriber
        /// </summary>
        /// <param name="guid">Subscriber ID</param>
        void RemoveSubscriber(Guid guid)
        {
            lock (_lock)
            {
                _subscribers.Remove(guid);
            }
        }
        
        /// <summary>
        /// Closes all connections.
        /// </summary>
        void CloseConnections()
        {
            List<TSubscriptionHolder> listeners = new List<TSubscriptionHolder>(_subscribers.Values);
            foreach (TSubscriptionHolder listener in listeners)
            {
                try
                {
                    CloseConnection(listener);
                    listener.Channel.Close();
                }
                catch (Exception exc)
                {

                }
            }
            _subscribers.Clear();
        }



        #region implementation specific

        /// <summary>
        /// Sends notification to all subscribers
        /// </summary>
        /// <param name="request">Request to send</param>
        protected virtual void SendNotifications(TRequest request)
        {

        }

        /// <summary>
        /// Sends welcome message to subscribers
        /// </summary>
        /// <param name="subscriptionHolder">Subscriber to greate</param>
        protected virtual void SendWelcomeMessage(TSubscriptionHolder subscriptionHolder)
        {

        }

        /// <summary>
        /// Closes all connections, if specific actions are required.
        /// </summary>
        /// <param name="holder">Subscription holder to be closed.</param>
        protected virtual void CloseConnection(TSubscriptionHolder holder)
        {

        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            CloseConnections();
        }


        #endregion
    }
}
