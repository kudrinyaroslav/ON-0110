///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestTool.GUI.Views;
using TestTool.GUI.Controllers;
using TestTool.GUI.Utils;
using Media = TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Media;
using TestTool.GUI.Data;
using TestTool.Tests.Common.TestEngine;

namespace TestTool.GUI.Controls.Device
{
    partial class MediaPage : Page, IMediaView
    {
        private class MediaProfileWrapper
        {
            public Media.Profile Profile { get; set; }
            public override string ToString()
            {
                return !string.IsNullOrEmpty(Profile.Name) ? string.Format("{0} ({1})", Profile.Name, Profile.token) : Profile.token;
            }
        }
        private class AudioSourceConfigurationWrapper
        {
            public Media.AudioSourceConfiguration Configuration { get; set; }
            public override string ToString()
            {
                return !string.IsNullOrEmpty(Configuration.Name) ? string.Format("{0} ({1})", Configuration.Name, Configuration.token) : Configuration.token;
            }
        }
        private class AudioEncoderConfigurationWrapper
        {
            public Media.AudioEncoderConfiguration Configuration { get; set; }
            public override string ToString()
            {
                return !string.IsNullOrEmpty(Configuration.Name) ? string.Format("{0} ({1})", Configuration.Name, Configuration.token) : Configuration.token;
            }
        }
        private class VideoSourceConfigurationWrapper
        {
            public Media.VideoSourceConfiguration Configuration { get; set; }
            public override string ToString()
            {
                return !string.IsNullOrEmpty(Configuration.Name) ? string.Format("{0} ({1})", Configuration.Name, Configuration.token) : Configuration.token;
            }
        }
        private class VideoEncoderConfigurationWrapper
        {
            public Media.VideoEncoderConfiguration Configuration { get; set; }
            public override string ToString()
            {
                return !string.IsNullOrEmpty(Configuration.Name) ? string.Format("{0} ({1})", Configuration.Name, Configuration.token) : Configuration.token;
            }
        }
        private class VideoResolutionWrapper
        {
            public Media.VideoResolution Resolution { get; set; }
            public override string ToString()
            {
                return Resolution != null ? string.Format("{0}x{1}", Resolution.Width, Resolution.Height) : string.Empty;
            }
        }
        private class VideoCodecWrapper
        {
            public Media.H264Options H264 { get; set; }
            public Media.JpegOptions Jpeg { get; set; }
            public Media.Mpeg4Options Mpeg4 { get; set; }

            public Media.VideoEncoding Encoding 
            {
                get { return H264 != null ? Media.VideoEncoding.H264 : Jpeg != null ? Media.VideoEncoding.JPEG : Media.VideoEncoding.MPEG4; }
            }
            public Media.IntRange GetFrameLimit()
            {
                Media.IntRange limit = null;
                if (H264 != null)
                {
                    limit = H264.FrameRateRange;
                }
                else if (Jpeg != null)
                {
                    limit = Jpeg.FrameRateRange;
                }
                else if (Mpeg4 != null)
                {
                    limit = Mpeg4.FrameRateRange;
                }
                return limit;
            }
            public Media.VideoResolution[] GetResolutions()
            {
                Media.VideoResolution[] res = null;
                if (H264 != null)
                {
                    res = H264.ResolutionsAvailable;
                }
                else if(Jpeg != null)
                {
                    res = Jpeg.ResolutionsAvailable;
                }
                else if(Mpeg4 != null)
                {
                    res = Mpeg4.ResolutionsAvailable;
                }
                return res;
            }
            public override string  ToString()
            {
                return H264 != null ? "H264" : Jpeg != null ? "Jpeg" : "Mpeg4";
            }
        }
        private class TransportProtocolWrapper
        {
            public Media.TransportProtocol Value { get; set; }
            public string Text { get; set; }

            public TransportProtocolWrapper(Media.TransportProtocol value, string text)
            {
                Value = value;
                Text = text;
            }
            public override string ToString()
            {
                return Text;
            }
            public static implicit operator Media.TransportProtocol(TransportProtocolWrapper item)
            {
                return item.Value;
            }
        }

