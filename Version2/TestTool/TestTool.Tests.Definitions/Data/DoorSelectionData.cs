using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.Definitions.Data
{
    public class DoorSelectionData
    {
        public class DoorShortInfo
        {
            public string Token { get; set; }
            public string Name { get; set; }
            public string DisplayName 
            {
                get 
                {
                    return string.IsNullOrEmpty(Name) ? Token : string.Format("{0} ({1})", Token, Name); 
                }
            }
        }

        private List<DoorShortInfo> m_Doors;
        public IEnumerable<DoorShortInfo> Doors
        {
            get { return m_Doors; }
            set { m_Doors = value.ToList(); }
        }
        public string SelectedToken { get; set; }
        public string MessageTemplate { get; set; }
    }
}
