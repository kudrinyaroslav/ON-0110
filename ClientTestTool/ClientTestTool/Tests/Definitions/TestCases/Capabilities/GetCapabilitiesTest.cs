///
/// @Author Matthew Tuusberg
///

﻿using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.Capabilities
{
  [Test(
    Name             = "Get Capabilities",
    Category         = Category.Core,
    Id               = "2",
    FeatureUnderTest = Feature.GetCapabilities
  )]
  public class GetCapabilitiesTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);
      var getServicesList = filteredList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetCapabilities")).ToList();

      AffectedPairs.AddRange(getServicesList);

      BeginStep("Client request contains <GetCapabilities> tag", getServicesList);

      if (!getServicesList.Any())
        throw new TestNotSupportedException("Conversation does not contain requests with <GetCapabilities> tag");

      StepCompleted();

      var responseList = getServicesList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);
      
      if (!responseList.Any())
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(pair => pair.GetResponse<HttpMessage>().ContainsTag("GetCapabilitiesResponse")).ToList();

      BeginStep("Device response contains <GetCapabilitiesResponse> tag", responseList, MessageType.Response);
      
      if (!responseList.Any())
        StepFailed("<GetCapabilitiesResponse> tag is missing");

      StepCompleted();
    }
  }
}

