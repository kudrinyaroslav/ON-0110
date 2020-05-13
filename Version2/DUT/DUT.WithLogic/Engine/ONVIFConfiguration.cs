using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUT.WithLogic.Engine
{
    public class ONVIFConfiguration
    {
        Services.DeviceManagement.ONVIFDeviceManagementCapabilities m_ONVIFDeviceManagementCapabilities;
        Services.Media2.ONVIFMedia2Capabilities m_ONVIFMedia2Capabilities;

        public Services.DeviceManagement.ONVIFDeviceManagementCapabilities ONVIFDeviceManagementCapabilities
        {
            get {
                if (m_ONVIFDeviceManagementCapabilities == null)
                {
                    m_ONVIFDeviceManagementCapabilities = Services.DeviceManagement.ONVIFDeviceManagementCapabilities.Load();
                }
                return m_ONVIFDeviceManagementCapabilities;
            }
        }

        public Services.Media2.ONVIFMedia2Capabilities ONVIFMedia2Capabilities
        {
            get
            {
                if (m_ONVIFMedia2Capabilities == null)
                {
                    m_ONVIFMedia2Capabilities = Services.Media2.ONVIFMedia2Capabilities.Load();
                }
                return m_ONVIFMedia2Capabilities;
            }
        }
    }
}