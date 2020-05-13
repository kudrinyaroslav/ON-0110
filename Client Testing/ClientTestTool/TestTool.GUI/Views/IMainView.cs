using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.GUI.Views
{
    interface IMainView : IView
    {
        void UpdateFormTitle(string address);
    }
}
