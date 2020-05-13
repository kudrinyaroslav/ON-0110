///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Reflection;
using System.Collections.Generic;
using TestTool.Tests.Common.Enums;

namespace TestTool.Tests.Common.TestEngine
{
    /// <summary>
    /// Test information
    /// </summary>
    public class TestInfo
    {
        /// <summary>
        /// Corresponding method
        /// </summary>
        public MethodInfo Method { get; set;}
        /// <summary>
        /// Group in the tree
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// Test name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Test order (number with leading zeros)
        /// </summary>
        public string Order { get; set; }

        public string Id { get; set; }
        public Category Category { get; set; }

        /// <summary>
        /// Flag indicating whether the test is interactive
        /// </summary>
        public bool Interactive { get; set; }

        public double Version { get; set; }

        /// <summary>
        /// Requirement level.
        /// </summary>
        public RequirementLevel RequirementLevel { get; set; }

        List<Feature> _requiredFeatures = new List<Feature>();

        public List<Feature> RequiredFeatures
        {
            get { return _requiredFeatures; }
        }
    
    }
}
