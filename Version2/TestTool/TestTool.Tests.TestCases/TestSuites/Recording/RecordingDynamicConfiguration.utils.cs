using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using Event = TestTool.Proxies.Event;
using TestTool.Tests.Definitions.Exceptions;
using System.Xml;
using TestTool.Tests.TestCases.Utils.Events;

namespace TestTool.Tests.TestCases.TestSuites.Recording
{
    public partial class RecordingControlRecordingTestSuite : RecordingTest
    {

        private void CheckRecordingTrack(GetTracksResponseItem track, TrackConfiguration conf)
        {
            bool ok = true;
            StringBuilder logger = new StringBuilder();
            if (track.Configuration.Description != conf.Description)
            {
                ok = false;
                logger.Append(string.Format("Description is invalid{0}", Environment.NewLine));
            }
            if (track.Configuration.TrackType != conf.TrackType)
            {
                ok = false;
                logger.Append(string.Format("Track type is invalid{0}", Environment.NewLine));
            }

            Assert(ok, logger.ToStringTrimNewLine(),
                   "Check that configuration parameters of new track are valid");
        }

        private string CreateRecording(out GetRecordingsResponseItem[] recordings,
                                       out GetRecordingsResponseItem[] recordingsRefreshed,
                                       out string recordingToken)
        {
            var capabilities = GetServiceCapabilities();
            Assert(capabilities != null,
                   "Capabilities hasn't been returned",
                   "Check that capabilities has been returned");

            Assert(capabilities.DynamicRecordings,
                   "Recording creating operation is unavailable",
                   "Check that recording can be created");

            recordings = GetRecordings();

            string token = string.Empty;
            if (recordings != null && recordings.Length == capabilities.MaxRecordings)
            {
                bool isDelete = false;
                foreach (var item in recordings)
                {
                    token = item.RecordingToken;
                    try
                    {
                        DeleteRecording(token);
                        isDelete = true;
                        break;
                    }
                    catch (FaultException exc)
                    {
                        if (exc.IsValidOnvifFault("Receiver/Action/CannotDelete"))
                        {
                            LogStepEvent(string.Format("Can't delete receiver (token = {0})", token));
                            StepPassed();
                        }
                        else
                        {
                            throw exc;
                        }
                    }
                }
                Assert(isDelete, "No receivers delete",
                       "Check that receiver was deleted");
                recordings = GetRecordings();
            }

            RecordingConfiguration conf = new RecordingConfiguration();
            conf.Source = new RecordingSourceInformation();
            conf.Source.Description = "SourceDescription";
            conf.Source.SourceId = "http://localhost/sourceID";
            conf.Source.Location = "LocationDescription";
            conf.Source.Name = "CameraName";
            conf.Source.Address = "http://localhost/address";
            conf.MaximumRetentionTime = "PT0S";
            conf.Content = "Recording from device";


            var newToken = CreateRecording(conf);
            recordingToken = newToken;
            if (recordings != null)
                Assert(recordings.FirstOrDefault(r => r.RecordingToken == newToken) == null,
                       "Recording token is not unique",
                       "Check that recording list doesn't contain recording token of recording created");
            recordingsRefreshed = GetRecordings();
            Assert(recordingsRefreshed != null && recordingsRefreshed.Length > 0,
                   "Recording list is empty",
                   "Check that recording list is not empty");
            var recording = recordingsRefreshed.FirstOrDefault(r => r.RecordingToken == newToken);
            Assert(recording != null,
                   "Recording list doesn't contain new recording",
                   "Check that recording list contains new recording after refresh");
            CheckRecording(capabilities, recording, conf);
            return newToken;
        }

