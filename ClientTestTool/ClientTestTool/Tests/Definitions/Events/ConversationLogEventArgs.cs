///
/// @Author Matthew Tuusberg
///

﻿using System;
using ClientTestTool.Tests.Definitions.Log.Test;

namespace ClientTestTool.Tests.Definitions.Events
{
  public class ConversationLogEventArgs : EventArgs
  {
    public ConversationLogEventArgs(ConversationLog log)
    {
      Log = log;
    }

    public ConversationLog Log;
  }
}
