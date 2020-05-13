using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading;
using TestTool.HttpTransport;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.InternalLogger;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;
using IPAddress = System.Net.IPAddress;

namespace TestTool.Tests.Engine.Base.TestBase
{
    public class BaseOnvifTest : BaseTest
    {
        /// <summary>
        /// Network interface data.
        /// </summary>
        protected NetworkInterfaceDescription _nic;
        public NetworkInterfaceDescription Nic { get { return _nic; } }

        private bool InternalLog = false;

        /// <summary>
        /// Environment settings.
        /// </summary>
        protected EnvironmentSettings _environmentSettings;
        /// <summary>
        /// Camera UUID.
        /// </summary>
        protected string _cameraId;
        public string CameraID
        {
            get { return _cameraId; }
        }
        /// <summary>
        /// Camera service address.
        /// </summary>
        //protected string _cameraAddress;
        public string CameraAddress
        {
            get { return _endpointController.Address.ToString(); }
        }
        /// <summary>
        /// Camera IP address.
        /// </summary>
        protected IPAddress _cameraIp;
        public IPAddress CameraIP
        {
            get { return _cameraIp; }
        }
        /// <summary>
        /// Message timeout.
        /// </summary>
        protected int _messageTimeout;
        public int MessageTimeout
        {
            get { return _messageTimeout; }
        }
        /// <summary>
        /// Reboot timeout.
        /// </summary>
        protected int _rebootTimeout;
        public int RebootTimeout
        {
            get { return _rebootTimeout; }
        }
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
        
        public string FirmwareFilePath { get; protected set; }
        
        public CredentialIdentifierValue CredentialIdentifierValueFirst { get; protected set; }
        public CredentialIdentifierValue CredentialIdentifierValueSecond { get; protected set; }
        public CredentialIdentifierValue CredentialIdentifierValueThird { get; set; }

        /// <summary>
        /// Object to interact with test operator 
        /// </summary>
        protected IOperator Operator { get; set; }

        /// <summary>
        /// UI to display video.
        /// </summary>
        protected IVideoForm _videoForm;


        /// <summary>
        /// Time delay for operation which can require some additional time.
        /// </summary>
        protected int _operationDelay;

        public int OperationDelay
        {
            get { return _operationDelay; }
        }

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
        public CredentialsProvider Credentials { get { return _credentialsProvider; } }
        
        public void UpdateCredentials(Security method, string userName, string password)
        {
            Credentials.Security = method;
            Credentials.Username = userName;
            Credentials.Password = password;
        }
        
        public void UpdateCredentials()
        {
            Credentials.Security = _security;
            Credentials.Username = _username;
            Credentials.Password = _password;
        }

        protected Security _security;

        /// <summary>
        /// Updates security.
        /// </summary>
        protected void UpdateSecurity()
        {
            RaiseSecurityChangedEvent(Credentials);
        }

        protected void UpdateSecurity(string username, string password, Security security)
        {
            Credentials.Security = security;
            Credentials.Username = username;
            Credentials.Password = password;

            UpdateSecurity();
        }

        protected void UpdateSecurity(string username, string password)
        {
            UpdateSecurity(username, password, Credentials.Security);
        }

        /// <summary>
        /// Features selected for test.
        /// </summary>
        private FeaturesList _features = new FeaturesList();

        /// <summary>
        /// Features selected by the operator.
        /// </summary>
        public FeaturesList Features
        {
            get { return _features; }
        }

        private readonly List<string> _declaredScopes = new List<string>();

        public List<string> DeclaredScopes
        {
            get { return _declaredScopes; }
        }

