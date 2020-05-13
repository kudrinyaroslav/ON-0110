///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.PtzListing
{
  [Test(
    Name             = "Get Node",
    Id               = "2",
    Category         = Category.ProfileS,
    FeatureUnderTest = Feature.GetNode
  )]
  public class GetNodeTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var getNodeList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetNode")).ToList();
      AffectedPairs.AddRange(getNodeList);

      BeginStep("Client request contains <GetNode> tag", getNodeList);

      if (0 == getNodeList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GetNode> tag");

      StepCompleted();

      var nodeTokenList = getNodeList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetNode", "NodeToken")).ToList();

      var valuesList = nodeTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "NodeToken")).ToList();

      BeginStep("<GetNode> includes tag: <NodeToken> with non-empty string value of specific token", nodeTokenList);

      if (0 == nodeTokenList.Count)
        StepFailed("<NodeToken> tag is absent");
      else if (valuesList.All(item => String.IsNullOrEmpty(item)))
        StepFailed("Value of <NodeToken> tag is empty");

      StepCompleted();

      var responseList = nodeTokenList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetNodeResponse")).ToList();

      BeginStep("Device response contains <GetNodeResponse>” tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetNodeResponse> tag is absent");

      StepCompleted();
    }
  }
}

