///
/// @Author Matthew Tuusberg
///

﻿using System.Linq;
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
    Name             = "Get Nodes",
    Id               = "1",
    Category         = Category.ProfileS,
    FeatureUnderTest = Feature.GetNodes
  )]
  public class GetNodesTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var getNodesList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetNodes")).ToList();
      AffectedPairs.AddRange(getNodesList);

      BeginStep("Client request contains <GetNodes> tag", getNodesList);

      if (0 == getNodesList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GetNodes> tag");

      StepCompleted();

      var responseList = getNodesList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetNodesResponse")).ToList();

      BeginStep("Device response contains <GetNodesResponse>” tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetNodesResponse> tag is absent");

      StepCompleted();
    }
  }
}
