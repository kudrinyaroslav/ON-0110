///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Net;
using System.Reflection;
using System.Threading;
using TestTool.Tests.Common.Trace;
using TestTool.Tests.Common.Exceptions;
using System.Collections.Generic;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Discovery;
using System.ServiceModel;
using System.Linq;

namespace TestTool.Tests.Common.TestBase
{
    /// <summary>
    /// Base test (== class with methods referring to tests from specification)
    /// </summary>
    public class BaseTest : ITestSuite
    {
        /// <summary>
        /// Network interface data.
        /// </summary>
        protected NetworkInterfaceDescription _nic;

        /// <summary>
        /// Environment settings.
        /// </summary>
        protected EnvironmentSettings _environmentSettings;
        /// <summary>
        /// Camera UUID.
        /// </summary>
        protected string _cameraId;
        /// <summary>
        /// Camera service address.
        /// </summary>
        protected string _cameraAddress;
        /// <summary>
        /// Camera IP address.
        /// </summary>
        protected IPAddress _cameraIp;
        /// <summary>
        /// Message timeout.
        /// </summary>
        protected int _messageTimeout;
        /// <summary>
        /// Reboot timeout.
        /// </summary>
        protected int _rebootTimeout;
        /// <summary>
        /// Username to be used in service calls.
        /// </summary>
        protected string _username;
        /// <summary>
        /// Password
        /// </summary>
        protected string _password;
        /// <summary>
        /// Timestamp format
        /// </summary>
        protected bool _useUTCTimestamp;
        /// <summary>
        /// Current step to save data.
        /// </summary>
        private StepResult _currentStep;
        /// <summary>
        /// Number of the current step.
        /// </summary>
        private int _currentStepNum;
        /// <summary>
        /// Current log
        /// </summary>
        protected TestLog _currentLog;
        /// <summary>
        /// PTZ node to test
        /// </summary>
        protected string _ptzNodeToken;

        /// <summary>
        /// Object to interact with test operator 
        /// </summary>
        protected IOperator _operator;

        /// <summary>
        /// UI to display video.
        /// </summary>
        protected IVideoForm _videoForm;

        /// <summary>
        /// Object to stop waiting for the answer.
        /// </summary>
        protected TestSemaphore _semaphore;

        /// <summary>
        /// Indicates that passwords from specification should be used.
        /// </summary>
        protected bool _useEmbeddedPassword;
        /// <summary>
        /// First user-provided password to use.
        /// </summary>
        protected string _password1;
        /// <summary>
        /// Second user-provided password to use.
        /// </summary>
        protected string _password2;
        /// <summary>
        /// Time delay for operation which can require some additional time.
        /// </summary>
        protected int _operationDelay;

        /// <summary>
        /// Time after sending request (enough e.g. if a DUT decided to restart).
        /// </summary>
        protected int _recoveryDelay;

        /// <summary>
        /// Relay output delay.
        /// </summary>
        protected string _relayOutputDelayTime;

        /// <summary>
        /// Is raised when a step is completed.
        /// </summary>
        public event StepCompleted OnStepCompleted;
        /// <summary>
        /// Is raised when a test is completed.
        /// </summary>
        public event TestCompleted OnTestCompleted;

        /// <summary>
        /// Is raised when a step is started.
        /// </summary>
        public event Action<StepResult> OnStepStarted;
        /// <summary>
        /// Is raised when a request is sent to the camera.
        /// </summary>
        public event Action<string> OnRequestSent;
        /// <summary>
        /// Is raised when a response is received.
        /// </summary>
        public event Action<string> OnResponseReceived;
        /// <summary>
        /// Is raised when some notification should be performed in the step.
        /// </summary>
        public event Action<string> OnStepEvent;
        /// <summary>
        /// Is raised when some notification should be performed at the test level.
        /// </summary>
        public event Action<string> OnTestEvent;
        /// <summary>
        /// Is raised when test execution is paused.
        /// </summary>
        public event Action Paused;

        /// <summary>
        /// Indicates that test is failed.
        /// </summary>
        protected bool _failed;

