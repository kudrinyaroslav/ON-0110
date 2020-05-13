///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
  class ReplayControlSpecification : CompositeSpecification<Feature>
  {
    public override bool IsSatisfiedBy(Feature feature)
    {
      return Feature.GetReplayUri.IsSupported() && (Feature.MPEG4ReplayRecording.IsSupported() ||
                                                    Feature.MJPEGReplayRecording.IsSupported() ||
                                                    Feature.H264ReplayRecording.IsSupported()); //TODO
    }
  }
}
