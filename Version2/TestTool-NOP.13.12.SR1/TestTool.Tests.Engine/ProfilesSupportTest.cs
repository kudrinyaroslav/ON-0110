using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.Tests.Engine
{
    class ProfilesSupportTest
    {
        internal delegate void ProfileDefinitionCompletedHandler(IProfileDefinition profile, ProfileStatus status, string dump);

        internal event ProfileDefinitionCompletedHandler ProfileDefinitionCompleted;

        /// <summary>
        /// Checks profiles support (using algorithm defined in profile definition)
        /// </summary>
        /// <param name="profiles">List of profiles.</param>
        /// <param name="features">Supported features.</param>
        /// <param name="scopes">Scopes.</param>
        internal void CheckProfiles(IEnumerable<IProfileDefinition> profiles, IEnumerable<Feature> features, IEnumerable<string> scopes, Dictionary<string, object> parameters)
        {
            foreach (IProfileDefinition profile in profiles)
            {
                string dump = string.Empty;
                ProfileStatus status = profile.Check(out dump, features, scopes, parameters);
                ReportProfileChecked(profile, status, dump);
            }
        }

        void ReportProfileChecked(IProfileDefinition profile, ProfileStatus status, string dump)
        {
            if (ProfileDefinitionCompleted != null)
            {
                ProfileDefinitionCompleted(profile, status, dump);
            }
        }



    }
}
