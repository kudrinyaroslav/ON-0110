///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.GUI.Data;

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
        /// Method to enable/disable View controls when long operation is being performed.
        /// </summary>
        /// <param name="state"></param>
        void SwitchToState(Enums.ApplicationState state);
        
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

        /// <summary>
        /// Method to update views according to saved context
        /// </summary>
        void LoadSavedContext(SavedContext context);

        /// <summary>
        /// Is raised when a long operation is started.
        /// </summary>
        event Action OperationStarted;
        
        /// <summary>
        /// Is raised when a long operation is completed.
        /// </summary>
        event Action OperationCompleted;

        /// <summary>
        /// Indicate that a time-consuming operation is being performed.
        /// </summary>
        bool RequestPending { get; }
        
        /// <summary>
        /// Stops time-consuming operation.
        /// </summary>
        void Stop();

    
    }
}
