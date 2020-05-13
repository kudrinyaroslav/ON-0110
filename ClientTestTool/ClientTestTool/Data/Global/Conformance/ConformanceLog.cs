///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Global.Conformance
{
  public class ConformanceLog
  {
    #region Singleton

    private static ConformanceLog mInstance;

    public static ConformanceLog Instance
    {
      get
      {
        return mInstance ?? (mInstance = new ConformanceLog());
      }
    }

    static ConformanceLog()
    {}

    #endregion

    #region Events

    public event EventHandler OnMessageAdded;

    #endregion

    private ConformanceLog()
    {
      Errors   = new Dictionary<Profile, List<String>>();
      Warnings = new Dictionary<Profile, List<String>>();

      Clear();

      Configuration.CTTConfiguration.OnConfigurationChanged += (sender, args) => Clear();
    }

    public Dictionary<Profile, List<String>> Errors
    {
      get;
      private set;
    }

    public Dictionary<Profile, List<String>> Warnings
    {
      get;
      private set;
    }

    public void AddError(Profile profile, String message)
    {
      Errors[profile].Add(message);

      if (null != OnMessageAdded)
        OnMessageAdded(this, new EventArgs());
    }

    public void AddWarning(Profile profile, String message)
    {
      Warnings[profile].Add(message);

      if (null != OnMessageAdded)
        OnMessageAdded(this, new EventArgs());
    }

    public void Clear()
    {
      Errors.Clear();
      Warnings.Clear();

      foreach (Profile profile in Enum.GetValues(typeof(Profile)))
      {
        Errors.  Add(profile, new List<String>());
        Warnings.Add(profile, new List<String>());
      }
    }
  }
}
