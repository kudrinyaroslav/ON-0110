///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.GUI.Views;
using TestTool.GUI.Data;
using TestTool.GUI.Utils;
using TestTool.GUI.Enums;
using Media = TestTool.Proxies.Media;
using Device = TestTool.Proxies.Device;
using PTZ = TestTool.Proxies.PTZ;

namespace TestTool.GUI.Controllers
{
    class PtzController : Controller<IPtzView>
    {
        private bool _deviceClientWorking = false;
        private bool _mediaClientWorking = false;
        private bool _ptzClientWorking = false;

        private ManagementServiceProvider _deviceClient;
        private MediaServiceProvider _mediaClient;
        private PTZServiceProvider _ptzClient;

        public PtzController(IPtzView view)
            :base(view)
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
            _deviceClient.ExceptionThrown += new Action<string, Exception>(OnExceptionThrown);
            _deviceClient.OperationCompleted += new Action(OnOperationCompleted);
            _deviceClient.OperationStarted += new Action(OnOperationStarted);
            _deviceClient.OnCapabilitiesReceived += OnCapabilitiesReceived;
            _deviceClient.Timeout = env.Timeouts.Message;
            _deviceClient.ResponseReceived += OnResponseReceived;
        }

        /// <summary>
        /// Creates client for media service
        /// </summary>
        /// <param name="address">Media service address</param>
        void InitializeMediaClient(string address)
        {
            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            _mediaClient = new MediaServiceProvider(address, env.Timeouts.Message);
            _mediaClient.ExceptionThrown += new Action<string, Exception>(OnExceptionThrown);
            _mediaClient.OperationCompleted += new Action(OnOperationCompleted);
            _mediaClient.OperationStarted += new Action(OnOperationStarted);
            _mediaClient.OnMediaUriReceived += OnMediaUriReceived;
            _mediaClient.Timeout = env.Timeouts.Message;
            _mediaClient.ResponseReceived += OnResponseReceived;
            _mediaClient.OnProfilesReceived += OnProfilesReceived;
            _mediaClient.OnPTZConfigurationAdded += OnPTZConfigurationAdded;
        }
        /// <summary>
        /// Creates client for PTZ service
        /// </summary>
        /// <param name="address">PTZ service address</param>
        void IntializePtzClient(string address)
        {
            DeviceEnvironment env = ContextController.GetDeviceEnvironment();
            _ptzClient = new PTZServiceProvider(address, env.Timeouts.Message);
            _ptzClient.ExceptionThrown += new Action<string, Exception>(OnExceptionThrown);
            _ptzClient.OperationCompleted += new Action(OnOperationCompleted);
            _ptzClient.OperationStarted += new Action(OnOperationStarted);
            _ptzClient.Timeout = env.Timeouts.Message;
            _ptzClient.ResponseReceived += OnResponseReceived;
            _ptzClient.OnPTZConfigurationsReceived += OnPTZConfigurationsReceived;
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
        /// Handles capabilities received event
        /// </summary>
        /// <param name="capabilities">Received capabilities</param>
        void OnCapabilitiesReceived(Device.Capabilities capabilities)
        {
            View.PTZAddress = string.Empty;
            View.MediaAddress = string.Empty;
            if (capabilities.PTZ != null)
            {
                View.PTZAddress = capabilities.PTZ.XAddr;
            }
            if(capabilities.Media != null)
            {
                View.MediaAddress = capabilities.Media.XAddr;
            }
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
            if (_ptzClientWorking)
            {
                _ptzClientWorking = false;
            }
            ReportOperationCompleted();
        }

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
            if (_ptzClientWorking)
            {
                _ptzClient.Stop();
            }
        }

