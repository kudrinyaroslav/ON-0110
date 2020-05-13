///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
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
    Name             = "Get Recording Information",
    Category         = Category.ProfileG,
    Id               = "4",
    FeatureUnderTest = Feature.RecordingInformation
  )]
  public class GetRecordingInformationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var getRecordingInfoList =
        conversation.GetMessages(ContentType.Http)
          .Where(item => item.GetRequest<HttpRequest>().ContainsTag("GetRecordingInformation"))
          .ToList();
      
      AffectedPairs.AddRange(getRecordingInfoList);

      BeginStep("Client request contains <GetRecordingInformation> tag", getRecordingInfoList);

      if (0 == getRecordingInfoList.Count)
        throw new TestNotSupportedException("Conversation does not contain requests with <GetRecordingInformation> tag");

      StepCompleted();

      var recordingTokenList = getRecordingInfoList
        .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetRecordingInformation", "RecordingToken"))
        .Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "RecordingToken"))).ToList();

      BeginStep("<GetRecordingInformation> includes tag <RecordingToken> with non-empty string value of specific token", recordingTokenList);

      if (0 == recordingTokenList.Count)
        StepFailed("<RecordingToken> tag with valid value is not present");

      StepCompleted();

      var responseList = getRecordingInfoList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response HTTP 200 OK is not present");

      StepCompleted();

      responseList = responseList
        .Where(item => item.GetResponse<HttpResponse>().ContainsTag("GetRecordingInformationResponse"))
        .ToList();

      BeginStep("Device response contains <GetRecordingInformationResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetRecordingInformationResponse> tag is not present");

      StepCompleted();
    }
  }
}
