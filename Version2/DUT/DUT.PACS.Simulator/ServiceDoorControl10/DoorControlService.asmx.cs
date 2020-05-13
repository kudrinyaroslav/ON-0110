using System;
using System.Collections.Generic;
using System.Linq;

namespace DUT.PACS.Simulator.ServiceDoorControl10
{
    /// <summary>
    /// Summary description for DoorControlService1
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "DoorControlBinding", Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
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
                DoorInfo door = ConfStorage.DoorInfoList[Token];
                FireAllEvents(door, state);
            }

            EventServerSave();
            ConfStorageSave();
        }

        #endregion //Events

        #region Sensors

        protected DoorSensorService DoorSensorService
        {
            get
            {
                DoorSensorService service = null;
                if (Application[Common.AppVars.SENSORSERBICE] != null)
                {
                    service = (DoorSensorService)Application[Common.AppVars.SENSORSERBICE];
                }
                else
                {
                    service = new DoorSensorService();
                    Application[Common.AppVars.SENSORSERBICE] = service;
                }

                return service;
            }
        }


        #endregion

        /***************************************************************************************/

        #region AccessDoor

        private delegate void AccessToDoor(string Token, TimeSpan AccessTime, int DoorAccessSesion);

        private void AccessToDoorImplementation(string Token, TimeSpan AccessTime, int DoorAccessSesion)
        {
            if (AccessTime == TimeSpan.Zero)
            {
                System.Threading.Thread.Sleep(c_DefaultAccessTime);
            }
            else
            {
                System.Threading.Thread.Sleep((int)(AccessTime.TotalMilliseconds));
            }
            if (ConfStorage.DoorStateList[Token].DoorMode == DoorMode.Accessed)
            {
                if (DoorAccessSesion == ConfStorage.DoorAccessList[Token])
                {
                    ConfStorage.DoorStateList[Token].DoorMode = ConfStorage.DoorAccessPreviousStateList[Token];
                    DoorSensorService.ProcessModeChanging(Token, ConfStorage.DoorStateList[Token].DoorMode, ConfStorage, EventServer, StateReporter);
                    StateReporter.ReportStateUpdate(Token, ConfStorage.DoorStateList[Token]);
                    EventServer.DoorModeEvent(this, "Changed", Token, ConfStorage.DoorStateList[Token].DoorMode);
                }
            }
        }

        #endregion AccessDoor

        /***************************************************************************************/

        #region WebMethods

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorState", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DoorState")]
        public override DoorState GetDoorState(string Token)
        {
            return GetInfo(Token, (c) => c.DoorStateList);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/AccessDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AccessDoor(string Token, bool UseExtendedTime, [System.Xml.Serialization.XmlIgnoreAttribute()] bool UseExtendedTimeSpecified, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string AccessTime, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string OpenTooLongTime, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string PreAlarmTime, AccessDoorExtension Extension)
        {
            ConfStorageLoad();
            EventServerLoad();

            LoggingService.LogMessage(string.Format("AccessDoor operation requested for {0}", Token),
                                      ExternalLogging.MessageType.Details);

            if (ConfStorage.DoorStateList.ContainsKey(Token))
            {
                if (ConfStorage.DoorInfoList[Token].Capabilities.AccessSpecified && ConfStorage.DoorInfoList[Token].Capabilities.Access)
                {
                    if ((ConfStorage.DoorStateList[Token].DoorMode == DoorMode.Blocked) || (ConfStorage.DoorStateList[Token].DoorMode == DoorMode.LockedDown) || (ConfStorage.DoorStateList[Token].DoorMode == DoorMode.LockedOpen))
                    {
                        string message = "Door " + Token + " is " + ConfStorage.DoorStateList[Token].DoorMode.ToString() +
                                         ". Operation denied.";

                        LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "ActionNotSupported" });
                        //throw FaultLib.GetSoapException(FaultType.General, message);
                    }
                    else
                    {
                        try
                        {
                            TimeSpan timeSpan = TimeSpan.Zero;
                            if (!string.IsNullOrEmpty(AccessTime))
                            {
                                timeSpan = System.Xml.XmlConvert.ToTimeSpan(AccessTime);
                            }

                            if (ConfStorage.DoorStateList[Token].DoorMode != DoorMode.Accessed)
                            {
                                ConfStorage.DoorAccessPreviousStateList[Token] = ConfStorage.DoorStateList[Token].DoorMode;
                            }

                            ConfStorage.DoorStateList[Token].DoorMode = DoorMode.Accessed;
                            StateReporter.ReportStateUpdate(Token, ConfStorage.DoorStateList[Token]);
                            DoorSensorService.ProcessModeChanging(Token, DoorMode.Accessed, ConfStorage, EventServer, StateReporter);

                            EventServer.DoorModeEvent(this, "Changed", Token, ConfStorage.DoorStateList[Token].DoorMode);

                            AccessToDoor func = new AccessToDoor(AccessToDoorImplementation);
                            ConfStorage.DoorAccessList[Token]++;

                            func.BeginInvoke(Token, timeSpan, ConfStorage.DoorAccessList[Token], null, null);
                        }
                        catch (Exception)
                        {
                            string message = string.Format("Wrong duration ({0})", AccessTime);
                            LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                            FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal" });
                            //throw FaultLib.GetSoapException(FaultType.General, "Wrong duration.");
                        }
                    }
                }
                else
                {
                    string message = string.Format("MomentaryAccess isn't supported for {0}.", Token);
                    LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "ActionNotSupported" });
                    //throw FaultLib.GetSoapException(FaultType.General, message);
                }
            }
            else
            {
                string message = string.Format("Token {0} does not exist", Token);
                LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
                //throw FaultLib.GetSoapException(FaultType.General, message);
            }

            EventServerSave();
            ConfStorageSave();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDoor(string Token)
        {
            string operationName = "Lock";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.Lock && DC.LockSpecified);
            Func<DoorMode, bool> operationAllowedCheck =
                DM => (DM == DoorMode.LockedDown || DM == DoorMode.LockedOpen);
            DoorMode target = DoorMode.Locked;
            string forbidSeverity = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationAllowedCheck, forbidSeverity, target);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/UnlockDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void UnlockDoor(string Token)
        {
            string operationName = "Unlock";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.Unlock && DC.UnlockSpecified);
            Func<DoorMode, bool> operationForbiddenCheck =
                DM => (DM == DoorMode.LockedDown || DM == DoorMode.LockedOpen);
            DoorMode target = DoorMode.Locked;
            string forbidSeverity = "denied";

            Func<DoorMode, DoorMode> transition =
                new Func<DoorMode, DoorMode>(
                    (initial) =>
                    {
                        return initial == DoorMode.DoubleLocked ? DoorMode.Locked : DoorMode.Unlocked;
                    }
                    );

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, transition);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/BlockDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void BlockDoor(string Token)
        {

            string operationName = "Block";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.Block && DC.BlockSpecified);
            Func<DoorMode, bool> operationForbiddenCheck =
                DM => (DM == DoorMode.LockedDown || DM == DoorMode.LockedOpen);
            DoorMode target = DoorMode.Blocked;
            string forbidSeverity = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockDownDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDownDoor(string Token)
        {
            string operationName = "LockDown";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.LockDown && DC.LockDownSpecified);
            Func<DoorMode, bool> operationForbiddenCheck =
                DM => (DM == DoorMode.LockedOpen);
            DoorMode target = DoorMode.LockedDown;
            string forbidSeverity = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockDownReleaseDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDownReleaseDoor(string Token)
        {
            string operationName = "LockDown";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.LockDown && DC.LockDownSpecified);
            Func<DoorMode, bool> operationForbiddenCheck =
                DM => (DM != DoorMode.LockedDown);
            DoorMode target = DoorMode.Locked;
            string forbidSeverity = "not applicable";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockOpenDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockOpenDoor(string Token)
        {
            string operationName = "LockOpen";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.LockOpen && DC.LockOpenSpecified);
            Func<DoorMode, bool> operationForbiddenCheck =
                DM => (DM == DoorMode.LockedDown);
            DoorMode target = DoorMode.LockedOpen;
            string forbidSeverity = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockOpenReleaseDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockOpenReleaseDoor(string Token)
        {

            string operationName = "LockedOpen";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.LockOpen && DC.LockOpenSpecified);
            Func<DoorMode, bool> operationForbiddenCheck =
                DM => (DM != DoorMode.LockedOpen);
            DoorMode target = DoorMode.Unlocked;
            string forbidSeverity = "not applicable";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/DoubleLockDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DoubleLockDoor(string Token)
        {
            string operationName = "DoubleLock";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.DoubleLock && DC.DoubleLockSpecified);
            Func<DoorMode, bool> operationForbiddenCheck =
                DM => (DM == DoorMode.LockedDown || DM == DoorMode.LockedOpen);
            DoorMode target = DoorMode.DoubleLocked;
            string forbidType = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidType, target);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override ServiceCapabilities GetServiceCapabilities()
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.DoorServiceCapabilities;
            return capabilities;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorInfo", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DoorInfo")]
        public override DoorInfo[] GetDoorInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.DoorServiceCapabilities;

            if (Token != null && Token.Length > capabilities.MaxLimit)
                FaultLib.ReturnFault("Too many items was requested. ", new[] { "Sender", "InvalidArgVal", "TooManyItems" });
            return GetListByTokenList<DoorInfo>(Token, C => C.DoorInfoList);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorInfoList", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetDoorInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("DoorInfo")] out DoorInfo[] DoorInfo)
        {
            ServiceCapabilities capabilities = Simulator.SystemCapabilities.Instance.DoorServiceCapabilities;
            int offset = 0;
            if (!string.IsNullOrEmpty(StartReference))
                if (!Int32.TryParse(StartReference, out offset))
                    FaultLib.ReturnFault("Invalid StartReferense value. ", new[] { "Sender", "InvalidArgVal", "InvalidArgVal" });

            if (!LimitSpecified)
                Limit = capabilities.MaxLimit > int.MaxValue ?
                    int.MaxValue : (int)capabilities.MaxLimit;

            DoorInfo = GetList<DoorInfo>(offset, true, Limit, true, C => C.DoorInfoList);
            string newStartReferense = null;
            if (offset + DoorInfo.Length < ConfStorage.DoorInfoList.Count)
            {
                newStartReferense = Convert.ToString(offset + DoorInfo.Length);
            } return newStartReferense;
        }

        #endregion //WebMethods

        /***************************************************************************************/

        #region Utils

        void DoorOperation(string Token,
            string operationName,
            Func<DoorCapabilities, bool> capabilitiesCheck,
            Func<DoorMode, bool> operationForbiddenCheck,
            string notAllowedSeverity,
            Func<DoorMode, DoorMode> transition)
        {

            LoggingService.LogMessage(string.Format("{0} operation requested for {1}", operationName, Token), ExternalLogging.MessageType.Details);

            ConfStorageLoad();
            EventServerLoad();

            if (ConfStorage.DoorStateList.ContainsKey(Token))
            {
                if (capabilitiesCheck(ConfStorage.DoorInfoList[Token].Capabilities))
                {
                    DoorState doorState = ConfStorage.DoorStateList[Token];
                    if (operationForbiddenCheck(doorState.DoorMode))
                    {

                        string message = String.Format("Door {0} is {1}. Operation {2}", Token,
                                                       doorState.DoorMode.ToString(), notAllowedSeverity);

                        LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                        FaultLib.ReturnFault(message, new string[] { "Sender", "ActionNotSupported" });
                        //throw FaultLib.GetSoapException(FaultType.General, message);
                    }
                    else
                    {
                        DoorMode targetState = transition(doorState.DoorMode);
                        doorState.DoorMode = targetState;
                        DoorSensorService.ProcessModeChanging(Token, targetState, ConfStorage, EventServer, StateReporter);
                        StateReporter.ReportStateUpdate(Token, doorState);
                        EventServer.DoorModeEvent(this, "Changed", Token, doorState.DoorMode);
                    }
                }
                else
                {
                    string message = string.Format("{0} is not supported for {1}.", operationName, Token);
                    LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                    FaultLib.ReturnFault(message, new string[] { "Sender", "ActionNotSupported" });
                    //throw FaultLib.GetSoapException(FaultType.General, message);
                }
            }
            else
            {
                string message = string.Format("Token {0} does not exist", Token);
                LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
                //throw FaultLib.GetSoapException(FaultType.General, message);
            }

            EventServerSave();
            ConfStorageSave();
        }

        void DoorOperation(string Token,
            string operationName,
            Func<DoorCapabilities, bool> capabilitiesCheck,
            Func<DoorMode, bool> operationAllowedCheck,
            string notAllowedSeverity,
            DoorMode targetState)
        {
            Func<DoorMode, DoorMode> transition = new Func<DoorMode, DoorMode>(DM => targetState);
            DoorOperation(Token, operationName, capabilitiesCheck, operationAllowedCheck, notAllowedSeverity, transition);

        }

        void FireAllEvents(string Token, DoorState state)
        {
            EventServer.DoorModeEvent(this, "Initialized", Token, state.DoorMode);
            EventServer.DoorPhysicalStateEvent(this, "Initialized", Token, state.DoorPhysicalState);
            EventServer.DoubleLockPhysicalStateEvent(this, "Initialized", Token, state.DoubleLockPhysicalState);
            EventServer.LockPhysicalStateEvent(this, "Initialized", Token, state.LockPhysicalState);
            EventServer.DoorAlarmMonitorEvent(this, "Initialized", Token, state.Alarm);
            if (state.Tamper != null)
            {
                EventServer.DoorTamperMonitorEvent(this, "Initialized", Token, state.Tamper.State);
            }

        }

        void FireAllEvents(DoorInfo door, DoorState state)
        {
            var capabilities = door.Capabilities;

            EventServer.DoorModeEvent(this, "Initialized", door.token, state.DoorMode);

            if (capabilities.DoorMonitorSpecified && capabilities.DoorMonitor)
                EventServer.DoorPhysicalStateEvent(this, "Initialized", door.token, state.DoorPhysicalState);

            if (capabilities.LockMonitorSpecified && capabilities.LockMonitor)
                EventServer.LockPhysicalStateEvent(this, "Initialized", door.token, state.LockPhysicalState);

            if (capabilities.DoubleLockMonitorSpecified && capabilities.DoubleLockMonitor)
                EventServer.DoubleLockPhysicalStateEvent(this, "Initialized", door.token, state.DoubleLockPhysicalState);

            if (capabilities.AlarmSpecified && capabilities.Alarm)
                EventServer.DoorAlarmMonitorEvent(this, "Initialized", door.token, state.Alarm);

            if (capabilities.TamperSpecified && capabilities.Tamper)
                if (state.Tamper != null)
                {
                    EventServer.DoorTamperMonitorEvent(this, "Initialized", door.token, state.Tamper.State);
                }

            if (capabilities.FaultSpecified && capabilities.Fault)
                if (state.Fault != null)
                {
                    EventServer.DoorFaultEvent(this, "Initialized", door.token, state.Fault.State);
                }

            //if (capabilities.FaultSpecified && capabilities.Fault)
            //    EventServer.DoorFaultMonitorEvent(this, "Initialized", door.token, state.Fault);

        }

        #endregion

    }
}
