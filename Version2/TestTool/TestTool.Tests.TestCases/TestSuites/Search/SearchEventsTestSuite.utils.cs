using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Event;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.Utils.Events;
using DateTime=System.DateTime;

namespace TestTool.Tests.TestCases
{
    partial class SearchEventsTestSuite
    {
        #region XML Utils

        readonly string TNS1NAMESPACE = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE);
        
      
        string GetSimpleItem(XmlElement message, string value)
        {
            Dictionary<string, string> items = new Dictionary<string, string>();

            XmlNamespaceManager manager = new XmlNamespaceManager(message.OwnerDocument.NameTable);
            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            string path;
            path = "/tt:Data/tt:SimpleItem";
            XmlNodeList itemNodesList = message.SelectNodes(path, manager);

            foreach (XmlNode node in itemNodesList)
            {
                XmlElement element = node as XmlElement;
                if (element == null)
                {
                    continue;
                }
                if (!element.HasAttribute(OnvifMessage.NAME))
                {
                    continue;
                }
                string name = element.Attributes[OnvifMessage.NAME].Value;
                if (name == value)
                {
                    return element.Attributes[OnvifMessage.VALUE].Value;
                }
            }

            return null;
        }

        Dictionary<string, string> GetMessageSimpleItems(XmlElement message)
        {
            Dictionary<string, string> items = new Dictionary<string, string>();

            XmlNamespaceManager manager = new XmlNamespaceManager(message.OwnerDocument.NameTable);
            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            string path;
            path = "/tt:Data/tt:SimpleItem";
            XmlNodeList itemNodesList = message.SelectNodes(path, manager);

            foreach (XmlNode node in itemNodesList)
            {
                XmlElement element = node as XmlElement;
                if (element == null)
                {
                    continue;
                }
                // handle wrong elements!
                items.Add(element.GetAttribute(OnvifMessage.NAME), element.GetAttribute(OnvifMessage.VALUE/*, OnvifMessage.ONVIF*/));
            }

            return items;
        }

        #endregion

        #region EventPortType utils

        private string _eventServiceAddress;
        void InitServiceClient(ServiceHolder serviceHolder, IEnumerable<IChannelController> controllers)
        {
            var found = false;
            if (!serviceHolder.HasAddress)
            {
                RunStep(() =>
                        {
                            serviceHolder.Retrieve(Features);
                            if (!serviceHolder.HasAddress)
                            {
                                throw new AssertException(string.Format("{0} service not found", serviceHolder.ServiceName));
                            }
                            else
                            {
                                found = true;
                                LogStepEvent(serviceHolder.Address);
                            }
                        }, 
                        string.Format("Get {0} service address", serviceHolder.ServiceName),
                        OnvifFaults.NoSuchService, true, true);
                DoRequestDelay();
            }

            Assert(found,
                   string.Format("{0} service address not found", serviceHolder.ServiceName),
                   string.Format("Check that the DUT returned {0} service address", serviceHolder.ServiceName));

            var controller = new EndpointController(new EndpointAddress(serviceHolder.Address));

            var ctrls = new List<IChannelController>();
            ctrls.Add(controller);
            ctrls.AddRange(controllers);

            var binding = CreateBinding(false, ctrls);

            serviceHolder.CreateClient(binding, AttachSecurity, SetupChannel);
        }

        protected TopicSetType GetTopicSet()
        {
            TopicSetType topicSet = null;

            if (null != eventService)
                RunStep(() =>
                        {
                            bool fixedTopicSet = false;
                            string[] topicExpressionDialect = null;
                            string[] messageContentFilterDialect = null;
                            string[] producerPropertiesFilterDialect = null;
                            string[] messageContentSchemaLocation = null;
                            XmlElement[] any = null;

                            eventService.GetEventProperties(out fixedTopicSet,
                                                            out topicSet,
                                                            out topicExpressionDialect,
                                                            out messageContentFilterDialect,
                                                            out producerPropertiesFilterDialect,
                                                            out messageContentSchemaLocation,
                                                            out any);
                        },
                        "Get Event Properties");
    

            return topicSet;
        }

