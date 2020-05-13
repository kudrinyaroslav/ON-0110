using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMC.Logging
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.onvif.org/simulator/logreceiver", ConfigurationName = "LogReceiver.LogReceiverSoap")]
    public interface LogReceiverSoap
    {

        // CODEGEN: Generating message contract since element name message from namespace http://www.onvif.org/simulator/logreceiver is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/simulator/logreceiver/LogMessage", IsOneWay = true)]
        void LogMessage(LogMessageRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/simulator/logreceiver/CloseConnection", ReplyAction = "*")]
        CloseConnectionResponse CloseConnection(CloseConnectionRequest request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class LogMessageRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "LogMessage", Namespace = "http://www.onvif.org/simulator/logreceiver", Order = 0)]
        public LogMessageRequestBody Body;

        public LogMessageRequest()
        {
        }

        public LogMessageRequest(LogMessageRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://www.onvif.org/simulator/logreceiver")]
    public partial class LogMessageRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string message;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 1)]
        public MessageType type;

        public LogMessageRequestBody()
        {
        }

        public LogMessageRequestBody(string message, MessageType type)
        {
            this.message = message;
            this.type = type;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CloseConnectionRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CloseConnection", Namespace = "http://www.onvif.org/simulator/logreceiver", Order = 0)]
        public CloseConnectionRequestBody Body;

        public CloseConnectionRequest()
        {
        }

        public CloseConnectionRequest(CloseConnectionRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://www.onvif.org/simulator/logreceiver")]
    public partial class CloseConnectionRequestBody
    {
        public CloseConnectionRequestBody()
        {
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CloseConnectionResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CloseConnectionResponse", Namespace = "http://www.onvif.org/simulator/logreceiver", Order = 0)]
        public CloseConnectionResponseBody Body;

        public CloseConnectionResponse()
        {
        }

        public CloseConnectionResponse(CloseConnectionResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class CloseConnectionResponseBody
    {

        public CloseConnectionResponseBody()
        {
        }
    }
    


}
