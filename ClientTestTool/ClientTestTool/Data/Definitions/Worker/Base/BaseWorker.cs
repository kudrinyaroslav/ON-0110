///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Enums;

namespace ClientTestTool.Data.Definitions.Worker.Base
{
  public abstract class BaseWorker
  {
    protected BaseWorker(ApplicationState state)
    {
      mRunningState = state;
    }

    public abstract void Run();

    protected readonly ApplicationState mRunningState;
  }
}
