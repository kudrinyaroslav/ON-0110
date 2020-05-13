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
    internal interface IView
    {
        IController GetController();

        void ReportError(string message);
    }
}
