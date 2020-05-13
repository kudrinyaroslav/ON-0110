using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml.Serialization;
using DUT.CameraWebService;

namespace DUT.CameraWebService.Events10
{
    public class EventServiceTest
    {
        #region Const

        private const string ServiceName = "Event10";

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int CreatePullPointSubscription = 0;
        private const int SMSUnsubscribe = 1;
        private const int GetEventProperties = 2;
        private const int PMSPullMessages = 3;
        private const int PMSSetSynchronizationPoint = 4;
        private const int SMSRenew = 5;
        private const int Subscribe = 6;
        private const int GetServiceCapabilities = 7;
        private const int SMSSeek = 8;
        private const int MaxCommands = 9;


        #endregion //Const

        #region Members

        /// <summary>
        /// Mass with command call count
        /// </summary>
        public int[] m_commandCount = new int[MaxCommands];

        TestCommon m_TestCommon = null;

        #endregion //Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public EventServiceTest(TestCommon testCommon)
        {
            for (int i = 0; i < m_commandCount.Length; i++)
            {
                m_commandCount[i] = 0;
            }

            m_TestCommon = testCommon;
        }

        #endregion //Constructors

        #region General

        public string LastNotificationAddress = "";

        public void ResetTestSuit()
        {
            for (int i = 0; i < m_commandCount.Length; i++)
            {
                m_commandCount[i] = 0;
            }
        }

        /// <summary>
        /// Return incremented target if it is not more than maxValue. Return 0 in other case.
        /// </summary>
        /// <param name="maxValue">Max value for target</param>
        /// <param name="teaget">Incremented value</param>
        /// <returns>Changed target</returns>
        public void Increment(int maxValue, int index)
        {
            if (maxValue - 1 <= m_commandCount[index])
            {
                m_commandCount[index] = 0;
            }
            else
            {
                m_commandCount[index]++;
            }
        }

        #endregion //General

        internal StepType CreatePullPointSubscriptionTest(out EndpointReferenceType target, out System.DateTime CurrentTime, out System.DateTime? TerminationTime, out SoapException ex, out int timeOut, FilterType Filter, string InitialTerminationTime, CreatePullPointSubscriptionSubscriptionPolicy SubscriptionPolicy, XmlElement[] Any)
        {
            StepType res = StepType.None;
            target = new EndpointReferenceType();
            timeOut = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            CurrentTime = System.DateTime.UtcNow;
            TerminationTime = CurrentTime.AddSeconds(10);

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("CreatePullPointSubscription");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[CreatePullPointSubscription]];

                #region Analyze request

                //InitialTerminationTime
                CommonCompare.StringCompare("RequestParameters/InitialTerminationTime", "InitialTerminationTime", InitialTerminationTime, ref logMessage, ref passed, test);

                //Filter
                if (Filter != null)
                {
                    if (test.SelectSingleNode("RequestParameters/Filter") != null)
                    {
                        if (test.SelectSingleNode("RequestParameters/Filter[@nocomp=\"true\"]") == null)
                        {
                            string XMLString = "";
                            foreach (XmlElement i in Filter.Any)
                            {
                                XMLString = XMLString + i.OuterXml;
                            }
                            CommonCompare.StringCompare("RequestParameters/Filter", "Filter", XMLString, ref logMessage, ref passed, test);
                        }
                    }
                    else
                    {
                        passed = false;
                        logMessage = logMessage + "Unexpected Filter.";
                    }
                }
                else
                {
                    if (test.SelectSingleNode("RequestParameters/Filter") != null)
                    {
                        passed = false;
                        logMessage = logMessage + "No Filter.";
                    }
                }

                #endregion //Analyze request

                //Generate response
                int useRealAddress = 0;
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoidSpecial(test, out targetObj, out ex, out timeOut, typeof(EndpointReferenceType), out useRealAddress);
                target = (EndpointReferenceType)targetObj;

                //Get real path to service
                if (useRealAddress != 0)
                {
                    if (target.Address != null)
                    {
                        switch (useRealAddress)
                        {
                            case 1:
                                target.Address.Value = m_TestCommon.SubscriptionManagerServiceUri;
                                break;
                            case 2:
                                target.Address.Value = m_TestCommon.PullpointSubscriptionServiceUri;
                                break;
                                case 3:
                                target.Address.Value = m_TestCommon.PullpointSubscriptionService2Uri;
                                break;
                        }
                    }
                }

