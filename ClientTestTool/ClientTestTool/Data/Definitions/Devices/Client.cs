///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Data.Definitions.Devices.Base;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.GUI.Enums;

namespace ClientTestTool.Data.Definitions.Devices
{
  public class Client : Unit
  {
    public Client(IEnumerable<NetworkTraceInfo> traces) : base(traces)
    {
      Type = UnitType.Client;

      OtherInformation = String.Empty;
      ProductType      = String.Empty;
    }

    public Client(NetworkTraceInfo networkTrace, String mac, String ip)
      : base(networkTrace, mac, ip)
    {
      Type = UnitType.Client;

      OtherInformation = String.Empty;
      ProductType      = String.Empty;
    }

    #region Properties

    public new String Name
    {
      get
      {
        return mName;
      }

      set
      {
        if (null == value)
          throw new ArgumentNullException();

        mName = value;
      }
    }

    public String Model
    {
      get
      {
        return Info.Model;
      }
      set
      {
        Info.Model = value;
      }
    }

    public String FirmwareVersion
    {
      get
      {
        return Info.FirmwareVersion;
      }
      set
      {
        Info.FirmwareVersion = value;
      }
    }

    public String Brand
    {
      get
      {
        return Info.Manufacturer;
      }
      set
      {
        Info.Manufacturer = value;
      }
    }

    public String OtherInformation
    {
      get;
      set;
    }

    public String ProductType
    {
      get;
      set;
    }

    #endregion
  }
}
