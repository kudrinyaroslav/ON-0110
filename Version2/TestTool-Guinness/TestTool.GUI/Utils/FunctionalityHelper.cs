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


            _displayNames.Add(Functionality.XPathDialect, "X-Path Dialect");
            _displayNames.Add(Functionality.RecordingStateEvent, "Recording/State Event");
            _displayNames.Add(Functionality.TrackStateEvent, "Track/State Event");
            _displayNames.Add(Functionality.RecordingCreationDeletionEvent, "Recording Creation/Deletion Event");
            _displayNames.Add(Functionality.TrackCreationDeletionEvent, "TrackCreation/Deletion Event");
            _displayNames.Add(Functionality.ChangeStateEvent, "ChangeState Event");
            _displayNames.Add(Functionality.ConnectionFailedEvent, "ConnectionFailed Event");

            
            /*
                _displayNames.Add(Functionality., "");
            */



        }

    }
}
