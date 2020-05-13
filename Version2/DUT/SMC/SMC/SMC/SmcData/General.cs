using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMC.SmcData
{
    class General
    {
        public string ManagementServiceAddress { get; set; }
        public string EventControlServiceAddress { get; set; }
        public string LoggingServiceAddress { get; set; }
        public string ConfigurationServiceAddress { get; set; }
        public string PACSServiceAddress { get; set; }
        public string AccessRulesServiceAddress { get; set; }
        public string CredentialServiceAddress { get; set; }
        public string SensorServiceAddress { get; set; }
        public string EventsServiceAddress { get; set; }
        public string MonitorServiceAddress { get; set; }
        public string ScheduleServiceAddress { get; set; }

        public event EventHandler Modified; 
        public void NotifyModified()
        {
            if (Modified != null)
            {
                Modified(this, new EventArgs());
            }
        }
    }
}
