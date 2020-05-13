///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
    class RelayOutputsSpecification : CompositeSpecification<Feature>
    {
        public override bool IsSatisfiedBy(Feature feature)
        {
          return (Feature.GetRelayOutputs.IsSupported() && Feature.SetRelayOutputState.IsSupported()) &&
                 (Feature.SetRelayOutputBistable.IsSupported() || Feature.SetRelayOutputMonostable.IsSupported());
        }
    }
}
