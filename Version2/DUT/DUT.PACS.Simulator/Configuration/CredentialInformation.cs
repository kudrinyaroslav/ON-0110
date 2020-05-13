using System;

namespace DUT.PACS.Simulator
{
    public class CredentialInformation1
    {
        //public CredentialInformation(string credentialToken,
        //    string credentialHolderName)
        //{
        //    this.m_credentialHolderName = credentialHolderName;
        //    this.m_credentialToken = credentialToken;
        //}

        private string m_credentialToken;
        private string m_credentialHolderName;

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

    }

}