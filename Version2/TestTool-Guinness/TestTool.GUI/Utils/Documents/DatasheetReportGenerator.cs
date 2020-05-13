using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TestTool.GUI.Data;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Engine;
using DateTime=System.DateTime;

namespace TestTool.GUI.Utils
{
    /// <summary>
    /// Creates DataSheet report (GetCapabilities response, features as reported by device).
    /// </summary>
    class DatasheetReportGenerator: IReportGenerator
    {

        #region IReportGenerator Implementation

        public event Action<Exception> OnException;

        public event Action OnReportSaved;

        public void CreateReport(string fileName, TestLogFull log)
        {
            try
            {
                var data = new Datasheet();
                data.Features = log.Features;

                if (log.DeviceInfo != null)
                {
                    data.Capabilities = log.DeviceInformation.Capabilities;

                    data.DeviceInformation = new Tests.Engine.DeviceInformation();
                    data.DeviceInformation.FirmwareVersion = log.DeviceInformation.FirmwareVersion;
                    data.DeviceInformation.HardwareID = log.DeviceInformation.HardwareID;
                    data.DeviceInformation.Manufacturer = log.DeviceInformation.Manufacturer;
                    data.DeviceInformation.Model = log.DeviceInformation.Model;
                    data.DeviceInformation.SerialNumber = log.DeviceInformation.SerialNumber;

                }
                data.TestInformation = new TestInformation();
                data.TestInformation.TestDate = log.TestExecutionTime;
                if (log.DeviceInfo != null)
                {
                    data.TestInformation.ProductName = log.DeviceInfo.ProductName;
                }
                if (log.Application != null)
                {
                    data.TestInformation.ToolVersion = log.Application.ToolVersion;
                }

                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    var serializer = new XmlSerializer(typeof(Datasheet));
                    serializer.Serialize(fileStream, data);
                }
                RaiseOnReportSaved();
            }
            catch (Exception ex)
            {
                RaiseOnException(ex);
            }
        }

        #endregion

        #region Private Methods

        private void RaiseOnException(Exception exception)
        {
            if (OnException != null)
            {
                OnException(exception);
            }
        }

        private void RaiseOnReportSaved()
        {
            if (OnReportSaved != null)
            {
                OnReportSaved();
            }
        }

        #endregion
    }

    public class TestInformation
    {
        public DateTime TestDate { get; set; }

        public string ProductName { get; set; }

        public string ToolVersion { get; set; }
    }

    public class Datasheet
    {
        public DeviceInformation DeviceInformation { get; set; }

        public TestInformation TestInformation { get; set; }

        public Capabilities Capabilities { get; set; }

        public List<Feature> Features { get; set; }
    }
}
