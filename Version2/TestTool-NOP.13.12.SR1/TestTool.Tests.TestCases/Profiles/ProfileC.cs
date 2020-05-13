using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions;

namespace TestTool.Tests.TestCases.Profiles
{
    [ProfileDefinition("Profile C", "onvif://www.onvif.org/Profile/C", ProfileVersionStatus.Release)]
    public class ProfileC : BaseProfile, IProfileDefinition
    {
        public ProfileC()
        {
            _mandatoryScopes = new List<String>();
            _mandatoryScopes.AddRange(new [] { this.GetProfileScope() });
            InitFeatures();
            InitDiscoveryTypes();
        }

        //public string Name
        //{
        //    get { return "Profile C"; }
        //}


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
        private const string ACCESSPOINTEXTERNALAUTHORIZATION = "External authorization";
        private const string PERSISTENTNOTIFICATIONSTORAGE = "Persistent notification storage";
        private const string RELAYOUTPUTS = "Relay Outputs";
        private const string DIGITALINPUTS = "Digital inputs";
        private const string IPFILTERING = "IP Filtering";
        private const string CONFIGCHANGEACCESSPOINTS = "Configuration change – Access points";
        private const string CONFIGCHANGEDOORS = "Configuration change – Doors";
        private const string CONFIGCHANGEAREAS = "Configuration change – Areas";
        private const string DURESS = "Duress";
        
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
                            //new FunctionalityItem(){Functionality = Functionality.GetCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES)},
                            //new FunctionalityItem(){Functionality = Functionality.Max, Path = GetProfileFeaturesPath(CAPABILITIES)},
                            new FunctionalityItem(){Functionality = Functionality.GetServices, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.GetServices }},
                            new FunctionalityItem(){Functionality = Functionality.MaxPullPoints, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.MaxPullPoints }},
                            new FunctionalityItem(){Functionality = Functionality.GetDeviceServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.GetServices}},
                            new FunctionalityItem(){Functionality = Functionality.GetEventsServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.GetServices}},
                            new FunctionalityItem(){Functionality = Functionality.GetDoorControlServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.DoorControlService }},
                            new FunctionalityItem(){Functionality = Functionality.GetAccessControlServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.AccessControlService }},
                            new FunctionalityItem(){Functionality = Functionality.GetWsdlUrl, Path = GetProfileFeaturesPath(CAPABILITIES)},
                            
                            new FunctionalityItem(){Functionality = Functionality.GetAccessPointInfoList, Path = GetProfileFeaturesPath(ACCESSPOINTSINFORMATION), Features = new Feature[]{ Feature.AccessControlService }},
                            new FunctionalityItem(){Functionality = Functionality.GetAccessPointInfo, Path = GetProfileFeaturesPath(ACCESSPOINTSINFORMATION), Features = new Feature[]{ Feature.AccessControlService }},

                            new FunctionalityItem(){Functionality = Functionality.GetDoorInfoList, Path = GetProfileFeaturesPath(DOORSINFORMATION), Features = new Feature[]{ Feature.DoorControlService }},
                            new FunctionalityItem(){Functionality = Functionality.GetDoorInfo, Path = GetProfileFeaturesPath(DOORSINFORMATION), Features = new Feature[]{ Feature.DoorControlService }},
                            
                            //new FunctionalityItem(){Functionality = Functionality.DoorChangedEvent, Path = GetProfileFeaturesPath(DOORSINFORMATION)},
                            //new FunctionalityItem(){Functionality = Functionality.DoorRemovedEvent, Path = GetProfileFeaturesPath(DOORSINFORMATION)},

                            new FunctionalityItem(){Functionality = Functionality.GetAreaInfoList, Path = GetProfileFeaturesPath(AREASINFORMATION), Features = new Feature[]{ Feature.AccessControlService }},
                            new FunctionalityItem(){Functionality = Functionality.GetAreaInfo, Path = GetProfileFeaturesPath(AREASINFORMATION), Features = new Feature[]{ Feature.AccessControlService }},

                            new FunctionalityItem(){Functionality = Functionality.GetAccessPointState, Path = GetProfileFeaturesPath(ACCESSPOINTSTATE), Features = new Feature[]{ Feature.AccessControlService }},
                            new FunctionalityItem(){Functionality = Functionality.AccessPointEnabledEvent, Path = GetProfileFeaturesPath(ACCESSPOINTSTATE), Features = new Feature[]{ Feature.AccessControlService }},
                            //new FunctionalityItem(){Functionality = Functionality.AccessPointTamperingEvent, Path = GetProfileFeaturesPath(ACCESSPOINTSTATE)},
                                                                                    
                            new FunctionalityItem(){Functionality = Functionality.GetDoorState, Path = GetProfileFeaturesPath(DOORSTATE), Features = new Feature[]{ Feature.DoorControlService }},
                            new FunctionalityItem(){Functionality = Functionality.DoorModeEvent, Path = GetProfileFeaturesPath(DOORSTATE), Features = new Feature[]{ Feature.DoorControlService }},
                            new FunctionalityItem(){Functionality = Functionality.DoorPhysicalStateEvent, Path = GetProfileFeaturesPath(DOORSTATE), Features = new Feature[]{ Feature.DoorPhysicalStateEvent }},
                            new FunctionalityItem(){Functionality = Functionality.DoubleLockPhysicalStateEvent, Path = GetProfileFeaturesPath(DOORSTATE), Features = new Feature[]{ Feature.DoubleLockPhysicalStateEvent }},
                            new FunctionalityItem(){Functionality = Functionality.LockPhysicalStateEvent, Path = GetProfileFeaturesPath(DOORSTATE), Features = new Feature[]{ Feature.LockPhysicalStateEvent }},
                            new FunctionalityItem(){Functionality = Functionality.DoorTamperEvent, Path = GetProfileFeaturesPath(DOORSTATE), Features = new Feature[]{ Feature.DoorTamperEvent }},
                            new FunctionalityItem(){Functionality = Functionality.DoorAlarmEvent, Path = GetProfileFeaturesPath(DOORSTATE), Features = new Feature[]{ Feature.DoorAlarmEvent }},
                            new FunctionalityItem(){Functionality = Functionality.DoorFaultEvent, Path = GetProfileFeaturesPath(DOORSTATE), Features = new Feature[]{ Feature.DoorFaultEvent }},

                            new FunctionalityItem(){Functionality = Functionality.AccessDoor, Path = GetProfileFeaturesPath(DOORCONTROL), Features = new Feature[]{ Feature.AccessDoor }},
                            new FunctionalityItem(){Functionality = Functionality.LockDoor, Path = GetProfileFeaturesPath(DOORCONTROL), Features = new Feature[]{ Feature.LockDoor }},
                            new FunctionalityItem(){Functionality = Functionality.UnlockDoor, Path = GetProfileFeaturesPath(DOORCONTROL), Features = new Feature[]{ Feature.UnlockDoor }},
                            new FunctionalityItem(){Functionality = Functionality.DoubleLockDoor, Path = GetProfileFeaturesPath(DOORCONTROL), Features = new Feature[]{ Feature.DoubleLockDoor }},
                            new FunctionalityItem(){Functionality = Functionality.BlockDoor, Path = GetProfileFeaturesPath(DOORCONTROL), Features = new Feature[]{ Feature.BlockDoor }},
                            new FunctionalityItem(){Functionality = Functionality.LockDownDoor, Path = GetProfileFeaturesPath(DOORCONTROL), Features = new Feature[]{ Feature.LockDownDoor }},
                            new FunctionalityItem(){Functionality = Functionality.LockDownReleaseDoor, Path = GetProfileFeaturesPath(DOORCONTROL), Features = new Feature[]{ Feature.LockDownDoor }},
                            new FunctionalityItem(){Functionality = Functionality.LockOpenDoor, Path = GetProfileFeaturesPath(DOORCONTROL), Features = new Feature[]{ Feature.LockOpenDoor }},
                            new FunctionalityItem(){Functionality = Functionality.LockOpenReleaseDoor, Path = GetProfileFeaturesPath(DOORCONTROL), Features = new Feature[]{ Feature.LockOpenDoor }},
                                                                                    
                            new FunctionalityItem(){Functionality = Functionality.AccessGrantedAnonymousEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS), Features = new Feature[]{ Feature.AnonymousAccess }},
                            new FunctionalityItem(){Functionality = Functionality.AccessGrantedCredentialEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS), Features = new Feature[]{ Feature.AccessGrantedCredentialEvent }},
                            new FunctionalityItem(){Functionality = Functionality.AccessTakenAnonymousEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS), Features = new Feature[]{ Feature.AccessTakenAnonymousEvent }},
                            new FunctionalityItem(){Functionality = Functionality.AccessTakenCredentialEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS), Features = new Feature[]{ Feature.AccessTakenCredentialEvent }},
                            new FunctionalityItem(){Functionality = Functionality.AccessNotTakenAnonymousEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS), Features = new Feature[]{ Feature.AccessNotTakenAnonymousEvent }},
                            new FunctionalityItem(){Functionality = Functionality.AccessNotTakenCredentialEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS), Features = new Feature[]{ Feature.AccessNotTakenCredentialEvent }},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedWithCredentialEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS), Features = new Feature[]{ Feature.AccessDeniedCredentialEvent }},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedToAnonymousEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS), Features = new Feature[]{ Feature.AnonymousAccess, Feature.ExternalAuthorization }},
                            new FunctionalityItem(){Functionality = Functionality.AccessDeniedCredentialCredentialNotFoundCardEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS), Features = new Feature[]{ Feature.AccessDeniedCredentialCredentialNotFoundCardEvent }},
                            //new FunctionalityItem(){Functionality = Functionality.DuressEvent, Path = GetProfileFeaturesPath(ACCESSCONTROLDECISIONS)},

                            new FunctionalityItem(){Functionality = Functionality.Renew, Path = GetProfileFeaturesPath(EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.Unsubscribe, Path = GetProfileFeaturesPath(EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.EventsSetSynchronizationPoint, Path = GetProfileFeaturesPath(EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.CreatePullPointSubscription, Path = GetProfileFeaturesPath(EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.PullMessages, Path = GetProfileFeaturesPath(EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.GetEventProperties, Path = GetProfileFeaturesPath(EVENT)},
                            new FunctionalityItem(){Functionality = Functionality.TopicFilter, Path = GetProfileFeaturesPath(EVENT)},
                            
                            // Profile Conditional
                            new FunctionalityItem(){Functionality = Functionality.AccessPointChangedEvent, Path = GetProfileConditionalFeaturesPath(CONFIGCHANGEACCESSPOINTS), Features = new Feature[]{ Feature.AccessPointChangedEvent }},
                            new FunctionalityItem(){Functionality = Functionality.AccessPointRemovedEvent, Path = GetProfileConditionalFeaturesPath(CONFIGCHANGEACCESSPOINTS), Features = new Feature[]{ Feature.AccessPointRemovedEvent }},

                            new FunctionalityItem(){Functionality = Functionality.DoorChangedEvent, Path = GetProfileConditionalFeaturesPath(CONFIGCHANGEDOORS), Features = new Feature[]{ Feature.DoorChangedEvent }},
                            new FunctionalityItem(){Functionality = Functionality.DoorRemovedEvent, Path = GetProfileConditionalFeaturesPath(CONFIGCHANGEDOORS), Features = new Feature[]{ Feature.DoorRemovedEvent }},

                            new FunctionalityItem(){Functionality = Functionality.AreaChangedEvent, Path = GetProfileConditionalFeaturesPath(CONFIGCHANGEAREAS), Features = new Feature[]{ Feature.AreaChangedEvent }},
                            new FunctionalityItem(){Functionality = Functionality.AreaRemovedEvent, Path = GetProfileConditionalFeaturesPath(CONFIGCHANGEAREAS), Features = new Feature[]{ Feature.AreaRemovedEvent }},

                            //Access Point Control
                            new FunctionalityItem(){Functionality = Functionality.EnableAccessPoint, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTCONTROL), Features=new Feature[]{Feature.EnableDisableAccessPoint}},
                            new FunctionalityItem(){Functionality = Functionality.DisableAccessPoint, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTCONTROL), Features=new Feature[]{Feature.EnableDisableAccessPoint}},

                            // Access point external authorization
                            new FunctionalityItem(){Functionality = Functionality.ExternalAutorization, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            //new FunctionalityItem(){Functionality = Functionality.AccessGrantedAnonymousEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            //new FunctionalityItem(){Functionality = Functionality.AccessGrantedCredentialEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            //new FunctionalityItem(){Functionality = Functionality.AccessDeniedAnonymousExternalEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            //new FunctionalityItem(){Functionality = Functionality.AccessDeniedWithCredentialEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.ExternalAuthorization}},
                            new FunctionalityItem(){Functionality = Functionality.RequestAnonymousEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.RequestAnonymousEvent}},
                            new FunctionalityItem(){Functionality = Functionality.RequestCredentialEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.RequestCredentialEvent}},
                            new FunctionalityItem(){Functionality = Functionality.RequestTimeoutEvent, Path = GetProfileConditionalFeaturesPath(ACCESSPOINTEXTERNALAUTHORIZATION), Features=new Feature[]{Feature.RequestTimeoutEvent}},
                            
                            // Duress
                            new FunctionalityItem(){Functionality = Functionality.DuressEvent, Path = GetProfileConditionalFeaturesPath(DURESS), Features=new Feature[]{Feature.DuressEvent}},

                            // Seek (Event Service)
                            new FunctionalityItem(){Functionality = Functionality.PersistentNotificationStorage, Path = GetProfileConditionalFeaturesPath(PERSISTENTNOTIFICATIONSTORAGE), Features=new Feature[]{Feature.PersistentNotificationStorage}},


                            // Relay outputs                 
                            //new FunctionalityItem(){Functionality = Functionality.GetIOServiceCapabilities, Path = GetProfileConditionalFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.DeviceIoService}},
                            //new FunctionalityItem(){Functionality = Functionality.IOGetRelayOutputs, Path = GetProfileConditionalFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.RelayOutputs}},
                            //new FunctionalityItem(){Functionality = Functionality.IOSetRelayOutputState, Path = GetProfileConditionalFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.RelayOutputs}},
                            //new FunctionalityItem(){Functionality = Functionality.IOSetRelayOutputSettings, Path = GetProfileConditionalFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.RelayOutputs}},
                            //new FunctionalityItem(){Functionality = Functionality.IOSetRelayOutputOptions, Path = GetProfileConditionalFeaturesPath(RELAYOUTPUTS), Features = new Feature[]{Feature.RelayOutputs}},
                            //// Digital inputs
                            ////new FunctionalityItem(){Functionality = Functionality.GetIOServiceCapabilities, Path = GetProfileConditionalFeaturesPath(DIGITALINPUTS)},
                            //new FunctionalityItem(){Functionality = Functionality.GetDigitalInputs, Path = GetProfileConditionalFeaturesPath(DIGITALINPUTS), Features=new Feature[]{Feature.DigitalInputs}},
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
                        new FunctionalityItem(){Functionality = Functionality.SetSystemFactoryDefault, Path = GetDeviceMandatoryFullPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.Reboot, Path = GetDeviceMandatoryFullPath(SYSTEM)},
                        // User handling
                        new FunctionalityItem(){Functionality = Functionality.GetUsers, Path = GetDeviceMandatoryFullPath(USERHANDLING)},
                        new FunctionalityItem(){Functionality = Functionality.CreateUsers, Path = GetDeviceMandatoryFullPath(USERHANDLING)},
                        new FunctionalityItem(){Functionality = Functionality.DeleteUsers, Path = GetDeviceMandatoryFullPath(USERHANDLING)},
                        new FunctionalityItem(){Functionality = Functionality.SetUser, Path = GetDeviceMandatoryFullPath(USERHANDLING)},
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

        public IEnumerable<Feature> MandatoryDiscoveryTypes { get; private set; }

        //public string Scope
        //{
        //    get { return C_PROFILE_SCOPE; }
        //}

        ///// </summary>
        //public ProfileVersionStatus Status
        //{
        //    get { return ProfileVersionStatus.Release; }
        //}


        public ProfileStatus Check(out string reason, IEnumerable<Feature> features, IEnumerable<string> scopes, Dictionary<string, object> parameters)
        {
            reason = string.Empty;

            var Name = this.GetProfileName();
            var Scope = this.GetProfileScope();

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
                        supported = features.ContainsFeature(feature);
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
                LogMandatoryFeature(sb, "Discovery");
                checkNextMandatory(Feature.DiscoveryTypesTdsDevice, "tds:Device");
                LogMandatoryFeature(sb, "Network Configuration");
                LogMandatoryFeature(sb, "System");
                LogMandatoryFeature(sb, "User Handling");
                LogMandatoryFeature(sb, "Event Handling");

                checkNextMandatory(Feature.MaxPullPoints, "EventService/GetServiceCapabilities/MaxPullPoints");
                if (profileOk)
                {
                    var v = (int)parameters["MaxPullPoints"];
                    profileOk = v >= 2;
                    sb.AppendLine(string.Format("EventService/GetServiceCapabilities/MaxPullPoints has value >= 2: {0}", profileOk ? "SUPPORTED" : "NOT SUPPORTED"));
                }

                checkNextMandatory(Feature.Digest, "HTTP Digest");

                checkNextMandatory(Feature.AccessControlService, "AccessControl Service");

                //checkNextMandatory(Feature.AreaChangedEvent, FeaturesHelper.GetDisplayName(Feature.AreaChangedEvent));
                //checkNextMandatory(Feature.AccessPointTamperingEvent, FeaturesHelper.GetDisplayName(Feature.AccessPointTamperingEvent));
                
                //if (features.Contains(Feature.ExternalAuthorization))
                //{
                //    checkNextMandatory(Feature.RequestCredentialEvent, FeaturesHelper.GetDisplayName(Feature.RequestCredentialEvent));
                //    //checkNextMandatory(Feature.AccessGrantedCredentialEvent, FeaturesHelper.GetDisplayName(Feature.AccessGrantedCredentialEvent));
                //    //checkNextMandatory(Feature.AccessDeniedWithCredentialEvent, FeaturesHelper.GetDisplayName(Feature.AccessDeniedWithCredentialEvent));
                //    checkNextMandatory(Feature.AccessDeniedCredentialEvent, FeaturesHelper.GetDisplayName(Feature.AccessDeniedCredentialEvent));
                //}

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
                checkPair(Feature.DoorFaultEvent, Feature.DoorFault);

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

            _features.Add(new ProfileFeature() { Feature = Feature.PersistentNotificationStorage, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.AccessControlService, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.DoorControlService, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.EnableDisableAccessPoint, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.ExternalAuthorization, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.Security, State = ProfileFeatureState.Mandatory });

            _features.Add(new ProfileFeature() { Feature = Feature.AccessDeniedAnonymousEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.AccessDeniedCredentialEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.AccessDeniedCredentialCredentialNotFoundCardEvent, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.AccessGrantedAnonymousEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.AccessGrantedCredentialEvent, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.AccessTakenAnonymousEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.AccessTakenCredentialEvent, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.AccessNotTakenAnonymousEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.AccessNotTakenCredentialEvent, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.RequestAnonymousEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.RequestCredentialEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.RequestTimeoutEvent, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.AreaChangedEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.AreaRemovedEvent, State = ProfileFeatureState.Optional });
            
            _features.Add(new ProfileFeature() { Feature = Feature.AccessPointChangedEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.AccessPointRemovedEvent, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.DuressEvent, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.DoorAlarmEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.DoorTamperEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.DoorFaultEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.DoorPhysicalStateEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.LockPhysicalStateEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.DoubleLockPhysicalStateEvent, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.DoorChangedEvent, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.DoorRemovedEvent, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.DoubleLockDoor, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.BlockDoor, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.LockDownDoor, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.LockOpenDoor, State = ProfileFeatureState.Optional });
        }

        void InitDiscoveryTypes()
        {
            var list = new List<Feature> { Feature.DiscoveryTypesTdsDevice };

            MandatoryDiscoveryTypes = list;
        }
    }
}
