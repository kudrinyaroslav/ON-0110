using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.CommonUtils.Comparison;
using TestTool.Tests.Definitions.Exceptions;


namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public partial class ReceiverServiceTestSuite : ReceiverTest
    {
        public ReceiverServiceTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        private const string PATH = "Receiver\\Capabilities";

        [Test(Name = "RECEIVER SERVICE CAPABILITIES",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.RECEIVER,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetReceiverServiceCapabilities })]
        public void SearchServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                // Initialize client
                ReceiverServiceCapabilities capabilities = GetServiceCapabilities();

                BeginStep("Validate Service Capabilities");

                bool ok = true;
                StringBuilder dump = new StringBuilder("Capabilities are incorrect" + Environment.NewLine);
                if (capabilities.SupportedReceivers <= 1)
                {
                    dump.Append(string.Format("   SupportedReceivers is incorrect ({0}){1}", capabilities.SupportedReceivers, Environment.NewLine));
                    ok = false;
                }
                if (capabilities.MaximumRTSPURILengthSpecified && capabilities.MaximumRTSPURILength <= 128)
                {
                    dump.Append(string.Format("   MaximumRTSPURILength is incorrect ({0}){1}", capabilities.MaximumRTSPURILength, Environment.NewLine));
                    ok = false;
                }

                if (!ok)
                {
                    throw new AssertException(dump.ToStringTrimNewLine());
                }

                StepPassed();
            });
        }

        [Test(Name = "GET SERVICES AND GET RECEIVER SERVICE CAPABILITIES CONSISTENCY",
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.RECEIVER,
            Path = PATH,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.ReceiverService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetReceiverServiceCapabilities })]
        public void SearchServicesAndRecieverServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                Service[] services = GetServices(true);

                Service recieverService = services.FindService(Definitions.Onvif.OnvifService.RECEIVER);



                Assert(recieverService != null, "No Reciever service information returned", "Check that the DUT returned Reciever service information");

                Assert((recieverService.Capabilities != null), "No Capabilities information included",
                       "Check that Capabilities element is included in Services element");

                ReceiverServiceCapabilities serviceCapabilities = ParseReceiverCapabilities(recieverService.Capabilities);

                ReceiverServiceCapabilities capabilities = GetServiceCapabilities();

                 CompareCapabilities(serviceCapabilities, capabilities);
            });
        }
    }
}
