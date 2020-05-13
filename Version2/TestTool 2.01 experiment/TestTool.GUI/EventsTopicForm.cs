using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestTool.Tests.Common.TestEngine;

namespace TestTool.GUI
{
    /// <summary>
    /// Dialog for selecting/entering events topic. 
    /// </summary>
    public partial class EventsTopicForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EventsTopicForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Topics available.
        /// </summary>
        /// <param name="filters">Topics information</param>
        public void SetFilters(List<EventsTopicInfo> filters)
        {
            cmbFilters.DataSource = filters;
            cmbFilters.DisplayMember = "Topic";
        }

        /// <summary>
        /// Topic selected or entered.
        /// </summary>
        public EventsTopicInfo Topic
        {
            get
            {
                return new EventsTopicInfo(){Topic = cmbFilters.Text, NamespacesDefinition = txtNamespaces.Text};
            }
        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilters.SelectedIndex >= 0)
            {
                EventsTopicInfo topic = (EventsTopicInfo)cmbFilters.SelectedItem;
                txtNamespaces.Text = topic.NamespacesDefinition;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string[] namespaces = txtNamespaces.Text.Replace(Environment.NewLine, "").Split(' ');

            bool valid = true;
            foreach (string definition in namespaces)
            {
                if (!string.IsNullOrEmpty(definition) && !definition.Contains("="))
                {
                    valid = false;
                }
            }

            if (!valid)
            {
                MessageBox.Show(
                    "Namespaces definition is incorrect. Should be in form \"prefix1=\"namespace1\"[ prefix2=\"namespace2\"]\" ");
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

    }
}
