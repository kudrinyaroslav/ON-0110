using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CamerasLogsToXlsx
{
    public partial class Form1 : Form
    {
        public List<String> pathList;
        public String XlsxFolder = Environment.CurrentDirectory + "\\";
        public String ExcelName1 = "";
        public String ExcelPath;


        public Form1()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void SelectLogsButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.RestoreDirectory = false;
            DialogResult result = openFileDialog1.ShowDialog();

            pathList = new List<String>();
            if (result == DialogResult.OK)
            {
                //XmlFolder = Environment.CurrentDirectory;
                for (int i = 0; i < openFileDialog1.FileNames.Length; i++)
                {
                    pathList.Add(openFileDialog1.FileNames.ElementAt(i));
                }
                if (ExcelPath != null && pathList != null)
                {
                    ActualState.Text = "Ready";
                    ActualState.ForeColor = Color.Green;
                    StartButton.Enabled = true;
                }
                SelectedAutoTest.Text = "Auto test(s) report selected";
                //foreach (var name in pathList)
                //{
                //    SelectedAutoTest.Text += name + '\n';    
                //}
            }
        }

        private void SelectExportFolderButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                XlsxFolder = folderBrowserDialog1.SelectedPath + "\\";
                FolderLabel.Text = XlsxFolder;
                ExcelPath = XlsxFolder + ExcelName.Text;
                if (pathList != null)
                {
                    ActualState.Text = "Ready";
                    ActualState.ForeColor = Color.Green;
                    StartButton.Enabled = true;
                }
            }

        }

        private void UserExcelName_TextChanged(object sender, EventArgs e)
        {
            ExcelName1 = UserExcelName.Text;
            ExcelName.Text = ExcelName1 + DateTime.Now.Date.ToShortDateString().Replace(".", "-") + ".xlsx";
            if (XlsxFolder != null)
            {
                ExcelPath = XlsxFolder + ExcelName.Text;
                //In ideal: ensure that the text is suitable for a filename
            }
            if (ExcelPath != null && pathList != null)
            {
                ActualState.Text = "Ready";
                ActualState.ForeColor = Color.Green;
                StartButton.Enabled = true;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            ActualState.Text = "In progress";
            ActualState.ForeColor = Color.Orange;
            try
            {
                FinalConvertcs convert = new FinalConvertcs();
                convert.Convert(pathList, ExcelPath);
                ActualState.Text = "Success";
                ActualState.ForeColor = Color.Green;
            }
            catch
            {
                ActualState.Text = "Fail";
                ActualState.ForeColor = Color.Red;
            }
        }

        private void openFileDialog1_FileOk_1(object sender, CancelEventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
    }
}
