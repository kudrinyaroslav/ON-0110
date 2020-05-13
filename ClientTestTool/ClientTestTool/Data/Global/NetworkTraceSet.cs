///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Events;
using ClientTestTool.GUI.Logging;
using ClientTestTool.GUI.Utils;

namespace ClientTestTool.Data.Global
{
  public sealed class NetworkTraceSet
  {
    #region Singleton

    private static NetworkTraceSet mInstance;

    public static NetworkTraceSet Instance
    {
      get
      {
        return mInstance ?? (mInstance = new NetworkTraceSet());
      }
    }

    static NetworkTraceSet()
    {}

    #endregion

    #region Events

    public static event EventHandler<NetworkTraceSetChangedEventArgs> OnTraceAdded;
    public static event EventHandler<NetworkTraceSetChangedEventArgs> OnTraceRemoved;

    #endregion

    #region Properties

    private readonly List<NetworkTraceInfo> mLoadedTraces;

    #endregion

    #region List Manipulation

    public static List<NetworkTraceInfo> LoadedTraces
    {
      get
      {
        return Instance.mLoadedTraces.ToList();
      }
    }

    public static bool IsEmpty
    {
      get
      {
        return 0 == Instance.mLoadedTraces.Count;
      }
    }

    public static void Add(NetworkTraceInfo item)
    {
      if (null == item)
        throw new ArgumentNullException("item");

      Instance.mLoadedTraces.Add(item);

      Logger.WriteLine(String.Format("Network trace: {0} added", item.Filename));

      if (null != OnTraceAdded)
        OnTraceAdded(Instance, new NetworkTraceSetChangedEventArgs(item));
    }

    public static void Remove(NetworkTraceInfo item)
    {
      if (null == item)
        throw new ArgumentNullException("item");

      Instance.mLoadedTraces.Remove(item);

      Logger.WriteLine(String.Format("Network trace: {0} removed", item.Filename));

      if (null != OnTraceRemoved)
        OnTraceRemoved(Instance, new NetworkTraceSetChangedEventArgs(item));
    }

    public static void RemoveAt(int index)
    {
      if (index < 0 || index >= Instance.mLoadedTraces.Count)
        throw new IndexOutOfRangeException("index");

      var item = Instance.mLoadedTraces[index];
      Instance.mLoadedTraces.RemoveAt(index);

      Logger.WriteLine(String.Format("Network trace: {0} removed", item.Filename));

      if (null != OnTraceRemoved)
        OnTraceRemoved(Instance, new NetworkTraceSetChangedEventArgs(item));
    }

    #endregion

    private NetworkTraceSet()
    {
      mLoadedTraces = new List<NetworkTraceInfo>();
    }
  }
}
