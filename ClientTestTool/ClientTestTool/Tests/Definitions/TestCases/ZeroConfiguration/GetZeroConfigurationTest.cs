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

namespace ClientTestTool.Tests.Definitions.TestCases.ZeroConfiguration
{
  [Test(
    Name             = "Get Zero Configuration",
    Id               = "1",
    Category         = Category.Core,
    FeatureUnderTest = Feature.GetZeroConfiguration
  )]
  public class GetZeroConfigurationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http).Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetZeroConfiguration")).ToList();

      AffectedPairs.AddRange(filteredList);

      //S1
      BeginStep("Client request contains <GetZeroConfiguration> tag", filteredList);

      if (0 == filteredList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GetZeroConfiguration> tag");

      StepCompleted();

      var responseList = filteredList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      //S2
      BeginStep("Device response contains HTTP/* 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetZeroConfigurationResponse")).ToList();

      //S3
      BeginStep("Device response contains <GetZeroConfigurationResponse>", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetZeroConfigurationResponse> tag is missing");

      StepCompleted();

    }
  }
}

