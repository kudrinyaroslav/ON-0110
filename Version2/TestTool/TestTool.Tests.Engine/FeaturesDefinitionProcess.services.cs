using System.ServiceModel;
using System.ServiceModel.Channels;
using TestTool.HttpTransport;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Onvif;
using TestTool.Proxies.Event;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Onvif;

namespace TestTool.Tests.Engine
{
    partial class FeaturesDefinitionProcess
    {
        #region Services

        protected override DeviceClient CreateClient()
        {
            HttpBinding binding = (HttpBinding)CreateBinding(true);
            DeviceClient client = new DeviceClient(binding, new EndpointAddress(CameraAddress));
            return client;
        }

        private bool _mediaSupported;
        private string _mediaServiceAddress;
        private MediaClient _mediaClient;

        protected MediaClient MediaClient
        {
            get
            {
                if (_mediaClient == null && _mediaSupported)
                {
                    Binding binding = CreateBinding(false);

                    _mediaClient = new MediaClient(binding, new EndpointAddress(_mediaServiceAddress));

                    AttachSecurity(_mediaClient.Endpoint);
                    SetupChannel(_mediaClient.InnerChannel);
                }
                return _mediaClient;
            }
        }

        private bool _ioSupported;
        private string _ioServiceAddress;
        private DeviceIOPortClient _ioClient;

        protected DeviceIOPortClient IoClient
        {
            get
            {
                if (_ioClient == null && _ioSupported)
                {
                    Binding binding = CreateBinding(false);

                    _ioClient = new DeviceIOPortClient(binding, new EndpointAddress(_ioServiceAddress));

                    AttachSecurity(_ioClient.Endpoint);
                    SetupChannel(_ioClient.InnerChannel);
                }
                return _ioClient;
            }
        }

        private bool _ptzSupported;
        private string _ptzServiceAddress;
        private PTZClient _ptzClient;

        protected PTZClient PtzClient
        {
            get
            {
                if (_ptzClient == null && _ptzSupported)
                {
                    Binding binding = CreateBinding(false);

                    _ptzClient = new PTZClient(binding, new EndpointAddress(_ptzServiceAddress));

                    AttachSecurity(_ptzClient.Endpoint);
                    SetupChannel(_ptzClient.InnerChannel);
                }
                return _ptzClient;
            }
        }

        private bool _imagingSupported;
        private string _imagingServiceAddress;
        private ImagingPortClient _imagingClient;

        protected ImagingPortClient ImagingClient
        {
            get
            {
                if (_imagingClient == null && _imagingSupported)
                {
                    Binding binding = CreateBinding(false);

                    _imagingClient = new ImagingPortClient(binding, new EndpointAddress(_imagingServiceAddress));

                    AttachSecurity(_imagingClient.Endpoint);
                    SetupChannel(_imagingClient.InnerChannel);
                }
                return _imagingClient;
            }
        }


        private bool _replaySupported;
        private string _replayServiceAddress;
        private ReplayPortClient _replayClient;

        protected ReplayPortClient ReplayClient
        {
            get
            {
                if (_replayClient == null && _replaySupported)
                {
                    Binding binding = CreateBinding(false);

                    _replayClient = new ReplayPortClient(binding, new EndpointAddress(_replayServiceAddress));

                    AttachSecurity(_replayClient.Endpoint);
                    SetupChannel(_replayClient.InnerChannel);
                }
                return _replayClient;
            }
        }


        private bool _searchSupported;
        private string _searchServiceAddress;
        private SearchPortClient _searchClient;

        protected SearchPortClient SearchClient
        {
            get
            {
                if (_searchClient == null && _searchSupported)
                {
                    Binding binding = CreateBinding(false);

                    _searchClient = new SearchPortClient(binding, new EndpointAddress(_searchServiceAddress));

                    AttachSecurity(_searchClient.Endpoint);
                    SetupChannel(_searchClient.InnerChannel);
                }
                return _searchClient;
            }
        }


        private bool _recordingControlSupported;
        private string _recordingControlServiceAddress;
        private RecordingPortClient _recordingControlClient;

        protected RecordingPortClient RecordingClient
        {
            get
            {
                if (_recordingControlClient == null && _recordingControlSupported)
                {
                    Binding binding = CreateBinding(false);

                    _recordingControlClient = new RecordingPortClient(binding, new EndpointAddress(_recordingControlServiceAddress));

                    AttachSecurity(_recordingControlClient.Endpoint);
                    SetupChannel(_recordingControlClient.InnerChannel);
                }
                return _recordingControlClient;
            }
        }


        private bool _receiverSupported;
        private string _receiverServiceAddress;
        private ReceiverPortClient _receiverClient;

        protected ReceiverPortClient ReceiverClient
        {
            get
            {
                if (_receiverClient == null && _receiverSupported)
                {
                    Binding binding = CreateBinding(false);

                    _receiverClient = new ReceiverPortClient(binding, new EndpointAddress(_receiverServiceAddress));

                    AttachSecurity(_receiverClient.Endpoint);
                    SetupChannel(_receiverClient.InnerChannel);
                }
                return _receiverClient;
            }
        }

