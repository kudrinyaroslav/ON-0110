///
/// @Author Matthew Tuusberg
///

﻿using System.Linq;
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

// ReSharper disable once CheckNamespace
namespace ClientTestTool.Tests.Definitions.TestCases.SystemTestCases
{
  [Test(
    Name             = "Get Device Information",
    Id               = "1",
    Category         = Category.Core,
    FeatureUnderTest = Feature.GetDeviceInformation
  )]
  public class GetDeviceInformationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http).Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetDeviceInformation")).ToList();

      AffectedPairs.AddRange(filteredList);

      BeginStep("Client request contains <GetDeviceInformation> tag", filteredList);

      if (0 == filteredList.Count)
        throw new TestNotSupportedException("Conversation does not contain requests with <GetDeviceInformation> tag");

      StepCompleted();

      var responseList = filteredList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains HTTP/* 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      BeginStep("Device response contains <GetDeviceInformationResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetDeviceInformationResponse> tag is missing");

      StepCompleted();
    }
  }
}
