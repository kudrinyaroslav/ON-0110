///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Definitions.Devices;
using ClientTestTool.Data.Definitions.Devices.Base;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Events;
using ClientTestTool.Data.Global.SingletonInitializer.Attributes;

namespace ClientTestTool.Data.Global.Configuration
{
  /// <summary>
  /// CTT Configuration
  /// </summary>
  [InitOnLoad]
  public sealed class CTTConfiguration
  {
    #region Singleton

    private static CTTConfiguration mInstance;

    public static CTTConfiguration Instance
    {
      get
      {
        return mInstance ?? (mInstance = new CTTConfiguration());
      }
    }

    static CTTConfiguration()
    {}

    #endregion

    public static event EventHandler OnConfigurationChanged;

    private CTTConfiguration()
    {
      NetworkTraceSet.OnTraceAdded   += (sender, args) => RaiseOnChangedEvent(ConfigurationChangedType.TraceAdded);
      NetworkTraceSet.OnTraceRemoved += (sender, args) => RaiseOnChangedEvent(ConfigurationChangedType.TraceRemoved);
      BaseUnit.OnIgnoredChanged      += (sender, args) => RaiseOnChangedEvent(ConfigurationChangedType.UnitIgnoredChanged);
      Device.OnFeatureListChanged    += (sender, args) => RaiseOnChangedEvent(ConfigurationChangedType.DeviceFeatureListChanged);
    }

    private void RaiseOnChangedEvent(ConfigurationChangedType type)
    {
      if (null != OnConfigurationChanged)
        OnConfigurationChanged(this, new ConfigurationChangedEventArgs(type));
    }
  }
}
