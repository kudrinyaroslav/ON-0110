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

        public List<DoorShortInfo> Doors { get; set; }
        public string SelectedToken { get; set; }
        public string MessageTemplate { get; set; }
    }
}
