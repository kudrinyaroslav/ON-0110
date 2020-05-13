using System;
using System.Collections.Generic;
using TestTool.Onvif;

namespace TestTool.Services
{
    [System.ServiceModel.ServiceBehavior(InstanceContextMode = System.ServiceModel.InstanceContextMode.Single)]
    public class DeviceService : BaseService, TestTool.Onvif.Device
    {
        private string _host;

        public DeviceService()
        {

        }

        public override void UpdateParameters(ServiceConfiguration configuration)
        {
            _host = configuration.BaseAddress;
            base.UpdateParameters(configuration);
        }


        #region Device Members

        public void DoNothing()
        {
            
        }

        public GetServicesResponse GetServices(GetServicesRequest request)
        {
            BeginMethod("GetServices");

            List<Service> res = new List<Service>();

            bool includeCapability = request.IncludeCapability;


            foreach (Service service in SimulatorConfiguration.ServicesConfiguration.Services)
            {
                Service copy = new Service();
                copy.Namespace = service.Namespace;
                copy.Version = service.Version;

                if (includeCapability)
                {
                    copy.Capabilities = SimulatorConfiguration.ServicesConfiguration.GetServiceCapabilitiesElement(service.Namespace);
                }

                string localAddress = string.Empty;

                switch (service.Namespace)
                {
                    case Common.Definitions.OnvifService.DEVICE:
                        localAddress = Definitions.LocalAddress.DEVICE;
                        break;
                    case Common.Definitions.OnvifService.ACCESSCONTROL:
                        localAddress = Definitions.LocalAddress.ACCESSCONTROL;
                        break;
                    case Common.Definitions.OnvifService.DOORCONTROL:
                        localAddress = Definitions.LocalAddress.DOORCONTROL;
                        break;
                    case Common.Definitions.OnvifService.EVENTS:
                        localAddress = Definitions.LocalAddress.EVENTS;
                        break;
                }
                
                copy.XAddr  = _host + localAddress;
                res.Add(copy);
            }

            EndMethod();

            return new GetServicesResponse(res.ToArray());
        }

        public TestTool.Onvif.DeviceServiceCapabilities GetServiceCapabilities()
        {
            BeginMethod("GetServiceCapabilities");

            DeviceServiceCapabilities capabilities = SimulatorConfiguration.ServicesConfiguration.DeviceServiceCapabilities;

            EndMethod();

            return capabilities;
        }

        public string GetDeviceInformation(out string Model, out string FirmwareVersion, out string SerialNumber, out string HardwareId)
        {
            BeginMethod("GetDeviceInformation");

            Model = SimulatorConfiguration.DeviceInformation.Model;
            FirmwareVersion = SimulatorConfiguration.DeviceInformation.FirmwareVersion;
            SerialNumber = SimulatorConfiguration.DeviceInformation.SerialNumber;
            HardwareId = SimulatorConfiguration.DeviceInformation.HardwareId;
            
            EndMethod();

            return SimulatorConfiguration.DeviceInformation.Brand;
        }

        public void SetSystemDateAndTime(TestTool.Onvif.SetDateTimeType DateTimeType, bool DaylightSavings, TestTool.Onvif.TimeZone TimeZone, TestTool.Onvif.DateTime UTCDateTime)
        {
            BeginMethod("SetSystemDateAndTime");

            EndMethod();
        }

        public TestTool.Onvif.SystemDateTime GetSystemDateAndTime()
        {
            throw new NotImplementedException();
        }

        public void SetSystemFactoryDefault(TestTool.Onvif.FactoryDefaultType FactoryDefault)
        {
            BeginMethod("SetSystemFactoryDefault");

            EndMethod();
        }

        public string UpgradeSystemFirmware(TestTool.Onvif.AttachmentData Firmware)
        {
            throw new NotImplementedException();
        }

        public string SystemReboot()
        {
            BeginMethod("SystemReboot");

            EndMethod();
            return "Rebooting in 5 seconds";
        }

