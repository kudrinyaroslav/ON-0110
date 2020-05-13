using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    [TestSuite]
    public class TestsCollectionBase
    {
        private IPrimitivesCollection _primitivesCollection;

        public TestsCollectionBase(IPrimitivesCollection primitivesCollection)
        {
            _primitivesCollection = primitivesCollection;
        }

        protected void ExecuteCommand1()
        {
            _primitivesCollection.ExecuteCommand1();
        }

        protected void ExecuteCommand2()
        {
            _primitivesCollection.ExecuteCommand2();
        }

        protected void ExecuteCommand3()
        {
            _primitivesCollection.ExecuteCommand3();
        }

        protected string GetString1()
        {
            return _primitivesCollection.GetString1();
        }

        protected string GetString2(string param)
        {
            return _primitivesCollection.GetString2(param);
        }

    }
}
