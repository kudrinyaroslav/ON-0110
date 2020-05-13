using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.CommonUtils.Comparison;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class ReplayUriTestSuite : ReplayTest
    {

        public ReplayUriTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Replay\\Get URI";

        [Test(Name = "GETREPLAYURI COMMAND WITH INVALID RECORDING TOKEN",
            Order = "01.02.01",
            Id = "1-2-1",
            Category = Category.REPLAY,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.ReplayService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetReplayUri  })]
        public void ReplayServiceInvalidTokenTest()
        {
            RunTest(() =>
            {
                StreamSetup streamSetup = new StreamSetup();
                streamSetup.Transport = new Transport();
                streamSetup.Transport.Protocol = TransportProtocol.UDP;
                streamSetup.Stream = StreamType.RTPUnicast;

                RunStep(
                    () =>
                        {
                            Client.GetReplayUri(streamSetup, Guid.NewGuid().ToString().Substring(0, 8));
                        //}, "GetReplayURI - invalid token", OnvifFaults.InvalidToken, true);
                        // [AR] fix for wush 189
                        }, "GetReplayURI - invalid token", OnvifFaults.NoRecording, true);
                
            });
        }
    }

}
