using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.WSDiscovery;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Discovery;
using TestTool.Tests.Common.Soap;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Utils.SoapValidation;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.Tests.TestCases.Base
{
    public class ReceiverTest : CommonServiceTest<ReceiverPort, ReceiverPortClient>
    {
        private SoapMessage<object> _message;
        private Fault _soapFault;
        private Exception _error;
        private AutoResetEvent _eventHelloReceived = new AutoResetEvent(false);
        private AutoResetEvent _eventByeReceived = new AutoResetEvent(false);
        private AutoResetEvent _eventTimeout = new AutoResetEvent(false);
        private AutoResetEvent _eventFaultReceived = new AutoResetEvent(false);
        private AutoResetEvent _eventDiscoveryError = new AutoResetEvent(false);


        public ReceiverTest(TestLaunchParam param)
            : base(param)
        {

        }

        protected override ReceiverPortClient CreateClient(string address)
        {

            Binding binding = CreateBinding(false,
                    new IChannelController[] { new SoapValidator(ReceiverSchemasSet.GetInstance()) });
            ReceiverPortClient client = new ReceiverPortClient(binding, new EndpointAddress(address));
            return client;
        }


        protected override string ServiceName
        {
            get { return "Receiver"; }
        }

        protected override string GetServiceAddress()
        {
            return GetReceiverServiceAddress();
        }
        
        /// <summary>
        /// Returns DUT's receiver service address
        /// </summary>
        /// <returns>Receiver service url</returns>
        protected string GetReceiverServiceAddress()
        {
            string address = string.Empty;
            RunStep(() =>
            {
                DeviceClient device = DeviceClient;
                
                address = device.GetReceiverServiceAddress(Features);

                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException("The DUT did not return Receiver service address");
                }

            }, "Get Receiver Service address");
            DoRequestDelay();
            return address;
        }

        protected ReceiverServiceCapabilities GetServiceCapabilities()
        {
            ReceiverServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = Client.GetServiceCapabilities(); }, "Get Service Capabilities");
            DoRequestDelay();
            return capabilities;
        }

        protected Receiver[] GetReceivers()
        {
            //Receiver[] items = null;
            //RunStep(() => { items = Client.GetReceivers(); }, "Get Receivers");
            //DoRequestDelay();
            //return items;
            return CommonMethodsProvider.GetReceivers(this, Client);

        }

        protected Receiver GetReceiver(string receiverToken)
        {
            return CommonMethodsProvider.GetReceiver(this, Client, receiverToken);
        }


        protected ReceiverStateInformation GetReceiverState(string receiverToken)
        {
            ReceiverStateInformation receiverSI = null;
            RunStep(() => { receiverSI = Client.GetReceiverState(receiverToken); }, 
                string.Format("Get Receiver '{0}' State", receiverToken));
            DoRequestDelay();
            return receiverSI;
        }


        // Maybe needs to delete
        protected void SetReceiverModeBack(string receiverToken, ReceiverMode mode)
        {
            RunStep(() => { Client.SetReceiverMode(receiverToken, mode) ; },
                "Set Receiver Mode back");
            DoRequestDelay();
        }

        protected void SetReceiverMode(string receiverToken, ReceiverMode mode)
        {
            RunStep(() => { Client.SetReceiverMode(receiverToken, mode); },
                string.Format("Set Receiver Mode to {0}", mode));
            DoRequestDelay();
        }

        protected Receiver CreateReceiver(ReceiverConfiguration config)
        {
            Receiver receiver = null;
            RunStep(() => receiver = Client.CreateReceiver(config), "Create Receiver");
            DoRequestDelay();
            return receiver;
        }

        protected void DeleteReceiver(string receiverToken)
        {
            RunStep(() => Client.DeleteReceiver(receiverToken), 
                string.Format("Delete Receiver (token = '{0}')",receiverToken));
            DoRequestDelay();
        }

        protected void ConfigureReceiver(string receiverToken, ReceiverConfiguration config)
        {
            RunStep(() => Client.ConfigureReceiver(receiverToken, config),
                string.Format("Configure Receiver (token = '{0}')", receiverToken));
            DoRequestDelay();
        }


        // Maybe needs to delete
        protected void ConfigureReceiverBack(string receiverToken, ReceiverConfiguration config)
        {
            RunStep(() => Client.ConfigureReceiver(receiverToken, config),
                string.Format("Configure Receiver (token = '{0}') back", receiverToken));
            DoRequestDelay();
        }

        protected string Reboot()
        {
            string response = string.Empty;
            RunStep(() => response = DeviceClient.SystemReboot(), "Device reboot");
            return response;
        }

        protected void WaitDeviceReboot()
        {
            RunStep(() =>
            {
                int res = WaitHandle.WaitAny(new WaitHandle[] { _semaphore.StopEvent }, _rebootTimeout);
                if (res == 0)
                {
                    throw new StopEventException();
                }
            }, Resources.StepWaitReboot_Title
                );
        }

        protected SoapMessage<HelloType> ReceiveHelloMessage()
        {
            SoapMessage<object> res = ReceiveHelloMessage(
                Resources.StepWaitHello_Title,
                Resources.ErrorNoHelloMessage_Text,
                _rebootTimeout + _messageTimeout);

            if (res != null)
            {
                RunStep(() => { Thread.Sleep(5000); }, "5 seconds timeout after Hello");
            }
            return res != null ? res.ToSoapMessage<HelloType>() : null;
        }


        private SoapMessage<object> ReceiveHelloMessage(
                    string stepName,
                    string noMessageText,
                    int timeout)
        {
            Discovery discovery = new Discovery(_nic.IP);
            discovery.HelloReceived += OnHelloReceived;
            discovery.ReceiveError += OnDiscoveryError;
            discovery.DiscoveryFinished += OnDiscoveryFinished;
            _eventTimeout.Reset();
            _eventHelloReceived.Reset();
            _eventByeReceived.Reset();
            _eventDiscoveryError.Reset();

            SoapMessage<object> response = null;
            try
            {
                discovery.WaitHello(_cameraIp, _cameraId, timeout);
                RunStep(() =>
                {
                    int res = WaitForResponse(new WaitHandle[] { 
                        _eventHelloReceived,
                        _eventByeReceived,
                        _eventDiscoveryError, 
                        _eventTimeout });
                    if (res == 0)
                    {
                        response = _message;
                        if (_message != null)
                        {
                            string dump = System.Text.Encoding.UTF8.GetString(_message.Raw);
                            LogResponse(dump);
                        }
                    }
                    else if (res == 2)
                    {
                        string message = _error.Message + _error.InnerException ?? " " + _error.InnerException.Message;
                        throw new AssertException(message);
                    }
                    else if (noMessageText != null)
                    {
                        throw new AssertException(noMessageText);
                    }
                }, stepName);
            }
            finally
            {
                discovery.Close();
            }
            return response;
        }

        /// <summary>
        /// Handles discovery hello event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        protected void OnHelloReceived(object sender, DiscoveryMessageEventArgs e)
        {
            _message = e.Message.Clone() as SoapMessage<object>;
            _eventHelloReceived.Set();
        }

        /// <summary>
        /// Handles discovery finished event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        protected void OnDiscoveryFinished(object sender, EventArgs e)
        {
            _eventTimeout.Set();
        }

        /// <summary>
        /// Handles discovery error
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        protected void OnDiscoveryError(object sender, DiscoveryErrorEventArgs e)
        {
            _error = e.Exception;
            _eventDiscoveryError.Set();
        }

        /// <summary>
        /// Validates incoming Hello message
        /// </summary>
        /// <param name="message">Hello message</param>
        /// <param name="reason">reason why message is invalid, null if message is valid</param>
        /// <returns>true, if message is valid</returns>
        protected bool ValidateHelloMessage(SoapMessage<HelloType> message, string[] scopes, out string reason)
        {
            bool mode1 = Features.Contains(Feature.DiscoveryTypesDnNetworkVideoTransmitter);
            bool mode2 = Features.Contains(Feature.DiscoveryTypesTdsDevice);
            bool res = true;
            reason = null;
            try
            {
                //check Types namespace

                HelloType hello = message.Object;
                if (hello.Types == null)
                {
                    reason = Resources.ErrorNoTypes_Text;
                    return false;
                }
                if (!DiscoveryUtils.CheckDeviceHelloType(message, out reason, mode1, mode2))
                {
                    return false;
                }
                if (hello.EndpointReference == null)
                {
                    reason = Resources.ErrorNoEndpointReference_Text;
                    return false;
                }
                if (hello.Scopes == null)
                {
                    reason = Resources.ErrorNoScopes_Text;
                    return false;
                }
                if (hello.Scopes.Text == null)
                {
                    reason = Resources.ErrorNoScopesText_Text;
                    return false;
                }
                //check mandatory scopes 
                string missingScope = DiscoveryUtils.GetMissingMandatoryScope(hello.Scopes.Text);
                if (!string.IsNullOrEmpty(missingScope))
                {
                    reason = string.Format(Resources.ErrorMissingMandatoryScope_Format, missingScope);
                    return false;
                }
                //check optional scopes
                if (scopes != null)
                {
                    missingScope = DiscoveryUtils.GetMissingScope(hello.Scopes.Text, scopes);
                    if (!string.IsNullOrEmpty(missingScope))
                    {
                        reason = string.Format(Resources.ErrorMissingScope_Format, missingScope);
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                reason = e.Message;
                res = false;
            }
            return res;
        }
    }
}
