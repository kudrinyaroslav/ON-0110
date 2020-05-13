using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DUT.PACS.Simulator.Configuration
{
    /// <summary>
    /// Replacement for Dictionary.
    /// </summary>
    [XmlRoot("DoorState")]
    public class DoorStateHolder
    {
        public string DoorToken { get; set; }
        public ServiceDoorControl10.DoorState State { get; set; }
    }

    /// <summary>
    /// Replacement for Dictionary.
    /// </summary>
    [XmlRoot("CredentialState")]
    public class CredentialStateHolder
    {
        public string CredentialToken { get; set; }
        public ServiceCredential10.CredentialState State { get; set; }
    }

    [XmlRoot("AccessPointState")]
    public class AccessPointStateHolder
    {
        public string AccessPointToken { get; set; }
        public ServiceAccessControl10.AccessPointState State { get; set; }
    }

    [XmlRoot("AccessPointTamperingState")]
    public class AccessPointTamperingStateHolder
    {
        public string AccessPointToken { get; set; }
        public bool State { get; set; }
    }

    /// <summary>
    /// Serializable configuration storage.
    /// </summary>
    [XmlRoot("Configuration")]
    public class SerializableConfiguration
    {
        public List<ServiceAccessControl10.AreaInfo> AreaInfoList { get; set; }
        public List<ServiceAccessControl10.AccessPointInfo> AccessPointInfoList { get; set; }
        public List<ServiceDoorControl10.DoorInfo> DoorInfoList { get; set; }
        //public List<ServiceCredential10.Credential> CredentialList { get; set; }
        //public List<ServiceAccessRules10.AccessProfile> AccessProfileList { get; set; }

        [XmlArrayItem("AccessPointState")]
        public List<AccessPointStateHolder> AccessPointInitialStates { get; set; }

        [XmlArrayItem("DoorState")]
        public List<DoorStateHolder> DoorInitialStates { get; set; }

        [XmlArrayItem("AccessPointTamperingState")]
        public List<AccessPointTamperingStateHolder> AccessPointTamperingInitialState { get; set; }
        

        //[XmlArrayItem("CredentialState")]
        //public List<CredentialStateHolder> CredentialInitialStates { get; set; }

        [XmlArrayItem("TriggerSettings")]
        public List<TriggerSettings> TriggerConfiguration { get; set; }

        //public List<CredentialInformation> CredentialInformation { get; set; }

    }

}
