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
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.MediaSearch
{
  [Test(
    Name             = "Get Media Attributes",
    Category         = Category.ProfileG,
    Id               = "5",
    FeatureUnderTest = Feature.MediaAttributes
  )]
  public class GetMediaAttributesTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var getMediaAttributesList = conversation.GetMessages(ContentType.Http)
          .Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetMediaAttributes"))
          .ToList();

      AffectedPairs.AddRange(getMediaAttributesList);

      BeginStep("Client request contains <GetMediaAttributes> tag", getMediaAttributesList);

      if (0 == getMediaAttributesList.Count)
        throw new TestNotSupportedException("Conversation does not contain requests with <GetMediaAttributes> tag");

      StepCompleted();

      var timeList = getMediaAttributesList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetMediaAttributes", "Time")).ToList();

      BeginStep("<GetMediaAttributes> includes tag: <Time>", timeList);

      if (0 == timeList.Count)
        StepFailed("<Time> tag is not present");

      StepCompleted();

      var responseList = getMediaAttributesList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response HTTP 200 OK is not present");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpResponse>().ContainsTag("GetMediaAttributesResponse")).ToList();

      BeginStep("Device response contains “<GetMediaAttributesResponse>” tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetMediaAttributesResponse> tag is not present");

      StepCompleted();
    }
  }
}
