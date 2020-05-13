using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SMC.Proxies.Configuration;
using System.Xml;

namespace SMC.Pages
{
    public partial class ConfigurationPage : BaseSmcControl
    {
        public ConfigurationPage()
        {
            InitializeComponent();
        }

        private ConfigurationServiceSoapClient _configurationServiceSoapClient;

        protected ConfigurationServiceSoapClient ConfigurationServiceClient
        {
            get
            {
                if (_configurationServiceSoapClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.ConfigurationServiceAddress);
                    _configurationServiceSoapClient = new ConfigurationServiceSoapClient(binding, address);
                }
                return _configurationServiceSoapClient;
            }

        }

        protected override void UpdateAddress()
        {
            if (_configurationServiceSoapClient != null)
            {
                string address = SMC.SmcData.Context.Instance.General.ConfigurationServiceAddress;
                if (address != _configurationServiceSoapClient.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _configurationServiceSoapClient = null;
                }
            }
        }

        private void btnGetCurrent_Click(object sender, EventArgs e)
        {
            SafeInvoke( () =>
            {
                XmlElement element = (XmlElement)ConfigurationServiceClient.GetCurrentConfiguration();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(element.OuterXml);


                MemoryStream xmlStream = new MemoryStream();
                doc.Save(xmlStream);
                xmlStream.Seek(0, SeekOrigin.Begin);
                string content = Encoding.UTF8.GetString(xmlStream.GetBuffer());

                tbConfiguration.Text = content;
                ColorConfiguration();
            }
        );

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SafeInvoke(() =>
                           {
                               SaveFileDialog sfd = new SaveFileDialog();
                               sfd.DefaultExt = "xml";
                               sfd.Filter = "XML files | *.xml";
                               sfd.AddExtension = true;
                               if (sfd.ShowDialog() == DialogResult.OK)
                               {
                                   TextWriter tw = File.AppendText(sfd.FileName);
                                   tw.Write(tbConfiguration.Text);
                                   tw.Close();
                               }

                           });
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            SafeInvoke(() =>
                           {
                               OpenFileDialog sfd = new OpenFileDialog();
                               sfd.Filter = "XML files | *.xml";
                               if (sfd.ShowDialog() == DialogResult.OK)
                               {
                                   tbConfiguration.Text = File.ReadAllText(sfd.FileName);
                                   ColorConfiguration();
                               }
                           });
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            SafeInvoke(() =>
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(tbConfiguration.Text);
                ConfigurationServiceClient.LoadConfiguration(doc.DocumentElement);
            });
        }
    

        void ColorConfiguration()
        {

        }


    }
}
