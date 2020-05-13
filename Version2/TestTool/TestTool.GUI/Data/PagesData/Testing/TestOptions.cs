///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Tests page data.
    /// </summary>
    public class TestOptions
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TestOptions()
        {
            Tests = new List<string>();
            Groups = new List<string>(); 
        }

        /// <summary>
        /// Tests selected.
        /// </summary>
        public List<string> Tests { get; private set; }
        /// <summary>
        /// Groups selected.
        /// </summary>
        public List<string> Groups { get; private set; }

        public bool Repeat { get; set; }
    }
}
