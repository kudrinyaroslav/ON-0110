///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using ClientTestTool.Data.Definitions.Devices.Base;

namespace ClientTestTool.Data.Definitions.Devices.Extensions
{
  public static class UnitExtension
  {
    public static String GetTracesString(this BaseUnit unit)
    {
      return String.Join(", ", unit.FoundInTraces.Select(item => item.Filename));
    }
  }
}
