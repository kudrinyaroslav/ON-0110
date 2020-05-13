///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Reflection;
using System.Collections.Generic;
using TestTool.Tests.Definitions.Enums;
using System.Xml.Serialization;

namespace TestTool.Tests.Definitions.Data
{
    /// <summary>
    /// Process type
    /// </summary>
    public enum ProcessType
    {
        /// <summary>
        /// Test (default)
        /// </summary>
        Test,
        /// <summary>
        /// Feature definition process.
        /// </summary>
        FeatureDefinition
    }



    /// <summary>
    /// Test information
    /// </summary>
    [Serializable]
    public class TestInfo
    {
        public TestInfo()
        {
            ProcessType = ProcessType.Test;
        }

        /// <summary>
        /// Corresponding method
        /// </summary>
        [XmlIgnore]
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
        [XmlIgnore]
        public string Order { get; set; }

        /// <summary>
        /// Execution order (at the beginning / no matter / at the end
        /// </summary>
        [XmlIgnore]
        public TestExecutionOrder ExecutionOrder { get; set; }

        /// <summary>
        /// Type of the process
        /// </summary>
        public ProcessType ProcessType { get; set; }

        /// <summary>
        /// Test case identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Test category
        /// </summary>
        public Category Category { get; set; }
        
        /// <summary>
        /// Test version
        /// </summary>
        [XmlIgnore]
        public double Version { get; set; }

        /// <summary>
        /// Requirement level.
        /// </summary>
        public RequirementLevel RequirementLevel { get; set; }

        /// <summary>
        /// List of required features.
        /// </summary>
        List<Feature> _requiredFeatures = new List<Feature>();

        /// <summary>
        /// List of require features
        /// </summary>
        public List<Feature> RequiredFeatures
        {
            get { return _requiredFeatures; }
        }
    
        List<Functionality> _functionalityUnderTest = new List<Functionality>();

        /// <summary>
        /// Functionality under testing
        /// </summary>
        public List<Functionality> FunctionalityUnderTest
        {
            get { return _functionalityUnderTest; }
        }
    }
}
