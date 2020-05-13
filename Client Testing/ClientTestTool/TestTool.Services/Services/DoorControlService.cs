using System;
using TestTool.Onvif;

namespace TestTool.Services
{
    [System.ServiceModel.ServiceBehavior(InstanceContextMode = System.ServiceModel.InstanceContextMode.Single)]
    public class DoorControlService : BasePacsService, TestTool.Onvif.DoorControlPort
    {
        public DoorControlService()
        {

        }
        
        public override string GetServiceName()
        {
            return "DoorControl";
        }

        protected override Type GetContractType()
        {
            return typeof (Onvif.DoorControlPort);
        }

        public override string GetLocalAddress()
        {
            return "onvif/doorcontrol_service";
        }

        #region DoorControlPort Members

        public void DoNothing()
        {
            
        }

        public DoorState GetDoorState(string Token)
        {
            BeginMethod("GetDoorState");
            EndMethod();
            return GetInfo(Token, (c) => c.PacsConfiguration.DoorStateList);

        }

        public GetDoorInfoListResponse GetDoorInfoList(TestTool.Onvif.GetDoorInfoListRequest request)
        {
            BeginMethod("GetDoorInfoList");

            DoorInfo[] list = GetList<DoorInfo>(request.Offset.GetValueOrDefault(), request.Offset.HasValue,
                                                request.Limit.GetValueOrDefault(), request.Limit.HasValue,
                                                A => A.token,
                                                C => C.PacsConfiguration.DoorInfoList);
            EndMethod();
            return new GetDoorInfoListResponse(list);
        }

        public GetDoorInfoListByTokenListResponse GetDoorInfoListByTokenList(TestTool.Onvif.GetDoorInfoListByTokenListRequest request)
        {
            BeginMethod("GetDoorInfoListByTokenList");

            DoorInfo[] list = GetListByTokenList<DoorInfo>(request.TokenList, A => A.token,
                                                C => C.PacsConfiguration.DoorInfoList);
            EndMethod();
            return new GetDoorInfoListByTokenListResponse(list);
        }

        public DoorInfo GetDoorInfo(string Token)
        {
            BeginMethod("GetDoorInfo");
            EndMethod();

            return GetInfo(Token, D => D.token, S => S.PacsConfiguration.DoorInfoList);
        }

        public AccessDoorResponse AccessDoor(TestTool.Onvif.AccessDoorRequest request)
        {
            BeginMethod("AccessDoor");

            string token = request.Token;

            Log(string.Format("AccessDoor operation requested for {0}", token));

            if (SimulatorConfiguration.PacsConfiguration.DoorStateList.ContainsKey(token))
            {
                if (SimulatorConfiguration.PacsConfiguration.DoorCapabilitiesList[token].MomentaryAccess)
                {
                    DoorModeType doorMode = SimulatorConfiguration.PacsConfiguration.DoorStateList[token].DoorMode;
                    if ((doorMode == DoorModeType.Blocked) || (doorMode == DoorModeType.LockedDown) || (doorMode == DoorModeType.LockedOpen))
                    {
                        string message = "Door " + token + " is " + SimulatorConfiguration.PacsConfiguration.DoorStateList[token].DoorMode.ToString() +
                                         ". Operation denied.";

                        Log(message);
                        Transport.CommonUtils.ReturnFault("Receiver", "OperationDenied");
                    }
                    else
                    {
                        try
                        {
                            TimeSpan timeSpan = TimeSpan.Zero;
                            if (!string.IsNullOrEmpty(request.AccessTime))
                            {
                                timeSpan = System.Xml.XmlConvert.ToTimeSpan(request.AccessTime);
                            }

                            if (SimulatorConfiguration.PacsConfiguration.DoorStateList[token].DoorMode != DoorModeType.Accessed)
                            {
                                SimulatorConfiguration.PacsConfiguration.DoorAccessPreviousStateList[token] = SimulatorConfiguration.PacsConfiguration.DoorStateList[token].DoorMode;
                            }

                            SimulatorConfiguration.PacsConfiguration.DoorStateList[token].DoorMode = DoorModeType.Accessed;
                            //DoorSensorService.ProcessModeChanging(Token, DoorModeType.Accessed, ConfStorage, EventServer, StateReporter);

                            //EventServer.DoorModeEvent(this, "Changed", Token, SimulatorConfiguration.PacsConfiguration.DoorStateList[Token].DoorMode);

                            AccessToDoor func = new AccessToDoor(AccessToDoorImplementation);
                            SimulatorConfiguration.PacsConfiguration.DoorAccessList[token]++;

                            func.BeginInvoke(token, timeSpan, SimulatorConfiguration.PacsConfiguration.DoorAccessList[token], null, null);
                        }
                        catch (Exception)
                        {
                            Log(string.Format("Wrong duration ({0})", request.AccessTime));
                            Transport.CommonUtils.ReturnFault("Receiver", "InvalidArg");
                        }
                    }
                }
                else
                {
                    string message = string.Format("MomentaryAccess isn't supported for {0}.", token);
                    Log(message);

                    Transport.CommonUtils.ReturnFault("Receiver", "ActionNotSupported");
                }
            }
            else
            {
                string message = string.Format("Token {0} does not exist", token);
                Log(message);
                Transport.CommonUtils.ReturnFault("Receiver", "InvalidArg", "TokenNotFound");
            }
            EndMethod();
            return new AccessDoorResponse();
        }
    

