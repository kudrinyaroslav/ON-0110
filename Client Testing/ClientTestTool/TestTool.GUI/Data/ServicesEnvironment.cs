﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.GUI.Data
{
    class ServicesEnvironment
    {
        private string _baseAddress;
        private string _deviceServiceAddress;
        public string BaseAddress
        {
            get
            {
                return _baseAddress;
            }
            set
            {
                _baseAddress = value;
                _deviceServiceAddress = GetDeviceServiceAddress(_baseAddress);
            }
        }

        public string DeviceServiceAddress
        {
            get { return _deviceServiceAddress; }
        }

        public static string GetDeviceServiceAddress(string baseAddress)
        {
            return baseAddress + "onvif/device_service";
        }

        public Device.AuthenticationMode AuthenticationMode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
