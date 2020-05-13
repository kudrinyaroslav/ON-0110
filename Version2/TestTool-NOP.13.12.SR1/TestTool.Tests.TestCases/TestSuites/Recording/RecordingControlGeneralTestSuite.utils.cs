using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.Utils.Comparison;

namespace TestTool.Tests.TestCases.TestSuites.Recording
{
    public partial class RecordingControlGeneralTestSuite : RecordingTest
    {
        public static void ValidateFullRecordingJobsList(GetRecordingJobsResponseItem[] jobs,
            GetRecordingsResponseItem[] recordings, AssertDelegate assert)
        {
            bool tokensOk = true;
            StringBuilder logger = new StringBuilder();

            List<string> logged = new List<string>();

            foreach (var item in jobs)
            {
                string token = item.JobConfiguration.RecordingToken;
                if (recordings.Where(r => r.RecordingToken == token).FirstOrDefault()==null)
                {
                    tokensOk = false;
                    logged.Add(token);
                    logger.AppendFormat("Recording list doesn't contain recording {0} which token returned in GetRecordingJobsResponse",
                        token, Environment.NewLine);
                }
            }

            assert(tokensOk, logger.ToStringTrimNewLine(),
                   "Validate recording job list got from GetRecordingJobs", null);
        }

        private void CompareJobStates(GetRecordingJobsResponseItem job,
            RecordingJobStateInformation jobState)
        {
            StringBuilder logger = new StringBuilder();
            bool ok = true;
            if (job.JobConfiguration.RecordingToken != jobState.RecordingToken)
            {
                ok = false;
                logger.Append(string.Format("   Recording tokens in JobResponse and in JobStateResponse are different{0}",
                    Environment.NewLine));
            }


            if (job.JobConfiguration.Source != null && jobState.Sources != null)
            {
                var jobStateSourceList = jobState.Sources;
                var jobSourceList = job.JobConfiguration.Source;
                foreach (var source in jobSourceList)
                {
                    var jobStateSource = jobStateSourceList.FirstOrDefault(
                        s => s.SourceToken.Token == source.SourceToken.Token && s.SourceToken.Type == source.SourceToken.Type);
                    if (jobStateSource == null)
                    {
                        ok = false;
                        logger.Append(string.Format(
                            "   Source list in GetRecordingJobStateResponse doesn't contain " +
                            "source with SourceToken='{0}' and Type='{1}'{2}",
                            source.SourceToken.Token, source.SourceToken.Type,
                            Environment.NewLine));             
                    }
                    else
                    {
                        if (source.Tracks==null && 
                            (jobStateSource.Tracks == null || jobStateSource.Tracks.Track == null))
                        {
                            if (jobStateSource.Tracks == null || jobStateSource.Tracks.Track == null)
                            {
                                ok = false;
                                logger.Append(string.Format(
                                        "   Recording job source '{0}' in GetRecordingJobStateResponse don't contain track list{1}",
                                        jobStateSource.SourceToken.Token, Environment.NewLine));
                            }
                            if (source.Tracks == null)
                            {
                                ok = false;
                                logger.Append(string.Format(
                                        "   Recording job source '{0}' in GetRecordingJobsResponse don't contain track list{1}",
                                        source.SourceToken.Token, Environment.NewLine));
                            }
                        }
                        else
                        {
                            var jobStateTrackList = jobStateSource.Tracks.Track;
                            var jobTrackList = source.Tracks;
                            foreach (var track in jobTrackList)
                            {
                                var jobStateTrack = jobStateTrackList.FirstOrDefault(
                                    t => t.SourceTag == track.SourceTag && t.Destination == track.Destination);
                                if (jobStateTrack == null)
                                {
                                    ok = false;
                                    logger.Append(string.Format(
                                        "   Source {0} in GetRecordingJobStateResponse doesn't "+
                                        "contain track with Tag='{1}' and Destination='{2}' which is "+
                                        "contained in GetRecordingJobsResponse{3}",
                                          source.SourceToken.Token, track.SourceTag, 
                                          track.Destination, Environment.NewLine));
                                }
                            }
                            foreach (var track in jobStateTrackList)
                            {
                                var jobTrack = jobTrackList.FirstOrDefault(
                                    t => t.SourceTag == track.SourceTag && t.Destination == track.Destination);
                                if (jobTrack == null)
                                {
                                    ok = false;
                                    logger.Append(string.Format(
                                        "   Source {0} in GetRecordingJobsResponse doesn't " +
                                        "contain track with Tag='{1}' and Destination='{2}' which is " +
                                        "contained in GetRecordingJobStateResponse{3}",
                                          source.SourceToken.Token, track.SourceTag,
                                          track.Destination, Environment.NewLine));
                                }
                            }
                        }
                    }
                }
                foreach (var source in jobStateSourceList)
                {
                    var jobSource = jobSourceList.FirstOrDefault(s =>
                        s.SourceToken.Token == source.SourceToken.Token && s.SourceToken.Type == source.SourceToken.Type);
                    if (jobSource == null)
                    {
                        ok = false;
                        logger.Append(string.Format(
                            "   Source list in GetRecordingJobsResponse doesn't contain "+
                            "source with SourceToken='{0}' and Type='{1}'{2}",
                            source.SourceToken.Token,source.SourceToken.Type,
                            Environment.NewLine));
                    }
                }
            }
            else
            {
                if (job.JobConfiguration.Source == null && jobState.Sources != null)
                {
                    ok = false;
                    logger.Append(string.Format("   Job with token = '{0}' in GetRecordingJobsResponse"+
                        " doesn't contain Source list{1}",job.JobToken, Environment.NewLine));
                }
                if (jobState.Sources == null && job.JobConfiguration.Source!= null)
                {
                    ok = false;
                    logger.Append(string.Format("   Job with token = '{0}' in GetRecordingJobStateResponse doesn't contain Source list{1}", job.JobToken, Environment.NewLine));
                }
            }
            Assert(ok, logger.ToStringTrimNewLine(), "Validate RecordingJobStateResponse");
        }

        void CompareConfigurations(RecordingConfiguration configuration1, RecordingConfiguration configuration2)
        {
            BeginStep("Compare Recording Configurations");
            StringBuilder dump = new StringBuilder("Configurations are different" + Environment.NewLine);
            bool equal = StorageTestsUtils.CompareConfigurations(configuration1, configuration2, dump, "GetRecordings", "GetRecordingConfiguration");

            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException("Configurations don't match");
            }

            StepPassed();
        }
    }
}