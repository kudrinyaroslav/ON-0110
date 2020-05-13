using System;
using System.Collections.Generic;
using System.Linq;
using DUT.PACS.Simulator.ServiceSchedule10;
using DUT.PACS.Simulator.Common;
using System.Threading.Tasks;
using System.Threading;

namespace DUT.PACS.Simulator.ServiceSchedule10
{
    /// <summary>
    /// Summary description for Schedule Service
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ScheduleBinding", Namespace = "http://www.onvif.org/ver10/schedulerules/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public class ScheduleService : ScheduleServiceBinding
    {
        private Task _scheduleTask;
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override ServiceCapabilities GetServiceCapabilities()
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;
            return capabilities;
        }

        #region Schedule

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetScheduleState", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ScheduleState")]
        public override ScheduleState GetScheduleState(string Token)
        {
            return GetInfo(Token, (c) => c.ScheduleStateList);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetScheduleInfo", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ScheduleInfo")]
        public override ScheduleInfo[] GetScheduleInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;

            if (Token != null && Token.Length > capabilities.MaxLimit)
                FaultLib.ReturnFault("Too many items was requested. ", new[] { "Sender", "InvalidArgs", "TooManyItems" });


            return Array.ConvertAll(GetListByTokenList<Schedule>(Token, C => C.ScheduleList), item => ToScheduleInfo(item));
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetScheduleInfoList", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetScheduleInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("ScheduleInfo")] out ScheduleInfo[] ScheduleInfo)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;
            int offset = 0;
            if (!string.IsNullOrEmpty(StartReference))
                if (!Int32.TryParse(StartReference, out offset))
                    FaultLib.ReturnFault("Invalid StartReferense value. ", new[] { "Sender", "InvalidArgVal", "InvalidArgVal" });

            if (!LimitSpecified)
                Limit = capabilities.MaxLimit > int.MaxValue ?
                    int.MaxValue : (int)capabilities.MaxLimit;

            ScheduleInfo = Array.ConvertAll(GetList<Schedule>(offset, true, Limit, true, C => C.ScheduleList), item => ToScheduleInfo(item));
            string newStartReferense = null;
            if (offset + ScheduleInfo.Length < ConfStorage.ScheduleList.Count)
            {
                newStartReferense = Convert.ToString(offset + ScheduleInfo.Length);
            } return newStartReferense;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetSchedules", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Schedule")]
        public override Schedule[] GetSchedules([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;

            if (Token != null && Token.Length > capabilities.MaxLimit)
                FaultLib.ReturnFault("Too many items was requested. ", new[] { "Sender", "InvalidArgs", "TooManyItems" });
            return GetListByTokenList<Schedule>(Token, C => C.ScheduleList);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetScheduleList", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetScheduleList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("Schedule")] out Schedule[] Schedule)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;
            int offset = 0;
            if (!string.IsNullOrEmpty(StartReference))
                if (!Int32.TryParse(StartReference, out offset))
                    FaultLib.ReturnFault("Invalid StartReferense value. ", new[] { "Sender", "InvalidArgVal", "InvalidArgVal" });

            if (!LimitSpecified)
                Limit = capabilities.MaxLimit > int.MaxValue ?
                    int.MaxValue : (int)capabilities.MaxLimit;

            Schedule = GetList<Schedule>(offset, true, Limit, true, C => C.ScheduleList);
            string newStartReferense = null;
            if (offset + Schedule.Length < ConfStorage.ScheduleList.Count)
            {
                newStartReferense = Convert.ToString(offset + Schedule.Length);
            } return newStartReferense;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/CreateSchedule", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string CreateSchedule(Schedule Schedule)
        {
            ConfStorageLoad();
            EventServerLoad();

            if (Schedule.token == "")
            {
                int i = 1;

                Schedule.token = "schedule" + i.ToString();

                while (ConfStorage.ScheduleList.Keys.Contains(Schedule.token))
                {
                    Schedule.token = "schedule" + i.ToString();
                    i++;
                }
            }
            else
            {
                string message = string.Format("Not empty token.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgs" });
            }

            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;
            string res = Schedule.token;

            ICalendar standardICalendar = new ICalendar(Schedule.Standard);
            var standardICalendarVEvents = standardICalendar.GetVEvents();
            if (standardICalendarVEvents != null && standardICalendarVEvents.Count > capabilities.MaxTimePeriodsPerDay)
            {
                string message = string.Format("There are too many TimePeriods in a day schedule, see MaxTimePeriodsPerDay capability.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxTimePeriodsPerDay" });
            }

            //Check that there is no schedule with the same token exists
            if (ConfStorage.ScheduleList.ContainsKey(Schedule.token))
            {
                string message = string.Format("Schedule with token {0} aleady exist.", Schedule.token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
            }


            //Check MaxSchedules capability
            if (ConfStorage.ScheduleList.Count() >= capabilities.MaxSchedules)
            {
                string message = string.Format("There is not enough space to create new schedule, see the MaxSchedules capability.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Receiver", "CapabilityViolated", "MaxSchedules" });
            }


            if (Schedule.SpecialDays != null)
            {

                if (Schedule.SpecialDays.Count() >= capabilities.MaxSpecialDaysSchedules)
                {
                    string message = string.Format("There are too many SpecialDaysSchedule entities referred in this schedule, see MaxSpecialDays-Schedules capability.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxSpecialDaysSchedules" });
                }

                foreach (var specialDays in Schedule.SpecialDays)
                {
                    if (specialDays.TimeRange.Count() > capabilities.MaxTimePeriodsPerDay)
                    {
                        string message = string.Format("There are too many TimePeriods in a day schedule, see MaxTimePeriodsPerDay capability.");
                        LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxTimePeriodsPerDay" });
                    }
                    if (specialDays.TimeRange != null)
                    {
                        foreach (var timeRange in specialDays.TimeRange)
                        {
                            if (timeRange.Until < timeRange.From)
                            {
                                string message = string.Format("Schedule SpecialDayGroupToken.TimeRange.Until value is less that From value.");
                                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "ReferenceNotFound" });
                            }
                        }
                    }
                }

                //Check that all Schedules exists
                if (Schedule.SpecialDays.Any(C => !(ConfStorage.SpecialDayGroupList.Keys.Contains(C.GroupToken))))
                {
                    string message = string.Format("Schedule special day group token does not exist.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                }
            }

            ConfStorage.ScheduleList.Add(Schedule.token, Schedule);
            ScheduleState scheduleState = new ScheduleState();

            DateTime dateStart = standardICalendarVEvents.First().DateStart;
            DateTime dateEnd = standardICalendarVEvents.First().DateEnd;

            if (dateStart.ToFileTime() < DateTime.Now.ToFileTime() && dateEnd.ToFileTime() > DateTime.Now.ToFileTime())
            {
                scheduleState.Active = true;
            }
            else
            {
                scheduleState.Active = false;
            }

            scheduleState.SpecialDaySpecified = capabilities.SpecialDaysSupported;
            scheduleState.SpecialDay = IsSpecialDay(Schedule);

            ConfStorage.ScheduleStateList.Add(Schedule.token, scheduleState);

            EventServer.ConfigurationScheduleChangedEvent(this, Schedule.token);
            EventServer.ScheduleStateActiveEvent(this, "Initialized", Schedule.token, Schedule.Name, scheduleState.Active, scheduleState.SpecialDay);




            if (dateStart.ToFileTime() > DateTime.Now.ToFileTime() && standardICalendarVEvents.First().Rrule.Contains("DAILY"))
            {
                ScheduleTask(dateStart, dateEnd, Schedule, capabilities);
            }
            else if (dateEnd.ToFileTime() > DateTime.Now.ToFileTime())
            {
                ScheduleTask(DateTime.Now, dateEnd, Schedule, capabilities);
            }

            var specialDayGroupToken = string.Empty;
            if (Schedule.SpecialDays != null)
            {
                specialDayGroupToken = Schedule.SpecialDays.First().GroupToken;
            }

            if (!string.IsNullOrEmpty(specialDayGroupToken))
            {

                if (ConfStorage.SpecialDayGroupList.ContainsKey(specialDayGroupToken))
                {
                    var specialDayGroup = ConfStorage.SpecialDayGroupList[specialDayGroupToken];

                    ICalendar specialDayGroupICalendar = new ICalendar(specialDayGroup.Days);
                    var specialDayGroupICalendarVEvents = specialDayGroupICalendar.GetVEvents();

                    DateTime dateSpecialDayStart = specialDayGroupICalendarVEvents.First().DateStart;
                    DateTime dateSpecialDayEnd = specialDayGroupICalendarVEvents.First().DateEnd;

                    if (string.IsNullOrEmpty(specialDayGroupICalendarVEvents.First().Rrule) || specialDayGroupICalendarVEvents.First().Rrule.Contains("DAILY"))
                    {
                        if (dateSpecialDayStart.ToFileTime() > DateTime.Now.ToFileTime())
                        {
                            ScheduleTask(dateSpecialDayStart, dateSpecialDayEnd, Schedule, capabilities);

                        }
                        else if (dateSpecialDayEnd.ToFileTime() > DateTime.Now.ToFileTime())
                        {
                            ScheduleTask(DateTime.Now, dateSpecialDayEnd, Schedule, capabilities);
                        }
                    }
                }
            }

            EventServerSave();
            ConfStorageSave();

            return res;
        }

        public void ScheduleTask(DateTime dateStart, DateTime dateEnd, Schedule schedule, ServiceCapabilities capabilities)
        {
            var startCancelSource = new CancellationTokenSource();
            CancellationToken startCancel = startCancelSource.Token;
            var delayStart = (int)dateStart.Subtract(DateTime.Now).TotalMilliseconds;
            var delayEnd = (int)dateEnd.Subtract(DateTime.Now).TotalMilliseconds;
            if (delayStart < 1000)
            {
                dateStart = dateStart.AddSeconds(1);
                dateEnd = dateEnd.AddSeconds(1);
            }
            if (dateEnd.Subtract(dateStart).TotalMilliseconds < 1000)
            {
                dateEnd = dateEnd.AddSeconds(1);
            }

            Task taskStart = Task.Factory.StartNew(() => ScheduleAction(() =>
            {
                ConfStorageLoad();
                EventServerLoad();
                if (ConfStorage.ScheduleList.ContainsKey(schedule.token))
                {
                    var specialDay = IsSpecialDay(schedule);

                    EventServer.ScheduleStateActiveEvent(this, "Changed", schedule.token, schedule.Name, true, specialDay);
                    ScheduleState state = new ScheduleState() { Active = true, SpecialDaySpecified = capabilities.SpecialDaysSupported, SpecialDay = specialDay };
                    if (!ConfStorage.ScheduleStateList.ContainsKey(schedule.token))
                    {
                        ConfStorage.ScheduleStateList.Add(schedule.token, state);
                    }
                    else
                    {
                        ConfStorage.ScheduleStateList[schedule.token] = state;
                    }
                }

                EventServerSave();
                ConfStorageSave();

            }, dateStart),
            startCancel);

            if (!ConfStorage.AwaitingTasks.ContainsKey(schedule.token))
            {
                ConfStorage.AwaitingTasks[schedule.token] = new List<CancellationTokenSource>();
            }
            ConfStorage.AwaitingTasks[schedule.token].Add(startCancelSource);

            var endCancelSource = new CancellationTokenSource();
            CancellationToken endCancel = endCancelSource.Token;

            Task taskEnd = Task.Factory.StartNew(() => ScheduleAction(() =>
            {
                ConfStorageLoad();
                EventServerLoad();

                var specialDay = IsSpecialDay(schedule);

                EventServer.ScheduleStateActiveEvent(this, "Changed", schedule.token, schedule.Name, false, specialDay);
                ScheduleState state = new ScheduleState() { Active = false, SpecialDaySpecified = capabilities.SpecialDaysSupported, SpecialDay = specialDay };
                if (!ConfStorage.ScheduleStateList.ContainsKey(schedule.token))
                {
                    ConfStorage.ScheduleStateList.Add(schedule.token, state);
                }
                else
                {
                    ConfStorage.ScheduleStateList[schedule.token] = state;
                }

                EventServerSave();
                ConfStorageSave();

            }, dateEnd),
            endCancel);
            ConfStorage.AwaitingTasks[schedule.token].Add(endCancelSource);
        }

        public bool IsSpecialDay(Schedule schedule)
        {
            var specialDay = schedule.SpecialDays != null && schedule.SpecialDays.Count() > 0;
            if (specialDay)
            {
                var specialDayGroupToken = schedule.SpecialDays.First().GroupToken;
                if (ConfStorage.SpecialDayGroupList.ContainsKey(specialDayGroupToken))
                {
                    var specialDayGroup = ConfStorage.SpecialDayGroupList[specialDayGroupToken];

                    ICalendar specialDayGroupICalendar = new ICalendar(specialDayGroup.Days);
                    var specialDayGroupICalendarVEvents = specialDayGroupICalendar.GetVEvents();

                    DateTime dateSpecialDayStart = specialDayGroupICalendarVEvents.First().DateStart;
                    DateTime dateSpecialDayEnd = specialDayGroupICalendarVEvents.First().DateEnd;

                    if (dateSpecialDayStart.ToFileTime() < DateTime.Now.ToFileTime() && dateSpecialDayEnd.ToFileTime() > DateTime.Now.ToFileTime())
                    {
                        specialDay = true;
                    }
                    else
                    {
                        specialDay = false;
                    }

                }
            }
            return specialDay;
        }

        public static async void ScheduleAction(Action action, DateTime ExecutionTime)
        {
            var delay = (int)ExecutionTime.Subtract(DateTime.Now).TotalMilliseconds;
            await Task.Delay(delay);
            action();
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/ModifySchedule", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ModifySchedule(Schedule Schedule)
        {
            ConfStorageLoad();
            EventServerLoad();

            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;

            ICalendar standardICalendar = new ICalendar(Schedule.Standard);
            var standardICalendarVEvents = standardICalendar.GetVEvents();
            if (standardICalendarVEvents != null && standardICalendarVEvents.Count > capabilities.MaxTimePeriodsPerDay)
            {
                string message = string.Format("There are too many TimePeriods in a day schedule, see MaxTimePeriodsPerDay capability.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxTimePeriodsPerDay" });
            }

            //Check that schedule exists
            if (!ConfStorage.ScheduleList.ContainsKey(Schedule.token))
            {
                string message = string.Format("Schedule with specified token {0} does not exists.", Schedule.token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            if (Schedule.SpecialDays != null)
            {
                if (Schedule.SpecialDays.Count() >= capabilities.MaxSpecialDaysSchedules)
                {
                    string message = string.Format("There are too many SpecialDaysSchedule entities referred in this schedule, see MaxSpecialDays-Schedules capability.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxSpecialDaysSchedules" });
                }

                foreach (var specialDays in Schedule.SpecialDays)
                {
                    if (specialDays.TimeRange.Count() >= capabilities.MaxTimePeriodsPerDay)
                    {
                        string message = string.Format("There are too many TimePeriods in a day schedule, see MaxTimePeriodsPerDay capability.");
                        LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxTimePeriodsPerDay" });
                    }
                    if (specialDays.TimeRange != null)
                    {
                        foreach (var timeRange in specialDays.TimeRange)
                        {
                            if (timeRange.Until < timeRange.From)
                            {
                                string message = string.Format("Schedule SpecialDayGroupToken.TimeRange.Until value is less that From value.");
                                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "ReferenceNotFound" });
                            }
                        }
                    }
                }

                //Check that all Schedules special days exists
                if (Schedule.SpecialDays.Any(C => !(ConfStorage.SpecialDayGroupList.Keys.Contains(C.GroupToken))))
                {
                    string message = string.Format("Schedule special day group token does not exist.");
                    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                }
            }

            ConfStorage.ScheduleList.Remove(Schedule.token);
            ConfStorage.ScheduleList.Add(Schedule.token, Schedule);

            EventServer.ConfigurationScheduleChangedEvent(this, Schedule.token);

            EventServerSave();
            ConfStorageSave();

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/DeleteSchedule", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteSchedule(string Token)
        {
            ConfStorageLoad();
            EventServerLoad();

            if (ConfStorage.AwaitingTasks.ContainsKey(Token))
            {
                foreach (var task in ConfStorage.AwaitingTasks[Token])
                {
                    task.Cancel();
                }
            }
            ConfStorage.AwaitingTasks.Remove(Token);

            if (ConfStorage.ScheduleList.ContainsKey(Token))
            {
                EventServer.ScheduleStateActiveEvent(this, "Removed", Token, ConfStorage.ScheduleList[Token].Name, ConfStorage.ScheduleStateList[Token].Active, ConfStorage.ScheduleStateList[Token].SpecialDay);
                EventServer.ConfigurationScheduleRemovedEvent(this, Token);
                ConfStorage.ScheduleList.Remove(Token);
                ConfStorage.ScheduleStateList.Remove(Token);
                LoggingService.LogMessage(string.Format("Schedule with token '{0}' was deleted.", Token), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
            }
            else
            {
                string message = string.Format("Schedule with token {0} does not exist", Token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            if (ConfStorage.ScheduleStateList.ContainsKey(Token))
            {
                ConfStorage.ScheduleStateList.Remove(Token);
            }

            EventServerSave();
            ConfStorageSave();
        }

        #endregion Schedule

        #region SpecialDayGroup

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroupInfo", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SpecialDayGroupInfo")]
        public override SpecialDayGroupInfo[] GetSpecialDayGroupInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;

            if (Token != null && Token.Length > capabilities.MaxLimit)
                FaultLib.ReturnFault("Too many items was requested. ", new[] { "Sender", "InvalidArgs", "TooManyItems" });


            return Array.ConvertAll(GetListByTokenList<SpecialDayGroup>(Token, C => C.SpecialDayGroupList), item => ToSpecialDayGroupInfo(item));
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroupInfoList", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetSpecialDayGroupInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("SpecialDayGroupInfo")] out SpecialDayGroupInfo[] SpecialDayGroupInfo)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;
            int offset = 0;
            if (!string.IsNullOrEmpty(StartReference))
                if (!Int32.TryParse(StartReference, out offset))
                    FaultLib.ReturnFault("Invalid StartReferense value. ", new[] { "Sender", "InvalidArgVal", "InvalidArgVal" });

            if (!LimitSpecified)
                Limit = capabilities.MaxLimit > int.MaxValue ?
                    int.MaxValue : (int)capabilities.MaxLimit;

            SpecialDayGroupInfo = Array.ConvertAll(GetList<SpecialDayGroup>(offset, true, Limit, true, C => C.SpecialDayGroupList), item => ToSpecialDayGroupInfo(item));
            string newStartReferense = null;
            if (offset + SpecialDayGroupInfo.Length < ConfStorage.SpecialDayGroupList.Count)
            {
                newStartReferense = Convert.ToString(offset + SpecialDayGroupInfo.Length);
            } return newStartReferense;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroups", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SpecialDayGroup")]
        public override SpecialDayGroup[] GetSpecialDayGroups([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;

            if (Token != null && Token.Length > capabilities.MaxLimit)
                FaultLib.ReturnFault("Too many items was requested. ", new[] { "Sender", "InvalidArgs", "TooManyItems" });
            return GetListByTokenList<SpecialDayGroup>(Token, C => C.SpecialDayGroupList);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroupList", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetSpecialDayGroupList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("SpecialDayGroup")] out SpecialDayGroup[] SpecialDayGroup)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;
            int offset = 0;
            if (!string.IsNullOrEmpty(StartReference))
                if (!Int32.TryParse(StartReference, out offset))
                    FaultLib.ReturnFault("Invalid StartReferense value. ", new[] { "Sender", "InvalidArgVal", "InvalidArgVal" });

            if (!LimitSpecified)
                Limit = capabilities.MaxLimit > int.MaxValue ?
                    int.MaxValue : (int)capabilities.MaxLimit;

            SpecialDayGroup = GetList<SpecialDayGroup>(offset, true, Limit, true, C => C.SpecialDayGroupList);
            string newStartReferense = null;
            if (offset + SpecialDayGroup.Length < ConfStorage.SpecialDayGroupList.Count)
            {
                newStartReferense = Convert.ToString(offset + SpecialDayGroup.Length);
            } return newStartReferense;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/CreateSpecialDayGroup", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string CreateSpecialDayGroup(SpecialDayGroup SpecialDayGroup)
        {
            ConfStorageLoad();
            EventServerLoad();

            if (SpecialDayGroup.token == "")
            {
                int i = 1;

                SpecialDayGroup.token = "specialdaygroup" + i.ToString();

                while (ConfStorage.SpecialDayGroupList.Keys.Contains(SpecialDayGroup.token))
                {
                    SpecialDayGroup.token = "specialdaygroup" + i.ToString();
                    i++;
                }
            }
            else
            {
                string message = string.Format("Not empty token.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgs" });
            }

            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;
            string res = SpecialDayGroup.token;

            //Check that there is no schedule with the same token exists
            if (ConfStorage.SpecialDayGroupList.ContainsKey(SpecialDayGroup.token))
            {
                string message = string.Format("SpecialDayGroup with token {0} aleady exist.", SpecialDayGroup.token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
            }

            //Check MaxSpecialDayGroups capability
            if (ConfStorage.SpecialDayGroupList.Count() >= capabilities.MaxSpecialDayGroups)
            {
                string message = string.Format("There is not enough space to create new SpecialDayGroup, see the MaxSpecialDayGroups capability.");
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Receiver", "CapabilityViolated", "MaxSpecialDayGroups" });
            }

            //TODO: Check MaxSpecialDaysInSpecialDayGroup capability
            //if (ConfStorage.ScheduleList.Values.SelectMany(schedule => schedule.SpecialDays).Count() >= capabilities.MaxSpecialDaysInSpecialDayGroup)
            //{
            //    string message = string.Format("There is not enough space to create new SpecialDayGroup, see the MaxSpecialDaysInSpecialDayGroup capability.");
            //    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
            //    FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxSpecialDaysInSpecialDayGroup" });
            //}

            ConfStorage.SpecialDayGroupList.Add(SpecialDayGroup.token, SpecialDayGroup);

            EventServer.ConfigurationSpecialDayGroupChangedEvent(this, SpecialDayGroup.token);

            EventServerSave();
            ConfStorageSave();


            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/ModifySpecialDayGroup", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ModifySpecialDayGroup(SpecialDayGroup SpecialDayGroup)
        {
            ConfStorageLoad();
            EventServerLoad();

            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.ScheduleCapabilities;

            //Check that Special Day Group exists
            if (!ConfStorage.SpecialDayGroupList.ContainsKey(SpecialDayGroup.token))
            {
                string message = string.Format("SpecialDayGroup with specified token {0} does not exists.", SpecialDayGroup.token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            //TODO: Check MaxSpecialDaysInSpecialDayGroup capability
            //if (ConfStorage.ScheduleList.Values.SelectMany(schedule => schedule.SpecialDays).Count() >= capabilities.MaxSpecialDaysInSpecialDayGroup)
            //{
            //    string message = string.Format("There is not enough space to create new SpecialDayGroup, see the MaxSpecialDaysInSpecialDayGroup capability.");
            //    LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
            //    FaultLib.ReturnFault(message, new string[] { "Sender", "CapabilityViolated", "MaxSpecialDaysInSpecialDayGroup" });
            //}


            ConfStorage.SpecialDayGroupList.Remove(SpecialDayGroup.token);
            ConfStorage.SpecialDayGroupList.Add(SpecialDayGroup.token, SpecialDayGroup);

            EventServer.ConfigurationSpecialDayGroupChangedEvent(this, SpecialDayGroup.token);

            EventServerSave();
            ConfStorageSave();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/DeleteSpecialDayGroup", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteSpecialDayGroup(string Token)
        {
            ConfStorageLoad();
            EventServerLoad();

            if (ConfStorage.SpecialDayGroupList.ContainsKey(Token))
            {
                ConfStorage.SpecialDayGroupList.Remove(Token);
                EventServer.ConfigurationSpecialDayGroupRemovedEvent(this, Token);
                LoggingService.LogMessage(string.Format("SpecialDayGroup with token '{0}' was deleted.", Token), DUT.PACS.Simulator.ExternalLogging.MessageType.Message);
            }
            else
            {
                string message = string.Format("SpecialDayGroup with token {0} does not exist", Token);
                LoggingService.LogMessage(message, DUT.PACS.Simulator.ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            EventServerSave();
            ConfStorageSave();
        }

        #endregion SpecialDayGroup

        #region Utils

        public static ScheduleInfo ToScheduleInfo(Schedule schedule)
        {
            ScheduleInfo scheduleInfo = new ScheduleInfo();

            scheduleInfo.token = schedule.token;
            scheduleInfo.Description = schedule.Description;
            scheduleInfo.Name = schedule.Name;

            return scheduleInfo;
        }

        public static SpecialDayGroupInfo ToSpecialDayGroupInfo(SpecialDayGroup specialDayGroup)
        {
            SpecialDayGroupInfo specialDayGroupInfo = new SpecialDayGroupInfo();

            specialDayGroupInfo.token = specialDayGroup.token;
            specialDayGroupInfo.Description = specialDayGroup.Description;
            specialDayGroupInfo.Name = specialDayGroup.Name;

            return specialDayGroupInfo;
        }

        public void SynchronizationPoint()
        {
            ConfStorageLoad();
            EventServerLoad();

            foreach (var schedule in ConfStorage.ScheduleList.Values)
            {
                EventServer.ScheduleStateActiveEvent(this, "Initialized", schedule.token, schedule.Name, ConfStorage.ScheduleStateList[schedule.token].Active, ConfStorage.ScheduleStateList[schedule.token].SpecialDay);
            }

            EventServerSave();
            ConfStorageSave();
        }

        #endregion
    }
}