        public TestTool.Onvif.RestoreSystemResponse RestoreSystem(TestTool.Onvif.RestoreSystemRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetSystemBackupResponse GetSystemBackup(TestTool.Onvif.GetSystemBackupRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.SystemLog GetSystemLog(TestTool.Onvif.SystemLogType LogType)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.SupportInformation GetSystemSupportInformation()
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetScopesResponse GetScopes(TestTool.Onvif.GetScopesRequest request)
        {
            BeginMethod("GetScopes");
            Scope[] scopes = SimulatorConfiguration.Scopes.ToArray();
            EndMethod();
            return new GetScopesResponse(scopes);
        }

        public TestTool.Onvif.SetScopesResponse SetScopes(TestTool.Onvif.SetScopesRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.AddScopesResponse AddScopes(TestTool.Onvif.AddScopesRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.RemoveScopesResponse RemoveScopes(TestTool.Onvif.RemoveScopesRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.DiscoveryMode GetDiscoveryMode()
        {
            throw new NotImplementedException();
        }

        public void SetDiscoveryMode(TestTool.Onvif.DiscoveryMode DiscoveryMode)
        {
            BeginMethod("SetDiscoveryMode");

            EndMethod();
        }

        public TestTool.Onvif.DiscoveryMode GetRemoteDiscoveryMode()
        {
            throw new NotImplementedException();
        }

        public void SetRemoteDiscoveryMode(TestTool.Onvif.DiscoveryMode RemoteDiscoveryMode)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetDPAddressesResponse GetDPAddresses(TestTool.Onvif.GetDPAddressesRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.SetDPAddressesResponse SetDPAddresses(TestTool.Onvif.SetDPAddressesRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetEndpointReferenceResponse GetEndpointReference(TestTool.Onvif.GetEndpointReferenceRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.RemoteUser GetRemoteUser()
        {
            throw new NotImplementedException();
        }

        public void SetRemoteUser(TestTool.Onvif.RemoteUser RemoteUser)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetUsersResponse GetUsers(TestTool.Onvif.GetUsersRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.CreateUsersResponse CreateUsers(TestTool.Onvif.CreateUsersRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.DeleteUsersResponse DeleteUsers(TestTool.Onvif.DeleteUsersRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.SetUserResponse SetUser(TestTool.Onvif.SetUserRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetWsdlUrlResponse GetWsdlUrl(TestTool.Onvif.GetWsdlUrlRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetCapabilitiesResponse GetCapabilities(TestTool.Onvif.GetCapabilitiesRequest request)
        {
            BeginMethod("GetCapabilities");

            List<CapabilityCategory> categories = new List<CapabilityCategory>(request.Category);
            CapabilityCategory singleCategory = CapabilityCategory.All;
            bool allCapabilitiesQueried = false;
            bool singleCategoryRequested = false;
            if (categories.Count == 0)
            {
                allCapabilitiesQueried = true;
            }
            else
            {
                if (categories.Count == 1)
                {
                    singleCategory = categories[0];
                    singleCategoryRequested = singleCategory != CapabilityCategory.All;
                }
                allCapabilitiesQueried = categories.Contains(CapabilityCategory.All);
            }
            
            if (categories.Contains(CapabilityCategory.Media) || 
                categories.Contains(CapabilityCategory.Analytics) || 
                categories.Contains(CapabilityCategory.Imaging) || 
                categories.Contains(CapabilityCategory.PTZ))
            {
                string[] faultCodes = new string[] { "Receiver","ActionNotSupported", "NoSuchService" };
                Transport.CommonUtils.ReturnFault(faultCodes);
            }

            SimulatorConfiguration.ServicesConfiguration.CreateOldStyleCapabilities();
            Capabilities capabilities = SimulatorConfiguration.ServicesConfiguration.Capabilities;

            if (singleCategoryRequested)
            {
                if (singleCategory == CapabilityCategory.Device)
                {
                    capabilities.Events = null;
                }
                else
                {
                    capabilities.Device = null;
                }
            }

            if (capabilities.Device != null)
            {
                capabilities.Device.XAddr = _host + Definitions.LocalAddress.DEVICE;
            }
            if (capabilities.Events != null)
            {
                capabilities.Events.XAddr = _host + Definitions.LocalAddress.EVENTS;
            }
            
            EndMethod();

            return new GetCapabilitiesResponse(capabilities);
        }

        public TestTool.Onvif.HostnameInformation GetHostname()
        {
            BeginMethod("GetHostname");

            EndMethod();
            return new HostnameInformation() {FromDHCP = false, Name = "Hostname"};
        }

        public TestTool.Onvif.SetHostnameResponse SetHostname(TestTool.Onvif.SetHostnameRequest request)
        {
            BeginMethod("SetHostname");

            EndMethod();
            return new SetHostnameResponse();
        }

        public bool SetHostnameFromDHCP(bool FromDHCP)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.DNSInformation GetDNS()
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.SetDNSResponse SetDNS(TestTool.Onvif.SetDNSRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.NTPInformation GetNTP()
        {
            BeginMethod("GetNTP");
            NTPInformation information = new NTPInformation();
            NetworkCapabilities network = SimulatorConfiguration.ServicesConfiguration.DeviceServiceCapabilities.Network;
            if (network.NTPSpecified && network.NTP>0)
            {

            }
            else
            {
                throw new NotImplementedException();
                
            }
            
            EndMethod();

            return information;
        }

        public TestTool.Onvif.SetNTPResponse SetNTP(TestTool.Onvif.SetNTPRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.DynamicDNSInformation GetDynamicDNS()
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.SetDynamicDNSResponse SetDynamicDNS(TestTool.Onvif.SetDynamicDNSRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetNetworkInterfacesResponse GetNetworkInterfaces(TestTool.Onvif.GetNetworkInterfacesRequest request)
        {
            throw new NotImplementedException();
        }

        public bool SetNetworkInterfaces(string InterfaceToken, TestTool.Onvif.NetworkInterfaceSetConfiguration NetworkInterface)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetNetworkProtocolsResponse GetNetworkProtocols(TestTool.Onvif.GetNetworkProtocolsRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.SetNetworkProtocolsResponse SetNetworkProtocols(TestTool.Onvif.SetNetworkProtocolsRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.NetworkGateway GetNetworkDefaultGateway()
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.SetNetworkDefaultGatewayResponse SetNetworkDefaultGateway(TestTool.Onvif.SetNetworkDefaultGatewayRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.NetworkZeroConfiguration GetZeroConfiguration()
        {
            throw new NotImplementedException();
        }

        public void SetZeroConfiguration(string InterfaceToken, bool Enabled)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.IPAddressFilter GetIPAddressFilter()
        {
            throw new NotImplementedException();
        }

        public void SetIPAddressFilter(TestTool.Onvif.IPAddressFilter IPAddressFilter)
        {
            throw new NotImplementedException();
        }

        public void AddIPAddressFilter(TestTool.Onvif.IPAddressFilter IPAddressFilter)
        {
            throw new NotImplementedException();
        }

        public void RemoveIPAddressFilter(TestTool.Onvif.IPAddressFilter IPAddressFilter)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.BinaryData GetAccessPolicy()
        {
            throw new NotImplementedException();
        }

        public void SetAccessPolicy(TestTool.Onvif.BinaryData PolicyFile)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.CreateCertificateResponse CreateCertificate(TestTool.Onvif.CreateCertificateRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetCertificatesResponse GetCertificates(TestTool.Onvif.GetCertificatesRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetCertificatesStatusResponse GetCertificatesStatus(TestTool.Onvif.GetCertificatesStatusRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.SetCertificatesStatusResponse SetCertificatesStatus(TestTool.Onvif.SetCertificatesStatusRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.DeleteCertificatesResponse DeleteCertificates(TestTool.Onvif.DeleteCertificatesRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetPkcs10RequestResponse GetPkcs10Request(TestTool.Onvif.GetPkcs10RequestRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.LoadCertificatesResponse LoadCertificates(TestTool.Onvif.LoadCertificatesRequest request)
        {
            throw new NotImplementedException();
        }

        public bool GetClientCertificateMode()
        {
            throw new NotImplementedException();
        }

        public void SetClientCertificateMode(bool Enabled)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetRelayOutputsResponse GetRelayOutputs(TestTool.Onvif.GetRelayOutputsRequest request)
        {
            throw new NotImplementedException();
        }

        public void SetRelayOutputSettings(string RelayOutputToken, TestTool.Onvif.RelayOutputSettings Properties)
        {
            throw new NotImplementedException();
        }

        public void SetRelayOutputState(string RelayOutputToken, TestTool.Onvif.RelayLogicalState LogicalState)
        {
            throw new NotImplementedException();
        }

        public string SendAuxiliaryCommand(string AuxiliaryCommand)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetCACertificatesResponse GetCACertificates(TestTool.Onvif.GetCACertificatesRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.LoadCertificateWithPrivateKeyResponse LoadCertificateWithPrivateKey(TestTool.Onvif.LoadCertificateWithPrivateKeyRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetCertificateInformationResponse GetCertificateInformation(TestTool.Onvif.GetCertificateInformationRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.LoadCACertificatesResponse LoadCACertificates(TestTool.Onvif.LoadCACertificatesRequest request)
        {
            throw new NotImplementedException();
        }

        public void CreateDot1XConfiguration(TestTool.Onvif.Dot1XConfiguration Dot1XConfiguration)
        {
            throw new NotImplementedException();
        }

        public void SetDot1XConfiguration(TestTool.Onvif.Dot1XConfiguration Dot1XConfiguration)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.Dot1XConfiguration GetDot1XConfiguration(string Dot1XConfigurationToken)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetDot1XConfigurationsResponse GetDot1XConfigurations(TestTool.Onvif.GetDot1XConfigurationsRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.DeleteDot1XConfigurationResponse DeleteDot1XConfiguration(TestTool.Onvif.DeleteDot1XConfigurationRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetDot11CapabilitiesResponse GetDot11Capabilities(TestTool.Onvif.GetDot11CapabilitiesRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.Dot11Status GetDot11Status(string InterfaceToken)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.ScanAvailableDot11NetworksResponse ScanAvailableDot11Networks(TestTool.Onvif.ScanAvailableDot11NetworksRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.GetSystemUrisResponse GetSystemUris(TestTool.Onvif.GetSystemUrisRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.StartFirmwareUpgradeResponse StartFirmwareUpgrade(TestTool.Onvif.StartFirmwareUpgradeRequest request)
        {
            throw new NotImplementedException();
        }

        public TestTool.Onvif.StartSystemRestoreResponse StartSystemRestore(TestTool.Onvif.StartSystemRestoreRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override string GetServiceName()
        {
            return "Device";
        }

        protected override Type GetContractType()
        {
            return typeof (Onvif.Device);
        }

        public override string GetLocalAddress()
        {
            return Definitions.LocalAddress.DEVICE ;
        }
    }
}
