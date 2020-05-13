using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestTool.GUI.Controls
{
    internal class BasePage : UserControl
    {
        public void ReportError(string message)
        {
            Invoke(new Action(() =>
                                  {
                                      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                  }));
        }

    }
}
