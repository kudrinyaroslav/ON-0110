///
/// @Author Matthew Tuusberg
///

using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Devices;
using ClientTestTool.Data.Definitions.Devices.Extensions;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Global;
using ClientTestTool.Tests.Definitions.Data;

namespace ClientTestTool.Data.Utils
{
  public static class TestCaseMapper
  {
    public static Dictionary<Conversation, HashSet<TestInfo>> GetMappedTests()
    {
      var mappedTests = new Dictionary<Conversation, HashSet<TestInfo>>();

      foreach (var conversation in ConversationList.GetConversations())
      {
        var device = conversation.Device as Device;

        if (null == device || !device.IsFeatureListAttached) //TODO think about Conversation w/ 2 Clients
        {
          mappedTests.Add(conversation, new HashSet<TestInfo>(TestCaseSet.Instance.Tests));
          continue;
        }

        var profiles = device.GetSupportedProfiles();

        var profileMandatoryFeatures   = profiles.SelectMany(profile => profile.GetMandatoryFeatures()).ToList();
        var profileConditionalFeatures = profiles.SelectMany(profile => profile.GetConditionalFeatures()).ToList();
        var deviceSpecificFeatures     = device.GetSupportedFeatures();

        var testList = profileMandatoryFeatures.Union(profileConditionalFeatures)
                                               .Union(deviceSpecificFeatures)
                                               .Select(feature => feature.GetDependingTest())
                                               .ToList();

        mappedTests.Add(conversation, new HashSet<TestInfo>(testList));
      }

      return mappedTests;
    }
  }
}