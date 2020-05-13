using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.PACS.Simulator.ServiceDoorControl10;

namespace DUT.PACS.Simulator.Events
{
    public abstract class DoorPropertyEventArgs : PropertyEventArgs
    {
        #region Memebers

        private DoorControlService m_doorControlService;
        private string m_doorToken;

        #endregion

        #region Properties

        public DoorControlService DoorControlService
        {
            get { return m_doorControlService; }
        }


        public string DoorToken
        {
            get { return m_doorToken; }
        }

        public abstract string PropertyName{ get;}
        public abstract string PropertyValue { get; }

        #endregion //Properties


        public DoorPropertyEventArgs(DoorControlService doorControlService, 
            DateTime utcTime, 
            string propertyOperation, 
            string doorToken)
            :base(utcTime, propertyOperation)
        {
            m_doorControlService = doorControlService;
            m_doorToken = doorToken;
        }


        public override Dictionary<string, string> GetData()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add(PropertyName, PropertyValue);
            return properties;
        }

        public override Dictionary<string, string> GetSource()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("DoorToken", DoorToken);
            return properties;
        }
    }
}
