///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Common controller methods.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Time-consuming initialization.
        /// </summary>
        void Initialize();

       
        /// <summary>
        /// Method to update context when corresponding page is left.
        /// </summary>
        void UpdateContext();
        
        /// <summary>
        /// Method to update information displayed at the view.
        /// </summary>
        void UpdateView();

        /// <summary>
        /// Method to update functions available accordingly to context data
        /// </summary>
        void UpdateViewFunctions();

    
    }
}
