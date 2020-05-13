using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using DUT.PACS.Simulator.Discovery;

namespace DUT.PACS.Simulator
{
    public class Global : System.Web.HttpApplication
    {
        private DUT.PACS.Simulator.Discovery.Discovery _discovery = null;

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (_discovery == null)
            {
                Uri uri = HttpContext.Current.Request.Url;
                string xAddr = uri.Scheme + Uri.SchemeDelimiter + "{0}" + ":" + uri.Port + "/onvif/device_service";
                _discovery = new DUT.PACS.Simulator.Discovery.Discovery(xAddr);
                Application["Discovery"] = _discovery;
            }
            
            if (HttpContext.Current.Request.Path.EndsWith("/onvif/device_service") ||
                HttpContext.Current.Request.Path.EndsWith("/onvif/device_service/"))
            {
                HttpContext.Current.RewritePath("~/ServiceDevice10/DeviceService.asmx", false);
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            ExternalLogging.LoggingService service = null;
            if (Application[Common.AppVars.LOGGINGSERVICE] != null)
            {
                service = (ExternalLogging.LoggingService)Application[Common.AppVars.LOGGINGSERVICE];
                service.Stop();
            }
            
            ExternalLogging.StateReportingService reportingService= null;
            if (Application[Common.AppVars.STATEREPORTER] != null)
            {
                reportingService = (ExternalLogging.StateReportingService)Application[Common.AppVars.STATEREPORTER];
                reportingService.Stop();
            }

        }
    }
}