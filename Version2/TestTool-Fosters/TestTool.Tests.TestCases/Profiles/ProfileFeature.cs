using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.Tests.TestCases.Profiles
{
    public enum ProfileFeatureState
    {
        Mandatory,
        ProfileMandatory,
        Optional
    }

    public class ProfileFeature
    {
        public Feature Feature { get; set; }
        public ProfileFeatureState State { get; set; }
    }

}
