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
            return GetDoorControlEventTopics().Union(GetAccessControlEventTopics()).ToDictionary(e => e.Key, e => e.Value);
        }

        private Dictionary<TopicInfo, Feature> GetAccessControlEventTopics()
        {
            return new Dictionary<TopicInfo, Feature>
                {
                    {
                        ConstructPacsTopic(new string[] {"AccessControl", "AccessGranted", "Anonymous"}),
                        Feature.AccessGrantedAnonymousEvent
                    },
                    {
                        ConstructPacsTopic(new string[] {"AccessControl", "AccessGranted", "Credential"}),
                        Feature.AccessGrantedCredentialEvent
                    },
                    {
                        ConstructPacsTopic(new string[] {"AccessControl", "AccessTaken", "Anonymous"}),
                        Feature.AccessTakenAnonymousEvent
                    },
                    {
                        ConstructPacsTopic(new string[] {"AccessControl", "AccessTaken", "Credential"}),
                        Feature.AccessTakenCredentialEvent
                    },
                    {
                        ConstructPacsTopic(new string[] {"AccessControl", "AccessNotTaken", "Anonymous"}),
                        Feature.AccessNotTakenAnonymousEvent
                    },
                    {
                        ConstructPacsTopic(new string[] {"AccessControl", "AccessNotTaken", "Credential"}),
                        Feature.AccessNotTakenCredentialEvent
                    },
                    {
                        ConstructPacsTopic(new string[] {"AccessControl", "Denied", "Credential"}),
                        Feature.AccessDeniedCredentialEvent
                    },
                    {
                        ConstructPacsTopic(new string[] {"AccessControl", "Denied", "Anonymous"}),
                        Feature.AccessDeniedAnonymousEvent
                    },
                    {
                        ConstructPacsTopic(new string[]{"AccessControl", "Denied", "CredentialNotFound", "Card"}),
                        Feature.AccessDeniedCredentialCredentialNotFoundCardEvent
                    },
                    {ConstructPacsTopic(new string[] {"AccessControl", "Duress"}), Feature.DuressEvent},
                    {
                        ConstructPacsTopic(new string[] {"AccessControl", "Request", "Anonymous"}),
                        Feature.RequestAnonymousEvent
                    },
                    {
                        ConstructPacsTopic(new string[] {"AccessControl", "Request", "Credential"}),
                        Feature.RequestCredentialEvent
                    },
                    {
                        ConstructPacsTopic(new string[] {"AccessControl", "Request", "Timeout"}),
                        Feature.RequestTimeoutEvent
                    },
                    {ConstructPacsTopic(new string[] {"Configuration", "AccessPoint", "Changed"}), Feature.AccessPointChangedEvent},
                    {ConstructPacsTopic(new string[] {"Configuration", "AccessPoint", "Removed"}), Feature.AccessPointRemovedEvent},
                    {ConstructPacsTopic(new string[] {"Configuration", "Area", "Changed"}), Feature.AreaChangedEvent},
                    {ConstructPacsTopic(new string[] {"Configuration", "Area", "Removed"}), Feature.AreaRemovedEvent},
                    {ConstructPacsTopic(new string[] {"AccessPoint", "State", "Enabled"}), Feature.AccessPointStateEnabledEvent},
                };
        }

        private Dictionary<TopicInfo, Feature> GetRecordingControlEventTopics()
        {
            return new Dictionary<TopicInfo, Feature>
                {
                    {
                        ConstructPacsTopic(new string[] {"RecordingConfig", "RecordingConfiguration"}),
                        Feature.RecordingConfigRecordingConfigurationEvent
                    },
                    {
                        ConstructPacsTopic(new string[] {"RecordingConfig", "RecordingJobConfiguration"}),
                        Feature.RecordingConfigRecordingJobConfigurationEvent
                    },
                    {
                        ConstructPacsTopic(new string[] {"RecordingConfig", "TrackConfiguration"}),
                        Feature.RecordingConfigTrackConfigurationEvent
                    },
                    {
                        ConstructPacsTopic(new string[] {"RecordingConfig", "DeleteTrackData"}),
                        Feature.RecordingConfigDeleteTrackDataEvent
                    }
                };
        }


        private Dictionary<TopicInfo, Feature> GetDoorControlEventTopics()
        {
            return new Dictionary<TopicInfo, Feature>
                {
                    {ConstructPacsTopic(new string[] {"Door", "State", "DoorMode"}), Feature.DoorModeEvent},
                    {ConstructPacsTopic(new string[] {"Door", "State", "DoorPhysicalState"}), Feature.DoorPhysicalStateEvent},
                    {ConstructPacsTopic(new string[] {"Door", "State", "LockPhysicalState"}),Feature.LockPhysicalStateEvent},
                    {ConstructPacsTopic(new string[] {"Door", "State", "DoubleLockPhysicalState"}),Feature.DoubleLockPhysicalStateEvent},
                    {ConstructPacsTopic(new string[] {"Door", "State", "DoorAlarm"}), Feature.DoorAlarmEvent},
                    {ConstructPacsTopic(new string[] {"Door", "State", "DoorTamper"}), Feature.DoorTamperEvent},
                    {ConstructPacsTopic(new string[] {"Door", "State", "DoorFault"}), Feature.DoorFaultEvent},
                    {ConstructPacsTopic(new string[] {"Configuration", "Door", "Changed"}), Feature.DoorChangedEvent},
                    {ConstructPacsTopic(new string[] {"Configuration", "Door", "Removed"}), Feature.DoorRemovedEvent}
                };
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
