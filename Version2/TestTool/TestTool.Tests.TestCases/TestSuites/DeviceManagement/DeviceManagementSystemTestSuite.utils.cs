using System;
using System.Text.RegularExpressions;
using TestTool.Tests.Definitions.Exceptions;

namespace TestTool.Tests.TestCases.TestSuites
{
    partial class DeviceManagementSystemTestSuite
    {

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
