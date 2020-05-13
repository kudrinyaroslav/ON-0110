using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DUT.PACS.Simulator.ServiceSchedule10
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

        public DateTime DateStart
        {
            get {
                return DateTime.ParseExact(DtStart, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);
            }
        }

        public DateTime DateEnd
        {
            get
            {
                return DateTime.ParseExact(DtEnd, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);
            }
        }



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

            var indexOfFirstBeginVEvent = parseiCalendar.IndexOf(string.Format("{0}:{1}", _beginFieldName, _iCalendarBeginEndVEventFieldValue));
            if (indexOfFirstBeginVEvent < 0)
            {
                return;
            }
            parseiCalendar.RemoveRange(0, indexOfFirstBeginVEvent);

            var indexOfLastEndVEvent = parseiCalendar.LastIndexOf(string.Format("{0}:{1}", _endFieldName, _iCalendarBeginEndVEventFieldValue));
            if (indexOfLastEndVEvent < 0)
            {
                return;
            }
            parseiCalendar.RemoveRange(indexOfLastEndVEvent, parseiCalendar.Count - indexOfLastEndVEvent);

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
}