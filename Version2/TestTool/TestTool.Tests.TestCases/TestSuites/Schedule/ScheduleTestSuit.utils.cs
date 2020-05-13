using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTool.Proxies.Onvif;
using Onvif = TestTool.Proxies.Onvif;
using TestTool.Tests.TestCases.TestSuites.Events;
using TestTool.Tests.TestCases.Utils;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.TestCases.OnvifServices;
using TestTool.Tests.TestCases.TestSuites.AccessRules;
using TestTool.Tests.Common.CommonUtils;
using NotificationMessageHolderType = TestTool.Proxies.Event.NotificationMessageHolderType;
using System.Xml;
using System.ComponentModel;
using System.ServiceModel;

namespace TestTool.Tests.TestCases.TestSuites.Schedule
{
    partial class ScheduleTestSuit
    {
        protected const string singleTab = "    ";
        protected const string doubleTab = singleTab + singleTab;
        public static bool equalScheduleCapabilities(ScheduleServiceCapabilities fromGetServiceCapabilities,
                                                       ScheduleServiceCapabilities fromGetServices,
                                                       StringBuilder logger)
        {
            bool flag = true;


            const string msgHeader = "Value of '{0}' field is inconsistent. GetServiceCapabilities: '{1}'. GetServices: '{2}'";

            if (fromGetServiceCapabilities.MaxLimit != fromGetServices.MaxLimit)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MaxLimit", fromGetServiceCapabilities.MaxLimit, fromGetServices.MaxLimit));
            }

