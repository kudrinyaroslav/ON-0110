///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Trace;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine.Base.Definitions;
using System.Text;

namespace TestTool.Tests.Engine.Base.TestBase
{
    public delegate void NetworkSettingsChanged(string newServiceAddress);
    public delegate void SecurityChanged(CredentialsProvider credentialsProvider);

    /// <summary>
    /// Base test (== class with methods referring to tests from specification)
    /// </summary>
    public class BaseTest : ITest
    {
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
        /// Object to stop waiting for the answer.
        /// </summary>
        protected TestSemaphore _semaphore;
        public TestSemaphore Semaphore { get { return _semaphore; } }
        public bool StopRequested()
        {
            if (_semaphore.StopRequested)
                _semaphore.ReportStop();

            return false;
        }

        // Raised when camera ip and service address are changed
        public event NetworkSettingsChanged NetworkSettingsChangedEvent;
        public void RaiseNetworkSettingsChangedEvent(string newServiceAddress)
        {
            if (NetworkSettingsChangedEvent != null)
                NetworkSettingsChangedEvent(newServiceAddress);
        }

        // Raised when camera ip and service address are changed
        public event SecurityChanged SecurityChangedEvent;
        public void RaiseSecurityChangedEvent(CredentialsProvider credentialsProvider)
        {
            if (SecurityChangedEvent != null)
                SecurityChangedEvent(credentialsProvider);
        }

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

        protected bool _halted;
        
        public bool Halted
        {
            get { return _halted; }
        }
    

        /// <summary>
        /// Object to catch outgoing and incoming messages.
        /// </summary>
        protected TrafficListener _trafficListener;
        public TrafficListener Traffic { get { return _trafficListener; } }


        /// <summary>
        /// Constructor. Performs initialization.
        /// </summary>
        /// <param name="param"></param>
        protected BaseTest(BaseTestParam param)
        {
            _semaphore = new TestSemaphore();

        }

        public void SetNesting(BaseTest test)
        {
            test.OnStepStarted += new Action<StepResult>(test_OnStepStarted);
            test.OnStepCompleted += new StepCompleted(test_OnStepCompleted);
        }

        void test_OnStepStarted(StepResult result)
        {
            
            _currentStep = result;
            _currentStep.Number = ++_currentStepNum;

            if (OnStepStarted != null)
            {
                OnStepStarted(_currentStep);
            }
        }

        void test_OnStepCompleted(StepResult result)
        {
            if (OnStepCompleted != null)
            {
                OnStepCompleted(result);
            }
        }

