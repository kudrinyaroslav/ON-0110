///
/// @Author Matthew Tuusberg
///

﻿using System.Linq;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
    class PtzListingSpecification : CompositeSpecification<Feature>
    {
        public override bool IsSatisfiedBy(Feature feature)
        {
            return feature.GetChildFeatures().Any(item => item.IsSupported());
        }
    }
}
