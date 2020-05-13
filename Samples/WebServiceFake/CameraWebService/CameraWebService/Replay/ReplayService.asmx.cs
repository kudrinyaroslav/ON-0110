using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CameraWebService.Replay
{
    /// <summary>
    /// Summary description for ReplayService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ReplayService : ReplayBinding
    {

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/replay/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/replay/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/replay/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override Capabilities GetServiceCapabilities()
        {
            return new Capabilities(){ReversePlayback = true};
        }

        public override string GetReplayUri(StreamSetup StreamSetup, string RecordingToken)
        {
            throw new NotImplementedException();
        }

        public override ReplayConfiguration GetReplayConfiguration()
        {
            throw new NotImplementedException();
        }

        public override void SetReplayConfiguration(ReplayConfiguration Configuration)
        {
            throw new NotImplementedException();
        }
    }
}
