///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.GUI.Enums;

namespace ClientTestTool.Data.Definitions.Interfaces
{
  public interface IUnit
  {
    String Name
    {
      get;
    }

    UnitType Type
    {
      get;
    }

    bool IsIgnored
    {
      get;
      set;
    }
  }
}
