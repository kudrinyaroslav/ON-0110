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

namespace ClientTestTool.Tests.Definitions.TestCases.UserHandling
{
  [Test(
    Name             = "Delete Users",
    Id               = "4",
    Category         = Category.Core,
    FeatureUnderTest = Feature.DeleteUsers
  )]
  public class DeleteUsersTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http).Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "DeleteUsers")).ToList();

      AffectedPairs.AddRange(filteredList);

      //S1
      BeginStep("Client request contains <DeleteUsers> tag", filteredList);

      if (0 == filteredList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <DeleteUsers> tag");

      StepCompleted();

      var userList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "DeleteUsers", "Username")).ToList();

      var usernameList = userList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "DeleteUsers", "Username")).ToList();
      var validUsenameList = usernameList.Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Username"))).ToList();

      //S2
      BeginStep("<DeleteUsers> includes tag: <Username> with non-empty string value", usernameList);

      if (0 == usernameList.Count)
        StepFailed("<Username> tag is missing");
      else if (0 == validUsenameList.Count)
        StepFailed("Value of <Username> tag is empty");

      StepCompleted();

      var responseList = filteredList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      //S3
      BeginStep("Device response contains HTTP/* 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "DeleteUsersResponse")).ToList();

      //S4
      BeginStep("Device response contains <DeleteUsersResponse>", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<DeleteUsersResponse> tag is missing");

      StepCompleted();
    }
  }
}
