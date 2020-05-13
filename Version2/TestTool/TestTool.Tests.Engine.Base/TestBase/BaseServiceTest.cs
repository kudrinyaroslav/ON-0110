///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.ServiceModel;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.Engine.Base.TestBase
{
    /// <summary>
    /// Base test with proxy class inside. Contains functionality to initialize service client, 
    /// Begin/End/Run tests.
    /// </summary>
    /// <typeparam name="TContract">Service contract.</typeparam>
    /// <typeparam name="TClient">Service client type.</typeparam>
    public abstract class BaseServiceTest<TContract, TClient> : BaseOnvifTest
        where TContract : class
        where TClient : ClientBase<TContract>, new()
    {
        /// <summary>
        /// Creates BaseServiceTest instance.
        /// </summary>
        /// <param name="param">Parameters for test run.</param>
        protected BaseServiceTest(TestLaunchParam param)
            : base(param)
        {

        }

        /// <summary>
        /// Proxy class.
        /// </summary>
        private TClient _client;

        /// <summary>
        /// Service client.
        /// </summary>
        protected TClient Client
        {
            get { return _client; }
        }
        


        /// <summary>
        /// Creates specific client.
        /// </summary>
        /// <returns>Service client for specific service.</returns>
        protected abstract TClient CreateClient();
        
        /// <summary>
        /// Updates security.
        /// </summary>
        protected void UpdateSecurity(bool updateGUI = false)
        {
            if (_client != null)
            {
                _client.Close();
            }

            if (null != _credentialsProvider)
            {
                _credentialsProvider.Security = _security;
                _credentialsProvider.Username = _username;
                _credentialsProvider.Password = _password;
            }

            if (updateGUI)
                RaiseSecurityChangedEvent(Credentials);

            _client = CreateClient();
            System.Net.ServicePointManager.Expect100Continue = false;
            AttachSecurity(_client.Endpoint);
            SetupChannel(_client.InnerChannel);
        }

        protected override void Initialize()
        {
            base.Initialize();
            _client = CreateClient();
            System.Net.ServicePointManager.Expect100Continue = false;
            AttachSecurity(_client.Endpoint);
            SetupChannel(_client.InnerChannel);

        }

        protected override void Initialize(BaseTest test)
        {
            base.Initialize(test);
            _client = CreateClient();
            System.Net.ServicePointManager.Expect100Continue = false;
            AttachSecurity(_client.Endpoint);
            SetupChannel(_client.InnerChannel);

        }

        protected override void Release()
        {
            base.Release();
            if(_client != null)
            {
                _client.Close();
                _client = null;
            }            
        }
        /// <summary>
        /// Sets up the channel to spoil the messages (for fault tests)
        /// </summary>
        /// <param name="spoiler">Object to process the message.</param>
        protected void SetBreakingBehaviour(ISoapMessageMutator spoiler)
        {
            if (_client != null)
            {
                SetBreakingBehaviour(Client.Endpoint, spoiler);
            }
        }

        /// <summary>
        /// Resets channel to pass packets "as is"
        /// </summary>
        protected void ResetBreakingBehaviour()
        {
            if (Client != null)
            {
                SetBreakingBehaviour(Client.Endpoint, null);
            }
        }
    }
}
