using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace TestSuite001
{
    public class TestsCollection : TestsCollectionBase
    {

        public TestsCollection(IPrimitivesCollection primitivesCollection):
            base(primitivesCollection)
        {
        }

        [Category("Green")]
        [Name("Test 00.001")]
        [Group("01\\00")]
        public void Sequence0_1()
        {
            ExecuteCommand1();
            ExecuteCommand2();
        }

        [Category("Yellow")]
        [Name("Test 00.002")]
        [Group("01\\00")]
        public void Sequence0_2()
        {
            GetString2("Hello World!");
        }

        [Category("Red")]
        [Name("Test 00.003")]
        [Group("01\\00")]
        public void Sequence0_3()
        {
            ExecuteCommand3();
            ExecuteCommand1();
        }

    }
}
