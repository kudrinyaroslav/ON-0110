using System;
using System.Collections.Generic;
using System.Linq;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Features;

namespace TestTool.Tests.Engine
{
    partial class FeaturesDefinitionProcess
    {
        protected string _secureOperation;
        protected string _ptzNode;
        protected string _recordingToken;
        
        private List<Feature> _features;
        private FeaturesSet _featuresSet;

        private static TestInfo _descriptor;
        public static TestInfo This
        {
            get
            {
                if (_descriptor == null)
                {
                    Type t = typeof(FeaturesDefinitionProcess);
                    _descriptor = new TestInfo
                        {
                            Name = "DEFINE FEATURES",
                            Method = t.GetMethod("RunFeatureDefinition"),
                            ProcessType = ProcessType.FeatureDefinition,
                            ExecutionOrder = TestExecutionOrder.FeatureDefinition
                        };
                }
                return _descriptor;
            }
        }

        private bool _warning = false;
        public bool Warning
        {
            get { return _warning; }
        }

        public int MaxPullPoints { get; protected set; }
        
        #region events

        public event Action<Feature, bool> FeatureDefined;

        public event Action<Feature> FeatureDefinitionFailed;

        public event Action<string> ScopeDefined;

        public event Action<DeviceInformation> DeviceInformationReceived;

        #endregion


    }

}
