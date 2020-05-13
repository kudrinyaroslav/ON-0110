///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
  class ZeroConfigurationSpecification : CompositeSpecification<Feature>
  {
    public override bool IsSatisfiedBy(Feature feature)
    {
      return true;
    }
  }
}
