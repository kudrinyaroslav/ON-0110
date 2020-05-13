///
/// @Author Matthew Tuusberg
///

using System;
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

namespace ClientTestTool.Tests.Definitions.TestCases.NTP
{
  [Test(
    Name             = "Set NTP",
    Id               = "2",
    Category         = Category.Core,
    FeatureUnderTest = Feature.SetNTP
  )]
  public class SetNTPTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http).Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetNTP")).ToList();

      AffectedPairs.AddRange(filteredList);

      //S1
      BeginStep("Client request contains <SetNTP> tag", filteredList);

      if (0 == filteredList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <SetNTP> tag");

      StepCompleted();

      filteredList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetNTP", "FromDHCP")).ToList();
      var validFilteredList = filteredList.Where(item =>
      {
        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "FromDHCP");
        return null != value && (value.ToUpper().Contains("TRUE") || value.ToUpper().Contains("FALSE"));
      }).ToList();

      //S2
      BeginStep("<SetNTP> includes tag: <FromDHCP> with “TRUE” OR ”FALSE” value", filteredList);

      if (0 == filteredList.Count)
        StepFailed("<FromDHCP> tag is missing");
      else if (0 == validFilteredList.Count)
        StepFailed("Value of <FromDHCP> tag is invalid");

      StepCompleted();

      var responseList = filteredList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      //S3
      BeginStep("Device response contains HTTP/* 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "SetNTPResponse")).ToList();

      //S4
      BeginStep("Device response contains <SetNTPResponse>", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<SetNTPResponse> tag is missing");

      StepCompleted();

    }
  }
}

