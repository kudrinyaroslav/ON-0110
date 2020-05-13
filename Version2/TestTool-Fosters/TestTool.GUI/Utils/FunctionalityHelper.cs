using System.Collections.Generic;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Utility methods for "Functionality" test property.
    /// </summary>
    class FunctionalityHelper
    {
        /// <summary>
        /// Gets display name
        /// </summary>
        /// <param name="functionality"></param>
        /// <returns></returns>
        public static string GetDisplayName(Functionality functionality)
        {
            if (_displayNames == null)
            {
                InitDisplayNames();
            }
            if (_displayNames.ContainsKey(functionality))
            {
                return _displayNames[functionality];
            }
            else
            {
                return functionality.ToString();
            }
        }

        private static Dictionary<Functionality, string> _displayNames;
        
        /// <summary>
        /// Initializes disply names where "ToString" is not OK.
        /// </summary>
        static void InitDisplayNames()
        {
            _displayNames = new Dictionary<Functionality, string>();

            _displayNames.Add(Functionality.WsSecurity, "WS-UsernameToken Authentication");
            _displayNames.Add(Functionality.DigestAuthentication, "HTTP Digest Authentication");
            _displayNames.Add(Functionality.WSDiscovery, "WS-Discovery");
            _displayNames.Add(Functionality.MediaSetSynchronizationPoint, "SetSynchronizationPoint");
            _displayNames.Add(Functionality.EventsSetSynchronizationPoint, "SetSynchronizationPoint (Event Service)");
            _displayNames.Add(Functionality.GetDns, "GetDNS");
            _displayNames.Add(Functionality.SetDns, "SetDNS");
            _displayNames.Add(Functionality.MediaStreamingRtsp, "Media Streaming RTSP ");
            _displayNames.Add(Functionality.MediaStreamingRtspJpegHeaderExtension, "Media Streaming RTSP  (JPEG RTP Header Extention)");
            _displayNames.Add(Functionality.GetPtzConfigurations, "GetConfigurations");
            _displayNames.Add(Functionality.GetPtzConfiguration, "GetConfiguration");
            _displayNames.Add(Functionality.GetPtzConfigurationOptions, "GetConfigurationOptions");
            _displayNames.Add(Functionality.AddPTZConfiguration, "AddPTZConfiguration");
            _displayNames.Add(Functionality.RemovePTZConfiguration, "RemovePTZConfiguration");
            _displayNames.Add(Functionality.SetPtzConfiguration, "SetConfiguration");
            _displayNames.Add(Functionality.PtzStop, "Stop");
            _displayNames.Add(Functionality.PtzGetStatus, "GetStatus");
            _displayNames.Add(Functionality.PtzAbsoluteMove, "AbsoluteMove");
            _displayNames.Add(Functionality.PtzRelativeMove, "RelativeMove");
            _displayNames.Add(Functionality.PtzGetNodes, "GetNodes");
            _displayNames.Add(Functionality.PtzGetNode, "GetNode");
            _displayNames.Add(Functionality.PtzContinuousMove, "ContinuousMove");
            _displayNames.Add(Functionality.PtzSetPreset, "SetPreset");
            _displayNames.Add(Functionality.PtzGetPreset, "GetPreset");
            _displayNames.Add(Functionality.PtzGotoPreset, "GotoPreset");
            _displayNames.Add(Functionality.PtzRemovePreset, "RemovePreset");


            _displayNames.Add(Functionality.DoorModeEvent, "Door/State/DoorMode");
            _displayNames.Add(Functionality.DoorPhysicalStateEvent, "Door/State/DoorPhysicalState");
            _displayNames.Add(Functionality.DoubleLockPhysicalStateEvent, "Door/State/LockPhysicalState");
            _displayNames.Add(Functionality.LockPhysicalStateEvent, "Door/State/DoubleLockPhysicalState");
            _displayNames.Add(Functionality.DoorTamperEvent, "Door/State/DoorTamper");
            _displayNames.Add(Functionality.DoorAlarmEvent, "Door/State/DoorAlarm");
            _displayNames.Add(Functionality.DoorChangedEvent, "Door/Changed");
            _displayNames.Add(Functionality.DoorRemovedEvent, "Door/Removed");
            _displayNames.Add(Functionality.DoorFaultEvent, "Door/State/DoorFault");
            _displayNames.Add(Functionality.AccessGrantedAnonymousEvent, "AccessControl/AccessGranted/Anonymous");
            _displayNames.Add(Functionality.AccessGrantedCredentialEvent, "AccessControl/AccessGranted/Credential");
            _displayNames.Add(Functionality.AccessGrantedAnonymousExternalEvent, "AccessControl/AccessGranted/Anonymous/External");
            _displayNames.Add(Functionality.AccessGrantedCredentialExternalEvent, "AccessControl/AccessGranted/Credential/External");
            _displayNames.Add(Functionality.AccessTakenAnonymousEvent, "AccessControl/AccessTaken/Anonymous");
            _displayNames.Add(Functionality.AccessTakenCredentialEvent, "AccessControl/AccessTaken/Credential");
            _displayNames.Add(Functionality.AccessNotTakenAnonymousEvent, "AccessControl/AccessNotTaken/Anonymous");
            _displayNames.Add(Functionality.AccessNotTakenCredentialEvent, "AccessControl/AccessNotTaken/Credential");
            _displayNames.Add(Functionality.AccessDeniedCredentialCredentialNotEnabledEvent, "AccessControl/Denied/Credential/CredentialNotEnabled");
            _displayNames.Add(Functionality.AccessDeniedCredentialCredentialNotActiveEvent, "AccessControl/Denied/Credential/CredentialNotActive");
            _displayNames.Add(Functionality.AccessDeniedCredentialCredentialExpiredEvent, "AccessControl/Denied/Credential/CredentialExpired");
            _displayNames.Add(Functionality.AccessDeniedCredentialInvalidPINEvent, "AccessControl/Denied/Credential/InvalidPIN");
            _displayNames.Add(Functionality.AccessDeniedCredentialNotPermittedAtThisTimeEvent, "AccessControl/Denied/Credential/NotPermittedAtThisTime");
            _displayNames.Add(Functionality.AccessDeniedCredentialUnauthorizedEvent, "AccessControl/Denied/Credential/Unauthorized");
            _displayNames.Add(Functionality.AccessDeniedCredentialExternalEvent, "AccessControl/Denied/Credential/External");
            _displayNames.Add(Functionality.AccessDeniedCredentialOtherEvent, "AccessControl/Denied/Credential/Other");

            _displayNames.Add(Functionality.AccessDeniedAnonymousNotPermittedAtThisTimeEvent, "AccessControl/Denied/Anonymous/NotPermittedAtThisTime");
            _displayNames.Add(Functionality.AccessDeniedAnonymousUnauthorizedEvent, "AccessControl/Denied/Anonymous/Unauthorized");
            _displayNames.Add(Functionality.AccessDeniedAnonymousExternalEvent, "AccessControl/Denied/Anonymous/External");
            _displayNames.Add(Functionality.AccessDeniedAnonymousOtherEvent, "AccessControl/Denied/Anonymous/Other");
            _displayNames.Add(Functionality.AccessDeniedCredentialCredentialNotFoundCardEvent, "AccessControl/Denied/Credential/CredentialNotFound/Card");
            _displayNames.Add(Functionality.DuressAnonymousEvent, "AccessControl/Duress/Anonymous");
            _displayNames.Add(Functionality.DuressCredentialEvent, "AccessControl/Duress/Credential");
            _displayNames.Add(Functionality.RequestAnonymousEvent, "AccessControl/Request/Anonymous");
            _displayNames.Add(Functionality.RequestCredentialEvent, "AccessControl/Request/Credential");
            _displayNames.Add(Functionality.RequestTimeoutAnonymousEvent, "AccessControl/Request/Timeout/Anonymous");

            _displayNames.Add(Functionality.RequestTimeoutCredentialEvent, "AccessControl/Request/Timeout/Credential");
            _displayNames.Add(Functionality.AccessPointChangedEvent, "AccessPoint/Changed");
            _displayNames.Add(Functionality.AccessPointRemovedEvent, "AccessPoint/Removed");
            _displayNames.Add(Functionality.AccessPointEnabledEvent, "AccessPoint/State/Enabled");
            _displayNames.Add(Functionality.AccessPointTamperingEvent, "AccessPoint/State/Tampering");
            _displayNames.Add(Functionality.AreaChangedEvent, "Area/Changed");
            _displayNames.Add(Functionality.AreaRemovedEvent, "Area/Removed");

            _displayNames.Add(Functionality.GetDeviceServiceCapabilities, "GetServiceCapabilities (Device Service)");
            _displayNames.Add(Functionality.GetEventsServiceCapabilities, "GetServiceCapabilities (Events Service)");
            _displayNames.Add(Functionality.GetDoorControlServiceCapabilities, "GetServiceCapabilities (Door Control Service)");
            _displayNames.Add(Functionality.GetAccessControlServiceCapabilities, "GetServiceCapabilities (Access Control Service)");
            _displayNames.Add(Functionality.EventsSeek, "Seek");
            
            _displayNames.Add(Functionality.GetIOServiceCapabilities, "GetServiceCapabilities (IO Service)");
            _displayNames.Add(Functionality.IOGetRelayOutputs, "GetRelayOutputs");
            _displayNames.Add(Functionality.IOSetRelayOutputOptions, "SetRelayOutputOptions");
            _displayNames.Add(Functionality.IOSetRelayOutputSettings, "SetRelayOutputSettings");
            _displayNames.Add(Functionality.IOSetRelayOutputState, "SetRelayOutputState");


            _displayNames.Add(Functionality.TopicFilter, "Topic Filter");
            _displayNames.Add(Functionality.MessageContentFilter, "Message Content Filter");


            /*
                _displayNames.Add(Functionality., "");
             */



        }

    }
}
