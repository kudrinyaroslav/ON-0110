///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Engine.Base;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Event;
using System.Xml;
using System.ServiceModel;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.TestSuites.Events;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class NotificationProducerTestSuite : EventTest
    {
        public NotificationProducerTestSuite(TestLaunchParam param)
            : base(param)
        {
            _subscriberAddress = "http://" + _nic.IP + "/onvif_notify_server";
        }

        private const string PATH = "Event Handling\\Basic Notification Interface";

        private readonly string _subscriberAddress;

        [Test(Name = "BASIC NOTIFICATION INTERFACE - SUBSCRIBE",
            Path = PATH,
            Order = "02.01.09",
            Id = "2-1-9",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.WSBasicNotification, },
            FunctionalityUnderTest = new Functionality[] { Functionality.Subscribe })]
#if true
        public void SubscribeTest()
        {
            int actualTerminationTime = 10;
            SubscriptionHandler Handler = new SubscriptionHandler(this, true);
            Handler.SetPolicy(new EventsVerifyPolicy(true));

            RunTest(
            () =>
            {
                Handler.SetAddress(GetEventServiceAddress());
                Handler.Subscribe(null, actualTerminationTime);
            },
            () =>
            {
                SubscriptionHandler.Unsubscribe(Handler);
            });
        }