        protected void CheckRecording(RecordingServiceCapabilities capabilities, GetRecordingsResponseItem recording, RecordingConfiguration conf)
        {
            //var recording = recordings.Where(r => r.RecordingToken == recordingToken).FirstOrDefault();
            //Assert(recording != null,
            //    "recording list doesn't contain new recording",
            //        "Check that recording list contains new recording after refresh");
            bool ok = true;
            StringBuilder logger = new StringBuilder();

            if (recording.Configuration.MaximumRetentionTime != conf.MaximumRetentionTime)
            {
                ok = false;
                logger.Append(string.Format("MaximumRetentionTime is invalid{0}", Environment.NewLine));
            }
            if (recording.Configuration.Content != conf.Content)
            {
                ok = false;
                logger.Append(string.Format("Content is '{0}' but must be '{1}'{2}",
                                            recording.Configuration.Content, conf.Content, Environment.NewLine));
            }

            if (recording.Configuration.Source != null)
            {
                if (recording.Configuration.Source.Address != conf.Source.Address)
                {
                    ok = false;
                    logger.Append(string.Format("Source Address is '{0}' but must be '{1}'{2}",
                                                recording.Configuration.Source.Address, conf.Source.Address,
                                                Environment.NewLine));
                }
                if (recording.Configuration.Source.Description != conf.Source.Description)
                {
                    ok = false;
                    logger.Append(string.Format("Source Description is '{0}' but must be '{1}'{2}",
                                                recording.Configuration.Source.Description, conf.Source.Description,
                                                Environment.NewLine));
                }
                if (recording.Configuration.Source.Location != conf.Source.Location)
                {
                    ok = false;
                    logger.Append(string.Format("Source Location is '{0}' but must be '{1}'{2}",
                                                recording.Configuration.Source.Location, conf.Source.Location,
                                                Environment.NewLine));
                }
                if (recording.Configuration.Source.Name != conf.Source.Name)
                {
                    ok = false;
                    logger.Append(string.Format("Source Name is '{0}' but must be '{1}'{2}",
                                                recording.Configuration.Source.Name, conf.Source.Name,
                                                Environment.NewLine));
                }
                if (recording.Configuration.Source.SourceId != conf.Source.SourceId)
                {
                    ok = false;
                    logger.Append(string.Format("Source SourceId is '{0}' but must be '{1}'{2}",
                                                recording.Configuration.Source.SourceId, conf.Source.SourceId,
                                                Environment.NewLine));
                }
            }
            else
            {
                ok = false;
                logger.Append(string.Format("recording doesn't contain Source{0}", Environment.NewLine));
            }
            if (recording.Tracks == null || recording.Tracks.Track == null)
            {
                ok = false;
                logger.Append(string.Format("Track list of recording '{0}' is empty", recording.RecordingToken));
            }
            else
            {
                foreach (TrackType type in Enum.GetValues(typeof(TrackType)))
                {
                    if (type != TrackType.Extended)
                    {
                        //if (recording.Tracks.Track.FirstOrDefault(t => t.Configuration.TrackType == type) == null)
                        //{
                        //    ok = false;
                        //    logger.Append(string.Format("Recording doesn't contain tracks with track type '{0}'{1}", type, Environment.NewLine));
                        //}
                        var actualTrackCount = recording.Tracks.Track.Count(t => t.Configuration.TrackType == type);
                        if (TrackType.Audio == type)
                        {
                            var audioEncodingsCount = capabilities.Encoding.Count(e =>
                                                                                  {
                                                                                      AudioEncoding v;
                                                                                      return AudioEncoding.TryParse(e, out v);
                                                                                  });

                            if (0 != audioEncodingsCount)
                            {
                                var flag = (1 <= actualTrackCount);
                                if (!flag)
                                    logger.AppendLine(string.Format("There are no tracks of type: '{0}'.", type));
                                ok = ok && flag;
                            }
                        }

                        if (TrackType.Video == type)
                        {
                            var videoEncodingsCount = capabilities.Encoding.Count(e =>
                                                                                  {
                                                                                      VideoEncoding v;
                                                                                      return VideoEncoding.TryParse(e, out v);
                                                                                  });

                            if (0 != videoEncodingsCount)
                            {
                                var flag = (1 <= actualTrackCount);
                                if (!flag)
                                    logger.AppendLine(string.Format("There are no tracks of type: '{0}'.", type));
                                ok = ok && flag;
                            }
                        }

                        if (TrackType.Metadata == type && Features.ContainsFeature(Feature.MetadataRecording))
                        {
                            var flag = (1 <= actualTrackCount);
                            if (!flag)
                                logger.AppendLine(string.Format("There are no tracks of type: '{0}'.", type));
                            ok = ok && flag;
                        }
                    }
                }
            }

            Assert(ok, logger.ToStringTrimNewLine(), "Check that configuration parameters of new recording are valid");
        }

