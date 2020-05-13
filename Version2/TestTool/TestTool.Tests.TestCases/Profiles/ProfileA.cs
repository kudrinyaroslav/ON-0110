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
    [ProfileDefinition("Profile A", "onvif://www.onvif.org/Profile/A", ProfileVersionStatus.ReleaseCandidate)]
    public class ProfileA : BaseProfile, IProfileDefinition
    {
        public ProfileA()
        {
            _mandatoryScopes = new List<String>();
            _mandatoryScopes.AddRange(new [] { this.GetProfileScope() });
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
        

        private const string ROOTPROFILEMANDATORY = "Profiles Mandatory Features";
        private const string ROOTPROFILECONDITIONAL = "Profile Conditional Features";
        
        // Profiles Mandatory Features
        private const string SECURITY = "Security";
        private const string CAPABILITIES = "Capabilities";
        private const string ACCESSPROFILES = "Access Profiles";
        private const string CREDENTIALS = "Credentials";
        private const string SCHEDULES = "Schedules";
        private const string EVENT = "Event Handling";
        private const string DISCOVERY = "Discovery";
        private const string NETWORKCONFIGURATION = "Network Configuration";
        private const string SYSTEM = "System";
        private const string USERHANDLING = "User Handling";

        // Profile Conditional Features
        private const string RESETANTIPASSBACKVIOLATIONS = "Reset Antipassback Violations";
        private const string SPECIALDAYSSCHEDULE = "Special Days Schedule";
        private const string PERSISTENTNOTIFICATIONSTORAGE = "Persistent notification storage";
        private const string IPFILTERING = "IP Filtering";

        
        protected List<FunctionalityItem> LoadProfileFunctionalities()
        {
            if (_profileFunctionalities == null)
            {
                _profileFunctionalities = new List<FunctionalityItem>();

                _profileFunctionalities.AddRange(
                    new FunctionalityItem[]
                        {
                            // Profiles Mandatory Features

                            // Security
                            // HTTP Digest Authentication
                            new FunctionalityItem(){Functionality = Functionality.DigestAuthentication, Path = GetFullPath(ROOTPROFILEMANDATORY, SECURITY), Features = new Feature[]{Feature.Digest}},

                            // Capabilities
                            // GetServices (Device)
                            new FunctionalityItem(){Functionality = Functionality.GetServices, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.GetServices }},
                            // GetServiceCapabilities (Device)
                            new FunctionalityItem(){Functionality = Functionality.GetDeviceServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.GetServices}},
                            // GetServiceCapabilities (Event)
                            new FunctionalityItem(){Functionality = Functionality.GetEventsServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.GetServices}},
                            // GetServiceCapabilities (Access Rules)
                            new FunctionalityItem(){Functionality = Functionality.GetAccessRulesServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.AccessRulesService }},
                            // GetServiceCapabilities (Credential)
                            new FunctionalityItem(){Functionality = Functionality.GetCredentialServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.Credential }},
                            // GetServiceCapabilities (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.GetScheduleServiceCapabilities, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.Schedule }},
                            // GetWsdlUrl (Device)
                            new FunctionalityItem(){Functionality = Functionality.GetWsdlUrl, Path = GetProfileFeaturesPath(CAPABILITIES)},
                            // MaxPullPoint capability is supported and value is not less than 2
                            new FunctionalityItem(){Functionality = Functionality.MaxPullPoints, Path = GetProfileFeaturesPath(CAPABILITIES), Features = new Feature[]{ Feature.MaxPullPoints }},

                            // Access Profiles
                            // GetAccessProfiles (Access Rules)
                            new FunctionalityItem(){Functionality = Functionality.GetAccessProfiles, Path = GetProfileFeaturesPath(ACCESSPROFILES), Features = new Feature[]{ Feature.AccessRulesService }},
                            // GetAccessProfileList  (Access Rules)
                            new FunctionalityItem(){Functionality = Functionality.GetAccessProfileList, Path = GetProfileFeaturesPath(ACCESSPROFILES), Features = new Feature[]{ Feature.AccessRulesService }},
                            // GetAccessProfileInfo  (Access Rules)
                            new FunctionalityItem(){Functionality = Functionality.GetAccessProfileInfo, Path = GetProfileFeaturesPath(ACCESSPROFILES), Features = new Feature[]{ Feature.AccessRulesService }},
                            // GetAccessProfileInfoList  (Access Rules)
                            new FunctionalityItem(){Functionality = Functionality.GetAccessProfileInfoList, Path = GetProfileFeaturesPath(ACCESSPROFILES), Features = new Feature[]{ Feature.AccessRulesService }},
                            // CreateAccessProfile  (Access Rules)
                            new FunctionalityItem(){Functionality = Functionality.CreateAccessProfile, Path = GetProfileFeaturesPath(ACCESSPROFILES), Features = new Feature[]{ Feature.AccessRulesService }},
                            // ModifyAccessProfile  (Access Rules)
                            new FunctionalityItem(){Functionality = Functionality.ModifyAccessProfile, Path = GetProfileFeaturesPath(ACCESSPROFILES), Features = new Feature[]{ Feature.AccessRulesService }},
                            // DeleteAccessProfile  (Access Rules)
                            new FunctionalityItem(){Functionality = Functionality.DeleteAccessProfile, Path = GetProfileFeaturesPath(ACCESSPROFILES), Features = new Feature[]{ Feature.AccessRulesService }},
                            // tns1:Configuration/AccessProfile/Changed
                            new FunctionalityItem(){Functionality = Functionality.ConfigurationAccessProfileChangedEvent, Path = GetProfileFeaturesPath(ACCESSPROFILES), Features = new Feature[]{ Feature.AccessRulesService }},
                            // tns1:Configuration/AccessProfile/Removed
                            new FunctionalityItem(){Functionality = Functionality.ConfigurationAccessProfileRemovedEvent, Path = GetProfileFeaturesPath(ACCESSPROFILES), Features = new Feature[]{ Feature.AccessRulesService }},
                            
                            // Credentials
                            // GetCredentials (Credential)
                            new FunctionalityItem(){Functionality = Functionality.GetCredentials, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // GetCredentialList (Credential)
                            new FunctionalityItem(){Functionality = Functionality.GetCredentialList, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // GetCredentialInfo (Credential)
                            new FunctionalityItem(){Functionality = Functionality.GetCredentialInfo, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // GetCredentialInfoList (Credential)
                            new FunctionalityItem(){Functionality = Functionality.GetCredentialInfoList, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // CreateCredential (Credential)
                            new FunctionalityItem(){Functionality = Functionality.CreateCredential, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // ModifyCredential (Credential)
                            new FunctionalityItem(){Functionality = Functionality.ModifyCredential, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // DeleteCredential (Credential)
                            new FunctionalityItem(){Functionality = Functionality.DeleteCredential, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // GetCredentialAccessProfiles (Credential)
                            new FunctionalityItem(){Functionality = Functionality.GetCredentialAccessProfiles, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // SetCredentialAccessProfiles (Credential)
                            new FunctionalityItem(){Functionality = Functionality.SetCredentialAccessProfiles, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // DeleteCredentialAccessProfiles (Credential)
                            new FunctionalityItem(){Functionality = Functionality.DeleteCredentialAccessProfiles, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // GetCredentialIdentifiers (Credential)
                            new FunctionalityItem(){Functionality = Functionality.GetCredentialIdentifiers, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // SetCredentialIdentifier (Credential)
                            new FunctionalityItem(){Functionality = Functionality.SetCredentialIdentifier, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // DeleteCredentialIdentifier (Credential)
                            new FunctionalityItem(){Functionality = Functionality.DeleteCredentialIdentifier, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // EnableCredential (Credential)
                            new FunctionalityItem(){Functionality = Functionality.EnableCredential, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // DisableCredential (Credential)
                            new FunctionalityItem(){Functionality = Functionality.DisableCredential, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // GetCredentialState (Credential)
                            new FunctionalityItem(){Functionality = Functionality.GetCredentialState, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // GetSupportedFormatTypes (Credential)
                            new FunctionalityItem(){Functionality = Functionality.GetSupportedFormatTypes, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // tns1:Configuration/Credential/Changed
                            new FunctionalityItem(){Functionality = Functionality.ConfigurationCredentialChangedEvent, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},
                            // tns1:Configuration/Credential/Removed
                            new FunctionalityItem(){Functionality = Functionality.ConfigurationCredentialRemovedEvent, Path = GetProfileFeaturesPath(CREDENTIALS), Features = new Feature[]{ Feature.Credential }},

                            // Schedules
                            // GetSchedules (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.GetSchedules, Path = GetProfileFeaturesPath(SCHEDULES), Features = new Feature[]{ Feature.Schedule }},
                            // GetScheduleList (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.GetScheduleList, Path = GetProfileFeaturesPath(SCHEDULES), Features = new Feature[]{ Feature.Schedule }},
                            // GetScheduleInfo (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.GetScheduleInfo, Path = GetProfileFeaturesPath(SCHEDULES), Features = new Feature[]{ Feature.Schedule }},
                            // GetScheduleInfoList (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.GetScheduleInfoList, Path = GetProfileFeaturesPath(SCHEDULES), Features = new Feature[]{ Feature.Schedule }},
                            // CreateSchedule (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.CreateSchedule, Path = GetProfileFeaturesPath(SCHEDULES), Features = new Feature[]{ Feature.Schedule }},
                            // ModifySchedule (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.ModifySchedule, Path = GetProfileFeaturesPath(SCHEDULES), Features = new Feature[]{ Feature.Schedule }},
                            // DeleteSchedule (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.DeleteSchedule, Path = GetProfileFeaturesPath(SCHEDULES), Features = new Feature[]{ Feature.Schedule }},
                            // GetScheduleState (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.GetScheduleState, Path = GetProfileFeaturesPath(SCHEDULES), Features = new Feature[]{ Feature.StateReporting }},
                            // tns1:Configuration/Schedule/Changed
                            new FunctionalityItem(){Functionality = Functionality.ConfigurationScheduleChangedEvent, Path = GetProfileFeaturesPath(SCHEDULES), Features = new Feature[]{ Feature.Schedule }},
                            // tns1:Configuration/ Schedule/Removed
                            new FunctionalityItem(){Functionality = Functionality.ConfigurationScheduleRemovedEvent, Path = GetProfileFeaturesPath(SCHEDULES), Features = new Feature[]{ Feature.Schedule }},
                            // tns1:Schedule/State/Active
                            new FunctionalityItem(){Functionality = Functionality.ScheduleStateActiveEvent, Path = GetProfileFeaturesPath(SCHEDULES), Features = new Feature[]{ Feature.StateReporting }},

                            // Event Handling
                            // Renew (Event)
                            new FunctionalityItem(){Functionality = Functionality.Renew, Path = GetProfileFeaturesPath(EVENT)},
                            // Unsubscribe (Event)
                            new FunctionalityItem(){Functionality = Functionality.Unsubscribe, Path = GetProfileFeaturesPath(EVENT)},
                            // SetSynchronizationPoint (Event)
                            new FunctionalityItem(){Functionality = Functionality.EventsSetSynchronizationPoint, Path = GetProfileFeaturesPath(EVENT)},
                            // CreatePullPointSubscription (Event)
                            new FunctionalityItem(){Functionality = Functionality.CreatePullPointSubscription, Path = GetProfileFeaturesPath(EVENT)},
                            // PullMessage (Event)
                            new FunctionalityItem(){Functionality = Functionality.PullMessages, Path = GetProfileFeaturesPath(EVENT)},
                            // GetEventProperties (Event)
                            new FunctionalityItem(){Functionality = Functionality.GetEventProperties, Path = GetProfileFeaturesPath(EVENT)},
                            // TopicFilter (Event)
                            new FunctionalityItem(){Functionality = Functionality.TopicFilter, Path = GetProfileFeaturesPath(EVENT)},


                            // Profile Conditional Features
                            
                            // Reset Antipassback Violations
                            // ResetAntipassbackViolation (Credential)
                            new FunctionalityItem(){Functionality = Functionality.ResetAntipassbackViolation, Path = GetProfileConditionalFeaturesPath(RESETANTIPASSBACKVIOLATIONS), Features = new Feature[]{ Feature.ResetAntipassbackViolation }},
                            // tns1:Credential/State/ApbViolation
                            new FunctionalityItem(){Functionality = Functionality.CredentialStateApbViolationEvent, Path = GetProfileConditionalFeaturesPath(RESETANTIPASSBACKVIOLATIONS), Features = new Feature[]{ Feature.ResetAntipassbackViolation }},

                            // Special Days Schedule
                            // GetSpecialDayGroups (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.GetSpecialDayGroups, Path = GetProfileConditionalFeaturesPath(SPECIALDAYSSCHEDULE), Features = new Feature[]{ Feature.SpecialDays }},
                            // GetSpecialDayGroupList (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.GetSpecialDayGroupList, Path = GetProfileConditionalFeaturesPath(SPECIALDAYSSCHEDULE), Features = new Feature[]{ Feature.SpecialDays }},
                            // GetSpecialDayGroupInfo (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.GetSpecialDayGroupInfo, Path = GetProfileConditionalFeaturesPath(SPECIALDAYSSCHEDULE), Features = new Feature[]{ Feature.SpecialDays }},
                            // GetSpecialDayGroupInfoList (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.GetSpecialDayGroupInfoList, Path = GetProfileConditionalFeaturesPath(SPECIALDAYSSCHEDULE), Features = new Feature[]{ Feature.SpecialDays }},
                            // CreateSpecialDayGroup (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.CreateSpecialDayGroup, Path = GetProfileConditionalFeaturesPath(SPECIALDAYSSCHEDULE), Features = new Feature[]{ Feature.SpecialDays }},
                            // ModifySpecialDayGroup (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.ModifySpecialDayGroup, Path = GetProfileConditionalFeaturesPath(SPECIALDAYSSCHEDULE), Features = new Feature[]{ Feature.SpecialDays }},
                            // DeleteSpecialDayGroup (Schedule)
                            new FunctionalityItem(){Functionality = Functionality.DeleteSpecialDayGroup, Path = GetProfileConditionalFeaturesPath(SPECIALDAYSSCHEDULE), Features = new Feature[]{ Feature.SpecialDays }},
                            // tns1:Configuration/SpecialDays/Changed
                            new FunctionalityItem(){Functionality = Functionality.ConfigurationSpecialDaysChangedEvent, Path = GetProfileConditionalFeaturesPath(SPECIALDAYSSCHEDULE), Features = new Feature[]{ Feature.SpecialDays }},
                            // tns1:Configuration/SpecialDays/Removed
                            new FunctionalityItem(){Functionality = Functionality.ConfigurationSpecialDaysRemovedEvent, Path = GetProfileConditionalFeaturesPath(SPECIALDAYSSCHEDULE), Features = new Feature[]{ Feature.SpecialDays }},

                            // Persistent notification storage	Seek (Event)
                            // Seek (Event)
                            new FunctionalityItem(){Functionality = Functionality.PersistentNotificationStorage, Path = GetProfileConditionalFeaturesPath(PERSISTENTNOTIFICATIONSTORAGE), Features=new Feature[]{Feature.PersistentNotificationStorage}},

                            // IP Filtering
                            // GetIPAddressFilter (Device)
                            new FunctionalityItem(){Functionality = Functionality.GetIPAddressFilter, Path = GetProfileConditionalFeaturesPath(IPFILTERING), Features=new Feature[]{Feature.IPFilter}},
                            // SetIPAddressFilter (Device)
                            new FunctionalityItem(){Functionality = Functionality.SetIPAddressFilter, Path = GetProfileConditionalFeaturesPath(IPFILTERING), Features=new Feature[]{Feature.IPFilter}},
                            // AddIPAddressFilter (Device)
                            new FunctionalityItem(){Functionality = Functionality.AddIPAddressFilter, Path = GetProfileConditionalFeaturesPath(IPFILTERING), Features=new Feature[]{Feature.IPFilter}},
                            // RemoveIPAddressFilter (Device)
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
                        new FunctionalityItem(){Functionality = Functionality.WSDiscovery, Path = GetProfileFeaturesPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.GetDiscoveryMode, Path = GetProfileFeaturesPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.SetDiscoveryMode, Path = GetProfileFeaturesPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.GetScopes, Path = GetProfileFeaturesPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.SetScopes, Path = GetProfileFeaturesPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.AddScopes, Path = GetProfileFeaturesPath(DISCOVERY)},
                        new FunctionalityItem(){Functionality = Functionality.RemoveScopes, Path = GetProfileFeaturesPath(DISCOVERY)},
                        // Network
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
                        // System
                        new FunctionalityItem(){Functionality = Functionality.GetDeviceInformation, Path = GetProfileFeaturesPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.GetSystemDateAndTime, Path = GetProfileFeaturesPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.SetSystemDateAndTime, Path = GetProfileFeaturesPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.SetSystemFactoryDefault, Path = GetProfileFeaturesPath(SYSTEM)},
                        new FunctionalityItem(){Functionality = Functionality.Reboot, Path = GetProfileFeaturesPath(SYSTEM)},
                        // User handling
                        new FunctionalityItem(){Functionality = Functionality.GetUsers, Path = GetProfileFeaturesPath(USERHANDLING)},
                        new FunctionalityItem(){Functionality = Functionality.CreateUsers, Path = GetProfileFeaturesPath(USERHANDLING)},
                        new FunctionalityItem(){Functionality = Functionality.DeleteUsers, Path = GetProfileFeaturesPath(USERHANDLING)},
                        new FunctionalityItem(){Functionality = Functionality.SetUser, Path = GetProfileFeaturesPath(USERHANDLING)},
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

                checkNextMandatory(Feature.AccessRulesService, "Access Rules Service");
                checkNextMandatory(Feature.Credential, "Credential Service");
                checkNextMandatory(Feature.Schedule, "Schedule Service");


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

            _features.Add(new ProfileFeature() { Feature = Feature.Digest, State = ProfileFeatureState.Mandatory });

            _features.Add(new ProfileFeature() { Feature = Feature.GetServices, State = ProfileFeatureState.Mandatory});

            _features.Add(new ProfileFeature() { Feature = Feature.MaxPullPoints, State = ProfileFeatureState.Mandatory });


            _features.Add(new ProfileFeature() { Feature = Feature.IPFilter, State = ProfileFeatureState.Optional });

            _features.Add(new ProfileFeature() { Feature = Feature.PersistentNotificationStorage, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.Credential, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.ResetAntipassbackViolation, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.Schedule, State = ProfileFeatureState.Mandatory });
            _features.Add(new ProfileFeature() { Feature = Feature.StateReporting, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.SpecialDays, State = ProfileFeatureState.Optional });
            _features.Add(new ProfileFeature() { Feature = Feature.AccessRulesService, State = ProfileFeatureState.Mandatory });
        }

        void InitDiscoveryTypes()
        {
            var list = new List<Feature> { Feature.DiscoveryTypesTdsDevice };

            MandatoryDiscoveryTypes = list;
        }
    }
}
