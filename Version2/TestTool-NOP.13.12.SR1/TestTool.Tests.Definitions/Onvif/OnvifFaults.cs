namespace TestTool.Tests.Definitions.Onvif
{
    /// <summary>
    /// Faults defined in ONVIF core specification
    /// </summary>
    public class OnvifFaults
    {
        public const string NotAuthorized =         "Sender/NotAuthorized";
        public const string SenderNotAuthorized =   "Sender/NotAuthorized/SenderNotAuthorized";
        public const string InvalidToken =          "Sender/InvalidArgVal/InvalidToken";
        public const string NoProfile =             "Sender/InvalidArgVal/NoProfile";
        public const string NoRecording =           "Sender/InvalidArgVal/NoRecording";
        public const string NoRecordingJob =        "Sender/InvalidArgVal/NoRecordingJob";
        public const string NotFound =              "Sender/InvalidArgVal/NotFound";
        public const string NoTrack =               "Sender/InvalidArgVal/NoTrack";
        
        public const string ReceiverMaxNumber =     "Receiver/Action/MaxReceivers";
        public const string ActionNotSupported =    "Receiver/ActionNotSupported";
        public const string NoSuchService =         "Receiver/ActionNotSupported/NoSuchService";
        public const string UnknownToken =          "Sender/InvalidArgVal/UnknownToken";
    }
}