#else
        public void SubscribeTest()
        {
            EndpointReferenceType subscriptionReference = null;
            int terminationTime = 10;
            RunTest<object>( 
                ()=> null, 
                () =>
                         {
                             EnsureNotificationProducerClientCreated();

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
                                    //StepPassed();
                                     StepFailed(exc);
                                     throw;
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
#endif

        void TestErrorStep(Action Step, ValidateTypeFault validateFunction)
        {
            FaultException excTest = null;
            try
            {
                Step();
            }
            catch (FaultException exc)
            {
                excTest = exc;
            }
            string reason;
            if (validateFunction(excTest, out reason))
            {
                if (excTest != null)
                {
                    SaveStepFault(excTest);
                    StepPassed();
                }
            }
            else
            {
                if (excTest != null)
                {
                    AssertException ex = new AssertException(reason);
                    throw ex;
                }
                else
                {
                    Assert(false,
                        reason,
                        "Checking expected fault");
                }
            }
        }

        [Test(Name = "BASIC NOTIFICATION INTERFACE - INVALID MESSAGE CONTENT FILTER",
            Path = PATH,
            Order = "02.01.19",
            Id = "2-1-19",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.WSBasicNotification, },
            FunctionalityUnderTest = new Functionality[] { Functionality.Subscribe, Functionality.MessageContentFilter })]
#if true
        public void InvalidMessageContentFilterTest()
        {
            int actualTerminationTime = 10;
            SubscriptionHandler Handler = new SubscriptionHandler(this, true);
            Handler.SetPolicy(new EventsVerifyPolicy(true));

            RunTest(
            () =>
            {
                Handler.SetAddress(GetEventServiceAddress());

                bool topicSetEmpty = false;
                FilterInfo filterInfo = GetInvalidMessageContentFilter(out topicSetEmpty);
                if (topicSetEmpty)
                {
                    return;
                }
                Assert(filterInfo != null,
                       "Failed to create filter for test",
                       "Check if a filter has been created");

                TestErrorStep(() =>
                              {
                                Handler.Subscribe(filterInfo.Filter, actualTerminationTime);
                              },
                              ValidateInvalidMessageContentFilterFault);
            },
            () =>
            {
                SubscriptionHandler.Unsubscribe(Handler);
            });
        }
#else
        public void InvalidMessageContentFilterTest()
        {
            EndpointReferenceType subscriptionReference = null;

            RunTest<object>(
               new Backup<object>(
                   () => { return null; }),
                   () =>
                   {
                       EnsureNotificationProducerClientCreated();
                       
                       Proxies.Event.Subscribe request = new Subscribe();
                       bool topicSetEmpty = false;
                       FilterInfo filterInfo = GetInvalidMessageContentFilter(out topicSetEmpty);

                       if (topicSetEmpty)
                       {
                           return;
                       }

                       Assert(filterInfo != null, 
                           "Failed to create filter for test",
                              "Check if a filter has been created");

                       request.Filter = filterInfo.Filter;
                       request.InitialTerminationTime = "PT10S";

                       request.ConsumerReference = new EndpointReferenceType();
                       request.ConsumerReference.Address = new AttributedURIType();
                       request.ConsumerReference.Address.Value = _subscriberAddress;

                       RunStep(
                           () =>
                               {
                                   SubscribeResponse response = _notificationProducerClient.Subscribe(request);
                                   if (response == null)
                                   {
                                       throw new AssertException("The DUT did not return Subscribe response");
                                   }
                                   subscriptionReference = response.SubscriptionReference;
                               },
                           "Subscribe - negative test",
                           new ValidateTypeFault(ValidateInvalidMessageContentFilterFault));

                   },
                  (o) =>
                  {
                      if (subscriptionReference != null)
                      {
                          if (_subscriptionManagerClient == null)
                          {
                              CreateSubscriptionManagerClient(subscriptionReference);
                          }
                          ReleaseSubscriptionManager(10000);
                      }
                  });
        }
#endif
        [Test(Name = "BASIC NOTIFICATION INTERFACE - INVALID TOPIC EXPRESSION",
            Path = PATH,
            Order = "02.01.23",
            Id = "2-1-23",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.WSBasicNotification, },
            FunctionalityUnderTest = new Functionality[] { Functionality.Subscribe, Functionality.TopicFilter })]
#if true
        public void InvalidTopicExpressionTest()
        {
            int actualTerminationTime = 10;
            SubscriptionHandler Handler = new SubscriptionHandler(this, true);
            Handler.SetPolicy(new EventsVerifyPolicy(true));

            RunTest(
            () =>
            {
                Handler.SetAddress(GetEventServiceAddress());

                FilterType Filter = new FilterType();
                XmlDocument doc = new XmlDocument();
                XmlElement topicElement = doc.CreateTopicElement();
                topicElement.InnerText = GetInvalidTopic();
                Filter.Any = new XmlElement[] { topicElement };

                TestErrorStep(() =>
                              {
                                Handler.Subscribe(Filter, actualTerminationTime);
                              },
                              ValidateInvalidTopicFilterFault);
            },
            () =>
            {
                SubscriptionHandler.Unsubscribe(Handler);
            });
        }
#else
        public void InvalidTopicExpressionTest()
        {
            EndpointReferenceType subscriptionReference = null;

            RunTest<object>(
               new Backup<object>(
                   () => { return null; }),
                   () =>
                   {
                        EnsureNotificationProducerClientCreated();

                        string invalidTopic = GetInvalidTopic();
                       
                        Proxies.Event.Subscribe request = new Subscribe();
                        request.Filter = new FilterType();
                        request.InitialTerminationTime = "PT10S";

                        request.ConsumerReference = new EndpointReferenceType();
                        request.ConsumerReference.Address = new AttributedURIType();
                        request.ConsumerReference.Address.Value = _subscriberAddress;

                        XmlDocument doc = new XmlDocument();

                        XmlElement topicElement = doc.CreateTopicElement();

                        topicElement.InnerText = invalidTopic;

                        request.Filter.Any = new XmlElement[] { topicElement };

                        RunStep(
                            () =>
                                {
                                    SubscribeResponse response=  _notificationProducerClient.Subscribe(request);
                                    subscriptionReference = response.SubscriptionReference;
                                },
                            "Subscribe - negative test",
                            new ValidateTypeFault(ValidateInvalidTopicFilterFault));

                       },
                      (o) =>
                      {
                          if (subscriptionReference != null)
                          {
                              if (_subscriptionManagerClient == null)
                              {
                                  CreateSubscriptionManagerClient(subscriptionReference);
                              }
                              ReleaseSubscriptionManager(10000);
                          }
                      });
        }
#endif
        [Test(Name = "BASIC NOTIFICATION INTERFACE - RENEW",
            Path = PATH,
            Order = "02.01.12",
            Id = "2-1-12",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.WSBasicNotification, },
            FunctionalityUnderTest = new Functionality[] { Functionality.Renew })]
