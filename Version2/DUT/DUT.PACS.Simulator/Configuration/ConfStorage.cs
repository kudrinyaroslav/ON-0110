using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DUT.PACS.Simulator.Configuration;
using DUT.PACS.Simulator.Proxies;
using DUT.PACS.Simulator.ServiceDoorControl10;
using DUT.PACS.Simulator.Device10;


namespace DUT.PACS.Simulator
{
    public class ConfStorage
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfStorage()
        {

        }

        #endregion //Constructors

        #region Members

        //Access Control Service
        private Dictionary<string, ServiceAccessControl10.AreaInfo> m_AreaInfoList;
        private Dictionary<string, ServiceAccessControl10.AccessPointInfo> m_AccessPointInfoList;
        private Dictionary<string, ServiceAccessControl10.AccessPointState> m_AccessPointState;
        private Dictionary<string, bool> m_AccessPointTamperingState;

        //Door Control Service
        private Dictionary<string, ServiceDoorControl10.DoorInfo> m_DoorInfoList;
        private Dictionary<string, ServiceDoorControl10.DoorState> m_DoorStateList;
        private Dictionary<string, int> m_DoorAccessList;
        private Dictionary<string, ServiceDoorControl10.DoorMode> m_DoorAccessPreviousStateList;

        //Credential Service
        private Dictionary<string, ServiceCredential10.Credential> m_CredentialList;
        private Dictionary<string, ServiceCredential10.CredentialState> m_CredentialStateList;

        //Access Rules Service
        private Dictionary<string, ServiceAccessRules10.AccessProfile> m_AccessProfileList;

        //Credentials Temp
        //private Dictionary<string, CredentialInformation> m_CredentialInformationList;

        //Other
        private TriggerConfiguration m_triggerConfiguration;

        private bool m_ConfStorageInitialized = false;

        private DiscoveryMode m_DiscoveryMode = DiscoveryMode.Discoverable;



        #endregion //Members

        #region Properties

        public DiscoveryMode DiscoveryMode
        {
            get { return m_DiscoveryMode; }
            set { m_DiscoveryMode = value; }
        }

        public Dictionary<string, ServiceAccessRules10.AccessProfile> AccessProfileList
        {
            get
            {
                if (!m_ConfStorageInitialized)
                {
                    ConfStorageInitialization();
                }
                return m_AccessProfileList;
            }
        }

        public Dictionary<string, ServiceCredential10.CredentialState> CredentialStateList
        {
            get
            {
                if (!m_ConfStorageInitialized)
                {
                    ConfStorageInitialization();
                }
                return m_CredentialStateList;
            }
        }

        public Dictionary<string, ServiceAccessControl10.AccessPointState> AccessPointState
        {
            get
            {
                if (!m_ConfStorageInitialized)
                {
                    ConfStorageInitialization();
                }
                return m_AccessPointState;
            }
        }

        public Dictionary<string, bool> AccessPointTamperingState
        {
            get
            {
                if (!m_ConfStorageInitialized)
                {
                    ConfStorageInitialization();
                }
                return m_AccessPointTamperingState;
            }
        }

        public Dictionary<string, ServiceCredential10.Credential> CredentialList
        {
            get
            {
                if (!m_ConfStorageInitialized)
                {
                    ConfStorageInitialization();
                }
                return m_CredentialList;
            }

        }


        public Dictionary<string, ServiceAccessControl10.AreaInfo> AreaInfoList
        {
            get
            {
                if (!m_ConfStorageInitialized)
                {
                    ConfStorageInitialization();
                }
                return m_AreaInfoList;
            }
        }

        public Dictionary<string, ServiceAccessControl10.AccessPointInfo> AccessPointInfoList
        {
            get
            {
                if (!m_ConfStorageInitialized)
                {
                    ConfStorageInitialization();
                }
                return m_AccessPointInfoList;
            }
        }

        public Dictionary<string, ServiceDoorControl10.DoorInfo> DoorInfoList
        {
            get
            {
                if (!m_ConfStorageInitialized)
                {
                    ConfStorageInitialization();
                }
                return m_DoorInfoList;
            }
        }

        public Dictionary<string, ServiceDoorControl10.DoorState> DoorStateList
        {
            get
            {
                if (!m_ConfStorageInitialized)
                {
                    ConfStorageInitialization();
                }
                return m_DoorStateList;
            }
        }

        public Dictionary<string, int> DoorAccessList
        {
            get
            {
                if (!m_ConfStorageInitialized)
                {
                    ConfStorageInitialization();
                }
                return m_DoorAccessList;
            }
        }

