///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

using System;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public partial class DeviceManagementSystemTestSuite : Base.DeviceManagementTest
    {
        public DeviceManagementSystemTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Device Management\\System";

        [Test(Name = "SYSTEM COMMAND GETSYSTEMDATEANDTIME",
            Path = PATH,
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.DEVICE,
            Version = 1.02,
            FunctionalityUnderTest =  new Functionality[]{Functionality.GetSystemDateAndTime},
            RequirementLevel = RequirementLevel.Must)]
        public void GetSystemDateTimeTest()
        {
            RunTest( () =>
                         {
                            SystemDateTime dateTime = GetSystemDateAndTime();

                            Assert(dateTime != null, "Date and time settings not found",
                                   "Check that DUT returned date and time settings");

                            if (dateTime.TimeZone != null)
                            {

                                TimeZoneDescription actualDescription = new TimeZoneDescription();
                                string reason;
                                bool bParseOk = ParseTimeZone(dateTime.TimeZone.TZ, actualDescription, out reason);
                                Assert(bParseOk, string.Format("Time zone format is not correct: {0}", reason),
                                       "Validate TimeZone string", string.Format("TimeZone: {0}", dateTime.TimeZone.TZ));
                            }
                            else
                            {
                                WriteStep("Validate TimeZone", "TimeZone is null");
                            }

                             bool bCorrect = true;

                             //
                             // ToDo : UTCDateTime should be mandatory for SetDateTimeType.Manual
                             // (Release notes to Core Specification 2.0 - Errata)
                             //
                             //
                             // 
                             string dump = null;
                             if (dateTime.DateTimeType == SetDateTimeType.Manual)
                             {
                                bCorrect = (dateTime.UTCDateTime != null);
                                dump = string.Format("DateTimeType: Manual; LocalDateTime: {0}; UTCDateTime: {1}", 
                                     dateTime.LocalDateTime == null ? "NOT PRESENT" : "PRESENT", 
                                     dateTime.UTCDateTime == null ? "NOT PRESENT" : "PRESENT");
                             }
                             else
                             {
                                 dump = "DateTimeType: NTP";
                             }

                            Assert(bCorrect, "DateTimeType is Manual but UTCDateTime is not set", "Check if settings are self-consistent", dump);

                            if (dateTime.LocalDateTime != null)
                            {
                                string errorMessage;
                                bool bDateTimeValid = dateTime.LocalDateTime.IsValidDateTime("LocalDateTime", out errorMessage);
                                Assert(bDateTimeValid, errorMessage, "Validate LocalDateTime");
                            }
                            else
                            {
                                WriteStep("Validate LocalDateTime", "LocalDateTime is NULL");
                            }

                            // UTCDateTime  must be not null (Core Spec 2.0)
                            if (dateTime.UTCDateTime != null)
                            {
                                string errorMessage;
                                bool bDateTimeValid = dateTime.UTCDateTime.IsValidDateTime("UTCDateTime", out errorMessage);
                                Assert(bDateTimeValid, errorMessage, "Validate UTCDateTime");
                            }
                             else
                             {
                                 WriteStep("Validate UTCDateTime", "UTCDateTime is NULL");
                             }

                         });

        }

        [Test(Name = "SYSTEM COMMAND SETSYSTEMDATEANDTIME",
            Path = PATH,
            Order = "03.01.11",
            Id = "3-1-11",
            Category = Category.DEVICE,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetSystemDateAndTime },
            RequirementLevel = RequirementLevel.Must)]
        public void SetSystemDateTimeTest()
        {

            SystemDateTime initialDateTimeSettings = null;
            bool restoreSettings = false;
            System.DateTime startTimePoint = new System.DateTime();
            RunTest(() =>
                    {
                        initialDateTimeSettings = GetSystemDateAndTime();
                        startTimePoint = System.DateTime.UtcNow;

                        var testDateTimeValue = new SystemDateTime();
                        var dateTime = new TestTool.Proxies.Onvif.DateTime { Date = new Date(), Time = new Time() };

                        var nowUtc = System.DateTime.UtcNow;

                        dateTime.Time.Hour = nowUtc.Hour;
                        dateTime.Time.Minute = nowUtc.Minute;
                        dateTime.Time.Second = nowUtc.Second;

                        dateTime.Date.Day = nowUtc.Day;
                        dateTime.Date.Month = nowUtc.Month;
                        dateTime.Date.Year = nowUtc.Year;

                        testDateTimeValue.DateTimeType = SetDateTimeType.Manual;
                        testDateTimeValue.DaylightSavings = false;
                        testDateTimeValue.UTCDateTime = dateTime;

                        SetSystemDateAndTime(testDateTimeValue);
                        restoreSettings = true;
                
                        SystemDateTime actualDateTime = GetSystemDateAndTime();

                        Assert(actualDateTime != null, "Date and time settings not found",
                               "Check that DUT returned date and time settings");

                        Assert(actualDateTime.DateTimeType == testDateTimeValue.DateTimeType, 
                            "DateTimeType has not been set", 
                            "Check that DateTimeType has been set. ",
                            string.Format("Expected: {0}, actual: {1}",
                            testDateTimeValue.DateTimeType, actualDateTime.DateTimeType));

                        Assert(actualDateTime.DaylightSavings == testDateTimeValue.DaylightSavings, 
                            "DaylightSavings not set", 
                            "Check that DaylightSavings has been set. ",
                            string.Format("Expected: {0}, actual: {1}",
                            testDateTimeValue.DaylightSavings, actualDateTime.DaylightSavings));

                        //Assert((actualDateTime.TimeZone != null), "The DUT did not return TimeZone settings",
                        //       "Check that DUT returned TimeZone settings");

                        //ValidateTimeZone(testDateTimeValue.TimeZone.TZ, actualDateTime.TimeZone.TZ);

                        bool bCorrect = true;
                        string dump = null;
                        if (actualDateTime.DateTimeType == SetDateTimeType.Manual)
                        {
                            bCorrect = (actualDateTime.UTCDateTime != null);
                            dump = string.Format("DateTimeType: Manual; LocalDateTime: {0}; UTCDateTime: {1}",
                                 actualDateTime.LocalDateTime == null ? "NOT PRESENT" : "PRESENT",
                                 actualDateTime.UTCDateTime == null ? "NOT PRESENT" : "PRESENT");
                        }
                        else
                        {
                            dump = "DateTimeType: NTP";
                        }

                        Assert(bCorrect, "DateTimeType is Manual but UTCDateTime is not set", "Check if settings are self-consistent", dump);
            
                        if (actualDateTime.LocalDateTime != null)
                        {
                            string errorMessage;
                            bool bDateTimeValid = actualDateTime.LocalDateTime.IsValidDateTime("LocalDateTime", out errorMessage);
                            Assert(bDateTimeValid, errorMessage, "Validate LocalDateTime");
                        }
                        else
                        {
                            WriteStep("Validate LocalDateTime", "LocalDateTime is NULL");
                        }

                        if (actualDateTime.UTCDateTime != null)
                        {
                            string errorMessage;
                            bool bDateTimeValid = actualDateTime.UTCDateTime.IsValidDateTime("UTCDateTime", out errorMessage);
                            Assert(bDateTimeValid, errorMessage, "Validate UTCDateTime");
                
                        }
                        else
                        {
                            WriteStep("Validate UTCDateTime", "UTCDateTime is NULL");
                        }

                    },
                    () =>
                    {
                        if (restoreSettings && null != initialDateTimeSettings)
                            SynchronizeTime(initialDateTimeSettings);
                    });

        }

        [Test(Name = "SYSTEM COMMAND SETSYSTEMDATEANDTIME USING NTP",
            Path = PATH,
            Order = "03.01.12",
            Id = "3-1-12",
            Category = Category.DEVICE,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetSystemDateAndTime },
            RequiredFeatures = new Feature[] {Feature.NTP},
            RequirementLevel = RequirementLevel.Must)]
        public void SetSystemDateTimeNTPTest()
        {
            NTPInformation initialNTPSettings = null;
            SystemDateTime initialDateTimeSettings = null;
            bool restoreSettings = false;
            RunTest(() =>
                    {
                        initialDateTimeSettings = GetSystemDateAndTime();
                        initialNTPSettings = GetNTP();

                        var ntp = new NTPInformation { FromDHCP = false, NTPManual = new[] { new NetworkHost() } };

                        ntp.NTPManual[0].Type = NetworkHostType.IPv4;
                        ntp.NTPManual[0].IPv4Address = _environmentSettings.NtpIpv4;

                        SetNTP(ntp);
                        restoreSettings = true;

                        var testDateTimeSettins = new SystemDateTime
                                                  {
                                                      DateTimeType = SetDateTimeType.NTP,
                                                      DaylightSavings = true,
                                                      TimeZone = new TestTool.Proxies.Onvif.TimeZone { TZ = "PST8PDT,M3.2.0,M11.1.0" }
                                                  };

                        SetSystemDateAndTime(testDateTimeSettins);

                        SystemDateTime actualDateTime = GetSystemDateAndTime();

                        Assert(actualDateTime != null, "Date and time settings not found",
                               "Check that DUT returned date and time settings");

                        Assert(actualDateTime.DateTimeType == testDateTimeSettins.DateTimeType,
                               "DateTimeType has not been set",
                               "Check that DateTimeType has been set. ",
                               string.Format("Expected: {0}, actual: {1}",
                               testDateTimeSettins.DateTimeType, actualDateTime.DateTimeType));

                        Assert(actualDateTime.DaylightSavings == testDateTimeSettins.DaylightSavings,
                               "DaylightSavings not set",
                               "Check that DaylightSavings has been set. ",
                               string.Format("Expected: {0}, actual: {1}",
                               testDateTimeSettins.DaylightSavings, actualDateTime.DaylightSavings));

                        Assert(actualDateTime.TimeZone != null, "The DUT did not return TimeZone settings",
                               "Check that DUT returned TimeZone settings");

                        ValidateTimeZone(testDateTimeSettins.TimeZone.TZ, actualDateTime.TimeZone.TZ);

                        if (actualDateTime.LocalDateTime != null)
                        {
                            string errorMessage;
                            bool bDateTimeValid = actualDateTime.LocalDateTime.IsValidDateTime("LocalDateTime", out errorMessage);
                            Assert(bDateTimeValid, errorMessage, "Validate LocalDateTime");
                        }
                        else
                        {
                            WriteStep("Validate LocalDateTime", "LocalDateTime is NULL");
                        }


                        if (actualDateTime.UTCDateTime != null)
                        {
                            string errorMessage;
                            bool bDateTimeValid = actualDateTime.UTCDateTime.IsValidDateTime("UTCDateTime", out errorMessage);
                            Assert(bDateTimeValid, errorMessage, "Validate UTCDateTime");
                        }
                        else
                        {
                            WriteStep("Validate UTCDateTime", "UTCDateTime is NULL");
                        }
                    },
                    () =>
                    {
                        if (restoreSettings)
                        {
                            if (null != initialDateTimeSettings)
                                SynchronizeTime(initialDateTimeSettings);

                            if (null != initialNTPSettings)
                                SetNTP(initialNTPSettings);
                        }
                    });
        }

        [Test(Name = "SYSTEM COMMAND SETSYSTEMDATEANDTIME TEST FOR INVALID TIMEZONE",
            Path = PATH,
            Order = "03.01.04",
            Id = "3-1-4",
            Category = Category.DEVICE,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetSystemDateAndTime },
            RequirementLevel = RequirementLevel.Must)]
        public void SetSystemDateTimeInvalidTimezoneTest()
        {
            RunTest( () =>
                         {
                            Proxies.Onvif.DateTime dateTime = new Proxies.Onvif.DateTime();

                            Proxies.Onvif.TimeZone timeZone = new Proxies.Onvif.TimeZone();

                            timeZone.TZ = "INVALIDTIMEZONE";

                            dateTime.Date = new Date();
                            dateTime.Time = new Time();

                            dateTime.Date.Day = System.DateTime.Now.Day;
                            dateTime.Date.Month = System.DateTime.Now.Month;
                            dateTime.Date.Year = System.DateTime.Now.Year;

                            dateTime.Time.Hour = System.DateTime.Now.Hour;
                            dateTime.Time.Minute = System.DateTime.Now.Minute;
                            dateTime.Time.Second = System.DateTime.Now.Second;

                             RunStep(() => Client.SetSystemDateAndTime(SetDateTimeType.Manual, true, timeZone, dateTime), 
                                     "Set system date and time - negative test",
                                     "Sender/InvalidArgVal/InvalidTimeZone", false);

                            SystemDateTime actualDateTime = GetSystemDateAndTime();

                            Assert(actualDateTime != null, "Date and time settings not found",
                                   "Check that DUT returned date and time settings");

                            Assert(actualDateTime.TimeZone != null, "The DUT did not return TimeZone settings",
                                   "Check that DUT returned TimeZone settings");

                            bool bCorrect = true;
                            string dump = null;

                            if (actualDateTime.DateTimeType == SetDateTimeType.Manual)
                            {
                                bCorrect = (actualDateTime.UTCDateTime != null);
                                dump = string.Format("DateTimeType: Manual; LocalDateTime: {0}; UTCDateTime: {1}",
                                     actualDateTime.LocalDateTime == null ? "NOT PRESENT" : "PRESENT",
                                     actualDateTime.UTCDateTime == null ? "NOT PRESENT" : "PRESENT");
                            }
                            else
                            {
                                dump = "DateTimeType: NTP";
                            }

                            Assert(bCorrect, "DateTimeType is Manual but UTCDateTime is not set", "Check if settings are self-consistent", dump);

                            if (actualDateTime.LocalDateTime != null)
                            {
                                string errorMessage;
                                bool bDateTimeValid = actualDateTime.LocalDateTime.IsValidDateTime("LocalDateTime", out errorMessage);
                                Assert(bDateTimeValid, errorMessage, "Validate LocalDateTime");
                            }
                            else
                            {
                                WriteStep("Validate LocalDateTime", "LocalDateTime is NULL");
                            }

                            if (actualDateTime.UTCDateTime != null)
                            {
                                string errorMessage;
                                bool bDateTimeValid = actualDateTime.UTCDateTime.IsValidDateTime("UTCDateTime", out errorMessage);
                                Assert(bDateTimeValid, errorMessage, "Validate UTCDateTime");
                            }
                            else
                            {
                                WriteStep("Validate UTCDateTime", "UTCDateTime is NULL");
                            }

                         },
                         () =>
                             {
                                 SynchronizeTime();
                             });

        }

        [Test(Name = "SYSTEM COMMAND SETSYSTEMDATEANDTIME TEST FOR INVALID DATE",
            Path = PATH,
            Order = "03.01.05",
            Id = "3-1-5",
            Category = Category.DEVICE,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetSystemDateAndTime },
            RequirementLevel = RequirementLevel.Must)]
        public void SetSystemDateTimeInvalidDateTimeTest()
        {
            RunTest( 
                () =>
                         {
                            Proxies.Onvif.DateTime dateTime = new Proxies.Onvif.DateTime();

                            Proxies.Onvif.TimeZone timeZone = new Proxies.Onvif.TimeZone();

                            timeZone.TZ = "PST8PDT,M3.2.0,M11.1.0";

                            dateTime.Date = new Date();
                            dateTime.Time = new Time();

                            dateTime.Date.Day = 32;
                            dateTime.Date.Month = 13;
                            dateTime.Date.Year = 2001;

                            dateTime.Time.Hour = 25;
                            dateTime.Time.Minute = 65;
                            dateTime.Time.Second = 70;

                            RunStep(() => Client.SetSystemDateAndTime(SetDateTimeType.Manual, true, timeZone, dateTime),
                                    "Set system date and time - negative test",
                                    "Sender/InvalidArgVal/InvalidDateTime", false);
                             

                            SystemDateTime actualDateTime = GetSystemDateAndTime();

                            Assert(actualDateTime != null, "Date and time settings not found",
                                   "Check that DUT returned date and time settings");

                            //Assert(actualDateTime.TimeZone != null, "The DUT did not return TimeZone settings",
                            //       "Check that DUT returned TimeZone settings");

                            //ValidateTimeZone(testDateTimeSettins.TimeZone.TZ, actualDateTime.TimeZone.TZ);

                            if (actualDateTime.TimeZone != null)
                            {
                                TimeZoneDescription actualDescription = new TimeZoneDescription();
                                string tzdump;
                                bool bParseOk = ParseTimeZone(actualDateTime.TimeZone.TZ, actualDescription, out tzdump);
                                Assert(bParseOk, string.Format("Time zone format is not correct: {0}", tzdump),
                                       "Validate TimeZone string", string.Format("TimeZone: {0}", actualDateTime.TimeZone.TZ));
                            }
                            else
                            {
                                WriteStep("Validate TimeZone", "TimeZone is null");
                            }

                            bool bCorrect = true;
                            string dump = null;

                            if (actualDateTime.DateTimeType == SetDateTimeType.Manual)
                            {
                                bCorrect = (actualDateTime.UTCDateTime != null);
                                dump = string.Format("DateTimeType: Manual; LocalDateTime: {0}; UTCDateTime: {1}",
                                     actualDateTime.LocalDateTime == null ? "NOT PRESENT" : "PRESENT",
                                     actualDateTime.UTCDateTime == null ? "NOT PRESENT" : "PRESENT");
                            }
                            else
                            {
                                dump = "DateTimeType: NTP";
                            }

                            Assert(bCorrect, "DateTimeType is Manual but UTCDateTime is not set", "Check if settings are self-consistent", dump);
                            
                            if (actualDateTime.LocalDateTime != null)
                            {
                                string errorMessage;
                                bool bDateTimeValid = actualDateTime.LocalDateTime.IsValidDateTime("LocalDateTime", out errorMessage);
                                Assert(bDateTimeValid, errorMessage, "Validate LocalDateTime");
                            }
                            else
                            {
                                WriteStep("Validate LocalDateTime", "LocalDateTime is NULL");
                            }

                            if (actualDateTime.UTCDateTime != null)
                            {
                                string errorMessage;
                                bool bDateTimeValid = actualDateTime.UTCDateTime.IsValidDateTime("UTCDateTime", out errorMessage);
                                Assert(bDateTimeValid, errorMessage, "Validate UTCDateTime");
                            }
                            else
                            {
                                WriteStep("Validate UTCDateTime", "UTCDateTime is NULL");
                            }

                         },
                         () =>
                         {
                             // set current time
                             SynchronizeTime();
                         });

        }
    
        void SynchronizeTime(SystemDateTime initialSettings = null)
        {
            var dateTime = new Proxies.Onvif.DateTime { Date = new Date(), Time = new Time() };

            System.DateTime nowUtc = System.DateTime.UtcNow;

            dateTime.Time.Hour = nowUtc.Hour;
            dateTime.Time.Minute = nowUtc.Minute;
            dateTime.Time.Second = nowUtc.Second;

            dateTime.Date.Day = nowUtc.Day;
            dateTime.Date.Month = nowUtc.Month;
            dateTime.Date.Year = nowUtc.Year;

            //If initial settings were not retrieved then use DaylightSavings = false and DateTimeType = SetDateTimeType.Manual
            if (null == initialSettings)
            {
                initialSettings = new SystemDateTime()
                                  {
                                      DaylightSavings = false,
                                      DateTimeType = SetDateTimeType.Manual
                                  };
            }

            initialSettings.LocalDateTime = null;
            initialSettings.UTCDateTime = dateTime;

            SetSystemDateAndTime(initialSettings, "Synchronize time");
        }

        [Test(Name = "SYSTEM COMMAND DEVICE INFORMATION",
            Path = PATH,
            Order = "03.01.09",
            Id = "3-1-9",
            Category = Category.DEVICE,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDeviceInformation },
            RequirementLevel = RequirementLevel.Must)]
        public void GetDeviceInformationTest()
        {
            RunTest( () =>
                         {
                            string model;
                            string firmareVersion;
                            string serial;
                            string hardwareId;
                            string manufacturer = GetDeviceInformation(out model, out firmareVersion, out serial, out hardwareId);

                            Assert(manufacturer != null, 
                                "Manufacturer information not received", 
                                "Check Manufacturer information",
                                manufacturer != null ? string.Format("Manufacturer: {0}", manufacturer) : string.Empty);
                            Assert(model != null, 
                                "Model information not received", 
                                "Check Model information", 
                                model != null ? string.Format("Model: {0}", model) : string.Empty);
                            Assert(firmareVersion != null, 
                                "FirmwareVersion information not received", 
                                "Check FirmwareVersion information", 
                                firmareVersion != null ? string.Format("FirmwareVersion: {0}", firmareVersion) : string.Empty);
                            Assert(serial != null, 
                                "SerialNumber information not received", 
                                "Check SerialNumber information", 
                                serial != null ? string.Format("SerialNumber: {0}", serial) : string.Empty);
                            Assert(model != null, 
                                "HardwareId information not received", 
                                "Check HardwareId information", 
                                hardwareId != null ? string.Format("HardwareId: {0}", hardwareId) : string.Empty);
                             
                         });

        }
        
    }
}
