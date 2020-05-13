using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceSchedule10;

namespace DUT.PACS.Simulator.Events
{
    public class ConfigurationScheduleChangedEventArgs : NotPropertyEventArgs
    {
        public ConfigurationScheduleChangedEventArgs(ScheduleService service,
            DateTime utcTime,
            string scheduleToken)
            : base(utcTime)
        {
            m_scheduleService = service;
            m_scheduleToken = scheduleToken;
        }

        #region Members

        private ScheduleService m_scheduleService;
        private string m_scheduleToken;

        #endregion

        #region Properties

        public ScheduleService ScheduleService
        {
            get { return m_scheduleService; }
        }

        public string ScheduleToken
        {
            get { return m_scheduleToken; }
        }

        #endregion //Properties

        public override Dictionary<string, string> GetData()
        {
            return null;
        }

        public override Dictionary<string, string> GetSource()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("ScheduleToken", m_scheduleToken);
            return properties;
        }
    }
}