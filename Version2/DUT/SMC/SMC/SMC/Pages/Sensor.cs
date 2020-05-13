using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMC.Pages
{
    enum SensorDeviceType
    { 
        Door,
        Credential
    }

    class Sensor
    {
        public Sensor()
        {

        }

        public Sensor(string deviceToken, string name, SensorDeviceType sensorDeviceType)
        {
            DeviceToken = deviceToken;
            SensorDeviceType = sensorDeviceType;
            Name = name;
        }

        public string DeviceToken { get; set; }

        public SensorDeviceType SensorDeviceType { get; set; }

        public string Name { get; set; }

        private List<string> _values = new List<string>();
        public List<string> Values
        {
            get { return _values; }
        }
    }
}
