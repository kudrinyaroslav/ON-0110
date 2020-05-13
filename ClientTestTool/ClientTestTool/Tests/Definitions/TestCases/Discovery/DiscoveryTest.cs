///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Discovery;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.Discovery
{
  [Test(
    Name             = "WS-Discovery",
    Category         = Category.Core,
    Id               = "1",
    FeatureUnderTest = Feature.WSDiscovery
  )]
  public class DiscoveryTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation) // TODO
    {
      var filteredList = conversation.GetMessages(ContentType.WSDiscovery);

      AffectedPairs.AddRange(filteredList);

      if (0 == filteredList.Count)
        throw new TestNotSupportedException("Conversation does not contain WS-Discovery messages");


      var actionList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<DiscoveryMessage>(), "Action")).ToList();

      BeginStep("Client request contains <Action> tag after the <Header> tag", actionList);

      if (0 == actionList.Count)
        StepFailed("Action tag is missing");

      StepCompleted();

      var validUrlList = actionList.Where(item => TestUtil.ValueOf(item.GetRequest<DiscoveryMessage>(), "Action").EndsWith("Probe")).ToList();

      BeginStep("<Action> includes URL address which ends with Probe value", validUrlList);

      if (0 == validUrlList.Count)
        StepFailed("Valid URL address is missing");

      StepCompleted();

      var messageIdList = validUrlList.Where(item => TestUtil.ContainsTag(item.GetRequest<DiscoveryMessage>(), "MessageID"))
                                      .Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<DiscoveryMessage>(), "MessageID")))
                                      .ToList();

      BeginStep("Client request contains <MessageID> with non-empty string value", messageIdList);

      if (0 == messageIdList.Count)
        StepFailed("MessageID tag is missing");

      StepCompleted();

      var probeList = messageIdList.Where(item => TestUtil.ContainsTag(item.GetRequest<DiscoveryMessage>(), "Probe")).ToList();

      BeginStep("Client request contains <Probe> tag", probeList);

      if (0 == probeList.Count)
        StepFailed("Probe tag is missing");

      StepCompleted();

      var probeMatchesList = probeList.Where(item => TestUtil.ContainsTag(item.GetResponse<DiscoveryMessage>(), "ProbeMatches")).ToList();

      BeginStep("Client response contains <ProbeMatches> tag", probeMatchesList, MessageType.Response);

      if (0 == probeMatchesList.Count)
        StepFailed("ProbeMatches tag is missing");

      StepCompleted();
    }
  }
}
