using System.ServiceModel;
using System.ServiceModel.Channels;
using TestTool.HttpTransport;
using TestTool.Proxies.Onvif;
using TestTool.Proxies.Event;

namespace TestTool.Tests.Engine
{
    partial class FeaturesDefinitionProcess
    {
        #region Services

        protected override DeviceClient CreateClient()
        {
            HttpBinding binding =
                (HttpBinding)CreateBinding(true);
            DeviceClient client = new DeviceClient(binding, new EndpointAddress(_cameraAddress));
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
                    Binding binding = CreateBinding(false);

                    _eventClient = new EventPortTypeClient(binding, new EndpointAddress(_eventsServiceAddress));

                    AttachSecurity(_eventClient.Endpoint);
                    SetupChannel(_eventClient.InnerChannel);
                }
                return _eventClient;
            }
        }



        void CloseClients()
        {
            ICommunicationObject[] clients = new ICommunicationObject[] 
                { 
                    _mediaClient, 
                    _ptzClient, 
                    _ioClient, 
                    _imagingClient, 
                    _accessControlClient, 
                    _doorControlClient 
                };

            foreach (ICommunicationObject client in clients)
            {
                if (client != null && client.State == CommunicationState.Opened)
                {
                    client.Close();
                }
            }
        }

        #endregion

    }
}
