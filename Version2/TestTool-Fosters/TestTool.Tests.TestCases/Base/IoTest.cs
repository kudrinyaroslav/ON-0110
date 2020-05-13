///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.ServiceModel.Channels;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.CommonUtils.SoapValidation;
using System.ServiceModel;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.Base
{
    public class IoTest : BaseServiceTest<DeviceIOPort , DeviceIOPortClient>
    {
        private DeviceClient _deviceClient;


        public IoTest(TestLaunchParam param)
            : base(param)
        {
        }

        protected override void Release()
        {
            if (_deviceClient != null)
            {
                _deviceClient.Close();
            }

            base.Release();
        }

        protected override DeviceIOPortClient CreateClient()
        {
            Binding binding = 
                CreateBinding(false,
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

        /// <summary>
        /// Returns DUT's imaging service address
        /// </summary>
        /// <returns>Imaging service url</returns>
        protected string GetIoServiceAdderss()
        {
            string address = string.Empty;
            RunStep(() =>
            {
                address = DeviceClient.GetIoServiceAddress(Features);

                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException("IO capabilities not found");
                }
                else
                {
                    LogStepEvent(address);
                }

            }, "Get Device IO service address");
            DoRequestDelay();

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
