using System.ServiceModel;
using System.ServiceModel.Channels;
using TestTool.HttpTransport;
using TestTool.Proxies.Onvif;

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


        void CloseClients()
        {
            foreach (ICommunicationObject client in 
                new ICommunicationObject[] { _mediaClient, _ptzClient, _ioClient, _imagingClient, _replayClient })
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
