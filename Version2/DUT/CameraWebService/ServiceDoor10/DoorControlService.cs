﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3634
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=2.0.50727.3038.
// 
namespace DUT.CameraWebService.Door10
{

    /// <remarks/>
    // CODEGEN: The optional WSDL extension element 'annotation' from namespace 'http://www.w3.org/2001/XMLSchema' was not handled.
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "DoorControlBinding", Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public abstract partial class Door10ServiceBinding : System.Web.Services.WebService
    {

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetDoorState", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DoorState")]
        public abstract DoorState GetDoorState(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetDoorInfoList", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("DoorInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public abstract DoorInfo[] GetDoorInfoList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList, int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, int Offset, [System.Xml.Serialization.XmlIgnoreAttribute()] bool OffsetSpecified);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetDoorInfo", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DoorInfo")]
        public abstract DoorInfo GetDoorInfo(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/AccessDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract void AccessDoor(string Token, bool UseExtendedTime, [System.Xml.Serialization.XmlIgnoreAttribute()] bool UseExtendedTimeSpecified, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string AccessTime, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string OpenTooLongTime, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string PreAlarmTime, AccessDoorExtension Extension);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract void LockDoor(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/UnlockDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract void UnlockDoor(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/BlockDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract void BlockDoor(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockDownDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract void LockDownDoor(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockDownReleaseDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract void LockDownReleaseDoor(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockOpenDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract void LockOpenDoor(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockOpenReleaseDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract void LockOpenReleaseDoor(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/DoubleLockDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract void DoubleLockDoor(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public abstract ServiceCapabilities GetServiceCapabilities();
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    public partial class DoorState
    {

        private DoorModeType doorModeField;

        private DoorMonitorStateType doorMonitorField;

        private DoorLockMonitorStateType doorLockMonitorField;

        private DoorLockMonitorStateType doorDoubleLockMonitorField;

        private bool doorDoubleLockMonitorFieldSpecified;

        private DoorTamperStateType doorTamperField;

        private DoorAlarmStateType doorAlarmField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        public DoorModeType DoorMode
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
        public DoorMonitorStateType DoorMonitor
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
        public DoorLockMonitorStateType DoorLockMonitor
        {
            get
            {
                return this.doorLockMonitorField;
            }
            set
            {
                this.doorLockMonitorField = value;
            }
        }

        /// <remarks/>
        public DoorLockMonitorStateType DoorDoubleLockMonitor
        {
            get
            {
                return this.doorDoubleLockMonitorField;
            }
            set
            {
                this.doorDoubleLockMonitorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DoorDoubleLockMonitorSpecified
        {
            get
            {
                return this.doorDoubleLockMonitorFieldSpecified;
            }
            set
            {
                this.doorDoubleLockMonitorFieldSpecified = value;
            }
        }

        /// <remarks/>
        public DoorTamperStateType DoorTamper
        {
            get
            {
                return this.doorTamperField;
            }
            set
            {
                this.doorTamperField = value;
            }
        }

        /// <remarks/>
        public DoorAlarmStateType DoorAlarm
        {
            get
            {
                return this.doorAlarmField;
            }
            set
            {
                this.doorAlarmField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    public enum DoorModeType
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    public enum DoorMonitorStateType
    {

        /// <remarks/>
        Unknown,

        /// <remarks/>
        Open,

        /// <remarks/>
        Closed,

        /// <remarks/>
        NotSupported,

        /// <remarks/>
        Fault,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    public enum DoorLockMonitorStateType
    {

        /// <remarks/>
        Unknown,

        /// <remarks/>
        Locked,

        /// <remarks/>
        Unlocked,

        /// <remarks/>
        NotSupported,

        /// <remarks/>
        Fault,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    public enum DoorTamperStateType
    {

        /// <remarks/>
        NotSupported,

        /// <remarks/>
        NotInTamper,

        /// <remarks/>
        TamperDetected,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    public enum DoorAlarmStateType
    {

        /// <remarks/>
        Normal,

        /// <remarks/>
        DoorForcedOpen,

        /// <remarks/>
        DoorOpenTooLong,

        /// <remarks/>
        Fault,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    public partial class ServiceCapabilities
    {

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    public partial class AccessDoorExtension
    {

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    public partial class DoorCapabilities
    {

        private bool momentaryAccessField;

        private bool lockField;

        private bool unlockField;

        private bool blockField;

        private bool doubleLockField;

        private bool lockDownField;

        private bool lockOpenField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        public bool MomentaryAccess
        {
            get
            {
                return this.momentaryAccessField;
            }
            set
            {
                this.momentaryAccessField = value;
            }
        }

        /// <remarks/>
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
        [System.Xml.Serialization.XmlAnyElementAttribute()]
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DoorInfoBase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DoorInfo))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    public partial class DataEntity
    {

        private string tokenField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string token
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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DoorInfo))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    public partial class DoorInfoBase : DataEntity
    {

        private string nameField;

        private string descriptionField;

        /// <remarks/>
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    public partial class DoorInfo : DoorInfoBase
    {

        private DoorCapabilities doorCapabilitiesField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        public DoorCapabilities DoorCapabilities
        {
            get
            {
                return this.doorCapabilitiesField;
            }
            set
            {
                this.doorCapabilitiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
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
}