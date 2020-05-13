///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
namespace TestTool.GUI.Utils
{
    static class ApplicationStateHelper
    {
        /// <summary>
        /// Checks if application is not in one of "long operation" states.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool IsActive(this Enums.ApplicationState state)
        {
            return (state != Enums.ApplicationState.Idle);
        }
    }
}
