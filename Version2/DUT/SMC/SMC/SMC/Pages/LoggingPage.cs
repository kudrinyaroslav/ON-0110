using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ServiceModel;
using SMC.Proxies.Logging;
using SMC.Logging;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace SMC.Pages
{
    public partial class LoggingPage : BaseSmcControl
    {
        #region Members

        CustomBinding _custombindingSoap12;
        private ServiceHost _listenerHost;
        private Guid _subscriptionId;
        private LoggingServiceSoapClient _client;
        private int _lastMessage;

        #endregion

        public LoggingPage()
        {
            InitializeComponent();
            tbLogReceiver.Text = System.Configuration.ConfigurationManager.AppSettings["LogReceiver"];

            btnStop.Enabled = false;

            _custombindingSoap12 = new CustomBinding();
            _custombindingSoap12.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, Encoding.UTF8));
            _custombindingSoap12.Elements.Add(new HttpTransportBindingElement());

            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

        }
        
        protected LoggingServiceSoapClient Client
        {
            get
            {
                if (_client == null)
                {
                    string loggerAddress = SmcContext.General.LoggingServiceAddress;
                    _client = new LoggingServiceSoapClient(GetBinding(), new EndpointAddress(loggerAddress));
                }
                return _client;
            }
        }


        protected override void UpdateAddress()
        {
            if (_client != null)
            {
                string address = SMC.SmcData.Context.Instance.General.LoggingServiceAddress;
                if (address != _client.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _client = null;
                }
            }
        }

        private void btnStartListening_Click(object sender, EventArgs e)
        {
            if (!chkError.Checked)
            {
                MessageBox.Show("No message types selected!");
            }
            else
            {
                System.Threading.Thread thread = new Thread(new ThreadStart(OpenHost));
                thread.Start();                
            }
        }

        void OpenHost()
        {
            try
            {
                MessageType type = MessageType.Error;
                if (chkDetails.Checked)
                {
                    type = MessageType.Details;
                }
                else
                {
                    if (chkMessage.Checked)
                    {
                        type = MessageType.Message;
                    }
                    else
                    {
                        if (chkWarning.Checked)
                        {
                            type = MessageType.Warning;
                        }
                    }
                }

                _subscriptionId = Client.Subscribe(tbLogReceiver.Text, type);

                LogReceiver logReceiver = new LogReceiver();
                logReceiver.MessageReceived += new MessageReceivedEvent(logReceiver_MessageReceived);
                logReceiver.ConnectionClosed += new Action(logReceiver_ConnectionClosed);

                _listenerHost = new ServiceHost(logReceiver, new Uri(tbLogReceiver.Text));
                ServiceEndpoint endpoint = _listenerHost.AddServiceEndpoint(typeof(Logging.LogReceiverSoap), _custombindingSoap12, string.Empty);
                _listenerHost.Open();

                EnableControls(true);

            }
            catch (Exception exc)
            {
                ShowError(exc);
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            System.Threading.Thread thread = new Thread(new ThreadStart(Unsubscribe));
            thread.Start();
        }

        void Unsubscribe()
        {
            try
            {
                Client.Unsubscribe(_subscriptionId);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
            finally
            {
                _listenerHost.Close();
                _listenerHost = null;
                EnableControls(false);
            }
        }
        
        void Application_ApplicationExit(object sender, EventArgs e)
        {
            try
            {
                if (_listenerHost != null)
                {
                    Unsubscribe();
                    _listenerHost.Close();
                }
                if (_client != null)
                {
                    _client.Close();
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lvLog.Items.Clear();
            _lastMessage = 0;
        }

        #region Server messages

        void logReceiver_MessageReceived(string message, SMC.Logging.MessageType type)
        {
            Invoke(new Action(
                       () =>
                           {
                               ListViewItem item = new ListViewItem(_lastMessage.ToString());
                               item.ImageKey = type.ToString();
                               item.SubItems.Add(new ListViewItem.ListViewSubItem(item,
                                                                                  DateTime.Now.ToString("HH-mm-ss.ffff")));
                               item.SubItems.Add(new ListViewItem.ListViewSubItem(item, message));
                               lvLog.Items.Add(item);
                               _lastMessage++;
                           }
                       ));
  
        }

        void logReceiver_ConnectionClosed()
        {
            Invoke(new Action(
                       () =>
                           {
                               _listenerHost.Close();

                               ListViewItem item = new ListViewItem("");

                               item.SubItems.Add(new ListViewItem.ListViewSubItem(item,
                                                                                  DateTime.Now.ToString("HH-mm-ss.ffff")));
                               item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "Server closed connection"));
                               lvLog.Items.Add(item);
                           }

                       ));
            EnableControls(false);
        }

        #endregion

        #region Checkboxes

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWarning.Checked)
            {
                chkError.Checked = true;
            }
            else
            {
                chkMessage.Checked = false;
                chkDetails.Checked = false;
            }
        }

        private void chkMessage_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMessage.Checked)
            {
                chkError.Checked = true;
                chkWarning.Checked = true;
            }
            else
            {
                chkDetails.Checked = false;
            }
        }

        private void chkDetails_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDetails.Checked)
            {
                chkError.Checked = true;
                chkWarning.Checked = true;
                chkMessage.Checked = true;
            }
        }

        private void chkError_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkError.Checked)
            {
                chkWarning.Checked = false;
                chkMessage.Checked = false;
                chkDetails.Checked = false;
            }
        }

        #endregion

        void EnableControls(bool connected)
        {
            Invoke(new Action(() =>
            {
                btnStop.Enabled = connected;
                btnStartListening.Enabled = !connected;
                tbLogReceiver.ReadOnly = connected;
                chkDetails.Enabled = !connected;
                chkMessage.Enabled = !connected;
                chkWarning.Enabled = !connected;
                chkError.Enabled = !connected;
            }));
        }

    }
}
