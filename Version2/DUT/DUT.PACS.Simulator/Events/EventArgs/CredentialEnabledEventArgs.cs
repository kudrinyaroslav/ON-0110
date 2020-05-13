using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceCredential10;

namespace DUT.PACS.Simulator.Events
{
    public class CredentiaEnabledPropertyEventArgs : PropertyEventArgs
    {
        public CredentiaEnabledPropertyEventArgs(CredentialService service,
            DateTime utcTime,
            string propertyOperation,
            string credentialToken)
            : base(utcTime, propertyOperation)
        {
            m_credentialService = service;
            m_credentialToken = credentialToken;
        }

        #region Members

        private CredentialService m_credentialService;
        private string m_credentialToken;

        private bool m_state;
        private string m_reason;

        #endregion

        #region Properties

        public CredentialService CredentialService
        {
            get { return m_credentialService; }
        }

        public string CredentialToken
        {
            get { return m_credentialToken; }
        }

        public bool Enabled
        {
            get { return m_state; }
            set { m_state = value; }

        }

        public string Reason
        {
            get { return m_reason; }
            set { m_reason = value; }
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

            properties.Add("CredentialToken", m_credentialToken);
            return properties;
        }
    }
}