        private VideoContainer _videoWindow;
        private string _currentVideoEncoder;
        private string _currentAudioEncoder;
        private MediaController _controller;

        #region IMediaView Members

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string MediaAddress 
        {
            get { return tbMediaUrl.Text; }
            set
            {
                BeginInvoke(new Action(() =>
                {
                    tbMediaUrl.Text = value;
                    ClearInfo();
                }));
            }
        }
        public void DisplayLog(string logEntry)
        {
            BeginInvoke(new Action(() => tbReport.Text = logEntry));
        }
        protected bool IsTestProfile(Media.Profile profile)
        {
            return profile.Name == MediaServiceProvider.TestMediaProfileName;
        }
        public void SetProfiles(Media.Profile[] profiles)
        {
            BeginInvoke(new Action(() =>
            {
                cmbMediaProfile.Items.Clear();
                object testProfile = null;;
                foreach (Media.Profile profile in profiles)
                {
                    object item = new MediaProfileWrapper() { Profile = profile };
                    if (IsTestProfile(profile))
                    {
                        testProfile = item;
                    }
                    cmbMediaProfile.Items.Add(item);
                }
                if (testProfile == null)
                {
                    testProfile = new MediaProfileWrapper() { Profile = new TestTool.Proxies.Onvif.Profile() };
                    ((MediaProfileWrapper)testProfile).Profile.Name = MediaServiceProvider.TestMediaProfileName;
                    cmbMediaProfile.Items.Add(testProfile);
                }
                cmbMediaProfile.SelectedItem = testProfile;
            }));
        }
        public void SetVideoSourceConfigs(Media.VideoSourceConfiguration[] configs)
        {
            BeginInvoke(new Action(() => {
                cmbVideoSource.Items.Clear();
                foreach (Media.VideoSourceConfiguration config in configs)
                {
                    cmbVideoSource.Items.Add(new VideoSourceConfigurationWrapper() { Configuration = config });
                }
                if(cmbVideoSource.Items.Count > 0)
                {
                    cmbVideoSource.SelectedIndex = 0;
                }
            }));
        }
        public void SetVideoEncoderConfigs(Media.VideoEncoderConfiguration[] configs)
        {
            BeginInvoke(new Action(() =>
            {
                cmbVideoEncoder.Items.Clear();
                foreach (Media.VideoEncoderConfiguration config in configs)
                {
                    cmbVideoEncoder.Items.Add(new VideoEncoderConfigurationWrapper() { Configuration = config });
                }
                if (cmbVideoEncoder.Items.Count > 0)
                {
                    cmbVideoEncoder.SelectedIndex = 0;
                }
            }));
        }
        public void SetVideoEncoderConfigOptions(Media.VideoEncoderConfigurationOptions options)
        {
            BeginInvoke(new Action(() =>
            {
                cmbVideoCodec.Items.Clear();
                if (options.JPEG != null)
                {
                    cmbVideoCodec.Items.Add(new VideoCodecWrapper { Jpeg = options.JPEG });
                }
                if (options.MPEG4 != null)
                {
                    cmbVideoCodec.Items.Add(new VideoCodecWrapper { Mpeg4 = options.MPEG4 });
                }
                if (options.H264 != null)
                {
                    cmbVideoCodec.Items.Add(new VideoCodecWrapper{ H264 = options.H264 });
                }
                if (cmbVideoCodec.Items.Count > 0)
                {
                    cmbVideoCodec.SelectedIndex = 0;
                }
            }));
        }
        public void SetAudioEncoderConfigOptions(Media.AudioEncoderConfigurationOptions options)
        {
            BeginInvoke(new Action(() =>
            {
                cmbAudioCodec.Items.Clear();
                foreach (Media.AudioEncoderConfigurationOption option in options.Options)
                {
                    cmbAudioCodec.Items.Add(option);
                }
                if (cmbAudioCodec.Items.Count > 0)
                {
                    cmbAudioCodec.SelectedIndex = 0;
                }
            }));
        }

