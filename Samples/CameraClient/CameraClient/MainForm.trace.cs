using System;
using System.Text;

namespace CameraClient
{
    public partial class MainForm
    {
        #region Trace

        void WriteLine(string message, System.Drawing.Color color)
        {
            int begin = tbConsole.Text.Length;
            tbConsole.AppendText(message + Environment.NewLine);
            int end = tbConsole.Text.Length;
            tbConsole.Select(begin, end - begin);
            tbConsole.SelectionColor = color;
            tbConsole.Invalidate();
            tbConsole.Select(0, 0);
        }

        void WriteLine(string message)
        {
            WriteLine(message, tbConsole.ForeColor);
        }

        void TraceException(Exception ex)
        {
            if (cbTrace.Checked)
            {
                WriteException(ex);
            }
        }

        void WriteException(Exception ex)
        {
            string message = string.Format("Exception: [{0}] {1}", ex.GetType().FullName, ex.Message);
            StringBuilder sb = new StringBuilder(message);
            Exception inner = ex.InnerException;
            string offset = Environment.NewLine + "   ";
            while (inner != null)
            {
                sb.Append(offset);
                sb.AppendFormat("InnerException: [{0}] {1}", inner.GetType().FullName, inner.Message);

                inner = inner.InnerException;
                offset += "   ";
            }

            _textBoxListener.WriteLine(sb.ToString(), InformationType.Error);
        }

        private void btnClearTrace_Click(object sender, EventArgs e)
        {
            tbTrace.Clear();
        }

        #endregion

        #region Status Bar

        void ShowStatusMessage(string message)
        {
            toolStripStatusLabelMessage.Text = message;
            ourTimer.Start();
        }

        private void ourTimer_Tick(object sender, EventArgs e)
        {
            ourTimer.Stop();
            toolStripStatusLabelMessage.Text = string.Empty;
        }

        #endregion

    }

}
