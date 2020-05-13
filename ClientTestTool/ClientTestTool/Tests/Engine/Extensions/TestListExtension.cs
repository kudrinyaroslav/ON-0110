///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Engine.Extensions
{
  internal static class TestListExtension
  {
    public static List<TestInfo> OrderedList(this IList<TestInfo> testList)
    {
      var features = new[]
      {
        //Core
        Feature.Security,
        Feature.Capabilities,
        Feature.EventHandling,
        Feature.Discovery,
        Feature.NetworkConfiguration,
        Feature.System,
        Feature.UserHandling,
        Feature.RelayOutputs,
        Feature.NTP,
        Feature.DynamicDns,
        Feature.ZeroConfiguration,
        Feature.IPAddressFiltering,
        Feature.PersistentNotificationStorageRetrieval,

        //Profile S
        Feature.MediaStreaming,
        Feature.VideoStreaming,
        Feature.MulticastStreaming,
        Feature.VideoEncoderConfigurations,
        Feature.MediaProfileConfigurations,
        Feature.VideoSourceConfigurations,
        Feature.PtzConfiguration,
        Feature.PtzContinuousPositioning,
        Feature.PtzListing,
        Feature.PtzAbsolutePositioning,
        Feature.PtzRelativePositioning,
        Feature.PtzPresets,
        Feature.PtzHomePosition,
        Feature.PtzAuxiliaryCommand,
        Feature.AudioStreaming,

        //Profle G
        Feature.MediaSearch,
        Feature.ReplayControl,

        //Profile C
        Feature.SystemComponentInformation,
        Feature.SystemComponentState,
        Feature.DoorControl,
        Feature.AccessPointControl,
        Feature.ExternalAuthorization
      }.ToList();

      return testList.OrderBy(item => item.Category)
                     .ThenBy (item => features.IndexOf(item.FeatureUnderTest.GetParentFeature()))
                     .ThenBy (item => item.Id)
                     .ToList ();
    }
  }
}
