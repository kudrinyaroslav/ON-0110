using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using DUT.PACS.Simulator.Events10;

namespace DUT.PACS.Simulator.Events
{
    /// <summary>
    /// List of evens that have place, but not delivered to Subscriber
    /// </summary>
    public class EventList: FilteredSubscription
    {
        #region Members

        /// <summary>
        /// List of notifications
        /// </summary>
        Queue<NotificationMessageHolderType> m_Notifications = new Queue<NotificationMessageHolderType>();

        #endregion //Members

        #region Properties
        public int Count
        {
            get { return m_Notifications.Count; }
        }
        #endregion

        #region PublicMethods



        /// <summary>
        /// Add notification to queue
        /// </summary>
        /// <param name="notificationMessage">Notification message</param>
        public void AddNotification(NotificationMessageHolderType notificationMessage)
        {
            m_Notifications.Enqueue(notificationMessage);
            NotifyAdded();
        }

        /// <summary>
        /// Get message from queue (the oldest) and delete them
        /// </summary>
        /// <returns>Notification message</returns>
        public NotificationMessageHolderType GetNotification()
        {
            return m_Notifications.Dequeue();
        }

        /// <summary>
        /// Retuns true if event list contains any message
        /// </summary>
        /// <returns></returns>
        public bool NotificationExist()
        {
            return m_Notifications.Count > 0;
        }

        public event EventHandler NewNotifications;

        void NotifyAdded()
        {
            if (NewNotifications != null)
            {
                NewNotifications(this, new EventArgs());
            }
        }

        #endregion //PublicMethods
    }
}
