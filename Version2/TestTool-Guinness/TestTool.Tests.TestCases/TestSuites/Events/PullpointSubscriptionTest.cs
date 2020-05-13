///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using System.Xml;
using TestTool.Proxies.Event;
using System.ServiceModel;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Utils.Events;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class PullpointSubscriptionTest : EventTest
    {
        public PullpointSubscriptionTest(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Event Handling\\Real-Time Pull-Point Notification Interface";

        [Test(Name = "REALTIME PULLPOINT SUBSCRIPTION - CREATE PULL POINT SUBSCRIPTION",
            Path = PATH,
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.CreatePullPointSubscription })]
        public void CreatePullPointSubscriptionTest()
        {
            bool subscribed = false;
            EndpointReferenceType eventAddress = null;
            int actualTerminationTime = 10;
            
            RunTest<object>(
                () =>
                    {
                        return null;
                    }, 
                () =>
                    {

                        EnsureEventPortTypeClientCreated();

                        XmlElement[] any = null;
                        DateTime currentTime;
                        DateTime? terminationTime;
                        //
                        // For this test it's stated explicitly that neither Topic Expression nor Message Content 
                        // filter is specified.
                        //
                        Proxies.Event.EndpointReferenceType endpointReference =
                            CreatePullPointSubscriptionSafe(null, 
                            "PT10S", 
                            null, 
                            ref any, 
                            out currentTime, 
                            out terminationTime, 
                            ref actualTerminationTime);

                        if (endpointReference == null)
                        {
                            return;
                        }

                        subscribed = true;

                        Assert(endpointReference != null, "The DUT did not return SubscriptionReference",
                               "Check if the DUT returned SubscriptionReference");

                        Assert(endpointReference.Address != null && endpointReference.Address.Value != null, "SubscriptionReference does not contain address",
                               "Check if SubscriptionReference contains address");

                        Assert(endpointReference.Address.Value.IsValidUrl(), "URL passed in SubscriptionReference is not valid",
                               "Check that URL specified is valid");

                        eventAddress = endpointReference;

                        Assert(terminationTime.HasValue, "TerminationTime is not specified",
                               "Check that TerminationTime is specified");

                        Assert(currentTime.AddSeconds(actualTerminationTime) <= terminationTime.Value,
                            "TerminationTime < CurrentTime + InitialTerminationTime",
                            "Validate times");

                    }, 
                (o) =>
                    {
                        if (subscribed)
                        {
                            if (eventAddress != null)
                            {
                                CreateSubscriptionManagerClient(eventAddress);
                            }
                            ReleaseSubscriptionManager(actualTerminationTime*1000);
                        }
                    });
        }

        [Test(Name = "REALTIME PULLPOINT SUBSCRIPTION - INVALID MESSAGE CONTENT FILTER",
            Path = PATH,
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.CreatePullPointSubscription, Functionality.MessageContentFilter })]
        public void CreateSubscriptionInvalidMessageContentFilterTest()
        {
            EndpointReferenceType endpointReference = null;
            RunTest<object>(
                () =>
                    {
                        return null;
                    },
                () =>
                {

                    XmlElement[] any = null;
                    DateTime currentTime;
                    DateTime? terminationTime;

                    FilterInfo filterInfo = GetInvalidMessageContentFilter();
                    Assert(filterInfo != null,
                        "Failed to create filter for test",
                           "Check if a filter has been created");

                    EnsureEventPortTypeClientCreated();

                    RunStep( 
                        () =>
                            {
                                endpointReference =
                                    _eventPortTypeClient.CreatePullPointSubscription(filterInfo.Filter, "PT10S", null, ref any, out currentTime, out terminationTime);
                                
                            }, 
                        "Create Pull Point Subscription - negative test",
                        new ValidateTypeFault(ValidateInvalidMessageContentFilterFault));
                }, 
                (o) =>
                {
                    if (endpointReference != null)
                    {
                        if (endpointReference.Address != null)
                        {
                            CreateSubscriptionManagerClient(endpointReference);
                        }
                        ReleaseSubscriptionManager(10000);
                    }
                });
        }

        [Test(Name = "REALTIME PULLPOINT SUBSCRIPTION - INVALID TOPIC EXPRESSION",
            Path = PATH,
            Order = "03.01.03",
            Id = "3-1-3",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.CreatePullPointSubscription, Functionality.TopicFilter })]
        public void CreateSubscriptionInvalidTopicExpressionTest()
        {
            EndpointReferenceType endpointReference = null;
            RunTest<object>(
                () =>
                {
                    return null;
                },
                () =>
                    {
                        string invalidTopic = GetInvalidTopic();

                        XmlElement[] any = null;
                        DateTime currentTime;
                        DateTime? terminationTime;

                        FilterType filter = new FilterType();
                        XmlDocument doc = new XmlDocument();

                        XmlElement topicElement = doc.CreateTopicElement();
                        topicElement.InnerText = invalidTopic;

                        filter.Any = new XmlElement[] {topicElement};

                        EnsureEventPortTypeClientCreated();

                        RunStep(
                            () =>
                            {
                                endpointReference =
                                    _eventPortTypeClient.CreatePullPointSubscription(filter, "PT10S", null, ref any, out currentTime, out terminationTime);

                            },
                            "Create Pull Point Subscription - negative test",
                            new ValidateTypeFault(ValidateInvalidTopicFilterFault));
                    },
                (o) =>
                {
                    if (endpointReference != null)
                    {
                        if (endpointReference.Address != null)
                        {
                            CreateSubscriptionManagerClient(endpointReference);
                        }
                        ReleaseSubscriptionManager(10000);
                    }
                });
        }
        
        [Test(Name = "REALTIME PULLPOINT SUBSCRIPTION - RENEW",
            Path = PATH,
            Order = "03.01.04",
            Id = "3-1-4",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Renew })]
        public void RenewTest()
       {
           EndpointReferenceType endpointReference = null;
           int actualTerminationTime = 10;
           RunTest<object>(
               () =>
               {
                   return null;
               },
               () =>
               {
                   EnsureEventPortTypeClientCreated();

                   endpointReference = CreateStandardSubscription(ref actualTerminationTime);

                   if (endpointReference == null)
                   {
                       return;
                   }

                   Renew renew = new Renew();
                    renew.TerminationTime = "PT10S";
                    // there was no requirement to retry Renew for this test

                    RenewResponse renewResponse = Renew(renew);

                    Assert(renewResponse != null, "The DUT did not return Renew response",
                       "Check that the DUT returned Renew response");

                    Assert(renewResponse.CurrentTimeSpecified, "Current time is not specified",
                          "Check that CurrentTime is specified");

                    Assert(renewResponse.TerminationTime.HasValue, "Termination time is not specified",
                           "Check that TerminationTime is specified");

                    Assert(renewResponse.CurrentTime.AddSeconds(10) <= renewResponse.TerminationTime.Value,
                        "TerminationTime < CurrentTime + InitialTerminationTime",
                        "Validate times");
               },
                (o) =>
                {
                    if (endpointReference != null)
                    {
                        if (endpointReference.Address != null && _subscriptionManagerClient == null)
                        {
                            CreateSubscriptionManagerClient(endpointReference);
                        }
                        ReleaseSubscriptionManager(actualTerminationTime * 1000);
                    }
                });
       }

        [Test(Name = "REALTIME PULLPOINT SUBSCRIPTION - UNSUBSCRIBE",
            Path = PATH,
            Order = "03.01.05",
            Id = "3-1-5",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.Unsubscribe })]
        public void UnsubscribeTest()
        {
            bool unsubscribed = true;
            RunTest<object>(
                () =>
                {
                    return null;
                },
                () =>
                {

                    EnsureEventPortTypeClientCreated();

                    EndpointReferenceType endpoint = CreateStandardSubscription();
                    if (endpoint == null )
                    {
                        return;
                    }

                    Unsubscribe request = new Unsubscribe();

                    UnsubscribeResponse unsubscribeResponse = Unsubscribe(request);
                    unsubscribed = true;
                    Renew renew = new Renew();
                    renew.TerminationTime = "PT10S";

                    RunStep(
                        () =>
                            {
                                _subscriptionManagerClient.Renew(renew);
                                unsubscribed = false;
                            },
                        "Renew - negative test", 
                        new ValidateTypeFault(ValidateResourseUnknownFault));
                
            },
            (o) =>
                {
                    if (!unsubscribed)
                    {
                        BeginStep("Wait until subscription expires");
                        Sleep(10000);
                        StepPassed();
                    }
                });
        }
        
        [Test(Name = "REALTIME PULLPOINT SUBSCRIPTION - TIMEOUT",
            Path = PATH,
            Order = "03.01.06",
            Id = "3-1-6",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Renew })]
        public void TimeoutTest()
        {
            EndpointReferenceType endpointReference = null;

            RunTest<object>(
                () =>
                {
                    return null;
                },
                () =>
                {

                    EnsureEventPortTypeClientCreated();

                    int actualTerminationTime = 20;
                    endpointReference = CreateStandardSubscription(ref actualTerminationTime);

                    if (endpointReference == null)
                    {
                        return;
                    }

                    BeginStep("Wait until subscription expires");
                    Sleep(actualTerminationTime * 1000 + _operationDelay);
                    StepPassed();

                    Renew renew = new Renew();
                    renew.TerminationTime = "PT10S";

                    RunStep(
                        () =>
                            {
                                _subscriptionManagerClient.Renew(renew);
                            },
                        "Renew - negative test",
                        new ValidateTypeFault(ValidateUnsubscribeFault));

                },
                (o) =>
                {
                    if (endpointReference != null)
                    {
                        if (endpointReference.Address != null && _subscriptionManagerClient == null)
                        {
                            CreateSubscriptionManagerClient(endpointReference);
                        }

                        //
                        // Use timeout of 10 seconds.
                        // TT has already spent time waiting for timeout passed in Subscribe.
                        // 10 seconds is the same as value passed in Renew.
                        //
                        ReleaseSubscriptionManager(10000);
                    }
                });
        }


        /// <summary>
        /// Creates subscription with dummy filter and initial termination time of 10 seconds.
        /// </summary>
        /// <returns>Endpoint returned from the DUT.</returns>
        EndpointReferenceType CreateStandardSubscription()
        {
            int actualTerminationTime = 10;
            return CreateStandardSubscription(ref actualTerminationTime);
        }

        /// <summary>
        /// Creates subscription with dummy filter and initial termination time specified.
        /// </summary>
        /// <param name="terminationTime">Termination time.</param>
        /// <returns>Endpoint returned from the DUT.</returns>
        EndpointReferenceType CreateStandardSubscription(ref int terminationTime)
        {
            FilterType f = null;
            return CreateStandardSubscription(f, ref terminationTime);
        }

        /// <summary>
        /// "Standard" subscription for 9.3.4 - 9.3.7 tests.
        /// </summary>
        /// <returns></returns>
        EndpointReferenceType CreateStandardSubscription(FilterType filter, 
            ref int terminationTimeSeconds)
        {
            string terminationTimeString = string.Format("PT{0}S", terminationTimeSeconds);

            XmlElement[] any = null;
            DateTime currentTime;
            DateTime? terminationTime;

            Proxies.Event.EndpointReferenceType endpointReference =
                CreatePullPointSubscriptionSafe(filter, terminationTimeString, null, ref any, out currentTime, out terminationTime, ref terminationTimeSeconds);

            if (endpointReference == null)
            {
                return null;
            }

            Assert(terminationTime.HasValue, "TerminationTime is not specified",
                   "Check that TerminationTime is specified");

            Assert(currentTime.AddSeconds(terminationTimeSeconds) <= terminationTime.Value,
                "TerminationTime < CurrentTime + InitialTerminationTime",
                "Validate times");

             Assert(endpointReference != null, "The DUT did not return SubscriptionReference",
                    "Check if the DUT returned SubscriptionReference");

             Assert(endpointReference.Address != null && endpointReference.Address.Value != null,
             "SubscriptionReference does not contain address",
                    "Check if SubscriptionReference contains address");

             Assert(endpointReference.Address.Value.IsValidUrl(), "URL passed in SubscriptionReference is not valid",
                    "Check that URL specified is valid");


            CreateSubscriptionManagerClient(endpointReference);

            return endpointReference;
        }

        protected EndpointReferenceType CreatePullPointSubscriptionSafe(
            FilterType Filter,
            string InitialTerminationTime,
            CreatePullPointSubscriptionSubscriptionPolicy SubscriptionPolicy,
            ref XmlElement[] Any,
            out DateTime CurrentTime,
            out System.Nullable<System.DateTime> TerminationTime,
            ref int terminationTime)
        {
            try
            {
                return CreatePullPointSubscription(Filter,
                                                   InitialTerminationTime,
                                                   SubscriptionPolicy,
                                                   ref Any,
                                                   out CurrentTime,
                                                   out TerminationTime);
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
                    terminationTime = GetRecommendedDuration<UnacceptableInitialTerminationTimeFaultType>(invalidTerminationTimeFault, out duration);

                    return CreatePullPointSubscription(Filter,
                                   duration,
                                   SubscriptionPolicy,
                                   ref Any,
                                   out CurrentTime,
                                   out TerminationTime);
                }
                else
                {
                    StepPassed();
                    TerminationTime = null;
                    CurrentTime = DateTime.MinValue;
                    return null;
                    //throw;
                }
            }
        }

   
    }
}