        public Dictionary<string, ServiceDoorControl10.DoorMode> DoorAccessPreviousStateList
        {
            get
            {
                if (!m_ConfStorageInitialized)
                {
                    ConfStorageInitialization();
                }
                return m_DoorAccessPreviousStateList;
            }
        }

        public TriggerConfiguration TriggerConfiguration
        {
            get
            {
                if (!m_ConfStorageInitialized)
                {
                    ConfStorageInitialization();
                }
                return m_triggerConfiguration;
            }
        }

        //public Dictionary<string, CredentialInformation> CredentialInformationList
        //{
        //    get
        //    {
        //        if (!m_ConfStorageInitialized)
        //        {
        //            ConfStorageInitialization();
        //        }
        //        return m_CredentialInformationList;
        //    }
        //}


        #endregion //Properties

        #region DefaultInitialization

        private void ConfStorageInitialization()
        {
            m_AreaInfoList = AreaInfoDefaultInitialization();
            m_AccessPointInfoList = AccessPointInfoDefaultInitialization();
            m_AccessPointState = AccessPointStateDefaultInitialization();
            m_AccessPointTamperingState = AccessPointTamperingStateDefaultInitialization();

            m_DoorInfoList = DoorInfoDefaultInitialization();

            m_DoorStateList = DoorStateDefaultInitialization();
            m_DoorAccessList = DoorAccessListDefaultInitialization();
            m_DoorAccessPreviousStateList = DoorAccessPreviousStateListInitialization();
            //m_CredentialInformationList = CredentialInfoListInitialization();
            m_triggerConfiguration = TriggerDefaultInitialization();

            m_CredentialList = CredentialListInitialization();
            m_CredentialStateList = CredentialStateListInitialization();

            m_AccessProfileList = AccessProfileListInitialization();

            m_ConfStorageInitialized = true;
        }

        private Dictionary<string, ServiceAccessRules10.AccessProfile> AccessProfileListInitialization()
        {
            Dictionary<string, ServiceAccessRules10.AccessProfile> res = new Dictionary<string, ServiceAccessRules10.AccessProfile>();

            ServiceAccessRules10.AccessProfile item;

            item = new ServiceAccessRules10.AccessProfile();

            item.token = "accessprofile1";
            item.Name = "Access Profile 1";
            item.Description = "Access Profile Description 1";
            item.AccessPolicy = new ServiceAccessRules10.AccessPolicy[2];
            item.AccessPolicy[0] = new ServiceAccessRules10.AccessPolicy();
            item.AccessPolicy[0].ScheduleToken = "schedule1";
            item.AccessPolicy[0].Entity = "tokenAccessPoint1";
            item.AccessPolicy[0].EntityType = new System.Xml.XmlQualifiedName("AccessPoint", "http://www.onvif.org/ver10/accesscontrol/wsdl");
            item.AccessPolicy[1] = new ServiceAccessRules10.AccessPolicy();
            item.AccessPolicy[1].ScheduleToken = "schedule2";
            item.AccessPolicy[1].Entity = "tokenAccessPoint2";
            item.AccessPolicy[1].EntityType = new System.Xml.XmlQualifiedName("AccessPoint", "http://www.onvif.org/ver10/accesscontrol/wsdl");

            res.Add(item.token, item);

            item = new ServiceAccessRules10.AccessProfile();

            item.token = "accessprofile2";
            item.Name = "Access Profile 2";
            item.Description = "Access Profile Description 2";
            item.AccessPolicy = new ServiceAccessRules10.AccessPolicy[1];
            item.AccessPolicy[0] = new ServiceAccessRules10.AccessPolicy();
            item.AccessPolicy[0].ScheduleToken = "schedule2";
            item.AccessPolicy[0].Entity = "tokenAccessPoint2";
            item.AccessPolicy[0].EntityType = null;


            res.Add(item.token, item);

            return res;
        }

