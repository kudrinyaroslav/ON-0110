///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Net;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.Tests.Engine
{
    /// <summary>
    /// Parameters for running tests in test suite.
    /// </summary>
    public class TestSuiteParameters
    {
        public TestSuiteParameters()
        {
            _testCases = new List<TestInfo>();
            _allTestCases = new List<TestInfo>();
            _profiles = new List<IProfileDefinition>();
        }

        private List<TestInfo> _testCases;
        
        /// <summary>
        /// List of tests to be executed
        /// </summary>
        public List<TestInfo> TestCases
        {
            get { return _testCases; }
        }

        private List<TestInfo> _allTestCases;

        /// <summary>
        /// List of tests to be executed
        /// </summary>
        public List<TestInfo> AllTestCases
        {
            get { return _allTestCases; }
        }

        private List<IProfileDefinition> _profiles;

        /// <summary>
        /// List of profiles
        /// </summary>
        public List<IProfileDefinition> Profiles
        {
            get { return _profiles; }
        }

        private bool _defineProfiles;

        /// <summary>
        /// True if profiles support must be checked
        /// </summary>
        public bool DefineProfiles
        {
            get { return _defineProfiles;}
            set { _defineProfiles = value;}
        }

        private bool _conformance;

        /// <summary>
        /// True if conformance process is running (tests selection depends on conformance/diagnostic)
        /// </summary>
        public bool Conformance
        {
            get { return _conformance; }
            set { _conformance = value; }
        }

        private bool _featureDefinition;

        /// <summary>
        /// True, if only features definition is required.
        /// </summary>
        public bool FeatureDefinition
        {
            get { return _featureDefinition; }
            set { _featureDefinition = value; }
        }

        private NetworkInterfaceDescription _nic;

        /// <summary>
        /// Network interface to be used
        /// </summary>
        public NetworkInterfaceDescription NetworkInterfaceController
        {
            get { return _nic; }
            set { _nic = value; }
        }

        private EnvironmentSettings _environmentSettings;

        /// <summary>
        /// Environment settings
        /// </summary>
        public EnvironmentSettings EnvironmentSettings 
        { 
            get { return _environmentSettings; } 
            set { _environmentSettings = value; }
        }
        
        private string _address;

        /// <summary>
        /// Device management service address
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        
        private IPAddress _cameraIp;

        /// <summary>
        /// Camera IP address
        /// </summary>
        public IPAddress CameraIP
        {
            get { return _cameraIp; }
            set { _cameraIp = value; }
        }

        private string _cameraUUID;
        /// <summary>
        /// Camera UUID
        /// </summary>
        public string CameraUUID
        {
            get { return _cameraUUID; }
            set { _cameraUUID = value; }
        }
        
        private int _messageTimeout;

        /// <summary>
        /// MEssage timeout
        /// </summary>
        public int MessageTimeout
        {
            get { return _messageTimeout; }
            set { _messageTimeout = value;}
        }

        private int _rebootTimeout;

        /// <summary>
        /// Reboot timeout
        /// </summary>
        public int RebootTimeout
        {
            get { return _rebootTimeout; }
            set { _rebootTimeout = value; }
        }

        private int _timeBetweenTests;

        /// <summary>
        /// Time between tests if last test failed.
        /// </summary>
        public int TimeBetweenTests
        {
            get { return _timeBetweenTests; }
            set { _timeBetweenTests = value; }
        }

        private List<Feature> _features = new List<Feature>();
        
        /// <summary>
        /// Features defined as "implemented" and "supported"
        /// </summary>
        public List<Feature> Features
        {
            get { return _features; }
        }

        private string _userName;
        
        /// <summary>
        /// Username
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _password;
        
        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }


        private bool _useUTCTimestamp;
        /// <summary>
        /// Format of timestamp
        /// </summary>
        public bool UseUTCTimestamp 
        {
            get { return _useUTCTimestamp; }
            set { _useUTCTimestamp = value; }
        }

        private IOperator _operator;
        
        /// <summary>
        /// Interface for getting operator's answers
        /// </summary>
        public IOperator Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        private IVideoForm _videoForm;

        /// <summary>
        /// Video form
        /// </summary>
        public IVideoForm VideoForm
        {
            get { return _videoForm; }
            set { _videoForm = value; }
        }

        private bool _interactiveFirst;

        /// <summary>
        /// Flag indicating whether interactive tests should be run first.
        /// </summary>
        public bool InteractiveFirst
        {
            get { return _interactiveFirst; }
            set { _interactiveFirst = value; }
        }

        private string _ptzNodeToken;
        /// <summary>
        /// PTZ node to test token
        /// </summary>
        public string PTZNodeToken
        {
            get { return _ptzNodeToken; }
            set { _ptzNodeToken = value; }
        }
        
        //private HttpTransport.ChannelControllers.Security _security;

        //public HttpTransport.ChannelControllers.Security Security
        //{
        //    get { return _security; }
        //    set { _security = value; }
        //}

        public string VideoSourceToken { get; set; }
        public bool UseEmbeddedPassword { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public string SecureMethod { get; set; }

        public int OperationDelay { get; set; }
        public int RecoveryDelay { get; set; }

        public string EventTopic { get; set; }
        public string TopicNamespaces { get; set; }
        public int SubscriptionTimeout { get; set; }
        
        public int RelayOutputDelayTimeMonostable { get; set; }

        public Dictionary<string, object> AdvancedPrameters { get; set; }

    }
}
