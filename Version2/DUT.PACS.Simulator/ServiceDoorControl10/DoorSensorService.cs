using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using DUT.PACS.Simulator.Events;

namespace DUT.PACS.Simulator.ServiceDoorControl10
{
    public class DoorSensorService
    {
        Dictionary<Sensor, UpdateDelegate> _updates;
        private Dictionary<DoorMode, DoorPhysicalState> _doorMonitorValues;
        private Dictionary<DoorMode, LockPhysicalState> _doorLockMonitorValues;
        private Dictionary<DoorMode, LockPhysicalState> _doorDoubleLockMonitorValues;

        private EventServer _eventServer;
        private ExternalLogging.StateReportingService _stateReporter;

        private delegate void UpdateDelegate(DoorState state, string doorToken, object value);

        public DoorSensorService()
        {
            InitStates();
        }
        
        void InitUpdateActions()
        {
            _updates = new Dictionary<Sensor, UpdateDelegate>();

            UpdateDelegate updateMonitor = new UpdateDelegate((s, token, value) =>
                {
                    DoorPhysicalState monitorState = (DoorPhysicalState)value;
                    s.DoorPhysicalState = monitorState;
                    _eventServer.DoorPhysicalStateEvent(null, "Changed", token, monitorState);
                    _stateReporter.ReportStateUpdate(token, s);
                });
            _updates.Add(Sensor.DoorMonitor, updateMonitor);

            UpdateDelegate updateLockMonitor = new UpdateDelegate((s, token, value) =>
                {
                    LockPhysicalState lockMonitorState = (LockPhysicalState)value;
                    s.LockPhysicalState = lockMonitorState;
                    _eventServer.LockPhysicalStateEvent(null, "Changed", token, lockMonitorState);
                    _stateReporter.ReportStateUpdate(token, s);
                });
            _updates.Add(Sensor.DoorLockMonitor, updateLockMonitor);

            UpdateDelegate updateDoubleLockMonitor = new UpdateDelegate((s, token, value) =>
                {
                    LockPhysicalState doubleLockMonitorState = (LockPhysicalState)value;
                    s.DoubleLockPhysicalState = doubleLockMonitorState;
                    _eventServer.DoubleLockPhysicalStateEvent(null, "Changed", token, doubleLockMonitorState);
                    _stateReporter.ReportStateUpdate(token, s);
                });
                _updates.Add(Sensor.DoorDoubleLockMonitor, updateDoubleLockMonitor);
        }

        void InitStates()
        {
            _doorMonitorValues = new Dictionary<DoorMode, DoorPhysicalState>();

            _doorMonitorValues.Add(DoorMode.Locked, DoorPhysicalState.Closed);
            _doorMonitorValues.Add(DoorMode.Blocked, DoorPhysicalState.Closed);
            _doorMonitorValues.Add(DoorMode.LockedDown, DoorPhysicalState.Closed);
            _doorMonitorValues.Add(DoorMode.DoubleLocked, DoorPhysicalState.Closed);

            _doorMonitorValues.Add(DoorMode.Unlocked, DoorPhysicalState.Open);
            _doorMonitorValues.Add(DoorMode.Accessed, DoorPhysicalState.Open);
            _doorMonitorValues.Add(DoorMode.LockedOpen, DoorPhysicalState.Open);

            _doorMonitorValues.Add(DoorMode.Unknown, DoorPhysicalState.Unknown);

            _doorLockMonitorValues = new Dictionary<DoorMode, LockPhysicalState>();
            _doorLockMonitorValues.Add(DoorMode.Locked, LockPhysicalState.Locked);
            _doorLockMonitorValues.Add(DoorMode.Blocked, LockPhysicalState.Locked);
            _doorLockMonitorValues.Add(DoorMode.LockedDown, LockPhysicalState.Locked);
            _doorLockMonitorValues.Add(DoorMode.DoubleLocked, LockPhysicalState.Locked);

            _doorLockMonitorValues.Add(DoorMode.Unlocked, LockPhysicalState.Unlocked);
            _doorLockMonitorValues.Add(DoorMode.Accessed, LockPhysicalState.Unlocked);
            _doorLockMonitorValues.Add(DoorMode.LockedOpen, LockPhysicalState.Unlocked);

            _doorLockMonitorValues.Add(DoorMode.Unknown, LockPhysicalState.Unknown);

            _doorDoubleLockMonitorValues = new Dictionary<DoorMode, LockPhysicalState>();

            _doorDoubleLockMonitorValues.Add(DoorMode.DoubleLocked, LockPhysicalState.Locked);

            _doorDoubleLockMonitorValues.Add(DoorMode.Locked, LockPhysicalState.Unlocked);
            _doorDoubleLockMonitorValues.Add(DoorMode.Blocked, LockPhysicalState.Unlocked);
            _doorDoubleLockMonitorValues.Add(DoorMode.LockedDown, LockPhysicalState.Unlocked);
            _doorDoubleLockMonitorValues.Add(DoorMode.Unlocked, LockPhysicalState.Unlocked);
            _doorDoubleLockMonitorValues.Add(DoorMode.Accessed, LockPhysicalState.Unlocked);
            _doorDoubleLockMonitorValues.Add(DoorMode.LockedOpen, LockPhysicalState.Unlocked);

            _doorDoubleLockMonitorValues.Add(DoorMode.Unknown, LockPhysicalState.Unknown);
            
        }

