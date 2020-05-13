///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Specification.Interfaces;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Utils.Specifications;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.Utils
{
  internal static class ExpectedScenarioHandler
  {
    public static FeatureStatus GetFeatureStatus(Feature feature)
    {
      if (FeatureType.Parent != feature.GetFeatureType())
        throw new ArgumentException("Wrong feature", "feature");

      if (feature.GetChildFeatures().All(item => item.GetInfo().Status == FeatureStatus.Undefined))
        return FeatureStatus.Undefined;

      return GetSpecification(feature).IsSatisfiedBy(feature) ? FeatureStatus.Supported : FeatureStatus.NotSupported;
    }

    private static ISpecification<Feature> GetSpecification(Feature feature)
    {
      switch (feature)
      {
        case Feature.Security:
          return new SecuritySpecification();

        case Feature.Capabilities:
          return new CapabilitiesSpecification();

        case Feature.EventHandling:
          return new EventHandlingSpecification();

        case Feature.Discovery:
          return new DiscoverySpecification();

        case Feature.NetworkConfiguration:
          return new NetworkConfigurationSpecification();

        case Feature.System:
          return new SystemSpecification();

        case Feature.UserHandling:
          return new UserHandlingSpecification();

        case Feature.NTP:
          return new NTPSpecification();

        case Feature.RelayOutputs:
          return new RelayOutputsSpecification();

        case Feature.DynamicDns:
          return new DynamicDnsSpecification();

        case Feature.ZeroConfiguration:
          return new ZeroConfigurationSpecification();

        case Feature.IPAddressFiltering:
          return new IpAddressFilteringSpecification();

        case Feature.PersistentNotificationStorageRetrieval:
          return new PersistentNotificationStorageRetrievalSpecification();

        case Feature.MediaStreaming:
          return new MediaStreamingSpecification();

        case Feature.VideoStreaming:
          return new VideoStreamingSpecification();

        case Feature.MulticastStreaming:
          return new MulticastStreamingSpecification();

        case Feature.VideoEncoderConfigurations:
          return new VideoEncoderConfigurationsSpecification();

        case Feature.MediaProfileConfigurations:
          return new MediaProfileConfigurationsSpecification();

        case Feature.VideoSourceConfigurations:
          return new VideoSourceConfigurationsSpecification();

        case Feature.PtzListing:
          return new PtzListingSpecification();

        case Feature.PtzConfiguration:
          return new PtzConfigurationSpecification();

        case Feature.PtzContinuousPositioning:
          return new PtzContinuousPositioningSpecification();

        case Feature.PtzAbsolutePositioning:
          return new PtzAbsolutePositioningSpecification();

        case Feature.PtzRelativePositioning:
          return new PtzRelativePositioningSpecification();

        case Feature.PtzPresets:
          return new PtzPresetsSpecification();

        case Feature.PtzHomePosition:
          return new PtzHomePositionSpecification();

        case Feature.AudioStreaming:
          return new AudioStreamingSpecification();

        case Feature.MediaSearch:
          return new MediaSearchSpecification();

        case Feature.ReplayControl:
          return new ReplayControlSpecification();

        case Feature.SystemComponentInformation:
          return new SystemComponentInformationSpecification();

        case Feature.SystemComponentState:
          return new SystemComponentStateSpecification();

        case Feature.DoorControl:
          return new DoorControlSpecification();

        case Feature.AccessPointControl:
          return new AccessPointsControlSpecification();

        case Feature.ExternalAuthorization:
          return new ExternalAuthorizationSpecification();

        default:
          throw new ArgumentException("Parent feature must have specification with expected scenario logic", "feature");
      }
    }

  }
}
