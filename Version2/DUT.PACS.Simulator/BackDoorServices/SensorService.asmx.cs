using System;
using System.Linq;
using System.Web.Services;
using DUT.PACS.Simulator.Common;
using DUT.PACS.Simulator.ServiceDoorControl10;
using DUT.PACS.Simulator.ServiceCredential10;

namespace DUT.PACS.Simulator.BackDoorServices
{
    /// <summary>
    /// Summary description for SensorService
    /// </summary>
    [WebService(Namespace = "http://www.onvif.org/simulator/sensor")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SensorService : BaseDutService
    {
        [WebMethod]
        public void SignalReceived(string deviceToken, string deviceType, string sensor, string value)
        {
            ConfStorageLoad();

            LoggingService.LogMessage(string.Format("Signal received: token={0}, deviceType={1} sensor={2}, value={3}", deviceToken, deviceType, sensor, value), ExternalLogging.MessageType.Details);

            if (deviceType == "Door")
            {

                DoorState doorState = null;
                if (ConfStorage.DoorStateList.ContainsKey(deviceToken))
                {
                    doorState = ConfStorage.DoorStateList[deviceToken];
                }

                switch (sensor)
                {
                    case "Alarm":
                        {
                            DoorAlarmState state = (DoorAlarmState)Enum.Parse(typeof(DoorAlarmState), value);
                            if (doorState != null)
                            {
                                doorState.Alarm = state;
                                EventServer.DoorAlarmMonitorEvent(null, "Changed", deviceToken, state);
                            }
                        }
                        break;
                    case "LockPhysicalState":
                        {
                            LockPhysicalState state =
                                (LockPhysicalState)Enum.Parse(typeof(LockPhysicalState), value);
                            if (doorState != null)
                            {
                                doorState.LockPhysicalState = state;
                                EventServer.LockPhysicalStateEvent(null, "Changed", deviceToken, state);
                            }
                        }
                        break;
                    case "DoubleLockPhysicalState":
                        {
                            LockPhysicalState state =
                                (LockPhysicalState)Enum.Parse(typeof(LockPhysicalState), value);

                            if (doorState != null)
                            {
                                doorState.DoubleLockPhysicalState = state;
                                EventServer.DoubleLockPhysicalStateEvent(null, "Changed", deviceToken, state);
                            }
                        }
                        break;
                    case "DoorPhysicalState":
                        {
                            DoorPhysicalState state =
                                (DoorPhysicalState)Enum.Parse(typeof(DoorPhysicalState), value);

                            if (doorState != null)
                            {
                                doorState.DoorPhysicalState = state;
                                EventServer.DoorPhysicalStateEvent(null, "Changed", deviceToken, state);
                            }

                        }
                        break;
                    case "Tamper":
                        {
                            DoorTamperState state =
                                (DoorTamperState)Enum.Parse(typeof(DoorTamperState), value);

                            if ((doorState != null) && (doorState.Tamper != null))
                            {
                                doorState.Tamper.State = state;
                                EventServer.DoorTamperMonitorEvent(null, "Changed", deviceToken, state);
                            }

                        }
                        break;
                    case "Fault":
                        {
                            DoorFaultState state =
                                (DoorFaultState)Enum.Parse(typeof(DoorFaultState), value);

                            if ((doorState != null) && (doorState.Fault != null))
                            {
                                doorState.Fault.State = state;
                                EventServer.DoorFaultEvent(null, "Changed", deviceToken, state);
                            }

                        }
                        break;
                }

                StateReporter.ReportStateUpdate(deviceToken, doorState);
            }

            if (deviceType == "Credential")
            {
                CredentialState credentialState = null;
                if (ConfStorage.CredentialStateList.ContainsKey(deviceToken))
                {
                    credentialState = ConfStorage.CredentialStateList[deviceToken];
                }

                switch (sensor)
                {
                    case "AntipassbackViolated":
                        {
                            bool state = (value == "True");
                            if (credentialState != null)
                            {
                                credentialState.AntipassbackState.AntipassbackViolated = state;
                                //TODO: event
                            }
                        }
                        break;
                    
                }

            }
        }



    }
}
