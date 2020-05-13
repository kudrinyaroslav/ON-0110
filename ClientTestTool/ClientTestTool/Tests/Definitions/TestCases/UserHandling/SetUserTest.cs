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
    Name             = "Set User",
    Id               = "3",
    Category         = Category.Core,
    FeatureUnderTest = Feature.SetUser
  )]
  public class SetUserTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http).Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetUser")).ToList();

      AffectedPairs.AddRange(filteredList);

      //S1
      BeginStep("Client request contains <SetUser> tag", filteredList);

      if (0 == filteredList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <SetUser> tag");

      StepCompleted();

      var userList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetUser", "User")).ToList();

      //S2
      BeginStep("<SetUser> includes tag: <User>", userList);

      if (0 == userList.Count)
        StepFailed("<User> tag is missing");

      StepCompleted();

      var usernameList = userList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "User", "Username")).ToList();
      var validUsenameList = usernameList.Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Username"))).ToList();

      //S3
      BeginStep("<User> includes tag: <Username> with non-empty string value", usernameList);

      if (0 == usernameList.Count)
        StepFailed("<Username> tag is missing");
      else if (0 == validUsenameList.Count)
        StepFailed("Value of <Username> tag is empty");

      StepCompleted();

      var passwordList = usernameList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "User", "Password")).ToList();
      var validPasswordList = passwordList.Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Password"))).ToList();

      //S4
      BeginStep("<User> includes tag: <Password> with non-empty string value", passwordList);

      if (0 == passwordList.Count)
        StepFailed("<Password> tag is missing");
      else if (0 == validPasswordList.Count)
        StepFailed("Value of <Password> tag is empty");

      StepCompleted();

      var userLevelList = passwordList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "User", "UserLevel")).ToList();
      var validUserLevelList = userLevelList.Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "UserLevel"))).ToList();

      //S5
      BeginStep("<User> includes tag: <UserLevel> with non-empty string value", userLevelList);

      if (0 == userLevelList.Count)
        StepFailed("<UserLevel> tag is missing");
      else if (0 == validUserLevelList.Count)
        StepFailed("Value of <UserLevel> tag is empty");

      StepCompleted();

      var responseList = filteredList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      //S6
      BeginStep("Device response contains HTTP/* 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "SetUserResponse")).ToList();

      //S7
      BeginStep("Device response contains <SetUserResponse>", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<SetUserResponse> tag is missing");

      StepCompleted();
    }
  }
}
