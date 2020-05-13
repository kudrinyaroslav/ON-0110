using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.Utils.Comparison;
using TestTool.Tests.Common.TestBase;


namespace TestTool.Tests.TestCases.TestSuites.Recording
{
    [TestClass]
    public partial class RecordingControlGeneralTestSuite : RecordingTest
    {
        public RecordingControlGeneralTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH_GENERAL = "Recording Control\\General";

        [Test(Name = "GET RECORDINGS",
            Order = "04.01.01",
            Id = "4-1-1",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordings })]
        public void GetRecordingsTest()
        {
            RunTest(() =>
            {
                GetRecordingsResponseItem[] recordings = GetRecordings();

                // validation?
                Assert(recordings != null, "No recordings returned", "Check that recordings list is not empty");

                StorageTestsUtils.ValidateFullRecordingsList(recordings, Assert);

            });
        }

        [Test(Name = "GET RECORDING CONFIGURATION",
            Order = "04.01.02",
            Id = "4-1-2",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordingConfiguration })]
        public void RecordingConfigurationTest()
        {
            RunTest(() =>
            {
                GetRecordingsResponseItem[] recordings = GetRecordings();

                if (recordings != null)
                {
                    foreach (GetRecordingsResponseItem item in recordings)
                    {

                        Assert(item.Configuration != null,
                            "Configuration is missing in the structure with Recording information",
                                "Check that Configuration is present");

                        RecordingConfiguration configuration = GetRecordingConfiguration(item.RecordingToken);

                        Assert(configuration != null, "The DUT did not return Recording Configuration", "Check that the DUT returned Configuration");

                        CompareConfigurations(item.Configuration, configuration);
                    }
                }
            });
        }

        [Test(Name = "GET RECORDING CONFIGURATION WITH INVALID TOKEN",
            Order = "04.01.03",
            Id = "4-1-3",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordingConfiguration })]
        public void RecordingConfigurationInvalidTokenTest()
        {
            string token = null;
            RunTest(() =>
            {
                GetRecordingsResponseItem[] recordings = GetRecordings();

                if (recordings == null || recordings.Length == 0)
                {
                    RecordingConfiguration config = new RecordingConfiguration();
                    config.MaximumRetentionTime = _retentionTime;
                    config.Content = "Recording from device";
                    config.Source = new RecordingSourceInformation();
                    config.Source.SourceId = _cameraAddress.Trim();
                    config.Source.Name = "CameraName";
                    config.Source.Location = "LocationDescription";
                    config.Source.Description = "SourceDescription";
                    config.Source.Address = _cameraAddress.Trim();

                    token = CreateRecording(config);
                }

                this.InvalidTokenTestBody((s) => Client.GetRecordingConfiguration(s),
                    RunStep, "Recording Configuration", OnvifFaults.NoRecording);
            },
            () =>
            {
                if (token != null)
                {
                    DeleteRecording(token);
                }
            });
        }

        [Test(Name = "GET RECORDING JOBS",
            Order = "04.01.04",
            Id = "4-1-4",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordingJobs})]
        public void GetRecordingJobsTest()
        {
            RunTest(() =>
            {
                GetRecordingJobsResponseItem[] jobs = GetRecordingJobs();

                if (jobs != null)
                {
                    // validation?
                    this.ValidateTokensInList(jobs, j => j.JobToken, Assert);
                }

            });
        }

        [Test(Name = "GET RECORDING JOB CONFIGURATION",
            Order = "04.01.05",
            Id = "4-1-5",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordingJobConfiguration })]
        public void GetRecordingJobConfigurationTest()
        {
            RunTest(() =>
            {
                //3.	ONVIF Client will invoke GetRecordingsRequest message to retrieve complete recordings list.
                //4.	Verify the GetRecordingsResponse message from the DUT.
                GetRecordingsResponseItem[] recordings = GetRecordings();
                if (recordings != null && recordings.Length > 0)
                {
                    List<string> recordingTokens = recordings.Select(R => R.RecordingToken).ToList();

                    //5.	ONVIF Client will invoke GetRecordingJobsRequest message to retrieve complete 
                    // recording jobs list.
                    //6.	Verify the GetRecordingJobsResponse message from the DUT.
                    GetRecordingJobsResponseItem[] jobs = GetRecordingJobs();

                    if (jobs != null)
                    {
                        foreach (GetRecordingJobsResponseItem job in jobs)
                        {
                            Assert(job.JobConfiguration != null,
                                   "Configuation is missing when information is received via GetRecordings",
                                   "Check that configuration is not missing");

                            Assert(recordingTokens.Contains(job.JobConfiguration.RecordingToken),
                                string.Format("Recording with token '{0}' does not exist", job.JobConfiguration.RecordingToken),
                                "Check that configuration is valid");


                            //7.	ONVIF Client will invoke GetRecordingJobConfigurationRequest message (JobToken = “Token1”, where Token1 is the first JobItem.JobToken from the GetRecordingJobsResponse message) to retrieve recording job configuration.
                            //8.	Verify the GetRecordingJobConfigurationResponse message from the DUT.
                            RecordingJobConfiguration configuration = GetRecordingJobConfiguration(job.JobToken);

                            Assert(configuration != null, "The DUT did not returned configuration",
                                   "Check that the DUT returned requested information");

                            CompareConfigurations(job.JobConfiguration, configuration, "GetRecordingJobs", "GetRecordingJobConfiguration");

                            //9.	Repeat steps 7-8 for all other recording jobs from the GetRecordingJobsResponse message.
                        }
                    }
                }
            });
        }

        // 4-1-6 moved to RecordongControlRecording, as it needs profile selection

        [Test(Name = "GET RECORDING JOB STATE",
            Order = "04.01.07",
            Id = "4-1-7",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordingJobState })]
        public void GetRecordingJobStateTest()
        {
            RunTest(() =>
            {
                GetRecordingsResponseItem[] recordings = GetRecordings();
                // validation?
                //Assert(recordings != null,
                //    "No recordings returned",
                //        "Check that recordings list is not empty");

                BeginStep("Check recording list");
                if (recordings == null || recordings.Length == 0)
                {
                    LogStepEvent("Recording list is empty");
                    StepPassed();
                    return;
                }
                else
                {
                    StepPassed();
                }

                StorageTestsUtils.ValidateFullRecordingsList(recordings, Assert);

                GetRecordingJobsResponseItem[] jobs = GetRecordingJobs();

                BeginStep("Check recording job list");
                if (jobs == null || jobs.Length == 0)
                {
                    LogStepEvent("RecordingJob list is empty");
                    StepPassed();
                    return;
                }
                else
                {
                    StepPassed();
                }

                ValidateFullRecordingJobsList(jobs, recordings, Assert);
                foreach (var job in jobs)
                {
                    string token = job.JobToken;
                    var jobState = GetRecordingJobState(job.JobToken);
                    Assert(jobState != null,
                        string.Format("Recording job state with jobtoken {0} wasn't returned", token),
                            "Check that recording jobs list is not empty");
                    CompareJobStates(job, jobState);
                }
            });
        }

        // 4-1-8 moved to RecordongControlRecording, as it needs profile selection

        [Test(Name = "GET TRACK CONFIGURATION",
            Order = "04.01.09",
            Id = "4-1-9",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetTrackConfiguration })]
        public void GetTrackConfigurationTest()
        {
            RunTest(() =>
            {
                GetRecordingsResponseItem[] recordings = GetRecordings();
                BeginStep("Check that recording list is not empty");
                if(recordings == null||recordings.Length==0)
                {
                    LogStepEvent("Recording list is empty");
                    StepPassed();
                    return;
                }
                else
                {
                    StepPassed();
                }

                StorageTestsUtils.ValidateFullRecordingsList(recordings, Assert);

                string recordingToken = string.Empty;
                string trackToken = string.Empty;
                foreach (var recording in recordings)
                {
                    recordingToken = recording.RecordingToken;
                    BeginStep(string.Format("Check that recording '{0}' has tracks", recording.RecordingToken));
                    if(recording.Tracks == null || recording.Tracks.Track == null || recording.Tracks.Track.Length == 0)
                    {
                        LogStepEvent("Track list is empty");
                        StepPassed();
                        continue;
                    }
                    else
                    {
                        StepPassed();
                    }
                    foreach (var track in recording.Tracks.Track)
                    {
                        trackToken = track.TrackToken;

                        var trackConfig = GetTrackConfiguration(recording.RecordingToken, track.TrackToken);
                        Assert(trackConfig != null,
                            string.Format("Track configuration (recording token = '{0}', track token = '{1}')", recordingToken, trackToken),
                                "Check that track configuration was returned");

                        CompareTrackConfigurations(track.Configuration, trackConfig);
                    }
                }
            });
        }

        [Test(Name = "GET TRACK CONFIGURATION WITH INVALID TOKEN",
            Order = "04.01.10",
            Id = "4-1-10",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 1.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetTrackConfiguration})]
        public void GetTrackConfigurationInvalidTokenTest()
        {
            string recordingToken = string.Empty;
            bool isCreated = false;
            RunTest(() =>
            {
                
                var recordings = GetRecordings();
                string trackToken = string.Empty;
                
                if (recordings == null || recordings.Length == 0)
                {
                    RecordingConfiguration config = new RecordingConfiguration();
                    config.MaximumRetentionTime = "PT0S";
                    config.Content = "Recording from device";
                    config.Source = new RecordingSourceInformation();
                    config.Source.SourceId = _cameraAddress.Trim();
                    config.Source.Name = "CameraName";
                    config.Source.Location = "LocationDescription";
                    config.Source.Description = "SourceDescription";
                    config.Source.Address = _cameraAddress.Trim();
                    try
                    {
                        recordingToken = CreateRecording(config);
                    }
                    catch (FaultException ex)
                    {
                        LogStepEvent(ex.Message);
                        StepPassed();
                        return;
                    }
                    
                    Assert(!string.IsNullOrEmpty(recordingToken),
                        "Recording token hasn't been returned",
                            "Check that recording token has been returned");
                    isCreated = true;
                    recordings = GetRecordings();
                    Assert(recordings!=null,
                        "Recording list is empty",
                            "Check that recording list is not empty");
                    var recording = recordings.FirstOrDefault(r=>r.RecordingToken==recordingToken);
                    Assert(recording!=null,
                        "Recording is absent",
                            string.Format("Check that recording '{0}' is present in recording list", recordingToken));
                    Assert(recording.Tracks!=null && recording.Tracks.Track!=null && recording.Tracks.Track.Length>0,
                        "Track list is empty",
                            string.Format("Check that track list of recording '{0}' is not empty", recordingToken));
                    trackToken = recording.Tracks.Track[0].TrackToken;
                }
                else
                {
                    recordingToken = recordings[0].RecordingToken;
                    trackToken = recordings[0].Tracks.Track[0].TrackToken;
                }
                
                this.InvalidTokenTestBody<string>((s, T) => Client.GetTrackConfiguration(T, s), recordingToken,
                    RunStep, "Get Track Configuration", null, OnvifFaults.NoTrack);

                this.InvalidTokenTestBody<string>((s, T) => Client.GetTrackConfiguration(s, T), trackToken,
                    RunStep, "Get Track Configuration", null, OnvifFaults.NoRecording);
            }, ()=>
                {
                    if(isCreated)
                        DeleteRecording(recordingToken);
                });
        }

        private void CompareTrackConfigurations(TrackConfiguration recordingTrackConfig, TrackConfiguration trackConfig)
        {
            StringBuilder logger = new StringBuilder();
            bool ok = true;
            if (recordingTrackConfig.Description != trackConfig.Description)
            {
                ok = false;
                logger.Append(string.Format("Description are different", Environment.NewLine));
            }
            if (recordingTrackConfig.TrackType != trackConfig.TrackType)
            {
                ok = false;
                logger.Append(string.Format("Track types are different", Environment.NewLine));
            }
            Assert(ok, logger.ToStringTrimNewLine(), "Verify track configuration");
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration1">Configuration from GetRecordingJobs</param>
        /// <param name="configuration2">Configuration from GetRecordingJobConfiguration</param>
        void CompareConfigurations(RecordingJobConfiguration configuration1,
            RecordingJobConfiguration configuration2, string descr1, string descr2)
        {
            BeginStep("Compare Recording Job Configurations");
            StringBuilder dump = new StringBuilder("Configurations are different" + Environment.NewLine);
            bool equal = true;
            bool local;

            if (configuration1.Mode != configuration2.Mode)
            {
                equal = false;
                dump.AppendFormat("   Mode field is different{0}", Environment.NewLine);
            }

            if (configuration1.Priority != configuration2.Priority)
            {
                equal = false;
                dump.AppendFormat("   Priority field is different{0}", Environment.NewLine);
            }

            if (configuration1.RecordingToken != configuration2.RecordingToken)
            {
                equal = false;
                dump.AppendFormat("   RecordingToken field is different{0}", Environment.NewLine);
            }

            // Compare Source 

            local = true;
            RecordingJobSource[] source1 = configuration1.Source;
            RecordingJobSource[] source2 = configuration2.Source;

            if (source1 != null && source2 != null)
            {
                // get intersection;
                // compare items in intersection

                Dictionary<RecordingJobSource, RecordingJobSource> common = new Dictionary<RecordingJobSource, RecordingJobSource>();
                bool ok;

                ok = CompareRecordingJobSourceLists(source1, source2,
                    common, descr1, descr2, dump);

                // for common only
                bool ok1 = CompareJobSources(common, dump);

                local = ok && ok1;
            }
            else
            {
                string messageFormat = "   Source information is skipped when information is received via {0}" +
                                       Environment.NewLine;
                if (source1 == null && source2 != null)
                {
                    local = false;
                    dump.AppendFormat(messageFormat, descr1);
                }
                if (source2 == null && source1 != null)
                {
                    local = false;
                    dump.AppendFormat(messageFormat, descr2);
                }
                // both null is OK
            }

            equal = equal && local;

            // There is also Extensio field.
            // Extension contains only Any...

            // Dump total result 

            if (!equal)
            {
                throw new AssertException(dump.ToStringTrimNewLine());
            }

            StepPassed();

        }

        /// <summary>
        /// Checks that for all items in first list item with the same token is presented in 
        /// second list and vice versa.
        /// </summary>
        /// <param name="list1">First list</param>
        /// <param name="list2">Second list</param>
        /// <param name="common">List of common tokens (actually out parameters)</param>
        /// <param name="description1">Description of the first list</param>
        /// <param name="description2">Description of the second list</param>
        /// <param name="logger">Logger to append error description, if needed.</param>
        /// <returns>True if set of tokens is the same in both lists.</returns>
        bool CompareRecordingJobSourceLists(IEnumerable<RecordingJobSource> list1,
            IEnumerable<RecordingJobSource> list2,
            Dictionary<RecordingJobSource, RecordingJobSource> common,
            string description1,
            string description2,
            StringBuilder logger)
        {
            bool ok = true;

            foreach (RecordingJobSource info in list1)
            {
                if (info.SourceToken != null)
                {
                    string token = info.SourceToken.Token;
                    string type = info.SourceToken.Type;

                    RecordingJobSource[] foundItems =
                        list2.Where(
                            I =>
                            I.SourceToken != null &&
                            I.SourceToken.Token == token &&
                            I.SourceToken.Type == type).ToArray();

                    if (foundItems.Length == 0)
                    {
                        logger.AppendFormat(
                            "      RecordingJobSource with SourceToken '{0}', Type = '{1}' not found in list received from{2}{3}",
                            token, type, description2, Environment.NewLine);
                        ok = false;
                    }
                    else
                    {
                        common.Add(info, foundItems[0]);
                    }
                }
            }

            foreach (RecordingJobSource info in list2)
            {
                if (info.SourceToken != null)
                {
                    string token = info.SourceToken.Token;
                    string type = info.SourceToken.Type;

                    RecordingJobSource[] foundItems =
                        list1.Where(
                            I =>
                            I.SourceToken != null &&
                            I.SourceToken.Token == token &&
                            I.SourceToken.Type == type).ToArray();

                    if (foundItems.Length == 0)
                    {
                        logger.AppendFormat(
                            "      RecordingJobSource with SourceToken '{0}', Type = '{1}' not found in list received from{2}{3}",
                            token, type, description1, Environment.NewLine);
                        ok = false;
                    }
                }
            }


            return ok;
        }

        bool CompareJobSources(Dictionary<RecordingJobSource, RecordingJobSource> common,
            StringBuilder logger)
        {
            bool ok = true;

            foreach (RecordingJobSource info1 in common.Keys)
            {
                string token = info1.SourceToken.Token;
                string type = info1.SourceToken.Type;
                RecordingJobSource info2 = common[info1];

                StringBuilder dump =
                    new StringBuilder(string.Format("      Information for RecordingJobSource with token '{0}' (type '{1}') is different:{2}",
                        token, type, Environment.NewLine));

                bool localOk = CompareJobSourceInformation(info1, info2, dump);

                if (!localOk)
                {
                    logger.Append(dump.ToString());
                    ok = false;
                }
            }

            return ok;
        }


        bool CompareJobSourceInformation(RecordingJobSource source1,
            RecordingJobSource source2,
            StringBuilder logger)
        {
            bool ok = true;
            StringBuilder dump = new StringBuilder();

            // AutoCreateReceiver is not specified, if information is valid.

            // Extension - SKIP ?

            // Token - as ID ?


            // Tracks...

            bool list1empty = source1.Tracks == null || source1.Tracks.Length == 0;
            bool list2empty = source2.Tracks == null || source2.Tracks.Length == 0;

            if (!list1empty && !list2empty)
            {

                List<string> notUnique1 = new List<string>();
                List<string> notUnique2 = new List<string>();

                foreach (RecordingJobTrack track in source1.Tracks)
                {
                    int cnt1 = source1.Tracks.Count(t => t.SourceTag == track.SourceTag);
                    if (cnt1 > 1)
                    {
                        notUnique1.Add(track.SourceTag);
                    }
                }
                foreach (RecordingJobTrack track in source2.Tracks)
                {
                    int cnt2 = source2.Tracks.Count(t => t.SourceTag == track.SourceTag);
                    if (cnt2 > 1)
                    {
                        notUnique2.Add(track.SourceTag);
                    }
                }

                if (notUnique1.Count > 0)
                {
                    dump.AppendFormat("         Tracks list is invalid when information is received from GetRecordingJobs. The following SourceTags are not unique: {0}{1}",
                                      string.Join(",", notUnique1.ToArray()), Environment.NewLine);

                    ok = false;
                }
                if (notUnique2.Count > 0)
                {
                    dump.AppendFormat("         Tracks list is invalid when information is received from GetRecordingJobConfiguration. The following SourceTags are not unique: {0}{1}",
                                      string.Join(",", notUnique2.ToArray()), Environment.NewLine);

                    ok = false;
                }

                if (notUnique1.Count == 0 && notUnique2.Count == 0)
                {
                    List<string> commonTags = new List<string>();

                    foreach (RecordingJobTrack track in source1.Tracks)
                    {
                        RecordingJobTrack second =
                            source2.Tracks.Where(t => t.SourceTag == track.SourceTag && t.Destination == track.Destination).
                            FirstOrDefault();
                        if (second == null)
                        {
                            dump.AppendFormat(
                                string.Format(
                                    "         Track with SourceTag = '{0}', Destination = '{1}' not found in tracks list in structure received from GetRecordingJobConfiguration{2}",
                                    track.SourceTag, track.Destination, Environment.NewLine));
                            ok = false;
                        }
                    }
                    foreach (RecordingJobTrack track in source2.Tracks)
                    {
                        int cnt = source1.Tracks.Count(t => t.SourceTag == track.SourceTag && t.Destination == track.Destination);
                        if (cnt == 0)
                        {
                            dump.AppendFormat(
                                string.Format(
                                    "         Track with SourceTag = '{0}', Destination = '{1}' not found in tracks list in structure received from GetRecordingJobs{2}",
                                    track.SourceTag, track.Destination, Environment.NewLine));
                            ok = false;
                        }
                    }
                }
            }
            else
            {
                if (!(list1empty && list2empty))
                {
                    dump.AppendLine("         Tracks list is present only in one structure");
                    ok = false;
                }
            }


            if (!ok)
            {
                logger.Append(dump.ToString());
            }

            return ok;
        }


        [Test(Name = "SET RECORDINGS CONFIGURATION (MAXIMUM LENGTH OF RECORDING SOURCE INFORMATION)",
            Order = "04.01.11",
            Id = "4-1-11",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 2.3,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordingConfiguration, Functionality.SetRecordingConfiguration })
        ]
        public void SetRecordingsConfigTest()
        {
            RecordingConfiguration oldConfig = null;
            RecordingConfiguration newConfig = null;
            String recordingToken = null;

            RunTest(() =>
            {
                GetRecordingsResponseItem[] recordings = GetRecordings();

                // Recording list validation 
                Assert(recordings != null, "No recordings returned", "Check that recordings list is not empty");
                StorageTestsUtils.ValidateFullRecordingsList(recordings, Assert);

                // Saving of old recording config
                oldConfig = recordings[0].Configuration;

                recordingToken = recordings[0].RecordingToken;
                newConfig = new RecordingConfiguration();
                newConfig.Source = new RecordingSourceInformation();

                // A device shall support at least 20 characters.
                newConfig.Source.Name = "SourceNameForTest001";
                // A device shall support at least 128 characters.
                newConfig.Source.SourceId = "http://source-id-for-test.com/identifier/for/the/source/chosen/by/the/client/that/creates/the/structure/0001/0002/0003/0004/005";
                // A device shall support at least 128 characters.
                newConfig.Source.Address = "http://source-address-for-test.com/uri/provided/by/the/service/supplying/data/to/be/recorded/0001/0002/0003/0004/0005/0006/0007";

                // Other values without changing
                newConfig.Source.Location = oldConfig.Source.Location;
                newConfig.Source.Description = oldConfig.Source.Description;
                newConfig.Content = oldConfig.Content;
                newConfig.MaximumRetentionTime = oldConfig.MaximumRetentionTime;

                // Setting of recording config
                SetRecordingConfiguration(recordingToken, newConfig);

                RecordingConfiguration actualConfig = GetRecordingConfiguration(recordingToken);

                // Validation of the result 
                Assert(new CheckCondition(() =>
                {
                    if (actualConfig.Source.Name == newConfig.Source.Name &&
                        actualConfig.Source.SourceId == newConfig.Source.SourceId &&
                        actualConfig.Source.Address == newConfig.Source.Address)
                        return true;
                    else
                        return false;
                }),
                "Settings were not applied", "Validation of applied settings");

            },
            () =>
            {
                if (newConfig != null)
                {
                    LogTestEvent(string.Format("Restoring the initial settings...{0}", Environment.NewLine));

                    SetRecordingConfiguration(recordingToken, oldConfig, "Restore previous settings");
                }
            });
        }

        protected void DeleteRecordingIfNecessary(GetRecordingsResponseItem[] recordings, 
                                                  out String recordingTokenDeleted, 
                                                  out RecordingConfiguration oldConfig)
        {
            bool isDeleted = false;
            oldConfig = null;
            recordingTokenDeleted = null;

            foreach (GetRecordingsResponseItem recording in recordings)
            {
                try
                {
                    // Saving of old config to restore it later
                    oldConfig = GetRecordingConfiguration(recording.RecordingToken);

                    DeleteRecording(recording.RecordingToken);
                    isDeleted = true;
                    recordingTokenDeleted = recording.RecordingToken;
                    break;
                }
                catch (FaultException exc)
                {
                    if (exc.IsValidOnvifFault("Receiver/Action/CannotDelete"))
                    {
                        LogStepEvent(string.Format("Can't delete recording (token = {0})", recording.RecordingToken));
                        StepPassed();
                    }
                    else
                    {
                        StepFailed(exc);
                    }
                }
            }

            // In case if deleting of every recording caused "Receiver/Action/CannotDelete" fault
            Assert(isDeleted == true, "No one recording was deleted", "Checking if some recording was deleted");
            
            // Checking if recording was deleted in reality
            if (isDeleted == true && recordingTokenDeleted != null)
            {
                GetRecordingsResponseItem[] recordingsRefreshed = GetRecordings();

                String recordingTokenDeletedCopy = recordingTokenDeleted;

                GetRecordingsResponseItem recording =
                    recordingsRefreshed.FirstOrDefault(r => r.RecordingToken == recordingTokenDeletedCopy);

                Assert(recording == null, string.Format("Recording (token = {0}) was not deleted in reality", recordingTokenDeleted),
                                          "Validation of deleted recording");
            }
        }

        [Test(Name = "DYNAMIC RECORDINGS CONFIGURATION (MAXIMUM LENGTH OF RECORDING SOURCE INFORMATION)",
            Order = "04.01.12",
            Id = "4-1-12",
            Category = Category.RECORDING,
            Path = PATH_GENERAL,
            Version = 2.3,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.RecordingControlService, Feature.DynamicRecordings },
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateRecording, Functionality.GetRecordingConfiguration })
        ]
        public void DynamicRecordingsConfigTest()
        {
            // token of deleted recording
            String recordingTokenDeleted = null;

            // token of created recording
            String recordingTokenCreated = null;
            bool recordingCreated = false;

            // deleted recording config
            RecordingConfiguration oldConfig = null;

            RecordingConfiguration config = new RecordingConfiguration();
            config.Source = new RecordingSourceInformation();
            config.Source.Description = "SourceDescription";

            // A device shall support at least 128 characters.
            config.Source.SourceId = "http://source-id-for-test.com/identifier/for/the/source/chosen/by/the/client/that/creates/the/structure/0001/0002/0003/0004/005";
            config.Source.Location = "LocationDescription";
            // A device shall support at least 20 characters.
            config.Source.Name = "SourceNameForTest001";
            // A device shall support at least 128 characters.
            config.Source.Address = "http://source-address-for-test.com/uri/provided/by/the/service/supplying/data/to/be/recorded/0001/0002/0003/0004/0005/0006/0007";
            config.MaximumRetentionTime = "PT60S";
            config.Content = "Create recording dynamic test";

            RunTest(() =>
            {

                // A.10 begins
                RecordingServiceCapabilities capabilities = GetServiceCapabilities();
                Assert(capabilities != null, "Capabilities hasn't been returned", "Check that capabilities has been returned");

                Assert(capabilities.DynamicRecordings == true,
                    "Recording creating operation is unavailable", "Check that recording can be created");

                GetRecordingsResponseItem[] recordings = GetRecordings();

                if (capabilities.MaxRecordingsSpecified == true && recordings.Length == capabilities.MaxRecordings)
                {
                    LogTestEvent(string.Format("Max limit of recordings is reached. Trying to delete some recording...{0}", Environment.NewLine));

                    DeleteRecordingIfNecessary(recordings, 
                                               out recordingTokenDeleted, out oldConfig);
                }

                bool needToDeleteRecording = false;

                try
                {
                    recordingTokenCreated = CreateRecording(config);
                    recordingCreated = true;
                }
                catch (FaultException exc)
                {
                    if (exc.IsValidOnvifFault("Receiver/Action/MaxRecordings"))
                    {
                        needToDeleteRecording = true;
                        LogStepEvent("Can't create new recording because of Max Recording limit is reached");
                        StepPassed();
                    }
                    else
                    {
                        StepFailed(exc);
                    }
                }

                if (needToDeleteRecording == true)
                {
                    LogTestEvent(string.Format("Max limit of recordings is reached. Trying to delete some recording...{0}", Environment.NewLine));

                    DeleteRecordingIfNecessary(recordings,
                                               out recordingTokenDeleted, out oldConfig);
                }

                if (recordingCreated == true)
                {
                    RecordingConfiguration configRefreshed = GetRecordingConfiguration(recordingTokenCreated);

                    // Validation of the result 
                    Assert(new CheckCondition(() =>
                    {
                        if (configRefreshed.Source.Name == config.Source.Name &&
                            configRefreshed.Source.SourceId == config.Source.SourceId &&
                            configRefreshed.Source.Address == config.Source.Address)
                            return true;
                        else
                            return false;
                    }),
                    "Settings were not applied", "Validation of applied settings");
                }            
            },
            () =>
            {
                bool needToRestore = false;

                if (recordingTokenCreated != null || recordingTokenDeleted != null)
                    needToRestore = true;

                if (needToRestore)
                    LogTestEvent(string.Format("Restoring the initial settings...{0}", Environment.NewLine));
                
                if (recordingTokenCreated != null && recordingCreated == true)
                {
                    DeleteRecording(recordingTokenCreated);
                }

                if (recordingTokenDeleted != null && oldConfig != null)
                {
                    CreateRecording(oldConfig);
                }
            });
        }
    }
}