        public override bool RequestPending
        {
            get { return _deviceClientWorking || _mediaClientWorking || _ptzClientWorking; }
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
            if (_ptzClientWorking)
            {
                _ptzClientWorking = false;
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
            if (context.PTZInfo != null)
            {
                View.PTZAddress = context.PTZInfo.ServiceAddress;
                View.MediaAddress = context.PTZInfo.MediaAddress;
            }
        }

        public override void UpdateContext()
        {
            PTZInfo info = new PTZInfo();
            info.ServiceAddress = View.PTZAddress;
            info.MediaAddress = View.MediaAddress;
            ContextController.UpdatePTZInfo(info);
        }

        /// <summary>
        /// Updates media tab control
        /// </summary>
        public override void UpdateView()
        {
            View.EnableControls(CurrentState == ApplicationState.Idle);
        }

        /// <summary>
        /// Returns service addresses
        /// </summary>
        public void GetAddress(Device.CapabilityCategory[] categories)
        {
            DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
            _deviceClientWorking = true;
            string address = devices != null ? devices.ServiceAddress : string.Empty;
            InitializeDeviceClient(address);
            _deviceClient.GetCapabilities(categories);
        }

        /// <summary>
        /// Returns media service address
        /// </summary>
        public void GetStreamUri()
        {
            _mediaClientWorking = true;
            InitializeMediaClient(View.MediaAddress);
            _mediaClient.GetMediaUri();
        }

        /// <summary>
        /// Moves camera step by step from min position to max
        /// </summary>
        /// <param name="absolute"></param>
        /// <param name="profile">profile token</param>
        /// <param name="xmin">pan min value</param>
        /// <param name="xmax">pan max value</param>
        /// <param name="ymin">tilt min value</param>
        /// <param name="ymax">tilt max value</param>
        /// <param name="zmin">zoom min value</param>
        /// <param name="zmax">zoom max value</param>
        public void AbosuteRelativeIncrementalMove(
            bool absolute,
            string profile, 
            decimal xmin, 
            decimal xmax, 
            decimal ymin,
            decimal ymax,
            decimal zmin,
            decimal zmax)
        {
            _ptzClientWorking = true;
            IntializePtzClient(View.PTZAddress);
            _ptzClient.AbosuteRelativeIncrementalMove(absolute, profile, xmin, xmax, ymin, ymax, zmin, zmax);
        }

        /// <summary>
        /// Moves camera to specified position
        /// </summary>
        /// <param name="absolute"></param>
        /// <param name="profile">profile token</param>
        /// <param name="x">pan position (-1..1)</param>
        /// <param name="y">tilt position (-1..1)</param>
        /// <param name="z">zoom position (0..1)</param>
        public void AbosuteRelativeMove(bool absolute, string profile, decimal x, decimal y, decimal z)
        {
            _ptzClientWorking = true;
            IntializePtzClient(View.PTZAddress);
            PTZ.PTZVector vector = new PTZ.PTZVector();
            vector.PanTilt = new PTZ.Vector2D();
            vector.PanTilt.space = absolute ?
                "http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace" :
                "http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace";
            vector.PanTilt.x = (float)x;
            vector.PanTilt.y = (float)y;
            vector.Zoom = new PTZ.Vector1D();
            vector.Zoom.space = absolute ?
                "http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace" :
                "http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace";
            vector.Zoom.x = (float)z;
            if (absolute)
            {
                _ptzClient.AbsoluteMove(profile, vector);
            }
            else
            {
                _ptzClient.RelativeMove(profile, vector);
            }
        }
        /// <summary>
        /// Starts continuous move with specified speed
        /// </summary>
        /// <param name="profile">profile</param>
        /// <param name="panTilt">if true, move pan/tilt</param>
        /// <param name="zoom">if true, move zoom</param>
        /// <param name="x">pan speed (-1..1)</param>
        /// <param name="y">tilt speed (-1..1)</param>
        /// <param name="z">zoom speed (0..1)</param>
        /// <param name="timeout">movement timeout. No timeout if value is less than zero</param>
        public void ContinuousMove(string profile, bool panTilt, bool zoom, decimal x, decimal y, decimal z, int timeout)
        {
            _ptzClientWorking = true;
            IntializePtzClient(View.PTZAddress);
            PTZ.PTZSpeed speed = new PTZ.PTZSpeed();
            if(panTilt)
            {
                speed.PanTilt = new PTZ.Vector2D();
                speed.PanTilt.space = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace";
                speed.PanTilt.x = (float)x;
                speed.PanTilt.y = (float)y;
            }
            if(zoom)
            {
                speed.Zoom = new PTZ.Vector1D();
                speed.Zoom.space = "http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace";
                speed.Zoom.x = (float)z;
            }
            string duration = null;
            if(timeout > 0)
            {
                TimeSpan span = TimeSpan.FromMilliseconds((double)timeout);
                duration = System.Xml.XmlConvert.ToString(span);
            }
            _ptzClient.ContinuousMove(profile, speed, duration);
        }

        /// <summary>
        /// Stops camera movements
        /// </summary>
        /// <param name="profile">profile token</param>
        /// <param name="panTilt">if true, stops pan/tilt movement</param>
        /// <param name="zoom">if true, stops zoom movement</param>
        public void Stop(string profile, bool panTilt, bool zoom)
        {
            _ptzClientWorking = true;
            IntializePtzClient(View.PTZAddress);
            _ptzClient.Stop(profile, panTilt, zoom);
        }

        /// <summary>
        /// Gets media profiles with PTZ configuration
        /// </summary>
        public void GetProfiles()
        {
            _mediaClientWorking = true;
            InitializeMediaClient(View.MediaAddress);
            _mediaClient.GetProfiles();
        }

        private Media.Profile _profile;
        /// <summary>
        /// Gets media profiles with PTZ configuration
        /// </summary>
        public void AddPTZConfiguration(Media.Profile profile)
        {
            _ptzClientWorking = true;
            IntializePtzClient(View.PTZAddress);
            _profile = profile;
            _ptzClient.GetConfigurations();
        }

        
        /// <summary>
        /// Handles PTZ configurations received event
        /// </summary>
        protected void OnPTZConfigurationsReceived(PTZ.PTZConfiguration[] configs)
        {
            if ((configs == null)||(configs.Length == 0))
            {
                throw new Exception("No PTZ configuration available");
            }
            _mediaClientWorking = true;
            InitializeMediaClient(View.MediaAddress);
            _mediaClient.AddPTZConfiguration(_profile.token, configs[0].token);
        }

        /// <summary>
        /// Handles media uri received event
        /// </summary>
        protected void OnMediaUriReceived(Media.MediaUri uri, Media.VideoEncoderConfiguration encoder, Media.AudioEncoderConfiguration audio)
        {
            View.ShowVideo(uri, encoder);
        }
        
        /// <summary>
        /// Handles profiles received event
        /// </summary>
        protected void OnProfilesReceived(Media.Profile[] profiles)
        {
            View.SetProfiles(profiles);
        }

        /// <summary>
        /// Handles profiles received event
        /// </summary>
        protected void OnPTZConfigurationAdded(string profile, string configuration)
        {
            View.OnPTZConfigurationAdded(profile, configuration);
        }
    }
}
