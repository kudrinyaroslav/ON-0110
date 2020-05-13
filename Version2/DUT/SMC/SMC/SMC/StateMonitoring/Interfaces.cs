using SMC.Proxies;

namespace SMC.StateMonitoring
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.onvif.org/simulator/StateReportReceiver", ConfigurationName = "StateReportReceiver.StateReportReceiverSoap")]
    public interface StateReportReceiverSoap
    {
        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/simulator/StateReportReceiver/UpdateState", IsOneWay = true)]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DoorState))]
        void UpdateState(UpdateStateRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.onvif.org/simulator/StateReportReceiver/CloseConnection", ReplyAction = "*")]
        CloseConnectionResponse CloseConnection(CloseConnectionRequest request);
    }

    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    [System.ServiceModel.XmlSerializerFormatAttribute()]
    public partial class UpdateStateRequest
    {
        [System.ServiceModel.MessageBodyMemberAttribute(Name = "UpdateState", Namespace = "http://www.onvif.org/simulator/StateReportReceiver", Order = 0)]
        public UpdateStateRequestBody Body;

        public UpdateStateRequest()
        {
        }

        public UpdateStateRequest(UpdateStateRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://www.onvif.org/simulator/StateReportReceiver")]
    [System.ServiceModel.XmlSerializerFormatAttribute()]
    public partial class UpdateStateRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string token;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 1)]
        public DoorState state;

        public UpdateStateRequestBody()
        {
        }

        public UpdateStateRequestBody(string Token, DoorState State)
        {
            this.token = Token;
            this.state = State;
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface StateReportReceiverSoapChannel : StateReportReceiverSoap, System.ServiceModel.IClientChannel
    {
    }

}
