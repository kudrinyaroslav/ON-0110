///
/// @Author Matthew Tuusberg
///

﻿using System;

namespace ClientTestTool.Data.Conversations.Events
{
  public class ElementEventArgs : EventArgs
  {
    public ElementEventArgs(int elementIndex)
    {
      ElementIndex = elementIndex;
    }

    public int ElementIndex
    {
      get;
      protected set;
    }
  }
}
