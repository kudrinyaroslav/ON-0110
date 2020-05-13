///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Engine.Extensions;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
  class MediaProfileConfigurationsSpecification : CompositeSpecification<Feature>
  {
    public override bool IsSatisfiedBy(Feature feature)
    {
      return Feature.CreateMediaProfile.IsSupported() &&
            (Feature.ListMediaProfiles.IsSupported()  ||
             Feature.GetMediaProfile.IsSupported());
             
    }
  }
}
