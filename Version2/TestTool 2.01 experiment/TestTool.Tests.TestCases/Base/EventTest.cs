///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Event;
using System.Xml;
using System.Xml.Schema;
using System.ServiceModel;

namespace TestTool.Tests.TestCases.Base
{
    /// <summary>
    /// Base class for Events tests.
    /// Contains wrapper for proxy method calls and common validation methods.
    /// </summary>
    public class EventTest : EventBaseTest
    {
        protected EventTest(TestLaunchParam param)
            : base(param)
        {
            
        }

        /// <summary>
        /// Gets event properties. Creates client, if necessary.
        /// </summary>
        /// <param name="FixedTopicSet"></param>
        /// <param name="TopicSet"></param>
        /// <param name="TopicExpressionDialect"></param>
        /// <param name="MessageContentFilterDialect"></param>
        /// <param name="ProducerPropertiesFilterDialect"></param>
        /// <param name="MessageContentSchemaLocation"></param>
        /// <param name="Any"></param>
        /// <returns></returns>
        /// <remarks>As this method uses EventPortType client, the client can be created without additional data.</remarks>
        protected string[] GetEventProperties(out bool FixedTopicSet,
            out TopicSetType TopicSet,
            out string[] TopicExpressionDialect,
            out string[] MessageContentFilterDialect,
            out string[] ProducerPropertiesFilterDialect,
            out string[] MessageContentSchemaLocation,
            out XmlElement[] Any)
        {
            EnsureEventPortTypeClientCreated();

            string[] response = null;

            bool fixedTopicSetCopy = false;
            TopicSetType topicSetCopy = null;
            string[] topicExpressionDialectCopy = null;
            string[] messageContentFilterDialectCopy = null;
            string[] producerPropertiesFilterDialectCopy = null;
            string[] messageContentSchemaLocationCopy = null;
            XmlElement[] anyCopy = null;

            RunStep(() =>
            {
                response = _eventPortTypeClient.GetEventProperties(out fixedTopicSetCopy,
                                                     out topicSetCopy,
                                                     out topicExpressionDialectCopy,
                                                     out messageContentFilterDialectCopy,
                                                     out producerPropertiesFilterDialectCopy,
                                                     out messageContentSchemaLocationCopy,
                                                     out anyCopy);
            },
                         "Get Event Properties");

            FixedTopicSet = fixedTopicSetCopy;
            TopicSet = topicSetCopy;
            TopicExpressionDialect = topicExpressionDialectCopy;
            MessageContentFilterDialect = messageContentFilterDialectCopy;
            ProducerPropertiesFilterDialect = producerPropertiesFilterDialectCopy;
            MessageContentSchemaLocation = messageContentSchemaLocationCopy;
            Any = anyCopy;

            return response;
        }

