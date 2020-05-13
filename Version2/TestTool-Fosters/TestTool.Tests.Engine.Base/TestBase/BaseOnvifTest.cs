using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using TestTool.HttpTransport;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Trace;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.Engine.Base.TestBase
{
    public class BaseOnvifTest : BaseTest
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
        /// Object to interact with test operator 
        /// </summary>
        protected IOperator _operator;

        /// <summary>
        /// UI to display video.
        /// </summary>
        protected IVideoForm _videoForm;


        /// <summary>
        /// Time delay for operation which can require some additional time.
        /// </summary>
        protected int _operationDelay;

        /// <summary>
        /// Time after sending request (enough e.g. if a DUT decided to restart).
        /// </summary>
        protected int _recoveryDelay;

        /// <summary>
        /// Endpoint controller (used to control endpoint address )
        /// </summary>
        protected EndpointController _endpointController;
        /// <summary>
        /// Credentials provider.
        /// </summary>
        protected CredentialsProvider _credentialsProvider;

        protected Security _security;

        /// <summary>
        /// Features selected for test.
        /// </summary>
        private List<Feature> _features = new List<Feature>();

        /// <summary>
        /// Features selected by the operator.
        /// </summary>
        public List<Feature> Features
        {
            get { return _features; }
        }


        protected BaseOnvifTest(TestLaunchParam param)
            : base(param)
        {
            _features = param.Features;

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

            _messageTimeout = param.MessageTimeout;

            _operationDelay = param.OperationDelay;
            _recoveryDelay = param.RecoveryDelay;

            _rebootTimeout = param.RebootTimeout;

            _trafficListener = new TrafficListener();
            _trafficListener.RequestSent += LogRequest;
            _trafficListener.ResponseReceived += LogResponse;

            _endpointController = new EndpointController(new EndpointAddress(_cameraAddress));

            _credentialsProvider = new CredentialsProvider();
            _credentialsProvider.Username = param.UserName;
            _credentialsProvider.Password = param.Password;
            _credentialsProvider.Security = param.Security;

        }


        protected void DoRequestDelay()
        {
            Sleep(_recoveryDelay);
        }

        /// <summary>
        /// Creates binding without address controller.
        /// </summary>
        /// <returns></returns>
        protected Binding CreateBinding()
        {
            return CreateBinding(false);
        }

        /// <summary>
        /// Creates binding.
        /// </summary>
        /// <param name="includeAddressController">True if address controller should be included.</param>
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
        /// Adds SecurityBehavior to endpoint.
        /// </summary>
        /// <param name="endpoint">Endpoint.</param>
        protected void AttachSecurity(System.ServiceModel.Description.ServiceEndpoint endpoint)
        {
            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
            {
                SecurityBehavior securityBehavior = new SecurityBehavior();
                securityBehavior.CredentialsProvider = _credentialsProvider;
                endpoint.Behaviors.Add(securityBehavior);
            }
        }

        /// <summary>
        /// Sets up channel.
        /// </summary>
        /// <param name="channel">Channel.</param>
        protected void SetupChannel(IClientChannel channel)
        {
            channel.OperationTimeout = new TimeSpan(0, 0, 0, 0, _messageTimeout);
        }


        /// <summary>
        /// Sets up the channel to spoil the messages (for fault tests)
        /// </summary>
        /// <param name="spoiler">Object to process the message.</param>
        /// <param name="endpoint"></param>
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
                Initialize();
                // other configuration
            }
            catch (Exception exc)
            {
                _currentLog.TestStatus = TestStatus.Failed;
                _failed = true;
                if (!InStep && (_currentLog.Steps.Count == 0))
                {
                    _currentLog.ErrorMessage = exc.Message;
                }
                throw;
            }

        }

        /// <summary>
        /// Ends the test.
        /// </summary>
        /// <param name="status"></param>
        protected void EndTest(TestStatus status)
        {
            Release();
            SetTestStatus(status);
            TestCompleted();
        }


        /// <summary>
        /// Runs test.
        /// </summary>
        /// <param name="action">Action to be performed.</param>
        protected void RunTest(Action action)
        {
            RunTest(action, null);
        }

        /// <summary>
        /// Perform common initialization/finalization. Run test action.
        /// </summary>
        /// <param name="action">Test "body"</param>
        /// <param name="cleanUpAction">Clean-up </param>
        protected void RunTest(Action action, Action cleanUpAction)
        {
            _halted = false;
            Exception exc = null;

            try
            {
                BeginTest();

                action();

                if (cleanUpAction == null)
                {
                    EndTest(TestStatus.Passed);
                    return;
                }
            }
            catch (StopEventException)
            {
                LogStepEvent("Halted");
                _halted = true;
            }
            catch (Exception ex)
            {
                StepFailed(ex);
                if (cleanUpAction == null)
                {
                    TestFailed(ex);
                    return;
                }
                else
                {
                    exc = ex;
                }
            }

            if (cleanUpAction != null && !_halted)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Clean up");
                    cleanUpAction();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception when trying to restore settings");
                    StepFailed(ex);
                    exc = ex;
                }

                if (exc == null)
                {
                    System.Diagnostics.Debug.WriteLine("End test - passed");
                    EndTest(TestStatus.Passed);
                }
                else
                {
                    Release();
                    System.Diagnostics.Debug.WriteLine("Test failed");
                    TestFailed(exc);
                }
            }
            else
            {
                if (_halted)
                {
                    Release();
                }
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
            bool backupNotNull = false;
            _halted = false;

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
                _halted = true;
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
                    if (backupNotNull && !_halted)
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

                Release();
                if (!_halted)
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




    }
}
