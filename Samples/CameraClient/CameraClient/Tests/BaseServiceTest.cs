using System.ServiceModel;

namespace CameraClient.Tests
{

    class BaseServiceTest<TContract, TClient> : BaseTest
        where TContract : class
        where TClient : ClientBase<TContract>, new()
    {

        protected BaseServiceTest(string address)
            : base(address)
        {
            
        }

        private TClient _client;

        protected TClient Client
        {
            get { return _client; }
        }

        protected void BeginTest()
        {
            _client = new TClient();
            _client.Endpoint.Address = new EndpointAddress(_cameraAddress);
            TestTrace.TraceBehavior tb = new TestTrace.TraceBehavior();
            tb.RequestReady += tb_RequestReady;
            tb.ResponseReceived += tb_ResponseReceived;
            _client.Endpoint.Behaviors.Add(tb);

            ResetLog();
            // other configuration
        }

        protected void EndTest()
        {
            _client.Close();
            _client = null;
            TestCompleted();
        }
        
        void tb_RequestReady(System.ServiceModel.Channels.Message request)
        {
            LogRequest(request);
        }

        void tb_ResponseReceived(System.ServiceModel.Channels.Message response)
        {
            LogResponse(response);
        }




    
    }
}
