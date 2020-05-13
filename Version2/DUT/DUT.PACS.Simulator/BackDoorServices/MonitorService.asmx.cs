using System;
using System.Web.Services;
using DUT.PACS.Simulator.Common;

namespace DUT.PACS.Simulator.BackDoorServices
{
    /// <summary>
    /// Summary description for MonitorService
    /// </summary>
    [WebService(Namespace = "http://www.onvif.org/simulator/StateReporting")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MonitorService : BaseDutService
    {
        [WebMethod]
        public Guid Subscribe(string receiver)
        {
            return StateReporter.Subscribe(receiver);
        }

        [WebMethod]
        public void Unsubscribe(Guid receiver)
        {
            StateReporter.Unsubscribe(receiver);
        }

    }
}
