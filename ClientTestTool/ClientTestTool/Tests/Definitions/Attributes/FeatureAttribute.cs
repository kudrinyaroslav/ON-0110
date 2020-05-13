///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Attributes
{
  [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = true)]
  internal class FeatureAttribute : Attribute
  {
    public FeatureAttribute()
    {
      Type          = FeatureType.Child;
      ParentFeature = Feature.Unknown;
    }

    public FeatureType Type
    {
      get;
      set;
    }

    public Feature ParentFeature
    {
      get;
      set;
    }

  }
}