                #region Serialization Temp
                //Events.EndpointReferenceType dsr = new Events.EndpointReferenceType();
                //dsr.Address = new Events.AttributedURIType();
                //dsr.Address.Value = "http://192.168.10.203/onvif/event";
                //dsr.Metadata = new Events.MetadataType();
                //dsr.ReferenceParameters = new Events.ReferenceParametersType();
                //XmlSerializer serializer1 = new XmlSerializer(typeof(Events.EndpointReferenceType));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer1.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                if (res == StepType.Normal)
                {
                    //CurrentTime
                    string CurrentTimeType = test.SelectSingleNode("ResponseParametersAdditional/CurrentTime/@type").InnerText;
                    switch (CurrentTimeType)
                    {
                        case "now":
                            {
                                CurrentTime = System.DateTime.UtcNow;
                                break;
                            }
                        case "value":
                            {
                                CurrentTime = Convert.ToDateTime(test.SelectSingleNode("ResponseParametersAdditional/CurrentTime").InnerText);
                                break;
                            }
                        case "nowDiff":
                            {
                                int timeDiff = Convert.ToInt32(test.SelectSingleNode("ResponseParametersAdditional/CurrentTime").InnerText);
                                CurrentTime = System.DateTime.UtcNow.AddSeconds(timeDiff);
                                break;
                            }
                    }

                    //TerminationTime
                    if (test.SelectNodes("ResponseParametersAdditional/TerminationTime[@nil=\"true\"]").Count != 0)
                    {
                        TerminationTime = null;
                    }
                    else
                    {
                        if (test.SelectNodes("ResponseParametersAdditional/TerminationTime[@differance=\"true\"]").Count != 0)
                        {
                            int timeDiff = Convert.ToInt32(test.SelectSingleNode("ResponseParametersAdditional/TerminationTime").InnerText);
                            TerminationTime = CurrentTime.AddSeconds(timeDiff);
                        }
                        else
                        {
                            TerminationTime = Convert.ToDateTime(test.SelectSingleNode("ResponseParametersAdditional/TerminationTime").InnerText);
                        }
                    }
                }

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, CreatePullPointSubscription);
            }
            else
            {
                timeOut = 0;
                target = null;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType SMSUnsubscribeTest(out UnsubscribeResponse target, out SoapException ex, out int timeOut, Unsubscribe Unsubscribe1)
        {
            StepType res = StepType.None;
            target = new UnsubscribeResponse();
            timeOut = 0;
            ex = null;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("SMSUnsubscribe");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[SMSUnsubscribe]];

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out timeOut);

                //object targetObj;
                //res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out timeOut, typeof(Events.UnsubscribeResponse));
                //target = (Events.UnsubscribeResponse)targetObj;

                #region Serialization Temp
                //Events.EndpointReferenceType dsr = new Events.EndpointReferenceType();
                //dsr.Address = new Events.AttributedURIType();
                //dsr.Address.Value = "http://192.168.10.203/onvif/event";
                //dsr.Metadata = new Events.MetadataType();
                //dsr.ReferenceParameters = new Events.ReferenceParametersType();
                //XmlSerializer serializer1 = new XmlSerializer(typeof(Events.EndpointReferenceType));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer1.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                m_TestCommon.writeToLog(test, "", true);

                Increment(m_testList.Count, SMSUnsubscribe);
            }
            else
            {
                timeOut = 0;
                target = null;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType GetEventPropertiesTest(out string[] target, out bool FixedTopicSet, out TopicSetType TopicSet, out string[] TopicExpressionDialect, out string[] MessageContentFilterDialect, out string[] ProducerPropertiesFilterDialect, out string[] MessageContentSchemaLocation, out XmlElement[] Any, out SoapException ex, out int timeOut)
        {
            StepType res = StepType.None;
            target = new string[1];
            timeOut = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";
            FixedTopicSet = true;
            TopicSet = new TopicSetType();
            TopicExpressionDialect = new string[1];
            MessageContentFilterDialect = new string[1];
            ProducerPropertiesFilterDialect = new string[1];
            MessageContentSchemaLocation = new string[1];
            Any = new XmlElement[1];

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("GetEventProperties");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[GetEventProperties]];

                //Generate response
                object targetObj;

                //TopicNamespaceLocation
                res = m_TestCommon.GenerateResponseStepTypeNotVoidSpecificXPath(test, out targetObj, out ex, out timeOut, typeof(string[]), "TopicNamespaceLocation");
                target = (string[])targetObj;

                #region Serialization Temp
                //bool dsr = false;
                //XmlSerializer serializer1 = new XmlSerializer(typeof(bool));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer1.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                //FixedTopicSet
                res = m_TestCommon.GenerateResponseStepTypeNotVoidSpecificXPath(test, out targetObj, out ex, out timeOut, typeof(bool), "FixedTopicSet");
                FixedTopicSet = (bool)targetObj;

                //TopicSet
                res = m_TestCommon.GenerateResponseStepTypeNotVoidSpecificXPath(test, out targetObj, out ex, out timeOut, typeof(TopicSetType), "TopicSet");
                TopicSet = (TopicSetType)targetObj;

                //TopicExpressionDialect
                res = m_TestCommon.GenerateResponseStepTypeNotVoidSpecificXPath(test, out targetObj, out ex, out timeOut, typeof(string[]), "TopicExpressionDialect");
                TopicExpressionDialect = (string[])targetObj;

                //MessageContentFilterDialect
                res = m_TestCommon.GenerateResponseStepTypeNotVoidSpecificXPath(test, out targetObj, out ex, out timeOut, typeof(string[]), "MessageContentFilterDialect");
                MessageContentFilterDialect = (string[])targetObj;

                //ProducerPropertiesFilterDialect
                res = m_TestCommon.GenerateResponseStepTypeNotVoidSpecificXPath(test, out targetObj, out ex, out timeOut, typeof(string[]), "ProducerPropertiesFilterDialect");
                ProducerPropertiesFilterDialect = (string[])targetObj;

                //MessageContentSchemaLocation
                res = m_TestCommon.GenerateResponseStepTypeNotVoidSpecificXPath(test, out targetObj, out ex, out timeOut, typeof(string[]), "MessageContentSchemaLocation");
                MessageContentSchemaLocation = (string[])targetObj;

                //Any
                res = m_TestCommon.GenerateResponseStepTypeNotVoidSpecificXPath(test, out targetObj, out ex, out timeOut, typeof(XmlElement[]), "Any");
                Any = (XmlElement[])targetObj;



                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, GetEventProperties);
            }
            else
            {
                timeOut = 0;
                target = null;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType PMSPullMessagesTest(out System.DateTime target, out NotificationMessageHolderType[] NotificationMessage, out System.DateTime TerminationTime, out SoapException ex, out int timeOut, out bool timeOutSpec, string Timeout, int MessageLimit, XmlElement[] Any)
        {
            StepType res = StepType.None;
            target = new System.DateTime();
            timeOut = 0;
            timeOutSpec = false;
            ex = null;
            bool passed = true;
            string logMessage = "";
            NotificationMessage = new NotificationMessageHolderType[1];
            TerminationTime = new System.DateTime();

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("PMSPullMessages");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[PMSPullMessages]];

                #region Analyze request

                //Timeout
                CommonCompare.StringCompare("RequestParameters/Timeout", "Timeout", Timeout, ref logMessage, ref passed, test);

                //MessageLimit
                CommonCompare.IntCompare("RequestParameters/MessageLimit", "MessageLimit", MessageLimit, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response

                //TopicNamespaceLocation
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoidTimeout(test, out targetObj, out ex, out timeOut, out timeOutSpec, typeof(NotificationMessageHolderType[]));
                NotificationMessage = (NotificationMessageHolderType[])targetObj;

                #region Serialization Temp
                //bool dsr = false;
                //XmlSerializer serializer1 = new XmlSerializer(typeof(bool));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer1.Serialize(textWriter, dsr);
                #endregion //Serialization Temp


                if (res == StepType.Normal)
                {
                    //CurrentTime
                    string CurrentTimeType = test.SelectSingleNode("ResponseParametersAdditional/CurrentTime/@type").InnerText;
                    switch (CurrentTimeType)
                    {
                        case "now":
                            {
                                target = System.DateTime.UtcNow;
                                break;
                            }
                        case "value":
                            {
                                target = Convert.ToDateTime(test.SelectSingleNode("ResponseParametersAdditional/CurrentTime").InnerText);
                                break;
                            }
                        case "nowDiff":
                            {
                                int timeDiff = Convert.ToInt32(test.SelectSingleNode("ResponseParametersAdditional/CurrentTime").InnerText);
                                target = System.DateTime.UtcNow.AddSeconds(timeDiff);
                                break;
                            }
                    }

                    //TerminationTime
                    if (test.SelectNodes("ResponseParametersAdditional/TerminationTime[@differance=\"true\"]").Count != 0)
                    {
                        int timeDiff = Convert.ToInt32(test.SelectSingleNode("ResponseParametersAdditional/TerminationTime").InnerText);
                        TerminationTime = target.AddSeconds(timeDiff);
                    }
                    else
                    {
                        TerminationTime = Convert.ToDateTime(test.SelectSingleNode("ResponseParametersAdditional/TerminationTime").InnerText);
                    }

                    //For tns1:Monitoring/OperatingTime/LastClockSynchronization event
                    if (test.SelectNodes("ResponseParametersAdditional/LastClockSynchronization[@differance=\"true\"]").Count != 0)
                    {
                        XmlNameTable xmlNameTable = NotificationMessage[0].Message.OwnerDocument.NameTable;
                        XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlNameTable);
                        xmlNamespaceManager.AddNamespace("tt", "http://www.onvif.org/ver10/schema");
                        NotificationMessage[0].Message.SelectSingleNode("tt:Data/tt:SimpleItem[@Name=\"Status\"]/@Value", xmlNamespaceManager);
                        //int timeDiff = Convert.ToInt32(test.SelectSingleNode("ResponseParametersAdditional/TerminationTime").InnerText);
                        //TerminationTime = target.AddSeconds(timeDiff);
                    }

                }


                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, PMSPullMessages);
            }
            else
            {
                timeOut = 0;
                target = new System.DateTime();
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType PMSSetSynchronizationPointTest(out SoapException ex, out int timeOut)
        {
            StepType res = StepType.None;
            timeOut = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("PMSSetSynchronizationPoint");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[PMSSetSynchronizationPoint]];

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out timeOut);

                //Send notification
                //m_TestCommon.SendNotification(test, ref logMessage, ref passed, LastNotificationAddress);
                m_TestCommon.SendNotifications(test, ref logMessage, ref passed, LastNotificationAddress);

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, PMSSetSynchronizationPoint);
            }
            else
            {
                timeOut = 0;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType SMSRenewTest(out RenewResponse target, out SoapException ex, out int timeOut, Renew Renew1)
        {
            StepType res = StepType.None;
            target = new RenewResponse();
            timeOut = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("SMSRenew");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[SMSRenew]];

                #region Analyze request

                //TerminationTime
                if (test.SelectNodes("RequestParameters/TerminationTime[@type=\"now+diff\"]").Count == 0)
                {
                    CommonCompare.StringCompare("RequestParameters/TerminationTime", "TerminationTime", Renew1.TerminationTime, ref logMessage, ref passed, test);
                }
                else
                {
                    CommonCompare.CompareRealTime("RequestParameters/TerminationTime", "TerminationTime", Renew1.TerminationTime, ref logMessage, ref passed, test);
                }

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out timeOut, typeof(RenewResponse));
                target = (RenewResponse)targetObj;

                #region Serialization Temp
                //Events.EndpointReferenceType dsr = new Events.EndpointReferenceType();
                //dsr.Address = new Events.AttributedURIType();
                //dsr.Address.Value = "http://192.168.10.203/onvif/event";
                //dsr.Metadata = new Events.MetadataType();
                //dsr.ReferenceParameters = new Events.ReferenceParametersType();
                //XmlSerializer serializer1 = new XmlSerializer(typeof(Events.EndpointReferenceType));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer1.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                if (res == StepType.Normal)
                {
                    //CurrentTime
                    string CurrentTimeType = test.SelectSingleNode("ResponseParametersAdditional/CurrentTime/@type").InnerText;
                    switch (CurrentTimeType)
                    {
                        case "now":
                            {
                                target.CurrentTime = System.DateTime.UtcNow;
                                break;
                            }
                        case "value":
                            {
                                target.CurrentTime = Convert.ToDateTime(test.SelectSingleNode("ResponseParametersAdditional/CurrentTime").InnerText);
                                break;
                            }
                        case "nowDiff":
                            {
                                int timeDiff = Convert.ToInt32(test.SelectSingleNode("ResponseParametersAdditional/CurrentTime").InnerText);
                                target.CurrentTime = System.DateTime.UtcNow.AddSeconds(timeDiff);
                                break;
                            }
                        case "none":
                            {
                                target.CurrentTimeSpecified = false;
                                break;
                            }
                    }

                    //TerminationTime
                    if (test.SelectNodes("ResponseParametersAdditional/TerminationTime[@type=\"fromrequest\"]").Count != 0)
                    {
                        target.TerminationTime = Convert.ToDateTime(Renew1.TerminationTime);
                    }
                    else
                    {
                        if (test.SelectNodes("ResponseParametersAdditional/TerminationTime[@differance=\"true\"]").Count != 0)
                        {
                            int timeDiff = Convert.ToInt32(test.SelectSingleNode("ResponseParametersAdditional/TerminationTime").InnerText);
                            target.TerminationTime = target.CurrentTime.AddSeconds(timeDiff);
                        }
                        else
                        {
                            target.TerminationTime = Convert.ToDateTime(test.SelectSingleNode("ResponseParametersAdditional/TerminationTime").InnerText);
                        }
                    }
                }

                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, SMSRenew);
            }
            else
            {
                timeOut = 0;
                target = null;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType SubscribeTest(out SubscribeResponse target, out SoapException ex, out int timeOut, Subscribe Subscribe1)
        {
            StepType res = StepType.None;
            target = new SubscribeResponse();
            timeOut = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("Subscribe");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[Subscribe]];

                #region Analyze request

                //InitialTerminationTime
                CommonCompare.StringCompare("RequestParameters/InitialTerminationTime", "InitialTerminationTime", Subscribe1.InitialTerminationTime, ref logMessage, ref passed, test);

                //ConsumerReference
                if (Subscribe1.ConsumerReference != null)
                {
                    //Address
                    CommonCompare.StringCompare("RequestParameters/Address", "Address", Subscribe1.ConsumerReference.Address.Value, ref logMessage, ref passed, test);
                    LastNotificationAddress = Subscribe1.ConsumerReference.Address.Value;
                    m_TestCommon.LastNotificationAddress = LastNotificationAddress;
                }
                else
                {
                    passed = false;
                    logMessage = logMessage + "No mandatory ConsumerReference.";
                }

                //Filter
                if (Subscribe1.Filter != null)
                {
                    if (test.SelectSingleNode("RequestParameters/Filter") != null)
                    {
                        if (test.SelectSingleNode("RequestParameters/Filter[@nocomp=\"true\"]") == null)
                        {
                            string XMLString = "";
                            foreach (XmlElement i in Subscribe1.Filter.Any)
                            {
                                XMLString = XMLString + i.OuterXml;
                            }
                            CommonCompare.StringCompare("RequestParameters/Filter", "Filter", XMLString, ref logMessage, ref passed, test);
                        }
                        else
                        {
                            string XMLString = "";
                            foreach (XmlElement i in Subscribe1.Filter.Any)
                            {
                                XMLString = XMLString + i.OuterXml;
                            }
                            logMessage = logMessage + "Filter=" + XMLString;
                        }
                    }
                    else
                    {
                        passed = false;
                        logMessage = logMessage + "Unexpected Filter.";
                    }
                }
                else
                {
                    if (test.SelectSingleNode("RequestParameters/Filter") != null)
                    {
                        passed = false;
                        logMessage = logMessage + "No Filter.";
                    }
                }



                #endregion //Analyze request

                //Generate response
                int useRealAddress = 0;
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoidSpecial(test, out targetObj, out ex, out timeOut, typeof(SubscribeResponse), out useRealAddress);
                target = (SubscribeResponse)targetObj;

                #region Serialization Temp
                //Events.EndpointReferenceType dsr = new Events.EndpointReferenceType();
                //dsr.Address = new Events.AttributedURIType();
                //dsr.Address.Value = "http://192.168.10.203/onvif/event";
                //dsr.Metadata = new Events.MetadataType();
                //dsr.ReferenceParameters = new Events.ReferenceParametersType();
                //XmlSerializer serializer1 = new XmlSerializer(typeof(Events.EndpointReferenceType));
                //TextWriter textWriter = new StreamWriter("c:\\2.txt");
                //serializer1.Serialize(textWriter, dsr);
                #endregion //Serialization Temp

                if (res == StepType.Normal)
                {
                    //Get real path to service
                    if (useRealAddress != 0)
                    {
                        if ((target.SubscriptionReference != null)&&(target.SubscriptionReference.Address != null))
                        {
                            switch (useRealAddress)
                            {
                                case 1:
                                    target.SubscriptionReference.Address.Value = m_TestCommon.SubscriptionManagerServiceUri;
                                    break;
                                case 2:
                                    target.SubscriptionReference.Address.Value = m_TestCommon.PullpointSubscriptionServiceUri;
                                    break;
                            }
                        }
                    }

                    //CurrentTime
                    string CurrentTimeType = test.SelectSingleNode("ResponseParametersAdditional/CurrentTime/@type").InnerText;
                    switch (CurrentTimeType)
                    {
                        case "now":
                            {
                                target.CurrentTime = System.DateTime.UtcNow;
                                break;
                            }
                        case "value":
                            {
                                target.CurrentTime = Convert.ToDateTime(test.SelectSingleNode("ResponseParametersAdditional/CurrentTime").InnerText);
                                break;
                            }
                        case "nowDiff":
                            {
                                int timeDiff = Convert.ToInt32(test.SelectSingleNode("ResponseParametersAdditional/CurrentTime").InnerText);
                                target.CurrentTime = System.DateTime.UtcNow.AddSeconds(timeDiff);
                                break;
                            }
                        case "none":
                            {
                                target.CurrentTimeSpecified = false;
                                break;
                            }
                    }

                    //TerminationTime
                    if (test.SelectNodes("ResponseParametersAdditional/TerminationTime[@differance=\"true\"]").Count != 0)
                    {
                        int timeDiff = Convert.ToInt32(test.SelectSingleNode("ResponseParametersAdditional/TerminationTime").InnerText);
                        target.TerminationTime = target.CurrentTime.AddSeconds(timeDiff);
                    }
                    else
                    {
                        target.TerminationTime = Convert.ToDateTime(test.SelectSingleNode("ResponseParametersAdditional/TerminationTime").InnerText);
                    }


                }

                //Send notification
                m_TestCommon.SendNotifications(test, ref logMessage, ref passed, LastNotificationAddress);
                
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, Subscribe);
            }
            else
            {
                timeOut = 0;
                target = null;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal StepType GetServiceCapabilitiesTest(out Capabilities target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetServiceCapabilities";
            int tmpCommandNumber = GetServiceCapabilities;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[tmpCommandNumber]];

                //Generate response
                object targetObj;
                res = m_TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Capabilities));
                target = (Capabilities)targetObj;

                //Log message
                m_TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, tmpCommandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + tmpCommandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal StepType SeekTest(out SoapException ex, out int timeOut, DateTime UtcTime, bool Reverse, bool ReverseSpecified, XmlElement[] Any)
        {
            StepType res = StepType.None;
            timeOut = 0;
            ex = null;

            //Get step list for command
            XmlNodeList m_testList = m_TestCommon.GetStepsForCommand("PMSSeek");

            if (m_testList != null)
            {
                //Get current step
                XmlNode test = m_testList[m_commandCount[SMSSeek]];

                //Generate response
                res = m_TestCommon.GenerateResponseStepTypeVoid(test, out ex, out timeOut);

                m_TestCommon.writeToLog(test, "", true);

                Increment(m_testList.Count, SMSSeek);
            }
            else
            {
                timeOut = 0;
                ex = null;
                res = StepType.None;
            }
            return res;
        }

        internal static DateTime TakeTerminationTime()
        {
            throw new NotImplementedException();
        }

        internal static NotificationMessageHolderType[] TakeNotificationMessage()
        {
            throw new NotImplementedException();
        }
    }
}
