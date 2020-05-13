using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;

namespace TestTool.Tests.TestCases.OnvifServices
{

    public class VEvent
    {
        private const string _CRLF = "\r\n";
        private const string _beginFieldName = "BEGIN";
        private const string _summary = "SUMMARY";
        private const string _dtStartFieldName = "DTSTART";
        private const string _dtEndFieldName = "DTEND";
        private const string _rruleFieldName = "RRULE";
        private const string _uidFieldName = "UID";
        private const string _endFieldName = "END";
        private Dictionary<string, string> _anotherFields;

        private const string _beginEndVEvent = "VEVENT";

        public string Summary { get; set; }
        public string DtStart { get; set; }
        public string DtEnd { get; set; }
        public string Rrule { get; set; }
        public string Uid { get; set; }

        public string VEventValue
        {
            get
            {
                string vEventValue = !string.IsNullOrEmpty(_beginEndVEvent) ? string.Format("{1}:{2}", _CRLF, _beginFieldName, _beginEndVEvent) : String.Empty;
                vEventValue += !string.IsNullOrEmpty(Summary) ? string.Format("{0}{1}:{2}", _CRLF, _summary, Summary) : String.Empty;
                vEventValue += !string.IsNullOrEmpty(DtStart) ? string.Format("{0}{1}:{2}", _CRLF, _dtStartFieldName, DtStart) : String.Empty;
                vEventValue += !string.IsNullOrEmpty(DtEnd) ? string.Format("{0}{1}:{2}", _CRLF, _dtEndFieldName, DtEnd) : String.Empty;
                vEventValue += !string.IsNullOrEmpty(Rrule) ? string.Format("{0}{1}:{2}", _CRLF, _rruleFieldName, Rrule) : String.Empty;
                vEventValue += !string.IsNullOrEmpty(Uid) ? string.Format("{0}{1}:{2}", _CRLF, _uidFieldName, Uid) : String.Empty;
                vEventValue += !string.IsNullOrEmpty(_beginEndVEvent) ? string.Format("{0}{1}:{2}", _CRLF, _endFieldName, _beginEndVEvent) : String.Empty;

                return vEventValue;
            }
        }

        public string VEventValueWithoutUid
        {
            get
            {
                string vEventValue = !string.IsNullOrEmpty(_beginEndVEvent) ? string.Format("{1}:{2}", _CRLF, _beginFieldName, _beginEndVEvent) : String.Empty;
                vEventValue += !string.IsNullOrEmpty(Summary) ? string.Format("{0}{1}:{2}", _CRLF, _summary, Summary) : String.Empty;
                vEventValue += !string.IsNullOrEmpty(DtStart) ? string.Format("{0}{1}:{2}", _CRLF, _dtStartFieldName, DtStart) : String.Empty;
                vEventValue += !string.IsNullOrEmpty(DtEnd) ? string.Format("{0}{1}:{2}", _CRLF, _dtEndFieldName, DtEnd) : String.Empty;
                vEventValue += !string.IsNullOrEmpty(Rrule) ? string.Format("{0}{1}:{2}", _CRLF, _rruleFieldName, Rrule) : String.Empty;
                vEventValue += !string.IsNullOrEmpty(_beginEndVEvent) ? string.Format("{0}{1}:{2}", _CRLF, _endFieldName, _beginEndVEvent) : String.Empty;

                return vEventValue;
            }
        }

        public VEvent()
        {
            _anotherFields = new Dictionary<string, string>();
        }

