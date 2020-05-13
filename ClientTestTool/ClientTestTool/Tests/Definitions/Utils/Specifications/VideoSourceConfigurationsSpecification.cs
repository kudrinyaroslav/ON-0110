///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Engine.Extensions;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
  class VideoSourceConfigurationsSpecification : CompositeSpecification<Feature>
  {
    public override bool IsSatisfiedBy(Feature feature)
    {
      return Feature.AddVideoSourceConfiguration.IsSupported()    &&
             Feature.ModifyVideoSourceConfiguration.IsSupported() &&
            (Feature.GetVideoSourceConfiguration.IsSupported()    ||
             Feature.GetVideoSourceConfigurations.IsSupported());

    }
  }
}