        protected void FindTopics(XmlElement element, List<XmlElement> topics)
        {
            if (element.RepresentsTopic())
            { topics.Add(element); }

            // If not a topic - enumerate child elements.
            foreach (var child in element.ChildNodes.OfType<XmlElement>())
            { FindTopics(child, topics); }
        }

        public List<EventsTopicInfo> GetTopics()
        {
            var topicSet = GetTopicSet();

            if (topicSet == null || topicSet.Any == null || topicSet.Any.Length == 0)
            { return null; }

            //Apply FindTopics to each element and aggregate results in one set
            var topics = topicSet.Any.Aggregate(new List<XmlElement>(), (s, e) => { FindTopics(e, s); return s; });

            //From each XmlElement make TopicInfo object and from this object take EventsTopicInfo object
            return topics.Select(nextTopicElement => TopicInfo.ConstructTopicInfo(nextTopicElement)).Select(info => info.GetPlainInfo()).ToList();
        }
        #endregion

        #region Topic utils

        XmlText GetTopicElement(FindEventResult e)
        {
            XmlText text = null;
            foreach (XmlNode child in e.Event.Topic.Any)
            {
                if (child is XmlText)
                {
                    text = child as XmlText;
                    break;
                }
            }
            return text;
        }
        
        TopicInfo CreateTopicInfo(FindEventResult e, XmlElement messageElement)
        {
            XmlElement topicElement = null;
            foreach (XmlElement child in messageElement.ChildNodes)
            {
                if (child.LocalName == "Topic" && child.NamespaceURI == BaseNotification.WSNT)
                {
                    topicElement = child;
                    break;
                }
            }

            XmlText text = GetTopicElement(e);

            if (topicElement == null)
            {
                return null;
            }
            if (text == null)
            {
                return null;
            }

            //TopicInfo actual = TopicInfo.ExtractTopicInfoAll(text.Value, topicElement);
            //[26.03.2013] AKS: return behavior like before merge
            TopicInfo actual = TopicInfo.ExtractTopicInfoPACS(text.Value, topicElement);
            return actual;
        }

        bool CheckEventTopic(FindEventResult e, XmlElement rawResultElement,
            string topic, 
            string ns)
        {
             if (e.Event != null && e.Event.Topic != null )
             {
                 TopicInfo expected =
                     TopicInfo.ConstructTopicInfo(new EventsTopicInfo() {NamespacesDefinition = ns, Topic = topic});

                 TopicInfo actual = CreateTopicInfo(e, rawResultElement); 

                 bool topicMatches = TopicInfo.TopicsMatch(actual, expected);

                 return topicMatches;
             }
            return false;
        }






        #endregion

        void ValidateRecordingEvents(RecordingInformation recordingInfo,
                                     Func<string, bool> trackTokenValidator,
                                     IEnumerable<FindEventResult> results,
                                     Dictionary<FindEventResult, XmlElement> elements,
                                     bool checkEdges, bool outsideRange)
        {
            // check sequence
            ValidateRecordingEventsSequence(recordingInfo, results, elements, checkEdges, outsideRange);
            
            // no other recordings
            ValidateEventsContext(recordingInfo, trackTokenValidator, "tns1:RecordingHistory/Track/State", results, elements);

            foreach (TrackInformation track in recordingInfo.Track)
            {
                if (track.DataTo > track.DataFrom)
                    ValidateTrackEvents(recordingInfo, track, results, elements, checkEdges, outsideRange);
            }

            ValidateEventsContext(recordingInfo, trackTokenValidator, "tns1:RecordingHistory/Track/VideoParameters", results, elements);

            ValidateEventsContext(recordingInfo, trackTokenValidator, "tns1:RecordingHistory/Track/AudioParameters", results, elements);
        }

