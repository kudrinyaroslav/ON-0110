using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.BackDoorServices;
using DUT.PACS.Simulator.ServiceAccessControl10;

namespace DUT.PACS.Simulator.Events
{
    public class RequestTimeoutEventArgs : PropertyEventArgs
    {
        public RequestTimeoutEventArgs(EventControlService service,
            DateTime utcTime,
            string propertyOperation,
            string accessPointToken,
            Requester requester)
            : base(utcTime, propertyOperation)
        {
            m_eventControlService = service;
            m_accessPointToken = accessPointToken;
            m_Requester = requester;
        }

        #region Members

        private EventControlService m_eventControlService;
        private string m_accessPointToken;
        private Requester m_Requester;

        private string m_credentialToken;

        #endregion

        #region Properties

        public EventControlService eventControlService
        {
            get { return m_eventControlService; }
        }

        public string AccessPointToken
        {
            get { return m_accessPointToken; }
        }

        public Requester Requester
        {
            get { return m_Requester; }
        }

        public string CredentialToken
        {
            get { return m_credentialToken; }
            set { m_credentialToken = value; }
        }


        #endregion //Properties

        public override Dictionary<string, string> GetData()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            if (Requester == Requester.Credential)
                properties.Add("CredentialToken", m_credentialToken);

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