        /// <summary>
        /// Indicates that the test is failed.
        /// </summary>
        public bool Failed
        {
            get { return _failed; }
        }


        /// <summary>
        /// Object to catch outgoing and incoming messages.
        /// </summary>
        protected TrafficListener _trafficListener;

        /// <summary>
        /// Endpoint controller (used to control endpoint address )
        /// </summary>
        protected EndpointController _endpointController;
        /// <summary>
        /// Credentials provider.
        /// </summary>
        protected CredentialsProvider _credentialsProvider;

        /// <summary>
        /// Constructor. Performs initialization.
        /// </summary>
        /// <param name="param"></param>
        protected BaseTest(TestLaunchParam param)
        {
            _cameraAddress = param.ServiceAddress;
            _cameraIp = param.CameraIp;
            _cameraId = param.CameraUUID;
            _nic = param.NIC;
            _username = param.UserName;
            _password = param.Password;
            _useUTCTimestamp = param.UseUTCTimestamp;
            _operator = param.Operator;
            _videoForm = param.VideoForm;
            _environmentSettings = param.EnvironmentSettings;
            _ptzNodeToken = param.PTZNodeToken;
            _useEmbeddedPassword = param.UseEmbeddedPassword;
            _password1 = param.Password1;
            _password2 = param.Password2;
            _operationDelay = param.OperationDelay;
            _recoveryDelay = param.RecoveryDelay; 
            _relayOutputDelayTime = param.RelayOutputDelayTime;

            _rebootTimeout = param.RebootTimeout;

            _semaphore = new TestSemaphore();

            _trafficListener = new TrafficListener();
            _trafficListener.RequestSent += LogRequest;
            _trafficListener.ResponseReceived += LogResponse;

            _endpointController = new EndpointController(new EndpointAddress(_cameraAddress));
            
            _credentialsProvider = new CredentialsProvider();
            _credentialsProvider.Username = param.UserName;
            _credentialsProvider.Password = param.Password;
        }
        
        /// <summary>
        /// Message timeout.
        /// </summary>
        public int MessageTimeout
        {
            get { return _messageTimeout; }
            set { _messageTimeout = value;}
        }

        /// <summary>
        /// Features selected for test.
        /// </summary>
        private List<Enums.Feature> _features = new List<Enums.Feature>();

        /// <summary>
        /// Features selected by the operator.
        /// </summary>
        public List<Enums.Feature> Features
        {
            get { return _features; }
        }

        /// <summary>
        /// Indicates that step is open.
        /// </summary>
        protected bool InStep 
        {
            get
            {
                return _currentStep != null;
            }
        } 

        /// <summary>
        /// Initializes step with empty name.
        /// </summary>
        protected void BeginStep()
        {
            BeginStep(string.Empty);
        }

        /// <summary>
        /// Initializes step with the name passed.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        protected void BeginStep(string stepName)
        {
            if (_pause)
            {
                System.Diagnostics.Debug.WriteLine("Begin step - pause");
                DoPause();
            }

            _currentStepNum++;
            _currentStep = new StepResult { Number = _currentStepNum, StepName = stepName, Status = StepStatus.Passed, Message =  stepName};
            
            if (OnStepStarted != null)
            {
                OnStepStarted(_currentStep);
            }
        }

        /// <summary>
        /// Waits for resume or halt.
        /// </summary>
        void DoPause()
        {
            if (Paused != null)
            {
                Paused();
            }
            int handle = WaitHandle.WaitAny(new WaitHandle[] {_resumeEvent, _semaphore.StopEvent});
            System.Diagnostics.Debug.WriteLine("Resume or stop");
            if (handle == 1)
            {
                System.Diagnostics.Debug.WriteLine("STOP - exit test");
                _semaphore.ReportStop();
                return;
            }
            else
            {
                _resumeEvent.Reset();
                System.Diagnostics.Debug.WriteLine("Continue test");
                _pause = false;
            }
        }

        /// <summary>
        /// Marks current step as completed.
        /// </summary>
        private void EndStep()
        {
            if (_currentLog != null)
            {
                _currentLog.Steps.Add(_currentStep);
            }
            ReportStepResult(_currentStep);
            _currentStep = null;
            if (_pause)
            {
                System.Diagnostics.Debug.WriteLine("EndStep - pause requested.");
                DoPause();
            }
        }

