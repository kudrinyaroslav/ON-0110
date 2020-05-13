///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Net;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.Discovery;

namespace TestTool.Tests.Common.TestEngine
{
    /// <summary>
    /// Parameters for running tests in test suite.
    /// </summary>
    public class TestSuiteParameters
    {
        public TestSuiteParameters()
        {
            _testCases = new List<TestInfo>();
        }

        private List<TestInfo> _testCases;
        
        /// <summary>
        /// List of tests to be executed
        /// </summary>
        public List<TestInfo> TestCases
        {
            get { return _testCases; }
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

        private List<Enums.Feature> _features = new List<Feature>();
        
        /// <summary>
        /// Features selected as "implemented" and "supported"
        /// </summary>
        public List<Enums.Feature> Features
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

        public bool UseEmbeddedPassword { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }

        public int OperationDelay { get; set; }
        public int RecoveryDelay { get; set; }
    }
}
