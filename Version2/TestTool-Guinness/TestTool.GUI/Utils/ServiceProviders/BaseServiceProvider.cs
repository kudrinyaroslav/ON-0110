///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Threading;
using System.ServiceModel.Channels;
using System.ServiceModel;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.Transport;
using TestTool.HttpTransport;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Definitions.Exceptions;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Base proxy-class wrapper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TChannel"></typeparam>
    abstract class BaseServiceProvider<T, TChannel> : ITrafficListener, IExecutionController
        where T : ClientBase<TChannel>
        where TChannel: class
    {
        /// <summary>
        /// Proxy-class instance
        /// </summary>
        private T _client;
        /// <summary>
        /// Service address
        /// </summary>
        protected string _serviceAddress;
        /// <summary>
        /// Message timeout
        /// </summary>
        private int _messageTimeout;

        protected CredentialsProvider _credentialsProvider;

        /// <summary>
        /// Is raised when an exception is thrown.
        /// </summary>
        public event Action<string, Exception> ExceptionThrown;
        /// <summary>
        /// Is raised when a fault exception is received.
        /// </summary>
        public event Action<string, FaultException> FaultThrown;
        /// <summary>
        /// Is raised when a response from the DUT is received.
        /// </summary>
        public event Action<string> ResponseReceived;
        /// <summary>
        /// Is raised when operation is started.
        /// </summary>
        public event Action OperationStarted;
        /// <summary>
        /// Is raised when operation is completed.
        /// </summary>
        public event Action OperationCompleted;

        protected T Client
        {
            get { return _client; }
        }

        /// <summary>
        /// Address
        /// </summary>
        public string Address
        {
            get
            {
                return _client.Endpoint.Address.Uri.OriginalString;
            }
            set
            {
                if (_serviceAddress != value)
                {
                    _serviceAddress = value;
                    CreateClient();
                }
            }
        }

        /// <summary>
        /// Timeout
        /// </summary>
        public int Timeout
        {
            get
            {
                return (int)_client.InnerChannel.OperationTimeout.TotalMilliseconds;
            }
            set
            {
                _messageTimeout = value;
                _client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, _messageTimeout);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceAddress">Service address.</param>
        /// <param name="messageTimeout">Message timeout.</param>
        protected BaseServiceProvider(string serviceAddress, int messageTimeout)
        {
            _serviceAddress = serviceAddress;
            _messageTimeout = messageTimeout;

            _security = Security.None;

            _stopEvent = new AutoResetEvent(false);

            CreateClient();

        }
        
        /// <summary>
        /// Creates client.
        /// </summary>
        /// <param name="binding">Binding.</param>
        /// <param name="address">Service address.</param>
        /// <returns></returns>
        public abstract T CreateClient(Binding binding, EndpointAddress address);
        
        /// <summary>
        /// Creates client.
        /// </summary>
        void CreateClient()
        {
            Data.DeviceEnvironment info = Controllers.ContextController.GetDeviceEnvironment();

            EndpointController controller = new EndpointController(new EndpointAddress(_serviceAddress));
            
            _credentialsProvider = new CredentialsProvider();
            _credentialsProvider.Username = info.Credentials.UserName;
            _credentialsProvider.Password = info.Credentials.Password;
            _credentialsProvider.Security = _security;

            Binding binding = new HttpBinding(new IChannelController[] { this, controller, _credentialsProvider });

            _client = CreateClient(binding, new EndpointAddress(_serviceAddress));
            System.Net.ServicePointManager.Expect100Continue = false;
            
            SecurityBehavior securityBehavior = new SecurityBehavior();
            securityBehavior.CredentialsProvider = _credentialsProvider;
            _client.Endpoint.Behaviors.Add(securityBehavior);

            _client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, _messageTimeout);
        }

        /// <summary>
        /// Logs request (interface implementation)
        /// </summary>
        /// <param name="request"></param>
        public void LogRequest(string request)
        {

        }

        private Security _security;
        public Security Security
        {
            get
            {
                return _security;
            }
            set
            {
                _security = value;
                if (_credentialsProvider !=null)
                {
                    _credentialsProvider.Security = value;
                }
            }
        }

        /// <summary>
        /// Gets/sets flag indicating that response logging is enabled/disabled.
        /// </summary>
        protected bool EnableLogResponse { get; set; }
        
        /// <summary>
        /// logs response
        /// </summary>
        /// <param name="response"></param>
        public void LogResponse(string response)
        {
            if (EnableLogResponse && ResponseReceived != null)
            {
                ResponseReceived(response);
            }
        }

        /// <summary>
        /// Notifies that operation is started.
        /// </summary>
        protected void ReportOperationStarted()
        {
            if (OperationStarted != null)
            {
                OperationStarted();
            }
        }

        /// <summary>
        /// Notifies that operation is completed.
        /// </summary>
        protected void ReportOperationCompleted()
        {
            if (OperationCompleted != null)
            {
                OperationCompleted();
            }
        }

        /// <summary>
        /// Reports fault.
        /// </summary>
        /// <param name="ex"></param>
        protected void ReportFault(FaultException ex)
        {
            if (FaultThrown != null)
            {
                FaultThrown(string.Empty, ex);
            }
        }

        /// <summary>
        /// Reports exception.
        /// </summary>
        /// <param name="ex"></param>
        protected void ReportException(Exception ex)
        {
            if (ExceptionThrown != null)
            {
                ExceptionThrown(string.Empty, ex);
            }
        }
        /// <summary>
        /// Reports exception.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="stage"></param>
        protected void ReportException(Exception ex, string stage)
        {
            if (ExceptionThrown != null)
            {
                ExceptionThrown(stage, ex);
            }
        }
        /// <summary>
        /// Runs action in background.
        /// </summary>
        /// <param name="action"></param>
        protected void RunInBackground(Action action)
        {
            ReportOperationStarted();
            System.Threading.Thread thread = 
                new Thread(
                    new ThreadStart(new Action ( () => { ProcessRequest(action);}) ));
            thread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            thread.Start();
        }

        /// <summary>
        /// Processes request.
        /// </summary>
        /// <param name="action"></param>
        protected void ProcessRequest(Action action)
        {
            bool stopped = false;
            try
            {
                ReportOperationStarted();
                action();
                //_client.Close();
            }
            catch (FaultException exc)
            {
                ReportFault(exc);
                // do nothing, since information about FaultException is displayed in received packet.
            }
            catch (StopEventException)
            {
                stopped = true; 
            }
            catch (Exception exc)
            {
                ReportException(exc);
            }
            finally
            {
                // if stopped, application is being closed
                if (!stopped)
                {
                    ReportOperationCompleted();
                }
            }
        }


        #region IExecutionController Members

        /// <summary>
        /// Stop event.
        /// </summary>
        private AutoResetEvent _stopEvent;

        /// <summary>
        /// 
        /// </summary>
        public WaitHandle StopEvent
        {
            get { return _stopEvent ; }
        }

        /// <summary>
        /// Reports that operation should be stopped.
        /// </summary>
        public void ReportStop()
        {
            throw new StopEventException();
        }

        #endregion

        /// <summary>
        /// Stops execution.
        /// </summary>
        public void Stop()
        {
            _stopEvent.Set();
        }

        protected Action ConstructSecurityTolerantAction(Action unsecure)
        {
            Action action = new Action(
                () =>
                    {
                        try
                        {
                            System.Diagnostics.Debug.WriteLine("Try without security");
                            Security = Security.None;
                            unsecure();
                        }
                        catch (HttpTransport.Interfaces.Exceptions.AccessDeniedException exc)
                        {
                            System.Diagnostics.Debug.WriteLine("Try with Digest");
                            Security = Security.Digest;
                            unsecure();
                        }
                        catch (FaultException exc)
                        {
                            if (exc.IsValidOnvifFault(OnvifFaults.NotAuthorized) || exc.IsValidOnvifFault(OnvifFaults.SenderNotAuthorized))
                            {
                                System.Diagnostics.Debug.WriteLine("Try with WSU");
                                Security = Security.WS;
                                unsecure();
                            }
                            else
                            {
                                throw exc;
                            }
                        }
                    });

            return action;
        }
    }
}
