using System.ServiceModel.Channels;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.SoapValidation;
using System.ServiceModel;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Exceptions;

namespace TestTool.Tests.TestCases.Base
{
    public class AccessControlTest : BaseServiceTest<PACSPort, PACSPortClient>
    {
        public AccessControlTest(TestLaunchParam param)
            : base(param)
        {

        }

        protected override PACSPortClient CreateClient()
        {
            Binding binding = 
                CreateBinding(false, 
                new IChannelController[] {new SoapValidator(AccessControlSchemaSet.GetInstance())} );
            PACSPortClient client = new PACSPortClient(binding, new EndpointAddress(GetAccessControlServiceAdderss()));
            return client;
        }

        private DeviceClient _deviceClient;

        private DeviceClient DeviceClient
        {
            get
            {
                if (_deviceClient == null)
                {
                    Binding binding =
                        CreateBinding(false,
                        new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                    _deviceClient =
                        new DeviceClient(binding, new EndpointAddress(_cameraAddress));

                    AttachSecurity(_deviceClient.Endpoint);
                    SetupChannel(_deviceClient.InnerChannel);

                }

                return _deviceClient;
            }
        }

        protected override void Release()
        {
            base.Release();
            if (_deviceClient != null)
            {
                _deviceClient.Close();
            }
        }
        /// <summary>
        /// Returns DUT's access control service address
        /// </summary>
        /// <returns>Access control service url</returns>
        protected string GetAccessControlServiceAdderss()
        {
            string address = string.Empty;
            RunStep(() =>
            {
                address = DeviceClient.GetServiceAddress(TestTool.Tests.Definitions.Onvif.OnvifService.ACCESSCONTROL);

                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException("PACS service not found");
                }
                else
                {
                    LogStepEvent(address);
                }

            }, "Get PACS service address");
            DoRequestDelay();
            return address;
        }



        protected AccessControlServiceCapabilities GetServiceCapabilities()
        {
            AccessControlServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = Client.GetServiceCapabilities(); }, "Get Service Capabilities");
            return capabilities;
        }

        protected AccessPointState GetAccessPointState(string token)
        {
            return GetAccessPointState(token, string.Format("Get AccessPointState for token='{0}'", token));
        }

        protected AccessPointState GetAccessPointState(string token, string stepName)
        {
            AccessPointState info = null;
            RunStep(() => { info = Client.GetAccessPointState(token); }, stepName);
            DoRequestDelay();
            return info;
        }


        protected AccessPointInfo[] GetAccessPointInfo(string[] tokensList)
        {
            return GetAccessPointInfo(tokensList, "Get AccessPointInfo");
        }

        protected AccessPointInfo[] GetAccessPointInfo(string[] tokensList, string stepName)
        {
            AccessPointInfo[] infos = null;
            RunStep(() => { infos = Client.GetAccessPointInfo(tokensList); }, stepName);
            DoRequestDelay();
            return infos;
        }

        protected string GetAccessPointInfoList(int? limit, string offset, out AccessPointInfo[] list)
        {
            return GetAccessPointInfoList(limit, offset, out list, "Get AccessPointInfo list");
        }

        protected string GetAccessPointInfoList(int? limit, string offset, out AccessPointInfo[] list, string stepName)
        {
            string nextReference = null;
            AccessPointInfo[] infos = null;
            RunStep(() => { nextReference = Client.GetAccessPointInfoList(limit, offset, out infos); }, stepName);
            DoRequestDelay();
            list = infos;
            return nextReference;
        }

        protected string GetAreaInfoList(int? limit, string offset, out AreaInfo[] list)
        {
            return GetAreaInfoList(limit, offset, out list, "Get AreaInfo list");
        }

        protected string GetAreaInfoList(int? limit, string offset, out AreaInfo[] list, string stepName)
        {
            string nextReference = null;
            AreaInfo[] infos = null;

            RunStep(() => { nextReference = Client.GetAreaInfoList(limit, offset, out infos); }, stepName);
            DoRequestDelay();
            list = infos;

            return nextReference;
        }

        protected AreaInfo[] GetAreaInfo(string[] tokensList)
        {
            return GetAreaInfo(tokensList, "Get AreaInfo");
        }

        protected AreaInfo[] GetAreaInfo(string[] tokensList, string stepName)
        {
            AreaInfo[] infos = null;
            RunStep(() => { infos = Client.GetAreaInfo(tokensList); }, stepName);
            DoRequestDelay();
            return infos;
        }
        
        protected void EnableAccessPoint(string token)
        {
            EnableAccessPoint(token, string.Format("Enable AccessPoint with token='{0}'", token));
        }

        protected void EnableAccessPoint(string token, string stepName)
        {
            RunStep(() => { Client.EnableAccessPoint(token);}, stepName);
            DoRequestDelay();
        }


        protected void DisableAccessPoint(string token)
        {
            DisableAccessPoint(token, string.Format("Disable AccessPoint with token='{0}'", token));
        }

        protected void DisableAccessPoint(string token, string stepName)
        {
            RunStep(() => { Client.DisableAccessPoint(token); }, stepName);
            DoRequestDelay();
        }

    }
}
