using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUT.WithLogic.Base
{
    public class AppPaths
    {
        //Device Management
        public const string PATH_DEVICEMANAGEMENT = @"Services/DeviceManagement/ONVIFDeviceManagement.asmx";
        public const string PATH_DEVICEMANAGEMENTCAPABILITIES = @"Configuration/DeviceManagement/ONVIFDeviceManagementCapabilities.xml";
        public const string PATH_DEVICEMANAGEMENTCONFIGURATION = @"Configuration/DeviceManagement/ONVIFDeviceManagementConfiguration.xml";
        public const string PATH_DEVICEMANAGEMENTCONFIGURATION_FACTORYDEFAULTS = @"Configuration/DeviceManagement/ONVIFDeviceManagementConfiguration_FactoryDefaults.xml";

        //Media2
        public const string PATH_MEDAI2 = @"Services/Media2/ONVIFMedia2.asmx";
        public const string PATH_MEDAI2CAPABILITIES = @"Configuration/Media2/ONVIFMedia2Capabilities.xml";
        public const string PATH_MEDAI2CONFIGURATION = @"Configuration/Media2/ONVIFMedia2Configuration.xml";
        public const string PATH_MEDAI2CONFIGURATION_FACTORYDEFAULTS = @"Configuration/Media2/ONVIFMedia2Configuration_FactoryDefaults.xml";


    }
}
