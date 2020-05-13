
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;

namespace ProfilesTestLibrary.Profiles
{
    [ProfileDefinition]
    public class Profile1 : BaseProfileClass
    {
        public const string PROFILE_SCOPE = "onvif://www.onvif.org/Profile/capabilities";

        public override string Name
        {
            get
            {
                return "Test Profile 1 - Capabilities";
            }
        }

        public override string Scope
        {
            get
            {
                return PROFILE_SCOPE;
            }
        }

        public override ProfileStatus Check(out string reason, System.Collections.Generic.IEnumerable<Feature> features, System.Collections.Generic.IEnumerable<string> scopes)
        {
            reason = string.Empty;


            StringBuilder sb = new StringBuilder();
            ProfileStatus status = ProfileStatus.NotSupported;

            sb.AppendLine(string.Format("Check profile support for {0}", Name));

            bool scopePresent = scopes.Contains(Scope);
            sb.AppendLine(string.Format("Scope {0}: \t\t{1}", Scope, scopePresent ? "PRESENT" : "NOT PRESENT"));

            if (!scopePresent)
            {
                sb.AppendFormat("Profile not supported");
            }
            else
            {
                bool profileOk = true;
                bool supported;

                Action<Feature, string> checkNextMandatory = new Action<Feature, string>(
                    (feature, displayName) =>
                    {
                        supported = features.Contains(feature);
                        LogMandatory(sb, displayName, supported);
                        profileOk = profileOk && supported;
                    });

                Action<Feature, string> checkNextOptional = new Action<Feature, string>(
                    (feature, displayName) =>
                    {
                        supported = features.Contains(feature);
                        LogOptional(sb, displayName, supported);
                    });

                if (profileOk)
                {
                    status = ProfileStatus.Supported;
                }
                else
                {
                    status = ProfileStatus.Failed;
                }
            }
            reason = sb.ToString();
            return status;
        }

        protected override void InitScopes()
        {
            _scopes = new List<string>();
            _scopes.Add(PROFILE_SCOPE);
        }

        protected const string SYSTEM = "System";

        protected override void LoadProfileFunctionalities()
        {
            _profileFunctionalities.AddRange(
                    new FunctionalityItem[]
                        {
                            new FunctionalityItem(){Functionality = Functionality.GetServices, Path = SYSTEM, Mandatory = true},
                            new FunctionalityItem(){Functionality = Functionality.GetCapabilities, Path = SYSTEM, Mandatory = true},
                            new FunctionalityItem(){Functionality = Functionality.GetDeviceServiceCapabilities, Path = SYSTEM, Mandatory = true},
                            new FunctionalityItem(){Functionality = Functionality.GetDeviceInformation, Path = SYSTEM, Mandatory = true}
                        });
        }
    }
}
