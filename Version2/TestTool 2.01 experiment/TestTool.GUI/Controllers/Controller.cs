///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.GUI.Views;
using TestTool.GUI.Data;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Base controller.
    /// </summary>
    /// <typeparam name="T">View interface.</typeparam>
    class Controller<T> : IController
        where T: IView
    {
        private readonly T _view;
        /// <summary>
        /// View.
        /// </summary>
        protected T View
        {
            get { return _view; }
        }

        /// <summary>
        /// Performs initialization.
        /// </summary>
        /// <param name="view">View.</param>
        public Controller(T view)
        {
            _view = view;
        }

        private Enums.ApplicationState _currentState;

        /// <summary>
        /// Current application state (discovery is running, tests are being executed, 
        /// time-consumed operation is performed etc.)
        /// </summary>
        protected Enums.ApplicationState CurrentState
        {
            get { return _currentState; }
        }

        /// <summary>
        /// Switches to the current application state.
        /// </summary>
        /// <param name="state"></param>
        public virtual void SwitchToState(Enums.ApplicationState state)
        {
            _currentState = state;
            View.SwitchToState(state);
        }

        /// <summary>
        /// Updates context when a user leaves the corresponding view.
        /// </summary>
        public virtual void UpdateContext()
        {

        }

        /// <summary>
        /// Updates view data before the view is activated.
        /// </summary>
        public virtual void UpdateView()
        {

        }

        /// <summary>
        /// Updates view functions before the view is activated.
        /// </summary>
        public virtual void UpdateViewFunctions()
        {

        }

        /// <summary>
        /// Loads context data.
        /// </summary>
        /// <param name="context">Context data.</param>
        public virtual void LoadSavedContext(SavedContext context)
        {

        }

        /// <summary>
        /// Performs some time-consuming initialization.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// This event is raised when a time-consuming operation is started.
        /// </summary>
        public event Action OperationStarted;

        /// <summary>
        /// This event is raised when a time-consuming operation is completed
        /// </summary>
        public event Action OperationCompleted;

        /// <summary>
        /// Protected method to raise events in descendants.
        /// </summary>
        protected void ReportOperationStarted()
        {
            if (OperationStarted != null)
            {
                OperationStarted();
            }
        }

        /// <summary>
        /// Protected method to raise events in descendants.
        /// </summary>
        protected void ReportOperationCompleted()
        {
            if (OperationCompleted != null)
            {
                OperationCompleted();
            }
        }

        /// <summary>
        /// Indicates whether time-consuming operation is being performed.
        /// </summary>
        public virtual bool RequestPending
        {
            get { return false; }
        }

        /// <summary>
        /// Stops time-consuming operation
        /// </summary>
        public virtual void Stop()
        {

        }

    }


}
