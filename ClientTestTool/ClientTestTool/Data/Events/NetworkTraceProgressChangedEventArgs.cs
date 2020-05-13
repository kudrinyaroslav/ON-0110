///
/// @Author Matthew Tuusberg
///

﻿
namespace ClientTestTool.Data.Events
{
  public class NetworkTraceProgressChangedEventArgs : NetworkTraceSetChangedEventArgs
  {
    public NetworkTraceProgressChangedEventArgs(NetworkTraceInfo networkTrace, int progress) : base(networkTrace)
    {
      Progress = progress;
    }

    public int Progress
    {
      get;
      private set;
    }

  }
}
