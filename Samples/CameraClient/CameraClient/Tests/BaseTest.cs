using System;
using CameraClient.Log;
using System.Reflection;
using System.Threading;

namespace CameraClient.Tests
{
    class BaseTest : ITestSuite
    {
        protected string _cameraAddress;
        private StepResult _currentStep;
        private int _currentStepNum;
        protected TestLog _currentLog;
        public event StepCompleted OnStepCompleted;
        public event TestCompleted OnTestCompleted;
        
        protected BaseTest(string address)
        {
            _cameraAddress = address;

        }

        protected void BeginStep()
        {
            BeginStep(string.Empty);
        }

        // call in concrete ServiceTest before method is called
        // or in Assert
        protected void BeginStep(string stepName)
        {
            _currentStepNum++;
            _currentStep = new StepResult { Number = _currentStepNum, StepName = stepName, Status = StepStatus.Passed};
        }

        // call in BaseServiceTest
        protected void LogRequest(System.ServiceModel.Channels.Message request)
        {
            if (_currentStep != null)
            {
                _currentStep.Request = request.ToString();
            }
        }

        // call in BaseServiceTest
        protected void LogResponse(System.ServiceModel.Channels.Message response)
        {
            if (_currentStep != null)
            {
                _currentStep.Response = response.ToString();
            }
        }

        // call in ServiceTest after method is called
        // or in Assert
        private void EndStep()
        {
            if (_currentLog != null)
            {
                _currentLog.Steps.Add(_currentStep);
            }
            ReportStepResult(_currentStep);
            _currentStep = null;
        }

        // call in ServiceTest after method is called
        // or in Assert
        protected void StepPassed()
        {
            if (_currentStep != null)
            {
                _currentStep.Status = StepStatus.Passed;
            }
            EndStep();
        }

        // call in concrete "step" in ServiceTest if an exception occurred
        // includes EndStep 
        protected void StepFailed(Exception ex)
        {
            // 
            // ToDo : check TimeOut exception
            // may be check AssertException

            if (_currentStep != null)
            {
                _currentStep.Exception = ex;
                _currentStep.Status = StepStatus.Failed;
                EndStep();
            }
        }
        
        private void ReportStepResult(StepResult result)
        {
            if (OnStepCompleted != null)
            {
                OnStepCompleted(result);
            }
        }

        // special "Step"
        protected void Assert(bool condition)
        {
            Assert(condition, "Assertion failed!");
        }

        // special "Step"
        protected void Assert(bool condition, string message)
        {
            BeginStep("Check condition");
            if (!condition)
            {
                _currentStep.Message = message;
                AssertException ex = new AssertException(message);
                StepFailed(ex);
                throw ex;
            }
            _currentStep.Message = "Assertion OK";
            StepPassed();

        }

        // another special step = "Verify that answer contains fault..."

        // call in BaseServiceTest (for internal use only)
        protected void ResetLog()
        {
            _currentLog = new TestLog();
            _currentStepNum = 0;
        }

        public void ReportMethodInvocationException(MethodInfo methodInfo, Exception exception)
        {
            Exception ex = new Exception(string.Format("Method {0} is not valid test method. Exception was thrown during invocation: {1}", 
                methodInfo.Name, exception.Message), 
                exception);
            // problem: test if method is incorrect, no step is started at this point.
            StepFailed(ex);
            // possibly all we need is this line.
            TestFailed(ex);
        }
        
        // called in concrete descendant of BaseServiceTest
        protected void TestFailed(Exception ex)
        {
            // ToDo : write some more information in log 
            TestCompleted();
        }
        
        protected void TestCompleted()
        {
            ReportTestCompleted();
            _currentLog = null;
        }

        private void ReportTestCompleted()
        {
            if (OnTestCompleted != null)
            {
                OnTestCompleted(_currentLog);
            }
        }
        
        ManualResetEvent _haltEvent = new ManualResetEvent(false);

        ManualResetEvent _pauseEvent = new ManualResetEvent(false);

        ManualResetEvent _resumeEvent = new ManualResetEvent(false);

        public void Halt()
        {
            _haltEvent.Set();
        }

        public void Pause()
        {
            _pauseEvent.Set();
        }

        public void Resume()
        {
            _resumeEvent.Set();
        }
        
        protected bool WaitForSomething(IAsyncResult result)
        {
            while (true)
            {
                int handle = WaitHandle.WaitAny(new WaitHandle[] { result.AsyncWaitHandle, _haltEvent, _pauseEvent });
                switch (handle)
                {
                    case 0:
                        System.Diagnostics.Debug.WriteLine(string.Format("{0}, Asynchronous method completed", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
                        return true;
                        break;
                    case 1:
                        System.Diagnostics.Debug.WriteLine("Halt event set");
                        throw new StopEventException();
                        break;
                    case 2:
                        System.Diagnostics.Debug.WriteLine("Pause event set");
                        _pauseEvent.Reset();
                        handle = WaitHandle.WaitAny(new WaitHandle[] { _haltEvent, _resumeEvent });
                        if (handle == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("Halt event set");
                            throw new StopEventException();
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Resume event set");
                            _resumeEvent.Reset();
                        }
                        break;
                }
            }

            return true;
        }

    }

}
