using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchitectureSample.Views
{
    public interface IFirstTab : IView
    {
        string Value1 { get; }
        string Value2 { get;  }
    }
}
