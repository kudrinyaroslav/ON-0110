///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
    class PtzContinuousPositioningSpecification : CompositeSpecification<Feature>
    {
        public override bool IsSatisfiedBy(Feature feature)
        {
          return Feature.Stop.IsSupported() &&
                (Feature.ContinuousMovePanTilt.IsSupported() ||
                 Feature.ContinuousMoveZoom.IsSupported());
        }
    }
}
