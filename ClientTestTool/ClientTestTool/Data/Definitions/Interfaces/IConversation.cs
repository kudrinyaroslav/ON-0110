///
/// @Author Matthew Tuusberg
///

﻿
using ClientTestTool.Data.Definitions.Devices.Base;

namespace ClientTestTool.Data.Definitions.Interfaces
{
  interface IConversation
  {
    Unit Sender
    {
      get;
    }

    Unit Receiver
    {
      get;
    }
  }
}