        void ValidateRecordingEventsSequence(RecordingInformation recording,
                                             IEnumerable<FindEventResult> results,
                                             Dictionary<FindEventResult, XmlElement> elements,
                                             bool checkEdges, bool outsideRange)
        {
            //
            // recording events 
            //
            Func<FindEventResult, bool> eventCheck = E => CheckEventTopic(E, elements[E], "tns1:RecordingHistory/Recording/State", TNS1NAMESPACE);

            // select Recording/State events
            var filtered = results.Where(E => eventCheck(E)).OrderBy(E => E.Time);

            Assert(filtered.All(E => E.RecordingToken == recording.RecordingToken),
                   "Event with topic 'tns1:RecordingHistory/Recording/State' for other recordings found",
                   string.Format("Check that all events with topic 'tns1:RecordingHistory/Recording/State' relate to recording '{0}'",
                                 recording.RecordingToken));

            // when checkEdges = false (narrowed interval), 1 event is OK (only virtual event)
            Assert(filtered.Count() >= (checkEdges ? 2 : 1), 
                   "Not enough tns1:RecordingHistory/Recording/State events found",
                   string.Format("Check that at least {0} with topic tns1:RecordingHistory/Recording/State found", checkEdges ? "two events" : "one event"));

            bool isRecordingOk = true;
            bool? lastIsRecording = null;
            if (checkEdges)
            {
                //If checkEdges == true => It is expected that searchInterval includes the entire recording
                //In this case IsRecording == true for event with min Time(first) and false for event with max Time(last)
                //By spec, check only event with min Time

                if (outsideRange)
                {
                    //FindEvents was called with range wider than recordingInfo's range. 
                    //Events with PropertyOperation = 'Initialized' and IsRecording = 'false' are possible.
                    var firstEvent = filtered.First();

                    bool isRecording = false;
                    try
                    {
                        var isRecordingValue = GetSimpleItem(firstEvent.Event.Message, "IsRecording");
                        isRecording = XmlConvert.ToBoolean(isRecordingValue);
                    }
                    catch (Exception e)
                    {
                        Assert(false,
                               string.Format("Failed to retrieve value of IsRecording flag: {0}", e.Message),
                               "Retrieve value of IsRecording flag in event with earliest time");
                    }

                    string operationType = "";
                    try
                    { operationType = firstEvent.Event.Message.GetAttribute(OnvifMessage.PROPERTYOPERATIONTYPE); }
                    catch (Exception e)
                    {
                        if (!isRecording)
                            Assert(false,
                                   string.Format("Failed to retrieve value of PropertyOperation flag: {0}", e.Message),
                                   "Retrieve value of PropertyOperation flag in event with earliest time");
                    }

                    Assert(isRecording || (!isRecording && OnvifMessage.INITIALIZED == operationType),
                           "Expected that earliest event has IsRecording = 'true' or OperationType = 'Initialized' and IsRecording = 'false'"
                           + Environment.NewLine
                           + string.Format("Actually: OperationType = '{0}' and IsRecording = '{1}'", operationType, isRecording),
                           "Check value of IsRecording flag in event with earliest time");

                    lastIsRecording = !isRecording; // forward ? false : true
                }
                else
                    lastIsRecording = false; // forward ? false : true
          }


            StringBuilder dump = new StringBuilder();
            bool first = true;
            foreach (FindEventResult ev in filtered)
            {
                if (!isRecordingOk)
                    break;

                bool f = first;
                first = false;

                XmlElement message = ev.Event.Message;

                string value = GetSimpleItem(message, "IsRecording");

                if (value == null)
                {
                    isRecordingOk = false;
                    dump.AppendLine(string.Format("IsRecording value not found for event with RecordingToken='{0}', Time='{1}'",
                                                  ev.RecordingToken, ev.Time.StdDateTimeToString()));
                }
                else
                {
                    bool isRecording = false;
                    bool attrOk = true;
                    try
                    {
                        isRecording = XmlConvert.ToBoolean(value);
                    }
                    catch (Exception)
                    {
                        attrOk = false;
                    }

                    if (!attrOk)
                    {
                        isRecordingOk = false;
                        dump.AppendLine(string.Format("IsRecording value for event with RecordingToken='{0}', Time='{1}' is incorrect ({2})",
                                                      ev.RecordingToken, ev.Time.StdDateTimeToString(), value));
                    }
                    else
                    {
                        if (ev.StartStateEvent)
                        {
                            continue;
                        }

                        if (lastIsRecording.HasValue)
                        {
                            if (isRecording == lastIsRecording.Value)
                            {
                                isRecordingOk = false;
                                if (f)
                                {
                                    dump.AppendLine(string.Format("IsRecording value for first event with RecordingToken='{0}', Time='{1}' is '{2}'{3}",
                                                                  ev.RecordingToken, ev.Time.StdDateTimeToString(), isRecording.ToString().ToLower(), Environment.NewLine));
                                }
                                else
                                {
                                    dump.AppendLine(string.Format("IsRecording value for event with RecordingToken='{0}', Time='{1}' is {2} while previous is also {2}{3}",
                                                                  ev.RecordingToken, ev.Time.StdDateTimeToString(), isRecording, Environment.NewLine));
                                }
                            }
                        }
                        lastIsRecording = isRecording;
                    }
                }
            }

            Assert(isRecordingOk, dump.ToStringTrimNewLine(), "Check that IsRecording values in events are correct");

        }

