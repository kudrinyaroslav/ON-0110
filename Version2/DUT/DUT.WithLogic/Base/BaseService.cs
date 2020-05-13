using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUT.WithLogic.Base
{
    public class BaseService : System.Web.Services.WebService
    {

        protected Engine.ONVIFServiceList ONVIFServiceList
        {
            get
            {
                if (Application[AppVars.ONVIFSERVICELIST] != null)
                {
                    return (Engine.ONVIFServiceList)Application[AppVars.ONVIFSERVICELIST];
                }
                else
                {
                    Engine.ONVIFServiceList onvifServiceList = new Engine.ONVIFServiceList(ONVIFConfiguration.ONVIFDeviceManagementCapabilities, ONVIFConfiguration.ONVIFMedia2Capabilities);
                    Application[AppVars.ONVIFSERVICELIST] = onvifServiceList;
                    return onvifServiceList;
                }
            }
        }

        protected Engine.ONVIFConfiguration ONVIFConfiguration
        {
            get
            {
                if (Application[AppVars.ONVIFCONFIGURATION] != null)
                {
                    return (Engine.ONVIFConfiguration)Application[AppVars.ONVIFCONFIGURATION];
                }
                else
                {
                    Engine.ONVIFConfiguration onvifConfiguration = new Engine.ONVIFConfiguration();
                    Application[AppVars.ONVIFCONFIGURATION] = onvifConfiguration;
                    return onvifConfiguration;
                }
            }
        }

    }
}