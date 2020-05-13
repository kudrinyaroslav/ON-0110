///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
    class PtzConfigurationSpecification : CompositeSpecification<Feature>
    {
        public override bool IsSatisfiedBy(Feature feature)
        {
          return Feature.AddPtzConfiguration.IsSupported();
        }

    }
}
