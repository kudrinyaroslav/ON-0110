using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DTT_CovarageMap
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void SelectTestSpecFolderButton_Click(object sender, EventArgs e)
        {
            SelectAllTestResultsXMLFileDialog.FileName = AllTestResultsXMLFileTextBox.Text;
            DialogResult result = SelectAllTestResultsXMLFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                AllTestResultsXMLFileTextBox.Text = SelectAllTestResultsXMLFileDialog.FileName;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            ActualStateLabel.Text = "In progress";
            ActualStateLabel.ForeColor = Color.Orange;
            try
            {
                ExcelGenerator.Convert(CovarageMapFolderTextBox.Text, AllTestResultsXMLFileTextBox.Text, ProfileComboBox.Text);
                ActualStateLabel.Text = "Success";
                ActualStateLabel.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                ActualStateLabel.Text = "Fail" + ex.Message;
                ActualStateLabel.ForeColor = Color.Red;
            }
        }

        private void CovarageMapFolderButton_Click(object sender, EventArgs e)
        {
            SelectCovarageMapFolderDialog.SelectedPath = CovarageMapFolderTextBox.Text;
            DialogResult result = SelectCovarageMapFolderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                CovarageMapFolderTextBox.Text = SelectCovarageMapFolderDialog.SelectedPath;
            }
        }

    }
}
