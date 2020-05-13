///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.GUI.Enums;

namespace ClientTestTool.Tests.Definitions.Enums
{
  public enum TestStatus
  {
    Failed,
    Passed,

    [EnumDescription("Not Detected")]
    NotDetected
  }
}
