using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.Tests.TestCases.Profiles
{
    public static class ProfileQExtension
    {
        public static bool ScopesMatched(IEnumerable<string> scopes)
        {
            //return scopes.Contains(FactoryDefaultScope) || scopes.Contains(OperationalScope);
            return true;
        }
    }

    [ProfileDefinition("Profile Q", "onvif://www.onvif.org/Profile/Q", ProfileVersionStatus.ReleaseCandidate)]
    public class ProfileQ: BaseProfile, IProfileDefinition
    {
        public ProfileQ()
        {
            _mandatoryScopes = new List<String>();
            _mandatoryScopes.AddRange(new [] { FactoryDefaultScope, OperationalScope });
            InitFeatures();
            InitDiscoveryTypes();
        }

        #region Features

        private List<String> _mandatoryScopes;

        #endregion

        #region Functionality

        private static string GetProfileFeaturesPath(string path)
        {
            return GetFullPath(ROOTPROFILEMANDATORY, path);
        }

        private static string GetProfileConditionalFeaturesPath(string path)
        {
            return GetFullPath(ROOTPROFILECONDITIONAL, path);
        }
        
        private static string GetFactoryDefaultStatePath(string path)
        {
            return GetFullPath("Factory Default State", path);
        }

        private static string GetOperationalStatePath(string path)
        {
            return GetFullPath("Operational State", path);
        }

        private const string ROOTPROFILEMANDATORY = "Profiles Mandatory Features";
        private const string ROOTPROFILECONDITIONAL = "Profile Conditional Features";
        
        // under Device Mandatory Features
        private const string DISCOVERY = "Device Discovery";
        private const string NETWORKCONFIGURATION = "Network Configuration";
        private const string SYSTEM = "System";
        private const string USERHANDLING = "User Handling";
        private const string EVENT = "Event Handling";

        // Profile Mandatory
        private const string AUTHENTICATION = "Authentication";
        private const string CAPABILITIES = "Capabilities";
        private const string STANDARD_MONITORING_EVENTS = "Standard Events for Monitoring";
        private const string ZEROCONFIGURATIONNETWORKCONFIGURATION = "ZeroConfiguration Network Configuration";
        private const string AUTOMATICIPASSIGNMENT = "Automatic IP Assignment";
        private const string OPERATIONALSTATE = "Operational State";
        private const string DEFAULTACCESSPOLICY = "Default Access Policy";

        // Profile Conditional

        private const string TLSCONFIGURATIONTLSSERVER = "TLS Configuration - TLS Server";
        private const string STANDARDEVENTSFORDEVICEMANAGEMENT = "Standard Events for Device Management";
        private const string FACTORYDEFAULTSTATE = "Factory Default State";
        private const string TLSCONFIGURATIONKEYSTORE = "TLS Configuration - Keystore";
        private const string REMOTEUSERMANAGEMENT = "Remote User Management";
        private const string FIRMWAREUPGRADE = "Firmware Upgrade";
        private const string BACKUPANDRESTORE = "Backup and Restore";
        private const string MEDIASERVICE = "Media Service";
        
        protected List<FunctionalityItem> LoadProfileFunctionalities()
        {
            if (_profileFunctionalities == null)
            {
                _profileFunctionalities = new List<FunctionalityItem>();

                _profileFunctionalities.AddRange(
                    new FunctionalityItem[]
                        {
                            new FunctionalityItem(){Functionality = Functionality.GetServices, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.GetServices }},
                            new FunctionalityItem(){Functionality = Functionality.GetDeviceServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.GetServices}},
                            new FunctionalityItem(){Functionality = Functionality.GetEventsServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.GetServices}},
                            new FunctionalityItem(){Functionality = Functionality.MaximumUsernameLength, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.MaximumUsernameLength }},
                            new FunctionalityItem(){Functionality = Functionality.MaximumPasswordLength, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.MaximumPasswordLength }},
                            
                            new FunctionalityItem(){Functionality = Functionality.GetHostname, Path = GetProfileFeaturesPath(NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.SetHostname, Path = GetProfileFeaturesPath(NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetDns, Path = GetProfileFeaturesPath(NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.SetDns, Path = GetProfileFeaturesPath(NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetNetworkInterfaces, Path = GetProfileFeaturesPath(NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.SetNetworkInterfaces, Path = GetProfileFeaturesPath(NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetNetworkProtocols, Path = GetProfileFeaturesPath(NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.SetNetworkProtocols, Path = GetProfileFeaturesPath(NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetNetworkDefaultGateway, Path = GetProfileFeaturesPath(NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.SetNetworkDefaultGateway, Path = GetProfileFeaturesPath(NETWORKCONFIGURATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetZeroConfiguration, Path = GetProfileFeaturesPath(NETWORKCONFIGURATION), Features = new Feature[]{ Feature.ZeroConfiguration }},
                            new FunctionalityItem(){Functionality = Functionality.SetZeroConfiguration, Path = GetProfileFeaturesPath(NETWORKCONFIGURATION), Features = new Feature[]{ Feature.ZeroConfiguration }},

                            new FunctionalityItem(){Functionality = Functionality.GetDeviceInformation, Path = GetProfileFeaturesPath(SYSTEM)},
                            new FunctionalityItem(){Functionality = Functionality.GetSystemDateAndTime, Path = GetProfileFeaturesPath(SYSTEM)},
                            new FunctionalityItem(){Functionality = Functionality.SetSystemDateAndTime, Path = GetProfileFeaturesPath(SYSTEM)},
                            new FunctionalityItem(){Functionality = Functionality.GetNTP, Path = GetProfileFeaturesPath(SYSTEM), Features = new Feature[]{ Feature.NTP }},
                            new FunctionalityItem(){Functionality = Functionality.SetNTP, Path = GetProfileFeaturesPath(SYSTEM), Features = new Feature[]{ Feature.NTP }},
                            new FunctionalityItem(){Functionality = Functionality.SetSystemFactoryDefault, Path = GetProfileFeaturesPath(SYSTEM)},
                            new FunctionalityItem(){Functionality = Functionality.Reboot, Path = GetProfileFeaturesPath(SYSTEM)},
                            
                            new FunctionalityItem(){Functionality = Functionality.MaxUsers, Path = GetProfileFeaturesPath(USERHANDLING), Features = new Feature[]{ Feature.MaxUsers }},
                            new FunctionalityItem(){Functionality = Functionality.GetUsers, Path = GetProfileFeaturesPath(USERHANDLING)},
                            new FunctionalityItem(){Functionality = Functionality.CreateUsers, Path = GetProfileFeaturesPath(USERHANDLING)},
                            new FunctionalityItem(){Functionality = Functionality.DeleteUsers, Path = GetProfileFeaturesPath(USERHANDLING)},
                            new FunctionalityItem(){Functionality = Functionality.SetUser, Path = GetProfileFeaturesPath(USERHANDLING)},

                            new FunctionalityItem(){Functionality = Functionality.MonitoringProcessorUsageEvent, Path = GetProfileFeaturesPath(STANDARD_MONITORING_EVENTS), Features = new Feature[]{ Feature.MonitoringProcessorUsageEvent }},
                            new FunctionalityItem(){Functionality = Functionality.MonitoringOperatingTimeLastResetEvent, Path = GetProfileFeaturesPath(STANDARD_MONITORING_EVENTS), Features = new Feature[]{ Feature.MonitoringOperatingTimeLastResetEvent }},
                            new FunctionalityItem(){Functionality = Functionality.MonitoringOperatingTimeLastRebootEvent, Path = GetProfileFeaturesPath(STANDARD_MONITORING_EVENTS), Features = new Feature[]{ Feature.MonitoringOperatingTimeLastRebootEvent }},
                            new FunctionalityItem(){Functionality = Functionality.MonitoringOperatingTimeLastClockSynchronizationEvent, Path = GetProfileFeaturesPath(STANDARD_MONITORING_EVENTS), Features = new Feature[]{ Feature.MonitoringOperatingTimeLastClockSynchronizationEvent }},
                                                                                    
                            new FunctionalityItem(){Functionality = Functionality.AtLeastTwoPullPointSubscription, Path = GetProfileFeaturesPath(EVENT), Features = new Feature[]{ Feature.MaxPullPoints }},
                            new FunctionalityItem(){Functionality = Functionality.EventsSetSynchronizationPoint, Path = GetProfileFeaturesPath(EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.CreatePullPointSubscription, Path = GetProfileFeaturesPath(EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.PullMessages, Path = GetProfileFeaturesPath(EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.GetEventProperties, Path = GetProfileFeaturesPath(EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.Renew, Path = GetProfileFeaturesPath(EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.Unsubscribe, Path = GetProfileFeaturesPath(EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.TopicFilter, Path = GetProfileFeaturesPath(EVENT)},

                            
                            // Profile Conditional
                            new FunctionalityItem(){Functionality = Functionality.GetRemoteUser, Path = GetProfileConditionalFeaturesPath(REMOTEUSERMANAGEMENT), Features = new Feature[]{ Feature.RemoteUserHandling }},
                            new FunctionalityItem(){Functionality = Functionality.SetRemoteUser, Path = GetProfileConditionalFeaturesPath(REMOTEUSERMANAGEMENT), Features = new Feature[]{ Feature.RemoteUserHandling }},

                            new FunctionalityItem(){Functionality = Functionality.StartFirmwareUgrade, Path = GetProfileConditionalFeaturesPath(FIRMWAREUPGRADE), Features = new Feature[]{ Feature.HttpFirmwareUpgrade }},

                            new FunctionalityItem(){Functionality = Functionality.GetSystemURIs, Path = GetProfileConditionalFeaturesPath(BACKUPANDRESTORE), Features = new Feature[]{ Feature.HttpSystemBackup }},
                            new FunctionalityItem(){Functionality = Functionality.StartSystemRestore, Path = GetProfileConditionalFeaturesPath(BACKUPANDRESTORE), Features = new Feature[]{ Feature.HttpSystemBackup }},

                            new FunctionalityItem(){Functionality = Functionality.CreateCertificationPath, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.RSAKeyPairGenerationOrTLSServerSupport,  }},
                            new FunctionalityItem(){Functionality = Functionality.CreatePKCS10CSR, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.RSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA }},
                            new FunctionalityItem(){Functionality = Functionality.CreateRSAKeyPair, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.RSAKeyPairGeneration, }},
                            new FunctionalityItem(){Functionality = Functionality.CreateSelfSignedCertificate, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.RSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA }},
                            new FunctionalityItem(){Functionality = Functionality.DeleteCertificate, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.PKCS12CertificateWithRSAPrivateKeyUploadOrRSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA, }},
                            new FunctionalityItem(){Functionality = Functionality.DeleteCertificationPath, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.PKCS12CertificateWithRSAPrivateKeyUploadOrTLSServerSupport, }},
                            new FunctionalityItem(){Functionality = Functionality.DeleteKey, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.RSAKeyPairGenerationOrPKCS12CertificateWithRSAPrivateKeyUpload, }},
                            new FunctionalityItem(){Functionality = Functionality.DeletePassphrase, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.PKCS12CertificateWithRSAPrivateKeyUpload, }},
                            new FunctionalityItem(){Functionality = Functionality.GetAllCertificates, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.PKCS12CertificateWithRSAPrivateKeyUploadOrRSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA, }},
                            new FunctionalityItem(){Functionality = Functionality.GetAllCertificationPaths, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.PKCS12CertificateWithRSAPrivateKeyUploadOrTLSServerSupport, }},
                            new FunctionalityItem(){Functionality = Functionality.GetAllKeys, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.RSAKeyPairGenerationOrPKCS12CertificateWithRSAPrivateKeyUpload, }},
                            new FunctionalityItem(){Functionality = Functionality.GetAllPassphrases, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.PKCS12CertificateWithRSAPrivateKeyUpload, }},
                            new FunctionalityItem(){Functionality = Functionality.GetCertificate, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.PKCS12CertificateWithRSAPrivateKeyUploadOrRSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA, }},
                            new FunctionalityItem(){Functionality = Functionality.GetCertificationPath, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.PKCS12CertificateWithRSAPrivateKeyUploadOrTLSServerSupport, }},
                            new FunctionalityItem(){Functionality = Functionality.GetKeyStatus, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.RSAKeyPairGenerationOrPKCS12CertificateWithRSAPrivateKeyUpload, }},
                            new FunctionalityItem(){Functionality = Functionality.UploadCertificate, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.RSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA }},
                            new FunctionalityItem(){Functionality = Functionality.UploadCertificateWithPrivateKeyInPKCS12, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.PKCS12CertificateWithRSAPrivateKeyUpload, }},
                            new FunctionalityItem(){Functionality = Functionality.UploadKeyPairInPKCS8, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.PKCS8RSAKeyPairUpload,  }},
                            new FunctionalityItem(){Functionality = Functionality.UploadPassphrase, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONKEYSTORE), Features=new []{ Feature.PKCS12CertificateWithRSAPrivateKeyUpload, }},

                            new FunctionalityItem(){Functionality = Functionality.AddServerCertificateAssignment, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONTLSSERVER), Features=new Feature[]{Feature.TLSServerSupport}},
                            new FunctionalityItem(){Functionality = Functionality.GetAssignedServerCertificates, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONTLSSERVER), Features=new Feature[]{Feature.TLSServerSupport}},
                            //new FunctionalityItem(){Functionality = Functionality.GetNetworkProtocols, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONTLSSERVER), Features=new Feature[]{}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveServerCertificateAssignment, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONTLSSERVER), Features=new Feature[]{Feature.TLSServerSupport}},
                            new FunctionalityItem(){Functionality = Functionality.ReplaceServerCertificateAssignment, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONTLSSERVER), Features=new Feature[]{Feature.TLSServerSupport}},
                            //new FunctionalityItem(){Functionality = Functionality.SetNetworkProtocols, Path = GetProfileConditionalFeaturesPath(TLSCONFIGURATIONTLSSERVER), Features=new Feature[]{}},
                            
                            new FunctionalityItem(){Functionality = Functionality.GetProfiles, Path = GetProfileConditionalFeaturesPath(MEDIASERVICE), Features=new Feature[]{Feature.MediaService}},
                            new FunctionalityItem(){Functionality = Functionality.GetStreamUri, Path = GetProfileConditionalFeaturesPath(MEDIASERVICE), Features=new Feature[]{Feature.MediaService, Feature.RTSS}},
                            new FunctionalityItem(){Functionality = Functionality.GetMediaServiceCapabilities, Path = GetProfileConditionalFeaturesPath(MEDIASERVICE), Features=new Feature[]{Feature.MediaService, Feature.GetServices}},
                            new FunctionalityItem(){Functionality = Functionality.MediaStreamingRtsp, Path = GetProfileConditionalFeaturesPath(MEDIASERVICE), Features=new Feature[]{Feature.MediaService, Feature.RTSS}},

                            new FunctionalityItem(){Functionality = Functionality.DeviceHardwareFailureFanFailureEvent, Path = GetProfileConditionalFeaturesPath(STANDARDEVENTSFORDEVICEMANAGEMENT), Features=new Feature[]{Feature.DeviceHardwareFailureFanFailureEvent}},
                            new FunctionalityItem(){Functionality = Functionality.DeviceHardwareFailurePowerSupplyFailureEvent, Path = GetProfileConditionalFeaturesPath(STANDARDEVENTSFORDEVICEMANAGEMENT), Features=new Feature[]{Feature.DeviceHardwareFailurePowerSupplyFailureEvent}},
                            new FunctionalityItem(){Functionality = Functionality.DeviceHardwareFailureStorageFailureEvent, Path = GetProfileConditionalFeaturesPath(STANDARDEVENTSFORDEVICEMANAGEMENT), Features=new Feature[]{Feature.DeviceHardwareFailureStorageFailureEvent}},
                            new FunctionalityItem(){Functionality = Functionality.DeviceHardwareFailureTemperatureCriticalEvent, Path = GetProfileConditionalFeaturesPath(STANDARDEVENTSFORDEVICEMANAGEMENT), Features=new Feature[]{Feature.DeviceHardwareFailureTemperatureCriticalEvent}},
                            new FunctionalityItem(){Functionality = Functionality.MonitoringBackupLastEvent, Path = GetProfileConditionalFeaturesPath(STANDARDEVENTSFORDEVICEMANAGEMENT), Features=new Feature[]{Feature.MonitoringBackupLastEvent}},

                            new FunctionalityItem(){Functionality = Functionality.FactoryDefaultStateIsSignaledByTheScope, Path = GetFactoryDefaultStatePath(FACTORYDEFAULTSTATE)},
                            new FunctionalityItem(){Functionality = Functionality.AnonymousAccessInFactoryDefaultState, Path = GetFactoryDefaultStatePath(FACTORYDEFAULTSTATE)},
                            new FunctionalityItem(){Functionality = Functionality.UserConfigurationInFactoryDefaultState, Path = GetFactoryDefaultStatePath(FACTORYDEFAULTSTATE)},

                            new FunctionalityItem(){Functionality = Functionality.WSDiscovery, Path = GetFactoryDefaultStatePath(DISCOVERY)},
                            new FunctionalityItem(){Functionality = Functionality.GetScopes, Path = GetFactoryDefaultStatePath(DISCOVERY)},

                            new FunctionalityItem(){Functionality = Functionality.DynamicIPConfigurationEnabledInFactoryDefaultState, Path = GetFactoryDefaultStatePath(ZEROCONFIGURATIONNETWORKCONFIGURATION)},

                            new FunctionalityItem(){Functionality = Functionality.IPv4DHCPEnabledInFactoryDefaultState, Path = GetFactoryDefaultStatePath(AUTOMATICIPASSIGNMENT)},
                            new FunctionalityItem(){Functionality = Functionality.IPv6StatelessAutoconfigurationEnabledInFactoryDefaultState, Path = GetFactoryDefaultStatePath(AUTOMATICIPASSIGNMENT), Features = new Feature[]{Feature.IPv6}},

                            new FunctionalityItem(){Functionality = Functionality.OperationalStateIsSignaledByTheScope, Path = GetOperationalStatePath(OPERATIONALSTATE)},

                            new FunctionalityItem(){Functionality = Functionality.DigestAuthentication, Path = GetOperationalStatePath(AUTHENTICATION), Features = new Feature[]{Feature.Digest}},

                            new FunctionalityItem(){Functionality = Functionality.DefaultAccessPolicy, Path = GetOperationalStatePath(DEFAULTACCESSPOLICY), Features = new Feature[]{Feature.DefaultAccessPolicy}},
                        });
            }

            return _profileFunctionalities;
        }
        #endregion

        #region IProfileDefinition Members

        public static bool ScopesMatched(IEnumerable<string> scopes)
        {
            return scopes.Contains(OperationalScope) || scopes.Contains(FactoryDefaultScope);
        }

        public IEnumerable<String> MandatoryScopes
        {
            get
            {
                return _mandatoryScopes;
            }
        }

        public IEnumerable<FunctionalityItem> Functionalities
        {
            get
            {
                if (_profileFunctionalities == null)
                {
                    LoadProfileFunctionalities();
                }
                return _profileFunctionalities;
            }
        }

        public static string FactoryDefaultScope = "onvif://www.onvif.org/Profile/Q/FactoryDefault";
        public static string OperationalScope    = "onvif://www.onvif.org/Profile/Q/Operational";

        public IEnumerable<Feature> MandatoryDiscoveryTypes { get; private set; }

        public ProfileStatus Check(out string reason, IEnumerable<Feature> features, IEnumerable<string> scopes, Dictionary<string, object> parameters)
        {
            reason = string.Empty;

            var Name = this.GetProfileName();
            var Scope = this.GetProfileScope();

            StringBuilder sb = new StringBuilder();
            ProfileStatus status = ProfileStatus.NotSupported;

            sb.AppendLine(string.Format("Check profile support for {0}", Name));

            bool scopePresent = scopes.Contains(FactoryDefaultScope) || scopes.Contains(OperationalScope);
            sb.AppendLine(string.Format("Scope {0}: \t\t{1}", FactoryDefaultScope, scopes.Contains(FactoryDefaultScope) ? "PRESENT" : "NOT PRESENT"));
            sb.AppendLine(string.Format("Scope {0}: \t\t{1}", OperationalScope,    scopes.Contains(OperationalScope)    ? "PRESENT" : "NOT PRESENT"));

            if (!scopePresent)
                sb.AppendFormat("Profile not supported");
            else
            {
                bool profileOk = true;
                bool supported;

                Action<Feature> checkNextMandatory = 
                    (feature) =>
                    {
                        supported = features.ContainsFeature(feature);
                        LogMandatory(sb, FeaturesHelper.GetDisplayName(feature), supported);
                        profileOk = profileOk && supported;
                    };

                Action<Feature> logNextMandatory = 
                    (feature) =>
                    {
                        supported = features.ContainsFeature(feature);
                        LogMandatoryFeature(sb, FeaturesHelper.GetDisplayName(feature));
                        profileOk = profileOk && supported;
                    };

                Action<Feature> checkNextOptional = 
                    (feature) =>
                    {
                        supported = features.Contains(feature);
                        LogOptional(sb, FeaturesHelper.GetDisplayName(feature), supported);
                    };

                checkNextMandatory(Feature.DiscoveryTypesTdsDevice);

                checkNextMandatory(Feature.GetServices);
                checkNextMandatory(Feature.MaximumUsernameLength);
                checkNextMandatory(Feature.MaximumPasswordLength);

                checkNextMandatory(Feature.ZeroConfiguration);
                checkNextMandatory(Feature.NTP);

                checkNextMandatory(Feature.MaxUsers);

                checkNextMandatory(Feature.MonitoringProcessorUsageEvent);
                checkNextMandatory(Feature.MonitoringOperatingTimeLastResetEvent);
                checkNextMandatory(Feature.MonitoringOperatingTimeLastRebootEvent);
                checkNextMandatory(Feature.MonitoringOperatingTimeLastClockSynchronizationEvent);
                
                logNextMandatory(Feature.EventsService);
                
                checkNextMandatory(Feature.MaxPullPoints);
                if (profileOk)
                {
                    var v = (int)parameters["MaxPullPoints"];
                    profileOk = v >= 2;
                    sb.AppendLine(string.Format("EventService/GetServiceCapabilities/MaxPullPoints has value >= 2: {0}", profileOk ? "SUPPORTED" : "NOT SUPPORTED"));
                }                

                if (features.ContainsFeature(Feature.AdvancedSecurity))
                {
                    checkNextMandatory(Feature.RSAKeyPairGenerationOrPKCS12CertificateWithRSAPrivateKeyUpload);

                    if (features.ContainsFeature(Feature.RSAKeyPairGeneration))
                        checkNextMandatory(Feature.SelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA);

                    checkNextMandatory(Feature.MaximumNumberOfKeys);
                    if (profileOk)
                    {
                        var v = (int)parameters["MaximumNumberOfKeys"];
                        profileOk = v >= 16;
                        sb.AppendLine(string.Format("Advanced Security Service/GetServiceCapabilities/MaximumNumberOfKeys has value >= 16: {0}", profileOk ? "SUPPORTED" : "NOT SUPPORTED"));
                    }

                    checkNextMandatory(Feature.MaximumNumberOfCertificates);
                    if (profileOk)
                    {
                        var v = (int)parameters["MaximumNumberOfCertificates"];
                        profileOk = v >= 16;
                        sb.AppendLine(string.Format("Advanced Security Service/GetServiceCapabilities/MaximumNumberOfCertificates has value >= 16: {0}", profileOk ? "SUPPORTED" : "NOT SUPPORTED"));
                    }

                    checkNextMandatory(Feature.TLSServerSupport);
                }

                if (features.ContainsFeature(Feature.MediaService))
                {
                    checkNextMandatory(Feature.GetServices);
                    checkNextMandatory(Feature.RTSS);
                }

                checkNextOptional(Feature.DeviceHardwareFailureFanFailureEvent);
                checkNextOptional(Feature.DeviceHardwareFailurePowerSupplyFailureEvent);
                checkNextOptional(Feature.DeviceHardwareFailureStorageFailureEvent);
                checkNextOptional(Feature.DeviceHardwareFailureTemperatureCriticalEvent);
                checkNextOptional(Feature.MonitoringBackupLastEvent);

                checkNextMandatory(Feature.DefaultAccessPolicy);

                checkNextMandatory(Feature.Digest);

                status = profileOk ? ProfileStatus.Supported : ProfileStatus.Failed;
            }

            reason = sb.ToString();

            return status;
        }


        #endregion

        void InitFeatures()
        {
            _features = new List<ProfileFeature>();

            _features.Add(new ProfileFeature() { Feature = Feature.GetServices, State = ProfileFeatureState.Mandatory});
            _features.Add(new ProfileFeature() { Feature = Feature.MaximumUsernameLength, State = ProfileFeatureState.Mandatory});
            _features.Add(new ProfileFeature() { Feature = Feature.MaximumPasswordLength, State = ProfileFeatureState.Mandatory});

            _features.Add(new ProfileFeature() { Feature = Feature.ZeroConfiguration, State = ProfileFeatureState.Mandatory});
            _features.Add(new ProfileFeature() { Feature = Feature.NTP, State = ProfileFeatureState.Mandatory});

            _features.Add(new ProfileFeature() { Feature = Feature.MaxUsers, State = ProfileFeatureState.Mandatory});

            _features.Add(new ProfileFeature() { Feature = Feature.MonitoringProcessorUsageEvent, State = ProfileFeatureState.Mandatory});
            _features.Add(new ProfileFeature() { Feature = Feature.MonitoringOperatingTimeLastResetEvent, State = ProfileFeatureState.Mandatory});
            _features.Add(new ProfileFeature() { Feature = Feature.MonitoringOperatingTimeLastRebootEvent, State = ProfileFeatureState.Mandatory});
            _features.Add(new ProfileFeature() { Feature = Feature.MonitoringOperatingTimeLastClockSynchronizationEvent, State = ProfileFeatureState.Mandatory});

            _features.Add(new ProfileFeature() { Feature = Feature.DeviceHardwareFailureFanFailureEvent, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.DeviceHardwareFailurePowerSupplyFailureEvent, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.DeviceHardwareFailureStorageFailureEvent, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.DeviceHardwareFailureTemperatureCriticalEvent, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.MonitoringBackupLastEvent, State = ProfileFeatureState.Optional});

            _features.Add(new ProfileFeature() { Feature = Feature.MaximumNumberOfKeys, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.MaximumNumberOfCertificates, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.RemoteUserHandling, State = ProfileFeatureState.Optional});

            _features.Add(new ProfileFeature() { Feature = Feature.HttpFirmwareUpgrade, State = ProfileFeatureState.Optional});

            _features.Add(new ProfileFeature() { Feature = Feature.HttpSystemBackup, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.HttpSystemBackup, State = ProfileFeatureState.Optional});

            _features.Add(new ProfileFeature() { Feature = Feature.RSAKeyPairGeneration, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.PKCS12CertificateWithRSAPrivateKeyUpload, State = ProfileFeatureState.Optional});

            _features.Add(new ProfileFeature() { Feature = Feature.SelfSignedCertificateCreationWithRSA, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.PKCS10ExternalCertificationWithRSA, State = ProfileFeatureState.Optional});

            _features.Add(new ProfileFeature() { Feature = Feature.PKCS8RSAKeyPairUpload, State = ProfileFeatureState.Optional});

            _features.Add(new ProfileFeature() { Feature = Feature.TLSServerSupport, State = ProfileFeatureState.Optional});

            /**************/
            _features.Add(new ProfileFeature() { Feature = Feature.RSAKeyPairGenerationOrTLSServerSupport, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.RSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.PKCS12CertificateWithRSAPrivateKeyUploadOrRSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.PKCS12CertificateWithRSAPrivateKeyUploadOrTLSServerSupport, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.RSAKeyPairGenerationOrPKCS12CertificateWithRSAPrivateKeyUpload, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.PKCS12CertificateWithRSAPrivateKeyUpload, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.PKCS8RSAKeyPairUpload, State = ProfileFeatureState.Optional});
            /**************/

            _features.Add(new ProfileFeature() { Feature = Feature.MediaService, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.RTSS, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.GetServices, State = ProfileFeatureState.Optional});

            _features.Add(new ProfileFeature() { Feature = Feature.IPv6, State = ProfileFeatureState.Optional});

            _features.Add(new ProfileFeature() { Feature = Feature.RTSS, State = ProfileFeatureState.Mandatory});

            _features.Add(new ProfileFeature() { Feature = Feature.DeviceHardwareFailureFanFailureEvent, State = ProfileFeatureState.Mandatory});
            _features.Add(new ProfileFeature() { Feature = Feature.DeviceHardwareFailurePowerSupplyFailureEvent, State = ProfileFeatureState.Mandatory});
            _features.Add(new ProfileFeature() { Feature = Feature.DeviceHardwareFailureStorageFailureEvent, State = ProfileFeatureState.Mandatory});
            _features.Add(new ProfileFeature() { Feature = Feature.DeviceHardwareFailureTemperatureCriticalEvent, State = ProfileFeatureState.Mandatory});
            _features.Add(new ProfileFeature() { Feature = Feature.MonitoringBackupLastEvent, State = ProfileFeatureState.Mandatory});

            _features.Add(new ProfileFeature() { Feature = Feature.DefaultAccessPolicy, State = ProfileFeatureState.Mandatory});

            _features.Add(new ProfileFeature() { Feature = Feature.Digest, State = ProfileFeatureState.Mandatory});
        }

        void InitDiscoveryTypes()
        {
            var list = new List<Feature> { Feature.DiscoveryTypesTdsDevice };

            MandatoryDiscoveryTypes = list;
        }
    }
}
