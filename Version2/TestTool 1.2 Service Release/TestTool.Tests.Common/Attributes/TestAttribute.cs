///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;

namespace TestTool.Tests.Common.Attributes
{
    /// <summary>
    /// Attribute to mark the method containing test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAttribute : System.Attribute
    {
        private string _name;

        /// <summary>
        /// Test name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _order;

        /// <summary>
        /// Test order (test number with leading zeros)
        /// </summary>
        public string Order
        {
            get { return _order; }
            set { _order = value; }
        }

        private string _path;

        /// <summary>
        /// Path to the test in the tree
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private Category _category;

        /// <summary>
        /// 
        /// </summary>
        public Category Category
        {
            get { return _category; }
            set { _category = value;}
        }

        private string _id;

        /// <summary>
        /// 
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value;}
        }

        private RequirementLevel _requirementLevel;

        /// <summary>
        /// Requirement level
        /// </summary>
        public RequirementLevel RequirementLevel
        {
            get { return _requirementLevel; }
            set { _requirementLevel = value; }
        }

        private Feature[] _features =new Feature[0];
        public Feature[] RequiredFeatures
        {
            get { return _features;}
            set { _features = value;}
        }
        
        private bool _interactive;

        /// <summary>
        /// Indicates whether test requires user interaction.
        /// </summary>
        public bool Interactive
        {
            get { return _interactive; }
            set { _interactive = value; }
        }

        private double _version;
        
        /// <summary>
        /// Test version.
        /// </summary>
        public  double Version
        {
            get { return _version; }
            set { _version = value; }
        }

    }
}
