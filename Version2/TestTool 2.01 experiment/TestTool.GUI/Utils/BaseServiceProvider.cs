///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Threading;
using System.ServiceModel.Channels;
using System.ServiceModel;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.TestEngine;
using TestTool.HttpTransport;

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
        private T _client;
        private string _serviceAddress;
        private int _messageTimeout;

        public event Action<string, Exception> ExceptionThrown;
        public event Action<string, FaultException> FaultThrown;
        public event Action<string> ResponseReceived;
        public event Action OperationStarted;
        public event Action OperationCompleted;

        protected T Client
        {
            get { return _client; }
        }
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

        public BaseServiceProvider(string serviceAddress, int messageTimeout)
        {
            _serviceAddress = serviceAddress;
            _messageTimeout = messageTimeout;

            _stopEvent = new AutoResetEvent(false);

            CreateClient();

        }
        
        public abstract T CreateClient(Binding binding, EndpointAddress address);
        
        void CreateClient()
        {
            Binding binding = new HttpTransport.HttpBinding(new IChannelController[] {this});

            _client = CreateClient(binding, new EndpointAddress(_serviceAddress));
            System.Net.ServicePointManager.Expect100Continue = false;

            Data.DeviceEnvironment info = Controllers.ContextController.GetDeviceEnvironment();

            SecurityBehavior securityBehavior = new SecurityBehavior();
            securityBehavior.UserName = info.Credentials.UserName;
            securityBehavior.Password = info.Credentials.Password;
            securityBehavior.UseUTCTimestamp = info.Credentials.UseUTCTimeStamp;
            _client.Endpoint.Behaviors.Add(securityBehavior);

            _client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, _messageTimeout);
        }

        public void LogRequest(string request)
        {

        }

        protected bool EnableLogResponse { get; set; }
        public void LogResponse(string response)
        {
            if (EnableLogResponse && ResponseReceived != null)
            {
                ResponseReceived(response);
            }
        }
        protected void ReportOperationStarted()
        {
            if (OperationStarted != null)
            {
                OperationStarted();
            }
        }
        protected void ReportOperationCompleted()
        {
            if (OperationCompleted != null)
            {
                OperationCompleted();
            }
        }
        protected void ReportFault(FaultException ex)
        {
            if (FaultThrown != null)
            {
                FaultThrown(string.Empty, ex);
            }
        }
        protected void ReportException(Exception ex)
        {
            if (ExceptionThrown != null)
            {
                ExceptionThrown(string.Empty, ex);
            }
        }
        protected void ReportException(Exception ex, string stage)
        {
            if (ExceptionThrown != null)
            {
                ExceptionThrown(stage, ex);
            }
        }
        protected void RunInBackground(Action action)
        {
            ReportOperationStarted();
            System.Threading.Thread thread = 
                new Thread(
                    new ThreadStart(new Action ( () => { ProcessRequest(action);}) ));
            thread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            thread.Start();
        }
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

        private AutoResetEvent _stopEvent;

        public WaitHandle StopEvent
        {
            get { return _stopEvent ; }
        }

        public void ReportStop()
        {
            throw new StopEventException();
        }

        #endregion

        public void Stop()
        {
            _stopEvent.Set();
        }
    }
}
