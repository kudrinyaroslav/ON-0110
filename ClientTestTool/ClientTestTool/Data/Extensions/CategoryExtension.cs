///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Utils;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Extensions
{
  public static class CategoryExtension
  {
    public static String GetDisplayName(this Category category)
    {
      return category.GetDescription();
    }

    public static Profile? GetProfile(this Category category)
    {
      Profile? result = null;

      switch (category)
      {
        case Category.ProfileS:
          result = Profile.S;
          break;

        case Category.ProfileG:
          result = Profile.G;
          break;

        case Category.ProfileC:
          result = Profile.C;
          break;
      }

      return result;
    }

    public static List<TestInfo> GetTests(this Category category)
    {
      return TestCaseSet.Instance.Tests.Where(item => item.Category == category).ToList();
    }
  }
}
