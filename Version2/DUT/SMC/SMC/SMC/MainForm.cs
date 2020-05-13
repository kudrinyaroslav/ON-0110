using System;
using System.Windows.Forms;

namespace SMC
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            doorsManagement.Enabled = !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["DoorControlService"]);
            accessRulesManagement.Enabled = !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AccessRulesService"]);
            credentialManagement.Enabled = !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["CredentialService"]);
            scheduleManagement.Enabled = !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["ScheduleService"]);
            eventsManagement.Enabled = !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["EventControlService"]);
            loggingPage.Enabled = !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LoggingService"]);

        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            generalSettings.UpdateContext();
            doorsManagement.Enabled = !string.IsNullOrEmpty(SmcData.Context.Instance.General.ManagementServiceAddress);
            accessRulesManagement.Enabled = !string.IsNullOrEmpty(SmcData.Context.Instance.General.AccessRulesServiceAddress);
            credentialManagement.Enabled = !string.IsNullOrEmpty(SmcData.Context.Instance.General.CredentialServiceAddress);
            scheduleManagement.Enabled = !string.IsNullOrEmpty(SmcData.Context.Instance.General.ScheduleServiceAddress);
            eventsManagement.Enabled = !string.IsNullOrEmpty(SmcData.Context.Instance.General.EventControlServiceAddress);
            loggingPage.Enabled = !string.IsNullOrEmpty(SmcData.Context.Instance.General.LoggingServiceAddress);
        }
    }
}
