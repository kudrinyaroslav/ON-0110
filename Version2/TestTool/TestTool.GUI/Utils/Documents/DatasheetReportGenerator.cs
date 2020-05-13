using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TestTool.GUI.Data;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Engine;
using DateTime=System.DateTime;
using System.Linq;


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

                if (log.DeviceInformation != null)
                {
                    data.Capabilities = log.DeviceInformation.Capabilities;

                    data.DeviceInformation = new DeviceInformation();
                    data.DeviceInformation.FirmwareVersion = log.DeviceInformation.FirmwareVersion;
                    data.DeviceInformation.HardwareID = log.DeviceInformation.HardwareID;
                    data.DeviceInformation.Manufacturer = log.DeviceInformation.Manufacturer;
                    data.DeviceInformation.Model = log.DeviceInformation.Model;
                    data.DeviceInformation.SerialNumber = log.DeviceInformation.SerialNumber;

                }
                data.TestInformation = new TestInformation();
                data.TestInformation.TestDate = log.TestExecutionTime;
                data.TestInformation.ProductName = log.ProductName;

                // this code block is commented until full list of product type will be available
                /*
                if (!string.IsNullOrEmpty(log.ProductTypes))
                {
                    string[] delimiters = { ", " };
                    string[] types = log.ProductTypes.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    if (types != null && types.Length != 0)
                    {
                        data.TestInformation.ProductType = new List<TestInformation.Type>();
                        foreach (var type in types)
                        {
                            switch (type)
                            {
                                case "Camera":
                                    data.TestInformation.ProductType.Add(TestInformation.Type.Camera);
                                    break;
                                case "Encoder":
                                    data.TestInformation.ProductType.Add(TestInformation.Type.Encoder);
                                    break;
                                case "Recorder":
                                    data.TestInformation.ProductType.Add(TestInformation.Type.Recorder);
                                    break;
                                case "VMS":
                                    data.TestInformation.ProductType.Add(TestInformation.Type.VMS);
                                    break;
                                case "Display":
                                    data.TestInformation.ProductType.Add(TestInformation.Type.Display);
                                    break;
                                case "Dome":
                                    data.TestInformation.ProductType.Add(TestInformation.Type.Dome);
                                    break;
                            }
                        }
                    }
                }
               */

                if (!string.IsNullOrEmpty(log.ProductTypes))
                {
                    string[] delimiters = { ", " };
                    string[] types = log.ProductTypes.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    if (types != null && types.Length != 0)
                    {
                        data.TestInformation.ProductType = new List<string>(types);
                    }
                }

                // conformance information
                var countOfFailedTests = log.TestResults.Count(item => item.Value.Log.TestStatus == TestTool.Tests.Definitions.Trace.TestStatus.Failed);
                var isConformanceFailed = (countOfFailedTests > 0)
                           || !log.InitializationData.SupportedProfiles.Any(e => ProfileVersionStatus.Release == e.GetProfileVersionStatus())
                           || log.InitializationData.FailedProfiles.Any(e => ProfileVersionStatus.Release == e.GetProfileVersionStatus());
                isConformanceFailed = isConformanceFailed || log.InitializationData.UndefinedFeatures.Any();

                if (!isConformanceFailed && (log.InitializationData.SupportedProfiles != null || log.InitializationData.SupportedProfiles.Count != 0 ))
                {
                    data.ConformanceInformation = new ConformanceInformation();
                    data.ConformanceInformation.SupportedProfile = new List<ConformanceInformation.Profile>();
                    foreach (var profile in log.InitializationData.SupportedProfiles)
                    {
                        if (profile.GetProfileVersionStatus() == ProfileVersionStatus.Release)
                        {
                            switch (profile.GetProfileName())
                            {
                                case "Profile S":
                                    data.ConformanceInformation.SupportedProfile.Add(ConformanceInformation.Profile.S);
                                    break;
                                case "Profile G":
                                    data.ConformanceInformation.SupportedProfile.Add(ConformanceInformation.Profile.G);
                                    break;
                                case "Profile C":
                                    data.ConformanceInformation.SupportedProfile.Add(ConformanceInformation.Profile.C);
                                    break;
                            }
                        }
                    }
                }

                if (log.SupportInfo != null)
                {
                    data.SupportInformation = new SupportInfo();

                    data.SupportInformation.InternationalAddress = 
                        string.IsNullOrEmpty(log.SupportInfo.InternationalAddress) ? null : log.SupportInfo.InternationalAddress;

                    data.SupportInformation.RegionalAddress =
                        string.IsNullOrEmpty(log.SupportInfo.RegionalAddress) ? null : log.SupportInfo.RegionalAddress;

                    data.SupportInformation.SupportUrl =
                        string.IsNullOrEmpty(log.SupportInfo.SupportUrl) ? null : log.SupportInfo.SupportUrl;

                    data.SupportInformation.SupportEmail =
                        string.IsNullOrEmpty(log.SupportInfo.SupportEmail) ? null : log.SupportInfo.SupportEmail;

                    data.SupportInformation.SupportPhone =
                        string.IsNullOrEmpty(log.SupportInfo.SupportPhone) ? null : log.SupportInfo.SupportPhone;
                }

                data.RequestsTimeouts = log.Timeouts;

                if (log.Application != null)
                {
                    data.TestInformation.ToolVersion = log.Application.ToolVersion;
                }

                data.ManagementSettings = log.ManagementSettings;

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

        //public enum Type { Camera, Encoder, Recorder, VMS, Display, Dome }
        //public List<Type> ProductType { get; set; }

        public List<string> ProductType { get; set; }

        public string ToolVersion { get; set; }
    }

    public class ConformanceInformation
    {
        public enum Profile { S, G, C }

        public List<Profile> SupportedProfile { get ; set; }
    }

    public class Datasheet
    {
        public DeviceInformation DeviceInformation { get; set; }

        public TestInformation TestInformation { get; set; }

        public ConformanceInformation ConformanceInformation { get; set; }

        public SupportInfo SupportInformation { get; set; }

        public RealTimeouts RequestsTimeouts { get; set; }

        public Data.Log.ManagementSettings ManagementSettings { get; set; }

        public Capabilities Capabilities { get; set; }

        public List<Feature> Features { get; set; }
    }
}
