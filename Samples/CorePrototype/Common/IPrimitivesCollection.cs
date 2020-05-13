using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IPrimitivesCollection
    {
        void ExecuteCommand1();
        void ExecuteCommand2();
        void ExecuteCommand3();
        string GetString1();
        string GetString2(string param);

    }
}