        public VEvent(string veventString)
        {
            _anotherFields = new Dictionary<string, string>();
            var parseVEvent = veventString.Split(new string[] { _CRLF }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var vEventFields = parseVEvent != null && parseVEvent.Count > 0 ? parseVEvent : null;
            var indexOfBeginVevent = vEventFields.IndexOf(string.Format("{0}:{1}", _beginFieldName, _beginEndVEvent));
            if (indexOfBeginVevent > 0)
            {
                vEventFields.RemoveRange(0, indexOfBeginVevent);
            }

            var indexOfEndVevent = vEventFields.IndexOf(string.Format("{0}:{1}", _endFieldName, _beginEndVEvent));
            if (indexOfEndVevent > 0)
            {
                vEventFields.RemoveRange(indexOfEndVevent, vEventFields.Count - indexOfEndVevent);
            }

            foreach (var field in vEventFields)
            {
                var parseField = field.Split(new char[] { ':' });
                if (parseField.Length == 2)
                {
                    var fieldName = parseField[0];
                    var fieldValue = parseField[1];
                    if (fieldName == _summary)
                    {
                        Summary = fieldValue;
                        continue;
                    }
                    if (fieldName == _dtStartFieldName)
                    {
                        DtStart = fieldValue;
                        continue;
                    }
                    if (fieldName == _dtEndFieldName)
                    {
                        DtEnd = fieldValue;
                        continue;
                    }
                    if (fieldName == _rruleFieldName)
                    {
                        Rrule = fieldValue;
                        continue;
                    }
                    if (fieldName == _uidFieldName)
                    {
                        Uid = fieldValue;
                        continue;
                    }
                    _anotherFields[fieldName] = fieldValue;
                }
            }
        }

        public void SetDates(System.DateTime startDate, System.DateTime endDate)
        {
            DtStart = startDate.ToString("yyyyMMddTHHmmss");
            DtEnd = endDate.ToString("yyyyMMddTHHmmss");
        }

        public Dictionary<string, string> GetAnotherFields()
        {
            return _anotherFields;
        }

    }

    public class ICalendar
    {
        private const string _CRLF = "\r\n";
        private const string _beginFieldName = "BEGIN";
        private const string _endFieldName = "END";
        private Dictionary<string, string> _anotherFields;
        private List<VEvent> _vEvents;

        private const string _iCalendarBeginEndVEventFieldValue = "VEVENT";
        private const string _iCalendarBeginEndFieldValue = "VCALENDAR";
        private const string _iCalendarVersionFieldName = "VERSION";
        private const string _iCalendarVersionFieldValue = "2.0";
        private const string _iCalendarProdidFieldName = "PRODID";
        private const string _iCalendarProdidFieldValue = "-//hacksw/handcal//NONSGML v1.0//EN";


        public string ICalendarValue
        {
            get
            {
                string iCalendarValue = !string.IsNullOrEmpty(_iCalendarBeginEndFieldValue) ? string.Format("{0}:{1}", _beginFieldName, _iCalendarBeginEndFieldValue) : String.Empty;
                iCalendarValue += !string.IsNullOrEmpty(_iCalendarVersionFieldValue) ? string.Format("{0}{1}:{2}", _CRLF, _iCalendarVersionFieldName, _iCalendarVersionFieldValue) : String.Empty;
                iCalendarValue += !string.IsNullOrEmpty(_iCalendarProdidFieldValue) ? string.Format("{0}{1}:{2}", _CRLF, _iCalendarProdidFieldName, _iCalendarProdidFieldValue) : String.Empty;

                foreach (var vEvent in _vEvents)
                {
                    iCalendarValue += string.Format("{0}{1}", _CRLF, vEvent.VEventValue);
                }
                iCalendarValue += !string.IsNullOrEmpty(_iCalendarBeginEndFieldValue) ? string.Format("{0}{1}:{2}", _CRLF, _endFieldName, _iCalendarBeginEndFieldValue) : String.Empty;

                return iCalendarValue;
            }
        }

