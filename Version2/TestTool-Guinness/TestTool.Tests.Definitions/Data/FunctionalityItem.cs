using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.Tests.Definitions.Data
{
    /// <summary>
    /// Profile functionality description
    /// </summary>
    public class FunctionalityItem
    {
        private string _path;

        /// <summary>
        /// Path
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private Functionality _functionality;
        
        /// <summary>
        /// Functionality
        /// </summary>
        public Functionality Functionality
        {
            get { return _functionality; }
            set { _functionality = value; }
        }

        //private bool _mandatory;

        ///// <summary>
        ///// True, if functionality is mandatory
        ///// </summary>
        //public bool Mandatory
        //{
        //    get { return _mandatory; }
        //    set { _mandatory = value; }
        //}

        private Feature[] _features;

        /// <summary>
        /// Features required
        /// </summary>
        public Feature[] Features
        {
            get { return _features; }
            set { _features = value; }
        }
    }
}
