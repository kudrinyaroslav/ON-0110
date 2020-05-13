using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMC.Proxies;

namespace SMC.Pages
{
    public class BaseSensorClientControl : BaseSmcControl
    {
        #region SensorService

        private SensorServiceSoapClient _sensorServiceClient;

        protected SensorServiceSoapClient SensorServiceClient
        {
            get
            {
                if (_sensorServiceClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.SensorServiceAddress);
                    _sensorServiceClient = new SensorServiceSoapClient(binding, address);
                }
                return _sensorServiceClient;
            }

        }

        #endregion

        #region DoorControlService

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

        #endregion

        #region CredentialService

        private CredentialPortClient _credentialClient;

        protected CredentialPortClient CredentialClient
        {
            get
            {
                if (_credentialClient == null)
                {
                    System.ServiceModel.BasicHttpBinding binding = GetBinding();
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(
                        SMC.SmcData.Context.Instance.General.CredentialServiceAddress);
                    _credentialClient = new CredentialPortClient(binding, address);
                }
                return _credentialClient;
            }

        }

        #endregion

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

            if (_credentialClient != null)
            {
                string address = SMC.SmcData.Context.Instance.General.CredentialServiceAddress;
                if (address != _credentialClient.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _credentialClient = null;
                }
            }

            if (_sensorServiceClient != null)
            {
                string address = SMC.SmcData.Context.Instance.General.SensorServiceAddress;
                if (address != _sensorServiceClient.Endpoint.Address.Uri.AbsoluteUri)
                {
                    _sensorServiceClient = null;
                }
            }
        }
    }
}
