///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.GUI.Enums;

namespace ClientTestTool.Data.Enums
{
  public enum ValidationStatus
  {
    Pending,
    Failed,
    Passed,
    [EnumDescription("Passed (Http Digest)")]
    HttpDigest,
    [EnumDescription("Passed (Rtsp Digest)")]
    RtspDigest
  }
}
