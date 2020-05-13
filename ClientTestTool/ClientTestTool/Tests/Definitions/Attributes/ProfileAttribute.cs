///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Attributes
{
  [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = true)]
  class ProfileAttribute : Attribute
  {
    public Profile Profile
    {
      get;
      set;
    }

    public RequirementLevel RequirementLevel
    {
      get;
      set;
    }

  }
}
