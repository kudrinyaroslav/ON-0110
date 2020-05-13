using DUT.PACS.Simulator.Events;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DUT.PACS.Simulator.Common
{

    /// <summary>
    /// Service with typed properties representing application variables. 
    /// </summary>
    /// <remarks>
    /// Currently this class is used as a base class for the folowing services:
    ///  EventControlService 
    ///  MonitorService
    ///  SensorService
    ///  EventBinding [auto-generated]
    ///  NotificationProducerBinding [auto-generated]
    ///  PullPointSubscriptionBinding [auto-generated]
    ///  SubscriptionManagerBinding [auto-generated] 
    ///  PACSServiceBinding [auto-generated]
    ///  DoorControlServiceBinding [auto-generated]
    /// Possible todo: use extension methods.</remarks>
    public class BaseDutService : System.Web.Services.WebService
    {

        #region Members

        /// <summary>
        /// Storage
        /// </summary>
        ConfStorage m_ConfStorage = null;
        
        /// <summary>
        /// Event Server
        /// </summary>
        EventServer m_EventServer = null;

        /// <summary>
        /// External logger
        /// </summary>
        private ExternalLogging.LoggingService m_loggingService = null;

        #endregion //Members

        #region Configuration

        /// <summary>
        /// Storage load if this is not done yet
        /// </summary>
        public void ConfStorageLoad()
        {
            if (Application[AppVars.CONFSTORAGE] != null)
            {
                m_ConfStorage = (ConfStorage)Application[AppVars.CONFSTORAGE];
            }
            else
            {
                m_ConfStorage = new ConfStorage();
                Application[AppVars.CONFSTORAGE] = m_ConfStorage;
            }
        }

        /// <summary>
        /// Storage save
        /// </summary>
        public void ConfStorageSave()
        {
            Application[AppVars.CONFSTORAGE] = m_ConfStorage;
        }

        protected ConfStorage ConfStorage
        {
            get
            {
                return m_ConfStorage;
            }
        }

        #endregion //General
        
        #region Events
        
        public void EventServerLoad()
        {
            if (Application[AppVars.EVENTSERVER] != null)
            {
                m_EventServer = (EventServer)Application[AppVars.EVENTSERVER];
            }
            else
            {
                m_EventServer = new EventServer();
                Application[AppVars.EVENTSERVER] = m_EventServer;

                EventServer.DoorModePropertyEvent += EventServer.DoorModeEventHandler;
                EventServer.DoorPhysicalStatePropertyEvent += EventServer.DoorPhysicalStateEventHandler;
                EventServer.DoubleLockPhysicalStatePropertyEvent += EventServer.DoubleLockPhysicalStatePropertyEventHandler;
                EventServer.LockPhysicalStatePropertyEvent += EventServer.LockPhysicalStatePropertyEventHandler;
                EventServer.DoorAlarmPropertyEvent += EventServer.DoorAlarmPropertyEventHandler;
                EventServer.DoorTamperPropertyEvent += EventServer.DoorTamperPropertyEventHandler;
                EventServer.DoorFaultPropertyEvent += EventServer.DoorFaultPropertyEventHandler;

                EventServer.AccessPointEnabledPropertyEvent += EventServer.AccessPointEnabledPropertyEventHandler;
                EventServer.AccessPointTamperingPropertyEvent += EventServer.AccessPointTamperingPropertyEventHandler;
                EventServer.RequestTimeoutPropertyEvent += EventServer.RequestTimeoutEventHandler;
                EventServer.AccessControlExternalPropertyEvent += EventServer.AccessControlExternalEventHandler;

                EventServer.AccessPointEnabledPropertyEvent += EventServer.AccessPointEnabledPropertyEventHandler;

                EventServer.ConfigurationAccessProfileChangedPropertyEvent += EventServer.ConfigurationAccessProfileChangedEventHandler;
                EventServer.ConfigurationCredentialChangedPropertyEvent += EventServer.ConfigurationCredentialChangedEventHandler;
                EventServer.ConfigurationCredentialRemovedPropertyEvent += EventServer.ConfigurationCredentialRemovedEventHandler;

                EventServer.ConfigurationCredentialEnabledDisabledPropertyEvent += EventServer.ConfigurationCredentialEnabledDisabledEventHandler;
                EventServer.evConfigurationCredentialAntipassbackEvent += EventServer.ConfigurationCredentialAntipassbackEventHandler;
                EventServer.evConfigurationCredentialCredentialIdentifierEvent += EventServer.ConfigurationCredentialCredentialIdentifierEventHandler;

                // Attach logger
                m_EventServer.ExternalLogger = LoggingService;
            }
        }

        public void EventServerSave()
        {
            Application[AppVars.EVENTSERVER] = m_EventServer;
        }
        
        protected EventServer EventServer
        {
            get
            {
                if (m_EventServer == null)
                {
                    EventServerLoad();
                }
                return m_EventServer;
            }
        }

        #endregion
        
        protected ExternalLogging.LoggingService LoggingService
        {
            get
            {
                if (Application[Common.AppVars.LOGGINGSERVICE] != null)
                {
                    m_loggingService = (ExternalLogging.LoggingService)Application[Common.AppVars.LOGGINGSERVICE];
                }
                else
                {
                    m_loggingService = new ExternalLogging.LoggingService();
                    Application[Common.AppVars.LOGGINGSERVICE] = m_loggingService;
                }
                return m_loggingService;
            }
        }
        
        protected ExternalLogging.StateReportingService StateReporter
        {
            get
            {
                ExternalLogging.StateReportingService service = null;
                if (Application[Common.AppVars.STATEREPORTER] != null)
                {
                    service = (ExternalLogging.StateReportingService)Application[Common.AppVars.STATEREPORTER];
                }
                else
                {
                    service = new ExternalLogging.StateReportingService();
                    Application[Common.AppVars.STATEREPORTER] = service;
                }

                return service;
            }
        }
    
        protected T GetInfo<T>(string token,  Func<ConfStorage, Dictionary<string, T>> listSelector)
        {
            T res = default(T);

            ConfStorageLoad();

            Dictionary<string, T> list = listSelector(ConfStorage);

            if (list.ContainsKey(token))
            {
                res = list[token];
            }
            else
            {
                string message = string.Format("Token {0} does not exist", token);
                LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
                //throw FaultLib.GetSoapException(FaultType.General, message);
            }

            ConfStorageSave();

            return res;
        }

        protected bool Exists<T>(string token, Func<ConfStorage, Dictionary<string, T>> listSelector)
        {
            bool res = false;

            ConfStorageLoad();

            Dictionary<string, T> list = listSelector(ConfStorage);

            if (list.ContainsKey(token))
            {
                res = true;
            }
            else
            {
                string message = string.Format("Token {0} does not exist", token);
                LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                FaultLib.ReturnFault(message, new string[] { "Sender", "InvalidArgVal", "NotFound" });
            }

            ConfStorageSave();

            return res;
        }

       

        protected T[] GetListByTokenList<T>(string[] tokens,             
            Func<ConfStorage, Dictionary<string, T>> listSelector)
        {
            Dictionary<string, T> list;
            List<T> tempRes = new List<T>();

            ConfStorageLoad();

            list = listSelector(ConfStorage);

            if ((tokens != null) && (tokens.Count() != 0))
            {
                foreach (string token in tokens)
                {
                    if (list.ContainsKey(token))
                    {
                        if (!tempRes.Contains(list[token]))
                        {
                            tempRes.Add(list[token]);
                        }
                    }
                    //else
                    //{
                    //    string message = string.Format("Token {0} does not exist", token);
                    //    LoggingService.LogMessage(message, ExternalLogging.MessageType.Error);
                    //    throw FaultLib.GetSoapException(FaultType.General, message);
                    //}
                }
            }
            else 
            {
                tempRes.AddRange(list.Values);
            }
            ConfStorageSave();

            return tempRes.ToArray();
        }

        protected T[] GetList<T>(int offset, bool offsetSpecified,
            int limit, bool limitSpecified,
            Func<ConfStorage, Dictionary<string, T>> listSelector)
        {
            T[] res;

            ConfStorageLoad();

            res = listSelector(ConfStorage).Values.ToArray();

            if (offsetSpecified)
            {
                res = res.Skip(offset).ToArray();
            }

            if (limitSpecified)
            {
                res = res.Take(limit).ToArray();
            }

            ConfStorageSave();

            return res;
        }

    }
}
