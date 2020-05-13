///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Engine.Extensions;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
  class MediaStreamingSpecification : CompositeSpecification<Feature>
  {
    public override bool IsSatisfiedBy(Feature feature)
    {
      return Feature.GetProfiles.IsSupported()  &&
             Feature.GetStreamURI.IsSupported() &&
            (Feature.RTSPStreaming.IsSupported() ||
             Feature.UDP.IsSupported()           ||
             Feature.HTTP.IsSupported());
    }
  }
}
