using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMC.Proxies;

namespace SMC.Pages
{
    public class BaseDoorClientControl : BaseSmcControl
    {
        private DoorControlPortClient _doorControlClient;

        protected DoorControlPortClient DoorControlClient
        {
            get
            {
                if (_doorControlClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.ManagementServiceAddress);
                    _doorControlClient = new DoorControlPortClient(binding, address);
                }
                return _doorControlClient;
            }

        }

        protected void CheckClientValid()
        {
            if (_doorControlClient != null)
            {
                string address = SMC.SmcData.Context.Instance.General.ManagementServiceAddress ;
                if (address != _doorControlClient.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _doorControlClient = null;
                }
            }
        }
    }
}
