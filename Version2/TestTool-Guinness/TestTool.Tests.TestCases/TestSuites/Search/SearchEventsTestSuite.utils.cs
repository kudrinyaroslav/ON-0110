using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.TestCases.Utils.Events;
using DateTime=System.DateTime;

namespace TestTool.Tests.TestCases
{
    partial class SearchEventsTestSuite
    {
        readonly string TNS1NAMESPACE = string.Format("tns1={0}", BaseNotification.TOPICSNAMESPACE);
        
        /*
            <wsnt:Message>
              <tt:Message UtcTime="2010-09-28T14:17:12.801" PropertyOperation="Initialized">
                <tt:Source>
                  <tt:SimpleItem Name="VideoSourceToken" Value="1"></tt:SimpleItem>
                </tt:Source>
                <tt:Data>
                  <tt:SimpleItem Name="State" Value="true"></tt:SimpleItem>
                </tt:Data>
              </tt:Message> 
            </wsnt:Message>*/        
        
        void GetMessageElements(List<FindEventResult> result, 
            Dictionary<FindEventResult, XmlDocument> rawResults,
            Dictionary<FindEventResult, XmlElement> messageElements)
        {
            
            string messagePath;
            messagePath = "/s:Envelope/s:Body/search:GetEventSearchResultsResponse/search:ResultList/onvif:Result/onvif:Event";
            
            for (int i = 0; i<result.Count; )
            {
                FindEventResult ev = result[i];
                XmlDocument soapRawPacket = rawResults[ev];
                XmlNamespaceManager manager = CreateNamespaceManager(soapRawPacket);
                XmlNodeList responseNodeList = soapRawPacket.SelectNodes(messagePath, manager);

                for (int j = 0; j < responseNodeList.Count; j++ )
                {
                    XmlNode node = responseNodeList[j];
                    XmlElement element = node as XmlElement;
                    if (element != null)
                    {
                        messageElements.Add(ev, element);
                        i++;
                        if (i < result.Count)
                        {
                            ev = result[i];
                        }
                    }
                }
            }
        }

        XmlNamespaceManager CreateNamespaceManager(XmlDocument soapRawPacket)
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(soapRawPacket.NameTable);
            manager.AddNamespace("s", "http://www.w3.org/2003/05/soap-envelope");
            manager.AddNamespace("search", "http://www.onvif.org/ver10/search/wsdl");
            manager.AddNamespace("onvif", "http://www.onvif.org/ver10/schema");
            manager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");

            return manager;
        }
        
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

            TopicInfo actual = TopicInfo.ExtractTopicInfo(text.Value, topicElement);
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

        RecordingInformation FindRecordingForTest()
        {
            RecordingInformation recording = null;
            RunStep(() => { recording = Client.GetRecordingInformation(_recordingToken); }, "Get Recording Information");

            Assert(recording != null, "Recording not found", "Check that recording for test found");

            Assert(recording.Track != null && recording.Track.Length > 0, "Recording has no tracks", "Check that recording has tracks");

            return recording;
        }

        void DefineEventsSearchRange(RecordingInformation recording, out DateTime start, out DateTime end)
        {
            DateTime minDate = DateTime.MaxValue;
            DateTime maxDate = DateTime.MinValue;

            foreach (TrackInformation trackInformation in recording.Track)
            {
                if (trackInformation.DataFrom < minDate)
                {
                    minDate = trackInformation.DataFrom;
                }
                if (trackInformation.DataTo > maxDate)
                {
                    maxDate = trackInformation.DataTo;
                }
            }

            start = recording.EarliestRecordingSpecified ? recording.EarliestRecording : minDate;
            end = recording.LatestRecordingSpecified ? recording.LatestRecording : maxDate;
        }

        void ValidateRecordingEvents(RecordingInformation recording,
            IEnumerable<FindEventResult> results,
            Dictionary<FindEventResult, XmlElement> elements)
        {
            ValidateRecordingEvents(recording, results, elements, true);
        }