        protected BaseOnvifTest(TestLaunchParam param): base(param)
        {
            _features.Mode = param.FeatureDefinitionMode;
            if (param.FeatureDefinitionMode == FeatureDefinitionMode.Default)
            {
                _features.AddRange(param.Features);
            }

            //_cameraAddress = param.ServiceAddress;
            FirmwareFilePath = param.FirmwareFilePath;
            
            CredentialIdentifierValueFirst  = param.CredentialIdentifierValueFirst;
            CredentialIdentifierValueSecond = param.CredentialIdentifierValueSecond;
            CredentialIdentifierValueThird = param.CredentialIdentifierValueThird;

            _cameraIp = param.CameraIp;
            _cameraId = param.CameraUUID;
            _nic = param.NIC;
            TestTool.HttpTransport.RequestNetworkStream.EndpointFrom = new IPEndPoint(_nic.IP, 0);
            _username = param.UserName;
            _password = param.Password;
            _useUTCTimestamp = param.UseUTCTimestamp;
            Operator = param.Operator;

            if (param.VideoForm != null)
            {
                _videoForm = param.VideoForm;
                // reinitialize video window
                _videoForm.Reset();
                _videoForm.NICIndex = param.NIC.Index;
                _videoForm.StopEvent = _semaphore.StopEvent;
            }

            _declaredScopes.AddRange(param.DeclaredScopes);

            _environmentSettings = param.EnvironmentSettings;

            _messageTimeout = param.MessageTimeout;

            _operationDelay = param.OperationDelay;
            _recoveryDelay = param.RecoveryDelay;

            _rebootTimeout = param.RebootTimeout;

            // should be done in BaseTest.Initialize();
            //_trafficListener = new TrafficListener();
            //_trafficListener.RequestSent += LogRequest;
            //_trafficListener.ResponseReceived += LogResponse;

            _endpointController = new EndpointController(new EndpointAddress(param.ServiceAddress));

            _credentialsProvider = new CredentialsProvider
                                   {
                                           Username = param.UserName,
                                           Password = param.Password,
                                           Security = param.Security
                                   };

            NetworkSettingsChangedEvent += address =>
                                           {
                                               var uri = new EndpointAddress(address);

                                               var addresses = Dns.GetHostAddresses(uri.Uri.Host);
                                               IPAddress newAddr = null;
                                               switch (uri.Uri.HostNameType)
                                               {
                                                   case UriHostNameType.IPv4:
                                                       newAddr = addresses.FirstOrDefault(u => u.AddressFamily == AddressFamily.InterNetwork);
                                                       break;
                                                   case UriHostNameType.IPv6:
                                                       newAddr = addresses.FirstOrDefault(u => u.AddressFamily == AddressFamily.InterNetworkV6);
                                                       break;
                                                   case UriHostNameType.Dns:
                                                       newAddr = addresses.FirstOrDefault(u => u.AddressFamily == AddressFamily.InterNetwork);
                                                       if (null == _cameraIp)
                                                           newAddr = addresses.FirstOrDefault(u => u.AddressFamily == AddressFamily.InterNetworkV6);
                                                       break;
                                               }

                                               if (null != newAddr)
                                               {
                                                   _endpointController.UpdateAddress(uri);
                                                   _cameraIp = newAddr;
                                               }
                                           };
        }


        public void DoRequestDelay()
        {
            Sleep(_recoveryDelay);
        }

        protected static void DoRequestDelay(BaseOnvifTest test)
        {
            test.DoRequestDelay();
        }

        public void Delay(string msg, int timeout)
        {
            //RunStep(() => Sleep(timeout), msg);
            LogStepEvent(msg);
            LogStepEvent("");

            Sleep(timeout);
        }

