///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Definitions.Trace;

namespace ClientTestTool.Data.Definitions.Interfaces
{
  interface ITraceItem
  {
    NetworkTraceInfo FoundInTrace
    {
      get;
    }
  }
}
