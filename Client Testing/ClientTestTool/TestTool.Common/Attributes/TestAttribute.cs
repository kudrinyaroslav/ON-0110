///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;

namespace TestTool.Common.Attributes
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

        private string _path = string.Empty;

        /// <summary>
        /// Path to the test in the tree
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
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


    }
}
