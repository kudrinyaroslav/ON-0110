using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using CameraClient.TraceStuff.StepTrace;
using CameraClient.TrafficTrace;

namespace CameraClient
{
    public partial class MainForm : Form
    {
        private TextBoxListener _textBoxListener;

        public MainForm()
        {
            InitializeComponent();

            _textBoxListener = new TextBoxListener(tbTrace);

            List<DiscoveryMode> modes = new List<DiscoveryMode>();
            modes.Add(DiscoveryMode.Discoverable);
            modes.Add(DiscoveryMode.NonDiscoverable);
            comboBoxDiscoveryMode.DataSource = modes;

            listViewTrace.Columns.Add("Number");
            listViewTrace.Columns.Add("Name").Width = 150;
            listViewTrace.Columns.Add("Status").Width = 100;
            listViewTrace.Columns.Add("Message").Width = 200;

            InitTestsTree();
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback((source, certificate, chain, error) =>
                                                            {
                                                                return true;
                                                            });

            Type deviceService = typeof (Device);

            ColumnHeader nameColumn = listViewMethods.Columns.Add("Name");
            nameColumn.Width = 200;

            ColumnHeader parametersColumn = listViewMethods.Columns.Add("Parameters");
            parametersColumn.Width = 40;

            ColumnHeader returnTypeColumn = listViewMethods.Columns.Add("Return Type");
            returnTypeColumn.Width = 100;


            foreach (MethodInfo mi in deviceService.GetMethods().OrderBy(m => m.Name))
            {
                if (mi.DeclaringType == deviceService && !mi.IsSpecialName)
                {
                    ListViewItem lvi = new ListViewItem(mi.Name);

                    long parameters = mi.GetParameters().Length;
                    if (parameters == 0)
                    {
                        lvi.ForeColor = System.Drawing.Color.Green;
                        lvi.BackColor = System.Drawing.Color.Gainsboro;
                    }

                    lvi.SubItems.Add(parameters.ToString());
                    lvi.SubItems.Add(mi.ReturnType.Name);

                    lvi.Tag = mi;
                    listViewMethods.Items.Add(lvi);
                }
                
            }
        }

        #region direct calls
        
        private void btnGetDiscoveryMode_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceClient service = GetService();

                DiscoveryMode discoveryMode = service.GetDiscoveryMode();
                ShowStatusMessage("Done");
                service.Close();

                if (cbShowResult.Checked)
                {
                    string methodInfo = "Result from GetDiscoveryMode";
                    QuickWatchForm form = new QuickWatchForm(methodInfo, discoveryMode);
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                ShowStatusMessage("Operation failed");
                TraceException(ex);
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSetDiscoveryMode_Click(object sender, EventArgs e)
        {
            try
            {
                DiscoveryMode mode = (DiscoveryMode) comboBoxDiscoveryMode.SelectedValue;

                DeviceClient service = GetService();

                if (cbTrace.Checked)
                {
                    if (cbLowLevelTrace.Checked)
                    {
                        System.ServiceModel.Channels.CustomBinding cb = (System.ServiceModel.Channels.CustomBinding)service.Endpoint.Binding;
                        CustomTextMessageBindingElement ctmbe = (CustomTextMessageBindingElement)cb.Elements[0];

                        string value = "Discoverrable";
                        string path = "/s:Envelope/s:Body/onvif:SetDiscoveryMode/onvif:DiscoveryMode";
                        Dictionary<string, string> namespaces = new Dictionary<string, string>();
                        namespaces.Add("s", "http://www.w3.org/2003/05/soap-envelope");
                        namespaces.Add("onvif", "http://www.onvif.org/ver10/device/wsdl");
                        ctmbe.AddBreakingBehaviour(new BreakingBehaviour(path, value, namespaces));
                    }
                }

                service.SetDiscoveryMode(mode);
                ShowStatusMessage("Done");
                service.Close();
            }
            catch (Exception ex)
            {
                ShowStatusMessage("Operation failed");
                TraceException(ex);
                MessageBox.Show(ex.Message);
            }

        }

        private void btnGetClientSertificateMode_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceClient service = GetService();
                bool result = service.GetClientCertificateMode();
                ShowStatusMessage("Done");
                service.Close();
            }
            catch (Exception ex)
            {
                ShowStatusMessage("Operation failed");
                TraceException(ex);
                MessageBox.Show(ex.Message);
            }
        }


        private void btnSetClientCertificateMode_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceClient service = GetService();

                service.SetClientCertificateMode(false);
                ShowStatusMessage("Done");
                service.Close();
            }
            catch (Exception ex)
            {
                ShowStatusMessage("Operation failed");
                TraceException(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void SetZeroConfiguration_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceClient service = GetService();

                service.SetZeroConfiguration("string", false);
                ShowStatusMessage("Done");
                service.Close();
            }
            catch (Exception ex)
            {
                ShowStatusMessage("Operation failed");
                TraceException(ex);
                MessageBox.Show(ex.Message);
            }

        }

        private void SetAccessPolicy_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceClient service = GetService();

                service.SetAccessPolicy("Some parameter");
                ShowStatusMessage("Done");
                service.Close();
            }
            catch (Exception ex)
            {
                ShowStatusMessage("Operation failed");
                TraceException(ex);
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
        
        DeviceClient GetService()
        {
            DeviceClient service = new DeviceClient();
            service.Endpoint.Address = new EndpointAddress(tbAddress.Text);
            System.ServiceModel.Channels.CustomBinding cb = (System.ServiceModel.Channels.CustomBinding)service.Endpoint.Binding;

            if (cbHttps.Checked)
            {
                HttpsTransportBindingElement elem = new HttpsTransportBindingElement();
                cb.Elements[1] = elem;
            }

            if (cbTrace.Checked)
            {
                if (cbLowLevelTrace.Checked)
                {
                    CustomTextMessageBindingElement ctmbe = new CustomTextMessageBindingElement();
                    cb.Elements[0] = ctmbe;
                    ctmbe.SetListener(_textBoxListener);
                }
                else
                {
                    service.Endpoint.Behaviors.Add(new TraceBehavior(_textBoxListener));
                }
            }

            return service;
        }

        private void cbTrace_CheckedChanged(object sender, EventArgs e)
        {
            cbLowLevelTrace.Enabled = cbTrace.Checked;
        }




        
    }
}
