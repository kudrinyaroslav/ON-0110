using System;
using System.Threading;
using System.Reflection;
using CameraClient.Log;

namespace CameraClient.Tests
{
    class TestDispatcher
    {
        private TestSuiteParameters _parameters;
        private Thread _thread;

        private bool _stop;

        public event Action<Exception> OnException;

        public event Action OnTestSuiteCompleted;

        public event StepCompleted OnStepCompleted;
        
        public event TestCompleted OnTestCompleted;

        public void Run(TestSuiteParameters parameters)
        {
            _parameters = parameters;
            _stop = false;

            _thread = new Thread(RunTests);
            _thread.Start();
        }

        private ITestSuite _currentTest;

        void RunTests()
        {
            Type[] types = new Type[]{typeof(string)};

            object[] args = new object[]{_parameters.Address};
            
            foreach (MethodInfo mi in _parameters.TestCases)
            {
                if (_stop)
                {
                    break;
                }
                
                try
                {
                    _currentTest = null;
                    Type t = mi.DeclaringType;
                    System.Reflection.ConstructorInfo ci =
                        t.GetConstructor(
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
                        null, 
                        System.Reflection.CallingConventions.HasThis, 
                        types, 
                        null);

                    object itObject = ci.Invoke(args);
                    // todo: check if t is descendant of ITestSuite
                    BaseTest test = (BaseTest) itObject;
                    test.OnStepCompleted += new StepCompleted(test_OnStepCompleted);
                    test.OnTestCompleted += new TestCompleted(test_OnTestCompleted);
                    _currentTest = itObject as TestSuite;

                    //System.Diagnostics.Debug.WriteLine(string.Format("{0}, Sleep 10 seconds before running test", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
                    //Thread.Sleep(10000);
                    //System.Diagnostics.Debug.WriteLine(string.Format("{0}, Run test", System.DateTime.Now.ToString("HH:mm:ss ffffff")));
                    
                    if (_delayedPause)
                    {
                        _currentTest.Pause();
                    }
                    if (_delayedHalt)
                    {
                        _currentTest.Halt();
                    }

                    mi.Invoke(itObject, new object[0]);
                }
                catch(System.Reflection.TargetException exc)
                {
                    _currentTest.ReportMethodInvocationException(mi, exc);
                    ReportException(exc);
                }
                catch( System.ArgumentException exc)
                {
                    _currentTest.ReportMethodInvocationException(mi, exc);
                    ReportException(exc);
                }
                //catch (System.Reflection.TargetInvocationException exc)
                //{
                //    _testSuite.ReportMethodInvocationException(mi, exc);
                //    ReportException(exc);
                //}                
                catch (System.Reflection.TargetParameterCountException exc)
                {
                    _currentTest.ReportMethodInvocationException(mi, exc);
                    ReportException(exc);
                }                    
                catch (System.MethodAccessException exc)
                {
                    _currentTest.ReportMethodInvocationException(mi, exc);
                    ReportException(exc);
                }
                catch (System.InvalidOperationException exc)
                {
                    _currentTest.ReportMethodInvocationException(mi, exc);
                    ReportException(exc);
                }
                catch (Exception exc)
                {
                    //
                    // TestFailed currently does not throw any exceptions
                    // 
                    // ToDo : ???
                    // possibly, write some info in log ? But TestRunner knows nothing about log
                    // and may be it's OK that all logs are done in corresponding methods...
                    //

                    // ... so, may be all exceptions are from Invoke(...) ?
                    ReportException(exc);
                }
            }

            if (OnTestSuiteCompleted != null)
            {
                OnTestSuiteCompleted();
            }
        }

        void test_OnTestCompleted(TestLog log)
        {
            if (OnTestCompleted != null)
            {
                OnTestCompleted(log);
            }
        }

        void test_OnStepCompleted(StepResult result)
        {
            if (OnStepCompleted != null)
            {
                OnStepCompleted(result);
            }
        }

        void ReportException(Exception ex)
        {
            if (OnException != null)
            {
                OnException(ex);
            }
        }

        public void Stop()
        {
            _stop = true;
        }

        public void Pause()
        {
            if (_currentTest != null)
            {
                _currentTest.Pause();
            }
            else
            {
                _delayedPause = true;
            }
        }

        public void Resume()
        {
            _delayedPause = false;
            if (_currentTest != null)
                _currentTest.Resume();
        }

        public void Halt()
        {
            if (_currentTest != null)
            {
                _currentTest.Halt();
            }
            else
            {
                _delayedHalt = true;
            }
        }

        private bool _delayedPause;
        private bool _delayedHalt;

    }
}
