﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace TestSchema_Class {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class ONVIF_TEST {
        
        private Test[] testField;
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Test")]
        public Test[] Test {
            get {
                return this.testField;
            }
            set {
                this.testField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Test {
        
        private TestType itemField;
        
        private ItemChoiceType itemElementNameField;
        
        private string groupField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_ALL_CAPABILITIES", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_DEVICE_CAPABILITIES", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_DEVICE_INFORMATION", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_DNS_CONFIGURATION", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_DNS_TEST", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_FACTORY_DEFAULT", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_FACTORY_DEFAULT_SOFT", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_HOSTNAME_CONFIGURATION", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_HOSTNAME_TEST", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_INVALID_DNS_TEST", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_INVALID_HOSTNAME_TEST", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_INVALID_IP_NTP_TEST", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_INVALID_NAME_NTP_TEST", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_MEDIA_CAPABILITIES", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_NTP_CONFIGURATION", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_NTP_TEST", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_RESET", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_SERVICE_CATEGORY_CAPABILITIES", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_SOAP_FAULT_MESSAGE", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_SYSTEM_DATE_AND_TIME", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_SYSTEM_DATE_AND_TIME_INVALID_DATE_TEST", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_SYSTEM_DATE_AND_TIME_INVALID_TIMEZONE_TEST", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_SYSTEM_DATE_AND_TIME_TEST", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DEVICE_WSDL_URL", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DISCOVERY_BYE_MESSAGE", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DISCOVERY_DEVICE_SCOPES_CONFIGURATION", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DISCOVERY_MULTICAST_HELLO", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DISCOVERY_MULTICAST_HELLO_VALIDATE", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DISCOVERY_MULTICAST_SCOPE_SEARCH", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DISCOVERY_MULTICAST_SCOPE_SEARCH_INVALID", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DISCOVERY_MULTICAST_SCOPE_SEARCH_OMITTED_DEVICE", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DISCOVERY_SOAP_FAULT_MESSAGE", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DISCOVERY_UNICAST_SCOPE_SEARCH", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DISCOVERY_UNICAST_SCOPE_SEARCH_INVALID", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("DISCOVERY_UNICAST_SCOPE_SEARCH_OMITTED_DEVICE", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("MEDIA_DYNAMIC_MEDIA_PROFILE_CONFIGURATION", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("MEDIA_INVALID_TRANSPORT_SOAP_FAULT_MESSAGE", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("MEDIA_JPEG_VIDEO_ENCODER_CONFIGURATION", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("MEDIA_PROFILE_CONFIGURATION", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("MEDIA_SOAP_FAULT_MESSAGE", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("MEDIA_STREAM_URI__RTP_RTSP_HTTP", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("MEDIA_STREAM_URI__RTP_UDP_UNICAST", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("RTS_RTP_RTSP_HTTP", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("RTS_RTP_UDP_UNICAST", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("RTS_RTSP_KEEPALIVE", typeof(TestType))]
        [System.Xml.Serialization.XmlElementAttribute("RTS_RTSP_TCP", typeof(TestType))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public TestType Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName {
            get {
                return this.itemElementNameField;
            }
            set {
                this.itemElementNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Group {
            get {
                return this.groupField;
            }
            set {
                this.groupField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("DISCOVERY_MULTICAST_HELLO", Namespace="", IsNullable=false)]
    public partial class TestType {
        
        private Options optionsField;
        
        /// <remarks/>
        public Options Options {
            get {
                return this.optionsField;
            }
            set {
                this.optionsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Options {
        
        private bool itemField;
        
        public Options() {
            this.itemField = true;
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Execute")]
        public bool Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    public enum ItemChoiceType {
        
        /// <remarks/>
        DEVICE_ALL_CAPABILITIES,
        
        /// <remarks/>
        DEVICE_DEVICE_CAPABILITIES,
        
        /// <remarks/>
        DEVICE_DEVICE_INFORMATION,
        
        /// <remarks/>
        DEVICE_DNS_CONFIGURATION,
        
        /// <remarks/>
        DEVICE_DNS_TEST,
        
        /// <remarks/>
        DEVICE_FACTORY_DEFAULT,
        
        /// <remarks/>
        DEVICE_FACTORY_DEFAULT_SOFT,
        
        /// <remarks/>
        DEVICE_HOSTNAME_CONFIGURATION,
        
        /// <remarks/>
        DEVICE_HOSTNAME_TEST,
        
        /// <remarks/>
        DEVICE_INVALID_DNS_TEST,
        
        /// <remarks/>
        DEVICE_INVALID_HOSTNAME_TEST,
        
        /// <remarks/>
        DEVICE_INVALID_IP_NTP_TEST,
        
        /// <remarks/>
        DEVICE_INVALID_NAME_NTP_TEST,
        
        /// <remarks/>
        DEVICE_MEDIA_CAPABILITIES,
        
        /// <remarks/>
        DEVICE_NTP_CONFIGURATION,
        
        /// <remarks/>
        DEVICE_NTP_TEST,
        
        /// <remarks/>
        DEVICE_RESET,
        
        /// <remarks/>
        DEVICE_SERVICE_CATEGORY_CAPABILITIES,
        
        /// <remarks/>
        DEVICE_SOAP_FAULT_MESSAGE,
        
        /// <remarks/>
        DEVICE_SYSTEM_DATE_AND_TIME,
        
        /// <remarks/>
        DEVICE_SYSTEM_DATE_AND_TIME_INVALID_DATE_TEST,
        
        /// <remarks/>
        DEVICE_SYSTEM_DATE_AND_TIME_INVALID_TIMEZONE_TEST,
        
        /// <remarks/>
        DEVICE_SYSTEM_DATE_AND_TIME_TEST,
        
        /// <remarks/>
        DEVICE_WSDL_URL,
        
        /// <remarks/>
        DISCOVERY_BYE_MESSAGE,
        
        /// <remarks/>
        DISCOVERY_DEVICE_SCOPES_CONFIGURATION,
        
        /// <remarks/>
        DISCOVERY_MULTICAST_HELLO,
        
        /// <remarks/>
        DISCOVERY_MULTICAST_HELLO_VALIDATE,
        
        /// <remarks/>
        DISCOVERY_MULTICAST_SCOPE_SEARCH,
        
        /// <remarks/>
        DISCOVERY_MULTICAST_SCOPE_SEARCH_INVALID,
        
        /// <remarks/>
        DISCOVERY_MULTICAST_SCOPE_SEARCH_OMITTED_DEVICE,
        
        /// <remarks/>
        DISCOVERY_SOAP_FAULT_MESSAGE,
        
        /// <remarks/>
        DISCOVERY_UNICAST_SCOPE_SEARCH,
        
        /// <remarks/>
        DISCOVERY_UNICAST_SCOPE_SEARCH_INVALID,
        
        /// <remarks/>
        DISCOVERY_UNICAST_SCOPE_SEARCH_OMITTED_DEVICE,
        
        /// <remarks/>
        MEDIA_DYNAMIC_MEDIA_PROFILE_CONFIGURATION,
        
        /// <remarks/>
        MEDIA_INVALID_TRANSPORT_SOAP_FAULT_MESSAGE,
        
        /// <remarks/>
        MEDIA_JPEG_VIDEO_ENCODER_CONFIGURATION,
        
        /// <remarks/>
        MEDIA_PROFILE_CONFIGURATION,
        
        /// <remarks/>
        MEDIA_SOAP_FAULT_MESSAGE,
        
        /// <remarks/>
        MEDIA_STREAM_URI__RTP_RTSP_HTTP,
        
        /// <remarks/>
        MEDIA_STREAM_URI__RTP_UDP_UNICAST,
        
        /// <remarks/>
        RTS_RTP_RTSP_HTTP,
        
        /// <remarks/>
        RTS_RTP_UDP_UNICAST,
        
        /// <remarks/>
        RTS_RTSP_KEEPALIVE,
        
        /// <remarks/>
        RTS_RTSP_TCP,
    }
}