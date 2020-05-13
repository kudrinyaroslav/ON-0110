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
    /// <summary>
    /// Wrapper for ReplayPortClient to be used as base test
    /// </summary>
    public class ReplayTest : BaseServiceTest<ReplayPort, ReplayPortClient>
    {
        public ReplayTest(TestLaunchParam param)
            : base(param)
        {

        }

        protected override ReplayPortClient CreateClient()
        {
            string address = GetReplayServiceAddress();

            BeginStep("Connect to Replay service");
            LogStepEvent(string.Format("Replay service address: {0}", address));
            if (!address.IsValidUrl())
            {
                throw new AssertException("Replay service address is invalid");
            }
            Binding binding = CreateBinding(false,
                    new IChannelController[] { new SoapValidator(ReplaySchemasSet.GetInstance()) });
            ReplayPortClient client = new ReplayPortClient(binding, new EndpointAddress(address));
            StepPassed();
            return client;
        }

        /// <summary>
        /// Returns DUT's media service address
        /// </summary>
        /// <returns>Replay service url</returns>
        protected string GetReplayServiceAddress()
        {
            string address = string.Empty;
            RunStep(() =>
            {
                Binding binding =
                    CreateBinding(false,
                    new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                DeviceClient device = new DeviceClient(binding, new EndpointAddress(_cameraAddress));

                AttachSecurity(device.Endpoint);
                SetupChannel(device.InnerChannel);

                address = device.GetReplayServiceAddress(Features);

                device.Close();

                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException("The DUT did not return Replay service address");
                }

            }, "Get Replay Service address");
            DoRequestDelay();
            return address;
        }


    }
}
