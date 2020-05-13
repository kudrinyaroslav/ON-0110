﻿//------------------------------------------------------------------------------
// <auto-generated>
// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace TestTool.Proxies.WSDiscovery
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    [System.Xml.Serialization.XmlRootAttribute("Hello", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery", IsNullable = false)]
    public partial class HelloType
    {

        private EndpointReferenceType endpointReferenceField;

        private string typesField;

        private ScopesType scopesField;

        private string xAddrsField;

        private uint metadataVersionField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
        public EndpointReferenceType EndpointReference
        {
            get
            {
                return this.endpointReferenceField;
            }
            set
            {
                this.endpointReferenceField = value;
            }
        }

        /// <remarks/>
        public string Types
        {
            get
            {
                return this.typesField;
            }
            set
            {
                this.typesField = value;
            }
        }

        /// <remarks/>
        public ScopesType Scopes
        {
            get
            {
                return this.scopesField;
            }
            set
            {
                this.scopesField = value;
            }
        }

        /// <remarks/>
        public string XAddrs
        {
            get
            {
                return this.xAddrsField;
            }
            set
            {
                this.xAddrsField = value;
            }
        }

        /// <remarks/>
        public uint MetadataVersion
        {
            get
            {
                return this.metadataVersionField;
            }
            set
            {
                this.metadataVersionField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    [System.Xml.Serialization.XmlRootAttribute("EndpointReference", Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing", IsNullable = false)]
    public partial class EndpointReferenceType
    {

        private AttributedURI addressField;

        private ReferencePropertiesType referencePropertiesField;

        private ReferenceParametersType referenceParametersField;

        private AttributedQName portTypeField;

        private ServiceNameType serviceNameField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        public AttributedURI Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        public ReferencePropertiesType ReferenceProperties
        {
            get
            {
                return this.referencePropertiesField;
            }
            set
            {
                this.referencePropertiesField = value;
            }
        }

        /// <remarks/>
        public ReferenceParametersType ReferenceParameters
        {
            get
            {
                return this.referenceParametersField;
            }
            set
            {
                this.referenceParametersField = value;
            }
        }

        /// <remarks/>
        public AttributedQName PortType
        {
            get
            {
                return this.portTypeField;
            }
            set
            {
                this.portTypeField = value;
            }
        }

        /// <remarks/>
        public ServiceNameType ServiceName
        {
            get
            {
                return this.serviceNameField;
            }
            set
            {
                this.serviceNameField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    [System.Xml.Serialization.XmlRootAttribute("MessageID", Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing", IsNullable = false)]
    public partial class AttributedURI
    {

        private System.Xml.XmlAttribute[] anyAttrField;

        private string valueField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute(DataType = "anyURI")]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    public partial class ResolveMatchType
    {

        private EndpointReferenceType endpointReferenceField;

        private string typesField;

        private ScopesType scopesField;

        private string xAddrsField;

        private uint metadataVersionField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
        public EndpointReferenceType EndpointReference
        {
            get
            {
                return this.endpointReferenceField;
            }
            set
            {
                this.endpointReferenceField = value;
            }
        }

        /// <remarks/>
        public string Types
        {
            get
            {
                return this.typesField;
            }
            set
            {
                this.typesField = value;
            }
        }

        /// <remarks/>
        public ScopesType Scopes
        {
            get
            {
                return this.scopesField;
            }
            set
            {
                this.scopesField = value;
            }
        }

        /// <remarks/>
        public string XAddrs
        {
            get
            {
                return this.xAddrsField;
            }
            set
            {
                this.xAddrsField = value;
            }
        }

        /// <remarks/>
        public uint MetadataVersion
        {
            get
            {
                return this.metadataVersionField;
            }
            set
            {
                this.metadataVersionField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    [System.Xml.Serialization.XmlRootAttribute("Scopes", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery", IsNullable = false)]
    public partial class ScopesType
    {

        private string matchByField;

        private System.Xml.XmlAttribute[] anyAttrField;

        private string[] textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string MatchBy
        {
            get
            {
                return this.matchByField;
            }
            set
            {
                this.matchByField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute(DataType = "anyURI")]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    public partial class ProbeMatchType
    {
        [System.Xml.Serialization.XmlNamespaceDeclarations]
        public System.Xml.Serialization.XmlSerializerNamespaces Xmlns
        {
            get { return xmlns; }
            set { xmlns = value; }
        }

        private System.Xml.Serialization.XmlSerializerNamespaces xmlns;

        private EndpointReferenceType endpointReferenceField;

        private string typesField;

        private ScopesType scopesField;

        private string xAddrsField;

        private uint metadataVersionField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
        public EndpointReferenceType EndpointReference
        {
            get
            {
                return this.endpointReferenceField;
            }
            set
            {
                this.endpointReferenceField = value;
            }
        }

        /// <remarks/>
        public string Types
        {
            get
            {
                return this.typesField;
            }
            set
            {
                this.typesField = value;
            }
        }

        /// <remarks/>
        public ScopesType Scopes
        {
            get
            {
                return this.scopesField;
            }
            set
            {
                this.scopesField = value;
            }
        }

        /// <remarks/>
        public string XAddrs
        {
            get
            {
                return this.xAddrsField;
            }
            set
            {
                this.xAddrsField = value;
            }
        }

        /// <remarks/>
        public uint MetadataVersion
        {
            get
            {
                return this.metadataVersionField;
            }
            set
            {
                this.metadataVersionField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    public partial class ServiceNameType
    {

        private string portNameField;

        private System.Xml.XmlAttribute[] anyAttrField;

        private System.Xml.XmlQualifiedName valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
        public string PortName
        {
            get
            {
                return this.portNameField;
            }
            set
            {
                this.portNameField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public System.Xml.XmlQualifiedName Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    public partial class AttributedQName
    {

        private System.Xml.XmlAttribute[] anyAttrField;

        private System.Xml.XmlQualifiedName valueField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public System.Xml.XmlQualifiedName Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    public partial class ReferenceParametersType
    {

        private System.Xml.XmlElement[] anyField;

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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    public partial class ReferencePropertiesType
    {

        private System.Xml.XmlElement[] anyField;

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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    [System.Xml.Serialization.XmlRootAttribute("Bye", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery", IsNullable = false)]
    public partial class ByeType
    {

        private EndpointReferenceType endpointReferenceField;

        private string typesField;

        private ScopesType scopesField;

        private string xAddrsField;

        private uint metadataVersionField;

        private bool metadataVersionFieldSpecified;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
        public EndpointReferenceType EndpointReference
        {
            get
            {
                return this.endpointReferenceField;
            }
            set
            {
                this.endpointReferenceField = value;
            }
        }

        /// <remarks/>
        public string Types
        {
            get
            {
                return this.typesField;
            }
            set
            {
                this.typesField = value;
            }
        }

        /// <remarks/>
        public ScopesType Scopes
        {
            get
            {
                return this.scopesField;
            }
            set
            {
                this.scopesField = value;
            }
        }

        /// <remarks/>
        public string XAddrs
        {
            get
            {
                return this.xAddrsField;
            }
            set
            {
                this.xAddrsField = value;
            }
        }

        /// <remarks/>
        public uint MetadataVersion
        {
            get
            {
                return this.metadataVersionField;
            }
            set
            {
                this.metadataVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MetadataVersionSpecified
        {
            get
            {
                return this.metadataVersionFieldSpecified;
            }
            set
            {
                this.metadataVersionFieldSpecified = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    [System.Xml.Serialization.XmlRootAttribute("Probe", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery", IsNullable = false)]
    public partial class ProbeType
    {

        private string typesField;

        private ScopesType scopesField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        public string Types
        {
            get
            {
                return this.typesField;
            }
            set
            {
                this.typesField = value;
            }
        }

        /// <remarks/>
        public ScopesType Scopes
        {
            get
            {
                return this.scopesField;
            }
            set
            {
                this.scopesField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    [System.Xml.Serialization.XmlRootAttribute("ProbeMatches", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery", IsNullable = false)]
    public partial class ProbeMatchesType
    {
        [System.Xml.Serialization.XmlNamespaceDeclarations]
        public System.Xml.Serialization.XmlSerializerNamespaces Xmlns
        {
            get { return xmlns; }
            set { xmlns = value; }
        }

        private System.Xml.Serialization.XmlSerializerNamespaces xmlns;

        private ProbeMatchType[] probeMatchField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ProbeMatch")]
        public ProbeMatchType[] ProbeMatch
        {
            get
            {
                return this.probeMatchField;
            }
            set
            {
                this.probeMatchField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    [System.Xml.Serialization.XmlRootAttribute("Resolve", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery", IsNullable = false)]
    public partial class ResolveType
    {

        private EndpointReferenceType endpointReferenceField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
        public EndpointReferenceType EndpointReference
        {
            get
            {
                return this.endpointReferenceField;
            }
            set
            {
                this.endpointReferenceField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    [System.Xml.Serialization.XmlRootAttribute("ResolveMatches", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery", IsNullable = false)]
    public partial class ResolveMatchesType
    {

        private ResolveMatchType resolveMatchField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        public ResolveMatchType ResolveMatch
        {
            get
            {
                return this.resolveMatchField;
            }
            set
            {
                this.resolveMatchField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    [System.Xml.Serialization.XmlRootAttribute("Security", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery", IsNullable = false)]
    public partial class SecurityType
    {

        private SigType sigField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        public SigType Sig
        {
            get
            {
                return this.sigField;
            }
            set
            {
                this.sigField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    [System.Xml.Serialization.XmlRootAttribute("Sig", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery", IsNullable = false)]
    public partial class SigType
    {

        private System.Xml.XmlElement[] anyField;

        private string schemeField;

        private byte[] keyIdField;

        private string refsField;

        private byte[] sigField;

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
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string Scheme
        {
            get
            {
                return this.schemeField;
            }
            set
            {
                this.schemeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "base64Binary")]
        public byte[] KeyId
        {
            get
            {
                return this.keyIdField;
            }
            set
            {
                this.keyIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREFS")]
        public string Refs
        {
            get
            {
                return this.refsField;
            }
            set
            {
                this.refsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "base64Binary")]
        public byte[] Sig
        {
            get
            {
                return this.sigField;
            }
            set
            {
                this.sigField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    [System.Xml.Serialization.XmlRootAttribute("AppSequence", Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery", IsNullable = false)]
    public partial class AppSequenceType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    [System.Xml.Serialization.XmlRootAttribute("RelatesTo", Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing", IsNullable = false)]
    public partial class Relationship
    {

        private System.Xml.XmlQualifiedName relationshipTypeField;

        private System.Xml.XmlAttribute[] anyAttrField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName RelationshipType
        {
            get
            {
                return this.relationshipTypeField;
            }
            set
            {
                this.relationshipTypeField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute(DataType = "anyURI")]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    [System.Xml.Serialization.XmlRootAttribute("ReplyAfter", Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing", IsNullable = false)]
    public partial class ReplyAfterType
    {

        private System.Xml.XmlAttribute[] anyAttrField;

        private string valueField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute(DataType = "nonNegativeInteger")]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    [System.Xml.Serialization.XmlRootAttribute("RetryAfter", Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing", IsNullable = false)]
    public partial class RetryAfterType
    {

        private System.Xml.XmlAttribute[] anyAttrField;

        private string valueField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute(DataType = "nonNegativeInteger")]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}
