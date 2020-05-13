using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMC.Events
{
    /// <summary>
    /// Events receiver
    /// </summary>
    [System.ServiceModel.ServiceBehavior(InstanceContextMode = System.ServiceModel.InstanceContextMode.Single)]
    class EventsReceiver : Proxies.Events.NotificationConsumer
    {
        public event Action<EventsReceiver, Proxies.Events.Notify1> NotificationReceived;

        #region NotificationConsumer Members

        public string Name { get; set; }
        public void Notify(Proxies.Events.Notify1 request)
        {
            if (NotificationReceived != null)
            {
                NotificationReceived(this, request);
            }
        }

        #endregion
    }
}
