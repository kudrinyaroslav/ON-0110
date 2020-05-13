///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Data.Definitions.Devices.Definitions;
using ClientTestTool.Data.Definitions.Trace;

namespace ClientTestTool.Data.Definitions.Devices.Base
{
  public abstract class Unit : BaseUnit
  {
    protected Unit(IEnumerable<NetworkTraceInfo> traces) : base(traces)
    {
      Mac  = String.Empty;
      Ip   = String.Empty;
      Info = DeviceInformation.Create();
    }

    protected Unit(NetworkTraceInfo networkTrace, String mac, String ip)
            : base(networkTrace)
    {
      Mac  = mac;
      Ip   = ip;
      Info = DeviceInformation.Create();
    }

    #region Properties

    public String Ip
    {
      get;
      protected set;
    }

    public String Mac
    {
      get;
      protected set;
    }

    public DeviceInformation Info
    {
      get;
      protected set;
    }

    #endregion

    #region Helpers

    public override bool Equals(object obj)
    {
      Unit unit = obj as Unit;
      if (null == obj || null == unit || GetType() != obj.GetType())
        return false;

      return Mac == unit.Mac;
    }

    public override int GetHashCode()
    {
      return Mac.GetHashCode();
    }

    #endregion
  }
}