        void ValidateTrackEvents(RecordingInformation recording,
                                 TrackInformation track,
                                 IEnumerable<FindEventResult> results,
                                 Dictionary<FindEventResult, XmlElement> elements, 
                                 bool checkEdges, bool outsideRange)
        {
            Func<FindEventResult, bool> eventCheck = E => CheckEventTopic(E, elements[E], "tns1:RecordingHistory/Track/State", TNS1NAMESPACE);

            var propertuOperationPriority = new Dictionary<string, int>()
                {
                    {OnvifMessage.INITIALIZED, 0},
                    {OnvifMessage.CHANGED,     1},
                    {OnvifMessage.DELETED,     2}
                };
            //If two events has the same time then event with "Initialized" flag should be first.
            var filtered = results.Where(E => eventCheck(E) && E.TrackToken == track.TrackToken).OrderBy(E => propertuOperationPriority[E.Event.Message.GetAttribute(OnvifMessage.PROPERTYOPERATIONTYPE)])
                                                                                                .OrderBy(E => E.Time);

            // checkEdges = false means that interval is narrowed and virtual events are sent
            // in this case 1 event is OK
            Assert(filtered.Count() >= (checkEdges ? 2 : 1),
                   string.Format("Not enough events with topic=tns1:RecordingHistory/Track/State, RecordingToken='{0}' for track '{1}' found in the response",
                                 recording.RecordingToken, track.TrackToken),
                   string.Format("Check that at least {0} with topic tns1:RecordingHistory/Track/State are present for track '{1}'", 
                                 checkEdges ? "two events" : "one event", track.TrackToken));

            bool dataPresentOk = true;
            bool? lastDataPresent = null;
            if (checkEdges)
            {
                //If checkEdges == true => It is expected that searchInterval includes the entire recording
                //In this case IsRecording == true for event with min Time(first) and false for event with max Time(last)
                //By spec, check only event with min Time

                if (outsideRange)
                {
                    //FindEvents was called with range wider than recording's range. 
                    //Events with PropertyOperation = 'Initialized' and IsDataPresent = 'false' are possible.

                    var firstEvent = filtered.First();

                    bool isDataPresent = false;
                    try
                    {
                        var isDataPresentValue = GetSimpleItem(firstEvent.Event.Message, "IsDataPresent");
                        isDataPresent = XmlConvert.ToBoolean(isDataPresentValue);
                    }
                    catch (Exception e)
                    {
                        Assert(false,
                               string.Format("Failed to retrieve value of isDataPresent flag: {0}", e.Message),
                               "Retrieve value of isDataPresent flag in event with earliest time");
                    }

                    string operationType = "";
                    try
                    { operationType = firstEvent.Event.Message.GetAttribute(OnvifMessage.PROPERTYOPERATIONTYPE); }
                    catch (Exception e)
                    {
                        if (!isDataPresent)
                            Assert(false,
                                   string.Format("Failed to retrieve value of PropertyOperation flag: {0}", e.Message),
                                   "Retrieve value of PropertyOperation flag in event with earliest time");
                    }

                    Assert(isDataPresent || (!isDataPresent && OnvifMessage.INITIALIZED == operationType),
                           "Expected that earliest event has IsDataPresent = 'true' or OperationType = 'Initialized' and IsDataPresent = 'false'"
                           + Environment.NewLine
                           + string.Format("Actually: OperationType = '{0}' and IsDataPresent = '{1}'", operationType, isDataPresent),
                           "Check value of IsDataPresent flag in event with earliest time");

                    lastDataPresent = !isDataPresent;
                }
                else
                    lastDataPresent = false;
            }

            StringBuilder dump = new StringBuilder();
            bool first = true;
            foreach (FindEventResult ev in filtered)
            {
                bool f = first;
                first = false;

                XmlElement message = ev.Event.Message;

                string value = GetSimpleItem(message, "IsDataPresent");

                if (value == null)
                {
                    dataPresentOk = false;
                    dump.AppendFormat("IsDataPresent value not found for event with RecordingToken='{0}', Time='{1}'{2}",
                                      ev.RecordingToken, ev.Time.StdDateTimeToString(), Environment.NewLine);
                }
                else
                {
                    bool dataPresent = false;
                    bool attrOk = true;

                    try
                    { dataPresent = XmlConvert.ToBoolean(value); }
                    catch (Exception)
                    { attrOk = false; }

                    if (!attrOk)
                    {
                        dataPresentOk = false;
                        dump.AppendFormat("IsDataPresent value for event with RecordingToken='{0}', Time='{1}' is incorrect ({2}){3}",
                                          ev.RecordingToken, ev.Time.StdDateTimeToString(), value, Environment.NewLine);
                    }
                    else
                    {
                        if (ev.StartStateEvent)
                        {
                            continue;
                        }

                        if (lastDataPresent.HasValue)
                        {
                            if (dataPresent == lastDataPresent)
                            {
                                dataPresentOk = false;
                                if (f)
                                {
                                    dump.AppendFormat("IsDataPresent value for first event with RecordingToken='{0}', Time='{1}' is false {2}",
                                                      ev.RecordingToken, ev.Time.StdDateTimeToString(), Environment.NewLine);
                                }
                                else
                                {
                                    dump.AppendFormat("IsDataPresent value for event with RecordingToken='{0}', Time='{1}' is {2} while previous is also {2}{3}",
                                                      ev.RecordingToken, ev.Time.StdDateTimeToString(), dataPresent, Environment.NewLine);
                                }
                            }
                        }
                        lastDataPresent = dataPresent;

                    }
                }
            }

            Assert(dataPresentOk, dump.ToStringTrimNewLine(), "Check that IsDataPresent values in events are correct");
        }