#if true
        public void RenewTest()
        {
            int actualTerminationTime = 10;
            SubscriptionHandler Handler = new SubscriptionHandler(this, true);
            Handler.SetPolicy(new EventsVerifyPolicy(true));
            RunTest(
            () =>
            {
                Handler.SetAddress(GetEventServiceAddress());
                Handler.Subscribe(null, actualTerminationTime);
                Handler.Renew(10);
                DateTime TerminationTime = Handler.GetDeadline();
                Handler.Renew(TerminationTime);
            },
            () =>
            {
                SubscriptionHandler.Unsubscribe(Handler);
            });
        }
#else
        public void RenewTest()
        {
            EndpointReferenceType subscriptionReference = null;
            bool subscribed = false;
            int terminationTime = 10;
            
            RunTest<object>(
                new Backup<object>( 
                    () => { return null; }), 
                    () =>
                        {
                            EnsureNotificationProducerClientCreated();

                            SubscribeResponse subscribeResponse = CreateStandardSubscription();

                            if (subscribeResponse == null)
                            {
                                return;
                            }

                            subscribed = true;
                            subscriptionReference = subscribeResponse.SubscriptionReference;

                              Renew renew = new Renew();
                              renew.TerminationTime = "PT10S";

                              RenewResponse renewResponse = null;
                              bool retry = false;

                              try
                              {
                                renewResponse = Renew(renew);
                              }
                              catch (FaultException exc)
                              {
                                  FaultException<UnacceptableTerminationTimeFaultType> invalidTerminationTimeFault =
                                    exc as FaultException<UnacceptableTerminationTimeFaultType>;

                                  if (invalidTerminationTimeFault != null)
                                  {
                                      LogStepEvent(string.Format("Exception of type FaultException<UnacceptableTerminationTimeFaultType> received. Try to renew subscription with new parameters"));
                                      StepPassed();

                                      string duration = string.Empty;

                                      terminationTime = GetRecommendedDuration<UnacceptableTerminationTimeFaultType>(invalidTerminationTimeFault, out duration);

                                      retry = true;
                                      renew.TerminationTime = duration;
                                  }
                                  else
                                  {
                                      StepFailed(exc);
                                      throw;
                                  }
                              }

                              if (retry)
                              {
                                renewResponse = Renew(renew);
                              }

                              ValidateRenewResponse(renewResponse, terminationTime);

                              DateTime requestTerminationTime = DateTime.UtcNow.AddSeconds(terminationTime);
                            
                             requestTerminationTime = new DateTime(requestTerminationTime.Year,
                                                                   requestTerminationTime.Month, 
                                                                   requestTerminationTime.Day,
                                                                   requestTerminationTime.Hour, 
                                                                   requestTerminationTime.Minute, 
                                                                   requestTerminationTime.Second, 
                                                                   requestTerminationTime.Kind);

                            

                            // TEST -->
                            // requestTerminationTime = DateTime.Parse("2011-06-09T13:27:46Z").ToUniversalTime();
                            // TEST <--

                            string xsDateTime = requestTerminationTime.ToString("yyyy-MM-ddTHH:mm:ssZ");

                              // TEST -->
                              //xsDateTime = "2011-06-09T13:27:46Z";
                              //requestTerminationTime = DateTime.Parse(xsDateTime);
                              // <-- 

                              renew.TerminationTime = xsDateTime;
                              
                              try
                              {
                                  renewResponse = Renew(renew, "Renew subscription - use xs:DateTime format for TerminationTime");
                              }
                              catch (FaultException exc)
                              {
                                    FaultException<UnacceptableTerminationTimeFaultType> invalidTerminationTimeFault =
                                      exc as FaultException<UnacceptableTerminationTimeFaultType>;

                                    if (invalidTerminationTimeFault != null)
                                    {
                                        LogStepEvent(string.Format("Exception of type FaultException<UnacceptableTerminationTimeFaultType> received. Try to renew subscription with new parameters"));
                                        StepPassed();

                                        string duration = string.Empty;

                                        terminationTime = GetRecommendedDuration<UnacceptableTerminationTimeFaultType>(invalidTerminationTimeFault, out duration);

                                        requestTerminationTime = DateTime.UtcNow.AddSeconds(terminationTime);
                                        requestTerminationTime = new DateTime(requestTerminationTime.Year,
                                                                              requestTerminationTime.Month,
                                                                              requestTerminationTime.Day,
                                                                              requestTerminationTime.Hour,
                                                                              requestTerminationTime.Minute,
                                                                              requestTerminationTime.Second,
                                                                              requestTerminationTime.Kind);

                                        xsDateTime = requestTerminationTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
                                        
                                        //LogStepEvent(string.Format("Send TerminationTime '{0}'", xsDateTime));
                                        
                                        retry = true;
                                        renew.TerminationTime = xsDateTime;
                                    }
                                    else
                                    {
                                        StepFailed(exc);
                                    }
                              }
                              if (retry)
                              {
                                  renewResponse = Renew(renew);
                              }

                              ValidateRenewResponse(renewResponse, requestTerminationTime);

                          }, 
                          (o) =>
                              {
                                  if (subscribed)
                                  {
                                      if (subscriptionReference != null)
                                      {
                                          CreateSubscriptionManagerClient(subscriptionReference);
                                      }
                                      ReleaseSubscriptionManager(terminationTime * 1000);
                                  }
                              });

        }
