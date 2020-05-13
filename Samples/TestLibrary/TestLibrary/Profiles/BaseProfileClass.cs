using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Interfaces;


namespace ProfilesTestLibrary
{

    public abstract class BaseProfileClass : IProfileDefinition
    {

        public BaseProfileClass()
        {
            
        }

        public virtual string Name
        {
            get { return "Base profile "; }
        }

        #region 

        protected List<string> _scopes;

        protected List<FunctionalityItem> _profileFunctionalities;

        protected string GetFullPath(string path)
        {
            return string.Format("Profile Specific Requirements\\{0}", path);
        }
        
        protected virtual void LoadProfileFunctionalities()
        {

        }

        #endregion

        #region IProfileDefinition Members

        public IEnumerable<string> MandatoryScopes
        {
            get
            {
                if (_scopes == null)
                {
                    InitScopes();
                }
                return _scopes;
            }
        }

        protected virtual void InitScopes()
        {
            _scopes = new List<string>();
        }

        public IEnumerable<FunctionalityItem> Functionalities
        {
            get
            {
                if (_profileFunctionalities == null)
                {
                    _profileFunctionalities = new List<FunctionalityItem>();
                    LoadProfileFunctionalities();
                }
                return _profileFunctionalities;
            }
        }
        
        #endregion

        #region IProfileDefinition Members


        public virtual string Scope
        {
            get { return string.Empty; }
        }

        public virtual ProfileStatus Check(out string reason, IEnumerable<Feature> features, IEnumerable<string> scopes)
        {
            reason = string.Empty;
            return scopes.Contains(Scope) ? ProfileStatus.Supported : ProfileStatus.NotSupported;
        }

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
    }


}
