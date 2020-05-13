///
/// @Author Matthew Tuusberg
///

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
    Name             = "Get NTP",
    Id               = "1",
    Category         = Category.Core,
    FeatureUnderTest = Feature.GetNTP
  )]
  public class GetNTPTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http).Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetNTP")).ToList();

      AffectedPairs.AddRange(filteredList);

      //S1
      BeginStep("Client request contains “<GetNTP>” tag", filteredList);

      if (0 == filteredList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GetNTP> tag");

      StepCompleted();

      var responseList = filteredList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      //S2
      BeginStep("Device response contains HTTP/* 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetNTPResponse")).ToList();

      //S3
      BeginStep("Device response contains <GetNTPResponse>", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetNTPResponse> tag is missing");

      StepCompleted();
    }
  }
}

