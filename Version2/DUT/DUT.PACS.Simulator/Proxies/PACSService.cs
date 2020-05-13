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
using DUT.PACS.Simulator.Common;

// 
// This source code was auto-generated by wsdl, Version=2.0.50727.3038.
// 
namespace DUT.PACS.Simulator.ServiceAccessControl10
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "PACSBinding", Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public abstract partial class PACSServiceBinding : BaseDutService
    {

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public abstract ServiceCapabilities GetServiceCapabilities();

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointInfoList", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public abstract string GetAccessPointInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("AccessPointInfo")] out AccessPointInfo[] AccessPointInfo);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointInfo", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AccessPointInfo")]
        public abstract AccessPointInfo[] GetAccessPointInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointState", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AccessPointState")]
        public abstract AccessPointState GetAccessPointState(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/EnableAccessPoint", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract void EnableAccessPoint(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/DisableAccessPoint", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract void DisableAccessPoint(string Token);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/ExternalAuthorization", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract void ExternalAuthorization(string AccessPointToken, string CredentialToken, string Reason, Decision Decision);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAreaInfoList", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public abstract string GetAreaInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("AreaInfo")] out AreaInfo[] AreaInfo);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAreaInfo", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AreaInfo")]
        public abstract AreaInfo[] GetAreaInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class ServiceCapabilities
    {

        private System.Xml.XmlElement[] anyField;

        private uint maxLimitField;

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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AreaInfoBase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AreaInfo))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AccessPointInfoBase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AccessPointInfo))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AreaInfo))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class AreaInfoBase : DataEntity
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public partial class AreaInfo : AreaInfoBase
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AccessPointInfo))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
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

        /// <remarks/>
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    public enum Decision
    {

        /// <remarks/>
        Granted,

        /// <remarks/>
        Denied,
    }
}