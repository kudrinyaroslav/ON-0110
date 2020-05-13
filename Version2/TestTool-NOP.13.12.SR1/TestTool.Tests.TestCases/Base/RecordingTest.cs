using System.ServiceModel;
using System.ServiceModel.Channels;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.TestCases.Utils.SoapValidation;

namespace TestTool.Tests.TestCases.Base
{
    public class RecordingTest : CommonServiceTest<RecordingPort, RecordingPortClient>
    {
        protected string _retentionTime;

        protected const string RECEIVERSOURCETYPE = "http://www.onvif.org/ver10/schema/Receiver";
        protected const string PROFILESOURCETYPE = "http://www.onvif.org/ver10/schema/Profile";

                
        public RecordingTest(TestLaunchParam param)
            : base(param)
        {
            _retentionTime = param.RetentionTime;
        }

        ReceiverPortClient _receiverClient = null;

        protected ReceiverPortClient ReceiverClient
        {
            get
            {
                if (_receiverClient == null)
                {
                    BeginStep("Connect to Receiver service");
                    string serviceAddress = DeviceClient.GetReceiverServiceAddress(Features);
                    
                    LogStepEvent(string.Format("Receiver service address: {0}", serviceAddress));
                    if (!serviceAddress.IsValidUrl())
                    {
                        throw new AssertException("Receiver service address is invalid");
                    }

                    Binding binding = CreateBinding(false,
                        new IChannelController[] { new SoapValidator(ReceiverSchemasSet.GetInstance()) });
                    _receiverClient = new ReceiverPortClient(binding, new EndpointAddress(serviceAddress));
                    StepPassed();               
                }


                return _receiverClient;
            }        
        }

        /// <summary>
        /// Returns DUT's recording service address
        /// </summary>
        /// <returns>Recording service url</returns>
        protected string GetRecordingServiceAddress()
        {
            string address = string.Empty;
            RunStep(() =>
            {
                DeviceClient device = DeviceClient;
                
                address = device.GetRecordingsServiceAddress(Features);

                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException("The DUT did not return Recording service address");
                }

            }, "Get Recording Service address");
            DoRequestDelay();
            return address;
        }
        
        protected override string ServiceName
        {
            get { return "Recording"; }
        }

        protected override string GetServiceAddress()
        {
            return GetRecordingServiceAddress();
        }

        protected override RecordingPortClient CreateClient(string address)
        {
            Binding binding = CreateBinding(false,
                new IChannelController[] { new SoapValidator(RecordingSchemasSet.GetInstance()) });
                RecordingPortClient client = new RecordingPortClient(binding, new EndpointAddress(address));
            return client;
        }

        protected override void Release()
        {
            if (_receiverClient != null)
            {
                _receiverClient.Close();
            }
            base.Release();
        }

