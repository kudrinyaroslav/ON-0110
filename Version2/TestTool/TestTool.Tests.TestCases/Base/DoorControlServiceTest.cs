using System.Collections.Generic;
using System.ServiceModel.Channels;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.CommonUtils.SoapValidation;
using System.ServiceModel;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Exceptions;

namespace TestTool.Tests.TestCases.Base
{
    public class DoorControlServiceTest : BaseServiceTest<DoorControlPort, DoorControlPortClient>
    {

        public DoorControlServiceTest(TestLaunchParam param)
            : base(param)
        {

        }

        protected override DoorControlPortClient CreateClient()
        {
            Binding binding = 
                CreateBinding(false, 
                new IChannelController[] {new SoapValidator(DoorControlSchemaSet.GetInstance())} );
            DoorControlPortClient client = new DoorControlPortClient(binding, new EndpointAddress(GetDoorControlServiceAdderss()));
            return client;
        }

        private DeviceClient _deviceClient;

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


        private ServiceHolder<PACSPortClient, PACSPort> _pacsServiceHolder;

        protected PACSPortClient PACSPortClient
        {
            get
            {
                if (_pacsServiceHolder == null)
                {
                    InitServiceHolders();
                }

                if (_pacsServiceHolder.Client == null)
                {
                    IChannelController[] controllers = new IChannelController[]
                                                           {
                                                               new SoapValidator(AccessControlSchemaSet.GetInstance()),
                                                           };

                    InitServiceClient(_pacsServiceHolder, controllers);

                }
                return _pacsServiceHolder.Client;
            }
        }


        void InitServiceHolders()
        {

            // access control
            _pacsServiceHolder = new ServiceHolder<PACSPortClient, PACSPort>(
                (features) => { return DeviceClient.GetServiceAddress(OnvifService.ACCESSCONTROL); },
                (binding, address) => { return new PACSPortClient(binding, address); },
                "Access Control");


        }

        void InitServiceClient(ServiceHolder serviceHolder, IEnumerable<IChannelController> controllers)
        {
            bool found = false;
            if (!serviceHolder.HasAddress)
            {
                RunStep(() =>
                {
                    serviceHolder.Retrieve(Features);
                    if (!serviceHolder.HasAddress)
                    {
                        throw new AssertException(string.Format("{0} service not found", serviceHolder.ServiceName));
                    }
                    else
                    {
                        found = true;
                        LogStepEvent(serviceHolder.Address);
                    }
                }, string.Format("Get {0} service address", serviceHolder.ServiceName),
                OnvifFaults.NoSuchService, true, true);
                DoRequestDelay();
            }

            Assert(found,
                string.Format("{0} service address not found", serviceHolder.ServiceName),
                string.Format("Check that the DUT returned {0} service address", serviceHolder.ServiceName));

            if (found)
            {
                EndpointController controller = new EndpointController(new EndpointAddress(serviceHolder.Address));

                List<IChannelController> ctrls = new List<IChannelController>();
                ctrls.Add(controller);
                ctrls.AddRange(controllers);

                Binding binding = CreateBinding(
                    false,
                    ctrls);

                serviceHolder.CreateClient(binding, AttachSecurity, SetupChannel);
            }
        }


        protected override void Release()
        {
            base.Release();
            if (_deviceClient != null)
            {
                _deviceClient.Close();
            }
            if (_pacsServiceHolder != null)
            {
                _pacsServiceHolder.Close();
            }

        } 

        /// <summary>
        /// Returns DUT's door control service address
        /// </summary>
        /// <returns>Door control service url</returns>
        protected string GetDoorControlServiceAdderss()
        {
            string address = string.Empty;
            RunStep(() =>
            {
                address = DeviceClient.GetServiceAddress(TestTool.Tests.Definitions.Onvif.OnvifService.DOORCONTROL);

                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException("Door Control service not found");
                }
                else
                {
                    LogStepEvent(address);
                }

            }, "Get Door Control service address");
            DoRequestDelay();
            return address;
        }

        protected DoorInfo[] GetDoorInfo(string[] tokensList)
        {
            return GetDoorInfo(tokensList, "Get DoorInfo");
        }

