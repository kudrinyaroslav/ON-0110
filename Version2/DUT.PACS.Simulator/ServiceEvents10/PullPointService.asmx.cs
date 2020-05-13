using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace DUT.PACS.Simulator.Events10
{
    /// <summary>
    /// Summary description for PullPointService
    /// </summary>
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/events/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PullPointService : PullPointBinding
    {


        public override GetMessagesResponse GetMessages(GetMessages GetMessages1)
        {
            throw new NotImplementedException();
        }

        public override DestroyPullPointResponse DestroyPullPoint(DestroyPullPoint DestroyPullPoint1)
        {
            throw new NotImplementedException();
        }

        public override void Notify(Notify Notify1)
        {
            throw new NotImplementedException();
        }
    }
}
