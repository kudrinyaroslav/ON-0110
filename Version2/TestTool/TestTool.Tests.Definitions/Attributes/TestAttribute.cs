///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Linq;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.Tests.Definitions.Attributes
{
    /// <summary>
    /// Attribute to mark the method containing test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAttribute : System.Attribute
    {
        public TestAttribute()
            :base()
        {
            _executionOrder = TestExecutionOrder.Normal;
        }

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
            get
            {
                if (!string.IsNullOrEmpty(_order))
                    return _order;

                if (string.IsNullOrEmpty(Id)) return null;

                var parts = Id.Split('-').Select(e => new string('0', Math.Max(0, 2 - e.Length)) + e + ".");

                return string.Concat(parts).TrimEnd('.');
            }
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

        private TestExecutionOrder _executionOrder;

        public TestExecutionOrder ExecutionOrder
        {
            get { return _executionOrder; }
            set { _executionOrder = value; }
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

        private string _LastChangedIn;
        public string LastChangedIn
        {
            get { return _LastChangedIn ?? "v14.12"; }
            set { _LastChangedIn = value; }
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

        Functionality[] _functionalityUnderTest = new Functionality[0];
        public Functionality[] FunctionalityUnderTest
        {
            get { return _functionalityUnderTest; }
            set { _functionalityUnderTest = value;}
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

        private Type[] _parametersTypes = new Type[0];
        /// <summary>
        /// "Advanced" parameters type
        /// </summary>
        public Type[] ParametersTypes
        {
            get { return _parametersTypes; }
            set { _parametersTypes = value; }
        }
    }
}
