﻿///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Threading;
using TestTool.Tests.Common.TestBase;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Discovery;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Exceptions;
using WSD = TestTool.Proxies.WSDiscovery;

namespace TestTool.Tests.TestCases.Base
{
    public class DeviceDiscoveryTest : DeviceManagementTest
    {
        protected enum WaitMessageType
        {
            Hello = 0x01,
            Bye = 0x02
        }

        private SoapMessage<object> _message;
        private Fault _soapFault;
        private Exception _error;
        //private int _rebootTimeout;
        private AutoResetEvent _eventHelloReceived = new AutoResetEvent(false);
        private AutoResetEvent _eventByeReceived = new AutoResetEvent(false);
        private AutoResetEvent _eventProbeMatchReceived = new AutoResetEvent(false);
        private AutoResetEvent _eventTimeout = new AutoResetEvent(false);
        private AutoResetEvent _eventFaultReceived = new AutoResetEvent(false);
        private AutoResetEvent _eventDiscoveryError = new AutoResetEvent(false);

        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="param">Test parameters</param>
        public DeviceDiscoveryTest(TestLaunchParam param)
            : base(param)
        {
            _messageTimeout = param.MessageTimeout;
            _rebootTimeout = param.RebootTimeout;
        }

        /// <summary>
        /// Reboots DUT
        /// </summary>
        protected void Reboot()
        {
            RunStep(new Action(() => { Client.SystemReboot(); }), "Reboot device");
        }

        /// <summary>
        /// Handles incoming discovery Hello message
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        protected void OnHelloReceived(object sender, DiscoveryMessageEventArgs e)
        {
            _message = e.Message.Clone() as SoapMessage<object>;
            _eventHelloReceived.Set();
        }