        void ValidateEventsContext(RecordingInformation recordingInfo,
                                   Func<string, bool> trackTokenValidator,
                                   string topic,
                                   IEnumerable<FindEventResult> results,
                                   Dictionary<FindEventResult, XmlElement> elements)
        {
            Func<FindEventResult, bool> eventCheck = E => CheckEventTopic(E, elements[E], topic, TNS1NAMESPACE);

            // select events
            List<FindEventResult> filtered = results.Where(eventCheck).ToList();

            BeginStep(string.Format("Check that all events with topic '{0}' relate to recording '{1}'",
                                    topic, recordingInfo.RecordingToken));

            StringBuilder sb = new StringBuilder();
            bool ok = true;
            // no other tracks
            foreach (FindEventResult result in filtered)
            {
                if (result.RecordingToken != recordingInfo.RecordingToken)
                {
                    ok = false;
                    sb.AppendFormat("Event with topic={0}, Time={1} relates to recording {2}{3}",
                                    topic, result.Time.StdDateTimeToString(), result.RecordingToken, Environment.NewLine);
                }

                //if (!recordingInfo.Track.Any(T => T.TrackToken == result.TrackToken))
                if (!trackTokenValidator(result.TrackToken))
                {
                    ok = false;
                    sb.AppendFormat("Event with topic={0}, Time={1} relates to track {2}, which does not belong to recording {3}{4}",
                                    topic, result.Time.StdDateTimeToString(), result.TrackToken, recordingInfo.RecordingToken, Environment.NewLine);
                }
            }

            if (!ok)
            {
                throw new AssertException(sb.ToStringTrimNewLine());
            }
            StepPassed();
        }
        
        void ValidateOrder(IList<FindEventResult> results, DateTime start, DateTime end)
        {
            ValidateOrder(results, R => R.Time, start, end);
        }

