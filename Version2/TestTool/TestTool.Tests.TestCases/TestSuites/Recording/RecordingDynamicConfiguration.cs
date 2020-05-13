using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
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
        private const string DYNAMIC_PATH = "Recording Control\\Dynamic Recording";

        [Test(Name = "DYNAMIC RECORDINGS CONFIGURATION",
            Id = "3-1-10",
            Category = Category.RECORDING,
            Path = DYNAMIC_PATH,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.DynamicRecordings, Feature.GetServices },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateRecording })]
        public void DynamicRecordingConfigurationTest()
        {
            string newToken = string.Empty;
            bool isDeleted = false;
            RunTest(() =>
                    {
                        GetRecordingsResponseItem[] recordings = null;
                        GetRecordingsResponseItem[] recordingsRefreshed = null;
                        
                        CreateRecording(out recordings, out recordingsRefreshed, out newToken);
                        
                        CheckRecordingListChanged(recordings, recordingsRefreshed.Where(r => r.RecordingToken != newToken).ToArray());

                        DeleteRecording(newToken);
                        
                        recordingsRefreshed = GetRecordings();
                        
                        Assert(recordingsRefreshed == null || (recordingsRefreshed.FirstOrDefault(r => r.RecordingToken == newToken) == null),
                               "Recording wasn't deleted",
                               "Check that recording list doesn't contain recording deleted");
                        isDeleted = true;
                        CheckRecordingListChanged(recordings, recordingsRefreshed);
                    },
                    () =>
                    {
                        if (!string.IsNullOrEmpty(newToken) && !isDeleted)
                            DeleteRecording(newToken);
                    });
        }

        [Test(Name = "DYNAMIC TRACKS CONFIGURATION",
              Order = "03.01.07",
              Id = "3-1-7",
              Category = Category.RECORDING,
              Path = DYNAMIC_PATH,
              Version = 1.0,
              RequirementLevel = RequirementLevel.Must,
              RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.DynamicTracks, Feature.RecordingOptions },
              FunctionalityUnderTest = new Functionality[] { Functionality.CreateTrack, Functionality.DeleteTrack })
        ]
        public void DynamicTrackConfigurationTest()
        {
            string recordingToken = string.Empty;
            string trackTypeStr = string.Empty;

            string trackToken = string.Empty;

            bool isTrackDeleted = false;
            bool isTrackCreated = false;
            bool isRecordingCreated = false;

            TrackConfiguration deletedTrackConf = null;

            RunTest(() =>
            {
                // A.14 - Selection of Recording for track creation
                GetRecordingForTrackCreation(out recordingToken, 
                                                                         out trackTypeStr, 
                                                                         out isRecordingCreated, 
                                                                         out isTrackDeleted, out deletedTrackConf);

                TrackType trackType = (trackTypeStr == "Video" ? TrackType.Video :
                                      (trackTypeStr == "Audio" ? TrackType.Audio :
                                      (trackTypeStr == "Metadata" ? TrackType.Metadata : TrackType.Extended)));

                // prepare track configuration
                var trackConf = new TrackConfiguration();
                trackConf.Description = "New Track";
                trackConf.TrackType = trackType;
                
                // get initial recordings
                GetRecordingsResponseItem[] recordingsInitial = GetRecordings();
                Assert(recordingsInitial != null, "Recording list is empty", "Check that recording list is not empty");

                // create track
                trackToken = CreateTrack(recordingToken, trackConf);
                isTrackCreated = true;

                // check that created track token is not presented in selected recording (from initial track list)
                GetRecordingsResponseItem recordingInitial = recordingsInitial.FirstOrDefault(rec => rec.RecordingToken == recordingToken);
                GetTracksResponseItem track = null;
                if (recordingInitial.Tracks.Track != null)
                  track = recordingInitial.Tracks.Track.FirstOrDefault(t => t.TrackToken == trackToken);
                Assert(track == null,
                       String.Format("Track (token = {0}) has already presented in recording (token = {1})", trackToken, recordingToken),
                       String.Format("Check that new track (token = {0}) wasn't presented in recording (token = {1})", trackToken, recordingToken));

                // get updated recordings (after CreateTrack)
                GetRecordingsResponseItem[] recordingsUpdated = GetRecordings();
                Assert(recordingsUpdated != null, "Recording list is empty", "Check that recording list is not empty");

                // check that created track token is presented in selected recording (from updated track list)
                GetRecordingsResponseItem recordingUpdated = recordingsUpdated.FirstOrDefault(rec => rec.RecordingToken == recordingToken);
                Assert(recordingUpdated != null, "Recording is not found", 
                            string.Format("Check that recording (token = {0}) exists in updated recording list", recordingToken));

                GetTracksResponseItem track2 = null;
                if (recordingUpdated.Tracks.Track != null)
                    track2 = recordingUpdated.Tracks.Track.FirstOrDefault(t => t.TrackToken == trackToken);
                Assert(track2 != null,
                       String.Format("Track (token = {0}) isn't presented in recording (token = {1})", trackToken, recordingToken),
                       String.Format("Check that new track (token = {0}) is presented in recording (token = {1})", trackToken, recordingToken));

                // check track parameters
                CheckRecordingTrack(track2, trackConf);

                // check that all other tracks for initial selected recording 
                // have the same parameters as for updated selected recording
                GetTracksResponseItem[] trackInitialOther = null;
                if (recordingInitial.Tracks.Track != null)
                    trackInitialOther = (recordingInitial.Tracks.Track.Where(tr => tr.TrackToken != trackToken)).ToArray<GetTracksResponseItem>();

                GetTracksResponseItem[] trackUpdatedOther = null;
                if (recordingUpdated.Tracks.Track != null)
                    trackUpdatedOther = (recordingUpdated.Tracks.Track.Where(tr => tr.TrackToken != trackToken)).ToArray<GetTracksResponseItem>();

                if (trackInitialOther != null && trackInitialOther.Length == 0)
                    trackInitialOther = null;

                if (trackUpdatedOther != null && trackUpdatedOther.Length == 0)
                    trackUpdatedOther = null;

                CheckTrackListChanged(trackInitialOther, trackUpdatedOther, 
                                                            string.Format("Check that initial track list of recording (token = {0}) wasn't changed", recordingInitial.RecordingToken));

                // check that all other recordings have the same parameters value
                // as before creating a track
                GetRecordingsResponseItem[] recordingsInitialOther = null;
                if (recordingsInitial != null)
                    recordingsInitialOther = (recordingsInitial.Where(rec => rec.RecordingToken != recordingToken)).ToArray<GetRecordingsResponseItem>();

                GetRecordingsResponseItem[] recordingsUpdatedOther = null;
                if (recordingsUpdated != null)
                    recordingsUpdatedOther = (recordingsUpdated.Where(rec => rec.RecordingToken != recordingToken)).ToArray<GetRecordingsResponseItem>();

                if (recordingsInitialOther != null && recordingsInitialOther.Length == 0)
                    recordingsInitialOther = null;

                if (recordingsUpdatedOther != null && recordingsUpdatedOther.Length == 0)
                    recordingsUpdatedOther = null;

                 // check that after CreateTrack 
                // all tracks of all  other recordings are the same
                CheckTrackListChanged_AllRecordings(recordingsInitialOther, recordingsUpdatedOther,
                                                                                        "Check that initial track list of other existing recordings wasn't changed after CreateTrack");

                // delete track
                DeleteTrack(recordingToken, trackToken);
                isTrackCreated = false;

                // get updated recordings (after DeleteTrack)
                GetRecordingsResponseItem[] recordingsUpdated2 = GetRecordings();
                Assert(recordingsUpdated2 != null, "Recording list is empty", "Check that recording list is not empty");

                // check that deleted track token is no longer presented in selected recording (from updated track list after deletion)
                GetRecordingsResponseItem recordingUpdated2 = recordingsUpdated2.FirstOrDefault(rec => rec.RecordingToken == recordingToken);
                Assert(recordingUpdated2 != null, "Recording is not found",
                             string.Format("Check that recording (token = {0}) exists in updated recording list", recordingToken));

                GetTracksResponseItem track3 = null;
                if (recordingUpdated2.Tracks.Track != null)
                    track3 = recordingUpdated2.Tracks.Track.FirstOrDefault(t => t.TrackToken == trackToken);
                Assert(track3 == null,
                       String.Format("Track (token = {0}) is still presented in recording (token = {1})", trackToken, recordingToken),
                       String.Format("Check that deleted track (token = {0}) is no longer presented in recording (token = {1})", trackToken, recordingToken));

                // check that before CreateTrack all other tracks in selected recording 
                // have the same parameters as after DeleteTrack
                CheckTrackListChanged(recordingInitial.Tracks.Track, recordingUpdated2.Tracks.Track,
                                                              string.Format("Check that initial track list of recording (token = {0}) wasn't changed", recordingInitial.RecordingToken));

                // check that afteer CreateTrack + DeleteTrack
                // all tracks of all recordings are the same
                CheckTrackListChanged_AllRecordings(recordingsInitial, recordingsUpdated2, 
                                                                                        "Check that initial track list of existing recordings wasn't changed");

            }, () =>
            {
                if (isTrackCreated || isRecordingCreated || isTrackDeleted)
                    LogTestEvent(string.Format("Restoring the initial settings...{0}", Environment.NewLine));

                // reverting changes made during test
                if (isTrackCreated && !string.IsNullOrEmpty(trackToken))
                    DeleteTrack(recordingToken, trackToken);

                // reverting changes made during A.14
                if (isRecordingCreated && !string.IsNullOrEmpty(recordingToken))
                    DeleteRecording(recordingToken);

                // reverting changes made during A.14
                if (isTrackDeleted && !string.IsNullOrEmpty(recordingToken) && deletedTrackConf != null)
                    CreateTrack(recordingToken, deletedTrackConf);
            });
        }

        [Test(Name = "RECORDING JOB CONFIGURATION - DIFFERENT PRIORITIES (ON MEDIA PROFILE)",
            Id = "3-1-11",
            Category = Category.RECORDING,
            Path = DYNAMIC_PATH,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.MediaService, Feature.RecordingOptions },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateTrack, Functionality.CreateRecordingJob, Functionality.GetRecordingOptions })
        ]
        public void RecordingJobConfigurationDiffPriorTest2()
        {
            string recordingToken = string.Empty;
            string profileToken = string.Empty;
            string jobToken = string.Empty;
            string jobTokenSecond = string.Empty;

            bool recordingCreated = false;
            bool isRecordingJobDeleted = false;
            bool isRecordingJobSecondDeleted = false;

            RunTest(() =>
            {
                // A.15 - Selection or Creation of Recording for recording job creation on a Media profile
                GetRecordingForJobCreationMediaProfile(out recordingToken, out profileToken, out recordingCreated, 1);

                // pass test if there is no recording token and profile token
                if (recordingToken == string.Empty && profileToken == string.Empty)
                    return;

                // create 1st recording job configuration
                var source = new RecordingJobSource
                {
                    SourceToken = new SourceReference { Token = profileToken, Type = PROFILESOURCETYPE },
                    AutoCreateReceiverSpecified = false
                };

                var jobConfiguration = new RecordingJobConfiguration
                {
                    RecordingToken = recordingToken,
                    Mode = ACTIVE,
                    Priority = 1,
                    Source = new[] { source }
                };
                var confStand = (RecordingJobConfiguration)CopyObject(jobConfiguration);

                // create 1st recording job
                 jobToken = CreateRecordingJob(ref jobConfiguration);

                Assert(!string.IsNullOrEmpty(jobToken), 
                             "Job token wasn't returned",
                             "Check that job token was returned");

                ValidateRecordingJobConfiguration(jobConfiguration, confStand);

                // wait
                Sleep(_operationDelay);

                // get job state
                RecordingJobStateInformation info = GetRecordingJobState(jobToken);

                ValidateJobState(info, recordingToken, new [] { ACTIVE, PARTIALLY_ACTIVE });
                var initialJobStateForFirstToken = info.State;

                // create 1st recording job configuration
                var jobConfigurationSecond = confStand;
                jobConfigurationSecond.Priority = 2;
                confStand = (RecordingJobConfiguration)CopyObject(jobConfigurationSecond);

                // create 2nd recording job
                jobTokenSecond = CreateRecordingJob(ref jobConfigurationSecond);

                Assert(!string.IsNullOrEmpty(jobTokenSecond),
                                "Job token wasn't returned",
                                "Check that job token was returned");

                ValidateRecordingJobConfiguration(jobConfigurationSecond, confStand);

                // wait
                Sleep(_operationDelay);

                // get job state
                info = GetRecordingJobState(jobToken);
                ValidateJobState(info, recordingToken, IDLE);

                // get job state
                info = GetRecordingJobState(jobTokenSecond);
                ValidateJobState(info, recordingToken, new [] { ACTIVE, PARTIALLY_ACTIVE });

                // delete 2nd job
                DeleteRecordingJob(jobTokenSecond);
                isRecordingJobSecondDeleted = true;

                // wait
                Sleep(_operationDelay);

                // get job state
                info = GetRecordingJobState(jobToken);
                ValidateJobState(info, recordingToken, initialJobStateForFirstToken);

                // delete 1st job
                DeleteRecordingJob(jobToken);
                isRecordingJobDeleted = true;

            }, () =>
            {
                if (!isRecordingJobDeleted && !string.IsNullOrEmpty(jobToken))
                {
                    DeleteRecordingJob(jobToken);
                }

                if (!isRecordingJobSecondDeleted && !string.IsNullOrEmpty(jobTokenSecond))
                {
                    DeleteRecordingJob(jobTokenSecond);
                }

                if (recordingCreated && !string.IsNullOrEmpty(recordingToken)) 
                    DeleteRecording(recordingToken);
            });
        }

        [Test(Name = "RECORDING JOB CONFIGURATION - DIFFERENT PRIORITIES (ON RECEIVER)",
            Id = "3-1-12",
            Category = Category.RECORDING,
            Path = DYNAMIC_PATH,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.ReceiverService, Feature.RecordingOptions },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateTrack, Functionality.CreateRecordingJob, Functionality.GetRecordingOptions })
        ]
        public void RecordingJobConfigurationDiffPriorOnReceiverTest2()
        {
            RecordingConfiguration replacedConfiguration = null;
            string recordingToken = null;
            bool recordingCreated = false;
            string jobToken = null;
            string jobTokenSecond = null;

            bool isRecordingJobDeleted = false;
            bool isRecordingJobSecondDeleted = false;
            RunTest(() =>
            {
                // Annex A.12 - Selection or Creation of Recording for recording job creation
                GetRecordingForJobCreation(out recordingToken, out recordingCreated, 1);

                // pass test if there is no recording token
                if (recordingToken == string.Empty)
                    return;

                // Annex A.13 - Auto Creation of Receiver
                RecordingJobConfiguration jobConfiguration;
                AutoCreationReceiver(recordingToken, out jobToken, out jobConfiguration);

                SetRecordingJobMode(jobToken, ACTIVE);
                Sleep(_operationDelay);

                var info = GetRecordingJobState(jobToken);
                ValidateJobState(info, recordingToken, new [] { ACTIVE, PARTIALLY_ACTIVE });
                var initialJobStateForFirstToken = info.State;

                var jobConfigurationSecond = (RecordingJobConfiguration)CopyObject(jobConfiguration);

                jobConfigurationSecond.Priority = 2;
                jobConfigurationSecond.Mode = ACTIVE;
                var confStand = (RecordingJobConfiguration)CopyObject(jobConfigurationSecond);
                
                jobTokenSecond = CreateRecordingJob(ref jobConfigurationSecond);
                Assert(!string.IsNullOrEmpty(jobTokenSecond),
                            "Job token wasn't returned",
                            "Check that job token was returned");

                Sleep(_operationDelay);
                ValidateRecordingJobConfiguration(jobConfigurationSecond, confStand);

                info = GetRecordingJobState(jobToken);
                ValidateJobState(info, recordingToken, IDLE);

                info = GetRecordingJobState(jobTokenSecond);
                ValidateJobState(info, recordingToken, new [] { ACTIVE, PARTIALLY_ACTIVE });

                DeleteRecordingJob(jobTokenSecond);
                isRecordingJobSecondDeleted = true;
                Sleep(_operationDelay);

                info = GetRecordingJobState(jobToken);
                ValidateJobState(info, recordingToken, initialJobStateForFirstToken);

                DeleteRecordingJob(jobToken);
                isRecordingJobDeleted = true;
            },
            () =>
            {
                if (!isRecordingJobDeleted && !string.IsNullOrEmpty(jobToken))
                {
                    DeleteRecordingJob(jobToken);
                }
                if (!isRecordingJobSecondDeleted && !string.IsNullOrEmpty(jobTokenSecond))
                {
                    DeleteRecordingJob(jobTokenSecond);
                }

                if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                {
                    DeleteRecording(recordingToken);
                }
            });
        }

        private void StepFailed()
        {
            throw new NotImplementedException();
        }

    }
}