#endif
        [Test(Name = "BASIC NOTIFICATION INTERFACE - UNSUBSCRIBE",
            Path = PATH,
            Order = "02.01.21",
            Id = "2-1-21",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.WSBasicNotification, },
            FunctionalityUnderTest = new Functionality[] { Functionality.Unsubscribe })]
#if true
        public void UnsubscribeTest()
        {
            int actualTerminationTime = 10;
            SubscriptionHandler Handler = new SubscriptionHandler(this, true);
            Handler.SetPolicy(new EventsVerifyPolicy(true));
            bool Unsubscribed = false;
            RunTest(
            () =>
            {
                Handler.SetAddress(GetEventServiceAddress());
                Handler.Subscribe(null, actualTerminationTime);
                SubscriptionHandler.Unsubscribe(Handler);
                Unsubscribed = true;

                TestErrorStep(
                    () => {
                        Renew renew = new Renew();
                        renew.TerminationTime = "PT10S";
                        Handler.GetProxy().Renew(renew);
                    },
                    ValidateResourseUnknownFault
                    );
            },
            () =>
            {
                if (!Unsubscribed)
                {
                    SubscriptionHandler.Unsubscribe(Handler);
                }
            });
        }
#else
        public void UnsubscribeTest()
        {
            EndpointReferenceType subscriptionReference = null;
            bool unsubscribed = true;
            
            RunTest<object>(
                    new Backup<object>( 
                        () =>
                            {
                                return null;
                            }),
                        () =>
                            {
                                EnsureNotificationProducerClientCreated();

                                SubscribeResponse subscribeResponse = CreateStandardSubscription();
                                unsubscribed = false;

                                if (subscribeResponse == null)
                                {
                                    return;
                                }

                                subscriptionReference = subscribeResponse.SubscriptionReference;
                                
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
                                      new ValidateTypeFault(ValidateResourseUnknownFault) );

                            },
                              (o) =>
                              {
                                  if (!unsubscribed)
                                  {
                                      if (subscriptionReference != null)
                                      {
                                          CreateSubscriptionManagerClient(subscriptionReference);
                                      }
                                      //
                                      // Use default timeout of 10 seconds.
                                      // Really timeout passed in Subscribe request may be different.
                                      //
                                      ReleaseSubscriptionManager();
                                  }
                              });

        }