        private void CheckTrackListChanged(GetTracksResponseItem[] prevList, GetTracksResponseItem[] newList, string stepName)
        {
            GetTracksResponseItem track = null;
            StringBuilder logger = new StringBuilder();
            bool ok = true;

            if (prevList == null && newList != null ||
                 prevList != null && newList == null)
            {
                ok = false;
                logger.Append("Initial track list was changed");

                Assert(ok, logger.ToStringTrimNewLine(),  stepName);

                return;
            }
            else if (prevList != null && newList != null)
            {
                if (prevList.Length != newList.Length)
                {
                    ok = false;
                    logger.Append("Initial track list was changed");

                    Assert(ok, logger.ToStringTrimNewLine(), stepName);
                }

                 foreach (var item in prevList)
                {
                    track = newList.FirstOrDefault(t => t.TrackToken == item.TrackToken);
                    if (track == null)
                    {
                        ok = false;
                        logger.Append("Initial track list was changed");
                    }
                    else
                    {
                        ok &= CompareTracks(item, track, logger);
                    }
                }
            }
            else if (prevList == null && newList == null)
            {
                ok = true;
            }

            Assert(ok, logger.ToStringTrimNewLine(), stepName);
        }

        
        private void CheckTrackListChanged_AllRecordings(GetRecordingsResponseItem[] oldRecordings, 
                                                                                                      GetRecordingsResponseItem[] newRecordings,
                                                                                                       string stepName)
        {
            GetRecordingsResponseItem recording = null;
            StringBuilder logger = new StringBuilder();
            bool ok = true;

            if (oldRecordings != null && newRecordings != null)
            {
                if (oldRecordings.Length != newRecordings.Length)
                {
                    ok = false;
                    logger.Append("Initial recording list was changed");
                    Assert(ok, logger.ToStringTrimNewLine(), stepName);
                }

                foreach (var item in newRecordings)
                {
                    recording = oldRecordings.FirstOrDefault(r => r.RecordingToken == item.RecordingToken);
                    if (recording == null)
                    {
                        ok = false;
                        logger.Append("Initial recording list was changed");
                    }
                    else
                    {
                        ok &= CompareRecordingTracks(item, recording, logger);
                    }
                }
            }
            else
            {
                if (oldRecordings != null && newRecordings == null)
                {
                    ok = false;
                    logger.AppendLine("Initial recording list was changed");
                }
                if (oldRecordings == null && newRecordings != null && newRecordings.Length > 0)
                {
                    ok = false;
                    logger.AppendLine("Initial recording list was changed");
                }

            }
            Assert(ok, logger.ToStringTrimNewLine(), stepName);
        }

        private bool CompareRecordingTracks(GetRecordingsResponseItem oldRecording, GetRecordingsResponseItem newRecording, StringBuilder logger)
        {
            bool ok = true;
            GetTracksResponseItem track = null;

            if (oldRecording.Tracks.Track == null && newRecording.Tracks.Track != null)
            {
                ok = false;
                logger.Append("Initial track list was changed");
                return ok;
            }
            else if (oldRecording.Tracks.Track != null && newRecording.Tracks.Track == null)
            {
                ok = false;
                logger.Append("Initial track list was changed");
                return ok;
            }
            else if (oldRecording.Tracks.Track == null && newRecording.Tracks.Track == null)
            {
                ok = true;
                return ok;
            }

            if (oldRecording.Tracks.Track.Length != newRecording.Tracks.Track.Length)
            {
                ok = false;
                logger.Append("Initial track list was changed");

                return ok;
            }

            foreach (var item in oldRecording.Tracks.Track)
            {
                track = newRecording.Tracks.Track.FirstOrDefault(t => t.TrackToken == item.TrackToken);
                if (track == null)
                {
                    ok = false;
                    logger.Append("Initial track list was changed");
                }
                else
                {
                    ok &= CompareTracks(item, track, logger, oldRecording.RecordingToken);
                }
            }
            return ok;
        }

