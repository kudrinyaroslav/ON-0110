using System.Collections.Generic;
using System.Linq;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.Tests.Engine
{
    class ConformanceLogicHandler
    {
        public static List<TestInfo> GetTestsByFeatures(IEnumerable<TestInfo> allTestCases, 
            IEnumerable<Feature> features, bool conformance)
        {
            List<TestInfo> tests = new List<TestInfo>();
            IEnumerable<TestInfo> baseList = null;
            if (conformance)
            {
                baseList = allTestCases.Where(
                    TI => TI.RequirementLevel == RequirementLevel.Must);
            }
            else
            {
                baseList =  allTestCases;
            }

            foreach (TestInfo info in baseList)
            {
                bool supported = true;
                foreach (Feature feature in info.RequiredFeatures)
                {
                    bool local = features.ContainsFeature(feature);
                    if (!local)
                    {
                        supported = false;
                        break;
                    }
                }

                if (supported)
                {
                    tests.Add(info);
                }
            }
            return tests;
        }

    }

}
