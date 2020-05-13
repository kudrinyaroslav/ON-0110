///////////////////////////////////////////////////////////////////////////
//!  @author        Ilya Gozman
////
using System;
using TestTool.Tests.CommonUtils.XmlTransformation;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Event;
using System.ServiceModel;
using System.Text;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class NamespacesEventTestSuite : Base.EventTest
    {
        public NamespacesEventTestSuite(TestLaunchParam param)
            : base(param)
        {
            _subscriberAddress = "http://" + _nic.IP + "/onvif_notify_server";
        }

        private readonly string _subscriberAddress;

        private const string PATH = "Event Handling\\Namespaces";

        [ Test( Name = "EVENT - NAMESPACES (DEFAULT NAMESPACES FOR EACH TAG)",
                Path = PATH,
                Order = "04.01.01",
                Id = "4-1-1",
                Category = Category.EVENT,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void EventNamespacesDefaultNamespacesForEachTagTest()
        {
            SubscribeTestWithTransformation(XmlTransformation.EachTag);
        }

        [ Test( Name = "EVENT - NAMESPACES (DEFAULT NAMESPACES FOR PARENT TAG)",
                Path = PATH,
                Order = "04.01.02",
                Id = "4-1-2",
                Category = Category.EVENT,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void EventNamespacesDefaultNamespacesForParentTagTest()
        {
            SubscribeTestWithTransformation(XmlTransformation.ParentTag);
        }

        [ Test( Name = "EVENT - NAMESPACES (NOT STANDARD PREFIXES)",
                Path = PATH,
                Order = "04.01.03",
                Id = "4-1-3",
                Category = Category.EVENT,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void EventNamespacesNotStandardPrefixesTagTest()
        {
            SubscribeTestWithTransformation(XmlTransformation.NotStandardPrefixes);
        }

        [ Test( Name = "EVENT - NAMESPACES (DIFFERENT PREFIXES FOR THE SAME NAMESPACE)",
                Path = PATH,
                Order = "04.01.04",
                Id = "4-1-4",
                Category = Category.EVENT,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void EventNamespacesDifferentPrefixesForTheSameNamespaceTest()
        {
            SubscribeTestWithTransformation(XmlTransformation.DifferentPrefixes);
        }

        [ Test( Name = "EVENT - NAMESPACES (THE SAME PREFIX FOR DIFFERENT NAMESPACES)",
                Path = PATH,
                Order = "04.01.05",
                Id = "4-1-5",
                Category = Category.EVENT,
                Version = 2.0,
                RequirementLevel = RequirementLevel.Must ) ]
        public void EventNamespacesTheSamePrefixesForDifferentNamespaceTest()
        {
            SubscribeTestWithTransformation(XmlTransformation.SamePrefixes);
        }

        void SubscribeTestWithTransformation(XmlTransformation transformation)
        {
            EndpointReferenceType subscriptionReference = null;
            int terminationTime = 10;
            RunTest<object>(
            () => null,
            () =>
            {
                EnsureNotificationProducerClientCreated();
                FaultException exc11, exc12;
                Subscribe(out exc11, out exc12, ref terminationTime);

                XmlNamespacesTransformer transformer = new XmlNamespacesTransformer(transformation);
                EnsureNotificationProducerClientCreated();
                SetBreakingBehaviour(_notificationProducerClient.Endpoint, transformer);

                FaultException exc21, exc22;
                Subscribe(out exc21, out exc22, ref terminationTime);

                BeginStep("Check if reaction to request was the same");
                bool ok = true;
                StringBuilder dump = new StringBuilder();

                Action<FaultException, FaultException, string> compareFaults = new Action<FaultException, FaultException, string>(
                    (exc1, exc2, descr) =>
                        {
                            if (exc1 == null)
                            {
                                if (exc2 == null)
                                {
                                    // ok
                                }
                                else
                                {
                                    ok = false;
                                    dump.AppendFormat(
                                        "Fault has been received in response to {0} request only when sending without transformation{1}",
                                        descr, Environment.NewLine);
                                }
                            }
                            else
                            {
                                if (exc2 == null)
                                {
                                    ok = false;
                                    dump.AppendFormat(
                                        "Fault has been received in response to {0} request only when sending with transformation{1}",
                                        descr, Environment.NewLine);
                                }
                                else
                                {
                                    // Both not null.
                                    // Compare types
                                    if (exc1.GetType().GUID != exc2.GetType().GUID)
                                    {

                                        ok = false;
                                        dump.AppendFormat(
                                            "Faults received in response to {0} request have different types{1}",
                                            descr, Environment.NewLine);
                                    }

                                }
                            }
                        }
                    );

                compareFaults(exc11, exc21, "first");

                FaultException<UnacceptableInitialTerminationTimeFaultType> invalidTerminationTimeFault1 =
                    exc11 as FaultException<UnacceptableInitialTerminationTimeFaultType>;
                FaultException<UnacceptableInitialTerminationTimeFaultType> invalidTerminationTimeFault2 =
                    exc21 as FaultException<UnacceptableInitialTerminationTimeFaultType>;

                if (invalidTerminationTimeFault1 != null && invalidTerminationTimeFault2 != null)
                {
                    compareFaults(exc12, exc22, "second");
                }

                if (!ok)
                {
                    throw new AssertException(dump.ToStringTrimNewLine());
                }

                StepPassed();

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

        void Subscribe(out FaultException exc1, out FaultException exc2, ref int terminationTime)
        {
            string duration = "PT10S";

            Proxies.Event.Subscribe request1 = new Subscribe();
            request1.InitialTerminationTime = duration;

            // Some endpoint reference. There is no information if the DUT must 
            // immediately validate it.
            request1.ConsumerReference = new EndpointReferenceType();
            request1.ConsumerReference.Address = new AttributedURIType();
            request1.ConsumerReference.Address.Value = _subscriberAddress;

            // request.Filter is null - CR 63

            SubscribeResponse response1 = null;

            bool retry = false;

            try
            {
                response1 = Subscribe(request1);
                exc1 = null;
            }
            catch (FaultException exc)
            {
                exc1 = exc;
                FaultException<UnacceptableInitialTerminationTimeFaultType> invalidTerminationTimeFault =
                    exc as FaultException<UnacceptableInitialTerminationTimeFaultType>;

                if (invalidTerminationTimeFault != null)
                {
                    LogStepEvent(string.Format("Exception of type FaultException<UnacceptableInitialTerminationTimeFaultType> received. Try to subscribe with new parameters"));
                    StepPassed();

                    terminationTime = GetRecommendedDuration(invalidTerminationTimeFault, out duration);

                    retry = true;
                    request1.InitialTerminationTime = duration;
                }
                else
                {
                    //throw;
                    StepPassed();
                    exc2 = null;
                    return;
                }
            }

            if (retry)
            {
                try
                {
                    response1 = Subscribe(request1, "Retry subscribe");
                }
                catch (FaultException exc)
                {
                    exc2 = exc;
                    StepFailed(exc);
                    return;
                }
            }

            exc2 = null;
            //
            // validate EndPointReference
            //
            ValidateSubscribeResponse(response1, terminationTime);

            return;
        }
        

    }
}