        private bool CompareTracks(GetTracksResponseItem prevTrack, GetTracksResponseItem newTrack, StringBuilder logger)
        {
            bool ok = true;
            if (prevTrack.Configuration.TrackType != newTrack.Configuration.TrackType)
            {
                ok = false;
                logger.Append(string.Format("Track Types of track with token '{0}' are different{1}", prevTrack.TrackToken, Environment.NewLine));
            }
            if (prevTrack.Configuration.Description != newTrack.Configuration.Description)
            {
                ok = false;
                logger.Append(string.Format("Descriptions of track with token '{0}' are different{1}", prevTrack.TrackToken, Environment.NewLine));
            }
            return ok;
        }

        private void CheckRecordingListChanged(GetRecordingsResponseItem[] prevList, GetRecordingsResponseItem[] newList)
        {
            GetRecordingsResponseItem recording = null;
            StringBuilder logger = new StringBuilder();
            bool ok = true;

            if (prevList != null && newList != null)
            {
                if (prevList.Count() != newList.Count())
                {
                    ok = false;
                    logger.AppendLine(string.Format("The initial recording list contain different number of recordings"));
                }
                else
                    foreach (var item in newList)
                    {
                        recording = prevList.FirstOrDefault(r => r.RecordingToken == item.RecordingToken);
                        if (recording == null)
                        {
                            ok = false;
                            logger.AppendLine(string.Format("First recording list doesn't contain recording with token '{0}'", item.RecordingToken));
                        }
                        else
                        {
                            ok = ok && CompareRecordings(item, recording, logger);
                        }
                    }
            }
            else
            {
                if (prevList != null && newList == null)
                {
                    ok = false;
                    logger.AppendLine("New recording list doesn't contain any recordings, while previous is not empty");
                }
                if (prevList == null && newList != null && newList.Any())
                {
                    ok = false;
                    logger.AppendLine("New recording list is not empty");
                }

            }
            Assert(ok, logger.ToStringTrimNewLine(),
                   "Check that initial recording list wasn't changed");
        }

        private bool CompareRecordings(GetRecordingsResponseItem prevRecording, GetRecordingsResponseItem newRecording, StringBuilder logger)
        {
            bool ok = true;
            if (!CompareComplexStringFields<RecordingConfiguration>(prevRecording.Configuration, newRecording.Configuration, logger, newRecording.RecordingToken))
                ok = false;
            if (!CompareComplexStringFields<RecordingSourceInformation>(prevRecording.Configuration.Source, newRecording.Configuration.Source, logger, newRecording.RecordingToken))
                ok = false;
            //ok &= CompareComplexStringFields<RecordingConfiguration>(prevRecording.Configuration, newRecording.Configuration, logger, newRecording.RecordingToken);
            //ok &= CompareComplexStringFields<RecordingSourceInformation>(prevRecording.Configuration.Source, newRecording.Configuration.Source, logger, newRecording.RecordingToken);
            //GetTracksResponseItem track = null;
            //foreach (var item in prevRecording.Tracks.Track)
            //{
            //    track = newRecording.Tracks.Track.FirstOrDefault(t => t.TrackToken == item.TrackToken);
            //    if (track == null)
            //    {
            //        ok = false;
            //        logger.Append(string.Format("Recording '{0}' of new recording list doesn't contain track with token '{1}'{2}",
            //            newRecording.RecordingToken, item.TrackToken, Environment.NewLine));
            //    }
            //    else
            //    {
            //        ok &= CompareTracks(item, track, logger, prevRecording.RecordingToken);
            //    }
            //}
            return ok;
        }

