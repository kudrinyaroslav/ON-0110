///
/// @Author Matthew Tuusberg
///

﻿using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.TestCases.ReplayControl.Base;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.ReplayControl
{
  [Test(
    Name             = "MJPEG Replay Recording",
    Category         = Category.ProfileG,
    Id               = "2",
    FeatureUnderTest = Feature.MJPEGReplayRecording
  )]
  public class MJPEGReplayRecordingTest : BaseReplayRecordingsTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      ProcessConversation(conversation, "RTSP");
    }
  }
}