        public void SetAudioSourceConfigs(Media.AudioSourceConfiguration[] configs)
        {
            BeginInvoke(new Action(() =>
            {
                cmbAudioSource.Items.Clear();
                foreach (Media.AudioSourceConfiguration config in configs)
                {
                    cmbAudioSource.Items.Add(new AudioSourceConfigurationWrapper() { Configuration = config });
                }
                if (cmbAudioSource.Items.Count > 0)
                {
                    cmbAudioSource.SelectedIndex = 0;
                }
            }));
        }
        public void SetAudioEncoderConfigs(Media.AudioEncoderConfiguration[] configs)
        {
            BeginInvoke(new Action(() =>
            {
                cmbAudioEncoder.Items.Clear();
                foreach (Media.AudioEncoderConfiguration config in configs)
                {
                    cmbAudioEncoder.Items.Add(new AudioEncoderConfigurationWrapper() { Configuration = config });
                }
                if (cmbAudioEncoder.Items.Count > 0)
                {
                    cmbAudioEncoder.SelectedIndex = 0;
                }
            }));
        }
        private Media.TransportProtocol GetTransportProtocol()
        {
            Media.TransportProtocol protocol = Media.TransportProtocol.UDP;
            Invoke(new Action(() => { protocol = (TransportProtocolWrapper)cmbTransport.SelectedItem; }));
            return protocol;
        }
        public void ShowVideo(Media.MediaUri uri, Media.VideoEncoderConfiguration encoder, Media.AudioEncoderConfiguration audio)
        {
            try
            {
                _videoWindow = new VideoContainer();
                DeviceEnvironment environment = ContextController.GetDeviceEnvironment();
                int messageTimeout = environment.Timeouts.Message;
                Media.TransportProtocol protocol = GetTransportProtocol();
                VideoUtils.AdjustVideo(
                    _videoWindow,
                    environment.Credentials.UserName,
                    environment.Credentials.Password,
                    messageTimeout,
                    protocol,
                    Media.StreamType.RTPUnicast,
                    uri,
                    encoder);
                _videoWindow.KEEPALIVE = true;
                _videoWindow.DebugPage = true;
                _videoWindow.OpenWindow(audio != null);

                Invoke(new Action(() => { btnGetStreams.Text = "Stop Video"; }));
            }
            catch
            {
                _videoWindow.DebugPage = false;
                _videoWindow = null;
                throw;
            }
        }
        #endregion

        public MediaPage()
        {
            InitializeComponent();
            _controller = new MediaController(this);
            cmbAudioCodec.DisplayMember = "Encoding";

            cmbTransport.Items.Add(new TransportProtocolWrapper(Media.TransportProtocol.UDP, "RTP/UDP"));
            cmbTransport.Items.Add(new TransportProtocolWrapper(Media.TransportProtocol.RTSP, "RTP/RTSP/TCP"));
            cmbTransport.Items.Add(new TransportProtocolWrapper(Media.TransportProtocol.HTTP, "RTP/RTSP/HTTP/TCP"));
            cmbTransport.SelectedIndex = 0;
        }
        protected void ClearInfo()
        {
            cmbAudioCodec.Items.Clear();
            cmbAudioEncoder.Items.Clear();
            cmbAudioSource.Items.Clear();
            cmbVideoCodec.Items.Clear();
            cmbVideoEncoder.Items.Clear();
            cmbVideoResolution.Items.Clear();
            cmbVideoSource.Items.Clear();
            cmbMediaProfile.Items.Clear();
            cmbAudioBitrate.Items.Clear();
            txtVideoBitrate.Text = null;
            txtVideoFramerate.Text = null;
         }

        internal MediaController Controller
        {
            get { return _controller; }
        }

        public override void SwitchToState(Enums.ApplicationState state)
        {
            if (state.IsActive())
            {
                EnableControls(false);
            }
            else
            {
                EnableControls(true);
            }
        }

