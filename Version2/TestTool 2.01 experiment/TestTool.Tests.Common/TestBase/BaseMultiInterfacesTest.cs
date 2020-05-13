///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.HttpTransport;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.Trace;
using System.ServiceModel.Channels;
using TestTool.Tests.Common.TestEngine;
using System.ServiceModel.Description;

namespace TestTool.Tests.Common.TestBase
{
    public abstract class BaseMultiInterfacesTest : BaseTest
    {
        /// <summary>
        /// Creates BaseServiceTest instance.
        /// </summary>
        /// <param name="param">Parameters for test run.</param>
        protected BaseMultiInterfacesTest(TestLaunchParam param)
            : base(param)
        {
            
        }
        
        /// <summary>
        /// Creates binding.
        /// </summary>
        /// <returns>Binding for custom HttpTransport.</returns>
        protected Binding CreateBinding()
        {
            return CreateBinding(false);
        }

        /// <summary>
        /// Creates binding.
        /// </summary>
        /// <returns>Binding for custom HttpTransport.</returns>
        protected Binding CreateBinding(bool includeAddressController)
        {
            IChannelController[] controllers;
            // add mandatory controllers.
            // _trafficListener is used to monitor data sent and received via Client.
            // _semaphore is used to stop waiting for the answer.
            if (includeAddressController)
            {
                controllers = new IChannelController[] { _trafficListener, _semaphore, _endpointController };
            }
            else
            {
                controllers = new IChannelController[] { _trafficListener, _semaphore };
            }
            Binding binding = new HttpBinding(controllers);
            return binding;
        }


        /// <summary>
        /// Performas initialization object for test run.
        /// </summary>
        protected void BeginTest()
        {
            BeginTest(true);
        }

        /// <summary>
        /// Performas initialization object for test run.
        /// </summary>
        /// <param name="resetLog">True, if log should be reset.</param>
        protected void BeginTest(bool resetLog)
        {
            if (resetLog)
            {
                ResetLog();
            }
            try
            {
                // Initialization
            }
            catch (Exception exc)
            {
                _currentLog.TestStatus = TestStatus.Failed;
                _failed = true;
                _currentLog.ErrorMessage = exc.Message;
                throw;
            }

        }

        /// <summary>
        /// Ends the test.
        /// </summary>
        /// <param name="status"></param>
        protected void EndTest(TestStatus status)
        {
            SetTestStatus(status);
            TestCompleted();
            CloseConnections();
        }

        protected abstract void CloseConnections();

        /// <summary>
        /// Perform common initialization/finailzation. Run test action.
        /// </summary>
        /// <param name="action">Test "body"</param>
        protected void RunTest(Action action)
        {
            try
            {
                BeginTest();

                action();

                EndTest(TestStatus.Passed);
            }
            catch (StopEventException)
            {
                LogStepEvent("Halted");
            }
            catch (Exception ex)
            {
                StepFailed(ex);
                TestFailed(ex);
            }
        }
        
        /// <summary>
        /// Perform common initialization/finailzation. Performs backup action before running the test;  
        /// run test and restores device settings (independently from passed/failed test status).
        /// </summary>
        /// <typeparam name="T">Type of backup data.</typeparam>
        /// <param name="backupAction">Method to backup device settings.</param>
        /// <param name="action">Test method.</param>
        /// <param name="restoreAction">Method to restore device settings.</param>
        protected void RunTest<T>(Backup<T> backupAction,
                                  Action action,
                                  Action<T> restoreAction)
        {
            T data = default(T);
            Exception exc = null;
            bool halted = false;
            bool backupNotNull = false;

            try
            {
                System.Diagnostics.Debug.WriteLine("Begin test");

                // initialize object for test run;
                BeginTest();

                System.Diagnostics.Debug.WriteLine("Backup");

                // backup device settings
                data = backupAction();
                backupNotNull = true;

                System.Diagnostics.Debug.WriteLine("Main action");

                // run test
                action();

                System.Diagnostics.Debug.WriteLine("Main action completed");
                
            }
            catch (StopEventException)
            {
                System.Diagnostics.Debug.WriteLine("HALT");
                LogStepEvent("Halted");
                halted = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Exception: {0}", ex.Message));
                StepFailed(ex);
                exc = ex;
            }
            finally
            {
                try
                {
                    if (backupNotNull && !halted)
                    {
                        System.Diagnostics.Debug.WriteLine("Restore");
                        restoreAction(data);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No backup data");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception when trying to restore settings");
                    StepFailed(ex);
                    exc = ex;
                }

                if (!halted)
                {
                    if (exc == null)
                    {
                        System.Diagnostics.Debug.WriteLine("End test - passed");
                        EndTest(TestStatus.Passed);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Test failed");
                        TestFailed(exc);
                    }
                }
            }
        }

        /// <summary>
        /// Sets up the channel to spoil the messages (for fault tests)
        /// </summary>
        /// <param name="spoiler">Object to process the message.</param>
        protected void SetBreakingBehaviour(ServiceEndpoint endpoint, MessageSpoiler spoiler)
        {
                Binding binding = endpoint.Binding;
                TestTool.HttpTransport.HttpTransportBindingElement traceElement = (HttpTransport.HttpTransportBindingElement)binding.CreateBindingElements()[1];

                if (spoiler != null)
                {
                    traceElement.Contollers.Add(spoiler);
                }
                else
                {
                    traceElement.Contollers.RemoveAll(c => c is ISoapMessageMutator);
                }

        }

        /// <summary>
        /// Resets channel to pass packets "as is"
        /// </summary>
        protected void ResetBreakingBehaviour(ServiceEndpoint endpoint)
        {
            SetBreakingBehaviour(endpoint, null);
        }
    }

}
