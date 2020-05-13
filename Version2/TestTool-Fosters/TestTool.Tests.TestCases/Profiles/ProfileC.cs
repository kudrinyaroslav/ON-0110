using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions;

namespace TestTool.Tests.TestCases.Profiles
{
    [ProfileDefinition]
    public class ProfileC : BaseProfile, IProfileDefinition
    {

        public const string C_PROFILE_SCOPE = "onvif://www.onvif.org/Profile/C";
                        
        public ProfileC()
        {
            _mandatoryScopes = new List<String>();
            _mandatoryScopes.AddRange(new String[]
                                   {
                                       C_PROFILE_SCOPE
                                   });
            InitFeatures();
        }

        public string Name
        {
            get { return "Profile C"; }
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
        
        private static string GetDeviceMandatoryFullPath(string path)
        {
            return GetFullPath("Device Mandatory Features", path);
        }

        private const string ROOTPROFILEMANDATORY = "Profiles Mandatory Features";
        private const string ROOTPROFILECONDITIONAL = "Profile Conditional Features";
        
        // under Device Mandatory Features
        private const string DISCOVERY = "Discovery";
        private const string NETWORKCONFIGURATION = "Network Configuration";
        private const string SYSTEM = "System";
        private const string USERHANDLING = "User Handling";
        private const string EVENT = "Event handling";

        // Profile Mandatory
        private const string SECURITY = "Security";
        private const string CAPABILITIES = "Capabilities";
        private const string ACCESSPOINTSINFORMATION = "System component information – Access points";
        private const string DOORSINFORMATION = "System component information – Doors";
        private const string AREASINFORMATION = "System component information – Areas";
        private const string ACCESSPOINTSTATE = "Access Point state";
        private const string DOORSTATE = "Door state";
        private const string DOORCONTROL = "Door control";
        private const string ACCESSCONTROLDECISIONS = "Access control decisions";

        // Profile Conditional

        private const string ACCESSPOINTCONTROL = "Access Point Control";
        private const string ACCESSPOINTEXTERNALAUTHORIZATION = "Access Point external authorization";
        private const string STOREEVENTS = "Store events";
        private const string RELAYOUTPUTS = "Relay Outputs";
        private const string DIGITALINPUTS = "Digital inputs";
        private const string IPFILTERING = "IP Filtering";
        
        protected List<FunctionalityItem> LoadProfileFunctionalities()
        {
            if (_profileFunctionalities == null)
            {
                _profileFunctionalities = new List<FunctionalityItem>();

                _profileFunctionalities.AddRange(
                    new FunctionalityItem[]
                        {
                            // security
                            new FunctionalityItem(){Functionality = Functionality.DigestAuthentication, Path = GetFullPath(ROOTPROFILEMANDATORY, SECURITY), Features = new Feature[]{Feature.Digest}},
                            // capabilities
                            new FunctionalityItem(){Functionality = Functionality.GetCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES)},
                            new FunctionalityItem(){Functionality = Functionality.GetDeviceServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES)},
                            new FunctionalityItem(){Functionality = Functionality.GetEventsServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES)},
                            new FunctionalityItem(){Functionality = Functionality.GetDoorControlServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES)},
                            new FunctionalityItem(){Functionality = Functionality.GetAccessControlServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES)},
                            new FunctionalityItem(){Functionality = Functionality.GetAccessPointInfoList, Path = GetProfileFeaturesPath(ACCESSPOINTSINFORMATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetAccessPointInfo, Path = GetProfileFeaturesPath(ACCESSPOINTSINFORMATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetDoorInfoList , Path = GetProfileFeaturesPath(DOORSINFORMATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetDoorInfo, Path = GetProfileFeaturesPath(DOORSINFORMATION)},
                            new FunctionalityItem(){Functionality = Functionality.DoorChangedEvent, Path = GetProfileFeaturesPath(DOORSINFORMATION)},
                            new FunctionalityItem(){Functionality = Functionality.DoorRemovedEvent, Path = GetProfileFeaturesPath(DOORSINFORMATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetAreaInfoList, Path = GetProfileFeaturesPath(AREASINFORMATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetAreaInfo, Path = GetProfileFeaturesPath(AREASINFORMATION)},
                            new FunctionalityItem(){Functionality = Functionality.GetAccessPointState, Path = GetProfileFeaturesPath(ACCESSPOINTSTATE)},
                            new FunctionalityItem(){Functionality = Functionality.AccessPointEnabledEvent, Path = GetProfileFeaturesPath(ACCESSPOINTSTATE)},
                            new FunctionalityItem(){Functionality = Functionality.AccessPointTamperingEvent, Path = GetProfileFeaturesPath(ACCESSPOINTSTATE)},
                                                                                    
                            new FunctionalityItem(){Functionality = Functionality.GetDoorState, Path = GetProfileFeaturesPath(DOORSTATE)},
                            new FunctionalityItem(){Functionality = Functionality.DoorModeEvent, Path = GetProfileFeaturesPath(DOORSTATE)},
                            new FunctionalityItem(){Functionality = Functionality.DoorPhysicalStateEvent, Path = GetProfileFeaturesPath(DOORSTATE)},
                            new FunctionalityItem(){Functionality = Functionality.DoubleLockPhysicalStateEvent, Path = GetProfileFeaturesPath(DOORSTATE)},
                            new FunctionalityItem(){Functionality = Functionality.LockPhysicalStateEvent, Path = GetProfileFeaturesPath(DOORSTATE)},
                            new FunctionalityItem(){Functionality = Functionality.DoorTamperEvent, Path = GetProfileFeaturesPath(DOORSTATE)},
                            new FunctionalityItem(){Functionality = Functionality.DoorAlarmEvent, Path = GetProfileFeaturesPath(DOORSTATE)},
                            new FunctionalityItem(){Functionality = Functionality.DoorFaultEvent, Path = GetProfileFeaturesPath(DOORSTATE)},
                            new FunctionalityItem(){Functionality = Functionality.AccessDoor, Path = GetProfileFeaturesPath(DOORCONTROL)},
                            new FunctionalityItem(){Functionality = Functionality.LockDoor, Path = GetProfileFeaturesPath(DOORCONTROL)},
                            new FunctionalityItem(){Functionality = Functionality.UnlockDoor, Path = GetProfileFeaturesPath(DOORCONTROL)},
                            new FunctionalityItem(){Functionality = Functionality.DoubleLockDoor, Path = GetProfileFeaturesPath(DOORCONTROL)},
                            new FunctionalityItem(){Functionality = Functionality.BlockDoor, Path = GetProfileFeaturesPath(DOORCONTROL)},
                            new FunctionalityItem(){Functionality = Functionality.LockDownDoor, Path = GetProfileFeaturesPath(DOORCONTROL)},
                            new FunctionalityItem(){Functionality = Functionality.LockDownReleaseDoor, Path = GetProfileFeaturesPath(DOORCONTROL)},
                            new FunctionalityItem(){Functionality = Functionality.LockOpenDoor, Path = GetProfileFeaturesPath(DOORCONTROL)},
                            new FunctionalityItem(){Functionality = Functionality.LockOpenReleaseDoor, Path = GetProfileFeaturesPath(DOORCONTROL)},
                                                                                    
                            new FunctionalityItem(){Functionality = Functionality.AccessGrantedAnonymousEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessGrantedCredentialEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessTakenAnonymousEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessTakenCredentialEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessNotTakenAnonymousEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessNotTakenCredentialEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedCredentialCredentialNotEnabledEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedCredentialCredentialNotActiveEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedCredentialCredentialExpiredEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedCredentialInvalidPINEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedCredentialNotPermittedAtThisTimeEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedCredentialUnauthorizedEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedCredentialOtherEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedAnonymousNotPermittedAtThisTimeEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedAnonymousUnauthorizedEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedAnonymousOtherEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedCredentialCredentialNotFoundCardEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.DuressAnonymousEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            new FunctionalityItem(){Functionality = Functionality.DuressCredentialEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},
                            
                            // Profile Conditional
                            //Access Point Control
                            new FunctionalityItem(){Functionality = Functionality.EnableAccessPoint, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTCONTROL), Features=new Feature[]{Feature.EnableDisableAccessPoint}},
                            new FunctionalityItem(){Functionality = Functionality.DisableAccessPoint, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTCONTROL), Features=new Feature[]{Feature.EnableDisableAccessPoint}},
                            // Access point external authorization
                            new FunctionalityItem(){Functionality = Functionality.ExternalAutorization, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            new FunctionalityItem(){Functionality = Functionality.AccessGrantedAnonymousExternalEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            new FunctionalityItem(){Functionality = Functionality.AccessGrantedCredentialExternalEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedAnonymousExternalEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedCredentialExternalEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            new FunctionalityItem(){Functionality = Functionality.RequestAnonymousEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            new FunctionalityItem(){Functionality = Functionality.RequestCredentialEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            new FunctionalityItem(){Functionality = Functionality.RequestTimeoutAnonymousEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            new FunctionalityItem(){Functionality = Functionality.RequestTimeoutCredentialEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            // Seek (Event Service)
                            new FunctionalityItem(){Functionality = Functionality.EventsSeek, Path = GetProfileConditionalFeaturesPath(STOREEVENTS), Features=new Feature[]{Feature.EventSeek}},

                            // Relay outputs                 
                            new FunctionalityItem(){Functionality = Functionality.GetIOServiceCapabilities, Path = GetProfileConditionalFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.DeviceIoService}},
                            new FunctionalityItem(){Functionality = Functionality.IOGetRelayOutputs, Path = GetProfileConditionalFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.RelayOutputs}},
                            new FunctionalityItem(){Functionality = Functionality.IOSetRelayOutputState, Path = GetProfileConditionalFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.RelayOutputs}},
                            new FunctionalityItem(){Functionality = Functionality.IOSetRelayOutputSettings, Path = GetProfileConditionalFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.RelayOutputs}},
                            new FunctionalityItem(){Functionality = Functionality.IOSetRelayOutputOptions, Path = GetProfileConditionalFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.RelayOutputs}},
                            // Digital inputs
                            //new FunctionalityItem(){Functionality = Functionality.GetIOServiceCapabilities, Path = GetProfileConditionalFeaturesPath(DIGITALINPUTS)},
                            new FunctionalityItem(){Functionality = Functionality.GetDigitalInputs, Path = GetProfileConditionalFeaturesPath(DIGITALINPUTS), Features=new Feature[]{Feature.DigitalInputs}},
                            // IP Address filtering
                            new FunctionalityItem(){Functionality = Functionality.GetIPAddressFilter, Path = GetProfileConditionalFeaturesPath(IPFILTERING), Features=new Feature[]{Feature.IPFilter}},
                            new FunctionalityItem(){Functionality = Functionality.SetIPAddressFilter, Path = GetProfileConditionalFeaturesPath(IPFILTERING), Features=new Feature[]{Feature.IPFilter}},
                            new FunctionalityItem(){Functionality = Functionality.AddIPAddressFilter, Path = GetProfileConditionalFeaturesPath(IPFILTERING), Features=new Feature[]{Feature.IPFilter}},
                            new FunctionalityItem(){Functionality = Functionality.RemoveIPAddressFilter, Path = GetProfileConditionalFeaturesPath(IPFILTERING), Features=new Feature[]{Feature.IPFilter}},
                        }

                        );
                _profileFunctionalities.AddRange(LoadCoreFunctionalities());
            }

            return _profileFunctionalities;
        }


        private static List<FunctionalityItem> LoadCoreFunctionalities()
        {
            List<FunctionalityItem> coreFunctionalities = new List<FunctionalityItem>();
            
            coreFunctionalities.AddRange(
                new FunctionalityItem[]
                    {
                        //Discovery
                        new FunctionalityItem(){Functionality = Functionality.WSDiscovery, Path = GetDeviceMandatoryFullPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.GetDiscoveryMode, Path = GetDeviceMandatoryFullPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.SetDiscoveryMode, Path = GetDeviceMandatoryFullPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.GetScopes, Path = GetDeviceMandatoryFullPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.SetScopes, Path = GetDeviceMandatoryFullPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.AddScopes, Path = GetDeviceMandatoryFullPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.RemoveScopes, Path = GetDeviceMandatoryFullPath(DISCOVERY)},
                        // Network
                        new FunctionalityItem(){Functionality = Functionality.GetHostname, Path = GetDeviceMandatoryFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.SetHostname, Path = GetDeviceMandatoryFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.GetDns, Path = GetDeviceMandatoryFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.SetDns, Path = GetDeviceMandatoryFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.GetNetworkInterfaces, Path = GetDeviceMandatoryFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.SetNetworkInterfaces, Path = GetDeviceMandatoryFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.GetNetworkProtocols, Path = GetDeviceMandatoryFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.SetNetworkProtocols, Path = GetDeviceMandatoryFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.GetNetworkDefaultGateway, Path = GetDeviceMandatoryFullPath(NETWORKCONFIGURATION)},
                        new FunctionalityItem(){Functionality = Functionality.SetNetworkDefaultGateway, Path = GetDeviceMandatoryFullPath(NETWORKCONFIGURATION)},
                        // System
                        new FunctionalityItem(){Functionality = Functionality.GetDeviceInformation, Path = GetDeviceMandatoryFullPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.GetSystemDateAndTime, Path = GetDeviceMandatoryFullPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.SetSystemDateAndTime, Path = GetDeviceMandatoryFullPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.SetSystemFactoryDefaults, Path = GetDeviceMandatoryFullPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.Reboot, Path = GetDeviceMandatoryFullPath(SYSTEM)},
                        // User handling
                        new FunctionalityItem(){Functionality = Functionality.GetUsers, Path = GetDeviceMandatoryFullPath(USERHANDLING)},
                        new FunctionalityItem(){Functionality = Functionality.CreateUsers, Path = GetDeviceMandatoryFullPath(USERHANDLING)},
                        new FunctionalityItem(){Functionality = Functionality.DeleteUsers, Path = GetDeviceMandatoryFullPath(USERHANDLING)},
                        new FunctionalityItem(){Functionality = Functionality.SetUser, Path = GetDeviceMandatoryFullPath(USERHANDLING)},
                        // Event handling
                        new FunctionalityItem(){Functionality = Functionality.Notify, Path = GetDeviceMandatoryFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.Subscribe, Path = GetDeviceMandatoryFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.Renew, Path = GetDeviceMandatoryFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.Unsubscribe, Path = GetDeviceMandatoryFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.EventsSetSynchronizationPoint, Path = GetDeviceMandatoryFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.CreatePullPointSubscription, Path = GetDeviceMandatoryFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.PullMessages, Path = GetDeviceMandatoryFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.GetEventProperties, Path = GetDeviceMandatoryFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.TopicFilter, Path = GetDeviceMandatoryFullPath(EVENT)},
                        new FunctionalityItem(){Functionality = Functionality.MessageContentFilter, Path = GetDeviceMandatoryFullPath(EVENT)}
                    }
                );

            return coreFunctionalities;
        }


        #endregion

        #region IProfileDefinition Members

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

        #endregion


        #region IProfileDefinition Members


        public string Scope
        {
            get { return C_PROFILE_SCOPE; }
        }

        public ProfileStatus Check(out string reason, IEnumerable<Feature> features, IEnumerable<string> scopes)
        {
            reason = string.Empty;


            StringBuilder sb = new StringBuilder();
            ProfileStatus status = ProfileStatus.NotSupported;

            sb.AppendLine(string.Format("Check profile support for {0}", Name));

            bool scopePresent = scopes.Contains(Scope);
            sb.AppendLine(string.Format("Scope {0}: \t\t{1}", Scope, scopePresent ? "PRESENT" : "NOT PRESENT"));

            if (!scopePresent)
            {
                sb.AppendFormat("Profile not supported");
            }
            else
            {
                bool profileOk = true;
                bool supported;

                Action<Feature, string> checkNextMandatory = new Action<Feature, string>(
                    (feature, displayName) =>
                    {
                        supported = features.Contains(feature);
                        LogMandatory(sb, displayName, supported);
                        profileOk = profileOk && supported;
                    });

                Action<Feature, string> checkNextOptional = new Action<Feature, string>(
                    (feature, displayName) =>
                    {
                        supported = features.Contains(feature);
                        LogOptional(sb, displayName, supported);
                    });

                checkNextMandatory(Feature.GetServices, "GetServices");
                checkNextMandatory(Feature.Digest, "HTTP Digest");
                checkNextMandatory(Feature.AccessControlService, "AccessControl Service");
                checkNextMandatory(Feature.AccessPointEnabledEvent, FeaturesHelper.GetDisplayName(Feature.AccessPointEnabledEvent));
                checkNextMandatory(Feature.AccessPointTamperingEvent, FeaturesHelper.GetDisplayName(Feature.AccessPointTamperingEvent));
                
                if (features.Contains(Feature.ExternalAuthorization))
                {
                    checkNextMandatory(Feature.RequestCredentialEvent, FeaturesHelper.GetDisplayName(Feature.RequestCredentialEvent));
                    checkNextMandatory(Feature.AccessGrantedCredentialExternalEvent, FeaturesHelper.GetDisplayName(Feature.AccessGrantedCredentialExternalEvent));
                    checkNextMandatory(Feature.AccessDeniedCredentialExternalEvent, FeaturesHelper.GetDisplayName(Feature.AccessDeniedCredentialExternalEvent));
                }

                checkNextMandatory(Feature.DoorControlService, "DoorControl Service");                
                checkNextMandatory(Feature.AccessDoor, "AccessDoor");
                checkNextMandatory(Feature.LockDoor, "LockDoor");
                checkNextMandatory(Feature.UnlockDoor, "UnlockDoor");

                Action<Feature, Feature> checkPair = new Action<Feature, Feature>(
                    (featureA, featureB) =>
                    {
                        if (features.Contains(featureB) && !features.Contains(featureA))
                        {
                            supported = false;
                            sb.AppendLine(string.Format("{0} feature is NOT SUPPORTED while {1} is SUPPORTED",
                                FeaturesHelper.GetDisplayName(featureA), FeaturesHelper.GetDisplayName(featureB)));
                        }
                        else 
                        {
                            if (features.Contains(featureB) && features.Contains(featureA))
                            {
                                sb.AppendLine(string.Format("Features {0} and {1} are SUPPORTED",
                                    FeaturesHelper.GetDisplayName(featureA), FeaturesHelper.GetDisplayName(featureB)));
                            }
                        }
                    });

                checkPair(Feature.DoorPhysicalStateEvent, Feature.DoorMonitor);
                checkPair(Feature.LockPhysicalStateEvent, Feature.LockMonitor);
                checkPair(Feature.DoubleLockPhysicalStateEvent, Feature.DoubleLockMonitor);
                checkPair(Feature.DoorAlarmEvent, Feature.DoorAlarm);
                checkPair(Feature.DoorTamperEvent, Feature.DoorTamper);
                checkPair(Feature.DoorFault, Feature.DoorFault);

                if (profileOk)
                {
                    status = ProfileStatus.Supported;
                }
                else
                {
                    status = ProfileStatus.Failed;
                }
            }
            reason = sb.ToString();
            return status;
        }


        #endregion

        void InitFeatures()
        {
            _features = new List<ProfileFeature>();

            _features.Add(new ProfileFeature() { Feature = Feature.GetServices, State = ProfileFeatureState.Mandatory});

            _features.Add(new ProfileFeature() { Feature = Feature.DeviceIoService, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.RelayOutputs, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.IPFilter, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.DigitalInputs, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.EventSeek, State = ProfileFeatureState.Optional});
            _features.Add(new ProfileFeature() { Feature = Feature.AccessControlService, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.DoorControlService, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.EnableDisableAccessPoint, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.ExternalAuthorization, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.Security, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.Security, State = ProfileFeatureState.Mandatory });
        }



    }
}