        protected DoorInfo[] GetDoorInfo(string[] tokensList, string stepName)
        {
            DoorInfo[] info = null;
            RunStep(() => { info = Client.GetDoorInfo(tokensList); }, stepName);
            DoRequestDelay();
            return info;
        }


        protected string GetDoorInfoList(int? limit, string offset, out DoorInfo[] list)
        {
            return GetDoorInfoList(limit, offset, out list,  "Get DoorInfo list");
        }

        protected string GetDoorInfoList(int? limit, string offset, out DoorInfo[] list, string stepName)
        {
            string nextReference = null;
            DoorInfo[] infos = null;
            RunStep(() => { nextReference = Client.GetDoorInfoList(limit, offset, out infos); }, stepName);
            DoRequestDelay();
            list = infos;
            return nextReference;
        }

        protected DoorState GetDoorState(string token)
        {
            return GetDoorState(token, string.Format("Get Door state (token={0})", token));
        }

        protected DoorState GetDoorState(string token, string stepName)
        {
            DoorState state = null;
            RunStep(() => { state = Client.GetDoorState(token); }, stepName);
            DoRequestDelay();
            return state;
        }

        protected const string LOCKDOORSTEPNAMEPATTERN = "Lock Door (token={0})";

        protected void LockDoor(string token)
        {
            LockDoor(token, string.Format(LOCKDOORSTEPNAMEPATTERN, token));
        }

        protected void LockDoor(string token, string stepName)
        {
            LockDoor(token, string.Format(LOCKDOORSTEPNAMEPATTERN, token), ValidateFault);
        }

        protected void LockDoor(string token, string stepName, ValidateTypeFault validate)
        {
            RunStep(() => { Client.LockDoor(token); }, stepName, validate);
            DoRequestDelay();
        }

        protected const string LOCKDOWNDOORSTEPNAMEPATTERN = "LockDown Door (token={0})";

        protected void LockDownDoor(string token)
        {
            LockDownDoor(token, string.Format(LOCKDOWNDOORSTEPNAMEPATTERN, token));
        }

        protected void LockDownDoor(string token, string stepName)
        {
            LockDownDoor(token, string.Format(LOCKDOWNDOORSTEPNAMEPATTERN, token), ValidateFault);
        }

        protected void LockDownDoor(string token, string stepName, ValidateTypeFault validate)
        {
            RunStep(() => { Client.LockDownDoor(token); }, stepName, validate);
            DoRequestDelay();
        }

        protected const string LOCKDOWNRELEASEDOORSTEPNAMEPATTERN = "LockDownRelease Door (token={0})";

        protected void LockDownReleaseDoor(string token)
        {
            LockDownReleaseDoor(token, string.Format(LOCKDOWNRELEASEDOORSTEPNAMEPATTERN, token));
        }

        protected void LockDownReleaseDoor(string token, string stepName)
        {
            LockDownReleaseDoor(token, string.Format(LOCKDOWNRELEASEDOORSTEPNAMEPATTERN, token), ValidateFault);
        }

        protected void LockDownReleaseDoor(string token, string stepName, ValidateTypeFault validate)
        {
            RunStep(() => { Client.LockDownReleaseDoor(token); }, stepName, validate);
            DoRequestDelay();
        }

        protected const string LOCKOPENRELEASEDOORSTEPNAMEPATTERN = "LockOpenRelease Door (token={0})";

        protected void LockOpenReleaseDoor(string token)
        {
            LockOpenReleaseDoor(token, string.Format(LOCKOPENRELEASEDOORSTEPNAMEPATTERN, token));
        }

        protected void LockOpenReleaseDoor(string token, string stepName)
        {
            LockOpenReleaseDoor(token, string.Format(LOCKOPENRELEASEDOORSTEPNAMEPATTERN, token), ValidateFault);
        }

        protected void LockOpenReleaseDoor(string token, string stepName, ValidateTypeFault validate)
        {
            RunStep(() => { Client.LockOpenReleaseDoor(token); }, stepName, validate);
            DoRequestDelay();
        }