        /// <summary>
        /// Indicates that step is open.
        /// </summary>
        public bool InStep 
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
        public void BeginStep(string stepName)
        {
            lock (_locker)
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

        object _locker = new object();

        /// <summary>
        /// Marks current step as completed.
        /// </summary>
        private void EndStep()
        {
            lock (_locker)
            {
                if (_currentLog != null)
                {
                    _currentLog.Steps.Add(_currentStep);
                }
                try
                {
                    // something strange here may rise exception
                    ReportStepResult(_currentStep);
                }
                catch (Exception)
                {
                    //MessageBox.Show("Hello!");
                }; 
                _currentStep = null;
                if (_pause)
                {
                    System.Diagnostics.Debug.WriteLine("EndStep - pause requested.");
                    DoPause();
                }
            }
        }

        /// <summary>
        /// Perform a step.
        /// </summary>
        /// <param name="action">Action to be performed in a step.</param>
        /// <param name="stepName">Step name</param>
        public void RunStep(Action action, string stepName)
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
        /// <param name="acceptOtherFaults">True if WARNING is generated on other faults.</param>
        public void RunStep(Action action, string stepName, string expectedFault, bool acceptOtherFaults)
        {
            RunStep(action, stepName, expectedFault, acceptOtherFaults, false);
        }

        /// <summary>
        /// Run step with a fault allowed.
        /// </summary>
        /// <param name="action">Action to b performed.</param>
        /// <param name="stepName">Step name.</param>
        /// <param name="expectedFault">Expected fault. </param>
        public void RunStepAllowFault(Action action, string stepName, string expectedFault)
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
        /// <param name="acceptOtherFaults">Indicates if another fault can be accepted
        public void RunStep(Action action, 
                            string stepName, 
                            string expectedFault, 
                            bool acceptOtherFaults, 
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
                LogFault(exc);

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
                        if(!acceptOtherFaults)
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

        //protected static void RunStep(BaseTest test, 
        //    Action action,
        //    string stepName,
        //    string expectedFault,
        //    bool acceptOtherFaults,
        //    bool allowNoFault)
        //{
        //    test.RunStep(action, stepName, expectedFault, acceptOtherFaults, allowNoFault);
        //}

        public static void RunStep(BaseTest test,
            Action action,
            string stepName)
        {
            test.RunStep(action, stepName);
        }


        protected void LogFault(FaultException exc)
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
            //
            // changes from 08/11/2011:
            // when Digest authentication is used, some operations require two requests (no challenge or 
            // stale nonce)
            // Channel notifies about all.
            // Test saves only last.
            // TestDispatcher is notified only about first.
            //
            bool requestSaved = false;
            if (_currentStep != null)
            {
                requestSaved = !string.IsNullOrEmpty(_currentStep.Request);
                _currentStep.Request = request;
            }
            Action<string> requestSent = OnRequestSent;
            if (requestSent != null && !requestSaved)
            {
                requestSent(request);
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
            Action<string> responseReceived = OnResponseReceived;
            if (responseReceived != null)
            {
                responseReceived(response);
            }
        }

        /// <summary>
        /// Notifies event handlers about step event.
        /// </summary>
        /// <param name="entry">Message to be "published"</param>
        public void LogStepEvent(string entry)
        {
            Action<string> stepEvent = OnStepEvent;
            if (stepEvent != null)
            {
                stepEvent(entry);
            }
        }

        /// <summary>
        /// Notifies event handlers about test-level event.
        /// </summary>
        /// <param name="entry">Message to be "published"</param>
        protected void LogTestEvent(string entry)
        {
            Action<string> testEvent = OnTestEvent;
            if (testEvent != null)
            {
                testEvent(entry);
            }
        }

        static public void LogTestEvent(BaseOnvifTest test, string entry)
        {
            test.LogTestEvent(entry);
        }

        /// <summary>
        /// Marks current step as passed.
        /// </summary>
        public void StepPassed()
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
        public void StepFailed(Exception ex)
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
                    if (!ex.Message.Contains("\r\n"))
                    {
                        _currentStep.Message = ex.Message.Replace("\n", Environment.NewLine);
                    }
                    else
                    {
                        _currentStep.Message = ex.Message;
                    }  
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
            StepCompleted stepCompleted = OnStepCompleted;
            if (stepCompleted != null)
            {
                stepCompleted(result);
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
        public void Assert(bool condition, string message, string stepName)
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
        public void Assert(bool condition, string message, string stepName, string stepDetails)
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
                bool allStepsPassed = _currentLog.Steps.Where(s => ((s == null) || (s.Status == StepStatus.Failed))).Count() == 0;

                if (allStepsPassed)
                {
                    LogTestEvent(string.Format("Internal error occurred: {0}{1}", ex.Message, Environment.NewLine));
                    LogTestEvent(string.Format("Internal error occurred: {0}{1}", ex.StackTrace, Environment.NewLine));
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
            TestCompleted testCompleted = OnTestCompleted;
            if (testCompleted != null)
            {
                testCompleted(_currentLog);
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
        public virtual void Halt()
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
        public int WaitForResponse(WaitHandle[] events)
        {
            int handle = -1;
            while (true)
            {
                var handles = new List<WaitHandle> {_semaphore.StopEvent};
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
        public void Sleep(int timeout)
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

        protected int Sleep(int timeout, IEnumerable<WaitHandle> waitHandles)
        {
            List<WaitHandle> handles = new List<WaitHandle>();
            handles.AddRange(waitHandles);
            handles.Add(_semaphore.StopEvent);

            int handle = System.Threading.WaitHandle.WaitAny(handles.ToArray(), timeout);

            if (handle == handles.Count-1)
            {
                System.Diagnostics.Debug.WriteLine("Stop event");
                _semaphore.ReportStop();
            }
            return handle;
        }

        public void AttachListenerTimeFilter(TrafficListener Listener)
        {
            Listener.MessageProcessingTime += i =>
            {
                if (null != _currentStep && 0 != i &&
                    !string.IsNullOrEmpty(_currentStep.Request) && TrafficListener.untrackedRequests.All(e => !_currentStep.Request.Contains(e)))
                {
                    //if (_currentStep.ProcessingTimeSpecified)
                    //    LogStepEvent(string.Format("Processing time for step '{0}' is already specified: {1}.", _currentStep.StepName, _currentStep.ProcessingTime));

                    _currentStep.ProcessingTimes.Add(i);
                }
            };
        }

        /// <summary>
        /// Executed in BeginTest()
        /// </summary>
        protected virtual void Initialize()
        {
            _trafficListener = new TrafficListener();
            _trafficListener.RequestSent += LogRequest;
            _trafficListener.ResponseReceived += LogResponse;
            AttachListenerTimeFilter(_trafficListener);
        }

        protected virtual void Initialize(BaseTest test)
        {
            _trafficListener = new TrafficListener();
            _trafficListener.RequestSent += test.LogRequest;
            _trafficListener.ResponseReceived += test.LogResponse;
            test.AttachListenerTimeFilter(_trafficListener);
        }

        /// <summary>
        /// Executed in EndTest() (before TestCompleted event )
        /// </summary>
        protected virtual void Release()
        {
            _semaphore.StopEvent.Close();
            _trafficListener.RequestSent -= LogRequest;
            _trafficListener.ResponseReceived -= LogResponse;
        }

        public virtual void Release(BaseTest test)
        {
            _semaphore.StopEvent.Close();
            _trafficListener.RequestSent -= test.LogRequest;
            _trafficListener.ResponseReceived -= test.LogResponse;
        }

        protected void FillRtspRequest(string rtspRequest)
        {
            if (_currentStep.StepName.Contains("OPTIONS") ||
                _currentStep.StepName.Contains("DESCRIBE") ||
                _currentStep.StepName.Contains("SETUP") ||
                _currentStep.StepName.Contains("PLAY") ||
                _currentStep.StepName.Contains("PAUSE") ||
                _currentStep.StepName.Contains("TEARDOWN"))
            {
                _currentStep.Request = rtspRequest;
            }
        }

        protected void FillRtspResponse(string rtspResponse)
        {
            if (_currentStep.StepName.Contains("OPTIONS") ||
                _currentStep.StepName.Contains("DESCRIBE") ||
                _currentStep.StepName.Contains("SETUP") ||
                _currentStep.StepName.Contains("PLAY") ||
                _currentStep.StepName.Contains("PAUSE") ||
                _currentStep.StepName.Contains("TEARDOWN"))
            {
                _currentStep.Response = rtspResponse;
            }
        }

        protected string ConvertToFaultCode(string faultCode)
        {
            string[] codes = faultCode.Split('/');
            string expectedSequence = "";
            if (codes.Length != 0)
            {
                StringBuilder expected = new StringBuilder(string.Format("env:{0}", codes[0]));
                for (int i = 1; i < codes.Length; i++)
                {
                    expected.AppendFormat("/ter:{0}", codes[i]);
                }

                expectedSequence = expected.ToString();
            }
            return expectedSequence;
        }
    }

}
