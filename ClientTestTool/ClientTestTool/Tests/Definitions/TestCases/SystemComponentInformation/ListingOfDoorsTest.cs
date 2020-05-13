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

namespace ClientTestTool.Tests.Definitions.TestCases.SystemComponentInformation
{
  [Test(
    Name             = "Listing of Doors",
    Category         = Category.ProfileC,
    Id               = "2",
    FeatureUnderTest = Feature.ListingOfDoors
    )]

  public class ListingOfDoorsTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      List<RequestResponsePair> filteredList = conversation.GetMessages(ContentType.Http)
        .Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetDoorInfoList"))
        .ToList();
      AffectedPairs.AddRange(filteredList);

      BeginStep("Client request contains <GetDoorInfoList> tag", filteredList);

      if (0 == filteredList.Count)
        throw new TestNotSupportedException("Conversation does not contain <GetDoorInfoList> messages");

      StepCompleted();

      var responseList = filteredList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("GetDoorInfoListResponse")).ToList();

      BeginStep("Device response contains <GetDoorInfoListResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetDoorInfoListResponse> tag is missing");

      StepCompleted();

      var nextStartRefList =
        conversation.GetMessages(ContentType.Http)
          .Where(item => item.GetResponse<HttpMessage>().ContainsTag("NextStartReference"))
          .Where(item => "401" != item.GetResponse<HttpResponse>().StatusCode)
          .ToList();

      var exceptList = conversation.GetMessages(ContentType.Http).Except(nextStartRefList).ToList();

      BeginStep("At least one Device response in the same Conversation does not contain <NextStartReference> tag", exceptList, MessageType.Response);

      if (0 == exceptList.Count)
        StepFailed("Valid Device response is not found");

      StepCompleted();
     
    }
  }
}