        #region IView Members

        public override IController GetController()
        {
            return _controller;
        }

        #endregion

        public void EnableControls(bool enable)
        {
            Invoke(new Action(()=>
            {
                DiscoveredDevices devices = ContextController.GetDiscoveredDevices();
                string address = devices != null ? devices.ServiceAddress : string.Empty;

                bool testProfileSelected = false;
                if (cmbMediaProfile.SelectedItem != null)
                {
                    testProfileSelected = IsTestProfile(((MediaProfileWrapper)cmbMediaProfile.SelectedItem).Profile);
                }

                btnGetMediaUrl.Enabled = enable && !string.IsNullOrEmpty(address);
                btnGetMediaUrl.Refresh();
                btnGetProfiles.Enabled = enable && !string.IsNullOrEmpty(MediaAddress);
                btnGetProfiles.Refresh();
                btnDeleteProfile.Enabled = enable && testProfileSelected;
                btnDeleteProfile.Refresh();
                buttonGetVideoSources.Enabled = testProfileSelected && enable && !string.IsNullOrEmpty(MediaAddress);
                buttonGetVideoSources.Refresh();
                buttonGetVideoEncoders.Enabled = testProfileSelected && enable && !string.IsNullOrEmpty(MediaAddress);
                buttonGetVideoEncoders.Refresh();
                buttonGetVideoCodecs.Enabled = buttonGetVideoEncoders.Enabled && (cmbVideoEncoder.SelectedItem != null);
                buttonGetVideoCodecs.Refresh();
                buttonGetAudioSources.Enabled = testProfileSelected && enable && !string.IsNullOrEmpty(MediaAddress);
                buttonGetAudioSources.Refresh();
                buttonGetAudioEncoders.Enabled = testProfileSelected && enable && !string.IsNullOrEmpty(MediaAddress);
                buttonGetAudioEncoders.Refresh();
                buttonGetAudioCodecs.Enabled = buttonGetAudioEncoders.Enabled && (cmbAudioEncoder.SelectedItem != null);
                buttonGetAudioCodecs.Refresh();
                btnGetStreams.Enabled = (enable && cmbVideoResolution.SelectedItem != null)||(_videoWindow != null);

                cmbMediaProfile.Enabled = enable;
                cmbAudioCodec.Enabled = testProfileSelected && enable;
                cmbAudioEncoder.Enabled = testProfileSelected && enable;
                cmbAudioSource.Enabled = testProfileSelected && enable;
                cmbVideoCodec.Enabled = testProfileSelected && enable;
                cmbVideoEncoder.Enabled = testProfileSelected && enable;
                cmbVideoResolution.Enabled = testProfileSelected && enable;
                txtVideoBitrate.Enabled = testProfileSelected && enable && (cmbVideoCodec.SelectedItem != null);
                txtVideoFramerate.Enabled = testProfileSelected && enable && (cmbVideoCodec.SelectedItem != null);
                cmbVideoSource.Enabled = testProfileSelected && enable;
                cmbAudioBitrate.Enabled = testProfileSelected && enable;
                cmbTransport.Enabled = testProfileSelected && enable;

                btnGetStreams.Refresh();
            }));
        }
        private void SetH264Configuration(
            Media.VideoEncoderConfiguration config,
            Media.H264Options options, 
            Media.VideoResolution resolution,
            int? framerate,
            int? bitrate)
        {
            config.Encoding = Media.VideoEncoding.H264;
            config.H264 = new Media.H264Configuration();
            config.H264.GovLength = options.GovLengthRange.Max < 30 ? options.GovLengthRange.Max : 30;
            if (config.H264.GovLength < options.GovLengthRange.Min)
            {
                config.H264.GovLength = options.GovLengthRange.Min;
            }
            if (options.H264ProfilesSupported.Length > 0)
            {
                config.H264.H264Profile = options.H264ProfilesSupported[0];
            }
            if ((config.RateControl == null) && (framerate.HasValue || bitrate.HasValue))
            {
                config.RateControl = new Media.VideoRateControl();
            }
            if (config.RateControl != null)
            {
                config.RateControl.FrameRateLimit = framerate.HasValue ? framerate.Value : options.FrameRateRange.Max;
                config.RateControl.EncodingInterval = options.EncodingIntervalRange.Min;
                if(bitrate.HasValue)
                {
                    config.RateControl.BitrateLimit = bitrate.Value;
                }
            }

            config.Resolution = resolution;
        }
        private void SetJPEGConfiguration(
            Media.VideoEncoderConfiguration config,
            Media.JpegOptions options,
            Media.VideoResolution resolution,
            int? framerate,
            int? bitrate)
        {
            config.Encoding = Media.VideoEncoding.JPEG;
            if ((config.RateControl == null) && (framerate.HasValue || bitrate.HasValue))
            {
                config.RateControl = new Media.VideoRateControl();
            }
            if (config.RateControl != null)
            {
                config.RateControl.FrameRateLimit = framerate.HasValue ? framerate.Value : options.FrameRateRange.Max;
                config.RateControl.EncodingInterval = options.EncodingIntervalRange.Min;
                if (bitrate.HasValue)
                {
                    config.RateControl.BitrateLimit = bitrate.Value;
                }
            }
            config.Resolution = resolution;
        }
        private void SetMPEG4Configuration(
            Media.VideoEncoderConfiguration config,
            Media.Mpeg4Options options,
            Media.VideoResolution resolution,
            int? framerate,
            int? bitrate)
        {
            config.Encoding = Media.VideoEncoding.MPEG4;
            config.MPEG4 = new Media.Mpeg4Configuration();
            config.MPEG4.GovLength = options.GovLengthRange.Max < 30 ? options.GovLengthRange.Max : 30;
            if (config.MPEG4.GovLength < options.GovLengthRange.Min)
            {
                config.MPEG4.GovLength = options.GovLengthRange.Min;
            }
            if (options.Mpeg4ProfilesSupported.Length > 0)
            {
                config.MPEG4.Mpeg4Profile = options.Mpeg4ProfilesSupported[0];
            }
            if ((config.RateControl == null) && (framerate.HasValue || bitrate.HasValue))
            {
                config.RateControl = new Media.VideoRateControl();
            }
            if (config.RateControl != null)
            {
                config.RateControl.FrameRateLimit = framerate.HasValue ? framerate.Value : options.FrameRateRange.Max;
                config.RateControl.EncodingInterval = options.EncodingIntervalRange.Min;
                if (bitrate.HasValue)
                {
                    config.RateControl.BitrateLimit = bitrate.Value;
                }
            }
            config.Resolution = resolution;
        }
        private void SetAudioConfiguration(
            Media.AudioEncoderConfiguration config,
            Media.AudioEncoderConfigurationOption options)
        {
            config.Encoding = options.Encoding;
            if(cmbAudioBitrate.SelectedItem != null)
            {
                config.Bitrate = (int)cmbAudioBitrate.SelectedItem;
            }
            else if ((options.BitrateList != null)&&(options.BitrateList.Length > 0))
            {
                config.Bitrate = options.BitrateList[0];
            }
            if ((options.SampleRateList != null) && (options.SampleRateList.Length > 0))
            {
                config.SampleRate = options.SampleRateList[0];
            }
            
        }
        private void OnPlayVideo()
        {
            MediaProfileWrapper profile = cmbMediaProfile.SelectedItem as MediaProfileWrapper;
            if(profile != null)
            {
                VideoSourceConfigurationWrapper videoSourceConfig = cmbVideoSource.SelectedItem as VideoSourceConfigurationWrapper;
                VideoEncoderConfigurationWrapper videoEncoderConfig = cmbVideoEncoder.SelectedItem as VideoEncoderConfigurationWrapper;
                AudioSourceConfigurationWrapper audioSourceConfig = cmbAudioSource.SelectedItem as AudioSourceConfigurationWrapper;
                AudioEncoderConfigurationWrapper audioEncoderConfig = cmbAudioEncoder.SelectedItem as AudioEncoderConfigurationWrapper;

                Media.TransportProtocol transport = GetTransportProtocol();
                if ((videoSourceConfig != null) && (videoEncoderConfig != null))
                {
                    bool testProfile = IsTestProfile(profile.Profile);
                    if (testProfile)
                    {
                        VideoCodecWrapper codecOptions = cmbVideoCodec.SelectedItem as VideoCodecWrapper;
                        VideoResolutionWrapper resolution = cmbVideoResolution.SelectedItem as VideoResolutionWrapper;
                        int? framerate = null;
                        if (!string.IsNullOrEmpty(txtVideoFramerate.Text))
                        {
                            framerate = int.Parse(txtVideoFramerate.Text);
                        }
                        int? bitrate = null;
                        if(!string.IsNullOrEmpty(txtVideoBitrate.Text))
                        {
                            bitrate = int.Parse(txtVideoBitrate.Text);
                        }
                        if (codecOptions.Encoding == Media.VideoEncoding.H264)
                        {
                            SetH264Configuration(videoEncoderConfig.Configuration, codecOptions.H264, resolution.Resolution, framerate, bitrate);
                        }
                        else if (codecOptions.Encoding == Media.VideoEncoding.JPEG)
                        {
                            SetJPEGConfiguration(videoEncoderConfig.Configuration, codecOptions.Jpeg, resolution.Resolution, framerate, bitrate);
                        }
                        else if (codecOptions.Encoding == Media.VideoEncoding.MPEG4)
                        {
                            SetMPEG4Configuration(videoEncoderConfig.Configuration, codecOptions.Mpeg4, resolution.Resolution, framerate, bitrate);
                        }

                        if((audioEncoderConfig != null)&&(audioEncoderConfig.Configuration != null))
                        {
                            Media.AudioEncoderConfigurationOption audioCodecOptions = cmbAudioCodec.SelectedItem as Media.AudioEncoderConfigurationOption;
                            if (audioCodecOptions != null)
                            {
                                SetAudioConfiguration(audioEncoderConfig.Configuration, audioCodecOptions);
                            }
                        }

                        Controller.GetMediaUri(
                            profile.Profile.token != null ? profile.Profile : null,//if profile was not really created, pass null as parameter
                            videoSourceConfig.Configuration,
                            videoEncoderConfig.Configuration,
                            audioSourceConfig != null ? audioSourceConfig.Configuration : null,
                            audioEncoderConfig != null ? audioEncoderConfig.Configuration : null, 
                            transport);
                    }
                    else
                    {
                        Controller.GetMediaUri(profile.Profile, transport);
                    }
                }
                else
                {
                    ShowPrompt("Select video source and encoder configuration", "Error");
                }
            }
            else
            {
                ShowPrompt("Select media profile", "Error");
            }
        }
        private void btnGetStreams_Click(object sender, EventArgs e)
        {
            try
            {
                if (_videoWindow == null)
                {
                    OnPlayVideo();
                }
                else
                {
                    _videoWindow.CloseWindow();
                    _videoWindow = null;
                    btnGetStreams.Text = "Play Video";
                }
            }
            catch(Exception ex)
            {
                ShowError(ex);
            }
        }
        private void btnGetMediaUrl_Click(object sender, EventArgs e)
        {
            try
            {
                Controller.GetMediaAddress();
            }
            catch(Exception ex)
            {
                ShowError(ex);
            }
        }

