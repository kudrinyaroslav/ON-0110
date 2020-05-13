///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Data.Definitions.Interfaces;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Global;
using ClientTestTool.GUI.Enums;

namespace ClientTestTool.Data.Definitions.Devices.Base
{
  public abstract class BaseUnit : IUnit
  {
    public static event EventHandler OnIgnoredChanged;

    protected BaseUnit(NetworkTraceInfo networkTrace)
    {
      mName         = GetUndefinedName();
      FoundInTraces = new HashSet<NetworkTraceInfo> { networkTrace };
      IsIgnored     = false;
    }

    protected BaseUnit(IEnumerable<NetworkTraceInfo> traces)
    {
      mName         = GetUndefinedName();
      FoundInTraces = new HashSet<NetworkTraceInfo>(traces);
      IsIgnored     = false;
    }

    #region Properties

    public UnitType Type
    {
      get;
      protected set; 
    }

    public bool IsIgnored
    {
      get
      {
        return mIsIgnored;
      }
      set
      {
        if (null != OnIgnoredChanged)
          OnIgnoredChanged(this, new EventArgs());

        mIsIgnored = value;
      }
    }

    public String Name
    {
      get
      {
        return mName;
      }
    }

    public HashSet<NetworkTraceInfo> FoundInTraces
    {
      get;
      private set;
    }

#endregion

    #region Helpers

    private String GetUndefinedName()
    {
      int count = UnitSet.GetUnits().Count + 1;
      return String.Join(" ", TAG_UNDEFINED, count);
    }

    private const String TAG_UNDEFINED = "Undefined";

    #endregion

    protected String mName;
    protected bool   mIsIgnored;
  }
}