        public string ICalendarValueWithoutUid
        {
            get
            {
                string iCalendarValue = !string.IsNullOrEmpty(_iCalendarBeginEndFieldValue) ? string.Format("{0}:{1}", _beginFieldName, _iCalendarBeginEndFieldValue) : String.Empty;
                iCalendarValue += !string.IsNullOrEmpty(_iCalendarVersionFieldValue) ? string.Format("{0}{1}:{2}", _CRLF, _iCalendarVersionFieldName, _iCalendarVersionFieldValue) : String.Empty;
                iCalendarValue += !string.IsNullOrEmpty(_iCalendarProdidFieldValue) ? string.Format("{0}{1}:{2}", _CRLF, _iCalendarProdidFieldName, _iCalendarProdidFieldValue) : String.Empty;

                foreach (var vEvent in _vEvents)
                {
                    iCalendarValue += string.Format("{0}{1}", _CRLF, vEvent.VEventValueWithoutUid);
                }
                iCalendarValue += !string.IsNullOrEmpty(_iCalendarBeginEndFieldValue) ? string.Format("{0}{1}:{2}", _CRLF, _endFieldName, _iCalendarBeginEndFieldValue) : String.Empty;

                return iCalendarValue;
            }
        }

        public ICalendar()
        {
            _vEvents = new List<VEvent>();
            _anotherFields = new Dictionary<string, string>();
        }

        public ICalendar(string iCalendar)
        {
            _vEvents = new List<VEvent>();
            _anotherFields = new Dictionary<string, string>();
            var parseiCalendar = iCalendar.Split(new string[] { _CRLF }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (parseiCalendar == null || parseiCalendar.Count == 0)
            {
                return;
            }
            var indexOfBeginICalendar = parseiCalendar.IndexOf(string.Format("{0}:{1}", _beginFieldName, _iCalendarBeginEndFieldValue));
            if (indexOfBeginICalendar < 0)
            {
                return;
            }
            parseiCalendar.RemoveRange(0, indexOfBeginICalendar);

            var indexOfEndICalendar = parseiCalendar.IndexOf(string.Format("{0}:{1}", _endFieldName, _iCalendarBeginEndFieldValue));
            if (indexOfEndICalendar < 0)
            {
                return;
            }
            parseiCalendar.RemoveRange(indexOfEndICalendar, parseiCalendar.Count - indexOfEndICalendar);

            var indexOfBeginVEvent = parseiCalendar.IndexOf(string.Format("{0}:{1}", _beginFieldName, _iCalendarBeginEndVEventFieldValue));
            if (indexOfBeginVEvent < 0)
            {
                return;
            }
            parseiCalendar.RemoveRange(0, indexOfBeginVEvent);

            var indexOfEndVEvent = parseiCalendar.IndexOf(string.Format("{0}:{1}", _endFieldName, _iCalendarBeginEndVEventFieldValue));
            if (indexOfEndVEvent < 0)
            {
                return;
            }
            parseiCalendar.RemoveRange(indexOfEndVEvent, parseiCalendar.Count - indexOfEndVEvent);

            var iCalendarVEventsFormatted = string.Join(_CRLF, parseiCalendar.ToArray());
            var parseICalendarVevents = iCalendarVEventsFormatted.Split(new string[] { string.Format("{0}:{1}", _beginFieldName, _iCalendarBeginEndVEventFieldValue) }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var vEventString in parseICalendarVevents)
            {
                AddVEvent(vEventString.Replace(string.Format("{0}:{1}", _endFieldName, _iCalendarBeginEndVEventFieldValue).Replace(string.Format("{0}:{1}", _beginFieldName, _iCalendarBeginEndVEventFieldValue), ""), ""));
            }
        }

        public void AddVEvent(VEvent vEvent)
        {
            _vEvents.Add(vEvent);
        }

        public void AddVEvent(string vEventString)
        {
            VEvent vEvent = new VEvent(vEventString);
            _vEvents.Add(vEvent);
        }

        public List<VEvent> GetVEvents()
        {
            return _vEvents;
        }

    }

    public interface IScheduleService : IBaseOnvifService2<SchedulePort, SchedulePortClient>
    { }

    public static class ScheduleServiceExtensions
    {
        private static void InitializeGuard(this IScheduleService s)
        {
            if (null == s.ServiceClient.Port)
                s.Test.Assert(false,
                              "Can't connect to Schedule Service",
                              "Check that Schedule Service is accessible");
        }

        public static string GetScheduleServiceAddress(this IDeviceService s, FeaturesList featureList)
        {
            return s.GetServiceAddress(OnvifService.SCHEDULE_SERVICE);
        }

