using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Proxies.Event;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.Tests.Engine
{
    partial class FeaturesDefinitionProcess
    {
        protected const string ONVIFTOPICSET = "http://www.onvif.org/ver10/topics";

        /// <summary>
        /// Gets TopicSet
        /// </summary>
        /// <returns></returns>
        protected TopicSetType GetTopicSet()
        {
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

        protected XmlNamespaceManager CreateNamespaceManager(XmlDocument soapRawPacket)
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(soapRawPacket.NameTable);
            manager.AddNamespace("s", "http://www.w3.org/2003/05/soap-envelope");
            manager.AddNamespace("events", "http://www.onvif.org/ver10/events/wsdl");
            manager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");

            return manager;
        }

        /// <summary>
        /// Finds all topics beneath the element specified.
        /// </summary>
        /// <param name="element">Topic or TopicNamespace element.</param>
        /// <param name="topics">Collection to add topics to.</param>
        protected void FindTopics(XmlElement element, List<XmlElement> topics)
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
                }
                FindTopics(child, topics);
            }
        }

        List<TopicInfo> GetTopicInfos(string response)
        {

            XmlDocument soapRawResponse = BaseNotificationXmlUtils.GetRawResponse(response);

            // find Topic elements in "raw" packet
            string topicPath;
            topicPath = "/s:Envelope/s:Body/events:GetEventPropertiesResponse/t1:TopicSet";
            XmlNamespaceManager manager = CreateNamespaceManager(soapRawResponse);
            manager.AddNamespace("t1", "http://docs.oasis-open.org/wsn/t-1");

            XmlNode topicSetNode = soapRawResponse.SelectSingleNode(topicPath, manager);
            XmlElement topicSetElement = topicSetNode as XmlElement;
            List<XmlElement> rootTopics = new List<XmlElement>();

            foreach (XmlNode node in topicSetElement.ChildNodes)
            {
                XmlElement e = node as XmlElement;
                if (e != null)
                {
                    rootTopics.Add(e);
                }
            }

            // Check that the topic of interest is supported

            // select all topics 
            List<XmlElement> topics = new List<XmlElement>();
            foreach (XmlElement element in rootTopics)
            {
                FindTopics(element, topics);
            }
            
            List<TopicInfo> topicInfos = new List<TopicInfo>();

            foreach (XmlElement topicElement in topics)
            {
                TopicInfo info = TopicInfo.ConstructTopicInfo(topicElement);
                topicInfos.Add(info);
            }

            return topicInfos;
        }

        Dictionary<TopicInfo, Feature> GetPACSTopics()
        {
            Dictionary<TopicInfo, Feature> topics = new Dictionary<TopicInfo, Feature>();

            // DoorControl service
            topics.Add(ConstructPacsTopic(new string[] { "Door", "State", "DoorMode" }), Feature.DoorModeEvent);
            topics.Add(ConstructPacsTopic(new string[] { "Door", "State", "DoorPhysicalState" }), Feature.DoorPhysicalStateEvent);
            topics.Add(ConstructPacsTopic(new string[] { "Door", "State", "LockPhysicalState" }), Feature.DoubleLockPhysicalStateEvent);
            topics.Add(ConstructPacsTopic(new string[] { "Door", "State", "DoubleLockPhysicalState" }), Feature.LockPhysicalStateEvent);
            topics.Add(ConstructPacsTopic(new string[] { "Door", "State", "DoorAlarm" }), Feature.DoorTamperEvent);
            topics.Add(ConstructPacsTopic(new string[] { "Door", "State", "DoorTamper" }), Feature.DoorAlarmEvent);
            topics.Add(ConstructPacsTopic(new string[] { "Door", "State", "DoorFault" }), Feature.DoorSetEvent);
            topics.Add(ConstructPacsTopic(new string[] { "Door", "Changed" }), Feature.DoorRemovedEvent);
            topics.Add(ConstructPacsTopic(new string[] { "Door", "Removed" }), Feature.DoorFaultEvent); 

            // Access Control service 
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "AccessGranted", "Anonymous" }), Feature.AccessGrantedAnonymousEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "AccessGranted", "Credential" }), Feature.AccessGrantedCredentialEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "AccessGranted", "Anonymous", "External" }), Feature.AccessGrantedAnonymousExternalEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "AccessGranted", "Credential", "External" }), Feature.AccessGrantedCredentialExternalEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "AccessTaken", "Anonymous" }), Feature.AccessTakenAnonymousEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "AccessTaken", "Credential" }), Feature.AccessTakenCredentialEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "AccessNotTaken", "Anonymous" }), Feature.AccessNotTakenAnonymousEvent); ;
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "AccessNotTaken", "Credential" }), Feature.AccessNotTakenCredentialEvent); ;
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Credential", "CredentialNotEnabled" }), Feature.AccessDeniedCredentialCredentialNotEnabledEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Credential", "CredentialNotActive" }), Feature.AccessDeniedCredentialCredentialNotActiveEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Credential", "CredentialExpired" }), Feature.AccessDeniedCredentialCredentialExpiredEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Credential", "InvalidPIN" }), Feature.AccessDeniedCredentialInvalidPINEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Credential", "NotPermittedAtThisTime" }), Feature.AccessDeniedCredentialNotPermittedAtThisTimeEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Credential", "Unauthorized" }), Feature.AccessDeniedCredentialUnauthorizedEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Credential", "External" }), Feature.AccessDeniedCredentialExternalEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Credential", "Other" }), Feature.AccessDeniedCredentialOtherEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Anonymous", "NotPermittedAtThisTime" }), Feature.AccessDeniedAnonymousNotPermittedAtThisTimeEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Anonymous", "Unauthorized" }), Feature.AccessDeniedAnonymousUnauthorizedEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Anonymous", "External" }), Feature.AccessDeniedAnonymousExternalEvent); 
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Anonymous", "Other" }), Feature.AccessDeniedAnonymousOtherEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Denied", "Credential", "CredentialNotFound", "Card" }), Feature.AccessDeniedCredentialCredentialNotFoundCardEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Duress", "Anonymous" }), Feature.DuressAnonymousEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Duress", "Credential" }), Feature.DuressCredentialEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Request", "Anonymous" }), Feature.RequestAnonymousEvent); 
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Request", "Credential" }), Feature.RequestCredentialEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Request", "Timeout", "Anonymous" }), Feature.RequestTimeoutAnonymousEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessControl", "Request", "Timeout", "Credential" }), Feature.RequestTimeoutCredentialEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessPoint", "Changed" }), Feature.AccessPointSetEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessPoint", "Removed" }), Feature.AccessPointRemovedEvent);
            topics.Add(ConstructPacsTopic(new string[] { "Area", "Changed" }), Feature.AccessPointEnabledEvent);
            topics.Add(ConstructPacsTopic(new string[] { "Area", "Removed" }), Feature.AccessPointTamperingEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessPoint", "State", "Enabled" }), Feature.AreaSetEvent);
            topics.Add(ConstructPacsTopic(new string[] { "AccessPoint", "State", "Tampering" }), Feature.AreaRemovedEvent);

            return topics;
        }

        protected TopicInfo ConstructPacsTopic(IEnumerable<string> sequence)
        {
            TopicInfo current = null;

            foreach (string topic in sequence)
            {
                TopicInfo last = current;
                current = new TopicInfo();
                current.ParentTopic = last;
                current.Name = topic;
                if (last == null)
                {
                    current.Namespace = ONVIFTOPICSET;
                    current.NamespacePrefix = "tns1";
                }
            }
            return current;
        }
    }
}
