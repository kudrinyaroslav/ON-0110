using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Common.CommonUtils;

namespace TestTool.Tests.TestCases.Profiles
{
    public class BaseProfile 
    {

        #region Log

        protected void LogMandatory(StringBuilder sb, string name, bool supported)
        {
            Log(sb, name, true, supported);
        }

        protected void LogOptional(StringBuilder sb, string name, bool supported)
        {
            Log(sb, name, false, supported);
        }

        protected void Log(StringBuilder sb, string name, bool mandatory, bool supported)
        {
            //offset counting is some sort of hack.
            int count = name.Length - 3;
            if (count < 9)
            {
                count++;
            }
            int len = (count / 9) + 1;
            string offset = string.Empty;
            switch (len)
            {
                case 1:
                    offset = "\t\t\t\t";
                    break;
                case 2:
                    offset = "\t\t\t";
                    break;
                case 3:
                    offset = "\t\t";
                    break;
                case 4:
                    offset = "\t";
                    break;
            }

            sb.AppendLine(string.Format("{0} feature {1} is {2}{3}",
                                        mandatory ? "Mandatory" : "Optional",
                                        name,
                                        offset,
                                        supported ? "SUPPORTED" : "NOT SUPPORTED"));
        }

        protected void LogMandatoryFeature(StringBuilder sb, string name)
        {
            sb.AppendLine(string.Format("{0} feature is mandatory", name));
        }

        #endregion

        protected List<ProfileFeature> _features;
        
        protected List<FunctionalityItem> _profileFunctionalities;

        public bool IsFunctionalityOptional(Functionality functionality, IEnumerable<Feature> supportedFeatures)
        {
            bool optional = true;

            FunctionalityItem item = _profileFunctionalities.FirstOrDefault(F => F.Functionality == functionality);

            if (item != null && item.Features != null)
            {
                optional = false;
                foreach (Feature feature in item.Features)
                {
                    ProfileFeature f = _features.FirstOrDefault(F => F.Feature == feature);
                    if (f != null)
                    {
                        //[19.07.2013] AKS:
                        //Suppose we have profile with mandatory functionality AorB,
                        //i.e. either A or B should be supported.
                        //Suppose functionality F require feature A.
                        //There are two cases when feature A is not supported:
                        //1. AorB is supported but A is not supported(i.e. only feature B is supported) => functionality F should be colored as absent and optional
                        //2. AorB is not supported => functionality F should be colored as absent and mandatory 
                        if (ProfileFeatureState.Mandatory == f.State && FeatureUtils.IsCompoundFeature(feature))
                        {
                            optional = supportedFeatures.ContainsFeature(feature);
                            break;
                        }
                        else 
                            if (f.State == ProfileFeatureState.Optional)
                        {
                            optional = true;
                            break;
                        }
                    }
                }
            }
            return optional;
        }

        protected static string GetFullPath(params string[] levels)
        {
            return string.Join("\\", levels);
        }
    }
}
