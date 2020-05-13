///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////

using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using System.Xml;
using TestTool.Proxies.Event;
using System.ServiceModel;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.TestCases.Utils;
using TestTool.HttpTransport;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.CommonUtils.SoapValidation;
using System.ServiceModel.Channels;
using System.Collections.Generic;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.TestCases.Utils;
using TestTool.Tests.Definitions.Exceptions;
using System.Threading;
using TestTool.Tests.Common.Trace;
using System.ServiceModel.Description;
using TestTool.Tests.Definitions.Data;
using System.IO;
using System.Linq;

namespace TestTool.Tests.TestCases.TestSuites.Events
{
    class EventsMainHelper
    {
        static public XmlNamespaceManager CreateNamespaceManager(XmlDocument soapRawPacket)
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(soapRawPacket.NameTable);
            manager.AddNamespace("s", "http://www.w3.org/2003/05/soap-envelope");
            manager.AddNamespace("events", "http://www.onvif.org/ver10/events/wsdl");
            manager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");
            return manager;
        }
        static public Dictionary<NotificationMessageHolderType, XmlElement> GetRawElements(NotificationMessageHolderType[] notificationMessages,
                                                                                           XmlDocument soapRawPacket,
                                                                                           XmlNamespaceManager manager,
                                                                                           bool Notify,
                                                                                           StringBuilder logger)
        {
            Dictionary<NotificationMessageHolderType, XmlElement> rawElements = new Dictionary<NotificationMessageHolderType, XmlElement>();

            string messagePath;
            if (Notify)
            {
                messagePath = "/s:Envelope/s:Body/b2:Notify/b2:NotificationMessage";
            }
            else
            {
                messagePath = "/s:Envelope/s:Body/events:PullMessagesResponse/b2:NotificationMessage";
            }

            XmlNodeList responseNodeList = soapRawPacket.SelectNodes(messagePath, manager);
            int cnt = 0;

            var onvifEvents = notificationMessages.Where(e => OnvifMessage.IsOnvifMessage(e.Message));

            if (notificationMessages.Count() != onvifEvents.Count())
                logger.AppendLine("WARNING: there is a message from non-ONVIF namespace");

            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            foreach (NotificationMessageHolderType message in onvifEvents)
            {
                var xml = (XmlElement) responseNodeList[cnt];
                rawElements.Add(message, xml);
                cnt++;
            }

            return rawElements;
        }
        static public string SortOutHttpHeader(
            string Dump
            )
        {
            string rawSoapPacket;
            System.IO.StringReader rdr = new System.IO.StringReader(Dump);
            string nextLine;

            do
            {
                nextLine = rdr.ReadLine();
            }
            while (!string.IsNullOrEmpty(nextLine));

            rawSoapPacket = rdr.ReadToEnd();
            // fix for #976
            rawSoapPacket = BaseNotificationXmlUtils.RemoveInvalidXmlChars(rawSoapPacket);

            return rawSoapPacket;
        }

        static public XmlElement FindElement(BaseFaultType detail, string localName, string nameSpace)
        {
            return FindElement(detail.Any, localName, nameSpace);
        }

