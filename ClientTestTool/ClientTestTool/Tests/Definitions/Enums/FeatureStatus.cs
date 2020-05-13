///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.GUI.Enums;

namespace ClientTestTool.Tests.Definitions.Enums
{
  public enum FeatureStatus
  {
    Undefined,
    Supported,

    [EnumDescription("Not Supported")]
    NotSupported
  }
}
