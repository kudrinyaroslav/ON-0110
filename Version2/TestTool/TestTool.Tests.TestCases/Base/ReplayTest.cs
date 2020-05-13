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
    public class ReplayTest : CommonServiceTest<ReplayPort, ReplayPortClient>
    {
        public ReplayTest(TestLaunchParam param)
            : base(param)
        {

        }

        protected override string ServiceName
        {
            get { return "Replay"; }
        }

        protected override string GetServiceAddress()
        {
            string address = string.Empty;
            RunStep(() =>
            {
                DeviceClient device = DeviceClient;

                address = device.GetReplayServiceAddress(Features);

                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException("The DUT did not return Replay service address");
                }

            }, "Get Replay Service address");
            DoRequestDelay();
            return address;
        }

        protected override ReplayPortClient CreateClient(string address)
        {
            Binding binding = CreateBinding(false,
                    new IChannelController[] { new SoapValidator(ReplaySchemasSet.GetInstance()) });
            ReplayPortClient client = new ReplayPortClient(binding, new EndpointAddress(address));
            return client;
        }
    }
}
