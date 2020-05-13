using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace TestTool.GUI.Data
{
    class ConfigurationFactory
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
            set { _id = value; }
        }

        /// <summary>
        /// Initialization method
        /// </summary>
        public MethodInfo Method { get; set; }

        public object Initializer { get; set; }
    
        public Common.Configuration.SimulatorConfiguration CreateConfiguration()
        {
            return (Common.Configuration.SimulatorConfiguration)Method.Invoke(Initializer, new object[0]);
        }

    }
}
