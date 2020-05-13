using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DUT.PACS.Simulator.ServiceCredential10;

namespace DUT.PACS.Simulator.Events
{
    public class ConfigurationCredentialChangedEventArgs : NotPropertyEventArgs
    {
        public ConfigurationCredentialChangedEventArgs(CredentialService service,
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

        #endregion //Properties

        public override Dictionary<string, string> GetData()
        {
            return null;
        }

        public override Dictionary<string, string> GetSource()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("CredentialToken", m_credentialToken);
            return properties;
        }
    }

    public class ConfigurationCredentialRemovedEventArgs : NotPropertyEventArgs
    {
        public ConfigurationCredentialRemovedEventArgs(CredentialService service,
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

        #endregion //Properties

        public override Dictionary<string, string> GetData()
        {
            return null;
        }

        public override Dictionary<string, string> GetSource()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("CredentialToken", m_credentialToken);
            return properties;
        }
    }

    public class ConfigurationCredentialStateChangeEventArgs : NotPropertyEventArgs
    {
        public ConfigurationCredentialStateChangeEventArgs(CredentialService service,
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

        #endregion //Properties

        public override Dictionary<string, string> GetData()
        {
            return null;
        }

        public override Dictionary<string, string> GetSource()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("CredentialToken", m_credentialToken);
            return properties;
        }
    }

}
