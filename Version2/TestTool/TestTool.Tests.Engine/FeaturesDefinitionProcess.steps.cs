using System;
using System.ServiceModel;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Definitions.Onvif;
using System.Collections.Generic;
using TestTool.HttpTransport.Interfaces.Exceptions;
using TestTool.Proxies.Event;
using System.Xml;

namespace TestTool.Tests.Engine
{
    partial class FeaturesDefinitionProcess
    {
        #region Steps

        protected TestTool.Proxies.Onvif.Capabilities GetCapabilities(CapabilityCategory[] categories, string stepName)
        {
            TestTool.Proxies.Onvif.Capabilities capabilities = null;

            bool retry = false;

            BeginStep("GetCapabilities (no credentials supplied)");
            try
            {
                capabilities = Client.GetCapabilities(categories);
            }
            catch (HttpTransport.Interfaces.Exceptions.AccessDeniedException exc)
            {
                // digest authorization required
                _credentialsProvider.Security = Security.Digest;
                LogStepEvent("DUT requires Digest authentication");
                LogStepEvent("Warning: GetCapabilities should not require authentication");
                retry = true;

            }
            catch (FaultException exc)
            {
                LogFault(exc);
                if (IsAccessDeniedFault(exc))
                {
                    // WS-username quthentication required
                    _credentialsProvider.Security = Security.WS;
                    LogStepEvent("DUT requires WS-UsernameToken authentication");
                    LogStepEvent("Warning: GetCapabilities should not require authentication");
                    retry = true;
                }
            }
            catch (Exception exc)
            {
                RethrowIfStop(exc);
                LogStepEvent(string.Format("GetCapabilities failed ({0})", exc.Message));
            }
            StepPassed();
            DoRequestDelay();

            // suppress any exceptions. Return null, if somethig goes wrong.
            if (retry)
            {
                BeginStep("Get Capabilities");
                try
                {
                    capabilities = Client.GetCapabilities(categories);
                }
                catch (FaultException exc)
                {
                    LogFault(exc);
                    LogStepEvent(string.Format("GetCapabilities failed ({0})", exc.Message));
                }
                catch (Exception exc)
                {
                    RethrowIfStop(exc);
                    LogStepEvent(string.Format("GetCapabilities failed ({0})", exc.Message));
                }

                StepPassed();
                DoRequestDelay();
            }

            return capabilities;
        }