        /// <summary>
        /// Handles incoming discovery Bye message
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        protected void OnByeReceived(object sender, DiscoveryMessageEventArgs e)
        {
            _eventByeReceived.Set();
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
        /// Handles discovery soap fault
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        protected void OnSoapFault(object sender, DiscoveryErrorEventArgs e)
        {
            _soapFault = e.Fault;
            _eventFaultReceived.Set();
        }

        /// <summary>
        /// Validates incoming Hello message
        /// </summary>
        /// <param name="message">Hello message</param>
        /// <param name="reason">reason why message is invalid, null if message is valid</param>
        /// <returns>true, if message is valid</returns>
        protected bool ValidateHelloMessage(SoapMessage<WSD.HelloType> message, string[] scopes, out string reason)
        {
            bool res = true;
            reason = null;
            try
            {
                //check Types namespace

                WSD.HelloType hello = message.Object;
                if (hello.Types == null)
                {
                    reason = Resources.ErrorNoTypes_Text;
                    return false;
                }
                if (!DiscoveryUtils.CheckDeviceHelloType(message))
                {
                    reason = string.Format(Resources.ErrorIncorrectTypes_Format,
                        DiscoveryUtils.ONVIF_DISCOVER_TYPES, DiscoveryUtils.ONVIF_NETWORK_WSDL_URL);
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
                if(hello.Scopes.Text == null)
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
                if(scopes != null)
                {
                    missingScope = DiscoveryUtils.GetMissingScope(hello.Scopes.Text, scopes);
                    if (!string.IsNullOrEmpty(missingScope))
                    {
                        reason = string.Format(Resources.ErrorMissingScope_Format, missingScope);
                        return false;
                    }
                }
            }
            catch(Exception e)
            {
                reason = e.Message;
                res = false;
            }
            return res;
        }

        /// <summary>
        /// Wait for Hello from device after device IP is changed
        /// </summary>
        protected SoapMessage<WSD.HelloType> IPChangeDeviceStep()
        {
            return ReceiveHelloMessage(false, true, null, _rebootTimeout + _messageTimeout);
        }

        /// <summary>
        /// Receives Hello message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="action">additional action after begin waiting</param>
        /// <returns>Hello message</returns>
        protected SoapMessage<WSD.HelloType> ReceiveHelloMessage(bool checkIP, bool checkDeviceId, Action action)
        {
            return ReceiveHelloMessage(checkIP, checkDeviceId, action, _rebootTimeout + _messageTimeout);
        }

        /// <summary>
        /// Receives Hello message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="action">additional action after begin waiting</param>
        /// <param name="timeout">time to wait</param>
        /// <returns>Hello message</returns>
        protected SoapMessage<WSD.HelloType> ReceiveHelloMessage(bool checkIP, bool checkDeviceId, Action action, int timeout)
        {
            SoapMessage<object> res = ReceiveMessageInternal(
                WaitMessageType.Hello,
                Resources.StepWaitHello_Title,
                Resources.ErrorNoHelloMessage_Text,
                checkIP,
                checkDeviceId,
                action,
                null,
                timeout);

            if(res != null)
            {
                RunStep(() => { Thread.Sleep(5000); }, "5 seconds timeout after Hello");
            }
            return res != null ? res.ToSoapMessage<WSD.HelloType>() : null;
        }

        /// <summary>
        /// Receives Bye message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="action">additional action after begin waiting</param>
        /// <returns>Bye message</returns>
        protected SoapMessage<WSD.ByeType> ReceiveByeMessage(bool checkIP, bool checkDeviceId, Action action)
        {
            return ReceiveByeMessage(checkIP, checkDeviceId, action, _rebootTimeout + _messageTimeout);
        }

        /// <summary>
        /// Receives Bye message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="action">additional action after begin waiting</param>
        /// <param name="timeout">time to wait</param>
        /// <returns>Bye message</returns>
        protected SoapMessage<WSD.ByeType> ReceiveByeMessage(bool checkIP, bool checkDeviceId, Action action, int timeout)
        {
            SoapMessage<object> res = ReceiveMessageInternal(
                WaitMessageType.Bye,
                Resources.StepWaitBye_Title,
                Resources.ErrorNoByeMessage_Text,
                checkIP,
                checkDeviceId,
                action,
                null,
                timeout);
            return res != null ? res.ToSoapMessage<WSD.ByeType>() : null;
        }

        /// <summary>
        /// Receives Bye or Hello message from DUT
        /// </summary>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="action">additional action after begin waiting</param>
        /// <param name="timeout">time to wait</param>
        /// <returns>Bye message</returns>
        protected SoapMessage<WSD.ByeType> ReceiveByeOrHelloMessage(bool checkIP, bool checkDeviceId, Action action, Action<SoapMessage<object>> validation)
        {
            SoapMessage<object> res = ReceiveMessageInternal(
                WaitMessageType.Bye | WaitMessageType.Hello,
                Resources.StepWaitByeOrHello_Title,
                null,
                checkIP,
                checkDeviceId,
                action,
                validation,
                 _rebootTimeout + _messageTimeout);
            return res != null ? res.ToSoapMessage<WSD.ByeType>() : null;
        }

        /// <summary>
        /// Receives message of specified type from DUT
        /// </summary>
        /// <param name="timeout">Time to wait</param>
        /// <param name="checkIP">if true wait from current device IP</param>
        /// <param name="checkDeviceId">if true wait for current device uuid</param>
        /// <param name="action">additional action after begin waiting</param>
        /// <returns>Message</returns>
        private SoapMessage<object> ReceiveMessageInternal(
            WaitMessageType type, 
            string stepName,
            string noMessageText,
            bool checkIP, 
            bool checkDeviceId, 
            Action action,
            Action<SoapMessage<object>> validation,
            int timeout)
        {
            Discovery discovery = new Discovery(_nic.IP);
            bool waitHello = (type & WaitMessageType.Hello) != 0;
            bool waitBye = (type & WaitMessageType.Bye) != 0;
            if (waitHello)
            {
                discovery.HelloReceived += OnHelloReceived;
            }
            if (waitBye)
            {
                discovery.ByeReceived += OnByeReceived;
            }
            discovery.ReceiveError += OnDiscoveryError;
            discovery.DiscoveryFinished += OnDiscoveryFinished;
            _eventTimeout.Reset();
            _eventHelloReceived.Reset();
            _eventByeReceived.Reset();
            _eventDiscoveryError.Reset();
            SoapMessage<object> response = null;
            try
            {
                if(waitBye && waitHello)
                {
                    discovery.WaitByeOrHello(checkIP ? _cameraIp : null, checkDeviceId ? _cameraId : null, timeout);
                }
                else if(waitBye)
                {
                    discovery.WaitBye(checkIP ? _cameraIp : null, checkDeviceId ? _cameraId : null, timeout);
                }
                else
                {
                    discovery.WaitHello(checkIP ? _cameraIp : null, checkDeviceId ? _cameraId : null, timeout);
                }
                if (action != null)
                {
                    action();
                }
                RunStep(() =>
                {
                    int res = WaitForResponse(new WaitHandle[] { 
                        _eventHelloReceived, 
                        _eventByeReceived, 
                        _eventDiscoveryError, 
                        _eventTimeout });
                    if (((res == 0) && waitHello) ||
                        ((res == 1) && waitBye))
                    {
                        response = _message;
                    }
                    else if(res == 2)
                    {
                        string message = _error.Message + _error.InnerException ?? " " + _error.InnerException.Message;
                        throw new AssertException(message);
                    }
                    else if(noMessageText != null)
                    {
                        throw new AssertException(noMessageText);
                    }
                    if(validation != null)
                    {
                        validation(response);
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
        /// Converts array of scopes to array of strings
        /// </summary>
        /// <param name="scopes">Array of scopes</param>
        /// <returns>Array of scopes as strings</returns>
        protected string[] ScopesToStringArray(Scope[] scopes)
        {
            string[] strScopes = new string[scopes.Length];
            
            for (int i = 0; i < scopes.Length; i++)
            {
                strScopes[i] = scopes[i].ScopeItem;
            }
            return strScopes;
        }

        /// <summary>
        /// Sets DUT scopes
        /// </summary>
        /// <param name="scopes">Scopes to be set</param>
        protected void SetScopes(Scope[] scopes)
        {
            SetScopes(ScopesToStringArray(scopes));
        }

        /// <summary>
        /// Sets DUT scopes
        /// </summary>
        /// <param name="scopes">Scopes to be set</param>
        protected void SetScopes(string[] scopes)
        {
            RunStep(new Action(() => { Client.SetScopes(scopes); }), "Set device scopes");
            DoRequestDelay();
        }

        /// <summary>
        /// Tries to set DUT scopes and checks if soap fault is returned
        /// </summary>
        /// <param name="scopes">Scopes to be set</param>
        protected void SetFixedScopes(Scope[] scopes)
        {
            SetFixedScopes(ScopesToStringArray(scopes));
        }

        /// <summary>
        /// Tries to set DUT scopes and checks if soap fault is returned
        /// </summary>
        /// <param name="scopes">Scopes to be set</param>
        protected void SetFixedScopes(string[] scopes)
        {
            RunStep(new Action(() => { Client.SetScopes(scopes); }), "Set device scopes - negative test", "Sender/OperationProhibited/ScopeOverwrite", true);
            DoRequestDelay();
        }

        /// <summary>
        /// Adds new scopes to DUT
        /// </summary>
        /// <param name="scopes">Scopes to be added</param>
        protected void AddScopes(string[] scopes)
        {
            RunStep(new Action(() => { Client.AddScopes(scopes); }), "Add device scopes");
            DoRequestDelay();
        }

        /// <summary>
        /// Removes scopes from DUT
        /// </summary>
        /// <param name="scopes">Scopes to be removed</param>
        protected void RemoveScopes(string[] scopes)
        {
            RunStep(new Action(() => { Client.RemoveScopes(ref scopes); }), "Remove device scopes");
            DoRequestDelay();
        }

        /// <summary>
        /// Get scopes of DUT
        /// </summary>
        /// <returns>Scopes of DUT</returns>
        protected Scope[] GetScopes()
        {
            Scope[] response = null; 
            RunStep(() => { response = Client.GetScopes(); }, "Get device scopes");
            DoRequestDelay();
            return response;
        }

        /// <summary>
        /// Handles device discovered event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        protected void OnDiscovered(object sender, DiscoveryMessageEventArgs e)
        {
            _message = e.Message.Clone() as SoapMessage<object>;
            _eventProbeMatchReceived.Set();
        }

        /// <summary>
        /// Sends probes message and waits for answer from device
        /// </summary>
        /// <param name="multicast">if true, message will be sent to multicast group address</param>
        /// <param name="scopes">Scope to be probed</param>
        /// <param name="matchRule">Scope matching rule</param>
        /// <returns>ProbeMatches message from device</returns>
        protected SoapMessage<WSD.ProbeMatchesType> ProbeDevice(
            bool multicast, 
            string[] scopes, 
            string matchRule) 
        {
            System.Diagnostics.Trace.WriteLine(string.Format("ProbeDevice: entry point"));
            System.Diagnostics.Trace.Flush();

            SoapMessage<WSD.ProbeMatchesType> response = null;
            Discovery discovery = new Discovery(_nic.IP);
            discovery.Discovered += OnDiscovered;
            discovery.DiscoveryFinished += OnDiscoveryFinished;
            discovery.SoapFaultReceived += OnSoapFault;
            discovery.ReceiveError += OnDiscoveryError;
            _eventProbeMatchReceived = new AutoResetEvent(false);
            _eventTimeout = new AutoResetEvent(false);
            _eventFaultReceived = new AutoResetEvent(false);
            _eventDiscoveryError = new AutoResetEvent(false);
            int res = -1;
            try
            {
                discovery.Probe(multicast, _cameraIp, null, _messageTimeout, scopes, matchRule);
                res = WaitForResponse(new WaitHandle[] { _eventProbeMatchReceived, _eventTimeout, _eventFaultReceived, _eventDiscoveryError });
                if (res == 0)
                {
                    response = _message.ToSoapMessage<WSD.ProbeMatchesType>();
                }
                else if (res == 2)
                {
                    throw new SoapFaultException(_soapFault);
                }
                else if (res == 3)
                {
                    string message = _error.Message + _error.InnerException ?? " " + _error.InnerException.Message;
                    throw new AssertException(message);
                }
            }
            finally
            {
                System.Diagnostics.Trace.WriteLine(string.Format("ProbeDevice: discovery.Dispose, res = {0}", res));
                System.Diagnostics.Trace.Flush();
                discovery.Dispose();
            }
            return response;
        }

        /// <summary>
        /// Performs probing device test step
        /// </summary>
        /// <param name="multicast">if true, probe message will be sent to multicast group address</param>
        /// <param name="scopes">Scopes to be probed</param>
        /// <returns>ProbeMacthes message from DUT</returns>
        protected SoapMessage<WSD.ProbeMatchesType> ProbeDeviceStep(bool multicast, string[] scopes)
        {
            return ProbeDeviceStep(multicast, scopes, null);
        }

        /// <summary>
        /// Performs probing device with invalid parameters test step, 
        /// makes sure that device does not respond to invalid probe
        /// </summary>
        /// <param name="multicast">if true, probe message will be sent to multicast group address</param>
        /// <param name="scopes">Scopes to be probed</param>
        protected void InvalidProbeDeviceStep(bool multicast, string[] scopes)
        {
            RunStep(() =>
            {
                SoapMessage<WSD.ProbeMatchesType>  response = ProbeDevice(multicast, scopes, null);
                if (response != null)
                {
                    throw new AssertException("Device responded to invalid probe message");
                }
            }, Resources.StepInvalidProbe_Title);
        }

        /// <summary>
        /// Performs probing device test step
        /// </summary>
        /// <param name="multicast">if true, probe message will be sent to multicast group address</param>
        /// <param name="scopes">Scopes to be probed</param>
        /// <param name="matchRule">Scopes matching rule</param>
        /// <returns>ProbeMacthes message from DUT</returns> 
        protected SoapMessage<WSD.ProbeMatchesType> ProbeDeviceStep(
            bool multicast,
            string[] scopes,
            string matchRule)
        {
            SoapMessage<WSD.ProbeMatchesType> response = null;

            RunStep(() =>
            {
                response = ProbeDevice(multicast, scopes, matchRule);
                if(response == null)
                {
                    throw new AssertException(Resources.ErrorNoProbeResponse_Text);
                }
            }, Resources.StepProbeDevice_Title);
            return response;
        }

        /// <summary>
        /// Validates ProbeMatch element of ProbeMatches message
        /// </summary>
        /// <param name="match">Element to be validated</param>
        /// <param name="reason">Reason why element is invalid</param>
        /// <returns>true, if element is valid</returns>
        protected bool ValidateProbeMatch(WSD.ProbeMatchType match, out string reason)
        {
            reason = null;
            if (match.Scopes == null)
            {
                reason = Resources.ErrorNoScopes_Text; 
                return false;
            }
            if (match.Types == null)
            {
                //it looks like this check is unnecessary, 
                //because only ProbeMatch with valid type should be validated
                reason = Resources.ErrorNoTypes_Text; 
                return false;
            }
            if (match.EndpointReference == null)
            {
                reason = Resources.ErrorNoEndpointReference_Text;
                return false;
            }
            if(string.IsNullOrEmpty(match.XAddrs))
            {
                reason = Resources.ErrorNoXAddrs_Text;
                return false;
            }
            string[] addresses = match.XAddrs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string address in addresses)
            {
                Uri addr;
                if (!Uri.TryCreate(address, UriKind.Absolute, out addr))
                {
                    reason = string.Format(Resources.ErrorInvalidXAddrs_Format, address);
                    return false;
                }
                else if (string.Compare(addr.LocalPath, "/onvif/device_service", true) != 0)
                {
                    reason = string.Format(Resources.ErrorInvalidServicePath_Format, addr.LocalPath);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Validates ProbeMatches messages
        /// </summary>
        /// <param name="message">Message to be validated</param>
        /// <param name="reason">Reason why message is invalid</param>
        /// <returns>true, if message is valid</returns>
        protected bool ValidateProbeMatchMessage(SoapMessage<WSD.ProbeMatchesType> message, out string reason)
        {
            bool res = true;
            reason = null;
            try
            {
                //check Types namespace
                if(message.Object.ProbeMatch == null)
                {
                    reason = Resources.ErrorNoProbeMatch_Text; 
                    return false;
                }
                bool found = false;
                for(int i = 0; i < message.Object.ProbeMatch.Length; i++)
                {
                    WSD.ProbeMatchType match = message.Object.ProbeMatch[i];
                    if(DiscoveryUtils.CheckDeviceMatchType(message, i))
                    {
                        if(!ValidateProbeMatch(match, out reason))
                        {
                            return false;
                        }
                        found = true;
                        break;
                    }
                }
                if(!found)
                {
                    reason = string.Format(Resources.ErrorIncorrectTypes_Format, DiscoveryUtils.ONVIF_DISCOVER_TYPES, DiscoveryUtils.ONVIF_NETWORK_WSDL_URL);
                    return false;
                }
            }
            catch(Exception e)
            {
                reason = e.Message;
                res = false;
            }
            return res;
        }

        /// <summary>
        /// Waits for device reboot timeout
        /// </summary>
        protected void WaitDeviceReboot()
        {
            RunStep(() => {
                int res = WaitHandle.WaitAny(new WaitHandle[] { _semaphore.StopEvent }, _rebootTimeout);
                if(res == 0)
                {
                    throw new StopEventException();
                }
                }, Resources.StepWaitReboot_Title
                );
        }
    }
}