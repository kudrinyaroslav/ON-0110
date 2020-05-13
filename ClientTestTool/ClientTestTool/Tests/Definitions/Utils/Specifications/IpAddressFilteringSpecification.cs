///
/// @Author Matthew Tuusberg
///

﻿using System.Linq;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Base;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Utils.Specifications
{
  class IpAddressFilteringSpecification : CompositeSpecification<Feature>
  {
    public override bool IsSatisfiedBy(Feature feature)
    {
      if (!Feature.GetIpAddressFilter.IsSupported())
        return false;

      return (Feature.AddIpV4AddressFilter.IsSupported() && Feature.SetIpV4AddressFilter.IsSupported() && Feature.RemoveIpV4AddressFilter.IsSupported()) ||
             (Feature.AddIpV6AddressFilter.IsSupported() && Feature.SetIpV6AddressFilter.IsSupported() && Feature.RemoveIpV6AddressFilter.IsSupported());
    }
  }
}
