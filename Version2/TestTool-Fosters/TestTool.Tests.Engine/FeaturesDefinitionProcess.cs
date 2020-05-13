using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Features;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.HttpTransport.Exceptions;
using System.Xml;
using TestTool.Tests.Common.CommonUtils;

namespace TestTool.Tests.Engine
{
    public partial class FeaturesDefinitionProcess : BaseServiceTest<Device, DeviceClient>
    {
        public FeaturesDefinitionProcess(TestLaunchParam param)
            : base(param)
        {
            _secureOperation = param.SecureMethod;
            _ptzNode = param.PTZNodeToken;

            _featuresSet = FeaturesSet.CreateFeaturesSet();
        }
        

        #region FeatureDefinition

        public void RunFeatureDefinition()
        {
            RunTest( () =>
                 {
                     _features = new List<Feature>();

                     Capabilities capabilities = null;
                     capabilities = GetCapabilities(null);

                     Service[] services = GetServices();
                     FeatureChecked(Feature.GetServices, services != null);

                     BeginStep("Check GetCapabiilities and GetServices");

                     LogAndNotify(Feature.GetCapabilities, "GetCapabilities", capabilities != null);

                     bool version21 = (services != null);

                     if (version21)
                     {
                         LogStepEvent("GetServices: SUPPORTED");
                         LogUsage("Use GetServices and GetServiceCapabilities for defining features");
                     }
                     else
                     {
                         LogStepEvent("GetServices: NOT SUPPORTED");
                     }

                     if (capabilities == null && services == null)
                     {
                         foreach (FeatureNode node in _featuresSet.Nodes)
                         {
                             // skip GetCapabilities && GetServices
                             if (node.Feature == Feature.DeviceService)
                             {
                                 SetUndefined(_featuresSet.FindNode(Feature.Network));
                                 SetUndefined(_featuresSet.FindNode(Feature.System));
                                 SetUndefined(_featuresSet.FindNode(Feature.DeviceIO));
                             }
                             else
                             {
                                 SetUndefined(node);
                             }
                         }

                         LogStepEvent("GetCapabilities not supported - unable to define other features.");
                         LogStepEvent("Neither GetCapabilities, nor GetServices is supported");
                         // don't exit  - we have DeviceInformation definition and scope definition
                     }

                     StepPassed();

                     DeviceServiceCapabilities serviceCapabilities = null;

                     if (version21)
                     {
                         serviceCapabilities = GetServiceCapabilities();
                     }

                     if (capabilities != null || serviceCapabilities != null)
                     {
                         DefineDeviceFeatures(capabilities, serviceCapabilities);

                         LogStepEvent("Events service is MANDATORY");
                         FeatureChecked(Feature.EventsService, true);
                         LogTestEvent(string.Empty);

                         DefineMediaFeatures(capabilities, services);

                         if (_mediaSupported)
                         {
                             MediaServiceCapabilities mediaCapabilities = null;
                             if (version21)
                             {
                                 mediaCapabilities = GetMediaCapabilities();
                             }
                             // GetServices supported, GetMediaCapabilities returned error => 
                             // streaming will be defined via Capabilities
                             if (capabilities != null || mediaCapabilities != null)
                             {
                                 DefineStreamingCapabilities(capabilities, mediaCapabilities);
                             }
                             else
                             {
                                 LogAssumeSupported("Streaming features");
                                 SetUndefined(_featuresSet.FindNode(Feature.RTPRTSPTCP));
                                 SetUndefined(_featuresSet.FindNode(Feature.RTPMulticastUDP));
                                 SetUndefined(_featuresSet.FindNode(Feature.SnapshotUri));
                             }
                             DefineAudioOutputFeatures();
                         }

                         DefineIoCapabilities(capabilities, services);

                         DefinePtzFeatures(capabilities, services);

                         DefineImagingFeatures(capabilities, services);

                         DefineAnalyticsFeatures(capabilities, services);
                     
                     }

                     if (services != null)
                     {
                         DefineAccessControlCapabilities(services);
                         DefineDoorControlCapabilities(services);
                                                                  
                         Service eventsService = Common.CommonUtils.Extensions.FindService(services, OnvifService.EVENTS);
                         _eventsServiceAddress = eventsService.XAddr;

                         DefinePACSEvents();
                     }
                     else 
                     {
                         TransmitUnsupported(Feature.DoorControlService);
                         TransmitUnsupported(Feature.AccessControlService);
                     }


                     DefineDeviceScopes();

                     DeviceInformation info = GetDeviceInformation();

                     if (DeviceInformationReceived != null)
                     {
                         info.Capabilities = capabilities;
                         info.Services = services;
                         DeviceInformationReceived(info);
                     }

                     if (Warning)
                     {
                         LogTestEvent("Some error occurred during feature definition process. ");
                     }
                 }, 
                 () =>
                     {
                         CloseClients();
                     });
        }