        public static ScheduleServiceCapabilities GetServiceCapabilities(this IScheduleService s)
        {
            s.InitializeGuard();

            ScheduleServiceCapabilities r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetServiceCapabilities(), "Get Service Capabilities(Schedule)");

            return r;
        }

        public static ScheduleState GetScheduleState(this IScheduleService s, string Token)
        {
            s.InitializeGuard();

            ScheduleState r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetScheduleState(Token), "Get ScheduleState");

            return r;
        }

        public static ScheduleInfo[] GetScheduleInfo(this IScheduleService s, string[] Token)
        {
            s.InitializeGuard();

            ScheduleInfo[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetScheduleInfo(Token), "Get ScheduleInfo");


            return r ?? new ScheduleInfo[0];
        }

        public static string GetScheduleInfoList(this IScheduleService s, int Limit, string StartReference, out ScheduleInfo[] ScheduleInfo)
        {
            s.InitializeGuard();

            string r = null;
            ScheduleInfo[] localScheduleInfo = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetScheduleInfoList(Limit, StartReference, out localScheduleInfo), "Get ScheduleInfo List");

            ScheduleInfo = localScheduleInfo ?? new ScheduleInfo[0];

            return r;
        }

        public static Schedule[] GetSchedules(this IScheduleService s, string[] Token)
        {
            s.InitializeGuard();

            Schedule[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetSchedules(Token), "Get Schedules");

            return r ?? new Schedule[0];
        }

        public static string GetScheduleList(this IScheduleService s, int? Limit, string StartReference, out Schedule[] Schedule)
        {
            s.InitializeGuard();

            string r = null;
            Schedule[] localSchedule = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetScheduleList(Limit, StartReference, out localSchedule), "Get Schedule List");

            Schedule = localSchedule ?? new Schedule[0];

