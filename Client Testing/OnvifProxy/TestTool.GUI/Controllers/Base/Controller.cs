///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.GUI.Views;

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
        /// Performs some time-consuming initialization.
        /// </summary>
        public virtual void Initialize()
        {

        }


    }


}
