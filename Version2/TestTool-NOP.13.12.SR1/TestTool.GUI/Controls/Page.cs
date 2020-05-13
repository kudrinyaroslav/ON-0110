///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Windows.Forms;
using System;

namespace TestTool.GUI.Controls
{
    abstract class Page : UserControl, Views.IView
    {
        protected void EnableControls(IEnumerable<Control> controls)
        {
            foreach (Control control  in controls)
            {
                control.Enabled = true;
            }
        }

        protected void DisableControls(IEnumerable<Control> controls)
        {
            foreach (Control control in controls)
            {
                control.Enabled = false;
            }
        }

        protected void ShowPrompt(string message, string caption)
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        #region IView Members

        /// <summary>
        /// Displays exception message to user
        /// </summary>
        /// <param name="e">Exception to be displayed</param>
        public virtual void ShowError(System.Exception e)
        {
            string message = e.Message.Length > 400 ? e.Message.Substring(0, 400) : e.Message;
            string errorMessage = "Unexpected error occurred: " + message;

            ShowErrorMessageBox(errorMessage);
        }

        public virtual void ShowError(string message)
        {
            ShowErrorMessageBox(message);
        }

        void ShowErrorMessageBox(string message)
        { 
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
            else 
            {
                MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }        
        }
        
        public abstract void SwitchToState(TestTool.GUI.Enums.ApplicationState state);

        public abstract TestTool.GUI.Controllers.IController GetController();

        #endregion
    }
}
