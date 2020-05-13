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
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.EventHandling
{
  [Test(
    Name = "Base notification",
    Category = Category.Core,
    Id = "2",
    FeatureUnderTest = Feature.WSBaseNotification
  )]
  public class BaseNotificationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      List<RequestResponsePair> subscribeList = conversation.GetMessages(ContentType.Http);
      subscribeList = subscribeList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("Subscribe")).ToList();

      AffectedPairs.AddRange(subscribeList);

      BeginStep("Client request contains <Subscribe> tag", subscribeList);

      if (!subscribeList.Any())
        throw new TestNotSupportedException("Conversation does not contain requests with <Subscribe> tag");
    
      StepCompleted();

      var consumerReferenceList =
  subscribeList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Subscribe", "ConsumerReference"))
    .ToList();

      BeginStep("<Subscribe> includes tag: <ConsumerReference>", consumerReferenceList);

      if (!consumerReferenceList.Any())
          StepFailed("<Subscribe> does not include tag: <ConsumerReference>");

      StepCompleted();

      BeginStep("<ConsumerReference> includes tag: <Address>", consumerReferenceList);

      if (!consumerReferenceList.Any(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ConsumerReference", "Address")))
          StepFailed("<ConsumerReference>does not include tag: <Address>");

      StepCompleted();

      var responseList = subscribeList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      BeginStep("Device response contains <SubscribeResponse> tag", responseList, MessageType.Response);

      if (!responseList.Any(item => item.GetResponse<HttpResponse>().ContainsTag("SubscribeResponse")))
          StepFailed("Device response does not contains tag: <SubscribeResponse>");

      StepCompleted();
    }
  }
}
