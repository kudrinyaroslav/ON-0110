using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SMC.SmcData;

namespace SMC.Pages
{
    public partial class GeneralSettings : BaseSmcControl
    {
        public GeneralSettings()
        {
            InitializeComponent();

            tbServiceAddress.Text = System.Configuration.ConfigurationManager.AppSettings["DoorControlService"];
            tbEventsControl.Text = System.Configuration.ConfigurationManager.AppSettings["EventControlService"];
            tbLogger.Text = System.Configuration.ConfigurationManager.AppSettings["LoggingService"];
            tbConfiguration.Text = System.Configuration.ConfigurationManager.AppSettings["ConfigurationService"];
            tbPacsService.Text = System.Configuration.ConfigurationManager.AppSettings["PacsService"];
            tbEventsService.Text = System.Configuration.ConfigurationManager.AppSettings["EventsService"];
            tbSensorService.Text = System.Configuration.ConfigurationManager.AppSettings["SensorService"];
            tbMonitorService.Text = System.Configuration.ConfigurationManager.AppSettings["MonitorService"];
            tbCredentialService.Text = System.Configuration.ConfigurationManager.AppSettings["CredentialService"];
            tbAccessRulesService.Text = System.Configuration.ConfigurationManager.AppSettings["AccessRulesService"];
            tbScheduleService.Text = System.Configuration.ConfigurationManager.AppSettings["ScheduleService"];
        }

        private void GeneralSettings_Load(object sender, EventArgs e)
        {

        }
        
        private void GeneralSettings_Leave(object sender, EventArgs e)
        {
        }

        public void UpdateContext()
        {
            Context.Instance.General.ManagementServiceAddress = tbServiceAddress.Text;
            Context.Instance.General.EventControlServiceAddress = tbEventsControl.Text;
            Context.Instance.General.LoggingServiceAddress= tbLogger.Text;
            Context.Instance.General.ConfigurationServiceAddress = tbConfiguration.Text;
            Context.Instance.General.PACSServiceAddress = tbPacsService.Text;
            Context.Instance.General.EventsServiceAddress = tbEventsService.Text;
            Context.Instance.General.SensorServiceAddress = tbSensorService.Text;
            Context.Instance.General.MonitorServiceAddress = tbMonitorService.Text;
            Context.Instance.General.AccessRulesServiceAddress = tbAccessRulesService.Text;
            Context.Instance.General.CredentialServiceAddress = tbCredentialService.Text;
            Context.Instance.General.ScheduleServiceAddress = tbScheduleService.Text;

            Context.Instance.General.NotifyModified();
        }

    }
}
