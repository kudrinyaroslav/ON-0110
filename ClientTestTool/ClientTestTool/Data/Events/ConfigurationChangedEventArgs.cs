///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Enums;

namespace ClientTestTool.Data.Events
{
  public class ConfigurationChangedEventArgs : EventArgs
  {
    public ConfigurationChangedEventArgs(ConfigurationChangedType type)
    {
      ConfigurationChangedType = type;
    }

    public ConfigurationChangedType ConfigurationChangedType
    {
      get;
      private set;
    }
  }
}
