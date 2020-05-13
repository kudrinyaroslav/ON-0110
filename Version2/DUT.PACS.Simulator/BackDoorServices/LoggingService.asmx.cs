using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DUT.PACS.Simulator.ExternalLogging;

namespace DUT.PACS.Simulator.BackDoorServices
{
    /// <summary>
    /// Summary description for LoggingService
    /// </summary>
    [WebService(Namespace = "http://www.onvif.org/simulator/logging")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LoggingService : System.Web.Services.WebService
    {
        [WebMethod]
        public Guid Subscribe(string receiver, MessageType messageType)
        {
            return ExternalLogger.Subscribe(receiver, messageType);
        }

        [WebMethod]
        public void Unsubscribe(Guid receiver)
        {
            ExternalLogger.Unsubscribe(receiver);
        }

        protected ExternalLogging.LoggingService ExternalLogger
        {
            get
            {
                ExternalLogging.LoggingService service = null;
                if (Application[Common.AppVars.LOGGINGSERVICE] != null)
                {
                    service = (ExternalLogging.LoggingService)Application[Common.AppVars.LOGGINGSERVICE];
                }
                else
                {
                    service = new ExternalLogging.LoggingService();
                    Application[Common.AppVars.LOGGINGSERVICE] = service;
                }

                return service;
            }
        }

    }




}
