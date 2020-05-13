///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using ClientTestTool.Data.Definitions.Devices.Base;
using ClientTestTool.Data.Definitions.Interfaces;

namespace ClientTestTool.Data.Definitions.Devices
{
  class UnitComparer : IComparer<IUnit>, IEqualityComparer<Unit>
  {
    public int Compare(IUnit x, IUnit y)
    {
      return x.Type.CompareTo(y.Type);
    }

    public bool Equals(Unit x, Unit y)
    {
      return x.Mac == y.Mac;
    }

    public int GetHashCode(Unit obj)
    {
      return obj.Mac.GetHashCode();
    }
  }
}