        public void ProcessModeChanging(string doorToken, 
            DoorMode mode, 
            ConfStorage storage,
            EventServer eventServer, 
            ExternalLogging.StateReportingService stateReporter)
        {
            if (_eventServer != eventServer || _stateReporter != stateReporter)
            {
                _eventServer = eventServer;
                _stateReporter = stateReporter;
                InitUpdateActions();
            }

            TriggerSettings[] settings =
                storage.TriggerConfiguration.Settings.Where(S => S.DoorToken == doorToken).ToArray();

            DoorState state = storage.DoorStateList[doorToken];

            if (settings != null)
            {
                List<TriggerSettings> triggers = new List<TriggerSettings>();

                TriggerSettings doorMonitorSettings =
                    settings.Where(S => S.Sensor == Sensor.DoorMonitor && S.DoorMode == mode).FirstOrDefault();
                if (doorMonitorSettings != null)
                {
                    triggers.Add(doorMonitorSettings);
                }

                TriggerSettings doorLockMonitorSettings =
                    settings.Where(S => S.Sensor == Sensor.DoorLockMonitor && S.DoorMode == mode).FirstOrDefault();
                if (doorLockMonitorSettings != null)
                {
                    triggers.Add(doorLockMonitorSettings);
                }

                TriggerSettings doorDoubleLockMonitorSettings =
                    settings.Where(S => S.Sensor == Sensor.DoorDoubleLockMonitor && S.DoorMode == mode).FirstOrDefault();
                if (doorDoubleLockMonitorSettings != null)
                {
                    triggers.Add(doorDoubleLockMonitorSettings);
                }

                List<TriggerSettings> orderedTriggers = triggers.OrderBy(TS => TS.Timeout).ToList();

                if (orderedTriggers.Count > 0)
                {
                    Dictionary<Sensor, object> values = new Dictionary<Sensor, object>();
                    
                    values.Add(Sensor.DoorMonitor, _doorMonitorValues[mode]);
                    values.Add(Sensor.DoorLockMonitor, _doorLockMonitorValues[mode]);
                    values.Add(Sensor.DoorDoubleLockMonitor, _doorDoubleLockMonitorValues[mode]);

                    int timeout = 0;

                    List<Action> update = new List<Action>();

                    for (int i = 0; i < orderedTriggers.Count; i++ )
                    {
                        TriggerSettings ts = orderedTriggers[i];
                        int delay = ts.Timeout - timeout;
                        if (delay > 0)
                        {
                            Action del = () => { Thread.Sleep(delay); };
                            update.Add(del);
                        }
                        Action nextAction = () => _updates[ts.Sensor](state, doorToken, values[ts.Sensor]);

                        System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1} => {2}", ts.Timeout, ts.Sensor, values[ts.Sensor]));

                        update.Add(nextAction);
                        timeout = ts.Timeout;
                    }

                    Action updateAll = new Action(
                        () =>
                            {
                                foreach (Action del in update)
                                {
                                    del.Invoke();
                                }
                            });
                    updateAll.BeginInvoke(null, null);
                }

            }
        }

    }
}