        public void Delay(string msg)
        {
            Delay(msg, OperationDelay);
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
        public Binding CreateBinding(bool includeAddressController,
                                     IEnumerable<IChannelController> customControllers)
        {
            var controllers = new List<IChannelController>();

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

            return CreateBinding(controllers);
        }

        protected Binding CreateBinding(IEnumerable<IChannelController> controllers)
        {
            return BindingFactory.CreateBinding(CameraAddress, controllers);
        }
        /// <summary>
        /// Adds SecurityBehavior to endpoint.
        /// </summary>
        /// <param name="endpoint">Endpoint.</param>
        public void AttachSecurity(System.ServiceModel.Description.ServiceEndpoint endpoint)
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
        public void SetupChannel(IClientChannel channel)
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
        public virtual void EndTest(TestStatus status)
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

        protected bool allPassedModeOn()
        {
#if __ALL_PASSED__
           if (_features.ContainsFeature(Feature.EventsService))
           {
               BeginTest();
               EndTest(TestStatus.Passed);

               return true;
           }
           else
           {
               return false;
           }
#else
           return false;
#endif
        }
        protected VideoContainer2 NewGenVideo = null;
        public override void Halt()
        {
          if (NewGenVideo != null)
          {
            if (NewGenVideo.Terminate(true))
            {
              //return;
            }
          }
          base.Halt();
        }

        /// <summary>
        /// Perform common initialization/finalization. Run test action.
        /// </summary>
        /// <param name="action">Test "body"</param>
        /// <param name="cleanUpAction">Clean-up </param>
        protected void RunTest(Action action, Action cleanUpAction)
        {
            if (allPassedModeOn())
                return;

            try
            {
                _halted = false;
                Exception exc = null;

                try
                {
                    System.Diagnostics.Debug.WriteLine("Begin test - inside 1");
                    BeginTest();

                    if (this is IBaseOnvifService)
                        (this as IBaseOnvifService).GeneralInitialize();

                    System.Diagnostics.Debug.WriteLine("Begin test - do action");
                    action();
                    System.Diagnostics.Debug.WriteLine("Begin test - action done");

                    if (cleanUpAction == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Begin test - to end test");
                        EndTest(TestStatus.Passed);
                        return;
                    }
                }
                catch (StopEventException)
                {
                    System.Diagnostics.Debug.WriteLine("HALT");

                    if (_videoForm != null)
                    {
                        _videoForm.CloseWindow();
                    }
                    if (NewGenVideo != null)
                    {
                      NewGenVideo.CloseWindow();
                    }

                    Release();
                    _halted = true;

                    LogStepEvent("Halted");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Begin test - Exception in action");
                    StepFailed(ex);
                    System.Diagnostics.Debug.WriteLine("Begin test - fail reported");

                    if (InternalLog) 
                        InternalLogger.GetInstance().SwitchOnForCurrentThread();
                    else
                        InternalLogger.GetInstance().SwitchOffForCurrentThread();

                    InternalLogger.GetInstance().LogMessage("Uncaught exception is thrown during the test!");
                    InternalLogger.GetInstance().LogException(ex);

                    if (cleanUpAction == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Begin test - doing Release");
                        Release();
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
                        System.Diagnostics.Debug.WriteLine("Test failed");
                        try
                        {
                            Release();
                        }
                        catch (Exception)
                        {

                        }
                        TestFailed(exc);
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("new catch - Test failed");
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
            if (allPassedModeOn())
                return;

            try
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

                    if (this is IBaseOnvifService)
                        (this as IBaseOnvifService).GeneralInitialize();

                    // run test
                    action();

                    System.Diagnostics.Debug.WriteLine("Main action completed");

                }
                catch (StopEventException)
                {
                    System.Diagnostics.Debug.WriteLine("HALT");

                    if (_videoForm != null)
                    {
                        _videoForm.CloseWindow();
                    }

                    Release();
                    _halted = true;

                    LogStepEvent("Halted");
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
                            try
                            {
                                Release();
                            }
                            catch (Exception)
                            {

                            } 
                            TestFailed(exc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("new catch - Test failed");
                TestFailed(ex);
            }
        }

        #region Settings Recovery

        /// <summary>
        /// Flag to indicate that during settings restoration unexpected fault is occured
        /// </summary>
        bool RestoreSettingsFailed { get; set; }
        /// <summary>
        /// Performs action and doesn't break test's sequence if fault is occured.
        /// </summary>
        /// <param name="action"></param>
        public void AllowFaultStep(Action action)
        {
            try
            { action(); }
            catch (Exception e)
            {
                RestoreSettingsFailed = true;
                StepFailed(e);
            }
        }

        /// <summary>
        ///  Performs action and passes test's step if expected fault is received.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exception"></param>
        public void DoActionWithSOAPFault(Action action, string exception)
        {
            try
            {
                action();
                Assert(false, string.Format("The previous step was performed successfully while SOAP Fault '{0}' is expected.", exception), "Fail the test");
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault(exception))
                    StepPassed();
                else
                    throw;
            }
        }

        /// <summary>
        /// Called at the end of all tests: breaks test's sequence if any unexpected fault are received during settings restoration.
        /// </summary>
        public void FinishRestoreSettings()
        {
            if (RestoreSettingsFailed)
                Assert(false, 
                       "One or more of steps during setting's recovery are failed",
                       "Check setting's recovery is successfull");
        }

        #endregion

    }
}