        static public XmlElement FindElement(XmlElement[] any, string localName, string nameSpace)
        {
            if (any != null)
            {
                foreach (XmlElement element in any)
                {
                    if (StringComparer.InvariantCultureIgnoreCase.Compare(element.LocalName, localName) == 0 &&
                        StringComparer.InvariantCultureIgnoreCase.Compare(element.NamespaceURI, nameSpace) == 0)
                    {
                        return element;
                    }
                }
            }
            return null;
        }
        static public DateTime GetDateTimeFromFault<T>(FaultException<T> fault, string localName, string nameSpace)
            where T : BaseFaultType
        {
            if (fault.Detail == null)
            {
                return DateTime.MinValue;
            };
            XmlElement TimeElement = FindElement(fault.Detail, localName, nameSpace);
            string time = TimeElement.InnerText;
            return DateTime.Parse(time);
        }
        static public int GetRecommendedDuration<T>(FaultException<T> fault, out string duration)
            where T : BaseFaultType
        {
            DateTime minimumTime = GetDateTimeFromFault(fault,
                "MinimumTime",
                "http://docs.oasis-open.org/wsn/b-2");
            DateTime timestamp = GetDateTimeFromFault(fault,
                BaseNotification.TIMESTAMP,
                BaseNotification.WSRFBFNAMESPACE);

            duration = string.Empty;
            int seconds = 0;
            if (minimumTime != DateTime.MinValue && timestamp != DateTime.MinValue)
            {
                TimeSpan diff = minimumTime - timestamp;
                seconds = (int)diff.TotalSeconds;
                duration = string.Format("PT{0}S", seconds);
            }
            return seconds;
        }

    }

    class EventsVerifyPolicy
    {
        public bool VerifyDataPresence;
        public bool VerifyMessagesPresence;
        public bool VerifyMessageLimit;
        public bool VerifyTerminationTime;
        public EventsVerifyPolicy(bool Notify)
        {
            VerifyDataPresence = !Notify;
            VerifyMessagesPresence = true;
            VerifyMessageLimit = !Notify;
            VerifyTerminationTime = true;
        }
    }

    public class TooLargeTimeoutException : Exception
    {
        public TooLargeTimeoutException(Exception innerException): base(innerException.Message, innerException) {}
    }

    class EventsProxy
    {
        BaseOnvifTest baseTest;
        public string eventServiceAddress;
        EndpointReferenceType endpointReference;
        /// <summary>
        /// SubscriptionManager client (providing Renew and Unsubscribe methods).
        /// </summary>
        SubscriptionManagerClient subscriptionManagerClient;

        /// <summary>
        /// EventPortType client (providing GetEventProperties and CreatePullPointSubscription methods)
        /// </summary>
        EventPortTypeClient eventPortTypeClient;

        /// <summary>
        /// NotificationProducer client (providing Subscribe method).
        /// </summary>
        NotificationProducerClient notificationProducerClient;

        /// <summary>
        /// PullPointSubscription client (providing PullMessages and SetSynchronizationPoint methods).
        /// </summary>
        PullPointSubscriptionClient pullPointSubscriptionClient;
        PullPointSubscriptionClient pullPointSubscriptionClientLocal;

        TrafficListener trafficListener;

        public EventsProxy(
            BaseOnvifTest baseTest
            )
        {
            this.baseTest = baseTest;
        }
        public void Close()
        {
            if (subscriptionManagerClient != null)
            {
                subscriptionManagerClient.Close();
                subscriptionManagerClient = null;
            }
            if (eventPortTypeClient != null)
            {
                eventPortTypeClient.Close();
                eventPortTypeClient = null;
            }
            if (notificationProducerClient != null)
            {
                notificationProducerClient.Close();
                notificationProducerClient = null;
            }
            if (pullPointSubscriptionClient != null)
            {
                pullPointSubscriptionClient.Close();
                pullPointSubscriptionClient = null;
            }
            if (pullPointSubscriptionClientLocal != null)
            {
                pullPointSubscriptionClientLocal.Close();
                pullPointSubscriptionClientLocal = null;
            }
            if (trafficListener != null)
            {
                trafficListener = null;
            }
        }

        protected Binding CreateBinding(
            string address, 
            bool LocalListener
            )
        {
            IChannelController[] controllers;

            EndpointController controller = new EndpointController(new EndpointAddress(address));

            WsaController wsaController = new WsaController();

            if (LocalListener && (trafficListener == null))
            {
                trafficListener = new TrafficListener();
                baseTest.AttachListenerTimeFilter(trafficListener);
            }

            controllers = new IChannelController[]
                              {
                                  LocalListener ? trafficListener : baseTest.Traffic, 
                                  baseTest.Semaphore, 
                                  baseTest.Credentials, 
                                  controller, 
                                  wsaController,
                                  new SoapValidator(EventsSchemasSet.GetInstance())
                              };
            if (LocalListener)
            {
                //controllers.I
            }

            Binding binding = new TestTool.HttpTransport.HttpBinding(controllers);

            return binding;
        }

        void Decorate<TChannel>(System.ServiceModel.ClientBase<TChannel> Client, bool Addressing) where TChannel : class
        {
            if (!string.IsNullOrEmpty(baseTest.Credentials.Username) && !string.IsNullOrEmpty(baseTest.Credentials.Password))
            {
                SecurityBehavior securityBehavior = new SecurityBehavior();
                securityBehavior.CredentialsProvider = baseTest.Credentials;
                Client.Endpoint.Behaviors.Add(securityBehavior);
            }
            if (Addressing && endpointReference.ReferenceParameters != null && endpointReference.ReferenceParameters.Any != null)
            {
                EndpointReferenceBehaviour behaviour = new EndpointReferenceBehaviour(endpointReference);
                Client.Endpoint.Behaviors.Add(behaviour);
            }
            Client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, baseTest.MessageTimeout);
        }

        void DecorateTimeout<TChannel>(System.ServiceModel.ClientBase<TChannel> Client, int Timeout) where TChannel : class
        {
            try
            {
                Client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, Timeout + 1000);
            }
            catch (Exception e)
            { throw new TooLargeTimeoutException(e); }
        }

        void DecorateTimeout<TChannel>(System.ServiceModel.ClientBase<TChannel> Client, string Timeout) where TChannel : class
        {
            int LocTimeout = Convert.ToInt32(Timeout.DurationToSeconds());

            try
            {
                if (LocTimeout <= 0)
                    DecorateTimeout(Client, baseTest.MessageTimeout);
                else
                    DecorateTimeout(Client, LocTimeout);
            }
            catch (Exception e)
            { throw new TooLargeTimeoutException(e); }
        }

        void ValidateEventPortType()
        {
            if (eventPortTypeClient != null)
            {
                return;
            };
            Binding binding = CreateBinding(eventServiceAddress, false);
            eventPortTypeClient = new EventPortTypeClient(binding, new EndpointAddress(eventServiceAddress));
            Decorate(eventPortTypeClient, false);
        }
        void ValidateNotificationProducer()
        {
            if (notificationProducerClient != null)
            {
                return;
            };
            Binding binding = CreateBinding(eventServiceAddress, false);
            notificationProducerClient = new NotificationProducerClient(binding, new EndpointAddress(eventServiceAddress));
            Decorate(notificationProducerClient, false);
        }
        void ValidateSubscriptionManager()
        {
            if (subscriptionManagerClient != null)
            {
                return;
            };
            Binding binding = CreateBinding(endpointReference.Address.Value, false);
            subscriptionManagerClient = new SubscriptionManagerClient(binding, new EndpointAddress(endpointReference.Address.Value));
            Decorate(subscriptionManagerClient, true);
        }

        void ValidatePullPointSubscription(int Timeout)
        {
            if (pullPointSubscriptionClient == null)
            {
                Binding binding = CreateBinding(endpointReference.Address.Value, false);
                pullPointSubscriptionClient = new PullPointSubscriptionClient(binding, new EndpointAddress(endpointReference.Address.Value));
                Decorate(pullPointSubscriptionClient, true);
            };
            try
            { DecorateTimeout(pullPointSubscriptionClient, Timeout); }
            catch (TooLargeTimeoutException)
            {
                baseTest.LogStepEvent("Too large PullMessages timeout is specified.");
                baseTest.LogStepEvent("OTT can handle timeout about 24 days.");
                baseTest.LogStepEvent("OTT will send PullMessages request with specified timeout but will wait only for MessageTimeout to complete.");
                baseTest.LogStepEvent("");

                DecorateTimeout(pullPointSubscriptionClient, baseTest.MessageTimeout);
            }
        }

        void ValidatePullPointSubscription()
        {
            ValidatePullPointSubscription(baseTest.MessageTimeout);
        }

        void ValidatePullPointSubscription(string Timeout)
        {
            ValidatePullPointSubscription((int)(Timeout.DurationToSeconds() * 1000));
        }

        void ValidatePullPointSubscriptionLocal(int Timeout)
        {
            if (pullPointSubscriptionClientLocal == null)
            {
                Binding binding = CreateBinding(endpointReference.Address.Value, true);
                pullPointSubscriptionClientLocal = new PullPointSubscriptionClient(binding, new EndpointAddress(endpointReference.Address.Value));
                Decorate(pullPointSubscriptionClientLocal, true);
            };
            DecorateTimeout(pullPointSubscriptionClientLocal, Timeout);
        }

        void ValidatePullPointSubscriptionLocal(string Timeout)
        {
            ValidatePullPointSubscriptionLocal((int)(Timeout.DurationToSeconds() * 1000));
        }

        private void ProceedResubscribe(FaultException<UnacceptableInitialTerminationTimeFaultType> ex,
            out string InitialTerminationTime)
        {
            baseTest.LogStepEvent(string.Format("{0}Exception of type FaultException<UnacceptableInitialTerminationTimeFaultType> received. Will try to Subscribe with new parameters{0}", Environment.NewLine));
            baseTest.StepPassed();

            int time = EventsMainHelper.GetRecommendedDuration(ex, out InitialTerminationTime);
            string entry = string.Format("Use duration {0}", InitialTerminationTime);
            baseTest.Assert(time > 0,
                            "Fault details or MinimumTime or TimeStamp not found",
                            "Check if time specified", entry);
        }

        public EndpointReferenceType Subscribe(
            string ConsumerAddress,
            FilterType Filter,
            string InitialTerminationTime,
            out DateTime CurrentTime,
            out Nullable<DateTime> TerminationTime)
        {
            ValidateNotificationProducer();
            Subscribe Request = new Subscribe();
            Request.InitialTerminationTime = InitialTerminationTime;
            Request.Filter = Filter;
            Request.ConsumerReference = new EndpointReferenceType();
            Request.ConsumerReference.Address = new AttributedURIType();
            Request.ConsumerReference.Address.Value = ConsumerAddress;

            SubscribeResponse response = null;
            try
            {
                BaseTest.RunStep(baseTest,
                                 () => response = notificationProducerClient.Subscribe(Request), 
                                 "Send Subscribe request");
            }
            catch (FaultException<UnacceptableInitialTerminationTimeFaultType> ex)
            {
                ProceedResubscribe(ex, out InitialTerminationTime);
                Request.InitialTerminationTime = InitialTerminationTime;
                BaseTest.RunStep(baseTest,
                () =>
                {
                    response = notificationProducerClient.Subscribe(Request);
                }, "Resend Subscribe request");
            }
            baseTest.Assert(response != null, "The DUT did not return Subscribe response",
              "Check that the DUT returned Subscribe response");

            baseTest.Assert(response.CurrentTimeSpecified, "Current time is not specified",
                  "Check that CurrentTime is specified");

            CurrentTime = response.CurrentTime;
            TerminationTime = response.TerminationTimeSpecified ? response.TerminationTime : null;
            return endpointReference = response.SubscriptionReference;
        }
        public EndpointReferenceType CreatePullPointSubscription(FilterType Filter,
                                                                 string InitialTerminationTime,
                                                                 out DateTime CurrentTime,
                                                                 out Nullable<DateTime> TerminationTime)
        {
            ValidateEventPortType();

            EndpointReferenceType result = null;

            System.Xml.XmlElement[] anyCopy = null;
            DateTime currentTimeCopy = DateTime.MinValue;
            DateTime? terminationTimeCopy = DateTime.MinValue;

            try
            {
                BaseTest.RunStep(baseTest,
                                 () =>
                                 {
                                     result = eventPortTypeClient.CreatePullPointSubscription(Filter,
                                                                                             InitialTerminationTime,
                                                                                             null,
                                                                                             ref anyCopy,
                                                                                             out currentTimeCopy,
                                                                                             out terminationTimeCopy);
                                 },
                                 "Create Pull Point Subscription");
            }
            catch (FaultException<UnacceptableInitialTerminationTimeFaultType> ex)
            {
                ProceedResubscribe(ex, out InitialTerminationTime);
                BaseTest.RunStep(baseTest,
                                 () =>
                                 {
                                     result = eventPortTypeClient.CreatePullPointSubscription(Filter,
                                                                                              InitialTerminationTime,
                                                                                              null,
                                                                                              ref anyCopy,
                                                                                              out currentTimeCopy,
                                                                                              out terminationTimeCopy);
                                 }, 
                                 "Create Pull Point Subscription with another time");
            }

            CurrentTime = currentTimeCopy;
            TerminationTime = terminationTimeCopy;
            return endpointReference = result;
        }
        public TopicSetType GetTopicSet()
        {
            ValidateEventPortType();

            string[] response = null;

            bool fixedTopicSet = false;
            TopicSetType topicSet = null;
            string[] topicExpressionDialect = null;
            string[] messageContentFilterDialect = null;
            string[] producerPropertiesFilterDialect = null;
            string[] messageContentSchemaLocation = null;
            XmlElement[] any = null;

            BaseTest.RunStep(baseTest,
            () =>
            {
                response = eventPortTypeClient.GetEventProperties(out fixedTopicSet,
                               out topicSet,
                               out topicExpressionDialect,
                               out messageContentFilterDialect,
                               out producerPropertiesFilterDialect,
                               out messageContentSchemaLocation,
                               out any);
            }, "Get Event Properties");
            return topicSet;
        }

        public RenewResponse Renew(
            Renew request
            )
        {
            ValidateSubscriptionManager();

            RenewResponse response = null;
            BaseTest.RunStep(baseTest, 
                             () => response = subscriptionManagerClient.Renew(request), 
                             "Renew subscription");
            return response;
        }
        public UnsubscribeResponse Unsubscribe(
            Unsubscribe request
            )
        {
            ValidateSubscriptionManager();

            UnsubscribeResponse response = null;
            BaseTest.RunStep(baseTest, 
                             () => response = subscriptionManagerClient.Unsubscribe(request), 
                             "Send Unsubscribe request");
            Close();
            return response;
        }
        public void SetSynchronizationPoint()
        {
            ValidatePullPointSubscription();

            BaseTest.RunStep(baseTest, 
                             () => pullPointSubscriptionClient.SetSynchronizationPoint(), 
                             "Set Synchronization Point");
        }
        public void Seek(DateTime Position, bool? Reverse)
        {
            ValidatePullPointSubscription();

            BaseTest.RunStep(baseTest,
                             () => pullPointSubscriptionClient.Seek(Position, Reverse, null), 
                             "Seek in persistent notification storage");
        }

        public DateTime PullMessages(string PullMessagesTimeout, int PullMessagesWaitResponseTimeout, int MessageLimit, out DateTime TerminationTime, out NotificationMessageHolderType[] NotificationMessage)
        {
            ValidatePullPointSubscription(PullMessagesWaitResponseTimeout);

            DateTime response = DateTime.MinValue;
            DateTime locTerminationTime = DateTime.MinValue;
            NotificationMessageHolderType[] locNotificationMessage = null;
            BaseTest.RunStep(baseTest,
                             () => response = pullPointSubscriptionClient.PullMessages(PullMessagesTimeout, MessageLimit, null, out locTerminationTime, out locNotificationMessage), 
                             "Send PullMessages request");

            TerminationTime = locTerminationTime;
            NotificationMessage = locNotificationMessage;
            return response;
        }

        public DateTime PullMessages(string PullMessagesTimeout,
                                     int MessageLimit,
                                     out DateTime TerminationTime,
                                     out NotificationMessageHolderType[] NotificationMessage)
        {
            return PullMessages(PullMessagesTimeout, (int)PullMessagesTimeout.DurationToSeconds(), MessageLimit, out TerminationTime, out NotificationMessage);
        }

        public DateTime PullMessages(string PullMessagesTimeout, int PullMessagesWaitResponseTimeout, int MessageLimit, out DateTime TerminationTime, out NotificationMessageHolderType[] NotificationMessage, out string Dump)
        {
            ValidatePullPointSubscription(PullMessagesWaitResponseTimeout);
            
            string localDump = null;
            Action<string> action = (data) => localDump = data;

            baseTest.Traffic.ResponseReceived += action;
            DateTime response = DateTime.MinValue;
            DateTime locTerminationTime = DateTime.MinValue;
            NotificationMessageHolderType[] locNotificationMessage = null;
            try
            {
                BaseTest.RunStep(baseTest,
                                 () => response = pullPointSubscriptionClient.PullMessages(PullMessagesTimeout, MessageLimit, null, out locTerminationTime, out locNotificationMessage), 
                                 "Send PullMessages request");
            }
            finally
            {
                baseTest.Traffic.ResponseReceived -= action;
            }
            TerminationTime = locTerminationTime;
            NotificationMessage = locNotificationMessage;
            Dump = localDump;

            return response;
        }

        public DateTime PullMessages(string PullMessagesTimeout,
                                     int MessageLimit,
                                     out DateTime TerminationTime,
                                     out NotificationMessageHolderType[] NotificationMessage,
                                     out string Dump)
        {
            return PullMessages(PullMessagesTimeout, (int)PullMessagesTimeout.DurationToSeconds(), MessageLimit, out TerminationTime, out NotificationMessage, out Dump);
        }

        public DateTime PullMessages(string PullMessagesTimeout,
                                     int PullMessagesWaitResponseTimeout,
                                     int MessageLimit,
                                     Action SomethingInTheMiddle,
                                     string MessageToLog,
                                     out DateTime TerminationTime,
                                     out NotificationMessageHolderType[] NotificationMessage,
                                     out string Dump)
        {
            DateTime response = DateTime.MinValue;
            DateTime locTerminationTime = DateTime.MinValue;
            NotificationMessageHolderType[] locNotificationMessage = null;
            TerminationTime = locTerminationTime;
            NotificationMessage = locNotificationMessage;
            Dump = null;

            ValidatePullPointSubscriptionLocal(PullMessagesWaitResponseTimeout);
            
            string localDump = null;
            Action<string> OnReceive = (data) => localDump = data;

            ManualResetEvent requestSentOkEvent = new ManualResetEvent(false);
            bool requestSentHandled = false;
            Action<string> OnSend = (data) =>
                {
                    if (!requestSentHandled)
                    {
                        baseTest.LogRequest(data);
                        if (baseTest.InStep)
                        {
                            string logEntry = "Waiting for PullMessagesResponse message";
                            if (MessageToLog != null)
                            {
                                logEntry += MessageToLog;
                            }
                            baseTest.LogStepEvent(logEntry);
                            baseTest.StepPassed();
                        }
                        requestSentOkEvent.Set();
                        requestSentHandled = true;
                    }
                };

            ManualResetEvent requestSentErrorEvent = new ManualResetEvent(false);
            Action PullAction = () =>
                {
                    try
                    {
                        response = pullPointSubscriptionClientLocal.PullMessages(PullMessagesTimeout, MessageLimit, null, out locTerminationTime, out locNotificationMessage);
                    }
                    catch (Exception)
                    {
                        requestSentErrorEvent.Set();
                        throw;
                    }
                };


            trafficListener.RequestSent += OnSend;
            trafficListener.ResponseReceived += OnReceive;

            try
            {
                baseTest.BeginStep("Send PullMessages request");
                IAsyncResult PullActionResult = PullAction.BeginInvoke(null, null);

                int hndl = System.Threading.WaitHandle.WaitAny(
                    new WaitHandle[] {
                    requestSentOkEvent, 
                    requestSentErrorEvent, 
                    baseTest.Semaphore.StopEvent
                });

                if (hndl == 2)
                {
                    System.Diagnostics.Debug.WriteLine("STOP - exit test");
                    baseTest.Semaphore.ReportStop();
                    return response;
                }
                if (hndl == 1)
                {
                    return response;
                }


                IAsyncResult SomethingInTheMiddleResult = null;
                if (SomethingInTheMiddle != null)
                {
                    SomethingInTheMiddleResult = SomethingInTheMiddle.BeginInvoke(null, null);
                }

                WaitHandle[] handles =
                    (SomethingInTheMiddle != null) ?
                    new WaitHandle[] { PullActionResult.AsyncWaitHandle, SomethingInTheMiddleResult.AsyncWaitHandle } :
                    new WaitHandle[] { PullActionResult.AsyncWaitHandle };

                WaitHandle.WaitAll(handles);

                if (SomethingInTheMiddle != null)
                {
                    SomethingInTheMiddle.EndInvoke(SomethingInTheMiddleResult);
                }

                baseTest.BeginStep("Get PullMessages response");
                baseTest.LogResponse(localDump);
                PullAction.EndInvoke(PullActionResult);
                baseTest.StepPassed();
            }
            finally
            {
                trafficListener.ResponseReceived -= OnReceive;
                trafficListener.RequestSent -= OnSend;
            }

            TerminationTime = locTerminationTime;
            NotificationMessage = locNotificationMessage;
            Dump = localDump;
            return response;
        }

        public DateTime PullMessages(string PullMessagesTimeout,
                                     int MessageLimit,
                                     Action SomethingInTheMiddle,
                                     string MessageToLog,
                                     out DateTime TerminationTime,
                                     out NotificationMessageHolderType[] NotificationMessage,
                                     out string Dump)
        {
            return PullMessages(PullMessagesTimeout, (int)PullMessagesTimeout.DurationToSeconds(), MessageLimit, SomethingInTheMiddle, MessageToLog, out TerminationTime, out NotificationMessage, out Dump);
        }
    };

    class NotificationServer : NotifyServer
    {
        BaseOnvifTest baseTest;
        public NotificationServer(NetworkInterfaceDescription Nic, BaseOnvifTest baseTest)
            : base(Nic)
        {
            this.baseTest = baseTest;
        }
        public void Setup()
        {
            WaitStarted += BeginStartListeningStep;
            WaitFinished += baseTest.StepPassed;
            NotificationReceived += LogNotifications;
            Timeout += ThrowTimeoutException;
        }
        public void Clear()
        {
            WaitStarted -= BeginStartListeningStep;
            WaitFinished -= baseTest.StepPassed;
            NotificationReceived -= LogNotifications;
            Timeout -= ThrowTimeoutException;
        }

        void BeginStartListeningStep()
        {
            baseTest.BeginStep("Wait for notification");
        }
        void LogNotifications(byte[] data)
        {
            string content =
                System.Text.Encoding.UTF8.GetString(data);
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                XmlDocument document = new XmlDocument();
                document.LoadXml(content);
                document.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                TextReader rdr = new StreamReader(memoryStream);
                content = rdr.ReadToEnd();
                rdr.Close();
            }
            catch (Exception exc)
            {
                // log responce "as is"
            }
            baseTest.LogResponse(content);
        }
        void ThrowTimeoutException()
        {
            throw new ApplicationException("No notification received!");
        }
    }

    class SubscriptionHandler
    {
        BaseOnvifTest baseTest;
        NotificationServer Server;
        EventsVerifyPolicy eventsVerifyPolicy;
        EventsProxy Proxy;
        EndpointReferenceType Subscription;
        DateTime SubscriptionStarted;
        DateTime SubscriptionDeadline;
        public DateTime GetDeadline() { return SubscriptionDeadline; }
        TimeSpan DUTTimeShift;
        int RenewTimeout;
        bool Notify;
        bool KeepAlive;
        bool Subscribed = false;

        private DateTime? m_TerminationTime;
        public DateTime TerminationTime 
        { 
            get
            { return m_TerminationTime.HasValue ? m_TerminationTime.Value : DateTime.MaxValue; } 
        }

        private DateTime m_CurrentTime;
        public DateTime CurrentTime { get { return m_CurrentTime; } }

        public string PullMessagesRequestTimeout { get; set; }

        private int? m_NotificationResponeWaitTimeout;
        public int NotificationResponeWaitTimeout
        {
            get
            {
                if (m_NotificationResponeWaitTimeout.HasValue)
                    return m_NotificationResponeWaitTimeout.Value;
    
                if (Notify)
                {
                    return 20000;
                }
                else
                {
                    if (!string.IsNullOrEmpty(PullMessagesRequestTimeout))
                        return (int) PullMessagesRequestTimeout.DurationToSeconds()*1000;
                }

                return baseTest.MessageTimeout;
            }
            set { m_NotificationResponeWaitTimeout = value; }
        }

        public SubscriptionHandler(BaseOnvifTest baseTest, bool Notify)
        {
            this.baseTest = baseTest;
            this.Notify = Notify;
            SubscriptionStarted = DateTime.MinValue;
            SubscriptionDeadline = DateTime.MinValue;
            RenewTimeout = 60;
            PullMessagesRequestTimeout = "PT20S";
            Subscription = null;
            Proxy = new EventsProxy(baseTest);
            Server = null;
            KeepAlive = false;
        }

        public SubscriptionHandler(BaseOnvifTest baseTest, bool Notify, string address): this(baseTest, Notify)
        {
            SetAddress(address);
            SetPolicy(new EventsVerifyPolicy(Notify));
        }

        public void SetPolicy(EventsVerifyPolicy eventsVerifyPolicy)
        {
            this.eventsVerifyPolicy = eventsVerifyPolicy;
        }
        public void SetAddress(string Address)
        {
            Proxy.eventServiceAddress = Address;
        }
        public EventsProxy GetProxy()
        {
            return Proxy;
        }
        public TopicSetType GetTopicSet()
        {
            return Proxy.GetTopicSet();
        }
        public void Subscribe(FilterType Filter, int TimeoutSeconds)
        {
            string InitialTerminationTime = string.Format("PT{0}S", TimeoutSeconds);
            if (TimeoutSeconds < 0)
            {
                KeepAlive = true;
                InitialTerminationTime = null;
            }
            if (Notify)
            {
                baseTest.BeginStep("Creating listening server");
                try
                {
                    Server = new NotificationServer(baseTest.Nic, baseTest);
                    Server.Setup();
                    Server.StartNotify();

                }
                catch (HttpListenerException e)
                {
                    string msg = e.Message;
                    if (5 == e.ErrorCode) //Access denied exception
                        msg = string.Format("Unable to create subscription: {0}", e.Message);
                    AssertException ee = new AssertException(msg, e);
                    baseTest.StepFailed(ee);
                    throw ee;
                }
                baseTest.StepPassed();

                Subscription = Proxy.Subscribe(Server.GetNotificationUri(),
                                               Filter,
                                               InitialTerminationTime,
                                               out m_CurrentTime,
                                               out m_TerminationTime);
            }
            else
            {
                Subscription = Proxy.CreatePullPointSubscription(Filter, InitialTerminationTime, out m_CurrentTime, out m_TerminationTime);
            }
            SubscriptionStarted = DateTime.Now;
            DUTTimeShift = SubscriptionStarted - CurrentTime;
            EventServiceUtils.ValidateSubscription(m_TerminationTime,
                                                   CurrentTime,
                                                   TimeoutSeconds,
                                                   Subscription,
                                                   baseTest.Assert);

            SubscriptionDeadline = TerminationTime != DateTime.MaxValue 
                                   ? 
                                   TerminationTime + DUTTimeShift : SubscriptionStarted.AddSeconds(TimeoutSeconds);

            Subscribed = true;
        }

        public DateTime GetMessages(int Count, out DateTime terminationTime, out NotificationMessageHolderType[] NotificationMessages)
        {
            var dump = "";
            var Current = DateTime.MinValue;
            if (Notify)
            {
                NotificationMessages = null;
                terminationTime = SubscriptionDeadline - DUTTimeShift;
            }
            else
            {
                bool UseRetry = false;
ReplayPullMessages:
                AutoRenew();

                try
                { Current = Proxy.PullMessages(PullMessagesRequestTimeout, NotificationResponeWaitTimeout, Count, out terminationTime, out NotificationMessages, out dump); }
                catch (FaultException<PullMessagesFaultResponseType> fault)
                {
                    baseTest.StepPassed();
                    baseTest.LogStepEvent(
                        string.Format(
                            "Exception of type FaultException<PullMessagesFaultResponseType> received. Will try to pull messages with new parameters{0}",
                            Environment.NewLine));
                    baseTest.Assert(!UseRetry,
                                    "New timeout from PullMessagesFaultResponse was not accepted",
                                    "Check that it is not double fault for this PullMesages request");
                    UseRetry = true;
                    baseTest.Assert(fault.Detail != null, "Detail field is null",
                                    "Check if correct parameters are specified in fault");
                    PullMessagesRequestTimeout = fault.Detail.MaxTimeout;
                    goto ReplayPullMessages;
                }
                //catch (TooLargeTimeoutException)
                //{
                //    baseTest.LogStepEvent("Too large PullMessages timeout is specified. Trying to use MessageTimeout instead.");
                //    PullMessagesRequestTimeout = XmlConvert.ToString(new TimeSpan(0, 0, 0, 0, baseTest._messageTimeout));
                //    goto ReplayPullMessages;
                //}
                catch (Exception exc)
                {
                    baseTest.StepFailed(exc);
                    throw;
                }
            }
            SubscriptionDeadline = terminationTime + DUTTimeShift;
            ValidateMessages(Count, NotificationMessages, dump, terminationTime, Current);

            return Current;
        }

        public DateTime GetMessages(int Count, out NotificationMessageHolderType[] NotificationMessages)
        {
            DateTime T;
            return GetMessages(Count, out T, out NotificationMessages);
        }

        public DateTime GetMessages(int Count,
                                    out DateTime terminationTime,
                                    out Dictionary<NotificationMessageHolderType, XmlElement> Notifications)
        {
            NotificationMessageHolderType[] NotificationMessages;
            string Dump = null;
            DateTime Current = DateTime.MinValue;
            if (Notify)
            {
                NotificationMessages = null;
                terminationTime = SubscriptionDeadline - DUTTimeShift;
            }
            else
            {
                bool UseRetry = false;
ReplayPullMessages:
                AutoRenew();

                try
                {
                    Current = Proxy.PullMessages(PullMessagesRequestTimeout, NotificationResponeWaitTimeout, Count, out terminationTime, out NotificationMessages, out Dump);
                }
                catch (FaultException<PullMessagesFaultResponseType> fault)
                {
                    baseTest.StepPassed();
                    baseTest.LogStepEvent(string.Format("Exception of type FaultException<PullMessagesFaultResponseType> received. Will try to pull messages with new parameters{0}", Environment.NewLine));
                    baseTest.Assert(!UseRetry, "New timeout from PullMessagesFaultResponse was not accepted", "Check that it is not double fault for this PullMesages request");
                    UseRetry = true;
                    baseTest.Assert(fault.Detail != null, "Detail field is null", "Check if correct parameters are specified in fault");
                    PullMessagesRequestTimeout = fault.Detail.MaxTimeout;
                    Count = fault.Detail.MaxMessageLimit;
                    goto ReplayPullMessages;
                }
                catch (Exception exc)
                {
                    baseTest.StepFailed(exc);
                    throw;
                }
            }
            SubscriptionDeadline = terminationTime + DUTTimeShift;
            ValidateMessages(Count, NotificationMessages, Dump, terminationTime, Current);
            XmlDocument doc = new XmlDocument();
            if (Dump != null)
            {
                doc.LoadXml(EventsMainHelper.SortOutHttpHeader(Dump));
            }
            XmlNamespaceManager manager = EventsMainHelper.CreateNamespaceManager(doc);

            var logger = new StringBuilder();
            Notifications = EventsMainHelper.GetRawElements(NotificationMessages, doc, manager, Notify, logger);

            if (logger.ToString().Any())
                baseTest.LogStepEvent(logger.ToString());

            return Current;
        }

        public DateTime GetMessages(int Count,
                                    out Dictionary<NotificationMessageHolderType, XmlElement> Notifications)
        {
            DateTime T;
            return GetMessages(Count, out T, out Notifications);
        }

        public DateTime GetMessages(int Count,
                                    Action SomethingInTheMiddle,
                                    string MessageToLog,
                                    out DateTime terminationTime,
                                    out Dictionary<NotificationMessageHolderType, XmlElement> Notifications)
        {
            NotificationMessageHolderType[] NotificationMessages;
            string Dump = null;
            DateTime Current = DateTime.MinValue;
            if (Notify)
            {
                Notify notify = Server.WaitForNotifyOnly(SomethingInTheMiddle, NotificationResponeWaitTimeout, baseTest.Semaphore.StopEvent);

                NotificationMessages = notify.NotificationMessage;
                Dump = System.Text.Encoding.UTF8.GetString(Server.RawData);

                terminationTime = SubscriptionDeadline - DUTTimeShift;
                Current = DateTime.Now - DUTTimeShift;

                ValidateNotificationsPacket(Server.RawData);
                ValidateHeaders(Server.SoapHeaders);
            }
            else
            {
                bool UseRetry = false;
ReplayPullMessages:
                AutoRenew();

                try
                {
                    Current = Proxy.PullMessages(PullMessagesRequestTimeout, NotificationResponeWaitTimeout, Count, SomethingInTheMiddle, MessageToLog, out terminationTime, out NotificationMessages, out Dump);
                }
                catch (FaultException<PullMessagesFaultResponseType> fault)
                {
                    baseTest.StepPassed();
                    baseTest.LogStepEvent(string.Format("Exception of type FaultException<PullMessagesFaultResponseType> received. Will try to pull messages with new parameters{0}", Environment.NewLine));
                    baseTest.Assert(!UseRetry, "New timeout from PullMessagesFaultResponse was not accepted", "Check that it is not double fault for this PullMesages request");
                    UseRetry = true;
                    baseTest.Assert(fault.Detail != null, "Detail field is null", "Check if correct parameters are specified in fault");
                    PullMessagesRequestTimeout = fault.Detail.MaxTimeout;
                    goto ReplayPullMessages;
                }
                catch (Exception exc)
                {
                    baseTest.StepFailed(exc);
                    throw;
                }
            }
            SubscriptionDeadline = terminationTime + DUTTimeShift;
            ValidateMessages(Count, NotificationMessages, Dump, terminationTime, Current);
            XmlDocument doc = new XmlDocument();
            if (Dump != null)
            {
                if (Notify)
                {
                    doc.LoadXml(Dump);
                }
                else
                {
                    doc.LoadXml(EventsMainHelper.SortOutHttpHeader(Dump));
                }
            }
            XmlNamespaceManager manager = EventsMainHelper.CreateNamespaceManager(doc);
            var logger = new StringBuilder();
            Notifications = EventsMainHelper.GetRawElements(NotificationMessages, doc, manager, Notify, logger);

            if (logger.ToString().Any())
                baseTest.LogStepEvent(logger.ToString());

            return Current;
        }

        public DateTime GetMessages(int Count,
                                    Action SomethingInTheMiddle,
                                    string MessageToLog,
                                    out Dictionary<NotificationMessageHolderType, XmlElement> Notifications)
        {
            DateTime T;
            return GetMessages(Count, SomethingInTheMiddle, MessageToLog, out T, out Notifications);
        }

        public enum WaitCondition
        {
            WC_Timeout      = 1,    //! wait until timeout
            WC_Condition    = 2,    //! wait until condition
            WC_Filter       = 4,    //! filter data by condition
            WC_KeepLast     = 8,    //! keep only last (maybe filtered) event
            WC_AllExit      = 3,    //! exit by either timeout or condition
            WC_TimeAndFilter= 5,    //! exit by timeout, use filtered
            WC_TimeAndFilterLast = 13,//! exit by timeout, use last filtered
            WC_NeedCall     = 6,    //! need to use condition function
            WC_TimeFilterCondition = 7,//! exif by timeout or condition, use filtered
            WC_ALL = 7     //! deprecated, migrate to WC_TimeFilterCondition
        };

        public bool WaitMessages(
            int WaitSeconds,
            Func<NotificationMessageHolderType, bool> messageChecker,
            WaitCondition Condition,
            out Dictionary<NotificationMessageHolderType, XmlElement> Notifications
            )
        {
            EventsVerifyPolicy localEventsVerifyPolicy = eventsVerifyPolicy;
            eventsVerifyPolicy = new EventsVerifyPolicy(Notify);
            eventsVerifyPolicy.VerifyMessagesPresence = false;
            try
            {
                DateTime Deadline = DateTime.Now.AddSeconds(WaitSeconds);
                Notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                Dictionary<NotificationMessageHolderType, XmlElement> NotificationsStep = null;
                do
                {
                    GetMessages(1, out NotificationsStep);
                    NotificationMessageHolderType Last = null;
                    foreach (NotificationMessageHolderType l in NotificationsStep.Keys)
                    {
                        Last = l;
                    }
                    if (Last != null)
                    {
                        bool Check = false;
                        if (((Condition & WaitCondition.WC_NeedCall) != 0) && (messageChecker != null))
                        {
                            Check = messageChecker(Last);
                        }
                        if (((Condition & WaitCondition.WC_Filter) == 0) || Check)
                        {
                            if ((Condition & WaitCondition.WC_KeepLast) != 0)
                            {
                                Notifications = NotificationsStep;
                            }
                            else
                            {
                                Notifications.Add(Last, NotificationsStep[Last]);
                            }
                        }
                        if (((Condition & WaitCondition.WC_Condition) != 0) && Check)
                        {
                            return true;
                        }
                    }
                    if (((Condition & WaitCondition.WC_Timeout) != 0) && (Deadline <= DateTime.Now))
                    {
                        return false;
                    }
                }
                while (true);
                //return false;
            }
            finally
            {
                eventsVerifyPolicy = localEventsVerifyPolicy;
            }
        }

        #region Polling Condition

        internal abstract class PollingConditionBase
        {
            protected PollingConditionBase(int timeout)
            {
                Deadline = DateTime.Now.AddSeconds(timeout);
            }

            protected PollingConditionBase(DateTime deadline)
            {
                Deadline = deadline;
            }

            public DateTime Deadline { get; set; }

            //Should return true when waiting condition is met.
            public abstract bool StopPulling { get; }
            public abstract string Reason { get; }

            //This member is called at the end of each pulling iteration to check if waiting condition is met.
            public abstract void Update(Dictionary<NotificationMessageHolderType, XmlElement> messages);

            //This member is used to filter messages which should be processed.
            public delegate bool MessageFilter(NotificationMessageHolderType msg);
            public MessageFilter Filter;

            //After messages are filtered, this member is used to make some transformation of received messages.
            public delegate NotificationMessageHolderType MessageTransformer(NotificationMessageHolderType msg);
            public MessageTransformer Transformer;
        }

        public class WaitNotificationsDuringTimeoutPollingCondition : PollingConditionBase
        {
            public WaitNotificationsDuringTimeoutPollingCondition(int timeout) : base(timeout)
            {}

            public bool TimeoutHasElapsed = false;

            public override bool StopPulling
            {
                get { return false; }
            }

            public override string Reason
            {
                get
                { return "Timeout has elapsed"; }
            }

            public override void Update(Dictionary<NotificationMessageHolderType, XmlElement> messages)
            {
                TimeoutHasElapsed = DateTime.Now > Deadline;
            }
        }

        public class WaitFirstNotificationPollingCondition : PollingConditionBase
        {
            public WaitFirstNotificationPollingCondition(int timeout): base(timeout)
            { }

            private bool m_FirstNotificationReceived;

            public override bool StopPulling
            {
                get { return m_FirstNotificationReceived; }
            }

            public override string Reason
            {
                get
                { return string.Format("Required notification {0} received", m_FirstNotificationReceived ? "is" : "isn't"); }
            }

            public override void Update(Dictionary<NotificationMessageHolderType, XmlElement> messages)
            {
                m_FirstNotificationReceived = m_FirstNotificationReceived || messages.Any();
            }
        }
        #endregion



        public bool WaitMessages(int messageLimit,
                                 PollingConditionBase condition,
                                 out Dictionary<NotificationMessageHolderType, XmlElement> notifications)
        {
            EventsVerifyPolicy localEventsVerifyPolicy = eventsVerifyPolicy;
            eventsVerifyPolicy = new EventsVerifyPolicy(Notify) { VerifyMessagesPresence = false };

            notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
            try
            {
                //Dictionary<NotificationMessageHolderType, XmlElement> lastReceived;
                do
                {
                    Dictionary<NotificationMessageHolderType, XmlElement> lastReceived;
                    GetMessages(messageLimit, out lastReceived);

                    if (null != condition.Filter)
                        lastReceived = lastReceived.Where(e => condition.Filter(e.Key)).ToDictionary(e => e.Key, e => e.Value);

                    if (null != condition.Transformer)
                        lastReceived = lastReceived.ToDictionary(e => condition.Transformer(e.Key), e => e.Value);

                    foreach (var e in lastReceived)
                    {
                        notifications.Add(e.Key, e.Value);
                    }

                    condition.Update(lastReceived);

                    if (DateTime.Now > condition.Deadline)
                        break;

                } while (!condition.StopPulling);
            }
            finally
            {
                eventsVerifyPolicy = localEventsVerifyPolicy;
            }

            return condition.StopPulling;
        }

        public void SetSynchronizationPoint()
        {
            Proxy.SetSynchronizationPoint();
        }

        public void Unsubscribe(bool Silent)
        {
            if (!Subscribed)
            {
                return;
            }
            Subscribed = false;
            if (SubscriptionStarted != DateTime.MinValue)
            {
                if (SubscriptionDeadline > DateTime.Now)
                {
                    bool UseTimeout = true;
                    try
                    {
                        Proxy.Unsubscribe(new Unsubscribe());
                        UseTimeout = false;
                    }
                    catch (Exception exc)
                    {
                        baseTest.LogStepEvent(string.Format("Failed to unsubscribe through request. Error received: {0}", exc.Message));
                        baseTest.StepPassed();
                    }
                    if (UseTimeout)
                    {
                        int Timeout = Convert.ToInt32((SubscriptionDeadline - DateTime.Now).TotalSeconds);
                        if (Timeout > 0)
                        {
                            BaseTest.RunStep(baseTest, () => { baseTest.Sleep(Timeout * 1000); }, "Wait until Subscription Manager is deleted by timeout");
                        }
                    }
                }
                else
                {
                    BaseTest.LogTestEvent(baseTest, "Subscription deleted by Timeout");
                }
            }
            else
            {
                //BaseTest.LogTestEvent(baseTest, "Subscription was not created - nothing to delete");
            }
            if (Notify)
            {
                if (Server != null)
                {
                    Server.StopNotify();
                }
                Server = null;
            }
        }
        static public void Unsubscribe(SubscriptionHandler Handler)
        {
            if (Handler != null)
            {
                Handler.Unsubscribe(true);
            }
            else
            {
                //BaseTest.LogTestEvent(Handler.baseTest, "Subscription was not created - nothing to delete");
            }
        }
        void AutoRenew()
        {
            if (KeepAlive)
            {
                return;
            }

            DateTime LocalDeadline = DateTime.Now.AddSeconds(PullMessagesRequestTimeout.DurationToSeconds() + 2);
            if (SubscriptionDeadline < LocalDeadline)
            {
                Renew();
            }
        }
        void Renew()
        {
            Renew renew = new Renew();
            renew.TerminationTime = string.Format("PT{0}S", RenewTimeout);
            Renew(renew);
        }
        public void Renew(int Seconds)
        {
            Renew renew = new Renew();
            renew.TerminationTime = string.Format("PT{0}S", Seconds);
            Renew(renew);
        }
        public void Renew(DateTime datetime)
        {
            Renew renew = new Renew();
            renew.TerminationTime = (datetime - DUTTimeShift).ToString("yyyy-MM-ddTHH:mm:ssZ");
            Renew(renew);
        }
        void Renew(Renew InitialRenew)
        {
            Renew renew = InitialRenew;
repeatRenew:

            RenewResponse Resp = null;
            try
            {
                Resp = Proxy.Renew(renew);
            }
            catch (FaultException<UnacceptableTerminationTimeFaultType> exc)
            {
                BaseTest.LogTestEvent(baseTest, "Possible exception - trying to select Renew time and repeat");
                baseTest.StepPassed();
                DateTime minimumTime = EventsMainHelper.GetDateTimeFromFault(exc,
                    "MinimumTime",
                    "http://docs.oasis-open.org/wsn/b-2");

                baseTest.Assert(minimumTime != DateTime.MinValue,
                    "Fault details or MinimumTime not found",
                    "Check if MinimumTime is specified");

                DateTime timestamp = EventsMainHelper.GetDateTimeFromFault(exc,
                    BaseNotification.TIMESTAMP,
                    BaseNotification.WSRFBFNAMESPACE);

                minimumTime += DUTTimeShift;
                if (timestamp != DateTime.MinValue)
                {
                    timestamp += DUTTimeShift;
                }
                else
                {
                    timestamp = System.DateTime.Now;
                }
                int diffSeconds = Convert.ToInt32((minimumTime - timestamp).TotalSeconds);
                if (diffSeconds < PullMessagesRequestTimeout.DurationToSeconds())
                {   // just for case, any big value can be there
                    if (RenewTimeout >= 90)
                    {
                        RenewTimeout += 30;
                    }
                    else
                    {
                        RenewTimeout = 90;
                    }
                }
                else
                {
                    if (RenewTimeout == diffSeconds)
                    {
                        baseTest.BeginStep("Check that it is not double fault for this Renew request");
                        BaseTest.LogTestEvent(baseTest, "Renew proposes timeout that it just denied");
                        throw;
                    }
                    RenewTimeout = diffSeconds;
                }
                if (RenewTimeout > 300)
                {
                    baseTest.BeginStep("Check that it is not multiple fault for this Renew request");
                    BaseTest.LogTestEvent(baseTest, "Can't find Renew time within 300 seconds limit - terminating test");
                    throw;
                }
                renew.TerminationTime = string.Format("PT{0}S", RenewTimeout);
                goto repeatRenew;
            }
            if (!Resp.TerminationTime.HasValue)
            {
                SubscriptionDeadline = System.DateTime.Now.AddSeconds(RenewTimeout - 2);
            }
            else
            {
                SubscriptionDeadline = Resp.TerminationTime.Value + DUTTimeShift;
            }
        }

        const string WSA_2005 = "http://www.w3.org/2005/08/addressing";

        protected void ValidateHeaders(List<XmlElement> headers)
        {
            baseTest.BeginStep("Validate Headers");
            XmlElement replyToHeader = headers.Where(h => h.LocalName == "ReplyTo" && h.NamespaceURI == WSA_2005).FirstOrDefault();
            if (replyToHeader != null)
            {
                XmlElement messageId = headers.Find(h => h.LocalName == "MessageID" && h.NamespaceURI == WSA_2005);
                if (messageId == null)
                {
                    throw new AssertException("ReplyTo header is present, but MessageID header not found");
                }
                else
                {
                    string value = messageId.InnerText;
                }
            }
            baseTest.StepPassed();
        }

        protected void ValidateNotificationsPacket(byte[] data)
        {
            baseTest.BeginStep("Validate notifications SOAP packet");
            MemoryStream stream = new MemoryStream(data);
            SoapValidator validator = new SoapValidator(EventsSchemasSet.GetInstance());
            try
            {
                validator.Validate(stream);
            }
            catch (Exception exc)
            {
                stream.Close();
                throw;
            }
            baseTest.StepPassed();
        }
        void ValidateMessages(int Count,
                              NotificationMessageHolderType[] NotificationMessages,
                              string Dump,
                              DateTime TerminationTime,
                              DateTime CurrentTime)
        {
            if (eventsVerifyPolicy == null)
            {
                return;
            }

            if (eventsVerifyPolicy.VerifyMessagesPresence)
            {
                baseTest.Assert(NotificationMessages != null && NotificationMessages.Any(),
                                "No notification messages received",
                                "Check that DUT sent notification messages");
            }
            if (eventsVerifyPolicy.VerifyTerminationTime)
            {
                baseTest.Assert(CurrentTime < TerminationTime,
                                "TerminationTime <= CurrentTime",
                                "Validate CurrentTime and TerminationTime");
            }
            if (eventsVerifyPolicy.VerifyMessageLimit)
            {
                baseTest.Assert(NotificationMessages.Length <= Count,
                                "Maximum number of messages exceeded",
                                string.Format("Check that a maximum number of {0} Notification Messages is included in PullMessagesResponse", Count));
            }
            if (eventsVerifyPolicy.VerifyDataPresence)
            {
                baseTest.Assert(!string.IsNullOrEmpty(Dump),
                                "No response recieved to PullMessages request",
                                "Response is not empty");
            }
        }
    }




    class TopicSetHelper
    {
        BaseOnvifTest baseTest;
        TopicSetType topicSet;
        bool Notify;
        public TopicSetHelper(
            BaseOnvifTest baseTest, 
            TopicSetType topicSet, 
            bool Notify
            )
        {
            this.baseTest = baseTest;
            this.topicSet = topicSet;
            this.Notify = Notify;
        }
        public bool HasTopics()
        {
            return !(topicSet == null || topicSet.Any == null || topicSet.Any.Length == 0);
        }
        public bool PropertyTopics()
        {
            List<XmlElement> topics = new List<XmlElement>();
            foreach (XmlElement element in topicSet.Any)
            {
                FindTopics(element, topics);
            }

            List<TopicInfo> propertyTopics = FindPropertyTopics(topics);
            return propertyTopics.Count > 0;
        }
        public void ValidateMessages(
            Dictionary<NotificationMessageHolderType, XmlElement> Notifications,
            FilterInfo filter
            )
        {
            baseTest.BeginStep("Validate messages");
            List<XmlElement> topics = new List<XmlElement>();
            foreach (XmlElement element in topicSet.Any)
            {
                FindTopics(element, topics);
            }

            foreach (NotificationMessageHolderType message in Notifications.Keys)
            {
                XmlNamespaceManager manager =
                    EventsMainHelper.CreateNamespaceManager(Notifications[message].OwnerDocument);
                string reason;
                if (!IsValidMessage(message,
                    Notifications[message],
                    manager,
                    filter,
                    topics,
                    out reason))
                {
                    throw new AssertException(reason);
                }
            }
            baseTest.StepPassed();
        }
        void FindTopics(
            XmlElement element, 
            List<XmlElement> topics
            )
        {
            if (element.RepresentsTopic())
            {
                topics.Add(element);
            }

            // If not a topic - enumerate child elements.
            foreach (XmlNode node in element.ChildNodes)
            {
                XmlElement child = node as XmlElement;
                if (child == null)
                {
                    continue;
                    ;
                }
                FindTopics(child, topics);
            }
        }
        List<TopicInfo> FindPropertyTopics(
            List<XmlElement> topics
            )
        {
            List<TopicInfo> propertyTopics = new List<TopicInfo>();
            foreach (XmlElement topicElement in topics)
            {
                XmlElement messageDescription = topicElement.GetMessageDescription();
                if (messageDescription != null)
                {
                    bool isProperty = false;
                    // check that it is a property
                    if (messageDescription.HasAttribute(OnvifMessage.ISPROPERTY))
                    {
                        isProperty = XmlConvert.ToBoolean(messageDescription.Attributes[OnvifMessage.ISPROPERTY].Value);
                    }
                    if (isProperty)
                    {
                        System.Diagnostics.Debug.WriteLine("--> PROPERTY");
                        propertyTopics.Add(TopicInfo.ConstructTopicInfo(topicElement));
                    }
                }
            }
            return propertyTopics;
        }
        public List<XmlElement> FindTopics()
        {
            List<XmlElement> topics = new List<XmlElement>();
            if (topicSet.Any != null)
            {
                foreach (XmlElement element in topicSet.Any)
                {
                    FindTopics(element, topics);
                }
            }

            return topics;
        }



        protected bool IsValidMessage(
            NotificationMessageHolderType notification,
            XmlElement messageRawElement,
            XmlNamespaceManager manager,
            FilterInfo filter,
            List<XmlElement> topics,
            out string reason
            )
        {
            if (notification.Topic == null)
            {
                reason = "Topic is null";
                return false;
            }

            XmlText text = null;
            if (notification.Topic.Any != null)
            {
                foreach (XmlNode any in notification.Topic.Any)
                {
                    XmlText current = any as XmlText;
                    if (any != null)
                    {
                        text = current;
                        break;
                    }
                }
            }

            XmlNode topicNode = messageRawElement.SelectSingleNode("b2:Topic", manager);

            string topic = text != null ? text.Value : "";

            //TopicInfo actualTopic = TopicInfo.ExtractTopicInfoAll(topic, topicNode);
            //[22.03.2013] AKS: using ExtractTopicInfoPACS instead of ExtractTopicInfoAll.
            TopicInfo actualTopic = TopicInfo.ExtractTopicInfoPACS(topic, topicNode);

            TopicInfo currentTopic = actualTopic;
            while (currentTopic != null)
            {
                if (currentTopic.ParentTopic == null && string.IsNullOrEmpty(currentTopic.NamespacePrefix))
                {
                    reason = string.Format("Topic {0} is incorrect: root topic must have namespace defined", topic);
                    return false;
                }
                if (!string.IsNullOrEmpty(currentTopic.NamespacePrefix) && string.IsNullOrEmpty(currentTopic.Namespace))
                {
                    reason = string.Format("Topic {0} is incorrect: namespace prefix {1} not defined", topic, currentTopic.NamespacePrefix);
                    return false;
                }
                currentTopic = currentTopic.ParentTopic;
            }

            if (filter != null && filter.Topic != null)
            {
                // validate topic

                string expectedTopicDescription = filter.Topic.GetDescription();
                string actualTopicDescription = actualTopic.GetDescription();

                bool match = TopicInfo.TopicsMatch(actualTopic, filter.Topic);

                if (!match)
                {
                    reason = string.Format("Invalid topic. {0}Expected: {1}{0}Actual: {2}",
                        Environment.NewLine,
                        expectedTopicDescription,
                        actualTopicDescription);
                    return false;
                }
            }

            XmlElement messageTopic = null;
            foreach (XmlElement topicElement in topics)
            {
                TopicInfo info = TopicInfo.ConstructTopicInfo(topicElement);
                if (TopicInfo.TopicsMatch(info, actualTopic))
                {
                    messageTopic = topicElement;
                    break;
                }
            }

            return IsValidMessageElement(notification.Message, messageTopic, filter, out reason);
        }


        protected bool IsValidMessageElement(
            XmlElement message,
            XmlElement topicElement,
            FilterInfo filter,
            out string reason
            )
        {
            if (message == null)
            {
                reason = "Message element not found";
                return false;
            }

            // Check that mandatory attribute is present.
            if (!message.HasAttribute(OnvifMessage.UTCTIMEATTRIBUTE))
            {
                reason = "Mandatory attribute UtcTime not found";
                return false;
            }

            // check UtcTime format
            string utcTimeValue = message.Attributes[OnvifMessage.UTCTIMEATTRIBUTE].Value;
            // xs:dateTime string

            if (!EventServiceUtils.IsValidXsdDateTime(utcTimeValue))
            {
                reason = string.Format("'{0}' is not valid xs:datetime value", utcTimeValue);
                return false;
            }

            XmlElement messageDescription = null;
            if (topicElement != null)
            {
                messageDescription = topicElement.GetMessageDescription();
            }

            // if message description found - check that message is valid accordingly to the filter.
            if (messageDescription != null)
            {
                bool isProperty = false;
                // check that it is a property
                if (messageDescription.HasAttribute(OnvifMessage.ISPROPERTY))
                {
                    isProperty = XmlConvert.ToBoolean(messageDescription.Attributes[OnvifMessage.ISPROPERTY].Value);
                }

                // if topic is Property topic
                if (isProperty)
                {
                    if (message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                    {
                        XmlAttribute propertyOperationType = message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];

                        bool match = false;
                        foreach (string allowedPropertyOperation in new string[] { OnvifMessage.INITIALIZED, OnvifMessage.CHANGED, OnvifMessage.DELETED })
                        {
                            if (propertyOperationType.Value == allowedPropertyOperation)
                            {
                                match = true;
                                break;
                            }
                        }

                        if (!match)
                        {
                            reason = string.Format("   PropertyOperation attribute has unexpected value: {0}",
                                                   propertyOperationType.Value);
                            return false;
                        }
                    }
                    else
                    {
                        reason = "   PropertyOperation attribute not found";
                        return false;
                    }
                }
            }

            List<string> allItems = new List<string>();

            foreach (XmlNode node in message.ChildNodes)
            {
                XmlElement child = node as XmlElement;
                if (child == null)
                {
                    continue;
                }

                if (child.LocalName != OnvifMessage.SOURCE &&
                    child.LocalName != OnvifMessage.KEY &&
                    child.LocalName != OnvifMessage.DATA &&
                    child.LocalName != OnvifMessage.EXTENSION)
                {
                    reason = string.Format("Unexpected element: {0}", child.Name);
                    return false;
                }

                if (child.NamespaceURI != OnvifMessage.ONVIF)
                {
                    reason = string.Format("Element {0} is not from expected namespace", child.Name);
                    return false;
                }

                if (child.LocalName != OnvifMessage.EXTENSION)
                {
                    List<string> items = new List<string>();

                    // content should be tt:ItemList
                    foreach (XmlNode childNode in child.ChildNodes)
                    {
                        XmlElement item = childNode as XmlElement;
                        if (item == null)
                        {
                            continue;
                        }
                        switch (item.LocalName)
                        {
                            case OnvifMessage.SIMPLEITEM:
                                {
                                    if (!item.HasAttribute(OnvifMessage.NAME))
                                    {
                                        reason = string.Format("Element {0} has no mandatory 'Name' attribute",
                                                               item.Name);
                                        return false;
                                    }
                                    string name = item.Attributes[OnvifMessage.NAME].Value;
                                    if (items.Contains(name))
                                    {
                                        reason = string.Format("Name {0} is not unique", name);
                                        return false;
                                    }

                                    items.Add(name);

                                    if (!item.HasAttribute(OnvifMessage.VALUE))
                                    {
                                        reason = string.Format("Element {0} has no mandatory 'Value' attribute",
                                                               item.Name);
                                        return false;
                                    }
                                }
                                break;
                            case OnvifMessage.ELEMENTITEM:
                                {
                                    if (!item.HasAttribute(OnvifMessage.NAME))
                                    {
                                        reason = string.Format("Element {0} has no mandatory 'Name' attribute",
                                                               item.Name);
                                        return false;
                                    }

                                    string name = item.Attributes[OnvifMessage.NAME].Value;
                                    if (items.Contains(name))
                                    {
                                        reason = string.Format("Name {0} is not unique", name);
                                        return false;
                                    }

                                    items.Add(name);

                                }
                                break;
                            case OnvifMessage.ITEMLISTEXTENSION:
                                {

                                }
                                break;
                            default:
                                {
                                    reason = string.Format("Unexpected element: {0}", item.Name);
                                    return false;
                                }
                        }
                    }

                    allItems.AddRange(items);
                }
            }

            reason = string.Empty;
            return true;
        }


    }

    [TestClass]
    class PullPointEventsTest : EventTest
    {
        public PullPointEventsTest(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATHBN = "Event Handling\\Basic Notification";
        private const string PATHPP = "Event Handling\\Real-Time Pull-Point Notification Interface";
        /*
        [Test(Name = "BASIC NOTIFICATION INTERFACE - NOTIFY",
            Path = PATHBN,
            Order = "02.01.00",
            Id = "2-1-0",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, Functionality.EventsSetSynchronizationPoint })]
        public void CreatePullPointSubscriptionTest2()
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            SubscriptionHandler Handler = new SubscriptionHandler(this, true);
            Handler.SetPolicy(new EventsVerifyPolicy(true));

            int MessagesCount = 0;

            Func<NotificationMessageHolderType, bool> messageCheck =
                        (n) => 
                        { 
                            return MessagesCount++ > 3;  
                        };

            RunTest(
            () =>
            {
                Handler.SetAddress(GetEventServiceAddress());
                TopicSetType topicSet = Handler.GetTopicSet();
                TopicSetHelper Helper = new TopicSetHelper(this, topicSet, false);
                if (!Helper.HasTopics())
                {
                    LogTestEvent(string.Format("The DUT provides no topics. Test passed.{0}", Environment.NewLine));
                    return;
                }
                if (!Helper.PropertyTopics())
                {
                    bool bContinue =
                        Operator.GetOkCancelAnswer("No property events found. You'll have to trigger an event manually.");
                    Assert(bContinue, "Operator cancelled the test", "Check if test should be continued");
                }
                LogTestEvent(string.Format("Timeout of {0} seconds will be used{1}", actualTerminationTime, Environment.NewLine));

                Handler.Subscribe(null, actualTerminationTime);
                Dictionary<NotificationMessageHolderType, XmlElement> Notifications;
                Action SetSyncPoint = () => { Handler.SetSynchronizationPoint();  };
                Handler.GetMessages(2, SetSyncPoint, null, out Notifications);
                //Handler.WaitMessages(200, messageCheck, out Notifications);
                Helper.ValidateMessages(Notifications, null);
            },
            () =>
            {
                SubscriptionHandler.Unsubscribe(Handler);
            });
        }
    
        [Test(Name = "REALTIME PULLPOINT SUBSCRIPTION - GETMESSAGES",
            Path = PATHPP,
            Order = "03.01.00",
            Id = "3-1-0",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.CreatePullPointSubscription, Functionality.EventsSetSynchronizationPoint })]
        public void CreatePullPointSubscriptionTest3()
        {
            int actualTerminationTime = 60;
            if (_eventSubscriptionTimeout != 0)
            {
                actualTerminationTime = _eventSubscriptionTimeout;
            }
            SubscriptionHandler Handler = new SubscriptionHandler(this, false);
            Handler.SetPolicy(new EventsVerifyPolicy(false));

            int MessagesCount = 0;

            Func<NotificationMessageHolderType, bool> messageCheck =
                        (n) =>
                        {
                            return MessagesCount++ > 3;
                        };

            RunTest(
            () =>
            {
                Handler.SetAddress(GetEventServiceAddress());
                TopicSetType topicSet = Handler.GetTopicSet();
                TopicSetHelper Helper = new TopicSetHelper(this, topicSet, false);
                if (!Helper.HasTopics())
                {
                    LogTestEvent(string.Format("The DUT provides no topics. Test passed.{0}", Environment.NewLine));
                    return;
                }
                if (!Helper.PropertyTopics())
                {
                    bool bContinue =
                        Operator.GetOkCancelAnswer("No property events found. You'll have to trigger an event manually.");
                    Assert(bContinue, "Operator cancelled the test", "Check if test should be continued");
                }
                LogTestEvent(string.Format("Timeout of {0} seconds will be used{1}", actualTerminationTime, Environment.NewLine));

                Handler.Subscribe(null, actualTerminationTime);
                Dictionary<NotificationMessageHolderType, XmlElement> Notifications;
                Action SetSyncPoint = () => { Handler.SetSynchronizationPoint(); };
                Handler.GetMessages(2, SetSyncPoint, null, out Notifications);
                //Handler.WaitMessages(200, messageCheck, out Notifications);
                Helper.ValidateMessages(Notifications, null);
            },
            () =>
            {
                SubscriptionHandler.Unsubscribe(Handler);
            });
        }*/
    }
}
