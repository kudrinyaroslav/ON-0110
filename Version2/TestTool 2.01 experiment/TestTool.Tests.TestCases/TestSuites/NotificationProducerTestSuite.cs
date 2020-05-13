///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Enums;
using TestTool.Proxies.Event;
using System.Xml;
using System.ServiceModel;

namespace TestTool.Tests.TestCases.TestSuites
{
//#if FULL
    [TestClass]
//#endif
    class NotificationProducerTestSuite : EventTest
    {
        public NotificationProducerTestSuite(TestLaunchParam param)
            : base(param)
        {
            _subscriberAddress = "http://" + _nic.IP + "/onvif_notify_server";
        }

        private const string PATH = "Event Handling\\Basic Notification";

        private readonly string _subscriberAddress;

        [Test(Name = "BASIC NOTIFICATION INTERFACE - SUBSCRIBE",
            Path = PATH,
            Order = "09.02.01",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Events },
            RequirementLevel = RequirementLevel.Must)]
        public void SubscribeTest()
        {
            EndpointReferenceType subscriptionReference = null;
            int terminationTime = 10;
            RunTest<object>( 
                ()=> null, 
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

        [Test(Name = "BASIC NOTIFICATION INTERFACE - INVALID MESSAGE CONTENT FILTER",
            Path = PATH,
            Order = "09.02.02",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Events },
            RequirementLevel = RequirementLevel.Should)]
        public void InvalidMessageContentFilterTest()
        {
            EndpointReferenceType subscriptionReference = null;

            RunTest<object>(
               new Backup<object>(
                   () => { return null; }),
                   () =>
                   {
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

                       EnsureNotificationProducerClientCreated();

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

        [Test(Name = "BASIC NOTIFICATION INTERFACE - INVALID TOPIC EXPRESSION",
            Path = PATH,
            Order = "09.02.03",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Events },
            RequirementLevel = RequirementLevel.Should)]
        public void InvalidTopicExpressionTest()
        {
            EndpointReferenceType subscriptionReference = null;

            RunTest<object>(
               new Backup<object>(
                   () => { return null; }),
                   () =>
                   {
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

                        EnsureNotificationProducerClientCreated();
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

        [Test(Name = "BASIC NOTIFICATION INTERFACE - RENEW",
            Path = PATH,
            Order = "09.02.04",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Events },
            RequirementLevel = RequirementLevel.Must)]
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
                                      throw;
                                  }
                              }

                              if (retry)
                              {
                                renewResponse = Renew(renew);
                              }

                              ValidateRenewResponse(renewResponse, terminationTime);

                              string xsDateTime = DateTime.UtcNow.AddSeconds(terminationTime).ToString("yyyy-MM-ddTHH:mm:ssZ");

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

                                        xsDateTime = DateTime.UtcNow.AddSeconds(terminationTime).ToString("yyyy-MM-ddTHH:mm:ssZ");
                                        
                                        //LogStepEvent(string.Format("Send TerminationTime '{0}'", xsDateTime));
                                        
                                        retry = true;
                                        renew.TerminationTime = xsDateTime;
                                    }
                                    else
                                    {
                                        throw;
                                    }
                              }
                              if (retry)
                              {
                                  renewResponse = Renew(renew);
                              }

                              ValidateRenewResponse(renewResponse, terminationTime);

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
        
        [Test(Name = "BASIC NOTIFICATION INTERFACE - UNSUBSCRIBE",
            Path = PATH,
            Order = "09.02.05",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Events },
            RequirementLevel = RequirementLevel.Must)]
        public void UnsubscribeTest()
        {
            EndpointReferenceType subscriptionReference = null;
            bool unsubscribed = false;
            
            RunTest<object>(
                    new Backup<object>( 
                        () =>
                            {
                                return null;
                            }),
                        () =>
                            {
                                SubscribeResponse subscribeResponse = CreateStandardSubscription();
                                
                                if (subscribeResponse == null)
                                {
                                    unsubscribed = true;
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

        [Test(Name = "BASIC NOTIFICATION INTERFACE - RESOURCE UNKNOWN",
            Path = PATH,
            Order = "09.02.06",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Events },
            RequirementLevel = RequirementLevel.Should)]
        public void ResourceUnknownTest()
        {
            EndpointReferenceType subscriptionReference = null;
            bool unsubscribed = false;
            int terminationTime = 10;

            RunTest<object>(
                    new Backup<object>(
                        () =>
                        {
                            return null;
                        }),
                        () =>
                        {

                            SubscribeResponse subscribeResponse = CreateStandardSubscription(ref terminationTime);
                            if (subscribeResponse == null)
                            {
                                unsubscribed = true;
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
                    StepPassed();
                    return null;
                    //throw;
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
            Assert(renewResponse != null, "The DUT did not return Renew response",
               "Check that the DUT returned Renew response");

            Assert(renewResponse.CurrentTimeSpecified, "Current time is not specified",
                  "Check that CurrentTime is specified");
            Assert(renewResponse.TerminationTime.HasValue, "Termination time is not specified",
                   "Check that TerminationTime is specified");

            Assert(renewResponse.CurrentTime.AddSeconds(seconds) <= renewResponse.TerminationTime.Value,
                "TerminationTime < CurrentTime + InitialTerminationTime",
                "Validate times");
        }

    }
}