        private Dictionary<string, ServiceCredential10.CredentialState> CredentialStateListInitialization()
        {
            Dictionary<string, ServiceCredential10.CredentialState> res = new Dictionary<string, ServiceCredential10.CredentialState>();

            ServiceCredential10.CredentialState item;

            item = new ServiceCredential10.CredentialState();

            item.AntipassbackState = new ServiceCredential10.AntipassbackState();
            item.AntipassbackState.AntipassbackViolated = true;
            item.Enabled = true;
            item.Reason = "Entrooder";
            
            res.Add("credential1", item);

            item = new ServiceCredential10.CredentialState();

            item.AntipassbackState = new ServiceCredential10.AntipassbackState();
            item.AntipassbackState.AntipassbackViolated = false;
            item.Enabled = false;
            item.Reason = "Normal";

            res.Add("credential2", item);

            item = new ServiceCredential10.CredentialState();

            item.AntipassbackState = new ServiceCredential10.AntipassbackState();
            item.AntipassbackState.AntipassbackViolated = false;
            item.Enabled = false;
            item.Reason = "Normal";

            res.Add("credential3", item);

            item = new ServiceCredential10.CredentialState();

            item.AntipassbackState = new ServiceCredential10.AntipassbackState();
            item.AntipassbackState.AntipassbackViolated = false;
            item.Enabled = false;
            item.Reason = "Normal";

            res.Add("credential4", item);

            return res;
        }

