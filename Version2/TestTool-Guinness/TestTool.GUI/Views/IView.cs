///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.GUI.Controllers;

namespace TestTool.GUI.Views
{
    /// <summary>
    /// Base view definition.
    /// </summary>
    public interface IView
    {
        void SwitchToState(Enums.ApplicationState state);
        IController GetController();    
    }
}
