///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.CommonUtils.MessageModification;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using System.ServiceModel;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class MediaErrorHandlingTestSuite : Base.MediaTest
    {
        public MediaErrorHandlingTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Media Configuration\\Error Handling"; 

        [Test(Name = "SOAP FAULT MESSAGE", 
            Path =  PATH,
            Order = "07.01.04",
            Id = "7-1-4",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must, 
            RequiredFeatures = new Feature[]{Feature.MediaService, Feature.RTSS})]
        public void FaultMessageForInvalidProfileTest()
        {
            RunTest( () =>
                         {
                             Profile[] profiles = GetProfiles();
                             Assert(profiles != null, "No profiles returned", "Check if DUT returned profiles");
                             
                             var invalidProfileToken = profiles.Select(e => e.token).GetNonMatchingString();

                             // get stream uri

                             StreamSetup setup = new StreamSetup();
                             setup.Transport = new Transport();
                             setup.Transport.Protocol = TransportProtocol.UDP;
                             setup.Stream = StreamType.RTPUnicast;

                             RunStep(() => Client.GetStreamUri(setup, invalidProfileToken),
                                     "Get Stream URI - negative test",
                                     "Sender/InvalidArgVal/NoProfile"); // strict check
                             DoRequestDelay();
                         });
        }
        
        [Test(Name = "SOAP FAULT MESSAGE", 
            Path =  PATH,
            Order = "07.01.02",
            Id = "7-1-2",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS })]
        public void FaultMessageForInvalidRequestTest()
        {
            RunTest( () =>
                         {
                             Profile[] profiles = GetProfiles();
                             Assert((profiles != null && profiles.Length > 0), "No profile available", "Check if DUT returned at least one profile");

                             StreamSetup setup = new StreamSetup();
                             setup.Transport = new Transport();
                             setup.Transport.Protocol = TransportProtocol.UDP;
                             setup.Stream = StreamType.RTPUnicast;

                             MessageSpoiler spoiler = new MessageSpoiler();

                             Dictionary<string, string> namespaces = new Dictionary<string, string>();
                             namespaces.Add("s", "http://www.w3.org/2003/05/soap-envelope");
                             namespaces.Add("media", "http://www.onvif.org/ver10/media/wsdl");
                             namespaces.Add("onvif", "http://www.onvif.org/ver10/schema");

                             Dictionary<string, string> replacements = new Dictionary<string, string>();
                             replacements.Add("/s:Envelope/s:Body/media:GetStreamUri/media:StreamSetup/onvif:Transport/onvif:Protocol", "RTP");

                             spoiler.Namespaces = namespaces;
                             spoiler.NodesToReplace = replacements;

                             SetBreakingBehaviour(spoiler);

                             try
                             {
                                 GetStreamUri(setup, profiles[0].token);
                             }
                            catch (FaultException exception)
                            {
                                bool fault = exception.IsValidOnvifFault();
                                SaveStepFault(exception);
                                if (!fault)
                                {
                                    LogStepEvent("A SOAP 1.2 fault message is invalid");
                                }
                                StepPassed();
                            }                             

                             ResetBreakingBehaviour();

                         });
        }
    
        [ Test( Name = "START MULTICAST - INVALID PROFILE TOKEN",
                Path = PATH,
                Order = "07.01.05",
                Id = "7-1-5",
                Category = Category.MEDIA,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must,
                RequiredFeatures = new Feature[] { Feature.MediaService, Feature.RTSS, Feature.RTPMulticastUDP },
                FunctionalityUnderTest = new Functionality[] { Functionality.StartMulticastStreaming})]
        public void NvtStartMulticastInvalidProfileToken()
        {
            RunTest(
            () =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");

                List<string> tokens = new List<string>();
                foreach (Profile profile in profiles)
                {
                    tokens.Add(profile.token);
                }

                string token = tokens.GetNonMatchingString();

                RunStep(
                    () => { Client.StartMulticastStreaming(token); },
                    "StartMulticastStreaming - negative test",
                    "Sender/InvalidArgVal/NoProfile",
                    true);
                DoRequestDelay();
            });
        }
    }
    

}