        private Dictionary<string, ServiceCredential10.Credential> CredentialListInitialization()
        {
            Dictionary<string, ServiceCredential10.Credential> res = new Dictionary<string, ServiceCredential10.Credential>();

            ServiceCredential10.Credential item;

            item = new ServiceCredential10.Credential();

            item.token = "credential1";

            item.ValidFromSpecified = true;
            item.ValidFrom = new System.DateTime(2014, 01, 10);

            item.ValidToSpecified = true;
            item.ValidTo = new System.DateTime(2014, 10, 10);

            item.Description = "Credential 1 Description";

            item.CredentialHolderReference = "CredentialHolder1";

            item.Attribute = new ServiceCredential10.Attribute[2];

            item.Attribute[0] = new ServiceCredential10.Attribute();
            item.Attribute[0].Name = "CustomAttribute1";
            item.Attribute[0].Value = "CustomAttributeValue1";
            item.Attribute[1] = new ServiceCredential10.Attribute();
            item.Attribute[1].Name = "CustomAttribute2";
            item.Attribute[1].Value = "CustomAttributeValue2";

            item.CredentialIdentifier = new ServiceCredential10.CredentialIdentifier[2];

            item.CredentialIdentifier[0] = new ServiceCredential10.CredentialIdentifier();
            item.CredentialIdentifier[0].ExemptedFromAuthentication = true;
            item.CredentialIdentifier[0].Type = new ServiceCredential10.CredentialIdentifierType();
            item.CredentialIdentifier[0].Type.Name = "ONVIFCard";
            item.CredentialIdentifier[0].Type.FormatType = "WIEGAND26";
            item.CredentialIdentifier[0].Value = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            item.CredentialIdentifier[1] = new ServiceCredential10.CredentialIdentifier();
            item.CredentialIdentifier[1].ExemptedFromAuthentication = true;
            item.CredentialIdentifier[1].Type = new ServiceCredential10.CredentialIdentifierType();
            item.CredentialIdentifier[1].Type.Name = "ONVIFPIN";
            item.CredentialIdentifier[1].Type.FormatType = "WIEGAND37";
            item.CredentialIdentifier[1].Value = new byte[] { 0x21, 0x21, 0x21, 0x21 };

            item.CredentialAccessProfile = new ServiceCredential10.CredentialAccessProfile[2];

            item.CredentialAccessProfile[0] = new ServiceCredential10.CredentialAccessProfile();
            item.CredentialAccessProfile[0].AccessProfileToken = "accessprofile1";
            item.CredentialAccessProfile[0].ValidFromSpecified = true;
            item.CredentialAccessProfile[0].ValidFrom = new System.DateTime(2014, 02, 10);
            item.CredentialAccessProfile[0].ValidToSpecified = true;
            item.CredentialAccessProfile[0].ValidTo = new System.DateTime(2015, 05, 14);

            item.CredentialAccessProfile[1] = new ServiceCredential10.CredentialAccessProfile();
            item.CredentialAccessProfile[1].AccessProfileToken = "accessprofile2";
            item.CredentialAccessProfile[1].ValidFromSpecified = false;          
            item.CredentialAccessProfile[1].ValidToSpecified = false;        
                        
            res.Add(item.token, item);

            item = new ServiceCredential10.Credential();

            item.token = "credential2";

            item.ValidFromSpecified = true;
            item.ValidFrom = new System.DateTime(2014, 02, 10);

            item.ValidToSpecified = true;
            item.ValidTo = new System.DateTime(2014, 12, 10);

            item.Description = "Credential 2 Description";

            item.CredentialHolderReference = "CredentialHolder2";

            item.CredentialIdentifier = new ServiceCredential10.CredentialIdentifier[2];

            item.CredentialIdentifier[0] = new ServiceCredential10.CredentialIdentifier();
            item.CredentialIdentifier[0].ExemptedFromAuthentication = true;
            item.CredentialIdentifier[0].Type = new ServiceCredential10.CredentialIdentifierType();
            item.CredentialIdentifier[0].Type.Name = "ONVIFPIN";
            item.CredentialIdentifier[0].Type.FormatType = "WIEGAND37";
            item.CredentialIdentifier[0].Value = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            item.CredentialIdentifier[1] = new ServiceCredential10.CredentialIdentifier();
            item.CredentialIdentifier[1].ExemptedFromAuthentication = true;
            item.CredentialIdentifier[1].Type = new ServiceCredential10.CredentialIdentifierType();
            item.CredentialIdentifier[1].Type.Name = "ONVIFCard";
            item.CredentialIdentifier[1].Type.FormatType = "WIEGAND26";
            item.CredentialIdentifier[1].Value = new byte[] { 0x21, 0x21, 0x21, 0x21 };

            res.Add(item.token, item);

            item = new ServiceCredential10.Credential();

            item.token = "credential3";

            item.ValidFromSpecified = true;
            item.ValidFrom = new System.DateTime(2014, 02, 10);

            item.ValidToSpecified = true;
            item.ValidTo = new System.DateTime(2014, 12, 10);

            item.Description = "Credential 3 Description";

            item.CredentialHolderReference = "CredentialHolder3";

            item.CredentialIdentifier = new ServiceCredential10.CredentialIdentifier[2];

            item.CredentialIdentifier[0] = new ServiceCredential10.CredentialIdentifier();
            item.CredentialIdentifier[0].ExemptedFromAuthentication = true;
            item.CredentialIdentifier[0].Type = new ServiceCredential10.CredentialIdentifierType();
            item.CredentialIdentifier[0].Type.Name = "ONVIFCard";
            item.CredentialIdentifier[0].Type.FormatType = "WIEGAND26";
            item.CredentialIdentifier[0].Value = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            item.CredentialIdentifier[1] = new ServiceCredential10.CredentialIdentifier();
            item.CredentialIdentifier[1].ExemptedFromAuthentication = true;
            item.CredentialIdentifier[1].Type = new ServiceCredential10.CredentialIdentifierType();
            item.CredentialIdentifier[1].Type.Name = "ONVIFPIN";
            item.CredentialIdentifier[1].Type.FormatType = "WIEGAND37";
            item.CredentialIdentifier[1].Value = new byte[] { 0x22 };

            res.Add(item.token, item);

            item = new ServiceCredential10.Credential();

            item.token = "credential4";

            item.ValidFromSpecified = true;
            item.ValidFrom = new System.DateTime(2014, 02, 10);

            item.ValidToSpecified = true;
            item.ValidTo = new System.DateTime(2014, 12, 10);

            item.Description = "Credential 4 Description";

            item.CredentialHolderReference = "CredentialHolder4";

            item.CredentialIdentifier = new ServiceCredential10.CredentialIdentifier[2];

            item.CredentialIdentifier[0] = new ServiceCredential10.CredentialIdentifier();
            item.CredentialIdentifier[0].ExemptedFromAuthentication = true;
            item.CredentialIdentifier[0].Type = new ServiceCredential10.CredentialIdentifierType();
            item.CredentialIdentifier[0].Type.Name = "ONVIFPIN";
            item.CredentialIdentifier[0].Type.FormatType = "WIEGAND37";
            item.CredentialIdentifier[0].Value = new byte[] { 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22 };

            item.CredentialIdentifier[1] = new ServiceCredential10.CredentialIdentifier();
            item.CredentialIdentifier[1].ExemptedFromAuthentication = true;
            item.CredentialIdentifier[1].Type = new ServiceCredential10.CredentialIdentifierType();
            item.CredentialIdentifier[1].Type.Name = "ONVIFCard";
            item.CredentialIdentifier[1].Type.FormatType = "WIEGAND26";
            item.CredentialIdentifier[1].Value = new byte[] { 0x22 };

            res.Add(item.token, item);

            return res;
        }