            return r;
        }

        public static string CreateSchedule(this IScheduleService s, Schedule Schedule)
        {
            s.InitializeGuard();

            string r = null;
            s.Test.RunStep(() => r = s.ServiceClient.Port.CreateSchedule(Schedule), "Create Schedule");

            return r;
        }

        public static void ModifySchedule(this IScheduleService s, Schedule Schedule)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.ModifySchedule(Schedule), "Modify Schedule");
        }

        public static void DeleteSchedule(this IScheduleService s, string Token)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteSchedule(Token), "Delete Schedule");
        }

        public static SpecialDayGroupInfo[] GetSpecialDayGroupInfo(this IScheduleService s, string[] Token)
        {
            s.InitializeGuard();

            SpecialDayGroupInfo[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetSpecialDayGroupInfo(Token), "Get SpecialDayGroupInfo");

            return r ?? new SpecialDayGroupInfo[0];
        }

        public static string GetSpecialDayGroupInfoList(this IScheduleService s,
                                                        int? Limit, string StartReference, out SpecialDayGroupInfo[] SpecialDayGroupInfo)
        {
            s.InitializeGuard();

            string r = null;
            SpecialDayGroupInfo[] localSpecialDayGroupInfo = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetSpecialDayGroupInfoList(Limit, StartReference, out localSpecialDayGroupInfo), "Get SpecialDayGroupInfo List");

            SpecialDayGroupInfo = localSpecialDayGroupInfo ?? new SpecialDayGroupInfo[0];

            return r;
        }

        public static SpecialDayGroup[] GetSpecialDayGroups(this IScheduleService s, string[] Token)
        {
            s.InitializeGuard();

            SpecialDayGroup[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetSpecialDayGroups(Token), "Get SpecialDayGroups");

            return r ?? new SpecialDayGroup[0];
        }

        public static string GetSpecialDayGroupList(this IScheduleService s, int? Limit, string StartReference, out SpecialDayGroup[] SpecialDayGroup)
        {
            s.InitializeGuard();

            string r = null;
            SpecialDayGroup[] localSpecialDayGroup = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetSpecialDayGroupList(Limit, StartReference, out localSpecialDayGroup), "Get SpecialDayGroup List");

            SpecialDayGroup = localSpecialDayGroup ?? new SpecialDayGroup[0];

            return r;
        }

        public static string CreateSpecialDayGroup(this IScheduleService s, SpecialDayGroup SpecialDayGroup)
        {
            s.InitializeGuard();

            string r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.CreateSpecialDayGroup(SpecialDayGroup), "Create SpecialDayGroup");

            return r;
        }

        public static void ModifySpecialDayGroup(this IScheduleService s, SpecialDayGroup SpecialDayGroup)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.ModifySpecialDayGroup(SpecialDayGroup), "Modify SpecialDayGroup");
        }

        public static void DeleteSpecialDayGroup(this IScheduleService s, string Token)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteSpecialDayGroup(Token), "Delete SpecialDayGroup");
        }

        public static ScheduleServiceCapabilities ExtractScheduleCapabilities(this IScheduleService s, Service service)
        {
            return s.ExtractCapabilities<ScheduleServiceCapabilities, SchedulePort, SchedulePortClient>(service, "Schedule");
        }

        public static List<ScheduleInfo> GetFullScheduleInfoListA1(this IScheduleService s)
        {
            var r = new List<ScheduleInfo>();

            string nextReference = null;
            do
            {
                ScheduleInfo[] dst = null;
                nextReference = s.GetScheduleInfoList(null, nextReference, out dst);
                r.AddRange(dst);
            } while (!string.IsNullOrEmpty(nextReference) && !s.Test.StopRequested());

            return r;
        }

        public static string GetScheduleInfoList(this IScheduleService s, int? Limit, string StartReference, out ScheduleInfo[] ScheduleInfo)
        {
            s.InitializeGuard();

            string r = null;
            ScheduleInfo[] localScheduleInfo = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetScheduleInfoList(Limit, StartReference, out localScheduleInfo), "Get ScheduleInfo List");

            ScheduleInfo = localScheduleInfo ?? new ScheduleInfo[0];

            return r;
        }

        public static List<Schedule> GetFullScheduleListA3(this IScheduleService s)
        {
            var r = new List<Schedule>();

            string nextReference = null;
            do
            {
                Schedule[] dst = null;
                nextReference = s.GetScheduleList(null, nextReference, out dst);
                r.AddRange(dst);
            } while (!string.IsNullOrEmpty(nextReference) && !s.Test.StopRequested());

            return r;
        }

        public static List<SpecialDayGroupInfo> GetSpecialDayGroupInfoListA4(this IScheduleService s)
        {
            var r = new List<SpecialDayGroupInfo>();

            string nextReference = null;
            do
            {
                SpecialDayGroupInfo[] dst = null;
                nextReference = s.GetSpecialDayGroupInfoList(null, nextReference, out dst);
                r.AddRange(dst);
            } while (!string.IsNullOrEmpty(nextReference) && !s.Test.StopRequested());

            return r;
        }

        public static List<SpecialDayGroup> GetSpecialDayGroupListA5(this IScheduleService s)
        {
            var r = new List<SpecialDayGroup>();

            string nextReference = null;
            do
            {
                SpecialDayGroup[] dst = null;
                nextReference = s.GetSpecialDayGroupList(null, nextReference, out dst);
                r.AddRange(dst);
            } while (!string.IsNullOrEmpty(nextReference) && !s.Test.StopRequested());

            return r;
        }

        public static Guid UIDiCalendarGenerationA6(this IScheduleService s)
        {
            return Guid.NewGuid();
        }

        public static string ScheduleiCalendarGenerationA7(this IScheduleService s, bool isExtendedReccurenceSupported, int increaseHour = 0)
        {
            ICalendar iCalendar = new ICalendar();
            VEvent vEvent = new VEvent();

            if (isExtendedReccurenceSupported)
            {
                System.DateTime startDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 9, 0, 0).AddHours(increaseHour);
                System.DateTime endDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 18, 0, 0).AddHours(increaseHour).AddDays(7);
                vEvent.SetDates(startDate, endDate);
                vEvent.Rrule = "FREQ=DAILY";
                vEvent.Summary = "Access from 9 AM to 6 PM";


            }
            else
            {
                System.DateTime startDate = new System.DateTime(1970, System.DateTime.Now.Month, System.DateTime.Now.Day, 9, 0, 0).AddHours(increaseHour);
                System.DateTime endDate = new System.DateTime(1970, System.DateTime.Now.Month, System.DateTime.Now.Day, 18, 0, 0).AddHours(increaseHour).AddDays(7);
                vEvent.SetDates(startDate, endDate);
                vEvent.Rrule = "FREQ=WEEKLY;BYDAY=MO,TU,WE,TH,FR";
                vEvent.Summary = "Access on weekdays from 9 AM to 6 PM for employees";
            }
            vEvent.Uid = UIDiCalendarGenerationA6(s).ToString();
            iCalendar.AddVEvent(vEvent);

            return iCalendar.ICalendarValue;

        }

        public static string CreateSpecialDayGroupA8(this IScheduleService s, out SpecialDayGroup specialDayGroup)
        {

            ICalendar iCalendar = new ICalendar();
            VEvent vEvent = new VEvent();

            vEvent.Summary = "Test special days";
            System.DateTime startDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 0, 0, 0);
            System.DateTime endDate = new System.DateTime(System.DateTime.Now.AddDays(1).Year, System.DateTime.Now.AddDays(1).Month, System.DateTime.Now.AddDays(1).Day, 0, 0, 0);
            vEvent.SetDates(startDate, endDate);
            vEvent.Uid = UIDiCalendarGenerationA6(s).ToString();

            iCalendar.AddVEvent(vEvent);

            return CreateSpecialDayGroupA8(s, out specialDayGroup, iCalendar.ICalendarValue); ;
        }

        public static string CreateSpecialDayGroupA8(this IScheduleService s, out SpecialDayGroup specialDayGroup, string days)
        {

            specialDayGroup = new SpecialDayGroup();
            specialDayGroup.token = "";
            specialDayGroup.Name = "Test SpecialDayGroup Name";
            specialDayGroup.Description = "Test SpecialDayGroup Description";
            specialDayGroup.Days = days;

            return s.CreateSpecialDayGroup(specialDayGroup);
        }

        public static void DeleteSpecialDayGroupA12(this IScheduleService s, string specialDayGroupToken)
        {
            s.DeleteSpecialDayGroup(specialDayGroupToken);
        }

        public static string CreateScheduleA13(this IScheduleService s, string scheduleiCalendarValue, bool specialDaysSupported, out Schedule schedule, string specialDayGroupToken = "")
        {
            schedule = new Schedule
                    {
                        token = "",
                        Name = "Test Name",
                        Description = "Test Description",
                        Standard = scheduleiCalendarValue,
                    };
            System.DateTime fromDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 22, 0, 0);
            if (specialDaysSupported)
            {
                schedule.SpecialDays = new SpecialDaysSchedule[]
                    {
                        new SpecialDaysSchedule
                        {
                            GroupToken = specialDayGroupToken,
                            TimeRange = new TimePeriod[]
                            {
                                new TimePeriod
                                {
                                    From = fromDate,
                                    Until = fromDate.AddHours(1),
                                    UntilSpecified = true
                                }
                            }
                        }
                    };
            }

            return CreateSchedule(s, schedule);
        }

        public static string HelperSpecialDayGroupiCalendarGenerationA16(this IScheduleService s, bool specialDayState)
        {
            ICalendar iCalendar = new ICalendar();
            VEvent vEvent = new VEvent();
            if (specialDayState)
            {
                vEvent.Summary = "Test special days";
                System.DateTime startDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 0, 0, 0);
                System.DateTime endDate = new System.DateTime(System.DateTime.Now.AddDays(1).Year, System.DateTime.Now.AddDays(1).Month, System.DateTime.Now.AddDays(1).Day, 0, 0, 0);
                vEvent.SetDates(startDate, endDate);
                vEvent.Uid = UIDiCalendarGenerationA6(s).ToString();
            }
            else
            {
                vEvent.Summary = "Test special days";
                System.DateTime startDate = new System.DateTime(System.DateTime.Now.AddDays(-2).Year, System.DateTime.Now.AddDays(-2).Month, System.DateTime.Now.AddDays(-2).Day, 0, 0, 0);
                System.DateTime endDate = new System.DateTime(System.DateTime.Now.AddDays(-1).Year, System.DateTime.Now.AddDays(-1).Month, System.DateTime.Now.AddDays(-1).Day, 0, 0, 0);
                vEvent.SetDates(startDate, endDate);
                vEvent.Uid = UIDiCalendarGenerationA6(s).ToString();
            }
            iCalendar.AddVEvent(vEvent);
            return iCalendar.ICalendarValue;
        }

        public static string HelperScheduleiCalTimePeriodsGenerationA17(this IScheduleService s, uint numberPeriodsPerDay, bool isExtendedRecurrenceSupported)
        {
            ICalendar iCalendar = new ICalendar();
            int startMinute = 1;
            System.DateTime startDate = System.DateTime.Now;
            
            if (isExtendedRecurrenceSupported)
            {
                startDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 0, startMinute, 0);
            }
            else
            {
                startDate = new System.DateTime(1970, System.DateTime.Now.Month, System.DateTime.Now.Day, 0, startMinute, 0);
            }
            System.DateTime endDate = startDate.AddMinutes(1);
            for (var k = 1; k <= numberPeriodsPerDay; k++)
            {
                if (isExtendedRecurrenceSupported)
                {
                    VEvent vEvent = new VEvent();
                    vEvent.Summary = "required number of time periods per day";
                    vEvent.SetDates(startDate, endDate);
                    vEvent.Rrule = "FREQ=DAILY";
                    vEvent.Uid = UIDiCalendarGenerationA6(s).ToString();

                    iCalendar.AddVEvent(vEvent);
                }
                else
                {
                    VEvent vEvent = new VEvent();
                    vEvent.Summary = "required number of time periods per day";
                    startMinute = endDate.Minute + 1;

                    vEvent.SetDates(startDate, endDate);
                    vEvent.Rrule = "FREQ=WEEKLY;BYDAY=MO,TU,WE,TH,FR";
                    vEvent.Uid = UIDiCalendarGenerationA6(s).ToString();

                    iCalendar.AddVEvent(vEvent);
                }
                startDate = endDate.AddMinutes(1);
                endDate = startDate.AddMinutes(1);
            }
            return iCalendar.ICalendarValue;
        }
        //public static string CreateCredentialA11(this IScheduleService s, out Credential credential, out CredentialState state, bool antipassbackViolated = false)
        //{
        //    credential = new Credential
        //                 {
        //                     token = "",
        //                     Description = "Test Description",
        //                     CredentialHolderReference = "TestUser",
        //                     CredentialIdentifier = null,    //new[] { new CredentialIdentifier { Type = new CredentialIdentifierType { Name = "ONVIFCard" }, CredentialIdentifierValue = null, ExemptedFromAuthentication = false, Extension = null } },
        //                     CredentialAccessProfile = null, //new[] { new CredentialAccessProfile { AccessProfileToken = null, ValidFromSpecified = false, ValidToSpecified = false } },
        //                     Extension = null,
        //                     ValidFromSpecified = false,
        //                     ValidToSpecified = false
        //                 };

        //    state = new CredentialState
        //                {
        //                    Enabled = true,
        //                    Reason = "Test Reason",
        //                    AntipassbackState = new AntipassbackState { AntipassbackViolated = antipassbackViolated }
        //                };

        //    return s.CreateCredential(credential, state);
        //}

        //public static string CreateCredentialA11(this IScheduleService s, bool antipassbackViolated = false)
        //{
        //    Credential credential = null;
        //    CredentialState state = null;

        //    return s.CreateCredentialA11(out credential, out state, antipassbackViolated);
        //}
    }
}
