using System;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SMC.Proxies.Events;
using SMC.Events;

namespace SMC.Pages
{
    public partial class SubscriptionPage : BaseSmcControl
    {
        public SubscriptionPage()
        {
            InitializeComponent();

            tbEventsReceiver.Text = System.Configuration.ConfigurationManager.AppSettings["EventsReceiver"];

            tbNamespaces.Text =
                "tns1=\"http://www.onvif.org/ver10/topics\" ";

            _custombindingSoap12 = new CustomBinding();
            _custombindingSoap12.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, Encoding.UTF8));
            _custombindingSoap12.Elements.Add(new HttpTransportBindingElement());
        }

        CustomBinding _custombindingSoap12;

        private NotificationProducerClient _client;
        NotificationProducerClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new NotificationProducerClient(_custombindingSoap12,
                                                             new EndpointAddress(
                                                                 new Uri(SmcContext.General.EventsServiceAddress)));
                }
                return _client;
            }
        }
        
        protected override void UpdateAddress()
        {
            if (_client != null)
            {
                string address = SMC.SmcData.Context.Instance.General.EventsServiceAddress;
                if (address != _client.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _client = null;
                }
            }
        }

        ServiceHost _host;
        EndpointReferenceType _subscriptionReference;
        SubscriptionManagerClient _subscriptionManager;
        private PullPointSubscriptionClient _pullPointSubscriptionClient;
        EventsReceiver _listener;

        private void btnSubscribe_Click(object sender, EventArgs e)
        {
            try
            {
                EventsReceiver listener = new EventsReceiver();
                listener.NotificationReceived += listener_NotificationReceived;
                listener.Name = tbEventsReceiver.Text;
                _listener = listener;

                _host = new ServiceHost(listener, new Uri(tbEventsReceiver.Text));
                ServiceEndpoint endpoint = _host.AddServiceEndpoint(typeof(Proxies.Events.NotificationConsumer), new WSHttpBinding(SecurityMode.None), string.Empty);
                _host.Open();


                Subscribe request = new Subscribe();
                request.ConsumerReference = new EndpointReferenceType();
                request.ConsumerReference.Address = new AttributedURIType();
                request.ConsumerReference.Address.Value = tbEventsReceiver.Text;

                if (!string.IsNullOrEmpty(tbTopicsFilter.Text))
                {
                    request.Filter = CreateFilter();
                }

                request.InitialTerminationTime = tbSubscriptionTime.Text;

                SubscribeResponse response = Client.Subscribe(request);
                tcSubscription.SelectedTab = tpManageSubscription;
                _subscriptionReference = response.SubscriptionReference;
                tbSubscriptionReference.Text = _subscriptionReference.Address.Value;
                _subscribed = true;
                if (response.TerminationTime.HasValue)
                {
                    _terminationTime = response.TerminationTime.Value;
                    tbTerminationTime.Text = _terminationTime.ToString("hh:mm:ss.fff");

                    DisplayTimeLeft();
                }

                EndpointAddress addr = new EndpointAddress(_subscriptionReference.Address.Value);
                EndpointReferenceBehaviour behaviour = new EndpointReferenceBehaviour(response.SubscriptionReference);

                _subscriptionManager = new SubscriptionManagerClient(_custombindingSoap12, addr);
                _subscriptionManager.Endpoint.Behaviors.Add(behaviour);

                _pullPointSubscriptionClient = new PullPointSubscriptionClient(_custombindingSoap12,addr);
                _pullPointSubscriptionClient.Endpoint.Behaviors.Add(behaviour);
                
                timer.Start();
                btnSubscribe.Enabled = false;
                btnRenew.Enabled = true;
                btnUnsubscribe.Enabled = true;
                btnSetSynchronizationPoint.Enabled = true;

            }
            catch (Exception exc)
            {
                _host.Close();
                MessageBox.Show(exc.Message);
            }
        }

        FilterType CreateFilter()
        {
            Dictionary<string, string> namespaces = new Dictionary<string, string>();
            string[] definitions = tbNamespaces.Text.Replace('"' + Environment.NewLine, "\" ").Split(' ');
            foreach (string definition in definitions)
            {
                if (!string.IsNullOrEmpty(definition))
                {
                    string[] parts = definition.Split('=');
                    namespaces.Add(parts[0], parts[1].Replace("\"", "").Replace(Environment.NewLine, "").Trim());
                }
            }
            
            string topicPath = tbTopicsFilter.Text;
            

            FilterType filterType = new FilterType();
            
            XmlDocument filterDoc = new XmlDocument();
            XmlElement filterTopicElement = filterDoc.CreateTopicElement();
            
            foreach (string prefix in namespaces.Keys)
            {
                filterTopicElement.AddNamespacePrefix(namespaces[prefix], prefix);
            }            
            filterTopicElement.InnerText = topicPath;

            filterType.Any = new XmlElement[] { filterTopicElement };
            return filterType;
        }
        
        void listener_NotificationReceived(EventsReceiver source, Notify1 obj)
        {
            Invoke(new Action(() =>
                                  {
                                      foreach (NotificationMessageHolderType message in obj.Notify.NotificationMessage)
                                      {
                                          string topic = string.Empty;
                                          foreach (XmlNode node in message.Topic.Any)
                                          {
                                              XmlText topicText = node as XmlText;
                                              if (topicText != null)
                                              {
                                                  topic = topicText.Value;
                                                  break;
                                              }
                                          }

                                          tbConsole.AppendText("Topic: " + topic + Environment.NewLine);
                                          tbConsole.AppendText(message.Message.OuterXml);
                                          tbConsole.AppendText(Environment.NewLine);
                                      }

                                      tbConsole.AppendText(Environment.NewLine);
                                  }));
        }
        
        private void btnRenew_Click(object sender, EventArgs e)
        {
            SafeInvoke(() =>
                           {
                               Renew renew = new Renew();
                               renew.TerminationTime = tbRenewTime.Text;
                               RenewResponse response = _subscriptionManager.Renew(renew);
                               if (response.TerminationTime.HasValue)
                               {
                                   _terminationTime = response.TerminationTime.Value;
                                   tbTerminationTime.Text = _terminationTime.ToString("hh:mm:ss.fff");
                               }
                           });
        }
        
        private void btnUnsubscribe_Click(object sender, EventArgs e)
        {
            SafeInvoke(() =>
            {
                _host.Close();
                Unsubscribe request = new Unsubscribe();
                _subscriptionManager.Unsubscribe(request);
                tcSubscription.SelectedTab = tbSubscribe;
                StopTimer();
            });

        }

        private void btnSetSynchronizationPoint_Click(object sender, EventArgs e)
        {
            SafeInvoke(()=>
                           {
                               _pullPointSubscriptionClient.SetSynchronizationPoint();
                           });
        }

        private DateTime _terminationTime;
        private bool _subscribed;
        private void timer_Tick(object sender, EventArgs e)
        {
            if (_subscribed)
            {
                DisplayTimeLeft();
                TimeSpan left = _terminationTime - DateTime.UtcNow;
                if (left.TotalSeconds < 1)
                {
                    _host.Close();
                    StopTimer();
                    tcSubscription.SelectedTab = tbSubscribe;
                }
            }
        }

        void DisplayTimeLeft()
        {
            TimeSpan left = _terminationTime - DateTime.UtcNow;
            tbTimeLeft.Text = (left.TotalSeconds - 1).ToString();
            if (left.TotalSeconds < 5)
            {
                tbTimeLeft.ForeColor = Color.Red;
            }

        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            tbConsole.Clear();
        }

        void StopTimer()
        {
            _subscribed = false;
            tbTimeLeft.ForeColor = Color.Black;
            timer.Stop();

            if (_listener != null)
            {
                _listener.NotificationReceived -= listener_NotificationReceived;
            }

            btnSubscribe.Enabled = true;
            btnRenew.Enabled = false;
            btnUnsubscribe.Enabled = false;
            btnSetSynchronizationPoint.Enabled = false;

        }

    }
}
