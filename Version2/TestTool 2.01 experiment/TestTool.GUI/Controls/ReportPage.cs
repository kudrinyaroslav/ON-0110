///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;

namespace TestTool.GUI.Controls
{
    partial class ReportPage : UserControl, IReportView
    {
        private ReportController _controller;

        public ReportPage()
        {
            InitializeComponent();
            _controller = new ReportController(this);
        }

        internal ReportController Controller
        {
            get { return _controller; }
        }

        #region IReportView Members

        public string FileName
        {
            get { return tbFileName.Text; }
            set { tbFileName.Text = value; }
        }

        #endregion
        public void SwitchToState(Enums.ApplicationState state)
        {
            switch (state)
            {
                case Enums.ApplicationState.Idle:
                case Enums.ApplicationState.CommandRunning:
                case Enums.ApplicationState.DiscoveryRunning:
                    {
                        BeginInvoke(new Action(() => { EnableControls(_controller.ReportNotEmpty());}));
                    }
                    break;
                case Enums.ApplicationState.TestPaused:
                case Enums.ApplicationState.TestRunning:
                    {
                        BeginInvoke(new Action(() => { EnableControls(false); }));
                    }
                    break;
            }
        }

        void EnableControls(bool bEnable)
        {
            tbFileName.Enabled = bEnable;
            btnSave.Enabled = bEnable;
            btnFileName.Enabled = bEnable;

        }

        public void EnableSaveReport(bool bEnable)
        {
            EnableControls(bEnable);
        }
        
        public void WriteLine(string entry)
        {
            BeginInvoke(new Action(() => { WriteLineInternal(entry); }));
        }

        public void Clear()
        {
            tbTestReport.Clear();
        }

        public void ReportException(Exception exception)
        {
            MessageBox.Show(exception.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ReportOperationCompleted()
        {
            MessageBox.Show("Report saved", "Operation completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void WriteLineInternal(string line)
        {
            tbTestReport.AppendText(string.Format("{0}{1}", line, Environment.NewLine));
        }

        #region IView Members


        public IController GetController()
        {
            return _controller;
        }

        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_controller.IsTestInfoFull())
            {
                MessageBox.Show("Please, fill Device Under Test information and Test Execution information at the Setup tab.", "Can not generate report");
                return;
            }

            string directory = System.IO.Directory.GetCurrentDirectory();

            if (string.IsNullOrEmpty(tbFileName.Text))
            {
                SaveFileDialog dlg = CreateDialog();

                DialogResult dr = dlg.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    tbFileName.Text = dlg.FileName;
                    _controller.SaveTestResults(dlg.FileName);
                }
            }
            else
            {
                _controller.SaveTestResults(tbFileName.Text);
            }

            System.IO.Directory.SetCurrentDirectory(directory);
        }

        private void btnFileName_Click(object sender, EventArgs e)
        {
            string directory = System.IO.Directory.GetCurrentDirectory();
            SaveFileDialog dlg = CreateDialog();

            try
            {
                string fileName = tbFileName.Text;
                string path = System.IO.Path.GetDirectoryName(fileName);
                dlg.InitialDirectory = path;
            }
            catch (Exception)
            {
            }

            DialogResult dr = dlg.ShowDialog();
            if (dr == DialogResult.OK)
            {
                tbFileName.Text = dlg.FileName;
            }
            System.IO.Directory.SetCurrentDirectory(directory);

        }

        SaveFileDialog CreateDialog()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = "pdf";
            dlg.CheckPathExists = true;
            dlg.OverwritePrompt = false;
            dlg.Filter = "PDF Document |*.pdf";

            try
            {
                string fileName = tbFileName.Text;
                new System.IO.FileInfo(fileName);
                dlg.FileName = fileName;
            }
            catch (Exception)
            {
            }
            return dlg;
        }
     
    }
}
