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

namespace ClientTestTool.Tests.Definitions.TestCases.PtzAuxiliaryCommand
{
  [Test(
    Name             = "SendAuxiliaryCommand",
    Id               = "1",
    Category         = Category.ProfileS,
    FeatureUnderTest = Feature.PtzSendAuxiliaryCommand
  )]
  public class PtzSendAuxiliaryCommand : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var sendAuxiliaryCommandList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SendAuxiliaryCommand")).ToList();
      AffectedPairs.AddRange(sendAuxiliaryCommandList);

      //[S1] Client request contains <sendAuxiliaryCommand> tag after the <Body>
      BeginStep("Client request contains <SendAuxiliaryCommand> tag", sendAuxiliaryCommandList);

      if (0 == sendAuxiliaryCommandList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <SendAuxiliaryCommand> tag");

      StepCompleted();

      //[S2] <SendAuxiliaryCommand> includes tag: <﻿ProfileToken> with non-empty string value of specific token
      var profileTokenList = sendAuxiliaryCommandList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SendAuxiliaryCommand", "ProfileToken")).ToList();
      var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<SendAuxiliaryCommand> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      //[S3] <SendAuxiliaryCommand> includes tag: <﻿ProfileToken> with non-empty string value of specific token
      var auxiliaryDataList = sendAuxiliaryCommandList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SendAuxiliaryCommand", "AuxiliaryData")).ToList();
      valuesList = auxiliaryDataList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "AuxiliaryData")).ToList();

      BeginStep("<SendAuxiliaryCommand> includes tag: <AuxiliaryData> with non-empty string value", profileTokenList);

      if (0 == auxiliaryDataList.Count)
        StepFailed("<AuxiliaryData> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <AuxiliaryData> tag is empty");

      StepCompleted();

      //[S4] Device response contains HTTP/* 200 OK
      var responseList = sendAuxiliaryCommandList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      //[S5] Device response contains <SendAuxiliaryCommandResponse> tag.
      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "SendAuxiliaryCommandResponse")).ToList();

      BeginStep("Device response contains <SendAuxiliaryCommandResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<SendAuxiliaryCommandResponse> tag is missing");

      StepCompleted();
    }

  }
}
