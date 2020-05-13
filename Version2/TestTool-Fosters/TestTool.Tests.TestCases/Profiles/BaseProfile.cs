using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;

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
        
        public bool IsFunctionalityOptional(Functionality functionality)
        {
            bool optional = true;

            FunctionalityItem item =
                _profileFunctionalities.Where(F => F.Functionality == functionality).FirstOrDefault();

            if (item != null && item.Features != null)
            {
                optional = false;
                foreach (Feature feature in item.Features)
                {
                    ProfileFeature f = _features.Where(F => F.Feature == feature).FirstOrDefault();
                    if (f != null)
                    {
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
