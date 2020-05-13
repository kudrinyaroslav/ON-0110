///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Credentials used to access device.
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// Login
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Flag indicating whether UTC timestamp should be used.
        /// </summary>
        public bool UseUTCTimeStamp
        {
            get { return true; }
            set { }
        }
    }
}
