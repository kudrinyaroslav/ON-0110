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
    Name             = "H264 Replay Recording",
    Category         = Category.ProfileG,
    Id               = "4",
    FeatureUnderTest = Feature.H264ReplayRecording
  )]
  public class H264ReplayRecordingTest : BaseReplayRecordingsTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      ProcessConversation(conversation, "HTTP");
    }
  }
}
