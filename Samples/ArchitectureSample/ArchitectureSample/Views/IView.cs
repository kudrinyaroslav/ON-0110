using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchitectureSample.Views
{
    public interface IView
    {
        void BeginLongOperation();
        void EndLongOperation();
    }
}
