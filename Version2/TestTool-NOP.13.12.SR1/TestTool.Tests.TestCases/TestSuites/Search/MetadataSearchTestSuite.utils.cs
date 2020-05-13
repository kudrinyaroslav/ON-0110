using System;
using System.Collections.Generic;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Onvif;
using System.Linq;
using System.Text;

namespace TestTool.Tests.TestCases.TestSuites.Search
{
    public partial class MetadataSearchTestSuite : Base.SearchTest
    {

        private void MetadataSearchEndpoints(Func<RecordingInformation, SearchRange> defineRange, bool isInsideSearch)
        {
            int testKeepAlive = _searchKeepAlive;
            SearchState state = SearchState.Completed;
            var metadataFilter = _metadataFilter;
            string searchToken = string.Empty;
            string recordingToken = _recordingToken;
            System.DateTime T1 = System.DateTime.Now;
            System.DateTime T2 = System.DateTime.Now;
            SearchRange recordingRange = new SearchRange()
                {
                    Start = System.DateTime.Now,
                    End = System.DateTime.Now
                };

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", testKeepAlive);

                    var recordingInfo = GetRecording(recordingToken);
                    T1 = defineRange(recordingInfo).Start;
                    T2 = defineRange(recordingInfo).End;

                    if (isInsideSearch)
                    {
                        recordingRange.Start = T1;
                        recordingRange.End = T2;
                    }
                    else
                    {
                        recordingRange = DefineSearchRange(recordingInfo);
                    }

                    // Verify T1 and T2?
                    state = SearchState.Completed;
                    var findMetadataResponse = FindMetadata(T1, T2, recordingToken, null);
                    Assert(findMetadataResponse != null,
                        "FindMetadataResponse wasn't returned",
                            "Check that FindMetadataResponse was returned");
                    searchToken = findMetadataResponse.SearchToken;

                    state = SearchState.Queued;
                    var responseAcsending =
                        GetAllMetadataSearchResults(searchToken, null, null, "PT5S", out state);
                    VerifyMetadataSearchResult(responseAcsending, recordingInfo, recordingRange, true);

                    state = SearchState.Completed;
                    findMetadataResponse = FindMetadata(T2, T1, recordingToken, null);
                    Assert(findMetadataResponse != null,
                        "FindMetadataResponse wasn't returned",
                            "Check that FindMetadataResponse was returned");
                    searchToken = findMetadataResponse.SearchToken;

                    state = SearchState.Queued;
                    var responseDescending =
                        GetAllMetadataSearchResults(searchToken, null, null, "PT5S", out state);
                    VerifyMetadataSearchResult(responseDescending, recordingInfo, recordingRange, false);

                    CompareResponses(responseAcsending, responseDescending);
                },
               () =>
               {
                   if (state != SearchState.Completed)
                   {
                       ReleaseSearch(searchToken, testKeepAlive * 1000);
                   }
               });
        }

        private void VerifyMetadataSearchResult(List<FindMetadataResult> response,
            RecordingInformation recording, SearchRange recordingRange, bool ascendingOrder)
        {
            Assert(response != null,
                        "No metadata was found",
                           "Check that metadata list is not empty");

            Assert(response.FirstOrDefault(r => r.RecordingToken != recording.RecordingToken) == null,
                "GetMetadataSearchResultsResponse contains results with invalid Recording token",
                    "Check that recording token is valid for GetMetadataSearchResultsResponse");
            bool ok = true;
            StringBuilder logger = new StringBuilder("Following tracks are invalid" + Environment.NewLine);
            foreach (var result in response)
            {
                if (recording.Track.FirstOrDefault(t => t.TrackToken == result.TrackToken) == null)
                {
                    ok = false;
                    logger.Append(string.Format("   TrackToken '{0}' is invalid{1}", result.TrackToken, Environment.NewLine));
                }

                if (result.Time < recordingRange.Start)
                {
                    ok = false;
                    logger.Append(
                        string.Format("   Track (TrackToken = '{0}') time (t = '{1}') is outside recording range{2}",
                            result.TrackToken, result.Time, Environment.NewLine));
                }
            }
            Assert(ok, logger.ToStringTrimNewLine(),
                            "Check that tracks are valid for GetMetadataSearchResultsResponse");

            Func<System.DateTime, System.DateTime, bool> condition = (t1, t2) => { return t1 <= t2; };
            if (ascendingOrder)
                condition = (t1, t2) => { return t1 >= t2; };

            //bool ok = true;
            for (int i = 0; i < response.Count - 1; i++)
            {
                if (condition(response[i].Time, response[i + 1].Time))
                {
                    ok = false;
                    break;
                }
            }

            Assert(ok,
                "DUT didn't return metadata search results in right order",
                    "Check that DUT return metadata search results in right order");
        }

        private void CompareResponses(List<FindMetadataResult> responseAscending,
            List<FindMetadataResult> responseDescending)
        {
            Assert(responseAscending.Count == responseDescending.Count,
                "ascending and descending responses have different number of items",
                    "Check that DUT returned the same number of items in ascending and descending responses");

            bool ok = true;
            int count = responseAscending.Count - 1;

            for (int i = 0; i <= count; i++)
            {
                if (responseAscending[i].Time != responseDescending[count - i].Time ||
                    responseAscending[i].RecordingToken != responseDescending[count - i].RecordingToken ||
                    responseAscending[i].TrackToken != responseDescending[count - i].TrackToken)
                {
                    ok = false;
                    break;
                }
            }

            Assert(ok,
                "ascending and descending responses are different",
                    "Check that DUT returned the same sets in ascending and descending responses");
        }

        private RecordingInformation GetRecording(string recordingToken)
        {
            var recordingInfo = GetRecordingInformation(recordingToken);
            Assert(recordingInfo != null,
                "Recording information wasn't returned",
                    "Check that Recording Information was returned");

            Assert(recordingInfo.Track.Where(t => t.TrackType == TrackType.Metadata).FirstOrDefault() != null,
                    "Recording has no metadata",
                        "Check that Recording has metadata");
            return recordingInfo;
        }
    }
}