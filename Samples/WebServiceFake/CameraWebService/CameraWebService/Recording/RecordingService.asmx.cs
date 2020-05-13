using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace CameraWebService.Recording
{
    /// <summary>
    /// Summary description for RecordingService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RecordingService : RecordingBinding
    {

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override RecordingServiceCapabilities GetServiceCapabilities()
        {
            RecordingServiceCapabilities capabilities =
                new RecordingServiceCapabilities()
                    {
                        DynamicRecordingsSpecified = true,
                        DynamicRecordings = true,
                        DynamicTracksSpecified = false,
                        MaxRate = 1,
                        MaxRateSpecified = true,
                        MaxRecordingsSpecified = true,
                        MaxRecordings = 3,
                        MaxTotalRateSpecified = false
                    };

            capabilities.Encoding = new string[] { "JPEG", "MPEG4" };

            return capabilities;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/CreateRecording", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RecordingToken")]
        public override string CreateRecording(RecordingConfiguration RecordingConfiguration)
        {
            CameraWebService.Search.RecordingInformation recording = new CameraWebService.Search.RecordingInformation();
            string token = "recording" + Guid.NewGuid().ToString().Substring(0, 8);
            recording.RecordingToken = token;
            recording.Source = new CameraWebService.Search.RecordingSourceInformation();
            recording.Source.Description = RecordingConfiguration.Source.Description;
            recording.Source.Address = RecordingConfiguration.Source.Address;
            recording.Source.Location = RecordingConfiguration.Source.Location;
            recording.Source.Name = RecordingConfiguration.Source.Name;
            recording.Source.SourceId = RecordingConfiguration.Source.SourceId;
            recording.Content = RecordingConfiguration.Content;
            recording.RecordingStatus = CameraWebService.Search.RecordingStatus.Initiated;

            //RecordingConfiguration.MaximumRetentionTime;
            List<CameraWebService.Search.TrackInformation> trackList = new List<CameraWebService.Search.TrackInformation>();
            trackList.Add(new CameraWebService.Search.TrackInformation()
                {
                    Description = string.Empty,
                    TrackType = CameraWebService.Search.TrackType.Video,
                    TrackToken = "VIDEO001"
                });
            trackList.Add(new CameraWebService.Search.TrackInformation()
                {
                    Description = string.Empty,
                    TrackType = CameraWebService.Search.TrackType.Audio,
                    TrackToken = "AUDIO001"
                });
            trackList.Add(new CameraWebService.Search.TrackInformation()
                {
                    Description = string.Empty,
                    TrackType = CameraWebService.Search.TrackType.Metadata,
                    TrackToken = "META001"
                });
            recording.Track = trackList.ToArray();
            Search.SearchStorage.Instance.Recordings.Add(recording);
            return token;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/DeleteRecording", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteRecording(string RecordingToken)
        {
            CameraWebService.Search.RecordingInformation recording =
                Search.SearchStorage.Instance.Recordings.Where(r => r.RecordingToken == RecordingToken).FirstOrDefault();
            if (recording != null)
                Search.SearchStorage.Instance.Recordings.Remove(recording);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetRecordings", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RecordingItem")]
        public override GetRecordingsResponseItem[] GetRecordings()
        {
            List<GetRecordingsResponseItem> items = new List<GetRecordingsResponseItem>();

            foreach (Search.RecordingInformation info in Search.SearchStorage.Instance.Recordings)
            {
                GetRecordingsResponseItem item = new GetRecordingsResponseItem();

                item.RecordingToken = info.RecordingToken;

                item.Configuration = new RecordingConfiguration();
                item.Configuration.Source = new RecordingSourceInformation();
                item.Configuration.Source.Description = info.Source.Description;
                item.Configuration.Source.Address = info.Source.Address;
                item.Configuration.Source.Location = info.Source.Location;
                item.Configuration.Source.Name = info.Source.Name;
                item.Configuration.Source.SourceId = info.Source.SourceId;

                item.Configuration.Content = info.Content;
                item.Configuration.MaximumRetentionTime = "PT10S";

                item.Tracks = new GetTracksResponseList();

                List<GetTracksResponseItem> trackItems = new List<GetTracksResponseItem>();
                bool first = true;
                foreach (Search.TrackInformation track in info.Track)
                {
                    //if (info.RecordingToken.EndsWith("1"))
                    //{
                    //    if (first)
                    //    {
                    //        first = false;
                    //        continue;
                    //    }
                    //}

                    GetTracksResponseItem trackItem = new GetTracksResponseItem();
                    trackItem.TrackToken = track.TrackToken;
                    trackItem.Configuration = new TrackConfiguration();
                    trackItem.Configuration.Description = track.Description;

                    trackItem.Configuration.TrackType = (TrackType)((int)track.TrackType);

                    //if (info.RecordingToken.EndsWith("2"))
                    //{
                    //    trackItem.Configuration.TrackType = (TrackType)(3-(int)track.TrackType);
                    //}

                    trackItems.Add(trackItem);
                }

                item.Tracks.Track = trackItems.ToArray();
                items.Add(item);

            }

            return items.ToArray();
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/SetRecordingConfiguration", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRecordingConfiguration(string RecordingToken, RecordingConfiguration RecordingConfiguration)
        {
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetRecordingConfiguration", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RecordingConfiguration")]
        public override RecordingConfiguration GetRecordingConfiguration(string RecordingToken)
        {
            Search.RecordingInformation info = Search.SearchStorage.Instance.Recordings.Where(r => r.RecordingToken == RecordingToken).FirstOrDefault();
            RecordingConfiguration config = new RecordingConfiguration();
            config.MaximumRetentionTime = "PT10S";
            config.Content = info.Content;

            config.Source = new RecordingSourceInformation();
            config.Source.Description = info.Source.Description;
            config.Source.Address = info.Source.Address;
            config.Source.Location = info.Source.Location;
            config.Source.Name = info.Source.Name;
            config.Source.SourceId = info.Source.SourceId;
            return config;
        }

        public override string CreateTrack(string RecordingToken, TrackConfiguration TrackConfiguration)
        {
            throw new NotImplementedException();
        }

        public override void DeleteTrack(string RecordingToken, string TrackToken)
        {
            throw new NotImplementedException();
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetTrackConfiguration", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("TrackConfiguration")]
        public override TrackConfiguration GetTrackConfiguration(string RecordingToken, string TrackToken)
        {
            var recording = Search.SearchStorage.Instance.Recordings.Where(r => r.RecordingToken == RecordingToken).FirstOrDefault();
            if (recording == null)
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "UnknownToken" });
            var track = recording.Track.Where(t => t.TrackToken == TrackToken).FirstOrDefault();
            if (track == null)
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "UnknownToken" });
            TrackConfiguration trackConfig = new TrackConfiguration()
            {
                Description = track.Description,
                TrackType = (TrackType)((int)track.TrackType)
            };
            return trackConfig;
        }

        public override void SetTrackConfiguration(string RecordingToken, string TrackToken, TrackConfiguration TrackConfiguration)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/CreateRecordingJob", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("JobToken")]
        public override string CreateRecordingJob(ref RecordingJobConfiguration JobConfiguration)
        {
            if (Search.SearchStorage.Instance.RecordingJobs.Count > 5)
            {
                ReturnFault(new string[] { "Receiver", "Action", "MaxRecordingJobs" });
            }

            GetRecordingJobsResponseItem job = new GetRecordingJobsResponseItem();
            job.JobToken = "job" + Guid.NewGuid().ToString().Substring(0, 8);
            job.JobConfiguration = JobConfiguration;

            if (JobConfiguration.Source != null)
            {
                foreach (RecordingJobSource source in JobConfiguration.Source)
                {
                    if (source.AutoCreateReceiverSpecified && source.AutoCreateReceiver)
                    {
                        Receiver.Receiver receiver = new Receiver.Receiver();
                        receiver.Token = Guid.NewGuid().ToString();
                        receiver.Configuration = new CameraWebService.Receiver.ReceiverConfiguration();
                        receiver.Configuration.StreamSetup = new CameraWebService.Receiver.StreamSetup();
                        receiver.Configuration.MediaUri = string.Empty;
                        receiver.Configuration.StreamSetup.Stream = CameraWebService.Receiver.StreamType.RTPUnicast;
                        receiver.Configuration.StreamSetup.Transport = new CameraWebService.Receiver.Transport();
                        receiver.Configuration.StreamSetup.Transport.Protocol = CameraWebService.Receiver.TransportProtocol.RTSP;

                        source.SourceToken = new SourceReference();
                        source.SourceToken.Token = receiver.Token;
                        source.SourceToken.Type = "http://www.onvif.org/ver10/schema/Receiver";


                        Search.SearchStorage.Instance.Receivers.Add(receiver);
                    }
                }
            }
            
            Search.SearchStorage.Instance.RecordingJobs.Add(job);
            Search.SearchStorage.Instance.JobStates.Add(job.JobToken, JobConfiguration.Mode);

            return job.JobToken;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/DeleteRecordingJob", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteRecordingJob(string JobToken)
        {
            GetRecordingJobsResponseItem job = Search.SearchStorage.Instance.RecordingJobs.Where(j => j.JobToken == JobToken).FirstOrDefault();

            if (job == null)
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "NoRecordingJob" });
            }
            else 
            {
                Search.SearchStorage.Instance.RecordingJobs.Remove(job);
            }
        
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetRecordingJobs", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("JobItem")]
        public override GetRecordingJobsResponseItem[] GetRecordingJobs()
        {
            return Search.SearchStorage.Instance.RecordingJobs.ToArray();
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/SetRecordingJobConfiguration", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRecordingJobConfiguration(string JobToken, ref RecordingJobConfiguration JobConfiguration)
        {
            Search.SearchStorage.Instance.JobStates[JobToken] = JobConfiguration.Mode;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetRecordingJobConfiguration", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("JobConfiguration")]
        public override RecordingJobConfiguration GetRecordingJobConfiguration(string JobToken)
        {
            RecordingJobConfiguration config = Search.SearchStorage.Instance.GetRecordingJobConfiguration(JobToken);
            if (config == null)
            {
                ReturnFault(new string[] { "Sender", "InvalidArgVal", "NoRecordingJob" });
            }

            return config;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/SetRecordingJobMode", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRecordingJobMode(string JobToken, string Mode)
        {

        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetRecordingJobState", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("State")]
        public override RecordingJobStateInformation GetRecordingJobState(string JobToken)
        {
            var jobStateInfo = new RecordingJobStateInformation();
            var jobState = Search.SearchStorage.Instance.RecordingJobs.Where(j => j.JobToken == JobToken).FirstOrDefault();
            jobStateInfo.RecordingToken = jobState.JobConfiguration.RecordingToken;
            jobStateInfo.State = "state";
            if (Search.SearchStorage.Instance.JobStates.ContainsKey(JobToken))
            {
                jobStateInfo.State = Search.SearchStorage.Instance.JobStates[JobToken] ;
            }
            List<RecordingJobStateSource> jobStateSourceList = new List<RecordingJobStateSource>();

            if (jobState.JobConfiguration.Source != null)
            {
                foreach (var source in jobState.JobConfiguration.Source)
                {
                    var jobStateSource = new RecordingJobStateSource();
                    jobStateSource.SourceToken = source.SourceToken;
                    jobStateSource.State = "state";
                    List<RecordingJobStateTrack> jobStateTracks = new List<RecordingJobStateTrack>();
                    foreach (var track in source.Tracks)
                    {
                        RecordingJobStateTrack jobStateTrack = new RecordingJobStateTrack();
                        jobStateTrack.SourceTag = track.SourceTag;
                        jobStateTrack.Destination = track.Destination;
                        jobStateTrack.State = "state";
                        jobStateTracks.Add(jobStateTrack);
                    }
                    jobStateSource.Tracks = new RecordingJobStateTracks();
                    jobStateSource.Tracks.Track = jobStateTracks.ToArray();
                    jobStateSourceList.Add(jobStateSource);
                }
            }
            jobStateInfo.Sources = jobStateSourceList.ToArray();
            return jobStateInfo;
        }

        void ReturnFault(string[] codes)
        {
            SoapFaultSubCode subCode = null;
            for (int i = codes.Length - 1; i > 0; i--)
            {
                SoapFaultSubCode currentSubCode = new SoapFaultSubCode(new XmlQualifiedName(codes[i], "http://www.onvif.org/ver10/error"), subCode);
                subCode = currentSubCode;
            }
            throw new SoapException("Error", new XmlQualifiedName(codes[0], "http://www.w3.org/2003/05/soap-envelope"), subCode);
        }
    }
}
