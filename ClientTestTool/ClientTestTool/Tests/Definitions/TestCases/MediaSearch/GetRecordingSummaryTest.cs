///
/// @Author Matthew Tuusberg
///

﻿using System.Linq;
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

namespace ClientTestTool.Tests.Definitions.TestCases.MediaSearch
{
  [Test(
   Name             = "Get Recording Summary",
   Category         = Category.ProfileG,
   Id               = "3",
   FeatureUnderTest = Feature.RecordingSummary
 )]
  public class GetRecordingSummaryTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var getRecordingSummaryList =
        conversation.GetMessages(ContentType.Http)
          .Where(item => item.GetRequest<HttpRequest>().ContainsTag("GetRecordingSummary"))
          .ToList();

      AffectedPairs.AddRange(getRecordingSummaryList);

      BeginStep("Client request contains <GetRecordingSummary> tag", getRecordingSummaryList);

      if (0 == getRecordingSummaryList.Count)
        throw new TestNotSupportedException("Conversation does not contain requests with <GetRecordingSummary> tag");

      StepCompleted();

      var responseList = getRecordingSummaryList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response HTTP 200 OK is not present");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpResponse>().ContainsTag("GetRecordingSummaryResponse")).ToList();

      BeginStep("Device response contains “<GetRecordingSummaryResponse>” tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetRecordingSummaryResponse> tag is not present");

      StepCompleted();
    }
  }
}
