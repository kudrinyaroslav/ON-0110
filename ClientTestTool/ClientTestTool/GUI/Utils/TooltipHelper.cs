///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using ClientTestTool.Data.Definitions.Devices;
using ClientTestTool.Data.Definitions.Devices.Extensions;
using ClientTestTool.Data.Extensions;
using ClientTestTool.Data.Utils;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Log.Test;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.GUI.Utils
{
  public static class TooltipHelper
  {
    #region Configuration

    public static String CreateDeviceTooltip(Device device)
    {
      if (null == device)
        return String.Empty;

      if (!device.IsFeatureListAttached)
        return String.Empty;

      return String.Format("Supported Profiles: {0}{1}SupportedFeatures:{1}{2}",
         String.Join(", ", device.GetSupportedProfiles()), Environment.NewLine,
         String.Join(Environment.NewLine, device.GetSupportedFeatures().Select(item => item.GetDisplayName())));
    }

    #endregion 

    #region Diagnostics

    #region Test Trees

    public static String CreateProfileTooltip(Profile profile)
    {
      return profile.IsSupported() ? SUPPORTEDFEATURE : NOTSUPPORTEDFEATURE;
    }

    public static String CreateFeatureTooltip(FeatureInfo feature)
    {
      return feature.Status.GetDescription().ToUpper();
    }

    public static String CreateTestTooltip(TestInfo testInfo, TestResult testResult)
    {
      String testName = testInfo.Name.ToUpper();
      String state = "NOT PERFORMED";

      String requirements = Environment.NewLine;

      requirements = Enum.GetValues(typeof(Profile)).Cast<Profile>()
        .Aggregate(requirements,
          (current, profile) =>
            current + String.Format("Profile {0}: {1}{2}", profile, testInfo.FeatureUnderTest.GetInfo().Requirement[profile],
              Environment.NewLine));

      String conversations = String.Empty;

      if (null != testResult)
      {
        state = testResult.TestStatus.GetDescription().ToUpper();
        conversations = String.Join(Environment.NewLine,
          testResult.Log.ConversationLogs.Select(item => String.Format("{0} : {1}", item.Conversation.Name, item.TestStatus.GetDescription())));
      }

      return String.Format("{0}{1}State: {2}{1}Requirement level:{3}{1}{4}", testName, Environment.NewLine, state, requirements, conversations);
    }

    private const String SUPPORTEDFEATURE    = "SUPPORTED";
    private const String NOTSUPPORTEDFEATURE = "NOT SUPPORTED";
    private const String UNDEFINEDFEATURE    = "UNDEFINED";


    #endregion

    #endregion


  }
}
