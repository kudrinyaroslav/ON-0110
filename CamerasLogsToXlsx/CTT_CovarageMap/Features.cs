using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTT_CovarageMap
{
    public enum RequirementLevel
    { 
        Mandatory,
        Conditional,
        Optional,
        Supplimentary,
        None
    }

    public class Feature
    {

        List<TestCase> _testCases = new List<TestCase>();

        internal List<TestCase> TestCases
        {
            get { return _testCases; }
            set { _testCases = value; }
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

        public Feature(string name, string id, RequirementLevel requirementLevel, string linkToProfile)
        {
            this.Id = id;
            this.Name = name;
            this.RequirementLevel = requirementLevel;
            this.LinkToProfile = linkToProfile;
        }


    }
}
