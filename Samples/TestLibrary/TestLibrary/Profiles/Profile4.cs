﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;

namespace ProfilesTestLibrary.Profiles
{
    [ProfileDefinition]
    public class Profile2 : BaseProfileClass
    {
        public const string PROFILE_SCOPE = "onvif://www.onvif.org/Profile/NTP";

        public override string Name
        {
            get { return "Test Profile 1 - NTP"; }
        }

        public override string Scope
        {
            get { return PROFILE_SCOPE; }
        }

        protected override void InitScopes()
        {
            _scopes = new List<string>();
            _scopes.Add(PROFILE_SCOPE);
            //_scopes.Add("onvif://www.onvif.org/Scope/Scope1");
            //_scopes.Add("onvif://www.onvif.org/Scope/Scope3");
        }

        #region Functionality

        private const string CAPABILITIES = "NTP";

        protected override void LoadProfileFunctionalities()
        {
            _profileFunctionalities.AddRange(
                new FunctionalityItem[]
                    {
                        new FunctionalityItem()
                            {Functionality = Functionality.GetNTP, 
                                Path = GetFullPath(CAPABILITIES), 
                                Mandatory = true, Features = new Feature[]{Feature.NTP }},
                        new FunctionalityItem()
                            {Functionality = Functionality.SetNTP, Path = GetFullPath(CAPABILITIES),
                                Mandatory = true, Features=new Feature[]{Feature.NTP}}
                    }
                );
        }

        #endregion

        #region IProfileDefinition Members

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

                checkNextMandatory(Feature.NTP, "NTP");

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


        #endregion
    }

}
