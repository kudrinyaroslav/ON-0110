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
    Name             = "MPEG4 Replay Recording",
    Category         = Category.ProfileG,
    Id               = "3",
    FeatureUnderTest = Feature.MPEG4ReplayRecording
  )]
  public class MPEG4ReplayRecordingTest : BaseReplayRecordingsTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      ProcessConversation(conversation, "UDP");
    }
  }
}
