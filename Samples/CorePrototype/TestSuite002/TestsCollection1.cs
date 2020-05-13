using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace TestSuite002
{
    public class TestsCollection1 : TestsCollectionBase
    {
        public TestsCollection1(IPrimitivesCollection primitivesCollection)
            : base(primitivesCollection)
        {
        }

        [Category("Green")]
        [Name("Test 01.001")]
        [Group("02\\01")]
        public void Sequence1_1()
        {
            ExecuteCommand1();
            ExecuteCommand2();
        }

        [Category("Red")]
        [Name("Test 01.002")]
        [Group("02\\01")]
        public void Sequence1_3()
        {
            ExecuteCommand3();
            ExecuteCommand1();
        }

    }
}