        private Dictionary<string, ServiceAccessControl10.AreaInfo> AreaInfoDefaultInitialization()
        {
            Dictionary<string, ServiceAccessControl10.AreaInfo> res = new Dictionary<string, ServiceAccessControl10.AreaInfo>();

            ServiceAccessControl10.AreaInfo item;

            item = new ServiceAccessControl10.AreaInfo();
            item.token = "tokenArea1";
            item.Name = "Area1 Name";
            item.Description = "Area1 Description";

            res.Add(item.token, item);

            item = new ServiceAccessControl10.AreaInfo();
            item.token = "tokenArea2";
            item.Name = "Area2 Name";
            item.Description = "Area2 Description";

            res.Add(item.token, item);

            return res;
        }

        private Dictionary<string, ServiceAccessControl10.AccessPointInfo> AccessPointInfoDefaultInitialization()
        {
            Dictionary<string, ServiceAccessControl10.AccessPointInfo> res = new Dictionary<string, ServiceAccessControl10.AccessPointInfo>();

            ServiceAccessControl10.AccessPointInfo item;

            item = new ServiceAccessControl10.AccessPointInfo();
            item.token = "tokenAccessPoint1";
            item.Name = "AccessPoint1 Name";
            item.Description = "AccessPoint1 Description";
            item.AreaFrom = "tokenArea1";
            item.AreaTo = "tokenArea2";
            item.EntityType = new System.Xml.XmlQualifiedName("Door", "http://www.onvif.org/ver10/doorcontrol/wsdl");
            item.Entity = "tokenDoor1";
            item.Capabilities = new ServiceAccessControl10.AccessPointCapabilities();
            item.Capabilities.DisableAccessPoint = true;
            item.Capabilities.AccessTaken = true;
            item.Capabilities.AccessTakenSpecified = true;
            item.Capabilities.AnonymousAccess = true;
            item.Capabilities.AnonymousAccessSpecified = true;
            item.Capabilities.Duress = true;
            item.Capabilities.DuressSpecified = true;
            item.Capabilities.ExternalAuthorization = true;
            item.Capabilities.ExternalAuthorizationSpecified = true;
            item.Capabilities.Tamper = true;
            item.Capabilities.TamperSpecified = true;

            res.Add(item.token, item);

            item = new ServiceAccessControl10.AccessPointInfo();
            item.token = "tokenAccessPoint2";
            item.Name = "AccessPoint2 Name";
            item.Description = "AccessPoint2 Description";
            item.EntityType = new System.Xml.XmlQualifiedName("Door", "http://www.onvif.org/ver10/doorcontrol/wsdl");
            item.Entity = "tokenDoor1";
            item.Capabilities = new ServiceAccessControl10.AccessPointCapabilities();
            item.Capabilities.DisableAccessPoint = false;
            item.Capabilities.AccessTaken = false;
            item.Capabilities.AccessTakenSpecified = false;
            item.Capabilities.AnonymousAccess = false;
            item.Capabilities.AnonymousAccessSpecified = false;
            item.Capabilities.Duress = false;
            item.Capabilities.DuressSpecified = false;
            item.Capabilities.ExternalAuthorization = false;
            item.Capabilities.ExternalAuthorizationSpecified = false;
            item.Capabilities.Tamper = false;
            item.Capabilities.TamperSpecified = false;

            res.Add(item.token, item);

            return res;
        }

        private Dictionary<string, ServiceAccessControl10.AccessPointState> AccessPointStateDefaultInitialization()
        {
            Dictionary<string, ServiceAccessControl10.AccessPointState> res = new Dictionary<string, ServiceAccessControl10.AccessPointState>();

            ServiceAccessControl10.AccessPointState item;

            item = new ServiceAccessControl10.AccessPointState();
            item.Enabled = true;

            res.Add("tokenAccessPoint1", item);

            item = new ServiceAccessControl10.AccessPointState();
            item.Enabled = true;

            res.Add("tokenAccessPoint2", item);

            return res;
        }

        private Dictionary<string, bool> AccessPointTamperingStateDefaultInitialization()
        {
            Dictionary<string, bool> res = new Dictionary<string, bool>();

            res.Add("tokenAccessPoint1", true);
            res.Add("tokenAccessPoint2", false);

            return res;
        }

        //private Dictionary<string, CredentialInformation> CredentialInfoListInitialization()
        //{
        //    Dictionary<string, CredentialInformation> res = new Dictionary<string, CredentialInformation>();

        //    CredentialInformation item;

        //    item = new CredentialInformation();
        //    item.CredentialToken = "tokenCredential1";
        //    item.CredentialHolderName = "holderNameCredential1";

        //    res.Add(item.CredentialToken, item);

        //    item = new CredentialInformation();
        //    item.CredentialToken = "tokenCredential2";
        //    item.CredentialHolderName = "holderNameCredential2";

