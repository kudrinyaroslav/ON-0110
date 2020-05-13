using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUT.PACS.Simulator.ExternalLogging
{

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
