using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Exceptions;

namespace TestTool.Tests.TestCases.TestSuites.Search
{
    partial class PTZSearchTestSuite
    {

        void CommonForwardBackwardPtzPositionSearchTest(Func<RecordingInformation, SearchRange> defineRange)
        {
            string searchToken = string.Empty;
            SearchState state = SearchState.Completed;
            int testKeepAlive = _searchKeepAlive;

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", testKeepAlive);

                    RecordingInformation recording = FindRecordingForTest();

                    SearchRange range = defineRange(recording);

                    System.DateTime start = range.Start;
                    System.DateTime end = range.End;

                    PTZPositionFilter filter = new PTZPositionFilter();
                    filter.MinPosition = new PTZVector();
                    filter.MinPosition.PanTilt = new Vector2D() { x = -1, y = -1 };
                    filter.MaxPosition = new PTZVector();
                    filter.MaxPosition.PanTilt = new Vector2D() { x = 1, y = 1 };

                    SearchScope scope = new SearchScope();
                    scope.IncludedRecordings = new string[] { recording.RecordingToken };

                    List<FindPTZPositionResult> results = new List<FindPTZPositionResult>();

                    Action<System.DateTime, System.DateTime> validateAction =
                        new Action<System.DateTime, System.DateTime>(
                            (startTime, endTime) =>
                            {
                                state = SearchState.Completed;
                                searchToken = FindPTZPosition(startTime, endTime, scope, filter, null, keepAlive);

                                state = SearchState.Queued;
                                results = GetAllPtzSearchResults(searchToken, filter, null, null, "PT5S", out state);

                                Assert(results != null && results.Count > 0, "No PTZ positions found",
                                       "Check that PTZ positions list is not empty");

                                /// check that results belong to specified recording?
                                ValidatePTZPositionResults(results, recording, range.Earliest, range.Latest);

                                ValidateOrder(results, startTime, endTime);

                            });

                    validateAction(start, end);
                    List<FindPTZPositionResult> results1 = new List<FindPTZPositionResult>();
                    results1.AddRange(results);

                    validateAction(end, start);
                    List<FindPTZPositionResult> results2 = new List<FindPTZPositionResult>();
                    results2.AddRange(results);

                    // compare lists

                    CompareLists(results1, results2, "list received for StartPoint < EndPoint", "list received for StartPoint > EndPoint");

                },
                () =>
                {
                    if (state != SearchState.Completed)
                    {
                        ReleaseSearch(searchToken, testKeepAlive * 1000);
                    }
                });
        }



        protected void CompareLists(IEnumerable<FindPTZPositionResult> list1,
            IEnumerable<FindPTZPositionResult> list2,
            string descr1, string descr2)
        {
            bool ok = true;
            StringBuilder dump = new StringBuilder();

            Dictionary<FindPTZPositionResult, FindPTZPositionResult> intersection = new Dictionary<FindPTZPositionResult, FindPTZPositionResult>();

            Func<FindPTZPositionResult, string> getDescription =
                new Func<FindPTZPositionResult, string>(
                    (info) =>
                    {
                        return string.Format("Result with Time={0}, RecordingToken='{1}', TrackToken='{2}'", info.Time, info.RecordingToken, info.TrackToken);
                    });


            // check results in list1, find results that are not in list2, 
            // find results that are in both lists (intersection)

            foreach (FindPTZPositionResult result1 in list1)
            {
                string descr = getDescription(result1);
                
                FindPTZPositionResult result2 = list2.FirstOrDefault(R => AreTheSame(result1, R));

                if (result2 == null)
                {
                    ok = false;
                    dump.AppendFormat("{0} not found in {1}{2}",
                                      descr, descr2, Environment.NewLine);
                }
                else
                {
                    intersection.Add(result1, result2);
                }
            }

            // check results in list2, find results that are not in list1

            foreach (FindPTZPositionResult result2 in list2)
            {
                string descr = getDescription(result2);

                FindPTZPositionResult result1 = list1.FirstOrDefault(R => AreTheSame(result2, R));

                if (result1 == null)
                {
                    ok = false;
                    dump.AppendFormat("{0} not found in {1}{2}",
                        descr, descr1, Environment.NewLine);
                }
            }

            // compare results in intersection is impossible now.
            // It seems that there are no field which could be compared.
            // Time, RecordingToken, TrackToken, Position are keys...

            Assert(ok, dump.ToStringTrimNewLine(), "Check that PTZ positions lists are the same");


        }

        bool AreTheSame(FindPTZPositionResult result1, FindPTZPositionResult result2)
        {
            bool equal = result1.Time == result2.Time && 
                result1.RecordingToken == result2.RecordingToken && 
                result1.TrackToken == result2.TrackToken;

            if (equal)
            { 
                // ToDo : How to compare PTZ positions ?
                // (is it possible that they will be returned for different space ? ) 

                bool positionsEqual = true;

                if (result1.Position != null && result2.Position != null)
                {
                    Vector2D panTilt1 = result1.Position.PanTilt;
                    Vector2D panTilt2 = result2.Position.PanTilt;
                    if (panTilt1 != null && panTilt2 != null)
                    {
                        positionsEqual = panTilt1.space == panTilt2.space &&
                            panTilt1.x == panTilt2.x &&
                            panTilt1.y == panTilt2.y;
                    }
                    else 
                    {
                        if (panTilt1 != null || panTilt2 != null)
                        {
                            positionsEqual = false;
                        }
                    }

                    if (positionsEqual)
                    {
                        Vector1D zoom1 = result1.Position.Zoom;
                        Vector1D zoom2 = result2.Position.Zoom;
                        if (zoom1 != null && zoom2 != null)
                        {
                            positionsEqual = zoom1.space == zoom2.space &&
                                zoom1.x == zoom2.x;
                        }
                        else
                        {
                            if (zoom1 != null || zoom2 != null)
                            {
                                positionsEqual = false;
                            }
                        }
                    }
                }
                else 
                {
                    if (result1.Position != null || result2.Position != null)
                    {
                        positionsEqual = false;
                    }
                    // else both null - OK
                }

                equal = positionsEqual;
            }

            return equal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="recordingToken"></param>
        void ValidatePTZPositionResults(IEnumerable<FindPTZPositionResult> list, 
            RecordingInformation recording, 
            System.DateTime startTime, 
            System.DateTime endTime)
        {
            string recordingToken = recording.RecordingToken;

            StringBuilder sb = new StringBuilder();
            BeginStep("Validate search results");
            bool ok = true;

            bool asc = startTime < endTime;
            System.DateTime minTime = asc ? startTime : endTime;
            System.DateTime maxTime = asc ? endTime : startTime;

            foreach (FindPTZPositionResult result in list)
            {
                if (result.Time < minTime || result.Time > maxTime)
                {
                    ok = false;
                    sb.AppendFormat("Result for time {0} is out of expected interval ({1}; {2}){3}",
                        result.Time.StdDateTimeToString(),
                        minTime.StdDateTimeToString(),
                        maxTime.StdDateTimeToString(), 
                        Environment.NewLine);
                }                
                
                if (result.RecordingToken != recordingToken)
                {
                    ok = false;
                    sb.AppendFormat("Result for time {0} does not belong to recording {1}{2}", 
                        result.Time.StdDateTimeToString(), recordingToken, Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(result.TrackToken) && recording.Track != null)
                {
                    if (recording.Track.Where(T => T.TrackToken == result.TrackToken).FirstOrDefault() == null)
                    {
                        ok = false;
                        sb.AppendFormat("Track with token '{0}', specified in result for time {1} does not belong to recording {2}{3}", 
                            result.TrackToken, result.Time.StdDateTimeToString(), recordingToken, Environment.NewLine);
                    }                
                }
            }

            if (!ok)
            {
                throw new AssertException(sb.ToStringTrimNewLine());
            }
            StepPassed();
       
        }

        void ValidateOrder(IList<FindPTZPositionResult> results, System.DateTime start, System.DateTime end)
        {
            ValidateOrder(results, R => R.Time, start, end);
        }



    }
}
