using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUT.PACS.Simulator.Events
{
    public class PropertyEventArgs : EventArgs
    {
        #region members

        private DateTime m_utcTime;
        private string m_propertyOperation;

        #endregion
        

        public PropertyEventArgs(DateTime utcTime, 
            string propertyOperation
)
        {
            m_utcTime = utcTime;
            m_propertyOperation = propertyOperation;

        }

        #region Properties

        public string PropertyOperation
        {
            get { return m_propertyOperation; }
        }

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
