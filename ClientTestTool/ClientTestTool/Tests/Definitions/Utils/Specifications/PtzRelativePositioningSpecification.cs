///
/// @Author Matthew Tuusberg
///

﻿using System.Linq;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Tests.Engine.Extensions;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
    class PtzRelativePositioningSpecification : CompositeSpecification<Feature>
    {
        public override bool IsSatisfiedBy(Feature feature)
        {
          return (Feature.GetNode.IsSupported() || Feature.GetNodes.IsSupported()) &&
                 feature.GetChildFeatures().Any(item => item.IsSupported());
        }
    }
}