        void CompareLists(IEnumerable<FindEventResult> list1,  IEnumerable<FindEventResult> list2,
                          Dictionary<FindEventResult, XmlElement> rawMessages1, Dictionary<FindEventResult, XmlElement> rawMessages2,
                          string descr1, string descr2)
        {
            bool ok = true;
            StringBuilder dump = new StringBuilder();

            Dictionary<FindEventResult, FindEventResult> intersection = new Dictionary<FindEventResult, FindEventResult>();

            Func<FindEventResult, string, string> getDescription =
                    (e, topic) =>
                    {
                        return string.Format("Event with Time={0}, RecordingToken='{1}', TrackToken='{2}', Topic='{3}'", e.Time, e.RecordingToken, e.TrackToken, topic);
                    };  


            // check events in list1, find events that are not in list2, 
            // find events that are in both lists (intersection)

            foreach (FindEventResult ev1 in list1)
            {
                if (!ev1.StartStateEvent)
                {
                    XmlText topicText = GetTopicElement(ev1);
                    TopicInfo topicInfo = CreateTopicInfo(ev1, rawMessages1[ev1]);

                    List<FindEventResult> filtered2 = list2.Where(E => AreTheSame(ev1, topicInfo, E, rawMessages2[E])).ToList();
                    string topic = topicText.Value;

                    string descr = getDescription(ev1, topic);

                    if (!filtered2.Any())
                    {
                        ok = false;
                        dump.AppendFormat("{0} not found in {1}{2}", descr, descr2, Environment.NewLine);
                    }
                    else
                    {
                        FindEventResult ev2 = filtered2.FirstOrDefault(E => HaveTheSamePayload(ev1, E));

                        if (ev2 != null)
                        {
                            intersection.Add(ev1, ev2);
                        }
                        else
                        { 
                            // how to describe the error ?????
                            ok = false;
                            dump.AppendFormat("For {0} (or one of them) in {1}, no event with the same Data can be found in {2}{3}", descr, descr1, descr2, Environment.NewLine);
                        }
                    }
                }
            }

            // check events in list2, find events that are not in list1
            
            foreach (FindEventResult ev2 in list2)
            {
                if (!ev2.StartStateEvent)
                {
                    XmlText topicText = GetTopicElement(ev2);
                    TopicInfo topicInfo = CreateTopicInfo(ev2, rawMessages2[ev2]);
                    string topic = topicText.Value;
                    string descr = getDescription(ev2, topic);

                    List<FindEventResult> filtered1 = list1.Where(E => AreTheSame(ev2, topicInfo, E, rawMessages1[E])).ToList();


                    if (!filtered1.Any())
                    {
                        ok = false;
                        dump.AppendFormat("{0} not found in {1}{2}", descr, descr1, Environment.NewLine);
                    }
                    else
                    {
                        FindEventResult ev1 = filtered1.FirstOrDefault(E => HaveTheSamePayload(ev2, E));
                        if (ev1 == null)
                        {
                            ok = false;
                            dump.AppendFormat("For {0} (or one of them) in {1}, no event with the same Data can be found in {2}{3}", descr, descr2, descr1, Environment.NewLine);
                        }
                    }
                }
            }


            // compare event in intersection
            // StartStateEvent
            // in Event:  ProducerReference, SubscriptionReference (should not both be empty?)
            // message items...
            
            foreach (FindEventResult ev1 in intersection.Keys)
            {
                bool localOk = true;
                StringBuilder sb = new StringBuilder(getDescription(ev1, GetTopicElement(ev1).Value));
                sb.AppendLine(" is different: ");

                FindEventResult ev2 = intersection[ev1];
                
                if (ev1.StartStateEvent != ev2.StartStateEvent)
                {
                    sb.AppendLine("StartStateEvent fields don't match");
                }

                // ProducerReference ?
                if (ev1.Event.ProducerReference != null && ev2.Event.ProducerReference != null)
                {
                    if (ev1.Event.ProducerReference.Address.Value != ev2.Event.ProducerReference.Address.Value)
                    {
                        localOk = false;
                        sb.AppendFormat("Event.ProducerReference fields don't match {0}", Environment.NewLine);
                    }
                }
                else
                {
                    if (ev1.Event.ProducerReference != null)
                    {
                        localOk = false;
                        sb.AppendFormat("Event.ProducerReference is missing in message from {0}{1}", descr2,
                                        Environment.NewLine);
                    }
                    if (ev2.Event.ProducerReference != null)
                    {
                        localOk = false;
                        sb.AppendFormat("Event.ProducerReference is missing in message from {0}{1}", descr1,
                                        Environment.NewLine);
                    }
                }

                // SubscriptionReference (?)
                if (ev1.Event.SubscriptionReference != null && ev2.Event.SubscriptionReference != null)
                {
                    if (ev1.Event.SubscriptionReference.Address.Value != ev2.Event.SubscriptionReference.Address.Value)
                    {
                        localOk = false;
                        sb.AppendFormat("Event.SubscriptionReference fields don't match {0}", Environment.NewLine);
                    }
                }
                else
                {
                    if (ev1.Event.SubscriptionReference != null)
                    {
                        localOk = false;
                        sb.AppendFormat("Event.SubscriptionReference is missing in message from {0}{1}", descr2,
                                        Environment.NewLine);
                    }
                    if (ev2.Event.SubscriptionReference != null)
                    {
                        localOk = false;
                        sb.AppendFormat("Event.SubscriptionReference is missing in message from {0}{1}", descr1,
                                        Environment.NewLine);
                    }
                }

                if (!localOk)
                {
                    ok = false;
                    dump.Append(sb.ToString());
                }            
            
            }
            
            Assert(ok, dump.ToStringTrimNewLine(), "Check that events lists are the same");
        }

