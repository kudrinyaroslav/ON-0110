///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.ServiceModel;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Onvif;
using System.Text.RegularExpressions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class DeviceManagementSystemTestSuite : Base.DeviceManagementTest
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
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.DEVICE,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must)]
        public void SetSystemDateTimeTest()
        {
            RunTest( () =>
            {
                SystemDateTime testDateTimeValue = new SystemDateTime();
                TestTool.Proxies.Onvif.DateTime dateTime = new TestTool.Proxies.Onvif.DateTime();

                dateTime.Date = new Date();
                dateTime.Time = new Time();

                System.DateTime nowUtc = System.DateTime.Now.ToUniversalTime();

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

            });

        }

        [Test(Name = "SYSTEM COMMAND SETSYSTEMDATEANDTIME USING NTP",
            Path = PATH,
            Order = "03.01.03",
            Id = "3-1-3",
            Category = Category.DEVICE,
            Version = 1.02,
            RequiredFeatures = new Feature[] {Feature.NTP},
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void SetSystemDateTimeNTPTest()
        {
            RunTest(
                () =>
                {

                    NTPInformation ntp = new NTPInformation();

                    ntp.FromDHCP = false;
                    ntp.NTPManual = new NetworkHost[] { new NetworkHost() };
                    ntp.NTPManual[0].Type = NetworkHostType.IPv4;
                    ntp.NTPManual[0].IPv4Address = _environmentSettings.NtpIpv4;

                    SetNTP(ntp);

                    SystemDateTime testDateTimeSettins = new SystemDateTime();
                    testDateTimeSettins.DateTimeType = SetDateTimeType.NTP;
                    testDateTimeSettins.DaylightSavings = true;
                    testDateTimeSettins.TimeZone = new TestTool.Proxies.Onvif.TimeZone();
                    testDateTimeSettins.TimeZone.TZ = "PST8PDT,M3.2.0,M11.1.0";
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
                        // set current time instead of restoring NTP
                        SetCurrentTime();
                    });

        }

        [Test(Name = "SYSTEM COMMAND SETSYSTEMDATEANDTIME TEST FOR INVALID TIMEZONE",
            Path = PATH,
            Order = "03.01.04",
            Id = "3-1-4",
            Category = Category.DEVICE,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Should)]
        public void SetSystemDateTimeInvalidTimezoneTest()
        {
            RunTest( () =>
                         {
                            SystemDateTime testDateTimeSettins = new SystemDateTime();
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

                            testDateTimeSettins.DateTimeType = SetDateTimeType.Manual;
                            testDateTimeSettins.DaylightSavings = true;
                            testDateTimeSettins.UTCDateTime = dateTime;
                            testDateTimeSettins.TimeZone = timeZone;

                            bool fault = false;
                            string reason = "The DUT did not return SOAP FAULT";
                            try
                            {
                                SetSystemDateAndTime(testDateTimeSettins);
                            }
                            catch (FaultException exc)
                            {
                                string faultDump;
                                fault = exc.IsValidOnvifFault("Sender/InvalidArgVal/InvalidTimeZone", out faultDump);
                                if (!fault)
                                {
                                    reason = string.Format("The SOAP FAULT returned from the DUT is invalid: {0}", faultDump);
                                }
                                SaveStepFault(exc);
                                StepPassed();
                            }

                            Assert(fault, reason, "Verify that correct SOAP FAULT is returned");

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
                                 SetCurrentTime();
                             });

        }

        [Test(Name = "SYSTEM COMMAND SETSYSTEMDATEANDTIME TEST FOR INVALID DATE",
            Path = PATH,
            Order = "03.01.05",
            Id = "3-1-5",
            Category = Category.DEVICE,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Should)]
        public void SetSystemDateTimeInvalidDateTimeTest()
        {
            RunTest( 
                () =>
                         {
                            SystemDateTime testDateTimeSettins = new SystemDateTime();
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

                            testDateTimeSettins.DateTimeType = SetDateTimeType.Manual;
                            testDateTimeSettins.DaylightSavings = false;
                            testDateTimeSettins.TimeZone = timeZone;
                            testDateTimeSettins.UTCDateTime = dateTime;

                            bool fault = false;
                            string reason = "The DUT did not return SOAP FAULT";
                            try
                            {
                                SetSystemDateAndTime(testDateTimeSettins);
                            }
                            catch (FaultException exc)
                            {
                                string faultDump;
                                fault = exc.IsValidOnvifFault("Sender/InvalidArgVal/InvalidDateTime", out faultDump);
                                if (!fault)
                                {
                                    reason = string.Format("The SOAP FAULT returned from the DUT is invalid: {0}", faultDump);
                                }
                                SaveStepFault(exc);
                                StepPassed();
                            }

                            Assert(fault, reason, "Verify that correct SOAP FAULT is returned");

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
                                 SetCurrentTime();
                             });

        }
    
        void SetCurrentTime()
        {
            SystemDateTime testDateTimeValue = new SystemDateTime();
            Proxies.Onvif.DateTime dateTime = new Proxies.Onvif.DateTime();

            dateTime.Date = new Date();
            dateTime.Time = new Time();

            System.DateTime nowUtc = System.DateTime.Now.ToUniversalTime();

            dateTime.Time.Hour = nowUtc.Hour;
            dateTime.Time.Minute = nowUtc.Minute;
            dateTime.Time.Second = nowUtc.Second;

            dateTime.Date.Day = nowUtc.Day;
            dateTime.Date.Month = nowUtc.Month;
            dateTime.Date.Year = nowUtc.Year;

            testDateTimeValue.DateTimeType = SetDateTimeType.Manual;
            testDateTimeValue.DaylightSavings = false;
            testDateTimeValue.UTCDateTime = dateTime;

            SetSystemDateAndTime(testDateTimeValue, "Synchronize time");
        }

        [Test(Name = "SYSTEM COMMAND DEVICE INFORMATION",
            Path = PATH,
            Order = "03.01.09",
            Id = "3-1-9",
            Category = Category.DEVICE,
            Version = 1.02,
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
        

        void ValidateTimeZone(string expected, string actual)
        {
            BeginStep("Validate TimeZone");

            string reason;

            TimeZoneDescription expectedDescription = new TimeZoneDescription();
            ParseTimeZone(expected, expectedDescription, out reason);

            TimeZoneDescription actualDescription = new TimeZoneDescription();
            bool bParseOk = ParseTimeZone(actual, actualDescription, out reason);
            
            if (!bParseOk)
            {
                throw new AssertException(string.Format("Time zone format is not correct: {0}", reason));
            }

            // STD
            bool bAllEquals = (string.Compare(expectedDescription.Std, actualDescription.Std, true) == 0);

            LogStepEvent(string.Format("Standart time: expected - {0}, actual - {1}", expectedDescription.Std, actualDescription.Std));

            // STD OFFSET
            bool bEquals = (expectedDescription.StdOffset.Equals(actualDescription.StdOffset));

            LogStepEvent(string.Format("Standart time offset: expected - {0}, actual - {1}",
                expectedDescription.StdOffset.OriginalString,
                actualDescription.StdOffset.OriginalString));

            bAllEquals = bAllEquals && bEquals;

            // DST
            LogStepEvent(string.Format("DST time: expected - {0}, actual - {1}", expectedDescription.Dst, actualDescription.Dst));

            bEquals = (string.Compare(expectedDescription.Dst, actualDescription.Dst, true) == 0);

            bAllEquals = bAllEquals && bEquals;

            // DST OFFSET

            LogStepEvent(string.Format("DST time offset: expected - {0}, actual - {1}",
                expectedDescription.DstOffset.OriginalString,
                actualDescription.DstOffset.OriginalString));

            bEquals = (expectedDescription.DstOffset.Equals(actualDescription.DstOffset));

            bAllEquals = bAllEquals && bEquals;

            // START

            LogStepEvent(string.Format("Start: expected - {0}, actual - {1}", expectedDescription.Start, actualDescription.Start));

            bEquals = TimeZoneDaysEqual(expectedDescription.Start, actualDescription.Start);

            bAllEquals = bAllEquals && bEquals;

            // START TIME

            LogStepEvent(string.Format("Start time: expected - {0}, actual - {1}",
                expectedDescription.StartTime.OriginalString,
                actualDescription.StartTime.OriginalString));

            bEquals = (expectedDescription.StartTime.Equals(actualDescription.StartTime));

            bAllEquals = bAllEquals && bEquals;


            // END

            LogStepEvent(string.Format("End: expected - {0}, actual - {1}", expectedDescription.End, actualDescription.End));

            bEquals = TimeZoneDaysEqual(expectedDescription.End, actualDescription.End);

            bAllEquals = bAllEquals && bEquals;

            // END TIME

            LogStepEvent(string.Format("End time: expected - {0}, actual - {1}",
                expectedDescription.EndTime.OriginalString,
                actualDescription.EndTime.OriginalString));

            bEquals = (expectedDescription.EndTime.Equals(actualDescription.EndTime));

            bAllEquals = bAllEquals && bEquals;

            if (!bAllEquals)
            {
                throw new AssertException("Time zone differs from one was set");
            }

            StepPassed();
        }

        class ShortTimeSpan
        {
            public int Hour;
            public int Minute;
            public int Second;
            public bool Positive;

            public string OriginalString;

            public static ShortTimeSpan FromString(string s)
            {
                string st = s;
                if (s.StartsWith("+") || s.StartsWith("-"))
                {
                    st = s.Substring(1);
                }
                
                string[] parts = st.Split(':');

                ShortTimeSpan timeSpan = new ShortTimeSpan();

                timeSpan.OriginalString = s;

                timeSpan.Hour = int.Parse(parts[0]);
                if (parts.Length > 1)
                {
                    timeSpan.Minute = int.Parse(parts[1]);
                }

                if (parts.Length > 2)
                {
                    timeSpan.Second = int.Parse(parts[2]);
                }

                if (s.StartsWith("-"))
                {
                    timeSpan.Positive = false;
                }

                return timeSpan;
            }

            public ShortTimeSpan()
            {
                Positive = true;
            }

            public ShortTimeSpan(int hour, int minute, int second)
                : this()
            {
                Hour = hour;
                Minute = minute;
                Second = second;
            }

            public bool Equals(ShortTimeSpan other)
            {
                return other.Hour == this.Hour && 
                    other.Minute == this.Minute && 
                    other.Second == this.Second && 
                    other.Positive == this.Positive;
            }

            public bool IsValid()
            {
                return Hour >= 0 && Hour < 24 && Minute >= 0 && Minute < 60 && Second >= 0 && Second < 60;
            }

        }

        class TimeZoneDescription
        {
            public string Std;
            public ShortTimeSpan StdOffset;
            public string Dst;
            public ShortTimeSpan DstOffset;
            public string Start;
            public ShortTimeSpan StartTime;
            public string End;
            public ShortTimeSpan EndTime;
        }

        bool IsValidTimeZoneDay(string day)
        {
            if (day.StartsWith("J"))
            {
                int d;
                return int.TryParse(day.Substring(1), out d);
            }
            else if (day.StartsWith("M"))
            {
                Regex regex = new Regex(@"^\d{1,2}.\d{1}.\d{1}$");
                string numPart = day.Substring(1);
                if (!regex.IsMatch(numPart))
                {
                    return false;
                }
                else
                {
                    string[] parts = numPart.Split('.');
                    int d = int.Parse(parts[0]);
                    if (d > 12)
                    {
                        return false;
                    }
                    d = int.Parse(parts[1]);
                    if (d > 5)
                    {
                        return false;
                    }
                    d = int.Parse(parts[2]);
                    if (d > 6)
                    {
                        return false;
                    }
                    return true;
                }
            }
            else
            {
                int d;
                return int.TryParse(day, out d);
            }
        }

        bool TimeZoneDaysEqual(string day1, string day2)
        {
            if (string.IsNullOrEmpty(day1) && string.IsNullOrEmpty(day2))
            {
                return true;
            }
            
            // it is true only when one value is null and second is neither null nor empty.
            if (day1 == null || day2 == null)
            {
                return false;
            }

            if (day1.StartsWith("J") && day2.StartsWith("J"))
            {
                int d1 = int.Parse(day1.Substring(1));
                int d2 = int.Parse(day2.Substring(1));
                return (d1 == d2);
            }
            else if (day1.StartsWith("M") && day2.StartsWith("M"))
            {
                string numPart1 = day1.Substring(1);
                string[] parts1 = numPart1.Split('.');

                string numPart2 = day2.Substring(1);
                string[] parts2 = numPart2.Split('.');

                return (int.Parse(parts1[0]) == int.Parse(parts2[0])) &&
                    (int.Parse(parts1[1]) == int.Parse(parts2[1])) &&
                    (int.Parse(parts1[2]) == int.Parse(parts2[2]));
            }
            else if (Char.IsDigit(day1[0]) && Char.IsDigit(day2[0]))
            {
                return (int.Parse(day1) == int.Parse(day2));
            }
            return false;
        }
        bool ParseTimeZone(string timeZone, TimeZoneDescription tz, out string reason)
        {
            if (string.IsNullOrEmpty(timeZone))
            {
                reason = "TimeZone string is null or empty";
                return false;
            }

            if (timeZone.EndsWith(","))
            {
                reason = "No value after comma";
                return false;
            }

            if (timeZone.EndsWith("/"))
            {
                reason = "No value after date and time separator (/)";
                return false;
            }

            int i = 0;
            reason = string.Empty;

            // find Std
            while (i < timeZone.Length)
            {
                char nextChar = timeZone[i];
                if (nextChar == '+' || nextChar == '-' || Char.IsDigit(nextChar))
                {
                    break;
                }
                i++;
            }
            tz.Std = timeZone.Substring(0, i);

            // std offset
            int nextPartStart = i;
            while (i < timeZone.Length)
            {
                char nextChar = timeZone[i];
                if (nextChar != ':' && nextChar != '+' && nextChar != '-' && !Char.IsDigit(nextChar))
                {
                    break;
                }
                i++;
            }

            string stdOffsetString = timeZone.Substring(nextPartStart, i - nextPartStart);

            Regex regex = new Regex(@"^[-\+]{0,1}\d{1,2}(:\d{1,2}){0,2}$");
            if (!regex.IsMatch(stdOffsetString))
            {
                reason = string.Format("Standart offset part format is incorrect ({0})", stdOffsetString);
                return false;
            }

            tz.StdOffset = ShortTimeSpan.FromString(stdOffsetString);

            if (!tz.StdOffset.IsValid())
            {
                reason = string.Format("Standart offset is incorrect ({0})", stdOffsetString);
                return false;
            }

            nextPartStart = i;
            bool noDstOffset = (i == timeZone.Length);

            // Dst
            while (i < timeZone.Length)
            {
                char nextChar = timeZone[i];
                if (nextChar == '+' || nextChar == '-' || Char.IsDigit(nextChar))
                {
                    break;
                }
                if (nextChar == ',')
                {
                    // comma means "no offset"
                    noDstOffset = true;
                    break;
                }
                i++;
            }

            tz.Dst = timeZone.Substring(nextPartStart, i - nextPartStart);

            if (noDstOffset)
            {
                i++;
            }

            nextPartStart = i;

            Action setDefaultDstOffset =
                () =>
                {
                    tz.DstOffset = new ShortTimeSpan(tz.StdOffset.Hour - 1,
                        tz.StdOffset.Minute,
                        tz.StdOffset.Second);

                    tz.DstOffset.OriginalString =
                        string.Format("{0}:{1}:{2}",
                        tz.DstOffset.Hour.ToString("00"),
                        tz.DstOffset.Minute.ToString("00"),
                        tz.DstOffset.Second.ToString("00"));

                };

            if (!noDstOffset)
            {
                while (i < timeZone.Length)
                {
                    char nextChar = timeZone[i];
                    if (nextChar != ':' && nextChar != '+' && nextChar != '-' && !Char.IsDigit(nextChar))
                    {
                        break;
                    }
                    i++;
                }
                string dstOffsetString = timeZone.Substring(nextPartStart, i - nextPartStart);

                if (string.IsNullOrEmpty(dstOffsetString))
                {
                    setDefaultDstOffset();

                }
                else
                {
                    if (!regex.IsMatch(dstOffsetString))
                    {
                        reason = string.Format("DST offset part format is incorrect ({0})", dstOffsetString);
                        return false;
                    }

                    tz.DstOffset = ShortTimeSpan.FromString(dstOffsetString);

                    if (!tz.DstOffset.IsValid())
                    {
                        reason = string.Format("DST offset is incorrect ({0})", dstOffsetString);
                        return false;
                    }
                }

                i++;
                nextPartStart = i;
            }
            else
            {
                setDefaultDstOffset();








            }

            bool noStartDate = (i >= timeZone.Length);
            bool noStartTime = (i >= timeZone.Length);
            // at the beginning of start time
            while (i < timeZone.Length)
            {
                char nextChar = timeZone[i];
                if (nextChar == '/' || nextChar == ',')
                {
                    noStartTime = (nextChar == ',');
                    break;
                }
                i++;
            }

            if (!noStartDate)
            {
                tz.Start = timeZone.Substring(nextPartStart, i - nextPartStart);

                if (!IsValidTimeZoneDay(tz.Start))
                {
                    reason = string.Format("Start part format is incorrect ({0})", tz.Start);
                    return false;
                }
            }

            i++;
            nextPartStart = i;

            if (!noStartTime)
            {

                while (i < timeZone.Length)
                {
                    char nextChar = timeZone[i];
                    if (nextChar == ',')
                    {
                        break;
                    }
                    i++;
                }

                string startTimeString = timeZone.Substring(nextPartStart, i - nextPartStart);

                if (string.IsNullOrEmpty(startTimeString))
                {
                    tz.StartTime = new ShortTimeSpan(2, 0, 0);
                    tz.StartTime.OriginalString = "02:00:00";
                }
                else
                {
                    if (!regex.IsMatch(startTimeString))
                    {
                        reason = string.Format("Start time part format is incorrect ({0})", startTimeString);
                        return false;
                    }

                    tz.StartTime = ShortTimeSpan.FromString(startTimeString);

                    if (!tz.StdOffset.IsValid())
                    {
                        reason = string.Format("Start time is incorrect ({0})", startTimeString);
                        return false;
                    }
                }

                i++;
                nextPartStart = i;
            }
            else
            {
                tz.StartTime = new ShortTimeSpan(2, 0, 0);
                tz.StartTime.OriginalString = "02:00:00";
            }

            bool noEndDate = (i > timeZone.Length);
            bool noEndTime = (i > timeZone.Length);
            while (i < timeZone.Length)
            {
                char nextChar = timeZone[i];
                if (nextChar == '/' || nextChar == ',')
                {
                    noEndTime = (nextChar == ',');
                    break;
                }
                i++;
            }

            if (!noEndDate)
            {
                tz.End = timeZone.Substring(nextPartStart, i - nextPartStart);
                if (!IsValidTimeZoneDay(tz.End))
                {
                    reason = string.Format("End part format is incorrect ({0})", tz.End);
                    return false;
                }

            }

            i++;
            if (i > timeZone.Length)
            {
                noEndTime = true;
            }

            nextPartStart = i;

            if (!noEndTime)
            {

                while (i < timeZone.Length)
                {
                    char nextChar = timeZone[i];
                    if (nextChar == ',')
                    {
                        break;
                    }
                    i++;
                }

                string endTimeString = timeZone.Substring(nextPartStart, i - nextPartStart);

                if (string.IsNullOrEmpty(endTimeString))
                {
                    tz.EndTime = new ShortTimeSpan(2, 0, 0);
                    tz.EndTime.OriginalString = "02:00:00";
                }
                else
                {
                    if (!regex.IsMatch(endTimeString))
                    {
                        reason = string.Format("End time part format is incorrect ({0})", endTimeString);
                        return false;
                    }

                    tz.EndTime = ShortTimeSpan.FromString(endTimeString);

                    if (!tz.StdOffset.IsValid())
                    {
                        reason = string.Format("End time is incorrect ({0})", endTimeString);
                        return false;
                    }
                }
            }
            else
            {
                tz.EndTime = new ShortTimeSpan(2, 0, 0);
                tz.EndTime.OriginalString = "02:00:00";
            }

            return true;
        }

    }
}