        /// <summary>
        /// Perform a step.
        /// </summary>
        /// <param name="action">Action to be performed in a step.</param>
        /// <param name="stepName">Step name</param>
        protected void RunStep(Action action, string stepName)
        {
            RunStep(action, stepName, (string)null);
        }

        /// <summary>
        /// Run step with a fault expected.
        /// </summary>
        /// <param name="action">Action to be performed in a step.</param>
        /// <param name="stepName">Step name.</param>
        /// <param name="expectedFault">Expected fault. If no fault or different fault  is received, 
        /// step is failed.</param>
        protected void RunStep(Action action, string stepName, string expectedFault)
        {
            RunStep(action, stepName, expectedFault, false);
        }

        /// <summary>
        /// Run step with a fault expected
        /// </summary>
        /// <param name="action">Action to be performed.</param>
        /// <param name="stepName">Step name.</param>
        /// <param name="expectedFault">Expected fault. </param>
        /// <param name="exceptOtherFaults">True if WARNING is generated on other faults.</param>
        protected void RunStep(Action action, string stepName, string expectedFault, bool exceptOtherFaults)
        {
            RunStep(action, stepName, expectedFault, exceptOtherFaults, false);
        }

        /// <summary>
        /// Run step with a fault allowed.
        /// </summary>
        /// <param name="action">Action to b performed.</param>
        /// <param name="stepName">Step name.</param>
        /// <param name="expectedFault">Expected fault. </param>
        protected void RunStepAllowFault(Action action, string stepName, string expectedFault)
        {
            RunStep(action, stepName, expectedFault, false, true);
        }

        /// <summary>
        /// Run step with a fault expected.
        /// </summary>
        /// <param name="action">Action to be performed in a step.</param>
        /// <param name="stepName">Step name.</param>
        /// <param name="expectedFault">Expected fault. If no fault or different fault  is received  
        /// and other fault not allowed, step is failed. </param>
        /// <param name="exceptOtherFaults">Indicates if another fault can be excepted
        protected void RunStep(Action action, 
            string stepName, 
            string expectedFault, 
            bool exceptOtherFaults, 
            bool allowNoFault)
        {
            BeginStep(stepName);
            try
            {
                action();
                if (!string.IsNullOrEmpty(expectedFault) && !allowNoFault)
                {
                    AssertException ex = new AssertException(string.Format("\"{0}\" fault is expected, but no SOAP fault returned", expectedFault));
                    throw ex;
                }
            }
            catch (FaultException exc)
            {
                LogStepEvent("SOAP fault returned");
                LogStepEvent(string.Format("Code: {0}", exc.Code.Name));
                System.ServiceModel.FaultCode subCode = exc.Code.SubCode;
                while (subCode != null)
                {
                    LogStepEvent(string.Format("Subcode: {0}", subCode.Name));
                    subCode = subCode.SubCode;
                }

                string faultReason = string.Format("Reason: {0}", exc.Reason).Replace("\n", Environment.NewLine);
                LogStepEvent(faultReason);
                
                /*******************/

                if (string.IsNullOrEmpty(expectedFault))
                {
                    throw exc;
                }
                else
                {
                    string faultDump;
                    bool fault = exc.IsValidOnvifFault(expectedFault, out faultDump);
                    if (!fault)
                    {
                        if(!exceptOtherFaults)
                        {
                            string reason = string.Format("The SOAP FAULT returned from the DUT is invalid: {0}", faultDump);
                            AssertException ex = new AssertException(reason);
                            throw ex;
                        }
                        else
                        {
                            string warning = string.Format("WARNING: The SOAP FAULT returned from the DUT is not as expected: {0}", faultDump);
                            LogStepEvent(warning);
                        }
                    }
                    SaveStepFault(exc);
                }
            }
            StepPassed();
        }
        
