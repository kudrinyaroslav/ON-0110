///
/// @Author Matthew Tuusberg
///

﻿using System;

namespace ClientTestTool.GUI.Enums
{
  /// <summary>
  /// Provides a description for an enumerated type
  /// </summary>
  [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
  public sealed class EnumDescriptionAttribute : Attribute
  {
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="description"></param>
    public EnumDescriptionAttribute(String description)
    {
      Description = description;
    }

    /// <summary>
    /// Gets the description stored in this attribute
    /// </summary>
    public String Description
    {
      get;
      private set;
    }
  }
}
