///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Definitions.Devices.Base;
using ClientTestTool.Data.Definitions.Devices.Definitions;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.GUI.Enums;

namespace ClientTestTool.Data.Definitions.Devices
{
  public sealed class Device : Unit
  {
    public static event EventHandler OnFeatureListChanged;

    public Device(NetworkTraceInfo networkTrace, String mac, String ip)
      : base(networkTrace, mac, ip)
    {
      Type                 = UnitType.Device;
      mFeatureListFilename = String.Empty;
    }

    public void SetInformation(DeviceInformation info)
    {
      if (null == info)
        throw new ArgumentNullException();

      Info = info;

      mName = Info.Manufacturer + "_" + Info.Model; // Manufacturer + Model
    }

    #region Properties

    private String mFeatureListFilename;

    public String FeatureList
    {
      get
      {
        return mFeatureListFilename;
      }
      set
      {
        if (null == value)
          throw new ArgumentException("value");

        mFeatureListFilename = value;

        if (null != OnFeatureListChanged)
          OnFeatureListChanged(this, new EventArgs());
      }
    }

    public bool IsFeatureListAttached
    {
      get
      {
        return !String.IsNullOrEmpty(mFeatureListFilename);
      }
    }

    #endregion

  }
}
