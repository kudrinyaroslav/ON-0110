using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.Tests.Engine.Base.Definitions
{
    public enum FeatureDefinitionMode
    {
        Default,
        AllSupported,
        AllNotSupported
    }

    public class FeaturesList
    {
        public FeaturesList()
        {
            _features = new List<Feature>();
            _mode = FeatureDefinitionMode.Default;
        }

        private FeatureDefinitionMode _mode;
        public FeatureDefinitionMode Mode
        {
            get { return _mode; }
            set { _mode = value;}
        }

        /// <summary>
        /// Features selected for test.
        /// </summary>
        private List<Feature> _features;

        /// <summary>
        /// Features selected by the operator.
        /// </summary>
        //public List<Feature> Features
        //{
        //    get { return _features; }
        //}
        
        public bool Contains(Feature feature)
        {
            return ContainsFeature(feature);
        }

        public bool ContainsFeature(Feature feature)
        {
            switch (_mode)
            {
                case FeatureDefinitionMode.AllSupported:
                    return true;
                    break;
                case FeatureDefinitionMode.AllNotSupported:
                    return false;
                    break;
            }
            return _features.ContainsFeature(feature);
        }

        public void AddRange(IEnumerable<Feature> features)
        {
            _features.AddRange(features);
        }
    }
}
