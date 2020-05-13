///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////

namespace TestTool.GUI.Data
{
    /// <summary>
    /// Values which can be defined by operator.
    /// </summary>
    public class TestSettings
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TestSettings()
        {
            UseEmbeddedPassword = true;
            OperationDelay = 5000;

            Password1 = "OnvifTest123";
            Password2 = "OnvifTest321";
        }

        /// <summary>
        /// PTZ node
        /// </summary>
        public string PTZNodeToken { get; set; }
        
        /// <summary>
        /// Flag indicating whether default passwords can be used.
        /// </summary>
        public bool UseEmbeddedPassword { get; set; }
        /// <summary>
        /// Password for first operation
        /// </summary>
        public string Password1 { get; set; }
        /// <summary>
        /// Password for second operation
        /// </summary>
        public string Password2 { get; set; }
        /// <summary>
        /// Operation delay
        /// </summary>
        public int OperationDelay { get; set; }
    }
}