        /// <summary>
        /// Method for validating fault.
        /// </summary>
        /// <param name="fault">Exception to be validated. NULL is passed when no fault is received.</param>
        /// <param name="reason">Output verification dump.</param>
        /// <returns>True if fault can be accepted, false otherwise.</returns>
        protected delegate bool ValidateTypeFault(FaultException fault, out string reason);

        /// <summary>
        /// Method to check if fault absence is allowed.
        /// </summary>
        /// <param name="reason">Output verification dump.</param>
        /// <returns>True if "no fault" can be accepted, false otherwise.</returns>
        protected delegate bool ValidateNoFault(out string reason);
        
        /// <summary>
        /// Run step with more flexible fault validation.
        /// </summary>
        /// <param name="action">Action to be performed.</param>
        /// <param name="stepName">Step name.</param>
        /// <param name="validateFunction">Fault validation function.</param>
        /// <remarks>When no fault is received, validateFunction is called with null parameter.</remarks>
        protected void RunStep(Action action, string stepName, ValidateTypeFault validateFunction)
        {
            BeginStep(stepName);
            try
            {
                action();
                string reason;

                if (!validateFunction(null, out reason))
                {
                    AssertException ex = new AssertException(reason);
                    throw ex;
                }
            }
            catch (FaultException exc)
            {
                LogStepEvent("SOAP fault returned");
                LogStepEvent(string.Format("Code: {0}", exc.Code.Name));
                System.ServiceModel.FaultCode subCode = exc.Code.SubCode;
                while (subCode != null)
                {
                    LogStepEvent(string.Format("Subcode: {0}", subCode.Name));
                    subCode = subCode.SubCode;
                }

                string faultReason = string.Format("Reason: {0}", exc.Reason).Replace("\n", Environment.NewLine);
                LogStepEvent(faultReason);

                LogStepEvent("Validate fault...");
                string reason;
                bool faultOK = validateFunction(exc, out reason);

                if (faultOK)
                {
                    SaveStepFault(exc);
                }
                else
                {
                    AssertException ex = new AssertException(reason);
                    throw ex;
                }
            }
            StepPassed();
        }
        

        /// <summary>
        /// Processes request sent to the DUT.
        /// </summary>
        /// <param name="request">request sent to the DUT</param>
        public void LogRequest(string request)
        {
            if (_currentStep != null)
            {
                _currentStep.Request = request;
            }
            if (OnRequestSent != null)
            {
                OnRequestSent(request);
            }
        }

        /// <summary>
        /// Processes response received from the DUT.
        /// </summary>
        /// <param name="response">response received from the DUT</param>
        public void LogResponse(string response)
        {
            if (_currentStep != null)
            {
                _currentStep.Response = response;
            }
            if (OnResponseReceived != null)
            {
                OnResponseReceived(response);
            }
        }

        /// <summary>
        /// Notifies event handlers about step event.
        /// </summary>
        /// <param name="entry">Message to be "published"</param>
        protected void LogStepEvent(string entry)
        {
            if (OnStepEvent != null)
            {
                OnStepEvent(entry);
            }
        }

        /// <summary>
        /// Notifies event handlers about test-level event.
        /// </summary>
        /// <param name="entry">Message to be "published"</param>
        protected void LogTestEvent(string entry)
        {
            if (OnTestEvent != null)
            {
                OnTestEvent(entry);
            }
        }


        /// <summary>
        /// Marks current step as passed.
        /// </summary>
        protected void StepPassed()
        {
            if (_currentStep != null)
            {
                _currentStep.Status = StepStatus.Passed;
            }
            EndStep();
        }

        /// <summary>
        /// Saves fault in the step.
        /// </summary>
        /// <param name="fault">Fault exception</param>
        protected void SaveStepFault(FaultException fault)
        {
            if (_currentStep != null)
            {
                _currentStep.Exception = fault;
            }
        }


        /// <summary>
        /// Marks current step as failed.
        /// </summary>
        /// <param name="ex">Exception which caused step to fail.</param>
        protected void StepFailed(Exception ex)
        {
            if (_currentStep != null)
            {
                _currentStep.Exception = ex;

                FaultException exc = ex as FaultException;
                if (exc != null)
                {
                    _currentStep.Message = exc.Reason.ToString().Replace("\n", Environment.NewLine);
                }
                else
                {
                    _currentStep.Message = ex.Message.Replace("\n", Environment.NewLine);
                }
                _currentStep.Status = StepStatus.Failed;
                EndStep();
            }
        }
        