        bool AreTheSame(FindEventResult ev1, TopicInfo topic1, FindEventResult ev2, XmlElement element2)
        {
            // Two Event Results will be assumed as the same, if they have the same values for 
            // GetEventSearchResultsResponse.ResultList.Result.RecordingToken, 
            // GetEventSearchResultsResponse.ResultList.Result.TrackToken, 
            // GetEventSearchResultsResponse.ResultList.Result.Time, 
            // GetEventSearchResultsResponse.ResultList.Result.Event.Topic and 
            // fields and values of GetEventSearchResultsResponse.ResultList.Result.Event.Message.Data.
            
            bool scopeMatches = (ev1.Time == ev2.Time) && (ev2.RecordingToken == ev1.RecordingToken) &&
                                (ev1.TrackToken == ev2.TrackToken);

            bool topicMatches = false;
            if (scopeMatches)
            {
                TopicInfo t2 = CreateTopicInfo(ev2, element2);
                topicMatches = TopicInfo.TopicsMatch(topic1, t2);
            }
            
            return topicMatches;
        }

        bool HaveTheSamePayload(FindEventResult ev1, FindEventResult ev2)
        {
            bool theSame = true;

            if (ev1.Event != null && ev2.Event != null)
            {
                if (ev1.Event.Message != null && ev2.Event.Message != null)
                {
                    Dictionary<string, string> items1 = GetMessageSimpleItems(ev1.Event.Message);
                    Dictionary<string, string> items2 = GetMessageSimpleItems(ev2.Event.Message);

                    List<string> common = new List<string>();
                    bool localOk = true;

                    foreach (string key in items1.Keys)
                    {
                        if (!items2.ContainsKey(key))
                        {
                            localOk = false;
                            break;
                        }
                        else
                        {
                            common.Add(key);
                        }
                    }
                    if (localOk)
                    {
                        foreach (string key in items2.Keys)
                        {
                            if (!items2.ContainsKey(key))
                            {
                                localOk = false;
                                break;
                            }
                        }
                    }
                    if (localOk)
                    {
                        foreach (string key in common)
                        {
                            if (items1[key] != items2[key])
                            {
                                localOk = false;
                            }
                        }
                    }

                    theSame = localOk;
                }
                else
                {
                    theSame = (ev1.Event.Message == null && ev2.Event.Message == null);
                }
            }
            else
            {
                theSame = (ev1.Event == null && ev2.Event == null);
            }
            return theSame;
        }

        void ValidateMessages(IEnumerable<FindEventResult> eventResults)
        {
            StringBuilder sb = new StringBuilder();
            BeginStep("Validate messages");
            bool ok = true;
            foreach (FindEventResult eventResult in eventResults)
            {
                bool local = ValidateMessage(eventResult, sb);
                ok = ok && local;
            }
            if (!ok)
            {
                throw new AssertException(sb.ToStringTrimNewLine());
            }
            StepPassed();
        }

