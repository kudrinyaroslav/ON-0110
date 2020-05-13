using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.IO;
using System.Xml.Serialization;
using DUT.CameraWebService;
using CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.ServiceSchedule10
{



    /// <summary>
    /// Class for Search Service tests
    /// </summary>
    public class ScheduleServiceTest : Base.BaseServiceTest
    {

        #region Members

        string m_schedule_token;
        string m_schedule_Standard;
        DateTime? m_schedule_SpecialDays0_TimeRange0_From;
        DateTime? m_schedule_SpecialDays0_TimeRange0_Until;

        string m_specialDayGroup_token;
        string m_specialDayGroup_Days;

        #endregion

        #region Const

        protected override string ServiceName { get { return "Schedule10"; } }

        /// <summary>
        /// Constant for Command index
        /// </summary>                

        private const int GetServiceCapabilities = 1;
        private const int GetScheduleState = 2;
        private const int GetScheduleInfo = 3;
        private const int GetScheduleInfoList = 4;
        private const int GetSchedules = 5;
        private const int GetScheduleList = 6;
        private const int CreateSchedule = 7;
        private const int ModifySchedule = 8;
        private const int DeleteSchedule = 9;
        private const int GetSpecialDayGroupInfo = 10;
        private const int GetSpecialDayGroupInfoList = 11;
        private const int GetSpecialDayGroups = 12;
        private const int GetSpecialDayGroupList = 13;
        private const int CreateSpecialDayGroup = 14;
        private const int ModifySpecialDayGroup = 15;
        private const int DeleteSpecialDayGroup = 16;
        private const int MaxCommands = 17;


        #endregion //Const


        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ScheduleServiceTest(TestCommon testCommon)
            : base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors

        //***************************************************************************************

        #region Helpers

        void ScheduleFromCreateModifyUpdate(int special, ref Schedule[] res)
        {
            if (special != 0)
            {
                var schedule = res.First(rule => rule.token == m_schedule_token);
                const int MIN_COUNT = 7;

                if ((special == 1) || (special == 2))
                {
                    var stringArray = m_schedule_Standard.Split(new char[] { '\n' });
                    if (stringArray.Count() > MIN_COUNT)
                    {
                        string strHelper;
                        for (int i = 4; i < (stringArray.Count() - 4)/2 + 2; i++)
                        {
                            strHelper = stringArray[i];
                            stringArray[i] = stringArray[stringArray.Count() - i - 1];
                            stringArray[stringArray.Count() - i - 1] = strHelper;
                        }
                    }
                    schedule.Standard = String.Join("\n", stringArray);
                    
                }

                if ((special == 4) )
                {
                    var stringArray = m_schedule_Standard.Split(new char[] { '\n' });
                    if (stringArray.Count() > MIN_COUNT)
                    {
                        string strHelper;
                        for (int i = 4; i < (stringArray.Count() - 4) / 2 + 2; i++)
                        {
                            strHelper = stringArray[i];
                            stringArray[i] = stringArray[stringArray.Count() - i + 1];
                            stringArray[stringArray.Count() - i + 1] = strHelper;
                        }
                    }
                    schedule.Standard = String.Join("\n", stringArray);

                }
                String oldGuid = schedule.Standard.Substring(schedule.Standard.IndexOf("UID"));
                oldGuid = oldGuid.Remove(oldGuid.IndexOf("\n"));
                schedule.Standard = schedule.Standard.Replace(oldGuid, "UID:" + Guid.NewGuid().ToString());

                if ((special == 1) || (special == 3)|| (special == 4))
                {

                    if ((schedule.SpecialDays != null) && (schedule.SpecialDays.Count() != 0))
                    {
                        if ((schedule.SpecialDays[0].TimeRange != null) && (schedule.SpecialDays[0].TimeRange.Count() != 0))
                        {
                            if (m_schedule_SpecialDays0_TimeRange0_From != null)
                            {
                                schedule.SpecialDays[0].TimeRange[0].From = (DateTime)m_schedule_SpecialDays0_TimeRange0_From;
                            }

                            if (m_schedule_SpecialDays0_TimeRange0_Until != null)
                            {
                                if (schedule.SpecialDays[0].TimeRange[0].UntilSpecified)
                                {
                                    schedule.SpecialDays[0].TimeRange[0].Until = (DateTime)m_schedule_SpecialDays0_TimeRange0_Until;
                                }
                            }
                        }
                    }
                }
            }

        }

        void ScheduleFromCreateModifyRemember(ParametersValidation validationRequest, string scheduleToken = "") // for CreateSchedule set m_schedule_token = scheduleToken (ValidationRules parameterName string is empty for this case)
        {
            if (validationRequest.ValidationRules.Any(rule => rule.ParameterName == "SpecialDays/TimeRange/From"))
            {
                m_schedule_SpecialDays0_TimeRange0_From = (DateTime)validationRequest.ValidationRules.First(rule => rule.ParameterName == "SpecialDays/TimeRange/From").Value;
            }
            else
            {
                m_schedule_SpecialDays0_TimeRange0_From = null;
            }

            if (validationRequest.ValidationRules.Any(rule => rule.ParameterName == "SpecialDays/TimeRange/Until"))
            {
                m_schedule_SpecialDays0_TimeRange0_Until = (DateTime)validationRequest.ValidationRules.First(rule => rule.ParameterName == "SpecialDays/TimeRange/Until").Value;
            }
            else
            {
                m_schedule_SpecialDays0_TimeRange0_Until = null;
            }

            m_schedule_token = string.IsNullOrEmpty(scheduleToken) ? (string)validationRequest.ValidationRules.First(rule => rule.ParameterName == "token").Value : scheduleToken;

            m_schedule_Standard = (string)validationRequest.ValidationRules.First(rule => rule.ParameterName == "Standard").Value;

        }

        void SpecialDayGroupFromCreateModifyRemember(ParametersValidation validationRequest, string specialDayGroup_token = "") // for CreateSchedule set m_schedule_token = scheduleToken (ValidationRules parameterName string is empty for this case)        {
        {
            m_specialDayGroup_token = string.IsNullOrEmpty(specialDayGroup_token) ? (string)validationRequest.ValidationRules.First(rule => rule.ParameterName == "token").Value : specialDayGroup_token;
            m_specialDayGroup_Days = (string)validationRequest.ValidationRules.First(rule => rule.ParameterName == "Days").Value;
        }

        void SpecialDayGroupFromCreateModifyUpdate(int special, ref SpecialDayGroup[] res)
        {
            if (special != 0)
            {
                var specialDayGroup = res.First(rule => rule.token == m_specialDayGroup_token);

                if (special == 1)
                {
                    var stringArray = m_specialDayGroup_Days.Split(new char[] { '\n' });
                    if (stringArray.Count() > 7)
                    {
                        string strHelper;
                        for (int i = 4; i < (stringArray.Count() - 4) / 2 + 2; i++)
                        {
                            strHelper = stringArray[i];
                            stringArray[i] = stringArray[stringArray.Count() - i - 1];
                            stringArray[stringArray.Count() - i - 1] = strHelper;
                        }
                    }
                    specialDayGroup.Days = String.Join("\n", stringArray);
                    

                }
                if (special == 2)
                {
                    var stringArray = m_specialDayGroup_Days.Split(new char[] { '\n' });
                    if (stringArray.Count() > 7)
                    {
                        string strHelper;
                        for (int i = 4; i < (stringArray.Count() - 4) / 2 + 2; i++)
                        {
                            strHelper = stringArray[i];
                            stringArray[i] = stringArray[stringArray.Count() - i + 1];
                            stringArray[stringArray.Count() - i + 1] = strHelper;
                        }
                    }
                    specialDayGroup.Days = String.Join("\n", stringArray);

                }
                String oldGuid = specialDayGroup.Days.Substring(specialDayGroup.Days.IndexOf("UID"));
                oldGuid = oldGuid.Remove(oldGuid.IndexOf("\n"));
                specialDayGroup.Days = specialDayGroup.Days.Replace(oldGuid, "UID:"+ Guid.NewGuid().ToString());
            }

        }
        #endregion


        //***************************************************************************************

        #region General

        internal ServiceCapabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int timeout)
        {
            return GetCommand<ServiceCapabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, out stepType, out ex, out timeout);
        }

        #endregion //General

        //***************************************************************************************


        //***************************************************************************************



        internal ScheduleInfo[] TakeScheduleInfoList()
        {
            return TakeSpecialParameter<ScheduleInfo[]>("GetScheduleInfoList", GetScheduleInfoList, "ArrayOfScheduleInfo");
        }

        internal Schedule[] TakeScheduleList()
        {
            string specialStr;
            int special = 0;

            var res = TakeSpecialParameter<Schedule[]>("GetScheduleList", GetScheduleList, "ArrayOfSchedule", "special", out specialStr);

            if (specialStr != "")
            {
                special = Convert.ToInt16(specialStr);
            }

            ScheduleFromCreateModifyUpdate(special, ref res);

            return res;
        }

        internal SpecialDayGroupInfo[] TakeSpecialDayGroupInfoList()
        {
            return TakeSpecialParameter<SpecialDayGroupInfo[]>("GetSpecialDayGroupInfoList", GetSpecialDayGroupInfoList, "ArrayOfSpecialDayGroupInfo");
        }

        internal SpecialDayGroup[] TakeSpecialDayGroupList()
        {
            string specialStr;
            int special = 0;

            var res = TakeSpecialParameter<SpecialDayGroup[]>("GetSpecialDayGroupList", GetSpecialDayGroupList, "ArrayOfSpecialDayGroup", "special", out specialStr);

            if (specialStr != "")
            {
                special = Convert.ToInt16(specialStr);
            }

            SpecialDayGroupFromCreateModifyUpdate(special, ref res);

            return res;
        }

        internal ScheduleState GetScheduleStateTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<ScheduleState>("GetScheduleState", GetScheduleState, validationRequest, out stepType, out exc, out timeout);
        }

        internal ScheduleInfo[] GetScheduleInfoTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<ScheduleInfo[]>("GetScheduleInfo", GetScheduleInfo, validationRequest, out stepType, out exc, out timeout);
        }

        internal string GetScheduleInfoListTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("GetScheduleInfoList", GetScheduleInfoList, validationRequest, out stepType, out exc, out timeout);
        }

        internal Schedule[] GetSchedulesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;

            var res = GetCommand<Schedule[]>("GetSchedules", GetSchedules, validationRequest, out stepType, out exc, out timeout, out special);

            ScheduleFromCreateModifyUpdate(special, ref res);

            return res;
        }

        internal string GetScheduleListTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("GetScheduleList", GetScheduleList, validationRequest, out stepType, out exc, out timeout);
        }

        internal string CreateScheduleTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            var res = GetCommand<string>("CreateSchedule", CreateSchedule, validationRequest, out stepType, out exc, out timeout);

            ScheduleFromCreateModifyRemember(validationRequest, res);

            return res;
        }

        internal void ModifyScheduleTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("ModifySchedule", ModifySchedule, validationRequest, out stepType, out exc, out timeout);

            ScheduleFromCreateModifyRemember(validationRequest);
        }

        internal void DeleteScheduleTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeleteSchedule", DeleteSchedule, validationRequest, out stepType, out exc, out timeout);
        }

        internal SpecialDayGroupInfo[] GetSpecialDayGroupInfoTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<SpecialDayGroupInfo[]>("GetSpecialDayGroupInfo", GetSpecialDayGroupInfo, validationRequest, out stepType, out exc, out timeout);
        }

        internal string GetSpecialDayGroupInfoListTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("GetSpecialDayGroupInfoList", GetSpecialDayGroupInfoList, validationRequest, out stepType, out exc, out timeout);
        }

        internal SpecialDayGroup[] GetSpecialDayGroupsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;

            var res = GetCommand<SpecialDayGroup[]>("GetSpecialDayGroups", GetSpecialDayGroups, validationRequest, out stepType, out exc, out timeout, out special);

            SpecialDayGroupFromCreateModifyUpdate(special, ref res);

            return res;
        }

        internal string GetSpecialDayGroupListTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("GetSpecialDayGroupList", GetSpecialDayGroupList, validationRequest, out stepType, out exc, out timeout);
        }

        internal string CreateSpecialDayGroupTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            var res = GetCommand<string>("CreateSpecialDayGroup", CreateSpecialDayGroup, validationRequest, out stepType, out exc, out timeout);

            SpecialDayGroupFromCreateModifyRemember(validationRequest, res);

            return res;
        }

        internal void ModifySpecialDayGroupTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("ModifySpecialDayGroup", ModifySpecialDayGroup, validationRequest, out stepType, out exc, out timeout);

            SpecialDayGroupFromCreateModifyRemember(validationRequest);
        }

        internal void DeleteSpecialDayGroupTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeleteSpecialDayGroup", DeleteSpecialDayGroup, validationRequest, out stepType, out exc, out timeout);
        }
    }
}