        //    res.Add(item.CredentialToken, item);

        //    return res;
        //}

        private Dictionary<string, ServiceDoorControl10.DoorInfo> DoorInfoDefaultInitialization()
        {
            Dictionary<string, ServiceDoorControl10.DoorInfo> res = new Dictionary<string, ServiceDoorControl10.DoorInfo>();

            ServiceDoorControl10.DoorInfo item;

            item = new ServiceDoorControl10.DoorInfo();
            item.token = "tokenDoor1";
            item.Name = "Door1 Name";
            item.Description = "Door1 Description";
            item.Capabilities = new DoorCapabilities();
            item.Capabilities.Access = true;
            item.Capabilities.AccessSpecified = true;
            item.Capabilities.AccessTimingOverride = true;
            item.Capabilities.AccessTimingOverrideSpecified = true;
            item.Capabilities.Alarm = true;
            item.Capabilities.AlarmSpecified = true;
            item.Capabilities.Block = true;
            item.Capabilities.BlockSpecified = true;
            item.Capabilities.DoorMonitor = true;
            item.Capabilities.DoorMonitorSpecified = true;
            item.Capabilities.DoubleLock = true;
            item.Capabilities.DoubleLockSpecified = true;
            item.Capabilities.DoubleLockMonitor = true;
            item.Capabilities.DoubleLockMonitorSpecified = true;
            item.Capabilities.Lock = true;
            item.Capabilities.LockSpecified = true;
            item.Capabilities.LockDown = true;
            item.Capabilities.LockDownSpecified = true;
            item.Capabilities.LockMonitor = true;
            item.Capabilities.LockMonitorSpecified = true;
            item.Capabilities.LockOpen = true;
            item.Capabilities.LockOpenSpecified = true;
            item.Capabilities.Tamper = true;
            item.Capabilities.TamperSpecified = true;
            item.Capabilities.Unlock = true;
            item.Capabilities.UnlockSpecified = true;
            item.Capabilities.Fault= true;
            item.Capabilities.FaultSpecified = true;

            res.Add(item.token, item);

            item = new ServiceDoorControl10.DoorInfo();
            item.token = "tokenDoor2";
            item.Name = "Door2 Name";
            item.Description = "Door2 Description";
            item.Capabilities = new DoorCapabilities();
            item.Capabilities.Access = false;
            item.Capabilities.AccessSpecified = true;
            item.Capabilities.AccessTimingOverride = false;
            item.Capabilities.AccessTimingOverrideSpecified = false;
            item.Capabilities.Alarm = false;
            item.Capabilities.Block = false;
            item.Capabilities.DoorMonitor = false;
            item.Capabilities.DoubleLock = false;
            item.Capabilities.DoubleLockMonitor = false;
            item.Capabilities.Lock = false;
            item.Capabilities.LockDown = false;
            item.Capabilities.LockMonitor = false;
            item.Capabilities.LockOpen = false;
            item.Capabilities.Tamper = false;
            item.Capabilities.Unlock = false;

            res.Add(item.token, item);

            return res;
        }

        private Dictionary<string, int> DoorAccessListDefaultInitialization()
        {
            Dictionary<string, int> res = new Dictionary<string, int>();

            string token = "tokenDoor1";

            res.Add(token, 0);

            token = "tokenDoor2";

            res.Add(token, 0);

            return res;
        }

        private Dictionary<string, ServiceDoorControl10.DoorMode> DoorAccessPreviousStateListInitialization()
        {
            Dictionary<string, ServiceDoorControl10.DoorMode> res = new Dictionary<string, ServiceDoorControl10.DoorMode>();

            string token = "tokenDoor1";

            res.Add(token, ServiceDoorControl10.DoorMode.Unknown);

            token = "tokenDoor2";

            res.Add(token, ServiceDoorControl10.DoorMode.Unknown);

            return res;
        }

