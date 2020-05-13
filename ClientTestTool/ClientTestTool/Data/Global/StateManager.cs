///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Enums;

namespace ClientTestTool.Data.Global
{
  public static class StateManager
  {
    private static ApplicationState sCurrentState;

    static StateManager()
    {
      sCurrentState = ApplicationState.Idle;
    }

    public static bool IsInActiveState()
    {
      return ApplicationState.Idle != GetState();
    }

    public static ApplicationState GetState()
    {
      return sCurrentState;
    }

    public static void SetState(ApplicationState state)
    {
      sCurrentState = state;
    }
  }
}
