///
/// @Author Matthew Tuusberg
///

﻿namespace ClientTestTool.Data.Enums
{
  public enum ApplicationState
  {
    /// <summary>
    /// No time-comsuming operations
    /// </summary>
    Idle,

    /// <summary>
    /// NTParser running
    /// </summary>
    ParserRunning,

    /// <summary>
    /// Tests are being executed
    /// </summary>
    TestRunning,

    /// <summary>
    /// Conformance checking running
    /// </summary>
    ConformanceRunning
  }
}