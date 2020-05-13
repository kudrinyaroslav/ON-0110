///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using TestTool.GUI.Views;
using TestTool.GUI.Data;
using TestTool.GUI.Utils;
using TestTool.GUI.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Onvif;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Controller for media tab
    /// </summary>
    class MediaController : Controller<IMediaView>
    {
        /// <summary>
        /// Object for making calls to device service.
        /// </summary>
        private ManagementServiceProvider _deviceClient;
        /// <summary>
        /// Object for making calls to media service.
        /// </summary>
        private MediaServiceProvider _mediaClient;

        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="view">Media tab control</param>
        public MediaController(IMediaView view) 
            : base(view)
        {
        }

        /// <summary>
        /// Creates client for device management service
        /// </summary>
        /// <param name="address">Management service address</param>
        void InitializeDeviceClient(string address)
        {
            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            _deviceClient = new ManagementServiceProvider(address, env.Timeouts.Message);
            _deviceClient.ExceptionThrown += OnExceptionThrown;
            _deviceClient.OperationCompleted += OnOperationCompleted;
            _deviceClient.OperationStarted += OnOperationStarted;
            _deviceClient.Timeout = env.Timeouts.Message;
            _deviceClient.ResponseReceived += OnResponseReceived;
            _deviceClient.OnServicesInfoReceived += OnServicesInfoReceived;
            _deviceClient.Security = ContextController.GetDebugInfo().Security;
        }

        /// <summary>
        /// Creates client for media service
        /// </summary>
        /// <param name="address">Media service address</param>
        void InitializeMediaClient(string address)
        {
            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            _mediaClient = new MediaServiceProvider(address, env.Timeouts.Message);
            _mediaClient.ExceptionThrown += OnExceptionThrown;
            _mediaClient.OperationCompleted += OnOperationCompleted;
            _mediaClient.OperationStarted += OnOperationStarted;
            _mediaClient.OnProfilesReceived += OnProfilesReceived;
            _mediaClient.OnVideoSourceConfigurationsReceived += OnSourceConfigsReceived;
            _mediaClient.OnVideoEncoderConfigurationReceived += OnVideoEncoderConfigsReceived;
            _mediaClient.OnVideoEncoderConfigOptionsReceived += OnVideoEncoderConfigOptionsReceived;
            _mediaClient.OnAudioSourceConfigurationsReceived += OnAudioSourceConfigsReceived;
            _mediaClient.OnAudioEncoderConfigurationReceived += OnAudioEncoderConfigsReceived;
            _mediaClient.OnAudioEncoderConfigOptionsReceived += OnAudioEncoderConfigOptionsReceived;
            _mediaClient.OnMediaUriReceived += OnMediaUriReceived;
            _mediaClient.Timeout = env.Timeouts.Message;
            _mediaClient.ResponseReceived += OnResponseReceived;

            _mediaClient.Security = ContextController.GetDebugInfo().Security;
        }

        /// <summary>
        /// Handles response received event
        /// </summary>
        /// <param name="response">Response</param>
        void OnResponseReceived(string response)
        {
            View.DisplayLog(response);
        }
        
        /// <summary>
        /// Handles operation started event
        /// </summary>
        void OnOperationStarted()
        {
            ReportOperationStarted();
        }

        /// <summary>
        /// Handles operation completed event
        /// </summary>
        void OnOperationCompleted()
        {
            if (_mediaClientWorking)
            {
                _mediaClientWorking = false;
            }
            if (_deviceClientWorking)
            {
                _deviceClientWorking = false;
            }

            ReportOperationCompleted();
        }

        /// <summary>
        /// Indicates that device service client is performing request.
        /// </summary>
        private bool _deviceClientWorking = false;
        /// <summary>
        /// Indicates that media service client is performing request.
        /// </summary>
        private bool _mediaClientWorking = false;

        /// <summary>
        /// Stops async operation
        /// </summary>
        public override void Stop()
        {
            if (_deviceClientWorking)
            {
                _deviceClient.Stop();
            }
            if (_mediaClientWorking)
            {
                _mediaClient.Stop();
            }
        }

        /// <summary>
        /// Inficates that time-consuming operation is pending.
        /// </summary>
        public override bool RequestPending
        {
            get { return _deviceClientWorking || _mediaClientWorking; }
        }

        /// <summary>
        /// Handles exception
        /// </summary>
        /// <param name="stage">Stage</param>
        /// <param name="exc">Exception</param>
        void OnExceptionThrown(string stage, Exception exc)
        {
            if (_mediaClientWorking)
            {
                _mediaClientWorking = false;
            }
            if (_deviceClientWorking)
            {
                _deviceClientWorking = false;
            }

            ReportOperationCompleted();
            SwitchToState(ApplicationState.Idle);

            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            string message;
            if (!string.IsNullOrEmpty(stage))
            {
                message = string.Format("{0}{1}{2}", stage, Environment.NewLine, exc.Message);
            }
            else
            {
                message = exc.Message;
            }
            View.DisplayLog(message.Replace("\n", Environment.NewLine));
        }

        /// <summary>
        /// Restores saved settings
        /// </summary>
        public override void LoadSavedContext(SavedContext context)
        {
            if(context.MediaInfo != null)
            {
                View.MediaAddress = context.MediaInfo.ServiceAddress;
            }
        }

        /// <summary>
        /// Updates context data.
        /// </summary>
        public override void UpdateContext()
        {
            MediaInfo info = new MediaInfo();
            info.ServiceAddress = View.MediaAddress;
            ContextController.UpdateMediaInfo(info);
        }

        /// <summary>
        /// Updates media tab control
        /// </summary>
        public override void UpdateView()
        {
            View.EnableControls(CurrentState == ApplicationState.Idle);
        }

        
        /// <summary>
        /// Gets media profiles
        /// </summary>
        public void GetMediaProfiles()
        {
            string address = View.MediaAddress;
            _mediaClientWorking = true;
            InitializeMediaClient(address);
            _mediaClient.GetProfiles();
        }

        /// <summary>
        /// Returns media service address
        /// </summary>
        public void GetMediaAddress()
        {
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            _deviceClientWorking = true;
            string address = devices != null ? devices.ServiceAddress : string.Empty;
            InitializeDeviceClient(address);
            DebugInfo info = ContextController.GetDebugInfo();
            bool capabilitiesStyle = (info.CapabilitiesExchange == CapabilitiesExchangeStyle.GetCapabilities);
            _deviceClient.GetServiceAddresses(capabilitiesStyle, new CapabilityCategory[] { CapabilityCategory.Media });
            //_deviceClient.GetCapabilities(new Onvif.CapabilityCategory[] { Onvif.CapabilityCategory.Media });
        }


        void OnServicesInfoReceived(Capabilities capabilities, Service[] services)
        {
            if (capabilities != null)
            {
                if (capabilities.Media != null)
                {
                    View.MediaAddress = capabilities.Media.XAddr;
                }
            }
            else
            {
                if (services != null)
                {
                    Service mediaService = 
                        Tests.Common.CommonUtils.Extensions.FindService(
                        services, OnvifService.MEDIA);
                    View.MediaAddress = mediaService.XAddr;
                }
            }
        }

        /// <summary>
        /// Retrieves video source configurations DUT
        /// </summary>
        public void GetVideoSourceConfigurations()
        {
            string address = View.MediaAddress;
            _mediaClientWorking = true;
            InitializeMediaClient(address);
            _mediaClient.GetVideoSourceConfigurations();
        }

        /// <summary>
        /// Retrieves audio source configurations DUT
        /// </summary>
        public void GetAudioSourceConfigurations()
        {
            string address = View.MediaAddress;
            _mediaClientWorking = true;
            InitializeMediaClient(address);
            _mediaClient.GetAudioSourceConfigurations();
        }

        /// <summary>
        /// Retrieves video encoder configurations from DUT
        /// </summary>
        public void GetVideoEncoderConfigurations()
        {
            string address = View.MediaAddress;
            _mediaClientWorking = true;
            InitializeMediaClient(address);
            _mediaClient.GetVideoEncoderConfigurations();
        }

        /// <summary>
        /// Retrieves video encoder configurations from DUT
        /// </summary>
        public void GetAudioEncoderConfigurations()
        {
            string address = View.MediaAddress;
            _mediaClientWorking = true;
            InitializeMediaClient(address);
            _mediaClient.GetAudioEncoderConfigurations();
        }
        /// <summary>
        /// Retrieves video encoder configuration options from DUT
        /// </summary>
        public void GetVideoEncoderConfigOptions(string encoder)
        {
            string address = View.MediaAddress;
            _mediaClientWorking = true;
            InitializeMediaClient(address);
            _mediaClient.GetVideoEncoderConfigOptions(encoder);
        }
        /// <summary>
        /// Retrieves video encoder configuration options from DUT
        /// </summary>
        public void GetAudioEncoderConfigOptions(string encoder)
        {
            string address = View.MediaAddress;
            _mediaClientWorking = true;
            InitializeMediaClient(address);
            _mediaClient.GetAudioEncoderConfigOptions(encoder);
        }
        /// <summary>
        /// Handles video source configurations received event
        /// </summary>
        /// <param name="configs">Received profiles</param>
        protected void OnSourceConfigsReceived(VideoSourceConfiguration[] configs)
        {
            View.SetVideoSourceConfigs(configs);
        }

        /// <summary>
        /// Handles media profiles received event
        /// </summary>
        /// <param name="profiles">Received profiles</param>
        protected void OnProfilesReceived(Proxies.Onvif.Profile[] profiles)
        {
            View.SetProfiles(profiles);
        }
        /// <summary>
        /// Handles audio source configurations received event
        /// </summary>
        /// <param name="configs">Received profiles</param>
        protected void OnAudioSourceConfigsReceived(AudioSourceConfiguration[] configs)
        {
            View.SetAudioSourceConfigs(configs);
        }
        
        /// <summary>
        /// Handles video encoder configurations received event
        /// </summary>
        /// <param name="configs">Received profiles</param>
        protected void OnVideoEncoderConfigsReceived(VideoEncoderConfiguration[] configs)
        {
            View.SetVideoEncoderConfigs(configs);
        }

        /// <summary>
        /// Handles audio encoder configurations received event
        /// </summary>
        /// <param name="configs">Received profiles</param>
        protected void OnAudioEncoderConfigsReceived(AudioEncoderConfiguration[] configs)
        {
            View.SetAudioEncoderConfigs(configs);
        }
        /// <summary>
        /// Handles video encoder configuration options received event
        /// </summary>
        /// <param name="options">Received profiles</param>
        protected void OnVideoEncoderConfigOptionsReceived(VideoEncoderConfigurationOptions options)
        {
            View.SetVideoEncoderConfigOptions(options);
        }

        /// <summary>
        /// Handles audio encoder configuration options received event
        /// </summary>
        /// <param name="options">Received profiles</param>
        protected void OnAudioEncoderConfigOptionsReceived(AudioEncoderConfigurationOptions options)
        {
            View.SetAudioEncoderConfigOptions(options);
        }

        /// <summary>
        /// Handles media uri received event
        /// </summary>
        protected void OnMediaUriReceived(MediaUri uri, VideoEncoderConfiguration encoder, AudioEncoderConfiguration audio)
        {
            View.ShowVideo(uri, encoder, audio);
        }

        /// <summary>
        /// Get stream uri with specified configurations
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="videoSourceConfig">Video source configuration</param>
        /// <param name="videoEncoderConfig">Video encoder configuration</param>
        /// <param name="audioSourceConfig">Audio source configuration</param>
        /// <param name="audioEncoderConfig">Audio encoder configuration</param>
        /// <param name="protocol"></param>
        public void GetMediaUri(
            Proxies.Onvif.Profile profile,
            VideoSourceConfiguration videoSourceConfig,
            VideoEncoderConfiguration videoEncoderConfig,
            AudioSourceConfiguration audioSourceConfig,
            AudioEncoderConfiguration audioEncoderConfig,
            TransportProtocol protocol)
        {
            if((videoEncoderConfig == null)||(videoSourceConfig == null))
            {
                throw new ArgumentNullException();
            }
            string address = View.MediaAddress;
            _mediaClientWorking = true;
            InitializeMediaClient(address);
            _mediaClient.GetMediaUri(profile, videoSourceConfig, videoEncoderConfig, audioSourceConfig, audioEncoderConfig, protocol);
        }
        /// <summary>
        /// Get stream uri for specified profile
        /// </summary>
        /// <param name="profile">Media profile</param>
        /// <param name="protocol"></param>
        public void GetMediaUri(Proxies.Onvif.Profile profile, TransportProtocol protocol)
        {
            string address = View.MediaAddress;
            _mediaClientWorking = true;
            InitializeMediaClient(address);
            _mediaClient.GetMediaUri(profile, protocol);
        }
    }
}
