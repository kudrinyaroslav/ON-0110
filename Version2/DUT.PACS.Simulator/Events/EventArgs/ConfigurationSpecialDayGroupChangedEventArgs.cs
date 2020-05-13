using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceSchedule10;

namespace DUT.PACS.Simulator.Events
{
    public class ConfigurationSpecialDayGroupChangedEventArgs : NotPropertyEventArgs
    {
        public ConfigurationSpecialDayGroupChangedEventArgs(ScheduleService service,
            DateTime utcTime,
            string specialDayGroupToken)
            : base(utcTime)
        {
            m_scheduleService = service;
            m_specialDayGroupToken = specialDayGroupToken;
        }

        #region Members

        private ScheduleService m_scheduleService;
        private string m_specialDayGroupToken;

        #endregion

        #region Properties

        public ScheduleService ScheduleService
        {
            get { return m_scheduleService; }
        }

        public string SpecialDayGroupToken
        {
            get { return m_specialDayGroupToken; }
        }

        #endregion //Properties

        public override Dictionary<string, string> GetData()
        {
            return null;
        }

        public override Dictionary<string, string> GetSource()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("SpecialDaysToken", m_specialDayGroupToken);
            return properties;
        }
    }
}