            if (fromGetServiceCapabilities.ExtendedRecurrenceSupported != fromGetServices.ExtendedRecurrenceSupported)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "ExtendedRecurrenceSupported", fromGetServiceCapabilities.ExtendedRecurrenceSupported, fromGetServices.ExtendedRecurrenceSupported));
            }

            if (fromGetServiceCapabilities.MaxSchedules != fromGetServices.MaxSchedules)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MaxSchedules", fromGetServiceCapabilities.MaxSchedules, fromGetServices.MaxSchedules));
            }

            if (fromGetServiceCapabilities.MaxSpecialDayGroups != fromGetServices.MaxSpecialDayGroups)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MaxSpecialDayGroups", fromGetServiceCapabilities.MaxSpecialDayGroups, fromGetServices.MaxSpecialDayGroups));
            }

            if (fromGetServiceCapabilities.MaxDaysInSpecialDayGroup != fromGetServices.MaxDaysInSpecialDayGroup)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MaxSpecialDaysInSpecialDayGroup", fromGetServiceCapabilities.MaxDaysInSpecialDayGroup, fromGetServices.MaxDaysInSpecialDayGroup));
            }

            if (fromGetServiceCapabilities.MaxSpecialDaysSchedules != fromGetServices.MaxSpecialDaysSchedules)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MaxSpecialDaysSchedules", fromGetServiceCapabilities.MaxSpecialDaysSchedules, fromGetServices.MaxSpecialDaysSchedules));
            }

            if (fromGetServiceCapabilities.MaxTimePeriodsPerDay != fromGetServices.MaxTimePeriodsPerDay)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "MaxTimePeriodsPerDay", fromGetServiceCapabilities.MaxTimePeriodsPerDay, fromGetServices.MaxTimePeriodsPerDay));
            }

            if (fromGetServiceCapabilities.SpecialDaysSupported != fromGetServices.SpecialDaysSupported)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "SpecialDaysSupported", fromGetServiceCapabilities.SpecialDaysSupported, fromGetServices.SpecialDaysSupported));
            }

            if (fromGetServiceCapabilities.StateReportingSupported != fromGetServices.StateReportingSupported)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "StateReportingSupported", fromGetServiceCapabilities.StateReportingSupported, fromGetServices.StateReportingSupported));
            }

            return flag;
        }

        public static bool validateListFromGetScheduleInfo(IEnumerable<ScheduleInfo> fromGetScheduleInfo,
                                                             IEnumerable<string> tokens,
                                                             StringBuilder logger)
        {
            var notRequested = fromGetScheduleInfo.Where(e => !tokens.Any(t => t == e.token));

            if (notRequested.Any())
            {
                const string msgHeader = "The list of ScheduleInfo items received through GetScheduleInfo contains not requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notRequested.Aggregate("", (s, e) => s + ", " + e.token).Trim(',', ' ')));

                return false;
            }

            var duplicates = fromGetScheduleInfo.Where(e => fromGetScheduleInfo.Count(e1 => e1.token == e.token) >= 2).Select(e => e.token).Distinct();

            if (duplicates.Any())
            {
                const string msgHeader = "The list of ScheduleInfo items received through GetScheduleInfo contains items with the following tokens more than once: {0}";
                logger.AppendLine(string.Format(msgHeader, duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            var notReceived = tokens.Where(t => !fromGetScheduleInfo.Any(e => e.token == t));

            if (notReceived.Any())
            {
                const string msgHeader = "The list of ScheduleInfo items received through GetScheduleInfo doesn't contain requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notReceived.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            return true;
        }
        public static bool equalScheduleInfo(ScheduleInfo fromGetScheduleInfoList,
                                               ScheduleInfo fromGetScheduleInfo,
                                               StringBuilder logger,
                                               string headerFirst = "GetScheduleInfoList",
                                               string headerSecond = "GetScheduleList")
        {
            bool flag = true;

            var msgHeader = string.Format("Value of '{{0}}' field is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'", headerFirst, headerSecond);
            var msgHeader1 = string.Format("The field '{{0}}' is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'", headerFirst, headerSecond);

            if (fromGetScheduleInfoList.token != fromGetScheduleInfo.token)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "token", fromGetScheduleInfoList.token, fromGetScheduleInfo.token));
            }

            if (fromGetScheduleInfoList.Name != fromGetScheduleInfo.Name)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Name", fromGetScheduleInfoList.Name, fromGetScheduleInfo.Name));
            }

            if (fromGetScheduleInfoList.Description != fromGetScheduleInfo.Description)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Description", fromGetScheduleInfoList.Description, fromGetScheduleInfo.Description));
            }

            return flag;
        }
        public List<ScheduleInfo> receiveAndValidateScheduleInfoList(int limit, bool skipLimitInRequest = false)
        {
            var stepTitle = "Checking received list of ScheduleInfo items";
            var msgHeader = "The received list of ScheduleInfo items contains {0} items though the expected number is not more than {1}";

            string nextStartRef = null;
            var fullScheduleInfoList = new List<ScheduleInfo>();

            do
            {
                ScheduleInfo[] scheduleInfoList1;
                nextStartRef = this.GetScheduleInfoList(skipLimitInRequest ? null : new int?(limit), nextStartRef, out scheduleInfoList1);

                Assert(scheduleInfoList1.Count() <= limit,
                       string.Format(msgHeader, scheduleInfoList1.Count(), limit),
                       stepTitle);

                fullScheduleInfoList.AddRange(scheduleInfoList1);

            } while (!string.IsNullOrEmpty(nextStartRef) && !StopRequested());

            var token = fullScheduleInfoList.Select(e => e.token);
            var duplicates = token.Where(t => fullScheduleInfoList.Count(e => e.token == t) >= 2).Distinct();

            Assert(!duplicates.Any(),
                   string.Format("The complete list of ScheduleInfo items received through GetScheduleInfoList contains items with the following tokens more than once: {0}",
                                 duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')),
                   "Checking complete list of ScheduleInfo items");

            return fullScheduleInfoList;
        }

        public bool equalScheduleInfoLists(IEnumerable<ScheduleInfo> scheduleInfoFirst,
                                             IEnumerable<ScheduleInfo> scheduleInfoSecond)
        {
            return scheduleInfoFirst.Select(e => e.token).OrderBy(e => e).SequenceEqual(scheduleInfoSecond.Select(e => e.token).OrderBy(e => e));
        }

        public static bool validateListFromGetSchedules(IEnumerable<Onvif.Schedule> fromGetSchedules,
                                                          IEnumerable<string> tokens,
                                                          StringBuilder logger)
        {
            var notRequested = fromGetSchedules.Where(e => !tokens.Any(t => t == e.token));

            if (notRequested.Any())
            {
                const string msgHeader = "The list of Schedule items received through GetSchedules contains not requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notRequested.Aggregate("", (s, e) => s + string.Format(", '{0}'", e.token)).Trim(',', ' ')));

                return false;
            }

            var duplicates = fromGetSchedules.Where(e => fromGetSchedules.Count(e1 => e1.token == e.token) >= 2).Select(e => e.token).Distinct();

            if (duplicates.Any())
            {
                const string msgHeader = "The list of Schedule items received through GetSchedules contains items with the following tokens more than once: {0}";
                logger.AppendLine(string.Format(msgHeader, duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            var notReceived = tokens.Where(t => !fromGetSchedules.Any(e => e.token == t));

            if (notReceived.Any())
            {
                const string msgHeader = "The list of Schedule items received through GetSchedules doesn't contain requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notReceived.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            return true;
        }
        public static bool validateListFromGetFullScheduleList(IEnumerable<Onvif.Schedule> fromGetFullScheduleList,
                                                          IEnumerable<string> tokens,
                                                          StringBuilder logger)
        {
            var notReceived = tokens.Where(t => !fromGetFullScheduleList.Any(e => e.token == t));

            if (notReceived.Any())
            {
                const string msgHeader = "The list of Schedule items received through GetScheduleList doesn't contain items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notReceived.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            return true;
        }

        public static bool validateListFromGetFullSpecialDaysList(IEnumerable<SpecialDayGroup> fromGetFullSpecialDayGroupList,
                                                          IEnumerable<string> tokens,
                                                          StringBuilder logger)
        {
            var notReceived = tokens.Where(t => !fromGetFullSpecialDayGroupList.Any(e => e.token == t));

            if (notReceived.Any())
            {
                const string msgHeader = "The list of Special Day Group items received through GetSpecialDayGroupList doesn't contain items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notReceived.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            return true;
        }
        public static bool equalSchedule(Onvif.Schedule fromGetScheduleList,
                                           Onvif.Schedule fromGetSchedules,
                                           StringBuilder logger,
                                           string headerFirst = "GetScheduleList",
                                           string headerSecond = "GetScheduleList")
        {
            bool flag = true;

            var msgHeader = string.Format("{0}Value of '{{0}}' field is inconsistent. {1}: '{{1}}'. {2}: '{{2}}'", singleTab, headerFirst, headerSecond);
            var msgHeader1 = string.Format("{0}The field '{{0}}' is inconsistent. {1}: '{{1}}'. {2}: '{{2}}'", singleTab, headerFirst, headerSecond);

            if (fromGetScheduleList.token != fromGetSchedules.token)
            {
                logger.AppendLine(string.Format(msgHeader, "token", fromGetScheduleList.token, fromGetSchedules.token));
                flag = false;
            }

            if (flag)
            {
                if (fromGetScheduleList.Name != fromGetSchedules.Name)
                {
                    flag = false;
                    logger.AppendLine(string.Format(msgHeader, "Name", fromGetScheduleList.Name, fromGetSchedules.Name));
                }

                if (fromGetScheduleList.Description != fromGetSchedules.Description)
                {
                    flag = false;
                    logger.AppendLine(string.Format(msgHeader, "Description", fromGetScheduleList.Description, fromGetSchedules.Description));
                }

                if (!equaliCalendarValues(fromGetScheduleList.Standard, fromGetSchedules.Standard))
                {
                    flag = false;
                    logger.AppendLine(string.Format(msgHeader, "Standard", fromGetScheduleList.Standard, fromGetSchedules.Standard));
                }

                var internalLogger = new StringBuilder();
                internalLogger.Clear();
                internalLogger.AppendLine(singleTab + "ScheduleSpecialDays lists are inconsistent.");
                if (!equalScheduleSpecialDaysLists(fromGetScheduleList.SpecialDays, fromGetSchedules.SpecialDays, internalLogger))
                {
                    flag = false;
                    logger.AppendLine(internalLogger.ToStringTrimNewLine());
                }
            }

            return flag;
        }

        public static bool equalScheduleSpecialDaysLists(IEnumerable<SpecialDaysSchedule> fromGetScheduleListSpecialDays,
                                                             IEnumerable<SpecialDaysSchedule> fromGetSchedulesSpecialDays,
                                                             StringBuilder logger,
                                                             string headerFirst = "GetScheduleList",
                                                             string headerSecond = "GetSchedules")
        {
            if (null == fromGetScheduleListSpecialDays)
                fromGetScheduleListSpecialDays = new SpecialDaysSchedule[0];

            if (null == fromGetSchedulesSpecialDays)
                fromGetSchedulesSpecialDays = new SpecialDaysSchedule[0];

            if (fromGetScheduleListSpecialDays.Count() != fromGetSchedulesSpecialDays.Count())
            {
                logger.AppendLine(doubleTab + "ScheduleSpecialDays lists has different number of items");
                return false;
            }

            var flag = true;

            foreach (var scheduleSpecialDays in fromGetScheduleListSpecialDays)
            {
                var twin = fromGetSchedulesSpecialDays.FirstOrDefault(e => e.GroupToken == scheduleSpecialDays.GroupToken);

                if (null == twin)
                {
                    logger.AppendLine(string.Format("{0}There is no corresponding ScheduleSpecialDays item for item with Token = '{1}'", doubleTab, scheduleSpecialDays.GroupToken));
                    flag = false;
                }
                else
                {
                    var l = new StringBuilder();
                    l.AppendLine(string.Format("{0}The ScheduleSpecialDays items with Token = '{1}' are inconsistent.", doubleTab, scheduleSpecialDays.GroupToken));
                    if (!equalScheduleSpecialDaysTimeRange(scheduleSpecialDays.TimeRange, twin.TimeRange, l, headerFirst, headerSecond))
                    {
                        flag = false;
                        logger.AppendLine(l.ToStringTrimNewLine());
                    }
                }
            }

            return flag;
        }
        public static bool equalScheduleSpecialDaysTimeRange(IEnumerable<TimePeriod> fromGetScheduleListSpecialDaysTimeRange,
                                                             IEnumerable<TimePeriod> fromGetSchedulesSpecialDaysTimeRange,
                                                             StringBuilder logger,
                                                             string headerFirst = "GetScheduleList",
                                                             string headerSecond = "GetSchedules")
        {
            if (null == fromGetScheduleListSpecialDaysTimeRange)
                fromGetScheduleListSpecialDaysTimeRange = new TimePeriod[0];

            if (null == fromGetSchedulesSpecialDaysTimeRange)
                fromGetSchedulesSpecialDaysTimeRange = new TimePeriod[0];

            if (fromGetScheduleListSpecialDaysTimeRange.Count() != fromGetSchedulesSpecialDaysTimeRange.Count())
            {
                logger.AppendLine(doubleTab + "TimeRange lists has different number of items");
                return false;
            }

            var flag = true;

            foreach (var scheduleSpecialDaysTimeRange in fromGetScheduleListSpecialDaysTimeRange)
            {
                var twin = fromGetSchedulesSpecialDaysTimeRange.FirstOrDefault(e => e.From.ToShortTimeString() == scheduleSpecialDaysTimeRange.From.ToShortTimeString() && e.Until.ToShortTimeString() == scheduleSpecialDaysTimeRange.Until.ToShortTimeString());

                if (null == twin)
                {
                    logger.AppendLine(string.Format("{0}There is no corresponding TimeRange item for item From = '{1}', Until = '{2}'", doubleTab, scheduleSpecialDaysTimeRange.From, scheduleSpecialDaysTimeRange.Until));
                    flag = false;
                }
            }

            return flag;
        }
        public List<Onvif.Schedule> receiveAndValidateScheduleList(int limit, bool skipLimitInRequest = false)
        {
            var stepTitle = "Checking received list of Schedule items";
            var msgHeader = "The received list of Schedule items contains {0} items though the expected number is not more than {1}";

            string nextStartRef = null;
            var fullScheduleList = new List<Onvif.Schedule>();

            do
            {
                Onvif.Schedule[] scheduleList1;
                nextStartRef = this.GetScheduleList(skipLimitInRequest ? null : new int?(limit), nextStartRef, out scheduleList1);

                Assert(scheduleList1.Count() <= limit,
                       string.Format(msgHeader, scheduleList1.Count(), limit),
                       stepTitle);

                fullScheduleList.AddRange(scheduleList1);

            } while (!string.IsNullOrEmpty(nextStartRef) && !StopRequested());

            var token = fullScheduleList.Select(e => e.token);
            var duplicates = token.Where(t => fullScheduleList.Count(e => e.token == t) >= 2).Distinct();

            Assert(!duplicates.Any(),
                   string.Format("The complete list of Schedule items received through GetScheduleList contains items with the following tokens more than once: {0}",
                                 duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')),
                   "Checking complete list of Schedule items");

            return fullScheduleList;
        }

        public static bool validateListFromGetScheduleAndGetScheduleInfoConsistency(IEnumerable<ScheduleInfo> fromGetScheduleInfo,
                                                                                        IEnumerable<Onvif.Schedule> fromGetSchedules,
                                                                                        StringBuilder logger)
        {
            if (!fromGetScheduleInfo.Select(e => e.token).OrderBy(e => e).SequenceEqual(fromGetSchedules.Select(e => e.token).OrderBy(e => e)))
            {
                logger.AppendLine("The DUT returned list of Schedule items inconsistent with list of ScheduleInfo items");

                return false;
            }

            bool flag = true;
            foreach (var scheduleInfo in fromGetScheduleInfo)
            {
                var n = fromGetSchedules.Count(e => e.token == scheduleInfo.token);
                if (1 != n)
                {
                    logger.AppendLine(string.Format("The DUT returned {0} Schedule item{1} for ScheduleInfo item with token '{2}'",
                                                    0 == n ? "no" : "several",
                                                    0 == n ? "" : "s",
                                                    scheduleInfo.token));

                    flag = false;
                }
                else
                {
                    var ap = fromGetSchedules.FirstOrDefault(e => e.token == scheduleInfo.token);

                    var internalLogger = new StringBuilder();
                    if (!equalScheduleInfo(scheduleInfo, ap, internalLogger))
                    {
                        flag = false;
                        logger.AppendLine(string.Format("ScheduleInfo item with token '{0}' is inconsistent with Schedule item for the same token", scheduleInfo.token));
                        logger.Append(internalLogger);
                    }
                }
            }

            return flag;
        }
        public static bool validateListFromGetSpecialDayGroupInfo(IEnumerable<SpecialDayGroupInfo> fromGetSpecialDayGroupInfo,
                                                                                        IEnumerable<string> tokens,
                                                                                        StringBuilder logger)
        {
            var notRequested = fromGetSpecialDayGroupInfo.Where(e => !tokens.Any(t => t == e.token));

            if (notRequested.Any())
            {
                const string msgHeader = "The list of SpecialDayGroupInfo items received through GetSpecialDayGroupInfo contains not requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notRequested.Aggregate("", (s, e) => s + ", " + e.token).Trim(',', ' ')));

                return false;
            }

            var duplicates = fromGetSpecialDayGroupInfo.Where(e => fromGetSpecialDayGroupInfo.Count(e1 => e1.token == e.token) >= 2).Select(e => e.token).Distinct();

            if (duplicates.Any())
            {
                const string msgHeader = "The list of SpecialDayGroupInfo items received through GetSpecialDayGroupInfo contains items with the following tokens more than once: {0}";
                logger.AppendLine(string.Format(msgHeader, duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            var notReceived = tokens.Where(t => !fromGetSpecialDayGroupInfo.Any(e => e.token == t));

            if (notReceived.Any())
            {
                const string msgHeader = "The list of SpecialDayGroupInfo items received through GetSpecialDayGroupInfo doesn't contain requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notReceived.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            return true;
        }
        public static bool equalSpecialDayGroupInfo(SpecialDayGroupInfo fromGetSpecialDayGroupInfoList,
                                               SpecialDayGroupInfo fromGetSpecialDayGroupInfo,
                                               StringBuilder logger,
                                               string headerFirst = "GetSpecialDayGroupInfoList",
                                               string headerSecond = "GetSpecialDayGroupList")
        {
            bool flag = true;

            var msgHeader = string.Format("Value of '{{0}}' field is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'", headerFirst, headerSecond);
            var msgHeader1 = string.Format("The field '{{0}}' is inconsistent. {0}: '{{1}}'. {1}: '{{2}}'", headerFirst, headerSecond);

            if (fromGetSpecialDayGroupInfoList.token != fromGetSpecialDayGroupInfo.token)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "token", fromGetSpecialDayGroupInfoList.token, fromGetSpecialDayGroupInfo.token));
            }

            if (fromGetSpecialDayGroupInfoList.Name != fromGetSpecialDayGroupInfo.Name)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Name", fromGetSpecialDayGroupInfoList.Name, fromGetSpecialDayGroupInfo.Name));
            }

            if (fromGetSpecialDayGroupInfoList.Description != fromGetSpecialDayGroupInfo.Description)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Description", fromGetSpecialDayGroupInfoList.Description, fromGetSpecialDayGroupInfo.Description));
            }
            return flag;
        }

        public List<SpecialDayGroupInfo> receiveAndValidateSpecialDayGroupInfoList(int limit, bool skipLimitInRequest = false)
        {
            var stepTitle = "Checking received list of SpecialDayGroupInfoInfo items";
            var msgHeader = "The received list of SpecialDayGroupInfo items contains {0} items though the expected number is not more than {1}";

            string nextStartRef = null;
            var fullSpecialDayGroupInfoList = new List<SpecialDayGroupInfo>();

            do
            {
                SpecialDayGroupInfo[] specialDayGroupInfoList1;
                nextStartRef = this.GetSpecialDayGroupInfoList(skipLimitInRequest ? null : new int?(limit), nextStartRef, out specialDayGroupInfoList1);

                Assert(specialDayGroupInfoList1.Count() <= limit,
                       string.Format(msgHeader, specialDayGroupInfoList1.Count(), limit),
                       stepTitle);

                fullSpecialDayGroupInfoList.AddRange(specialDayGroupInfoList1);

            } while (!string.IsNullOrEmpty(nextStartRef) && !StopRequested());

            var token = fullSpecialDayGroupInfoList.Select(e => e.token);
            var duplicates = token.Where(t => fullSpecialDayGroupInfoList.Count(e => e.token == t) >= 2).Distinct();

            Assert(!duplicates.Any(),
                   string.Format("The complete list of SpecialDayGroupInfo items received through GetSpecialDayGroupInfoList contains items with the following tokens more than once: {0}",
                                 duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')),
                   "Checking complete list of SpecialDayGroupInfo items");

            return fullSpecialDayGroupInfoList;
        }

        public bool equalSpecialDayGroupInfoLists(IEnumerable<SpecialDayGroupInfo> specialDayGroupInfoFirst,
                                             IEnumerable<SpecialDayGroupInfo> specialDayGroupInfoSecond)
        {
            return specialDayGroupInfoFirst.Select(e => e.token).OrderBy(e => e).SequenceEqual(specialDayGroupInfoSecond.Select(e => e.token).OrderBy(e => e));
        }

        public static bool validateListFromGetSpecialDayGroups(IEnumerable<SpecialDayGroup> fromGetSpecialDayGroups,
                                                          IEnumerable<string> tokens,
                                                          StringBuilder logger)
        {
            var notRequested = fromGetSpecialDayGroups.Where(e => !tokens.Any(t => t == e.token));

            if (notRequested.Any())
            {
                const string msgHeader = "The list of Special Day Group items received through GetSpecialDayGroups contains not requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notRequested.Aggregate("", (s, e) => s + string.Format(", '{0}'", e.token)).Trim(',', ' ')));

                return false;
            }

            var duplicates = fromGetSpecialDayGroups.Where(e => fromGetSpecialDayGroups.Count(e1 => e1.token == e.token) >= 2).Select(e => e.token).Distinct();

            if (duplicates.Any())
            {
                const string msgHeader = "The list of Special Day Group items received through GetSpecialDayGroups contains items with the following tokens more than once: {0}";
                logger.AppendLine(string.Format(msgHeader, duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            var notReceived = tokens.Where(t => !fromGetSpecialDayGroups.Any(e => e.token == t));

            if (notReceived.Any())
            {
                const string msgHeader = "The list of Special Day Group items received through GetSpecialDayGroups doesn't contain requested items with the following tokens: {0}";
                logger.AppendLine(string.Format(msgHeader, notReceived.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')));

                return false;
            }

            return true;
        }

        public static bool equalSpecialDayGroup(SpecialDayGroup fromGetSpecialDayGroupList,
                                           SpecialDayGroup fromGetSpecialDayGroups,
                                           StringBuilder logger,
                                           string headerFirst = "GetSpecialDayGroupList",
                                           string headerSecond = "GetSpecialDayGroups")
        {
            bool flag = true;

            var msgHeader = string.Format("{0}Value of '{{0}}' field is inconsistent.{3}{1}:{3}'{{1}}'.{3}{2}:{3}'{{2}}'", singleTab, headerFirst, headerSecond, Environment.NewLine);
            var msgHeader1 = string.Format("{0}The field '{{0}}' is inconsistent.{3}{1}:{3}'{{1}}'.{3}{2}:{3}'{{2}}'", singleTab, headerFirst, headerSecond, Environment.NewLine);

            if (fromGetSpecialDayGroupList.token != fromGetSpecialDayGroups.token)
            {
                logger.AppendLine(string.Format(msgHeader, "token", fromGetSpecialDayGroupList.token, fromGetSpecialDayGroups.token));
                return false;
            }

            if (fromGetSpecialDayGroupList.Name != fromGetSpecialDayGroups.Name)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Name", fromGetSpecialDayGroupList.Name, fromGetSpecialDayGroups.Name));
            }

            if (fromGetSpecialDayGroupList.Description != fromGetSpecialDayGroups.Description)
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Description", fromGetSpecialDayGroupList.Description, fromGetSpecialDayGroups.Description));
            }

            if (!equaliCalendarValues(fromGetSpecialDayGroupList.Days, fromGetSpecialDayGroups.Days))
            {
                flag = false;
                logger.AppendLine(string.Format(msgHeader, "Days", fromGetSpecialDayGroupList.Days, fromGetSpecialDayGroups.Days));
            }

            return flag;
        }

        public List<SpecialDayGroup> receiveAndValidateSpecialDayGroupList(int limit, bool skipLimitInRequest = false)
        {
            var stepTitle = "Checking received list of SpecialDayGroup items";
            var msgHeader = "The received list of SpecialDayGroup items contains {0} items though the expected number is not more than {1}";

            string nextStartRef = null;
            var fullSpecialDayGroupList = new List<SpecialDayGroup>();

            do
            {
                SpecialDayGroup[] specialDayGroupList1;
                nextStartRef = this.GetSpecialDayGroupList(skipLimitInRequest ? null : new int?(limit), nextStartRef, out specialDayGroupList1);

                Assert(specialDayGroupList1.Count() <= limit,
                       string.Format(msgHeader, specialDayGroupList1.Count(), limit),
                       stepTitle);

                fullSpecialDayGroupList.AddRange(specialDayGroupList1);

            } while (!string.IsNullOrEmpty(nextStartRef) && !StopRequested());

            var token = fullSpecialDayGroupList.Select(e => e.token);
            var duplicates = token.Where(t => fullSpecialDayGroupList.Count(e => e.token == t) >= 2).Distinct();

            Assert(!duplicates.Any(),
                   string.Format("The complete list of SpecialDayGroup items received through GetSpecialDayGroupList contains items with the following tokens more than once: {0}",
                                 duplicates.Aggregate("", (s, e) => s + string.Format(", '{0}'", e)).Trim(',', ' ')),
                   "Checking complete list of SpecialDayGroup items");

            return fullSpecialDayGroupList;
        }

        public static bool validateListFromGetSpecialDayGroupAndGetSpecialDayGroupInfoConsistency(IEnumerable<SpecialDayGroupInfo> fromGetSpecialDayGroupInfo,
                                                                                        IEnumerable<SpecialDayGroup> fromGetSpecialDayGroups,
                                                                                        StringBuilder logger)
        {
            if (!fromGetSpecialDayGroupInfo.Select(e => e.token).OrderBy(e => e).SequenceEqual(fromGetSpecialDayGroups.Select(e => e.token).OrderBy(e => e)))
            {
                logger.AppendLine("The DUT returned list of SpecialDayGroup items inconsistent with list of SpecialDayGroupInfo items");

                return false;
            }

            bool flag = true;
            foreach (var specialDayGroupInfo in fromGetSpecialDayGroupInfo)
            {
                var n = fromGetSpecialDayGroups.Count(e => e.token == specialDayGroupInfo.token);
                if (1 != n)
                {
                    logger.AppendLine(string.Format("The DUT returned {0} SpecialDayGroup item{1} for SpecialDayGroupInfo item with token '{2}'",
                                                    0 == n ? "no" : "several",
                                                    0 == n ? "" : "s",
                                                    specialDayGroupInfo.token));

                    flag = false;
                }
                else
                {
                    var ap = fromGetSpecialDayGroups.FirstOrDefault(e => e.token == specialDayGroupInfo.token);

                    var internalLogger = new StringBuilder();
                    if (!equalSpecialDayGroupInfo(specialDayGroupInfo, ap, internalLogger))
                    {
                        flag = false;
                        logger.AppendLine(string.Format("SpecialDayGroup item with token '{0}' is inconsistent with SpecialDayGroupInfo item for the same token", specialDayGroupInfo.token));
                        logger.Append(internalLogger);
                    }
                }
            }

            return flag;
        }

        #region Filter utiles
        bool messageFilterBase(NotificationMessageHolderType msg, TopicInfo topicInfo, string expectedPropertyOperation, string elementName, string expectedScheduleToken, Func<StringBuilder, bool> action)
        {
            var invalidFlag = false;
            var log = new StringBuilder();
            try
            {
                if (!EventServiceExtensions.NotificationTopicMatch(msg, topicInfo))
                {
                    log.AppendLine(string.Format("Received notification has unexpected topic '{0}' while the expected one should have with topic '{1}'",
                                                 EventServiceExtensions.GetTopicInfo(msg).GetDescription(), topicInfo.GetDescription()));
                    invalidFlag = true;
                }

                if (null != expectedPropertyOperation)
                {
                    if (!msg.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                    {
                        log.AppendLine("Received notification has no 'PropertyOperation' field");
                        invalidFlag = true;
                    }
                    else
                    {
                        XmlAttribute propertyOperationType = msg.Message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];
                        //Skip non-changed messages
                        if (propertyOperationType.Value != expectedPropertyOperation)
                        {
                            return false;
                        }
                    }
                }

                var source = msg.Message.GetMessageSourceSimpleItems();
                if (!string.IsNullOrEmpty(elementName))
                {
                    if (!source.ContainsKey(elementName))
                    {
                        log.AppendLine(string.Format("Received notification has no Source/SimpleItem with Name = '{0}'", elementName));
                        invalidFlag = true;
                    }
                    else
                    {
                        var scheduleToken = source[elementName];
                        if (scheduleToken != expectedScheduleToken)
                        {
                            log.AppendLine(string.Format("Received notification has Source/SimpleItem with Name = '{2}' and Value = '{0}' but notification for Schedule item with token = '{1}' is expected", scheduleToken, expectedScheduleToken, elementName));
                            invalidFlag = true;
                        }
                    }
                }

                if (null != action && action(log))
                    invalidFlag = true;

                return true;
            }
            finally
            {
                Assert(!invalidFlag, log.ToStringTrimNewLine(), "Validation of received notification");
            }
        }

        bool messageFilterBase(Proxies.Event.NotificationMessageHolderType message,
                               TopicInfo topicInfo,
                               string expectedPropertyOperation,
                               string validationScript,
                               IEnumerable<Onvif.Schedule> scheduleList)
        {
            if (!string.IsNullOrEmpty(expectedPropertyOperation))
            {
                if (!message.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                    return true;

                XmlAttribute propertyOperationType = message.Message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];
                if (expectedPropertyOperation != propertyOperationType.Value)
                    return false;
            }

            var variables = new Dictionary<string, string>();
            variables.Add("scheduleList", string.Format(@"{{{0}}}", string.Join(",", scheduleList.Select(s => string.Format(@"""{0}"" : ""{1}""", s.token, s.Name)))));


            var logger = new StringBuilder();
            string validationScriptPath = string.Format("TestTool.Tests.TestCases.TestSuites.Schedule.Scripts.{0}", validationScript);
            Assert(ValidationEngine.GetInstance().Validate(message,
                                                           topicInfo,
                                                           validationScriptPath,
                                                           logger,
                                                           variables),
                   logger.ToStringTrimNewLine(),
                   "Validate received notification(s)");

            return true;
        }

        bool messageFilterBase(Proxies.Event.NotificationMessageHolderType message,
                               TopicInfo topicInfo,
                               string expectedPropertyOperation,
                               string validationScript,
                               IEnumerable<SpecialDayGroup> specialDayGroups)
        {
            if (!string.IsNullOrEmpty(expectedPropertyOperation))
            {
                if (!message.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                    return true;

                XmlAttribute propertyOperationType = message.Message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];
                if (expectedPropertyOperation != propertyOperationType.Value)
                    return false;
            }

            var variables = new Dictionary<string, string>();
            variables.Add("specialDayGroups", string.Format(@"{{{0}}}", string.Join(",", specialDayGroups.Select(s => string.Format(@"""{0}"" : ""{1}""", s.token, s.Name)))));


            var logger = new StringBuilder();
            string validationScriptPath = string.Format("TestTool.Tests.TestCases.TestSuites.Schedule.Scripts.{0}", validationScript);
            Assert(ValidationEngine.GetInstance().Validate(message,
                                                           topicInfo,
                                                           validationScriptPath,
                                                           logger,
                                                           variables),
                   logger.ToStringTrimNewLine(),
                   "Validate received notification(s)");

            return true;
        }

        bool messageFilterBase(Proxies.Event.NotificationMessageHolderType message,
                               TopicInfo topicInfo,
                               string expectedPropertyOperation,
                               string validationScript,
                               string elementName,
                               string expectedElementValue,
                               bool activeValueExpected,
                               bool specialDayValueExpected,
                               Onvif.Schedule schedule)
        {
            if (!string.IsNullOrEmpty(expectedPropertyOperation))
            {
                if (!message.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
                    return true;

                XmlAttribute propertyOperationType = message.Message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];
                if (expectedPropertyOperation != propertyOperationType.Value)
                    return false;
            }

            var source = message.Message.GetMessageSourceSimpleItems();
            if (!string.IsNullOrEmpty(elementName))
            {
                if (source.ContainsKey(elementName))
                {
                    var scheduleToken = source[elementName];
                    if (scheduleToken != expectedElementValue)
                    {
                        return false;
                    }
                }
            }

            var variables = new Dictionary<string, string>();
            variables.Add("scheduleToken", schedule.token);
            variables.Add("scheduleNameExpected", schedule.Name);
            variables.Add("activeExpected", activeValueExpected.ToString().ToLower());
            variables.Add("specialDayExpected", specialDayValueExpected.ToString().ToLower());


            var logger = new StringBuilder();
            string validationScriptPath = string.Format("TestTool.Tests.TestCases.TestSuites.Schedule.Scripts.{0}", validationScript);
            Assert(ValidationEngine.GetInstance().Validate(message,
                                                           topicInfo,
                                                           validationScriptPath,
                                                           logger,
                                                           variables),
                   logger.ToStringTrimNewLine(),
                   "Validate received notification(s)");

            return true;
        }

        protected Proxies.Event.FilterType CreateSubscriptionFilter(TopicInfo topicInfo)
        {
            return CreateSubscriptionFilter(new TopicInfo[] { topicInfo });
        }

        protected Proxies.Event.FilterType CreateSubscriptionFilter(IEnumerable<TopicInfo> topicInfos)
        {
            Proxies.Event.FilterType filter = new Proxies.Event.FilterType();

            XmlDocument filterDoc = new XmlDocument();
            XmlElement filterTopicElement = filterDoc.CreateTopicElement();

            string topicPath = string.Empty;
            foreach (TopicInfo topicInfo in topicInfos)
            {
                string topicExpression = TopicInfo.CreateTopicPath(filterTopicElement, topicInfo);

                if (string.IsNullOrEmpty(topicPath))
                {
                    topicPath = topicExpression;
                }
                else
                {
                    topicPath = string.Format("{0}|{1}", topicPath, topicExpression);
                }
            }

            filterTopicElement.InnerText = topicPath;

            filter.Any = new XmlElement[] { filterTopicElement };

            return filter;
        }

        #endregion

        #region Notification validation
        protected bool ValidateNotifications(Dictionary<NotificationMessageHolderType, XmlElement> messages, TopicInfo topic, string elementName, string elementValue, StringBuilder logger)
        {
            var filtered = messages.Where(pair => EventServiceExtensions.NotificationTopicMatch(pair.Key, pair.Value, topic)).Select(k => k.Key);

            if (!filtered.Any())
            {
                logger.AppendLine(string.Format("There is no required notification with topic '{0}'", topic.GetDescription()));
                return false;
            }

            foreach (var msg in filtered)
            {
                var source = msg.Message.GetMessageSourceSimpleItems();

                if (source.Any(s => s.Key == elementName && s.Value == elementValue))
                    return true;
            }

            logger.AppendLine(string.Format("There is no notification containing 'Source/SimpleItem' element with Name = '{1}' and Value = '{0}'", elementValue, elementName));
            return false;
        }

        protected bool ValidateNotifications(Dictionary<NotificationMessageHolderType, XmlElement> messages, TopicInfo topic, string elementName, IEnumerable<string> elementValue, StringBuilder logger)
        {
            var filtered = messages.Where(pair => EventServiceExtensions.NotificationTopicMatch(pair.Key, pair.Value, topic)).Select(k => k.Key);

            if (!filtered.Any())
            {
                logger.AppendLine(string.Format("There is no required notification with topic '{0}'", topic.GetDescription()));
                return false;
            }

            foreach (var msg in filtered)
            {
                var source = msg.Message.GetMessageSourceSimpleItems();

                if (source.Any(s => s.Key == elementName && elementValue.Contains(s.Value)))
                    return true;
            }

            logger.AppendLine(string.Format("There is no notification containing 'Source/SimpleItem' element with Name = '{0}'", elementName));
            return false;
        }
        #endregion

        #region Validate consistency

        public void ValidateConsistency(Onvif.Schedule initial,
                                        Onvif.Schedule received,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            logger.AppendLine(string.Format("The Schedule item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != received && equalSchedule(initial, received, logger, commandInitial, commandReceive),
                   null == received ? string.Format("The {0} command returned no Schedule item", commandReceive) : logger.ToStringTrimNewLine(),
                   "Checking received Schedule item");
        }

        public void ValidateConsistency(Onvif.Schedule initial,
                                        IEnumerable<Onvif.Schedule> receivedList,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            var twin = receivedList.FirstOrDefault(e => e.token == initial.token);
            logger.AppendLine(string.Format("The Schedule item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != twin && equalSchedule(initial, twin, logger, commandInitial, commandReceive),
                   null == twin ? string.Format("Received Schedule list doesn't contain item with token = '{0}'", initial.token) : logger.ToStringTrimNewLine(),
                   "Checking received Schedule item");
        }

        public void ValidateConsistency(ScheduleInfo initial,
                                        ScheduleInfo received,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            logger.AppendLine(string.Format("The ScheduleInfo item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != received && equalScheduleInfo(initial, received, logger, commandInitial, commandReceive),
                   null == received ? string.Format("The {0} command returned no ScheduleInfo item", commandReceive) : logger.ToStringTrimNewLine(),
                   "Checking received ScheduleInfo item");
        }

        public void ValidateConsistency(ScheduleInfo initial,
                                        IEnumerable<ScheduleInfo> receivedList,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            var twin = receivedList.FirstOrDefault(e => e.token == initial.token);
            logger.AppendLine(string.Format("The ScheduleInfo item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != twin && equalScheduleInfo(initial, twin, logger, commandInitial, commandReceive),
                   null == twin ? string.Format("Received ScheduleInfo list doesn't contain item with token = '{0}'", initial.token) : logger.ToStringTrimNewLine(),
                   "Checking received ScheduleInfo item");
        }
        public void ValidateConsistency(SpecialDayGroup initial,
                                        SpecialDayGroup received,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            logger.AppendLine(string.Format("The SpecialDayGroup item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != received && equalSpecialDayGroup(initial, received, logger, commandInitial, commandReceive),
                   null == received ? string.Format("The {0} command returned no SpecialDayGroup item", commandReceive) : logger.ToStringTrimNewLine(),
                   "Checking received SpecialDayGroup item");
        }

        public void ValidateConsistency(SpecialDayGroup initial,
                                        IEnumerable<SpecialDayGroup> receivedList,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            var twin = receivedList.FirstOrDefault(e => e.token == initial.token);
            logger.AppendLine(string.Format("The SpecialDayGroup item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != twin && equalSpecialDayGroup(initial, twin, logger, commandInitial, commandReceive),
                   null == twin ? string.Format("Received SpecialDayGroup list doesn't contain item with token = '{0}'", initial.token) : logger.ToStringTrimNewLine(),
                   "Checking received SpecialDayGroup item");
        }

        public void ValidateConsistency(SpecialDayGroupInfo initial,
                                        SpecialDayGroupInfo received,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            logger.AppendLine(string.Format("The SpecialDayGroupInfo item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != received && equalSpecialDayGroupInfo(initial, received, logger, commandInitial, commandReceive),
                   null == received ? string.Format("The {0} command returned no SpecialDayGroupInfo item", commandReceive) : logger.ToStringTrimNewLine(),
                   "Checking received SpecialDayGroupInfo item");
        }

        public void ValidateConsistency(SpecialDayGroupInfo initial,
                                        IEnumerable<SpecialDayGroupInfo> receivedList,
                                        string commandInitial,
                                        string commandReceive)
        {
            var logger = new StringBuilder();
            var twin = receivedList.FirstOrDefault(e => e.token == initial.token);
            logger.AppendLine(string.Format("The SpecialDayGroupInfo item sent in {0} command is inconsistent with received one through {1} command:", commandInitial, commandReceive));
            Assert(null != twin && equalSpecialDayGroupInfo(initial, twin, logger, commandInitial, commandReceive),
                   null == twin ? string.Format("Received SpecialDayGroupInfo list doesn't contain item with token = '{0}'", initial.token) : logger.ToStringTrimNewLine(),
                   "Checking received SpecialDayGroupInfo item");
        }

        public void ValidateConsistency(string scheduleToken,
                                        IEnumerable<Onvif.Schedule> receivedSchedules)
        {
            var logger = new StringBuilder();
            var twin = receivedSchedules.Any(e => e.token == scheduleToken);
            Assert(twin && receivedSchedules.Any(),
                   true == twin ? "Previosly removed Schedule exists and didn't removed!" : "Received list of Schedules is not empty",
                   "Verifying removed schedule cannot be retrieved");
        }

        public void ValidateConsistency(string scheduleToken,
                                        IEnumerable<ScheduleInfo> receivedScheduleInfoes)
        {
            var logger = new StringBuilder();
            var twin = receivedScheduleInfoes.Any(e => e.token == scheduleToken);
            Assert(twin && receivedScheduleInfoes.Any(),
                   false == twin ? "Previosly removed Schedule Info exists and didn't removed!" : "GetScheduleInfoList retrives deleted schedule info for token = \"" + scheduleToken + "\"",
                   "Verifying removed schedule info cannot be retrieved");
        }

        public void ValidateConsistency(ScheduleState scheduleState,
                                        Onvif.Schedule schedule)
        {
            bool isEqualsSpecialDay = (scheduleState.SpecialDay && schedule.SpecialDays != null && schedule.SpecialDays.Length > 0) ||
                            (!scheduleState.SpecialDay && (schedule.SpecialDays == null || schedule.SpecialDays.Length == 0));
            string errorMessage = "";
            if (!isEqualsSpecialDay)
            {
                errorMessage = scheduleState.SpecialDay ? string.Format("ScheduleToken = {0} does not contain items of SpecialDays, but there are items in ScheduleState.SpecialDay", schedule.token) :
                errorMessage = string.Format("ScheduleToken = {0} contains {1} items of SpecialDays, but there is no item ScheduleState.SpecialDay", schedule.token, schedule.SpecialDays.Length);
            }
            Assert(isEqualsSpecialDay,
                errorMessage,
                "Checking that ScheduleState Special Days has value");
        }

        public void ValidateDeleteConsistency(IEnumerable<Onvif.Schedule> schedules,
                                        string scheduleToken,
                                        string commandName, string stepMessage, bool isList = false)
        {
            bool twin = schedules.Any(e => e.token == scheduleToken);
            string errorMessage = "";
            if (twin)
            {
                errorMessage = string.Format("{1} request retrieved the removed Schedule with token = {0}", scheduleToken, commandName);
            }
            else if (schedules.Any() && !twin && !isList)
            {
                errorMessage = string.Format("Received {1} list for Schedule token = {0} is not empty", scheduleToken, commandName);
            }
            Assert(string.IsNullOrEmpty(errorMessage),
                       errorMessage,
                       stepMessage);
        }

        public void ValidateDeleteConsistency(IEnumerable<ScheduleInfo> scheduleInfos,
                                        string scheduleToken,
                                        string commandName, string stepMessage, bool isList = false)
        {
            bool twin = scheduleInfos.Any(e => e.token == scheduleToken);
            string errorMessage = "";
            if (twin)
            {
                errorMessage = string.Format("{1} request retrieved the removed Schedule with token = {0}", scheduleToken, commandName);
            }
            else if (scheduleInfos.Any() && !twin && !isList)
            {
                errorMessage = string.Format("Received {1} list for Schedule Token = {0} is not empty", scheduleToken, commandName);
            }
            Assert(string.IsNullOrEmpty(errorMessage),
                       errorMessage,
                       stepMessage);
        }

        public void ValidateDeleteConsistency(IEnumerable<SpecialDayGroup> specialDays,
                                        string specialDayToken,
                                        string commandName, string stepMessage, bool isList = false)
        {
            bool twin = specialDays.Any(e => e.token == specialDayToken);
            string errorMessage = "";
            if (twin)
            {
                errorMessage = string.Format("{1} request retrieved the removed SpecialDayGroup with token = {0}", specialDayToken, commandName);
            }
            else if (specialDays.Any() && !twin && !isList)
            {
                errorMessage = string.Format("Received {1} list for SpecialDayGroup Token = {0} is not empty", specialDayToken, commandName);
            }
            Assert(string.IsNullOrEmpty(errorMessage),
                       errorMessage,
                       stepMessage);
        }

        public void ValidateDeleteConsistency(IEnumerable<SpecialDayGroupInfo> specialDayInfos,
                                        string specialDayToken,
                                        string commandName, string stepMessage, bool isList = false)
        {
            bool twin = specialDayInfos.Any(e => e.token == specialDayToken);
            string errorMessage = "";
            if (twin)
            {
                errorMessage = string.Format("{1} request retrieved the removed SpecialDayGroup with token = {0}", specialDayToken, commandName);
            }
            else if (specialDayInfos.Any() && !twin && !isList)
            {
                errorMessage = string.Format("Received {1} list for SpecialDayGroup Token = {0} is not empty", specialDayToken, commandName);
            }
            Assert(string.IsNullOrEmpty(errorMessage),
                       errorMessage,
                       stepMessage);
        }

        #endregion

        public string GetSpecialDaysiCalendarValue(System.DateTime startDate, System.DateTime endDate, Guid uId, string summary = "Test special days", string rrule = "")
        {
            ICalendar iCalendar = new ICalendar();
            VEvent vEvent = new VEvent();

            vEvent.Summary = summary;
            vEvent.SetDates(startDate, endDate);
            vEvent.Uid = uId.ToString();
            vEvent.Rrule = rrule;

            iCalendar.AddVEvent(vEvent);

            return iCalendar.ICalendarValue;
        }
        private static bool equaliCalendarValues(string firstiCalendarValue, string secondiCalendarValue)
        {

            if (string.IsNullOrEmpty(firstiCalendarValue) && string.IsNullOrEmpty(secondiCalendarValue))
            {
                return true;
            }
            if (string.IsNullOrEmpty(firstiCalendarValue) || string.IsNullOrEmpty(secondiCalendarValue))
            {
                return false;
            }

            ICalendar firstICalendar = new ICalendar(firstiCalendarValue);
            ICalendar secondICalendar = new ICalendar(secondiCalendarValue);

            List<VEvent> firstICalendarVEvents = firstICalendar.GetVEvents();
            List<VEvent> secondICalendarVEvents = secondICalendar.GetVEvents();

            if (firstICalendarVEvents == null && secondICalendarVEvents == null)
            {
                return true;
            }

            if ((firstICalendarVEvents == null && secondICalendarVEvents != null) ||
                (firstICalendarVEvents != null && secondICalendarVEvents == null))
            {
                return false;
            }

            if (firstICalendarVEvents.Count != secondICalendarVEvents.Count)
            {
                return false;
            }

            var secondICalendarVEventsValues = secondICalendarVEvents.Select(e => e.VEventValueWithoutUid);
            foreach (var firstICalendarVEvent in firstICalendarVEvents)
            {
                if (!secondICalendarVEventsValues.Contains(firstICalendarVEvent.VEventValueWithoutUid))
                {
                    return false;
                }
                var secondICalendarVEvent = secondICalendarVEvents.Where(e => e.VEventValueWithoutUid == firstICalendarVEvent.VEventValueWithoutUid).First();
                var firstAnotherFields = firstICalendarVEvent.GetAnotherFields();
                var secondAnotherFields = secondICalendarVEvent.GetAnotherFields();

                if (!(firstAnotherFields != null && secondAnotherFields != null &&
                firstAnotherFields.Count == secondAnotherFields.Count &&
                !firstAnotherFields.Keys.Any(e => !secondAnotherFields.Keys.Contains(e)) &&
                !secondAnotherFields.Values.Any(e => !secondAnotherFields.Values.Contains(e))))
                {
                    return false;
                }
            }

            return true;
        }

        protected void FindTopics(XmlElement element, List<XmlElement> topics)
        {
            if (element.RepresentsTopic())
            {
                topics.Add(element);
            }

            // If not a topic - enumerate child elements.
            foreach (XmlNode node in element.ChildNodes)
            {
                XmlElement child = node as XmlElement;
                if (child == null)
                {
                    continue;
                }
                FindTopics(child, topics);
            }
        }

        protected XmlElement GetTopicElement(IEnumerable<XmlElement> topics, TopicInfo topicInfo)
        {
            // check if "our" topic is present
            XmlElement topicElement = null;
            foreach (XmlElement el in topics)
            {
                TopicInfo info = TopicInfo.ConstructTopicInfo(el);
                if (TopicInfo.TopicsMatch(info, topicInfo))
                {
                    topicElement = el;
                    break;
                }
            }
            return topicElement;
        }

        #region Schedule event description

        internal class EventItemDescription
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Namespace { get; set; }
            public bool Mandatory { get; set; }

            public EventItemDescription()
                : this(null, null, null, null)
            { }

            public EventItemDescription(string path, string name, string type, string ns)
                : this(path, name, type, ns, true)
            { }

            public EventItemDescription(string path, string name, string type, string ns, bool mandatory)
            {
                Path = path;
                Name = name;
                Namespace = ns;
                Type = type;
                Mandatory = mandatory;
            }
        }

        internal class ScheduleEventDescription
        {
            public List<EventItemDescription> itemDescriptions { get; private set; }

            public ScheduleEventDescription()
            { itemDescriptions = new List<EventItemDescription>(); }

            public void addItemDescription(EventItemDescription description) { itemDescriptions.Add(description); }

            public bool isProperty { get; set; }
        }

        #endregion
        #region Validating utils

        bool checkEventDescription(XmlElement eventNode,
                                   ScheduleEventDescription eventDescription,
                                   IXmlNamespaceResolver namespaceResolver,
                                   StringBuilder logger)
        {
            return checkEventDescription(eventNode, eventDescription, namespaceResolver.LookupNamespace, logger);
        }

        bool checkEventDescription(XmlElement eventNode,
                                   ScheduleEventDescription eventDescription,
                                   XmlElement namespaceResolver,
                                   StringBuilder logger)
        {
            return checkEventDescription(eventNode, eventDescription, namespaceResolver.GetNamespaceOfPrefix, logger);
        }

        bool checkEventDescription(XmlElement eventNode, ScheduleEventDescription eventDescription, Func<string, string> namespaceResolver, StringBuilder logger)
        {
            var descriptionNode = eventNode.GetMessageDescription();
            if (null == descriptionNode)
            {
                logger.AppendLine("MessageDescription element is absent.");
                return false;
            }

            var manager = new XmlNamespaceManager(eventNode.OwnerDocument.NameTable);
            manager.AddNamespace(OnvifMessage.ONVIFPREFIX, OnvifMessage.ONVIF);

            var isPropertyAttribute = descriptionNode.Attributes[OnvifMessage.ISPROPERTY];
            if (null == isPropertyAttribute)
            {
                //IsProperty == false in description
                if (eventDescription.isProperty)
                {
                    logger.AppendLine("The 'IsProperty' attribute is absent but expected with value 'true'");
                    return false;
                }
            }
            else if (XmlConvert.ToBoolean(isPropertyAttribute.Value) != eventDescription.isProperty)
            {
                logger.AppendLine(string.Format("The value of 'IsProperty' attribute is incorrect. Expected: {0}. Actual: {1}",
                                                eventDescription.isProperty,
                                                isPropertyAttribute.Value));
                return false;
            }


            bool flag = true;
            foreach (var itemDescription in eventDescription.itemDescriptions)
            {
                var path = itemDescription.Path.Split('/').Select(e => "tt:" + e).Aggregate("", (s, s1) => s + s1 + "/").Trim('/');
                var nodes = descriptionNode.SelectNodes(path, manager).OfType<XmlElement>();
                var itemNode = nodes.FirstOrDefault(e => null != e.Attributes[OnvifMessage.NAME]
                                                         && e.Attributes[OnvifMessage.NAME].Value == itemDescription.Name);
                if (null == itemNode)
                {
                    if (itemDescription.Mandatory)
                    {
                        flag = false;
                        logger.AppendLine(string.Format("Mandatory element {0} of type '{1}' is absent", itemDescription.Name, itemDescription.Type));
                    }
                }
                else
                {
                    XmlAttribute type = itemNode.Attributes[OnvifMessage.TYPE];
                    if (type == null)
                    {
                        flag = false;
                        logger.AppendLine(string.Format("'Type' attribute is missing for '{0}' simple item", itemDescription.Name));
                    }
                    else
                    {
                        string error = string.Empty;
                        if (!type.IsCorrectQName(itemDescription.Type, itemDescription.Namespace, namespaceResolver, ref error))
                        {
                            flag = false;
                            logger.AppendLine(string.Format("'Type' attribute is incorrect for '{0}' simple item: {1}", itemDescription.Name, error));
                        }
                    }
                }
            }

            return flag;
        }
        #endregion

        public class WaitNotificationsForAllSchedulesPollingCondition : SubscriptionHandler.PollingConditionBase
        {
            private TopicInfo _topicInfo;
            public WaitNotificationsForAllSchedulesPollingCondition(int timeout, IEnumerable<string> waitingNotificationsFor, TopicInfo topicInfo)
                : base(timeout)
            {
                _topicInfo = topicInfo;
                m_WaitingNotificationsFor = new HashSet<string>(waitingNotificationsFor);
            }

            public override bool StopPulling
            {
                get { return !m_WaitingNotificationsFor.Any(); }
            }

            public override string Reason
            {
                get
                {
                    if (m_WaitingNotificationsFor.Any())
                    {
                        var log = new StringBuilder();
                        log.AppendLine("Not all required notifications are received");
                        var tokens = string.Join(", ", m_WaitingNotificationsFor.Select(e => string.Format("'{0}'", e)).ToArray()).Trim(new[] { ' ', ',' });
                        if (m_WaitingNotificationsFor.Count() > 1)
                            log.AppendFormat("No notifications for the schedules with tokens: {0}", tokens);
                        else
                            log.AppendFormat("No notification for the schedules with token: {0}", tokens);

                        return log.ToString();
                    }
                    else
                        return "Notifications for all schedules are received";
                }
            }

            public override void Update(Dictionary<NotificationMessageHolderType, XmlElement> messages)
            {
                if (null != messages)
                    foreach (var msg in messages.Keys)
                    {
                        string scheduleToken = null;
                        if (null != msg.Message.GetMessageSourceSimpleItems()
                            && msg.Message.GetMessageSourceSimpleItems().ContainsKey("ScheduleToken"))
                            scheduleToken = msg.Message.GetMessageSourceSimpleItems()["ScheduleToken"];

                        string validationScriptPath = string.Format("TestTool.Tests.TestCases.TestSuites.Schedule.Scripts.ScheduleStatActiveEvent.PropertyOperation.xq");
                        StringBuilder sb = new StringBuilder();
                        bool isValidMessage = ValidationEngine.GetInstance().Validate(msg,
                                                                _topicInfo,
                                                                validationScriptPath,
                                                                sb);

                        if (null != scheduleToken && isValidMessage)
                            m_WaitingNotificationsFor.Remove(scheduleToken);
                    }
            }

            private readonly HashSet<string> m_WaitingNotificationsFor;
        }

        public void CheckRequestedInfo<T>(IEnumerable<T> list,
            string token, string itemName, Func<T, string> tokenSelector)
        {
            string error = string.Empty;

            int count = 0;
            if (list != null)
            {
                count = list.Count();
            }

            if (count == 0)
            {
                error = string.Format("No {0} information returned", itemName);
            }
            else
            {
                if (count > 1)
                {
                    error = "More than one entry returned";
                }
                else
                {
                    T item = list.First();
                    if (tokenSelector(item) != token)
                    {
                        error = "Entry for other token returned";
                    }
                }
            }
            Assert(string.IsNullOrEmpty(error), error, "Check response");

        }



    }
}