        /// <summary>
        /// Notifies event handlers about step completion.
        /// </summary>
        /// <param name="result">Step result.</param>
        private void ReportStepResult(StepResult result)
        {
            if (OnStepCompleted != null)
            {
                OnStepCompleted(result);
            }
        }

        /// <summary>
        /// Checks if a comdition is met.
        /// </summary>
        /// <param name="condition">Condidion.</param>
        protected void Assert(bool condition)
        {
            Assert(condition, "Assertion failed!");
        }

        /// <summary>
        /// Check if a condition is met
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <param name="message">Message to be saved in the log if condition is not met.</param>
        protected void Assert(bool condition, string message)
        {
            Assert(condition, message, "Check condition");
        }

        /// <summary>
        /// Check is a special condition is met.
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <param name="message">Message to be saved in the log if condition is not met.</param>
        /// <param name="stepName">Name of the step.</param>
        protected void Assert(bool condition, string message, string stepName)
        {
            Assert(condition, message, stepName, string.Empty);
        }

        /// <summary>
        /// Checks condition.
        /// </summary>
        /// <returns>True, if some conditions are met.</returns>
        protected delegate bool CheckCondition();

        /// <summary>
        /// Check if parameter passed is TRUE>
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <param name="message">Message to be used if condition is not met.</param>
        /// <param name="stepName">Step name.</param>
        /// <param name="stepDetails">Step details.</param>
        protected void Assert(bool condition, string message, string stepName, string stepDetails)
        {
            Assert(() => { return condition; }, message, stepName, stepDetails);
        }

        /// <summary>
        /// Check if a special condition is met.
        /// </summary>
        /// <param name="action">Action to check condition.</param>
        /// <param name="message">Message to be saved in the log if condition is not met.</param>
        /// <param name="stepName">Name of the step.</param>
        protected void Assert(CheckCondition action, string message, string stepName)
        {
            Assert(action, message, stepName, string.Empty);
        }

        /// <summary>
        /// Check if a special condition is met.
        /// </summary>
        /// <param name="action">Action to check condition.</param>
        /// <param name="message">Message to be saved in the log if condition is not met.</param>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="stepDetails">Details to be logged.</param>
        protected void Assert(CheckCondition action, string message, string stepName, string stepDetails)
        {
            BeginStep(stepName);
            if (!string.IsNullOrEmpty(stepDetails))
            {
                LogStepEvent(stepDetails);
            }
            bool condition = action();
            if (!condition)
            {
                _currentStep.Message = message;
                AssertException ex = new AssertException(message);
                StepFailed(ex);
                throw ex;
            }
            _currentStep.Message = string.Format("{0} - OK", stepName);
            StepPassed();

        }
        
        /// <summary>
        /// Logs step.
        /// </summary>
        /// <param name="stepName">Step name.</param>
        /// <param name="stepDetails">Step details.</param>
        protected void WriteStep(string stepName, string stepDetails)
        {
            BeginStep(stepName);
            if (!string.IsNullOrEmpty(stepDetails))
            {
                LogStepEvent(stepDetails);
            } 
            StepPassed();
        }

        /// <summary>
        /// Resets log
        /// </summary>
        protected void ResetLog()
        {
            _currentLog = new TestLog();
            _currentStepNum = 0;
        }
        
        /// <summary>
        /// Marks the test as failed.
        /// </summary>
        /// <param name="ex">Exception that caused the test to fail.</param>
        protected void TestFailed(Exception ex)
        {
            if (_currentLog != null)
            {
                bool allStepsPassed = _currentLog.Steps.Where(s => s.Status == StepStatus.Failed).Count() == 0;

                if (allStepsPassed)
                {
                    LogTestEvent(string.Format("Internal error occurred: {0}{1}", ex.Message, Environment.NewLine));
                    _currentLog.ErrorMessage = string.Empty;
                }
            }

            SetTestStatus(TestStatus.Failed);
            TestCompleted();
        }
        
