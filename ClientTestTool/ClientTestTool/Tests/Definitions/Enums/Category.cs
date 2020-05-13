///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.GUI.Enums;

namespace ClientTestTool.Tests.Definitions.Enums
{
  /// <summary>
  /// Test category
  /// </summary>
  public enum Category
  {
    [EnumDescription("Core Test Cases")]
    Core,

    [EnumDescription("Profile S Test Cases")]
    ProfileS,
    
    [EnumDescription("Profile G Test Cases")]
    ProfileG,

    [EnumDescription("Profile C Test Cases")]
    ProfileC
  }
}
