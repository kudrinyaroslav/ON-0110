using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace TestEngineProto
{
    class PrimitivesCollection : IPrimitivesCollection
    {
        private IListener _listener;
        public PrimitivesCollection(IListener listener)
        {
            _listener = listener;
        }

        #region IPrimitivesCollection Members

        public void ExecuteCommand1()
        {
            _listener.Write("ExecuteCommand1");
        }

        public void ExecuteCommand2()
        {
            _listener.Write("ExecuteCommand2");
        }

        public void ExecuteCommand3()
        {
            _listener.Write("ExecuteCommand3");
        }

        public string GetString1()
        {
            _listener.Write("GetString1");
            return "Hello world!";
        }

        public string GetString2(string param)
        {
            _listener.Write(string.Format("GetString2({0})", param));
            return "Hello World!";
        }

        #endregion
    }
}