        void ValidateRecordingEvents(RecordingInformation recording,
            IEnumerable<FindEventResult> results,
            Dictionary<FindEventResult, XmlElement> elements,
            bool checkEdges)
        {

            // check sequence
            ValidateRecordingEventsSequence(recording, results, elements, checkEdges);
            
            // no other recordings
            ValidateEventsContext(recording, "tns1:RecordingHistory/Track/State", results, elements);

            foreach (TrackInformation track in recording.Track)
            {
                if (track.DataTo > track.DataFrom)
                {
                    ValidateTrackEvents(recording, track, results, elements, checkEdges);
                }
            }

            ValidateEventsContext(recording, "tns1:RecordingHistory/Track/VideoParameters", results, elements);

            ValidateEventsContext(recording, "tns1:RecordingHistory/Track/AudioParameters", results, elements);

        }

        void ValidateRecordingEventsSequence(RecordingInformation recording,
            IEnumerable<FindEventResult> results,
            Dictionary<FindEventResult, XmlElement> elements,
            bool checkEdges)
        {
            Func<FindEventResult, bool> eventCheck;

            //
            // recording events 
            //
            eventCheck =
                (E =>
                {
                    bool topicMatches = CheckEventTopic(E, elements[E], "tns1:RecordingHistory/Recording/State",
                                                        TNS1NAMESPACE);

                    return (topicMatches);
                });

            // select Recording/State events
            List<FindEventResult> filtered = results.Where(eventCheck).OrderBy(E => E.Time).ToList();

            // check that there is no events for other recordings
            List<FindEventResult> nonmatching =
                filtered.Where(E => E.RecordingToken != recording.RecordingToken).ToList();

            Assert(nonmatching.Count == 0,
                   "Event with topic 'tns1:RecordingHistory/Recording/State' for other recordings found",
                   string.Format(
                       "Check that all events with topic 'tns1:RecordingHistory/Recording/State' relate to recording '{0}'",
                       recording.RecordingToken));

            bool isRecordingOk = true;
            bool? lastIsRecording = null;
            if (checkEdges)
            {
                lastIsRecording = false;

            
                // when checkEdges = false (narrowed interval), 1 event is OK (only virtual event)
                Assert(filtered.Count >= 2, "Not enough tns1:RecordingHistory/Recording/State events found", 
                    "Check that at least two events with topic tns1:RecordingHistory/Recording/State found");
                
            }


            StringBuilder dump = new StringBuilder();
            for (int i = 0; i < filtered.Count; i++)
            {
                FindEventResult ev = filtered[i];
                
                XmlElement message = ev.Event.Message;

                string value = GetSimpleItem(message, "IsRecording");

                if (value == null)
                {
                    isRecordingOk = false;
                    dump.AppendFormat(
                        string.Format(
                            "IsRecording value not found for event with RecordingToken='{0}', Time='{1}'{2}",
                            ev.RecordingToken, ev.Time.StdDateTimeToString(), Environment.NewLine));
                }
                else
                {
                    bool isRecording;
                    bool attrOk = bool.TryParse(value, out isRecording);
                    if (!attrOk)
                    {
                        isRecordingOk = false;
                        dump.AppendFormat(
                            string.Format(
                                "IsRecording value for event with RecordingToken='{0}', Time='{1}' is incorrect ({2}){3}",
                                ev.RecordingToken, ev.Time.StdDateTimeToString(), value, Environment.NewLine));
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
                                if (i == 0)
                                {
                                    dump.AppendFormat(
                                        string.Format(
                                            "IsRecording value for first event with RecordingToken='{0}', Time='{1}' is false {2}",
                                            ev.RecordingToken, ev.Time.StdDateTimeToString(), Environment.NewLine));
                                }
                                else
                                {
                                    dump.AppendFormat(
                                        string.Format(
                                            "IsRecording value for event with RecordingToken='{0}', Time='{1}' is {2} while previous is also {2}{3}",
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
            bool checkEdges)
        {
            {
                Func<FindEventResult, bool> eventCheck =
                    (E =>
                    {
                        bool topicMatches = CheckEventTopic(E, elements[E], "tns1:RecordingHistory/Track/State",
                                                            TNS1NAMESPACE);

                        return (topicMatches);
                    });

                List<FindEventResult> filtered = results.Where(E=> eventCheck(E) && E.TrackToken == track.TrackToken).OrderBy( E => E.Time).ToList();
                
                bool dataPresentOk = true;
                bool? lastDataPresent = null;
                if (checkEdges)
                {
                    lastDataPresent = false;

                    // checkEdges = false means that interval is narrowed and virtual events are sent
                    // in this case 1 event is OK
                    Assert(filtered.Count >= 2,
                           string.Format(
                               "Not enough events with topic=tns1:RecordingHistory/Track/State, RecordingToken='{0}' for track '{1}' found in the response",
                               recording.RecordingToken, track.TrackToken),
                           string.Format("Check that at least two events with topic tns1:RecordingHistory/Track/State are present for track '{0}'", track.TrackToken));

                }

                StringBuilder dump = new StringBuilder();
                for (int i =0; i< filtered.Count; i++)
                {
                    FindEventResult ev = filtered[i];

                    XmlElement message = ev.Event.Message;

                    string value = GetSimpleItem(message, "IsDataPresent");

                    if (value == null)
                    {
                        dataPresentOk = false;
                        dump.AppendFormat(
                            string.Format(
                                "IsDataPresent value not found for event with RecordingToken='{0}', Time='{1}'{2}",
                                ev.RecordingToken, ev.Time.StdDateTimeToString(), Environment.NewLine));
                    }
                    else
                    {
                        bool dataPresent;
                        bool attrOk = bool.TryParse(value, out dataPresent);
                        if (!attrOk)
                        {
                            dataPresentOk = false;
                            dump.AppendFormat(
                                string.Format(
                                    "IsDataPresent value for event with RecordingToken='{0}', Time='{1}' is incorrect ({2}){3}",
                                    ev.RecordingToken, ev.Time.StdDateTimeToString(), value, Environment.NewLine));
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
                                    if (i == 0)
                                    {
                                        dump.AppendFormat(
                                            string.Format(
                                                "IsDataPresent value for first event with RecordingToken='{0}', Time='{1}' is false {2}",
                                                ev.RecordingToken, ev.Time.StdDateTimeToString(), Environment.NewLine));
                                    }
                                    else
                                    {
                                        dump.AppendFormat(
                                            string.Format(
                                                "IsDataPresent value for event with RecordingToken='{0}', Time='{1}' is {2} while previous is also {2}{3}",
                                                ev.RecordingToken, ev.Time.StdDateTimeToString(), dataPresent,
                                                Environment.NewLine));
                                    }
                                }
                            }
                            lastDataPresent = dataPresent;

                        }
                    }
                }
                
                Assert(dataPresentOk, dump.ToStringTrimNewLine(), "Check that IsDataPresent values in events are correct");
            }
        }

        void ValidateEventsContext(RecordingInformation recording, 
            string topic,
            IEnumerable<FindEventResult> results,
            Dictionary<FindEventResult, XmlElement> elements)
        {
            Func<FindEventResult, bool> eventCheck =
                (E =>
                     {
                         bool topicMatches = CheckEventTopic(E, elements[E], topic,
                                                             TNS1NAMESPACE);

                         return (topicMatches);
                     });

            // select events
            List<FindEventResult> filtered = results.Where(eventCheck).ToList();

            BeginStep(string.Format(
                       "Check that all events with topic '{0}' relate to recording '{1}'",
                        topic, recording.RecordingToken));

            StringBuilder sb = new StringBuilder();
            bool ok = true;
            // no other tracks
            foreach (FindEventResult result in filtered)
            {
                if (result.RecordingToken != recording.RecordingToken)
                {
                    ok = false;
                    sb.AppendFormat("Event with topic={0}, Time={1} relates to recording {2}{3}",
                        topic, result.Time.StdDateTimeToString(), result.RecordingToken, Environment.NewLine);
                }

                if (recording.Track.Where(T => T.TrackToken == result.TrackToken).Count() == 0)
                {
                    ok = false;
                    sb.AppendFormat("Event with topic={0}, Time={1} relates to track {2}, which does not belong to recording {3}{4}",
                        topic, result.Time.StdDateTimeToString(), result.TrackToken, recording.RecordingToken, Environment.NewLine);
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

            bool ok;
            bool ascending = start < end;
            ok = true;
            for (int i = 1; i < results.Count; i++)
            {
                if (results[i - 1].Time == results[i].Time)
                {
                    continue;
                }
                if (results[i - 1].Time <= results[i].Time != ascending)
                {
                    ok = false;
                }
            }

            Assert(ok, "Results order is broken", string.Format("Check that results are ordered {0}", ascending ? "ascending" : "descending"));
        }

        void CompareLists(IEnumerable<FindEventResult> list1,  IEnumerable<FindEventResult> list2,
            Dictionary<FindEventResult, XmlElement> rawMessages1, Dictionary<FindEventResult, XmlElement> rawMessages2,
            string descr1, string descr2)
        {
            bool ok = true;
            StringBuilder dump = new StringBuilder();

            Dictionary<FindEventResult, FindEventResult> intersection = new Dictionary<FindEventResult, FindEventResult>();

            Func<FindEventResult, string, string> getDescription =
                new Func<FindEventResult, string, string>(
                    (e, topic) =>
                        {
                            return string.Format("Event with Time={0}, RecordingToken='{1}', TrackToken='{2}', Topic='{3}'", e.Time, e.RecordingToken, e.TrackToken, topic);
                        });  


            // check events in list1, find events that are not in list2, 
            // find events that are in both lists (intersection)

            foreach (FindEventResult ev1 in list1)
            {
                if (!ev1.StartStateEvent)
                {
                    XmlText topicText = GetTopicElement(ev1);
                    TopicInfo topicInfo = CreateTopicInfo(ev1, rawMessages1[ev1]);

                    FindEventResult ev2 = list2.FirstOrDefault(E => AreTheSame(ev1, topicInfo, E, rawMessages2[E]));
                    string topic = topicText.Value;

                    string descr = getDescription(ev1, topic);

                    if (ev2 == null)
                    {
                        ok = false;
                        dump.AppendFormat("{0} not found in {1}{2}",
                                          descr, descr2, Environment.NewLine);
                    }
                    else
                    {
                        intersection.Add(ev1, ev2);
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

                    FindEventResult ev1 = list1.FirstOrDefault(E => AreTheSame(ev2, topicInfo, E, rawMessages1[E]));

                    if (ev1 == null)
                    {
                        ok = false;
                        dump.AppendFormat("{0} not found in {1}{2}",
                            descr, descr1, Environment.NewLine);
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


                // Message items
                if (ev1.Event.Message != null && ev2.Event.Message != null)
                {
                    Dictionary<string, string> items1 = GetMessageSimpleItems(ev1.Event.Message);
                    Dictionary<string, string> items2 = GetMessageSimpleItems(ev2.Event.Message);

                    List<string> common = new List<string>();

                    foreach (string key in items1.Keys)
                    {
                        if (!items2.ContainsKey(key))
                        {
                            localOk = false;
                            sb.AppendFormat("SimpleItem with name='{0}' not found in message from {1}{2}", key, descr2,
                                            Environment.NewLine);
                        }
                        else
                        {
                            common.Add(key);
                        }
                    }
                    foreach (string key in items2.Keys)
                    {
                        if (!items2.ContainsKey(key))
                        {
                            localOk = false;
                            sb.AppendFormat("SimpleItem with name='{0}' not found in message from {1}{2}", key, descr1,
                                            Environment.NewLine);
                        }
                    }

                    foreach (string key in common)
                    {
                        if (items1[key] != items2[key])
                        {
                            localOk = false;
                            sb.AppendFormat("SimpleItem elements with name='{0}' have different values {1}", key, Environment.NewLine);
                        }
                    }

                    if (!localOk)
                    {
                        ok = false;
                        dump.Append(sb.ToString());
                    }
                }
                else
                {
                    if (ev1.Event.Message == null)
                    {
                        sb.AppendLine(string.Format("Message field is missing in structure from {0}", descr1));
                    }
                    if (ev2.Event.Message == null)
                    {
                        sb.AppendLine(string.Format("Message field is missing in structure from {0}", descr2));
                    }
                }
            }
            
            Assert(ok, dump.ToStringTrimNewLine(), "Check that events lists are the same");
        }

        bool AreTheSame(FindEventResult ev1, TopicInfo topic1, FindEventResult ev2, XmlElement element2)
        {
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
                items.Add(element.GetAttribute(OnvifMessage.NAME/*, OnvifMessage.ONVIF*/), element.GetAttribute(OnvifMessage.VALUE/*, OnvifMessage.ONVIF*/));
            }

            return items;
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

    }
}
