using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceAccessControl10;

namespace DUT.PACS.Simulator.Events
{
    public class AccessControlCredentialExternalEventArgs : PropertyEventArgs
    {
        public AccessControlCredentialExternalEventArgs(PACSService service,
            DateTime utcTime,
            string propertyOperation,
            string accessPointToken,
            Decision decision)
            : base(utcTime, propertyOperation)
        {
            m_pacsService = service;
            m_accessPointToken = accessPointToken;
            m_decision = decision;
        }

        #region Members

        private PACSService m_pacsService;
        private string m_accessPointToken;
        private Decision m_decision;

        private string m_credentialToken;
        private string m_credentialHolderName;
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

        public Decision Decision
        {
            get { return m_decision; }
        }

        public string CredentialToken
        {
            get { return m_credentialToken; }
            set { m_credentialToken = value; }
        }

        public string CredentialHolderName
        {
            get { return m_credentialHolderName; }
            set { m_credentialHolderName = value; }
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

            properties.Add("CredentialToken", m_credentialToken);
            properties.Add("CredentialHolderName", m_credentialHolderName);
            if (Decision == Decision.Denied)
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