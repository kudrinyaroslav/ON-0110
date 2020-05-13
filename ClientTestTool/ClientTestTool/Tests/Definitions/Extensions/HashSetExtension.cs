///
/// @Author Matthew Tuusberg
///

﻿
using System.Collections.Generic;

namespace ClientTestTool.Tests.Definitions.Extensions
{
  public static class HashSetExtension
  {
    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> values)
    {
      foreach (var value in values)
        set.Add(value);
    }
  }
}