        private void buttonGetVideoSources_Click(object sender, EventArgs e)
        {
            try
            {
                _controller.GetVideoSourceConfigurations();
            }
            catch(Exception ex)
            {
                ShowError(ex);
            }
        }

        private void buttonGetVideoEncoders_Click(object sender, EventArgs e)
        {
            try
            {
                _controller.GetVideoEncoderConfigurations();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void buttonGetVideoCodecs_Click(object sender, EventArgs e)
        {
            VideoEncoderConfigurationWrapper encoderConfig = cmbVideoEncoder.SelectedItem as VideoEncoderConfigurationWrapper;
            if(encoderConfig == null)
            {
                ShowPrompt("Select video encoder configuration", "Error");
            }
            else
            {
                try
                {
                    _controller.GetVideoEncoderConfigOptions(encoderConfig.Configuration.token);
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
        }

        private void buttonGetAudioSources_Click(object sender, EventArgs e)
        {
            try
            {
                _controller.GetAudioSourceConfigurations();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void buttonGetAudioEncoders_Click(object sender, EventArgs e)
        {
            try
            {
                _controller.GetAudioEncoderConfigurations();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void buttonGetAudioCodecs_Click(object sender, EventArgs e)
        {
            AudioEncoderConfigurationWrapper encoderConfig = cmbAudioEncoder.SelectedItem as AudioEncoderConfigurationWrapper;
            if (encoderConfig == null)
            {
                ShowPrompt("Select audio encoder configuration", "Error");
            }
            else
            {
                try
                {
                    _controller.GetAudioEncoderConfigOptions(encoderConfig.Configuration.token);
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
        }

        private void cmbVideoEncoder_SelectedIndexChanged(object sender, EventArgs e)
        {
            VideoEncoderConfigurationWrapper config = cmbVideoEncoder.SelectedItem as VideoEncoderConfigurationWrapper;
            if ((config == null) || (config.Configuration.token != _currentVideoEncoder))
            {
                _currentVideoEncoder = config.Configuration.token;
                cmbVideoCodec.Items.Clear();
                cmbVideoResolution.Items.Clear();
                txtVideoBitrate.Text = null;
                txtVideoFramerate.Text = null;
                btnGetStreams.Enabled = _videoWindow != null;
            }
        }

        private void cmbVideoCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbVideoResolution.Items.Clear();
            txtVideoBitrate.Text = null;
            txtVideoFramerate.Text = null;
            VideoCodecWrapper encoder = cmbVideoCodec.SelectedItem as VideoCodecWrapper;
            if(encoder != null)
            {
                Media.VideoResolution[] resolutions = encoder.GetResolutions();
                if(resolutions != null)
                {
                    foreach (Media.VideoResolution resolution in resolutions)
                    {
                        cmbVideoResolution.Items.Add(new VideoResolutionWrapper { Resolution = resolution });
                    }
                }
                Media.IntRange framerate = encoder.GetFrameLimit();
                if(framerate != null)
                {
                    txtVideoFramerate.Text = framerate.Min.ToString();
                }
            }
            if(cmbVideoResolution.Items.Count > 0)
            {
                cmbVideoResolution.SelectedIndex = 0;
            }
        }

        private void cmbAudioEncoder_SelectedIndexChanged(object sender, EventArgs e)
        {
            AudioEncoderConfigurationWrapper config = cmbAudioEncoder.SelectedItem as AudioEncoderConfigurationWrapper;
            if ((config == null) || (config.Configuration.token != _currentAudioEncoder))
            {
                _currentAudioEncoder = config.Configuration.token;
                cmbAudioCodec.Items.Clear();
                cmbAudioBitrate.Items.Clear();
            }
        }

        private void cmbMediaProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            Media.Profile profile = ((MediaProfileWrapper)cmbMediaProfile.SelectedItem).Profile;
            bool testProfileSelected = IsTestProfile(profile);
            
            cmbAudioCodec.Items.Clear();
            cmbAudioEncoder.Items.Clear();
            cmbAudioSource.Items.Clear();
            cmbVideoCodec.Items.Clear();
            cmbVideoEncoder.Items.Clear();
            cmbVideoResolution.Items.Clear();
            cmbVideoSource.Items.Clear();
            cmbAudioBitrate.Items.Clear();
            txtVideoBitrate.Text = null;
            txtVideoFramerate.Text = null;
            if(!testProfileSelected)
            {
                if (profile.VideoSourceConfiguration != null)
                {
                    cmbVideoSource.Items.Add(new VideoSourceConfigurationWrapper() { Configuration = profile.VideoSourceConfiguration });
                    cmbVideoSource.SelectedIndex = 0;
                }
                if (profile.VideoEncoderConfiguration != null)
                {
                    cmbVideoEncoder.Items.Add(new VideoEncoderConfigurationWrapper() { Configuration = profile.VideoEncoderConfiguration });
                    cmbVideoEncoder.SelectedIndex = 0;

                    cmbVideoCodec.Items.Add(profile.VideoEncoderConfiguration.Encoding);
                    cmbVideoCodec.SelectedIndex = 0;

                    cmbVideoResolution.Items.Add(new VideoResolutionWrapper() { Resolution = profile.VideoEncoderConfiguration.Resolution });
                    cmbVideoResolution.SelectedIndex = 0;

                    if (profile.VideoEncoderConfiguration.RateControl != null)
                    {
                        txtVideoBitrate.Text = profile.VideoEncoderConfiguration.RateControl.BitrateLimit.ToString();
                        txtVideoFramerate.Text = profile.VideoEncoderConfiguration.RateControl.FrameRateLimit.ToString();
                    }
                }
                if (profile.AudioSourceConfiguration != null)
                {
                    cmbAudioSource.Items.Add(new AudioSourceConfigurationWrapper() { Configuration = profile.AudioSourceConfiguration });
                    cmbAudioSource.SelectedIndex = 0;
                }
                if (profile.AudioEncoderConfiguration != null)
                {
                    cmbAudioEncoder.Items.Add(new AudioEncoderConfigurationWrapper() { Configuration = profile.AudioEncoderConfiguration });
                    cmbAudioEncoder.SelectedIndex = 0;

                    cmbAudioCodec.Items.Add(profile.AudioEncoderConfiguration.Encoding);
                    cmbAudioCodec.SelectedIndex = 0;

                    cmbAudioBitrate.Items.Add(profile.AudioEncoderConfiguration.Bitrate);
                    cmbAudioBitrate.SelectedIndex = 0;
                }
            }

            EnableControls(true); //refresh controls disabling
        }

        private void btnGetProfiles_Click(object sender, EventArgs e)
        {
            try
            {
                _controller.GetMediaProfiles();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void txtVideoBitrate_Validating(object sender, CancelEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtVideoBitrate.Text))
            {
                int value;
                bool valid = int.TryParse(txtVideoBitrate.Text, out value);
                if (!valid)
                {
                    ShowPrompt("Please enter integer value", "Error");
                    e.Cancel = true;
                }
            }
        }

        private void txtVideoFramerate_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtVideoFramerate.Text))
            {
                int value;
                Media.IntRange range = null;
                bool valid = int.TryParse(txtVideoFramerate.Text, out value);
                VideoCodecWrapper codec = cmbVideoCodec.SelectedItem as VideoCodecWrapper;
                if (codec != null)
                {
                    range = codec.GetFrameLimit();
                    if ((range != null)&&(valid))
                    {
                        valid = (value >= range.Min) && (value <= range.Max);
                    }
                }
                if (!valid)
                {
                    string message = "Please enter integer value";
                    if (range != null)
                    {
                        message += string.Format(" between {0} and {1}", range.Min, range.Max);
                    }
                    ShowPrompt(message, "Error");
                    e.Cancel = true;
                }
            }
        }

        private void cmbAudioCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbAudioBitrate.Items.Clear();
            Media.AudioEncoderConfigurationOption option = cmbAudioCodec.SelectedItem as Media.AudioEncoderConfigurationOption;
            if((option != null)&&(option.BitrateList != null))
            {
                foreach (int bitrate in option.BitrateList)
                {
                    cmbAudioBitrate.Items.Add(bitrate);
                }
                if(cmbAudioBitrate.Items.Count > 0)
                {
                    cmbAudioBitrate.SelectedIndex = 0;
                }
            }
        }
    }
}
