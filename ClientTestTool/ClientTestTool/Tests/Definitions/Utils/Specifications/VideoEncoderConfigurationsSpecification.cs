///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Engine.Extensions;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
  class VideoEncoderConfigurationsSpecification : CompositeSpecification<Feature>
  {
    public override bool IsSatisfiedBy(Feature feature)
    {
      return Feature.ModifyVideoEncoderConfiguration.IsSupported() &&
            (Feature.GetVideoEncoderConfiguration.IsSupported()    ||
             Feature.GetVideoSourceConfigurations.IsSupported());
             
    }
  }
}
