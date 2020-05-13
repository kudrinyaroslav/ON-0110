///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Definitions.Devices;
using ClientTestTool.Data.Definitions.Devices.Base;
using ClientTestTool.Data.Global.SingletonInitializer.Attributes;
using ClientTestTool.Data.Utils;

namespace ClientTestTool.Data.Global.Configuration
{
  /// <summary>
  /// CTT Configuration
  /// </summary>
  [InitOnLoad]
  public sealed class Configuration
  {
    #region Singleton

    private static Configuration mInstance;

    public static Configuration Instance
    {
      get
      {
        return mInstance ?? (mInstance = new Configuration());
      }
    }

    static Configuration()
    {}

    #endregion

    public static event EventHandler OnConfigurationChanged;

    private Configuration()
    {
      NetworkTraceSet.OnTraceAdded   += (sender, args) => RaiseOnChangedEvent();
      NetworkTraceSet.OnTraceRemoved += (sender, args) => RaiseOnChangedEvent();
      BaseUnit.OnIgnoredChanged      += (sender, args) => RaiseOnChangedEvent();
      Device.OnFeatureListChanged    += (sender, args) => RaiseOnChangedEvent();
    }

    private void RaiseOnChangedEvent()
    {
      if (null != OnConfigurationChanged)
        OnConfigurationChanged(this, new EventArgs());
    }
  }
}
