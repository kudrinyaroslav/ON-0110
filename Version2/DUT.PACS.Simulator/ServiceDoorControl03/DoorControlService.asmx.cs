using System;
using System.Collections.Generic;
using System.Linq;

namespace DUT.PACS.Simulator.ServiceDoorControl03
{
    /// <summary>
    /// Summary description for DoorControlService1
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "DoorControlBinding", Namespace = "http://www.onvif.org/v3/DoorControl/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DoorControlService : DoorControlServiceBinding
    {

        #region Members
        
        /// <summary>
        /// Default Access time for AccessDoor command
        /// </summary>
        const int c_DefaultAccessTime = 30000;

        #endregion //Members


        /***************************************************************************************/

        #region Events


        public DoorControlService()
        {
            ConfStorageLoad();
            EventServerLoad();

            EventServer.DoorControlService = this;
            EventServerSave();
            ConfStorageSave();
        }

        public void SynchronizationPoint(string Token)
        {
            ConfStorageLoad();
            EventServerLoad();

            DoorState state = ConfStorage.DoorStateList[Token];
            FireAllEvents(Token, state);

            EventServerSave();
            ConfStorageSave();
        }

        public void SynchronizationPoint()
        {
            ConfStorageLoad();
            EventServerLoad();

            foreach (string Token in ConfStorage.DoorStateList.Keys)
            {
                DoorState state = ConfStorage.DoorStateList[Token];
                FireAllEvents(Token, state);
            }

            EventServerSave();
            ConfStorageSave();
        }

        #endregion //Events

        /***************************************************************************************/

        #region AccessDoor

        private delegate void AccessToDoor(string Token, TimeSpan AccessTime, int DoorAccessSesion);

        private void AccessToDoorImplementation(string Token, TimeSpan AccessTime, int DoorAccessSesion)
        {
            if (AccessTime == null)
            {
                System.Threading.Thread.Sleep(c_DefaultAccessTime);
            }
            else
            {
                System.Threading.Thread.Sleep((int)(AccessTime.TotalMilliseconds));
            }
            if (ConfStorage.DoorStateList[Token].DoorMode == DoorModeType.Accessed)
            {
                if (DoorAccessSesion == ConfStorage.DoorAccessList[Token])
                {
                    ConfStorage.DoorStateList[Token].DoorMode = DoorModeType.Locked;
                }
            }
        }

        #endregion AccessDoor

        /***************************************************************************************/

        #region WebMethods

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetDoorState", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DoorState")]
        public override DoorState GetDoorState(string Token)
        {
            DoorState res;

            ConfStorageLoad();
            EventServerLoad();

            if (ConfStorage.DoorStateList.ContainsKey(Token))
            {

                res = ConfStorage.DoorStateList[Token];
            }
            else
            {
                string message = string.Format("Token {0} does not exist", Token);
                LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                throw FaultLib.GetSoapException(FaultType.General, "Token " + Token + " does not exist.");
            }

            EventServerSave();
            ConfStorageSave();

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetDoorInfoList", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("DoorInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override DoorInfo[] GetDoorInfoList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList, int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, int Offset, [System.Xml.Serialization.XmlIgnoreAttribute()] bool OffsetSpecified)
        {
            DoorInfo[] res;
            List<DoorInfo> tempRes = new List<DoorInfo>();
            bool tokenFaund = false;

            ConfStorageLoad();
            EventServerLoad();

            res = ConfStorage.DoorInfoList;

            if ((TokenList == null) || (TokenList.Count() == 0))
            {
                res = ConfStorage.DoorInfoList;

                if (OffsetSpecified)
                {
                    res = res.Skip(Offset).ToArray();
                }

                if (LimitSpecified)
                {
                    res = res.Take(Limit).ToArray();
                }
            }
            else
            {
                foreach (string token in TokenList)
                {
                    tokenFaund = false;
                    foreach (DoorInfo doorInfo in ConfStorage.DoorInfoList)
                    {
                        if (doorInfo.token == token)
                        {
                            if (!tempRes.Contains(doorInfo))
                            {
                                tempRes.Add(doorInfo);
                            }
                            tokenFaund = true;
                            break;
                        }
                    }

                    if (!tokenFaund)
                    {
                        string message = string.Format("Token {0} does not exist", token);
                        LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                        throw FaultLib.GetSoapException(FaultType.General, "Token " + token + " does not exist.");
                    }
                }
                res = tempRes.ToArray();
            }

            EventServerSave();
            ConfStorageSave();

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/AccessDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AccessDoor(string Token, bool UseExtendedTime, [System.Xml.Serialization.XmlIgnoreAttribute()] bool UseExtendedTimeSpecified, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string AccessTime, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string OpenTooLongTime, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string PreAlarmTime, AccessDoorExtension Extension)
        {
            ConfStorageLoad();
            EventServerLoad();

            LoggingService.LogMessage(string.Format("AccessDoor operation requested for {0}", Token),
                                      ExternalLogging.MessageType.Details);

            if (ConfStorage.DoorStateList.ContainsKey(Token))
            {
                if (ConfStorage.DoorCapabilitiesList[Token].MomentaryAccess)
                {
                    if ((ConfStorage.DoorStateList[Token].DoorMode == DoorModeType.Blocked) || (ConfStorage.DoorStateList[Token].DoorMode == DoorModeType.LockedDown) || (ConfStorage.DoorStateList[Token].DoorMode == DoorModeType.LockedOpen))
                    {
                        string message = "Door " + Token + " is " + ConfStorage.DoorStateList[Token].DoorMode.ToString() +
                                         ". Operation denied.";

                        LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);

                        throw FaultLib.GetSoapException(FaultType.General, message);
                    }
                    else
                    {
                        try
                        {
                            TimeSpan timeSpan = System.Xml.XmlConvert.ToTimeSpan(AccessTime);

                            ConfStorage.DoorStateList[Token].DoorMode = DoorModeType.Accessed;

                            EventServer.DoorModeEvent(this, "Changed", Token, ConfStorage.DoorStateList[Token].DoorMode);

                            AccessToDoor func = new AccessToDoor(AccessToDoorImplementation);
                            ConfStorage.DoorAccessList[Token]++;
                            func.BeginInvoke(Token, timeSpan, ConfStorage.DoorAccessList[Token], null, null);
                        }
                        catch (Exception)
                        {
                            LoggingService.LogMessage(string.Format("Wrong duration ({0})", AccessTime), ExternalLogging.MessageType.Error);
                            throw FaultLib.GetSoapException(FaultType.General, "Wrong duration.");
                        }
                    }
                }
                else
                {
                    string message = string.Format("MomentaryAccess isn't supported for {0}.", Token);
                    LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);

                    throw FaultLib.GetSoapException(FaultType.General, message);
                }
            }
            else
            {
                string message = string.Format("Token {0} does not exist", Token);
                LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                throw FaultLib.GetSoapException(FaultType.General, message);
            }

            EventServerSave();
            ConfStorageSave();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDoor(string Token)
        {
            string operationName = "Lock";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.Lock);
            Func<DoorModeType, bool> operationAllowedCheck =
                DM => (DM == DoorModeType.LockedDown || DM == DoorModeType.LockedOpen);
            DoorModeType target = DoorModeType.Locked;
            string forbidSeverity = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationAllowedCheck, forbidSeverity, target);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/UnlockDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void UnlockDoor(string Token)
        {
            string operationName = "Unlock";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.Unlock);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM == DoorModeType.LockedDown || DM == DoorModeType.LockedOpen);
            DoorModeType target = DoorModeType.Locked;
            string forbidSeverity = "denied";

            Func<DoorModeType, DoorModeType> transition = 
                new Func<DoorModeType, DoorModeType>(
                    (initial) =>
                        {
                            return initial == DoorModeType.DoubleLocked ? DoorModeType.Locked : DoorModeType.Unlocked;
                        }
                    );

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, transition);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/BlockDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void BlockDoor(string Token)
        {

            string operationName = "Block";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.Block);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM == DoorModeType.LockedDown || DM == DoorModeType.LockedOpen);
            DoorModeType target = DoorModeType.Blocked;
            string forbidSeverity = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockDownDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDownDoor(string Token)
        {
            string operationName = "LockDown";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.LockDown);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM == DoorModeType.LockedDown || DM == DoorModeType.LockedOpen);
            DoorModeType target = DoorModeType.LockedDown;
            string forbidSeverity = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockDownReleaseDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDownReleaseDoor(string Token)
        {
            string operationName = "LockDown";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.LockDown);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM != DoorModeType.LockedDown);
            DoorModeType target = DoorModeType.Locked;
            string forbidSeverity = "not applicable";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockOpenDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockOpenDoor(string Token)
        {
            string operationName = "LockOpen";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.LockOpen);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM == DoorModeType.LockedDown || DM == DoorModeType.LockedOpen);
            DoorModeType target = DoorModeType.LockedDown;
            string forbidSeverity = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockOpenReleaseDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockOpenReleaseDoor(string Token)
        {

            string operationName = "LockedOpen";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.LockOpen);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM != DoorModeType.LockedOpen);
            DoorModeType target = DoorModeType.Unlocked;
            string forbidSeverity = "not applicable";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/DoubleLockDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DoubleLockDoor(string Token)
        {
            string operationName = "DoubleLock";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>( DC => DC.DoubleLock);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM == DoorModeType.LockedDown || DM == DoorModeType.LockedOpen);
            DoorModeType target = DoorModeType.DoubleLocked;
            string forbidType = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidType, target);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override DoorServiceCapabilities GetServiceCapabilities()
        {
            DoorServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.DoorServiceCapabilities;
            return capabilities;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetDoorInfo", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DoorInfo")]
        public override DoorInfo GetDoorInfo(string Token)
        {
            DoorInfo res;

            ConfStorageLoad();

            if (ConfStorage.DoorInfoList.Count(doorInfo => doorInfo.token == Token) != 0)
            {

                res = ConfStorage.DoorInfoList.Single(doorInfo => doorInfo.token == Token); 
            }
            else
            {
                string message = string.Format("Token {0} does not exist", Token);
                LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                throw FaultLib.GetSoapException(FaultType.General, "Token " + Token + " does not exist.");
            }

            ConfStorageSave();

            return res;
        }

        #endregion //WebMethods

        /***************************************************************************************/

        #region Utils
        
        void DoorOperation(string Token, 
            string operationName,
            Func<DoorCapabilities, bool> capabilitiesCheck,
            Func<DoorModeType, bool> operationForbiddenCheck,
            string notAllowedSeverity,
            Func<DoorModeType, DoorModeType> transition)
        {

            LoggingService.LogMessage(string.Format("{0} operation requested for {1}", operationName, Token), ExternalLogging.MessageType.Details);

            ConfStorageLoad();
            EventServerLoad();

            if (ConfStorage.DoorStateList.ContainsKey(Token))
            {
                if (capabilitiesCheck(ConfStorage.DoorCapabilitiesList[Token]))
                {
                    DoorState doorState = ConfStorage.DoorStateList[Token];
                    if (operationForbiddenCheck(doorState.DoorMode))
                    {

                        string message = String.Format("Door {0} is {1}. Operation {2}", Token,
                                                       doorState.DoorMode.ToString(), notAllowedSeverity);
                    
                        LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                        throw FaultLib.GetSoapException(FaultType.General,
                                                        message);
                    }
                    else
                    {
                        DoorModeType targetState = transition(doorState.DoorMode);
                        doorState.DoorMode = targetState;
                        StateReporter.ReportStateUpdate(Token, doorState);
                        EventServer.DoorModeEvent(this, "Changed", Token, doorState.DoorMode);
                    }
                }
                else
                {
                    string message = string.Format("{0} is not suported for {1}.", operationName, Token);
                    LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);

                    throw FaultLib.GetSoapException(FaultType.General, message);
                }
            }
            else
            {
                string message = string.Format("Token {0} does not exist", Token);
                LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                throw FaultLib.GetSoapException(FaultType.General, message);
            }

            EventServerSave();
            ConfStorageSave();
        }

        void DoorOperation(string Token, 
            string operationName,
            Func<DoorCapabilities, bool> capabilitiesCheck,
            Func<DoorModeType, bool> operationAllowedCheck,
            string notAllowedSeverity,
            DoorModeType targetState)
        {
            Func<DoorModeType, DoorModeType> transition = new Func<DoorModeType, DoorModeType>(DM => targetState);
            DoorOperation(Token, operationName, capabilitiesCheck, operationAllowedCheck, notAllowedSeverity, transition);

        }

        void FireAllEvents(string Token, DoorState state)
        {
            EventServer.DoorModeEvent(this, "Initialized", Token, state.DoorMode);
            EventServer.DoorMonitorEvent(this, "Initialized", Token, state.DoorMonitor);
            EventServer.DoorDoubleLockMonitorEvent(this, "Initialized", Token, state.DoorDoubleLockMonitor);
            EventServer.DoorLockMonitorEvent(this, "Initialized", Token, state.DoorLockMonitor);
            EventServer.DoorAlarmMonitorEvent(this, "Initialized", Token, state.DoorAlarm);
            EventServer.DoorTamperMonitorEvent(this, "Initialized", Token, state.DoorTamper);

        }

        #endregion

        #region Depricated

        //[WebMethod]
        //[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetDoorCapabilities", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[return: System.Xml.Serialization.XmlElementAttribute("DoorCapabilities")]
        //public override DoorCapabilities GetDoorCapabilities(string Token)
        //{
        //    DoorCapabilities res;

        //    ConfStorageLoad();
        //    EventServerLoad();

        //    if (m_ConfStorage.DoorCapabilitiesList.ContainsKey(Token))
        //    {

        //        res = m_ConfStorage.DoorCapabilitiesList[Token];
        //    }
        //    else
        //    {
        //        throw FaultLib.GetSoapException(FaultType.General, "Token " + Token + " does not exist.");
        //    }

        //    EventServerSave();
        //    ConfStorageSave();

        //    return res;
        //}

        #endregion //Depricated
    }
}
