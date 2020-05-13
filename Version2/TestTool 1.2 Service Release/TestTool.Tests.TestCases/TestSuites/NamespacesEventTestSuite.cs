///////////////////////////////////////////////////////////////////////////
//!  @author        Ilya Gozman
////
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Proxies.Event;
using System.ServiceModel;

namespace TestTool.Tests.TestCases.TestSuites
{
    //[TestClass]
    public class NamespacesEventTestSuite : Base.EventTest
    {
        public NamespacesEventTestSuite(TestLaunchParam param)
            : base(param)
        {
            _subscriberAddress = "http://" + _nic.IP + "/onvif_notify_server";
        }

        private readonly string _subscriberAddress;

        private const string PATH = "Event Handling\\Namespaces";

        [ Test( Name = "EVENT - NAMESPACES (DEFAULT NAMESPASES FOR EACH TAG)",
                Path = PATH,
                Order = "04.01.01",
                Id = "4-1-1",
                Category = Category.GENERAL,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void EventNamespacesDefaultNamespacesForEachTagTest()
        {
            SubscribeTestWithTransofrmation(XmlTransformation.EachTag);
        }

        [ Test( Name = "EVENT - NAMESPACES (DEFAULT NAMESPASES FOR PARENT TAG)",
                Path = PATH,
                Order = "04.01.02",
                Id = "4-1-2",
                Category = Category.GENERAL,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void EventNamespacesDefaultNamespacesForParentTagTest()
        {
            SubscribeTestWithTransofrmation(XmlTransformation.ParentTag);
        }

        [ Test( Name = "EVENT - NAMESPACES (NOT STANDARD PREFIXES)",
                Path = PATH,
                Order = "04.01.03",
                Id = "4-1-3",
                Category = Category.GENERAL,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void EventNamespacesNotStandardPrefixesTagTest()
        {
            SubscribeTestWithTransofrmation(XmlTransformation.NotStandardPrefixes);
        }

        [ Test( Name = "EVENT - NAMESPACES (DIFFERENT PREFIXES FOR THE SAME NAMESPACE)",
                Path = PATH,
                Order = "04.01.04",
                Id = "4-1-4",
                Category = Category.GENERAL,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void EventNamespacesDifferentPrefixesForTheSameNamespaceTest()
        {
            SubscribeTestWithTransofrmation(XmlTransformation.DifferentPrefixes);
        }

        [ Test( Name = "EVENT - NAMESPACES (THE SAME PREFIX FOR DIFFERENT NAMESPACES)",
                Path = PATH,
                Order = "04.01.05",
                Id = "4-1-5",
                Category = Category.GENERAL,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void EventNamespacesTheSamePrefixesForDifferentNamespaceTest()
        {
            SubscribeTestWithTransofrmation(XmlTransformation.SamePrefixes);
        }

        void SubscribeTestWithTransofrmation(XmlTransformation transformation)
        {
            EndpointReferenceType subscriptionReference = null;
            int terminationTime = 10;
            RunTest<object>(
            () => null,
            () =>
            {
                Proxies.Event.Subscribe request = new Subscribe();
                request.InitialTerminationTime = "PT10S";

                // Some endpoint reference. There is no information if the DUT must 
                // immediately validate it.
                request.ConsumerReference = new EndpointReferenceType();
                request.ConsumerReference.Address = new AttributedURIType();
                request.ConsumerReference.Address.Value = _subscriberAddress;

                // request.Filter is null - CR 63

                SubscribeResponse response = null;

                bool retry = false;

                XmlNamespacesTransformer transformer = new XmlNamespacesTransformer(transformation);
                EnsureNotificationProducerClientCreated();
                SetBreakingBehaviour(_notificationProducerClient.Endpoint, transformer);

                try
                {
                    response = Subscribe(request);
                }
                catch (FaultException exc)
                {
                    FaultException<UnacceptableInitialTerminationTimeFaultType> invalidTerminationTimeFault =
                        exc as FaultException<UnacceptableInitialTerminationTimeFaultType>;

                    if (invalidTerminationTimeFault != null)
                    {
                        LogStepEvent(string.Format("Exception of type FaultException<UnacceptableInitialTerminationTimeFaultType> received. Try to subscribe with new parameters"));
                        StepPassed();

                        string duration = string.Empty;
                        terminationTime = GetRecommendedDuration(invalidTerminationTimeFault, out duration);

                        retry = true;
                        request.InitialTerminationTime = duration;
                    }
                    else
                    {
                        //throw;
                        StepPassed();
                        return;
                    }
                }

                if (retry)
                {
                    response = Subscribe(request, "Retry subscribe");
                }

                subscriptionReference = response.SubscriptionReference;

                //
                // validate EndPointReference
                //
                Assert(subscriptionReference != null, "The DUT did not return SubscriptionReference",
                       "Check if the DUT returned SubscriptionReference");

                Assert(subscriptionReference.Address != null && subscriptionReference.Address.Value != null,
                "SubscriptionReference does not contain address",
                       "Check if SubscriptionReference contains address");

                Assert(subscriptionReference.Address.Value.IsValidUrl(), "URL passed in SubscriptionReference is not valid",
                       "Check that URL specified is valid");


                Assert(response.CurrentTimeSpecified, "Current time is not specified",
                       "Check that CurrentTime is specified");
                Assert(response.TerminationTimeSpecified, "Termination time is not specified",
                       "Check that TerminationTime is specified");

                Assert(response.CurrentTime.AddSeconds(terminationTime) <= response.TerminationTime,
                    "TerminationTime < CurrentTime + InitialTerminationTime",
                    "Validate times");
            },
            (o) =>
            {
                ResetBreakingBehaviour(_notificationProducerClient.Endpoint);

                if (subscriptionReference != null)
                {
                    if (_subscriptionManagerClient == null)
                    {
                        CreateSubscriptionManagerClient(subscriptionReference);
                    }
                    ReleaseSubscriptionManager(terminationTime * 1000);
                }
            });
        }
    }
}