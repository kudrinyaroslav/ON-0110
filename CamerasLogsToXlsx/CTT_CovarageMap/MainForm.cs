using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTT_CovarageMap
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        private void SelectTestSpecFolderButton_Click(object sender, EventArgs e)
        {
            SelectTestSpecFolderDialog.SelectedPath = SelectTestSpecFolderTextBox.Text;
            DialogResult result = SelectTestSpecFolderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                SelectTestSpecFolderTextBox.Text = SelectTestSpecFolderDialog.SelectedPath;
                StartButton.Enabled = true;


            }

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            ActualStateLabel.Text = "In progress";
            ActualStateLabel.ForeColor = Color.Orange;
            try
            {
                //ExcelGenerator.Convert(SelectTestSpecFolderTextBox.Text, "A");
                //ExcelGenerator.Convert(SelectTestSpecFolderTextBox.Text, "C");
                //ExcelGenerator.Convert(SelectTestSpecFolderTextBox.Text, "T");
                //ExcelGenerator.Convert(SelectTestSpecFolderTextBox.Text, "S");
                //ExcelGenerator.Convert(SelectTestSpecFolderTextBox.Text, "Q");
                //ExcelGenerator.Convert(SelectTestSpecFolderTextBox.Text, "G");
                ExcelGenerator.Convert(SelectTestSpecFolderTextBox.Text, "D");
                ActualStateLabel.Text = "Success";
                ActualStateLabel.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                ActualStateLabel.Text = "Fail" + ex.Message;
                ActualStateLabel.ForeColor = Color.Red;
            }
        }
    }
}