        protected const string LOCKOPENDOORSTEPNAMEPATTERN = "LockOpen Door (token={0})";

        protected void LockOpenDoor(string token)
        {
            LockOpenDoor(token, string.Format(LOCKOPENDOORSTEPNAMEPATTERN, token));
        }

        protected void LockOpenDoor(string token, string stepName)
        {
            LockOpenDoor(token, string.Format(LOCKOPENDOORSTEPNAMEPATTERN, token), ValidateFault);
        }

        protected void LockOpenDoor(string token, string stepName, ValidateTypeFault validate)
        {
            RunStep(() => { Client.LockOpenDoor(token); }, stepName, validate);
            DoRequestDelay();
        }

        protected const string UNLOCKDOORSTEPNAMEPATTERN = "Unlock Door (token={0})";

        protected void UnlockDoor(string token)
        {
            UnlockDoor(token, string.Format(UNLOCKDOORSTEPNAMEPATTERN, token));
        }

        protected void UnlockDoor(string token, string stepName)
        {
            UnlockDoor(token, string.Format(UNLOCKDOORSTEPNAMEPATTERN, token), ValidateFault);
        }

        protected void UnlockDoor(string token, string stepName, ValidateTypeFault validate)
        {
            RunStep(() => { Client.UnlockDoor(token); }, stepName, validate);
            DoRequestDelay();
        }

        protected const string BLOCKDOORSTEPNAMEPATTERN = "Block Door (token={0})";

        protected void BlockDoor(string token)
        {
            BlockDoor(token, string.Format(BLOCKDOORSTEPNAMEPATTERN, token));
        }

        protected void BlockDoor(string token, string stepName)
        {
            BlockDoor(token, string.Format(BLOCKDOORSTEPNAMEPATTERN, token), ValidateFault);
        }

        protected void BlockDoor(string token, string stepName, ValidateTypeFault validate)
        {
            RunStep(() => { Client.BlockDoor(token); }, stepName, validate);
            DoRequestDelay();
        }

        protected const string DOUBLELOCKDOORSTEPNAMEPATTERN = "DoubleLock Door (token={0})";

        protected void DoubleLockDoor(string token)
        {
            DoubleLockDoor(token, string.Format(DOUBLELOCKDOORSTEPNAMEPATTERN, token));
        }

        protected void DoubleLockDoor(string token, string stepName)
        {
            DoubleLockDoor(token, string.Format(DOUBLELOCKDOORSTEPNAMEPATTERN, token), ValidateFault);
        }

        protected void DoubleLockDoor(string token, string stepName, ValidateTypeFault validate)
        {
            RunStep(() => { Client.DoubleLockDoor(token); }, stepName, validate);
            DoRequestDelay();
        }

        protected void AccessDoor(string token, 
            bool? useExtendedTime, 
            string accessTime, 
            string openTooLongTime, 
            string preAlarmTime, 
            AccessDoorExtension extension)
        {
            AccessDoor(token, useExtendedTime, accessTime, openTooLongTime, preAlarmTime, extension, string.Format("Access Door (token={0})", token));
        }

        protected void AccessDoor(string token, 
            bool? useExtendedTime, 
            string accessTime, 
            string openTooLongTime, 
            string preAlarmTime, 
            AccessDoorExtension extension, 
            string stepName)
        {
            RunStep(() => { Client.AccessDoor(token, useExtendedTime, accessTime, openTooLongTime, preAlarmTime, extension); }, stepName, ValidateFault);
            DoRequestDelay();
        }

        protected DoorControlServiceCapabilities GetServiceCapabilities()
        {
            DoorControlServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = Client.GetServiceCapabilities(); }, "Get Service Capabilities");
            return capabilities;
        }


        /// <summary>
        /// Fault validation method with standard signature.
        /// For these test cases does nothing.
        /// </summary>
        /// <param name="fault">Exception (null if no exception).</param>
        /// <param name="reason">Validation text result.</param>
        /// <returns></returns>
        protected bool ValidateFault(FaultException fault, out string reason)
        {
            reason = string.Empty;
            return true;
        }
    }

}
