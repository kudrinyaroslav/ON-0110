///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using ClientTestTool.Data.Global.SingletonInitializer.Attributes;

namespace ClientTestTool.Data.Global.SingletonInitializer
{
  public static class SingletonInitializer
  {
    public static void Initialize()
    {
      // get a list of types which are marked with the InitOnLoadAttribute
      var types = from t in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                  where t.GetCustomAttributes(typeof(InitOnLoadAttribute), false).Any()
                  select t;

      foreach (var type in types)
      {
        var property = type.GetProperties().FirstOrDefault(item => item.PropertyType == type);

        if (null != property)
          property.GetValue(null);
      }
    }
  }
}
