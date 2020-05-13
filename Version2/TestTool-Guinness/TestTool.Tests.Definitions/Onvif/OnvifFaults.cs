namespace TestTool.Tests.Definitions.Onvif
{
    /// <summary>
    /// Faults defined in ONVIF core specification
    /// </summary>
    public class OnvifFaults
    {
        public const string NotAuthorized = "Sender/NotAuthorized";
        public const string SenderNotAuthorized = "Sender/NotAuthorized/SenderNotAuthorized";
        public const string NoSuchService = "Receiver/ActionNotSupported/NoSuchService";
        public const string ActionNotSupported = "Receiver/ActionNotSupported";
        public const string InvalidToken = "Sender/InvalidArgVal/InvalidToken";
        public const string NoProfile = "Sender/InvalidArgVal/NoProfile";

    }
}
