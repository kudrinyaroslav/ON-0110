using System;
using System.ServiceModel;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Definitions.Onvif;

namespace TestTool.Tests.Engine
{
    partial class FeaturesDefinitionProcess
    {
        #region Steps

        protected Capabilities GetCapabilities(CapabilityCategory[] categories, string stepName)
        {
            Capabilities capabilities = null;

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

        protected Capabilities GetCapabilities(CapabilityCategory[] categories)
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
                    services = Client.GetServices(false);
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

        protected ReplayServiceCapabilities GetReplayServiceCapabilities()
        {
            ReplayServiceCapabilities response = null;
            RunStep(() => { response = ReplayClient.GetServiceCapabilities() ; }, "Get Replay service capabilities");
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
            RunStep(() => { response = RecordingClient.GetServiceCapabilities(new GetRecordingServiceCapabilities()).Capabilities; }, "Get Recording service capabilities");
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>All exceptions are handled here. Null is returned, if an error occurs. </remarks>
        //protected DeviceServiceCapabilities GetServiceCapabilities()
        //{
        //    DeviceServiceCapabilities response = null;

        //    bool retry = false;

        //    if (_credentialsProvider.Security == HttpTransport.ChannelControllers.Security.None)
        //    {
        //        BeginStep("Get Service Capabilities (no credentials supplied)");
        //        try
        //        {
        //            response = Client.GetServiceCapabilities();
        //        }
        //        catch (HttpTransport.Exceptions.AccessDeniedException exc)
        //        {
        //            // digest authorization required
        //            _credentialsProvider.Security = HttpTransport.ChannelControllers.Security.Digest;
        //            LogStepEvent("DUT requires Digest authentication");
        //            LogStepEvent("Warning: GetServiceCapabilities should not require authentication");
        //            _warning = true;
        //            retry = true;
        //        }
        //        catch (FaultException exc)
        //        {
        //            LogFault(exc);
        //            if (exc.IsAccessDeniedFault())
        //            {
        //                // WS-username quthentication required
        //                _credentialsProvider.Security = HttpTransport.ChannelControllers.Security.WS;
        //                LogStepEvent("DUT requires WS-UsernameToken authentication");
        //                LogStepEvent("Warning: GetServiceCapabilitiess should not require authentication");
        //                _warning = true;
        //                retry = true;
        //            }
        //            else
        //            {
        //                LogStepEvent("GetServiceCapabilities failed");
        //            }
        //        }
        //        catch (Exception exc)
        //        {
        //            RethrowIfStop(exc);
        //            LogStepEvent(string.Format("GetServiceCapabilities failed ({0})", exc.Message));
        //        }
        //        StepPassed();
        //        DoRequestDelay();
        //    }
        //    else
        //    {
        //        retry = true;
        //    }
        //    if (retry)
        //    {
        //        BeginStep("Get Service Capabilities");
        //        try
        //        {
        //            response = Client.GetServiceCapabilities();
        //        }
        //        catch (Exception exc)
        //        {
        //            RethrowIfStop(exc);
        //            LogStepEvent(string.Format("GetServiceCapabilities failed ({0})", exc.Message));
        //        }

        //        StepPassed();
        //        DoRequestDelay();
        //    }

        //    return response;
        //}

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
            RunStep(() => { options = mediaClient.GetAudioDecoderConfigurationOptions(null, null); },
                    "Get Audio Decoder Configuration Options");
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
            RunStep(() =>
            {
                response = client.GetSnapshotUri(profileToken);
            },
            "Get snapshot URI");
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
            RunStep(() =>
            {
                profile = client.CreateProfile(name, token);
            },
                "Create profile");
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

        bool IsAccessDeniedFault(FaultException ex)
        {
            return ex.IsValidOnvifFault(OnvifFaults.NotAuthorized) ||
                   ex.IsValidOnvifFault(OnvifFaults.SenderNotAuthorized);

        }

        #endregion

    }
}
