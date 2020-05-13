///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.IO;
using ClientTestTool.GUI.Enums;
using ClientTestTool.GUI.Utils;
using ClientTestTool.Parsers.Interfaces;

namespace ClientTestTool.Data.Definitions.Trace
{
  /// <summary>
  /// Contains information about given Network Trace
  /// </summary>
  public class NetworkTraceInfo
  {
    /// <summary>
    /// ctor
    /// </summary>
    public NetworkTraceInfo(String filename)
    {
      FullName = filename;
      Size     = FileHelper.GetSizeString(filename);
      Status   = NetworkTraceStatus.Stopped;
      Parser   = null;
    }

    #region Properties

    private String mFilename;

    /// <summary>
    /// Network trace file's path
    /// </summary>
    public String FullName
    {
      get
      {
        return mFilename;
      }

      set
      {
        mFilename = value;
      }
    }

    /// <summary>
    /// Filename
    /// </summary>
    public String Filename
    {
      get
      {
        return Path.GetFileName(mFilename);
      }
    }

    /// <summary>
    /// Size of Network Trace File
    /// </summary>
    public String Size
    {
      get;
      private set;
    }

    /// <summary>
    /// Parser binded to this trace
    /// </summary>
    public ITraceParser Parser
    {
      get;
      set;
    }

    /// <summary>
    /// Status
    /// </summary>
    public NetworkTraceStatus Status
    {
      get;
      set;
    }

    #endregion

    public override String ToString()
    {
      return Filename;
    }
  }
}