        void DefineDeviceScopes()
        {
            LogTestEvent("Define device scope(s)" + Environment.NewLine);

            Security security = _credentialsProvider.Security;
            if (security == Security.None)
            {
                // use WS for futher feature definition, if no security has been defined.
                _credentialsProvider.Security = Security.WS;
            }

            List<string> _scopes = new List<string>();

            Scope[] scopes = null;
            List<string> strScopes = new List<string>();
            try
            {
                scopes = GetScopes();

                if (scopes != null)
                {
                    foreach (Scope scope in scopes)
                    {
                        strScopes.Add(scope.ScopeItem);
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
                LogTestEvent("Warning: an error occurred during scopes definition" + Environment.NewLine);
                return;
            }

            _scopes.AddRange(strScopes);
            if (ScopeDefined != null)
            {
                foreach (string scope in strScopes)
                {
                    ScopeDefined(scope);
                }
            }

            BeginStep("Check scopes");

            foreach (string scope in strScopes)
            {
                LogStepEvent(scope);
            }

            StepPassed();
        }

        void DefineDeviceFeatures(Capabilities capabilities, DeviceServiceCapabilities serviceCapabilities)
        {
            // system
            bool systemLogging = false;
            bool byeSupported = false;

            //network
            bool ntpSupported = false;
            bool ipv6Supported = false;
            bool dhcpv6Supported = false;
            bool zcSupported = false;
            bool dynamicDns = false;
            bool ipFilter = false;

            // io
            bool? relayOutputs = null;

            //security
            bool wsuSupported = false;
            bool digestSupported = false;

            BeginStep("Analyze Device Service capabilities ");

            if (serviceCapabilities != null)
            {
                LogUsage("Use GetServiceCapabilities response");

                if (serviceCapabilities.Network != null)
                {
                    if (serviceCapabilities.Network.NTPSpecified)
                    {
                        ntpSupported = (serviceCapabilities.Network.NTP > 0);
                    }
                    else
                    {
                        LogStepEvent("NTP not specified");
                    }
                    if (serviceCapabilities.Network.IPVersion6Specified)
                    {
                        ipv6Supported = serviceCapabilities.Network.IPVersion6;
                    }
                    else
                    {
                        LogStepEvent("IPv6 support not specified");
                    }

                    if (serviceCapabilities.Network.ZeroConfigurationSpecified)
                    {
                        zcSupported = serviceCapabilities.Network.ZeroConfiguration;
                    }
                    else
                    {
                        LogStepEvent("Zero Configuration not specified");
                    }

                    if (serviceCapabilities.Network.DynDNSSpecified)
                    {
                        dynamicDns = serviceCapabilities.Network.DynDNS;
                    }
                    else
                    {
                        LogStepEvent("Dynamic DNS not specified");
                    }

                    if (serviceCapabilities.Network.IPFilterSpecified)
                    {
                        ipFilter = serviceCapabilities.Network.IPFilter;
                    }
                    else
                    {
                        LogStepEvent("IP Filter not specified");
                    }
                }
                else
                {
                    LogStepEvent("Network field is empty");
                }

                if (serviceCapabilities.System != null)
                {
                    byeSupported = serviceCapabilities.System.DiscoveryBye;
                    if (serviceCapabilities.System.SystemLoggingSpecified)
                    {
                        systemLogging = serviceCapabilities.System.SystemLogging;
                    }
                    else
                    {
                        LogStepEvent("Logging feature not specified");
                    }
                }
                else
                {
                    LogStepEvent("System field is empty");
                }

                if (serviceCapabilities.Security != null)
                {
                    if (serviceCapabilities.Security.UsernameTokenSpecified)
                    {
                        wsuSupported = serviceCapabilities.Security.UsernameToken;
                    }
                    else
                    {
                        LogStepEvent("WS-Username token feature not specified");
                    }
                    if (serviceCapabilities.Security.HttpDigestSpecified)
                    {
                        digestSupported = serviceCapabilities.Security.HttpDigest;
                    }
                    else
                    {
                        LogStepEvent("Digest authentication feature not specified");
                    }
                }
                else
                {
                    LogStepEvent("Security field is empty");
                }

                if (capabilities != null)
                {
                    if (capabilities.Device == null)
                    {
                        // ERROR - relay outputs undefined
                        relayOutputs = false;
                    }
                    else
                    {
                        if (capabilities.Device.IO != null)
                        {
                            if (capabilities.Device.IO.RelayOutputsSpecified)
                            {
                                /*
                                 2)	If GetCapabilitiesResponse doesn’t include Device.IO.RelayOutput element 
                                 * or this element is equal to 0, Relay Outputs feature for Device Service 
                                 * doesn’t supported by the DUT. Skip other steps for I/O features defining 
                                 * for Device Service.
                                 */
                                relayOutputs = capabilities.Device.IO.RelayOutputs > 0;
                            }
                            else
                            {
                                relayOutputs = false;
                            }
                        }
                        else
                        {
                            relayOutputs = false;
                            LogStepEvent("IO field is empty");
                        }
                    }
                }
                else
                {
                    /*
                     1)	If GetCapabilities feature is not supported by the DUT, Relay Outputs feature 
                     * for Device Service is not supported by the DUT. Skip other steps for I/O features 
                     * defining for Device Service.
                     */
                    relayOutputs = false;
                }
            }
            else
            {
                LogUsage("Use GetCapabilities response");

                //
                // USE GETCAPABILITIES
                //                
                if (capabilities.Device == null)
                {
                    LogStepEvent("Device field is empty");
                    // all related unsupported
                }
                else
                {
                    wsuSupported = true;

                    if (capabilities.Device.Network != null)
                    {
                        if (capabilities.Device.Network.IPVersion6Specified)
                        {
                            ipv6Supported = capabilities.Device.Network.IPVersion6;
                        }
                        if (capabilities.Device.Network.ZeroConfigurationSpecified)
                        {
                            zcSupported = capabilities.Device.Network.ZeroConfiguration;
                        }
                        if (capabilities.Device.Network.DynDNSSpecified)
                        {
                            dynamicDns = capabilities.Device.Network.DynDNS;
                        }
                        if (capabilities.Device.Network.IPFilterSpecified)
                        {
                            ipFilter = capabilities.Device.Network.IPFilter;
                        }
                    }
                    else
                    {
                        LogStepEvent("Device.Network field is empty");
                    }

                    if (capabilities.Device.System != null)
                    {
                        systemLogging = capabilities.Device.System.SystemLogging;
                        byeSupported = capabilities.Device.System.DiscoveryBye;
                    }
                    else
                    {
                        LogStepEvent("Device.System field is empty");
                    }

                    if (capabilities.Device.IO != null)
                    {
                        if (capabilities.Device.IO.RelayOutputsSpecified)
                        {
                            relayOutputs = capabilities.Device.IO.RelayOutputs > 0;
                        }
                        else
                        {
                            relayOutputs = false;
                        }
                    }
                    else
                    {
                        relayOutputs = false;
                        LogStepEvent("IO field is empty");
                    }
                }
            }
            StepPassed();

            BeginStep("Define Network features");
            if (serviceCapabilities != null)
            {
                LogAndNotify(Feature.NTP, "NTP", ntpSupported);
            }
            LogAndNotify(Feature.IPv6, "IPv6", ipv6Supported);
            LogAndNotify(Feature.ZeroConfiguration, "Zero Configuration", zcSupported);
            LogAndNotify(Feature.DynamicDNS, "Dynamic DNS", dynamicDns);
            LogAndNotify(Feature.IPFilter, "IP Filter", ipFilter);
            StepPassed();

            BeginStep("Define Security capabilities");
            LogAndNotify(Feature.WSU, "WS-UserName token", wsuSupported);
            if (serviceCapabilities != null)
            {
                // otherwise check Digest using separate method
                LogAndNotify(Feature.Digest, "Digest", digestSupported);
            }
            StepPassed();

            if (serviceCapabilities == null)
            {
                CheckDigestSupport();
                CheckNTPSupport();
            }
            else
            {
                if (digestSupported)
                {
                    _credentialsProvider.Security = Security.Digest;
                }
                else
                {
                    _credentialsProvider.Security = Security.WS;
                }
            }

            BeginStep("Define Discovery features");
            LogAndNotify(Feature.BYE, "Bye message", byeSupported);
            StepPassed();

            BeginStep("Define Logging features");
            LogAndNotify(Feature.SystemLogging, "Logging", systemLogging);
            StepPassed();

            BeginStep("Define Device IO features");
            if (relayOutputs.HasValue)
            {
                LogAndNotify(Feature.DeviceIORelayOutputs, "Device Service\\IO\\Relay Outputs", relayOutputs.Value);
            }
            else
            {
                LogAssumeSupported("Device Service\\IO\\Relay Outputs");
                SetUndefined(_featuresSet.FindNode(Feature.DeviceIORelayOutputs));
            }
            StepPassed();

            if (relayOutputs.HasValue)
            {
                if (relayOutputs.Value)
                {
                    DefineDeviceIOFeatures();
                }
                else
                {
                    TransmitUnsupported(Feature.DeviceIORelayOutputs);
                }
            }
        }

        void CheckNTPSupport()
        {
            LogTestEvent("Check NTP support by requesting NTP information");
            LogTestEvent(string.Empty);

            try
            {
                NTPInformation ntp = GetNTP();
                LogAndNotify(Feature.NTP, "NTP", true);
            }
            catch (FaultException exc)
            {
                StepPassed();
                LogAndNotify(Feature.NTP, "NTP", false);
                LogTestEvent(string.Empty);
            }
            catch (Exception exc)
            {
                HandleException(exc);
                LogTestEvent("Warning: an error occurred during checking NTP functionality" + Environment.NewLine);
                LogAssumeSupported("NTP");
                ReportUndefined(Feature.NTP);
            }
        }

        void CheckDigestSupport()
        {
            LogTestEvent("Check Digest authentication support by sending request");
            LogTestEvent(string.Empty);

            bool digestSupported = false;

            BeginStep("Check which method is to be used to define Digest/WS-Username support");
            System.Reflection.MethodInfo mi = Client.GetType().GetMethod(_secureOperation);
            LogStepEvent(string.Format("Use {0}", _secureOperation));
            object[] parameters = null;

            if (_secureOperation == "GetDeviceInformation")
            {
                parameters = new string[4];
            }
            else if (_secureOperation == "GetCapabilities")
            {
                parameters = new object[] { new CapabilityCategory[] { CapabilityCategory.All } };
            }

            StepPassed();

            bool exception = false;
            // backup security, if has been defined aleardy.
            Security security = _credentialsProvider.Security;
            if (security == Security.None)
            {
                // use WS for futher feature definition, if no security has been defined.
                security = Security.WS;
            }
            _credentialsProvider.Security = Security.None;
            try
            {
                RunStep(
                    () => { mi.Invoke(Client, parameters); },
                    string.Format("Invoke {0} without credentials supplied", _secureOperation));
            }
            catch (Exception exc)
            {
                exception = true;
                Exception inner = exc.InnerException;
                bool handled = false;
                if (inner != null)
                {
                    if (inner is AccessDeniedException)
                    {
                        // digest 
                        digestSupported = true;
                        handled = true;
                        // Turn ON Digest
                        _credentialsProvider.Security = Security.Digest;

                        LogStepEvent("The DUT requires Digest authentication");
                        StepPassed();
                    }
                    else if (inner is FaultException)
                    {
                        // digest authentication NOT required
                        // may be WSU
                        FaultException fault = inner as FaultException;
                        LogFault(fault);
                        handled = true;
                        if (
                            fault.IsValidOnvifFault(OnvifFaults.NotAuthorized) ||
                            fault.IsValidOnvifFault(OnvifFaults.SenderNotAuthorized))
                        {
                            // WSU
                            LogStepEvent("The DUT requires WS-Username token authentication");
                            // Turn ON WS-username
                            _credentialsProvider.Security = Security.WS;
                        }
                        else
                        {
                            _warning = true;
                            _credentialsProvider.Security = security;
                            LogTestEvent("Warning: no security-related error occurred when sending request without credentials supplied.");
                        }
                        StepPassed();
                    }
                }

                if (!handled)
                {
                    HandleException(exc.InnerException ?? exc);
                    _credentialsProvider.Security = security;
                    LogTestEvent("Warning: unexpected error occurred during checking Digest support" + Environment.NewLine);
                }
            }
            finally
            {
                DoRequestDelay();
            }

            if (!exception)
            {
                _warning = true;
                LogTestEvent("Warning: no error occurred when sending request without credentials supplied. Unable to check Digest support." + Environment.NewLine);
                _credentialsProvider.Security = security;
            }

            LogAndNotify(Feature.Digest, "Digest authentication", digestSupported);

            LogTestEvent(string.Empty);
        }

        void DefineDeviceIOFeatures()
        {
            RelayOutput[] outputs = null;

            //
            // Device IO\\Relay Outputs feature is supported
            // Child features might be unsupported or undefined.
            //
            try
            {
                outputs = GetRelayOutputs();
            }
            catch (Exception exc)
            {
                HandleException(exc);
                LogTestEvent("GetRelayOutputs failed. Assume all sub-features SUPPORTED ");
                SetUndefined(_featuresSet.FindNode(Feature.DeviceIORelayOutputsBistable));
                SetUndefined(_featuresSet.FindNode(Feature.DeviceIORelayOutputsMonostable));
                return;
            }

            if (outputs == null || outputs.Length == 0)
            {
                LogTestEvent("The DUT returned no Relay outputs. Assume all sub-features SUPPORTED");
                _warning = true;
                SetUndefined(_featuresSet.FindNode(Feature.DeviceIORelayOutputsBistable));
                SetUndefined(_featuresSet.FindNode(Feature.DeviceIORelayOutputsMonostable));
                return;
            }

            string output = outputs[0].token;

            RelayOutputSettings outputSettings = new RelayOutputSettings();
            outputSettings.DelayTime = "PT30S";

            bool openSupported = false;
            bool closedSupported = false;

            Func<RelayOutputSettings, bool> set = new Func<RelayOutputSettings, bool>(
                (settings) =>
                {
                    try
                    {
                        SetRelayOutputSettings(output, settings);
                        return true;
                    }
                    catch (Exception exc)
                    {
                        HandleException(exc);
                        return false;
                    }

                }
                );

            Action check = new Action(
                () =>
                {
                    outputSettings.IdleState = RelayIdleState.closed;
                    closedSupported = set(outputSettings);

                    outputSettings.IdleState = RelayIdleState.open;
                    openSupported = set(outputSettings);
                });


            outputSettings.Mode = RelayMode.Monostable;
            check();

            LogAndNotify(Feature.DeviceIORelayOutputsMonostableClosed, "Device Service\\IO\\Relay Outputs\\Monostable\\Closed", closedSupported);
            LogAndNotify(Feature.DeviceIORelayOutputsMonostableOpen, "Device Service\\IO\\Relay Outputs\\Monostable\\Open", openSupported);
            LogAndNotify(Feature.DeviceIORelayOutputsMonostable, "Device Service\\IO\\Relay Outputs\\Monostable", openSupported || closedSupported);
            LogTestEvent(string.Empty);

            // Bistable

            outputSettings.Mode = RelayMode.Bistable;
            check();

            LogAndNotify(Feature.DeviceIORelayOutputsBistableClosed, "Device Service\\IO\\Relay Outputs\\Bistable\\Closed", closedSupported);
            LogAndNotify(Feature.DeviceIORelayOutputsBistableOpen, "Device Service\\IO\\Relay Outputs\\Bistable\\Open", openSupported);
            LogAndNotify(Feature.DeviceIORelayOutputsBistable, "Device Service\\IO\\Relay Outputs\\Bistable", openSupported || closedSupported);
            LogTestEvent(string.Empty);

        }

        void DefineMediaFeatures(Capabilities capabilities, Service[] services)
        {
            BeginStep("Define Media features");
            _mediaSupported = false;

            if (services != null)
            {
                LogUsage("Use GetServices response");

                Service mediaService = Common.CommonUtils.Extensions.FindService(services, OnvifService.MEDIA);
                _mediaSupported = mediaService != null;
                if (mediaService != null)
                {
                    _mediaServiceAddress = mediaService.XAddr;
                }
                else
                {
                    _mediaServiceAddress = string.Empty;
                }
            }
            else
            {
                LogUsage("Use GetCapabilities response");
                _mediaSupported = capabilities.Media != null;
                if (_mediaSupported)
                {
                    _mediaServiceAddress = capabilities.Media.XAddr;
                }
                else
                {
                    _mediaServiceAddress = string.Empty;
                }
            }

            LogAndNotify(Feature.MediaService, "Media service", _mediaSupported);

            if (_mediaSupported)
            {
                LogStepEvent("MANDATORY features: Video, JPEG, RTP/UDP, RTP/RTSP/HTTP");

                FeatureDefined(Feature.JPEG, true);
                FeatureDefined(Feature.RTPUDP, true);
                FeatureDefined(Feature.RTPRTSPHTTP, true);

                StepPassed();

                Action assumeCodecs = new Action(
                    () =>
                    {
                        LogAssumeSupported("H.264");
                        LogAssumeSupported("MPEG4");

                        ReportUndefined(Feature.H264);
                        ReportUndefined(Feature.MPEG4);
                        LogTestEvent(string.Empty);
                    });

                try
                {
                    VideoEncoderConfigurationOptions options = GetVideoEncoderConfigurationOptions();
                    if (options != null)
                    {
                        LogAndNotify(Feature.H264, "H.264", options.H264 != null);
                        LogAndNotify(Feature.MPEG4, "MPEG4", options.MPEG4 != null);
                        LogTestEvent(string.Empty);
                    }
                    else
                    {
                        LogTestEvent("Warning: no Video Encoder configuration options returned." + Environment.NewLine);
                        _warning = true;
                        assumeCodecs();
                    }
                }
                catch (Exception exc)
                {
                    HandleException(exc);
                    LogTestEvent("Warning: an error occurred during getting configuration options" + Environment.NewLine);
                    assumeCodecs();
                }

                Action assumeAudioSupported = new Action(
                    () =>
                    {
                        LogAssumeSupported("Audio");
                        SetUndefined(_featuresSet.FindNode(Feature.Audio));
                    });

                try
                {
                    AudioEncoderConfigurationOptions audioOptions = null;
                    try
                    {
                        audioOptions = GetAudioEncoderConfigurationOptions();

                        BeginStep("Define Audio features");
                        FeatureDefined(Feature.Audio, true);
                        LogStepEvent("Audio : SUPPORTED");

                        FeatureDefined(Feature.G711, true);
                        LogStepEvent("MANDATORY feature: G711");

                        AudioEncoderConfigurationOption g726 = audioOptions.Options != null ?
                            audioOptions.Options.FirstOrDefault(o => o.Encoding == AudioEncoding.G726) : null;
                        LogAndNotify(Feature.G726, "G726", g726 != null);

                        AudioEncoderConfigurationOption aac = audioOptions.Options != null ?
                            audioOptions.Options.FirstOrDefault(o => o.Encoding == AudioEncoding.AAC) : null;
                        LogAndNotify(Feature.AAC, "AAC", aac != null);

                        StepPassed();
                    }
                    catch (FaultException exc)
                    {
                        if (InStep)
                        {
                            StepPassed();
                        }
                        FeatureDefined(Feature.Audio, false);
                        LogStepEvent("Audio : NOT SUPPORTED" + Environment.NewLine);
                        TransmitUnsupported(Feature.Audio);
                    }
                    catch (Exception exc)
                    {
                        HandleException(exc);
                        LogTestEvent("Warning: an error occurred during getting configuration options");
                        assumeAudioSupported();
                    }
                }
                catch (Exception exc)
                {
                    HandleException(exc);

                    LogTestEvent("Warning: an error occurred during getting configuration options");
                    assumeAudioSupported();
                }
            }
            else
            {
                StepPassed();
                TransmitUnsupported(Feature.MediaService);
            }
        }

        void DefineStreamingCapabilities(Capabilities capabilities, MediaServiceCapabilities mediaCapabilities)
        {
            BeginStep("Define Streaming features");

            if (mediaCapabilities != null)
            {
                LogUsage("Use GetServiceCapabilities response");
                bool rtpMulticast = false;
                bool rtpRtspTcp = false;
                if (mediaCapabilities.StreamingCapabilities != null)
                {
                    if (mediaCapabilities.StreamingCapabilities.RTP_RTSP_TCPSpecified)
                    {
                        rtpRtspTcp = mediaCapabilities.StreamingCapabilities.RTP_RTSP_TCP;
                    }
                    if (mediaCapabilities.StreamingCapabilities.RTPMulticastSpecified)
                    {
                        rtpMulticast = mediaCapabilities.StreamingCapabilities.RTPMulticast;
                    }
                }
                else
                {
                    LogStepEvent("StreamingCapabilities field is empty");
                }
                LogAndNotify(Feature.RTPRTSPTCP, "RTP/RTSP/TCP", rtpRtspTcp);
                LogAndNotify(Feature.RTPMulticastUDP, "RTPMulticast/UDP", rtpMulticast);
            }
            else
            {
                bool rtpMulticast = false;
                bool rtpRtspTcp = false;

                if (capabilities.Media != null)
                {
                    if (capabilities.Media.StreamingCapabilities != null)
                    {
                        if (capabilities.Media.StreamingCapabilities.RTP_RTSP_TCPSpecified)
                        {
                            rtpRtspTcp = capabilities.Media.StreamingCapabilities.RTP_RTSP_TCP;
                        }
                        if (capabilities.Media.StreamingCapabilities.RTPMulticastSpecified)
                        {
                            rtpMulticast = capabilities.Media.StreamingCapabilities.RTPMulticast;
                        }
                    }
                    else
                    {
                        LogStepEvent("Media.StreamingCapabilities field is empty");
                    }
                }
                else
                {
                    LogStepEvent("Media field is empty");
                }
                LogAndNotify(Feature.RTPRTSPTCP, "RTP/RTSP/TCP", rtpRtspTcp);
                LogAndNotify(Feature.RTPMulticastUDP, "RTPMulticast/UDP", rtpMulticast);
            }
            StepPassed();

            if (_mediaSupported)
            {
                DefineSnapshotURIFeature();
            }
        }

        void DefineSnapshotURIFeature()
        {
            LogTestEvent("Define GetSnapshotURI capability" + Environment.NewLine);
            bool snapshotUriSupported = false;
            bool undefined = false;

            Action setUndefined = new Action(
                () =>
                {
                    LogAssumeSupported("Snapshot URI");
                    LogTestEvent(string.Empty);
                    SetUndefined(_featuresSet.FindNode(Feature.SnapshotUri));
                });

            Profile[] profiles = null;
            try
            {
                profiles = GetProfiles();
            }
            catch (Exception exc)
            {
                HandleException(exc);
                LogTestEvent("Warning: an error occurred during getting profiles. It is impossible to define GetSnapshotURI capability" + Environment.NewLine);
                setUndefined();
                return;
            }

            Profile profile = null;

            LogTestEvent("Find profile with Video Source and Video Encoder for testing Snapshot URI feature" + Environment.NewLine);

            if (profiles != null && profiles.Length > 0)
            {
                profile =
                    profiles.FirstOrDefault(
                        P => P.VideoEncoderConfiguration != null && P.VideoSourceConfiguration != null);

            }

            if (profile != null)
            {
                LogTestEvent("Use profile with token " + profile.token + Environment.NewLine);
            }
            else
            {
                LogTestEvent("Failed to find profile for testing. It is impossible to define GetSnapshotUri capability" + Environment.NewLine);
                setUndefined();
                return;
            }

            try
            {
                GetSnapshotUri(profile.token);
                snapshotUriSupported = true;
            }
            catch (FaultException exc)
            {
                StepPassed();
                snapshotUriSupported = false;
            }
            catch (Exception exc)
            {
                HandleException(exc);
                undefined = true;
                LogTestEvent("Warning: an error occurred during defining Snapshot URI feature. " + Environment.NewLine);
                setUndefined();
            }

            if (!undefined)
            {
                LogAndNotify(Feature.SnapshotUri, "GetSnapshotUri", snapshotUriSupported);
            }
            LogTestEvent(string.Empty);
        }

        void DefineAudioOutputFeatures()
        {
            AudioOutput[] outputs = null;
            try
            {
                outputs = GetAudioOutputs();
            }
            catch (FaultException exc)
            {
                // outputs == null  - OK
                StepFailed(exc);
            }
            catch (Exception exc)
            {
                RethrowIfStop(exc);
                LogStepEvent("GetAudioOutputs failed. Assume Audio Output features supported");
                LogAssumeSupported("Audio Output");
                SetUndefined(_featuresSet.FindNode(Feature.AudioOutput));
                StepFailed(exc);
                return;
            }

            bool output = false;

            BeginStep("Define Audio Output features");
            if (outputs == null || outputs.Length == 0)
            {
                LogAndNotify(Feature.AudioOutput, "Audio Output", false);
                TransmitUnsupported(Feature.AudioOutput);
            }
            else
            {
                output = true;
                LogAndNotify(Feature.AudioOutput, "Audio Output", true);
            }
            StepPassed();

            if (output)
            {
                LogTestEvent("Check Audio Output sub-features" + Environment.NewLine);

                AudioDecoderConfigurationOptions options = null;
                try
                {
                    options = GetAudioDecoderConfigurationOptions();
                }
                catch (Exception exc)
                {
                    HandleException(exc);
                    LogStepEvent("GetAudioDecoderConfigurationOptions failed, assume all Audio Output features are supported." + Environment.NewLine);
                    TransmitUndefined(Feature.AudioOutput);
                }
                if (options != null)
                {
                    LogAndNotify(Feature.AudioOutputAAC, "AAC", options.AACDecOptions != null);
                    LogAndNotify(Feature.AudioOutputG711, "G711", options.G711DecOptions != null);
                    LogAndNotify(Feature.AudioOutputG726, "G726", options.G726DecOptions != null);
                }
            }
        }

        void DefinePtzFeatures(Capabilities capabilities, Service[] services)
        {
            BeginStep("Define PTZ service");
            _ptzSupported = false;
            if (services != null)
            {
                LogUsage("Use GetServices response");

                Service ptzService = Common.CommonUtils.Extensions.FindService(services, OnvifService.PTZ);
                _ptzSupported = ptzService != null;
                if (ptzService != null)
                {
                    _ptzServiceAddress = ptzService.XAddr;
                }
                else
                {
                    _ptzServiceAddress = string.Empty;
                }
            }
            else
            {
                LogUsage("Use GetCapabilities response");
                _ptzSupported = capabilities.PTZ != null;
                if (_ptzSupported)
                {
                    _ptzServiceAddress = capabilities.PTZ.XAddr;
                }
                else
                {
                    _ptzServiceAddress = string.Empty;
                }
            }

            LogAndNotify(Feature.PTZService, "PTZ Service", _ptzSupported);

            StepPassed();

            if (_ptzSupported)
            {
                PTZNode node = null;

                if (string.IsNullOrEmpty(_ptzNode))
                {
                    LogTestEvent("Note: no node for testing was selected. Testing will be performed for the first node in list.");
                    LogTestEvent(string.Empty);

                    PTZNode[] nodes = null;
                    try
                    {
                        nodes = GetPtzNodes();
                    }
                    catch (Exception exc)
                    {
                        //LogIfFault(exc);
                        HandleException(exc);

                        TransmitUndefined(Feature.PTZService);
                        LogStepEvent("GetNodes failed");
                        LogStepEvent("Assume all PTZ features are supported" + Environment.NewLine);
                        return;
                    }

                    if (nodes != null && nodes.Length > 0)
                    {
                        node = nodes[0];
                        LogTestEvent(string.Format("Use node with token {0}", node.token));
                        LogTestEvent(string.Empty);
                    }
                    else
                    {
                        TransmitUndefined(Feature.PTZService);
                        LogStepEvent("No PTZ node for test were found. Assume all PTZ features are supported" + Environment.NewLine);
                        return;
                    }
                }
                else
                {
                    try
                    {
                        node = GetPtzNode();
                    }
                    catch (Exception exc)
                    {
                        HandleException(exc);

                        TransmitUndefined(Feature.PTZService);
                        LogStepEvent("GetNode failed");
                        LogStepEvent("Assume all PTZ features are supported" + Environment.NewLine);
                        return;
                    }

                    if (node == null)
                    {
                        TransmitUndefined(Feature.PTZService);
                        LogStepEvent(string.Format("No information has been received for node with token {0}. Assume all PTZ features are supported" + Environment.NewLine, _ptzNode));
                        return;
                    }
                }

                BeginStep("Define PTZ Features");

                bool absolutePanTilt = false;
                bool absoluteZoom = false;
                bool relativePanTilt = false;
                bool relativeZoom = false;
                bool continiousPanTilt = false;
                bool continiousZoom = false;
                bool speedPanTilt = false;
                bool speedZoom = false;

                bool presets = false;
                bool home = false;
                bool auxilliary = false;

                if (node.SupportedPTZSpaces != null)
                {
                    // left from "check in cycle"
                    //if (!absolutePanTilt)
                    {
                        absolutePanTilt = CheckPTZSpaces<Space2DDescription>(
                            node.SupportedPTZSpaces.AbsolutePanTiltPositionSpace);
                    }
                    //if (!absoluteZoom)
                    {
                        absoluteZoom = CheckPTZSpaces<Space1DDescription>(
                            node.SupportedPTZSpaces.AbsoluteZoomPositionSpace);
                    }
                    //if (!relativePanTilt)
                    {
                        relativePanTilt = CheckPTZSpaces<Space2DDescription>(
                            node.SupportedPTZSpaces.RelativePanTiltTranslationSpace);
                    }
                    //if (!relativeZoom)
                    {
                        relativeZoom = CheckPTZSpaces<Space1DDescription>(
                            node.SupportedPTZSpaces.RelativeZoomTranslationSpace);
                    }
                    //if (!continiousPanTilt)
                    {
                        continiousPanTilt = CheckPTZSpaces<Space2DDescription>(
                            node.SupportedPTZSpaces.ContinuousPanTiltVelocitySpace);
                    }
                    //if (!continiousZoom)
                    {
                        continiousZoom = CheckPTZSpaces<Space1DDescription>(
                            node.SupportedPTZSpaces.ContinuousZoomVelocitySpace);
                    }

                    //if (!speedPanTilt)
                    {
                        speedPanTilt = CheckPTZSpaces<Space1DDescription>(
                            node.SupportedPTZSpaces.PanTiltSpeedSpace);
                    }
                    //if (!speedZoom)
                    {
                        speedZoom = CheckPTZSpaces<Space1DDescription>(
                            node.SupportedPTZSpaces.ZoomSpeedSpace);
                    }
                }

                presets = /*presets ||*/ (node.MaximumNumberOfPresets > 0);
                home = /*home || */(node.HomeSupported);
                auxilliary = /*auxilliary ||*/ (node.AuxiliaryCommands != null && node.AuxiliaryCommands.Length > 0);

                LogAndNotify(Feature.PTZAbsolute, "Absolute", absolutePanTilt || absoluteZoom);
                LogAndNotify(Feature.PTZAbsolutePanTilt, "Absolute Pan/Tilt", absolutePanTilt);
                LogAndNotify(Feature.PTZAbsoluteZoom, "Absolute Zoom", absoluteZoom);

                LogAndNotify(Feature.PTZRelative, "Relative", relativePanTilt || relativeZoom);
                LogAndNotify(Feature.PTZRelativePanTilt, "Relative Pan/Tilt", relativePanTilt);
                LogAndNotify(Feature.PTZRelativeZoom, "Relative Zoom", relativeZoom);

                LogStepEvent(string.Format("Absolute or relative: {0}",
                    (absolutePanTilt || absoluteZoom || relativePanTilt || relativeZoom) ? "SUPPORTED" : "NOT SUPPORTED"));

                LogStepEvent("Continuous move is MANDATORY");
                FeatureChecked(Feature.PTZContinious, true);
                LogAndNotify(Feature.PTZContinuousPanTilt, "Continuous Pan/Tilt", continiousPanTilt);
                LogAndNotify(Feature.PTZContinuousZoom, "Continuous Zoom", continiousZoom);

                LogAndNotify(Feature.PTZSpeed, "Speed", speedPanTilt || speedZoom);
                LogAndNotify(Feature.PTZSpeedPanTilt, "Speed Pan/Tilt", speedPanTilt);
                LogAndNotify(Feature.PTZSpeedZoom, "Speed Zoom", speedZoom);

                LogAndNotify(Feature.PTZPresets, "Presets", presets);
                LogAndNotify(Feature.PTZHome, "Home position", home);
                LogAndNotify(Feature.PTZAuxiliary, "Auxilliary", auxilliary);

                StepPassed();

                if (home)
                {
                    DefineHomeFeature(node);
                }
                else
                {
                    TransmitUnsupported(Feature.PTZHome);
                }
            }
            else
            {
                TransmitUnsupported(Feature.PTZService);
            }
        }

        void DefineHomeFeature(PTZNode node)
        {
            // fixed or configurable home
            LogTestEvent("Define Fixed/Configurable Home " + Environment.NewLine);
            bool configurable = false;

            Action assumeSupported = new Action(
                () =>
                {
                    LogAssumeSupported("PTZ Fixed/Configurable Home");
                    TransmitUndefined(Feature.PTZHome);
                    LogStepEvent(string.Empty);
                });

            if (!_mediaSupported)
            {
                LogTestEvent("Media service required - unable to check" + Environment.NewLine);
                _warning = true;
                assumeSupported();
                return;
            }

            PTZConfiguration[] configurations = null;

            try
            {
                configurations = GetPtzConfigurations();

                if (configurations == null || configurations.Length == 0)
                {
                    LogTestEvent("No PTZ configuration returned. It is impossible to define Fixed/Configurable Home feature" + Environment.NewLine);
                    _warning = true;
                    assumeSupported();
                    return;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
                LogTestEvent("Warning: getting PTZ configurations failed. It is impossible to define Fixed/Configurable Home feature" + Environment.NewLine);
                assumeSupported();
                return;
            }

            LogTestEvent(string.Empty);

            PTZConfiguration config = configurations.Where(C => C.NodeToken == node.token).FirstOrDefault();

            if (config == null)
            {
                LogTestEvent(string.Format("No PTZ Configuration was found for node {0}. It is impossible to define Fixed/Configurable Home" + Environment.NewLine, _ptzNode));
                assumeSupported();
                return;
            }

            Profile[] profiles = null;
            try
            {
                profiles = GetProfiles();
            }
            catch (Exception exc)
            {
                //LogIfFault(exc);
                HandleException(exc);
                LogTestEvent("Warning: an error occurred getting profiles. It is impossible to define Fixed/Configurable Home feature" + Environment.NewLine);
                assumeSupported();
                return;
            }

            PTZConfiguration backup = null;
            bool doRestore = true;
            Profile profile = null;
            bool deleteProfile = false;

            if (profiles != null && profiles.Length > 0)
            {
                // try to find profile for this node
                profile =
                    profiles.FirstOrDefault(
                        p =>
                        p.VideoEncoderConfiguration != null && p.VideoSourceConfiguration != null &&
                        p.PTZConfiguration != null && p.PTZConfiguration.NodeToken == _ptzNode);
                // if found - no need to change.

                if (profile == null)
                {
                    // try to find valid not-fixed
                    profile =
                        profiles.FirstOrDefault(
                            p => p.VideoEncoderConfiguration != null &&
                                p.VideoSourceConfiguration != null &&
                                p.@fixed == false);

                    if (profile == null)
                    {
                        // if not found - copy from valid.
                        Profile valid = profiles.FirstOrDefault(
                            p => p.VideoEncoderConfiguration != null &&
                                p.VideoSourceConfiguration != null);

                        if (valid == null)
                        {
                            LogStepEvent("No valid profile has been found. Skip defining fixed/confugurable Home");
                            assumeSupported();
                            return;
                        }

                        try
                        {
                            profile = CreateProfile("testprofile", "testprofile");
                            deleteProfile = true;
                            doRestore = true;
                            AddVideoSourceConfiguration(profile.token, valid.VideoSourceConfiguration.token);
                            AddVideoEncoderConfiguration(profile.token, valid.VideoEncoderConfiguration.token);
                        }
                        catch (Exception exc)
                        {
                            HandleException(exc);
                            if (exc is FaultException)
                            {
                                LogFault(exc as FaultException);
                            }
                            LogTestEvent("Failed to create or setup profile for test" + Environment.NewLine);
                            assumeSupported();

                            if (deleteProfile)
                            {
                                try
                                {
                                    DeleteProfile(profile.token);
                                }
                                catch (Exception ex)
                                {
                                    HandleException(ex);
                                    LogStepEvent("Warning: an error occurred during clean-up actions" + Environment.NewLine);
                                }
                            }
                            return;
                        }
                    }
                    else
                    {
                        backup = profile.PTZConfiguration;
                    }
                }
                else
                {
                    // cool, no need for setup
                    doRestore = false;
                }
            }
            else
            {
                LogTestEvent("No profiles returned from the DUT" + Environment.NewLine);
                assumeSupported();
                return;
            }

            try
            {

                if (doRestore)
                {
                    try
                    {
                        AddPtzConfiguration(profile.token, config.token);
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                        if (ex is FaultException)
                        {
                            LogFault(ex as FaultException);
                        }
                        LogTestEvent("Failed to setup profile for test" + Environment.NewLine);
                        assumeSupported();
                        return;
                    }
                }

                try
                {
                    SetHomePostion(profile.token);
                    configurable = true;
                }
                catch (FaultException exc)
                {
                    StepPassed();
                    configurable = false;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
                LogTestEvent("Warning: an error occurred during defining Fixed/Configurable Home feature." + Environment.NewLine);
                assumeSupported();
                return;
            }
            finally
            {
                try
                {
                    if (deleteProfile)
                    {
                        DeleteProfile(profile.token);
                    }
                    else
                    {
                        if (doRestore)
                        {
                            // restore
                            if (backup != null)
                            {
                                AddPtzConfiguration(profile.token, backup.token);
                            }
                            else
                            {
                                RemovePtzConfiguration(profile.token);
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    HandleException(exc);
                    LogStepEvent("Warning: an error occurred during clean-up actions" + Environment.NewLine);
                }
            }

            LogAndNotify(Feature.PTZFixedHome, "Fixed Home", !configurable);
            LogAndNotify(Feature.PTZConfigurableHome, "Configurable Home", configurable);
            LogTestEvent(string.Empty);
        }

        void DefineIoCapabilities(Capabilities capabilities, Service[] services)
        {
            BeginStep("Check IO service");
            _ioSupported = false;
            if (services != null)
            {
                LogUsage("Use GetServices response");

                Service ioService = Common.CommonUtils.Extensions.FindService(services, OnvifService.IO);
                _ioSupported = ioService != null;
                if (ioService != null)
                {
                    _ioServiceAddress = ioService.XAddr;
                }
                else
                {
                    _ioServiceAddress = string.Empty;
                }
            }
            else
            {
                LogUsage("Use GetCapabilities response");
                _ioSupported = capabilities.Extension != null && capabilities.Extension.DeviceIO != null;
                if (_ioSupported)
                {
                    _ioServiceAddress = capabilities.Extension.DeviceIO.XAddr;
                }
                else
                {
                    _ioServiceAddress = string.Empty;
                }
            }

            LogAndNotify(Feature.DeviceIoService, "Device IO Service", _ioSupported);
            if (!_ioSupported)
            {
                TransmitUnsupported(Feature.DeviceIoService);
                FeatureChecked(Feature.RelayOutputs, false);
                TransmitUnsupported(Feature.RelayOutputs);
            }
            StepPassed();

            if (_ioSupported)
            {
                Action assumeRelayOutputsSupported = new Action(
                        () =>
                        {
                            LogAssumeSupported("Relay Outputs");
                            SetUndefined(_featuresSet.FindNode(Feature.RelayOutputs));
                        });

                IOServiceCapabilities ioCapabilities = null;
                if (services != null)
                {
                    try
                    {
                        ioCapabilities = GetIoCapabilities();
                    }
                    catch (Exception exc)
                    {
                        //LogIfFault(exc);
                        HandleException(exc);
                        LogTestEvent("Warning: unable to get Capabilities from IO service" + Environment.NewLine);

                        assumeRelayOutputsSupported();
                        return;
                    }

                    if (ioCapabilities == null)
                    {
                        _warning = true;
                        LogTestEvent("Warning: DUT did not return IO service capabilities" + Environment.NewLine);
                        assumeRelayOutputsSupported();
                        return;
                    }
                }

                BeginStep("Define RelayOutputs features");
                bool relayOutputs = false;

                if (services != null)
                {
                    LogUsage("Use GetServiceCapabilities response");

                    relayOutputs = (ioCapabilities.RelayOutputs > 0);
                }
                else
                {
                    if (capabilities.Extension != null)
                    {
                        if (capabilities.Extension.DeviceIO != null)
                        {
                            relayOutputs = (capabilities.Extension.DeviceIO.RelayOutputs > 0);
                        }
                        else
                        {
                            LogStepEvent("Extension.DeviceIO field is empty");
                        }
                    }
                    else
                    {
                        LogStepEvent("Extension field is empty");
                    }
                }
                LogAndNotify(Feature.RelayOutputs, "Relay outputs", relayOutputs);

                StepPassed();
            }
        }

        void DefineImagingFeatures(Capabilities capabilities, Service[] services)
        {
            BeginStep("Define Imaging features");
            _imagingSupported = false;

            if (services != null)
            {
                LogUsage("Use GetServices response");

                Service imagingService = Common.CommonUtils.Extensions.FindService(services, OnvifService.IMAGING);
                _imagingSupported = imagingService != null;
                if (imagingService != null)
                {
                    _imagingServiceAddress = imagingService.XAddr;
                }
                else
                {
                    _imagingServiceAddress = string.Empty;
                }
            }
            else
            {
                LogUsage("Use GetCapabilities response");
                _imagingSupported = capabilities.Imaging != null;
                if (_imagingSupported)
                {
                    _imagingServiceAddress = capabilities.Imaging.XAddr;
                }
                else
                {
                    _imagingServiceAddress = string.Empty;
                }
            }

            LogAndNotify(Feature.ImagingService, "Imaging", _imagingSupported);
            StepPassed();
        }

        void DefineAnalyticsFeatures(Capabilities capabilities, Service[] services)
        {
            BeginStep("Define Analytics features");
            bool supported = false;
            if (services != null)
            {
                LogUsage("Use GetServices response");

                Service analyticsService = Common.CommonUtils.Extensions.FindService(services, OnvifService.ANALYTICS);
                supported = analyticsService != null;
            }
            else
            {
                LogUsage("Use GetCapabilities response");
                supported = capabilities.Analytics != null;
            }

            LogAndNotify(Feature.AnalyticsService, "Analytics", supported);
            StepPassed();
        }
        
        void DefineDoorControlCapabilities(Service[] services)
        {
            BeginStep("Define DoorControl service support");
            if (services != null)
            {
                _doorControlSupported = services.Count(S => string.Compare(S.Namespace, OnvifService.DOORCONTROL, true ) == 0 ) > 0;

                if (_doorControlSupported)
                {
                    Service service = services.
                        OrderByDescending(s => s.Version == null ? 0 : s.Version.Major).
                        ThenByDescending(s => s.Version == null ? 0 : s.Version.Minor).
                        FirstOrDefault(S => string.Compare(S.Namespace, OnvifService.DOORCONTROL, true) == 0);
                    _doorControlServiceAddress = service.XAddr;
                }
            }

            LogAndNotify(Feature.DoorControlService, "DoorControl service", _doorControlSupported);

            StepPassed();

            if (_doorControlSupported)
            {
                LogStepEvent("MANDATORY feature: Door Entity" + Environment.NewLine);

                FeatureDefined(Feature.DoorEntity, true);

                DefineDoorCapabilities();
            }
            else
            { 
                TransmitUnsupported(Feature.DoorControlService);            
            }

            LogTestEvent(string.Empty);
        }

        void DefineDoorCapabilities()
        {
            DoorControlPortClient client = DoorControlClient;
            List<DoorInfo> doorInfos = null;
            try
            {
                doorInfos = GetFullList<DoorInfo>(GetDoorInfoList, null);
            }
            catch (Exception exc)
            {
                HandleException(exc);
                LogTestEvent("Warning: an error occurred during getting Doors list" + Environment.NewLine);
                // mark all features under 'DoorEntity' as "undefined"

                Action<Feature, string> logUndefined =
                    new Action<Feature, string>(
                        (feature, name) =>
                        {
                            LogAssumeSupported(name);
                            ReportUndefined(feature);
                        });
                logUndefined(Feature.AccessDoor, "Access Door");
                logUndefined(Feature.LockDoor, "Lock Door");
                logUndefined(Feature.UnlockDoor, "Unlock Door");
                logUndefined(Feature.DoubleLockDoor, "Double Lock Door");
                logUndefined(Feature.BlockDoor, "Block Door");
                logUndefined(Feature.LockDownDoor, "Lock Down Door");
                logUndefined(Feature.LockOpenDoor, "Lock Open Door");
                logUndefined(Feature.DoorMonitor, "Door Monitor");
                logUndefined(Feature.LockMonitor, "Lock Monitor");
                logUndefined(Feature.DoubleLockMonitor, "Double Lock Monitor");
                logUndefined(Feature.DoorAlarm, "Door/Alarm");
                logUndefined(Feature.DoorTamper, "Door/Tamper");
                logUndefined(Feature.DoorFault, "Door/Fault");
            }
            if (doorInfos != null)
            {
                Action<Feature, string, Func<DoorCapabilities, bool>> checkFeatureSupport =
                    new Action<Feature, string, Func<DoorCapabilities, bool>>(
                        (feature, featureName, capabilityCheck) =>
                        {
                            DoorInfo filtered = doorInfos.Where(D => capabilityCheck(D.Capabilities)).FirstOrDefault();
                            LogAndNotify(feature, featureName, filtered != null);
                        });

                // Access
                Func<DoorCapabilities, bool> check =
                    new Func<DoorCapabilities, bool>(
                        C => C != null && C.AccessSpecified && C.Access);

                checkFeatureSupport(Feature.AccessDoor, "Access Door", check);

                // Lock
                check = new Func<DoorCapabilities, bool>(C => C != null && C.LockSpecified && C.Lock);
                checkFeatureSupport(Feature.LockDoor, "Lock Door", check);

                // Unlock
                check = new Func<DoorCapabilities, bool>(C => C != null && C.UnlockSpecified && C.Unlock);
                checkFeatureSupport(Feature.UnlockDoor, "Unlock Door", check);

                // Double Lock
                check = new Func<DoorCapabilities, bool>(C => C != null && C.DoubleLockSpecified && C.DoubleLock);
                checkFeatureSupport(Feature.DoubleLockDoor, "Double Lock Door", check);

                // Block
                check = new Func<DoorCapabilities, bool>(C => C != null && C.BlockSpecified && C.Block);
                checkFeatureSupport(Feature.BlockDoor, "Block Door", check);

                // Lock Down
                check = new Func<DoorCapabilities, bool>(C => C != null && C.LockDownSpecified && C.LockDown);
                checkFeatureSupport(Feature.LockDownDoor, "Lock Down Door", check);

                // LockOpen
                check = new Func<DoorCapabilities, bool>(C => C != null && C.LockOpenSpecified && C.LockOpen);
                checkFeatureSupport(Feature.LockOpenDoor, "Lock Open Door", check);

                check = new Func<DoorCapabilities, bool>(C => C != null && C.DoorMonitorSpecified && C.DoorMonitor);
                checkFeatureSupport(Feature.DoorMonitor, "Door Monitor", check);

                check = new Func<DoorCapabilities, bool>(C => C != null && C.LockMonitorSpecified && C.LockMonitor);
                checkFeatureSupport(Feature.LockMonitor, "Lock Monitor", check);

                check = new Func<DoorCapabilities, bool>(C => C != null && C.DoubleLockMonitorSpecified && C.DoubleLockMonitor);
                checkFeatureSupport(Feature.DoubleLockMonitor, "Double Lock Monitor", check);
                
                check = new Func<DoorCapabilities, bool>(C => C != null && C.AlarmSpecified && C.Alarm);
                checkFeatureSupport(Feature.DoorAlarm, "Door/Alarm", check);
                
                check = new Func<DoorCapabilities, bool>(C => C != null && C.TamperSpecified && C.Tamper);
                checkFeatureSupport(Feature.DoorTamper, "Door/Tamper", check);
                
                check = new Func<DoorCapabilities, bool>(C => C != null && C.FaultSpecified && C.Fault);
                checkFeatureSupport(Feature.DoorFault, "Door/Fault", check);
            }
        }

        void DefinePACSEvents()
        {
            Dictionary<TopicInfo, Feature> pacsTopics = GetPACSTopics();
            
            try
            {
                // we'll need "raw" topic information - otherwise not all namespaces are available.
                string response = string.Empty;

                Action<string> dumpAction = new Action<string>(s => response = s);

                this._trafficListener.ResponseReceived += dumpAction;

                // Get all topics.
                TestTool.Proxies.Event.TopicSetType topicSet = GetTopicSet();

                this._trafficListener.ResponseReceived -= dumpAction;

                List<TopicInfo> topicInfos = GetTopicInfos(response);
                
                foreach (TopicInfo info in pacsTopics.Keys)
                {
                    TopicInfo found = topicInfos.Where(T => TopicInfo.TopicsMatch(T, info)).FirstOrDefault();
                    if (found != null)
                    {
                        topicInfos.Remove(found);
                    }

                    LogAndNotify(pacsTopics[info], pacsTopics[info].ToString(), found != null);
                }
            }
            catch (Exception exc)
            {                
                HandleException(exc);
                LogTestEvent("Warning: an error occurred during getting supported Events set" + Environment.NewLine);
                // lots of undefined events

                foreach (Feature feature in pacsTopics.Values)
                {
                    ReportUndefined(feature);
                }
            }
            LogTestEvent(string.Empty);
        }

        void DefineAccessControlCapabilities(Service[] services)
        {
            BeginStep("Define AccessControl service support");
 
            if (services != null)
            {
                _accessControlSupported = services.Count(S => string.Compare(S.Namespace, OnvifService.ACCESSCONTROL, true) == 0) > 0;

                if (_accessControlSupported)
                {
                    Service service = services.
                        OrderByDescending(s => s.Version == null ? 0 : s.Version.Major).
                        ThenByDescending(s => s.Version == null ? 0 : s.Version.Minor).
                        FirstOrDefault(S => string.Compare(S.Namespace, OnvifService.ACCESSCONTROL, true) == 0);
                    _accessControlServiceAddress = service.XAddr;
                }
            }

            LogAndNotify(Feature.AccessControlService, "AccessControl service", _accessControlSupported);

            StepPassed();

            if (_accessControlSupported)
            {
                //
                // Area entity
                //
                PACSPortClient client = AccessControlClient;
                List<AreaInfo> areaInfos = null;
                try
                {
                    areaInfos = GetFullList<AreaInfo>(GetAreaInfoList, null);
                    LogAndNotify(Feature.AreaEntity, "Area Entity", areaInfos.Count > 0);
                }
                catch (Exception exc)
                {
                    HandleException(exc);
                    LogTestEvent("Warning: an error occurred during getting AreaInfo list" + Environment.NewLine);
                    LogAssumeSupported("Area Entity");
                    ReportUndefined(Feature.AreaEntity);
                }

                //
                // Access Point Entity and Enable/Disable Access Point
                //
                LogStepEvent("MANDATORY feature: AccessPoint Entity" + Environment.NewLine);
                FeatureDefined(Feature.AccessPointEntity, true);

                DefineAccessPointCapabilities();
            }
            else
            {
                TransmitUnsupported(Feature.AccessControlService);
            }
        }

        void DefineAccessPointCapabilities()
        {
            PACSPortClient client = AccessControlClient;
            List<AccessPointInfo> accessPointInfos = null;
            try
            {
                accessPointInfos = GetFullList<AccessPointInfo>(GetAccessPointInfoList, null);

                Action<Feature, string, Func<AccessPointCapabilities, bool>> checkFeatureSupport =
                    new Action<Feature, string, Func<AccessPointCapabilities, bool>>(
                        (feature, featureName, capabilityCheck) =>
                        {
                            AccessPointInfo filtered = accessPointInfos.Where(A => capabilityCheck(A.Capabilities)).FirstOrDefault();
                            LogAndNotify(feature, featureName, filtered != null);
                        });


                //Enable/Disable Access Point feature
                Func<AccessPointCapabilities, bool> check =
                    new Func<AccessPointCapabilities, bool>(
                        C => C != null && C.DisableAccessPoint);

                checkFeatureSupport(Feature.EnableDisableAccessPoint, "Enable/Disable AccessPoint", check);

                //Duress feature
                check = new Func<AccessPointCapabilities, bool>(C => C != null && C.DuressSpecified && C.Duress);
                checkFeatureSupport(Feature.Duress, "Duress", check);
                //Access Taken feature
                check = new Func<AccessPointCapabilities, bool>(C => C != null && C.AccessTakenSpecified && C.AccessTaken);
                checkFeatureSupport(Feature.AccessTaken, "AccessTaken", check);
                //External Authorization feature
                check = new Func<AccessPointCapabilities, bool>(C => C != null && C.ExternalAuthorizationSpecified && C.ExternalAuthorization);
                checkFeatureSupport(Feature.ExternalAuthorization, "External Authorization", check);
                //Tamper feature
                check = new Func<AccessPointCapabilities, bool>(C => C != null && C.TamperSpecified && C.Tamper);
                checkFeatureSupport(Feature.Tamper, "Tamper", check);
                //Anonymous Access feature
                check = new Func<AccessPointCapabilities, bool>(C => C != null && C.AnonymousAccessSpecified && C.AnonymousAccess);
                checkFeatureSupport(Feature.AnonymousAccess, "Anonymous Access", check);
            }
            catch (Exception exc)
            {
                HandleException(exc);
                LogTestEvent("Warning: an error occurred during getting AccessPointInfo list" + Environment.NewLine);

                Action<Feature, string> logUndefined =
                    new Action<Feature, string>(
                        (feature, name) =>
                        {
                            LogAssumeSupported(name);
                            ReportUndefined(feature);
                        });
                
                //Enable/Disable Access Point feature
                //Duress feature
                //Access Taken feature
                //External Authorization feature
                //Tamper feature
                //Anonymous Access feature

                logUndefined(Feature.EnableDisableAccessPoint, "Enable/Disable AccessPoint");
                logUndefined(Feature.Duress, "Duress");
                logUndefined(Feature.AccessTaken, "AccessTaken");
                logUndefined(Feature.ExternalAuthorization, "External Authorization");
                logUndefined(Feature.Tamper, "Tamper");
                logUndefined(Feature.AnonymousAccess, "Anonymous Access");
            }

            LogStepEvent("");
        }

        #endregion

        #region Features state reporting
        
        void LogUsage(string entry)
        {
            LogStepEvent(entry);
        }

        void LogAndNotify(Feature feature, string name, bool supported)
        {
            LogStepEvent(string.Format("{0}: {1}", name, supported ? "SUPPORTED" : "NOT SUPPORTED"));
            FeatureChecked(feature, supported);
        }

        void FeatureChecked(Feature feature, bool supported)
        {
            ReportFeatureDefined(feature, supported);
            if (supported)
            {
                _features.Add(feature);
            }
        }
        
        void ReportFeatureDefined(Feature feature, bool supported)
        {
            if (FeatureDefined != null)
            {
                FeatureDefined(feature, supported);
            }
        }

        void LogAssumeSupported(string name)
        {
            LogStepEvent(string.Format("{0} undefined, assume SUPPORTED", name));
        }

        void ReportUndefined(Feature feature)
        {
            if (FeatureDefinitionFailed != null)
            {
                FeatureDefinitionFailed(feature);
            }
        }

        void SetUndefined(FeatureNode node)
        {
            if (node.Status != FeatureStatus.Group)
            {
                ReportUndefined(node.Feature);
            }
            TransmitUndefined(node.Feature);
        }

        void TransmitUndefined(Feature feature)
        {
            FeatureNode node = _featuresSet.FindNode(feature);
            if (node != null)
            {
                TransmitUndefined(node);
            }
        }
        
        void TransmitUndefined(FeatureNode node)
        {
            foreach (FeatureNode child in node.Nodes)
            {
                if (child.Status != FeatureStatus.Group)
                {
                    ReportUndefined(child.Feature);
                }
                TransmitUndefined(child);
            } 
        }

        void TransmitUnsupported(Feature feature)
        {
            FeatureNode node = _featuresSet.FindNode(feature);
            if (node != null)
            {
                TransmitUnsupported(node);
            }
        }

        void TransmitUnsupported(FeatureNode node)
        {
            foreach (FeatureNode child in node.Nodes)
            {
                if (child.Status != FeatureStatus.Group)
                {
                    ReportFeatureDefined(child.Feature, false);
                }
                TransmitUnsupported(child);
            }
        }

        #endregion

        #region Utils
        
        protected bool CheckPTZSpaces<T>(T[] spaces)
        {
            bool res = false;
            if (spaces != null)
            {
                res = spaces.Length > 0;
            }

            return res;
        }
        
        #endregion

        #region Exception handling

        void RethrowIfStop(Exception exc)
        {
            if (exc is StopEventException)
            {
                throw exc;
            }
            if (exc is System.Net.Sockets.SocketException)
            {
                throw exc;
            }
        }

        void HandleException(Exception exc)
        {
            RethrowIfStop(exc);
            if (InStep)
            {
                StepFailed(exc);
            }
            _warning = true;
        }

        #endregion
        
    }
}
