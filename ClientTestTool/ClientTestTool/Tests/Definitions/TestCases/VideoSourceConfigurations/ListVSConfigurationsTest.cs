///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Conversations;
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

namespace ClientTestTool.Tests.Definitions.TestCases.VideoSourceConfigurations
{
  [Test(
    Name              = "List Video Source Configurations",
    Category          = Category.ProfileS,
    Id                = "1",
    FeatureUnderTest = Feature.GetVideoSourceConfigurations
    )]
  public class ListVSConfigurationsTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      List<RequestResponsePair> filteredList =
        conversation.GetMessages(ContentType.Http);
      filteredList = 
        filteredList.Where(item => 
          item.GetRequest<HttpMessage>().ContainsTag(
            "GetVideoSourceConfigurations")).ToList();

      if (0 == filteredList.Count)
          throw new TestNotSupportedException(
            "Conversation does not contain requests with " +
              "<GetVideoSourceConfigurations> tag");
      AffectedPairs.AddRange(filteredList);
      BeginStep("Client request contains <GetVideoSourceConfigurations> " +
                  "tag", filteredList);
      StepCompleted();

      filteredList = filteredList.Where(item =>
        "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();
      BeginStep("Device response contains \"HTTP/* 200 OK\"", filteredList,
                  MessageType.Response);
      if (filteredList.Count == 0) {
        StepFailed("Response does not contain 200 OK");
      }
      StepCompleted();

      filteredList = filteredList.Where(
                       item => item.GetResponse<HttpMessage>().ContainsTag(
                                 "GetVideoSourceConfigurations" +
                                   "Response")).ToList();
      BeginStep("Device response contains \"" +
                  "<GetVideoSourceConfigurationsResponse> tag",
                  filteredList, MessageType.Response);
      if (filteredList.Count == 0) {
        StepFailed("<GetVideoSourceConfigurationsResponse> tag is missing");
      }
      StepCompleted();
    }
  }
}