        protected TestTool.Proxies.Onvif.Capabilities GetCapabilities(CapabilityCategory[] categories)
        {
            return GetCapabilities(categories, "Get capabilities");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>Never throws any exceptions.</remarks>
        protected Service[] GetServices()
        {
            Service[] services = null;

            bool retry = false;
            bool exit = false;

            if (_credentialsProvider.Security == Security.None)
            {
                BeginStep("GetServices (no credentials supplied)");
                try
                {
                    services = Client.GetServices(true);
                }
                catch (HttpTransport.Interfaces.Exceptions.AccessDeniedException exc)
                {
                    // digest authorization required
                    _credentialsProvider.Security = Security.Digest;
                    LogStepEvent("DUT requires Digest authentication");
                    LogStepEvent("Warning: GetServices should not require authentication");
                    retry = true;

                }
                catch (FaultException exc)
                {
                    LogFault(exc);
                    if (IsAccessDeniedFault(exc))
                    {
                        // WS-username quthentication required
                        _credentialsProvider.Security = Security.WS;
                        LogStepEvent("DUT requires WS-UsernameToken authentication");
                        LogStepEvent("Warning: GetServices should not require authentication");
                        retry = true;
                    }
                    else
                    {
                        LogStepEvent(string.Format("GetServices not supported ({0})", exc.Message));
                        exit = true;
                    }
                }
                catch (Exception exc)
                {
                    RethrowIfStop(exc);
                    LogStepEvent(string.Format("GetServices not supported ({0})", exc.Message));
                    exit = true;
                }
                StepPassed();
                DoRequestDelay();
                if (exit)
                {
                    return null;
                }
            }
            else
            {
                retry = true;
            }


            if (retry)
            {
                BeginStep("Get services");
                try
                {
                    services = Client.GetServices(false);
                }
                catch (Exception exc)
                {
                    if (exc is FaultException)
                    {
                        LogFault(exc as FaultException);
                    }
                    RethrowIfStop(exc);
                    LogStepEvent(string.Format("GetServices not supported ({0})", exc.Message));
                }

                StepPassed();
                DoRequestDelay();
            }

            return services;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>All exceptions are handled here. Null is returned, if an error occurs. </remarks>
        protected DeviceServiceCapabilities GetServiceCapabilities()
        {
            if (null == Client)
            {
                LogStepEvent(string.Format("GetServiceCapabilities failed: cannot connect to Device service"));
                return null;
            }

            DeviceServiceCapabilities response = null;

            bool retry = false;

            if (_credentialsProvider.Security == Security.None)
            {
                BeginStep("Get Service Capabilities (no credentials supplied)");
                try
                {
                    response = Client.GetServiceCapabilities();
                    StepPassed();
                }
                catch (HttpTransport.Interfaces.Exceptions.AccessDeniedException exc)
                {
                    // digest authorization required
                    _credentialsProvider.Security = Security.Digest;
                    LogStepEvent("DUT requires Digest authentication");
                    LogStepEvent("Warning: GetServiceCapabilities shall not require authentication");
                    _warning = true;
                    retry = true;
                    StepPassed();
                }
                catch (FaultException exc)
                {
                    LogFault(exc);
                    if (IsAccessDeniedFault(exc))
                    {
                        // WS-username quthentication required
                        _credentialsProvider.Security = Security.WS;
                        LogStepEvent("DUT requires WS Username token authentication");
                        LogStepEvent("Warning: GetServiceCapabilitiess shall not require authentication");
                        _warning = true;
                        retry = true;
                        StepPassed();
                    }
                    else
                    {
                        LogStepEvent("GetServiceCapabilities failed");
                        StepFailed(exc);
                    }
                }
                catch (Exception exc)
                {
                    RethrowIfStop(exc);
                    LogStepEvent(string.Format("GetServiceCapabilities failed ({0})", exc.Message));
                    StepFailed(exc);
                }
                DoRequestDelay();
            }
            else
            {
                retry = true;
            }
            if (retry)
            {
                BeginStep("Get Service Capabilities");
                try
                {
                    response = Client.GetServiceCapabilities();
                    StepPassed();
                }
                catch (Exception exc)
                {
                    RethrowIfStop(exc);
                    LogStepEvent(string.Format("GetServiceCapabilities failed ({0})", exc.Message));
                    StepFailed(exc);
                }
                DoRequestDelay();
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected MediaServiceCapabilities GetMediaCapabilities()
        {
            Proxies.Onvif.MediaClient client = MediaClient;
            if (client == null)
            {
                return null;
            }

            GetServiceCapabilitiesResponse capabilities = null;

            try
            {
                RunStep(() => { capabilities = client.GetServiceCapabilities(new GetServiceCapabilities()); }, "Get Media Capabilities");
            }
            catch (Exception exc)
            {
                if (exc is FaultException)
                {
                    LogFault(exc as FaultException);
                }
                RethrowIfStop(exc);
                LogStepEvent(string.Format("GetServiceCapabilities not supported ({0})", exc.Message));
                StepFailed(exc);
            }
            DoRequestDelay();
            return capabilities == null ? null : capabilities.Capabilities;
        }

        protected IOServiceCapabilities GetIoCapabilities()
        {
            DeviceIOPortClient client = IoClient;
            if (client == null)
            {
                return null;
            }

            IOServiceCapabilities capabilities = null;
            try
            {

                RunStep(() => { capabilities = client.GetServiceCapabilities(); }, "Get IO capabilities");
            }
            catch (Exception exc)
            {
                RethrowIfStop(exc);
                LogStepEvent(string.Format("GetServiceCapabilities not supported ({0})", exc.Message));
                StepFailed(exc);
            }

            DoRequestDelay();
            return capabilities;
        }

        protected ImagingOptions20 GetImagingOptions(string VideoSourceToken)
        {
            ImagingPortClient client = ImagingClient;
            if (client == null)
            {
                return null;
            }

            ImagingOptions20 options = null;
            try
            {

              RunStep(() => { options = client.GetOptions(VideoSourceToken); }, "Get Imaging Options");
            }
            catch (Exception exc)
            {
                RethrowIfStop(exc);
                LogStepEvent(string.Format("Imaging GetOptions not supported ({0})", exc.Message));
                StepFailed(exc);
            }

            DoRequestDelay();
            return options;
        }

        protected ReplayServiceCapabilities GetReplayServiceCapabilities()
        {
            ReplayServiceCapabilities response = null;
            RunStep(() => { response = ReplayClient.GetServiceCapabilities(); }, "Get Replay service capabilities");
            DoRequestDelay();
            return response;
        }

        protected SearchServiceCapabilities GetSearchServiceCapabilities()
        {
            SearchServiceCapabilities response = null;
            RunStep(() => { response = SearchClient.GetServiceCapabilities(); }, "Get Search service capabilities");
            DoRequestDelay();
            return response;
        }

        protected RecordingServiceCapabilities GetRecordingServiceCapabilities()
        {
            RecordingServiceCapabilities response = null;
            RunStep(() => { response = RecordingClient.GetServiceCapabilities(); }, "Get Recording service capabilities");
            DoRequestDelay();
            return response;
        }

        protected TestTool.Proxies.Event.EventServiceCapabilities GetEventServiceCapabilities()
        {
            TestTool.Proxies.Event.EventServiceCapabilities response = null;
            try
            {
                RunStep(() => { response = EventClient.GetServiceCapabilities(); }, "Get Event service capabilities");
                DoRequestDelay();
            }
            catch (FaultException e)
            {
                LogFault(e);
                StepFailed(e);
            }
            catch (Exception e)
            {
                RethrowIfStop(e);
                LogStepEvent(string.Format("GetServiceCapabilities not supported ({0})", e.Message));
                StepFailed(e);
            }
            return response;
        }

        protected AccessRulesServiceCapabilities GetAccessRulesServiceCapabilities()
        {
            AccessRulesServiceCapabilities response = null;
            RunStep(() => { response = AccessRulesClient.GetServiceCapabilities(); }, "Get Access Rules service capabilities");
            DoRequestDelay();
            return response;
        }

        protected AdvancedSecurityCapabilities GetAdvancedSecurityServiceCapabilities()
        {
            AdvancedSecurityCapabilities response = null;
            RunStep(() => { response = AdvancedSecurityClient.GetServiceCapabilities(); }, "Get Advanced Security service capabilities");
            DoRequestDelay();
            return response;
        }

        protected CredentialServiceCapabilities GetCredentialServiceCapabilities()
        {
            CredentialServiceCapabilities response = null;
            RunStep(() => { response = CredentialPortClient.GetServiceCapabilities(); }, "Get Credential service capabilities");
            DoRequestDelay();
            return response;
        }

        protected ScheduleServiceCapabilities GetScheduleServiceCapabilities()
        {
            ScheduleServiceCapabilities response = null;
            RunStep(() => { response = SchedulePortClient.GetServiceCapabilities(); }, "Get Schedule service capabilities");
            DoRequestDelay();
            return response;
        }

        protected GetRecordingsResponseItem[] GetRecordings()
        {
            RecordingPortClient client = RecordingClient;
            GetRecordingsResponseItem[] response = null;
            RunStep(() => { response = RecordingClient.GetRecordings(); }, "Get Recordings");
            DoRequestDelay();
            return response;
        }

        DeviceInformation GetDeviceInformation()
        {
            DeviceInformation info = new DeviceInformation();

            string information = null;

            // Cannot use ref or out parameter inside an anonymous method, lambda expression, or query expression
            string modelCopy = null;
            string firmwareVersionCopy = null;
            string serialNumberCopy = null;
            string hardwareIdCopy = null;


            bool retry = false;
            bool exit = false;

            if (_credentialsProvider.Security == Security.None)
            {
                BeginStep("Get device information (no credentials supplied)");
                try
                {
                    information = Client.GetDeviceInformation(out modelCopy,
                     out firmwareVersionCopy,
                     out serialNumberCopy,
                     out hardwareIdCopy);
                }
                catch (HttpTransport.Interfaces.Exceptions.AccessDeniedException exc)
                {
                    // digest authorization required
                    _credentialsProvider.Security = Security.Digest;
                    LogStepEvent("DUT requires Digest authentication");
                    retry = true;

                }
                catch (FaultException exc)
                {
                    LogFault(exc);
                    if (IsAccessDeniedFault(exc))
                    {
                        // WS-username quthentication required
                        _credentialsProvider.Security = Security.WS;
                        LogStepEvent("DUT requires WS-UsernameToken authentication");
                        retry = true;
                    }
                    else
                    {
                        LogStepEvent(string.Format("Get device information failed ({0})", exc.Message));
                        exit = true;
                    }
                }
                catch (Exception exc)
                {
                    RethrowIfStop(exc);
                    LogStepEvent(string.Format("Get device information failed ({0})", exc.Message));
                    exit = true;
                }
                StepPassed();
                DoRequestDelay();
                if (exit)
                {
                    return info;
                }
            }
            else
            {
                retry = true;
            }

            if (retry)
            {
                BeginStep("Get device information");
                try
                {
                    information = Client.GetDeviceInformation(out modelCopy,
                     out firmwareVersionCopy,
                     out serialNumberCopy,
                     out hardwareIdCopy);
                }
                catch (Exception exc)
                {
                    if (exc is FaultException)
                    {
                        LogFault(exc as FaultException);
                    }
                    RethrowIfStop(exc);
                    LogStepEvent(string.Format("Get device information failed ({0})", exc.Message));
                }

                StepPassed();
                DoRequestDelay();
            }

            info.FirmwareVersion = firmwareVersionCopy;
            info.HardwareID = hardwareIdCopy;
            info.Manufacturer = information;
            info.Model = modelCopy;
            info.SerialNumber = serialNumberCopy;

            return info;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks> Is used in try-catch block. All exceptions will be handled in this block.</remarks>
        protected NTPInformation GetNTP()
        {
            NTPInformation ntp = null;
            RunStep(() => { ntp = Client.GetNTP(); }, "Get NTP Information");
            DoRequestDelay();
            return ntp;
        }

        protected RelayOutput[] GetRelayOutputs()
        {
            RelayOutput[] outputs = null;
            RunStep(() => { outputs = Client.GetRelayOutputs(); }, "Get Relay Outputs");
            DoRequestDelay();
            return outputs;
        }

        protected void SetRelayOutputSettings(string token, RelayOutputSettings settings)
        {
            RunStep(
                () =>
                {
                    Client.SetRelayOutputSettings(token, settings);
                },
                    string.Format("Set Relay Output settings (IdleState={1}, Mode={0})", settings.IdleState, settings.Mode));

            DoRequestDelay();

        }

        /// <summary>
        /// Get scopes of DUT
        /// </summary>
        /// <returns>Scopes of DUT</returns>
        protected Scope[] GetScopes()
        {
            Scope[] response = null;
            RunStep(() => { response = Client.GetScopes(); }, "Get device scopes");
            DoRequestDelay();
            return response;
        }
        
        protected VideoEncoderConfigurationOptions GetVideoEncoderConfigurationOptions()
        {
            MediaClient mediaClient = MediaClient;
            VideoEncoderConfigurationOptions options = null;
            RunStep(() => { options = mediaClient.GetVideoEncoderConfigurationOptions(null, null); }, "Get Video Encoder Configuration Options");
            DoRequestDelay();
            return options;
        }

        protected VideoEncoderConfiguration[] GetVideoEncoderConfigurations()
        {
            MediaClient mediaClient = MediaClient;
            VideoEncoderConfiguration[] configurations = null;
            RunStep(() => { configurations = mediaClient.GetVideoEncoderConfigurations(); }, "Get Video Encoder Configurations");
            DoRequestDelay();
            if (configurations != null)
            {
                LogTestEvent(string.Format("{0} configurations found", configurations.Length) + Environment.NewLine);
            }
            return configurations;
        }

        protected AudioOutput[] GetAudioOutputs()
        {
            MediaClient mediaClient = MediaClient;
            AudioOutput[] outputs = null;
            RunStep(() => { outputs = mediaClient.GetAudioOutputs(); }, "Get Audio Outputs");
            DoRequestDelay();
            return outputs;
        }

        protected AudioEncoderConfigurationOptions GetAudioEncoderConfigurationOptions()
        {
            MediaClient mediaClient = MediaClient;
            AudioEncoderConfigurationOptions options = null;
            RunStep(() => { options = mediaClient.GetAudioEncoderConfigurationOptions(null, null); }, "Get Audio Encoder Configuration Options");
            DoRequestDelay();
            return options;
        }

        protected Profile[] GetProfiles()
        {
            MediaClient client = MediaClient;
            if (client == null)
            {
                return null;
            }
            Profile[] profiles = null;
            RunStep(() => { profiles = client.GetProfiles(); }, "Get Profiles");
            DoRequestDelay();
            return profiles;
        }

        protected void AddPtzConfiguration(string profileToken, string configurationToken)
        {
            MediaClient client = MediaClient;
            RunStep(() => { client.AddPTZConfiguration(profileToken, configurationToken); }, "Add PTZ configuration");
            DoRequestDelay();
        }

        protected void RemovePtzConfiguration(string profileToken)
        {
            MediaClient client = MediaClient;
            RunStep(() => { client.RemovePTZConfiguration(profileToken); }, "Remove PTZ configuration");
            DoRequestDelay();
        }

        protected void AddVideoEncoderConfiguration(string profileToken, string configurationToken)
        {
            MediaClient client = MediaClient;
            RunStep(() => { client.AddVideoEncoderConfiguration(profileToken, configurationToken); }, "Add Video Encoder configuration");
            DoRequestDelay();
        }

        protected AudioDecoderConfigurationOptions GetAudioDecoderConfigurationOptions()
        {
            MediaClient mediaClient = MediaClient;
            AudioDecoderConfigurationOptions options = null;
            RunStep(() => { options = mediaClient.GetAudioDecoderConfigurationOptions(null, null); }, "Get Audio Decoder Configuration Options");
            DoRequestDelay();
            return options;
        }

        protected void RemoveVideoEncoderConfiguration(string profileToken)
        {
            MediaClient client = MediaClient;
            RunStep(() => { client.RemoveVideoEncoderConfiguration(profileToken); }, "Remove Video Encoder configuration");
            DoRequestDelay();
        }

        protected void AddVideoSourceConfiguration(string profileToken, string configurationToken)
        {
            MediaClient client = MediaClient;
            RunStep(() => { client.AddVideoSourceConfiguration(profileToken, configurationToken); }, "Add Video Source configuration");
            DoRequestDelay();
        }

        protected void RemoveVideoSourceConfiguration(string profileToken)
        {
            MediaClient client = MediaClient;
            RunStep(() => { client.RemoveVideoSourceConfiguration(profileToken); }, "Remove Video Source configuration");
            DoRequestDelay();
        }

        /// <summary>
        /// Returns snapshot uri from DUT
        /// </summary>
        /// <param name="profileToken">Get snapshot parameters</param>
        /// <returns>Snapshot uri</returns>
        protected MediaUri GetSnapshotUri(string profileToken)
        {
            MediaClient client = MediaClient;
            MediaUri response = null;
            RunStep(() => {response = client.GetSnapshotUri(profileToken);}, "Get snapshot URI");
            DoRequestDelay();
            return response;
        }

        /// <summary>
        /// Creates new media profile
        /// </summary>
        /// <param name="name">Name of new profile</param>
        /// <param name="token">Token of new profile</param>
        /// <returns>Created profile</returns>
        protected Profile CreateProfile(string name, string token)
        {
            MediaClient client = MediaClient;
            Profile profile = null;
            RunStep(() => {profile = client.CreateProfile(name, token);},"Create profile");
            DoRequestDelay();
            return profile;
        }

        /// <summary>
        /// Deletes media profile from DUT
        /// </summary>
        /// <param name="token">Token of profile to be deleted</param>
        protected void DeleteProfile(string token)
        {
            MediaClient client = MediaClient;
            RunStep(() => { client.DeleteProfile(token); },
                string.Format(string.Format("Delete profile {0}", token)));

            DoRequestDelay();
        }

        /// <summary>
        /// Retrieves lists of video source configurations from DUT
        /// </summary>
        /// <returns>Array of video source configurations</returns>
        protected VideoSourceConfiguration[] GetVideoSourceConfigurations()
        {
            MediaClient client = MediaClient;
            VideoSourceConfiguration[] configurations = null;
            RunStep(() => { configurations = client.GetVideoSourceConfigurations(); }, "Get Video Source Configurations");
            DoRequestDelay();
            if (configurations != null)
            {
                LogStepEvent(string.Format("{0} configurations found", configurations.Length) + Environment.NewLine);
            }
            return configurations;
        }


        /// <summary>
        /// Retrieves lists of video sources form DUT
        /// </summary>
        /// <returns>Array of video sources</returns>
        protected VideoSource[] GetVideoSources()
        {
            MediaClient client = MediaClient;
            VideoSource[] sources = null;
            RunStep(() => { sources = client.GetVideoSources(); }, "Get Video Sources");
            DoRequestDelay();
            if (sources != null)
            {
                LogStepEvent(string.Format("{0} sources found", sources.Length) + Environment.NewLine);
            }
            return sources;
        }
        protected VideoSource[] GetVideoSourcesFromMediaOrIO()
        {
          VideoSource[] sources = null;
          if (MediaClient != null)
          {
            MediaClient client = MediaClient;
            RunStep(() => { sources = client.GetVideoSources(); }, "Get Video Sources");
            DoRequestDelay();
            if (sources != null)
            {
              LogStepEvent(string.Format("{0} sources found", sources.Length) + Environment.NewLine);
            }
            return sources;
          }
          if (IoClient != null)
          {
            DeviceIOPortClient client = IoClient;
            RunStep(() => { sources = client.GetVideoSources(); }, "Get Video Sources");
            DoRequestDelay();
            if (sources != null)
            {
              LogStepEvent(string.Format("{0} sources found", sources.Length) + Environment.NewLine);
            }
            return sources;
          }
          return sources;
        }

        /// <summary>
        /// Retrieves lists of video source configurations compatible with specified profile from DUT
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <returns>Array of video source configurations</returns>
        protected VideoSourceConfiguration[] GetCompatibleVideoSourceConfigurations(string profile)
        {
            Proxies.Onvif.MediaClient client = MediaClient;
            VideoSourceConfiguration[] configs = null;
            RunStep(() => { configs = client.GetCompatibleVideoSourceConfigurations(profile); },
                string.Format("Get compatible video sources for profile '{0}'", profile));
            DoRequestDelay();

            return configs;
        }

        #endregion

        #region PTZ

        protected PTZNode[] GetPtzNodes()
        {
            PTZClient client = PtzClient;
            PTZNode[] nodes = null;
            RunStep(() => { nodes = client.GetNodes(); }, "Get PTZ Nodes");
            DoRequestDelay();
            return nodes;
        }

        protected PTZNode GetPtzNode()
        {
            PTZClient client = PtzClient;
            PTZNode node = null;
            RunStep(() => { node = client.GetNode(_ptzNode); }, "Get PTZ Node");
            DoRequestDelay();
            return node;
        }

        protected PTZConfiguration[] GetPtzConfigurations()
        {
            PTZClient client = PtzClient;
            PTZConfiguration[] configurations = null;
            RunStep(() => { configurations = client.GetConfigurations(); }, "Get PTZ Configurations");
            DoRequestDelay();
            if (configurations != null)
            {
                LogStepEvent(string.Format("{0} configurations found", configurations.Length));
            }
            return configurations;
        }

        protected void SetHomePostion(string profile)
        {
            PTZClient client = PtzClient;
            RunStep(() => { client.SetHomePosition(profile); }, "Set Home position");
            DoRequestDelay();
        }

        #endregion

        #region Door Control

        protected string GetDoorInfoList(int? limit, string offset, out DoorInfo[] list)
        {
            return GetDoorInfoList(limit, offset, out list, "Get DoorInfo list");
        }

        protected string GetDoorInfoList(int? limit, string offset, out DoorInfo[] list, string stepName)
        {
            DoorControlPortClient client = DoorControlClient;
            string nextReference = null;
            DoorInfo[] infos = null;
            RunStep(() => { nextReference = client.GetDoorInfoList(limit, offset, out infos); }, stepName);
            DoRequestDelay();
            list = infos;
            return nextReference;
        }

        protected AccessControlServiceCapabilities GetAccessControlServiceCapabilities()
        {
            PACSPortClient client = AccessControlClient;
            AccessControlServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = client.GetServiceCapabilities(); }, "Get AccessControl service capabilities");
            return capabilities;
        }

        #endregion

        #region Access control

        protected string GetAccessPointInfoList(int? limit, string offset, out AccessPointInfo[] list)
        {
            return GetAccessPointInfoList(limit, offset, out list, "Get AccessPointInfo list");
        }

        protected string GetAccessPointInfoList(int? limit, string offset, out AccessPointInfo[] list, string stepName)
        {
            PACSPortClient client = AccessControlClient;
            string nextReference = null;
            AccessPointInfo[] infos = null;
            RunStep(() => { nextReference = client.GetAccessPointInfoList(limit, offset, out infos); }, stepName);
            DoRequestDelay();
            list = infos;
            return nextReference;
        }


        protected string GetAreaInfoList(int? limit, string offset, out AreaInfo[] list)
        {
            return GetAreaInfoList(limit, offset, out list, "Get AreaInfo list");
        }

        protected string GetAreaInfoList(int? limit, string offset, out AreaInfo[] list, string stepName)
        {
            PACSPortClient client = AccessControlClient;
            string nextReference = null;
            AreaInfo[] infos = null;
            RunStep(() => { nextReference = client.GetAreaInfoList(limit, offset, out infos); }, stepName);
            DoRequestDelay();
            list = infos;
            return nextReference;
        }



        #endregion

        #region Events

        protected string[] GetEventProperties(out bool FixedTopicSet,
            out TopicSetType TopicSet,
            out string[] TopicExpressionDialect,
            out string[] MessageContentFilterDialect,
            out string[] ProducerPropertiesFilterDialect,
            out string[] MessageContentSchemaLocation,
            out XmlElement[] Any)
        {
            EventPortTypeClient client = EventClient;

            string[] response = null;

            bool fixedTopicSetCopy = false;
            TopicSetType topicSetCopy = null;
            string[] topicExpressionDialectCopy = null;
            string[] messageContentFilterDialectCopy = null;
            string[] producerPropertiesFilterDialectCopy = null;
            string[] messageContentSchemaLocationCopy = null;
            XmlElement[] anyCopy = null;

            RunStep(() =>
            {
                response = client.GetEventProperties(out fixedTopicSetCopy,
                                                     out topicSetCopy,
                                                     out topicExpressionDialectCopy,
                                                     out messageContentFilterDialectCopy,
                                                     out producerPropertiesFilterDialectCopy,
                                                     out messageContentSchemaLocationCopy,
                                                     out anyCopy);
            },
                         "Get Event Properties");

            FixedTopicSet = fixedTopicSetCopy;
            TopicSet = topicSetCopy;
            TopicExpressionDialect = topicExpressionDialectCopy;
            MessageContentFilterDialect = messageContentFilterDialectCopy;
            ProducerPropertiesFilterDialect = producerPropertiesFilterDialectCopy;
            MessageContentSchemaLocation = messageContentSchemaLocationCopy;
            Any = anyCopy;

            return response;
        }

        RecordingInformation GetRecordingInformation(string token)
        {
            RecordingInformation info = null;
            Proxies.Onvif.SearchPortClient client = SearchClient;
            RunStep(() => { info = client.GetRecordingInformation(token); },
                string.Format("Get recording information (token = '{0}')", token));
            DoRequestDelay();
            return info;
        }

        FindPTZPositionResponse FindPTZPosition(System.DateTime startPoint, 
            System.DateTime? endPoint, 
            SearchScope scope, 
            PTZPositionFilter searchFilter, 
            int? maxMatches, 
            string keepAliveTime)
        {
            FindPTZPositionResponse response = null;
            Proxies.Onvif.SearchPortClient client = SearchClient;
            RunStep(() => { response = client.FindPTZPosition(startPoint, endPoint, scope, searchFilter, maxMatches, keepAliveTime); },
                "Send FindPTZPosition request");
            DoRequestDelay();
            return response;
        }

        void StopPTZPositionSearch(string token)
        {
            Proxies.Onvif.SearchPortClient client = SearchClient;
            RunStep(() => {  client.EndSearch(token); }, "EndSearch");
            DoRequestDelay();
        }

        #endregion

        bool IsAccessDeniedFault(FaultException ex)
        {
            return ex.IsValidOnvifFault(OnvifFaults.NotAuthorized) ||
                   ex.IsValidOnvifFault(OnvifFaults.SenderNotAuthorized);
        }

        delegate string GetListMethod<T>(int? limit, string offset, out T[] list);

        List<T> GetFullList<T>(GetListMethod<T> getList,
            int? chunk)
        {
            List<T> fullList = new List<T>();
            if (chunk > 0 || !chunk.HasValue)
            {
                string currentOffset = null;
                while (true)
                {
                    T[] portion = null;
                    currentOffset = getList(chunk, currentOffset, out portion);

                    if (portion != null)
                    {
                        fullList.AddRange(portion);
                    }
                    if (string.IsNullOrEmpty(currentOffset))
                    {
                        break;
                    }
                }
            }
            return fullList;
        }

    }
}
