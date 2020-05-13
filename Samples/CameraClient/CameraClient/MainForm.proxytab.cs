using System;
using System.Net;
using System.Windows.Forms;
using System.Reflection;
using System.ServiceModel;

namespace CameraClient
{
    public partial class MainForm
    {

        private void btnCall_Click(object sender, EventArgs e)
        {
            try
            {
                ListViewItem lvi = listViewMethods.SelectedItems[0];
                MethodInfo mi = (MethodInfo)lvi.Tag;

                if (mi.GetParameters().Length == 0)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                    DeviceClient service = GetService();

                    WriteLine(string.Format("Call {0}", mi.Name), System.Drawing.Color.Brown);
                    if (mi.ReturnType != typeof(void))
                    {
                        object result = mi.Invoke(service, new object[0]);
                        if (cbShowResult.Checked)
                        {
                            string methodInfo = string.Format("Result from {0}", mi.Name);
                            QuickWatchForm form = new QuickWatchForm(methodInfo, result);
                            form.Show();
                        }
                    }
                    else
                    {
                        mi.Invoke(service, new object[0]);
                    }
                    WriteLine("Done.\n");
                    ShowStatusMessage("Done");
                    service.Close();
                }

            }
            catch (Exception ex)
            {
                WriteLine(string.Format("Error: {0}", ex.Message));
                ShowStatusMessage("Operation failed");
                FaultException faultException = ex.InnerException as FaultException;

                if (faultException != null)
                {
                    WriteLine(string.Format("   InnerException: {0}", faultException.Message), System.Drawing.Color.Red);
                    WriteLine("");
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbConsole.Clear();
        }

        private void listViewMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCall.Enabled = listViewMethods.SelectedItems.Count > 0;
        }

        private void listViewMethods_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewMethods.SelectedItems.Count > 0)
            {
                ListViewItem lvi = listViewMethods.SelectedItems[0];
                MethodInfo mi = (MethodInfo)lvi.Tag;
                MethodInfoForm form = new MethodInfoForm(mi);
                form.Show();
            }
        }


    }
}
