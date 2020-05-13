using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.TestCases.TestSuites;
using TestTool.Tests.TestCases.Utils.Comparison;

using ArrayUtils = TestTool.Tests.TestCases.Utils.Comparison.ArrayUtils;

namespace TestTool.Tests.TestCases
{
    public partial class SearchRecordingsTestSuite
    {
        #region Search result validation


        protected void ValidateSearchResult(IEnumerable<RecordingInformation> recordings)
        {
            ValidateRecordingsListNotEmpty(recordings);

            ValidateFoundRecordingsTokens(recordings);

            ValidateFoundRecordingsInformation(recordings);
        }

        protected void ValidateRecordingsListNotEmpty(IEnumerable<RecordingInformation> recordings)
        {
            Assert(recordings.Count() > 0,
                "No recordings returned",
                "Check that recordings list is not empty");
        }

        protected void ValidateFoundRecordingsTokens(IEnumerable<RecordingInformation> recordings)
        {
            StringBuilder logger = new StringBuilder();
            bool tokensOk = ArrayUtils.ValidateTokens(recordings, RI => RI.RecordingToken, logger);

            string stepName = string.Empty;
            stepName = "Check that tokens in recordings list are different";

            Assert(tokensOk, logger.ToStringTrimNewLine(),
                   stepName);

        }

        protected void ValidateFoundRecordingsInformation(IEnumerable<RecordingInformation> recordings)
        {
            StringBuilder logger = new StringBuilder();
            bool ok = true; 

            foreach (RecordingInformation recording in recordings)
            {
                if (recording.Track == null || recording.Track.Length == 0)
                {
                    //ok = false;
                    //logger.AppendFormat("Tracks list is empty for recording with token '{0}' {1}",
                    //    recording.RecordingToken, Environment.NewLine);
                    continue;
                }

                if (recording.EarliestRecordingSpecified)
                {
                    long minTime = recording.Track.Min(T => T != null ? T.DataFrom.Ticks: long.MaxValue);
                    if (minTime < recording.EarliestRecording.Ticks)
                    {
                        // earlier data found;
                        ok = false;
                        logger.AppendFormat("Recording with token '{0}' contains track starting before EarliestRecording ({1}){2}", 
                            recording.RecordingToken, recording.EarliestRecording.StdDateTimeToString(), Environment.NewLine);
                    }
                    if (minTime > recording.EarliestRecording.Ticks)
                    {
                        // no data at startPoint
                        ok = false;

                        logger.AppendFormat("Recording with token '{0}' contains no track starting at EarliestRecording ({1}){2}",
                            recording.RecordingToken, recording.EarliestRecording.StdDateTimeToString(), Environment.NewLine);
                    }
                }

                if (recording.LatestRecordingSpecified)
                {
                    long maxTime = recording.Track.Max(T => T != null ? T.DataTo.Ticks : 0);
                    if (maxTime > recording.LatestRecording.Ticks)
                    {
                        // later data found;
                        ok = false;
                        logger.AppendFormat("Recording with token '{0}' contains track ended after LatestRecording ({1}){2}",
                            recording.RecordingToken, recording.LatestRecording.StdDateTimeToString(), Environment.NewLine);
                    }
                    if (maxTime < recording.LatestRecording.Ticks)
                    {
                        // no data at endPoint
                        ok = false;

                        logger.AppendFormat("Recording with token '{0}' contains no track ending at LatestRecording ({1}){2}",
                            recording.RecordingToken, recording.LatestRecording.StdDateTimeToString(), Environment.NewLine);
                    }
                }
            }

            Assert(ok, logger.ToStringTrimNewLine(),
                   "Check that Recording information and Tracks information are consistent");

        }


        #endregion

        #region All Recordings

        GetRecordingsResponseItem[] GetAllRecordings()
        {
            GetRecordingsResponseItem[] recordings = GetRecordings();
            Assert(recordings != null, "No recordings returned", "Check that recordings list is not empty");

            StorageTestsUtils.ValidateFullRecordingsList(recordings, Assert);
            return recordings;
        }