        bool _accessControlSupported;
        string _accessControlServiceAddress;
        private PACSPortClient _accessControlClient;
        protected PACSPortClient AccessControlClient
        {
            get
            {
                if (_accessControlClient == null && _accessControlSupported)
                {
                    Binding binding = CreateBinding(false);

                    _accessControlClient = new PACSPortClient(binding, new EndpointAddress(_accessControlServiceAddress));

                    AttachSecurity(_accessControlClient.Endpoint);
                    SetupChannel(_accessControlClient.InnerChannel);
                }
                return _accessControlClient;
            }
        }

        bool _doorControlSupported;
        string _doorControlServiceAddress;

        private DoorControlPortClient _doorControlClient;
        protected DoorControlPortClient DoorControlClient
        {
            get
            {
                if (_doorControlClient == null && _doorControlSupported)
                {
                    Binding binding = CreateBinding(false);

                    _doorControlClient = new DoorControlPortClient(binding, new EndpointAddress(_doorControlServiceAddress));

                    AttachSecurity(_doorControlClient.Endpoint);
                    SetupChannel(_doorControlClient.InnerChannel);
                }
                return _doorControlClient;
            }
        }

        private string _eventsServiceAddress;
        private EventPortTypeClient  _eventClient;

        protected EventPortTypeClient EventClient
        {
            get
            {
                if (_eventClient == null)
                {
                    //var binding = CreateBinding(false);
                    //[21.05.2013] AKS: added WsaController to make behaviour of client like in Event tests
                    var controller = new EndpointController(new EndpointAddress(_eventsServiceAddress));

                    var wsaController = new WsaController();

                    var controllers = new IChannelController[]
                              {
                                  _trafficListener, 
                                  _semaphore, 
                                  _credentialsProvider, 
                                  controller, 
                                  wsaController
                              };

                    var binding = CreateBinding(controllers);

                    _eventClient = new EventPortTypeClient(binding, new EndpointAddress(_eventsServiceAddress));

                    AttachSecurity(_eventClient.Endpoint);
                    SetupChannel(_eventClient.InnerChannel);
                }
                return _eventClient;
            }
        }

        private string _accessRulesServiceAddress;
        private AccessRulesPortClient m_AccessRulesClient;
        private AccessRulesPortClient AccessRulesClient
        {
            get
            {
                if (null == m_AccessRulesClient && !string.IsNullOrEmpty(_accessRulesServiceAddress))
                {
                    var customControllers = new[] { new SoapValidator(AccessRulesSchemaSet.GetInstance()) };
                    Binding binding = CreateBinding(false, customControllers);

                    m_AccessRulesClient = new AccessRulesPortClient(binding, new EndpointAddress(_accessRulesServiceAddress));

                    AttachSecurity(m_AccessRulesClient.Endpoint);
                    SetupChannel(m_AccessRulesClient.InnerChannel);
                }

                return m_AccessRulesClient;
            }
        }

        private string _advancedSecurityServiceAddress;
        private AdvancedSecurityServiceClient m_AdvancedSecurityClient;
        private AdvancedSecurityServiceClient AdvancedSecurityClient
        {
            get
            {
                if (null == m_AdvancedSecurityClient && !string.IsNullOrEmpty(_advancedSecurityServiceAddress))
                {
                    var customControllers = new[] { new SoapValidator(AdvancedSecuritySchemaSet.GetInstance()) };
                    Binding binding = CreateBinding(false, customControllers);

                    m_AdvancedSecurityClient = new AdvancedSecurityServiceClient(binding, new EndpointAddress(_advancedSecurityServiceAddress));

                    AttachSecurity(m_AdvancedSecurityClient.Endpoint);
                    SetupChannel(m_AdvancedSecurityClient.InnerChannel);
                }

                return m_AdvancedSecurityClient;
            }
        }

        private string _credentialServiceAddress;
        private CredentialPortClient m_CredentialPortClient;
        private CredentialPortClient CredentialPortClient
        {
            get
            {
                if (null == m_CredentialPortClient && !string.IsNullOrEmpty(_credentialServiceAddress))
                {
                    var customControllers = new[] { new SoapValidator(CredentialSchemaSet.GetInstance()) };
                    Binding binding = CreateBinding(false, customControllers);

                    m_CredentialPortClient = new CredentialPortClient(binding, new EndpointAddress(_credentialServiceAddress));

                    AttachSecurity(m_CredentialPortClient.Endpoint);
                    SetupChannel(m_CredentialPortClient.InnerChannel);
                }

                return m_CredentialPortClient;
            }
        }

        private string _scheduleServiceAddress;
        private SchedulePortClient m_SchedulePortClient;
        private SchedulePortClient SchedulePortClient
        {
            get
            {
                if (null == m_SchedulePortClient && !string.IsNullOrEmpty(_scheduleServiceAddress))
                {
                    var customControllers = new[] { new SoapValidator(ScheduleSchemaSet.GetInstance()) };
                    Binding binding = CreateBinding(false, customControllers);

                    m_SchedulePortClient = new SchedulePortClient(binding, new EndpointAddress(_scheduleServiceAddress));

                    AttachSecurity(m_SchedulePortClient.Endpoint);
                    SetupChannel(m_SchedulePortClient.InnerChannel);
                }

                return m_SchedulePortClient;
            }
        }


        void CloseClients()
        {
            foreach (ICommunicationObject client in
                new ICommunicationObject[] { _mediaClient, _ptzClient, _ioClient, _imagingClient, _replayClient, _accessControlClient, _doorControlClient })
            {
                if (client != null)
                {
                    client.Close();
                }
            }
        }

        #endregion

    }
}
