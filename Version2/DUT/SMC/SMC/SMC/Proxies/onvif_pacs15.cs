using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
namespace SMC.Proxies
{


    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ConfigurationName = "DoorControlPort")]
    public interface DoorControlPort
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/GetServiceCapabilities", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Capabilities")]
        DoorControlServiceCapabilities GetServiceCapabilities();

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorInfoList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        GetDoorInfoListResponse1 GetDoorInfoList(GetDoorInfoListRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorInfo", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "DoorInfo")]
        GetDoorInfoResponse GetDoorInfo(GetDoorInfoRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorState", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "DoorState")]
        DoorState GetDoorState(string Token);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/AccessDoor", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        AccessDoorResponse1 AccessDoor(AccessDoorRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/LockDoor", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        void LockDoor(string Token);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/UnlockDoor", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        void UnlockDoor(string Token);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/BlockDoor", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        void BlockDoor(string Token);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/LockDownDoor", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        void LockDownDoor(string Token);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/LockDownReleaseDoor", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        void LockDownReleaseDoor(string Token);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/LockOpenDoor", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        void LockOpenDoor(string Token);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/LockOpenReleaseDoor", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        void LockOpenReleaseDoor(string Token);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/doorcontrol/wsdl/DoubleLockDoor", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        void DoubleLockDoor(string Token);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public partial class DoorControlServiceCapabilities
    {

        private System.Xml.XmlElement[] anyField;

        private uint maxLimitField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxLimit
        {
            get
            {
                return this.maxLimitField;
            }
            set
            {
                this.maxLimitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class AccessPointState
    {

        private bool enabledField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public bool Enabled
        {
            get
            {
                return this.enabledField;
            }
            set
            {
                this.enabledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ServiceCapabilities", Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class AccessControlServiceCapabilities
    {

        private System.Xml.XmlElement[] anyField;

        private uint maxLimitField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxLimit
        {
            get
            {
                return this.maxLimitField;
            }
            set
            {
                this.maxLimitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public partial class AccessDoorExtension
    {

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public partial class DoorFault
    {

        private string reasonField;

        private DoorFaultState stateField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Reason
        {
            get
            {
                return this.reasonField;
            }
            set
            {
                this.reasonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public DoorFaultState State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public enum DoorFaultState
    {

        /// <remarks/>
        Unknown,

        /// <remarks/>
        NotInFault,

        /// <remarks/>
        FaultDetected,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public partial class DoorTamper
    {

        private string reasonField;

        private DoorTamperState stateField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Reason
        {
            get
            {
                return this.reasonField;
            }
            set
            {
                this.reasonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public DoorTamperState State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public enum DoorTamperState
    {

        /// <remarks/>
        Unknown,

        /// <remarks/>
        NotInTamper,

        /// <remarks/>
        TamperDetected,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public partial class DoorState
    {

        private DoorPhysicalState doorPhysicalStateField;

        private bool doorPhysicalStateFieldSpecified;

        private LockPhysicalState lockPhysicalStateField;

        private bool lockPhysicalStateFieldSpecified;

        private LockPhysicalState doubleLockPhysicalStateField;

        private bool doubleLockPhysicalStateFieldSpecified;

        private DoorAlarmState alarmField;

        private bool alarmFieldSpecified;

        private DoorTamper tamperField;

        private DoorFault faultField;

        private DoorMode doorModeField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public DoorPhysicalState DoorPhysicalState
        {
            get
            {
                return this.doorPhysicalStateField;
            }
            set
            {
                this.doorPhysicalStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DoorPhysicalStateSpecified
        {
            get
            {
                return this.doorPhysicalStateFieldSpecified;
            }
            set
            {
                this.doorPhysicalStateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public LockPhysicalState LockPhysicalState
        {
            get
            {
                return this.lockPhysicalStateField;
            }
            set
            {
                this.lockPhysicalStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LockPhysicalStateSpecified
        {
            get
            {
                return this.lockPhysicalStateFieldSpecified;
            }
            set
            {
                this.lockPhysicalStateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public LockPhysicalState DoubleLockPhysicalState
        {
            get
            {
                return this.doubleLockPhysicalStateField;
            }
            set
            {
                this.doubleLockPhysicalStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DoubleLockPhysicalStateSpecified
        {
            get
            {
                return this.doubleLockPhysicalStateFieldSpecified;
            }
            set
            {
                this.doubleLockPhysicalStateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public DoorAlarmState Alarm
        {
            get
            {
                return this.alarmField;
            }
            set
            {
                this.alarmField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AlarmSpecified
        {
            get
            {
                return this.alarmFieldSpecified;
            }
            set
            {
                this.alarmFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public DoorTamper Tamper
        {
            get
            {
                return this.tamperField;
            }
            set
            {
                this.tamperField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public DoorFault Fault
        {
            get
            {
                return this.faultField;
            }
            set
            {
                this.faultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public DoorMode DoorMode
        {
            get
            {
                return this.doorModeField;
            }
            set
            {
                this.doorModeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 7)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public enum DoorPhysicalState
    {

        /// <remarks/>
        Unknown,

        /// <remarks/>
        Open,

        /// <remarks/>
        Closed,

        /// <remarks/>
        Fault,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public enum LockPhysicalState
    {

        /// <remarks/>
        Unknown,

        /// <remarks/>
        Locked,

        /// <remarks/>
        Unlocked,

        /// <remarks/>
        Fault,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public enum DoorAlarmState
    {

        /// <remarks/>
        Normal,

        /// <remarks/>
        DoorForcedOpen,

        /// <remarks/>
        DoorOpenTooLong,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public enum DoorMode
    {

        /// <remarks/>
        Unknown,

        /// <remarks/>
        Locked,

        /// <remarks/>
        Unlocked,

        /// <remarks/>
        Accessed,

        /// <remarks/>
        Blocked,

        /// <remarks/>
        LockedDown,

        /// <remarks/>
        LockedOpen,

        /// <remarks/>
        DoubleLocked,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class AccessPointCapabilities
    {

        private System.Xml.XmlElement[] anyField;

        private bool disableAccessPointField;

        private bool duressField;

        private bool duressFieldSpecified;

        private bool accessTakenField;

        private bool accessTakenFieldSpecified;

        private bool externalAuthorizationField;

        private bool externalAuthorizationFieldSpecified;

        private bool tamperField;

        private bool tamperFieldSpecified;

        private bool anonymousAccessField;

        private bool anonymousAccessFieldSpecified;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool DisableAccessPoint
        {
            get
            {
                return this.disableAccessPointField;
            }
            set
            {
                this.disableAccessPointField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Duress
        {
            get
            {
                return this.duressField;
            }
            set
            {
                this.duressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DuressSpecified
        {
            get
            {
                return this.duressFieldSpecified;
            }
            set
            {
                this.duressFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool AccessTaken
        {
            get
            {
                return this.accessTakenField;
            }
            set
            {
                this.accessTakenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AccessTakenSpecified
        {
            get
            {
                return this.accessTakenFieldSpecified;
            }
            set
            {
                this.accessTakenFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool ExternalAuthorization
        {
            get
            {
                return this.externalAuthorizationField;
            }
            set
            {
                this.externalAuthorizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExternalAuthorizationSpecified
        {
            get
            {
                return this.externalAuthorizationFieldSpecified;
            }
            set
            {
                this.externalAuthorizationFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Tamper
        {
            get
            {
                return this.tamperField;
            }
            set
            {
                this.tamperField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TamperSpecified
        {
            get
            {
                return this.tamperFieldSpecified;
            }
            set
            {
                this.tamperFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool AnonymousAccess
        {
            get
            {
                return this.anonymousAccessField;
            }
            set
            {
                this.anonymousAccessField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AnonymousAccessSpecified
        {
            get
            {
                return this.anonymousAccessFieldSpecified;
            }
            set
            {
                this.anonymousAccessFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public partial class DoorCapabilities
    {

        private System.Xml.XmlElement[] anyField;

        private bool accessField;

        private bool accessFieldSpecified;

        private bool accessTimingOverrideField;

        private bool accessTimingOverrideFieldSpecified;

        private bool lockField;

        private bool lockFieldSpecified;

        private bool unlockField;

        private bool unlockFieldSpecified;

        private bool blockField;

        private bool blockFieldSpecified;

        private bool doubleLockField;

        private bool doubleLockFieldSpecified;

        private bool lockDownField;

        private bool lockDownFieldSpecified;

        private bool lockOpenField;

        private bool lockOpenFieldSpecified;

        private bool doorMonitorField;

        private bool doorMonitorFieldSpecified;

        private bool lockMonitorField;

        private bool lockMonitorFieldSpecified;

        private bool doubleLockMonitorField;

        private bool doubleLockMonitorFieldSpecified;

        private bool alarmField;

        private bool alarmFieldSpecified;

        private bool tamperField;

        private bool tamperFieldSpecified;

        private bool faultField;

        private bool faultFieldSpecified;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Access
        {
            get
            {
                return this.accessField;
            }
            set
            {
                this.accessField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AccessSpecified
        {
            get
            {
                return this.accessFieldSpecified;
            }
            set
            {
                this.accessFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool AccessTimingOverride
        {
            get
            {
                return this.accessTimingOverrideField;
            }
            set
            {
                this.accessTimingOverrideField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AccessTimingOverrideSpecified
        {
            get
            {
                return this.accessTimingOverrideFieldSpecified;
            }
            set
            {
                this.accessTimingOverrideFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Lock
        {
            get
            {
                return this.lockField;
            }
            set
            {
                this.lockField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LockSpecified
        {
            get
            {
                return this.lockFieldSpecified;
            }
            set
            {
                this.lockFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Unlock
        {
            get
            {
                return this.unlockField;
            }
            set
            {
                this.unlockField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UnlockSpecified
        {
            get
            {
                return this.unlockFieldSpecified;
            }
            set
            {
                this.unlockFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Block
        {
            get
            {
                return this.blockField;
            }
            set
            {
                this.blockField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BlockSpecified
        {
            get
            {
                return this.blockFieldSpecified;
            }
            set
            {
                this.blockFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool DoubleLock
        {
            get
            {
                return this.doubleLockField;
            }
            set
            {
                this.doubleLockField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DoubleLockSpecified
        {
            get
            {
                return this.doubleLockFieldSpecified;
            }
            set
            {
                this.doubleLockFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool LockDown
        {
            get
            {
                return this.lockDownField;
            }
            set
            {
                this.lockDownField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LockDownSpecified
        {
            get
            {
                return this.lockDownFieldSpecified;
            }
            set
            {
                this.lockDownFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool LockOpen
        {
            get
            {
                return this.lockOpenField;
            }
            set
            {
                this.lockOpenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LockOpenSpecified
        {
            get
            {
                return this.lockOpenFieldSpecified;
            }
            set
            {
                this.lockOpenFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool DoorMonitor
        {
            get
            {
                return this.doorMonitorField;
            }
            set
            {
                this.doorMonitorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DoorMonitorSpecified
        {
            get
            {
                return this.doorMonitorFieldSpecified;
            }
            set
            {
                this.doorMonitorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool LockMonitor
        {
            get
            {
                return this.lockMonitorField;
            }
            set
            {
                this.lockMonitorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LockMonitorSpecified
        {
            get
            {
                return this.lockMonitorFieldSpecified;
            }
            set
            {
                this.lockMonitorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool DoubleLockMonitor
        {
            get
            {
                return this.doubleLockMonitorField;
            }
            set
            {
                this.doubleLockMonitorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DoubleLockMonitorSpecified
        {
            get
            {
                return this.doubleLockMonitorFieldSpecified;
            }
            set
            {
                this.doubleLockMonitorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Alarm
        {
            get
            {
                return this.alarmField;
            }
            set
            {
                this.alarmField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AlarmSpecified
        {
            get
            {
                return this.alarmFieldSpecified;
            }
            set
            {
                this.alarmFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Tamper
        {
            get
            {
                return this.tamperField;
            }
            set
            {
                this.tamperField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TamperSpecified
        {
            get
            {
                return this.tamperFieldSpecified;
            }
            set
            {
                this.tamperFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Fault
        {
            get
            {
                return this.faultField;
            }
            set
            {
                this.faultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FaultSpecified
        {
            get
            {
                return this.faultFieldSpecified;
            }
            set
            {
                this.faultFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AreaInfo))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class AreaInfoBase : DataEntity
    {

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class AreaInfo : AreaInfoBase
    {

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AccessPointInfo))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class AccessPointInfoBase : DataEntity
    {

        private string nameField;

        private string descriptionField;

        private string areaFromField;

        private string areaToField;

        private System.Xml.XmlQualifiedName entityTypeField;

        private string entityField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string AreaFrom
        {
            get
            {
                return this.areaFromField;
            }
            set
            {
                this.areaFromField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string AreaTo
        {
            get
            {
                return this.areaToField;
            }
            set
            {
                this.areaToField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public System.Xml.XmlQualifiedName EntityType
        {
            get
            {
                return this.entityTypeField;
            }
            set
            {
                this.entityTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string Entity
        {
            get
            {
                return this.entityField;
            }
            set
            {
                this.entityField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class AccessPointInfo : AccessPointInfoBase
    {

        private AccessPointCapabilities capabilitiesField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public AccessPointCapabilities Capabilities
        {
            get
            {
                return this.capabilitiesField;
            }
            set
            {
                this.capabilitiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DoorInfo))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public partial class DoorInfoBase : DataEntity
    {

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public partial class DoorInfo : DoorInfoBase
    {

        private DoorCapabilities capabilitiesField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public DoorCapabilities Capabilities
        {
            get
            {
                return this.capabilitiesField;
            }
            set
            {
                this.capabilitiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public partial class GetDoorInfoList
    {

        private int limitField;

        private bool limitFieldSpecified;

        private string startReferenceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Limit
        {
            get
            {
                return this.limitField;
            }
            set
            {
                this.limitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LimitSpecified
        {
            get
            {
                return this.limitFieldSpecified;
            }
            set
            {
                this.limitFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string StartReference
        {
            get
            {
                return this.startReferenceField;
            }
            set
            {
                this.startReferenceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public partial class GetDoorInfoListResponse
    {

        private string nextStartReferenceField;

        private DoorInfo[] doorInfoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string NextStartReference
        {
            get
            {
                return this.nextStartReferenceField;
            }
            set
            {
                this.nextStartReferenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DoorInfo", Order = 1)]
        public DoorInfo[] DoorInfo
        {
            get
            {
                return this.doorInfoField;
            }
            set
            {
                this.doorInfoField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetDoorInfoListRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Order = 0)]
        public GetDoorInfoList GetDoorInfoList;

        public GetDoorInfoListRequest()
        {
        }

        public GetDoorInfoListRequest(GetDoorInfoList GetDoorInfoList)
        {
            this.GetDoorInfoList = GetDoorInfoList;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetDoorInfoListResponse1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Order = 0)]
        public GetDoorInfoListResponse GetDoorInfoListResponse;

        public GetDoorInfoListResponse1()
        {
        }

        public GetDoorInfoListResponse1(GetDoorInfoListResponse GetDoorInfoListResponse)
        {
            this.GetDoorInfoListResponse = GetDoorInfoListResponse;
        }
    }


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetDoorInfo", WrapperNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", IsWrapped = true)]
    public partial class GetDoorInfoRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Token")]
        public string[] Token;

        public GetDoorInfoRequest()
        {
        }

        public GetDoorInfoRequest(string[] Token)
        {
            this.Token = Token;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetDoorInfoResponse", WrapperNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", IsWrapped = true)]
    public partial class GetDoorInfoResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DoorInfo")]
        public DoorInfo[] DoorInfo;

        public GetDoorInfoResponse()
        {
        }

        public GetDoorInfoResponse(DoorInfo[] DoorInfo)
        {
            this.DoorInfo = DoorInfo;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public partial class AccessDoor
    {

        private string tokenField;

        private bool useExtendedTimeField;

        private bool useExtendedTimeFieldSpecified;

        private string accessTimeField;

        private string openTooLongTimeField;

        private string preAlarmTimeField;

        private AccessDoorExtension extensionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Token
        {
            get
            {
                return this.tokenField;
            }
            set
            {
                this.tokenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool UseExtendedTime
        {
            get
            {
                return this.useExtendedTimeField;
            }
            set
            {
                this.useExtendedTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UseExtendedTimeSpecified
        {
            get
            {
                return this.useExtendedTimeFieldSpecified;
            }
            set
            {
                this.useExtendedTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration", Order = 2)]
        public string AccessTime
        {
            get
            {
                return this.accessTimeField;
            }
            set
            {
                this.accessTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration", Order = 3)]
        public string OpenTooLongTime
        {
            get
            {
                return this.openTooLongTimeField;
            }
            set
            {
                this.openTooLongTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration", Order = 4)]
        public string PreAlarmTime
        {
            get
            {
                return this.preAlarmTimeField;
            }
            set
            {
                this.preAlarmTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public AccessDoorExtension Extension
        {
            get
            {
                return this.extensionField;
            }
            set
            {
                this.extensionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    public partial class AccessDoorResponse
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class AccessDoorRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Order = 0)]
        public AccessDoor AccessDoor;

        public AccessDoorRequest()
        {
        }

        public AccessDoorRequest(AccessDoor AccessDoor)
        {
            this.AccessDoor = AccessDoor;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class AccessDoorResponse1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Order = 0)]
        public AccessDoorResponse AccessDoorResponse;

        public AccessDoorResponse1()
        {
        }

        public AccessDoorResponse1(AccessDoorResponse AccessDoorResponse)
        {
            this.AccessDoorResponse = AccessDoorResponse;
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface DoorControlPortChannel : DoorControlPort, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class DoorControlPortClient : System.ServiceModel.ClientBase<DoorControlPort>, DoorControlPort
    {

        public DoorControlPortClient()
        {
        }

        public DoorControlPortClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public DoorControlPortClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public DoorControlPortClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public DoorControlPortClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public DoorControlServiceCapabilities GetServiceCapabilities()
        {
            return base.Channel.GetServiceCapabilities();
        }

        public string GetDoorInfoList(int? Limit, string StartReference, out DoorInfo[] DoorInfo)
        {
            GetDoorInfoList inValue = new GetDoorInfoList();
            inValue.LimitSpecified = Limit.HasValue;
            inValue.Limit = Limit.GetValueOrDefault();
            inValue.StartReference = StartReference;
            GetDoorInfoListResponse retVal = GetDoorInfoList(inValue);
            DoorInfo = retVal.DoorInfo;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetDoorInfoListResponse1 DoorControlPort.GetDoorInfoList(GetDoorInfoListRequest request)
        {
            return base.Channel.GetDoorInfoList(request);
        }

        public GetDoorInfoListResponse GetDoorInfoList(GetDoorInfoList GetDoorInfoList1)
        {
            GetDoorInfoListRequest inValue = new GetDoorInfoListRequest();
            inValue.GetDoorInfoList = GetDoorInfoList1;
            GetDoorInfoListResponse1 retVal = ((DoorControlPort)(this)).GetDoorInfoList(inValue);
            return retVal.GetDoorInfoListResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetDoorInfoResponse DoorControlPort.GetDoorInfo(GetDoorInfoRequest request)
        {
            return base.Channel.GetDoorInfo(request);
        }

        public DoorInfo[] GetDoorInfo(string[] Token)
        {
            GetDoorInfoRequest inValue = new GetDoorInfoRequest();
            inValue.Token = Token;
            GetDoorInfoResponse retVal = ((DoorControlPort)(this)).GetDoorInfo(inValue);
            return retVal.DoorInfo;
        }

        public DoorState GetDoorState(string Token)
        {
            return base.Channel.GetDoorState(Token);
        }

        public void AccessDoor(string Token, bool? UseExtendedTime, string AccessTime, string OpenTooLongTime, string PreAlarmTime, AccessDoorExtension Extension)
        {
            AccessDoor inValue = new AccessDoor();
            inValue.Token = Token;
            inValue.UseExtendedTimeSpecified = UseExtendedTime.HasValue;
            inValue.UseExtendedTime = UseExtendedTime.GetValueOrDefault();
            inValue.AccessTime = AccessTime;
            inValue.OpenTooLongTime = OpenTooLongTime;
            inValue.PreAlarmTime = PreAlarmTime;
            inValue.Extension = Extension;
            AccessDoorResponse retVal = AccessDoor(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        AccessDoorResponse1 DoorControlPort.AccessDoor(AccessDoorRequest request)
        {
            return base.Channel.AccessDoor(request);
        }

        public AccessDoorResponse AccessDoor(AccessDoor AccessDoor1)
        {
            AccessDoorRequest inValue = new AccessDoorRequest();
            inValue.AccessDoor = AccessDoor1;
            AccessDoorResponse1 retVal = ((DoorControlPort)(this)).AccessDoor(inValue);
            return retVal.AccessDoorResponse;
        }

        public void LockDoor(string Token)
        {
            base.Channel.LockDoor(Token);
        }

        public void UnlockDoor(string Token)
        {
            base.Channel.UnlockDoor(Token);
        }

        public void BlockDoor(string Token)
        {
            base.Channel.BlockDoor(Token);
        }

        public void LockDownDoor(string Token)
        {
            base.Channel.LockDownDoor(Token);
        }

        public void LockDownReleaseDoor(string Token)
        {
            base.Channel.LockDownReleaseDoor(Token);
        }

        public void LockOpenDoor(string Token)
        {
            base.Channel.LockOpenDoor(Token);
        }

        public void LockOpenReleaseDoor(string Token)
        {
            base.Channel.LockOpenReleaseDoor(Token);
        }

        public void DoubleLockDoor(string Token)
        {
            base.Channel.DoubleLockDoor(Token);
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ConfigurationName = "PACSPort")]
    public interface PACSPort
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/accesscontrol/wsdl/GetServiceCapabilities", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Capabilities")]
        AccessControlServiceCapabilities GetServiceCapabilities();

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointInfoList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        GetAccessPointInfoListResponse1 GetAccessPointInfoList(GetAccessPointInfoListRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointInfo", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "AccessPointInfo")]
        GetAccessPointInfoResponse GetAccessPointInfo(GetAccessPointInfoRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointState", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "AccessPointState")]
        AccessPointState GetAccessPointState(string Token);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/accesscontrol/wsdl/EnableAccessPoint", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        void EnableAccessPoint(string Token);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/accesscontrol/wsdl/DisableAccessPoint", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        void DisableAccessPoint(string Token);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/accesscontrol/wsdl/ExternalAuthorization", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        void ExternalAuthorization(string AccessPointToken, string CredentialToken, string Reason, Decision Decision);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/accesscontrol/wsdl/GetAreaInfoList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        GetAreaInfoListResponse1 GetAreaInfoList(GetAreaInfoListRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/accesscontrol/wsdl/GetAreaInfo", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "AreaInfo")]
        GetAreaInfoResponse GetAreaInfo(GetAreaInfoRequest request);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class GetAccessPointInfoList
    {

        private int limitField;

        private bool limitFieldSpecified;

        private string startReferenceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Limit
        {
            get
            {
                return this.limitField;
            }
            set
            {
                this.limitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LimitSpecified
        {
            get
            {
                return this.limitFieldSpecified;
            }
            set
            {
                this.limitFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string StartReference
        {
            get
            {
                return this.startReferenceField;
            }
            set
            {
                this.startReferenceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class GetAccessPointInfoListResponse
    {

        private string nextStartReferenceField;

        private AccessPointInfo[] accessPointInfoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string NextStartReference
        {
            get
            {
                return this.nextStartReferenceField;
            }
            set
            {
                this.nextStartReferenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AccessPointInfo", Order = 1)]
        public AccessPointInfo[] AccessPointInfo
        {
            get
            {
                return this.accessPointInfoField;
            }
            set
            {
                this.accessPointInfoField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetAccessPointInfoListRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Order = 0)]
        public GetAccessPointInfoList GetAccessPointInfoList;

        public GetAccessPointInfoListRequest()
        {
        }

        public GetAccessPointInfoListRequest(GetAccessPointInfoList GetAccessPointInfoList)
        {
            this.GetAccessPointInfoList = GetAccessPointInfoList;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetAccessPointInfoListResponse1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Order = 0)]
        public GetAccessPointInfoListResponse GetAccessPointInfoListResponse;

        public GetAccessPointInfoListResponse1()
        {
        }

        public GetAccessPointInfoListResponse1(GetAccessPointInfoListResponse GetAccessPointInfoListResponse)
        {
            this.GetAccessPointInfoListResponse = GetAccessPointInfoListResponse;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetAccessPointInfo", WrapperNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", IsWrapped = true)]
    public partial class GetAccessPointInfoRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Token")]
        public string[] Token;

        public GetAccessPointInfoRequest()
        {
        }

        public GetAccessPointInfoRequest(string[] Token)
        {
            this.Token = Token;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetAccessPointInfoResponse", WrapperNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", IsWrapped = true)]
    public partial class GetAccessPointInfoResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("AccessPointInfo")]
        public AccessPointInfo[] AccessPointInfo;

        public GetAccessPointInfoResponse()
        {
        }

        public GetAccessPointInfoResponse(AccessPointInfo[] AccessPointInfo)
        {
            this.AccessPointInfo = AccessPointInfo;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public enum Decision
    {

        /// <remarks/>
        Granted,

        /// <remarks/>
        Denied,
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class GetAreaInfoList
    {

        private int limitField;

        private bool limitFieldSpecified;

        private string startReferenceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Limit
        {
            get
            {
                return this.limitField;
            }
            set
            {
                this.limitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LimitSpecified
        {
            get
            {
                return this.limitFieldSpecified;
            }
            set
            {
                this.limitFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string StartReference
        {
            get
            {
                return this.startReferenceField;
            }
            set
            {
                this.startReferenceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class GetAreaInfoListResponse
    {

        private string nextStartReferenceField;

        private AreaInfo[] areaInfoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string NextStartReference
        {
            get
            {
                return this.nextStartReferenceField;
            }
            set
            {
                this.nextStartReferenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AreaInfo", Order = 1)]
        public AreaInfo[] AreaInfo
        {
            get
            {
                return this.areaInfoField;
            }
            set
            {
                this.areaInfoField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetAreaInfoListRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Order = 0)]
        public GetAreaInfoList GetAreaInfoList;

        public GetAreaInfoListRequest()
        {
        }

        public GetAreaInfoListRequest(GetAreaInfoList GetAreaInfoList)
        {
            this.GetAreaInfoList = GetAreaInfoList;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetAreaInfoListResponse1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Order = 0)]
        public GetAreaInfoListResponse GetAreaInfoListResponse;

        public GetAreaInfoListResponse1()
        {
        }

        public GetAreaInfoListResponse1(GetAreaInfoListResponse GetAreaInfoListResponse)
        {
            this.GetAreaInfoListResponse = GetAreaInfoListResponse;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetAreaInfo", WrapperNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", IsWrapped = true)]
    public partial class GetAreaInfoRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Token")]
        public string[] Token;

        public GetAreaInfoRequest()
        {
        }

        public GetAreaInfoRequest(string[] Token)
        {
            this.Token = Token;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetAreaInfoResponse", WrapperNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", IsWrapped = true)]
    public partial class GetAreaInfoResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("AreaInfo")]
        public AreaInfo[] AreaInfo;

        public GetAreaInfoResponse()
        {
        }

        public GetAreaInfoResponse(AreaInfo[] AreaInfo)
        {
            this.AreaInfo = AreaInfo;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface PACSPortChannel : PACSPort, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class PACSPortClient : System.ServiceModel.ClientBase<PACSPort>, PACSPort
    {

        public PACSPortClient()
        {
        }

        public PACSPortClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public PACSPortClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public PACSPortClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public PACSPortClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public AccessControlServiceCapabilities GetServiceCapabilities()
        {
            return base.Channel.GetServiceCapabilities();
        }


        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetAccessPointInfoListResponse1 PACSPort.GetAccessPointInfoList(GetAccessPointInfoListRequest request)
        {
            return base.Channel.GetAccessPointInfoList(request);
        }

        public GetAccessPointInfoListResponse GetAccessPointInfoList(GetAccessPointInfoList GetAccessPointInfoList1)
        {
            GetAccessPointInfoListRequest inValue = new GetAccessPointInfoListRequest();
            inValue.GetAccessPointInfoList = GetAccessPointInfoList1;
            GetAccessPointInfoListResponse1 retVal = ((PACSPort)(this)).GetAccessPointInfoList(inValue);
            return retVal.GetAccessPointInfoListResponse;
        }

        public string GetAccessPointInfoList(int? Limit, string StartReference, out AccessPointInfo[] AccessPointInfo)
        {
            GetAccessPointInfoList inValue = new GetAccessPointInfoList();
            inValue.LimitSpecified = Limit.HasValue;
            inValue.Limit = Limit.GetValueOrDefault();
            inValue.StartReference = StartReference;
            GetAccessPointInfoListResponse retVal = GetAccessPointInfoList(inValue);
            AccessPointInfo = retVal.AccessPointInfo;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetAccessPointInfoResponse PACSPort.GetAccessPointInfo(GetAccessPointInfoRequest request)
        {
            return base.Channel.GetAccessPointInfo(request);
        }

        public AccessPointInfo[] GetAccessPointInfo(string[] Token)
        {
            GetAccessPointInfoRequest inValue = new GetAccessPointInfoRequest();
            inValue.Token = Token;
            GetAccessPointInfoResponse retVal = ((PACSPort)(this)).GetAccessPointInfo(inValue);
            return retVal.AccessPointInfo;
        }

        public AccessPointState GetAccessPointState(string Token)
        {
            return base.Channel.GetAccessPointState(Token);
        }

        public void EnableAccessPoint(string Token)
        {
            base.Channel.EnableAccessPoint(Token);
        }

        public void DisableAccessPoint(string Token)
        {
            base.Channel.DisableAccessPoint(Token);
        }

        public void ExternalAuthorization(string AccessPointToken, string CredentialToken, string Reason, Decision Decision)
        {
            base.Channel.ExternalAuthorization(AccessPointToken, CredentialToken, Reason, Decision);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetAreaInfoListResponse1 PACSPort.GetAreaInfoList(GetAreaInfoListRequest request)
        {
            return base.Channel.GetAreaInfoList(request);
        }

        public GetAreaInfoListResponse GetAreaInfoList(GetAreaInfoList GetAreaInfoList1)
        {
            GetAreaInfoListRequest inValue = new GetAreaInfoListRequest();
            inValue.GetAreaInfoList = GetAreaInfoList1;
            GetAreaInfoListResponse1 retVal = ((PACSPort)(this)).GetAreaInfoList(inValue);
            return retVal.GetAreaInfoListResponse;
        }

        public string GetAreaInfoList(int? Limit, string StartReference, out AreaInfo[] AreaInfo)
        {
            GetAreaInfoList inValue = new GetAreaInfoList();
            inValue.LimitSpecified = Limit.HasValue;
            inValue.Limit = Limit.GetValueOrDefault();
            inValue.StartReference = StartReference;
            GetAreaInfoListResponse retVal = GetAreaInfoList(inValue);
            AreaInfo = retVal.AreaInfo;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetAreaInfoResponse PACSPort.GetAreaInfo(GetAreaInfoRequest request)
        {
            return base.Channel.GetAreaInfo(request);
        }

        public AreaInfo[] GetAreaInfo(string[] Token)
        {
            GetAreaInfoRequest inValue = new GetAreaInfoRequest();
            inValue.Token = Token;
            GetAreaInfoResponse retVal = ((PACSPort)(this)).GetAreaInfo(inValue);
            return retVal.AreaInfo;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class ScheduleStateExtension
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class ScheduleState
    {

        private bool activeField;

        private bool specialDayField;

        private bool specialDayFieldSpecified;

        private ScheduleStateExtension extensionField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public bool Active
        {
            get { return this.activeField; }
            set { this.activeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool SpecialDay
        {
            get { return this.specialDayField; }
            set { this.specialDayField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SpecialDaySpecified
        {
            get { return this.specialDayFieldSpecified; }
            set { this.specialDayFieldSpecified = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public ScheduleStateExtension Extension
        {
            get { return this.extensionField; }
            set { this.extensionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ServiceCapabilities",
            Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class ScheduleServiceCapabilities
    {

        private uint maxLimitField;

        private uint maxSchedulesField;

        private uint maxTimePeriodsPerDayField;

        private uint maxSpecialDayGroupsField;

        private uint maxSpecialDaysInSpecialDayGroupField;

        private uint maxSpecialDaysSchedulesField;

        private bool extendedRecurrenceSupportedField;

        private bool specialDaysSupportedField;

        private bool stateReportingSupportedField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxLimit
        {
            get { return this.maxLimitField; }
            set { this.maxLimitField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxSchedules
        {
            get { return this.maxSchedulesField; }
            set { this.maxSchedulesField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxTimePeriodsPerDay
        {
            get { return this.maxTimePeriodsPerDayField; }
            set { this.maxTimePeriodsPerDayField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxSpecialDayGroups
        {
            get { return this.maxSpecialDayGroupsField; }
            set { this.maxSpecialDayGroupsField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxDaysInSpecialDayGroup
        {
            get { return this.maxSpecialDaysInSpecialDayGroupField; }
            set { this.maxSpecialDaysInSpecialDayGroupField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxSpecialDaysSchedules
        {
            get { return this.maxSpecialDaysSchedulesField; }
            set { this.maxSpecialDaysSchedulesField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool ExtendedRecurrenceSupported
        {
            get { return this.extendedRecurrenceSupportedField; }
            set { this.extendedRecurrenceSupportedField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool SpecialDaysSupported
        {
            get { return this.specialDaysSupportedField; }
            set { this.specialDaysSupportedField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool StateReportingSupported
        {
            get { return this.stateReportingSupportedField; }
            set { this.stateReportingSupportedField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    public partial class AntipassbackState
    {

        private bool antipassbackViolatedField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public bool AntipassbackViolated
        {
            get { return this.antipassbackViolatedField; }
            set { this.antipassbackViolatedField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    public partial class CredentialState
    {

        private bool enabledField;

        private string reasonField;

        private AntipassbackState antipassbackStateField;

        private CredentialExtension extensionField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public bool Enabled
        {
            get { return this.enabledField; }
            set { this.enabledField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Reason
        {
            get { return this.reasonField; }
            set { this.reasonField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public AntipassbackState AntipassbackState
        {
            get { return this.antipassbackStateField; }
            set { this.antipassbackStateField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public CredentialExtension Extension
        {
            get
            {
                return this.extensionField;
            }
            set
            {
                this.extensionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    public partial class CredentialIdentifierFormatTypeInfoExtension
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    public partial class CredentialIdentifierFormatTypeInfo
    {

        private string formatTypeField;

        private string descriptionField;

        private CredentialIdentifierFormatTypeInfoExtension extensionField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string FormatType
        {
            get { return this.formatTypeField; }
            set { this.formatTypeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Description
        {
            get { return this.descriptionField; }
            set { this.descriptionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public CredentialIdentifierFormatTypeInfoExtension Extension
        {
            get { return this.extensionField; }
            set { this.extensionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    //[System.SerializableAttribute()]
    public class CredentialIdentifierValue
    {
        public CredentialIdentifierValue() { }

        public CredentialIdentifierValue(string typeName,
                                         string formatType,
                                         byte[] value)
        {
            TypeName = typeName;
            FormatType = formatType;
            Value = value;
        }

        public CredentialIdentifierValue(string typeName,
                                         CredentialIdentifierFormatTypeInfo formatType,
                                         byte[] value)
            : this(typeName, formatType.FormatType, value)
        { }

        //[System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string TypeName { get; set; }

        //[System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string FormatType { get; set; }

        //[System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public byte[] Value { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    public partial class ServiceCapabilitiesExtension
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ServiceCapabilities",
            Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    public partial class CredentialServiceCapabilities
    {

        private string[] supportedIdentifierTypeField;

        private ServiceCapabilitiesExtension extensionField;

        private uint maxLimitField;

        private bool credentialValiditySupportedField;

        private bool credentialAccessProfileValiditySupportedField;

        private bool validitySupportsTimeValueField;

        private uint maxCredentialsField;

        private uint maxAccessProfilesPerCredentialField;

        private bool resetAntipassbackSupportedField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SupportedIdentifierType", Order = 0)]
        public string[] SupportedIdentifierType
        {
            get { return this.supportedIdentifierTypeField; }
            set { this.supportedIdentifierTypeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public ServiceCapabilitiesExtension Extension
        {
            get { return this.extensionField; }
            set { this.extensionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxLimit
        {
            get { return this.maxLimitField; }
            set { this.maxLimitField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool CredentialValiditySupported
        {
            get { return this.credentialValiditySupportedField; }
            set { this.credentialValiditySupportedField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool CredentialAccessProfileValiditySupported
        {
            get { return this.credentialAccessProfileValiditySupportedField; }
            set { this.credentialAccessProfileValiditySupportedField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool ValiditySupportsTimeValue
        {
            get { return this.validitySupportsTimeValueField; }
            set { this.validitySupportsTimeValueField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxCredentials
        {
            get { return this.maxCredentialsField; }
            set { this.maxCredentialsField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxAccessProfilesPerCredential
        {
            get { return this.maxAccessProfilesPerCredentialField; }
            set { this.maxAccessProfilesPerCredentialField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool ResetAntipassbackSupported
        {
            get { return this.resetAntipassbackSupportedField; }
            set { this.resetAntipassbackSupportedField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class SpecialDayGroupExtension
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class ScheduleExtension
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class SpecialDaysScheduleExtension
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class TimePeriodExtension
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class TimePeriod
    {

        private System.DateTime fromField;

        private System.DateTime untilField;

        private bool untilFieldSpecified;

        private TimePeriodExtension extensionField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "time", Order = 0)]
        public System.DateTime From
        {
            get { return this.fromField; }
            set { this.fromField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "time", Order = 1)]
        public System.DateTime Until
        {
            get { return this.untilField; }
            set { this.untilField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UntilSpecified
        {
            get { return this.untilFieldSpecified; }
            set { this.untilFieldSpecified = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public TimePeriodExtension Extension
        {
            get { return this.extensionField; }
            set { this.extensionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class SpecialDaysSchedule
    {

        private string groupTokenField;

        private TimePeriod[] timeRangeField;

        private SpecialDaysScheduleExtension extensionField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string GroupToken
        {
            get { return this.groupTokenField; }
            set { this.groupTokenField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TimeRange", Order = 1)]
        public TimePeriod[] TimeRange
        {
            get { return this.timeRangeField; }
            set { this.timeRangeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public SpecialDaysScheduleExtension Extension
        {
            get { return this.extensionField; }
            set { this.extensionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    public partial class CredentialExtension
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/pacs")]
    public partial class Attribute
    {

        private System.Xml.XmlElement[] anyField;

        private string nameField;

        private string valueField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get { return this.valueField; }
            set { this.valueField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    public partial class CredentialAccessProfile
    {

        private string accessProfileTokenField;

        private System.DateTime validFromField;

        private bool validFromFieldSpecified;

        private System.DateTime validToField;

        private bool validToFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AccessProfileToken
        {
            get { return this.accessProfileTokenField; }
            set { this.accessProfileTokenField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public System.DateTime ValidFrom
        {
            get { return this.validFromField; }
            set { this.validFromField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ValidFromSpecified
        {
            get { return this.validFromFieldSpecified; }
            set { this.validFromFieldSpecified = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public System.DateTime ValidTo
        {
            get { return this.validToField; }
            set { this.validToField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ValidToSpecified
        {
            get { return this.validToFieldSpecified; }
            set { this.validToFieldSpecified = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    public partial class CredentialIdentifierType
    {

        private string nameField;

        private string formatTypeField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string FormatType
        {
            get { return this.formatTypeField; }
            set { this.formatTypeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    public partial class CredentialIdentifier
    {

        private CredentialIdentifierType typeField;

        private bool exemptedFromAuthenticationField;

        private byte[] valueField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public CredentialIdentifierType Type
        {
            get { return this.typeField; }
            set { this.typeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool ExemptedFromAuthentication
        {
            get { return this.exemptedFromAuthenticationField; }
            set { this.exemptedFromAuthenticationField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "hexBinary", Order = 2)]
        public byte[] Value
        {
            get { return this.valueField; }
            set { this.valueField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 3)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl")]
    public partial class AccessProfileExtension
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl")]
    public partial class AccessPolicyExtension
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl")]
    public partial class AccessPolicy
    {

        private string scheduleTokenField;

        private string entityField;

        private System.Xml.XmlQualifiedName entityTypeField;

        private AccessPolicyExtension extensionField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ScheduleToken
        {
            get { return this.scheduleTokenField; }
            set { this.scheduleTokenField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Entity
        {
            get { return this.entityField; }
            set { this.entityField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public System.Xml.XmlQualifiedName EntityType
        {
            get { return this.entityTypeField; }
            set { this.entityTypeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public AccessPolicyExtension Extension
        {
            get { return this.extensionField; }
            set { this.extensionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SpecialDayGroupInfo))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SpecialDayGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ScheduleInfo))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Schedule))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CredentialInfo))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Credential))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AccessProfileInfo))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AccessProfile))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/pacs")]
    public partial class DataEntity
    {

        private string tokenField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string token
        {
            get { return this.tokenField; }
            set { this.tokenField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SpecialDayGroup))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class SpecialDayGroupInfo : DataEntity
    {

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Description
        {
            get { return this.descriptionField; }
            set { this.descriptionField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class SpecialDayGroup : SpecialDayGroupInfo
    {

        private string daysField;

        private SpecialDayGroupExtension extensionField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Days
        {
            get { return this.daysField; }
            set
            {
                Regex r = new Regex("(?<!\r)\n");
                this.daysField = r.Replace(value, "\r\n");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SpecialDayGroupExtension Extension
        {
            get { return this.extensionField; }
            set { this.extensionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Schedule))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class ScheduleInfo : DataEntity
    {

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Description
        {
            get { return this.descriptionField; }
            set { this.descriptionField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    public partial class Schedule : ScheduleInfo
    {

        private string standardField;

        private SpecialDaysSchedule[] specialDaysField;

        private ScheduleExtension extensionField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Standard
        {
            get { return this.standardField; }
            set
            {
                Regex r = new Regex("(?<!\r)\n");
                this.standardField = r.Replace(value, "\r\n");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SpecialDays", Order = 1)]
        public SpecialDaysSchedule[] SpecialDays
        {
            get { return this.specialDaysField; }
            set { this.specialDaysField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public ScheduleExtension Extension
        {
            get { return this.extensionField; }
            set { this.extensionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Credential))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    public partial class CredentialInfo : DataEntity
    {

        private string descriptionField;

        private string credentialHolderReferenceField;

        private System.DateTime validFromField;

        private bool validFromFieldSpecified;

        private System.DateTime validToField;

        private bool validToFieldSpecified;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Description
        {
            get { return this.descriptionField; }
            set { this.descriptionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string CredentialHolderReference
        {
            get { return this.credentialHolderReferenceField; }
            set { this.credentialHolderReferenceField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public System.DateTime ValidFrom
        {
            get { return this.validFromField; }
            set { this.validFromField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ValidFromSpecified
        {
            get { return this.validFromFieldSpecified; }
            set { this.validFromFieldSpecified = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public System.DateTime ValidTo
        {
            get { return this.validToField; }
            set { this.validToField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ValidToSpecified
        {
            get { return this.validToFieldSpecified; }
            set { this.validToFieldSpecified = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl")]
    public partial class Credential : CredentialInfo
    {

        private CredentialIdentifier[] credentialIdentifierField;

        private CredentialAccessProfile[] credentialAccessProfileField;

        private Attribute[] attributeField;

        private CredentialExtension extensionField;

        private System.Xml.XmlAttribute[] anyAttr1Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CredentialIdentifier", Order = 0)]
        public CredentialIdentifier[] CredentialIdentifier
        {
            get { return this.credentialIdentifierField; }
            set { this.credentialIdentifierField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CredentialAccessProfile", Order = 1)]
        public CredentialAccessProfile[] CredentialAccessProfile
        {
            get { return this.credentialAccessProfileField; }
            set { this.credentialAccessProfileField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Attribute", Order = 2)]
        public Attribute[] Attribute
        {
            get { return this.attributeField; }
            set { this.attributeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public CredentialExtension Extension
        {
            get { return this.extensionField; }
            set { this.extensionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr1
        {
            get { return this.anyAttr1Field; }
            set { this.anyAttr1Field = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AccessProfile))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl")]
    public partial class AccessProfileInfo : DataEntity
    {

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Description
        {
            get { return this.descriptionField; }
            set { this.descriptionField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl")]
    public partial class AccessProfile : AccessProfileInfo
    {

        private AccessPolicy[] accessPolicyField;

        private AccessProfileExtension extensionField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AccessPolicy", Order = 0)]
        public AccessPolicy[] AccessPolicy
        {
            get { return this.accessPolicyField; }
            set { this.accessPolicyField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public AccessProfileExtension Extension
        {
            get { return this.extensionField; }
            set { this.extensionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl")]
    public partial class AccessRulesServiceCapabilities
    {

        private System.Xml.XmlElement[] anyField;

        private uint maxLimitField;

        private uint maxAccessProfilesField;

        private uint maxAccessPoliciesPerAccessProfileField;

        private bool multipleSchedulesPerAccessPointSupportedField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get { return this.anyField; }
            set { this.anyField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxLimit
        {
            get { return this.maxLimitField; }
            set { this.maxLimitField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxAccessProfiles
        {
            get { return this.maxAccessProfilesField; }
            set { this.maxAccessProfilesField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint MaxAccessPoliciesPerAccessProfile
        {
            get { return this.maxAccessPoliciesPerAccessProfileField; }
            set { this.maxAccessPoliciesPerAccessProfileField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool MultipleSchedulesPerAccessPointSupported
        {
            get { return this.multipleSchedulesPerAccessPointSupportedField; }
            set { this.multipleSchedulesPerAccessPointSupportedField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get { return this.anyAttrField; }
            set { this.anyAttrField = value; }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
            ConfigurationName = "CredentialPort")]
    public interface CredentialPort
    {

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetServiceCapabilities", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Capabilities")]
        CredentialServiceCapabilities GetServiceCapabilities();

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetServiceCapabilities", ReplyAction = "*")]
        System.IAsyncResult BeginGetServiceCapabilities(System.AsyncCallback callback, object asyncState);

        [return: System.ServiceModel.MessageParameterAttribute(Name = "Capabilities")]
        CredentialServiceCapabilities EndGetServiceCapabilities(System.IAsyncResult result);

        // CODEGEN: Parameter 'FormatTypeInfo' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetSupportedFormatTypes", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "FormatTypeInfo")]
        GetSupportedFormatTypesResponse GetSupportedFormatTypes(GetSupportedFormatTypesRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetSupportedFormatTypes", ReplyAction = "*")]
        System.IAsyncResult BeginGetSupportedFormatTypes(GetSupportedFormatTypesRequest request,
                                                         System.AsyncCallback callback,
                                                         object asyncState);

        GetSupportedFormatTypesResponse EndGetSupportedFormatTypes(System.IAsyncResult result);

        // CODEGEN: Parameter 'CredentialInfo' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentialInfo", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "CredentialInfo")]
        GetCredentialInfoResponse GetCredentialInfo(GetCredentialInfoRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentialInfo", ReplyAction = "*")]
        System.IAsyncResult BeginGetCredentialInfo(GetCredentialInfoRequest request,
                                                   System.AsyncCallback callback,
                                                   object asyncState);

        GetCredentialInfoResponse EndGetCredentialInfo(System.IAsyncResult result);

        // CODEGEN: Parameter 'CredentialInfo' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentialInfoList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "NextStartReference")]
        GetCredentialInfoListResponse GetCredentialInfoList(GetCredentialInfoListRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentialInfoList", ReplyAction = "*")]
        System.IAsyncResult BeginGetCredentialInfoList(GetCredentialInfoListRequest request,
                                                       System.AsyncCallback callback,
                                                       object asyncState);

        GetCredentialInfoListResponse EndGetCredentialInfoList(System.IAsyncResult result);

        // CODEGEN: Parameter 'Credential' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentials", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Credential")]
        GetCredentialsResponse GetCredentials(GetCredentialsRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentials", ReplyAction = "*")]
        System.IAsyncResult BeginGetCredentials(GetCredentialsRequest request,
                                                System.AsyncCallback callback,
                                                object asyncState);

        GetCredentialsResponse EndGetCredentials(System.IAsyncResult result);

        // CODEGEN: Parameter 'Credential' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentialList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "NextStartReference")]
        GetCredentialListResponse GetCredentialList(GetCredentialListRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentialList", ReplyAction = "*")]
        System.IAsyncResult BeginGetCredentialList(GetCredentialListRequest request,
                                                   System.AsyncCallback callback,
                                                   object asyncState);

        GetCredentialListResponse EndGetCredentialList(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/CreateCredential", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Token")]
        string CreateCredential(Credential Credential, CredentialState State);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/CreateCredential", ReplyAction = "*")]
        System.IAsyncResult BeginCreateCredential(Credential Credential,
                                                  CredentialState State,
                                                  System.AsyncCallback callback,
                                                  object asyncState);

        [return: System.ServiceModel.MessageParameterAttribute(Name = "Token")]
        string EndCreateCredential(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/ModifyCredential", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void ModifyCredential(Credential Credential);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/ModifyCredential", ReplyAction = "*")]
        System.IAsyncResult BeginModifyCredential(Credential Credential,
                                                  System.AsyncCallback callback,
                                                  object asyncState);

        void EndModifyCredential(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/DeleteCredential", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void DeleteCredential(string Token);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/DeleteCredential", ReplyAction = "*")]
        System.IAsyncResult BeginDeleteCredential(string Token, System.AsyncCallback callback, object asyncState);

        void EndDeleteCredential(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentialState", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "State")]
        CredentialState GetCredentialState(string Token);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentialState", ReplyAction = "*")]
        System.IAsyncResult BeginGetCredentialState(string Token, System.AsyncCallback callback, object asyncState);

        [return: System.ServiceModel.MessageParameterAttribute(Name = "State")]
        CredentialState EndGetCredentialState(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/EnableCredential", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void EnableCredential(string Token, string Reason);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/EnableCredential", ReplyAction = "*")]
        System.IAsyncResult BeginEnableCredential(string Token,
                                                  string Reason,
                                                  System.AsyncCallback callback,
                                                  object asyncState);

        void EndEnableCredential(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/DisableCredential", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void DisableCredential(string Token, string Reason);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/DisableCredential", ReplyAction = "*")]
        System.IAsyncResult BeginDisableCredential(string Token,
                                                   string Reason,
                                                   System.AsyncCallback callback,
                                                   object asyncState);

        void EndDisableCredential(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/ResetAntipassbackViolation", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void ResetAntipassbackViolation(string CredentialToken);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/ResetAntipassbackViolation", ReplyAction = "*")]
        System.IAsyncResult BeginResetAntipassbackViolation(string CredentialToken,
                                                            System.AsyncCallback callback,
                                                            object asyncState);

        void EndResetAntipassbackViolation(System.IAsyncResult result);

        // CODEGEN: Parameter 'CredentialIdentifier' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentialIdentifiers", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "CredentialIdentifier")]
        GetCredentialIdentifiersResponse GetCredentialIdentifiers(GetCredentialIdentifiersRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentialIdentifiers", ReplyAction = "*")]
        System.IAsyncResult BeginGetCredentialIdentifiers(GetCredentialIdentifiersRequest request,
                                                          System.AsyncCallback callback,
                                                          object asyncState);

        GetCredentialIdentifiersResponse EndGetCredentialIdentifiers(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/SetCredentialIdentifier", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void SetCredentialIdentifier(string CredentialToken, CredentialIdentifier CredentialIdentifier);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/SetCredentialIdentifier", ReplyAction = "*")]
        System.IAsyncResult BeginSetCredentialIdentifier(string CredentialToken,
                                                         CredentialIdentifier CredentialIdentifier,
                                                         System.AsyncCallback callback,
                                                         object asyncState);

        void EndSetCredentialIdentifier(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/DeleteCredentialIdentifier", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void DeleteCredentialIdentifier(string CredentialToken, string CredentialIdentifierTypeName);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/DeleteCredentialIdentifier", ReplyAction = "*")]
        System.IAsyncResult BeginDeleteCredentialIdentifier(string CredentialToken,
                                                            string CredentialIdentifierTypeName,
                                                            System.AsyncCallback callback,
                                                            object asyncState);

        void EndDeleteCredentialIdentifier(System.IAsyncResult result);

        // CODEGEN: Parameter 'CredentialAccessProfile' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentialAccessProfiles", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "CredentialAccessProfile")]
        GetCredentialAccessProfilesResponse GetCredentialAccessProfiles(GetCredentialAccessProfilesRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/GetCredentialAccessProfiles", ReplyAction = "*")]
        System.IAsyncResult BeginGetCredentialAccessProfiles(GetCredentialAccessProfilesRequest request,
                                                             System.AsyncCallback callback,
                                                             object asyncState);

        GetCredentialAccessProfilesResponse EndGetCredentialAccessProfiles(System.IAsyncResult result);

        // CODEGEN: Parameter 'CredentialAccessProfile' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/SetCredentialAccessProfiles", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        SetCredentialAccessProfilesResponse SetCredentialAccessProfiles(SetCredentialAccessProfilesRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/SetCredentialAccessProfiles", ReplyAction = "*")]
        System.IAsyncResult BeginSetCredentialAccessProfiles(SetCredentialAccessProfilesRequest request,
                                                             System.AsyncCallback callback,
                                                             object asyncState);

        SetCredentialAccessProfilesResponse EndSetCredentialAccessProfiles(System.IAsyncResult result);

        // CODEGEN: Parameter 'AccessProfileToken' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/credential/wsdl/DeleteCredentialAccessProfiles", ReplyAction = "*")
        ]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        DeleteCredentialAccessProfilesResponse DeleteCredentialAccessProfiles(
                DeleteCredentialAccessProfilesRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/credential/wsdl/DeleteCredentialAccessProfiles", ReplyAction = "*")
        ]
        System.IAsyncResult BeginDeleteCredentialAccessProfiles(DeleteCredentialAccessProfilesRequest request,
                                                                System.AsyncCallback callback,
                                                                object asyncState);

        DeleteCredentialAccessProfilesResponse EndDeleteCredentialAccessProfiles(System.IAsyncResult result);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetSupportedFormatTypes",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetSupportedFormatTypesRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        public string CredentialIdentifierTypeName;

        public GetSupportedFormatTypesRequest()
        {
        }

        public GetSupportedFormatTypesRequest(string CredentialIdentifierTypeName)
        {
            this.CredentialIdentifierTypeName = CredentialIdentifierTypeName;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetSupportedFormatTypesResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetSupportedFormatTypesResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("FormatTypeInfo")]
        public
            CredentialIdentifierFormatTypeInfo[] FormatTypeInfo;

        public GetSupportedFormatTypesResponse()
        {
        }

        public GetSupportedFormatTypesResponse(CredentialIdentifierFormatTypeInfo[] FormatTypeInfo)
        {
            this.FormatTypeInfo = FormatTypeInfo;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetCredentialInfo",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetCredentialInfoRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Token")]
        public string[] Token;

        public GetCredentialInfoRequest()
        {
        }

        public GetCredentialInfoRequest(string[] Token)
        {
            this.Token = Token;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetCredentialInfoResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetCredentialInfoResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("CredentialInfo")]
        public CredentialInfo[]
            CredentialInfo;

        public GetCredentialInfoResponse()
        {
        }

        public GetCredentialInfoResponse(CredentialInfo[] CredentialInfo)
        {
            this.CredentialInfo = CredentialInfo;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetCredentialInfoList",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetCredentialInfoListRequest
    {

        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl", Order = 0)]
        public int? Limit;

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "Limit",
                Namespace = "http://www.onvif.org/ver10/credential/wsdl", Order = 0)]
        public string LimitSerialize
        {
            get { return Limit.HasValue ? Limit.Value.ToString() : null; }
        }


        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 1)]
        public string StartReference;

        public GetCredentialInfoListRequest()
        {
        }

        public GetCredentialInfoListRequest(int Limit, string StartReference)
        {
            this.Limit = Limit;
            this.StartReference = StartReference;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetCredentialInfoListResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetCredentialInfoListResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        public string NextStartReference;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("CredentialInfo")]
        public CredentialInfo[]
            CredentialInfo;

        public GetCredentialInfoListResponse()
        {
        }

        public GetCredentialInfoListResponse(string NextStartReference, CredentialInfo[] CredentialInfo)
        {
            this.NextStartReference = NextStartReference;
            this.CredentialInfo = CredentialInfo;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetCredentials",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetCredentialsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Token")]
        public string[] Token;

        public GetCredentialsRequest()
        {
        }

        public GetCredentialsRequest(string[] Token)
        {
            this.Token = Token;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetCredentialsResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetCredentialsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Credential")]
        public Credential[] Credential;

        public GetCredentialsResponse()
        {
        }

        public GetCredentialsResponse(Credential[] Credential)
        {
            this.Credential = Credential;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetCredentialList",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetCredentialListRequest
    {
        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl", Order = 0)]
        public int? Limit;

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "Limit",
                Namespace = "http://www.onvif.org/ver10/credential/wsdl", Order = 0)]
        public string LimitSerialize
        {
            get { return Limit.HasValue ? Limit.Value.ToString() : null; }
        }

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 1)]
        public string StartReference;

        public GetCredentialListRequest()
        {
        }

        public GetCredentialListRequest(int Limit, string StartReference)
        {
            this.Limit = Limit;
            this.StartReference = StartReference;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetCredentialListResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetCredentialListResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        public string NextStartReference;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Credential")]
        public Credential[] Credential;

        public GetCredentialListResponse()
        {
        }

        public GetCredentialListResponse(string NextStartReference, Credential[] Credential)
        {
            this.NextStartReference = NextStartReference;
            this.Credential = Credential;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetCredentialIdentifiers",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetCredentialIdentifiersRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        public string CredentialToken;

        public GetCredentialIdentifiersRequest()
        {
        }

        public GetCredentialIdentifiersRequest(string CredentialToken)
        {
            this.CredentialToken = CredentialToken;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetCredentialIdentifiersResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetCredentialIdentifiersResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("CredentialIdentifier")]
        public
            CredentialIdentifier[] CredentialIdentifier;

        public GetCredentialIdentifiersResponse()
        {
        }

        public GetCredentialIdentifiersResponse(CredentialIdentifier[] CredentialIdentifier)
        {
            this.CredentialIdentifier = CredentialIdentifier;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetCredentialAccessProfiles",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetCredentialAccessProfilesRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        public string CredentialToken;

        public GetCredentialAccessProfilesRequest()
        {
        }

        public GetCredentialAccessProfilesRequest(string CredentialToken)
        {
            this.CredentialToken = CredentialToken;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetCredentialAccessProfilesResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class GetCredentialAccessProfilesResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("CredentialAccessProfile")]
        public
            CredentialAccessProfile[] CredentialAccessProfile;

        public GetCredentialAccessProfilesResponse()
        {
        }

        public GetCredentialAccessProfilesResponse(CredentialAccessProfile[] CredentialAccessProfile)
        {
            this.CredentialAccessProfile = CredentialAccessProfile;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SetCredentialAccessProfiles",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class SetCredentialAccessProfilesRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        public string CredentialToken;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("CredentialAccessProfile")]
        public
            CredentialAccessProfile[] CredentialAccessProfile;

        public SetCredentialAccessProfilesRequest()
        {
        }

        public SetCredentialAccessProfilesRequest(string CredentialToken,
                                                  CredentialAccessProfile[] CredentialAccessProfile)
        {
            this.CredentialToken = CredentialToken;
            this.CredentialAccessProfile = CredentialAccessProfile;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SetCredentialAccessProfilesResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class SetCredentialAccessProfilesResponse
    {

        public SetCredentialAccessProfilesResponse()
        {
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "DeleteCredentialAccessProfiles",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class DeleteCredentialAccessProfilesRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 0)]
        public string CredentialToken;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/credential/wsdl",
                Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("AccessProfileToken")]
        public string[]
            AccessProfileToken;

        public DeleteCredentialAccessProfilesRequest()
        {
        }

        public DeleteCredentialAccessProfilesRequest(string CredentialToken, string[] AccessProfileToken)
        {
            this.CredentialToken = CredentialToken;
            this.AccessProfileToken = AccessProfileToken;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "DeleteCredentialAccessProfilesResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/credential/wsdl", IsWrapped = true)]
    public partial class DeleteCredentialAccessProfilesResponse
    {

        public DeleteCredentialAccessProfilesResponse()
        {
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CredentialPortChannel : CredentialPort, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CredentialPortClient : System.ServiceModel.ClientBase<CredentialPort>, CredentialPort
    {

        public CredentialPortClient()
        {
        }

        public CredentialPortClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public CredentialPortClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CredentialPortClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            :
                    base(endpointConfigurationName, remoteAddress)
        {
        }

        public CredentialPortClient(System.ServiceModel.Channels.Binding binding,
                                    System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public CredentialServiceCapabilities GetServiceCapabilities()
        {
            return base.Channel.GetServiceCapabilities();
        }

        public System.IAsyncResult BeginGetServiceCapabilities(System.AsyncCallback callback, object asyncState)
        {
            return base.Channel.BeginGetServiceCapabilities(callback, asyncState);
        }

        public CredentialServiceCapabilities EndGetServiceCapabilities(System.IAsyncResult result)
        {
            return base.Channel.EndGetServiceCapabilities(result);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetSupportedFormatTypesResponse CredentialPort.GetSupportedFormatTypes(GetSupportedFormatTypesRequest request)
        {
            return base.Channel.GetSupportedFormatTypes(request);
        }

        public CredentialIdentifierFormatTypeInfo[] GetSupportedFormatTypes(string CredentialIdentifierTypeName)
        {
            GetSupportedFormatTypesRequest inValue = new GetSupportedFormatTypesRequest();
            inValue.CredentialIdentifierTypeName = CredentialIdentifierTypeName;
            GetSupportedFormatTypesResponse retVal = ((CredentialPort)(this)).GetSupportedFormatTypes(inValue);
            return retVal.FormatTypeInfo;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult CredentialPort.BeginGetSupportedFormatTypes(GetSupportedFormatTypesRequest request,
                                                                        System.AsyncCallback callback,
                                                                        object asyncState)
        {
            return base.Channel.BeginGetSupportedFormatTypes(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetSupportedFormatTypes(string CredentialIdentifierTypeName,
                                                                System.AsyncCallback callback,
                                                                object asyncState)
        {
            GetSupportedFormatTypesRequest inValue = new GetSupportedFormatTypesRequest();
            inValue.CredentialIdentifierTypeName = CredentialIdentifierTypeName;
            return ((CredentialPort)(this)).BeginGetSupportedFormatTypes(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetSupportedFormatTypesResponse CredentialPort.EndGetSupportedFormatTypes(System.IAsyncResult result)
        {
            return base.Channel.EndGetSupportedFormatTypes(result);
        }

        public CredentialIdentifierFormatTypeInfo[] EndGetSupportedFormatTypes(System.IAsyncResult result)
        {
            GetSupportedFormatTypesResponse retVal = ((CredentialPort)(this)).EndGetSupportedFormatTypes(result);
            return retVal.FormatTypeInfo;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetCredentialInfoResponse CredentialPort.GetCredentialInfo(GetCredentialInfoRequest request)
        {
            return base.Channel.GetCredentialInfo(request);
        }

        public CredentialInfo[] GetCredentialInfo(string[] Token)
        {
            GetCredentialInfoRequest inValue = new GetCredentialInfoRequest();
            inValue.Token = Token;
            GetCredentialInfoResponse retVal = ((CredentialPort)(this)).GetCredentialInfo(inValue);
            return retVal.CredentialInfo;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult CredentialPort.BeginGetCredentialInfo(GetCredentialInfoRequest request,
                                                                  System.AsyncCallback callback,
                                                                  object asyncState)
        {
            return base.Channel.BeginGetCredentialInfo(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetCredentialInfo(string[] Token,
                                                          System.AsyncCallback callback,
                                                          object asyncState)
        {
            GetCredentialInfoRequest inValue = new GetCredentialInfoRequest();
            inValue.Token = Token;
            return ((CredentialPort)(this)).BeginGetCredentialInfo(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetCredentialInfoResponse CredentialPort.EndGetCredentialInfo(System.IAsyncResult result)
        {
            return base.Channel.EndGetCredentialInfo(result);
        }

        public CredentialInfo[] EndGetCredentialInfo(System.IAsyncResult result)
        {
            GetCredentialInfoResponse retVal = ((CredentialPort)(this)).EndGetCredentialInfo(result);
            return retVal.CredentialInfo;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetCredentialInfoListResponse CredentialPort.GetCredentialInfoList(GetCredentialInfoListRequest request)
        {
            return base.Channel.GetCredentialInfoList(request);
        }

        public string GetCredentialInfoList(int? Limit, string StartReference, out CredentialInfo[] CredentialInfo)
        {
            GetCredentialInfoListRequest inValue = new GetCredentialInfoListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            GetCredentialInfoListResponse retVal = ((CredentialPort)(this)).GetCredentialInfoList(inValue);
            CredentialInfo = retVal.CredentialInfo;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult CredentialPort.BeginGetCredentialInfoList(GetCredentialInfoListRequest request,
                                                                      System.AsyncCallback callback,
                                                                      object asyncState)
        {
            return base.Channel.BeginGetCredentialInfoList(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetCredentialInfoList(int Limit,
                                                              string StartReference,
                                                              System.AsyncCallback callback,
                                                              object asyncState)
        {
            GetCredentialInfoListRequest inValue = new GetCredentialInfoListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            return ((CredentialPort)(this)).BeginGetCredentialInfoList(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetCredentialInfoListResponse CredentialPort.EndGetCredentialInfoList(System.IAsyncResult result)
        {
            return base.Channel.EndGetCredentialInfoList(result);
        }

        public string EndGetCredentialInfoList(System.IAsyncResult result, out CredentialInfo[] CredentialInfo)
        {
            GetCredentialInfoListResponse retVal = ((CredentialPort)(this)).EndGetCredentialInfoList(result);
            CredentialInfo = retVal.CredentialInfo;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetCredentialsResponse CredentialPort.GetCredentials(GetCredentialsRequest request)
        {
            return base.Channel.GetCredentials(request);
        }

        public Credential[] GetCredentials(string[] Token)
        {
            GetCredentialsRequest inValue = new GetCredentialsRequest();
            inValue.Token = Token;
            GetCredentialsResponse retVal = ((CredentialPort)(this)).GetCredentials(inValue);
            return retVal.Credential;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult CredentialPort.BeginGetCredentials(GetCredentialsRequest request,
                                                               System.AsyncCallback callback,
                                                               object asyncState)
        {
            return base.Channel.BeginGetCredentials(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetCredentials(string[] Token, System.AsyncCallback callback, object asyncState)
        {
            GetCredentialsRequest inValue = new GetCredentialsRequest();
            inValue.Token = Token;
            return ((CredentialPort)(this)).BeginGetCredentials(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetCredentialsResponse CredentialPort.EndGetCredentials(System.IAsyncResult result)
        {
            return base.Channel.EndGetCredentials(result);
        }

        public Credential[] EndGetCredentials(System.IAsyncResult result)
        {
            GetCredentialsResponse retVal = ((CredentialPort)(this)).EndGetCredentials(result);
            return retVal.Credential;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetCredentialListResponse CredentialPort.GetCredentialList(GetCredentialListRequest request)
        {
            return base.Channel.GetCredentialList(request);
        }

        public string GetCredentialList(int? Limit, string StartReference, out Credential[] Credential)
        {
            GetCredentialListRequest inValue = new GetCredentialListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            GetCredentialListResponse retVal = ((CredentialPort)(this)).GetCredentialList(inValue);
            Credential = retVal.Credential;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult CredentialPort.BeginGetCredentialList(GetCredentialListRequest request,
                                                                  System.AsyncCallback callback,
                                                                  object asyncState)
        {
            return base.Channel.BeginGetCredentialList(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetCredentialList(int Limit,
                                                          string StartReference,
                                                          System.AsyncCallback callback,
                                                          object asyncState)
        {
            GetCredentialListRequest inValue = new GetCredentialListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            return ((CredentialPort)(this)).BeginGetCredentialList(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetCredentialListResponse CredentialPort.EndGetCredentialList(System.IAsyncResult result)
        {
            return base.Channel.EndGetCredentialList(result);
        }

        public string EndGetCredentialList(System.IAsyncResult result, out Credential[] Credential)
        {
            GetCredentialListResponse retVal = ((CredentialPort)(this)).EndGetCredentialList(result);
            Credential = retVal.Credential;
            return retVal.NextStartReference;
        }

        public string CreateCredential(Credential Credential, CredentialState State)
        {
            return base.Channel.CreateCredential(Credential, State);
        }

        public System.IAsyncResult BeginCreateCredential(Credential Credential,
                                                         CredentialState State,
                                                         System.AsyncCallback callback,
                                                         object asyncState)
        {
            return base.Channel.BeginCreateCredential(Credential, State, callback, asyncState);
        }

        public string EndCreateCredential(System.IAsyncResult result)
        {
            return base.Channel.EndCreateCredential(result);
        }

        public void ModifyCredential(Credential Credential)
        {
            base.Channel.ModifyCredential(Credential);
        }

        public System.IAsyncResult BeginModifyCredential(Credential Credential,
                                                         System.AsyncCallback callback,
                                                         object asyncState)
        {
            return base.Channel.BeginModifyCredential(Credential, callback, asyncState);
        }

        public void EndModifyCredential(System.IAsyncResult result)
        {
            base.Channel.EndModifyCredential(result);
        }

        public void DeleteCredential(string Token)
        {
            base.Channel.DeleteCredential(Token);
        }

        public System.IAsyncResult BeginDeleteCredential(string Token, System.AsyncCallback callback, object asyncState)
        {
            return base.Channel.BeginDeleteCredential(Token, callback, asyncState);
        }

        public void EndDeleteCredential(System.IAsyncResult result)
        {
            base.Channel.EndDeleteCredential(result);
        }

        public CredentialState GetCredentialState(string Token)
        {
            return base.Channel.GetCredentialState(Token);
        }

        public System.IAsyncResult BeginGetCredentialState(string Token,
                                                           System.AsyncCallback callback,
                                                           object asyncState)
        {
            return base.Channel.BeginGetCredentialState(Token, callback, asyncState);
        }

        public CredentialState EndGetCredentialState(System.IAsyncResult result)
        {
            return base.Channel.EndGetCredentialState(result);
        }

        public void EnableCredential(string Token, string Reason)
        {
            base.Channel.EnableCredential(Token, Reason);
        }

        public System.IAsyncResult BeginEnableCredential(string Token,
                                                         string Reason,
                                                         System.AsyncCallback callback,
                                                         object asyncState)
        {
            return base.Channel.BeginEnableCredential(Token, Reason, callback, asyncState);
        }

        public void EndEnableCredential(System.IAsyncResult result)
        {
            base.Channel.EndEnableCredential(result);
        }

        public void DisableCredential(string Token, string Reason)
        {
            base.Channel.DisableCredential(Token, Reason);
        }

        public System.IAsyncResult BeginDisableCredential(string Token,
                                                          string Reason,
                                                          System.AsyncCallback callback,
                                                          object asyncState)
        {
            return base.Channel.BeginDisableCredential(Token, Reason, callback, asyncState);
        }

        public void EndDisableCredential(System.IAsyncResult result)
        {
            base.Channel.EndDisableCredential(result);
        }

        public void ResetAntipassbackViolation(string CredentialToken)
        {
            base.Channel.ResetAntipassbackViolation(CredentialToken);
        }

        public System.IAsyncResult BeginResetAntipassbackViolation(string CredentialToken,
                                                                   System.AsyncCallback callback,
                                                                   object asyncState)
        {
            return base.Channel.BeginResetAntipassbackViolation(CredentialToken, callback, asyncState);
        }

        public void EndResetAntipassbackViolation(System.IAsyncResult result)
        {
            base.Channel.EndResetAntipassbackViolation(result);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetCredentialIdentifiersResponse CredentialPort.GetCredentialIdentifiers(GetCredentialIdentifiersRequest request)
        {
            return base.Channel.GetCredentialIdentifiers(request);
        }

        public CredentialIdentifier[] GetCredentialIdentifiers(string CredentialToken)
        {
            GetCredentialIdentifiersRequest inValue = new GetCredentialIdentifiersRequest();
            inValue.CredentialToken = CredentialToken;
            GetCredentialIdentifiersResponse retVal = ((CredentialPort)(this)).GetCredentialIdentifiers(inValue);
            return retVal.CredentialIdentifier;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult CredentialPort.BeginGetCredentialIdentifiers(GetCredentialIdentifiersRequest request,
                                                                         System.AsyncCallback callback,
                                                                         object asyncState)
        {
            return base.Channel.BeginGetCredentialIdentifiers(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetCredentialIdentifiers(string CredentialToken,
                                                                 System.AsyncCallback callback,
                                                                 object asyncState)
        {
            GetCredentialIdentifiersRequest inValue = new GetCredentialIdentifiersRequest();
            inValue.CredentialToken = CredentialToken;
            return ((CredentialPort)(this)).BeginGetCredentialIdentifiers(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetCredentialIdentifiersResponse CredentialPort.EndGetCredentialIdentifiers(System.IAsyncResult result)
        {
            return base.Channel.EndGetCredentialIdentifiers(result);
        }

        public CredentialIdentifier[] EndGetCredentialIdentifiers(System.IAsyncResult result)
        {
            GetCredentialIdentifiersResponse retVal = ((CredentialPort)(this)).EndGetCredentialIdentifiers(result);
            return retVal.CredentialIdentifier;
        }

        public void SetCredentialIdentifier(string CredentialToken, CredentialIdentifier CredentialIdentifier)
        {
            base.Channel.SetCredentialIdentifier(CredentialToken, CredentialIdentifier);
        }

        public System.IAsyncResult BeginSetCredentialIdentifier(string CredentialToken,
                                                                CredentialIdentifier CredentialIdentifier,
                                                                System.AsyncCallback callback,
                                                                object asyncState)
        {
            return base.Channel.BeginSetCredentialIdentifier(CredentialToken, CredentialIdentifier, callback, asyncState);
        }

        public void EndSetCredentialIdentifier(System.IAsyncResult result)
        {
            base.Channel.EndSetCredentialIdentifier(result);
        }

        public void DeleteCredentialIdentifier(string CredentialToken, string CredentialIdentifierTypeName)
        {
            base.Channel.DeleteCredentialIdentifier(CredentialToken, CredentialIdentifierTypeName);
        }

        public System.IAsyncResult BeginDeleteCredentialIdentifier(string CredentialToken,
                                                                   string CredentialIdentifierTypeName,
                                                                   System.AsyncCallback callback,
                                                                   object asyncState)
        {
            return base.Channel.BeginDeleteCredentialIdentifier(CredentialToken,
                                                                CredentialIdentifierTypeName,
                                                                callback,
                                                                asyncState);
        }

        public void EndDeleteCredentialIdentifier(System.IAsyncResult result)
        {
            base.Channel.EndDeleteCredentialIdentifier(result);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetCredentialAccessProfilesResponse CredentialPort.GetCredentialAccessProfiles(
                GetCredentialAccessProfilesRequest request)
        {
            return base.Channel.GetCredentialAccessProfiles(request);
        }

        public CredentialAccessProfile[] GetCredentialAccessProfiles(string CredentialToken)
        {
            GetCredentialAccessProfilesRequest inValue = new GetCredentialAccessProfilesRequest();
            inValue.CredentialToken = CredentialToken;
            GetCredentialAccessProfilesResponse retVal = ((CredentialPort)(this)).GetCredentialAccessProfiles(inValue);
            return retVal.CredentialAccessProfile;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult CredentialPort.BeginGetCredentialAccessProfiles(GetCredentialAccessProfilesRequest request,
                                                                            System.AsyncCallback callback,
                                                                            object asyncState)
        {
            return base.Channel.BeginGetCredentialAccessProfiles(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetCredentialAccessProfiles(string CredentialToken,
                                                                    System.AsyncCallback callback,
                                                                    object asyncState)
        {
            GetCredentialAccessProfilesRequest inValue = new GetCredentialAccessProfilesRequest();
            inValue.CredentialToken = CredentialToken;
            return ((CredentialPort)(this)).BeginGetCredentialAccessProfiles(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetCredentialAccessProfilesResponse CredentialPort.EndGetCredentialAccessProfiles(System.IAsyncResult result)
        {
            return base.Channel.EndGetCredentialAccessProfiles(result);
        }

        public CredentialAccessProfile[] EndGetCredentialAccessProfiles(System.IAsyncResult result)
        {
            GetCredentialAccessProfilesResponse retVal = ((CredentialPort)(this)).EndGetCredentialAccessProfiles(result);
            return retVal.CredentialAccessProfile;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SetCredentialAccessProfilesResponse CredentialPort.SetCredentialAccessProfiles(
                SetCredentialAccessProfilesRequest request)
        {
            return base.Channel.SetCredentialAccessProfiles(request);
        }

        public void SetCredentialAccessProfiles(string CredentialToken,
                                                CredentialAccessProfile[] CredentialAccessProfile)
        {
            SetCredentialAccessProfilesRequest inValue = new SetCredentialAccessProfilesRequest();
            inValue.CredentialToken = CredentialToken;
            inValue.CredentialAccessProfile = CredentialAccessProfile;
            SetCredentialAccessProfilesResponse retVal = ((CredentialPort)(this)).SetCredentialAccessProfiles(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult CredentialPort.BeginSetCredentialAccessProfiles(SetCredentialAccessProfilesRequest request,
                                                                            System.AsyncCallback callback,
                                                                            object asyncState)
        {
            return base.Channel.BeginSetCredentialAccessProfiles(request, callback, asyncState);
        }

        public System.IAsyncResult BeginSetCredentialAccessProfiles(string CredentialToken,
                                                                    CredentialAccessProfile[] CredentialAccessProfile,
                                                                    System.AsyncCallback callback,
                                                                    object asyncState)
        {
            SetCredentialAccessProfilesRequest inValue = new SetCredentialAccessProfilesRequest();
            inValue.CredentialToken = CredentialToken;
            inValue.CredentialAccessProfile = CredentialAccessProfile;
            return ((CredentialPort)(this)).BeginSetCredentialAccessProfiles(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SetCredentialAccessProfilesResponse CredentialPort.EndSetCredentialAccessProfiles(System.IAsyncResult result)
        {
            return base.Channel.EndSetCredentialAccessProfiles(result);
        }

        public void EndSetCredentialAccessProfiles(System.IAsyncResult result)
        {
            SetCredentialAccessProfilesResponse retVal = ((CredentialPort)(this)).EndSetCredentialAccessProfiles(result);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DeleteCredentialAccessProfilesResponse CredentialPort.DeleteCredentialAccessProfiles(
                DeleteCredentialAccessProfilesRequest request)
        {
            return base.Channel.DeleteCredentialAccessProfiles(request);
        }

        public void DeleteCredentialAccessProfiles(string CredentialToken, string[] AccessProfileToken)
        {
            DeleteCredentialAccessProfilesRequest inValue = new DeleteCredentialAccessProfilesRequest();
            inValue.CredentialToken = CredentialToken;
            inValue.AccessProfileToken = AccessProfileToken;
            DeleteCredentialAccessProfilesResponse retVal =
                    ((CredentialPort)(this)).DeleteCredentialAccessProfiles(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult CredentialPort.BeginDeleteCredentialAccessProfiles(
                DeleteCredentialAccessProfilesRequest request, System.AsyncCallback callback, object asyncState)
        {
            return base.Channel.BeginDeleteCredentialAccessProfiles(request, callback, asyncState);
        }

        public System.IAsyncResult BeginDeleteCredentialAccessProfiles(string CredentialToken,
                                                                       string[] AccessProfileToken,
                                                                       System.AsyncCallback callback,
                                                                       object asyncState)
        {
            DeleteCredentialAccessProfilesRequest inValue = new DeleteCredentialAccessProfilesRequest();
            inValue.CredentialToken = CredentialToken;
            inValue.AccessProfileToken = AccessProfileToken;
            return ((CredentialPort)(this)).BeginDeleteCredentialAccessProfiles(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DeleteCredentialAccessProfilesResponse CredentialPort.EndDeleteCredentialAccessProfiles(
                System.IAsyncResult result)
        {
            return base.Channel.EndDeleteCredentialAccessProfiles(result);
        }

        public void EndDeleteCredentialAccessProfiles(System.IAsyncResult result)
        {
            DeleteCredentialAccessProfilesResponse retVal =
                    ((CredentialPort)(this)).EndDeleteCredentialAccessProfiles(result);
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
            ConfigurationName = "SchedulePort")]
    public interface SchedulePort
    {

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetServiceCapabilities", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Capabilities")]
        ScheduleServiceCapabilities GetServiceCapabilities();

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetServiceCapabilities", ReplyAction = "*")]
        System.IAsyncResult BeginGetServiceCapabilities(System.AsyncCallback callback, object asyncState);

        [return: System.ServiceModel.MessageParameterAttribute(Name = "Capabilities")]
        ScheduleServiceCapabilities EndGetServiceCapabilities(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetScheduleState", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "ScheduleState")]
        ScheduleState GetScheduleState(string Token);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetScheduleState", ReplyAction = "*")]
        System.IAsyncResult BeginGetScheduleState(string Token, System.AsyncCallback callback, object asyncState);

        [return: System.ServiceModel.MessageParameterAttribute(Name = "ScheduleState")]
        ScheduleState EndGetScheduleState(System.IAsyncResult result);

        // CODEGEN: Parameter 'ScheduleInfo' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetScheduleInfo", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "ScheduleInfo")]
        GetScheduleInfoResponse GetScheduleInfo(GetScheduleInfoRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetScheduleInfo", ReplyAction = "*")]
        System.IAsyncResult BeginGetScheduleInfo(GetScheduleInfoRequest request,
                                                 System.AsyncCallback callback,
                                                 object asyncState);

        GetScheduleInfoResponse EndGetScheduleInfo(System.IAsyncResult result);

        // CODEGEN: Parameter 'ScheduleInfo' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetScheduleInfoList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "NextStartReference")]
        GetScheduleInfoListResponse GetScheduleInfoList(GetScheduleInfoListRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetScheduleInfoList", ReplyAction = "*")]
        System.IAsyncResult BeginGetScheduleInfoList(GetScheduleInfoListRequest request,
                                                     System.AsyncCallback callback,
                                                     object asyncState);

        GetScheduleInfoListResponse EndGetScheduleInfoList(System.IAsyncResult result);

        // CODEGEN: Parameter 'Schedule' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/ver10/schedule/wsdl/GetSchedules"
                , ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Schedule")]
        GetSchedulesResponse GetSchedules(GetSchedulesRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetSchedules", ReplyAction = "*")]
        System.IAsyncResult BeginGetSchedules(GetSchedulesRequest request,
                                              System.AsyncCallback callback,
                                              object asyncState);

        GetSchedulesResponse EndGetSchedules(System.IAsyncResult result);

        // CODEGEN: Parameter 'Schedule' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetScheduleList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "NextStartReference")]
        GetScheduleListResponse GetScheduleList(GetScheduleListRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetScheduleList", ReplyAction = "*")]
        System.IAsyncResult BeginGetScheduleList(GetScheduleListRequest request,
                                                 System.AsyncCallback callback,
                                                 object asyncState);

        GetScheduleListResponse EndGetScheduleList(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/CreateSchedule", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Token")]
        string CreateSchedule(Schedule Schedule);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/CreateSchedule", ReplyAction = "*")]
        System.IAsyncResult BeginCreateSchedule(Schedule Schedule, System.AsyncCallback callback, object asyncState);

        [return: System.ServiceModel.MessageParameterAttribute(Name = "Token")]
        string EndCreateSchedule(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/ModifySchedule", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void ModifySchedule(Schedule Schedule);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/ModifySchedule", ReplyAction = "*")]
        System.IAsyncResult BeginModifySchedule(Schedule Schedule, System.AsyncCallback callback, object asyncState);

        void EndModifySchedule(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/DeleteSchedule", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void DeleteSchedule(string Token);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/DeleteSchedule", ReplyAction = "*")]
        System.IAsyncResult BeginDeleteSchedule(string Token, System.AsyncCallback callback, object asyncState);

        void EndDeleteSchedule(System.IAsyncResult result);

        // CODEGEN: Parameter 'SpecialDayGroupInfo' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroupInfo", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "SpecialDayGroupInfo")]
        GetSpecialDayGroupInfoResponse GetSpecialDayGroupInfo(GetSpecialDayGroupInfoRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroupInfo", ReplyAction = "*")]
        System.IAsyncResult BeginGetSpecialDayGroupInfo(GetSpecialDayGroupInfoRequest request,
                                                        System.AsyncCallback callback,
                                                        object asyncState);

        GetSpecialDayGroupInfoResponse EndGetSpecialDayGroupInfo(System.IAsyncResult result);

        // CODEGEN: Parameter 'SpecialDayGroupInfo' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroupInfoList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "NextStartReference")]
        GetSpecialDayGroupInfoListResponse GetSpecialDayGroupInfoList(GetSpecialDayGroupInfoListRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroupInfoList", ReplyAction = "*")]
        System.IAsyncResult BeginGetSpecialDayGroupInfoList(GetSpecialDayGroupInfoListRequest request,
                                                            System.AsyncCallback callback,
                                                            object asyncState);

        GetSpecialDayGroupInfoListResponse EndGetSpecialDayGroupInfoList(System.IAsyncResult result);

        // CODEGEN: Parameter 'SpecialDayGroup' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroups", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "SpecialDayGroup")]
        GetSpecialDayGroupsResponse GetSpecialDayGroups(GetSpecialDayGroupsRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroups", ReplyAction = "*")]
        System.IAsyncResult BeginGetSpecialDayGroups(GetSpecialDayGroupsRequest request,
                                                     System.AsyncCallback callback,
                                                     object asyncState);

        GetSpecialDayGroupsResponse EndGetSpecialDayGroups(System.IAsyncResult result);

        // CODEGEN: Parameter 'SpecialDayGroup' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroupList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "NextStartReference")]
        GetSpecialDayGroupListResponse GetSpecialDayGroupList(GetSpecialDayGroupListRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroupList", ReplyAction = "*")]
        System.IAsyncResult BeginGetSpecialDayGroupList(GetSpecialDayGroupListRequest request,
                                                        System.AsyncCallback callback,
                                                        object asyncState);

        GetSpecialDayGroupListResponse EndGetSpecialDayGroupList(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/CreateSpecialDayGroup", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Token")]
        string CreateSpecialDayGroup(SpecialDayGroup SpecialDayGroup);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/CreateSpecialDayGroup", ReplyAction = "*")]
        System.IAsyncResult BeginCreateSpecialDayGroup(SpecialDayGroup SpecialDayGroup,
                                                       System.AsyncCallback callback,
                                                       object asyncState);

        [return: System.ServiceModel.MessageParameterAttribute(Name = "Token")]
        string EndCreateSpecialDayGroup(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/ModifySpecialDayGroup", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void ModifySpecialDayGroup(SpecialDayGroup SpecialDayGroup);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/ModifySpecialDayGroup", ReplyAction = "*")]
        System.IAsyncResult BeginModifySpecialDayGroup(SpecialDayGroup SpecialDayGroup,
                                                       System.AsyncCallback callback,
                                                       object asyncState);

        void EndModifySpecialDayGroup(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/schedule/wsdl/DeleteSpecialDayGroup", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void DeleteSpecialDayGroup(string Token);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/schedule/wsdl/DeleteSpecialDayGroup", ReplyAction = "*")]
        System.IAsyncResult BeginDeleteSpecialDayGroup(string Token, System.AsyncCallback callback, object asyncState);

        void EndDeleteSpecialDayGroup(System.IAsyncResult result);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetScheduleInfo",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetScheduleInfoRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Token")]
        public string[] Token;

        public GetScheduleInfoRequest()
        {
        }

        public GetScheduleInfoRequest(string[] Token)
        {
            this.Token = Token;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetScheduleInfoResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetScheduleInfoResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ScheduleInfo")]
        public ScheduleInfo[]
            ScheduleInfo;

        public GetScheduleInfoResponse()
        {
        }

        public GetScheduleInfoResponse(ScheduleInfo[] ScheduleInfo)
        {
            this.ScheduleInfo = ScheduleInfo;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetScheduleInfoList",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetScheduleInfoListRequest
    {
        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl", Order = 0)] 
        public int? Limit;

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "Limit", Namespace = "http://www.onvif.org/ver10/schedule/wsdl", Order = 0)]
        public string LimitSerialize
        {
            get { return Limit.HasValue ? Limit.Value.ToString() : null; }
        }

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 1)]
        public string StartReference;

        public GetScheduleInfoListRequest()
        {
        }

        public GetScheduleInfoListRequest(int? Limit, string StartReference)
        {
            this.Limit = Limit;
            this.StartReference = StartReference;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetScheduleInfoListResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetScheduleInfoListResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 0)]
        public string NextStartReference;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ScheduleInfo")]
        public ScheduleInfo[]
            ScheduleInfo;

        public GetScheduleInfoListResponse()
        {
        }

        public GetScheduleInfoListResponse(string NextStartReference, ScheduleInfo[] ScheduleInfo)
        {
            this.NextStartReference = NextStartReference;
            this.ScheduleInfo = ScheduleInfo;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetSchedules",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetSchedulesRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Token")]
        public string[] Token;

        public GetSchedulesRequest()
        {
        }

        public GetSchedulesRequest(string[] Token)
        {
            this.Token = Token;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetSchedulesResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetSchedulesResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Schedule")]
        public Schedule[] Schedule;

        public GetSchedulesResponse()
        {
        }

        public GetSchedulesResponse(Schedule[] Schedule)
        {
            this.Schedule = Schedule;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetScheduleList",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetScheduleListRequest
    {
        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl", Order = 0)] 
        public int? Limit;

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "Limit", Namespace = "http://www.onvif.org/ver10/schedule/wsdl", Order = 0)]
        public string LimitSerialize
        {
            get { return Limit.HasValue ? Limit.Value.ToString() : null; }
        }

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 1)]
        public string StartReference;

        public GetScheduleListRequest()
        {
        }

        public GetScheduleListRequest(int? Limit, string StartReference)
        {
            this.Limit = Limit;
            this.StartReference = StartReference;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetScheduleListResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetScheduleListResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 0)]
        public string NextStartReference;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Schedule")]
        public Schedule[] Schedule;

        public GetScheduleListResponse()
        {
        }

        public GetScheduleListResponse(string NextStartReference, Schedule[] Schedule)
        {
            this.NextStartReference = NextStartReference;
            this.Schedule = Schedule;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetSpecialDayGroupInfo",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetSpecialDayGroupInfoRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Token")]
        public string[] Token;

        public GetSpecialDayGroupInfoRequest()
        {
        }

        public GetSpecialDayGroupInfoRequest(string[] Token)
        {
            this.Token = Token;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetSpecialDayGroupInfoResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetSpecialDayGroupInfoResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SpecialDayGroupInfo")]
        public
            SpecialDayGroupInfo[] SpecialDayGroupInfo;

        public GetSpecialDayGroupInfoResponse()
        {
        }

        public GetSpecialDayGroupInfoResponse(SpecialDayGroupInfo[] SpecialDayGroupInfo)
        {
            this.SpecialDayGroupInfo = SpecialDayGroupInfo;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetSpecialDayGroupInfoList",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetSpecialDayGroupInfoListRequest
    {

        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl", Order = 0)] 
        public int? Limit;

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "Limit", Namespace = "http://www.onvif.org/ver10/schedule/wsdl", Order = 0)]
        public string LimitSerialize
        {
            get { return Limit.HasValue ? Limit.Value.ToString() : null; }
        }

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 1)]
        public string StartReference;

        public GetSpecialDayGroupInfoListRequest()
        {
        }

        public GetSpecialDayGroupInfoListRequest(int? Limit, string StartReference)
        {
            this.Limit = Limit;
            this.StartReference = StartReference;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetSpecialDayGroupInfoListResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetSpecialDayGroupInfoListResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 0)]
        public string NextStartReference;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("SpecialDayGroupInfo")]
        public
            SpecialDayGroupInfo[] SpecialDayGroupInfo;

        public GetSpecialDayGroupInfoListResponse()
        {
        }

        public GetSpecialDayGroupInfoListResponse(string NextStartReference, SpecialDayGroupInfo[] SpecialDayGroupInfo)
        {
            this.NextStartReference = NextStartReference;
            this.SpecialDayGroupInfo = SpecialDayGroupInfo;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetSpecialDayGroups",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetSpecialDayGroupsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Token")]
        public string[] Token;

        public GetSpecialDayGroupsRequest()
        {
        }

        public GetSpecialDayGroupsRequest(string[] Token)
        {
            this.Token = Token;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetSpecialDayGroupsResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetSpecialDayGroupsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SpecialDayGroup")]
        public SpecialDayGroup[]
            SpecialDayGroup;

        public GetSpecialDayGroupsResponse()
        {
        }

        public GetSpecialDayGroupsResponse(SpecialDayGroup[] SpecialDayGroup)
        {
            this.SpecialDayGroup = SpecialDayGroup;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetSpecialDayGroupList",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetSpecialDayGroupListRequest
    {
        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl", Order = 0)] 
        public int? Limit;

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "Limit", Namespace = "http://www.onvif.org/ver10/schedule/wsdl", Order = 0)]
        public string LimitSerialize
        {
            get { return Limit.HasValue ? Limit.Value.ToString() : null; }
        }

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 1)]
        public string StartReference;

        public GetSpecialDayGroupListRequest()
        {
        }

        public GetSpecialDayGroupListRequest(int? Limit, string StartReference)
        {
            this.Limit = Limit;
            this.StartReference = StartReference;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetSpecialDayGroupListResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/schedule/wsdl", IsWrapped = true)]
    public partial class GetSpecialDayGroupListResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 0)]
        public string NextStartReference;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl",
                Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("SpecialDayGroup")]
        public SpecialDayGroup[]
            SpecialDayGroup;

        public GetSpecialDayGroupListResponse()
        {
        }

        public GetSpecialDayGroupListResponse(string NextStartReference, SpecialDayGroup[] SpecialDayGroup)
        {
            this.NextStartReference = NextStartReference;
            this.SpecialDayGroup = SpecialDayGroup;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SchedulePortChannel : SchedulePort, System.ServiceModel.IClientChannel
    {
    }

    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SchedulePortClient : System.ServiceModel.ClientBase<SchedulePort>, SchedulePort
    {

        public SchedulePortClient()
        {
        }

        public SchedulePortClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public SchedulePortClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public SchedulePortClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public SchedulePortClient(System.ServiceModel.Channels.Binding binding,
                                  System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public ScheduleServiceCapabilities GetServiceCapabilities()
        {
            return base.Channel.GetServiceCapabilities();
        }

        public System.IAsyncResult BeginGetServiceCapabilities(System.AsyncCallback callback, object asyncState)
        {
            return base.Channel.BeginGetServiceCapabilities(callback, asyncState);
        }

        public ScheduleServiceCapabilities EndGetServiceCapabilities(System.IAsyncResult result)
        {
            return base.Channel.EndGetServiceCapabilities(result);
        }

        public ScheduleState GetScheduleState(string Token)
        {
            return base.Channel.GetScheduleState(Token);
        }

        public System.IAsyncResult BeginGetScheduleState(string Token, System.AsyncCallback callback, object asyncState)
        {
            return base.Channel.BeginGetScheduleState(Token, callback, asyncState);
        }

        public ScheduleState EndGetScheduleState(System.IAsyncResult result)
        {
            return base.Channel.EndGetScheduleState(result);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetScheduleInfoResponse SchedulePort.GetScheduleInfo(GetScheduleInfoRequest request)
        {
            return base.Channel.GetScheduleInfo(request);
        }

        public ScheduleInfo[] GetScheduleInfo(string[] Token)
        {
            GetScheduleInfoRequest inValue = new GetScheduleInfoRequest();
            inValue.Token = Token;
            GetScheduleInfoResponse retVal = ((SchedulePort)(this)).GetScheduleInfo(inValue);
            return retVal.ScheduleInfo;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SchedulePort.BeginGetScheduleInfo(GetScheduleInfoRequest request,
                                                              System.AsyncCallback callback,
                                                              object asyncState)
        {
            return base.Channel.BeginGetScheduleInfo(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetScheduleInfo(string[] Token, System.AsyncCallback callback, object asyncState)
        {
            GetScheduleInfoRequest inValue = new GetScheduleInfoRequest();
            inValue.Token = Token;
            return ((SchedulePort)(this)).BeginGetScheduleInfo(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetScheduleInfoResponse SchedulePort.EndGetScheduleInfo(System.IAsyncResult result)
        {
            return base.Channel.EndGetScheduleInfo(result);
        }

        public ScheduleInfo[] EndGetScheduleInfo(System.IAsyncResult result)
        {
            GetScheduleInfoResponse retVal = ((SchedulePort)(this)).EndGetScheduleInfo(result);
            return retVal.ScheduleInfo;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetScheduleInfoListResponse SchedulePort.GetScheduleInfoList(GetScheduleInfoListRequest request)
        {
            return base.Channel.GetScheduleInfoList(request);
        }

        public string GetScheduleInfoList(int? Limit, string StartReference, out ScheduleInfo[] ScheduleInfo)
        {
            GetScheduleInfoListRequest inValue = new GetScheduleInfoListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            GetScheduleInfoListResponse retVal = ((SchedulePort)(this)).GetScheduleInfoList(inValue);
            ScheduleInfo = retVal.ScheduleInfo;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SchedulePort.BeginGetScheduleInfoList(GetScheduleInfoListRequest request,
                                                                  System.AsyncCallback callback,
                                                                  object asyncState)
        {
            return base.Channel.BeginGetScheduleInfoList(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetScheduleInfoList(int? Limit,
                                                            string StartReference,
                                                            System.AsyncCallback callback,
                                                            object asyncState)
        {
            GetScheduleInfoListRequest inValue = new GetScheduleInfoListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            return ((SchedulePort)(this)).BeginGetScheduleInfoList(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetScheduleInfoListResponse SchedulePort.EndGetScheduleInfoList(System.IAsyncResult result)
        {
            return base.Channel.EndGetScheduleInfoList(result);
        }

        public string EndGetScheduleInfoList(System.IAsyncResult result, out ScheduleInfo[] ScheduleInfo)
        {
            GetScheduleInfoListResponse retVal = ((SchedulePort)(this)).EndGetScheduleInfoList(result);
            ScheduleInfo = retVal.ScheduleInfo;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetSchedulesResponse SchedulePort.GetSchedules(GetSchedulesRequest request)
        {
            return base.Channel.GetSchedules(request);
        }

        public Schedule[] GetSchedules(string[] Token)
        {
            GetSchedulesRequest inValue = new GetSchedulesRequest();
            inValue.Token = Token;
            GetSchedulesResponse retVal = ((SchedulePort)(this)).GetSchedules(inValue);
            return retVal.Schedule;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SchedulePort.BeginGetSchedules(GetSchedulesRequest request,
                                                           System.AsyncCallback callback,
                                                           object asyncState)
        {
            return base.Channel.BeginGetSchedules(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetSchedules(string[] Token, System.AsyncCallback callback, object asyncState)
        {
            GetSchedulesRequest inValue = new GetSchedulesRequest();
            inValue.Token = Token;
            return ((SchedulePort)(this)).BeginGetSchedules(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetSchedulesResponse SchedulePort.EndGetSchedules(System.IAsyncResult result)
        {
            return base.Channel.EndGetSchedules(result);
        }

        public Schedule[] EndGetSchedules(System.IAsyncResult result)
        {
            GetSchedulesResponse retVal = ((SchedulePort)(this)).EndGetSchedules(result);
            return retVal.Schedule;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetScheduleListResponse SchedulePort.GetScheduleList(GetScheduleListRequest request)
        {
            return base.Channel.GetScheduleList(request);
        }

        public string GetScheduleList(int? Limit, string StartReference, out Schedule[] Schedule)
        {
            GetScheduleListRequest inValue = new GetScheduleListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            GetScheduleListResponse retVal = ((SchedulePort)(this)).GetScheduleList(inValue);
            Schedule = retVal.Schedule;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SchedulePort.BeginGetScheduleList(GetScheduleListRequest request,
                                                              System.AsyncCallback callback,
                                                              object asyncState)
        {
            return base.Channel.BeginGetScheduleList(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetScheduleList(int? Limit,
                                                        string StartReference,
                                                        System.AsyncCallback callback,
                                                        object asyncState)
        {
            GetScheduleListRequest inValue = new GetScheduleListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            return ((SchedulePort)(this)).BeginGetScheduleList(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetScheduleListResponse SchedulePort.EndGetScheduleList(System.IAsyncResult result)
        {
            return base.Channel.EndGetScheduleList(result);
        }

        public string EndGetScheduleList(System.IAsyncResult result, out Schedule[] Schedule)
        {
            GetScheduleListResponse retVal = ((SchedulePort)(this)).EndGetScheduleList(result);
            Schedule = retVal.Schedule;
            return retVal.NextStartReference;
        }

        public string CreateSchedule(Schedule Schedule)
        {
            return base.Channel.CreateSchedule(Schedule);
        }

        public System.IAsyncResult BeginCreateSchedule(Schedule Schedule,
                                                       System.AsyncCallback callback,
                                                       object asyncState)
        {
            return base.Channel.BeginCreateSchedule(Schedule, callback, asyncState);
        }

        public string EndCreateSchedule(System.IAsyncResult result)
        {
            return base.Channel.EndCreateSchedule(result);
        }

        public void ModifySchedule(Schedule Schedule)
        {
            base.Channel.ModifySchedule(Schedule);
        }

        public System.IAsyncResult BeginModifySchedule(Schedule Schedule,
                                                       System.AsyncCallback callback,
                                                       object asyncState)
        {
            return base.Channel.BeginModifySchedule(Schedule, callback, asyncState);
        }

        public void EndModifySchedule(System.IAsyncResult result)
        {
            base.Channel.EndModifySchedule(result);
        }

        public void DeleteSchedule(string Token)
        {
            base.Channel.DeleteSchedule(Token);
        }

        public System.IAsyncResult BeginDeleteSchedule(string Token, System.AsyncCallback callback, object asyncState)
        {
            return base.Channel.BeginDeleteSchedule(Token, callback, asyncState);
        }

        public void EndDeleteSchedule(System.IAsyncResult result)
        {
            base.Channel.EndDeleteSchedule(result);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetSpecialDayGroupInfoResponse SchedulePort.GetSpecialDayGroupInfo(GetSpecialDayGroupInfoRequest request)
        {
            return base.Channel.GetSpecialDayGroupInfo(request);
        }

        public SpecialDayGroupInfo[] GetSpecialDayGroupInfo(string[] Token)
        {
            GetSpecialDayGroupInfoRequest inValue = new GetSpecialDayGroupInfoRequest();
            inValue.Token = Token;
            GetSpecialDayGroupInfoResponse retVal = ((SchedulePort)(this)).GetSpecialDayGroupInfo(inValue);
            return retVal.SpecialDayGroupInfo;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SchedulePort.BeginGetSpecialDayGroupInfo(GetSpecialDayGroupInfoRequest request,
                                                                     System.AsyncCallback callback,
                                                                     object asyncState)
        {
            return base.Channel.BeginGetSpecialDayGroupInfo(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetSpecialDayGroupInfo(string[] Token,
                                                               System.AsyncCallback callback,
                                                               object asyncState)
        {
            GetSpecialDayGroupInfoRequest inValue = new GetSpecialDayGroupInfoRequest();
            inValue.Token = Token;
            return ((SchedulePort)(this)).BeginGetSpecialDayGroupInfo(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetSpecialDayGroupInfoResponse SchedulePort.EndGetSpecialDayGroupInfo(System.IAsyncResult result)
        {
            return base.Channel.EndGetSpecialDayGroupInfo(result);
        }

        public SpecialDayGroupInfo[] EndGetSpecialDayGroupInfo(System.IAsyncResult result)
        {
            GetSpecialDayGroupInfoResponse retVal = ((SchedulePort)(this)).EndGetSpecialDayGroupInfo(result);
            return retVal.SpecialDayGroupInfo;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetSpecialDayGroupInfoListResponse SchedulePort.GetSpecialDayGroupInfoList(
                GetSpecialDayGroupInfoListRequest request)
        {
            return base.Channel.GetSpecialDayGroupInfoList(request);
        }

        public string GetSpecialDayGroupInfoList(int? Limit,
                                                 string StartReference,
                                                 out SpecialDayGroupInfo[] SpecialDayGroupInfo)
        {
            GetSpecialDayGroupInfoListRequest inValue = new GetSpecialDayGroupInfoListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            GetSpecialDayGroupInfoListResponse retVal = ((SchedulePort)(this)).GetSpecialDayGroupInfoList(inValue);
            SpecialDayGroupInfo = retVal.SpecialDayGroupInfo;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SchedulePort.BeginGetSpecialDayGroupInfoList(GetSpecialDayGroupInfoListRequest request,
                                                                         System.AsyncCallback callback,
                                                                         object asyncState)
        {
            return base.Channel.BeginGetSpecialDayGroupInfoList(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetSpecialDayGroupInfoList(int? Limit,
                                                                   string StartReference,
                                                                   System.AsyncCallback callback,
                                                                   object asyncState)
        {
            GetSpecialDayGroupInfoListRequest inValue = new GetSpecialDayGroupInfoListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            return ((SchedulePort)(this)).BeginGetSpecialDayGroupInfoList(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetSpecialDayGroupInfoListResponse SchedulePort.EndGetSpecialDayGroupInfoList(System.IAsyncResult result)
        {
            return base.Channel.EndGetSpecialDayGroupInfoList(result);
        }

        public string EndGetSpecialDayGroupInfoList(System.IAsyncResult result,
                                                    out SpecialDayGroupInfo[] SpecialDayGroupInfo)
        {
            GetSpecialDayGroupInfoListResponse retVal = ((SchedulePort)(this)).EndGetSpecialDayGroupInfoList(result);
            SpecialDayGroupInfo = retVal.SpecialDayGroupInfo;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetSpecialDayGroupsResponse SchedulePort.GetSpecialDayGroups(GetSpecialDayGroupsRequest request)
        {
            return base.Channel.GetSpecialDayGroups(request);
        }

        public SpecialDayGroup[] GetSpecialDayGroups(string[] Token)
        {
            GetSpecialDayGroupsRequest inValue = new GetSpecialDayGroupsRequest();
            inValue.Token = Token;
            GetSpecialDayGroupsResponse retVal = ((SchedulePort)(this)).GetSpecialDayGroups(inValue);
            return retVal.SpecialDayGroup;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SchedulePort.BeginGetSpecialDayGroups(GetSpecialDayGroupsRequest request,
                                                                  System.AsyncCallback callback,
                                                                  object asyncState)
        {
            return base.Channel.BeginGetSpecialDayGroups(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetSpecialDayGroups(string[] Token,
                                                            System.AsyncCallback callback,
                                                            object asyncState)
        {
            GetSpecialDayGroupsRequest inValue = new GetSpecialDayGroupsRequest();
            inValue.Token = Token;
            return ((SchedulePort)(this)).BeginGetSpecialDayGroups(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetSpecialDayGroupsResponse SchedulePort.EndGetSpecialDayGroups(System.IAsyncResult result)
        {
            return base.Channel.EndGetSpecialDayGroups(result);
        }

        public SpecialDayGroup[] EndGetSpecialDayGroups(System.IAsyncResult result)
        {
            GetSpecialDayGroupsResponse retVal = ((SchedulePort)(this)).EndGetSpecialDayGroups(result);
            return retVal.SpecialDayGroup;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetSpecialDayGroupListResponse SchedulePort.GetSpecialDayGroupList(GetSpecialDayGroupListRequest request)
        {
            return base.Channel.GetSpecialDayGroupList(request);
        }

        public string GetSpecialDayGroupList(int? Limit, string StartReference, out SpecialDayGroup[] SpecialDayGroup)
        {
            GetSpecialDayGroupListRequest inValue = new GetSpecialDayGroupListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            GetSpecialDayGroupListResponse retVal = ((SchedulePort)(this)).GetSpecialDayGroupList(inValue);
            SpecialDayGroup = retVal.SpecialDayGroup;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SchedulePort.BeginGetSpecialDayGroupList(GetSpecialDayGroupListRequest request,
                                                                     System.AsyncCallback callback,
                                                                     object asyncState)
        {
            return base.Channel.BeginGetSpecialDayGroupList(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetSpecialDayGroupList(int? Limit,
                                                               string StartReference,
                                                               System.AsyncCallback callback,
                                                               object asyncState)
        {
            GetSpecialDayGroupListRequest inValue = new GetSpecialDayGroupListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            return ((SchedulePort)(this)).BeginGetSpecialDayGroupList(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetSpecialDayGroupListResponse SchedulePort.EndGetSpecialDayGroupList(System.IAsyncResult result)
        {
            return base.Channel.EndGetSpecialDayGroupList(result);
        }

        public string EndGetSpecialDayGroupList(System.IAsyncResult result, out SpecialDayGroup[] SpecialDayGroup)
        {
            GetSpecialDayGroupListResponse retVal = ((SchedulePort)(this)).EndGetSpecialDayGroupList(result);
            SpecialDayGroup = retVal.SpecialDayGroup;
            return retVal.NextStartReference;
        }

        public string CreateSpecialDayGroup(SpecialDayGroup SpecialDayGroup)
        {
            return base.Channel.CreateSpecialDayGroup(SpecialDayGroup);
        }

        public System.IAsyncResult BeginCreateSpecialDayGroup(SpecialDayGroup SpecialDayGroup,
                                                              System.AsyncCallback callback,
                                                              object asyncState)
        {
            return base.Channel.BeginCreateSpecialDayGroup(SpecialDayGroup, callback, asyncState);
        }

        public string EndCreateSpecialDayGroup(System.IAsyncResult result)
        {
            return base.Channel.EndCreateSpecialDayGroup(result);
        }

        public void ModifySpecialDayGroup(SpecialDayGroup SpecialDayGroup)
        {
            base.Channel.ModifySpecialDayGroup(SpecialDayGroup);
        }

        public System.IAsyncResult BeginModifySpecialDayGroup(SpecialDayGroup SpecialDayGroup,
                                                              System.AsyncCallback callback,
                                                              object asyncState)
        {
            return base.Channel.BeginModifySpecialDayGroup(SpecialDayGroup, callback, asyncState);
        }

        public void EndModifySpecialDayGroup(System.IAsyncResult result)
        {
            base.Channel.EndModifySpecialDayGroup(result);
        }

        public void DeleteSpecialDayGroup(string Token)
        {
            base.Channel.DeleteSpecialDayGroup(Token);
        }

        public System.IAsyncResult BeginDeleteSpecialDayGroup(string Token,
                                                              System.AsyncCallback callback,
                                                              object asyncState)
        {
            return base.Channel.BeginDeleteSpecialDayGroup(Token, callback, asyncState);
        }

        public void EndDeleteSpecialDayGroup(System.IAsyncResult result)
        {
            base.Channel.EndDeleteSpecialDayGroup(result);
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl",
            ConfigurationName = "AccessRulesPort")]
    public interface AccessRulesPort
    {

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/GetServiceCapabilities", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Capabilities")]
        AccessRulesServiceCapabilities GetServiceCapabilities();

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/GetServiceCapabilities", ReplyAction = "*")]
        System.IAsyncResult BeginGetServiceCapabilities(System.AsyncCallback callback, object asyncState);

        [return: System.ServiceModel.MessageParameterAttribute(Name = "Capabilities")]
        AccessRulesServiceCapabilities EndGetServiceCapabilities(System.IAsyncResult result);

        // CODEGEN: Parameter 'AccessProfileInfo' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfileInfo", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "AccessProfileInfo")]
        GetAccessProfileInfoResponse GetAccessProfileInfo(GetAccessProfileInfoRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfileInfo", ReplyAction = "*")]
        System.IAsyncResult BeginGetAccessProfileInfo(GetAccessProfileInfoRequest request,
                                                      System.AsyncCallback callback,
                                                      object asyncState);

        GetAccessProfileInfoResponse EndGetAccessProfileInfo(System.IAsyncResult result);

        // CODEGEN: Parameter 'AccessProfileInfo' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfileInfoList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "NextStartReference")]
        GetAccessProfileInfoListResponse GetAccessProfileInfoList(GetAccessProfileInfoListRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfileInfoList", ReplyAction = "*")]
        System.IAsyncResult BeginGetAccessProfileInfoList(GetAccessProfileInfoListRequest request,
                                                          System.AsyncCallback callback,
                                                          object asyncState);

        GetAccessProfileInfoListResponse EndGetAccessProfileInfoList(System.IAsyncResult result);

        // CODEGEN: Parameter 'AccessProfile' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfiles", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "AccessProfile")]
        GetAccessProfilesResponse GetAccessProfiles(GetAccessProfilesRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfiles", ReplyAction = "*")]
        System.IAsyncResult BeginGetAccessProfiles(GetAccessProfilesRequest request,
                                                   System.AsyncCallback callback,
                                                   object asyncState);

        GetAccessProfilesResponse EndGetAccessProfiles(System.IAsyncResult result);

        // CODEGEN: Parameter 'AccessProfile' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfileList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "NextStartReference")]
        GetAccessProfileListResponse GetAccessProfileList(GetAccessProfileListRequest request);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/GetAccessProfileList", ReplyAction = "*")]
        System.IAsyncResult BeginGetAccessProfileList(GetAccessProfileListRequest request,
                                                      System.AsyncCallback callback,
                                                      object asyncState);

        GetAccessProfileListResponse EndGetAccessProfileList(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/CreateAccessProfile", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Token")]
        string CreateAccessProfile(AccessProfile AccessProfile);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/CreateAccessProfile", ReplyAction = "*")]
        System.IAsyncResult BeginCreateAccessProfile(AccessProfile AccessProfile,
                                                     System.AsyncCallback callback,
                                                     object asyncState);

        [return: System.ServiceModel.MessageParameterAttribute(Name = "Token")]
        string EndCreateAccessProfile(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/ModifyAccessProfile", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void ModifyAccessProfile(AccessProfile AccessProfile);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/ModifyAccessProfile", ReplyAction = "*")]
        System.IAsyncResult BeginModifyAccessProfile(AccessProfile AccessProfile,
                                                     System.AsyncCallback callback,
                                                     object asyncState);

        void EndModifyAccessProfile(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/DeleteAccessProfile", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DataEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(ConfigurationEntity))]
        //[System.ServiceModel.ServiceKnownTypeAttribute(typeof(DeviceEntity))]
        void DeleteAccessProfile(string Token);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true,
                Action = "http://www.onvif.org/ver10/accessrules/wsdl/DeleteAccessProfile", ReplyAction = "*")]
        System.IAsyncResult BeginDeleteAccessProfile(string Token, System.AsyncCallback callback, object asyncState);

        void EndDeleteAccessProfile(System.IAsyncResult result);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetAccessProfileInfo",
            WrapperNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", IsWrapped = true)]
    public partial class GetAccessProfileInfoRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Token")]
        public string[] Token;

        public GetAccessProfileInfoRequest()
        {
        }

        public GetAccessProfileInfoRequest(string[] Token)
        {
            this.Token = Token;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetAccessProfileInfoResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", IsWrapped = true)]
    public partial class GetAccessProfileInfoResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("AccessProfileInfo")]
        public
            AccessProfileInfo[] AccessProfileInfo;

        public GetAccessProfileInfoResponse()
        {
        }

        public GetAccessProfileInfoResponse(AccessProfileInfo[] AccessProfileInfo)
        {
            this.AccessProfileInfo = AccessProfileInfo;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetAccessProfileInfoList",
            WrapperNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", IsWrapped = true)]
    public partial class GetAccessProfileInfoListRequest
    {

        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl", Order = 0)]
        public int? Limit;

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "Limit",
                Namespace = "http://www.onvif.org/ver10/accessrules/wsdl", Order = 0)]
        public string LimitSerialize
        {
            get { return Limit.HasValue ? Limit.Value.ToString() : null; }
        }

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl",
                Order = 1)]
        public string StartReference;

        public GetAccessProfileInfoListRequest()
        {
        }

        public GetAccessProfileInfoListRequest(int? Limit, string StartReference)
        {
            this.Limit = Limit;
            this.StartReference = StartReference;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetAccessProfileInfoListResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", IsWrapped = true)]
    public partial class GetAccessProfileInfoListResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl",
                Order = 0)]
        public string NextStartReference;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl",
                Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("AccessProfileInfo")]
        public
            AccessProfileInfo[] AccessProfileInfo;

        public GetAccessProfileInfoListResponse()
        {
        }

        public GetAccessProfileInfoListResponse(string NextStartReference, AccessProfileInfo[] AccessProfileInfo)
        {
            this.NextStartReference = NextStartReference;
            this.AccessProfileInfo = AccessProfileInfo;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetAccessProfiles",
            WrapperNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", IsWrapped = true)]
    public partial class GetAccessProfilesRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Token")]
        public string[] Token;

        public GetAccessProfilesRequest()
        {
        }

        public GetAccessProfilesRequest(string[] Token)
        {
            this.Token = Token;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetAccessProfilesResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", IsWrapped = true)]
    public partial class GetAccessProfilesResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl",
                Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("AccessProfile")]
        public AccessProfile[]
            AccessProfile;

        public GetAccessProfilesResponse()
        {
        }

        public GetAccessProfilesResponse(AccessProfile[] AccessProfile)
        {
            this.AccessProfile = AccessProfile;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetAccessProfileList",
            WrapperNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", IsWrapped = true)]
    public partial class GetAccessProfileListRequest
    {

        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl", Order = 0)]
        public int? Limit;

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "Limit",
                Namespace = "http://www.onvif.org/ver10/accessrules/wsdl", Order = 0)]
        public string LimitSerialize
        {
            get { return Limit.HasValue ? Limit.Value.ToString() : null; }
        }

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl",
                Order = 1)]
        public string StartReference;

        public GetAccessProfileListRequest()
        {
        }

        public GetAccessProfileListRequest(int Limit, string StartReference)
        {
            this.Limit = Limit;
            this.StartReference = StartReference;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "GetAccessProfileListResponse",
            WrapperNamespace = "http://www.onvif.org/ver10/accessrules/wsdl", IsWrapped = true)]
    public partial class GetAccessProfileListResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl",
                Order = 0)]
        public string NextStartReference;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.onvif.org/ver10/accessrules/wsdl",
                Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("AccessProfile")]
        public AccessProfile[]
            AccessProfile;

        public GetAccessProfileListResponse()
        {
        }

        public GetAccessProfileListResponse(string NextStartReference, AccessProfile[] AccessProfile)
        {
            this.NextStartReference = NextStartReference;
            this.AccessProfile = AccessProfile;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AccessRulesPortChannel : AccessRulesPort, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AccessRulesPortClient : System.ServiceModel.ClientBase<AccessRulesPort>, AccessRulesPort
    {

        public AccessRulesPortClient()
        {
        }

        public AccessRulesPortClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public AccessRulesPortClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public AccessRulesPortClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            :
                    base(endpointConfigurationName, remoteAddress)
        {
        }

        public AccessRulesPortClient(System.ServiceModel.Channels.Binding binding,
                                     System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public AccessRulesServiceCapabilities GetServiceCapabilities()
        {
            return base.Channel.GetServiceCapabilities();
        }

        public System.IAsyncResult BeginGetServiceCapabilities(System.AsyncCallback callback, object asyncState)
        {
            return base.Channel.BeginGetServiceCapabilities(callback, asyncState);
        }

        public AccessRulesServiceCapabilities EndGetServiceCapabilities(System.IAsyncResult result)
        {
            return base.Channel.EndGetServiceCapabilities(result);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetAccessProfileInfoResponse AccessRulesPort.GetAccessProfileInfo(GetAccessProfileInfoRequest request)
        {
            return base.Channel.GetAccessProfileInfo(request);
        }

        public AccessProfileInfo[] GetAccessProfileInfo(string[] Token)
        {
            GetAccessProfileInfoRequest inValue = new GetAccessProfileInfoRequest();
            inValue.Token = Token;
            GetAccessProfileInfoResponse retVal = ((AccessRulesPort)(this)).GetAccessProfileInfo(inValue);
            return retVal.AccessProfileInfo;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult AccessRulesPort.BeginGetAccessProfileInfo(GetAccessProfileInfoRequest request,
                                                                      System.AsyncCallback callback,
                                                                      object asyncState)
        {
            return base.Channel.BeginGetAccessProfileInfo(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetAccessProfileInfo(string[] Token,
                                                             System.AsyncCallback callback,
                                                             object asyncState)
        {
            GetAccessProfileInfoRequest inValue = new GetAccessProfileInfoRequest();
            inValue.Token = Token;
            return ((AccessRulesPort)(this)).BeginGetAccessProfileInfo(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetAccessProfileInfoResponse AccessRulesPort.EndGetAccessProfileInfo(System.IAsyncResult result)
        {
            return base.Channel.EndGetAccessProfileInfo(result);
        }

        public AccessProfileInfo[] EndGetAccessProfileInfo(System.IAsyncResult result)
        {
            GetAccessProfileInfoResponse retVal = ((AccessRulesPort)(this)).EndGetAccessProfileInfo(result);
            return retVal.AccessProfileInfo;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetAccessProfileInfoListResponse AccessRulesPort.GetAccessProfileInfoList(
                GetAccessProfileInfoListRequest request)
        {
            return base.Channel.GetAccessProfileInfoList(request);
        }

        public string GetAccessProfileInfoList(int? Limit,
                                               string StartReference,
                                               out AccessProfileInfo[] AccessProfileInfo)
        {
            GetAccessProfileInfoListRequest inValue = new GetAccessProfileInfoListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            GetAccessProfileInfoListResponse retVal = ((AccessRulesPort)(this)).GetAccessProfileInfoList(inValue);
            AccessProfileInfo = retVal.AccessProfileInfo;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult AccessRulesPort.BeginGetAccessProfileInfoList(GetAccessProfileInfoListRequest request,
                                                                          System.AsyncCallback callback,
                                                                          object asyncState)
        {
            return base.Channel.BeginGetAccessProfileInfoList(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetAccessProfileInfoList(int Limit,
                                                                 string StartReference,
                                                                 System.AsyncCallback callback,
                                                                 object asyncState)
        {
            GetAccessProfileInfoListRequest inValue = new GetAccessProfileInfoListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            return ((AccessRulesPort)(this)).BeginGetAccessProfileInfoList(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetAccessProfileInfoListResponse AccessRulesPort.EndGetAccessProfileInfoList(System.IAsyncResult result)
        {
            return base.Channel.EndGetAccessProfileInfoList(result);
        }

        public string EndGetAccessProfileInfoList(System.IAsyncResult result, out AccessProfileInfo[] AccessProfileInfo)
        {
            GetAccessProfileInfoListResponse retVal = ((AccessRulesPort)(this)).EndGetAccessProfileInfoList(result);
            AccessProfileInfo = retVal.AccessProfileInfo;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetAccessProfilesResponse AccessRulesPort.GetAccessProfiles(GetAccessProfilesRequest request)
        {
            return base.Channel.GetAccessProfiles(request);
        }

        public AccessProfile[] GetAccessProfiles(string[] Token)
        {
            GetAccessProfilesRequest inValue = new GetAccessProfilesRequest();
            inValue.Token = Token;
            GetAccessProfilesResponse retVal = ((AccessRulesPort)(this)).GetAccessProfiles(inValue);
            return retVal.AccessProfile;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult AccessRulesPort.BeginGetAccessProfiles(GetAccessProfilesRequest request,
                                                                   System.AsyncCallback callback,
                                                                   object asyncState)
        {
            return base.Channel.BeginGetAccessProfiles(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetAccessProfiles(string[] Token,
                                                          System.AsyncCallback callback,
                                                          object asyncState)
        {
            GetAccessProfilesRequest inValue = new GetAccessProfilesRequest();
            inValue.Token = Token;
            return ((AccessRulesPort)(this)).BeginGetAccessProfiles(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetAccessProfilesResponse AccessRulesPort.EndGetAccessProfiles(System.IAsyncResult result)
        {
            return base.Channel.EndGetAccessProfiles(result);
        }

        public AccessProfile[] EndGetAccessProfiles(System.IAsyncResult result)
        {
            GetAccessProfilesResponse retVal = ((AccessRulesPort)(this)).EndGetAccessProfiles(result);
            return retVal.AccessProfile;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetAccessProfileListResponse AccessRulesPort.GetAccessProfileList(GetAccessProfileListRequest request)
        {
            return base.Channel.GetAccessProfileList(request);
        }

        public string GetAccessProfileList(int? Limit, string StartReference, out AccessProfile[] AccessProfile)
        {
            GetAccessProfileListRequest inValue = new GetAccessProfileListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            GetAccessProfileListResponse retVal = ((AccessRulesPort)(this)).GetAccessProfileList(inValue);
            AccessProfile = retVal.AccessProfile;
            return retVal.NextStartReference;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult AccessRulesPort.BeginGetAccessProfileList(GetAccessProfileListRequest request,
                                                                      System.AsyncCallback callback,
                                                                      object asyncState)
        {
            return base.Channel.BeginGetAccessProfileList(request, callback, asyncState);
        }

        public System.IAsyncResult BeginGetAccessProfileList(int Limit,
                                                             string StartReference,
                                                             System.AsyncCallback callback,
                                                             object asyncState)
        {
            GetAccessProfileListRequest inValue = new GetAccessProfileListRequest();
            inValue.Limit = Limit;
            inValue.StartReference = StartReference;
            return ((AccessRulesPort)(this)).BeginGetAccessProfileList(inValue, callback, asyncState);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetAccessProfileListResponse AccessRulesPort.EndGetAccessProfileList(System.IAsyncResult result)
        {
            return base.Channel.EndGetAccessProfileList(result);
        }

        public string EndGetAccessProfileList(System.IAsyncResult result, out AccessProfile[] AccessProfile)
        {
            GetAccessProfileListResponse retVal = ((AccessRulesPort)(this)).EndGetAccessProfileList(result);
            AccessProfile = retVal.AccessProfile;
            return retVal.NextStartReference;
        }

        public string CreateAccessProfile(AccessProfile AccessProfile)
        {
            return base.Channel.CreateAccessProfile(AccessProfile);
        }

        public System.IAsyncResult BeginCreateAccessProfile(AccessProfile AccessProfile,
                                                            System.AsyncCallback callback,
                                                            object asyncState)
        {
            return base.Channel.BeginCreateAccessProfile(AccessProfile, callback, asyncState);
        }

        public string EndCreateAccessProfile(System.IAsyncResult result)
        {
            return base.Channel.EndCreateAccessProfile(result);
        }

        public void ModifyAccessProfile(AccessProfile AccessProfile)
        {
            base.Channel.ModifyAccessProfile(AccessProfile);
        }

        public System.IAsyncResult BeginModifyAccessProfile(AccessProfile AccessProfile,
                                                            System.AsyncCallback callback,
                                                            object asyncState)
        {
            return base.Channel.BeginModifyAccessProfile(AccessProfile, callback, asyncState);
        }

        public void EndModifyAccessProfile(System.IAsyncResult result)
        {
            base.Channel.EndModifyAccessProfile(result);
        }

        public void DeleteAccessProfile(string Token)
        {
            base.Channel.DeleteAccessProfile(Token);
        }

        public System.IAsyncResult BeginDeleteAccessProfile(string Token,
                                                            System.AsyncCallback callback,
                                                            object asyncState)
        {
            return base.Channel.BeginDeleteAccessProfile(Token, callback, asyncState);
        }

        public void EndDeleteAccessProfile(System.IAsyncResult result)
        {
            base.Channel.EndDeleteAccessProfile(result);
        }
    }


}