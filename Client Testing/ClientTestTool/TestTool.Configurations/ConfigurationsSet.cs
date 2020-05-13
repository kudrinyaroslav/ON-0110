using System;
using System.Collections.Generic;
using TestTool.Common.Attributes;
using TestTool.Common.Configuration;
using TestTool.Onvif;

namespace TestTool.Configurations
{
    [TestClass]
    public class ConfigurationsSet
    {
        [Test(Name = "Configuration 01", Id = "001", Path = "Predefined")]
        public SimulatorConfiguration NormalConfiguration()
        {
            SimulatorConfiguration configuration = new SimulatorConfiguration();
            configuration.ServicesConfiguration = new ServicesConfiguration();

            DeviceServiceCapabilities deviceServiceCapabilities = new DeviceServiceCapabilities();

            deviceServiceCapabilities.Network = new NetworkCapabilities();
            deviceServiceCapabilities.Network.Dot11Configuration = true;
            deviceServiceCapabilities.Network.Dot11ConfigurationSpecified = true;
            deviceServiceCapabilities.Network.DynDNS = true;
            deviceServiceCapabilities.Network.Dot11ConfigurationSpecified = true;
            deviceServiceCapabilities.Network.HostnameFromDHCP = true;
            deviceServiceCapabilities.Network.HostnameFromDHCPSpecified = true;
            deviceServiceCapabilities.Network.IPFilter = false;
            deviceServiceCapabilities.Network.IPFilterSpecified = true;
            deviceServiceCapabilities.Network.IPVersion6 = false;
            deviceServiceCapabilities.Network.IPVersion6Specified = true;
            deviceServiceCapabilities.Network.NTP = 1;
            deviceServiceCapabilities.Network.NTPSpecified = true;
            deviceServiceCapabilities.Network.ZeroConfiguration = false;
            deviceServiceCapabilities.Network.ZeroConfigurationSpecified = true;

            deviceServiceCapabilities.Security = new SecurityCapabilities();

            deviceServiceCapabilities.System = new SystemCapabilities();
            deviceServiceCapabilities.System.HttpSystemBackup = true;
            deviceServiceCapabilities.System.HttpSystemBackupSpecified = true;
            deviceServiceCapabilities.System.SystemLogging = true;
            deviceServiceCapabilities.System.SystemLoggingSpecified = true;

            configuration.ServicesConfiguration.DeviceServiceCapabilities = deviceServiceCapabilities;
            configuration.ServicesConfiguration.CreateOldStyleCapabilities();


            configuration.DeviceInformation = new DeviceInformation();
            configuration.DeviceInformation.Brand = "ONVIF";
            configuration.DeviceInformation.FirmwareVersion = "1.0";
            configuration.DeviceInformation.HardwareId = "12345";
            configuration.DeviceInformation.Model = "Ideal PACS Device";
            configuration.DeviceInformation.SerialNumber = "123456789";

            AccessControlServiceCapabilities accessControlServiceCapabilities = new AccessControlServiceCapabilities();
            accessControlServiceCapabilities.DisableAccessPoint = true;
            accessControlServiceCapabilities.DisableAccessPointSpecified = true;
            accessControlServiceCapabilities.MaxLimit = 10;

            configuration.ServicesConfiguration.AccessControlCapabilities = accessControlServiceCapabilities;

            DoorControlServiceCapabilities doorControlServiceCapabilities = new DoorControlServiceCapabilities();
            doorControlServiceCapabilities.MaxLimit = 3;
            configuration.ServicesConfiguration.DoorServiceCapabilities = doorControlServiceCapabilities;

            configuration.ServicesConfiguration.InitializeXmlElements();

            List<Service> services = new List<Service>();

            {
                Service device = new Service();
                device.Namespace = Common.Definitions.OnvifService.DEVICE;
                device.Version = new OnvifVersion();
                device.Version.Major = 2;
                device.Version.Minor = 2;
                services.Add(device);
            }
            {
                Service events = new Service();
                events.Namespace = Common.Definitions.OnvifService.EVENTS;
                events.Version = new OnvifVersion();
                events.Version.Major = 2;
                events.Version.Minor = 2;
                services.Add(events);
            }

            {
                Service pacs = new Service();
                pacs.Namespace = Common.Definitions.OnvifService.ACCESSCONTROL;
                pacs.Version = new OnvifVersion();
                pacs.Version.Major = 2;
                pacs.Version.Minor = 2;
                services.Add(pacs);
            }
            {
                Service doorControl = new Service();
                doorControl.Namespace = Common.Definitions.OnvifService.DOORCONTROL;
                doorControl.Version = new OnvifVersion();
                doorControl.Version.Major = 2;
                doorControl.Version.Minor = 2;
                services.Add(doorControl);
            }

            configuration.ServicesConfiguration.Services = services;

            List<Scope> scopes = new List<Scope>();
            scopes.Add(new Scope() { ScopeItem = "onvif://www.onvif.org/profile/profilec" });
            scopes.Add(new Scope() { ScopeItem = "onvif://www.onvif.org/name/Simulator" });
            scopes.Add(new Scope() { ScopeItem = "onvif://www.onvif.org/hardware/PC" });
            scopes.Add(new Scope() { ScopeItem = "onvif://www.onvif.org/location/scope1" });

            configuration.Scopes = scopes;


            #region PACS initialization
            configuration.PacsConfiguration = new PacsConfiguration();

            {
                AccessPointInfo info = new AccessPointInfo();
                info.token = "tokenAccessPoint1";
                info.Name = "AccessPoint1 Name";
                info.Description = "AccessPoint1 Description";
                info.AreaFrom = "tokenArea1";
                info.AreaTo = "tokenArea2";
                info.Enabled = true;
                info.Type = "tdc:Door";
                info.Entity = "tokenDoor1";
                info.Capabilities = new AccessPointCapabilities();
                info.Capabilities.DisableAccessPoint = true;
                configuration.PacsConfiguration.AccessPointInfoList.Add(info);
            }

            {
                AccessPointInfo info = new AccessPointInfo();
                info.token = "tokenAccessPoint2";
                info.Name = "AccessPoint2 Name";
                info.Description = "AccessPoint2 Description";
                info.Enabled = true;
                info.Type = "tdc:Door";
                info.Entity = "tokenDoor1";
                info.Capabilities = new AccessPointCapabilities();
                info.Capabilities.DisableAccessPoint = false;
                configuration.PacsConfiguration.AccessPointInfoList.Add(info);
            }

            {
                AreaInfo info = new AreaInfo();
                info.token = "tokenArea1";
                info.Name = "Area1 Name";
                info.Description = "Area1 Description";
                configuration.PacsConfiguration.AreaInfoList.Add(info);
            }
            {
                AreaInfo info = new AreaInfo();
                info.token = "tokenArea2";
                info.Name = "Area2 Name";
                info.Description = "Area2Description";
                configuration.PacsConfiguration.AreaInfoList.Add(info);
            }

            {
                DoorInfo info = new DoorInfo();

                info.token = "tokenDoor1";
                info.Name = "Door1 Name";
                info.Description = "Door1 Description";
                DoorCapabilities value = new DoorCapabilities();
                value.Block = true;
                value.DoubleLock = true;
                value.Lock = true;
                value.LockDown = true;
                value.LockOpen = true;
                value.MomentaryAccess = true;
                value.Unlock = true;
                info.Capabilities = value;
                configuration.PacsConfiguration.DoorInfoList.Add(info);
            }

            {
                DoorInfo info = new DoorInfo();

                info.token = "tokenDoor2";
                info.Name = "Door2 Name";
                info.Description = "Door2 Description";

                DoorCapabilities value = new DoorCapabilities();
                value.Block = false;
                value.DoubleLock = false;
                value.Lock = false;
                value.LockDown = false;
                value.LockOpen = false;
                value.MomentaryAccess = false;
                value.Unlock = false;

                info.Capabilities = value;

                configuration.PacsConfiguration.DoorInfoList.Add(info);
            }

            foreach (DoorInfo door in configuration.PacsConfiguration.DoorInfoList)
            {
                configuration.PacsConfiguration.DoorCapabilitiesList.Add(door.token, door.Capabilities);
                configuration.PacsConfiguration.DoorAccessList.Add(door.token, 0);
                configuration.PacsConfiguration.DoorAccessPreviousStateList.Add(door.token, DoorModeType.Unknown);
            }

            {
                DoorState value = new DoorState();

                value.DoorAlarm = DoorAlarmStateType.Normal;
                value.DoorDoubleLockMonitor = DoorLockMonitorStateType.Locked;
                value.DoorDoubleLockMonitorSpecified = true;
                value.DoorLockMonitor = DoorLockMonitorStateType.Locked;
                value.DoorMode = DoorModeType.Locked;
                value.DoorMonitor = DoorMonitorStateType.Closed;
                value.DoorTamper = DoorTamperStateType.NotInTamper;

                configuration.PacsConfiguration.DoorStateList.Add("tokenDoor1", value);
            }

            {
                DoorState value = new DoorState();

                value.DoorAlarm = DoorAlarmStateType.Normal;
                value.DoorDoubleLockMonitor = DoorLockMonitorStateType.NotSupported;
                value.DoorDoubleLockMonitorSpecified = false;
                value.DoorLockMonitor = DoorLockMonitorStateType.NotSupported;
                value.DoorMode = DoorModeType.Locked;
                value.DoorMonitor = DoorMonitorStateType.NotSupported;
                value.DoorTamper = DoorTamperStateType.NotSupported;

                configuration.PacsConfiguration.DoorStateList.Add("tokenDoor2", value);
            }

            #endregion

            return configuration;
        }
    }
}