#endif
        [Test(Name = "BASIC NOTIFICATION INTERFACE - RESOURCE UNKNOWN",
            Path = PATH,
            Order = "02.01.22",
            Id = "2-1-22",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.WSBasicNotification, },
            FunctionalityUnderTest = new Functionality[] { Functionality.Unsubscribe })]
#if true
        public void ResourceUnknownTest()
        {
            int actualTerminationTime = 10;
            SubscriptionHandler Handler = new SubscriptionHandler(this, true);
            Handler.SetPolicy(new EventsVerifyPolicy(true));
            bool Unsubscribed = false;

            RunTest(
            () =>
            {
                Handler.SetAddress(GetEventServiceAddress());
                Handler.Subscribe(null, actualTerminationTime);
                SubscriptionHandler.Unsubscribe(Handler);
                Unsubscribed = true;

                TestErrorStep(() => 
                              {
                                  Unsubscribe request = new Unsubscribe();
                                  Handler.GetProxy().Unsubscribe(request);
                              },
                              ValidateUnsubscribeFault);
            },
            () =>
            {
                if (!Unsubscribed)
                {
                    SubscriptionHandler.Unsubscribe(Handler);
                }
            });
        }
#else
        public void ResourceUnknownTest()
        {
            EndpointReferenceType subscriptionReference = null;
            bool unsubscribed = true;
            int terminationTime = 10;

            RunTest<object>(
                    new Backup<object>(
                        () =>
                        {
                            return null;
                        }),
                        () =>
                        {
                            EnsureNotificationProducerClientCreated();

                            SubscribeResponse subscribeResponse = CreateStandardSubscription(ref terminationTime);
                            unsubscribed = false;

                            if (subscribeResponse == null)
                            {
                                return;
                            }

                            subscriptionReference = subscribeResponse.SubscriptionReference;

                            Unsubscribe request = new Unsubscribe();

                            UnsubscribeResponse unsubscribeResponse = Unsubscribe(request);
                            unsubscribed = true;

                            RunStep(
                                () => { _subscriptionManagerClient.Unsubscribe(request); },
                                "Unsubscribe - negative test",
                                new ValidateTypeFault(ValidateUnsubscribeFault));
                                
                        },
                        (o) =>
                        {
                            if (!unsubscribed)
                              {
                                  if (subscriptionReference != null &&  _subscriptionManagerClient == null)
                                  {
                                      CreateSubscriptionManagerClient(subscriptionReference);
                                  }
                                  ReleaseSubscriptionManager(terminationTime*1000);
                              }
                        });

        }
