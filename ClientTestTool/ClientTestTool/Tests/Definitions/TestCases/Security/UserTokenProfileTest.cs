///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Data.Utils.SOAP;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.Security
{
  [Test(
    Name             = "User Token Profile",
    Category         = Category.Core,
    Id               = "1",
    FeatureUnderTest = Feature.UsernameToken
    )]
  public class UserTokenProfileTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      List<RequestResponsePair> filteredList = conversation.GetMessages(ContentType.Http);
      filteredList = filteredList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("Security")).ToList();
      
      AffectedPairs.AddRange(filteredList);

      BeginStep("Client request contains <Security> tag", filteredList);

      if (!filteredList.Any())
        throw new TestNotSupportedException("Conversation does not contain messages with <Security> tag");

      StepCompleted();

      BeginStep("Client request contains <UsernameToken> tag", filteredList);
      FindTagStep("UsernameToken", filteredList);
      StepCompleted();

      BeginStep("Client request contains <Username> tag", filteredList);
      FindTagStep("Username", filteredList);
      StepCompleted();

      BeginStep("Client request contains <Password> tag", filteredList);
      FindTagStep("Password", filteredList);
      StepCompleted();

      BeginStep("Client request contains <Nonce> tag", filteredList);
      FindTagStep("Nonce", filteredList);
      StepCompleted();

      BeginStep("Client request contains <Created> tag", filteredList);
      FindTagStep("Created", filteredList);
      StepCompleted();

      var responseList = filteredList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      BeginStep("Device response does NOT contain <Fault> tag", filteredList, MessageType.Response);

      if (filteredList.All(item => item.GetResponse<HttpMessage>().ContainsTag(SoapOptions.FAULT_TAG)))
          StepFailed("SOAP fault detected");

      StepCompleted();
    }

    private void FindTagStep(String tag, IEnumerable<RequestResponsePair> pairs, MessageType type = MessageType.Request)
    {
      RequestResponsePair pair;

      if (MessageType.Request == type)
        pair = pairs.FirstOrDefault(item => item.GetRequest<HttpMessage>().ContainsTag(tag));
      else
        pair = pairs.FirstOrDefault(item => item.GetResponse<HttpMessage>().ContainsTag(tag));

      if (null == pair)
        StepFailed(String.Format("{0} does not contain <{1}> tag", type, tag));
    }

  }
}