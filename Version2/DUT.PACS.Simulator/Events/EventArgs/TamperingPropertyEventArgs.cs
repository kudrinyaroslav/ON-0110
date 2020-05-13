using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceAccessControl10;

namespace DUT.PACS.Simulator.Events
{
    public class TamperingPropertyEventArgs : PropertyEventArgs
    {
        public TamperingPropertyEventArgs(PACSService service, 
            DateTime utcTime, 
            string propertyOperation, 
            string accessPointToken)
            :base(utcTime, propertyOperation)
        {
            m_pacsService = service;
            m_accessPointToken = accessPointToken;
        }

        #region Members

        private PACSService m_pacsService;
        private string m_accessPointToken;

        private bool m_state;
        private string m_reason;

        #endregion

        #region Properties

        public PACSService PACSService
        {
            get { return m_pacsService; }
        }
        
        public string AccessPointToken
        {
            get { return m_accessPointToken; }
        }

        public bool Active
        {
            get { return m_state; }
            set { m_state = value;}

        }

        public string Reason
        {
            get { return m_reason; }
            set { m_reason = value;}
        }
        
        #endregion //Properties

        public override Dictionary<string, string> GetData()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("State", m_state.ToString().ToLower());
            properties.Add("Reason", m_reason);

            return properties;
        }

        public override Dictionary<string, string> GetSource()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("AccessPointToken", m_accessPointToken);
            return properties;
        }
    }
}
