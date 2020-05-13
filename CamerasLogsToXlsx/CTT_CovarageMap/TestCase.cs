using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTT_CovarageMap
{
    public class TestCase
    {
        string _subfeature;

        public string Subfeature
        {
            get { return _subfeature; }
            set { _subfeature = value; }
        }

        string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        RequirementLevel _requirementLevel;

        public RequirementLevel RequirementLevel
        {
            get { return _requirementLevel; }
            set { _requirementLevel = value; }
        }

        string _linkToProfile;

        public string LinkToProfile
        {
            get { return _linkToProfile; }
            set { _linkToProfile = value; }
        }

        public TestCase(string name, string id, RequirementLevel requirementLevel, string subfeature, string linkToProfile)
        {
            this.Id = id;
            this.Name = name;
            this.RequirementLevel = requirementLevel;
            this.Subfeature = subfeature;
            this.LinkToProfile = linkToProfile;
        }
    }
}
