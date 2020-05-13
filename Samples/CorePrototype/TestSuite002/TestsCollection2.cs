using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace TestSuite002
{
    class TestsCollection2 : TestsCollectionBase
    {
        public TestsCollection2(IPrimitivesCollection primitivesCollection)
            : base(primitivesCollection)

        {
        }

        [Category("Yellow")]
        [Name("Test 02.001")]
        [Group("02\\02")]
        public void Sequence2_1()
        {
            ExecuteCommand1();
            ExecuteCommand2();
        }

        [Category("Red")]
        [Name("Test 02.002")]
        [Group("02\\02\\01")]
        public void Sequence2_2()
        {
            ExecuteCommand2();
            ExecuteCommand3();
        }

        [Category("Red")]
        [Name("Test 02.003")]
        [Group("02\\02")]
        public void Sequence2_3()
        {
            ExecuteCommand3();
            ExecuteCommand1();
        }


    }
}
