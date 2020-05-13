using System.Collections.Generic;
using System.Reflection;

namespace CameraClient.Tests
{
    class TestSuiteParameters
    {
        public TestSuiteParameters()
        {
            _testCases = new List<MethodInfo>();
        }

        private List<MethodInfo> _testCases;
        public  List<MethodInfo> TestCases
        {
            get { return _testCases; }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

    }
}
