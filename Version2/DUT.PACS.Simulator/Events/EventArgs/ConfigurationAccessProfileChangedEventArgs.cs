using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DUT.PACS.Simulator.ServiceAccessRules10;

namespace DUT.PACS.Simulator.Events
{
    public class ConfigurationAccessProfileChangedEventArgs : NotPropertyEventArgs
    {
        public ConfigurationAccessProfileChangedEventArgs(AccessRulesService service,
            DateTime utcTime,
            string accessProfileToken)
            : base(utcTime)
        {
            m_accessRulesService = service;
            m_accessProfileToken = accessProfileToken;
        }

        #region Members

        private AccessRulesService m_accessRulesService;
        private string m_accessProfileToken;

        #endregion

        #region Properties

        public AccessRulesService AccessRulesService
        {
            get { return m_accessRulesService; }
        }

        public string AccessProfileToken
        {
            get { return m_accessProfileToken; }
        }

        #endregion //Properties

        public override Dictionary<string, string> GetData()
        {
            return null;
        }

        public override Dictionary<string, string> GetSource()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("AccessProfileToken", m_accessProfileToken);
            return properties;
        }
    }
}
