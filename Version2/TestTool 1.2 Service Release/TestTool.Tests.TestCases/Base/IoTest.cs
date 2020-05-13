///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using TestTool.HttpTransport;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.SoapValidation;
using TestTool.Tests.Common.TestBase;
using System.ServiceModel;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.TestCases.Utils.SoapValidation;

namespace TestTool.Tests.TestCases.Base
{
    public class IoTest : BaseServiceTest<DeviceIOPort , DeviceIOPortClient>
    {
        private DeviceClient _deviceClient;


        public IoTest(TestLaunchParam param)
            : base(param)
        {
        }

        protected override DeviceIOPortClient CreateClient()
        {
            HttpTransport.HttpBinding binding = 
                (HttpTransport.HttpBinding)CreateBinding(false,
                new IChannelController[] { new SoapValidator(DeviceIoSchemaSet.GetInstance()) });
            DeviceIOPortClient client = new DeviceIOPortClient(binding, new EndpointAddress(GetIoServiceAdderss()));
            return client;
        }

        protected Capabilities GetCapabilities(CapabilityCategory[] categories, string stepName)
        {
            Capabilities capabilities = null;
            RunStep(() => { capabilities = DeviceClient.GetCapabilities(categories); }, stepName);
            DoRequestDelay();
            return capabilities;
        }

        protected Capabilities GetCapabilities(CapabilityCategory[] categories)
        {
            return GetCapabilities(categories, "Get capabilities");
        }

        private DeviceClient DeviceClient
        {
            get
            {
                if (_deviceClient == null)
                {
                    HttpTransport.HttpBinding binding =
                        (HttpTransport.HttpBinding)CreateBinding(false,
                        new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                    _deviceClient =
                        new DeviceClient(binding, new EndpointAddress(_cameraAddress));

                    AttachSecurity(_deviceClient.Endpoint);
                    SetupChannel(_deviceClient.InnerChannel);

                }

                return _deviceClient;
            }
        }

        /// <summary>
        /// Returns DUT's imaging service address
        /// </summary>
        /// <returns>Imaging service url</returns>
        protected string GetIoServiceAdderss()
        {
            string address = string.Empty;
            RunStep(() =>
            {

                Capabilities capabilities =
                    DeviceClient.GetCapabilities(
                        new CapabilityCategory[]
                            {
                                CapabilityCategory.All
                        });

                DoRequestDelay();

                if (capabilities.Extension != null)
                {
                    if (capabilities.Extension.DeviceIO != null)
                    {
                        address = capabilities.Extension.DeviceIO.XAddr;
                        LogStepEvent(address);
                    }
                }
                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException("IO capabilities not found");
                }

            }, "Get Device IO service address");
            return address;
        }


        protected RelayOutput[] GetRelayOutputs()
        {
            return GetRelayOutputs("Get relay outputs");
        }

        protected RelayOutput[] GetRelayOutputs(string stepName)
        {
            RelayOutput[] outputs = null;
            RunStep(() => { outputs = Client.GetRelayOutputs(); }, stepName);
            DoRequestDelay();
            return outputs;
        }

        protected void SetRelayOutputSettings(RelayOutput output)
        {
            SetRelayOutputSettings(output, "Set relay output settings");
        }

        protected void SetRelayOutputSettings(RelayOutput output, string stepName)
        {
            RunStep(() => { Client.SetRelayOutputSettings(output); }, stepName);
            DoRequestDelay();
        }

        protected void SetRelayOutputState(string token, RelayLogicalState state)
        {
            SetRelayOutputState(token, state, "Set relay output state");
        }

        protected void SetRelayOutputState(string token, RelayLogicalState state, string stepName)
        {
            RunStep(() => { Client.SetRelayOutputState(token, state); }, stepName);
            DoRequestDelay();
        }



    }
}
