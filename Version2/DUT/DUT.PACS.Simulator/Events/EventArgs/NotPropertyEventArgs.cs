using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUT.PACS.Simulator.Events
{
    public class NotPropertyEventArgs : EventArgs
    {
        #region members

        private DateTime m_utcTime;

        #endregion
        

        public NotPropertyEventArgs(DateTime utcTime)
        {
            m_utcTime = utcTime;
        }

        #region Properties

        public DateTime UtcTime
        {
            get { return m_utcTime; }
        }

        #endregion


        public virtual Dictionary<string, string> GetData()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            return properties;
        }

        public virtual Dictionary<string, string> GetSource()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            return properties;
        }
    }
}
