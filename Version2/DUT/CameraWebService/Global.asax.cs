using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using CameraWebService.Discovery;
using System.IO;
using System.Configuration;

namespace DUT.CameraWebService
{
    public class Global : System.Web.HttpApplication
    {
        //private Discovery _discovery = null;
        private static Boolean IsClearedLocalAuthFile = false;
        
        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string path = HttpContext.Current.Request.Url.AbsolutePath;
            if (path.EndsWith("/onvif/device_service") || path.EndsWith("/onvif/device_service/"))
            {
                HttpContext.Current.RewritePath("~/ServiceDevice10/DeviceServiceFake.asmx", false);
            }

            //if (_discovery == null)
            //{
            //    Uri uri = HttpContext.Current.Request.Url;
            //    string xAddr = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port + "/ServiceDevice10/DeviceServiceFake.asmx";
            //    _discovery = new Discovery(xAddr);
            //    Application["Discovery"] = _discovery;
            //}


            #region Change Auth Mode for Future Requests

            if (!IsClearedLocalAuthFile)
            {
                IsClearedLocalAuthFile = true;
                string userFileVpath = System.Configuration.ConfigurationManager.AppSettings["Digest.Samples.DigestAuthenticationModule_LocalPublicMethods"];
                string userFileName = this.Request.MapPath(userFileVpath);

                if (File.Exists(userFileName))
                {
                    File.Delete(userFileName);
                }

                // clearing extra users file

                userFileVpath = System.Configuration.ConfigurationManager.AppSettings["Digest.Samples.DigestAuthenticationModule_ExtraUsersFiles"];
                userFileName = this.Request.MapPath(userFileVpath);

                if (File.Exists(userFileName))
                {
                    File.Delete(userFileName);
                }
            }

            #endregion //Change Auth Mode for Future Requests
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

        }
    }
}