using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestTool.GUI.Controls
{
    internal partial class LogViewer : UserControl
    {
        public LogViewer()
        {
            InitializeComponent();
        }

        public void LogEvent(string message)
        {
            tbPlainLog.AppendText(message);
        }

        public void Clear()
        {
            tbPlainLog.Clear();
            lvCommands.Clear();
            tbResponse.Clear();
            tbRequest.Clear();
        }

        public void DisplayRequestProcessingLog(TestTool.Services.RequestProcessingLog log)
        {
            string id = (lvCommands.Items.Count + 1).ToString();
            ListViewItem item = new ListViewItem(id);
            item.SubItems.Add(log.Time.ToString("HH:mm:ss.fff"));
            item.SubItems.Add(log.Service);
            item.SubItems.Add(log.Method);

            item.Tag = log;

            Func<Services.RequestProcessingLog, bool> stateValidation =
                (l) =>
                    {
                        if (_selectedService == log.Service)
                        {
                            if (string.IsNullOrEmpty(_selectedMethod) || _selectedMethod == log.Method)
                            {
                                return true;
                            }
                        }
                        return false;
                    };
            SetupItem(item, stateValidation);

            lvCommands.Items.Add(item);
        }

        private void lvCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCommands.SelectedItems.Count > 0)
            {
                Services.RequestProcessingLog log = (Services.RequestProcessingLog) lvCommands.SelectedItems[0].Tag;
                tbRequest.Text = log.Request;
                tbResponse.Text = log.Response;
            }
            else
            {
                tbRequest.Clear();
                tbResponse.Clear();
            }
        }

        private string _selectedService;
        private string _selectedMethod;

        public void SelectByMethod(string serviceName, string methodName)
        {
            SelectItems(l => l.Service == serviceName && l.Method == methodName);

            _selectedService = serviceName;
            _selectedMethod = methodName;
        }

        public void SelectByService(string serviceName)
        {
            SelectItems(l => l.Service == serviceName);
            _selectedService = serviceName;
            _selectedMethod = string.Empty;
        }

        public void ResetSelection()
        {
            SelectItems(l => false);
            _selectedService = string.Empty;
            _selectedMethod = string.Empty;
        }

        void SelectItems(Func<Services.RequestProcessingLog, bool> selector)
        {
            foreach (ListViewItem item in lvCommands.Items)
            {
                SetupItem(item, selector);
            }
        }

        void SetupItem(ListViewItem item, Func<Services.RequestProcessingLog, bool> selector)
        {
            Services.RequestProcessingLog log = (Services.RequestProcessingLog)item.Tag;
            if (selector(log))
            {
                item.BackColor = Color.LightCyan;
            }
            else
            {
                item.BackColor = Color.White;
            }
        }

    }
}
