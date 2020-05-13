///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Data.Definitions.Trace;

namespace ClientTestTool.Data.Events
{
  public class NetworkTraceSetChangedEventArgs : EventArgs
  {
    public NetworkTraceSetChangedEventArgs(NetworkTraceInfo networkTrace)
    {
      NetworkTrace = networkTrace;
    }

    public NetworkTraceInfo NetworkTrace
    {
      get;
      private set;
    }
    
  }
}
