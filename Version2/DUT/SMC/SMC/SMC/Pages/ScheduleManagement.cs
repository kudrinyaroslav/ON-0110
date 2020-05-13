using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel.Channels;
using SMC.Controls;
using SMC.Proxies;
using SMC.Proxies.Monitoring;
using SMC.StateMonitoring;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace SMC.Pages
{
    public partial class ScheduleManagement : BaseSmcControl
    {
        CustomBinding _custombindingSoap12;
        public ScheduleManagement()
        {
            InitializeComponent();
            //item = new ServiceSchedule10.Schedule();

            //item.token = "schedule1";
            //item.Description = "schedule description";
            //item.Name = "schedule name";
            //item.Standard = "schedule standard";
            //item.SpecialDays = new ServiceSchedule10.SpecialDaysSchedule[] { new ServiceSchedule10.SpecialDaysSchedule() };
            //item.SpecialDays[0].GroupToken = "specialdaygroup1";

            //item.SpecialDays[0].TimeRange = null;
            //res.Add(item.token, item);

            tvSchedules.AfterSelect += new TreeViewEventHandler(tvSchedules_AfterSelect);
            DisplaySchedule(null);

            _custombindingSoap12 = new CustomBinding();
            _custombindingSoap12.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, Encoding.UTF8));
            _custombindingSoap12.Elements.Add(new HttpTransportBindingElement());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                tvSchedules.Nodes.Clear();

                //Schedule[] schedules = ScheduleClient.GetSchedules(null);

                List<Schedule> schedules = GetList<Schedule>(ScheduleClient.GetScheduleList);

                foreach (var schedule in schedules)
                {
                    {
                        TreeNode scheduleNode = new TreeNode(schedule.token);
                        scheduleNode.Tag = schedule;

                        tvSchedules.Nodes.Add(scheduleNode);
                    }

                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        #region Client

        private SchedulePortClient _scheduleClient;

        protected SchedulePortClient ScheduleClient
        {
            get
            {
                if (_scheduleClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.ScheduleServiceAddress);
                    _scheduleClient = new SchedulePortClient(binding, address);
                }
                return _scheduleClient;
            }

        }

        protected void CheckClientValid()
        {
            if (_scheduleClient != null)
            {
                string address = SMC.SmcData.Context.Instance.General.ScheduleServiceAddress;
                if (address != _scheduleClient.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _scheduleClient = null;
                }
            }
        }


        protected override void UpdateAddress()
        {
            CheckClientValid();

        }

        #endregion


        void tvSchedules_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null)
            {
                while (node.Parent != null)
                {
                    node = node.Parent;
                }

                Schedule info = node.Tag as Schedule;
                DisplaySchedule(info);
            }
            else
            {
                DisplaySchedule(null);

            }
        }

        private void lvSpecialDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSpecialDays.SelectedItems.Count > 0)
            {
                var selectedSpecialDayText = lvSpecialDays.SelectedItems[0].Text;
                var selectedSpecialDays = _schedule.SpecialDays.Where(specialDay => specialDay.GroupToken == selectedSpecialDayText);
                if (selectedSpecialDays.Count() > 0)
                {
                    var selectedSpecialDay = selectedSpecialDays.FirstOrDefault();
                    if (selectedSpecialDay.TimeRange != null)
                    {
                        lvTimeRange.Items.Clear();
                        foreach (var timeRange in selectedSpecialDay.TimeRange)
                        {
                            if (timeRange != null)
                            {
                                ListViewItem item = new ListViewItem();
                                item.Tag = timeRange;
                                item.Text = timeRange.From.ToShortTimeString();
                                item.SubItems.Add(timeRange.Until.ToShortTimeString());
                                lvTimeRange.Items.Add(item);
                            }
                        }
                    }
                }
            }
        }

        #region Right pane

        private Schedule _schedule;

        public void DisplaySchedule(Schedule info)
        {
            _schedule = info;

            if (info != null)
            {
                tbToken.Text = info.token;
                tbName.Text = info.Name;
                tbDescription.Text = info.Description;
                tbStandard.Text = info.Standard;

                //Access Policies

                lvSpecialDays.Items.Clear();

                if (info.SpecialDays != null)
                {
                    foreach (var specialDay in info.SpecialDays)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Tag = specialDay;
                        item.Text = specialDay.GroupToken;
                        lvSpecialDays.Items.Add(item);
                    }
                }
            }
            else
            {
                tbToken.Text = string.Empty;
                tbName.Text = string.Empty;
                tbDescription.Text = string.Empty;
            }

            foreach (Button btn in new Button[] { })
            {
                btn.Enabled = info != null;
            }
        }

        #endregion

    }
}
