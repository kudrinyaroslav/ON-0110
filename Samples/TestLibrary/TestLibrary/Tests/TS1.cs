using System;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Engine.Base.Definitions;

namespace ProfilesTestLibrary.Tests
{
    class TS1 : BaseDummyTest
    {
        private TestLaunchParam _parameters;

        public TS1(TestLaunchParam param)
            : base(param)
        {
            _parameters = param;
        }
                
        private const string PATH = "Engine Testing\\Advanced parameters";


        [Test(Name = "TEST 1",
            Order = "03.01.01",
            Id = "3-1-1",
            Category = Category.GENERAL,
            Path = PATH,
            Version = 1.02,
            ParametersTypes = new Type[] {typeof(Parameters.TestSettings), typeof(Parameters.SecurityTestSettings2) },
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures =  new Feature[]{Feature.GetCapabilities},
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCapabilities })]
        public void Test()
        {
            RunTest(() =>
            {

                //object myParameters = _parameters.AdvancedPrameters[typeof(Parameters.TestSettings).GUID.ToString()];
                //Parameters.TestSettings settings = (Parameters.TestSettings)myParameters;

                //this.LogTestEvent(string.Format("Value entered: {0}", settings.Text1));

                LogTestEvent("This test does nothing");

            });

        }

        [Test(Name = "TEST 10",
            Order = "03.01.10",
            Id = "3-1-2",
            Category = Category.GENERAL,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures =  new Feature[]{Feature.GetServices},
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void Test10()
        {
            RunTest(() =>
            {
                LogTestEvent("This test does nothing");
            });
        }

        [Test(Name = "TEST 11",
            Order = "03.01.11",
            Id = "3-1-11",
            Category = Category.GENERAL,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void Test11()
        {
            RunTest(() =>
            {
                LogTestEvent("This test does nothing");
            });
        }

        /*
        [Test(Name = "TEST 12",
           Order = "03.01.12",
           Id = "3-1-12",
           Category = Category.GENERAL,
           Path = PATH,
           Version = 1.02,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.GetServices },
           FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void Test12()
        {
            RunTest(() =>
            {
                LogTestEvent("This test does nothing");
            });
        }
        [Test(Name = "TEST 13",
           Order = "03.01.13",
           Id = "3-1-13",
           Category = Category.GENERAL,
           Path = PATH,
           Version = 1.02,
           RequirementLevel = RequirementLevel.Must,
           RequiredFeatures = new Feature[] { Feature.GetServices },
           FunctionalityUnderTest = new Functionality[] { Functionality.GetServices })]
        public void Test13()
        {
            RunTest(() =>
            {
                LogTestEvent("This test does nothing");
            });
        }
        */
        
        [Test(Name = "TEST 3",
            Order = "03.01.03",
            Id = "3-1-3",
            Category = Category.GENERAL,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDeviceInformation })]
        public void Test3()
        {
            RunTest(() =>
            {
                //LogTestEvent("This test does nothing");
                throw new AssertException("Some error occurred...");
            });
        }


        [Test(Name = "TEST 4",
            Order = "03.01.04",
            Id = "3-1-4",
            Category = Category.GENERAL,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDeviceInformation })]
        public void Test4()
        {
            RunTest(() =>
            {
                LogTestEvent("This test does nothing");
            });
        }


    }
}
