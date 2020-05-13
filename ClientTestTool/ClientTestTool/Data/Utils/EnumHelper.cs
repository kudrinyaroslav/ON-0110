///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ClientTestTool.GUI.Enums;

namespace ClientTestTool.Data.Utils
{
  public static class EnumHelper
  {
    /// <summary>
    /// Gets the <see cref="DescriptionAttribute"/> of an <see cref="Enum"/> type value
    /// </summary>
    public static String GetDescription(this Enum value)
    {
      if (null == value)
        throw new ArgumentNullException("value");

      var attribute = value.GetAttribute<EnumDescriptionAttribute>();

      if (null != attribute)
        return attribute.Description;

      return value.ToString();
    }

    public static T GetAttribute<T>(this Enum value) where T : Attribute
    {
      String    fieldName = value.ToString();
      FieldInfo fieldInfo = value.GetType().GetField(fieldName);

      var attribute = fieldInfo.GetCustomAttribute(typeof(T)) as T;

      return attribute;
    }

    public static List<T> GetAttributes<T>(this Enum value) where T : Attribute
    {
      String fieldName = value.ToString();
      FieldInfo fieldInfo = value.GetType().GetField(fieldName);

      var attributes = fieldInfo.GetCustomAttributes(typeof(T)) as IEnumerable<T>;

      return attributes.ToList();
    }
  }
}