        private Dictionary<string, ServiceDoorControl10.DoorState> DoorStateDefaultInitialization()
        {
            Dictionary<string, ServiceDoorControl10.DoorState> res = new Dictionary<string, ServiceDoorControl10.DoorState>();

            string token = "tokenDoor1";
            ServiceDoorControl10.DoorState value = new DUT.PACS.Simulator.ServiceDoorControl10.DoorState();

            value.Alarm = ServiceDoorControl10.DoorAlarmState.Normal;
            value.AlarmSpecified = true;
            value.DoubleLockPhysicalState = ServiceDoorControl10.LockPhysicalState.Locked;
            value.DoubleLockPhysicalStateSpecified = true;
            value.LockPhysicalState = ServiceDoorControl10.LockPhysicalState.Locked;
            value.LockPhysicalStateSpecified = true;
            value.DoorMode = ServiceDoorControl10.DoorMode.Locked;
            value.DoorPhysicalState = ServiceDoorControl10.DoorPhysicalState.Closed;
            value.DoorPhysicalStateSpecified = true;
            value.Fault = new DoorFault();
            value.Fault.State = DoorFaultState.NotInFault;
            value.Tamper = new DoorTamper();
            value.Tamper.State = ServiceDoorControl10.DoorTamperState.NotInTamper;
            value.Tamper.Reason = null;

            res.Add(token, value);

            token = "tokenDoor2";
            value = new DUT.PACS.Simulator.ServiceDoorControl10.DoorState();

            value.Alarm = ServiceDoorControl10.DoorAlarmState.Normal;
            value.AlarmSpecified = false;
            value.DoubleLockPhysicalState = ServiceDoorControl10.LockPhysicalState.Locked;
            value.DoubleLockPhysicalStateSpecified = false;
            value.LockPhysicalState = ServiceDoorControl10.LockPhysicalState.Locked;
            value.LockPhysicalStateSpecified = false;
            value.DoorMode = ServiceDoorControl10.DoorMode.Locked;
            value.DoorPhysicalState = ServiceDoorControl10.DoorPhysicalState.Closed;
            value.DoorPhysicalStateSpecified = false;
            value.Fault = null;
            value.Tamper = null;

            res.Add(token, value);

            return res;
        }

        private TriggerConfiguration TriggerDefaultInitialization()
        {
            TriggerConfiguration res = new TriggerConfiguration();

            res.Add("tokenDoor1", DoorMode.Locked, Sensor.DoorMonitor, 1000);
            res.Add("tokenDoor1", DoorMode.Locked, Sensor.DoorLockMonitor, 2000);
            res.Add("tokenDoor1", DoorMode.Locked, Sensor.DoorDoubleLockMonitor, 3000);

            res.Add("tokenDoor1", DoorMode.Unlocked, Sensor.DoorMonitor, 1000);
            res.Add("tokenDoor1", DoorMode.Unlocked, Sensor.DoorLockMonitor, 2000);
            res.Add("tokenDoor1", DoorMode.Unlocked, Sensor.DoorDoubleLockMonitor, 2000);


            return res;
        }

        #endregion //DefaultInitialization

        #region Serialization/Deserialization


        public SerializableConfiguration GetSerializableConfiguration()
        {
            if (!m_ConfStorageInitialized)
            {
                ConfStorageInitialization();
            }

            Configuration.SerializableConfiguration config = new SerializableConfiguration();

            config.AccessPointInfoList = m_AccessPointInfoList.Values.ToList();
            config.AreaInfoList = m_AreaInfoList.Values.ToList();
            config.DoorInfoList = m_DoorInfoList.Values.ToList();
            //config.CredentialList = m_CredentialList.Values.ToList();
            //config.AccessProfileList = m_AccessProfileList.Values.ToList();

            config.DoorInitialStates = new List<DoorStateHolder>();
            foreach (string token in m_DoorStateList.Keys)
            {
                config.DoorInitialStates.Add(new DoorStateHolder() { DoorToken = token, State = m_DoorStateList[token] });
            }

            config.AccessPointTamperingInitialState = new List<AccessPointTamperingStateHolder>();
            foreach (string token in m_AccessPointTamperingState.Keys)
            {
                config.AccessPointTamperingInitialState.Add(new AccessPointTamperingStateHolder() { AccessPointToken = token, State = m_AccessPointTamperingState[token] });
            }

            //config.CredentialInitialStates = new List<CredentialStateHolder>();
            //foreach (string token in m_CredentialStateList.Keys)
            //{
            //    config.CredentialInitialStates.Add(new CredentialStateHolder() { CredentialToken = token, State = m_CredentialStateList[token] });
            //}

            config.AccessPointInitialStates = new List<AccessPointStateHolder>();
            foreach (string token in m_AccessPointState.Keys)
            {
                config.AccessPointInitialStates.Add(new AccessPointStateHolder() { AccessPointToken = token, State = m_AccessPointState[token] });
            }

            //config.CredentialInformation = new List<CredentialInformation>();
            //config.CredentialInformation.AddRange(m_CredentialInformationList.Values);

            config.TriggerConfiguration = m_triggerConfiguration.Settings;
            return config;
        }