        protected RecordingServiceCapabilities GetServiceCapabilities()
        {
            RecordingServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = Client.GetServiceCapabilities(); }, "Get Service Capabilities");
            DoRequestDelay();
            return capabilities;
        }


        protected GetRecordingsResponseItem[] GetRecordings()
        {
            GetRecordingsResponseItem[] items = null;
            RunStep(() => { items = Client.GetRecordings(); }, "Get Recordings");
            DoRequestDelay();
            return items;
        }

        protected RecordingConfiguration GetRecordingConfiguration(string token)
        {
            RecordingConfiguration response = null;
            RunStep(() => { response = Client.GetRecordingConfiguration(token); }, 
                string.Format("Get Recording Configuration (token = '{0}')", token));
            DoRequestDelay();
            return response;
        }

        protected GetRecordingJobsResponseItem[] GetRecordingJobs()
        {
            GetRecordingJobsResponseItem[] items = null;
            RunStep(() => { items = Client.GetRecordingJobs(); }, "Get Recording Jobs");
            DoRequestDelay();
            return items;
        }

        protected RecordingJobConfiguration GetRecordingJobConfiguration(string token)
        {
            RecordingJobConfiguration configuration = null;
            RunStep(() => { configuration = Client.GetRecordingJobConfiguration(token); }, 
                string.Format("Get Recording Job Configuration (token = '{0}')", token));
            DoRequestDelay();
            return configuration;
        }

        protected RecordingJobStateInformation GetRecordingJobState(string token)
        {
            RecordingJobStateInformation stateInformation = null;
            RunStep(() => { stateInformation = Client.GetRecordingJobState(token); },
                string.Format("Get Recording Job State (token = '{0}')", token));
            DoRequestDelay();
            return stateInformation;
        }

        protected TrackConfiguration GetTrackConfiguration(string recordingToken, string trackToken)
        {
            TrackConfiguration trackConfiguration = null;
            RunStep(() => { trackConfiguration = Client.GetTrackConfiguration(recordingToken, trackToken); },
                string.Format("Get track configuration (recording token = '{0}', track token = '{1}')", 
                    recordingToken, trackToken));
            DoRequestDelay();

            
            return trackConfiguration;
        }

        protected string CreateRecordingJob(ref RecordingJobConfiguration configuration)
        {
            return CreateRecordingJob(ref configuration, "Create recording job");
        }

        protected string CreateRecordingJob(ref RecordingJobConfiguration configuration, string stepName)
        {
            RecordingJobConfiguration config = configuration;

            string result = string.Empty;
            RunStep(() => { result = Client.CreateRecordingJob(ref config) ; },
                stepName);
            DoRequestDelay();
            configuration = config;
            return result;
        }
        protected string CreateRecording(RecordingConfiguration configuration)
        {
            return CreateRecording(configuration, "Create recording");
        }

        protected string CreateRecording(RecordingConfiguration configuration, string stepName)
        {
            string result = string.Empty;
            RunStep(() => { result = Client.CreateRecording(configuration); },
                stepName);
            DoRequestDelay();
            return result;
        }
        
        protected string CreateTrack(string token, TrackConfiguration configuration)
        {
            return CreateTrack(token, configuration, "Create track");
        }

        protected string CreateTrack(string token, TrackConfiguration configuration, string stepName)
        {
            string result = string.Empty;
            RunStep(() => { result = Client.CreateTrack(token, configuration); },
                stepName);
            DoRequestDelay();
            return result;
        }
        
        protected void SetRecordingConfiguration(string recordingToken, RecordingConfiguration recordingConfiguration)
        {
            SetRecordingConfiguration(recordingToken, recordingConfiguration, "Set Recording Configuration");
        }

        protected void SetRecordingConfiguration(string recordingToken, RecordingConfiguration recordingConfiguration, string stepName)
        {
            RunStep(() => { Client.SetRecordingConfiguration(recordingToken, recordingConfiguration); }, stepName);
            DoRequestDelay();
        }
                
        protected void DeleteRecording(string token)
        {
            DeleteRecording(token, string.Format("Delete recording '{0}'", token));
        }

        protected void DeleteRecording(string token, string stepName)
        {
            RunStep(() => { Client.DeleteRecording(token); }, stepName);
            DoRequestDelay();
        }

        protected void DeleteTrack(string recordingToken, string trackToken)
        {
            DeleteTrack(recordingToken, trackToken, string.Format("Delete track '{0}' from recording '{1}'", trackToken, recordingToken));
        }

        protected void DeleteTrack(string recordingToken, string trackToken, string stepName)
        {
            RunStep(() => { Client.DeleteTrack(recordingToken, trackToken); }, stepName);
            DoRequestDelay();
        }

        protected void SetRecordingJobMode(string jobToken, string mode)
        {
            RunStep(() => { Client.SetRecordingJobMode(jobToken, mode); }, 
                string.Format("Set Recording Job Mode (jobToken = '{0}') to '{1}'", jobToken, mode));
            DoRequestDelay();
        }


        protected void DeleteRecordingJob(string jobToken)
        {
            RunStep(() => { Client.DeleteRecordingJob(jobToken); }, 
                string.Format("Delete Recording Job (jobToken = '{0}')", jobToken));
            DoRequestDelay();
        }




        #region Receiver service methods

        protected Receiver GetReceiver(string receiverToken)
        {
            ReceiverPortClient client = ReceiverClient;
            return CommonMethodsProvider.GetReceiver(this, client, receiverToken);
        }

        protected void ConfigureReceiver(string receiverToken, ReceiverConfiguration config)
        {
            ReceiverPortClient client = ReceiverClient;
            CommonMethodsProvider.ConfigureReceiver(this, client, receiverToken, config);
        }
        
        protected void SetReceiverMode(string receiverToken, ReceiverMode mode)
        {
            ReceiverPortClient client = ReceiverClient;
            CommonMethodsProvider.SetReceiverMode(this, client, receiverToken, mode);
        }        
        
        protected ReceiverStateInformation GetReceiverState(string receiverToken)
        {
            ReceiverPortClient client = ReceiverClient;
            return CommonMethodsProvider.GetReceiverState(this, client, receiverToken);
        }
        
        protected ReceiverServiceCapabilities GetReceiverServiceCapabilities()
        {
            ReceiverPortClient client = ReceiverClient;
            return CommonMethodsProvider.GetReceiverServiceCapabilities(this, client);
        }

        protected Receiver[] GetReceivers()
        {
            ReceiverPortClient client = ReceiverClient;
            return CommonMethodsProvider.GetReceivers(this, client);
        }

        protected void DeleteReceiver(string receiverToken)
        {
            ReceiverPortClient client = ReceiverClient;
            CommonMethodsProvider.DeleteReceiver(this, client, receiverToken);
        }

        protected RecordingOptions GetRecordingOptions(string token)
        {
            RecordingOptions options = null;
            RunStep(() => { options = Client.GetRecordingOptions(token); },
                string.Format("Get Recording Options (token = '{0}')", token));
            DoRequestDelay();
            return options;
        }

        #endregion
    }
}
