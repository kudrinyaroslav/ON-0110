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

namespace DUT.CameraWebService.Events10
{

    /// <summary>
    /// Class for Search Service tests
    /// </summary>
    public class PullPointSubscriptionServiceTest : Base.BaseServiceTest
    {

        #region Const

        protected override string ServiceName
        { 
            get 
            { 
                //Used in DUT to define service name for command
                return "PullPointSubscriptionService10"; 
            } 
        }

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int SetSynchronizationPoint = 0;
        private const int Seek = 1;
        private const int PullMessages = 2;
        private const int Renew = 3;
        private const int Unsubscribe = 4;
        private const int MaxCommands = 5;

        #endregion //Const

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public PullPointSubscriptionServiceTest(TestCommon testCommon)
            :base(testCommon)
        {                        
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors

        //***************************************************************************************

        #region Additional Parameters

        internal DateTime TakeTerminationTime(DateTime currenTime)
        {
            string flag;
            string resultString;

            TakeSpecialParameterFlag("PullMessages", PullMessages, "TerminationTime", "differance", out flag);
            if (flag == "true")
            {

                return currenTime.AddSeconds(Convert.ToInt32(TakeSpecialParameterSimple<string>("PullMessages", PullMessages, "TerminationTime")));
            }
            else
            {
                resultString = TakeSpecialParameterSimple<string>("PullMessages", PullMessages, "TerminationTime");
                if (resultString == null)
                {
                    return new DateTime();
                }
                else
                {
                    return System.Xml.XmlConvert.ToDateTime(resultString);
                }
            }
        }

        internal DateTime TakeCurrentTime()
        {
            string flag;
            string resultString;

            TakeSpecialParameterFlag("PullMessages", PullMessages, "CurrentTime", "type", out flag);

            switch (flag)
            {
                case "now":
                    {
                        return System.DateTime.UtcNow;
                    }
                case "value":
                    {
                        return System.Xml.XmlConvert.ToDateTime(TakeSpecialParameterSimple<string>("PullMessages", PullMessages, "CurrentTime"));
                    }
                case "nowDiff":
                    {
                        int timeDiff = Convert.ToInt32(TakeSpecialParameterSimple<string>("PullMessages", PullMessages, "CurrentTime"));
                        return System.DateTime.UtcNow.AddSeconds(timeDiff);
                    }
                default:
                    {
                        resultString = TakeSpecialParameterSimple<string>("PullMessages", PullMessages, "CurrentTime");
                        if (resultString == null)
                        {
                            return new DateTime();
                        }
                        else
                        {
                            return System.Xml.XmlConvert.ToDateTime(resultString);
                        }
                    }
            }
        }

        #endregion

        #region Commands

        internal NotificationMessageHolderType[] PullMessagesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            int special;
            int timeDiff;
            Dictionary<string,string> prefixes;
            NotificationMessageHolderType[] result;

            timeDiff = Convert.ToInt32(TakeSpecialParameterSimple<string>("PullMessages", PullMessages, "TimeShift"));
            prefixes = TakePrefixes("PullMessages", PullMessages);
            result =  GetCommand<NotificationMessageHolderType[]>("PullMessages", PullMessages, validationRequest, true, out stepType, out exc, out timeout, out special);

            switch (special)
            {
                case 1:
                    if (result.Count() != 0)
                    {
                        result[0].Message.Attributes["UtcTime"].Value = XmlConvert.ToString(System.DateTime.UtcNow.AddSeconds(timeDiff), XmlDateTimeSerializationMode.Utc);
                    }
                    break;
                default:
                    break;
            }


            foreach (NotificationMessageHolderType notification in result)
            {
                if (notification.Xmlns == null)
                {
                    notification.Xmlns = new XmlSerializerNamespaces();
                }
                foreach (string prefix in prefixes.Keys)
                {
                    notification.Xmlns.Add(prefix, prefixes[prefix]);
                }
            }

            return result;
        }

        internal void RenewTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("Renew", Renew, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void SetSynchronizationPointTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetSynchronizationPoint", SetSynchronizationPoint, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal UnsubscribeResponse UnsubscribeTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("Unsubscribe", Unsubscribe, validationRequest, true, out stepType, out exc, out timeout);
            return null;
        }

        internal void SeekTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("Seek", Seek, validationRequest, true, out stepType, out exc, out timeout);
        }

        #endregion;

    }
}
