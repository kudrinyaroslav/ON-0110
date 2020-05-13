///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Common.TestBase;
using System.ServiceModel;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Exceptions;
using System.ServiceModel.Channels;
using System;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.Base
{
    public class ImagingTest : BaseServiceTest<ImagingPort, ImagingPortClient>
    {
        private string _mediaServiceAddress;
        private string _ioServiceAddress;
        private MediaClient _mediaClient;
        private DeviceClient _deviceClient;
        protected DeviceIOPortClient _deviceIoClient;
        
        private bool _mediaNotSupported = false;
        private bool _deviceIoNotSupported = false;

        public ImagingTest(TestLaunchParam param)
            : base(param)
        {
        }

        protected override void Release()
        {
            if (_deviceClient != null)
            {
                _deviceClient.Close();
            }
            if (_mediaClient != null)
            {
                _mediaClient.Close();
            }
            if (_deviceIoClient != null)
            {
                _deviceIoClient.Close();
            }
            base.Release();
        }


        protected override ImagingPortClient CreateClient()
        {
            Binding binding =
                CreateBinding(false,
                new IChannelController[]{new  SoapValidator(ImagingSchemaSet.GetInstance())});
            ImagingPortClient client = new ImagingPortClient(
                binding, 
                new EndpointAddress(GetImagingServiceAdderss()));
            return client;
        }

        private DeviceClient DeviceClient
        {
            get
            {
                if (_deviceClient == null)
                {
                    Binding binding =
                        CreateBinding(false,
                        new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                    _deviceClient = new DeviceClient(binding, new EndpointAddress(CameraAddress));

                    AttachSecurity(_deviceClient.Endpoint);
                    SetupChannel(_deviceClient.InnerChannel);

                }

                return _deviceClient;
            }
        }
        
        protected MediaClient MediaClient
        {
            get
            {
                if (_mediaClient == null && !_mediaNotSupported)
                {
                    if (string.IsNullOrEmpty(_mediaServiceAddress))
                    {
                        RunStep(() =>
                        {
                            _mediaNotSupported = true;

                            string address = this.DeviceClient.GetMediaServiceAddress(Features);
                            if (string.IsNullOrEmpty(address))
                            {
                                LogStepEvent("Media service not found");
                            }
                            else
                            {
                                _mediaServiceAddress = address;
                                _mediaNotSupported = false;
                                LogStepEvent(_mediaServiceAddress);
                            }
                        }, "Get media service address", OnvifFaults.NoSuchService, true, true);
                        DoRequestDelay();
                    }

                    if (!_mediaNotSupported)
                    {
                        Binding binding = CreateBinding(
                            false,
                            new IChannelController[] {new SoapValidator(MediaSchemasSet.GetInstance())});

                        _mediaClient = new MediaClient(binding, new EndpointAddress(_mediaServiceAddress));

                        AttachSecurity(_mediaClient.Endpoint);
                        SetupChannel(_mediaClient.InnerChannel);
                    }
                }
                return _mediaClient;
            }
        }
        
        protected DeviceIOPortClient DeviceIoClient
        {
            get
            {
                if (_deviceIoClient == null && !_deviceIoNotSupported)
                {
                    if (string.IsNullOrEmpty(_ioServiceAddress))
                    {
                        RunStep(() =>
                        {
                            _deviceIoNotSupported = true;
                            
                            string address = DeviceClient.GetIoServiceAddress(Features);

                            if (string.IsNullOrEmpty(address))
                            {
                                LogStepEvent("Device I/O capabilities not found");
                            }
                            else
                            {
                                _ioServiceAddress = address;
                                _deviceIoNotSupported = false;
                                LogStepEvent(_ioServiceAddress);
                            }
                        }, "Get I/O service address", OnvifFaults.NoSuchService, true, true);
                        DoRequestDelay();
                    }

                    if (!_deviceIoNotSupported)
                    {
                        Binding binding = CreateBinding(
                            false,
                            new IChannelController[] { new SoapValidator(DeviceIoSchemaSet.GetInstance()) });

                        _deviceIoClient = new DeviceIOPortClient(binding, new EndpointAddress(_ioServiceAddress));

                        AttachSecurity(_deviceIoClient.Endpoint);
                        SetupChannel(_deviceIoClient.InnerChannel);
                    }

                }
                return _deviceIoClient;
            }

        }


        /// <summary>
        /// Returns DUT's imaging service address
        /// </summary>
        /// <returns>Imaging service url</returns>
        protected string GetImagingServiceAdderss()
        {
            string address = string.Empty;
            RunStep(() =>
            {
                address = DeviceClient.GetImagingServiceAddress(Features);
                
                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException("Imaging capabilities not found");
                }
                else
                {
                    LogStepEvent(address);
                }

            }, "Get imaging service address");
            DoRequestDelay();
            return address;
        }

        protected bool RunStepHandleNotSupported(Action action, string stepName)
        {
            return RunStepHandleNotSupported(action, stepName, null);
        }

        protected bool RunStepHandleNotSupported(Action action, string stepName, ValidateTypeFault validateTypeFault)
        {
            return RunStepHandleNotSupported(action, stepName, validateTypeFault, null);
        }

        /// <summary>
        /// Perform a step.
        /// </summary>
        /// <param name="action">Action to be performed in a step.</param>
        /// <param name="stepName">Step name</param>
        protected bool RunStepHandleNotSupported(Action action, 
            string stepName, 
            ValidateTypeFault validateTypeFault, 
            ValidateNoFault validateNoFault)
        {
            bool ok = false; // easier to set "true" if action supported
            BeginStep(stepName);
            try
            {
                action();
                if (validateNoFault == null)
                {
                    ok = true; // OK, action supported
                }
                else
                {
                    string reason;
                    bool noFaultOk = validateNoFault(out reason);
                    if (!noFaultOk)
                    {
                        AssertException ex = new AssertException(reason);
                        throw ex;
                    }
                }
            }
            catch (FaultException exc)
            {
                ok = false; // action not supported, except cases of some "Reasonable" fault

                LogStepEvent("SOAP fault returned");
                LogStepEvent(string.Format("Code: {0}", exc.Code.Name));
                System.ServiceModel.FaultCode subCode = exc.Code.SubCode;
                while (subCode != null)
                {
                    LogStepEvent(string.Format("Subcode: {0}", subCode.Name));
                    subCode = subCode.SubCode;
                }

                string faultReason = string.Format("Reason: {0}", exc.Reason).Replace("\n", Environment.NewLine);
                LogStepEvent(faultReason);


                if (!exc.IsValidOnvifFault("Receiver/ActionNotSupported/NoImagingForSource"))
                {
                    if (!exc.IsValidOnvifFault("Receiver/ActionNotSupported"))
                    {
                        //ok = true; // action supported
                        if (validateTypeFault != null)
                        {
                            CustomValidateFault(validateTypeFault, exc);
                        }
                        else
                        {
                            // other faults are also OK
                            LogStepEvent("Warning: fault received is neither Receiver/ActionNotSupported, nor Receiver/ActionNotSupported/NoImagingForSource");
                        }
                    }
                }

            }
            StepPassed();
            return ok;
        }
        
        void CustomValidateFault(ValidateTypeFault validateTypeFault, FaultException exc)
        {
            // other faults are -may be- caught
            string reason;
            bool faultOK = validateTypeFault(exc, out reason);

            //
            // if other faults are accepted, handle this in validateTypeFault and return TRUE
            //

            if (!faultOK)
            {
                string dump = string.Format("The SOAP FAULT returned from the DUT is invalid: {0}", reason);
                AssertException ex = new AssertException(dump);
                throw ex;
            }

        }
        
        protected bool HandleNoExpectedFault(out string reason)
        {
            reason = "No SOAP fault received";
            return false;
        }
        
        protected ImagingSettings20 GetImagingSettings(string token)
        {
            return GetImagingSettings(token, "Get imaging settings");
        }        
        
        protected ImagingSettings20 GetImagingSettings(string token, string stepName)
        {
            ImagingSettings20 settings = null;
            RunStep(() => { settings = Client.GetImagingSettings(token) ; }, stepName);
            DoRequestDelay();
            return settings;
        }
        
        protected void SetImagingSettings(string token, ImagingSettings20 settings, bool forcePersistance)
        {
            SetImagingSettings(token, settings, forcePersistance, "Set imaging settings");
        }

        protected void SetImagingSettings(string token, ImagingSettings20 settings, bool forcePersistance, string stepName)
        {
            RunStep( () => { Client.SetImagingSettings(token, settings, forcePersistance);}, stepName);
            DoRequestDelay();
        }

        protected ImagingOptions20 GetOptions(string token)
        {
            return GetOptions(token, "Get imaging options");
        }

        protected ImagingOptions20 GetOptions(string token, string stepName)
        {
            ImagingOptions20 options = null;
            RunStep(() => { options = Client.GetOptions(token); }, stepName);
            DoRequestDelay();
            return options;
        }

        protected void Move(string token, FocusMove focus)
        {
            Move(token, focus, string.Format("Send Move command ({0})", token));
        }

        protected void Move(string token, FocusMove focus, string stepName)
        {
            RunStep(() => { Client.Move(token, focus); }, stepName);
            DoRequestDelay();
        }

        protected void Stop(string token)
        {
            bool succeeded;
            Stop(token, out succeeded);
        }

        protected void Stop(string token, out bool succeeded)
        {
            Stop(token, "Stop", out succeeded);
        }

        protected void Stop(string token, string stepName, out bool succeeded)
        {
            succeeded = RunStepHandleNotSupported( () => { Client.Stop(token);}, stepName);
            DoRequestDelay();
        }

        protected ImagingStatus20 GetImagingStatus(string token)
        {
            return GetImagingStatus(token, "Get imaging status");
        }

        protected ImagingStatus20 GetImagingStatus(string token, out bool succeeded)
        {
            return GetImagingStatus(token, "Get imaging status", out succeeded);
        }

        protected ImagingStatus20 GetImagingStatus(string token, string stepName)
        {
            bool succeeded;
            return GetImagingStatus(token, stepName, out succeeded);
        }

        protected ImagingStatus20 GetImagingStatus(string token, string stepName, out bool succeeded)
        {
            ImagingStatus20 status = null;
            succeeded = RunStepHandleNotSupported(() => { status = Client.GetStatus(token); }, stepName);
            DoRequestDelay();
            return status;
        }        
        protected MoveOptions20 GetMoveOptions(string token)
        {
            return GetMoveOptions(token, string.Format("Get Move options for {0}", token));
        }

        protected MoveOptions20 GetMoveOptions(string token, string stepName)
        {
            MoveOptions20 options = null;
            RunStep(() => { options = Client.GetMoveOptions(token); }, stepName);
            DoRequestDelay();
            return options;
        }


        /// <summary>
        /// Retrieves lists of video sources form DUT
        /// </summary>
        /// <returns>Array of video sources</returns>
        protected VideoSource[] GetVideoSources()
        {
            VideoSource[] sources = null;
            MediaClient client = MediaClient;
            if (client != null)
            {
                RunStep(() => { sources = client.GetVideoSources(); }, "Get video sources");
            }
            else
            {
                DeviceIOPortClient deviceIOPortClient = DeviceIoClient;
                if (deviceIOPortClient != null)
                {
                    RunStep(() => { sources = deviceIOPortClient.GetVideoSources(); }, "Get video sources");
                    DoRequestDelay();
                }
                else
                {
                    Assert(false, 
                        "Neither media, nor I/O supported.", 
                        "Check if media or I/O supported");
                }
            }

            return sources;
        }

    }
}