        /// <summary>
        /// Wrapper for Renew method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>As this method uses SubscriptionManager client, care should be taken that client is created.
        /// It's not posisble to create it here since Subscribe method should be called first to get endpoint address.</remarks>
        protected RenewResponse Renew(Renew request)
        {
            return Renew(request, "Renew subscription");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="stepName"></param>
        /// <returns></returns>
        /// <remarks>As this method uses SubscriptionManager client, care should be taken that client is created.
        /// It's not posisble to create it here since Subscribe method should be called first to get endpoint address.</remarks>
        protected RenewResponse Renew(Renew request, string stepName)
        {
            RenewResponse response = null;

            RunStep(() =>
            {
                response = _subscriptionManagerClient.Renew(request);

            }, stepName);

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>As this method uses SubscriptionManager client, care should be taken that client is created.
        /// It's not posisble to create it here since Subscribe method should be called first to get endpoint address.</remarks>
        protected UnsubscribeResponse Unsubscribe(Unsubscribe request)
        {
            UnsubscribeResponse response = null;

            RunStep(() =>
            {
                response = _subscriptionManagerClient.Unsubscribe(request);
            }, "Send unsubscribe request");


            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Filter"></param>
        /// <param name="InitialTerminationTime"></param>
        /// <param name="SubscriptionPolicy"></param>
        /// <param name="Any"></param>
        /// <param name="CurrentTime"></param>
        /// <param name="TerminationTime"></param>
        /// <returns></returns>
        /// <remarks>As this method uses EventPortType client, the client can be created without additional data.</remarks>
        protected EndpointReferenceType CreatePullPointSubscription(
            FilterType Filter,
            string InitialTerminationTime,
            CreatePullPointSubscriptionSubscriptionPolicy SubscriptionPolicy,
            ref XmlElement[] Any,
            out DateTime CurrentTime,
            out System.Nullable<System.DateTime> TerminationTime)
        {

            EnsureEventPortTypeClientCreated();

            EndpointReferenceType result = null;

            System.Xml.XmlElement[] anyCopy = Any;
            System.DateTime currentTimeCopy = DateTime.MinValue;
            DateTime? terminationTimeCopy = DateTime.MinValue;

            RunStep(() =>
            {
                result = _eventPortTypeClient.CreatePullPointSubscription(
                    Filter,
                    InitialTerminationTime,
                    SubscriptionPolicy,
                    ref anyCopy,
                    out currentTimeCopy,
                    out terminationTimeCopy);
            }, "Create Pull Point Subsciption");

            CurrentTime = currentTimeCopy;
            TerminationTime = terminationTimeCopy;

            return result;
        }


        /// <summary>
        /// Gets TopicSet
        /// </summary>
        /// <returns></returns>
        protected TopicSetType GetTopicSet()
        {
            // service client to get event information
            EnsureEventPortTypeClientCreated();

            string[] response = null;

            bool fixedTopicSet = false;
            TopicSetType topicSet = null;
            string[] topicExpressionDialect = null;
            string[] messageContentFilterDialect = null;
            string[] producerPropertiesFilterDialect = null;
            string[] messageContentSchemaLocation = null;
            XmlElement[] any = null;

            // query properties
            response = GetEventProperties(out fixedTopicSet,
                               out topicSet,
                               out topicExpressionDialect,
                               out messageContentFilterDialect,
                               out producerPropertiesFilterDialect,
                               out messageContentSchemaLocation,
                               out any);

            return topicSet;
        }

        protected SubscribeResponse Subscribe(Subscribe request)
        {
            return Subscribe(request, "Send Subscribe request");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>As this method uses NotificationProducer client, the client can be created without additional data.</remarks>
        protected SubscribeResponse Subscribe(Subscribe request, string stepName)
        {
            EnsureNotificationProducerClientCreated();

            SubscribeResponse response = null;
            RunStep(() =>
            {
                response = _notificationProducerClient.Subscribe(request);
            }, stepName);

            return response;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>As this method uses PullPointSubscription client, care should be taken that client is created.
        /// It's not posisble to create it here since Subscribe method should be called first to get endpoint address.</remarks>
        protected void SetSynchronizationPoint()
        {
            RunStep(() =>
                        {
                            _pullPointSubscriptionClient.SetSynchronizationPoint();
                        }, "Set Synchronization Point"

            );
       
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="messageLimit"></param>
        /// <param name="any"></param>
        /// <param name="terminationTime"></param>
        /// <param name="notificationMessage"></param>
        /// <returns></returns>
        /// <remarks>As this method uses PullPointSubscription client, care should be taken that client is created.
        /// It's not posisble to create it here since Subscribe method should be called first to get endpoint address.</remarks>
        protected DateTime PullMessages(string timeout, 
            int messageLimit, 
            XmlElement[] any, 
            out DateTime terminationTime, 
            out NotificationMessageHolderType[] notificationMessage)
        {
            NotificationMessageHolderType[] notificationMessageCopy = null;
            System.DateTime terminationTimeCopy = System.DateTime.MinValue;
            System.DateTime result = System.DateTime.MinValue;

            RunStep( () =>
                         {
                             result = _pullPointSubscriptionClient.PullMessages(timeout, 
                                                                                messageLimit, 
                                                                                any,
                                                                                out terminationTimeCopy,
                                                                                out notificationMessageCopy);    
                         }, 
                         "Pull Messages");

            terminationTime = terminationTimeCopy;
            notificationMessage = notificationMessageCopy;

            return result;
        }

        /// <summary>
        /// Releases subscription manager (via unsubscribe or via timeout). Default timeout of 10 seconds is used.
        /// </summary>
        protected  void ReleaseSubscriptionManager()
        {
            ReleaseSubscriptionManager(10000);
        }

        /// <summary>
        /// Releases subscription manager (via unsubscribe or via timeout)
        /// </summary>
        protected  void ReleaseSubscriptionManager(int timeout)
        {
            System.Diagnostics.Debug.WriteLine("EventTest.Release() ");
            BeginStep("Delete Subscription Manager");
            bool unsubscribeByRequest = false;
            try
            {
                if (_subscriptionManagerClient != null)
                {
                    LogStepEvent("Send unsubscribe request");
                    Unsubscribe request = new Unsubscribe();
                    _subscriptionManagerClient.Unsubscribe(request);
                    unsubscribeByRequest = true;
                }
                else
                {
                    LogStepEvent("Reference to Subscription Manager has not been obtained");
                }
            }
            catch (Exception exc)
            {
                LogStepEvent(string.Format("Failed to unsubscribe through request. Error received: {0}", exc.Message));
            }

            if (!unsubscribeByRequest)
            {
                LogStepEvent("Wait until Subscription Manager is deleted by timeout");
                Sleep(timeout);
            }
            StepPassed();
        }
        
        #region Invalid filters


        protected FilterInfo GetInvalidMessageContentFilter()
        {
            bool empty;
            return GetInvalidMessageContentFilter(out empty);
        }

        /// <summary>
        /// Creates invalid MessageContent filter (unexisting item specified for ItemFilter).
        /// </summary>
        /// <returns></returns>
        protected FilterInfo GetInvalidMessageContentFilter(out bool topicSetEmpty)
        {
            FilterInfo filter;

            TestTool.Proxies.Event.TopicSetType topicSet = GetTopicSet();

            topicSetEmpty = (topicSet == null || topicSet.Any == null || topicSet.Any.Length == 0);

            if (topicSetEmpty)
            {
                Assert(true, "TopicSet is empty", "Check that TopicSet is not empty");
                return null;
            }

            // enumerate topics in topic set
            foreach (XmlElement element in topicSet.Any)
            {
                XmlNamespaceManager manager = new XmlNamespaceManager(element.OwnerDocument.NameTable);
                manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

                filter = GetInvalidMessageContentFilter(element, manager);

                // if filter created - OK
                if (filter != null)
                {
                    return filter;
                }
            }

            // return NULL if cannot create filter
            return null;
        }

        /// <summary>
        /// Gets invalid MessageContentFilter for element passed.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        private FilterInfo GetInvalidMessageContentFilter(XmlElement element, XmlNamespaceManager manager)
        {
            FilterInfo filter = null;

            // if element is a topic
            if (element.RepresentsTopic())
            {
                // Element represents a topic.
                // Look for MessageDescription

                List<string> itemNames = new List<string>();
                XmlElement messageDescription = element.GetMessageDescription();
                if (messageDescription != null)
                {
                    CollectItemNames(messageDescription, manager, OnvifMessage.SIMPLEITEMDESCRIPTION, itemNames);
                    CollectItemNames(messageDescription, manager, OnvifMessage.ELEMENTITEMDESCRIPTION, itemNames);
                }

                string invalidItemName = itemNames.GetNonMatchingString();

                filter = new FilterInfo();

                filter.Filter = new FilterType();

                XmlDocument doc = new XmlDocument();
                XmlElement topicElement = doc.CreateTopicElement();

                TopicInfo topicInfo = TopicInfo.ConstructTopicInfo(element);
                string topicPath = TopicInfo.CreateTopicPath(topicElement, topicInfo);

                topicElement.InnerText = topicPath;

                XmlElement contentFilterElement = doc.CreateContentFilterElement();

                string simpleItemPrefix = contentFilterElement.GetNamespacePrefix(OnvifMessage.ONVIF);
                contentFilterElement.InnerText = string.Format("boolean((//{0}:{1}[@{2}=\"{3}\"] )",
                    simpleItemPrefix, OnvifMessage.SIMPLEITEM, OnvifMessage.NAME, invalidItemName);

                filter.Filter.Any = new XmlElement[] { topicElement, contentFilterElement };
                filter.Topic = topicInfo;
                filter.MessageDescription = messageDescription;

                return filter;
            }

            foreach (XmlNode childNode in element.ChildNodes)
            {
                XmlElement child = childNode as XmlElement;
                if (child != null)
                {
                    filter = GetInvalidMessageContentFilter(child, manager);
                    if (filter != null)
                    {
                        return filter;
                    }
                }
            }

            // if cannot create filter for any element - return null
            return null;
        }

        /// <summary>
        /// Collects item names from MessageDescription
        /// </summary>
        /// <param name="messageDescription">MEssageDescription element.</param>
        /// <param name="manager">Namespace manager.</param>
        /// <param name="elementName">Element name (SimpleItem or ElementItem)</param>
        /// <param name="itemNames">Collection to add names to.</param>
        void CollectItemNames(XmlElement messageDescription,
            XmlNamespaceManager manager,
            string elementName,
            List<string> itemNames)
        {
            XmlNodeList items =
                messageDescription.SelectNodes(
                string.Format("./*/{0}:{1}", OnvifMessage.ONVIFPREFIX, elementName),
                manager);

            // for each item found 
            foreach (XmlNode node in items)
            {
                XmlElement simpleItem = node as XmlElement;
                // if item has a "Name" attribute
                if (simpleItem.HasAttribute(OnvifMessage.NAME))
                {
                    XmlAttribute nameAttriute = simpleItem.Attributes[OnvifMessage.NAME];
                    itemNames.Add(nameAttriute.Value);
                }
            }

        }

        /// <summary>
        /// Gets invalid topic.
        /// </summary>
        /// <returns>String which does not match any of topics supported.</returns>
        protected string GetInvalidTopic()
        {
            TestTool.Proxies.Event.TopicSetType topicSet = GetTopicSet();

            Assert((topicSet != null), "The DUT did not return any TopicSet", "Check if TopicSet returned");

            Assert((topicSet.Any != null), "TopicSet is empty", "Check that TopicSet is not empty");

            // get all topics and create a string which does not match any of them
            List<string> topics = new List<string>();
            foreach (XmlElement element in topicSet.Any)
            {
                FindTopics(element, topics);
            }

            return topics.GetNonMatchingString();
        }

        /// <summary>
        /// Finds all topics beneath the element specified.
        /// </summary>
        /// <param name="element">Topic or TopicNamespace element.</param>
        /// <param name="topics">Collection to add topics to.</param>
        void FindTopics(XmlElement element, List<string> topics)
        {
            if (element.RepresentsTopic())
            {
                topics.Add(element.Name);
            }

            // If not a topic - enumerate child elements.
            foreach (XmlNode node in element.ChildNodes)
            {
                XmlElement child = node as XmlElement;
                if (child == null)
                {
                    continue;
                }
                FindTopics(child, topics);
            }
        }

        #endregion

        
        #region Validate faults


        /// <summary>
        /// Checks that a string passed is valid xs:datetime string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool IsValidXsdDateTime(string value)
        {
            try
            {
                string onvif = "http://www.onvif.org/ver10/schema";
                string message = string.Format("<tt:Message UtcTime=\"{0}\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" ><//tt:Message>", value);
                string schemaString = "<xs:schema xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" elementFormDefault=\"qualified\"><xs:element name=\"Message\"><xs:complexType><xs:attribute name=\"UtcTime\" type=\"xs:dateTime\" use=\"required\"/></xs:complexType></xs:element></xs:schema>";

                XmlSchemaSet schemaSet = new XmlSchemaSet();
                XmlReader rdr = XmlReader.Create(new System.IO.StringReader(schemaString));
                schemaSet.Add(onvif, rdr);

                XmlReader reader = XmlReader.Create(new System.IO.StringReader(message));
                XmlNamespaceManager manager = new XmlNamespaceManager(reader.NameTable);

                XmlSchemaValidator validator = new XmlSchemaValidator(reader.NameTable, schemaSet, manager, XmlSchemaValidationFlags.None);
                validator.Initialize();

                XmlSchemaInfo schemaInfo = new XmlSchemaInfo();

                validator.ValidateElement("Message", onvif, schemaInfo);

                validator.ValidateAttribute("UtcTime", "", value, schemaInfo);

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        /// <summary>
        /// Validates base fault.
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected bool ValidateBaseFault(BaseFaultType detail, out string reason)
        {
            // standard serializer does not parse Timestamp correctly - it goes in Any
            XmlElement timestampElement = FindElement(detail, 
                                                      BaseNotification.TIMESTAMP,
                                                      BaseNotification.WSRFBFNAMESPACE);

            if (timestampElement != null)
            {
                string timestamp = timestampElement.InnerText;
                if (string.IsNullOrEmpty(timestamp))
                {
                    reason = "Timestamp is empty";
                    return false;
                }
                if (!IsValidXsdDateTime(timestamp))
                {
                    reason = string.Format("Timestamp '{0}' is not valid ", timestamp);
                    return false;
                }
            }
            else
            {
                // should this error be fixed in some moment...
                if (detail.Timestamp == DateTime.MinValue)
                {
                    reason = "Timestamp not specified";
                    return false;
                }                
            }
            
            reason = string.Empty;
            return true;
        }

        protected XmlElement FindElement(BaseFaultType detail, string localName, string nameSpace)
        {
            return FindElement(detail.Any, localName, nameSpace);
        }

        protected XmlElement FindElement(XmlElement[] any, string localName, string nameSpace)
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

        protected string GetRecommendedDuration(FaultException<UnacceptableInitialTerminationTimeFaultType> invalidTerminationTimeFault)
        {
            string duration;
            GetRecommendedDuration<UnacceptableInitialTerminationTimeFaultType>(invalidTerminationTimeFault, out duration);
            return duration;
        }

        protected int GetRecommendedDuration<T>(FaultException<T> fault, out string duration)
            where T : BaseFaultType
        {
            XmlElement minimumTimeElement = null;
            XmlElement timestampElement = null;

            if (fault.Detail != null)
            {
                minimumTimeElement =
                    FindElement(fault.Detail, "MinimumTime",
                                "http://docs.oasis-open.org/wsn/b-2");
            
                timestampElement = FindElement(fault.Detail,
                                               BaseNotification.TIMESTAMP,
                                               BaseNotification.WSRFBFNAMESPACE);
                
            }


            string entry = string.Empty;
            duration = string.Empty; 
            int seconds = 0;
            if (minimumTimeElement != null && timestampElement != null)
            {
                string time = minimumTimeElement.InnerText;
                DateTime minimumTime = DateTime.Parse(time);

                string timestamp = timestampElement.InnerText;
                DateTime currentTime = DateTime.Parse(timestamp);

                TimeSpan diff = minimumTime - currentTime;

                seconds = (int)diff.TotalSeconds;
                duration = string.Format("PT{0}S", seconds);

                entry = string.Format("Use duration {0}", duration);
            }
            Assert(minimumTimeElement != null,
                "Fault details or MinimumTime not found",
                "Check if MinimumTime is specified", entry);

            return seconds;
        }

        /// <summary>
        /// Validates fault for invalid message content filter.
        /// </summary>
        /// <param name="fault"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected bool ValidateInvalidMessageContentFilterFault(FaultException fault, out string reason)
        {
            // no fault at all - error
            if (fault == null)
            {
                reason = "No SOAP fault received.";
                return false;
            }

            FaultException<InvalidFilterFaultType> invalidFilterFault = fault as FaultException<InvalidFilterFaultType>;

            if (invalidFilterFault != null)
            {
                if (invalidFilterFault.Detail == null)
                {
                    reason = "Fault received is of type FaultException<InvalidFilterFaultType>, but Details field is null";
                    return false;
                }
                if (!ValidateBaseFault(invalidFilterFault.Detail, out reason))
                {
                    return false;
                }
                // Timestamp format is validated when a fault is being deserialized. So here it should be valid.
            }

            FaultException<InvalidMessageContentExpressionFaultType> invalidMessageContentExpressionFaultType =
                fault as FaultException<InvalidMessageContentExpressionFaultType>;

            if (invalidMessageContentExpressionFaultType != null)
            {
                if (invalidMessageContentExpressionFaultType.Detail == null)
                {
                    reason = "Fault received is of type FaultException<InvalidFilterFaultType>, but Details field is null";
                    return false;
                }
                if (!ValidateBaseFault(invalidMessageContentExpressionFaultType.Detail, out reason))
                {
                    return false;
                }
                // Timestamp format is validated when a fault is being deserialized. So here it should be valid.
            }

            if (invalidFilterFault == null && invalidMessageContentExpressionFaultType == null)
            {
                LogStepEvent("Warning: Fault received is neither  FaultException<InvalidFilterFaultType> nor FaultException<InvalidMessageContentExpressionFaultType>");
            }

            reason = string.Empty;
            return true;
        }
        
        /// <summary>
        /// Validates fault for invalid topic filter.
        /// </summary>
        /// <param name="fault"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected bool ValidateInvalidTopicFilterFault(FaultException fault, out string reason)
        {
            if (fault == null)
            {
                reason = "No SOAP fault received.";
                return false;
            }

            FaultException<InvalidFilterFaultType> invalidFilterFault =
                fault as FaultException<InvalidFilterFaultType>;

            if (invalidFilterFault != null)
            {
                if (invalidFilterFault.Detail == null)
                {
                    reason = "Fault received is of type FaultException<InvalidFilterFaultType>, but Details field is null";
                    return false;
                }
                if (!ValidateBaseFault(invalidFilterFault.Detail, out reason))
                {
                    return false;
                }
                // Timestamp format is validated when a fault is being deserialized. So here it should be valid.
            }

            FaultException<TopicNotSupportedFaultType> topicNotSupportedFault =
                fault as FaultException<TopicNotSupportedFaultType>;

            if (topicNotSupportedFault != null)
            {
                if (topicNotSupportedFault.Detail == null)
                {
                    reason = "Fault received is of type FaultException<TopicNotSupportedFaultType>, but Details field is null";
                    return false;
                }
                if (!ValidateBaseFault(topicNotSupportedFault.Detail, out reason))
                {
                    return false;
                }
                // Timestamp format is validated when a fault is being deserialized. So here it should be valid.
            }

            if (invalidFilterFault == null && topicNotSupportedFault == null)
            {
                LogStepEvent(
                    "Warning: Fault received is neither  FaultException<InvalidFilterFaultType> nor FaultException<TopicNotSupportedFaultType>");
            }

            reason = string.Empty;
            return true;
        }
        
        /// <summary>
        /// Validate ResourseUnknown fault.
        /// </summary>
        /// <param name="fault"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected bool ValidateResourseUnknownFault(FaultException fault, out string reason)
        {
            if (fault == null)
            {
                reason = "No SOAP fault received.";
                return false;
            }

            FaultException<ResourceUnknownFaultType> resourceUnknownFault =
                fault as FaultException<ResourceUnknownFaultType>;

            if (resourceUnknownFault == null)
            {
                LogStepEvent("Warning: Fault received is not of type FaultException<ResourceUnknownFaultType>");
            }
            else
            {
                if (resourceUnknownFault.Detail == null)
                {
                    reason = "Fault received is of type FaultException<ResourceUnknownFaultType>, but Details field is null";
                    return false;
                }
                if (!ValidateBaseFault(resourceUnknownFault.Detail, out reason))
                {
                    return false;
                }
            }
            reason = string.Empty;
            return true;
        }


        /// <summary>
        /// </summary>
        /// <param name="fault"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected bool ValidateUnsubscribeFault(FaultException fault, out string reason)
        {
            if (fault == null)
            {
                reason = "No SOAP fault received.";
                return false;
            }

            FaultException<ResourceUnknownFaultType> resourceUnknownFault =
                fault as FaultException<ResourceUnknownFaultType>;

            if (resourceUnknownFault != null)
            {
                if (resourceUnknownFault.Detail == null)
                {
                    reason = "Fault received is of type FaultException<ResourceUnknownFaultType>, but Details field is null";
                    return false;
                }
                if (!ValidateBaseFault(resourceUnknownFault.Detail, out reason))
                {
                    return false;
                }
            }

            FaultException<UnableToDestroySubscriptionFaultType> unableToDestroySubscriptionFault =
                fault as FaultException<UnableToDestroySubscriptionFaultType>;
            if (unableToDestroySubscriptionFault != null)
            {
                if (unableToDestroySubscriptionFault.Detail == null)
                {
                    reason = "Fault received is of type FaultException<UnableToDestroySubscriptionFaultType>, but Details field is null";
                    return false;
                }
                if (!ValidateBaseFault(unableToDestroySubscriptionFault.Detail, out reason))
                {
                    return false;
                }
            }

            if (resourceUnknownFault == null && unableToDestroySubscriptionFault == null )
            {
                LogStepEvent("Warning: Fault received is neither of type FaultException<ResourceUnknownFaultType> nor FaultException<UnableToDestroySubscriptionFaultType>");

            }

            reason = string.Empty;
            return true;
        }


        #endregion
        
        protected void ValidateSubscribeResponse(SubscribeResponse subscribeResponse, int terminationTime)
        {
            Assert(subscribeResponse != null, "The DUT did not return Subscribe response",
              "Check that the DUT returned Subscribe response");

            Assert(subscribeResponse.CurrentTimeSpecified, "Current time is not specified",
                  "Check that CurrentTime is specified");
            Assert(subscribeResponse.TerminationTimeSpecified, "Termination time is not specified",
                   "Check that TerminationTime is specified");

            Assert(subscribeResponse.CurrentTime.AddSeconds(terminationTime) <= subscribeResponse.TerminationTime,
                "TerminationTime < CurrentTime + InitialTerminationTime",
                "Validate CurrentTime and TerminationTime");

            Assert(subscribeResponse.SubscriptionReference != null, "The DUT did not return SubscriptionReference",
               "Check if the DUT returned SubscriptionReference");

            Assert(subscribeResponse.SubscriptionReference.Address != null
                && subscribeResponse.SubscriptionReference.Address.Value != null,
                "SubscriptionReference does not contain address",
                   "Check if SubscriptionReference contains address");

            Assert(subscribeResponse.SubscriptionReference.Address.Value.IsValidUrl(), "URL passed in SubscriptionReference is not valid",
                   "Check that URL specified is valid");


        }
    }
}
