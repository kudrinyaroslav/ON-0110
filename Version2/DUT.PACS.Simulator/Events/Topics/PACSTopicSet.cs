namespace DUT.PACS.Simulator.Events
{
    /// <summary>
    /// PACS-specific topic set.
    /// </summary>
    public class PACSTopicSet : TopicSet
    {
        private PACSTopicSet()
        {
        }

        private static string ONVIFEVENTS = "http://www.onvif.org/ver10/topics";
        private static string TNS1 = "tns1";

        private static string TDCNAMESPACE = "http://www.onvif.org/ver10/doorcontrol/wsdl";
        private static string TDC = "tdc";

        private static string TACNAMESPACE = "http://www.onvif.org/ver10/accesscontrol/wsdl";
        private static string TAC = "tac";

        private static string TARNAMESPACE = "http://www.onvif.org/ver10/accessrules/wsdl";
        private static string TAR = "tar";

        private static string TCRNAMESPACE = "http://www.onvif.org/ver10/credential/wsdl";
        private static string TCR = "tar";

        private static string TSCNAMESPACE = "http://www.onvif.org/ver10/schedule/wsdl";
        private static string TSC = "tsc";

        private static string PTNAMESPACE = "http://www.onvif.org/ver10/pacs";
        private static string PT = "pt";

        private const string XSNAMESPACE = "http://www.w3.org/2001/XMLSchema";

        private static PACSTopicSet _instance;
        public static PACSTopicSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    Initialize();
                }
                return _instance;
            }
        }

        #region Topics


        /////////////////////////////////////////////////////////////////////////////
        //  
        //  References to these topics are convenient when notifications on known topics are sent.
        //  It's also possible to get topic using name when it's necessary.
        //

        private Topic _doorControl;
        public Topic DoorControlTopic
        {
            get { return _doorControl; }
        }

        private Topic _doorMode;
        public Topic DoorModeTopic
        {
            get { return _doorMode; }
        }

        Topic _doorPhysicalState;
        public Topic DoorPhysicalState
        {
            get { return _doorPhysicalState; }
        }
        Topic _doubleLockPhysicalState;
        public Topic DoubleLockPhysicalState
        {
            get { return _doubleLockPhysicalState; }
        }
        Topic _lockPhysicalState;
        public Topic LockPhysicalState
        {
            get { return _lockPhysicalState; }
        }
        Topic _doorTamper;
        public Topic DoorTamper
        {
            get { return _doorTamper; }
        }
        Topic _doorAlarm;
        public Topic DoorAlarm
        {
            get { return _doorAlarm; }
        }
        Topic _doorFault;
        public Topic DoorFault
        {
            get { return _doorFault; }
        }
        Topic _doorTopicsNamespace;
        public Topic DoorConfiguration
        {
            get { return _doorTopicsNamespace; }
        }
        Topic _door;
        public Topic Door
        {
            get { return _door; }
        }
        Topic _doorSet;
        public Topic DoorSet
        {
            get { return _doorSet; }
        }
        Topic _doorRemoved;
        public Topic DoorRemoved
        {
            get { return _doorRemoved; }
        }

        private Topic _accessControl;
        public Topic AccessControl
        {
            get { return _accessControl; }
        }

        private Topic _accessControlAccessPoint;
        public Topic AccessControlAccessPoint
        {
            get { return _accessControlAccessPoint; }
        }

        private Topic _accessControlAccessPointEnabled;
        public Topic AccessControlEnabled
        {
            get { return _accessControlAccessPointEnabled; }
        }

        private Topic _accessControlAccessPointTampering;
        public Topic AccessControlTampering
        {
            get { return _accessControlAccessPointTampering; }
        }

        private Topic _accessControlAccessGranted;
        public Topic AccessControlAccessGranted
        {
            get { return _accessControlAccessGranted; }
        }

        private Topic _accessGrantedAnonymous;
        public Topic AccessGrantedAnonymous
        {
            get { return _accessGrantedAnonymous; }
        }

        //private Topic _accessGrantedAnonymousExternal;
        //public Topic AccessGrantedAnonymousExternal
        //{
        //    get { return _accessGrantedAnonymousExternal; }
        //}

        private Topic _accessGrantedCredential;
        public Topic AccessGrantedCredential
        {
            get { return _accessGrantedCredential; }
        }

        private Topic _deniedCredential;
        public Topic DeniedCredential
        {
            get { return _deniedCredential; }
        }

        //private Topic _accessGrantedCredentialExternal;
        //public Topic AccessGrantedCredentialExternal
        //{
        //    get { return _accessGrantedCredentialExternal; }
        //}

        private Topic _requestTimeoutAnonymous;
        public Topic RequestTimeoutAnonymous
        {
            get { return _requestTimeoutAnonymous; }
        }

        private Topic _requestTimeoutCredential;
        public Topic RequestTimeoutCredential
        {
            get { return _requestTimeoutCredential; }
        }

        private Topic _deniedAnonymous;
        public Topic DeniedAnonymous
        {
            get { return _deniedAnonymous; }
        }

        #region Credential

        private Topic _credential;
        public Topic Credential
        {
            get { return _credential; }
        }

        private Topic _credentialState;
        public Topic CredentialState
        {
            get { return _credentialState; }
        }

        private Topic _credentialClientUpdated;
        public Topic CredentialClientUpdated
        {
            get { return _credentialClientUpdated; }
        }

        private Topic _credentialStateApbViolation;
        public Topic CredentialStateApbViolation
        {
            get { return _credentialStateApbViolation; }
        }

        private Topic _credentialStateEnabled;
        public Topic CredentialStateEnabled
        {
            get { return _credentialStateEnabled; }
        }

        private Topic _configurationCredential;
        public Topic ConfigurationCredential
        {
            get { return _configurationCredential; }
        }

        private Topic _configurationCredentialChanged;
        public Topic ConfigurationCredentialChanged
        {
            get { return _configurationCredentialChanged; }
        }

        private Topic _configurationCredentialRemoved;
        public Topic ConfigurationCredentialRemoved
        {
            get { return _configurationCredentialRemoved; }
        }

        #endregion

        #region Access Rules

        private Topic _configurationAccessProfile;
        public Topic ConfigurationAccessProfile
        {
            get { return _configurationAccessProfile; }
        }

        private Topic _configurationAccessProfileChanged;
        public Topic ConfigurationAccessProfileChanged
        {
            get { return _configurationAccessProfileChanged; }
        }

        private Topic _configurationAccessProfileRemoved;
        public Topic ConfigurationAccessProfileRemoved
        {
            get { return _configurationAccessProfileRemoved; }
        }

        #endregion

        #region Schedule

        Topic _schedule;
        public Topic Schedule
        {
            get { return _schedule; }
        }

        private Topic _scheduleState;
        public Topic ScheduleState
        {
            get { return _scheduleState; }
        }

        private Topic _scheduleStateActive;
        public Topic ScheduleStateActive
        {
            get { return _scheduleStateActive; }
        }

        private Topic _configurationSchedule;
        public Topic ConfigurationSchedule
        {
            get { return _configurationSchedule; }
        }

        private Topic _configurationScheduleChanged;
        public Topic ConfigurationScheduleChanged
        {
            get { return _configurationScheduleChanged; }
        }

        private Topic _configurationScheduleRemoved;
        public Topic ConfigurationScheduleRemoved
        {
            get { return _configurationScheduleRemoved; }
        }

        private Topic _configurationSpecialDays;
        public Topic ConfigurationSpecialDays
        {
            get { return _configurationSpecialDays; }
        }

        private Topic _configurationSpecialDaysChanged;
        public Topic ConfigurationSpecialDaysChanged
        {
            get { return _configurationSpecialDaysChanged; }
        }

        private Topic _configurationSpecialDaysRemoved;
        public Topic ConfigurationSpecialDaysRemoved
        {
            get { return _configurationSpecialDaysRemoved; }
        }

        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes _instance structure.
        /// </summary>
        static void Initialize()
        {
            _instance = new PACSTopicSet();

            SimpleItemDescription doorToken = new SimpleItemDescription("DoorToken", "ReferenceToken", PTNAMESPACE);

            // Door

            //Door
            Topic door = new Topic(PacsTopic.DOOR, ONVIFEVENTS, TNS1, false);
            _instance._door = door;

            //Door/State
            Topic doorState = new Topic("State", null, null, false);

            Topic doorControl = new Topic(PacsTopic.DOORCONTROL, null, null);
            _instance._doorControl = doorControl;

            //DoorControl/DoorMode[topic]
            Topic doorMode = new Topic(PacsTopic.DOORMODE, null, null, true);
            _instance._doorMode = doorMode;
            doorMode.MessageDescription.IsProperty = true;
            doorMode.MessageDescription.SourceItems.Add(doorToken);
            doorMode.MessageDescription.DataItems.Add(new SimpleItemDescription("State", "DoorMode", TDCNAMESPACE));

            //DoorControl/DoorPhysicalState[topic]

            Topic doorPhysicalState = new Topic(PacsTopic.DOORPHYSICALSTATE, null, null, true);
            _instance._doorPhysicalState = doorPhysicalState;
            doorPhysicalState.MessageDescription.IsProperty = true;
            doorPhysicalState.MessageDescription.SourceItems.Add(doorToken);
            doorPhysicalState.MessageDescription.DataItems.Add(new SimpleItemDescription("State", "DoorPhysicalState", TDCNAMESPACE));


            //DoorControl/DoubleLockPhysicalState[topic]

            Topic doubleLockPhysicalState = new Topic(PacsTopic.DOUBLELOCKPHYSICALSTATE, null, null, true);
            _instance._doubleLockPhysicalState = doubleLockPhysicalState;
            doubleLockPhysicalState.MessageDescription.IsProperty = true;
            doubleLockPhysicalState.MessageDescription.SourceItems.Add(doorToken);
            doubleLockPhysicalState.MessageDescription.DataItems.Add(new SimpleItemDescription("State", "LockPhysicalState", TDCNAMESPACE));

            //DoorControl/LockPhysicalState[topic]

            Topic lockPhysicalState = new Topic(PacsTopic.LOCKPHYSICALSTATE, null, null, true);
            _instance._lockPhysicalState = lockPhysicalState;
            lockPhysicalState.MessageDescription.IsProperty = true;
            lockPhysicalState.MessageDescription.SourceItems.Add(doorToken);
            lockPhysicalState.MessageDescription.DataItems.Add(new SimpleItemDescription("State", "LockPhysicalState", TDCNAMESPACE));

            //DoorControl/DoorTamper[topic]

            Topic doorTamper = new Topic(PacsTopic.DOORTAMPER, null, null, true);
            _instance._doorTamper = doorTamper;
            doorTamper.MessageDescription.IsProperty = true;
            doorTamper.MessageDescription.SourceItems.Add(doorToken);
            doorTamper.MessageDescription.DataItems.Add(new SimpleItemDescription("State", "DoorTamperState", TDCNAMESPACE));

            //DoorControl/DoorAlarm[topic]
            Topic doorAlarm = new Topic(PacsTopic.DOORALARM, null, null, true);
            _instance._doorAlarm = doorAlarm;
            doorAlarm.MessageDescription.IsProperty = true;
            doorAlarm.MessageDescription.SourceItems.Add(doorToken);
            doorAlarm.MessageDescription.DataItems.Add(new SimpleItemDescription("State", "DoorAlarmState", TDCNAMESPACE));

            //Door/State/DoorFault[topic]
            Topic doorFault = new Topic(PacsTopic.DOORFAULT, null, null, true);
            _instance._doorFault = doorFault;
            doorFault.MessageDescription.IsProperty = true;
            doorFault.MessageDescription.SourceItems.Add(doorToken);
            doorFault.MessageDescription.DataItems.Add(new SimpleItemDescription("State", "DoorFaultState", TDCNAMESPACE));
            doorFault.MessageDescription.DataItems.Add(new SimpleItemDescription("Reason", "string", XSNAMESPACE));            

            //Door/Set[topic]

            Topic doorChanged = new Topic(PacsTopic.CHANGED, null, null, true);
            _instance._doorSet = doorChanged;
            doorChanged.MessageDescription.IsProperty = false;
            doorChanged.MessageDescription.SourceItems.Add(doorToken);

            //Door/Removed[topic]
            Topic doorRemoved = new Topic(PacsTopic.REMOVED, null, null, true);
            _instance._doorRemoved = doorRemoved;
            doorRemoved.MessageDescription.IsProperty = false;
            doorRemoved.MessageDescription.SourceItems.Add(doorToken);

            Topic configurationDoor = new Topic(PacsTopic.DOOR, null, null, false);

            /////////////////////////////////////////////////////////////////////////////////
            //                              AccessControl                                  //
            /////////////////////////////////////////////////////////////////////////////////

            #region AccessControl

            // SimpleItemDescriptions
            var accessPointToken = new SimpleItemDescription("AccessPointToken", "ReferenceToken", PTNAMESPACE);
            var areaToken = new SimpleItemDescription("AreaToken", "ReferenceToken", PTNAMESPACE);
            var credentialToken = new SimpleItemDescription("CredentialToken", "ReferenceToken", PTNAMESPACE);
            var credentialsHolder = new SimpleItemDescription("CredentialHolderName", "string", XSNAMESPACE);
            var reason = new SimpleItemDescription("Reason", "string", XSNAMESPACE);
            var clientUpdated = new SimpleItemDescription("ClientUpdated", "boolean", XSNAMESPACE);
            var card = new SimpleItemDescription("Card", "string", XSNAMESPACE);
            var external = new SimpleItemDescription("External", "boolean", XSNAMESPACE);

            Topic accessControl = new Topic(PacsTopic.ACCESSCONTROL, ONVIFEVENTS, TNS1);
            _instance._accessControl = accessControl;
            
            //AccessControl/AccessGranted
            Topic accessControlAccessGranted = new Topic(PacsTopic.ACCESSGRANTED, null, null);
            _instance._accessControlAccessGranted = accessControlAccessGranted;

            //AccessControl/AccessGranted/Anonymous [topic]
            Topic accessGrantedAnonymous = new Topic(PacsTopic.ANONYMOUS, null, null, true);
            _instance._accessGrantedAnonymous = accessGrantedAnonymous;
            accessGrantedAnonymous.MessageDescription.IsProperty = false;
            accessGrantedAnonymous.MessageDescription.SourceItems.Add(accessPointToken);
            accessGrantedAnonymous.MessageDescription.DataItems.Add(external);

            ////AccessControl/AccessGranted/Anonymous/External [topic]
            //Topic accessGrantedAnonymousExternal = new Topic(PacsTopic.EXTERNAL, null, null, true);
            //_instance._accessGrantedAnonymousExternal = accessGrantedAnonymousExternal;
            //accessGrantedAnonymousExternal.MessageDescription.IsProperty = false;
            //accessGrantedAnonymousExternal.MessageDescription.SourceItems.Add(accessPointToken);

            //AccessControl/AccessGranted/Credential [topic]
            Topic accessGrantedCredential = new Topic(PacsTopic.CREDENTIAL, null, null, true);
            _instance._accessGrantedCredential = accessGrantedCredential;
            accessGrantedCredential.MessageDescription.IsProperty = false;
            accessGrantedCredential.MessageDescription.SourceItems.Add(accessPointToken);
            accessGrantedCredential.MessageDescription.DataItems.Add(credentialToken);
            accessGrantedCredential.MessageDescription.DataItems.Add(credentialsHolder);
            accessGrantedCredential.MessageDescription.DataItems.Add(external);

            ////AccessControl/AccessGranted/Credential/External [topic]
            //Topic accessGrantedCredentialExternal = new Topic(PacsTopic.EXTERNAL, null, null, true);
            //_instance._accessGrantedCredentialExternal = accessGrantedCredentialExternal;
            //accessGrantedCredentialExternal.MessageDescription.IsProperty = false;
            //accessGrantedCredentialExternal.MessageDescription.SourceItems.Add(accessPointToken);
            //accessGrantedCredentialExternal.MessageDescription.DataItems.Add(credentialToken);
            //accessGrantedCredentialExternal.MessageDescription.DataItems.Add(credentialsHolder);

            // AccessControl/AccessTaken
            Topic accessTaken = new Topic(PacsTopic.ACCESSTAKEN, null, null);

            // AccessControl/AccessTaken/Anonymous
            Topic accessTakenAnonymous = new Topic(PacsTopic.ANONYMOUS, null, null, true);
            accessTakenAnonymous.MessageDescription.IsProperty = false;
            accessTakenAnonymous.MessageDescription.SourceItems.Add(accessPointToken);

            // AccessControl/AccessTaken/Credential
            Topic accessTakenCredential = new Topic(PacsTopic.CREDENTIAL, null, null, true);
            accessTakenCredential.MessageDescription.IsProperty = false;
            accessTakenCredential.MessageDescription.SourceItems.Add(accessPointToken);
            accessTakenCredential.MessageDescription.DataItems.Add(credentialToken);
            accessTakenCredential.MessageDescription.DataItems.Add(credentialsHolder);

            // AccessControl/AccessNotTaken
            Topic accessNotTaken = new Topic(PacsTopic.ACCESSSNOTTAKEN, null, null);

            // AccessControl/AccessNotTaken/Anonymous
            Topic accessNotTakenAnonymous = new Topic(PacsTopic.ANONYMOUS, null, null, true);
            accessNotTakenAnonymous.MessageDescription.IsProperty = false;
            accessNotTakenAnonymous.MessageDescription.SourceItems.Add(accessPointToken);

            // AccessControl/AccessNotTaken/Credential
            Topic accessNotTakenCredential = new Topic(PacsTopic.CREDENTIAL, null, null, true);
            accessNotTakenCredential.MessageDescription.IsProperty = false;
            accessNotTakenCredential.MessageDescription.SourceItems.Add(accessPointToken);
            accessNotTakenCredential.MessageDescription.DataItems.Add(credentialToken);
            accessNotTakenCredential.MessageDescription.DataItems.Add(credentialsHolder);

            // AccessControl/Denied  - TOPICS NAMESPACE
            Topic denied = new Topic(PacsTopic.DENIED, null, null);

            // AccessControl/Denied/Credential  - TOPICS NAMESPACE
            Topic deniedCredential = new Topic(PacsTopic.CREDENTIAL, null, null, true);

            // AccessControl/Denied/Credential
            _instance._deniedCredential = deniedCredential;
            deniedCredential.MessageDescription.IsProperty = false;
            deniedCredential.MessageDescription.SourceItems.Add(accessPointToken);
            deniedCredential.MessageDescription.DataItems.Add(credentialToken);
            deniedCredential.MessageDescription.DataItems.Add(credentialsHolder);
            deniedCredential.MessageDescription.DataItems.Add(reason);
            deniedCredential.MessageDescription.DataItems.Add(external);

            //// AccessControl/Denied/Credential/CredentialNotEnabled
            //Topic deniedCredentialNotEnabled = new Topic(PacsTopic.CREDENTIALNOTENABLED, null, null, true);
            //deniedCredentialNotEnabled.MessageDescription.IsProperty = false;
            //deniedCredentialNotEnabled.MessageDescription.SourceItems.Add(accessPointToken);
            //deniedCredentialNotEnabled.MessageDescription.DataItems.Add(credentialToken);
            //deniedCredentialNotEnabled.MessageDescription.DataItems.Add(credentialsHolder);
            //deniedCredentialNotEnabled.MessageDescription.DataItems.Add(reason);

            //// AccessControl/Denied/Credential/CredentialNotActive
            //Topic deniedCredentialNotActive = new Topic(PacsTopic.CREDENTIALNOTACTIVE, null, null, true);
            //deniedCredentialNotActive.MessageDescription.IsProperty = false;
            //deniedCredentialNotActive.MessageDescription.SourceItems.Add(accessPointToken);
            //deniedCredentialNotActive.MessageDescription.DataItems.Add(credentialToken);
            //deniedCredentialNotActive.MessageDescription.DataItems.Add(credentialsHolder);

            //// AccessControl/Denied/Credential/CredentialExpired
            //Topic deniedCredentialExpired = new Topic(PacsTopic.CREDENTIALEXPIRED, null, null, true);
            //deniedCredentialExpired.MessageDescription.IsProperty = false;
            //deniedCredentialExpired.MessageDescription.SourceItems.Add(accessPointToken);
            //deniedCredentialExpired.MessageDescription.DataItems.Add(credentialToken);
            //deniedCredentialExpired.MessageDescription.DataItems.Add(credentialsHolder);

            //// AccessControl/Denied/Credential/InvalidPin
            //Topic deniedCredentialInvalidPin = new Topic(PacsTopic.INVALIDPIN, null, null, true);
            //deniedCredentialInvalidPin.MessageDescription.IsProperty = false;
            //deniedCredentialInvalidPin.MessageDescription.SourceItems.Add(accessPointToken);
            //deniedCredentialInvalidPin.MessageDescription.DataItems.Add(credentialToken);
            //deniedCredentialInvalidPin.MessageDescription.DataItems.Add(credentialsHolder);

            //// AccessControl/Denied/Credential/NotPermittedAtThisTime
            //Topic deniedCredentialNotPermittedAtThisTime = new Topic(PacsTopic.NOTPERMITTEDATTHISTIME, null, null, true);
            //deniedCredentialNotPermittedAtThisTime.MessageDescription.IsProperty = false;
            //deniedCredentialNotPermittedAtThisTime.MessageDescription.SourceItems.Add(accessPointToken);
            //deniedCredentialNotPermittedAtThisTime.MessageDescription.DataItems.Add(credentialToken);
            //deniedCredentialNotPermittedAtThisTime.MessageDescription.DataItems.Add(credentialsHolder);
            //deniedCredentialNotPermittedAtThisTime.MessageDescription.DataItems.Add(reason);

            //// AccessControl/Denied/Credential/Unathorized
            //Topic deniedCredentialUnathorized = new Topic(PacsTopic.UNAUTHORIZED, null, null, true);
            //deniedCredentialUnathorized.MessageDescription.IsProperty = false;
            //deniedCredentialUnathorized.MessageDescription.SourceItems.Add(accessPointToken);
            //deniedCredentialUnathorized.MessageDescription.DataItems.Add(credentialToken);
            //deniedCredentialUnathorized.MessageDescription.DataItems.Add(credentialsHolder);

            //// AccessControl/Denied/Credential/External
            //Topic deniedCredentialExternal = new Topic(PacsTopic.EXTERNAL, null, null, true);
            //_instance._deniedCredentialExternal = deniedCredentialExternal;
            //deniedCredentialExternal.MessageDescription.IsProperty = false;
            //deniedCredentialExternal.MessageDescription.SourceItems.Add(accessPointToken);
            //deniedCredentialExternal.MessageDescription.DataItems.Add(credentialToken);
            //deniedCredentialExternal.MessageDescription.DataItems.Add(credentialsHolder);
            //deniedCredentialExternal.MessageDescription.DataItems.Add(reason);

            //// AccessControl/Denied/Credential/Other
            //Topic deniedCredentialOther = new Topic(PacsTopic.OTHER, null, null, true);
            //deniedCredentialOther.MessageDescription.IsProperty = false;
            //deniedCredentialOther.MessageDescription.SourceItems.Add(accessPointToken);
            //deniedCredentialOther.MessageDescription.DataItems.Add(credentialToken);
            //deniedCredentialOther.MessageDescription.DataItems.Add(credentialsHolder);
            //deniedCredentialOther.MessageDescription.DataItems.Add(reason);
            //deniedCredentialOther.MessageDescription.DataItems.Add(card);
            ////deniedCredentialOther.MessageDescription.DataItems.Add(new SimpleItemDescription("", "...", XSNAMESPACE));

            // AccessControl/Denied/CredentialNotFound
            Topic deniedCredentialNotFound = new Topic(PacsTopic.CREDENTIALNOTFOUND, null, null);

            // AccessControl/Denied/Credential/CredentialNotFound/Card
            Topic deniedCredentialNotFoundCard = new Topic(PacsTopic.CARD, null, null, true);
            deniedCredentialNotFoundCard.MessageDescription.IsProperty = false;
            deniedCredentialNotFoundCard.MessageDescription.SourceItems.Add(accessPointToken);
            deniedCredentialNotFoundCard.MessageDescription.DataItems.Add(card);

            // AccessControl/Denied/Anonymous
            Topic deniedAnonymous = new Topic(PacsTopic.ANONYMOUS, null, null, true);
            _instance._deniedAnonymous = deniedAnonymous;

            deniedAnonymous.MessageDescription.IsProperty = false;
            deniedAnonymous.MessageDescription.SourceItems.Add(accessPointToken);
            deniedAnonymous.MessageDescription.DataItems.Add(reason);
            deniedAnonymous.MessageDescription.DataItems.Add(external);

            //// AccessControl/Denied/Anonymous/NotPermittedAtThisTime
            //Topic deniedAnonymousNotPermitted = new Topic(PacsTopic.NOTPERMITTEDATTHISTIME, null, null, true);
            //deniedAnonymousNotPermitted.MessageDescription.IsProperty = false;
            //deniedAnonymousNotPermitted.MessageDescription.SourceItems.Add(accessPointToken);
            //deniedAnonymousNotPermitted.MessageDescription.DataItems.Add(reason);

            //// AccessControl/Denied/Anonymous/Unauthorised
            //Topic deniedAnonymousUnauthorised = new Topic(PacsTopic.UNAUTHORIZED, null, null, true);
            //deniedAnonymousUnauthorised.MessageDescription.IsProperty = false;
            //deniedAnonymousUnauthorised.MessageDescription.SourceItems.Add(accessPointToken);
            //deniedAnonymousUnauthorised.MessageDescription.DataItems.Add(reason);

            //// AccessControl/Denied/Anonymous/External
            //Topic deniedAnonymousExternal = new Topic(PacsTopic.EXTERNAL, null, null, true);
            //_instance._deniedAnonymousExternal = deniedAnonymousExternal;
            //deniedAnonymousExternal.MessageDescription.IsProperty = false;
            //deniedAnonymousExternal.MessageDescription.SourceItems.Add(accessPointToken);
            //deniedAnonymousExternal.MessageDescription.DataItems.Add(reason);

            //// AccessControl/Denied/Anonymous/Other
            //Topic deniedAnonymousOther = new Topic(PacsTopic.OTHER, null, null, true);
            //deniedAnonymousOther.MessageDescription.IsProperty = false;
            //deniedAnonymousOther.MessageDescription.SourceItems.Add(accessPointToken);
            //deniedAnonymousOther.MessageDescription.DataItems.Add(reason);


            // AccessControl/Duress
            Topic duress = new Topic(PacsTopic.DURESS, null, null, true);
            duress.MessageDescription.IsProperty = false;
            duress.MessageDescription.SourceItems.Add(accessPointToken);
            duress.MessageDescription.DataItems.Add(credentialToken);
            duress.MessageDescription.DataItems.Add(credentialsHolder);
            duress.MessageDescription.DataItems.Add(reason);

            //// AccessControl/Duress/Anonymous
            //Topic duressAnonymous = new Topic(PacsTopic.ANONYMOUS, null, null, true);
            //duressAnonymous.MessageDescription.IsProperty = false;
            //duressAnonymous.MessageDescription.SourceItems.Add(accessPointToken);
            //duressAnonymous.MessageDescription.DataItems.Add(reason);

            //// AccessControl/Duress/Credential
            //Topic duressCredential = new Topic(PacsTopic.CREDENTIAL, null, null, true);
            //duressCredential.MessageDescription.IsProperty = false;
            //duressCredential.MessageDescription.SourceItems.Add(accessPointToken);
            //duressCredential.MessageDescription.DataItems.Add(credentialToken);
            //duressCredential.MessageDescription.DataItems.Add(credentialsHolder);
            //duressCredential.MessageDescription.DataItems.Add(reason);

            // AccessControl/Request
            Topic request = new Topic(PacsTopic.REQUEST, null, null);

            // AccessControl/Request/Anonymous
            Topic requestAnonymous = new Topic(PacsTopic.ANONYMOUS, null, null, true);
            requestAnonymous.MessageDescription.IsProperty = false;
            requestAnonymous.MessageDescription.SourceItems.Add(accessPointToken);

            // AccessControl/Request/Credential
            Topic requestCredential = new Topic(PacsTopic.CREDENTIAL, null, null, true);
            requestCredential.MessageDescription.IsProperty = false;
            requestCredential.MessageDescription.SourceItems.Add(accessPointToken);
            requestCredential.MessageDescription.DataItems.Add(credentialToken);
            requestCredential.MessageDescription.DataItems.Add(credentialsHolder);

            // AccessControl/Request/Timeout
            Topic requestTimeout = new Topic(PacsTopic.TIMEOUT, null, null, true);
            //_instance._requestTimeoutAnonymous = requestTimeoutAnonymous;
            requestTimeout.MessageDescription.IsProperty = false;
            requestTimeout.MessageDescription.SourceItems.Add(accessPointToken);

            //// AccessControl/Request/Timeout/Anonymous
            //Topic requestTimeoutAnonymous = new Topic(PacsTopic.ANONYMOUS, null, null, true);
            //_instance._requestTimeoutAnonymous = requestTimeoutAnonymous;
            //requestTimeoutAnonymous.MessageDescription.IsProperty = false;
            //requestTimeoutAnonymous.MessageDescription.SourceItems.Add(accessPointToken);

            //// AccessControl/Request/Timeout/Credential
            //Topic requestTimeoutCredential = new Topic(PacsTopic.CREDENTIAL, null, null, true);
            //_instance._requestTimeoutCredential = requestTimeoutCredential;
            //requestTimeoutCredential.MessageDescription.IsProperty = false;
            //requestTimeoutCredential.MessageDescription.SourceItems.Add(accessPointToken);
            //requestTimeoutCredential.MessageDescription.DataItems.Add(credentialToken);


            // AccessPoint
            Topic accessPoint = new Topic(PacsTopic.ACCESSPOINT, ONVIFEVENTS, TNS1, false);
            _instance._accessControlAccessPoint = accessPoint;

            Topic configurationAccessPoint = new Topic(PacsTopic.ACCESSPOINT, null, null, false);

            // AccessPoint/Changed
            Topic configurationAccessPointChanged = new Topic(PacsTopic.CHANGED, null, null, true);
            configurationAccessPointChanged.MessageDescription.IsProperty = false;
            configurationAccessPointChanged.MessageDescription.SourceItems.Add(accessPointToken);

            //// AccessPoint/Set
            //Topic configurationAccessPointSet = new Topic(PacsTopic.CHANGED, null, null, true);
            //configurationAccessPointSet.MessageDescription.IsProperty = false;
            //configurationAccessPointSet.MessageDescription.SourceItems.Add(accessPointToken);
            //configurationAccessPointSet.MessageDescription.DataItems.Add(reason);

            //// AccessPoint/Removed
            Topic configurationAccessPointRemoved = new Topic(PacsTopic.REMOVED, null, null, true);
            configurationAccessPointRemoved.MessageDescription.IsProperty = false;
            configurationAccessPointRemoved.MessageDescription.SourceItems.Add(accessPointToken);

            Topic configuration = new Topic(PacsTopic.CONFIGURATION, ONVIFEVENTS, TNS1, false);

            // Area
            Topic area = new Topic(PacsTopic.AREA, null, null, false);

            // Area/Changed
            Topic configurationAreaChanged = new Topic(PacsTopic.CHANGED, null, null, true);
            configurationAreaChanged.MessageDescription.IsProperty = false;
            configurationAreaChanged.MessageDescription.SourceItems.Add(areaToken);

            // Area/Removed
            Topic configurationAreaRemoved = new Topic(PacsTopic.REMOVED, null, null, true);
            configurationAreaRemoved.MessageDescription.IsProperty = false;
            configurationAreaRemoved.MessageDescription.SourceItems.Add(areaToken);

            //// Area/Set
            //Topic configurationAreaSet = new Topic(PacsTopic.CHANGED, null, null, true);
            //configurationAreaSet.MessageDescription.IsProperty = false;
            //configurationAreaSet.MessageDescription.SourceItems.Add(areaToken);
            //configurationAreaSet.MessageDescription.DataItems.Add(reason);

            //// Area/Removed
            //Topic configurationAreaRemoved = new Topic(PacsTopic.REMOVED, null, null, true);
            //configurationAreaRemoved.MessageDescription.IsProperty = false;
            //configurationAreaRemoved.MessageDescription.SourceItems.Add(areaToken);
            //configurationAreaRemoved.MessageDescription.DataItems.Add(reason);


            // Property events

            // AccessPoint/State
            Topic accessPointState = new Topic("State", null, null, false);

            //AccessControl/AccessPoint/Enabled [topic]
            Topic accessControlAccessPointEnabled = new Topic(PacsTopic.ENABLED, null, null, true);
            _instance._accessControlAccessPointEnabled = accessControlAccessPointEnabled;
            accessControlAccessPointEnabled.MessageDescription.IsProperty = true;
            accessControlAccessPointEnabled.MessageDescription.SourceItems.Add(accessPointToken);
            accessControlAccessPointEnabled.MessageDescription.DataItems.Add(new SimpleItemDescription("State", "boolean", XSNAMESPACE));
            //accessControlAccessPointEnabled.MessageDescription.DataItems.Add(reason);

            //AccessControl/AccessPoint/Tampering [topic]
            //Topic accessControlAccessPointTampering = new Topic(PacsTopic.TAMPERING, null, null, true);
            //_instance._accessControlAccessPointTampering = accessControlAccessPointTampering;
            //accessControlAccessPointTampering.MessageDescription.IsProperty = true;
            //accessControlAccessPointTampering.MessageDescription.SourceItems.Add(accessPointToken);
            //accessControlAccessPointTampering.MessageDescription.DataItems.Add(new SimpleItemDescription("State", "boolean", XSNAMESPACE));
            //accessControlAccessPointTampering.MessageDescription.DataItems.Add(reason);

            #endregion

            /////////////////////////////////////////////////////////////////////////////////
            //                               Credentials                                   //
            /////////////////////////////////////////////////////////////////////////////////

            #region Credentials

            // SimpleItemDescriptions
            var state = new SimpleItemDescription("State", "boolean", XSNAMESPACE);
            var apbViolation = new SimpleItemDescription("ApbViolation", "boolean", XSNAMESPACE);

            //Credential
            Topic credential = new Topic(PacsTopic.CREDENTIAL, ONVIFEVENTS, TNS1, false);
            _instance._credential = credential;

            //Credential/State
            Topic credentialState = new Topic(PacsTopic.STATE, null, null, false);
            _instance._credentialState = credentialState;

            

            //Credential/State/Enabled [topic]
            Topic credentialStateEnabled = new Topic(PacsTopic.ENABLED, null, null, true);
            _instance._credentialStateEnabled = credentialStateEnabled;
            credentialStateEnabled.MessageDescription.IsProperty = false;
            credentialStateEnabled.MessageDescription.SourceItems.Add(credentialToken);
            credentialStateEnabled.MessageDescription.DataItems.Add(state);
            credentialStateEnabled.MessageDescription.DataItems.Add(reason);
            credentialStateEnabled.MessageDescription.DataItems.Add(clientUpdated);

            //Credential/State/ApbViolation [topic]
            Topic credentialStateApbViolation = new Topic(PacsTopic.APBVIOLATION, null, null, true);
            _instance._credentialStateApbViolation = credentialStateApbViolation;
            credentialStateApbViolation.MessageDescription.IsProperty = false;
            credentialStateApbViolation.MessageDescription.SourceItems.Add(credentialToken);
            credentialStateApbViolation.MessageDescription.DataItems.Add(apbViolation);
            credentialStateApbViolation.MessageDescription.DataItems.Add(clientUpdated);

            //Configuration/Credential
            Topic configurationCredential = new Topic(PacsTopic.CREDENTIAL, null, null, false);
            _instance._configurationCredential = configurationCredential;

            //Configuration/Credential/Changed [topic]
            Topic configurationCredentialChanged = new Topic(PacsTopic.CHANGED, null, null, true);
            _instance._configurationCredentialChanged = configurationCredentialChanged;
            configurationCredentialChanged.MessageDescription.IsProperty = false;
            configurationCredentialChanged.MessageDescription.SourceItems.Add(credentialToken);

            //Configuration/Credential/Removed [topic]
            Topic configurationCredentialRemoved = new Topic(PacsTopic.REMOVED, null, null, true);
            _instance._configurationCredentialRemoved = configurationCredentialRemoved;
            configurationCredentialRemoved.MessageDescription.IsProperty = false;
            configurationCredentialRemoved.MessageDescription.SourceItems.Add(credentialToken);

            #endregion

            /////////////////////////////////////////////////////////////////////////////////
            //                               Access Rules                                  //
            /////////////////////////////////////////////////////////////////////////////////


            #region AccessRules

            var accessProfileToken = new SimpleItemDescription("AccessProfileToken", "ReferenceToken", PTNAMESPACE);

            //Configuration/AccessProfile
            Topic configurationAccessProfile = new Topic(PacsTopic.ACCESSPROFILE, null, null, false);
            _instance._configurationAccessProfile = configurationAccessProfile;

            //Configuration/AccessProfile/Changed [topic]
            Topic configurationAccessProfileChanged = new Topic(PacsTopic.CHANGED, null, null, true);
            _instance._configurationAccessProfileChanged = configurationAccessProfileChanged;
            configurationAccessProfileChanged.MessageDescription.IsProperty = false;
            configurationAccessProfileChanged.MessageDescription.SourceItems.Add(accessProfileToken);

            //Configuration/AccessProfile/Removed [topic]
            Topic configurationAccessProfileRemoved = new Topic(PacsTopic.REMOVED, null, null, true);
            _instance._configurationAccessProfileRemoved = configurationAccessProfileRemoved;
            configurationAccessProfileRemoved.MessageDescription.IsProperty = false;
            configurationAccessProfileRemoved.MessageDescription.SourceItems.Add(accessProfileToken);

            #endregion

            /////////////////////////////////////////////////////////////////////////////////
            //                                 Schedule                                    //
            /////////////////////////////////////////////////////////////////////////////////


            #region Schedule

            var scheduleToken = new SimpleItemDescription("ScheduleToken", "ReferenceToken", PTNAMESPACE);
            var specialDaysToken = new SimpleItemDescription("SpecialDaysToken", "ReferenceToken", PTNAMESPACE);
            var name = new SimpleItemDescription("Name", "string", XSNAMESPACE);
            var active = new SimpleItemDescription("Active", "boolean", XSNAMESPACE);
            var specialDay = new SimpleItemDescription("SpecialDay", "boolean", XSNAMESPACE);

            //Schedule
            Topic schedule = new Topic(PacsTopic.SCHEDULE, ONVIFEVENTS, TNS1, false);
            _instance._schedule = schedule;

            //Schedule/State
            Topic scheduleState = new Topic(PacsTopic.STATE, null, null, false);

            //Schedule/State/Active [topic]
            Topic scheduleStateActive = new Topic(PacsTopic.ACTIVE, null, null, true);
            _instance._scheduleStateActive = scheduleStateActive;
            scheduleStateActive.MessageDescription.IsProperty = true;
            scheduleStateActive.MessageDescription.SourceItems.Add(scheduleToken);
            scheduleStateActive.MessageDescription.SourceItems.Add(name);
            scheduleStateActive.MessageDescription.DataItems.Add(active);
            scheduleStateActive.MessageDescription.DataItems.Add(specialDay);

            //Configuration/Schedule
            Topic configurationSchedule = new Topic(PacsTopic.SCHEDULE, null, null, false);
            _instance._configurationSchedule = configurationSchedule;

            //Configuration/Schedule/Changed [topic]
            Topic configurationScheduleChanged = new Topic(PacsTopic.CHANGED, null, null, true);
            _instance._configurationScheduleChanged = configurationScheduleChanged;
            configurationScheduleChanged.MessageDescription.IsProperty = false;
            configurationScheduleChanged.MessageDescription.SourceItems.Add(scheduleToken);

            //Configuration/Schedule/Removed [topic]
            Topic configurationScheduleRemoved = new Topic(PacsTopic.REMOVED, null, null, true);
            _instance._configurationScheduleRemoved = configurationScheduleRemoved;
            configurationScheduleRemoved.MessageDescription.IsProperty = false;
            configurationScheduleRemoved.MessageDescription.SourceItems.Add(scheduleToken);

            //Configuration/SpecialDays
            Topic configurationSpecialDays = new Topic(PacsTopic.SPECIALDAYS, null, null, false);
            _instance._configurationSpecialDays = configurationSpecialDays;

            //Configuration/SpecialDays/Changed [topic]
            Topic configurationSpecialDaysChanged = new Topic(PacsTopic.CHANGED, null, null, true);
            _instance._configurationSpecialDaysChanged = configurationSpecialDaysChanged;
            configurationSpecialDaysChanged.MessageDescription.IsProperty = false;
            configurationSpecialDaysChanged.MessageDescription.SourceItems.Add(specialDaysToken);

            //Configuration/SpecialDays/Removed [topic]
            Topic configurationSpecialDaysRemoved = new Topic(PacsTopic.REMOVED, null, null, true);
            _instance._configurationSpecialDaysRemoved = configurationSpecialDaysRemoved;
            configurationSpecialDaysRemoved.MessageDescription.IsProperty = false;
            configurationSpecialDaysRemoved.MessageDescription.SourceItems.Add(specialDaysToken);

            #endregion




            // Create Tree


            _instance.AddTopic(door);
            door.Add(doorState);
            doorState.Add(doorMode,
                          doorPhysicalState,
                          doubleLockPhysicalState,
                          lockPhysicalState,
                          doorTamper,
                          doorAlarm, 
                          doorFault);
            
            _instance.AddTopic(accessControl);
            accessControl.Add(accessControlAccessGranted, accessTaken, accessNotTaken, denied, duress, request);

            accessControlAccessGranted.Add(accessGrantedAnonymous, accessGrantedCredential);
            accessTaken.Add(accessTakenAnonymous, accessTakenCredential);
            accessNotTaken.Add(accessNotTakenAnonymous, accessNotTakenCredential);
            denied.Add(deniedCredential, deniedAnonymous, deniedCredentialNotFound);
            deniedCredentialNotFound.Add(deniedCredentialNotFoundCard);
            request.Add(requestAnonymous, requestCredential, requestTimeout);

            configuration.Add(configurationAccessPoint);
            configuration.Add(configurationDoor);
            configuration.Add(area);
            configuration.Add(configurationCredential);
            configuration.Add(configurationAccessProfile);
            configuration.Add(configurationSchedule);
            configuration.Add(configurationSpecialDays);

            configurationDoor.Add(doorChanged, doorRemoved);
            area.Add(configurationAreaChanged, configurationAreaRemoved);
            accessPoint.Add(accessPointState);
            configurationAccessPoint.Add(configurationAccessPointChanged, configurationAccessPointRemoved);
            accessPointState.Add(accessControlAccessPointEnabled);
            configurationSchedule.Add(configurationScheduleChanged, configurationScheduleRemoved);
            configurationSpecialDays.Add(configurationSpecialDaysChanged, configurationSpecialDaysRemoved);

            configurationCredential.Add(configurationCredentialChanged);
            configurationCredential.Add(configurationCredentialRemoved);
            configurationAccessProfile.Add(configurationAccessProfileChanged);
            configurationAccessProfile.Add(configurationAccessProfileRemoved);

            _instance.AddTopic(accessPoint);
            _instance.AddTopic(configuration);

            _instance.AddTopic(credential);

            credential.Add(credentialState);
            credentialState.Add(credentialStateApbViolation);
            credentialState.Add(credentialStateEnabled);

            _instance.AddTopic(schedule);
            schedule.Add(scheduleState);
            scheduleState.Add(scheduleStateActive);

        }

        #endregion
    }
}