        private bool CompareTracks(GetTracksResponseItem prevTrack, GetTracksResponseItem newTrack, StringBuilder logger, string recordingToken)
        {
            bool ok = true;
            //ok &= CompareStringFields(prevTrack.Configuration.Description,
            //    newTrack.Configuration.Description, logger, "Recording", string.Format("Track {0} Configuration.Description", prevTrack.TrackToken), recordingToken);
            if (!CompareStringFields(prevTrack.Configuration.Description,
                newTrack.Configuration.Description, logger, "Recording", string.Format("Track {0} Configuration.Description", prevTrack.TrackToken), recordingToken))
                ok = false;

            if (prevTrack.Configuration.TrackType != newTrack.Configuration.TrackType)
            {
                ok = false;
                logger.Append(string.Format("Track types of track '{0}' in recording '{1}' are different{2}",
                    prevTrack.TrackToken, recordingToken, Environment.NewLine));
            }
            return ok;
        }

        private bool CompareComplexStringFields<T>(T field1, T field2, StringBuilder logger, string token)
        {
            bool ok = true;
            foreach (var prop in field1.GetType().GetProperties().Where(p => p.PropertyType == typeof(string)))
            {
                ok &= CompareStringFields((string)prop.GetValue(field1, null),
                (string)prop.GetValue(field2, null), logger, "Recording", prop.Name, token);
            }
            return ok;
        }

        private bool CompareStringFields(string str1, string str2, StringBuilder logger, string obj, string fieldName, string token)
        {
            bool ok = str1 == str2;
            if (!ok)
                logger.Append(string.Format("{0} fields of {1} with token '{2}' are different{3}", 
                    fieldName, obj, token, Environment.NewLine));
            return ok;
        }

