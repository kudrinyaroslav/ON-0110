///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using TestTool.HttpTransport;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Definitions.Exceptions;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.Engine.Base.TestBase
{
    /// <summary>
    /// Abstract class which uses several services.
    /// </summary>
    public abstract class BaseMultiInterfacesTest : BaseOnvifTest
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
        /// Creates binding with address controller, if specified.
        /// </summary>
        /// <param name="includeAddressController"></param>
        /// <returns></returns>
        protected Binding CreateBinding(bool includeAddressController)
        {
            return CreateBinding(includeAddressController, null);
        }

        /// <summary>
        /// Creates binding.
        /// </summary>
        /// <returns>Binding for custom HttpTransport.</returns>
        protected Binding CreateBinding(bool includeAddressController,
            IEnumerable<IChannelController> customControllers)
        {
            List<IChannelController> controllers = new List<IChannelController>();
            if (customControllers != null)
            {
                controllers.AddRange(customControllers);
            }
            // add mandatory controllers.
            // _trafficListener is used to monitor data sent and received via Client.
            // _semaphore is used to stop waiting for the answer.
            if (includeAddressController)
            {
                controllers.AddRange(new IChannelController[] { _trafficListener, _semaphore, _credentialsProvider, _endpointController });
            }
            else
            {
                controllers.AddRange(new IChannelController[] { _trafficListener, _semaphore, _credentialsProvider });
            }
            Binding binding = new HttpBinding(controllers);
            return binding;
        }

        protected Binding CreateBinding(IEnumerable<IChannelController> controllers)
        {
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

        /// <summary>
        /// Closes all connections.
        /// </summary>
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
        protected void SetBreakingBehaviour(ServiceEndpoint endpoint, ISoapMessageMutator spoiler)
        {
                Binding binding = endpoint.Binding;
                TestTool.HttpTransport.ControlledTextMessageBindingElement traceElement = (HttpTransport.ControlledTextMessageBindingElement)binding.CreateBindingElements()[0];

                if (spoiler != null)
                {
                    traceElement.Controllers.Add(spoiler);
                }
                else
                {
                    traceElement.Controllers.RemoveAll(c => c is ISoapMessageMutator);
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
