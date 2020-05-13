using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DUT.CameraWebService;

namespace DUT.CameraWebService.Events10
{
    /// <summary>
    /// Summary description for NotificationProducer
    /// </summary>
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/events/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NotificationProducer : NotificationProducerBinding
    {


        public override SubscribeResponse Subscribe(Subscribe Subscribe1)
        {
            throw new NotImplementedException();
        }

        public override GetCurrentMessageResponse GetCurrentMessage(GetCurrentMessage GetCurrentMessage1)
        {
            throw new NotImplementedException();
        }
    }
}