        public static ConfStorage Load(Configuration.SerializableConfiguration config)
        {
            ConfStorage storage = new ConfStorage();

            storage.m_AccessPointInfoList = new Dictionary<string, DUT.PACS.Simulator.ServiceAccessControl10.AccessPointInfo>();
            foreach (DUT.PACS.Simulator.ServiceAccessControl10.AccessPointInfo info in config.AccessPointInfoList)
            {
                storage.m_AccessPointInfoList.Add(info.token, info);
            }

            //storage.m_CredentialList = new Dictionary<string, DUT.PACS.Simulator.ServiceCredential10.Credential>();
            //foreach (DUT.PACS.Simulator.ServiceCredential10.Credential info in config.CredentialList)
            //{
            //    storage.m_CredentialList.Add(info.token, info);
            //}

            //storage.m_AccessProfileList = new Dictionary<string, DUT.PACS.Simulator.ServiceAccessRules10.AccessProfile>();
            //foreach (DUT.PACS.Simulator.ServiceAccessRules10.AccessProfile info in config.AccessProfileList)
            //{
            //    storage.m_AccessProfileList.Add(info.token, info);
            //}

            storage.m_AreaInfoList = new Dictionary<string, DUT.PACS.Simulator.ServiceAccessControl10.AreaInfo>();
            foreach (DUT.PACS.Simulator.ServiceAccessControl10.AreaInfo info in config.AreaInfoList)
            {
                storage.m_AreaInfoList.Add(info.token, info);
            }

            storage.m_DoorInfoList = new Dictionary<string, DoorInfo>();
            foreach (DoorInfo info in config.DoorInfoList)
            {
                storage.m_DoorInfoList.Add(info.token, info);
            }

            storage.m_triggerConfiguration = new TriggerConfiguration();
            storage.m_triggerConfiguration.Settings.AddRange(config.TriggerConfiguration);

            storage.m_DoorStateList = new Dictionary<string, DoorState>();
            storage.m_DoorAccessList = new Dictionary<string, int>();
            storage.m_DoorAccessPreviousStateList = new Dictionary<string, DoorMode>();

            foreach (DoorStateHolder holder in config.DoorInitialStates)
            {
                storage.m_DoorStateList.Add(holder.DoorToken, holder.State);
            }

            DoorState undefined = new DoorState();
            foreach (DoorInfo info in config.DoorInfoList)
            {
                if (!storage.m_DoorStateList.ContainsKey(info.token))
                {
                    storage.m_DoorStateList.Add(info.token, undefined);
                }

                storage.m_DoorAccessList.Add(info.token, 0);

                storage.m_DoorAccessPreviousStateList.Add(info.token, storage.m_DoorStateList[info.token].DoorMode);
            }

            //storage.m_CredentialStateList = new Dictionary<string, DUT.PACS.Simulator.ServiceCredential10.CredentialState>();
            //foreach (CredentialStateHolder holder in config.CredentialInitialStates)
            //{
            //    storage.m_CredentialStateList.Add(holder.CredentialToken, holder.State);
            //}

            //DUT.PACS.Simulator.ServiceCredential10.CredentialState undefinedCredential = new DUT.PACS.Simulator.ServiceCredential10.CredentialState();
            //foreach (DUT.PACS.Simulator.ServiceCredential10.Credential credential in config.CredentialList)
            //{
            //    if (!storage.m_CredentialStateList.ContainsKey(credential.token))
            //    {
            //        storage.m_CredentialStateList.Add(credential.token, undefinedCredential);
            //    }

            //}

            storage.m_AccessPointState = new Dictionary<string, DUT.PACS.Simulator.ServiceAccessControl10.AccessPointState>();

            foreach (AccessPointStateHolder state in config.AccessPointInitialStates)
            {
                storage.m_AccessPointState.Add(state.AccessPointToken, state.State);
            }

            storage.m_AccessPointTamperingState = new Dictionary<string, bool>();

            foreach (AccessPointTamperingStateHolder state in config.AccessPointTamperingInitialState)
            {
                storage.m_AccessPointTamperingState.Add(state.AccessPointToken, state.State);
            }

            storage.m_ConfStorageInitialized = true;

            //TEMP
            storage.m_CredentialList = storage.CredentialListInitialization();
            storage.m_CredentialStateList = storage.CredentialStateListInitialization();

            storage.m_AccessProfileList = storage.AccessProfileListInitialization();

            return storage;
        }

        #endregion

    }
}