        bool ValidateMessage(FindEventResult eventResult, StringBuilder dump)
        {
            XmlText topicText = GetTopicElement(eventResult);
            string topic = topicText.Value;

            bool valid = true;

            List<string> simpleItems = new List<string>();
            XmlNamespaceManager manager = new XmlNamespaceManager(eventResult.Event.Message.OwnerDocument.NameTable);
            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            string path;
            path = "/tt:Data/tt:SimpleItem";
            XmlNodeList itemNodesList = eventResult.Event.Message.SelectNodes(path, manager);

            StringBuilder local  =new StringBuilder();

            foreach (XmlNode node in itemNodesList)
            {
                XmlElement element = node as XmlElement;
                if (element == null)
                {
                    continue;
                }
                string name = element.GetAttribute(OnvifMessage.NAME);
                if (simpleItems.Contains(name))
                {
                    valid = false;
                    local.AppendFormat("   Multiple SimpleItem elements with 'Name' attribute set to '{0}' found{1}", name, Environment.NewLine);
                }
                simpleItems.Add(name);
            }
            if (!valid)
            {
                dump.AppendFormat("Message for event with time={0}, topic ={1} is incorrect: {2}", eventResult.Time.StdDateTimeToString(), topic, Environment.NewLine);
                dump.Append(local.ToString());
            }
            return valid;
        }


        #region Common Test


        void CommonForwardBackwardSearchEventsTest(Func<RecordingInformation, SearchRange> defineRange,
                                                   bool includeStartStateInSearch,
                                                   bool validateEdges,
                                                   bool outsideRange)
        {
            string searchToken = string.Empty;
            SearchState state = SearchState.Completed;

            RunTest(() =>
                    {
                        string keepAlive = string.Format("PT{0}S", _searchKeepAlive);

                        var recordingInfo = FindRecordingForTest();
                        var recording = GetRecordings().FirstOrDefault(e => e.RecordingToken == recordingInfo.RecordingToken);

                        Assert(null != recording,
                               string.Format("No recording with token '{0}' in GetRecordings response", recordingInfo.RecordingToken),
                               string.Format("Check recording with token '{0}' exists", recordingInfo.RecordingToken));

                        SearchRange range = defineRange(recordingInfo);

                        DateTime start = range.Start;
                        DateTime end = range.End;

                        List<FindEventResult> results = new List<FindEventResult>();

                        Dictionary<FindEventResult, XmlElement> elements1 = new Dictionary<FindEventResult, XmlElement>();
                        Dictionary<FindEventResult, XmlElement> elements2 = new Dictionary<FindEventResult, XmlElement>();
                        Dictionary<FindEventResult, XmlElement> elements = elements1;

                        EventFilter filter = new EventFilter();

                        SearchScope scope = new SearchScope();
                        scope.IncludedRecordings = new string[] { recordingInfo.RecordingToken };

                        Action<DateTime, DateTime> validateAction =
                                (startTime, endTime) =>
                                {
                                    state = SearchState.Completed;
                                    searchToken = FindEvents(scope, filter, startTime, endTime, includeStartStateInSearch, null, keepAlive);

                                    Dictionary<FindEventResult, XmlDocument> rawResults = new Dictionary<FindEventResult, XmlDocument>();

                                    results = GetAllEventsSearchResults(searchToken, null, null, "PT5S", elements, out state);

                                    Assert(results != null && results.Any(), "No events found", "Check that events list is not empty");

                                    ValidateMessages(results);

                                    //GetMessageElements(results, rawResults, elements);

                                    ValidateRecordingEvents(recordingInfo, (trackToken) => recording.Tracks.Track.Any(T => T.TrackToken == trackToken), results, elements, validateEdges, outsideRange);

                                    ValidateOrder(results, startTime, endTime);
                                };

                        validateAction(start, end);
                        List<FindEventResult> results1 = new List<FindEventResult>();
                        results1.AddRange(results);

                        elements = elements2;
                        validateAction(end, start);
                        List<FindEventResult> results2 = new List<FindEventResult>();
                        results2.AddRange(results);

                        // compare lists

                        CompareLists(results1, results2, elements1, elements2, "list received for StartPoint < EndPoint", "list received for StartPoint > EndPoint");
                    },
                    () =>
                    {
                        if (state != SearchState.Completed)
                        {
                            ReleaseSearch(searchToken, _searchTimeout);
                        }
                    });
        }


        #endregion

    }
}
