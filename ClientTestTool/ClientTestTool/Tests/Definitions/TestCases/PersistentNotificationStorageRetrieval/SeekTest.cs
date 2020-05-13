///
/// @Author Matthew Tuusberg
///

using System;
using System.Linq;
using System.ServiceModel.Security;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Base;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.PersistentNotificationStorageRetrieval
{
  [Test(
    Name             = "Seek",
    Id               = "1",
    Category         = Category.Core,
    FeatureUnderTest = Feature.Seek
  )]
  public class SeekTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var pullpointList = conversation.GetMessages(ContentType.Http).Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "CreatePullPointSubscription")).ToList();

      AffectedPairs.AddRange(pullpointList);

      //S1
      BeginStep("Client request contains <CreatePullPointSubscription> tag", pullpointList);

      if (0 == pullpointList.Count)
        throw new TestNotSupportedException("Conversation does not contains messages with <CreatePullPointSubscription> tag");

      StepCompleted();

      var responseList = pullpointList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      //S2
      BeginStep("Device response contains HTTP/* 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "CreatePullPointSubscriptionResponse")).ToList();

      //S3
      BeginStep("Device response contains <CreatePullPointSubscriptionResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<CreatePullPointSubscriptionResponse> tag is missing");

      StepCompleted();


      var seekList = conversation.GetMessages(ContentType.Http).Where(item => item.GetRequest<HttpMessage>().ContainsTag("Seek")).ToList();

      AffectedPairs.AddRange(seekList);

      //S4
      BeginStep("Client request contains “<Seek>” tag", seekList);

      if (0 == seekList.Count)
        throw new TestNotSupportedException("Conversation does not contain message with <Seek> tag");

      StepCompleted();

      var timeList      = seekList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("UtcTime")).ToList();
      var validTimeList = timeList.Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "UtcTime"))).ToList();

      //S5
      BeginStep("Seek>” includes tag: <UtcTime> with non-empty value of date and time", timeList);

      if (0 == timeList.Count)
        StepFailed("");
      else if (0 == validTimeList.Count)
        StepFailed("");

      StepCompleted();

      responseList = seekList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      //S6
      BeginStep("Device response contains HTTP/* 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "SeekResponse")).ToList();

      //S7
      BeginStep("Device response contains <SeekResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<SeekResponse> tag is missing");

      StepCompleted();

      var pullMessagesList = conversation.GetMessages(ContentType.Http)
                                   .Where(item => item.GetRequest<HttpMessage>().ContainsTag("PullMessages"))
                                   .ToList();

      AffectedPairs.AddRange(pullMessagesList);

      //S8
      BeginStep("Client request contains <PullMessages> tag", pullMessagesList);

      if (0 == pullMessagesList.Count)
        StepFailed("Request with <PullMessages> tag is not present");

      StepCompleted();

      //S9
      BeginStep("<PullMessages> includes tag: <Timeout>", pullMessagesList);

      if (!pullMessagesList.Any(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "PullMessages", "Timeout")))
        StepFailed("<Timeout> tag is not present");

      StepCompleted();

      //S10
      BeginStep("<PullMessages> includes tag: <MessageLimit>", pullMessagesList);

      if (!pullMessagesList.Any(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "PullMessages", "MessageLimit")))
        StepFailed("<MessageLimit> tag is not present");

      StepCompleted();

      //S11
      BeginStep("Device response contains \"HTTP/* 200 OK\"", pullMessagesList, MessageType.Response);

      if (pullMessagesList.All(item => "200" != item.GetResponse<HttpResponse>().StatusCode))
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      //S12
      BeginStep("Device response contains <PullMessagesResponse> tag", pullMessagesList, MessageType.Response);

      if (pullMessagesList.All(item => !item.GetResponse<HttpResponse>().ContainsTag("PullMessagesResponse")))
        StepFailed("PullMessagesResponse tag missing");

      StepCompleted();

    }

  }

}

