using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUT.PACS.Simulator.Events
{
    /// <summary>
    /// Topic constants
    /// </summary>
    /// <remarks>Has been added at earlier stage, when topics were distinguished by names.
    /// Now these constants are used only when topics tree is created.</remarks>
    /// 
    public class PacsTopic
    {
        public const string DOORCONTROL = "DoorControl";
        public const string DOORMODE = "DoorMode";
        public const string DOORPHYSICALSTATE = "DoorPhysicalState";
        public const string LOCKPHYSICALSTATE = "LockPhysicalState";
        public const string DOUBLELOCKPHYSICALSTATE = "DoubleLockPhysicalState";
        public const string DOORALARM = "DoorAlarm";
        public const string DOORTAMPER = "DoorTamper";
        public const string DOORFAULT = "DoorFault";

        public const string CONFIGURATION = "Configuration";
        public const string DOOR = "Door";
        public const string ACCESSPOINT = "AccessPoint";
        public const string AREA = "Area";
        public const string CHANGED = "Changed";
        public const string REMOVED = "Removed";

        public const string ACCESSCONTROL = "AccessControl";
        public const string TAMPERING = "Tampering";
        public const string ENABLED = "Enabled";
        public const string ANONYMOUS = "Anonymous";
        public const string EXTERNAL = "External";

        public const string CREDENTIALSTATE = "CredentialState";
        public const string STATE = "State";
        public const string APBVIOLATION = "ApbViolation";
        public const string ACCESSPROFILE = "AccessProfile";
        
        public const string TIMEOUT = "Timeout";
        public const string CARD = "Card";
        public const string GRANTED  = "Granted";
        public const string ACCESSGRANTED = "AccessGranted";
        public const string ACCESSTAKEN = "AccessTaken";
        public const string ACCESSSNOTTAKEN = "AccessNotTaken";
        public const string DENIED = "Denied";
        public const string OTHER = "Other";
        public const string ACTIVESTATE = "ActiveState";
        public const string NOTENABLED = "NotEnabled";
        public const string CREDENTIALNOTACTIVE = "CredentialNotActive";
        public const string CREDENTIALNOTENABLED = "CredentialNotEnabled";
        public const string CREDENTIALNOTFOUND = "CredentialNotFound";
        public const string CREDENTIALEXPIRED = "CredentialExpired";
        public const string AUTHENTICATION = "Authentication";
        public const string INVALIDPIN = "InvalidPIN";
        public const string INVALIDBIOMETRIC = "InvalidBiometric";
        public const string INVALIDCERTIFICATE = "InvalidCertificate";
        public const string UNAUTHORIZED =  "Unauthorized";
        public const string NOTPERMITTEDATTHISTIME = "NotPermittedAtThisTime";
        public const string DURESS = "Duress";
        public const string AUTHORIZATION = "Authorization";
        public const string CREDENTIAL = "Credential";
        public const string REQUEST = "Request";

    }
}
