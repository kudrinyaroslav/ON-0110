///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Web;
using System.Xml;
using TestTool.HttpTransport.Internals.Http;
using TestTool.Proxies.WSDiscovery;
using TestTool.Tests.Common.Discovery;
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

        [Test(Name = "GET SYSTEM URIS",
            Path = PATH,
            Order = "03.01.13",
            Id = "3-1-13",
            Category = Category.DEVICE,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSystemURIs },
            RequiredFeatures = new Feature[] { Feature.HttpSystemBackupOrHttpSystemLoggingOrHttpSupportInformation },
            LastChangedIn = "v14.12",
            RequirementLevel = RequirementLevel.Optional)]
        public void GetSystemUrisTest()
        {
            RunTest(() =>
                    {
                        DeviceServiceCapabilities serviceCapabilities = null;
                        DeviceCapabilities deviceManagementCapabilities = null;

                        if (Features.ContainsFeature(Feature.GetServices))
                            serviceCapabilities = GetServiceCapabilities();
                        else
                            deviceManagementCapabilities = GetCapabilities(null).Device;
                        
                        Assert(null != serviceCapabilities || null != deviceManagementCapabilities,
                               "The DUT didn't returned device capabilities",
                               "Check capabilities is returned");

                        string supportInfoUri;
                        string systemBackupUri;
                        GetSystemUrisResponseExtension extension;
                        var systemLogURIs = GetSystemURIs(out supportInfoUri, out systemBackupUri, out extension);

                        if (null != serviceCapabilities && null != serviceCapabilities.System && serviceCapabilities.System.HttpSystemLoggingSpecified && serviceCapabilities.System.HttpSystemLogging
                         || null != deviceManagementCapabilities && null != deviceManagementCapabilities.System && null != deviceManagementCapabilities.System.Extension && deviceManagementCapabilities.System.Extension.HttpSystemLoggingSpecified && deviceManagementCapabilities.System.Extension.HttpSystemLogging)
                        {
                            Assert(systemLogURIs.Any(uri => uri.Uri.Any()),
                                   "There are no valid System Log URIs",
                                   "Check there are non-empty System Log URIs");

                            foreach (var systemLogUri in systemLogURIs)
                            {
                                HttpWebResponse httpResponse = null;

                                RunStep(() =>
                                        {
                                            var sender = new DigestAuthFixer(_username, _password);
                                            httpResponse = sender.GrabResponse(systemLogUri.Uri);
                                            httpResponse.GetResponseStream().Close();
                                        }, 
                                        string.Format("Invoke HTTP GET request on URI '{0}'", systemLogUri.Uri));

                                Assert(httpResponse.StatusCode == HttpStatusCode.OK, 
                                       "HTTP Status is not '200 OK'", 
                                       "Check HTTP status code", 
                                       string.Format("HTTP Status: {0} {1}", ((int)httpResponse.StatusCode), httpResponse.StatusDescription));

                                Assert(0 != httpResponse.ContentLength, 
                                       "The DUT returned empty System Log", 
                                       "Check System Log content is returned");
                            }
                        }

                        if (null != serviceCapabilities && null != serviceCapabilities.System && serviceCapabilities.System.HttpSupportInformationSpecified && serviceCapabilities.System.HttpSupportInformation
                         || null != deviceManagementCapabilities && null != deviceManagementCapabilities.System && null != deviceManagementCapabilities.System.Extension && deviceManagementCapabilities.System.Extension.HttpSupportInformationSpecified && deviceManagementCapabilities.System.Extension.HttpSupportInformation)
                        {
                            Assert(supportInfoUri.Any(),
                                   "Support Info URI is empty",
                                   "Check Support Info URI isn't empty");

                            HttpWebResponse httpResponse = null;

                            RunStep(() =>
                                    {
                                        var sender = new DigestAuthFixer(_username, _password);
                                        httpResponse = sender.GrabResponse(supportInfoUri);
                                        httpResponse.GetResponseStream().Close();
                                    },
                                    string.Format("Invoke HTTP GET request on URI '{0}'", supportInfoUri));

                            Assert(httpResponse.StatusCode == HttpStatusCode.OK,
                                   "HTTP Status is not '200 OK'",
                                   "Check HTTP status code",
                                   string.Format("HTTP Status: {0} {1}", ((int)httpResponse.StatusCode), httpResponse.StatusDescription));

                            Assert(0 != httpResponse.ContentLength,
                                   "The DUT returned empty Support Info",
                                   "Check Support Info content is returned");
                        }

                        if (null != serviceCapabilities && null != serviceCapabilities.System && serviceCapabilities.System.HttpSystemBackupSpecified && serviceCapabilities.System.HttpSystemBackup
                         || null != deviceManagementCapabilities && null != deviceManagementCapabilities.System && null != deviceManagementCapabilities.System.Extension && deviceManagementCapabilities.System.Extension.HttpSystemBackupSpecified && deviceManagementCapabilities.System.Extension.HttpSystemBackup)
                        {
                            Assert(systemBackupUri.Any(),
                                   "System Backup URI is empty",
                                   "Check System Backup URI isn't empty");

                            HttpWebResponse httpResponse = null;

                            RunStep(() =>
                                    {
                                        var sender = new DigestAuthFixer(_username, _password);
                                        httpResponse = sender.GrabResponse(systemBackupUri);
                                        httpResponse.GetResponseStream().Close();
                                    },
                                    string.Format("Invoke HTTP GET request on URI '{0}'", systemBackupUri));

                            Assert(httpResponse.StatusCode == HttpStatusCode.OK,
                                   "HTTP Status is not '200 OK'",
                                   "Check HTTP status code",
                                   string.Format("HTTP Status: {0} {1}", ((int)httpResponse.StatusCode), httpResponse.StatusDescription));

                            Assert(0 != httpResponse.ContentLength,
                                   "The DUT returned empty System Backup",
                                   "Check System Backup content is returned");
                        }
                    });
        }

        [Test(Name = "START SYSTEM RESTORE",
            Path = PATH,
            Order = "03.01.14",
            Id = "3-1-14",
            Category = Category.DEVICE,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.StartSystemRestore },
            RequiredFeatures = new Feature[] { Feature.HttpSystemBackup },
            LastChangedIn = "v14.12",
            RequirementLevel = RequirementLevel.Optional)]
        public void StartSystemRestoreTest()
        {
            RunTest(() =>
                    {
                        string supportInfoUri;
                        string systemBackupUri;
                        GetSystemUrisResponseExtension extension;
                        GetSystemURIs(out supportInfoUri, out systemBackupUri, out extension);

                        Assert(systemBackupUri.Any(),
                               "System Backup URI is empty",
                               "Check System Backup URI isn't empty");

                        HttpWebResponse httpResponse = null;
                        var firmwareupgrade = new MemoryStream();

                        RunStep(() =>
                                {
                                    var sender = new DigestAuthFixer(_username, _password);
                                    httpResponse = sender.GrabResponse(systemBackupUri);
                                    httpResponse.GetResponseStream().CopyTo(firmwareupgrade);
                                },
                                string.Format("Invoke HTTP GET request on URI '{0}'", systemBackupUri));

                        Assert(httpResponse.StatusCode == HttpStatusCode.OK,
                               "HTTP Status is not '200 OK'",
                               "Check HTTP status code",
                               string.Format("HTTP Status: {0} {1}", ((int)httpResponse.StatusCode), httpResponse.StatusDescription));

                        Assert(0 != httpResponse.ContentLength,
                               "The DUT returned empty System Backup",
                               "Check System Backup content is returned");

                        string expectedDownTime = null;
                        var uploadURI = StartSystemRestore(out expectedDownTime);

                        try
                        {
                            RunStep(() =>
                                    {
                                        var sender = new DigestAuthFixer(_username, _password);
                                        httpResponse = sender.GrabResponse(uploadURI, "application/octet-stream", firmwareupgrade.ToArray());
                                    },
                                    string.Format("Invoke HTTP POST request on URI '{0}'", uploadURI));
                        }
                        catch (WebException e)
                        {
                            if (null == (e.Response as HttpWebResponse))
                            {
                                StepFailed(e);
                                return;
                            }

                            StepPassed();

                            httpResponse = (HttpWebResponse) e.Response;
                        }
                        catch (Exception e)
                        {
                            StepFailed(e);
                            return;
                        }

                        Assert(httpResponse.StatusCode == HttpStatusCode.OK,
                               "HTTP Status is not '200 OK'",
                               "Check HTTP status code",
                               string.Format("HTTP Status: {0} {1}", ((int)httpResponse.StatusCode), httpResponse.StatusDescription));

                        var downTime = XmlConvert.ToTimeSpan(expectedDownTime);
                        RunStep(() => Sleep((int)downTime.TotalMilliseconds),
                                string.Format("{0} seconds timeout after StartSystemRestore", (int)downTime.TotalSeconds));


                        discoverDevice();
                    });
        }

        [Test(Name = "START SYSTEM RESTORE – INVALID BACKUP FILE",
            Path = PATH,
            Order = "03.01.15",
            Id = "3-1-15",
            Category = Category.DEVICE,
            Version = 1.02,
            FunctionalityUnderTest = new Functionality[] { Functionality.StartSystemRestore },
            RequiredFeatures = new Feature[] { Feature.HttpSystemBackup },
            LastChangedIn = "v14.12",
            RequirementLevel = RequirementLevel.Optional)]
        public void StartSystemRestoreInvalidBackupFileTest()
        {
            RunTest(() =>
                    {
                        HttpWebResponse httpResponse = null;

                        string expectedDownTime = null;
                        var uploadURI = StartSystemRestore(out expectedDownTime);

                        try
                        {
                            RunStep(() =>
                                    {
                                        var sender = new DigestAuthFixer(_username, _password);
                                        httpResponse = sender.GrabResponse(uploadURI, "application/octet-stream", new byte[]{ 1, 2, 3 });
                                    },
                                    string.Format("Invoke HTTP POST request on URI '{0}'", uploadURI));
                        }
                        catch (WebException e)
                        {
                            if (null == (e.Response as HttpWebResponse))
                            {
                                StepFailed(e);
                                return;
                            }

                            StepPassed();

                            httpResponse = (HttpWebResponse) e.Response;
                        }
                        catch (Exception e)
                        {
                            StepFailed(e);
                            return;
                        }

                        Assert(httpResponse.StatusCode == HttpStatusCode.UnsupportedMediaType,
                               "HTTP Status is not '415 Unsupported Media Type'",
                               "Check HTTP status code",
                               string.Format("HTTP Status: {0} {1}", ((int)httpResponse.StatusCode), httpResponse.StatusDescription));
                    });
        }

        protected ProbeMatchesType sendUnicastProbeRequest()
        {
            ProbeMatchesType r = null;

            var discoveryFinished = new EventWaitHandle(false, EventResetMode.AutoReset);
            var helloReceivedFlag = false;
            var discovery = new Discovery(_nic.IP);
            discovery.MessageSent += (sender, args) => LogRequest(args.Message);
            discovery.Discovered += (sender, args) =>
                                    {
                                        LogResponse(string.Join("", args.Message.Raw.Select(e => Convert.ToChar(e).ToString()).ToArray()));

                                        var response = args.Message.ToSoapMessage<ProbeMatchesType>();
                                        if (null != response)
                                        {
                                            r = response.Object;
                                            helloReceivedFlag = true;
                                        }
                                    };
            discovery.DiscoveryFinished += (sender, args) => discoveryFinished.Set();

            try
            {
                BeginStep("Sending Unicast Probe request");
                var address = System.Net.IPAddress.IsLoopback(_cameraIp) ? _nic.IP : _cameraIp;
                discovery.Probe(address, null, null);

                WaitForResponse(new WaitHandle[] { discoveryFinished });
            }
            catch (FaultException e)
            {
                LogFault(e);
            }
            finally
            {
                StepPassed();
            }

            return r;
        }

        protected ProbeMatchesType discoverDevice()
        {
            ProbeMatchesType r = null;
            var T = _rebootTimeout;

            var start = System.DateTime.Now;
            var end = start.AddMilliseconds(T);

            while (System.DateTime.Now <= end && null == r)
            {
                r = sendUnicastProbeRequest();
            }

            Assert(null != r,
                   "The DUT did not send PROBE MATCH message within the timeout.",
                   "Checking PROBE MATCH is received");

            return r;
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
