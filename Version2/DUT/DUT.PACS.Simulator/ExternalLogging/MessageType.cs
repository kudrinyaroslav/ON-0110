
namespace DUT.PACS.Simulator.ExternalLogging
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "MessageType", Namespace = "http://www.onvif.org/simulator/logreceiver")]
    public enum MessageType : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Error = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Warning = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Message = 2,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Details = 3,
    }

}