using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceDoorControl10;

namespace DUT.PACS.Simulator
{
    public enum Sensor
    {
        DoorMonitor,
        DoorLockMonitor,
        DoorDoubleLockMonitor
    }

    public class TriggerSettings
    {
        public string DoorToken { get; set; }
        public DoorMode DoorMode { get; set; }
        public Sensor Sensor { get; set; }
        public int Timeout { get; set; }
    }

    public class TriggerConfiguration
    {
        List<TriggerSettings> _settings = new List<TriggerSettings>();

        public List<TriggerSettings> Settings
        {
            get { return _settings; }
        }

        public void Add(string doorToken, DoorMode doorMode, Sensor sensor, int timeout)
        {
            _settings.Add(new TriggerSettings()
                              {DoorToken = doorToken, DoorMode = doorMode, Sensor = sensor, Timeout = timeout});
        }

    }
}
