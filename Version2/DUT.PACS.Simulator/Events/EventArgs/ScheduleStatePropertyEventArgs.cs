using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceSchedule10;
using System.Xml;

namespace DUT.PACS.Simulator.Events
{
    /// <summary>
    /// Event arguments for Door Mode Property Evens
    /// </summary>
    public class ScheduleStateActivePropertyEventArgs : PropertyEventArgs
    {
        #region Memebers

        private string m_scheduleToken;
        private string m_name;
        private bool m_active;
        private bool m_specialDay;



        #endregion //Memebers

        #region Constructor

        /// <summary>
        /// Constructor for ScheduleStateActivePropertyEventArgs
        /// </summary>
        /// <param name="doorControlService">Door Control Service object</param>
        /// <param name="utcTime">UTC time of event</param>
        /// <param name="propertyOperation">Property operation</param>
        /// <param name="doorToken">Door Token</param>
        /// <param name="currentState">Current Door Mode state</param>
        public ScheduleStateActivePropertyEventArgs(ScheduleService scheduleService, DateTime utcTime, string propertyOperation, string scheduleToken, string name, bool active, bool specialDay)
            :base(utcTime, propertyOperation)
        {
            m_scheduleToken = scheduleToken;
            m_name = name;
            m_active = active;
            m_specialDay = specialDay;
        }

        #endregion //Constructor

        #region Properties
        
        public string ScheduleToken
        {
            get { return m_scheduleToken; }
        }

        public string Name
        {
            get { return m_name; }
        }

        public bool Active
        {
            get { return m_active; }
        }

        public bool SpecialDay
        {
            get { return m_specialDay; }
        }

        #endregion //Properties

        public override Dictionary<string, string> GetData()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("Active", XmlConvert.ToString(Active));
            properties.Add("SpecialDay", XmlConvert.ToString(SpecialDay));

            return properties;
        }

        public override Dictionary<string, string> GetSource()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("ScheduleToken", ScheduleToken);
            properties.Add("Name", Name);

            return properties;
        }

    }
}
