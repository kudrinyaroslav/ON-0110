///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
  class PTZSpecification : CompositeSpecification<Feature>
  {
    public override bool IsSatisfiedBy(Feature feature)
    {
      return (Feature.GetNode.IsSupported() || Feature.GetNodes.IsSupported()) &&
              Feature.AddPtzConfiguration.IsSupported() &&
             (Feature.ContinuousMoveZoom.IsSupported() || Feature.ContinuousMovePanTilt.IsSupported()) &&
              Feature.Stop.IsSupported();
    }
  }
}