        private string ValidateRecordingJobConfiguration(RecordingJobConfiguration actual,
            RecordingJobConfiguration expected)
        {
            //ONVIF Client will invoke CreateRecordingJobRequest message 
            //(JobConfiguration.RecordingToken=RecordingToken1, JobConfiguration.Mode=Active, 
            //JobConfiguration.Priority=2, JobConfiguration.Source.SourceToken.Token=”ProfileToken1”, 
            //where ProfileToken1 is token of MediaProfile configured for recording, 
            //JobConfiguration.Source.SourceToken.Type=”http://www.onvif.org/ver10/schema/Profile”, 
            //JobConfiguration.Source.AutoCreateReceiver=false) to create a new recording job for 
            //configured recording with Active state and higher priority.
            string receiverToken = string.Empty;
            bool ok = true;
            StringBuilder logger = new StringBuilder();
            if (actual.RecordingToken != expected.RecordingToken)
            {
                ok = false;
                logger.Append(string.Format("Recording token is {0} but must be {1}{2}",
                actual.RecordingToken, expected.RecordingToken, Environment.NewLine));
            }
            if (actual.Mode != expected.Mode)
            {
                ok = false;
                logger.Append(string.Format("Mode is {0} but must be {1}{2}",
                actual.Mode, expected.Mode, Environment.NewLine));
            }
            if (actual.Priority != expected.Priority)
            {
                ok = false;
                logger.Append(string.Format("Priority is {0} but must be {1}{2}",
                actual.Priority, expected.Priority, Environment.NewLine));
            }
            if (actual.Source != null)
            {
                if (actual.Source.Length != expected.Source.Length)
                {
                    ok = false;
                    logger.Append(string.Format("RecordingJobSource list size is {0} but must be {1}{2}",
                    actual.Source.Length, expected.Source.Length, Environment.NewLine));
                }
                else
                {
                    if (actual.Source[0].AutoCreateReceiverSpecified == true && actual.Source[0].AutoCreateReceiver)
                    {
                        ok = false;
                        logger.Append(string.Format(
                            "AutoCreateReceiver is present in RecordingJobSource{0}",
                            Environment.NewLine));
                    }
                    else
                    {
                        if (actual.Source[0].SourceToken != null)
                        {
                            if (expected.Source[0].AutoCreateReceiverSpecified &&
                                expected.Source[0].AutoCreateReceiver)
                            {
                                receiverToken = actual.Source[0].SourceToken.Token;
                                if (string.IsNullOrEmpty(receiverToken))
                                {
                                    ok = false;
                                    logger.Append(string.Format(
                                        "Receiver token is not present in RecordingJobSource{0}",
                                        Environment.NewLine));
                                }

                                string receiverType = "http://www.onvif.org/ver10/schema/Receiver";
                                if (actual.Source[0].SourceToken.Type != receiverType)
                                {
                                    ok = false;
                                    logger.Append(string.Format(
                                        "RecordingJobSource SourceToken.Type is \"{0}\" but must be \"{1}\"{2}",
                                        actual.Source[0].SourceToken.Type, receiverType,
                                        Environment.NewLine));
                                }

                            }
                            else
                            {
                                if (actual.Source[0].SourceToken.Token != expected.Source[0].SourceToken.Token)
                                {
                                    ok = false;
                                    logger.Append(string.Format(
                                        "RecordingJobSource SourceToken.Token is {0} but must be {1}{2}",
                                        actual.Source[0].SourceToken.Token, expected.Source[0].SourceToken.Token,
                                        Environment.NewLine));
                                }
                                if (actual.Source[0].SourceToken.Type != expected.Source[0].SourceToken.Type)
                                {
                                    ok = false;
                                    logger.Append(string.Format(
                                        "RecordingJobSource SourceToken.Type is {0} but must be {1}{2}",
                                        actual.Source[0].SourceToken.Type, expected.Source[0].SourceToken.Type,
                                        Environment.NewLine));
                                }
                            }
                        }
                        else
                        {
                            ok = false;
                            logger.Append(string.Format("RecordingJobSource doesn't contain SourceToken{0}",
                            Environment.NewLine));
                        }
                    }
                }
            }
            else
            {
                ok = false;
                logger.Append(
                    string.Format("RecordingJobConfiguration doesn't contain RecordingJobSource{0}",
                        Environment.NewLine));
            }
            Assert(ok, logger.ToStringTrimNewLine(), "Validate recording job configuration");
            return receiverToken;
        }

        private static object CopyObject(object obj)
        {
            var copyObj = Activator.CreateInstance(obj.GetType());
            var type = obj.GetType();
            foreach (var item in obj.GetType().GetProperties())
            {
                if (!item.CanRead || !item.CanWrite)
                    continue;

                var itemVal = item.GetValue(obj, null);
                if (item.PropertyType.IsValueType || item.PropertyType == typeof(string))
                {
                    item.SetValue(copyObj, itemVal, null);
                }
                else if (item.PropertyType.IsClass && item.PropertyType != typeof(string) &&
                    !item.PropertyType.IsArray && itemVal != null)
                {
                    item.SetValue(copyObj, CopyObject(itemVal), null);
                }
                else if (item.PropertyType.IsArray && itemVal != null)
                {
                    var arraySource = (Array)itemVal;
                    var elementType = item.PropertyType.GetElementType();
                    var arrayCopy = Array.CreateInstance(elementType, arraySource.Length);

                    var isValueType = elementType.IsValueType;
                    var isClass = elementType.IsClass;

                    for (int itemIndex = 0; itemIndex < arraySource.Length; itemIndex++)
                    {
                        if (isValueType)
                        {
                            arrayCopy.SetValue(arraySource.GetValue(itemIndex), itemIndex);
                        }
                        else if (isClass)
                        {
                            arrayCopy.SetValue(CopyObject(arraySource.GetValue(itemIndex)), itemIndex);
                        }
                    }

                    item.SetValue(copyObj, arrayCopy, null);
                }
                else if (itemVal == null)
                {
                }
                else
                {
                    throw new Exception();
                }
            }

            return copyObj;
        }
    }
}