        private delegate void AccessToDoor(string Token, TimeSpan AccessTime, int DoorAccessSesion);

        private void AccessToDoorImplementation(string Token, TimeSpan AccessTime, int DoorAccessSesion)
        {
            if (AccessTime == TimeSpan.Zero)
            {
                System.Threading.Thread.Sleep(5000);
            }
            else
            {
                System.Threading.Thread.Sleep((int)(AccessTime.TotalMilliseconds));
            }
            if (SimulatorConfiguration.PacsConfiguration.DoorStateList[Token].DoorMode == DoorModeType.Accessed)
            {
                if (DoorAccessSesion == SimulatorConfiguration.PacsConfiguration.DoorAccessList[Token])
                {
                    SimulatorConfiguration.PacsConfiguration.DoorStateList[Token].DoorMode = SimulatorConfiguration.PacsConfiguration.DoorAccessPreviousStateList[Token];
                    //EventServer.DoorModeEvent(this, "Changed", Token, SimulatorConfiguration.PacsConfiguration.DoorStateList[Token].DoorMode);
                }
            }
        }



        public void LockDoor(string Token)
        {
            BeginMethod("LockDoor");

            string operationName = "Lock";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.Lock);
            Func<DoorModeType, bool> operationAllowedCheck =
                DM => (DM == DoorModeType.LockedDown || DM == DoorModeType.LockedOpen);
            DoorModeType target = DoorModeType.Locked;
            string forbidSeverity = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationAllowedCheck, forbidSeverity, target);
            EndMethod();
        }

        public void UnlockDoor(string Token)
        {
            BeginMethod("UnlockDoor");

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
            EndMethod();
        }

        public void BlockDoor(string Token)
        {
            BeginMethod("BlockDoor");

            string operationName = "Block";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.Block);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM == DoorModeType.LockedDown || DM == DoorModeType.LockedOpen);
            DoorModeType target = DoorModeType.Blocked;
            string forbidSeverity = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
            EndMethod();
        }

        public void LockDownDoor(string Token)
        {
            BeginMethod("LockDownDoor");

            string operationName = "LockDown";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.LockDown);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM == DoorModeType.LockedOpen);
            DoorModeType target = DoorModeType.LockedDown;
            string forbidSeverity = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
            EndMethod();
        }

        public void LockDownReleaseDoor(string Token)
        {
            BeginMethod("LockDownReleaseDoor");

            string operationName = "LockDown";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.LockDown);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM != DoorModeType.LockedDown);
            DoorModeType target = DoorModeType.Locked;
            string forbidSeverity = "not applicable";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
            EndMethod();
        }

        public void LockOpenDoor(string Token)
        {
            BeginMethod("LockOpenDoor");

            string operationName = "LockOpen";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.LockOpen);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM == DoorModeType.LockedDown);
            DoorModeType target = DoorModeType.LockedOpen;
            string forbidSeverity = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
            EndMethod();
        }

        public void LockOpenReleaseDoor(string Token)
        {
            BeginMethod("LockOpenReleaseDoor");

            string operationName = "LockedOpen";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.LockOpen);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM != DoorModeType.LockedOpen);
            DoorModeType target = DoorModeType.Unlocked;
            string forbidSeverity = "not applicable";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidSeverity, target);
            EndMethod();
        }

        public void DoubleLockDoor(string Token)
        {
            BeginMethod("DoubleLockDoor");

            string operationName = "DoubleLock";
            Func<DoorCapabilities, bool> capabilitiesCheck = new Func<DoorCapabilities, bool>(DC => DC.DoubleLock);
            Func<DoorModeType, bool> operationForbiddenCheck =
                DM => (DM == DoorModeType.LockedDown || DM == DoorModeType.LockedOpen);
            DoorModeType target = DoorModeType.DoubleLocked;
            string forbidType = "denied";

            DoorOperation(Token, operationName, capabilitiesCheck, operationForbiddenCheck, forbidType, target);
            EndMethod();
        }


        #region Utility methods


        void DoorOperation(string Token,
            string operationName,
            Func<DoorCapabilities, bool> capabilitiesCheck,
            Func<DoorModeType, bool> operationForbiddenCheck,
            string notAllowedSeverity,
            Func<DoorModeType, DoorModeType> transition)
        {

            Log(string.Format("{0} operation requested for {1}", operationName, Token));

            if (SimulatorConfiguration.PacsConfiguration.DoorStateList.ContainsKey(Token))
            {
                if (capabilitiesCheck(SimulatorConfiguration.PacsConfiguration.DoorCapabilitiesList[Token]))
                {
                    DoorState doorState = SimulatorConfiguration.PacsConfiguration.DoorStateList[Token];
                    if (operationForbiddenCheck(doorState.DoorMode))
                    {

                        string message = String.Format("Door {0} is {1}. Operation {2}", Token,
                                                       doorState.DoorMode.ToString(), notAllowedSeverity);

                        Log(message);
                        Transport.CommonUtils.ReturnFault("Receiver", "ActionNotAllowed");
                    }
                    else
                    {
                        DoorModeType targetState = transition(doorState.DoorMode);
                        doorState.DoorMode = targetState;
                        //DoorSensorService.ProcessModeChanging(Token, targetState, ConfStorage, EventServer, StateReporter);
                        //EventServer.DoorModeEvent(this, "Changed", Token, doorState.DoorMode);
                    }
                }
                else
                {
                    string message = string.Format("{0} is not supported for {1}.", operationName, Token);
                    Log(message);

                    Transport.CommonUtils.ReturnFault("Receiver", "ActionNotSupported");
                }
            }
            else
            {
                string message = string.Format("Token {0} does not exist", Token);
                Log(message);
                Transport.CommonUtils.ReturnFault("Sender", "InvalidArg", "TokenNotFound");
            }

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




        #endregion


        public DoorControlServiceCapabilities GetServiceCapabilities()
        {
            BeginMethod("GetServiceCapabilities");

            DoorControlServiceCapabilities capabilities = SimulatorConfiguration.ServicesConfiguration.DoorServiceCapabilities;

            EndMethod();

            return capabilities;
        }

        #endregion
    }
}