        #endregion

        protected void CheckInclusion(GetRecordingsResponseItem[] fullList, IEnumerable<RecordingInformation> list)
        {
            StringBuilder logger = new StringBuilder();
            bool ok = true;
            ok = ArrayUtils.CheckTokensInclusion(fullList, I => I.RecordingToken,
                                                 "full recordings list got via FindRecording",
                                                 list, RI => RI.RecordingToken, "Recording", logger);

            Assert(ok, logger.ToStringTrimNewLine(), "Check that all tokens from resulting list are present in full list");

        }

        protected bool CompareSourceInformation(RecordingSourceInformation source1,
            RecordingSourceInformation source2, string itemToken, StringBuilder logger)
        {
            return StorageTestsUtils.CompareSourceInformation(source1, source2, "GetRecording",
                                                              "FindRecordingSearchResult", itemToken, logger);
        }

        protected bool CompareTracks(
            GetTracksResponseList list,
            TrackInformation[] tracks,
            string itemToken,
            StringBuilder logger)
        {
            bool ok = true;

            //Validate:  list not null, list.Tracks not empty, tracks not empty
            if (list == null || list.Track == null || list.Track.Length == 0)
            {
                if (tracks == null || tracks.Length == 0)
                {
                    // skip validation for this lists
                    return true;
                }
                else
                {
                    logger.AppendFormat(
                        "List of tracks is empty when information is got via GetRecordings for recording with token '{0}'{1}",
                        itemToken, Environment.NewLine);
                    return false;
                }
            }

            if (tracks == null || tracks.Length == 0)
            {
                logger.AppendFormat(
                    "List of tracks is empty when information is got via GetRecordingSearchResults for recording with token '{0}'{1}",
                    itemToken, Environment.NewLine);
                return false;
            }

            if (tracks.Length != list.Track.Length)
            {
                logger.AppendFormat(
                    "Number of tracks is different for recording with token '{0}'{1}",
                    itemToken, Environment.NewLine);
                return false;
            }

            //
            // validate tokens
            //

            GetTracksResponseItem[] tracksList = list.Track;

            // check that tokens are unique
            StringBuilder sb = new StringBuilder();
            bool tokensOk = ArrayUtils.ValidateTokens(tracksList, TR => TR.TrackToken, sb);
            if (!tokensOk)
            {
                logger.AppendFormat("List of tracks is not valid when received from GetRecordings. {0}{1}", sb.ToStringTrimNewLine(), Environment.NewLine);
                ok = false;
            }

            sb = new StringBuilder();
            tokensOk = ArrayUtils.ValidateTokens(tracks, TR => TR.TrackToken, sb);
            if (!tokensOk)
            {
                logger.AppendFormat("List of tracks is not valid when received from GetRecordingSearchResults. {0}{1}", sb.ToStringTrimNewLine(), Environment.NewLine);
                ok = false;
            }

            if (!ok)
            {
                return false;
            }

            //
            // May be skip this comparison (and token validation)
            // When for each track in tracks find track with the same token
            //  if 0 - NOK
            //  if 2 and more - don't validate, NOK
            // Then for each track in second list find track in first list
            //  if 1 - already checked
            //  if 0 - NOK
            //  if 2 and more - NOK
            //

            ok = ArrayUtils.CompareTokensLists(tracks,
                T => T.TrackToken,
                string.Format("list got via GetRecordingSearchResults for recording with token '{0}' ", itemToken),
                tracksList, T => T.TrackToken,
                string.Format("list got via GetRecording for recording with token '{0}' ", itemToken),
                "Track",
                logger);

            if (!ok)
            {
                return false;
            }

            // 
            // fields to compare: Description, TrackType (find via TrackToken)
            //
            foreach (GetTracksResponseItem item in tracksList)
            {
                string trackToken = item.TrackToken;

                // if track token is not unique, don't perform check
                if (tracksList.Count( T => T.TrackToken == trackToken) > 1 )
                {
                    // error is added to log already
                    continue;
                }

                if (item.Configuration == null)
                {
                    ok = false;
                    logger.AppendFormat("Configuration is missing for tracks with token '{0}' (recording {1}). Skip validation for this item.{2}",
                        trackToken, itemToken, Environment.NewLine);

                    continue;
                }

                TrackInformation[] infos = tracks.Where(T => T.TrackToken == trackToken).ToArray();
                if (infos.Length != 1)
                {
                    // error is added to log already
                    continue;
                }
                TrackInformation info = infos[0];

                if (item.Configuration.Description != info.Description)
                {
                    ok = false;
                    logger.AppendFormat("Description is different for tracks with token '{0}' (recording '{1}'){2}",
                        trackToken, itemToken, Environment.NewLine);
                }

                if (item.Configuration.TrackType != info.TrackType)
                {
                    ok = false;
                    logger.AppendFormat("TrackType is different for tracks with token '{0}' (recording '{1}'){2}",
                        trackToken, itemToken, Environment.NewLine);
                }
            }
            
            return ok;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullList"></param>
        /// <param name="list"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        /// <remarks>When this method is called, it's assumed THAT
        /// a. Tokens are unique in fullList
        /// b. "Configuration" element is not null for all items in fullList
        /// c. For each recording in list received via search, token can be found in fullList 
        /// </remarks>
        protected bool CheckRecordings(GetRecordingsResponseItem[] fullList, IEnumerable<RecordingInformation> list, StringBuilder logger)
        {
            bool ok = true;

            // info - in search result
            // item - in full list
            foreach (RecordingInformation info in list)
            {
                // find entry in fullList;
                // compare

                string token = info.RecordingToken;
                GetRecordingsResponseItem item =
                    fullList.FirstOrDefault(RI => RI.RecordingToken == token);

                // Content [Description]
                if (info.Content != item.Configuration.Content)
                {
                    ok = false;
                    logger.AppendFormat(
                        "Content is different for item with token='{0}' ('{1}' for GetRecordings and '{2}' for GetRecordingSearchResults){3}",
                        token, item.Configuration.Content, info.Content, Environment.NewLine);
                }

                // TrackInformation 
                bool local = CompareTracks(item.Tracks, info.Track, item.RecordingToken, logger);
                ok = ok && local;

                // RecordingSourceInformation 
                local = CompareSourceInformation(item.Configuration.Source, info.Source, item.RecordingToken, logger);
                ok = ok && local;

            }

            return ok;
        }

        void CompareLists(IEnumerable<RecordingInformation> recordings1, IEnumerable<RecordingInformation> recordings2)
        {
            bool ok = true;
            StringBuilder logger = new StringBuilder();

            List<string> common = new List<string>();

            // check that all tokens from full list are present in list of found recordings
            foreach (RecordingInformation item in recordings1)
            {
                string token = item.RecordingToken;
                RecordingInformation[] foundItems =
                    recordings2.Where(RI => RI.RecordingToken == token).ToArray();

                if (foundItems.Length == 0)
                {
                    logger.AppendFormat(
                            "Recording with token {0} not found in second list{1}",
                            item.RecordingToken, Environment.NewLine);
                    ok = false;
                }
                else
                {
                    common.Add(token);
                }
            }
            
            foreach (RecordingInformation item in recordings2)
            {
                string token = item.RecordingToken;
                RecordingInformation[] foundItems =
                    recordings1.Where(RI => RI.RecordingToken == token).ToArray();

                if (foundItems.Length == 0)
                {
                    logger.AppendFormat(
                            "Recording with token {0} not found in first list{1}",
                            item.RecordingToken, Environment.NewLine);
                    ok = false;
                }
            }

            // for common only
            
            foreach (RecordingInformation info1 in recordings1)
            {
                string token = info1.RecordingToken;
                if (!common.Contains(token))
                {
                    continue;
                }

                RecordingInformation info2 = recordings2.Where(RI => RI.RecordingToken == token).FirstOrDefault();

                StringBuilder dump =
                    new StringBuilder(string.Format("Information for recording with token '{0}' is different:{1}", token,
                                                    Environment.NewLine));

                bool localOk = true;

                if (info1.Content != info2.Content)
                {
                    localOk = false;
                    dump.AppendLine("   Content is different");
                }
                if (info1.RecordingStatus != info2.RecordingStatus)
                {
                    localOk = false;
                    dump.AppendLine("   RecordingStatus is different");
                }

                Action<Func<RecordingInformation, bool>, Func<RecordingInformation, System.DateTime>, string> check =
                    new Action<Func<RecordingInformation, bool>, Func<RecordingInformation, System.DateTime>, string>(
                        (specifiedSelector, valueSelector, fieldName) =>
                            {
                                bool specified1 = specifiedSelector(info1);
                                bool specified2 = specifiedSelector(info2);

                                if (specified1 || specified2)
                                {
                                    if (specified1 && specified2)
                                    {
                                        System.DateTime value1 = valueSelector(info1);
                                        System.DateTime value2 = valueSelector(info2);

                                        if (value1 != value2)
                                        {
                                            localOk = false;
                                            dump.AppendFormat("   {0} is different{1}", fieldName, Environment.NewLine);
                                        }
                                    }
                                    else
                                    {
                                        if (!specified1)
                                        {
                                            localOk = false;
                                            dump.AppendFormat("   {0} not specified for the first list{1}", fieldName,
                                                              Environment.NewLine);
                                        }
                                        if (!specified2)
                                        {
                                            localOk = false;
                                            dump.AppendFormat("   {0} not specified for the second list{1}", fieldName,
                                                              Environment.NewLine);
                                        }
                                    }
                                }
                            });

                check(RI => RI.EarliestRecordingSpecified, RI => RI.EarliestRecording, "EarliestRecording");
                check(RI => RI.LatestRecordingSpecified, RI => RI.LatestRecording, "LatestRecording");

                RecordingSourceInformation source1 = info1.Source;
                RecordingSourceInformation source2 = info2.Source;

                if (source1 != null || source2 != null)
                {
                    if (source1 != null && source2 != null)
                    {
                        Action<string, Func<RecordingSourceInformation, string>> checkStringAction =
                            new Action<string, Func<RecordingSourceInformation, string>>(
                                (name, fieldSelector) =>
                                    {
                                        string value1 = fieldSelector(source1);
                                        string value2 = fieldSelector(source2);
                                        if (value1 != value2)
                                        {
                                            localOk = false;
                                            dump.AppendFormat("   Source.{0} field is different {1}", name, Environment.NewLine);
                                        }
                                    });

                        checkStringAction("Address", S => S.Address);
                        checkStringAction("Description", S => S.Description);
                        checkStringAction("Location", S => S.Location);
                        checkStringAction("Name", S => S.Name);
                        checkStringAction("SourceId", S => S.SourceId);

                    }
                    else
                    {
                        if (source1 == null)
                        {
                            dump.AppendLine("   RecordingSourceInformation is missing for the item from the first list{0}");
                            localOk = false;
                        }
                        if (source2 == null)
                        {
                            dump.AppendLine("   RecordingSourceInformation is missing for the item from the second list{0}");
                            localOk = false;
                        }
                    }
                }

                bool trackList1Ok = info1.Track != null && info1.Track.Length > 0;
                bool trackList2Ok = info2.Track != null && info2.Track.Length > 0;

                if (trackList1Ok || trackList2Ok)
                {
                    if (trackList1Ok && trackList2Ok)
                    {
                        // compare track by track

                        bool tracksOk = true;

                        TrackInformation[] tracks1 = info1.Track;
                        TrackInformation[] tracks2 = info2.Track;

                        StringBuilder sb = new StringBuilder();
                        bool tokensOk = ArrayUtils.ValidateTokens(tracks1, TR => TR.TrackToken, sb);
                        if (!tokensOk)
                        {
                            dump.AppendFormat("   List of tracks is not valid for item from the first list - not all checks will be performed{0}", Environment.NewLine);
                            tracksOk = false;
                        }

                        sb = new StringBuilder();
                        tokensOk = ArrayUtils.ValidateTokens(tracks2, TR => TR.TrackToken, sb);
                        if (!tokensOk)
                        {
                            dump.AppendFormat("   List of tracks is not valid for item from the second list - not all checks will be performed{0}", Environment.NewLine);
                            tracksOk = false;
                        }

                        {
                            List<string> commonTracks = new List<string>();
                            
                            // compare tokens
                            foreach (TrackInformation info in tracks1)
                            {
                                TrackInformation inf =
                                    tracks2.Where(T => T.TrackToken == info.TrackToken).FirstOrDefault();
                                if (inf == null)
                                {
                                    tracksOk = false;
                                    // not found
                                    dump.AppendFormat("   Track with token {0} not found for recording from second list{1}", info.TrackToken, Environment.NewLine);
                                }
                                else
                                {
                                    int cnt = tracks1.Where(T => T.TrackToken == info.TrackToken).Count();
                                    if (cnt == 1)
                                    {
                                        commonTracks.Add(info.TrackToken);
                                    }
                                }
                            }
                            foreach (TrackInformation info in tracks2)
                            {
                                TrackInformation inf =
                                    tracks1.Where(T => T.TrackToken == info.TrackToken).FirstOrDefault();
                                if (inf == null)
                                {
                                    tracksOk = false;
                                    // not found
                                    dump.AppendFormat("   Track with token {0} not found for recording from  first list{1}", info.TrackToken, Environment.NewLine);
                                }
                            }

                            {
                                // compare common
                                
                                foreach (TrackInformation trackInfo1 in tracks1)
                                {
                                    string trackToken = trackInfo1.TrackToken;
                                    
                                    if (!commonTracks.Contains(trackToken))
                                    {
                                        continue;
                                    }

                                    TrackInformation[] infos = tracks2.Where(T => T.TrackToken == trackToken).ToArray();
                                    if (infos.Length != 1)
                                    {
                                        // error is added to log already
                                        continue;
                                    }
                                    TrackInformation trackInfo2 = infos[0];
                                    
                                    // DataFrom, DataTo, Description, TrackType

                                    if (trackInfo1.Description != trackInfo2.Description)
                                    {
                                        tracksOk = false;
                                        dump.AppendFormat("   Description is different for tracks with token '{0}'{1}",
                                            trackToken, Environment.NewLine);
                                    }

                                    if (trackInfo1.TrackType != trackInfo2.TrackType)
                                    {
                                        tracksOk = false;
                                        dump.AppendFormat("   TrackType is different for tracks with token '{0}' {1}",
                                            trackToken, Environment.NewLine);
                                    }

                                    if (trackInfo1.DataFrom != trackInfo2.DataFrom)
                                    {
                                        tracksOk = false;
                                        dump.AppendFormat("   DataFrom is different for tracks with token '{0}' {1}",
                                            trackToken,  Environment.NewLine);
                                    }
                                    if (trackInfo1.DataTo != trackInfo2.DataTo)
                                    {
                                        tracksOk = false;
                                        dump.AppendFormat("   DataFrom is different for tracks with token '{0}' {1}",
                                            trackToken, Environment.NewLine);
                                    }
                                }
                            }
                        }

                        localOk = localOk && tracksOk;
                    }
                    else
                    {
                        if (!trackList1Ok)
                        {
                            dump.AppendFormat("   Track list is missing for the item from the first list{0}",
                                Environment.NewLine);
                            localOk = false;
                        }
                        if (!trackList2Ok)
                        {
                            dump.AppendFormat("   Track list is missing for the item from the first list{0}",
                                Environment.NewLine);
                            localOk = false;
                        }
                    }

                }

                if (!localOk)
                {
                    logger.Append(dump.ToString());
                    ok = false;
                }

            }
            
            Assert(ok, logger.ToStringTrimNewLine(), "Check that all recordings are returned");

        }
    }
}