#endif
        
        /// <summary>
        /// Creates subscription with "dummy" filter and InitialTerminationTime of 10 aseconds
        /// </summary>
        /// <returns>SusbcribeResponse from the DUT</returns>
        SubscribeResponse CreateStandardSubscription()
        {
            int terminationTime = 10;
            return CreateStandardSubscription(ref terminationTime);
        }

        /// <summary>
        /// Creates subscription with "dummy" filter and InitialTerminationTime specified
        /// </summary>
        /// <param name="terminationTime">TerminationTime for subscription.</param>
        /// <returns>SusbcribeResponse from the DUT</returns>
        SubscribeResponse CreateStandardSubscription(ref int terminationTime)
        {
            FilterType filter = null;
            return Subscribe(_subscriberAddress, ref terminationTime, filter);
        }

        /// <summary>
        /// Creates subscription providing listening address, termintion time and filter. Validates DUT
        /// response.
        /// </summary>
        /// <param name="listeningAddress">Address of NotificationConsumer serbice provided.</param>
        /// <param name="terminationTime">TerminationTime for subscription.</param>
        /// <param name="filter">Filter information.</param>
        /// <returns>SusbcribeResponse from the DUT.
        /// NULL means that the DUT returned fault (which is acceptable in most test cases).
        /// </returns>
        SubscribeResponse Subscribe(string listeningAddress, ref int terminationTime, FilterType filter)
        {
            Proxies.Event.Subscribe subscribeRequest = new Subscribe();
            
            subscribeRequest.InitialTerminationTime = string.Format("PT{0}S", terminationTime);

            subscribeRequest.Filter = filter;

            subscribeRequest.ConsumerReference = new EndpointReferenceType();
            subscribeRequest.ConsumerReference.Address = new AttributedURIType();
            subscribeRequest.ConsumerReference.Address.Value = listeningAddress;

            bool retry = false;

            SubscribeResponse subscribeResponse = null;
            try
            {
                subscribeResponse = Subscribe(subscribeRequest);
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
                    
                    retry = true;
                    subscribeRequest.InitialTerminationTime = duration;
                }
                else
                {
                    //StepPassed();
                    StepFailed(exc);
                    throw;
                }
            }

            if (retry)
            {
                subscribeResponse = Subscribe(subscribeRequest, "Retry subscribe");
            }

            ValidateSubscribeResponse(subscribeResponse, terminationTime);
            
            CreateSubscriptionManagerClient(subscribeResponse.SubscriptionReference);
            
            return subscribeResponse;
        }


        /// <summary>
        /// Validates Renew response.
        /// </summary>
        /// <param name="renewResponse">Response to the Renew command.</param>
        /// <param name="seconds">Termination time (in seconds) passed to the Renew command</param>
        void ValidateRenewResponse(RenewResponse renewResponse, int seconds)
        {
            ValidateRenewResponse(renewResponse);

            Assert(renewResponse.CurrentTime.AddSeconds(seconds) <= renewResponse.TerminationTime.Value,
                "TerminationTime < CurrentTime + InitialTerminationTime",
                "Validate times");
        }
        
        /// <summary>
        /// Validates Renew response.
        /// </summary>
        /// <param name="renewResponse">Response to the Renew command.</param>
        /// <param name="terminationTime">Termination Time.</param>
        void ValidateRenewResponse(RenewResponse renewResponse, DateTime terminationTime)
        {
            ValidateRenewResponse(renewResponse);

            DateTime responseTerminationTimeUtc = renewResponse.TerminationTime.Value;
            if (renewResponse.TerminationTime.Value.Kind != DateTimeKind.Utc)
            {
                responseTerminationTimeUtc = renewResponse.TerminationTime.Value.ToUniversalTime();
            }
            DateTime terminationTimeUtc = terminationTime.ToUniversalTime();

            Assert(terminationTimeUtc <= responseTerminationTimeUtc,
                "TerminationTime (in Response) < InitialTerminationTime",
                "Check termination time in request and response");

            Assert(renewResponse.CurrentTime <= renewResponse.TerminationTime.Value,
                "TerminationTime (in Response) < CurrentTime (in Response)",
                "Check TerminationTime and CurrentTime in response");
        }

        void ValidateRenewResponse(RenewResponse renewResponse)
        {
            Assert(renewResponse != null, "The DUT did not return Renew response",
               "Check that the DUT returned Renew response");

            Assert(renewResponse.CurrentTimeSpecified, "Current time is not specified",
                  "Check that CurrentTime is specified");
            Assert(renewResponse.TerminationTime.HasValue, "Termination time is not specified",
                   "Check that TerminationTime is specified");

        }
        
    }
}
