using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceCredential10;

namespace DUT.PACS.Simulator.Events
{
    public class CredentiaResetAntipassbackViolationPropertyEventArgs : NotPropertyEventArgs
    {
        public CredentiaResetAntipassbackViolationPropertyEventArgs(CredentialService service,
            DateTime utcTime,            
            string credentialToken)
            : base(utcTime)
        {
            m_credentialService = service;
            m_credentialToken = credentialToken;
        }

        #region Members

        private CredentialService m_credentialService;
        private string m_credentialToken;

        private bool m_apbViolation;
        private bool m_clientUpdate;

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

        public bool ApbViolation
        {
            get { return m_apbViolation; }
            set { m_apbViolation = value; }
        }

        public bool ClientUpdated
        {
            get { return m_clientUpdate; }
            set { m_clientUpdate = value; }
        }

        #endregion //Properties

        public override Dictionary<string, string> GetData()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("ApbViolation", m_apbViolation.ToString().ToLower());
            properties.Add("ClientUpdated", m_clientUpdate.ToString().ToLower());

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