        /// <summary>
        /// Completes the test.
        /// </summary>
        protected void TestCompleted()
        {
            ReportTestCompleted();
            _currentLog = null;
        }

        /// <summary>
        /// Changes status if the test.
        /// </summary>
        /// <param name="status"></param>
        protected void SetTestStatus(TestStatus status)
        {
            if (_currentLog != null)
            {
                _currentLog.TestStatus = status;
                _failed = (status == TestStatus.Failed);
            }
        }

        /// <summary>
        /// Notifies event handlers that the test is completed.
        /// </summary>
        private void ReportTestCompleted()
        {
            if (OnTestCompleted != null)
            {
                OnTestCompleted(_currentLog);
            }
        }

        /// <summary>
        /// Test method.
        /// </summary>
        private MethodInfo _entryPoint;
        /// <summary>
        /// Entry point.
        /// </summary>
        public MethodInfo EntryPoint
        {
            get { return _entryPoint; }
            set { _entryPoint = value; }
        }

        /// <summary>
        /// True if pause is requested.
        /// </summary>
        private bool _pause = false;

        /// <summary>
        /// Event which signals that user selects to resume the test.
        /// </summary>
        ManualResetEvent _resumeEvent = new ManualResetEvent(false);

        /// <summary>
        /// Stops the test immediately.
        /// </summary>
        public void Halt()
        {
            System.Diagnostics.Debug.WriteLine("--> BaseTest.Halt()");
            _semaphore.Stop();
        }

        /// <summary>
        /// Sets a flag indicating that a pause is requested.
        /// </summary>
        public void Pause()
        {
            System.Diagnostics.Debug.WriteLine("--> BaseTest.Pause()");
            _pause = true;
        }

        /// <summary>
        /// Resumes the test.
        /// </summary>
        public void Resume()
        {
            System.Diagnostics.Debug.WriteLine("--> BaseTest.Resume()");
            _resumeEvent.Set();
        }
        
        /// <summary>
        /// Starts the test.
        /// </summary>
        public void Start()
        {
            if (_entryPoint != null)
            {
                _entryPoint.Invoke(this, new object[0]);
            }
        }

        /// <summary>
        /// Exits the test if an exception occurs.
        /// </summary>
        /// <param name="ex">Exception.</param>
        public void ExitTest(Exception ex)
        {
            TestFailed(ex);
        }

        /// <summary>
        /// Waits for stop command or other handle.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        protected int WaitForResponse(WaitHandle[] events)
        {
            int handle = -1;
            while (true)
            {
                List<WaitHandle> handles = new List<WaitHandle> {_semaphore.StopEvent};
                handles.AddRange(events);
                handle = WaitHandle.WaitAny(handles.ToArray());

                switch (handle)
                {
                    case 0:
                        System.Diagnostics.Debug.WriteLine("Halt event set");
                        throw new StopEventException();
                    default:
                        if (handle > 0)
                        {
                            System.Diagnostics.Trace.WriteLine(string.Format("{0}, Asynchronous method completed", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
                            return handle - 1;//return position of handle in param array
                        }
                        break;
                }
            }
        }
        
        /// <summary>
        /// Perfors delay. Handles "Stop" command.
        /// </summary>
        /// <param name="timeout"></param>
        protected void Sleep(int timeout)
        {
            WaitHandle[] handles;
            handles = new WaitHandle[] { _semaphore.StopEvent };

            int handle = System.Threading.WaitHandle.WaitAny(handles, timeout);

            if (handle == 0)
            {
                System.Diagnostics.Debug.WriteLine("Stop event");
                _semaphore.ReportStop();
            }

        }

        protected void DoRequestDelay()
        {
            Sleep(_recoveryDelay);
        }

        /// <summary>
        /// Executed in BeginTest()
        /// </summary>
        protected virtual void Initialize()
        {

        }

        /// <summary>
        /// Executed in EndTest() (before TestCompleted event )
        /// </summary>
        protected virtual void Release()
        {

        }
        
    }

}
