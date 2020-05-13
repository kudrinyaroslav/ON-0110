using System;
using Common;

namespace TestSuite001
{
    public class TestsCollection3 : TestsCollectionBase
    {
        public TestsCollection3(IPrimitivesCollection primitivesCollection):
            base(primitivesCollection)
        {

        }

        [Category("Blue")]
        [Name("Test 03.001")]
        [Group("01\\03")]
        public void Sequence3_1()
        {
            ExecuteCommand1();
            ExecuteCommand2();
            ExecuteCommand3();
        }

        [Category("Black")]
        [Name("Test with Exception")]
        [Group("01\\03")]
        public void MethodWithException()
        {
            throw new Exception("An error occurred! ");
        }
    }
}
